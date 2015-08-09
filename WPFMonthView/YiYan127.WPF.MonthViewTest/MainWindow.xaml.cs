using System.Windows;
using YiYan127.WPF.MonthView.EventArgs;

namespace YiYan127.WPF.MonthViewTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //事项源
            TestMonthControl.DateTimeEvents = DiaryCache.Diaries;
            //事项界面工厂
            TestMonthControl.DateTimeEventControlFactory=new DiaryBreifControlFactory();
            TestMonthControl.DayBlankDoubleClicked += AddNewDiary;
        }

        /// <summary>
        /// 日期控件双击的处理
        /// </summary>
        /// <param name="e"></param>
        private void AddNewDiary(DayEventArgs e)
        {
            var diary = new Diary
            {
                HappenTime = e.DayTime,
            };
            var wnd = new DiaryWindow
            {
                Diary = diary,
                Title="添加日记",
                Owner = Application.Current.MainWindow
            };
            if (true == wnd.ShowDialog())
            {
                DiaryCache.Diaries.Add(wnd.Diary);
            }
        }
    }
}
