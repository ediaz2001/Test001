//==========================================================================================
//FixUpSvcConversion: Strings Extraction Completed at: 3/19/2012 11:10 AM
//==========================================================================================
/* Validation rules */
/*------------------------------------------------------------------------ 
File        :  
Author(s)   :  
Created     :
    
/// <summary>
///Validation Rules
/// </summary>
History     :         
03/17/06   Nernov scr49309 - Several rules are excluded from ValidationRuleType table 
           which are not needed in Ver9.0     
04/02/08  VictorI scr 49545 - Several Rules messages should be corrected. Added new field to store messages, that should 
          be put to log. If there are several messages, they are separeted by "|".
04/03/08  VictorI SCR 49552 Obsolete rule is removed - 5.2 Book is a consolidation book
05/22/08  TatyanaK SCR49080 - Obsolete function and field "UserDefined" was removed.
08/30/09  VladimirD SCR 64008 - modified segment value global error text.
09/28/09  VladimirD SCR 65935 - fixed error text for 'different signs' validation rule.
04/29/10  TatyanaK SCR 74568 - Rule "Transaction amount is zero for currency account" was moved to rule level.
05/13/10  TatyanaK SCR 75240 - Validation type numbers were corrected.
05/14/10  VladimirJ SCR 74568 - add a comment about pe/cvpe0000.p
08/24/10 IrinaY    SCR 77803 - move data initialize in precudure to fill data correctly in cv programs and added procedures for copy data from another booking rule and remove outdated records.
09/12/10 IrinaY    SCR 74880 - Added new validation rule 'Red storno amount is positive'.
09/13/10 IrinaY    SCR 74880 - Default action for rule 'Red storno amount is positive' should be 'Autocorrect'. 
----------------------------------------------------------------------*/


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Epicor.Data;
using Erp;
using Erp.Tables;
using Ice;
#if USE_EF_CORE
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace Erp.Internal.PE
{

    public enum VR_LEVEL
    {
        BOOK,
        RULE,
        GLOB
    }

    public enum VR_ACTION
    {
        Undefined,
        Error,
        Warning,
        Ignore,
        Autocorrect,
        Autocorrect_with_Warning
    }

    public static class ActionParse
    {
        public static VR_ACTION ToEnum(string value)
        {
            string v = value.Replace(' ', '_');
            VR_ACTION retValue;
            if (Enum.TryParse<VR_ACTION>(v, out retValue))
            {
                return retValue;
            }
            else
            {
                return VR_ACTION.Undefined;
            }
        }
        public static string ToString(VR_ACTION value)
        {
            return value.ToString().Replace('_', ' ');
        }
    }



    public enum ValidationType
    {
        /* Define book-level validation types */
        BookLevel_None = 0, 
        BookLevel_SegmentNotUsed = 1,
        BookLevel_UOMExistsAccNotDefined = 2,
        BookLevel_QtyZeroAmtNotZero = 3,
        BookLevel_NonStatAcc = 4,
        BookLevel_ClosingPeriodNotExists = 5,
        BookLevel_ApplyDateOutOfRange = 6,
        BookLevel_FiscalPeriodClosed = 7,
        BookLevel_SelfBalSegNotBal = 8,
        BookLevel_TransNotBiLine = 9,
        BookLevel_SelfBalSegNotBalNextYear = 10,
        BookLevel_UOMNotSpecForStatAcc = 11,
        BookLevel_UOMNotConformStatAccDef = 12,
        BookLevel_QtyZeroForStatAcc = 13,
        BookLevel_AccNotDefinedAsCurAcc = 14,
        BookLevel_CurAmntSpecForNon_curAcc = 15,

        /* Define rule-level validation types */
        RuleLevel_ABTLineNotExists = 16,
        RuleLevel_TransLineNotCreated = 17,
        Rulelevel_WrongSignOfAmountForDbtOrCrt = 18,
        RuleLevel_CurAmntZeroBookCurAmntNotZero = 19,
        RuleLevel_RuleProducedSingleLine = 20,
        RuleLevel_RuleProducedDebit_CreditLines = 21,
        RuleLevel_CurAmountZeroForCurAcc = 22,
        Rulelevel_WrongSignOfAmountForRedStorno = 23,

        /* Define glob-level validation types */
        GlobLevel_SegmentValue = 24,
        GlobLevel_ReqDynSegNotSpec = 25,
        GlobLevel_CombControlSegInvalid = 26,
        GlobLevel_AccInactive = 27,
        GlobLevel_CurNotSpecForCurAcc = 28,       /* Is not used */
        GlobLevel_CurNotConformCurAccDef = 29,
        GlobLevel_FiscalParamNotBasedOnApplyDate = 30,
        GlobLevel_BookNotExist = 31,
        GlobLevel_BookIConsolidated = 32,        /*Is not used*/
        GlobLevel_PostToBookDisabled = 33,
        GlobLevel_JournalCodeWrong = 34,
        GlobLevel_TransNotBalance = 35,
        GlobLevel_RedStornoDisabled = 36,
        GlobLevel_REAccountNotMatchToMask = 37,
        GlobLevel_REAccountWrong = 38,
        GlobLevel_MapNotFound = 39,
        GlobLevel_MapTargetSegNotFound = 40,
        GlobLevel_MapTargetAccNotFound = 41,
        GlobLevel_TransAndBookAmtHasDiffSigns = 42,
        GlobLevel_QtyAndAmtHasDiffSigns = 43,
        GlobLevel_LookupTableNotFound = 44,
        GlobLevel_AuthorizedSite = 45
    }



    public class ValidationRuleType
    {
        public ValidationType TypeUID { get; set; }
        public string Description { get; set; }
        public VR_LEVEL Level { get; set; }
        public VR_ACTION[] ActionsList { get; set; }
        public VR_ACTION ActionDefault { get; set; }
        public string ErrMessage { get; set; }


        public ValidationRuleType(ValidationType typeUID, string description, VR_LEVEL level, VR_ACTION[] actions, VR_ACTION defAction, string errMessage)
        {
            this.TypeUID = typeUID;
            this.Description = description;
            this.Level = level;
            this.ActionsList = actions;
            this.ActionDefault = defAction;
            this.ErrMessage = errMessage;
        }
    }

    public class ValidationRuleTypeCollection : List<ValidationRuleType>
    {

        private void AddValidation(ValidationType idType, VR_LEVEL level, string description, string error, VR_ACTION[] actions, VR_ACTION defAction)
        {
            this.Add(new ValidationRuleType(
                idType,
                description,
                level,
                actions,
                defAction,
                error
            ));

        }

        public void AddGlobalLevelValidation(ValidationType idType, string description, string error, VR_ACTION[] actions)
        {
            this.AddValidation(idType, VR_LEVEL.GLOB, description, error, actions, actions[0]);
        }

        public void AddBookLevelValidation(ValidationType idType, string description, string error, VR_ACTION[] actions)
        {
            this.AddValidation(idType, VR_LEVEL.BOOK, description, error, actions, actions[0]);
        }

        public void AddRuleLevelValidation(ValidationType idType, string description, string error, VR_ACTION[] actions)
        {
            this.AddValidation(idType, VR_LEVEL.RULE, description, error, actions, actions[0]);
        }

    }


    public partial class BVRule : Ice.Libraries.ContextLibraryBase<ErpContext>
    {
        public ValidationRuleType ValidationRuleType;
        public ValidationRuleTypeCollection ValidationRuleTypeRows;

        public BVRule(ErpContext ctx) : base(ctx)
        {
            this.Initialize();
            
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (ValidationRuleTypeRows != null)
                {
                    ValidationRuleTypeRows.Clear();
                    ValidationRuleTypeRows = null;
                }
                ValidationRuleType = null;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            initValudationRules();
        }

        /* Rule */
        public void initValudationRules()
        {
            /* load ValidationRule table */
            ValidationRuleTypeRows = new ValidationRuleTypeCollection();
            //this.ValidationRuleTypeRows.Clear();

            // **********************
            // Book rules
            // **********************

            // 1
            this.ValidationRuleTypeRows.AddBookLevelValidation(
                ValidationType.BookLevel_SegmentNotUsed,
                "Segment is defined as 'not used', but Segment value is specified (depends on natural account)",
                "Segment {0} is defined as 'not used', but segment value is specified",
                new VR_ACTION[] { VR_ACTION.Autocorrect_with_Warning, VR_ACTION.Error, VR_ACTION.Autocorrect }
            );

            // 2
            //ValidationRuleTypeRows.AddBookLevelValidation(
            //    ValidationType.BookLevel_UOMExistsAccNotDefined,
            //    "UOM is specified, but account is not defined as statistical account",
            //    "UOM is specified, but account is not defined as statistical account",
            //    new VR_ACTION[] { VR_ACTION.Ignore, VR_ACTION.Warning, VR_ACTION.Error }
            //);

            // 3
            //ValidationRuleTypeRows.AddBookLevelValidation(
            //    ValidationType.BookLevel_QtyZeroAmtNotZero,
            //    "Quantity is zero, but amount is not zero (for mixed account)",
            //    "Quantity is zero, but amount is not zero (for mixed account)",
            //    new VR_ACTION[] { VR_ACTION.Ignore, VR_ACTION.Warning, VR_ACTION.Error }
            //);

            // 4
            //ValidationRuleTypeRows.AddBookLevelValidation(
            //    ValidationType.BookLevel_NonStatAcc,
            //    "Quantity is specified for non-statistical account",
            //    "Quantity is specified for non-statistical account",
            //    new VR_ACTION[] { VR_ACTION.Ignore, VR_ACTION.Warning, VR_ACTION.Error }
            //);

            // 5
            ValidationRuleTypeRows.AddBookLevelValidation(
                ValidationType.BookLevel_ClosingPeriodNotExists,
                "Closing Period is specified, but does not exist",
                "Closing Period is specified, but does not exist",
                new VR_ACTION[] { VR_ACTION.Error, VR_ACTION.Autocorrect_with_Warning }
            );


            // 6
            ValidationRuleTypeRows.AddBookLevelValidation(
                ValidationType.BookLevel_ApplyDateOutOfRange,
                "Apply Date is earlier than Earliest Apply Date",
                "Apply Date is earlier than Earliest Apply Date",
                new VR_ACTION[] { VR_ACTION.Autocorrect_with_Warning, VR_ACTION.Error, VR_ACTION.Ignore }
            );

            // 7
            ValidationRuleTypeRows.AddBookLevelValidation(
                ValidationType.BookLevel_FiscalPeriodClosed,
                "Fiscal Period is closed",
                "Fiscal Period is closed",
                new VR_ACTION[] { VR_ACTION.Autocorrect_with_Warning, VR_ACTION.Error, VR_ACTION.Ignore }
            );

            // 8
            ValidationRuleTypeRows.AddBookLevelValidation(
                ValidationType.BookLevel_SelfBalSegNotBal,
                "Self-balancing segment does not balance",
                "Self-balancing segment {0} does not balance",
                new VR_ACTION[] { VR_ACTION.Autocorrect, VR_ACTION.Warning, VR_ACTION.Error }
            );

            // 9
            ValidationRuleTypeRows.AddBookLevelValidation(
                ValidationType.BookLevel_TransNotBiLine,
                "Accounting correspondence is defined",
                "Transaction doesn't meet the correspondence accounting criteria",
                new VR_ACTION[] { VR_ACTION.Ignore, VR_ACTION.Error }
            );

            // 10
            ValidationRuleTypeRows.AddBookLevelValidation(
                ValidationType.BookLevel_SelfBalSegNotBalNextYear,
                "Next Year OB - Self-balancing segment does not balance",
                "Next Year OB - Self-balancing segment does not balance",
                new VR_ACTION[] { VR_ACTION.Autocorrect, VR_ACTION.Warning, VR_ACTION.Error }
            );

            // 11
            //ValidationRuleTypeRows.AddBookLevelValidation(
            //    ValidationType.BookLevel_UOMNotSpecForStatAcc,
            //    "UOM is not specified for statistical account",
            //    "UOM is not specified for statistical account",
            //    new VR_ACTION[] { VR_ACTION.Error }
            //);

            // 12
            //ValidationRuleTypeRows.AddBookLevelValidation(
            //    ValidationType.BookLevel_UOMNotConformStatAccDef,
            //    "UOM is specified, but does not conform to statistical account definition",
            //    "UOM is specified, but does not conform to statistical account definition",
            //    new VR_ACTION[] { VR_ACTION.Error }
            //);

            // 13
            //ValidationRuleTypeRows.AddBookLevelValidation(
            //    ValidationType.BookLevel_QtyZeroForStatAcc,
            //    "Quantity is zero for statistical account",
            //    "Quantity is zero for statistical account",
            //    new VR_ACTION[] { VR_ACTION.Error }
            //);

            // 14
            //ValidationRuleTypeRows.AddBookLevelValidation(
            //    ValidationType.BookLevel_AccNotDefinedAsCurAcc,
            //    "Transaction currency is specified, but account is not defined as currency account",
            //    "Transaction currency is specified, but account is not defined as currency account",
            //    new VR_ACTION[] { VR_ACTION.Ignore, VR_ACTION.Warning, VR_ACTION.Error }
            //);

            // 15 Is not used (obsolete)
            //ValidationRuleTypeRows.AddBookLevelValidation(
            //    ValidationType.BookLevel_CurAmntSpecForNon_curAcc,
            //    "Transaction currency amount is specified for non-currency account",
            //    "Transaction currency amount is specified for non-currency account",
            //    new VR_ACTION[] { VR_ACTION.Ignore, VR_ACTION.Warning, VR_ACTION.Error }
            //);


            // ***********************************
            // Booking Rules rules
            // ***********************************

            // 1
            ValidationRuleTypeRows.AddRuleLevelValidation(
                ValidationType.RuleLevel_ABTLineNotExists,
                "Rule didn't have an ABT line to process",
                "Rule '{0}' didn't have an ABT line to process",
                new VR_ACTION[] { VR_ACTION.Ignore, VR_ACTION.Warning, VR_ACTION.Error }
            );

            // 2
            ValidationRuleTypeRows.AddRuleLevelValidation(
                ValidationType.RuleLevel_TransLineNotCreated,
                "Rule was invoked but didn't produce a transaction line",
                "Rule '{0}' was executed but didn't produce a complete transaction line. {1} not defined",  /* {0} - rule name, {1} - account(s) amount(s) was/were */
                new VR_ACTION[] { VR_ACTION.Ignore, VR_ACTION.Warning, VR_ACTION.Error }
            );

            // 3
            ValidationRuleTypeRows.AddRuleLevelValidation(
                ValidationType.Rulelevel_WrongSignOfAmountForDbtOrCrt,
                "Amount is negative (not for red storno)",
                "Amount in {0} currency is negative|Negative {0} amount in {1} currency is posted to {2}",  
                new VR_ACTION[] { VR_ACTION.Error, VR_ACTION.Autocorrect_with_Warning }
            );

            // 4
            ValidationRuleTypeRows.AddRuleLevelValidation(
                ValidationType.RuleLevel_CurAmntZeroBookCurAmntNotZero,
                "Transaction amount is zero, but book amount is not zero",
                "Transaction amount is zero, but book amount is not zero",
                new VR_ACTION[] { VR_ACTION.Error, VR_ACTION.Warning, VR_ACTION.Ignore }
            );

            // 5
            ValidationRuleTypeRows.AddRuleLevelValidation(
                ValidationType.RuleLevel_RuleProducedSingleLine,
                "Rule produced single transaction line",
                "Rule '{0}' is expected to produce two transactions lines, but has produced only one",
                new VR_ACTION[] { VR_ACTION.Ignore, VR_ACTION.Warning, VR_ACTION.Error }
            );

            // 6
            ValidationRuleTypeRows.AddRuleLevelValidation(
                ValidationType.RuleLevel_RuleProducedDebit_CreditLines,
                "Rule produced debit/credit transaction lines",
                "Rule '{0}' produced debit/credit transaction lines",
                new VR_ACTION[] { VR_ACTION.Ignore, VR_ACTION.Warning, VR_ACTION.Error }
            );

            // 7
            ValidationRuleTypeRows.AddRuleLevelValidation(
                ValidationType.RuleLevel_CurAmountZeroForCurAcc,
                "Transaction amount is zero for currency account",
                "Transaction amount is zero for currency account",
                new VR_ACTION[] { VR_ACTION.Ignore, VR_ACTION.Warning, VR_ACTION.Error }
            );

            // 8
            ValidationRuleTypeRows.AddRuleLevelValidation(
                ValidationType.Rulelevel_WrongSignOfAmountForRedStorno,
                "Red storno amount is positive",
                "Amount in {0} currency is positive in a red storno transaction",
                new VR_ACTION[] { VR_ACTION.Autocorrect, VR_ACTION.Error }
            );


            // ***************************************
            //  Global rules
            // ***************************************

            // 1
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_SegmentValue,
                "Segment value is wrong or inactive",
                "Segment value is wrong or inactive",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 2
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_ReqDynSegNotSpec,
                "Required dynamic segment is not specified",
                "Required dynamic segment is not specified",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 3
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_CombControlSegInvalid,
                "Combination of controlled segments does not exist in COA",
                "Combination of controlled segments is invalid",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 4
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_AccInactive,
                "Account is inactive",
                "Account is inactive",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 5  Is not used (obsolete)
            //ValidationRuleTypeRows.AddGlobalLevelValidation(
            //    ValidationType.GlobLevel_CurNotSpecForCurAcc,
            //    "Transaction currency is not specified for currency account",
            //    "Transaction currency is not specified for currency account",
            //    new VR_ACTION[] { VR_ACTION.Error }
            //);

            // 6
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_CurNotConformCurAccDef,
                "Transaction currency is specified, but does not conform to currency account definition",
                "Transaction currency is specified, but does not conform to currency account definition",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 7
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_FiscalParamNotBasedOnApplyDate,
                "Fiscal Year/Period can not be determined based on Apply Date",
                "Fiscal Year/Period can not be determined based on Apply Date",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 8
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_BookNotExist,
                "Book does not exist",
                "Book does not exist",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 9   Is not used (obsolete)
            //ValidationRuleTypeRows.AddGlobalLevelValidation(
            //    ValidationType.GlobLevel_BookIConsolidated,
            //    "Book is a consolidation book",
            //    "Book is a consolidation book",
            //    new VR_ACTION[] { VR_ACTION.Error }
            //);

            // 10 
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_PostToBookDisabled,
                "Posting to book is disabled",
                "The Book '{0}' is inactive and cannot receive GL Transactions. Please make sure it is removed from the GL Transaction Type '{1}'.",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 11 
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_JournalCodeWrong,
                "Journal Code is wrong",
                "Journal Code is wrong",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 12
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_TransNotBalance,
                "Transaction does not balance",
                "Transaction does not balance",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 13
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_RedStornoDisabled,
                "Red storno is indicated but is disabled for book",
                "Red storno is indicated but is disabled for book",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 14
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_REAccountNotMatchToMask,
                "RE account can not be determined for source account (does not match to any mask)",
                "RE account can not be determined for source account (does not match to any mask)",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 15
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_REAccountWrong,
                "RE account is wrong",
                "RE account is wrong",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 16
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_MapNotFound,
                "Map not found",
                "COA map {0} does not exist",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 17
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_MapTargetSegNotFound,
                "Target segment not found",
                "Unable to map an account. None of source segments found account: {0}",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 18
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_MapTargetAccNotFound,
                "Target account not found",
                "Unable to map an account. Can't find account mapping with source value: {0}",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 19
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_TransAndBookAmtHasDiffSigns,
                "Transaction and book amounts has different signs (one is Debit, and other is Credit)",
                "Transaction and book amounts has different signs (one is Debit, and other is Credit) or both amounts have zero values.",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            // 20
            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_QtyAndAmtHasDiffSigns,
                "Quantity and amount has different signs (one is Debit, and other is Credit)",
                "Quantity and amount has different signs (one is Debit, and other is Credit)",
                new VR_ACTION[] { VR_ACTION.Error }
            );

            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_LookupTableNotFound,
                "Lookup table not found",
                "Lookup table {0} does not exist",
                 new VR_ACTION[] { VR_ACTION.Error }
            );

            ValidationRuleTypeRows.AddGlobalLevelValidation(
                ValidationType.GlobLevel_AuthorizedSite,
                "You need to be authorized in the Site",
                "You need to be authorized in the Site {0} in order to use accounts linked to this Site",
                 new VR_ACTION[] { VR_ACTION.Error });
        }

        ///
        /* Create Validation Rules for Booking Rule.                      */
        /* If fromRuleUID > 0 then the program copies action values from  */
        /* mentioned Booking Rule                                         */
        /*---------------------------------------------------------------*/
        public void AddBRValidationRules(string pCompany, int pACTTypeUID, int pACTRevisionUID, string pBookID, int pRuleUID, int fromACTTypeUID, int fromACTRevisionUID, string fromBookID, int fromRuleUID, ref Erp.Tables.BVRule BVRule)
        {

            VR_ACTION action;

            bool isDefault = false;
            IEnumerable<BVRuleInfo> listBVRulesFrom = null;
        
            if (fromRuleUID > 0)
                listBVRulesFrom = SelectBVRuleInfo(pCompany, fromACTTypeUID, fromACTRevisionUID, fromBookID, fromRuleUID);

            foreach (var ValidationRuleType_iterator in (from ValidationRuleType_Row in ValidationRuleTypeRows
                                                         where ValidationRuleType_Row.Level == VR_LEVEL.RULE
                                                         select ValidationRuleType_Row))
            {
                ValidationRuleType ValidationRuleTypeRow = ValidationRuleType_iterator;
                action = ValidationRuleTypeRow.ActionDefault;
                isDefault = true;
                if (fromRuleUID > 0 && listBVRulesFrom != null)
                {
                    var bvRule = (from row in listBVRulesFrom
                                  where row.VRuleUID == (int)ValidationRuleTypeRow.TypeUID
                                  select row).FirstOrDefault();

                    if (bvRule != null)
                    {
                        Enum.TryParse<VR_ACTION>(bvRule.Action.Replace(' ', '_'), out action);
                        isDefault = bvRule.IsDefault;
                    }
                }

                BVRule = new Erp.Tables.BVRule();
                BVRule.Company = pCompany;
                BVRule.ACTTypeUID = pACTTypeUID;
                BVRule.ACTRevisionUID = pACTRevisionUID;
                BVRule.BookID = pBookID;
                BVRule.RuleUID = pRuleUID;

                BVRule.VLevel = ValidationRuleTypeRow.Level.ToString();
                BVRule.VRuleUID = (int)ValidationRuleTypeRow.TypeUID;
                BVRule.Description = ValidationRuleTypeRow.Description;
                BVRule.Action = action.ToString().Replace('_', ' ');
                BVRule.IsDefault = isDefault;
                Db.BVRule.Insert(BVRule);
                
            }
            Db.Validate();
        }        
    }
}
