using System;
using System.Collections.ObjectModel;
using YiYan127.WPF.MonthView.Interface;

namespace YiYan127.WPF.MonthViewTest
{
    public static class DiaryCache
    {
        /// <summary>
        /// 所有日记
        /// </summary>
        private static readonly ObservableCollection<IDateTimeEvent> InnerDiaries = new ObservableCollection<IDateTimeEvent>();

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static DiaryCache()
        {
            var contact = new Diary
            {
                HappenTime = DateTime.Now,
                Weather = "阴",
                Title = "欢迎",
                Content = "欢迎下载本程序,如有问题,可发送电子邮件至yiyan127@sina.com"
            };
            InnerDiaries.Add(contact);
        }

        /// <summary>
        /// 所有日记
        /// </summary>
        public static ObservableCollection<IDateTimeEvent> Diaries
        {
            get { return InnerDiaries; }
        }
    }
}