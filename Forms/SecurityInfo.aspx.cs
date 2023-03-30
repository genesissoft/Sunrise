using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

public partial class Forms_SecurityInfo : System.Web.UI.Page
{
    int intSelIndex = -1;
    // Dim objCommon As New clsCommonFuns
    clsCommonFuns objCommon = new clsCommonFuns();
    // Dim sqlConn As SqlConnection
    SqlConnection sqlConn = new SqlConnection();
    string s = "";
    string securityId = "";

    //public Forms_SecurityInfo()
    //{
    //   this.Load += new System.EventHandler(this.Page_Load);
    //}
    public class SecIds
    {
        public decimal Id;
        public SecIds() { }
        public SecIds(decimal Id)
        {
            this.Id = Id;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D);
            Response.Expires = -1500;

            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Cache-Control", "no-cache");
            Response.AddHeader("Cache-Control", "no-store");

            //Hid_ForDate.Value = Request.QueryString["fordate"].ToString();
            //System.Web.UI.WebControls.HiddenField uc1 =this.Parent.FindControl("Hid_SecIds");
            //  System.Web.UI.WebControls.HiddenField=  Parent.FindControl("Hid_SecIds");

            Hid_SecIds.Value = Request["secids"];
            Hid_ForDate.Value = Request["fordate"];
            Hid_IsGsec.Value = Request["Gsec"];


            Session["fordate"] = Convert.ToString(Request["fordate"]);
            string IsStockUpdate = Request["StockUpdate"];

