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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        public RichTextBox RichText;
        public string str;
        public Form4(RichTextBox rtb)
        {
            RichText = rtb;
        }

        internal void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            RichText.SelectedText = str;
        }

        internal void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
