using System;

namespace YiYan127.WPF.MonthView.EventArgs
{
    /// <summary>
    /// 月份改变的事件参数
    /// </summary>
    public struct MonthChangedEventArgs
    {
        /// <summary>
        /// 旧的月份开始日期
        /// </summary>
        public DateTime OldDisplayStartDate { get; set; }

        /// <summary>
        /// 新的月分开始日期
        /// </summary>
        public DateTime NewDisplayStartDate { get; set; }
    }
}