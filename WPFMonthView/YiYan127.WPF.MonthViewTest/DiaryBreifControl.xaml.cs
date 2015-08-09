/*********************************************************
 * 创建时间：2014/8/18 23:43:18
 * 描述说明：
 * 
 * 更改历史：
 * 
 * *******************************************************/

#region using

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YiYan127.WPF.MonthView.Interface;

#endregion using

namespace YiYan127.WPF.MonthViewTest
{
    /// <summary>
    /// 日记概要控件
    /// </summary>
    public partial class DiaryBreifControl : IDateTimeEventControl
    {
        #region Fields

        /// <summary>
        /// 日记
        /// </summary>
        private Diary _diary;

        #endregion Fields

        #region Properties

        public IDateTimeEvent DateTimeEvent
        {
            get { return Diary; }
            set { Diary = value as Diary; }
        }

        public Control EventControl
        {
            get { return this; }
        }

        /// <summary>
        /// 日记
        /// </summary>
        public Diary Diary
        {
            get { return _diary; }
            set
            {
                _diary = value;
                DataContext = null;
                DataContext = value;
            }
        }

        #endregion Properties

        #region Delegates

        #endregion Delegates

        #region Public Methods

        public DiaryBreifControl()
        {
            DataContext = Diary;
            InitializeComponent();
            MouseDoubleClick += DiaryBreifControl_MouseDoubleClick;
        }

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

        #region Event Handling

        /// <summary>
        /// 控件双击的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiaryBreifControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var wnd = new DiaryWindow
            {
                Diary = Diary,
                Owner = Application.Current.MainWindow,
                Title="修改日记",
            };
            wnd.ShowDialog();
        }

        #endregion 	Event Handling
    }
}
