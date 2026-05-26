using System.Collections.Generic;
using System.Threading.Tasks;
using TH_LTW_Buoi02.Models;

namespace TH_LTW_Buoi02.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}
