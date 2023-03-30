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


public partial class Forms_SendMailToDealer : System.Web.UI.Page
{

    int intSelIndex = -1;
    // Dim objCommon As New clsCommonFuns
    clsCommonFuns objCommon = new clsCommonFuns();
    // Dim sqlConn As SqlConnection
    SqlConnection sqlConn = new SqlConnection();

    public class UserIds
    {
        public int Id;
        // public SecIds();

        public UserIds() { }
        public UserIds(int Id)
        {
            this.Id = Id;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Hid_ForDate.Value = Request.QueryString["fordate"].ToString();
            //System.Web.UI.WebControls.HiddenField uc1 =this.Parent.FindControl("Hid_SecIds");
            //  System.Web.UI.WebControls.HiddenField=  Parent.FindControl("Hid_SecIds");


            // Hid_SecIds.Value = Request.Params["secids"];
            //Response.Buffer = true ;
            //Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D);
            //Response.Expires = -1500;
            //Response.CacheControl = "no-cache";
            //Response.AddHeader("Pragma", "no-cache");
            //Response.AddHeader("Cache-Control", "no-cache");
            //Response.AddHeader("Cache-Control", "no-store");

            Hid_SecIds.Value = Convert.ToString(Session["Secids"]);
            btn_Send.Attributes.Add("onclick", "return Validate();");

            FillSecurityGrid();

            // Request.Params["secids"] 


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

    private void FillSecurityGrid()
    {
        try
        {

            DataTable dt = (DataTable)Session["SecurityMaster"];
            DataTable dtSec = dt.Clone();

            //  int[] arrSecId ;
            //  arrSecId = Hid_SecIds.Value.ToString().Split[','];
            string[] arrSecId = Hid_SecIds.Value.Split(new char[] { ',' });

            foreach (DataRow row in dt.Rows)
            {
                if (Array.IndexOf(arrSecId, row["SecurityId"].ToString()).ToString() != "-1")
                {
                    dtSec.ImportRow(row);
                }
            }

            dg_Selection.DataSource = dtSec;
            dg_Selection.DataBind();

            Session["SecurityTable"] = dtSec;

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
            string strCond = Hid_Cond.Value;

            dtGrid = (DataTable)Session["SecurityTable"];

            DataView DvSec = new DataView(dtGrid);
            DvSec.RowFilter = strCond;

            dg_Selection.PageIndex = intPageIndex;
            dg_Selection.DataSource = DvSec;
            dg_Selection.DataBind();

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
            //FillGrid("", 0);
            //  SetComboValues()
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


    protected void Page_Unload(object sender, System.EventArgs e)
    {

        Session["SecIds"] = null;

    }

    //protected void btn_Sumbit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        //from address
    //       // string  fromAddress = "spoonam.sinha@gmail.com";
    //        string fromAddress =Convert.ToString(Session["EmailId"]);

    //        //to address
    //        string toAddress = "spoonam.sinha@gmail.com";

    //        //subject
    //        String subject = "Subject";

    //        String body = "From: " + "Name";
    //        body += "Email: " + "YourEmail.Text" + "vbLf";
    //        body += "Subject: " + "Subject:"  ;
    //        body += "Question: "  + "Comments.Text" ;

    //        MailHelper.SendMailMessage(fromAddress, toAddress, "", "", subject, body);

    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write(ex.Message);
    //    }
    //}

    protected void btn_Send_Click(object sender, EventArgs e)
    {

        SqlCommand sqlComm = new SqlCommand();

        try
        {


            //get email id of dealers
            ListItem itm = srh_Staff.SelectListBox.Items.FindByText("");

            if (itm != null)
                srh_Staff.SelectListBox.Items.Remove(itm);

            List<UserIds> list = new List<UserIds>();

            foreach (ListItem lstItem in srh_Staff.SelectListBox.Items)
            {
                Int32 userid = Convert.ToInt32(lstItem.Value);
                UserIds userid1 = new UserIds();
                userid1.Id = userid;
                list.Add(userid1);
            }

            XmlDocument xmldoc = new XmlDocument();
            XmlSerializer xs = new XmlSerializer(typeof(List<UserIds>));

            using (MemoryStream stream = new MemoryStream())
            {
                xs.Serialize(stream, list);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                xmldoc.Load(stream);
            }

            SqlDataAdapter sqlda = new SqlDataAdapter();
            DataTable dtUser = new DataTable();
            DataView sqldv = new DataView();
            OpenConn();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandTimeout = 100;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = "ID_Fill_DealerEmailIds";
            sqlComm.Parameters.Clear();

            objCommon.SetCommandParameters(sqlComm, "@Ids_Xml", SqlDbType.Xml, 0, 'I', 0, 0, xmldoc.OuterXml);
            objCommon.SetCommandParameters(sqlComm, "@intFlag", SqlDbType.Int, 4, 'O', 0, 0, 0);
            sqlComm.ExecuteNonQuery();
            sqlda.SelectCommand = sqlComm;
            sqlda.Fill(dtUser);
            sqldv = dtUser.DefaultView;

            //from address
            // string  fromAddress = "spoonam.sinha@gmail.com";
            string fromAddress = Convert.ToString(Session["EmailId"]);
            //subject
            String subject = "List of Securities for Sale ";

            String body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
            body += "<html><body> The following is a list of Securities for Sale with Quantum and Price :";
            //body += GridViewToHtml(dg_Selection);

            body += "<br/><br/><TABLE cellspacing='1' width='100%' border='1'>";
            // body += "<TR align='left'><TH>Security Name</TH><TH>ISIN Number</TH><TH>Quantum</TH><TH>Selling Price</TH></TR>";
            body += "<TR align='left'><TD>Security Name</TD><TD>ISIN Number</TD><TD>Quantum</TD><TD>Selling Price</TD></TR>";

            // body += "<TR><TD>First Name:</TD><TD>" + "fname" + "</TD><TD></TD></TR>";
            // body += "<TR><TD>Last Name:</TD><TD>" + "lname" + "</TD></TR>";

            for (Int16 I = 0; I <= dg_Selection.Rows.Count - 1; I++)
            {

                //  txt_SecurityName
                String secname = ((TextBox)dg_Selection.Rows[I].FindControl("txt_SecurityName")).Text;

                body += "<TR><TD> " + secname + " </TD>";
                body += "<TD>" + dg_Selection.Rows[I].Cells[1].Text + "</TD>";
                body += "<TD>" + dg_Selection.Rows[I].Cells[3].Text + "</TD>";
                body += "<TD>" + dg_Selection.Rows[I].Cells[2].Text + "</TD></TR>";
            }

            body += "</TABLE><br/><br/>";

            body += Environment.NewLine;

            //body += "<TR><TD>Email:</TD><TD>" + "email" + "</TD></TR></TABLE>";

            body += " From, <br/>" + Convert.ToString(Session["Username"]);
            body += "</body></html>";

            if (fromAddress != "")
            {
                foreach (DataRow row in dtUser.Rows)
                {
                    string toAddress = row["EmailId"].ToString();
                    if (toAddress != "")
                    {
                        MailHelper.SendMailMessage(fromAddress, toAddress, "", "", subject, body,"","");
                    }
                }
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('Mail has been sent Successfully !');", true);

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private string GridViewToHtml(GridView gv)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gv.RenderControl(hw);
        return sb.ToString();
    }
}
