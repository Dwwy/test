using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using test.Service;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<dynamic> Get()
        {
            //String connectionString = "Server=202.157.183.113,1533;Database=PersonalColl_LiveClone_20240122;User Id=PC_SystemUser;Password=PC_User@13579";
            //using (IDbConnection db = new SqlConnection(connectionString))
            //{
            //    db.Open();
            //    var result = db.Query<object>("select top 100 * from Tbl_MemberInfo");

            //    foreach (var row in result)
            //    {
            //        // Convert each dynamic row to a dictionary
            //        IDictionary<string, object> dictionaryRow = (IDictionary<string, object>)row;

            //        // Access values by key in string form
            //        foreach (var keyValuePair in dictionaryRow)
            //        {
            //            string key = keyValuePair.Key;

            //            object value = keyValuePair.Value;
            //            Console.WriteLine($"{key}: {value}");
            //        }

            //        // Or, if you want to access a specific value by key
            //        if (dictionaryRow.ContainsKey("YourColumnName"))
            //        {
            //            object specificValue = dictionaryRow["YourColumnName"];
            //            Console.WriteLine($"YourColumnName: {specificValue}");
            //        }

            //        Console.WriteLine("-----------");
            //    }

            //    return Ok(result);

            //}


            //Differences:
            // Params to be replaced will be ? instead of {0}
            // No nid of quotes to be in place for replacements of input
            // Used methods for wwdb is Execute, ExecuteSP, OpenTable and GetDataTable
            // sbSQL no nid to call ToString() method
            //        StringBuilder sbSQL = new StringBuilder();
            //        wwdb db = new wwdb();

            //        sbSQL.Clear();
            //        //sbSQL.AppendFormat($@"
            //        //         SELECT 1
            //        //         FROM tbl_Sales s WITH (NOLOCK)
            //        //         WHERE s.IsDeleted = 0 and s.SalesType = 1 
            //        //          and s.StatusX not in ('P','R','F')
            //        //          and s.MemberID = ?
            //        //        ", "MY85373910");
            //        sbSQL.AppendFormat(@"EXEC [dbo].[SP_TRANSFERWALLET] 
            //@FROMID = ?,
            //@TOID = ?,
            //@AMOUNT = ?,
            //@REMARKS = ?,
            //@WALLETTYPE = ?;", "000001", "MY27749776", 78.0, "", 1);
            //        db.executeScalarSP(sbSQL);
            //        return true;
            return "Hi, Daniel's Test Project test#kin test111";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
