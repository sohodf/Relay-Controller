using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RelayController
{
    class SerialActions
    {
        public string[] GetComs()
        {
            string[] coms = null;
            coms = System.IO.Ports.SerialPort.GetPortNames();
            return coms;

        }


    }
}
