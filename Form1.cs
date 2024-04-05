using System;
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
        
        int     DCNT_Retry_Reset   = 0;

        string  Config_Packet_Debug = "";
        UInt16  UART_Available_Data = 0;
        byte[]  UART_Data_Buffer = new byte[200];
        byte    SYNC_Byte          = 0;
        UInt32  Device_Signature   = 0;
        UInt32  Device_Baud_Rate   = 0;
        UInt16  CRC_Received       = 0;
        UInt16  CRC_Calculated     = 0;
        UInt16  CRC_OK             = 0;


        public AVR_Universal_Boot()
        {
            InitializeComponent();
        }

        private void UART_DTR_RTS_Enable()
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
            }
        }

        private void UART_DTR_RTS_Disable()
        {
            if (UART.IsOpen)
            {
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

        private void UART_Reset()
        {
            UART_DTR_RTS_Enable();
            Thread.Sleep(1);
            UART_DTR_RTS_Disable();
        }

        UInt16 Calcuate_CRC(UInt16 crc, byte data)
        {
            UInt16 temp = data;
            temp <<= 8;
            crc = (UInt16)(crc ^ temp);
            for (byte i = 0; i < 8; i++)
            {
                if ((crc & 0x8000) == 0x8000)
                {
                    crc = (UInt16)((crc << 1) ^ 0x1021);
                }
                else
                {
                    crc <<= 1;
                }
            }
            return crc;
        }

        UInt16 Calculate_CRC_Block(byte[] data, UInt16 len)
        {
            UInt16 crc = 0;
            for(UInt16 i = 0; i < len; i++)
            {
                crc = Calcuate_CRC(crc, data[i]);
            }
            return crc;
        }

        private void UART_Data_Validate(object sender, EventArgs e)
        {

            Config_Packet_Debug = "";
            CRC_OK = 0;

            SYNC_Byte        = UART_Data_Buffer[0];

            Device_Signature = UART_Data_Buffer[1];
            Device_Signature <<= 8;
            Device_Signature |= UART_Data_Buffer[2];
            Device_Signature <<= 8;
            Device_Signature |= UART_Data_Buffer[3];
            Device_Signature <<= 8;
            Device_Signature |= UART_Data_Buffer[4];

            Device_Baud_Rate  = UART_Data_Buffer[5];
            Device_Baud_Rate <<= 8;
            Device_Baud_Rate |= UART_Data_Buffer[6];
            Device_Baud_Rate <<= 8;
            Device_Baud_Rate |= UART_Data_Buffer[7];

            CRC_Received      = UART_Data_Buffer[8];
            CRC_Received     <<= 8;
            CRC_Received     |= UART_Data_Buffer[9];

            CRC_Calculated = Calculate_CRC_Block(UART_Data_Buffer, 8);

            if(CRC_Received == CRC_Calculated)
            {
                CRC_OK = 1;
            }

            /*
            Config_Packet_Debug += "SYNC ";
            Config_Packet_Debug += SYNC_Byte.ToString("X");

            Config_Packet_Debug += " Device ";
            Config_Packet_Debug += Device_Signature.ToString("X");

            Config_Packet_Debug += " BaudRate ";
            Config_Packet_Debug += Device_Baud_Rate.ToString("X");

            Config_Packet_Debug += " CRCrecv ";
            Config_Packet_Debug += CRC_Received.ToString("X");

            Config_Packet_Debug += " CRCcalc ";
            Config_Packet_Debug += CRC_Calculated.ToString("X");

            Config_Packet_Debug += " CRC_OK ";
            Config_Packet_Debug += CRC_OK.ToString("X");

            this.Invoke(new EventHandler(rtbOutput_TextChanged));
            */
        }

        byte Get_CRC_Status()
        {
            byte sts = 0;
            if(CRC_OK == 1)
            {
                sts = 1;
                CRC_OK = 0;
            }
            return sts;
        }

        byte Get_SYNC_Byte()
        {
            return SYNC_Byte;
        }

        UInt32 Get_Device_Signature()
        {
            return Device_Signature;
        }

        UInt32 Get_Baud_Rate()
        {
            return Device_Baud_Rate;
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
            DCNT_Retry_Reset = 10;
            tmrRetry.Enabled = true;
            UART.ReadExisting();
            UART.DiscardInBuffer();
            UART_Available_Data = 0;
        }

        private void tmrRetry_Tick(object sender, EventArgs e)
        {
            if (DCNT_Retry_Reset > 0)
            {
                tmrRetry.Enabled = false;
                if (Get_CRC_Status() == 1)
                {
                    DCNT_Retry_Reset = 0;
                }
                else
                {
                    DCNT_Retry_Reset--;
                    UART_Reset();
                    tmrRetry.Enabled = true;
                }
            }
            else
            {
                tmrRetry.Enabled = false;
            }
        }

        private void UART_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            UART_Available_Data = (UInt16)UART.BytesToRead;
            UART.Read(UART_Data_Buffer, 0, UART_Available_Data);

            if (UART_Available_Data == 10)
            {
                this.Invoke(new EventHandler(UART_Data_Validate));
            }
        }

        private void rtbOutput_TextChanged(object sender, EventArgs e)
        {
            rtbOutput.Text = Config_Packet_Debug;
        }
    }
}
