using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Startup
{
    public static class ParsingJson
    {
        public static void ParsingDemo()
        {
            // Create a new JSON object using a simple syntax.
            JObject jsonObject = JObject.FromObject(new
            {
                channel = new
                {
                    title = "abc",
                    link = "www.baidu.com",
                    description = "antonio's blog"
                }
            });

            Console.WriteLine(jsonObject);
            Console.WriteLine();

            // Query value associated with the specified key in the JSON object.
            Console.WriteLine(jsonObject["channel"]["link"]);

            // Test for JSON array.
            JObject account1 = JObject.FromObject(new
            {
                Name = "Nathan",
                Age = 12
            });
            JObject account2 = JObject.FromObject(new
            {
                Name = "Chris",
                Age = 32
            });
            JObject account3 = JObject.FromObject(new
            {
                Name = "Leon",
                Age = 24
            });

            JArray array = new JArray();
            array.Add(account1);
            array.Add(account2);
            array.Add(account3);
            JObject accounts = JObject.FromObject(new
            {
                Type = "Accounts",
                Body = array
            });

            Console.WriteLine(accounts);
            Console.WriteLine(accounts["Body"][1]["Age"]);

            // Convert it to bytes and convert back to string.
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonObject.ToString());
            string jsonText = Encoding.UTF8.GetString(jsonBytes);
            Console.WriteLine(jsonText);
        }
    }
}
