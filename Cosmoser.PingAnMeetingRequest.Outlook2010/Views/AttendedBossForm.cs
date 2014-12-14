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

        public DialogResult Display()
        {
           return  this.ShowDialog();
        }
    }
}
