using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using MyToDo.Shared.Dtos;
using System.Windows;
using RestSharp;
using WpfApp1.Events;
using WpfApp1.IService;

namespace WpfApp1.Service
{
    public class UpdateService : IUpdateService
    {
        private readonly HttpRestClient _httpClient;
        private readonly IEventAggregator _eventAggregator;
        private readonly string _updateInfoUrl;

        public UpdateService(HttpRestClient httpClient, IEventAggregator eventAggregator)
        {
            _httpClient = httpClient;
            _eventAggregator = eventAggregator;
            _updateInfoUrl = "api/Update/GetLatestVersion"; // 调整为相对路径
        }

        public async Task<UpdateInfo> CheckForUpdatesAsync()
        {
            try
            {
                var request = new BaseRequest
                {
                    Method = Method.Get,
                    Route = _updateInfoUrl
                };

                var response = await _httpClient.ExecuteAsync<UpdateInfo>(request);

                if (response.IsSuccess)
                {
                    return response.Result;
                }
                else
                {
                  
                    return null;
                }
            }
            catch (Exception ex)
            {
               
                return null;
            }
        }

        public async Task DownloadUpdateAsync(string downloadUrl, string filePath, IProgress<double> progress)
        {
            // 增加参数检查
            if (string.IsNullOrEmpty(downloadUrl))
                throw new ArgumentNullException(nameof(downloadUrl), "下载URL不能为空");

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath), "文件路径不能为空");

            try
            {
                var client = new RestClient();
                var restRequest = new RestRequest(downloadUrl, Method.Get);

                // 执行请求
                var response = await client.ExecuteAsync(restRequest);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"下载更新失败：HTTP Error: {(int)response.StatusCode} {response.StatusDescription}", "更新错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var totalBytes = response.ContentLength.GetValueOrDefault();

                using (var contentStream = new MemoryStream(response.RawBytes))
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    var buffer = new byte[8192];
                    var bytesRead = 0;
                    var totalBytesRead = 0L;

                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        totalBytesRead += bytesRead;

                        var percentage = (double)totalBytesRead / totalBytes * 100;
                        progress?.Report(percentage);

                        // 更新UI显示
                        _eventAggregator.GetEvent<UpdateLoadingEvent>().Publish(new UpdateModel
                        {
                            IsOpen = true,
                            Progress = percentage,
                            StatusText = "正在下载更新",
                            DetailsText = $"已下载 {FormatBytes(totalBytesRead)} / {FormatBytes(totalBytes)}"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录更详细的错误信息
                MessageBox.Show($"下载更新失败：{ex.Message}", "更新错误", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        public void ApplyUpdate(string updateFilePath)
        {
            // 增加参数检查
            if (string.IsNullOrEmpty(updateFilePath))
                throw new ArgumentNullException(nameof(updateFilePath), "更新文件路径不能为空");

            if (!File.Exists(updateFilePath))
                throw new FileNotFoundException("更新文件不存在", updateFilePath);

            try
            {
                // 创建批处理脚本执行更新
                var updateScriptPath = Path.Combine(Path.GetTempPath(), "update.bat");
                File.WriteAllText(updateScriptPath, $@"
@echo off
timeout /t 3 /nobreak
taskkill /f /im WpfApp1.exe
echo 删除旧文件...
del /q /f ""%~dp0\*.*""
echo 解压更新文件...
tar -xf ""{updateFilePath}"" -C ""%~dp0""
echo 启动应用...
start """" ""%~dp0\WpfApp1.exe""
del /q /f ""%~f0""
");

                // 启动批处理脚本
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"{updateScriptPath}\"",
                    WindowStyle = ProcessWindowStyle.Hidden
                });

                // 关闭当前应用
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"应用更新失败：{ex.Message}", "更新错误", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private string FormatBytes(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < suffixes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:0.##} {suffixes[order]}";
        }
    }
}