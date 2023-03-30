using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Text;


namespace aaproject
{
    /// <summary>
    /// AppDataUpdateCache holds generic DataTable and performs SQL Bulk insert 
    /// of same at the specified number of rows interval.
    /// </summary>
    public sealed class CacheEngine
    {
        #region ctor
        public CacheEngine(DataTable dt, string selectStoredProc, string insertStoredProc, string deleteStoredProc, string updateStoredProc, string connectionString, int numRowsPerUpdate)
        {
            this._cacheTable = dt;
            this._connectionString = connectionString;
            this._selectStoredProc = selectStoredProc;
            this._insertStoredProc = insertStoredProc;
            this._deleteStoredProc = deleteStoredProc;
            this._updateStoredProc = updateStoredProc;
            this._numRowsPerUpdate = numRowsPerUpdate;

            Init();
        }
        public CacheEngine(DataTable dt, string selectStoredProc, string insertStoredProc, string deleteStoredProc, string updateStoredProc, string connectionString, int numRowsPerUpdate, string TableName)
        {
            this._cacheTable = dt;
            this._connectionString = connectionString;
            this._selectStoredProc = selectStoredProc;
            this._insertStoredProc = insertStoredProc;
            this._deleteStoredProc = deleteStoredProc;
            this._updateStoredProc = updateStoredProc;
            this._numRowsPerUpdate = numRowsPerUpdate;
            this._TableName = TableName;
            createDynamicProcedure();
            Init();
        }

        #endregion

        #region Create dynamic Procedure
        private void createDynamicProcedure()
        {
            createInsertProcedure();
            createSelectProcedure();
        }

        private void createInsertProcedure()
        {
            
            SqlCommand cmdSqlCommand = new SqlCommand();
            SqlParameter para1;
            SqlConnection conSqlConnection = new SqlConnection(_connectionString);
            conSqlConnection.Open();
            cmdSqlCommand.CommandType = CommandType.StoredProcedure;
            cmdSqlCommand.Connection = conSqlConnection;
            cmdSqlCommand.CommandText = "pr__SYS_MakeInsertRecordProc1";

            para1 = new SqlParameter();
            para1.ParameterName = "@sTableName";
            para1.Value = TableName;
            cmdSqlCommand.Parameters.Add(para1);

            para1 = new SqlParameter();
            para1.ParameterName = "@bExecute";
            para1.Value = 1;
            cmdSqlCommand.Parameters.Add(para1);

            cmdSqlCommand.ExecuteNonQuery();
            conSqlConnection.Close();
        }
        private void createSelectProcedure()
        {
            SqlCommand cmdSqlCommand = new SqlCommand();
            SqlParameter para1;
            SqlConnection conSqlConnection = new SqlConnection(_connectionString);
            conSqlConnection.Open();
            cmdSqlCommand.CommandType = CommandType.StoredProcedure;
            cmdSqlCommand.Connection = conSqlConnection;
            cmdSqlCommand.CommandText = "pr__SYS_MakeSelectRecordProc1";

            para1 = new SqlParameter();
            para1.ParameterName = "@sTableName";
            para1.Value = TableName;
            cmdSqlCommand.Parameters.Add(para1);

            para1 = new SqlParameter();
            para1.ParameterName = "@bExecute";
            para1.Value = 1;
            cmdSqlCommand.Parameters.Add(para1);

            cmdSqlCommand.ExecuteNonQuery();
            conSqlConnection.Close();
        }
        private string getInsertProcedure()
        {
            StringBuilder strInsertProcedure = new StringBuilder();

            strInsertProcedure.Append(" if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[usp_InsertAppDataText]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) ");
            strInsertProcedure.Append(" drop procedure [dbo].[usp_InsertAppDataText] ");
            strInsertProcedure.Append(" GO ");
            strInsertProcedure.Append(" SET QUOTED_IDENTIFIER OFF ");
            strInsertProcedure.Append(" GO ");
            strInsertProcedure.Append(" SET ANSI_NULLS OFF ");
            strInsertProcedure.Append(" GO ");
            strInsertProcedure.Append(" CREATE PROCEDURE usp_InsertAppDataText ");

            //variable name and datatype
            for (int k = 0; k < _cacheTable.Columns.Count; k++)
            {
                string columnName = _cacheTable.Columns[k].ColumnName.ToString();
                string ColumnType = _cacheTable.Columns[k].DataType.ToString();
                if ((ColumnType == "System.Int16") ||
                (ColumnType == "System.Int32") ||
                (ColumnType == "System.Int64") ||
                (ColumnType == "System.int"))
                {
                    ColumnType = "int";
                }
                else if (ColumnType == "System.String")
                {
                    ColumnType = "varchar";
                }
                else if (ColumnType == "System.Byte")
                {
                    ColumnType = "varchar";
                }
                else if (ColumnType == "System.DateTime")
                {
                    ColumnType = "datetime";
                }
                else if (ColumnType == "System.Text")
                {
                    ColumnType = "text";
                }
                else
                {

                }


                strInsertProcedure.Append("@" + _cacheTable.Columns[k].ColumnName + " " + _cacheTable.Columns[k].DataType);
                strInsertProcedure.Append(",");
            }

            strInsertProcedure.Append(" AS ");

            strInsertProcedure.Append(" INSERT INTO " + _TableName + "(");
            //fieldName
            for (int k = 0; k < _cacheTable.Columns.Count; k++)
            {
                strInsertProcedure.Append(_cacheTable.Columns[k].ColumnName);
                strInsertProcedure.Append(",");
            }

            strInsertProcedure.Append(" )");
            strInsertProcedure.Append(" VALUES (");
            //variable name
            for (int k = 0; k < _cacheTable.Columns.Count; k++)
            {
                strInsertProcedure.Append("@" + _cacheTable.Columns[k].ColumnName);
                strInsertProcedure.Append(",");
            }
            strInsertProcedure.Append(" )");
            return strInsertProcedure.ToString();
        }

