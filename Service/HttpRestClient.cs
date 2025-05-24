using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyToDo.Shared;
using Newtonsoft.Json;
using RestSharp;
using JsonException = Newtonsoft.Json.JsonException;

namespace WpfApp1.Service
{
    public class HttpRestClient
    {
        private readonly string _apiUrl;
        private readonly RestClient _client;
        // 用于存储缓存数据的字典，键为请求的唯一标识，值为包含缓存项和过期时间的元组
        private readonly Dictionary<string, (object CacheItem, DateTime ExpirationTime)> _cache = new Dictionary<string, (object, DateTime)>();
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10); // 缓存过期时间为 10 分钟

        public HttpRestClient(string apiUrl)
        {
            _apiUrl = apiUrl;
            var options = new RestClientOptions(_apiUrl)
            {
                Timeout = TimeSpan.FromSeconds(30),
                ThrowOnAnyError = false
            };
            _client = new RestClient(options);
        }

        // 生成请求的唯一标识
        private string GenerateCacheKey(BaseRequest baseRequest)
        {
            string parameterJson = baseRequest.Parameter != null ? JsonConvert.SerializeObject(baseRequest.Parameter) : "";
            return $"{baseRequest.Method}-{baseRequest.Route}-{parameterJson}";
        }

        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            string cacheKey = GenerateCacheKey(baseRequest);

            // 检查缓存中是否存在该请求的结果且未过期
            if (_cache.TryGetValue(cacheKey, out var cacheEntry) && cacheEntry.ExpirationTime > DateTime.Now)
            {
                return (ApiResponse)cacheEntry.CacheItem;
            }

            try
            {
                var request = new RestRequest(baseRequest.Route, baseRequest.Method);
                request.AddHeader("Content-Type", baseRequest.ContentType);

                // 参数处理
                if (baseRequest.Parameter != null)
                {
                    if (baseRequest.Method == Method.Get)
                    {
                        // GET 请求：将参数对象属性转换为查询参数
                        var properties = baseRequest.Parameter.GetType().GetProperties();
                        foreach (var prop in properties)
                        {
                            var value = prop.GetValue(baseRequest.Parameter)?.ToString();
                            request.AddQueryParameter(prop.Name, value);
                        }
                    }
                    else
                    {
                        // 非 GET 请求：添加 JSON 请求体
                        var properties = baseRequest.Parameter.GetType().GetProperties();
                        foreach (var prop in properties)
                        {
                            var value = prop.GetValue(baseRequest.Parameter)?.ToString();
                            request.AddQueryParameter(prop.Name, value);
                        }
                    }
                }

                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<ApiResponse>(response.Content!);
                    // 将请求结果存入缓存，并设置过期时间
                    _cache[cacheKey] = (result, DateTime.Now.Add(_cacheExpiration));
                    return result;
                }

                return ApiResponse.Fail($"HTTP Error: {(int)response.StatusCode} {response.StatusDescription}");
            }
            catch (JsonException ex)
            {
                return ApiResponse.Fail($"JSON Error: {ex.Message}");
            
            }
        }

        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            string cacheKey = GenerateCacheKey(baseRequest);

            // 检查缓存中是否存在该请求的结果且未过期
            if (_cache.TryGetValue(cacheKey, out var cacheEntry) && cacheEntry.ExpirationTime > DateTime.Now)
            {
                return (ApiResponse<T>)cacheEntry.CacheItem;
            }

            try
            {
                var request = new RestRequest(baseRequest.Route, baseRequest.Method);
                request.AddHeader("Content-Type", baseRequest.ContentType);

                // 参数处理（同上）
                if (baseRequest.Parameter != null)
                {
                    if (baseRequest.Method == Method.Get)
                    {
                        var properties = baseRequest.Parameter.GetType().GetProperties();
                        foreach (var prop in properties)
                        {
                            var value = prop.GetValue(baseRequest.Parameter)?.ToString();
                            request.AddQueryParameter(prop.Name, value);
                        }
                    }
                    else
                    {
                        var properties = baseRequest.Parameter.GetType().GetProperties();
                        foreach (var prop in properties)
                        {
                            var value = prop.GetValue(baseRequest.Parameter)?.ToString();
                            request.AddQueryParameter(prop.Name, value);
                        }
                    }
                }
                
             
                var response = await _client.ExecuteAsync(request);
               
            

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content!);
                    // 将请求结果存入缓存，并设置过期时间
                    _cache[cacheKey] = (result, DateTime.Now.Add(_cacheExpiration));
                    return result;
                }

                return ApiResponse<T>.Fail($"HTTP Error: {(int)response.StatusCode} {response.StatusDescription}");
            }
            catch (JsonException ex)
            {
                return ApiResponse<T>.Fail($"JSON Error: {ex.Message}");
            }
        }
    }

}
