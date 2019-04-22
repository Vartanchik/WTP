using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WTP.BLL.Services
{
    public interface IMaintainableDto<T>
    {
        Task<bool> CreateAsync(T obj);
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<bool> UpdateAsync(T obj);
        Task<bool> DeleteAsync(int id);
    }
}
