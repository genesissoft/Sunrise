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
using System.Net.Mail;
using System.Net;
using System.Web.Configuration;
using System.Net.Configuration;



public partial class Forms_TestEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        try
        {

            lblmsg.Text = "";
            MailMessage mMailMessage = new MailMessage();
            System.Net.Mail.Attachment attachment = default(System.Net.Mail.Attachment);
            System.Net.Mail.Attachment attachment1 = default(System.Net.Mail.Attachment);

            mMailMessage.From = new MailAddress(txtUserName.Text);
            
            string[] ToMuliId = txtto.Text.Split(',');
            foreach (string ToEMailId in ToMuliId)
            {
                mMailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
            }



            if ((txtBCC.Text != null) && (txtBCC.Text != string.Empty))
            {
                mMailMessage.Bcc.Add(new MailAddress(txtBCC.Text));
            }
            if ((txtCC.Text != null) && (txtCC.Text != string.Empty))
            {
                mMailMessage.CC.Add(new MailAddress(txtCC.Text));
            }   
            mMailMessage.Subject = "TEST Mail" ;

           
            mMailMessage.Body = "TEST Mail";

            
            mMailMessage.IsBodyHtml = true;
            mMailMessage.Priority = MailPriority.Normal;

           
           SmtpClient mSmtpClient = new SmtpClient(txtSmtpServer.Text, Convert.ToInt32(txtPort.Text));
                
           mSmtpClient.Credentials = new NetworkCredential(txtUserName.Text,txtPassword.Text);
               
           if (ddlSSL.SelectedValue == "True")
                {
                    mSmtpClient.EnableSsl = true;
                }
           else
                {
                    mSmtpClient.EnableSsl = false;
                }
              
           mSmtpClient.Send(mMailMessage);
           lblmsg.Text = "Send Successfully...";
        }
        catch (Exception ex)
        {
            throw ex;
            lblmsg.Text = ex.Message;
        }

    }
    
}
