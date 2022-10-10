using OPD.Models;
using System;
using System.Linq;
using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using System.Configuration;
using RestSharp;


namespace OPD.Controllers
{
    [RoutePrefix("Employee")]
    public class EmployeeController : ApiController
    {
        string JsonRequest = "";
        string JsonResponse = "";

        [HttpPost]
        [Route("getEmployeeDetails")]
        public IHttpActionResult GetEmployeeData(EmployeeParam employeeParam)
        {
            EmployeeBL employee = new EmployeeBL();
            ResponseBL response = new ResponseBL();

            try
            {
                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(employeeParam);
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
                    try
                    {
                        if (APIKey == CommonUtilities.Decrypt(Convert.FromBase64String(AuthKey), Convert.FromBase64String(Key), Convert.FromBase64String(IV)))
                        {
                            if (!string.IsNullOrEmpty(employeeParam.emp_id))
                            {
                                response = employee.GetEmployeeDetails(employeeParam);
                            }
                            else
                            {
                                response.status = "Failed";
                                response.remarks = "Emp Id cannot be blank";
                            }
                        }
                        else
                        {
                            response.status = "Failed";
                            response.remarks = "Invalid Auth Key";
                        }

                        JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        CommonUtilities.fnStoreErrorLog("API", "Controller_GetEmployeeData", "Request=" + JsonRequest + " Response=" + JsonResponse, employeeParam.emp_id);
                    }
                    catch (Exception ex)
                    {
                        response.status = "Failed";
                        response.remarks = "Please enter valid Auth Key in Headers.";
                        JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        //CommonUtilities.FnWriteErrorLog("GetEmployeeData_AUthkey", ex.StackTrace);
                        CommonUtilities.fnStoreErrorLog("API", "Controller_GetEmployeeData", "Request=" + JsonRequest + " Response=" + JsonResponse + " Exception=" + ex.StackTrace, employeeParam.emp_id);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                    JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    CommonUtilities.fnStoreErrorLog("API", "Controller_GetEmployeeData", "Request=" + JsonRequest + " Response=" + JsonResponse, employeeParam.emp_id);
                }

            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong. Please co-ordinate with IT Team.";

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_GetEmployeeData", "Request=" + JsonRequest + " Response=" + JsonResponse + " Exception=" + ex.StackTrace, employeeParam.emp_id);
            }
            //CommonUtilities.Decrypt();

            return Json(response);
        }

        [HttpPost]
        [Route("getWalletStatus")]
        public IHttpActionResult CheckWalletActive(WalletStatusParam param)
        {
            EmployeeBL employee = new EmployeeBL();
            WalletStatusResponse response = new WalletStatusResponse();


            try
            {
                string AuthKey = "";
                string Key = ConfigurationManager.AppSettings["Key"];
                string IV = ConfigurationManager.AppSettings["IV"];
                string APIKey = ConfigurationManager.AppSettings["AuthKey"];
                var re = Request;
                var headers = re.Headers;

                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(param);

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
                    try
                    {
                        if (APIKey == CommonUtilities.Decrypt(Convert.FromBase64String(AuthKey), Convert.FromBase64String(Key), Convert.FromBase64String(IV)))
                        {
                            if (!string.IsNullOrEmpty(param.emp_id))
                            {
                                response = employee.getWalletStatus(param);
                            }
                            else
                            {
                                response.status = "Failed";
                                response.remarks = "Emp Id cannot be blank";
                            }
                        }
                        else
                        {
                            response.status = "Failed";
                            response.remarks = "Invalid Auth Key";
                        }
                    }
                    catch (Exception ex)
                    {
                        response.status = "Failed";
                        response.remarks = "Please enter valid Auth Key in Headers.";
                        JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        CommonUtilities.fnStoreErrorLog("API", "Controller_CheckWalletActive", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.emp_id);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_CheckWalletActive", "Request=" + JsonRequest + "Response=" + JsonResponse, param.emp_id);
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong. Please co-ordinate with IT Team.";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_CheckWalletActive", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.emp_id);
            }
            //CommonUtilities.Decrypt();

            return Json(response);
        }

