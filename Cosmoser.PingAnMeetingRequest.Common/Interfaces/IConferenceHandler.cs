using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cosmoser.PingAnMeetingRequest.Common.Model;

namespace Cosmoser.PingAnMeetingRequest.Common.Interfaces
{
    public interface IConferenceHandler
    {
        bool Login(ref HandlerSession session);
        bool BookingMeeting(SVCMMeetingDetail meetingDetail, HandlerSession session, out string error);
        bool DeleteMeeting(string conferId, HandlerSession session);
        bool UpdateMeeting(SVCMMeetingDetail meetingDetail, string operateType, HandlerSession session, out string error, out string errorCode);
        bool TryGetMeetingDetail(string meetingId, HandlerSession session, out SVCMMeetingDetail meetingDetail);
        bool TryGetMeetingList(MeetingListQuery query, HandlerSession session, out List<SVCMMeeting> meetingList);
        bool TryGetSeriesList(HandlerSession session, out List<MeetingSeries> seriesList);
        bool TryGetMeetingRoomList(MeetingRoomListQuery query, HandlerSession session, out List<MeetingRoom> roomList);
        bool TryGetLeaderList(HandlerSession session, out List<MeetingLeader> leaderList);
        bool TryGetMobileTermList(HandlerSession session, out List<MobileTerm> mobileTermList);
        bool TryGetRegionCatagory(RegionCatagoryQuery query, HandlerSession session, out RegionCatagory regionCatagory);
        bool TryGetMeetingScheduler(MeetingSchedulerQuery query, HandlerSession session, out List<MeetingScheduler> schedulerList);
    }
}
