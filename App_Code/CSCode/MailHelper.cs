using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Net;
using System.Web.Configuration;
using System.Net.Configuration;


/// <summary>
/// Summary description for MailHelper
/// </summary>
public class MailHelper
{
    public MailHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //MailHelper.SendMailMessage("fromAddress@yourdomain.com", "toAddress@yourdomain.com", 
    //"bccAddress@yourdomain.com", "ccAddress@yourdomain.com", "Sample Subject", 
    //"Sample body of text for mail message")


    /// <summary>
    /// Sends an mail message
    /// </summary>
    /// <param name="from">Sender address</param>
    /// <param name="to">Recepient address</param>
    /// <param name="bcc">Bcc recepient</param>
    /// <param name="cc">Cc recepient</param>
    /// <param name="subject">Subject of mail message</param>
    /// <param name="body">Body of mail message</param>
    public static void SendMailMessage(string from, string to, string bcc, string cc, string subject, string body, string attachmentFilename, string attachmentFilename1)
    {

        try
        {

            // Instantiate a new instance of MailMessage
            MailMessage mMailMessage = new MailMessage();
            System.Net.Mail.Attachment attachment = default(System.Net.Mail.Attachment);
            System.Net.Mail.Attachment attachment1 = default(System.Net.Mail.Attachment);
            if (attachmentFilename1 != "")
            {
                 attachment1 = default(System.Net.Mail.Attachment);
            }

            if (attachmentFilename != "")
            {
                attachment = new System.Net.Mail.Attachment(attachmentFilename);
            }

         
           if (attachmentFilename1 != "")
           {
               
               attachment1 = new System.Net.Mail.Attachment(attachmentFilename1);

           }
          
           
            // Set the sender address of the mail message
            //mMailMessage.From = new MailAddress(from);
            // Set the recepient address of the mail message
            //mMailMessage.To.Add(new MailAddress(to));

            string[] ToMuliId = to.Split(',');
            foreach (string ToEMailId in ToMuliId)
            {
                mMailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
            }
            if   (attachmentFilename != "")
            {
                 mMailMessage.Attachments.Add(attachment);
            }

           
            if (attachmentFilename1 != "")
            {

                mMailMessage.Attachments.Add(attachment1);

            }
            
              //mMailMessage.Attachments.Add(attachment)  ;
            // Check if the bcc value is null or an empty string
            if ((bcc != null) && (bcc != string.Empty))
            {
                // Set the Bcc address of the mail message
                mMailMessage.Bcc.Add(new MailAddress(bcc));
            }      // Check if the cc value is null or an empty value
            if ((cc != null) && (cc != string.Empty))
            {
                // Set the CC address of the mail message
                mMailMessage.CC.Add(new MailAddress(cc));
            }       // Set the subject of the mail message
            mMailMessage.Subject = subject;
         
            // Set the body of the mail message
            mMailMessage.Body = body;

            // Set the format of the mail message body as HTML
            mMailMessage.IsBodyHtml = true;
            // Set the priority of the mail message to normal
            mMailMessage.Priority = MailPriority.Normal;

            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(
            HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup mailSettings =
                 (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

            if (mailSettings != null)
            {                
                // Instantiate a new instance of SmtpClient
                SmtpClient mSmtpClient = new SmtpClient(mailSettings.Smtp.Network.Host, mailSettings.Smtp.Network.Port);
               // mSmtpClient.Credentials = new NetworkCredential(from, mailSettings.Smtp.Network.Password);
                mSmtpClient.Credentials = new NetworkCredential(mailSettings.Smtp.From, mailSettings.Smtp.Network.Password);

                mMailMessage.From = new MailAddress(mailSettings.Smtp.From);

                if (mailSettings.Smtp.Network.Host =="smtp.gmail.com")
                {
                    mSmtpClient.EnableSsl = true;
                }
                else
                {
                    mSmtpClient.EnableSsl = false;
                }

                // Send the mail message
                mSmtpClient.Send(mMailMessage);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

  
    
}
