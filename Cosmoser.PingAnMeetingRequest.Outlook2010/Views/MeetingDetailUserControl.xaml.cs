using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cosmoser.PingAnMeetingRequest.Common.Model;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Views
{
    /// <summary>
    /// Interaction logic for MeetingDetailUserControl.xaml
    /// </summary>
    public partial class MeetingDetailUserControl : UserControl
    {
        public SVCMMeetingDetail MeetingDetail
        {
            get;
            set;
        }

        public MeetingDetailUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.lblMeetingName.Content = this.MeetingDetail.Name;
            this.lblStartTime.Content = this.MeetingDetail.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.lblEndTime.Content = this.MeetingDetail.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.lblMeetingType.Content = this.MeetingDetail.ConfMideaType ==  MideaType.Local?"本地普通会议":"视频会议";
            this.lblStatus.Content = this.MeetingDetail.StatusStr;
            this.lblIpCount.Content = this.MeetingDetail.IpTelephoneNumber;
            this.lblAttendNum.Content = this.MeetingDetail.ParticipatorNumber;
            this.lblSeries.Content = this.MeetingDetail.Series.Name;
            this.lblAccount.Content = this.MeetingDetail.AccountName;
            this.lblAccountPhone.Content = this.MeetingDetail.Phone;
            this.lblAccountDepartment.Content = this.MeetingDetail.Department;
            this.lblLeader.Content = this.MeetingDetail.LeaderNameListStr;
            this.lblLeaderRoom.Content = this.MeetingDetail.LeaderRoom;
            this.lblMemo.Content = this.MeetingDetail.Memo;

            this.dataGridRoomList.DataContext = this.MeetingDetail.Rooms;
            
        }
    }
}
