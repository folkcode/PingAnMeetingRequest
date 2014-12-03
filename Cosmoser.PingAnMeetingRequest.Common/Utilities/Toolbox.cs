using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace Cosmoser.PingAnMeetingRequest.Common.Utilities
{
    public static class Toolbox
    {
        public static string Serialize<T>(T serializeObj)
        {
            MemoryStream stream = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream, serializeObj);
            string objString = Convert.ToBase64String(stream.ToArray());
            return objString;
        }
        public static TReturn Deserialize<TReturn>(string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            MemoryStream stream = new MemoryStream(bytes);
            DataContractSerializer serializer = new DataContractSerializer(typeof(TReturn));
            TReturn obj = (TReturn)serializer.ReadObject(stream);
            return obj;
        }
    }
}