        [HttpPost]
        [Route("validateMobileNo")]
        public IHttpActionResult ValidateMobileNumber(VerifyEmployee param)
        {
            EmployeeBL employee = new EmployeeBL();
            ResponseBL response = new ResponseBL();

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
                    try
                    {
                        if (APIKey == CommonUtilities.Decrypt(Convert.FromBase64String(AuthKey), Convert.FromBase64String(Key), Convert.FromBase64String(IV)))
                        {
                            if (!string.IsNullOrEmpty(param.mobileNo))
                            {
                                response = employee.VerifyEmployee(param);
                            }
                            else
                            {
                                response.status = "Failed";
                                response.remarks = "Mobile No cannot be blank";
                            }
                        }
                        else
                        {
                            response.status = "Failed";
                            response.remarks = "Invalid Auth Key";
                        }

                        JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        CommonUtilities.fnStoreErrorLog("API", "Controller_GetEmployeeData", "Request=" + JsonRequest + " Response=" + JsonResponse, param.mobileNo);
                    }
                    catch (Exception ex)
                    {
                        response.status = "Failed";
                        response.remarks = "Please enter valid Auth Key in Headers.";
                        JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        //CommonUtilities.FnWriteErrorLog("GetEmployeeData_AUthkey", ex.StackTrace);
                        CommonUtilities.fnStoreErrorLog("API", "Controller_GetEmployeeData", "Request=" + JsonRequest + " Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.mobileNo);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                    JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    CommonUtilities.fnStoreErrorLog("API", "Controller_ValidateMobileNumber", "Request=" + JsonRequest + " Response=" + JsonResponse, param.mobileNo);
                }

            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong. Please co-ordinate with IT Team.";

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_GetEmployeeData", "Request=" + JsonRequest + " Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.mobileNo);
            }

            return Json(response);

        }

