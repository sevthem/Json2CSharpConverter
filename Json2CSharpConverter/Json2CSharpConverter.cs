﻿using System;
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
            outputTextBox.Text = JsonCSharpConvertion.Convert(inputTextBox.Text);
        }
    }
}