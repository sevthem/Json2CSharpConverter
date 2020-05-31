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
            button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            outputTextBox.Text = JsonCSharpConvertion.Convert(richTextBox1.Text);
          
        }

       
    }
}
