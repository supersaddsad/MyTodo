using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Prism.Mvvm;

namespace WpfApp1.Moudls
{
    public class Menuentity: BindableBase
    {
        private string icon;
        public string Icon {
            get => icon;
            set => icon = value;
        }


        private string title;
        public string Title
        {
            get => title;
            set => title = value;
        }


        private string namespaces;
        public string Namespace
        {
            get => namespaces;
            set => namespaces = value;
        }
    }
}
