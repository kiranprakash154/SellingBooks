using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Login_System
{
    public partial class CheckoutPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Double finalAmount = 0.0;
            if (!IsPostBack)
            {
                List<CartObject> cartItems = new List<CartObject>();
                string emailId = "kiranprakash154@gmail.com";
                string cartFile = "C:\\Users\\Kiran\\Desktop\\cart.xml";
                XmlDocument doc = new XmlDocument();
                doc.Load(cartFile);


                foreach (XmlNode xNode in doc.SelectNodes("carts/cart"))
                {

                    //XmlNode cartNode = xNode.SelectSingleNode("cart");
                    if (xNode.Attributes != null)
                    {

                        if (xNode.Attributes["emailId"].Value == emailId)
                        {

                            XmlNodeList booksNodeList = xNode.ChildNodes;

                            foreach (XmlNode xBookNode in booksNodeList)
                            {
                                //added to the list to use later which may come handy while searching
                                CartObject tempCartObject = new CartObject();

                                tempCartObject.itemName = xBookNode.SelectSingleNode("name").InnerText;
                                tempCartObject.itemPrice = Convert.ToDouble(xBookNode.SelectSingleNode("price").InnerText);
                                finalAmount += tempCartObject.itemPrice;
                                tempCartObject.itemQuantity = Convert.ToUInt16(xBookNode.SelectSingleNode("quantity").InnerText);
                                cartItems.Add(tempCartObject);

                                //adding to the checkbox list to display on page
                                ListItem item = new ListItem();
                                item.Text = tempCartObject.itemName + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + tempCartObject.itemPrice + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + tempCartObject.itemQuantity;
                                item.Value = tempCartObject.itemName;
                                ListBox1.Items.Add(item);
                                //CheckBoxList1.Items.Add(item);


                            }

                        }
                    }

                }





                if (!Page.IsPostBack)
                {
                    DataTable dtItems = new DataTable();
                    dtItems.Columns.Add("itemCount");
                    dtItems.Columns.Add("itemValue");
                    dtItems.Columns.Add("quantityValue");
                    dtItems.Columns.Add("amountValue");
                    dtItems.Rows.Add("1", "Cellphone", "10", "200.00");
                    dtItems.Rows.Add("2", "Bag", "2", "250.00");
                    dtItems.Rows.Add("3", "Mouse", "10", "3500.00");
                    dtItems.Rows.Add("4", "Keyboard", "5", "200.00");

                    rptItems.DataSource = dtItems;
                    rptItems.DataBind();
                }
                //FinalAmountLabel.Text = finalAmount.ToString();
            }
        }
    }
}