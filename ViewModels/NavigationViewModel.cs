using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using WpfApp1.Events;
using WpfApp1.Extensions;

namespace WpfApp1.ViewModels
{
    public class NavigationViewModel :BindableBase, INavigationAware
    {
        private readonly IContainerProvider containerProvider;
        private readonly IEventAggregator aggregator;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
            aggregator=containerProvider.Resolve<IEventAggregator>();
        }

        protected NavigationViewModel()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public void UpdateLoading(bool loading)
        {
            aggregator.UpdateLoading(new UpdateModel()
            {
                IsOpen = loading
            });

        }
    }
}
