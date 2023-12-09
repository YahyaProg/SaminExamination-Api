using SaminExamination.Context;
using SaminExamination.Dto;
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
        public bool AddProducts(Product products)
        {
           _context.Add(products);
            return Save();
        }

        public bool DeleteProducts(Product product)
        {
            _context.Remove(product);
            return Save();
        }

        public Product GetProductById(int id)
        {
            return _context.products.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Product> GetProducts()
        {
            return _context.products.ToList();
        }

        public bool ProductIsExist(int id)
        {
           return  _context.products.Any(p => p.Id == id);
         
        }

        public bool UpdateProducts(Product products)
        {
            _context.Update(products);
            return Save();
        }
        public Category GetCategoryByProductId(int Id)
        {
            return _context.products.Where(p => p.Id == Id).Select(c => c.Category).FirstOrDefault();
        }
        public ICollection<Product> GetProductsListByCategoryId(int categoryId)
        {
            return _context.products.Where(p => p.CategoryId == categoryId).ToList();
        }
        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0? true : false;
        }
    }
}
