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


public partial class Forms_TransferDPStock : System.Web.UI.Page
{
    Common objComm = new Common();
    clsCommonFuns objCommon = new clsCommonFuns();
    DataTable dt;
    string strMsg = "";
    SqlDataAdapter da;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDated.Text = DateTime.Now.ToString("dd/MM/yyyy");
            SetControls();
            FillCombo();
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string strId = objCommon.DecryptText(HttpUtility.UrlDecode(Request.QueryString["Id"]));
                Hid_Id.Value = strId;
                if (objComm.Val(Hid_Id.Value) > 0)
                {
                    FillFields();
                }

            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "test", "ReportType();", true);
        }
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "test", "ReportType();", true);
    }

    private void FillFields()
    {
        string stkqty = "";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        List<SqlParameter> lstParam = new List<SqlParameter>();

        try
        {
            lstParam.Add(new SqlParameter("@DPTransferId", objComm.Val(Hid_Id.Value) > 0 ? Hid_Id.Value : null));
            ds = objComm.FillDetails(lstParam, "ID_Fill_DPTransferStockDetails");
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                txtDated.Text = objComm.Trim(dt.Rows[0]["TransferDate"].ToString());
                cboFromDPId.SelectedValue = objComm.Trim(dt.Rows[0]["FromDPId"].ToString());
                cboToSGLId.SelectedValue = objComm.Trim(dt.Rows[0]["ToSGLId"].ToString());
                Srh_NameofSecurity.SearchTextBox.Text = objComm.Trim(dt.Rows[0]["SecurityName"].ToString());
                Srh_NameofSecurity.SelectedId = Convert.ToString(dt.Rows[0]["SecurityId"]);
                stkqty = getAvailableStockQtyDP();
                txtStkQty.Text = stkqty;
                btn_Save.Visible = false;
                btn_Save.Text = " Update ";
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public void SetControls()
    {
        //Srh_NameofSecurity.Columns.Add("SecurityName");
        //Srh_NameofSecurity.Columns.Add("SecurityTypeName");
        //Srh_NameofSecurity.Columns.Add("SecurityIssuer");
        //Srh_NameofSecurity.Columns.Add("SM.SecurityId");
    }

    private void FillCombo()
    {
        DataSet dsDP = new DataSet();
        DataSet dsDP1 = new DataSet();
        DataSet dsSGL = new DataSet();
        DataSet dsSGL1 = new DataSet();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        List<SqlParameter> lstParam1 = new List<SqlParameter>();
        List<SqlParameter> lstParam2 = new List<SqlParameter>();
        List<SqlParameter> lstParam3 = new List<SqlParameter>();

        try
        {
            lstParam.Add(new SqlParameter("@CompId", objComm.Val(Session["CompId"].ToString()) > 0 ? Session["CompId"].ToString() : null));
            lstParam1.Add(new SqlParameter("@CompId", objComm.Val(Session["CompId"].ToString()) > 0 ? Session["CompId"].ToString() : null));
            lstParam2.Add(new SqlParameter("@CompId", objComm.Val(Session["CompId"].ToString()) > 0 ? Session["CompId"].ToString() : null));
            lstParam3.Add(new SqlParameter("@CompId", objComm.Val(Session["CompId"].ToString()) > 0 ? Session["CompId"].ToString() : null));
            dsDP = objComm.FillDetails(lstParam, "ID_Fill_DmatClientIdCombo");
            dsSGL = objComm.FillDetails(lstParam1, "ID_Fill_SGLIdCombo");
            dsDP1 = objComm.FillDetails(lstParam2, "ID_Fill_DmatClientIdCombo");
            dsSGL1 = objComm.FillDetails(lstParam3, "ID_Fill_SGLIdCombo");
            if (dsDP != null && dsDP.Tables.Count > 0)
            {
                cboFromDPId.DataSource = dsDP.Tables[0];
                cboFromDPId.DataValueField = "Id";
                cboFromDPId.DataTextField = "Name";
                cboFromDPId.DataBind();
            }
            if (dsSGL != null && dsSGL.Tables.Count > 0)
            {
                cboToSGLId.DataSource = dsSGL.Tables[0];
                cboToSGLId.DataValueField = "Id";
                cboToSGLId.DataTextField = "Name";
                cboToSGLId.DataBind();
            }

            if (dsDP1 != null && dsDP1.Tables.Count > 0)
            {
                cboToDPId.DataSource = dsDP1.Tables[0];
                cboToDPId.DataValueField = "Id";
                cboToDPId.DataTextField = "Name";
                cboToDPId.DataBind();
            }
            if (dsSGL1 != null && dsSGL1.Tables.Count > 0)
            {
                cboFromSGLId.DataSource = dsSGL1.Tables[0];
                cboFromSGLId.DataValueField = "Id";
                cboFromSGLId.DataTextField = "Name";
                cboFromSGLId.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void cboFromDPId_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string stkqty = "";
        try
        {
            stkqty = getAvailableStockQtyDP();
            txtStkQty.Text = stkqty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void cboFromSGLId_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string stkqty = "";
        try
        {
            stkqty = getAvailableStockQtyDP();
            txtStkQty.Text = stkqty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string getAvailableStockQtyDP()
    {
        DataSet ds = new DataSet();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        string StkQty = "";
        try
        {
            lstParam.Add(new SqlParameter("@CompId", objComm.Val(Session["CompId"].ToString()) > 0 ? Session["CompId"].ToString() : null));
            lstParam.Add(new SqlParameter("@DMatId", objComm.Val(cboFromDPId.SelectedValue) > 0 ? cboFromDPId.SelectedValue : null));
            lstParam.Add(new SqlParameter("@SecurityId", objComm.Val(Srh_NameofSecurity.SelectedId) > 0 ? Srh_NameofSecurity.SelectedId : null));
            lstParam.Add(new SqlParameter("@SGLId", objComm.Val(cboFromSGLId.SelectedValue) > 0 ? cboFromSGLId.SelectedValue : null));
            lstParam.Add(new SqlParameter("@Asondate", objComm.Trim(txtDated.Text) != "" ? txtDated.Text : null));
            lstParam.Add(new SqlParameter("@TransferType", objComm.Trim(rbl_TypeOFTransfer.SelectedValue)));
            ds = objComm.FillDetails(lstParam, "ID_Get_DPTransferStockQty");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                StkQty = objComm.Trim(ds.Tables[0].Rows[0]["AvailableQty"].ToString());

            }
            else
            {
                StkQty = "0";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return StkQty;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (SaveUpdate("ID_InsertUpdate_TransferStockDP"))
            {
                Response.Redirect("TransferDPStockDetails.aspx", false);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("TransferDPStockDetails.aspx", false);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private bool SaveUpdate(string strProc)
    {
        List<SqlParameter> lstParam = new List<SqlParameter>();
        bool blnReturn = false;
        try
        {
            lstParam.Add(new SqlParameter("@DPTransferId", objComm.Val(Hid_Id.Value) > 0 ? Hid_Id.Value : null));
            if (rbl_TypeOFTransfer.SelectedValue == "DPToSGL")
            {
                lstParam.Add(new SqlParameter("@FromDPId", objComm.Val(cboFromDPId.SelectedValue) > 0 ? cboFromDPId.SelectedValue : null));
                lstParam.Add(new SqlParameter("@ToSGLId", objComm.Val(cboToSGLId.SelectedValue) > 0 ? cboToSGLId.SelectedValue : null));

            }
            if (rbl_TypeOFTransfer.SelectedValue == "DPToDP")
            {
                lstParam.Add(new SqlParameter("@FromDPId", objComm.Val(cboFromDPId.SelectedValue) > 0 ? cboFromDPId.SelectedValue : null));
                lstParam.Add(new SqlParameter("@ToDPId", objComm.Val(cboToDPId.SelectedValue) > 0 ? cboToDPId.SelectedValue : null));

            }
            if (rbl_TypeOFTransfer.SelectedValue == "SGLToDP")
            {
                lstParam.Add(new SqlParameter("@FromSGLId", objComm.Val(cboFromSGLId.SelectedValue) > 0 ? cboFromSGLId.SelectedValue : null));
                lstParam.Add(new SqlParameter("@ToDPId", objComm.Val(cboToDPId.SelectedValue) > 0 ? cboToDPId.SelectedValue : null));

            }

            lstParam.Add(new SqlParameter("@CompId", objComm.Val(Session["CompId"].ToString()) > 0 ? Session["CompId"].ToString() : null));
            lstParam.Add(new SqlParameter("@SecurityId", objComm.Val(Srh_NameofSecurity.SelectedId) > 0 ? Srh_NameofSecurity.SelectedId : null));
            lstParam.Add(new SqlParameter("@UserId", objComm.Val(Session["UserId"].ToString()) > 0 ? Session["UserId"].ToString() : null));
            lstParam.Add(new SqlParameter("@TransferQty", objComm.Val(txtTransferQty.Text) > 0 ? txtTransferQty.Text : null));
            lstParam.Add(new SqlParameter("@TransferDate", objComm.Trim(txtDated.Text) != "" ? txtDated.Text : null));
            lstParam.Add(new SqlParameter("@TransferType", objComm.Trim(rbl_TypeOFTransfer.SelectedValue)));

            if (objComm.InsertUpdateDetails(lstParam, strProc) > 0)
            {
                blnReturn = true;
            }
            return blnReturn;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
        }
    }
}
