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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.Views
{
    /// <summary>
    /// ProgressView.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressView : UserControl
    {
        public static readonly DependencyProperty IsIndeterminateProperty =
            DependencyProperty.Register(nameof(IsIndeterminate), typeof(bool), typeof(ProgressView), new PropertyMetadata(true));

        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.Register(nameof(ProgressValue), typeof(double), typeof(ProgressView), new PropertyMetadata(0.0));

        public static readonly DependencyProperty StatusTextProperty =
            DependencyProperty.Register(nameof(StatusText), typeof(string), typeof(ProgressView), new PropertyMetadata("正在加载"));

        public static readonly DependencyProperty DetailsTextProperty =
            DependencyProperty.Register(nameof(DetailsText), typeof(string), typeof(ProgressView), new PropertyMetadata("请稍候..."));

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }

        public double ProgressValue
        {
            get { return (double)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        public string StatusText
        {
            get { return (string)GetValue(StatusTextProperty); }
            set { SetValue(StatusTextProperty, value); }
        }

        public string DetailsText
        {
            get { return (string)GetValue(DetailsTextProperty); }
            set { SetValue(DetailsTextProperty, value); }
        }

        public ProgressView()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}