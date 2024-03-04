using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Service
{
    public class StringBuilder
    {
        public string query { get; set; } = "";
        public List<string> param { get; set; } = new List<string>();

        public void Clear()
        {
            this.query = "";
            this.param = new List<string>();
        }
        public void Append(string input)
        {
            //string cleanedInput = input.Replace("\r", "").Replace("\n", "").Replace("\t", "");
            query += input;
        }
        public void AppendFormat (string input, params object[] args)
        {
            this.Append(input);
            param.AddRange(args.Select(arg => arg.ToString()));
        }
    }
}
