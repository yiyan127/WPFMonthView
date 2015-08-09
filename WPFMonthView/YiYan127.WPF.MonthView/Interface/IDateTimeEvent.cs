using System;

namespace YiYan127.WPF.MonthView.Interface
{
    /// <summary>
    /// 日期事件的接口
    /// </summary>
    public interface IDateTimeEvent
    {
        /// <summary>
        /// 发生时间
        /// </summary>
        DateTime HappenTime { get; set; }
    }
}