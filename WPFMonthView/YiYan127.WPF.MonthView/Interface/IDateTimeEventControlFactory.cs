namespace YiYan127.WPF.MonthView.Interface
{
    /// <summary>
    /// 创建日期事件控件的工厂接口
    /// </summary>
    public interface IDateTimeEventControlFactory
    {
        /// <summary>
        /// 根据日期事件创建控件
        /// </summary>
        /// <param name="dateTimeEvent">日期事件</param>
        /// <returns>创建好的控件</returns>
        IDateTimeEventControl GetControl(IDateTimeEvent dateTimeEvent);
    }
}