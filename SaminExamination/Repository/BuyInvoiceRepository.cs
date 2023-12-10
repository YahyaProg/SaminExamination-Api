using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool>  BuyInvoices( int productId, BuyInvoiceDto buyInvoice , CancellationToken cancellationToken)
        {
            var mybuyInvoice = _mapper.Map<BuyInvoice>(buyInvoice);
            mybuyInvoice.TotalPrice = mybuyInvoice.Count * mybuyInvoice.Price;
            await _contect.BuyInvoices.AddAsync(mybuyInvoice);
            
            var product =await _contect.products.Where(c => c.Id == productId).FirstOrDefaultAsync();
      
            
            if (mybuyInvoice.updatePrice == true) {
                product!.Price = mybuyInvoice.Price;
                product.count += mybuyInvoice.Count;
                return await AsyncSave();
            }
            else
            {
                product!.count += mybuyInvoice.Count;
                return await AsyncSave();
            }

     
        }

        public async Task<BuyInvoice>  GetBuyInvoices(int invoiceId, CancellationToken cancellationToken)
        {
            var BuyInvoice =await _contect.BuyInvoices.Where(b => b.Id == invoiceId).FirstOrDefaultAsync(cancellationToken);
            return BuyInvoice;
        }

        public async Task<ICollection<BuyInvoice>>  GetBuyInvoices(CancellationToken cancellationToken)
        {
            return await _contect.BuyInvoices.ToListAsync(cancellationToken);
        }
       public async Task<BuyInvoice> GetBuyInvoicesById(int id , CancellationToken cancellationToken)
        {
            return await _contect.BuyInvoices.Where(b => b.Id == id).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<bool> IsExist(int invoiceId)
        {
            return await _contect.BuyInvoices.AnyAsync(b => b.Id == invoiceId);
        }
        public async Task<bool> AsyncSave()
        {
            var save =await _contect.SaveChangesAsync();
         return   save > 0 ? true : false;
        }
    }
}
