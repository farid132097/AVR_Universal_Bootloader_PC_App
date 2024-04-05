namespace AVR_Universal_Bootloader_PC_App
{
    partial class AVR_Universal_Boot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cbSelectPort = new System.Windows.Forms.ComboBox();
            this.UART = new System.IO.Ports.SerialPort(this.components);
            this.btnWrite = new System.Windows.Forms.Button();
            this.tmrRetry = new System.Windows.Forms.Timer(this.components);
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // cbSelectPort
            // 
            this.cbSelectPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectPort.FormattingEnabled = true;
            this.cbSelectPort.Location = new System.Drawing.Point(12, 12);
            this.cbSelectPort.Name = "cbSelectPort";
            this.cbSelectPort.Size = new System.Drawing.Size(121, 21);
            this.cbSelectPort.TabIndex = 0;
            this.cbSelectPort.SelectedIndexChanged += new System.EventHandler(this.cbSelectPort_SelectedIndexChanged);
            this.cbSelectPort.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbSelectPort_MouseClick);
            // 
            // UART
            // 
            this.UART.Handshake = System.IO.Ports.Handshake.XOnXOff;
            this.UART.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.UART_DataReceived);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(12, 39);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(121, 23);
            this.btnWrite.TabIndex = 1;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // tmrRetry
            // 
            this.tmrRetry.Interval = 40;
            this.tmrRetry.Tick += new System.EventHandler(this.tmrRetry_Tick);
            // 
            // rtbOutput
            // 
            this.rtbOutput.Location = new System.Drawing.Point(12, 77);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.Size = new System.Drawing.Size(358, 43);
            this.rtbOutput.TabIndex = 2;
            this.rtbOutput.Text = "";
            this.rtbOutput.TextChanged += new System.EventHandler(this.rtbOutput_TextChanged);
            // 
            // AVR_Universal_Boot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 143);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.cbSelectPort);
            this.Name = "AVR_Universal_Boot";
            this.Text = "AVR Universal Boot";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbSelectPort;
        private System.IO.Ports.SerialPort UART;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Timer tmrRetry;
        private System.Windows.Forms.RichTextBox rtbOutput;
    }
}

