using System.Globalization;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using BE_Team7.Models;

namespace BE_Team7.Libraries
{
    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new(new VnPayCompare());

        public Payments GetFullResponseData(IQueryCollection collection, string hashSecret)
        {
            var vnPay = new VnPayLibrary();

            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_") && !string.IsNullOrEmpty(value))
                {
                    vnPay.AddResponseData(key, value);
                }
            }

            // Kiểm tra và chuyển đổi an toàn
            var orderIdString = vnPay.GetResponseData("vnp_TxnRef");
            var transactionNoString = vnPay.GetResponseData("vnp_TransactionNo");
            var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
            var vnpSecureHash = collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value.ToString() ?? string.Empty;
            var orderInfo = vnPay.GetResponseData("vnp_OrderInfo");

            // Xác thực chữ ký
            var checkSignature = vnPay.ValidateSignature(vnpSecureHash, hashSecret);
            if (!checkSignature)
                return new Payments() { Success = false };

            // Chuyển đổi orderIdString sang Guid
            Guid? orderId = Guid.TryParse(orderIdString, out var guidOrderId) ? guidOrderId : null;

            return new Payments()
            {
                Success = vnpResponseCode == "00",
                PaymentMethod = "VnPay",
                OrderDescription = orderInfo,
                OrderId = orderId, // Sử dụng orderId đã chuyển đổi
                PaymentId = Guid.TryParse(transactionNoString, out Guid paymentGuid) && paymentGuid != Guid.Empty
    ? paymentGuid
    : Guid.NewGuid(), TransactionId = transactionNoString,
                Token = vnpSecureHash,
                VnPayResponseCode = vnpResponseCode
            };
        }

        public string GetIpAddress(HttpContext context)
        {
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;
                if (remoteIpAddress == null) return "127.0.0.1";

                if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                        .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                }

                return remoteIpAddress?.ToString() ?? "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                _requestData[key] = value;
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                _responseData[key] = value;
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            var data = new StringBuilder();
            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append($"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(value)}&");
            }

            if (data.Length > 0)
            {
                data.Length--; // Xóa ký tự `&` cuối cùng
            }

            var querystring = data.ToString();
            var vnpSecureHash = HmacSha512(vnpHashSecret, querystring);
            return $"{baseUrl}?{querystring}&vnp_SecureHash={vnpSecureHash}";
        }

        public bool ValidateSignature(string? inputHash, string secretKey)
        {
            if (string.IsNullOrEmpty(inputHash)) return false;

            var rspRaw = GetResponseData();
            var myChecksum = HmacSha512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string HmacSha512(string key, string inputData)
        {
            var hash = new StringBuilder();
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            foreach (var theByte in hashValue)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        private string GetResponseData()
        {
            var data = new StringBuilder();

            var excludedKeys = new HashSet<string> { "vnp_SecureHashType", "vnp_SecureHash" };
            foreach (var (key, value) in _responseData.Where(kv => !excludedKeys.Contains(kv.Key) && !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append($"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(value)}&");
            }

            if (data.Length > 0)
            {
                data.Length--; // Xóa ký tự `&` cuối cùng
            }

            return data.ToString();
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return CompareInfo.GetCompareInfo("en-US").Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
