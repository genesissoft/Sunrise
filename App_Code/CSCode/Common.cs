using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Threading;

public class Common
{
    public int strSendStatus = 0;
    string EmailtemplatePath = Convert.ToString(ConfigurationManager.AppSettings["EmailtemplatePath"].ToString());
    string EmailTo = Convert.ToString(ConfigurationManager.AppSettings["EmailTo"].ToString());
    string IsImgRqd = "";
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
            sqlComm.CommandTimeout = 300;
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
            //sqlComm.Parameters.Add("@LoginId", SqlDbType.Int, 4).Value = intLoginId;
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

    public void SendInterestReceivableMail()
    {
        DataSet ds = new DataSet();
        DataTable dtInterest = new DataTable();
        DataTable dtFinInterest = new DataTable();
        DataTable dtUser = new DataTable();
        DataTable dtStatus = new DataTable();
        List<SqlParameter> lstParam = new List<SqlParameter>();
        string strUserEmail = "", strCCEmail = "", strTitle = "", strCompanyName = "", strCustomerName = "", strHTML = "", strFinHTML = "";
        double dblTotalInterest = 0, dblTotalStock = 0, dblTotalQuantity = 0;
        double dblGrandTotalInterest = 0, dblGrandTotalStock = 0, dblGrandTotalQuantity = 0;
        int intStatus = -1;

        try
        {
            //lstParam.Add(new SqlParameter("@UserId", (Val(HttpContext.Current.Session["UserId"].ToString()) > 0) ? HttpContext.Current.Session["UserId"].ToString() : null));
            ds = FillDetails(lstParam, "Fill_InterestReceivable_MailDetails");

            dtInterest = ds.Tables[0];
            dtFinInterest = ds.Tables[1];
            dtUser = ds.Tables[2];
            dtStatus = ds.Tables[3];

            if ((dtInterest.Rows.Count > 0 || dtFinInterest.Rows.Count > 0) && dtUser.Rows.Count > 0 && dtStatus.Rows.Count == 0)
            {
                intStatus = 0;
                strTitle = Trim(dtUser.Rows[0]["Title"].ToString());
                strUserEmail = Trim(dtUser.Rows[0]["EmailId"].ToString());
                strCCEmail = Trim(dtUser.Rows[0]["CCEmailId"].ToString());

                if (strUserEmail != "")
                {
                    if (dtInterest.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtInterest.Rows)
                        {
                            if (strCompanyName != "" & strCompanyName != Trim(row["CompanyName"].ToString()))
                            {
                                strHTML += "<tr class='align_left bold' style='font-weight: bold;'>";
                                strHTML += "<td colspan='3'>Sub Total:</td>";
                                strHTML += "<td class='align_right'>" + Math.Round(dblTotalInterest, 2).ToString("0.00") + "</td>";
                                strHTML += "<td class='align_right'>" + Math.Round(dblTotalStock, 2).ToString("0.00") + "</td>";
                                strHTML += "<td class='align_right'>" + Math.Round(dblTotalQuantity, 2).ToString() + "</td>";
                                strHTML += "</tr>";

                                dblTotalInterest = 0;
                                dblTotalStock = 0;
                                dblTotalQuantity = 0;
                            }

                            strHTML += "<tr class='align_left'>";
                            strHTML += "<td>" + Trim(row["CompanyName"].ToString()) + "</td>";
                            strHTML += "<td>" + Trim(row["SecurityName"].ToString()) + "</td>";
                            strHTML += "<td class='align_center'>" + Trim(row["NSDLAcNumber"].ToString()) + "</td>";
                            strHTML += "<td class='align_right'>" + Trim(row["InterestReceivable"].ToString()) + "</td>";
                            strHTML += "<td class='align_right'>" + Trim(row["StockValue"].ToString()) + "</td>";
                            strHTML += "<td class='align_right'>" + Trim(row["Quantity"].ToString()) + "</td>";
                            strHTML += "</tr>";

                            strCompanyName = Trim(row["CompanyName"].ToString());
                            dblTotalInterest += Convert.ToDouble(row["InterestReceivable"]);
                            dblTotalStock += Convert.ToDouble(row["StockValue"]);
                            dblTotalQuantity += Convert.ToDouble(row["Quantity"]);
                            dblGrandTotalInterest += Convert.ToDouble(row["InterestReceivable"]);
                            dblGrandTotalStock += Convert.ToDouble(row["StockValue"]);
                            dblGrandTotalQuantity += Convert.ToDouble(row["Quantity"]);
                        }

                        strHTML += "<tr class='align_left bold' style='font-weight: bold;'>";
                        strHTML += "<td colspan='3'>Sub Total:</td>";
                        strHTML += "<td class='align_right'>" + Math.Round(dblTotalInterest, 2).ToString("0.00") + "</td>";
                        strHTML += "<td class='align_right'>" + Math.Round(dblTotalStock, 2).ToString("0.00") + "</td>";
                        strHTML += "<td class='align_right'>" + Math.Round(dblTotalQuantity, 2).ToString() + "</td>";
                        strHTML += "</tr>";

                        strHTML += "<tr class='align_left bold' style='background-color: #EEEEEE; font-weight: bold;'>";
                        strHTML += "<td colspan='3'>Grand Total:</td>";
                        strHTML += "<td class='align_right'>" + Math.Round(dblGrandTotalInterest, 2).ToString("0.00") + "</td>";
                        strHTML += "<td class='align_right'>" + Math.Round(dblGrandTotalStock, 2).ToString("0.00") + "</td>";
                        strHTML += "<td class='align_right'>" + Math.Round(dblGrandTotalQuantity, 2).ToString() + "</td>";
                        strHTML += "</tr>";
                    }

                    if (dtFinInterest.Rows.Count > 0)
                    {
                        dblTotalInterest = 0;
                        dblTotalStock = 0;
                        dblTotalQuantity = 0;
                        dblGrandTotalInterest = 0;
                        dblGrandTotalStock = 0;
                        dblGrandTotalQuantity = 0;

                        foreach (DataRow row in dtFinInterest.Rows)
                        {
                            if (strCustomerName != "" & strCustomerName != Trim(row["CustomerName"].ToString()))
                            {
                                strFinHTML += "<tr class='align_left bold' style='font-weight: bold;'>";
                                strFinHTML += "<td colspan='3'>Sub Total:</td>";
                                strFinHTML += "<td class='align_right'>" + Math.Round(dblTotalInterest, 2).ToString("0.00") + "</td>";
                                strFinHTML += "<td class='align_right'>" + Math.Round(dblTotalStock, 2).ToString("0.00") + "</td>";
                                strFinHTML += "<td class='align_right'>" + Math.Round(dblTotalQuantity, 2).ToString() + "</td>";
                                strFinHTML += "</tr>";

                                dblTotalInterest = 0;
                                dblTotalStock = 0;
                                dblTotalQuantity = 0;
                            }

                            strFinHTML += "<tr class='align_left'>";
                            strFinHTML += "<td>" + Trim(row["CustomerName"].ToString()) + "</td>";
                            strFinHTML += "<td>" + Trim(row["SecurityName"].ToString()) + "</td>";
                            strFinHTML += "<td class='align_center'>" + Trim(row["NSDLAcNumber"].ToString()) + "</td>";
                            strFinHTML += "<td class='align_right'>" + Trim(row["InterestReceivable"].ToString()) + "</td>";
                            strFinHTML += "<td class='align_right'>" + Trim(row["StockValue"].ToString()) + "</td>";
                            strFinHTML += "<td class='align_right'>" + Trim(row["Quantity"].ToString()) + "</td>";
                            strFinHTML += "</tr>";

                            strCustomerName = Trim(row["CustomerName"].ToString());
                            dblTotalInterest += Convert.ToDouble(row["InterestReceivable"]);
                            dblTotalStock += Convert.ToDouble(row["StockValue"]);
                            dblTotalQuantity += Convert.ToDouble(row["Quantity"]);
                            dblGrandTotalInterest += Convert.ToDouble(row["InterestReceivable"]);
                            dblGrandTotalStock += Convert.ToDouble(row["StockValue"]);
                            dblGrandTotalQuantity += Convert.ToDouble(row["Quantity"]);
                        }

                        strFinHTML += "<tr class='align_left bold' style='font-weight: bold;'>";
                        strFinHTML += "<td colspan='3'>Sub Total:</td>";
                        strFinHTML += "<td class='align_right'>" + Math.Round(dblTotalInterest, 2).ToString("0.00") + "</td>";
                        strFinHTML += "<td class='align_right'>" + Math.Round(dblTotalStock, 2).ToString("0.00") + "</td>";
                        strFinHTML += "<td class='align_right'>" + Math.Round(dblTotalQuantity, 2).ToString() + "</td>";
                        strFinHTML += "</tr>";

                        strFinHTML += "<tr class='align_left bold' style='background-color: #EEEEEE; font-weight: bold;'>";
                        strFinHTML += "<td colspan='3'>Grand Total:</td>";
                        strFinHTML += "<td class='align_right'>" + Math.Round(dblGrandTotalInterest, 2).ToString("0.00") + "</td>";
                        strFinHTML += "<td class='align_right'>" + Math.Round(dblGrandTotalStock, 2).ToString("0.00") + "</td>";
                        strFinHTML += "<td class='align_right'>" + Math.Round(dblGrandTotalQuantity, 2).ToString() + "</td>";
                        strFinHTML += "</tr>";
                    }

                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/interestreceivable.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{InterestDetails}", strHTML);
                    body = body.Replace("{FinInterestDetails}", strFinHTML);

                    if (SendMail(strUserEmail, "", strCCEmail, strTitle, body, ""))
                    {
                        intStatus = 1;
                    }
                }
            }
            lstParam.Clear();
            lstParam.Add(new SqlParameter("@Status", intStatus));
            //lstParam.Add(new SqlParameter("@UserId", (Val(HttpContext.Current.Session["UserId"].ToString()) > 0) ? HttpContext.Current.Session["UserId"].ToString() : null));
            InsertUpdateDetails(lstParam, "InsertUpdate_DailyTaskStatus");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool SendMail(string strMailTo, string strMailBcc, string strMailCc, string strMailSubject, string strMailMessage, string strMailAttachment)
    {
        bool blnReturn = false;
        Util objUtil = new Util();
        try
        {
            if (strMailTo != "")
            {
                MailMessage Mail = new MailMessage();

                foreach (string address in strMailTo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Mail.To.Add(address);
                }

                foreach (string address in strMailBcc.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Mail.Bcc.Add(address);
                }

                foreach (string address in strMailCc.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Mail.CC.Add(address);
                }

                Mail.Subject = strMailSubject;
                Mail.Body = strMailMessage;
                Mail.IsBodyHtml = true;

                //Mail.Priority = MailPriority.High;
                Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(strMailMessage, null, "text/html");
                Mail.AlternateViews.Add(htmlView);

                Attachment Attachment;
                if (strMailAttachment != "")
                {
                    Attachment = new Attachment(strMailAttachment);
                    Mail.Attachments.Add(Attachment);
                }
                SmtpClient SMTP = new SmtpClient();
                SMTP.EnableSsl = true;
                SMTP.Send(Mail);
                blnReturn = true;
            }
            return blnReturn;
        }
        catch (Exception ex)
        {
            objUtil.WritErrorLog("Common.cs", "SendMail", "Error in SendMail", "", ex);
            return blnReturn;
        }
    }
    public void SendOffer(string strCustomerName, string strFileName, string strCustEmailId, string strYTMDate, string strSenderName, string StrSenderContactNumber)
    {
        try
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(EmailtemplatePath + "//SendOffer.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{CustomerName}", strCustomerName);
            body = body.Replace("{SenderName}", strSenderName);
            body = body.Replace("{SenderContactNumber}", StrSenderContactNumber);
            if (strCustEmailId != "")
            {
                IsImgRqd = "No";
                ThreadStart threadStart = delegate () { SendMail(strCustEmailId, "", "", "Quotes " + DateTime.Now.ToLongDateString(), body, strFileName); };
                Thread thread = new Thread(threadStart);
                thread.Start();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
