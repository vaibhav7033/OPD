using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using OPD.Models;

namespace OPD.Models
{
    public class RaiseQueryBL
    {
        DBHelper dBHelper = new DBHelper();
        string JsonRequest = "";
        string JsonResponse = "";
        public QueryResponse AddNewQuery(AddQueryParams param)
        {
            QueryResponse response = new QueryResponse();
            DataTable dtGetResponse = new DataTable();
            try
            {
                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "AddQuery"));
                paramList.Add(new SqlParameter("@emp_id", param.emp_id));
                paramList.Add(new SqlParameter("@querytype", param.queryType));
                paramList.Add(new SqlParameter("@querydescription", param.queryDescription));
                dtGetResponse = dBHelper.GetTableFromSP("SP_RaiseQuery", paramList.ToArray());
                if (dtGetResponse.Rows.Count > 0)
                {
                    response.status = Convert.ToString(dtGetResponse.Rows[0]["Result"]);
                    response.remarks = Convert.ToString(dtGetResponse.Rows[0]["remarks"]);
                }                
            }
            catch (Exception ex)
            {
                //CommonUtilities.FnWriteErrorLog("AddNewQuery", ex.StackTrace);
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "RaiseQueryBL_AddNewQuery", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.emp_id);
            }

            return response;
        }

        public QueryResponse GiveFeedback(FeedBackParams param)
        {
            QueryResponse response = new QueryResponse();
            DataTable dtGetResponse = new DataTable();
            JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(param);
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "AddFeedBack"));
                paramList.Add(new SqlParameter("@emp_id", param.emp_id));
                paramList.Add(new SqlParameter("@ratings", param.ratings));
                paramList.Add(new SqlParameter("@querydescription", param.remarks));
                dtGetResponse = dBHelper.GetTableFromSP("SP_RaiseQuery", paramList.ToArray());
                if (dtGetResponse.Rows.Count > 0)
                {
                    response.status = Convert.ToString(dtGetResponse.Rows[0]["Result"]);
                    response.remarks = Convert.ToString(dtGetResponse.Rows[0]["remarks"]);
                }
            }
            catch (Exception ex)
            {
                //CommonUtilities.FnWriteErrorLog("GiveFeedback", ex.StackTrace);
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "RaiseQueryBL_GiveFeedback", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.emp_id);
            }

            return response;
        }


        public FAQResponse getFAQData()
        {
            FAQResponse response = new FAQResponse();
            DataTable dtFAQDetails = new DataTable();
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "getFAQ"));
                dtFAQDetails = dBHelper.GetTableFromSP("SP_RaiseQuery", paramList.ToArray());
                if (dtFAQDetails.Rows.Count > 0)
                {
                    response.status = "Success";
                    response.remarks = "";
                    FAQParams faqData = new FAQParams();
                    List<FAQParams> lstFaqDetails = new List<FAQParams>();
                    foreach (DataRow dr in dtFAQDetails.Rows)
                    {

                        faqData.FAQ_TITLE = Convert.ToString(dr["FAQ_TITLE"]);
                        faqData.FAQ_DESCRIPTION = Convert.ToString(dr["FAQ_DESCRIPTION"]);

                        lstFaqDetails.Add(faqData);
                        faqData = new FAQParams();
                    }

                    response.lstFAQDetails = lstFaqDetails;
                }
                else
                {
                    response.status = "Fail";
                    response.remarks = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                //CommonUtilities.FnWriteErrorLog("getFAQData", ex.StackTrace);
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "RaiseQueryBL_getFAQData", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }

            return response;
        }
    }

    public class AddQueryParams
    {
        public string queryType { get; set; }
        public string queryDescription { get; set; }
        public string emp_id { get; set; }
        public string user_id { get; set; }
    }

    public class QueryResponse
    {
        public string status { get; set; }
        public string remarks { get; set; }
    }

    public class FAQParams
    {
        public string FAQ_TITLE { get; set; }

        public string FAQ_DESCRIPTION { get; set; }
    }
    
    public class FAQResponse
    {
        public string status { get; set; }
        public string remarks { get; set; }
        public List<FAQParams> lstFAQDetails { get; set; }
    }

    public class FeedBackParams
    {
        public string emp_id { get; set; }
        public string ratings { get; set; }
        public string remarks { get; set; }
    }
}