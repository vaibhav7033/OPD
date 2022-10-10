using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace OPD.Models
{
    public class BankmasterBL
    {
        //DBHelper dBHelper = new DBHelper();
        public BankBL GetBankDetails(Bankmaster bankmaster)
        {
            BankBL response = new BankBL();
            List<BankBL> lstResponse = new List<BankBL>();
            DataTable dtbankdetails = new DataTable();
            string Request = "";
            string Response = "";
                
            try
            {
                Request = Newtonsoft.Json.JsonConvert.SerializeObject(bankmaster);

                using (SqlConnection con = new SqlConnection(getConnection.strConnection))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_bankdetails", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@type", "Getbankdetails");
                        cmd.Parameters.AddWithValue("@bank_master_id", bankmaster.BankId);
                        SqlDataAdapter sqlData = new SqlDataAdapter();
                        con.Open();
                        sqlData.SelectCommand = cmd;
                        sqlData.Fill(dtbankdetails);
                        con.Close();

                        /* foreach (DataRow dr in dtbankdetails.Rows)
                         {
                             lstResponse.Add(new  Bankmaster
                             {
                                 BankId = Convert.ToString(dr["bank_master_id"]),
                                 Bankname = Convert.ToString(dr["bank_name"]),
                                 Bankcode = Convert.ToString(dr["bank_code"]),
                             });
                         }*/



                        if (dtbankdetails.Rows.Count > 0)
                        {
                            response.bankstatus = "Success";
                            response.bankremarks = "";
                            Bankmaster bankDetails = new Bankmaster();
                            List<Bankmaster> lstBankDetails = new List<Bankmaster>();
                            foreach (DataRow dataRow in dtbankdetails.Rows)
                            {

                                bankDetails.BankId = Convert.ToString(dataRow["bank_master_id"]);
                                bankDetails.Bankname = Convert.ToString(dataRow["bank_name"]);
                                bankDetails.Bankcode = Convert.ToString(dataRow["bank_code"]);
                               response.bankDetails = bankDetails;
                                lstResponse.Add(response);
                            }
                        }
                        else
                        {
                            response.bankstatus = "Failed";
                            response.bankremarks = "No data found";
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                response.bankstatus = "Failed";
                response.bankremarks = "Something went wrong";
                CommonUtilities.fnStoreErrorLog("API", "BankmasterBL_GetBankDetails", "Request=" + Request + "Response=" + Response + "Exception=" + ex.StackTrace, "");
            }

            return response;
        }
        public InsertBank InsertBankDetails(Bankmaster addbankdata)
        {
            InsertBank response = new InsertBank();
            List<InsertBank> lstResponse = new List<InsertBank>();
            DataTable bankdetails = new DataTable();
            string Request = "";
            string Response = "";
            int id;
            try
            {
                Request = Newtonsoft.Json.JsonConvert.SerializeObject(addbankdata);

                using (SqlConnection con = new SqlConnection(getConnection.strConnection))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_bankdetails", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@type", "AddBankDetais");
                        cmd.Parameters.AddWithValue("@bank_name", addbankdata.Bankname);
                        cmd.Parameters.AddWithValue("@bank_code", addbankdata.Bankcode);
                        cmd.Parameters.Add("@lastBtid", SqlDbType.VarChar, 100);
                        cmd.Parameters["@lastBtid"].Direction = ParameterDirection.Output;
                        SqlDataAdapter sqlData = new SqlDataAdapter();
                        con.Open();
                        id = Convert.ToInt32(cmd.Parameters["@lastBtid"].Value);
                        sqlData.SelectCommand = cmd;
                        sqlData.Fill(bankdetails);
                        con.Close();

                        


                       /* if (bankdetails.Rows.Count > 0)
                        {
                            response.bankstatus = "Success";
                            response.bankremarks = "";
                            Bankmaster bankDetails = new Bankmaster();
                            List<Bankmaster> lstBankDetails = new List<Bankmaster>();
                            foreach (DataRow dataRow in bankdetails.Rows)
                            {

                                bankDetails.BankId = Convert.ToString(dataRow["bank_master_id"]);
                                bankDetails.Bankname = Convert.ToString(dataRow["bank_name"]);
                                bankDetails.Bankcode = Convert.ToString(dataRow["bank_code"]);
                                response.bankDetails = bankDetails;
                                lstResponse.Add(response);
                            }
                        }
                        else
                        {
                            response.bankstatus = "Failed";
                            response.bankremarks = "No data found";
                        }*/

                    }
                }
            
            }
            catch (Exception ex)
            {
                //CommonUtilities.FnWriteErrorLog("InsertReimbursementData", ex.StackTrace);
                Response = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "BankmasterBL_GBankDetails", "Request=" + Request + "Response=" + Response + "Exception=" + ex.StackTrace, "");
            }

            return response;
        }

    }
    public class Bankmaster
    {
        public string type { get; set; }

        public string BankId { get; set; }

         
         public string Bankname { get; set; }

         public string Bankcode { get; set; }



    }
    public class BankBL
    {
        public string bankstatus { get; set; }
        public string bankremarks { get; set; }
       public Bankmaster bankDetails { get; set; }
    }
    public class InsertBank
    {
        public string bankstatus { get; set; }
        public string bankremarks { get; set; }
    }

}
