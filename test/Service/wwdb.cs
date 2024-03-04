using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;

namespace test.Service
{
    public class wwdb
    {
        private DataTable _Datatable;
        private int timeOut = 14400;

        public DataTable Datatable
        {
            get { return _Datatable; }
            set { _Datatable = value; }
        }
        private OleDbCommand Cmd;

        public OleDbCommand SQLCmd
        {
            get { return Cmd; }
            set { Cmd = value; }
        }
        private OleDbConnection conn;
        public string ErrorMessage;
        public bool HasError = false;
        private int iRow = 0;
        private OleDbTransaction Trans;
        public bool UseTransaction = false;
        public int SqlErrorCode = 0;



        public wwdb()
        {
            String connectionString = "Provider=SQLOLEDB;Data Source=202.157.183.113,1533;Initial Catalog=PersonalColl_LiveClone_20240122;User Id=PC_SystemUser;Password=PC_User@13579;";

            try
            {
                this.HasError = false;
                this.conn = new OleDbConnection(connectionString);
                this.conn.Open();
            }
            catch (Exception exception)
            {
                this.HasError = true;
                this.ErrorMessage = exception.Message;
            }
        }

        public void BeginTransaction()
        {
            this.Trans = this.conn.BeginTransaction(IsolationLevel.ReadCommitted);
            this.UseTransaction = true;
        }

        public bool Bof()
        {
            return (this.iRow == 0);
        }

        public void Close()
        {
            if (this.conn != null)
            {
                if (this.conn.State == System.Data.ConnectionState.Open)
                {
                    this.conn.Close();
                    this.conn.Dispose();
                    this.conn = null;
                }
            }
        }

        public void CommitTransaction()
        {
            this.Trans.Commit();
            this.UseTransaction = false;
        }

        public DataTable Datasource()
        {
            return this._Datatable;
        }

        public bool Eof()
        {
            try
            {
                return (this.iRow == this._Datatable.Rows.Count);
            }
            catch (Exception)
            {
                return true;
            }
        }

        public bool Execute(string strSQL, List<string> parameterValues)
        {
            this.Cmd = new OleDbCommand();

            if (this.UseTransaction)
            {
                this.Cmd.Transaction = this.Trans;
            }

            this.Cmd.CommandText = strSQL;
            this.Cmd.CommandTimeout = timeOut;
            this.Cmd.Connection = this.conn;
            this.HasError = false;

            if (parameterValues != null)
            {
                for (int i = 0; i < parameterValues.Count; i++)
                {
                    this.Cmd.Parameters.AddWithValue($"param{i + 1}", parameterValues[i]);
                }
            }

            try
            {
                this.Cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                this.SqlErrorCode = sqlEx.Number;
                //TextFileLogger.LogToFile("wwdb - Execute - Message", sqlEx.Message, true);
                //TextFileLogger.LogToFile("wwdb - Execute - StackTrace", sqlEx.StackTrace, true);
                //TextFileLogger.LogToFile("wwdb - Execute - SQLQuery", strSQL, true);

                this.HasError = true;
                this.ErrorMessage = sqlEx.Message;
            }
            catch (Exception exception)
            {
                //TextFileLogger.LogToFile("wwdb - Execute - Message", exception.Message, true);
                //TextFileLogger.LogToFile("wwdb - Execute - StackTrace", exception.StackTrace, true);
                //TextFileLogger.LogToFile("wwdb - Execute - SQLQuery", strSQL, true);
                this.HasError = true;
                this.ErrorMessage = exception.Message;
            }

            this.Cmd = null;
            return !this.HasError;

        }

        public bool Execute(string strSQL)
        {
            return this.Execute(strSQL, null);
        }

        public bool Execute(StringBuilder builder)
        {
            return this.Execute(builder.query, builder.param);
        }

        public object executeScalarSP(string strSQL, List<string> parameterValues)
        {
            object obj2 = null;
            this.Cmd = new OleDbCommand();
            if (this.UseTransaction)
            {
                this.Cmd.Transaction = this.Trans;
            }
            this.Cmd.CommandText = strSQL;
            this.Cmd.CommandTimeout = timeOut;
            this.Cmd.CommandType = CommandType.StoredProcedure;
            this.Cmd.Connection = this.conn;
            {
                for (int i = 0; i < parameterValues.Count; i++)
                {
                    this.Cmd.Parameters.AddWithValue($"param{i + 1}", parameterValues[i]);
                }
            }
            this.HasError = false;
            try
            {
                obj2 = this.Cmd.ExecuteScalar();
            }
            catch (SqlException sqlEx)
            {
                this.SqlErrorCode = sqlEx.Number;
                //TextFileLogger.LogToFile("wwdb - Execute - Message", sqlEx.Message, true);
                //TextFileLogger.LogToFile("wwdb - Execute - StackTrace", sqlEx.StackTrace, true);
                //TextFileLogger.LogToFile("wwdb - Execute - SQLQuery", constructParamsToSQL(strSQL, sqlParams), true);
                this.HasError = true;
                this.ErrorMessage = sqlEx.Message;
            }
            catch (Exception exception)
            {
                //TextFileLogger.LogToFile("wwdb - Execute - Message", exception.Message, true);
                //TextFileLogger.LogToFile("wwdb - Execute - StackTrace", exception.StackTrace, true);
                //TextFileLogger.LogToFile("wwdb - Execute - SQLQuery", constructParamsToSQL(strSQL, sqlParams), true);
                this.HasError = true;
                this.ErrorMessage = exception.Message;
            }
            this.Cmd = null;
            return obj2;
        }

