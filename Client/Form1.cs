﻿using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using CommonConnection;

namespace ClientWindow
{
    public partial class Form1 : Form, IClientListener
    {
        private TcpClient _clientSocket;
        private bool isPrinting = false;
        private ModuleClient _mc;

        public Form1()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog {Multiselect = true};

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileName != "")
                {
                    foreach (var name in dialog.FileNames)
                    {
                        filesList.Items.Add(System.IO.Path.GetFileName(name));
                    }

                    printButton.Enabled = true;
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (isPrinting)
            {
                // 1 thread / file to cancel
            }

            Close();
        }

        private void filesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            deleteButton.Enabled = true;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

            foreach (ListViewItem i in filesList.SelectedItems)
            {
                filesList.Items.Remove(i);
            }

            if (filesList.Items.Count <= 0)
            {
                printButton.Enabled = false;
            }

            deleteButton.Enabled = false;
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            deleteButton.Enabled = false;
            printButton.Enabled = false;
            addButton.Enabled = false;
            isPrinting = true;
            var job = new Job(42);

            // send file name + file size as string

            _mc.SendDataToServer(GetBytes("Ceci est un test"));
        }

        private byte[] GetBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        private string GetString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        private void networkOptionsLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var networkOptions = new NetworkOptions(this);
            networkOptions.ShowDialog();
            if (networkOptions.IsValidConnection())
            {
                this._mc = networkOptions.GetModuleClient();
            }
        }

        public byte[] ProcessDataFromServer(byte[] responseFromServer, int dataSize)
        {
            Console.WriteLine(GetString(responseFromServer));
            return responseFromServer;
        }
    }
}
