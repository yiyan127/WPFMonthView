/*********************************************************
 * 
 * 创建时间：2014/8/18 23:29:07
 * 描述说明：
 * 
 * 更改历史：
 * 
 * *******************************************************/

#region using

using System.Windows;

#endregion using

namespace YiYan127.WPF.MonthViewTest
{
    /// <summary>
    /// 日记控件
    /// </summary>
    public partial class DiaryWindow
    {
        #region Fields

        /// <summary>
        /// 日记
        /// </summary>
        private Diary _diary;

        #endregion Fields

        #region Properties

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

        public DiaryWindow()
        {
            DataContext = Diary;
            InitializeComponent();
        }

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

        #region Event Handling

        /// <summary>
        /// 删除按钮的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDel_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes ==
                MessageBox.Show("确实删除该日记", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                DiaryCache.Diaries.Remove(Diary);
                Close();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 确定按钮的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOK_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        #endregion 	Event Handling

    }
}