        public object executeScalarSP(string strSQL)
        {
            return this.executeScalarSP(strSQL, null);
        }
        public object executeScalarSP(StringBuilder builder)
        {
            return this.executeScalarSP(builder.query, builder.param);
        }
        public void First()
        {
            this.iRow = 0;
        }

        public DataTable GetDataTable(string strSQL, List<string> parameterValues)
        {
            try
            {
                OpenTable(strSQL, parameterValues);
                return this._Datatable;
            }
            catch (Exception exception)
            {
                //TextFileLogger.LogToFile("wwdb - getDataTable - Message", exception.Message, true);
                //TextFileLogger.LogToFile("wwdb - getDataTable - StackTrace", exception.StackTrace, true);
                //TextFileLogger.LogToFile("wwdb - getDataTable - SQLQuery", strSQL, true);
                this.HasError = true;
                this.ErrorMessage = exception.Message;
            }
            return null;
        }
        public DataTable GetDataTable(string strSQL)
        {
            return this.GetDataTable(strSQL, null);
        }
        public DataTable GetDataTable(StringBuilder builder)
        {
            return this.GetDataTable(builder.query, builder.param);
        }

        public string Item(string sItem)
        {
            try
            {
                return this._Datatable.Rows[this.iRow][sItem].ToString().Trim();
            }
            catch (Exception exception)
            {
                //TextFileLogger.LogToFile("wwdb - Item - Message", exception.Message, true);
                //TextFileLogger.LogToFile("wwdb - Item - StackTrace", exception.StackTrace, true);
                //TextFileLogger.LogToFile("wwdb - Item - Item", sItem, true);
                return "";
            }
        }

        public DataRow get(int index)
        {
            try
            {
                return this._Datatable.Rows[index];
            }
            catch (Exception exception)
            {
                //TextFileLogger.LogToFile("wwdb - Item - Message", exception.Message, true);
                //TextFileLogger.LogToFile("wwdb - Item - StackTrace", exception.StackTrace, true);
                //TextFileLogger.LogToFile("wwdb - Item - Item", sItem, true);
                return null;
            }
        }

        public void Last()
        {
            this.iRow = this._Datatable.Rows.Count - 1;
        }

        public void MoveNext()
        {
            this.iRow++;
        }

        public void MovePrevious()
        {
            if (this.iRow != 0)
            {
                this.iRow--;
            }
        }

        public void OpenTable(string strsql, List<string> parameterValues)
        {
            this.iRow = 0;
            this._Datatable = null;
            this._Datatable = new DataTable();
            try
            {
                this.HasError = false;
                using (this.Cmd = new OleDbCommand(strsql, conn))
                {

                    if (parameterValues != null)
                    {
                        for (int i = 0; i < parameterValues.Count; i++)
                        {
                            this.Cmd.Parameters.AddWithValue($"param{i + 1}", parameterValues[i]);
                        }
                    }

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(this.Cmd))
                    {
                        adapter.Fill(this._Datatable);
                    }
                }
            }
            catch (Exception exception)
            {
                //TextFileLogger.LogToFile("wwdb - OpenTable - Message", exception.Message, true);
                //TextFileLogger.LogToFile("wwdb - OpenTable - StackTrace", exception.StackTrace, true);
                //TextFileLogger.LogToFile("wwdb - OpenTable - SQLQuery", strsql, true);
                this.HasError = true;
                this.ErrorMessage = exception.Message;
            }
        }
        public void OpenTable(string strsql)
        {
            this.OpenTable(strsql, null);
        }
        public void OpenTable(StringBuilder builder)
        {
            this.OpenTable(builder.query, builder.param);
        }

        public int RecordCount()
        {
            return this._Datatable.Rows.Count;
        }

        public void Rollback()
        {
            this.Trans.Rollback();
            this.UseTransaction = false;
        }
    }
}
