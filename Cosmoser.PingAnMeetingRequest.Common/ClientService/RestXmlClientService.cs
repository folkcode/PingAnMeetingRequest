using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cosmoser.PingAnMeetingRequest.Common.Interfaces;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using System.Reflection;
using log4net;
using Cosmoser.PingAnMeetingRequest.Common.Utilities;

namespace Cosmoser.PingAnMeetingRequest.Common.ClientService
{
    public class RestXmlClientService : IConferenceHandler
    {
        private DataTransform _dataTransform = new DataTransform();
        private RestXMLApiClient _client = new RestXMLApiClient();

        private static ILog logger = IosLogManager.GetLogger(typeof(RestXmlClientService));

        public bool Login(ref Model.HandlerSession session)
        {
            try
            {
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><login><userName>{0}</userName><userType>1</userType></login>", session.UserName);
                logger.Debug(string.Format("Login, xmldata: {0}", xmlData));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "login", xmlData);
                logger.Debug(string.Format("Login response, xmldata: {0}", response.OuterXml));
                string status = response.SelectSingleNode("login").SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    session.Token = response.SelectSingleNode("login").SelectSingleNode("token").InnerText;
                    string confType = response.SelectSingleNode("login").SelectSingleNode("confType").InnerText;
                    session.ConfTypeList = new List<ConferenceType>();
                    foreach (var item in confType.Split(",".ToArray()))
                    {
                        session.ConfTypeList.Add((ConferenceType)int.Parse(item));
                    }
                    session.IfBookMobileTerm = response.SelectSingleNode("login").SelectSingleNode("ifBookMobileTerm").InnerText == "1" ? true : false;
                    session.IfBookIPConf = response.SelectSingleNode("login").SelectSingleNode("ifBookIPConf").InnerText == "1" ? true : false;
                    session.IsActive = true;
                    //每次登陆都需要重设messageId
                    session.ResetMessageId();
                    return true;
                }
                else
                {
                    logger.Error(string.Format("Login failed, status: {0}, error:{1}",status,response.InnerXml));
                }
            }
            catch(Exception ex)
            {
                logger.Error("Login failed, error:" + ex.Message + "\n" + ex.StackTrace);
            }

