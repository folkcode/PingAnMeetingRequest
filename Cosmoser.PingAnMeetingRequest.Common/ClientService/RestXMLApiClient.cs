using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace Cosmoser.PingAnMeetingRequest.Common.ClientService
{
    public class RestXMLApiClient
    {
        public XmlDocument DoHttpWebRequest(String url, string data)
        {
            XmlDocument doc = new XmlDocument();
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.KeepAlive = false;
            req.ContentType = "application/xml";
            req.Method = "POST";

            byte[] buffer = Encoding.UTF8.GetBytes(data); //Encoding.GetEncoding("GB2312").GetBytes(data);
            Stream PostData = req.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();

            using (var response = req.GetResponse() as HttpWebResponse)
            {
                Encoding encoding = Encoding.GetEncoding("GB2312");

                StreamReader reader = new StreamReader(response.GetResponseStream(),encoding);

                string result = reader.ReadToEnd();

                doc.LoadXml(result);
            }

            return doc;
        }
    }
}
