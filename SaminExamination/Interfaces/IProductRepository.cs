using SaminExamination.Dto;
using SaminExamination.Models;

namespace SaminExamination.Interfaces
{
    public interface IProductRepository
    {
        bool AddProducts(Product products);
        bool UpdateProducts(Product products);

        bool DeleteProducts(Product products);

        ICollection<Product> GetProducts();

        Category GetCategoryByProductId(int id);
        Product GetProductById(int id);

        bool Save();
        bool ProductIsExist(int id);

        ICollection<Product> GetProductsListByCategoryId(int categoryId);
    }
}
