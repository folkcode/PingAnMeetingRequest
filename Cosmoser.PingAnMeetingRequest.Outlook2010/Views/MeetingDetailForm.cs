using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cosmoser.PingAnMeetingRequest.Common.Model;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Views
{
    public partial class MeetingDetailForm : Form
    {
        public SVCMMeetingDetail MeetingDetail
        {
            get;
            set;
        }
        public MeetingDetailForm()
        {
            InitializeComponent();
        }

        private void MeetingDetailForm_Load(object sender, EventArgs e)
        {
            var userControl = new MeetingDetailUserControl();
            userControl.MeetingDetail = this.MeetingDetail;

            this.elementHost1.Child = userControl;
        }


    }
}
