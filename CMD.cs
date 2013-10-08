using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace RelayController
{
    class CMD
    {
        //global - adb timeout in miliseconds
        static int timeout = 900000;
        //global - main class
        Relay mw = null;

        public CMD(Relay mw)
        {
            this.mw = mw;
        }


        //this method handles the actual sending of the command to shell
        //no putput redirection. executed in a seperate window.
        public void Execute(string command)
        {
            ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = false;

            Process p = new Process();
            //p.ErrorDataReceived += cmd_Error;
            //p.OutputDataReceived += cmd_DataReceived;
            p.StartInfo = startInfo;

            p.Start();

            if (!p.WaitForExit(timeout))
                return;

        }

        //executes a cli command and returns a string with it's result.
        public string ExecuteResponding(string command)
        {
            Process p = new Process();
            //p.ErrorDataReceived += cmd_Error;
            //p.OutputDataReceived += cmd_DataReceived;
            ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            p.StartInfo = startInfo;

            p.Start();

            string adbResponse = p.StandardOutput.ReadToEnd();
            adbResponse += p.StandardError.ReadToEnd();

            if (!p.WaitForExit(timeout))
            {
                return string.Empty;
            }
            else
            {
                return adbResponse;
            }


        }


        //private void cmd_DataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    mw.listBox1.Invoke(new Action(() => { mw.listBox1.Items.Add(e.Data); }));
        //}

        //private void cmd_Error(object sender, DataReceivedEventArgs e)
        //{
        //    mw.listBox1.Invoke(new Action(() => { mw.listBox1.Items.Add(e.Data); }));
        //}

    }
}
