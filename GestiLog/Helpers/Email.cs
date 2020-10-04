using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace GestiLog.Helpers
{
    public class Email
    {
        public string MaskAddress { get; set; }
        public string MaskName { get; set; }
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public List<Attachment> Attachments { get; set; }

        public Email()
        {
            this.MaskAddress = WebConfigurationManager.AppSettings["Mail.MascaraCorreo"].ToString();
            this.MaskName = WebConfigurationManager.AppSettings["Mail.MascaraNombre"].ToString();
            this.Smtp = WebConfigurationManager.AppSettings["Mail.Smtp"].ToString();
            this.Port = int.Parse(WebConfigurationManager.AppSettings["Mail.Puerto"].ToString());
            this.Account = WebConfigurationManager.AppSettings["Mail.Cuenta"].ToString();
            this.Password = WebConfigurationManager.AppSettings["Mail.Clave"].ToString();
            this.Attachments = new List<Attachment>();
        }

        public void Send(List<string> destination, string subject, string body)
        {
            try
            {
                MailMessage m = new MailMessage();
                m.From = new MailAddress(this.MaskAddress, this.MaskName);
                destination.ForEach(i =>
                {
                    m.To.Add(new MailAddress(i));
                });
                m.Subject = subject;
                m.Body = body;
                m.IsBodyHtml = true;
                if (Attachments != null)
                {
                    if (Attachments.Count > 0)
                    {
                        Attachments.ForEach(i =>
                        {
                            m.Attachments.Add(i);
                        });
                    }
                }

                SmtpClient smtp = new SmtpClient(this.Smtp, this.Port);
                smtp.Credentials = new NetworkCredential(this.Account, this.Password);
                smtp.EnableSsl = true;
                smtp.Send(m);
            }
            catch (Exception ex) { }
        }
    }
}