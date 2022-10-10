using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OPD.Models
{
    public class EmployeeBL
    {
        public ResponseBL GetEmployeeDetails(EmployeeParam employeeParam)
        {
            ResponseBL response = new ResponseBL();
            List<ResponseBL> lstResponse = new List<ResponseBL>();
            DataTable dtEmployeeDetails = new DataTable();
            string Request = "";
            string Response = "";
            try
            {
                #region MyRegion
                //using (SqlConnection con = new SqlConnection(getConnection.strConnection))
                //{
                //    using (SqlCommand cmd = new SqlCommand("sp_employeedetails", con))
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@type", "GetEmployeeData");
                //        cmd.Parameters.AddWithValue("@empid", employeeParam.emp_id);
                //        SqlDataAdapter sqlData = new SqlDataAdapter();
                //        con.Open();
                //        sqlData.SelectCommand = cmd;
                //        sqlData.Fill(dtEmployeeDetails);
                //        con.Close();

                //        if (dtEmployeeDetails.Rows.Count > 0)
                //        {
                //            response.status = "Success";
                //            response.remarks = "";
                //            EmployeeDetailsResposeParam empDetails = new EmployeeDetailsResposeParam();
                //            List<EmployeeDetailsResposeParam> lstEmpDetails = new List<EmployeeDetailsResposeParam>();
                //            foreach (DataRow dataRow in dtEmployeeDetails.Rows)
                //            {
                //                lstResponse = new List<ResponseBL>();
                //                empDetails.f_name = Convert.ToString(dataRow["F_name"]);
                //                empDetails.l_name = Convert.ToString(dataRow["l_name"]);
                //                empDetails.mobile = Convert.ToString(dataRow["mobile_no"]);
                //                empDetails.email_id = Convert.ToString(dataRow["email_id"]);
                //                empDetails.m_name = Convert.ToString(dataRow["m_name"]);
                //                empDetails.emp_id = Convert.ToString(dataRow["emp_id"]);
                //                empDetails.client_type = Convert.ToString(dataRow["CLIENT_TYPE"]);
                //                empDetails.client_name = Convert.ToString(dataRow["client_name"]);
                //                response.empDetails = empDetails;
                //                lstResponse.Add(response);
                //            }                                                       
                //        }
                //        else
                //        {
                //            response.status = "Failed";
                //            response.remarks = "No data found";
                //        }

                //        //response.empDetails = lstResponse;

                //    }
                //}
                #endregion

                Request = Newtonsoft.Json.JsonConvert.SerializeObject(employeeParam);

                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetEmployeeData"));
                paramList.Add(new SqlParameter("@empid", employeeParam.emp_id));

                DBHelper dBHelper = new DBHelper();
                dtEmployeeDetails = dBHelper.GetTableFromSP("sp_employeedetails", paramList.ToArray());
                if (dtEmployeeDetails.Rows.Count > 0)
                {
                    response.status = "Success";
                    response.remarks = "";
                    EmployeeDetailsResposeParam empDetails = new EmployeeDetailsResposeParam();
                    List<EmployeeDetailsResposeParam> lstEmpDetails = new List<EmployeeDetailsResposeParam>();
                    foreach (DataRow dataRow in dtEmployeeDetails.Rows)
                    {
                        lstResponse = new List<ResponseBL>();
                        empDetails.f_name = Convert.ToString(dataRow["F_name"]);
                        empDetails.l_name = Convert.ToString(dataRow["l_name"]);
                        empDetails.mobile = Convert.ToString(dataRow["mobile_no"]);
                        empDetails.email_id = Convert.ToString(dataRow["email_id"]);
                        empDetails.m_name = Convert.ToString(dataRow["m_name"]);
                        empDetails.emp_id = Convert.ToString(dataRow["emp_id"]);
                        empDetails.client_type = Convert.ToString(dataRow["CLIENT_TYPE"]);
                        empDetails.client_name = Convert.ToString(dataRow["client_name"]);
                        empDetails.emp_grade = Convert.ToString(dataRow["emp_grade"]);
                        empDetails.wallet_start_dt = Convert.ToString(dataRow["wallet_start_dt"]);
                        empDetails.wallet_end_dt = Convert.ToString(dataRow["wallet_end_dt"]);
                        empDetails.otp_time_expiry = Convert.ToString(dataRow["otp_time_expiry"]);
                        empDetails.assigned_amount = Convert.ToString(dataRow["assigned_amount"]);
                        empDetails.balance_amt = Convert.ToString(dataRow["balance_amt"]);
                        response.empDetails = empDetails;
                        lstResponse.Add(response);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "No data found";
                }
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong";
                CommonUtilities.fnStoreErrorLog("API", "EmployeeBL_GetEmployeeDetails", "Request=" + Request + "Response=" + Response + "Exception=" + ex.StackTrace, "");
            }

            return response;
        }

        public WalletStatusResponse getWalletStatus(WalletStatusParam param)
        {
            WalletStatusResponse response = new WalletStatusResponse();
            string Request = "";
            string Response = "";
            try
            {
                Request = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetWalletStatus"));
                paramList.Add(new SqlParameter("@empid", param.emp_id));
                DataTable dtEmpWalletStatus = new DataTable();

                DBHelper dBHelper = new DBHelper();
                dtEmpWalletStatus = dBHelper.GetTableFromSP("sp_employeedetails", paramList.ToArray());
                if (dtEmpWalletStatus.Rows.Count > 0)
                {
                    response.status = Convert.ToString(dtEmpWalletStatus.Rows[0]["Result"]);
                    response.remarks = Convert.ToString(dtEmpWalletStatus.Rows[0]["Remarks"]);
                }
                else
                {
                    response.status = "Fail";
                    response.remarks = "No data found.";
                }

                Response = Newtonsoft.Json.JsonConvert.SerializeObject(param);
            }
            catch (Exception ex)
            {
                response.status = "Fail";
                response.remarks = "Something went wrong";
                Response = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                CommonUtilities.fnStoreErrorLog("API", "EmployeeBL_getWalletStatus", "Request=" + Request + "Response=" + Response, param.emp_id);
            }

            return response;
        }

        public ResponseBL VerifyEmployee(VerifyEmployee param)
        {
            ResponseBL response = new ResponseBL();
            List<ResponseBL> lstResponse = new List<ResponseBL>();
            DataTable dtEmployeeDetails = new DataTable();
            string Request = "";
            string Response = "";
            try
            {
                Request = Newtonsoft.Json.JsonConvert.SerializeObject(param);

                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "VerifyMobileNo"));
                paramList.Add(new SqlParameter("@mobileNo", param.mobileNo));

                DBHelper dBHelper = new DBHelper();
                dtEmployeeDetails = dBHelper.GetTableFromSP("sp_employeedetails", paramList.ToArray());
                if (dtEmployeeDetails.Rows.Count > 0)
                {
                    if (Convert.ToString(dtEmployeeDetails.Rows[0]["Result"]) == "Success")
                    {
                        response.status = "Success";
                        response.remarks = "";
                        EmployeeDetailsResposeParam empDetails = new EmployeeDetailsResposeParam();
                        List<EmployeeDetailsResposeParam> lstEmpDetails = new List<EmployeeDetailsResposeParam>();
                        foreach (DataRow dataRow in dtEmployeeDetails.Rows)
                        {
                            lstResponse = new List<ResponseBL>();
                            empDetails.f_name = Convert.ToString(dataRow["F_name"]);
                            empDetails.l_name = Convert.ToString(dataRow["l_name"]);
                            empDetails.mobile = Convert.ToString(dataRow["mobile_no"]);
                            empDetails.email_id = Convert.ToString(dataRow["email_id"]);
                            empDetails.m_name = Convert.ToString(dataRow["m_name"]);
                            empDetails.emp_id = Convert.ToString(dataRow["emp_id"]);
                            empDetails.client_type = Convert.ToString(dataRow["CLIENT_TYPE"]);
                            empDetails.client_name = Convert.ToString(dataRow["client_name"]);
                            empDetails.emp_grade = Convert.ToString(dataRow["emp_grade"]);
                            empDetails.wallet_start_dt = Convert.ToString(dataRow["wallet_start_dt"]);
                            empDetails.wallet_end_dt = Convert.ToString(dataRow["wallet_end_dt"]);
                            empDetails.otp_time_expiry = Convert.ToString(dataRow["otp_time_expiry"]);
                            empDetails.assigned_amount = Convert.ToString(dataRow["assigned_amount"]);
                            empDetails.balance_amt = Convert.ToString(dataRow["balance_amt"]);
                            response.empDetails = empDetails;
                            lstResponse.Add(response);
                        }
                    }
                    else
                    {
                        response.status = Convert.ToString(dtEmployeeDetails.Rows[0]["Result"]);
                        response.remarks = Convert.ToString(dtEmployeeDetails.Rows[0]["Remarks"]);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "No data found";
                }
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong";
                CommonUtilities.fnStoreErrorLog("API", "EmployeeBL_GetEmployeeDetails", "Request=" + Request + "Response=" + Response + "Exception=" + ex.StackTrace, "");
            }

            return response;
        }

        public VerifyEmployeeResponse ValidateMobileNumber(VerifyEmployee param)
        {
            VerifyEmployeeResponse response = new VerifyEmployeeResponse();
            List<VerifyEmployeeResponse> lstResponse = new List<VerifyEmployeeResponse>();
            DataTable dtEmployeeDetails = new DataTable();
            string Request = "";
            string Response = "";
            try
            {
                Request = Newtonsoft.Json.JsonConvert.SerializeObject(param);

                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "ValidateMobileNo"));
                paramList.Add(new SqlParameter("@mobileNo", param.mobileNo));

                DBHelper dBHelper = new DBHelper();
                dtEmployeeDetails = dBHelper.GetTableFromSP("SP_ValidateMobile", paramList.ToArray());
                dtEmployeeDetails = dBHelper.GetTableFromSP("SP_ValidateMobile", paramList.ToArray());
                if (dtEmployeeDetails.Rows.Count > 0)
                {
                    if (Convert.ToString(dtEmployeeDetails.Rows[0]["Result"]) == "Success")
                    {
                        foreach (DataRow dataRow in dtEmployeeDetails.Rows)
                        {
                            lstResponse = new List<VerifyEmployeeResponse>();
                            lstResponse = new List<VerifyEmployeeResponse>();
                            response.empid = Convert.ToString(dataRow["emp_id"]);
                            response.mobileNo = Convert.ToString(dataRow["mobile_no"]);
                            response.status = Convert.ToString(dataRow["Result"]);
                            lstResponse.Add(response);
                        }
                    }
                    else
                    {
                        response.status = Convert.ToString(dtEmployeeDetails.Rows[0]["Result"]);
                        response.remarks = Convert.ToString(dtEmployeeDetails.Rows[0]["Remarks"]);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "No data found";
                }
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong";
                CommonUtilities.fnStoreErrorLog("API", "EmployeeBL_GetEmployeeDetails", "Request=" + Request + "Response=" + Response + "Exception=" + ex.StackTrace, "");
            }

            return response;
        }

        public CommunicationType getSMSScript(getSMS param)
        {
            CommunicationType response = new CommunicationType();
            List<CommunicationType> lstResponse = new List<CommunicationType>();
            DataTable dtEmployeeDetails = new DataTable();
            string Request = "";
            string Response = "";
            try
            {
                Request = Newtonsoft.Json.JsonConvert.SerializeObject(param);

                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", param.type));
                paramList.Add(new SqlParameter("@mode", param.mode));

                DBHelper dBHelper = new DBHelper();
                dtEmployeeDetails = dBHelper.GetTableFromSP("SP_ValidateMobile", paramList.ToArray());
                if (dtEmployeeDetails.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dtEmployeeDetails.Rows)
                    {
                        lstResponse = new List<CommunicationType>();
                        response.status = "Success";
                        response.comm_type_id = Convert.ToString(dataRow["comm_type_id"]);
                        response.COMM_TYPE = Convert.ToString(dataRow["COMM_TYPE"]);
                        response.COMM_MODE = Convert.ToString(dataRow["COMM_MODE"]);
                        response.COMM_DESCRIPTION = Convert.ToString(dataRow["COMM_DESCRIPTION"]);
                        lstResponse.Add(response);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "No data found";
                }
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong";
                CommonUtilities.fnStoreErrorLog("API", "EmployeeBL_getSMSScript", "Request=" + Request + "Response=" + Response + "Exception=" + ex.StackTrace, "");
            }

            return response;
        }

        public CommunicationType saveSMSLog(smsLogParam param)
        {
            CommunicationType response = new CommunicationType();
            DataTable dtEmployeeDetails = new DataTable();
            string Request = "";
            string Response = "";
            try
            {
                Request = Newtonsoft.Json.JsonConvert.SerializeObject(param);

                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", param.type));
                paramList.Add(new SqlParameter("@comm_type_id", param.comm_type_id));
                paramList.Add(new SqlParameter("@empid", param.emp_id));
                paramList.Add(new SqlParameter("@mobileno", param.mobile_no));
                paramList.Add(new SqlParameter("@message", param.message));
                paramList.Add(new SqlParameter("@response", param.response));
                paramList.Add(new SqlParameter("@responseid", param.response_id));
                paramList.Add(new SqlParameter("@status", param.status));
                paramList.Add(new SqlParameter("@OTP", param.OTP));

                DBHelper dBHelper = new DBHelper();
                dtEmployeeDetails = dBHelper.GetTableFromSP("SP_ValidateMobile", paramList.ToArray());
                if (dtEmployeeDetails.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dtEmployeeDetails.Rows)
                    {
                        response.status = Convert.ToString(dataRow["Result"]);
                        response.remarks = Convert.ToString(dataRow["Remarks"]);
                    }
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "No data found";
                }
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong";
                CommonUtilities.fnStoreErrorLog("API", "EmployeeBL_getSMSLog", "Request=" + Request + "Response=" + Response + "Exception=" + ex.StackTrace, "");
            }

            return response;
        }

        public OtpresposeBL VerifyOTP(verifyOTPParams param)
        {
            OtpresposeBL response = new OtpresposeBL();
            DataTable dtVerifyOTPDetails = new DataTable();
            string Request = "";
            string Response = "";
            try
            {
                Request = Newtonsoft.Json.JsonConvert.SerializeObject(param);

                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "VerifyOTP"));
                paramList.Add(new SqlParameter("@mobileno", param.mobileNo));
                paramList.Add(new SqlParameter("@OTP", param.otp));

                DBHelper dBHelper = new DBHelper();
                dtVerifyOTPDetails = dBHelper.GetTableFromSP("SP_ValidateMobile", paramList.ToArray());
                if (dtVerifyOTPDetails.Rows.Count > 0)
                {
                    if (Convert.ToString(dtVerifyOTPDetails.Rows[0]["Result"]) == "Success")
                    {
                        response.status = Convert.ToString(dtVerifyOTPDetails.Rows[0]["Result"]);
                        response.mobileNo = param.mobileNo;
                    }
                    else
                    {
                        response.status = Convert.ToString(dtVerifyOTPDetails.Rows[0]["Result"]);
                        response.remarks = Convert.ToString(dtVerifyOTPDetails.Rows[0]["Remarks"]);                        
                    }

                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "No data found";
                }
            }
            catch (Exception ex)
            {
                response.status = "Failed";
                response.remarks = "Something went wrong";
                CommonUtilities.fnStoreErrorLog("API", "EmployeeBL_VerifyOTP", "Request=" + Request + "Response=" + Response + "Exception=" + ex.StackTrace, "");
            }

            return response;
        }
    }

    public class EmployeeParam
    {
        public string emp_id { get; set; }
    }
    public class EmployeeDetailsResposeParam
    {
        public string emp_id { get; set; }
        public string f_name { get; set; }
        public string m_name { get; set; }
        public string l_name { get; set; }
        public string mobile { get; set; }
        public string email_id { get; set; }
        public string client_type { get; set; }
        public string emp_grade { get; set; }
        public string client_name { get; set; }
        public string balance_amt { get; set; }
        public string assigned_amount { get; set; }
        public string otp_time_expiry { get; set; }
        public string wallet_start_dt { get; set; }
        public string wallet_end_dt { get; set; }

    }
    public class ResponseBL
    {
        public string status { get; set; }
        public string remarks { get; set; }
        public EmployeeDetailsResposeParam empDetails { get; set; }
    }
    public class WalletStatusParam
    {
        public string emp_id { get; set; }
        public string opd_type_id { get; set; }
    }
    public class WalletStatusResponse
    {
        public string status { get; set; }
        public string remarks { get; set; }
    }
    public class VerifyEmployee
    {
        public string mobileNo { get; set; }
    }
    public class VerifyEmployeeResponse
    {
        public string status { get; set; }
        public string remarks { get; set; }
        public string empid { get; set; }
        public string mobileNo { get; set; }
    }
    public class getSMS
    {
        public string type { get; set; }
        public string mode { get; set; }
    }
    public class CommunicationType
    {
        public string status { get; set; }
        public string remarks { get; set; }
        public string comm_type_id { get; set; }
        public string COMM_TYPE { get; set; }
        public string COMM_MODE { get; set; }
        public string COMM_DESCRIPTION { get; set; }
    }
    public class smsLogParam
    {
        public string type { get; set; }
        public string comm_type_id { get; set; }
        public string OTP { get; set; }
        public string emp_id { get; set; }
        public string mobile_no { get; set; }
        public string message { get; set; }
        public string response { get; set; }
        public string response_id { get; set; }
        public string status { get; set; }
    }
    public class verifyOTPParams
    {
        public string otp { get; set; }
        public string mobileNo { get; set; }
    }
    public class OtpresposeBL
    {
        public string status { get; set; }
        public string remarks { get; set; }
        public string mobileNo { get; set; }
    }
}