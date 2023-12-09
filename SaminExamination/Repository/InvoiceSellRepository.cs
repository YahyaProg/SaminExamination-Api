using AutoMapper;
using SaminExamination.Context;
using SaminExamination.Dto;
using SaminExamination.Interfaces;
using SaminExamination.Models;

namespace SaminExamination.Repository
{
    public class InvoiceSellRepository : IInvoiceSellRepository
    {
        public DataContext _context;
        public IMapper _mapper;
        public InvoiceSellRepository(IMapper mapper , DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public InvoiceSell GetSellInvoceById(int invoiceId)
        {
            return _context.invoiceSells.Where(i => i.Id == invoiceId).FirstOrDefault();
        }

        public ICollection<InvoiceSell> GetSellInvoices()
        {
            return _context.invoiceSells.ToList();
        }

        public bool IsExist(int id)
        {
            return _context.invoiceSells.Any(x => x.Id == id);
        }

        public bool NewSellInvoices(int productId, InvoiceSellDto InvoiceSell)
        {
            var mSellInvoice = _mapper.Map<InvoiceSell>(InvoiceSell);
            mSellInvoice.TotalPrice = mSellInvoice.Count * mSellInvoice.Price;
            _context.invoiceSells.Add(mSellInvoice);

            var product = _context.products.Where(c => c.Id == productId).FirstOrDefault();
            product.count -= mSellInvoice.Count;
            return Save();
        }

        public bool EnoughProduct(int productId, InvoiceSellDto InvoiceSell)
        {
            var mSellInvoice = _mapper.Map<InvoiceSell>(InvoiceSell);
            var product = _context.products.Where(c => c.Id == productId).FirstOrDefault();
            if (mSellInvoice.Count > product.count)
                return false;
            return true;
        }

        public bool Save()
        {
           var Save = _context.SaveChanges();   
            return Save > 0 ? true : false;
        }
    }
}
