using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Shared;
using MyToDo.Shared.Contact;
using MyToDo.Shared.Parameter;
using RestSharp;
using WpfApp1.IService;

namespace WpfApp1.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly HttpRestClient _client;
        private readonly string _serviceName;
        private const string BaseRouteTemplate = "api/{0}/{1}";

        public BaseService(HttpRestClient client, string serviceName)
        {
            _client = client;
            _serviceName = serviceName;
        }

        private BaseRequest CreateRequest(Method method, string action, object parameter = null)
        {
            return new BaseRequest
            {
                Method = method,
                Route = string.Format(BaseRouteTemplate, _serviceName, action),
                Parameter = parameter
            };
        }

        public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
        {
            var request = CreateRequest(Method.Post, "Add", entity);
            return await _client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var request = CreateRequest(Method.Delete, "Delete", new { id });
            return await _client.ExecuteAsync(request);
        }

        public async Task<ApiResponse<TEntity>> GetAllAsync()
        {
            var request = CreateRequest(Method.Get, "GetAll");
            return await _client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>> GetFirstOfDefaultAsync(int id)
        {
            var request = CreateRequest(Method.Get, "GetById", new { id });
            return await _client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            var request = CreateRequest(Method.Post, "Update", entity);
            return await _client.ExecuteAsync<TEntity>(request);
        }

    

        public async Task<ApiResponse<PagedList<TEntity>>> GetAllListAsync(MyToDo.Shared.Parameter.QueryParameter parameter)
        {
            var request = CreateRequest(Method.Get, "GetAllpageList", parameter);
            return await _client.ExecuteAsync<PagedList<TEntity>>(request);
        }
    }
}
