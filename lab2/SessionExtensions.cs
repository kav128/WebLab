using System;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace lab2
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T? value) where T : class
        {
            string jsonString = JsonSerializer.Serialize(value);
            byte[] bytes = Encoding.Default.GetBytes(jsonString);
            
            session.Set(key, bytes);
        }

        public static T? Get<T>(this ISession session, string key) where  T : class
        {
            byte[] bytes = session.Get(key);

            if (bytes is null) return null;

            string jsonString = Encoding.Default.GetString(bytes);
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
