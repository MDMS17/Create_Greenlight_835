using System.Text.Json;
using System.Text;
namespace Create_Greenlight_835
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("appsettings.json"));
            string outputPath = settings.OutputPath;
            string connectionString = settings.ConnectionStrings["Facets"];
            string TransactionNumber = "23289B1000010102";
            Header835 header835 = new Header835();
            List<ClaimHeader> headers = new List<ClaimHeader>();
            List<ClaimLine> lines = new List<ClaimLine>();
            DataOperations.GetRemitData(connectionString, settings.ConnectionStrings["PROD45"], TransactionNumber, ref header835, ref headers, ref lines);
            if (headers.Count == 0) return;
            StringBuilder sb = new StringBuilder();
            int JulianDate = DateTime.Today.DayOfYear;
            string ICN = JulianDate.ToString().PadLeft(3, '0') + DateTime.Now.ToString("HHmmssff").Substring(1, 6);
            string OpDate = DateTime.Today.ToString("yyyyMMdd");
            string OpTime = DateTime.Now.ToString("HHmmss");
            ISA isa = new ISA();
            isa.ISA06_SenderID = "820344294";
            isa.ISA08_ReceiverID = "592715634";
            isa.ISA09_Date = OpDate.Substring(2, 6);
            isa.ISA10_Time = OpTime.Substring(0, 4);
            isa.ISA13_ICN = ICN;
            isa.ISA15_ProductionFlag = "P";
            sb.Append(isa.ToX12String());
            GS gs = new GS();
            gs.GS01_FunctionalIDCode = "HP";
            gs.GS02_SenderID = "820344294";
            gs.GS03_ReceiverID = "592715634";
            gs.GS04_TransactionDate = OpDate;
            gs.GS05_TransactionTime = OpTime.Substring(0, 4);
            gs.GS06_GroupControlNumber = ICN;
            gs.GS07_ResponsibleAgencyCode = "X";
            gs.GS08_VersionID = "005010X221A1";
            sb.Append(gs.ToX12String());
            ST st = new ST();
            st.ST01_SetIdCode = "835";
            st.ST02_SetControlNumber = "0001";
            sb.Append(st.ToX12String());
            BPR bpr = new BPR();
            bpr.BPR01_TransactionHandlingCode = "I";
            bpr.BPR02_PaymentAmount = header835.Net_Amt;
            bpr.BPR03_CreditFlag = "C";
            bpr.BPR04_PaymentMethodCode = "ACH";
            bpr.BPR05_PaymentFormatCode = "CTX";
            bpr.BPR06_IdQualifier = "01";
            bpr.BPR07_IdNumber = "882112111";
            bpr.BPR08_SenderAccountQualifier = "DA";
            bpr.BPR09_SenderAccountNumber = "0123456789";
            bpr.BPR10_OriginatingCompanyId = "1820344294";
            bpr.BPR12_DFIIdQualifier = "01";
            bpr.BPR13_ReceiverBankNumber = "121000248";
            bpr.BPR14_ReceiverAccountQualifier = "DA";
            bpr.BPR15_ReceiverAccountNumber = "0883172785";
            bpr.BPR16_EffectiveDate = OpDate;
            sb.Append(bpr.ToX12String());
            TRN trn = new TRN();
            trn.TRN01_TraceTypeCode = "1";
            trn.TRN02_ReferenceId = TransactionNumber;
            trn.TRN03_OriginatingCompanyId = "1820344294";
            sb.Append(trn.ToX12String());
            REF ref1 = new REF();
            ref1.REF01_Qualifier = "EV";
            ref1.REF02_Id = header835.Payee_Id;
            sb.Append(ref1.ToX12String());
            DTM dtm = new DTM();
            dtm.DTM01_Qualifier = "405";
            dtm.DTM02_Date = header835.CheckDate;
            sb.Append(dtm.ToX12String());
            N1 n1 = new N1();
            n1.N101_NameQualifier = "PR";
            n1.N102_Name = "Blue Cross of Idaho";
            sb.Append(n1.ToX12String());
            N3 n3 = new N3();
            n3.N301_Address = "3000 E Pine";
            sb.Append(n3.ToX12String());
            N4 n4 = new N4();
            n4.N401_City = "Meridian";
            n4.N402_State = "ID";
            n4.N403_Zipcode = "83642";
            sb.Append(n4.ToX12String());
            ref1 = new REF();
            ref1.REF01_Qualifier = "2U";
            ref1.REF02_Id = header835.Lobd_Id;
            sb.Append(ref1.ToX12String());
            PER per = new PER();
            per.PER01_ContactFunctionCode = "BL";
            per.PER02_ContactName = "EDI HELP DESK";
            per.PER03_NumberQualifier = "EM";
            per.PER04_ContactNumber = "EDIHELPDESK@BCIDAHO.COM";
            sb.Append(per.ToX12String());
            n1 = new N1();
            n1.N101_NameQualifier = "PE";
            n1.N102_Name = header835.PayeeName;
            n1.N103_IdQualifier = "XX";
            n1.N104_Id = header835.PayeeNpi;
            sb.Append(n1.ToX12String());
            n3 = new N3();
            n3.N301_Address = header835.PayeeAddress;
            n3.N302_Address2 = header835.PayeeAddress2;
            sb.Append(n3.ToX12String());
            n4 = new N4();
            n4.N401_City = header835.PayeeCity;
            n4.N402_State = header835.PayeeState;
            n4.N403_Zipcode = header835.PayeeZip;
            sb.Append(n4.ToX12String());
            ref1 = new REF();
            ref1.REF01_Qualifier = "TJ";
            ref1.REF02_Id = header835.PayeeTaxId;
            sb.Append(ref1.ToX12String());
            LX lx = new LX();
            lx.LX01_Number = "0";
            sb.Append(lx.ToX12String());
            foreach (ClaimHeader header in headers)
            {
                CLP clp = new CLP();
                clp.CLP01_PatientControlNumber = header.ProviderClaimId;
                clp.CLP02_ClaimStatusCode = "1";
                clp.CLP03_TotalClaimChargeAmount = header.ChargeAmount;
                clp.CLP04_TotalClaimPaymentAmount = header.PaidAmount;
                if (header.PatientResponsibleAmount != "0")
                {
                    clp.CLP05_PatientResponsibilityAmount = header.PatientResponsibleAmount;
                }
                clp.CLP06_ClaimFilingIndicatorCode = "12";
                clp.CLP07_PayerClaimControlNumber = header.ClaimId;
                clp.CLP08_FacilityTypeCode = header.FacilityType;
                clp.CLP09_ClaimFrequencyCode = header.FrequencyCode;
                sb.Append(clp.ToX12String());
                NM1 nm1 = new NM1();
                nm1.NM101_NameQualifier = "QC";
                nm1.NM102_NameType = "1";
                nm1.NM103_LastName = header.PatientLastName;
                nm1.NM104_FirstName = header.PatientFirstName;
                sb.Append(nm1.ToX12String());
                nm1 = new NM1();
                nm1.NM101_NameQualifier = "IL";
                nm1.NM102_NameType = "1";
                nm1.NM103_LastName = header.SubscriberLastName;
                nm1.NM104_FirstName = header.SubscriberFirstName;
                nm1.NM108_IDQualifer = "MI";
                nm1.NM109_IDCode = header.SubscriberId;
                sb.Append(nm1.ToX12String());
                nm1 = new NM1();
                if (!string.IsNullOrEmpty(header.RenderingLastName))
                {
                    nm1.NM101_NameQualifier = "82";
                    if (string.IsNullOrEmpty(header.RenderingFirstName))
                    {
                        nm1.NM102_NameType = "2";
                    }
                    else
                    {
                        nm1.NM102_NameType = "1";
                        nm1.NM104_FirstName = header.RenderingFirstName;
                    }
                    nm1.NM103_LastName = header.RenderingLastName;
                    nm1.NM108_IDQualifer = "XX";
                    nm1.NM109_IDCode = header.RenderingId;
                    sb.Append(nm1.ToX12String());
                }
                ref1 = new REF();
                ref1.REF01_Qualifier = "1L";
                ref1.REF02_Id = header.GroupId;
                sb.Append(ref1.ToX12String());
                dtm = new DTM();
                dtm.DTM01_Qualifier = "050";
                dtm.DTM02_Date = header.ReceivedDate;
                sb.Append(dtm.ToX12String());
                AMT amt = new AMT();
                if (header.AllowedAmt != "0")
                {
                    amt.AMT01_Qualifier = "AU";
                    amt.AMT02_Amount = header.AllowedAmt;
                    sb.Append(amt.ToX12String());
                }
                foreach (ClaimLine line in lines.Where(x => x.ClaimId == header.ClaimId).OrderBy(x => int.Parse(x.LineNumber)))
                {
                    SVC svc = new SVC();
                    svc.SVC011_ServiceTypeCode = "HC";
                    if (header.ClaimType == "H") svc.SVC011_ServiceTypeCode = "NU";
                    svc.SVC012_ProcedureCode = line.ProcedureCode;
                    svc.SVC02_LineChargeAmount = line.LineChargeAmount;
                    svc.SVC03_LinePaymentAmount = line.LinePaidAmount;
                    svc.SVC05_UnitsPaid = line.UnitCount;
                    sb.Append(svc.ToX12String());
                    dtm = new DTM();
                    dtm.DTM01_Qualifier = "472";
                    dtm.DTM02_Date = line.DateOfService;
                    sb.Append(dtm.ToX12String());
                    if (!string.IsNullOrEmpty(line.AdjustAmountCO) && line.AdjustAmountCO != "0")
                    {
                        CAS cas = new CAS();
                        cas.CAS01_GroupCode = "CO";
                        cas.CAS02_ReasonCode = "45";
                        cas.CAS03_Amount = line.AdjustAmountCO;
                        sb.Append(cas.ToX12String());
                    }
                    if (!string.IsNullOrEmpty(line.AdjustAmountPR1) && line.AdjustAmountPR1 != "0")
                    {
                        CAS cas = new CAS();
                        cas.CAS01_GroupCode = "PR";
                        cas.CAS02_ReasonCode = "1";
                        cas.CAS03_Amount = line.AdjustAmountPR1;
                        sb.Append(cas.ToX12String());
                    }
                    if (!string.IsNullOrEmpty(line.AdjustAmountPR2) && line.AdjustAmountPR2 != "0")
                    {
                        CAS cas = new CAS();
                        cas.CAS01_GroupCode = "PR";
                        cas.CAS02_ReasonCode = "2";
                        cas.CAS03_Amount = line.AdjustAmountPR2;
                        sb.Append(cas.ToX12String());
                    }
                    if (!string.IsNullOrEmpty(line.AdjustAmountPR3) && line.AdjustAmountPR3 != "0")
                    {
                        CAS cas = new CAS();
                        cas.CAS01_GroupCode = "PR";
                        cas.CAS02_ReasonCode = "3";
                        cas.CAS03_Amount = line.AdjustAmountPR3;
                        sb.Append(cas.ToX12String());
                    }
                    if (!string.IsNullOrEmpty(line.LineId))
                    {
                        ref1 = new REF();
                        ref1.REF01_Qualifier = "6R";
                        ref1.REF02_Id = line.LineId;
                        sb.Append(ref1.ToX12String());
                    }
                    if (!string.IsNullOrEmpty(line.AllowedAmt) && line.AllowedAmt != "0")
                    {
                        amt = new AMT();
                        amt.AMT01_Qualifier = "B6";
                        amt.AMT02_Amount = line.AllowedAmt;
                        sb.Append(amt.ToX12String());
                    }
                }
            }
            SE se = new SE();
            int SeCount = sb.ToString().Count(x => x == '~');
            se.SE01_NumberOfSegments = (SeCount - 1).ToString();
            se.SE02_TransactionControlNumber = "0001";
            sb.Append(se.ToX12String());
            GE ge = new GE();
            ge.GE01_NumberOfTransactions = "1";
            ge.GE02_GroupControlNumber = ICN;
            sb.Append(ge.ToX12String());
            IEA iea = new IEA();
            iea.IEA01_NumberOfFunctionalGroups = "1";
            iea.IEA02_InterchangeControlNumber = ICN;
            sb.Append(iea.ToX12String());
            string FileName = Path.Combine(settings.OutputPath, "SSIEDI.BCI835.5010." + OpDate + "-" + OpDate + "-" + OpTime.Substring(0, 4) + ".835");
            File.WriteAllText(FileName, sb.ToString());
        }
    }
}
