using System.Collections.Generic;
using System.Threading.Tasks;
using TH_LTW_Buoi02.Models;

namespace TH_LTW_Buoi02.Repositories
{
    public interface IBannerRepository
    {
        Task<IEnumerable<Banner>> GetAllAsync();
        Task<Banner> GetByIdAsync(int id);
        Task AddAsync(Banner banner);
        Task UpdateAsync(Banner banner);
        Task DeleteAsync(int id);
    }
}
