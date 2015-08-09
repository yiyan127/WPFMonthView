using YiYan127.WPF.MonthView.Interface;

namespace YiYan127.WPF.MonthViewTest
{
    /// <summary>
    /// 日记概要控件的工厂
    /// </summary>
    public class DiaryBreifControlFactory:IDateTimeEventControlFactory
    {
        public IDateTimeEventControl GetControl(IDateTimeEvent dateTimeEvent)
        {
            var breifControl = new DiaryBreifControl {DateTimeEvent = dateTimeEvent};
            return breifControl;
        }
    }
}