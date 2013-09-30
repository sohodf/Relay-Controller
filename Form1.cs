using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RelayController
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] coms = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string com in coms)
            {
                comboBox1.Items.Add(com);
            }
            comboBox1.SelectedIndex = 0;

        }
    }
}
