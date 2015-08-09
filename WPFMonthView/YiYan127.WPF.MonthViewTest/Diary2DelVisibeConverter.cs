using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace YiYan127.WPF.MonthViewTest
{
    /// <summary>
    /// 将日记转换成删除可见的转换器
    /// </summary>
    public class Diary2DelVisibeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Diary)
            {
                var diary = value as Diary;
                if (DiaryCache.Diaries.Contains(diary))
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}