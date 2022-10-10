using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OPD.Models;

namespace OPD.Controllers
{
    [RoutePrefixAttribute("RaiseQuery")]
    public class RaiseQueryController : ApiController
    {
        RaiseQueryBL raiseQueryBL = new RaiseQueryBL();
        string AuthKey = "";
        string Key = ConfigurationManager.AppSettings["Key"];
        string IV = ConfigurationManager.AppSettings["IV"];
        string APIKey = ConfigurationManager.AppSettings["AuthKey"];
        string JsonRequest = "";
        string JsonResponse = "";

        [HttpPost]
        [Route("AddQuery")]
        public IHttpActionResult AddQuery(AddQueryParams param)
        {
            QueryResponse response = new QueryResponse();

            try
            {
                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                var re = Request;
                var headers = re.Headers;

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
                        if (!string.IsNullOrEmpty(param.emp_id))
                        {
                            if (!string.IsNullOrEmpty(param.queryType))
                            {
                                response = raiseQueryBL.AddNewQuery(param);
                            }
                            else
                            {
                                response.status = "Failed";
                                response.remarks = "Query Type cannot be blank.";
                            }
                        }
                        else
                        {
                            response.status = "Failed";
                            response.remarks = "Employee ID cannot be blank.";
                        }
                    }
                    else
                    {
                        response.status = "Failed";
                        response.remarks = "Invalid AuthKey in Headers";
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_AddQuery", "Request=" + JsonRequest + "Response=" + JsonResponse, param.emp_id);
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong.";

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_AddQuery", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.emp_id);
            }
            return Json(response);
        }

        [HttpPost]
        [Route("GiveFeedBack")]
        public IHttpActionResult GiveFeedBack(FeedBackParams param)
        {
            QueryResponse response = new QueryResponse();

            try
            {
                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                var re = Request;
                var headers = re.Headers;

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
                        if (!string.IsNullOrEmpty(param.emp_id))
                        {
                            if (!string.IsNullOrEmpty(param.ratings))
                            {
                                response = raiseQueryBL.GiveFeedback(param);
                            }
                            else
                            {
                                response.status = "Failed";
                                response.remarks = "Please rate Us.";
                            }
                        }
                        else
                        {
                            response.status = "Failed";
                            response.remarks = "Employee ID cannot be blank.";
                        }
                    }
                    else
                    {
                        response.status = "Failed";
                        response.remarks = "Invalid AuthKey in Headers";
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_GiveFeedback", "Request=" + JsonRequest + "Response=" + JsonResponse, param.emp_id);
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong";

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_GiveFeedback", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.emp_id);
            }
            return Json(response);
        }

        [HttpPost]
        [Route("GetFAQDetails")]
        public IHttpActionResult GetFAQDetails()
        {
            FAQResponse response = new FAQResponse();

            try
            {
                var re = Request;
                var headers = re.Headers;

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
                        response = raiseQueryBL.getFAQData();
                    }
                    else
                    {
                        response.status = "Failed";
                        response.remarks = "Invalid AuthKey in Headers";
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_getFAQDetails", "Request=" + JsonRequest + "Response=" + JsonResponse, "");
            }
            catch (Exception ex)
            {
                //CommonUtilities.FnWriteErrorLog("GetFAQDetails_controller", ex.StackTrace);
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_getFAQDetails", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }
            return Json(response);
        }
    }
}