        #endregion

        #region public fields / properties
        private DataTable _cacheTable;
        private DataSet ds = new DataSet();
        public DataTable CacheTable
        {
            get
            {
                return _cacheTable;
            }
            set
            {
                _cacheTable = value;
            }
        }

        private string _connectionString;
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }
        private string _selectStoredProc;
        public string SelectStoredProc
        {
            get
            {
                return _selectStoredProc;
            }
            set
            {
                _selectStoredProc = value;
            }
        }
        private string _deleteStoredProc;
        public string DeleteStoredProc
        {
            get
            {
                return _deleteStoredProc;
            }
            set
            {
                _deleteStoredProc = value;
            }
        }

        private string _updateStoredProc;
        public string UpdateStoredProc
        {
            get
            {
                return _updateStoredProc;
            }
            set
            {
                _updateStoredProc = value;
            }
        }
        private string _insertStoredProc;
        public string InsertStoredProc
        {
            get
            {
                return _insertStoredProc;
            }
            set
            {
                _insertStoredProc = value;
            }
        }

        private int _numRowsPerUpdate = 250;
        public int NumRowsPerUpdate
        {
            get
            {
                return _numRowsPerUpdate;
            }
            set
            {
                _numRowsPerUpdate = value;
            }
        }

        //for Table Name
        private string _TableName;
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }

        public void AddItem(object[] objItemArray)
        {
            if (this._connectionString == null || this._connectionString == "")
                throw new InvalidOperationException("All properties must be set to add an item.");
            DataRow row = this._cacheTable.NewRow();
            row.ItemArray = objItemArray;
            this._cacheTable.Rows.Add(row);
        }
        #endregion

        #region private members
        private void Init()
        {

            DataSet newDs = new DataSet();
            this.ds = newDs;
            try
            {
                this._cacheTable.TableName = _TableName;
                this.ds.Tables.Add(_cacheTable);
                this._cacheTable.RowChanged += new DataRowChangeEventHandler(Row_Changed);
            }
            catch
            {
                _cacheTable.Clear();
            }
        }


        private void Row_Changed(object sender, DataRowChangeEventArgs e)
        {
            if (((DataTable)sender).Rows.Count >= this._numRowsPerUpdate)
            {
                BulkInsertData();
            }
        }

        private void BulkInsertData()
        {
            this._cacheTable.RowChanged -= new DataRowChangeEventHandler(Row_Changed);
            try
            {
                DataSet ds2 = new DataSet();
                DataTable tbl;
                lock (_cacheTable)
                {
                    tbl = this._cacheTable.Copy();
                }
                tbl.TableName = TableName;
                ds2.Tables.Add(tbl);
                DaHandler.SubmitChanges(ref ds2, this._connectionString,
                    this._selectStoredProc, this._updateStoredProc,
                    this._insertStoredProc, this._deleteStoredProc);
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Debug.Write(Ex.Message + Ex.StackTrace);
            }
            finally
            {
                _cacheTable.Clear();
                this._cacheTable.RowChanged += new DataRowChangeEventHandler(Row_Changed);
            }
        }
        #endregion
    }
}