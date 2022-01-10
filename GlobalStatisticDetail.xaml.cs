using KMS.src.db;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace KMS
{
    /// <summary>
    /// Interaction logic for GlobalStatisticDetail.xaml
    /// </summary>
    public partial class GlobalStatisticDetail : Window
    {
        private bool isSortProcRunning;
        private uint kbLast;
        private uint msLast;

        private Record kbTotal;
        private Record msTotal;
        private List<Record> records;

        private delegate void SortRecords();

        public GlobalStatisticDetail()
        {
            InitializeComponent();
        }

        internal void SetStatistic(Record kbTotal, Record msTotal,
            Record msSideForward,
            Record msSideBackward,
            Record msWheelClick,
            Record msWheelFw,
            Record msWheelBw,
            Record msLbtn,
            Record msRbtn,
            List<Record> kbKeys)
        {
            if (msSideBackward is null || msSideForward is null || msWheelClick is null
                || kbKeys is null || kbTotal is null || msTotal is null
                || msWheelFw is null || msWheelBw is null || msLbtn is null || msRbtn is null)
                return;

            this.kbTotal = kbTotal;
            this.msTotal = msTotal;
            records = new List<Record>(kbKeys.Count + 7);
            records.AddRange(kbKeys);
            records.Add(msSideBackward);
            records.Add(msSideForward);
            records.Add(msWheelClick);
            records.Add(msWheelFw);
            records.Add(msWheelBw);
            records.Add(msLbtn);
            records.Add(msRbtn);
            
            records.Sort();
        }

        private void LoadRecords()
        {
            foreach (Record rco in records)
            {
                LVGlobalKeys.Items.Add(rco);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRecords();
            kbLast = kbTotal.Value;
            msLast = msTotal.Value;
            new Thread(RecordsSortProc).Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isSortProcRunning = false;
        }

        private void RecordsSortProc(object cb)
        {
            if (!isSortProcRunning)
            {
                isSortProcRunning = true;
                while (isSortProcRunning)
                {
                    Thread.Sleep(2000);
                    if (kbTotal.Value != kbLast || msLast != msTotal.Value)
                    {
                        kbLast = kbTotal.Value;
                        msLast = msTotal.Value;
                        records.Sort();
                        LVGlobalKeys.Dispatcher.Invoke(new SortRecords(Sort));
                    }
                }
            }
        }

        private void Sort()
        {
            LVGlobalKeys.Items.Clear();
            LoadRecords();
        }
    }
}
