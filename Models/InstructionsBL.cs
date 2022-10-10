using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OPD.Models
{
    public class InstructionsBL
    {
        //private string connStr = Convert.ToString(ConfigurationManager.ConnectionStrings["Sqlconnection"]);
        //List<LookUpResponse> resList = new List<LookUpResponse>();
        string JsonRequest = "";
        string JsonResponse = "";

        public LookUpResBL getInstruction(lookupParam prop)
        {
            LookUpResBL response = new LookUpResBL();            
            try
            {
                DataTable dtInstruction = new DataTable();
               
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "INSTRUCT"));
                paramList.Add(new SqlParameter("@lookuptxt", prop.lookuptext));
                DBHelper dBHelper = new DBHelper();
                dtInstruction = dBHelper.GetTableFromSP("sp_opdinstructions", paramList.ToArray());

                if (dtInstruction.Rows.Count > 0)
                { 
                    response.Status = "Success";
                    response.Remarks = "";
                    InstructionParams instruction = new InstructionParams();
                    List<InstructionParams> lstInstruction = new List<InstructionParams>();
                    foreach (DataRow dr in dtInstruction.Rows)
                    {
                        
                        instruction.lookupid = Convert.ToInt32(dr["lookup_id"]);
                        instruction.lookuptext = dr["lookup_text"].ToString();
                        instruction.lookupDescription = dr["lookup_Description"].ToString();
                        //response.details = resList;
                        lstInstruction.Add(instruction);
                        instruction = new InstructionParams();
                    }

                    response.instruction = lstInstruction;
                }
                else
                {
                    response.Status = "Failed";
                    response.Remarks = "NO data Found";
                }                
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.Remarks = "Something went Wrong";
                CommonUtilities.FnWriteErrorLog("getInstruction", ex.StackTrace);
                JsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                CommonUtilities.fnStoreErrorLog("API", "InstructionBL_getInstruction", "Request=" + JsonRequest + "Response=" + JsonResponse + " Exception=" + ex.StackTrace, "");
            }

            return response;
        }
    }
    public class lookupParam
    {
        public int lookupid;
        public string lookuptext;
    }
    public class InstructionParams
    {
        public int lookupid { get; set; }
        public string lookuptext { get; set; }
        public string lookupDescription { get; set; }

    }
    public class LookUpResBL
    {
        public string Status { get; set; }
        public string Remarks { get; set; }        
        public List<InstructionParams> instruction { get; set; }
    }
}