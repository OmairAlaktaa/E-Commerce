using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProkodersECommerce.Models;
using System.Collections.Generic;

namespace ProkodersECommerce.Helpers
{
    public static class SessionCartHelper
    {
        private const string CartSessionKey = "CartSession";

        public static List<CartItem> GetCartItems(ISession session)
        {
            var json = session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(json);
        }

        public static void SaveCartItems(ISession session, List<CartItem> items)
        {
            var json = JsonConvert.SerializeObject(items);
            session.SetString(CartSessionKey, json);
        }

        public static void ClearCart(ISession session)
        {
            session.Remove(CartSessionKey);
        }
    }
}
