using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using System.Configuration;
using RestSharp;
using OPD.Models;

namespace OPD.Controllers
{
    [RoutePrefix("bank")]
    public class bankController : ApiController
    {

        [HttpPost]
        [Route("getBankDetails")]
        public IHttpActionResult GetBankDetails(Bankmaster bankmaster)
        {
            BankmasterBL bank = new BankmasterBL();
            BankBL response = new BankBL();
            string AuthKey = "";
            string Key = ConfigurationManager.AppSettings["Key"];
            string IV = ConfigurationManager.AppSettings["IV"];
            string APIKey = ConfigurationManager.AppSettings["AuthKey"];

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
                        if (!string.IsNullOrEmpty(bankmaster.BankId) & bankmaster.BankId != "0")
                        {
                            response = bank.GetBankDetails(bankmaster);
                        }
                        else
                        {
                            response.bankstatus = "Failed";
                            response.bankremarks = "Bank Id cannot be Zero";
                        }
                    }
                    else
                    {
                        response.bankstatus = "Failed";
                        response.bankremarks = "Invalid Auth Key";
                    }
                }
                else
                {
                    response.bankstatus = "Failed";
                    response.bankremarks = "Please pass AuthKey in Headers";
                }

            }
            catch (Exception ex)
            {


            }
            //CommonUtilities.Decrypt();

            return Json(response);
        }

        [HttpPost]
        [Route("InsertBankData")]
        public IHttpActionResult InsertBankData(Bankmaster bankmaster)
        {
            BankmasterBL addbank = new BankmasterBL();
            InsertBank response = new InsertBank();
            string AuthKey = "";
            string Key = ConfigurationManager.AppSettings["Key"];
            string IV = ConfigurationManager.AppSettings["IV"];
            string APIKey = ConfigurationManager.AppSettings["AuthKey"];
            Tuple<string, string> remark;

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
                        if (!string.IsNullOrEmpty(bankmaster.Bankname))
                        {

                            if (!string.IsNullOrEmpty(bankmaster.Bankcode))
                            {
                                remark = CommonUtilities.validation(bankmaster.Bankname, bankmaster.Bankcode);
                                if (remark.Item1 == "")
                                {
                                    if (remark.Item2 == "")
                                    {


                                        response = addbank.InsertBankDetails(bankmaster);
                                        response.bankstatus = "Succesful";
                                        response.bankremarks = "Bank Inserted Successfully";
                                    }
                                    else
                                    {

                                        response.bankstatus = "";
                                        response.bankremarks = remark.Item2;
                                    }
                                }

                                else
                                {
                                    response.bankstatus = "Failed";
                                    response.bankremarks = remark.Item1;
                                }
                            }
                        }
                        else
                        {
                            response.bankstatus = "Failed";
                            response.bankremarks = "Bank Code cannot be blank.";
                        }
                    }
                    else
                    {
                        response.bankstatus = "Failed";
                        response.bankremarks = "Bank Name cannot be blank.";
                    }
                }
            
                
                else
                {
                    response.bankstatus = "Failed";
                    response.bankremarks = "Please pass AuthKey in Headers";
                }

                //CommonUtilities.validation("API", "Controller_InsertBankData", "Request=" + Request + "Response=" + response, "");

            }
            catch (Exception ex)
            {


            }
            //CommonUtilities.Decrypt();

            return Json(response);
        }

    }
}



