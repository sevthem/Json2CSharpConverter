using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Json2CSharpLib;

namespace codeGenerator
{
    public partial class Json2CSharpConverter : Form
    {
        public Json2CSharpConverter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            outputTextBox.Text = JsonCSharpConvertion.Convert(richTextBox1.Text);
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            //var inputLines = inputTextBox.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
            //var builder = new StringBuilder();
            //inputLines.ForEach(x => builder.AppendLine(x));
            //inputTextBox.Text = builder.ToString();
        }
    }
}
