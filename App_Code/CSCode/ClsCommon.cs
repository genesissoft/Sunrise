using System;
using System.IO;
using System.Text;
using System.Data;
using System.Configuration;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Security.Cryptography;
//using System.Management;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;

public class ClsCommon
{
    public DataSet FillAllCombo(string strProc)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        SqlCommand sqlComm = new SqlCommand();
        DataSet ds = new DataSet();

        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProc;
            sqlComm.Parameters.Clear();
            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlComm);
            sqlDa.Fill(ds);
            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
            }
            sqlConn = null;
            sqlComm = null;
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

    public DataSet FillDetails(List<SqlParameter> lstparam, string strProc)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        SqlCommand sqlComm = new SqlCommand();
        DataSet ds = new DataSet();

        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProc;
            sqlComm.Parameters.Clear();
            SqlParameter[] dbParam = lstparam.ToArray();
            sqlComm.Parameters.AddRange(dbParam);
            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlComm);
            sqlDa.Fill(ds);
            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
            }
            sqlConn = null;
            sqlComm = null;
        }
    }

    public Int32 InsertUpdateDetails(List<SqlParameter> lstparam, string strProc)
    {
        string strMessage = "";
        Int32 intResult = 0;
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        SqlCommand sqlComm = new SqlCommand();

        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandText = strProc;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.Clear();
            SqlParameter[] dbParam = lstparam.ToArray();
            sqlComm.Parameters.AddRange(dbParam);
            sqlConn.Open();
            //strMessage = sqlComm.ExecuteScalar().ToString();
            intResult = Convert.ToInt32(sqlComm.ExecuteScalar());
            return intResult;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
            }
            sqlConn = null;
            sqlComm = null;
        }

    }

    public string InsertUpdateDeleteDetailsMsg(List<SqlParameter> lstparam, string strProc)
    {
        string strMessage = "";
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        SqlCommand sqlComm = new SqlCommand();

        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandText = strProc;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Parameters.Clear();
            SqlParameter[] dbParam = lstparam.ToArray();
            sqlComm.Parameters.AddRange(dbParam);
            sqlConn.Open();
            strMessage = sqlComm.ExecuteScalar().ToString();
            return strMessage;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
            }
            sqlConn = null;
            sqlComm = null;
        }
    }

    public DataTable FillGrid(Int32 intLoginId, string strSearch, string strProc, string strConnection)
    {
        SqlConnection sqlConn = new SqlConnection(strConnection);
        SqlCommand sqlComm = new SqlCommand();
        DataTable dt = new DataTable();

        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProc;
            sqlComm.Parameters.Clear();
            sqlComm.Parameters.Add("@LoginId", SqlDbType.Int, 4).Value = intLoginId;
            if (strSearch != "")
            {
                sqlComm.Parameters.Add("@Search", SqlDbType.VarChar, 2000).Value = strSearch;
            }

            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlComm);
            sqlDa.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlConn = null;
            sqlComm = null;
        }
    }

    public void FillCombo(Int32 intLoginId, DropDownList cboControl, string DataTextField, string DataValueField, string strProc, string strConnection)
    {
        SqlConnection sqlConn = new SqlConnection(strConnection);
        SqlCommand sqlComm = new SqlCommand();
        DataTable dt = new DataTable();

        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProc;
            sqlComm.Parameters.Clear();
            sqlComm.Parameters.Add("@LoginId", SqlDbType.Int, 4).Value = intLoginId;
            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlComm);
            sqlDa.Fill(dt);

            cboControl.DataSource = dt;
            cboControl.DataTextField = DataTextField;
            cboControl.DataValueField = DataValueField;
            cboControl.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlConn = null;
            sqlComm = null;
        }
    }

    private decimal GetDecimalValue(string strValues)
    {
        decimal decData = 0;
        bool blnNum;
        try
        {
            if (strValues.Trim() != "")
            {
                strValues = strValues.Replace("%", "").Replace(" ", "");
                blnNum = decimal.TryParse(strValues, out decData);
            }

            return decData;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string GetMACAddress()
    {
        string strMACAddress = "";
        return strMACAddress;
    }

    //public bool SendMail(string strMailTo, string strMailBcc, string strMailCc, string strMailSubject, string strMailMessage, string strMailAttachment)
    //{
    //    bool blnReturn = false;

    //    try
    //    {
    //        if (strMailTo != "")
    //        {
    //            MailMessage Mail = new MailMessage();
    //            Mail.From = new MailAddress("taufique.ahmed@genesissoftware.co.in");

    //            foreach (var address in strMailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
    //            {
    //                Mail.To.Add(address);
    //            }

    //            if (strMailBcc != "")
    //            {
    //                Mail.Bcc.Add(strMailBcc);
    //            }
    //            if (strMailCc != "")
    //            {
    //                Mail.CC.Add(strMailCc);
    //            }
    //            Mail.Subject = strMailSubject;
    //            Mail.Body = strMailMessage;
    //            Mail.IsBodyHtml = true;
    //            //Mail.Priority = MailPriority.High;
    //            Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
    //            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(strMailMessage, null, "text/html");
    //            Mail.AlternateViews.Add(htmlView);

    //            Attachment Attachment;
    //            if (strMailAttachment != "")
    //            {
    //                Attachment = new Attachment(strMailAttachment);
    //                Mail.Attachments.Add(Attachment);
    //            }

    //            SmtpClient SMTP = new SmtpClient();
    //            SMTP.Host = "mail.genesissoftware.co.in";
    //            NetworkCredential NetworkCredentials = new NetworkCredential("taufique.ahmed@genesissoftware.co.in", "genesis123");
    //            //NetworkCredential NetworkCredentials = new NetworkCredential("info@lifeforce.in", "Asdf!@#756");
    //            SMTP.Credentials = NetworkCredentials;
    //            SMTP.Send(Mail);
    //            blnReturn = true;
    //        }
    //        return blnReturn;
    //    }
    //    catch (Exception ex)
    //    {
    //        return blnReturn;
    //    }
    //}

    public string EncodeValue(string strValue, string strEncoder, Int16 intLeft)
    {
        char value = Convert.ToChar(strEncoder);
        if (strValue.Length >= 4)
        {
            strValue = new string(value, strValue.Length - intLeft) + strValue.Substring(strValue.Length - intLeft, (strValue.Length - (strValue.Length - intLeft)));
        }

        return strValue;
    }

    public DataSet GetDetails(Int32 intRecordId, string strProc, string strConn)
    {
        SqlConnection sqlConn = new SqlConnection(strConn);
        SqlCommand sqlComm = new SqlCommand();
        DataSet ds = new DataSet();

        try
        {
            sqlComm.Connection = sqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProc;
            sqlComm.Parameters.Clear();
            if (intRecordId > 0)
            {
                sqlComm.Parameters.Add("@RecordId", SqlDbType.Int, 4).Value = intRecordId;
            }
            SqlDataAdapter sqlda = new SqlDataAdapter(sqlComm);
            sqlda.Fill(ds);

            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
            }
            sqlConn.Dispose();
            sqlComm.Dispose();
        }
    }

}

