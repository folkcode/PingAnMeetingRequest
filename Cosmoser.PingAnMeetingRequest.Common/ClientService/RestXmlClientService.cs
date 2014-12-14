using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cosmoser.PingAnMeetingRequest.Common.Interfaces;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using Cosmoser.PingAnMeetingRequest.Common.Model;

namespace Cosmoser.PingAnMeetingRequest.Common.ClientService
{
    public class RestXmlClientService : IConferenceHandler
    {
        private DataTransform _dataTransform = new DataTransform();
        private RestXMLApiClient _client = new RestXMLApiClient();
        
        public bool Login(ref Model.HandlerSession session)
        {
            try
            {
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><login><userName>{0}</userName><userType>1</userType></login>", session.UserName);

                var response = this._client.DoHttpWebRequest(session.BaseUrl + "login", xmlData);

                string status = response.SelectSingleNode("login").SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    session.Token = response.SelectSingleNode("login").SelectSingleNode("token").InnerText;
                    session.IsActive = true;
                    //每次登陆都需要重设messageId
                    session.ResetMessageId();
                    return true;
                }
            }
            catch
            {

            }

            session.IsActive = false;
            return false;
        }

        public bool BookingMeeting(Model.SVCMMeetingDetail meetingDetail, Model.HandlerSession session)
        {
            try
            {
                string xmlData = this._dataTransform.GetXmlDataFromMeetingDetail(meetingDetail, session);

                var response = this._client.DoHttpWebRequest(session.BaseUrl + "startConfer", xmlData);

                string status = response.SelectSingleNode("startConfer").SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    meetingDetail.Id = response.SelectSingleNode("startConfer").SelectSingleNode("conferId").InnerText;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

            }

            return false;
        }

