using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Helpers
{
    public interface ILocalStorageService
    {
        void SaveSetting(string key, string value);
        string GetSetting(string key);
    }
}
