using SaminExamination.Dto;
using SaminExamination.Models;

namespace SaminExamination.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> AddProducts(Product products , CancellationToken cancellationToken);
        Task<bool> UpdateProducts(Product products , CancellationToken cancellationToken);

        Task<bool> DeleteProducts(Product products, CancellationToken cancellationToken);

        Task<ICollection<Product>> GetProducts(GetProductDto product,CancellationToken cancellationToken);

        Task<Category>GetCategoryByProductId(int id , CancellationToken cancellationToken);
        Task<Product> GetProductById(int id , CancellationToken cancellationToken);

        Task<bool> SaveAsync();
       Task<bool> ProductIsExist(int id);

      Task<ICollection<Product>> GetProductsListByCategoryId(int categoryId, CancellationToken cancellationToken);
    }
}
