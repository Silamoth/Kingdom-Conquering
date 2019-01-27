using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace Launcher
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            String playerName = kingdomNameTextBox.Text;
            String password = passwordTextBox.Text;

            TcpClient client = new TcpClient("127.0.0.1", 1303);

            StreamWriter writer = new StreamWriter(client.GetStream());

            String messageString = "3 " + playerName + " " + password;
            byte[] message = Encoding.ASCII.GetBytes(messageString);
            writer.BaseStream.Write(message, 0, message.Length);
            writer.Flush();

            byte[] buffer = new byte[client.Client.ReceiveBufferSize];
            client.Client.Receive(buffer);
            String response = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', '\0' });
            
            switch (response)
            {
                case "Success":
                    string genArgs = playerName;
                    string pathToFile = "C:/Users/Michael/Documents/Project Files/Kingdom Conquering/Kingdom Conquering/Kingdom Conquering/bin/Windows/x86/Debug/Kingdom Conquering.exe";
                    Process runProg = new Process();
                    try
                    {
                        runProg.StartInfo.FileName = pathToFile;
                        runProg.StartInfo.Arguments = genArgs;
                        runProg.StartInfo.CreateNoWindow = true;
                        runProg.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not start program: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "Wrong":
                    MessageBox.Show("Wrong Kingdom Name or Password.  Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case "Failure":
                    MessageBox.Show("Unfortunately, a connection could not be established with the server.  Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            //client.Close();
        }

        private void createAccountButton_Click(object sender, EventArgs e)
        {
            CreateAccountForm form = new CreateAccountForm();
            form.ShowDialog();
        }
    }
}