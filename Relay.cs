using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections;


namespace RelayController
{
    public partial class Relay : Form
    {
        //global - holds the active serial port name
        string activePort = null;
        //global - active port
        SerialPort sPort = null;
        //global relay status
        byte[] relayStatus = null;

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

        public SerialPort OpenCom(string com)
        {
            SerialPort Port = new SerialPort();
            Port.PortName = com;
            Port.BaudRate = 9600;

            Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            // Set the read/write timeouts
            Port.ReadTimeout = 500;
            Port.WriteTimeout = 500;
            try
            {
                Port.Open();
                return Port;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            activePort = comboBox1.GetItemText(comboBox1.SelectedItem);
            sPort = OpenCom(activePort);
            if (sPort.IsOpen)
            {
                byte[] getState= {1,0,1,0,1,1};
                sPort.Write(getState, 0, 1);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            SendByte(92);
            
            SendByte(100);
            
            

        }

        public void DataReceivedHandler(object sender,
                                        SerialDataReceivedEventArgs args)
        {
            SerialPort sp = (SerialPort)sender;
            relayStatus = GetBytes(sp.ReadExisting());
            BitArray bits = new BitArray(relayStatus);
            bool[] status = new bool[8];
            for (int i=0; i<8; i++)
            {
                status[i] = bits.Get(i);
            }

            if (status[0] == true)
                label2.Text = "On";
            else
                label2.Text = "Off";
            
        }

        public void SendByte(byte toSend)
        {
            byte[] command = new byte[1];
            command[0] = toSend;
            sPort.Write(command, 0, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendByte(92);
            SendByte(100);
            GetRelayState();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendByte(92);
            SendByte(110);
            GetRelayState();
        }

        public void GetRelayState()
        {
            SendByte(91);
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


    }
}
