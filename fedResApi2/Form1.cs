using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proxys;
using MSG;
using Encumbrance;

namespace fedResApi2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox1.Text == null)
            {
                MessageBox.Show("select file");
                return;
            }
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked && !checkBox10.Checked)
            {
                MessageBox.Show("чтобы продолжить, укажите что имеется для поиска");
                return;
            }
            if (!checkBox6.Checked && !checkBox7.Checked && !checkBox8.Checked && !checkBox9.Checked)
            {
                MessageBox.Show("чтобы продолжить, укажите что необходимо вытащить");
                return;
            }
            ProxyClass.GetFromTxt();
            List<Person> people = new List<Person>();
            List<Search> searches = new List<Search>();
            List<PageData> pageData = new List<PageData>();
            List<string> inns = new List<string>();
            List<string> searchStrings = new List<string>();
            List<Encumbrances> encumbrances = new List<Encumbrances>();
            Messages message = new Messages();
            if (checkBox4.Checked ) 
            {
                people = people.GetFromTxt(textBox1.Text, true);
            }//поиск по ФИО
            if (checkBox3.Checked)
            {
                inns = SearchM.GetListInns(textBox1.Text);
            }//поиск по ИНН
            if (checkBox1.Checked)
            {
                searches = searches.AddFromTxt(textBox1.Text.Trim());
            }//поиск по SearchList
            if (checkBox2.Checked)
            {
                pageData = pageData.AddFromTxt(textBox1.Text.Trim());
            }//поиск по PageData
            if (checkBox10.Checked)
            {
                searchStrings = Encumbrances.GetSearchStrings(textBox1.Text.Trim());
            }//поиск по ключ словам, участникам, номеру сообщения пока только 
            if (checkBox9.Checked)
            {
                searches = searches.AddFromSite(inns, 100);
                searches.Write();
            }//получить SearchList
            if (checkBox7.Checked)
            {
                if (!checkBox10.Checked)
                {
                    pageData = new List<PageData>().AddFromSite(searches, 1);
                    pageData.Write();
                }
                else 
                {
                    encumbrances = Encumbrances.GetFromSite(searchStrings);
                }
            }//получить PageData
            //if (checkBox8.Checked) { List<MessageFedRes> messageFedRess = new List<MessageFedRes>().AddFromSite(pageData); }
            if (checkBox8.Checked) 
            {
                message.AddMessages(pageData, 1);
            }
            MessageBox.Show("ok");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.txt|*.txt";
            ofd.ShowDialog();
            if (ofd.FileName != null) { textBox1.Text = ofd.FileName; }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox5.Enabled = false;
                checkBox6.Checked = false;
                checkBox6.Enabled = false;
                checkBox7.Checked = true;
                checkBox7.Enabled = true;
                checkBox8.Checked = true;
                checkBox8.Enabled = true;
                checkBox9.Checked = false;
                checkBox9.Enabled = false;
                checkBox10.Checked = false;
            }
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked)
            {
                checkBox5.Checked = false;
                checkBox5.Enabled = false;
                checkBox6.Checked = false;
                checkBox6.Enabled = false;
                checkBox7.Checked = false;
                checkBox7.Enabled = false;
                checkBox8.Checked = false;
                checkBox8.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox5.Enabled = false;
                checkBox6.Checked = false;
                checkBox6.Enabled = false;
                checkBox7.Checked = false;
                checkBox7.Enabled = false;
                checkBox8.Checked = true;
                checkBox8.Enabled = true;
                checkBox10.Checked = false;
            }
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked)
            {
                checkBox5.Checked = false;
                checkBox5.Enabled = false;
                checkBox6.Checked = false;
                checkBox6.Enabled = false;
                checkBox7.Checked = false;
                checkBox7.Enabled = false;
                checkBox8.Checked = false;
                checkBox8.Enabled = false;
                checkBox9.Checked = false;
                checkBox9.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox5.Enabled = false;
                checkBox7.Checked = true;
                checkBox7.Enabled = true;
                checkBox6.Checked = true;
                checkBox6.Enabled = true;
                checkBox8.Checked = true;
                checkBox8.Enabled = true;
                checkBox9.Checked = true;
                checkBox9.Enabled = true;
                checkBox10.Checked = false;
            }
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked)
            {
                checkBox5.Checked = false;
                checkBox5.Enabled = false;
                checkBox6.Checked = false;
                checkBox6.Enabled = false;
                checkBox7.Checked = false;
                checkBox7.Enabled = false;
                checkBox8.Checked = false;
                checkBox8.Enabled = false;
                checkBox9.Checked = false;
                checkBox9.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox5.Enabled = true;
                checkBox7.Checked = true;
                checkBox7.Enabled = true;
                checkBox6.Checked = true;
                checkBox6.Enabled = true;
                checkBox8.Checked = true;
                checkBox8.Enabled = true;
                checkBox9.Checked = true;
                checkBox9.Enabled = true;
                checkBox10.Checked = false;
            }
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked)
            {
                checkBox5.Checked = false;
                checkBox5.Enabled = false;
                checkBox6.Checked = false;
                checkBox6.Enabled = false;
                checkBox7.Checked = false;
                checkBox7.Enabled = false;
                checkBox8.Checked = false;
                checkBox8.Enabled = false;
                checkBox9.Checked = false;
                checkBox9.Enabled = false;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox7.Checked && !checkBox2.Checked)
            {
                checkBox8.Checked = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox7.Checked && !checkBox2.Checked && checkBox8.Checked)
            {
                checkBox7.Checked = true;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox9.Checked && (checkBox3.Checked || checkBox4.Checked))
            {
                checkBox7.Checked = false;
                checkBox7.Enabled = false;
                checkBox8.Checked = false;
                checkBox8.Enabled = false;
            }
            if (checkBox9.Checked && (checkBox3.Checked || checkBox4.Checked))
            {
                checkBox7.Checked = false;
                checkBox7.Enabled = true;
                checkBox8.Checked = false;
                checkBox8.Enabled = true;
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox10.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Enabled = false;
                checkBox7.Checked = true;
                checkBox7.Enabled = true;
                checkBox6.Checked = false;
                checkBox6.Enabled = false;
                checkBox8.Checked = false;
                checkBox8.Enabled = false;
                checkBox9.Checked = false;
                checkBox9.Enabled = false;
            }
        }
    }
}