            Session["StockUpdated"] = Request["StockUpdate"];
            if (Convert.ToString(Session["StockUpdated"]) == "Y")
            {
                FillSecurityGrid_StockUpdateMaster();
            }
            else
            {
                FillSecurityGrid();
            }
            //btn_Add.Attributes.Add("onclick", "return RateValidation();");
            //Parent.
        }
        else
        {
            //FillGridWithCondition();
        }
    }

    private void FillGrid(string strSort, Int16 intPageIndex)
    {
        try
        {

            clsSearch objSearch = new clsSearch();
            string strCondFld = "fordate";

            Int16 intCondVal = 0;
            DataTable dtGrid = new DataTable();

            //dtGrid = objSearch.FillGrid(dg_Selection, null, null, Hid_ColList.Value, 2, strCondFld, Hid_ForDate.Value, null, "ID_SEARCH_SecurityFieldsNew", strSort, intPageIndex, null, 0, false, false, 0, 0, false, false, null, null, null, null, null, null);
            FillSecurityGrid();
            //Session["SecurityTable"] = dtGrid;
        }
        catch (Exception ex)
        {
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('" + ex.Message.Replace("'", " ").Replace(Strings.Chr(13), " ").Replace(Strings.Chr(10), " ") + "');", true);
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

    private void FillSecurityGrid_StockUpdateMaster()
    {
        try
        {
            DataTable dt = (DataTable)Session["SecurityMaster"];
            dt = RemoveEmptyRowsFromDataTable(dt);

            Hid_AddedSec.Value = "";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Hid_AddedSec.Value += Convert.ToString(dt.Rows[i]["SecurityId"]) + "|";
                    }
                }
            }

            List<SecIds> list = new List<SecIds>();
            if (dt != null)
            {
                if (dt.Rows.Count > 1)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        decimal secid = Convert.ToDecimal(row["SecurityId"]);
                        SecIds secid1 = new SecIds(secid);
                        list.Add(secid1);
                        //list11.Add(Convert.ToDecimal(row["SecurityId"]));
                    }

                    XmlDocument xmldoc = new XmlDocument();
                    XmlSerializer xs = new XmlSerializer(typeof(List<SecIds>));

                    using (MemoryStream stream = new MemoryStream())
                    {
                        xs.Serialize(stream, list);
                        stream.Flush(); stream.Seek(0, SeekOrigin.Begin);
                        xmldoc.Load(stream);
                    }
                }
            }


            SqlDataAdapter sqlda = new SqlDataAdapter();
            SqlCommand sqlComm = new SqlCommand();
            DataTable sqldt = new DataTable();
            DataView sqldv = new DataView();

            string strCond = Hid_Cond.Value;
            if (strCond.Contains("ISINNumber"))
            {
                strCond = strCond.Replace("ISINNumber", "NSDLAcNumber");
            }
            OpenConn();

            sqlComm.Connection = sqlConn;
            sqlComm.CommandTimeout = 100;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = "ID_SEARCH_StockUpdateMaster_UpdatedStock";
            sqlComm.Parameters.Clear();
            objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, 'I', 0, 0, Convert.ToInt32(Session["UserTypeId"]));
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, 'I', 0, 0, Convert.ToInt32(Session["UserId"]));
            objCommon.SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 1000, 'I', 0, 0, strCond);
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, 'O', 0, 0, null);

            sqlComm.ExecuteNonQuery();
            sqlda.SelectCommand = sqlComm;
            sqlda.Fill(sqldt);
            //sqldt = RemoveDuplicates(sqldt);

            sqldv = sqldt.DefaultView;
            DateTime today = DateTime.Today; // As DateTime
            string s_today = today.ToString("dd/MM/yyyy"); // As String
            sqldv.RowFilter = "StockDate='" + s_today + "'";
            // dg_Selection.PageIndex = intPageIndex;
            //sqldt = sqldv.ToTable;
            dg_Selection.DataSource = sqldv;
            dg_Selection.DataBind();

            Session["SecurityTable"] = sqldt;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('" + ex.Message.Replace("'", " ").Replace(Strings.Chr(13), " ").Replace(Strings.Chr(10), " ") + "');", true);
        }
        finally
        {
            CloseConn();
        }
    }

    protected DataTable RemoveDuplicates(DataTable dt)
    {
        dt = dt.DefaultView.ToTable(true, "NatureOFInstrument", "Semi_Ann_Flag", "CombineIPMat", "Rate_Actual_Flag", "Equal_Actual_Flag", "IntDays", "FirstYrAllYr", "StockUpdtId", "SecurityId", "TypeFlag", "SecurityTypeId", "SecurityIssuer", "SecurityName", "SecurityTypeName", "ISIN", "MaturityDate", "CallDate", "FaceValue", "BookStock", "Rating", "SellingRate", "StockDate", "ShowNumber", "CouponRate", "NameOfUser", "YTMAnn", "YTMSemi", "YTCSemi", "YTPAnn", "YTPSemi", "Yield", "YTCAnn", "Margin", "MasterSecurity", "dealer_mailflag", "LotFlag", "StockDate1");
        return dt;
    }

    private void FillSecurityGrid()
    {
        lbl_Message.Text = "";
        try
        {
            DataTable dt = (DataTable)Session["SecurityMaster"];
            dt = RemoveEmptyRowsFromDataTable(dt);
            Hid_AddedSec.Value = "";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Hid_AddedSec.Value += Convert.ToString(dt.Rows[i]["SecurityId"]) + "|";
                    }
                }
            }
            List<SecIds> list = new List<SecIds>();

            if (dt != null)
            {
                if (dt.Rows.Count > 1)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        decimal secid = Convert.ToDecimal(row["SecurityId"]);
                        SecIds secid1 = new SecIds(secid);
                        list.Add(secid1);
                        //list11.Add(Convert.ToDecimal(row["SecurityId"]));
                    }
                }
            }
            XmlDocument xmldoc = new XmlDocument();
            XmlSerializer xs = new XmlSerializer(typeof(List<SecIds>));

            using (MemoryStream stream = new MemoryStream())
            {
                xs.Serialize(stream, list);
                stream.Flush(); stream.Seek(0, SeekOrigin.Begin);
                xmldoc.Load(stream);
            }

            SqlDataAdapter sqlda = new SqlDataAdapter();
            SqlCommand sqlComm = new SqlCommand();
            DataTable sqldt = new DataTable();
            DataView sqldv = new DataView();

            string strCond = Hid_Cond.Value;
            if (strCond.Contains("ISINNumber"))
            {
                strCond = strCond.Replace("ISINNumber", "NSDLAcNumber");
            }
            OpenConn();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandTimeout = 100;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = "ID_SEARCH_SecurityFieldsNew_StockUpdate";
            sqlComm.Parameters.Clear();
            objCommon.SetCommandParameters(sqlComm, "@Ids_Xml", SqlDbType.Xml, 0, 'I', 0, 0, xmldoc.OuterXml);
            objCommon.SetCommandParameters(sqlComm, "@exist", SqlDbType.Bit, 0, 'I', 0, 0, false);
            objCommon.SetCommandParameters(sqlComm, "@ForDate", SqlDbType.SmallDateTime, 4, 'I', 0, 0, objCommon.DateFormat(Convert.ToString(Session["fordate"])));
            objCommon.SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 1000, 'I', 0, 0, strCond);
            if (Hid_IsTypeFlag.Value != "")
            {
                objCommon.SetCommandParameters(sqlComm, "@IsSearchSecurity", SqlDbType.Char, 1, 'I', 0, 0, "1");
            }
            else
            {
                objCommon.SetCommandParameters(sqlComm, "@IsSearchSecurity", SqlDbType.Char, 1, 'I', 0, 0, "");
            }
            objCommon.SetCommandParameters(sqlComm, "@TypeFlag", SqlDbType.Char, 1, 'I', 0, 0, Hid_IsGsec.Value);
            objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, 'O', 0, 0, null);

            sqlComm.ExecuteNonQuery();
            sqlda.SelectCommand = sqlComm;
            sqlda.Fill(sqldt);
            DataTable dtISIN = new DataTable();
            if (Hid_CondSecIds.Value != "")
            {
                dtISIN = ToTable(Hid_CondSecIds.Value);
            }

            System.Data.DataView view_ = new System.Data.DataView(sqldt);
            DataTable dtNew = view_.ToTable(false, "NSDLAcNumber");

            DataTable dtThirdTable = new DataTable();
            dtThirdTable.Columns.Add("NSDLAcNumber", Type.GetType("System.String"));
            string UnMatched = "";
            if (dtISIN.Rows.Count > 0)
            {
                for (int I = 0; I < dtISIN.Rows.Count; I++)
                {
                    string NSDLAcNumber1 = "";
                    string NSDLAcNumber2 = "";
                    bool matched = false;
                    for (int J = 0; J < dtNew.Rows.Count; J++)
                    {
                        NSDLAcNumber1 = Convert.ToString(dtISIN.Rows[I][0].ToString());
                        NSDLAcNumber2 = Convert.ToString(dtNew.Rows[J][0].ToString());
                        if (NSDLAcNumber1.Replace("\r", "") == NSDLAcNumber2)
                        {
                            matched = true;
                        }
                    }
                    if (!matched)
                    {
                        UnMatched += NSDLAcNumber1 + ",";
                    }
                }
            }
            if (UnMatched != "")
            {
                lbl_Message.Text = "ISIN's " + UnMatched + " not found.";
                lbl_Message.ForeColor = System.Drawing.Color.Red;
            }

            sqldv = sqldt.DefaultView;
            sqldv.Sort = "SecurityId desc";
            dg_Selection.DataSource = sqldv;


            dg_Selection.DataBind();

            Session["SecurityTable"] = sqldt;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('" + ex.Message.Replace("'", " ").Replace(Strings.Chr(13), " ").Replace(Strings.Chr(10), " ") + "');", true);
        }
        finally
        {
            CloseConn();
        }
    }


    private void FillSecurityGrid___Old17Aug2017()
    {
        try
        {

            DataTable dt = (DataTable)Session["SecurityMaster"];
            List<SecIds> list = new List<SecIds>();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        decimal secid = Convert.ToDecimal(row["SecurityId"]);
                        SecIds secid1 = new SecIds(secid);
                        list.Add(secid1);
                        //list11.Add(Convert.ToDecimal(row["SecurityId"]));
                    }
                }
            }
            XmlDocument xmldoc = new XmlDocument();
            XmlSerializer xs = new XmlSerializer(typeof(List<SecIds>));

            using (MemoryStream stream = new MemoryStream())
            {
                xs.Serialize(stream, list);
                stream.Flush(); stream.Seek(0, SeekOrigin.Begin);
                xmldoc.Load(stream);
            }

            SqlDataAdapter sqlda = new SqlDataAdapter();
            SqlCommand sqlComm = new SqlCommand();
            DataTable sqldt = new DataTable();
            DataView sqldv = new DataView();

            string strCond = Hid_Cond.Value;
            if (strCond.Contains("ISINNumber"))
            {
                strCond = strCond.Replace("ISINNumber", "NSDLAcNumber");
            }

            OpenConn();

            sqlComm.Connection = sqlConn;
            sqlComm.CommandTimeout = 100;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = "ID_SEARCH_StockUpdateMaster_UpdatedStock";
            sqlComm.Parameters.Clear();
            objCommon.SetCommandParameters(sqlComm, "@UserTypeId", SqlDbType.Int, 4, 'I', 0, 0, Convert.ToInt32(Session["UserTypeId"]));
            objCommon.SetCommandParameters(sqlComm, "@UserId", SqlDbType.Int, 4, 'I', 0, 0, Convert.ToInt32(Session["UserId"]));
            objCommon.SetCommandParameters(sqlComm, "@Cond", SqlDbType.VarChar, 1000, 'I', 0, 0, strCond);
            objCommon.SetCommandParameters(sqlComm, "@RET_CODE", SqlDbType.Int, 4, 'O', 0, 0, null);

            sqlComm.ExecuteNonQuery();
            sqlda.SelectCommand = sqlComm;
            sqlda.Fill(sqldt);
            sqldv = sqldt.DefaultView;

            // dg_Selection.PageIndex = intPageIndex;

            dg_Selection.DataSource = sqldv;
            dg_Selection.DataBind();

            Session["SecurityTable"] = sqldt;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('" + ex.Message.Replace("'", " ").Replace(Strings.Chr(13), " ").Replace(Strings.Chr(10), " ") + "');", true);
        }
        finally
        {
            CloseConn();
        }
    }

    private void OpenConn()
    {
        if (sqlConn == null)
        {
            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
            sqlConn.Open();
        }
        else if (sqlConn.State == ConnectionState.Closed)
        {
            sqlConn.ConnectionString = ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString();
            sqlConn.Open();
        }
    }

    private void CloseConn()
    {
        if (sqlConn == null)
            return;
        if (sqlConn.State == ConnectionState.Open)
            sqlConn.Close();
    }

    private void FillGridWithCondition(int intPageIndex)
    {
        try
        {
            clsSearch objSearch = new clsSearch();
            DataTable dtGrid = new DataTable();
            String strCond = Hid_Cond.Value;
            if (Convert.ToString(Session["StockUpdated"]) == "Y")
            {
                FillSecurityGrid_StockUpdateMaster();
                dtGrid = (DataTable)Session["SecurityTable"];
                DataView DvSec = new DataView(dtGrid);
                DateTime today = DateTime.Today; // As DateTime
                string s_today = today.ToString("dd/MM/yyyy"); // As String
                DvSec.RowFilter = "StockDate='" + s_today + "'";
                dg_Selection.PageIndex = intPageIndex;
                dg_Selection.DataSource = DvSec;
                dg_Selection.DataBind();
                Hid_Cond.Value = "";
            }
            else
            {
                FillSecurityGrid();
                dtGrid = (DataTable)Session["SecurityTable"];
                DataView DvSec = new DataView(dtGrid);

                dg_Selection.PageIndex = intPageIndex;
                dg_Selection.DataSource = DvSec;
                dg_Selection.DataBind();
                Hid_Cond.Value = "";
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('" + ex.Message.Replace("'", " ").Replace(Strings.Chr(13), " ").Replace(Strings.Chr(10), " ") + "');", true);
        }
    }

    protected void dg_Selection_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            int intPageIndex = e.NewPageIndex;
            FillGridWithCondition(intPageIndex);

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            //  Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('" + ex.Message.Replace("'", " ").Replace(Strings.Chr(13), " ").Replace(Strings.Chr(10), " ") + "');", true); 
        }
    }

    protected void dg_Selection_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {

            ImageButton imgBtn = default(ImageButton);
            GridViewRow gvRow = default(GridViewRow);
            DataRow dr = null; DataTable dt = null;

            dt = (DataTable)Session["SecurityTable"];

            if (e.CommandName == "SelectRow")
            {
                imgBtn = e.CommandSource as ImageButton;
                gvRow = (GridViewRow)imgBtn.Parent.Parent;

                // gvRow.RowIndex;
                dr = dt.Rows[gvRow.DataItemIndex];
                // Hid_Security.Value = dr["SecurityName"];  
                //  txt_SecurityName.Text = dr["SecurityName"];   
                // Hid_SecurityId.Value = dr["SecurityId"];   
                intSelIndex = gvRow.DataItemIndex;
                //FillGrid("", 0);
                if (intSelIndex == gvRow.RowIndex)
                {
                    gvRow.BackColor = System.Drawing.Color.FromName("#D1E4F8");
                    imgBtn.ImageUrl = "~/Images/images.jpg";
                }

                //ElseIf e.CommandName = "ShowStock" Then   
                //    imgBtn = TryCast(e.CommandSource, ImageButton)  
                //    gvRow = imgBtn.Parent.Parent  
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('" + ex.Message.Replace("'", " ").Replace(Strings.Chr(13), " ").Replace(Strings.Chr(10), " ") + "');", true);
        }
    }

    //protected void dg_Selection_RowDataBound(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e) 
    //{
    //    try
    //    {

    //        int IntSecurityId = 0; 
    //        string strSecurityName = null;           

    //        //if (e.Row.RowType == ListItemType.AlternatingItem || e.Row.RowType == ListItemType.Item)
    //        //{
    //        //    ImageButton img = default(ImageButton);
    //        //    //LinkButton LinkSecName = default(LinkButton);
    //        //    img = (ImageButton)e.Row.FindControl("img_Select");

    //        //    //if (intSelIndex == e.Row.RowIndex)
    //        //    //{
    //        //    //    e.Row.BackColor = System.Drawing.Color.FromName("#D1E4F8");
    //        //    //    img.ImageUrl = "~/Images/images.jpg"; 
    //        //    //}
    //        //    ////if (!string.IsNullOrEmpty(Strings.Trim(Request.QueryString("rowIndex") + "")))
    //        //    ////{ 
    //        //    ////    img.Visible = false; 
    //        //    ////}

    //        //}

    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write(ex.Message);
    //        //Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('" + ex.Message.Replace("'", " ").Replace(Strings.Chr(13), " ").Replace(Strings.Chr(10), " ") + "');", true);
    //    }
    //}

    protected void btn_Search_Click(object sender, EventArgs e)
    {
        try
        {
            Hid_IsTypeFlag.Value = "1";
            FillGridWithCondition(0);
        }
        catch (Exception ex)
        {

        }
    }

    protected void btn_ShowAll_Click(object sender, EventArgs e)
    {
        try
        {
           
            FillGridWithCondition(0);
            lbl_Message.Text = "";
        }
        catch (Exception ex)
        {

        }
    }

    protected void btn_Add_Click(object sender, EventArgs e)
    {

        // FillTempSecurityTable()
    }


    private DataTable ToTable(string Ids)
    {
        DataTable dataTable = new DataTable();
        var lines = Ids.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var colname in lines[0].Split(','))
        {
            //dataTable.Columns.Add(new DataColumn(colname));
            dataTable.Columns.Add(new DataColumn("NSDLAcNumber"));
        }
        foreach (var row in lines.Where((r, i) => i > 0))
        {
            dataTable.Rows.Add(row.Split(','));
        }
        return dataTable;
    }


    public static DataTable CompareTwoDataTable(DataTable dt1, DataTable dt2)
    {
        dt1.Merge(dt2);
        //DataTable d3 = dt1.GetChanges();
        DataTable d3 = dt1.DefaultView.ToTable(true, "NSDLACNumber");
        return d3;
    }
}
