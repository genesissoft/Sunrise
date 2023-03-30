using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;


namespace aaproject
{
    public class CommonFunction
    {
        clsCommonFuns objcommon = new clsCommonFuns();
        public CommonFunction()
        {
        }

        public string checkquotes(string inputString)
        {
            string parsestring = inputString.Replace("'", "''");
            return parsestring;
        }

        public string CheckSpace(string inputString)
        {
            string strOutput;// to use this add namespace text.regualarexpression
            strOutput = Regex.Replace(inputString, "\\s+", " ");
            return strOutput;
        }
        //CommonFunction objcomm = new CommonFunction(); 

        //public ReportDocument getReport(string ReportName)
        //{
        //    ReportDocument crDc = new ReportDocument();
        //    string strReportPath = ConfigurationManager.AppSettings["ReportsPath"].ToString();
        //    crDc.Load(strReportPath + ReportName);

        //    TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
        //    TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
        //    ConnectionInfo crConnectionInfo = new ConnectionInfo();
        //    Tables CrTables;

        //    crConnectionInfo.ServerName = ConfigurationManager.AppSettings["DBServerName"].ToString();
        //    crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["DatabaseName"].ToString();
        //    crConnectionInfo.UserID = ConfigurationManager.AppSettings["DBUserID"].ToString();
        //    crConnectionInfo.Password = ConfigurationManager.AppSettings["DBPassword"].ToString();

        //    CrTables = crDc.Database.Tables;

        //    foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
        //    {
        //        crtableLogoninfo = CrTable.LogOnInfo;
        //        crtableLogoninfo.ConnectionInfo = crConnectionInfo;
        //        CrTable.ApplyLogOnInfo(crtableLogoninfo);
        //    }
        //    return crDc;
        //}
        //public bool ValidateDate(string strdate, CultureInfo culture)
        //{
        //    try
        //    {
        //        StringBuilder str = new StringBuilder();
        //        str.Append(strdate);
        //        if(Convert.ToInt32(str.Length)<10)
        //        {
        //            if(str[2]!= '/')
        //            {
        //                str =  str.Insert(0,"0");
        //            }
        //            if(str[5]!='/')
        //            {
        //                str = str.Insert(3,"0");
        //            }
        //            if(Convert.ToInt32(str.Length) != 10)
        //            {
        //                str = str.Insert(7,"19");
        //            }
        //            if(Convert.ToInt32(str.Length)>10)
        //            {
        //                return false;
        //            }
        //        }
        //        System.Globalization.CultureInfo dtfi = new CultureInfo(culture.Name);
        //        DateTime dt = DateTime.ParseExact(str.ToString(), "d", dtfi.DateTimeFormat);
        //    }
        //    catch(Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public bool CheckIsAlphaNumeric(string strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[^a-z A-Z0-9]");
            return !objAlphaNumericPattern.IsMatch(strToCheck);
            //return true;
        }

        public bool CheckIsNumeric(string strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[^0-9]");
            return !objAlphaNumericPattern.IsMatch(strToCheck);
            //return true;
        }

