using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Login_System
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        Hashtable allItemsHashtable = new Hashtable();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
             cartLink.Text = Session["cartTable"].ToString();
             string file = "C:\\Users\\Kiran\\Desktop\\Inventory.xml";
             string gap = "                                      ";
             //ListItem item;
             XmlDocument xDoc = new XmlDocument();
             UserNameLabel.Text = Session["userName"].ToString();
             xDoc.Load(file);
             
             foreach (XmlNode node in xDoc.SelectNodes("items/item"))
             {
                string itemName = node.SelectSingleNode("itemName").InnerText;
                string itemPrice = node.SelectSingleNode("itemPrice").InnerText;
                ListItem item = new ListItem();
                item.Text = itemName + gap + itemPrice;
                item.Value = itemName;
                CheckBoxList1.Items.Add(item);
                allItemsHashtable.Add(itemName, itemPrice);
                //++position;
             }

           }

        }



        protected void addToCartButton_Click(object sender, EventArgs e)
        {
            List<CartObject> newCartObj = new List<CartObject>();
            //Hashtable selectedItemsHashtable = new Hashtable();

            //string file = "C:\\Users\\Kiran\\Desktop\\userCred.xml";
            //Hashtable selectedItemsHashtable = new Hashtable();
            

            // Loop through each item.
            foreach (ListItem item in CheckBoxList1.Items)
            {
                if (item.Selected)
                {
                    // If the item is selected, add the value to the list.
                    CartObject tempCartObject = new CartObject();
                    tempCartObject.itemName = item.Value.ToString();
                    tempCartObject.itemPrice = Convert.ToDouble(allItemsHashtable[tempCartObject.itemName]);
                    //selectedItemsHashtable.Add(tempCartObject.itemName, tempCartObject.itemPrice);
                    newCartObj.Add(tempCartObject);

                }

            }
            int size = newCartObj.Count;
            size += Convert.ToUInt16(Session["cartCount"].ToString());
            cartLink.Text = "Cart ( " + size + " )";
            updateCartFile(newCartObj);
        }

        public void updateCartFile(List<CartObject> newCartObj)
        {
            bool found = false;

            string cartFile = "C:\\Users\\Kiran\\Desktop\\cart.xml";

            if (!File.Exists(cartFile))
            {
                XmlTextWriter xWriter = new XmlTextWriter(cartFile, Encoding.UTF8);
                xWriter.Formatting = Formatting.Indented;
                xWriter.WriteStartElement("carts");
                xWriter.WriteStartElement("cart");
                xWriter.WriteAttributeString("emailId", Session["emailId"].ToString());
                xWriter.Close();
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(cartFile);
            foreach (CartObject cartItem in newCartObj)
            {
                foreach (XmlNode xNode in doc.SelectNodes("carts"))
                {
                    XmlNode cartNode = xNode.SelectSingleNode("cart");
                    if (cartNode.Attributes["emailId"].InnerText == Session["emailId"].ToString())
                    {
                        found = true;
                        XmlNode bookNode = doc.CreateElement("book");
                        XmlNode nameNode = doc.CreateElement("name");
                        nameNode.InnerText = cartItem.itemName;
                        XmlNode priceNode = doc.CreateElement("price");
                        priceNode.InnerText = cartItem.itemPrice.ToString();
                        XmlNode quantityNode = doc.CreateElement("quantity");
                        quantityNode.InnerText = "1";
                        bookNode.AppendChild(nameNode);
                        bookNode.AppendChild(priceNode);
                        bookNode.AppendChild(quantityNode);
                        cartNode.AppendChild(bookNode);                       
                    }
                }                    
                if(!found)
                  {
                        XmlNode cartNode = doc.CreateElement("cart");
                        cartNode.Attributes["emailId"].InnerText = Session["emailId"].ToString();
                        XmlNode bookNode = doc.CreateElement("book");
                        XmlNode nameNode = doc.CreateElement("name");
                        nameNode.InnerText = cartItem.itemName;
                        XmlNode priceNode = doc.CreateElement("price");
                        priceNode.InnerText = cartItem.itemPrice.ToString();
                        XmlNode quantityNode = doc.CreateElement("quantity");
                        quantityNode.InnerText = "1";
                        bookNode.AppendChild(nameNode);
                        bookNode.AppendChild(priceNode);
                        bookNode.AppendChild(quantityNode);
                        cartNode.AppendChild(bookNode);
                        doc.DocumentElement.AppendChild(cartNode);
                        
                  }
                }
            doc.Save(cartFile);
        }

        protected void checkOutButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CheckoutPage.aspx");
        }
    }

        
}
