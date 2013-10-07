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
using System.Globalization;


namespace RelayController
{
    public partial class Relay : Form
    {
        //global - holds the active serial port name
        string activePort = null;
        //global - active port
        SerialPort sPort = null;
        //global relay status
        bool[] status = new bool[8];
        //global - path to files to be flashed
        string[] flsFiles = null;

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
            PowerCombo.SelectedIndex = 7;
            USBCombo.SelectedIndex = 0;
            ResetCombo.SelectedIndex = 1;
            
        }

        public SerialPort OpenCom(string com)
        {
            SerialPort Port = new SerialPort();
            Port.PortName = com;
            Port.BaudRate = 9600;

            //Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

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
            try
            {
                if (sPort.IsOpen)
                {
                    sPort.Close();
                }
            }

            catch (Exception serialPortOpen)
            {
                //do nothing
            }

            activePort = comboBox1.GetItemText(comboBox1.SelectedItem);
            sPort = OpenCom(activePort);
            if (sPort.IsOpen)
            {
                //turn all realys off
                SendCommand(110);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            SendCommand(0x65);
            
        }

        //public void DataReceivedHandler(object sender,
        //                                SerialDataReceivedEventArgs args)
        //{
        //    SerialPort sp = (SerialPort)sender;
        //    byte[] relayStatus = Encoding.Unicode.GetBytes(sp.ReadExisting());


        //    string statusString = hex2binary(relayStatus[0].ToString());
        //    BitArray bits = new BitArray(relayStatus);

        //    for (int i = 0; i < 8; i++)
        //    {
        //        status[i] = bits.Get(i);
        //    }


        //}

        public void SendByte(byte toSend)
        {
            try
            {
                byte[] command = { toSend };
                if (sPort.IsOpen)
                {
                    sPort.Write(command, 0, 1);
                }
                else
                    MessageBox.Show("Port Closed");
            }
            catch (Exception portExce)
            {
                MessageBox.Show(portExce.Message);
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            SendByte(92);
            SendByte(100);
            //UpdateStatus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendByte(92);
            SendByte(110);
            //UpdateStatus();
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

        public void UpdateStatus()
        {
            GetRelayState();

            //if (status[0] == true)
            //    label2.Text = "On";
            //else
            //    label2.Text = "Off";
        }

        private string hex2binary(string hexvalue)
        {
            string binaryval = "";
            binaryval = Convert.ToString(Convert.ToInt32(hexvalue, 16), 2);
            return binaryval;
        }

        public void SendCommand(byte command)
        {
            //SendByte(0x5c);
            SendByte(command);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendCommand(0x6f);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SendCommand(102);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SendCommand(112);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SendCommand(108);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SendCommand(118);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            byte toSend = new byte();
            
            try
            {
                toSend = Byte.Parse(textBox1.Text);
                SendCommand(toSend);
            }
            catch (Exception ByteParse)
            {
                MessageBox.Show(ByteParse.Message);
            }
                
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                int input = int.Parse(textBox1.Text);
                input++;
                textBox1.Text = input.ToString();
            }
            catch (Exception intParse)
            {
                MessageBox.Show(intParse.Message);
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                int input = int.Parse(textBox1.Text);
                input--;
                textBox1.Text = input.ToString();
            }
            catch (Exception intParse)
            {
                MessageBox.Show(intParse.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                int input = int.Parse(textBox1.Text);
                input += 10;
                textBox1.Text = input.ToString();
            }
            catch (Exception intParse)
            {
                MessageBox.Show(intParse.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                int input = int.Parse(textBox1.Text);
                input -= 10;
                textBox1.Text = input.ToString();
            }
            catch (Exception intParse)
            {
                MessageBox.Show(intParse.Message);
            }
        }

        private void Relay_FormClosed(object sender, FormClosedEventArgs e)
        {
            sPort.Close();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            
            if (flsFiles == null)
                listBox1.Items.Add("No fls files selected");
            else
            {
                CMD cli = new CMD(this);
                ArrayList args = new ArrayList();
                args.Add(USBCombo.Text);
                args.Add(PowerCombo.Text);
                args.Add(ResetCombo.Text);
                args.Add(flsFiles);

                if (checkBox1.Checked)
                    backgroundWorker1.RunWorkerAsync(args);
                else
                    listBox1.Items.Add("Not implemented yet");
                    //backgroundWorker2.RunWorkerAsync(args);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ArrayList arguments = e.Argument as ArrayList;
            string[] files = (string[])arguments[3];
            CMD cli = new CMD(this);

            

            this.listBox1.Invoke(new Action(() => { listBox1.Items.Clear(); }));
            this.listBox1.Invoke(new Action(() => {listBox1.Items.Add("Cutting all relays");}));
            System.Threading.Thread.Sleep(500);
            SendCommand(110); //all relays off
            string burnCommand = "downloadtool -cu1 ";
            foreach (string fls in files)
                burnCommand += fls + " ";
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Starting burn process."); }));
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Do not interrupt!"); }));
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Pressing reset");}));
            System.Threading.Thread.Sleep(1000);
            int resetPort = int.Parse((string)arguments[2]) + 100;
            SendCommand((byte)resetPort);
            System.Threading.Thread.Sleep(1000);
            //do the actual burning
            backgroundWorker3.RunWorkerAsync(burnCommand);
            System.Threading.Thread.Sleep(2500);
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("USB on"); }));
            System.Threading.Thread.Sleep(1000);
            int usbPort = int.Parse((string)arguments[0]) + 100;
            SendCommand((byte)usbPort);
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Power On"); }));
            int powerPort = int.Parse((string)arguments[1]) + 100;
            SendCommand((byte)powerPort);

            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Waiting 12 seconds"); }));
            System.Threading.Thread.Sleep(12000);
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Reset released"); }));
            resetPort = int.Parse((string)arguments[2]) + 110;
            SendCommand((byte)resetPort);
            System.Threading.Thread.Sleep(1000);



            //this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("USB on"); }));
            //System.Threading.Thread.Sleep(1000);
            //int usbPort = int.Parse((string)arguments[0]) + 100;
            //SendCommand((byte)usbPort);
            //this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Power On"); }));
            //System.Threading.Thread.Sleep(1000);
            //int powerPort = int.Parse((string)arguments[1]) + 100;
            //SendCommand((byte)powerPort);
            //this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Pressing reset for 12 seconds..."); }));
            //System.Threading.Thread.Sleep(1000);
            //int resetPort = int.Parse((string)arguments[2]) + 100;
            //SendCommand((byte)resetPort);
            //System.Threading.Thread.Sleep(12000);
            //this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Reset complete"); }));

            //this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Resetting"); }));
            //System.Threading.Thread.Sleep(1000);
            //int resetPort = int.Parse((string)arguments[2]) + 100;
            //SendCommand((byte)resetPort);
            //System.Threading.Thread.Sleep(500);
            //int resetPortOff = int.Parse((string)arguments[2]) + 110;
            //SendCommand((byte)resetPortOff);
            //this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Start flashing NOW"); }));
            //System.Threading.Thread.Sleep(2000);
            //this.listBox1.Invoke(new Action(() => { listBox1.Items.Clear(); }));
        }

        private void button15_Click(object sender, EventArgs e)
        {
            SendByte(104);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            SendByte(105);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            SendByte(106);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            SendByte(114);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            SendByte(115);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            SendByte(116);
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            ArrayList arguments = e.Argument as ArrayList;

            this.listBox1.Invoke(new Action(() => { listBox1.Items.Clear(); }));
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Cutting all relays"); }));
            System.Threading.Thread.Sleep(500);
            SendCommand(110); //all relays off
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("USB on"); }));
            System.Threading.Thread.Sleep(1000);
            int usbPort = int.Parse((string)arguments[0]) + 100;
            SendCommand((byte)usbPort);
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Power On"); }));
            System.Threading.Thread.Sleep(1000);
            int powerPort = int.Parse((string)arguments[1]) + 100;
            SendCommand((byte)powerPort);
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Pressing reset for 12 seconds..."); }));
            System.Threading.Thread.Sleep(1000);
            int resetPort = int.Parse((string)arguments[2]) + 100;
            SendCommand((byte)resetPort);
            System.Threading.Thread.Sleep(12000);
            int resetPortOff = int.Parse((string)arguments[2]) + 110;
            SendCommand((byte)resetPortOff);
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Relesed reset button"); }));
            this.listBox1.Invoke(new Action(() => { listBox1.Items.Add("Start flashing NOW"); }));
            System.Threading.Thread.Sleep(2000);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "FLS files (*.fls)|*.fls";
            dialog.Title = "Select files to flash";
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == DialogResult.OK)
                flsFiles = dialog.FileNames;
            if (flsFiles == null)
                return;
           
            //add double quotes.
            //for (int i = 0; i < flsFiles.Length; i++)
            //    flsFiles[i] = (char)34 + flsFiles[i] + (char)34;
            
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
           
            execute((string)e.Argument);
        }

        public void execute(string command)
        {
            CMD cli = new CMD(this);
            cli.Execute(command);
        }

    }
}
