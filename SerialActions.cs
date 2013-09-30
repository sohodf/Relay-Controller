using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace RelayController
{
    class SerialActions
    {
        public static SerialPort OpenCom(string com)
        {
          SerialPort Port = new SerialPort();
          Port.PortName = com;
          Port.BaudRate = 9600;

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


    }
}
