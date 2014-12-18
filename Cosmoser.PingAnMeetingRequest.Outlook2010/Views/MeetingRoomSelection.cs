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
    public partial class MeetingRoomSelection : Form, IMeetingRoomView
    {
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

        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }

        public DialogResult Display()
        {
            return this.ShowDialog();
        }

        private void MeetingRoomSelection_Load(object sender, EventArgs e)
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
            listBoxAvailableRoom.Items.Clear();
            if (ClientServiceFactory.Create().TryGetMeetingRoomList(query, OutlookFacade.Instance().Session, out _availableroomList))
            {
                foreach (var item in _availableroomList)
                {
                    listBoxAvailableRoom.Items.Add(item);
                }
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
                foreach (var item in this.listBoxAvailableRoom.SelectedItems)
                {
                    MeetingRoom room = item as MeetingRoom;

                    if (room != null && !this.MeetingRoomList.Exists(x => x.RoomId == room.RoomId))
                        this.MeetingRoomList.Add(room);

                    this.listBoxSelectedRooms.DataSource = null;
                    this.listBoxSelectedRooms.DataSource = this.MeetingRoomList;
                }
                
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
                }

                this.listBoxSelectedRooms.DataSource = null;
                this.listBoxSelectedRooms.DataSource = this.MeetingRoomList;
            }
            else
            {
                MessageBox.Show("请在已选会议室里选择会议室！");
            }
        }

        bool select2all = false;
        private void btnSelectAllOnSecondLevel_Click(object sender, EventArgs e)
        {
            select2all = true;
            this.listBoxLevel.SelectedIndex = 1;

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
            select1all = true;
            this.listBoxLevel.SelectedIndex = 0;

            this.SelectAll();
        }

        private void btnMainRoomSetting_Click(object sender, EventArgs e)
        {
            if (this.listBoxSelectedRooms.SelectedIndex > -1)
            {
                this.MainRoom = this.listBoxSelectedRooms.SelectedItem as MeetingRoom;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
