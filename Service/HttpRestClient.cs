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

        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
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
                    return JsonConvert.DeserializeObject<ApiResponse>(response.Content!);
                }

                return  ApiResponse.Fail($"HTTP Error: {(int)response.StatusCode} {response.StatusDescription}");
            }
            catch (JsonException ex)
            {
                return ApiResponse.Fail($"JSON Error: {ex.Message}");
            
            }
        }

        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
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
                    return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content!);
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
