using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace delegate_test
{
    public partial class Form1 : Form
    {
        Thread othread;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SysHelper._textbox = textBox1;
            othread = new Thread(SysHelper.Run);
            othread.Start();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            othread.Abort();
        }
    }
    //無限運算，被除數被1000整除時回傳被除數。
    class SysHelper
    {
        public static TextBox _textbox;
        //呼叫其他執行緒要透過委派，宣告Delegate和參數，要與被呼叫的方法一樣，包含回傳型別
        delegate void PrintHandler(TextBox tb, string text);
        public static void Run()
        {
            int x = 0;
            while (true)
            {
                if (x % 1 == 0)
                {
                    //當數字可被整除時，要更新其他執行緒的控制項
                    Print(_textbox, x.ToString());
                }
                x++;
            }
        }
        public static void Print(TextBox tb, string text)
        {
            //判斷這個TextBox物件是否在同一個執行緒上
            if (tb.InvokeRequired)
            {
                //invokeRequired為true時，表示在不同執行緒上
                //實體化Delegate(new)，要指定方法
                PrintHandler ph = new PrintHandler(Print);
                //呼叫Print方法
                tb.Invoke(ph, tb, text);
            }
            //建立完Delegate並invoke後，invokeRequire傳回false，表示在藤一個執行緒上，可以正常呼叫到這個TextBox物件
            else
            {
                tb.Text = tb.Text + text + Environment.NewLine;
            }
        }
    }
}
