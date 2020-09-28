using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //接受数据并去除空格
            string email = textBox1.Text.Trim();
            string name = textBox3.Text.Trim();
            string passwd = textBox2.Text.Trim();

            if (!Regex.IsMatch(email, @"^[0-9a-zA-Z]+@[0-9a-zA-Z]+.com$"))
            {
                MessageBox.Show("请输入正确的邮箱格式");
                return ;
            }

            if (!Regex.IsMatch(name, @"^[A-Za-z]+$"))
            {
                MessageBox.Show("用户名必须为字母");
                return ;
            }

            if (!Regex.IsMatch(passwd, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("密码必须是数字加字母");
                return ;
            }

            if (name.Length > 10)
            {
                MessageBox.Show("用户名的长度只能在10以内");
                return;
            }

            if (email.Length > 10)
            {
                MessageBox.Show("邮箱的长度只能在10以内");
                return ; 
            }

            if (passwd.Length > 10)
            {
                MessageBox.Show("密码只能在长度10以内");
                return ; 
            }
            

            if (up_Date_Db(email, name, passwd))
            {
                MessageBox.Show("注册成功");
                this.Dispose();
            }
            else
                MessageBox.Show("注册失败");

            
        }

        private bool up_Date_Db(string email,string name, string passwd)
        {
            LLKDataContext db = new LLKDataContext();
            users u1 = new users()
            {
                邮箱 = email,
                用户名 = name,
                密码 = passwd,
                盘数 = 0,
                最高分 = 0
            };

            try
            {
                db.users.InsertOnSubmit(u1);
                //提交数据

                db.SubmitChanges();
                //修改

                //如果发生异常，则无法执行到此步，于是就会在catch内返回一个false;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
