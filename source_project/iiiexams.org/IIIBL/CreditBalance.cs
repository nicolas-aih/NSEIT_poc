using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IIIDL;

namespace IIIBL
{
    public class CreditBalance
    {
        public String ApproveRejectCreditBalanceEntry(System.String ConnectionString, System.Int64 InstructionId, System.String Status, System.String ApproversRemark, System.Int64 ApprovedBy)
        {
            IIIDL.CreditBalance objCreditBalance = null;
            String Message = String.Empty;
            try
            {
                objCreditBalance = new IIIDL.CreditBalance();
                Message = objCreditBalance.ApproveRejectCreditBalanceEntry(ConnectionString, InstructionId, Status, ApproversRemark, ApprovedBy );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objCreditBalance = null;
            }
            return Message;
        }

        public String SaveCreditBalanceEntry(System.String ConnectionString, System.Int64 CompanyCode, System.String InstructionType, System.String InstrumentNo, System.Decimal Amount, System.String ModeOfPayment, System.DateTime DateOfPayment, System.String Remark, System.Int64 CreatedBy,System.String Narration)
        {
            IIIDL.CreditBalance objCreditBalance = null;
            String Message = String.Empty;
            try
            {
                objCreditBalance = new IIIDL.CreditBalance();
                Message = objCreditBalance.SaveCreditBalanceEntry(ConnectionString, CompanyCode, InstructionType, InstrumentNo, Amount, ModeOfPayment, DateOfPayment,Remark, CreatedBy, Narration);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objCreditBalance = null;
            }
            return Message;
        }

        public DataSet GetCreditBalanceEntries(System.String ConnectionString, System.Int32 Hint, System.Int64 InstructionId, System.Int32 CompanyCode)
        {
            IIIDL.CreditBalance objCreditBalance = null;
            DataSet objDataSet = null;
            try
            {
                objCreditBalance = new IIIDL.CreditBalance();
                objDataSet = objCreditBalance.GetCreditBalanceEntries(ConnectionString, Hint, InstructionId, CompanyCode );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objCreditBalance = null;
            }
            return objDataSet;
        }

        public DataSet GetLedger(System.String ConnectionString, Int64 CompanyCode, DateTime FromDate, DateTime ToDate)
        {
            IIIDL.CreditBalance objCreditBalance = null;
            DataSet objDataSet = null;
            try
            {
                objCreditBalance = new IIIDL.CreditBalance();
                objDataSet = objCreditBalance.GetLedger(ConnectionString, CompanyCode, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objCreditBalance = null;
            }
            return objDataSet;
        }

        public Decimal GetCurrentBalance(System.String ConnectionString, System.Int32 CompanyCode)
        {
            IIIDL.CreditBalance objCreditBalance = null;
            Decimal Amount = 0M;
            try
            {
                objCreditBalance = new IIIDL.CreditBalance();
                Amount = objCreditBalance.GetCurrentBalance(ConnectionString, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objCreditBalance = null;
            }
            return Amount;
        }

        public String ValidateCreditMode(System.String ConnectionString, String RoleCode, System.Int32 CompanyCode)
        {
            IIIDL.CreditBalance objCreditBalance = null;
            System.String HasSubscribedCreditBalance = "N";
            try
            {
                objCreditBalance = new IIIDL.CreditBalance();
                HasSubscribedCreditBalance = objCreditBalance.ValidateCreditMode(ConnectionString, RoleCode, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objCreditBalance = null;
            }
            return HasSubscribedCreditBalance;
        }

        public String ValidateCreditMode(System.String ConnectionString, String RoleCode, System.Int32 CompanyCode,out String TopLevelCompanyCode)
        {
            IIIDL.CreditBalance objCreditBalance = null;
            System.String HasSubscribedCreditBalance = "N";
            try
            {
                objCreditBalance = new IIIDL.CreditBalance();
                HasSubscribedCreditBalance = objCreditBalance.ValidateCreditMode(ConnectionString, RoleCode, CompanyCode, out TopLevelCompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objCreditBalance = null;
            }
            return HasSubscribedCreditBalance;
        }


        public Decimal GetCurrentBalanceOAIMS(System.String ConnectionString, System.String CompanyCode)
        {
            IIIDL.CreditBalance objCreditBalance = null;
            Decimal Amount = 0M;
            try
            {
                objCreditBalance = new IIIDL.CreditBalance();
                Amount = objCreditBalance.GetCurrentBalanceOAIMS(ConnectionString, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objCreditBalance = null;
            }
            return Amount;
        }
       
    }
}