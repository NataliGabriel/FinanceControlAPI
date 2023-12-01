using FinanceControlAPI.Models;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using System.Net;

namespace FinanceControlAPI.Negócio
{
    public class Extensao
    {
        public async Task<string> SendEmail(Users user, bool Cadastro)
        {
            try
            {
                string senderEmail = "financecontrol@outlook.com.br";
                string senderPassword = "t@ti1311";

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(senderEmail, "Finance Control");
                mail.To.Add(user.Email); // para
                mail.Subject = "Finance Control"; // assunto
                mail.Priority = MailPriority.High;
                if (Cadastro == true)
                {
                    mail.Body =
                            @$"Olá {user.User}! Seja muito bem-vindo ao nosso site de gestão financeira pessoal, espero que goste do que oferecemos.
                        <br />
                        <br />                  
                        Essa é uma mensagem automática, não responda.";
                }
                else
                {
                    mail.Body = 
                            @$"Olá {user.User}!
                        <br />
                        <br />
                        Seu login é: 
                        <br />  
                        email: {user.Email} 
                        <br />
                        senha: {user.Password} 
                        <br />
                        <br />
                        Essa é uma mensagem automática, não responda.";
                }
                mail.IsBodyHtml = true;
                using (var smtp = new System.Net.Mail.SmtpClient("smtp.office365.com"))
                {
                    smtp.EnableSsl = true;
                    smtp.Port = 587;       
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network; 
                    smtp.UseDefaultCredentials = false;

                    // seu usuário e senha para autenticação
                    smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    // envia o e-mail
                    smtp.Send(mail);
                }


                return "";
            }
            catch (Exception ex) { return ex.Message; }
        }
    }
}
