using Epicor.Data;
using Erp.Tables;
using Ice.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Erp.Services.BO
{
    public partial class ConfigurationRuntimeSvc
    {

        #region "CnvProgs Queries"

        static Func<ErpContext, string, string, string, DateTime?> findCnvProgsQuery;
        private DateTime? FindCnvProgs(string systemCode, string conversionID, string programStatus)
        {
            if (findCnvProgsQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, DateTime?>> expression =
                    (ctx, systemCode_ex, conversionID_ex, programStatus_ex) =>
                   (from row in ctx.CnvProgs
                    where row.SystemCode == systemCode_ex &&
                    row.ConversionID == conversionID_ex &&
                    row.ProgStatus == programStatus_ex
                    select row.LastRunOn).FirstOrDefault();
                findCnvProgsQuery = DBExpressionCompiler.Compile(expression);
            }
            return findCnvProgsQuery(this.Db, systemCode, conversionID, programStatus);
        }
        #endregion "CnvProgs Queries

        #region Currency Queries

        private class CurrencyData
        {
            public string CurrencyCode { get; set; }
            public string CurrencyID { get; set; }
            public string CurrSymbol { get; set; }
        }
        static Func<ErpContext, string, CurrencyData> findBaseCurrencyQuery;
        private CurrencyData FindBaseCurrency(string company)
        {
            if (findBaseCurrencyQuery == null)
            {
                Expression<Func<ErpContext, string, CurrencyData>> expression =
                    (ctx, company_ex) =>
                        (from row in ctx.Currency
                         where row.Company == company_ex &&
                         row.BaseCurr == true
                         select new CurrencyData
                         {
                             CurrencyCode = row.CurrencyCode,
                             CurrencyID = row.CurrencyID,
                             CurrSymbol = row.CurrSymbol
                         }).FirstOrDefault();
                findBaseCurrencyQuery = DBExpressionCompiler.Compile(expression);
            }
            return findBaseCurrencyQuery(this.Db, company);
        }

        static Func<ErpContext, string, string, CurrencyData> findFirstCurrencyQuery;
        private CurrencyData FindFirstCurrency(string company, string currencyCode)
        {
            if (findFirstCurrencyQuery == null)
            {
                Expression<Func<ErpContext, string, string, CurrencyData>> expression =
                    (ctx, company_ex, currencyCode_ex) =>
                        (from row in ctx.Currency
                         where row.Company == company_ex &&
                         row.CurrencyCode == currencyCode_ex
                         select new CurrencyData
                         {
                             CurrencyCode = row.CurrencyCode,
                             CurrencyID = row.CurrencyID,
                             CurrSymbol = row.CurrSymbol
                         }).FirstOrDefault();
                findFirstCurrencyQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstCurrencyQuery(this.Db, company, currencyCode);
        }

        #endregion Configuration Summary Queries

        #region Customer Queries

        private class CustomerCols
        {
            public bool OTSmartString { get; set; }
        }
        static Func<ErpContext, string, int, CustomerCols> findFirstCustomerQuery;
        private CustomerCols FindFirstCustomer(string company, int custNum)
        {
            if (findFirstCustomerQuery == null)
            {
                Expression<Func<ErpContext, string, int, CustomerCols>> expression =
                    (ctx, company_ex, custNum_ex) =>
                        (from row in ctx.Customer
                         where row.Company == company_ex &&
                         row.CustNum == custNum_ex
                         select new CustomerCols
                         {
                             OTSmartString = row.OTSmartString
                         }).FirstOrDefault();
                findFirstCustomerQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstCustomerQuery(this.Db, company, custNum);
        }

        #endregion Customer Queries

        #region DemandDetail Queries

        private class DemandDetailCols
        {
            public int DemandContractNum { get; set; }
            public int DemandHeadSeq { get; set; }
            public int DemandDtlSeq { get; set; }
            public DateTime? LastConfigDate { get; set; }
            public int LastConfigTime { get; set; }
            public string LastConfigUserID { get; set; }
            public decimal ConfigBaseUnitPrice { get; set; }
            public decimal ConfigUnitPrice { get; set; }
            public string BasePartNum { get; set; }
            public string BaseRevisionNum { get; set; }
            public int GroupSeq { get; set; }
        }
        static Func<ErpContext, string, Guid, DemandDetailCols> findFirstDemandDetailBySysRowIDQuery;
        private DemandDetailCols FindFirstDemandDetailBySysRowID(string company, Guid sysRowID)
        {
            if (findFirstDemandDetailBySysRowIDQuery == null)
            {
                Expression<Func<ErpContext, string, Guid, DemandDetailCols>> expression =
                    (ctx, company_ex, sysRowID_ex) =>
                        (from row in ctx.DemandDetail
                         where row.Company == company_ex &&
                         row.SysRowID == sysRowID_ex
                         select new DemandDetailCols
                         {
                             DemandContractNum = row.DemandContractNum,
                             DemandHeadSeq = row.DemandHeadSeq,
                             DemandDtlSeq = row.DemandDtlSeq,
                             LastConfigDate = row.LastConfigDate,
                             LastConfigUserID = row.LastConfigUserID,
                             LastConfigTime = row.LastConfigTime,
                             ConfigBaseUnitPrice = row.ConfigBaseUnitPrice,
                             ConfigUnitPrice = row.ConfigUnitPrice,
                             BasePartNum = row.BasePartNum,
                             BaseRevisionNum = row.BaseRevisionNum,
                             GroupSeq = row.GroupSeq
                         }).FirstOrDefault();
                findFirstDemandDetailBySysRowIDQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstDemandDetailBySysRowIDQuery(this.Db, company, sysRowID);
        }

        static Func<ErpContext, string, int, int, int, DemandDetail> findFirstDemandDetailUpdLockQuery;
        private DemandDetail FindFirstDemandDetailUpdLock(string company, int dmdContNum, int dmdHeadNum, int dmdLineNum)
        {
            if (findFirstDemandDetailUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, DemandDetail>> expression =
                    (ctx, company_ex, dmdContractNum_ex, dmdHeadNum_ex, dmdLineNum_ex) =>
                        (from row in ctx.DemandDetail.With(LockHint.UpdLock)
                         where row.Company == company_ex &&
                         row.DemandContractNum == dmdContractNum_ex &&
                         row.DemandHeadSeq == dmdHeadNum_ex &&
                         row.DemandDtlSeq == dmdLineNum_ex
                         select row).FirstOrDefault();
                findFirstDemandDetailUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstDemandDetailUpdLockQuery(this.Db, company, dmdContNum, dmdHeadNum, dmdLineNum);
        }

        static Func<ErpContext, string, int, int, int, IEnumerable<SysRowIDResult>> selectDemandDetailQuery_2;
        private IEnumerable<SysRowIDResult> SelectDemandDetail(string company, int demandContractNum, int demandHeadSeq, int demandDtlSeq)
        {
            if (selectDemandDetailQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, int, int, IEnumerable<SysRowIDResult>>> expression =
                    (ctx, company_ex, demandContractNum_ex, demandHeadSeq_ex, demandDtlSeq_ex) =>
                        (from row in ctx.DemandDetail
                         where row.Company == company_ex &&
                         row.DemandContractNum == demandContractNum_ex &&
                         row.DemandHeadSeq == demandHeadSeq_ex &&
                         row.DemandDtlSeq != demandDtlSeq_ex
                         select new SysRowIDResult { SysRowID = row.SysRowID });
                selectDemandDetailQuery_2 = DBExpressionCompiler.Compile(expression);
            }
            return selectDemandDetailQuery_2(this.Db, company, demandContractNum, demandHeadSeq, demandDtlSeq);
        }

        #endregion DemandDetail Queries

        #region DemandHead Queries

        static Func<ErpContext, string, int, string> findDemandHeadCurrencyCodeQuery;
        private string FindDemandHeadCurrencyCode(string company, int dmdHeadNum)
        {
            if (findDemandHeadCurrencyCodeQuery == null)
            {
                Expression<Func<ErpContext, string, int, string>> expression =
                    (ctx, company_ex, dmdHeadNum_ex) =>
                        (from row in ctx.DemandHead
                         where row.Company == company_ex &&
                         row.DemandHeadSeq == dmdHeadNum_ex
                         select row.CurrencyCode).FirstOrDefault();
                findDemandHeadCurrencyCodeQuery = DBExpressionCompiler.Compile(expression);
            }
            return findDemandHeadCurrencyCodeQuery(this.Db, company, dmdHeadNum);
        }

        private class DemandHeadCols
        {
            public string Company { get; set; }
            public int CustNum { get; set; }
            public string ShipToNum { get; set; }
        }
        static Func<ErpContext, string, int, DemandHeadCols> findFirstDemandHeadQuery;
        private DemandHeadCols FindFirstDemandHead(string company, int dmdHeadNum)
        {
            if (findFirstDemandHeadQuery == null)
            {
                Expression<Func<ErpContext, string, int, DemandHeadCols>> expression =
                    (ctx, company_ex, dmdHeadNum_ex) =>
                        (from row in ctx.DemandHead
                         where row.Company == company_ex &&
                         row.DemandHeadSeq == dmdHeadNum_ex
                         select new DemandHeadCols
                         {
                             Company = row.Company,
                             CustNum = row.CustNum,
                             ShipToNum = row.ShipToNum
                         }).FirstOrDefault();
                findFirstDemandHeadQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstDemandHeadQuery(this.Db, company, dmdHeadNum);
        }

        #endregion DemandHead Queries

        #region Image Queries


        static Func<ErpContext, string, string, byte[]> findFirstImage;
        private byte[] FindFirstImage(string company, string imageID)
        {
            if (findFirstImage == null)
            {
                Expression<Func<ErpContext, string, string, byte[]>> expression =
                    (ctx, company_ex, imageID_ex) =>
                        (from row in ctx.Image
                         join file in ctx.FileStore on new { row.Company, row.ImageID } equals new { file.Company, ImageID = file.FileName }
                         where row.Company == company_ex &&
                         row.ImageID == imageID_ex
                         select file.Content).FirstOrDefault();
                findFirstImage = DBExpressionCompiler.Compile(expression);
            }
            return findFirstImage(this.Db, company, imageID);
        }



        #endregion

        #region InspPlanRev Queries

        private class InspPlanRevCols
        {
            public string ConfigID { get; set; }
        }
        Func<ErpContext, string, string, string, InspPlanRevCols> findInspPlanRev;
        private InspPlanRevCols FindInspPlanRev(string company, string inspPlanNum, string inspPlanRev)
        {
            if (findInspPlanRev == null)
            {
                Expression<Func<ErpContext, string, string, string, InspPlanRevCols>> expression =
                    (ctx, company_ex, inspPlanNum_ex, inspPlanRev_ex) =>
                        (from row in ctx.InspPlanRev
                         where row.Company == company_ex &&
                            row.InspPlanNum == inspPlanNum_ex &&
                            row.InspPlanRevNum == inspPlanRev_ex
                         select new InspPlanRevCols
                         {
                             ConfigID = row.ConfigID
                         }).FirstOrDefault();
                findInspPlanRev = DBExpressionCompiler.Compile(expression);
            }
            return findInspPlanRev(Db, company, inspPlanNum, inspPlanRev);
        }

        #endregion InspPlanRev Queries

        #region InspResults queries

        private class InspResultsCols
        {
            public string InspPlanPartNum { get; set; }
            public string InspPlanRevNum { get; set; }
        }
        static Func<ErpContext, Guid, InspResultsCols> findFirstInspResultsQuery;
        private InspResultsCols FindFirstInspResults(Guid sysRowID)
        {
            if (findFirstInspResultsQuery == null)
            {
                Expression<Func<ErpContext, Guid, InspResultsCols>> expression =
                    (ctx, sysRowID_ex) =>
                        (from row in ctx.InspResults
                         where row.SysRowID == sysRowID_ex
                         select new InspResultsCols
                         {
                             InspPlanPartNum = row.InspPlanPartNum,
                             InspPlanRevNum = row.InspPlanRevNum
                         }).FirstOrDefault();
                findFirstInspResultsQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstInspResultsQuery(this.Db, sysRowID);
        }
        #endregion InspResults queries

        #region OrderDtl Queries


        private class OrderDetailCols
        {
            public int OrderNum { get; set; }
            public int OrderLine { get; set; }
            public decimal ConfigBaseUnitPrice { get; set; }
            public decimal ConfigUnitPrice { get; set; }
            public string BasePartNum { get; set; }
            public string BaseRevisionNum { get; set; }
            public DateTime? LastConfigDate { get; set; }
            public int LastConfigTime { get; set; }
            public string LastConfigUserID { get; set; }
            public int GroupSeq { get; set; }
        }
        static Func<ErpContext, string, Guid, OrderDetailCols> findOrderDtlBySysRowIDQuery;
        private OrderDetailCols FindOrderDtlBySysRowID(string company, Guid sysRowID)
        {
            if (findOrderDtlBySysRowIDQuery == null)
            {
                Expression<Func<ErpContext, string, Guid, OrderDetailCols>> expression =
                    (ctx, company_ex, sysRowID_ex) =>
                        (from row in ctx.OrderDtl
                         where row.Company == company_ex &&
                         row.SysRowID == sysRowID_ex
                         select new OrderDetailCols
                         {
                             OrderNum = row.OrderNum,
                             OrderLine = row.OrderLine,
                             ConfigBaseUnitPrice = row.ConfigBaseUnitPrice,
                             ConfigUnitPrice = row.ConfigUnitPrice,
                             BasePartNum = row.BasePartNum,
                             BaseRevisionNum = row.BaseRevisionNum,
                             LastConfigDate = row.LastConfigDate,
                             LastConfigTime = row.LastConfigTime,
                             LastConfigUserID = row.LastConfigUserID,
                             GroupSeq = row.GroupSeq
                         }).FirstOrDefault();
                findOrderDtlBySysRowIDQuery = DBExpressionCompiler.Compile(expression);
            }
            return findOrderDtlBySysRowIDQuery(this.Db, company, sysRowID);
        }

        static Func<ErpContext, string, int, int, IEnumerable<SysRowIDResult>> selectOrderDtlQuery_2;
        private IEnumerable<SysRowIDResult> SelectOrderDtl(string company, int ordernum, int orderline)
        {
            if (selectOrderDtlQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<SysRowIDResult>>> expression =
                    (ctx, company_ex, ordernum_ex, orderline_ex) =>
                        (from row in ctx.OrderDtl
                         where row.Company == company_ex &&
                         row.OrderNum == ordernum_ex &&
                         row.OrderLine != orderline_ex
                         select new SysRowIDResult { SysRowID = row.SysRowID });
                selectOrderDtlQuery_2 = DBExpressionCompiler.Compile(expression);
            }
            return selectOrderDtlQuery_2(this.Db, company, ordernum, orderline);
        }

        #endregion OrderDtl Queries

        #region OrderHed Queries

        static Func<ErpContext, string, int, string> findOrderCurrencyQuery;
        private string FindOrderCurrency(string company, int orderNum)
        {
            if (findOrderCurrencyQuery == null)
            {
                Expression<Func<ErpContext, string, int, string>> expression =
                    (ctx, company_ex, orderNum_ex) =>
                        (from row in ctx.OrderHed
                         where row.Company == company_ex &&
                         row.OrderNum == orderNum_ex
                         select row.CurrencyCode).FirstOrDefault();
                findOrderCurrencyQuery = DBExpressionCompiler.Compile(expression);
            }
            return findOrderCurrencyQuery(this.Db, company, orderNum);
        }
        #endregion OrderHed Queries

        #region Part Queries
        static Func<ErpContext, string, string, bool> existsGlobalPartQuery;
        private bool existsGlobalPart(string company, string partNum)
        {
            if (existsGlobalPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (ctx, company_ex, partNum_ex) =>
                        (from row in ctx.Part
                         where row.Company == company_ex &&
                         row.PartNum == partNum_ex &&
                         row.GlobalPart == true
                         select row).Any();

                existsGlobalPartQuery = DBExpressionCompiler.Compile(expression);
            }
            return existsGlobalPartQuery(Db, company, partNum);
        }

        Func<ErpContext, string, string, Part> findPartGlobalUpdate;
        private Part FindPartGlobalUpdate(string company, string partNum)
        {
            if (findPartGlobalUpdate == null)
            {
                Expression<Func<ErpContext, string, string, Part>> expression =
                    (ctx, company_ex, partNum_ex) =>
                        (from row in ctx.Part.With(LockHint.UpdLock)
                         where row.Company == company_ex &&
                         row.PartNum == partNum_ex
                         select row).FirstOrDefault();
                findPartGlobalUpdate = DBExpressionCompiler.Compile(expression);
            }
            return findPartGlobalUpdate(Db, company, partNum);
        }

        static Func<ErpContext, string, string, bool> existsPartQuery;
        private bool ExistsPart(string company, string partNum)
        {
            if (existsPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (ctx, company_ex, partNum_ex) =>
                        (from row in ctx.Part
                         where row.Company == company_ex &&
                         row.PartNum == partNum_ex
                         select row).Any();
                existsPartQuery = DBExpressionCompiler.Compile(expression);
            }
            return existsPartQuery(this.Db, company, partNum);
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
        #endregion Part Queries

        #region PartPlant Queries
        static Func<ErpContext, string, string, string, bool> partIsSalesKitForPlantQuery;
        private bool PartIsSalesKitForParentAndPhantomPart(string company, string partNum, string plantID)
        {
            if (partIsSalesKitForPlantQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
                    (ctx, company_ex, partNum_ex, plantID_ex) =>
                        (from row in ctx.PartPlant
                         where row.Company == company_ex &&
                         row.PartNum == partNum_ex &&
                         row.Plant == plantID_ex &&
                         row.SourceType.Equals("K")
                         select row).Any();

                partIsSalesKitForPlantQuery = DBExpressionCompiler.Compile(expression);
            }
            return partIsSalesKitForPlantQuery(Db, company, partNum, plantID);
        }
        #endregion PartPlant Queries

        #region PartRev Queries

        static Func<ErpContext, string, string, string, string, bool> existsPartRevQuery;
        private bool ExistsEqualBaseConfigID(string company, string partNum, string revisionNum, string configID)
        {
            if (existsPartRevQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, bool>> expression =
        (ctx, company_ex, partNum_ex, revisionNum_ex, configID_ex) =>
        (from row in ctx.PartRev
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.RevisionNum == revisionNum_ex &&
         !String.IsNullOrEmpty(row.ConfigID) &&
         row.ConfigID == configID_ex
         select row).Any();
                existsPartRevQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPartRevQuery(this.Db, company, partNum, revisionNum, configID);
        }

        /// <summary>
        /// Use to return only the PartRev Base part number and base revision number
        /// </summary>
        private class BasePartRev
        {
            public string BasePartNumber { get; set; }
            public string BaseRevisionNumber { get; set; }
        }

        static Func<ErpContext, string, string, string, bool> existsPartRevQuery_3;
        private bool ExistsPartRev(string company, string partNum, string revisionNum)
        {
            if (existsPartRevQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
                    (ctx, company_ex, partNum_ex, revisionNum_ex) =>
                        (from row in ctx.PartRev
                         where row.Company == company_ex &&
                         row.PartNum == partNum_ex &&
                         row.RevisionNum == revisionNum_ex
                         select row).Any();
                existsPartRevQuery_3 = DBExpressionCompiler.Compile(expression);
            }
            return existsPartRevQuery_3(this.Db, company, partNum, revisionNum);
        }

        static Func<ErpContext, string, string, string, string, PartRev> findFirstPartRevQuery_2;
        private PartRev FindFirstPartRev(string company, string partNum, string revisionNum, string configID)
        {
            if (findFirstPartRevQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, PartRev>> expression =
                    (ctx, company_ex, partNum_ex, revisionNum_ex, configID_ex) =>
                        (from row in ctx.PartRev
                         where row.Company == company_ex &&
                         row.PartNum == partNum_ex &&
                         row.RevisionNum == revisionNum_ex &&
                         row.ConfigID == configID_ex
                         select row).FirstOrDefault();
                findFirstPartRevQuery_2 = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPartRevQuery_2(this.Db, company, partNum, revisionNum, configID);
        }

        private class PartRevCols
        {
            public string Company { get; set; }
            public string ConfigID { get; set; }
            public string PartNum { get; set; }
            public string RevisionNum { get; set; }
        }
        static Func<ErpContext, string, string, string, PartRevCols> findFirstPartRevQuery;
        private PartRevCols FindFirstPartRev(string company, string partNum, string revisionNum)
        {
            if (findFirstPartRevQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, PartRevCols>> expression =
                    (ctx, company_ex, partNum_ex, revisionNum_ex) =>
                        (from row in ctx.PartRev
                         where row.Company == company_ex &&
                         row.PartNum == partNum_ex &&
                         row.RevisionNum == revisionNum_ex
                         select new PartRevCols
                         {
                             Company = row.Company,
                             ConfigID = row.ConfigID,
                             PartNum = row.PartNum,
                             RevisionNum = row.RevisionNum
                         }).FirstOrDefault();
                findFirstPartRevQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPartRevQuery(this.Db, company, partNum, revisionNum);
        }

        static Func<ErpContext, string, string, string, string, PartRev> findFirstPartRevAltQuery;
        private PartRev FindFirstPartRevAlt(string company, string partNum, string revisionNum, string altMethod)
        {
            if (findFirstPartRevAltQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, PartRev>> expression =
                    (ctx, company_ex, partNum_ex, revisionNum_ex, altMethod_ex) =>
                        (from row in ctx.PartRev
                         where row.Company == company_ex &&
                         row.PartNum == partNum_ex &&
                         row.RevisionNum == revisionNum_ex &&
                         row.AltMethod == altMethod_ex
                         select row).FirstOrDefault();
                findFirstPartRevAltQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPartRevAltQuery(this.Db, company, partNum, revisionNum, altMethod);
        }


        static Func<ErpContext, string, string, string, string, Guid> findPartRevSysRowIDQuery;
        private Guid FindPartRevSysRowID(string company, string partNum, string revisionNum, string configID)
        {
            if (findPartRevSysRowIDQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, Guid>> expression =
                    (ctx, company_ex, partNum_ex, revisionNum_ex, configID_ex) =>
                        (from row in ctx.PartRev
                         where row.Company == company_ex &&
                         row.PartNum == partNum_ex &&
                         row.RevisionNum == revisionNum_ex &&
                         row.ConfigID == configID_ex
                         select row.SysRowID).FirstOrDefault();
                findPartRevSysRowIDQuery = DBExpressionCompiler.Compile(expression);
            }
            return findPartRevSysRowIDQuery(this.Db, company, partNum, revisionNum, configID);
        }
        #endregion PartRev Queries

        #region PcDynLst Queries
        private static Func<ErpContext, string, string, string, bool> existsPcDynLstQuery;
        private bool ExistsPcDynLst(string company, string configID, string inputName)
        {
            if (existsPcDynLstQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
                    (ctx, company_ex, configID_ex, inputName_ex) =>
                        (from row in ctx.PcDynLst
                         where row.Company == company_ex &&
                         row.ConfigID == configID_ex &&
                         row.InputName == inputName_ex
                         select row).Any();
                existsPcDynLstQuery = DBExpressionCompiler.Compile(expression);
            }
            return existsPcDynLstQuery(this.Db, company, configID, inputName);
        }

        #endregion

        #region PcECCOrderDtl Queries

        class SysRowIDResult
        {
            public Guid SysRowID { get; set; }
        }
        static Func<ErpContext, string, string, IEnumerable<SysRowIDResult>> selectPcECCOrderDtlQuery;
        private IEnumerable<SysRowIDResult> SelectPcECCOrderDtl(string company, string ECCQuoteNum)
        {
            if (selectPcECCOrderDtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<SysRowIDResult>>> expression =
                    (ctx, company_ex, ECCQuoteNum_ex) =>
                        (from row in ctx.PcECCOrderDtl
                         where row.Company == company_ex &&
                         row.ECCQuoteNum == ECCQuoteNum_ex
                         select new SysRowIDResult { SysRowID = row.SysRowID });

                selectPcECCOrderDtlQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectPcECCOrderDtlQuery(this.Db, company, ECCQuoteNum);
        }

        #endregion PcECCOrderDtl Queries

        #region PcInputs Queries

        private class PcInputsCols
        {
            public string FormatString { get; set; }
            public string DataType { get; set; }
        }
        static Func<ErpContext, string, string, string, PcInputsCols> findFirstPcInputsQuery;
        private PcInputsCols FindFirstPcInputs(string company, string configID, string inputName)
        {
            if (findFirstPcInputsQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, PcInputsCols>> expression =
                (ctx, company_ex, configID_ex, inputName_ex) =>
                  (from row in ctx.PcInputs
                   where row.Company == company_ex &&
                     row.ConfigID == configID_ex &&
                     row.InputName == inputName_ex
                   select new PcInputsCols
                   {
                       FormatString = row.FormatString,
                       DataType = row.DataType
                   }).FirstOrDefault();
                findFirstPcInputsQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPcInputsQuery(this.Db, company, configID, inputName);
        }

        static Func<ErpContext, string, string, bool> existsPcInputsQuery;
        private bool ExistsPcInputs(string company, string configID)
        {
            if (existsPcInputsQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (ctx, company_ex, configID_ex) =>
                        (from row in ctx.PcInputs
                         where row.Company == company_ex &&
                         row.ConfigID == configID_ex
                         select row).Any();
                existsPcInputsQuery = DBExpressionCompiler.Compile(expression);
            }
            return existsPcInputsQuery(this.Db, company, configID);
        }

        private class PcInputNodeValues
        {
            public string SideLabel { get; set; }
            public string SummaryLabel { get; set; }
            public bool NoDispSummary { get; set; }
            public int TabOrder { get; set; }
        }
        static Func<ErpContext, string, string, string, bool, PcInputNodeValues> isNodePcInputsQuery;
        private PcInputNodeValues IsNodePcInputs(string company, string configID, string inputName, bool noDispSummary)
        {
            if (isNodePcInputsQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool, PcInputNodeValues>> expression =
                (ctx, company_ex, configID_ex, inputName_ex, noDispSummary_ex) =>
                  (from row in ctx.PcInputs
                   where row.Company == company_ex &&
                     row.ConfigID == configID_ex &&
                     row.InputName == inputName_ex &&
                         row.NoDispSummary == noDispSummary_ex
                   select new PcInputNodeValues
                   {
                       SideLabel = row.SideLabel,
                       SummaryLabel = row.SummaryLabel,
                       NoDispSummary = row.NoDispSummary,
                       TabOrder = row.TabOrder
                   }).FirstOrDefault();
                isNodePcInputsQuery = DBExpressionCompiler.Compile(expression);
            }

            return isNodePcInputsQuery(this.Db, company, configID, inputName, noDispSummary);
        }

        static Func<ErpContext, string, string, string, IEnumerable<PcInputs>> selectPcInputsQuery_3;
        private IEnumerable<PcInputs> SelectPcInputs(string company, string configID, string Param)
        {
            if (selectPcInputsQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, string, IEnumerable<PcInputs>>> expression =
                    (ctx, company_ex, configID_ex, Param_ex) =>
                        (from row in ctx.PcInputs
                         where row.Company == company_ex &&
                         row.ConfigID == configID_ex &&
                         (("~" + Param_ex + "~").IndexOf("~" + row.ControlType + "~") == -1)
                         select row);
                selectPcInputsQuery_3 = DBExpressionCompiler.Compile(expression);
            }
            return selectPcInputsQuery_3(this.Db, company, configID, Param);
        }


        static Func<ErpContext, string, string, string, bool> existsPcInputLayeredImageQuery;
        private bool ExistsPcInputLayeredImage(string company, string configID, string designControlType)
        {
            if (existsPcInputLayeredImageQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
                    (ctx, company_ex, configID_ex, designControlType_ex) =>
                        (from row in ctx.PcInputs
                         where row.Company == company_ex &&
                         row.ConfigID == configID_ex &&
                         (("~" + designControlType_ex + "~").IndexOf("~" + row.DesignControlType + "~") != -1)
                         select row).Any();
                existsPcInputLayeredImageQuery = DBExpressionCompiler.Compile(expression);
            }
            return existsPcInputLayeredImageQuery(this.Db, company, configID, designControlType);
        }
        #endregion PcInputs Queries

        #region PcPage Queries

        private class PageSummaryInfo
        {
            public string PageTitle { get; set; }
            public int DisplaySeq { get; set; }
        }

        static Func<ErpContext, string, string, int, PageSummaryInfo> getPageTitleQuery;
        private PageSummaryInfo GetPageTitle(string company, string configID, int pageSeq)
        {
            if (getPageTitleQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, PageSummaryInfo>> expression =
                    (ctx, company_ex, configID_ex, pageSeq_ex) =>
                        (from row in ctx.PcPage
                         where row.Company == company_ex &&
                         row.ConfigID == configID_ex &&
                         row.PageSeq == pageSeq_ex
                         select new PageSummaryInfo { PageTitle = row.PageTitle, DisplaySeq = row.DisplaySeq }).FirstOrDefault();
                getPageTitleQuery = DBExpressionCompiler.Compile(expression);
            }
            return getPageTitleQuery(this.Db, company, configID, pageSeq);
        }
        #endregion

        #region PcRuleSet Queries

        private static Func<ErpContext, string, string, string, string, bool> findPcRulesSetQuery;
        private bool FindPcRulesSet(string company, string configID, string partNum, string revisionNum)
        {
            if (findPcRulesSetQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, bool>> expression =
                    (ctx, company_ex, configID_ex, partNum_ex, revisionNum_ex) =>
                        (from row in ctx.PcRuleSet
                         where row.Company == company_ex &&
                         row.ConfigID == configID_ex &&
                         row.PartNum == partNum_ex &&
                         row.RevisionNum == revisionNum_ex
                         select row).Any();
                findPcRulesSetQuery = DBExpressionCompiler.Compile(expression);
            }
            return findPcRulesSetQuery(this.Db, company, configID, partNum, revisionNum);
        }
        #endregion

        #region PcStatus Queries
        private class PcStatusEWCResult
        {
            public string Company { get; set; }
            public string ConfigID { get; set; }
            public string ConfigType { get; set; }
            public bool ExtConfig { get; set; }
            public int ConfigVersion { get; set; }
            public bool Approved { get; set; }
            public bool EWCClientSyncRequired { get; set; }
        }


        static Func<ErpContext, string, string, PcStatusEWCResult> findFirstPcStatusSmallQuery;
        private PcStatusEWCResult FindFirstPcStatusEWCResult(string company, string configID)
        {
            if (findFirstPcStatusSmallQuery == null)
            {
                Expression<Func<ErpContext, string, string, PcStatusEWCResult>> expression =
                    (ctx, company_ex, configID_ex) =>
                      (from row in ctx.PcStatus
                       where row.Company == company_ex &&
                       row.ConfigID == configID_ex
                       select new PcStatusEWCResult
                       {
                           Company = row.Company,
                           ConfigID = row.ConfigID,
                           ConfigType = row.ConfigType,
                           ExtConfig = row.ExtConfig,
                           ConfigVersion = row.ConfigVersion,
                           Approved = row.Approved,
                           EWCClientSyncRequired = row.EWCClientSyncRequired

                       }).FirstOrDefault();
                findFirstPcStatusSmallQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPcStatusSmallQuery(this.Db, company, configID);
        }




        static Func<ErpContext, string, string, PcStatus> findFirstPcStatusQuery;
        private PcStatus FindFirstPcStatus(string company, string configID)
        {
            if (findFirstPcStatusQuery == null)
            {
                Expression<Func<ErpContext, string, string, PcStatus>> expression =
                    (ctx, company_ex, configID_ex) =>
                      (from row in ctx.PcStatus
                       where row.Company == company_ex &&
                       row.ConfigID == configID_ex
                       select row).FirstOrDefault();
                findFirstPcStatusQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPcStatusQuery(this.Db, company, configID);
        }


        private class PcStatusResult
        {
            public string Company { get; set; }
            public string ConfigID { get; set; }
            public string ConfigType { get; set; }
            public string StringStyle { get; set; }
            public string Separator { get; set; }
            public bool Approved { get; set; }
            public bool SaveInputValues { get; set; }
            public bool GenerateMethod { get; set; }
            public bool CreateRev { get; set; }
            public bool PrefacePart { get; set; }
            public bool CrtCustPart { get; set; }
            public string NumberFormat { get; set; }
            public int StartNumber { get; set; }
            public bool DispConfSummary { get; set; }
        }

        static Func<ErpContext, string, string, PcStatusResult> findFirstPcStatusResultQuery;
        private PcStatusResult FindFirstPcStatusResult(string company, string configID)
        {
            if (findFirstPcStatusResultQuery == null)
            {
                Expression<Func<ErpContext, string, string, PcStatusResult>> expression =
                    (ctx, company_ex, configID_ex) =>
                      (from row in ctx.PcStatus
                       where row.Company == company_ex &&
                       row.ConfigID == configID_ex
                       select new PcStatusResult
                       {
                           Company = row.Company,
                           ConfigID = row.ConfigID,
                           ConfigType = row.ConfigType,
                           StringStyle = row.StringStyle,
                           Separator = row.Separator,
                           Approved = row.Approved,
                           SaveInputValues = row.SaveInputValues,
                           GenerateMethod = row.GenerateMethod,
                           CreateRev = row.CreateRev,
                           PrefacePart = row.PrefacePart,
                           CrtCustPart = row.CrtCustPart,
                           NumberFormat = row.NumberFormat,
                           StartNumber = row.StartNumber,
                           DispConfSummary = row.DispConfSummary

                       }).FirstOrDefault();
                findFirstPcStatusResultQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPcStatusResultQuery(this.Db, company, configID);
        }



        private class PcStatusEntResult
        {
            public bool ExtConfig { get; set; }
            public bool EntprsConf { get; set; }
        }

        static Func<ErpContext, string, string, PcStatusEntResult> findFirstPcStatusEntResultQuery;
        private PcStatusEntResult FindFirstPcStatusEntResult(string company, string configID)
        {
            if (findFirstPcStatusEntResultQuery == null)
            {
                Expression<Func<ErpContext, string, string, PcStatusEntResult>> expression =
                    (ctx, company_ex, configID_ex) =>
                      (from row in ctx.PcStatus
                       where row.Company == company_ex &&
                       row.ConfigID == configID_ex
                       select new PcStatusEntResult
                       {
                           ExtConfig = row.ExtConfig,
                           EntprsConf = row.EntprsConf
                       }).FirstOrDefault();
                findFirstPcStatusEntResultQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPcStatusEntResultQuery(this.Db, company, configID);
        }


        class PcStatusCols
        {
            public string ConfigID { get; set; }
        }
        static Func<ErpContext, Guid, PcStatusCols> findFirstPcStatusQueryCols;
        private PcStatusCols FindFirstPcStatus(Guid sysRowID)
        {
            if (findFirstPcStatusQueryCols == null)
            {
                Expression<Func<ErpContext, Guid, PcStatusCols>> expression =
                    (ctx, sysRowID_ex) =>
                      (from row in ctx.PcStatus
                       where row.SysRowID == sysRowID_ex
                       select new PcStatusCols
                       {
                           ConfigID = row.ConfigID
                       }).FirstOrDefault();
                findFirstPcStatusQueryCols = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPcStatusQueryCols(this.Db, sysRowID);
        }

        static Func<ErpContext, string, string, string, PcStatusCols> findFirstPcStatusQuery2;
        private PcStatusCols FindFirstPcStatus(string company, string partNum, string revisionNum)
        {
            if (findFirstPcStatusQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, string, PcStatusCols>> expression =
                    (ctx, company_ex, partNum_ex, revisionNum_ex) =>
                      (from row in ctx.PartRev
                       where row.Company == company_ex &&
                       row.PartNum == partNum_ex &&
                       row.RevisionNum == revisionNum_ex
                       select new PcStatusCols
                       {
                           ConfigID = row.ConfigID
                       }).FirstOrDefault();
                findFirstPcStatusQuery2 = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPcStatusQuery2(this.Db, company, partNum, revisionNum);
        }

        static Func<ErpContext, string, string, bool> canfindFirstPcStatusQuery;
        private bool ExistsPcStatus(string company, string configID)
        {
            if (canfindFirstPcStatusQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (ctx, company_ex, configID_ex) =>
                      (from row in ctx.PcStatus
                       where row.Company == company_ex &&
                       row.ConfigID == configID_ex
                       select row).Any();
                canfindFirstPcStatusQuery = DBExpressionCompiler.Compile(expression);
            }
            return canfindFirstPcStatusQuery(this.Db, company, configID);
        }

        static Func<ErpContext, string, string, bool> isEWCQuery;
        private bool IsEWC(string company, string configID)
        {
            if (isEWCQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (ctx, company_ex, configID_ex) =>
                      (from row in ctx.PcStatus
                       where row.Company == company_ex &&
                       row.ConfigID == configID_ex &&
                       row.ConfigType == "EWC"
                       select row).Any();
                isEWCQuery = DBExpressionCompiler.Compile(expression);
            }
            return isEWCQuery(this.Db, company, configID);
        }

        class EnterprisePcStatusCols
        {
            public string configType { get; set; }
            public bool extConfig { get; set; }
            public bool EWCClientSyncRequired { get; set; }
        }
        static Func<ErpContext, string, string, bool> existsEnterprisePcStatusQuery;
        private bool ExistsEnterprisePcStatus(string company, string configID)
        {
            if (existsEnterprisePcStatusQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (ctx, company_ex, configID_ex) =>
                      (from row in ctx.PcStatus
                       where row.Company == company_ex &&
                       row.ConfigID == configID_ex &&
                       row.ConfigType == "EWC" &&
                       row.ExtConfig == true
                       select row).Any();
                existsEnterprisePcStatusQuery = DBExpressionCompiler.Compile(expression);
            }
            return existsEnterprisePcStatusQuery(this.Db, company, configID);
        }
        #endregion PcStatus Queries

        #region PcStatusExpr Queries
        private static Func<ErpContext, string, string, bool> existsPricingExpression;
        private bool ExistsPricingExpression(string company, string configID)
        {
            if (existsPricingExpression == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (ctx, company_ex, configID_ex) =>
                        (from row in ctx.PcDocRulesExpr
                         where row.Company == company_ex &&
                         row.ConfigID == configID_ex &&
                         (row.ExprType.Equals("SetTargetEntityPricingExpression") || row.ExprType.Equals("SetTargetEntityPricing"))
                         select row).Any();
                existsPricingExpression = DBExpressionCompiler.Compile(expression);
            }
            return existsPricingExpression(this.Db, company, configID);
        }

        #endregion PcStatusExpr Queries

        #region PcStrComp Queries

        static Func<ErpContext, string, string, IEnumerable<PcStrComp>> findSelectedPcStrCompQuery;
        private IEnumerable<PcStrComp> FindSmartStringValues(string company, string configID)
        {
            if (findSelectedPcStrCompQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<PcStrComp>>> expression =
                    (ctx, company_ex, configID_ex) =>
                        (from row in ctx.PcStrComp
                         where row.Company == company_ex &&
                         row.ConfigID == configID_ex
                         orderby row.PosOrder
                         select row);
                findSelectedPcStrCompQuery = DBExpressionCompiler.Compile(expression);
            }
            return findSelectedPcStrCompQuery(this.Db, company, configID);
        }
        #endregion PcStrComp Queries

        #region PcTargetEntity Queries
        private class PcTargetEntityCols
        {
            public bool IncomingSmartString { get; set; }
            public bool AllowRecordCreation { get; set; }
        }
        Func<ErpContext, string, string, string, PcTargetEntityCols> findPcTargetEntity;
        private PcTargetEntityCols FindPcTargetEntity(string company, string configID, string tableName)
        {
            if (findPcTargetEntity == null)
            {
                Expression<Func<ErpContext, string, string, string, PcTargetEntityCols>> expression =
                    (ctx, company_ex, configID_ex, tableName_ex) =>
                        (from row in ctx.PcTargetEntity
                         where row.Company == company_ex
                            && row.ConfigID == configID_ex
                            && row.TableName == tableName_ex
                         select new PcTargetEntityCols
                         {
                             IncomingSmartString = row.IncomingSmartString,
                             AllowRecordCreation = row.AllowRecordCreation
                         }).FirstOrDefault();
                findPcTargetEntity = DBExpressionCompiler.Compile(expression);
            }
            return findPcTargetEntity(Db, company, configID, tableName);
        }

        static Func<ErpContext, string, string, bool> existsDemandAllowIncomingQuery;
        private bool ExistsDemandAllowIncoming(string company, string configID)
        {
            if (existsDemandAllowIncomingQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (ctx, company_ex, configID_ex) =>
                        (from row in ctx.PcTargetEntity
                         where row.Company == company_ex &&
                         row.ConfigID == configID_ex &&
                         row.TableName == "DemandDetail" &&
                         row.IncomingSmartString == true
                         select row).Any();

                existsDemandAllowIncomingQuery = DBExpressionCompiler.Compile(expression);
            }
            return existsDemandAllowIncomingQuery(Db, company, configID);
        }
        #endregion PcTargetEntity Queries

        #region PcValueGrp Queries
        Func<ErpContext, string, int, PcValueGrp> findPcValueGrp;
        private PcValueGrp FindFirstPcValueGrp(string company, int groupSeq)
        {
            if (findPcValueGrp == null)
            {
                Expression<Func<ErpContext, string, int, PcValueGrp>> expression =
                    (ctx, company_ex, groupSeq_ex) =>
                        (from row in ctx.PcValueGrp
                         where row.Company == company_ex &&
                         row.GroupSeq == groupSeq_ex
                         select row).FirstOrDefault();
                findPcValueGrp = DBExpressionCompiler.Compile(expression);
            }
            return findPcValueGrp(Db, company, groupSeq);
        }
        #endregion PcValueGrp Queries

        #region PcValueHead Queries

        Func<ErpContext, string, int, string, string, PcValueHead> findPcValueHeadByStructTag;
        private PcValueHead FindPcValueHeadByStructTag(string company, int groupSeq, string structTag, string configID)
        {
            if (findPcValueHeadByStructTag == null)
            {
                Expression<Func<ErpContext, string, int, string, string, PcValueHead>> expression =
                    (ctx, company_ex, groupSeq_ex, structTag_ex, configID_ex) =>
                        (from row in ctx.PcValueHead
                         where row.Company == company_ex &&
                         row.GroupSeq == groupSeq_ex &&
                         row.ConfigID == configID_ex &&
                         row.StructTag == structTag_ex
                         select row).FirstOrDefault();
                findPcValueHeadByStructTag = DBExpressionCompiler.Compile(expression);
            }
            return findPcValueHeadByStructTag(Db, company, groupSeq, structTag, configID);
        }

        static Func<ErpContext, string, int, string, string, int, bool> existPcValueHeadQuery;
        private bool ExistPcValueHead(string companyID, int groupSeq, string structTag, string configID, int revolvingSeq)
        {
            if (existPcValueHeadQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, bool>> expression =
                  (ctx, company_ex, groupSeq_ex, structTag_ex, configID_ex, revolvingSeq_ex) =>
                    (from row in ctx.PcValueHead
                     where row.Company == company_ex &&
                        row.GroupSeq == groupSeq_ex &&
                        row.ConfigID == configID_ex &&
                        row.StructTag == structTag_ex &&
                        row.RevolvingSeq == revolvingSeq_ex
                     select row).Any();
                existPcValueHeadQuery = DBExpressionCompiler.Compile(expression);
            }
            return existPcValueHeadQuery(this.Db, companyID, groupSeq, structTag, configID, revolvingSeq);
        }

        static Func<ErpContext, string, int, string, string, int, PcValueHead> findFirstPcValueHeadWithUpLockQuery;
        private PcValueHead FindFirstPcValueHeadWithUpLock(string companyID, int groupSeq, string structTag, string configID, int revolvingSeq)
        {
            if (findFirstPcValueHeadWithUpLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, PcValueHead>> expression =
                  (ctx, company_ex, groupSeq_ex, structTag_ex, configID_ex, revolvingSeq_ex) =>
                    (from row in ctx.PcValueHead.With(LockHint.UpdLock)
                     where row.Company == company_ex &&
                        row.GroupSeq == groupSeq_ex &&
                        row.ConfigID == configID_ex &&
                        row.StructTag == structTag_ex &&
                        row.RevolvingSeq == revolvingSeq_ex
                     select row).FirstOrDefault();
                findFirstPcValueHeadWithUpLockQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPcValueHeadWithUpLockQuery(this.Db, companyID, groupSeq, structTag, configID, revolvingSeq);
        }

        static Func<ErpContext, string, int, string, string, IEnumerable<PcValueHead>> selectPcValueHeadWithUpLock;
        private IEnumerable<PcValueHead> SelectPcValueHeadWithUpLock(string company, int groupSeq, string structTag, string ruleTag)
        {
            if (selectPcValueHeadWithUpLock == null)
            {
                Expression<Func<ErpContext, string, int, string, string, IEnumerable<PcValueHead>>> expression =
                    (ctx, company_ex, groupSeq_ex, structTag_ex, ruleTag_ex) =>
                        (from row in ctx.PcValueHead.With(LockHint.UpdLock)
                         where row.Company == company_ex &&
                         row.GroupSeq == groupSeq_ex &&
                         row.StructTag.StartsWith(structTag_ex) &&
                         row.RuleTag.StartsWith(ruleTag_ex)
                         select row);

                selectPcValueHeadWithUpLock = DBExpressionCompiler.Compile(expression);
            }
            return selectPcValueHeadWithUpLock(Db, company, groupSeq, structTag, ruleTag);
        }
        static Func<ErpContext, string, int, IEnumerable<PcValueHead>> selectPcValueHeadForSummaryQuery;
        private IEnumerable<PcValueHead> SelectPcValueHeadForSummary(string company, int groupSeq)
        {
            if (selectPcValueHeadForSummaryQuery == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<PcValueHead>>> expression =
                    (ctx, company_ex, groupSeq_ex) =>
                        (from row in ctx.PcValueHead
                         where row.Company == company_ex &&
                         row.GroupSeq == groupSeq_ex
                         select row);

                selectPcValueHeadForSummaryQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectPcValueHeadForSummaryQuery(Db, company, groupSeq);
        }

        #endregion PcValueHead Queries

        #region PcValueSet Queries

        static Func<ErpContext, string, string, Guid, IEnumerable<PcValueSet>> getPcValueSetsWithLockQuery;
        private IEnumerable<PcValueSet> GetPcValueSetsWithLock(string company, string inputName, Guid sysRowID)
        {
            if (getPcValueSetsWithLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, Guid, IEnumerable<PcValueSet>>> expression =
                    (ctx, company_ex, inputName_ex, sysRowID_ex) =>
                        (from rowGrp in ctx.PcValueGrp
                         where rowGrp.Company == company_ex &&
                         rowGrp.RelatedToSysRowID == sysRowID_ex
                         join rowHead in ctx.PcValueHead on new { rowGrp.Company, rowGrp.GroupSeq } equals new { rowHead.Company, rowHead.GroupSeq } into JoinedHead
                         from rowJoinedHead in JoinedHead
                         join rowInput in ctx.PcInputs on new { rowJoinedHead.Company, rowJoinedHead.ConfigID } equals new { rowInput.Company, rowInput.ConfigID } into JoinedInput
                         from rowJndInput in JoinedInput
                         where rowJndInput.InputName == inputName_ex &&
                         rowJndInput.IsGlobal
                         join rowValue in ctx.PcValueSet on new { rowJoinedHead.Company, rowJoinedHead.GroupSeq, rowJoinedHead.HeadNum, rowJndInput.PageSeq } equals new { rowValue.Company, rowValue.GroupSeq, rowValue.HeadNum, rowValue.PageSeq } into JoinedValue
                         from row in JoinedValue
                         select row);
                getPcValueSetsWithLockQuery = DBExpressionCompiler.Compile(expression);
            }
            return getPcValueSetsWithLockQuery(this.Db, company, inputName, sysRowID);
        }

        Func<ErpContext, string, int, int, IEnumerable<PcValueSet>> getPcValueSetRecords;
        private IEnumerable<PcValueSet> GetPcValueSetRecords(string company, int groupSeq, int headNum)
        {
            if (getPcValueSetRecords == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<PcValueSet>>> expression =
                    (ctx, companyID_ex, groupSeq_ex, headNum_ex) =>
                        (from row in ctx.PcValueSet
                         where row.Company == companyID_ex &&
                         row.GroupSeq == groupSeq_ex &&
                         row.HeadNum == headNum_ex
                         select row);
                getPcValueSetRecords = DBExpressionCompiler.Compile(expression);
            }
            return getPcValueSetRecords(Db, company, groupSeq, headNum);
        }

        static Func<ErpContext, string, int, int, IEnumerable<PcValueSet>> updatePCValueSetQuery;
        private IEnumerable<PcValueSet> UpdatePCValueSet(string company, int groupSeq, int headNum)
        {
            if (updatePCValueSetQuery == null)
            {

                Expression<Func<ErpContext, string, int, int, IEnumerable<PcValueSet>>> expression =
                    (ctx, company_ex, groupSeq_ex, headnum_ex) =>
                        (from row in ctx.PcValueSet.With(LockHint.UpdLock)
                         where row.Company == company_ex &&
                         row.GroupSeq == groupSeq_ex &&
                         row.HeadNum == headnum_ex
                         select row);
                updatePCValueSetQuery = DBExpressionCompiler.Compile(expression);
            }
            return updatePCValueSetQuery(this.Db, company, groupSeq, headNum);
        }

        static Func<ErpContext, string, int, bool> existsPcValueSetQuery;
        private bool ExistsPcValueSet(string company, int groupSeq)
        {
            if (existsPcValueSetQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool>> expression =
                (ctx, company_ex, groupSeq_ex) =>
                  (from row in ctx.PcValueSet
                   where row.Company == company_ex &&
                       row.GroupSeq == groupSeq_ex
                   select row).Any();
                existsPcValueSetQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPcValueSetQuery(this.Db, company, groupSeq);
        }
        #endregion PcValueSet Queries

        #region PODetail Queries
        private class PODetailCols
        {
            public int PONum { get; set; }
            public decimal ConfigBaseUnitPrice { get; set; }
            public decimal ConfigUnitPrice { get; set; }
            public string BasePartNum { get; set; }
            public string BaseRevisionNum { get; set; }
            public int GroupSeq { get; set; }
        }
        static Func<ErpContext, string, Guid, PODetailCols> findPODetailBySysRowIDQuery;
        private PODetailCols FindPODetailBySysRowID(string company, Guid sysRowID)
        {
            if (findPODetailBySysRowIDQuery == null)
            {
                Expression<Func<ErpContext, string, Guid, PODetailCols>> expression =
                    (ctx, company_ex, sysRowID_ex) =>
                        (from row in ctx.PODetail
                         where row.Company == company_ex &&
                         row.SysRowID == sysRowID_ex
                         select new PODetailCols
                         {
                             PONum = row.PONUM,
                             ConfigBaseUnitPrice = row.ConfigBaseUnitCost,
                             ConfigUnitPrice = row.ConfigUnitCost,
                             BasePartNum = row.BasePartNum,
                             BaseRevisionNum = row.BaseRevisionNum,
                             GroupSeq = row.GroupSeq
                         }).FirstOrDefault();
                findPODetailBySysRowIDQuery = DBExpressionCompiler.Compile(expression);
            }
            return findPODetailBySysRowIDQuery(this.Db, company, sysRowID);
        }

        static Func<ErpContext, string, int, IEnumerable<SysRowIDResult>> selectPODetailQuery;
        private IEnumerable<SysRowIDResult> SelectPODetail(string company, int pordernum)
        {
            if (selectPODetailQuery == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<SysRowIDResult>>> expression =
                    (ctx, company_ex, pordernum_ex) =>
                        (from row in ctx.PODetail
                         where row.Company == company_ex &&
                         row.PONUM == pordernum_ex
                         select new SysRowIDResult { SysRowID = row.SysRowID });

                selectPODetailQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectPODetailQuery(this.Db, company, pordernum);
        }

        #endregion PODetail Queries

        #region POHeader Queries
        static Func<ErpContext, string, int, string> findPOCurrencyQuery;
        private string FindPOCurrency(string company, int ponum)
        {
            if (findPOCurrencyQuery == null)
            {
                Expression<Func<ErpContext, string, int, string>> expression =
                    (ctx, company_ex, ponum_ex) =>
                        (from row in ctx.POHeader
                         where row.Company == company_ex &&
                         row.PONum == ponum_ex
                         select row.CurrencyCode).FirstOrDefault();
                findPOCurrencyQuery = DBExpressionCompiler.Compile(expression);
            }
            return findPOCurrencyQuery(this.Db, company, ponum);
        }



        static Func<ErpContext, string, int, DateTime?> findPOOrderDateQuery;
        private DateTime? FindPOOrderDate(string company, int ponum)
        {
            if (findPOOrderDateQuery == null)
            {
                Expression<Func<ErpContext, string, int, DateTime?>> expression =
                    (ctx, company_ex, ponum_ex) =>
                        (from row in ctx.POHeader
                         where row.Company == company_ex &&
                         row.PONum == ponum_ex
                         select row.OrderDate).FirstOrDefault();
                findPOOrderDateQuery = DBExpressionCompiler.Compile(expression);
            }
            return findPOOrderDateQuery(this.Db, company, ponum);
        }
        #endregion POHeader Queries

        #region QuoteAsm Queries
        static Func<ErpContext, string, int, int, int, IEnumerable<SysRowIDResult>> selectQuoteAsmQuery;
        private IEnumerable<SysRowIDResult> SelectQuoteAsm(string company, int quoteNum, int quoteLine, int assemblySeq)
        {
            if (selectQuoteAsmQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, IEnumerable<SysRowIDResult>>> expression =
                    (ctx, company_ex, quoteNum_ex, quoteLine_ex, assemblySeq_ex) =>
                        (from row in ctx.QuoteAsm
                         where row.Company == company_ex &&
                         row.QuoteNum == quoteNum_ex &&
                         row.QuoteLine == quoteLine_ex &&
                         row.AssemblySeq != assemblySeq_ex
                         select new SysRowIDResult() { SysRowID = row.SysRowID });
                selectQuoteAsmQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectQuoteAsmQuery(this.Db, company, quoteNum, quoteLine, assemblySeq);
        }

        #endregion

        #region QuoteDtl Queries

        private class QuoteDetailCols
        {
            public int QuoteNum { get; set; }
            public int QuoteLine { get; set; }
            public decimal ConfigBaseUnitPrice { get; set; }
            public decimal ConfigUnitPrice { get; set; }
            public string BasePartNum { get; set; }
            public string BaseRevisionNum { get; set; }
            public DateTime? LastConfigDate { get; set; }
            public int LastConfigTime { get; set; }
            public string LastConfigUserID { get; set; }
            public int GroupSeq { get; set; }
        }
        static Func<ErpContext, string, Guid, QuoteDetailCols> findQuoteDtlBySysRowIDQuery;
        private QuoteDetailCols FindQuoteDtlBySysRowID(string company, Guid sysRowID)
        {
            if (findQuoteDtlBySysRowIDQuery == null)
            {
                Expression<Func<ErpContext, string, Guid, QuoteDetailCols>> expression =
                    (ctx, company_ex, sysRowID_ex) =>
                        (from row in ctx.QuoteDtl
                         where row.Company == company_ex &&
                         row.SysRowID == sysRowID_ex
                         select new QuoteDetailCols
                         {
                             QuoteNum = row.QuoteNum,
                             QuoteLine = row.QuoteLine,
                             ConfigBaseUnitPrice = row.ConfigBaseUnitPrice,
                             ConfigUnitPrice = row.ConfigUnitPrice,
                             BasePartNum = row.BasePartNum,
                             BaseRevisionNum = row.BaseRevisionNum,
                             LastConfigDate = row.LastConfigDate,
                             LastConfigTime = row.LastConfigTime,
                             LastConfigUserID = row.LastConfigUserID,
                             GroupSeq = row.GroupSeq
                         }).FirstOrDefault();
                findQuoteDtlBySysRowIDQuery = DBExpressionCompiler.Compile(expression);
            }
            return findQuoteDtlBySysRowIDQuery(this.Db, company, sysRowID);
        }

        static Func<ErpContext, string, int, int, IEnumerable<SysRowIDResult>> selectQuoteDtlQuery;
        private IEnumerable<SysRowIDResult> SelectQuoteDtl(string company, int quoteNum, int quoteLine)
        {
            if (selectQuoteDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<SysRowIDResult>>> expression =
                    (ctx, company_ex, quoteNum_ex, quoteLine_ex) =>
                        (from row in ctx.QuoteDtl
                         where row.Company == company_ex &&
                         row.QuoteNum == quoteNum_ex &&
                         row.QuoteLine != quoteLine_ex
                         select new SysRowIDResult() { SysRowID = row.SysRowID });
                selectQuoteDtlQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectQuoteDtlQuery(this.Db, company, quoteNum, quoteLine);
        }




        #endregion QuoteDtl Queries

        #region QuoteHed Queries
        static Func<ErpContext, string, int, string> findQuoteCurrencyQuery;
        private string FindQuoteCurrency(string company, int quoteNum)
        {
            if (findQuoteCurrencyQuery == null)
            {
                Expression<Func<ErpContext, string, int, string>> expression =
                    (ctx, company_ex, quoteNum_ex) =>
                        (from row in ctx.QuoteHed
                         where row.Company == company_ex &&
                         row.QuoteNum == quoteNum_ex
                         select row.CurrencyCode).FirstOrDefault();
                findQuoteCurrencyQuery = DBExpressionCompiler.Compile(expression);
            }
            return findQuoteCurrencyQuery(this.Db, company, quoteNum);
        }
        #endregion

        #region QuoteMtl Queries
        static Func<ErpContext, string, int, int, int, int, IEnumerable<SysRowIDResult>> selectQuoteMtlQuery;
        private IEnumerable<SysRowIDResult> SelectQuoteMtl(string company, int quoteNum, int quoteLine, int assemblySeq, int mtlSeq)
        {
            if (selectQuoteMtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, int, IEnumerable<SysRowIDResult>>> expression =
                    (ctx, company_ex, quoteNum_ex, quoteLine_ex, assemblySeq_ex, mtlSeq_ex) =>
                        (from row in ctx.QuoteMtl
                         where row.Company == company_ex &&
                         row.QuoteNum == quoteNum_ex &&
                         row.QuoteLine == quoteLine_ex &&
                         row.AssemblySeq == assemblySeq_ex &&
                         row.MtlSeq != mtlSeq_ex
                         select new SysRowIDResult() { SysRowID = row.SysRowID });
                selectQuoteMtlQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectQuoteMtlQuery(this.Db, company, quoteNum, quoteLine, assemblySeq, mtlSeq);
        }

        #endregion

        #region ShipTo Queries

        private class ShipToCols
        {
            public bool DemandUseCustomerValues { get; set; }
            public bool OTSmartString { get; set; }
        }
        static Func<ErpContext, string, int, string, ShipToCols> findFirstShipToQuery;
        private ShipToCols FindFirstShipTo(string company, int custNum, string shipToNum)
        {
            if (findFirstShipToQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, ShipToCols>> expression =
                    (ctx, company_ex, custNum_ex, shipToNum_ex) =>
                        (from row in ctx.ShipTo
                         where row.Company == company_ex &&
                         row.CustNum == custNum_ex &&
                         row.ShipToNum == shipToNum_ex
                         select new ShipToCols
                         {
                             DemandUseCustomerValues = row.DemandUseCustomerValues,
                             OTSmartString = row.OTSmartString
                         }).FirstOrDefault();
                findFirstShipToQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstShipToQuery(this.Db, company, custNum, shipToNum);
        }
        #endregion ShipTo Queries

        #region xbSyst Queries
        static Func<ErpContext, string, string> getEWConfiguratorURLQuery;
        private string GetEWConfiguratorURL(string company)
        {
            if (getEWConfiguratorURLQuery == null)
            {
                Expression<Func<ErpContext, string, string>> expression =
                    (ctx, company_ex) =>
                        (from row in ctx.XbSyst
                         where row.Company == company_ex
                         select row.EWConfiguratorURL).FirstOrDefault();
                getEWConfiguratorURLQuery = DBExpressionCompiler.Compile(expression);
            }
            return getEWConfiguratorURLQuery(this.Db, company);
        }
        #endregion xbSysQueries

        #region FileStore Queries

        class FileStoreColumns
        {
            public Guid SysRowID;
            public string Category;
        }
        static Func<ErpContext, string, string, string, DateTime, FileStoreColumns> existsNewerFileQuery;
        private FileStoreColumns SelectFileStoreSysRowIDIfNewer(string TenantID, string Company, string FileName, DateTime LastModifiedUTC)
        {
            if (existsNewerFileQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, DateTime, FileStoreColumns>> expression =
                   (ctx, tenantID_ex, company_ex, fileName_ex, lastModifiedUTC_ex) =>
                     (from row in ctx.FileStore
                      where row.TenantID == tenantID_ex
                        && row.Company == company_ex
                        && row.FileName == fileName_ex
                        && row.ModifiedOn > lastModifiedUTC_ex
                      select new FileStoreColumns { SysRowID = row.SysRowID, Category = row.Category }).FirstOrDefault();
                existsNewerFileQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsNewerFileQuery(this.Db, TenantID, Company, FileName, LastModifiedUTC);
        }

        static Func<ErpContext, string, string, string, FileStoreColumns> selectFileStoreQuery;
        private FileStoreColumns SelectFileStoreQuery(string TenantID, string Company, string FileName)
        {
            if (selectFileStoreQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, FileStoreColumns>> expression =
                   (ctx, tenantID_ex, company_ex, fileName_ex) =>
                     (from row in ctx.FileStore
                      where row.TenantID == tenantID_ex
                        && row.Company == company_ex
                        && row.FileName == fileName_ex
                      select new FileStoreColumns { SysRowID = row.SysRowID }).FirstOrDefault();
                selectFileStoreQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectFileStoreQuery(this.Db, TenantID, Company, FileName);
        }

        static Func<ErpContext, string, string, string, FileStore> findFirstFileStoreWithFileNameAndUpdateLock;
        private FileStore FindFirstFileStoreByFileNameAndUpdateLock(string tenantID, string company, string fileName)
        {
            if (findFirstFileStoreWithFileNameAndUpdateLock == null)
            {
                Expression<Func<ErpContext, string, string, string, FileStore>> expression =
                    (ctx, tenantID_ex, company_ex, fileName_ex) =>
                        (from row in ctx.FileStore.With(LockHint.UpdLock)
                         where row.TenantID == tenantID_ex &&
                         row.Company == company_ex &&
                         row.FileName == fileName_ex
                         select row).FirstOrDefault();
                findFirstFileStoreWithFileNameAndUpdateLock = DBExpressionCompiler.Compile(expression);
            }
            return findFirstFileStoreWithFileNameAndUpdateLock(this.Db, tenantID, company, fileName);
        }
        #endregion FileStore Queries


    }
}
