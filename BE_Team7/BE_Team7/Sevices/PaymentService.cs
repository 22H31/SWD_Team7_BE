﻿using BE_Team7.Interfaces.Service.Contracts;
using BE_Team7.Libraries;
using BE_Team7.Models;

namespace BE_Team7.Sevices
{
    public class PaymentService : IVnPayService
    {
        private readonly IConfiguration _config;
        public PaymentService(IConfiguration config)
        {
            _config = config;
        }
        public string CreatePaymentUrl(PaymentInformation model, HttpContext context, Guid orderId)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_config["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _config["VnPay:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _config["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _config["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _config["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _config["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _config["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);
            pay.AddRequestData("vnp_TxnRef", orderId.ToString()); // Truyền OrderId vào vnp_TxnRef

            var paymentUrl =
                pay.CreateRequestUrl(_config["Vnpay:BaseUrl"], _config["Vnpay:HashSecret"]);

            return paymentUrl;
        }       
        public Payments PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _config["Vnpay:HashSecret"]);
            return response;
        }
    }
}