        public bool CheckIsPositiveNumber(string strNumber)
        {
            Regex objNotPositivePattern = new Regex("[^0-9.]");
            Regex objPositivePattern = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");

            return !objNotPositivePattern.IsMatch(strNumber) &&
                objPositivePattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber);
        }
        public void AddControl(TextBox strTxtName, ListBox LstBoxName, Button strButtonVis)
        {
            if ((CheckSpace(strTxtName.Text.Trim()) != "") && (CheckSpace(strTxtName.Text.Trim()) != " "))
            {
                for (int i = 0; i < LstBoxName.Items.Count; i++)
                {
                    if (LstBoxName.Items[i].Text == strTxtName.Text.Trim())
                    {
                        LstBoxName.Items.Remove(strTxtName.Text.Trim());
                    }
                }

                LstBoxName.Items.Add(strTxtName.Text.Trim());
            }

            LstBoxName.Visible = true;
            strButtonVis.Visible = true;
            strTxtName.Text = "";
        }
        //		public void AddControl(TextBox strTxtName,ListBox LstBoxName, Button strButtonVis, DataView myDv)
        //		{
        //			if(myDv.Table.Rows.Count > 0)
        //			{
        //				if((CheckSpace(strTxtName.Text.Trim())!= "")&& (CheckSpace(strTxtName.Text.Trim())!= " "))
        //				{
        //					for(int i=0;i<LstBoxName.Items.Count;i++)
        //					{
        //						if(LstBoxName.Items[i].Text==strTxtName.Text.Trim())
        //						{
        //							LstBoxName.Items.Remove(strTxtName.Text.Trim());
        //						}
        //					}
        //
        //					LstBoxName.Items.Add(strTxtName.Text.Trim());
        //				}
        //			}
        //			
        //			LstBoxName.Visible=true;
        //			strButtonVis.Visible=true;
        //			strTxtName.Text="";
        //		}
        //		public void AddDbComboControl(DbCombo strTxtName,ListBox LstBoxName,Button strButtonVis )
        //		{
        //			if((CheckSpace(strTxtName.Text.Trim())!= "")&& (CheckSpace(strTxtName.Text.Trim())!= " "))
        //			{
        //				//if(CheckIsAlphaNumeric(strTxtName.Text.Trim()))
        //				LstBoxName.Items.Add(strTxtName.Text.Trim());
        //			}
        //			
        //			LstBoxName.Visible=true;
        //			strButtonVis.Visible=true;
        //			strTxtName.Text="";
        //
        //		}
        public void AddControlInt(TextBox strTxtName, ListBox LstBoxName, Button strButtonVis)
        {
            if ((CheckSpace(strTxtName.Text.Trim()) != "") && (CheckSpace(strTxtName.Text.Trim()) != " "))
            {
                if (CheckIsNumeric(strTxtName.Text.Trim()))
                    LstBoxName.Items.Add(strTxtName.Text.Trim());
            }

            LstBoxName.Visible = true;
            strButtonVis.Visible = true;
            strTxtName.Text = "";
        }


        public void AddControlEmail(TextBox strTxtName, ListBox LstBoxName, Button strButtonVis)
        {
            if ((CheckSpace(strTxtName.Text.Trim()) != "") && (CheckSpace(strTxtName.Text.Trim()) != " "))
            {

                LstBoxName.Items.Add(strTxtName.Text.Trim());
            }

            LstBoxName.Visible = true;
            strButtonVis.Visible = true;
            strTxtName.Text = "";
        }
        public void showErrorMsg(System.Web.UI.Page page, string message)
        {
            message = message.Replace("'", "''");
            page.RegisterStartupScript("Alert", String.Format("<script DEFER : TRUE LANGUAGE = JavaScript> alert('{0}');</script>", message));

        }
        public void DelControl(TextBox strTxtName, ListBox LstBoxName, Button strButtonVis)
        {
            for (int i = 0; i < LstBoxName.Items.Count; i++)
            {
                if (LstBoxName.Items[i].Selected)
                {
                    LstBoxName.Items.Remove(LstBoxName.Items[i]);
                }
            }
            if (LstBoxName.Items.Count == 0)
            {
                LstBoxName.Visible = false;
                strButtonVis.Visible = false;

            }
        }

        public void ControlVisibleFalse(ListBox lstBoxName, Button strButton)
        {
            lstBoxName.Items.Clear();
            lstBoxName.Visible = false;
            strButton.Visible = false;
        }
        public void SetCommandParameters(SqlCommand oCommand, string Name, SqlDbType DataType, int size, char Direction, object oValue)
        {
            SqlParameter oParam = new SqlParameter();
            if (size == 0)
            {
                oParam = oCommand.Parameters.Add(Name, DataType);
            }
            else
            {
                oParam = oCommand.Parameters.Add(Name, DataType, size);
            }
            if (Direction == 'I')
            {
                oParam.Direction = ParameterDirection.Input;
            }
            if (Direction == 'O')
            {
                oParam.Direction = ParameterDirection.Output;
            }
            if (Direction == 'w')
            {
                oParam.Direction = ParameterDirection.InputOutput;
            }
            if (Direction == 'R')
            {
                oParam.Direction = ParameterDirection.ReturnValue;
            }

            if (Direction == 'I' || Direction == 'w')
            {
                oParam.Value = oValue;
            }
        }
        public void Search(ListBox lstbox, StringBuilder QuerySearch, string FieldName)
        {

            if (lstbox.Items.Count != 0)
            {
                QuerySearch.Append("(");
                for (int i = 0; i < lstbox.Items.Count; i++)
                {

                    QuerySearch.Append(FieldName);
                    QuerySearch.Append("='");
                    QuerySearch.Append(checkquotes(lstbox.Items[i].Value));

                    QuerySearch.Append("' or ");
                }
                QuerySearch.Remove(QuerySearch.Length - 3, 2);
                QuerySearch.Append(")");
                QuerySearch.Append(" and ");

            }
        }
        public void SearchDate(ListBox lstbox, StringBuilder QuerySearch, string FieldName, CultureInfo culture)
        {
            DateValidate obj = new DateValidate();
            if (lstbox.Items.Count != 0)
            {
                QuerySearch.Append("(");
                for (int i = 0; i < lstbox.Items.Count; i++)
                {
                    QuerySearch.Append(FieldName);
                    QuerySearch.Append("='");
                    QuerySearch.Append(obj.convertDate(lstbox.Items[i].Value, culture));
                    QuerySearch.Append("' or ");
                }
                QuerySearch.Remove(QuerySearch.Length - 3, 2);
                QuerySearch.Append(")");
                QuerySearch.Append(" and ");
            }
        }

        public StringBuilder SearchCat(ListBox lstbox, StringBuilder QuerySearch, string FieldName, StringBuilder strCat)
        {
            strCat = new StringBuilder();
            if (lstbox.Items.Count != 0)
            {
                QuerySearch.Append("(");
                for (int i = 0; i < lstbox.Items.Count; i++)
                {

                    QuerySearch.Append(FieldName);
                    QuerySearch.Append("='");
                    QuerySearch.Append(checkquotes(lstbox.Items[i].Value));
                    strCat.Append(lstbox.Items[i].Value);
                    strCat.Append(";");

                    QuerySearch.Append("' or ");
                }
                QuerySearch.Remove(QuerySearch.Length - 3, 2);
                QuerySearch.Append(")");
                QuerySearch.Append(" and ");
            }
            return strCat;
        }
        public StringBuilder SearchCatforCP(ListBox lstbox, StringBuilder QuerySearch, string FieldName, StringBuilder strCat)
        {
            strCat = new StringBuilder();
            if (lstbox.Items.Count != 0)
            {
                QuerySearch.Append("(");
                for (int i = 0; i < lstbox.Items.Count; i++)
                {

                    QuerySearch.Append(FieldName);
                    QuerySearch.Append(" like '");
                    QuerySearch.Append(checkquotes(lstbox.Items[i].Value));
                    strCat.Append(lstbox.Items[i].Value);
                    strCat.Append(";");

                    QuerySearch.Append("%' or ");
                }
                QuerySearch.Remove(QuerySearch.Length - 3, 2);
                QuerySearch.Append(")");
                QuerySearch.Append(" and ");
            }
            return strCat;
        }
        public StringBuilder SearchCatComma(ListBox lstbox, StringBuilder QuerySearch, string FieldName, StringBuilder strCat)
        {
            strCat = new StringBuilder();
            if (lstbox.Items.Count != 0)
            {
                QuerySearch.Append("(");
                for (int i = 0; i < lstbox.Items.Count; i++)
                {
                    QuerySearch.Append(FieldName);
                    QuerySearch.Append(" like '%");
                    QuerySearch.Append(checkquotes(lstbox.Items[i].Value));
                    strCat.Append(lstbox.Items[i].Value);
                    strCat.Append(";");

                    QuerySearch.Append("%' and ");
                }
                QuerySearch.Remove(QuerySearch.Length - 4, 3);
                QuerySearch.Append(")");
                QuerySearch.Append(" and ");
            }
            return strCat;
        }

        public StringBuilder SearchCatDate(ListBox lstbox, StringBuilder QuerySearch, string FieldName, StringBuilder strCat, CultureInfo culture)
        {
            DateValidate obj = new DateValidate();
            strCat = new StringBuilder();
            if (lstbox.Items.Count != 0)
            {
                QuerySearch.Append("(");
                for (int i = 0; i < lstbox.Items.Count; i++)
                {
                    QuerySearch.Append(FieldName);
                    QuerySearch.Append("='");
                    string strDate = obj.convertDate(lstbox.Items[i].Value, culture);
                    QuerySearch.Append(strDate);
                    strCat.Append(strDate);
                    QuerySearch.Append("' or ");
                }
                QuerySearch.Remove(QuerySearch.Length - 3, 2);
                QuerySearch.Append(")");
                QuerySearch.Append(" and ");
            }
            return strCat;
        }

        public bool ExecuteSclar(string Query)
        {
            SqlConnection ObjCon = new SqlConnection(ConfigurationManager.AppSettings["connectionstring"].ToString());
            SqlCommand cmd = new SqlCommand(Query, ObjCon);
            SqlDataReader dr;
            ObjCon.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dr.GetString(2);
                return true;
            }
            return false;
        }

        public bool ExecuteSclarPosition(string Query)
        {
            SqlConnection ObjCon = new SqlConnection(ConfigurationManager.AppSettings["connectionstring"].ToString());
            SqlCommand cmd = new SqlCommand(Query, ObjCon);
            SqlDataReader dr;
            ObjCon.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dr.GetString(0);
                return true;
            }
            return false;
        }

        public string CheckDBNULL(string strDBNULL)
        {
            if (strDBNULL != DBNull.Value.ToString())
            {
                return strDBNULL;
            }
            else
            {
                return "";
            }
        }

        public string CheckDBNULLnbsp(string strDBNULL)
        {
            if (strDBNULL != "&nbsp;")
            {
                return strDBNULL.Replace("'", "''");
            }
            else
            {
                return "";
            }
        }

        public string CheckSemiColon(string strDBNULL)
        {
            if (strDBNULL != ";")
            {
                return strDBNULL.Replace("'", "''");
            }
            else
            {
                return "";
            }
        }

        public string CheckDBNULLnbspINT(string strDBNULL)
        {
            if (strDBNULL == "&nbsp;")
            {
                return "0";
            }
            return strDBNULL;
        }

        public bool IsModificationDateValid(TextBox txtdatecreation, TextBox txtdatemodification, Label lblerrordatemodification)
        {
            string dateModification;
            string dateCreation;
            if (txtdatemodification.Text != "")
            {
                dateModification = DateTime.Parse(txtdatemodification.Text.Trim(), CultureInfo.CurrentCulture).ToShortDateString();
            }
            if (txtdatecreation.Text != "")
            {
                dateCreation = DateTime.Parse(txtdatecreation.Text.Trim(), CultureInfo.CurrentCulture).ToShortDateString();
            }
            //if (Convert.ToDateTime(dateModification) < Convert.ToDateTime(dateCreation))
            if (Convert.ToDateTime(DateTime.Parse(txtdatemodification.Text.Trim(), CultureInfo.CurrentCulture).ToShortDateString()) < Convert.ToDateTime(DateTime.Parse(txtdatecreation.Text.Trim(), CultureInfo.CurrentCulture).ToShortDateString()))
            {
                lblerrordatemodification.Text = "Date modification cannot be earlier than date creation";
                lblerrordatemodification.Visible = true;
                return false;
            }
            return true;
        }

        public string Execute_scalar(string text)
        {
            try
            {
                SqlConnection ObjCon = new SqlConnection(ConfigurationManager.AppSettings["connectionString"].ToString());
                SqlCommand ObjCmd = new SqlCommand();
                if (ObjCon.State.ToString() != "Open")
                {
                    ObjCon.Open();

                }
                ObjCmd.Connection = ObjCon;
                ObjCmd.CommandText = text;
                if (ObjCmd.ExecuteScalar() != DBNull.Value)
                    return ObjCmd.ExecuteScalar().ToString();
                else
                    return "false";

            }
            catch (Exception Ex)
            {
                string msg = Ex.Message;
                return "false";
            }
        }

        public void FillCP_DropDown(DropDownList drp)
        {
            SqlConnection ObjCon = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
            string Q = "select * from cpregion order by cpcode asc";
            ObjCon.Open();
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand(Q, ObjCon);
            dr = cmd.ExecuteReader();

            drp.Items.Add("CP");
            while (dr.Read())
            {
                drp.Items.Add(Convert.ToString(dr.GetInt32(0)));
            }
        }
        public int CheckTable(string ProcedureName, string strMonth, string strYear, SqlConnection sqlConn)
        {
            SqlCommand comm = new SqlCommand();
            comm.Connection = sqlConn;
            //comm.Transaction = sqlTrans;  
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = ProcedureName;
            objcommon.SetCommandParameters(comm, "@Month", SqlDbType.VarChar, 50, 'I', 0, 0, strMonth);
            objcommon.SetCommandParameters(comm, "@Year", SqlDbType.VarChar, 50, 'I', 0, 0, strYear);
            objcommon.SetCommandParameters(comm, "@xtype", SqlDbType.Char, 1, 'I', 0, 0, "U");
            int x = (int)comm.ExecuteScalar();
            return x;

        }
        public void CreateTable(string ProcedureName, string strMonth, string strYear, SqlConnection sqlConn)
        {
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.Connection = sqlConn;
            //sqlcomm.Transaction = sqlTrans;
            sqlcomm.CommandText = ProcedureName;
            sqlcomm.CommandType = CommandType.StoredProcedure;
            objcommon.SetCommandParameters(sqlcomm, "@Month", SqlDbType.VarChar, 50, 'I', 0, 0, strMonth);
            objcommon.SetCommandParameters(sqlcomm, "@Year", SqlDbType.VarChar, 50, 'I', 0, 0, strYear);
            sqlcomm.ExecuteNonQuery();

        }
        //public void FillTelerikCombo(string DataTextField,string DataValueField,string tableName,int pageSize, object dbCombo,string searchText)
        //{
        //    if ((DataTextField != "") &&
        //    (DataValueField != "") &&
        //    (tableName != "")) 
        //    {
        //        try
        //        {
        //            RadComboBox combo = (RadComboBox)dbCombo ;  
        //            combo.Items.Clear();
        //            string myConnection = ConfigurationManager.AppSettings["connectionString"].ToString();     
        //            SqlConnection dbCon = new SqlConnection(myConnection); //@"Data Source=TESTMACHINE\TESTMACHINEDB;Initial Catalog=ProjectAA;Persist Security Info=True;User ID=sa;Password=sa");
        //            dbCon.Open();
        //            string text = "";
        //            text = searchText; 
        //            string sql = "SELECT distinct top " + pageSize + " " + DataValueField + "," + DataTextField + " from " + tableName + " where " + DataTextField + " like '" + text + "%'" + " order by " + DataTextField; 

        //            SqlDataAdapter adapter = new SqlDataAdapter(sql, dbCon);
        //            DataTable dt = new DataTable();
        //            adapter.Fill(dt);
        //            dbCon.Close();
        //            if (dt.Rows.Count != 0)
        //            {
        //                if (DataTextField.Substring(0, 1) == "[")
        //                {
        //                    DataTextField = DataTextField.Remove(0, 1);
        //                    DataTextField = DataTextField.Remove(DataTextField.Length - 1, 1);
        //                }
        //                if (DataValueField.Substring(0, 1) == "[")
        //                {
        //                    DataValueField = DataValueField.Remove(0, 1);
        //                    DataValueField = DataValueField.Remove(DataValueField.Length - 1, 1);
        //                }
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    RadComboBoxItem item = new RadComboBoxItem(row[DataTextField].ToString(), row[DataValueField].ToString());
        //                    combo.Items.Add(item);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw(ex);
        //        }
        //    }
        //}
        //public void FillTelerikCombo(string DataTextField, string DataValueField, string tableName, string sortedBy, int pageSize, object dbCombo, string searchText)
        //{
        //    if ((DataTextField != "") &&
        //    (DataValueField != "") &&
        //    (tableName != ""))
        //    {
        //        try
        //        {
        //            RadComboBox combo = (RadComboBox)dbCombo;
        //            combo.Items.Clear();
        //            string myConnection = ConfigurationManager.AppSettings["connectionString"].ToString();
        //            SqlConnection dbCon = new SqlConnection(myConnection); //@"Data Source=TESTMACHINE\TESTMACHINEDB;Initial Catalog=ProjectAA;Persist Security Info=True;User ID=sa;Password=sa");
        //            dbCon.Open();
        //            string text = "";
        //            text = searchText;
        //            string sql = "SELECT distinct top " + pageSize + " " + DataValueField + "," + DataTextField + " as field1 from " + tableName + " where " + sortedBy + " like '" + text + "%'" + " order by field1 desc";

        //            SqlDataAdapter adapter = new SqlDataAdapter(sql, dbCon);
        //            DataTable dt = new DataTable();
        //            adapter.Fill(dt);
        //            dbCon.Close();
        //            if (dt.Rows.Count != 0)
        //            {
        //                if (DataValueField.Substring(0, 1) == "[")
        //                {
        //                    DataValueField = DataValueField.Remove(0, 1);
        //                    DataValueField = DataValueField.Remove(DataValueField.Length - 1, 1);
        //                }

        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    RadComboBoxItem item = new RadComboBoxItem(row["field1"].ToString(), row[DataValueField].ToString());

        //                    combo.Items.Add(item);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw (ex);
        //        }

        //    }

        //}
        //public void FillTelerikCombo(string Query, string sortedBy, string condition, object dbCombo, string searchText)
        //{
        //    string sql;
        //    if (Query != "") 
        //    {
        //        try
        //        {
        //            RadComboBox combo = (RadComboBox)dbCombo;
        //            DataView dv = new DataView();
        //            combo.Items.Clear();
        //            string myConnection = ConfigurationManager.AppSettings["connectionString"].ToString();
        //            SqlConnection dbCon = new SqlConnection(myConnection); //@"Data Source=TESTMACHINE\TESTMACHINEDB;Initial Catalog=ProjectAA;Persist Security Info=True;User ID=sa;Password=sa");
        //            dbCon.Open();
        //            string text = "";
        //            text = searchText;
        //            if (condition == "")
        //            {
        //                 sql = Query + " where " + sortedBy + " like '" + text + "%'" + " order by " + sortedBy;
        //            }
        //            else
        //            {
        //                 sql = Query + " where " + sortedBy + " like '" + text + "%'" + " and " + condition + " order by " + sortedBy;
        //            }

        //            SqlDataAdapter adapter = new SqlDataAdapter(sql, dbCon);
        //            DataSet ds = new DataSet();
        //            adapter.Fill(ds);
        //            dbCon.Close();
        //            if (ds.Tables[0].Rows.Count  > 0)
        //            {
        //                dv = ds.Tables[0].DefaultView;
        //            }
        //            for (int i = 0; i <=dv.Table.Rows.Count - 1; i++)
        //            {
        //                RadComboBoxItem item = new RadComboBoxItem(dv.Table.Rows[i][dv.Table.Columns[1].ColumnName].ToString(), dv.Table.Rows[i][dv.Table.Columns[0].ColumnName].ToString());
        //                combo.Items.Add(item);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw (ex);
        //        }

        //    }
        //}
        //public void FillTelerikCombo(string DataTextField,string tableName, int pageSize, object dbCombo, string searchText)
        //{
        //    if ((DataTextField != "") &&  (tableName != ""))
        //    {
        //        try
        //        {
        //            RadComboBox combo = (RadComboBox)dbCombo;
        //            combo.Items.Clear();
        //            string myConnection = ConfigurationManager.AppSettings["connectionString"].ToString();
        //            SqlConnection dbCon = new SqlConnection(myConnection); //@"Data Source=TESTMACHINE\TESTMACHINEDB;Initial Catalog=ProjectAA;Persist Security Info=True;User ID=sa;Password=sa");
        //            dbCon.Open();
        //            string text = "";
        //            text = searchText;
        //            string sql = "SELECT distinct top " + pageSize + " " + DataTextField + " from " + tableName + " where " + DataTextField + " like '" + text + "%'" + " order by " + DataTextField;

        //            SqlDataAdapter adapter = new SqlDataAdapter(sql, dbCon);
        //            DataTable dt = new DataTable();
        //            adapter.Fill(dt);
        //            dbCon.Close();
        //            if (dt.Rows.Count != 0)
        //            {
        //                if (DataTextField.Substring(0, 1) == "[")
        //                {
        //                    DataTextField = DataTextField.Remove(0, 1);
        //                    DataTextField = DataTextField.Remove(DataTextField.Length - 1, 1);
        //                }
        //                //if (DataValueField.Substring(0, 1) == "[")
        //                //{
        //                //    DataValueField = DataValueField.Remove(0, 1);
        //                //    DataValueField = DataValueField.Remove(DataValueField.Length - 1, 1);
        //                //}

        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    RadComboBoxItem item = new RadComboBoxItem(row[DataTextField].ToString());

        //                    combo.Items.Add(item);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw (ex);
        //        }

        //    }
        //}

        //public void FillTelerikCombo(string Query, object dbCombo)
        //{
        //    string sql;
        //    if (Query != "")
        //    {
        //        try
        //        {
        //            RadComboBox combo = (RadComboBox)dbCombo;
        //            DataView dv = new DataView();
        //            combo.Items.Clear();
        //            string myConnection = ConfigurationManager.AppSettings["connectionString"].ToString();
        //            SqlConnection dbCon = new SqlConnection(myConnection); //@"Data Source=TESTMACHINE\TESTMACHINEDB;Initial Catalog=ProjectAA;Persist Security Info=True;User ID=sa;Password=sa");
        //            dbCon.Open();

        //            sql = Query;

        //            SqlDataAdapter adapter = new SqlDataAdapter(sql, dbCon);
        //            DataSet ds = new DataSet();
        //            adapter.Fill(ds);
        //            dbCon.Close();
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                dv = ds.Tables[0].DefaultView;
        //            } 
        //            for (int i = 0; i < dv.Table.Rows.Count; i++)
        //            {

        //                if (combo.Items.Count == 0)
        //                {
        //                    RadComboBoxItem item0 = new RadComboBoxItem(dv.Table.Rows[i][dv.Table.Columns[0].ColumnName].ToString(), dv.Table.Rows[i][dv.Table.Columns[0].ColumnName].ToString());                        
        //                    //item0.Text = "--Select--";
        //                    //item0.Value = "--Select--";
        //                    combo.Items.Add(item0);
        //                }

        //                RadComboBoxItem item = new RadComboBoxItem(dv.Table.Rows[i][dv.Table.Columns[0].ColumnName].ToString(), dv.Table.Rows[i][dv.Table.Columns[0].ColumnName].ToString());                        
        //                item.Text = dv.Table.Rows[i][dv.Table.Columns[1].ColumnName].ToString();
        //                item.Value = dv.Table.Rows[i][dv.Table.Columns[0].ColumnName].ToString();
        //                combo.Items.Add(item);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw (ex);
        //        }

        //    }
        //}

        public void SetCommandParameters(SqlCommand sqlcomm, string p, SqlDbType sqlDbType, int p_4, char p_5)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
