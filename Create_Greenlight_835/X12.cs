using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Create_Greenlight_835
{
    public class ISA
    {
        public string ISA06_SenderID { get; set; }
        public string ISA08_ReceiverID { get; set; }
        public string ISA09_Date { get; set; }
        public string ISA10_Time { get; set; }
        public string ISA13_ICN { get; set; }
        public string ISA15_ProductionFlag { get; set; }
        public string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ISA*00*          *00*          *ZZ*");
            sb.Append(ISA06_SenderID.PadRight(15, ' '));
            sb.Append("*ZZ*" + ISA08_ReceiverID.PadRight(15, ' '));
            sb.Append("*" + ISA09_Date + "*" + ISA10_Time + "*^*00501*" + ISA13_ICN + "*1*" + ISA15_ProductionFlag + "*:~");
            return sb.ToString();
        }
    }
    public class GS
    {
        public string GS01_FunctionalIDCode { get; set; }
        public string GS02_SenderID { get; set; }
        public string GS03_ReceiverID { get; set; }
        public string GS04_TransactionDate { get; set; }
        public string GS05_TransactionTime { get; set; }
        public string GS06_GroupControlNumber { get; set; }
        public string GS07_ResponsibleAgencyCode { get; set; }
        public string GS08_VersionID { get; set; }
        public string ToX12String()
        {
            return $"GS*{GS01_FunctionalIDCode}*{GS02_SenderID}*{GS03_ReceiverID}*{GS04_TransactionDate}*{GS05_TransactionTime}*{GS06_GroupControlNumber}*{GS07_ResponsibleAgencyCode}*{GS08_VersionID}~";
        }
    }
    public class ST
    {
        public string ST01_SetIdCode { get; set; }
        public string ST02_SetControlNumber { get; set; }
        public string ToX12String()
        {
            return $"ST*{ST01_SetIdCode}*{ST02_SetControlNumber}~";
        }
    }
    public class BPR
    {
        public string BPR01_TransactionHandlingCode { get; set; }
        public string BPR02_PaymentAmount { get; set; }
        public string BPR03_CreditFlag { get; set; }
        public string BPR04_PaymentMethodCode { get; set; }
        public string BPR05_PaymentFormatCode { get; set; }
        public string BPR06_IdQualifier { get; set; }
        public string BPR07_IdNumber { get; set; }
        public string BPR08_SenderAccountQualifier { get; set; }
        public string BPR09_SenderAccountNumber { get; set; }
        public string BPR10_OriginatingCompanyId { get; set; }
        public string BPR11_OriginatingCompanySupplementalCode { get; set; }
        public string BPR12_DFIIdQualifier { get; set; }
        public string BPR13_ReceiverBankNumber { get; set; }
        public string BPR14_ReceiverAccountQualifier { get; set; }
        public string BPR15_ReceiverAccountNumber { get; set; }
        public string BPR16_EffectiveDate { get; set; }
        public string ToX12String()
        {
            return $"BPR*{BPR01_TransactionHandlingCode}*{BPR02_PaymentAmount}*{BPR03_CreditFlag}*{BPR04_PaymentMethodCode}*{BPR05_PaymentFormatCode}*{BPR06_IdQualifier}*{BPR07_IdNumber}*{BPR08_SenderAccountQualifier}*{BPR09_SenderAccountNumber}*{BPR10_OriginatingCompanyId}*{BPR11_OriginatingCompanySupplementalCode}*{BPR12_DFIIdQualifier}*{BPR13_ReceiverBankNumber}*{BPR14_ReceiverAccountQualifier}*{BPR15_ReceiverAccountNumber}*{BPR16_EffectiveDate}~";
        }
    }
    public class TRN
    {
        public string TRN01_TraceTypeCode { get; set; }
        public string TRN02_ReferenceId { get; set; }
        public string TRN03_OriginatingCompanyId { get; set; }
        public string TRN04_OriginatingCompanySupplementalCode { get; set; }
        public string ToX12String()
        {
            string retStr = $"TRN*{TRN01_TraceTypeCode}*{TRN02_ReferenceId}*{TRN03_OriginatingCompanyId}";
            if (!string.IsNullOrEmpty(TRN04_OriginatingCompanySupplementalCode)) retStr += $"*{TRN04_OriginatingCompanySupplementalCode}";
            return retStr + "~";
        }
    }
    public class DTM
    {
        public string DTM01_Qualifier { get; set; }
        public string DTM02_Date { get; set; }
        public string ToX12String()
        {
            return $"DTM*{DTM01_Qualifier}*{DTM02_Date}~";
        }
    }
    public class N1
    {
        public string N101_NameQualifier { get; set; }
        public string N102_Name { get; set; }
        public string N103_IdQualifier { get; set; }
        public string N104_Id { get; set; }
        public string ToX12String()
        {
            if (string.IsNullOrEmpty(N103_IdQualifier) || string.IsNullOrEmpty(N104_Id))
            {
                return $"N1*{N101_NameQualifier}*{N102_Name}~";
            }
            else
            {
                return $"N1*{N101_NameQualifier}*{N102_Name}*{N103_IdQualifier}*{N104_Id}~";
            }
        }
    }
    public class N3
    {
        public string N301_Address { get; set; }
        public string N302_Address2 { get; set; }
        public string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("N3*" + N301_Address);
            if (!string.IsNullOrEmpty(N302_Address2)) sb.Append("*" + N302_Address2);
            sb.Append("~");
            return sb.ToString();
        }
    }
    public class N4
    {
        public string N401_City { get; set; }
        public string N402_State { get; set; }
        public string N403_Zipcode { get; set; }
        public string N404_Country { get; set; }
        public string N405_CountrySubCode { get; set; }
        public string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("N4*" + N401_City);
            if (!string.IsNullOrEmpty(N405_CountrySubCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(N402_State)) sb.Append(N402_State);
                sb.Append("*");
                if (!string.IsNullOrEmpty(N403_Zipcode)) sb.Append(N403_Zipcode);
                sb.Append("*");
                if (!string.IsNullOrEmpty(N404_Country)) sb.Append(N404_Country);
                sb.Append("***" + N405_CountrySubCode);
            }
            else if (!string.IsNullOrEmpty(N404_Country))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(N402_State)) sb.Append(N402_State);
                sb.Append("*");
                if (!string.IsNullOrEmpty(N403_Zipcode)) sb.Append(N403_Zipcode);
                sb.Append("*" + N404_Country);
            }
            else if (!string.IsNullOrEmpty(N403_Zipcode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(N402_State)) sb.Append(N402_State);
                sb.Append("*" + N403_Zipcode);
            }
            else if (!string.IsNullOrEmpty(N402_State))
            {
                sb.Append("*" + N402_State);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
    public class PER
    {
        public string PER01_ContactFunctionCode { get; set; }
        public string PER02_ContactName { get; set; }
        public string PER03_NumberQualifier { get; set; }
        public string PER04_ContactNumber { get; set; }
        public string PER05_SecondQualifier { get; set; }
        public string PER06_SecondNumber { get; set; }
        public string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"PER*{PER01_ContactFunctionCode}");
            if (!string.IsNullOrEmpty(PER02_ContactName)) sb.Append($"*{PER02_ContactName}");
            if (!string.IsNullOrEmpty(PER04_ContactNumber)) sb.Append($"*{PER03_NumberQualifier}*{PER04_ContactNumber}");
            if (!string.IsNullOrEmpty(PER06_SecondNumber)) sb.Append($"*{PER05_SecondQualifier}*{PER06_SecondNumber}");
            sb.Append("~");
            return sb.ToString();
        }
    }
    public class REF
    {
        public string REF01_Qualifier { get; set; }
        public string REF02_Id { get; set; }
        public string ToX12String()
        {
            return $"REF*{REF01_Qualifier}*{REF02_Id}~";
        }
    }
    public class LX
    {
        public string LX01_Number { get; set; }
        public string ToX12String()
        {
            return $"LX*{LX01_Number}~";
        }
    }
    public class CLP
    {
        public string CLP01_PatientControlNumber { get; set; }
        public string CLP02_ClaimStatusCode { get; set; }
        public string CLP03_TotalClaimChargeAmount { get; set; }
        public string CLP04_TotalClaimPaymentAmount { get; set; }
        public string CLP05_PatientResponsibilityAmount { get; set; }
        public string CLP06_ClaimFilingIndicatorCode { get; set; }
        public string CLP07_PayerClaimControlNumber { get; set; }
        public string CLP08_FacilityTypeCode { get; set; }
        public string CLP09_ClaimFrequencyCode { get; set; }
        public string CLP10_PatientStatusCode { get; set; }
        public string CLP11_DRGCode { get; set; }
        public string CLP12_DRGWeight { get; set; }
        public string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"CLP*{CLP01_PatientControlNumber}*{CLP02_ClaimStatusCode}*{CLP03_TotalClaimChargeAmount}*{CLP04_TotalClaimPaymentAmount}*{CLP05_PatientResponsibilityAmount}*{CLP06_ClaimFilingIndicatorCode}*{CLP07_PayerClaimControlNumber}");
            if (!string.IsNullOrEmpty(CLP12_DRGWeight))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(CLP08_FacilityTypeCode)) sb.Append($"{CLP08_FacilityTypeCode}");
                sb.Append("*");
                if (!string.IsNullOrEmpty(CLP09_ClaimFrequencyCode)) sb.Append($"{CLP09_ClaimFrequencyCode}");
                sb.Append("*");
                if (!string.IsNullOrEmpty(CLP10_PatientStatusCode)) sb.Append($"{CLP10_PatientStatusCode}");
                sb.Append("*");
                if (!string.IsNullOrEmpty(CLP11_DRGCode)) sb.Append($"{CLP11_DRGCode}");
                sb.Append($"*{CLP12_DRGWeight}");
            }
            else if (!string.IsNullOrEmpty(CLP11_DRGCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(CLP08_FacilityTypeCode)) sb.Append($"{CLP08_FacilityTypeCode}");
                sb.Append("*");
                if (!string.IsNullOrEmpty(CLP09_ClaimFrequencyCode)) sb.Append($"{CLP09_ClaimFrequencyCode}");
                sb.Append("*");
                if (!string.IsNullOrEmpty(CLP10_PatientStatusCode)) sb.Append($"{CLP10_PatientStatusCode}");
                sb.Append($"*{CLP11_DRGCode}");
            }
            else if (!string.IsNullOrEmpty(CLP10_PatientStatusCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(CLP08_FacilityTypeCode)) sb.Append($"{CLP08_FacilityTypeCode}");
                sb.Append("*");
                if (!string.IsNullOrEmpty(CLP09_ClaimFrequencyCode)) sb.Append($"{CLP09_ClaimFrequencyCode}");
                sb.Append($"*{CLP10_PatientStatusCode}");
            }
            else if (!string.IsNullOrEmpty(CLP09_ClaimFrequencyCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(CLP08_FacilityTypeCode)) sb.Append($"{CLP08_FacilityTypeCode}");
                sb.Append($"*{CLP09_ClaimFrequencyCode}");
            }
            else if (!string.IsNullOrEmpty(CLP08_FacilityTypeCode))
            {
                sb.Append($"*{CLP08_FacilityTypeCode}");
            }
            sb.Append("~");
            return sb.ToString();
        }

    }
    public class NM1
    {
        public string NM101_NameQualifier { get; set; }
        public string NM102_NameType { get; set; }
        public string NM103_LastName { get; set; }
        public string NM104_FirstName { get; set; }
        public string NM105_MiddleName { get; set; }
        public string NM107_Suffix { get; set; }
        public string NM108_IDQualifer { get; set; }
        public string NM109_IDCode { get; set; }
        public string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("NM1*" + NM101_NameQualifier + "*" + NM102_NameType);
            if (!string.IsNullOrEmpty(NM109_IDCode))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(NM103_LastName)) sb.Append(NM103_LastName);
                sb.Append("*");
                if (!string.IsNullOrEmpty(NM104_FirstName)) sb.Append(NM104_FirstName);
                sb.Append("*");
                if (!string.IsNullOrEmpty(NM105_MiddleName)) sb.Append(NM105_MiddleName);
                sb.Append("**");
                if (!string.IsNullOrEmpty(NM107_Suffix)) sb.Append(NM107_Suffix);
                sb.Append("*");
                if (!string.IsNullOrEmpty(NM108_IDQualifer)) sb.Append(NM108_IDQualifer);
                sb.Append("*" + NM109_IDCode);
            }
            else if (!string.IsNullOrEmpty(NM107_Suffix))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(NM103_LastName)) sb.Append(NM103_LastName);
                sb.Append("*");
                if (!string.IsNullOrEmpty(NM104_FirstName)) sb.Append(NM104_FirstName);
                sb.Append("*");
                if (!string.IsNullOrEmpty(NM105_MiddleName)) sb.Append(NM105_MiddleName);
                sb.Append("**" + NM107_Suffix);
            }
            else if (!string.IsNullOrEmpty(NM105_MiddleName))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(NM103_LastName)) sb.Append(NM103_LastName);
                sb.Append("*");
                if (!string.IsNullOrEmpty(NM104_FirstName)) sb.Append(NM104_FirstName);
                sb.Append("*" + NM105_MiddleName);
            }
            else if (!string.IsNullOrEmpty(NM104_FirstName))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(NM103_LastName)) sb.Append(NM103_LastName);
                sb.Append("*" + NM104_FirstName);
            }
            else if (!string.IsNullOrEmpty(NM103_LastName))
            {
                sb.Append("*" + NM103_LastName);
            }
            sb.Append("~");
            return sb.ToString();
        }
    }
    public class AMT
    {
        public string AMT01_Qualifier { get; set; }
        public string AMT02_Amount { get; set; }
        public string ToX12String()
        {
            return $"AMT*{AMT01_Qualifier}*{AMT02_Amount}~";
        }
    }
    public class SVC
    {
        public string SVC011_ServiceTypeCode { get; set; }
        public string SVC012_ProcedureCode { get; set; }
        public string SVC013_Modifier1 { get; set; }
        public string SVC014_Modifier2 { get; set; }
        public string SVC015_Modifier3 { get; set; }
        public string SVC016_Modifier4 { get; set; }
        public string SVC017_Description { get; set; }
        public string SVC02_LineChargeAmount { get; set; }
        public string SVC03_LinePaymentAmount { get; set; }
        public string SVC04_RevenueCode { get; set; }
        public string SVC05_UnitsPaid { get; set; }
        public string SVC061_OriginalIdQualifier { get; set; }
        public string SVC062_OriginalProcedureCode { get; set; }
        public string SVC063_OriginalModifier1 { get; set; }
        public string SVC064_OriginalModifier2 { get; set; }
        public string SVC065_OriginalModifier3 { get; set; }
        public string SVC066_OriginalModifier4 { get; set; }
        public string SVC067_OriginalDescription { get; set; }
        public string SVC07_OriginalUnits { get; set; }
        public string ToX12String()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"SVC*{SVC011_ServiceTypeCode}:{SVC012_ProcedureCode}");
            if (!string.IsNullOrEmpty(SVC017_Description))
            {
                int delimiterrepeat = 0;
                if (!string.IsNullOrEmpty(SVC013_Modifier1)) { sb.Append(":" + SVC013_Modifier1); delimiterrepeat++; }
                if (!string.IsNullOrEmpty(SVC014_Modifier2)) { sb.Append(":" + SVC014_Modifier2); delimiterrepeat++; }
                if (!string.IsNullOrEmpty(SVC015_Modifier3)) { sb.Append(":" + SVC015_Modifier3); delimiterrepeat++; }
                if (!string.IsNullOrEmpty(SVC016_Modifier4)) { sb.Append(":" + SVC016_Modifier4); delimiterrepeat++; }
                sb.Append(new String(':', 5 - delimiterrepeat) + SVC017_Description);
            }
            else
            {
                if (!string.IsNullOrEmpty(SVC013_Modifier1)) sb.Append(":" + SVC013_Modifier1);
                if (!string.IsNullOrEmpty(SVC014_Modifier2)) sb.Append(":" + SVC014_Modifier2);
                if (!string.IsNullOrEmpty(SVC015_Modifier3)) sb.Append(":" + SVC015_Modifier3);
                if (!string.IsNullOrEmpty(SVC016_Modifier4)) sb.Append(":" + SVC016_Modifier4);
            }
            sb.Append("*" + SVC02_LineChargeAmount);
            sb.Append("*" + SVC03_LinePaymentAmount);
            sb.Append("*" + SVC04_RevenueCode);
            if (string.IsNullOrEmpty(SVC05_UnitsPaid)) sb.Append("*1");
            else sb.Append($"*{SVC05_UnitsPaid}");
            sb.Append("~");
            if (!string.IsNullOrEmpty(SVC07_OriginalUnits))
            {
                sb.Append("*");
                if (!string.IsNullOrEmpty(SVC062_OriginalProcedureCode)) sb.Append($"{SVC061_OriginalIdQualifier}:{SVC062_OriginalProcedureCode}");
                sb.Append($"*{SVC07_OriginalUnits}");
            }
            else if (!string.IsNullOrEmpty(SVC062_OriginalProcedureCode))
            {
                sb.Append($"*{SVC061_OriginalIdQualifier}:{SVC062_OriginalProcedureCode}");
            }
            return sb.ToString();
        }
    }
    public class CAS
    {
        public string CAS01_GroupCode { get; set; }
        public string CAS02_ReasonCode { get; set; }
        public string CAS03_Amount { get; set; }
        public string CAS04_Quantity { get; set; }
        public string ToX12String()
        {
            return $"CAS*{CAS01_GroupCode}*{CAS02_ReasonCode}*{CAS03_Amount}*{CAS04_Quantity}~";
        }
    }
    public class LQ
    {
        public string LQ01_LineRemarkQualifier { get; set; }
        public string LQ02_LineRemarkCode { get; set; }
        public string ToX12String()
        {
            return $"LQ*{LQ01_LineRemarkQualifier}*{LQ02_LineRemarkCode}~";
        }
    }
    public class PLB
    {
        public string PLB01_ProviderIdentifier { get; set; }
        public string PLB02_FiscalPeriodDate { get; set; }
        public string PLB031_AdjustmentReasonCode { get; set; }
        public string PLB032_AdjustmentIDentifier { get; set; }
        public string PLB04_AdjustmentAmount { get; set; }
        public string ToX12String()
        {
            return $"PLB*{PLB01_ProviderIdentifier}*{PLB02_FiscalPeriodDate}*{PLB031_AdjustmentReasonCode}:{PLB032_AdjustmentIDentifier}*{PLB04_AdjustmentAmount}~";
        }
    }
    public class SE
    {
        public string SE01_NumberOfSegments { get; set; }
        public string SE02_TransactionControlNumber { get; set; }
        public string ToX12String()
        {
            return $"SE*{SE01_NumberOfSegments}*{SE02_TransactionControlNumber}~";
        }
    }
    public class GE
    {
        public string GE01_NumberOfTransactions { get; set; }
        public string GE02_GroupControlNumber { get; set; }
        public string ToX12String()
        {
            return $"GE*{GE01_NumberOfTransactions}*{GE02_GroupControlNumber}~";
        }
    }
    public class IEA
    {
        public string IEA01_NumberOfFunctionalGroups { get; set; }
        public string IEA02_InterchangeControlNumber { get; set; }
        public string ToX12String()
        {
            return $"IEA*{IEA01_NumberOfFunctionalGroups}*{IEA02_InterchangeControlNumber}~";
        }
    }
}