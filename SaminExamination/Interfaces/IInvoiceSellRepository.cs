using SaminExamination.Dto;
using SaminExamination.Models;

namespace SaminExamination.Interfaces
{
    public interface IInvoiceSellRepository
    {
       Task<InvoiceSell> GetSellInvoceById  (int invoiceId , CancellationToken cancellationToken);
       Task<ICollection<InvoiceSell>>  GetSellInvoices(CancellationToken cancellationToken);
       Task<bool> NewSellInvoices(int productId, InvoiceSellDto InvoiceSell , CancellationToken cancellationToken);
       Task<bool> EnoughProduct(int productId, InvoiceSellDto InvoiceSell);
       Task<bool>IsExist(int id);
       Task<bool> AsyncSave();
    }
}
