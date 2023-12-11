using System.Data;
using System.Data.SqlClient;
namespace Create_Greenlight_835
{
    public static class DataOperations
    {
        public static void GetRemitData(string cn, string cn_PROD45, string TransactionNumber, ref Header835 header835, ref List<ClaimHeader> headers, ref List<ClaimLine> lines)
        {
            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @$"select CKPY_REF_ID,CKPY_TYPE,convert(varchar(8),CKPY_PAY_DT,112) as CKPY_PAY_DT,LOBD_ID,CKPY_PAYEE_TYPE,CKPY_ORIG_AMT,CKPY_DEDUCT_AMT,CKPY_NET_AMT,
b.PRPR_NAME,b.PRPR_NPI,b.MCTN_ID,CKPY_PAYEE_PR_ID
from FACETS..CMC_CKPY_PAYEE_SUM a
inner join CMC_PRPR_PROV b on a.CKPY_PAYEE_PR_ID=b.PRPR_ID
where a.CKPY_REF_ID = '{TransactionNumber}';

select BPCL.CLCL_ID,BPCL.CLCL_PA_ACCT_NO,convert(varchar(8),BPCL.CLCL_RECD_DT,112) as CLCL_RECD_DT,BPCL.CLCL_CL_SUB_TYPE,GRGR_ID,SBSB_ID,SBSB_LAST_NAME,SBSB_FIRST_NAME,
MEME_LAST_NAME,MEME_FIRST_NAME,PRPR_NAME,PRPR_NPI ,BPCL.CLCL_PA_PAID_AMT,CLCK_NET_AMT,CLCK_ALLOW,CLCL.CLCL_TOT_CHG,rtrim(CLCL.CLCL_MICRO_ID) as CLCL_MICRO_ID
from FACETS..CMC_BPCL_CLM BPCL
inner join CMC_CLCL_CLAIM CLCL on BPCL.CLCL_ID=CLCL.CLCL_ID
where CKPY_REF_ID = '{TransactionNumber}';

select a.CLCL_ID,CDML_SEQ_NO,substring(IPCD_ID,1,5) as IPCD_ID,CDML_CHG_AMT,CDML_ALLOW,CDML_UNITS,CDML_DED_AMT,CDML_COPAY_AMT,CDML_COINS_AMT,CDML_PAID_AMT,CDML_PR_LN_CNTL_NO_NVL,convert(varchar(8),CDML_FROM_DT,112) as CDML_FROM_DT
from CMC_CDML_CL_LINE a
inner join CMC_BPCL_CLM b on a.CLCL_ID=b.CLCL_ID
where b.CKPY_REF_ID='{TransactionNumber}'
order by a.CLCL_ID,a.CDML_SEQ_NO;
";
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    header835.Ref_Id = ds.Tables[0].Rows[0][0].ToString();
                    header835.Ref_Type = ds.Tables[0].Rows[0][1].ToString();
                    header835.CheckDate = ds.Tables[0].Rows[0][2].ToString();
                    header835.Lobd_Id = ds.Tables[0].Rows[0][3].ToString();
                    header835.Payee_Id = ds.Tables[0].Rows[0][11].ToString();
                    header835.Payee_Type = ds.Tables[0].Rows[0][4].ToString();
                    header835.Orig_Amt = ((decimal)ds.Tables[0].Rows[0][5]).ToString("G29");
                    header835.Deduct_Amt = ((decimal)ds.Tables[0].Rows[0][6]).ToString("G29");
                    header835.Net_Amt = ((decimal)ds.Tables[0].Rows[0][7]).ToString("G29");
                    header835.PayeeName = ds.Tables[0].Rows[0][8].ToString().Trim();
                    header835.PayeeNpi = ds.Tables[0].Rows[0][9].ToString();
                    header835.PayeeTaxId = ds.Tables[0].Rows[0][10].ToString();
                    header835.PayeeAddress = "PO Box 35180";
                    header835.PayeeCity = "Seattle";
                    header835.PayeeState = "WA";
                    header835.PayeeZip = "98124";
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ClaimHeader header = new ClaimHeader();
                        header.ProviderClaimId = ds.Tables[1].Rows[i][1].ToString();
                        header.ChargeAmount = ((decimal)ds.Tables[1].Rows[i][15]).ToString("G29").TrimStart('0');
                        header.PaidAmount = ((decimal)ds.Tables[1].Rows[i][13]).ToString("G29");
                        header.PatientResponsibleAmount = ((decimal)ds.Tables[1].Rows[i][14] - (decimal)ds.Tables[1].Rows[i][13]).ToString("G29");
                        header.ClaimId = ds.Tables[1].Rows[i][0].ToString();
                        header.PatientLastName = ds.Tables[1].Rows[i][8].ToString();
                        header.PatientFirstName = ds.Tables[1].Rows[i][9].ToString();
                        header.SubscriberLastName = ds.Tables[1].Rows[i][6].ToString();
                        header.SubscriberFirstName = ds.Tables[1].Rows[i][7].ToString();
                        header.SubscriberId = ds.Tables[1].Rows[i][5].ToString();
                        header.RenderingLastName = ds.Tables[1].Rows[i][10].ToString();
                        header.RenderingId = ds.Tables[1].Rows[i][11].ToString();
                        header.GroupId = ds.Tables[1].Rows[i][4].ToString();
                        header.ReceivedDate = ds.Tables[1].Rows[i][2].ToString();
                        header.AllowedAmt = ((decimal)ds.Tables[1].Rows[i][14]).ToString("G29");
                        header.ClaimType = ds.Tables[1].Rows[i][3].ToString();
                        header.DCN = ds.Tables[1].Rows[i][16].ToString();
                        headers.Add(header);
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ClaimLine line = new ClaimLine();
                        line.ClaimId = ds.Tables[2].Rows[i][0].ToString();
                        line.LineNumber = ds.Tables[2].Rows[i][1].ToString();
                        line.ProcedureCode = ds.Tables[2].Rows[i][2].ToString();
                        line.LineChargeAmount = ((decimal)ds.Tables[2].Rows[i][3]).ToString("G29").TrimStart('0');
                        line.LinePaidAmount = ((decimal)ds.Tables[2].Rows[i][9]).ToString("G29");
                        line.UnitCount = ds.Tables[2].Rows[i][5].ToString();
                        line.DateOfService = ds.Tables[2].Rows[i][11].ToString();
                        line.AdjustAmountCO = ((decimal)ds.Tables[2].Rows[i][3] - (decimal)ds.Tables[2].Rows[i][4]).ToString("G29").TrimStart('0');
                        line.AdjustAmountPR1 = ((decimal)ds.Tables[2].Rows[i][6]).ToString("G29");
                        line.AdjustAmountPR2 = ((decimal)ds.Tables[2].Rows[i][8]).ToString("G29").TrimStart('0');
                        line.AdjustAmountPR3 = ((decimal)ds.Tables[2].Rows[i][7]).ToString("G29");
                        line.AllowedAmt = ((decimal)ds.Tables[2].Rows[i][4]).ToString("G29");
                        line.LineId = ds.Tables[2].Rows[i][10].ToString();
                        if ((decimal)ds.Tables[2].Rows[i][4] != (decimal)ds.Tables[2].Rows[i][9] + (decimal)ds.Tables[2].Rows[i][6] + (decimal)ds.Tables[2].Rows[i][7] + (decimal)ds.Tables[2].Rows[i][8])
                        {
                            line.AdjustAmountPR3 = ((decimal)ds.Tables[2].Rows[i][4] - (decimal)ds.Tables[2].Rows[i][9] - (decimal)ds.Tables[2].Rows[i][6] - (decimal)ds.Tables[2].Rows[i][8]).ToString("G29");
                        }
                        lines.Add(line);
                    }
                }
            }
            using (SqlConnection conn = new SqlConnection(cn_PROD45))
            {
                string queue = "select DCN,SBSB_ID, billtype from ch_pear_ErrAccept_Claims where DCN in ('";
                foreach (string s in headers.Select(x => x.DCN))
                {
                    queue += s + "','";
                }
                queue = queue.Substring(0, queue.Length - 2);
                queue = queue + ")";
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = queue;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    List<Tuple<string, string, string>> supps = new List<Tuple<string, string, string>>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        supps.Add(Tuple.Create(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString()));
                    }
                    List<ClaimHeader> ops = new List<ClaimHeader>(headers);
                    for (int i = 0; i < ops.Count; i++)
                    {
                        Tuple<string, string, string> supp = supps.FirstOrDefault(x => x.Item1 == ops[i].DCN);
                        if (supp != null)
                        {
                            ops[i].SubscriberId = supp.Item2;
                            ops[i].FacilityType = supp.Item3.Substring(0, 2);
                            ops[i].FrequencyCode = supp.Item3.Substring(2, 1);
                        }
                    }
                    headers = new List<ClaimHeader>(ops);
                }
            }
        }
    }
}
