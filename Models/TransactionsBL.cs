using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace OPD.Models
{
    public class TransactionsBL
    {
        DBHelper dBHelper = new DBHelper();
        string JsonRequest = "";
        string JsonResponse = "";

        public ResponsetransBL GetTransactionDetails(TransactionParam transactionParam)
        {
            ResponsetransBL response = new ResponsetransBL();
            List<ResponsetransBL> lstResponsetrans = new List<ResponsetransBL>();
            DataTable dtTransactionDetails = new DataTable();
            try
            {
                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(transactionParam);
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "GetTransactionData"));
                paramList.Add(new SqlParameter("@empId", transactionParam.emp_id));
                paramList.Add(new SqlParameter("@opd_type_id", transactionParam.opd_type_id));
                dtTransactionDetails = dBHelper.GetTableFromSP("sp_transactiondetails", paramList.ToArray());

                if (dtTransactionDetails.Rows.Count > 0)
                {
                    response.status = "Success";
                    response.remarks = "";
                    TransactionDetailsResposeParam transDetails = new TransactionDetailsResposeParam();
                    List<TransactionDetailsResposeParam> lstTransDetails = new List<TransactionDetailsResposeParam>();
                    foreach (DataRow dataRow in dtTransactionDetails.Rows)
                    {
                        transDetails.emp_trans_id = Convert.ToString(dataRow["emp_trans_id"]);
                        transDetails.emp_id = Convert.ToString(dataRow["emp_id"]);
                        transDetails.opd_type_mast_id = Convert.ToString(dataRow["opd_type_mast_id"]);
                        transDetails.lookup_Description = Convert.ToString(dataRow["lookup_Description"]);
                        transDetails.Invoice_amt = Convert.ToString(dataRow["Invoice_amt"]);
                        transDetails.approved_amt = Convert.ToString(dataRow["approved_amt"]);
                        transDetails.provider_name = Convert.ToString(dataRow["provider_name"]);
                        transDetails.product_name = Convert.ToString(dataRow["product_name"]);
                        transDetails.emp_trans_details_id = Convert.ToString(dataRow["emp_trans_details_id"]);
                        transDetails.invoice_date = Convert.ToString(dataRow["invoice_date"]);
                        transDetails.invoice_no = Convert.ToString(dataRow["invoice_no"]);
                        transDetails.sinvoice_amt = Convert.ToString(dataRow["sinvoice_amt"]);
                        transDetails.sapproved_amt = Convert.ToString(dataRow["sapproved_amt"]);
                        transDetails.invoice_path = Convert.ToString(dataRow["invoice_path"]);
                        transDetails.invoice_file_name = Convert.ToString(dataRow["invoice_file_name"]);
                        transDetails.file_ext_id = Convert.ToString(dataRow["file_ext_id"]);

                        lstTransDetails.Add(transDetails);
                        transDetails = new TransactionDetailsResposeParam();
                    }

                    response.transDetails = lstTransDetails;
                }
                else
                {
                    response.status = "Failed";
                    response.remarks = "No data found";
                }
            }
            catch (Exception ex)
            {
                //CommonUtilities.FnWriteErrorLog("getTransactionDetails", ex.StackTrace);
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "TransactionBL_GetTransactionDetails", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }

            return response;
        }

        public AddTransResponseBL InsertReimbursementData(ReimbursementInsertDataParam addDataParam)
        {
            AddTransResponseBL response = new AddTransResponseBL();
            DataTable dtGetResponse = new DataTable();
            try
            {
                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(addDataParam);
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "AddReimbursementData"));
                paramList.Add(new SqlParameter("@empId", addDataParam.emp_id));
                paramList.Add(new SqlParameter("@opd_type_id", addDataParam.opd_type_mast_id));
                paramList.Add(new SqlParameter("@total_invoice_amt", addDataParam.invoice_amt));
                paramList.Add(new SqlParameter("@invoice_no", addDataParam.invoice_no));
                paramList.Add(new SqlParameter("@invoice_date", addDataParam.invoice_date));
                paramList.Add(new SqlParameter("@provider_name", addDataParam.provider_name));
                paramList.Add(new SqlParameter("@product_name", addDataParam.product_name));
                paramList.Add(new SqlParameter("@product_description", addDataParam.product_description));
                paramList.Add(new SqlParameter("@individual_product_amt", addDataParam.individual_product_amt));
                paramList.Add(new SqlParameter("@invoice_file_path", addDataParam.invoice_file_path));
                paramList.Add(new SqlParameter("@invoice_file_name", addDataParam.invoice_file_name));
                paramList.Add(new SqlParameter("@file_extension", addDataParam.file_extension));
                paramList.Add(new SqlParameter("@inserted_by", addDataParam.inserted_by));
                dtGetResponse = dBHelper.GetTableFromSP("sp_transactiondetails", paramList.ToArray());
                if (dtGetResponse.Rows.Count > 0)
                {
                    response.status = Convert.ToString(dtGetResponse.Rows[0]["Result"]);
                    response.remarks = Convert.ToString(dtGetResponse.Rows[0]["remarks"]);
                }
            }
            catch (Exception ex)
            {
                //CommonUtilities.FnWriteErrorLog("InsertReimbursementData", ex.StackTrace);
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "TransactionBL_InsertReimbursementData", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, addDataParam.emp_id);
            }

            return response;
        }

        #region MyRegion
        //public AddTransResponseBL InsertReimbursementData_new(ReimbursementInsertDataParamBL addDataParam)
        //{
        //    AddTransResponseBL response = new AddTransResponseBL();
        //    DataTable dtGetResponse = new DataTable();
        //    try
        //    {
        //        List<SqlParameter> paramList = new List<SqlParameter>();
        //        paramList.Add(new SqlParameter("@type", "AddReimbursementData"));
        //        paramList.Add(new SqlParameter("@empId", addDataParam.emp_id));
        //        paramList.Add(new SqlParameter("@opd_type_id", addDataParam.opd_type_mast_id));
        //        paramList.Add(new SqlParameter("@total_invoice_amt", addDataParam.invoice_amt));
        //        paramList.Add(new SqlParameter("@inserted_by", addDataParam.inserted_by));
        //        paramList.Add(new SqlParameter("@invoice_file_path", addDataParam.upload_invoice.invoice_file_path));
        //        paramList.Add(new SqlParameter("@invoice_file_name", addDataParam.upload_invoice.invoice_file_name));
        //        paramList.Add(new SqlParameter("@file_extension", addDataParam.upload_invoice.file_extension));

        //        dtGetResponse = dBHelper.GetTableFromSP("sp_transactiondetails", paramList.ToArray());

        //        if (dtGetResponse.Rows.Count > 0 && Convert.ToString(dtGetResponse.Rows[0]["Result"]) == "Success" && !string.IsNullOrEmpty(Convert.ToString(dtGetResponse.Rows[0]["trans_id"])))
        //        {
        //            foreach (var providerdata in addDataParam.provider_details)
        //            {                        
        //                foreach (var productdetails in providerdata.products)
        //                {
        //                    paramList = new List<SqlParameter>();
        //                    paramList.Add(new SqlParameter("@type", "AddProducts"));
        //                    paramList.Add(new SqlParameter("@transaction_id", Convert.ToString(dtGetResponse.Rows[0]["trans_id"])));
        //                    paramList.Add(new SqlParameter("@invoice_no", providerdata.invoice_no));
        //                    paramList.Add(new SqlParameter("@invoice_date", providerdata.invoice_date));
        //                    paramList.Add(new SqlParameter("@provider_name", providerdata.provider_name));

        //                    paramList.Add(new SqlParameter("@product_name", productdetails.product_name));
        //                    paramList.Add(new SqlParameter("@product_description", productdetails.product_description));
        //                    paramList.Add(new SqlParameter("@individual_product_amt", productdetails.individual_product_amt));
        //                    paramList.Add(new SqlParameter("@inserted_by", addDataParam.emp_id));
        //                    dtGetResponse = dBHelper.GetTableFromSP("sp_transactiondetails", paramList.ToArray());
        //                }
        //            }

        //            if (dtGetResponse.Rows.Count > 0)
        //            {
        //                response.status = Convert.ToString(dtGetResponse.Rows[0]["Result"]);
        //                response.remarks = Convert.ToString(dtGetResponse.Rows[0]["remarks"]);
        //            }
        //        }
        //        else
        //        {
        //            response.status = Convert.ToString(dtGetResponse.Rows[0]["Result"]);
        //            response.remarks = Convert.ToString(dtGetResponse.Rows[0]["remarks"]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonUtilities.FnWriteErrorLog("InsertReimbursementData", ex.StackTrace);
        //    }

        //    return response;
        //}
        #endregion

        public AddTransResponseBL InsertReimbursementData_new(ReimbursementInsertDataParamBL addDataParam)
        {
            AddTransResponseBL response = new AddTransResponseBL();
            DataTable dtGetResponse = new DataTable();
            try
            {
                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(addDataParam);
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "AddReimbursementData"));
                paramList.Add(new SqlParameter("@empId", addDataParam.emp_id));
                paramList.Add(new SqlParameter("@opd_type_id", addDataParam.opd_type_mast_id));
                paramList.Add(new SqlParameter("@total_invoice_amt", addDataParam.invoice_amt));
                paramList.Add(new SqlParameter("@inserted_by", addDataParam.inserted_by));

                dtGetResponse = dBHelper.GetTableFromSP("sp_transactiondetails", paramList.ToArray());

                if (dtGetResponse.Rows.Count > 0 && Convert.ToString(dtGetResponse.Rows[0]["Result"]) == "Success" && !string.IsNullOrEmpty(Convert.ToString(dtGetResponse.Rows[0]["trans_id"])))
                {
                    try
                    {
                        foreach (var providerdata in addDataParam.products)
                        {

                            DateTime dt = DateTime.ParseExact(providerdata.invoice_date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                            //string s = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                            //DateTime dt = Convert.ToDateTime(providerdata.invoice_date);

                            string invoiceDate = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                            paramList = new List<SqlParameter>();
                            paramList.Add(new SqlParameter("@type", "AddProducts"));
                            paramList.Add(new SqlParameter("@transaction_id", Convert.ToString(dtGetResponse.Rows[0]["trans_id"])));
                            paramList.Add(new SqlParameter("@invoice_no", providerdata.invoice_no));
                            paramList.Add(new SqlParameter("@invoice_date", invoiceDate));
                            paramList.Add(new SqlParameter("@provider_name", addDataParam.providername));
                            paramList.Add(new SqlParameter("@product_name", providerdata.productname));
                            paramList.Add(new SqlParameter("@product_description", providerdata.productdescription));
                            paramList.Add(new SqlParameter("@individual_product_amt", providerdata.productamt));
                            paramList.Add(new SqlParameter("@inserted_by", addDataParam.emp_id));
                            dtGetResponse = dBHelper.GetTableFromSP("sp_transactiondetails", paramList.ToArray());
                        }

                        if (dtGetResponse.Rows.Count > 0)
                        {
                            response.status = Convert.ToString(dtGetResponse.Rows[0]["Result"]);
                            response.remarks = Convert.ToString(dtGetResponse.Rows[0]["remarks"]);
                            response.transactionid = Convert.ToString(dtGetResponse.Rows[0]["trans_id"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        //CommonUtilities.FnWriteErrorLog("inner if", ex.StackTrace);
                        response.status = "Fail";
                        response.remarks = "Something went wrong";

                        JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        CommonUtilities.fnStoreErrorLog("API", "TransactionBL_InsertReimbursementData_new", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, addDataParam.emp_id);
                    }
                }
                else
                {
                    response.status = Convert.ToString(dtGetResponse.Rows[0]["Result"]);
                    response.remarks = Convert.ToString(dtGetResponse.Rows[0]["remarks"]);
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "TransactionBL_InsertReimbursementData_new", "Request=" + JsonRequest + "Response=" + JsonResponse, addDataParam.emp_id);
            }
            catch (Exception ex)
            {
                // CommonUtilities.FnWriteErrorLog("InsertReimbursementData", ex.StackTrace);
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "TransactionBL_InsertReimbursementData_new", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace + "Message=" + ex.Message, addDataParam.emp_id);
            }

            return response;
        }

        public AddTransResponseBL UploadInvoice(uploadInvoice param)
        {
            AddTransResponseBL response = new AddTransResponseBL();
            DataTable dtGetResponse = new DataTable();


            try
            {
                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "UploadInvoice"));
                paramList.Add(new SqlParameter("@transaction_id", param.trans_id));
                paramList.Add(new SqlParameter("@invoice_file_path", param.filePath));
                paramList.Add(new SqlParameter("@invoice_file_name", param.invoice_name));
                paramList.Add(new SqlParameter("@file_extension", param.file_extension));
                paramList.Add(new SqlParameter("@inserted_by", param.empid));
                dtGetResponse = dBHelper.GetTableFromSP("sp_transactiondetails", paramList.ToArray());
                if (dtGetResponse.Rows.Count > 0)
                {
                    response.status = Convert.ToString(dtGetResponse.Rows[0]["Result"]);
                    response.remarks = Convert.ToString(dtGetResponse.Rows[0]["remarks"]);
                }
                else
                {
                    response.status = "Fail";
                    response.remarks = "No data found.";
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "TransactionBL_UploadInvoice", "Request=" + JsonRequest + "Response=" + JsonResponse, param.empid);
            }
            catch (Exception ex)
            {
                response.status = "Fail";
                response.remarks = "Something went wrong.";
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "TransactionBL_UploadInvoice", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.empid);
            }

            return response;
        }


        public AddTransResponseBL InsertCashlessTransactiondata(CashlessDataParam param)
        {
            AddTransResponseBL response = new AddTransResponseBL();
            DataTable dtGetResponse = new DataTable();
            try
            {
                JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "AddReimbursementData"));
                paramList.Add(new SqlParameter("@empId", param.emp_id));
                paramList.Add(new SqlParameter("@opd_type_id", param.opd_type_mast_id));
                paramList.Add(new SqlParameter("@total_invoice_amt", param.invoice_amt));

                dtGetResponse = dBHelper.GetTableFromSP("sp_transactiondetails", paramList.ToArray());

                if (dtGetResponse.Rows.Count > 0)
                {
                    response.status = Convert.ToString(dtGetResponse.Rows[0]["Result"]);
                    response.remarks = Convert.ToString(dtGetResponse.Rows[0]["remarks"]);
                    response.transactionid = Convert.ToString(dtGetResponse.Rows[0]["trans_id"]);
                }

                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "TransactionBL_InsertCashlessTransactionData", "Request=" + JsonRequest + "Response=" + JsonResponse, param.emp_id);
            }
            catch (Exception ex)
            {
                //CommonUtilities.FnWriteErrorLog("InsertCashlessTransactiondata", ex.StackTrace);
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "TransactionBL_InsertCashlessTransactionData", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, param.emp_id);
            }

            return response;
        }
    }

    public class CashlessDataParam
    {
        public string emp_id { get; set; }
        public string opd_type_mast_id { get; set; }
        public string invoice_amt { get; set; }
        public string invoice_file_path { get; set; }
        public string invoice_file_name { get; set; }
        public string file_extension { get; set; }
        public string inserted_by { get; set; }
    }

    public class uploadInvoice
    {
        public string empid { get; set; }
        public string trans_id { get; set; }
        public string invoice_name { get; set; }
        public string filePath { get; set; }
        public string file_extension { get; set; }

    }

    public class TransactionParam
    {
        public string emp_id { get; set; }
        public string opd_type_id { get; set; }
    }

    public class TransactionDetailsResposeParam
    {
        public string emp_trans_id { get; set; }
        public string emp_id { get; set; }
        public string opd_type_mast_id { get; set; }
        public string lookup_Description { get; set; }
        public string Invoice_amt { get; set; }
        public string approved_amt { get; set; }
        public string provider_name { get; set; }
        public string product_name { get; set; }
        public string emp_trans_details_id { get; set; }
        public string invoice_date { get; set; }
        public string invoice_no { get; set; }
        public string sinvoice_amt { get; set; }
        public string sapproved_amt { get; set; }
        public string invoice_path { get; set; }
        public string invoice_file_name { get; set; }
        public string file_ext_id { get; set; }


    }

    public class ResponsetransBL
    {
        public string status { get; set; }
        public string remarks { get; set; }
        public List<TransactionDetailsResposeParam> transDetails { get; set; }
    }

    public class AddTransResponseBL
    {
        public string status { get; set; }
        public string remarks { get; set; }
        public string transactionid { get; set; }
    }

    public class ReimbursementInsertDataParam
    {
        public string emp_id { get; set; }
        public string opd_type_mast_id { get; set; }
        public string invoice_amt { get; set; }
        public string invoice_no { get; set; }
        public string invoice_date { get; set; }
        public string provider_name { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public string individual_product_amt { get; set; }
        public string invoice_file_path { get; set; }
        public string invoice_file_name { get; set; }
        public string file_extension { get; set; }
        public string inserted_by { get; set; }
    }

    //public class ReimbursementInsertDataParamBL
    //{
    //    public string emp_id { get; set; }
    //    public string opd_type_mast_id { get; set; }
    //    public string invoice_amt { get; set; }
    //    public string inserted_by { get; set; }
    //    public List<ReimbursementProviderDetails> provider_details { get; set; }
    //    //public List<InvoiceUpload> upload_invoice { get; set; }
    //    public InvoiceUpload upload_invoice { get; set; }
    //}

    public class ReimbursementInsertDataParamBL
    {
        public string emp_id { get; set; }
        public string opd_type_mast_id { get; set; }
        public string invoice_amt { get; set; }
        public string providername { get; set; }
        public string inserted_by { get; set; }
        public List<ReimbursementProductDetails> products { get; set; }

        // public List<InvoiceUpload> upload_invoice { get; set; }
        public InvoiceUpload upload_invoice { get; set; }
    }

    public class ReimbursementProviderDetails
    {
        public string invoice_no { get; set; }
        public string invoice_date { get; set; }
        public string provider_name { get; set; }
        public List<ProductList> products { get; set; }
    }


    public class ReimbursementProductDetails
    {
        public string invoice_no { get; set; }
        public string invoice_date { get; set; }
        public string providername { get; set; }
        public string productname { get; set; }
        public string productdescription { get; set; }
        public string productamt { get; set; }
    }

    public class ProductList
    {
        public string product_name { get; set; }
        public string product_description { get; set; }
        public string individual_product_amt { get; set; }
    }

    public class InvoiceUpload
    {
        public string invoice_file_path { get; set; }
        public string invoice_file_name { get; set; }
        public string file_extension { get; set; }
    }
}