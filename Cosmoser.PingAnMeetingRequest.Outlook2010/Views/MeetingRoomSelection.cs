using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        public DialogResult Display()
        {
            return this.ShowDialog();
        }
    }
}
