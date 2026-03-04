namespace AdvancedRAT.UI.Builder
{
    partial class FormBuilder
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
            this.tbC2Server = new System.Windows.Forms.TextBox();
            this.tbC2Port = new System.Windows.Forms.TextBox();
            this.tbAESKey = new System.Windows.Forms.TextBox();
            this.cbAMSI = new System.Windows.Forms.CheckBox();
            this.cbUAC = new System.Windows.Forms.CheckBox();
            this.cbDefender = new System.Windows.Forms.CheckBox();
            this.cbAntiVM = new System.Windows.Forms.CheckBox();
            this.cbChrome = new System.Windows.Forms.CheckBox();
            this.cbTelegram = new System.Windows.Forms.CheckBox();
            this.cbSteam = new System.Windows.Forms.CheckBox();
            this.cbRansomware = new System.Windows.Forms.CheckBox();
            this.cbMJPEG = new System.Windows.Forms.CheckBox();
            this.cbMicTrigger = new System.Windows.Forms.CheckBox();
            this.cbOCR = new System.Windows.Forms.CheckBox();
            this.cbSelfDestruct = new System.Windows.Forms.CheckBox();
            this.btnBuild = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tbIconPath = new System.Windows.Forms.TextBox();
            this.btnSelectIcon = new System.Windows.Forms.Button();
            this.cbPersistence = new System.Windows.Forms.CheckBox();
            this.cbKeylogger = new System.Windows.Forms.CheckBox();
            this.cbScreenCapture = new System.Windows.Forms.CheckBox();
            this.cbWebcam = new System.Windows.Forms.CheckBox();
            this.cbMicrophone = new System.Windows.Forms.CheckBox();
            this.cbTrolling = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbC2Server
            // 
            this.tbC2Server.Location = new System.Drawing.Point(12, 25);
            this.tbC2Server.Name = "tbC2Server";
            this.tbC2Server.Size = new System.Drawing.Size(200, 20);
            this.tbC2Server.TabIndex = 0;
            // 
            // tbC2Port
            // 
            this.tbC2Port.Location = new System.Drawing.Point(218, 25);
            this.tbC2Port.Name = "tbC2Port";
            this.tbC2Port.Size = new System.Drawing.Size(100, 20);
            this.tbC2Port.TabIndex = 1;
            // 
            // tbAESKey
            // 
            this.tbAESKey.Location = new System.Drawing.Point(12, 65);
            this.tbAESKey.Name = "tbAESKey";
            this.tbAESKey.Size = new System.Drawing.Size(306, 20);
            this.tbAESKey.TabIndex = 2;
            // 
            // cbAMSI
            // 
            this.cbAMSI.AutoSize = true;
            this.cbAMSI.Location = new System.Drawing.Point(12, 105);
            this.cbAMSI.Name = "cbAMSI";
            this.cbAMSI.Size = new System.Drawing.Size(85, 17);
            this.cbAMSI.TabIndex = 3;
            this.cbAMSI.Text = "Bypass AMSI";
            this.cbAMSI.UseVisualStyleBackColor = true;
            // 
            // cbUAC
            // 
            this.cbUAC.AutoSize = true;
            this.cbUAC.Location = new System.Drawing.Point(12, 128);
            this.cbUAC.Name = "cbUAC";
            this.cbUAC.Size = new System.Drawing.Size(91, 17);
            this.cbUAC.TabIndex = 4;
            this.cbUAC.Text = "Bypass UAC";
            this.cbUAC.UseVisualStyleBackColor = true;
            // 
            // cbDefender
            // 
            this.cbDefender.AutoSize = true;
            this.cbDefender.Location = new System.Drawing.Point(12, 151);
            this.cbDefender.Name = "cbDefender";
            this.cbDefender.Size = new System.Drawing.Size(115, 17);
            this.cbDefender.TabIndex = 5;
            this.cbDefender.Text = "Suspend Defender";
            this.cbDefender.UseVisualStyleBackColor = true;
            // 
            // cbAntiVM
            // 
            this.cbAntiVM.AutoSize = true;
            this.cbAntiVM.Location = new System.Drawing.Point(12, 174);
            this.cbAntiVM.Name = "cbAntiVM";
            this.cbAntiVM.Size = new System.Drawing.Size(72, 17);
            this.cbAntiVM.TabIndex = 6;
            this.cbAntiVM.Text = "Anti-VM";
            this.cbAntiVM.UseVisualStyleBackColor = true;
            // 
            // cbChrome
            // 
            this.cbChrome.AutoSize = true;
            this.cbChrome.Location = new System.Drawing.Point(12, 197);
            this.cbChrome.Name = "cbChrome";
            this.cbChrome.Size = new System.Drawing.Size(97, 17);
            this.cbChrome.TabIndex = 7;
            this.cbChrome.Text = "Steal Chrome";
            this.cbChrome.UseVisualStyleBackColor = true;
            // 
            // cbTelegram
            // 
            this.cbTelegram.AutoSize = true;
            this.cbTelegram.Location = new System.Drawing.Point(12, 220);
            this.cbTelegram.Name = "cbTelegram";
            this.cbTelegram.Size = new System.Drawing.Size(102, 17);
            this.cbTelegram.TabIndex = 8;
            this.cbTelegram.Text = "Steal Telegram";
            this.cbTelegram.UseVisualStyleBackColor = true;
            // 
            // cbSteam
            // 
            this.cbSteam.AutoSize = true;
            this.cbSteam.Location = new System.Drawing.Point(12, 243);
            this.cbSteam.Name = "cbSteam";
            this.cbSteam.Size = new System.Drawing.Size(92, 17);
            this.cbSteam.TabIndex = 9;
            this.cbSteam.Text = "Steal Steam";
            this.cbSteam.UseVisualStyleBackColor = true;
            // 
            // cbRansomware
            // 
            this.cbRansomware.AutoSize = true;
            this.cbRansomware.Location = new System.Drawing.Point(12, 266);
            this.cbRansomware.Name = "cbRansomware";
            this.cbRansomware.Size = new System.Drawing.Size(108, 17);
            this.cbRansomware.TabIndex = 10;
            this.cbRansomware.Text = "Fake Ransomware";
            this.cbRansomware.UseVisualStyleBackColor = true;
            // 
            // cbMJPEG
            // 
            this.cbMJPEG.AutoSize = true;
            this.cbMJPEG.Location = new System.Drawing.Point(12, 289);
            this.cbMJPEG.Name = "cbMJPEG";
            this.cbMJPEG.Size = new System.Drawing.Size(103, 17);
            this.cbMJPEG.TabIndex = 11;
            this.cbMJPEG.Text = "MJPEG Streaming";
            this.cbMJPEG.UseVisualStyleBackColor = true;
            // 
            // cbMicTrigger
            // 
            this.cbMicTrigger.AutoSize = true;
            this.cbMicTrigger.Location = new System.Drawing.Point(12, 312);
            this.cbMicTrigger.Name = "cbMicTrigger";
            this.cbMicTrigger.Size = new System.Drawing.Size(114, 17);
            this.cbMicTrigger.TabIndex = 12;
            this.cbMicTrigger.Text = "Microphone Trigger";
            this.cbMicTrigger.UseVisualStyleBackColor = true;
            // 
            // cbOCR
            // 
            this.cbOCR.AutoSize = true;
            this.cbOCR.Location = new System.Drawing.Point(12, 335);
            this.cbOCR.Name = "cbOCR";
            this.cbOCR.Size = new System.Drawing.Size(53, 17);
            this.cbOCR.TabIndex = 13;
            this.cbOCR.Text = "OCR";
            this.cbOCR.UseVisualStyleBackColor = true;
            // 
            // cbSelfDestruct
            // 
            this.cbSelfDestruct.AutoSize = true;
            this.cbSelfDestruct.Location = new System.Drawing.Point(12, 358);
            this.cbSelfDestruct.Name = "cbSelfDestruct";
            this.cbSelfDestruct.Size = new System.Drawing.Size(86, 17);
            this.cbSelfDestruct.TabIndex = 14;
            this.cbSelfDestruct.Text = "Self Destruct";
            this.cbSelfDestruct.UseVisualStyleBackColor = true;
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(12, 400);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(75, 23);
            this.btnBuild.TabIndex = 15;
            this.btnBuild.Text = "Build";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 430);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "Ready";
            // 
            // tbIconPath
            // 
            this.tbIconPath.Location = new System.Drawing.Point(12, 375);
            this.tbIconPath.Name = "tbIconPath";
            this.tbIconPath.Size = new System.Drawing.Size(200, 20);
            this.tbIconPath.TabIndex = 17;
            // 
            // btnSelectIcon
            // 
            this.btnSelectIcon.Location = new System.Drawing.Point(218, 375);
            this.btnSelectIcon.Name = "btnSelectIcon";
            this.btnSelectIcon.Size = new System.Drawing.Size(100, 23);
            this.btnSelectIcon.TabIndex = 18;
            this.btnSelectIcon.Text = "Select Icon";
            this.btnSelectIcon.UseVisualStyleBackColor = true;
            this.btnSelectIcon.Click += new System.EventHandler(this.btnSelectIcon_Click);
            // 
            // cbPersistence
            // 
            this.cbPersistence.AutoSize = true;
            this.cbPersistence.Location = new System.Drawing.Point(12, 79);
            this.cbPersistence.Name = "cbPersistence";
            this.cbPersistence.Size = new System.Drawing.Size(78, 17);
            this.cbPersistence.TabIndex = 19;
            this.cbPersistence.Text = "Persistence";
            this.cbPersistence.UseVisualStyleBackColor = true;
            // 
            // cbKeylogger
            // 
            this.cbKeylogger.AutoSize = true;
            this.cbKeylogger.Location = new System.Drawing.Point(125, 105);
            this.cbKeylogger.Name = "cbKeylogger";
            this.cbKeylogger.Size = new System.Drawing.Size(72, 17);
            this.cbKeylogger.TabIndex = 20;
            this.cbKeylogger.Text = "Keylogger";
            this.cbKeylogger.UseVisualStyleBackColor = true;
            // 
            // cbScreenCapture
            // 
            this.cbScreenCapture.AutoSize = true;
            this.cbScreenCapture.Location = new System.Drawing.Point(125, 128);
            this.cbScreenCapture.Name = "cbScreenCapture";
            this.cbScreenCapture.Size = new System.Drawing.Size(102, 17);
            this.cbScreenCapture.TabIndex = 21;
            this.cbScreenCapture.Text = "Screen Capture";
            this.cbScreenCapture.UseVisualStyleBackColor = true;
            // 
            // cbWebcam
            // 
            this.cbWebcam.AutoSize = true;
            this.cbWebcam.Location = new System.Drawing.Point(125, 151);
            this.cbWebcam.Name = "cbWebcam";
            this.cbWebcam.Size = new System.Drawing.Size(72, 17);
            this.cbWebcam.TabIndex = 22;
            this.cbWebcam.Text = "Webcam";
            this.cbWebcam.UseVisualStyleBackColor = true;
            // 
            // cbMicrophone
            // 
            this.cbMicrophone.AutoSize = true;
            this.cbMicrophone.Location = new System.Drawing.Point(125, 174);
            this.cbMicrophone.Name = "cbMicrophone";
            this.cbMicrophone.Size = new System.Drawing.Size(84, 17);
            this.cbMicrophone.TabIndex = 23;
            this.cbMicrophone.Text = "Microphone";
            this.cbMicrophone.UseVisualStyleBackColor = true;
            // 
            // cbTrolling
            // 
            this.cbTrolling.AutoSize = true;
            this.cbTrolling.Location = new System.Drawing.Point(125, 197);
            this.cbTrolling.Name = "cbTrolling";
            this.cbTrolling.Size = new System.Drawing.Size(61, 17);
            this.cbTrolling.TabIndex = 24;
            this.cbTrolling.Text = "Trolling";
            this.cbTrolling.UseVisualStyleBackColor = true;
            // 
            // FormBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 460);
            this.Controls.Add(this.cbTrolling);
            this.Controls.Add(this.cbMicrophone);
            this.Controls.Add(this.cbWebcam);
            this.Controls.Add(this.cbScreenCapture);
            this.Controls.Add(this.cbKeylogger);
            this.Controls.Add(this.cbPersistence);
            this.Controls.Add(this.btnSelectIcon);
            this.Controls.Add(this.tbIconPath);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.cbSelfDestruct);
            this.Controls.Add(this.cbOCR);
            this.Controls.Add(this.cbMicTrigger);
            this.Controls.Add(this.cbMJPEG);
            this.Controls.Add(this.cbRansomware);
            this.Controls.Add(this.cbSteam);
            this.Controls.Add(this.cbTelegram);
            this.Controls.Add(this.cbChrome);
            this.Controls.Add(this.cbAntiVM);
            this.Controls.Add(this.cbDefender);
            this.Controls.Add(this.cbUAC);
            this.Controls.Add(this.cbAMSI);
            this.Controls.Add(this.tbAESKey);
            this.Controls.Add(this.tbC2Port);
            this.Controls.Add(this.tbC2Server);
            this.Name = "FormBuilder";
            this.Text = "AdvancedRAT Builder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbC2Server;
        private System.Windows.Forms.TextBox tbC2Port;
        private System.Windows.Forms.TextBox tbAESKey;
        private System.Windows.Forms.CheckBox cbAMSI;
        private System.Windows.Forms.CheckBox cbUAC;
        private System.Windows.Forms.CheckBox cbDefender;
        private System.Windows.Forms.CheckBox cbAntiVM;
        private System.Windows.Forms.CheckBox cbChrome;
        private System.Windows.Forms.CheckBox cbTelegram;
        private System.Windows.Forms.CheckBox cbSteam;
        private System.Windows.Forms.CheckBox cbRansomware;
        private System.Windows.Forms.CheckBox cbMJPEG;
        private System.Windows.Forms.CheckBox cbMicTrigger;
        private System.Windows.Forms.CheckBox cbOCR;
        private System.Windows.Forms.CheckBox cbSelfDestruct;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox tbIconPath;
        private System.Windows.Forms.Button btnSelectIcon;
        private System.Windows.Forms.CheckBox cbPersistence;
        private System.Windows.Forms.CheckBox cbKeylogger;
        private System.Windows.Forms.CheckBox cbScreenCapture;
        private System.Windows.Forms.CheckBox cbWebcam;
        private System.Windows.Forms.CheckBox cbMicrophone;
        private System.Windows.Forms.CheckBox cbTrolling;
    }
}