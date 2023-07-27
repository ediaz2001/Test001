using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Epicor.Data;
using Erp.Tables;
namespace Erp.Internal.PE
{
    public partial class ValidationRule
    {


        #region ACTType Queries

        public class ACTTypeResult
        {
            public string DisplayName { get; set; }
            public bool Limited { get; set; }
        }

        static Func<ErpContext, string, string, ACTTypeResult> findFirstACTTypeQuery;
        private ACTTypeResult FindFirstACTType(string company, string abtuid)
        {
            if (findFirstACTTypeQuery == null)
            {
                Expression<Func<ErpContext, string, string, ACTTypeResult>> expression =
              (ctx, company_ex, abtUID_ex) =>
                (from row in ctx.ACTType
                 join rowAbtHead in ctx.ABTHead on new { row.Company, row.ACTTypeUID } equals new { rowAbtHead.Company, rowAbtHead.ACTTypeUID }
                 where rowAbtHead.Company == company_ex &&
                 rowAbtHead.ABTUID == abtUID_ex
                 select new ACTTypeResult() { DisplayName = row.DisplayName, Limited = row.Limited }).FirstOrDefault();
                findFirstACTTypeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstACTTypeQuery(this.Db, company, abtuid);
        }


        #endregion ACTType Queries

        #region BVRule Queries

        class BVRuleRes
        {
            public int BVRuleUID { get; set; }
            public int VRuleUID { get; set; }
            public string Action { get; set; }
        }

        private static Func<ErpContext, string, int, IEnumerable<BVRuleRes>> SelectBVRuleQuery0;
        private IEnumerable<BVRuleRes> SelectBVRule(string Company, int RuleUID)
        {
            if (SelectBVRuleQuery0 == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<BVRuleRes>>> expression =
                    (context, company_ex, ruleuid_ex) =>
                    (from row in context.BVRule
                     where
                     row.Company == company_ex &&
                     row.RuleUID == ruleuid_ex
                     select new BVRuleRes
                     {
                         BVRuleUID = row.BVRuleUID,
                         VRuleUID = row.VRuleUID,
                         Action = row.Action
                     });
                SelectBVRuleQuery0 = DBExpressionCompiler.Compile(expression);
            }
            return SelectBVRuleQuery0(this.Db, Company, RuleUID);
        }

        static Func<ErpContext, string, string, string, IEnumerable<BVRuleRes>> findFirstBVRuleQuery_2;
        private IEnumerable<BVRuleRes> SelectBVRule(string Company, string BookID, string VLevel)
        {
            if (findFirstBVRuleQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, IEnumerable<BVRuleRes>>> expression =
                  (ctx, company_ex, bookID_ex, vlevel_ex) =>
                    (from row in ctx.BVRule
                     where row.Company == company_ex &&
                     row.BookID == bookID_ex &&
                     row.VLevel == vlevel_ex
                     select new BVRuleRes
                     {
                         BVRuleUID = row.BVRuleUID,
                         VRuleUID = row.VRuleUID,
                         Action = row.Action
                     });
                findFirstBVRuleQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstBVRuleQuery_2(this.Db, Company, BookID, VLevel);
        }

        #endregion BVRule Queries

        #region GLAcctDisp Queries

        private static Func<ErpContext, string, string, string, string> GLAcctDispBoolQuery;
        private bool existsGLAcctDisp(string Company, string COACode, string GLAccount)
        {
            if (GLAcctDispBoolQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string>> expression =
                    (ctx, chk_Company, chk_CoaCode, chk_GLAccount) =>
                    (from row in ctx.GLAcctDisp
                     where row.Company == chk_Company &&
                     row.COACode == chk_CoaCode &&
                     row.GLAccount == chk_GLAccount
                     select row.Company).FirstOrDefault();
                GLAcctDispBoolQuery = DBExpressionCompiler.Compile(expression);
            }
            return (!string.IsNullOrEmpty(GLAcctDispBoolQuery(this.Db, Company, COACode, GLAccount)));
        }
        #endregion

        #region COASegAcct Queries
        static Func<ErpContext, string, string, string, string, bool, COASegAcct> existsCOASegAcctQuery;
        private bool ExistsCOASegAcct(string company, string coacode, string currencyCode, string segmentCode, bool allowed)
        {
            if (existsCOASegAcctQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, bool, COASegAcct>> expression =
              (ctx, company_ex, coacode_ex, currencyCode_ex, segmentCode_ex, allowed_ex) =>
                (from row in ctx.COASegAcct
                 where row.Company == company_ex &&
                 row.COACode == coacode_ex &&
                 row.CurrencyCode == currencyCode_ex &&
                 row.SegmentCode == segmentCode_ex &&
                 row.Allowed == allowed_ex
                 select row).FirstOrDefault();
                existsCOASegAcctQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsCOASegAcctQuery(this.Db, company, coacode, currencyCode, segmentCode, allowed) != null;
        }
        #endregion COASegAcct Queries

        #region COASegment Queries

        static Func<ErpContext, string, string, IEnumerable<int>> findLastCOASegmentQuery0;
        private IEnumerable<int> SelectDynamicCOASegment(string company, string coacode)
        {
            if (findLastCOASegmentQuery0 == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<int>>> expression =
                  (ctx, company_ex, coacode_ex) =>
                    (from row in ctx.COASegment
                     where row.Company == company_ex &&
                     row.COACode == coacode_ex &&
                     row.Dynamic == true
                     select row.SegmentNbr);
                findLastCOASegmentQuery0 = DBExpressionCompiler.Compile(expression);
            }

            return findLastCOASegmentQuery0(this.Db, company, coacode);
        }


        static Func<ErpContext, string, string, int> findLastCOASegmentQuery;
        private int FindLastControlledCOASegment(string company, string coacode)
        {
            if (findLastCOASegmentQuery == null)
            {
                Expression<Func<ErpContext, string, string, int>> expression =
                  (ctx, company_ex, coacode_ex) =>
                    (from row in ctx.COASegment
                     where row.Company == company_ex &&
                     row.COACode == coacode_ex &&
                     row.Dynamic == false
                     orderby row.SegmentNbr
                     select row.SegmentNbr).LastOrDefault();
                findLastCOASegmentQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastCOASegmentQuery(this.Db, company, coacode);
        }

        class AltCOASegmentExpressionColumnResult
        {
            public string Company { get; set; }
            public int SegmentNbr { get; set; }
            public string COACode { get; set; }
        }

        class AltCOASegmentExpression2ColumnResult
        {
            public string Company { get; set; }
            public int SegmentNbr { get; set; }
            public string COACode { get; set; }
            public int Level { get; set; }
        }

        static Func<ErpContext, string, string, bool, IEnumerable<COASegment>> selectCOASegmentQuery_3;
        private IEnumerable<COASegment> SelectCOASegment(string company, string coacode, bool dynamic)
        {
            if (selectCOASegmentQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, bool, IEnumerable<COASegment>>> expression =
              (ctx, company_ex, coacode_ex, dynamic_ex) =>
                (from row in ctx.COASegment
                 where row.Company == company_ex &&
                 row.COACode == coacode_ex &&
                 row.Dynamic == dynamic_ex
                 select row);
                selectCOASegmentQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return selectCOASegmentQuery_3(this.Db, company, coacode, dynamic);
        }

        static Func<ErpContext, string, string, bool, string, string, IEnumerable<COASegment>> selectCOASegmentQuery_4;
        private IEnumerable<COASegment> SelectCOASegment(string company, string coacode, bool dynamic, string entryControl, string entryControl2)
        {
            if (selectCOASegmentQuery_4 == null)
            {
                Expression<Func<ErpContext, string, string, bool, string, string, IEnumerable<COASegment>>> expression =
                  (ctx, company_ex, coacode_ex, dynamic_ex, entryControl_ex, entryControl2_ex) =>
                    (from row in ctx.COASegment
                     where row.Company == company_ex &&
                     row.COACode == coacode_ex &&
                     row.Dynamic == dynamic_ex &&
                    (row.EntryControl == entryControl_ex ||
                     row.EntryControl == entryControl2_ex)
                     select row);
                selectCOASegmentQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return selectCOASegmentQuery_4(this.Db, company, coacode, dynamic, entryControl, entryControl2);
        }

        class AltCOASegmentExpression5ColumnResult
        {
            public bool SegSelfBal { get; set; }
            public int Level { get; set; }
        }

        public class COASegmentResult
        {
            public string Company { get; set; }
            public string COACode { get; set; }
            public int SegmentNbr { get; set; }
            public string SegmentName { get; set; }

        }

        static Func<ErpContext, string, string, IEnumerable<COASegmentResult>> selectCOASegment3Query;
        private IEnumerable<COASegmentResult> SelectCOASegment3(string company, string coacode)
        {
            if (selectCOASegment3Query == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<COASegmentResult>>> expression =
              (ctx, company_ex, coacode_ex) =>
                (from row in ctx.COASegment
                 where row.Company == company_ex &&
                 row.COACode == coacode_ex
                 select new COASegmentResult() { Company = row.Company, COACode = row.COACode, SegmentNbr = row.SegmentNbr, SegmentName = row.SegmentName });
                selectCOASegment3Query = DBExpressionCompiler.Compile(expression);
            }

            return selectCOASegment3Query(this.Db, company, coacode);
        }

        static Func<ErpContext, string, string, bool> existSelfBalSegQuery;
        private bool ExistSelfBalSeg(string company, string coacode)
        {
            if (existSelfBalSegQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                  (ctx, company_ex, coacode_ex) =>
                    (from row in ctx.COASegment
                     where row.Company == company_ex && row.COACode == coacode_ex && row.SegSelfBal == true
                     select row).Any();
                existSelfBalSegQuery = DBExpressionCompiler.Compile(expression);
            }

            return existSelfBalSegQuery(this.Db, company, coacode);
        }

        static Func<ErpContext, string, string, IEnumerable<COASegment>> selectCOASegmentSBQuery;
        private IEnumerable<COASegment> SelectCOASegmentSBSetup(string company, string coacode)
        {
            if (selectCOASegmentSBQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<COASegment>>> expression =
              (ctx, company_ex, coacode_ex) =>
                (from row in ctx.COASegment
                 where row.Company == company_ex &&
                 row.COACode == coacode_ex &&
                 row.SegSelfBal == true
                 select row);
                selectCOASegmentSBQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectCOASegmentSBQuery(this.Db, company, coacode);
        }


        static Func<ErpContext, string, string, IEnumerable<COASegmentResult>> selectCOASegment7Query;
        private IEnumerable<COASegmentResult> SelectCOASegment_GLCOARefType(string company, string coacode)
        {
            if (selectCOASegment7Query == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<COASegmentResult>>> expression =
              (ctx, company_ex, coacode_ex) =>
                (from row in ctx.COASegment
                 where row.Company == company_ex &&
                 row.COACode == coacode_ex &&
                 row.UseRefEntity == true &&
                 row.RefEntity == "GLCOARefType"
                 select new COASegmentResult() { Company = row.Company, COACode = row.COACode, SegmentNbr = row.SegmentNbr, SegmentName = row.SegmentName });
                selectCOASegment7Query = DBExpressionCompiler.Compile(expression);
            }

            return selectCOASegment7Query(this.Db, company, coacode);
        }


        #endregion COASegment Queries

        #region COASegOpt Queries
        static Func<ErpContext, string, string, int, string, IEnumerable<COASegOpt>> selectCOASegOptQuery;
        private IEnumerable<COASegOpt> SelectCOASegOpt(string company, string coacode, int subSegmentNbr, string valOption)
        {
            if (selectCOASegOptQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, IEnumerable<COASegOpt>>> expression =
                  (ctx, company_ex, coacode_ex, subSegmentNbr_ex, valOption_ex) =>
                    (from row in ctx.COASegOpt
                     where row.Company == company_ex &&
                     row.COACode == coacode_ex &&
                     row.SubSegmentNbr == subSegmentNbr_ex &&
                     row.ValOption == valOption_ex
                     select row);
                selectCOASegOptQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectCOASegOptQuery(this.Db, company, coacode, subSegmentNbr, valOption);
        }

        static Func<ErpContext, string, string, string, string, IEnumerable<int>> selectCOASegOptQuery1;
        private IEnumerable<int> SelectCOASegOpt(string company, string coacode, string segmentCode, string valOption)
        {
            if (selectCOASegOptQuery1 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, IEnumerable<int>>> expression =
                  (ctx, company_ex, coacode_ex, segmentCode_ex, valOption_ex) =>
                    (from row in ctx.COASegOpt
                     where row.Company == company_ex &&
                     row.COACode == coacode_ex &&
                     row.SegmentNbr == 1 &&
                     row.SegmentCode == segmentCode_ex &&
                     row.ValOption == valOption_ex
                     select row.SubSegmentNbr);
                selectCOASegOptQuery1 = DBExpressionCompiler.Compile(expression);
            }

            return selectCOASegOptQuery1(this.Db, company, coacode, segmentCode, valOption);
        }
        #endregion COASegOpt Queries

        #region COASegValues Queries
        static Func<ErpContext, string, string, int, string, string, COASegValues> existsCOASegValuesQuery;
        private bool ExistsCOASegValues(string company, string coacode, int segmentNbr, string segmentCode, string currAcctType)
        {
            if (existsCOASegValuesQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, COASegValues>> expression =
                  (ctx, company_ex, coacode_ex, segmentNbr_ex, segmentCode_ex, currAcctType_ex) =>
                    (from row in ctx.COASegValues
                     where row.Company == company_ex &&
                     row.COACode == coacode_ex &&
                     row.SegmentNbr == segmentNbr_ex &&
                     row.SegmentCode == segmentCode_ex &&
                     row.CurrencyAcctType != currAcctType_ex
                     select row).FirstOrDefault();
                existsCOASegValuesQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsCOASegValuesQuery(this.Db, company, coacode, segmentNbr, segmentCode, currAcctType) != null;
        }

        static Func<ErpContext, string, string, int, COASegValues> existsCOASegValuesQuery_2;
        private bool ExistsCOASegValues(string company, string coacode, int segmentNbr)
        {
            if (existsCOASegValuesQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, COASegValues>> expression =
              (ctx, company_ex, coacode_ex, segmentNbr_ex) =>
                (from row in ctx.COASegValues
                 where row.Company == company_ex &&
                 row.COACode == coacode_ex &&
                 row.SegmentNbr == segmentNbr_ex
                 select row).FirstOrDefault();
                existsCOASegValuesQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return existsCOASegValuesQuery_2(this.Db, company, coacode, segmentNbr) != null;
        }

        static Func<ErpContext, string, string, int, string, DateTime?, string> existsCOASegValuesQuery_3;
        private bool IsCOASegValueActive(string company, string coacode, int segmentNbr, string segmentCode, DateTime? jeDate)
        {
            if (existsCOASegValuesQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, DateTime?, string>> expression =
              (ctx, company_ex, coacode_ex, segmentNbr_ex, segmentCode_ex, jeDate_ex) =>
                (from row in ctx.COASegValues
                 where row.Company == company_ex &&
                 row.COACode == coacode_ex &&
                 row.SegmentNbr == segmentNbr_ex &&
                 row.SegmentCode == segmentCode_ex &&
                 (row.ActiveFlag == true &&
                 (row.EffectiveFrom == null || row.EffectiveFrom <= jeDate_ex) &&
                  (row.EffectiveTo == null || row.EffectiveTo >= jeDate_ex))
                 select row.Company).FirstOrDefault();
                existsCOASegValuesQuery_3 = DBExpressionCompiler.Compile(expression);
            }
            return existsCOASegValuesQuery_3(this.Db, company, coacode, segmentNbr, segmentCode, jeDate) != null;
        }


        public class COASegValuesStatResult
        {
            public int Statistical { get; set; }
        }

        static Func<ErpContext, string, string, int, string, COASegValuesStatResult> findFirstCOASegValuesQuery;
        private COASegValuesStatResult FindFirstCOASegValuesStatistical(string company, string coacode, int segmentNbr, string segmentCode)
        {
            if (findFirstCOASegValuesQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, COASegValuesStatResult>> expression =
              (ctx, company_ex, coacode_ex, segmentNbr_ex, segmentCode_ex) =>
                (from row in ctx.COASegValues
                 where row.Company == company_ex &&
                 row.COACode == coacode_ex &&
                 row.SegmentNbr == segmentNbr_ex &&
                 row.SegmentCode == segmentCode_ex
                 select new COASegValuesStatResult() { Statistical = row.Statistical }).FirstOrDefault();
                findFirstCOASegValuesQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCOASegValuesQuery(this.Db, company, coacode, segmentNbr, segmentCode);
        }

        public class COASegValuesRefResult
        {
            public string RefEntityType { get; set; }
        }

        static Func<ErpContext, string, string, int, string, COASegValuesRefResult> findFirstCOASegValuesQuery2;
        private COASegValuesRefResult FindFirstCOASegValues_RefEntityType(string company, string coacode, int segmentNbr, string segmentCode)
        {
            if (findFirstCOASegValuesQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, COASegValuesRefResult>> expression =
              (ctx, company_ex, coacode_ex, segmentNbr_ex, segmentCode_ex) =>
                (from row in ctx.COASegValues
                 where row.Company == company_ex &&
                 row.COACode == coacode_ex &&
                 row.SegmentNbr == segmentNbr_ex &&
                 row.SegmentCode == segmentCode_ex
                 select new COASegValuesRefResult { RefEntityType = row.RefEntityType }).FirstOrDefault();
                findFirstCOASegValuesQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCOASegValuesQuery2(this.Db, company, coacode, segmentNbr, segmentCode);
        }

        static Func<ErpContext, string, string, int, string, string> isCurrencyAccountQuery;
        private bool IsCurrencyAccount(string company, string coacode, int segmentNbr, string segmentCode)
        {
            if (isCurrencyAccountQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string>> expression =
              (ctx, company_ex, coacode_ex, segmentNbr_ex, segmentCode_ex) =>
                (from row in ctx.COASegValues
                 where row.Company == company_ex &&
                 row.COACode == coacode_ex &&
                 row.SegmentNbr == segmentNbr_ex &&
                 row.SegmentCode == segmentCode_ex &&
                 row.CurrAcct == true
                 select row.Company).FirstOrDefault();
                isCurrencyAccountQuery = DBExpressionCompiler.Compile(expression);
            }

            return isCurrencyAccountQuery(this.Db, company, coacode, segmentNbr, segmentCode) != null;

        }


        static Func<ErpContext, string, string, string, string> isIncomestatementAccountQuery;
        private bool IsIncomestatementAccount(string company, string coacode, string segmentCode)
        {
            if (isIncomestatementAccountQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string>> expression =
              (ctx, company_ex, coacode_ex, segmentCode_ex) =>
                (from row in ctx.COASegValues
                 join rowCat in ctx.COAActCat on new { row.Company, row.COACode, row.Category } equals new { rowCat.Company, rowCat.COACode, Category = rowCat.CategoryID }
                 where row.Company == company_ex &&
                 row.COACode == coacode_ex &&
                 row.SegmentNbr == 1 &&
                 row.SegmentCode == segmentCode_ex &&
                 rowCat.Type == "I"
                 select row.Company).FirstOrDefault();
                isIncomestatementAccountQuery = DBExpressionCompiler.Compile(expression);
            }

            return isIncomestatementAccountQuery(this.Db, company, coacode, segmentCode) != null;
        }

        static Func<ErpContext, string, string, COASegment> findFirstCOASegmentQuery_4;
        private COASegment FindFirstCOASiteSegment(string company, string coacode)
        {
            if (findFirstCOASegmentQuery_4 == null)
            {
                Expression<Func<ErpContext, string, string, COASegment>> expression =
      (ctx, company_ex, coacode_ex) =>
        (from row in ctx.COASegment
         where row.Company == company_ex &&
         row.COACode == coacode_ex &&
         row.SiteSegment == true
         select row).FirstOrDefault();
                findFirstCOASegmentQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCOASegmentQuery_4(this.Db, company, coacode);
        }

        static Func<ErpContext, string, string, int, string, COASegValues> selectCOASegValuesQuery2;
        private COASegValues FindFirstCOASegValues(string company, string coaCode, int segmentNbr, string segmentCode)
        {
            if (selectCOASegValuesQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, COASegValues>> expression =
                  (ctx, company_ex, coaCode_ex, segmentNbr_ex, segmentCode_ex) =>
                    (from row in ctx.COASegValues
                     where row.Company == company_ex &&
                     row.COACode == coaCode_ex &&
                     row.SegmentNbr == segmentNbr_ex &&
                     row.SegmentCode == segmentCode_ex
                     select row).FirstOrDefault();
                selectCOASegValuesQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectCOASegValuesQuery2(this.Db, company, coaCode, segmentNbr, segmentCode);
        }

        static Func<ErpContext, string, string, int, string, COASegValues> findFirstCOASegValueLinkedPlantQuery2;
        private COASegValues FindFirstCOASegmentValueLinkedPlant(string company, string coaCode, int segmentNbr, string linkedPlant)
        {
            if (findFirstCOASegValueLinkedPlantQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, COASegValues>> expression =
                    (ctx, company_ex, coaCode_ex, segmentNbr_ex, linkedPlant_ex) =>
                        (from row in ctx.COASegValues
                         where row.Company == company_ex &&
                         row.COACode == coaCode_ex &&
                         row.SegmentNbr == segmentNbr_ex &&
                         row.LinkedPlant == linkedPlant_ex
                         select row).FirstOrDefault();
                findFirstCOASegValueLinkedPlantQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCOASegValueLinkedPlantQuery2(this.Db, company, coaCode, segmentNbr, linkedPlant);
        }

        #endregion COASegValues Queries

        #region EADComp Queries
        static Func<ErpContext, string, EADComp> findFirstEADCompQuery;
        private EADComp FindFirstEADComp(string company)
        {
            if (findFirstEADCompQuery == null)
            {
                Expression<Func<ErpContext, string, EADComp>> expression =
              (ctx, company_ex) =>
                (from row in ctx.EADComp
                 where row.Company == company_ex
                 select row).FirstOrDefault();
                findFirstEADCompQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEADCompQuery(this.Db, company);
        }
        #endregion EADComp Queries

        #region EADType Queries
        static Func<ErpContext, string, string, EADType> findFirstEADTypeQuery;
        private EADType FindFirstEADType(string company, string eadtype1)
        {
            if (findFirstEADTypeQuery == null)
            {
                Expression<Func<ErpContext, string, string, EADType>> expression =
                  (ctx, company_ex, eadtype1_ex) =>
                    (from row in ctx.EADType
                     where row.Company == company_ex &&
                     row.EADType1 == eadtype1_ex
                     select row).FirstOrDefault();
                findFirstEADTypeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEADTypeQuery(this.Db, company, eadtype1);
        }
        #endregion EADType Queries

        #region EntityGLC Queries

        static Func<ErpContext, string, string, string, string, string, string, string> findFirstEntityGLCQuery_6;
        private string FindFirstEntityGLC(string company, string relatedToFile, string key1, string key2, string key3, string glcontrolType)
        {
            if (findFirstEntityGLCQuery_6 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, string, string>> expression =
                    (ctx, company_ex, relatedToFile_ex, key1_ex, key2_ex, key3_ex, glcontrolType_ex) =>
                        (from row in ctx.EntityGLC
                         where row.Company == company_ex &&
                         row.RelatedToFile == relatedToFile_ex &&
                         row.Key1 == key1_ex &&
                         row.Key2 == key2_ex &&
                         row.Key3 == key3_ex &&
                         row.GLControlType == glcontrolType_ex
                         select row.GLControlCode).FirstOrDefault();
                findFirstEntityGLCQuery_6 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEntityGLCQuery_6(this.Db, company, relatedToFile, key1, key2, key3, glcontrolType);
        }
        #endregion

        #region FiscalPer Queries
        static Func<ErpContext, string, string, int, string, FiscalPer> findFirstFiscalPerQuery;
        private FiscalPer FindFirstFiscalPer(string company, string fiscalCalendarID, int fiscalYear, string fiscalYearSuffix)
        {
            if (findFirstFiscalPerQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, FiscalPer>> expression =
                  (ctx, company_ex, fiscalCalendarID_ex, fiscalYear_ex, fiscalYearSuffix_ex) =>
                    (from row in ctx.FiscalPer
                     where row.Company == company_ex &&
                     row.FiscalCalendarID == fiscalCalendarID_ex &&
                     row.FiscalYear == fiscalYear_ex &&
                     row.FiscalYearSuffix == fiscalYearSuffix_ex
                     select row).FirstOrDefault();
                findFirstFiscalPerQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstFiscalPerQuery(this.Db, company, fiscalCalendarID, fiscalYear, fiscalYearSuffix);
        }
        #endregion FiscalPer Queries

        #region FiscalYr Queries
        static Func<ErpContext, string, string, int, string, FiscalYr> findFirstFiscalYrQuery;
        private FiscalYr FindFirstFiscalYr(string company, string fiscalCalendarID, int fiscalYear, string fiscalYearSuffix)
        {
            if (findFirstFiscalYrQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, FiscalYr>> expression =
                  (ctx, company_ex, fiscalCalendarID_ex, fiscalYear_ex, fiscalYearSuffix_ex) =>
                    (from row in ctx.FiscalYr
                     where row.Company == company_ex &&
                     row.FiscalCalendarID == fiscalCalendarID_ex &&
                     row.FiscalYear == fiscalYear_ex &&
                     row.FiscalYearSuffix == fiscalYearSuffix_ex
                     select row).FirstOrDefault();
                findFirstFiscalYrQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstFiscalYrQuery(this.Db, company, fiscalCalendarID, fiscalYear, fiscalYearSuffix);
        }

        static Func<ErpContext, string, string, DateTime?, FiscalYr> findFirstFiscalYrQuery2;
        private FiscalYr FindFirstFiscalYr(string company, string fiscalCalendarID, DateTime? date)
        {
            if (findFirstFiscalYrQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, FiscalYr>> expression =
                  (ctx, company_ex, fiscalCalendarID_ex, date_ex) =>
                    (from row in ctx.FiscalYr
                     where row.Company == company_ex &&
                     row.FiscalCalendarID == fiscalCalendarID_ex &&
                     row.StartDate <= date_ex &&
                     row.EndDate >= date_ex
                     select row).FirstOrDefault();
                findFirstFiscalYrQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstFiscalYrQuery2(this.Db, company, fiscalCalendarID, date);
        }
        #endregion FiscalYr Queries        

        #region GLAccountMasks Queries
        static Func<ErpContext, string, string, string, string, IEnumerable<GLAccountMasks>> selectGLAccountMasksQuery;
        private IEnumerable<GLAccountMasks> SelectGLAccountMasks(string company, string coacode, string bookID, string maskType)
        {
            if (selectGLAccountMasksQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, IEnumerable<GLAccountMasks>>> expression =
                  (ctx, company_ex, coacode_ex, bookID_ex, maskType_ex) =>
                    (from row in ctx.GLAccountMasks
                     where row.Company == company_ex &&
                     row.COACode == coacode_ex &&
                     row.BookID == bookID_ex &&
                     row.MaskType == maskType_ex
                     select row);
                selectGLAccountMasksQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectGLAccountMasksQuery(this.Db, company, coacode, bookID, maskType);
        }
        #endregion GLAccountMasks Queries

        #region GLBook Queries
        static Func<ErpContext, string, string, GLBook> findFirstGLBookQuery;
        private GLBook FindFirstGLBook(string company, string bookID)
        {
            if (findFirstGLBookQuery == null)
            {
                Expression<Func<ErpContext, string, string, GLBook>> expression =
                  (ctx, company_ex, bookID_ex) =>
                    (from row in ctx.GLBook
                     where row.Company == company_ex &&
                     row.BookID == bookID_ex
                     select row).FirstOrDefault();
                findFirstGLBookQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookQuery(this.Db, company, bookID);
        }

        static Func<ErpContext, string, string, string> findFirstGLBookQuery2;
        private string FindFirstGLBook2(string company, string bookID)
        {
            if (findFirstGLBookQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
              (ctx, company_ex, bookID_ex) =>
                (from row in ctx.GLBook
                 where row.Company == company_ex &&
                 row.BookID == bookID_ex
                 select row.OpenBalUpdateOpt).FirstOrDefault();
                findFirstGLBookQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookQuery2(this.Db, company, bookID);
        }
        static Func<ErpContext, string, string, string> findFirstGLBookDescQuery;
        private string FindFirstGLBookDesc(string company, string bookID)
        {
            if (findFirstGLBookDescQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
                  (ctx, company_ex, bookID_ex) =>
                    (from row in ctx.GLBook
                     where row.Company == company_ex &&
                     row.BookID == bookID_ex
                     select row.Description).FirstOrDefault();
                findFirstGLBookDescQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookDescQuery(this.Db, company, bookID);
        }

        static Func<ErpContext, string, string, string> findFirstGLBookCurrQuery;
        private string FindFirstGLBookCurr(string company, string bookID)
        {
            if (findFirstGLBookCurrQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
      (ctx, company_ex, bookID_ex) =>
        (from row in ctx.GLBook
         where row.Company == company_ex &&
         row.BookID == bookID_ex
         select row.CurrencyCode).FirstOrDefault();
                findFirstGLBookCurrQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookCurrQuery(this.Db, company, bookID);
        }
        #endregion GLBook Queries

        #region GLBookPer Queries
        static Func<ErpContext, string, string, string, DateTime?, DateTime?, bool, GLBookPer> findFirstGLBookPerQuery;
        private GLBookPer FindFirstGLBookPer(string company, string bookID, string fiscalCalendarID, DateTime? startDate, DateTime? endDate, bool closedPeriod)
        {
            if (findFirstGLBookPerQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, DateTime?, DateTime?, bool, GLBookPer>> expression =
                  (ctx, company_ex, bookID_ex, fiscalCalendarID_ex, startDate_ex, endDate_ex, closedPeriod_ex) =>
                    (from row in ctx.GLBookPer
                     where row.Company == company_ex &&
                     row.BookID == bookID_ex &&
                     row.FiscalCalendarID == fiscalCalendarID_ex &&
                     row.StartDate.Value <= startDate_ex.Value &&
                     row.EndDate.Value >= endDate_ex.Value &&
                     row.ClosedPeriod == closedPeriod_ex &&
                     row.CloseFiscalPeriod == 0
                     select row).FirstOrDefault();
                findFirstGLBookPerQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookPerQuery(this.Db, company, bookID, fiscalCalendarID, startDate, endDate, closedPeriod);
        }

        static Func<ErpContext, string, string, string, DateTime?, bool, GLBookPer> findFirstGLBookPerQuery_2;
        private GLBookPer FindFirstGLBookPer(string company, string bookID, string fiscalCalendarID, DateTime? startDate, bool closedPeriod)
        {
            if (findFirstGLBookPerQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, DateTime?, bool, GLBookPer>> expression =
              (ctx, company_ex, bookID_ex, fiscalCalendarID_ex, startDate_ex, closedPeriod_ex) =>
                (from row in ctx.GLBookPer
                 where row.Company == company_ex &&
                 row.BookID == bookID_ex &&
                 row.FiscalCalendarID == fiscalCalendarID_ex &&
                 row.StartDate.Value >= startDate_ex.Value &&
                 row.ClosedPeriod == closedPeriod_ex
                 select row).FirstOrDefault();
                findFirstGLBookPerQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookPerQuery_2(this.Db, company, bookID, fiscalCalendarID, startDate, closedPeriod);
        }

        static Func<ErpContext, string, string, string, int, string, int, GLBookPer> findFirstGLBookPerQuery_3;
        private GLBookPer FindFirstGLBookPer(string company, string bookID, string fiscalCalendarID, int fiscalYear, string fiscalYearSuffix, int fiscalPeriod)
        {
            if (findFirstGLBookPerQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, string, int, string, int, GLBookPer>> expression =
              (ctx, company_ex, bookID_ex, fiscalCalendarID_ex, fiscalYear_ex, fiscalYear_suffix_ex, fiscalPeriod_ex) =>
                (from row in ctx.GLBookPer
                 where row.Company == company_ex &&
                 row.BookID == bookID_ex &&
                 row.FiscalCalendarID == fiscalCalendarID_ex &&
                 row.FiscalYear == fiscalYear_ex &&
                 row.FiscalYearSuffix == fiscalYear_suffix_ex &&
                 row.FiscalPeriod == fiscalPeriod_ex
                 select row).FirstOrDefault();
                findFirstGLBookPerQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookPerQuery_3(this.Db, company, bookID, fiscalCalendarID, fiscalYear, fiscalYearSuffix, fiscalPeriod);
        }

        static Func<ErpContext, string, string, string, int, string, int, int, GLBookPer> findFirstGLBookPerQuery_4;
        private GLBookPer FindFirstGLBookPer(string company, string bookID, string fiscalCalendarID, int fiscalYear, string fiscalYearSuffix, int fiscalPeriod, int closeFiscalPeriod)
        {
            if (findFirstGLBookPerQuery_4 == null)
            {
                Expression<Func<ErpContext, string, string, string, int, string, int, int, GLBookPer>> expression =
              (ctx, company_ex, bookID_ex, fiscalCalendarID_ex, fiscalYear_ex, fiscalYearSuffix_ex, fiscalPeriod_ex, closeFiscalPeriod_ex) =>
                (from row in ctx.GLBookPer
                 where row.Company == company_ex &&
                 row.BookID == bookID_ex &&
                 row.FiscalCalendarID == fiscalCalendarID_ex &&
                 row.FiscalYear == fiscalYear_ex &&
                 row.FiscalYearSuffix == fiscalYearSuffix_ex &&
                 row.FiscalPeriod == fiscalPeriod_ex &&
                 row.CloseFiscalPeriod > closeFiscalPeriod_ex
                 select row).FirstOrDefault();
                findFirstGLBookPerQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookPerQuery_4(this.Db, company, bookID, fiscalCalendarID, fiscalYear, fiscalYearSuffix, fiscalPeriod, closeFiscalPeriod);
        }

        static Func<ErpContext, string, string, string, DateTime?, DateTime?, int, GLBookPer> findFirstGLBookPerQuery_5;
        private GLBookPer FindFirstGLBookPer(string company, string bookID, string fiscalCalendarID, DateTime? startDate, DateTime? endDate, int closeFiscalPeriod)
        {
            if (findFirstGLBookPerQuery_5 == null)
            {
                Expression<Func<ErpContext, string, string, string, DateTime?, DateTime?, int, GLBookPer>> expression =
              (ctx, company_ex, bookID_ex, fiscalCalendarID_ex, startDate_ex, endDate_ex, closeFiscalPeriod_ex) =>
                (from row in ctx.GLBookPer
                 where row.Company == company_ex &&
                 row.BookID == bookID_ex &&
                 row.FiscalCalendarID == fiscalCalendarID_ex &&
                 row.StartDate.Value == startDate_ex.Value &&
                 row.EndDate.Value == endDate_ex.Value &&
                 row.CloseFiscalPeriod == closeFiscalPeriod_ex
                 select row).FirstOrDefault();
                findFirstGLBookPerQuery_5 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookPerQuery_5(this.Db, company, bookID, fiscalCalendarID, startDate, endDate, closeFiscalPeriod);
        }

        static Func<ErpContext, string, string, string, int, int, DateTime?, DateTime?, GLBookPer> findFirstGLBookPerQuery_6;
        private GLBookPer FindFirstGLBookPer(string company, string bookID, string fiscalCalendarID, int fiscalYear, int fiscalPeriod, DateTime? startDate, DateTime? endDate)
        {
            if (findFirstGLBookPerQuery_6 == null)
            {
                Expression<Func<ErpContext, string, string, string, int, int, DateTime?, DateTime?, GLBookPer>> expression =
              (ctx, company_ex, bookID_ex, fiscalCalendarID_ex, fiscalYear_ex, fiscalPeriod_ex, startDate_ex, endDate_ex) =>
                (from row in ctx.GLBookPer
                 where row.Company == company_ex &&
                 row.BookID == bookID_ex &&
                 row.FiscalCalendarID == fiscalCalendarID_ex &&
                 row.FiscalYear == fiscalYear_ex &&
                 row.FiscalPeriod == fiscalPeriod_ex &&
                 row.StartDate.Value <= startDate_ex.Value &&
                 row.EndDate.Value >= endDate_ex.Value
                 select row).FirstOrDefault();
                findFirstGLBookPerQuery_6 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookPerQuery_6(this.Db, company, bookID, fiscalCalendarID, fiscalYear, fiscalPeriod, startDate, endDate);
        }

        static Func<ErpContext, string, string, string, DateTime?, bool, GLBookPer> findFirstGLBookPer2Query;
        private GLBookPer FindFirstGLBookPer2(string company, string bookID, string fiscalCalendarID, DateTime? endDate, bool closedPeriod)
        {
            if (findFirstGLBookPer2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, DateTime?, bool, GLBookPer>> expression =
                      (ctx, company_ex, bookID_ex, fiscalCalendarID_ex, endDate_ex, closedPeriod_ex) =>
                        (from row in ctx.GLBookPer
                         where row.Company == company_ex &&
                         row.BookID == bookID_ex &&
                         row.FiscalCalendarID == fiscalCalendarID_ex &&
                         row.EndDate.Value > endDate_ex.Value &&
                         row.ClosedPeriod == closedPeriod_ex &&
                         row.CloseFiscalPeriod == 0
                         select row).FirstOrDefault();
                findFirstGLBookPer2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookPer2Query(this.Db, company, bookID, fiscalCalendarID, endDate, closedPeriod);
        }
        static Func<ErpContext, string, string, string, DateTime?, bool, int, GLBookPer> findFirstGLBookPer3Query;
        private GLBookPer FindFirstGLBookPer3(string company, string bookID, string fiscalCalendarID, DateTime? endDate, bool closedPeriod, int closingNum)
        {
            if (findFirstGLBookPer3Query == null)
            {
                Expression<Func<ErpContext, string, string, string, DateTime?, bool, int, GLBookPer>> expression =
                  (ctx, company_ex, bookID_ex, fiscalCalendarID_ex, endDate_ex, closedPeriod_ex, closingNum_ex) =>
                    (from row in ctx.GLBookPer
                     where row.Company == company_ex &&
                     row.BookID == bookID_ex &&
                     row.FiscalCalendarID == fiscalCalendarID_ex &&
                     row.EndDate.Value == endDate_ex.Value &&
                     row.ClosedPeriod == closedPeriod_ex &&
                     row.CloseFiscalPeriod > closingNum_ex
                     select row).FirstOrDefault();
                findFirstGLBookPer3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBookPer3Query(this.Db, company, bookID, fiscalCalendarID, endDate, closedPeriod, closingNum);
        }
        static Func<ErpContext, string, string, string, DateTime?, DateTime?, int, GLBookPer> findLastGLBookPerQuery;
        private GLBookPer FindLastGLBookPer(string company, string bookID, string fiscalCalendarID, DateTime? startDate, DateTime? endDate, int closeFiscalPeriod)
        {
            if (findLastGLBookPerQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, DateTime?, DateTime?, int, GLBookPer>> expression =
              (ctx, company_ex, bookID_ex, fiscalCalendarID_ex, startDate_ex, endDate_ex, closeFiscalPeriod_ex) =>
                (from row in ctx.GLBookPer
                 where row.Company == company_ex &&
                 row.BookID == bookID_ex &&
                 row.FiscalCalendarID == fiscalCalendarID_ex &&
                 row.StartDate.Value == startDate_ex.Value &&
                 row.EndDate.Value == endDate_ex.Value &&
                 row.CloseFiscalPeriod > closeFiscalPeriod_ex
                 select row).LastOrDefault();
                findLastGLBookPerQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastGLBookPerQuery(this.Db, company, bookID, fiscalCalendarID, startDate, endDate, closeFiscalPeriod);
        }

        static Func<ErpContext, string, string, string, int, bool, DateTime?, DateTime?, GLBookPer> findfirstGLBookPerQuery_2;
        private GLBookPer FindFirstGLBookPer(string company, string bookID, string fiscalCalendarID, int closeFiscalPeriod, bool closedPeriod, DateTime? startDate, DateTime? endDate)
        {
            if (findfirstGLBookPerQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, int, bool, DateTime?, DateTime?, GLBookPer>> expression =
              (ctx, company_ex, bookID_ex, fiscalCalendarID_ex, closeFiscalPeriod_ex, closedPeriod_ex, startDate_ex, endDate_ex) =>
                (from row in ctx.GLBookPer
                 where row.Company == company_ex &&
                 row.BookID == bookID_ex &&
                 row.FiscalCalendarID == fiscalCalendarID_ex &&
                 row.CloseFiscalPeriod == closeFiscalPeriod_ex &&
                 row.ClosedPeriod == closedPeriod_ex &&
                 row.StartDate.Value <= startDate_ex.Value &&
                 row.EndDate.Value >= endDate_ex.Value
                 select row).FirstOrDefault();
                findfirstGLBookPerQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findfirstGLBookPerQuery_2(this.Db, company, bookID, fiscalCalendarID, closeFiscalPeriod, closedPeriod, startDate, endDate);
        }
        #endregion GLBookPer Queries

        #region GLCOARefAcct Queries
        static Func<ErpContext, string, string, int, string, IEnumerable<GLCOARefAcct>> selectGLCOARefAcctQuery;
        private IEnumerable<GLCOARefAcct> SelectGLCOARefAcct(string company, string coacode, int segmentNbr, string refType)
        {
            if (selectGLCOARefAcctQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, IEnumerable<GLCOARefAcct>>> expression =
                  (ctx, company_ex, coacode_ex, segmentNbr_ex, refType_ex) =>
                    (from row in ctx.GLCOARefAcct
                     where row.Company == company_ex &&
                     row.COACode == coacode_ex &&
                     row.SegmentNbr == segmentNbr_ex &&
                     row.RefType == refType_ex
                     select row);
                selectGLCOARefAcctQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectGLCOARefAcctQuery(this.Db, company, coacode, segmentNbr, refType);
        }
        #endregion GLCOARefAcct Queries

        #region GLCOARefType Queries
        static Func<ErpContext, string, string, int, string, IEnumerable<GLCOARefType>> selectGLCOARefTypeQuery;
        private IEnumerable<GLCOARefType> SelectGLCOARefType(string company, string coacode, int segmentNbr, string refType)
        {
            if (selectGLCOARefTypeQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, IEnumerable<GLCOARefType>>> expression =
                  (ctx, company_ex, coacode_ex, segmentNbr_ex, refType_ex) =>
                    (from row in ctx.GLCOARefType
                     where row.Company == company_ex &&
                     row.COACode == coacode_ex &&
                     row.SegmentNbr == segmentNbr_ex &&
                     row.RefType == refType_ex
                     select row);
                selectGLCOARefTypeQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectGLCOARefTypeQuery(this.Db, company, coacode, segmentNbr, refType);
        }

        static Func<ErpContext, string, string, int, IEnumerable<GLCOARefType>> selectGLCOARefTypeQuery2;
        private IEnumerable<GLCOARefType> SelectGLCOARefType(string company, string coacode, int segmentNbr)
        {
            if (selectGLCOARefTypeQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, int, IEnumerable<GLCOARefType>>> expression =
                  (ctx, company_ex, coacode_ex, segmentNbr_ex) =>
                    (from row in ctx.GLCOARefType
                     where row.Company == company_ex &&
                     row.COACode == coacode_ex &&
                     row.SegmentNbr == segmentNbr_ex
                     select row);
                selectGLCOARefTypeQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectGLCOARefTypeQuery2(this.Db, company, coacode, segmentNbr);
        }
        #endregion GLCOARefType Queries        

        #region GLCntrlAcct Queries
        static Func<ErpContext, string, string, string, string, string, GLCntrlAcct> findFirstGLCntrlAcctQuery;
        private GLCntrlAcct FindFirstGLCntrlAcct(string company, string glcontrolType, string glcontrolCode, string bookID, string glacctContext1)
        {
            if (findFirstGLCntrlAcctQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, GLCntrlAcct>> expression =
      (ctx, company_ex, glcontrolType_ex, glcontrolCode_ex, bookID_ex, glacctContext_ex) =>
        (from row in ctx.GLCntrlAcct
         where row.Company == company_ex &&
         row.GLControlType == glcontrolType_ex &&
         row.GLControlCode == glcontrolCode_ex &&
         row.BookID == bookID_ex &&
         row.GLAcctContext == glacctContext_ex
         select row).FirstOrDefault();
                findFirstGLCntrlAcctQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstGLCntrlAcctQuery(this.Db, company, glcontrolType, glcontrolCode, bookID, glacctContext1);
        }
        #endregion

        #region JrnlCode Queries
        //HOLDLOCK
        static Func<ErpContext, string, string, JrnlCode> findFirstJrnlCodeQuery;
        private JrnlCode FindFirstJrnlCode(string company, string journalCode)
        {
            if (findFirstJrnlCodeQuery == null)
            {
                Expression<Func<ErpContext, string, string, JrnlCode>> expression =
                  (ctx, company_ex, journalCode_ex) =>
                    (from row in ctx.JrnlCode
                     where row.Company == company_ex &&
                     row.JournalCode == journalCode_ex
                     select row).FirstOrDefault();
                findFirstJrnlCodeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJrnlCodeQuery(this.Db, company, journalCode);
        }
        #endregion JrnlCode Queries

        #region Plant
        static Func<ErpContext, string, string, Plant> findFirstPlantQuery;
        private Plant FindFirstPlant(string company, string plant1)
        {
            if (findFirstPlantQuery == null)
            {
                Expression<Func<ErpContext, string, string, Plant>> expression =
      (ctx, company_ex, plant1_ex) =>
        (from row in ctx.Plant
         where row.Company == company_ex &&
         row.Plant1 == plant1_ex
         select row).FirstOrDefault();
                findFirstPlantQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlantQuery(this.Db, company, plant1);
        }
        #endregion

        #region Statistical Queries
        //select NumOfDec from erp.StatUOM
        static Func<ErpContext, string, string, bool> statUOMQuery;
        private bool ExistStatUOM(string company, string uomCode)
        {
            if (statUOMQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                  (ctx, company_ex, uomcode_ex) =>
                    (from row in ctx.StatUOM
                     where row.Company == company_ex && row.StatUOMCode == uomcode_ex
                     select row).Any();
                statUOMQuery = DBExpressionCompiler.Compile(expression);
            }

            return statUOMQuery(this.Db, company, uomCode);
        }
        #endregion Statistical

        #region XbSyst Queries
        static Func<ErpContext, string, XbSyst> findFirstXbSystQuery;
        private XbSyst FindFirstXbSyst(string company)
        {
            if (findFirstXbSystQuery == null)
            {
                Expression<Func<ErpContext, string, XbSyst>> expression =
      (ctx, company_ex) =>
        (from row in ctx.XbSyst
         where row.Company == company_ex
         select row).FirstOrDefault();
                findFirstXbSystQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstXbSystQuery(this.Db, company);
        }
        #endregion XbSyst Queries
    }
}

