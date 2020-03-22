using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 记事本窗口
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //this.MainMenuStrip = menuStrip2;
        }


        private static string openfilepath = "";  //保存所打开文件的路径
        IDataObject iData = Clipboard.GetDataObject();  //剪切或复制的内容会被保存到该字段中
        //代开子菜单时间用于打开文件文件
        private void 打开OCtrlOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*"; //设置文件类型
            openFileDialog1.FilterIndex = 1; //设置默认文件类型的显示顺序
            openFileDialog1.RestoreDirectory = true;  //打开对话框是否记忆上次打开的目录
            StreamReader sr = null;    //定义StreamReader对象
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    openfilepath = openFileDialog1.FileName;   //获取打开的文件路径
                    string name = openfilepath.Substring(openfilepath.LastIndexOf("\\") + 1);
                    this.Text = name;   //文件名作为标题
                    sr = new StreamReader(openfilepath, Encoding.Default);  //实例化sr
                    richTextBox1.Text = sr.ReadToEnd();    //读取所有文件内容
                }
                catch
                {
                    MessageBox.Show("打开文件时出错。", "错误",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                finally 
                {
                    if (sr != null)
                    {
                        sr.Close();  //关闭对象sr
                        sr.Dispose();  //释放对象sr资源
                    }
                }
            }
        }

        //新建子菜单事件用于新建文件文件
        private void 新建NCtrlNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Modified)
            {
                //提示保存对话框
                DialogResult dResult = MessageBox.Show("文件" + this.Text + "的内容已改变,需要保存吗？",
                    "保存文件", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (dResult)
                {
                    case DialogResult.Yes:
                        另存为LCtrlLToolStripMenuItem_Click(null, null);
                        richTextBox1.Clear();
                        this.Text = "无标题-记事本";
                        break;
                    case DialogResult.No:
                        richTextBox1.Clear();
                        this.Text = "无标题-记事本";
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
            else 
            {
                richTextBox1.Clear();
                this.Text = "无标题-记事本";
                richTextBox1.Modified = false;
            }

        }


        //另存为菜单时间用于将文件另存到电脑中
        private void 另存为LCtrlLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "文本文件(*.txt)|*txt|所有文件(*.*)|*.*";    //设置文件类型
            saveFileDialog1.FilterIndex = 2;                   //设置默认文件类型的显示顺序
            saveFileDialog1.RestoreDirectory = true;            //保存对话框是否记忆上次打开的目录
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openfilepath = saveFileDialog1.FileName.ToString();  //获取文件路径
                FileStream fs = null;
                try
                {
                    fs = File.Create(openfilepath);
                }
                catch 
                {
                    MessageBox.Show("建立文件时出错。", "错误",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Warning);
                }

                byte[] content = Encoding.Default.GetBytes(richTextBox1.Text);
                try
                {
                    fs.Write(content,0,content.Length);
                    fs.Flush();
                    toolStripStatusLabel1.Text = "保存成功";
                }
                catch
                {
                    MessageBox.Show("写入文件时出错。", "错误",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Warning);
                }
                finally
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        //保存子菜单事件用于保存文件文件
        internal void 保存SCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter sw = null;
            if (openfilepath == "")
            {
                另存为LCtrlLToolStripMenuItem_Click(null, null);
                return;
            }

            try
            {
                sw = new StreamWriter(openfilepath, false, Encoding.Default);
                sw.Write(richTextBox1.Text);
                toolStripStatusLabel1.Text = "保存成功";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            finally 
            {
                if (sw != null)
                {
                    sw.Close();       //关闭StreamWriter
                    sw.Dispose();     //释放资源
                }
            }
        }

        //以下子菜单事件分别实现打印、退出等功能
        private void 打印PCtrlLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.ShowDialog();
        }

        private void 退出ECtrlEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void 剪切CtrlXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void 复制CCtrlCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        internal void 粘贴PCtrlVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void 删除LDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
        }

        private void 查找FCtrlFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 ff = new Form2();
            ff.richtextbox = richTextBox1;
            ff.ShowDialog();
        }

        private void 替换RCtrlHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 fc = new Form3();
            fc.richText = richTextBox1;
            fc.ShowDialog();
        }

        private void 全选ACtrlAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        //时间日期子菜单用来在文本后添加当前的时间日期
        private void 时间日期DCtrlTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                richTextBox1.SelectedText = DateTime.Now.Hour.ToString() + ":" +
                    DateTime.Now.Second.ToString() + "" + DateTime.Now.Day.ToString();
            }
            else
            {
                richTextBox1.SelectedText += DateTime.Now.Hour.ToString()+":" +
                    DateTime.Now.Second.ToString() + "" + DateTime.Now.Year.ToString() + "-"
                    + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            }
        }

        //自动换行子菜单用来控制文本是否自动换行
        private void 自动换行wToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.WordWrap == true)
            {
                richTextBox1.WordWrap = false;
                自动换行wToolStripMenuItem.Checked = false;
                richTextBox1.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            }
            else
            {
                richTextBox1.WordWrap = true;
                自动换行wToolStripMenuItem.Checked = true;
                richTextBox1.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            }
        }

        //字体子菜单用来设置所选择的文本字体
        private void 字体FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                Font font = fontDialog1.Font;
                richTextBox1.SelectionFont = font;
            }
        }

        //状态栏子菜单用来设置是否显示状态栏
        private void 状态栏SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (statusStrip1.Visible == true)
            {
                statusStrip1.Visible = false;
                状态栏SToolStripMenuItem.Checked = false;
                richTextBox1.Height += 22;
            }
        }

        private void 关于记事本GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此记事的版本为V1.0!");
        }

        //“新建”图标
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.新建NCtrlNToolStripMenuItem_Click(sender, e);
        }

        //“打开”图标
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.打开OCtrlOToolStripMenuItem_Click(sender, e);
        }

        //“保存”图标
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.保存SCtrlSToolStripMenuItem_Click(sender, e);
        }

        //“剪切”图标
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.剪切CtrlXToolStripMenuItem_Click(sender, e);
        }

        //“复制”图标
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            this.复制CCtrlCToolStripMenuItem_Click(sender, e);
        }

        //“粘贴”图标
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.粘贴PCtrlVToolStripMenuItem_Click(sender, e);
        }

        //“查找”图标
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            this.查找FCtrlFToolStripMenuItem_Click(sender, e);
        }

        //“替换”图标
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            this.替换RCtrlHToolStripMenuItem_Click(sender, e);
        }

        //快捷菜单“撤销”
        private void 撤销UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.撤销ToolStripMenuItem_Click(sender, e);
        }

        //快捷菜单“剪切”
        private void 剪切XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.剪切CtrlXToolStripMenuItem_Click(sender, e);
        }

        //快捷菜单“复制”
        private void 复制CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.复制CCtrlCToolStripMenuItem_Click(sender, e);
        }

        //快捷菜单“粘贴”
        private void 粘贴VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.粘贴PCtrlVToolStripMenuItem_Click(sender, e);
        }

        //快捷菜单“删除”
        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.删除LDeleteToolStripMenuItem_Click(sender, e);
        }
        

        /*下面的三个事件都条用同一个方法place()，用于计算当前鼠标位置并显示在状态栏中*/
        //richTextBox1的KeyUp
        //在释放键时发生事件
        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            this.place();
        }

        //richTextBox1的MouseUp
        //鼠标指针在richTextBox1上方并释放鼠标按钮时发生事件
        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.place();
        }

        //richTextBox1的TextChanged
        //更改文本时引发的事件
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.place();
        }


        private void place()
        {
            string str = this.richTextBox1.Text;
            int m = this.richTextBox1.SelectionStart;
            int Ln = 0;
            int Col = 0;
            for (int i = m - 1; i >= 0; i--)
            {
                if (str[i] == '\n')
                    Ln++;

                if (Ln < 1)
                    Col++;
            }

            Ln = Ln + 1;
            Col = Col + 1;
            toolStripStatusLabel1.Text = "行：" + Ln.ToString() + "," + "列：" + Col.ToString();
        }

        //撤销
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            this.撤销ToolStripMenuItem_Click(sender, e);
        }

        //重做
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }
        
        //Timer控件起监测作用
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.剪切CtrlXToolStripMenuItem.Enabled = (!string.IsNullOrEmpty(this.richTextBox1.SelectedText));
            this.剪切XToolStripMenuItem.Enabled = (!string.IsNullOrEmpty(this.richTextBox1.SelectedText));
            this.复制CCtrlCToolStripMenuItem.Enabled = (!string.IsNullOrEmpty(this.richTextBox1.SelectedText)); 
            this.复制CToolStripMenuItem.Enabled = (!string.IsNullOrEmpty(this.richTextBox1.SelectedText));
            this.删除DToolStripMenuItem.Enabled = (!string.IsNullOrEmpty(this.richTextBox1.SelectedText));
            this.删除LDeleteToolStripMenuItem.Enabled = (!string.IsNullOrEmpty(this.richTextBox1.SelectedText));
        }

        //覆盖
        private void 覆盖CtrlGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.richTextBox1.SelectedText != "")
            {
                Form4 fg = new Form4();
                fg.RichText = richTextBox1;
                //获取剪切或复制被保存到iData中的字符串内容
                fg.str = (String)iData.GetData(DataFormats.Text);  
                fg.ShowDialog();
            }
        }

    }
}
