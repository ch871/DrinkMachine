using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DrinkMachine
{

    public partial class Form1 : Form
    {
        //יצירת משתנה שיכיל את הקובץ
        XmlDocument XmlDocument;
        //הכתובת במחשב שיאוחסן בו הקובץ
        string pathName = Directory.GetCurrentDirectory() + "\\drinkData.xml";
        public Form1()
        {
            InitializeComponent();
            //יצירת מופע של XML
            XmlDocument = new XmlDocument();
            //אם קיים קובץ במיקום הזה
            if (File.Exists(pathName))
            {
                try
                {
                    //טעינה של הקובץ
                    XmlDocument.Load(pathName);
                }
                catch (Exception ex)
                {
                    //שגיעת טעינה
                    MessageBox.Show("ישך' בעיה בטעינה" + ex.Message);
                }
            }
            //אם לא קיים שם קובץ
            else
            {
                //יצירת תגית ראשית לקובץ
                XmlNode root = XmlDocument.CreateElement("Drinks");
                //הוספת התגית לקובץ
                XmlDocument.AppendChild(root);
                //שמירת הקובץ במחשב
                XmlDocument.Save(pathName);
            }
            //קראה לפונקציה שמציגה את הקובץ בגריד
            ShowAllDrinks();
        }
        //יצירת משקה לפי מבנה מסוכם. מקבל את התגית הראשית כפרמטר
        private bool CreateDrink(XmlNode root)
        {
            try
            {
                //יצירת תגית למשקה
                XmlNode drink = XmlDocument.CreateElement("drink");
                //הכנסת תגיות וערכים למשקה
                drink.AppendChild(XmlDocument.CreateElement("name")).InnerText = txtNewDrinke.Text;
                drink.AppendChild(XmlDocument.CreateElement("suger")).InnerText = cmbSugar.Text;
                drink.AppendChild(XmlDocument.CreateElement("coffe")).InnerText = cmbCffe.Text;
                drink.AppendChild(XmlDocument.CreateElement("coco")).InnerText = cmbCocou.Text;
                drink.AppendChild(XmlDocument.CreateElement("milk")).InnerText = cmbMilk.Text;
                drink.AppendChild(XmlDocument.CreateElement("price")).InnerText = txtPrice.Text;
                //הכנסת המשקה לקובץ
                root.AppendChild(drink);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //פונקציה בלחיצה על הוספת משקה
        private void btnAddDrink_Click(object sender, EventArgs e)
        {
            //שליחה לפונקציה של היצירה ושליחת התגית הראשית של הקובץ
            CreateDrink(XmlDocument.FirstChild);
        }
        //ניקוי התאים בעמוד
        private void cleer()
        {
            foreach (Control ctr in Controls)
            {
                if (ctr is ComboBox || ctr is TextBox)
                {
                    ctr.Text = string.Empty;
                }
                txtPrice.Text = "0";
            }
        }
        //פונקציה בלחיצה על הכפתור שמירה
        private void btnSave_Click(object sender, EventArgs e)
        {
            //שמירת הקובץ שיצרתי
            XmlDocument.Save(pathName);
            //ניקוי הדף
            cleer();
            //הצגת המשקאות
            ShowAllDrinks();
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            txtPrice.Text = (double.Parse(txtPrice.Text) + 0.5).ToString();
        }

        private void btnMinus_Click(object sender, EventArgs e)
        { 
            if (txtPrice.Text == "0")
            {
                return;
            }
            txtPrice.Text = (double.Parse(txtPrice.Text) - 0.5).ToString();
        }
        //בלחיצה על כפתור המחיקה
        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (XmlNode drink in XmlDocument.FirstChild.ChildNodes)
            {
                if (drink.SelectSingleNode("name").InnerText == txtNewDrinke.Text)
                {
                    XmlDocument.FirstChild.RemoveChild(drink);
                }
            }
            ShowAllDrinks();
        }       
        //הצגת כל המשקאות
        private void ShowAllDrinks()
        {
            dgvDrinksTable.Rows.Clear();
            foreach (XmlNode drink in XmlDocument.FirstChild.ChildNodes)
            {
                string name = "", suger = "", coffee = "", cocoa = "", price = "", milk = "";
                foreach (XmlNode node in drink.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "name":
                            name = node.InnerText; break;
                        case "suger":
                            suger = node.InnerText; break;
                        case "coffe":
                            coffee = node.InnerText; break;
                        case "coco":
                            cocoa = node.InnerText; break;
                        case "milk":
                            milk = node.InnerText; break;
                        case "price":
                            price = node.InnerText; break;
                    }
                }
                dgvDrinksTable.Rows.Add(name, suger, coffee, cocoa, milk, price+" ש''ח ");
        }   }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            foreach (XmlNode drink in XmlDocument.FirstChild.ChildNodes)
            {
                if (drink.SelectSingleNode("name").InnerText == txtNewDrinke.Text)
                {
                    drink.SelectSingleNode("suger").InnerText = cmbSugar.Text;
                    drink.SelectSingleNode("coffe").InnerText = cmbCffe.Text;
                    drink.SelectSingleNode("milk").InnerText = cmbMilk.Text;
                    drink.SelectSingleNode("coco").InnerText = cmbCocou.Text;
                    drink.SelectSingleNode("price").InnerText = txtPrice.Text;
                    ShowAllDrinks();
                }

            } 

        }

    }
}
