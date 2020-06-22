using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Amo.Lib.DataBase
{
    public abstract class MssqlHelper
    {
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

        public static async Task<int> ExecuteNonQueryAsync(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = await cmd.ExecuteNonQueryAsync();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = await cmd.ExecuteNonQueryAsync();
            cmd.Parameters.Clear();
            return val;
        }

        public static async Task<int> ExecuteNonQueryAsync(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = await cmd.ExecuteNonQueryAsync();
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

        /// <summary>
        /// for long time query....
        /// </summary>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="timeout">the timeout of connection</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, int timeout, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to
            // close the connection throw code, because no datareader will exist, hence the
            // commandBehaviour.CloseConnection will not work
            try
            {
                cmd.CommandTimeout = timeout;
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

        public static async Task<SqlDataReader> ExecuteReaderAsync(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to
            // close the connection throw code, because no datareader will exist, hence the
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public static async Task<SqlDataReader> ExecuteReaderAsync(string connectionString, CommandType cmdType, string cmdText, int timeout, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to
            // close the connection throw code, because no datareader will exist, hence the
            // commandBehaviour.CloseConnection will not work
            try
            {
                cmd.CommandTimeout = timeout;
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
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

        public static async Task<object> ExecuteScalarAsync(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = await cmd.ExecuteScalarAsync();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static async Task<object> ExecuteScalarAsync(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute Dataset
        /// </summary>
        /// <param name="connectionString">an existing database connection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
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

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="connString">connString</param>
        /// <param name="sqlString">sqlString</param>
        /// <param name="cmdParms">cmdParms</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string connString, string sqlString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, CommandType.Text, sqlString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    return ds;
                }
            }
        }

        /// <summary>
        /// Get DB String
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static string GetDBString(SqlDataReader rdr, string fieldname)
        {
            string s = string.Empty;
            int ordinal = 0;
            ordinal = rdr.GetOrdinal(fieldname);
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetString(ordinal);
            }

            return s;
        }

        /// <summary>
        /// Get DB String
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field index</param>
        /// <returns>Field Value</returns>
        public static string GetDBString(SqlDataReader rdr, int ordinal)
        {
            string s = string.Empty;
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetString(ordinal);
            }

            return s;
        }

        /// <summary>
        /// Get DB Int
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field index</param>
        /// <returns>Field Value</returns>
        public static int GetDBInt(SqlDataReader rdr, int ordinal)
        {
            int s = 0;
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetInt32(ordinal);
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
            int ordinal = 0;
            ordinal = rdr.GetOrdinal(fieldname);
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetInt32(ordinal);
            }

            return s;
        }

        /// <summary>
        /// Get DB SmallInt
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static int GetDBSmallInt(SqlDataReader rdr, string fieldname)
        {
            int s = 0;
            int ordinal = 0;
            ordinal = rdr.GetOrdinal(fieldname);
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetInt16(ordinal);
            }

            return s;
        }

        /// <summary>
        /// Get DB Short
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field index</param>
        /// <returns>Field Value</returns>
        public static int GetDBShort(SqlDataReader rdr, int ordinal)
        {
            int s = 0;
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetInt16(ordinal);
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
            int ordinal = 0;
            ordinal = rdr.GetOrdinal(fieldname);
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetInt16(ordinal);
            }

            return s;
        }

        /// <summary>
        /// Get DB Double
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field index</param>
        /// <returns>Field Value</returns>
        public static double GetDBDouble(SqlDataReader rdr, int ordinal)
        {
            double s = 0d;
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetDouble(ordinal);
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
            int ordinal = 0;
            ordinal = rdr.GetOrdinal(fieldname);
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetDouble(ordinal);
            }

            return s;
        }

        /// <summary>
        /// Get DB Decimal
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field Name</param>
        /// <returns>Field Value</returns>
        public static decimal GetDBDecimal(SqlDataReader rdr, int ordinal)
        {
            decimal s = 0m;
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetDecimal(ordinal);
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
            int ordinal = 0;
            ordinal = rdr.GetOrdinal(fieldname);
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetDecimal(ordinal);
            }

            return s;
        }

        /// <summary>
        /// Get DB Bool
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field Name</param>
        /// <returns>Field Value</returns>
        public static bool GetDBBool(SqlDataReader rdr, int ordinal)
        {
            bool s = false;
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetBoolean(ordinal);
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
            int ordinal = 0;
            ordinal = rdr.GetOrdinal(fieldname);
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetBoolean(ordinal);
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
            int ordinal = 0;
            ordinal = rdr.GetOrdinal(fieldname);
            if (!rdr.IsDBNull(ordinal))
            {
                s = (DateTime)rdr.GetSqlDateTime(ordinal);
            }

            return s;
        }

        /// <summary>
        /// Get DB GUID
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">ordinal</param>
        /// <returns>Field Value</returns>
        public static string GetDBGUID(SqlDataReader rdr, int ordinal)
        {
            string s = string.Empty;
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetSqlGuid(ordinal).ToString();
            }

            return s;
        }

        /// <summary>
        /// Get DB GUID
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="fieldname">Field Name</param>
        /// <returns>Field Value</returns>
        public static string GetDBGUID(SqlDataReader rdr, string fieldname)
        {
            string s = string.Empty;
            int ordinal = 0;
            ordinal = rdr.GetOrdinal(fieldname);
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetSqlGuid(ordinal).ToString();
            }

            return s;
        }

        /// <summary>
        /// Get DB Int
        /// </summary>
        /// <param name="rdr">SqlDataReader</param>
        /// <param name="ordinal">Field Name</param>
        /// <returns>Field Value</returns>
        public static long GetDBInt64(SqlDataReader rdr, int ordinal)
        {
            long s = 0;
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetInt64(ordinal);
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
            int ordinal = 0;
            ordinal = rdr.GetOrdinal(fieldname);
            if (!rdr.IsDBNull(ordinal))
            {
                s = rdr.GetInt64(ordinal);
            }

            return s;
        }

        /// <summary>
        /// 创建参数体
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">长度</param>
        /// <param name="direction">类型</param>
        /// <param name="value">值</param>
        /// <returns>参数体</returns>
        public static SqlParameter MakeParam(string paramName, SqlDbType dbType, int size, ParameterDirection direction, object value)
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

        /// <summary>
        /// 创建输入参数体
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">长度</param>
        /// <param name="value">值</param>
        /// <returns>参数体</returns>
        public static SqlParameter MakeInParam(string paramName, SqlDbType dbType, int size, object value)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Input, value);
        }

        /// <summary>
        /// 创建输入参数体
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="value">值</param>
        /// <returns>参数体</returns>
        public static SqlParameter MakeInParam(string paramName, SqlDbType dbType, object value)
        {
            return MakeParam(paramName, dbType, 0, ParameterDirection.Input, value);
        }

        /// <summary>
        /// 创建输出参数体
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">长度</param>
        /// <param name="value">值</param>
        /// <returns>参数体</returns>
        public static SqlParameter MakeOutParam(string paramName, SqlDbType dbType, int size, object value)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Output, value);
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
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = 600;
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
    }
}
