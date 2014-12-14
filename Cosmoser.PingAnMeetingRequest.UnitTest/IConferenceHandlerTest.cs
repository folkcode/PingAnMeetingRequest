using Cosmoser.PingAnMeetingRequest.Common.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using System.Collections.Generic;
using Cosmoser.PingAnMeetingRequest.Common.ClientService;

namespace Cosmoser.PingAnMeetingRequest.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for IConferenceHandlerTest and is intended
    ///to contain all IConferenceHandlerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IConferenceHandlerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private HandlerSession _session = new HandlerSession()
        {
            UserName = "zhangxue016",
            IP = "192.166.5.190",
            Port = "7080"
        };

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        internal virtual IConferenceHandler CreateIConferenceHandler()
        {
            // TODO: Instantiate an appropriate concrete class.
            IConferenceHandler target = ClientServiceFactory.Create();
            return target;
        }

        /// <summary>
        ///A test for BookingMeeting
        ///</summary>
        [TestMethod()]
        public void BookingMeetingTest()
        {
            IConferenceHandler target = CreateIConferenceHandler(); // TODO: Initialize to an appropriate value
            SVCMMeetingDetail meetingDetail = new SVCMMeetingDetail()
            {
                Name = "test-tony",
                //AccountName = "",
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddMinutes(30),
                ConfMideaType = MideaType.Local,
                ConfType = ConferenceType.Furture,
                IPDesc = "23324,333,4343",
                ParticipatorNumber = 3,
                Phone = "138138899",
                Memo = "test",
                Password = "",
                LeaderList = new List<MeetingLeader>()
                {
                    new MeetingLeader()
                    {
                        UserName = "ALIBROKER",
                        Name = "Ali"
                    },
                    new MeetingLeader()
                    {
                        UserName = "CAOLIUYI",
                        Name = "test"
                    },
                    new MeetingLeader()
                    {
                        UserName = "CENXINJIANG757",
                        Name = "test"
                    }
                },
                LeaderRoom = "main room",
                MainRoom = new MeetingRoom() { RoomId = "13483,0,0", Name = "3.52" },
                MobileTermList = new List<MobileTerm>()
                {
                    new MobileTerm() { RoomId = "13702",RoomName="bao/BAO/42342323"},
                    new MobileTerm() { RoomId = "13703",RoomName="bao/BAO/42342"}
                },
                Rooms = new List<MeetingRoom>()
                {
                    new MeetingRoom() { RoomId = "13483,0,0", Name = "3.52" },
                    new MeetingRoom() { RoomId = "13484,0,0", Name = "3.54" }
                },
                VideoSet = VideoSet.Audio,
                MultiExceptDay = "",
                MultiExceptWeek = "",
                RegularMeetingNum = 0,
                RegularMaxNum = 1,
                RegularMeetingType = 1,
                TheFirstFew = 3,
                EveryFewMonths = 1,
                Week = 4

            }; // TODO: Initialize to an appropriate value
            HandlerSession session = this._session; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Login(ref session);
            if(actual)
            actual = target.BookingMeeting(meetingDetail, session);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DeleteMeeting
        ///</summary>
        [TestMethod()]
        public void DeleteMeetingTest()
        {
            IConferenceHandler target = CreateIConferenceHandler(); // TODO: Initialize to an appropriate value
            string conferId = "1410960"; // TODO: Initialize to an appropriate value
            HandlerSession session = this._session; // TODO: Initialize to an appropriate value         
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Login(ref session);
            if (actual)
                actual = target.DeleteMeeting(conferId, session);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Login
        ///</summary>
        [TestMethod()]
        public void LoginTest()
        {
            IConferenceHandler target = CreateIConferenceHandler(); // TODO: Initialize to an appropriate value
            HandlerSession session = new HandlerSession(); // TODO: Initialize to an appropriate value
            session.UserName = "zhangxue016";
            session.IP = "192.166.5.190";
            session.Port = "7080";
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Login(ref session);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TryGetLeaderList
        ///</summary>
        [TestMethod()]
        public void TryGetLeaderListTest()
        {
            IConferenceHandler target = CreateIConferenceHandler(); // TODO: Initialize to an appropriate value
            HandlerSession session = this._session;

            List<MeetingLeader> leaderList = null; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;

            actual = target.Login(ref session);
            if (actual == true)
                actual = target.TryGetLeaderList(session, out leaderList);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TryGetMeetingDetail
        ///</summary>
        [TestMethod()]
        public void TryGetMeetingDetailTest()
        {
            IConferenceHandler target = CreateIConferenceHandler(); // TODO: Initialize to an appropriate value
            string meetingId = "1410960"; // TODO: Initialize to an appropriate value
            HandlerSession session = this._session; // TODO: Initialize to an appropriate value
            SVCMMeetingDetail meetingDetail = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Login(ref session);
            if (actual)
                actual = target.TryGetMeetingDetail(meetingId, session, out meetingDetail);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TryGetMeetingList
        ///</summary>
        [TestMethod()]
        public void TryGetMeetingListTest()
        {
            IConferenceHandler target = CreateIConferenceHandler(); // TODO: Initialize to an appropriate value
            MeetingListQuery query =  new MeetingListQuery(); // TODO: Initialize to an appropriate value

            query.MeetingName = "";
            query.RoomName = "";
            query.ConferenceProperty = "";
            query.ConfType = ConferenceType.Immediate;
            query.Alias = "";
            query.ServiceKey = "";
            query.StatVideoType = 2;
            query.StartTime = DateTime.Today;
            query.EndTime = DateTime.Today.AddDays(10);

            HandlerSession session = this._session; // TODO: Initialize to an appropriate value
            List<SVCMMeeting> meetingList = null; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Login(ref session);
            if (actual)
                actual = target.TryGetMeetingList(query, session, out meetingList);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TryGetMeetingRoomList
        ///</summary>
        [TestMethod()]
        public void TryGetMeetingRoomListTest()
        {
            IConferenceHandler target = CreateIConferenceHandler(); // TODO: Initialize to an appropriate value
            MeetingRoomListQuery query = new MeetingRoomListQuery();
            query.SeriesId = "2";
            query.LevelId = "1,1";
            query.ConfType = ConferenceType.Immediate;
            query.StartTime = DateTime.Now;
            query.EndTime = DateTime.Now.AddMinutes(30);

            HandlerSession session = this._session; // TODO: Initialize to an appropriate value
            List<MeetingRoom> roomList = null; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;

            actual = target.Login(ref session);
            if (actual)
                actual = target.TryGetMeetingRoomList(query, session, out roomList);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TryGetMobileTermList
        ///</summary>
        [TestMethod()]
        public void TryGetMobileTermListTest()
        {
            IConferenceHandler target = CreateIConferenceHandler(); // TODO: Initialize to an appropriate value
            HandlerSession session = this._session;

            List<MobileTerm> termList = null; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;

            actual = target.Login(ref session);
            if (actual == true)
                actual = target.TryGetMobileTermList(session, out termList);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TryGetRegionCatagory
        ///</summary>
        [TestMethod()]
        public void TryGetRegionCatagoryTest()
        {
            IConferenceHandler target = CreateIConferenceHandler(); // TODO: Initialize to an appropriate value
            string seriesId = "2"; // TODO: Initialize to an appropriate value
            HandlerSession session = this._session; // TODO: Initialize to an appropriate value
            RegionCatagory regionCatagory = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Login(ref session);
            if (actual)
                actual = target.TryGetRegionCatagory(seriesId, session, out regionCatagory);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TryGetSeriesList
        ///</summary>
        [TestMethod()]
        public void TryGetSeriesListTest()
        {
            IConferenceHandler target = CreateIConferenceHandler(); // TODO: Initialize to an appropriate value
            HandlerSession session = new HandlerSession(); // TODO: Initialize to an appropriate value

            session.UserName = "zhangxue016";
            session.IP = "192.166.5.190";
            session.Port = "7080";

            List<MeetingSeries> seriesList = null; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;

            actual = target.Login(ref session);
            if (actual == true)
                actual = target.TryGetSeriesList(session, out seriesList);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UpdateMeeting
        ///</summary>
        [TestMethod()]
        public void UpdateMeetingTest()
        {
            IConferenceHandler target = CreateIConferenceHandler(); // TODO: Initialize to an appropriate value
            SVCMMeetingDetail meetingDetail = new SVCMMeetingDetail()
            {
                Id = "",
                Name = "test-tony",
                //AccountName = "",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddMinutes(30),
                ConfMideaType = MideaType.Local,
                ConfType = ConferenceType.Furture,
                EveryFewMonths = 1,
                IPDesc = "23324,333,4343",
                ParticipatorNumber = 3,
                Phone = "138138899",
                Memo = "test",
                Password = "",
                LeaderList = new List<MeetingLeader>()
                {
                    new MeetingLeader()
                    {
                        UserName = "ALIBROKER",
                        Name = "Ali"
                    },
                    new MeetingLeader()
                    {
                        UserName = "CAOLIUYI",
                        Name = "test"
                    },
                    new MeetingLeader()
                    {
                        UserName = "CENXINJIANG757",
                        Name = "test"
                    }
                },
                LeaderRoom = "main room",
                MainRoom = new MeetingRoom() { RoomId = "13483,0,0", Name = "3.52" },
                MobileTermList = new List<MobileTerm>()
                {
                    new MobileTerm() { RoomId = "13702",RoomName="bao/BAO/42342323"},
                    new MobileTerm() { RoomId = "13703",RoomName="bao/BAO/42342"}
                },
                Rooms = new List<MeetingRoom>()
                {
                    new MeetingRoom() { RoomId = "13483,0,0", Name = "3.52" },
                    new MeetingRoom() { RoomId = "13484,0,0", Name = "3.54" }
                },
                VideoSet = VideoSet.Audio,
                MultiExceptDay = "",
                MultiExceptWeek = "",
                RegularMeetingNum = 0,
                RegularMaxNum = 1,
                RegularMeetingType = 1,
                TheFirstFew = 3,
                Week = 4

            }; // TODO: Initialize to an appropriate value
            HandlerSession session = new HandlerSession(); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Login(ref session);
            if (actual)
                actual = target.UpdateMeeting(meetingDetail, session);
            Assert.AreEqual(expected, actual);
        }
    }
}
