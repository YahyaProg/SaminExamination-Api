using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        public async Task<InvoiceSell>  GetSellInvoceById(int invoiceId, CancellationToken cancellationToken)
        {
            return await _context.invoiceSells.Where(i => i.Id == invoiceId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<InvoiceSell>> GetSellInvoices(CancellationToken cancellationToken)
        {
            return await _context.invoiceSells.ToListAsync(cancellationToken);
        }

        public async Task<bool> IsExist(int id)
        {
            return await _context.invoiceSells.AnyAsync(x => x.Id == id);
        }

        public async Task<bool>  NewSellInvoices(int productId, InvoiceSellDto InvoiceSell , CancellationToken cancellationToken)
        {
            var mSellInvoice = _mapper.Map<InvoiceSell>(InvoiceSell);
            mSellInvoice.TotalPrice = mSellInvoice.Count * mSellInvoice.Price;
           await _context.invoiceSells.AddAsync(mSellInvoice);

            var product =await _context.products.Where(c => c.Id == productId).FirstOrDefaultAsync();
            product!.count -= mSellInvoice.Count;
            return await AsyncSave();
        }

        public async Task<bool> EnoughProduct(int productId, InvoiceSellDto InvoiceSell)
        {
            var mSellInvoice = _mapper.Map<InvoiceSell>(InvoiceSell);
            var product =await _context.products.Where(c => c.Id == productId).FirstOrDefaultAsync();
            if (mSellInvoice.Count > product?.count)
                return false;
            return true;
        }

        public async Task<bool> AsyncSave()
        {
           var Save = await _context.SaveChangesAsync();   
            return Save > 0 ? true : false;
        }
    }
}
