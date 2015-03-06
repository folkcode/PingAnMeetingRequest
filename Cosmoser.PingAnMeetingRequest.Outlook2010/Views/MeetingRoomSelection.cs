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
    public partial class MeetingRoomSelection : Form, IMeetingRoomView
    {
        private static ILog logger = IosLogManager.GetLogger(typeof( MeetingRoomSelection));
        public MeetingRoomSelection()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public List<Common.Model.MeetingRoom> MeetingRoomList
        {
            get;
            set;
        }

        public Common.Model.MeetingRoom MainRoom
        {
            get;
            set;
        }

        public MideaType ConfType
        {
            get;
            set;
        }

        public VideoSet VideoSet
        {
            get;
            set;
        }

        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }

        public DialogResult Display()
        {
            return this.ShowDialog();
        }

        private void MeetingRoomSelection_Load(object sender, EventArgs e)
        {
            try
            {
                List<MeetingSeries> seriesList;

                if (ClientServiceFactory.Create().TryGetSeriesList(OutlookFacade.Instance().Session, out seriesList))
                {
                    foreach (var item in seriesList)
                    {
                        this.listBoxMeetingRoom.Items.Add(item);
                    }
                }
                else
                {
                    MessageBox.Show("获取会议室分组信息失败，请重试！");
                }

                this.listBoxLevel.Items.Add(new RoomLevel()
                {
                    LevelName = "总部级",
                    LevelId = "1,1"
                });

                this.listBoxLevel.Items.Add(new RoomLevel()
                {
                    LevelName = "二级机构",
                    LevelId = "1,2"
                });

                this.listBoxLevel.Items.Add(new RoomLevel()
                {
                    LevelName = "三级机构",
                    LevelId = "1,3"
                });

                this.listBoxLevel.Items.Add(new RoomLevel()
                {
                    LevelName = "四级机构",
                    LevelId = "1,4"
                });

                this.listBoxLevel.SelectedIndex = 0;
                this.listBoxMeetingRoom.SelectedIndex = 0;

                this.listBoxSelectedRooms.DataSource = null;
                this.listBoxSelectedRooms.DataSource = this.MeetingRoomList;

                //本地会议不需要设置主会场
                if (this.ConfType == MideaType.Local)
                    this.btnMainRoomSetting.Enabled = false;

                if (this.MainRoom != null)
                    this.lblMainRoom.Text = "主会场:" + this.MainRoom.Name;
                else
                    this.lblMainRoom.Text = "主会场：无";
            }
            catch (Exception ex)
            {
                logger.Error("MeetingRoomSelection_Load", ex);
                MessageBox.Show("");
            }
        }

        private void listBoxMeetingRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxLevel.SelectedIndex > -1)
            {
                this.LoadRoomList((this.listBoxLevel.SelectedItem as RoomLevel).LevelId);
            }
        }

        private void listBoxLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxMeetingRoom.SelectedIndex > -1)
            {
                this.LoadRoomList((this.listBoxLevel.SelectedItem as RoomLevel).LevelId);

                if (select2all || select1all)
                {
                    this.SelectAll();
                    select2all = false;
                    select1all = false;
                }
            }
        }

        List<MeetingRoom> _availableroomList;

        private void LoadRoomList(string leverId)
        {
            MeetingRoomListQuery query = new MeetingRoomListQuery();
            query.LevelId = leverId;// (this.listBoxLevel.SelectedItem as ListViewItem).Tag.ToString();
            query.SeriesId = (this.listBoxMeetingRoom.SelectedItem as MeetingSeries).Id;
            query.ConfType = this.ConfType;
            query.StartTime = this.StarTime;
            query.EndTime = this.EndTime;

            this._availableroomList = null;

            if (ClientServiceFactory.Create().TryGetMeetingRoomList(query, OutlookFacade.Instance().Session, out _availableroomList))
            {
                List<MeetingRoom> removedRooms = new List<MeetingRoom>();
                foreach (var item in _availableroomList)
                {
                    if (this.MeetingRoomList.Exists(x => x.RoomId == item.RoomId))
                        removedRooms.Add(item);
                }

                foreach (var item in removedRooms)
                {
                    this._availableroomList.Remove(item);
                }

                this.listBoxSelectedRooms.DataSource = null;
                this.listBoxAvailableRoom.DataSource = null;
                this.listBoxSelectedRooms.DataSource = this.MeetingRoomList;
                this.listBoxAvailableRoom.DataSource = this._availableroomList;
            }
            else
            {
                MessageBox.Show("获取会议室信息失败，请重试！");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.listBoxAvailableRoom.SelectedIndex > -1)
            {
                if (this.listBoxSelectedRooms.Items.Count > 0 && this.ConfType == MideaType.Local)
                {
                    MessageBox.Show("本地会议只能选一个会议室！");
                    return;
                }

                foreach (var item in this.listBoxAvailableRoom.SelectedItems)
                {
                    MeetingRoom room = item as MeetingRoom;

                    if (room != null && !this.MeetingRoomList.Exists(x => x.RoomId == room.RoomId))
                    {
                        this.MeetingRoomList.Add(room);
                        this._availableroomList.Remove(room);
                    }
                }

                this.listBoxSelectedRooms.DataSource = null;
                this.listBoxAvailableRoom.DataSource = null;
                this.listBoxSelectedRooms.DataSource = this.MeetingRoomList;
                this.listBoxAvailableRoom.DataSource = this._availableroomList;

                if (this.listBoxAvailableRoom.SelectedItems != null)
                    this.listBoxAvailableRoom.SelectedItems.Clear();
            }
            else
            {
                MessageBox.Show("请在待选会议室里选择一个会议室！");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.listBoxSelectedRooms.SelectedIndex > -1)
            {
                foreach (var item in this.listBoxSelectedRooms.SelectedItems)
                {
                    var room = item as MeetingRoom;
                    this.MeetingRoomList.Remove(room);
                    this._availableroomList.Insert(0,room);
                    if (this.MainRoom != null && this.MainRoom.RoomId == room.RoomId)
                        this.MainRoom = null;
                }

                if (this.listBoxSelectedRooms.SelectedItems != null)
                    this.listBoxSelectedRooms.SelectedItems.Clear();
                this.listBoxSelectedRooms.DataSource = null;
                this.listBoxAvailableRoom.DataSource = null;
                this.listBoxSelectedRooms.DataSource = this.MeetingRoomList;
                this.listBoxAvailableRoom.DataSource = this._availableroomList;
                
                
            }
            else
            {
                MessageBox.Show("请在已选会议室里选择会议室！");
            }
        }

        bool select2all = false;
        private void btnSelectAllOnSecondLevel_Click(object sender, EventArgs e)
        {
            //select2all = true;
            //this.listBoxLevel.SelectedIndex = 1;

            this.LoadRoomList("-2");
            this.SelectAll();
        }

        private void SelectAll()
        {
            for (int i = 0; i < this.listBoxAvailableRoom.Items.Count; i++)
            {
                this.listBoxAvailableRoom.SetSelected(i, true);
            }
        }

        bool select1all = false;
        private void btnSelectAllOnCountry_Click(object sender, EventArgs e)
        {
            //select1all = true;
            //this.listBoxLevel.SelectedIndex = 0;

            this.LoadRoomList("-1");
            this.SelectAll();
        }

        private void btnMainRoomSetting_Click(object sender, EventArgs e)
        {

            if (this.ConfType == MideaType.Local)
            {
                MessageBox.Show("本地会议不需要设置主会场！");
                return;
            }

            if (this.listBoxSelectedRooms.SelectedItems != null && this.listBoxSelectedRooms.SelectedItems.Count > 1)
            {
                MessageBox.Show("请选择一个会议室作为主会场！");
                return;
            }

            if (this.listBoxSelectedRooms.SelectedIndex > -1)
            {
                this.MainRoom = this.listBoxSelectedRooms.SelectedItem as MeetingRoom;
                //this.listBoxSelectedRooms.DrawMode
                //Graphics g = e.Graphics;//获取Graphics对象。
                //Rectangle bound = e.Bounds;//获取当前要绘制的行的一个矩形范围。

                this.lblMainRoom.Text = "主会场:" + this.MainRoom.Name;
                MessageBox.Show("主会场 " + this.MainRoom.Name + " 已设置.");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