            session.IsActive = false;
            return false;
        }

        public bool BookingMeeting(Model.SVCMMeetingDetail meetingDetail, Model.HandlerSession session, out string error)
        {
            error = string.Empty;
            try
            {
                session.AddMessageId();
                string xmlData = this._dataTransform.GetXmlDataFromMeetingDetail(meetingDetail, session);
                logger.Debug(string.Format("BookingMeeting, xmldata: {0}", xmlData));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "startConfer", xmlData);
                logger.Debug(string.Format("BookingMeeting response, xmldata: {0}", response.OuterXml));
                string status = response.SelectSingleNode("startConfer").SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    meetingDetail.Id = response.SelectSingleNode("startConfer").SelectSingleNode("conferId").InnerText;
                    return true;
                }
                else
                {
                    error = response.SelectSingleNode("startConfer").SelectSingleNode("result").Attributes["property"].Value;
                    logger.Error(string.Format("BookingMeeting failed, status: {0}, error:{1}", status, response.InnerXml));
                    this.ReLogin(session, response.SelectSingleNode("startConfer").SelectSingleNode("result"));
                    return false;
                }
            }
            catch(Exception ex)
            {
                logger.Error("BookingMeeting failed, error:" + ex.Message + "\n" + ex.StackTrace);
                this.Login(ref session);

            }

            return false;
        }

        private void ReLogin(HandlerSession session, XmlNode resultNode)
        {
            string property = resultNode.Attributes["property"].Value;
            string status = resultNode.InnerText;

            if (property == "TOKEN不存在!" || property == "MESSAGEID有误,出现丢包现象!" || status == "404" || status == "502")
            {
                //重新登录
                this.Login(ref session);
            }
        }

        public bool DeleteMeeting(string conferId, Model.HandlerSession session, out string error)
        {
            error = string.Empty;
            try
            {
                session.AddMessageId();
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><deleteConfer><messageId>{0}</messageId><token>{1}</token><conferId>{2}</conferId><conf_operate_obj>2</conf_operate_obj></deleteConfer>", session.MessageId, session.Token, conferId);
                logger.Debug(string.Format("DeleteMeeting, xmldata: {0}", xmlData));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "deleteConfer", xmlData);
                logger.Debug(string.Format("DeleteMeeting response, xmldata: {0}", response.OuterXml));
                XmlNode root = response.SelectSingleNode("deleteConfer");
                string status = root.SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    return true;
                }
                else
                {
                    //logger.Error(string.Format("DeleteMeeting failed, status: {0}, error:{1}", status, response.InnerXml));
                    //this.ReLogin(session, root.SelectSingleNode("result"));
                    //return false;
                    error = response.SelectSingleNode("deleteConfer").SelectSingleNode("result").Attributes["property"].Value;
                    logger.Error(string.Format("DeleteMeeting failed, status: {0}, error:{1}", status, response.InnerXml));
                    this.ReLogin(session, response.SelectSingleNode("deleteConfer").SelectSingleNode("result"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("DeleteMeeting failed, error:" + ex.Message + "\n" + ex.StackTrace);
                this.Login(ref session);
            }

            return false;
        }

        public bool UpdateMeeting(Model.SVCMMeetingDetail meetingDetail, string operateType, Model.HandlerSession session, out string error, out string errorCode)
        {
            error = string.Empty;
            errorCode = string.Empty;
            try
            {
                session.AddMessageId();
                string xmlData = this._dataTransform.GetXmlDataForUpdatingMeeting(meetingDetail, operateType, session);
                logger.Debug(string.Format("UpdateMeeting, xmldata: {0}", xmlData));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "updateConfer", xmlData);
                logger.Debug(string.Format("UpdateMeeting response, xmldata: {0}", response.OuterXml));
                string status = response.SelectSingleNode("updateConfer").SelectSingleNode("result").InnerText;
                errorCode = status;
                if (status == "200")
                {
                    return true;
                }
                else
                {
                    error = response.SelectSingleNode("updateConfer").SelectSingleNode("result").Attributes["property"].Value;
                    logger.Error(string.Format("UpdateMeeting failed, status: {0}, error:{1}", status, response.InnerXml));
                    if(errorCode == "500" || errorCode == "502" || errorCode == "501")
                    this.ReLogin(session, response.SelectSingleNode("updateConfer").SelectSingleNode("result"));
                    return false;
                }
            }
            catch(Exception ex)
            {
                logger.Error("UpdateMeeting failed, error:" + ex.Message + "\n" + ex.StackTrace);
                error = ex.Message;
                this.Login(ref session);
            }

            return false;
        }

       

        public bool TryGetSeriesList(Model.HandlerSession session, out List<Model.MeetingSeries> seriesList)
        {
            seriesList = new List<Model.MeetingSeries>();
            try
            {
                session.AddMessageId();
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><seriesList><messageId>{0}</messageId><token>{1}</token></seriesList>", session.MessageId, session.Token);
                logger.Debug(string.Format("TryGetSeriesList, xmldata: {0}", xmlData));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "seriesList", xmlData);
                logger.Debug(string.Format("TryGetSeriesList response, xmldata: {0}", response.OuterXml));
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
                    logger.Error(string.Format("TryGetSeriesList failed, status: {0}, error:{1}", status, response.InnerXml));
                    this.ReLogin(session, root.SelectSingleNode("result"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("TryGetSeriesList failed, error:" + ex.Message + "\n" + ex.StackTrace);
                this.Login(ref session);

            }

            return false;
        }

        public bool TryGetMeetingRoomList(MeetingRoomListQuery query, Model.HandlerSession session, out List<Model.MeetingRoom> roomList)
        {
            roomList = new List<MeetingRoom>();

            try
            {
                session.AddMessageId();
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><roomList><messageId>{0}</messageId><token>{1}</token><seriesId>{2}</seriesId><levelId>{3}</levelId><confType>{4}</confType><startTime>{5}</startTime><endTime>{6}</endTime></roomList>",
                                                session.MessageId,
                                                session.Token,
                                                query.SeriesId,
                                                query.LevelId,
                                                (int)query.ConfType,
                                                query.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                query.EndTime.ToString("yyyy-MM-dd HH:mm:ss")
                                                );
                logger.Debug(string.Format("TryGetMeetingRoomList, xmldata: {0}", xmlData));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "roomList", xmlData);
                logger.Debug(string.Format("TryGetMeetingRoomList response, xmldata: {0}", response.OuterXml));

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
                    logger.Error(string.Format("TryGetMeetingRoomList failed, status: {0}, error:{1}", status, response.InnerXml));
                    this.ReLogin(session, root.SelectSingleNode("result"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("TryGetMeetingRoomList failed, error:" + ex.Message + "\n" + ex.StackTrace);
                this.Login(ref session);
            }

            return false;

        }

        public bool TryGetLeaderList(Model.HandlerSession session, out List<Model.MeetingLeader> leaderList)
        {
            leaderList = new List<MeetingLeader>();
            try
            {
                session.AddMessageId();
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><leaderList><messageId>{0}</messageId><token>{1}</token></leaderList>", session.MessageId, session.Token);
                logger.Debug(string.Format("TryGetLeaderList, xmldata: {0}", xmlData));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "leaderList", xmlData);
                logger.Debug(string.Format("TryGetLeaderList response, xmldata: {0}", response.OuterXml));
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
                    logger.Error(string.Format("TryGetLeaderList failed, status: {0}, error:{1}", status, response.InnerXml));
                    this.ReLogin(session, root.SelectSingleNode("result"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("TryGetLeaderList failed, error:" + ex.Message + "\n" + ex.StackTrace);
                this.Login(ref session);

            }

            return false;
           
        }

        public bool TryGetMobileTermList(Model.HandlerSession session, out List<Model.MobileTerm> mobileTermList)
        {
            mobileTermList = new List<MobileTerm>();
            try
            {
                session.AddMessageId();
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><mobileTermList><messageId>{0}</messageId><token>{1}</token><startTime>{2}</startTime><endTime>{3}</endTime></mobileTermList>",
                                                session.MessageId,
                                                session.Token,
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                DateTime.Now.AddMinutes(30).ToString("yyyy-MM-dd HH:mm:ss"));
                logger.Debug(string.Format("TryGetMobileTermList, xmldata: {0}", xmlData));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "mobileTermList", xmlData);
                logger.Debug(string.Format("TryGetMobileTermList response, xmldata: {0}", response.OuterXml));
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
                    logger.Error(string.Format("TryGetMobileTermList failed, status: {0}, error:{1}", status, response.InnerXml));
                    this.ReLogin(session, root.SelectSingleNode("result"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("TryGetMobileTermList failed, error:" + ex.Message + "\n" + ex.StackTrace);
                this.Login(ref session);

            }

            return false;
        }

        public bool TryGetRegionCatagory(RegionCatagoryQuery query, Model.HandlerSession session, out Model.RegionCatagory regionCatagory)
        {
            regionCatagory = new RegionCatagory();

            try
            {
                session.AddMessageId();
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><searchCity><messageId>{0}</messageId><token>{1}</token><seriesId>{2}</seriesId><provinceCode>{3}</provinceCode><cityCode>{4}</cityCode><boroughCode>{5}</boroughCode></searchCity>",
                                                session.MessageId,
                                                session.Token,
                                                query.SeriesId,
                                                query.ProvinceCode,
                                                query.CityCode,
                                                query.BoroughCode);
                logger.Debug(string.Format("TryGetRegionCatagory, xmldata: {0}", xmlData));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "searchcity", xmlData);
                logger.Debug(string.Format("TryGetRegionCatagory response, xmldata: {0}", response.OuterXml));
                XmlNode root = response.SelectSingleNode("searchCity");
                string status = root.SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    regionCatagory.SeriesList = new List<MeetingSeries>();
                    if(root.SelectSingleNode("seriesList") != null)
                        foreach (var item in root.SelectSingleNode("seriesList").SelectNodes("series"))
                    {
                        var node = item as XmlNode;
                        var series = new MeetingSeries();
                        series.Id = node.SelectSingleNode("seriesId").InnerText;
                        series.Name = node.SelectSingleNode("seriesName").InnerText;

                        regionCatagory.SeriesList.Add(series);
                    }

                    regionCatagory.ProvinceList = new List<RegionInfo>();
                    if(root.SelectSingleNode("provinceList") != null)
                    foreach (var item in root.SelectSingleNode("provinceList").SelectNodes("provinceInfo"))
                    {
                        var node = item as XmlNode;
                        var region = new RegionInfo();
                        region.Code = node.SelectSingleNode("provinceCode").InnerText;
                        region.Name = node.SelectSingleNode("provinceName").InnerText;

                        regionCatagory.ProvinceList.Add(region);
                    }

                    regionCatagory.CityList = new List<RegionInfo>();
                    if (root.SelectSingleNode("cityList") != null)
                    foreach (var item in root.SelectSingleNode("cityList").SelectNodes("cityInfo"))
                    {
                        var node = item as XmlNode;
                        var region = new RegionInfo();
                        region.Code = node.SelectSingleNode("cityCode").InnerText;
                        region.Name = node.SelectSingleNode("cityName").InnerText;

                        regionCatagory.CityList.Add(region);
                    }

                    regionCatagory.BoroughList = new List<RegionInfo>();
                    if (root.SelectSingleNode("boroughList") != null)
                    foreach (var item in root.SelectSingleNode("boroughList").SelectNodes("boroughInfo"))
                    {
                        var node = item as XmlNode;
                        var region = new RegionInfo();
                        region.Code = node.SelectSingleNode("boroughCode").InnerText;
                        XmlNode nameNode = node.SelectSingleNode("boroughName");
                        if(nameNode == null)
                            nameNode = node.SelectSingleNode("cityName");
                        region.Name = nameNode.InnerText;

                        regionCatagory.BoroughList.Add(region);
                    }

                    return true;
                }
                else
                {
                    logger.Error(string.Format("TryGetRegionCatagory failed, status: {0}, error:{1}", status, response.InnerXml));
                    this.ReLogin(session, root.SelectSingleNode("result"));
                    return false;
                }
            }
            catch(Exception ex)
            {
                logger.Error("TryGetRegionCatagory failed, error:" + ex.Message + "\n" + ex.StackTrace);
                this.Login(ref session);

            }

            return false;
        

        }

        public bool TryGetMeetingDetail(string meetingId, Model.HandlerSession session, out Model.SVCMMeetingDetail detail)
        {
           detail = new SVCMMeetingDetail();

           try
           {
               session.AddMessageId();
               string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><confInfo><messageId>{0}</messageId><token>{1}</token><conferId>{2}</conferId></confInfo>", session.MessageId, session.Token, meetingId);
               logger.Debug(string.Format("TryGetMeetingDetail, xmldata: {0}", xmlData));
               var response = this._client.DoHttpWebRequest(session.BaseUrl + "getConfInfo", xmlData);
               logger.Debug(string.Format("TryGetMeetingDetail response, xmldata: {0}", response.OuterXml));
               XmlNode root = response.SelectSingleNode("confInfo");
               string status = root.SelectSingleNode("result").InnerText;

               if (status == "200")
               {
                   detail.Id = root.SelectSingleNode("conferId").InnerText;
                   detail.Name = root.SelectSingleNode("conferName").InnerText??string.Empty;
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
                   detail.Phone = root.SelectSingleNode("telephone").InnerText.Replace("null", "");

                   var leaders = root.SelectSingleNode("leader").InnerText.Replace("null", "").Split(",".ToArray());
                   
                   foreach (var item in leaders)
                   {
                       detail.LeaderList.Add(new MeetingLeader()
                       {
                           UserName = item
                       });
                   }

                   detail.LeaderNameListStr = root.SelectSingleNode("leaderName").InnerText;
                   detail.LeaderRoom = root.SelectSingleNode("leaderRoom").InnerText.Replace("null", "");
                   detail.IpTelephoneNumber = root.SelectSingleNode("ipTelephoneNumber").InnerText;
                   detail.Department = root.SelectSingleNode("department").InnerText;
                   
                   detail.Memo = root.SelectSingleNode("confMemo").InnerText.Replace("null", "");
                   detail.Password = root.SelectSingleNode("confPassword").InnerText.Replace("null", "");

                   if (root.SelectSingleNode("videoSet") != null)
                       detail.VideoSet = (VideoSet)int.Parse(root.SelectSingleNode("videoSet").InnerText);

                   detail.IPDesc = root.SelectSingleNode("ipdesc").InnerText.Replace("null","");
                   XmlNode mobileTermlistNode = root.SelectSingleNode("mobileTermList");
                   if (mobileTermlistNode != null)
                   {
                       foreach (var item in root.SelectSingleNode("mobileTermList").SelectNodes("roomInfo"))
                       {
                           var node = item as XmlNode;

                           detail.MobileTermList.Add(new MobileTerm()
                           {
                               RoomId = node.SelectSingleNode("roomId").InnerText,
                               RoomName = node.SelectSingleNode("roomName").InnerText
                           });
                       }
                   }

                   XmlNode roomlistNode = root.SelectSingleNode("roomList");
                   if (roomlistNode != null)
                   {
                       foreach (var item in root.SelectSingleNode("roomList").SelectNodes("roomInfo"))
                       {
                           var node = item as XmlNode;
                           string termType = node.SelectSingleNode("termType").InnerText;

                           if (termType == "1")
                           {
                               detail.Rooms.Add(new MeetingRoom()
                               {
                                   RoomId = node.SelectSingleNode("roomId").InnerText,
                                   Name = node.SelectSingleNode("roomName").InnerText,
                                   Address = node.SelectSingleNode("address").InnerText

                               });

                               if (node.SelectSingleNode("roomType").InnerText == "1")
                               {
                                   detail.MainRoom = new MeetingRoom()
                                   {
                                       RoomId = node.SelectSingleNode("roomId").InnerText,
                                       Name = node.SelectSingleNode("roomName").InnerText
                                   };
                               }
                           }
                           //else
                           //{
                           //    detail.MobileTermList.Add(new MobileTerm()
                           //    {
                           //        RoomId = node.SelectSingleNode("roomId").InnerText,
                           //        RoomName = node.SelectSingleNode("roomName").InnerText
                           //    });
                           //}
                       }
                   }

                   //TermList
                   
                   return true;
               }
               else
               {
                   logger.Error(string.Format("TryGetMeetingDetail failed, status: {0}, error:{1}", status, response.InnerXml));
                   this.ReLogin(session, root.SelectSingleNode("result"));
                   return false;
               }
           }
           catch (Exception ex)
           {
               logger.Error("TryGetMeetingDetail failed, error:" + ex.Message + "\n" + ex.StackTrace);
               this.Login(ref session);

           }

           return false;
        }

        public bool TryGetMeetingList(Model.MeetingListQuery query, Model.HandlerSession session, out List<Model.SVCMMeeting> meetingList)
        {
            meetingList = new List<SVCMMeeting>();

            try
            {
                session.AddMessageId();
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
                logger.Debug(string.Format("TryGetMeetingList, xmldata: {0}", xmlData));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "bookingConferList", xmlData);
                logger.Debug(string.Format("TryGetMeetingList response, xmldata: {0}", response.OuterXml));
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
                        meeting.StatusCode = int.Parse(node.SelectSingleNode("status").InnerText);
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
                    logger.Error(string.Format("TryGetMeetingList failed, status: {0}, error:{1}", status, response.InnerXml));
                    this.ReLogin(session, root.SelectSingleNode("result"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("TryGetMeetingList failed, error:" + ex.Message + "\n" + ex.StackTrace);
                this.Login(ref session);

            }

            return false;
        }


        public bool TryGetMeetingScheduler(MeetingSchedulerQuery query, HandlerSession session, out List<MeetingScheduler> schedulerList)
        {
            schedulerList = new List<MeetingScheduler>();

            try
            {
                session.AddMessageId();
                string xmlData = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><termConferList><messageId>{0}</messageId><token>{1}</token><roomName>{2}</roomName><levelId>{3}</levelId><seriesId>{4}</seriesId><provinceCode>{5}</provinceCode><cityCode>{6}</cityCode><boroughCode>{7}</boroughCode><boardroomState>{8}</boardroomState><roomIfTerminal>{9}</roomIfTerminal><capacity>{10}</capacity><startTime>{11}</startTime><endTime>{12}</endTime><dataAll>{13}</dataAll></termConferList>",
                                               session.MessageId,
                                               session.Token,
                                               query.RoomName,
                                               query.LevelId,
                                               query.SeriesId,
                                               query.ProvinceCode,                                               
                                               query.CityCode,                                               
                                               query.BoroughCode,                                               
                                               query.BoardRoomState,
                                               query.RoomIfTerminal,
                                               query.Capacity,
                                               query.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                               query.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                               query.DataAll);
                logger.Debug(string.Format("TryGetMeetingScheduler, xmldata: {0}", xmlData));
                var response = this._client.DoHttpWebRequest(session.BaseUrl + "termConferList", xmlData);
                logger.Debug(string.Format("TryGetMeetingScheduler response, xmldata: {0}", response.OuterXml));
                XmlNode root = response.SelectSingleNode("termConferList");
                string status = root.SelectSingleNode("result").InnerText;

                if (status == "200")
                {
                    foreach (var item in root.SelectSingleNode("roomList").SelectNodes("roomInfo"))
                    {
                        var node = item as XmlNode;

                        string roomId = node.SelectSingleNode("roomId").InnerText;
                        string roomName = node.SelectSingleNode("roomName").InnerText;
                        string seriesName = node.SelectSingleNode("seriesName").InnerText;
                        int IfTerminal = int.Parse(node.SelectSingleNode("IfTerminal").InnerText);
                        string property = node.SelectSingleNode("property").InnerText;                     
                        string address = node.SelectSingleNode("seriesName").InnerText;

                        var conferIdNodes = node.SelectNodes("conferId");
                        var startTimeNodes = node.SelectNodes("startTime");
                        var endTimeNodes = node.SelectNodes("endTime");
                        var approveStatusNodes = node.SelectNodes("approveStatus");
                        var statusNodes = node.SelectNodes("status");

                        if (conferIdNodes != null && conferIdNodes.Count > 0)
                        {
                            for (int i = 0; i < conferIdNodes.Count; i++)
                            {
                                var meeting = new MeetingScheduler();
                                meeting.RoomId = roomId;
                                meeting.RoomName = roomName;
                                meeting.IfTerminal = IfTerminal;
                                meeting.Property = property;
                                meeting.Address = address;
                                meeting.SeriesName = seriesName;

                                meeting.ConferId = conferIdNodes[i].InnerText;
                                meeting.StartTime = DateTime.Parse(startTimeNodes[i].InnerText);
                                meeting.EndTime = DateTime.Parse(endTimeNodes[i].InnerText);
                                meeting.ApproveStatus = int.Parse(approveStatusNodes[i].InnerText);
                                meeting.Status = int.Parse(statusNodes[i].InnerText);

                                schedulerList.Add(meeting);
                            }
                        }
                        else
                        {
                            var meeting = new MeetingScheduler();
                            meeting.RoomId = roomId;
                            meeting.RoomName = roomName;
                            meeting.IfTerminal = IfTerminal;
                            meeting.Property = property;
                            meeting.Address = address;
                            meeting.SeriesName = seriesName;
                            meeting.StartTime = DateTime.MinValue;

                            schedulerList.Add(meeting);
                        }                
                    }

                    return true;
                }
                else
                {
                    logger.Error(string.Format("TryGetMeetingScheduler failed, status: {0}, error:{1}", status, response.InnerXml));
                    this.ReLogin(session, root.SelectSingleNode("result"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("TryGetMeetingScheduler failed, error:" + ex.Message + "\n" + ex.StackTrace);
                this.Login(ref session);

            }

            return false;
        }
    }
}
