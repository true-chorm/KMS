using KMS.src.core;
using KMS.src.tool;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace KMS.src.db
{
    class SQLiteManager
    {
        private const string TAG = "SQLiteManager";

        internal const byte GLOBAL_RECORD = 1;
        internal const byte YEAR_RECORD = 2;

        private const string DATABASE_DIR = "data";
        private const string GLOBAL_TABLE = "global";
        private const string YEAR_TABLE = "year";
        private const string MONTH_TABLE = "month";
        private const string DAY_TABLE = "day";
        private const string HOUR_TABLE = "hour";

        private static SQLiteManager instance;

        private readonly SQLiteHelper globalDatabase; //存储全局统计数据
        private SQLiteHelper detailDatabase; //存储本年度统计数据

        internal static SQLiteManager GetInstance
        {
            get
            {
                if (instance == null)
                    instance = new SQLiteManager();

                return instance;
            }
        }

        private SQLiteManager()
        {
            //make sure the data path ok
            if (File.Exists(DATABASE_DIR))
            {
                File.Move(DATABASE_DIR, DATABASE_DIR + "_rename");
            }
            if (!Directory.Exists(DATABASE_DIR))
            {
                Directory.CreateDirectory(DATABASE_DIR);
            }

            globalDatabase = new SQLiteHelper();
            detailDatabase = new SQLiteHelper();
        }

        /// <summary>
        /// 连接到全局数据库与年度数据库。
        /// </summary>
        /// <returns>true 两个数据库均已正常连接。false 未能全部正常创建连接。</returns>
        internal bool Init()
        {
            if (globalDatabase is null || detailDatabase is null)
                return false;

            if (!globalDatabase.OpenDatabase(GlobalDb))
                return false;

            if (!detailDatabase.OpenDatabase(YearDb))
                return false;

            return true;
        }

        /// <summary>
        /// 检查全局库、年度库中各对应表是否存在，若不存在，则创建表并为各统计字段插入初始值。
        /// 2021-01-14 12:07
        /// </summary>
        internal void InitTables()
        {
            if (!globalDatabase.IsTableExist(GLOBAL_TABLE))
            {
                Logger.v(TAG, "creating global table");
                globalDatabase.ExecuteNonQuery("CREATE TABLE " + GLOBAL_TABLE + "(type SMALLINT PRIMARY KEY NOT NULL,value INTEGER)");
                InitGlobalTable();
            }

            if (!detailDatabase.IsTableExist(YEAR_TABLE))
            {
                Logger.v(TAG, "creating year table");
                CreateYearTable();
                InitYearTable();
            }

            if (!detailDatabase.IsTableExist(MONTH_TABLE))
            {
                Logger.v(TAG, "creating month table");
                CreateMonthTable();
                InitMonthTable();
            }

            if (!detailDatabase.IsTableExist(DAY_TABLE))
            {
                Logger.v(TAG, "creating day table");
                CreateDayTable();
                InitDayTable();
            }

            if (!detailDatabase.IsTableExist(HOUR_TABLE))
            {
                Logger.v(TAG, "creating hour table");
                detailDatabase.ExecuteNonQuery("CREATE TABLE " + HOUR_TABLE +
                    "(type SMALLINT NOT NULL,value INTEGER,year SMALLINT NOT NULL,month TINYINT NOT NULL,day TINYINT NOT NULL,hour TINYINT NOT NULL)");
                //Enough!
            }
        }

        /// <summary>
        /// 日期更换时由StatisticManager调用。
        /// </summary>
        internal SQLiteDataReader TryQueryDayWhileSwitchDate()
        {
            Logger.v(TAG, "TryQueryDayWhileSwitchDate()");
            // 检查指定日期记录是否存在。
            SQLiteDataReader reader = detailDatabase.ExecuteQuery(QueryDayTableSQL);
            if (reader is null)
            {
                Logger.v(TAG, "query day table null");
                return null;
            }
            else
            {
                if (!reader.HasRows)
                {
                    reader.Close();
                    Logger.v(TAG, "No day record found, initing");
                    InitDayTable();
                    return null;
                }
            }

            return reader;
        }

        internal SQLiteDataReader TryQueryMonthWhileSwitchDate()
        {
            SQLiteDataReader reader = detailDatabase.ExecuteQuery(QueryMonthTableSQL);
            if (reader is null)
            {
                Logger.v(TAG, "query month table null");
                return null;
            }
            else
            {
                if (!reader.HasRows)
                {
                    reader.Close();
                    Logger.v(TAG, "No month record found, initing");
                    InitMonthTable();
                    return null;
                }
            }

            return reader;
        }

        internal SQLiteDataReader TryQueryYearWhileSwitchDate()
        {
            SQLiteDataReader reader = detailDatabase.ExecuteQuery(QueryYearTableSQL);
            if (reader is null)
            {
                Logger.v(TAG, "query year table null");
                return null;
            }
            else
            {
                if (!reader.HasRows)
                {
                    reader.Close();
                    Logger.v(TAG, "No year record found, initing");
                    InitYearTable();
                    return null;
                }
            }

            return reader;
        }

        private void CreateYearTable()
        {
            detailDatabase.ExecuteNonQuery("CREATE TABLE " + YEAR_TABLE + "(type SMALLINT PRIMARY KEY NOT NULL,value INTEGER,year SMALLINT NOT NULL)");
        }

        private void CreateMonthTable()
        {
            detailDatabase.ExecuteNonQuery("CREATE TABLE " + MONTH_TABLE + "(type SMALLINT NOT NULL,value INTEGER,year SMALLINT NOT NULL,month TINYINT NOT NULL)");
        }

        private void CreateDayTable()
        {
            detailDatabase.ExecuteNonQuery("CREATE TABLE " + DAY_TABLE + "(type SMALLINT NOT NULL,value INTEGER,year SMALLINT NOT NULL,month TINYINT NOT NULL,day TINYINT NOT NULL)");
        }

        internal void InitGlobalTable()
        {
            if (globalDatabase.BeginTransaction())
            {
                InsertGlobalItem(Constants.TypeNumber.KEYBOARD_TOTAL);
                InsertGlobalItem(Constants.TypeNumber.KEYBOARD_COMBOL_TOTAL);
                InsertGlobalItem(Constants.TypeNumber.MOUSE_TOTAL);
                InsertGlobalItem(Constants.TypeNumber.MOUSE_LEFT_BTN);
                InsertGlobalItem(Constants.TypeNumber.MOUSE_RIGHT_BTN);
                InsertGlobalItem(Constants.TypeNumber.MOUSE_WHEEL_FORWARD);
                InsertGlobalItem(Constants.TypeNumber.MOUSE_WHEEL_BACKWARD);
                InsertGlobalItem(Constants.TypeNumber.MOUSE_WHEEL_CLICK);
                InsertGlobalItem(Constants.TypeNumber.MOUSE_SIDE_FORWARD);
                InsertGlobalItem(Constants.TypeNumber.MOUSE_SIDE_BACKWARD);

                Dictionary<byte, Key>.KeyCollection keys = Constants.Keyboard.Keys;
                foreach (byte keycode in keys)
                {
                    InsertGlobalItem(keycode);
                }

                globalDatabase.CommitTransaction();
            }
            else
            {
                MessageBox.Show("Init database failed");
                throw new Exception();
            }
        }

        internal void InitYearTable()
        {
            if (detailDatabase.BeginTransaction())
            {
                InsertYearItem(Constants.TypeNumber.KEYBOARD_TOTAL);
                InsertYearItem(Constants.TypeNumber.KEYBOARD_COMBOL_TOTAL);
                InsertYearItem(Constants.TypeNumber.MOUSE_TOTAL);
                InsertYearItem(Constants.TypeNumber.MOUSE_LEFT_BTN);
                InsertYearItem(Constants.TypeNumber.MOUSE_RIGHT_BTN);
                InsertYearItem(Constants.TypeNumber.MOUSE_WHEEL_FORWARD);
                InsertYearItem(Constants.TypeNumber.MOUSE_WHEEL_BACKWARD);
                InsertYearItem(Constants.TypeNumber.MOUSE_WHEEL_CLICK);
                InsertYearItem(Constants.TypeNumber.MOUSE_SIDE_FORWARD);
                InsertYearItem(Constants.TypeNumber.MOUSE_SIDE_BACKWARD);

                Dictionary<byte, Key>.KeyCollection keys = Constants.Keyboard.Keys;
                foreach (byte keycode in keys)
                {
                    InsertYearItem(keycode);
                }

                detailDatabase.CommitTransaction();
            }
            else
            {
                MessageBox.Show("Init database failed");
                throw new Exception();
            }
        }

        internal void InitMonthTable()
        {
            if (detailDatabase.BeginTransaction())
            {
                InsertMonthItem(Constants.TypeNumber.KEYBOARD_TOTAL);
                InsertMonthItem(Constants.TypeNumber.KEYBOARD_COMBOL_TOTAL);
                InsertMonthItem(Constants.TypeNumber.MOUSE_TOTAL);
                InsertMonthItem(Constants.TypeNumber.MOUSE_LEFT_BTN);
                InsertMonthItem(Constants.TypeNumber.MOUSE_RIGHT_BTN);
                InsertMonthItem(Constants.TypeNumber.MOUSE_WHEEL_FORWARD);
                InsertMonthItem(Constants.TypeNumber.MOUSE_WHEEL_BACKWARD);
                InsertMonthItem(Constants.TypeNumber.MOUSE_WHEEL_CLICK);
                InsertMonthItem(Constants.TypeNumber.MOUSE_SIDE_FORWARD);
                InsertMonthItem(Constants.TypeNumber.MOUSE_SIDE_BACKWARD);

                Dictionary<byte, Key>.KeyCollection keys = Constants.Keyboard.Keys;
                foreach (byte keycode in keys)
                {
                    InsertMonthItem(keycode);
                }

                detailDatabase.CommitTransaction();
            }
            else
            {
                MessageBox.Show("Init database failed");
                throw new Exception();
            }
        }

        private void InitDayTable()
        {
            Logger.v(TAG, "InitDayTable()()");
            if (detailDatabase.BeginTransaction())
            {
                InsertDayItem(Constants.TypeNumber.KEYBOARD_TOTAL);
                InsertDayItem(Constants.TypeNumber.KEYBOARD_COMBOL_TOTAL);
                InsertDayItem(Constants.TypeNumber.MOUSE_TOTAL);
                InsertDayItem(Constants.TypeNumber.MOUSE_LEFT_BTN);
                InsertDayItem(Constants.TypeNumber.MOUSE_RIGHT_BTN);
                InsertDayItem(Constants.TypeNumber.MOUSE_WHEEL_FORWARD);
                InsertDayItem(Constants.TypeNumber.MOUSE_WHEEL_BACKWARD);
                InsertDayItem(Constants.TypeNumber.MOUSE_WHEEL_CLICK);
                InsertDayItem(Constants.TypeNumber.MOUSE_SIDE_FORWARD);
                InsertDayItem(Constants.TypeNumber.MOUSE_SIDE_BACKWARD);

                Dictionary<byte, Key>.KeyCollection keys = Constants.Keyboard.Keys;
                foreach (byte keycode in keys)
                {
                    InsertDayItem(keycode);
                }

                detailDatabase.CommitTransaction();
            }
            else
            {
                MessageBox.Show("Init database failed");
                throw new Exception();
            }
        }

        internal void InsertGlobalItem(ushort type)
        {
            globalDatabase.ExecuteNonQuery("INSERT INTO " + GLOBAL_TABLE + " VALUES(" + type + ",0)");
        }

        private void InsertYearItem(ushort type)
        {
            detailDatabase.ExecuteNonQuery("INSERT INTO " + YEAR_TABLE + " VALUES(" + type + ",0," + TimeManager.TimeUsing.Year + ")");
        }

        private void InsertMonthItem(ushort type)
        {
            detailDatabase.ExecuteNonQuery("INSERT INTO " + MONTH_TABLE + " VALUES(" + type + ",0," + TimeManager.TimeUsing.Year + "," + TimeManager.TimeUsing.Month + ")");
        }

        private void InsertDayItem(ushort type)
        {
            detailDatabase.ExecuteNonQuery("INSERT INTO " + DAY_TABLE + " VALUES(" + type + ",0," + TimeManager.TimeUsing.Year + "," + TimeManager.TimeUsing.Month + "," + TimeManager.TimeUsing.Day + ")");
        }

        internal void InsertHourItem(ushort type, uint value, byte hour)
        {
            detailDatabase.ExecuteNonQuery("INSERT INTO " + HOUR_TABLE + " VALUES(" + type + "," + value + ","
                + TimeManager.TimeUsing.Year + ","
                + TimeManager.TimeUsing.Month + ","
                + TimeManager.TimeUsing.Day + ","
                + hour + ")");
        }

        internal void close()
        {
            //if (sqliteHelper != null)
            //    sqliteHelper.closeDababase();
        }

        internal bool BeginTransaction(byte which)
        {
            if (which == GLOBAL_RECORD)
                return globalDatabase.BeginTransaction();
            else if (which == YEAR_RECORD)
                return detailDatabase.BeginTransaction();
            else
                return false;
        }

        internal void CommitTransaction(byte which)
        {
            if (which == GLOBAL_RECORD)
                globalDatabase.CommitTransaction();
            else if (which == YEAR_RECORD)
                detailDatabase.CommitTransaction();
        }

        internal void UpdateGlobal(ushort type, uint value)
        {
            globalDatabase.ExecuteNonQuery("UPDATE " + GLOBAL_TABLE + " SET value=" + value + " WHERE type=" + type);
        }

        internal void UpdateYear(ushort type, uint value)
        {
            detailDatabase.ExecuteNonQuery("UPDATE " + YEAR_TABLE + " SET value=" + value + " WHERE type=" + type
                + " AND year=" + TimeManager.TimeUsing.Year);
        }

        internal void UpdateMonth(ushort type, uint value)
        {
            detailDatabase.ExecuteNonQuery("UPDATE " + MONTH_TABLE + " SET value=" + value + " WHERE type=" + type
                + " AND year=" + TimeManager.TimeUsing.Year
                + " AND month=" + TimeManager.TimeUsing.Month);
        }

        internal void UpdateDay(ushort type, uint value)
        {
            detailDatabase.ExecuteNonQuery("UPDATE " + DAY_TABLE + " SET value=" + value
                + " WHERE type=" + type
                + " AND year=" + TimeManager.TimeUsing.Year
                + " AND month=" + TimeManager.TimeUsing.Month
                + " AND day=" + TimeManager.TimeUsing.Day);
        }

        internal void UpdateHour(ushort type, uint value, byte hour)
        {
            Logger.v(TAG, "UpdateHour,type:" + type + ",hour:" + hour + ",value:" + value);
            detailDatabase.ExecuteNonQuery("UPDATE " + HOUR_TABLE + " SET value=" + value
                + " WHERE year=" + TimeManager.TimeUsing.Year
                + " AND month=" + TimeManager.TimeUsing.Month
                + " AND day=" + TimeManager.TimeUsing.Day
                + " AND hour=" + hour
                + " AND type=" + type);
        }

        internal SQLiteDataReader QueryGlobalStatistic()
        {
            return Query(globalDatabase, GLOBAL_TABLE, QueryGlobalTableSQL);
        }

        internal SQLiteDataReader QueryHourStatistic()
        {
            return Query(detailDatabase, HOUR_TABLE, QueryHourTableSQL);
        }

        private SQLiteDataReader Query(SQLiteHelper sqlite, string table, string sql)
        {
            if (sqlite.IsDbReady())
            {
                if (sqlite.IsTableExist(table))
                {
                    return sqlite.ExecuteQuery(sql);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        internal void SwitchYearDB()
        {
            detailDatabase.CloseDababase();
            detailDatabase.OpenDatabase(YearDb);
        }

        /// <summary>
        /// 全局统计数据数据库文件。
        /// </summary>
        private string GlobalDb
        {
            get
            {
                return DATABASE_DIR + "/kmsall.db";
            }
        }

        /// <summary>
        /// 本年度统计数据库文件。
        /// </summary>
        private string YearDb
        {
            get
            {
                return DATABASE_DIR + "/kms" + TimeManager.TimeUsing.Year + ".db";
            }
        }

        private string QueryGlobalTableSQL
        {
            get
            {
                return "SELECT*FROM " + GLOBAL_TABLE;
            }
        }

        private string QueryYearTableSQL
        {
            get
            {
                return "SELECT*FROM " + YEAR_TABLE;
            }
        }

        private string QueryMonthTableSQL
        {
            get
            {
                return "SELECT*FROM " + MONTH_TABLE
                    + " WHERE year=" + TimeManager.TimeUsing.Year
                    + " AND month=" + TimeManager.TimeUsing.Month;
            }
        }

        private string QueryDayTableSQL
        {
            get
            {
                return "SELECT*FROM " + DAY_TABLE + " WHERE year=" + TimeManager.TimeUsing.Year
                    + " AND month=" + TimeManager.TimeUsing.Month
                    + " AND day=" + TimeManager.TimeUsing.Day;
            }
        }

        private string QueryHourTableSQL
        {
            get
            {
                return "SELECT type,value,hour FROM " + HOUR_TABLE
                    + " WHERE year=" + TimeManager.TimeUsing.Year
                    + " AND month=" + TimeManager.TimeUsing.Month
                    + " AND day=" + TimeManager.TimeUsing.Day;
            }
        }
    }
}
