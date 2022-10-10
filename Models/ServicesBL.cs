using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OPD.Models
{
    public class ServicesBL
    {
        string JsonRequest = "";
        string JsonResponse = "";
        public OPDResponseBL getServiceDetails(ServicesParams param)
        {
            OPDResponseBL response = new OPDResponseBL();
            try
            {
                DataTable dtInstruction = new DataTable();

                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetServicesList"));
                paramList.Add(new SqlParameter("@servicetype", param.servicetype));
                paramList.Add(new SqlParameter("@stateid", param.stateid));
                paramList.Add(new SqlParameter("@cityid", param.cityid));
                paramList.Add(new SqlParameter("@pincode", param.pincode));
                DBHelper dBHelper = new DBHelper();
                dtInstruction = dBHelper.GetTableFromSP("SP_OPDServiceDetails", paramList.ToArray());

                if (dtInstruction.Rows.Count > 0)
                {
                    response.status = "Success";
                    response.remarks = "";
                    ServicesDetails servicesDetails = new ServicesDetails();
                    List<ServicesDetails> lstServicesDetails = new List<ServicesDetails>();
                    foreach (DataRow dr in dtInstruction.Rows)
                    {

                        servicesDetails.state = Convert.ToString(dr["state"]);
                        servicesDetails.provider_address = dr["provider_address"].ToString();
                        servicesDetails.latitude = dr["latitude"].ToString();
                        servicesDetails.longitude = Convert.ToString(dr["longitude"]);
                        servicesDetails.provider_name = Convert.ToString(dr["provider_name"]);
                        servicesDetails.provider_type = Convert.ToString(dr["provider_type"]);
                        servicesDetails.mobile_no = Convert.ToString(dr["mobile_no"]);
                        servicesDetails.email_id = Convert.ToString(dr["email_id"]);
                        servicesDetails.city = Convert.ToString(dr["city"]);
                        servicesDetails.pincode = Convert.ToString(dr["pincode"]);
                        servicesDetails.image = Convert.ToString(dr["image"]);
                        //response.details = resList;
                        lstServicesDetails.Add(servicesDetails);
                        servicesDetails = new ServicesDetails();
                    }

                    response.opdServices = lstServicesDetails;
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "NO data Found";
                }
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went Wrong";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_getServiceDetails", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }

            return response;
        }

        public StateResponse getStateList()
        {
            StateResponse response = new StateResponse();
            try
            {
                DataTable dtStateList = new DataTable();

                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetStateList"));
                DBHelper dBHelper = new DBHelper();
                dtStateList = dBHelper.GetTableFromSP("SP_OPDServiceDetails", paramList.ToArray());

                if (dtStateList.Rows.Count > 0)
                {
                    response.status = "Success";
                    response.remarks = "";
                    StateDetails stateList = new StateDetails();
                    List<StateDetails> lstStateDeatils = new List<StateDetails>();
                    foreach (DataRow dr in dtStateList.Rows)
                    {

                        stateList.stateId = Convert.ToString(dr["state_id"]);
                        stateList.statename = Convert.ToString(dr["state_name"]);

                        lstStateDeatils.Add(stateList);
                        stateList = new StateDetails();
                    }

                    response.stateList = lstStateDeatils;
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "NO data Found";
                }
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong";
                // CommonUtilities.FnWriteErrorLog("getStateList", ex.StackTrace);
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_getStateList", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }

            return response;
        }

        public CityResponse getCityList(cityListParams param)
        {
            CityResponse response = new CityResponse();
            try
            {
                DataTable dtCityList = new DataTable();

                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetCityList"));
                paramList.Add(new SqlParameter("@stateid", param.stateid));
                DBHelper dBHelper = new DBHelper();
                dtCityList = dBHelper.GetTableFromSP("SP_OPDServiceDetails", paramList.ToArray());

                if (dtCityList.Rows.Count > 0)
                {
                    response.status = "Success";
                    response.remarks = "";
                    CityDetails cityList = new CityDetails();
                    List<CityDetails> lstCityDetails = new List<CityDetails>();
                    foreach (DataRow dr in dtCityList.Rows)
                    {

                        cityList.cityId = Convert.ToString(dr["city_id"]);
                        cityList.cityName = Convert.ToString(dr["city_name"]);

                        lstCityDetails.Add(cityList);
                        cityList = new CityDetails();
                    }

                    response.cityList = lstCityDetails;
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "NO data Found";
                }
            }
            catch (Exception ex)
            {
                //CommonUtilities.FnWriteErrorLog("getCityList", ex.StackTrace);
                response.status = "Failed";
                response.remarks = "Something went wrong";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "Controller_getCityList", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }
            return response;
        }
    }

    public class ServicesParams
    {
        public string servicetype { get; set; }
        public string stateid { get; set; }
        public string cityid { get; set; }
        public string pincode { get; set; }

    }

    public class ServicesDetails
    {
        public string provider_name { get; set; }
        public string provider_type { get; set; }
        public string mobile_no { get; set; }
        public string email_id { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string provider_address { get; set; }
        public string image { get; set; }

    }

    public class OPDResponseBL
    {
        public string status { get; set; }
        public string remarks { get; set; }
        public List<ServicesDetails> opdServices { get; set; }
    }
    public class cityListParams
    {
        public string stateid { get; set; }
    }
    public class StateResponse
    {
        public string status { get; set; }
        public string remarks { get; set; }
        public List<StateDetails> stateList { get; set; }
    }
    public class StateDetails
    {
        public string stateId { get; set; }
        public string statename { get; set; }
    }

    public class CityResponse
    {
        public string status { get; set; }
        public string remarks { get; set; }
        public List<CityDetails> cityList { get; set; }
    }
    public class CityDetails
    {
        public string cityId { get; set; }
        public string cityName { get; set; }
    }
}