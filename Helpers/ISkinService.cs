using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Moudls;

namespace WpfApp1.Helpers
{
    public interface ISkinService
    {
        List<SkinItem> GetAvailableSkins();
        void ApplySkin(SkinItem skin);
    }
}
