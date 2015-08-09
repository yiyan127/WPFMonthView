/*********************************************************
 * 
 * 创建时间：2014/7/28 10:43:32
 * 描述说明：
 * 
 * 更改历史：
 * 
 * *******************************************************/

#region using

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using YiYan127.WPF.MonthView.EventArgs;
using YiYan127.WPF.MonthView.Interface;
using Calendar = System.Globalization.Calendar;

#endregion using

namespace YiYan127.WPF.MonthView.Controls
{
    /// <summary>
    /// 月控件
    /// </summary>
    public partial class MonthControl
    {
        #region Fields

        /// <summary>
        /// 展示的开始日期,为当前月的1号
        /// </summary>
        private DateTime _displayStartDate = DateTime.Now.AddDays(-DateTime.Now.Day + 1);

        /// <summary>
        /// 展示的月
        /// </summary>
        private int _displayMonth;

        /// <summary>
        /// 展示的年
        /// </summary>
        private int _displayYear;

        /// <summary>
        /// 当前区域的时间表示
        /// </summary>
        private readonly Calendar _cultureCalendar = CultureInfo.CurrentUICulture.Calendar;

        /// <summary>
        /// 日期事件列表
        /// </summary>
        private ObservableCollection<IDateTimeEvent> _dateTimeEvents = new ObservableCollection<IDateTimeEvent>();

        /// <summary>
        /// 起始时间的星期数
        /// </summary>
        private int _startDayOffset;

        #endregion Fields

        #region Delegates

        /// <summary>
        /// 月份改变的处理
        /// </summary>
        /// <param name="e">月份改变的事件参数</param>
        public delegate void DisplayMonthChangedEventHandler(MonthChangedEventArgs e);

        /// <summary>
        /// 月份改变的事件
        /// </summary>
        public event DisplayMonthChangedEventHandler DisplayMonthChanged;

        /// <summary>
        /// 天控件双击的处理
        /// </summary>
        /// <param name="e"></param>
        public delegate void DayBoxDoubleClickedEventHandler(DayEventArgs e);

        /// <summary>
        /// 在日期空白处双击的事件
        /// </summary>
        public event DayBoxDoubleClickedEventHandler DayBlankDoubleClicked;

        #endregion Delegates

        #region Properties

        /// <summary>
        /// 展示的起始时间
        /// </summary>
        private DateTime DisplayStartDate
        {
            get { return _displayStartDate; }
            set
            {
                _displayStartDate = value;
                _displayMonth = _displayStartDate.Month;
                _displayYear = _displayStartDate.Year;
            }
        }

        /// <summary>
        /// 日期事件列表
        /// </summary>
        public ObservableCollection<IDateTimeEvent> DateTimeEvents
        {
            get { return _dateTimeEvents; }
            set
            {
                _dateTimeEvents = value;
                BuildCalendarUI();
                _dateTimeEvents.CollectionChanged += _dateTimeEvents_CollectionChanged;
            }
        }

        /// <summary>
        /// 创建日期事件控件的工厂
        /// </summary>
        public IDateTimeEventControlFactory DateTimeEventControlFactory { get; set; }

        #endregion Properties

        #region Public Methods

