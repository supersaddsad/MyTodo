using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation.Regions;
using WpfApp1.DialogHostcs;
using WpfApp1.Extensions;
using WpfApp1.Moudls;

namespace WpfApp1.ViewModels
{
   public class MainViewModel:BindableBase, IConfigureService
    {

        
        public MainViewModel(IRegionManager regionmanager)
        {
            menuBars = new ObservableCollection<Menuentity>();
            NavigateCommand = new DelegateCommand<Menuentity>(Navigate);
            this._regionManager = regionmanager;
        }

        private void Navigate(Menuentity obj)
        {
            if (obj==null || string.IsNullOrWhiteSpace(obj.Namespace))
            {
                return;
            }
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Namespace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });

            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal!=null && journal.CanGoBack)
                {
                    journal.GoBack();
                }

            });

            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward)
                {
                    journal.GoForward();
                }
            });
        }

        public DelegateCommand<Menuentity> NavigateCommand { get; private set; }

       

        private  IRegionNavigationJournal journal;
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }

        private ObservableCollection<Menuentity> menuBars;
        private readonly IRegionManager _regionManager;
        public ObservableCollection<Menuentity> MenuBars
        {
            get => menuBars;
            set
            {
                menuBars = value;RaisePropertyChanged();
            }
        }

        public IRegionManager RegionManager { get; }
     
        void CreateMenu()
        {
            MenuBars.Add(new Menuentity(){Icon = "HomeCircle", Title = "首页",Namespace = "IndexView"});
            MenuBars.Add(new Menuentity(){Icon = "CalendarAlert", Title = "待办",Namespace = "ToDoView"});
            MenuBars.Add(new Menuentity() { Icon = "NoteOutline", Title = "备忘录", Namespace = "MeMoView" });
            MenuBars.Add(new Menuentity() { Icon = "CogOutline", Title = "设置", Namespace = "SettingView" });
        }

        public void Configure()
        {
            CreateMenu();
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
        }
    }
}
