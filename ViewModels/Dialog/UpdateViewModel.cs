using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Shared.Dtos;
using System.Windows;
using WpfApp1.Extensions;
using WpfApp1.IService;

namespace WpfApp1.ViewModels.Dialog
{
    public class UpdateViewModel : BindableBase, INavigationAware
    {
        private readonly IUpdateService _updateService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        private UpdateInfo _updateInfo;
        public UpdateInfo UpdateInfo
        {
            get => _updateInfo;
            set => SetProperty(ref _updateInfo, value);
        }

        private double _downloadProgress;
        public double DownloadProgress
        {
            get => _downloadProgress;
            set => SetProperty(ref _downloadProgress, value);
        }

        private string _statusText;
        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        private bool _isDownloading;
        public bool IsDownloading
        {
            get => _isDownloading;
            set => SetProperty(ref _isDownloading, value);
        }

        public DelegateCommand DownloadCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public UpdateViewModel(IUpdateService updateService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _updateService = updateService;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            DownloadCommand = new DelegateCommand(ExecuteDownloadCommand);
            CancelCommand = new DelegateCommand(ExecuteCancelCommand);

            StatusText = "准备更新...";
        }

        private async void ExecuteDownloadCommand()
        {
            try
            {
                IsDownloading = true;
                StatusText = "开始下载更新...";

                var downloadPath = Path.Combine(Path.GetTempPath(), "WpfApp1Update.zip");
                var progress = new Progress<double>(p => DownloadProgress = p);

                await _updateService.DownloadUpdateAsync(UpdateInfo.DownloadUrl, downloadPath, progress);

                StatusText = "下载完成，准备安装...";
                await Task.Delay(1000);

                _updateService.ApplyUpdate(downloadPath);
            }
            catch (Exception ex)
            {
                StatusText = "更新失败";
                MessageBox.Show($"更新过程中发生错误: {ex.Message}", "更新错误", MessageBoxButton.OK, MessageBoxImage.Error);
                IsDownloading = false;
            }
        }

        private void ExecuteCancelCommand()
        {
            _regionManager.RequestNavigate(PrismManager.MainViewRegionName, "HomeView");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            UpdateInfo = navigationContext.Parameters.GetValue<UpdateInfo>("UpdateInfo");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
