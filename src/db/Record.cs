using KMS.src.core;
using System;
using System.ComponentModel;

namespace KMS.src.db
{
    class Record : IComparable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedEventArgs pcaValue;
        private readonly PropertyChangedEventArgs pcaDesc;

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
        private ushort millisecond;
        private uint value;
        private string desc;

        internal Record() : this(Constants.TypeNumber.INVALID)
        {

        }

        internal Record(ushort type)
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
                    second = DEF_SECOND;
                }
                else
                {
                    second = value;
                }
            }
        }

        internal ushort MilliSecond
        {
            get { return millisecond; }
            set
            {
                if (value > 999)
                    millisecond = 0;
                else
                    millisecond = value;
            }
        }

        internal ushort Type
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
                if (value > 2000000000) //上限20亿
                {
                    this.value = 2000000000;
                }
                else
                {
                    this.value = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged.Invoke(this, pcaValue);
                    }
                }

                IsUpdated = true;
            }
        }

        public string Name
        {
            get;
            set;
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

        internal bool IsUpdated
        {
            get;
            set;
        }

        internal void SetTime(DateTime time)
        {
            Year = (short)time.Year;
            Month = (byte)time.Month;
            Day = (byte)time.Day;
            Hour = (byte)time.Hour;
            Minute = (byte)time.Minute;
            Second = (byte)time.Second;
        }

        public int CompareTo(object obj)
        {
            if (obj is Record rco)
            {
                if (rco.Value > Value)
                    return 1;
                else if (rco.Value == Value)
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
