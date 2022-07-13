using System.Drawing;

namespace ControllerNTR
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusLabel = new System.Windows.Forms.Label();
            this.typeSelectGroupBox = new System.Windows.Forms.GroupBox();
            this.xbox360ControllerRadioButton = new System.Windows.Forms.RadioButton();
            this.dualShock4RadioButton = new System.Windows.Forms.RadioButton();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.statusCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.typeSelectGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(13, 9);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(94, 68);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "控制器1: 检测中\r\n控制器2: 检测中\r\n控制器3: 检测中\r\n控制器4: 检测中";
            // 
            // typeSelectGroupBox
            // 
            this.typeSelectGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.typeSelectGroupBox.Controls.Add(this.xbox360ControllerRadioButton);
            this.typeSelectGroupBox.Controls.Add(this.dualShock4RadioButton);
            this.typeSelectGroupBox.Location = new System.Drawing.Point(12, 84);
            this.typeSelectGroupBox.Name = "typeSelectGroupBox";
            this.typeSelectGroupBox.Size = new System.Drawing.Size(360, 80);
            this.typeSelectGroupBox.TabIndex = 1;
            this.typeSelectGroupBox.TabStop = false;
            this.typeSelectGroupBox.Text = "目标控制器类型";
            // 
            // xbox360ControllerRadioButton
            // 
            this.xbox360ControllerRadioButton.AutoSize = true;
            this.xbox360ControllerRadioButton.Location = new System.Drawing.Point(6, 49);
            this.xbox360ControllerRadioButton.Name = "xbox360ControllerRadioButton";
            this.xbox360ControllerRadioButton.Size = new System.Drawing.Size(335, 21);
            this.xbox360ControllerRadioButton.TabIndex = 1;
            this.xbox360ControllerRadioButton.Text = "Microsoft Xbox 360 Controller (仅使用启动时的控制器)";
            this.xbox360ControllerRadioButton.UseVisualStyleBackColor = true;
            // 
            // dualShock4RadioButton
            // 
            this.dualShock4RadioButton.AutoSize = true;
            this.dualShock4RadioButton.Checked = true;
            this.dualShock4RadioButton.Location = new System.Drawing.Point(6, 22);
            this.dualShock4RadioButton.Name = "dualShock4RadioButton";
            this.dualShock4RadioButton.Size = new System.Drawing.Size(130, 21);
            this.dualShock4RadioButton.TabIndex = 0;
            this.dualShock4RadioButton.TabStop = true;
            this.dualShock4RadioButton.Text = "Sony DualShock 4";
            this.dualShock4RadioButton.UseVisualStyleBackColor = true;
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startButton.Location = new System.Drawing.Point(12, 174);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(85, 30);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "启动";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(287, 174);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(85, 30);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "停止";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // statusCheckTimer
            // 
            this.statusCheckTimer.Enabled = true;
            this.statusCheckTimer.Interval = 1000;
            this.statusCheckTimer.Tick += new System.EventHandler(this.statusCheckTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(384, 216);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.typeSelectGroupBox);
            this.Controls.Add(this.statusLabel);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "ControllerNTR";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.typeSelectGroupBox.ResumeLayout(false);
            this.typeSelectGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.GroupBox typeSelectGroupBox;
        private System.Windows.Forms.RadioButton xbox360ControllerRadioButton;
        private System.Windows.Forms.RadioButton dualShock4RadioButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Timer statusCheckTimer;
    }
}

