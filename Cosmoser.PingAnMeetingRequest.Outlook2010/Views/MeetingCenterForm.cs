using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using Cosmoser.PingAnMeetingRequest.Common.ClientService;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Views
{
    public partial class MeetingCenterForm : Form
    {
        private string currentMeetingId;
        private MeetingData _meetingData;

        public MeetingCenterForm()
        {
            InitializeComponent();
        }

        private void MeetingCenterForm_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;

            var task = OutlookFacade.Instance().CalendarFolder.CalendarDataManager.GetMeetingListSyncTask();

            task.Wait();

            _meetingData = task.Result;

            this.SetDataSource(_meetingData.Values.ToList());

            this.InitializeUI();
        }

        private void InitializeUI()
        {
            this.dateTimePickerStart.Value = DateTime.Today;
            this.dateTimePickerEnd.Value = DateTime.Today.AddMonths(2);

            this.comboBoxConfProperty.Items.Add("全部");//sting.Empty
            this.comboBoxConfProperty.Items.Add("执委会议"); //1
            this.comboBoxConfProperty.Items.Add("非执委会议"); //2
            this.comboBoxConfProperty.SelectedIndex = 0;

            this.comboBoxMideaType.Items.Add("全部");//-1
            this.comboBoxMideaType.Items.Add("本地会议");//4
            this.comboBoxMideaType.Items.Add("两方视频会议");//1
            this.comboBoxMideaType.Items.Add("多方视频会议");//2
            this.comboBoxMideaType.SelectedIndex = 0;

            this.comboBoxConfType.Items.Add("全部");//-1
            this.comboBoxConfType.Items.Add("视频");//1
            this.comboBoxConfType.Items.Add("本地");//2
            this.comboBoxConfType.SelectedIndex = 0;

        }

        private void SetDataSource(List<SVCMMeeting> list)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = list;
            //this.dataGridView1.RowCount = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Id"].Value = list[i].Id;
                dataGridView1.Rows[i].Cells["checkbox"].Value = SelectedStatus.NoSelected;
                dataGridView1.Rows[i].Cells["MeetingName"].Value = list[i].Name;
                dataGridView1.Rows[i].Cells["StartTime"].Value = list[i].StartTime;
                dataGridView1.Rows[i].Cells["EndTime"].Value = list[i].EndTime;
                dataGridView1.Rows[i].Cells["MeetingStatus"].Value = list[i].Status;
                dataGridView1.Rows[i].Cells["MeetingType"].Value = list[i].MideaTypeStr;
                dataGridView1.Rows[i].Cells["MainMeetingRoom"].Value = list[i].MainRoom;
                dataGridView1.Rows[i].Cells["ServiceKey"].Value = list[i].ServiceKey;
                dataGridView1.Rows[i].Cells["MeetingPwd"].Value = list[i].Password;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];

                if (column is DataGridViewCheckBoxColumn)
                {
                    DataGridViewDisableCheckBoxCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewDisableCheckBoxCell;
                    if (!cell.Enabled)
                    {
                        return;
                    }
                    if ((SelectedStatus)cell.Value == SelectedStatus.NoSelected)
                    {
                        cell.Value = SelectedStatus.Selected;
                        SetRadioButtonValue(cell);//if radiobutton, uncomment this code line.
                        currentMeetingId = this.dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                    }
                    else
                    {
                        cell.Value = SelectedStatus.NoSelected;
                    }
                }
            }
        }

        private void SetRadioButtonValue(DataGridViewDisableCheckBoxCell cell)
        {
            SelectedStatus status = (SelectedStatus)cell.Value;
            if (status == SelectedStatus.Selected)
            {
                status = SelectedStatus.NoSelected;
            }
            else
            {
                status = SelectedStatus.Selected;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewDisableCheckBoxCell cel = dataGridView1.Rows[i].Cells["checkbox"] as DataGridViewDisableCheckBoxCell;
                if (!cel.Equals(cell))
                {
                    cel.Value = status;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(currentMeetingId))
            {
                var appt = OutlookFacade.Instance().CalendarFolder.AppointmentCollection[currentMeetingId];
                appt.Display();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(currentMeetingId))
            {
                var appt = OutlookFacade.Instance().CalendarFolder.AppointmentCollection[currentMeetingId];
                appt.Delete();
                _meetingData.Remove(currentMeetingId);
                this.SetDataSource(_meetingData.Values.ToList());
            }
        }

        private void MeetingCenterForm_Activated(object sender, EventArgs e)
        {
            //this.SetDataSource(_meetingData.Values.ToList());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            MeetingListQuery query = new MeetingListQuery();
            query.StartTime = this.dateTimePickerStart.Value;
            query.EndTime = this.dateTimePickerEnd.Value;
            
            int index = this.comboBoxConfType.SelectedIndex;

            switch (index)
            {
                case 0:
                    query.StatVideoType = -1;
                    break;
                case 1:
                    query.StatVideoType = 4;
                    break;
                case 2:
                    query.StatVideoType = 1;
                    break;
                case 3:
                    query.StatVideoType = 2;
                    break;

            }

            query.ConferenceProperty = this.comboBoxConfProperty.SelectedIndex == 0 ? string.Empty : this.comboBoxConfProperty.SelectedIndex.ToString();

            query.ConfType = this.comboBoxConfType.SelectedIndex == 0 ? "-1" : this.comboBoxConfType.SelectedIndex.ToString();

            query.MeetingName = this.txtMeetingName.Text;
            query.RoomName = this.txtRoomName.Text;
            query.Alias = this.txtAlias.Text;
            query.ServiceKey = this.txtServiceKey.Text;

            List<SVCMMeeting> list;
            if (ClientServiceFactory.Create().TryGetMeetingList(query, OutlookFacade.Instance().Session, out list))
            {
                this.SetDataSource(list);
            }
            else
            {
                MessageBox.Show("获取会议列表失败！");
            }
        }
    }
}
