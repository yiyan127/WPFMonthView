using System.Windows.Controls;

namespace YiYan127.WPF.MonthView.Interface
{
    /// <summary>
    /// 日期事件控件
    /// </summary>
    public interface IDateTimeEventControl
    {
        /// <summary>
        /// 日期事件
        /// </summary>
        IDateTimeEvent DateTimeEvent { get; set; }

        /// <summary>
        /// 控件
        /// </summary>
        Control EventControl { get;  }
    }
}