using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public class RegionCatagory
    {
        public List<MeetingSeries> SeriesList { get; set; }
        public List<RegionInfo> ProvinceList { get; set; }
        public List<RegionInfo> CityList { get; set; }
        public List<RegionInfo> BoroughList { get; set; }
    }
}
