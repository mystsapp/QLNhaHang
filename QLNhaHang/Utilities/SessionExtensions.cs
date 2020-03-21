using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Utilities
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static IEnumerable<T> Gets<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value != null)
            {
                IEnumerable<T> deserializedlistobj = (IEnumerable<T>)JsonConvert.DeserializeObject(value, typeof(IEnumerable<T>));
                return deserializedlistobj;
            }
            return null;
            //return value == null ? default(T) :
            //    JsonConvert.DeserializeObject<T>(value);
        }

        public static T GetSingle<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}