        public MonthControl()
        {
            InitializeComponent();
            _displayMonth = _displayStartDate.Month;
            _displayYear = _displayStartDate.Year;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 构造日历界面
        /// </summary>
        private void BuildCalendarUI()
        {
            //返回指定月份的总天数
            int daysInMonth = _cultureCalendar.GetDaysInMonth(_displayStartDate.Year, _displayStartDate.Month);

            //返回展示日期的星期数
            _startDayOffset = (int)Enum.ToObject(typeof(DayOfWeek), _displayStartDate.DayOfWeek);

            //总周数
            int weekCount = 0;
            var weekRowCtrl = new WeekControl();

            GrdMonthView.Children.Clear();
            SetMonthGridRowDefinition(daysInMonth, _startDayOffset);
            LblMonth.Content = string.Format("{0}年{1}月", _displayYear, _displayMonth);

            for (int i = 1; i <= daysInMonth; i++)
            {
                //算出是星期几
                int week = (i + _startDayOffset - 1) % 7;
                //因为第一次已经有周控件了,所以i != 1
                if ((i != 1) && (0 == week))
                {
                    //开始新一周,将上周的控件加入
                    Grid.SetRow(weekRowCtrl, weekCount);
                    GrdMonthView.Children.Add(weekRowCtrl);
                    weekRowCtrl = new WeekControl();
                    weekCount += 1;
                }

                //添加日控件
                var dayControl = new DayControl
                {
                    LblDayNumber = { Content = i.ToString(CultureInfo.InvariantCulture) },
                    Tag = i
                };
                dayControl.MouseDoubleClick += DayControl_DoubleClick;

                //如果为当天,设置不同的样式
                if (new DateTime(_displayYear, _displayMonth, i) == DateTime.Today)
                {
                    dayControl.BdrDayLabel.Background = (Brush)dayControl.TryFindResource("TodayBrush");
                    dayControl.SplDateTimeEvents.Background = Brushes.Wheat;
                }

                int iday = i;
                if (DateTimeEventControlFactory != null)
                {
                    var eventsInDay = from e in _dateTimeEvents
                                      where DayEqual(e.HappenTime, DisplayStartDate.AddDays(iday - 1))
                                      select e;
                    foreach (var e in eventsInDay)
                    {
                        var control = DateTimeEventControlFactory.GetControl(e);
                        if (control != null)
                        {
                            dayControl.SplDateTimeEvents.Children.Add(control.EventControl);
                        }
                    }
                }

                Grid.SetColumn(dayControl, (i - (weekCount * 7)) + _startDayOffset);
                weekRowCtrl.GrdWeek.Children.Add(dayControl);
            }

            //加入最后一周
            Grid.SetRow(weekRowCtrl, weekCount);
            GrdMonthView.Children.Add(weekRowCtrl);
        }

        /// <summary>
        /// 设置月份表格的行定义
        /// </summary>
        /// <param name="daysInMonth">该月的总天数</param>
        /// <param name="startDayOffSet">该月第一天的星期数</param>
        private void SetMonthGridRowDefinition(int daysInMonth, int startDayOffSet)
        {
            GrdMonthView.RowDefinitions.Clear();
            var rowHeight = new GridLength(60, GridUnitType.Star);
            //该月的最后一天
            var endDay = _displayStartDate.AddDays(daysInMonth - 1).DayOfWeek;
            //该月最后一天之后还剩的星期数
            int endDayOffset = 7 - ((int)Enum.ToObject(typeof(DayOfWeek), endDay) + 1);
            //计数总行数
            for (int i = 1; i <= (daysInMonth + startDayOffSet + endDayOffset) / 7; i++)
            {
                var rowDef = new RowDefinition
                {
                    Height = rowHeight
                };
                //添加行定义
                GrdMonthView.RowDefinitions.Add(rowDef);
            }
        }

        /// <summary>
        /// 比较两个日期是否在同一天
        /// </summary>
        /// <param name="dt1">第一个时间</param>
        /// <param name="dt2">第二个时间</param>
        /// <returns>在同一天返回true,否则返回false</returns>
        private bool DayEqual(DateTime dt1, DateTime dt2)
        {
            return (dt1.Year == dt2.Year) && (dt1.Month == dt2.Month) && (dt1.Day == dt2.Day);
        }

        /// <summary>
        /// 更新月
        /// </summary>
        /// <param name="monthsToAdd">添加的月数</param>
        private void UpdateMonth(int monthsToAdd)
        {
            var ev = new MonthChangedEventArgs
            {
                OldDisplayStartDate = _displayStartDate
            };
            DisplayStartDate = DisplayStartDate.AddMonths(monthsToAdd);
            ev.NewDisplayStartDate = DisplayStartDate;
            if (DisplayMonthChanged != null)
            {
                DisplayMonthChanged(ev);
            }
            BuildCalendarUI();
        }

        /// <summary>
        /// 根据号数查找天控件
        /// </summary>
        /// <param name="day">号数</param>
        /// <returns>查找到的天控件</returns>
        private DayControl FindDayControl(int day)
        {
            int index = day + _startDayOffset - 1;
            int row = index / 7;
            //因为第一个不是DayControl
            int column = index % 7 + 1;
            if (row == 0)
            {
                //第一行有空白
                column -= _startDayOffset;
            }

            var weekControl = GrdMonthView.Children[row] as WeekControl;
            if (weekControl != null)
            {
                return weekControl.GrdWeek.Children[column] as DayControl;
            }

            return null;
        }

        #endregion Private Methods

        #region Events Handling

        /// <summary>
        /// 载入控件的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_dateTimeEvents != null)
            {
                BuildCalendarUI();
            }
        }

        /// <summary>
        /// 点击上一月的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthGoPrev_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateMonth(-1);
        }

        /// <summary>
        /// 点击下一月的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthGoNext_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateMonth(1);
        }

        /// <summary>
        /// 天控件双击的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DayControl_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((DayControl)e.Source).Tag != null)
            {
                var dayControl = (DayControl)e.Source;
                if (e.OriginalSource.Equals(dayControl.SplDateTimeEvents))
                {
                    var day = (int)((DayControl)e.Source).Tag;
                    if (DayBlankDoubleClicked != null)
                    {
                        var ev = new DayEventArgs
                        {
                            DayTime = new DateTime(_displayYear, _displayMonth, day)
                        };
                        DayBlankDoubleClicked(ev);
                    }
                }
            }
        }

        /// <summary>
        /// 集合改变的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _dateTimeEvents_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if ((e.NewItems != null) && (DateTimeEventControlFactory != null))
            {
                foreach (var newItem in e.NewItems)
                {
                    if (newItem is IDateTimeEvent)
                    {
                        var datetimeEvent = newItem as IDateTimeEvent;
                        if ((datetimeEvent.HappenTime.Year == _displayYear) &&
                            (datetimeEvent.HappenTime.Month == _displayMonth))
                        {
                            var dayControl = FindDayControl(datetimeEvent.HappenTime.Day);
                            dayControl.SplDateTimeEvents.Children.Add(
                                DateTimeEventControlFactory.GetControl(datetimeEvent).EventControl);
                        }
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    if (oldItem is IDateTimeEvent)
                    {
                        var datetimeEvent = oldItem as IDateTimeEvent;
                        if ((datetimeEvent.HappenTime.Year == _displayYear) &&
                            (datetimeEvent.HappenTime.Month == _displayMonth))
                        {
                            var dayControl = FindDayControl(datetimeEvent.HappenTime.Day);
                            foreach (UIElement child in dayControl.SplDateTimeEvents.Children)
                            {
                                if ((child is IDateTimeEventControl) &&
                                    (child as IDateTimeEventControl).DateTimeEvent == datetimeEvent)
                                {
                                    dayControl.SplDateTimeEvents.Children.Remove(child);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        #endregion  Events Handling
    }
}
