using BE_Team7.Models;

namespace BE_Team7.Interfaces.Service.Contracts
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformation model, HttpContext context, Guid orderId);
        Payments PaymentExecute(IQueryCollection collections);
    }
}
