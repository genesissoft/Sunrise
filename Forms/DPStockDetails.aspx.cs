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

public partial class Forms_DPStockDetails : System.Web.UI.Page
{
    SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
    SqlCommand sqlComm = new SqlCommand();
    Common objComm = new Common();
    clsCommonFuns objCommon = new clsCommonFuns();
    DataTable dt;
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
                ScriptManager.RegisterStartupScript(this.Page,this.GetType(), "test", "ReportType();", true);
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

    protected void btn_Export_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> lstParam = new List<SqlParameter>();

        try
        {
            lstParam.Add(new SqlParameter("@CompId", objComm.Val(Session["CompId"].ToString()) > 0 ? Session["CompId"].ToString() : null));
            lstParam.Add(new SqlParameter("@UserId", objComm.Val(Session["UserId"].ToString()) > 0 ? Session["UserId"].ToString() : null));
            lstParam.Add(new SqlParameter("@DMatId", objComm.Val(cboDPId.SelectedValue) > 0 ? cboDPId.SelectedValue : null));
            lstParam.Add(new SqlParameter("@SGLId", objComm.Val(cboSGLId.SelectedValue) > 0 ? cboSGLId.SelectedValue : null));
            lstParam.Add(new SqlParameter("@Asondate", objComm.Trim(txtDated.Text) != "" ? txtDated.Text.Trim() : null));
            lstParam.Add(new SqlParameter("@DeliveryMode", objComm.Trim(rbl_TypeOFTransfer.SelectedValue)));
            ds = objComm.FillDetails(lstParam, "ID_DPStock_Export");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ExportToExcel(ds.Tables[0]);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "alert('No stock found.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "alert('" + ex.Message + "');", true);
        }
    }

    public void ExportToExcel(DataTable dt)
    {
        int TotalNoOfBond = 0;
        string SheetName = "";
        try
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if ((rbl_TypeOFTransfer.SelectedValue == "D"))
                {
                    SheetName = "Available DP  Stock Details";
                }
                else
                {
                    SheetName = "Available SGL  Stock Details";

                }
                IXLWorksheet ws = wb.Worksheets.Add(dt, SheetName);
                ws.Table(0).ShowAutoFilter = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TotalNoOfBond = TotalNoOfBond + Convert.ToInt32(dt.Rows[i]["Available  Stock"].ToString());
                }
                int count = dt.Rows.Count + 2;
                int NoofBond_Index = Convert.ToInt32(dt.Columns.IndexOf("Available  Stock").ToString());
                NoofBond_Index = NoofBond_Index + 1;
                string NoofBondLetter = Number2String(NoofBond_Index, true);
                ws.Cell(NoofBondLetter + count.ToString()).SetValue(TotalNoOfBond);
                ws.Cell(NoofBondLetter + count.ToString()).Style.Font.Bold = true;
                int dcCount = dt.Columns.Count;
                string CC = Number2String(dcCount, true);

                ws.Range("A1:" + CC + count.ToString()).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range("A1:" + CC + count.ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                ws.Range("A1:" + CC + count.ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("A1:" + CC + count.ToString()).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                ws.Columns().AdjustToContents();
                string strdtTime = DateTime.Now.ToString("h:mm:ss tt");
                strdtTime = strdtTime.Replace("PM", "");
                strdtTime = strdtTime.Replace("AM", "");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=DPStockDetails" + strdtTime + ".xlsx");

                ws.Cell("A" + count.ToString()).SetValue("Total");
                ws.Cell("A" + count.ToString()).Style.Font.Bold = true;

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.SuppressContent = true;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "alert('" + ex.Message + "');", true);
        }
    }

    private String Number2String(int number, bool isCaps)
    {
        int number1 = number / 27;
        int number2 = number - (number1 * 26);
        if (number2 > 26)
        {
            number1 = number1 + 1;
            number2 = number - (number1 * 26);
        }
        Char a = (Char)((isCaps ? 65 : 97) + (number1 - 1));
        Char b = (Char)((isCaps ? 65 : 97) + (number2 - 1));
        Char c = (Char)((isCaps ? 65 : 97) + (number - 1));
        string d = String.Concat(a, b);
        if (number <= 26)
            return c.ToString();
        else
            return d;
    }
}
