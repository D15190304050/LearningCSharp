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

            // Convert it to bytes and convert back to string.
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonObject.ToString());
            string jsonText = Encoding.UTF8.GetString(jsonBytes);
            Console.WriteLine(jsonText);
        }
    }
}
