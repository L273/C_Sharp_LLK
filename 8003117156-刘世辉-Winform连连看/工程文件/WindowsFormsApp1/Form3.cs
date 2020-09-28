using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        private bool into;

        public string email;
        //记录登录的邮箱

        private LLKDataContext db = new LLKDataContext();

        public Form3()
        {
            InitializeComponent();
            
            //初始into为false
            this.into = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.ToString().Trim();
            string passwd = textBox2.Text.ToString().Trim();

            if (email == "")
            {
                MessageBox.Show("邮箱不能输入为空");
                return ;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[0-9a-zA-Z]+@[0-9a-zA-Z]+.com$"))
            {
                MessageBox.Show("请输入正确的邮箱格式");
                return;
            }

            if (passwd == "")
            {
                MessageBox.Show("密码不能输入为空");
                return;
            }

            if (this.check(email, passwd)) {
                this.into = true;
                this.email = email;
                this.Hide();
            }
            else
            {
                MessageBox.Show(this.GetWhy());
            }
        }
        private bool check(string email,string passswd)
        {
            
            var results = from r in db.users
                          where r.邮箱 == email && r.密码 == passswd
                          select r;

            foreach (var i in results)
            { 
                return true;
            }
            return false;
        }

        private string GetWhy()
        {
            string email = textBox1.Text.ToString().Trim();
            var results = from r in db.users
                          where r.邮箱 == email
                          select r;
            foreach (var i in results)
            {
                return "密码错误";
            }
            return "没有该用户";
        }


        private void Form3_Load(object sender, EventArgs e)
        {

        }
       
        public bool getInto()
        {
            return this.into;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
