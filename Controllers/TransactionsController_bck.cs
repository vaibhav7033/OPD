using OPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using System.Configuration;
using System.Web;
using System.IO;

namespace OPD.Controllers
{
    [RoutePrefix("Transactions")]
    public class TransactionsController : ApiController
    {
        string JsonRequest = "";
        string JsonResponse = "";
        [HttpPost]
        [Route("getTransactionDetails")]
        public IHttpActionResult GetTransactionData(TransactionParam transactionParam)
        {
            TransactionsBL transaction = new TransactionsBL();
            ResponsetransBL response = new ResponsetransBL();
            string AuthKey = "";
            string Key = ConfigurationManager.AppSettings["Key"];
            string IV = ConfigurationManager.AppSettings["IV"];
            string APIKey = ConfigurationManager.AppSettings["AuthKey"];

            try
            {
                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(transactionParam);
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
                        if (!string.IsNullOrEmpty(transactionParam.emp_id))
                        {
                            response = transaction.GetTransactionDetails(transactionParam);
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
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_GetTransactionData", "Request=" + JsonRequest + "Response=" + JsonResponse, transactionParam.emp_id);
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went Wrong";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_GetTransactionData", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");

            }

            return Json(response);
        }

        [HttpPost]
        [Route("InsertReimbursementData")]
        public IHttpActionResult InsertReimbursementData(ReimbursementInsertDataParam param)
        {
            TransactionsBL transactionsBL = new TransactionsBL();
            AddTransResponseBL response = new AddTransResponseBL();
            string AuthKey = "";
            string Key = ConfigurationManager.AppSettings["Key"];
            string IV = ConfigurationManager.AppSettings["IV"];
            string APIKey = ConfigurationManager.AppSettings["AuthKey"];

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
                            if (!string.IsNullOrEmpty(param.opd_type_mast_id))
                            {
                                response = transactionsBL.InsertReimbursementData(param);
                            }
                            else
                            {
                                response.status = "Failed";
                                response.remarks = "OPD Type Id cannot be blank.";
                            }
                        }
                        else
                        {
                            response.status = "Failed";
                            response.remarks = "Employee ID cannot be blank.";
                        }
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_InsertReimbursementData", "Request=" + JsonRequest + "Response=" + JsonResponse, param.emp_id);
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went Wrong";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_InsertReimbursementData", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.emp_id);
            }
            return Json(response);
        }

        [HttpPost]
        [Route("InsertReimbursementData_new")]
        public IHttpActionResult InsertReimbursementData_new(ReimbursementInsertDataParamBL param)
        {
            TransactionsBL transactionsBL = new TransactionsBL();
            AddTransResponseBL response = new AddTransResponseBL();
            string AuthKey = "";
            string Key = ConfigurationManager.AppSettings["Key"];
            string IV = ConfigurationManager.AppSettings["IV"];
            string APIKey = ConfigurationManager.AppSettings["AuthKey"];

            try
            {
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
                    if (APIKey == CommonUtilities.Decrypt(Convert.FromBase64String(AuthKey), Convert.FromBase64String(Key), Convert.FromBase64String(IV)))
                    {
                        if (!string.IsNullOrEmpty(param.emp_id))
                        {
                            if (!string.IsNullOrEmpty(param.opd_type_mast_id))
                            {
                                response = transactionsBL.InsertReimbursementData_new(param);
                            }
                            else
                            {
                                response.status = "Failed";
                                response.remarks = "OPD Type Id cannot be blank.";
                            }
                        }
                        else
                        {
                            response.status = "Failed";
                            response.remarks = "Employee ID cannot be blank.";
                        }
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_InsertReimbursementData_new", "Request=" + JsonRequest + "Response=" + JsonResponse, param.emp_id);
            }
            catch (Exception ex)
            {
                //CommonUtilities.FnWriteErrorLog("InsertReimbursementData", ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went Wrong";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_InsertReimbursementData_new", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.emp_id);
            }
            return Json(response);
        }


        [HttpPost]
        [Route("UploadInvoice")]
        public IHttpActionResult UploadInvoiceData(uploadInvoice param)
        {
            TransactionsBL transactionsBL = new TransactionsBL();
            AddTransResponseBL response = new AddTransResponseBL();
            string AuthKey = "";
            string Key = ConfigurationManager.AppSettings["Key"];
            string IV = ConfigurationManager.AppSettings["IV"];
            string APIKey = ConfigurationManager.AppSettings["AuthKey"];

            try
            {



                //Create the Directory.
                string path = HttpContext.Current.Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Fetch the File.
                var httpRequest = HttpContext.Current.Request;
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response1 = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                    }
                }

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
                        if (!string.IsNullOrEmpty(param.empid))
                        {
                            if (!string.IsNullOrEmpty(param.trans_id))
                            {

                                









                                //Fetch the File Name.
                                //        string fileName = Path.GetFileName(postedFile.FileName);

                                ////Save the File.
                                //postedFile.SaveAs(path + fileName);



                                response = transactionsBL.UploadInvoice(param);
                            }
                            else
                            {
                                response.status = "Failed";
                                response.remarks = "Transaction Id cannot be blank.";
                            }
                        }
                        else
                        {
                            response.status = "Failed";
                            response.remarks = "Employee ID cannot be blank.";
                        }
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_UploadInvoiceData", "Request=" + JsonRequest + "Response=" + JsonResponse, param.empid);
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went Wrong";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_UploadInvoiceData", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.empid);
            }
            return Json(response);
            // return Request.CreateResponse(HttpStatusCode.OK, "");



        }

        [HttpPost]
        [Route("InsertCashlessData")]
        public IHttpActionResult InsertCashlessData(CashlessDataParam param)
        {
            TransactionsBL transactionsBL = new TransactionsBL();
            AddTransResponseBL response = new AddTransResponseBL();
            string AuthKey = "";
            string Key = ConfigurationManager.AppSettings["Key"];
            string IV = ConfigurationManager.AppSettings["IV"];
            string APIKey = ConfigurationManager.AppSettings["AuthKey"];

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
                            if (!string.IsNullOrEmpty(param.opd_type_mast_id))
                            {
                                response = transactionsBL.InsertCashlessTransactiondata(param);
                            }
                            else
                            {
                                response.status = "Failed";
                                response.remarks = "Opd Type Id cannot be blank.";
                            }
                        }
                        else
                        {
                            response.status = "Failed";
                            response.remarks = "Employee ID cannot be blank.";
                        }
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "Please pass AuthKey in Headers";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_InsertCashlessData", "Request=" + JsonRequest + "Response=" + JsonResponse, param.emp_id);
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went Wrong";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_InsertCashlessData", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.emp_id);
            }
            return Json(response);
        }

    }
}