using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DryIoc;
using WpfApp1.Extensions;

namespace WpfApp1.Views
{
    /// <summary>
    /// MainView1.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(IEventAggregator aggregator)
        {
            InitializeComponent();

            aggregator.Resgiter(arg =>
            {
                DialogHost.IsOpen = arg.IsOpen;

                if (DialogHost.IsOpen)
                    DialogHost.DialogContent = new ProgressView();
            });

            btnClose.Click += ((sender, args) =>
            {
                this.Close();
            });

            btnMax.Click += ((sender, args) =>
            {
                this.WindowState = this.WindowState!=WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
            });


            btnMin.Click += ((sender, args) =>
            {
                this.WindowState = this.WindowState != WindowState.Minimized ? WindowState.Minimized : WindowState.Normal;
            });

            ColorZone.MouseDoubleClick += ((sender, args) =>
            {
                this.WindowState = this.WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
            });





        }

    
        
    }
}
