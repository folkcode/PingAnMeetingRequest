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

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Views
{
    public partial class MeetingDateSearchForm : Form
    {
        public DateTime SelectedDate
        {
            get;
            set;
        }
        private static ILog logger = IosLogManager.GetLogger(typeof(MeetingDateSearchForm));
        public MeetingDateSearchForm()
        {
            InitializeComponent();
        }

        private void MeetingDateSearchForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.dateTimePickerSearchDate.Value = this.SelectedDate.Date;

                DateTime start = DateTime.Today;
                DateTime endTime = DateTime.Today.AddDays(1);

                while (start < endTime)
                {
                    this.comboBoxStartTime.Items.Add(start.ToString("HH:mm"));
                    this.comboBoxEndTime.Items.Add(start.ToString("HH:mm"));

                    start = start.AddMinutes(30);
                }

                this.comboBoxStartTime.SelectedItem = "08:00";
                this.comboBoxEndTime.SelectedItem = "20:00";

                List<MeetingSeries> seriesList;

                if (ClientServiceFactory.Create().TryGetSeriesList(OutlookFacade.Instance().Session, out seriesList))
                {
                    foreach (var item in seriesList)
                    {
                        this.comboBoxSeries.Items.Add(item);
                    }

                    this.comboBoxSeries.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("获取系列信息失败，请重试！");
                }

                this.comboBoxLevel.Items.Add(new RoomLevel()
                {
                    LevelName = "--全部--",
                    LevelId = "0"
                });

                this.comboBoxLevel.Items.Add(new RoomLevel()
                {
                    LevelName = "总部级",
                    LevelId = "1"
                });

                this.comboBoxLevel.Items.Add(new RoomLevel()
                {
                    LevelName = "二级机构",
                    LevelId = "2"
                });

                this.comboBoxLevel.Items.Add(new RoomLevel()
                {
                    LevelName = "三级机构",
                    LevelId = "3"
                });

                this.comboBoxLevel.Items.Add(new RoomLevel()
                {
                    LevelName = "四级机构",
                    LevelId = "4"
                });

                this.comboBoxLevel.SelectedIndex = 0;

                this.rbStatusAll.Checked = true;
                this.rbTypeAll.Checked = true;

                this.comboBoxCapacity.Items.Add(new CapacityInfo() { Label = "--全部--", Value = string.Empty });
                this.comboBoxCapacity.Items.Add(new CapacityInfo()
                {
                    Label = "0< 人数 <=10",
                    Value = "0,10"
                });
                this.comboBoxCapacity.Items.Add(new CapacityInfo()
                {
                    Label = "10< 人数 <=25",
                    Value = "10,25"
                });
                this.comboBoxCapacity.Items.Add(new CapacityInfo()
                {
                    Label = "25< 人数 <=40",
                    Value = "25,40"
                });
                this.comboBoxCapacity.Items.Add(new CapacityInfo()
                {
                    Label = "40< 人数",
                    Value = "40,0"
                });

                this.comboBoxCapacity.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                logger.Error("初始化查询条件错误！", ex);
            }
        }

        private void comboBoxSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegionCatagory rc;
            MeetingSeries s = this.comboBoxSeries.SelectedItem as MeetingSeries;
            RegionCatagoryQuery query = new RegionCatagoryQuery();
            query.SeriesId = s.Id;
            query.ProvinceCode = "0";
            query.CityCode = "0";
            query.BoroughCode = "0";
            if (ClientServiceFactory.Create().TryGetRegionCatagory(query, OutlookFacade.Instance().Session, out rc))
            {
                this.comboBoxProvince.Items.Clear();
                foreach (var item in rc.ProvinceList)
                {
                    this.comboBoxProvince.Items.Add(item);
                }

                this.comboBoxProvince.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("获取机构变更信息失败！");
            }
        }

        private void comboBoxProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegionCatagory rc;
            MeetingSeries s = this.comboBoxSeries.SelectedItem as MeetingSeries;
            RegionInfo p = this.comboBoxProvince.SelectedItem as RegionInfo;
            RegionCatagoryQuery query = new RegionCatagoryQuery();
            query.SeriesId = s.Id;
            query.ProvinceCode = p.Code;
            query.CityCode = "0";
            query.BoroughCode = "0";
            if (ClientServiceFactory.Create().TryGetRegionCatagory(query, OutlookFacade.Instance().Session, out rc))
            {
                this.comboBoxCity.Items.Clear();
                foreach (var item in rc.CityList)
                {
                    this.comboBoxCity.Items.Add(item);
                }

                this.comboBoxCity.SelectedIndex = 0;

                if (this.comboBoxBobough.Items.Count == 0)
                {
                    this.comboBoxBobough.Items.Add(new RegionInfo()
                    {
                        Code = "0",
                        Name = "--全部--"
                    });
                    this.comboBoxBobough.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("获取机构变更信息失败！");
            }
        }

        private void comboBoxCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.comboBoxProvince.SelectedIndex == 0)
            //    return;

            RegionCatagory rc;
            MeetingSeries s = this.comboBoxSeries.SelectedItem as MeetingSeries;
            RegionInfo p = this.comboBoxProvince.SelectedItem as RegionInfo;
            RegionInfo c = this.comboBoxCity.SelectedItem as RegionInfo;
            RegionCatagoryQuery query = new RegionCatagoryQuery();
            query.SeriesId = s.Id;
            query.ProvinceCode = p.Code;
            query.CityCode = c.Code;
            query.BoroughCode = "0";
            if (ClientServiceFactory.Create().TryGetRegionCatagory(query, OutlookFacade.Instance().Session, out rc))
            {
                this.comboBoxBobough.Items.Clear();
                foreach (var item in rc.BoroughList)
                {
                    this.comboBoxBobough.Items.Add(item);
                }

                this.comboBoxBobough.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("获取机构变更信息失败！");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.DoSearch(false);
        }

        private void DoSearch(bool isAll)
        {
            if (this.comboBoxLevel.SelectedItem == null
                || this.comboBoxSeries.SelectedItem == null
                || this.comboBoxCapacity.SelectedItem == null
                || this.comboBoxProvince.SelectedItem == null
                || this.comboBoxCity.SelectedItem == null
                || this.comboBoxBobough.SelectedItem == null)
            {
                MessageBox.Show("参数不全，不能查询！");
                return;
            }

            // add by robin start
            if (this.comboBoxStartTime.SelectedIndex >= this.comboBoxEndTime.SelectedIndex)
            {
                MessageBox.Show("结束时间必须晚于开始时间，请重新选择！");
                this.comboBoxEndTime.SelectedIndex = this.comboBoxStartTime.SelectedIndex < this.comboBoxEndTime.Items.Count ? this.comboBoxStartTime.SelectedIndex + 1 : this.comboBoxStartTime.SelectedIndex;
                return;
            }
            // add by robin end

            MeetingSchedulerQuery query = new MeetingSchedulerQuery();

            query.RoomName = this.txtRoomName.Text.ToString().Trim();
            query.LevelId = (this.comboBoxLevel.SelectedItem as RoomLevel).LevelId;
            query.SeriesId = (this.comboBoxSeries.SelectedItem as MeetingSeries).Id;
            query.BoardRoomState = this.rbStatusAll.Checked ? 0 : 1;
            if (this.rbTypeAll.Checked)
                query.RoomIfTerminal = 2;
            else if (this.rbTypeVideo.Checked)
                query.RoomIfTerminal = 1;
            else
                query.RoomIfTerminal = 0;
            query.Capacity = (this.comboBoxCapacity.SelectedItem as CapacityInfo).Value;
            query.ProvinceCode = (this.comboBoxProvince.SelectedItem as RegionInfo).Code;
            query.CityCode = (this.comboBoxCity.SelectedItem as RegionInfo).Code;
            query.BoroughCode = (this.comboBoxBobough.SelectedItem as RegionInfo).Code;
            query.DataAll = isAll ? 1 : 0;
            query.StartTime = DateTime.Parse(this.dateTimePickerSearchDate.Value.ToString("yyyy-MM-dd ") + this.comboBoxStartTime.SelectedItem.ToString());
            query.EndTime = DateTime.Parse(this.dateTimePickerSearchDate.Value.ToString("yyyy-MM-dd ") + this.comboBoxEndTime.SelectedItem.ToString());

            List<MeetingScheduler> list;

            if (ClientServiceFactory.Create().TryGetMeetingScheduler(query, OutlookFacade.Instance().Session, out list))
            {
                List<RoomScheduler> rlist = RoomScheduler.PopulateFromMeetingScheduler(list, query.StartTime, query.EndTime, query.BoardRoomState == 0 ? true : false);

                this.SetDataSource(rlist, query.StartTime, query.EndTime);
            }
            else
            {
                MessageBox.Show("查询会议日程失败，请重试！");
            }
        }

        private void SetDataSource(List<RoomScheduler> list, DateTime startTime, DateTime endTime)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = list;

            this.dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("SeriesName", "系列");
            dataGridView1.Columns.Add("RoomName", "会议室");
            dataGridView1.Columns.Add("Type", "类型");
            int n = (int)(endTime - startTime).TotalMinutes / 30;
            if (list.Count > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    dataGridView1.Columns.Add("c" + i, startTime.AddMinutes(30 * i).ToString("HH:mm"));
                    dataGridView1.Columns["c" + i].Width = 40;
                }
            }

            for (int i = 0; i < list.Count; i++)
            {

                dataGridView1.Rows[i].Cells["SeriesName"].Value = list[i].SeriesName;
                dataGridView1.Rows[i].Cells["RoomName"].Value = list[i].RoomName;
                dataGridView1.Rows[i].Cells["Type"].Value = list[i].Type;

                for (int j = 0; j < n; j++)
                {
                    dataGridView1.Rows[i].Cells["c" + j].Style.BackColor = list[i].TimeSheduler[j];
                }
            }
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            this.DoSearch(true);
        }

        private void comboBoxLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RegionCatagory rc;
                MeetingSeries s = this.comboBoxSeries.SelectedItem as MeetingSeries;
                RegionCatagoryQuery query = new RegionCatagoryQuery();
                query.SeriesId = "0";
                query.ProvinceCode = "0";
                query.CityCode = "0";
                query.BoroughCode = "0";
                if (ClientServiceFactory.Create().TryGetRegionCatagory(query, OutlookFacade.Instance().Session, out rc))
                {
                    this.comboBoxSeries.Items.Clear();
                    foreach (var item in rc.SeriesList)
                    {
                        this.comboBoxSeries.Items.Add(item);
                    }

                    this.comboBoxSeries.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("获取机构变更信息失败！");
                }
            }
            catch (Exception ex)
            {
                logger.Error("comboBoxLevel_SelectedIndexChanged error", ex);
            }
        }

        private void comboBoxEndTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            // add by robin start
            if (this.comboBoxStartTime.SelectedIndex >= this.comboBoxEndTime.SelectedIndex)
            {
                MessageBox.Show("结束时间必须晚于开始时间，请重新选择！");
                this.comboBoxEndTime.SelectedIndex = this.comboBoxStartTime.SelectedIndex < this.comboBoxEndTime.Items.Count ? this.comboBoxStartTime.SelectedIndex + 1 : this.comboBoxStartTime.SelectedIndex;
                return;
            }
            // add by robin end
        }
    }
}
