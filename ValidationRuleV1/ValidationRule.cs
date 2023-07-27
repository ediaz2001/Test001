#region 4gl comments
//==========================================================================================
//FixUpSvcConversion: Strings Extraction Completed at: 4/13/2012 6:40 PM
//==========================================================================================
/*------------------------------------------------------------------------ 
    File        :   pe/ValidationRule.i
    Purpose     :   Contains function to process validation rules.
    Syntax      :
    Description : 
    Author(s)   :   Tatyana Kozmina
    Created     :
    Notes       : 
    History     : 

    12/19/07 TatyanaK SCR 41081 - Created.
    04/10/08 TatyanaK SCR 49692 - Check of combination of controlled segments was corrected. 
    05/08/08 irinay scr 50527 - modified glbTransNotBalance to check balance only by book amounts.
    05/16/08 irinay scr 40605 - modified IsTransNotBiLine to check correctly in case Red Storno lines with negative amounts.
    05/19/08 SergeyK scr40605 - CurAmntZeroBookCurAmntNotZero_Err function was modified to using negative BookAmounts feature.
    06/25/08 irinay scr 52149 - g CurAmntZeroBookCurAmntNotZero_Err, CurAmountZeroForCurAcc_Err, 
                                TransAndBookAmtHasDiffSigns_Err to avoid these validation rules for Currency Revaluation transactions;
                                modified IsTransNotBiLine to check correctly this line is debit or credit.
    08/04/08 SergeyK SCR 52440 - fpClosingPeriodNotExists function was fixed for correct handling closing periods.
    08/04/08 SergeyK SCR 52597 - fpApplyDateOutOfRange function was fixed for correct proceed EarliestApplyDate. 
    08/05/08 SergeyK SCR 52534 & SCR 53141 - ConvertNullToZero function were added. Usage of this function was added to 
                                CurAmntZeroBookCurAmntNotZero_Err and TransAndBookAmtHasDiffSigns_Err functions.
    08/06/08 SergeyK SCR 52602 - fpNotBasedOnApplyDate function was changed. 
    08/04/08 SergeyK SCR #### - fpClosingPeriodNotExists function was fixed.
    08/25/08 IrinaY scr 54136 - modified balTransactionsLevel to correct creating GLAccount for Balancing transaction.                          
    09/15/08 irinaY scr 52597 - modified fpApplyDateOutOfRange to correct calculation of new Date.
    09/15/08 VladimirD SCR 50101 - Reference type account validation has been added.
    09/24/08 VladimirD SCR *** - Validation for different signs has been changed.
    10/10/08 VladimirD SCR 53842 - Account validation has been corrected according effective dates.
    10/14/08 IrinaY scr 55844 - modified balTransactionsLevel to set SysRowID for new created lines.
    10/22/08 VladimirD SCR 56154 - added No GLConfirmation mode for validation rules. 
    10/24/08 nzirbes scr 50246 - updated coasegment offset and balance account fields                            
    10/28/08 irinay scr 55698 - moved setting gloTransactionAccIsCurrencyAcc and gloTransactionCurrIsSpecified from isValidTrHeader to isValidTrLine.                           
    10/29/08 irinay scr 54363 - modified IsValidTrHeader to allow posting unbalanced transactions is the special flag is set.
    10/30/08 SergeyK SCR 54482 - balTransactionsLevel procedure was changed.                         
    11/11/08 IrinaY scr 56969 - modified balTransactionsLevel to set tranSeq in zero for self-balancing lines.
    11/11/08 SergeyK SCR 52284 - REAccountNotMatchToMask_Err function was changed. REAccountWrong_Err function was renamed to REAccountWrongForOB_Err.                        
    14/11/08 VladimirD SCR 52284 - AccountIsActive function has been corrected. Minor change in balancing.                        
    11/18/08 IrinaY - scr 54458 - modified balTransactionsLevel to correct calculation amounts for Red Storno self-balansing lines.
    11/19/08 IrinaY - scr 56969 - corrected procedure IsTransNotBiLine.
    11/20/08 IrinaY - scr 54458 - roll back change of procedure balTransactionsLevel.
    12/10/08 VictorI  SCR 58495 - Implemented BVRule caching
    12/11/08 IrinaY - scr 50364 - modified balTransactionsLevel to show correct Error Message
    12/11/08 VictorI  SCR 58495 - search result is used to check whether BVRule data is cached, not shared var
    12/16/08 IrinaY   scr 58761 - modified to correct warning message is autococcection was done.
    12/23/08 IrinaY   scr 58761 - modified fpClosed to not show the same error/warning several times.
    12/25/08 SergeyK scr 53134  - corrected procedure IsTransNotBiLine.
    01/27/09 VladimirD SCR 42513 - error class has been added. Errors handling modified.
    02/27/09 VladimirD SCR 42513 - Rounding amount difference has been implemented.
    03/18/09 VladimirD SCR 60474 - Rounding calculation and checking logic has been changed.
    03/25/09 IrinaY    SCR 61236 - corrected glbTransNotBalance.
    04/27/09 VladimirD SCR 62217 - glbTransNotBalance modified. Now all transaction difference amounts less then book tolerance 
                                    should be booked to tolerance account.
    05/28/09 amiletin  scr 62340 - line indication was added in review journal error message.
    05/29/09 amiletin  scr 62341 - call all rules to get all errors for transaction line.
    06/19/09 VladimirJ SCR 62311 - AccountIsActive was corrected.
    06/29/09 VladimirD SCR 63190 - Inventory/WIP reconciliation report performance optimizations.
    08/28/09 VladimirJ SCR 62311 - additional correction for AccountIsActive.
    08/30/09 VladimirD SCR 64008 - fixes COASegment active date validation.
    09/16/09 VladimirJ SCR 62311 - proc AccountIsActive was modified: if GL Account has less than static segments in COA, return false.
    09/28/09 VladimirD SCR 65935 - fixed different signs' validation rule. Both (transaction and book) amounts can't be zero as well.
    10/05/09 VladimirD SCR 65503 - transactional amount is zero for currency account rule should be disabled for consolidation.
    10/09/09 VladimirD SCR 65935 - fixed different signs validation rule. Both (transaction and book) amounts can't be zero as well (summarize case).
    11/04/09 JuliaK    SCR 67221 - YearSuffix can be different, have to be updated with fiscal year each time. 
    11/29/09 Juliak    scr 67221 - move logic from multi-company(consolidation should use teh same logic) 
    12/02/09 VladimirD SCR 67443 - fixed AccountIsActive validation function.
    12/17/09 VictorI   SCR 65738 - corr accounting: IsTransNotBiLine rule was modified to take into account summarization
    12/15/09 VladimirD SCR 69640 - changed selfbalancing function. now works with specified book.
    12/18/09 VladimirD SCR 68451 - fixed glbTransNotBalance function. Added bookID parameter.
    01/26/10 VladimirD SCR 42513 - fixed AccountIsActive function. Query should be closed properly.
    01/27/09 VladimirJ SCR 65105 - using a patch field for GLBook.(RndTolerance,RndAccount,RndSegValue) was removed.
    01/29/09 VladimirJ SCR 68680 - proc CurAmountZeroForCurAcc_Err was modified not to check GLAllocation line.
    02/15/10 juliak    SCR 63190 - perfomance: fields()have been added to for each
    02/24/10 irinay    SCR 71389 - corrected validation of Segment Value.
    03/10/10 VladimirD SCR 71819 - fixed CombControlSegInvalid_Err. Added ? segment value handling.
    04/27/10 VladimirJ SCR 74684 - rouning difference line has a different SysRowID.
    04/29/10 TatyanaK  SCR 74568 - Rule "Transaction amount is zero for currency account" was moved to rule level.
    06/16/10 JuliaK    SCR 75033 - Performance, some issue can be checked once, should  not to do it each time.
    06/22/10 VladimirJ SCR 76372 - correctly concat REGLAccount from RESegValues in REAccountNotMatchToMask_Err.
    08/24/10 irinay    SCR 77803 - return default action if BVRule record is not found.
    08/31/10 JuliaK    SCR 67247 - Have to save valid account to Disp table once (less find for the same account).
    09/09/10 YuriR     SCR 68774 - fpApplyDateOutOfRange - Cash Management option should control Bank Adjustment, Bank Reconciliation, Bank Funds Transfer and Petty Cash. 
    09/13/10 IrinaY    SCR 74880 - Red Storno transactions should be with negative amounts. Not Red Storno transaction should be with positive amounts.
    09/24/10 IrinaY    SCR 75514 - added variable gloRevaluationTran
    10/04/10 IrinaY    SCR 79400 - corrected work of validation Rule 'Transaction currency amount is zero, but book currency amount is not zero'.
    10/08/10 VladimirD SCR 79232 - General TranGLC validation added. This should prevent duplicate posting for all transaction types.   
    10/11/10 TatyanaK  SCR 74155 - ReqDynSegNotSpec_Err was expanded to process dynamic segments with 'By Natural Account' entry control.
    10/13/10 IrinaY    SCR 77802 - corrected validation TranGLC.
    10/26/10 VladimirD SCR 79232 - TranGLC duplicates should be allowed for several transaction types.
      11/11/10 SergeyK   SCR 80224 - GetRuleLevelAction procedure modified. Suppress execution of rules from rule level for limited transaction types.
    11/25/10 VladimirD SCR 71875 - Balancing segments logic corrected.
      12/23/10 SergeyK   SCR 80224 - GetRuleLevelAction procedure modified. Suppress execution of rules from rule level for lines which not been created by booking rules.
      02/04/11 VladimirD SCR 82428 - added TranGLC.Recordtype = 'A' validation logic.
      02/09/11 JuliaK    SCR 80239 - Performance: Index has been added to Db schema , should not upload all records taht could be not necessary.
    02/11/11 CCisneros SCR 81561 - Translation project: changed all xlate(substitute()) for substitute(xlate())
    03/03/11 AbrahamM  SCR 82485 - Translations project: Fixed translations used in pe/PEFunctionsPRG.p.
    03/10/11 VladimirJ SCR 82485 - fix previous change: substitute should have only 9 params.
    03/10/11 JuliaK    SCR 80239 - performance, fileds () + first for checking on duplicate in isvalidheader
    03/14/11 CCisneros SCR 83602 - Added missing xlate call
    04/12/11 VladimirD SCR 84492 - Corrected EAD validation for General Journal.
    04/15/11 VladimirD SCR 81469 - Fixed Asset transaction type has been added to exclude validation list.
      04/21/11 VladimirD SCR 83521 - Corrected account validation logic. GlobLevel_AccInactive.
    04/21/11 amiletin  SCR 83632 - Payroll Check transaction type has been added to exclude validation list.
    09/30/11 andreaP  scr 87946 - fixed tablename reference as it wasn't converting well to ICE3.
    11/08/11 TatyanaK SCR 82323 - Behaviour of the rule CurNotConformCurAccDef_Err was corrected.
    11/22/11 TatyanaK SCR 83844 - "AP PI Write-off" and "AR PI Write-off" were allowed to use TranGLC duplicates.
    12/07/11 TatyanaK SCR 84442 - Logic modifying GLAcctDisp was corrected.
    02/29/12 VladimirD SCR 93972 - Corrected ref segments validation logic.    
    03/05/12 VladimirD SCR 94304 - Exception.i include file has been added
    05/18/12 RonM      SCR 69204 - Modify fpClosingPeriodNotExists to not report warning message on Autocorrect.
    06/01/12 TatyanaK  SCR 95528 - Reversed lines are not validated by fpClosingPeriodNotExists rule.
  ----------------------------------------------------------------------*/
/*          This .W file was created with the Progress AppBuilder.      */
/*----------------------------------------------------------------------*/
/* ***************************  Definitions  ************************** */
/* ************************ Data access layer ************************* */
#endregion 4gl comments
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Epicor.Data;
using Epicor.Utilities;
using Erp.Internal.Lib.Utils;
using Erp.Tables;
using Ice;
using Ice.Lib;
using Strings = Erp.Internal.PE.Resources.Strings;

namespace Erp.Internal.PE
{

    public enum LogicalExtended
    {
        Undefined,
        Yes,
        No
    }

    public partial class ValidationRule : Ice.Libraries.ContextLibraryBase<ErpContext>
    {
        const bool DB_AWARE = false;
        private List<GLAccountWithJEDate> RetainedEarningsAccountRows;

        private bool gloTransactionCurrIsSpecified = false;
        private bool gloTransactionAccIsCurrencyAcc = false;

        private LogicalExtended gloIsLimitedTransaction = LogicalExtended.Undefined;
        private class iLine : IRow
        {
            public string BookID;
            public int JournalLine;

            public object this[string columnName]
            {
                get
                {
                    switch (columnName.ToUpperInvariant())
                    {
                        case "BOOKID":
                            return BookID;


                    };
                    return JournalLine;
                }

                set
                {
                    switch (columnName.ToUpperInvariant())
                    {
                        case "BOOKID":
                            BookID = (string)value;
                            break;
                        case "JOURNALLINE":
                            JournalLine = (int)value;
                            break;
                        default:
                            break;
                    };
                }
            }
        }

        public bool gloRevaluationTran = false;

        public PEData _PEData;


        public class GLAccountWithJEDate : GLAccount, IGLAccount
        {
            public DateTime? JEDate { get; set; }
            public string BookID { get; set; }
            public int JournalLine { get; set; }
        }


        private Lazy<UDFieldCheckHelper> libUDFieldCheckHelper;
        public UDFieldCheckHelper UDFieldCheckHelper
        {
            get { return libUDFieldCheckHelper.Value; }
        }


        public Dictionary<int, Dictionary<int, VR_ACTION>> BVRuleCache = new Dictionary<int, Dictionary<int, VR_ACTION>>();

        private Dictionary<string, Dictionary<int, VR_ACTION>> BVBookCache = new Dictionary<string, Dictionary<int, VR_ACTION>>(StringComparer.InvariantCultureIgnoreCase);

        Lazy<Erp.Internal.PE.BVRule> libBVRule;
        Erp.Internal.PE.BVRule LibBVRule
        {
            get { return this.libBVRule.Value; }
        }

        Lazy<Erp.Internal.GL.GetREAccount> libGetREAccount;
        Erp.Internal.GL.GetREAccount LibGetREAccount
        {
            get { return libGetREAccount.Value; }
        }

        private Lazy<Erp.Internal.Lib.PlantAuthorized> libPlant;
        private Erp.Internal.Lib.PlantAuthorized LibPlant
        {
            get
            {
                return this.libPlant.Value;
            }
        }

        private bool pedatacreated;
        private bool isVietnamLocalization = false;

        public ValidationRule(ErpContext ctx)
            : base(ctx)
        {
            this.Initialize();
            this._PEData = new PEData(Db);
            pedatacreated = true;
            _PEData.PEError = new PEError(ctx);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (libBVRule != null && libBVRule.IsValueCreated) libBVRule.Value.Dispose();
                if (libGetREAccount != null && libGetREAccount.IsValueCreated) libGetREAccount.Value.Dispose();
                if (RetainedEarningsAccountRows != null) RetainedEarningsAccountRows.Clear();
                if (_PEData != null && pedatacreated) _PEData.Dispose();
                if (libUDFieldCheckHelper != null && libUDFieldCheckHelper.IsValueCreated) libUDFieldCheckHelper.Value.Dispose();
                if (libPlant != null && libPlant.IsValueCreated) libPlant.Value.Dispose();
            }
        }
        public ValidationRule(ErpContext ctx, PEData peData)
            : base(ctx)
        {
            this.Initialize();
            this._PEData = peData;
            this.UDFieldCheckHelper.Init(this._PEData.TransactionTypeName);
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.RetainedEarningsAccountRows = new List<GLAccountWithJEDate>();
            this.libBVRule = new Lazy<Erp.Internal.PE.BVRule>(() => new Erp.Internal.PE.BVRule(Db));
            this.libGetREAccount = new Lazy<GL.GetREAccount>(() => new GL.GetREAccount(this.Db));
            this.libUDFieldCheckHelper = new Lazy<UDFieldCheckHelper>(() => new UDFieldCheckHelper(this.Db));
            this.libPlant = new Lazy<Internal.Lib.PlantAuthorized>(() => new Internal.Lib.PlantAuthorized(Db));
            isVietnamLocalization = string.Equals(Session.CountryCode, "VN", StringComparison.OrdinalIgnoreCase);
        }

