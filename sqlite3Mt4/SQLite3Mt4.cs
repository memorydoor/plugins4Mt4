using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



namespace sqlite3Mt4
{
    public class SQLite3Mt4
    {
        private static Dictionary<string, SQLiteConnection> connections = new Dictionary<string, SQLiteConnection>();
        private static int connectionHandler = 0;
        private static string lastError;

        [DllExport("openSqlite3Connection", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static String openSqlite3Connection([MarshalAs(UnmanagedType.LPWStr)]string dbPath)
        {
            try
            {

                if (!System.IO.File.Exists(dbPath))
                {
                    SQLiteConnection.CreateFile(dbPath);
                }

                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + dbPath + ";Version=3;");

                m_dbConnection.Open();

                connectionHandler++;

                connections.Add(connectionHandler.ToString(), m_dbConnection);

                return connectionHandler.ToString();

            }
            catch (Exception ex)
            {
                lastError = "Error " + ex.ToString();
                return lastError;
            }
        }

        [DllExport("execute", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string execute(
            [MarshalAs(UnmanagedType.LPWStr)] string connectionHandler,
            [MarshalAs(UnmanagedType.LPWStr)] string sql)
        {
            try
            {
                SQLiteConnection connection = connections[connectionHandler];

                SQLiteCommand command = new SQLiteCommand(sql, connection);
                int count = command.ExecuteNonQuery();
                return count.ToString();

            }
            catch (Exception ex)
            {
                lastError = "Error " + ex.ToString();
                return lastError;
            }
        }

        private static Dictionary<String, SQLiteDataReader> readers = new Dictionary<String, SQLiteDataReader>();
        private static int readerHandler = 0;


        [DllExport("query", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string query(
            [MarshalAs(UnmanagedType.LPWStr)] string connectionHandler,
            [MarshalAs(UnmanagedType.LPWStr)] string sql)
        {
            try
            {
                readerHandler++;
                SQLiteConnection connection = connections[connectionHandler];

                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                readers.Add(readerHandler.ToString(), reader);

                return readerHandler.ToString();

            }
            catch (Exception ex)
            {
                lastError = "Error " + ex.ToString();
                return lastError;
            }
        }


        [DllExport("step", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string step([MarshalAs(UnmanagedType.LPWStr)] string readerHandler)
        {
            try
            {
                SQLiteDataReader reader = readers[readerHandler];
                bool hasResult = reader.Read();

                if (hasResult) {
                    return "1";
                } else {
                    reader.Close();
                    return "-1";
                }

            }
            catch (Exception ex)
            {
                lastError = "Error " + ex.ToString();
                return lastError;
            }
        }

        [DllExport("getColumnAsString", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string getColumnAsString(
            [MarshalAs(UnmanagedType.LPWStr)] string readerHandler, 
            [MarshalAs(UnmanagedType.LPWStr)] string columnName)
        {
            try
            {
                SQLiteDataReader reader = readers[readerHandler];

                return reader[columnName].ToString();

            }
            catch (Exception ex)
            {
                lastError = "Error " + ex.ToString();
                return lastError;
            }
        }

        [DllExport("closeSqlite3Connection", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string closeSqlite3Connection([MarshalAs(UnmanagedType.LPWStr)]string connectionHandler)
        {
            try
            {
                SQLiteConnection connection = connections[connectionHandler];
                connections.Remove(connectionHandler);
                connection.Close();
                return "";
            }
            catch (Exception ex)
            {
                lastError = "Error " + ex.ToString();
                return lastError;
            }
        }

        [DllExport("getLastError", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string getLastError()
        {
            return lastError;
        }
    }
}
