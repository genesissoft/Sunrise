using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using System.Data.SqlClient;
using System.Drawing;
using log4net;

public partial class Forms_CRMInteractionDetails : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection();
    ClsCommonCS objCommon = new ClsCommonCS();
    DataTable dtManageby = new DataTable();
    string _StrSearch = "";
    Int64 Id;
    Int16 Pagecnt;
    public String FileName;
    string FromDt, ToDt;
    Util objUtil = new Util();
    String PgName = "$CRMInteractionDetails$";

    protected void Page_Load(object sender, EventArgs e)
    {
        //fillgrid function : It shows data from ClientEntry table according to specified date 
        //also grid has functionality for edit,delete meeting details
        try
        {
            if (Convert.ToString(Session["UserName"]) == "")
            {
                Response.Redirect("Login.aspx", false);
                return;
            }
            if (Page.IsPostBack == false)
            {
                dtManageby = FindManageBy();
                if (dtManageby.Rows.Count > 0)
                {
                    Hid_ManageBy.Value = Convert.ToString(dtManageby.Rows[0]["NAMEOFUSER"]);
                }
                Hid_PageCnt.Value = "0";
                //btn_Go.Attributes.Add("onclick", "return Validation();");
                //btn_ShowAll.Attributes.Add("onclick", "return Validation();");
                if (Request.QueryString["InteractionId"] != "")
                {
                    Id = Convert.ToInt64(Request.QueryString["InteractionId"]);
                }
                bool isNumeric;
                int i;
                isNumeric = int.TryParse(Request.QueryString["Pagecnt"], out i);
                if (isNumeric == true)
                {
                    Pagecnt = Convert.ToInt16(Request.QueryString["Pagecnt"]);
                }
                dg_CRMDetails.Attributes.Add("bordercolor", "#BBC294");
                if (Page.IsPostBack == false)
                {
                    if (Convert.ToString(Session["UserType"]).ToLower() != "administrator")
                    {
                        //var QueryData = Request.QueryString["Val"];
                        //if (Convert.ToString(Session["CompName"]).ToUpper() == "TRUST INVESTMENT")
                        //{
                        //    //cbo_ClientType.Items.Add(new ListItem("Issuer", "IS"));
                        //    _StrSearch = " AND ClientDiff <> 'CU' ";// AND UM.UserId=" + Session["UserId"];
                        //    Hid_SelectionId.Value = "I";
                        //}
                        //else
                        //{
                        //    cbo_ClientType.Items.Add(new ListItem("Customer", "CU"));
                        //    _StrSearch = "AND ClientDiff = 'CU' "; // AND UM.UserId=" + Session["UserId"];
                        //    Hid_SelectionId.Value = "C";
                        //}
                        cbo_ClientType.Items.Add(new ListItem("Customer", "CU"));
                        FromDt = txt_Fromdate.Text = DateTime.Now.AddMonths(-3).ToString("dd/MM/yyyy");
                        ToDt = txt_ToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        fillgrid(_StrSearch, Pagecnt, FromDt, ToDt);
                    }
                    else
                    {
                        //cbo_ClientType.Items.Add(new ListItem("Issuer", "IS"));
                        cbo_ClientType.Items.Add(new ListItem("Customer", "CU"));
                        Hid_SelectionId.Value = "A";
                        FromDt = txt_Fromdate.Text = DateTime.Now.AddMonths(-3).ToString("dd/MM/yyyy");
                        ToDt = txt_ToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        fillgrid("", Pagecnt, FromDt, ToDt);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "Page_Load", "Error in Page_Load", "", ex);
            Response.Write(ex.Message);
        }
    }

    private void fillgrid(string _StrSearch, int intPageIndex, String FromDt, String ToDt)
    {
        DataTable dt = new DataTable();
        try
        {
            dg_CRMDetails.CurrentPageIndex = intPageIndex;
            Hid_PageCnt.Value = Convert.ToString(intPageIndex);
            dt = objCommon.FillInteractionDetails("CRM_Fill_ClientInteractionDetails", 0, _StrSearch, FromDt, ToDt);
            dg_CRMDetails.DataSource = dt;
            dg_CRMDetails.DataBind();
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "fillgrid", "Error in fillgrid", "", ex);
            Response.Write(ex.Message);
        }
    }

    protected void dg_CRMDetails_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        int ClientId = 0;
        Int64 InteractionId = 0;
        String  Temp, Name, MeetingDate, Access;
        ImageButton imgbtn = default(ImageButton);
        ImageButton imgbtnDel = default(ImageButton);
        //Int64 intManageById = 0;
        try
        {
            if (e.Item.ItemType != ListItemType.Header & e.Item.ItemType != ListItemType.Footer)
            {
                InteractionId = Convert.ToInt64(((Label)e.Item.FindControl("lbl_InteractionId")).Text);
                imgbtn = (ImageButton)e.Item.FindControl("img_Edit");
                imgbtnDel = (ImageButton)e.Item.FindControl("img_Delete");
                Access = Convert.ToString(((Label)e.Item.FindControl("lbl_Access")).Text);

                if (Id == InteractionId)
                {
                    //e.Item.BackColor = Color.FromName("#F7EED9");
                    e.Item.BackColor = Color.LightBlue;
                    imgbtn.Focus();
                }

                if (Session["NameOfUser"].ToString().ToLower() != Access.ToLower())
                {
                    imgbtn.Attributes.Add("onclick", " return Update('" + InteractionId + "','" + "U" + "')");
                    imgbtnDel.Attributes.Add("onclick", "return Delete('" + InteractionId + "','" + "N" + "')");
                    imgbtnDel.Visible = false;
                }
                else
                {
                    imgbtn.Attributes.Add("onclick", "return Update('" + InteractionId + "','" + "U" + "')");
                    imgbtnDel.Attributes.Add("onclick", "return Delete('" + InteractionId + "','" + "U" + "')");
                }
            }
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "dg_CRMDetails_ItemDataBound", "Error in dg_CRMDetails_ItemDataBound", "", ex);
            Response.Write(ex.Message);
        }
    }

    private void OpenConn()
    {
        if (con == null)
        {

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
            con.Open();
        }
        else if (con.State == ConnectionState.Closed)
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString();
            con.Open();
        }
    }

    private void CloseConn()
    {
        if (con == null)
        { return; }
        if (con.State == ConnectionState.Open)
        { con.Close(); }
    }

    public DataTable FindManageBy()
    {
        SqlCommand sqlComm = new SqlCommand();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        DataTable dt = new DataTable();
        try
        {
            sqlComm.CommandText = "CRM_FILL_MANAGEBY";
            sqlComm.CommandType = CommandType.StoredProcedure;
            OpenConn();
            sqlComm.Connection = con;
            sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 8).Value = Convert.ToInt64(Session["UserId"]);
            sqlComm.ExecuteNonQuery();
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "FindManageBy", "Error in FindManageBy", "", ex);
            Response.Write(ex.Message);
        }
        finally
        {
            CloseConn();
        }
        return dt;
    }

    protected void btn_Search_Click(object sender, EventArgs e)
    {
        try
        {
            FromDt = Convert.ToString(txt_Fromdate.Text).Trim();
            ToDt = Convert.ToString(txt_ToDate.Text).Trim();
            string StrSearch = "";
            string SeletedVal = cbo_ClientType.SelectedValue;
            lbl_msg.Text = "";
            if (Convert.ToString(Session["UserType"]).ToLower() != "administrator")
            {
                //StrSearch = "AND ClientDiff='" + SeletedVal + "'" + " AND TempClient Like '" + (txt_Search.Text) + "%'"; // +" AND UM.UserId=" + Session["UserId"];
                StrSearch = " AND CustomerName Like '" + (txt_Search.Text) + "%'";
                fillgrid(StrSearch, 0, FromDt, ToDt);
            }
            else
            {
                //StrSearch = "AND ClientDiff='" + SeletedVal + "'" + " AND TempClient Like '" + (txt_Search.Text) + "%'";
                StrSearch = " AND CustomerName  Like '" + (txt_Search.Text) + "%'";
                fillgrid(StrSearch, 0, FromDt, ToDt);
            }
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "btn_Search_Click", "Error in btn_Search_Click", "", ex);
            Response.Write(ex.Message);
        }

    }

    protected void btn_ShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            txt_Search.Text = "";
            String _StrSearch;
            lbl_msg.Text = "";

            FromDt = Convert.ToString(txt_Fromdate.Text).Trim();
            ToDt = Convert.ToString(txt_ToDate.Text).Trim();
            if (Convert.ToString(Session["UserType"]).ToLower() != "administrator")
            {
                //var QueryData = Request.QueryString["Val"];
                //if (Convert.ToString(Session["CompName"]).ToUpper() == "TRUST INVESTMENT")
                //{
                //    _StrSearch = " AND ClientDiff <> 'CU' ";// AND UM.UserId=" + Session["UserId"];
                //}
                //else
                //{
                //    _StrSearch = " AND ClientDiff = 'CU' ";// AND UM.UserId=" + Session["UserId"];
                //}
                fillgrid("", 0, FromDt, ToDt);
            }
            else
            {
                fillgrid("", 0, FromDt, ToDt);
            }
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "btn_ShowAll_Click", "Error in btn_ShowAll_Click", "", ex);
            Response.Write(ex.Message);
        }
    }

    protected void dg_CRMDetails_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        int _intresult;
        lbl_msg.Text = "";
        try
        {
            DataTable dt = new DataTable();
            if (e.CommandName == "Delete")
            {
                _intresult = Delete();
                if (_intresult > 0)
                {
                    FromDt = Convert.ToString(txt_Fromdate.Text);
                    ToDt = Convert.ToString(txt_ToDate.Text);

                    txt_Search.Text = "";
                    String _StrSearch;
                    if (Convert.ToString(Session["UserType"]).ToLower() != "administrator")
                    {
                        //var QueryData = Request.QueryString["Val"];
                        //if (Convert.ToString(Session["CompName"]).ToUpper() == "TRUST INVESTMENT")
                        //{
                        //    _StrSearch = " AND ClientDiff <> 'CU' ";// AND UM.UserId=" + Session["UserId"];
                        //}
                        //else
                        //{
                        //    _StrSearch = " AND ClientDiff = 'CU' ";// AND UM.UserId=" + Session["UserId"];
                        //}
                        fillgrid("", Convert.ToInt32(Hid_PageCnt.Value), FromDt, ToDt);
                    }
                    else
                    {
                        fillgrid("", Convert.ToInt32(Hid_PageCnt.Value), FromDt, ToDt);
                    }


                    lbl_msg.Text = "Record Deleted Successfully.";
                }
            }
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "dg_CRMDetails_ItemCommand", "Error in dg_CRMDetails_ItemCommand", "", ex);
            Response.Write(ex.Message);
        }
    }

    public int Delete()
    {
        int _intResult = 0;
        try
        {
            SqlCommand sqlComm = new SqlCommand();
            SqlDataAdapter sqlDa = new SqlDataAdapter();
            DataTable dt = new DataTable();
            sqlComm.CommandText = "CRM_Delete_InteractionDetail";
            sqlComm.CommandType = CommandType.StoredProcedure;
            OpenConn();
            sqlComm.Connection = con;
            sqlComm.Parameters.Add("@InteractionId", SqlDbType.BigInt, 8).Value = Convert.ToInt64(Hid_Id.Value);
            _intResult = sqlComm.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "Delete", "Error in Delete", "", ex);
            Response.Write(ex.Message);
        }
        finally
        {
            CloseConn();
        }
        return _intResult;
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        CloseConn();
        if (con == null)
        {
            con.Dispose();
            System.GC.Collect();
        }
    }

    protected void dg_CRMDetails_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        try
        {
            lbl_msg.Text = "";
            FromDt = txt_Fromdate.Text;
            ToDt = txt_ToDate.Text;
            Hid_PageCnt.Value = Convert.ToString(e.NewPageIndex);
            if (Convert.ToString(txt_Search.Text) != "")
            {
                string StrSearch = "";
                string SeletedVal = cbo_ClientType.SelectedValue;

                if (Convert.ToString(Session["UserType"]).ToLower() != "administrator")
                {
                    //StrSearch = "AND ClientDiff='" + SeletedVal + "'" + " AND TempClient Like '" + (txt_Search.Text) + "%'";// +" AND UM.UserId=" + Session["UserId"];
                    StrSearch =  " AND CustomerName Like '" + (txt_Search.Text) + "%'";
                    fillgrid(StrSearch, e.NewPageIndex, FromDt, ToDt);
                }
                else
                {
                    //StrSearch = "AND ClientDiff='" + SeletedVal + "'" + " AND TempClient Like '" + (txt_Search.Text) + "%'";
                    StrSearch = " AND CustomerName Like '" + (txt_Search.Text) + "%'";
                    fillgrid(StrSearch, e.NewPageIndex, FromDt, ToDt);
                }
            }
            else
            {
                fillgrid("", e.NewPageIndex, FromDt, ToDt);
            }
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "dg_CRMDetails_PageIndexChanged", "Error in dg_CRMDetails_PageIndexChanged", "", ex);
            Response.Write(ex.Message);
        }


    }

    protected void btn_Go_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_msg.Text = "";
            txt_Search.Text = "";
            String _StrSearch;

            FromDt = txt_Fromdate.Text;
            ToDt = txt_ToDate.Text;

            if (Convert.ToString(Session["UserType"]).ToLower() != "administrator")
            {
                //var QueryData = Request.QueryString["Val"];
                //if (Convert.ToString(Session["CompName"]).ToUpper() == "TRUST INVESTMENT")
                //{
                //    _StrSearch = " AND ClientDiff <> 'CU' ";// AND UM.UserId=" + Session["UserId"];
                //}
                //else
                //{
                //    _StrSearch = " AND ClientDiff = 'CU' ";// AND UM.UserId=" + Session["UserId"];
                //}
                fillgrid("", Convert.ToInt32(Hid_PageCnt.Value), FromDt, ToDt);
            }
            else
            {
                fillgrid("", Convert.ToInt32(Hid_PageCnt.Value), FromDt, ToDt);
            }
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "btn_Go_Click", "Error in btn_Go_Click", "", ex);
            Response.Write(ex.Message);
        }

    }

    protected void btn_Add_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("CRMEntry.aspx", false);
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog(PgName, "btn_Add_Click", "Error in btn_Add_Click", "", ex);
            Response.Write(ex.Message);
        }

        //"CRMEntry.aspx?" + "ClientDiff=" + ClientDiff;
    }
}