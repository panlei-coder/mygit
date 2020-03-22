using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 记事本窗口
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public int start = 0;
        public RichTextBox richText;

        public Form3(RichTextBox rtb)
        {
            richText = rtb;
        }

        //查找下一个
        private void button1_Click(object sender, EventArgs e)
        {
            string str1;    //存放要查找的文本
            str1 = textBox1.Text.Trim();
            richText.SelectionColor = Color.Blue;
            start = richText.Find(str1, start, RichTextBoxFinds.MatchCase);  //查找下一个
            if (start == -1)
            {
                MessageBox.Show("已查找到文档的结尾", "查找结束对话框",
                    MessageBoxButtons.OK);
                start = 0;
            }
            else
            {
                start = start + str1.Length;
            }
            richText.SelectionColor = Color.Red;
            richText.Focus();
        }

        //替换
        private void button2_Click(object sender, EventArgs e)
        {
            string str1, str2;
            str1 = textBox1.Text;
            str2 = textBox2.Text;
            richText.SelectionColor = Color.Blue;

            if (start < richText.Text.Length)
            {
                if (this.checkBox1.Checked) //区分大小写
                    start = richText.Find(str1, start, RichTextBoxFinds.MatchCase);
                else     //不区分大小写
                    start = richText.Find(str1, start, RichTextBoxFinds.None);
            }
            else
                start = -1;

            if (start == -1)
            {
                MessageBox.Show("已查找到文档的结尾", "替换结束对话框",
                    MessageBoxButtons.OK);
            }
            else
            {
                start = start + str1.Length;
                richText.SelectedText = str2;
            }

            richText.SelectionColor = Color.Red;
            richText.Focus();
        }

        //全部替换
        private void button3_Click(object sender, EventArgs e)
        {
            string str1, str2;
            str1 = textBox1.Text;
            str2 = textBox2.Text;
            richText.SelectionColor = Color.Blue;

            if (this.checkBox1.Checked)  //区分大小写
            {
                start = richText.Find(str1, 0, RichTextBoxFinds.MatchCase);

                while (start != -1)
                {
                    richText.SelectedText = str2;
                    start += str2.Length;
                    start = richText.Find(str1, start, RichTextBoxFinds.MatchCase);
                }
            }
            else     //不区分大小写
            {
                start = richText.Find(str1, 0, RichTextBoxFinds.None);

                while (start != -1)
                {
                    richText.SelectedText = str2;
                    start += str2.Length;
                    start = richText.Find(str1, start, RichTextBoxFinds.None);
                }
            }

            MessageBox.Show("已查找到文档的结尾", "替换结束对话框", MessageBoxButtons.OK);
            start = 0;
            richText.Focus();
        }

        //取消
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();   //关闭窗口
        }
    }
}