        private void SiteSegmentBalancing(string inBookID, string sourcePlant)
        {
            XbSyst ttXbSyst = FindFirstXbSyst(Session.CompanyID);
            const string siteGLControlType = "MultiSite";

            if (ttXbSyst.SiteIsLegalEntity && !String.IsNullOrEmpty(sourcePlant))
            {
                var ttprocRvJrnTrDtl = this._PEData.ProcRvJrnTrDtlRows.GetItemsByIndex(0, inBookID).OrderByDescending(r => r.JournalLine).FirstOrDefault();
                if (ttprocRvJrnTrDtl != null)
                {
                    int iJournalLine = ttprocRvJrnTrDtl.JournalLine;
                    COASegment siteSegment = FindFirstCOASiteSegment(ttprocRvJrnTrDtl.Company, ttprocRvJrnTrDtl.COACode);

                    if (siteSegment != null)
                    {
                        #region Create unique combination of self-balancing segments
                        var s_list = this._PEData.ProcRvJrnTrDtlRows.GetItemsByIndex(inBookID);
                        List<procRvJrnTrDtl> uniqueCombinations = new List<procRvJrnTrDtl>();
                        bool toAdd = false;

                        foreach (procRvJrnTrDtl src in s_list)
                        {
                            // Fill self-balanced segments with the values from journal line
                            string segValColumn = "SegValue" + siteSegment.SegmentNbr.ToString();
                            var balanceCombination = Tuple.Create(segValColumn, src[segValColumn].ToString(), src["CurrencyCode"].ToString());

                            var coaSegValues = FindFirstCOASegValues(ttprocRvJrnTrDtl.Company, ttprocRvJrnTrDtl.COACode, siteSegment.SegmentNbr, src[segValColumn].ToString());
                            if (coaSegValues != null && coaSegValues.LinkedPlant.KeyEquals(sourcePlant))

                            {
                                continue;
                            }

                            // Test if journal line's segment combination is unique
                            toAdd = false;
                            if (uniqueCombinations.Count == 0)
                            {
                                toAdd = true;
                            }
                            else
                            {
                                bool found = false;
                                foreach (procRvJrnTrDtl uc in uniqueCombinations)
                                {
                                    if (uc[balanceCombination.Item1].ToString().KeyEquals(balanceCombination.Item2) && uc["CurrencyCode"].ToString().KeyEquals(balanceCombination.Item3))
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                toAdd = (!found);
                            }

                            if (toAdd)
                            {
                                // Add the record with the unique combinations of self-balanced segments
                                procRvJrnTrDtl target = new procRvJrnTrDtl();

                                BufferCopy.CopyExceptFor(src, target, procRvJrnTrDtl.ColumnNames.SysRowID, procRvJrnTrDtl.ColumnNames.IsSummarize, procRvJrnTrDtl.ColumnNames.TranSeq, procRvJrnTrDtl.ColumnNames.RvJrnTrDtlUID, procRvJrnTrDtl.ColumnNames.CurrencyCode,
                                        procRvJrnTrDtl.ColumnNames.SegValue1, procRvJrnTrDtl.ColumnNames.SegValue2, procRvJrnTrDtl.ColumnNames.SegValue3, procRvJrnTrDtl.ColumnNames.SegValue4, procRvJrnTrDtl.ColumnNames.SegValue5,
                                        procRvJrnTrDtl.ColumnNames.SegValue6, procRvJrnTrDtl.ColumnNames.SegValue7, procRvJrnTrDtl.ColumnNames.SegValue8, procRvJrnTrDtl.ColumnNames.SegValue9, procRvJrnTrDtl.ColumnNames.SegValue10,
                                        procRvJrnTrDtl.ColumnNames.SegValue11, procRvJrnTrDtl.ColumnNames.SegValue12, procRvJrnTrDtl.ColumnNames.SegValue13, procRvJrnTrDtl.ColumnNames.SegValue14, procRvJrnTrDtl.ColumnNames.SegValue15,
                                        procRvJrnTrDtl.ColumnNames.SegValue16, procRvJrnTrDtl.ColumnNames.SegValue17, procRvJrnTrDtl.ColumnNames.SegValue18, procRvJrnTrDtl.ColumnNames.SegValue19, procRvJrnTrDtl.ColumnNames.SegValue20,
                                        procRvJrnTrDtl.ColumnNames.ExtCompanyID, procRvJrnTrDtl.ColumnNames.ExtRefType, procRvJrnTrDtl.ColumnNames.ExtRefCode,
                                        procRvJrnTrDtl.ColumnNames.ExtSegValue1, procRvJrnTrDtl.ColumnNames.ExtSegValue2, procRvJrnTrDtl.ColumnNames.ExtSegValue3, procRvJrnTrDtl.ColumnNames.ExtSegValue4, procRvJrnTrDtl.ColumnNames.ExtSegValue5,
                                        procRvJrnTrDtl.ColumnNames.ExtSegValue6, procRvJrnTrDtl.ColumnNames.ExtSegValue7, procRvJrnTrDtl.ColumnNames.ExtSegValue8, procRvJrnTrDtl.ColumnNames.ExtSegValue9, procRvJrnTrDtl.ColumnNames.ExtSegValue10,
                                        procRvJrnTrDtl.ColumnNames.ExtSegValue11, procRvJrnTrDtl.ColumnNames.ExtSegValue12, procRvJrnTrDtl.ColumnNames.ExtSegValue13, procRvJrnTrDtl.ColumnNames.ExtSegValue14, procRvJrnTrDtl.ColumnNames.ExtSegValue15,
                                        procRvJrnTrDtl.ColumnNames.ExtSegValue16, procRvJrnTrDtl.ColumnNames.ExtSegValue17, procRvJrnTrDtl.ColumnNames.ExtSegValue18, procRvJrnTrDtl.ColumnNames.ExtSegValue19, procRvJrnTrDtl.ColumnNames.ExtSegValue20,
                                        procRvJrnTrDtl.ColumnNames.ExtGLAccount, procRvJrnTrDtl.ColumnNames.ExtCOACode,
                                        procRvJrnTrDtl.ColumnNames.DebitStatAmt, procRvJrnTrDtl.ColumnNames.CreditStatAmt, procRvJrnTrDtl.ColumnNames.StatUOMCode, procRvJrnTrDtl.ColumnNames.Statistical);

                                target[balanceCombination.Item1] = src[balanceCombination.Item1];
                                target["CurrencyCode"] = src["CurrencyCode"];

                                uniqueCombinations.Add(target);
                            }
                            else
                            {
                                // Update the record with the unique combinations of self-balanced segments
                                foreach (procRvJrnTrDtl uc in uniqueCombinations)
                                {
                                    if (!uc[balanceCombination.Item1].ToString().KeyEquals(balanceCombination.Item2) || !uc["CurrencyCode"].ToString().KeyEquals(balanceCombination.Item3))
                                    {
                                        continue;
                                    }

                                    uc.BookDebitAmount += src.BookDebitAmount;
                                    uc.BookCreditAmount += src.BookCreditAmount;
                                    uc.DebitAmount += src.DebitAmount;
                                    uc.CreditAmount += src.CreditAmount;
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region Decide if balancing transactions have to be created
                        bool createBalTransactions = false;
                        if (uniqueCombinations.Count > 0)
                            createBalTransactions = uniqueCombinations.Exists(r => (r.BookDebitAmount - r.BookCreditAmount) != 0);

                        if (!createBalTransactions)
                            return;
                        #endregion Decide if balancing transactions have to be created

                        #region Create balancing transactions

                        foreach (procRvJrnTrDtl uc in uniqueCombinations)
                        {
                            //Do not process records where balancing amount is zero
                            decimal dTotalAmount = uc.BookDebitAmount - uc.BookCreditAmount;
                            decimal dDocTotalAmount = uc.DebitAmount - uc.CreditAmount;
                            if (dTotalAmount != 0)
                            {
                                string sourceSegValue = string.Empty;

                                var COASegValues = FindFirstCOASegmentValueLinkedPlant(Session.CompanyID, uc.COACode, siteSegment.SegmentNbr, uc.SourcePlant);
                                if (COASegValues != null)
                                {
                                    sourceSegValue = COASegValues.SegmentCode;
                                }

                                const string dueFromContext = "Due From";
                                const string dueToContext = "Due To";

                                string glControlCode = string.Empty;

                                // Define Inter-Site To/From account that should be used for balancing transaction
                                uc.GLAccount = String.Empty;

                                var coaSegValue = FindFirstCOASegValues(Session.CompanyID, uc.COACode, siteSegment.SegmentNbr, uc["SegValue" + siteSegment.SegmentNbr].ToString());
                                if (coaSegValue != null && !String.IsNullOrEmpty(coaSegValue.LinkedPlant))
                                {
                                    glControlCode = FindFirstEntityGLC(Session.CompanyID, "", inBookID, sourcePlant, coaSegValue.LinkedPlant, siteGLControlType);
                                    if (!String.IsNullOrEmpty(glControlCode))
                                    {
                                        var GLCntrlAcctrow = FindFirstGLCntrlAcct(Session.CompanyID, siteGLControlType, glControlCode, inBookID, dueToContext);
                                        if (!String.IsNullOrEmpty(GLCntrlAcctrow.GLAccount))
                                        {
                                            BufferCopy.Copy(GLCntrlAcctrow, uc);
                                        }
                                    }
                                }

                                // Fill other fields                                
                                uc.IsSummarize = false;
                                uc.TranSeq = -1; // Balancing transaction indication (used in Reconciliation report)
                                iJournalLine = iJournalLine + 1;
                                uc.JournalLine = iJournalLine;
                                uc.Description = Strings.InterSiteBalancingtransaction;
                                uc.BRuleUID = 0;
                                uc.BookDebitAmount = 0;
                                uc.BookCreditAmount = 0;
                                uc.DebitAmount = 0;
                                uc.CreditAmount = 0;
                                if (!uc.RedStorno)
                                {
                                    if (dTotalAmount > 0)
                                    {
                                        uc.BookCreditAmount = dTotalAmount;
                                        uc.CreditAmount = dDocTotalAmount;
                                    }
                                    else
                                    {
                                        uc.BookDebitAmount = -dTotalAmount;
                                        uc.DebitAmount = -dDocTotalAmount;
                                    }
                                }
                                else
                                {
                                    if (dTotalAmount < 0)
                                    {
                                        uc.BookCreditAmount = dTotalAmount;
                                        uc.CreditAmount = dDocTotalAmount;
                                    }
                                    else
                                    {
                                        uc.BookDebitAmount = -dTotalAmount;
                                        uc.DebitAmount = -dDocTotalAmount;
                                    }
                                }
                                _PEData.ProcRvJrnTrDtlRows.Add(uc);

                                // create balancing transaction
                                var ucBalancingTran = new procRvJrnTrDtl();

                                BufferCopy.CopyExceptFor(uc, ucBalancingTran, procRvJrnTrDtl.ColumnNames.SysRowID, procRvJrnTrDtl.ColumnNames.IsSummarize, procRvJrnTrDtl.ColumnNames.TranSeq, procRvJrnTrDtl.ColumnNames.RvJrnTrDtlUID, procRvJrnTrDtl.ColumnNames.CurrencyCode,
                                        procRvJrnTrDtl.ColumnNames.SegValue1, procRvJrnTrDtl.ColumnNames.SegValue2, procRvJrnTrDtl.ColumnNames.SegValue3, procRvJrnTrDtl.ColumnNames.SegValue4, procRvJrnTrDtl.ColumnNames.SegValue5,
                                        procRvJrnTrDtl.ColumnNames.SegValue6, procRvJrnTrDtl.ColumnNames.SegValue7, procRvJrnTrDtl.ColumnNames.SegValue8, procRvJrnTrDtl.ColumnNames.SegValue9, procRvJrnTrDtl.ColumnNames.SegValue10,
                                        procRvJrnTrDtl.ColumnNames.SegValue11, procRvJrnTrDtl.ColumnNames.SegValue12, procRvJrnTrDtl.ColumnNames.SegValue13, procRvJrnTrDtl.ColumnNames.SegValue14, procRvJrnTrDtl.ColumnNames.SegValue15,
                                        procRvJrnTrDtl.ColumnNames.SegValue16, procRvJrnTrDtl.ColumnNames.SegValue17, procRvJrnTrDtl.ColumnNames.SegValue18, procRvJrnTrDtl.ColumnNames.SegValue19, procRvJrnTrDtl.ColumnNames.SegValue20,
                                        procRvJrnTrDtl.ColumnNames.ExtCompanyID, procRvJrnTrDtl.ColumnNames.ExtRefType, procRvJrnTrDtl.ColumnNames.ExtRefCode,
                                        procRvJrnTrDtl.ColumnNames.ExtSegValue1, procRvJrnTrDtl.ColumnNames.ExtSegValue2, procRvJrnTrDtl.ColumnNames.ExtSegValue3, procRvJrnTrDtl.ColumnNames.ExtSegValue4, procRvJrnTrDtl.ColumnNames.ExtSegValue5,
                                        procRvJrnTrDtl.ColumnNames.ExtSegValue6, procRvJrnTrDtl.ColumnNames.ExtSegValue7, procRvJrnTrDtl.ColumnNames.ExtSegValue8, procRvJrnTrDtl.ColumnNames.ExtSegValue9, procRvJrnTrDtl.ColumnNames.ExtSegValue10,
                                        procRvJrnTrDtl.ColumnNames.ExtSegValue11, procRvJrnTrDtl.ColumnNames.ExtSegValue12, procRvJrnTrDtl.ColumnNames.ExtSegValue13, procRvJrnTrDtl.ColumnNames.ExtSegValue14, procRvJrnTrDtl.ColumnNames.ExtSegValue15,
                                        procRvJrnTrDtl.ColumnNames.ExtSegValue16, procRvJrnTrDtl.ColumnNames.ExtSegValue17, procRvJrnTrDtl.ColumnNames.ExtSegValue18, procRvJrnTrDtl.ColumnNames.ExtSegValue19, procRvJrnTrDtl.ColumnNames.ExtSegValue20,
                                        procRvJrnTrDtl.ColumnNames.ExtGLAccount, procRvJrnTrDtl.ColumnNames.ExtCOACode,
                                        procRvJrnTrDtl.ColumnNames.DebitStatAmt, procRvJrnTrDtl.ColumnNames.CreditStatAmt, procRvJrnTrDtl.ColumnNames.StatUOMCode, procRvJrnTrDtl.ColumnNames.Statistical);

                                ucBalancingTran.IsSummarize = false;
                                ucBalancingTran.TranSeq = -1; // Balancing transaction indication (used in Reconciliation report)
                                iJournalLine = iJournalLine + 1;
                                ucBalancingTran.JournalLine = iJournalLine;
                                ucBalancingTran.Description = Strings.InterSiteBalancingtransaction;
                                ucBalancingTran.BRuleUID = 0;
                                ucBalancingTran.BookDebitAmount = 0;
                                ucBalancingTran.BookCreditAmount = 0;
                                ucBalancingTran.DebitAmount = 0;
                                ucBalancingTran.CreditAmount = 0;
                                ucBalancingTran.CurrencyCode = uc.CurrencyCode;
                                ucBalancingTran.SourcePlant = sourcePlant;
                                ucBalancingTran.CreditAmount = uc.DebitAmount;
                                ucBalancingTran.BookCreditAmount = uc.BookDebitAmount;
                                ucBalancingTran.DebitAmount = uc.CreditAmount;
                                ucBalancingTran.BookDebitAmount = uc.BookCreditAmount;

                                if (!String.IsNullOrEmpty(glControlCode))
                                {
                                    var GLCntrlAcctrow = FindFirstGLCntrlAcct(Session.CompanyID, siteGLControlType, glControlCode, inBookID, dueFromContext);
                                    if (!String.IsNullOrEmpty(GLCntrlAcctrow.GLAccount))
                                    {
                                        BufferCopy.Copy(GLCntrlAcctrow, ucBalancingTran);
                                    }
                                }

                                _PEData.ProcRvJrnTrDtlRows.Add(ucBalancingTran);
                            }
                        }
                        #endregion Create Balance Transactions
                    }
                }
            }
        }

        public void BalTransactionsLevel(string cCOA, VR_ACTION cAction, string ERROR_MESSAGE, string inBookID, out bool isValid)
        {
            isValid = true;
            var ttprocRvJrnTrDtl = this._PEData.ProcRvJrnTrDtlRows.GetItemsByIndex(0, inBookID).OrderByDescending(r => r.JournalLine).FirstOrDefault();
            if (ttprocRvJrnTrDtl != null)
            {
                int iJournalLine = ttprocRvJrnTrDtl.JournalLine;
                int selfBalVNACGroup = 0;
                int selfBalVNACType = 0;
                if (isVietnamLocalization)
                {
                    selfBalVNACGroup = this._PEData.ProcRvJrnTrDtlRows.GetItemsByIndex(0, inBookID).Max(r => (int)r["VNACGroup"]) + 1;
                    selfBalVNACType = 1;
                }

                // 1. Retrieve self-balancing setup info 
                IEnumerable<COASegment> coaSegsSelfBal = SelectCOASegmentSBSetup(Session.CompanyID, cCOA);

                // 2. System cycles through all journal lines and calculates the balance for each unique combination of self-balancing segments.
                //    Empty values for the optional segments are also included in the combinations.
                #region Create unique combination of self-balancing segments
                var s_list = this._PEData.ProcRvJrnTrDtlRows.GetItemsByIndex(0, inBookID, cCOA);

                List<procRvJrnTrDtl> uniqueCombinations = new List<procRvJrnTrDtl>();
                bool toAdd = false;

                foreach (procRvJrnTrDtl src in s_list)
                {
                    // Fill self-balanced segments with the values from journal line
                    Dictionary<string, string> segments = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                    foreach (COASegment coaSegSelfBal in coaSegsSelfBal)
                    {
                        string segValColumn = "SegValue" + coaSegSelfBal.SegmentNbr.ToString();
                        segments.Add(segValColumn, src[segValColumn].ToString());
                    }

                    // Test if journal line's segment combination is unique
                    toAdd = false;
                    if (uniqueCombinations.Count == 0)
                    {
                        toAdd = true;
                    }
                    else
                    {
                        bool found = false;
                        foreach (procRvJrnTrDtl uc in uniqueCombinations)
                        {
                            bool segMatch = true;
                            foreach (KeyValuePair<string, string> segment in segments)
                            {
                                if (!uc[segment.Key].ToString().KeyEquals(segment.Value.ToString()))
                                {
                                    segMatch = false;
                                    break;
                                }
                            }

                            found = segMatch;
                            if (found)
                                break;
                        }
                        toAdd = (!found);
                    }

                    if (toAdd)
                    {
                        // Add the record with the unique combinations of self-balanced segments
                        procRvJrnTrDtl target = new procRvJrnTrDtl();

                        BufferCopy.CopyExceptFor(src, target, procRvJrnTrDtl.ColumnNames.SysRowID, procRvJrnTrDtl.ColumnNames.IsSummarize, procRvJrnTrDtl.ColumnNames.TranSeq, procRvJrnTrDtl.ColumnNames.RvJrnTrDtlUID, procRvJrnTrDtl.ColumnNames.CurrencyCode,
                                procRvJrnTrDtl.ColumnNames.SegValue1, procRvJrnTrDtl.ColumnNames.SegValue2, procRvJrnTrDtl.ColumnNames.SegValue3, procRvJrnTrDtl.ColumnNames.SegValue4, procRvJrnTrDtl.ColumnNames.SegValue5,
                                procRvJrnTrDtl.ColumnNames.SegValue6, procRvJrnTrDtl.ColumnNames.SegValue7, procRvJrnTrDtl.ColumnNames.SegValue8, procRvJrnTrDtl.ColumnNames.SegValue9, procRvJrnTrDtl.ColumnNames.SegValue10,
                                procRvJrnTrDtl.ColumnNames.SegValue11, procRvJrnTrDtl.ColumnNames.SegValue12, procRvJrnTrDtl.ColumnNames.SegValue13, procRvJrnTrDtl.ColumnNames.SegValue14, procRvJrnTrDtl.ColumnNames.SegValue15,
                                procRvJrnTrDtl.ColumnNames.SegValue16, procRvJrnTrDtl.ColumnNames.SegValue17, procRvJrnTrDtl.ColumnNames.SegValue18, procRvJrnTrDtl.ColumnNames.SegValue19, procRvJrnTrDtl.ColumnNames.SegValue20,
                                procRvJrnTrDtl.ColumnNames.ExtCompanyID, procRvJrnTrDtl.ColumnNames.ExtRefType, procRvJrnTrDtl.ColumnNames.ExtRefCode,
                                procRvJrnTrDtl.ColumnNames.ExtSegValue1, procRvJrnTrDtl.ColumnNames.ExtSegValue2, procRvJrnTrDtl.ColumnNames.ExtSegValue3, procRvJrnTrDtl.ColumnNames.ExtSegValue4, procRvJrnTrDtl.ColumnNames.ExtSegValue5,
                                procRvJrnTrDtl.ColumnNames.ExtSegValue6, procRvJrnTrDtl.ColumnNames.ExtSegValue7, procRvJrnTrDtl.ColumnNames.ExtSegValue8, procRvJrnTrDtl.ColumnNames.ExtSegValue9, procRvJrnTrDtl.ColumnNames.ExtSegValue10,
                                procRvJrnTrDtl.ColumnNames.ExtSegValue11, procRvJrnTrDtl.ColumnNames.ExtSegValue12, procRvJrnTrDtl.ColumnNames.ExtSegValue13, procRvJrnTrDtl.ColumnNames.ExtSegValue14, procRvJrnTrDtl.ColumnNames.ExtSegValue15,
                                procRvJrnTrDtl.ColumnNames.ExtSegValue16, procRvJrnTrDtl.ColumnNames.ExtSegValue17, procRvJrnTrDtl.ColumnNames.ExtSegValue18, procRvJrnTrDtl.ColumnNames.ExtSegValue19, procRvJrnTrDtl.ColumnNames.ExtSegValue20,
                                procRvJrnTrDtl.ColumnNames.ExtGLAccount, procRvJrnTrDtl.ColumnNames.ExtCOACode,
                                procRvJrnTrDtl.ColumnNames.DebitStatAmt, procRvJrnTrDtl.ColumnNames.CreditStatAmt, procRvJrnTrDtl.ColumnNames.StatUOMCode, procRvJrnTrDtl.ColumnNames.Statistical);

                        foreach (KeyValuePair<string, string> segment in segments)
                        {
                            target[segment.Key] = src[segment.Key];
                        }
                        uniqueCombinations.Add(target);
                    }
                    else
                    {
                        // Update the record with the unique combinations of self-balanced segments
                        foreach (procRvJrnTrDtl uc in uniqueCombinations)
                        {
                            bool segMatch = true;
                            foreach (KeyValuePair<string, string> segment in segments)
                            {
                                if (!uc[segment.Key].ToString().KeyEquals(segment.Value.ToString()))
                                {
                                    segMatch = false;
                                    break;
                                }
                            }

                            if (segMatch)
                            {
                                uc.BookDebitAmount += src.BookDebitAmount;
                                uc.BookCreditAmount += src.BookCreditAmount;
                                break;
                            }
                        }

                    }
                }
                #endregion Create unique combination of self-balancing segments

                // 3. Decide if balancing transactions have to be created
                #region Decide if balancing transactions have to be created
                bool createBalTransactions = false;
                if (uniqueCombinations.Count > 0)
                    createBalTransactions = uniqueCombinations.Exists(r => (r.BookDebitAmount - r.BookCreditAmount) != 0);

                if (createBalTransactions)
                {
                    string segmentNames = string.Join(", ", coaSegsSelfBal.Select(c => c.SegmentName));

                    if (cAction == VR_ACTION.Error)
                    {
                        isValid = false;
                        this._PEData.PEError.AddException(new RuleException(String.Format(ERROR_MESSAGE, segmentNames), inBookID), true);
                        return;
                    }
                    if (cAction == VR_ACTION.Warning)
                    {
                        isValid = true;
                        this._PEData.PEError.AddWarning(new RuleException(String.Format(ERROR_MESSAGE, segmentNames), inBookID));
                        return;
                    }
                }
                else
                    return;
                #endregion Decide if balancing transactions have to be created

                // 4. Create balancing transactions
                #region Create balancing transactions

                //Create list with self-balance segment numbers
                List<int> segNbrs = new List<int>();
                foreach (COASegment coaSegSelfBal in coaSegsSelfBal)
                    segNbrs.Add(coaSegSelfBal.SegmentNbr);
                string balancingCurr = FindFirstGLBookCurr(Session.CompanyID, inBookID);

                foreach (procRvJrnTrDtl uc in uniqueCombinations)
                {
                    //Do not process records where balancing amount is zero
                    decimal dTotalAmount = uc.BookDebitAmount - uc.BookCreditAmount;
                    if (dTotalAmount != 0)
                    {
                        // Define if balancing or offset account will be used.
                        bool processOffset = false;
                        foreach (COASegment coaSegSelfBal in coaSegsSelfBal)
                        {
                            string segValColumn = string.Format("SegValue{0}", coaSegSelfBal.SegmentNbr.ToString());
                            if (string.IsNullOrEmpty(uc[segValColumn].ToString()))
                            {
                                processOffset = true;
                                break;
                            }
                        }

                        string setupSegmentFld = string.Empty;
                        COASegment lvl;

                        if (processOffset)
                        {
                            // For each combination where one or more segments are missing the system takes offset account for a missing segment with the lowest level.
                            // In the offset account the self - balancing segments are overridden with segments from the unique combination.
                            setupSegmentFld = "OffSegValue";
                            int level = 0;
                            foreach (COASegment coaSegSelfBal in coaSegsSelfBal)
                            {
                                string segValColumn = string.Format("SegValue{0}", coaSegSelfBal.SegmentNbr.ToString());
                                if (string.IsNullOrEmpty(uc[segValColumn].ToString()))
                                {
                                    if (coaSegSelfBal.Level > level)
                                        level = coaSegSelfBal.Level;
                                }
                            }
                            lvl = coaSegsSelfBal.First(r => r.Level == level);
                        }
                        else
                        {
                            // A combination where all self-balancing segments are present receives account from the last level 
                            // where all self-balancing segments are replaced with the segment values from the selected combination.
                            setupSegmentFld = "BalSegValue";

                            // Sort self-balance segments by level and select the last
                            IEnumerable<COASegment> lvls = coaSegsSelfBal.OrderBy(r => r.Level);
                            lvl = lvls.Last();
                        }

                        // Compose account from segments
                        string sAccount = string.Empty;
                        string sep = PEData.ACC_SEPINTERNAL.ToString();
                        for (int i = 20; i > 0; i--)
                        {
                            string srcFld = string.Format("{0}{1}", setupSegmentFld, i.ToString());
                            string trgFld = string.Format("SegValue{0}", i.ToString());
                            if (!segNbrs.Exists(x => x == i))
                            {
                                uc[trgFld] = lvl[srcFld];
                            }

                            string segValue = uc[trgFld].ToString();
                            if (string.IsNullOrEmpty(segValue))
                            {
                                if (!string.IsNullOrEmpty(sAccount))
                                    sAccount = string.Format("{0}{1}", sep, sAccount);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(sAccount))
                                    sAccount = segValue;
                                else
                                    sAccount = string.Format("{0}{1}{2}", segValue, sep, sAccount);
                            }

                        }

                        // Fill other fields
                        uc.GLAccount = sAccount;
                        uc.IsSummarize = false;
                        uc.TranSeq = -1; // Balancing transaction indication (used in Reconciliation report)
                        iJournalLine = iJournalLine + 1;
                        uc.JournalLine = iJournalLine;
                        uc.Description = string.Format("Balancing transaction. Level - {0}", lvl.Level.ToString());
                        uc.BRuleUID = 0;
                        uc.CurrencyCode = balancingCurr;

                        uc.BookDebitAmount = 0;
                        uc.BookCreditAmount = 0;
                        uc.DebitAmount = 0;
                        uc.CreditAmount = 0;
                        if (!uc.RedStorno)
                        {
                            if (dTotalAmount > 0)
                                uc.BookCreditAmount = dTotalAmount;
                            else
                                uc.BookDebitAmount = -dTotalAmount;
                        }
                        else
                        {
                            if (dTotalAmount < 0)
                                uc.BookCreditAmount = dTotalAmount;
                            else
                                uc.BookDebitAmount = -dTotalAmount;
                        }
                        uc.DebitAmount = uc.BookDebitAmount;
                        uc.CreditAmount = uc.BookCreditAmount;

                        if (isVietnamLocalization)
                        {
                            uc.SetUDField<int>("VNACGroup", selfBalVNACGroup);
                            uc.SetUDField<int>("VNACType", selfBalVNACType);
                            selfBalVNACType = 0;
                        }
                        if (_PEData.TransactionTypeName.KeyEquals("COSAndWIP"))
                            uc.LegalNumber = string.Empty;

                        _PEData.ProcRvJrnTrDtlRows.Add(uc);
                    }

                }
                #endregion Create balancing transactions
            }
        }

        /*------------------------------------------------------------------------------
          Purpose:  Account is inactive  
            Notes:  validation type = GlobLevel_AccInactive  
        ------------------------------------------------------------------------------*/
        public bool AccInactive_Err(string errMess, IRow hHandle)
        {
            bool lResult = true;
            bool lActive = true;

            string cacheIndex = GetAccountCacheIndex(hHandle, "AccInactive_Err");
            bool isValid = true;

            if (GetAccountCacheData(cacheIndex, out isValid, hHandle)) return isValid;
            AddBlankToCache(cacheIndex);

            VR_ACTION action = GetGlobLevelAction(ValidationType.GlobLevel_AccInactive);

            if (action != VR_ACTION.Ignore)
            {
                lActive = AccountIsActive(Compatibility.Convert.ToString(getGLAccount(hHandle)), Compatibility.Convert.ToString(hHandle["COACode"]), Compatibility.Convert.ToString(hHandle["Company"]), (DateTime?)hHandle["JEDate"]);
                if (lActive == false)
                {
                    errMess = errMess + String.Format(" Company {0}.  COACode {1}. Account {2}. Date {3}", hHandle["Company"], hHandle["COACode"], getGLAccount(hHandle), ((DateTime)hHandle["JEDate"]).Date.ToShortDateString());
                    switch (action)
                    {
                        case VR_ACTION.Error:
                            {
                                lResult = false;
                                this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle), Compatibility.Convert.ToString(hHandle["BookID"])), true);
                                AddErrorToCache(cacheIndex, errMess, true, Compatibility.Convert.ToString(hHandle["BookID"]));
                            }
                            break;
                        case VR_ACTION.Warning:
                            {
                                this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle), Compatibility.Convert.ToString(hHandle["BookID"])));
                                AddErrorToCache(cacheIndex, errMess, false, Compatibility.Convert.ToString(hHandle["BookID"]));
                            }
                            break;
                    }
                }
            }
            return lResult;
        }
        /*------------------------------------------------------------------------------
          Purpose:  Transaction currency is specified, but account is not defined as currency account  
            Notes:  validation type = BookLevel_AccNotDefinedAsCurAcc
        ------------------------------------------------------------------------------*/
        public bool AccNotDefinedAsCurAcc_Err(string errMess, IRow inRow)
        {
            bool lResult = true;
            VR_ACTION action = GetBookLevelAction(ValidationType.BookLevel_AccNotDefinedAsCurAcc, inRow["BookID"].ToString());
            if ((action != VR_ACTION.Ignore) && (gloTransactionCurrIsSpecified))
            {
                if (!gloTransactionAccIsCurrencyAcc)
                {
                    switch (action)
                    {                      /* Error */
                        case VR_ACTION.Error:
                            {
                                this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, inRow), inRow["BookID"].ToString()), true);
                                lResult = false;
                            }
                            break;                     /* Warning */
                        case VR_ACTION.Warning:
                            {
                                this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, inRow), inRow["BookID"].ToString()));
                            }
                            break;
                    }
                }
            }
            return lResult;
        }

        /*------------------------------------------------------------------------------
          Purpose:  User authorized in the Site  
            Notes:  validation type = GlobLevel_AuthorizedSite  
        ------------------------------------------------------------------------------*/
        public bool AccountAuthorizedSite_Err(string errMess, IRow hHandle)
        {
            bool lResult = true;

            string cacheIndex = GetAccountCacheIndex(hHandle, "AccountAuthorizedSite_Err");
            bool isValid = true;

            if (GetAccountCacheData(cacheIndex, out isValid, hHandle)) return isValid;
            AddBlankToCache(cacheIndex);

            VR_ACTION action = GetGlobLevelAction(ValidationType.GlobLevel_AuthorizedSite);

            if (action != VR_ACTION.Ignore)
            {
                XbSyst ttXbSyst = FindFirstXbSyst(Session.CompanyID);
                if (ttXbSyst.SiteIsLegalEntity)
                {
                    COASegment siteSegment = FindFirstCOASiteSegment(Compatibility.Convert.ToString(hHandle["Company"]), Compatibility.Convert.ToString(hHandle["COACode"]));
                    if (siteSegment != null && !String.IsNullOrEmpty(Compatibility.Convert.ToString(hHandle["SegValue" + siteSegment.SegmentNbr])))
                    {
                        COASegValues cOASegValues = FindFirstCOASegValues(Compatibility.Convert.ToString(hHandle["Company"]), Compatibility.Convert.ToString(hHandle["COACode"]), siteSegment.SegmentNbr, Compatibility.Convert.ToString(hHandle["SegValue" + siteSegment.SegmentNbr]));
                        if (cOASegValues != null && !String.IsNullOrEmpty(cOASegValues.LinkedPlant))
                        {
                            bool isValidPlant = false;
                            LibPlant.IsAuthorizedForPlant(cOASegValues.LinkedPlant, out isValidPlant, true);

                            if (!isValidPlant)
                            {
                                var Plant = FindFirstPlant(hHandle["Company"].ToString(), cOASegValues.LinkedPlant);
                                errMess = String.Format(errMess, Plant.Name);
                                switch (action)
                                {
                                    case VR_ACTION.Error:
                                        {
                                            lResult = false;
                                            this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle), Compatibility.Convert.ToString(hHandle["BookID"])), true);
                                            AddErrorToCache(cacheIndex, errMess, true, Compatibility.Convert.ToString(hHandle["BookID"]));
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            return lResult;
        }

        private void AddBlankToCache(string cacheIndex)
        {
            if (_PEData != null)
            {
                PEData.GLAccountValidCache acctCache = new PEData.GLAccountValidCache();
                acctCache.ErrMessData.Add(new PEData.ErrMessage(false, true));
                acctCache.IsValid = true;
                _PEData.GLAccountValidCacheRows.Add(cacheIndex, acctCache);
            }
        }

        private void AddErrorToCache(string cacheIndex, string errMessage, bool isError, string bookID = "")
        {
            if (_PEData != null)
            {
                PEData.GLAccountValidCache acctCache = new PEData.GLAccountValidCache();
                _PEData.GLAccountValidCacheRows.TryGetValue(cacheIndex, out acctCache);
                if (acctCache != null)
                {
                    acctCache.IsValid = acctCache.IsValid & !isError;
                    if (acctCache.ErrMessData[0].errMessage == String.Empty)
                    {
                        acctCache.ErrMessData[0].errMessage = errMessage;
                        acctCache.ErrMessData[0].IsError = isError;
                        acctCache.ErrMessData[0].BookID = bookID;
                    }
                }
            }
        }

        private string GetAccountCacheIndex(IRow hHandle, string ruleName = "")
        {
            string strJEDate = String.Empty;
            if (hHandle["JEDate"] != null)
                strJEDate = ConvertDateToString(hHandle["JEDate"]);
            return Compatibility.Convert.ToString(hHandle["Company"]) + "--" + Compatibility.Convert.ToString(hHandle["COACode"]) + "--" + getGLAccount(hHandle) + "--" + strJEDate + "--" + ruleName;
        }

        public string GetAccountCacheIndex(string[] keyList)
        {
            return String.Join("--", keyList);
        }

        private string ConvertDateToString(object date)
        {
            DateTime dt = (DateTime)date;
            return dt.ToString("MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture);
        }

        private string FormatErrMsgWithLineNum(string errMsg, IRow hHandle)
        {
            string retValue = errMsg;
            if (hHandle != null && !(hHandle is procRvTranGLC))
            {
                int iLine = (int)hHandle["JournalLine"];
                if (iLine > 0)
                    retValue = Strings.Line(iLine) + retValue;
            }
            return retValue;
        }

        private bool GetAccountCacheData(string cacheIndex, out bool isValid, IRow hHandle = null)
        {
            isValid = true;
            if (_PEData != null)
            {
                PEData.GLAccountValidCache acctCache = new PEData.GLAccountValidCache();
                _PEData.GLAccountValidCacheRows.TryGetValue(cacheIndex, out acctCache);
                if (acctCache != null)
                {
                    isValid = acctCache.IsValid;
                    if (acctCache.IsValid)
                        return true;

                    string bookID = Compatibility.Convert.ToString(hHandle["BookID"]);
                    foreach (var acctCacheRow in acctCache.ErrMessData)
                    {
                        string errMess = acctCacheRow.errMessage;

                        if (string.IsNullOrEmpty(bookID))
                        {
                            if (acctCacheRow.IsError)
                                this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle)), true);
                            else
                                this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle)));
                        }
                        else
                        {
                            if (acctCacheRow.IsError)
                                this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle), bookID), true);
                            else
                                this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle), bookID));
                        }
                    }
                    return true;
                }
                else return false;
            }
            else return false;
        }

        /// <summary>
        /// Check active of account without caching. Use AccountIsActiveRule method instead.
        /// </summary>
        /// <param name="iAccount"></param>
        /// <param name="iCOACode"></param>
        /// <param name="iCompany"></param>
        /// <param name="iDate"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:query is checked for injections")]
        public bool AccountIsActive(string iAccount, string iCOACode, string iCompany, DateTime? iDate)
        {
            using (PostingProfiler pt = new PostingProfiler(_PEData.TraceKeys, "AccountIsActive"))
            {
                string sQuery = string.Empty;
                string segFld = string.Empty;
                bool lAcctActive = false;
                iLine handle = new iLine() { BookID = "", JournalLine = 0 };
                string cacheIndex = string.Format("AccountIsActive_Err{0}{1}{2}{3}", iAccount, iCOACode, iCompany, iDate.Value.ToShortDateString());
                bool isValid = true;
                if (GetAccountCacheData(cacheIndex, out isValid, handle)) return isValid;

                PEData.GLAccountValidCache acctCache = new PEData.GLAccountValidCache();
                acctCache.IsValid = true;
                _PEData.GLAccountValidCacheRows.Add(cacheIndex, acctCache);

                using (SqlCommand cmd = new SqlCommand())
                {
                    sQuery = "select EffFrom, EffTo from Erp.GLAccount where GLAccount.Company = @Company and GLAccount.COACode = @COACode";
                    cmd.Parameters.AddWithValue("@Company", iCompany);
                    cmd.Parameters.AddWithValue("@COACode", iCOACode);

                    foreach (var COASegmentRow in (this.SelectCOASegment(iCompany, iCOACode, false)))
                    {
                        segFld = "SegValue" + COASegmentRow.SegmentNbr.ToString();
                        if (iAccount.NumEntries("|") >= COASegmentRow.SegmentNbr)
                        {
                            string paramName = "@" + segFld;
                            sQuery = sQuery + " and Erp.GLAccount." + segFld + " = " + paramName;
                            cmd.Parameters.AddWithValue(paramName, iAccount.Entry(COASegmentRow.SegmentNbr - 1, '|'));
                        }
                        else
                        {
                            sQuery = sQuery + " and (Erp.GLAccount." + segFld + " = '' or Erp.GLAccount." + segFld + " IS NULL)";
                        }
                    }

                    sQuery = sQuery + " and GLAccount.Active = 1";

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = Db.SqlConnection;
                    Epicor.Data.SafeSql.CheckSQLInjection.ValidateSyntax(sQuery);
                    cmd.CommandText = sQuery; // NOSONAR - Query is already checked for secure injections.

                    if (Db.SqlConnection.State == ConnectionState.Closed)
                    {
                        Db.SqlConnection.Open();
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lAcctActive = true;
                            if (reader["EffFrom"] != System.DBNull.Value && (DateTime)reader["EffFrom"] > iDate)
                            {
                                lAcctActive = false;

                            }
                            if (lAcctActive && reader["EffTo"] != System.DBNull.Value && (DateTime)reader["EffTo"] < iDate)
                            {
                                lAcctActive = false;

                            }
                        }
                        reader.Close();
                    }
                }

                if (!lAcctActive)
                {
                    _PEData.GLAccountValidCacheRows.TryGetValue(cacheIndex, out acctCache);
                    acctCache.IsValid = false;
                }
                pt.WriteMessage(string.Format("iAccount={0},iDate={1},lAcctActive={2}", iAccount, iDate.ToString(), lAcctActive));
                return lAcctActive;

            }
        }


        /*------------------------------------------------------------------------------
          Purpose:  Combination of controlled segments is invalid  
            Notes:  validation type = GlobLevel_CombControlSegInvalid    
        ------------------------------------------------------------------------------*/
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:query is checked for injections")]
        public bool CombControlSegInvalid_Err(string errMess, IRow hHandle, bool check = false)
        {
            bool lResult = true;
            string cmp = string.Empty;
            string coa = string.Empty;
            string segFld = string.Empty;
            string sQuery = string.Empty;
            string segValue = string.Empty;

            string cacheIndex = GetAccountCacheIndex(hHandle, "CombControlSegInvalid_Err");
            bool isValid = true;
            if (GetAccountCacheData(cacheIndex, out isValid, hHandle)) return isValid;
            AddBlankToCache(cacheIndex);

            var action = GetGlobLevelAction(ValidationType.GlobLevel_CombControlSegInvalid);
            if (check == true)
            {
                action = VR_ACTION.Undefined;
            }
            if (action != VR_ACTION.Ignore)
            {
                cmp = Compatibility.Convert.ToString(hHandle["Company"]);
                coa = Compatibility.Convert.ToString(hHandle["COACode"]);

                using (SqlCommand cmd = new SqlCommand())
                {

                    sQuery = "select GLAccount from Erp.GLAccount where Erp.GLAccount.Company = @Company and Erp.GLAccount.COACode = @COACode";
                    cmd.Parameters.AddWithValue("@Company", cmp);
                    cmd.Parameters.AddWithValue("@COACode", coa);

                    foreach (var COASegmentRow in (this.SelectCOASegment(cmp, coa, false)))
                    {
                        segFld = "SegValue" + COASegmentRow.SegmentNbr.ToString();
                        string paramName = "@" + segFld;
                        segValue = ((hHandle[segFld] == null) ? "" : hHandle[segFld].ToString());
                        sQuery = sQuery + " and GLAccount." + segFld + " = " + paramName;
                        cmd.Parameters.AddWithValue(paramName, segValue);
                    }

                    if (Db.SqlConnection.State == ConnectionState.Closed)
                    {
                        Db.SqlConnection.Open();
                    }

                    cmd.Connection = Db.SqlConnection;
                    cmd.CommandType = CommandType.Text;
                    Epicor.Data.SafeSql.CheckSQLInjection.ValidateSyntax(sQuery);
                    cmd.CommandText = sQuery;

                    var account = cmd.ExecuteScalar();
                    if (account == null)
                    {
                        errMess = errMess + String.Format("  Company {0}.  COACode {1}. Account {2}.", cmp, coa, getGLAccount(hHandle));
                        switch (action)
                        {
                            case VR_ACTION.Error:
                                {
                                    lResult = false;
                                    this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle), Compatibility.Convert.ToString(hHandle["BookID"])), true);
                                    AddErrorToCache(cacheIndex, errMess, true, Compatibility.Convert.ToString(hHandle["BookID"]));
                                }
                                break;
                            case VR_ACTION.Warning:
                                {
                                    this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle), Compatibility.Convert.ToString(hHandle["BookID"])));
                                    AddErrorToCache(cacheIndex, errMess, false, Compatibility.Convert.ToString(hHandle["BookID"]));
                                }
                                break;
                            case VR_ACTION.Undefined:
                                {
                                    lResult = false;
                                }
                                break;
                        }
                    }
                }
            }
            return lResult;
        }

        /*------------------------------------------------------------------------------
          Purpose:  Transaction currency amount is zero, but book currency amount is not zero    
            Notes:  validation type = RuleLevel_CurAmntZeroBookCurAmntNotZero 
        ------------------------------------------------------------------------------*/
        public bool CurAmntZeroBookCurAmntNotZero_Err(string errMess, procRvJrnTrDtl inRow)
        {
            bool lResult = true;
            if (inRow.IsRevaluationTran || gloRevaluationTran)
            {
                return lResult;
            }

            var action = GetRuleLevelAction(ValidationType.RuleLevel_CurAmntZeroBookCurAmntNotZero, inRow.BRuleUID);

            if (action != VR_ACTION.Ignore)
            {
                var lres = true;
                if (inRow.DebitAmount == 0 && inRow.BookDebitAmount != 0)
                    lres = false;
                else
                    if (inRow.CreditAmount == 0 && inRow.BookCreditAmount != 0)
                    lres = false;
                if (lres == false)
                {
                    switch (action)
                    {
                        case VR_ACTION.Error:
                            {
                                lResult = false;
                                this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, inRow), inRow.BookID), true);
                            }
                            break;                    /*run ErrorPut(errMess).*/
                        case VR_ACTION.Warning:
                            {
                                this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, inRow), inRow.BookID));
                            }
                            break;
                    }
                }
            }
            return lResult;
        }



        /*------------------------------------------------------------------------------
          Purpose:  Transaction currency amount is zero for currency account  
            Notes:  validation type = GlobLevel_CurAmountZeroForCurAcc
        ------------------------------------------------------------------------------*/
        public bool CurAmountZeroForCurAcc_Err(string errMess, procRvJrnTrDtl inRow)
        {
            bool lResult = true;
            if (inRow.IsRevaluationTran == true
            || inRow.IsGLAllocation == true
            || (inRow.SrcType.KeyEquals("P") || inRow.SrcType.KeyEquals("C"))
            || gloRevaluationTran)
            {
                return lResult;
            }

            var action = GetRuleLevelAction(ValidationType.RuleLevel_CurAmountZeroForCurAcc, inRow.BRuleUID);

            if (action != VR_ACTION.Ignore)
            {
                if (gloTransactionAccIsCurrencyAcc && (inRow.CreditAmount == 0 && inRow.DebitAmount == 0))
                {
                    switch (action)
                    {
                        case VR_ACTION.Error:
                            {
                                lResult = false;
                                this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, inRow), inRow.BookID), true);
                            }
                            break;                        /*run ErrorPut(errMess).*/
                        case VR_ACTION.Warning:
                            {
                                this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, inRow), inRow.BookID));
                            }
                            break;
                    }
                }
            }
            return lResult;
        }
        /*------------------------------------------------------------------------------
          Purpose:  Transaction currency is specified, but does not conform to currency account definition    
            Notes:  validation type = GlobLevel_CurNotConformCurAccDef
        ------------------------------------------------------------------------------*/
        public bool CurNotConformCurAccDef_Err(string errMess, procRvJrnTrDtl inRow)
        {
            bool lResult = true;
            var action = GetGlobLevelAction(ValidationType.GlobLevel_CurNotConformCurAccDef);
            if (action != VR_ACTION.Ignore)
            {

                var segmentCode = Compatibility.Convert.ToString(inRow["SegValue1"]);
                if (!String.IsNullOrEmpty(segmentCode) && !String.IsNullOrEmpty(inRow.CurrencyCode))
                {
                    if ((this.ExistsCOASegValues(inRow.Company, inRow.COACode, 1, segmentCode, "N")))
                    {
                        errMess = errMess + String.Format(" COACode {0}. SegmentCode {1}. Currency {2}.", inRow.COACode, segmentCode, inRow.CurrencyCode);
                        lResult = (this.ExistsCOASegAcct(inRow.Company, inRow.COACode, inRow.CurrencyCode, segmentCode, true));
                    }
                    if (!lResult)
                    {
                        this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, inRow), inRow.BookID), true);
                    }
                }
            }
            return lResult;
        }
        public bool fpApplyDateOutOfRange(string errMess, procRvJrnTrDtl inRow)
        {
            bool lResult = true;
            string vEADType = String.Empty;
            DateTime? newDate = null;
            DateTime? eaDate = null;
            bool eaDateDefined = false;

            var action = GetBookLevelAction(ValidationType.BookLevel_ApplyDateOutOfRange, inRow.BookID);
            if (inRow.CloseFiscalPeriod > 0)
            {
                return true;
            }

            if (action != VR_ACTION.Ignore)
            {
                vEADType = inRow.SourceModule;
                switch (this._PEData.TransactionTypeName.ToUpperInvariant())
                {         /* Cash Management controls following ACT types */
                    case "AR PAYMENT":
                    case "REVERSE CASH RECEIPT":
                    case "AP PAYMENT":
                    case "AP VOID PAYMENT":
                    case "BANK ADJUSTMENT":
                    case "BANK RECONCILIATION":
                    case "BANK FUNDS TRANSFER":
                    case "PETTY CASH MISC":
                        {
                            vEADType = "CM";
                        }
                        break;
                    case "FIXED ASSET":
                        {
                            vEADType = "AS";
                        }
                        break;
                    case "COSANDWIP":
                        {
                            vEADType = "IP";
                        }
                        break;
                }
                /*EAD maintenance should default dates by transaction type,
                this is not implemented yet. For now General Journal <=> General Ledger*/
                if (vEADType.KeyEquals("GL"))
                {
                    vEADType = "GJ";
                }
                var eadCompRow = this.FindFirstEADComp(inRow.Company);
                if (eadCompRow != null)
                {
                    eaDate = eadCompRow.EarliestApplyDate;
                    eaDateDefined = true;
                }

                /* Try to get earliest apply date for module by vEADType */
                var eadTypeRow = this.FindFirstEADType(inRow.Company, vEADType);
                if (eadTypeRow != null)
                {
                    if (eaDate == null || eaDate < eadTypeRow.EarliestApplyDate)
                    {
                        eaDate = eadTypeRow.EarliestApplyDate;
                    }

                    eaDateDefined = true;
                }

                if (eaDateDefined == true && inRow.JEDate < eaDate)
                {
                    errMess = errMess + String.Format(" BookID {0}. FiscalCalendarID {1}. FiscalYear {2}. FiscalPeriod {3}. EntryDate {4}. ", inRow.BookID, inRow.FiscalCalendarID, inRow.FiscalYear, inRow.FiscalPeriod, inRow.JEDate.Value.ToString("MM/dd/yyyy"));
                    switch (action)
                    {
                        case VR_ACTION.Error:
                            {
                                lResult = false;
                                this._PEData.PEError.AddException(new RuleException(errMess, inRow.BookID), true);
                            }
                            break;
                        case VR_ACTION.Autocorrect_with_Warning:
                            {
                                Erp.Tables.GLBookPer GLBookPerRow = null;


                                GLBookPerRow = this.FindFirstGLBookPer(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, eaDate, eaDate, false);
                                if (GLBookPerRow != null)
                                {
                                    newDate = eaDate;
                                }
                                else
                                {
                                    GLBookPerRow = this.FindFirstGLBookPer(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, eaDate, false);
                                    if (GLBookPerRow != null)
                                    {
                                        newDate = GLBookPerRow.StartDate;
                                    }
                                }
                                if (GLBookPerRow != null)
                                {
                                    inRow.JEDate = newDate;
                                    inRow.FiscalYear = GLBookPerRow.FiscalYear;
                                    inRow.FiscalYearSuffix = GLBookPerRow.FiscalYearSuffix;
                                    inRow.FiscalPeriod = GLBookPerRow.FiscalPeriod;
                                    errMess = errMess + String.Format("Apply Date was shifted to {0}.", newDate.Value.ToString("MM/dd/yyyy"));
                                    this._PEData.PEError.AddWarning(new RuleException(errMess, inRow.BookID));
                                }
                            }
                            break;
                    }
                }
            }
            return lResult;
        }
        public bool fpClosed(string errMess, procRvJrnTrDtl inRow)
        {
            bool lResult = true;
            var action = GetBookLevelAction(ValidationType.BookLevel_FiscalPeriodClosed, inRow.BookID);
            if (action != VR_ACTION.Ignore)
            {
                var GLBookPerRow = this.FindFirstGLBookPer(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, inRow.FiscalYear, inRow.FiscalYearSuffix, inRow.FiscalPeriod);
                if (GLBookPerRow != null)
                {
                    if (GLBookPerRow.ClosedPeriod == true)
                    {
                        errMess = errMess + String.Format(" BookID {0}. FiscalCalendarID {1}. FiscalYear {2}. FiscalYearSuffix {3}. FiscalPeriod {4}.", inRow.BookID, inRow.FiscalCalendarID, inRow.FiscalYear, inRow.FiscalYearSuffix, inRow.FiscalPeriod);
                        if (action == VR_ACTION.Error)
                        {
                            if (!this._PEData.PEError.IsExceptionDuplicated(errMess))
                            {
                                this._PEData.PEError.AddException(new RuleException(errMess, inRow.BookID), true);
                            }

                            lResult = false;
                        }
                        else
                        {
                            this._PEData.PEError.AddWarning(new RuleException(errMess, inRow.BookID));          /*run warningput(errMess).*/
                            if (action == VR_ACTION.Autocorrect_with_Warning)
                            {
                                if (inRow.CloseFiscalPeriod > 0)
                                    GLBookPerRow = this.FindFirstGLBookPer3(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, inRow.JEDate, false, inRow.CloseFiscalPeriod);
                                else
                                    GLBookPerRow = this.FindFirstGLBookPer2(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, inRow.JEDate, false);

                                if (GLBookPerRow != null)
                                {
                                    inRow.FiscalYear = GLBookPerRow.FiscalYear;
                                    inRow.FiscalPeriod = GLBookPerRow.FiscalPeriod;
                                    inRow.JEDate = GLBookPerRow.StartDate;
                                    inRow.FiscalYearSuffix = GLBookPerRow.FiscalYearSuffix;
                                    if (!this._PEData.PEError.IsExceptionDuplicated(errMess))
                                    {
                                        errMess = errMess + String.Format(" Fiscal Period was shifted to {0}.", GLBookPerRow.FiscalPeriod);
                                        this._PEData.PEError.AddWarning(new RuleException(errMess, inRow.BookID));
                                    }
                                }
                                else
                                {
                                    if (!this._PEData.PEError.IsExceptionDuplicated(errMess))
                                    {
                                        errMess = errMess + String.Format(" Opened Fiscal Period for Book {0} was not found.", inRow.BookID);
                                        this._PEData.PEError.AddException(new RuleException(errMess, inRow.BookID), true);
                                    }
                                    lResult = false;
                                }
                            }
                            else
                            {
                                this._PEData.PEError.AddWarning(new RuleException(errMess, inRow.BookID));
                            }
                        }
                    }
                }
            }
            return lResult;
        }
        public bool fpClosingPeriodNotExists(string errMess, procRvJrnTrDtl inRow)
        {
            if (inRow.Reverse)
                return true;

            int intClosingPeriod = 0;
            Erp.Tables.GLBookPer bGLBookPer = null;


            /* Is there Closing Period Number from BookingRules? */
            intClosingPeriod = inRow.CloseFiscalPeriod;
            if (intClosingPeriod == 0)
            {
                bGLBookPer = this.FindFirstGLBookPer(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, inRow.FiscalYear, inRow.FiscalYearSuffix, inRow.FiscalPeriod, 0);
                if (bGLBookPer != null)
                {
                    intClosingPeriod = bGLBookPer.CloseFiscalPeriod;
                }
            }
            /* Closing period doesn't specified. */
            if (intClosingPeriod == 0)
            {
                return true;
            }
            /* Closing Period Number was specified */

            /* If specified Closing Period Number is more then quantity of existing Closing Periods - we are giving the lastest period */
            bGLBookPer = this.FindFirstGLBookPer(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, inRow.JEDate, inRow.JEDate, intClosingPeriod);
            if (bGLBookPer == null)
            {
                bGLBookPer = this.FindLastGLBookPer(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, inRow.JEDate, inRow.JEDate, 0);
            }
            /* Excellent we've found Closing Period */
            if (bGLBookPer != null)
            {
                inRow.FiscalYear = bGLBookPer.FiscalYear;
                inRow.FiscalPeriod = bGLBookPer.FiscalPeriod;
                inRow.FiscalYearSuffix = bGLBookPer.FiscalYearSuffix;
                return true;
            }
            else
            {
                var action = GetBookLevelAction(ValidationType.BookLevel_ClosingPeriodNotExists, inRow.BookID);
                errMess = errMess + String.Format(" BookID {0}. FiscalCalendarID {1}. FiscalYear {2}. ClosingPeriod {3}.", inRow.BookID, inRow.FiscalCalendarID, inRow.FiscalYear, intClosingPeriod);
                switch (action)
                {
                    case VR_ACTION.Error:
                        {
                            this._PEData.PEError.AddException(new RuleException(errMess, inRow.BookID), true);
                            return false;
                        }
                    /* Autocorrect */
                    case VR_ACTION.Autocorrect_with_Warning:
                        {
                            this._PEData.PEError.AddWarning(new RuleException(errMess, inRow.BookID));
                            /* autocorrection - find opened odinary period for current date */
                            var GLBookPerRow = this.FindFirstGLBookPer(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, 0, false, inRow.JEDate, inRow.JEDate);
                            if (GLBookPerRow != null)
                            {
                                inRow.FiscalYear = GLBookPerRow.FiscalYear;
                                inRow.FiscalPeriod = GLBookPerRow.FiscalPeriod;
                                inRow.FiscalYearSuffix = GLBookPerRow.FiscalYearSuffix;
                                inRow.FiscalCalendarID = GLBookPerRow.FiscalCalendarID;
                                inRow.CloseFiscalPeriod = 0;
                            }
                            else
                            {
                                /* autocorrection - find closed odinary period for current date */
                                GLBookPerRow = this.FindFirstGLBookPer(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, 0, true, inRow.JEDate, inRow.JEDate);
                                if (GLBookPerRow != null)
                                {
                                    inRow.CloseFiscalPeriod = 0;
                                    inRow.FiscalPeriod = GLBookPerRow.FiscalPeriod;
                                    this._PEData.PEError.AddWarning(new RuleException(Strings.Autocorrectionlogicforvalidationrule(GetErrorMessage(ValidationType.BookLevel_ClosingPeriodNotExists)), inRow.BookID));
                                }
                                else
                                    this._PEData.PEError.AddWarning(new RuleException(Strings.Autocorrectionlogicforvalidationrulefailed(GetErrorMessage(ValidationType.BookLevel_ClosingPeriodNotExists), inRow.JEDate), inRow.BookID));
                            }
                        }
                        break;
                }
            }
            return true;
        }
        public bool fpNotBasedOnApplyDate(string errMess, procRvJrnTrDtl inRow)
        {
            bool lResult = true;
            bool isManualUpdate = false;

            if (inRow.FiscalPeriod == 0)
            {
                string openBal = FindFirstGLBook2(Session.CompanyID, inRow.BookID);
                if (!String.IsNullOrEmpty(openBal))
                    isManualUpdate = (openBal.KeyEquals("JOURNAL"));
            }

            if (isManualUpdate == false)
            {
                GLBookPer GLBookPer = null;

                if (inRow.FiscalYear == 0)
                {
                    /* try to find it Fiscal Year/Period */
                    GLBookPer = this.FindFirstGLBookPer(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, inRow.JEDate, inRow.JEDate, false);
                    if (GLBookPer != null)
                    {
                        inRow.FiscalYear = GLBookPer.FiscalYear;
                        inRow.FiscalYearSuffix = GLBookPer.FiscalYearSuffix;
                        inRow.FiscalPeriod = GLBookPer.FiscalPeriod;
                    }
                }
                else
                    GLBookPer = this.FindFirstGLBookPer(inRow.Company, inRow.BookID, inRow.FiscalCalendarID, inRow.FiscalYear, inRow.FiscalPeriod, inRow.JEDate, inRow.JEDate);

                if (GLBookPer == null)
                {
                    errMess = errMess + String.Format(" BookID {0}. FiscalCalendarID {1}. FiscalYear {2}. FiscalPeriod {3}. ApplyDate {4}", inRow.BookID, inRow.FiscalCalendarID, inRow.FiscalYear, inRow.FiscalPeriod, inRow.JEDate.Value.ToString("MM/dd/yyyy"));
                    this._PEData.PEError.AddException(new RuleException(errMess, inRow.BookID), true);
                    lResult = false;
                }
            }
            else
            {
                /* validate FiscalYr and ApplyDate for openning period */
                var FiscalYear = this.FindFirstFiscalYr(inRow.Company, inRow.FiscalCalendarID, inRow.FiscalYear, inRow.FiscalYearSuffix);
                if (FiscalYear == null)
                {
                    errMess = errMess + String.Format(" BookID {0}. FiscalCalendarID {1}. FiscalYear {2}. FiscalPeriod {3}. ApplyDate {4}", inRow.BookID, inRow.FiscalCalendarID, inRow.FiscalYear, inRow.FiscalPeriod, inRow.JEDate.Value.ToString("MM/dd/yyyy"));
                    this._PEData.PEError.AddException(new RuleException(errMess, inRow.BookID), true);
                    lResult = false;
                }
                else
                {
                    /* correct Apply Date for oppening Period */
                    if (FiscalYear.StartDate != inRow.JEDate)
                        inRow.JEDate = FiscalYear.StartDate;
                }
            }
            return lResult;
        }

        public VR_ACTION GetBookLevelAction(ValidationType validRuleTypeUID, string bookid)
        {
            Dictionary<int, VR_ACTION> currBook;
            BVBookCache.TryGetValue(bookid, out currBook);

            if (currBook == null)
            {
                currBook = new Dictionary<int, VR_ACTION>();
                foreach (var BVRuleitem in SelectBVRule(Session.CompanyID, bookid, VR_LEVEL.BOOK.ToString()))
                {
                    if (currBook.ContainsKey(BVRuleitem.VRuleUID))
                    {
                        this._PEData.PEError.AddException(new RuleException(Strings.InvalidvalidationrulesdataPleaserunconversionID1180), true);
                        return VR_ACTION.Undefined;
                    }
                    currBook.Add(BVRuleitem.VRuleUID, ActionParse.ToEnum(BVRuleitem.Action));
                }
                BVBookCache.Add(bookid, currBook);
            }

            VR_ACTION retAction;
            if (currBook.TryGetValue(Convert.ToInt32(validRuleTypeUID), out retAction))
            {
                return retAction;
            }

            return VR_ACTION.Undefined;
        }

        public VR_ACTION GetGlobLevelAction(ValidationType validRuleTypeUID)
        {
            VR_ACTION action = VR_ACTION.Undefined;
            var altValidationRuleType = (from row in this.LibBVRule.ValidationRuleTypeRows
                                         where row.TypeUID == validRuleTypeUID
                                         select row).FirstOrDefault();

            if (altValidationRuleType != null)
            {
                action = altValidationRuleType.ActionsList[0];
            }
            return action;
        }

        public string GetErrorMessage(ValidationType errorCode)
        {
            string errMess = "Error description was not found";
            var altValidationRuleType = (from row in this.LibBVRule.ValidationRuleTypeRows
                                         where row.TypeUID == errorCode
                                         select row).FirstOrDefault();
            if (altValidationRuleType != null)
            {
                errMess = altValidationRuleType.ErrMessage;
            }

            return errMess;
        }

        public string GetErrorMessage(ValidationType errorCode, params object[] args)
        {
            string s = GetErrorMessage(errorCode);
            return String.Format(s, args);
        }

        public VR_ACTION GetRuleLevelAction(ValidationType validRuleTypeUID, int ruleUID)
        {
            if (ruleUID == 0)
            {
                return VR_ACTION.Ignore;
            }

            if (gloIsLimitedTransaction == LogicalExtended.Undefined)
            {

                ACTTypeResult acttype = FindFirstACTType(Session.CompanyID, this._PEData.ABTUID);
                if (acttype != null)
                {
                    this._PEData.Limited = acttype.Limited;
                    if (this._PEData.Limited)
                        gloIsLimitedTransaction = LogicalExtended.Yes;
                    else
                        gloIsLimitedTransaction = LogicalExtended.No;
                }

            }

            if (gloIsLimitedTransaction == LogicalExtended.Yes)
            {
                return VR_ACTION.Ignore;   /* Ignore */
            }

            Dictionary<int, VR_ACTION> currRule;
            BVRuleCache.TryGetValue(ruleUID, out currRule);

            if (currRule == null)
            {
                currRule = new Dictionary<int, VR_ACTION>();
                foreach (var BVRuleitem in SelectBVRule(Session.CompanyID, ruleUID))
                {
                    currRule.Add(BVRuleitem.VRuleUID, ActionParse.ToEnum(BVRuleitem.Action));
                }
                BVRuleCache.Add(ruleUID, currRule);
            }

            VR_ACTION retAction;
            if (currRule.TryGetValue(Convert.ToInt32(validRuleTypeUID), out retAction))
            {
                return retAction;
            }

            var altValidationRuleType = (from row in this.LibBVRule.ValidationRuleTypeRows
                                         where row.TypeUID == validRuleTypeUID
                                         select row).FirstOrDefault();

            if (altValidationRuleType != null)
                return altValidationRuleType.ActionDefault;

            return VR_ACTION.Undefined;
        }

        public int fillttGLAccount<T>(string id, DataTable dt, T row)
        {
            int maxUsedSegment = 20;
            if (dt.Columns.Count == 0) return maxUsedSegment;
            DataRow dtRow = dt.NewRow();
            dtRow[0] = id;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var fieldName = row.GetType().GetProperty(dt.Columns[i].ColumnName);
                if (fieldName != null)
                {
                    dtRow[i] = fieldName.GetValue(row);
                    if (dt.Columns[i].ColumnName.StartsWith("SegValue", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(dtRow[i].ToString()) && dtRow[i] != null)
                        {
                            int segNum = Convert.ToInt16(dt.Columns[i].ColumnName.Substring(1, dt.Columns[i].ColumnName.Length - 1));
                            if (maxUsedSegment < segNum)
                                maxUsedSegment = segNum;
                        }
                    }
                }
            }
            dt.Rows.Add(dtRow);
            return maxUsedSegment;
        }

        ///<summary>  
        ///<purpose> Check if Account is Valid </purpose>
        ///<param name="hHandle">row with account to validate</param>
        ///<param name="isBalanceAccount">Determines if validation account is balance account</param>
        ///</summary>
        public bool GLAccountValid(IRow hHandle, bool isBalanceAccount = false)
        {
            using (PostingProfiler pt = new PostingProfiler(_PEData.TraceKeys, "GLAccountValid"))
            {

                bool AccountValid = true;
                string cErrMess = string.Empty;
                string cErrTranGLC = string.Empty;

                string[] arSegValues = new string[20];

                if (hHandle is procRvTranGLC)
                {
                    cErrTranGLC = Strings.ReferAccountValidError;
                    cErrTranGLC = cErrTranGLC + String.Format("ControlType {0} Context {1} BookID {2} Reference Info {3} Key1 {4} Key2 {5} Key3 {6} Key4 {7} Key5 {8}.  ", hHandle["SysGlControltype"], hHandle["GLAcctContext"], hHandle["BookID"], hHandle["RelatedToFile"], hHandle["key1"], hHandle["key2"], hHandle["key3"], hHandle["key4"], hHandle["key5"]);
                }

                /* Check block for "BOOK" and "GLOB" levels */
                cErrMess = cErrTranGLC + GetErrorMessage(ValidationType.GlobLevel_SegmentValue);
                AccountValid = SegmentValue_Err(cErrMess, hHandle) && AccountValid;
                if (!isBalanceAccount)
                {
                    cErrMess = cErrTranGLC + GetErrorMessage(ValidationType.GlobLevel_ReqDynSegNotSpec);
                    AccountValid = ReqDynSegNotSpec_Err(cErrMess, hHandle) && AccountValid;
                }
                cErrMess = cErrTranGLC + GetErrorMessage(ValidationType.BookLevel_SegmentNotUsed);
                AccountValid = SegmentNotUsed_Err(cErrMess, hHandle) && AccountValid;
                if (!isBalanceAccount)
                {
                    cErrMess = cErrTranGLC + GetErrorMessage(ValidationType.GlobLevel_CombControlSegInvalid);
                    AccountValid = CombControlSegInvalid_Err(cErrMess, hHandle, false) && AccountValid;

                    cErrMess = cErrTranGLC + GetErrorMessage(ValidationType.GlobLevel_AccInactive);
                    AccountValid = AccInactive_Err(cErrMess, hHandle) && AccountValid;

                    if (this._PEData.GLPost && hHandle is procRvJrnTrDtl)
                    {
                        GLBook peBook = null;
                        if (setbufPEBook(hHandle["BookID"].ToString(), ref peBook))
                        {
                            if (peBook.OpenBalUpdateOpt.KeyEquals("Auto"))
                            {
                                cErrMess = cErrTranGLC + GetErrorMessage(ValidationType.GlobLevel_REAccountNotMatchToMask);
                                AccountValid = REAccountNotMatchToMask_Err(cErrMess, (procRvJrnTrDtl)hHandle, true) && AccountValid;
                            }
                        }
                    }

                    cErrMess = cErrTranGLC + GetErrorMessage(ValidationType.GlobLevel_AuthorizedSite);
                    AccountValid = AccountAuthorizedSite_Err(cErrMess, hHandle) && AccountValid;
                }
                string glAccount = getGLAccount(hHandle).ToString();
                if (AccountValid)
                {
                    int segCount = glAccount.NumEntries("|");

                    #region assign arSegValues[i]
                    arSegValues[0] = hHandle["SegValue1"].ToString();
                    if (segCount > 1) arSegValues[1] = hHandle["SegValue2"].ToString();
                    if (segCount > 2) arSegValues[2] = hHandle["SegValue3"].ToString();
                    if (segCount > 3) arSegValues[3] = hHandle["SegValue4"].ToString();
                    if (segCount > 4) arSegValues[4] = hHandle["SegValue5"].ToString();
                    if (segCount > 5) arSegValues[5] = hHandle["SegValue6"].ToString();
                    if (segCount > 6) arSegValues[6] = hHandle["SegValue7"].ToString();
                    if (segCount > 7) arSegValues[7] = hHandle["SegValue8"].ToString();
                    if (segCount > 8) arSegValues[8] = hHandle["SegValue9"].ToString();
                    if (segCount > 9) arSegValues[9] = hHandle["SegValue10"].ToString();
                    if (segCount > 10) arSegValues[10] = hHandle["SegValue11"].ToString();
                    if (segCount > 11) arSegValues[11] = hHandle["SegValue12"].ToString();
                    if (segCount > 12) arSegValues[12] = hHandle["SegValue13"].ToString();
                    if (segCount > 13) arSegValues[13] = hHandle["SegValue14"].ToString();
                    if (segCount > 14) arSegValues[14] = hHandle["SegValue15"].ToString();
                    if (segCount > 15) arSegValues[15] = hHandle["SegValue16"].ToString();
                    if (segCount > 16) arSegValues[16] = hHandle["SegValue17"].ToString();
                    if (segCount > 17) arSegValues[17] = hHandle["SegValue18"].ToString();
                    if (segCount > 18) arSegValues[18] = hHandle["SegValue19"].ToString();
                    if (segCount > 19) arSegValues[19] = hHandle["SegValue20"].ToString();
                    #endregion

                    if (!existsGLAcctDisp(hHandle["Company"].ToString(), hHandle["COACode"].ToString(), glAccount))
                    {
                        using (var txScope = new TransactionScope(TransactionScopeOption.Suppress))
                        {
                            using (var context = Ice.Services.ContextFactory.CreateContext<ErpContext>())
                            {
                                var defaultTransactionOptions = new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(3, 0, 0) };
                                using (var tr = new System.Transactions.TransactionScope(TransactionScopeOption.Required, defaultTransactionOptions))
                                {
                                    using (var libGLAcctDisp = new GL.CreateGLAcctDisp(context))
                                    {
                                        libGLAcctDisp.ProcessDispAccount(hHandle["Company"].ToString(), hHandle["COACode"].ToString(), glAccount, arSegValues, "", false, false, false);
                                    }
                                    tr.Complete();
                                }
                            }
                            txScope.Complete();
                        }
                    }
                }
                pt.WriteMessage(string.Format("glAccount={0},AccountValid={1}", glAccount, AccountValid));
                return AccountValid;
            }
        }

        public bool glbTransNotBalance(string errMess, string inBookID)
        {
            bool lResult = true;

            /*  Modified to check balance of transaction only by book amounts,                 */
            /*  because accounts can have currency different from book currency and            */
            /*  then transaction amounts will be depend on acoount currency.                   */
            /*  EXAMPLE:                                                                       */
            /*  Debit Line:  DebitAmount = 10    BookDebitAmount  = 11.5  CurrencyCode = ?UR?*/
            /*  Credit Line: CreditAmount = 11.5 BookCreditAmount = 11.5  CurrencyCode = ?SD?*/
            /*  Such transaction is balanced.                                                  */
            decimal bcTotAmount = decimal.Zero;
            decimal bdTotAmount = decimal.Zero;
            decimal dRoundDiff = decimal.Zero;
            int iJournalLine = 0;
            int iSeg = 0;
            string sSegField = string.Empty;

            Erp.Tables.GLBook peBook = null;
            foreach (var bprocRvJrnTrDtl_iterator in (from row in this._PEData.ProcRvJrnTrDtlRows
                                                      where row.ParentLine == 0 &&
                                                       row.BookID.KeyEquals(inBookID)
                                                      orderby row.JournalLine
                                                      select row))
            {
                bcTotAmount = bcTotAmount + bprocRvJrnTrDtl_iterator.BookCreditAmount;
                bdTotAmount = bdTotAmount + bprocRvJrnTrDtl_iterator.BookDebitAmount;
            }

            var bprocRvJrnTrDtl = (from row in this._PEData.ProcRvJrnTrDtlRows
                                   where row.ParentLine == 0 &&
                                    row.BookID.KeyEquals(inBookID)
                                   orderby row.JournalLine
                                   select row).FirstOrDefault();


            if (!setbufPEBook(inBookID, ref peBook))
            {
                this._PEData.PEError.AddException(new RuleException(Strings.UnknownBook(inBookID)), true);
                return false;
            }
            if (Math.Abs(bdTotAmount - bcTotAmount) > 0)
            {
                if (Math.Abs(bdTotAmount - bcTotAmount) <= peBook.RndTolerance)
                {
                    var procRvJrnTrDtlRow = (from row in this._PEData.ProcRvJrnTrDtlRows
                                             where row.ParentLine == 0 &&
                                             row.BookID.KeyEquals(inBookID)
                                             orderby row.JournalLine descending
                                             select row).FirstOrDefault();

                    iJournalLine = procRvJrnTrDtlRow.JournalLine;

                    dRoundDiff = bdTotAmount - bcTotAmount;            /*Creating rounding transaction*/

                    var newProcRvJrnTrDtl = new procRvJrnTrDtl();
                    BufferCopy.CopyExceptFor(bprocRvJrnTrDtl, newProcRvJrnTrDtl, procRvJrnTrDtl.ColumnNames.SysRowID, procRvJrnTrDtl.ColumnNames.IsSummarize, procRvJrnTrDtl.ColumnNames.RvJrnTrDtlUID,
                                procRvJrnTrDtl.ColumnNames.ExtCompanyID, procRvJrnTrDtl.ColumnNames.ExtRefType, procRvJrnTrDtl.ColumnNames.ExtRefCode, procRvJrnTrDtl.ColumnNames.ExtSegValue1, procRvJrnTrDtl.ColumnNames.ExtSegValue2, procRvJrnTrDtl.ColumnNames.ExtSegValue3, procRvJrnTrDtl.ColumnNames.ExtSegValue4, procRvJrnTrDtl.ColumnNames.ExtSegValue5, procRvJrnTrDtl.ColumnNames.ExtSegValue6,
                                procRvJrnTrDtl.ColumnNames.ExtSegValue7, procRvJrnTrDtl.ColumnNames.ExtSegValue8, procRvJrnTrDtl.ColumnNames.ExtSegValue9, procRvJrnTrDtl.ColumnNames.ExtSegValue10, procRvJrnTrDtl.ColumnNames.ExtSegValue11, procRvJrnTrDtl.ColumnNames.ExtSegValue12, procRvJrnTrDtl.ColumnNames.ExtSegValue13, procRvJrnTrDtl.ColumnNames.ExtSegValue14, procRvJrnTrDtl.ColumnNames.ExtSegValue15,
                                procRvJrnTrDtl.ColumnNames.ExtSegValue16, procRvJrnTrDtl.ColumnNames.ExtSegValue17, procRvJrnTrDtl.ColumnNames.ExtSegValue18, procRvJrnTrDtl.ColumnNames.ExtSegValue19, procRvJrnTrDtl.ColumnNames.ExtSegValue20, procRvJrnTrDtl.ColumnNames.ExtGLAccount, procRvJrnTrDtl.ColumnNames.ExtCOACode,
                                procRvJrnTrDtl.ColumnNames.DebitStatAmt, procRvJrnTrDtl.ColumnNames.CreditStatAmt, procRvJrnTrDtl.ColumnNames.StatUOMCode, procRvJrnTrDtl.ColumnNames.Statistical);
                    if (_PEData.TransactionTypeName.KeyEquals("COSAndWIP"))
                        newProcRvJrnTrDtl.LegalNumber = "";
                    newProcRvJrnTrDtl.BRuleUID = 0;
                    newProcRvJrnTrDtl.Description = "Rounding difference amount";
                    newProcRvJrnTrDtl.DebitAmount = 0;
                    newProcRvJrnTrDtl.CreditAmount = 0;
                    newProcRvJrnTrDtl.BookDebitAmount = 0;
                    newProcRvJrnTrDtl.BookCreditAmount = 0;
                    newProcRvJrnTrDtl.JournalLine = iJournalLine + 1;
                    if (!newProcRvJrnTrDtl.RedStorno)
                    {
                        if (dRoundDiff < 0)
                        {
                            newProcRvJrnTrDtl.BookDebitAmount = -dRoundDiff;
                        }
                        else
                        {
                            newProcRvJrnTrDtl.BookCreditAmount = dRoundDiff;
                        }
                    }
                    else if (dRoundDiff > 0)
                    {
                        newProcRvJrnTrDtl.BookDebitAmount = -dRoundDiff;
                    }
                    else
                    {
                        newProcRvJrnTrDtl.BookCreditAmount = dRoundDiff;
                    }

                    newProcRvJrnTrDtl.GLAccount = peBook.RndAccount;
                    for (iSeg = 1; iSeg <= 20; iSeg++)
                    {
                        sSegField = "RndSegValue" + iSeg.ToString();
                        newProcRvJrnTrDtl["SegValue" + iSeg.ToString()] = peBook[sSegField];
                    }

                    _PEData.ProcRvJrnTrDtlRows.Add(newProcRvJrnTrDtl);
                    _PEData.LogView.AddWarning(Strings.WarningRoundingMessage(inBookID, Compatibility.Convert.ToString(dRoundDiff)), _PEData.LogView.RJ);

                }
                else
                {
                    this._PEData.PEError.AddException(new RuleException(errMess, inBookID), true);
                    lResult = false;
                }
            }
            return lResult;
        }

        public bool IsIncomestatementAcct(procRvJrnTrDtl inRow)
        {
            string segmentCode = string.Empty;
            bool bAccIsIncomeStatement = false;

            /* Get segment code of natural account */
            segmentCode = inRow.SegValue1;
            if (!String.IsNullOrEmpty(segmentCode))
            {
                bAccIsIncomeStatement = IsIncomestatementAccount(inRow.Company, inRow.COACode, segmentCode);
            }

            return bAccIsIncomeStatement;
        }
        public bool IsSelfBalSegNotBal(string ERROR_MESSAGE, string inBookID)
        {
            bool isValid = true;
            foreach (var altprocRvJrnTrDtl in (from row in this._PEData.ProcRvJrnTrDtlRows
                                               where row.ParentLine != 0 && row.BookID.KeyEquals(inBookID)
                                               select row))
            {
                altprocRvJrnTrDtl.JournalLine = 0;
            }
            using (ListWrapper<procRvJrnTrDtl> listWrapper = new ListWrapper<procRvJrnTrDtl>(this._PEData.ProcRvJrnTrDtlRows.GetItemsByIndex(0, inBookID)))
            {
                foreach (var altprocRvJrnTrDtlRow in listWrapper)
                {
                    if (listWrapper.FirstOf("BookID"))
                    {
                        var action = GetBookLevelAction(ValidationType.BookLevel_SelfBalSegNotBal, altprocRvJrnTrDtlRow.BookID);
                        if (this.ExistSelfBalSeg(altprocRvJrnTrDtlRow.Company, altprocRvJrnTrDtlRow.COACode))
                        {
                            BalTransactionsLevel(altprocRvJrnTrDtlRow.COACode, action, ERROR_MESSAGE, inBookID, out isValid);
                            if (!isValid)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            int iLines = 0;
            var altprocRvJrnTrDtl1 = (from row in this._PEData.ProcRvJrnTrDtlRows
                                      where row.ParentLine == 0 && row.BookID.KeyEquals(inBookID)
                                      select row).LastOrDefault();

            if (altprocRvJrnTrDtl1 != null)
            {
                iLines = altprocRvJrnTrDtl1.JournalLine;
            }

            foreach (var altprocRvJrnTrDtl_iterator in (from row in this._PEData.ProcRvJrnTrDtlRows
                                                        where row.ParentLine != 0 && row.BookID.KeyEquals(inBookID)
                                                        select row))
            {
                iLines = iLines + 1;
                altprocRvJrnTrDtl_iterator.JournalLine = iLines;
            }
            return isValid;
        }
        /*------------------------------------------------------------------------------
            Purpose:
            Notes: this rule is good only if both (Credit and Debit) lines will be created by one booking rule. 
        ------------------------------------------------------------------------------*/
        public bool IsTransNotBiLine(string errMess)
        {
            bool lResult = true;
            int iCredit = 0;
            int iDebit = 0;
            bool lCorrAccUIDViolated = false;

            List<int> ProcessedTranSequencesRows = new List<int>();

            var procRvJrnTrDtlRow = (from procRvJrnTrDtl_Row in this._PEData.ProcRvJrnTrDtlRows
                                     select procRvJrnTrDtl_Row).FirstOrDefault();

            var sAction = GetBookLevelAction(ValidationType.BookLevel_TransNotBiLine, procRvJrnTrDtlRow.BookID);

            if (sAction != VR_ACTION.Ignore)
            {
                using (ListWrapper<procRvJrnTrDtl> listwrapper = new ListWrapper<procRvJrnTrDtl>(
                        from bprocRvJrnTrDtl_Row in this._PEData.ProcRvJrnTrDtlRows
                        where bprocRvJrnTrDtl_Row.ParentLine == 0
                        orderby bprocRvJrnTrDtl_Row.BookID, bprocRvJrnTrDtl_Row.TranSeq
                        select bprocRvJrnTrDtl_Row
                    ))
                {
                    foreach (var bprocRvJrnTrDtl in listwrapper)
                    {
                        if (bprocRvJrnTrDtl.BookCreditAmount != 0)
                        {
                            iCredit = iCredit + 1;
                        }
                        else
                        {
                            iDebit = iDebit + 1;
                        }
                        lCorrAccUIDViolated = true;

                        foreach (var altprocRvJrnTrDtl_iterator in (from row in this._PEData.ProcRvJrnTrDtlRows
                                                                    where row.ParentLine == bprocRvJrnTrDtl.JournalLine &&
                                                                    row.BookID.KeyEquals(bprocRvJrnTrDtl.BookID)
                                                                    select row))
                        {
                            var JoinFieldsResult = altprocRvJrnTrDtl_iterator;

                            foreach (var bprocRvTranGLC_iterator in this._PEData.ProcRvTranGLCRows.GetItemsByTranSeqIndex(JoinFieldsResult.BookID, JoinFieldsResult.TranSeq))
                            {
                                var bprocRvTranGLCRow = bprocRvTranGLC_iterator;

                                var altprocRvTranGLC = this._PEData.ProcRvTranGLCRows.GetItemsByTranSeqIndex(bprocRvTranGLCRow.BookID, bprocRvTranGLCRow.TranSeq).Where(r => r.SysRowID != bprocRvTranGLCRow.SysRowID).FirstOrDefault();
                                lCorrAccUIDViolated = altprocRvTranGLC == null;
                                if (!lCorrAccUIDViolated)
                                {
                                    ProcessedTranSequencesRows.Add(altprocRvTranGLC.TranSeq);
                                }
                            }
                        }

                        bool lastofBookID = listwrapper.LastOf("BookID");
                        bool lastofTranSeq = listwrapper.LastOf("TranSeq");

                        if (lastofBookID || lastofTranSeq)
                        {

                            int ProcessedTranSequences = ProcessedTranSequencesRows.IndexOf(bprocRvJrnTrDtl.TranSeq);

                            if ((iCredit != 1 || iDebit != 1))
                            {
                                if (lCorrAccUIDViolated && ProcessedTranSequences == -1)
                                {
                                    if (sAction == VR_ACTION.Error)
                                    {
                                        this._PEData.PEError.AddException(new RuleException(errMess, bprocRvJrnTrDtl.BookID), true);
                                        lResult = false;
                                    }
                                    if (sAction == VR_ACTION.Warning)
                                    {
                                        this._PEData.PEError.AddWarning(new RuleException(errMess, bprocRvJrnTrDtl.BookID));
                                    }
                                    return lResult;
                                }
                            }
                            iCredit = 0;
                            iDebit = 0;
                        }

                    }
                }
            }
            return lResult;
        }
        /*------------------------------------------------------------------------------
            Purpose:
            Notes: isExcludeBalance =true, for perfomance.
        ------------------------------------------------------------------------------*/
        public bool isValidTrHeader(bool isExcludeBalance, string inBookID, procRvJrnTrDtl inRow)
        {
            using (PostingProfiler pt = new PostingProfiler(_PEData.TraceKeys, "isValidTrHeader"))
            {
                bool lresult = true;    /* Check block for "BOOK" and "GLOB" levels */
                bool isExistBook = false;
                bool notConsolidatedBook = true;
                bool activeBook = true;
                bool lBalancedTransaction = false;
                // Erp.Tables.TranGLC bTranGLC = null;
                Erp.Tables.GLBook peBook = null;

                bool lDuplicateError = false;
                string cDupMess = string.Empty;
                string vABTName = string.Empty;
                string cDupAbtUID = string.Empty;

                if (this._PEData.GLPost == false)
                {
                    return true;
                }
                if (this._PEData.Mode != PostingMode.PostEditList)
                {
                    string[] glAllowTranGLCDuplicate = { "AP Void Logged Invoice", "CompleteStorno", "GL Allocations", "GLRevaluation",
                                                "PeriodicConsolidation", "Posted Invoice Update", "Revaluation", "Reverse Cash Receipt",
                                                "SingleGLJrn", "Bank Funds Transfer", "Fixed Asset", "Payroll Check", "AP PI Write-off",
                                                "AR PI Write-off", "Bank Reconciliation","AR Bill of Exchange Protested",
                                                "AR Bill of Exchange Cancelled","AR Collections","AR PI Payment", "AR PI Voiding",
                                                "Apply Credit Memo", "AR PI Reverse Endorsement" };

                    /* Get ACT Type by cSharedAbtUID */
                    var acttype = FindFirstACTType(Session.CompanyID, this._PEData.ABTUID);
                    if (acttype != null)
                    {
                        vABTName = acttype.DisplayName;
                    }

                    if (!glAllowTranGLCDuplicate.Any(x => x.Compare(vABTName, false) == 0))
                    {

                        foreach (var _ttprocRvTranGLC in _PEData.ProcRvTranGLCRows.Where(x => x.BookID.KeyEquals(inBookID)))
                        {
                            _PEData.ttprocRvTranGLC = _ttprocRvTranGLC;

                            #region copy of code that check existedTranglc using storageprovcedure. for 1,5mln from 30 min to 13min
                            bool existsTranGLC = false;
                            using (var sqlCommand = new SqlCommand("Erp.ExistsTranGLC", Db.SqlConnection))
                            {
                                sqlCommand.CommandType = CommandType.StoredProcedure;

                                var param1 = new SqlParameter("@Company", SqlDbType.NVarChar);
                                param1.Direction = ParameterDirection.Input;
                                param1.Value = _PEData.ttprocRvTranGLC.Company;
                                sqlCommand.Parameters.Add(param1);

                                var param2 = new SqlParameter("@RelatedToFile", SqlDbType.NVarChar);
                                param2.Direction = ParameterDirection.Input;
                                param2.Value = _PEData.ttprocRvTranGLC.RelatedToFile;
                                sqlCommand.Parameters.Add(param2);

                                var param3 = new SqlParameter("@Key1", SqlDbType.NVarChar);
                                param3.Direction = ParameterDirection.Input;
                                param3.Value = _PEData.ttprocRvTranGLC.Key1;
                                sqlCommand.Parameters.Add(param3);

                                var param4 = new SqlParameter("@Key2", SqlDbType.NVarChar);
                                param4.Direction = ParameterDirection.Input;
                                param4.Value = _PEData.ttprocRvTranGLC.Key2;
                                sqlCommand.Parameters.Add(param4);

                                var param5 = new SqlParameter("@Key3", SqlDbType.NVarChar);
                                param5.Direction = ParameterDirection.Input;
                                param5.Value = _PEData.ttprocRvTranGLC.Key3;
                                sqlCommand.Parameters.Add(param5);

                                var param6 = new SqlParameter("@Key4", SqlDbType.NVarChar);
                                param6.Direction = ParameterDirection.Input;
                                param6.Value = _PEData.ttprocRvTranGLC.Key4;
                                sqlCommand.Parameters.Add(param6);

                                var param7 = new SqlParameter("@Key5", SqlDbType.NVarChar);
                                param7.Direction = ParameterDirection.Input;
                                param7.Value = _PEData.ttprocRvTranGLC.Key5;
                                sqlCommand.Parameters.Add(param7);

                                var param8 = new SqlParameter("@Key6", SqlDbType.NVarChar);
                                param8.Direction = ParameterDirection.Input;
                                param8.Value = _PEData.ttprocRvTranGLC.Key6;
                                sqlCommand.Parameters.Add(param8);

                                var param9 = new SqlParameter("@SysGLControlType", SqlDbType.NVarChar);
                                param9.Direction = ParameterDirection.Input;
                                param9.Value = _PEData.ttprocRvTranGLC.SysGLControlType;
                                sqlCommand.Parameters.Add(param9);

                                var param10 = new SqlParameter("@GLAcctContext", SqlDbType.NVarChar);
                                param10.Direction = ParameterDirection.Input;
                                param10.Value = _PEData.ttprocRvTranGLC.GLAcctContext;
                                sqlCommand.Parameters.Add(param10);

                                var param11 = new SqlParameter("@BookID", SqlDbType.NVarChar);
                                param11.Direction = ParameterDirection.Input;
                                param11.Value = _PEData.ttprocRvTranGLC.BookID;
                                sqlCommand.Parameters.Add(param11);

                                var param12 = new SqlParameter("@RecordType", SqlDbType.NVarChar);
                                param12.Direction = ParameterDirection.Input;
                                param12.Value = _PEData.ttprocRvTranGLC.RecordType;
                                sqlCommand.Parameters.Add(param12);

                                var param13 = new SqlParameter("@IsExist", SqlDbType.Bit);
                                param13.Value = false;
                                param13.Direction = ParameterDirection.InputOutput;
                                sqlCommand.Parameters.Add(param13);

                                var param14 = new SqlParameter("@ABTUID", SqlDbType.NVarChar, 36);
                                param14.Direction = ParameterDirection.InputOutput;
                                param14.Value = string.Empty;
                                sqlCommand.Parameters.Add(param14);

                                sqlCommand.CommandTimeout = PELib.CommandTimeout;
                                sqlCommand.ExecuteNonQuery();
                                existsTranGLC = (bool)sqlCommand.Parameters["@IsExist"].Value;
                                cDupAbtUID = (string)sqlCommand.Parameters["@ABTUID"].Value;
                                lDuplicateError = existsTranGLC;

                            }
                            if (existsTranGLC)
                            {
                                cDupMess = Strings.TranGLCDupliFoundPostingProcessAbortedToPrevent + Environment.NewLine;
                                cDupMess += Strings.NTransTypeRevisAbtUIDOldAb(vABTName, this._PEData.ABTUID, cDupAbtUID);
                                this._PEData.PEError.AddException(new RuleException(cDupMess, inBookID), true);

                            }
                            #endregion copy of code that check existedTranglc using storage procedure. for 1,5mln from 30 min to 13min

                            if (lDuplicateError) return false;
                            /*TranGLC accounts validation (reference rules)*/
                            if (_PEData.ttprocRvTranGLC.RecordType.KeyEquals("A"))
                                lresult = GLAccountValid(_PEData.ttprocRvTranGLC) && lresult;
                        }
                    }
                }
                /*for good performance*/
                gloTransactionCurrIsSpecified = false;

                if (setbufPEBook(inRow.BookID, ref peBook))
                {
                    isExistBook = true;
                    notConsolidatedBook = (peBook.BookType != 2);
                    activeBook = (peBook.Inactive == false);
                }

                foreach (var ValidationRuleType_iterator in (from ValidationRuleType_Row in LibBVRule.ValidationRuleTypeRows
                                                             where ValidationRuleType_Row.Level != VR_LEVEL.RULE
                                                             select ValidationRuleType_Row))
                {
                    var ValidationRuleTypeRow = ValidationRuleType_iterator;
                    switch (ValidationRuleTypeRow.TypeUID)
                    {
                        case ValidationType.GlobLevel_BookNotExist:
                            {
                                if (!isExistBook)
                                {
                                    this._PEData.PEError.AddException(new RuleException(ValidationRuleTypeRow.ErrMessage, inBookID), true);
                                    lresult = false;
                                }
                            }
                            break;
                        case ValidationType.GlobLevel_BookIConsolidated:
                            {
                                if (!notConsolidatedBook)
                                {
                                    this._PEData.PEError.AddException(new RuleException(ValidationRuleTypeRow.ErrMessage, inBookID), true);
                                    lresult = false;
                                }
                            }
                            break;
                        case ValidationType.GlobLevel_PostToBookDisabled:
                            {
                                if (!activeBook)
                                {
                                    this._PEData.PEError.AddException(new RuleException(GetErrorMessage(ValidationType.GlobLevel_PostToBookDisabled, peBook.Description, _PEData.TransactionTypeName), inBookID), true);
                                    lresult = false;
                                }
                            }
                            break;
                        case ValidationType.GlobLevel_JournalCodeWrong:
                            {
                                var JrnlCodeRow = this.FindFirstJrnlCode(inRow.Company, inRow.JournalCode);
                                if (JrnlCodeRow == null)
                                {
                                    this._PEData.PEError.AddException(new RuleException(ValidationRuleTypeRow.ErrMessage, inBookID), true);
                                    lresult = false;
                                }
                            }
                            break;
                        case ValidationType.BookLevel_TransNotBiLine:
                            {
                                lresult = IsTransNotBiLine(ValidationRuleTypeRow.ErrMessage) && lresult;
                            }
                            break;
                        case ValidationType.GlobLevel_TransNotBalance:
                            {
                                if (isExcludeBalance == false)
                                {
                                    if (inRow.UnbalancedTran == true)
                                    {
                                        lBalancedTransaction = false;
                                    }
                                    else
                                    {
                                        lBalancedTransaction = glbTransNotBalance(ValidationRuleTypeRow.ErrMessage, inBookID);
                                        lresult = lresult && lBalancedTransaction;
                                    }
                                }
                            }
                            break;
                        case ValidationType.BookLevel_SelfBalSegNotBalNextYear:
                            {
                                lresult = SelfBalSegNotBalNextYear_Err(ValidationRuleTypeRow.ErrMessage) && lresult;
                            }
                            break;
                    }
                }
                if (isExcludeBalance == false && lBalancedTransaction)
                {
                    var validationRuleTypeRow = (from ValidationRuleType_Row in this.LibBVRule.ValidationRuleTypeRows
                                                 where ValidationRuleType_Row.TypeUID == ValidationType.BookLevel_SelfBalSegNotBal
                                                 select ValidationRuleType_Row).FirstOrDefault();
                    if (validationRuleTypeRow != null)
                    {
                        lresult = IsSelfBalSegNotBal(validationRuleTypeRow.ErrMessage, inBookID) && lresult;
                    }
                }

                SiteSegmentBalancing(inBookID, inRow.SourcePlant);

                if (isVietnamLocalization)
                {
                    lresult = VNCheckAccountCorrespondence(_PEData.ProcRvJrnTrDtlRows.GetItemsByIndex(0, inBookID), inBookID) && lresult;
                }
                var persistentErr = (from row in Db.WError where row.Company == inRow.Company && row.RvJrnUID == inRow.RvJrnUID && row.RvJrnTrUID == inRow.RvJrnTrUID && row.IsError == true && row.Persistent == true select row).Any();
                lresult = lresult && !persistentErr;

                pt.WriteMessage("isValid" + lresult);
                return lresult;
            }
        }
        public bool IsValidTrLine(procRvJrnTrDtl inRow)
        {
            using (PostingProfiler pt = new PostingProfiler(_PEData.TraceKeys, "isValidTrLine"))
            {
                bool lresult = true;
                string segmentCode = string.Empty;
                string cErrMess = string.Empty;
                Erp.Tables.GLBook peBook = null;

                /*for good perfomance*/
                gloTransactionCurrIsSpecified = false;
                if (setbufPEBook(inRow.BookID, ref peBook))
                {
                    gloTransactionCurrIsSpecified = !peBook.CurrencyCode.KeyEquals(inRow.CurrencyCode);
                }
                gloTransactionAccIsCurrencyAcc = false;    /* Get segment code from corresponding field of transaction line for getting Natural Account with could be Currency or not */

                segmentCode = inRow.SegValue1;
                if (!String.IsNullOrEmpty(segmentCode))
                {
                    gloTransactionAccIsCurrencyAcc = IsCurrencyAccount(inRow.Company, inRow.COACode, 1, segmentCode);
                }

                if (this._PEData.GLPost == true)
                {
                    cErrMess = GetErrorMessage(ValidationType.RuleLevel_CurAmountZeroForCurAcc);
                    lresult = CurAmountZeroForCurAcc_Err(cErrMess, inRow) && lresult;
                    cErrMess = GetErrorMessage(ValidationType.RuleLevel_CurAmntZeroBookCurAmntNotZero);
                    lresult = CurAmntZeroBookCurAmntNotZero_Err(cErrMess, inRow) && lresult;
                    cErrMess = GetErrorMessage(ValidationType.Rulelevel_WrongSignOfAmountForDbtOrCrt);
                    lresult = WrongSignOfAmountForDbtOrCrt_Err(cErrMess, inRow) && lresult;
                    cErrMess = GetErrorMessage(ValidationType.Rulelevel_WrongSignOfAmountForRedStorno);
                    lresult = WrongSignOfAmountForRedStorno_Err(cErrMess, inRow) && lresult;
                    cErrMess = GetErrorMessage(ValidationType.GlobLevel_CurNotConformCurAccDef);
                    lresult = CurNotConformCurAccDef_Err(cErrMess, inRow) && lresult;
                    cErrMess = GetErrorMessage(ValidationType.GlobLevel_RedStornoDisabled);
                    lresult = RedStornoDisabled_Err(cErrMess) && lresult;
                    cErrMess = GetErrorMessage(ValidationType.GlobLevel_TransAndBookAmtHasDiffSigns);
                    lresult = TransAndBookAmtHasDiffSigns_Err(cErrMess, inRow) && lresult;
                    cErrMess = GetErrorMessage(ValidationType.GlobLevel_QtyAndAmtHasDiffSigns);
                    lresult = QtyAndAmtHasDiffSigns_Err(cErrMess) && lresult;
                    cErrMess = GetErrorMessage(ValidationType.BookLevel_ClosingPeriodNotExists);
                    lresult = fpClosingPeriodNotExists(cErrMess, inRow) && lresult;
                    cErrMess = GetErrorMessage(ValidationType.BookLevel_FiscalPeriodClosed);
                    lresult = fpClosed(cErrMess, inRow) && lresult;
                    cErrMess = GetErrorMessage(ValidationType.BookLevel_ApplyDateOutOfRange);
                    lresult = fpApplyDateOutOfRange(cErrMess, inRow) && lresult;
                    cErrMess = GetErrorMessage(ValidationType.GlobLevel_FiscalParamNotBasedOnApplyDate);
                    lresult = fpNotBasedOnApplyDate(cErrMess, inRow) && lresult;
                    cErrMess = string.Empty;
                    lresult = CheckStatData(cErrMess, inRow) && lresult;
                }        /* moved to GLAccountValid        

            /*performance - should check once*/
                /* Not supported in .NET */
                lresult = GLAccountValid(inRow) && lresult;
                pt.WriteMessage("isValid" + lresult);
                return lresult;
            }
        }


        /*------------------------------------------------------------------------------
            Purpose: Quantity and amount has different signs (one is Dr, and other is Cr)
            Notes: validation type = GlobLevel_QtyAndAmtHasDiffSigns
        ------------------------------------------------------------------------------*/
        public bool QtyAndAmtHasDiffSigns_Err(string errMess)
        {
            bool lResult = true;
            var action = GetGlobLevelAction(ValidationType.GlobLevel_AccInactive);
            if (action != VR_ACTION.Ignore)
            { }
            return lResult;
        }
        /*------------------------------------------------------------------------------
          Purpose:  Quantity is specified for non-statistical account  
            Notes:  validation type = BookLevel_NonStatAcc  
        ------------------------------------------------------------------------------*/
        public bool QtyForNonStatAcc_Err(string errMess, procRvJrnTrDtl inRow)
        {
            bool lResult = true;
            var action = GetBookLevelAction(ValidationType.BookLevel_NonStatAcc, inRow.BookID);
            if (action != VR_ACTION.Ignore)
            { }
            return lResult;
        }


        /*------------------------------------------------------------------------------
          Purpose:  Quantity is zero, but amount is not zero (for mixed account)    
            Notes:  validation type = BookLevel_QtyZeroAmtNotZero  
        ------------------------------------------------------------------------------*/
        public bool QtyZeroAmtNotZero_Err(string errMess, procRvJrnTrDtl inRow)
        {
            bool lResult = true;
            var action = GetBookLevelAction(ValidationType.BookLevel_QtyZeroAmtNotZero, inRow.BookID);
            if (action != VR_ACTION.Ignore)
            { }
            return lResult;
        }


        /*------------------------------------------------------------------------------
          Purpose:  Quantity is zero for statistical account  
            Notes:  validation type = BookLevel_QtyZeroForStatAcc
        ------------------------------------------------------------------------------*/
        public bool QtyZeroForStatAcc_Err(string errMess, procRvJrnTrDtl inRow)
        {
            bool lResult = true;
            var action = GetBookLevelAction(ValidationType.BookLevel_QtyZeroForStatAcc, inRow.BookID);
            if (action != VR_ACTION.Ignore)
            { }
            return lResult;
        }
        /*------------------------------------------------------------------------------
            Purpose: Retained Earnings account can not be determined for source account 
                    (does not match to any mask) 
                    Note: masks for retrieving Retained Earnings accounts should be defined in the COA settings 

            Notes: validation type = GlobLevel_REAccountNotMatchToMask
        ------------------------------------------------------------------------------*/
        public bool REAccountNotMatchToMask_Err(string errMess, procRvJrnTrDtl inRow, bool errCache = false)
        {
            using (PostingProfiler pt = new PostingProfiler(_PEData.TraceKeys, "REAccountNotMatchToMask_Err"))
            {
                bool lResult = true;
                bool lSubResult = true;
                int i = 0;
                bool bRetainedEarningsAccFound = false;
                string sREWrongErr = "Retained Earnings account is wrong: ";
                string dispAcct = string.Empty;
                int acctLength = 0;
                Erp.Tables.FiscalPer bFiscalPer = null;
                Erp.Tables.GLBook peBook = null;
                int ffiscalYear = 0;
                string sMaskedSegValue = string.Empty;
                string sSourceSegValue = string.Empty;
                Erp.Tables.GLAccountMasks bGLAccountMasks = null;
                var action = GetGlobLevelAction(ValidationType.GlobLevel_REAccountNotMatchToMask);
                if (action != VR_ACTION.Ignore)
                {
                    /* Check existing of next year open balance */
                    /* this checking meaningful only if next year period exist*/
                    ffiscalYear = inRow.FiscalYear + 1;
                    /*  find first next year period */
                    bFiscalPer = this.FindFirstFiscalPer(Session.CompanyID, inRow.FiscalCalendarID, ffiscalYear, inRow.FiscalYearSuffix);
                    if (bFiscalPer == null)
                    {
                        return true;
                    }

                    if (IsIncomestatementAcct(inRow))
                    {
                        foreach (var bGLAccountMasks_iterator in (this.SelectGLAccountMasks(inRow.Company, inRow.COACode, inRow.BookID, "RE")))
                        {
                            bGLAccountMasks = bGLAccountMasks_iterator;

                            if (!Erp.Internal.GL.GLAccountCore.AccountMaskMatch(inRow.GLAccount, bGLAccountMasks.GLMaskedAccount))
                                continue;

                            /* So, we have found records specified by mask.
                            In this case we have to create Retained Earnings account and check its validation */

                            var ttRetainedEarningsAccounts = new GLAccountWithJEDate();
                            ttRetainedEarningsAccounts.Company = inRow.Company;
                            ttRetainedEarningsAccounts.COACode = inRow.COACode;
                            ttRetainedEarningsAccounts.JournalLine = inRow.JournalLine;
                            ttRetainedEarningsAccounts.BookID = inRow.BookID;

                            dispAcct = "";
                            acctLength = 0;
                            for (i = 1; i <= 20; i++)
                            {
                                sSourceSegValue = inRow["SegValue" + i.ToString()].ToString();
                                sMaskedSegValue = LibGetREAccount.GetSegValueByMask(bGLAccountMasks["TgtSegValue" + i.ToString()].ToString(), sSourceSegValue);

                                ttRetainedEarningsAccounts["SegValue" + i.ToString()] = sMaskedSegValue;

                                if (!String.IsNullOrEmpty(dispAcct))
                                {
                                    dispAcct = dispAcct + "|";
                                }

                                if (!String.IsNullOrEmpty(sMaskedSegValue))
                                {
                                    dispAcct = dispAcct + sMaskedSegValue;
                                    acctLength = dispAcct.Length;
                                }
                            }
                            ttRetainedEarningsAccounts.GLAccount1 = dispAcct.Substring(0, acctLength);
                            ttRetainedEarningsAccounts.JEDate = GetNextYearStartDate(inRow.FiscalCalendarID, inRow.JEDate);
                            bRetainedEarningsAccFound = true;

                            /* Have to Check founded accounts here */
                            sREWrongErr = GetErrorMessage(ValidationType.GlobLevel_REAccountWrong) + String.Format(" - {0} - ", ttRetainedEarningsAccounts.GLAccount1);                /* Whether account active for next year first period or not  */

                            lSubResult = REAccountWrongForOB_Err(sREWrongErr + GetErrorMessage(ValidationType.GlobLevel_AccInactive), ttRetainedEarningsAccounts, bFiscalPer.StartDate);

                            lSubResult = lSubResult && SegmentValue_Err(sREWrongErr + GetErrorMessage(ValidationType.GlobLevel_SegmentValue), ttRetainedEarningsAccounts);

                            lSubResult = lSubResult && ReqDynSegNotSpec_Err(sREWrongErr + GetErrorMessage(ValidationType.GlobLevel_ReqDynSegNotSpec), ttRetainedEarningsAccounts);

                            lSubResult = lSubResult && CombControlSegInvalid_Err(sREWrongErr + GetErrorMessage(ValidationType.GlobLevel_CombControlSegInvalid), ttRetainedEarningsAccounts);

                            lSubResult = lSubResult && SegmentNotUsed_Err(sREWrongErr + GetErrorMessage(ValidationType.BookLevel_SegmentNotUsed), ttRetainedEarningsAccounts);

                            lResult = lSubResult;

                            /* we need only first matched mask */
                            break;
                        }  /* for each bGLAccountMasks where */

                        if (!bRetainedEarningsAccFound)
                        {
                            if (setbufPEBook(inRow.BookID, ref peBook))
                            {
                                GLAccountWithJEDate ttRetainedEarningsAccounts = null;
                                if (RetainedEarningsAccountRows != null)
                                    ttRetainedEarningsAccounts = (from r in RetainedEarningsAccountRows
                                                                  where r.BookID.KeyEquals(inRow.BookID)
                                                                  select r).FirstOrDefault();

                                if (ttRetainedEarningsAccounts == null ||
                                    (!string.IsNullOrEmpty(peBook.REAccount) && (peBook.REAccount.IndexOf('_') >= 0 || peBook.REAccount.IndexOf('%') >= 0))) // REAccount contains masked segments
                                {
                                    ttRetainedEarningsAccounts = new GLAccountWithJEDate();
                                    RetainedEarningsAccountRows.Add(ttRetainedEarningsAccounts);
                                    ttRetainedEarningsAccounts.Company = inRow.Company;
                                    ttRetainedEarningsAccounts.COACode = inRow.COACode;
                                    ttRetainedEarningsAccounts.JournalLine = inRow.JournalLine;
                                    ttRetainedEarningsAccounts.BookID = inRow.BookID;
                                    dispAcct = "";
                                    acctLength = 0;
                                    for (i = 1; i <= 20; i++)
                                    {
                                        sSourceSegValue = getSegment(inRow, i);
                                        sMaskedSegValue = LibGetREAccount.GetSegValueByMask(peBook["RESegValue" + i.ToString()].ToString(), sSourceSegValue);

                                        ttRetainedEarningsAccounts["SegValue" + i.ToString()] = sMaskedSegValue;
                                        if (!String.IsNullOrEmpty(dispAcct))
                                        {
                                            dispAcct = dispAcct + "|";
                                        }

                                        if (!String.IsNullOrEmpty(sMaskedSegValue))
                                        {
                                            dispAcct = dispAcct + sMaskedSegValue;
                                            acctLength = dispAcct.Length;
                                        }
                                    }
                                    ttRetainedEarningsAccounts.GLAccount1 = dispAcct.Substring(0, acctLength);
                                    ttRetainedEarningsAccounts.JEDate = GetNextYearStartDate(inRow.FiscalCalendarID, inRow.JEDate);
                                }
                                else
                                {
                                    ttRetainedEarningsAccounts.JournalLine = inRow.JournalLine;
                                }

                                sREWrongErr = GetErrorMessage(ValidationType.GlobLevel_REAccountWrong) + String.Format(" - {0} - ", ttRetainedEarningsAccounts.GLAccount1);                        /* Whether account active for next year first period or not  */
                                lSubResult = REAccountWrongForOB_Err(sREWrongErr + GetErrorMessage(ValidationType.GlobLevel_AccInactive), ttRetainedEarningsAccounts, bFiscalPer.StartDate);
                                lSubResult = lSubResult && SegmentValue_Err(sREWrongErr + GetErrorMessage(ValidationType.GlobLevel_SegmentValue), ttRetainedEarningsAccounts);
                                lSubResult = lSubResult && ReqDynSegNotSpec_Err(sREWrongErr + GetErrorMessage(ValidationType.GlobLevel_ReqDynSegNotSpec), ttRetainedEarningsAccounts);
                                lSubResult = lSubResult && CombControlSegInvalid_Err(sREWrongErr + GetErrorMessage(ValidationType.GlobLevel_CombControlSegInvalid), ttRetainedEarningsAccounts);
                                lSubResult = lSubResult && SegmentNotUsed_Err(sREWrongErr + GetErrorMessage(ValidationType.BookLevel_SegmentNotUsed), ttRetainedEarningsAccounts);
                                lResult = lSubResult;

                            }
                        }
                    }
                }
                return lResult;
            }
        }


        private DateTime? GetNextYearStartDate(string inFiscalCalendarID, DateTime? inRowDate)
        {
            FiscalYr fiscalYr = FindFirstFiscalYr(Session.CompanyID, inFiscalCalendarID, inRowDate);
            if (fiscalYr != null)
            {
                FiscalYr nextFiscalYear = FindFirstFiscalYr(Session.CompanyID, inFiscalCalendarID, fiscalYr.NextFiscalYear, fiscalYr.NextFiscalYearSuffix);
                if (nextFiscalYear != null)
                {
                    return nextFiscalYear.StartDate;
                }
                else
                    return fiscalYr.EndDate;
            }
            else
                return CompanyTime.Today();
        }

        /*------------------------------------------------------------------------------
            Purpose: Retained Earnings account is wrong for opening balance period
            Notes: validation type = GlobLevel_REAccountWrong
        ------------------------------------------------------------------------------*/
        public bool REAccountWrongForOB_Err(string errMess, IRow hBuff, DateTime? chkDate)
        {
            bool lResult = true;

            /* this checking accured in REAccountNotMatchToMask_Err */
            var action = GetGlobLevelAction(ValidationType.GlobLevel_REAccountWrong);
            if (action != VR_ACTION.Ignore)
            {
                lResult = AccountIsActive(getGLAccount(hBuff).ToString(), hBuff["COACode"].ToString(), hBuff["Company"].ToString(), chkDate);
                if (!lResult)
                {
                    switch (action)
                    {
                        case VR_ACTION.Error:
                            {
                                this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, hBuff), Compatibility.Convert.ToString(hBuff["BookID"])), true);
                            }
                            break;
                        case VR_ACTION.Warning:
                            {
                                this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, hBuff), Compatibility.Convert.ToString(hBuff["BookID"])));
                            }
                            break;
                    }
                }
            }
            return lResult;
        }


        /*------------------------------------------------------------------------------
            Purpose: Red storno is indicated but is disabled for book (red storno flag is set 
                for the accounting transaction, but the red storno is prohibited in the book? settings)

            Notes: validation type = GlobLevel_RedStornoDisabled
        ------------------------------------------------------------------------------*/
        public bool RedStornoDisabled_Err(string errMess)
        {

            bool lResult = true;
            var action = GetGlobLevelAction(ValidationType.GlobLevel_AccInactive);
            if (action != VR_ACTION.Ignore)
            { }

            return lResult;
        }
        /*------------------------------------------------------------------------------
          Purpose:  Required dynamic segment is not specified   
            Notes:  validation type = GlobLevel_ReqDynSegNotSpec  
        ------------------------------------------------------------------------------*/
        public bool ReqDynSegNotSpec_Err(string errMess, IRow hHandle)
        {
            bool lResult = true;
            string segVal = string.Empty;
            bool errFound = false;

            string cacheIndex = GetAccountCacheIndex(hHandle, "ReqDynSegNotSpec_Err");
            bool isValid = true;
            if (GetAccountCacheData(cacheIndex, out isValid, hHandle)) return isValid;
            AddBlankToCache(cacheIndex);

            var action = GetGlobLevelAction(ValidationType.GlobLevel_ReqDynSegNotSpec);
            if (action != VR_ACTION.Ignore)
            {
                foreach (var COASegmentRow in (this.SelectCOASegment(hHandle["Company"].ToString(), hHandle["COACode"].ToString(), true, "0", "1")))
                {
                    segVal = getSegment(hHandle, COASegmentRow.SegmentNbr);
                    if (segVal.Trim().Length == 0)
                    {
                        errFound = false;
                        errMess = errMess + String.Format("  COACode {0}. SegmentNumber {1}. Account {2}", hHandle["COACode"].ToString(), COASegmentRow.SegmentNbr, getGLAccount(hHandle).ToString());
                        if (COASegmentRow.EntryControl == "0")
                        {
                            errFound = true;
                        }
                        else
                        {
                            foreach (var COASegOptRow in (this.SelectCOASegOpt(COASegmentRow.Company, COASegmentRow.COACode, COASegmentRow.SegmentNbr, "M")))
                            {
                                if (getSegment(hHandle, COASegOptRow.SegmentNbr).KeyEquals(COASegOptRow.SegmentCode))
                                {
                                    errFound = true;
                                    break;
                                }
                            }
                        }
                        if (errFound)
                        {
                            switch (action)
                            {
                                case VR_ACTION.Error:
                                    {
                                        this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle), Compatibility.Convert.ToString(hHandle["BookID"])), true);
                                        AddErrorToCache(cacheIndex, errMess, true);
                                        lResult = false;
                                    }
                                    break;
                                case VR_ACTION.Warning:
                                    {
                                        this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle), Compatibility.Convert.ToString(hHandle["BookID"])));
                                        AddErrorToCache(cacheIndex, errMess, false);
                                    }
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
            return lResult;
        }
        /*------------------------------------------------------------------------------
          Purpose:  Segment is defined as 'not used', but Segment value is specified (depends on natural account)    
            Notes:  validation type = BookLevel_SegmentNotUsed  
        ------------------------------------------------------------------------------*/
        public bool SegmentNotUsed_Err(string errMess, IRow hHandle)
        {
            bool lResult = true;
            string segVal = string.Empty;

            string cacheIndex = GetAccountCacheIndex(hHandle, "SegmentNotUsed_Err");
            bool isValid = true;
            if (GetAccountCacheData(cacheIndex, out isValid, hHandle)) return isValid;
            AddBlankToCache(cacheIndex);

            var action = GetBookLevelAction(ValidationType.BookLevel_SegmentNotUsed, ((IGLAccount)hHandle).BookID);
            if (action != VR_ACTION.Ignore)
            {
                var notUsedSubSegments = new List<int>();
                foreach (var COASegmentRow in (this.SelectCOASegment3(((IGLAccount)hHandle).Company, ((IGLAccount)hHandle).COACode)))
                {

                    if (COASegmentRow.SegmentNbr == 1) // Natural segment
                    {
                        var options = SelectCOASegOpt(((IGLAccount)hHandle).Company, ((IGLAccount)hHandle).COACode, ((IGLAccount)hHandle).SegValue1, "N");
                        notUsedSubSegments.AddRange(options);
                    }
                    if (!(this.ExistsCOASegValues(COASegmentRow.Company, COASegmentRow.COACode, COASegmentRow.SegmentNbr)) || notUsedSubSegments.Contains(COASegmentRow.SegmentNbr))
                    {
                        segVal = getSegment(hHandle, COASegmentRow.SegmentNbr);
                        errMess = String.Format(errMess, COASegmentRow.SegmentName);
                        if (segVal.Trim().Length > 0)
                        {
                            switch (action)
                            {
                                case VR_ACTION.Error:
                                    {
                                        this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle), Compatibility.Convert.ToString(hHandle["BookID"])), true);
                                        AddErrorToCache(cacheIndex, errMess, true);
                                        lResult = false;
                                    }
                                    break;
                                case VR_ACTION.Autocorrect:
                                    {
                                        CorrectNotUsedSegment(ref hHandle, COASegmentRow.SegmentNbr);
                                    }
                                    break;
                                case VR_ACTION.Autocorrect_with_Warning:
                                    {
                                        CorrectNotUsedSegment(ref hHandle, COASegmentRow.SegmentNbr);
                                        this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, hHandle), Compatibility.Convert.ToString(hHandle["BookID"])));
                                    }
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
            return lResult;
        }

        private static void CorrectNotUsedSegment(ref IRow hHandle, int segmentNbr)
        {
            hHandle["SegValue" + segmentNbr] = "";
            hHandle["GLAccount"] = hHandle["GLAccount"].ToString().Entry("", segmentNbr - 1, PEData.ACC_SEPINTERNAL).TrimEnd(PEData.ACC_SEPINTERNAL);
        }

        /*------------------------------------------------------------------------------
 Purpose:  Segment value is wrong  
   Notes:  validation type = GlobLevel_SegmentValue
------------------------------------------------------------------------------*/
        public bool SegmentValue_Err(string errMess, IRow hBuff)
        {
            bool lResult = true;
            bool lMatches = true;
            int i = 0;
            string segVal = string.Empty;
            bool errRef = false;
            string cacheIndex = GetAccountCacheIndex(hBuff, "SegmentValue_Err");
            bool isValid = true;
            if (GetAccountCacheData(cacheIndex, out isValid, hBuff)) return isValid;
            AddBlankToCache(cacheIndex);

            var action = GetGlobLevelAction(ValidationType.GlobLevel_SegmentValue);
            DateTime? JEDate = CompanyTime.Now();
            string company = hBuff["Company"].ToString();
            string coaCode = hBuff["COACode"].ToString();
            if (action != VR_ACTION.Ignore)
            {
                for (i = 1; i <= 20; i++)
                {
                    segVal = getSegment(hBuff, i);
                    if (segVal.Trim().Length > 0)
                    {
                        if (hBuff["JEDate"] != null)
                        {
                            JEDate = (DateTime?)hBuff["JEDate"];
                        }

                        if (!IsCOASegValueActive(company, coaCode, i, segVal, JEDate))
                        {
                            errMess = errMess + String.Format("  COACode {0}. SegmentNumber {1}. SegmentValue {2}. Date {3}", coaCode, i, segVal, Convert.ToDateTime(JEDate).ToShortDateString());
                            switch (action)
                            {
                                case VR_ACTION.Error:
                                    {
                                        this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, hBuff), Compatibility.Convert.ToString(hBuff["BookID"])), true);
                                        AddErrorToCache(cacheIndex, errMess, true);
                                        lResult = false;
                                    }
                                    break;
                                case VR_ACTION.Warning:
                                    {
                                        this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, hBuff), Compatibility.Convert.ToString(hBuff["BookID"])));
                                        AddErrorToCache(cacheIndex, errMess, false);
                                    }
                                    break;
                            }
                            break;
                        }
                    }
                }

                /*COA Reference type values is wrong*/
                errRef = false;

                int lastControlledSegmentNbr = this.FindLastControlledCOASegment(company, coaCode);
                var listDinamicSegments = SelectDynamicCOASegment(company, coaCode);

                foreach (var coaSegment in SelectCOASegment_GLCOARefType(company, coaCode))
                {
                    string segValue = getSegment(hBuff, coaSegment.SegmentNbr);
                    string refType = "";
                    IEnumerable<GLCOARefType> listGLCOARefType = null;

                    if (!string.IsNullOrEmpty(segValue))
                    {
                        var COASegValues = FindFirstCOASegValues_RefEntityType(company, coaCode, coaSegment.SegmentNbr, segValue);
                        if (COASegValues == null)
                        {
                            errRef = true;
                            errMess = errMess + Strings.ReferenceCodeInvalidForThisAccount(segValue, coaCode, coaSegment.SegmentNbr);
                            break;
                        }
                        refType = COASegValues.RefEntityType;

                        listGLCOARefType = SelectGLCOARefType(company, coaCode, coaSegment.SegmentNbr, refType);
                        if (listGLCOARefType == null)
                        {
                            errRef = true;
                            errMess = errMess + Strings.ReferenceCodeInvalidForThisAccount(segValue, coaCode, coaSegment.SegmentNbr);
                            break;
                        }
                    }
                    else
                    {
                        listGLCOARefType = SelectGLCOARefType(company, coaCode, coaSegment.SegmentNbr);
                        if (listGLCOARefType == null)
                            continue;
                    }

                    bool MatchingIsFound = false;

                    #region Check GLCOARefAcct for this GL Reference Type
                    foreach (var GLCOARefTypeRow in listGLCOARefType)
                    {
                        foreach (var GLCOARefAcctRow1 in (this.SelectGLCOARefAcct(company, GLCOARefTypeRow.COACode, GLCOARefTypeRow.SegmentNbr, GLCOARefTypeRow.RefType)))
                        {
                            lMatches = true;
                            /* Checking for account mask */
                            for (i = 1; i <= lastControlledSegmentNbr; i++)
                            {
                                if (listDinamicSegments.Contains(i))
                                    continue;

                                string source = getSegment(hBuff, i);
                                string sourceRef = getSegment(GLCOARefAcctRow1, i);

                                if (!Erp.Internal.GL.GLAccountCore.SegmentMaskMatch(source, sourceRef))
                                {
                                    lMatches = false;
                                    break;
                                }
                            }

                            if (!lMatches)
                                continue;

                            MatchingIsFound = true;

                            switch (GLCOARefAcctRow1.RefStatus.ToUpperInvariant())
                            {
                                case "R":
                                    if (String.IsNullOrEmpty(segValue))
                                    {
                                        errRef = true;
                                        errMess = errMess + Strings.ReferSegmentIsEmptyCOACodeSegmeHBuffCOACode(GLCOARefAcctRow1.COACode, GLCOARefAcctRow1.SegmentNbr);
                                    }
                                    break;
                                case "E":
                                    if (!String.IsNullOrEmpty(segValue))
                                    {
                                        errRef = true;
                                        errMess = errMess + Strings.ReferSegmentIsNotEmptyCOACodeSegmeHBuffCOACode(GLCOARefAcctRow1.COACode, GLCOARefTypeRow.SegmentNbr);
                                    }
                                    break;
                            }

                            if (errRef)
                                break;
                        }
                    }
                    #endregion

                    if (!errRef && !MatchingIsFound && !string.IsNullOrEmpty(segValue))
                    {
                        errRef = true;
                        errMess = errMess + Strings.ReferenceCodeInvalidForThisAccount(segValue, coaCode, coaSegment.SegmentNbr);
                    }

                    if (errRef)
                        break;
                }

                if (errRef)
                {
                    switch (action)
                    {
                        case VR_ACTION.Error:
                            {
                                this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, hBuff), Compatibility.Convert.ToString(hBuff["BookID"])), true);
                                AddErrorToCache(cacheIndex, errMess, true);
                                lResult = false;
                            }
                            break;
                        case VR_ACTION.Warning:
                            {
                                this._PEData.PEError.AddWarning(new RuleException(FormatErrMsgWithLineNum(errMess, hBuff), Compatibility.Convert.ToString(hBuff["BookID"])));
                                AddErrorToCache(cacheIndex, errMess, false);
                            }
                            break;
                    }
                }
            }
            return lResult;
        }


        public bool SelfBalSegNotBalNextYear_Err(string errMess)
        {
            return true;
        }

        public bool setbufPEBook(string currBook, ref Erp.Tables.GLBook peBook)
        {
            bool result = false;
            if (peBook != null && peBook.Company.KeyEquals(Session.CompanyID) && peBook.BookID.KeyEquals(currBook))
            {
                return true;
            }
            else
            {
                peBook = this.FindFirstGLBook(Session.CompanyID, currBook);
                result = (peBook != null);
            }

            return result;
        }

        /*------------------------------------------------------------------------------
            Purpose: Transaction and book amounts has different signs (one is Dr, and other is Cr)
            Notes: validation type = GlobLevel_TransAndBookAmtHasDiffSigns
        ------------------------------------------------------------------------------*/
        public bool TransAndBookAmtHasDiffSigns_Err(string errMess, procRvJrnTrDtl inRow)
        {

            bool lResult = true;
            bool lTrAmtIsPositive = true;
            bool lBkAmtIsPositive = true;
            bool lBothZero = false;
            if (inRow.IsRevaluationTran == true)
            {
                return lResult;
            }

            var action = GetGlobLevelAction(ValidationType.GlobLevel_TransAndBookAmtHasDiffSigns);
            if (action != VR_ACTION.Ignore)
            {
                if (inRow.DebitAmount != 0 || inRow.BookDebitAmount != 0)
                {
                    lTrAmtIsPositive = inRow.DebitAmount > 0;
                    lBkAmtIsPositive = inRow.BookDebitAmount > 0;
                    if (inRow.DebitAmount == 0 || inRow.BookDebitAmount == 0)
                    {
                        lTrAmtIsPositive = lBkAmtIsPositive;
                    }
                }
                else
                {
                    lTrAmtIsPositive = (inRow.CreditAmount > 0);
                    lBkAmtIsPositive = (inRow.BookCreditAmount > 0);
                    if (inRow.CreditAmount == 0 || inRow.BookCreditAmount == 0)
                    {
                        lTrAmtIsPositive = lBkAmtIsPositive;
                    }
                }
                lBothZero = (inRow.CreditAmount == 0 && inRow.BookCreditAmount == 0 && inRow.DebitAmount == 0 && inRow.BookDebitAmount == 0 && inRow.DebitStatAmt == 0 && inRow.CreditStatAmt == 0);
                if ((lTrAmtIsPositive != lBkAmtIsPositive) || (lBothZero && !(inRow.IsSummarize == true && inRow.ParentLine == 0)))
                {
                    this._PEData.PEError.AddException(new RuleException(errMess, inRow.BookID), true);
                }
            }
            return lResult;
        }

        /*------------------------------------------------------------------------------
            Purpose:  Transaction line is posted as not Red Storno transaction, 
            but the amount is negative

            Notes: validation type = Globlevel_WrongSignOfAmountForDbtOrCrt
        ------------------------------------------------------------------------------*/
        public bool WrongSignOfAmountForDbtOrCrt_Err(string errMess, procRvJrnTrDtl inRow)
        {
            if (inRow.RedStorno)
                return true;

            if (inRow.BookDebitAmount >= 0 && inRow.BookCreditAmount >= 0 && inRow.DebitAmount >= 0 && inRow.CreditAmount >= 0)
                return true;

            bool lResult = true;
            var action = GetRuleLevelAction(ValidationType.Rulelevel_WrongSignOfAmountForDbtOrCrt, inRow.BRuleUID);
            switch (action)
            {
                case VR_ACTION.Error:
                    errMess = errMess.Entry(0, '|');
                    lResult = false;
                    if (inRow.BookDebitAmount < 0 || inRow.BookCreditAmount < 0)
                        this._PEData.PEError.AddException(new RuleException(String.Format(FormatErrMsgWithLineNum(errMess, inRow), "Book"), inRow.BookID), true);

                    if (inRow.DebitAmount < 0 || inRow.CreditAmount < 0)
                        this._PEData.PEError.AddException(new RuleException(String.Format(FormatErrMsgWithLineNum(errMess, inRow), "Transactional"), inRow.BookID), true);

                    break;
                case VR_ACTION.Autocorrect_with_Warning:
                    errMess = errMess.Entry(1, '|');
                    if (inRow.BookDebitAmount < 0 || inRow.BookCreditAmount < 0)
                    {
                        if (inRow.BookDebitAmount < 0)
                            this._PEData.PEError.AddWarning(new RuleException(String.Format(FormatErrMsgWithLineNum(errMess, inRow), "Debit", "Book", "Credit"), inRow.BookID));
                        else
                            this._PEData.PEError.AddWarning(new RuleException(String.Format(FormatErrMsgWithLineNum(errMess, inRow), "Credit", "Book", "Debit"), inRow.BookID));

                        var dBookCreditAmount = inRow.BookCreditAmount;
                        var dBookDebitAmount = inRow.BookDebitAmount;
                        inRow.BookCreditAmount = -dBookDebitAmount;
                        inRow.BookDebitAmount = -dBookCreditAmount;
                    }

                    if (inRow.DebitAmount < 0 || inRow.CreditAmount < 0)
                    {
                        if (inRow.DebitAmount < 0)
                            this._PEData.PEError.AddWarning(new RuleException(String.Format(FormatErrMsgWithLineNum(errMess, inRow), "Debit", "Transactional", "Credit"), inRow.BookID));
                        else
                            this._PEData.PEError.AddWarning(new RuleException(String.Format(FormatErrMsgWithLineNum(errMess, inRow), "Credit", "Transactional", "Debit"), inRow.BookID));

                        var dCreditAmount = inRow.CreditAmount;
                        var dDebitAmount = inRow.DebitAmount;
                        inRow.CreditAmount = -dDebitAmount;
                        inRow.DebitAmount = -dCreditAmount;
                    }

                    break;
            }

            return lResult;
        }
        /*------------------------------------------------------------------------------
            Purpose:  Transaction line is posted as Red Storno, 
                      but the amount is positive OR Transaction line is posted as not RedStorno, 
                      but the amount is negative

            Notes: validation type = RuleLevel_WrongSignOfAmount
        ------------------------------------------------------------------------------*/
        public bool WrongSignOfAmountForRedStorno_Err(string errMess, procRvJrnTrDtl inRow)
        {
            if (!inRow.RedStorno)
                return true;

            if (inRow.BookDebitAmount <= 0 && inRow.BookCreditAmount <= 0 && inRow.DebitAmount <= 0 && inRow.CreditAmount <= 0)
                return true;

            bool lResult = true;

            VR_ACTION action = VR_ACTION.Ignore;
            if (inRow.BRuleUID == 0 && inRow.IsSummarize)
                action = VR_ACTION.Autocorrect;
            else
                action = GetRuleLevelAction(ValidationType.Rulelevel_WrongSignOfAmountForRedStorno, inRow.BRuleUID);

            switch (action)
            {
                /* Error */
                case VR_ACTION.Error:
                    lResult = false;

                    if ((inRow.BookDebitAmount > 0 || inRow.BookCreditAmount > 0))
                        this._PEData.PEError.AddException(new RuleException(String.Format(FormatErrMsgWithLineNum(errMess, inRow), "Book"), inRow.BookID), true);


                    if ((inRow.DebitAmount > 0 || inRow.CreditAmount > 0))
                        this._PEData.PEError.AddException(new RuleException(String.Format(FormatErrMsgWithLineNum(errMess, inRow), "Transactional"), inRow.BookID), true);

                    break;
                /* Auto-correction */
                case VR_ACTION.Autocorrect:
                    if ((inRow.BookDebitAmount > 0 || inRow.BookCreditAmount > 0))
                    {
                        var dBookCreditAmount = inRow.BookCreditAmount;
                        var dBookDebitAmount = inRow.BookDebitAmount;
                        inRow.BookCreditAmount = -dBookDebitAmount;
                        inRow.BookDebitAmount = -dBookCreditAmount;

                        var procRvTranGLCRow = (from procRvTranGLC_Row in this._PEData.ProcRvTranGLCRows.GetItemsByTranSeqIndex(inRow.BookID, inRow.TranSeq)
                                                where procRvTranGLC_Row.RvJrnTrUID == inRow.RvJrnTrUID
                                                select procRvTranGLC_Row).FirstOrDefault();
                        if (procRvTranGLCRow != null)
                            procRvTranGLCRow.TGLCTranNum = (-1) * (procRvTranGLCRow.TGLCTranNum - 1);
                    }

                    if ((inRow.DebitAmount > 0 || inRow.CreditAmount > 0))
                    {
                        var dCreditAmount = inRow.CreditAmount;
                        var dDebitAmount = inRow.DebitAmount;
                        inRow.CreditAmount = -dDebitAmount;
                        inRow.DebitAmount = -dCreditAmount;
                    }
                    break;
            }


            return lResult;
        }

        private object getGLAccount(IRow hHandle)
        {
            return hHandle is Erp.Tables.GLAccount ? hHandle["GLAccount1"] : hHandle["GLAccount"];
        }

        public bool CheckStatData(string errMess, procRvJrnTrDtl inRow)
        {
            bool result = true;

            var COASegValues = FindFirstCOASegValuesStatistical(inRow.Company, inRow.COACode, 1, inRow.SegValue1);
            if (COASegValues != null)
            {
                if (COASegValues.Statistical == 0)
                {
                    if (inRow.CreditStatAmt != 0 || inRow.DebitStatAmt != 0)
                    {
                        this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, inRow) + Strings.Nonzerostatisticalamountfornonstatisticalaccount, inRow.BookID), true);
                        result = false;
                    }

                    if (inRow.StatUOMCode.Length != 0)
                    {
                        this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, inRow) + Strings.NotEmptyStatisctialCode, inRow.BookID), true);
                        result = false;
                    }

                }
                else if (COASegValues.Statistical == 1) //Mixed
                {
                    if (inRow.StatUOMCode.Length > 0)
                    {
                        if (!ExistStatUOM(inRow.Company, inRow.StatUOMCode))
                        {
                            this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, inRow) + Strings.StatisticalCodeWrong(inRow.StatUOMCode), inRow.BookID), true);
                            result = false;
                        }
                    }
                }
                else if (COASegValues.Statistical == 2) // Statistical Only
                {
                    if (inRow.CreditAmount != 0 || inRow.DebitAmount != 0 || inRow.BookCreditAmount != 0 || inRow.BookDebitAmount != 0)
                    {
                        this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, inRow) + Strings.Nonzerofinancialamountforastatisticalaccount, inRow.BookID), true);
                        result = false;
                    }


                    if (inRow.StatUOMCode.Length > 0)
                    {
                        if (!ExistStatUOM(inRow.Company, inRow.StatUOMCode))
                        {
                            this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, inRow) + Strings.StatisticalCodeWrong(inRow.StatUOMCode), inRow.BookID), true);
                            result = false;
                        }

                        if (inRow.CreditStatAmt == 0 && inRow.DebitStatAmt == 0)
                        {
                            this._PEData.PEError.AddException(new RuleException(FormatErrMsgWithLineNum(errMess, inRow) + Strings.Zerostatisticalamount, inRow.BookID), true);
                            result = false;
                        }

                    }
                }

            }

            return result;
        }

        private string getSegment(GLCOARefAcct row, int i)
        {
            switch (i)
            {
                case 1: return row.SegValue1;
                case 2: return row.SegValue2;
                case 3: return row.SegValue3;
                case 4: return row.SegValue4;
                case 5: return row.SegValue5;
                case 6: return row.SegValue6;
                case 7: return row.SegValue7;
                case 8: return row.SegValue8;
                case 9: return row.SegValue9;
                case 10: return row.SegValue10;
                case 11: return row.SegValue11;
                case 12: return row.SegValue12;
                case 13: return row.SegValue13;
                case 14: return row.SegValue14;
                case 15: return row.SegValue15;
                case 16: return row.SegValue16;
                case 17: return row.SegValue17;
                case 18: return row.SegValue18;
                case 19: return row.SegValue19;
                case 20: return row.SegValue20;
            }
            return string.Empty;
        }

        private string getSegment(IRow hHandle, int i)
        {
            if (hHandle is IGLAccount)
            {
                IGLAccount row = (IGLAccount)hHandle;
                switch (i)
                {
                    case 1: return row.SegValue1;
                    case 2: return row.SegValue2;
                    case 3: return row.SegValue3;
                    case 4: return row.SegValue4;
                    case 5: return row.SegValue5;
                    case 6: return row.SegValue6;
                    case 7: return row.SegValue7;
                    case 8: return row.SegValue8;
                    case 9: return row.SegValue9;
                    case 10: return row.SegValue10;
                    case 11: return row.SegValue11;
                    case 12: return row.SegValue12;
                    case 13: return row.SegValue13;
                    case 14: return row.SegValue14;
                    case 15: return row.SegValue15;
                    case 16: return row.SegValue16;
                    case 17: return row.SegValue17;
                    case 18: return row.SegValue18;
                    case 19: return row.SegValue19;
                    case 20: return row.SegValue20;
                }
            }
            return string.Empty;
        }


        public void CheckUDFieldsDataDefinitions()
        {

            if (!this.UDFieldCheckHelper.StructureCheckProcessed)
            {
                this.UDFieldCheckHelper.CheckUDForRestrictedTables();
                // Check GLJrnDtl UD fields
                this.UDFieldCheckHelper.CheckUDFieldsGLJrnDtl();
                this.UDFieldCheckHelper.CheckUDFieldTranGLC();
                this.UDFieldCheckHelper.StructureCheckProcessed = true;
            }

            // Raise definition errors
            var errors = from row in this.UDFieldCheckHelper.UDFieldsDefinitionMessages where row.level == System.Diagnostics.TraceLevel.Error select row;
            foreach (var sm in errors)
            {
                _PEData.PEError.AddException(new GlobalException(sm.message), true);
            }

            // Raise definition warnings
            var warnings = from row in this.UDFieldCheckHelper.UDFieldsDefinitionMessages where row.level == System.Diagnostics.TraceLevel.Warning select row;
            foreach (var sm in warnings)
            {
                _PEData.PEError.AddWarning(new GlobalException(sm.message));
            }

            // Terminate posting in case there are errors
            if (errors.Any())
            {
                this._PEData.LogView.IsValid = false;
                _PEData.PEError.AddException(new GlobalException(Strings.NoUDFields), true);
                string exceptionMessage = _PEData.PEError.SaveInRJ(0, 0, ExceptionSavedRange.Both, "", true);
                throw new BLException(exceptionMessage);
            }

        }

        public void ProcessUDDataExceptions(string bookID, int ruleUID)
        {
            // Raise data warnings
            var dataWarnings = from row in this.UDFieldCheckHelper.UDFieldsDataMessages where row.level == System.Diagnostics.TraceLevel.Warning select row;
            foreach (var sm in dataWarnings)
            {
                _PEData.PEError.AddWarning(new PersistentRuleException(sm.message, bookID, ruleUID));
            }

            // Raise data errors
            var dataErrors = from row in this.UDFieldCheckHelper.UDFieldsDataMessages where row.level == System.Diagnostics.TraceLevel.Error select row;
            foreach (var sm in dataErrors)
            {
                _PEData.PEError.AddException(new PersistentRuleException(sm.message, bookID, ruleUID), true);
            }

            // Terminate posting in case there are errors
            if (dataErrors.Any())
            {
                this._PEData.LogView.IsValid = false;
                string exceptionMessage = _PEData.PEError.SaveInRJ(0, 0, ExceptionSavedRange.Both, "", true);
                throw new BLException(exceptionMessage);
            }
        }


        private class VNCheckInfo
        {
            public int JournalNum { get; set; }
            public int Count { get; set; }
        }

        public bool VNCheckAccountCorrespondence(IEnumerable<IRow> rows, string bookID)
        {
            Dictionary<int, VNCheckInfo> countAllByGroups = new Dictionary<int, VNCheckInfo>(),
                                 countType0ByGroups = new Dictionary<int, VNCheckInfo>(),
                                 countType1ByGroups = new Dictionary<int, VNCheckInfo>(),
                                 countType2ByGroups = new Dictionary<int, VNCheckInfo>();
            string bookDesc = FindFirstGLBookDesc(Session.CompanyID, bookID);
            foreach (IRow row in rows)
            {
                int vnAcGroup = (int)row["VNACGroup"];
                int journalNum = (int)row["JournalNum"];
                if (countAllByGroups.ContainsKey(vnAcGroup))
                    countAllByGroups[vnAcGroup].Count++;
                else
                    countAllByGroups[vnAcGroup] = new VNCheckInfo() { JournalNum = journalNum, Count = 1 };

                int vnAcType = (int)row["VNACType"];
                if (vnAcType == 0)
                {
                    if (countType0ByGroups.ContainsKey(vnAcGroup))
                        countType0ByGroups[vnAcGroup].Count++;
                    else
                        countType0ByGroups[vnAcGroup] = new VNCheckInfo() { JournalNum = journalNum, Count = 1 };
                }
                else if (vnAcType == 1)
                {
                    if (countType1ByGroups.ContainsKey(vnAcGroup))
                        countType1ByGroups[vnAcGroup].Count++;
                    else
                        countType1ByGroups[vnAcGroup] = new VNCheckInfo() { JournalNum = journalNum, Count = 1 };
                }
                else if (vnAcType == 2)
                {
                    if (countType2ByGroups.ContainsKey(vnAcGroup))
                        countType2ByGroups[vnAcGroup].Count++;
                    else
                        countType2ByGroups[vnAcGroup] = new VNCheckInfo() { JournalNum = journalNum, Count = 1 };
                }
                else
                {
                    this._PEData.PEError.AddException(new RuleException(Strings.VNCorrespondenceAccountingTypeValueShouldBeZeroOneOrTwo(vnAcGroup) + (journalNum == 0 ? "" : " " + Strings.JournalNum(journalNum)), bookID), true);
                    return false;
                }
            }
            foreach (KeyValuePair<int, VNCheckInfo> pairTypeAll in countAllByGroups)
            {
                if (pairTypeAll.Value.Count == 1)
                {
                    this._PEData.PEError.AddException(new RuleException(Strings.VNCorrespondenceAccountingGroupIncludesOneLineOnly(pairTypeAll.Key) + (pairTypeAll.Value.JournalNum == 0 ? "" : " " + Strings.JournalNum(pairTypeAll.Value.JournalNum)), bookID), true);
                    return false;
                }

                if (countType0ByGroups.ContainsKey(pairTypeAll.Key) && pairTypeAll.Value.Count == countType0ByGroups[pairTypeAll.Key].Count)
                {
                    this._PEData.PEError.AddException(new RuleException(Strings.VNCorrespondenceAccountingGroupIncludesLinesWithZeroTypeOnly(pairTypeAll.Key) + (pairTypeAll.Value.JournalNum == 0 ? "" : " " + Strings.JournalNum(pairTypeAll.Value.JournalNum)), bookID), true);
                    return false;
                }
            }
            foreach (KeyValuePair<int, VNCheckInfo> pairType1 in countType1ByGroups)
            {
                if (pairType1.Value.Count > 1)
                {
                    this._PEData.PEError.AddException(new RuleException(Strings.VNCorrespondenceAccountingGroupIncludesMoreThanOneLineWithTypeOne(pairType1.Key) + (pairType1.Value.JournalNum == 0 ? "" : " " + Strings.JournalNum(pairType1.Value.JournalNum)), bookID), true);
                    return false;
                }
            }
            foreach (KeyValuePair<int, VNCheckInfo> pairType2 in countType2ByGroups)
            {
                if (pairType2.Value.Count > 2)
                {
                    this._PEData.PEError.AddException(new RuleException(Strings.DirectCorrespondenceAccountingTypeShouldHaveTwoLinesOnly(pairType2.Key) + (pairType2.Value.JournalNum == 0 ? "" : " " + Strings.JournalNum(pairType2.Value.JournalNum)), bookID), true);
                    return false;
                }
                if (countType0ByGroups.ContainsKey(pairType2.Key) ||
                    countType1ByGroups.ContainsKey(pairType2.Key))
                {
                    this._PEData.PEError.AddException(new RuleException(Strings.VNMixtureOfDirectAndOneToManyCorrespondenceAccountingTypesIsNotAllowed(pairType2.Key) + (pairType2.Value.JournalNum == 0 ? "" : " " + Strings.JournalNum(pairType2.Value.JournalNum)), bookID), true);
                    return false;
                }
            }
            return true;
        }
    }
}
