using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Login_System
{
    public partial class CartPage : System.Web.UI.Page
    {



        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                List<CartObject> cartItems = new List<CartObject>();
                string emailId = "kiranprakash154@gmail.com";
                string cartFile = "C:\\Users\\Kiran\\Desktop\\cart.xml";
                XmlDocument doc = new XmlDocument();
                doc.Load(cartFile);


                foreach (XmlNode xNode in doc.SelectNodes("carts/cart"))
                {
                    //TextBox1.Text += "Entered for each loop<br/>";
                    //XmlNode cartNode = xNode.SelectSingleNode("cart");
                    if (xNode.Attributes != null)
                    {
                        //TextBox1.Text += "Found attributes<br/>";
                        if (xNode.Attributes["emailId"].Value == emailId)
                        {
                            //TextBox1.Text += "Found email ID<br/>";
                            XmlNodeList booksNodeList = xNode.ChildNodes;

                            foreach (XmlNode xBookNode in booksNodeList)
                            {

                                //TextBox1.Text += "Printing books<br/>";
                                //added to the list to use later which may come handy while searching
                                CartObject tempCartObject = new CartObject();

                                tempCartObject.itemName = xBookNode.SelectSingleNode("name").InnerText;
                                tempCartObject.itemPrice = Convert.ToDouble(xBookNode.SelectSingleNode("price").InnerText);
                                tempCartObject.itemQuantity = Convert.ToUInt16(xBookNode.SelectSingleNode("quantity").InnerText);
                                cartItems.Add(tempCartObject);

                                //adding to the checkbox list to display on page
                                ListItem item = new ListItem();
                                item.Text = tempCartObject.itemName + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + tempCartObject.itemPrice + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + tempCartObject.itemQuantity;
                                item.Value = tempCartObject.itemName;
                                CheckBoxList1.Items.Add(item);


                            }

                        }
                    }
                   

                }



            }
        }

            

        protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList list = (CheckBoxList)sender;

            string[] control = Request.Form.Get("__EVENTTARGET").Split('$');
            int idx = control.Length - 1;
            string sel = list.Items[int.Parse(control[idx])].Value;
        }
    }
}