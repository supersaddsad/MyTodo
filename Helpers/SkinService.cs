using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using WpfApp1.Moudls;

namespace WpfApp1.Helpers
{
    public class SkinService:ISkinService
    {
        public List<SkinItem> GetAvailableSkins()
        {
            return new List<SkinItem>
            {
                new SkinItem
                {
                    Name = "默认皮肤",
                    PrimaryColor = "#0288d1",
                    SecondaryColor = "#009688",
                    PreviewImage = "/Resources/Skins/Default.png"
                },
                new SkinItem
                {
                    Name = "深紫色",
                    PrimaryColor = "#7B1FA2",
                    SecondaryColor = "#FFC107",
                    PreviewImage = "/Resources/Skins/DeepPurple.png"
                },
                new SkinItem
                {
                    Name = "蓝色",
                    PrimaryColor = "#1976D2",
                    SecondaryColor = "#4CAF50",
                    PreviewImage = "/Resources/Skins/Blue.png"
                },
                new SkinItem
                {
                    Name = "绿色",
                    PrimaryColor = "#388E3C",
                    SecondaryColor = "#FF9800",
                    PreviewImage = "/Resources/Skins/Green.png"
                },
                new SkinItem
                {
                    Name = "红色",
                    PrimaryColor = "#D32F2F",
                    SecondaryColor = "#FFEB3B",
                    PreviewImage = "/Resources/Skins/Red.png"
                }
            };
        }

        public void ApplySkin(SkinItem skin)
        {
            if (skin == null) return;

            try
            {
                // 应用主色调
                Application.Current.Resources["PrimaryHueMidBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(skin.PrimaryColor));

                // 应用辅助色
                Application.Current.Resources["SecondaryAccentBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(skin.SecondaryColor));

                Console.WriteLine($"皮肤已应用: {skin.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"应用皮肤失败: {ex.Message}");
            }
        }
    }
}
