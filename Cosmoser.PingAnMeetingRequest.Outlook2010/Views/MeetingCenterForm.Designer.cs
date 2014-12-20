namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Views
{
    partial class MeetingCenterForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.dataGridViewDisableCheckBoxColumn1 = new Cosmoser.PingAnMeetingRequest.Outlook2010.Views.DataGridViewDisableCheckBoxColumn();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtAlias = new System.Windows.Forms.TextBox();
            this.txtServiceKey = new System.Windows.Forms.TextBox();
            this.txtRoomName = new System.Windows.Forms.TextBox();
            this.txtMeetingName = new System.Windows.Forms.TextBox();
            this.comboBoxMideaType = new System.Windows.Forms.ComboBox();
            this.comboBoxConfType = new System.Windows.Forms.ComboBox();
            this.comboBoxConfProperty = new System.Windows.Forms.ComboBox();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkbox = new Cosmoser.PingAnMeetingRequest.Outlook2010.Views.DataGridViewDisableCheckBoxColumn();
            this.MeetingName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeetingStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeetingType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainMeetingRoom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeetingPwd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.checkbox,
            this.MeetingName,
            this.StartTime,
            this.EndTime,
            this.MeetingStatus,
            this.MeetingType,
            this.MainMeetingRoom,
            this.ServiceKey,
            this.MeetingPwd});
            this.dataGridView1.Location = new System.Drawing.Point(12, 383);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(852, 221);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(261, 626);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 21);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(469, 626);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 21);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(364, 624);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 16;
            this.btnUpdate.Text = "修改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // dataGridViewDisableCheckBoxColumn1
            // 
            this.dataGridViewDisableCheckBoxColumn1.HeaderText = "";
            this.dataGridViewDisableCheckBoxColumn1.Name = "dataGridViewDisableCheckBoxColumn1";
            this.dataGridViewDisableCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewDisableCheckBoxColumn1.Width = 25;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(359, 337);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 45;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(546, 298);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 12);
            this.label13.TabIndex = 44;
            this.label13.Text = "（可部分匹配）";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(546, 263);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 12);
            this.label12.TabIndex = 43;
            this.label12.Text = "（可部分匹配）";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(546, 228);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 42;
            this.label11.Text = "（可部分匹配）";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(546, 192);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 12);
            this.label10.TabIndex = 41;
            this.label10.Text = "（可部分匹配）";
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point(309, 295);
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size(214, 21);
            this.txtAlias.TabIndex = 40;
            // 
            // txtServiceKey
            // 
            this.txtServiceKey.Location = new System.Drawing.Point(309, 260);
            this.txtServiceKey.Name = "txtServiceKey";
            this.txtServiceKey.Size = new System.Drawing.Size(214, 21);
            this.txtServiceKey.TabIndex = 39;
            // 
            // txtRoomName
            // 
            this.txtRoomName.Location = new System.Drawing.Point(309, 225);
            this.txtRoomName.Name = "txtRoomName";
            this.txtRoomName.Size = new System.Drawing.Size(214, 21);
            this.txtRoomName.TabIndex = 38;
            // 
            // txtMeetingName
            // 
            this.txtMeetingName.Location = new System.Drawing.Point(309, 189);
            this.txtMeetingName.Name = "txtMeetingName";
            this.txtMeetingName.Size = new System.Drawing.Size(214, 21);
            this.txtMeetingName.TabIndex = 37;
            // 
            // comboBoxMideaType
            // 
            this.comboBoxMideaType.FormattingEnabled = true;
            this.comboBoxMideaType.Location = new System.Drawing.Point(309, 154);
            this.comboBoxMideaType.Name = "comboBoxMideaType";
            this.comboBoxMideaType.Size = new System.Drawing.Size(150, 20);
            this.comboBoxMideaType.TabIndex = 36;
            // 
            // comboBoxConfType
            // 
            this.comboBoxConfType.FormattingEnabled = true;
            this.comboBoxConfType.Location = new System.Drawing.Point(309, 121);
            this.comboBoxConfType.Name = "comboBoxConfType";
            this.comboBoxConfType.Size = new System.Drawing.Size(150, 20);
            this.comboBoxConfType.TabIndex = 35;
            // 
            // comboBoxConfProperty
            // 
            this.comboBoxConfProperty.FormattingEnabled = true;
            this.comboBoxConfProperty.Location = new System.Drawing.Point(309, 89);
            this.comboBoxConfProperty.Name = "comboBoxConfProperty";
            this.comboBoxConfProperty.Size = new System.Drawing.Size(150, 20);
            this.comboBoxConfProperty.TabIndex = 34;
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Location = new System.Drawing.Point(309, 18);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(150, 21);
            this.dateTimePickerStart.TabIndex = 33;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.Location = new System.Drawing.Point(309, 52);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(150, 21);
            this.dateTimePickerEnd.TabIndex = 32;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(223, 263);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 31;
            this.label9.Text = "呼入号码";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(223, 298);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 30;
            this.label8.Text = "终端号码";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(223, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 29;
            this.label7.Text = "截止时间";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(223, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 28;
            this.label6.Text = "会议性质";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(223, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 27;
            this.label5.Text = "会议类型";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(217, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "视频 本地";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(223, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "会议名称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(211, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 24;
            this.label2.Text = "会议室名称";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(223, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "开始时间";
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // checkbox
            // 
            this.checkbox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.checkbox.DataPropertyName = "Selected";
            this.checkbox.FalseValue = "NoSelected";
            this.checkbox.HeaderText = "";
            this.checkbox.IndeterminateValue = "Indeterminate";
            this.checkbox.Name = "checkbox";
            this.checkbox.ReadOnly = true;
            this.checkbox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.checkbox.TrueValue = "Selected";
            this.checkbox.Width = 5;
            // 
            // MeetingName
            // 
            this.MeetingName.DataPropertyName = "Name";
            this.MeetingName.HeaderText = "会议名称";
            this.MeetingName.Name = "MeetingName";
            this.MeetingName.ReadOnly = true;
            this.MeetingName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // StartTime
            // 
            this.StartTime.DataPropertyName = "StartTime";
            this.StartTime.HeaderText = "召开时间";
            this.StartTime.Name = "StartTime";
            this.StartTime.ReadOnly = true;
            // 
            // EndTime
            // 
            this.EndTime.DataPropertyName = "EndTime";
            this.EndTime.HeaderText = "结束时间";
            this.EndTime.Name = "EndTime";
            this.EndTime.ReadOnly = true;
            // 
            // MeetingStatus
            // 
            this.MeetingStatus.HeaderText = "会议状态";
            this.MeetingStatus.Name = "MeetingStatus";
            this.MeetingStatus.ReadOnly = true;
            // 
            // MeetingType
            // 
            this.MeetingType.HeaderText = "会议类型";
            this.MeetingType.Name = "MeetingType";
            this.MeetingType.ReadOnly = true;
            // 
            // MainMeetingRoom
            // 
            this.MainMeetingRoom.HeaderText = "主会场";
            this.MainMeetingRoom.Name = "MainMeetingRoom";
            this.MainMeetingRoom.ReadOnly = true;
            // 
            // ServiceKey
            // 
            this.ServiceKey.HeaderText = "呼入号";
            this.ServiceKey.Name = "ServiceKey";
            this.ServiceKey.ReadOnly = true;
            // 
            // MeetingPwd
            // 
            this.MeetingPwd.HeaderText = "会议密码";
            this.MeetingPwd.Name = "MeetingPwd";
            this.MeetingPwd.ReadOnly = true;
            // 
            // MeetingCenterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(876, 654);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtAlias);
            this.Controls.Add(this.txtServiceKey);
            this.Controls.Add(this.txtRoomName);
            this.Controls.Add(this.txtMeetingName);
            this.Controls.Add(this.comboBoxMideaType);
            this.Controls.Add(this.comboBoxConfType);
            this.Controls.Add(this.comboBoxConfProperty);
            this.Controls.Add(this.dateTimePickerStart);
            this.Controls.Add(this.dateTimePickerEnd);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MeetingCenterForm";
            this.Text = "个人会议中心";
            this.Activated += new System.EventHandler(this.MeetingCenterForm_Activated);
            this.Load += new System.EventHandler(this.MeetingCenterForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private DataGridViewDisableCheckBoxColumn dataGridViewDisableCheckBoxColumn1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtAlias;
        private System.Windows.Forms.TextBox txtServiceKey;
        private System.Windows.Forms.TextBox txtRoomName;
        private System.Windows.Forms.TextBox txtMeetingName;
        private System.Windows.Forms.ComboBox comboBoxMideaType;
        private System.Windows.Forms.ComboBox comboBoxConfType;
        private System.Windows.Forms.ComboBox comboBoxConfProperty;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private DataGridViewDisableCheckBoxColumn checkbox;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeetingName;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeetingStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeetingType;
        private System.Windows.Forms.DataGridViewTextBoxColumn MainMeetingRoom;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeetingPwd;
    }
}