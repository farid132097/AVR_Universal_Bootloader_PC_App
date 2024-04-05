﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AVR_Universal_Bootloader_PC_App
{

    public partial class AVR_Universal_Boot : Form
    {
        
        int Retry_Reset = 0;

        public AVR_Universal_Boot()
        {
            InitializeComponent();
        }

        private void UART_Reset_Target()
        {
            if (UART.IsOpen)
            {
                try
                {
                    UART.DtrEnable = true;
                    UART.RtsEnable = true;
                }
                catch
                {

                }

                Thread.Sleep(5);
                try
                {
                    UART.DtrEnable = false;
                    UART.RtsEnable = false;
                }
                catch
                {

                }
            }
        }

        private void cbSelectPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UART.IsOpen)
            {
                try
                {
                    UART.Close();
                }
                catch
                {

                }
            }

            UART.PortName = cbSelectPort.SelectedItem.ToString();
            UART.BaudRate = 9600;
            try
            {
                UART.Open();
            }
            catch { 
            
            }

            if (UART.IsOpen)
            {
                cbSelectPort.ForeColor = Color.LightGreen;
            }
            else
            {
                cbSelectPort.ForeColor = Color.Red;
            }
            

        }

        private void cbSelectPort_MouseClick(object sender, MouseEventArgs e)
        {
            if (UART.IsOpen)
            {
                try
                {
                    UART.Close();
                }
                catch { 
                    
                }
            }

            string[] ports = SerialPort.GetPortNames();
            cbSelectPort.Items.Clear();
            cbSelectPort.Items.AddRange(ports);
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            Retry_Reset = 5;
            tmrRetry.Enabled = true;

        }

        private void tmrRetry_Tick(object sender, EventArgs e)
        {
            if (Retry_Reset > 0)
            {
                Retry_Reset--;
                UART_Reset_Target();
            }
            else
            {
                tmrRetry.Enabled = false;
            }
        }
    }
}
