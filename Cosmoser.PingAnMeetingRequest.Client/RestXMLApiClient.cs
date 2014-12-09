using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Cosmoser.PingAnMeetingRequest.Client
{
    public class RestXMLApiClient
    {
        public HttpWebResponse DoHttpWebRequest(String url, string data)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.KeepAlive = false;
            req.ContentType = "application/xml";
            req.Method = "POST";
            
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            Stream PostData = req.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            
            return req.GetResponse() as HttpWebResponse;
        }
    }
}
