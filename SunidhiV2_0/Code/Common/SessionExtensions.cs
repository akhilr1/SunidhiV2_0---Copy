using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SunidhiV2_0.Code.Common
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session,
                      string key, object value)
        {
            string stringValue = JsonConvert.
                                 SerializeObject(value);
            session.SetString(key, stringValue);
        }

        public static T GetObject<T>(this ISession session,
                                     string key)
        {
            string stringValue = session.GetString(key);
            T value = JsonConvert.DeserializeObject<T>
                                  (stringValue);
            return value;
        }
    }
}
