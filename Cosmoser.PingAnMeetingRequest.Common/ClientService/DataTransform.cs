using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using System.Xml;

namespace Cosmoser.PingAnMeetingRequest.Common.ClientService
{
    public class DataTransform
    {
        public string GetXmlDataFromMeetingDetail(SVCMMeetingDetail detail, HandlerSession session)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><startConfer></startConfer>");

            XmlNode root = xmlDoc.SelectSingleNode("startConfer");

            this.AddChildrenNode(xmlDoc, root, "messageId", session.MessageId.ToString());
            this.AddChildrenNode(xmlDoc, root, "token", session.Token);
            this.AddChildrenNode(xmlDoc, root, "conferName", detail.Name);
            this.AddChildrenNode(xmlDoc, root, "startTime", detail.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            this.AddChildrenNode(xmlDoc, root, "durationHour", detail.DurantionHours.ToString());
            this.AddChildrenNode(xmlDoc, root, "durationMinute", detail.DurantionMinutes.ToString());
            this.AddChildrenNode(xmlDoc, root, "mcuTemplateId", "");
            this.AddChildrenNode(xmlDoc, root, "confPassword", detail.Password);
            this.AddChildrenNode(xmlDoc, root, "chairPassword", "");
            this.AddChildrenNode(xmlDoc, root, "caption", "");
            this.AddChildrenNode(xmlDoc, root, "conferMemo", detail.Memo);
            if(detail.MainRoom != null && detail.MainRoom.RoomId != null)
            this.AddChildrenNode(xmlDoc, root, "meetingRoom", detail.MainRoom.RoomId.Split(",".ToArray())[0]);
            else
                this.AddChildrenNode(xmlDoc, root, "meetingRoom", "");

            this.AddChildrenNode(xmlDoc, root, "termIds", detail.RoomIds);
            this.AddChildrenNode(xmlDoc, root, "videoSet", ((int)detail.VideoSet).ToString());
            //是否设定轮询 1 是 0 否，平安业务无此字段，保留
            this.AddChildrenNode(xmlDoc, root, "ifPolling", "1");
            //主席会场 0：表示无主席会场 ，平安业务无此字段，保留
            this.AddChildrenNode(xmlDoc, root, "chairRoom", "1");
            //与会人Ids,平安业务无此字段，保留 
            this.AddChildrenNode(xmlDoc, root, "participateIds", "");
            //是否发送会议短信,平安业务无此字段，保留
            this.AddChildrenNode(xmlDoc, root, "sendmsgflag", "");
            //是否音视频分离,平安业务无此字段，保留
            this.AddChildrenNode(xmlDoc, root, "isSperate", "");
            this.AddChildrenNode(xmlDoc, root, "participatorNumber", detail.ParticipatorNumber.ToString());
            this.AddChildrenNode(xmlDoc, root, "phone", detail.Phone);
            this.AddChildrenNode(xmlDoc, root, "ipdesc", detail.IPDesc);
            //点对点会议是否上MCU，0：不上MCU，1：上MCU，快乐平安新增字段，保留，默认填0
            this.AddChildrenNode(xmlDoc, root, "inMCU", "0");
            this.AddChildrenNode(xmlDoc, root, "leader", detail.LeaderListStr);
            this.AddChildrenNode(xmlDoc, root, "leaderRoom", detail.LeaderRoom);
            this.AddChildrenNode(xmlDoc, root, "conferType", ((int)detail.ConfType).ToString());
            this.AddChildrenNode(xmlDoc, root, "conferMideaType", ((int)detail.ConfMideaType).ToString());
            this.AddChildrenNode(xmlDoc, root, "regularMeetingType", detail.RegularMeetingType.ToString());
            this.AddChildrenNode(xmlDoc, root, "regularMaxnum", detail.RegularMaxNum.ToString());
            this.AddChildrenNode(xmlDoc, root, "regularMeetingNum", detail.RegularMeetingNum.ToString());
            this.AddChildrenNode(xmlDoc, root, "multiExceptDay", detail.MultiExceptDay);
            this.AddChildrenNode(xmlDoc, root, "multiExceptWeek", detail.MultiExceptWeek);
            this.AddChildrenNode(xmlDoc, root, "everyFewMonths", ((int)detail.EveryFewMonths).ToString());
            this.AddChildrenNode(xmlDoc, root, "theFirstFew", ((int)detail.TheFirstFew).ToString());
            this.AddChildrenNode(xmlDoc, root, "week", detail.Week.ToString());

            //----会议操作对象，1：web端，2：outlook客户端，3：快乐平安客户端---
            this.AddChildrenNode(xmlDoc, root, "conf_operate_obj", "2");
            return xmlDoc.InnerXml;
        }

        public string GetXmlDataForUpdatingMeeting(SVCMMeetingDetail detail, string operationType, HandlerSession session)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><updateConfer></updateConfer>");

            XmlNode root = xmlDoc.SelectSingleNode("updateConfer");

            this.AddChildrenNode(xmlDoc, root, "messageId", session.MessageId.ToString());
            this.AddChildrenNode(xmlDoc, root, "token", session.Token);
            this.AddChildrenNode(xmlDoc, root, "conferId", detail.Id);
            this.AddChildrenNode(xmlDoc, root, "conferName", detail.Name);
            this.AddChildrenNode(xmlDoc, root, "startTime", detail.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            this.AddChildrenNode(xmlDoc, root, "durationHour", detail.DurantionHours.ToString());
            this.AddChildrenNode(xmlDoc, root, "durationMinute", detail.DurantionMinutes.ToString());
            this.AddChildrenNode(xmlDoc, root, "confPassword", detail.Password??string.Empty);
            this.AddChildrenNode(xmlDoc, root, "conferMemo", detail.Memo);
            this.AddChildrenNode(xmlDoc, root, "meetingRoom", detail.MainRoom == null ? string.Empty : detail.MainRoom.RoomId.Split(",".ToArray())[0]);
            this.AddChildrenNode(xmlDoc, root, "termIds", detail.RoomIds);
            this.AddChildrenNode(xmlDoc, root, "videoSet", ((int)detail.VideoSet).ToString());
            this.AddChildrenNode(xmlDoc, root, "participatorNumber", detail.ParticipatorNumber.ToString());
            this.AddChildrenNode(xmlDoc, root, "phone", detail.Phone);
            this.AddChildrenNode(xmlDoc, root, "ipdesc", detail.IPDesc);
            //点对点会议是否上MCU，0：不上MCU，1：上MCU，快乐平安新增字段，保留，默认填0
            this.AddChildrenNode(xmlDoc, root, "inMCU", "0");
            this.AddChildrenNode(xmlDoc, root, "leader", detail.LeaderListStr);
            this.AddChildrenNode(xmlDoc, root, "leaderRoom", detail.LeaderRoom);
            this.AddChildrenNode(xmlDoc, root, "conferType", ((int)detail.ConfType).ToString());
            this.AddChildrenNode(xmlDoc, root, "conferMideaType", ((int)detail.ConfMideaType).ToString());
            this.AddChildrenNode(xmlDoc, root, "operateType", operationType);

            //----会议操作对象，1：web端，2：outlook客户端，3：快乐平安客户端---
            this.AddChildrenNode(xmlDoc, root, "conf_operate_obj", "2");

            return xmlDoc.InnerXml;
        }

        private void AddChildrenNode(XmlDocument xmlDoc, XmlNode node, string name, string innerText)
        {
            XmlElement element = xmlDoc.CreateElement(name);
            element.InnerText = innerText;
            node.AppendChild(element);
        }
    }
}
