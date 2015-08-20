using System;
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
    public partial class Inventory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IdTextBox.Enabled = false;
            NameTextBox.Enabled = false;
            PriceTextBox.Enabled = false;
            QauntityTextBox.Enabled = false;
            OkButton.Enabled = false;
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            OkButton.Text = DropDownList.SelectedValue;
            IdTextBox.Enabled = true;
            NameTextBox.Enabled = true;
            PriceTextBox.Enabled = true;
            QauntityTextBox.Enabled = true;
            OkButton.Enabled = true;
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            string file = "C:\\Users\\Kiran\\Desktop\\Inventory.xml";
            if ((IdTextBox.Text == null) || (NameTextBox.Text == null) || (PriceTextBox.Text == null) || (QauntityTextBox.Text == null))
            {

                ErrorLabel.Visible = true;
            }
            else {
                CartObject item = new CartObject();
                item.itemId = IdTextBox.Text;
                item.itemName = NameTextBox.Text;
                item.itemPrice = Convert.ToDouble(PriceTextBox.Text);
                item.itemQuantity = Convert.ToUInt16(QauntityTextBox.Text);

                if (!File.Exists(file))
                {
                    XmlTextWriter xWriter = new XmlTextWriter(file, Encoding.UTF8);
                    xWriter.Formatting = Formatting.Indented;
                    xWriter.WriteStartElement("items");
                    xWriter.WriteStartElement("item");
                    xWriter.WriteStartElement("itemId");
                    xWriter.WriteString(item.itemId);
                    xWriter.WriteEndElement();
                    xWriter.WriteStartElement("itemName");
                    xWriter.WriteString(item.itemName);
                    xWriter.WriteEndElement();
                    xWriter.WriteStartElement("itemPrice");
                    xWriter.WriteString(Convert.ToString(item.itemPrice));
                    xWriter.WriteEndElement();
                    xWriter.WriteStartElement("itemQuantity");
                    xWriter.WriteString(Convert.ToString(item.itemQuantity));
                    xWriter.WriteEndElement();
                    xWriter.Close();
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    XmlNode itemNode = doc.CreateElement("item");
                    XmlNode itemIdNode = doc.CreateElement("itemId");
                    itemIdNode.InnerText = item.itemId;
                    XmlNode itemNameNode = doc.CreateElement("itemName");
                    itemNameNode.InnerText = item.itemName;
                    XmlNode itemPriceNode = doc.CreateElement("itemPrice");
                    itemPriceNode.InnerText = Convert.ToString(item.itemPrice);
                    XmlNode itemQuantityNode = doc.CreateElement("itemQuantity");
                    itemQuantityNode.InnerText = Convert.ToString(item.itemQuantity);
                    itemNode.AppendChild(itemIdNode);
                    itemNode.AppendChild(itemNameNode);
                    itemNode.AppendChild(itemPriceNode);
                    itemNode.AppendChild(itemQuantityNode);
                    doc.DocumentElement.AppendChild(itemNode);
                    doc.Save(file);

                }

            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}