using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OPD.Models;
using System.Configuration;
using RouteAttribute = System.Web.Http.RouteAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;


namespace OPD.Controllers
{
    [RoutePrefixAttribute("Instruction")]
    public class InstructionController : ApiController
    {
        string jsonRequest = "";
        string jsonResponse = "";

        [HttpPost]
        [Route("GetInstruction")]
        public IHttpActionResult GetInstruction(lookupParam props)
        {
            InstructionsBL instructions = new InstructionsBL();
            LookUpResBL lookUpRespo = new LookUpResBL();
            string AuthKey = "";
            string Key = ConfigurationManager.AppSettings["Key"];
            string IV = ConfigurationManager.AppSettings["IV"];
            string APIKey = ConfigurationManager.AppSettings["AuthKey"];
            try
            {
                jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(props);
                var re = Request;
                var headers = re.Headers;
                int lid = props.lookupid;
                string ltxt = props.lookuptext;
                if (headers.Contains("AuthKey"))
                {
                    AuthKey = headers.GetValues("AuthKey").First();
                }
                else
                {
                    AuthKey = "";
                }
                if (!string.IsNullOrEmpty(AuthKey))
                {
                    if (APIKey == CommonUtilities.Decrypt(Convert.FromBase64String(AuthKey), Convert.FromBase64String(Key), Convert.FromBase64String(IV)))
                    {
                        if (!string.IsNullOrEmpty(ltxt))
                        {
                            lookUpRespo = instructions.getInstruction(props);
                        }
                        else
                        {
                            lookUpRespo.Status = "Failed";
                            lookUpRespo.Remarks = "lookupTxt and lookupid cannot be blank";
                        }
                    }
                    else
                    {
                        lookUpRespo.Status = "Failed";
                        lookUpRespo.Remarks = "Invalid Auth Key";
                    }
                }
                else
                {
                    lookUpRespo.Status = "Failed";
                    lookUpRespo.Remarks = "Please pass AuthKey in Headers";
                }

                jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(lookUpRespo);                
                CommonUtilities.fnStoreErrorLog("API", "InstructionController_getInstruction", "Request=" + jsonRequest + "Response=" + jsonResponse, "");
            }
            catch (Exception ex)
            {
                lookUpRespo.Status = "Failed";
                lookUpRespo.Remarks = "Something went wrong";
                jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(lookUpRespo);                
                CommonUtilities.fnStoreErrorLog("API", "InstructionController_getInstruction", "Request=" + jsonRequest + "Response=" + jsonResponse + " Exception=" + ex.StackTrace, "");
            }
            return Json(lookUpRespo);
        }
        
    }
}