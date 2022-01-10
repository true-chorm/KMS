using KMS.src.tool;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace KMS.src.db
{
    class SQLiteHelper
    {
        private SQLiteConnection sqliteConnection;
        private SQLiteTransaction sqliteTransaction;
        private SQLiteCommand nonQueryCmd;
        private SQLiteCommand queryCmd;

        public bool OpenDatabase(string path)
        {
            if (path is null || path.Length == 0)
                return false;

            try
            {
                sqliteConnection = new SQLiteConnection("data source=" + path);
                sqliteConnection.Open();

                //SQLite在打开文件时不会检测其是否合法，此处通过执行SQL语句来检测库文件合法性。
                //当库文件不合法时，将其重命名为KMS不识别的格式以避免后续继续打开此文件而浪费计算资源。
                //chorm, 2021-01-12 22:55
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand(sqliteConnection);
                    cmd.CommandText = "SELECT name from sqlite_master";
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    reader.Close();
                }
                catch (SQLiteException)
                {
                    sqliteConnection.Close();
                    File.Move(path, path + "_invalid");
                    return false;
                }

                nonQueryCmd = new SQLiteCommand(sqliteConnection);
                queryCmd = new SQLiteCommand(sqliteConnection);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        internal bool IsDbReady()
        {
            if (sqliteConnection is null)
                return false;

            return sqliteConnection.State is ConnectionState.Open;
        }

        internal bool IsTableExist(string name)
        {
            if (name is null || name.Length is 0 || sqliteConnection is null || sqliteConnection.State != ConnectionState.Open)
                return false;

            SQLiteCommand cmd = new SQLiteCommand(sqliteConnection);
            cmd.CommandText = "SELECT name FROM sqlite_master where type='table' AND name='" + name + "'";
            SQLiteDataReader reader = cmd.ExecuteReader();
            bool hasRow = reader.HasRows;
            reader.Close();
            
            return hasRow;
        }

        public void CloseDababase()
        {
            if (IsDbReady())
            {
                sqliteConnection.Close();
            }
            sqliteConnection = null;
        }

        internal bool BeginTransaction()
        {
            sqliteTransaction = sqliteConnection.BeginTransaction();
            return sqliteTransaction != null;
        }

        internal bool CommitTransaction()
        {
            if (sqliteTransaction is null)
                return false;

            sqliteTransaction.Commit();
            sqliteTransaction = null;
            return true;
        }

        internal void ExecuteNonQuery(string sql)
        {
            nonQueryCmd.CommandText = sql;
            nonQueryCmd.ExecuteNonQuery();
        }

        internal SQLiteDataReader ExecuteQuery(string sql)
        {
            queryCmd.CommandText = sql;
            return queryCmd.ExecuteReader();
        }
    }
}
