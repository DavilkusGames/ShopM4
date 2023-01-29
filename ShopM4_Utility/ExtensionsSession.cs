using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ShopM4_Utility
{
    // МЕТОДЫ РАСШИРЕНИЯ - гуглить, чтобы понять как это работает

    public static class ExtensionsSession
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}

