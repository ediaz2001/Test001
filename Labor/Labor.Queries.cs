using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Epicor.Data;
using Erp.Tables;

#if !USE_EF_CORE
using System.Data.Entity;
#endif

namespace Erp.Services.BO
{
    public partial class LaborSvc
    {
        #region Capability Queries

        static Func<ErpContext, string, string, Capability> findFirstCapabilityQuery;
        private Capability FindFirstCapability(string company, string capabilityID)
        {
            if (findFirstCapabilityQuery == null)
            {
                Expression<Func<ErpContext, string, string, Capability>> expression =
      (ctx, company_ex, capabilityID_ex) =>
        (from row in ctx.Capability
         where row.Company == company_ex &&
         row.CapabilityID == capabilityID_ex
         select row).FirstOrDefault();
                findFirstCapabilityQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCapabilityQuery(this.Db, company, capabilityID);
        }


        static Func<ErpContext, string, string, Capability> findFirstCapability2Query;
        private Capability FindFirstCapability2(string company, string capabilityID)
        {
            if (findFirstCapability2Query == null)
            {
                Expression<Func<ErpContext, string, string, Capability>> expression =
      (ctx, company_ex, capabilityID_ex) =>
        (from row in ctx.Capability
         where row.Company == company_ex &&
         row.CapabilityID == capabilityID_ex
         select row).FirstOrDefault();
                findFirstCapability2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCapability2Query(this.Db, company, capabilityID);
        }


        static Func<ErpContext, string, string, Capability> findFirstCapability3Query;
        private Capability FindFirstCapability3(string company, string capabilityID)
        {
            if (findFirstCapability3Query == null)
            {
                Expression<Func<ErpContext, string, string, Capability>> expression =
      (ctx, company_ex, capabilityID_ex) =>
        (from row in ctx.Capability
         where row.Company == company_ex &&
         row.CapabilityID == capabilityID_ex
         select row).FirstOrDefault();
                findFirstCapability3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCapability3Query(this.Db, company, capabilityID);
        }


        static Func<ErpContext, string, string, Capability> findFirstCapability4Query;
        private Capability FindFirstCapability4(string company, string capabilityID)
        {
            if (findFirstCapability4Query == null)
            {
                Expression<Func<ErpContext, string, string, Capability>> expression =
      (ctx, company_ex, capabilityID_ex) =>
        (from row in ctx.Capability
         where row.Company == company_ex &&
         row.CapabilityID == capabilityID_ex
         select row).FirstOrDefault();
                findFirstCapability4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCapability4Query(this.Db, company, capabilityID);
        }


        static Func<ErpContext, string, string, Capability> findFirstCapability5Query;
        private Capability FindFirstCapability5(string company, string capabilityID)
        {
            if (findFirstCapability5Query == null)
            {
                Expression<Func<ErpContext, string, string, Capability>> expression =
      (ctx, company_ex, capabilityID_ex) =>
        (from row in ctx.Capability
         where row.Company == company_ex &&
         row.CapabilityID == capabilityID_ex
         select row).FirstOrDefault();
                findFirstCapability5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCapability5Query(this.Db, company, capabilityID);
        }
        #endregion Capability Queries

        #region CapResLnk Queries

        static Func<ErpContext, string, string, string, bool> existsCapResLnkQuery;
        private bool ExistsCapResLnk(string company, string capabilityID, string resourceID)
        {
            if (existsCapResLnkQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, capabilityID_ex, resourceID_ex) =>
        (from row in ctx.CapResLnk
         where row.Company == company_ex &&
         row.CapabilityID == capabilityID_ex &&
         row.ResourceID == resourceID_ex
         select row).Any();
                existsCapResLnkQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsCapResLnkQuery(this.Db, company, capabilityID, resourceID);
        }


        static Func<ErpContext, string, string, string, bool> existsCapResLnk2Query;
        private bool ExistsCapResLnk2(string company, string capabilityID, string resourceID)
        {
            if (existsCapResLnk2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, capabilityID_ex, resourceID_ex) =>
        (from row in ctx.CapResLnk
         where row.Company == company_ex &&
         row.CapabilityID == capabilityID_ex &&
         row.ResourceID == resourceID_ex
         select row).Any();
                existsCapResLnk2Query = DBExpressionCompiler.Compile(expression);
            }

            return existsCapResLnk2Query(this.Db, company, capabilityID, resourceID);
        }



        static Func<ErpContext, string, string, string, CapResLnk> findFirstCapResLnkQuery;
        private CapResLnk FindFirstCapResLnk(string company, string capabilityID, string resourceID)
        {
            if (findFirstCapResLnkQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, CapResLnk>> expression =
      (ctx, company_ex, capabilityID_ex, resourceID_ex) =>
        (from row in ctx.CapResLnk
         where row.Company == company_ex &&
         row.CapabilityID == capabilityID_ex &&
         row.ResourceID == resourceID_ex
         select row).FirstOrDefault();
                findFirstCapResLnkQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCapResLnkQuery(this.Db, company, capabilityID, resourceID);
        }
        #endregion CapResLnk Queries

        #region Company Queries

        static Func<ErpContext, string, Company> findFirstCompanyQuery;
        private Company FindFirstCompany(string company1)
        {
            if (findFirstCompanyQuery == null)
            {
                Expression<Func<ErpContext, string, Company>> expression =
      (ctx, company1_ex) =>
        (from row in ctx.Company
         where row.Company1 == company1_ex
         select row).FirstOrDefault();
                findFirstCompanyQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCompanyQuery(this.Db, company1);
        }

        static Func<ErpContext, string, bool> existsCompanyQuery;
        private bool ExistsCompany(string company)
        {
            if (existsCompanyQuery == null)
            {
                Expression<Func<ErpContext, string, bool>> expression =
                  (ctx, company_ex) =>
                  ctx.Company.Where(at => at.Company1 == company_ex).Any();
                existsCompanyQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsCompanyQuery(this.Db, company);
        }
        #endregion Company Queries

        #region EmpBasic Queries

        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasicQuery;
        private EmpBasic FindFirstEmpBasic(string company, string empID)
        {
            if (findFirstEmpBasicQuery == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasicQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasicQuery(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic10Query;
        private EmpBasic FindFirstEmpBasic10(string company, string empID)
        {
            if (findFirstEmpBasic10Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic10Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic10Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic11Query;
        private EmpBasic FindFirstEmpBasic11(string company, string empID)
        {
            if (findFirstEmpBasic11Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic11Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic11Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic12Query;
        private EmpBasic FindFirstEmpBasic12(string company, string empID)
        {
            if (findFirstEmpBasic12Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic12Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic12Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic13Query;
        private EmpBasic FindFirstEmpBasic13(string company, string empID)
        {
            if (findFirstEmpBasic13Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic13Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic13Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic14Query;
        private EmpBasic FindFirstEmpBasic14(string company, string empID)
        {
            if (findFirstEmpBasic14Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic14Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic14Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic15Query;
        private EmpBasic FindFirstEmpBasic15(string company, string empID)
        {
            if (findFirstEmpBasic15Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic15Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic15Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic16Query;
        private EmpBasic FindFirstEmpBasic16(string company, string empID)
        {
            if (findFirstEmpBasic16Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic16Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic16Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic17Query;
        private EmpBasic FindFirstEmpBasic17(string company, string empID)
        {
            if (findFirstEmpBasic17Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic17Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic17Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic18Query;
        private EmpBasic FindFirstEmpBasic18(string company, string empID)
        {
            if (findFirstEmpBasic18Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic18Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic18Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic19Query;
        private EmpBasic FindFirstEmpBasic19(string company, string empID)
        {
            if (findFirstEmpBasic19Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic19Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic19Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic2Query;
        private EmpBasic FindFirstEmpBasic2(string company, string empID)
        {
            if (findFirstEmpBasic2Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic2Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic20Query;
        private EmpBasic FindFirstEmpBasic20(string company, string empID)
        {
            if (findFirstEmpBasic20Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic20Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic20Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic21Query;
        private EmpBasic FindFirstEmpBasic21(string company, string empID)
        {
            if (findFirstEmpBasic21Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic21Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic21Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic22Query;
        private EmpBasic FindFirstEmpBasic22(string company, string empID)
        {
            if (findFirstEmpBasic22Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic22Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic22Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic23Query;
        private EmpBasic FindFirstEmpBasic23(string company, string empID)
        {
            if (findFirstEmpBasic23Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic23Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic23Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic24Query;
        private EmpBasic FindFirstEmpBasic24(string company, string empID)
        {
            if (findFirstEmpBasic24Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic24Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic24Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic25Query;
        private EmpBasic FindFirstEmpBasic25(string company, string empID)
        {
            if (findFirstEmpBasic25Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic25Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic25Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic26Query;
        private EmpBasic FindFirstEmpBasic26(string company, string empID)
        {
            if (findFirstEmpBasic26Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic26Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic26Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic3Query;
        private EmpBasic FindFirstEmpBasic3(string company, string empID)
        {
            if (findFirstEmpBasic3Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic3Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic4Query;
        private EmpBasic FindFirstEmpBasic4(string company, string empID)
        {
            if (findFirstEmpBasic4Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic4Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic5Query;
        private EmpBasic FindFirstEmpBasic5(string company, string empID)
        {
            if (findFirstEmpBasic5Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic5Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic6Query;
        private EmpBasic FindFirstEmpBasic6(string company, string empID)
        {
            if (findFirstEmpBasic6Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic6Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic6Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic7Query;
        private EmpBasic FindFirstEmpBasic7(string company, string empID)
        {
            if (findFirstEmpBasic7Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic7Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic7Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic8Query;
        private EmpBasic FindFirstEmpBasic8(string company, string empID)
        {
            if (findFirstEmpBasic8Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic8Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic8Query(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, EmpBasic> findFirstEmpBasic9Query;
        private EmpBasic FindFirstEmpBasic9(string company, string empID)
        {
            if (findFirstEmpBasic9Query == null)
            {
                Expression<Func<ErpContext, string, string, EmpBasic>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpBasic
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).FirstOrDefault();
                findFirstEmpBasic9Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpBasic9Query(this.Db, company, empID);
        }
        #endregion EmpBasic Queries

        #region EmpCal Queries

        static Func<ErpContext, string, string, DateTime, DateTime?, DateTime?, EmpCal> findLastEmpCalQuery;
        private EmpCal FindLastEmpCal(string company, string empID, DateTime fromEffectiveDate, DateTime? toEffectiveDate, DateTime? toEffectiveDate2)
        {
            if (findLastEmpCalQuery == null)
            {
                Expression<Func<ErpContext, string, string, DateTime, DateTime?, DateTime?, EmpCal>> expression =
      (ctx, company_ex, empID_ex, fromEffectiveDate_ex, toEffectiveDate_ex, toEffectiveDate2_ex) =>
        (from row in ctx.EmpCal
         where row.Company == company_ex &&
         row.EmpID == empID_ex &&
         row.FromEffectiveDate <= fromEffectiveDate_ex &&
         row.ToEffectiveDate.Value != toEffectiveDate_ex.Value &&
         row.ToEffectiveDate.Value >= toEffectiveDate2_ex.Value
         select row).LastOrDefault();
                findLastEmpCalQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastEmpCalQuery(this.Db, company, empID, fromEffectiveDate, toEffectiveDate, toEffectiveDate2);
        }


        static Func<ErpContext, string, string, DateTime, DateTime?, EmpCal> findLastEmpCalQuery_2;
        private EmpCal FindLastEmpCal(string company, string empID, DateTime fromEffectiveDate, DateTime? toEffectiveDate)
        {
            if (findLastEmpCalQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, DateTime, DateTime?, EmpCal>> expression =
      (ctx, company_ex, empID_ex, fromEffectiveDate_ex, toEffectiveDate_ex) =>
        (from row in ctx.EmpCal
         where row.Company == company_ex &&
         row.EmpID == empID_ex &&
         row.FromEffectiveDate <= fromEffectiveDate_ex &&
         row.ToEffectiveDate.Value == toEffectiveDate_ex.Value
         select row).LastOrDefault();
                findLastEmpCalQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findLastEmpCalQuery_2(this.Db, company, empID, fromEffectiveDate, toEffectiveDate);
        }
        #endregion EmpCal Queries

        #region EmpRole Queries
        //HOLDLOCK



        static Func<ErpContext, string, string, bool> existsEmpRoleQuery;
        private bool ExistsEmpRole(string company, string empID)
        {
            if (existsEmpRoleQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpRole
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select row).Any();
                existsEmpRoleQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsEmpRoleQuery(this.Db, company, empID);
        }


        static Func<ErpContext, string, string, string, bool> existsEmpRoleQuery_2;
        private bool ExistsEmpRole(string company, string empID, string roleCd)
        {
            if (existsEmpRoleQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, empID_ex, roleCd_ex) =>
        (from row in ctx.EmpRole
         where row.Company == company_ex &&
         row.EmpID == empID_ex &&
         row.RoleCd == roleCd_ex
         select row).Any();
                existsEmpRoleQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return existsEmpRoleQuery_2(this.Db, company, empID, roleCd);
        }


        static Func<ErpContext, string, string, string, bool> existsEmpRole2Query;
        private bool ExistsEmpRole2(string company, string empID, string roleCd)
        {
            if (existsEmpRole2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, empID_ex, roleCd_ex) =>
        (from row in ctx.EmpRole
         where row.Company == company_ex &&
         row.EmpID == empID_ex &&
         row.RoleCd == roleCd_ex
         select row).Any();
                existsEmpRole2Query = DBExpressionCompiler.Compile(expression);
            }

            return existsEmpRole2Query(this.Db, company, empID, roleCd);
        }


        static Func<ErpContext, string, string, bool, EmpRole> findFirstEmpRoleQuery;
        private EmpRole FindFirstEmpRole(string company, string empID, bool primary)
        {
            if (findFirstEmpRoleQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, EmpRole>> expression =
      (ctx, company_ex, empID_ex, primary_ex) =>
        (from row in ctx.EmpRole
         where row.Company == company_ex &&
         row.EmpID == empID_ex &&
         row.Primary == primary_ex
         select row).FirstOrDefault();
                findFirstEmpRoleQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpRoleQuery(this.Db, company, empID, primary);
        }


        static Func<ErpContext, string, string, bool, EmpRole> findFirstEmpRole2Query;
        private EmpRole FindFirstEmpRole2(string company, string empID, bool primary)
        {
            if (findFirstEmpRole2Query == null)
            {
                Expression<Func<ErpContext, string, string, bool, EmpRole>> expression =
      (ctx, company_ex, empID_ex, primary_ex) =>
        (from row in ctx.EmpRole
         where row.Company == company_ex &&
         row.EmpID == empID_ex &&
         row.Primary == primary_ex
         select row).FirstOrDefault();
                findFirstEmpRole2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpRole2Query(this.Db, company, empID, primary);
        }


        class EmpRoleExpression3ColumnResult
        {
            public string Company { get; set; }
            public string EmpID { get; set; }
            public string RoleCd { get; set; }
        }
        static Func<ErpContext, string, string, IEnumerable<EmpRoleExpression3ColumnResult>> selectEmpRoleQuery;
        private IEnumerable<EmpRoleExpression3ColumnResult> SelectEmpRole(string company, string empID)
        {
            if (selectEmpRoleQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<EmpRoleExpression3ColumnResult>>> expression =
      (ctx, company_ex, empID_ex) =>
        (from row in ctx.EmpRole
         where row.Company == company_ex &&
         row.EmpID == empID_ex
         select new EmpRoleExpression3ColumnResult() { Company = row.Company, EmpID = row.EmpID, RoleCd = row.RoleCd });
                selectEmpRoleQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectEmpRoleQuery(this.Db, company, empID);
        }


        class EmpRoleExpression7ColumnResult
        {
            public string Company { get; set; }
            public string EmpID { get; set; }
            public bool Primary { get; set; }
            public string RoleCd { get; set; }
        }
        static Func<ErpContext, string, string, bool, IEnumerable<EmpRoleExpression7ColumnResult>> selectEmpRoleQuery_2;
        private IEnumerable<EmpRoleExpression7ColumnResult> SelectEmpRole(string company, string empID, bool primary)
        {
            if (selectEmpRoleQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, bool, IEnumerable<EmpRoleExpression7ColumnResult>>> expression =
      (ctx, company_ex, empID_ex, primary_ex) =>
        (from row in ctx.EmpRole
         where row.Company == company_ex &&
         row.EmpID == empID_ex &&
         row.Primary == primary_ex
         select new EmpRoleExpression7ColumnResult() { Company = row.Company, EmpID = row.EmpID, Primary = row.Primary, RoleCd = row.RoleCd });
                selectEmpRoleQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return selectEmpRoleQuery_2(this.Db, company, empID, primary);
        }
        #endregion EmpRole Queries

        #region EmpRoleRt Queries

        static Func<ErpContext, string, string, string, string, bool> existsEmpRoleRtQuery;
        private bool ExistsEmpRoleRt(string company, string empID, string roleCd, string timeTypCd)
        {
            if (existsEmpRoleRtQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, bool>> expression =
      (ctx, company_ex, empID_ex, roleCd_ex, timeTypCd_ex) =>
        (from row in ctx.EmpRoleRt
         where row.Company == company_ex &&
         row.EmpID == empID_ex &&
         row.RoleCd == roleCd_ex &&
         row.TimeTypCd == timeTypCd_ex
         select row).Any();
                existsEmpRoleRtQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsEmpRoleRtQuery(this.Db, company, empID, roleCd, timeTypCd);
        }
        #endregion EmpRoleRt Queries

        #region Equip Queries

        static Func<ErpContext, string, string, Equip> findFirstEquipQuery;
        private Equip FindFirstEquip(string company, string equipID)
        {
            if (findFirstEquipQuery == null)
            {
                Expression<Func<ErpContext, string, string, Equip>> expression =
      (ctx, company_ex, equipID_ex) =>
        (from row in ctx.Equip
         where row.Company == company_ex &&
         row.EquipID == equipID_ex
         select row).FirstOrDefault();
                findFirstEquipQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEquipQuery(this.Db, company, equipID);
        }


        static Func<ErpContext, string, string, Equip> findFirstEquip2Query;
        private Equip FindFirstEquip2(string company, string equipID)
        {
            if (findFirstEquip2Query == null)
            {
                Expression<Func<ErpContext, string, string, Equip>> expression =
      (ctx, company_ex, equipID_ex) =>
        (from row in ctx.Equip
         where row.Company == company_ex &&
         row.EquipID == equipID_ex
         select row).FirstOrDefault();
                findFirstEquip2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEquip2Query(this.Db, company, equipID);
        }


        static Func<ErpContext, string, string, string, bool, IEnumerable<Equip>> selectEquipQuery;
        private IEnumerable<Equip> SelectEquip(string company, string resourceID, string laborMeterOpt, bool inActive)
        {
            if (selectEquipQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool, IEnumerable<Equip>>> expression =
      (ctx, company_ex, resourceID_ex, laborMeterOpt_ex, inActive_ex) =>
        (from row in ctx.Equip
         where row.Company == company_ex &&
         row.ResourceID == resourceID_ex &&
         row.LaborMeterOpt != laborMeterOpt_ex &&
         row.InActive == inActive_ex
         select row);
                selectEquipQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectEquipQuery(this.Db, company, resourceID, laborMeterOpt, inActive);
        }


        static Func<ErpContext, string, string, string, bool, IEnumerable<Equip>> selectEquip2Query;
        private IEnumerable<Equip> SelectEquip2(string company, string resourceID, string laborMeterOpt, bool inActive)
        {
            if (selectEquip2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, bool, IEnumerable<Equip>>> expression =
      (ctx, company_ex, resourceID_ex, laborMeterOpt_ex, inActive_ex) =>
        (from row in ctx.Equip
         where row.Company == company_ex &&
         row.ResourceID == resourceID_ex &&
         row.LaborMeterOpt != laborMeterOpt_ex &&
         row.InActive == inActive_ex
         select row);
                selectEquip2Query = DBExpressionCompiler.Compile(expression);
            }

            return selectEquip2Query(this.Db, company, resourceID, laborMeterOpt, inActive);
        }
        #endregion Equip Queries

        #region FirstArt Queries

        static Func<ErpContext, string, string, string, int, int, string, bool> existsFirstArtQuery;
        private bool ExistsFirstArt(string company, string fastatus, string jobNum, int assemblySeq, int oprSeq, string resourceID)
        {
            if (existsFirstArtQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, int, string, bool>> expression =
      (ctx, company_ex, fastatus_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, resourceID_ex) =>
        (from row in ctx.FirstArt
         where row.Company == company_ex &&
         row.FAStatus == fastatus_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.ResourceID == resourceID_ex
         select row).Any();
                existsFirstArtQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsFirstArtQuery(this.Db, company, fastatus, jobNum, assemblySeq, oprSeq, resourceID);
        }


        static Func<ErpContext, string, string, int, int, string, bool> existsFirstArtQuery_2;
        private bool ExistsFirstArt(string company, string jobNum, int assemblySeq, int oprSeq, string resourceID)
        {
            if (existsFirstArtQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, resourceID_ex) =>
        (from row in ctx.FirstArt
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.ResourceID == resourceID_ex
         select row).Any();
                existsFirstArtQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return existsFirstArtQuery_2(this.Db, company, jobNum, assemblySeq, oprSeq, resourceID);
        }


        static Func<ErpContext, string, string, int, int, string, FirstArt> findLastFirstArtQuery;
        private FirstArt FindLastFirstArt(string company, string jobNum, int assemblySeq, int oprSeq, string resourceID)
        {
            if (findLastFirstArtQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, FirstArt>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, resourceID_ex) =>
        (from row in ctx.FirstArt
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.ResourceID == resourceID_ex
         orderby row.Company, row.JobNum, row.AssemblySeq, row.OprSeq, row.ResourceID, row.SeqNum
         select row).LastOrDefault();
                findLastFirstArtQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastFirstArtQuery(this.Db, company, jobNum, assemblySeq, oprSeq, resourceID);
        }


        static Func<ErpContext, string, string, int, int, string, FirstArt> findLastFirstArt2Query;
        private FirstArt FindLastFirstArt2(string company, string jobNum, int assemblySeq, int oprSeq, string resourceID)
        {
            if (findLastFirstArt2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, FirstArt>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, resourceID_ex) =>
        (from row in ctx.FirstArt
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.ResourceID == resourceID_ex
         orderby row.Company, row.JobNum, row.AssemblySeq, row.OprSeq, row.ResourceID, row.SeqNum
         select row).LastOrDefault();
                findLastFirstArt2Query = DBExpressionCompiler.Compile(expression);
            }

            return findLastFirstArt2Query(this.Db, company, jobNum, assemblySeq, oprSeq, resourceID);
        }
        #endregion FirstArt Queries

        #region FSCallhd Queries

        static Func<ErpContext, string, int, bool> existsFSCallhdQuery;
        private bool ExistsFSCallhd(string company, int callNum)
        {
            if (existsFSCallhdQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool>> expression =
      (ctx, company_ex, callNum_ex) =>
        (from row in ctx.FSCallhd
         where row.Company == company_ex &&
         row.CallNum == callNum_ex
         select row).Any();
                existsFSCallhdQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsFSCallhdQuery(this.Db, company, callNum);
        }


        static Func<ErpContext, string, int, FSCallhd> findFirstFSCallhdQuery;
        private FSCallhd FindFirstFSCallhd(string company, int callNum)
        {
            if (findFirstFSCallhdQuery == null)
            {
                Expression<Func<ErpContext, string, int, FSCallhd>> expression =
      (ctx, company_ex, callNum_ex) =>
        (from row in ctx.FSCallhd
         where row.Company == company_ex &&
         row.CallNum == callNum_ex
         select row).FirstOrDefault();
                findFirstFSCallhdQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstFSCallhdQuery(this.Db, company, callNum);
        }


        static Func<ErpContext, string, int, FSCallhd> findFirstFSCallhd2Query;
        private FSCallhd FindFirstFSCallhd2(string company, int callNum)
        {
            if (findFirstFSCallhd2Query == null)
            {
                Expression<Func<ErpContext, string, int, FSCallhd>> expression =
      (ctx, company_ex, callNum_ex) =>
        (from row in ctx.FSCallhd
         where row.Company == company_ex &&
         row.CallNum == callNum_ex
         select row).FirstOrDefault();
                findFirstFSCallhd2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstFSCallhd2Query(this.Db, company, callNum);
        }

        //HOLDLOCK



        static Func<ErpContext, string, int, FSCallhd> findFirstFSCallhdWithUpdLockQuery;
        private FSCallhd FindFirstFSCallhdWithUpdLock(string company, int callNum)
        {
            if (findFirstFSCallhdWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, FSCallhd>> expression =
      (ctx, company_ex, callNum_ex) =>
        (from row in ctx.FSCallhd.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.CallNum == callNum_ex
         select row).FirstOrDefault();
                findFirstFSCallhdWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstFSCallhdWithUpdLockQuery(this.Db, company, callNum);
        }
        #endregion FSCallhd Queries

        #region HCMLaborDtlSync Queries
        private static Func<ErpContext, string, Guid, bool> existsHCMLaborDtlSyncQuery;
        private bool ExistsHCMLaborDtlSync(string company, Guid sysRowID)
        {
            if (existsHCMLaborDtlSyncQuery == null)
            {
                Expression<Func<ErpContext, string, Guid, bool>> expression =
                    (context, company_ex, sysRowID_ex) =>
                    (from row in context.HCMLaborDtlSync
                     where row.Company == company_ex &&
                     row.LaborDtlSysRowID == sysRowID_ex &&
                     row.Status != ""
                     select row)
                    .Any();
                existsHCMLaborDtlSyncQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsHCMLaborDtlSyncQuery(this.Db, company, sysRowID);
        }

        private class HCMLaborDtlSyncResult
        {
            public string Status { get; set; }
        }
        private static Func<ErpContext, string, Guid, HCMLaborDtlSyncResult> findFirstHCMLaborDtlSyncQuery;
        private HCMLaborDtlSyncResult FindFirstHCMLaborDtlSync(string company, Guid sysRowID)
        {
            if (findFirstHCMLaborDtlSyncQuery == null)
            {
                Expression<Func<ErpContext, string, Guid, HCMLaborDtlSyncResult>> expression =
                    (context, company_ex, sysRowID_ex) =>
                    (from row in context.HCMLaborDtlSync
                     where row.Company == company_ex &&
                     row.LaborDtlSysRowID == sysRowID_ex
                     select new HCMLaborDtlSyncResult
                     {
                         Status = row.Status
                     })
                    .FirstOrDefault();
                findFirstHCMLaborDtlSyncQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstHCMLaborDtlSyncQuery(this.Db, company, sysRowID);
        }
        #endregion HCMLaborDtlSync Queries

        #region Indirect Queries

        static Func<ErpContext, string, string, Indirect> findFirstIndirectQuery;
        private Indirect FindFirstIndirect(string company, string indirectCode)
        {
            if (findFirstIndirectQuery == null)
            {
                Expression<Func<ErpContext, string, string, Indirect>> expression =
      (ctx, company_ex, indirectCode_ex) =>
        (from row in ctx.Indirect
         where row.Company == company_ex &&
         row.IndirectCode == indirectCode_ex
         select row).FirstOrDefault();
                findFirstIndirectQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstIndirectQuery(this.Db, company, indirectCode);
        }


        static Func<ErpContext, string, string, Indirect> findFirstIndirect2Query;
        private Indirect FindFirstIndirect2(string company, string indirectCode)
        {
            if (findFirstIndirect2Query == null)
            {
                Expression<Func<ErpContext, string, string, Indirect>> expression =
      (ctx, company_ex, indirectCode_ex) =>
        (from row in ctx.Indirect
         where row.Company == company_ex &&
         row.IndirectCode == indirectCode_ex
         select row).FirstOrDefault();
                findFirstIndirect2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstIndirect2Query(this.Db, company, indirectCode);
        }


        static Func<ErpContext, string, string, Indirect> findFirstIndirect3Query;
        private Indirect FindFirstIndirect3(string company, string indirectCode)
        {
            if (findFirstIndirect3Query == null)
            {
                Expression<Func<ErpContext, string, string, Indirect>> expression =
      (ctx, company_ex, indirectCode_ex) =>
        (from row in ctx.Indirect
         where row.Company == company_ex &&
         row.IndirectCode == indirectCode_ex
         select row).FirstOrDefault();
                findFirstIndirect3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstIndirect3Query(this.Db, company, indirectCode);
        }


        static Func<ErpContext, string, string, Indirect> findFirstIndirect4Query;
        private Indirect FindFirstIndirect4(string company, string indirectCode)
        {
            if (findFirstIndirect4Query == null)
            {
                Expression<Func<ErpContext, string, string, Indirect>> expression =
      (ctx, company_ex, indirectCode_ex) =>
        (from row in ctx.Indirect
         where row.Company == company_ex &&
         row.IndirectCode == indirectCode_ex
         select row).FirstOrDefault();
                findFirstIndirect4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstIndirect4Query(this.Db, company, indirectCode);
        }


        static Func<ErpContext, string, string, Indirect> findFirstIndirect5Query;
        private Indirect FindFirstIndirect5(string company, string indirectCode)
        {
            if (findFirstIndirect5Query == null)
            {
                Expression<Func<ErpContext, string, string, Indirect>> expression =
      (ctx, company_ex, indirectCode_ex) =>
        (from row in ctx.Indirect
         where row.Company == company_ex &&
         row.IndirectCode == indirectCode_ex
         select row).FirstOrDefault();
                findFirstIndirect5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstIndirect5Query(this.Db, company, indirectCode);
        }
        #endregion Indirect Queries

        #region InspResults Queries

        static Func<ErpContext, string, string, int, int, bool> existsInspResultsQuery;
        private bool ExistsInspResults(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (existsInspResultsQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.InspResults
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).Any();
                existsInspResultsQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsInspResultsQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }
        #endregion InspResults Queries

        #region JCDept Queries

        static Func<ErpContext, string, string, bool> existsJCDeptQuery;
        private bool ExistsJCDept(string company, string jcdept1)
        {
            if (existsJCDeptQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, jcdept1_ex) =>
        (from row in ctx.JCDept
         where row.Company == company_ex &&
         row.JCDept1 == jcdept1_ex
         select row).Any();
                existsJCDeptQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJCDeptQuery(this.Db, company, jcdept1);
        }


        static Func<ErpContext, string, string, JCDept> findFirstJCDeptQuery;
        private JCDept FindFirstJCDept(string company, string jcdept1)
        {
            if (findFirstJCDeptQuery == null)
            {
                Expression<Func<ErpContext, string, string, JCDept>> expression =
      (ctx, company_ex, jcdept1_ex) =>
        (from row in ctx.JCDept
         where row.Company == company_ex &&
         row.JCDept1 == jcdept1_ex
         select row).FirstOrDefault();
                findFirstJCDeptQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJCDeptQuery(this.Db, company, jcdept1);
        }
        #endregion JCDept Queries

        #region JCShift Queries

        static Func<ErpContext, string, int, bool> existsJCShiftQuery;
        private bool ExistsJCShift(string company, int shift)
        {
            if (existsJCShiftQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool>> expression =
      (ctx, company_ex, shift_ex) =>
        (from row in ctx.JCShift
         where row.Company == company_ex &&
         row.Shift == shift_ex
         select row).Any();
                existsJCShiftQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJCShiftQuery(this.Db, company, shift);
        }


        static Func<ErpContext, string, int, JCShift> findFirstJCShiftQuery;
        private JCShift FindFirstJCShift(string company, int shift)
        {
            if (findFirstJCShiftQuery == null)
            {
                Expression<Func<ErpContext, string, int, JCShift>> expression =
      (ctx, company_ex, shift_ex) =>
        (from row in ctx.JCShift
         where row.Company == company_ex &&
         row.Shift == shift_ex
         select row).FirstOrDefault();
                findFirstJCShiftQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJCShiftQuery(this.Db, company, shift);
        }


        private class JCShiftResult
        {
            public decimal StartTime { get; set; }
        }

        static Func<ErpContext, string, int, JCShiftResult> findFirstJCShift2Query;
        private JCShiftResult FindFirstJCShift2(string company, int shift)
        {
            if (findFirstJCShift2Query == null)
            {
                Expression<Func<ErpContext, string, int, JCShiftResult>> expression =
                (ctx, company_ex, shift_ex) =>
                (from row in ctx.JCShift
                 where row.Company == company_ex &&
                 row.Shift == shift_ex
                 select new JCShiftResult
                 {
                     StartTime = row.StartTime
                 }).FirstOrDefault();
                findFirstJCShift2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJCShift2Query(this.Db, company, shift);
        }


        static Func<ErpContext, string, int, JCShift> findFirstJCShift3Query;
        private JCShift FindFirstJCShift3(string company, int shift)
        {
            if (findFirstJCShift3Query == null)
            {
                Expression<Func<ErpContext, string, int, JCShift>> expression =
      (ctx, company_ex, shift_ex) =>
        (from row in ctx.JCShift
         where row.Company == company_ex &&
         row.Shift == shift_ex
         select row).FirstOrDefault();
                findFirstJCShift3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJCShift3Query(this.Db, company, shift);
        }


        static Func<ErpContext, string, int, JCShift> findFirstJCShift4Query;
        private JCShift FindFirstJCShift4(string company, int shift)
        {
            if (findFirstJCShift4Query == null)
            {
                Expression<Func<ErpContext, string, int, JCShift>> expression =
      (ctx, company_ex, shift_ex) =>
        (from row in ctx.JCShift
         where row.Company == company_ex &&
         row.Shift == shift_ex
         select row).FirstOrDefault();
                findFirstJCShift4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJCShift4Query(this.Db, company, shift);
        }


        static Func<ErpContext, string, int, JCShift> findFirstJCShift5Query;
        private JCShift FindFirstJCShift5(string company, int shift)
        {
            if (findFirstJCShift5Query == null)
            {
                Expression<Func<ErpContext, string, int, JCShift>> expression =
      (ctx, company_ex, shift_ex) =>
        (from row in ctx.JCShift
         where row.Company == company_ex &&
         row.Shift == shift_ex
         select row).FirstOrDefault();
                findFirstJCShift5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJCShift5Query(this.Db, company, shift);
        }


        static Func<ErpContext, string, int, JCShift> findFirstJCShift6Query;
        private JCShift FindFirstJCShift6(string company, int shift)
        {
            if (findFirstJCShift6Query == null)
            {
                Expression<Func<ErpContext, string, int, JCShift>> expression =
      (ctx, company_ex, shift_ex) =>
        (from row in ctx.JCShift
         where row.Company == company_ex &&
         row.Shift == shift_ex
         select row).FirstOrDefault();
                findFirstJCShift6Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJCShift6Query(this.Db, company, shift);
        }


        static Func<ErpContext, string, int, JCShift> findFirstJCShift7Query;
        private JCShift FindFirstJCShift7(string company, int shift)
        {
            if (findFirstJCShift7Query == null)
            {
                Expression<Func<ErpContext, string, int, JCShift>> expression =
      (ctx, company_ex, shift_ex) =>
        (from row in ctx.JCShift
         where row.Company == company_ex &&
         row.Shift == shift_ex
         select row).FirstOrDefault();
                findFirstJCShift7Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJCShift7Query(this.Db, company, shift);
        }


        static Func<ErpContext, string, int, JCShift> findFirstJCShift8Query;
        private JCShift FindFirstJCShift8(string company, int shift)
        {
            if (findFirstJCShift8Query == null)
            {
                Expression<Func<ErpContext, string, int, JCShift>> expression =
      (ctx, company_ex, shift_ex) =>
        (from row in ctx.JCShift
         where row.Company == company_ex &&
         row.Shift == shift_ex
         select row).FirstOrDefault();
                findFirstJCShift8Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJCShift8Query(this.Db, company, shift);
        }

        static Func<ErpContext, string, int, string> findFirstJCShiftDescriptionQuery;
        private string FindFirstJCShiftDescription(string company, int shift)
        {
            if (findFirstJCShiftDescriptionQuery == null)
            {
                Expression<Func<ErpContext, string, int, string>> expression =
      (ctx, company_ex, shift_ex) =>
        (from row in ctx.JCShift
         where row.Company == company_ex &&
         row.Shift == shift_ex
         select row.Description).FirstOrDefault();
                findFirstJCShiftDescriptionQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJCShiftDescriptionQuery(this.Db, company, shift);
        }
        #endregion JCShift Queries

        #region JCSyst Queries

        static Func<ErpContext, string, JCSyst> findFirstJCSystQuery;
        private JCSyst FindFirstJCSyst(string company)
        {
            if (findFirstJCSystQuery == null)
            {
                Expression<Func<ErpContext, string, JCSyst>> expression =
      (ctx, company_ex) =>
        (from row in ctx.JCSyst
         where row.Company == company_ex
         select row).FirstOrDefault();
                findFirstJCSystQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJCSystQuery(this.Db, company);
        }

        private class JCSystPartialRow : Epicor.Data.TempRowBase
        {
            public bool AdvancedLaborRate { get; set; }
            public string ClockFormat { get; set; }
        }

        private static Func<ErpContext, string, JCSystPartialRow> getJCSystJobProductionValuesQuery;
        private JCSystPartialRow GetJCSystJobProductionValues(string company)
        {
            if (getJCSystJobProductionValuesQuery == null)
            {
                Expression<Func<ErpContext, string, JCSystPartialRow>> expression =
                    (context, company_ex) =>
                    (from row in context.JCSyst
                     where row.Company == company_ex
                     select new JCSystPartialRow { AdvancedLaborRate = row.AdvancedLaborRate, ClockFormat = row.ClockFormat })
                    .FirstOrDefault();
                getJCSystJobProductionValuesQuery = DBExpressionCompiler.Compile(expression);
            }

            return getJCSystJobProductionValuesQuery(this.Db, company);
        }
        #endregion JCSyst Queries

        #region JobAsmbl Queries

        static Func<ErpContext, string, string, int, bool> existsJobAsmblQuery;
        private bool ExistsJobAsmbl(string company, string jobNum, int assemblySeq)
        {
            if (existsJobAsmblQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select row).Any();
                existsJobAsmblQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobAsmblQuery(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, int, int, bool> existsJobAsmblQuery_2;
        private bool ExistsJobAsmbl(string company, string jobNum, int assemblySeq, int finalOpr)
        {
            if (existsJobAsmblQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, finalOpr_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.FinalOpr == finalOpr_ex
         select row).Any();
                existsJobAsmblQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return existsJobAsmblQuery_2(this.Db, company, jobNum, assemblySeq, finalOpr);
        }


        static Func<ErpContext, string, string, int, int> getJobAsmblFinalOprQuery;
        private int GetJobAsmblFinalOpr(string company, string jobNum, int assemblySeq)
        {
            if (getJobAsmblFinalOprQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int>> expression =
                (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
               (from row in ctx.JobAsmbl
                where row.Company == company_ex &&
                           row.JobNum == jobNum_ex &&
                           row.AssemblySeq == assemblySeq_ex
                select row.FinalOpr).FirstOrDefault();
                getJobAsmblFinalOprQuery = DBExpressionCompiler.Compile(expression);
            }
            return getJobAsmblFinalOprQuery(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, int, JobAsmbl> findFirstJobAsmbl2Query;
        private JobAsmbl FindFirstJobAsmbl2(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmbl2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmbl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select row).FirstOrDefault();
                findFirstJobAsmbl2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmbl2Query(this.Db, company, jobNum, assemblySeq);
        }

        static Func<ErpContext, string, string, int, string> findFirstJobAsmblIUMQuery;
        private string FindFirstJobAsmblIUM(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmblIUMQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select row.IUM).FirstOrDefault();
                findFirstJobAsmblIUMQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmblIUMQuery(this.Db, company, jobNum, assemblySeq);
        }

        static Func<ErpContext, string, string, int, JobAsmbl> findFirstJobAsmbl3Query;
        private JobAsmbl FindFirstJobAsmbl3(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmbl3Query == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmbl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select row).FirstOrDefault();
                findFirstJobAsmbl3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmbl3Query(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, int, JobAsmbl> findFirstJobAsmbl4Query;
        private JobAsmbl FindFirstJobAsmbl4(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmbl4Query == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmbl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select row).FirstOrDefault();
                findFirstJobAsmbl4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmbl4Query(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, int, JobAsmbl> findFirstJobAsmbl5Query;
        private JobAsmbl FindFirstJobAsmbl5(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmbl5Query == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmbl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select row).FirstOrDefault();
                findFirstJobAsmbl5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmbl5Query(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, int, JobAsmbl> findFirstJobAsmbl6Query;
        private JobAsmbl FindFirstJobAsmbl6(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmbl6Query == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmbl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select row).FirstOrDefault();
                findFirstJobAsmbl6Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmbl6Query(this.Db, company, jobNum, assemblySeq);
        }

        class JobAsmblPartResult
        {
            public string PartNum { get; set; }
            public string PartDescription { get; set; }
        }
        static Func<ErpContext, string, string, int, JobAsmblPartResult> findFirstJobAsmblPartQuery;
        private JobAsmblPartResult FindFirstJobAsmblPart(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmblPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmblPartResult>> expression =
                (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
                (from row in ctx.JobAsmbl
                 where row.Company == company_ex
                    && row.JobNum == jobNum_ex
                    && row.AssemblySeq == assemblySeq_ex
                 select new JobAsmblPartResult()
                 {
                     PartNum = row.PartNum,
                     PartDescription = row.Description
                 }).FirstOrDefault();
                findFirstJobAsmblPartQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstJobAsmblPartQuery(this.Db, company, jobNum, assemblySeq);
        }

        class JobAsmblPartialResult
        {
            public string RevisionNum { get; set; }
            public string IUM { get; set; }
        }
        static Func<ErpContext, string, string, int, JobAsmblPartialResult> findFirstJobAsmblPartialQuery;
        private JobAsmblPartialResult FindFirstJobAsmblPartial(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmblPartialQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmblPartialResult>> expression =
                (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
                (from row in ctx.JobAsmbl
                 where row.Company == company_ex
                    && row.JobNum == jobNum_ex
                    && row.AssemblySeq == assemblySeq_ex
                 select new JobAsmblPartialResult()
                 {
                     RevisionNum = row.RevisionNum,
                     IUM = row.IUM
                 }).FirstOrDefault();
                findFirstJobAsmblPartialQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstJobAsmblPartialQuery(this.Db, company, jobNum, assemblySeq);
        }
        #endregion JobAsmbl Queries

        #region JobHead Queries
        static Func<ErpContext, string, string, bool, bool, bool> existsJobHeadQuery;
        private bool ExistsJobHead(string company, string jobNum, bool jobClosed, bool jobReleased)
        {
            if (existsJobHeadQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool, bool>> expression =
      (ctx, company_ex, jobNum_ex, jobClosed_ex, jobReleased_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.JobClosed == jobClosed_ex &&
         row.JobReleased == jobReleased_ex
         select row).Any();
                existsJobHeadQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobHeadQuery(this.Db, company, jobNum, jobClosed, jobReleased);
        }

        static Func<ErpContext, string, string, bool, bool> existsJobHeadQuery_2;
        private bool ExistsJobHead(string company, string jobNum, bool jobClosed)
        {
            if (existsJobHeadQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
      (ctx, company_ex, jobNum_ex, jobClosed_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.JobClosed == jobClosed_ex
         select row).Any();
                existsJobHeadQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return existsJobHeadQuery_2(this.Db, company, jobNum, jobClosed);
        }


        static Func<ErpContext, string, string, bool> existsJobHeadForJobNumQuery;
        private bool ExistsJobHeadForJobNum(string company, string jobNum)
        {
            if (existsJobHeadForJobNumQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).Any();
                existsJobHeadForJobNumQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobHeadForJobNumQuery(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHeadQuery;
        private JobHead FindFirstJobHead(string company, string jobNum)
        {
            if (findFirstJobHeadQuery == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHeadQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHeadQuery(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, string, JobHead> findFirstJobHeadQuery_2;
        private JobHead FindFirstJobHead(string company, string plant, string jobNum)
        {
            if (findFirstJobHeadQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, JobHead>> expression =
      (ctx, company_ex, plant_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHeadQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHeadQuery_2(this.Db, company, plant, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead10Query;
        private JobHead FindFirstJobHead10(string company, string jobNum)
        {
            if (findFirstJobHead10Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead10Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead10Query(this.Db, company, jobNum);
        }


        class JobHeadPartialResult
        {
            public string RevisionNum { get; set; }
            public string IUM { get; set; }
        }
        static Func<ErpContext, string, string, JobHeadPartialResult> findFirstJobHeadPartialQuery;
        private JobHeadPartialResult FindFirstJobHeadPartial(string company, string jobNum)
        {
            if (findFirstJobHeadPartialQuery == null)
            {
                Expression<Func<ErpContext, string, string, JobHeadPartialResult>> expression =
                (ctx, company_ex, jobNum_ex) =>
                (from row in ctx.JobHead
                 where row.Company == company_ex
                    && row.JobNum == jobNum_ex

                 select new JobHeadPartialResult()
                 {
                     RevisionNum = row.RevisionNum,
                     IUM = row.IUM
                 }).FirstOrDefault();
                findFirstJobHeadPartialQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstJobHeadPartialQuery(this.Db, company, jobNum);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, JobHead> findFirstJobHead12Query;
        private JobHead FindFirstJobHead12(string company, string jobNum)
        {
            if (findFirstJobHead12Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead12Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead12Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead13Query;
        private JobHead FindFirstJobHead13(string company, string jobNum)
        {
            if (findFirstJobHead13Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead13Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead13Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead14Query;
        private JobHead FindFirstJobHead14(string company, string jobNum)
        {
            if (findFirstJobHead14Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead14Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead14Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead15Query;
        private JobHead FindFirstJobHead15(string company, string jobNum)
        {
            if (findFirstJobHead15Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead15Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead15Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead16Query;
        private JobHead FindFirstJobHead16(string company, string jobNum)
        {
            if (findFirstJobHead16Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead16Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead16Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead17Query;
        private JobHead FindFirstJobHead17(string company, string jobNum)
        {
            if (findFirstJobHead17Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead17Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead17Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, string> findFirstJobHeadJobTypeQuery;
        private string FindFirstJobHeadJobType(string company, string jobNum)
        {
            if (findFirstJobHeadJobTypeQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row.JobType).FirstOrDefault();
                findFirstJobHeadJobTypeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHeadJobTypeQuery(this.Db, company, jobNum);
        }

        static Func<ErpContext, string, string, JobHead> findFirstJobHead19Query;
        private JobHead FindFirstJobHead19(string company, string jobNum)
        {
            if (findFirstJobHead19Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead19Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead19Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead2Query;
        private JobHead FindFirstJobHead2(string company, string jobNum)
        {
            if (findFirstJobHead2Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead2Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, string, JobHead> findFirstJobHead2Query_2;
        private JobHead FindFirstJobHead2(string company, string plant, string jobNum)
        {
            if (findFirstJobHead2Query_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, JobHead>> expression =
      (ctx, company_ex, plant_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead2Query_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead2Query_2(this.Db, company, plant, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead3Query;
        private JobHead FindFirstJobHead3(string company, string jobNum)
        {
            if (findFirstJobHead3Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead3Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, string, JobHead> findFirstJobHead3Query_2;
        private JobHead FindFirstJobHead3(string company, string jobNum, string jobType)
        {
            if (findFirstJobHead3Query_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex, jobType_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.JobType == jobType_ex
         select row).FirstOrDefault();
                findFirstJobHead3Query_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead3Query_2(this.Db, company, jobNum, jobType);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead4Query;
        private JobHead FindFirstJobHead4(string company, string jobNum)
        {
            if (findFirstJobHead4Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead4Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead5Query;
        private JobHead FindFirstJobHead5(string company, string jobNum)
        {
            if (findFirstJobHead5Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead5Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead6Query;
        private JobHead FindFirstJobHead6(string company, string jobNum)
        {
            if (findFirstJobHead6Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead6Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead6Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead7Query;
        private JobHead FindFirstJobHead7(string company, string jobNum)
        {
            if (findFirstJobHead7Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead7Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead7Query(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, JobHead> findFirstJobHead8Query;
        private JobHead FindFirstJobHead8(string company, string jobNum)
        {
            if (findFirstJobHead8Query == null)
            {
                Expression<Func<ErpContext, string, string, JobHead>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobHead8Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHead8Query(this.Db, company, jobNum);
        }


        private class JobHeadResPrj
        {
            public string ProjectID { get; set; }
            public string PhaseID { get; set; }
        }


        static Func<ErpContext, string, string, JobHeadResPrj> findFirstJobHeadResPrjQuery;
        private JobHeadResPrj FindFirstJobHeadResPrj(string company, string jobNum)
        {
            if (findFirstJobHeadResPrjQuery == null)
            {
                Expression<Func<ErpContext, string, string, JobHeadResPrj>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select new JobHeadResPrj { ProjectID = row.ProjectID, PhaseID = row.PhaseID }).FirstOrDefault();
                findFirstJobHeadResPrjQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobHeadResPrjQuery(this.Db, company, jobNum);
        }

        private class JobHeadProject
        {
            public string Company { get; set; }
            public string JobNum { get; set; }
            public string ProjectID { get; set; }
        }
        static Func<ErpContext, string, string, JobHeadProject> findFirstJobHeadProjectQuery;
        private JobHeadProject FindFirstJobHeadProject(string company, string jobNum)
        {
            if (findFirstJobHeadProjectQuery == null)
            {
                Expression<Func<ErpContext, string, string, JobHeadProject>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobHead
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select new JobHeadProject()
         {
             Company = row.Company,
             JobNum = row.JobNum,
             ProjectID = row.ProjectID
         }
   ).FirstOrDefault();
                findFirstJobHeadProjectQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstJobHeadProjectQuery(this.Db, company, jobNum);
        }

        private class JobHeadPartProcessMode
        {
            public string PartNum { get; set; }
            public string ProcessMode { get; set; }
        }


        static Func<ErpContext, string, string, JobHeadPartProcessMode> getJobHeadPartProcessModeQuery;
        private JobHeadPartProcessMode GetJobHeadPartProcessMode(string company, string jobNum)
        {
            if (getJobHeadPartProcessModeQuery == null)
            {
                Expression<Func<ErpContext, string, string, JobHeadPartProcessMode>> expression =
                (ctx, company_ex, jobNum_ex) =>
                (from row in ctx.JobHead
                 where row.Company == company_ex &&
                       row.JobNum == jobNum_ex
                 select new JobHeadPartProcessMode { PartNum = row.PartNum, ProcessMode = row.ProcessMode }).FirstOrDefault();
                getJobHeadPartProcessModeQuery = DBExpressionCompiler.Compile(expression);
            }

            return getJobHeadPartProcessModeQuery(this.Db, company, jobNum);
        }
        #endregion JobHead Queries

        #region JobMtl Queries
        static Func<ErpContext, string, string, int, int, bool> existsJobMtlReassignSNAsmQuery;
        private bool ExistsJobMtlReassignSNAsm(string company, string jobNum, int asmSeq, int mtlSeq)
        {
            if (existsJobMtlReassignSNAsmQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
                (ctx, company_ex, jobNum_ex, asmSeq_ex, mtlSeq_ex) =>
                (from row in ctx.JobMtl
                 where row.Company == company_ex &&
                 row.JobNum == jobNum_ex &&
                 row.AssemblySeq == asmSeq_ex &&
                 row.MtlSeq == mtlSeq_ex
                 select row).Any();
                existsJobMtlReassignSNAsmQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobMtlReassignSNAsmQuery(this.Db, company, jobNum, asmSeq, mtlSeq);
        }

        #endregion

        #region JobOpDtl Queries

        static Func<ErpContext, Guid, JobOpDtl> findFirstJobOpDtlQuery_2;
        private JobOpDtl FindFirstJobOpDtl(Guid sysRowID)
        {
            if (findFirstJobOpDtlQuery_2 == null)
            {
                Expression<Func<ErpContext, Guid, JobOpDtl>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.JobOpDtl
         where row.SysRowID == sysRowID_ex
         select row).FirstOrDefault();
                findFirstJobOpDtlQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOpDtlQuery_2(this.Db, sysRowID);
        }


        static Func<ErpContext, string, string, int, int, int, JobOpDtl> findFirstJobOpDtl2Query;
        private JobOpDtl FindFirstJobOpDtl2(string company, string jobNum, int assemblySeq, int oprSeq, int opDtlSeq)
        {
            if (findFirstJobOpDtl2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, JobOpDtl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, opDtlSeq_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.OpDtlSeq == opDtlSeq_ex
         select row).FirstOrDefault();
                findFirstJobOpDtl2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOpDtl2Query(this.Db, company, jobNum, assemblySeq, oprSeq, opDtlSeq);
        }

        static Func<ErpContext, string, string, int, int, string, string, string, JobOpDtl> findFirstJobOpDtl2_1Query;
        private JobOpDtl FindFirstJobOpDtl2(string company, string jobNum, int assemblySeq, int oprSeq, string resID, string resGrpID, string capID)
        {
            if (findFirstJobOpDtl2_1Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, string, string, JobOpDtl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, resID_ex, resGrpID_ex, capID_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.ResourceID == resID_ex &&
         row.ResourceGrpID == resGrpID_ex &&
         row.CapabilityID == capID_ex &&
         row.OverrideRates == true
         select row).FirstOrDefault();
                findFirstJobOpDtl2_1Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOpDtl2_1Query(this.Db, company, jobNum, assemblySeq, oprSeq, resID, resGrpID, capID);
        }


        static Func<ErpContext, string, string, int, int, int, JobOpDtl> findFirstJobOpDtl3Query;
        private JobOpDtl FindFirstJobOpDtl3(string company, string jobNum, int assemblySeq, int oprSeq, int opDtlSeq)
        {
            if (findFirstJobOpDtl3Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, JobOpDtl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, opDtlSeq_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.OpDtlSeq == opDtlSeq_ex
         select row).FirstOrDefault();
                findFirstJobOpDtl3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOpDtl3Query(this.Db, company, jobNum, assemblySeq, oprSeq, opDtlSeq);
        }


        static Func<ErpContext, string, string, int, int, int, JobOpDtl> findFirstJobOpDtl4Query;
        private JobOpDtl FindFirstJobOpDtl4(string company, string jobNum, int assemblySeq, int oprSeq, int opDtlSeq)
        {
            if (findFirstJobOpDtl4Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, JobOpDtl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, opDtlSeq_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.OpDtlSeq == opDtlSeq_ex
         select row).FirstOrDefault();
                findFirstJobOpDtl4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOpDtl4Query(this.Db, company, jobNum, assemblySeq, oprSeq, opDtlSeq);
        }


        static Func<ErpContext, string, string, int, int, int, JobOpDtl> findFirstJobOpDtl5Query;
        private JobOpDtl FindFirstJobOpDtl5(string company, string jobNum, int assemblySeq, int oprSeq, int opDtlSeq)
        {
            if (findFirstJobOpDtl5Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, JobOpDtl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, opDtlSeq_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.OpDtlSeq == opDtlSeq_ex
         select row).FirstOrDefault();
                findFirstJobOpDtl5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOpDtl5Query(this.Db, company, jobNum, assemblySeq, oprSeq, opDtlSeq);
        }

        static Func<ErpContext, string, string, int, int, string, string, string, JobOpDtl> findFirstJobOpDtl6Query;
        private JobOpDtl FindFirstJobOpDtl6(string company, string jobNum, int assemblySeq, int oprSeq, string resourceGrpID, string setupOrProd, string setupOrProd2)
        {
            if (findFirstJobOpDtl6Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, string, string, JobOpDtl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, resourceGrpID_Ex, setupOrProd_ex, setupOrProd2_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.ResourceGrpID == resourceGrpID_Ex &&
          (row.SetupOrProd == setupOrProd_ex ||
         row.SetupOrProd == setupOrProd2_ex)
         select row).FirstOrDefault();
                findFirstJobOpDtl6Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOpDtl6Query(this.Db, company, jobNum, assemblySeq, oprSeq, resourceGrpID, setupOrProd, setupOrProd2);
        }


        static Func<ErpContext, string, string, int, int, string, string, IEnumerable<JobOpDtl>> selectJobOpDtlQuery;
        private IEnumerable<JobOpDtl> SelectJobOpDtl(string company, string jobNum, int assemblySeq, int oprSeq, string setupOrProd, string setupOrProd2)
        {
            if (selectJobOpDtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, string, IEnumerable<JobOpDtl>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, setupOrProd_ex, setupOrProd2_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
        (row.SetupOrProd == setupOrProd_ex ||
         row.SetupOrProd == setupOrProd2_ex)
         select row);
                selectJobOpDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectJobOpDtlQuery(this.Db, company, jobNum, assemblySeq, oprSeq, setupOrProd, setupOrProd2);
        }


        static Func<ErpContext, string, string, int, int, IEnumerable<JobOpDtl>> selectJobOpDtlQuery_2;
        private IEnumerable<JobOpDtl> SelectJobOpDtl(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (selectJobOpDtlQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, IEnumerable<JobOpDtl>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row);
                selectJobOpDtlQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return selectJobOpDtlQuery_2(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, IEnumerable<JobOpDtl>> selectJobOpDtl2Query;
        private IEnumerable<JobOpDtl> SelectJobOpDtl2(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (selectJobOpDtl2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, IEnumerable<JobOpDtl>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row);
                selectJobOpDtl2Query = DBExpressionCompiler.Compile(expression);
            }

            return selectJobOpDtl2Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, IEnumerable<JobOpDtl>> selectJobOpDtl3Query;
        private IEnumerable<JobOpDtl> SelectJobOpDtl3(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (selectJobOpDtl3Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, IEnumerable<JobOpDtl>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row);
                selectJobOpDtl3Query = DBExpressionCompiler.Compile(expression);
            }

            return selectJobOpDtl3Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, IEnumerable<JobOpDtl>> selectJobOpDtl4Query;
        private IEnumerable<JobOpDtl> SelectJobOpDtl4(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (selectJobOpDtl4Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, IEnumerable<JobOpDtl>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row);
                selectJobOpDtl4Query = DBExpressionCompiler.Compile(expression);
            }

            return selectJobOpDtl4Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }
        #endregion JobOpDtl Queries

        #region JobOper Queries

        static Func<ErpContext, string, string, int, int, bool> existsJobOperQuery;
        private bool ExistsJobOper(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (existsJobOperQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq > oprSeq_ex
         select row).Any();
                existsJobOperQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobOperQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        static Func<ErpContext, string, string, int, int, bool> existsJobOperSNRequiredOprQuery;
        private bool ExistsJobOperSNRequiredOpr(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (existsJobOperSNRequiredOprQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
              (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
                (from row in ctx.JobOper
                 where row.Company == company_ex &&
                 row.JobNum == jobNum_ex &&
                 row.AssemblySeq == assemblySeq_ex &&
                 row.OprSeq == oprSeq_ex &&
                 row.SNRequiredOpr == true
                 select row).Any();
                existsJobOperSNRequiredOprQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobOperSNRequiredOprQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        static Func<ErpContext, string, string, int, int, bool> isLaborEntryMethodTimeandBackflushQuery;
        private bool IsLaborEntryMethodTimeAndBackflush(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (isLaborEntryMethodTimeandBackflushQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.LaborEntryMethod == "X"
         select row).Any();
                isLaborEntryMethodTimeandBackflushQuery = DBExpressionCompiler.Compile(expression);
            }

            return isLaborEntryMethodTimeandBackflushQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        static Func<ErpContext, string, string, int, bool> existsLaborEntryMethodBFQuery;
        private bool ExistsLaborEntryMethodBF(string company, string jobNum, int assemblySeq)
        {
            if (existsLaborEntryMethodBFQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.LaborEntryMethod == "X"
         select row).Any();
                existsLaborEntryMethodBFQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborEntryMethodBFQuery(this.Db, company, jobNum, assemblySeq);
        }

        static Func<ErpContext, string, string, int, int, bool> IsThereAnyFurtherOperationsQuery;
        private bool IsThereAnyFurtherOperations(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (IsThereAnyFurtherOperationsQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         ((row.OprSeq > oprSeq_ex && row.SubContract != true))
         select row).Any();
                IsThereAnyFurtherOperationsQuery = DBExpressionCompiler.Compile(expression);
            }

            return IsThereAnyFurtherOperationsQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        static Func<ErpContext, string, string, int, int> maxFurtherOperationQuery;
        private int MaxFurtherOperation(string company, string jobNum, int assemblySeq)
        {
            if (maxFurtherOperationQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.SubContract != true
         orderby row.OprSeq descending
         select row.OprSeq
         ).FirstOrDefault();
                maxFurtherOperationQuery = DBExpressionCompiler.Compile(expression);
            }

            return maxFurtherOperationQuery(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, int, int, bool> existsJobOperForJobNumAssemblyOprSeqQuery;
        private bool ExistJobOperForJobNumAssemblyOprSeq(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (existsJobOperForJobNumAssemblyOprSeqQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).Any();
                existsJobOperForJobNumAssemblyOprSeqQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobOperForJobNumAssemblyOprSeqQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        static Func<ErpContext, string, string, int, int, bool> isFixHoursAndQtyOnlyQuery;
        private bool IsFixHoursAndQtyOnly(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (isFixHoursAndQtyOnlyQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.LaborEntryMethod == "Q" &&
         row.StdFormat == "HR"
         select row).Any();
                isFixHoursAndQtyOnlyQuery = DBExpressionCompiler.Compile(expression);
            }

            return isFixHoursAndQtyOnlyQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        static Func<ErpContext, string, string, int, int, decimal> getProdFixedHoursQuery;
        private decimal GetProdFixedHours(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (getProdFixedHoursQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, decimal>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select (row.EstProdHours - row.ActProdHours)).FirstOrDefault();
                getProdFixedHoursQuery = DBExpressionCompiler.Compile(expression);
            }

            return getProdFixedHoursQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        static Func<ErpContext, string, string, int, int, decimal> getSetupFixedHoursQuery;
        private decimal GetSetupFixedHours(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (getSetupFixedHoursQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, decimal>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select (row.EstSetHours - row.ActSetupHours)).FirstOrDefault();
                getSetupFixedHoursQuery = DBExpressionCompiler.Compile(expression);
            }

            return getSetupFixedHoursQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOperQuery;
        private JobOper FindFirstJobOper(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOperQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOperQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOperQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, bool, JobOper> findFirstJobOperQuery_2;
        private JobOper FindFirstJobOper(string company, string jobNum, int assemblySeq, int oprSeq, bool Param)
        {
            if (findFirstJobOperQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, Param_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
        (Param_ex)
         select row).FirstOrDefault();
                findFirstJobOperQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOperQuery_2(this.Db, company, jobNum, assemblySeq, oprSeq, Param);
        }


        static Func<ErpContext, string, string, int, JobOper> findFirstJobOperQuery_3;
        private JobOper FindFirstJobOper(string company, string jobNum, int oprSeq)
        {
            if (findFirstJobOperQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOperQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOperQuery_3(this.Db, company, jobNum, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOperPrimaryQuery;
        private JobOper FindFirstJobOperPrimary(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOperPrimaryQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOperPrimaryQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOperPrimaryQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper10Query;
        private JobOper FindFirstJobOper10(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper10Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper10Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper10Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper11Query;
        private JobOper FindFirstJobOper11(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper11Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper11Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper11Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper12Query;
        private JobOper FindFirstJobOper12(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper12Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper12Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper12Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper13Query;
        private JobOper FindFirstJobOper13(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper13Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper13Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper13Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper14Query;
        private JobOper FindFirstJobOper14(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper14Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper14Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper14Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper15Query;
        private JobOper FindFirstJobOper15(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper15Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper15Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper15Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper16Query;
        private JobOper FindFirstJobOper16(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper16Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper16Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper16Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper17Query;
        private JobOper FindFirstJobOper17(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper17Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper17Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper17Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper18Query;
        private JobOper FindFirstJobOper18(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper18Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper18Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper18Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper19Query;
        private JobOper FindFirstJobOper19(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper19Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper19Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper19Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper2Query;
        private JobOper FindFirstJobOper2(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper2Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper20Query;
        private JobOper FindFirstJobOper20(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper20Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper20Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper20Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper21Query;
        private JobOper FindFirstJobOper21(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper21Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper21Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper21Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper22Query;
        private JobOper FindFirstJobOper22(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper22Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper22Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper22Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper23Query;
        private JobOper FindFirstJobOper23(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper23Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper23Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper23Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper24Query;
        private JobOper FindFirstJobOper24(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper24Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper24Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper24Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper25Query;
        private JobOper FindFirstJobOper25(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper25Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper25Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper25Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper26Query;
        private JobOper FindFirstJobOper26(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper26Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper26Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper26Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper27Query;
        private JobOper FindFirstJobOper27(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper27Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper27Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper27Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper28Query;
        private JobOper FindFirstJobOper28(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper28Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper28Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper28Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper29Query;
        private JobOper FindFirstJobOper29(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper29Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper29Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper29Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper3Query;
        private JobOper FindFirstJobOper3(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper3Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper3Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper30Query;
        private JobOper FindFirstJobOper30(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper30Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper30Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper30Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper31Query;
        private JobOper FindFirstJobOper31(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper31Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper31Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper31Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper32Query;
        private JobOper FindFirstJobOper32(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper32Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper32Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper32Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper33Query;
        private JobOper FindFirstJobOper33(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper33Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper33Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper33Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper34Query;
        private JobOper FindFirstJobOper34(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper34Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper34Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper34Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper4Query;
        private JobOper FindFirstJobOper4(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper4Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper4Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper5Query;
        private JobOper FindFirstJobOper5(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper5Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper5Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper6Query;
        private JobOper FindFirstJobOper6(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper6Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper6Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper6Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper8Query;
        private JobOper FindFirstJobOper8(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper8Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper8Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper8Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper9Query;
        private JobOper FindFirstJobOper9(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper9Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper9Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper9Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOperWithUpdLockQuery;
        private JobOper FindFirstJobOperWithUpdLock(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOperWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOperWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOperWithUpdLockQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }
        #endregion JobOper Queries

        #region JobOperAction Queries
        static Func<ErpContext, string, string, int, int, IEnumerable<JobOperAction>> selectJobOperActionQuery;
        private IEnumerable<JobOperAction> SelectJobOperAction(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (selectJobOperActionQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, IEnumerable<JobOperAction>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOperAction
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row);
                selectJobOperActionQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectJobOperActionQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }

        static Func<ErpContext, string, string, int, int, int, JobOperAction> findFirstJobOperActionQuery;
        private JobOperAction FindFirstJobOperAction(string company, string jobNum, int assemblySeq, int oprSeq, int actionSeq)
        {
            if (findFirstJobOperActionQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, JobOperAction>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, actionSeq_ex) =>
        (from row in ctx.JobOperAction
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.ActionSeq == actionSeq_ex
         select row).FirstOrDefault();
                findFirstJobOperActionQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOperActionQuery(this.Db, company, jobNum, assemblySeq, oprSeq, actionSeq);
        }

        static Func<ErpContext, string, string, int, int, bool> existRequiredOpenJobOperActionQuery;
        private bool ExistRequiredOpenJobOperAction(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (existRequiredOpenJobOperActionQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOperAction//.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.Completed == false &&
         row.Required == true
         select row).Any();
                existRequiredOpenJobOperActionQuery = DBExpressionCompiler.Compile(expression);
            }

            return existRequiredOpenJobOperActionQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }
        #endregion JobOperAction Queries

        #region JobOperInsp Queries

        static Func<ErpContext, string, string, int, int, bool> existsJobOperInspQuery;
        private bool ExistsJobOperInsp(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (existsJobOperInspQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOperInsp
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).Any();
                existsJobOperInspQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobOperInspQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, bool> existsJobOperInsp2Query;
        private bool ExistsJobOperInsp2(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (existsJobOperInsp2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOperInsp
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).Any();
                existsJobOperInsp2Query = DBExpressionCompiler.Compile(expression);
            }

            return existsJobOperInsp2Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }
        #endregion JobOperInsp Queries

        #region JobPart Queries

        static Func<ErpContext, string, string, int, bool> existsJobPartQuery;
        private bool ExistsJobPart(string company, string jobNum, int partsPerOp)
        {
            if (existsJobPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, partsPerOp_ex) =>
        (from row in ctx.JobPart
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.PartsPerOp > partsPerOp_ex
         select row).Any();
                existsJobPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobPartQuery(this.Db, company, jobNum, partsPerOp);
        }


        static Func<ErpContext, string, string, int> countJobPartQuery;
        private int CountJobPart(string company, string jobNum)
        {
            if (countJobPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, int>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobPart
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).Take(2).Count();
                countJobPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return countJobPartQuery(this.Db, company, jobNum);
        }
        //This query has been optimized as much as possible and cannot be replaced with an Any, it is
        //querying based on a non-unique combination and returning true/false if there is only one row 
        //that matches the combination company/jobnum
        private bool ExistsUniqueJobPart(string company, string jobNum)
        {
            return CountJobPart(company, jobNum) == 1;
        }


        static Func<ErpContext, string, string, bool> existsFirstJobPartQuery;
        private bool ExistsFirstJobPart(string company, string jobNum)
        {
            if (existsFirstJobPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobPart
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).Any();
                existsFirstJobPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsFirstJobPartQuery(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, string, JobPart> findFirstJobPartQuery;
        private JobPart FindFirstJobPart(string company, string jobNum, string partNum)
        {
            if (findFirstJobPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, JobPart>> expression =
      (ctx, company_ex, jobNum_ex, partNum_ex) =>
        (from row in ctx.JobPart
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.PartNum == partNum_ex
         select row).FirstOrDefault();
                findFirstJobPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobPartQuery(this.Db, company, jobNum, partNum);
        }


        static Func<ErpContext, string, string, string, string> findFirstJobPartIUMQuery;
        private string FindFirstJobPartIUM(string company, string jobNum, string partNum)
        {
            if (findFirstJobPartIUMQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string>> expression =
      (ctx, company_ex, jobNum_ex, partNum_ex) =>
        (from row in ctx.JobPart
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.PartNum == partNum_ex
         select row.IUM).FirstOrDefault();
                findFirstJobPartIUMQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobPartIUMQuery(this.Db, company, jobNum, partNum);
        }


        static Func<ErpContext, string, string, string> findFirstPartIUMQuery;
        private string FindFirstPartIUM(string company, string partNum)
        {
            if (findFirstPartIUMQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
      (ctx, company_ex, partNum_ex) =>
        (from row in ctx.Part
         where row.Company == company_ex &&
         row.PartNum == partNum_ex
         select row.IUM).FirstOrDefault();
                findFirstPartIUMQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartIUMQuery(this.Db, company, partNum);
        }



        static Func<ErpContext, string, string, string, JobPart> findFirstJobPart3Query;
        private JobPart FindFirstJobPart3(string company, string jobNum, string partNum)
        {
            if (findFirstJobPart3Query == null)
            {
                Expression<Func<ErpContext, string, string, string, JobPart>> expression =
      (ctx, company_ex, jobNum_ex, partNum_ex) =>
        (from row in ctx.JobPart
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.PartNum == partNum_ex
         select row).FirstOrDefault();
                findFirstJobPart3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobPart3Query(this.Db, company, jobNum, partNum);
        }


        static Func<ErpContext, string, string, string, JobPart> findFirstJobPartWithUpdLockQuery;
        private JobPart FindFirstJobPartWithUpdLock(string company, string jobNum, string partNum)
        {
            if (findFirstJobPartWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, JobPart>> expression =
      (ctx, company_ex, jobNum_ex, partNum_ex) =>
        (from row in ctx.JobPart.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.PartNum == partNum_ex
         select row).FirstOrDefault();
                findFirstJobPartWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobPartWithUpdLockQuery(this.Db, company, jobNum, partNum);
        }


        static Func<ErpContext, string, string, IEnumerable<JobPart>> selectJobPartQuery;
        private IEnumerable<JobPart> SelectJobPart(string company, string jobNum)
        {
            if (selectJobPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<JobPart>>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobPart
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row);
                selectJobPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectJobPartQuery(this.Db, company, jobNum);
        }

        static Func<ErpContext, string, string, string, IEnumerable<JobPart>> selectJobPartQuery_1;
        private IEnumerable<JobPart> SelectJobPart(string company, string plant, string jobNum)
        {
            if (selectJobPartQuery_1 == null)
            {
                Expression<Func<ErpContext, string, string, string, IEnumerable<JobPart>>> expression =
      (ctx, company_ex, plant_ex, jobNum_ex) =>
        (from row in ctx.JobPart
         where row.Company == company_ex &&
               row.Plant == plant_ex &&
               row.JobNum == jobNum_ex
         select row);
                selectJobPartQuery_1 = DBExpressionCompiler.Compile(expression);
            }

            return selectJobPartQuery_1(this.Db, company, plant, jobNum);
        }

        #endregion JobPart Queries

        #region LabExpCd Queries

        static Func<ErpContext, string, string, bool> existsLabExpCdQuery;
        private bool ExistsLabExpCd(string company, string expenseCode)
        {
            if (existsLabExpCdQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, expenseCode_ex) =>
        (from row in ctx.LabExpCd
         where row.Company == company_ex &&
         row.ExpenseCode == expenseCode_ex
         select row).Any();
                existsLabExpCdQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsLabExpCdQuery(this.Db, company, expenseCode);
        }


        static Func<ErpContext, string, string, LabExpCd> findFirstLabExpCdQuery;
        private LabExpCd FindFirstLabExpCd(string company, string expenseCode)
        {
            if (findFirstLabExpCdQuery == null)
            {
                Expression<Func<ErpContext, string, string, LabExpCd>> expression =
      (ctx, company_ex, expenseCode_ex) =>
        (from row in ctx.LabExpCd
         where row.Company == company_ex &&
         row.ExpenseCode == expenseCode_ex
         select row).FirstOrDefault();
                findFirstLabExpCdQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLabExpCdQuery(this.Db, company, expenseCode);
        }


        static Func<ErpContext, string, string, LabExpCd> findFirstLabExpCd2Query;
        private LabExpCd FindFirstLabExpCd2(string company, string expenseCode)
        {
            if (findFirstLabExpCd2Query == null)
            {
                Expression<Func<ErpContext, string, string, LabExpCd>> expression =
      (ctx, company_ex, expenseCode_ex) =>
        (from row in ctx.LabExpCd
         where row.Company == company_ex &&
         row.ExpenseCode == expenseCode_ex
         select row).FirstOrDefault();
                findFirstLabExpCd2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLabExpCd2Query(this.Db, company, expenseCode);
        }
        #endregion LabExpCd Queries

        #region LaborDtl Queries

        static Func<ErpContext, string, int, string, string, int, int, string, int, bool, bool> existsLaborDtlQuery;
        private bool ExistsLaborDtl1(string company, int laborHedSeq, string employeeNum, string jobNum, int assemblySeq, int oprSeq, string resourceID, int laborDtlSeq, bool activeTrans)
        {
            if (existsLaborDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, int, string, int, bool, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, employeeNum_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, resourceID_ex, laborDtlSeq_ex, activeTrans_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.ResourceID == resourceID_ex &&
         row.LaborDtlSeq != laborDtlSeq_ex &&
         row.ActiveTrans == activeTrans_ex
         select row).Any();
                existsLaborDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborDtlQuery(this.Db, company, laborHedSeq, employeeNum, jobNum, assemblySeq, oprSeq, resourceID, laborDtlSeq, activeTrans);
        }


        static Func<ErpContext, string, LaborDtlParams, bool> existsLaborDtlQuery_22;
        private bool ExistsLaborDtl2(string company, LaborDtlParams ipLaborDtlParams)
        {
            if (existsLaborDtlQuery_22 == null)
            {
                Expression<Func<ErpContext, string, LaborDtlParams, bool>> expression =
      (ctx, company_ex, LaborDtlParams_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.PayrollDate.Value == LaborDtlParams_ex.PayrollDate.Value &&
         row.JobNum == LaborDtlParams_ex.JobNum &&
         row.AssemblySeq == LaborDtlParams_ex.AssemblySeq &&
         row.OprSeq == LaborDtlParams_ex.OprSeq &&
         row.EmployeeNum == LaborDtlParams_ex.EmployeeNum &&
         row.LaborType == LaborDtlParams_ex.LaborType &&
         row.ProjectID == LaborDtlParams_ex.ProjectID &&
         row.PhaseID == LaborDtlParams_ex.PhaseID &&
         row.IndirectCode == LaborDtlParams_ex.IndirectCode &&
         row.RoleCd == LaborDtlParams_ex.RoleCd &&
         row.TimeTypCd == LaborDtlParams_ex.TimeTypCd &&
         row.ResourceGrpID == LaborDtlParams_ex.ResourceGrpID &&
         row.ResourceID == LaborDtlParams_ex.ResourceID &&
         row.ExpenseCode == LaborDtlParams_ex.ExpenseCode &&
         row.Shift == LaborDtlParams_ex.Shift &&
         row.LaborDtlSeq != LaborDtlParams_ex.LaborDtlSeq
         select row).Any();
                existsLaborDtlQuery_22 = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborDtlQuery_22(this.Db, company, ipLaborDtlParams);
        }


        static Func<ErpContext, string, string, string, int, int, bool, bool> existsLaborDtlQuery_3;
        private bool ExistsLaborDtl3(string company, string employeeNum, string jobNum, int assemblySeq, int oprSeq, bool activeTrans)
        {
            if (existsLaborDtlQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, string, int, int, bool, bool>> expression =
      (ctx, company_ex, employeeNum_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, activeTrans_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.ActiveTrans == activeTrans_ex
         select row).Any();
                existsLaborDtlQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborDtlQuery_3(this.Db, company, employeeNum, jobNum, assemblySeq, oprSeq, activeTrans);
        }


        static Func<ErpContext, string, string, int, int, bool, int, bool> existsLaborDtlQuery_4;
        private bool ExistsLaborDtl4(string company, string jobNum, int assemblySeq, int oprSeq, bool opComplete, int laborDtlSeq)
        {
            if (existsLaborDtlQuery_4 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, opComplete_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.OpComplete == opComplete_ex &&
         row.LaborDtlSeq != laborDtlSeq_ex
         select row).Any();
                existsLaborDtlQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborDtlQuery_4(this.Db, company, jobNum, assemblySeq, oprSeq, opComplete, laborDtlSeq);
        }



        static Func<ErpContext, string, string, bool, string, bool> existsLaborDtlQuery_5;
        private bool ExistsLaborDtl5(string company, string employeeNum, bool activeTrans, string laborType)
        {
            if (existsLaborDtlQuery_5 == null)
            {
                Expression<Func<ErpContext, string, string, bool, string, bool>> expression =
      (ctx, company_ex, employeeNum_ex, activeTrans_ex, laborType_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.ActiveTrans == activeTrans_ex &&
         row.LaborType == laborType_ex
         select row).Any();
                existsLaborDtlQuery_5 = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborDtlQuery_5(this.Db, company, employeeNum, activeTrans, laborType);
        }


        static Func<ErpContext, string, int, bool, bool> existsLaborDtlQuery_6;
        private bool ExistsLaborDtl6(string company, int laborHedSeq, bool wipPosted)
        {
            if (existsLaborDtlQuery_6 == null)
            {
                Expression<Func<ErpContext, string, int, bool, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, wipPosted_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.WipPosted == wipPosted_ex
         select row).Any();
                existsLaborDtlQuery_6 = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborDtlQuery_6(this.Db, company, laborHedSeq, wipPosted);
        }

        static Func<ErpContext, string, int, bool> existsLaborDtlQuery_8;
        private bool ExistsLaborDtl8(string company, int laborHedSeq)
        {
            if (existsLaborDtlQuery_8 == null)
            {
                Expression<Func<ErpContext, string, int, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).Any();
                existsLaborDtlQuery_8 = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborDtlQuery_8(this.Db, company, laborHedSeq);
        }

        static Func<ErpContext, string, int, Guid, bool> existsOtherLaborDtlQuery;
        private bool ExistsOtherLaborDtl(string company, int laborHedSeq, Guid sysRowID)
        {
            if (existsOtherLaborDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, Guid, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, sysRowID_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.SysRowID != sysRowID_ex
         select row).Any();
                existsOtherLaborDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsOtherLaborDtlQuery(this.Db, company, laborHedSeq, sysRowID);
        }

        static Func<ErpContext, string, int, int, decimal> findFirstLaborDtlPrevQtyQuery;
        private decimal FindFirstLaborDtlPrevQty(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtlPrevQtyQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, decimal>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row.LaborQty).FirstOrDefault();
                findFirstLaborDtlPrevQtyQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlPrevQtyQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }



        static Func<ErpContext, string, int, int, LaborDtl> findFirstLaborDtlQuery;
        private LaborDtl FindFirstLaborDtl(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, LaborDtl>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstLaborDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }


        static Func<ErpContext, string, string, int, string, int, int, string, bool, LaborDtl> findFirstLaborDtlQuery_2;
        private LaborDtl FindFirstLaborDtl(string company, string employeeNum, int laborHedSeq, string jobNum, int assemblySeq, int oprSeq, string resourceID, bool activeTrans)
        {
            if (findFirstLaborDtlQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, int, int, string, bool, LaborDtl>> expression =
      (ctx, company_ex, employeeNum_ex, laborHedSeq_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, resourceID_ex, activeTrans_ex) =>
        (from row in ctx.LaborDtl
         where (row.Company == company_ex) &&
        (row.EmployeeNum == employeeNum_ex) &&
        (row.LaborHedSeq == laborHedSeq_ex) &&
        (row.JobNum == jobNum_ex) &&
        (row.AssemblySeq == assemblySeq_ex) &&
        (row.OprSeq == oprSeq_ex) &&
        (row.ResourceID == resourceID_ex) &&
        (row.ActiveTrans == activeTrans_ex)
         select row).FirstOrDefault();
                findFirstLaborDtlQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlQuery_2(this.Db, company, employeeNum, laborHedSeq, jobNum, assemblySeq, oprSeq, resourceID, activeTrans);
        }


        static Func<ErpContext, Guid, LaborDtl> findFirstLaborDtlQuery_3;
        private LaborDtl FindFirstLaborDtl(Guid sysRowID)
        {
            if (findFirstLaborDtlQuery_3 == null)
            {
                Expression<Func<ErpContext, Guid, LaborDtl>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.LaborDtl
         where row.SysRowID == sysRowID_ex
         select row).FirstOrDefault();
                findFirstLaborDtlQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlQuery_3(this.Db, sysRowID);
        }


        static Func<ErpContext, string, string, int, LaborDtl> findFirstLaborDtlQuery_4;
        private LaborDtl FindFirstLaborDtl(string company, string company2, int laborHedSeq)
        {
            if (findFirstLaborDtlQuery_4 == null)
            {
                Expression<Func<ErpContext, string, string, int, LaborDtl>> expression =
      (ctx, company_ex, company2_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.Company == company2_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborDtlQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlQuery_4(this.Db, company, company2, laborHedSeq);
        }

        static Func<ErpContext, string, string, int, int, string, bool, LaborDtl> findFirstLaborDtlQuery_5;
        private LaborDtl FindFirstLaborDtl(string company, string jobNum, int assemblySeq, int oprSeq, string employeeNum, bool activeTrans)
        {
            if (findFirstLaborDtlQuery_5 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, bool, LaborDtl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, employeeNum_ex, activeTrans_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.ActiveTrans == activeTrans_ex
         select row).FirstOrDefault();
                findFirstLaborDtlQuery_5 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlQuery_5(this.Db, company, jobNum, assemblySeq, oprSeq, employeeNum, activeTrans);
        }


        static Func<ErpContext, string, int, int, LaborDtl> findFirstLaborDtl2Query;
        private LaborDtl FindFirstLaborDtl2(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtl2Query == null)
            {
                Expression<Func<ErpContext, string, int, int, LaborDtl>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstLaborDtl2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtl2Query(this.Db, company, laborHedSeq, laborDtlSeq);
        }


        static Func<ErpContext, Guid, LaborDtl> findFirstLaborDtl2Query_2;
        private LaborDtl FindFirstLaborDtl2(Guid sysRowID)
        {
            if (findFirstLaborDtl2Query_2 == null)
            {
                Expression<Func<ErpContext, Guid, LaborDtl>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.LaborDtl
         where row.SysRowID == sysRowID_ex
         select row).FirstOrDefault();
                findFirstLaborDtl2Query_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtl2Query_2(this.Db, sysRowID);
        }


        static Func<ErpContext, string, int, int, LaborDtl> findFirstLaborDtl3Query;
        private LaborDtl FindFirstLaborDtl3(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtl3Query == null)
            {
                Expression<Func<ErpContext, string, int, int, LaborDtl>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstLaborDtl3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtl3Query(this.Db, company, laborHedSeq, laborDtlSeq);
        }


        static Func<ErpContext, Guid, LaborDtl> findFirstLaborDtl3Query_2;
        private LaborDtl FindFirstLaborDtl3(Guid sysRowID)
        {
            if (findFirstLaborDtl3Query_2 == null)
            {
                Expression<Func<ErpContext, Guid, LaborDtl>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.LaborDtl
         where row.SysRowID == sysRowID_ex
         select row).FirstOrDefault();
                findFirstLaborDtl3Query_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtl3Query_2(this.Db, sysRowID);
        }


        static Func<ErpContext, string, int, int, LaborDtl> findFirstLaborDtl4Query;
        private LaborDtl FindFirstLaborDtl4(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtl4Query == null)
            {
                Expression<Func<ErpContext, string, int, int, LaborDtl>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstLaborDtl4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtl4Query(this.Db, company, laborHedSeq, laborDtlSeq);
        }


        static Func<ErpContext, Guid, LaborDtl> findFirstLaborDtl4Query_2;
        private LaborDtl FindFirstLaborDtl4(Guid sysRowID)
        {
            if (findFirstLaborDtl4Query_2 == null)
            {
                Expression<Func<ErpContext, Guid, LaborDtl>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.LaborDtl
         where row.SysRowID == sysRowID_ex
         select row).FirstOrDefault();
                findFirstLaborDtl4Query_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtl4Query_2(this.Db, sysRowID);
        }


        static Func<ErpContext, string, int, int, LaborDtl> findFirstLaborDtl5Query;
        private LaborDtl FindFirstLaborDtl5(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtl5Query == null)
            {
                Expression<Func<ErpContext, string, int, int, LaborDtl>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstLaborDtl5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtl5Query(this.Db, company, laborHedSeq, laborDtlSeq);
        }


        static Func<ErpContext, string, int, int, LaborDtl> findFirstLaborDtl6Query;
        private LaborDtl FindFirstLaborDtl6(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtl6Query == null)
            {
                Expression<Func<ErpContext, string, int, int, LaborDtl>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstLaborDtl6Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtl6Query(this.Db, company, laborHedSeq, laborDtlSeq);
        }


        static Func<ErpContext, string, int, int, LaborDtl> findFirstLaborDtl7Query;
        private LaborDtl FindFirstLaborDtl7(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtl7Query == null)
            {
                Expression<Func<ErpContext, string, int, int, LaborDtl>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstLaborDtl7Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtl7Query(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        static Func<ErpContext, string, int, int, decimal> findFirstLaborDtlLaborQtyQuery;
        private decimal FindFirstLaborDtlLaborQty(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtlLaborQtyQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, decimal>> expression =
              (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
                (from row in ctx.LaborDtl
                 where row.Company == company_ex &&
                 row.LaborHedSeq == laborHedSeq_ex &&
                 row.LaborDtlSeq == laborDtlSeq_ex
                 select row.LaborQty).FirstOrDefault();
                findFirstLaborDtlLaborQtyQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlLaborQtyQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        static Func<ErpContext, string, int, int, decimal> findFirstLaborDtlDiscrepQtyQuery;
        private decimal FindFirstLaborDtlDiscrepQtyQty(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtlDiscrepQtyQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, decimal>> expression =
              (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
                (from row in ctx.LaborDtl
                 where row.Company == company_ex &&
                 row.LaborHedSeq == laborHedSeq_ex &&
                 row.LaborDtlSeq == laborDtlSeq_ex
                 select row.DiscrepQty).FirstOrDefault();
                findFirstLaborDtlDiscrepQtyQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlDiscrepQtyQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        static Func<ErpContext, string, int, int, string, int, int, decimal> getOldLaborDtlQtyQuery;
        private decimal GetOldLaborDtlQty(string company, int laborHedSeq, int laborDtlSeq, string jobNum, int assemblySeq, int oprSeq)
        {
            if (getOldLaborDtlQtyQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, string, int, int, decimal>> expression =
              (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
                (from row in ctx.LaborDtl
                 where row.Company == company_ex &&
                 row.LaborHedSeq == laborHedSeq_ex &&
                 row.LaborDtlSeq == laborDtlSeq_ex &&
                 row.JobNum == jobNum_ex &&
                 row.AssemblySeq == assemblySeq_ex &&
                 row.OprSeq == oprSeq_ex
                 select row.LaborQty).FirstOrDefault();
                getOldLaborDtlQtyQuery = DBExpressionCompiler.Compile(expression);
            }

            return getOldLaborDtlQtyQuery(this.Db, company, laborHedSeq, laborDtlSeq, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, int, int, LaborDtl> findFirstLaborDtlWithUpdLockQuery;
        private LaborDtl FindFirstLaborDtlWithUpdLock(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtlWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, LaborDtl>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstLaborDtlWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlWithUpdLockQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        private class LaborDtlPartialRow : Epicor.Data.TempRowBase
        {
            public string CreatedBy { get; set; }
            public Guid SysRowID { get; set; }
        }

        private static Func<ErpContext, string, int, int, LaborDtlPartialRow> findFirstLaborDtlPartialRowQuery;
        private LaborDtlPartialRow FindFirstLaborDtlPartialRow(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtlPartialRowQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, LaborDtlPartialRow>> expression =
                    (context, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
                    (from row in context.LaborDtl
                     where row.Company == company_ex &&
                     row.LaborHedSeq == laborHedSeq_ex &&
                     row.LaborDtlSeq == laborDtlSeq_ex
                     select new LaborDtlPartialRow { CreatedBy = row.CreatedBy, SysRowID = row.SysRowID })
                    .FirstOrDefault();
                findFirstLaborDtlPartialRowQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlPartialRowQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }


        static Func<ErpContext, string, string, IEnumerable<LaborDtl>> selectActiveLaborDtlQuery;
        private IEnumerable<LaborDtl> SelectActiveLaborDtl(string company, string employeeNum)
        {
            if (selectActiveLaborDtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.ActiveTrans
         orderby row.LaborDtlSeq
         select row);
                selectActiveLaborDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectActiveLaborDtlQuery(this.Db, company, employeeNum);
        }

        static Func<ErpContext, string, string, int, IEnumerable<LaborDtl>> selectActiveLaborDtlProdSetupQuery;
        private IEnumerable<LaborDtl> SelectActiveLaborDtlProdSetup(string company, string employeeNum, int laborHedSeq)
        {
            if (selectActiveLaborDtlProdSetupQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.ActiveTrans &&
         (row.LaborType == "P" || row.LaborType == "S")
         orderby row.LaborDtlSeq
         select row);
                selectActiveLaborDtlProdSetupQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectActiveLaborDtlProdSetupQuery(this.Db, company, employeeNum, laborHedSeq);
        }

        static Func<ErpContext, string, string, int, IEnumerable<LaborDtl>> selectActiveLaborDtlIndirectQuery;
        private IEnumerable<LaborDtl> SelectActiveLaborDtlIndirect(string company, string employeeNum, int laborHedSeq)
        {
            if (selectActiveLaborDtlIndirectQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.LaborType == "I" &&
         row.JobNum != "" &&
         row.IndirectCode != "" &&
         row.ClockOutTime == 0
         orderby row.LaborDtlSeq
         select row);
                selectActiveLaborDtlIndirectQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectActiveLaborDtlIndirectQuery(this.Db, company, employeeNum, laborHedSeq);
        }

        static Func<ErpContext, string, string, int, bool> existsLaborDtlDowntime;
        private bool ExistsLaborDtlDowntime(string company, string employeeNum, int laborHedSeq)
        {
            if (existsLaborDtlDowntime == null)
            {
                Expression<Func<ErpContext, string, string, int, bool>> expression =
                (ctx, company_ex, employeeNum_ex, laborHedSeq_ex) =>
                 (from row in ctx.LaborDtl
                  where row.Company == company_ex &&
                     row.LaborHedSeq == laborHedSeq_ex &&
                     row.EmployeeNum == employeeNum_ex &&
                     row.LaborType == "I" &&
                     row.JobNum != "" &&
                     row.ClockOutTime == 0
                  select row).Any();
                existsLaborDtlDowntime = DBExpressionCompiler.Compile(expression);
            }
            return existsLaborDtlDowntime(this.Db, company, employeeNum, laborHedSeq);
        }


        static Func<ErpContext, string, string, DateTime?, string, string, string, string, string, string, int, int, string, string, string, IEnumerable<LaborDtl>> selectLaborDtlQuery_11;
        private IEnumerable<LaborDtl> SelectLaborDtlWithUpdLock4(string company, string empNum, DateTime? payDate, string labType, string labTypePseudo, string projID, string phaseID,
            string timeTypCode, string jobNum, int asmNum, int oprSeq, string indCode, string roleCd, string resGrpID)
        {
            if (selectLaborDtlQuery_11 == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, string, string, string, string, string, string, int, int, string, string, string, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, payrollDate_ex, laborType_ex, laborTypePseudo_ex, projectID_ex, phaseID_ex, timeTypCd_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, indirectCode_ex, roleCd_ex, resourceGrpID_ex) =>
        (from row in ctx.LaborDtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value &&
         row.LaborType == laborType_ex &&
         row.LaborTypePseudo == laborTypePseudo_ex &&
         row.ProjectID == projectID_ex &&
         row.PhaseID == phaseID_ex &&
         row.TimeTypCd == timeTypCd_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.IndirectCode == indirectCode_ex &&
         row.RoleCd == roleCd_ex &&
         row.ResourceGrpID == resourceGrpID_ex
         select row);
                selectLaborDtlQuery_11 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlQuery_11(this.Db, company, empNum, payDate, labType, labTypePseudo, projID, phaseID, timeTypCode, jobNum, asmNum, oprSeq, indCode, roleCd, resGrpID);
        }

        //HOLDLOCK


        static Func<ErpContext, string, int, int, LaborDtl> findFirstLaborDtlWithUpdLock2Query;
        private LaborDtl FindFirstLaborDtlWithUpdLock2(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtlWithUpdLock2Query == null)
            {
                Expression<Func<ErpContext, string, int, int, LaborDtl>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstLaborDtlWithUpdLock2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlWithUpdLock2Query(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        static Func<ErpContext, string, int, string, LaborDtl> findFirstParentLaborDtlWithUpdLockQuery;
        private LaborDtl FindFirstParentLaborDtlWithUpdLock(string company, int laborHedSeq, string jobNum)
        {
            if (findFirstParentLaborDtlWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, LaborDtl>> expression =
      (ctx, company_ex, laborHedSeq_ex, jobNum_ex) =>
        (from row in ctx.LaborDtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.JobNum == jobNum_ex &&
         row.Downtime == true &&
         row.ActiveTrans == true
         select row).FirstOrDefault();
                findFirstParentLaborDtlWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstParentLaborDtlWithUpdLockQuery(this.Db, company, laborHedSeq, jobNum);
        }

        static Func<ErpContext, string, string, int, int, string, int, int, IEnumerable<LaborDtl>> selectLaborDtlQuery;
        private IEnumerable<LaborDtl> SelectLaborDtl(string company, string jobNum, int assemblySeq, int oprSeq, string resourceID, int clockOutMinute, int clockInMInute)
        {
            if (selectLaborDtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, int, int, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, resourceID_ex, clockOutMinute_ex, clockInMInute_ex) =>
        (from row in ctx.LaborDtl
         where (row.Company == company_ex) &&
        (row.JobNum == jobNum_ex) &&
        (row.AssemblySeq == assemblySeq_ex) &&
        (row.OprSeq == oprSeq_ex) &&
        (row.ResourceID == resourceID_ex) &&
        (row.ClockOutMinute >= clockOutMinute_ex) &&
        (row.ClockInMInute <= clockInMInute_ex)
         orderby row.Company, row.JobNum, row.AssemblySeq, row.OprSeq, row.ResourceID, row.ClockOutMinute, row.ClockInMInute
         select row);
                selectLaborDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlQuery(this.Db, company, jobNum, assemblySeq, oprSeq, resourceID, clockOutMinute, clockInMInute);
        }


        private class LaborDtlLaborHrsResult
        {
            public string Company { get; set; }
            public bool WipPosted { get; set; }
            public decimal LaborHrs { get; set; }
            public Guid SysRowID { get; set; }
        }

        static Func<ErpContext, string, int, IEnumerable<LaborDtlLaborHrsResult>> selectLaborDtlQuery_2;
        private IEnumerable<LaborDtlLaborHrsResult> SelectLaborDtl(string company, int laborHedSeq)
        {
            if (selectLaborDtlQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<LaborDtlLaborHrsResult>>> expression =
                (ctx, company_ex, laborHedSeq_ex) =>
                (from row in ctx.LaborDtl
                 where (row.Company == company_ex) &&
                (row.LaborHedSeq == laborHedSeq_ex)
                 orderby row.Company, row.LaborHedSeq, row.LaborDtlSeq
                 select new LaborDtlLaborHrsResult
                 {
                     Company = row.Company,
                     WipPosted = row.WipPosted,
                     LaborHrs = row.LaborHrs,
                     SysRowID = row.SysRowID

                 });
                selectLaborDtlQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlQuery_2(this.Db, company, laborHedSeq);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, int, int, int, bool, IEnumerable<LaborDtl>> selectLaborDtlQuery_3;
        private IEnumerable<LaborDtl> SelectLaborDtl(string company, string jobNum, int assemblySeq, int oprSeq, int laborDtlSeq, bool activeTrans)
        {
            if (selectLaborDtlQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, bool, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, laborDtlSeq_ex, activeTrans_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.LaborDtlSeq != laborDtlSeq_ex &&
         row.ActiveTrans == activeTrans_ex
         select row);
                selectLaborDtlQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlQuery_3(this.Db, company, jobNum, assemblySeq, oprSeq, laborDtlSeq, activeTrans);
        }


        class LaborDtlSummary
        {
            public string Company { get; set; }
            public int LaborHedSeq { get; set; }
            public bool ActiveTrans { get; set; }
            public decimal TotalLabHrs { get; set; }
            public decimal TotalBurHrs { get; set; }
            public decimal TotalHCMPayHrs { get; set; }

        }
        static Func<ErpContext, string, int, bool, IEnumerable<LaborDtlSummary>> sumLaborHrsByHedQuery;
        private IEnumerable<LaborDtlSummary> SumLaborHrsByHed(string company, int laborHedSeq, bool activeTrans)
        {
            if (sumLaborHrsByHedQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool, IEnumerable<LaborDtlSummary>>> expression =
      (ctx, company_ex, laborHedSeq_ex, activeTrans_ex) =>
        ctx.LaborDtl.Where(at => at.Company == company_ex
                                           && at.ActiveTrans == activeTrans_ex
                                           && at.LaborHedSeq == laborHedSeq_ex).OrderBy(ord => ord.Company).ThenBy(ord => ord.LaborHedSeq).ThenBy(ord => ord.ClockInMInute)
                                           .GroupBy(grp => new { grp.Company, grp.ActiveTrans, grp.LaborHedSeq })
                                           .Select(row => new LaborDtlSummary
                                           {
                                               Company = row.Key.Company,
                                               LaborHedSeq = row.Key.LaborHedSeq,
                                               ActiveTrans = row.Key.ActiveTrans,
                                               TotalBurHrs = row.Sum(x => x.BurdenHrs),
                                               TotalLabHrs = row.Sum(x => x.LaborHrs),
                                               TotalHCMPayHrs = row.Sum(x => x.HCMPayHours)
                                           }
                                           ).DefaultIfEmpty(new LaborDtlSummary
                                           {
                                               Company = company_ex,
                                               LaborHedSeq = laborHedSeq_ex,
                                               ActiveTrans = activeTrans_ex,
                                               TotalBurHrs = decimal.Zero,
                                               TotalLabHrs = decimal.Zero,
                                               TotalHCMPayHrs = decimal.Zero
                                           });

                sumLaborHrsByHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return sumLaborHrsByHedQuery(this.Db, company, laborHedSeq, activeTrans);
        }


        static Func<ErpContext, string, LaborDtlParams, IEnumerable<LaborDtl>> selectLaborDtlQuery_5;
        private IEnumerable<LaborDtl> SelectLaborDtl(string company, LaborDtlParams ipLaborParams)
        {
            if (selectLaborDtlQuery_5 == null)
            {
                Expression<Func<ErpContext, string, LaborDtlParams, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, LaborDtlParams_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.PayrollDate.Value == LaborDtlParams_ex.PayrollDate.Value &&
         row.JobNum == LaborDtlParams_ex.JobNum &&
         row.AssemblySeq == LaborDtlParams_ex.AssemblySeq &&
         row.OprSeq == LaborDtlParams_ex.OprSeq &&
         row.EmployeeNum == LaborDtlParams_ex.EmployeeNum &&
         row.LaborType == LaborDtlParams_ex.LaborType &&
         row.ProjectID == LaborDtlParams_ex.ProjectID &&
         row.PhaseID == LaborDtlParams_ex.PhaseID &&
         row.IndirectCode == LaborDtlParams_ex.IndirectCode &&
         row.RoleCd == LaborDtlParams_ex.RoleCd &&
         row.TimeTypCd == LaborDtlParams_ex.TimeTypCd &&
         row.ResourceGrpID == LaborDtlParams_ex.ResourceGrpID &&
         row.ResourceID == LaborDtlParams_ex.ResourceID &&
         row.ExpenseCode == LaborDtlParams_ex.ExpenseCode &&
         row.Shift == LaborDtlParams_ex.Shift &&
         row.LaborDtlSeq != LaborDtlParams_ex.LaborDtlSeq
         select row);
                selectLaborDtlQuery_5 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlQuery_5(this.Db, company, ipLaborParams);
        }


        static Func<ErpContext, string, string, DateTime?, string, decimal, IEnumerable<LaborDtl>> selectLaborDtlExceptEntryMeth;
        private IEnumerable<LaborDtl> SelectLaborDtlExceptEntryMeth(string company, string employeeNum, DateTime? payrollDate, string laborEntryMethod, decimal minLaborHrs)
        {
            if (selectLaborDtlExceptEntryMeth == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, string, decimal, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, payrollDate_ex, laborEntryMethod_ex, minLaborHrs_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value &&
         row.LaborEntryMethod != laborEntryMethod_ex &&
         row.LaborHrs != minLaborHrs_ex
         select row);
                selectLaborDtlExceptEntryMeth = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlExceptEntryMeth(this.Db, company, employeeNum, payrollDate, laborEntryMethod, minLaborHrs);
        }

        static Func<ErpContext, string, string, DateTime?, string, decimal, IEnumerable<LaborDtl>> selectLaborDtlLaborEntryQuery;
        private IEnumerable<LaborDtl> SelectLaborDtlEntryMeth(string company, string employeeNum, DateTime? payrollDate, string laborEntryMethod, decimal minLaborHrs)
        {
            if (selectLaborDtlLaborEntryQuery == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, string, decimal, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, payrollDate_ex, laborEntryMethod_ex, minLaborHrs_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value &&
         row.LaborEntryMethod == laborEntryMethod_ex &&
         row.LaborHrs != minLaborHrs_ex
         select row);
                selectLaborDtlLaborEntryQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlLaborEntryQuery(this.Db, company, employeeNum, payrollDate, laborEntryMethod, minLaborHrs);
        }


        static Func<ErpContext, string, string, DateTime?, string, IEnumerable<LaborDtl>> selectLaborDtlLaborEntryQuery2;
        private IEnumerable<LaborDtl> SelectLaborDtlEntryMeth(string company, string employeeNum, DateTime? payrollDate, string laborEntryMethod)
        {
            if (selectLaborDtlLaborEntryQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, string, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, payrollDate_ex, laborEntryMethod_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value &&
         row.LaborEntryMethod == laborEntryMethod_ex
         select row);
                selectLaborDtlLaborEntryQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlLaborEntryQuery2(this.Db, company, employeeNum, payrollDate, laborEntryMethod);
        }


        static Func<ErpContext, string, string, DateTime?, DateTime?, bool, decimal, IEnumerable<LaborDtl>> selectLaborDtlPeriodQuery;
        private IEnumerable<LaborDtl> SelectLaborDtlPeriod(string company, string employeeNum, DateTime? prlStartDate, DateTime? prlEndDate, bool activeTrans, decimal minLaborHrs)
        {
            if (selectLaborDtlPeriodQuery == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, DateTime?, bool, decimal, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, prlStartDate_ex, prlEndDate_ex, activeTrans_ex, minLaborHrs_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value >= prlStartDate_ex.Value &&
         row.PayrollDate.Value <= prlEndDate_ex.Value &&
         row.ActiveTrans == activeTrans_ex &&
         row.LaborHrs != minLaborHrs_ex
         select row);
                selectLaborDtlPeriodQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlPeriodQuery(this.Db, company, employeeNum, prlStartDate, prlEndDate, activeTrans, minLaborHrs);
        }


        static Func<ErpContext, string, DateTime?, string, IEnumerable<LaborDtl>> selectLaborDtlQuery_7;
        private IEnumerable<LaborDtl> SelectLaborDtl(string company, DateTime? payrollDate, string employeeNum)
        {
            if (selectLaborDtlQuery_7 == null)
            {
                Expression<Func<ErpContext, string, DateTime?, string, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, payrollDate_ex, employeeNum_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value &&
         row.EmployeeNum == employeeNum_ex
         select row);
                selectLaborDtlQuery_7 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlQuery_7(this.Db, company, payrollDate, employeeNum);
        }


        static Func<ErpContext, string, string, DateTime?, string, string, string, string, string, string, int, int, string, string, string, IEnumerable<LaborDtl>> selectLaborDtlQuery_8;
        private IEnumerable<LaborDtl> SelectLaborDtlWithUpdLock(string company, string empNum, DateTime? payDate, string labType, string labTypePseudo, string projID, string phaseID,
            string timeTypCode, string jobNum, int asmNum, int oprSeq, string indCode, string roleCd, string resGrpID)
        {
            if (selectLaborDtlQuery_8 == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, string, string, string, string, string, string, int, int, string, string, string, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, payrollDate_ex, laborType_ex, laborTypePseudo_ex, projectID_ex, phaseID_ex, timeTypCd_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, indirectCode_ex, roleCd_ex, resourceGrpID_ex) =>
        (from row in ctx.LaborDtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value &&
         row.LaborType == laborType_ex &&
         row.LaborTypePseudo == laborTypePseudo_ex &&
         row.ProjectID == projectID_ex &&
         row.PhaseID == phaseID_ex &&
         row.TimeTypCd == timeTypCd_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.IndirectCode == indirectCode_ex &&
         row.RoleCd == roleCd_ex &&
         row.ResourceGrpID == resourceGrpID_ex
         select row);
                selectLaborDtlQuery_8 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlQuery_8(this.Db, company, empNum, payDate, labType, labTypePseudo, projID, phaseID, timeTypCode, jobNum, asmNum, oprSeq, indCode, roleCd, resGrpID);
        }


        //HOLDLOCK



        static Func<ErpContext, string, string, DateTime?, decimal, string, IEnumerable<LaborDtl>> selectLaborDtlQuery_9;
        private IEnumerable<LaborDtl> SelectLaborDtlWithUpdLock(string company, string employeeNum, DateTime? payrollDate, decimal laborHrs, string timeStatus)
        {
            if (selectLaborDtlQuery_9 == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, decimal, string, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, payrollDate_ex, laborHrs_ex, timeStatus_ex) =>
        (from row in ctx.LaborDtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value &&
         row.LaborHrs == laborHrs_ex &&
         row.TimeStatus == timeStatus_ex
         select row);
                selectLaborDtlQuery_9 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlQuery_9(this.Db, company, employeeNum, payrollDate, laborHrs, timeStatus);
        }

        //HOLDLOCK



        static Func<ErpContext, string, int, int, IEnumerable<LaborDtl>> selectLaborDtlQuery_10;
        private IEnumerable<LaborDtl> SelectLaborDtl(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (selectLaborDtlQuery_10 == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row);
                selectLaborDtlQuery_10 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlQuery_10(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        // **************************************************************************************************

        class LaborDtlCustom
        {
            public decimal ClockOutTime { get; set; }
            public DateTime calc_ClockInDate { get; set; }
        }

        static Func<ErpContext, string, int, decimal, IEnumerable<LaborDtlCustom>> selectLaborDtl2Query;
        private IEnumerable<LaborDtlCustom> SelectLaborDtl2(string company, int laborHedSeq, decimal laborHedClockInTime)
        {
            if (selectLaborDtl2Query == null)
            {
                Expression<Func<ErpContext, string, int, decimal, IEnumerable<LaborDtlCustom>>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborHedClockInTime_ex) =>
        (from row in ctx.LaborDtl
         where (row.Company == company_ex) &&
        (row.LaborHedSeq == laborHedSeq_ex)
         select new LaborDtlCustom
         {
             ClockOutTime = row.ClockOutTime,
#if !USE_EF_CORE
             calc_ClockInDate = row.ClockOutTime < laborHedClockInTime_ex ? (DateTime)DbFunctions.AddDays(row.ClockInDate, 1) : (DateTime)row.ClockInDate
#endif
         });
                selectLaborDtl2Query = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtl2Query(this.Db, company, laborHedSeq, laborHedClockInTime);
        }

        // **************************************************************************************************

        static Func<ErpContext, string, int, IEnumerable<LaborDtl>> selectLaborDtl2Query_12;
        private IEnumerable<LaborDtl> SelectLaborDtl12(string company, int laborHedSeq)
        {
            if (selectLaborDtl2Query_12 == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborDtl
         where (row.Company == company_ex) &&
        (row.LaborHedSeq == laborHedSeq_ex)
         orderby row.Company, row.ClockInDate descending, row.ClockinTime descending, row.ClockOutTime descending
         select row);
                selectLaborDtl2Query_12 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtl2Query_12(this.Db, company, laborHedSeq);
        }

        static Func<ErpContext, string, int, decimal, LaborDtl> selectLaborDtl2Query_13;
        private LaborDtl SelectLaborDtl13(string company, int laborHedSeq, decimal clockOutTime)
        {
            if (selectLaborDtl2Query_13 == null)
            {
                Expression<Func<ErpContext, string, int, decimal, LaborDtl>> expression =
      (ctx, company_ex, laborHedSeq_ex, clockOutTime_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
                row.LaborHedSeq == laborHedSeq_ex &&
                row.ClockinTime == clockOutTime_ex
         select row).FirstOrDefault();
                selectLaborDtl2Query_13 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtl2Query_13(this.Db, company, laborHedSeq, clockOutTime);
        }



        static Func<ErpContext, string, string, DateTime?, string, string, string, string, string, string, int, int, string, string, string, IEnumerable<LaborDtl>> selectLaborDtl2Query_2;
        private IEnumerable<LaborDtl> SelectLaborDtl2(string company, string empNum, DateTime? payDate, string labType, string labTypePseudo, string projID, string phaseID,
            string timeTypCode, string jobNum, int asmNum, int oprSeq, string indCode, string roleCd, string resGrpID)
        {
            if (selectLaborDtl2Query_2 == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, string, string, string, string, string, string, int, int, string, string, string, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, payrollDate_ex, laborType_ex, laborTypePseudo_ex, projectID_ex, phaseID_ex, timeTypCd_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, indirectCode_ex, roleCd_ex, resourceGrpID_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value &&
         row.LaborType == laborType_ex &&
         row.LaborTypePseudo == laborTypePseudo_ex &&
         row.ProjectID == projectID_ex &&
         row.PhaseID == phaseID_ex &&
         row.TimeTypCd == timeTypCd_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.IndirectCode == indirectCode_ex &&
         row.RoleCd == roleCd_ex &&
         row.ResourceGrpID == resourceGrpID_ex
         select row);
                selectLaborDtl2Query_2 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtl2Query_2(this.Db, company, empNum, payDate, labType, labTypePseudo, projID, phaseID, timeTypCode, jobNum, asmNum, oprSeq, indCode, roleCd, resGrpID);
        }


        static Func<ErpContext, string, int, IEnumerable<LaborDtl>> selectLaborDtl3Query;
        private IEnumerable<LaborDtl> SelectLaborDtl3(string company, int laborHedSeq)
        {
            if (selectLaborDtl3Query == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborDtl
         where (row.Company == company_ex) &&
        (row.LaborHedSeq == laborHedSeq_ex)
         orderby row.Company, row.LaborHedSeq, row.ClockOutMinute descending
         select row);
                selectLaborDtl3Query = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtl3Query(this.Db, company, laborHedSeq);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, DateTime?, string, string, string, string, string, string, int, int, string, string, string, IEnumerable<LaborDtl>> selectLaborDtl3Query_2;
        private IEnumerable<LaborDtl> SelectLaborDtlWithUpdLock3(string company, string empNum, DateTime? payDate, string labType, string labTypePseudo, string projID, string phaseID,
            string timeTypCode, string jobNum, int asmNum, int oprSeq, string indCode, string roleCd, string resGrpID)
        {
            if (selectLaborDtl3Query_2 == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, string, string, string, string, string, string, int, int, string, string, string, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, payrollDate_ex, laborType_ex, laborTypePseudo_ex, projectID_ex, phaseID_ex, timeTypCd_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, indirectCode_ex, roleCd_ex, resourceGrpID_ex) =>
        (from row in ctx.LaborDtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value &&
         row.LaborType == laborType_ex &&
         row.LaborTypePseudo == laborTypePseudo_ex &&
         row.ProjectID == projectID_ex &&
         row.PhaseID == phaseID_ex &&
         row.TimeTypCd == timeTypCd_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.IndirectCode == indirectCode_ex &&
         row.RoleCd == roleCd_ex &&
         row.ResourceGrpID == resourceGrpID_ex
         select row);
                selectLaborDtl3Query_2 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtl3Query_2(this.Db, company, empNum, payDate, labType, labTypePseudo, projID, phaseID, timeTypCode, jobNum, asmNum, oprSeq, indCode, roleCd, resGrpID);
        }


        static Func<ErpContext, string, int, IEnumerable<LaborDtl>> selectLaborDtl4Query;
        private IEnumerable<LaborDtl> SelectLaborDtl4(string company, int laborHedSeq)
        {
            if (selectLaborDtl4Query == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row);
                selectLaborDtl4Query = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtl4Query(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, LaborDtlParams, IEnumerable<LaborDtl>> selectWeeklyLaborDtlQuery;
        private IEnumerable<LaborDtl> SelectWeeklyLaborDtlWithUpdLock(string company, LaborDtlParams ipLaborDtlParams2)
        {
            if (selectWeeklyLaborDtlQuery == null)
            {
                Expression<Func<ErpContext, string, LaborDtlParams, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, LaborDtlParams2_ex) =>
        (from row in ctx.LaborDtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.EmployeeNum == LaborDtlParams2_ex.EmployeeNum &&
         row.PayrollDate.Value >= LaborDtlParams2_ex.WeekBeginDate.Value &&
         row.PayrollDate.Value <= LaborDtlParams2_ex.WeekEndDate.Value &&
         row.LaborType == LaborDtlParams2_ex.LaborType &&
         row.TimeStatus == LaborDtlParams2_ex.TimeStatus &&
         row.LaborTypePseudo == LaborDtlParams2_ex.LaborTypePseudo &&
         row.ProjectID == LaborDtlParams2_ex.ProjectID &&
         row.PhaseID == LaborDtlParams2_ex.PhaseID &&
         row.TimeTypCd == LaborDtlParams2_ex.TimeTypCd &&
         row.JobNum == LaborDtlParams2_ex.JobNum &&
         row.AssemblySeq == LaborDtlParams2_ex.AssemblySeq &&
         row.OprSeq == LaborDtlParams2_ex.OprSeq &&
         row.IndirectCode == LaborDtlParams2_ex.IndirectCode &&
         row.RoleCd == LaborDtlParams2_ex.RoleCd &&
         row.ResourceGrpID == LaborDtlParams2_ex.ResourceGrpID &&
         row.ResourceID == LaborDtlParams2_ex.ResourceID &&
         row.ExpenseCode == LaborDtlParams2_ex.ExpenseCode &&
         row.Shift == LaborDtlParams2_ex.Shift &&
         row.QuickEntryCode == LaborDtlParams2_ex.QuickEntryCode &&
         row.LaborHrs > LaborDtlParams2_ex.LaborHrs
         select row);
                selectWeeklyLaborDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectWeeklyLaborDtlQuery(this.Db, company, ipLaborDtlParams2);
        }

        static Func<ErpContext, string, LaborDtlParams, IEnumerable<LaborDtl>> selectWeeklyLaborDtlQuery2;
        private IEnumerable<LaborDtl> SelectWeeklyLaborDtlWithUpdLock2(string company, LaborDtlParams ipLaborDtlParams2)
        {
            if (selectWeeklyLaborDtlQuery2 == null)
            {
                Expression<Func<ErpContext, string, LaborDtlParams, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, LaborDtlParams2_ex) =>
        (from row in ctx.LaborDtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.EmployeeNum == LaborDtlParams2_ex.EmployeeNum &&
         row.PayrollDate.Value >= LaborDtlParams2_ex.WeekBeginDate.Value &&
         row.PayrollDate.Value <= LaborDtlParams2_ex.WeekEndDate.Value &&
         row.LaborType == LaborDtlParams2_ex.LaborType &&
         row.TimeStatus == LaborDtlParams2_ex.TimeStatus &&
         row.LaborTypePseudo == LaborDtlParams2_ex.LaborTypePseudo &&
         row.ProjectID == LaborDtlParams2_ex.ProjectID &&
         row.PhaseID == LaborDtlParams2_ex.PhaseID &&
         row.TimeTypCd == LaborDtlParams2_ex.TimeTypCd &&
         row.JobNum == LaborDtlParams2_ex.JobNum &&
         row.AssemblySeq == LaborDtlParams2_ex.AssemblySeq &&
         row.OprSeq == LaborDtlParams2_ex.OprSeq &&
         row.IndirectCode == LaborDtlParams2_ex.IndirectCode &&
         row.RoleCd == LaborDtlParams2_ex.RoleCd &&
         row.ResourceGrpID == LaborDtlParams2_ex.ResourceGrpID &&
         row.ResourceID == LaborDtlParams2_ex.ResourceID &&
         row.ExpenseCode == LaborDtlParams2_ex.ExpenseCode &&
         row.Shift == LaborDtlParams2_ex.Shift &&
         row.QuickEntryCode == LaborDtlParams2_ex.QuickEntryCode &&
         row.LaborHrs != LaborDtlParams2_ex.LaborHrs
         select row);
                selectWeeklyLaborDtlQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectWeeklyLaborDtlQuery2(this.Db, company, ipLaborDtlParams2);
        }


        static Func<ErpContext, string, string, int, int, int, IEnumerable<LaborDtl>> selectLaborDtlWithUpdLockQuery;
        private IEnumerable<LaborDtl> SelectLaborDtlWithUpdLock(string company, string employeeNum, int laborHedSeq, int clockOutMinute, int clockInMInute)
        {
            if (selectLaborDtlWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, employeeNum_ex, laborHedSeq_ex, clockOutMinute_ex, clockInMInute_ex) =>
        (from row in ctx.LaborDtl.With(LockHint.UpdLock)
         where (row.Company == company_ex) &&
        (row.EmployeeNum == employeeNum_ex) &&
        (row.LaborHedSeq == laborHedSeq_ex) &&
        (row.ClockOutMinute >= clockInMInute_ex) &&
        (row.ClockInMInute <= clockOutMinute_ex)
         select row);
                selectLaborDtlWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlWithUpdLockQuery(this.Db, company, employeeNum, laborHedSeq, clockOutMinute, clockInMInute);
        }


        static Func<ErpContext, string, int, IEnumerable<LaborDtl>> selectLaborDtlWithUpdLockQuery_2;
        private IEnumerable<LaborDtl> SelectLaborDtlWithUpdLock(string company, int laborHedSeq)
        {
            if (selectLaborDtlWithUpdLockQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<LaborDtl>>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborDtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row);
                selectLaborDtlWithUpdLockQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlWithUpdLockQuery_2(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, int, bool> existsLaborHedPosted;
        private bool ExistsLaborHedPosted(string company, int laborHedSeq)
        {
            if (existsLaborHedPosted == null)
            {
                Expression<Func<ErpContext, string, int, bool>> expression =
                (ctx, company_ex, laborHedSeq_ex) =>
                 (from row in ctx.LaborDtl
                  where row.Company == company_ex &&
                  row.LaborHedSeq == laborHedSeq_ex &&
                  row.WipPosted == true
                  select row).Any();
                existsLaborHedPosted = DBExpressionCompiler.Compile(expression);
            }
            return existsLaborHedPosted(this.Db, company, laborHedSeq);
        }

        static Func<ErpContext, string, string, DateTime?, int, bool> existsLaborHedNotPosted;
        private bool ExistsLaborHedNotPosted(string company, string employeeNum, DateTime? date, int laborHedSeq)
        {
            if (existsLaborHedNotPosted == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, int, bool>> expression =
                (ctx, company_ex, employee_ex, date_ex, laborHedSeq_ex) =>
                 (from row in ctx.LaborDtl
                  where row.Company == company_ex &&
                  row.EmployeeNum == employee_ex &&
                  row.PayrollDate.Value == date_ex.Value &&
                  row.LaborHedSeq != laborHedSeq_ex &&
                  row.WipPosted == false
                  select row).Any();
                existsLaborHedNotPosted = DBExpressionCompiler.Compile(expression);
            }
            return existsLaborHedNotPosted(this.Db, company, employeeNum, date, laborHedSeq);
        }

        class LaborDtlPartial
        {
            public Guid SysRowID { get; set; }
            public bool EpicorFSA { get; set; }
        }
        static Func<ErpContext, string, int, int, LaborDtlPartial> findFirstLaborDtlSysRowIDQuery;
        private LaborDtlPartial FindFirstLaborDtlSysRowID(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstLaborDtlSysRowIDQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, LaborDtlPartial>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtl
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select new LaborDtlPartial { SysRowID = row.SysRowID, EpicorFSA = row.EpicorFSA }).FirstOrDefault();
                findFirstLaborDtlSysRowIDQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlSysRowIDQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        #endregion LaborDtl Queries

        #region LabotrDtlAction Queries
        static Func<ErpContext, string, int, int, bool> existsLaborDtlActionQuery;
        private bool ExistsLaborDtlAction(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (existsLaborDtlActionQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtlAction
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).Any();
                existsLaborDtlActionQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborDtlActionQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        static Func<ErpContext, string, int, int, IEnumerable<LaborDtlAction>> selectLaborDtlActionQuery;
        private IEnumerable<LaborDtlAction> SelectLaborDtlAction(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (selectLaborDtlActionQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<LaborDtlAction>>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtlAction
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row);
                selectLaborDtlActionQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborDtlActionQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }
        #endregion LabotrDtlAction Queries

        #region LaborDtlComment Queries

        static Func<ErpContext, string, int, int, string, bool> existsLaborDtlCommentQuery;
        private bool ExistsLaborDtlComment(string company, int laborHedSeq, int laborDtlSeq, string commentType)
        {
            if (existsLaborDtlCommentQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, string, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex, commentType_ex) =>
        (from row in ctx.LaborDtlComment
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex &&
         row.CommentType == commentType_ex
         select row).Any();
                existsLaborDtlCommentQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborDtlCommentQuery(this.Db, company, laborHedSeq, laborDtlSeq, commentType);
        }


        static Func<ErpContext, string, int, int, string, Guid, bool> existsLaborDtlCommentQuery_2;
        private bool ExistsLaborDtlComment(string company, int laborHedSeq, int laborDtlSeq, string commentType, Guid sysRowID)
        {
            if (existsLaborDtlCommentQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, int, string, Guid, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex, commentType_ex, sysRowID_ex) =>
        (from row in ctx.LaborDtlComment
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex &&
         row.CommentType == commentType_ex &&
         row.SysRowID != sysRowID_ex
         select row).Any();
                existsLaborDtlCommentQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborDtlCommentQuery_2(this.Db, company, laborHedSeq, laborDtlSeq, commentType, sysRowID);
        }

        static Func<ErpContext, string, int, int, bool> existsLaborDtlCommentQuery_3;
        private bool ExistsLaborDtlComment(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (existsLaborDtlCommentQuery_3 == null)
            {
                Expression<Func<ErpContext, string, int, int, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborDtlComment
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).Any();
                existsLaborDtlCommentQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborDtlCommentQuery_3(this.Db, company, laborHedSeq, laborDtlSeq);
        }
        #endregion LaborDtlComment Queries

        #region LaborDtlGroup Queries

        static Func<ErpContext, string, string, string, LaborDtlGroup> findFirstLaborDtlGroupQuery;
        private LaborDtlGroup FindFirstLaborDtlGroup(string company, string employeeNum, string claimRef)
        {
            if (findFirstLaborDtlGroupQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, LaborDtlGroup>> expression =
      (ctx, company_ex, employeeNum_ex, claimRef_ex) =>
        (from row in ctx.LaborDtlGroup
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.ClaimRef == claimRef_ex
         select row).FirstOrDefault();
                findFirstLaborDtlGroupQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborDtlGroupQuery(this.Db, company, employeeNum, claimRef);
        }
        #endregion LaborDtlGroup Queries

        #region LaborEquip Queries

        static Func<ErpContext, string, int, int, string, LaborEquip> findFirstLaborEquipQuery;
        private LaborEquip FindFirstLaborEquip(string company, int laborHedSeq, int laborDtlSeq, string equipID)
        {
            if (findFirstLaborEquipQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, string, LaborEquip>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex, equipID_ex) =>
        (from row in ctx.LaborEquip
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex &&
         row.EquipID == equipID_ex
         select row).FirstOrDefault();
                findFirstLaborEquipQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborEquipQuery(this.Db, company, laborHedSeq, laborDtlSeq, equipID);
        }


        static Func<ErpContext, string, int, int, IEnumerable<LaborEquip>> selectLaborEquipQuery;
        private IEnumerable<LaborEquip> SelectLaborEquip(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (selectLaborEquipQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<LaborEquip>>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborEquip
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row);
                selectLaborEquipQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborEquipQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        static Func<ErpContext, string, int, int, Guid, string, bool> existsLaborEquipQuery;
        private bool ExistsLaborEquip(string company, int laborHedSeq, int laborDtlSeq, Guid sysRowID, string equipID)
        {
            if (existsLaborEquipQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, Guid, string, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex, sysRowID_ex, equipID_ex) =>
        (from row in ctx.LaborEquip
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex &&
         row.EquipID == equipID_ex &&
         row.SysRowID != sysRowID_ex
         select row).Any();
                existsLaborEquipQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborEquipQuery(this.Db, company, laborHedSeq, laborDtlSeq, sysRowID, equipID);
        }

        #endregion LaborEquip Queries

        #region LaborHed Queries

        static Func<ErpContext, string, int, bool> existsLaborHedQuery;
        private bool ExistsLaborHed(string company, int laborHedSeq)
        {
            if (existsLaborHedQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).Any();
                existsLaborHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborHedQuery(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, string, DateTime?, LaborHed> findFirstLaborHedQuery;
        private LaborHed FindFirstLaborHed(string company, string employeeNum, DateTime? payrollDate)
        {
            if (findFirstLaborHedQuery == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, LaborHed>> expression =
      (ctx, company_ex, employeeNum_ex, payrollDate_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value
         select row).FirstOrDefault();
                findFirstLaborHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHedQuery(this.Db, company, employeeNum, payrollDate);
        }

        static Func<ErpContext, string, string, LaborHed> findFirstLaborHed;
        private LaborHed FindFirstLaborHed(string company, string employeeNum)
        {
            if (findFirstLaborHed == null)
            {
                Expression<Func<ErpContext, string, string, LaborHed>> expression =
      (ctx, company_ex, employeeNum_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.ActiveTrans
         select row).FirstOrDefault();
                findFirstLaborHed = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed(this.Db, company, employeeNum);
        }


        static Func<ErpContext, string, int, LaborHed> findFirstLaborHedQuery_2;
        private LaborHed FindFirstLaborHed(string company, int laborHedSeq)
        {
            if (findFirstLaborHedQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHedQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHedQuery_2(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, Guid, LaborHed> findFirstLaborHedQuery_3;
        private LaborHed FindFirstLaborHed(Guid sysRowID)
        {
            if (findFirstLaborHedQuery_3 == null)
            {
                Expression<Func<ErpContext, Guid, LaborHed>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.LaborHed
         where row.SysRowID == sysRowID_ex
         select row).FirstOrDefault();
                findFirstLaborHedQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHedQuery_3(this.Db, sysRowID);
        }

        static Func<ErpContext, Guid, string> findFirstLaborHedHCMPayHoursCalcTypeQuery;
        private string FindFirstLaborHedHCMPayHoursCalcType(Guid sysRowID)
        {
            if (findFirstLaborHedHCMPayHoursCalcTypeQuery == null)
            {
                Expression<Func<ErpContext, Guid, string>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.LaborHed
         where row.SysRowID == sysRowID_ex
         select row.HCMPayHoursCalcType).FirstOrDefault();
                findFirstLaborHedHCMPayHoursCalcTypeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHedHCMPayHoursCalcTypeQuery(this.Db, sysRowID);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, DateTime?, int, LaborHed> findFirstLaborHedQuery_4;
        private LaborHed FindFirstLaborHed(string company, string employeeNum, DateTime? payrollDate, int shift)
        {
            if (findFirstLaborHedQuery_4 == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, int, LaborHed>> expression =
      (ctx, company_ex, employeeNum_ex, payrollDate_ex, shift_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value &&
         row.Shift == shift_ex
         select row).FirstOrDefault();
                findFirstLaborHedQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHedQuery_4(this.Db, company, employeeNum, payrollDate, shift);
        }

        static Func<ErpContext, string, string, DateTime?, int, LaborHed> findFirstLaborHedQuery_5;
        private LaborHed FindFirstLaborHedNotPosted(string company, string employeeNum, DateTime? payrollDate, int shift)
        {
            if (findFirstLaborHedQuery_5 == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, int, LaborHed>> expression =
                (ctx, company_ex, employeeNum_ex, payrollDate_ex, shift_ex) =>
                (from row in ctx.LaborHed
                 where row.Company == company_ex &&
                 row.EmployeeNum == employeeNum_ex &&
                 row.PayrollDate.Value == payrollDate_ex.Value &&
                 row.Shift == shift_ex
                 where
ctx.LaborDtl.Any(at => at.Company == company_ex && at.EmployeeNum == employeeNum_ex && at.PayrollDate.Value == payrollDate_ex.Value && at.Shift == row.Shift && at.LaborHedSeq == row.LaborHedSeq && at.WipPosted == false)
                 select row).FirstOrDefault();
                findFirstLaborHedQuery_5 = DBExpressionCompiler.Compile(expression);
            }
            return findFirstLaborHedQuery_5(this.Db, company, employeeNum, payrollDate, shift);
        }

        static Func<ErpContext, string, int, LaborHed> findFirstLaborHed10Query;
        private LaborHed FindFirstLaborHed10(string company, int laborHedSeq)
        {
            if (findFirstLaborHed10Query == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHed10Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed10Query(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, int, LaborHed> findFirstLaborHed11Query;
        private LaborHed FindFirstLaborHed11(string company, int laborHedSeq)
        {
            if (findFirstLaborHed11Query == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHed11Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed11Query(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, string, DateTime?, LaborHed> findFirstLaborHed2Query;
        private LaborHed FindFirstLaborHed2(string company, string employeeNum, DateTime? payrollDate)
        {
            if (findFirstLaborHed2Query == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, LaborHed>> expression =
      (ctx, company_ex, employeeNum_ex, payrollDate_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.EmployeeNum == employeeNum_ex &&
         row.PayrollDate.Value == payrollDate_ex.Value
         select row).FirstOrDefault();
                findFirstLaborHed2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed2Query(this.Db, company, employeeNum, payrollDate);
        }


        static Func<ErpContext, string, int, LaborHed> findFirstLaborHed2Query_2;
        private LaborHed FindFirstLaborHed2(string company, int laborHedSeq)
        {
            if (findFirstLaborHed2Query_2 == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHed2Query_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed2Query_2(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, int, LaborHed> findFirstLaborHed3Query;
        private LaborHed FindFirstLaborHed3(string company, int laborHedSeq)
        {
            if (findFirstLaborHed3Query == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHed3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed3Query(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, int, LaborHed> findFirstLaborHed4Query;
        private LaborHed FindFirstLaborHed4(string company, int laborHedSeq)
        {
            if (findFirstLaborHed4Query == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHed4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed4Query(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, int, LaborHed> findFirstLaborHed5Query;
        private LaborHed FindFirstLaborHed5(string company, int laborHedSeq)
        {
            if (findFirstLaborHed5Query == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHed5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed5Query(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, int, LaborHed> findFirstLaborHed6Query;
        private LaborHed FindFirstLaborHed6(string company, int laborHedSeq)
        {
            if (findFirstLaborHed6Query == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHed6Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed6Query(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, int, LaborHed> findFirstLaborHed7Query;
        private LaborHed FindFirstLaborHed7(string company, int laborHedSeq)
        {
            if (findFirstLaborHed7Query == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHed7Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed7Query(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, int, LaborHed> findFirstLaborHed8Query;
        private LaborHed FindFirstLaborHed8(string company, int laborHedSeq)
        {
            if (findFirstLaborHed8Query == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHed8Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed8Query(this.Db, company, laborHedSeq);
        }


        static Func<ErpContext, string, int, LaborHed> findFirstLaborHed9Query;
        private LaborHed FindFirstLaborHed9(string company, int laborHedSeq)
        {
            if (findFirstLaborHed9Query == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHed9Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHed9Query(this.Db, company, laborHedSeq);
        }

        static Func<ErpContext, string, int, LaborHed> findFirstLaborHedWithUpdLockQuery;
        private LaborHed FindFirstLaborHedWithUpdLockQuery(string company, int laborHedSeq)
        {
            if (findFirstLaborHedWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, LaborHed>> expression =
      (ctx, company_ex, laborHedSeq_ex) =>
        (from row in ctx.LaborHed.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex
         select row).FirstOrDefault();
                findFirstLaborHedWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLaborHedWithUpdLockQuery(this.Db, company, laborHedSeq);
        }

        static Func<ErpContext, string, string, DateTime?, IEnumerable<LaborHed>> selectLaborHedQuery;
        private IEnumerable<LaborHed> SelectLaborHed(string company, string empID, DateTime? payrollDate)
        {
            if (selectLaborHedQuery == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, IEnumerable<LaborHed>>> expression =
                (ctx, company_ex, empID_ex, payrollDate_ex) =>
                    (from row in ctx.LaborHed
                     where row.Company == company_ex &&
                     row.EmployeeNum == empID_ex &&
                     row.PayrollDate == payrollDate_ex
                     select row);
                selectLaborHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborHedQuery(this.Db, company, empID, payrollDate);
        }
        static Func<ErpContext, string, string, LaborHed> findFirstActiveLaborHedSeq;
        private LaborHed FindFirstActiveLaborHed(string company, string empID)
        {
            if (findFirstActiveLaborHedSeq == null)
            {
                Expression<Func<ErpContext, string, string, LaborHed>> expression =
                (ctx, company_ex, empID_ex) =>
                (from row in ctx.LaborHed
                 where row.Company == company_ex &&
                 row.EmployeeNum == empID_ex &&
                 row.ActiveTrans == true
                 select row).FirstOrDefault();
                findFirstActiveLaborHedSeq = DBExpressionCompiler.Compile(expression);
            }

            return findFirstActiveLaborHedSeq(this.Db, company, empID);
        }
        #endregion LaborHed Queries

        #region LaborPart Queries

        static Func<ErpContext, string, int, int, bool> existsLaborPartQuery;
        private bool ExistsLaborPart(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (existsLaborPartQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborPart
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).Any();
                existsLaborPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsLaborPartQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }



        static Func<ErpContext, string, int, int, IEnumerable<LaborPart>> selectLaborPartQuery;
        private IEnumerable<LaborPart> SelectLaborPart(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (selectLaborPartQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<LaborPart>>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborPart
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row);
                selectLaborPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborPartQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }


        static Func<ErpContext, string, string, int, int, LaborPart> FindFirstLaborPartQuery;
        private LaborPart FindFirstLaborPart(string company, string partNum, int laborHedSeq, int laborDtlSeq)
        {
            if (FindFirstLaborPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, LaborPart>> expression =
      (ctx, company_ex, partNum_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborPart
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                FindFirstLaborPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return FindFirstLaborPartQuery(this.Db, company, partNum, laborHedSeq, laborDtlSeq);
        }

        static Func<ErpContext, Guid, LaborPart> FindFirstLaborPartQuery2;
        private LaborPart FindFirstLaborPart(Guid sysRowID)
        {
            if (FindFirstLaborPartQuery2 == null)
            {
                Expression<Func<ErpContext, Guid, LaborPart>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.LaborPart
         where row.SysRowID == sysRowID_ex
         select row).FirstOrDefault();
                FindFirstLaborPartQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return FindFirstLaborPartQuery2(this.Db, sysRowID);
        }

        //HOLDLOCK



        static Func<ErpContext, string, int, int, string, IEnumerable<LaborPart>> selectLaborPart2Query;
        private IEnumerable<LaborPart> SelectLaborPart2(string company, int laborHedSeq, int laborDtlSeq, string mainPart)
        {
            if (selectLaborPart2Query == null)
            {
                Expression<Func<ErpContext, string, int, int, string, IEnumerable<LaborPart>>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex, mainPart_ex) =>
        (from row in ctx.LaborPart
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex &&
         row.PartNum != mainPart_ex
         select row);
                selectLaborPart2Query = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborPart2Query(this.Db, company, laborHedSeq, laborDtlSeq, mainPart);
        }


        static Func<ErpContext, string, int, int, IEnumerable<LaborPart>> selectLaborPart3Query;
        private IEnumerable<LaborPart> SelectLaborPart3(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (selectLaborPart3Query == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<LaborPart>>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborPart
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row);
                selectLaborPart3Query = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborPart3Query(this.Db, company, laborHedSeq, laborDtlSeq);
        }


        static Func<ErpContext, string, int, int, IEnumerable<LaborPart>> selectLaborPart4Query;
        private IEnumerable<LaborPart> SelectLaborPart4(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (selectLaborPart4Query == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<LaborPart>>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborPart
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row);
                selectLaborPart4Query = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborPart4Query(this.Db, company, laborHedSeq, laborDtlSeq);
        }


        static Func<ErpContext, string, int, int, IEnumerable<LaborPart>> selectLaborPartWithUpdLockQuery;
        private IEnumerable<LaborPart> SelectLaborPartWithUpdLock(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (selectLaborPartWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<LaborPart>>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.LaborPart.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row);
                selectLaborPartWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectLaborPartWithUpdLockQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }
        #endregion LaborPart Queries

        #region MtlQueue Queries

        static Func<ErpContext, string, int, MtlQueue> findFirstMtlQueueWithUpdLockQuery;
        private MtlQueue FindFirstMtlQueueWithUpdLock(string company, int nctranID)
        {
            if (findFirstMtlQueueWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, MtlQueue>> expression =
      (ctx, company_ex, nctranID_ex) =>
        (from row in ctx.MtlQueue.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.NCTranID == nctranID_ex
         select row).FirstOrDefault();
                findFirstMtlQueueWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstMtlQueueWithUpdLockQuery(this.Db, company, nctranID);
        }

        //HOLDLOCK



        static Func<ErpContext, string, int, string, MtlQueue> findFirstMtlQueueWithUpdLockQuery_2;
        private MtlQueue FindFirstMtlQueueWithUpdLock(string company, int nctranID, string _StartsWith)
        {
            if (findFirstMtlQueueWithUpdLockQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, string, MtlQueue>> expression =
      (ctx, company_ex, nctranID_ex, _StartsWith_ex) =>
        (from row in ctx.MtlQueue.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.NCTranID == nctranID_ex &&
         !(row.TranType.StartsWith(_StartsWith_ex))
         select row).FirstOrDefault();
                findFirstMtlQueueWithUpdLockQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstMtlQueueWithUpdLockQuery_2(this.Db, company, nctranID, _StartsWith);
        }
        #endregion MtlQueue Queries

        #region NonConf Queries

        static Func<ErpContext, string, int, int, NonConf> findFirstNonConfQuery;
        private NonConf FindFirstNonConf(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstNonConfQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, NonConf>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.NonConf
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstNonConfQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstNonConfQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        //HOLDLOCK



        static Func<ErpContext, string, int, int, NonConf> findFirstNonConf2Query;
        private NonConf FindFirstNonConf2(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstNonConf2Query == null)
            {
                Expression<Func<ErpContext, string, int, int, NonConf>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.NonConf
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstNonConf2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstNonConf2Query(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        static Func<ErpContext, string, int, int, bool> existsNonConfWithProcessedInspectionQuery;
        private bool ExistsNonConfWithProcessedInspection(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (existsNonConfWithProcessedInspectionQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, bool>> expression =
                (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) => ctx.NonConf.Where(row => row.Company == company_ex
                                                                                           && row.LaborHedSeq == laborHedSeq_ex
                                                                                           && row.LaborDtlSeq == laborDtlSeq_ex
                                                                                           && row.InspectionPending == false).Any();
                existsNonConfWithProcessedInspectionQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsNonConfWithProcessedInspectionQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        static Func<ErpContext, string, string, int, int, bool> existsNonConfQuery;
        private bool ExistsNonConf(string company, string jobNum, int laborHedSeq, int laborDtlSeq)
        {
            if (existsNonConfQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
                (ctx, company_ex, jobnum_ex, laborHedSeq_ex, laborDtlSeq_ex) => ctx.NonConf.Where(row => row.Company == company_ex
                                                                                           && row.JobNum == jobnum_ex
                                                                                           && row.LaborHedSeq == laborHedSeq_ex
                                                                                           && row.LaborDtlSeq == laborDtlSeq_ex
                                                                                           && row.TrnTyp == "D").Any();
                existsNonConfQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsNonConfQuery(this.Db, company, jobNum, laborHedSeq, laborDtlSeq);
        }

        static Func<ErpContext, string, int, int, NonConf> findFirstNonConfWithUpdLockQuery;
        private NonConf FindFirstNonConfWithUpdLock(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (findFirstNonConfWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, NonConf>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
        (from row in ctx.NonConf.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex
         select row).FirstOrDefault();
                findFirstNonConfWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstNonConfWithUpdLockQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        #endregion NonConf Queries

        #region OpMaster Queries

        static Func<ErpContext, string, string, OpMaster> findFirstOpMasterQuery;
        private OpMaster FindFirstOpMaster(string company, string opCode)
        {
            if (findFirstOpMasterQuery == null)
            {
                Expression<Func<ErpContext, string, string, OpMaster>> expression =
      (ctx, company_ex, opCode_ex) =>
        (from row in ctx.OpMaster
         where row.Company == company_ex &&
         row.OpCode == opCode_ex
         select row).FirstOrDefault();
                findFirstOpMasterQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOpMasterQuery(this.Db, company, opCode);
        }


        static Func<ErpContext, string, string, OpMaster> findFirstOpMaster2Query;
        private OpMaster FindFirstOpMaster2(string company, string opCode)
        {
            if (findFirstOpMaster2Query == null)
            {
                Expression<Func<ErpContext, string, string, OpMaster>> expression =
      (ctx, company_ex, opCode_ex) =>
        (from row in ctx.OpMaster
         where row.Company == company_ex &&
         row.OpCode == opCode_ex
         select row).FirstOrDefault();
                findFirstOpMaster2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOpMaster2Query(this.Db, company, opCode);
        }


        static Func<ErpContext, string, string, OpMaster> findFirstOpMaster3Query;
        private OpMaster FindFirstOpMaster3(string company, string opCode)
        {
            if (findFirstOpMaster3Query == null)
            {
                Expression<Func<ErpContext, string, string, OpMaster>> expression =
      (ctx, company_ex, opCode_ex) =>
        (from row in ctx.OpMaster
         where row.Company == company_ex &&
         row.OpCode == opCode_ex
         select row).FirstOrDefault();
                findFirstOpMaster3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOpMaster3Query(this.Db, company, opCode);
        }


        static Func<ErpContext, string, string, OpMaster> findFirstOpMaster4Query;
        private OpMaster FindFirstOpMaster4(string company, string opCode)
        {
            if (findFirstOpMaster4Query == null)
            {
                Expression<Func<ErpContext, string, string, OpMaster>> expression =
      (ctx, company_ex, opCode_ex) =>
        (from row in ctx.OpMaster
         where row.Company == company_ex &&
         row.OpCode == opCode_ex
         select row).FirstOrDefault();
                findFirstOpMaster4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOpMaster4Query(this.Db, company, opCode);
        }


        static Func<ErpContext, string, string, OpMaster> findFirstOpMaster6Query;
        private OpMaster FindFirstOpMaster6(string company, string opCode)
        {
            if (findFirstOpMaster6Query == null)
            {
                Expression<Func<ErpContext, string, string, OpMaster>> expression =
      (ctx, company_ex, opCode_ex) =>
        (from row in ctx.OpMaster
         where row.Company == company_ex &&
         row.OpCode == opCode_ex
         select row).FirstOrDefault();
                findFirstOpMaster6Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOpMaster6Query(this.Db, company, opCode);
        }
        #endregion OpMaster Queries

        #region Part Queries

        static Func<ErpContext, string, string, bool, bool> existsPartTrackSerialNumQuery;
        private bool ExistsPartTrackSerialNum(string company, string partNum, bool trackSerialNum)
        {
            if (existsPartTrackSerialNumQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
              (ctx, company_ex, partNum_ex, trackSerialNum_ex) =>
                (from row in ctx.Part
                 where row.Company == company_ex &&
                 row.PartNum == partNum_ex &&
                 row.TrackSerialNum == trackSerialNum_ex
                 select row).Any();
                existsPartTrackSerialNumQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPartTrackSerialNumQuery(Db, company, partNum, trackSerialNum);
        }

        private class PartPartial : TempRowBase
        {
            public bool TrackLots { get; set; }
            public bool LotUseGlobalSeq { get; set; }
            public string AttrClassID { get; set; }
            public bool TrackInventoryByRevision { get; set; }
            public bool TrackInventoryAttributes { get; set; }
        }

        static Func<ErpContext, string, string, PartPartial> findFirstPartPartialQuery;
        private PartPartial FindFirstPartPartial(string company, string partNum)
        {
            if (findFirstPartPartialQuery == null)
            {
                Expression<Func<ErpContext, string, string, PartPartial>> expression =
              (ctx, company_ex, partNum_ex) =>
                (from row in ctx.Part
                 where row.Company == company_ex &&
                 row.PartNum == partNum_ex
                 select new PartPartial()
                 {
                     TrackLots = row.TrackLots,
                     LotUseGlobalSeq = row.LotUseGlobalSeq,
                     AttrClassID = row.AttrClassID,
                     TrackInventoryByRevision = row.TrackInventoryByRevision,
                     TrackInventoryAttributes = row.TrackInventoryAttributes
                 }).FirstOrDefault();
                findFirstPartPartialQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartPartialQuery(this.Db, company, partNum);
        }

        static Func<ErpContext, string, string, Part> findFirstPartQuery;
        private Part FindFirstPart(string company, string partNum)
        {
            if (findFirstPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, Part>> expression =
      (ctx, company_ex, partNum_ex) =>
        (from row in ctx.Part
         where row.Company == company_ex &&
         row.PartNum == partNum_ex
         select row).FirstOrDefault();
                findFirstPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartQuery(this.Db, company, partNum);
        }

        static Func<ErpContext, string, string, Part> findFirstPart2Query;
        private Part FindFirstPart2(string company, string partNum)
        {
            if (findFirstPart2Query == null)
            {
                Expression<Func<ErpContext, string, string, Part>> expression =
      (ctx, company_ex, partNum_ex) =>
        (from row in ctx.Part
         where row.Company == company_ex &&
         row.PartNum == partNum_ex
         select row).FirstOrDefault();
                findFirstPart2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPart2Query(this.Db, company, partNum);
        }
        #endregion Part Queries

        #region PartTran Queries

        static Func<ErpContext, string, DateTime, int, int, PartTran> findFirstPartTranQuery;
        private PartTran FindFirstPartTran(string company, DateTime sysDate, int sysTime, int tranNum)
        {
            if (findFirstPartTranQuery == null)
            {
                Expression<Func<ErpContext, string, DateTime, int, int, PartTran>> expression =
      (ctx, company_ex, sysDate_ex, sysTime_ex, tranNum_ex) =>
        (from row in ctx.PartTran
         where row.Company == company_ex &&
         row.SysDate == sysDate_ex &&
         row.SysTime == sysTime_ex &&
         row.TranNum == tranNum_ex
         select row).FirstOrDefault();
                findFirstPartTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartTranQuery(this.Db, company, sysDate, sysTime, tranNum);
        }
        #endregion PartTran Queries

        #region PBRoleRt Queries

        static Func<ErpContext, string, string, string, string, bool> existsPBRoleRtQuery;
        private bool ExistsPBRoleRt(string company, string projectID, string roleCd, string timeTypCd)
        {
            if (existsPBRoleRtQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, bool>> expression =
      (ctx, company_ex, projectID_ex, roleCd_ex, timeTypCd_ex) =>
        (from row in ctx.PBRoleRt
         where row.Company == company_ex &&
         row.ProjectID == projectID_ex &&
         row.RoleCd == roleCd_ex &&
         row.TimeTypCd == timeTypCd_ex
         select row).Any();
                existsPBRoleRtQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPBRoleRtQuery(this.Db, company, projectID, roleCd, timeTypCd);
        }
        #endregion PBRoleRt Queries

        #region PkgControlHeader Queries

        static Func<ErpContext, string, string, bool, bool> existsPkgControlHeaderOutboundContainerQuery;
        private bool ExistsPkgControlHeaderOutboundContainer(string company, string pcid, bool outboundContainer)
        {
            if (existsPkgControlHeaderOutboundContainerQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
              (ctx, company_ex, pcid_ex, outboundContainer_ex) =>
                (from row in ctx.PkgControlHeader
                 where row.Company == company_ex &&
                 row.PCID == pcid_ex &&
                 row.OutboundContainer == outboundContainer_ex
                 select row).Any();
                existsPkgControlHeaderOutboundContainerQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPkgControlHeaderOutboundContainerQuery(Db, company, pcid, outboundContainer);
        }

        #endregion PkgControlHeader Queries

        #region PkgControlStageHeader Queries

        static Func<ErpContext, string, string, bool, bool> existsPkgControlStageHeaderOutboundContainerQuery;
        private bool ExistsPkgControlStageHeaderOutboundContainer(string company, string pcid, bool outboundContainer)
        {
            if (existsPkgControlStageHeaderOutboundContainerQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
              (ctx, company_ex, pcid_ex, outboundContainer_ex) =>
                (from row in ctx.PkgControlStageHeader
                 where row.Company == company_ex &&
                 row.PCID == pcid_ex &&
                 row.OutboundContainer == outboundContainer_ex
                 select row).Any();
                existsPkgControlStageHeaderOutboundContainerQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPkgControlStageHeaderOutboundContainerQuery(Db, company, pcid, outboundContainer);
        }

        #endregion PkgControlStageHeader Queries

        #region PkgControlStageItem Queries

        static Func<ErpContext, string, string, string, bool> existsPkgControlStageItemDifferentItemPartNumQuery;
        private bool ExistsPkgControlStageItemDifferentItemPartNum(string company, string pcid, string itemPartNum)
        {
            if (existsPkgControlStageItemDifferentItemPartNumQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
              (ctx, company_ex, pcid_ex, itemPartNum_ex) =>
                (from row in ctx.PkgControlStageItem
                 where row.Company == company_ex &&
                 row.PCID == pcid_ex &&
                 row.ItemPartNum != itemPartNum_ex
                 select row).Any();
                existsPkgControlStageItemDifferentItemPartNumQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPkgControlStageItemDifferentItemPartNumQuery(Db, company, pcid, itemPartNum);
        }

        static Func<ErpContext, string, string, string, bool> existsPkgControlStageItemDifferentSupplyJobNumQuery;
        private bool ExistsPkgControlStageItemDifferentSupplyJobNum(string company, string pcid, string supplyJobNum)
        {
            if (existsPkgControlStageItemDifferentSupplyJobNumQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
              (ctx, company_ex, pcid_ex, supplyJobNum_ex) =>
                (from row in ctx.PkgControlStageItem
                 where row.Company == company_ex &&
                 row.PCID == pcid_ex &&
                 row.SupplyJobNum != supplyJobNum_ex
                 select row).Any();
                existsPkgControlStageItemDifferentSupplyJobNumQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPkgControlStageItemDifferentSupplyJobNumQuery(Db, company, pcid, supplyJobNum);
        }

        #endregion PkgControlStageItem Queries

        #region Plant Queries

        static Func<ErpContext, string, string, Plant> findFirstPlantQuery;
        private Plant FindFirstPlant(string company, string plant)
        {
            if (findFirstPlantQuery == null)
            {
                Expression<Func<ErpContext, string, string, Plant>> expression =
      (ctx, company_ex, plant_ex) =>
        (from row in ctx.Plant
         where row.Company == company_ex &&
         row.Plant1 == plant_ex
         select row).FirstOrDefault();
                findFirstPlantQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlantQuery(this.Db, company, plant);
        }


        static Func<ErpContext, string, string, Plant> findFirstPlant2Query;
        private Plant FindFirstPlant2(string company, string plant)
        {
            if (findFirstPlant2Query == null)
            {
                Expression<Func<ErpContext, string, string, Plant>> expression =
      (ctx, company_ex, plant_ex) =>
        (from row in ctx.Plant
         where row.Company == company_ex &&
         row.Plant1 == plant_ex
         select row).FirstOrDefault();
                findFirstPlant2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlant2Query(this.Db, company, plant);
        }
        #endregion Plant Queries

        #region PlantConfCtrl Queries

        static Func<ErpContext, string, string, bool> isTimeAutoSubmitPlantConfCtrlQuery;
        private bool IsTimeAutoSubmitPlantConfCtrl(string company, string plant)
        {
            if (isTimeAutoSubmitPlantConfCtrlQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                (ctx, company_ex, plant_ex) => ctx.PlantConfCtrl.Where(at => at.Company == company_ex
                                                                          && at.Plant == plant_ex).Select(row => row.TimeAutoSubmit).FirstOrDefault();
                isTimeAutoSubmitPlantConfCtrlQuery = DBExpressionCompiler.Compile(expression);
            }

            return isTimeAutoSubmitPlantConfCtrlQuery(this.Db, company, plant);
        }

        static Func<ErpContext, string, string, PlantConfCtrl> findFirstPlantConfCtrlQuery;
        private PlantConfCtrl FindFirstPlantConfCtrl(string company, string plant)
        {
            if (findFirstPlantConfCtrlQuery == null)
            {
                Expression<Func<ErpContext, string, string, PlantConfCtrl>> expression =
      (ctx, company_ex, plant_ex) =>
        (from row in ctx.PlantConfCtrl
         where row.Company == company_ex &&
         row.Plant == plant_ex
         select row).FirstOrDefault();
                findFirstPlantConfCtrlQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlantConfCtrlQuery(this.Db, company, plant);
        }


        static Func<ErpContext, string, string, PlantConfCtrl> findFirstPlantConfCtrl2Query;
        private PlantConfCtrl FindFirstPlantConfCtrl2(string company, string plant)
        {
            if (findFirstPlantConfCtrl2Query == null)
            {
                Expression<Func<ErpContext, string, string, PlantConfCtrl>> expression =
      (ctx, company_ex, plant_ex) =>
        (from row in ctx.PlantConfCtrl
         where row.Company == company_ex &&
         row.Plant == plant_ex
         select row).FirstOrDefault();
                findFirstPlantConfCtrl2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlantConfCtrl2Query(this.Db, company, plant);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, PlantConfCtrl> findFirstPlantConfCtrl3Query;
        private PlantConfCtrl FindFirstPlantConfCtrl3(string company, string plant)
        {
            if (findFirstPlantConfCtrl3Query == null)
            {
                Expression<Func<ErpContext, string, string, PlantConfCtrl>> expression =
      (ctx, company_ex, plant_ex) =>
        (from row in ctx.PlantConfCtrl
         where row.Company == company_ex &&
         row.Plant == plant_ex
         select row).FirstOrDefault();
                findFirstPlantConfCtrl3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlantConfCtrl3Query(this.Db, company, plant);
        }


        static Func<ErpContext, string, string, PlantConfCtrl> findFirstPlantConfCtrl4Query;
        private PlantConfCtrl FindFirstPlantConfCtrl4(string company, string plant)
        {
            if (findFirstPlantConfCtrl4Query == null)
            {
                Expression<Func<ErpContext, string, string, PlantConfCtrl>> expression =
      (ctx, company_ex, plant_ex) =>
        (from row in ctx.PlantConfCtrl
         where row.Company == company_ex &&
         row.Plant == plant_ex
         select row).FirstOrDefault();
                findFirstPlantConfCtrl4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlantConfCtrl4Query(this.Db, company, plant);
        }
        #endregion PlantConfCtrl Queries

        #region PrjRoleRt Queries

        static Func<ErpContext, string, string, string, bool> existsPrjRoleRtQuery;
        private bool ExistsPrjRoleRt(string company, string roleCd, string timeTypCd)
        {
            if (existsPrjRoleRtQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, roleCd_ex, timeTypCd_ex) =>
        (from row in ctx.PrjRoleRt
         where row.Company == company_ex &&
         row.RoleCd == roleCd_ex &&
         row.TimeTypCd == timeTypCd_ex
         select row).Any();
                existsPrjRoleRtQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPrjRoleRtQuery(this.Db, company, roleCd, timeTypCd);
        }
        #endregion PrjRoleRt Queries

        #region PRSyst

        static Func<ErpContext, string, bool> existsHCMEnabledQuery;
        private bool ExistsHCMEnabled(string company)
        {
            if (existsHCMEnabledQuery == null)
            {
                Expression<Func<ErpContext, string, bool>> expression =
      (ctx, company_ex) =>
        (from row in ctx.PRSyst
         where row.Company == company_ex &&
         row.HCMEnabled == true
         select row).Any();
                existsHCMEnabledQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsHCMEnabledQuery(this.Db, company);
        }

        #endregion PRSyst

        #region ProdCal Queries

        static Func<ErpContext, string, string, ProdCal> findFirstProdCalQuery;
        private ProdCal FindFirstProdCal(string company, string calendarID)
        {
            if (findFirstProdCalQuery == null)
            {
                Expression<Func<ErpContext, string, string, ProdCal>> expression =
      (ctx, company_ex, calendarID_ex) =>
        (from row in ctx.ProdCal
         where row.Company == company_ex &&
         row.CalendarID == calendarID_ex
         select row).FirstOrDefault();
                findFirstProdCalQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProdCalQuery(this.Db, company, calendarID);
        }


        static Func<ErpContext, string, string, ProdCal> findFirstProdCal2Query;
        private ProdCal FindFirstProdCal2(string company, string calendarID)
        {
            if (findFirstProdCal2Query == null)
            {
                Expression<Func<ErpContext, string, string, ProdCal>> expression =
      (ctx, company_ex, calendarID_ex) =>
        (from row in ctx.ProdCal
         where row.Company == company_ex &&
         row.CalendarID == calendarID_ex
         select row).FirstOrDefault();
                findFirstProdCal2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProdCal2Query(this.Db, company, calendarID);
        }
        #endregion ProdCal Queries

        #region ProdCalDay Queries

        static Func<ErpContext, string, string, DateTime, ProdCalDay> findFirstProdCalDayQuery;
        private ProdCalDay FindFirstProdCalDay(string company, string calendarID, DateTime modifiedDay)
        {
            if (findFirstProdCalDayQuery == null)
            {
                Expression<Func<ErpContext, string, string, DateTime, ProdCalDay>> expression =
      (ctx, company_ex, calendarID_ex, modifiedDay_ex) =>
        (from row in ctx.ProdCalDay
         where row.Company == company_ex &&
         row.CalendarID == calendarID_ex &&
         row.ModifiedDay == modifiedDay_ex
         select row).FirstOrDefault();
                findFirstProdCalDayQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProdCalDayQuery(this.Db, company, calendarID, modifiedDay);
        }
        #endregion ProdCalDay Queries

        #region Project Queries

        static Func<ErpContext, string, string, Project> findFirstProjectQuery;
        private Project FindFirstProject(string company, string projectID)
        {
            if (findFirstProjectQuery == null)
            {
                Expression<Func<ErpContext, string, string, Project>> expression =
      (ctx, company_ex, projectID_ex) =>
        (from row in ctx.Project
         where row.Company == company_ex &&
         row.ProjectID == projectID_ex
         select row).FirstOrDefault();
                findFirstProjectQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProjectQuery(this.Db, company, projectID);
        }


        static Func<ErpContext, string, string, Project> findFirstProject2Query;
        private Project FindFirstProject2(string company, string projectID)
        {
            if (findFirstProject2Query == null)
            {
                Expression<Func<ErpContext, string, string, Project>> expression =
      (ctx, company_ex, projectID_ex) =>
        (from row in ctx.Project
         where row.Company == company_ex &&
         row.ProjectID == projectID_ex
         select row).FirstOrDefault();
                findFirstProject2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProject2Query(this.Db, company, projectID);
        }


        static Func<ErpContext, string, string, Project> findFirstProject3Query;
        private Project FindFirstProject3(string company, string projectID)
        {
            if (findFirstProject3Query == null)
            {
                Expression<Func<ErpContext, string, string, Project>> expression =
      (ctx, company_ex, projectID_ex) =>
        (from row in ctx.Project
         where row.Company == company_ex &&
         row.ProjectID == projectID_ex
         select row).FirstOrDefault();
                findFirstProject3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProject3Query(this.Db, company, projectID);
        }


        static Func<ErpContext, string, string, Project> findFirstProject4Query;
        private Project FindFirstProject4(string company, string projectID)
        {
            if (findFirstProject4Query == null)
            {
                Expression<Func<ErpContext, string, string, Project>> expression =
      (ctx, company_ex, projectID_ex) =>
        (from row in ctx.Project
         where row.Company == company_ex &&
         row.ProjectID == projectID_ex
         select row).FirstOrDefault();
                findFirstProject4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProject4Query(this.Db, company, projectID);
        }


        static Func<ErpContext, string, string, Project> findFirstProject5Query;
        private Project FindFirstProject5(string company, string projectID)
        {
            if (findFirstProject5Query == null)
            {
                Expression<Func<ErpContext, string, string, Project>> expression =
      (ctx, company_ex, projectID_ex) =>
        (from row in ctx.Project
         where row.Company == company_ex &&
         row.ProjectID == projectID_ex
         select row).FirstOrDefault();
                findFirstProject5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProject5Query(this.Db, company, projectID);
        }

        static Func<ErpContext, string, string, bool> existsProjectActiveQuery;
        private bool ExistsActiveProject(string company, string projectID)
        {
            if (existsProjectActiveQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, projectID_ex) =>
        (from row in ctx.Project
         where row.Company == company_ex &&
         row.ProjectID == projectID_ex &&
         row.ActiveProject == true
         select row).Any();
                existsProjectActiveQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsProjectActiveQuery(this.Db, company, projectID);
        }

        private static Func<ErpContext, string, string, string> findFirstProjectDescQuery;
        private string FindFirstProjectDescription(string company, string projectID)
        {
            if (findFirstProjectDescQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
                    (context, company_ex, projectID_ex) =>
                    (from row in context.Project
                     where row.Company == company_ex &&
                     row.ProjectID == projectID_ex
                     select row.Description).FirstOrDefault();
                findFirstProjectDescQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProjectDescQuery(this.Db, company, projectID);
        }
        #endregion Project Queries

        #region ProjPhase Queries

        static Func<ErpContext, string, string, string, ProjPhase> findFirstProjPhaseQuery;
        private ProjPhase FindFirstProjPhase(string company, string projectID, string phaseID)
        {
            if (findFirstProjPhaseQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, ProjPhase>> expression =
      (ctx, company_ex, projectID_ex, phaseID_ex) =>
        (from row in ctx.ProjPhase
         where row.Company == company_ex &&
         row.ProjectID == projectID_ex &&
         row.PhaseID == phaseID_ex
         select row).FirstOrDefault();
                findFirstProjPhaseQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProjPhaseQuery(this.Db, company, projectID, phaseID);
        }


        static Func<ErpContext, string, string, ProjPhase> findFirstProjPhaseQuery_2;
        private ProjPhase FindFirstProjPhase(string company, string phaseID)
        {
            if (findFirstProjPhaseQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, ProjPhase>> expression =
      (ctx, company_ex, phaseID_ex) =>
        (from row in ctx.ProjPhase
         where row.Company == company_ex &&
         row.PhaseID == phaseID_ex
         select row).FirstOrDefault();
                findFirstProjPhaseQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProjPhaseQuery_2(this.Db, company, phaseID);
        }


        static Func<ErpContext, string, string, string, ProjPhase> findFirstProjPhase2Query;
        private ProjPhase FindFirstProjPhase2(string company, string projectID, string phaseID)
        {
            if (findFirstProjPhase2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, ProjPhase>> expression =
      (ctx, company_ex, projectID_ex, phaseID_ex) =>
        (from row in ctx.ProjPhase
         where row.Company == company_ex &&
         row.ProjectID == projectID_ex &&
         row.PhaseID == phaseID_ex
         select row).FirstOrDefault();
                findFirstProjPhase2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProjPhase2Query(this.Db, company, projectID, phaseID);
        }
        #endregion ProjPhase Queries

        #region QuickEntry Queries

        static Func<ErpContext, string, string, string, string, QuickEntry> findFirstQuickEntryQuery;
        private QuickEntry FindFirstQuickEntry(string company, string empID, string quickEntryType, string quickEntryCode)
        {
            if (findFirstQuickEntryQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, QuickEntry>> expression =
      (ctx, company_ex, empID_ex, quickEntryType_ex, quickEntryCode_ex) =>
        (from row in ctx.QuickEntry
         where row.Company == company_ex &&
         row.EmpID == empID_ex &&
         row.QuickEntryType == quickEntryType_ex &&
         row.QuickEntryCode == quickEntryCode_ex
         select row).FirstOrDefault();
                findFirstQuickEntryQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstQuickEntryQuery(this.Db, company, empID, quickEntryType, quickEntryCode);
        }
        #endregion QuickEntry Queries

        #region Reason Queries

        static Func<ErpContext, string, string, bool> existsScrapCodeQuery;
        private bool ExistsScrapCode(string company, string reasonCode)
        {
            if (existsScrapCodeQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, reasonCode_ex) =>
        (from row in ctx.Reason
         where row.Company == company_ex &&
         row.ReasonCode == reasonCode_ex &&
         row.ReasonType == "S" &&
         row.Scrap == true
         select row).Any();
                existsScrapCodeQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsScrapCodeQuery(this.Db, company, reasonCode);
        }


        static Func<ErpContext, string, string, string, Reason> findFirstReasonQuery;
        private Reason FindFirstReason(string company, string reasonType, string reasonCode)
        {
            if (findFirstReasonQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, Reason>> expression =
      (ctx, company_ex, reasonType_ex, reasonCode_ex) =>
        (from row in ctx.Reason
         where row.Company == company_ex &&
         row.ReasonType == reasonType_ex &&
         row.ReasonCode == reasonCode_ex
         select row).FirstOrDefault();
                findFirstReasonQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstReasonQuery(this.Db, company, reasonType, reasonCode);
        }


        static Func<ErpContext, string, string, string, Reason> findFirstReason2Query;
        private Reason FindFirstReason2(string company, string reasonType, string reasonCode)
        {
            if (findFirstReason2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, Reason>> expression =
      (ctx, company_ex, reasonType_ex, reasonCode_ex) =>
        (from row in ctx.Reason
         where row.Company == company_ex &&
         row.ReasonType == reasonType_ex &&
         row.ReasonCode == reasonCode_ex
         select row).FirstOrDefault();
                findFirstReason2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstReason2Query(this.Db, company, reasonType, reasonCode);
        }


        static Func<ErpContext, string, string, string, Reason> findFirstReason3Query;
        private Reason FindFirstReason3(string company, string reasonCode, string reasonType)
        {
            if (findFirstReason3Query == null)
            {
                Expression<Func<ErpContext, string, string, string, Reason>> expression =
      (ctx, company_ex, reasonCode_ex, reasonType_ex) =>
        (from row in ctx.Reason
         where row.Company == company_ex &&
         row.ReasonCode == reasonCode_ex &&
         row.ReasonType == reasonType_ex
         select row).FirstOrDefault();
                findFirstReason3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstReason3Query(this.Db, company, reasonCode, reasonType);
        }
        #endregion Reason Queries

        #region Resource Queries

        static Func<ErpContext, string, string, bool> existsResourceQuery;
        private bool ExistsResource(string company, string resourceID)
        {
            if (existsResourceQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, resourceID_ex) =>
        (from row in ctx.Resource
         where row.Company == company_ex &&
         row.ResourceID == resourceID_ex
         select row).Any();
                existsResourceQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsResourceQuery(this.Db, company, resourceID);
        }


        static Func<ErpContext, string, string, bool, Resource> findFirstResourceQuery_2;
        private Resource FindFirstResource(string company, string resourceGrpID, bool inactive)
        {
            if (findFirstResourceQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, bool, Resource>> expression =
      (ctx, company_ex, resourceGrpID_ex, inactive_ex) =>
        (from row in ctx.Resource
         where row.Company == company_ex &&
         row.ResourceGrpID == resourceGrpID_ex &&
         row.Inactive == inactive_ex
         select row).FirstOrDefault();
                findFirstResourceQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstResourceQuery_2(this.Db, company, resourceGrpID, inactive);
        }


        class ResourceResult
        {
            public decimal ProdLabRate { get; set; }
            public decimal SetupLabRate { get; set; }
        }

        static Func<ErpContext, string, string, string, ResourceResult> findFirstResource20Query;
        private ResourceResult FindFirstResource20(string company, string resourceGrpID, string resourceID)
        {
            if (findFirstResource20Query == null)
            {
                Expression<Func<ErpContext, string, string, string, ResourceResult>> expression =
      (ctx, company_ex, resourceGrpID_ex, resourceID_ex) =>
        (from row in ctx.Resource
         where row.Company == company_ex &&
         row.ResourceGrpID == resourceGrpID_ex &&
         row.ResourceID == resourceID_ex &&
         row.GetDefaultLaborFromGroup == false
         select new ResourceResult()
         {
             ProdLabRate = row.ProdLabRate,
             SetupLabRate = row.SetupLabRate
         }).FirstOrDefault();
                findFirstResource20Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstResource20Query(this.Db, company, resourceGrpID, resourceID);
        }

        private class ResourcePartialRow : Epicor.Data.TempRowBase
        {
            public bool AutoMove { get; set; }
        }

        private static Func<ErpContext, string, string, ResourcePartialRow> findFirstResourceQuery2;
        private ResourcePartialRow FindFirstResource20(string Company, string ResourceID)
        {
            if (findFirstResourceQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, ResourcePartialRow>> expression =
                    (context, Company_ex, ResourceID_ex) =>
                    (from row in context.Resource
                     where row.Company == Company_ex &&
                           row.ResourceID == ResourceID_ex
                     select new ResourcePartialRow { AutoMove = row.AutoMove })
                    .FirstOrDefault();
                findFirstResourceQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstResourceQuery2(this.Db, Company, ResourceID);
        }

        private class ResourceWhseBin : TempRowBase
        {
            public string OutputWhse { get; set; }
            public string OutputBinNum { get; set; }
            public string InputWhse { get; set; }
            public string InputBinNum { get; set; }
            public bool Location { get; set; }
        }

        private static Func<ErpContext, string, string, ResourceWhseBin> findFirstResourceWhseBinQuery;
        private ResourceWhseBin FindFirstResourceWhseBin(string company, string resourceID)
        {
            if (findFirstResourceWhseBinQuery == null)
            {
                Expression<Func<ErpContext, string, string, ResourceWhseBin>> expression =
                    (context, company_ex, resourceID_ex) =>
                    (from row in context.Resource
                     where row.Company == company_ex &&
                           row.ResourceID == resourceID_ex
                     select new ResourceWhseBin
                     {
                         OutputWhse = row.OutputWhse,
                         OutputBinNum = row.OutputBinNum,
                         InputWhse = row.InputWhse,
                         InputBinNum = row.InputBinNum,
                         Location = row.Location
                     }).FirstOrDefault();
                findFirstResourceWhseBinQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstResourceWhseBinQuery(this.Db, company, resourceID);
        }

        #endregion Resource Queries

        #region ResourceCal Queries

        static Func<ErpContext, string, string, string, DateTime, ResourceCal> findFirstResourceCalQuery;
        private ResourceCal FindFirstResourceCal(string company, string resourceID, string resourceGrpID, DateTime specialDay)
        {
            if (findFirstResourceCalQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, DateTime, ResourceCal>> expression =
      (ctx, company_ex, resourceID_ex, resourceGrpID_ex, specialDay_ex) =>
        (from row in ctx.ResourceCal
         where row.Company == company_ex &&
         row.ResourceID == resourceID_ex &&
         row.ResourceGrpID == resourceGrpID_ex &&
         row.SpecialDay == specialDay_ex
         select row).FirstOrDefault();
                findFirstResourceCalQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstResourceCalQuery(this.Db, company, resourceID, resourceGrpID, specialDay);
        }
        #endregion ResourceCal Queries

        #region ResourceGroup Queries

        static Func<ErpContext, string, string, string> findFirstResourceGroupJCDeptQuery;
        private string FindFirstResourceGroupJCDept(string company, string resourceGrpID)
        {
            if (findFirstResourceGroupJCDeptQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
      (ctx, company_ex, resourceGrpID_ex) =>
        (from row in ctx.ResourceGroup
         where row.Company == company_ex &&
         row.ResourceGrpID == resourceGrpID_ex
         select row.JCDept).FirstOrDefault();
                findFirstResourceGroupJCDeptQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstResourceGroupJCDeptQuery(this.Db, company, resourceGrpID);
        }


        static Func<ErpContext, string, string, string, ResourceGroup> findFirstResourceGroupQuery_2;
        private ResourceGroup FindFirstResourceGroup(string company, string plant, string resourceGrpID)
        {
            if (findFirstResourceGroupQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, ResourceGroup>> expression =
      (ctx, company_ex, plant_ex, resourceGrpID_ex) =>
        (from row in ctx.ResourceGroup
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.ResourceGrpID == resourceGrpID_ex
         select row).FirstOrDefault();
                findFirstResourceGroupQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstResourceGroupQuery_2(this.Db, company, plant, resourceGrpID);
        }

        private static Func<ErpContext, string, string, ResourceWhseBin> findFirstResourceGroupWhseBinQuery;
        private ResourceWhseBin FindFirstResourceGroupWhseBin(string company, string resourceGrpID)
        {
            if (findFirstResourceGroupWhseBinQuery == null)
            {
                Expression<Func<ErpContext, string, string, ResourceWhseBin>> expression =
                    (context, company_ex, resourceGrpID_ex) =>
                    (from row in context.ResourceGroup
                     where row.Company == company_ex &&
                           row.ResourceGrpID == resourceGrpID_ex
                     select new ResourceWhseBin
                     {
                         OutputWhse = row.OutputWhse,
                         OutputBinNum = row.OutputBinNum,
                         InputWhse = row.InputWhse,
                         InputBinNum = row.InputBinNum,
                         Location = row.Location
                     }).FirstOrDefault();
                findFirstResourceGroupWhseBinQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstResourceGroupWhseBinQuery(this.Db, company, resourceGrpID);
        }

        #endregion ResourceGroup Queries

        #region ResourceTimeUsed Queries

        static Func<ErpContext, string, string, int, int, int, bool, int, ResourceTimeUsed> findFirstResourceTimeUsedQuery;
        private ResourceTimeUsed FindFirstResourceTimeUsed(string company, string jobNum, int assemblySeq, int oprSeq, int opDtlSeq, bool whatIf, int allocNum)
        {
            if (findFirstResourceTimeUsedQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, bool, int, ResourceTimeUsed>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, opDtlSeq_ex, whatIf_ex, allocNum_ex) =>
        (from row in ctx.ResourceTimeUsed
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.OpDtlSeq == opDtlSeq_ex &&
         row.WhatIf == whatIf_ex &&
         row.AllocNum == allocNum_ex
         select row).FirstOrDefault();
                findFirstResourceTimeUsedQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstResourceTimeUsedQuery(this.Db, company, jobNum, assemblySeq, oprSeq, opDtlSeq, whatIf, allocNum);
        }

        private class ResourceTimeUsedPartialRow : Epicor.Data.TempRowBase
        {
            public string ResourceID { get; set; }
        }

        private static Func<ErpContext, string, string, int, int, int, bool, ResourceTimeUsedPartialRow> findFirstResourceTimeUsedQuery2;
        private ResourceTimeUsedPartialRow FindFirstResourceTimeUsed(string Company, string JobNum, int AssemblySeq, int OprSeq, int OpDtlSeq, bool WhatIf)
        {
            if (findFirstResourceTimeUsedQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, bool, ResourceTimeUsedPartialRow>> expression =
                    (context, Company_ex, JobNum_ex, AssemblySeq_ex, OprSeq_ex, OpDtlSeq_ex, WhatIf_ex) =>
                    (from row in context.ResourceTimeUsed
                     where row.Company == Company_ex &&
                           row.JobNum == JobNum_ex &&
                           row.AssemblySeq == AssemblySeq_ex &&
                           row.OprSeq == OprSeq_ex &&
                           row.OpDtlSeq == OpDtlSeq_ex &&
                           row.WhatIf == WhatIf_ex
                     select new ResourceTimeUsedPartialRow { ResourceID = row.ResourceID })
                    .FirstOrDefault();
                findFirstResourceTimeUsedQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstResourceTimeUsedQuery2(this.Db, Company, JobNum, AssemblySeq, OprSeq, OpDtlSeq, WhatIf);
        }

        #endregion ResourceTimeUsed Queries

        #region RoleCd Queries
        private class RoleCdResult
        {
            public string RoleCode { get; set; }
            public string RoleDescription { get; set; }
        }

        static Func<ErpContext, string, string, RoleCdResult> findFirstRoleCdQuery;
        private RoleCdResult FindFirstRoleCd(string company, string roleCode)
        {
            if (findFirstRoleCdQuery == null)
            {
                Expression<Func<ErpContext, string, string, RoleCdResult>> expression =
                (ctx, company_ex, roleCode_ex) =>
                (from row in ctx.RoleCd
                 where row.Company == company_ex &&
                 row.RoleCode == roleCode_ex
                 select new RoleCdResult
                 {
                     RoleCode = row.RoleCode,
                     RoleDescription = row.RoleDescription
                 }).FirstOrDefault();
                findFirstRoleCdQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstRoleCdQuery(this.Db, company, roleCode);
        }
        #endregion RoleCd Queries

        #region SaleAuth Queries

        static Func<ErpContext, string, string, string, bool> existsSaleAuthQuery;
        private bool ExistsSaleAuth(string company, string salesRepCode, string dcdUserID)
        {
            if (existsSaleAuthQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, salesRepCode_ex, dcdUserID_ex) =>
        (from row in ctx.SaleAuth
         where row.Company == company_ex &&
         row.SalesRepCode == salesRepCode_ex &&
         row.DcdUserID == dcdUserID_ex
         select row).Any();
                existsSaleAuthQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsSaleAuthQuery(this.Db, company, salesRepCode, dcdUserID);
        }
        #endregion SaleAuth Queries

        #region SalesRep Queries
        static Func<ErpContext, string, string, bool> isactiveWorkForceQuery;
        private bool IsactiveWorkForce(string company, string salesRepCode)
        {
            if (isactiveWorkForceQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, salesRepCode_ex) =>
        (from row in ctx.SalesRep
         where row.Company == company_ex &&
         row.SalesRepCode == salesRepCode_ex &&
         row.InActive == false
         select row).Any();
                isactiveWorkForceQuery = DBExpressionCompiler.Compile(expression);
            }

            return isactiveWorkForceQuery(this.Db, company, salesRepCode);
        }

        #endregion SalesRep Queries
        #region SerialMatch Queries

        static Func<ErpContext, string, string, string, bool> existsSerialMatchQuery;
        private bool ExistsSerialMatch(string company, string childPartNum, string childSerialNo)
        {
            if (existsSerialMatchQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, childPartNum_ex, childSerialNo_ex) =>
        (from row in ctx.SerialMatch
         where row.Company == company_ex &&
         row.ChildPartNum == childPartNum_ex &&
         row.ChildSerialNo == childSerialNo_ex
         select row).Any();
                existsSerialMatchQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsSerialMatchQuery(this.Db, company, childPartNum, childSerialNo);
        }
        #endregion SerialMatch Queries

        #region SerialNo Queries

        static Func<ErpContext, string, string, int, int, string, int, int, bool, int, int, string, int> countSerialNoNew;
        private int CountSerialNoNew(string company, string jobNum, int assemblySeq, int mtlSeq, string partNum, int attributeSetID, int lastOprSeq, bool scrapped, int scrapLaborHedSeq, int scrapLaborDtlSeq, string prevSNStatus)
        {
            if (countSerialNoNew == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, int, int, bool, int, int, string, int>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, partNum_ex, attributeSetID_ex, lastOprSeq_ex, scrapped_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex, prevSNStatus_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.PartNum == partNum_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.LastLbrOprSeq == lastOprSeq_ex &&
         row.Scrapped == scrapped_ex &&
         row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex &&
         row.PrevSNStatus == prevSNStatus_ex
         select row).Count();
                countSerialNoNew = DBExpressionCompiler.Compile(expression);
            }

            return countSerialNoNew(this.Db, company, jobNum, assemblySeq, mtlSeq, partNum, attributeSetID, lastOprSeq, scrapped, scrapLaborHedSeq, scrapLaborDtlSeq, prevSNStatus);
        }



        static Func<ErpContext, string, string, string, int, int, string, string, SerialNo> findFirstSerialNoQuery;
        private SerialNo FindFirstSerialNo(string company, string partNum, string jobNum, int assemblySeq, int mtlSeq, string partNum2, string serialNumber)
        {
            if (findFirstSerialNoQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, int, string, string, SerialNo>> expression =
      (ctx, company_ex, partNum_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, partNum2_ex, serialNumber_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.PartNum == partNum2_ex &&
         row.SerialNumber == serialNumber_ex
         select row).FirstOrDefault();
                findFirstSerialNoQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSerialNoQuery(this.Db, company, partNum, jobNum, assemblySeq, mtlSeq, partNum2, serialNumber);
        }


        static Func<ErpContext, string, string, string, SerialNo> findFirstSerialNoQuery_2;
        private SerialNo FindFirstSerialNo(string company, string partNum, string serialNumber)
        {
            if (findFirstSerialNoQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, SerialNo>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex
         select row).FirstOrDefault();
                findFirstSerialNoQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSerialNoQuery_2(this.Db, company, partNum, serialNumber);
        }


        static Func<ErpContext, string, string, int, int, string, int, int, string, SerialNo> findFirstSerialNoWithUpdLockQuery;
        private SerialNo FindFirstSerialNoWithUpdLock(string company, string jobNum, int assemblySeq, int mtlSeq, string partNum, int scrapLaborHedSeq, int scrapLaborDtlSeq, string serialNumber)
        {
            if (findFirstSerialNoWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, int, int, string, SerialNo>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, partNum_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex, serialNumber_ex) =>
        (from row in ctx.SerialNo.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.PartNum == partNum_ex &&
         row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex &&
         row.SerialNumber == serialNumber_ex
         select row).FirstOrDefault();
                findFirstSerialNoWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSerialNoWithUpdLockQuery(this.Db, company, jobNum, assemblySeq, mtlSeq, partNum, scrapLaborHedSeq, scrapLaborDtlSeq, serialNumber);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, int, int, string, SerialNo> findFirstSerialNoWithUpdLockQuery_2;
        private SerialNo FindFirstSerialNoWithUpdLock(string company, string jobNum, int assemblySeq, int mtlSeq, string serialNumber)
        {
            if (findFirstSerialNoWithUpdLockQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, SerialNo>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, serialNumber_ex) =>
        (from row in ctx.SerialNo.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.SerialNumber == serialNumber_ex
         select row).FirstOrDefault();
                findFirstSerialNoWithUpdLockQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSerialNoWithUpdLockQuery_2(this.Db, company, jobNum, assemblySeq, mtlSeq, serialNumber);
        }



        static Func<ErpContext, string, int, int, IEnumerable<SerialNo>> selectSerialNoQuery;
        private IEnumerable<SerialNo> SelectSerialNo(string company, int scrapLaborHedSeq, int scrapLaborDtlSeq)
        {
            if (selectSerialNoQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<SerialNo>>> expression =
      (ctx, company_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex
         select row);
                selectSerialNoQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectSerialNoQuery(this.Db, company, scrapLaborHedSeq, scrapLaborDtlSeq);
        }



        class SerialNoExpression2ColumnResult
        {
            public string Company { get; set; }
            public int ScrapLaborHedSeq { get; set; }
            public int ScrapLaborDtlSeq { get; set; }
            public string SerialNumber { get; set; }
            public string JobNum { get; set; }
            public int AssemblySeq { get; set; }
            public string SNStatus { get; set; }
        }
        static Func<ErpContext, string, string, int, int, string, int, int, int, IEnumerable<SerialNoExpression2ColumnResult>> selectSerialNoQuery_2;
        private IEnumerable<SerialNoExpression2ColumnResult> SelectSerialNo(string company, string jobNum, int assemblySeq, int mtlSeq, string partNum, int attributeSetID, int scrapLaborHedSeq, int scrapLaborDtlSeq)
        {
            if (selectSerialNoQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, int, int, int, IEnumerable<SerialNoExpression2ColumnResult>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, partNum_ex, attributeSetID_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.PartNum == partNum_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex
         select new SerialNoExpression2ColumnResult() { Company = row.Company, ScrapLaborHedSeq = row.ScrapLaborHedSeq, ScrapLaborDtlSeq = row.ScrapLaborDtlSeq, SerialNumber = row.SerialNumber, JobNum = row.JobNum, AssemblySeq = row.AssemblySeq, SNStatus = row.SNStatus });
                selectSerialNoQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return selectSerialNoQuery_2(this.Db, company, jobNum, assemblySeq, mtlSeq, partNum, attributeSetID, scrapLaborHedSeq, scrapLaborDtlSeq);
        }


        static Func<ErpContext, string, int, int, string, int, int, string, int, IEnumerable<SerialNo>> selectSerialNoQuery_3;
        private IEnumerable<SerialNo> SelectSerialNo(string company, int scrapLaborHedSeq, int scrapLaborDtlSeq, string jobNum, int assemblySeq, int mtlSeq, string partNum, int attributeSetID)
        {
            if (selectSerialNoQuery_3 == null)
            {
                Expression<Func<ErpContext, string, int, int, string, int, int, string, int, IEnumerable<SerialNo>>> expression =
      (ctx, company_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, partNum_ex, attributeSetID_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.PartNum == partNum_ex &&
         row.AttributeSetID == attributeSetID_ex
         select row);
                selectSerialNoQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return selectSerialNoQuery_3(this.Db, company, scrapLaborHedSeq, scrapLaborDtlSeq, jobNum, assemblySeq, mtlSeq, partNum, attributeSetID);
        }


        static Func<ErpContext, string, string, int, int, string, int, IEnumerable<SerialNo>> selectSerialNoQuery_4;
        private IEnumerable<SerialNo> SelectSerialNo(string company, string jobNum, int assemblySeq, int mtlSeq, string partNum, int attributeSetID)
        {
            if (selectSerialNoQuery_4 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, int, IEnumerable<SerialNo>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, partNum_ex, attributeSetID_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.PartNum == partNum_ex &&
         row.AttributeSetID == attributeSetID_ex
         select row);
                selectSerialNoQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return selectSerialNoQuery_4(this.Db, company, jobNum, assemblySeq, mtlSeq, partNum, attributeSetID);
        }

        static Func<ErpContext, string, int, int, int, string, bool> existsSerialNoRwQuery;
        private bool ExistsSerialNoRw(string company, int laborHedSeq, int laborDtlSeq, int oprSeq, string tranType)
        {
            if (existsSerialNoRwQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, string, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex, oprSeq_ex, tranType_ex) => ctx.SerialNo.Join(ctx.SNTran,
                                                                            serialno => new { serialno.Company, serialno.PartNum, serialno.SerialNumber, serialno.LastLbrOprSeq },
                                                                            sntran => new { sntran.Company, sntran.PartNum, sntran.SerialNumber, sntran.LastLbrOprSeq },
                                                                            (serialno, sntran) => new { SerialNo = serialno, SNTran = sntran })
                                                                            .Where(at => at.SerialNo.Company == company_ex &&
                                                                                   at.SerialNo.ScrapLaborHedSeq == laborHedSeq_ex &&
                                                                                   at.SerialNo.ScrapLaborDtlSeq == laborDtlSeq_ex &&
                                                                                   at.SNTran.LastLbrOprSeq == oprSeq_ex &&
                                                                                   at.SNTran.TranType == tranType_ex)
                                                                            .Any();
                existsSerialNoRwQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsSerialNoRwQuery(this.Db, company, laborHedSeq, laborDtlSeq, oprSeq, tranType);
        }


        #endregion SerialNo Queries

        #region ShopWrn Queries

        static Func<ErpContext, string, string, int, int, int, int, IEnumerable<ShopWrn>> selectShopWrnWithUpdLockQuery;
        private IEnumerable<ShopWrn> SelectShopWrnWithUpdLock(string company, string warnType, int laborHedSeq, int laborDtlSeq, int labWarnNum, int labWarnNum2)
        {
            if (selectShopWrnWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, int, IEnumerable<ShopWrn>>> expression =
      (ctx, company_ex, warnType_ex, laborHedSeq_ex, laborDtlSeq_ex, labWarnNum_ex, labWarnNum2_ex) =>
        (from row in ctx.ShopWrn.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.WarnType == warnType_ex &&
         row.LaborHedSeq == laborHedSeq_ex &&
         row.LaborDtlSeq == laborDtlSeq_ex &&
         row.LabWarnNum >= labWarnNum_ex &&
         row.LabWarnNum <= labWarnNum2_ex
         select row);
                selectShopWrnWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectShopWrnWithUpdLockQuery(this.Db, company, warnType, laborHedSeq, laborDtlSeq, labWarnNum, labWarnNum2);
        }
        #endregion ShopWrn Queries

        #region SNTran Queries
        static Func<ErpContext, string, string, string, string, int> countSNTranQuery;
        private int CountSNTran(string company, string partNum, string serialNumber, string tranType)
        {
            if (countSNTranQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, int>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranType_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex
         select row).Take(2).Count();
                countSNTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return countSNTranQuery(this.Db, company, partNum, serialNumber, tranType);
        }

        //This query has been optimized as much as possible and cannot be replaced with an Any, it is
        //querying based on a non-unique combination and returning true/false if there is only one row 
        //that matches the combination company/partnum/serialnumber/trantype
        private bool ExistsUniqueSNTran(string company, string partNum, string serialNumber, string tranType)
        {
            return CountSNTran(company, partNum, serialNumber, tranType) == 1;
        }


        static Func<ErpContext, string, string, string, string, string, int, int, int, int, int, bool> existsSNTranQuery;
        private bool ExistsSNTran(string company, string tranType, string partNum, string serialNumber, string jobNum, int assemblySeq, int mtlSeq, int lastLbrOprSeq, int scrapLaborHedSeq, int scrapLaborDtlSeq)
        {
            if (existsSNTranQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, int, int, int, int, int, bool>> expression =
      (ctx, company_ex, tranType_ex, partNum_ex, serialNumber_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, lastLbrOprSeq_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.TranType == tranType_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex &&
        (row.ScrapLaborHedSeq != scrapLaborHedSeq_ex ||
         row.ScrapLaborDtlSeq != scrapLaborDtlSeq_ex)
         select row).Any();
                existsSNTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsSNTranQuery(this.Db, company, tranType, partNum, serialNumber, jobNum, assemblySeq, mtlSeq, lastLbrOprSeq, scrapLaborHedSeq, scrapLaborDtlSeq);
        }


        static Func<ErpContext, string, string, int, int, string, string, string, int, int, int, bool> existsSNTranQuery_2;
        private bool ExistsSNTran(string company, string jobNum, int assemblySeq, int mtlSeq, string partNum, string serialNumber, string tranType, int lastLbrOprSeq, int scrapLaborHedSeq, int scrapLaborDtlSeq)
        {
            if (existsSNTranQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, string, string, int, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, partNum_ex, serialNumber_ex, tranType_ex, lastLbrOprSeq_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex &&
        (row.ScrapLaborHedSeq != scrapLaborHedSeq_ex ||
         row.ScrapLaborDtlSeq != scrapLaborDtlSeq_ex)
         select row).Any();
                existsSNTranQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return existsSNTranQuery_2(this.Db, company, jobNum, assemblySeq, mtlSeq, partNum, serialNumber, tranType, lastLbrOprSeq, scrapLaborHedSeq, scrapLaborDtlSeq);
        }


        static Func<ErpContext, string, string, string, string, string, int, int, int, bool> existsSNTranQuery_3;
        private bool ExistsSNTran(string company, string tranType, string partNum, string serialNumber, string jobNum, int assemblySeq, int mtlSeq, int lastLbrOprSeq)
        {
            if (existsSNTranQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, int, int, int, bool>> expression =
      (ctx, company_ex, tranType_ex, partNum_ex, serialNumber_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, lastLbrOprSeq_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.TranType == tranType_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex
         select row).Any();
                existsSNTranQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return existsSNTranQuery_3(this.Db, company, tranType, partNum, serialNumber, jobNum, assemblySeq, mtlSeq, lastLbrOprSeq);
        }


        static Func<ErpContext, string, string, int, int, string, string, string, int, bool> existsSNTranQuery_4;
        private bool ExistsSNTran(string company, string jobNum, int assemblySeq, int mtlSeq, string partNum, string serialNumber, string tranType, int lastLbrOprSeq)
        {
            if (existsSNTranQuery_4 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, string, string, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, partNum_ex, serialNumber_ex, tranType_ex, lastLbrOprSeq_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex
         select row).Any();
                existsSNTranQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return existsSNTranQuery_4(this.Db, company, jobNum, assemblySeq, mtlSeq, partNum, serialNumber, tranType, lastLbrOprSeq);
        }


        static Func<ErpContext, string, string, string, int, int, string, string, int, int, bool> existsSNTranQuery_5;
        private bool ExistsSNTran(string company, string partNum, string jobNum, int assemblySeq, int mtlSeq, string serialNumber, string tranType, int scrapLaborHedSeq, int scrapLaborDtlSeq)
        {
            if (existsSNTranQuery_5 == null)
            {
                Expression<Func<ErpContext, string, string, string, int, int, string, string, int, int, bool>> expression =
      (ctx, company_ex, partNum_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, serialNumber_ex, tranType_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex &&
        (row.ScrapLaborHedSeq != scrapLaborHedSeq_ex ||
         row.ScrapLaborDtlSeq != scrapLaborDtlSeq_ex)
         select row).Any();
                existsSNTranQuery_5 = DBExpressionCompiler.Compile(expression);
            }

            return existsSNTranQuery_5(this.Db, company, partNum, jobNum, assemblySeq, mtlSeq, serialNumber, tranType, scrapLaborHedSeq, scrapLaborDtlSeq);
        }


        static Func<ErpContext, string, string, int, int, string, string, string, int, int, int, bool> existsSNTran2Query;
        private bool ExistsSNTran2(string company, string jobNum, int assemblySeq, int mtlSeq, string partNum, string serialNumber, string tranType, int lastLbrOprSeq, int scrapLaborHedSeq, int scrapLaborDtlSeq)
        {
            if (existsSNTran2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, string, string, int, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, partNum_ex, serialNumber_ex, tranType_ex, lastLbrOprSeq_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex &&
        (row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex)
         select row).Any();
                existsSNTran2Query = DBExpressionCompiler.Compile(expression);
            }

            return existsSNTran2Query(this.Db, company, jobNum, assemblySeq, mtlSeq, partNum, serialNumber, tranType, lastLbrOprSeq, scrapLaborHedSeq, scrapLaborDtlSeq);
        }


        static Func<ErpContext, string, string, string, int, int, string, string, int, int, bool> existsSNTran2Query_2;
        private bool ExistsSNTran2(string company, string partNum, string jobNum, int assemblySeq, int mtlSeq, string serialNumber, string tranType, int scrapLaborHedSeq, int scrapLaborDtlSeq)
        {
            if (existsSNTran2Query_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, int, int, string, string, int, int, bool>> expression =
      (ctx, company_ex, partNum_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, serialNumber_ex, tranType_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex &&
        (row.ScrapLaborHedSeq != scrapLaborHedSeq_ex ||
         row.ScrapLaborDtlSeq != scrapLaborDtlSeq_ex)
         select row).Any();
                existsSNTran2Query_2 = DBExpressionCompiler.Compile(expression);
            }

            return existsSNTran2Query_2(this.Db, company, partNum, jobNum, assemblySeq, mtlSeq, serialNumber, tranType, scrapLaborHedSeq, scrapLaborDtlSeq);
        }


        static Func<ErpContext, string, string, string, int, int, string, string, int, int, bool> existsSNTran3Query;
        private bool ExistsSNTran3(string company, string partNum, string jobNum, int assemblySeq, int mtlSeq, string serialNumber, string tranType, int scrapLaborHedSeq, int scrapLaborDtlSeq)
        {
            if (existsSNTran3Query == null)
            {
                Expression<Func<ErpContext, string, string, string, int, int, string, string, int, int, bool>> expression =
      (ctx, company_ex, partNum_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, serialNumber_ex, tranType_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex &&
         row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex
         select row).Any();
                existsSNTran3Query = DBExpressionCompiler.Compile(expression);
            }

            return existsSNTran3Query(this.Db, company, partNum, jobNum, assemblySeq, mtlSeq, serialNumber, tranType, scrapLaborHedSeq, scrapLaborDtlSeq);
        }



        static Func<ErpContext, string, string, string, string, int, SNTran> findFirstSNTranQuery;
        private SNTran FindFirstSNTran(string company, string partNum, string serialNumber, string tranType, int lastLbrOprSeq)
        {
            if (findFirstSNTranQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, int, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranType_ex, lastLbrOprSeq_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex
         select row).FirstOrDefault();
                findFirstSNTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSNTranQuery(this.Db, company, partNum, serialNumber, tranType, lastLbrOprSeq);
        }


        //HOLDLOCK



        static Func<ErpContext, string, string, string, int, int, int, string, SNTran> findFirstSNTranWithUpdLockQuery;
        private SNTran FindFirstSNTranWithUpdLock(string company, string partNum, string serialNumber, int scrapLaborHedSeq, int scrapLaborDtlSeq, int lastLbrOprSeq, string tranType)
        {
            if (findFirstSNTranWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, int, int, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex, lastLbrOprSeq_ex, tranType_ex) =>
        (from row in ctx.SNTran.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex &&
         row.TranType == tranType_ex
         select row).FirstOrDefault();
                findFirstSNTranWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSNTranWithUpdLockQuery(this.Db, company, partNum, serialNumber, scrapLaborHedSeq, scrapLaborDtlSeq, lastLbrOprSeq, tranType);
        }

        static Func<ErpContext, Guid, SNTran> findFirstSNTranWithUpdLock2Query;
        private SNTran FindFirstSNTranWithUpdLock2(Guid sysRowID)
        {
            if (findFirstSNTranWithUpdLock2Query == null)
            {
                Expression<Func<ErpContext, Guid, SNTran>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.SNTran.With(LockHint.UpdLock)
         where row.SysRowID == sysRowID_ex
         select row).FirstOrDefault();
                findFirstSNTranWithUpdLock2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSNTranWithUpdLock2Query(this.Db, sysRowID);
        }
        //HOLDLOCK



        static Func<ErpContext, string, string, string, SNTran> findLastSNTranQuery;
        private SNTran FindLastSNTran(string company, string partNum, string serialNumber)
        {
            if (findLastSNTranQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType != "OPR-RWK"
         select row).LastOrDefault();
                findLastSNTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTranQuery(this.Db, company, partNum, serialNumber);
        }


        static Func<ErpContext, string, string, string, string, SNTran> findLastSNTranQuery_2;
        private SNTran FindLastSNTran(string company, string partNum, string serialNumber, string tranType)
        {
            if (findLastSNTranQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranType_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex
         select row).LastOrDefault();
                findLastSNTranQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTranQuery_2(this.Db, company, partNum, serialNumber, tranType);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, string, string, SNTran> findLastSNTran2Query;
        private SNTran FindLastSNTran2(string company, string partNum, string serialNumber, string tranType)
        {
            if (findLastSNTran2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranType_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex
         select row).LastOrDefault();
                findLastSNTran2Query = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTran2Query(this.Db, company, partNum, serialNumber, tranType);
        }


        static Func<ErpContext, string, string, string, string, SNTran> findLastSNTran3Query;
        private SNTran FindLastSNTran3(string company, string partNum, string serialNumber, string tranType)
        {
            if (findLastSNTran3Query == null)
            {
                Expression<Func<ErpContext, string, string, string, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranType_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex
         select row).LastOrDefault();
                findLastSNTran3Query = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTran3Query(this.Db, company, partNum, serialNumber, tranType);
        }


        static Func<ErpContext, string, string, string, SNTran> findLastSNTranWithUpdLockQuery;
        private SNTran FindLastSNTranWithUpdLock(string company, string partNum, string serialNumber)
        {
            if (findLastSNTranWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex) =>
        (from row in ctx.SNTran.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType != "OPR-RWK"
         select row).LastOrDefault();
                findLastSNTranWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTranWithUpdLockQuery(this.Db, company, partNum, serialNumber);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, string, string, int, int, SNTran> findLastSNTranWithUpdLockQuery_2;
        private SNTran FindLastSNTranWithUpdLock(string company, string partNum, string serialNumber, string tranType, int scrapLaborHedSeq, int scrapLaborDtlSeq)
        {
            if (findLastSNTranWithUpdLockQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, int, int, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranType_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex) =>
        (from row in ctx.SNTran.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex &&
         row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex
         select row).LastOrDefault();
                findLastSNTranWithUpdLockQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTranWithUpdLockQuery_2(this.Db, company, partNum, serialNumber, tranType, scrapLaborHedSeq, scrapLaborDtlSeq);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, string, string, SNTran> findLastSNTranWithUpdLockQuery_3;
        private SNTran FindLastSNTranWithUpdLock(string company, string partNum, string serialNumber, string tranType)
        {
            if (findLastSNTranWithUpdLockQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranType_ex) =>
        (from row in ctx.SNTran.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex
         select row).LastOrDefault();
                findLastSNTranWithUpdLockQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTranWithUpdLockQuery_3(this.Db, company, partNum, serialNumber, tranType);
        }


        static Func<ErpContext, string, string, string, int, string, SNTran> findLastSNTranWithUpdLockQuery_4;
        private SNTran FindLastSNTranWithUpdLock(string company, string partNum, string serialNumber, int lastLbrOprSeq, string tranType)
        {
            if (findLastSNTranWithUpdLockQuery_4 == null)
            {
                Expression<Func<ErpContext, string, string, string, int, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, lastLbrOprSeq_ex, tranType_ex) =>
        (from row in ctx.SNTran.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex &&
         row.TranType == tranType_ex
         select row).LastOrDefault();
                findLastSNTranWithUpdLockQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTranWithUpdLockQuery_4(this.Db, company, partNum, serialNumber, lastLbrOprSeq, tranType);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, string, string, SNTran> findLastSNTranWithUpdLock2Query;
        private SNTran FindLastSNTranWithUpdLock2(string company, string partNum, string serialNumber, string tranType)
        {
            if (findLastSNTranWithUpdLock2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranType_ex) =>
        (from row in ctx.SNTran.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex
         select row).LastOrDefault();
                findLastSNTranWithUpdLock2Query = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTranWithUpdLock2Query(this.Db, company, partNum, serialNumber, tranType);
        }

        //HOLDLOCK



        static Func<ErpContext, string, string, string, SNTran> findLastSNTranWithUpdLock3Query;
        private SNTran FindLastSNTranWithUpdLock3(string company, string partNum, string serialNumber)
        {
            if (findLastSNTranWithUpdLock3Query == null)
            {
                Expression<Func<ErpContext, string, string, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex) =>
        (from row in ctx.SNTran.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex
         select row).LastOrDefault();
                findLastSNTranWithUpdLock3Query = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTranWithUpdLock3Query(this.Db, company, partNum, serialNumber);
        }

        static Func<ErpContext, string, string, string, int, string, string, SNTran> findLastSNTranWithUpdLock5Query;
        private SNTran FindLastSNTranWithUpdLock5(string company, string partNum, string serialNumber, int lastLbrOprSeq, string tranType, string jobNum)
        {
            if (findLastSNTranWithUpdLock5Query == null)
            {
                Expression<Func<ErpContext, string, string, string, int, string, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, lastLbrOprSeq_ex, tranType_ex, jobNum_ex) =>
        (from row in ctx.SNTran.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex &&
         row.TranType == tranType_ex &&
         row.JobNum == jobNum_ex
         select row).LastOrDefault();
                findLastSNTranWithUpdLock5Query = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTranWithUpdLock5Query(this.Db, company, partNum, serialNumber, lastLbrOprSeq, tranType, jobNum);
        }



        class SNTranExpression3ColumnResult
        {
            public string Company { get; set; }
            public int ScrapLaborHedSeq { get; set; }
            public int ScrapLaborDtlSeq { get; set; }
            public string SerialNumber { get; set; }
        }
        static Func<ErpContext, string, string, string, int, int, int, IEnumerable<SNTranExpression3ColumnResult>> selectSNTranQuery;
        private IEnumerable<SNTranExpression3ColumnResult> SelectSNTran(string company, string tranType, string partNum, int scrapLaborHedSeq, int scrapLaborDtlSeq, int lastLbrOprSeq)
        {
            if (selectSNTranQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, int, int, IEnumerable<SNTranExpression3ColumnResult>>> expression =
      (ctx, company_ex, tranType_ex, partNum_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex, lastLbrOprSeq_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.TranType == tranType_ex &&
         row.PartNum == partNum_ex &&
         row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex
         select new SNTranExpression3ColumnResult() { Company = row.Company, ScrapLaborHedSeq = row.ScrapLaborHedSeq, ScrapLaborDtlSeq = row.ScrapLaborDtlSeq, SerialNumber = row.SerialNumber });
                selectSNTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectSNTranQuery(this.Db, company, tranType, partNum, scrapLaborHedSeq, scrapLaborDtlSeq, lastLbrOprSeq);
        }


        static Func<ErpContext, string, int, int, string, int, IEnumerable<SNTran>> selectSNTranQuery_2;
        private IEnumerable<SNTran> SelectSNTran(string company, int scrapLaborHedSeq, int scrapLaborDtlSeq, string tranType, int lastLbrOprSeq)
        {
            if (selectSNTranQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, int, string, int, IEnumerable<SNTran>>> expression =
      (ctx, company_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex, tranType_ex, lastLbrOprSeq_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex &&
         row.TranType == tranType_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex
         select row);
                selectSNTranQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return selectSNTranQuery_2(this.Db, company, scrapLaborHedSeq, scrapLaborDtlSeq, tranType, lastLbrOprSeq);
        }

        static Func<ErpContext, string, int, int, string, int, IEnumerable<SNTran>> selectSNTranWithUpdLockQuery_2;
        private IEnumerable<SNTran> SelectSNTranWithUpdLock(string company, int scrapLaborHedSeq, int scrapLaborDtlSeq, string tranType, int lastLbrOprSeq)
        {
            if (selectSNTranWithUpdLockQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, int, string, int, IEnumerable<SNTran>>> expression =
      (ctx, company_ex, scrapLaborHedSeq_ex, scrapLaborDtlSeq_ex, tranType_ex, lastLbrOprSeq_ex) =>
        (from row in ctx.SNTran.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.ScrapLaborHedSeq == scrapLaborHedSeq_ex &&
         row.ScrapLaborDtlSeq == scrapLaborDtlSeq_ex &&
         row.TranType == tranType_ex &&
         row.LastLbrOprSeq == lastLbrOprSeq_ex
         select row);
                selectSNTranWithUpdLockQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return selectSNTranWithUpdLockQuery_2(this.Db, company, scrapLaborHedSeq, scrapLaborDtlSeq, tranType, lastLbrOprSeq);
        }

        static Func<ErpContext, string, int, int, int, string, bool> existsSNTranRwQuery;
        private bool ExistsSNTranRw(string company, int laborHedSeq, int laborDtlSeq, int oprSeq, string tranType)
        {
            if (existsSNTranRwQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, string, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex, oprSeq_ex, tranType_ex) => ctx.SNTran.Join(ctx.SNTran,
                                                                            snTran => new { snTran.Company, snTran.JobNum, snTran.PartNum, snTran.SerialNumber },
                                                                            snTranRw => new { snTranRw.Company, snTranRw.JobNum, snTranRw.PartNum, snTranRw.SerialNumber },
                                                                            (snTran, snTranRw) => new { SNTran = snTran, SNTranRw = snTranRw })
                                                                            .Where(row => row.SNTran.Company == company_ex &&
                                                                                   row.SNTran.ScrapLaborHedSeq == laborHedSeq_ex &&
                                                                                   row.SNTran.ScrapLaborDtlSeq == laborDtlSeq_ex &&
                                                                                   row.SNTranRw.LastLbrOprSeq == oprSeq_ex &&
                                                                                   row.SNTran.TranType == tranType_ex)
                                                                            .Any();
                existsSNTranRwQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsSNTranRwQuery(this.Db, company, laborHedSeq, laborDtlSeq, oprSeq, tranType);
        }

        #endregion SNTran Queries

        // ***********************************************************************************************

        static Func<ErpContext, string, int, int, bool> existsJobPartChangesQuery;
        private bool ExistsJobPartChanges(string company, int laborHedSeq, int laborDtlSeq)
        {
            if (existsJobPartChangesQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, bool>> expression =
      (ctx, company_ex, laborHedSeq_ex, laborDtlSeq_ex) =>
         (from tmpLaborPart in ctx.LaborPart
          where tmpLaborPart.Company == company_ex &&
          tmpLaborPart.LaborHedSeq == laborHedSeq_ex &&
          tmpLaborPart.LaborDtlSeq == laborDtlSeq_ex
          join tmpJobPart in ctx.JobPart on tmpLaborPart.PartNum equals tmpJobPart.PartNum into joinLabors
          from newJobPartRow in joinLabors.DefaultIfEmpty()
          where newJobPartRow.PartNum == null
          select tmpLaborPart).Any();
                existsJobPartChangesQuery = DBExpressionCompiler.Compile(expression);
            }
            return existsJobPartChangesQuery(this.Db, company, laborHedSeq, laborDtlSeq);
        }

        // ***********************************************************************************************

        #region Task Queries

        private class TaskResult
        {
            public string SalesRepCode { get; set; }
        }
        static Func<ErpContext, string, string, string, string, bool, string, IEnumerable<TaskResult>> selectTaskQuery;
        private IEnumerable<TaskResult> SelectTask(string company, string relatedToFile, string key1, string key2, bool complete, string completionAction)
        {
            if (selectTaskQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, bool, string, IEnumerable<TaskResult>>> expression =
                (ctx, company_ex, relatedToFile_ex, key1_ex, key2_ex, complete_ex, completionAction_ex) =>
                (from row in ctx.Task
                 where row.Company == company_ex &&
                 row.RelatedToFile == relatedToFile_ex &&
                 row.Key1 == key1_ex &&
                 row.Key2 == key2_ex &&
                 row.Complete == complete_ex &&
                 row.CompletionAction == completionAction_ex
                 select new TaskResult
                 {
                     SalesRepCode = row.SalesRepCode
                 });
                selectTaskQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectTaskQuery(this.Db, company, relatedToFile, key1, key2, complete, completionAction);
        }
        #endregion Task Queries

        #region TemplateId Queries

        static Func<ErpContext, string, string, int, int, string> findJobOperTemplateIdQuery;
        private string FindJobOperTemplateId(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findJobOperTemplateIdQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row.TemplateID).FirstOrDefault();
                findJobOperTemplateIdQuery = DBExpressionCompiler.Compile(expression);
            }

            return findJobOperTemplateIdQuery(this.Db, company, jobNum, assemblySeq, oprSeq);
        }
        #endregion TemplateId Queries


        #region TimeTypCd Queries

        static Func<ErpContext, string, string, TimeTypCd> findFirstTimeTypCdQuery;
        private TimeTypCd FindFirstTimeTypCd(string company, string timeTypCd1)
        {
            if (findFirstTimeTypCdQuery == null)
            {
                Expression<Func<ErpContext, string, string, TimeTypCd>> expression =
      (ctx, company_ex, timeTypCd1_ex) =>
        (from row in ctx.TimeTypCd
         where row.Company == company_ex &&
         row.TimeTypCd1 == timeTypCd1_ex
         select row).FirstOrDefault();
                findFirstTimeTypCdQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTimeTypCdQuery(this.Db, company, timeTypCd1);
        }
        #endregion TimeTypCd Queries

        #region UserFile Queries

        static Func<ErpContext, string, UserFile> findFirstUserFileQuery;
        private UserFile FindFirstUserFile(string dcdUserID)
        {
            if (findFirstUserFileQuery == null)
            {
                Expression<Func<ErpContext, string, UserFile>> expression =
      (ctx, dcdUserID_ex) =>
        (from row in ctx.UserFile
         where row.DcdUserID == dcdUserID_ex
         select row).FirstOrDefault();
                findFirstUserFileQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstUserFileQuery(this.Db, dcdUserID);
        }

        static Func<ErpContext, string, UserFile> findFirstUserFileUpdQuery;
        private UserFile FindFirstUserFileUpd(string dcdUserID)
        {
            if (findFirstUserFileUpdQuery == null)
            {
                Expression<Func<ErpContext, string, UserFile>> expression =
      (ctx, dcdUserID_ex) =>
        (from row in ctx.UserFile.With(LockHint.UpdLock)
         where row.DcdUserID == dcdUserID_ex
         select row).FirstOrDefault();
                findFirstUserFileUpdQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstUserFileUpdQuery(this.Db, dcdUserID);
        }



        static Func<ErpContext, string, UserFile> findFirstUserFileWithUpdLockQuery;
        private UserFile FindFirstUserFileWithUpdLock(string dcdUserID)
        {
            if (findFirstUserFileWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, UserFile>> expression =
      (ctx, dcdUserID_ex) =>
        (from row in ctx.UserFile.With(LockHint.UpdLock)
         where row.DcdUserID == dcdUserID_ex
         select row).FirstOrDefault();
                findFirstUserFileWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstUserFileWithUpdLockQuery(this.Db, dcdUserID);
        }
        #endregion UserFile Queries

        #region userComp Queries

        static Func<ErpContext, string, string, bool, bool> canUserUpdateTimeQuery;
        private bool CanUserUpdateTime(string company, string userID, bool canUpdateTime)
        {
            if (canUserUpdateTimeQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
                    (ctx, company_ex, userID_ex, canUpdateTime_ex) => ctx.UserComp.Where(at => at.Company == company_ex
                                                                          && at.DcdUserID == userID_ex
                                                                          && at.CanUpdateTime == canUpdateTime_ex).Any();

                canUserUpdateTimeQuery = DBExpressionCompiler.Compile(expression);
            }

            return canUserUpdateTimeQuery(this.Db, company, userID, canUpdateTime);
        }
        #endregion

        #region HCM Integration queries
        static Func<ErpContext, string, bool, bool> isHCMEnabledAtCompanyQuery;
        private bool IsHCMEnabledAtCompany(string company, bool hcmenabled)
        {
            if (isHCMEnabledAtCompanyQuery == null)
            {
                Expression<Func<ErpContext, string, bool, bool>> expression =
                (ctx, company_ex, hcmenabled_ex) => ctx.PRSyst.Where(at => at.Company == company_ex
                                                              && at.HCMEnabled == hcmenabled_ex).Any();
                isHCMEnabledAtCompanyQuery = DBExpressionCompiler.Compile(expression);
            }

            return isHCMEnabledAtCompanyQuery(this.Db, company, hcmenabled);
        }

        static Func<ErpContext, string, string, bool, bool> isHCMEnabledAtSiteQuery;
        private bool IsHCMEnabledByEmployee(string company, string siteID, bool hcmenabled)
        {
            if (isHCMEnabledAtSiteQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
                (ctx, company_ex, siteID_ex, hcmenabled_ex) => ctx.PlantConfCtrl.Where(at => at.Company == company_ex
                                                                                          && at.CanEmployeeOverrideHCM == hcmenabled_ex
                                                                                          && at.Plant == siteID_ex).Any();
                isHCMEnabledAtSiteQuery = DBExpressionCompiler.Compile(expression);
            }

            return isHCMEnabledAtSiteQuery(this.Db, company, siteID, hcmenabled);
        }

        static Func<ErpContext, string, string, string> getHCMValueAtSiteQuery;
        private string GetHCMValueAtSite(string company, string siteID)
        {
            if (getHCMValueAtSiteQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
                (ctx, company_ex, siteID_ex) => ctx.PlantConfCtrl.Where(at => at.Company == company_ex
                                                                                          && at.Plant == siteID_ex).Select(row => row.PayrollValuesForHCM).FirstOrDefault();
                getHCMValueAtSiteQuery = DBExpressionCompiler.Compile(expression);
            }

            return getHCMValueAtSiteQuery(this.Db, company, siteID);
        }

        static Func<ErpContext, string, string, string> getHCMValueAtEmplQuery;
        private string GetHCMValueAtEmpl(string company, string empID)
        {
            if (getHCMValueAtEmplQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
                (ctx, company_ex, empID_ex) => ctx.EmpBasic.Where(at => at.Company == company_ex
                                                                        && at.EmpID == empID_ex).Select(row => row.PayrollValuesForHCM).FirstOrDefault();
                getHCMValueAtEmplQuery = DBExpressionCompiler.Compile(expression);
            }

            return getHCMValueAtEmplQuery(this.Db, company, empID);
        }


        /// <summary>
        /// LaborHCMRecord
        /// </summary>
        class LaborHCMRecord
        {
            public string Company { get; set; }
            public string EmployeeNum { get; set; }
            public int LaborHedSeq { get; set; }
            public int LaborDtlSeq { get; set; }
            public string LaborTypePseudo { get; set; }
            public string DspClockInTime { get; set; }
            public string DspClockOutTime { get; set; }
            public decimal PayHours { get; set; }
            public string LaborNote { get; set; }
            public string JCDept { get; set; }
            public int Shift { get; set; }
            public string ProjectID { get; set; }
            public string HCMEnabledAt { get; set; }
            public Guid SysRowID { get; set; }
            public DateTime? ClockInDate { get; set; }

            public HCMLaborDtlSync HCMLaborDtlSync { get; set; }
            public string HCMPayHoursCalcType { get; set; }
        }

        static Func<ErpContext, string, string, DateTime?, DateTime?, string, IEnumerable<LaborHCMRecord>> selectLaborDtlJoinQuery;
        private IEnumerable<LaborHCMRecord> SelectLaborDtlJoin(string company, string employeeNum, DateTime? startClockInDate, DateTime? endClockInDate, string timeStatus)
        {
            if (selectLaborDtlJoinQuery == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, DateTime?, string, IEnumerable<LaborHCMRecord>>> expression =
                    (ctx, company_ex, employeeNum_ex, startClockInDate_ex, endClockInDate_ex, timeStatus_ex) =>
                     ctx.LaborDtl
                                .GroupJoin(ctx.HCMLaborDtlSync,
                                     lbdtl => new { lbdtl.Company, RowID = lbdtl.SysRowID },
                                     hcmsyn => new { hcmsyn.Company, RowID = hcmsyn.LaborDtlSysRowID },
                                     (lbdtl, hcmsyn) => new { LaborDtl = lbdtl, HCMLaborDtlSync = hcmsyn })
                                .SelectMany(lbhd => lbhd.HCMLaborDtlSync.DefaultIfEmpty(),
                                     (lbdtl, hcmsyn) => new { LaborDtl = lbdtl.LaborDtl, HCMLaborDtlSync = hcmsyn })
                                .Join(ctx.EmpBasic,
                                      lbdtl => new { lbdtl.LaborDtl.Company, EmpID = lbdtl.LaborDtl.EmployeeNum },
                                      emp => new { emp.Company, emp.EmpID },
                                      (lbdtl, emp) => new { Labor = lbdtl, Employee = emp })
                                .Where(at => at.Labor.LaborDtl.Company == company_ex
                                         && at.Labor.LaborDtl.EmployeeNum == employeeNum_ex
                                         && at.Labor.LaborDtl.TimeStatus == timeStatus_ex
                                         && at.Labor.LaborDtl.ClockInDate.Value >= startClockInDate_ex.Value
                                         && at.Labor.LaborDtl.ClockInDate.Value <= endClockInDate_ex.Value
                                         && at.Labor.LaborDtl.HCMPayHours > 0)
                                //&& at.Employee.Payroll == payroll_ex*/) Change to LaborHed.FeedPayroll = payroll_ex
                                .Select(row => new LaborHCMRecord
                                {
                                    Company = row.Labor.LaborDtl.Company,
                                    EmployeeNum = row.Employee.EmpID,
                                    LaborHedSeq = row.Labor.LaborDtl.LaborHedSeq,
                                    LaborDtlSeq = row.Labor.LaborDtl.LaborDtlSeq,
                                    LaborNote = row.Labor.LaborDtl.LaborNote,
                                    LaborTypePseudo = row.Labor.LaborDtl.LaborTypePseudo,
                                    ClockInDate = row.Labor.LaborDtl.ClockInDate,
                                    DspClockInTime = row.Labor.LaborDtl.DspClockInTime,
                                    DspClockOutTime = row.Labor.LaborDtl.DspClockOutTime,
                                    PayHours = row.Labor.LaborDtl.HCMPayHours,
                                    JCDept = row.Labor.LaborDtl.JCDept,
                                    Shift = row.Labor.LaborDtl.Shift,
                                    ProjectID = row.Labor.LaborDtl.ProjectID,
                                    SysRowID = row.Labor.LaborDtl.SysRowID,
                                    HCMLaborDtlSync = row.Labor.HCMLaborDtlSync
                                });


                selectLaborDtlJoinQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectLaborDtlJoinQuery(this.Db, company, employeeNum, startClockInDate, endClockInDate, timeStatus);
        }

        static Func<ErpContext, string, string, DateTime?, DateTime?, string, string, IEnumerable<LaborHCMRecord>> selectLaborDtlJoin2Query;
        private IEnumerable<LaborHCMRecord> SelectLaborDtlJoin2(string company, string employeeNum, DateTime? startClockInDate, DateTime? endClockInDate, string timeStatus, string status)
        {
            if (selectLaborDtlJoin2Query == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, DateTime?, string, string, IEnumerable<LaborHCMRecord>>> expression =
                    (ctx, company_ex, employeeNum_ex, startClockInDate_ex, endClockInDate_ex, timeStatus_ex, status_ex) =>
                     ctx.LaborDtl
                                .GroupJoin(ctx.HCMLaborDtlSync,
                                     lbdtl => new { lbdtl.Company, RowID = lbdtl.SysRowID },
                                     hcmsyn => new { hcmsyn.Company, RowID = hcmsyn.LaborDtlSysRowID },
                                     (lbdtl, hcmsyn) => new { LaborDtl = lbdtl, HCMLaborDtlSync = hcmsyn })
                                .SelectMany(lbhd => lbhd.HCMLaborDtlSync.DefaultIfEmpty(),
                                     (lbdtl, hcmsyn) => new { LaborDtl = lbdtl.LaborDtl, HCMLaborDtlSync = hcmsyn })
                                .Join(ctx.EmpBasic,
                                      lbdtl => new { lbdtl.LaborDtl.Company, EmpID = lbdtl.LaborDtl.EmployeeNum },
                                      emp => new { emp.Company, emp.EmpID },
                                      (lbdtl, emp) => new { Labor = lbdtl, Employee = emp })
                                .Where(at => at.Labor.LaborDtl.Company == company_ex
                                         && at.Labor.LaborDtl.EmployeeNum == employeeNum_ex
                                         && at.Labor.LaborDtl.TimeStatus == timeStatus_ex
                                         && at.Labor.LaborDtl.ClockInDate.Value >= startClockInDate_ex.Value
                                         && at.Labor.LaborDtl.ClockInDate.Value <= endClockInDate_ex.Value
                                         && at.Labor.LaborDtl.HCMPayHours > 0
                                         && status_ex.ToUpper().Contains(at.Labor.HCMLaborDtlSync.Status.ToUpper()))
                                // && at.Employee.Payroll == payroll_ex) Change to LaborHed.FeedPayroll = payroll_ex
                                .Select(row => new LaborHCMRecord
                                {
                                    Company = row.Labor.LaborDtl.Company,
                                    EmployeeNum = row.Employee.EmpID,
                                    LaborHedSeq = row.Labor.LaborDtl.LaborHedSeq,
                                    LaborDtlSeq = row.Labor.LaborDtl.LaborDtlSeq,
                                    LaborNote = row.Labor.LaborDtl.LaborNote,
                                    LaborTypePseudo = row.Labor.LaborDtl.LaborTypePseudo,
                                    ClockInDate = row.Labor.LaborDtl.ClockInDate,
                                    DspClockInTime = row.Labor.LaborDtl.DspClockInTime,
                                    DspClockOutTime = row.Labor.LaborDtl.DspClockOutTime,
                                    PayHours = row.Labor.LaborDtl.HCMPayHours,
                                    JCDept = row.Labor.LaborDtl.JCDept,
                                    Shift = row.Labor.LaborDtl.Shift,
                                    ProjectID = row.Labor.LaborDtl.ProjectID,
                                    SysRowID = row.Labor.LaborDtl.SysRowID,
                                    HCMLaborDtlSync = row.Labor.HCMLaborDtlSync
                                });


                selectLaborDtlJoin2Query = DBExpressionCompiler.Compile(expression);
            }
            return selectLaborDtlJoin2Query(this.Db, company, employeeNum, startClockInDate, endClockInDate, timeStatus, status);
        }

        static Func<ErpContext, string, string, DateTime?, DateTime?, bool, IEnumerable<LaborHCMRecord>> selectLaborHedJoinQuery;
        private IEnumerable<LaborHCMRecord> SelectLaborHedJoin(string company, string employeeNum, DateTime? startClockInDate, DateTime? endClockInDate, bool payroll)
        {
            if (selectLaborHedJoinQuery == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, DateTime?, bool, IEnumerable<LaborHCMRecord>>> expression =
                    (ctx, company_ex, employeeNum_ex, startClockInDate_ex, endClockInDate_ex, payroll_ex) =>
                     ctx.LaborHed
                                .GroupJoin(ctx.HCMLaborDtlSync,
                                     lbhd => new { lbhd.Company, RowID = lbhd.SysRowID },
                                     hcmsyn => new { hcmsyn.Company, RowID = hcmsyn.LaborDtlSysRowID },
                                     (lbhd, hcmsyn) => new { LaborHed = lbhd, HCMLaborDtlSync = hcmsyn })
                                .SelectMany(lbhd => lbhd.HCMLaborDtlSync.DefaultIfEmpty(),
                                            (lbhd, hcmsyn) => new { LaborHed = lbhd.LaborHed, HCMLaborDtlSync = hcmsyn })
                                .Join(ctx.EmpBasic,
                                      lbhd => new { lbhd.LaborHed.Company, EmpID = lbhd.LaborHed.EmployeeNum },
                                      emp => new { emp.Company, emp.EmpID },
                                      (lbhd, emp) => new { Labor = lbhd, Employee = emp })
                                .Where(at => at.Labor.LaborHed.Company == company_ex
                                         && at.Labor.LaborHed.EmployeeNum == employeeNum_ex
                                         && at.Labor.LaborHed.ClockInDate.Value >= startClockInDate_ex.Value
                                         && at.Labor.LaborHed.ClockInDate.Value <= endClockInDate_ex.Value
                                         && at.Labor.LaborHed.FeedPayroll == payroll_ex)
                                .Select(row => new LaborHCMRecord
                                {
                                    Company = row.Labor.LaborHed.Company,
                                    EmployeeNum = row.Employee.EmpID,
                                    LaborHedSeq = row.Labor.LaborHed.LaborHedSeq,
                                    LaborDtlSeq = 0,
                                    LaborNote = string.Empty,
                                    LaborTypePseudo = string.Empty,
                                    ClockInDate = row.Labor.LaborHed.ClockInDate,
                                    DspClockInTime = row.Labor.LaborHed.DspClockInTime,
                                    DspClockOutTime = row.Labor.LaborHed.DspClockOutTime,
                                    PayHours = row.Labor.LaborHed.PayHours,
                                    JCDept = string.Empty,
                                    Shift = row.Labor.LaborHed.Shift,
                                    ProjectID = string.Empty,
                                    SysRowID = row.Labor.LaborHed.SysRowID,
                                    HCMPayHoursCalcType = row.Labor.LaborHed.HCMPayHoursCalcType,
                                    HCMLaborDtlSync = row.Labor.HCMLaborDtlSync
                                });


                selectLaborHedJoinQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectLaborHedJoinQuery(this.Db, company, employeeNum, startClockInDate, endClockInDate, payroll);
        }

        static Func<ErpContext, string, string, DateTime?, DateTime?, bool, string, IEnumerable<LaborHCMRecord>> selectLaborHedJoin2Query;
        private IEnumerable<LaborHCMRecord> SelectLaborHedJoin2(string company, string employeeNum, DateTime? startClockInDate, DateTime? endClockInDate, bool payroll, string status)
        {
            if (selectLaborHedJoin2Query == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, DateTime?, bool, string, IEnumerable<LaborHCMRecord>>> expression =
                    (ctx, company_ex, employeeNum_ex, startClockInDate_ex, endClockInDate_ex, payroll_ex, status_ex) =>
                     ctx.LaborHed
                                .GroupJoin(ctx.HCMLaborDtlSync,
                                     lbhd => new { lbhd.Company, RowID = lbhd.SysRowID },
                                     hcmsyn => new { hcmsyn.Company, RowID = hcmsyn.LaborDtlSysRowID },
                                     (lbhd, hcmsyn) => new { LaborHed = lbhd, HCMLaborDtlSync = hcmsyn })
                                .SelectMany(lbhd => lbhd.HCMLaborDtlSync.DefaultIfEmpty(),
                                            (lbhd, hcmsyn) => new { LaborHed = lbhd.LaborHed, HCMLaborDtlSync = hcmsyn })
                                .Join(ctx.EmpBasic,
                                      lbhd => new { lbhd.LaborHed.Company, EmpID = lbhd.LaborHed.EmployeeNum },
                                      emp => new { emp.Company, emp.EmpID },
                                      (lbhd, emp) => new { Labor = lbhd, Employee = emp })
                                .Where(at => at.Labor.LaborHed.Company == company_ex
                                         && at.Labor.LaborHed.EmployeeNum == employeeNum_ex
                                         && at.Labor.LaborHed.ClockInDate.Value >= startClockInDate_ex.Value
                                         && at.Labor.LaborHed.ClockInDate.Value <= endClockInDate_ex.Value
                                         && status_ex.ToUpper().Contains(at.Labor.HCMLaborDtlSync.Status.ToUpper())
                                         && at.Labor.LaborHed.FeedPayroll == payroll_ex)
                                .Select(row => new LaborHCMRecord
                                {
                                    Company = row.Labor.LaborHed.Company,
                                    EmployeeNum = row.Employee.EmpID,
                                    LaborHedSeq = row.Labor.LaborHed.LaborHedSeq,
                                    LaborDtlSeq = 0,
                                    LaborNote = string.Empty,
                                    LaborTypePseudo = string.Empty,
                                    ClockInDate = row.Labor.LaborHed.ClockInDate,
                                    DspClockInTime = row.Labor.LaborHed.DspClockInTime,
                                    DspClockOutTime = row.Labor.LaborHed.DspClockOutTime,
                                    PayHours = row.Labor.LaborHed.PayHours,
                                    JCDept = string.Empty,
                                    Shift = row.Labor.LaborHed.Shift,
                                    ProjectID = string.Empty,
                                    SysRowID = row.Labor.LaborHed.SysRowID,
                                    HCMPayHoursCalcType = row.Labor.LaborHed.HCMPayHoursCalcType,
                                    HCMLaborDtlSync = row.Labor.HCMLaborDtlSync
                                });


                selectLaborHedJoin2Query = DBExpressionCompiler.Compile(expression);
            }
            return selectLaborHedJoin2Query(this.Db, company, employeeNum, startClockInDate, endClockInDate, payroll, status);
        }

        static Func<ErpContext, string, Guid, HCMLaborDtlSync> findFirstHCMLaborDtlSyncWithUpdLockQuery;
        private HCMLaborDtlSync FindFirstHCMLaborDtlWithUpdLock(string company, Guid laborDtlSysRowID)
        {
            if (findFirstHCMLaborDtlSyncWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, Guid, HCMLaborDtlSync>> expression =
      (ctx, company_ex, laborDtlSysRowID_ex) =>
        (from row in ctx.HCMLaborDtlSync.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.LaborDtlSysRowID == laborDtlSysRowID_ex
         select row).FirstOrDefault();
                findFirstHCMLaborDtlSyncWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstHCMLaborDtlSyncWithUpdLockQuery(this.Db, company, laborDtlSysRowID);
        }

        #endregion HCM Integration queries

        #region XbSyst Queries

        private static Func<ErpContext, string, bool> existsXbSystJobLotDfltQuery;

        private bool ExistsXbSystJobLotDflt(string company)
        {
            if (existsXbSystJobLotDfltQuery == null)
            {
                Expression<Func<ErpContext, string, bool>> expression =
                (ctx, company_ex) =>
                (from row in ctx.XbSyst
                 where row.Company == company_ex
                 select row.JobLotDflt).FirstOrDefault();
                existsXbSystJobLotDfltQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsXbSystJobLotDfltQuery(this.Db, company);
        }

        #endregion XbSyst Queries

        #region XFileAttch Queries
        static Func<ErpContext, string, string, string, string, string, bool> existsXFileAttchQuery;
        private bool ExistsXFileAttch(string company, string relatedToSchemaName, string relatedToFile, string key1, string key2)
        {
            if (existsXFileAttchQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, bool>> expression =
        (ctx, company_ex, relatedToSchemaNamestring_ex, relatedToFile_ex, key1_ex, key2_ex) =>
        (from row in ctx.XFileAttch
         where row.Company == company_ex &&
         row.RelatedToSchemaName == relatedToSchemaNamestring_ex &&
         row.RelatedToFile == relatedToFile_ex &&
         row.Key1 == key1_ex &&
         row.Key2 == key2_ex &&
         row.Key3 == "" &&
         row.Key4 == "" &&
         row.Key5 == ""
         select row).Any();
                existsXFileAttchQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsXFileAttchQuery(this.Db, company, relatedToSchemaName, relatedToFile, key1, key2);
        }

        #endregion XFileAttch Queries
    }
}
