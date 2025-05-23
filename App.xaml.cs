using System.Windows;
using WpfApp1.Views;
using WpfApp1.ViewModels;
using WpfApp1.Service;
using DryIoc;
using System.ComponentModel;
using Prism.Ioc;
using Prism.DryIoc;
using Prism.Ioc;
using WpfApp1.IService;
using WpfApp1.Helpers;
using WpfApp1.Views.Dialogs;
using WpfApp1.ViewModels.Dialog;
using WpfApp1.DialogHostcs;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {

        protected override Window CreateShell()
        {
            //

            return Container.Resolve<MainView>();
           
        }


        protected override void OnInitialized()
        {
            Container.Resolve<SkinViewModel>();
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure();
            base.OnInitialized();
            base.OnInitialized();
        }

          public static void LoginOut(IContainerProvider containerProvider)
        {
            Current.MainWindow.Hide();

            var dialog = containerProvider.Resolve<IDialogService>();

            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                } 

                Current.MainWindow.Show();
            });
        }



        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"
                ));


            containerRegistry.GetContainer().RegisterInstance(@"http://1.95.147.41:80/", serviceKey: "webUrl");

            containerRegistry.RegisterSingleton<ISkinService, SkinService>();

            // 注册本地存储服务
           

            containerRegistry.Register<IToDoService, ToDoService>();
            containerRegistry.Register<IMeMoService, MeMoService>();
            containerRegistry.RegisterForNavigation<AddToDoView, AddToDoViewModel>();
            containerRegistry.RegisterForNavigation<AddMemoView, AdddMemoViewModel>();
            containerRegistry.RegisterForNavigation<IndexView,IndexViewModel>();
            containerRegistry.RegisterForNavigation<AboutView, AboutViewMoudel>();
            containerRegistry.RegisterForNavigation<ToDoView, ToDoViewMoudel>();
            containerRegistry.RegisterForNavigation<MeMoView, MeMoViewMoudel>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
            containerRegistry.RegisterForNavigation<SettingView, SettingViewMoudel>();
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.RegisterSingleton<ILocalStorageService, LocalStorageService>();
        }
    }

}
