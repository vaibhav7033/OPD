using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OPD.Models;

namespace OPD.Controllers
{
    [RoutePrefix("FeedBack")]
    public class FeedBackController : ApiController
    {
        [HttpPost]
        [Route("getTransactionDetails")]
        private IHttpActionResult GetFeedBackData()
        {
            try
            {

            }
            catch (Exception ex)
            {
                CommonUtilities.FnWriteErrorLog("GetFeedBackData", ex.StackTrace);
            }
        }
    }
}
