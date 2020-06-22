using System;
using System.Collections;
using System.Data;
using SqlCommand = MySql.Data.MySqlClient.MySqlCommand;
using SqlConnection = MySql.Data.MySqlClient.MySqlConnection;
using SqlDataAdapter = MySql.Data.MySqlClient.MySqlDataAdapter;
using SqlDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using SqlDbType = MySql.Data.MySqlClient.MySqlDbType;
using SqlParameter = MySql.Data.MySqlClient.MySqlParameter;
using SqlTransaction = MySql.Data.MySqlClient.MySqlTransaction;

namespace Amo.Lib.DataBase
{
    /// <summary>
    /// MySql没有Guid,所以Guid的东西不能有,其他都可以保留,然后映射引用名
    /// </summary>
    public class MySqlHelper
    {
        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static long ExecuteInsert(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return cmd.LastInsertedId;
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against an existing database connection
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">an existing database connection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing sql transaction</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to
            // close the connection throw code, because no datareader will exist, hence the
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, int timeout, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to
            // close the connection throw code, because no datareader will exist, hence the
            // commandBehaviour.CloseConnection will not work
            try
            {
                cmd.CommandTimeout = 0;
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">an existing database connection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);

                // Create the DataAdapter & DataSet
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();

                    // Fill the DataSet using default values for DataTable names, etc
                    da.FillSchema(ds, SchemaType.Source);
                    da.Fill(ds);

                    // Detach the SqlParameters from the command object, so they can be used again
                    cmd.Parameters.Clear();

                    // Return the dataset
                    return ds;
                }
            }
        }

