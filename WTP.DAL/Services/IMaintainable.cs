using System.Collections.Generic;
using System.Threading.Tasks;

namespace WTP.DAL.Services
{
    public interface IMaintainable<T>
    {
        Task<bool> CreateAsync(T obj);
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<bool> UpdateAsync(T obj);
        Task<bool> DeleteAsync(int id);
    }
}
