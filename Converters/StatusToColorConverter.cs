using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

    namespace WpfApp1.Converters;


public class StatusToColorConverter : DependencyObject, IValueConverter
{
    public static readonly DependencyProperty IncompleteColorProperty =
        DependencyProperty.Register("IncompleteColor", typeof(Brush),
            typeof(StatusToColorConverter), new PropertyMetadata(Brushes.Green));

    public static readonly DependencyProperty CompleteColorProperty =
        DependencyProperty.Register("CompleteColor", typeof(Brush),
            typeof(StatusToColorConverter), new PropertyMetadata(Brushes.Blue));

    public static readonly DependencyProperty DefaultColorProperty =
        DependencyProperty.Register("DefaultColor", typeof(Brush),
            typeof(StatusToColorConverter), new PropertyMetadata(Brushes.Transparent));

    public Brush IncompleteColor
    {
        get => (Brush)GetValue(IncompleteColorProperty);
        set => SetValue(IncompleteColorProperty, value);
    }

    public Brush CompleteColor
    {
        get => (Brush)GetValue(CompleteColorProperty);
        set => SetValue(CompleteColorProperty, value);
    }

    public Brush DefaultColor
    {
        get => (Brush)GetValue(DefaultColorProperty);
        set => SetValue(DefaultColorProperty, value);
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            0 => IncompleteColor,    // 未完成
            1 => CompleteColor,      // 已完成
            _ => DefaultColor
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// 状态文本转换器
public class StatusTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value switch
        {
            0 => "待完成",
            1 => "已完成",
            _ => "未知状态"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

// 状态图标转换器
public class StatusIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value switch
        {
            0 => PackIconKind.ClockOutline,
            1 => PackIconKind.CheckCircleOutline,
            _ => PackIconKind.AlertCircleOutline,
           
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

