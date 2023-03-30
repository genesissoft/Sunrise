using System;
using System.Data;
using System.Xml;
using System.IO;
using System.Configuration;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

public class ClsCommonCS
{
    SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());

    public DataTable FillDataTableId(string strProc, int _IntId, String _search)
    {
        SqlCommand sqlComm = new SqlCommand();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        DataTable dt = new DataTable();
        SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        try
        {
            string _Strout = null;
            sqlComm.CommandText = strProc;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Connection = SqlConn;
            if (Convert.ToString(HttpContext.Current.Session["UserType"]).ToLower() != "administrator")
            {
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            }
            else
            {
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            }
            if (_search == "")
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 4000).Value = DBNull.Value;
            }
            else
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 4000).Value = _search;
            }
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4);
            sqlComm.Parameters["@RET_CODE"].Direction = ParameterDirection.Output;
            //sqlComm.ExecuteNonQuery();
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);
            _Strout = sqlComm.Parameters["@RET_CODE"].Value.ToString();
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlComm.Dispose();
            sqlDa = null;
        }
    }


    public DataTable FillEquityResearchDetails(string strProc, int _IntId, String _search, String FromDt, String ToDt)
    {
        SqlCommand sqlComm = new SqlCommand();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        DataTable dt = new DataTable();
        SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        string _Strout = null;
        try
        {
            sqlComm.CommandText = strProc;
            sqlComm.CommandType = CommandType.StoredProcedure;
            //OpenConn();          
            sqlComm.Connection = SqlConn;
            //if (Convert.ToString(HttpContext.Current.Session["UserName"]).ToLower() != "admin")
            if (Convert.ToString(HttpContext.Current.Session["UserType"]).ToLower() != "administrator")
            {
                sqlComm.Parameters.Add("@UserType", SqlDbType.Char, 1).Value = "U";
            }
            else
            {
                sqlComm.Parameters.Add("@UserType", SqlDbType.Char, 1).Value = "A";
            }

            sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 8).Value = Convert.ToInt64(HttpContext.Current.Session["UserId"]);
            sqlComm.Parameters.Add("@FromDt", SqlDbType.VarChar, 10).Value = DateFormatMMDDYY(FromDt);
            sqlComm.Parameters.Add("@ToDt", SqlDbType.VarChar, 10).Value = DateFormatMMDDYY(ToDt);

            if (_search == "")
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = DBNull.Value;
            }
            else
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = _search;
            }

            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4);
            sqlComm.Parameters["@RET_CODE"].Direction = ParameterDirection.Output;
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);
            _Strout = Convert.ToString(sqlComm.Parameters["@RET_CODE"].Value);
            return dt;

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlComm.Dispose();
            sqlDa = null;
        }
    }


    public DataTable FillDataTableId_WeeklyUpdate(string strProc, int _IntId, String _search, string strDate)
    {

        SqlCommand sqlComm = new SqlCommand();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        DataTable dt = new DataTable();
        SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        try
        {
            string _Strout = null;
            sqlComm.CommandText = strProc;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Connection = SqlConn;
            if (Convert.ToString(HttpContext.Current.Session["UserType"]).ToLower() != "administrator")
            {
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            }
            else
            {
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            }
            if (_search == "")
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 4000).Value = DBNull.Value;
            }
            else
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 4000).Value = _search;
            }
            sqlComm.Parameters.Add("@WeekDate", SqlDbType.VarChar, 20).Value = DateFormatMMDDYY(strDate);
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4);
            sqlComm.Parameters["@RET_CODE"].Direction = ParameterDirection.Output;
            //sqlComm.ExecuteNonQuery();
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);
            _Strout = sqlComm.Parameters["@RET_CODE"].Value.ToString();
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            SqlConn = null;
            sqlComm.Dispose();
            sqlDa = null;
        }
    }


    public DataTable FillDataTable(string strProc, int _IntId, String _search)
    {
        SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        SqlCommand sqlComm = new SqlCommand();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        DataTable dt = new DataTable();
        try
        {

            string _Strout = null;
            sqlComm.CommandText = strProc;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Connection = SqlConn;
            if (Convert.ToString(HttpContext.Current.Session["UserType"]) != "")
            {
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            }
            if (_search == "")
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = DBNull.Value;
            }
            else
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = _search;
            }
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4);
            sqlComm.Parameters["@RET_CODE"].Direction = ParameterDirection.Output;
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);
            _Strout = sqlComm.Parameters["@RET_CODE"].Value.ToString();
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlComm.Dispose();
            sqlDa = null;
        }
    }

    public DataTable FillDataTable_CustomerRating(string strProc, int _IntId, String _search, String strClientTypeId)
    {
        SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        SqlCommand sqlComm = new SqlCommand();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        DataTable dt = new DataTable();
        try
        {

            string _Strout = null;
            sqlComm.CommandText = strProc;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Connection = SqlConn;
            if (Convert.ToString(HttpContext.Current.Session["UserType"]) != "")
            {
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            }
            if (_search == "")
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = DBNull.Value;
            }
            else
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = _search;
            }
            if (strClientTypeId == "")
            {
                sqlComm.Parameters.Add("@StrClientTypeId", SqlDbType.VarChar, 200).Value = DBNull.Value;
            }
            else
            {
                sqlComm.Parameters.Add("@StrClientTypeId", SqlDbType.VarChar, 200).Value = strClientTypeId;
            }
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4);
            sqlComm.Parameters["@RET_CODE"].Direction = ParameterDirection.Output;
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);
            _Strout = sqlComm.Parameters["@RET_CODE"].Value.ToString();
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlComm.Dispose();
            sqlDa = null;
        }
    }

    public DataTable FillDataTable_Equity(string strProc, int _IntId, String _search, String strClientTypeId)
    {
        SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        SqlCommand sqlComm = new SqlCommand();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        DataTable dt = new DataTable();
        try
        {

            string _Strout = null;
            sqlComm.CommandText = strProc;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Connection = SqlConn;
            if (Convert.ToString(HttpContext.Current.Session["UserType"]) != "")
            {
                sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            }
            if (_search == "")
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = DBNull.Value;
            }
            else
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = _search;
            }
            if (strClientTypeId == "")
            {
                sqlComm.Parameters.Add("@StrClientTypeId", SqlDbType.VarChar, 200).Value = DBNull.Value;
            }
            else
            {
                sqlComm.Parameters.Add("@StrClientTypeId", SqlDbType.VarChar, 200).Value = strClientTypeId;
            }
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4);
            sqlComm.Parameters["@RET_CODE"].Direction = ParameterDirection.Output;
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);
            _Strout = sqlComm.Parameters["@RET_CODE"].Value.ToString();
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlComm.Dispose();
            sqlDa = null;
        }
    }

    private void OpenConn()
    {
        if (SqlConn == null)
        {

            SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
            SqlConn.Open();
        }
        else if (SqlConn.State == ConnectionState.Closed)
        {
            SqlConn.ConnectionString = ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString();
            SqlConn.Open();
        }
    }

    private void CloseConn()
    {
        if (SqlConn == null)
        { return; }
        if (SqlConn.State == ConnectionState.Open)
        { SqlConn.Close(); }
    }

    public ReportDocument GetReport(ReportDocument crDc)
    {
        TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
        TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
        ConnectionInfo crConnectionInfo = new ConnectionInfo();
        Tables CrTables = default(Tables);
        crConnectionInfo.ServerName = ConfigurationManager.AppSettings["DBServerName"].ToString();
        crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["DatabaseName"].ToString();
        crConnectionInfo.UserID = ConfigurationManager.AppSettings["DBUserID"].ToString();
        crConnectionInfo.Password = ConfigurationManager.AppSettings["DBPassword"].ToString();
        CrTables = crDc.Database.Tables;
        foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
        {
            crtableLogoninfo = CrTable.LogOnInfo;
            crtableLogoninfo.ConnectionInfo = crConnectionInfo;
            CrTable.ApplyLogOnInfo(crtableLogoninfo);
        }
        return crDc;
    }

    public string DateFormatMMDDYY(string strDate)
    {
        string NewDate;
        if (strDate == "")
        {
            NewDate = "";
        }
        else
        {
            NewDate = strDate.Substring(3, 02) + "/" + strDate.Substring(0, 2) + "/" + strDate.Substring(6, 4);
        }
        return NewDate;
    }

    public static string Encrypt(string TextToBeEncrypted)
    {
        RijndaelManaged RijndaelCipher = new RijndaelManaged();
        string Password = "CSC";
        byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(TextToBeEncrypted);
        byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
        PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
        //Creates a symmetric encryptor object.
        ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
        MemoryStream memoryStream = new MemoryStream();
        //Defines a stream that links data streams to cryptographic transformations
        CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(PlainText, 0, PlainText.Length);
        //Writes the final state and clears the buffer
        cryptoStream.FlushFinalBlock();
        byte[] CipherBytes = memoryStream.ToArray();
        memoryStream.Close();
        cryptoStream.Close();
        string EncryptedData = Convert.ToBase64String(CipherBytes);
        //EncryptedData = EncryptedData.Replace(' ', '+');
        return EncryptedData;
    }

    public static string Decrypt(string TextToBeDecrypted)
    {
        TextToBeDecrypted = TextToBeDecrypted.Replace(" ", "+");
        RijndaelManaged RijndaelCipher = new RijndaelManaged();
        string Password = "CSC";

        byte[] EncryptedData = Convert.FromBase64String(TextToBeDecrypted);
        byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
        //Making of the key for decryption
        PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
        //Creates a symmetric Rijndael decryptor object.
        ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
        MemoryStream memoryStream = new MemoryStream(EncryptedData);
        //Defines the cryptographics stream for decryption.THe stream contains decrpted data
        CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
        byte[] PlainText = new byte[EncryptedData.Length];
        int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
        memoryStream.Close();
        cryptoStream.Close();
        //Converting to string
        string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
        return DecryptedData;
    }

    public DataTable FillIssuerDataEntryDetails(string strProc, int _IntId, String _search)
    {
        SqlCommand sqlComm = new SqlCommand();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        DataTable dt = new DataTable();
        SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        string _Strout = null;
        try
        {
            sqlComm.CommandText = strProc;
            sqlComm.CommandType = CommandType.StoredProcedure;
            //OpenConn();          
            sqlComm.Connection = SqlConn;
            //if (Convert.ToString(HttpContext.Current.Session["UserName"]).ToLower() != "admin")
            if (Convert.ToString(HttpContext.Current.Session["UserType"]).ToLower() != "administrator")
            {
                sqlComm.Parameters.Add("@UserType", SqlDbType.Char, 1).Value = "U";
            }
            else
            {
                sqlComm.Parameters.Add("@UserType", SqlDbType.Char, 1).Value = "A";
            }
            sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 8).Value = Convert.ToInt64(HttpContext.Current.Session["UserId"]);
            if (_search == "")
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 200).Value = DBNull.Value;
            }
            else
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 200).Value = _search;
            }
            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4);
            sqlComm.Parameters["@RET_CODE"].Direction = ParameterDirection.Output;
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);
            _Strout = Convert.ToString(sqlComm.Parameters["@RET_CODE"].Value);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlComm.Dispose();
            sqlDa = null;
        }
    }

    public DataTable FillInteractionDetails(string strProc, int _IntId, String _search, String FromDt, String ToDt)
    {
        SqlCommand sqlComm = new SqlCommand();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        DataTable dt = new DataTable();
        SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        string _Strout = null;
        try
        {
            sqlComm.CommandText = strProc;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.Connection = SqlConn;
            sqlComm.Parameters.Add("@FromDate", SqlDbType.VarChar, 10).Value = FromDt;
            sqlComm.Parameters.Add("@ToDate", SqlDbType.VarChar, 10).Value = ToDt;
            sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 8).Value = Convert.ToInt64(HttpContext.Current.Session["UserId"]);
            if (_search == "")
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = DBNull.Value;
            }
            else 
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 100).Value = _search;
            }
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlComm.Dispose();
            sqlDa = null;
        }
    }

    public DataTable FillBidOfferDetails(string strProc, int _IntId, String _search, String FromDt, String ToDt)
    {
        SqlCommand sqlComm = new SqlCommand();
        SqlDataAdapter sqlDa = new SqlDataAdapter();
        DataTable dt = new DataTable();
        SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["InstadealConnectionString"].ToString());
        string _Strout = null;
        try
        {
            sqlComm.CommandText = strProc;
            sqlComm.CommandType = CommandType.StoredProcedure;
            //OpenConn();          
            sqlComm.Connection = SqlConn;
            //if (Convert.ToString(HttpContext.Current.Session["UserName"]).ToLower() != "admin")
            //if (Convert.ToString(HttpContext.Current.Session["UserType"]).ToLower() != "administrator")
            //{
            //    sqlComm.Parameters.Add("@UserType", SqlDbType.Char, 1).Value = "U";
            //}
            //else
            //{
            //    sqlComm.Parameters.Add("@UserType", SqlDbType.Char, 1).Value = "A";
            //}

            //sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 8).Value = Convert.ToInt64(HttpContext.Current.Session["UserId"]);
            sqlComm.Parameters.Add("@FromDt", SqlDbType.VarChar, 10).Value = DateFormatMMDDYY(FromDt);
            sqlComm.Parameters.Add("@ToDt", SqlDbType.VarChar, 10).Value = DateFormatMMDDYY(ToDt);

            if (_search == "")
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 500).Value = DBNull.Value;
            }
            else
            {
                sqlComm.Parameters.Add("@StrSearch", SqlDbType.VarChar, 500).Value = _search;
            }

            sqlComm.Parameters.Add("@RET_CODE", SqlDbType.Int, 4);
            sqlComm.Parameters["@RET_CODE"].Direction = ParameterDirection.Output;
            sqlDa.SelectCommand = sqlComm;
            sqlDa.Fill(dt);
            _Strout = Convert.ToString(sqlComm.Parameters["@RET_CODE"].Value);
            return dt;

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sqlComm.Dispose();
            sqlDa = null;
        }
    }

    public static string Right(string value, int length)
    {
        return value.Substring(value.Length - length);
    }

    public void SetCommandParameters(SqlCommand sqlComm, string p, SqlDbType sqlDbType, int p_4, char p_5, int p_6, int p_7, long QuoteEntryId)
    {
        throw new NotImplementedException();
    }

    public DataSet FillDetails(Int32 intRecordId, Int32 intUserId, string strProc, string strSearch)
    {
        SqlCommand sqlComm = new SqlCommand();
        DataSet ds = new DataSet();

        try
        {
            sqlComm.Connection = SqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProc;
            sqlComm.Parameters.Clear();
            if (intRecordId > 0)
            {
                sqlComm.Parameters.Add("@RecordId", SqlDbType.BigInt, 4).Value = intRecordId;
            }
            if (intUserId > 0)
            {
                sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 4).Value = intUserId;
            }
            if (strSearch != "")
            {
                sqlComm.Parameters.Add("@Search", SqlDbType.VarChar, 500).Value = strSearch;
            }
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
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
            }
            sqlComm = null;
        }
        return ds;
    }

    public void FillCombo(Int32 intUserId, DropDownList cboControl, string DataTextField, string DataValueField, string strProc)
    {
        SqlCommand sqlComm = new SqlCommand();
        DataTable dt = new DataTable();

        try
        {
            sqlComm.Connection = SqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProc;
            sqlComm.Parameters.Clear();
            sqlComm.Parameters.Add("@UserId", SqlDbType.Int, 4).Value = intUserId;
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
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
            }
            sqlComm = null;
        }
    }

    public DataSet FillAllPageCombo(Int32 intUserId, string strProc)
    {
        SqlCommand sqlComm = new SqlCommand();
        DataSet ds = new DataSet();
        try
        {
            sqlComm.Connection = SqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProc;
            sqlComm.Parameters.Clear();

            if (intUserId > 0)
            {
                sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 4).Value = intUserId;
            }
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
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
            }
            sqlComm = null;
        }
    }

    public DataTable FillFields(Int32 intRecordId, Int32 intUserId, string strProc, string strConn)
    {
        SqlCommand sqlComm = new SqlCommand();
        DataTable dt = new DataTable();
        try
        {
            sqlComm.Connection = SqlConn;
            sqlComm.CommandType = CommandType.StoredProcedure;
            sqlComm.CommandText = strProc;
            sqlComm.Parameters.Clear();
            if (intRecordId > 0)
            {
                sqlComm.Parameters.Add("@RecordId", SqlDbType.BigInt, 4).Value = intRecordId;
            }
            if (intUserId > 0)
            {
                sqlComm.Parameters.Add("@UserId", SqlDbType.BigInt, 4).Value = intUserId;
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
            if (SqlConn.State == ConnectionState.Open)
            {
                SqlConn.Close();
            }
            sqlComm = null;
        }
    }
}

public static class StringMyDateTimeFormatExtension
{
    //public static DateTime ParseMyFormatDateTime(this string s)
    //{
    //    var culture = System.Globalization.CultureInfo.CurrentCulture;
    //    return DateTime.ParseExact(s, "MM/dd/yyyy", null);
    //}
}
//namespace System.Runtime.CompilerServices
//{
//    public class ExtensionAttribute : Attribute { }
//}
