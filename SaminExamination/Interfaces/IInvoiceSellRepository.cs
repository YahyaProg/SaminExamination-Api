using SaminExamination.Dto;
using SaminExamination.Models;

namespace SaminExamination.Interfaces
{
    public interface IInvoiceSellRepository
    {
        Models.InvoiceSell GetSellInvoceById  (int invoiceId);
        ICollection<Models.InvoiceSell> GetSellInvoices();
        bool NewSellInvoices(int productId, InvoiceSellDto InvoiceSell);
        bool EnoughProduct(int productId, InvoiceSellDto InvoiceSell);
        bool IsExist(int id);
        bool Save();
    }
}
