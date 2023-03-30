using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

public partial class Forms_UploadSettDates : System.Web.UI.Page
{
    OleDbConnection Econ;
    SqlConnection con;

    string constr, Query, sqlconn;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { }
    }

    private void connection()
    {
        sqlconn = ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ConnectionString;
        con = new SqlConnection(sqlconn);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (rbtType.SelectedItem.Text == "ICCL")
        {
            ExcelConnICCL(0);
        }
        else if (rbtType.SelectedItem.Text == "NSCCL")
        {
            ExcelConnNSCCL(0);
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('ICCL File Not Uploaded...');", true);
        }
    }

    public void ExcelConnICCL(int Update)
    {
        clsCommonFuns objcls = new clsCommonFuns();
        DataTable tblcsv = new DataTable();
        string CSVFilePath = string.Empty;

        //creating columns  
        tblcsv.Columns.Add("Sett No");
        tblcsv.Columns.Add("Sett Type");
        tblcsv.Columns.Add("Trans Date");
        tblcsv.Columns.Add("Sec/Funds Pay-in & Pay-out date");
        //getting full file path of Uploaded file  
        if (Update == 0)
        {
            string FilePath = string.Empty;
            string FileName = Path.GetFileName(fuDetails.PostedFile.FileName);
            string Extension = Path.GetExtension(fuDetails.PostedFile.FileName);
            FilePath = Path.Combine(Server.MapPath("~/Temp/"), FileName);
            fuDetails.SaveAs(FilePath);

            CSVFilePath = FilePath; //Path.GetFullPath(fuDetails.PostedFile.FileName);
            ViewState["CSVFilePath"] = FilePath;
        }
        else
        {
            CSVFilePath = ViewState["CSVFilePath"].ToString();
        }
        //Reading All text  
        string ReadCSV = File.ReadAllText(CSVFilePath);
        //spliting row after new line  
        foreach (string csvRow in ReadCSV.Split('\n'))
        {
            if (!string.IsNullOrEmpty(csvRow) && !csvRow.Contains("Sett No"))
            {
                //Adding each row into datatable  
                tblcsv.Rows.Add();
                int count = 0;
                string data = string.Empty;
                foreach (string FileRec in csvRow.Split(','))
                {
                    if (FileRec.Contains("-"))
                    {
                        //FileRec.ToString() = "10-Feb-2020";
                        //data = objcls.DateFormat(FileRec).ToString();
                        data = (FileRec);
                    }
                    else
                    {
                        data = FileRec;
                    }
                    tblcsv.Rows[tblcsv.Rows.Count - 1][count] = data;
                    count++;
                }
            }
        }
        //Calling insert Functions  
        InsertCSVRecords(tblcsv, Update);
    }

    private int InsertCSVRecords(DataTable csvdt, int Update)
    {
        connection();
        //creating object of SqlBulkCopy    
        SqlBulkCopy objbulk = new SqlBulkCopy(con);
        //assigning Destination table name    
        objbulk.DestinationTableName = "SettDatesTempTable";
        //Mapping Table column    
        objbulk.ColumnMappings.Add("[Sett No]", "SettNo");
        objbulk.ColumnMappings.Add("[Sett Type]", "SettType");
        objbulk.ColumnMappings.Add("[Trans Date]", "TransDate");
        objbulk.ColumnMappings.Add("[Sec/Funds Pay-in & Pay-out date]", "PayDate");
        //inserting Datatable Records to DataBase  

        con.Open();
        SqlCommand truncate = new SqlCommand("TRUNCATE TABLE SettDatesTempTable", con);
        truncate.ExecuteNonQuery();

        objbulk.WriteToServer(csvdt);
        con.Close();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        SqlCommand sqlComm = new SqlCommand("dbo.ID_BulkUploadSettDates", con);
        sqlComm.CommandType = CommandType.StoredProcedure;
        sqlComm.Parameters.Add("@Update", SqlDbType.Int).Value = Update;
        sqlDa = new SqlDataAdapter(sqlComm);
        DataTable dt1 = new DataTable();
        sqlDa.Fill(dt1);
        sqlComm.Parameters.Clear();
        con.Close();
        if (dt1.Rows.Count > 0)
        {
            if (Convert.ToInt32(dt1.Rows[0]["ErrorCode"]) == 1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup();", true);
                //  ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Settlement Dates Are Already Uploaded Do you want to Continue?');", true);
            }
            else
            {
                if (Update == 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Settlement Dates Uploaded Successfully.');", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Settlement Dates Upated Successfully.');", true);
                }
            }
        }
        return Convert.ToInt32(dt1.Rows[0]["ErrorCode"]);
    }


    public void ExcelConnNSCCL(int Update)
    {
        clsCommonFuns objcls = new clsCommonFuns();
        DataTable tblcsv = new DataTable();
        string CSVFilePath = string.Empty;

        //creating columns  
        tblcsv.Columns.Add("SettlementDate");
        //tblcsv.Columns.Add("TransactionDate");
        tblcsv.Columns.Add("SettlementNo");
        tblcsv.Columns.Add("UserId");
        tblcsv.Columns["UserId"].DefaultValue = Convert.ToInt16(Session["UserId"]);
        tblcsv.AcceptChanges();


        //getting full file path of Uploaded file  
        if (Update == 0)
        {
            string FilePath = string.Empty;
            string FileName = Path.GetFileName(fuDetails.PostedFile.FileName);
            string Extension = Path.GetExtension(fuDetails.PostedFile.FileName);
            FilePath = Path.Combine(Server.MapPath("~/Temp/"), FileName);
            fuDetails.SaveAs(FilePath);

            CSVFilePath = FilePath; //Path.GetFullPath(fuDetails.PostedFile.FileName);
            ViewState["CSVFilePath"] = FilePath;
        }
        else
        {
            CSVFilePath = ViewState["CSVFilePath"].ToString();
        }
        //Reading All text  
        string ReadCSV = File.ReadAllText(CSVFilePath);
        //spliting row after new line  
        foreach (string csvRow in ReadCSV.Split('\n'))
        {
            if (csvRow.Trim().Length > 0)
            {
                if (!string.IsNullOrEmpty(csvRow) && !csvRow.Contains("SettlementNo"))
                {
                    //Adding each row into datatable  
                    tblcsv.Rows.Add();
                    int count = 0;
                    string data = string.Empty;
                    foreach (string FileRec in csvRow.Split(','))
                    {
                        if (FileRec.Contains("-"))
                        {
                            //data = objcls.DateFormat(FileRec).ToString();
                            data = (FileRec);
                        }
                        else
                        {
                            data = FileRec;
                        }
                        tblcsv.Rows[tblcsv.Rows.Count - 1][count] = data;
                        count++;
                    }
                }
            }
        }

        //****17/Oct/2019*****Mehul*****************
        for (int i = tblcsv.Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = tblcsv.Rows[i];
            if (dr["SettlementDate"] == "")
                dr.Delete();
        }
        tblcsv.AcceptChanges();
        //**********************************************

        //Calling insert Functions  
        InsertCSVRecordsNSCCL(tblcsv, Update);
    }

    private int InsertCSVRecordsNSCCL(DataTable csvdt, int Update)
    {
        connection();
        //creating object of SqlBulkCopy    
        SqlBulkCopy objbulk = new SqlBulkCopy(con);
        //assigning Destination table name    
        //objbulk.DestinationTableName = "SettlementCalendar";
        objbulk.DestinationTableName = "SettlementCalendarNSCCL";
        //Mapping Table column   
        //objbulk.ColumnMappings.Add("[Dated]", System.DateTime.Now.Date );
        objbulk.ColumnMappings.Add("[SettlementNo]", "SettlementNo");
        //objbulk.ColumnMappings.Add("[TransactionDate]", "TransactionDate");
        objbulk.ColumnMappings.Add("[SettlementDate]", "SettlementDate");
        objbulk.ColumnMappings.Add("[UserId]", "UserId");

        //inserting Datatable Records to DataBase  

        con.Open();
        //SqlCommand truncate = new SqlCommand("TRUNCATE TABLE SettlementCalendar", con);
        SqlCommand truncate = new SqlCommand("TRUNCATE TABLE SettlementCalendarNSCCL", con);
        truncate.ExecuteNonQuery();

        objbulk.WriteToServer(csvdt);
        con.Close();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        //SqlCommand sqlComm = new SqlCommand("dbo.ID_BulkUploadSettDates", con);
        SqlCommand sqlComm = new SqlCommand("dbo.ID_BulkUploadSettDatesNSCCL", con);
        sqlComm.CommandType = CommandType.StoredProcedure;
        sqlComm.Parameters.Add("@Update", SqlDbType.Int).Value = Update;
        sqlDa = new SqlDataAdapter(sqlComm);
        DataTable dt1 = new DataTable();
        sqlDa.Fill(dt1);
        sqlComm.Parameters.Clear();
        con.Close();
        if (dt1.Rows.Count > 0)
        {
            if (Convert.ToInt32(dt1.Rows[0]["ErrorCode"]) == 1)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup();", true);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Settlement Dates Are Already Uploaded Do you want to Continue?');", true);
            }
            else
            {
                if (Update == 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Settlement Dates Uploaded Successfully.');", true);
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Settlement Dates Upated Successfully.');", true);
                }
            }
        }
        return Convert.ToInt32(dt1.Rows[0]["ErrorCode"]);
    }



    [System.Web.Services.WebMethod]
    public static string UploadSettDates(string categoryid)
    {
        Forms_UploadSettDates obj = new Forms_UploadSettDates();
        obj.ExcelConnICCL(Convert.ToInt32(categoryid));
        return "0";
    }

    protected void UpdateRecord(object sender, EventArgs e)
    {
        ExcelConnICCL(Convert.ToInt32(1));
    }

}
