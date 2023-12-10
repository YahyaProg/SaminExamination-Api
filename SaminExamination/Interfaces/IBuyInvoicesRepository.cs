using SaminExamination.Dto;
using SaminExamination.Models;

namespace SaminExamination.Interfaces
{
    public interface IBuyInvoicesRepository
    {
     Task<bool> BuyInvoices( int productId, BuyInvoiceDto buyInvoice , CancellationToken cancellationToken);
      Task<BuyInvoice> GetBuyInvoices(int invoiceId , CancellationToken cancellationToken);
       Task<ICollection<BuyInvoice>> GetBuyInvoices(CancellationToken cancellationToken);
       Task<BuyInvoice> GetBuyInvoicesById(int id , CancellationToken cancellationToken);
      Task<bool> IsExist(int id );
       Task<bool> AsyncSave();
    }
}
