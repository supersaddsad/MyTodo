using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Shared.Contact;
using MyToDo.Shared;
using MyToDo.Shared.Parameter;

namespace WpfApp1.IService
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<ApiResponse<TEntity>> AddAsync(TEntity entity);

        Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity);

        Task<ApiResponse> DeleteAsync(int id);

        Task<ApiResponse<TEntity>> GetFirstOfDefaultAsync(int id);

        Task<ApiResponse<TEntity>> GetAllAsync();

        Task<ApiResponse<PagedList<TEntity>>> GetAllListAsync(QueryParameter parameter);
    }
}
