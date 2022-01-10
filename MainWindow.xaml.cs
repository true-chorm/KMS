using System;
using System.Threading;
using System.Windows;
using KMS.src.core;
using KMS.src.tool;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;

namespace KMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const string TAG = "KMS";

        private Thread countThread;
        private readonly StatisticManager statisticManager;
        private GlobalStatisticDetail globalStatisticDetail;
        private NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            Logger.v(TAG, "hello world");
            src.tool.Timer.StartTimer();
            TimeManager.TimeUsing = DateTime.Now;

            statisticManager = StatisticManager.GetInstance;
            StartWatching();
            bindData();
            NotifyIconInit();
        }

        private void bindData()
        {
            //全局键盘统计
            System.Windows.Data.Binding binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.GetRecord(Constants.TypeNumber.KEYBOARD_TOTAL);
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(KbTotal, TextBlock.TextProperty, binding);

            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.GetRecord(Constants.TypeNumber.KEYBOARD_COMBOL_TOTAL);
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(ComboTotal, TextBlock.TextProperty, binding);

            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.GetRecord(Constants.TypeNumber.KB_SK_TOP1);
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(SkTop1, TextBlock.TextProperty, binding);

            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.GetRecord(Constants.TypeNumber.KB_SK_TOP2);
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(SkTop2, TextBlock.TextProperty, binding);

            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.GetRecord(Constants.TypeNumber.KB_SK_TOP3);
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(SkTop3, TextBlock.TextProperty, binding);

            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.GetRecord(Constants.TypeNumber.KB_SK_TOP4);
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(SkTop4, TextBlock.TextProperty, binding);

            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.GetRecord(Constants.TypeNumber.KB_SK_TOP5);
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(SkTop5, TextBlock.TextProperty, binding);

            //全局鼠标统计
            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.GetRecord(Constants.TypeNumber.MOUSE_LEFT_BTN);
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(MsLeftBtn, TextBlock.TextProperty, binding);

            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.GetRecord(Constants.TypeNumber.MOUSE_RIGHT_BTN);
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(MsRightBtn, TextBlock.TextProperty, binding);

            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.GetRecord(Constants.TypeNumber.MOUSE_WHEEL_FORWARD);
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(MsWheelForward, TextBlock.TextProperty, binding);

            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.GetRecord(Constants.TypeNumber.MOUSE_WHEEL_BACKWARD);
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(MsWheelBackward, TextBlock.TextProperty, binding);

            //今日统计
            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.SttKeyboardTotalToday;
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(KbAllToday, TextBlock.TextProperty, binding);

            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.SttMouseTotalToday;
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(MsAllToday, TextBlock.TextProperty, binding);

            binding = new System.Windows.Data.Binding();
            binding.Source = statisticManager.SttMostOpHourToday;
            binding.Path = new PropertyPath("Desc");
            BindingOperations.SetBinding(MostOpHourToday, TextBlock.TextProperty, binding);
        }

        private void NotifyIconInit()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Text = "键鼠统计器";
            notifyIcon.MouseClick += NotifyIcon_Click;

            ContextMenuStrip menus = new ContextMenuStrip();
            menus.Items.Add(Resources["exit"].ToString());
            menus.ItemClicked += Menus_ItemClick;
            notifyIcon.ContextMenuStrip = menus;

            notifyIcon.Icon = new System.Drawing.Icon("icon_small.ico");
            notifyIcon.Visible = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Hide();
                e.Cancel = true;
            }
        }

        private void NotifyIcon_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                notifyIcon.ContextMenuStrip.Show();
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (Visibility == Visibility.Hidden)
                {
                    Show();
                }
            }
        }

        private void Menus_ItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            if (Resources["exit"].ToString().Equals(e.ClickedItem.ToString()))
            {
                Logger.v(TAG, "Exiting kms");
                StopAll();
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void GlobalDetail(object sender, MouseButtonEventArgs e)
        {
            if (globalStatisticDetail != null)
            {
                globalStatisticDetail.Close();
            }

            globalStatisticDetail = new GlobalStatisticDetail();
            globalStatisticDetail.SetStatistic(
                statisticManager.GetRecord(Constants.TypeNumber.KEYBOARD_TOTAL),
                statisticManager.GetRecord(Constants.TypeNumber.MOUSE_TOTAL),
                statisticManager.GetRecord(Constants.TypeNumber.MOUSE_SIDE_FORWARD),
                statisticManager.GetRecord(Constants.TypeNumber.MOUSE_SIDE_BACKWARD),
                statisticManager.GetRecord(Constants.TypeNumber.MOUSE_WHEEL_CLICK),
                statisticManager.GetRecord(Constants.TypeNumber.MOUSE_WHEEL_FORWARD),
                statisticManager.GetRecord(Constants.TypeNumber.MOUSE_WHEEL_BACKWARD),
                statisticManager.GetRecord(Constants.TypeNumber.MOUSE_LEFT_BTN),
                statisticManager.GetRecord(Constants.TypeNumber.MOUSE_RIGHT_BTN),
                statisticManager.GetSingleKeyRecords()); //Must call before 'show'
            globalStatisticDetail.ShowDialog();
        }

        private void StartWatching()
        {
            countThread = new Thread(CountThread.ThreadProc);
            countThread.Start();
            Logger.v(TAG, "Count thread started");

            KMEventHook.InsertHook();
            Logger.v(TAG, "KM-event listening");
        }

        private void StopAll()
        {
            KMEventHook.RemoveHook();
            if (countThread != null)
                CountThread.CanThreadRun = false;
            Logger.v(TAG, "Hook removed");
            src.tool.Timer.DestroyTimer();
            statisticManager.Shutdown();
        }
    }
}
