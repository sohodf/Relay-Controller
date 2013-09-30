using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;


namespace RelayController
{
    public partial class Relay : Form
    {
        //global - holds the active serial port name
        string activePort = null;
        //global - active port
        SerialPort sPort = null;
        //global relay status
        string relayStatus = null;

        public Relay()
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            activePort = comboBox1.GetItemText(comboBox1.SelectedItem);
            sPort = SerialActions.OpenCom(activePort);
            if (sPort.IsOpen)
            {
                byte[] getState= {1,0,1,0,1,1};
                sPort.Write(getState, 0, 1);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        public void OnSerialDataReceived(object sender,
                                        SerialDataReceivedEventArgs args)
        {
            relayStatus = sPort.ReadExisting();
            string wait;
        }



    }
}
