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
    [RoutePrefixAttribute("Services")]
    public class ServicesController : ApiController
    {
        ServicesBL servicesBL = new ServicesBL();
        string JsonRequest = "";
        string JsonResponse = "";

        [HttpPost]
        [Route("GetServices")]
        public IHttpActionResult GetServices(ServicesParams param)
        {
            OPDResponseBL responseBL = new OPDResponseBL();

            try
            {
                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                string AuthKey = "";
                string Key = ConfigurationManager.AppSettings["Key"];
                string IV = ConfigurationManager.AppSettings["IV"];
                string APIKey = ConfigurationManager.AppSettings["AuthKey"];

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
                        responseBL = servicesBL.getServiceDetails(param);
                    }
                    else
                    {
                        responseBL.status = "Failed";
                        responseBL.remarks = "Invalid Auth Key";
                    }
                }
                else
                {
                    responseBL.status = "Failed";
                    responseBL.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(responseBL);
                CommonUtilities.fnStoreErrorLog("API", "Controller_getServices", "Request=" + JsonRequest + "Response=" + JsonResponse, "");
            }
            catch (Exception ex)
            {
                //CommonUtilities.FnWriteErrorLog("GetServices_controller", ex.StackTrace);
                responseBL.status = "Failed";
                responseBL.remarks = "Something went wrong";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(responseBL);
                CommonUtilities.fnStoreErrorLog("API", "Controller_getServices", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }

            return Json(responseBL);
        }


        [HttpPost]
        [Route("getStateList")]
        public IHttpActionResult GetStateData()
        {
            StateResponse response = new StateResponse();
            try
            {
                string AuthKey = "";
                string Key = ConfigurationManager.AppSettings["Key"];
                string IV = ConfigurationManager.AppSettings["IV"];
                string APIKey = ConfigurationManager.AppSettings["AuthKey"];

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
                        response = servicesBL.getStateList();
                    }
                    else
                    {
                        response.status = "Failed";
                        response.remarks = "Invalid Auth Key";
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_getStateData", "Request=" + JsonRequest + "Response=" + JsonResponse, "");
            }
            catch (Exception ex)
            {               
                response.status = "Failed";
                response.remarks = "Something went wrong.";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_getStateData", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }

            return Json(response);
        }

        [HttpPost]
        [Route("getCityList")]
        public IHttpActionResult GetCityList(cityListParams param)
        {
            CityResponse response = new CityResponse();
            try
            {
                string AuthKey = "";
                string Key = ConfigurationManager.AppSettings["Key"];
                string IV = ConfigurationManager.AppSettings["IV"];
                string APIKey = ConfigurationManager.AppSettings["AuthKey"];

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
                        if (!string.IsNullOrEmpty(param.stateid))
                        {
                            response = servicesBL.getCityList(param);
                        }
                        else
                        {
                            response.status = "Fail";
                            response.remarks = "State Id can not be blank.";
                        }
                    }
                    else
                    {
                        response.status = "Failed";
                        response.remarks = "Invalid Auth Key";
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_getCityList", "Request=" + JsonRequest + "Response=" + JsonResponse, "");
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went Wrong";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_getCityList", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }

            return Json(response);
        }
    }
}
