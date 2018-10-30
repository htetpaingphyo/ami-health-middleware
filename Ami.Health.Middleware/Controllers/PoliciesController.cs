using Ami.Health.Middleware.DataModels;
using Ami.Health.Middleware.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ami.Health.Middleware.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        OracleDbContext context = null;
        DataTable tempData = null;

        public PoliciesController()
        {
            context = new OracleDbContext();
        }

        // GET: policies
        [HttpGet]
        public ActionResult Get()
        {
            /** Method changed 'coz of report format changed! **/

            //var testPolicy = "AMI/YGN/LHI/18000004";
            //var base64encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(testPolicy));
            //return Get(base64encoded);
            return Ok();
        }

        // GET: policies/xxx-xxx-xxx
        [HttpGet("{id}", Name = "Get")]
        public ActionResult Get(string id)
        {
            //Convert base64 encoded string to regular string.
            byte[] decode_id = Convert.FromBase64String(id);
            id = Encoding.UTF8.GetString(decode_id);

            //Populate data to temp data table.
            tempData = context.GetPopulatedData(id);

            if (tempData == null)
            {
                return NotFound();
            }

            string jsonPolicy = JsonConvert.SerializeObject(tempData);
            //Modified by Htet Paing 'coz of report format changed!
            //IEnumerable<Policy> policies = JsonConvert.DeserializeObject<IEnumerable<Policy>>(jsonPolicies);
            IEnumerable<PolicyInfo> policies = JsonConvert.DeserializeObject<IEnumerable<PolicyInfo>>(jsonPolicy);

            if (policies == null)
            {
                return NotFound();
            }
            else
            {
                var policy = policies.SingleOrDefault(x => x.POLICY_NO == id);
                if (policy == null)
                {
                    return NotFound();
                }
                else
                {
                    return new JsonResult(policy);
                }
            }
        }

        // PUT: policies/xxx-xxx-xxx
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            return BadRequest();
        }

        // PUT: policies/xxx-xxx-xxx
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            return BadRequest();
        }

        // DELETE: policies/xxx-xxx-xxx
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return BadRequest();
        }
    }
}
