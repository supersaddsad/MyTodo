using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Prism.Navigation.Regions;
using WpfApp1.Extensions;
using WpfApp1.Moudls;

namespace WpfApp1.ViewModels
{
    public class SettingViewMoudel:BindableBase
    {
        public SettingViewMoudel(IRegionManager regionManager)
        {
            Menuabar = new ObservableCollection<Menuentity>();
            this._regionManager = regionManager;

            NavigateCommand = new DelegateCommand<Menuentity>(Navigate);
            CreateMenu();
           
            
        }



        public DelegateCommand<Menuentity> NavigateCommand { get; private set; }

        private ObservableCollection<Menuentity> menuabar;
        private readonly IRegionManager _regionManager;
        public ObservableCollection<Menuentity> Menuabar
        {
            get => menuabar;
            set
            {
                menuabar = value; RaisePropertyChanged();
            }
        }

        public IRegionManager RegionManager { get; set; }

        private void Navigate(Menuentity obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.Namespace))
            {
                return;
            }
            _regionManager.Regions[PrismManager.SettingViewRegionName].RequestNavigate(obj.Namespace);

        }

        void CreateMenu()
        {
            Menuabar.Add(new Menuentity() { Icon = "HomeCircle", Title = "个性化", Namespace = "SkinView" });
            Menuabar.Add(new Menuentity() { Icon = "CalendarAlert", Title = "系统设置", Namespace = "" });
            Menuabar.Add(new Menuentity() { Icon = "NoteOutline", Title = "关于", Namespace = "AboutView" });
           
        }

    }
}
