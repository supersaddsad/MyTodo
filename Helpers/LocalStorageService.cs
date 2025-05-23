using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WpfApp1.Helpers
{
    public class  LocalStorageService : ILocalStorageService
    {
        private readonly string _settingsFilePath;

        public LocalStorageService()
        {
            // 获取应用程序数据文件夹路径
            string appDataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            string appFolderPath = Path.Combine(appDataPath, "WpfApp1");

            // 确保文件夹存在
            if (!Directory.Exists(appFolderPath))
            {
                Directory.CreateDirectory(appFolderPath);
            }

            // 设置文件路径 (使用.json扩展名更合适)
            _settingsFilePath = Path.Combine(appFolderPath, "Settings.json");
        }

        public void SaveSetting(string key, string value)
        {
            try
            {
                // 加载现有的设置
                Dictionary<string, string> settings = LoadSettings();

                // 添加或更新设置
                settings[key] = value;

                // 保存设置
                SaveSettings(settings);
            }
            catch (Exception ex)
            {
                throw new Exception($"保存设置失败: {ex.Message}", ex);
            }
        }

        public string GetSetting(string key)
        {
            try
            {
                // 加载现有的设置
                Dictionary<string, string> settings = LoadSettings();

                // 获取设置值
                if (settings.TryGetValue(key, out string value))
                {
                    return value;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取设置失败: {ex.Message}", ex);
            }
        }

        private Dictionary<string, string> LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string json = File.ReadAllText(_settingsFilePath);
                    return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                }

                return new Dictionary<string, string>();
            }
            catch
            {
                // 如果加载失败，返回空字典
                return new Dictionary<string, string>();
            }
        }

        private void SaveSettings(Dictionary<string, string> settings)
        {
            try
            {
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_settingsFilePath, json);
            }
            catch
            {
                // 处理保存失败的情况
            }
        }
    }
}
