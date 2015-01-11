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
using log4net;
using Cosmoser.PingAnMeetingRequest.Common.Utilities;
using System.Threading.Tasks;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Views
{
    public partial class MeetingCenterForm : Form
    {
        private string currentMeetingId;
        private MeetingData _meetingData;
        static ILog logger = IosLogManager.GetLogger(typeof(MeetingCenterForm));

        public MeetingCenterForm()
        {
            InitializeComponent();
        }

        private void MeetingCenterForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView1.AutoGenerateColumns = false;
                this.InitializeUI();
                lblMessage.Text = "正在同步...";
                lblMessage.ForeColor = Color.Red;

                Task<MeetingData> task = OutlookFacade.Instance().CalendarFolder.CalendarDataManager.GetMeetingListSyncTask();

                //task.Start();

                task.Wait();

                _meetingData = task.Result;
                this.SetDataSource(_meetingData.Values.ToList());

                //Func<MeetingData,bool> func = OutlookFacade.Instance().CalendarFolder.CalendarDataManager.LoadMeetingdataFromServer;
                //MeetingData meetingData = new MeetingData();
                //Task.Factory.FromAsync<MeetingData,bool>(func.BeginInvoke, func.EndInvoke, meetingData, null)
                //    .ContinueWith((result) =>
                //    {
                //        bool succeed = result.Result;
                //        if (succeed)
                //        {
                //            _meetingData = meetingData;
                //            this.SetDataSource(_meetingData.Values.ToList());
                //        }
                //        else
                //        {
                //            logger.Error("同步会议列表信息错误！");
                //        }
                //    });
                
                
                

                lblMessage.Text = string.Empty;
              
            }
            catch (Exception ex)
            {
                logger.Error("Load failed!" + ex.Message + ex.StackTrace);
                MessageBox.Show("加载失败！" + ex.Message);
            }
        }

        private void InitializeUI()
        {
            this.dateTimePickerStart.Value = DateTime.Today;
            this.dateTimePickerEnd.Value = DateTime.Today.AddMonths(2);

            this.comboBoxConfProperty.Items.Add("全部");//sting.Empty
            this.comboBoxConfProperty.Items.Add("执委会议"); //1
            this.comboBoxConfProperty.Items.Add("非执委会议"); //2
            this.comboBoxConfProperty.SelectedIndex = 0;

            this.comboBoxConfType.Items.Add("全部");//-1
            this.comboBoxConfType.Items.Add("本地会议");//4
            this.comboBoxConfType.Items.Add("两方视频会议");//1
            this.comboBoxConfType.Items.Add("多方视频会议");//2
            this.comboBoxConfType.SelectedIndex = 0;

            this.comboBoxMideaType.Items.Add("全部");//-1
            this.comboBoxMideaType.Items.Add("视频");//1
            this.comboBoxMideaType.Items.Add("本地");//2
            this.comboBoxMideaType.SelectedIndex = 0;

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
                //dataGridView1.Rows[i].Cells["MeetingPwd"].Value = list[i].Password;

                if (list[i].StatusCode == 3)
                {
                    dataGridView1.Rows[i].Cells["checkbox"].Style.ForeColor = Color.Red;
                    dataGridView1.Rows[i].Cells["MeetingName"].Style.ForeColor = Color.Red;
                    dataGridView1.Rows[i].Cells["StartTime"].Style.ForeColor = Color.Red;
                    dataGridView1.Rows[i].Cells["EndTime"].Style.ForeColor = Color.Red;
                    dataGridView1.Rows[i].Cells["MeetingStatus"].Style.ForeColor = Color.Red;
                    dataGridView1.Rows[i].Cells["MeetingType"].Style.ForeColor = Color.Red;
                    dataGridView1.Rows[i].Cells["MainMeetingRoom"].Style.ForeColor = Color.Red;
                    dataGridView1.Rows[i].Cells["ServiceKey"].Style.ForeColor = Color.Red;
                }
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
                if (_meetingData[currentMeetingId].StatusCode == 3)
                {
                    MessageBox.Show("会议正在进行，不能修改！");
                    return;
                }
                var appt = OutlookFacade.Instance().CalendarFolder.AppointmentCollection[currentMeetingId];
                appt.Display();
            }
            else
            {
                MessageBox.Show("请选择一个会议！");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(currentMeetingId))
            {
                // add by robin at 20141231 start 
                //if (MessageBox.Show("你确定要删除该会议?", "提示信息", MessageBoxButtons.YesNo) != DialogResult.Yes)
                //{
                //    return;
                //}
                // add by robin at 20141231 end 

                if (_meetingData[currentMeetingId].StatusCode == 3)
                {
                    MessageBox.Show("会议正在进行，不能删除！");
                    return;
                }
                var appt = OutlookFacade.Instance().CalendarFolder.AppointmentCollection[currentMeetingId];
                appt.Delete();

                if (!OutlookFacade.Instance().CalendarFolder.CalendarDataManager.MeetingDetailDataLocal.ContainsKey(currentMeetingId))
                {
                    _meetingData.Remove(currentMeetingId);
                    currentMeetingId = null;
                }
                this.SetDataSource(_meetingData.Values.ToList());
                
            }
            else
            {
                MessageBox.Show("请选择一个会议！");
            }
        }

        private void MeetingCenterForm_Activated(object sender, EventArgs e)
        {
            //this.SetDataSource(_meetingData.Values.ToList());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "正在同步...";
            lblMessage.ForeColor = Color.Red;

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

            query.ConfType = this.comboBoxMideaType.SelectedIndex == 0 ? "-1" : this.comboBoxMideaType.SelectedIndex.ToString();

            query.MeetingName = this.txtMeetingName.Text;
            query.RoomName = this.txtRoomName.Text;
            query.Alias = this.txtAlias.Text;
            query.ServiceKey = this.txtServiceKey.Text;

            var task = Task.Factory.StartNew(() =>
            {
                return this.SearchMeetingList(query);
            });

            task.Wait();

            lblMessage.Text = string.Empty;
            List<SVCMMeeting> list = task.Result;

            if (list != null)
            {
                this.SetDataSource(list);
            }
            else
            {
                MessageBox.Show("获取会议列表失败！");
            }

            //Func<MeetingListQuery,List<SVCMMeeting>> func = this.SearchMeetingList;

            //Task.Factory.FromAsync<MeetingListQuery, List<SVCMMeeting>>(func.BeginInvoke, func.EndInvoke, query, null).ContinueWith((result) =>
            //{
            //    lblMessage.Text = string.Empty;
            //    List<SVCMMeeting> list = result.Result;

            //    if (list != null)
            //    {
            //        this.SetDataSource(list);
            //    }
            //    else
            //    {
            //        MessageBox.Show("获取会议列表失败！");
            //    }

            //});
        }

        private List<SVCMMeeting> SearchMeetingList(MeetingListQuery query)
        {
            List<SVCMMeeting> list;
            if (!ClientServiceFactory.Create().TryGetMeetingList(query, OutlookFacade.Instance().Session, out list))
            {
                list = null;
            }

            return list;
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(currentMeetingId))
            {
                SVCMMeetingDetail detail;
                if (ClientServiceFactory.Create().TryGetMeetingDetail(currentMeetingId, OutlookFacade.Instance().Session, out detail))
                {
                    MeetingDetailForm form = new MeetingDetailForm();
                    form.MeetingDetail = detail;
                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("获取会议详情失败！");
                }
            }
            else
            {
                MessageBox.Show("请选择一个会议！");
            }
        }
    }
}
