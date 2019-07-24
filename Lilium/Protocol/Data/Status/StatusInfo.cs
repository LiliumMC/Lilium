using System;
using System.Collections.Generic;
using System.Text;

namespace Lilium.Protocol.Data.Status
{
    public class StatusInfo
    {
        private Dictionary<string, object> info = new Dictionary<string, object>();
        public void Add(string key,object value)
        {
            info.Add(key, value);
        }
        public void Remove(string key)
        {
            info.Remove(key);
        }
        public override string ToString()
        {
            List<string> stringBuilder = new List<string>();
            foreach(string key in info.Keys)
            {
                stringBuilder.Add(string.Format("\"{0}\":{1}", key, info[key]));
            }
            return "{"+string.Join(",", stringBuilder)+"}";
        }
    }
}