        [HttpPost]
        [Route("VerifyMobileNo")]
        public IHttpActionResult VerifyMobileNumber(VerifyEmployee param)
        {
            EmployeeBL employee = new EmployeeBL();
            VerifyEmployeeResponse response = new VerifyEmployeeResponse();

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
                    try
                    {
                        if (APIKey == CommonUtilities.Decrypt(Convert.FromBase64String(AuthKey), Convert.FromBase64String(Key), Convert.FromBase64String(IV)))
                        {
                            if (!string.IsNullOrEmpty(param.mobileNo))
                            {
                                response = employee.ValidateMobileNumber(param);
                                if (response.status == "Success")
                                {
                                    Random generator = new Random();
                                    String OTP = generator.Next(0, 1000000).ToString("D6");

                                    string whatsappAPI = ConfigurationManager.AppSettings["whatsAppApi"];
                                    string whatsappKey = ConfigurationManager.AppSettings["WAkey"];

                                    CommunicationType communicationType = new CommunicationType();
                                    getSMS getSMSParam = new getSMS();
                                    getSMSParam.type = "getSMS";
                                    getSMSParam.mode = "1057";
                                    communicationType = employee.getSMSScript(getSMSParam);
                                    string whatsappMsg = "";

                                    if (communicationType != null)
                                    {
                                        whatsappMsg = communicationType.COMM_DESCRIPTION;
                                        whatsappMsg = whatsappMsg.Replace("<OTP>", OTP);
                                        whatsappMsg = whatsappMsg.Replace("<url>", "https://e-walletscanner.alphonsol.com");
                                    }

                                    string completeAPI = whatsappAPI + whatsappKey + "&mobile=91" + param.mobileNo + "&msg=" + whatsappMsg;

                                    try
                                    {
                                        var client = new RestClient(completeAPI);
                                        client.Timeout = -1;
                                        var request = new RestRequest(Method.POST);
                                        IRestResponse waResponse = client.Execute(request);
                                        Console.WriteLine(waResponse.Content);
                                        dynamic results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(waResponse.Content);
                                        string status = results.status;
                                        string smsErrorMsg = results.errormsg;
                                        string msgrequestid = results.requestid;

                                        smsLogParam smsLog = new smsLogParam();
                                        smsLog.type = "saveSMSLog";
                                        smsLog.status = status;
                                        smsLog.message = whatsappMsg;
                                        smsLog.response = waResponse.Content;
                                        smsLog.response_id = msgrequestid;
                                        smsLog.mobile_no = param.mobileNo;
                                        smsLog.emp_id = response.empid;
                                        smsLog.comm_type_id = communicationType.comm_type_id;
                                        smsLog.OTP = OTP;
                                        communicationType = employee.saveSMSLog(smsLog);

                                        if (status == "success")
                                        {
                                            response.status = "Success";
                                            response.remarks = communicationType.remarks;
                                        }
                                        else
                                        {
                                            response.status = status;
                                            response.remarks = smsErrorMsg;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        response.status = "Failed";
                                        response.remarks = "Please enter valid Auth Key in Headers.";
                                        JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                                        CommonUtilities.fnStoreErrorLog("API", "Controller_VerifyMobileNumber", "Request=" + JsonRequest + " Response=" + JsonResponse + " Exception=" + ex.StackTrace + "Message=" + ex.Message, param.mobileNo);
                                    }
                                }
                            }
                            else
                            {
                                response.status = "Failed";
                                response.remarks = "Mobile No cannot be blank";
                            }
                        }
                        else
                        {
                            response.status = "Failed";
                            response.remarks = "Invalid Auth Key";
                        }
                    }
                    catch (Exception ex)
                    {
                        response.status = "Failed";
                        response.remarks = "Please enter valid Auth Key in Headers.";
                        JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        CommonUtilities.fnStoreErrorLog("API", "Controller_VerifyMobileNumber", "Request=" + JsonRequest + " Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.mobileNo);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                    JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    CommonUtilities.fnStoreErrorLog("API", "Controller_VerifyMobileNumber", "Request=" + JsonRequest + " Response=" + JsonResponse, param.mobileNo);
                }
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong. Please co-ordinate with IT Team.";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_VerifyMobileNumber", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }

            return Json(response);
        }

        [HttpPost]
        [Route("VerifyOTP")]
        public IHttpActionResult VerifyOTP(verifyOTPParams param)
        {
            EmployeeBL employee = new EmployeeBL();
            OtpresposeBL response = new OtpresposeBL();
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
                    try
                    {
                        if (APIKey == CommonUtilities.Decrypt(Convert.FromBase64String(AuthKey), Convert.FromBase64String(Key), Convert.FromBase64String(IV)))
                        {
                            if (!string.IsNullOrEmpty(param.mobileNo))
                            {
                                if (!string.IsNullOrEmpty(param.otp))
                                {
                                    response = employee.VerifyOTP(param);                                    
                                }
                                else
                                {
                                    response.status = "Failed";
                                    response.remarks = "OTP cannot be blank";
                                }
                            }
                            else
                            {
                                response.status = "Failed";
                                response.remarks = "Mobile No cannot be blank";
                            }
                        }
                        else
                        {
                            response.status = "Failed";
                            response.remarks = "Invalid Auth Key";
                        }
                    }
                    catch (Exception ex)
                    {
                        response.status = "Failed";
                        response.remarks = "Please enter valid Auth Key in Headers.";
                        JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        CommonUtilities.fnStoreErrorLog("API", "Controller_VerifyMobileNumber", "Request=" + JsonRequest + " Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.mobileNo);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                    JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    CommonUtilities.fnStoreErrorLog("API", "Controller_VerifyMobileNumber", "Request=" + JsonRequest + " Response=" + JsonResponse, param.mobileNo);
                }
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong. Please co-ordinate with IT Team.";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_VerifyMobileNumber", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }

            return Json(response);
        }
    }
}
