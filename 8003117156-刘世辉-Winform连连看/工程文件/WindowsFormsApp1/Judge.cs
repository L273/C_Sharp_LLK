using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Judge
    {
        private LLKDataContext llk = new LLKDataContext();
        public bool OverGame(games g1)
        {
            try
            { 
                //首先更新游戏数据库内的信息
                llk.games.InsertOnSubmit(g1);
                //提交数据
                llk.SubmitChanges();
                //修改


                //其次再更新user 内的盘数
                var result = from r in llk.users
                        where g1.邮箱 == r.邮箱
                        select r;

                foreach (var i in result)
                {
                    i.盘数 = i.盘数 + 1;
                    //加一盘
                    //由于邮箱是主键，这个使用foreach，如果查到，也只会有一个

                    //查看是否超越最高分
                    if (i.最高分 < g1.分数)
                    {
                        i.最高分 = g1.分数;
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public int StatusGame(int time,bool over)
        {
            //返回0，表示未定
            //返回1，表示赢了
            //返回2，表示输了

            if (time <= 0)
            {
                return 2;
            }
            else
            {
                //over的值是GoBang棋盘状态的判断函数
                //如果over=false，说明棋盘全部为0。反之如果为true，说明还有子
                if (over)
                    return 1;
            }
            return 0;
        }

        public string Who(string email)
        {
            var result = from r in llk.users
                         where r.邮箱 == email
                         select r;
            foreach(var i in result)
            {
                return i.用户名;
            }
            return null;
        }
    }

}
