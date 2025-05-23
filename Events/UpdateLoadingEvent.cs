using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Events
{

    public class UpdateModel
    {
        public bool IsOpen { get; set; }

        public double Progress { get; set; }
        public string StatusText { get; set; }
        public string DetailsText { get; set; }

    }

    public class UpdateLoadingEvent : PubSubEvent<UpdateModel>
    {

    }
}
