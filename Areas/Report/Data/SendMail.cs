using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
namespace Standus_5_0.Areas.Report.Data
{
     public class SendMail
    {
        public static bool Send(MailMessage mail)
        {
            try
            {

                // Dim path As String = System.Web.HttpContext.Current.Request.PhysicalPath("Activation.aspx")

                ServicePointManager.ServerCertificateValidationCallback = (s, c, h, e) => true;

                var client = new SmtpClient("mail.standus.in", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("shailesh@standus.in", "Sta@qwer777"),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                mail.IsBodyHtml = true;

    //            System.Net.ServicePointManager.ServerCertificateValidationCallback =
    //(sender, certificate, chain, sslPolicyErrors) => true;
                client.Send(mail);
                // MsgBox("Mail sent")
                return true;
            }
            catch (Exception ex)
            {
                // MsgBox(ex.ToString)
                return false;
            }
        }
    }

}