        public bool DeleteMeeting(string conferId, Model.HandlerSession session)
        {
            try
            {
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><deleteConfer><messageId>{0}</messageId><token>{1}</token><conferId>{2}</conferId></deleteConfer>", session.MessageId, session.Token,conferId);
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "deleteConfer", xmlData);

                XmlNode root = response.SelectSingleNode("deleteConfer");
                string status = root.SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public bool UpdateMeeting(Model.SVCMMeetingDetail meetingDetail, Model.HandlerSession session)
        {
            try
            {
                string xmlData = this._dataTransform.GetXmlDataForUpdatingMeeting(meetingDetail, session);

                var response = this._client.DoHttpWebRequest(session.BaseUrl + "updateConfer", xmlData);

                string status = response.SelectSingleNode("updateConfer").SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

            }

            return false;
        }

        public bool TryGetSeriesList(Model.HandlerSession session, out List<Model.MeetingSeries> seriesList)
        {
            seriesList = new List<Model.MeetingSeries>();
            try
            {
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><seriesList><messageId>{0}</messageId><token>{1}</token></seriesList>", session.MessageId, session.Token);
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "seriesList", xmlData);

                XmlNode root = response.SelectSingleNode("seriesList");
                string status = root.SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    foreach (var item in root.SelectNodes("series"))
                    {
                        var node = item as XmlNode;
                        var series = new MeetingSeries();
                        series.Id = node.SelectSingleNode("seriesId").InnerText;
                        series.Name = node.SelectSingleNode("seriesName").InnerText;

                        seriesList.Add(series);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public bool TryGetMeetingRoomList(MeetingRoomListQuery query, Model.HandlerSession session, out List<Model.MeetingRoom> roomList)
        {
            roomList = new List<MeetingRoom>();

            try
            {
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><roomList><messageId>{0}</messageId><token>{1}</token><seriesId>{2}</seriesId><levelId>{3}</levelId><confType>{4}</confType><startTime>{5}</startTime><endTime>{6}</endTime></roomList>",
                                                session.MessageId,
                                                session.Token,
                                                query.SeriesId,
                                                query.LevelId,
                                                (int)query.ConfType,
                                                query.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                query.EndTime.ToString("yyyy-MM-dd HH:mm:ss")
                                                );

                var response = this._client.DoHttpWebRequest(session.BaseUrl + "roomList", xmlData);

                XmlNode root = response.SelectSingleNode("roomList");
                string status = root.SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    foreach (var item in root.SelectNodes("roomInfo"))
                    {
                        var node = item as XmlNode;
                        var room = new MeetingRoom();
                        room.RoomId = node.SelectSingleNode("roomId").InnerText;
                        room.Name = node.SelectSingleNode("roomName").InnerText;

                        roomList.Add(room);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

            }

            return false;

        }

        public bool TryGetLeaderList(Model.HandlerSession session, out List<Model.MeetingLeader> leaderList)
        {
            leaderList = new List<MeetingLeader>();
            try
            {
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><leaderList><messageId>{0}</messageId><token>{1}</token></leaderList>", session.MessageId, session.Token);
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "leaderList", xmlData);

                XmlNode root = response.SelectSingleNode("leaderList");
                string status = root.SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    foreach (var item in root.SelectNodes("leader"))
                    {
                        var node = item as XmlNode;
                        var leader = new MeetingLeader();
                        leader.UserName = node.SelectSingleNode("userName").InnerText;
                        leader.Name = node.SelectSingleNode("name").InnerText;
                        leader.LeaderPRI = node.SelectSingleNode("leaderPRI").InnerText;
                        leader.LeaderPRIDesc = node.SelectSingleNode("leaderPRIDesc").InnerText;

                        leaderList.Add(leader);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

            }

            return false;
           
        }

        public bool TryGetMobileTermList(Model.HandlerSession session, out List<Model.MobileTerm> mobileTermList)
        {
            mobileTermList = new List<MobileTerm>();
            try
            {
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><mobileTermList><messageId>{0}</messageId><token>{1}</token><startTime>{2}</startTime><endTime>{3}</endTime></mobileTermList>",
                                                session.MessageId, 
                                                session.Token,
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                DateTime.Now.AddMinutes(30).ToString("yyyy-MM-dd HH:mm:ss"));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "mobileTermList", xmlData);

                XmlNode root = response.SelectSingleNode("mobileTermList");
                string status = root.SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    foreach (var item in root.SelectNodes("roomInfo"))
                    {
                        var node = item as XmlNode;
                        var term = new MobileTerm();
                        term.RoomId = node.SelectSingleNode("roomId").InnerText;
                        term.RoomName = node.SelectSingleNode("roomName").InnerText;

                        mobileTermList.Add(term);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public bool TryGetRegionCatagory(string seriesId, Model.HandlerSession session, out Model.RegionCatagory regionCatagory)
        {
            regionCatagory = new RegionCatagory();

            try
            {
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><searchCity><messageId>{0}</messageId><token>{1}</token><seriesId>{2}</seriesId ><provinceCode>0</provinceCode ><cityCode>0</cityCode ><boroughCode>0</boroughCode></searchCity>",
                                                session.MessageId,
                                                session.Token,
                                                seriesId);

                var response = this._client.DoHttpWebRequest(session.BaseUrl + "searchCity", xmlData);

                XmlNode root = response.SelectSingleNode("searchCity");
                string status = root.SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    regionCatagory.SeriesList = new List<MeetingSeries>();
                    foreach (var item in root.SelectSingleNode("seriesList").SelectNodes("seriesInfo"))
                    {
                        var node = item as XmlNode;
                        var series = new MeetingSeries();
                        series.Id = node.SelectSingleNode("seriesId").InnerText;
                        series.Name = node.SelectSingleNode("seriesName").InnerText;

                        regionCatagory.SeriesList.Add(series);
                    }

                    regionCatagory.ProvinceList = new List<RegionInfo>();
                    foreach (var item in root.SelectSingleNode("provinceList").SelectNodes("provinceInfo"))
                    {
                        var node = item as XmlNode;
                        var region = new RegionInfo();
                        region.Code = node.SelectSingleNode("provinceCode").InnerText;
                        region.Name = node.SelectSingleNode("provinceName").InnerText;

                        regionCatagory.ProvinceList.Add(region);
                    }

                    regionCatagory.CityList = new List<RegionInfo>();
                    foreach (var item in root.SelectSingleNode("cityList").SelectNodes("cityInfo"))
                    {
                        var node = item as XmlNode;
                        var region = new RegionInfo();
                        region.Code = node.SelectSingleNode("cityCode").InnerText;
                        region.Name = node.SelectSingleNode("cityName").InnerText;

                        regionCatagory.CityList.Add(region);
                    }

                    regionCatagory.BoroughList = new List<RegionInfo>();
                    foreach (var item in root.SelectSingleNode("boroughList").SelectNodes("boroughInfo"))
                    {
                        var node = item as XmlNode;
                        var region = new RegionInfo();
                        region.Code = node.SelectSingleNode("boroughCode").InnerText;
                        region.Name = node.SelectSingleNode("boroughCode").InnerText;

                        regionCatagory.BoroughList.Add(region);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

            }

            return false;
        

        }

        public bool TryGetMeetingDetail(string meetingId, Model.HandlerSession session, out Model.SVCMMeetingDetail meetingDetail)
        {
           meetingDetail = new SVCMMeetingDetail();

           try
           {
               string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><confInfo><messageId>{0}</messageId><token>{1}</token><conferId>{2}</conferId></confInfo>", session.MessageId, session.Token,meetingId);
               var response = this._client.DoHttpWebRequest(session.BaseUrl + "getConfInfo", xmlData);

               XmlNode root = response.SelectSingleNode("confInfo");
               string status = root.SelectSingleNode("result").InnerText;

               if (status == "200")
               {

                   var detail = new SVCMMeetingDetail();
                   detail.Id = root.SelectSingleNode("conferId").InnerText;
                   detail.Name = root.SelectSingleNode("conferName").InnerText;
                   detail.StartTime = DateTime.Parse(root.SelectSingleNode("startTime").InnerText);
                   detail.EndTime = DateTime.Parse(root.SelectSingleNode("endTime").InnerText);
                   detail.Status = root.SelectSingleNode("status").InnerText;
                   
                   string str = root.SelectSingleNode("mediaType").InnerText;
                   if (str == "4")
                       detail.ConfMideaType = MideaType.Local;
                   else
                       detail.ConfMideaType = MideaType.Video;

                   detail.ParticipatorNumber = int.Parse(root.SelectSingleNode("participatorNumber").InnerText);
                   detail.Series.Name = root.SelectSingleNode("seriesName").InnerText;
                   detail.AccountName = root.SelectSingleNode("accountName").InnerText;
                   detail.Phone = root.SelectSingleNode("telephone").InnerText;

                   var leaders = root.SelectSingleNode("leader").InnerText.Split(",".ToArray());
                   foreach (var item in leaders)
                   {
                       detail.LeaderList.Add(new MeetingLeader()
                       {
                           UserName = item
                       });
                   }

                   detail.LeaderRoom = root.SelectSingleNode("leaderRoom").InnerText;
                   //detail.Id = root.SelectSingleNode("ipTelephoneNumber").InnerText;
                   detail.Memo= root.SelectSingleNode("confMemo").InnerText;
                   detail.Password = root.SelectSingleNode("confPassword").InnerText;
                   
                   detail.VideoSet = (VideoSet)int.Parse(root.SelectSingleNode("videoSet").InnerText);

                   detail.IPDesc = root.SelectSingleNode("ipdesc").InnerText;

                   foreach (var item in root.SelectSingleNode("roomList").SelectNodes("roomInfo"))
                   {
                       var node = item as XmlNode;
                       detail.Rooms.Add(new MeetingRoom()
                       {
                           RoomId = node.SelectSingleNode("roomId").InnerText,
                           Name = node.SelectSingleNode("roomName").InnerText
                       });
                   }

                   //TermList
                   
                   return true;
               }
               else
               {
                   return false;
               }
           }
           catch (Exception ex)
           {

           }

           return false;
        }

        public bool TryGetMeetingList(Model.MeetingListQuery query, Model.HandlerSession session, out List<Model.SVCMMeeting> meetingList)
        {
            meetingList = new List<SVCMMeeting>();

            try
            {
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><bookingConferList><messageId>{0}</messageId><token>{1}</token><conferName>{2}</conferName><roomName>{3}</roomName><servicegk>{4}</servicegk><alias>{5}</alias><startTime>{6}</startTime><endTime>{7}</endTime ><confProperty>{8}</confProperty><statVideoType>{9}</statVideoType><confType>{10}</confType></bookingConferList>",
                                               session.MessageId,
                                               session.Token,
                                               query.MeetingName,
                                               query.RoomName,
                                               query.ServiceKey,
                                               query.Alias,
                                               query.StartTime.ToString("yyyy-MM-dd"),
                                               query.EndTime.ToString("yyyy-MM-dd"),
                                               query.ConferenceProperty,
                                               query.StatVideoType,
                                               query.ConfType);

                var response = this._client.DoHttpWebRequest(session.BaseUrl + "bookingConferList", xmlData);
                XmlNode root = response.SelectSingleNode("bookingConferList");
                string status = root.SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    foreach (var item in root.SelectSingleNode("conferenceList").SelectNodes("conference"))
                    {
                        var node = item as XmlNode;
                        var meeting = new SVCMMeeting();
                        meeting.Id = node.SelectSingleNode("conferId").InnerText;
                        meeting.Name = node.SelectSingleNode("conferName").InnerText;
                        meeting.AccountName = node.SelectSingleNode("accountName").InnerText;
                        meeting.StartTime = DateTime.Parse(node.SelectSingleNode("startTime").InnerText);
                        meeting.EndTime = DateTime.Parse(node.SelectSingleNode("endTime").InnerText);
                        meeting.Status = node.SelectSingleNode("status").InnerText;
                        meeting.Type = int.Parse(node.SelectSingleNode("mediaType").InnerText);
                        meeting.MainRoom = node.SelectSingleNode("mettingRoom").InnerText;
                        meeting.ServiceKey = node.SelectSingleNode("servicegk").InnerText;
                        meeting.Password = node.SelectSingleNode("confPassword").InnerText;
                        meetingList.Add(meeting);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }
    }
}
