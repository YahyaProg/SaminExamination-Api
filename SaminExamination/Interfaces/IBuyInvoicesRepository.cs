using SaminExamination.Dto;
using SaminExamination.Models;

namespace SaminExamination.Interfaces
{
    public interface IBuyInvoicesRepository
    {
      bool BuyInvoices( int productId, BuyInvoiceDto buyInvoice);
       BuyInvoice GetBuyInvoices(int invoiceId);
        ICollection<BuyInvoice> GetBuyInvoices();
        BuyInvoice GetBuyInvoicesById(int id);
        bool IsExist(int id);
        bool Save();
    }
}
