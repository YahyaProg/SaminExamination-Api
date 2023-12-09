using AutoMapper;
using SaminExamination.Context;
using SaminExamination.Dto;
using SaminExamination.Interfaces;
using SaminExamination.Models;

namespace SaminExamination.Repository
{
    public class BuyInvoiceRepository : IBuyInvoicesRepository
    {
        public DataContext _contect;

        public IMapper _mapper;
        public BuyInvoiceRepository(DataContext context , IMapper mapper)
        {
            _contect = context;
          
            _mapper = mapper;
        }

        public bool BuyInvoices( int productId, BuyInvoiceDto buyInvoice)
        {
            var mybuyInvoice = _mapper.Map<BuyInvoice>(buyInvoice);
            mybuyInvoice.TotalPrice = mybuyInvoice.Count * mybuyInvoice.Price;
            _contect.BuyInvoices.Add(mybuyInvoice);
            
            var product = _contect.products.Where(c => c.Id == productId).FirstOrDefault();
      
            
            if (mybuyInvoice.updatePrice == true) {
                product.Price = mybuyInvoice.Price;
                product.count += mybuyInvoice.Count;
                return Save();
            }
            else
            {
                product.count += mybuyInvoice.Count;
                return Save();
            }

     
        }

        public BuyInvoice GetBuyInvoices(int invoiceId)
        {
            var BuyInvoice = _contect.BuyInvoices.Where(b => b.Id == invoiceId).FirstOrDefault();
            return BuyInvoice;
        }

        public ICollection<BuyInvoice> GetBuyInvoices()
        {
            return _contect.BuyInvoices.ToList();
        }
       public BuyInvoice GetBuyInvoicesById(int id)
        {
            return _contect.BuyInvoices.Where(b => b.Id == id).FirstOrDefault();
        }
        public bool IsExist(int invoiceId)
        {
            return _contect.BuyInvoices.Any(b => b.Id == invoiceId);
        }
        public bool Save()
        {
            var save = _contect.SaveChanges();
         return   save > 0 ? true : false;
        }
    }
}
