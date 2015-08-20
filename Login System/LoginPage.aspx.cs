using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

using System.Web.UI.WebControls;


using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections;

namespace Login_System
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            notFoundError.Visible = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

                    
            User user = new User();
            bool dataPresent = false;
            
            Hashtable cartTable = new Hashtable();
            string file = "C:\\Users\\Kiran\\Desktop\\userCred.xml";
            string cartFile = "C:\\Users\\Kiran\\Desktop\\cart.xml";



            if ((TextBox1.Text.Length > 1) && (TextBox2.Text.Length > 1))
            {

                string email = TextBox1.Text;
                string passwd = HashSHA1(TextBox2.Text);

                if (Authenticate(email, passwd))
                {
                    if (File.Exists(file))
                    {
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.Load(file);

                        foreach (XmlNode xNode in xdoc.SelectNodes("users/user"))
                            if (xNode.SelectSingleNode("email").InnerText == TextBox1.Text)
                            {
                                user.firstName = xNode.SelectSingleNode("firstName").InnerText;
                                user.lastName = xNode.SelectSingleNode("lastName").InnerText;
                                user.email = xNode.SelectSingleNode("email").InnerText;
                                if (xNode.SelectSingleNode("cart").InnerText == "yes")
                                    dataPresent = true;
                            }

                    }
                    if (dataPresent)
                    {
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.Load(cartFile);
                        foreach (XmlNode xNode in xdoc.SelectNodes("carts"))
                            if (xNode.SelectSingleNode("cart").Attributes["emailId"].Value == user.email)
                            {
                                foreach (XmlNode xNode1 in xdoc.SelectNodes("book"))
                                {
                                    
                                   
                                    string itemName = xNode.SelectSingleNode("name").InnerText;
                                    double itemPrice = Convert.ToDouble(xNode.SelectSingleNode("price").InnerText);
                                    cartTable.Add(itemName, itemPrice);
                                    

                                }

                            }
                        //break somewhere here
                    }
                    //user object created
                    if (cartTable.Count == 0) Session["cartTable"] = "No Items Found";
                    else Session["cartTable"] = "Cart ( " + cartTable.Count.ToString() +" )";
                    Session["userName"] = user.firstName;
                    Session["cartCount"] = cartTable.Count.ToString();
                    Session["emailId"] = user.email;

                    Response.Redirect("~/WelcomePage.aspx");
                }
                else {
                    notFoundError.Visible = true;
                }
            }
            
         }

        protected Boolean Authenticate(string email, string passwd) {
            string file = "C:\\Users\\Kiran\\Desktop\\userCred.xml";
            if (File.Exists(file))
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(file);

                foreach (XmlNode xNode in xdoc.SelectNodes("users/user"))
                    if ((xNode.SelectSingleNode("email").InnerText == email) && (xNode.SelectSingleNode("password").InnerText == passwd))
                    {
                        xdoc.Save(file);
                        return true;
                    }

                xdoc.Save(file);
                return false;
            }
            else {
                return false;
            }

            //string userCredFile = "~/ UserCred.xml";
            //DataSet ds = new DataSet();

            //FileStream fs = new FileStream(Server.MapPath(userCredFile),
            //FileMode.Open, FileAccess.Read);
            //StreamReader reader = new StreamReader(fs);
            //ds.ReadXml(reader);
            //fs.Close();

            


            
        }

        protected string HashSHA1(string value)
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

    }
}