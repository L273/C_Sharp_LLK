using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class GoBang
    {
        public char[,] chess = new char[26, 26];
        //26*26的棋盘，外一圈为检测圈，内24*24为落子处

        public bool start = false;
        //判断是否初始化棋盘，如果初始化了棋盘，则为true，反之则为false；

        private bool[] exist4 = new bool[4];
        //四个方向是否存在字的布尔数组，其中的排列为：上、下、左、右。四个boolean值。
        
        //打乱一个长度为25的数组，通过生成随机交换下标
        private int[] RandomArr(int[] temp)
        {
            Random r = new Random();
            int index, value;
            for (int i = 1; i < 25; i++)
            {
                index = r.Next(1, 25);//得到一个交换的随机序列
                value = temp[index];
                temp[index] = temp[i];
                temp[i] = value;
            }
            return temp;
        }

        //初始化一个24*24的连连看棋盘
        public void Init()
        {
            int start = 0x41;
            int end = 0x5a;
            Random r = new Random();//用于生成随机的ASCII码，范围介于start和end
            char temp;

            int[] x = new int[26];//两组坐标
            int[] y = new int[26];//两组坐标

            int[,,] postion = new int[24, 24, 2]; //填充随机坐标值，24*24的容量，2的宽度

            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    //棋盘的初始化，0代表这个地方没有填入数值
                    this.chess[i, j] = ' ';
                }
            }

            for (int i = 1; i < 25; i++)
            {
                x[i] = i;
                y[i] = i;
            }

            x = this.RandomArr(x);//传入数组地址，打乱数组后返回
            y = this.RandomArr(y);


            //填充随机坐标，以便之后填入字母
            //由于四边的的坐标是判断行列
            //所以，这里的操作，应该局限于1~24之间的序列

            for (int i = 1; i < 25; i++)
            {
                for (int j = 1; j < 25; j = j + 2)
                {
                    temp = (char)r.Next(start, end);//在打乱坐标内依次填入两个字母
                    this.chess[x[i], y[j]] = temp;
                    this.chess[x[i], y[j + 1]] = temp;
                }
                y = this.RandomArr(y);//完成一次以后再打乱纵坐标，以便下一行的重排有新的秩序
            }

            for (int i = 0; i < 4; i++)
            {
                this.exist4[i] = true;
                //初始化方向判断的四个向量，默认四个都有子
            }

            this.start = true;

            /*
            chess[1, 3] = 'S';
            chess[2, 3] = ' ';
            chess[3, 3] = ' ';
            chess[3, 4] = ' ';
            chess[4, 4] = 'S';
            */
        }

        private void FlushExist4(int[] p)
        {
            //刷新exist4的值，通过检查chess和传入的p点。
            //p的值:0、1分别是x，y坐标

            //棋子上方
            if (this.chess[p[0] - 1, p[1]] != ' ')
            {
                this.exist4[0] = true;
                //存在棋子
            }
            else
            {
                this.exist4[0] = false;
                //不存在棋子
            }

            //棋子下方
            if (this.chess[p[0] + 1, p[1]] != ' ')
            {
                this.exist4[1] = true;
                //存在棋子
            }
            else
            {
                this.exist4[1] = false;
                //不存在棋子
            }

            //棋子左侧
            if (this.chess[p[0], p[1] - 1] != ' ')
            {
                this.exist4[2] = true;
                //存在棋子
            }
            else
            {
                this.exist4[2] = false;
                //不存在棋子
            }

            //棋子右侧
            if (this.chess[p[0], p[1] + 1] != ' ')
            {
                this.exist4[3] = true;
                //存在棋子
            }
            else
            {
                this.exist4[3] = false;
                //不存在棋子
            }
        }

        //两个不同方向找墙，用多态实现

        //同向
        private bool ExistWall(int[] p1, int[] p2, int vector)
        {
            //放回false说明存在墙，返回true说明不存在墙
            //vector 0：向上   1：向下    3：向右    2：向左
            int[] temp_1 = new int[2];  //操作p1
            int[] temp_2 = new int[2];  //操作p2
            //两个临时操作的坐标
            if (vector == 0)
            {
                if (p1[0] < p2[0])
                {
                    //p1在p2的上方
                    temp_1[0] = p1[0];
                    temp_1[1] = p1[1];

                    //首先判断p2能不能接上p1所在的行
                    temp_2[0] = p1[0];
                    temp_2[1] = p2[1];

                    if (!this.LieCheck(temp_2, p2, 2))
                        return true;
                    //不能接上就说明p2不可能再往上生长

                    //同方向检查，向上生长，左右检查
                    for (int i = temp_2[0]; i > 0; i--)
                    {
                        if (this.LieCheck(temp_1, temp_2, 1))
                            return false;
                        //找到一条连接线

                        //为下一次循环准备坐标
                        temp_1[0]--;
                        temp_2[0]--;

                        //判断循环是否还有价值进行的条件
                        if (!this.LieCheck(temp_1, p1, 2))
                            return true;
                        if (!this.LieCheck(temp_2, p2, 2))
                            return true;
                        //两个无法生长的情况，直接退出循环，返回无法连接的结果
                    }

                    //已经检查到最外层，说明两个方向可以连接
                    return false;
                }
                else
                {
                    //非同行同列情况，反之就是p2在p1的上方
                    temp_2[0] = p2[0];
                    temp_2[1] = p2[1];

                    //首先判断p1能不能接上p2所在的行
                    temp_1[0] = p2[0];
                    temp_1[1] = p1[1];

                    if (!this.LieCheck(temp_1, p1, 2))
                        return true;
                    //不能接上就说明p1不可能再往上生长

                    //同方向检查，向上生长，左右检查
                    for (int i = temp_1[0]; i > 0; i--)
                    {
                        if (this.LieCheck(temp_1, temp_2, 1))
                            return false;
                        //找到一条连接线

                        //为下一次循环准备坐标
                        temp_2[0]--;
                        temp_1[0]--;

                        //判断循环是否还有价值进行的条件
                        if (!this.LieCheck(temp_1, p1, 2))
                            return true;
                        if (!this.LieCheck(temp_2, p2, 2))
                            return true;
                        //两个无法生长的情况，直接退出循环，返回无法连接的结果

                    }

                    //已经检查到最外层，说明两个方向可以连接
                    return false;
                }
            }
            else if (vector == 1)
            {
                if (p1[0] < p2[0])
                {
                    //p2在p1的下方
                    temp_2[0] = p2[0];
                    temp_2[1] = p2[1];

                    //首先判断p1能不能接上p2所在的行
                    temp_1[0] = p2[0];
                    temp_1[1] = p1[1];

                    if (!this.LieCheck(temp_1, p1, 2))
                        return true;
                    //不能接上就说明p1不可能再往下生长

                    //同方向检查，向下生长，左右检查
                    for (int i = temp_1[0]; i < 26; i++)
                    {
                        if (this.LieCheck(temp_1, temp_2, 1))
                            return false;
                        //找到一条连接线

                        //为下一次循环准备坐标
                        temp_2[0]++;
                        temp_1[0]++;

                        //判断循环是否还有价值进行的条件
                        if (!this.LieCheck(temp_1, p1, 2))
                            return true;
                        if (!this.LieCheck(temp_2, p2, 2))
                            return true;
                        //两个无法生长的情况，直接退出循环，返回无法连接的结果         
                    }
                    //已经检查到最外层，说明两个方向可以连接
                    return false;
                }
                else
                {
                    //p1在p2的下方
                    temp_1[0] = p1[0];
                    temp_1[1] = p1[1];

                    //首先判断p2能不能接上p1所在的行
                    temp_2[0] = p1[0];
                    temp_2[1] = p2[1];

                    if (!this.LieCheck(temp_2, p2, 2))
                        return true;
                    //不能接上就说明p2不可能再往下生长

                    //同方向检查，向下生长，左右检查
                    for (int i = temp_2[0]; i < 26; i++)
                    {
                        if (this.LieCheck(temp_1, temp_2, 1))
                            return false;
                        //找到一条连接线

                        //为下一次循环准备坐标
                        temp_1[0]++;
                        temp_2[0]++;

                        //判断循环是否还有价值进行的条件
                        if (!this.LieCheck(temp_1, p1, 2))
                            return true;
                        if (!this.LieCheck(temp_2, p2, 2))
                            return true;
                        //两个无法生长的情况，直接退出循环，返回无法连接的结果

                    }

                    //已经检查到最外层，说明两个方向可以连接
                    return false;
                }
            }
            else if (vector == 3)
            {
                if (p1[1] < p2[1])
                {
                    //p2在p1的右侧
                    temp_2[0] = p2[0];
                    temp_2[1] = p2[1];

                    //首先，判断p1能不能接上p2所在的列
                    temp_1[0] = p1[0];
                    temp_1[1] = p2[1];

                    if (!this.LieCheck(p1, temp_1, 1))
                        return true;
                    //不能接上，说明两个不可能相连

                    for (int i = temp_1[1]; i < 26; i++)
                    {
                        if (this.LieCheck(temp_1, temp_2, 2))
                            return false;
                        //找到一条线，能够相连。

                        //为下一次循环准备坐标
                        temp_1[1]++;
                        temp_2[1]++;

                        if (!this.LieCheck(p1, temp_1, 1))
                            return true;
                        if (!this.LieCheck(p2, temp_2, 1))
                            return true;
                        //若无法生长，则说明下一次循环无法进行
                    }

                    return false;//此时已经生长到最外围
                }
                else
                {
                    //p1在p2的右侧
                    temp_1[0] = p1[0];
                    temp_1[1] = p1[1];

                    //首先，判断p2能不能接上p1所在的列
                    temp_2[0] = p2[0];
                    temp_2[1] = p1[1];

                    if (!this.LieCheck(p2, temp_2, 1))
                        return true;
                    //不能接上，说明两个不可能相连

                    for (int i = temp_2[1]; i < 26; i++)
                    {
                        if (this.LieCheck(temp_1, temp_2, 2))
                            return false;
                        //找到一条线，能够相连。

                        //为下一次循环准备坐标
                        temp_1[1]++;
                        temp_2[1]++;

                        if (!this.LieCheck(p1, temp_1, 1))
                            return true;
                        if (!this.LieCheck(p2, temp_2, 1))
                            return true;
                        //若无法生长，则说明下一次循环无法进行
                    }
                    return false;//此时已经生长到最外围
                }
            }
            else if (vector == 2)
            {

                if (p1[1] < p2[1])
                {
                    //p1在p2的左侧
                    temp_1[0] = p1[0];
                    temp_1[1] = p1[1];

                    //首先，判断p2能不能接上p1所在的列
                    temp_2[0] = p2[0];
                    temp_2[1] = p1[1];

                    if (!this.LieCheck(p2, temp_2, 1))
                        return true;
                    //不能接上，说明两个不可能相连

                    for (int i = temp_2[1]; i > 0; i--)
                    {
                        if (this.LieCheck(temp_1, temp_2, 2))
                            return false;
                        //找到一条线，能够相连。

                        //为下一次循环准备坐标
                        temp_1[1]--;
                        temp_2[1]--;

                        if (!this.LieCheck(p1, temp_1, 1))
                            return true;
                        if (!this.LieCheck(p2, temp_2, 1))
                            return true;
                        //若无法生长，则说明下一次循环无法进行
                    }
                    return false;//此时已经生长到最外围
                }
                else
                {

                    //p2在p1的左侧
                    temp_2[0] = p2[0];
                    temp_2[1] = p2[1];

                    //首先，判断p1能不能接上p2所在的列
                    temp_1[0] = p1[0];
                    temp_1[1] = p2[1];


                    if (!this.LieCheck(p1, temp_1, 1))
                        return true;
                    //不能接上，说明两个不可能相连

                    for (int i = temp_1[1]; i > 0; i--)
                    {
                        if (this.LieCheck(temp_1, temp_2, 2))
                            return false;
                        //找到一条线，能够相连。

                        //为下一次循环准备坐标
                        temp_1[1]--;
                        temp_2[1]--;

                        if (!this.LieCheck(p1, temp_1, 1))
                            return true;
                        if (!this.LieCheck(p2, temp_2, 1))
                            return true;
                        //若无法生长，则说明下一次循环无法进行
                    }
                    return false;//此时已经生长到最外围
                }
            }

            return true; //输入的参数有误的时候，即vector输入有误，直接返回true，有墙
        }

        //反向
        private bool ExistWall(int[] p1, int[] p2, int p1_vector, int p2_vector)
        {
            //放回false说明存在墙，返回true说明不存在墙
            //vector 1：向上   2：向右    3：向下    4：向左

            int[] temp_1 = new int[2];
            int[] temp_2 = new int[2];
            //两个操作数
            temp_1[0] = p1[0];
            temp_1[1] = p1[1];

            temp_2[0] = p2[0];
            temp_2[1] = p2[1];

            if (p1_vector % 2 == p2_vector % 2)
            {
                //说明存在方向相对的向量，即左右相对，或者上下相对

                if (p1_vector % 2 == 0)
                {
                    //左右相对
                    if (p1[1] < p2[1])
                    {
                        //p1在p2的左侧，所以temp_2左移
                        temp_2[1] = p1[1];

                        //向左生长
                        for (int i = p1[1]; i <= p2[1]; i++, temp_1[1]++, temp_2[1]++)//右移处理
                        {
                            //两个执行前提
                            if (!this.LieCheck(p1, temp_1, 1))
                                continue;
                            if (!this.LieCheck(p2, temp_2, 1))
                                continue;

                            //判断成立条件，两个操作节点不能有值
                            if (this.chess[temp_1[0], temp_1[1]] != ' ')
                                continue;
                            if (this.chess[temp_2[0], temp_2[1]] != ' ')
                                continue;

                            if (this.LieCheck(temp_1, temp_2, 2))
                                return false;
                            //如果p2可以连接temp_2，以及temp_1可以连接temp_2，那就说明对向可连
                        }
                    }
                    else
                    {
                        //p2在p1的左侧，所以temp_1左移
                        temp_1[1] = p2[1];

                        //向左生长
                        for (int i = p2[1]; i <= p1[1]; i++, temp_1[1]++, temp_2[1]++)//右移处理
                        {
                            //两个执行前提
                            if (!this.LieCheck(p1, temp_1, 1))
                                continue;
                            if (!this.LieCheck(p2, temp_2, 1))
                                continue;

                            //判断成立条件，两个操作节点不能有值
                            if (this.chess[temp_1[0], temp_1[1]] != ' ')
                                continue;
                            if (this.chess[temp_2[0], temp_2[1]] != ' ')
                                continue;

                            if (this.LieCheck(temp_1, temp_2, 2))
                                return false;
                            //如果p2可以连接temp_2，以及temp_1可以连接temp_2，那就说明对向可连
                        }
                    }
                }

                if (p1_vector % 2 == 1)
                {
                    //上下相对
                    if (p1[0] < p2[0])
                    {
                        //p1在p2的上方，所以temp_2上移
                        temp_2[0] = p1[0];

                        for (int i = p1[0]; i <= p2[0]; i++,temp_1[0]++, temp_2[0]++)//上移处理
                        {
                            //两个执行前提
                            if (!this.LieCheck(p1, temp_1, 2))
                                continue;
                            if (!this.LieCheck(p2, temp_2, 2))
                                continue;

                            //判断成立条件，两个操作节点不能有值
                            if (this.chess[temp_1[0], temp_1[1]] != ' ')
                                continue;
                            if (this.chess[temp_2[0], temp_2[1]] != ' ')
                                continue;

                            if (this.LieCheck(temp_1, temp_2, 1))
                                return false;
                            //如果p2可以连接temp_2，以及temp_1可以连接temp_2，那就说明对向可连

                            temp_1[0]++;
                            temp_2[0]++;
                        }
                    }
                    else
                    {
                        //p2在p1的上方，所以temp_1上移
                        temp_1[0] = p2[0];

                        for (int i = p2[0]; i <= p1[0]; i++,temp_1[0]++, temp_2[0]++)//上移处理
                        {
                            //两个执行前提
                            if (!this.LieCheck(p1, temp_1, 2))
                                continue;
                            if (!this.LieCheck(p2, temp_2, 2))
                                continue;

                            //判断成立条件，两个操作节点不能有值
                            if (this.chess[temp_1[0], temp_1[1]] != ' ')
                                continue;
                            if (this.chess[temp_2[0], temp_2[1]] != ' ')
                                continue;

                            if (this.LieCheck(temp_1, temp_2, 1))
                                return false;
                            //如果p2可以连接temp_2，以及temp_1可以连接temp_2，那就说明对向可连

                            temp_1[0]++;
                            temp_2[0]++;
                        }
                    }
                }
            }
            return true; // 传入错误参数，默认返回有墙
        }

        private bool Corner(int[] p1, int[] p2)
        {
            int[] temp_p = new int[2];
            
            //temp移至p1同层
            temp_p[0] = p1[0];
            temp_p[1] = p2[1];
            //转角坐标不能存在值
            if (this.LieCheck(p1, temp_p, 1) && this.LieCheck(p2, temp_p, 2))
                if (this.chess[temp_p[0], temp_p[1]] == ' ')
                    return true;
            //连接两条线，转角可连接。

            //temp移至p2同层
            temp_p[0] = p2[0];
            temp_p[1] = p1[1];
            //转角坐标不能存在值
            if (this.LieCheck(p1, temp_p, 2) && this.LieCheck(p2, temp_p, 1))
                if (this.chess[temp_p[0], temp_p[1]] == ' ')
                    return true;
            //连接两条线，转角可连接。

            //未通过上述检查，返回false
            return false;
        }


        private bool LieCheck(int[] p1, int[] p2, int vector)
        {
            //vector 1：左右连线检查 2：上下连线检查
            //reutrn true代表两个坐标可以相连，false代表无法相连
            //由于用vector 确定方向，p1和p2在判断的时候不会存在同列或同行的情况。

            if (vector == 1)
            {
                if (p1[1] < p2[1])
                {
                    //p1点位于p2左侧
                    for (int i = p1[1] + 1; i < p2[1]; i++)
                    {
                        if (this.chess[p1[0], i] != ' ')
                            return false;
                        //有子，直接返回false
                    }
                    //检查无误，可以连接，返回true
                    return true;
                }
                else
                {
                    //p1点位于p2右侧
                    for (int i = p2[1] + 1; i < p1[1]; i++)
                    {
                        if (this.chess[p2[0], i] != ' ')
                            return false;
                        //有子，直接返回false
                    }
                    //检查无误，可以连接，返回true
                    return true;
                    //反之右侧
                }
            }
            else if (vector == 2)
            {

                //上下连线的情况
                if (p1[0] < p2[0])
                {

                    //p1点位于p2上方
                    for (int i = p1[0] + 1; i < p2[0]; i++)
                    {
                        if (this.chess[i, p1[1]] != ' ')
                            return false;
                        //有子，无法连线，直接返回false
                    }
                    return true;
                    //检查无误，直接返回true
                }
                else
                {
                    for (int i = p2[0] + 1; i < p1[0]; i++)
                    {
                        if (this.chess[i, p2[1]] != ' ')
                            return false;
                        //有子，无法连线，直接返回false
                    }
                    return true;
                    //检查无误，直接返回true
                }
            }
            return false; //传入参数有误的时候，即vector 有误的时候
        }

        private bool Check(int[] p1, int[] p2)
        {
            //检查传入的两个坐标是否可以消除
            //返回true代表可以消除，返回false代表不可以消除
            //0是x坐标，1是y坐标

            //首先判断两个坐标内放入的棋子内容是否相同
            if (this.chess[p1[0], p1[1]] != this.chess[p2[0], p2[1]])
                return false;

            //如果是一个坐标，不做处理
            if (p1[0] == p2[0] && p1[1] == p2[1])
                return false;


            bool[] p1_vector = new bool[4];
            bool[] p2_vector = new bool[4];
            //两个子的方向向量，如果有对应的向量平行，则说明两个字可消除

            this.FlushExist4(p1);
            for (int i = 0; i < 4; i++)
            {
                //更新第一个子的四个向量
                p1_vector[i] = this.exist4[i];
            }


            this.FlushExist4(p2);
            for (int i = 0; i < 4; i++)
            {
                //更新第二个子的四个向量
                p2_vector[i] = this.exist4[i];
            }

            //情况1：同列或者同行，单线情况处理
            if (p1[0] == p2[0])
            {
                //同行
                if (p1[1] == p2[1] + 1 || p1[1] == p2[1] - 1)
                    return true;
                //差一列的相邻，即相邻情况

                //检查左右连接
                if (this.LieCheck(p1, p2, 1))
                    return true;
                //左右可以相连，即可消除
            }
            else if (p1[1] == p2[1])
            {
                //同列
                if (p1[0] == p2[0] + 1 || p1[0] == p2[0] - 1)
                    return true;
                //差一行相邻，即相邻情况

                //检查上下相连
                if (this.LieCheck(p1, p2, 2))
                    return true;
                //上下可以相连，即可消除
            }

            //情况2：两个向量同方向
            for (int i = 0; i < 4; i++)
            {
                //比较四个方向向量
                if (p1_vector[i] == false && p2_vector[i] == false)
                {
                    if (!this.ExistWall(p1, p2, i))
                        return true;
                    //存在一个方向，两个子没有阻塞的棋子,且两个向量之间没有墙存在
                }
            }

            //情况3：两个向量方向相反

            //传入参数，1、向上 3、向下
            //2、向右 4、向左

            //左右对向
            if (p1_vector[0] == false && p2_vector[1] == false)
            {
                if (!this.ExistWall(p1, p2, 1, 3))
                    return true;
            }
            else if (p1_vector[1] == false && p2_vector[0] == false)
            {
                if (!this.ExistWall(p1, p2, 3, 1))
                    return true;
            }

            //上下对向
            if (p1_vector[2] == false && p2_vector[3] == false)
            {
                if (!this.ExistWall(p1, p2, 4, 2))
                    return true;
            }
            else if (p1_vector[3] == false && p2_vector[2] == false)
            {
                if (!this.ExistWall(p1, p2, 2, 4))
                    return true;
            }

            //情况4：折角转弯
            if (this.Corner(p1, p2))
                return true;
            //如果通过，就说明两个点有折角转弯连线
            
            return false;
            //不满足三种可消除情况，返回false
        }

        public bool FlushChess(int[] p1, int[] p2)
        {
            //刷新棋盘
            if (this.Check(p1, p2))
            {
                this.chess[p1[0], p1[1]] = ' ';
                this.chess[p2[0], p2[1]] = ' ';
                return true;
            }
            return false;
        }

        public void IfOver()
        {
            foreach(char i in chess)
            {
                if (i != ' ')
                {
                    this.start = true;
                    return ;
                }       
            }
            //没通过结束检查
            this.start = false;
        }
    }
}
