using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ami.Health.Middleware.Models
{
    public class PolicyRenewalReqModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}