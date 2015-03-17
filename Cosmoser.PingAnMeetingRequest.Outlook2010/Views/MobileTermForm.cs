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
    public partial class MobileTermForm : Form,IMobileTermView
    {
        public MobileTermForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public List<Common.Model.MobileTerm> MobileTermList
        {
            get;

            set;

        }

        private List<MobileTerm> _allTermList;

        public DialogResult Display()
        {
            return this.ShowDialog();
        }

        private void MobileTermForm_Load(object sender, EventArgs e)
        {
            List<MobileTerm> all;

            if (ClientServiceFactory.Create().TryGetMobileTermList(OutlookFacade.Instance().Session, out all))
            {
                this._allTermList = new List<MobileTerm>();
                foreach (var item in all)
                {
                    if (!this.MobileTermList.Exists(x => x.RoomId == item.RoomId))
                        this._allTermList.Add(item);
                }

                listBoxAvailable.DataSource = this._allTermList;
                listBoxSelected.DataSource = this.MobileTermList;

                this.lblAvailable.Text = string.Format("待选移动终端(共{0}个)", this._allTermList.Count);
                this.lblSelected.Text = string.Format("已选移动终端(共{0}个)", this.MobileTermList.Count);
            }
            else
            {
                MessageBox.Show("获取移动终端失败");
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (listBoxAvailable.SelectedItem == null)
            {
                MessageBox.Show("请在可选列表里选择一个！");
                return;
            }

            this.DoAddItems();

            this.lblAvailable.Text = string.Format("待选移动终端(共{0}个)", this._allTermList.Count);
            this.lblSelected.Text = string.Format("已选移动终端(共{0}个)", this.MobileTermList.Count);
        }

        private void DoAddItems()
        {
            foreach (var item in this.listBoxAvailable.SelectedItems)
            {
                MobileTerm room = item as MobileTerm;

                if (room != null && !this.MobileTermList.Exists(x => x.RoomId == room.RoomId))
                {
                    this.MobileTermList.Add(room);
                    this._allTermList.Remove(room);
                }
            }

            this.listBoxSelected.DataSource = null;
            this.listBoxAvailable.DataSource = null;
            this.listBoxSelected.DataSource = this.MobileTermList;
            this.listBoxAvailable.DataSource = this._allTermList;

            if (this.listBoxAvailable.SelectedItems != null)
                this.listBoxAvailable.SelectedItems.Clear();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBoxSelected.SelectedItem == null)
            {
                MessageBox.Show("请在已选列表里选择一个！");
                return;
            }

            foreach (var item in listBoxSelected.SelectedItems)
            {
                MobileTerm term = item as MobileTerm;
                this.MobileTermList.Remove(term);
                if (!this._allTermList.Contains(term))
                {
                    this._allTermList.Add(term);
                }
            }

            listBoxAvailable.DataSource = null;
            listBoxSelected.DataSource = null;
            listBoxAvailable.DataSource = this._allTermList;
            listBoxSelected.DataSource = this.MobileTermList;

            this.lblAvailable.Text = string.Format("待选移动终端(共{0}个)", this._allTermList.Count);
            this.lblSelected.Text = string.Format("已选移动终端(共{0}个)", this.MobileTermList.Count);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void txtAvailabelSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.listBoxAvailable.SelectedItems.Clear();
                string str = this.txtAvailabelSearch.Text.Trim();

                if (!string.IsNullOrEmpty(str))
                {
                    for (int i = 0; i < this.listBoxAvailable.Items.Count; i++)
                    {
                        if (this.listBoxAvailable.Items[i].ToString().Contains(str))
                            this.listBoxAvailable.SetSelected(i, true);
                    }
                }
            }
        }

        private void txtSelectedSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.listBoxSelected.SelectedItems.Clear();
                string str = this.txtSelectedSearch.Text.Trim();

                if (!string.IsNullOrEmpty(str))
                {
                    for (int i = 0; i < this.listBoxSelected.Items.Count; i++)
                    {
                        if (this.listBoxSelected.Items[i].ToString().Contains(str))
                            this.listBoxSelected.SetSelected(i, true);
                    }
                }
            }
        }
    }
}
