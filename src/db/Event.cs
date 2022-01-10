using System;
using System.ComponentModel;

namespace KMS.src.db
{
    /// <summary>
    /// 一个键鼠事件，包含：1、具体的时间信息；2、事件类型。
    /// </summary>
    class Event : IComparable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private PropertyChangedEventArgs pcaValue;
        private PropertyChangedEventArgs pcaDesc;

        private const short DEF_YEAR = 0;
        private const byte DEF_MONTH = 0;
        private const byte DEF_DAY = 0;
        private const byte DEF_HOUR = 0;
        private const byte DEF_MINUTE = 0;
        private const byte DEF_SECOND = 0;

        private short year;
        private byte month;
        private byte day;
        private byte hour;
        private byte minute;
        private byte second;
        private uint value;
        private string desc;


        internal Event() : this(null)
        {
        
        }

        internal Event(core.Type type)
        {
            Type = type;
            pcaValue = new PropertyChangedEventArgs("Value");
            pcaDesc = new PropertyChangedEventArgs("Desc");
        }

        internal short Year
        {
            get { return year; }
            set
            {
                if (value < 1970 || value > 2099)
                {
                    year = DEF_YEAR;
                }
                else
                {
                    year = value;
                }
            }
        }

        internal byte Month
        {
            get { return month; }
            set
            {
                if (value < 1 || value > 12)
                {
                    year = DEF_YEAR;
                    month = DEF_MONTH;
                }
                else
                {
                    month = value;
                }
            }
        }

        internal byte Day
        {
            get { return day; }
            set
            {
                if (value < 1 || value > 31)
                {
                    year = DEF_YEAR;
                    month = DEF_MONTH;
                    day = DEF_DAY;
                }
                else
                {
                    day = value;
                }
            }
        }

        internal byte Hour
        {
            get { return hour; }
            set
            {
                if (value > 23)
                {
                    year = DEF_YEAR;
                    month = DEF_MONTH;
                    day = DEF_DAY;
                    hour = DEF_HOUR;
                }
                else
                {
                    hour = value;
                }
            }
        }

        internal byte Minute
        {
            get { return minute; }
            set
            {
                if (value > 59)
                {
                    year = DEF_YEAR;
                    month = DEF_MONTH;
                    day = DEF_DAY;
                    hour = DEF_HOUR;
                    minute = DEF_MINUTE;
                }
                else
                {
                    minute = value;
                }
            }
        }

        internal byte Second
        {
            get { return second; }
            set
            {
                if (value > 59)
                {
                    year = DEF_YEAR;
                    month = DEF_MONTH;
                    day = DEF_DAY;
                    hour = DEF_HOUR;
                    minute = DEF_MINUTE;
                    second = DEF_SECOND;
                }
                else
                {
                    second = value;
                }
            }
        }

        internal core.Type Type
        {
            get;
            set;
        }

        public uint Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, pcaValue);
                }
            }
        }

        public string Desc
        {
            get
            {
                return desc;
            }

            set
            {
                desc = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, pcaDesc);
                }
            }
        }

        internal void setTime(DateTime time)
        {
            Year = (short)time.Year;
            Month = (byte)time.Month;
            Day = (byte)time.Day;
            Hour = (byte)time.Hour;
            Minute = (byte)time.Minute;
            Second = (byte)time.Second;
        }

        public int CompareTo(Object obj)
        {
            if (obj is Event)
            {
                Event evt = (Event)obj;
                if (evt.Value > Value)
                    return 1;
                else if (evt.Value == Value)
                    return 0;
                else
                    return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}