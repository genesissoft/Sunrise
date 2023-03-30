using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Data.OleDb;
using ClosedXML.Excel;
using System.Windows.Forms;

public partial class Forms_DPReconciliation : System.Web.UI.Page
{
    SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
    SqlCommand sqlComm = new SqlCommand();
    Common objComm = new Common();
    clsCommonFuns objCommon = new clsCommonFuns();
    DataTable dt;
    string strMsg = "";
    SqlDataAdapter da;
    string strError = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        dt = new DataTable();
        try
        {
            if (string.IsNullOrEmpty(Session["UserName"] as string))
            {
                Response.Redirect("Login.aspx", false);
            }

            //HtmlMeta xuac = new HtmlMeta();
            //xuac.HttpEquiv = "X-UA-Compatible";
            //xuac.Content = "IE=edge";
            //Header.Controls.AddAt(0, xuac);

            if (!Page.IsPostBack)
            {
                txtDated.Text = DateTime.Now.ToString("dd/MM/yyyy");
                FillCombo();
               ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "test", "ReportType();", true);
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "test", "ReportType();", true);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void FillCombo()
    {
        DataSet dsDP = new DataSet();
        DataSet dsSGL = new DataSet();
        List<SqlParameter> lstParamDP = new List<SqlParameter>();
        List<SqlParameter> lstParamSGL = new List<SqlParameter>();


        try
        {
            lstParamDP.Add(new SqlParameter("@CompId", objComm.Val(Session["CompId"].ToString()) > 0 ? Session["CompId"].ToString() : null));
            lstParamSGL.Add(new SqlParameter("@CompId", objComm.Val(Session["CompId"].ToString()) > 0 ? Session["CompId"].ToString() : null));
            dsDP = objComm.FillDetails(lstParamDP, "ID_Fill_DmatClientIdCombo");
            dsSGL = objComm.FillDetails(lstParamSGL, "ID_Fill_SGLIdCombo");
            if (dsDP != null && dsDP.Tables.Count > 0)
            {
                cboDPId.DataSource = dsDP.Tables[0];
                cboDPId.DataValueField = "Id";
                cboDPId.DataTextField = "Name";
                cboDPId.DataBind();
            }
            if (dsSGL != null && dsSGL.Tables.Count > 0)
            {
                cboSGLId.DataSource = dsSGL.Tables[0];
                cboSGLId.DataValueField = "Id";
                cboSGLId.DataTextField = "Name";
                cboSGLId.DataBind();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (fUpload.HasFile)
            {
                string FileName = Path.GetFileName(fUpload.PostedFile.FileName);
                string Extension = Path.GetExtension(fUpload.PostedFile.FileName);
                string FilePath = Path.Combine(Server.MapPath("~/Temp/"), FileName);

                fUpload.SaveAs(FilePath);
                Import_To_Grid(FilePath, Extension);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "alert('" + ex.Message + "');", true);
        }
    }

    private void Import_To_Grid(string FilePath, string Extension)
    {
        string strFilePath = "";
        string strUserId = "";
        string strIssueId = "";
        string strType = "";
        Int32 intTotalRecords = 200;
        OleDbConnection cnnExcel = new OleDbConnection();
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataSet dsData = new DataSet();

        DataSet ds = new DataSet();
        DataTable dtExcelSchema = new DataTable();

        DataTable dtData = new DataTable();
        DataTable dtMapFields = new DataTable();
        List<SqlParameter> lstParam = new List<SqlParameter>();

        DataTable dtSharing = new DataTable();
        strMsg = "";
        strMsg = strError;
        string cnnString;
        string conStr = "";
        switch (Extension)
        {
            case ".xlsx":
                {
                    cnnString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ToString();
                    break;
                }
            case ".xls":
                {
                    cnnString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ToString();
                    break;
                }
            default:
                {
                    cnnString = "";
                    break;
                }
        }

        cnnString = String.Format(cnnString, FilePath, 0);
        cnnExcel = new OleDbConnection(cnnString);
        cmdExcel.Connection = cnnExcel;
        cnnExcel.Open();
        dtExcelSchema = cnnExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        cmdExcel.CommandType = CommandType.Text;
        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        cnnExcel.Close();

        //Read Data from First Sheet
        cnnExcel.Open();
        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
        oda.SelectCommand = cmdExcel;
        oda.Fill(dt);
        cnnExcel.Close();

        strType = "3";
        lstParam.Clear();
        ds = FillMapFields();
        dtMapFields = ds.Tables[0];
        DataRow[] rowsMapFields = dtMapFields.Select("FileTypeId>0");
        if (dt.Rows.Count > 0 && rowsMapFields.Length > 0)
        {
            CreateBlankTable(ref dtData, rowsMapFields);
            dt = RemoveEmptyRowsFromDataTable(dt);
            if (ValidateExcelSheet(dt, rowsMapFields) && dtData.Columns.Count > 0 && strMsg == " ")
            {
                if (dt.Rows.Count > 0)
                {
                    sqlComm = new SqlCommand("ID_DPStock_Match", sqlConn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.Clear();
                    sqlComm.Parameters.Add("@InsertDPStock", SqlDbType.Structured).Value = dt;
                    sqlComm.Parameters.AddWithValue("@Asondate", txtDated.Text);
                    sqlComm.Parameters.AddWithValue("@CompId", Val(Session["CompId"].ToString()));
                    sqlComm.Parameters.AddWithValue("@DMatId", Val(cboDPId.SelectedValue));
                    sqlComm.Parameters.AddWithValue("@SGLId", Val(cboSGLId.SelectedValue));
                    sqlComm.Parameters.AddWithValue("@UserId", Val(Session["UserId"].ToString()));
                    sqlComm.Parameters.AddWithValue("@DeliveryMode", objComm.Trim(rbl_TypeOFTransfer.SelectedValue));
                   
                    SqlDataAdapter sqlsda = new SqlDataAdapter(sqlComm);
                    ds = new DataSet();
                    sqlsda.Fill(ds);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ExportUnmatchedStock(ds.Tables[0]);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "alert('DP Stock Match Successfully.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "alert('No Data to upload.');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "alert('" + strMsg + "');", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "alert('No Data to upload.');", true);
            return;
        }
    }

    public void ExportUnmatchedStock(DataTable dt)
    {
        try
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add(dt, "DP Unmatched Stock");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=DPUnmatchedStock.xlsx");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public DataSet FillMapFields()
    {
        DataSet ds = new DataSet();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        try
        {
            lstParam.Add(new SqlParameter("@FileTypeId", 3));
            ds = objComm.FillDetails(lstParam, "ID_Fill_MapFields");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ds;
    }

    public void CreateBlankTable(ref DataTable dtData, DataRow[] rowsMapFields)
    {
        int i = 0;
        string strDataType = "";
        string strDataBaseFieldName = "";
        try
        {
            dtData.Columns.Add("RowNo", typeof(Int32));
            for (i = 0; i < rowsMapFields.Length; i++)
            {
                strDataType = Trim(rowsMapFields[i]["DataType"].ToString());
                strDataBaseFieldName = rowsMapFields[i]["DataBaseFieldName"].ToString();
                if (strDataType == "NUMBER")
                {
                    dtData.Columns.Add(strDataBaseFieldName, typeof(Int64));
                }
                else if (strDataType == "DECIMAL")
                {
                    dtData.Columns.Add(strDataBaseFieldName, typeof(Decimal));
                }
                else if (strDataType == "DATE")
                {
                    dtData.Columns.Add(strDataBaseFieldName, typeof(DateTime));
                }
                else if (strDataType == "VARCHAR")
                {
                    dtData.Columns.Add(strDataBaseFieldName, typeof(String));
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public Decimal Val(string strValues)
    {
        Decimal result;
        if (Decimal.TryParse(strValues, out result))
        {
            return result;
        }
        return 0;
    }

    public string Trim(string strValues)
    {
        if (!string.IsNullOrEmpty(strValues))
        {
            return strValues;
        }
        return "";
    }

    public bool ValidateExcelSheet(DataTable dtData, DataRow[] rowsMapFields)
    {
        decimal num = 0;
        int numeric = 0;
        DateTime date;
        string strDataType, strExcelColumnName;
        Int16 intExcelPos;
        
        int rowno = 2;
        int i = 0;
        bool blnRequired;
        DataTable dtDealNo = new DataTable();
        dtDealNo.Columns.Add("DealSlipId", typeof(int));
        try
        {

            string strColumns = Trim(rowsMapFields[0]["ColumnName"].ToString());
            if (dtData.Columns.Count < Val(rowsMapFields[0]["ColumnLength"].ToString()))
            {
                strMsg = "Prescribed columns have been altered in the excel file. Kindly reupload the file in the prescribed format only.";
                return false;
            }

            for (i = 0; i < rowsMapFields.Length; i++)
            {
                intExcelPos = Convert.ToInt16(rowsMapFields[i]["ExcelPosition"]);
                strExcelColumnName = rowsMapFields[i]["ExcelHeaderName"].ToString();
                if (strExcelColumnName.ToUpper().Trim().Replace(" ", "") != dtData.Columns[intExcelPos].ToString().ToUpper().Trim().Replace(" ", ""))
                //if (strExcelColumnName.ToUpper() != dtData.Columns[intExcelPos].ToString().ToUpper().Trim().Replace(" ", "").Replace("#", "."))
                {
                    strError = strError + "Column name must be in the folllowing order only.";
                    strError = strError + strColumns;
                    strMsg = strError;
                    return false;
                }
            }

            if (strError == "")
            {
                foreach (DataRow row in dtData.Rows)
                {
                    for (i = 0; i < rowsMapFields.Length; i++)
                    {
                        strDataType = Convert.ToString(rowsMapFields[i]["DataType"]);
                        intExcelPos = Convert.ToInt16(rowsMapFields[i]["ExcelPosition"]);
                        strExcelColumnName = rowsMapFields[i]["ExcelHeaderName"].ToString();
                        blnRequired = Convert.ToBoolean(rowsMapFields[i]["IsRequired"]);

                        if (strDataType == "NUMBER")
                        {
                            if (string.IsNullOrEmpty(row[intExcelPos].ToString().Trim()) == true)
                            {
                                strError = strError + " " + strExcelColumnName + " cannot be blank. Error Details , Row No: " + rowno + " , Column No: " + (intExcelPos + 1).ToString() + " (require only numeric values)";
                                return false;
                            }
                            else if (string.IsNullOrEmpty(row[intExcelPos].ToString().Trim()) != true && !(Int32.TryParse(row[intExcelPos].ToString().Trim(), out numeric)))
                            {
                                strError = strError + "Invalid value for " + strExcelColumnName + ".Error Details , Row No: " + rowno + " , Column No: " + (intExcelPos + 1).ToString() + " (require only numeric values)";
                                return false;
                            }
                        }
                        else if (strDataType == "DATE")
                        {
                            if (string.IsNullOrEmpty(row[intExcelPos].ToString().Trim()) == true)
                            {
                                strError = strError + " " + strExcelColumnName + " cannot be blank. Error Details , Row No: " + rowno + " , Column No: " + (intExcelPos + 1).ToString() + " (require only date values i-e dd/mmm/yyyy)";
                                return false;
                            }
                            else if (string.IsNullOrEmpty(row[intExcelPos].ToString().Trim()) != true && !(DateTime.TryParse(row[intExcelPos].ToString().Trim(), out date)))
                            {
                                strError = strError + "Invalid value for " + strExcelColumnName + ". Error Details , Row No: " + rowno + " , Column No: " + (intExcelPos + 1).ToString() + " (require only date values i-e dd/mmm/yyyy)";
                                return false;
                            }
                        }
                        else if (strDataType == "DECIMAL(18,6)")
                        {
                            if (string.IsNullOrEmpty(row[intExcelPos].ToString().Trim()) != true && !(decimal.TryParse(row[intExcelPos].ToString().Trim(), out num)))
                            {
                                strError = strError + "Invalid value for " + strExcelColumnName + ".Error Details , Row No: " + rowno + " , Column No: " + (intExcelPos + 1).ToString() + " (require only numeric values)";
                                return false;
                            }
                        }
                        else if (strDataType == "VARCHAR" && strExcelColumnName != "Particulars")
                        {
                            if (string.IsNullOrEmpty(row[intExcelPos].ToString().Trim()) == true && blnRequired)
                            {
                                strError = strError + " " + strExcelColumnName + " cannot be blank. Error Details  , Row No: " + rowno + " , Column No: " + (intExcelPos + 1).ToString();
                                return false;
                            }
                        }
                        else if (strDataType == "VARCHAR")
                        {
                        }
                    }
                    rowno = rowno + 1;
                }
            }

            if (strError == "")
            {
                return true;
            }
            else if (strError.Contains("Invalid column name"))
            {
                strMsg = strMsg + " Invalid columns, Please upload correct file.";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "alert('" + strMsg + "' Invalid columns, Please upload correct file.');", true);

                return false;
            }
            else
            {
                strMsg = strMsg + " " + strError;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "alert('" + strMsg + " " + strError + "');", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            strMsg = strMsg + " " + strError;
        }
    }

    DataTable RemoveEmptyRowsFromDataTable(DataTable dt)
    {
        for (int i = dt.Rows.Count - 1; i >= 0; i--)
        {
            if (dt.Rows[i][1] == DBNull.Value)
            {
                dt.Rows[i].Delete();
            }
        }
        dt.AcceptChanges();
        return dt;
    }

    DataTable RemoveEmptyDealNoFromDataTable(DataTable dt)
    {
        for (int i = dt.Rows.Count - 1; i >= 0; i--)
        {
            if (dt.Rows[i][0] == DBNull.Value)
            {
                dt.Rows[i].Delete();
            }
        }
        dt.AcceptChanges();
        return dt;
    }
}
