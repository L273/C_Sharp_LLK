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
    public partial class Form1 : Form
    {
        private Color color = Color.Black; //字体的颜色
        private Font font = new Font("宋体", 20, FontStyle.Bold);
        private GoBang b1 = new GoBang();
        private Point p1;
        private Point p2;
        private bool trans=true;

        private int time;
        //记录主计时器的毫秒数

        Timer clock = new Timer();
        //定时器,与显示时间有关

        Timer judge_clock = new Timer();
        Judge judge = new Judge();
        //与游戏的状态有关

        private string email;
        private string mode;
        //记录邮箱和模式
        private long score=0;
        //游戏分数的初始化

        public Form1()
        {
            InitializeComponent();
            //未输入正确数据，结束Form1窗口
        }

        private void DrawChess()
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            //先清理

            Pen pen = new Pen(Color.Gold);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Brush bush = new SolidBrush(this.color);//填充的颜色

            for (int i = 0; i <= 600; i = i + 25)
            {
                g.DrawLine(pen, 0, i, 600, i);
                //画竖线
                g.DrawLine(pen, i, 0, i, 600);
                //画横线
            }

            //填字母
            for (int i = 1; i < 25; i++)
            {
                for (int j = 1; j < 25; j++)
                {
                    g.DrawString(b1.chess[i, j].ToString(), this.font, bush, (i - 1) * 25, (j - 1) * 25);
                    //填字母
                }
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (b1.start)
            {
                //如果b1内的棋盘初始化了的话
                this.Cursor = System.Windows.Forms.Cursors.Hand;
                //点击时，鼠标变成小手

                int[] p1 = new int[2];
                int[] p2 = new int[2];
                Graphics g = pictureBox1.CreateGraphics();
                Pen pen = new Pen(Color.Gold);

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Brush bush = new SolidBrush(Color.Red);//填充的颜色

                if (this.trans)
                {
                    this.p1 = this.PointToClient(Control.MousePosition);
                    trans = !trans;
                    p1[0] = (this.p1.X ) / 25 + 1;
                    p1[1] = (this.p1.Y ) / 25 + 1;
                    //获取相对窗体左上角坐标

                    //变红，以便观察
                    g.DrawString(b1.chess[p1[0], p1[1]].ToString(), this.font, bush, (p1[0] - 1) * 25, (p1[1] - 1) * 25);

                }
                else
                {
                    this.p2 = this.PointToClient(Control.MousePosition);
                    trans = !trans;
                    //获取相对窗体左上角坐标

                    //MessageBox.Show("P1:横坐标" + ((this.p1.X-30)/25).ToString()+" P1:纵坐标"+ ((this.p1.Y - 30) / 25).ToString());
                    //MessageBox.Show("P2:横坐标" + ((this.p2.X - 30) / 25).ToString() + " P2:纵坐标" + ((this.p2.Y - 30) / 25).ToString());
                    //点击第二次开始检查
                    //由于数据导入后会重新刷新，所以这里要重新赋值
                    p1[0] = (this.p1.X ) / 25 + 1;
                    p1[1] = (this.p1.Y ) / 25 + 1;

                    p2[0] = (this.p2.X ) / 25 + 1;
                    p2[1] = (this.p2.Y ) / 25 + 1;
                    //如果刷新成功的话
                    g.DrawString(b1.chess[p2[0], p2[1]].ToString(), this.font, bush, (p2[0] - 1) * 25, (p2[1] - 1) * 25);

                    bush = new SolidBrush(Color.White);//填充的颜色

                    g.FillRectangle(bush, (p1[0] - 1) * 25 + 1, (p1[1] - 1) * 25 + 1, 23, 23);
                    g.FillRectangle(bush, (p2[0] - 1) * 25 + 1, (p2[1] - 1) * 25 + 1, 23, 23);
                    //用白色矩阵填充点击后的图谱


                    if (b1.FlushChess(p1, p2))
                    {
                        if (this.mode == "地狱")
                        {
                            this.score += 25;
                        }else if(this.mode == "困难")
                        {
                            this.score += 12;
                        }else if (this.mode == "普通")
                        {
                            this.score += 5;
                        }else if (this.mode == "简单")
                        {
                            this.score += 1;
                        }
                        label3.Text = "分数："+score.ToString();
                    }
                    else
                    {
                        //如果没有刷新成功的话，则重置字体

                        //恢复选择字体的颜色
                        bush = new SolidBrush(this.color);//填充的颜色
                        g.DrawString(b1.chess[p1[0], p1[1]].ToString(), this.font, bush, (p1[0] - 1) * 25, (p1[1] - 1) * 25);
                        g.DrawString(b1.chess[p2[0], p2[1]].ToString(), this.font, bush, (p2[0] - 1) * 25, (p2[1] - 1) * 25);
                    }
                }
            }
            else
            {
                MessageBox.Show("请先点击开始");
            }
            //更新chess内的值

            //MessageBox.Show(p1[0].ToString());
            //MessageBox.Show(p1[1].ToString());

            //MessageBox.Show(b1.chess[p1[0], p1[1]].ToString());
            //MessageBox.Show(b1.chess[p2[0], p2[1]].ToString());


            //g.DrawString(" ", this.font, bush, (p1[0] - 1) * 25, (p1[1] - 1) * 25);
            //g.DrawString(" ", this.font, bush, (p2[0] - 1) * 25, (p2[1] - 1) * 25);
            //更新花在画板上的两个坐标上的字符串
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
            form3.Dispose();
            if (!form3.getInto())
            {
                this.Dispose();
            }
            else
            {
                //设置email
                this.email = form3.email;
                //初始化用户显示
                this.Text = "欢迎："+this.judge.Who(this.email);


                // MessageBox.Show("登入成功");
            }
            //登入界面进入完成，接下来就是进入模式选择页面
            Form5 form5 = new Form5();
            form5.ShowDialog();
            form5.Dispose();

            this.time = form5.time;
            //设置时间
            this.mode = form5.mode;
            //设置模式

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("退出不计分哦！");
            this.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //开始
            b1.Init();
            this.DrawChess();
            //刻画棋盘
            
            time_Set();
            //设置时间
        }


        private void label1_Click(object sender, EventArgs e)
        { 

        }
        
        //t为毫秒数
        private void time_Set()
        {
            timer1.Interval = this.time;
            //以time为桥梁

            //每一秒(1000毫秒)显示一次
            clock.Interval = 1000;
            clock.Tick += new EventHandler(show_Label);
            clock.Start();
            SetJudge();
        }

        private void show_Label(object source, EventArgs e)

        {
            if(time<60)
                label1.Text = "剩余时间：" + (time/1000).ToString()+" 秒";
            else
                label1.Text = "剩余时间：" + ((time / 1000) /60).ToString()
                    + " 分 "+((time/1000)%60).ToString() + " 秒";

            if (timer1.Interval == this.time)
            {
                timer1.Start();
                //初次启动
            }

            time = time - 1000;
            if (this.time < 0)
            {
                clock.Stop();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            //停止倒计时

            b1.Init();
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);

            MessageBox.Show("你输了哦！");
            this.Dispose();

        }

        private void SetJudge()
        {
            judge_clock.Interval = 100;//每100毫秒检查一次

            judge_clock.Tick += new EventHandler(Status);

            judge_clock.Start();
            
        }

        private void Status(object source, EventArgs e)
        {
            int i = judge.StatusGame(this.time, !this.b1.start);//非start就说明没启动
            if (i == 1)
            {
                //说明赢了
                if (!this.judge.OverGame(new games
                {
                    分数 = this.score,
                    模式 = this.mode.Trim(),
                    邮箱 = this.email.Trim()
                }))
                    MessageBox.Show("数据库更新失败");
            }
            else if (i == 2)
            {
                //说明输了
                MessageBox.Show("你输了哦！");
                this.Dispose();
            }
            else
            {
                //i==0
                //说明情况未定
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //分数
        }
    }
}
