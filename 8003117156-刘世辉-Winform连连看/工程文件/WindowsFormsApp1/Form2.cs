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
    public partial class Form2 : Form
    {
        private LLKDataContext db = new LLKDataContext();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            var results = from r in db.games
                          orderby r.分数 descending
                          select new ShowBar
                          {
                              Mode = r.模式,
                              Email = r.邮箱,
                              Score = r.分数,
                              User = this.Who(r.邮箱)
                          };
            int index = 0;
            foreach(var i in results)
            {
                if(index < 10)
                {
                    index = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[index].Cells[0].Value = i.Mode;
                    this.dataGridView1.Rows[index].Cells[1].Value = i.Score;
                    this.dataGridView1.Rows[index].Cells[2].Value = i.User;
                    this.dataGridView1.Rows[index].Cells[3].Value = i.Email; 
                }
                else
                    break;
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private string Who(string email)
        {

            var result = from r in db.users
                         where r.邮箱 == email
                         select r;
            foreach (var i in result)
            {
                return i.用户名;
            }
            return null;
        }
    }
}