        public static DataSet Query(string connString, string sQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, CommandType.Text, sQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    return ds;
                }
            }
        }

        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] cmdParms)
        {
            parmCache[cacheKey] = cmdParms;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
            {
                return null;
            }

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
            {
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();
            }

            return clonedParms;
        }

        #region Get DB String

        /// <summary>
        /// Get DB String
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static string GetDBString(SqlDataReader rdr, string fieldname)
        {
            string s = string.Empty;
            try
            {
                int ordinal = 0;
                ordinal = rdr.GetOrdinal(fieldname);
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetString(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        /// <summary>
        /// Get DB String
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field Index</param>
        /// <returns>Field Value</returns>
        public static string GetDBString(SqlDataReader rdr, int ordinal)
        {
            string s = string.Empty;
            try
            {
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetString(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }
        #endregion

        #region Get DB Int

        /// <summary>
        /// Get DB Int
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field Index</param>
        /// <returns>Field Value</returns>
        public static int GetDBInt(SqlDataReader rdr, int ordinal)
        {
            int s = 0;
            try
            {
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetInt32(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        /// <summary>
        /// Get DB Int
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static int GetDBInt(SqlDataReader rdr, string fieldname)
        {
            int s = 0;
            try
            {
                int ordinal = 0;
                ordinal = rdr.GetOrdinal(fieldname);
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetInt32(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        /// <summary>
        /// Get DB SmallInt
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">File Name</param>
        /// <returns>Values</returns>
        public static int GetDBSmallInt(SqlDataReader rdr, string fieldname)
        {
            int s = 0;
            try
            {
                int ordinal = 0;
                ordinal = rdr.GetOrdinal(fieldname);
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetInt16(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }
        #endregion

        #region Get DB Short

        /// <summary>
        /// Get DB Short
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field Index</param>
        /// <returns>Field Value</returns>
        public static int GetDBShort(SqlDataReader rdr, int ordinal)
        {
            int s = 0;
            try
            {
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetInt16(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        /// <summary>
        /// Get DB Short
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static int GetDBShort(SqlDataReader rdr, string fieldname)
        {
            int s = 0;
            try
            {
                int ordinal = 0;
                ordinal = rdr.GetOrdinal(fieldname);
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetInt16(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }
        #endregion

        #region Get DB Double

        /// <summary>
        /// Get DB Double
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field Index</param>
        /// <returns>Field Value</returns>
        public static double GetDBDouble(SqlDataReader rdr, int ordinal)
        {
            double s = 0d;
            try
            {
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetDouble(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        /// <summary>
        /// Get DB Double
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static double GetDBDouble(SqlDataReader rdr, string fieldname)
        {
            double s = 0d;
            try
            {
                int ordinal = 0;
                ordinal = rdr.GetOrdinal(fieldname);
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetDouble(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }
        #endregion

        #region Get Db Decimal

        /// <summary>
        /// Get DB Decimal
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field Index</param>
        /// <returns>Field Value</returns>
        public static decimal GetDBDecimal(SqlDataReader rdr, int ordinal)
        {
            decimal s = 0m;
            try
            {
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetDecimal(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        /// <summary>
        /// Get DB Decimal
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static decimal GetDBDecimal(SqlDataReader rdr, string fieldname)
        {
            decimal s = 0m;
            try
            {
                int ordinal = 0;
                ordinal = rdr.GetOrdinal(fieldname);
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetDecimal(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }
        #endregion

        #region Get DB Bool

        /// <summary>
        /// Get DB Bool
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field Indx</param>
        /// <returns>Field Value</returns>
        public static bool GetDBBool(SqlDataReader rdr, int ordinal)
        {
            bool s = false;
            try
            {
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetBoolean(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        /// <summary>
        /// Get DB Bool
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static bool GetDBBool(SqlDataReader rdr, string fieldname)
        {
            bool s = false;
            try
            {
                int ordinal = 0;
                ordinal = rdr.GetOrdinal(fieldname);
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetBoolean(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        /// <summary>
        /// Get DB Date
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static DateTime GetDBDateTime(SqlDataReader rdr, string fieldname)
        {
            DateTime s = default(DateTime);
            try
            {
                int ordinal = 0;
                ordinal = rdr.GetOrdinal(fieldname);
                if (!rdr.IsDBNull(ordinal))
                {
                    s = (DateTime)rdr.GetMySqlDateTime(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }
        #endregion

        #region Get DB Image

        /// <summary>
        /// Get DB Image
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static byte[] GetDBImage(SqlDataReader rdr, string fieldname)
        {
            byte[] s = new byte[] { };
            try
            {
                int ordinal = 0;
                ordinal = rdr.GetOrdinal(fieldname);
                if (!rdr.IsDBNull(ordinal))
                {
                    s = (byte[])rdr[ordinal];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        /// <summary>
        /// Get DB String
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field Index</param>
        /// <returns>Field Value</returns>
        public static byte[] GetDBImage(SqlDataReader rdr, int ordinal)
        {
            byte[] s = new byte[] { };
            try
            {
                if (!rdr.IsDBNull(ordinal))
                {
                    s = (byte[])rdr[ordinal];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }
        #endregion

        #region Get DB Int64

        /// <summary>
        /// Get DB Int
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field Name</param>
        /// <returns>Field Value</returns>
        public static long GetDBInt64(SqlDataReader rdr, int ordinal)
        {
            long s = 0;
            try
            {
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetInt64(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        /// <summary>
        /// Get DB Int
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static long GetDBInt64(SqlDataReader rdr, string fieldname)
        {
            long s = 0;
            try
            {
                int ordinal = 0;
                ordinal = rdr.GetOrdinal(fieldname);
                if (!rdr.IsDBNull(ordinal))
                {
                    s = rdr.GetInt64(ordinal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        #endregion

        #region SqlParam

        public static SqlParameter MakeInParam(string paramName, SqlDbType dbType, int size, object value)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Input, value);
        }

        public static SqlParameter MakeInParam(string paramName, SqlDbType dbType, object value)
        {
            return MakeParam(paramName, dbType, 0, ParameterDirection.Input, value);
        }

        #endregion

        public static string LinkedDataSource(string connectionLocal, string connectionLinked)
        {
            string connStr = string.Empty;

            var connStrA = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connectionLocal);
            var connStrB = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connectionLinked);

            // 跨服务器
            if (connStrA.Server != connStrB.Server)
            {
                connStr = connStr + "`" + connStrB.Server + "`.";
            }

            // 跨数据库
            if (connStrA.Database != connStrB.Database || connStrA.Server != connStrB.Server)
            {
                connStr = connStr + "`" + connStrB.Database + "`.";
            }

            return connStr;
        }

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandTimeout = 0;
                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static SqlParameter MakeParam(string paramName, SqlDbType dbType, int size, ParameterDirection direction, object value)
        {
            SqlParameter param;
            if (size > 0)
            {
                param = new SqlParameter(paramName, dbType, size);
            }
            else
            {
                param = new SqlParameter(paramName, dbType);
            }

            param.Direction = direction;
            if (!(direction == ParameterDirection.Output && value == null))
            {
                param.Value = value == null ? DBNull.Value : value;
            }

            return param;
        }
    }
}
