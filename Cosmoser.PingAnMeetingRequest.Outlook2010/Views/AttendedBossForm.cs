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
    public partial class AttendedBossForm : Form, IAttendedLeadersView
    {
        public AttendedBossForm()
        {
            InitializeComponent();
        }

        public List<Common.Model.MeetingLeader> LeaderList
        {
            get;
            set;
        }

        public string LeaderRoom
        {
            get;
            set;
        }

        private List<MeetingLeader> _allLeaders;

        public DialogResult Display()
        {
           return  this.ShowDialog();
        }

        private void AttendedBossForm_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = this.LeaderRoom;
            if (ClientServiceFactory.Create().TryGetLeaderList(OutlookFacade.Instance().Session, out this._allLeaders))
            {
                var list1 = this._allLeaders.FindAll(x => x.LeaderPRI.StartsWith("1"));

                //主任层
                foreach (var item in list1)
                {
                    this.listView1.Items.Add(new ListViewItem()
                    {
                        Text = item.Name,
                        Tag = item.UserName,
                        Checked = this.LeaderList.Exists(x=>x.UserName == item.UserName)
                    });
                }

                //常委层 
                var list2 = this._allLeaders.FindAll(x => x.LeaderPRI.StartsWith("2"));
                foreach (var item in list2)
                {
                    this.listView2.Items.Add(new ListViewItem()
                    {
                        Text = item.Name,
                        Tag = item.UserName,
                        Checked = this.LeaderList.Exists(x => x.UserName == item.UserName)
                    });
                }

                //委员
                var list3 = this._allLeaders.FindAll(x => x.LeaderPRI.StartsWith("3"));
                foreach (var item in list3)
                {
                    this.listView3.Items.Add(new ListViewItem()
                    {
                        Text = item.Name,
                        Tag = item.UserName,
                        Checked = this.LeaderList.Exists(x => x.UserName == item.UserName)
                    });
                }

            }
            else
            {
                MessageBox.Show("获取领导数据失败");
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.LeaderList.Clear();

            foreach (var item in listView1.Items)
            {
                ListViewItem viewItem = item as ListViewItem;
                if (viewItem.Checked)
                {
                   var leader = this._allLeaders.Find(x => x.UserName == viewItem.Tag.ToString());

                   this.LeaderList.Add(leader);
                }
            }

            foreach (var item in listView2.Items)
            {
                ListViewItem viewItem = item as ListViewItem;
                if (viewItem.Checked)
                {
                    var leader = this._allLeaders.Find(x => x.UserName == viewItem.Tag.ToString());

                    this.LeaderList.Add(leader);
                }
            }

            foreach (var item in listView3.Items)
            {
                ListViewItem viewItem = item as ListViewItem;
                if (viewItem.Checked)
                {
                    var leader = this._allLeaders.Find(x => x.UserName == viewItem.Tag.ToString());

                    this.LeaderList.Add(leader);
                }
            }

            this.LeaderRoom = this.textBox1.Text.Trim();

            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
