using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Launcher
{
    public partial class CreateAccountForm : Form
    {
        String currentTypedName;

        public CreateAccountForm()
        {
            InitializeComponent();
            currentTypedName = String.Empty;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text == String.Empty)
            {
                MessageBox.Show("You must enter a name for your player.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (passwordTextBox.Text == String.Empty)
            {
                MessageBox.Show("You must enter a password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (verifyTextBox.Text == String.Empty)
            {
                MessageBox.Show("You must verify your password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (passwordTextBox.Text != verifyTextBox.Text)
            {
                MessageBox.Show("Your passwords must match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TcpClient client = new TcpClient("127.0.0.1", 1303);

            StreamWriter writer = new StreamWriter(client.GetStream());

            String messageString = "4 " + nameTextBox.Text + " " + passwordTextBox.Text;
            byte[] message = Encoding.ASCII.GetBytes(messageString);
            writer.BaseStream.Write(message, 0, message.Length);

            byte[] buffer = new byte[client.Client.ReceiveBufferSize];
            client.Client.Receive(buffer);
            String response = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\n', '\r', '\0' });

            if (response == "Good")
            {
                MessageBox.Show("Congratulations!  Your account has successfully been created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);

                string genArgs = nameTextBox.Text;
                string pathToFile = "C:/Users/Michael/Documents/Project Files/Kingdom Conquering/Kingdom Conquering/Kingdom Conquering/bin/Windows/x86/Debug/Kingdom Conquering.exe";
                Process runProg = new Process();
                try
                {
                    runProg.StartInfo.FileName = pathToFile;
                    runProg.StartInfo.Arguments = genArgs;
                    runProg.StartInfo.CreateNoWindow = true;
                    runProg.Start();

                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not start program: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (response == "Taken")            
                MessageBox.Show("Unfortunately, that name is taken already...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (response == "Error")
                MessageBox.Show("Unfortunately, something went wrong.  Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (nameTextBox.Text.Length > 20)
            {
                nameTextBox.Text = currentTypedName;
            }

            currentTypedName = nameTextBox.Text;
            nameTextBox.SelectionStart = currentTypedName.Length;
        }
    }
}