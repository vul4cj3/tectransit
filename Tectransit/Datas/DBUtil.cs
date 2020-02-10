using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace Tectransit.Datas
{
    public class DBUtil
    {
        //SqlServer
        static String dbType = "SqlServer";
        public static string m_ConnectionString = string.Empty;        

        public static string GetConnectionString()
        {
            AppConfigHelper objAppConfig = new AppConfigHelper();

            if (m_ConnectionString == string.Empty)
            {
                m_ConnectionString = objAppConfig.ConnectionString;
            }
            return m_ConnectionString;
        }        

        //Select
        public static ArrayList Select(string strSQL, Hashtable args)
        {
            DataTable data = new DataTable();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            try
            {
                if (cn.State != ConnectionState.Open)
                {
                    cn.ConnectionString = GetConnectionString();
                    cn.Open();
                }
                cmd = new SqlCommand(strSQL, cn);
                if (args != null) SetArgs(strSQL, args, cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(data);
            }
            catch (Exception ex)
            {
                string msg = ex.Message?.ToString();
                throw ex;
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
            }
            return DataTable2ArrayList(data);
        }

        public static ArrayList Select(string strSQL, Hashtable args, SqlConnection conn, SqlTransaction ts)
        {
            DataTable data = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            try
            {

                cmd = new SqlCommand(strSQL, conn, ts);
                cmd.Transaction = ts;
                if (args != null) SetArgs(strSQL, args, cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(data);
            }
            catch (Exception ex)
            {
                string msg = ex.Message?.ToString();
                throw ex;
            }
            return DataTable2ArrayList(data);
        }

        public static DataTable SelectDataTable(string strSQL)
        {
            return SelectDataTable(strSQL, null);
        }

        //SelectDataTable
        public static DataTable SelectDataTable(string strSQL, Hashtable args)
        {
            DataTable data = new DataTable();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = null;
            SqlDataAdapter da = null;

            try
            {
                if (cn.State != ConnectionState.Open)
                {
                    cn.ConnectionString = GetConnectionString();
                    cn.Open();
                }
                cmd = new SqlCommand(strSQL, cn);
                if (args != null) SetArgs(strSQL, args, cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(data);
            }
            catch (Exception ex)
            {
                string msg = ex.Message?.ToString();
                throw ex;
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
            }
            return data;
        }

        //EXECUTE
        public static int EXECUTE(string sql, Hashtable args = null)
        {
            int result = 0;

            using (SqlConnection cn = new SqlConnection() { ConnectionString = GetConnectionString() })
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cn.Open();
                        if (args != null)
                        {
                            MatchCollection ms = Regex.Matches(sql, @"@\w+");
                            foreach (Match m in ms)
                            {
                                string key = m.Value;

                                if (args[key] == null)
                                {
                                    if (args[key.Substring(1)] != null)
                                        cmd.Parameters.Add(new SqlParameter(key, args[key.Substring(1)]));
                                    else
                                        cmd.Parameters.Add(new SqlParameter(key, DBNull.Value));

                                }
                                else if (args[key] != null)
                                {
                                    cmd.Parameters.Add(new SqlParameter(key, args[key]));
                                    //int length = args[key].ToString().Length;
                                    //string str = key + " is " + length;
                                }
                            }
                        }
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch
                {
                    throw;
                }
            }
            return result;
        }

        //EXECUTE
        public static int EXECUTE(string sql, Hashtable args, SqlConnection sqlConn, SqlTransaction sqlTran)
        {
            SqlConnection con = sqlConn;
            SqlCommand cmd = new SqlCommand(sql, con, sqlTran);
            cmd.Transaction = sqlTran;
            if (args != null) SetArgs(sql, args, cmd);
            return cmd.ExecuteNonQuery();
        }
        
        public static string GetSingleValue1(string sql)
        {
            string Result = string.Empty;
            ArrayList dataAll = new ArrayList();
            dataAll = Select(sql, null);
            if (dataAll.Count > 0)
            {
                Hashtable record = (Hashtable)dataAll[0];
                Result = record["COL1"] == null ? "" : record["COL1"].ToString();
            }
            return Result;
        }

        #region 私有
        private static void SetArgs(string sql, Hashtable args, IDbCommand cmd)
        {
            if (dbType == "MySql")
            {
                MatchCollection ms = Regex.Matches(sql, @"@\w+");
                foreach (Match m in ms)
                {
                    string key = m.Value;
                    string newKey = "?" + key.Substring(1);
                    sql = sql.Replace(key, newKey);

                    Object value = args[key];
                    if (value == null)
                    {
                        value = args[key.Substring(1)];
                    }

                    //cmd.Parameters.Add(new MySqlParameter(newKey, value));
                }
                cmd.CommandText = sql;
            }
            else if (dbType == "SqlServer")
            {
                MatchCollection ms = Regex.Matches(sql, @"@\w+");
                foreach (Match m in ms)
                {
                    string key = m.Value;

                    Object value = args[key];
                    if (value == null)
                    {
                        value = args[key.Substring(1)];
                    }
                    if (value == null) value = DBNull.Value;

                    cmd.Parameters.Add(new SqlParameter(key, value));
                }
                cmd.CommandText = sql;
            }
        }
        private static ArrayList DataTable2ArrayList(DataTable data)
        {
            ArrayList array = new ArrayList();

            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];

                Hashtable record = new Hashtable();
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    object cellValue = row[j];
                    if (cellValue.GetType() == typeof(DBNull))
                    {
                        cellValue = null;
                    }
                    record[data.Columns[j].ColumnName] = cellValue;
                }
                array.Add(record);
            }

            return array;
        }

        #endregion

    }
}
