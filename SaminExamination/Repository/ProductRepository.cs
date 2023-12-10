using Microsoft.EntityFrameworkCore;
using SaminExamination.Context;
using SaminExamination.Dto;
using SaminExamination.Helper;
using SaminExamination.Interfaces;
using SaminExamination.Models;

namespace SaminExamination.Repository
{
    public class ProductRepository : IProductRepository
    {
        public DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool>  AddProducts(Product products, CancellationToken cancellationToken)
        {
          await _context.AddAsync(products , cancellationToken);
            return await SaveAsync();
        }

        public async Task<bool> DeleteProducts(Product product , CancellationToken cancellationToken)
        {
          _context.Remove(product);
            return await  SaveAsync();
        }

        public async Task<Product> GetProductById(int id , CancellationToken cancellationToken)
        {
            return await _context.products.Where(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ICollection<Product>> GetProducts(GetProductDto product ,CancellationToken cancellationToken)
        {
           var products = await _context.products.ToListAsync();
            return products.ToPaged(product.PageNumber , product.PageSiz).ToList();
        }

        public async Task<bool> ProductIsExist(int id)
        {
           return await _context.products.AnyAsync(p => p.Id == id);
         
        }

        public async Task<bool> UpdateProducts(Product products, CancellationToken cancellationToken)
        {
            _context.Update(products);
            return await SaveAsync();
        }
        public async Task<Category> GetCategoryByProductId(int Id, CancellationToken cancellationToken)
        {
            return await _context.products.Where(p => p.Id == Id).Select(c => c.Category).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<ICollection<Product>> GetProductsListByCategoryId(int categoryId, CancellationToken cancellationToken)
        {
            return await _context.products.Where(p => p.CategoryId == categoryId).ToListAsync(cancellationToken);
        }
        public async Task<bool> SaveAsync()
        {
            var save =await _context.SaveChangesAsync();
            return save > 0? true : false;
        }
    }
}
