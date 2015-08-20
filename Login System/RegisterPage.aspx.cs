using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Login_System
{
    public partial class RegisterPage : System.Web.UI.Page
    {
        protected static string HashSHA1(string value)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        protected static bool isPresent(string email) {
            string file = "C:\\Users\\Kiran\\Desktop\\userCred.xml";
            if (File.Exists(file))
            {

                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(file);

                foreach (XmlNode xNode in xdoc.SelectNodes("users/user"))
                    if (xNode.SelectSingleNode("email").InnerText == email)
                        return true;
                xdoc.Save(file);
                return false;
            }
            else return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string file = "C:\\Users\\Kiran\\Desktop\\userCred.xml";
            User user = new User();
            user.firstName = firstNameBox.Text;
            user.lastName = lastNameBox0.Text;
            user.email = eMailBox.Text;
            if (isPresent(user.email))  //If email already registered
            {
                emailIdError.Visible = true;
                eMailBox.Text = "";
            }
            else
            {
                if (passwordBox.Text != confirmPasswordBox.Text)
                {
                    confirmPasswordError.Visible = true;
                    confirmPasswordBox.Text = "";

                }
                else
                {
                    user.password = HashSHA1(confirmPasswordBox.Text);


                    //Verify whether a file is exists or not
                    if (!File.Exists(file))
                    {
                        XmlTextWriter xWriter = new XmlTextWriter(file, Encoding.UTF8);
                        xWriter.Formatting = Formatting.Indented;
                        xWriter.WriteStartElement("users");
                        xWriter.WriteStartElement("user");
                        xWriter.WriteStartElement("firstName");
                        xWriter.WriteString(user.firstName);
                        xWriter.WriteEndElement();
                        xWriter.WriteStartElement("lastName");
                        xWriter.WriteString(user.lastName);
                        xWriter.WriteEndElement();
                        xWriter.WriteStartElement("password");
                        xWriter.WriteString(user.password);
                        xWriter.WriteEndElement();
                        xWriter.WriteStartElement("email");
                        xWriter.WriteString(user.email);
                        xWriter.WriteEndElement();
                        xWriter.WriteStartElement("cart");
                        xWriter.WriteString("no");
                        xWriter.WriteEndElement();
                        xWriter.Close();
                    }
                    else
                    {
                        XmlDocument doc = new XmlDocument();
                        
                        doc.Load(file);
                        XmlNode userNode = doc.CreateElement("user");
                        XmlNode userNameNode = doc.CreateElement("firstName");
                        userNameNode.InnerText = user.firstName;
                        XmlNode userPhoneNode = doc.CreateElement("lastName");
                        userPhoneNode.InnerText = user.lastName;
                        XmlNode userPasswordNode = doc.CreateElement("password");
                        userPasswordNode.InnerText = user.password;
                        XmlNode userEmailNode = doc.CreateElement("email");
                        userEmailNode.InnerText = user.email;
                        XmlNode userCartNode = doc.CreateElement("cart");
                        userCartNode.InnerText = "no";
                        userNode.AppendChild(userNameNode);
                        userNode.AppendChild(userPhoneNode);
                        userNode.AppendChild(userPasswordNode);
                        userNode.AppendChild(userEmailNode);
                        userNode.AppendChild(userCartNode);
                        doc.DocumentElement.AppendChild(userNode);
                        doc.Save(file);

                    }

                    //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 25);

                    //smtpClient.Credentials = new System.Net.NetworkCredential("kplogger154@gmail.com", "kiran154");
                    //smtpClient.UseDefaultCredentials = true;
                    //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtpClient.EnableSsl = true;
                    //MailMessage mail = new MailMessage();
                    
                    ////Setting From , To and CC
                    //mail.From = new MailAddress("info@MyWebsiteDomainName", "MyWeb Site");
                    //mail.To.Add(new MailAddress(eMailBox.Text));       
                    //smtpClient.Send(mail);


                    Response.Redirect("~/WelcomePage.aspx");
                }
            }
        }

        protected void eMailBox_TextChanged(object sender, EventArgs e)
        {
            emailIdError.Visible = false;
        }
    }
}