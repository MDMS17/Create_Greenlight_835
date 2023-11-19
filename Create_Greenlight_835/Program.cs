using Create_Greenlight_835;
using System.Data.SqlTypes;
using System.Text;
string cn = "";
List<string> Dcns = DataAccess.GetDcns(cn);
if (Dcns.Count == 0) return;
StringBuilder sb = new StringBuilder();
int juliandate = DateTime.Today.DayOfYear;
string ICN = juliandate.ToString().PadLeft(3, '0') + DateTime.Now.ToString("Hmmssf");
string OpDate = DateTime.Today.ToString("yyyyMMdd");
string OpTime = DateTime.Now.ToString("HHmmss");
ISA isa = new ISA();
isa.ISA09_Date = OpDate.Substring(2, 6);
isa.ISA10_Time = OpTime.Substring(0, 4);
isa.ISA13_ICN = ICN;
isa.ISA15_ProductionFlag = "P";
sb.Append(isa.ToX12String());
GS gs = new GS();
gs.GS01_FunctionalIDCode = "HP";
gs.GS04_TransactionDate = OpDate;
gs.GS05_TransactionTime = OpTime;
gs.GS06_GroupControlNumber = ICN;
sb.Append(gs.ToX12String());
int STLoop = 1;
Header835 header835;
List<Line835> line835s;
int segmentCount = 0;
foreach (string DCN in Dcns)
{
    header835 = new Header835();
    line835s = new List<Line835>();
    DataAccess.GetGreenlight835Date(cn, DCN, ref header835, ref line835s);
    ST st = new ST();
    st.ST01_SetIdCode = "835";
    st.ST02_SetControlNumber = STLoop.ToString().PadLeft(4, '0');
    sb.Append(st.ToX12String());
    BPR bpr = new BPR();
    bpr.BPR01_TransactionHandlingCode = "I";
    bpr.BPR02_PaymentAmount = header835.CheckAmount;
    bpr.BPR03_CreditFlag = "C";
    bpr.BPR04_PaymentMethodCode = "CHK";
    bpr.BPR16_EffectiveDate = header835.CheckDate;
    sb.Append(bpr.ToX12String());
    TRN trn = new TRN();
    trn.TRN01_TraceTypeCode = "1";
    trn.TRN02_ReferenceId = header835.CheckNumber;
    sb.Append(trn.ToX12String());
    REF ref1 = new REF();
    ref1.REF01_Qualifier = "EV";
    sb.Append(ref1.ToX12String());
    DTM dtm = new DTM();
    dtm.DTM01_Qualifier = "405";
    dtm.DTM02_Date = header835.CheckDate;
    sb.Append(dtm.ToX12String());
    N1 n1 = new N1();
    n1.N101_NameQualifier = "PR";
    n1.N102_Name = header835.PayerName;
    n1.N103_IdQualifier = "XV";
    n1.N104_Id = header835.PayerNpi;
    sb.Append(n1.ToX12String());
    N3 n3 = new N3();
    n3.N301_Address = header835.PayerAddress;
    sb.Append(n3.ToX12String());
    N4 n4 = new N4();
    n4.N401_City = header835.PayerCity;
    n4.N402_State = header835.PayerState;
    n4.N403_Zipcode = header835.PayerZip;
    sb.Append(n4.ToX12String());
    PER per = new PER();
    per.PER01_ContactFunctionCode = "CX";
    per.PER02_ContactName = "SUPPORT";
    per.PER03_NumberQualifier = "EM";
    per.PER04_ContactNumber = header835.PayerEmail;
    sb.Append(per.ToX12String());
    n1 = new N1();
    n1.N101_NameQualifier = "PE";
    n1.N102_Name = header835.PayeeName;
    n1.N103_IdQualifier = "FI";
    n1.N104_Id = header835.PayeeTaxId;
    sb.Append(n1.ToX12String());
    n3 = new N3();
    n3.N301_Address = header835.PayeeAddress;
    sb.Append(n3.ToX12String());
    n4 = new N4();
    n4.N401_City = header835.PayeeCity;
    n4.N402_State = header835.PayeeState;
    n4.N403_Zipcode = header835.PayeeZip;
    sb.Append(n4.ToX12String());
    LX lx = new LX();
    lx.LX01_Number = "1";
    sb.Append(lx.ToX12String());
    CLP clp = new CLP();
    clp.CLP01_PatientControlNumber = header835.VendorClaimNo;
    clp.CLP02_ClaimStatusCode = "1";
    clp.CLP03_TotalClaimChargeAmount = header835.ChargeAmount;
    clp.CLP04_TotalClaimPaymentAmount = header835.PaidAmount;
    clp.CLP05_PatientResponsibilityAmount = header835.PatientResponsibility;
    clp.CLP06_ClaimFilingIndicatorCode = "12";
    clp.CLP07_PayerClaimControlNumber = header835.EldoClaimNo;
    sb.Append(clp.ToX12String());
    NM1 nm1 = new NM1();
    nm1.NM101_NameQualifier = "QC";
    nm1.NM102_NameType = "1";
    nm1.NM103_LastName = header835.PatientLastName;
    nm1.NM104_FirstName = header835.PatientFirstName;
    sb.Append(nm1.ToX12String());
    nm1 = new NM1();
    nm1.NM101_NameQualifier = "IL";
    nm1.NM102_NameType = "1";
    nm1.NM103_LastName = header835.InsurerLastName;
    nm1.NM104_FirstName = header835.InsurerFirstName;
    nm1.NM108_IDQualifer = "MI";
    nm1.NM109_IDCode = header835.InsurerSSN;
    sb.Append(nm1.ToX12String());
    ref1 = new REF();
    ref1.REF01_Qualifier = "IL";
    ref1.REF02_Id = header835.InsurerGroupNo;
    sb.Append(ref1.ToX12String());
    AMT amt = new AMT();
    amt.AMT01_Qualifier = "AU";
    amt.AMT02_Amount = header835.AllowedAmount;
    sb.Append(amt.ToX12String());
    //claim level balance
    decimal ClaimCharged = decimal.Parse(header835.ChargeAmount);
    decimal ClaimPaid = decimal.Parse(header835.PaidAmount);
    decimal ClaimAllowed = decimal.Parse(header835.AllowedAmount);
    decimal TotalLineCharged = line835s.Sum(x => decimal.Parse(x.LineChargeAmount));
    decimal TotalLinePaid = line835s.Sum(x => decimal.Parse(x.LinePaidAmount));
    decimal TotalLineAllowed = line835s.Sum(x => decimal.Parse(x.LineAllowedAmount));
    if (ClaimCharged != TotalLineCharged)
    {
        decimal rateCharged = Math.Round(ClaimCharged / TotalLineCharged, 4);
        decimal sumCharged = 0;
        for (int i = 0; i < line835s.Count; i++)
        {
            decimal lineCharged = Math.Round(decimal.Parse(line835s[i].LineChargeAmount) * rateCharged, 2);
            line835s[i].LineChargeAmount = lineCharged.ToString("#.00");
            if (i == line835s.Count - 1)
            {
                line835s[i].LineChargeAmount = (ClaimCharged - sumCharged).ToString("#.00");
            }
            sumCharged += lineCharged;
        }
    }
    if (ClaimPaid != TotalLinePaid)
    {
        decimal ratePaid = Math.Round(ClaimPaid / TotalLinePaid, 4);
        decimal sumPaid = 0;
        for (int i = 0; i < line835s.Count; i++)
        {
            decimal linePaid = Math.Round(decimal.Parse(line835s[i].LinePaidAmount) * ratePaid, 2);
            line835s[i].LinePaidAmount = linePaid.ToString("#.00");
            if (i == line835s.Count - 1)
            {
                line835s[i].LinePaidAmount = (ClaimPaid - sumPaid).ToString("#.00");
            }
            sumPaid += linePaid;
        }
    }
    if (ClaimAllowed != TotalLineAllowed)
    {
        decimal rateAllowed = Math.Round(ClaimAllowed / TotalLineAllowed, 4);
        decimal sumAllowed = 0;
        for (int i = 0; i < line835s.Count; i++)
        {
            decimal lineAllowed = Math.Round(decimal.Parse(line835s[i].LineAllowedAmount) * rateAllowed, 2);
            line835s[i].LineAllowedAmount = lineAllowed.ToString("#.00");
            if (i == line835s.Count - 1)
            {
                line835s[i].LineAllowedAmount = (ClaimAllowed - sumAllowed).ToString("#.00");
            }
            sumAllowed += lineAllowed;
        }
    }
    foreach (Line835 line835 in line835s)
    {
        //line level balance, lineallowed=linepaid+linePR; linecharge=linepaid+lineadjust
        decimal lineCharge = decimal.Parse(line835.LineChargeAmount);
        decimal linePaid = decimal.Parse(line835.LinePaidAmount);
        decimal lineAdjust = decimal.Parse(line835.LineAdjustment);
        decimal lineDeductible = decimal.Parse(line835.LineDeductible);
        decimal lineCoInsurance = decimal.Parse(line835.LineCoInsurance);
        decimal lineCoPay = decimal.Parse(line835.LineCoPay);
        decimal lineAllowed = decimal.Parse(line835.LineAllowedAmount);
        if (lineCharge != linePaid + lineAdjust)
        {
            lineAdjust = lineCharge - linePaid;
            line835.LineAdjustment = lineAdjust.ToString("#.00");
        }
        if (lineAllowed != linePaid + lineDeductible + lineCoInsurance + lineCoPay)
        {
            lineCoPay = lineAllowed - linePaid - lineDeductible - lineCoInsurance;
            line835.LineCoPay = lineCoPay.ToString("#.00");
        }
        SVC svc = new SVC();
        svc.SVC011_ServiceTypeCode = line835.ServiceCodeQualifier;
        svc.SVC012_ProcedureCode = line835.ServiceCode;
        svc.SVC02_LineChargeAmount = line835.LineChargeAmount;
        svc.SVC03_LinePaymentAmount = line835.LinePaidAmount;
        svc.SVC05_UnitsPaid = line835.Units;
        sb.Append(svc.ToX12String());
        dtm = new DTM();
        dtm.DTM01_Qualifier = "472";
        dtm.DTM02_Date = line835.Service_From;
        sb.Append(dtm.ToX12String());
        if (lineAdjust != 0)
        {
            CAS cas = new CAS();
            cas.CAS01_GroupCode = "CO";
            cas.CAS02_ReasonCode = "45";
            cas.CAS03_Amount = line835.LineAdjustment;
            sb.Append(cas.ToX12String());
        }
        if (lineDeductible != 0)
        {
            CAS cas = new CAS();
            cas.CAS01_GroupCode = "PR";
            cas.CAS02_ReasonCode = "1";
            cas.CAS03_Amount = line835.LineDeductible;
            sb.Append(cas.ToX12String());
        }
        if (lineCoInsurance != 0)
        {
            CAS cas = new CAS();
            cas.CAS01_GroupCode = "PR";
            cas.CAS02_ReasonCode = "2";
            cas.CAS03_Amount = line835.LineCoInsurance;
            sb.Append(cas.ToX12String());
        }
        if (lineCoPay != 0)
        {
            CAS cas = new CAS();
            cas.CAS01_GroupCode = "PR";
            cas.CAS02_ReasonCode = "3";
            cas.CAS03_Amount = line835.LineCoPay;
            sb.Append(cas.ToX12String());
        }
    }
    int seCount = sb.ToString().Count(x => x == '~');
    SE se = new SE();
    se.SE01_NumberOfSegments = (seCount - segmentCount - 1).ToString();
    se.SE02_TransactionControlNumber = STLoop.ToString().PadLeft(4, '0');
    sb.Append(se.ToX12String());
    segmentCount = seCount;
    STLoop++;
}
GE ge = new GE();
ge.GE01_NumberOfTransactions = (STLoop - 1).ToString();
ge.GE02_GroupControlNumber = ICN;
sb.Append(ge.ToX12String());
IEA iea = new IEA();
iea.IEA01_NumberOfFunctionalGroups = "1";
iea.IEA02_InterchangeControlNumber = ICN;
sb.Append(iea.ToX12String());
File.WriteAllText(@"C:\Project_MY\GreenLight\New_Gens\BENESYS_" + OpDate + "_" + OpTime + "_CLAIM_835.TXT", sb.ToString().Replace("~", "~" + Environment.NewLine));
