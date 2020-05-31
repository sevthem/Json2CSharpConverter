using Json2CSharpLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Json2CSarpConverterCore
{
    public partial class Json2CSarpConverterCore : Form
    {
        public Json2CSarpConverterCore()
        {
            InitializeComponent();
        }

        private void Convert_Click(object sender, EventArgs e)
        {
            try
            {
                outputTextBox.Text = JsonCSharpConvertion.ConvertCore(richTextBox1.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Not a valid json", "Json2SCharp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
