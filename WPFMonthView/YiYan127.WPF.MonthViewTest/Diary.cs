using System;
using YiYan127.WPF.MonthView.Interface;

namespace YiYan127.WPF.MonthViewTest
{
    /// <summary>
    /// 日记
    /// </summary>
    public class Diary : IDateTimeEvent
    {
        /// <summary>
        /// 日记时间
        /// </summary>
        public DateTime HappenTime { get; set; }

        /// <summary>
        /// 天气
        /// </summary>
        public string Weather { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}