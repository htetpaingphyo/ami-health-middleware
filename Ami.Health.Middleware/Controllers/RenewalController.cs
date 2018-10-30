using Ami.Health.Middleware.DataModels;
using Ami.Health.Middleware.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace Ami.Health.Middleware.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RenewalController : ControllerBase
    {
        OracleDbContext context = null;
        DataTable temp = null;

        public RenewalController()
        {
            context = new OracleDbContext();
        }

        //GET: renwal
        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }

        //POST: renwal
        [HttpPost]
        public ActionResult Post([FromBody] PolicyRenewalReqModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            temp = context.GetRenewalData(request.FromDate, request.ToDate);

            string jsonRenewalPolicies = JsonConvert.SerializeObject(temp);
            IEnumerable<PolicyRenewal> policyRenewals = JsonConvert.DeserializeObject<IEnumerable<PolicyRenewal>>(jsonRenewalPolicies);

            if (null == policyRenewals)
            {
                return NotFound();
            }
            else
            {
                return new JsonResult(policyRenewals);
            }
        }
    }
}