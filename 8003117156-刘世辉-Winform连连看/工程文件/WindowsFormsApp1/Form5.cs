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
    public partial class Form5 : Form
    {
        public int time = 1000 * 60 * 20;
        //默认二十分钟

        public string mode = "普通";
        //默认的模式是普通

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //简单模式
            this.time = 1000 * 60 * 30;
            //30分钟
            this.mode = "简单";

            this.Dispose();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //普通模式
            //使用默认值
            //20分钟

            this.Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //困难模式
            this.time = 1000 * 60 * 12;
            //12分钟
            this.mode = "困难";

            this.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //地狱模式
            this.time = 1000 * 60 * 8;
            //8分钟
            this.mode = "地狱";

            this.Dispose();
        }
    }
}
