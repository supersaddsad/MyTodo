using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using WpfApp1.Helpers;
using System.Text.Json;

namespace WpfApp1.ViewModels
{
    public class SkinViewModel : BindableBase
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        // 主题设置键名
        private const string THEME_SETTINGS_KEY = "ThemeSettings";

        // 深色/浅色主题
        private bool _isDarkTheme;

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    ModifyTheme(theme => theme.SetBaseTheme(value ? BaseTheme.Dark : BaseTheme.Light));
                    SaveThemeSettings();
                }
            }
        }

        // 颜色调整
        private bool _isColorAdjusted;

        public bool IsColorAdjusted
        {
            get => _isColorAdjusted;
            set
            {
                if (SetProperty(ref _isColorAdjusted, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme)
                        {
                            internalTheme.ColorAdjustment = value
                                ? new ColorAdjustment
                                {
                                    DesiredContrastRatio = DesiredContrastRatio,
                                    Contrast = ContrastValue,
                                    Colors = ColorSelectionValue
                                }
                                : null;
                        }
                    });
                    SaveThemeSettings();
                }
            }
        }

        private float _desiredContrastRatio = 4.5f;

        public float DesiredContrastRatio
        {
            get => _desiredContrastRatio;
            set
            {
                if (SetProperty(ref _desiredContrastRatio, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                            internalTheme.ColorAdjustment.DesiredContrastRatio = value;
                    });
                    SaveThemeSettings();
                }
            }
        }

        public IEnumerable<Contrast> ContrastValues => Enum.GetValues(typeof(Contrast)).Cast<Contrast>();

        private Contrast _contrastValue;

        public Contrast ContrastValue
        {
            get => _contrastValue;
            set
            {
                if (SetProperty(ref _contrastValue, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                            internalTheme.ColorAdjustment.Contrast = value;
                    });
                    SaveThemeSettings();
                }
            }
        }

        public IEnumerable<ColorSelection> ColorSelectionValues =>
            Enum.GetValues(typeof(ColorSelection)).Cast<ColorSelection>();

        private ColorSelection _colorSelectionValue;

        public ColorSelection ColorSelectionValue
        {
            get => _colorSelectionValue;
            set
            {
                if (SetProperty(ref _colorSelectionValue, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                            internalTheme.ColorAdjustment.Colors = value;
                    });
                    SaveThemeSettings();
                }
            }
        }

        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;
        public DelegateCommand<object> ChangeHueCommand { get; private set; }

        public SkinViewModel(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;

            Console.WriteLine("初始化 SkinViewModel...");

            // 加载保存的主题设置
            LoadSavedTheme();

            // 应用主题（确保立即生效）
            ApplyCurrentTheme();

            // 初始化主题变更监听
            if (_paletteHelper.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += (_, e) =>
                {
                    Console.WriteLine($"主题变更事件: {(e.NewTheme?.GetBaseTheme() == BaseTheme.Dark ? "深色" : "浅色")}");
                    IsDarkTheme = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
                    SaveThemeSettings();
                };
            }

            ChangeHueCommand = new DelegateCommand<object>(ChangeHue);
        }

        private void ApplyCurrentTheme()
        {
            Console.WriteLine($"应用当前主题: 深色={IsDarkTheme}, 主色={_paletteHelper.GetTheme().PrimaryMid.Color}");

            ModifyTheme(theme =>
            {
                // 设置明暗主题
                theme.SetBaseTheme(IsDarkTheme ? BaseTheme.Dark : BaseTheme.Light);

                // 应用颜色调整
                if (IsColorAdjusted)
                {
                    theme.ColorAdjustment = new ColorAdjustment
                    {
                        DesiredContrastRatio = DesiredContrastRatio,
                        Contrast = ContrastValue,
                        Colors = ColorSelectionValue
                    };
                }
                else
                {
                    theme.ColorAdjustment = null;
                }
            });
        }

        private void ChangeHue(object? obj)
        {
            if (obj is Color hue)
            {
                Theme theme = _paletteHelper.GetTheme();
                theme.PrimaryLight = new ColorPair(hue.Lighten());
                theme.PrimaryMid = new ColorPair(hue);
                theme.PrimaryDark = new ColorPair(hue.Darken());
                _paletteHelper.SetTheme(theme);
                SaveThemeSettings();
            }
        }

        private static void ModifyTheme(Action<Theme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();
            modificationAction?.Invoke(theme);
            paletteHelper.SetTheme(theme);
        }

        // 保存主题设置到本地存储
        private void SaveThemeSettings()
        {
            try
            {
                var theme = _paletteHelper.GetTheme();
                var themeSettings = new Dictionary<string, object>
                {
                    { nameof(IsDarkTheme), IsDarkTheme },
                    { nameof(IsColorAdjusted), IsColorAdjusted },
                    { nameof(DesiredContrastRatio), DesiredContrastRatio },
                    { nameof(ContrastValue), ContrastValue.ToString() },
                    { nameof(ColorSelectionValue), ColorSelectionValue.ToString() },
                    { "PrimaryColor", theme.PrimaryMid.Color.ToString() }
                };

                // 直接序列化并保存主题设置
                string json = JsonSerializer.Serialize(themeSettings);
                _localStorageService.SaveSetting(THEME_SETTINGS_KEY, json);
                Console.WriteLine($"保存主题设置: {json}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存主题设置失败: {ex.Message}");
            }
        }

        private void LoadSavedTheme()
        {
            try
            {
                string settingsJson = _localStorageService.GetSetting(THEME_SETTINGS_KEY);

                if (!string.IsNullOrEmpty(settingsJson))
                {
                    // 直接反序列化主题设置
                    var themeSettings = JsonSerializer.Deserialize<Dictionary<string, object>>(settingsJson);

                    if (themeSettings != null)
                    {
                        Console.WriteLine("成功加载主题设置");

                        // 恢复明暗主题
                        if (themeSettings.TryGetValue(nameof(IsDarkTheme), out var darkThemeObj) && darkThemeObj is JsonElement darkThemeElement)
                        {
                            if (darkThemeElement.ValueKind == JsonValueKind.True || darkThemeElement.ValueKind == JsonValueKind.False)
                            {
                                IsDarkTheme = darkThemeElement.GetBoolean();
                            }
                        }

                        // 恢复颜色调整设置
                        if (themeSettings.TryGetValue(nameof(IsColorAdjusted), out var colorAdjustedObj) && colorAdjustedObj is JsonElement colorAdjustedElement)
                        {
                            if (colorAdjustedElement.ValueKind == JsonValueKind.True || colorAdjustedElement.ValueKind == JsonValueKind.False)
                            {
                                IsColorAdjusted = colorAdjustedElement.GetBoolean();
                            }
                        }

                        // 恢复对比度比例
                        if (themeSettings.TryGetValue(nameof(DesiredContrastRatio), out var contrastRatioObj) && contrastRatioObj is JsonElement contrastRatioElement)
                        {
                            if (contrastRatioElement.ValueKind == JsonValueKind.Number)
                            {
                                DesiredContrastRatio = (float)contrastRatioElement.GetDouble();
                            }
                        }

                        // 恢复对比度模式
                        if (themeSettings.TryGetValue(nameof(ContrastValue), out var contrastValueObj) && contrastValueObj is JsonElement contrastValueElement)
                        {
                            if (contrastValueElement.ValueKind == JsonValueKind.String)
                            {
                                ContrastValue = Enum.Parse<Contrast>(contrastValueElement.GetString());
                            }
                        }

                        // 恢复颜色选择模式
                        if (themeSettings.TryGetValue(nameof(ColorSelectionValue), out var colorSelectionValueObj) && colorSelectionValueObj is JsonElement colorSelectionValueElement)
                        {
                            if (colorSelectionValueElement.ValueKind == JsonValueKind.String)
                            {
                                ColorSelectionValue = Enum.Parse<ColorSelection>(colorSelectionValueElement.GetString());
                            }
                        }

                        // 恢复主色
                        if (themeSettings.TryGetValue("PrimaryColor", out var primaryColorObj) && primaryColorObj is JsonElement primaryColorElement)
                        {
                            if (primaryColorElement.ValueKind == JsonValueKind.String)
                            {
                                string primaryColorStr = primaryColorElement.GetString();
                                if (ColorConverter.ConvertFromString(primaryColorStr) is Color primaryColor)
                                {
                                    Theme theme = _paletteHelper.GetTheme();
                                    theme.PrimaryLight = new ColorPair(primaryColor.Lighten());
                                    theme.PrimaryMid = new ColorPair(primaryColor);
                                    theme.PrimaryDark = new ColorPair(primaryColor.Darken());
                                    _paletteHelper.SetTheme(theme);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载主题设置失败: {ex.Message}");
            }
        }
    }
}