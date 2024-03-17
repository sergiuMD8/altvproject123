using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltvServeru
{
    class Utils
    {
        public static void adminLog(string text, string username)
        {
            HTTP.Post("https://discord.com/api/webhooks/1218905530630930452/XiF4cloGcnoBgSipKdOHIwWQOVkHmR0bjXEXH0F3VZMk8CNjZeMeXBYMv0JqivQnScdL", new System.Collections.Specialized.NameValueCollection
            {

                {
                    "username",
                        username
                },
                {
                    "content",
                        text
                }

            });
        }
    }
}
