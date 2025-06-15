using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechMall.Helpers
{
    public static class SessionExtensions
    {
        // Extension method to set the session value
        public static void Set<T>(this HttpSessionStateBase session, string key, T value)
        {
            session[key] = JsonConvert.SerializeObject(value);
        }

        // Extension method to get the session value
        public static T Get<T>(this HttpSessionStateBase session, string key)
        {
            var value = session[key]?.ToString();
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}