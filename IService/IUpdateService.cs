using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Shared.Dtos;

namespace WpfApp1.IService
{
    public interface IUpdateService
    {
        Task<UpdateInfo> CheckForUpdatesAsync();
        Task DownloadUpdateAsync(string downloadUrl, string filePath, IProgress<double> progress);
        void ApplyUpdate(string updateFilePath);
    }
}

