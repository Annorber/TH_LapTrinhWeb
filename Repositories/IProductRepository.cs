using System.Collections.Generic;
using System.Threading.Tasks;
using TH_LTW_Buoi02.Models;

namespace TH_LTW_Buoi02.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}
