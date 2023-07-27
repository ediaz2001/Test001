using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using Epicor.Data;
using Erp.Internal.AP;
using Erp.Tables;
using Ice.Tables;

namespace Erp.Services.BO
{
    public partial class APInvoiceSvc
    {
        private static Func<ErpContext, string, int, string, IEnumerable<APInvHedMscTax>> selectAPInvHedMscTaxQuery2;
        private IEnumerable<APInvHedMscTax> SelectAPInvHedMscTaxRefresh(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAPInvHedMscTaxQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvHedMscTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    from row in context.APInvHedMscTax
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.ECAcquisitionSeq == 0
                    select row;
                selectAPInvHedMscTaxQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedMscTaxQuery2(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, string, int, string, string, int, APInvHedMscTax> findFirstAPInvHedMscTaxWithUpdLockQuery;
        private APInvHedMscTax FindFirstAPInvHedMscTaxWithUpdLock(string Company, int VendorNum, string InvoiceNum, int MscNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (findFirstAPInvHedMscTaxWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, int, APInvHedMscTax>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, MscNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APInvHedMscTax.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.MscNum == MscNum_ex &&
                    row.TaxCode == TaxCode_ex &&
                    row.RateCode == RateCode_ex &&
                    row.ECAcquisitionSeq != EcAcq_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPInvHedMscTaxWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedMscTaxWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum, MscNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, int, int, Guid> findFirstAPInvHedMscTaxQuery;
        private Guid FindFirstAPInvHedMscTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int MscNum)
        {
            if (findFirstAPInvHedMscTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, Guid>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, MscNum_ex) =>
                    (from row in context.APInvMsc
                     where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.InvoiceLine == InvoiceLine_ex &&
                    row.MscNum == MscNum_ex
                     select row.SysRowID)
                    .FirstOrDefault();
                findFirstAPInvHedMscTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedMscTaxQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, MscNum);
        }

        private static Func<ErpContext, string, int, string, string, string, int, bool> existsAPInvHedMscTaxQuery5;
        private bool ExistsAPInvHedMscTax(string Company, int VendorNum, string InvoiceNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (existsAPInvHedMscTaxQuery5 == null)
            {
                Expression<Func<ErpContext, string, int, string, string, string, int, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APInvHedMscTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq == EcAcq_ex
                     select row)
                    .Any();
                existsAPInvHedMscTaxQuery5 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedMscTaxQuery5(this.Db, Company, VendorNum, InvoiceNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, int, string, string, int, bool> existsAPInvHedMscTaxQuery4;
        private bool ExistsAPInvHedMscTax(string Company, int VendorNum, string InvoiceNum, int MscNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (existsAPInvHedMscTaxQuery4 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, int, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, MscNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APInvHedMscTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.MscNum != MscNum_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq == EcAcq_ex
                     select row)
                    .Any();
                existsAPInvHedMscTaxQuery4 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedMscTaxQuery4(this.Db, Company, VendorNum, InvoiceNum, MscNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, int, string, string, IEnumerable<APInvHedMscTax>> selectAPInvHedMscTaxWithUpdLockQuery;
        private IEnumerable<APInvHedMscTax> SelectAPInvHedMscTaxWithUpdLock(string Company, int VendorNum, string InvoiceNum, int MscNum, string TaxCode, string RateCode)
        {
            if (selectAPInvHedMscTaxWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, IEnumerable<APInvHedMscTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, MscNum_ex, TaxCode_ex, RateCode_ex) =>
                    from row in context.APInvHedMscTax.With(LockHint.UpdLock)
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.MscNum == MscNum_ex &&
                    (row.TaxCode != TaxCode_ex || row.RateCode != RateCode_ex)
                    select row;
                selectAPInvHedMscTaxWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedMscTaxWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum, MscNum, TaxCode, RateCode);
        }

        private static Func<ErpContext, string, int, string, int, IEnumerable<APInvHedMscTax>> selectAPInvHedMscTaxAllWithUpdLockQuery;
        private IEnumerable<APInvHedMscTax> SelectAPInvHedMscTaxAllWithUpdLock(string Company, int VendorNum, string InvoiceNum, int MscNum)
        {
            if (selectAPInvHedMscTaxAllWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvHedMscTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, MscNum_ex) =>
                    from row in context.APInvHedMscTax.With(LockHint.UpdLock)
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.MscNum == MscNum_ex
                    select row;
                selectAPInvHedMscTaxAllWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedMscTaxAllWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum, MscNum);
        }


        private static Func<ErpContext, string, int, string, int, string, string, int, bool> existsAPInvHedMscTaxQuery3;
        private bool ExistsAPInvHedMscTaxOld(string Company, int VendorNum, string InvoiceNum, int MscNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (existsAPInvHedMscTaxQuery3 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, int, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, MscNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APInvHedMscTax
                     where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.MscNum == MscNum_ex &&
                    row.TaxCode == TaxCode_ex &&
                    row.RateCode == RateCode_ex &&
                    row.ECAcquisitionSeq == EcAcq_ex
                     select row)
                    .Any();
                existsAPInvHedMscTaxQuery3 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedMscTaxQuery3(this.Db, Company, VendorNum, InvoiceNum, MscNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, int, string, string, Guid, bool> existsAPInvHedMscTaxQuery2;
        private bool ExistsAPInvHedMscTax(string Company, int VendorNum, string InvoiceNum, int MscNum, string TaxCode, string RateCode, Guid SysRowID)
        {
            if (existsAPInvHedMscTaxQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, Guid, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, MscNum_ex, TaxCode_ex, RateCode_ex, SysRowID_ex) =>
                    (from row in context.APInvHedMscTax
                     where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.MscNum == MscNum_ex &&
                    row.TaxCode == TaxCode_ex &&
                    row.RateCode == RateCode_ex &
                    row.SysRowID != SysRowID_ex
                     select row)
                    .Any();
                existsAPInvHedMscTaxQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedMscTaxQuery2(this.Db, Company, VendorNum, InvoiceNum, MscNum, TaxCode, RateCode, SysRowID);
        }

        private static Func<ErpContext, string, int, string, int, string, bool> existsAPInvHedMscTaxQuery;
        private bool ExistsAPInvHedMscTax(string Company, int VendorNum, string InvoiceNum, int MscNum, string TaxCode)
        {
            if (existsAPInvHedMscTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, MscNum_ex, TaxCode_ex) =>
                    (from row in context.APInvHedMscTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.MscNum == MscNum_ex &&
                     row.TaxCode == TaxCode_ex
                     select row)
                    .Any();
                existsAPInvHedMscTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedMscTaxQuery(this.Db, Company, VendorNum, InvoiceNum, MscNum, TaxCode);
        }


        private class APInvHedMscTaxPartialRow : Epicor.Data.TempRowBase
        {
            public int VendorNum { get; set; }
            public string Company { get; set; }
            public string InvoiceNum { get; set; }
            public int MscNum { get; set; }
            public string TaxCode { get; set; }
            public string RateCode { get; set; }
            public int ECAcquisitionSeq { get; set; }
            public decimal DocTaxAmt { get; set; }
            public decimal TaxAmt { get; set; }
            public decimal Rpt1TaxAmt { get; set; }
            public decimal Rpt2TaxAmt { get; set; }
            public decimal Rpt3TaxAmt { get; set; }
            public int CollectionType { get; set; }
            public decimal TaxAmtVar { get; set; }
            public decimal DocTaxAmtVar { get; set; }
            public decimal Rpt1TaxAmtVar { get; set; }
            public decimal Rpt2TaxAmtVar { get; set; }
            public decimal Rpt3TaxAmtVar { get; set; }
            public decimal DedTaxAmt { get; set; }
            public decimal DocDedTaxAmt { get; set; }
            public decimal Rpt1DedTaxAmt { get; set; }
            public decimal Rpt2DedTaxAmt { get; set; }
            public decimal Rpt3DedTaxAmt { get; set; }
        }

        private static Func<ErpContext, string, int, string, int, string, string, int, IEnumerable<APInvHedMscTaxPartialRow>> selectAPInvHedMscTaxQuery;
        private IEnumerable<APInvHedMscTaxPartialRow> SelectAPInvHedMscTax(string Company, int VendorNum, string InvoiceNum, int MscNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (selectAPInvHedMscTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, int, IEnumerable<APInvHedMscTaxPartialRow>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, MscNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    from row in context.APInvHedMscTax
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.ECAcquisitionSeq == EcAcq_ex &&
                    row.MscNum == MscNum_ex &&
                    (row.TaxCode != TaxCode_ex || row.RateCode != RateCode_ex) &&
                    row.ECAcquisitionSeq == EcAcq_ex
                    select new APInvHedMscTaxPartialRow { VendorNum = row.VendorNum, Company = row.Company, InvoiceNum = row.InvoiceNum, MscNum = row.MscNum, TaxCode = row.TaxCode, RateCode = row.RateCode, ECAcquisitionSeq = row.ECAcquisitionSeq, DocTaxAmt = row.DocTaxAmt, TaxAmt = row.TaxAmt };
                selectAPInvHedMscTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedMscTaxQuery(this.Db, Company, VendorNum, InvoiceNum, MscNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, int, IEnumerable<APInvHedMscTaxPartialRow>> selectAPInvHedMscTaxMscNumQuery;
        private IEnumerable<APInvHedMscTaxPartialRow> SelectAPInvHedMscTaxMscNum(string Company, int VendorNum, string InvoiceNum, int MscNum)
        {
            if (selectAPInvHedMscTaxMscNumQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvHedMscTaxPartialRow>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, MscNum_ex) =>
                    from row in context.APInvHedMscTax
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.MscNum == MscNum_ex
                    select new APInvHedMscTaxPartialRow { VendorNum = row.VendorNum, Company = row.Company, InvoiceNum = row.InvoiceNum, MscNum = row.MscNum, ECAcquisitionSeq = row.ECAcquisitionSeq, DocTaxAmt = row.DocTaxAmt, TaxAmt = row.TaxAmt, Rpt1TaxAmt = row.Rpt1TaxAmt, Rpt2TaxAmt = row.Rpt2TaxAmt, Rpt3TaxAmt = row.Rpt3TaxAmt, CollectionType = row.CollectionType, TaxAmtVar = row.TaxAmtVar, DocTaxAmtVar = row.DocTaxAmtVar, Rpt1TaxAmtVar = row.Rpt1TaxAmtVar, Rpt2TaxAmtVar = row.Rpt2TaxAmtVar, Rpt3TaxAmtVar = row.Rpt3TaxAmtVar, DedTaxAmt = row.DedTaxAmt, DocDedTaxAmt = row.DocDedTaxAmt, Rpt1DedTaxAmt = row.Rpt1DedTaxAmt, Rpt2DedTaxAmt = row.Rpt2DedTaxAmt, Rpt3DedTaxAmt = row.Rpt3DedTaxAmt };
                selectAPInvHedMscTaxMscNumQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedMscTaxMscNumQuery(this.Db, Company, VendorNum, InvoiceNum, MscNum);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<APInvLnMscTax>> selectAPInvLnMscTaxQuery3;
        private IEnumerable<APInvLnMscTax> SelectAPInvLnMscTaxRefresh(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAPInvLnMscTaxQuery3 == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvLnMscTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    from row in context.APInvLnMscTax
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.ECAcquisitionSeq == 0
                    select row;
                selectAPInvLnMscTaxQuery3 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvLnMscTaxQuery3(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, string, int, int, string, string, int, bool> existsAPInvLnMscTaxQuery5;
        private bool ExistsAPInvLnMscTaxOld(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int MscNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (existsAPInvLnMscTaxQuery5 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, string, string, int, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, MscNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APInvLnMscTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.MscNum == MscNum_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq == EcAcq_ex
                     select row)
                    .Any();
                existsAPInvLnMscTaxQuery5 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvLnMscTaxQuery5(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, MscNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, int, int, string, bool> existsAPInvLnMscTaxQuery4;
        private bool ExistsAPInvLnMscTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int MscNum, string TaxCode)
        {
            if (existsAPInvLnMscTaxQuery4 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, MscNum_ex, TaxCode_ex) =>
                    (from row in context.APInvLnMscTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.MscNum == MscNum_ex &&
                     row.TaxCode == TaxCode_ex
                     select row)
                    .Any();
                existsAPInvLnMscTaxQuery4 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvLnMscTaxQuery4(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, MscNum, TaxCode);
        }

        private class APInvLnMscTaxPartialRow : Epicor.Data.TempRowBase
        {
            public string Company { get; set; }
            public int VendorNum { get; set; }
            public string InvoiceNum { get; set; }
            public int InvoiceLine { get; set; }
            public int ECAcquisitionSeq { get; set; }
            public string TaxCode { get; set; }
            public string RateCode { get; set; }
            public decimal TaxAmt { get; set; }
            public decimal DocTaxAmt { get; set; }
        }

        private static Func<ErpContext, string, int, string, int, int, string, string, int, IEnumerable<APInvLnMscTaxPartialRow>> selectAPInvLnMscTaxQuery2;
        private IEnumerable<APInvLnMscTaxPartialRow> SelectAPInvLnMscTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int MscNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (selectAPInvLnMscTaxQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, string, string, int, IEnumerable<APInvLnMscTaxPartialRow>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, MscNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    from row in context.APInvLnMscTax
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.InvoiceLine == InvoiceLine_ex &&
                    row.ECAcquisitionSeq == EcAcq_ex &&
                    row.MscNum == MscNum_ex &&
                    (row.TaxCode != TaxCode_ex || row.RateCode != RateCode_ex) &&
                    row.ECAcquisitionSeq == EcAcq_ex
                    select new APInvLnMscTaxPartialRow { Company = row.Company, VendorNum = row.VendorNum, InvoiceNum = row.InvoiceNum, InvoiceLine = row.InvoiceLine, ECAcquisitionSeq = row.ECAcquisitionSeq, TaxCode = row.TaxCode, RateCode = row.RateCode, TaxAmt = row.TaxAmt, DocTaxAmt = row.DocTaxAmt };
                selectAPInvLnMscTaxQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvLnMscTaxQuery2(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, MscNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, string, string, int, bool> existsAPLnTaxQuery6;
        private bool ExistsAPLnTax(string Company, int VendorNum, string InvoiceNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (existsAPLnTaxQuery6 == null)
            {
                Expression<Func<ErpContext, string, int, string, string, string, int, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq == EcAcq_ex
                     select row)
                    .Any();
                existsAPLnTaxQuery6 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPLnTaxQuery6(this.Db, Company, VendorNum, InvoiceNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, int, int, string, string, int, bool> existsAPInvLnMscTaxQuery3;
        private bool ExistsAPInvLnMscTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int MscNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (existsAPInvLnMscTaxQuery3 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, string, string, int, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, MscNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APInvLnMscTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     (row.InvoiceLine != InvoiceLine_ex || row.MscNum != MscNum_ex) &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq == EcAcq_ex
                     select row)
                    .Any();
                existsAPInvLnMscTaxQuery3 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvLnMscTaxQuery3(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, MscNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, string, string, int, bool> existsAPInvLnMscTaxQuery2;
        private bool ExistsAPInvLnMscTax(string Company, int VendorNum, string InvoiceNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (existsAPInvLnMscTaxQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, string, string, int, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APInvLnMscTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq == EcAcq_ex
                     select row)
                    .Any();
                existsAPInvLnMscTaxQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvLnMscTaxQuery2(this.Db, Company, VendorNum, InvoiceNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, int, int, string, string, Guid, bool> existsAPInvLnMscTaxQuery;
        private bool ExistsAPInvLnMscTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int MscNum, string TaxCode, string RateCode, Guid SysRowID)
        {
            if (existsAPInvLnMscTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, string, string, Guid, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, MscNum_ex, TaxCode_ex, RateCode_ex, SysRowID_ex) =>
                    (from row in context.APInvLnMscTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.MscNum == MscNum_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &
                     row.SysRowID != SysRowID_ex
                     select row)
                    .Any();
                existsAPInvLnMscTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvLnMscTaxQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, MscNum, TaxCode, RateCode, SysRowID);
        }



        private static Func<ErpContext, string, int, string, int, int, string, string, int, APInvLnMscTax> findFirstAPInvLnMscTaxWithUpdLockQuery;
        private APInvLnMscTax FindFirstAPInvLnMscTaxWithUpdLock(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int MscNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (findFirstAPInvLnMscTaxWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, string, string, int, APInvLnMscTax>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, MscNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APInvLnMscTax.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.MscNum == MscNum_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq != EcAcq_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPInvLnMscTaxWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvLnMscTaxWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, MscNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, int, int, APInvMsc> findFirstAPInvMscWithUpdLockQuery;
        private APInvMsc FindFirstAPInvMscWithUpdLock(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int Mscnum)
        {
            if (findFirstAPInvMscWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, APInvMsc>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, Mscnum_ex) =>
                    (from row in context.APInvMsc.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.MscNum == Mscnum_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPInvMscWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvMscWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, Mscnum);
        }

        private static Func<ErpContext, string, int, string, int, int, string, string, IEnumerable<APInvLnMscTax>> selectAPInvLnMscTaxWithUpdLockQuery;
        private IEnumerable<APInvLnMscTax> SelectAPInvLnMscTaxWithUpdLock(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int MscNum, string TaxCode, string RateCode)
        {
            if (selectAPInvLnMscTaxWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, string, string, IEnumerable<APInvLnMscTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, MscNum_ex, TaxCode_ex, RateCode_ex) =>
                    from row in context.APInvLnMscTax.With(LockHint.UpdLock)
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.InvoiceLine == InvoiceLine_ex &&
                    row.MscNum == MscNum_ex &&
                    (row.TaxCode != TaxCode_ex || row.RateCode != RateCode_ex)
                    select row;
                selectAPInvLnMscTaxWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvLnMscTaxWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, MscNum, TaxCode, RateCode);
        }

        private static Func<ErpContext, string, int, string, int, int, IEnumerable<APInvLnMscTax>> selectAPInvLnMscTaxAllWithUpdLockQuery;
        private IEnumerable<APInvLnMscTax> SelectAPInvLnMscTaxAllWithUpdLock(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int MscNum)
        {
            if (selectAPInvLnMscTaxAllWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, IEnumerable<APInvLnMscTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, MscNum_ex) =>
                    from row in context.APInvLnMscTax.With(LockHint.UpdLock)
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.InvoiceLine == InvoiceLine_ex &&
                    row.MscNum == MscNum_ex
                    select row;
                selectAPInvLnMscTaxAllWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvLnMscTaxAllWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, MscNum);
        }

        private static Func<ErpContext, string, int, string, int, int, APInvMsc> findFirstAPInvMscQuery;
        private APInvMsc FindFirstAPInvMsc(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int MscNum)
        {
            if (findFirstAPInvMscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, APInvMsc>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, MscNum_ex) =>
                    (from row in context.APInvMsc
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.MscNum == MscNum_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPInvMscQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvMscQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, MscNum);
        }

        private static Func<ErpContext, string, int, string, int, IEnumerable<APLnTaxPartialRow2>> selectAPInvLnMscTaxQuery;
        private IEnumerable<APLnTaxPartialRow2> SelectAPInvLnMscTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectAPInvLnMscTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APLnTaxPartialRow2>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    from row in context.APInvLnMscTax
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.InvoiceLine == InvoiceLine_ex
                    select new APLnTaxPartialRow2 { Company = row.Company, VendorNum = row.VendorNum, InvoiceNum = row.InvoiceNum, InvoiceLine = row.InvoiceLine, DocTaxAmt = row.DocTaxAmt, TaxAmt = row.TaxAmt, Rpt1TaxAmt = row.Rpt1TaxAmt, Rpt2TaxAmt = row.Rpt2TaxAmt, Rpt3TaxAmt = row.Rpt3TaxAmt, CollectionType = row.CollectionType, ECAcquisitionSeq = row.ECAcquisitionSeq, TaxAmtVar = row.TaxAmtVar, DocTaxAmtVar = row.DocTaxAmtVar, Rpt1TaxAmtVar = row.Rpt1TaxAmtVar, Rpt2TaxAmtVar = row.Rpt2TaxAmtVar, Rpt3TaxAmtVar = row.Rpt3TaxAmtVar, DedTaxAmt = row.DedTaxAmt, DocDedTaxAmt = row.DocDedTaxAmt, Rpt1DedTaxAmt = row.Rpt1DedTaxAmt, Rpt2DedTaxAmt = row.Rpt2DedTaxAmt, Rpt3DedTaxAmt = row.Rpt3DedTaxAmt };
                selectAPInvLnMscTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvLnMscTaxQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }

        private static Func<ErpContext, string, int, string, int, int, IEnumerable<APLnTaxPartialRow2>> selectAPInvLnMscTaxMscNumQuery;
        private IEnumerable<APLnTaxPartialRow2> SelectAPInvLnMscTaxMscNum(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int MscNum)
        {
            if (selectAPInvLnMscTaxMscNumQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, IEnumerable<APLnTaxPartialRow2>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, MscNum_ex) =>
                    from row in context.APInvLnMscTax
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.InvoiceLine == InvoiceLine_ex &&
                    row.MscNum == MscNum_ex
                    select new APLnTaxPartialRow2 { Company = row.Company, VendorNum = row.VendorNum, InvoiceNum = row.InvoiceNum, InvoiceLine = row.InvoiceLine, DocTaxAmt = row.DocTaxAmt, TaxAmt = row.TaxAmt, Rpt1TaxAmt = row.Rpt1TaxAmt, Rpt2TaxAmt = row.Rpt2TaxAmt, Rpt3TaxAmt = row.Rpt3TaxAmt, CollectionType = row.CollectionType, ECAcquisitionSeq = row.ECAcquisitionSeq, TaxAmtVar = row.TaxAmtVar, DocTaxAmtVar = row.DocTaxAmtVar, Rpt1TaxAmtVar = row.Rpt1TaxAmtVar, Rpt2TaxAmtVar = row.Rpt2TaxAmtVar, Rpt3TaxAmtVar = row.Rpt3TaxAmtVar, DedTaxAmt = row.DedTaxAmt, DocDedTaxAmt = row.DocDedTaxAmt, Rpt1DedTaxAmt = row.Rpt1DedTaxAmt, Rpt2DedTaxAmt = row.Rpt2DedTaxAmt, Rpt3DedTaxAmt = row.Rpt3DedTaxAmt };
                selectAPInvLnMscTaxMscNumQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvLnMscTaxMscNumQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, MscNum);
        }



        private static Func<ErpContext, string, int, string, string, string, int, APInvTax> findFirstAPInvTaxQuery;
        private APInvTax FindFirstAPInvTax(string Company, int VendorNum, string InvoiceNum, string TaxCode, string RateCode, int EcAcq)
        {
            if (findFirstAPInvTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, string, int, APInvTax>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APInvTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq == EcAcq_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPInvTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvTaxQuery(this.Db, Company, VendorNum, InvoiceNum, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, APInvTax> findFirstAPInvTax2;
        private APInvTax FindFirstAPInvTax2(string Company, int VendorNum, string InvoiceNum)
        {
            if (findFirstAPInvTax2 == null)
            {
                Expression<Func<ErpContext, string, int, string, APInvTax>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APInvTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPInvTax2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvTax2(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, bool, bool> existsXbSystQuery;
        private bool ExistsXbSystLineTax(string Company, bool ApTaxLnLevel)
        {
            if (existsXbSystQuery == null)
            {
                Expression<Func<ErpContext, string, bool, bool>> expression =
                    (context, Company_ex, ApTaxLnLevel_ex) =>
                    (from row in context.XbSyst
                     where row.Company == Company_ex &&
                     row.APTaxLnLevel == ApTaxLnLevel_ex
                     select row)
                    .Any();
                existsXbSystQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsXbSystQuery(this.Db, Company, ApTaxLnLevel);
        }

        private static Func<ErpContext, string, int, string, bool> existsAPInvDtlQuery3;
        private bool ExistsAPInvDtl3(string Company, int VendorNum, string InvoiceNum)
        {
            if (existsAPInvDtlQuery3 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APInvDtl
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex
                     select row)
                    .Any();
                existsAPInvDtlQuery3 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvDtlQuery3(this.Db, Company, VendorNum, InvoiceNum);
        }

        static Func<ErpContext, string, string, string, string, bool> existsInvDtlLineQuery;
        private bool ExistsInvDtlLine(string company, string InvoiceNum, string invTypeRec, string invTypeMisc)
        {
            if (existsInvDtlLineQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, bool>> expression =
                (ctx, company_ex, InvoiceNum_ex, invTypeRec_ex, invTypeMisc_ex) =>
                (from row in ctx.APInvDtl
                 where row.Company == company_ex &&
                 row.InvoiceNum == InvoiceNum_ex &&
                 (row.LineType != invTypeRec_ex &&
                 row.LineType != invTypeMisc_ex)
                 select row).Any();
                existsInvDtlLineQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsInvDtlLineQuery(this.Db, company, InvoiceNum, invTypeRec, invTypeMisc);
        }

        static Func<ErpContext, string, int, int, int, bool> existUnreceivedLineForPOQuery;
        private bool ExistUnreceivedLineForPO(string company, int PONum, int POLine, int VendorNum)
        {
            if (existUnreceivedLineForPOQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, bool>> expression =
                (ctx, company_ex, PONum_ex, POLine_ex, VendorNum_ex) =>
                (from invDtlRow in ctx.APInvDtl
                 join poRelRow in ctx.PORel on
                     new { invDtlRow.Company, invDtlRow.PONum } equals
                     new { poRelRow.Company, poRelRow.PONum }
                 where invDtlRow.Company == company_ex &&
                       invDtlRow.VendorNum == VendorNum_ex &&
                       invDtlRow.PONum == PONum_ex &&
                       invDtlRow.POLine == POLine_ex &&
                       invDtlRow.LineType == "U" &&
                       invDtlRow.PORelNum == poRelRow.PORelNum

                 select true).Any();
                existUnreceivedLineForPOQuery = DBExpressionCompiler.Compile(expression);
            }

            return existUnreceivedLineForPOQuery(this.Db, company, PONum, POLine, VendorNum);
        }

        private class APInvTaxTotal
        {
            public decimal DocTaxAmt { get; set; }
            public decimal TaxAmt { get; set; }
        }

        private static Func<ErpContext, string, int, string, int, APInvTaxTotal> selectAPInvTaxTotalsQuery;
        private APInvTaxTotal GetAPInvTaxTotals(string Company, int VendorNum, string InvoiceNum, int CollectionType)
        {
            if (selectAPInvTaxTotalsQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, APInvTaxTotal>> expression =
                (context, Company_ex, VendorNum_ex, InvoiceNum_ex, CollectionType_ex) =>
                         (from invcTaxRow in context.APInvTax
                          join salesTaxRow in context.SalesTax on
                               new { invcTaxRow.Company, invcTaxRow.TaxCode } equals
                               new { salesTaxRow.Company, salesTaxRow.TaxCode }
                          where invcTaxRow.Company == Company_ex &&
                                invcTaxRow.VendorNum == VendorNum_ex &&
                                invcTaxRow.InvoiceNum == InvoiceNum_ex &&
                                invcTaxRow.CollectionType == CollectionType_ex &&
                                (salesTaxRow.DiscountType == 2 || salesTaxRow.DiscountType == 3) //payment discount treatment is "Term Discount Reduces Tax" or "Payment Discount Before Tax"
                          group invcTaxRow by new { invcTaxRow.Company, invcTaxRow.VendorNum, invcTaxRow.InvoiceNum } into taxes
                          select new APInvTaxTotal
                          {
                              DocTaxAmt = taxes.Sum(invoice => invoice.DocTaxAmt),
                              TaxAmt = taxes.Sum(invoice => invoice.TaxAmt)
                          }).FirstOrDefault();
                selectAPInvTaxTotalsQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvTaxTotalsQuery(this.Db, Company, VendorNum, InvoiceNum, CollectionType);
        }

        static Func<ErpContext, string, int, string, IEnumerable<APInvTax>> selectAPInvTaxQuery3;
        private IEnumerable<APInvTax> SelectAPInvTax3(string company, int vendorNum, string invoiceNum)
        {
            if (selectAPInvTaxQuery3 == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvTax>>> expression =
                  (ctx, company_ex, vendornum_ex, invoicenum_ex) =>
                    (from row in ctx.APInvTax
                     where row.Company == company_ex &&
                     row.VendorNum == vendornum_ex &&
                     row.InvoiceNum == invoicenum_ex
                     select row);
                selectAPInvTaxQuery3 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvTaxQuery3(this.Db, company, vendorNum, invoiceNum);
        }

        static Func<ErpContext, string, int, string, IEnumerable<APInvTax>> selectAPInvTaxQuery4;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Server", "QueryMustSpecifyCompany:Queries must be company specific.", Justification = "The query is already comparing the company, the '||' is confusing the rule recognition,the rule will be changed to recognize when the query uses an ||")]
        private IEnumerable<APInvTax> SelectAPInvTax4(string company, int vendorNum, string invoiceNum)
        {
            if (selectAPInvTaxQuery4 == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvTax>>> expression =
                  (ctx, company_ex, vendornum_ex, invoicenum_ex) =>
                    (from row in ctx.APInvTax
                     where row.Company == company_ex &&
                     row.VendorNum == vendornum_ex &&
                     row.InvoiceNum == invoicenum_ex &&
                     (row.ECAcquisitionSeq == 0 || row.ECAcquisitionSeq == 2)
                     select row);
                selectAPInvTaxQuery4 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvTaxQuery4(this.Db, company, vendorNum, invoiceNum);
        }

        private static Func<ErpContext, string, int, int, PODetail> findFirstPODetailQuery;
        private PODetail FindFirstPODetail(string company, int ponum, int poline)
        {
            if (findFirstPODetailQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, PODetail>> expression =
      (ctx, company_ex, ponum_ex, poline_ex) =>
        (from row in ctx.PODetail
         where row.Company == company_ex &&
         row.PONUM == ponum_ex &&
         row.POLine == poline_ex
         select row).FirstOrDefault();
                findFirstPODetailQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPODetailQuery(this.Db, company, ponum, poline);
        }

        private static Func<ErpContext, string, string, int, int, bool> existsSalesTaxQuery2;
        private bool ExistsSalesTax(string Company, string TaxCode, int CollectionType, int Timing)
        {
            if (existsSalesTaxQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
                    (context, Company_ex, TaxCode_ex, CollectionType_ex, Timing_ex) =>
                   (from row in context.SalesTax
                    where row.Company == Company_ex &&
                    row.TaxCode == TaxCode_ex &&
                    row.CollectionType == CollectionType_ex &&
                    row.Timing == Timing_ex
                    select row)
                    .Any();
                existsSalesTaxQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return existsSalesTaxQuery2(this.Db, Company, TaxCode, CollectionType, Timing);
        }

        private static Func<ErpContext, string, string, bool> salesTaxAllowsNonDeductibleQuery;
        private bool SalesTaxAllowsNonDeductible(string Company, string TaxCode)
        {
            if (salesTaxAllowsNonDeductibleQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (context, Company_ex, TaxCode_ex) =>
                   (from row in context.SalesTax
                    where row.Company == Company_ex &&
                    row.TaxCode == TaxCode_ex &&
                    row.CollectionType == 0 &&
                    (row.Timing == 0 ||
                    row.Timing == 1 ||
                    row.Timing == 2)
                    select row)
                    .Any();
                salesTaxAllowsNonDeductibleQuery = DBExpressionCompiler.Compile(expression);
            }

            return salesTaxAllowsNonDeductibleQuery(this.Db, Company, TaxCode);
        }

        private class TaxRgnSalesTaxPartialRow : Epicor.Data.TempRowBase
        {
            public string TaxCode { get; set; }
        }

        private static Func<ErpContext, string, string, IEnumerable<TaxRgnSalesTaxPartialRow>> selectTaxRgnSalesTaxQuery;
        private IEnumerable<TaxRgnSalesTaxPartialRow> SelectTaxRgnSalesTax(string Company, string TaxRgn)
        {
            if (selectTaxRgnSalesTaxQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<TaxRgnSalesTaxPartialRow>>> expression =
                    (context, Company_ex, TaxRgn_ex) =>
                    from row in context.TaxRgnSalesTax
                    where row.Company == Company_ex &&
                    row.TaxRegionCode == TaxRgn_ex
                    select new TaxRgnSalesTaxPartialRow { TaxCode = row.TaxCode };
                selectTaxRgnSalesTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectTaxRgnSalesTaxQuery(this.Db, Company, TaxRgn);
        }

        private static Func<ErpContext, string, int, string, bool, bool> existsAPInvHedQuery3;
        private bool ExistsAPInvHed(string Company, int VendorNum, string InvoiceNum, bool InPrice)
        {
            if (existsAPInvHedQuery3 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InPrice_ex) =>
                    (from row in context.APInvHed
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InPrice == InPrice_ex
                     select row)
                    .Any();
                existsAPInvHedQuery3 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedQuery3(this.Db, Company, VendorNum, InvoiceNum, InPrice);
        }

        private static Func<ErpContext, string, int, string, bool, bool> existsAPInvHedRecalcQuery;
        private bool ExistsAPInvHedRecalc(string Company, int VendorNum, string InvoiceNum, bool Recalc)
        {
            if (existsAPInvHedRecalcQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, Recalc_ex) =>
                    (from row in context.APInvHed
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.DevLog2 == Recalc_ex
                     select row)
                    .Any();
                existsAPInvHedRecalcQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedRecalcQuery
                (this.Db, Company, VendorNum, InvoiceNum, Recalc);
        }
        private static Func<ErpContext, string, int, string, string, bool, bool> existsRcvHeadQuery;
        private bool ExistsRcvHead(string company, int vendorNum, string purPoint, string packSlip, bool inPrice)
        {
            if (existsRcvHeadQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, bool, bool>> expression =
                    (context, Company_ex, vendorNum_ex, purPoint_ex, packSlip_ex, inPrice_ex) =>
                    (from row in context.RcvHead
                     where row.Company == Company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.PurPoint == purPoint_ex &&
                     row.PackSlip == packSlip_ex &&
                     row.InPrice == inPrice_ex
                     select row)
                    .Any();
                existsRcvHeadQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsRcvHeadQuery(this.Db, company, vendorNum, purPoint, packSlip, inPrice);
        }


        private class APInvDtlPartialRow4 : Epicor.Data.TempRowBase
        {
            public decimal DocExtCost { get; set; }
            public decimal DocInExtCost { get; set; }
            public decimal ExtCost { get; set; }
            public decimal InExtCost { get; set; }
            public decimal Rpt1ExtCost { get; set; }
            public decimal Rpt1InExtCost { get; set; }
            public decimal Rpt2ExtCost { get; set; }
            public decimal Rpt2InExtCost { get; set; }
            public decimal Rpt3ExtCost { get; set; }
            public decimal Rpt3InExtCost { get; set; }
        }

        private static Func<ErpContext, string, int, int, int, int, string, IEnumerable<APInvDtlPartialRow4>> selectAPInvDtlQuery3;
        private IEnumerable<APInvDtlPartialRow4> SelectAPInvDtl(string Company, int VendorNum, int PONum, int POLine, int PORel, string LineType)
        {
            if (selectAPInvDtlQuery3 == null)
            {
                Expression<Func<ErpContext, string, int, int, int, int, string, IEnumerable<APInvDtlPartialRow4>>> expression =
                    (context, Company_ex, VendorNum_ex, PONum_ex, POLine_ex, PORel_ex, LineType_ex) =>
                    from row in context.APInvDtl
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.PONum == PONum_ex &&
                    row.POLine == POLine_ex &&
                    row.PORelNum == PORel_ex &&
                    row.LineType == LineType_ex
                    select new APInvDtlPartialRow4 { DocExtCost = row.DocExtCost, DocInExtCost = row.DocInExtCost, ExtCost = row.ExtCost, InExtCost = row.InExtCost, Rpt1ExtCost = row.Rpt1ExtCost, Rpt1InExtCost = row.Rpt1InExtCost, Rpt2ExtCost = row.Rpt2ExtCost, Rpt2InExtCost = row.Rpt2InExtCost, Rpt3ExtCost = row.Rpt3ExtCost, Rpt3InExtCost = row.Rpt3InExtCost };
                selectAPInvDtlQuery3 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlQuery3(this.Db, Company, VendorNum, PONum, POLine, PORel, LineType);
        }


        private class POHeaderPartialRow : Epicor.Data.TempRowBase
        {
            public string APLOCID { get; set; }
            public int PONum { get; set; }
        }

        private static Func<ErpContext, string, int, int, POHeaderPartialRow> findFirstPartialPOHeaderQuery;
        private POHeaderPartialRow FindFirstPartialPOHeader(string Company, int PoNum, int VendorNum)
        {
            if (findFirstPartialPOHeaderQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, POHeaderPartialRow>> expression =
                    (context, Company_ex, PoNum_ex, VendorNum_ex) =>
                    (from row in context.POHeader
                     where row.Company == Company_ex &&
                     row.PONum == PoNum_ex &&
                     row.VendorNum == VendorNum_ex
                     select new POHeaderPartialRow { APLOCID = row.APLOCID, PONum = row.PONum })
                    .FirstOrDefault();
                findFirstPartialPOHeaderQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartialPOHeaderQuery(this.Db, Company, PoNum, VendorNum);
        }




        private class APInvDtlPartialRow3 : Epicor.Data.TempRowBase
        {
            public int PONum { get; set; }
            public int InvoiceLine { get; set; }
            public string APLOCID { get; set; }
        }

        private class APInvoiceHedAndDtl : Epicor.Data.TempRowBase
        {
            public APInvHed GRNIAPInvHed { get; set; }
            public APInvDtl GRNIAPInvDtl { get; set; }
        }

        private static Func<ErpContext, string, int, string, string, IEnumerable<APInvDtlPartialRow3>> selectAPInvDtlQuery2;
        private IEnumerable<APInvDtlPartialRow3> SelectAPInvDtl(string Company, int VendorNum, string InvoiceNum, string LineType)
        {
            if (selectAPInvDtlQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, string, IEnumerable<APInvDtlPartialRow3>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, LineType_ex) =>
                    from row in context.APInvDtl
                    join poHeader in context.POHeader
                    on new { row.Company, row.PONum } equals new { poHeader.Company, poHeader.PONum } into poHeaders
                    from poHeaderRow in poHeaders.DefaultIfEmpty()
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.LineType == LineType_ex
                    select new APInvDtlPartialRow3 { PONum = row.PONum, InvoiceLine = row.InvoiceLine, APLOCID = poHeaderRow.APLOCID };
                selectAPInvDtlQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlQuery2(this.Db, Company, VendorNum, InvoiceNum, LineType);
        }


        private static Func<ErpContext, string, int, int, IEnumerable<POMisc>> selectPOMiscQuery;
        private IEnumerable<POMisc> SelectPOMisc(string Company, int PONum, int PPLine)
        {
            if (selectPOMiscQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<POMisc>>> expression =
                    (context, Company_ex, PONum_ex, PPLine_ex) =>
                    from row in context.POMisc
                    where row.Company == Company_ex &&
                    row.PONum == PONum_ex &&
                    row.POLine == PPLine_ex
                    select row;
                selectPOMiscQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectPOMiscQuery(this.Db, Company, PONum, PPLine);
        }

        private class APInvExpPartialRow : Epicor.Data.TempRowBase
        {
            public string Company { get; set; }
            public decimal DocExpAmt { get; set; }
            public int InvoiceLine { get; set; }
            public string InvoiceNum { get; set; }
            public int VendorNum { get; set; }
        }

        static Func<ErpContext, string, int, string, VendBank> findFirstVendBankQuery_2;
        private VendBank FindFirstVendBank(string company, int vendorNum, string bankID)
        {
            if (findFirstVendBankQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, string, VendBank>> expression =
      (ctx, company_ex, vendorNum_ex, bankID_ex) =>
        (from row in ctx.VendBank
         where row.Company == company_ex &&
         row.VendorNum == vendorNum_ex &&
         row.BankID == bankID_ex
         select row).FirstOrDefault();
                findFirstVendBankQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstVendBankQuery_2(this.Db, company, vendorNum, bankID);
        }

        static Func<ErpContext, string, string, Vendor> findFirstVendorQuery_2;
        private Vendor FindFirstVendor(string company, string vendorID)
        {
            if (findFirstVendorQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, Vendor>> expression =
      (ctx, company_ex, vendorID_ex) =>
        (from row in ctx.Vendor
         where row.Company == company_ex &&
         row.VendorID == vendorID_ex
         select row).FirstOrDefault();
                findFirstVendorQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstVendorQuery_2(this.Db, company, vendorID);
        }

        private static Func<ErpContext, string, int, string, int, IEnumerable<APInvExpPartialRow>> selectAPInvExpQuery4;
        private IEnumerable<APInvExpPartialRow> SelectAPInvExp2(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectAPInvExpQuery4 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvExpPartialRow>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    from row in context.APInvExp
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.InvoiceLine == InvoiceLine_ex
                    select new APInvExpPartialRow { Company = row.Company, DocExpAmt = row.DocExpAmt, InvoiceLine = row.InvoiceLine, InvoiceNum = row.InvoiceNum, VendorNum = row.VendorNum };
                selectAPInvExpQuery4 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvExpQuery4(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }

        private static Func<ErpContext, string, int, string, int, IEnumerable<APInvJob>> selectAPInvJobWithUpdLockQuery;
        private IEnumerable<APInvJob> SelectAPInvJobWithUpdLock(string Company, int VendorNum, string InvoiceNum, int LineNum)
        {
            if (selectAPInvJobWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvJob>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, LineNum_ex) =>
                    from row in context.APInvJob.With(LockHint.UpdLock)
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.InvoiceLine == LineNum_ex
                    orderby row.ExtCost descending
                    select row;
                selectAPInvJobWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvJobWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum, LineNum);
        }

        private static Func<ErpContext, string, int, string, string, int, int, Guid, bool> existsOtherNRExpAPInvJobQuery;
        private bool ExistsOtherNRExpAPInvJob(string Company, int vendorNum, string invoiceNum, string jobNum, int asmSeq, int mtlSeq, Guid sysRowID)
        {
            if (existsOtherNRExpAPInvJobQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, int, Guid, bool>> expression =
                   (context, Company_ex, vendorNum_ex, invoiceNum_ex, jobNum_ex, asmSeq_ex, mtlSeq_ex, sysRowID_ex) =>
                   (from row in context.APInvJob
                    join empExpRow in context.EmpExpense on new { row.Company, row.JobNum, row.AssemblySeq, row.MtlSeq } equals new { empExpRow.Company, empExpRow.JobNum, empExpRow.AssemblySeq, empExpRow.MtlSeq }
                    where row.Company == Company_ex &&
                    row.VendorNum == vendorNum_ex &&
                    row.InvoiceNum == invoiceNum_ex &&
                    row.JobNum == jobNum_ex &&
                    row.AssemblySeq == asmSeq_ex &&
                    row.MtlSeq == mtlSeq_ex &&
                    row.SysRowID != sysRowID_ex &&
                    empExpRow.Indirect == false &&
                    empExpRow.Reimbursable == false
                    select row).Any();
                existsOtherNRExpAPInvJobQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsOtherNRExpAPInvJobQuery(this.Db, Company, vendorNum, invoiceNum, jobNum, asmSeq, mtlSeq, sysRowID);
        }

        static Func<ErpContext, string, string, int, int, Guid?, decimal> getUnpostedJobChargeQuery;
        private decimal GetUnpostedJobChargeAmt(string company, string jobNum, int assemblySeq, int mtlSeq, Guid? sysRowId)
        {
            if (getUnpostedJobChargeQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, Guid?, decimal>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, sysRowId_ex) =>
        (from ApJobRow in ctx.APInvJob
         join ApHedRow in ctx.APInvHed on new { ApJobRow.Company, ApJobRow.VendorNum, ApJobRow.InvoiceNum } equals new { ApHedRow.Company, ApHedRow.VendorNum, ApHedRow.InvoiceNum }
         where ApJobRow.Company == company_ex &&
         ApJobRow.JobNum == jobNum_ex &&
         ApJobRow.AssemblySeq == assemblySeq_ex &&
         ApJobRow.MtlSeq == mtlSeq_ex &&
         ApHedRow.Posted == false &&
         ApJobRow.SysRowID != sysRowId_ex &&
         ApJobRow.ExtCost < 0
         select ApJobRow.ExtCost).DefaultIfEmpty().Sum();
                getUnpostedJobChargeQuery = DBExpressionCompiler.Compile(expression);
            }

            return getUnpostedJobChargeQuery(this.Db, company, jobNum, assemblySeq, mtlSeq, sysRowId);
        }

        static Func<ErpContext, string, string, int, int, string, decimal> getUninvoicedExpenseAmtNetQuery;
        private decimal GetUninvoicedExpenseAmt(string company, string jobNum, int assemblySeq, int mtlSeq, string currencyCode)
        {
            if (getUninvoicedExpenseAmtNetQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, decimal>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, currencyCode_ex) =>
        (from row in ctx.EmpExpense
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.ExpCurrencyCode == currencyCode_ex &&
         row.Indirect == false &&
         row.Reimbursable == true &&
         row.Invoiced == false &&
         row.DocClaimAmt < 0
         select row.DocClaimAmt).DefaultIfEmpty().Sum();
                getUninvoicedExpenseAmtNetQuery = DBExpressionCompiler.Compile(expression);
            }

            return getUninvoicedExpenseAmtNetQuery(this.Db, company, jobNum, assemblySeq, mtlSeq, currencyCode);
        }

        private static Func<ErpContext, string, int, IEnumerable<APTran>> selectAPTranQuery;
        private IEnumerable<APTran> SelectAPTran(string company, int HeadNum)
        {
            if (selectAPTranQuery == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<APTran>>> expression =
                    (context, company_ex, HeadNum_ex) =>
                    from row in context.APTran
                    where row.Company == company_ex &&
                          row.HeadNum == HeadNum_ex
                    select row;
                selectAPTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPTranQuery(this.Db, company, HeadNum);
        }

        private static Func<ErpContext, string, string, int, bool> existsAPTranNotVoidedQuery;
        private bool ExistsAPTranNotVoided(string company, string invoiceNum, int vendorNum)
        {
            if (existsAPTranNotVoidedQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, bool>> expression =
                    (context, company_ex, invoiceNum_ex, vendorNum_ex) =>
                    (from origAPTranRow in context.APTran
                     join voidAPTranRow in context.APTran on
                        new { origAPTranRow.Company, origAPTranRow.VendorNum, origAPTranRow.InvoiceNum, origAPTranRow.HeadNum, isVoidAPTran = true, IsVoidAPTranPosted = true } equals
                        new { voidAPTranRow.Company, voidAPTranRow.VendorNum, voidAPTranRow.InvoiceNum, voidAPTranRow.HeadNum, isVoidAPTran = voidAPTranRow.Voided, IsVoidAPTranPosted = voidAPTranRow.Posted }
                     into voidedRows
                     from voidedRow in voidedRows.DefaultIfEmpty()
                     where origAPTranRow.Company == company_ex &&
                           origAPTranRow.InvoiceNum == invoiceNum_ex &&
                           origAPTranRow.VendorNum == vendorNum_ex &&
                           origAPTranRow.Posted &&
                           voidedRow == null
                     select origAPTranRow).Any();
                existsAPTranNotVoidedQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPTranNotVoidedQuery(this.Db, company, invoiceNum, vendorNum);
        }
        private static Func<ErpContext, string, int, string, string, int, bool, bool> existsAPInvDtlByInvoiceRefQuery;
        private bool ExistsAPInvDtlByInvoiceRef(string company, int vendorNum, string invoiceRef, string currInvoiceNum, int currInvoiceLine, bool posted)
        {
            if (existsAPInvDtlByInvoiceRefQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, bool, bool>> expression =
                    (ctx, company_ex, vendorNum_ex, invoiceRef_ex, currInvoiceNum_ex, currInvoiceLine_ex, posted_ex) =>
                    (from APInvDtlRow in ctx.APInvDtl
                     join APInvHedRow in ctx.APInvHed on new { APInvDtlRow.Company, APInvDtlRow.VendorNum, APInvDtlRow.InvoiceNum }
                     equals new { APInvHedRow.Company, APInvHedRow.VendorNum, APInvHedRow.InvoiceNum }
                     where APInvDtlRow.Company == company_ex &&
                     APInvDtlRow.InvoiceRef == invoiceRef_ex &&
                     (APInvDtlRow.InvoiceNum != currInvoiceNum_ex ||
                     APInvDtlRow.InvoiceLine != currInvoiceLine_ex) &&
                     APInvDtlRow.VendorNum == vendorNum_ex &&
                     APInvHedRow.Posted == posted_ex
                     select APInvDtlRow).Any();
                existsAPInvDtlByInvoiceRefQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvDtlByInvoiceRefQuery(this.Db, company, vendorNum, invoiceRef, currInvoiceNum, currInvoiceLine, posted);
        }

        #region ACTBook Queries

        private class ACTBookPartilaRow
        {
            public string BookID { get; set; }
            public string Description { get; set; }
            public string COACode { get; set; }
        }

        static Func<ErpContext, string, string, string, IEnumerable<ACTBookPartilaRow>> selectACTBookQuery;
        private IEnumerable<ACTBookPartilaRow> SelectACTBook(string company, string actTypeName, string actRevisionStatus)
        {
            if (selectACTBookQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, IEnumerable<ACTBookPartilaRow>>> expression =
                    (ctx, company_ex, actTypeName_ex, actRevisionStatus_ex) =>
                    (from actTypeRow in ctx.ACTType
                     join actRevisionRow in ctx.ACTRevision on new { actTypeRow.Company, actTypeRow.ACTTypeUID } equals new { actRevisionRow.Company, actRevisionRow.ACTTypeUID }
                     join actBookRow in ctx.ACTBook on new { actRevisionRow.Company, actRevisionRow.ACTTypeUID, actRevisionRow.ACTRevisionUID } equals new { actBookRow.Company, actBookRow.ACTTypeUID, actBookRow.ACTRevisionUID }
                     join bookRow in ctx.GLBook on new { actBookRow.Company, actBookRow.BookID } equals new { bookRow.Company, bookRow.BookID }
                     where actTypeRow.Company == company_ex &&
                            actTypeRow.DisplayName == actTypeName_ex &&
                            actRevisionRow.RevisionStatus == actRevisionStatus_ex &&
                            actBookRow.UseMapFlag == false
                     orderby bookRow.BookID
                     select new ACTBookPartilaRow
                     {
                         BookID = bookRow.BookID,
                         Description = bookRow.Description,
                         COACode = bookRow.COACode
                     });
                selectACTBookQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectACTBookQuery(Db, company, actTypeName, actRevisionStatus);
        }

        #endregion ACTBook Queries


        #region APInvDtl Queries

        private static Func<ErpContext, string, string, bool, bool, bool, IEnumerable<CalculateGLAnalysisVariance.APInvDtl_ExtCost>> selectAPInvDtlQuery;
        private IEnumerable<CalculateGLAnalysisVariance.APInvDtl_ExtCost> SelectAPInvDtl(string company, string groupID, bool posted, bool deferredExp, bool cancellationDtl)
        {
            if (selectAPInvDtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool, bool, IEnumerable<CalculateGLAnalysisVariance.APInvDtl_ExtCost>>> expression =
                    (ctx, company_ex, groupID_ex, posted_ex, deferredExp_ex, cancellationDtl_ex) =>
                    from APInvDtlRow in ctx.APInvDtl
                    join APInvHedRow in ctx.APInvHed on new { APInvDtlRow.Company, APInvDtlRow.VendorNum, APInvDtlRow.InvoiceNum } equals new { APInvHedRow.Company, APInvHedRow.VendorNum, APInvHedRow.InvoiceNum }
                    join APInvGrpRow in ctx.APInvGrp on new { APInvHedRow.Company, APInvHedRow.GroupID } equals new { APInvGrpRow.Company, APInvGrpRow.GroupID }
                    where APInvGrpRow.Company == company_ex
                    && APInvGrpRow.GroupID == groupID_ex
                    && APInvHedRow.Posted == posted_ex
                    && APInvDtlRow.DeferredExp == deferredExp_ex
                    && APInvDtlRow.CancellationDtl == cancellationDtl_ex
                    select new CalculateGLAnalysisVariance.APInvDtl_ExtCost() { VendorNum = APInvDtlRow.VendorNum, InvoiceNum = APInvDtlRow.InvoiceNum, InvoiceLine = APInvDtlRow.InvoiceLine, ExtCost = APInvDtlRow.ExtCost, DocExtCost = APInvDtlRow.DocExtCost, Rpt1ExtCost = APInvDtlRow.Rpt1ExtCost, Rpt2ExtCost = APInvDtlRow.Rpt2ExtCost, Rpt3ExtCost = APInvDtlRow.Rpt3ExtCost };
                selectAPInvDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlQuery(this.Db, company, groupID, posted, deferredExp, cancellationDtl);
        }

        private static Func<ErpContext, string, string, bool, bool, bool, IEnumerable<APInvDtl>> selectAPInvDtlQuery1;
        private IEnumerable<APInvDtl> SelectAPInvDtlWithUpdLock(string company, string groupID, bool posted, bool deferredExp, bool cancellationDtl)
        {
            if (selectAPInvDtlQuery1 == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool, bool, IEnumerable<APInvDtl>>> expression =
                    (ctx, company_ex, groupID_ex, posted_ex, deferredExp_ex, cancellationDtl_ex) =>
                    from APInvDtlRow in ctx.APInvDtl.With(LockHint.UpdLock)
                    join APInvHedRow in ctx.APInvHed on new { APInvDtlRow.Company, APInvDtlRow.VendorNum, APInvDtlRow.InvoiceNum } equals new { APInvHedRow.Company, APInvHedRow.VendorNum, APInvHedRow.InvoiceNum }
                    join APInvGrpRow in ctx.APInvGrp on new { APInvHedRow.Company, APInvHedRow.GroupID } equals new { APInvGrpRow.Company, APInvGrpRow.GroupID }
                    where APInvGrpRow.Company == company_ex
                    && APInvGrpRow.GroupID == groupID_ex
                    && APInvHedRow.Posted == posted_ex
                    && APInvDtlRow.DeferredExp == deferredExp_ex
                    && APInvDtlRow.CancellationDtl == cancellationDtl_ex
                    select APInvDtlRow;
                selectAPInvDtlQuery1 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlQuery1(this.Db, company, groupID, posted, deferredExp, cancellationDtl);
        }

        static Func<ErpContext, string, int, string, string, int, string, decimal> getPackSlipInvoicedQtyQuery;
        private decimal getPackSlipInvoicedQty(string company, int vendorNum, string purPoint, string packSlip, int packLine, string invoicenum)
        {
            if (getPackSlipInvoicedQtyQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, string, decimal>> expression =
      (ctx, company_ex, vendorNum_ex, purPoint_ex, packSlip_ex, packLine_ex, invoicenum_ex) =>
        (
         from row in ctx.APInvDtl
         where row.Company == company_ex &&
         row.VendorNum == vendorNum_ex &&
         row.PurPoint == purPoint_ex &&
         row.PackSlip == packSlip_ex &&
         row.PackLine == packLine_ex &&
         row.InvoiceNum != invoicenum_ex
         group row by row.PackSlip into g
         select g.Sum(row => row.VendorQty)).FirstOrDefault();
                getPackSlipInvoicedQtyQuery = DBExpressionCompiler.Compile(expression);
            }

            return getPackSlipInvoicedQtyQuery(this.Db, company, vendorNum, purPoint, packSlip, packLine, invoicenum);
        }


        static Func<ErpContext, string, int, int, string, int, string, string> existsUnpostedInvoiceLineQuery;
        private string existsUnpostedInvoiceLine(string company, int vendorNum, int poNum, string packSlip, int packLine, string invoiceNum)
        {
            if (existsUnpostedInvoiceLineQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, string, int, string, string>> expression =
      (ctx, company_ex, vendorNum_ex, poNum_ex, packSlip_ex, packLine_ex, invoiceNum_ex) =>
        (
         from row in ctx.APInvDtl
         join hed in ctx.APInvHed
         on new { row.Company, row.VendorNum, row.InvoiceNum }
         equals new { hed.Company, hed.VendorNum, hed.InvoiceNum }
         where row.Company == company_ex &&
         row.VendorNum == vendorNum_ex &&
         row.PONum == poNum_ex &&
         row.PackSlip == packSlip_ex &&
         row.PackLine == packLine_ex &&
         hed.Posted == false &&
         hed.GroupID != (from row2 in ctx.APInvHed where row2.Company == company_ex && row2.VendorNum == vendorNum_ex && row2.InvoiceNum == invoiceNum_ex select row2.GroupID).FirstOrDefault()
         select hed.GroupID).FirstOrDefault();
                existsUnpostedInvoiceLineQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsUnpostedInvoiceLineQuery(this.Db, company, vendorNum, poNum, packSlip, packLine, invoiceNum);
        }

        private static Func<ErpContext, string, int, string, int, bool, int> FindFirstAPInvDtlCorrectionLineQuery;
        private int FindFirstAPInvDtlCorrectionLine(string company, int vendorNum, string invoiceNum, int refInvoiceLine, bool correctionDtl)
        {
            if (FindFirstAPInvDtlCorrectionLineQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, bool, int>> expression =
                    (ctx, company_ex, vendorNum_ex, invoiceNum_ex, refInvoiceLine_ex, correctionDtl_ex) =>
                        (from row in ctx.APInvDtl
                         where row.Company == company_ex &&
                               row.VendorNum == vendorNum_ex &&
                               row.InvoiceNum == invoiceNum_ex &&
                               row.InvoiceLineRef == refInvoiceLine_ex &&
                               row.CorrectionDtl == correctionDtl_ex
                         select row.InvoiceLine)
                               .FirstOrDefault();
                FindFirstAPInvDtlCorrectionLineQuery = DBExpressionCompiler.Compile(expression);
            }
            return FindFirstAPInvDtlCorrectionLineQuery(this.Db, company, vendorNum, invoiceNum, refInvoiceLine, correctionDtl);
        }

        static Func<ErpContext, string, int, string, int, APInvDtl> selectAPInvDtlQuery5;
        private APInvDtl FindFirstAPInvDtl(string company, int vendorNum, string invoiceNum, int invoiceLine)
        {
            if (selectAPInvDtlQuery5 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, APInvDtl>> expression =
                  (ctx, company_ex, vendornum_ex, invoicenum_ex, invoiceLine_ex) =>
                    (from row in ctx.APInvDtl
                     where row.Company == company_ex &&
                     row.VendorNum == vendornum_ex &&
                     row.InvoiceNum == invoicenum_ex &&
                     row.InvoiceLine == invoiceLine_ex
                     select row).FirstOrDefault();
                selectAPInvDtlQuery5 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlQuery5(this.Db, company, vendorNum, invoiceNum, invoiceLine);
        }

        static Func<ErpContext, string, string, int, int, int, string, int, bool, string, bool, bool> existFinalInvoice;
        private bool ExistFinalInvoice(string company, string invoiceNum, int pONum, int pOLine, int pORel, string packSlip, int packLine, bool cancellationInv, string refcancellationBy, bool finalInvoice)
        {
            if (existFinalInvoice == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, string, int, bool, string, bool, bool>> expression =
                  (ctx, company_ex, invoicenum_ex, pONum_ex, pOLine_ex, pORel_ex, packSlip_ex, packLine_ex, cancellationInv_ex, refcancellationBy_ex, finalInvoice_ex) =>
                    (from APInvDtlRow in ctx.APInvDtl
                     join APInvHedRow in ctx.APInvHed on new { APInvDtlRow.Company, APInvDtlRow.InvoiceNum, APInvDtlRow.VendorNum }
                                                  equals new { APInvHedRow.Company, APInvHedRow.InvoiceNum, APInvHedRow.VendorNum }
                     where APInvDtlRow.Company == company_ex &&
                     APInvDtlRow.InvoiceNum != invoicenum_ex &&
                     APInvDtlRow.PONum == pONum_ex &&
                     APInvDtlRow.POLine == pOLine_ex &&
                     APInvDtlRow.PORelNum == pORel_ex &&
                     APInvDtlRow.PackSlip == packSlip_ex &&
                     APInvDtlRow.PackLine == packLine_ex &&
                     APInvHedRow.CancellationInv == cancellationInv_ex &&
                     APInvHedRow.RefCancelledby == refcancellationBy_ex &&
                     APInvDtlRow.FinalInvoice == finalInvoice_ex
                     select true).Any();
                existFinalInvoice = DBExpressionCompiler.Compile(expression);
            }

            return existFinalInvoice(this.Db, company, invoiceNum, pONum, pOLine, pORel, packSlip, packLine, cancellationInv, refcancellationBy, finalInvoice);
        }

        static Func<ErpContext, string, int, string, int, decimal> selectAPInvDtlVendorQtyQuery;
        private decimal FindFirstAPInvDtlVendorQty(string company, int vendorNum, string invoiceNum, int invoiceLine)
        {
            if (selectAPInvDtlVendorQtyQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, decimal>> expression =
                  (ctx, company_ex, vendornum_ex, invoicenum_ex, invoiceLine_ex) =>
                    (from row in ctx.APInvDtl
                     where row.Company == company_ex &&
                     row.VendorNum == vendornum_ex &&
                     row.InvoiceNum == invoicenum_ex &&
                     row.InvoiceLine == invoiceLine_ex
                     select row.VendorQty).FirstOrDefault();
                selectAPInvDtlVendorQtyQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlVendorQtyQuery(this.Db, company, vendorNum, invoiceNum, invoiceLine);
        }


        #endregion

        #region APInvHed

        static Func<ErpContext, string, int, string, string> findAPInvHedCancelledbyInvoiceQuery;
        private string FindAPInvHedCancelledbyInvoice(string company, int vendorNum, string invoiceNum)
        {
            if (findAPInvHedCancelledbyInvoiceQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string>> expression =
                    (ctx, company_ex, vendornum_ex, invoicenum_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.InvoiceNum == invoicenum_ex &&
                     row.VendorNum == vendornum_ex &&
                     row.RefCancelledby != string.Empty
                     select row).FirstOrDefault().RefCancelledby;
                findAPInvHedCancelledbyInvoiceQuery = DBExpressionCompiler.Compile(expression);
            }
            return findAPInvHedCancelledbyInvoiceQuery(this.Db, company, vendorNum, invoiceNum);
        }

        static Func<ErpContext, string, int, string, APInvHed> findAPInvHedCancelledbyInvQuery;
        private APInvHed FindAPInvHedCancelledbyInv(string company, int vendorNum, string refCancelledby)
        {
            if (findAPInvHedCancelledbyInvQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, APInvHed>> expression =
                    (ctx, company_ex, vendornum_ex, RefCancelledby_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.RefCancelledby == RefCancelledby_ex &&
                     row.VendorNum == vendornum_ex
                     select row).FirstOrDefault();
                findAPInvHedCancelledbyInvQuery = DBExpressionCompiler.Compile(expression);
            }
            return findAPInvHedCancelledbyInvQuery(this.Db, company, vendorNum, refCancelledby);
        }

        private static Func<ErpContext, string, int, bool, bool, bool, bool, DateTime, IEnumerable<APInvHed>> selectAPInvHedForTrackerQuery;
        private IEnumerable<APInvHed> SelectAPInvHedForTracker(string Company, int VendorNum, bool Posted, bool all, bool open, bool inRange, DateTime fromDay)
        {
            if (selectAPInvHedForTrackerQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool, bool, bool, bool, DateTime, IEnumerable<APInvHed>>> expression =
                    (context, Company_ex, VendorNum_ex, Posted_ex, all_ex, open_ex, inRange_ex, fromDay_ex) =>
                    (from row in context.APInvHed
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.Posted == Posted_ex &&
                     (all_ex || row.OpenPayable == open_ex) &&
                     (!inRange_ex || DateTime.Compare(fromDay_ex, (DateTime)row.InvoiceDate) <= 0)
                     select row);
                selectAPInvHedForTrackerQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedForTrackerQuery(this.Db, Company, VendorNum, Posted, all, open, inRange, fromDay);
        }

        private static Func<ErpContext, string, string, bool> existsUniqueAPImportNumQuery;
        private bool ExistsUniqueAPImportNum(string Company, string ImportNum)
        {
            if (existsUniqueAPImportNumQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (context, Company_ex, ImportNum_ex) =>
                    (from row in context.APInvHed
                     where row.Company == Company_ex &&
                     row.ImportNum == ImportNum_ex
                     select row)
                    .Any();
                existsUniqueAPImportNumQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsUniqueAPImportNumQuery(this.Db, Company, ImportNum);
        }

        private class APInvHedPartialRow3 : Epicor.Data.TempRowBase
        {
            public string CHISRCodeLine { get; set; }
            public string SEBankRef { get; set; }
        }

        private static Func<ErpContext, string, int, string, APInvHedPartialRow3> findFirstAPInvHedQuery5;
        private APInvHedPartialRow3 FindFirstAPInvHed2(string company, int vendorNum, string invoiceNum)
        {
            if (findFirstAPInvHedQuery5 == null)
            {
                Expression<Func<ErpContext, string, int, string, APInvHedPartialRow3>> expression =
                    (context, company_ex, vendorNum_ex, invoiceNum_ex) =>
                    (from row in context.APInvHed
                     where row.Company == company_ex &&
                           row.VendorNum == vendorNum_ex &&
                           row.InvoiceNum == invoiceNum_ex
                     select new APInvHedPartialRow3 { CHISRCodeLine = row.CHISRCodeLine, SEBankRef = row.SEBankRef })
                    .FirstOrDefault();
                findFirstAPInvHedQuery5 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedQuery5(this.Db, company, vendorNum, invoiceNum);
        }

        static Func<ErpContext, string, int, string, APInvHed> findFirstAPInvHedQuery;
        private APInvHed FindFirstAPInvHed(string company, int vendorNum, string invoiceNum)
        {
            if (findFirstAPInvHedQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, APInvHed>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceNum_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex
                     select row).FirstOrDefault();
                findFirstAPInvHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedQuery(this.Db, company, vendorNum, invoiceNum);
        }

        private class APInvHedPartial
        {
            public bool MatchedFromLI { get; set; }
            public bool AllowOverrideLI { get; set; }
            public bool DebitMemo { get; set; }

        }

        static Func<ErpContext, string, int, string, APInvHedPartial> findFirstAPInvHedPartialQuery;
        private APInvHedPartial FindFirstAPInvHedPartial(string company, int vendorNum, string invoiceNum)
        {
            if (findFirstAPInvHedPartialQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, APInvHedPartial>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceNum_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex
                     select new APInvHedPartial
                     {
                         MatchedFromLI = row.MatchedFromLI,
                         AllowOverrideLI = row.AllowOverrideLI,
                         DebitMemo = row.DebitMemo

                     }).FirstOrDefault();
                findFirstAPInvHedPartialQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedPartialQuery(this.Db, company, vendorNum, invoiceNum);
        }

        static Func<ErpContext, string, int, string, bool> isAPInvoiceDebitMemo;
        private bool IsAPInvoiceDebitMemo(string company, int vendorNum, string invoiceNum)
        {
            if (isAPInvoiceDebitMemo == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceNum_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex
                     select row.DebitMemo).FirstOrDefault();
                isAPInvoiceDebitMemo = DBExpressionCompiler.Compile(expression);
            }

            return isAPInvoiceDebitMemo(this.Db, company, vendorNum, invoiceNum);
        }

        private class APInvHedTypePartialRow : Epicor.Data.TempRowBase
        {
            public bool GRNIClearing { get; set; }
            public bool DebitMemo { get; set; }
        }
        static Func<ErpContext, string, string, APInvHedTypePartialRow> findGroupInvoiceTypeQuery;
        /// <summary>
        /// Retrieves the GRNIClearing and DebitMemo flags from the GroupID specified.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="groupid"></param>
        /// <returns>APInvHedTypePartialRow which contains only two boolean fields; GRNIClearing and DebitMemo</returns>
        private APInvHedTypePartialRow FindGroupInvoiceTypeQuery(string company, string groupid)
        {
            if (findGroupInvoiceTypeQuery == null)
            {
                Expression<Func<ErpContext, string, string, APInvHedTypePartialRow>> expression =
                    (ctx, company_ex, groupid_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.GroupID == groupid_ex &&
                     row.Posted == false
                     select new APInvHedTypePartialRow { GRNIClearing = row.GRNIClearing, DebitMemo = row.DebitMemo }).FirstOrDefault();
                findGroupInvoiceTypeQuery = DBExpressionCompiler.Compile(expression);
            }
            return findGroupInvoiceTypeQuery(this.Db, company, groupid);
        }

        static Func<ErpContext, string, int, string, string> findAPInvHedEntryPersonQuery;
        private string FindAPInvHedEntryPersonQuery(string company, int vendorNum, string invoiceNum)
        {
            if (findAPInvHedEntryPersonQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string>> expression =
                    (ctx, company_ex, vendornum_ex, invoicenum_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.VendorNum == vendornum_ex &&
                     row.InvoiceNum == invoicenum_ex
                     select row).FirstOrDefault().EntryPerson;
                findAPInvHedEntryPersonQuery = DBExpressionCompiler.Compile(expression);
            }
            return findAPInvHedEntryPersonQuery(this.Db, company, vendorNum, invoiceNum);
        }

        static Func<ErpContext, string, int, string, DateTime?> findFirstAPInvoiceDateQuery;
        private DateTime? FindFirstAPInvoiceDate(string company, int vendorNum, string invoiceNum)
        {
            if (findFirstAPInvoiceDateQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, DateTime?>> expression =
                    (ctx, company_ex, vendornum_ex, invoicenum_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.VendorNum == vendornum_ex &&
                     row.InvoiceNum == invoicenum_ex
                     select row).FirstOrDefault().InvoiceDate;
                findFirstAPInvoiceDateQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstAPInvoiceDateQuery(this.Db, company, vendorNum, invoiceNum);
        }

        private class APInvHedPartialRowForPaySched : Epicor.Data.TempRowBase
        {
            public string CurrencyCode { get; set; }
            public bool DebitMemo { get; set; }
            public decimal DocInvoiceAmt { get; set; }
            public decimal InvoiceAmt { get; set; }
            public decimal Rpt1InvoiceAmt { get; set; }
            public decimal Rpt2InvoiceAmt { get; set; }
            public decimal Rpt3InvoiceAmt { get; set; }
            public DateTime? InvoiceDate { get; set; }
        }

        private static Func<ErpContext, string, int, string, APInvHedPartialRowForPaySched> findFirstAPInvHedQueryForPaySched;
        private APInvHedPartialRowForPaySched FindFirstAPInvHedForPaySched(string Company, int VendorNum, string InvoiceNum)
        {
            if (findFirstAPInvHedQueryForPaySched == null)
            {
                Expression<Func<ErpContext, string, int, string, APInvHedPartialRowForPaySched>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APInvHed
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex
                     select new APInvHedPartialRowForPaySched { CurrencyCode = row.CurrencyCode, DebitMemo = row.DebitMemo, DocInvoiceAmt = row.DocInvoiceAmt, InvoiceAmt = row.InvoiceAmt, Rpt1InvoiceAmt = row.Rpt1InvoiceAmt, Rpt2InvoiceAmt = row.Rpt2InvoiceAmt, Rpt3InvoiceAmt = row.Rpt3InvoiceAmt, InvoiceDate = row.InvoiceDate })
                    .FirstOrDefault();
                findFirstAPInvHedQueryForPaySched = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedQueryForPaySched(this.Db, Company, VendorNum, InvoiceNum);
        }

        private class PEAPInvHedPartialRow : Epicor.Data.TempRowBase
        {
            public decimal DocPEDetTaxAmt { get; set; }
            public decimal UnpostedBal { get; set; }
            public decimal DocUnpostedBal { get; set; }
            public decimal Rpt1UnpostedBal { get; set; }
            public decimal Rpt2UnpostedBal { get; set; }
            public decimal Rpt3UnpostedBal { get; set; }
        }
        static Func<ErpContext, string, int, string, PEAPInvHedPartialRow> findFirstPEAPInvHedQuery;
        private PEAPInvHedPartialRow FindFirstPEAPInvHedPartial(string company, int vendorNum, string invoiceNum)
        {
            if (findFirstPEAPInvHedQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, PEAPInvHedPartialRow>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceNum_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex
                     select new PEAPInvHedPartialRow
                     {
                         DocPEDetTaxAmt = row.DocPEDetTaxAmt,
                         UnpostedBal = row.UnpostedBal,
                         DocUnpostedBal = row.DocUnpostedBal,
                         Rpt1UnpostedBal = row.Rpt1UnpostedBal,
                         Rpt2UnpostedBal = row.Rpt2UnpostedBal,
                         Rpt3UnpostedBal = row.Rpt3UnpostedBal
                     }).FirstOrDefault();
                findFirstPEAPInvHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPEAPInvHedQuery(this.Db, company, vendorNum, invoiceNum);
        }

        private static Func<ErpContext, string, int, string, bool, bool> existsAPInvHedQuery2;
        private bool ExistsAPInvHed2(string company, int vendorNum, string invoiceNum, bool posted)
        {
            if (existsAPInvHedQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, bool>> expression =
                    (ctx, company_ex, vendorNum_ex, invoiceNum_ex, posted_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex &&
                     row.Posted == posted_ex
                     select row)
                    .Any();
                existsAPInvHedQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedQuery2(this.Db, company, vendorNum, invoiceNum, posted);
        }
        private static Func<ErpContext, string, int, string, bool, bool, APInvHed> findFirstAPInvHedQuery23;
        private APInvHed FindFirstAPInvHed23(string company, int vendorNum, string invoiceNum, bool posted, bool GRNIClearing)
        {
            if (findFirstAPInvHedQuery23 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, bool, APInvHed>> expression =
                    (ctx, company_ex, vendorNum_ex, invoiceNum_ex, posted_ex, GRNIClearing_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex &&
                     row.Posted == posted_ex &&
                     row.GRNIClearing == GRNIClearing_ex
                     select row).FirstOrDefault();
                findFirstAPInvHedQuery23 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedQuery23(this.Db, company, vendorNum, invoiceNum, posted, GRNIClearing);
        }

        static Func<ErpContext, string, int, string, bool, bool, decimal> sumAPInvHedAPInvDtlQuery;
        private decimal SUMAPInvHedAPInvDtlQuery(string company, int vendorNum, string invoiceNum, bool posted, bool GRNIClearing)
        {
            if (sumAPInvHedAPInvDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, bool, decimal>> expression =
                  (ctx, company_ex, vendornum_ex, invoicenum_ex, posted_ex, GRNIClearing_ex) =>
                    (from APInvDtlRow in ctx.APInvDtl
                     join APInvHedRow in ctx.APInvHed on new { APInvDtlRow.Company, APInvDtlRow.VendorNum, APInvDtlRow.InvoiceNum }
                     equals new { APInvHedRow.Company, APInvHedRow.VendorNum, APInvHedRow.InvoiceNum }
                     where APInvDtlRow.Company == company_ex &&
                     APInvDtlRow.VendorNum == vendornum_ex &&
                     APInvDtlRow.InvoiceNum == invoicenum_ex &&
                     APInvHedRow.Posted == posted_ex &&
                     APInvHedRow.GRNIClearing == GRNIClearing_ex
                     select APInvDtlRow.DocExtCost).Sum();
                sumAPInvHedAPInvDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return sumAPInvHedAPInvDtlQuery(this.Db, company, vendorNum, invoiceNum, posted, GRNIClearing);
        }

        static Func<ErpContext, string, int, string, int, string, bool> existsRefAPInvHedQuery;
        private bool ExistsRefAPInvHed(string company, int vendorNum, string invoiceNum, int exceptInvoiceLine, string currencyCode)
        {
            if (existsRefAPInvHedQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, bool>> expression =
                  (ctx, company_ex, vendornum_ex, invoicenum_ex, exceptInvoiceLine_ex, currencyCode_ex) =>
                    (from APInvDtlRow in ctx.APInvDtl
                     join APInvHedRow in ctx.APInvHed on new { APInvDtlRow.Company, APInvDtlRow.VendorNum, APInvDtlRow.InvoiceNum }
                     equals new { APInvHedRow.Company, APInvHedRow.VendorNum, APInvHedRow.InvoiceNum }
                     where APInvDtlRow.Company == company_ex &&
                     APInvDtlRow.VendorNum == vendornum_ex &&
                     APInvDtlRow.InvoiceNum == invoicenum_ex &&
                     APInvDtlRow.InvoiceLine != exceptInvoiceLine_ex &&
                     APInvHedRow.CurrencyCode != currencyCode_ex
                     select APInvDtlRow).Any();
                existsRefAPInvHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsRefAPInvHedQuery(this.Db, company, vendorNum, invoiceNum, exceptInvoiceLine, currencyCode);
        }

        static Func<ErpContext, string, string, bool, IEnumerable<APInvHed>> findAPInvHed1;
        private IEnumerable<APInvHed> FindAPInvHed1(string Company, string GroupID, bool CalcAll)
        {
            if (findAPInvHed1 == null)
            {
                Expression<Func<ErpContext, string, string, bool, IEnumerable<APInvHed>>> expression =
                    (ctx, company_ex, groupId_ex, calcAll_ex) =>
                    (from ApInvHed in ctx.APInvHed
                     where ApInvHed.Company == company_ex &&
                           !ApInvHed.Posted &&
                           ApInvHed.GroupID == groupId_ex &&
                           ((calcAll_ex && ApInvHed.ReadyToCalc) || !calcAll_ex)
                     select ApInvHed);
                findAPInvHed1 = DBExpressionCompiler.Compile(expression);
            }

            return findAPInvHed1(this.Db, Company, GroupID, CalcAll);
        }

        static Func<ErpContext, string, string, int, IEnumerable<APInvHed>> findAPInvHed2;
        private IEnumerable<APInvHed> FindAPInvHed2(string Company, string InvoiceNum, int VendorNum)
        {
            if (findAPInvHed2 == null)
            {
                Expression<Func<ErpContext, string, string, int, IEnumerable<APInvHed>>> expression =
                    (ctx, company_ex, invoiceNum_ex, vendorNum_ex) =>
                    (from ApInvHed in ctx.APInvHed
                     where ApInvHed.Company == company_ex &&
                           !ApInvHed.Posted &&
                           ApInvHed.InvoiceNum == invoiceNum_ex &&
                           ApInvHed.VendorNum == vendorNum_ex
                     select ApInvHed);
                findAPInvHed2 = DBExpressionCompiler.Compile(expression);
            }

            return findAPInvHed2(this.Db, Company, InvoiceNum, VendorNum);
        }

        static Func<ErpContext, string, string, bool, bool, IEnumerable<APInvoiceHedAndDtl>> selectAPClearingInvHedQueryDtls;
        private IEnumerable<APInvoiceHedAndDtl> SelectAPClearingInvHedDtls(string company, string groupID, bool posted, bool grniclearing)
        {
            if (selectAPClearingInvHedQueryDtls == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool, IEnumerable<APInvoiceHedAndDtl>>> expression =
                  (ctx, company_ex, groupID_ex, posted_ex, grniclearing_ex) =>
                    (from hed in ctx.APInvHed.With(LockHint.UpdLock)
                     join dtls in ctx.APInvDtl
                     on new { hed.Company, hed.InvoiceNum, hed.VendorNum } equals new { dtls.Company, dtls.InvoiceNum, dtls.VendorNum }
                     where hed.Company == company_ex &&
                     hed.GroupID == groupID_ex &&
                     hed.Posted == posted_ex &&
                     hed.GRNIClearing == grniclearing_ex
                     select new APInvoiceHedAndDtl { GRNIAPInvHed = hed, GRNIAPInvDtl = dtls });
                selectAPClearingInvHedQueryDtls = DBExpressionCompiler.Compile(expression);
            }

            return selectAPClearingInvHedQueryDtls(this.Db, company, groupID, posted, grniclearing);
        }

        static Func<ErpContext, string, int, string, IEnumerable<APInvHedMscTax>> selectAPInvHedMiscTaxQuery;
        private IEnumerable<APInvHedMscTax> SelectAPInvHedMiscTax(string company, int vendorNum, string invoiceNum)
        {
            if (selectAPInvHedMiscTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvHedMscTax>>> expression =
                  (ctx, company_ex, vendornum_ex, invoicenum_ex) =>
                    (from row in ctx.APInvHedMscTax
                     where row.Company == company_ex &&
                     row.VendorNum == vendornum_ex &&
                     row.InvoiceNum == invoicenum_ex
                     select row);
                selectAPInvHedMiscTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedMiscTaxQuery(this.Db, company, vendorNum, invoiceNum);
        }

        static Func<ErpContext, string, int, string, string, IEnumerable<APInvHedMscTax>> selectAPInvHedMiscTaxQuery2;
        private IEnumerable<APInvHedMscTax> SelectAPInvHedMiscTax(string company, int vendorNum, string invoiceNum, string miscCode)
        {
            if (selectAPInvHedMiscTaxQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, string, IEnumerable<APInvHedMscTax>>> expression =
                  (ctx, company_ex, vendornum_ex, invoicenum_ex, miscCode_ex) =>
                    (from row in ctx.APInvHedMscTax
                     where row.Company == company_ex &&
                     row.VendorNum == vendornum_ex &&
                     row.InvoiceNum == invoicenum_ex &&
                     row.MiscCode == miscCode_ex
                     select row);
                selectAPInvHedMiscTaxQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedMiscTaxQuery2(this.Db, company, vendorNum, invoiceNum, miscCode);
        }

        static Func<ErpContext, string, string, IEnumerable<APInvHed>> selectAPInvHedGroupQuery;
        private IEnumerable<APInvHed> SelectAPInvHed(string company, string groupID)
        {
            if (selectAPInvHedGroupQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<APInvHed>>> expression =
                  (ctx, company_ex, groupID_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                           row.GroupID == groupID_ex
                     select row);
                selectAPInvHedGroupQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedGroupQuery(this.Db, company, groupID);
        }
        static Func<ErpContext, string, string, IEnumerable<APInvHed>> selectAPInvHedDMRWithUpdLockQuery;
        private IEnumerable<APInvHed> SelectAPInvHedDMRWithUpdLock(string company, string groupID)
        {
            if (selectAPInvHedDMRWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<APInvHed>>> expression =
                  (ctx, company_ex, groupID_ex) =>
                    (from invoice in ctx.APInvHed.With(LockHint.UpdLock)
                     join dmrActn in ctx.DMRActn on
                        new { invoice.Company, InvoiceNum = invoice.InvoiceNum } equals
                        new { dmrActn.Company, InvoiceNum = dmrActn.DebitMemoNum }
                     where invoice.Company == company_ex &&
                           invoice.GroupID == groupID_ex &&
                           invoice.DebitMemo
                     select invoice);
                selectAPInvHedDMRWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedDMRWithUpdLockQuery(this.Db, company, groupID);
        }
        static Func<ErpContext, string, string, IEnumerable<APInvHed>> selectAPInvHedGroupWithUpdLockQuery;
        private IEnumerable<APInvHed> SelectAPInvHedWithUpdLock(string company, string groupID)
        {
            if (selectAPInvHedGroupWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<APInvHed>>> expression =
                  (ctx, company_ex, groupID_ex) =>
                    (from row in ctx.APInvHed.With(LockHint.UpdLock)
                     where row.Company == company_ex &&
                           row.GroupID == groupID_ex
                     select row);
                selectAPInvHedGroupWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedGroupWithUpdLockQuery(this.Db, company, groupID);
        }

        static Func<ErpContext, string, int, string, bool> isAPInvoicePosted;
        private bool IsAPInvoicePosted(string company, int vendorNum, string invoiceNum)
        {
            if (isAPInvoicePosted == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceNum_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex
                     select row.Posted).FirstOrDefault();
                isAPInvoicePosted = DBExpressionCompiler.Compile(expression);
            }

            return isAPInvoicePosted(this.Db, company, vendorNum, invoiceNum);
        }
        #endregion

        #region APInvSched Queries
        private static Func<ErpContext, string, int, string, IEnumerable<APInvSched>> selectAPInvSchedQuery;
        private IEnumerable<APInvSched> SelectAPInvSched(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAPInvSchedQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvSched>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    from row in context.APInvSched
                    where row.Company == Company_ex &&
                          row.VendorNum == VendorNum_ex &&
                          row.InvoiceNum == InvoiceNum_ex
                    select row;
                selectAPInvSchedQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvSchedQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, string, bool> selectAPInvSchedQuery4;
        private bool OnlyOneAPInvSched(string company, int vendorNum, string invoiceNum)
        {
            if (selectAPInvSchedQuery4 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                    (context, Company_ex, vendorNum_ex, invoiceNum_ex) =>
                    (from row in context.APInvSched
                     where row.Company == Company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex
                     select row).Any();
                selectAPInvSchedQuery4 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvSchedQuery4(this.Db, company, vendorNum, invoiceNum);

        }

        private static Func<ErpContext, string, int, string, APInvSched> findFirstAPInvSchedWithUpdLockQuery;
        private APInvSched FindFirstAPInvSchedWithUpdLock(string Company, int VendorNum, string InvoiceNum)
        {
            if (findFirstAPInvSchedWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, APInvSched>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APInvSched.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPInvSchedWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvSchedWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        #endregion

        #region CurrExChain Queries


        private static Func<ErpContext, string, string, string, string, string, IEnumerable<CurrExChain>> SelectCurrExChainWithUpdLockQuery;
        private IEnumerable<CurrExChain> SelectCurrExChainWithUpdLock(string Company, string TableName, string Key1, string Key2)
        {
            if (SelectCurrExChainWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, IEnumerable<CurrExChain>>> expression =
                    (ctx, Company_ex, TableName_ex, Key1_ex, Key2_ex, TableNameTax_ex) =>
                            (from row in ctx.CurrExChain.With(LockHint.UpdLock)
                             where row.Company == Company_ex &&
                             (row.TableName == TableName_ex || row.TableName == TableNameTax_ex) &&
                             row.Key1 == Key1_ex &&
                             row.Key2 == Key2_ex
                             select row);
                SelectCurrExChainWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return SelectCurrExChainWithUpdLockQuery(this.Db, Company, TableName, Key1, Key2, TableName + "-Tax");
        }
        //HOLDLOCK
        #endregion

        #region Currency Queries
        private static Func<ErpContext, string, bool, string> findFirstCurrencyQuery2;
        private string FindCurrencyBase(string Company, bool Base)
        {
            if (findFirstCurrencyQuery2 == null)
            {
                Expression<Func<ErpContext, string, bool, string>> expression =
                     (context, Company_ex, Base_ex) =>
                    (from row in context.Currency
                     where row.Company == Company_ex &&
                          row.BaseCurr == Base_ex
                     select row.CurrencyCode).FirstOrDefault();
                findFirstCurrencyQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCurrencyQuery2(this.Db, Company, Base);
        }

        static Func<ErpContext, string, string, bool> expCurrencyNotBaseQuery;
        private bool ExpCurrencyNotBase(string company, string currCode)
        {
            if (expCurrencyNotBaseQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, currCode_ex) =>
        (from row in ctx.Currency
         where row.Company == company_ex &&
         row.CurrencyCode == currCode_ex &&
         row.BaseCurr == false
         select row).Any();
                expCurrencyNotBaseQuery = DBExpressionCompiler.Compile(expression);
            }

            return expCurrencyNotBaseQuery(this.Db, company, currCode);
        }
        #endregion

        #region FiscalPer Queries

        static Func<ErpContext, string, string, DateTime?, DateTime?, FiscalPer> findFirstFiscalPerQuery;
        private FiscalPer FindFirstFiscalPer(string company, string fiscalCalendarID, DateTime? startDate, DateTime? endDate)
        {
            if (findFirstFiscalPerQuery == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, DateTime?, FiscalPer>> expression =
                  (ctx, company_ex, fiscalCalendarID_ex, startDate_ex, endDate_ex) =>
                    (from row in ctx.FiscalPer
                     where row.Company == company_ex &&
                     row.FiscalCalendarID == fiscalCalendarID_ex &&
                     row.StartDate.Value <= startDate_ex.Value &&
                     row.EndDate.Value >= endDate_ex.Value
                     select row).FirstOrDefault();
                findFirstFiscalPerQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstFiscalPerQuery(this.Db, company, fiscalCalendarID, startDate, endDate);
        }

        #endregion

        #region TaxRate Queries

        static Func<ErpContext, string, string, string, DateTime?, TaxRate> findLastTaxRateQuery;
        private TaxRate FindLastTaxRate(string company, string taxCode, string rateCode, DateTime? effectiveFrom)
        {
            if (findLastTaxRateQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, DateTime?, TaxRate>> expression =
                    (ctx, company_ex, taxCode_ex, rateCode_ex, effectiveFrom_ex) =>
                    (from row in ctx.TaxRate
                     where row.Company == company_ex
                        && row.TaxCode == taxCode_ex
                        && row.RateCode == rateCode_ex
                        && row.EffectiveFrom <= effectiveFrom_ex
                     orderby row.EffectiveFrom descending
                     select row).FirstOrDefault();
                findLastTaxRateQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastTaxRateQuery(this.Db, company, taxCode, rateCode, effectiveFrom);
        }

        #endregion TaxRate Queries

        #region TaxRgn Queries
        private static Func<ErpContext, string, string, bool, string> findFirstTaxRgnQuery;
        private string FindFirstTaxRgn(string Company, string TaxLiabilityType, bool isDefault)
        {
            if (findFirstTaxRgnQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, string>> expression =
                    (context, Company_ex, TaxLiabilityType_ex, isDefault_ex) =>
                    (from row in context.TaxRgn
                     where row.Company == Company_ex &&
                           row.INTaxLiabilityType == TaxLiabilityType_ex &&
                           row.INDefault == isDefault_ex
                     select row.TaxRegionCode).FirstOrDefault();
                findFirstTaxRgnQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTaxRgnQuery(this.Db, Company, TaxLiabilityType, isDefault);
        }

        private static Func<ErpContext, string, string, bool, bool> existsTaxRgnQuery;
        private bool ExistsTaxRgn(string Company, string TaxRegionCode, bool UseInAP)
        {
            if (existsTaxRgnQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
                    (context, Company_ex, TaxRegionCode_ex, UseInAP_ex) =>
                    (from row in context.TaxRgn
                     where row.Company == Company_ex &&
                     row.TaxRegionCode == TaxRegionCode_ex &&
                     row.UseInAP == UseInAP_ex
                     select row)
                    .Any();
                existsTaxRgnQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsTaxRgnQuery(this.Db, Company, TaxRegionCode, UseInAP);
        }

        private static Func<ErpContext, string, string, string, bool> existsTaxRgnQuery2;
        private bool ExistsTaxRgn(string CompanyID, string TaxRgnCode, string INTaxLiabilityType)
        {
            if (existsTaxRgnQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
                    (context, CompanyID_ex, TaxRgnCode_ex, INTaxLiabilityType_ex) =>
                    (from row in context.TaxRgn
                     where row.Company == CompanyID_ex &&
                           row.TaxRegionCode == TaxRgnCode_ex &&
                           row.INTaxLiabilityType == INTaxLiabilityType_ex
                     select row).Any();
                existsTaxRgnQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return existsTaxRgnQuery2(this.Db, CompanyID, TaxRgnCode, INTaxLiabilityType);
        }

        private static Func<ErpContext, string, string, bool, bool, bool> existsTaxRgnAvalaraQuery;
        private bool ExistsTaxRgnAvalara(string Company, string TaxRegionCode, bool UseInAp, bool TaxConnectCalc)
        {
            if (existsTaxRgnAvalaraQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool, bool>> expression =
                    (context, Company_ex, TaxRegionCode_ex, UseInAp_ex, TaxConnectCalc_ex) =>
                    (from row in context.TaxRgn
                     where row.Company == Company_ex &&
                     row.TaxRegionCode == TaxRegionCode_ex &&
                     row.UseInAP == UseInAp_ex &&
                     row.TaxConnectCalc == TaxConnectCalc_ex
                     select row)
                    .Any();
                existsTaxRgnAvalaraQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsTaxRgnAvalaraQuery(this.Db, Company, TaxRegionCode, UseInAp, TaxConnectCalc);
        }

        #endregion

        #region TranGLC Queries

        static Func<ErpContext, string, string, string, string, IEnumerable<TranGLC>> selectInvoiceTranGLCQuery;
        private IEnumerable<TranGLC> SelectInvoiceTranGLC(string company, string relatedToFile, string vendorNum, string invoiceNum)
        {
            if (selectInvoiceTranGLCQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, IEnumerable<TranGLC>>> expression =
                   (ctx, Company_ex, RelatedToFile_ex, vendorNum_ex, invoiceNum_ex) =>
                    (from row in ctx.TranGLC
                     where row.Company == Company_ex &&
                     row.RelatedToFile == RelatedToFile_ex &&
                     row.Key1 == vendorNum_ex &&
                     row.Key2 == invoiceNum_ex
                     select row);
                selectInvoiceTranGLCQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectInvoiceTranGLCQuery(this.Db, company, relatedToFile, vendorNum, invoiceNum);
        }

        static Func<ErpContext, Guid, bool> existsTranGLCQuery0;
        private bool ExistsTranGLC(Guid SysRowID)
        {
            if (existsTranGLCQuery0 == null)
            {
                Expression<Func<ErpContext, Guid, bool>> expression =
                   (ctx, SysRowID_ex) =>
                    (from row in ctx.TranGLC
                     where row.SysRowID == SysRowID_ex
                     select row.Company).Any();
                existsTranGLCQuery0 = DBExpressionCompiler.Compile(expression);
            }
            return existsTranGLCQuery0(this.Db, SysRowID);
        }

        private static Func<ErpContext, string, string, string, string, string, string, bool> existsTranGLCQuery;
        private bool ExistsTranGLC(string company, string relatedToFile, string key1, string key2, string key3, string glAcctContext)
        {
            if (existsTranGLCQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, string, bool>> expression =
                    (context, company_ex, relatedToFile_ex, key1_ex, key2_ex, key3_ex, glAcctContext_ex) =>
                    (from row in context.TranGLC
                     where row.Company == company_ex
                     && row.RelatedToFile == relatedToFile_ex
                     && row.Key1 == key1_ex
                     && row.Key2 == key2_ex
                     && row.Key3 == key3_ex
                     && row.GLAcctContext == glAcctContext_ex
                     select row)
                    .Any();
                existsTranGLCQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsTranGLCQuery(this.Db, company, relatedToFile, key1, key2, key3, glAcctContext);
        }

        private static Func<ErpContext, string, string, string, string, string, string, string, string, IEnumerable<TranGLC>> selectTranGLCQuery;
        private IEnumerable<TranGLC> SelectTranGLC(string company, string relatedToFile, string key1, string key2, string key3, string key4, string key5, string glAcctContext)
        {
            if (selectTranGLCQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, string, string, string, IEnumerable<TranGLC>>> expression =
                    (context, company_ex, relatedToFile_ex, key1_ex, key2_ex, key3_ex, key4_ex, key5_ex, glAcctContext_ex) =>
                    from row in context.TranGLC
                    where row.Company == company_ex
                    && row.RelatedToFile == relatedToFile_ex
                    && row.Key1 == key1_ex
                    && row.Key2 == key2_ex
                    && row.Key3 == key3_ex
                    && row.Key4 == key4_ex
                    && row.Key5 == key5_ex
                    && row.GLAcctContext == glAcctContext_ex
                    select row;
                selectTranGLCQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectTranGLCQuery(this.Db, company, relatedToFile, key1, key2, key3, key4, key5, glAcctContext);
        }

        private static Func<ErpContext, string, string, string, string, IEnumerable<TranGLC>> selectTranGLCQuery2;
        private IEnumerable<TranGLC> SelectTranGLCWithUpdLock(string company, string relatedToFile, string key1, string key2)
        {
            if (selectTranGLCQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, IEnumerable<TranGLC>>> expression =
                    (context, company_ex, relatedToFile_ex, key1_ex, key2_ex) =>
                    from row in context.TranGLC.With(LockHint.UpdLock)
                    where row.Company == company_ex
                    && row.RelatedToFile == relatedToFile_ex
                    && row.Key1 == key1_ex
                    && row.Key2 == key2_ex
                    select row;
                selectTranGLCQuery2 = DBExpressionCompiler.Compile(expression);
            }
            return selectTranGLCQuery2(this.Db, company, relatedToFile, key1, key2);
        }

        private static Func<ErpContext, string, string, string, string, string, string, string, string, string, string, TranGLC> findFirstTranGLCQuery;
        private TranGLC FindFirstTranGLC(string company, string relatedToFile, string key1, string key2, string key3, string key4, string key5, string glAcctContext, string bookID, string sysGLControlType)
        {
            if (findFirstTranGLCQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, string, string, string, string, string, TranGLC>> expression =
                    (context, company_ex, relatedToFile_ex, key1_ex, key2_ex, key3_ex, key4_ex, key5_ex, glAcctContext_ex, bookID_ex, sysGLControlType_ex) =>
                    (from row in context.TranGLC
                     where row.Company == company_ex
                     && row.RelatedToFile == relatedToFile_ex
                     && row.Key1 == key1_ex
                     && row.Key2 == key2_ex
                     && row.Key3 == key3_ex
                     && row.Key4 == key4_ex
                     && row.Key5 == key5_ex
                     && row.GLAcctContext == glAcctContext_ex
                     && row.BookID == bookID_ex
                     && row.SysGLControlType == sysGLControlType_ex
                     select row).FirstOrDefault();
                findFirstTranGLCQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTranGLCQuery(this.Db, company, relatedToFile, key1, key2, key3, key4, key5, glAcctContext, bookID, sysGLControlType);
        }

        private static Func<ErpContext, string, string, string, string, string, string, string, string, string, TranGLC> findFirstTranGLCQuery22;
        private TranGLC FindFirstTranGLC2(string company, string relatedToFile, string key1, string key2, string key3, string key4, string key5, string key6, string glAcctContext)
        {
            if (findFirstTranGLCQuery22 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, string, string, string, string, TranGLC>> expression =
                    (context, company_ex, relatedToFile_ex, key1_ex, key2_ex, key3_ex, key4_ex, key5_ex, key6_ex, glAcctContext_ex) =>
                    (from row in context.TranGLC
                     where row.Company == company_ex
                     && row.RelatedToFile == relatedToFile_ex
                     && row.Key1 == key1_ex
                     && row.Key2 == key2_ex
                     && row.Key3 == key3_ex
                     && row.Key4 == key4_ex
                     && row.Key5 == key5_ex
                     && row.Key6 == key6_ex
                     && row.GLAcctContext == glAcctContext_ex
                     select row).FirstOrDefault();
                findFirstTranGLCQuery22 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTranGLCQuery22(this.Db, company, relatedToFile, key1, key2, key3, key4, key5, key6, glAcctContext);
        }


        #endregion

        #region Part Queries
        private static Func<ErpContext, string, string, Part> findFirstPartQuery;
        private Part FindFirstPart(string Company, string PartNum)
        {
            if (findFirstPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, Part>> expression =
                    (context, Company_ex, PartNum_ex) =>
                    (from row in context.Part
                     where row.Company == Company_ex &&
                     row.PartNum == PartNum_ex
                     select row)
                    .FirstOrDefault();
                findFirstPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartQuery(this.Db, Company, PartNum);
        }
        #endregion Part Queries

        #region Plant Queries

        private static Func<ErpContext, string, string, string, int, Plant> findFirstPlantQuery;
        private Plant FindFirstPlant(string Company, string Plant, string invalidState, int startCountryNum)
        {
            if (findFirstPlantQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, Plant>> expression =
                    (context, Company_ex, Plant_ex, invalidState_ex, startCountryNum_ex) =>
                    (from row in context.Plant
                     where row.Company == Company_ex &&
                        row.Plant1 == Plant_ex &&
                        row.State != invalidState_ex &&
                        row.CountryNum > startCountryNum_ex
                     select row)
                    .FirstOrDefault();
                findFirstPlantQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlantQuery(this.Db, Company, Plant, invalidState, startCountryNum);
        }

        #endregion

        #region PurMisc Queries

        class PurMiscResult
        {
            public bool TakeDiscount { get; set; }
        }

        static Expression<Func<ErpContext, string, string, PurMiscResult>> PurMiscExpression13 =
         (ctx, company_ex, miscCode_ex) =>
           (from row in ctx.PurMisc
            where row.Company == company_ex &&
            row.MiscCode == miscCode_ex
            select new PurMiscResult { TakeDiscount = row.TakeDiscount }).FirstOrDefault();

        private static Func<ErpContext, string, string, bool, bool> existsPurMiscQuery;
        private bool ExistsPurMisc(string Company, string MiscCode, bool Inactive)
        {
            if (existsPurMiscQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
                    (context, Company_ex, MiscCode_ex, Inactive_ex) =>
                    (from row in context.PurMisc
                     where row.Company == Company_ex &&
                         row.MiscCode == MiscCode_ex &&
                         row.Inactive == Inactive_ex
                     select row)
                    .Any();
                existsPurMiscQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPurMiscQuery(this.Db, Company, MiscCode, Inactive);
        }

        private static Func<ErpContext, string, string, bool, bool> existsPurMiscQuery1;
        private bool ExistsPurMiscDiscount(string Company, string MiscCode, bool TakeDiscount)
        {
            if (existsPurMiscQuery1 == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
                    (context, Company_ex, MiscCode_ex, TakeDiscount_ex) =>
                    (from row in context.PurMisc
                     where row.Company == Company_ex &&
                     row.MiscCode == MiscCode_ex &&
                     row.TakeDiscount == TakeDiscount_ex
                     select row)
                    .Any();
                existsPurMiscQuery1 = DBExpressionCompiler.Compile(expression);
            }

            return existsPurMiscQuery1(this.Db, Company, MiscCode, TakeDiscount);
        }
        #endregion

        #region POHeader Queries

        private class POHeaderTaxRegCodePartialRow
        {
            public string TaxRegionCode { get; set; }
        }

        private static Func<ErpContext, string, int, int, POHeaderTaxRegCodePartialRow> findFirstPOHeaderTaxRegionCodeQuery;
        private POHeaderTaxRegCodePartialRow FindFirstPOTaxRegionCode(string Company, int VendorNum, int PONum)
        {
            if (findFirstPOHeaderTaxRegionCodeQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, POHeaderTaxRegCodePartialRow>> expression =
                    (ctx, Company_ex, VendorNum_ex, PONum_ex) =>
                     (from row in ctx.POHeader
                      where row.Company == Company_ex &&
                      row.VendorNum == VendorNum_ex &&
                      row.PONum == PONum_ex
                      select new POHeaderTaxRegCodePartialRow { TaxRegionCode = row.TaxRegionCode }).FirstOrDefault();

                findFirstPOHeaderTaxRegionCodeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPOHeaderTaxRegionCodeQuery(this.Db, Company, VendorNum, PONum);
        }
        #endregion

        #region PurTerms Queries
        static Func<ErpContext, string, string, PurTerms> findFirstPurTermsQuery;
        private PurTerms FindFirstPurTerms(string company, string termsCode)
        {
            if (findFirstPurTermsQuery == null)
            {
                Expression<Func<ErpContext, string, string, PurTerms>> expression =
                    (ctx, company_ex, termsCode_ex) =>
                (from row in ctx.PurTerms
                 where row.Company == company_ex &&
                 row.TermsCode == termsCode_ex
                 select row).FirstOrDefault();

                findFirstPurTermsQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPurTermsQuery(this.Db, company, termsCode);
        }
        #endregion PurTerms Queries

        #region Vendor Queries
        private static Func<ErpContext, string, int, Vendor> findFirstVendorQuery;
        private Vendor FindFirstVendor(string Company, int VendorNum)
        {
            if (findFirstVendorQuery == null)
            {
                Expression<Func<ErpContext, string, int, Vendor>> expression =
                    (context, Company_ex, VendorNum_ex) =>
                    (from row in context.Vendor
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex
                     select row)
                    .FirstOrDefault();
                findFirstVendorQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstVendorQuery(this.Db, Company, VendorNum);
        }

        static Func<ErpContext, string, int, string> getVendorTaxPayerIDQuery;
        private string GetVendorTaxPayerID(string company, int vendorNum)
        {
            if (getVendorTaxPayerIDQuery == null)
            {
                Expression<Func<ErpContext, string, int, string>> expression =
                (ctx, company_ex, vendorNum_ex) =>
                    (from row in ctx.Vendor
                     where row.Company == company_ex
                     && row.VendorNum == vendorNum_ex
                     select row.TaxPayerID).FirstOrDefault();
                getVendorTaxPayerIDQuery = DBExpressionCompiler.Compile(expression);
            }

            return getVendorTaxPayerIDQuery(this.Db, company, vendorNum);
        }
        #endregion

        #region VendorPP Queries
        private static Func<ErpContext, string, string, int, string, int, VendorPP> findFirstVendorPPQuery;
        private VendorPP FindFirstVendorPP(string Company, string PurPoint, int VendorNum, string invalidState, int startCountryNum)
        {
            if (findFirstVendorPPQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, int, VendorPP>> expression =
                    (context, Company_ex, PurPoint_ex, VendorNum_ex, invalidState_ex, startCountryNum_ex) =>
                    (from row in context.VendorPP
                     where row.Company == Company_ex &&
                     row.PurPoint == PurPoint_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.State != invalidState_ex &&
                     row.CountryNum > startCountryNum_ex
                     select row)
                    .FirstOrDefault();
                findFirstVendorPPQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstVendorPPQuery(this.Db, Company, PurPoint, VendorNum, invalidState, startCountryNum);
        }
        #endregion

        #region VendBank Queries
        private class VendorBankJoinResult : Epicor.Data.TempRowBase
        {
            public int VendorNum { get; set; }
            public string VendorID { get; set; }
            public string VendorName { get; set; }
            public string VendorBankID { get; set; }
            public string VendorBankType { get; set; }
            public string VendorBankCustNum { get; set; }
            public int VendorBankCustNumStartPos { get; set; }
            public int VendorBankCustNumLen { get; set; }
        }

        private static Func<ErpContext, string, string, IEnumerable<VendorBankJoinResult>> selectVendorBankQuery;
        private IEnumerable<VendorBankJoinResult> SelectVendorBank(string company, string ISRPartyID)
        {
            if (selectVendorBankQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<VendorBankJoinResult>>> expression =
                    (context, company_ex, ISRPartyID_ex) =>
                    (from Vendor_Row in context.Vendor
                     join VendBank_Row in context.VendBank
                        on new { Vendor_Row.Company, Vendor_Row.VendorNum } equals new { VendBank_Row.Company, VendBank_Row.VendorNum }
                     where Vendor_Row.Company == company_ex &&
                     VendBank_Row.CHISRPartyID == ISRPartyID_ex
                     select new VendorBankJoinResult()
                     {
                         VendorNum = Vendor_Row.VendorNum,
                         VendorID = Vendor_Row.VendorID,
                         VendorName = Vendor_Row.Name,
                         VendorBankID = VendBank_Row.BankID,
                         VendorBankType = VendBank_Row.CardCode,
                         VendorBankCustNum = VendBank_Row.BankCustNum,
                         VendorBankCustNumStartPos = VendBank_Row.BankCustNumberStartPos,
                         VendorBankCustNumLen = VendBank_Row.BankCustNumberLen
                     });
                selectVendorBankQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectVendorBankQuery(this.Db, company, ISRPartyID);
        }

        private static Func<ErpContext, string, string, IEnumerable<VendorBankJoinResult>> selectVendorBankQuery_1;
        private IEnumerable<VendorBankJoinResult> SelectVendorBank2(string company, string POBankAcctNum)
        {
            if (selectVendorBankQuery_1 == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<VendorBankJoinResult>>> expression =
                    (context, company_ex, POBankAcctNum_ex) =>
                    (from Vendor_Row in context.Vendor
                     join VendBank_Row in context.VendBank
                        on new { Vendor_Row.Company, Vendor_Row.VendorNum } equals new { VendBank_Row.Company, VendBank_Row.VendorNum }
                     where Vendor_Row.Company == company_ex &&
                     VendBank_Row.POBankAcctNum == POBankAcctNum_ex
                     select new VendorBankJoinResult()
                     {
                         VendorNum = Vendor_Row.VendorNum,
                         VendorID = Vendor_Row.VendorID,
                         VendorName = Vendor_Row.Name,
                         VendorBankID = VendBank_Row.BankID,
                         VendorBankType = VendBank_Row.CardCode,
                         VendorBankCustNum = VendBank_Row.BankCustNum,
                         VendorBankCustNumStartPos = VendBank_Row.BankCustNumberStartPos,
                         VendorBankCustNumLen = VendBank_Row.BankCustNumberLen
                     });
                selectVendorBankQuery_1 = DBExpressionCompiler.Compile(expression);
            }

            return selectVendorBankQuery_1(this.Db, company, POBankAcctNum);
        }

        private static Func<ErpContext, string, int, string, bool> existsVendBankQuery;
        private bool ExistsVendBank(string company, int vendorNum, string ISRPartyID)
        {
            if (existsVendBankQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                  (ctx, company_ex, vendorNum_ex, ISRPartyID_ex) =>
                    (from row in ctx.VendBank
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.CHISRPartyID == ISRPartyID_ex
                     select row).Any();
                existsVendBankQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsVendBankQuery(this.Db, company, vendorNum, ISRPartyID);
        }

        private static Func<ErpContext, string, int, string, bool> existsVendBankQuery_1;
        private bool ExistsVendBank2(string company, int vendorNum, string POBankAcctNum)
        {
            if (existsVendBankQuery_1 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                  (ctx, company_ex, vendorNum_ex, POBankAcctNum_ex) =>
                    (from row in ctx.VendBank
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.POBankAcctNum == POBankAcctNum_ex
                     select row).Any();
                existsVendBankQuery_1 = DBExpressionCompiler.Compile(expression);
            }

            return existsVendBankQuery_1(this.Db, company, vendorNum, POBankAcctNum);
        }

        static Func<ErpContext, string, string, int, string> findFirstVendBankNameQuery;
        private string FindFirstVendBankName(string company, string bankID, int vendorNum)
        {
            if (findFirstVendBankNameQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string>> expression =
      (ctx, company_ex, bankID_ex, vendorNum_ex) =>
        (from row in ctx.VendBank
         where row.Company == company_ex &&
         row.VendorNum == vendorNum_ex &&
         row.BankID == bankID_ex
         select row.BankName).FirstOrDefault();
                findFirstVendBankNameQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstVendBankNameQuery(this.Db, company, bankID, vendorNum);
        }

        static Func<ErpContext, string, int, VendBank> findFirstVendBankQuery;
        private VendBank FindFirstVendBank(string company, int vendorNum)
        {
            if (findFirstVendBankQuery == null)
            {
                Expression<Func<ErpContext, string, int, VendBank>> expression =
      (ctx, company_ex, vendorNum_ex) =>
        (from row in ctx.VendBank
         where row.Company == company_ex &&
         row.VendorNum == vendorNum_ex
         select row).FirstOrDefault();
                findFirstVendBankQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstVendBankQuery(this.Db, company, vendorNum);
        }

        static Func<ErpContext, string, int, Vendor> findFirstVendor7Query;
        private Vendor FindFirstVendor7(string company, int vendorNum)
        {
            if (findFirstVendor7Query == null)
            {
                Expression<Func<ErpContext, string, int, Vendor>> expression =
      (ctx, company_ex, vendorNum_ex) =>
        (from row in ctx.Vendor
         where row.Company == company_ex &&
         row.VendorNum == vendorNum_ex
         select row).FirstOrDefault();
                findFirstVendor7Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstVendor7Query(this.Db, company, vendorNum);
        }

        #endregion

        #region RASchedCd Queries

        static Func<ErpContext, string, string, string, string, bool, RASchedCd> findFirstRASchedCdQuery;
        private RASchedCd FindFirstRASchedCd(string company, string racode, string scope1, string scope2, bool active)
        {
            if (findFirstRASchedCdQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, bool, RASchedCd>> expression =
                  (ctx, company_ex, racode_ex, scope1_ex, scope2_ex, active_ex) =>
                    (from row in ctx.RASchedCd
                     where row.Company == company_ex &&
                     row.RACode == racode_ex &&
                     (row.Scope == scope1_ex || row.Scope == scope2_ex) &&
                     row.Active == active_ex
                     select row).FirstOrDefault();
                findFirstRASchedCdQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstRASchedCdQuery(this.Db, company, racode, scope1, scope2, active);
        }

        #endregion RASchedCd Queries

        #region RcvMisc Queries

        private static Func<ErpContext, string, int, string, string, int, RcvMisc> findFirstRcvMiscQuery;
        private RcvMisc FindFirstRcvMisc(string Company, int VendorNum, string PurPoint, string PackSlip, int MiscSeq)
        {
            if (findFirstRcvMiscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, RcvMisc>> expression =
                    (context, Company_ex, VendorNum_ex, PurPoint_ex, PackSlip_ex, MiscSeq_ex) =>
                    (from row in context.RcvMisc
                     where row.Company == Company_ex
                     && row.VendorNum == VendorNum_ex
                     && row.PurPoint == PurPoint_ex
                     && row.PackSlip == PackSlip_ex
                     && row.MiscSeq == MiscSeq_ex
                     select row)
                    .FirstOrDefault();
                findFirstRcvMiscQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstRcvMiscQuery(this.Db, Company, VendorNum, PurPoint, PackSlip, MiscSeq);
        }

        private static Func<ErpContext, string, int, string, int, int, IEnumerable<RcvMisc>> SelectRcvMiscWithUpdLockQuery;
        private IEnumerable<RcvMisc> SelectRcvMiscWithUpdLock(string company, int vendorNum, string invoiceNum, int invoiceLine, int miscNum)
        {
            if (SelectRcvMiscWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, IEnumerable<RcvMisc>>> expression =
                    (context, company_ex, vendorNum_ex, invoiceNum_ex, invoiceLine_ex, miscNum_ex) =>
                                    (from row in context.RcvMisc.With(LockHint.UpdLock)
                                     where row.Company == company_ex &&
                                           row.APInvVendorNum == vendorNum_ex &&
                                           row.InvoiceNum == invoiceNum_ex &&
                                           row.InvoiceLine == invoiceLine_ex &&
                                           row.MscNum == miscNum_ex
                                     select row);
                SelectRcvMiscWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return SelectRcvMiscWithUpdLockQuery(this.Db, company, vendorNum, invoiceNum, invoiceLine, miscNum);
        }

        class RcvMscPartial
        {
            public string Company { get; set; }

            public int VendorNum { get; set; }

            public string PurPoint { get; set; }

            public string PackSlip { get; set; }

            public int PackLine { get; set; }
            public decimal DocActualAmt { get; set; }
        }

        private static Func<ErpContext, string, int, string, int, int, IEnumerable<RcvMscPartial>> selectRcvMiscPartialQuery;
        private IEnumerable<RcvMscPartial> SelectRcvMiscPartial(string company, int vendorNum, string invoiceNum, int invoiceLine, int mscNum)
        {
            if (selectRcvMiscPartialQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, IEnumerable<RcvMscPartial>>> expression =
                    (context, company_ex, vendorNum_ex, invoiceNum_ex, invoiceLine_ex, mscNum_ex) =>
                                    (from row in context.RcvMisc
                                     where row.Company == company_ex &&
                                           row.APInvVendorNum == vendorNum_ex &&
                                           row.InvoiceNum == invoiceNum_ex &&
                                           row.InvoiceLine == invoiceLine_ex &&
                                           row.MscNum == mscNum_ex
                                     select new RcvMscPartial()
                                     {
                                         Company = row.Company,
                                         VendorNum = row.VendorNum,
                                         PurPoint = row.PurPoint,
                                         PackSlip = row.PackSlip,
                                         PackLine = row.PackLine,
                                         DocActualAmt = row.DocActualAmt
                                     });
                selectRcvMiscPartialQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectRcvMiscPartialQuery(this.Db, company, vendorNum, invoiceNum, invoiceLine, mscNum);
        }



        private static Func<ErpContext, string, int, string, int, int, int, RcvMscPartial> findFirstRcvMscDocActualAmountQuery;
        private RcvMscPartial FindFirstRcvMscPartial(string company, int vendorNum, string invoiceNum, int invoiceLine, int mscNum, int miscSeq)
        {
            if (findFirstRcvMscDocActualAmountQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, int, RcvMscPartial>> expression =
                    (context, company_ex, vendorNum_ex, invoiceNum_ex, invoiceLine_ex, mscNum_ex, miscSeq_ex) =>
                                    (from row in context.RcvMisc
                                     where row.Company == company_ex &&
                                           row.APInvVendorNum == vendorNum_ex &&
                                           row.InvoiceNum == invoiceNum_ex &&
                                           row.InvoiceLine == invoiceLine_ex &&
                                           row.MscNum == mscNum_ex &&
                                           row.MiscSeq == miscSeq_ex
                                     select new RcvMscPartial()
                                     {
                                         Company = row.Company,
                                         VendorNum = row.VendorNum,
                                         PurPoint = row.PurPoint,
                                         PackSlip = row.PackSlip,
                                         PackLine = row.PackLine,
                                         DocActualAmt = row.DocActualAmt
                                     }).FirstOrDefault();
                findFirstRcvMscDocActualAmountQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstRcvMscDocActualAmountQuery(this.Db, company, vendorNum, invoiceNum, invoiceLine, mscNum, miscSeq);
        }
        #endregion

        #region RcvDtl Queries

        private static Func<ErpContext, string, string, int, RcvDtl> findFirstRcvDtlQuery;
        private RcvDtl FindFirstRcvDtl(string Company, string PackSlip, int PONum)
        {
            if (findFirstRcvDtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, RcvDtl>> expression =
                    (context, Company_ex, PackSlip_ex, PONum_ex) =>
                    (from row in context.RcvDtl
                     where row.Company == Company_ex &&
                     row.PackSlip == PackSlip_ex &&
                     row.PONum == PONum_ex
                     select row)
                    .FirstOrDefault();
                findFirstRcvDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstRcvDtlQuery(this.Db, Company, PackSlip, PONum);
        }

        private static Func<ErpContext, string, string, int, int, decimal> getRcvDtlLineSuppQtyQuery;
        private decimal getRcvDtlLineSuppQty(string Company, string PackSlip, int PONum, int Line)
        {
            if (getRcvDtlLineSuppQtyQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, decimal>> expression =
                    (context, Company_ex, PackSlip_ex, PONum_ex, Line_ex) =>
                    (from row in context.RcvDtl
                     where row.Company == Company_ex &&
                     row.PackSlip == PackSlip_ex &&
                     row.PONum == PONum_ex &&
                     row.PackLine == Line_ex
                     select row.VendorQty)
                    .FirstOrDefault();
                getRcvDtlLineSuppQtyQuery = DBExpressionCompiler.Compile(expression);
            }

            return getRcvDtlLineSuppQtyQuery(this.Db, Company, PackSlip, PONum, Line);
        }

        private static Func<ErpContext, string, string, int, int, decimal> getRcvDtlLineSuppUninvoicedQtyQuery;
        private decimal getRcvDtlLineSuppUninvoicedQty(string Company, string PackSlip, int PONum, int PackLine)
        {
            if (getRcvDtlLineSuppUninvoicedQtyQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, decimal>> expression =
                    (context, Company_ex, PackSlip_ex, PONum_ex, PackLine_ex) =>
                    (from row in context.RcvDtl
                     where row.Company == Company_ex &&
                     row.PackSlip == PackSlip_ex &&
                     row.PONum == PONum_ex &&
                     row.PackLine == PackLine_ex
                     select row.SupplierUnInvcReceiptQty)
                    .FirstOrDefault();
                getRcvDtlLineSuppUninvoicedQtyQuery = DBExpressionCompiler.Compile(expression);
            }

            return getRcvDtlLineSuppUninvoicedQtyQuery(this.Db, Company, PackSlip, PONum, PackLine);
        }

        private static Func<ErpContext, string, string, int, string, bool> existsRcvDtlNotInvoicedQuery;
        private bool existsRcvDtlNotInvoiced(string Company, string PackSlip, int VendorNum, string InvoiceNum)
        {
            if (existsRcvDtlNotInvoicedQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, bool>> expression =
                    (context, Company_ex, PackSlip_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from RcvDtl in context.RcvDtl
                     where RcvDtl.Company == Company_ex &&
                     RcvDtl.PackSlip == PackSlip_ex &&
                     RcvDtl.VendorNum == VendorNum_ex &&
                     !(from APInvDtl in context.APInvDtl
                       where APInvDtl.Company == RcvDtl.Company &&
                       APInvDtl.InvoiceNum == InvoiceNum_ex &&
                       APInvDtl.PONum == RcvDtl.PONum &&
                       APInvDtl.VendorNum == RcvDtl.VendorNum &&
                       APInvDtl.PackSlip == RcvDtl.PackSlip &&
                       APInvDtl.PackLine == RcvDtl.PackLine
                       select RcvDtl).Any()
                     select RcvDtl).Any();
                existsRcvDtlNotInvoicedQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsRcvDtlNotInvoicedQuery(this.Db, Company, PackSlip, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, string, int, int, string, bool> existsRcvDtlNotInvoiced2Query;
        private bool existsRcvDtlNotInvoiced2(string Company, string PackSlip, int VendorNum, int PackLine, string InvoiceNum)
        {
            if (existsRcvDtlNotInvoiced2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, bool>> expression =
                    (context, Company_ex, PackSlip_ex, VendorNum_ex, PackLine_ex, InvoiceNum_ex) =>
                    (from RcvDtl in context.RcvDtl
                     where RcvDtl.Company == Company_ex &&
                     RcvDtl.PackSlip == PackSlip_ex &&
                     RcvDtl.VendorNum == VendorNum_ex &&
                     RcvDtl.PackLine == PackLine_ex &&
                     !(from APInvDtl in context.APInvDtl
                       where APInvDtl.Company == RcvDtl.Company &&
                       APInvDtl.InvoiceNum == InvoiceNum_ex &&
                       APInvDtl.PONum == RcvDtl.PONum &&
                       APInvDtl.VendorNum == RcvDtl.VendorNum &&
                       APInvDtl.PackSlip == RcvDtl.PackSlip &&
                       APInvDtl.PackLine == RcvDtl.PackLine
                       select RcvDtl).Any()
                     select RcvDtl).Any();
                existsRcvDtlNotInvoiced2Query = DBExpressionCompiler.Compile(expression);
            }

            return existsRcvDtlNotInvoiced2Query(this.Db, Company, PackSlip, VendorNum, PackLine, InvoiceNum);
        }

        static Func<ErpContext, string, int, string, string, int, RcvDtl> findFirstRcvDtlWithUpdLockQuery;
        private RcvDtl FindFirstRcvDtlWithUpdLock(string company, int vendorNum, string purPoint, string packSlip, int packLine)
        {
            if (findFirstRcvDtlWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, RcvDtl>> expression =
      (ctx, company_ex, vendorNum_ex, purPoint_ex, packSlip_ex, packLine_ex) =>
        (from row in ctx.RcvDtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.VendorNum == vendorNum_ex &&
         row.PurPoint == purPoint_ex &&
         row.PackSlip == packSlip_ex &&
         row.PackLine == packLine_ex
         select row).FirstOrDefault();
                findFirstRcvDtlWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstRcvDtlWithUpdLockQuery(this.Db, company, vendorNum, purPoint, packSlip, packLine);
        }

        static Func<ErpContext, string, int, string, string, int, RcvDtl> findFirstRcvDtlQuery9;
        private RcvDtl FindFirstRcvDtl(string company, int vendorNum, string purPoint, string packSlip, int packLine)
        {
            if (findFirstRcvDtlQuery9 == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, RcvDtl>> expression =
      (ctx, company_ex, vendorNum_ex, purPoint_ex, packSlip_ex, packLine_ex) =>
        (from row in ctx.RcvDtl
         where row.Company == company_ex &&
         row.VendorNum == vendorNum_ex &&
         row.PurPoint == purPoint_ex &&
         row.PackSlip == packSlip_ex &&
         row.PackLine == packLine_ex
         select row).FirstOrDefault();
                findFirstRcvDtlQuery9 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstRcvDtlQuery9(this.Db, company, vendorNum, purPoint, packSlip, packLine);
        }

        private static Func<ErpContext, string, int, int, int, string, IEnumerable<RcvDtl>> selectRcvDtlQuery;
        private IEnumerable<RcvDtl> SelectRcvDtl(string Company, int ipVendorList, int ipSelectedPOsList, int ipSelectedPOLine, string ipSelectedPackSlip)
        {
            if (selectRcvDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, string, IEnumerable<RcvDtl>>> expression =
                    (context, Company_ex, IpVendorList_ex, IpSelectedPOsList_ex, IpSelectedPOLine_ex, IpSelectedPackSlip_ex) =>
                    from row in context.RcvDtl.With(LockHint.UpdLock)
                    where row.Company == Company_ex &&
                     row.OurUnInvcReceiptQty != 0 &&
                     row.Received == true &&
                     row.VendorNum == IpVendorList_ex &&
                     row.PONum == IpSelectedPOsList_ex &&
                     row.POLine == IpSelectedPOLine_ex &&
                     row.PackSlip == IpSelectedPackSlip_ex
                    select row;
                selectRcvDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectRcvDtlQuery(this.Db, Company, ipVendorList, ipSelectedPOsList, ipSelectedPOLine, ipSelectedPackSlip);
        }

        private static Func<ErpContext, string, string, IEnumerable<RcvDtl>> selectRcvDtlQuery2;
        private IEnumerable<RcvDtl> SelectRcvDtl(string Company, string ipVendorList)
        {
            if (selectRcvDtlQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<RcvDtl>>> expression =
                    (context, Company_ex, IpVendorList_ex) =>
                    from row in context.RcvDtl.With(LockHint.UpdLock)
                    where row.Company == Company_ex &&
                        row.OurUnInvcReceiptQty != 0 &&
                        row.Received == true &&
                        (string.IsNullOrEmpty(IpVendorList_ex) || ErpEFFunctions.ListLookup(SqlFunctions.StringConvert((decimal)row.VendorNum).Trim(), IpVendorList_ex, Ice.Constants.LIST_DELIM) > -1)
                    select row;
                selectRcvDtlQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectRcvDtlQuery2(this.Db, Company, ipVendorList);
        }

        private static Func<ErpContext, string, string, bool, bool> validateRcvDtlQuery;
        private bool ValidateRcvDtl(string Company, string ipVendorList, bool Invoiced)
        {
            if (validateRcvDtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
                    (context, Company_ex, IpVendorList_ex, Invoiced_ex) =>
                    (from row in context.RcvDtl
                     join APInvDtlJoin in context.APInvDtl on new { row.Company, row.VendorNum, row.PONum, row.PackSlip, row.PackLine, row.PartNum }
                                                             equals new { APInvDtlJoin.Company, APInvDtlJoin.VendorNum, APInvDtlJoin.PONum, APInvDtlJoin.PackSlip, APInvDtlJoin.PackLine, APInvDtlJoin.PartNum }
                     join APInvHedJoin in context.APInvHed on new { APInvDtlJoin.Company, APInvDtlJoin.InvoiceNum, APInvDtlJoin.VendorNum }
                                                             equals new { APInvHedJoin.Company, APInvHedJoin.InvoiceNum, APInvHedJoin.VendorNum }
                     where row.Company == Company_ex &&
                         row.OurUnInvcReceiptQty != 0 &&
                         row.Invoiced == Invoiced_ex &&
                         APInvHedJoin.Posted == false &&
                         (string.IsNullOrEmpty(IpVendorList_ex) || ErpEFFunctions.ListLookup(SqlFunctions.StringConvert((decimal)row.VendorNum).Trim(), IpVendorList_ex, Ice.Constants.LIST_DELIM) > -1)
                     select row).Any();
                validateRcvDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return validateRcvDtlQuery(this.Db, Company, ipVendorList, Invoiced);
        }

        #endregion

        #region Warehse Queries

        private static Func<ErpContext, string, string, Warehse> findFirstWarehseQuery;
        private Warehse FindFirstWarehse(string Company, string WareHouseCode)
        {
            if (findFirstWarehseQuery == null)
            {
                Expression<Func<ErpContext, string, string, Warehse>> expression =
              (ctx, Company_ex, WareHouseCode_ex) =>
                (from row in ctx.Warehse
                 where row.Company == Company_ex &&
                 row.WarehouseCode == WareHouseCode_ex
                 select row).FirstOrDefault();
                findFirstWarehseQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstWarehseQuery(this.Db, Company, WareHouseCode);
        }
        #endregion

        #region SalesTax Queries
        private static Func<ErpContext, string, string, int, bool> existsSalesTaxQuery;
        private bool ExistsSalesTax(string ipCompany, string ipTaxCode, int ipCollectionType)
        {
            if (existsSalesTaxQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, bool>> expression =
                (ctx, ipCompany_ex, ipTaxCode_ex, ipCollectionType_ex) =>
                (from row in ctx.SalesTax
                 where row.Company == ipCompany_ex &&
                       row.TaxCode == ipTaxCode_ex &&
                       row.CollectionType == ipCollectionType_ex
                 select row).Any();
                existsSalesTaxQuery = DBExpressionCompiler.Compile(expression);
            }
            return existsSalesTaxQuery(this.Db, ipCompany, ipTaxCode, ipCollectionType);
        }

        static Func<ErpContext, string, string, string, SalesTax> findFirstSalesTaxQuery;
        private SalesTax FindFirstSalesTax(string company, string taxCode, string ipSpecialTax)
        {
            if (findFirstSalesTaxQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, SalesTax>> expression =
      (ctx, company_ex, taxCode_ex, ipSpecialTax_ex) =>
        (from row in ctx.SalesTax
         where row.Company == company_ex &&
         row.TaxCode == taxCode_ex &&
         row.PETaxOriginType == ipSpecialTax_ex
         select row).FirstOrDefault();
                findFirstSalesTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSalesTaxQuery(this.Db, company, taxCode, ipSpecialTax);
        }

        #endregion SalesTax Queries

        #region TranDocTypeAuth Queries
        private class TranDocTypeAuthPartialRow : Epicor.Data.TempRowBase
        {
            public string TranDocTypeID { get; set; }
        }

        private static Func<ErpContext, string, string, string, bool, TranDocTypeAuthPartialRow> findFirstTranDocTypeAuthQuery;
        private TranDocTypeAuthPartialRow FindFirstTranDocTypeAuth(string company, string userID, string docType, bool defaultTranDocType)
        {
            if (findFirstTranDocTypeAuthQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool, TranDocTypeAuthPartialRow>> expression =
                    (context, company_ex, userID_ex, docType_ex, defaultTranDocType_ex) =>
                    (from row in context.TranDocTypeAuth
                     where row.Company == company_ex &&
                           row.DcdUserID == userID_ex &&
                           row.SystemTranID == docType_ex &&
                           row.DefaultTranDocType == defaultTranDocType_ex
                     select new TranDocTypeAuthPartialRow { TranDocTypeID = row.TranDocTypeID })
                    .FirstOrDefault();
                findFirstTranDocTypeAuthQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTranDocTypeAuthQuery(this.Db, company, userID, docType, defaultTranDocType);
        }
        #endregion

        #region TaxRgn Queries
        private class TaxRegPartialRow
        {
            public bool InPrice { get; set; }
        }

        private static Func<ErpContext, string, string, TaxRegPartialRow> findFirstInPriceTaxRegionCodeQuery;
        private TaxRegPartialRow FindFirstInPriceTaxRegionCode(string Company, string TaxRegionCode)
        {
            if (findFirstInPriceTaxRegionCodeQuery == null)
            {
                Expression<Func<ErpContext, string, string, TaxRegPartialRow>> expression =
                    (ctx, Company_ex, TaxRegionCode_ex) =>
                     (from row in ctx.TaxRgn
                      where row.Company == Company_ex &&
                      row.TaxRegionCode == TaxRegionCode_ex
                      select new TaxRegPartialRow { InPrice = row.InPrice }).FirstOrDefault();

                findFirstInPriceTaxRegionCodeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstInPriceTaxRegionCodeQuery(this.Db, Company, TaxRegionCode);
        }

        #endregion

        #region XFileAttch Queries
        private class XFileAttchFileRefJoinResult : Epicor.Data.TempRowBase
        {
            public string Company { get; set; }
            public string RelatedToSchemaName { get; set; }
            public int XFileRefNum { get; set; }
            public string XFileName { get; set; }
            public string XFileDesc { get; set; }
            public string DocTypeID { get; set; }
            public string BaseFileName { get; set; }
            public string ExternalSystemDoc { get; set; }
        }

        private static Func<ErpContext, string, string, string, string, string, IEnumerable<XFileAttchFileRefJoinResult>> selectXFileAttchFileRefQuery;
        private IEnumerable<XFileAttchFileRefJoinResult> SelectXFileAttchFileRefQuery(string company, string relatedToSchemaName, string relatedToFile, string key1, string key2)
        {
            if (selectXFileAttchFileRefQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, IEnumerable<XFileAttchFileRefJoinResult>>> expression =
                    (context, company_ex, relatedToSchemaName_ex, relatedToFile_ex, key1_ex, key2_ex) =>
                    (from XFileAttch_row in context.XFileAttch
                     join XFileRef_row in context.XFileRef
                        on new { XFileAttch_row.Company, XFileAttch_row.XFileRefNum }
                        equals new { XFileRef_row.Company, XFileRef_row.XFileRefNum }
                     where XFileAttch_row.Company == company_ex
                     && XFileAttch_row.RelatedToSchemaName == relatedToSchemaName_ex
                     && XFileAttch_row.RelatedToFile == relatedToFile_ex
                     && XFileAttch_row.Key1 == key1_ex
                     && XFileAttch_row.Key2 == key2_ex
                     select new XFileAttchFileRefJoinResult()
                     {
                         Company = XFileAttch_row.Company
                         ,
                         RelatedToSchemaName = XFileAttch_row.RelatedToSchemaName
                         ,
                         XFileRefNum = XFileAttch_row.XFileRefNum
                         ,
                         XFileName = XFileRef_row.XFileName
                         ,
                         XFileDesc = XFileRef_row.XFileDesc
                         ,
                         DocTypeID = XFileRef_row.DocTypeID
                         ,
                         BaseFileName = XFileRef_row.BaseFileName
                         ,
                         ExternalSystemDoc = XFileRef_row.ExternalSystemDoc
                     });
                selectXFileAttchFileRefQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectXFileAttchFileRefQuery(this.Db, company, relatedToSchemaName, relatedToFile, key1, key2);
        }

        #endregion XFileAttch Queries

        static Func<ErpContext, string, int, string, int, int, int, bool> existsAPInvHedWthTax;
        private bool ExistsAPInvHedWthTax(string company, int vendorNum, string invoiceNum, int wthCollectionType, int partPaidTiming, int fullyPaidTiming)
        {
            if (existsAPInvHedWthTax == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, int, bool>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceNum_ex, wthCollectionType_ex, partPaidTiming_ex, fullyPaidTiming_ex) =>
                    (from APInvHedRow in ctx.APInvHed
                     join TaxRgnSalesTaxRow in ctx.TaxRgnSalesTax on new { APInvHedRow.Company, APInvHedRow.TaxRegionCode }
                     equals new { TaxRgnSalesTaxRow.Company, TaxRgnSalesTaxRow.TaxRegionCode }
                     join SalesTaxRow in ctx.SalesTax on new { TaxRgnSalesTaxRow.Company, TaxRgnSalesTaxRow.TaxCode }
                     equals new { SalesTaxRow.Company, SalesTaxRow.TaxCode }
                     where APInvHedRow.Company == company_ex &&
                     APInvHedRow.VendorNum == vendorNum_ex &&
                     APInvHedRow.InvoiceNum == invoiceNum_ex &&
                     SalesTaxRow.CollectionType == wthCollectionType_ex &&
                     (SalesTaxRow.Timing == partPaidTiming_ex ||
                     SalesTaxRow.Timing == fullyPaidTiming_ex)
                     select APInvHedRow).Any();
                existsAPInvHedWthTax = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedWthTax(this.Db, company, vendorNum, invoiceNum, wthCollectionType, partPaidTiming, fullyPaidTiming);
        }

        static Func<ErpContext, string, int, string, int, int, int, bool> existsAPInvDtlWthTax;
        private bool ExistsAPInvDtlWthTax(string company, int vendorNum, string invoiceNum, int wthCollectionType, int partPaidTiming, int fullyPaidTiming)
        {
            if (existsAPInvDtlWthTax == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, int, bool>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceNum_ex, wthCollectionType_ex, partPaidTiming_ex, fullyPaidTiming_ex) =>
                    (from APInvDtlRow in ctx.APInvDtl
                     join TaxRgnSalesTaxRow in ctx.TaxRgnSalesTax on new { APInvDtlRow.Company, APInvDtlRow.TaxRegionCode }
                     equals new { TaxRgnSalesTaxRow.Company, TaxRgnSalesTaxRow.TaxRegionCode }
                     join SalesTaxRow in ctx.SalesTax on new { TaxRgnSalesTaxRow.Company, TaxRgnSalesTaxRow.TaxCode }
                     equals new { SalesTaxRow.Company, SalesTaxRow.TaxCode }
                     where APInvDtlRow.Company == company_ex &&
                     APInvDtlRow.VendorNum == vendorNum_ex &&
                     APInvDtlRow.InvoiceNum == invoiceNum_ex &&
                     SalesTaxRow.CollectionType == wthCollectionType_ex &&
                     (SalesTaxRow.Timing == partPaidTiming_ex ||
                     SalesTaxRow.Timing == fullyPaidTiming_ex)
                     select APInvDtlRow).Any();
                existsAPInvDtlWthTax = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvDtlWthTax(this.Db, company, vendorNum, invoiceNum, wthCollectionType, partPaidTiming, fullyPaidTiming);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<APInvMsc>> selectAPInvMscWithUpdLockQuery;
        private IEnumerable<APInvMsc> SelectAPInvMscWithUpdLock(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAPInvMscWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvMsc>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    from row in context.APInvMsc.With(LockHint.UpdLock)
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex
                    select row;
                selectAPInvMscWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvMscWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        private class DMRDebitMemoPartialRow : Epicor.Data.TempRowBase
        {
            public string CostPerCode { get; set; }
            public string CurrencyCode { get; set; }
            public int DMRNum { get; set; }
            public Decimal ExtAmount { get; set; }
            public string IUM { get; set; }
            public Decimal OurUnitCost { get; set; }
            public string PartNum { get; set; }
            public int PONum { get; set; }
            public Decimal Quantity { get; set; }
            public DateTime? ReceiptDate { get; set; }
            public string RevisionNum { get; set; }
            public int VendorNum { get; set; }
            public Decimal VendorQty { get; set; }
            public string VendorName { get; set; }
            public int ActionNum { get; set; }
            public Guid SysRowID { get; set; }
        }

        class StringComparerInvariant : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(string obj)
            {
                return obj.GetHashCode();
            }
        }

        private static Func<ErpContext, string, string, int, IEnumerable<DMRDebitMemoPartialRow>> selectDMRDMQuery;
        private IEnumerable<DMRDebitMemoPartialRow> SelectDMRDMQuery(string Company, List<string> ipSuppList, string actionType, int debitMemoLine)
        {
            if (selectDMRDMQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, IEnumerable<DMRDebitMemoPartialRow>>> expression =
                    (context, company_ex, actionType_ex, debitMemoLine_ex) =>
                    (from row in context.DMRHead
                     join DMRActnJoin in context.DMRActn on new { row.Company, row.DMRNum, } equals new { DMRActnJoin.Company, DMRActnJoin.DMRNum }
                     join RcvDtlJoin in context.RcvDtl on new { row.Company, row.PONum, DMRActnJoin.PackSlip, DMRActnJoin.PackLine } equals new { RcvDtlJoin.Company, RcvDtlJoin.PONum, RcvDtlJoin.PackSlip, RcvDtlJoin.PackLine }
                        into RcvDtlRow
                     from RcvDtlJoin in RcvDtlRow.DefaultIfEmpty()
                     join VendorJoin in context.Vendor on new { row.Company, row.VendorNum } equals new { VendorJoin.Company, VendorJoin.VendorNum }
                     where row.Company == company_ex
                     && DMRActnJoin.ActionType == actionType_ex
                     && DMRActnJoin.DebitMemoNum == ""
                     && DMRActnJoin.DebitMemoLine == debitMemoLine_ex
                     select new DMRDebitMemoPartialRow
                     {
                         VendorNum = row.VendorNum
                         ,
                         DMRNum = row.DMRNum
                         ,
                         PartNum = row.PartNum
                         ,
                         CurrencyCode = DMRActnJoin.CurrencyCode
                         ,
                         Quantity = DMRActnJoin.Quantity
                         ,
                         IUM = row.IUM
                         ,
                         OurUnitCost = DMRActnJoin.DocUnitCredit
                         ,
                         VendorQty = DMRActnJoin.Quantity
                         ,
                         CostPerCode = RcvDtlJoin.CostPerCode
                         ,
                         RevisionNum = row.RevisionNum
                         ,
                         PONum = row.PONum
                         ,
                         ReceiptDate = RcvDtlJoin.ReceiptDate
                         ,
                         VendorName = VendorJoin.Name
                         ,
                         ActionNum = DMRActnJoin.ActionNum
                         ,
                         SysRowID = DMRActnJoin.SysRowID
                     }
                    );

                selectDMRDMQuery = DBExpressionCompiler.Compile(expression);
            }

            return (from row in selectDMRDMQuery(this.Db, Company, actionType, debitMemoLine)
                    where ipSuppList.Count == 0 || ipSuppList.Contains(row.VendorNum.ToString(), new StringComparerInvariant())
                    select row);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<APInvDtl>> selectAPInvDtlNoLockQuery;
        private IEnumerable<APInvDtl> SelectAPInvDtl(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAPInvDtlNoLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvDtl>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    from row in context.APInvDtl
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex
                    select row;
                selectAPInvDtlNoLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlNoLockQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<APInvDtl>> selectAPInvDtlNoTaxExemptQuery;
        private IEnumerable<APInvDtl> SelectAPInvDtlWithoutTaxExempt(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAPInvDtlNoTaxExemptQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvDtl>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    from row in context.APInvDtl
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.TaxExempt == ""
                    select row;
                selectAPInvDtlNoTaxExemptQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlNoTaxExemptQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<APInvDtl>> selectAPInvDtlWithUpdLockQuery;
        private IEnumerable<APInvDtl> SelectAPInvDtlWithUpdLock(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAPInvDtlWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvDtl>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    from row in context.APInvDtl.With(LockHint.UpdLock)
                    where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex
                    select row;
                selectAPInvDtlWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, int, string, int, string, int> findFirstAPInvDtlFromDMRActn;
        private int FindFirstAPInvDtlFromDMRActn(string ipCompany, int ipDMRNum, int ipDMRActionNum, string ip_ActionType, int ipVendorNum, string ipInvoiceNum)
        {
            if (findFirstAPInvDtlFromDMRActn == null)
            {
                Expression<Func<ErpContext, string, int, int, string, int, string, int>> expression =
                    (context, ipCompany_ex, ipDMRNum_ex, ipDMRActionNum_ex, ip_ActionType_ex, ipVendorNum_ex, ipInvoiceNum_ex) =>
                        (from DMRActn_Row in context.DMRActn
                         join APInvDtl_Row in context.APInvDtl
                         on new { DMRActn_Row.Company, DMRActn_Row.PackSlip, DMRActn_Row.PackLine } equals new { APInvDtl_Row.Company, APInvDtl_Row.PackSlip, APInvDtl_Row.PackLine }
                         where DMRActn_Row.Company == ipCompany_ex &&
                               DMRActn_Row.DMRNum == ipDMRNum_ex &&
                               DMRActn_Row.ActionNum != ipDMRActionNum_ex &&
                               DMRActn_Row.ActionType == ip_ActionType_ex &&
                               APInvDtl_Row.VendorNum == ipVendorNum_ex &&
                               APInvDtl_Row.InvoiceNum == ipInvoiceNum_ex
                         select APInvDtl_Row.InvoiceLine)
                               .FirstOrDefault();
                findFirstAPInvDtlFromDMRActn = DBExpressionCompiler.Compile(expression);
            }
            return findFirstAPInvDtlFromDMRActn(this.Db, ipCompany, ipDMRNum, ipDMRActionNum, ip_ActionType, ipVendorNum, ipInvoiceNum);
        }


        private static Func<ErpContext, string, string, IEnumerable<APInvDtl>> selectAPInvDtlByInvoiceRefQuery;
        private IEnumerable<APInvDtl> SelectAPInvDtlByInvoiceRef(string Company, string InvoiceRef)
        {
            if (selectAPInvDtlByInvoiceRefQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<APInvDtl>>> expression =
                    (context, Company_ex, InvoiceRef_ex) =>
                    from row in context.APInvDtl
                    where row.Company == Company_ex &&
                    row.InvoiceRef == InvoiceRef_ex
                    select row;
                selectAPInvDtlByInvoiceRefQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlByInvoiceRefQuery(this.Db, Company, InvoiceRef);
        }

        private class APInvHedPartialRow2 : Epicor.Data.TempRowBase
        {
            public DateTime? InvoiceDate { get; set; }
        }

        private static Func<ErpContext, string, int, string, int, bool, APInvHedPartialRow2> findFirstAPInvHedQuery4;
        private APInvHedPartialRow2 FindFirstAPInvHed(string Company, int VendorNum, string InvoiceRef, int InstanceNum, bool IsRecurring)
        {
            if (findFirstAPInvHedQuery4 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, bool, APInvHedPartialRow2>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceRef_ex, InstanceNum_ex, IsRecurring_ex) =>
                    (from row in context.APInvHed
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceRef == InvoiceRef_ex &&
                     row.InstanceNum == InstanceNum_ex &&
                     row.IsRecurring == IsRecurring_ex
                     select new APInvHedPartialRow2 { InvoiceDate = row.InvoiceDate })
                    .FirstOrDefault();
                findFirstAPInvHedQuery4 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedQuery4(this.Db, Company, VendorNum, InvoiceRef, InstanceNum, IsRecurring);
        }

        private static Func<ErpContext, string, int, string, int, bool, bool> existsAPInvMscLCQuery;
        private bool ExistsAPInvMsc(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, bool LCFlag)
        {
            if (existsAPInvMscLCQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, bool, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, LCFlag_ex) =>
                    (from row in context.APInvMsc
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.LCFlag == LCFlag_ex
                     select row)
                    .Any();
                existsAPInvMscLCQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvMscLCQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, LCFlag);
        }

        private class APInvHedPartialRow : Epicor.Data.TempRowBase
        {
            public string InvoiceNum { get; set; }
            public int InstanceNum { get; set; }
        }

        private static Func<ErpContext, string, string, int, bool, int, APInvHedPartialRow> findFirstAPInvHedQuery3;
        private APInvHedPartialRow FindFirstAPInvHed(string Company, string InvoiceNum, int VendorNum, bool RecurSource, int InstanceNum)
        {
            if (findFirstAPInvHedQuery3 == null)
            {
                Expression<Func<ErpContext, string, string, int, bool, int, APInvHedPartialRow>> expression =
                    (context, Company_ex, InvoiceNum_ex, VendorNum_ex, RecurSource_ex, InstanceNum_ex) =>
                    (from row in context.APInvHed
                     where row.Company == Company_ex &&
                     row.InvoiceRef == InvoiceNum_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InstanceNum == InstanceNum_ex &&
                     row.RecurSource == RecurSource_ex
                     orderby row.InstanceNum descending
                     select new APInvHedPartialRow { InvoiceNum = row.InvoiceNum, InstanceNum = row.InstanceNum })
                    .FirstOrDefault();
                findFirstAPInvHedQuery3 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedQuery3(this.Db, Company, InvoiceNum, VendorNum, RecurSource, InstanceNum);
        }

        private class APInvHedPrecalcTaxPartialRow : Epicor.Data.TempRowBase
        {
            public int VendorNum { get; set; }
            public string Company { get; set; }
            public string InvoiceNum { get; set; }

        }

        private static Func<ErpContext, string, string, IEnumerable<APInvHedPrecalcTaxPartialRow>> selectAPInvHedPrecalcTaxQuery;
        private IEnumerable<APInvHedPrecalcTaxPartialRow> SelectAPInvHedPrecalcTax(string Company, string GroupID)
        {
            if (selectAPInvHedPrecalcTaxQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<APInvHedPrecalcTaxPartialRow>>> expression =
                    (context, Company_ex, GroupID_ex) =>
                    from row in context.APInvHed
                    where row.Company == Company_ex &&
                    row.GroupID == GroupID_ex &&
                    row.DevLog2 == true
                    select new APInvHedPrecalcTaxPartialRow { VendorNum = row.VendorNum, Company = row.Company, InvoiceNum = row.InvoiceNum };
                selectAPInvHedPrecalcTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedPrecalcTaxQuery(this.Db, Company, GroupID);
        }

        private static Func<ErpContext, string, string, int, bool, bool, APInvHed> findFirstAPInvHedQuery21;
        private APInvHed FindFirstAPInvHed(string Company, string InvoiceNum, int VendorNum, bool GRNIClearing, bool Posted)
        {
            if (findFirstAPInvHedQuery21 == null)
            {
                Expression<Func<ErpContext, string, string, int, bool, bool, APInvHed>> expression =
                    (context, Company_ex, InvoiceNum_ex, VendorNum_ex, GRNIClearing_ex, Posted_ex) =>
                    (from row in context.APInvHed
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.GRNIClearing == GRNIClearing_ex &&
                     row.Posted == Posted_ex
                     select row).FirstOrDefault();
                findFirstAPInvHedQuery21 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedQuery21(this.Db, Company, InvoiceNum, VendorNum, GRNIClearing, Posted);
        }

        private static Func<ErpContext, string, string, int, bool> isGRNIInvoiceQuery;
        private bool IsGRNIInvoice(string Company, string InvoiceNum, int VendorNum)
        {
            if (isGRNIInvoiceQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, bool>> expression =
                    (context, Company_ex, InvoiceNum_ex, VendorNum_ex) =>
                    (from row in context.APInvHed
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex
                     select row.GRNIClearing).FirstOrDefault();
                isGRNIInvoiceQuery = DBExpressionCompiler.Compile(expression);
            }

            return isGRNIInvoiceQuery(this.Db, Company, InvoiceNum, VendorNum);
        }

        #region APInvGrp

        private static Func<ErpContext, string, string, APInvGrp> findFirstAPInvGrpQuery;
        private APInvGrp FindFirstAPInvGrp(string Company, string GroupID)
        {
            if (findFirstAPInvGrpQuery == null)
            {
                Expression<Func<ErpContext, string, string, APInvGrp>> expression =
                    (context, Company_ex, GroupID_ex) =>
                    (from row in context.APInvGrp
                     where row.Company == Company_ex &&
                           row.GroupID == GroupID_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPInvGrpQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvGrpQuery(this.Db, Company, GroupID);
        }

        private static Func<ErpContext, string, string, APInvGrp> findFirstAPInvGrpWithUpdLockQuery;
        private APInvGrp FindFirstAPInvGrpWithUpdLock(string Company, string GroupID)
        {
            if (findFirstAPInvGrpWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, APInvGrp>> expression =
                    (context, Company_ex, GroupID_ex) =>
                    (from row in context.APInvGrp.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                           row.GroupID == GroupID_ex
                     select row).FirstOrDefault();
                findFirstAPInvGrpWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvGrpWithUpdLockQuery(this.Db, Company, GroupID);
        }

        static Func<ErpContext, string, string, string, bool> apInvGrpLockedToAnotherUserQuery;
        private bool APInvGrpLockedToAnotherUser(string company, string groupID, string userID)
        {
            if (apInvGrpLockedToAnotherUserQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
                    (ctx, company_ex, groupID_ex, userID_ex) =>
                    (from row in ctx.APInvGrp
                     where row.Company == company_ex &&
                     row.GroupID == groupID_ex &&
                     !string.IsNullOrEmpty(row.ActiveUserID) &&
                     row.ActiveUserID != userID_ex
                     select row).Any();
                apInvGrpLockedToAnotherUserQuery = DBExpressionCompiler.Compile(expression);
            }

            return apInvGrpLockedToAnotherUserQuery(this.Db, company, groupID, userID);
        }

        #endregion APInvGrp

        static Func<ErpContext, string, int, string, APInvHed> findFirstAPInvHedWithUpdLockQuery;
        private APInvHed FindFirstAPInvHedWithUpdLock(string company, int vendorNum, string invoiceNum)
        {
            if (findFirstAPInvHedWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, APInvHed>> expression =
      (ctx, company_ex, vendorNum_ex, invoiceNum_ex) =>
        (from row in ctx.APInvHed.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.VendorNum == vendorNum_ex &&
         row.InvoiceNum == invoiceNum_ex
         select row).FirstOrDefault();
                findFirstAPInvHedWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedWithUpdLockQuery(this.Db, company, vendorNum, invoiceNum);
        }

        private class latestPostedAPInvHedResult20
        {
            public string InvoiceNum { get; set; }
            public bool Posted { get; set; }
        }
        static Func<ErpContext, string, string, string, int, bool, latestPostedAPInvHedResult20> findFirstAPInvHedQuery20;
        private latestPostedAPInvHedResult20 FindFirstAPInvHed20(string company, string invoiceRef, string invoiceNum, int vendorNum, bool posted)
        {
            if (findFirstAPInvHedQuery20 == null)
            {
                Expression<Func<ErpContext, string, string, string, int, bool, latestPostedAPInvHedResult20>> expression =
      (ctx, company_ex, invoiceRef_ex, invoiceNum_ex, vendorNum_ex, posted_ex) =>
        (from row in ctx.APInvHed
         where row.Company == company_ex &&
         row.InvoiceRef == invoiceRef_ex &&
         row.InvoiceNum == invoiceNum_ex &&
         row.VendorNum == vendorNum_ex &&
         row.Posted == posted_ex
         select new latestPostedAPInvHedResult20()
         {
             InvoiceNum = row.InvoiceNum,
             Posted = row.Posted
         }
   ).FirstOrDefault();
                findFirstAPInvHedQuery20 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedQuery20(this.Db, company, invoiceRef, invoiceNum, vendorNum, posted);
        }


        private static Func<ErpContext, string, string, RecurringCycle> findFirstRecurringCycleQuery2;
        private RecurringCycle FindFirstRecurringCycle2(string Company, string CycleCode)
        {
            if (findFirstRecurringCycleQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, RecurringCycle>> expression =
                    (context, Company_ex, CycleCode_ex) =>
                    (from row in context.RecurringCycle
                     where row.Company == Company_ex &&
                     row.CycleCode == CycleCode_ex
                     select row)
                    .FirstOrDefault();
                findFirstRecurringCycleQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstRecurringCycleQuery2(this.Db, Company, CycleCode);
        }

        static Func<ErpContext, string, string, bool, bool> existsRecurringCycleQuery;
        private bool ExistsRecurringCycle(string company, string cycleCode, bool maximumValue)
        {
            if (existsRecurringCycleQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
      (ctx, company_ex, cycleCode_ex, maximumValue_ex) =>
        (from row in ctx.RecurringCycle
         where row.Company == company_ex &&
         row.CycleCode == cycleCode_ex &&
         row.MaximumValue == maximumValue_ex
         select row).Any();
                existsRecurringCycleQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsRecurringCycleQuery(this.Db, company, cycleCode, maximumValue);
        }


        private static Func<ErpContext, string, IEnumerable<APInvHed>> selectAPInvHedWithUpdLockQuery;
        private IEnumerable<APInvHed> SelectAPInvHedWithUpdLock(string Company)
        {
            if (selectAPInvHedWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, IEnumerable<APInvHed>>> expression =
                    (context, Company_ex) =>
                    (from row in context.APInvHed.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                     row.RecurSource == true
                     select row);
                selectAPInvHedWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedWithUpdLockQuery(this.Db, Company);
        }



        private static Func<ErpContext, string, int, string, bool, APInvHed> findFirstAPInvHedQuery2;
        private APInvHed FindFirstAPInvHed(string Company, int VendorNum, string InvoiceNum, bool RecurrSource)
        {
            if (findFirstAPInvHedQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, APInvHed>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, RecurrSource_ex) =>
                    (from row in context.APInvHed
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.RecurSource == RecurrSource_ex
                     select row).FirstOrDefault();
                findFirstAPInvHedQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvHedQuery2(this.Db, Company, VendorNum, InvoiceNum, RecurrSource);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<APInvHed>> selectAPInvHedQuery;
        private IEnumerable<APInvHed> SelectAPInvHed(string Company, int VendorNum, string InvoiceRef)
        {
            if (selectAPInvHedQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvHed>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceRef_ex) =>
                    from row in context.APInvHed
                    where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceRef == InvoiceRef_ex &&
                     row.InvoiceRef != row.InvoiceNum
                    select row;
                selectAPInvHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedQuery(this.Db, Company, VendorNum, InvoiceRef);
        }

        private static Func<ErpContext, string, string, IEnumerable<APInvHed>> selectAPInvHed2Query;
        private IEnumerable<APInvHed> SelectAPInvHed2(string Company, string InvoiceNum)
        {
            if (selectAPInvHed2Query == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<APInvHed>>> expression =
                    (context, Company_ex, InvoiceNum_ex) =>
                    from row in context.APInvHed
                    where row.Company == Company_ex &&
                     row.InvoiceNum == InvoiceNum_ex
                    select row;
                selectAPInvHed2Query = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHed2Query(this.Db, Company, InvoiceNum);
        }

        private class EmpExpensePartialRow : Epicor.Data.TempRowBase
        {
            public string Company { get; set; }
            public decimal DocExpenseAmt { get; set; }
            public int EmpExpenseNum { get; set; }
            public string EmpID { get; set; }
            public string ExpCurrencyCode { get; set; }
            public bool Invoiced { get; set; }
            public string MiscCode { get; set; }
            public int MtlSeq { get; set; }
            public string PhaseID { get; set; }
            public string ExpenseStatus { get; set; }
            public bool Reimbursable { get; set; }
        }

        private static Func<ErpContext, string, string, int, EmpExpensePartialRow> findFirstEmpExpenseQuery2;
        private EmpExpensePartialRow FindFirstEmpExpense2(string Company, string EmpID, int EmpExpenseNum)
        {
            if (findFirstEmpExpenseQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, int, EmpExpensePartialRow>> expression =
                    (context, Company_ex, EmpID_ex, EmpExpenseNum_ex) =>
                    (from row in context.EmpExpense
                     where row.Company == Company_ex &&
                     row.EmpID == EmpID_ex &&
                     row.EmpExpenseNum == EmpExpenseNum_ex
                     select new EmpExpensePartialRow { Company = row.Company, DocExpenseAmt = row.DocExpenseAmt, EmpExpenseNum = row.EmpExpenseNum, EmpID = row.EmpID, ExpCurrencyCode = row.ExpCurrencyCode, Invoiced = row.Invoiced, MiscCode = row.MiscCode, MtlSeq = row.MtlSeq, PhaseID = row.PhaseID, ExpenseStatus = row.ExpenseStatus, Reimbursable = row.Reimbursable })
                    .FirstOrDefault();
                findFirstEmpExpenseQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpExpenseQuery2(this.Db, Company, EmpID, EmpExpenseNum);
        }



        private static Func<ErpContext, string, string, bool> existsEmpBasicQuery;
        private bool ExistsEmpBasic(string Company, string EmpID)
        {
            if (existsEmpBasicQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (context, Company_ex, EmpID_ex) =>
                    (from row in context.EmpBasic
                     where row.Company == Company_ex &&
                     row.EmpID == EmpID_ex
                     select row)
                    .Any();
                existsEmpBasicQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsEmpBasicQuery(this.Db, Company, EmpID);
        }

        private static Func<ErpContext, string, string, int, EmpExpense> findFirstEmpExpenseQuery;
        private EmpExpense FindFirstEmpExpense(string Company, string EmpID, int EmpExpenseNum)
        {
            if (findFirstEmpExpenseQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, EmpExpense>> expression =
                    (context, Company_ex, EmpID_ex, EmpExpenseNum_ex) =>
                    (from row in context.EmpExpense
                     where row.Company == Company_ex &&
                     row.EmpID == EmpID_ex &&
                     row.EmpExpenseNum == EmpExpenseNum_ex
                     select row).With(LockHint.UpdLock)
                    .FirstOrDefault();
                findFirstEmpExpenseQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEmpExpenseQuery(this.Db, Company, EmpID, EmpExpenseNum);
        }

        private static Func<ErpContext, string, int, string, int, string, string, bool> existsAPLnTaxQuery4;
        private bool ExistsAPLnTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, string TaxCode, string RateCode)
        {
            if (existsAPLnTaxQuery4 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, TaxCode_ex, RateCode_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     (row.TaxCode != TaxCode_ex || row.RateCode != RateCode_ex)
                     select row)
                    .Any();
                existsAPLnTaxQuery4 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPLnTaxQuery4(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, TaxCode, RateCode);
        }

        private static Func<ErpContext, string, int, string, int, string, string, int, bool> existsAPLnTaxQuery7;
        private bool ExistsAPLnTax2(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, string TaxCode, string RateCode, int EcAcq)
        {
            if (existsAPLnTaxQuery7 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, int, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq == EcAcq_ex
                     select row)
                    .Any();
                existsAPLnTaxQuery7 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPLnTaxQuery7(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, TaxCode, RateCode, EcAcq);
        }


        private static Func<ErpContext, string, int, string, int, string, string, int, bool> existsAPLnTaxQuery5;
        private bool ExistsAPLnTax5(string Company, int Vendor, string InvoiceNum, int InvoiceLine, string TaxCode, string RateCode, int EcAcq)
        {
            if (existsAPLnTaxQuery5 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, int, bool>> expression =
                    (context, Company_ex, Vendor_ex, InvoiceNum_ex, InvoiceLine_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == Vendor_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq == EcAcq_ex &&
                     row.InvoiceLine != InvoiceLine_ex
                     select row)
                    .Any();
                existsAPLnTaxQuery5 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPLnTaxQuery5(this.Db, Company, Vendor, InvoiceNum, InvoiceLine, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, int, string, string, int, APLnTax> findFirstAPLnTaxWithUpdLockQuery;
        private APLnTax FindFirstAPLnTaxWithUpdLock(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, string TaxCode, string RateCode, int EcAcq)
        {
            if (findFirstAPLnTaxWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, int, APLnTax>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in context.APLnTax.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq != EcAcq_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPLnTaxWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPLnTaxWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, TaxCode, RateCode, EcAcq);
        }

        private static Func<ErpContext, string, int, string, int, APLnTax> findFirstAPLnQuery;
        private APLnTax FindFirstAPLn(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (findFirstAPLnQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, APLnTax>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPLnQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPLnQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }

        private static Func<ErpContext, string, int, string, bool> existsAPLnTaxQuery3;
        private bool ExistsAPLnTax(string Company, int VendorNum, string InvoiceNum)
        {
            if (existsAPLnTaxQuery3 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex
                     select row)
                    .Any();
                existsAPLnTaxQuery3 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPLnTaxQuery3(this.Db, Company, VendorNum, InvoiceNum);
        }

        private class APInvHedRecurring
        {
            public APInvHed APInvHed { get; set; }
            public string SupplierName { get; set; }
        }
        private static Func<ErpContext, string, string, IEnumerable<APInvHedRecurring>> SelectAPInvHedRecurringQuery;
        private IEnumerable<APInvHedRecurring> SelectAPInvHedRecurring(string ipCompany, string ipCycleCodesList)
        {
            if (SelectAPInvHedRecurringQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<APInvHedRecurring>>> expression =
                    (context, ipCompany_ex, ipCycleCodesList_ex) =>
                            from APInv in context.APInvHed
                            join Vend in context.Vendor on new { APInv.Company, APInv.VendorNum } equals new { Vend.Company, Vend.VendorNum } into lj
                            from Vendor in lj.DefaultIfEmpty()
                            where APInv.Company == ipCompany_ex
                               && APInv.Posted
                               && !APInv.CycleInactive
                               && APInv.RecurSource
                               && (string.IsNullOrEmpty(ipCycleCodesList_ex) || ErpEFFunctions.ListLookup(APInv.CycleCode, ipCycleCodesList_ex, Ice.Constants.LIST_DELIM) > -1)
                            select new APInvHedRecurring { APInvHed = APInv, SupplierName = Vendor.Name };

                SelectAPInvHedRecurringQuery = DBExpressionCompiler.Compile(expression);
            }

            return SelectAPInvHedRecurringQuery(this.Db, ipCompany, ipCycleCodesList);
        }

        private class RcvNotInvPartialRow : Epicor.Data.TempRowBase
        {
            public int DSNum { get; set; }
            public string DSPackSlip { get; set; }
            public Decimal FailedQty { get; set; }
            public string IUM { get; set; }
            public string PackSlip { get; set; }
            public string PartNum { get; set; }
            public Decimal PassedQty { get; set; }
            public int POLine { get; set; }
            public int PONum { get; set; }
            public Decimal VendorQty { get; set; }
            public Decimal VendorUnitCost { get; set; }
            public int VendorNum { get; set; }
            public DateTime? ReceiptDate { get; set; }
            public string CostPerCode { get; set; }
            public string PartDescription { get; set; }
            public Guid SysRowID { get; set; }
            public Decimal SupplierUnInvcReceiptQty { get; set; }
            public Decimal DocLineAmount { get; set; }
            public bool Invoiced { get; set; }
            public int PackLine { get; set; }
        }

        private static Func<ErpContext, string, IEnumerable<RcvNotInvPartialRow>> SelectRcvNotInvQuery;
        private IEnumerable<RcvNotInvPartialRow> SelectRcvNotInv(string Company)
        {
            if (SelectRcvNotInvQuery == null)
            {
                Expression<Func<ErpContext, string, IEnumerable<RcvNotInvPartialRow>>> expression =
                    (context, company_ex) =>
                    (from row in context.RcvDtl
                     join PartJoin in context.Part on new { row.Company, row.PartNum } equals new { PartJoin.Company, PartJoin.PartNum } into lj
                     from Part in lj.DefaultIfEmpty()
                     where row.Company == company_ex
                     && row.OurUnInvcReceiptQty != 0
                     && row.Received == true
                     select new RcvNotInvPartialRow
                     {
                         DSNum = row.PONum,
                         DSPackSlip = row.PackSlip,
                         FailedQty = row.FailedQty,
                         IUM = row.IUM,
                         PackSlip = row.PackSlip,
                         PartNum = row.PartNum,
                         PassedQty = row.PassedQty,
                         POLine = row.POLine,
                         PONum = row.PONum,
                         VendorQty = row.VendorQty,
                         VendorUnitCost = row.VendorUnitCost,
                         VendorNum = row.VendorNum,
                         ReceiptDate = row.ReceiptDate,
                         CostPerCode = row.CostPerCode,
                         PartDescription = Part.PartDescription,
                         SupplierUnInvcReceiptQty = row.SupplierUnInvcReceiptQty,
                         Invoiced = row.Invoiced,
                         PackLine = row.PackLine,
                         SysRowID = row.SysRowID
                     });
                SelectRcvNotInvQuery = DBExpressionCompiler.Compile(expression);
            }
            return SelectRcvNotInvQuery(this.Db, Company);
        }

        private static Func<ErpContext, string, int, int, string, int, string, bool> existsAPInvHedPostedQuery;
        private bool ExistsAPInvHedPosted(string Company, int VendorNum, int PONum, string PackSlip, int PackLine, string PartNum)
        {
            if (existsAPInvHedPostedQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, string, int, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, PONum_ex, PackSlip_ex, PackLine_ex, PartNum_ex) =>
                    (from row in context.APInvDtl
                     join row_APInvHed in context.APInvHed on new { row.Company, row.VendorNum, row.InvoiceNum }
                                                        equals new { row_APInvHed.Company, row_APInvHed.VendorNum, row_APInvHed.InvoiceNum }
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.PONum == PONum_ex &&
                           row.PackSlip == PackSlip_ex &&
                           row.PackLine == PackLine_ex &&
                           row.PartNum == PartNum_ex &&
                           row_APInvHed.Posted == true
                     select row).Any();
                existsAPInvHedPostedQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedPostedQuery(this.Db, Company, VendorNum, PONum, PackSlip, PackLine, PartNum);
        }

        private class RecurringCyclePartialRow : Epicor.Data.TempRowBase
        {
            public bool HoldInvoice { get; set; }
            public bool CopyLatestInvoice { get; set; }
            public bool Inactive { get; set; }
            public int Duration { get; set; }
            public bool MaximumValue { get; set; }
        }

        private static Func<ErpContext, string, string, RecurringCyclePartialRow> findFirstRecurringCycleQuery;
        private RecurringCyclePartialRow FindFirstRecurringCycle(string ipCompany, string ipCycleCode)
        {
            if (findFirstRecurringCycleQuery == null)
            {
                Expression<Func<ErpContext, string, string, RecurringCyclePartialRow>> expression =
                    (context, ipCompany_ex, ipCycleCode_ex) =>
                    (from row in context.RecurringCycle
                     where row.Company == ipCompany_ex &&
                     row.CycleCode == ipCycleCode_ex
                     select new RecurringCyclePartialRow { HoldInvoice = row.HoldInvoice, CopyLatestInvoice = row.CopyLatestInvoice, Inactive = row.Inactive, Duration = row.Duration, MaximumValue = row.MaximumValue })
                    .FirstOrDefault();
                findFirstRecurringCycleQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstRecurringCycleQuery(this.Db, ipCompany, ipCycleCode);
        }

        private static Func<ErpContext, string, int, string, int, int, IEnumerable<APInvExp>> selectAPInvExpQuery2;
        private IEnumerable<APInvExp> SelectAPInvExp(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int InvExpSeq)
        {
            if (selectAPInvExpQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, IEnumerable<APInvExp>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, InvExpSeq_ex) =>
                    (from row in context.APInvExp.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                          row.VendorNum == VendorNum_ex &&
                          row.InvoiceNum == InvoiceNum_ex &&
                          row.InvoiceLine == InvoiceLine_ex &&
                          row.InvExpSeq == InvExpSeq_ex
                     select row);
                selectAPInvExpQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvExpQuery2(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<APInvExp>> selectAPInvExpQuery;
        private IEnumerable<APInvExp> SelectAPInvExp(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAPInvExpQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvExp>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APInvExp
                     where row.Company == Company_ex &&
                          row.VendorNum == VendorNum_ex &&
                          row.InvoiceNum == InvoiceNum_ex
                     select row);
                selectAPInvExpQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvExpQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<APLnTax>> selectAPLnTaxQuery4;
        private IEnumerable<APLnTax> SelectAPLnTax(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAPLnTaxQuery4 == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APLnTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.ECAcquisitionSeq == 0
                     select row);
                selectAPLnTaxQuery4 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPLnTaxQuery4(this.Db, Company, VendorNum, InvoiceNum);
        }



        private class APLnTaxPartialRow2 : Epicor.Data.TempRowBase
        {
            public string Company { get; set; }
            public int InvoiceLine { get; set; }
            public string InvoiceNum { get; set; }
            public int VendorNum { get; set; }
            public decimal TaxAmt { get; set; }
            public decimal DocTaxAmt { get; set; }
            public decimal Rpt1TaxAmt { get; set; }
            public decimal Rpt2TaxAmt { get; set; }
            public decimal Rpt3TaxAmt { get; set; }
            public int CollectionType { get; set; }
            public int ECAcquisitionSeq { get; set; }
            public decimal TaxAmtVar { get; set; }
            public decimal DocTaxAmtVar { get; set; }
            public decimal Rpt1TaxAmtVar { get; set; }
            public decimal Rpt2TaxAmtVar { get; set; }
            public decimal Rpt3TaxAmtVar { get; set; }
            public decimal DedTaxAmt { get; set; }
            public decimal DocDedTaxAmt { get; set; }
            public decimal Rpt1DedTaxAmt { get; set; }
            public decimal Rpt2DedTaxAmt { get; set; }
            public decimal Rpt3DedTaxAmt { get; set; }
        }

        private static Func<ErpContext, string, int, string, int, IEnumerable<APLnTaxPartialRow2>> selectAPLnTaxQuery3;
        private IEnumerable<APLnTaxPartialRow2> SelectAPLnTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectAPLnTaxQuery3 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APLnTaxPartialRow2>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.InvoiceLine != 0
                     select new APLnTaxPartialRow2 { Company = row.Company, InvoiceLine = row.InvoiceLine, InvoiceNum = row.InvoiceNum, VendorNum = row.VendorNum, TaxAmt = row.TaxAmt, DocTaxAmt = row.DocTaxAmt, Rpt1TaxAmt = row.Rpt1TaxAmt, Rpt2TaxAmt = row.Rpt2TaxAmt, Rpt3TaxAmt = row.Rpt3TaxAmt, CollectionType = row.CollectionType, ECAcquisitionSeq = row.ECAcquisitionSeq, TaxAmtVar = row.TaxAmtVar, DocTaxAmtVar = row.DocTaxAmtVar, Rpt1TaxAmtVar = row.Rpt1TaxAmtVar, Rpt2TaxAmtVar = row.Rpt2TaxAmtVar, Rpt3TaxAmtVar = row.Rpt3TaxAmtVar, DedTaxAmt = row.DedTaxAmt, DocDedTaxAmt = row.DocDedTaxAmt, Rpt1DedTaxAmt = row.Rpt1DedTaxAmt, Rpt2DedTaxAmt = row.Rpt2DedTaxAmt, Rpt3DedTaxAmt = row.Rpt3DedTaxAmt });
                selectAPLnTaxQuery3 = DBExpressionCompiler.Compile(expression);

            }

            return selectAPLnTaxQuery3(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }

        private static Func<ErpContext, string, int, string, int, IEnumerable<APLnTax>> selectAPLnTaxQuery5;
        private IEnumerable<APLnTax> SelectAPLnTax5(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectAPLnTaxQuery5 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APLnTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.InvoiceLine != 0
                     select row);
                selectAPLnTaxQuery5 = DBExpressionCompiler.Compile(expression);

            }

            return selectAPLnTaxQuery5(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<APLnTax>> selectAllAPLnTaxQuery;
        private IEnumerable<APLnTax> SelectAllInvoiceLineTaxes(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAllAPLnTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APLnTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine != 0
                     select row);
                selectAllAPLnTaxQuery = DBExpressionCompiler.Compile(expression);

            }

            return selectAllAPLnTaxQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, string, int, IEnumerable<Guid>> selectAPLnTaxGuids;
        private IEnumerable<Guid> SelectAPLnTaxGuids(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectAPLnTaxGuids == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<Guid>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.InvoiceLine != 0
                     select row.SysRowID);
                selectAPLnTaxGuids = DBExpressionCompiler.Compile(expression);

            }

            return selectAPLnTaxGuids(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }
        private static Func<ErpContext, string, int, string, string, bool> existsAPInvDtlMiscQuery;
        private bool ExistsAPInvDtl(string Company, int VendorNum, string InvoiceNum, string LineType)
        {
            if (existsAPInvDtlMiscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, LineType_ex) =>
                    (from row in context.APInvDtl
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           row.LineType != LineType_ex
                     select row)
                    .Any();
                existsAPInvDtlMiscQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvDtlMiscQuery(this.Db, Company, VendorNum, InvoiceNum, LineType);
        }

        private static Func<ErpContext, string, int, string, bool> existsAPInvDtlQuery1;
        private bool ExistsAPInvDtl1(string Company, int VendorNum, string InvoiceNum)
        {
            if (existsAPInvDtlQuery1 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APInvDtl
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           (row.LineType != "M" &&
                           row.LineType != "A" &&
                           row.LineType != "U" &&
                           row.LineType != "R")

                     select row)
                    .Any();
                existsAPInvDtlQuery1 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvDtlQuery1(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, string, bool> existsAPInvDtlQuery2;
        private bool ExistsAPInvDtl2(string Company, int VendorNum, string InvoiceNum)
        {
            if (existsAPInvDtlQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APInvDtl
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           (row.LineType == "A" ||
                           row.LineType == "U" ||
                           row.LineType == "R")

                     select row)
                    .Any();
                existsAPInvDtlQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvDtlQuery2(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, string, bool> existsAPInvDtlAssetAddQuery;
        private bool ExistsAPInvDtlAssetAdd(string Company, int VendorNum, string InvoiceNum)
        {
            if (existsAPInvDtlAssetAddQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APInvDtl
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           row.LineType == "S" &&
                           row.DocAssetInvoiceBal != Decimal.Zero
                     select row)
                    .Any();
                existsAPInvDtlAssetAddQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvDtlAssetAddQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, string, int, string, bool> existsAPLnTaxQuery2;
        private bool ExistsAPLnTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, string TaxCode)
        {
            if (existsAPLnTaxQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, TaxCode_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.TaxCode == TaxCode_ex
                     select row)
                    .Any();
                existsAPLnTaxQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return existsAPLnTaxQuery2(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, TaxCode);
        }

        private class APInvDtlPartialRow : Epicor.Data.TempRowBase
        {
            public string Company { get; set; }
            public decimal DocInExtCost { get; set; }
            public decimal InExtCost { get; set; }
            public int InvoiceLine { get; set; }
            public string InvoiceNum { get; set; }
            public int VendorNum { get; set; }
        }

        private static Func<ErpContext, string, int, string, int, APInvDtlPartialRow> findFirstAPInvDtlQuery2;
        private APInvDtlPartialRow FindFirstAPInvDtl2(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (findFirstAPInvDtlQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, APInvDtlPartialRow>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    (from row in context.APInvDtl
                     where row.Company == Company_ex &&
                            row.VendorNum == VendorNum_ex &&
                            row.InvoiceNum == InvoiceNum_ex &&
                            row.InvoiceLine == InvoiceLine_ex
                     select new APInvDtlPartialRow { Company = row.Company, DocInExtCost = row.DocInExtCost, InExtCost = row.InExtCost, InvoiceLine = row.InvoiceLine, InvoiceNum = row.InvoiceNum, VendorNum = row.VendorNum })
                    .FirstOrDefault();
                findFirstAPInvDtlQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvDtlQuery2(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }



        private class APInvMscPartialRow : Epicor.Data.TempRowBase
        {
            public string Company { get; set; }
            public decimal DocInMiscAmt { get; set; }
            public decimal InMiscAmt { get; set; }
            public int InvoiceLine { get; set; }
            public string InvoiceNum { get; set; }
            public int VendorNum { get; set; }
            public decimal DocMiscAmt { get; set; }
        }

        private static Func<ErpContext, string, int, string, int, IEnumerable<APInvMscPartialRow>> selectPartialAPInvMscQuery;
        private IEnumerable<APInvMscPartialRow> SelectPartialAPInvMsc(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectPartialAPInvMscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvMscPartialRow>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    (from row in context.APInvMsc
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           row.InvoiceLine == InvoiceLine_ex
                     select new APInvMscPartialRow { Company = row.Company, DocInMiscAmt = row.DocInMiscAmt, InMiscAmt = row.InMiscAmt, InvoiceLine = row.InvoiceLine, InvoiceNum = row.InvoiceNum, VendorNum = row.VendorNum, DocMiscAmt = row.DocMiscAmt });
                selectPartialAPInvMscQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectPartialAPInvMscQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }

        private static Func<ErpContext, string, int, string, int, IEnumerable<APInvMsc>> selectAPInvMscQuery;
        private IEnumerable<APInvMsc> SelectAPInvMsc(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectAPInvMscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvMsc>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    (from row in context.APInvMsc
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           row.InvoiceLine == InvoiceLine_ex
                     select row);
                selectAPInvMscQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvMscQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }


        private static Func<ErpContext, string, int, string, IEnumerable<APInvMsc>> selectAPInvMscQuery1;
        private IEnumerable<APInvMsc> SelectAPInvMsc1(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAPInvMscQuery1 == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvMsc>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APInvMsc
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           row.InvoiceLine != 0
                     select row);
                selectAPInvMscQuery1 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvMscQuery1(this.Db, Company, VendorNum, InvoiceNum);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<APInvMsc>> selectAllAPInvMscQuery;
        private IEnumerable<APInvMsc> SelectAllAPInvMsc(string Company, int VendorNum, string InvoiceNum)
        {
            if (selectAllAPInvMscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvMsc>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.APInvMsc
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex
                     select row);
                selectAllAPInvMscQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAllAPInvMscQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        private class APLnTaxPartialRow : Epicor.Data.TempRowBase
        {
            public string Company { get; set; }
            public decimal DocTaxAmt { get; set; }
            public int InvoiceLine { get; set; }
            public string InvoiceNum { get; set; }
            public string RateCode { get; set; }
            public decimal TaxAmt { get; set; }
            public string TaxCode { get; set; }
            public int VendorNum { get; set; }
        }

        private static Func<ErpContext, string, int, string, int, string, string, IEnumerable<APLnTaxPartialRow>> selectAPLnTaxQuery2;
        private IEnumerable<APLnTaxPartialRow> SelectAPLnTax2(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, string TaxCode, string RateCode)
        {
            if (selectAPLnTaxQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, IEnumerable<APLnTaxPartialRow>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, TaxCode_ex, RateCode_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           row.InvoiceLine == InvoiceLine_ex &&
                           row.ECAcquisitionSeq == 0 &&
                           (row.TaxCode != TaxCode_ex || row.RateCode != RateCode_ex)
                     select new APLnTaxPartialRow { Company = row.Company, DocTaxAmt = row.DocTaxAmt, InvoiceLine = row.InvoiceLine, InvoiceNum = row.InvoiceNum, RateCode = row.RateCode, TaxAmt = row.TaxAmt, TaxCode = row.TaxCode, VendorNum = row.VendorNum });
                selectAPLnTaxQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPLnTaxQuery2(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, TaxCode, RateCode);
        }



        private static Func<ErpContext, string, string, string, SalesTRC> findFirstSalesTRCQuery;
        private SalesTRC FindFirstSalesTRC(string Company, string TaxCode, string RateCode)
        {
            if (findFirstSalesTRCQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, SalesTRC>> expression =
                    (context, Company_ex, TaxCode_ex, RateCode_ex) =>
                    (from row in context.SalesTRC
                     where row.Company == Company_ex &&
                    row.TaxCode == TaxCode_ex &&
                    row.RateCode == RateCode_ex
                     select row)
                    .FirstOrDefault();
                findFirstSalesTRCQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSalesTRCQuery(this.Db, Company, TaxCode, RateCode);
        }

        private static Func<ErpContext, string, string, string, bool> existsDefaultSalesTRCQuery;
        private bool ExistsDefaultSalesTRC(string company, string taxCode, string rateCode)
        {
            if (existsDefaultSalesTRCQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
                    (context, company_ex, taxCode_ex, rateCode_ex) =>
                    (from row in context.SalesTRC
                     where row.Company == company_ex &&
                     row.TaxCode == taxCode_ex &&
                     row.RateCode == rateCode_ex
                     select row.DefaultRate).FirstOrDefault();
                existsDefaultSalesTRCQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsDefaultSalesTRCQuery(this.Db, company, taxCode, rateCode);
        }

        private static Func<ErpContext, string, int, string, int, string, string, Guid, bool> existsAPLnTaxQuery;
        private bool ExistsAPLnTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, string TaxCode, string RateCode, Guid SysRowID)
        {
            if (existsAPLnTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, Guid, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, TaxCode_ex, RateCode_ex, SysRowID_ex) =>
                    (from row in context.APLnTax
                     where row.Company == Company_ex &&
                          row.VendorNum == VendorNum_ex &&
                          row.InvoiceNum == InvoiceNum_ex &&
                          row.InvoiceLine == InvoiceLine_ex &&
                          row.TaxCode == TaxCode_ex &&
                          row.RateCode == RateCode_ex &&
                          row.SysRowID != SysRowID_ex
                     select row)
                    .Any();
                existsAPLnTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPLnTaxQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, TaxCode, RateCode, SysRowID);
        }

        private static Func<ErpContext, string, int, string, int, APInvDtl> findFirstAPInvDtlWithUpdLockQuery;
        private APInvDtl FindFirstAPInvWithUpdLockDtl(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (findFirstAPInvDtlWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, APInvDtl>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    (from row in context.APInvDtl.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           row.InvoiceLine == InvoiceLine_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPInvDtlWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvDtlWithUpdLockQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }


        private static Func<ErpContext, string, int, string, int, string, string, IEnumerable<APLnTax>> selectAPLnTaxQuery;
        private IEnumerable<APLnTax> SelectAPLnTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, string TaxCode, string RateCode)
        {
            if (selectAPLnTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, string, IEnumerable<APLnTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, TaxCode_ex, RateCode_ex) =>
                    (from row in context.APLnTax.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           row.InvoiceLine == InvoiceLine_ex &&
                          (row.TaxCode != TaxCode_ex || row.RateCode != RateCode_ex)
                     select row);
                selectAPLnTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPLnTaxQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, TaxCode, RateCode);
        }

        private static Func<ErpContext, string, int, string, int, IEnumerable<APLnTax>> selectAPLnTaxAllQuery;
        private IEnumerable<APLnTax> SelectAPLnTaxAll(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectAPLnTaxAllQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APLnTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    (from row in context.APLnTax.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           row.InvoiceLine == InvoiceLine_ex
                     select row);
                selectAPLnTaxAllQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPLnTaxAllQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }


        #region TWGUIVendDfltDocType Queries

        private static Func<ErpContext, string, int, string, TWGUIVendDfltDocType> findFirstTWGUIVendDfltDocType;
        private TWGUIVendDfltDocType FindFirstTWGUIVendDfltDocType(string company, int vendorNum, string invoiceType)
        {
            if (findFirstTWGUIVendDfltDocType == null)
            {
                Expression<Func<ErpContext, string, int, string, TWGUIVendDfltDocType>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceType_ex) =>
                    (from row in ctx.TWGUIVendDfltDocType
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceType == invoiceType_ex &&
                     row.TranDocType != ""
                     select row).FirstOrDefault();
                findFirstTWGUIVendDfltDocType = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTWGUIVendDfltDocType(this.Db, company, vendorNum, invoiceType);
        }

        #endregion

        private static Func<ErpContext, string, string, bool> existsAPInvHedQuery;
        private bool ExistsAPInvHed(string company, string invoiceNum)
        {
            if (existsAPInvHedQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (ctx, company_ex, invoiceNum_ex) =>
                    (from row in ctx.APInvHed
                     where row.Company == company_ex &&
                     row.InvoiceNum == invoiceNum_ex
                     select row)
                    .Any();
                existsAPInvHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedQuery(this.Db, company, invoiceNum);
        }

        private static Func<ErpContext, string, string, Currency> findFirstCurrencyQuery;
        private Currency FindFirstCurrency(string company, string currencyID)
        {
            if (findFirstCurrencyQuery == null)
            {
                Expression<Func<ErpContext, string, string, Currency>> expression =
      (ctx, company_ex, currencyID_ex) =>
        (from row in ctx.Currency
         where row.Company == company_ex &&
         row.CurrencyID == currencyID_ex
         select row).FirstOrDefault();
                findFirstCurrencyQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCurrencyQuery(this.Db, company, currencyID);
        }

        private static Func<ErpContext, string, string, Currency> findFirstCurrencyWithCodeQuery;
        private Currency FindFirstCurrencyWithCode(string company, string currencyCode)
        {
            if (findFirstCurrencyWithCodeQuery == null)
            {
                Expression<Func<ErpContext, string, string, Currency>> expression =
      (ctx, company_ex, currencyCode_ex) =>
        (from row in ctx.Currency
         where row.Company == company_ex &&
         row.CurrencyCode == currencyCode_ex
         select row).FirstOrDefault();
                findFirstCurrencyWithCodeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCurrencyWithCodeQuery(this.Db, company, currencyCode);
        }


        static Expression<Func<ErpContext, string, int, int, string, int, POMisc>> POMiscExpression =
      (ctx, Company, REFPONum, _POLine, poMiscMiscCode, poMiscSeqNum) =>
        (from row in ctx.POMisc
         where row.Company == Company &&
         row.PONum == REFPONum &&
         row.POLine == _POLine &&
         row.MiscCode == poMiscMiscCode &&
         row.SeqNum == poMiscSeqNum
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, Guid, APInvMsc>> APInvMscExpression =
      (ctx, rAPInvMsc) =>
        (from row in ctx.APInvMsc
         where row.SysRowID == rAPInvMsc
         select row).FirstOrDefault();


        static Func<ErpContext, string, int, string, int, IEnumerable<APInvExp>> selectAPInvExpWithInvoiceLineQuery;
        private IEnumerable<APInvExp> SelectAPInvExp(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectAPInvExpWithInvoiceLineQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvExp>>> expression =
                (ctx, Company_ex, VendorNum_ex, InvoiceNum_ex, _InvoiceLine) =>
                (from row in ctx.APInvExp
                 where row.Company == Company_ex &&
                 row.VendorNum == VendorNum_ex &&
                 row.InvoiceNum == InvoiceNum_ex &&
                 row.InvoiceLine == _InvoiceLine
                 select row);
                selectAPInvExpWithInvoiceLineQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvExpWithInvoiceLineQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }


        static Expression<Func<ErpContext, string, int, int, int, POMisc>> POMiscExpression2 =
      (ctx, Company, PONum, POMiscPOLine, POMiscSeqNum) =>
        (from row in ctx.POMisc
         where row.Company == Company &&
         row.PONum == PONum &&
         row.POLine == POMiscPOLine &&
         row.SeqNum == POMiscSeqNum
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, int, int, APInvMsc>> BufAPInvMscExpression =
      (ctx, CompanyID, APInvVendorNum, InvoiceNum, InvoiceLine, MscNum) =>
        (from row in ctx.APInvMsc
         where row.Company == CompanyID &&
         row.VendorNum == APInvVendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.MscNum == MscNum
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, int, string, int, int, IEnumerable<RcvMisc>>> BufRcvMiscExpression =
      (ctx, CompanyID, VendorNum, InvoiceNum, InvoiceLine, MscNum) =>
        (from row in ctx.RcvMisc
         where row.Company == CompanyID &&
         row.APInvVendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.MscNum == MscNum
         select row);


        static Expression<Func<ErpContext, string, int, string, int, int, IEnumerable<RcvMisc>>> BufRcvMiscExpression2 =
      (ctx, CompanyID, VendorNum, InvoiceNum, InvoiceLine, MscNum) =>
        (from row in ctx.RcvMisc.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.APInvVendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.MscNum == MscNum
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, string, RcvMisc>> BufRcvMiscExpression3 =
      (ctx, CompanyID, VendorNum, PurPoint, PackSlip) =>
        (from row in ctx.RcvMisc
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == PackSlip
         select row).LastOrDefault();


        static Func<ErpContext, string, int, string, int, APInvExp> findLastAPInvExpQuery;
        private APInvExp FindLastAPInvExp(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (findLastAPInvExpQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, APInvExp>> expression =
                (ctx, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                (from row in ctx.APInvExp
                 where row.Company == Company_ex &&
                 row.VendorNum == VendorNum_ex &&
                 row.InvoiceNum == InvoiceNum_ex &&
                 row.InvoiceLine == InvoiceLine_ex
                 orderby row.Company, row.VendorNum, row.InvoiceNum, row.InvoiceLine, row.InvExpSeq descending
                 select row).FirstOrDefault();
                findLastAPInvExpQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastAPInvExpQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }

        static Func<ErpContext, string, int, string, int, int, APInvExp> findLastAPInvExpQuery2;
        private APInvExp FindLastAPInvExp(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int InvExpSeq)
        {
            if (findLastAPInvExpQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, APInvExp>> expression =
                (ctx, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, InvExpSeq_ex) =>
                (from row in ctx.APInvExp
                 where row.Company == Company_ex &&
                 row.VendorNum == VendorNum_ex &&
                 row.InvoiceNum == InvoiceNum_ex &&
                 row.InvoiceLine == InvoiceLine_ex &&
                 row.InvExpSeq == InvExpSeq_ex
                 orderby row.Company, row.VendorNum, row.InvoiceNum, row.InvoiceLine, row.InvExpSeq descending
                 select row).FirstOrDefault();
                findLastAPInvExpQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findLastAPInvExpQuery2(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq);
        }


        static Expression<Func<ErpContext, string, int, string, int, int, APInvExp>> APInvExpExpression3 =
      (ctx, Company, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq) =>
        (from row in ctx.APInvExp
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.InvExpSeq == InvExpSeq
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, bool, GLBook>> GLBookExpression =
      (ctx, CompanyID, _MainBook) =>
        (from row in ctx.GLBook
         where row.Company == CompanyID &&
         row.MainBook == _MainBook
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, string, string, IEnumerable<SysGLCTAcctCntxt>>> SysGLCTAcctCntxtExpression =
      (ctx, CompanyID, _SysGLControlType, _SysGLAcctContext, _RecordType) =>
        (from row in ctx.SysGLCTAcctCntxt
         where row.Company == CompanyID &&
         row.SysGLControlType == _SysGLControlType &&
         row.SysGLAcctContext == _SysGLAcctContext &&
         row.RecordType == _RecordType
         select row);



        static Expression<Func<ErpContext, string, int, string, int, int, IEnumerable<RcvMisc>>> RcvMiscExpression =
      (ctx, CompanyID, VendorNum, InvoiceNum, InvoiceLine, MscNum) =>
        (from row in ctx.RcvMisc
         where row.Company == CompanyID &&
         row.APInvVendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.MscNum == MscNum
         select row);



        static Expression<Func<ErpContext, string, int, string, int, int, APInvMsc>> APInvMscExpression2 =
      (ctx, CompanyID, saveVendorNum, saveInvoiceNum, saveInvoiceLine, saveMscNum) =>
        (from row in ctx.APInvMsc
         where row.Company == CompanyID &&
         row.VendorNum == saveVendorNum &&
         row.InvoiceNum == saveInvoiceNum &&
         row.InvoiceLine == saveInvoiceLine &&
         row.MscNum == saveMscNum
         select row).FirstOrDefault();

        static Expression<Func<ErpContext, string, int, string, IEnumerable<APInvTax>>> APInvTaxExpression =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.ECAcquisitionSeq != 9
         select row);

        class APInvMiscTot
        {
            public decimal MiscAmt { get; set; }
            public decimal DocMiscAmt { get; set; }
        }

        static Func<ErpContext, string, int, string, APInvMiscTot> GetAPInvMiscTotQuery;
        private APInvMiscTot GetAPInvMiscTot(string Company, int VendorNum, string InvoiceNum)
        {
            if (GetAPInvMiscTotQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, APInvMiscTot>> expression =
                (ctx, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                                    (from InvMscRow in ctx.APInvMsc
                                     join PurMiscRow in ctx.PurMisc on
                                            new { InvMscRow.Company, InvMscRow.MiscCode } equals
                                            new { PurMiscRow.Company, PurMiscRow.MiscCode } into PurMiscRows
                                     from PurMiscRow in PurMiscRows.DefaultIfEmpty()
                                     where InvMscRow.Company == Company_ex &&
                                           InvMscRow.VendorNum == VendorNum_ex &&
                                           InvMscRow.InvoiceNum == InvoiceNum_ex &&
                                           InvMscRow.MiscAmt != 0 &&
                                           (PurMiscRow != null && !PurMiscRow.TakeDiscount)
                                     group InvMscRow by new { InvMscRow.Company, InvMscRow.VendorNum, InvMscRow.InvoiceNum } into invoices
                                     select new APInvMiscTot()
                                     {
                                         MiscAmt = invoices.Sum(inv => inv.MiscAmt),
                                         DocMiscAmt = invoices.Sum(inv => inv.DocMiscAmt)
                                     }).FirstOrDefault();
                GetAPInvMiscTotQuery = DBExpressionCompiler.Compile(expression);
            }

            return GetAPInvMiscTotQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        static Expression<Func<ErpContext, Guid, APInvMsc>> APInvMscExpression4 =
        (ctx, rCurrentAPInvMscRowid) =>
        (from row in ctx.APInvMsc
         where row.SysRowID == rCurrentAPInvMscRowid
         select row).FirstOrDefault();
        static Func<ErpContext, string, int, string, int, APInvMsc> findLastAPInvMscQuery;
        private APInvMsc FindLastAPInvMsc(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (findLastAPInvMscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, APInvMsc>> expression =
                (ctx, Company_ex, VendorNum_ex, InvoiceNum_ex, _InvoiceLine) =>
                (from row in ctx.APInvMsc
                 where row.Company == Company_ex &&
                 row.VendorNum == VendorNum_ex &&
                 row.InvoiceNum == InvoiceNum_ex &&
                 row.InvoiceLine == _InvoiceLine
                 orderby row.Company, row.VendorNum, row.InvoiceNum, row.InvoiceLine, row.MscNum descending
                 select row).FirstOrDefault();
                findLastAPInvMscQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastAPInvMscQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }


        static Expression<Func<ErpContext, string, string, bool, bool>> PurMiscExpression =
      (ctx, CompanyID, MiscCode, _LCFlag) =>
        (from row in ctx.PurMisc
         where row.Company == CompanyID &&
         row.MiscCode == MiscCode &&
         row.LCFlag == _LCFlag
         select row).Any();



        static Expression<Func<ErpContext, string, int, int, string, DMRActn>> DMRActnExpression =
      (ctx, CompanyID, DMRNum, DMRActionNum, _ActionType) =>
        (from row in ctx.DMRActn.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.DMRNum == DMRNum &&
         row.ActionNum == DMRActionNum &&
         row.ActionType == _ActionType
         select row).FirstOrDefault();
        //HOLDLOCK


        static Expression<Func<ErpContext, string, TaxCat>> TaxCatExpression2 =
      (ctx, CompanyID) =>
        (from row in ctx.TaxCat
         where row.Company == CompanyID &&
         row.SysDefault == true
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, int, string, int, bool>> APInvExpExpression5 =
      (ctx, Company, VendorNum, InvoiceNum, InvoiceLine) =>
        (from row in ctx.APInvExp
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine
         select row).Any();



        static Expression<Func<ErpContext, string, int, string, int, bool>> APInvExpExpression6 =
      (ctx, Company, VendorNum, InvoiceNum, InvoiceLine) =>
        (from row in ctx.APInvExp
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine
         select row).Any();



        static Expression<Func<ErpContext, string, int, string, int, bool>> APInvJobExpression =
      (ctx, Company, VendorNum, InvoiceNum, InvoiceLine) =>
        (from row in ctx.APInvJob
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine
         select row).Any();



        static Expression<Func<ErpContext, string, bool, bool>> XbSystExpression2 =
      (ctx, CompanyID, _APPurchType) =>
        (from row in ctx.XbSyst
         where row.Company == CompanyID &&
         row.APPurchType == _APPurchType
         select row).Any();


        static Func<ErpContext, string, string, GLPurch> GLPurchExpression;
        private GLPurch FindFirstGLPurch(string Company, string PCode)
        {
            if (GLPurchExpression == null)
            {
                Expression<Func<ErpContext, string, string, GLPurch>> expression =
                (ctx, Company_ex, PurchCode) =>
                (from row in ctx.GLPurch
                 where row.Company == Company_ex &&
                 row.PurchCode == PurchCode
                 select row).FirstOrDefault();
                GLPurchExpression = DBExpressionCompiler.Compile(expression);
            }

            return GLPurchExpression(this.Db, Company, PCode);
        }



        static Expression<Func<ErpContext, string, bool, bool>> XbSystExpression3 =
      (ctx, CompanyID, _APDiscount) =>
        (from row in ctx.XbSyst
         where row.Company == CompanyID &&
         row.APDiscount == _APDiscount
         select row).Any();



        static Expression<Func<ErpContext, string, int, int, int, bool>> PORelExpression =
      (ctx, Company, PONum, POLine, PORelNum) =>
        (from row in ctx.PORel
         where row.Company == Company &&
         row.PONum == PONum &&
         row.POLine == POLine &&
         row.PORelNum == PORelNum &&
         row.DropShip == true
         select row).Any();



        static Expression<Func<ErpContext, string, string, int, PartXRefVend>> PartXRefVendExpression =
      (ctx, CompanyID, PartNum, VendorNum) =>
        (from row in ctx.PartXRefVend
         where row.Company == CompanyID &&
         row.PartNum == PartNum &&
         row.VendorNum == VendorNum
         select row).FirstOrDefault();


        static Func<ErpContext, string, int, int, int, PORel> PORelExpression2;
        private PORel FindFirstPORel(string Company, int PONum, int POLine, int PORelNum)
        {
            if (PORelExpression2 == null)
            {
                Expression<Func<ErpContext, string, int, int, int, PORel>> expression =
                (ctx, CompanyID, PONum_ex, POLine_ex, PORelNum_ex) =>
                (from row in ctx.PORel
                 where row.Company == CompanyID &&
                 row.PONum == PONum_ex &&
                 row.POLine == POLine_ex &&
                 row.PORelNum == PORelNum_ex
                 select row).FirstOrDefault();
                PORelExpression2 = DBExpressionCompiler.Compile(expression);
            }

            return PORelExpression2(this.Db, Company, PONum, POLine, PORelNum);
        }

        static Func<ErpContext, string, int, string, bool, int, IEnumerable<APInvDtl>> selectAPInvDtlQuery_3;
        private IEnumerable<APInvDtl> SelectAPInvDtl(string company, int vendorNum, string invoiceNum, bool correctionDtl, int exceptInvoiceLine)
        {
            if (selectAPInvDtlQuery_3 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, int, IEnumerable<APInvDtl>>> expression =
                  (ctx, company_ex, vendornum_ex, invoicenum_ex, correctionDtl_ex, exceptInvoiceLine_ex) =>
                    (from row in ctx.APInvDtl
                     where row.Company == company_ex &&
                     row.VendorNum == vendornum_ex &&
                     row.InvoiceNum == invoicenum_ex &&
                     row.InvoiceLine != exceptInvoiceLine_ex &&
                     row.CorrectionDtl == correctionDtl_ex
                     select row);
                selectAPInvDtlQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlQuery_3(this.Db, company, vendorNum, invoiceNum, correctionDtl, exceptInvoiceLine);
        }

        static Func<ErpContext, string, int, string, int, bool, APInvDtl> selectAPInvDtlQuery_4;
        private APInvDtl FindFirstAPInvDtl(string company, int vendorNum, string invoiceNum, int invoiceLineRef, bool correctionDtl)
        {
            if (selectAPInvDtlQuery_4 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, bool, APInvDtl>> expression =
                  (ctx, company_ex, vendornum_ex, invoicenum_ex, invoiceLineRef_ex, correctionDtl_ex) =>
                    (from row in ctx.APInvDtl
                     where row.Company == company_ex &&
                     row.VendorNum == vendornum_ex &&
                     row.InvoiceNum == invoicenum_ex &&
                     row.InvoiceLineRef == invoiceLineRef_ex &&
                     row.CorrectionDtl == correctionDtl_ex
                     select row).FirstOrDefault();
                selectAPInvDtlQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlQuery_4(this.Db, company, vendorNum, invoiceNum, invoiceLineRef, correctionDtl);
        }



        static Expression<Func<ErpContext, string, string, IEnumerable<TaxRgnSalesTax>>> TaxRgnSalesTaxExpression =
      (ctx, Company, TaxRegionCode) =>
        (from row in ctx.TaxRgnSalesTax
         where row.Company == Company &&
         row.TaxRegionCode == TaxRegionCode
         select row);



        static Expression<Func<ErpContext, string, string, int, SalesTax>> SalesTaxExpression =
      (ctx, CompanyID, TaxCode, _CollectionType) =>
        (from row in ctx.SalesTax
         where row.Company == CompanyID &&
         row.TaxCode == TaxCode &&
         row.CollectionType == _CollectionType
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, IEnumerable<TaxRgnSalesTax>>> TaxRgnSalesExpression =
      (ctx, CompanyID, TaxRegionCode) =>
        (from row in ctx.TaxRgnSalesTax
         where row.Company == CompanyID &&
         row.TaxRegionCode == TaxRegionCode
         select row);



        static Expression<Func<ErpContext, string, string, int, int, int, SalesTax>> SalesTaxExpression2 =
      (ctx, CompanyID, TaxCode, _CollectionType, _Timing, _Timing2) =>
        (from row in ctx.SalesTax
         where row.Company == CompanyID &&
         row.TaxCode == TaxCode &&
         row.CollectionType == _CollectionType &&
        (row.Timing == _Timing ||
         row.Timing == _Timing2)
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, bool, SalesTRC>> SalesTRCExpression =
      (ctx, CompanyID, TaxCode, _DefaultRate) =>
        (from row in ctx.SalesTRC
         where row.Company == CompanyID &&
         row.TaxCode == TaxCode &&
         row.DefaultRate == _DefaultRate
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, string, TaxRate>> TaxRateExpression =
      (ctx, CompanyID, TaxCode, RateCode) =>
        (from row in ctx.TaxRate
         where row.Company == CompanyID &&
         row.TaxCode == TaxCode &&
         row.RateCode == RateCode
         select row).LastOrDefault();


        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvTax>>> APInvTaxExpression2 =
      (ctx, Company, VendorNum, InvoiceNum, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.ECAcquisitionSeq == _ECAcquisitionSeq
         select row);



        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvDtl>>> BAPInvDtlExpression =
      (ctx, CompanyID, SaveVendorNum, SaveInvoiceNum, SaveInvoiceLine) =>
        (from row in ctx.APInvDtl
         where row.Company == CompanyID &&
         row.VendorNum == SaveVendorNum &&
         row.InvoiceNum == SaveInvoiceNum &&
         row.InvoiceLine != SaveInvoiceLine
         select row);


        static Expression<Func<ErpContext, string, int, string, APInvDtl>> BAPInvDtlExpression2 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         orderby row.InvoiceLine descending
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, int, int, bool>> APInvMscExpression7 =
      (ctx, Company, VendorNum, InvoiceNum, InvoiceLine, _ContainerID) =>
        (from row in ctx.APInvMsc
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
        (row.ContainerID != _ContainerID ||
         row.PackSlip != string.Empty)
         select row).Any();



        static Expression<Func<ErpContext, string, string, int, IEnumerable<RebateTrans>>> RebateTransExpression =
      (ctx, CompanyID, InvoiceNum, InvoiceLine) =>
        (from row in ctx.RebateTrans.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.APInvoiceNum == InvoiceNum &&
         row.APInvoiceLine == InvoiceLine
         select row);
        //HOLDLOCK


        static Expression<Func<ErpContext, string, int, int, bool, POHeader>> POHeaderExpression =
      (ctx, Company, VendorNum, PONum, _OpenOrder) =>
        (from row in ctx.POHeader
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PONum == PONum &&
         row.OpenOrder == _OpenOrder
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, int, bool, bool>> PODetailExpression =
      (ctx, Company, PONum, POLine, _OpenLine) =>
        (from row in ctx.PODetail
         where row.Company == Company &&
         row.PONUM == PONum &&
         row.POLine == POLine &&
         row.OpenLine == _OpenLine
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, int, int, int, bool, bool>> PORelExpression3 =
      (ctx, Company, PONum, POLine, PORelNum, _OpenRelease) =>
        (from row in ctx.PORel
         where row.Company == Company &&
         row.PONum == PONum &&
         row.POLine == POLine &&
         row.PORelNum == PORelNum &&
         row.OpenRelease == _OpenRelease
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, int, bool, bool>> POHeaderExpression2 =
      (ctx, Company, PONum, _ConsolidatedPO) =>
        (from row in ctx.POHeader
         where row.Company == Company &&
         row.PONum == PONum &&
         row.ConsolidatedPO == _ConsolidatedPO
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, int, string, bool, bool>> APInvHedExpression19 =
      (ctx, Company, VendorNum, InvoiceNum, _CPay) =>
        (from row in ctx.APInvHed
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.CPay == _CPay
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, bool>> TaxCatExpression3 =
      (ctx, Company, TaxCatID) =>
        (from row in ctx.TaxCat
         where row.Company == Company &&
         row.TaxCatID == TaxCatID
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, int, IEnumerable<APInvPB>>> APInvPBExpression =
      (ctx, CompanyID, InvoiceNum, InvoiceLine) =>
        (from row in ctx.APInvPB.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, Guid, APInvDtl>> APInvDtlExpression4 =
      (ctx, rSaveAPInvDtlRowid) =>
        (from row in ctx.APInvDtl
         where row.SysRowID == rSaveAPInvDtlRowid
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, string, string, string, string, IEnumerable<TranGLC>>> BAPInvExpTGLCExpression =
      (ctx, Company, _RelatedToFile, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq) =>
        (from row in ctx.TranGLC
         where row.Company == Company &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum &&
         row.Key3 == InvoiceLine &&
         row.Key4 == InvExpSeq
         select row);



        static Expression<Func<ErpContext, string, int, string, int, int, bool>> APInvExpExpression10 =
      (ctx, Company, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq) =>
        (from row in ctx.APInvExp
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.InvExpSeq == InvExpSeq
         select row).Any();



        static Expression<Func<ErpContext, string, string, string, string, string, string, bool>> TranGLCExpression =
      (ctx, Company, _RelatedToFile, Key1, Key2, Key3, Key4) =>
        (from row in ctx.TranGLC
         where row.Company == Company &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == Key1 &&
         row.Key2 == Key2 &&
         row.Key3 == Key3 &&
         row.Key4 == Key4
         select row).Any();


        static Expression<Func<ErpContext, string, string, string, string, string, string, bool, TranGLC>> TranGLCExpression2 =
      (ctx, Company, _RelatedToFile, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq, _IsExternalCompany) =>
        (from row in ctx.TranGLC
         where row.Company == Company &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum &&
         row.Key3 == InvoiceLine &&
         row.Key4 == InvExpSeq &&
         row.IsExternalCompany == _IsExternalCompany
         orderby row.BookID
         select row).FirstOrDefault();

        static Expression<Func<ErpContext, string, string, string, string, string, string, IEnumerable<TranGLC>>> APInvExpTranGLCExpression =
      (ctx, Company, _RelatedToFile, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq) =>
        (from row in ctx.TranGLC
         join GLBook_row in ctx.GLBook on
                               new { row.Company, row.BookID } equals
                               new { GLBook_row.Company, GLBook_row.BookID }
         where row.Company == Company &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum &&
         row.Key3 == InvoiceLine &&
         row.Key4 == InvExpSeq
         orderby GLBook_row.MainBook descending
         select row);

        static Expression<Func<ErpContext, string, string, string, GLAccount>> GLAccountExpression =
      (ctx, Company, COACode, GLAccount) =>
        (from row in ctx.GLAccount
         where row.Company == Company &&
         row.COACode == COACode &&
         row.GLAccount1 == GLAccount
         select row).FirstOrDefault();

        static Expression<Func<ErpContext, string, string, string, GLAcctDisp>> GLAcctDispExpression =
      (ctx, Company, COACode, GLAccount) =>
        (from row in ctx.GLAcctDisp
         where row.Company == Company &&
         row.COACode == COACode &&
         row.GLAccount == GLAccount
         select row).FirstOrDefault();

        static Func<ErpContext, string, string, string, GLAcctDisp> findFirstGLAcctDescQuery0;
        private GLAcctDisp FindFirstGLAcctDispByGLAccount(string company, string coaCode, string glAccount)
        {
            if (findFirstGLAcctDescQuery0 == null)
            {
                Expression<Func<ErpContext, string, string, string, GLAcctDisp>> expression =
                  (ctx, company_ex, coaCode_ex, glAccount_ex) =>
                      (from row in ctx.GLAcctDisp
                       where row.Company == company_ex &&
                            row.COACode == coaCode_ex &&
                            row.GLAccount == glAccount_ex
                       select row).FirstOrDefault();
                findFirstGLAcctDescQuery0 = DBExpressionCompiler.Compile(expression);
            }
            return findFirstGLAcctDescQuery0(this.Db, company, coaCode, glAccount);
        }

        static Func<ErpContext, string, string, string, GLAcctDisp> findFirstGLAcctDescQuery;
        private GLAcctDisp FindFirstGLAcctDesc(string company, string coaCode, string glAcctDisp)
        {
            if (findFirstGLAcctDescQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, GLAcctDisp>> expression =
                  (ctx, company_ex, coaCode_ex, glAcctDisp_ex) =>
                      (from row in ctx.GLAcctDisp
                       where row.Company == company_ex &&
                            row.COACode == coaCode_ex &&
                            row.GLAcctDisp1 == glAcctDisp_ex
                       select row).FirstOrDefault();
                findFirstGLAcctDescQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstGLAcctDescQuery(this.Db, company, coaCode, glAcctDisp);
        }

        private static Func<ErpContext, string, string, string, string, GLBGLAcctDisp> findFirstGLBGLAcctDispQuery;
        private GLBGLAcctDisp FindFirstGLBGLAcctDisp(string company, string extCompanyID, string coaCode, string glAccount)
        {
            if (findFirstGLBGLAcctDispQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, GLBGLAcctDisp>> expression =
                (ctx, company_ex, extCompanyID_ex, coaCode_ex, glAccount_ex) =>
                (from row in ctx.GLBGLAcctDisp
                 where row.Company == company_ex &&
                 row.ExtCompanyID == extCompanyID_ex &&
                 row.COACode == coaCode_ex &&
                 row.GLAccount == glAccount_ex
                 select row).FirstOrDefault();
                findFirstGLBGLAcctDispQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstGLBGLAcctDispQuery(this.Db, company, extCompanyID, coaCode, glAccount);
        }

        static Func<ErpContext, string, string, string, string, string> findFirstGLBGLAccountQuery;
        private string FindFirstGLBGLAccount(string company, string extCompanyID, string coaCode, string glAccount)
        {
            if (findFirstGLBGLAccountQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string>> expression =
                (ctx, company_ex, extCompanyID_ex, coaCode_ex, glAccount_ex) =>
                (from row in ctx.GLBGLAcctDisp
                 where row.Company == company_ex &&
                 row.ExtCompanyID == extCompanyID_ex &&
                 row.COACode == coaCode_ex &&
                 row.GLAccount == glAccount_ex
                 select row.GLAccount).FirstOrDefault();
                findFirstGLBGLAccountQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBGLAccountQuery(this.Db, company, extCompanyID, coaCode, glAccount);
        }

        static Func<ErpContext, string, bool, string> findFirstCOACodeQuery;
        private string FindFirstCOACode(string company, bool mainBook)
        {
            if (findFirstCOACodeQuery == null)
            {
                Expression<Func<ErpContext, string, bool, string>> expression =
            (ctx, company_ex, mainBook_ex) =>
                       (from row in ctx.GLBook
                        where row.Company == company_ex &&
                        row.MainBook == mainBook_ex
                        select row.COACode).FirstOrDefault();
                findFirstCOACodeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstCOACodeQuery(this.Db, company, mainBook);
        }


        static Expression<Func<ErpContext, string, string, string, string, string, string, bool, TranGLC>> TranGLCExpression3 =
      (ctx, Company, _RelatedToFile, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq, _IsExternalCompany) =>
        (from row in ctx.TranGLC
         where row.Company == Company &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum &&
         row.Key3 == InvoiceLine &&
         row.Key4 == InvExpSeq &&
         row.IsExternalCompany == _IsExternalCompany
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvDtl>>> APInvDtlExpression6 =
      (ctx, Company, VendorNum, InvoiceNum, InvoiceLine) =>
        (from row in ctx.APInvDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine
         select row);


        static Func<ErpContext, string, string, string, ExtCompany> findFirstExtCompanyQuery;
        private ExtCompany FindFirstExtCompany(string Company, string ExtSystem, string ExtCompany)
        {
            if (findFirstExtCompanyQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, ExtCompany>> expression =
                (ctx, Company_ex, ExtSystemID_ex, ExtCompanyID_ex) =>
                (from row in ctx.ExtCompany
                 where row.Company == Company_ex &&
                 row.ExtSystemID == ExtSystemID_ex &&
                 row.ExtCompanyID == ExtCompanyID_ex
                 select row).FirstOrDefault();
                findFirstExtCompanyQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstExtCompanyQuery(this.Db, Company, ExtSystem, ExtCompany);
        }


        static Expression<Func<ErpContext, string, int, string, int, int, APInvExp>> APInvExpExpression11 =
      (ctx, Company, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq) =>
        (from row in ctx.APInvExp
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.InvExpSeq == InvExpSeq
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, string, GLAccount>> GLAccountExpression2 =
      (ctx, Company, COACode, GLAccount) =>
        (from row in ctx.GLAccount
         where row.Company == Company &&
         row.COACode == COACode &&
         row.GLAccount1 == GLAccount
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, string, string, IEnumerable<EntityGLC>>> EntityGLCExpression =
      (ctx, Company, _RelatedToFile, VendorNum, InvoiceNum) =>
        (from row in ctx.EntityGLC
         where row.Company == Company &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum
         select row);


        private static Func<ErpContext, string, string, APChkGrp> findFirstAPChkGrpQuery;
        private APChkGrp FindFirstAPChkGrp(string Company, string GroupID)
        {
            if (findFirstAPChkGrpQuery == null)
            {
                Expression<Func<ErpContext, string, string, APChkGrp>> expression =
                    (context, Company_ex, GroupID_ex) =>
                    (from row in context.APChkGrp
                     where row.Company == Company_ex &&
                     row.GroupID == GroupID_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPChkGrpQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPChkGrpQuery(this.Db, Company, GroupID);
        }

        private static Func<ErpContext, string, string, APChkGrp> findFirstAPChkGrpWithUpdLockQuery;
        private APChkGrp FindFirstAPChkGrpWithUpdLock(string Company, string GroupID)
        {
            if (findFirstAPChkGrpWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, APChkGrp>> expression =
                    (context, Company_ex, GroupID_ex) =>
                    (from row in context.APChkGrp.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                     row.GroupID == GroupID_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPChkGrpWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPChkGrpWithUpdLockQuery(this.Db, Company, GroupID);
        }


        static Expression<Func<ErpContext, string, string, bool, IEnumerable<TranDocType>>> TranDocTypeExpression =
      (ctx, CompanyID, _SystemTranID, _SystemTranDefault) =>
        (from row in ctx.TranDocType
         where row.Company == CompanyID &&
         row.SystemTranID == _SystemTranID &&
         row.SystemTranDefault == _SystemTranDefault
         select row);


        private static Func<ErpContext, string, string, TranDocType> findFirstTranDocTypeQuery;
        private TranDocType FindFirstTranDocType(string CompanyID, string TranDocTypeID)
        {
            if (findFirstTranDocTypeQuery == null)
            {
                Expression<Func<ErpContext, string, string, TranDocType>> expression =
                    (context, CompanyID_ex, TranDocTypeID_ex) =>
                    (from row in context.TranDocType
                     where row.Company == CompanyID_ex &&
                           row.TranDocTypeID == TranDocTypeID_ex
                     select row).FirstOrDefault();
                findFirstTranDocTypeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTranDocTypeQuery(this.Db, CompanyID, TranDocTypeID);
        }

        private static Func<ErpContext, string, string, bool> findFirstTranDocTypeSelfInvoiceQuery;
        private bool FindFirstTranDocTypeSelfInvoice(string company, string tranDocTypeID)
        {
            if (findFirstTranDocTypeSelfInvoiceQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (context, company_ex, tranDocTypeID_ex) =>
                    (from row in context.TranDocType
                     where row.Company == company_ex && row.TranDocTypeID == tranDocTypeID_ex
                     select row.SelfInvoice)
                    .FirstOrDefault();
                findFirstTranDocTypeSelfInvoiceQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstTranDocTypeSelfInvoiceQuery(this.Db, company, tranDocTypeID);
        }

        static Expression<Func<ErpContext, string, string, bool, TranDocType>> TranDocTypeExpression3 =
      (ctx, CompanyID, cDocType, _SystemTranDefault) =>
        (from row in ctx.TranDocType
         where row.Company == CompanyID &&
         row.SystemTranID == cDocType &&
         row.SystemTranDefault == _SystemTranDefault
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, string, int, bool>> APInvMscExpression9 =
      (ctx, Company, InvoiceNum, VendorNum) =>
        (from row in ctx.APInvMsc
         where row.Company == Company &&
         row.InvoiceNum == InvoiceNum &&
         row.VendorNum == VendorNum
         select row).Any();


        static Expression<Func<ErpContext, string, string, int, bool>> APInvDtlExpression8 =
      (ctx, Company, InvoiceNum, VendorNum) =>
        (from row in ctx.APInvDtl
         where row.Company == Company &&
         row.InvoiceNum == InvoiceNum &&
         row.VendorNum == VendorNum
         select row).Any();


        static Expression<Func<ErpContext, string, string, int, bool>> APInvTaxExpression3 =
      (ctx, Company, InvoiceNum, VendorNum) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.InvoiceNum == InvoiceNum &&
         row.VendorNum == VendorNum
         select row).Any();


        static Func<ErpContext, string, int, string, bool> existsAPInvMscQuery;
        private bool ExistsAPInvMsc(string company, int VendorNum, string InvoiceNum)
        {
            if (existsAPInvMscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                (ctx, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in ctx.APInvMsc
                     where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex
                     select row.Company).Any();
                existsAPInvMscQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvMscQuery(this.Db, company, VendorNum, InvoiceNum);
        }

        static Expression<Func<ErpContext, string, string, int, bool>> APInvMscExpression10 =
      (ctx, Company, InvoiceNum, VendorNum) =>
        (from row in ctx.APInvMsc
         where row.Company == Company &&
         row.InvoiceNum == InvoiceNum &&
         row.VendorNum == VendorNum
         select row).Any();


        static Func<ErpContext, string, int, string, bool> existsAPInvDtlQuery;
        private bool ExistsAPInvDtl(string company, int VendorNum, string InvoiceNum)
        {
            if (existsAPInvDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                (ctx, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in ctx.APInvDtl
                     where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex
                     select row.Company).Any();
                existsAPInvDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvDtlQuery(this.Db, company, VendorNum, InvoiceNum);
        }

        static Expression<Func<ErpContext, string, string, int, bool>> APInvDtlExpression9 =
      (ctx, Company, InvoiceNum, VendorNum) =>
        (from row in ctx.APInvDtl
         where row.Company == Company &&
         row.InvoiceNum == InvoiceNum &&
         row.VendorNum == VendorNum
         select row).Any();


        static Func<ErpContext, string, string, int, int, bool> existsAPInvDtlDRMQuery;
        private bool ExistsAPInvDtlDRM(string company, string InvoiceNum, int VendorNum, int DRMNum)
        {
            if (existsAPInvDtlDRMQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
                (ctx, Company_ex, InvoiceNum_ex, VendorNum_ex, DRMNum_ex) =>
                    (from row in ctx.APInvDtl
                     where row.Company == Company_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.DMRNum != DRMNum_ex
                     select row).Any();
                existsAPInvDtlDRMQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvDtlDRMQuery(this.Db, company, InvoiceNum, VendorNum, DRMNum);
        }


        static Expression<Func<ErpContext, string, string, int, int, bool>> APInvDtlExpression10 =
      (ctx, Company, InvoiceNum, VendorNum, _DMRNum) =>
        (from row in ctx.APInvDtl
         where row.Company == Company &&
         row.InvoiceNum == InvoiceNum &&
         row.VendorNum == VendorNum &&
         row.DMRNum != _DMRNum
         select row).Any();


        static Func<ErpContext, string, int, string, bool> existsAPInvTaxnQuery;
        private bool ExistsAPInvTax(string company, int VendorNum, string InvoiceNum)
        {
            if (existsAPInvTaxnQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                (ctx, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in ctx.APInvTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex
                     select row.Company).Any();
                existsAPInvTaxnQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvTaxnQuery(this.Db, company, VendorNum, InvoiceNum);
        }

        static Func<ErpContext, string, int, string, string, string, int, bool> existsAPInvTax2Query;
        private bool ExistsAPInvTax2(string company, int VendorNum, string InvoiceNum, string TaxCode, string RateCode, int ECAcq)
        {
            if (existsAPInvTax2Query == null)
            {
                Expression<Func<ErpContext, string, int, string, string, string, int, bool>> expression =
                (ctx, Company_ex, VendorNum_ex, InvoiceNum_ex, TaxCode_ex, RateCode_ex, EcAcq_ex) =>
                    (from row in ctx.APInvTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.TaxCode == TaxCode_ex &&
                     row.RateCode == RateCode_ex &&
                     row.ECAcquisitionSeq == EcAcq_ex
                     select row.Company).Any();
                existsAPInvTax2Query = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvTax2Query(this.Db, company, VendorNum, InvoiceNum, TaxCode, RateCode, ECAcq);
        }

        static Expression<Func<ErpContext, string, int, string, bool>> APInvTaxExpression4 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row).Any();


        static Func<ErpContext, string, string, bool> existsSystemTranDocTypeQuery;
        private bool ExistsSystemTranDocType(string company, string SystemTranType)
        {
            if (existsSystemTranDocTypeQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                (ctx, Company_ex, SystemTranType_ex) =>
                    (from row in ctx.TranDocType
                     where row.Company == Company_ex &&
                     row.SystemTranID == SystemTranType_ex
                     select row).Any();
                existsSystemTranDocTypeQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsSystemTranDocTypeQuery(this.Db, company, SystemTranType);
        }

        static Expression<Func<ErpContext, string, string, bool>> TranDocTypeExpression4 =
      (ctx, CompanyID, SystemTranType) =>
        (from row in ctx.TranDocType
         where row.Company == CompanyID &&
         row.SystemTranID == SystemTranType
         select row).Any();


        static Func<ErpContext, string, string, string> findFirstTranDocTypeDescQuery;
        private string FindFirstTranDocTypeDescription(string company, string TranDocTypeID)
        {
            if (findFirstTranDocTypeDescQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
                (ctx, Company_ex, TranDocTypeID_ex) =>
                    (from row in ctx.TranDocType
                     where row.Company == Company_ex &&
                     row.TranDocTypeID == TranDocTypeID_ex
                     select row.Description).FirstOrDefault();
                findFirstTranDocTypeDescQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTranDocTypeDescQuery(this.Db, company, TranDocTypeID);
        }


        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvTax>>> APInvTaxExpression5 =
      (ctx, Company, VendorNum, InvoiceNum, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.ECAcquisitionSeq == _ECAcquisitionSeq
         select row);



        static Expression<Func<ErpContext, string, string, int, CheckHed>> CheckHedExpression =
      (ctx, CompanyID, vGroupID, vHeadNum) =>
        (from row in ctx.CheckHed.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.GroupID == vGroupID &&
         row.HeadNum == vHeadNum
         select row).FirstOrDefault();
        //HOLDLOCK



        static Expression<Func<ErpContext, string, string, string, IEnumerable<CurrExChain>>> CurrExChainExpression =
      (ctx, CompanyID, _TableName, HeadNum) =>
        (from row in ctx.CurrExChain.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.TableName == _TableName &&
         row.Key1 == HeadNum
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, LogAPInv>> LogAPInvExpression =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.LogAPInv.With(LockHint.UpdLock)
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row).FirstOrDefault();
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, bool>> APInvHedExpression24 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvHed
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row).Count() == 1;



        static Func<ErpContext, string, int, string, bool, APInvHed> findFirstBAPInvHedByRefInvoiceQuery;
        private APInvHed FindFirstBAPInvHedByRefInvoice(string Company, int VendorNum, string InvoiceRef, bool cpay)
        {
            if (findFirstBAPInvHedByRefInvoiceQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, APInvHed>> expression =
                (ctx, Company_ex, VendorNum_ex, InvoiceRef_ex, _CPay) =>
                (from row in ctx.APInvHed
                 where row.Company == Company_ex &&
                 row.VendorNum == VendorNum_ex &&
                 row.InvoiceNum == InvoiceRef_ex &&
                 row.CPay == _CPay
                 select row).FirstOrDefault();
                findFirstBAPInvHedByRefInvoiceQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstBAPInvHedByRefInvoiceQuery(this.Db, Company, VendorNum, InvoiceRef, cpay);
        }



        static Expression<Func<ErpContext, string, int, string, bool>> APInvHedExpression25 =
      (ctx, Company, VendorNum, ScrInvoiceRef) =>
        (from row in ctx.APInvHed
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == ScrInvoiceRef &&
         !row.DebitMemo == true &&
         row.Posted == true
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, bool>> LegalNumCnfgExpression =
      (ctx, CompanyID, cLegalNumberType) =>
        (from row in ctx.LegalNumCnfg
         where row.Company == CompanyID &&
         row.LegalNumberType == cLegalNumberType
         select row).Any();


        static Expression<Func<ErpContext, string, string, bool>> LegalNumDocTypeExpression =
              (ctx, companyID, legalNumberId) =>
                (from row in ctx.LegalNumDocType
                 where row.Company == companyID &&
                 row.LegalNumberID == legalNumberId
                 select row).Any();


        static Expression<Func<ErpContext, string, string, Vendor>> VendorExpression2 =
      (ctx, Company, VendorNumVendorID) =>
        (from row in ctx.Vendor
         where row.Company == Company &&
         row.VendorID == VendorNumVendorID
         select row).FirstOrDefault();

        private class VendorPartialRow : Epicor.Data.TempRowBase
        {
            public bool PayHold { get; set; }
            public bool Inactive { get; set; }
            public bool NoBankingReference { get; set; }
        }

        private static Func<ErpContext, string, int, VendorPartialRow> selectPartialVendorQuery;
        private VendorPartialRow SelectPartialVendor(string Company, int VendorNum)
        {
            if (selectPartialVendorQuery == null)
            {
                Expression<Func<ErpContext, string, int, VendorPartialRow>> expression =
                    (context, Company_ex, VendorNum_ex) =>
                    (from row in context.Vendor
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex
                     select new VendorPartialRow()
                     {
                         PayHold = row.PayHold,
                         Inactive = row.Inactive,
                         NoBankingReference = row.NoBankingReference
                     }).FirstOrDefault();

                selectPartialVendorQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectPartialVendorQuery(this.Db, Company, VendorNum);
        }



        static Expression<Func<ErpContext, string, int, string, bool>> APInvMscExpression11 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvMsc
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
        (row.ContainerID != 0 ||
         row.PackSlip != string.Empty)
         select row).Any();



        static Expression<Func<ErpContext, string, string, string, string, IEnumerable<Memo>>> MemoExpression =
      (ctx, Company, GroupID, VendorNum, InvoiceNum) =>
        (from row in ctx.Memo.With(LockHint.UpdLock)
         where row.Company == Company &&
         row.RelatedToFile == "APInvHed" &&
         row.RelatedToSchemaName == "Erp" &&
         row.Key1 == GroupID &&
         row.Key2 == VendorNum &&
         row.Key3 == InvoiceNum
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, string, IEnumerable<RebateTrans>>> RebateTransExpression2 =
      (ctx, CompanyID, InvoiceNum) =>
        (from row in ctx.RebateTrans.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.APInvoiceNum == InvoiceNum
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, string, TaxSvcHead>> TaxSvcHeadExpression =
      (ctx, CompanyID, vTaxSvcID) =>
        (from row in ctx.TaxSvcHead
         where row.Company == CompanyID &&
         row.TaxSvcID == vTaxSvcID
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, bool>> LogAPInvExpression2 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.LogAPInv
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row).Any();


        static Func<ErpContext, string, int, int, POHeader> findFirstPOHeaderWithVendorQuery;
        private POHeader FindFirstPOHeader(string Company, int VendorNum, int PONum)
        {
            if (findFirstPOHeaderWithVendorQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, POHeader>> expression =
                (ctx, Company_ex, VendorNum_ex, PONum_ex) =>
                (from row in ctx.POHeader
                 where row.Company == Company_ex &&
                 row.VendorNum == VendorNum_ex &&
                 row.PONum == PONum_ex
                 select row).FirstOrDefault();
                findFirstPOHeaderWithVendorQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPOHeaderWithVendorQuery(this.Db, Company, VendorNum, PONum);
        }

        static Func<ErpContext, string, int, POHeader> findFirstPOHeaderQuery;
        private POHeader FindFirstPOHeader(string Company, int PONum)
        {
            if (findFirstPOHeaderQuery == null)
            {
                Expression<Func<ErpContext, string, int, POHeader>> expression =
                (ctx, Company_ex, PONum_ex) =>
                (from row in ctx.POHeader
                 where row.Company == Company_ex &&
                 row.PONum == PONum_ex
                 select row).FirstOrDefault();
                findFirstPOHeaderQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPOHeaderQuery(this.Db, Company, PONum);
        }


        static Expression<Func<ErpContext, string, int, APInvDtl>> AltAPInvDtlExpression =
      (ctx, CompanyID, REFPONum) =>
        (from row in ctx.APInvDtl
         where row.Company == CompanyID &&
         row.PONum == REFPONum
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, string, bool>> PurTermsExpression2 =
      (ctx, Company, TermsCode) =>
        (from row in ctx.PurTerms
         where row.Company == Company &&
         row.TermsCode == TermsCode
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, int, string, bool, bool, bool>> APInvHedExpression29 =
      (ctx, Company, VendorNum, ScrInvoiceRef, _DebitMemo, _Posted) =>
        (from row in ctx.APInvHed
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == ScrInvoiceRef &&
         row.DebitMemo == _DebitMemo &&
         row.Posted == _Posted
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, APLOC>> APLOCExpression =
      (ctx, CompanyID, APLOCID) =>
        (from row in ctx.APLOC
         where row.Company == CompanyID &&
         row.LCID == APLOCID
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, int, bool>> APLOCExpression2 =
      (ctx, CompanyID, APLOCID, VendorNum) =>
        (from row in ctx.APLOC
         where row.Company == CompanyID &&
         row.LCID == APLOCID &&
         row.VendorNum == VendorNum
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, int, string, bool>> APLOCExpression3 =
      (ctx, CompanyID, APLOCID, VendorNum, CurrencyCode) =>
        (from row in ctx.APLOC
         where row.Company == CompanyID &&
         row.LCID == APLOCID &&
         row.VendorNum == VendorNum &&
         row.CurrencyCode == CurrencyCode
         select row).Count() == 1;


        static Expression<Func<ErpContext, string, int, bool, bool>> VendorExpression4 =
      (ctx, Company, VendorNum, _GlobalVendor) =>
        (from row in ctx.Vendor
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.GlobalVendor == _GlobalVendor
         select row).Any();



        static Expression<Func<ErpContext, string, int, bool>> GlbVendorExpression =
      (ctx, Company, VendorNum) =>
        (from row in ctx.GlbVendor
         where row.Company == Company &&
         row.VendorNum == VendorNum
         select row).Any();



        static Expression<Func<ErpContext, string, string, APInvHed>> AltAPInvHedExpression2 =
      (ctx, CompanyID, PrePaymentNum) =>
        (from row in ctx.APInvHed
         where row.Company == CompanyID &&
         row.InvoiceNum == PrePaymentNum
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, Guid, APInvHed>> APInvHedExpression30 =
      (ctx, rSaveAPInvHedRowid) =>
        (from row in ctx.APInvHed.With(LockHint.UpdLock)
         where row.SysRowID == rSaveAPInvHedRowid
         select row).FirstOrDefault();
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, IEnumerable<APTran>>> APTranExpression =
      (ctx, Company, vCheckNum, vInvoiceRef) =>
        (from row in ctx.APTran.With(LockHint.UpdLock)
         where row.Company == Company &&
         row.HeadNum == vCheckNum &&
         row.InvoiceNum == vInvoiceRef
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvTax>>> APInvTaxExpression6 =
      (ctx, Company, VendorNum, InvoiceNum, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.ECAcquisitionSeq == _ECAcquisitionSeq
         select row);


        static Func<ErpContext, string, string, string, ProjPhase> findFirstProjPhaseQuery;
        private ProjPhase FindFirstProjPhase(string CompanyID, string ProjectID, string PhaseID)
        {
            if (findFirstProjPhaseQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, ProjPhase>> expression =
                (ctx, CompanyID_ex, ProjectID_ex, PhaseID_ex) =>
                (from row in ctx.ProjPhase
                 where row.Company == CompanyID_ex &&
                 row.ProjectID == ProjectID_ex &&
                 row.PhaseID == PhaseID_ex
                 select row).FirstOrDefault();
                findFirstProjPhaseQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstProjPhaseQuery(this.Db, CompanyID, ProjectID, PhaseID);
        }

        static Func<ErpContext, string, int, FSCallhd> findFirstFSCallhdWithUpdLockExpression;
        private FSCallhd FindFirstFSCallhdWithUpdLock(string Company, int CallNum)
        {
            if (findFirstFSCallhdWithUpdLockExpression == null)
            {
                Expression<Func<ErpContext, string, int, FSCallhd>> expression =
                (ctx, Company_ex, CallNum_ex) =>
                (from row in ctx.FSCallhd.With(LockHint.UpdLock)
                 where row.Company == Company_ex &&
                 row.CallNum == CallNum_ex
                 select row).FirstOrDefault();
                findFirstFSCallhdWithUpdLockExpression = DBExpressionCompiler.Compile(expression);
            }

            return findFirstFSCallhdWithUpdLockExpression(this.Db, Company, CallNum);
        }


        static Expression<Func<ErpContext, string, string, bool, bool>> JobHeadExpression2 =
      (ctx, Company, JobNum, _JobClosed) =>
        (from row in ctx.JobHead
         where row.Company == Company &&
         row.JobNum == JobNum &&
         row.JobClosed == _JobClosed
         select row).Count() == 1;

        static Func<ErpContext, string, string, bool, JobHead> findJobHeadExpression3;
        private JobHead FindFirstJobHead(string Company, string JobNum, bool _JobClosed)
        {
            if (findJobHeadExpression3 == null)
            {
                Expression<Func<ErpContext, string, string, bool, JobHead>> expression =
                    (context, Company_ex, JobNum_ex, _JobClosed) =>
                        (from row in context.JobHead
                         where row.Company == Company_ex &&
                               row.JobNum == JobNum_ex &&
                               row.JobClosed == _JobClosed
                         select row).FirstOrDefault();

                findJobHeadExpression3 = DBExpressionCompiler.Compile(expression);
            }

            return findJobHeadExpression3(this.Db, Company, JobNum, _JobClosed);
        }

        static Expression<Func<ErpContext, string, string, int, bool>> JobAsmblExpression =
      (ctx, Company, JobNum, AssemblySeq) =>
        (from row in ctx.JobAsmbl
         where row.Company == Company &&
         row.JobNum == JobNum &&
         row.AssemblySeq == AssemblySeq
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, int, int, bool, bool>> JobMtlExpression =
      (ctx, Company, JobNum, AssemblySeq, MtlSeq, _MiscCharge) =>
        (from row in ctx.JobMtl
         where row.Company == Company &&
         row.JobNum == JobNum &&
         row.AssemblySeq == AssemblySeq &&
         row.MtlSeq == MtlSeq &&
         row.MiscCharge == _MiscCharge
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, int, APInvDtl>> AltAPInvDtlExpression2 =
      (ctx, CompanyID, InvoiceNum, InvoiceLine) =>
        (from row in ctx.APInvDtl
         where row.Company == CompanyID &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine
         select row).FirstOrDefault();


        class AltAPInvJobExpressionColumnResult
        {
            public string Company { get; set; }
            public int VendorNum { get; set; }
            public string InvoiceNum { get; set; }
            public int InvoiceLine { get; set; }
            public int AssemblySeq { get; set; }
            public int MtlSeq { get; set; }
            public decimal ExtCost { get; set; }
        }
        static Expression<Func<ErpContext, string, string, IEnumerable<AltAPInvJobExpressionColumnResult>>> AltAPInvJobExpression =
      (ctx, Company, JobNum) =>
        (from row in ctx.APInvJob
         where row.Company == Company &&
         row.JobNum == JobNum
         orderby row.ExtCost descending, row.Company, row.VendorNum, row.InvoiceNum, row.InvoiceLine, row.JobNum, row.AssemblySeq, row.MtlSeq
         select new AltAPInvJobExpressionColumnResult() { Company = row.Company, VendorNum = row.VendorNum, InvoiceNum = row.InvoiceNum, InvoiceLine = row.InvoiceLine, AssemblySeq = row.AssemblySeq, MtlSeq = row.MtlSeq, ExtCost = row.ExtCost });



        static Expression<Func<ErpContext, string, int, string, bool, string, bool>> APInvHedExpression31 =
      (ctx, Company, VendorNum, InvoiceNum, _Posted, PostGroupID) =>
        (from row in ctx.APInvHed
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
        (row.Posted == _Posted ||
         row.GroupID == PostGroupID)
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, int, int, decimal, JobMtl>> JobMtlExpression2 =
      (ctx, CompanyID, JobNum, AssemblySeq, MtlSeq, _EstUnitCost) =>
        (from row in ctx.JobMtl.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.JobNum == JobNum &&
         row.AssemblySeq == AssemblySeq &&
         row.MtlSeq == MtlSeq &&
         row.EstUnitCost == _EstUnitCost
         select row).FirstOrDefault();
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvTax>>> APInvTaxExpression7 =
      (ctx, Company, VendorNum, InvoiceNum, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.ECAcquisitionSeq == _ECAcquisitionSeq
         select row);


        static Expression<Func<ErpContext, string, string, bool, bool>> PurMiscExpression2 =
      (ctx, CompanyID, MiscCode, _LCFlag) =>
        (from row in ctx.PurMisc
         where row.Company == CompanyID &&
         row.MiscCode == MiscCode &&
         row.LCFlag == _LCFlag
         select row).Any();



        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvPB>>> AltAPInvPBExpression =
      (ctx, CompanyID, VendorNum, InvoiceNum, InvoiceLine) =>
        (from row in ctx.APInvPB
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine
         select row);



        static Expression<Func<ErpContext, string, int, string, int, Guid, IEnumerable<APInvPB>>> AltAPInvPBExpression2 =
      (ctx, CompanyID, VendorNum, InvoiceNum, InvoiceLine, SysRowID) =>
        (from row in ctx.APInvPB
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.SysRowID != SysRowID
         select row);



        static Expression<Func<ErpContext, string, string, bool, RoleCd>> RoleCdExpression =
      (ctx, CompanyID, RoleCd, _Inactive) =>
        (from row in ctx.RoleCd
         where row.Company == CompanyID &&
         row.RoleCode == RoleCd &&
         row.Inactive == _Inactive
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, int, string, int, Guid, IEnumerable<APInvPB>>> AltAPInvPBExpression3 =
      (ctx, CompanyID, VendorNum, InvoiceNum, InvoiceLine, SysRowID) =>
        (from row in ctx.APInvPB
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.SysRowID != SysRowID
         select row);


        static Func<ErpContext, string, string, string, DateTime, TaxRate> findFirstTaxRateQuery;
        private TaxRate FindFirstTaxRate(string Company, string TaxCode, string RateCode, DateTime effectivedate)
        {
            if (findFirstTaxRateQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, DateTime, TaxRate>> expression =
                (ctx, CompanyID, TaxCode_ex, RateCode_ex, effectivedate_ex) =>
                (from row in ctx.TaxRate
                 where row.Company == CompanyID &&
                 row.TaxCode == TaxCode_ex &&
                 row.RateCode == RateCode_ex &&
                 row.EffectiveFrom <= effectivedate_ex
                 select row).FirstOrDefault();
                findFirstTaxRateQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTaxRateQuery(this.Db, Company, TaxCode, RateCode, effectivedate);
        }


        static Expression<Func<ErpContext, string, string, string, DateTime, decimal, TaxGRate>> TaxGRateExpression =
      (ctx, CompanyID, TaxCode, RateCode, vEffectiveFrom, TaxableAmt) =>
        (from row in ctx.TaxGRate
         where row.Company == CompanyID &&
         row.TaxCode == TaxCode &&
         row.RateCode == RateCode &&
         row.EffectiveFrom <= vEffectiveFrom &&
         row.FromAmount >= TaxableAmt
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, string, string, int, APInvTax>> BAPInvTaxExpression =
      (ctx, Company, VendorNum, InvoiceNum, TaxCode, RateCode, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax.With(LockHint.UpdLock)
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.TaxCode == TaxCode &&
         row.RateCode == RateCode &&
         row.ECAcquisitionSeq == _ECAcquisitionSeq
         select row).FirstOrDefault();
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvTax>>> APInvTaxExpression8 =
      (ctx, Company, VendorNum, InvoiceNum, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.ECAcquisitionSeq == _ECAcquisitionSeq
         select row);



        static Expression<Func<ErpContext, string, int, string, string, string, int, bool>> APInvTaxExpression9 =
      (ctx, Company, VendorNum, InvoiceNum, TaxCode, RateCode, ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.TaxCode == TaxCode &&
         row.RateCode == RateCode &&
         row.ECAcquisitionSeq == ECAcquisitionSeq
         select row).Any();



        static Expression<Func<ErpContext, string, int, string, bool, bool, bool>> APInvHedExpression39 =
      (ctx, Company, VendorNum, InvoiceNum, _MatchedFromLI, _AllowOverrideLI) =>
        (from row in ctx.APInvHed
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.MatchedFromLI == _MatchedFromLI &&
         row.AllowOverrideLI == _AllowOverrideLI
         select row).Any();



        static Expression<Func<ErpContext, string, string, SalesTax>> SalesTaxExpression4 =
      (ctx, Company, TaxCode) =>
        (from row in ctx.SalesTax
         where row.Company == Company &&
         row.TaxCode == TaxCode
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, DateTime?, DateTime?, FiscalPer>> FiscalPerExpression =
      (ctx, CompanyID, CompanyFiscalCalendarID, TransApplyDate, TransApplyDate2) =>
        (from row in ctx.FiscalPer
         where row.Company == CompanyID &&
         row.FiscalCalendarID == CompanyFiscalCalendarID &&
         row.StartDate.Value <= TransApplyDate.Value &&
         row.EndDate.Value >= TransApplyDate2.Value
         select row).FirstOrDefault();



        class APInvDtlExpression19ColumnResult
        {
            public decimal ExtCost { get; set; }
            public decimal Rpt1ExtCost { get; set; }
            public decimal Rpt2ExtCost { get; set; }
            public decimal Rpt3ExtCost { get; set; }
            public decimal DocExtCost { get; set; }
            public decimal TotalMiscChrg { get; set; }
            public decimal Rpt1TotalMiscChrg { get; set; }
            public decimal Rpt2TotalMiscChrg { get; set; }
            public decimal Rpt3TotalMiscChrg { get; set; }
            public decimal DocTotalMiscChrg { get; set; }
            public decimal AdvancePayAmt { get; set; }
            public decimal Rpt1AdvancePayAmt { get; set; }
            public decimal Rpt2AdvancePayAmt { get; set; }
            public decimal Rpt3AdvancePayAmt { get; set; }
            public decimal DocAdvancePayAmt { get; set; }
            public int VendorNum { get; set; }
            public string InvoiceNum { get; set; }
            public int InvoiceLine { get; set; }
            public decimal ScrWithholdAmt { get; set; }
            public decimal DocScrWithholdAmt { get; set; }
            public decimal Rpt1ScrWithholdAmt { get; set; }
            public decimal Rpt2ScrWithholdAmt { get; set; }
            public decimal Rpt3ScrWithholdAmt { get; set; }
        }

        static Expression<Func<ErpContext, string, int, string, IEnumerable<APInvDtlExpression19ColumnResult>>> APInvDtlExpression19 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select new APInvDtlExpression19ColumnResult()
         {
             ExtCost = row.ExtCost,
             Rpt1ExtCost = row.Rpt1ExtCost,
             Rpt2ExtCost = row.Rpt2ExtCost,
             Rpt3ExtCost = row.Rpt3ExtCost,
             DocExtCost = row.DocExtCost,
             TotalMiscChrg = row.TotalMiscChrg,
             Rpt1TotalMiscChrg = row.Rpt1TotalMiscChrg,
             Rpt2TotalMiscChrg = row.Rpt2TotalMiscChrg,
             Rpt3TotalMiscChrg = row.Rpt3TotalMiscChrg,
             DocTotalMiscChrg = row.DocTotalMiscChrg,
             AdvancePayAmt = row.AdvancePayAmt,
             Rpt1AdvancePayAmt = row.Rpt1AdvancePayAmt,
             Rpt2AdvancePayAmt = row.Rpt2AdvancePayAmt,
             Rpt3AdvancePayAmt = row.Rpt3AdvancePayAmt,
             DocAdvancePayAmt = row.DocAdvancePayAmt,
             VendorNum = row.VendorNum,
             InvoiceNum = row.InvoiceNum,
             InvoiceLine = row.InvoiceLine,
             ScrWithholdAmt = row.ScrWithholdAmt,
             DocScrWithholdAmt = row.DocScrWithholdAmt,
             Rpt1ScrWithholdAmt = row.Rpt1ScrWithholdAmt,
             Rpt2ScrWithholdAmt = row.Rpt2ScrWithholdAmt,
             Rpt3ScrWithholdAmt = row.Rpt3ScrWithholdAmt
         });

        class ExistsAPRecordsColumnResult
        {
            public string InvoiceNum { get; set; }
            public bool ExistsLines { get; set; }
            public bool ExistsMsc { get; set; }
            public bool ExistsTaxes { get; set; }
            public bool ExistsMiscLines { get; set; }
            public bool ExistsLACMiscs { get; set; }
        }

        static Func<ErpContext, string, int, bool, bool, bool, bool, DateTime, IEnumerable<ExistsAPRecordsColumnResult>> selectAPInvHedGetRowsTrackerBooleansQuery;
        private IEnumerable<ExistsAPRecordsColumnResult> SelectAPInvHedBooleans(string company, int VendorNum, bool Posted, bool all, bool open, bool inRange, DateTime fromDay)
        {
            if (selectAPInvHedGetRowsTrackerBooleansQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool, bool, bool, bool, DateTime, IEnumerable<ExistsAPRecordsColumnResult>>> expression =
      (ctx, company_ex, VendorNum_ex, Posted_ex, all_ex, open_ex, inRange_ex, fromDay_ex) =>
        (from row in ctx.APInvHed
         where row.Company == company_ex &&
         row.VendorNum == VendorNum_ex &&
         row.Posted == Posted_ex &&
         (all_ex || row.OpenPayable == open_ex) &&
         (!inRange_ex || DateTime.Compare(fromDay_ex, (DateTime)row.InvoiceDate) <= 0)
         select new ExistsAPRecordsColumnResult()
         {
             InvoiceNum = row.InvoiceNum,
             ExistsLines = (from row2 in ctx.APInvDtl where row2.Company == row.Company && row2.InvoiceNum == row.InvoiceNum && row2.VendorNum == row.VendorNum select 1).Any(),
             ExistsMsc = (from row2 in ctx.APInvMsc where row2.Company == row.Company && row2.InvoiceNum == row.InvoiceNum && row2.VendorNum == row.VendorNum select 1).Any(),
             ExistsTaxes = (from row2 in ctx.APInvTax where row2.Company == row.Company && row2.InvoiceNum == row.InvoiceNum && row2.VendorNum == row.VendorNum select 1).Any(),
             ExistsMiscLines = (from row2 in ctx.APInvDtl where row2.Company == row.Company && row2.InvoiceNum == row.InvoiceNum && row2.VendorNum == row.VendorNum && row2.LineType != "M" select 1).Any(),
             ExistsLACMiscs = (from row2 in ctx.APInvMsc where row2.Company == row.Company && row2.InvoiceNum == row.InvoiceNum && row2.VendorNum == row.VendorNum && row2.InvoiceLine == 0 && row2.LCFlag select 1).Any(),
         });
                selectAPInvHedGetRowsTrackerBooleansQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectAPInvHedGetRowsTrackerBooleansQuery(this.Db, company, VendorNum, Posted, all, open, inRange, fromDay);
        }

        class APInvDtlExpression20ColumnResult
        {
            public string InvoiceNum { get; set; }
            public decimal ScrInvLineTotal { get; set; }
            public decimal ScrDocInvLineTotal { get; set; }
            public decimal Rpt1ScrInvLineTotal { get; set; }
            public decimal Rpt2ScrInvLineTotal { get; set; }
            public decimal Rpt3ScrInvLineTotal { get; set; }
            public decimal ScrWithholdAmt { get; set; }
            public decimal Rpt1ScrWithholdAmt { get; set; }
            public decimal Rpt2ScrWithholdAmt { get; set; }
            public decimal Rpt3ScrWithholdAmt { get; set; }
            public decimal DocScrWithholdAmt { get; set; }
        }

        static Func<ErpContext, string, int, string, APInvDtlExpression20ColumnResult> selectSumAPInvDtlQuery;
        private APInvDtlExpression20ColumnResult selectSumAPInvDtl(string company, int VendorNum, string InvoiceNum)
        {
            if (selectSumAPInvDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, APInvDtlExpression20ColumnResult>> expression =
      (ctx, company_ex, VendorNum_ex, InvoiceNum_ex) =>
        (from row in ctx.APInvDtl
         where row.Company == company_ex &&
         row.VendorNum == VendorNum_ex &&
         row.InvoiceNum == InvoiceNum_ex
         group row by new { row.Company, row.VendorNum, row.InvoiceNum } into empg
         select new APInvDtlExpression20ColumnResult()
         {
             ScrInvLineTotal = empg.Sum(x => x.ExtCost + x.TotalMiscChrg - x.AdvancePayAmt - x.ScrWithholdAmt),
             ScrDocInvLineTotal = empg.Sum(x => x.DocExtCost + x.DocTotalMiscChrg - x.DocAdvancePayAmt - x.DocScrWithholdAmt),
             Rpt1ScrInvLineTotal = empg.Sum(x => x.Rpt1ExtCost + x.Rpt1TotalMiscChrg - x.Rpt1AdvancePayAmt - x.Rpt1ScrWithholdAmt),
             Rpt2ScrInvLineTotal = empg.Sum(x => x.Rpt2ExtCost + x.Rpt2TotalMiscChrg - x.Rpt2AdvancePayAmt - x.Rpt2ScrWithholdAmt),
             Rpt3ScrInvLineTotal = empg.Sum(x => x.Rpt3ExtCost + x.Rpt3TotalMiscChrg - x.Rpt3AdvancePayAmt - x.Rpt3ScrWithholdAmt),
             ScrWithholdAmt = empg.Sum(x => x.ScrWithholdAmt),
             DocScrWithholdAmt = empg.Sum(x => x.DocScrWithholdAmt),
             Rpt1ScrWithholdAmt = empg.Sum(x => x.Rpt1ScrWithholdAmt),
             Rpt2ScrWithholdAmt = empg.Sum(x => x.Rpt2ScrWithholdAmt),
             Rpt3ScrWithholdAmt = empg.Sum(x => x.Rpt3ScrWithholdAmt),
         }).FirstOrDefault();
                selectSumAPInvDtlQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectSumAPInvDtlQuery(this.Db, company, VendorNum, InvoiceNum);
        }

        static Func<ErpContext, string, int, bool, bool, bool, bool, DateTime, IEnumerable<APInvDtlExpression20ColumnResult>> selectSumAllAPInvDtlQuery;
        private IEnumerable<APInvDtlExpression20ColumnResult> SelectSumAllAPInvDtl(string company, int VendorNum, bool Posted, bool all, bool open, bool inRange, DateTime fromDay)
        {
            if (selectSumAllAPInvDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool, bool, bool, bool, DateTime, IEnumerable<APInvDtlExpression20ColumnResult>>> expression =
      (ctx, company_ex, VendorNum_ex, Posted_ex, all_ex, open_ex, inRange_ex, fromDay_ex) =>
        (from row in ctx.APInvDtl
         join APInvHed_row in ctx.APInvHed
         on new { row.Company, row.InvoiceNum, row.VendorNum } equals new { APInvHed_row.Company, APInvHed_row.InvoiceNum, APInvHed_row.VendorNum }
         where row.Company == company_ex &&
         row.VendorNum == VendorNum_ex &&
         APInvHed_row.Posted == Posted_ex &&
         (all_ex || APInvHed_row.OpenPayable == open_ex) &&
         (!inRange_ex || DateTime.Compare(fromDay_ex, (DateTime)APInvHed_row.InvoiceDate) <= 0)
         group row by new { row.Company, row.VendorNum, row.InvoiceNum } into empg
         select new APInvDtlExpression20ColumnResult()
         {
             InvoiceNum = empg.Key.InvoiceNum,
             ScrInvLineTotal = empg.Sum(x => x.ExtCost + x.TotalMiscChrg - x.AdvancePayAmt - x.ScrWithholdAmt),
             ScrDocInvLineTotal = empg.Sum(x => x.DocExtCost + x.DocTotalMiscChrg - x.DocAdvancePayAmt - x.DocScrWithholdAmt),
             Rpt1ScrInvLineTotal = empg.Sum(x => x.Rpt1ExtCost + x.Rpt1TotalMiscChrg - x.Rpt1AdvancePayAmt - x.Rpt1ScrWithholdAmt),
             Rpt2ScrInvLineTotal = empg.Sum(x => x.Rpt2ExtCost + x.Rpt2TotalMiscChrg - x.Rpt2AdvancePayAmt - x.Rpt2ScrWithholdAmt),
             Rpt3ScrInvLineTotal = empg.Sum(x => x.Rpt3ExtCost + x.Rpt3TotalMiscChrg - x.Rpt3AdvancePayAmt - x.Rpt3ScrWithholdAmt),
             ScrWithholdAmt = empg.Sum(x => x.ScrWithholdAmt),
             DocScrWithholdAmt = empg.Sum(x => x.DocScrWithholdAmt),
             Rpt1ScrWithholdAmt = empg.Sum(x => x.Rpt1ScrWithholdAmt),
             Rpt2ScrWithholdAmt = empg.Sum(x => x.Rpt2ScrWithholdAmt),
             Rpt3ScrWithholdAmt = empg.Sum(x => x.Rpt3ScrWithholdAmt),
         });
                selectSumAllAPInvDtlQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectSumAllAPInvDtlQuery(this.Db, company, VendorNum, Posted, all, open, inRange, fromDay);
        }



        class APInvMscExpression13ColumnResult
        {
            public string InvoiceNum { get; set; }
            public decimal MiscAmt { get; set; }
            public decimal DocMiscAmt { get; set; }
            public decimal Rpt1MiscAmt { get; set; }
            public decimal Rpt2MiscAmt { get; set; }
            public decimal Rpt3MiscAmt { get; set; }
        }

        static Func<ErpContext, string, int, int, bool, bool, bool, bool, DateTime, IEnumerable<APInvMscExpression13ColumnResult>> selectSumAllAPInvMscQuery;
        private IEnumerable<APInvMscExpression13ColumnResult> SelectSumAllAPInvMsc(string company, int VendorNum, int InvoiceLine, bool Posted, bool all, bool open, bool inRange, DateTime fromDay)
        {
            if (selectSumAllAPInvMscQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, bool, bool, bool, bool, DateTime, IEnumerable<APInvMscExpression13ColumnResult>>> expression =
      (ctx, company_ex, VendorNum_ex, InvoiceLine_ex, Posted_ex, all_ex, open_ex, inRange_ex, fromDay_ex) =>
        (from row in ctx.APInvMsc
         join APInvHed_row in ctx.APInvHed
         on new { row.Company, row.InvoiceNum } equals new { APInvHed_row.Company, APInvHed_row.InvoiceNum }
         where row.Company == company_ex &&
         row.VendorNum == VendorNum_ex &&
         row.InvoiceLine == InvoiceLine_ex &&
         APInvHed_row.Posted == Posted_ex &&
         (all_ex || APInvHed_row.OpenPayable == open_ex) &&
         (!inRange_ex || DateTime.Compare(fromDay_ex, (DateTime)APInvHed_row.InvoiceDate) <= 0)
         group row by new { row.Company, row.VendorNum, row.InvoiceNum, row.InvoiceLine } into empg
         select new APInvMscExpression13ColumnResult()
         {
             InvoiceNum = empg.Key.InvoiceNum,
             MiscAmt = empg.Sum(x => x.MiscAmt),
             DocMiscAmt = empg.Sum(x => x.DocMiscAmt),
             Rpt1MiscAmt = empg.Sum(x => x.Rpt1MiscAmt),
             Rpt2MiscAmt = empg.Sum(x => x.Rpt2MiscAmt),
             Rpt3MiscAmt = empg.Sum(x => x.Rpt3MiscAmt),
         });
                selectSumAllAPInvMscQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectSumAllAPInvMscQuery(this.Db, company, VendorNum, InvoiceLine, Posted, all, open, inRange, fromDay);
        }

        static Func<ErpContext, string, int, string, int, APInvMscExpression13ColumnResult> selectSumAPInvMscQuery;
        private APInvMscExpression13ColumnResult SelectSumAPInvMsc(string company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectSumAPInvMscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, APInvMscExpression13ColumnResult>> expression =
      (ctx, company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
        (from row in ctx.APInvMsc
         where row.Company == company_ex &&
         row.VendorNum == VendorNum_ex &&
         row.InvoiceNum == InvoiceNum_ex &&
         row.InvoiceLine == InvoiceLine_ex
         group row by new { row.Company, row.VendorNum, row.InvoiceNum, row.InvoiceLine } into empg
         select new APInvMscExpression13ColumnResult()
         {
             MiscAmt = empg.Sum(x => x.MiscAmt),
             DocMiscAmt = empg.Sum(x => x.DocMiscAmt),
             Rpt1MiscAmt = empg.Sum(x => x.Rpt1MiscAmt),
             Rpt2MiscAmt = empg.Sum(x => x.Rpt2MiscAmt),
             Rpt3MiscAmt = empg.Sum(x => x.Rpt3MiscAmt),
         }).FirstOrDefault();
                selectSumAPInvMscQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectSumAPInvMscQuery(this.Db, company, VendorNum, InvoiceNum, InvoiceLine);
        }

        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvMscExpression13ColumnResult>>> APInvMscExpression13 =
      (ctx, Company, VendorNum, InvoiceNum, _InvoiceLine) =>
        (from row in ctx.APInvMsc
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == _InvoiceLine
         select new APInvMscExpression13ColumnResult() { MiscAmt = row.MiscAmt, DocMiscAmt = row.DocMiscAmt, Rpt1MiscAmt = row.Rpt1MiscAmt, Rpt2MiscAmt = row.Rpt2MiscAmt, Rpt3MiscAmt = row.Rpt3MiscAmt });



        class APInvExpExpression13ColumnResult
        {
            public string InvoiceNum { get; set; }
            public decimal ExpAmt { get; set; }
            public decimal DocExpAmt { get; set; }
            public decimal Rpt1ExpAmt { get; set; }
            public decimal Rpt2ExpAmt { get; set; }
            public decimal Rpt3ExpAmt { get; set; }
        }

        static Func<ErpContext, string, int, int, bool, bool, bool, bool, DateTime, IEnumerable<APInvExpExpression13ColumnResult>> selectSumAllAPInvExpQuery;
        private IEnumerable<APInvExpExpression13ColumnResult> SelectSumAllAPInvExp(string company, int VendorNum, int InvoiceLine, bool Posted, bool all, bool open, bool inRange, DateTime fromDay)
        {
            if (selectSumAllAPInvExpQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, bool, bool, bool, bool, DateTime, IEnumerable<APInvExpExpression13ColumnResult>>> expression =
      (ctx, company_ex, VendorNum_ex, InvoiceLine_ex, Posted_ex, all_ex, open_ex, inRange_ex, fromDay_ex) =>
        (from row in ctx.APInvExp
         join APInvHed_row in ctx.APInvHed
         on new { row.Company, row.InvoiceNum } equals new { APInvHed_row.Company, APInvHed_row.InvoiceNum }
         where row.Company == company_ex &&
         row.VendorNum == VendorNum_ex &&
         row.InvoiceLine == InvoiceLine_ex &&
         APInvHed_row.Posted == Posted_ex &&
         (all_ex || APInvHed_row.OpenPayable == open_ex) &&
         (!inRange_ex || DateTime.Compare(fromDay_ex, (DateTime)APInvHed_row.InvoiceDate) <= 0)
         group row by new { row.Company, row.VendorNum, row.InvoiceNum, row.InvoiceLine } into empg
         select new APInvExpExpression13ColumnResult()
         {
             InvoiceNum = empg.Key.InvoiceNum,
             ExpAmt = empg.Sum(x => x.ExpAmt),
             DocExpAmt = empg.Sum(x => x.DocExpAmt),
             Rpt1ExpAmt = empg.Sum(x => x.Rpt1ExpAmt),
             Rpt2ExpAmt = empg.Sum(x => x.Rpt2ExpAmt),
             Rpt3ExpAmt = empg.Sum(x => x.Rpt3ExpAmt),
         });
                selectSumAllAPInvExpQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectSumAllAPInvExpQuery(this.Db, company, VendorNum, InvoiceLine, Posted, all, open, inRange, fromDay);
        }

        static Func<ErpContext, string, int, string, int, APInvExpExpression13ColumnResult> selectSumAPInvExpQuery;
        private APInvExpExpression13ColumnResult SelectSumAPInvExp(string company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectSumAPInvExpQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, APInvExpExpression13ColumnResult>> expression =
      (ctx, company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
        (from row in ctx.APInvExp
         where row.Company == company_ex &&
         row.VendorNum == VendorNum_ex &&
         row.InvoiceNum == InvoiceNum_ex &&
         row.InvoiceLine == InvoiceLine_ex
         group row by new { row.Company, row.VendorNum, row.InvoiceNum, row.InvoiceLine } into empg
         select new APInvExpExpression13ColumnResult()
         {
             ExpAmt = empg.Sum(x => x.ExpAmt),
             DocExpAmt = empg.Sum(x => x.DocExpAmt),
             Rpt1ExpAmt = empg.Sum(x => x.Rpt1ExpAmt),
             Rpt2ExpAmt = empg.Sum(x => x.Rpt2ExpAmt),
             Rpt3ExpAmt = empg.Sum(x => x.Rpt3ExpAmt),
         }).FirstOrDefault();
                selectSumAPInvExpQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectSumAPInvExpQuery(this.Db, company, VendorNum, InvoiceNum, InvoiceLine);
        }

        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvExpExpression13ColumnResult>>> APInvExpExpression13 =
      (ctx, Company, VendorNum, InvoiceNum, _InvoiceLine) =>
        (from row in ctx.APInvExp
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == _InvoiceLine
         select new APInvExpExpression13ColumnResult() { ExpAmt = row.ExpAmt, DocExpAmt = row.DocExpAmt, Rpt1ExpAmt = row.Rpt1ExpAmt, Rpt2ExpAmt = row.Rpt2ExpAmt, Rpt3ExpAmt = row.Rpt3ExpAmt });

        class APInvTaxExpression10ColumnResult
        {
            public decimal TaxableAmt { get; set; }
            public decimal Rpt1TaxableAmt { get; set; }
            public decimal Rpt2TaxableAmt { get; set; }
            public decimal Rpt3TaxableAmt { get; set; }
            public decimal DocTaxableAmt { get; set; }
            public decimal ReportableAmt { get; set; }
            public decimal Rpt1ReportableAmt { get; set; }
            public decimal Rpt2ReportableAmt { get; set; }
            public decimal Rpt3ReportableAmt { get; set; }
            public decimal DocReportableAmt { get; set; }
            public decimal TaxAmt { get; set; }
            public decimal Rpt1TaxAmt { get; set; }
            public decimal Rpt2TaxAmt { get; set; }
            public decimal Rpt3TaxAmt { get; set; }
            public decimal DocTaxAmt { get; set; }
            public decimal TaxAmtVar { get; set; }
            public decimal DocTaxAmtVar { get; set; }
            public decimal Rpt1TaxAmtVar { get; set; }
            public decimal DedTaxAmt { get; set; }
            public decimal Rpt1DedTaxAmt { get; set; }
            public decimal Rpt2DedTaxAmt { get; set; }
            public decimal Rpt3DedTaxAmt { get; set; }
            public decimal DocDedTaxAmt { get; set; }
            public decimal Rpt2TaxAmtVar { get; set; }
            public decimal Rpt3TaxAmtVar { get; set; }
            public string Company { get; set; }
            public int VendorNum { get; set; }
            public string InvoiceNum { get; set; }
            public int CollectionType { get; set; }
            public string TaxCode { get; set; }
            public int ECAcquisitionSeq { get; set; }
        }

        static Func<ErpContext, string, int, int, bool, bool, bool, bool, DateTime, IEnumerable<APInvTaxExpression10ColumnResult>> selectSumAllAPInvTaxQuery;
        private IEnumerable<APInvTaxExpression10ColumnResult> SelectSumAllAPInvTax(string company, int VendorNum, int ECAcquisitionSeq, bool Posted, bool all, bool open, bool inRange, DateTime fromDay)
        {
            if (selectSumAllAPInvTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, bool, bool, bool, bool, DateTime, IEnumerable<APInvTaxExpression10ColumnResult>>> expression =
                (ctx, company_ex, VendorNum_ex, ECAcquisitionSeq_ex, Posted_ex, all_ex, open_ex, inRange_ex, fromDay_ex) =>
                  (from row in ctx.APInvTax
                   join APInvHed_row in ctx.APInvHed
                   on new { row.Company, row.InvoiceNum } equals new { APInvHed_row.Company, APInvHed_row.InvoiceNum }
                   where row.Company == company_ex &&
                   row.VendorNum == VendorNum_ex &&
                  (row.ECAcquisitionSeq == ECAcquisitionSeq_ex || row.ECAcquisitionSeq == 2) &&
                   APInvHed_row.Posted == Posted_ex &&
                   (all_ex || APInvHed_row.OpenPayable == open_ex) &&
                   (!inRange_ex || DateTime.Compare(fromDay_ex, (DateTime)APInvHed_row.InvoiceDate) <= 0)
                   group row by new { row.Company, row.VendorNum, row.InvoiceNum, row.CollectionType } into empg
                   select new APInvTaxExpression10ColumnResult()
                   {
                       TaxableAmt = empg.Sum(x => x.TaxableAmt),
                       Rpt1TaxableAmt = empg.Sum(x => x.Rpt1TaxableAmt),
                       Rpt2TaxableAmt = empg.Sum(x => x.Rpt2TaxableAmt),
                       Rpt3TaxableAmt = empg.Sum(x => x.Rpt3TaxableAmt),
                       DocTaxableAmt = empg.Sum(x => x.DocTaxableAmt),
                       ReportableAmt = empg.Sum(x => x.ReportableAmt),
                       Rpt1ReportableAmt = empg.Sum(x => x.Rpt1ReportableAmt),
                       Rpt2ReportableAmt = empg.Sum(x => x.Rpt2ReportableAmt),
                       Rpt3ReportableAmt = empg.Sum(x => x.Rpt3ReportableAmt),
                       DocReportableAmt = empg.Sum(x => x.DocReportableAmt),
                       TaxAmt = empg.Sum(x => x.TaxAmt),
                       Rpt1TaxAmt = empg.Sum(x => x.Rpt1TaxAmt),
                       Rpt2TaxAmt = empg.Sum(x => x.Rpt2TaxAmt),
                       Rpt3TaxAmt = empg.Sum(x => x.Rpt3TaxAmt),
                       DocTaxAmt = empg.Sum(x => x.DocTaxAmt),
                       TaxAmtVar = empg.Sum(x => x.TaxAmtVar),
                       DocTaxAmtVar = empg.Sum(x => x.DocTaxAmtVar),
                       Rpt1TaxAmtVar = empg.Sum(x => x.Rpt1TaxAmtVar),
                       DedTaxAmt = empg.Sum(x => x.DedTaxAmt),
                       Rpt1DedTaxAmt = empg.Sum(x => x.Rpt1DedTaxAmt),
                       Rpt2DedTaxAmt = empg.Sum(x => x.Rpt2DedTaxAmt),
                       Rpt3DedTaxAmt = empg.Sum(x => x.Rpt3DedTaxAmt),
                       DocDedTaxAmt = empg.Sum(x => x.DocDedTaxAmt),
                       Rpt2TaxAmtVar = empg.Sum(x => x.Rpt2TaxAmtVar),
                       Rpt3TaxAmtVar = empg.Sum(x => x.Rpt3TaxAmtVar),
                       Company = empg.Key.Company,
                       VendorNum = empg.Key.VendorNum,
                       InvoiceNum = empg.Key.InvoiceNum,
                       CollectionType = empg.Key.CollectionType,
                   });
                selectSumAllAPInvTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectSumAllAPInvTaxQuery(this.Db, company, VendorNum, ECAcquisitionSeq, Posted, all, open, inRange, fromDay);
        }

        static Func<ErpContext, string, int, string, int, IEnumerable<APInvTaxExpression10ColumnResult>> selectAPInvTaxQuery;
        private IEnumerable<APInvTaxExpression10ColumnResult> SelectAPInvTax(string company, int VendorNum, string InvoiceNum, int ECAcquisitionSeq)
        {
            if (selectAPInvTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvTaxExpression10ColumnResult>>> expression =
                (ctx, company_ex, VendorNum_ex, InvoiceNum_ex, ECAcquisitionSeq_ex) =>
                  (from row in ctx.APInvTax
                   where row.Company == company_ex &&
                   row.VendorNum == VendorNum_ex &&
                   row.InvoiceNum == InvoiceNum_ex &&
                   (row.ECAcquisitionSeq == ECAcquisitionSeq_ex || row.ECAcquisitionSeq == 2)
                   select new APInvTaxExpression10ColumnResult() { TaxableAmt = row.TaxableAmt, Rpt1TaxableAmt = row.Rpt1TaxableAmt, Rpt2TaxableAmt = row.Rpt2TaxableAmt, Rpt3TaxableAmt = row.Rpt3TaxableAmt, DocTaxableAmt = row.DocTaxableAmt, ReportableAmt = row.ReportableAmt, Rpt1ReportableAmt = row.Rpt1ReportableAmt, Rpt2ReportableAmt = row.Rpt2ReportableAmt, Rpt3ReportableAmt = row.Rpt3ReportableAmt, DocReportableAmt = row.DocReportableAmt, TaxAmt = row.TaxAmt, Rpt1TaxAmt = row.Rpt1TaxAmt, Rpt2TaxAmt = row.Rpt2TaxAmt, Rpt3TaxAmt = row.Rpt3TaxAmt, DocTaxAmt = row.DocTaxAmt, TaxAmtVar = row.TaxAmtVar, DocTaxAmtVar = row.DocTaxAmtVar, Rpt1TaxAmtVar = row.Rpt1TaxAmtVar, DedTaxAmt = row.DedTaxAmt, Rpt1DedTaxAmt = row.Rpt1DedTaxAmt, Rpt2DedTaxAmt = row.Rpt2DedTaxAmt, Rpt3DedTaxAmt = row.Rpt3DedTaxAmt, DocDedTaxAmt = row.DocDedTaxAmt, Rpt2TaxAmtVar = row.Rpt2TaxAmtVar, Rpt3TaxAmtVar = row.Rpt3TaxAmtVar, Company = row.Company, VendorNum = row.VendorNum, InvoiceNum = row.InvoiceNum, CollectionType = row.CollectionType, TaxCode = row.TaxCode, ECAcquisitionSeq = row.ECAcquisitionSeq });
                selectAPInvTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvTaxQuery(this.Db, company, VendorNum, InvoiceNum, ECAcquisitionSeq);
        }

        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvTaxExpression10ColumnResult>>> APInvTaxExpression10 =
      (ctx, Company, VendorNum, InvoiceNum, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.ECAcquisitionSeq == _ECAcquisitionSeq
         select new APInvTaxExpression10ColumnResult() { TaxableAmt = row.TaxableAmt, Rpt1TaxableAmt = row.Rpt1TaxableAmt, Rpt2TaxableAmt = row.Rpt2TaxableAmt, Rpt3TaxableAmt = row.Rpt3TaxableAmt, DocTaxableAmt = row.DocTaxableAmt, ReportableAmt = row.ReportableAmt, Rpt1ReportableAmt = row.Rpt1ReportableAmt, Rpt2ReportableAmt = row.Rpt2ReportableAmt, Rpt3ReportableAmt = row.Rpt3ReportableAmt, DocReportableAmt = row.DocReportableAmt, TaxAmt = row.TaxAmt, Rpt1TaxAmt = row.Rpt1TaxAmt, Rpt2TaxAmt = row.Rpt2TaxAmt, Rpt3TaxAmt = row.Rpt3TaxAmt, DocTaxAmt = row.DocTaxAmt, TaxAmtVar = row.TaxAmtVar, DocTaxAmtVar = row.DocTaxAmtVar, Rpt1TaxAmtVar = row.Rpt1TaxAmtVar, DedTaxAmt = row.DedTaxAmt, Rpt1DedTaxAmt = row.Rpt1DedTaxAmt, Rpt2DedTaxAmt = row.Rpt2DedTaxAmt, Rpt3DedTaxAmt = row.Rpt3DedTaxAmt, DocDedTaxAmt = row.DocDedTaxAmt, Rpt2TaxAmtVar = row.Rpt2TaxAmtVar, Rpt3TaxAmtVar = row.Rpt3TaxAmtVar, Company = row.Company, VendorNum = row.VendorNum, InvoiceNum = row.InvoiceNum, CollectionType = row.CollectionType, TaxCode = row.TaxCode, ECAcquisitionSeq = row.ECAcquisitionSeq });

        static Func<ErpContext, string, string, PurMisc> findFirstPurMiscQuery;
        private PurMisc FindFirstPurMisc(string Company, string MiscCode)
        {
            if (findFirstPurMiscQuery == null)
            {
                Expression<Func<ErpContext, string, string, PurMisc>> expression =
                (ctx, Company_ex, MiscCode_ex) =>
                    (from row in ctx.PurMisc
                     where row.Company == Company_ex &&
                     row.MiscCode == MiscCode_ex
                     select row).FirstOrDefault();
                findFirstPurMiscQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPurMiscQuery(this.Db, Company, MiscCode);
        }

        static Func<ErpContext, string, int, FSCallhd> findFirstFSCallhdExpression;
        private FSCallhd FindFirstFSCallhd(string Company, int CallNum)
        {
            if (findFirstFSCallhdExpression == null)
            {
                Expression<Func<ErpContext, string, int, FSCallhd>> expression =
                (ctx, Company_ex, CallNum_ex) =>
                (from row in ctx.FSCallhd
                 where row.Company == Company_ex &&
                 row.CallNum == CallNum_ex
                 select row).FirstOrDefault();
                findFirstFSCallhdExpression = DBExpressionCompiler.Compile(expression);
            }

            return findFirstFSCallhdExpression(this.Db, Company, CallNum);
        }

        static Func<ErpContext, string, string, string> findFirstGLBCOACodeQuery;
        private string FindFirstGLBCOACode(string company, string extCompanyID)
        {
            if (findFirstGLBCOACodeQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
      (ctx, company_ex, extCompanyID_ex) =>
        (from row in ctx.GLBCOA
         where row.Company == company_ex &&
         row.ExtCompanyID == extCompanyID_ex
         select row.COACode).FirstOrDefault();
                findFirstGLBCOACodeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstGLBCOACodeQuery(this.Db, company, extCompanyID);
        }

        static Expression<Func<ErpContext, string, string, GLBCOA>> GLBCOAExpression =
      (ctx, CompanyID, inExtCompanyID) =>
        (from row in ctx.GLBCOA
         where row.Company == CompanyID &&
         row.ExtCompanyID == inExtCompanyID
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, IEnumerable<APInvJob>>> APInvJobExpression2 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvJob
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row);



        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvTax>>> APInvTaxExpression11 =
      (ctx, Company, VendorNum, InvoiceNum, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.ECAcquisitionSeq == _ECAcquisitionSeq
         select row);


        static Expression<Func<ErpContext, string, string, int, PurMiscVend>> PurMiscVendExpression =
      (ctx, CompanyID, miscCode, vendorNum) =>
        (from row in ctx.PurMiscVend
         where row.Company == CompanyID &&
         row.MiscCode == miscCode &&
         row.VendorNum == vendorNum
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, string, int, bool>> JobAsmblExpression2 =
      (ctx, Company, JobNum, ProposedAssemblySeq) =>
        (from row in ctx.JobAsmbl
         where row.Company == Company &&
         row.JobNum == JobNum &&
         row.AssemblySeq == ProposedAssemblySeq
         select row).Any();


        static Expression<Func<ErpContext, string, string, GlbGLSyst>> GlbGLSystExpression =
      (ctx, Company, ProposedExtCompID) =>
        (from row in ctx.GlbGLSyst
         where row.Company == Company &&
         row.ExtCompanyID == ProposedExtCompID
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, string, DateTime?, DateTime?, FiscalPer>> FiscalPerExpression2 =
      (ctx, CompanyID, CompanyFiscalCalendarID, ProposedInvoiceDate, ProposedInvoiceDate2) =>
        (from row in ctx.FiscalPer
         where row.Company == CompanyID &&
         row.FiscalCalendarID == CompanyFiscalCalendarID &&
         row.StartDate.Value <= ProposedInvoiceDate.Value &&
         row.EndDate.Value >= ProposedInvoiceDate2.Value
         select row).FirstOrDefault();



        static Func<ErpContext, string, int, string, bool, bool, APInvHed> BAPInvHedExpression6;
        private APInvHed FindFirstAPInvHedDMByRef(string Company, int VendorNum, string InvoiceNum, bool isDM, bool isPosted)
        {
            if (BAPInvHedExpression6 == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, bool, APInvHed>> expression =
                (ctx, Company_ex, VendorNum_ex, InvoiceNum_ex, isDM_ex, isPosted_ex) =>
                (from row in ctx.APInvHed
                 where row.Company == Company_ex &&
                 row.VendorNum == VendorNum_ex &&
                 row.InvoiceNum == InvoiceNum_ex &&
                 row.DebitMemo == isDM_ex &&
                 row.Posted == isPosted_ex
                 select row).FirstOrDefault();
                BAPInvHedExpression6 = DBExpressionCompiler.Compile(expression);
            }

            return BAPInvHedExpression6(this.Db, Company, VendorNum, InvoiceNum, isDM, isPosted);
        }


        static Expression<Func<ErpContext, string, string, int, string, JobMtl>> JobMtlExpression4 =
      (ctx, CompanyID, JobNum, AssemblySeq, ProposedJobMisc) =>
        (from row in ctx.JobMtl
         where row.Company == CompanyID &&
         row.JobNum == JobNum &&
         row.AssemblySeq == AssemblySeq &&
         row.MiscCode == ProposedJobMisc
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, int, int, JobMtl>> JobMtlExpression5 =
      (ctx, CompanyID, JobNum, AssemblySeq, AddedJobMtlSeq) =>
        (from row in ctx.JobMtl.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.JobNum == JobNum &&
         row.AssemblySeq == AssemblySeq &&
         row.MtlSeq == AddedJobMtlSeq
         select row).FirstOrDefault();
        //HOLDLOCK



        static Expression<Func<ErpContext, string, string, bool, Project>> ProjectExpression =
      (ctx, CompanyID, propProject, _ActiveProject) =>
        (from row in ctx.Project
         where row.Company == CompanyID &&
         row.ProjectID == propProject &&
         row.ActiveProject == _ActiveProject
         select row).FirstOrDefault();




        static Expression<Func<ErpContext, string, int, bool, FSCallhd>> FSCallhdExpression3 =
      (ctx, Company, CallNum, _Invoiced) =>
        (from row in ctx.FSCallhd
         where row.Company == Company &&
         row.CallNum == CallNum &&
         row.Invoiced == _Invoiced
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, bool>> APLOCExpression4 =
      (ctx, CompanyID, ProposedLOCID) =>
        (from row in ctx.APLOC
         where row.Company == CompanyID &&
         row.LCID == ProposedLOCID
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, int, bool>> APLOCExpression5 =
      (ctx, CompanyID, ProposedLOCID, VendorNum) =>
        (from row in ctx.APLOC
         where row.Company == CompanyID &&
         row.LCID == ProposedLOCID &&
         row.VendorNum == VendorNum
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, int, string, bool>> APLOCExpression6 =
      (ctx, CompanyID, ProposedLOCID, VendorNum, CurrencyCode) =>
        (from row in ctx.APLOC
         where row.Company == CompanyID &&
         row.LCID == ProposedLOCID &&
         row.VendorNum == VendorNum &&
         row.CurrencyCode == CurrencyCode
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, APLOC>> APLOCExpression7 =
      (ctx, CompanyID, ProposedLOCID) =>
        (from row in ctx.APLOC
         where row.Company == CompanyID &&
         row.LCID == ProposedLOCID
         select row).FirstOrDefault();


        private static Func<ErpContext, string, string, APLOC> findFirstAPLOCQuery;
        private APLOC FindFirstAPLOC(string Company, string ApLOCID)
        {
            if (findFirstAPLOCQuery == null)
            {
                Expression<Func<ErpContext, string, string, APLOC>> expression =
                    (context, Company_ex, ApLOCID_ex) =>
                    (from row in context.APLOC
                     where row.Company == Company_ex &&
                     row.LCID == ApLOCID_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPLOCQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPLOCQuery(this.Db, Company, ApLOCID);
        }


        class PurTermDPartial
        {
            public decimal DiscountPercent { get; set; }
        }
        static Func<ErpContext, string, string, PurTermDPartial> findFirstPurTermDPartialQuery;
        private PurTermDPartial FindFirstPurTermDPartial(string company, string termsCode)
        {
            if (findFirstPurTermDPartialQuery == null)
            {
                Expression<Func<ErpContext, string, string, PurTermDPartial>> expression =
               (ctx, companyID_ex, termsCode_ex) =>
                                            (from row in ctx.PurTermD
                                             where row.Company == companyID_ex &&
                                                   row.TermsCode == termsCode_ex
                                             orderby row.Company descending, row.TermsCode descending, row.DiscountPercent descending
                                             select new PurTermDPartial
                                             {
                                                 DiscountPercent = row.DiscountPercent
                                             }).FirstOrDefault();
                findFirstPurTermDPartialQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPurTermDPartialQuery(this.Db, company, termsCode);
        }


        static Expression<Func<ErpContext, string, string, SalesTax>> SalesTaxExpression6 =
      (ctx, CompanyID, TaxCode) =>
        (from row in ctx.SalesTax
         where row.Company == CompanyID &&
         row.TaxCode == TaxCode
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, string, TaxRate>> TaxRateExpression5 =
      (ctx, CompanyID, TaxCode, RateCode) =>
        (from row in ctx.TaxRate
         where row.Company == CompanyID &&
         row.TaxCode == TaxCode &&
         row.RateCode == RateCode
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, APInvDtl>> AltApInvDtlExpression =
      (ctx, CompanyID, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvDtl
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, bool, bool>> PurMiscExpression7 =
      (ctx, Company, MiscCode, _LCFlag) =>
        (from row in ctx.PurMisc
         where row.Company == Company &&
         row.MiscCode == MiscCode &&
         row.LCFlag == _LCFlag
         select row).Any();



        static Expression<Func<ErpContext, string, string, bool, bool>> PurMiscExpression8 =
      (ctx, Company, MiscCode, _LCFlag) =>
        (from row in ctx.PurMisc
         where row.Company == Company &&
         row.MiscCode == MiscCode &&
         row.LCFlag == _LCFlag
         select row).Any();



        static Expression<Func<ErpContext, string, string, int, int, bool, bool>> JobMtlExpression6 =
      (ctx, Company, JobNum, AssemblySeq, ProposedMtlSeq, _MiscCharge) =>
        (from row in ctx.JobMtl
         where row.Company == Company &&
         row.JobNum == JobNum &&
         row.AssemblySeq == AssemblySeq &&
         row.MtlSeq == ProposedMtlSeq &&
         row.MiscCharge == _MiscCharge
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, bool, bool>> ExtCompanyExpression3 =
      (ctx, Company, _ExtSystemID, _AllowAPAlloc) =>
        (from row in ctx.ExtCompany
         where row.Company == Company &&
         row.ExtSystemID == _ExtSystemID &&
         row.AllowAPAlloc == _AllowAPAlloc
         select row).Any();



        static Expression<Func<ErpContext, string, bool>> GlbGLSystExpression2 =
      (ctx, Company) =>
        (from row in ctx.GlbGLSyst
         where row.Company == Company
         select row).Any();



        static Expression<Func<ErpContext, string, int, string, VendPart>> VendPartExpression =
      (ctx, Company, VendorNum, PartNum) =>
        (from row in ctx.VendPart
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PartNum == PartNum &&
         row.OpCode == ""
         orderby row.Company, row.VendorNum, row.PartNum, row.OpCode, row.EffectiveDate
         select row).LastOrDefault();


        static Expression<Func<ErpContext, string, int, PayMethod>> PayMethodExpression =
      (ctx, CompanyID, PMUID) =>
        (from row in ctx.PayMethod
         where row.Company == CompanyID &&
         row.PMUID == PMUID
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, int, bool, PORel>> PORelExpression6 =
      (ctx, Company, PONUM, POLine, _OpenRelease) =>
        (from row in ctx.PORel
         where row.Company == Company &&
         row.PONum == PONUM &&
         row.POLine == POLine &&
         row.OpenRelease == _OpenRelease
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, int, string, POHeader>> POHeaderExpression6 =
      (ctx, Company, VendorNum, ProposedPONum, _Company) =>
        (from row in ctx.POHeader
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PONum == ProposedPONum &&

    (from row2 in ctx.PODetail
     where row2.Company == _Company &&
     row2.PONUM == row.PONum
     select row2).Any()
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, int, APInvPB>> APInvPBExpression2 =
      (ctx, CompanyID, InvoiceNum, InvoiceLine) =>
        (from row in ctx.APInvPB
         where row.Company == CompanyID &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, int, int, bool, PORel>> PORelExpression7 =
      (ctx, Company, PONum, POLine, ProposedPORelNum, _OpenRelease) =>
        (from row in ctx.PORel
         where row.Company == Company &&
         row.PONum == PONum &&
         row.POLine == POLine &&
         row.PORelNum == ProposedPORelNum &&
         row.OpenRelease == _OpenRelease
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, bool, bool, bool, APInvHed>> AltAPInvHedExpression4 =
      (ctx, CompanyID, VendorNum, prepaymentNum, _PrePayment, _Posted, _OpenPayable) =>
        (from row in ctx.APInvHed
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == prepaymentNum &&
         row.PrePayment == _PrePayment &&
         row.Posted == _Posted &&
         row.OpenPayable == _OpenPayable
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, string, string, int, int, APInvTax>> BApInvTaxExpression =
      (ctx, CompanyID, TaxCode, InvoiceNum, VendorNum, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == CompanyID &&
         row.TaxCode == TaxCode &&
         row.InvoiceNum == InvoiceNum &&
         row.VendorNum == VendorNum &&
         row.ECAcquisitionSeq == _ECAcquisitionSeq
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, bool, SalesTRC>> SalesTRCExpression3 =
      (ctx, Company, TaxCode, _DefaultRate) =>
        (from row in ctx.SalesTRC
         where row.Company == Company &&
         row.TaxCode == TaxCode &&
         row.DefaultRate == _DefaultRate
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, string, APTran>> APTranExpression2 =
      (ctx, CompanyID, vHeadNum, InvoiceRef, _TranType) =>
        (from row in ctx.APTran
         where row.Company == CompanyID &&
         row.HeadNum == vHeadNum &&
         row.InvoiceNum == InvoiceRef &&
         row.TranType == _TranType
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, LegalNumCnfg>> LegalNumCnfgExpression2 =
      (ctx, CompanyID, _LegalNumberType) =>
        (from row in ctx.LegalNumCnfg
         where row.Company == CompanyID &&
         row.LegalNumberType == _LegalNumberType
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, bool, APInvHed>> APInvHedExpression56 =
      (ctx, CompanyID, cGroupID, _DebitMemo) =>
        (from row in ctx.APInvHed
         where row.Company == CompanyID &&
         row.GroupID == cGroupID &&
         row.DebitMemo == _DebitMemo &&
         row.LegalNumber == ""
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, int, string, IEnumerable<APInvPB>>> APInvPBExpression3 =
      (ctx, CompanyID, ipVendNum, ipInvNum, ipInvLine, ipRoleCd) =>
        (from row in ctx.APInvPB
         where row.Company == CompanyID &&
         row.VendorNum == ipVendNum &&
         row.InvoiceNum == ipInvNum &&
         row.InvoiceLine == ipInvLine &&
         row.RoleCd != ipRoleCd
         select row);



        static Expression<Func<ErpContext, string, int, PayMethod>> PayMethodExpression2 =
      (ctx, Company, ipPaymentMethod) =>
        (from row in ctx.PayMethod
         where row.Company == Company &&
         row.PMUID == ipPaymentMethod
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, EFTHead>> EFTHeadExpression =
      (ctx, CompanyID, EFTHeadUID) =>
        (from row in ctx.EFTHead
         where row.Company == CompanyID &&
         row.EFTHeadUID == EFTHeadUID
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, PurTermD>> PurTermDExpression2 =
      (ctx, CompanyID, TermsCode) =>
        (from row in ctx.PurTermD
         where row.Company == CompanyID &&
         row.TermsCode == TermsCode
         orderby row.Company descending, row.TermsCode descending, row.DiscountPercent descending
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, APInvDtl>> AltApInvDtlExpression2 =
      (ctx, CompanyID, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvDtl
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, string>> APInvHedExpression58 =
      (ctx, CompanyID, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvHed
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row.SEBankRef).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, Vendor>> BVendorExpression =
      (ctx, CompanyID, VendorNum) =>
        (from row in ctx.Vendor
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, bool, bool>> APInvHedExpression59 =
      (ctx, Company, VendorNum, InvoiceNum, _CPayLinked) =>
        (from row in ctx.APInvHed
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.CPayLinked == _CPayLinked
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, int, EFTHead>> EFTHeadExpression2 =
      (ctx, CompanyID, EFTHeadUID) =>
        (from row in ctx.EFTHead
         where row.Company == CompanyID &&
         row.EFTHeadUID == EFTHeadUID
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, string, string, IEnumerable<EntityGLC>>> BufEntityGLCExpression =
      (ctx, CompanyID, vendorNum, sourceInvoiceNum, _RelatedToFile) =>
        (from row in ctx.EntityGLC
         where row.Company == CompanyID &&
         row.Key1 == vendorNum &&
         row.Key2 == sourceInvoiceNum &&
         row.RelatedToFile == _RelatedToFile
         select row);



        static Expression<Func<ErpContext, string, string, int, IEnumerable<APInvExp>>> BufDestAPInvExpExpression =
      (ctx, CompanyID, destInvoiceNum, destLineNum) =>
        (from row in ctx.APInvExp.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.InvoiceNum == destInvoiceNum &&
         row.InvoiceLine == destLineNum
         select row);
        //HOLDLOCK

        private static Func<ErpContext, string, int, string, int, IEnumerable<APInvExp>> findAPInvExp;
        private IEnumerable<APInvExp> FindAPInvExp(string company, int vendorNum, string invoiceNum, int invoiceLine)
        {
            if (findAPInvExp == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvExp>>> expression =
                    (ctx, company_ex, vendorNum_ex, invoiceNum_ex, invoiceLine_ex) =>
                    (from row in ctx.APInvExp
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex &&
                     row.InvoiceLine == invoiceLine_ex &&
                     !row.NonDedTax
                     select row);
                findAPInvExp = DBExpressionCompiler.Compile(expression);
            }
            return findAPInvExp(Db, company, vendorNum, invoiceNum, invoiceLine);
        }

        static Expression<Func<ErpContext, string, string, string, string, string, string, string, IEnumerable<TranGLC>>> BufSrcTranGLCExpression =
      (ctx, CompanyID, _RelatedToFile, vendorNum, sourceInvoiceNum, sourceLineNum, InvExpSeq, sourceGroup) =>
        (from row in ctx.TranGLC
         where row.Company == CompanyID &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == vendorNum &&
         row.Key2 == sourceInvoiceNum &&
         row.Key3 == sourceLineNum &&
         row.Key4 == InvExpSeq &&
         row.Key5 == sourceGroup &&
         row.Key6 == ""
         select row);



        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvJob>>> BufAPInvJobExpression =
      (ctx, CompanyID, vendorNum, sourceInvoiceNum, sourceLineNum) =>
        (from row in ctx.APInvJob
         where row.Company == CompanyID &&
         row.VendorNum == vendorNum &&
         row.InvoiceNum == sourceInvoiceNum &&
         row.InvoiceLine == sourceLineNum
         select row);



        static Func<ErpContext, string, string, int, bool, IEnumerable<APInvDtl>> SelectAPInvDtlQuery;
        private IEnumerable<APInvDtl> SelectAPInvDtl(string companyID, string sourceInvoiceNum, int vendorNum, bool correctionDtl)
        {
            if (SelectAPInvDtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, bool, IEnumerable<APInvDtl>>> expression =
                (ctx, company_ex, sourceInvoiceNum_ex, vendorNum_ex, correctionDtl_ex) =>
                                                                    (from row in ctx.APInvDtl
                                                                     where row.Company == company_ex &&
                                                                           row.InvoiceNum == sourceInvoiceNum_ex &&
                                                                           row.VendorNum == vendorNum_ex &&
                                                                           (row.CorrectionDtl == correctionDtl_ex)
                                                                     select row);
                SelectAPInvDtlQuery = DBExpressionCompiler.Compile(expression);
            }
            return SelectAPInvDtlQuery(this.Db, companyID, sourceInvoiceNum, vendorNum, correctionDtl);

        }


        private class SelectAPInvDetail
        {
            public string Company { get; set; }
            public string InvoiceNum { get; set; }
            public int VendorNum { get; set; }
            public string LineType { get; set; }
            public int PONum { get; set; }
            public int POLine { get; set; }
            public int PORelNum { get; set; }
            public string PackSlip { get; set; }
            public int PackLine { get; set; }
            public bool FinaInvoice { get; set; }
        }

        static Func<ErpContext, string, string, int, string, bool, IEnumerable<SelectAPInvDetail>> selectAPInvoiceDtlQuery;
        private IEnumerable<SelectAPInvDetail> SelectAPInvoiceDtl(string company, string invoiceNum, int vendorNum, string lineType, bool finalInvoice)
        {
            if (selectAPInvoiceDtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, bool, IEnumerable<SelectAPInvDetail>>> expression =
                    (ctx, company_ex, InvoiceNum_ex, VendorNum_ex, LineType_ex, finalInvoice_ex) =>
                    (from row in ctx.APInvDtl
                     where row.Company == company_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.LineType == LineType_ex &&
                     row.FinalInvoice == finalInvoice_ex
                     select new SelectAPInvDetail() { Company = row.Company, InvoiceNum = row.InvoiceNum, VendorNum = row.VendorNum, LineType = row.LineType, PONum = row.PONum, POLine = row.POLine, PORelNum = row.PORelNum, PackSlip = row.PackSlip, PackLine = row.PackLine, FinaInvoice = row.FinalInvoice });
                selectAPInvoiceDtlQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectAPInvoiceDtlQuery(this.Db, company, invoiceNum, vendorNum, lineType, finalInvoice);
        }

        static Expression<Func<ErpContext, string, string, int, int, IEnumerable<APInvTax>>> BAPInvTaxExpression2 =
      (ctx, CompanyID, sourceInvoiceNum, vendorNum, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == CompanyID &&
         row.InvoiceNum == sourceInvoiceNum &&
         row.VendorNum == vendorNum &&
         row.ECAcquisitionSeq == _ECAcquisitionSeq
         select row);


        static Expression<Func<ErpContext, string, string, int, APInvHed>> BAPInvHedExpression11 =
      (ctx, CompanyID, sourceInvoiceNum, sourceVendorNum) =>
        (from row in ctx.APInvHed
         where row.Company == CompanyID &&
         row.InvoiceNum == sourceInvoiceNum &&
         row.VendorNum == sourceVendorNum
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, int, APInvHed>> AltAPInvHedExpression5 =
      (ctx, CompanyID, sourceInvoiceNum, sourceVendorNum) =>
        (from row in ctx.APInvHed
         where row.Company == CompanyID &&
         row.InvoiceRef == sourceInvoiceNum &&
         row.VendorNum == sourceVendorNum &&
         row.CorrectionInv == true
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, string, int, JobMtl>> AltJobMtlExpression =
      (ctx, CompanyID, JobNum, _AssemblySeq) =>
        (from row in ctx.JobMtl
         where row.Company == CompanyID &&
         row.JobNum == JobNum &&
         row.AssemblySeq == _AssemblySeq
         orderby row.Company, row.JobNum, row.AssemblySeq, row.MtlSeq descending
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, int, int, IEnumerable<POMisc>>> POMiscExpression3 =
      (ctx, Company, REFPONum, _POLine) =>
        (from row in ctx.POMisc
         where row.Company == Company &&
         row.PONum == REFPONum &&
         row.POLine == _POLine
         select row);



        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvDtl>>> APInvDtlExpression24 =
      (ctx, Company, VendorNum, InvoiceNum, _PONum) =>
        (from row in ctx.APInvDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.PONum > _PONum
         select row);



        static Expression<Func<ErpContext, string, int, IEnumerable<POMisc>>> POMiscExpression4 =
      (ctx, Company, PONum) =>
        (from row in ctx.POMisc
         where row.Company == Company &&
         row.PONum == PONum
         select row);



        static Expression<Func<ErpContext, string, int, bool, bool, IEnumerable<DropShipHead>>> DropShipHeadExpression =
      (ctx, Company, VendorNum, _APInvoiced, _ReceivedShipped) =>
        (from row in ctx.DropShipHead
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.APInvoiced == _APInvoiced &&
         row.ReceivedShipped == _ReceivedShipped
         select row);



        static Expression<Func<ErpContext, string, int, string, string, int, bool>> DropShipDtlExpression =
      (ctx, CompanyID, VendorNum, PurPoint, PackSlip, InPONum) =>
        (from row in ctx.DropShipDtl
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == PackSlip &&
         row.PONum == InPONum
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, int, string, string, bool, bool>> DropShipDtlExpression2 =
      (ctx, Company, VendorNum, PurPoint, PackSlip, _APInvoiced) =>
        (from row in ctx.DropShipDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == PackSlip &&
         row.APInvoiced == _APInvoiced
         select row).Any();



        static Expression<Func<ErpContext, string, int, string, string, bool, IEnumerable<DropShipDtl>>> DropShipDtlExpression3 =
      (ctx, Company, VendorNum, PurPoint, PackSlip, _APInvoiced) =>
        (from row in ctx.DropShipDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == PackSlip &&
         row.APInvoiced == _APInvoiced
         select row);


        static Expression<Func<ErpContext, string, int, string, string, int, bool>> RcvDtlExpression3 =
      (ctx, CompanyID, VendorNum, PurPoint, PackSlip, InPONum) =>
        (from row in ctx.RcvDtl
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == PackSlip &&
         row.PONum == InPONum
         select row).Any();



        static Expression<Func<ErpContext, string, int, string, string, bool, string, string, bool, bool, bool>> RcvDtlExpression4 =
      (ctx, Company, VendorNum, PurPoint, PackSlip, _Received, _POType, _POType2, _AutoReceipt, _Invoiced) =>
        (from row in ctx.RcvDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == PackSlip &&
         row.Received == _Received &&
        ((row.POType == _POType) ||
        (row.POType == _POType2 &&
         row.AutoReceipt == _AutoReceipt)) &&
         ((row.Invoiced == _Invoiced) || (row.Invoiced == true && row.SupplierUnInvcReceiptQty != 0))
         select row).Any();



        static Expression<Func<ErpContext, string, int, string, string, string, string, bool, bool, bool, IEnumerable<RcvDtl>>> RcvDtlExpression5 =
      (ctx, Company, VendorNum, PurPoint, PackSlip, _POType, _POType2, _AutoReceipt, _Invoiced, _Received) =>
        (from row in ctx.RcvDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == PackSlip &&
        ((row.POType == _POType) ||
        (row.POType == _POType2 &&
         row.AutoReceipt == _AutoReceipt)) &&
         ((row.Invoiced == _Invoiced) || (row.Invoiced == true && row.SupplierUnInvcReceiptQty != 0)) &&
         row.Received == _Received
         select row);



        static Expression<Func<ErpContext, string, bool, string, string, IEnumerable<TranDocType>>> TranDocTypeExpression6 =
      (ctx, CompanyID, _Inactive, SysTranType, Delim) =>
        (from row in ctx.TranDocType
         where row.Company == CompanyID &&
         row.Inactive == _Inactive &&
         (Delim + SysTranType + Delim).Contains(Delim + row.SystemTranID + Delim)
         select row);



        static Expression<Func<ErpContext, string, string, Vendor>> VendorExpression14 =
      (ctx, CompanyID, vendorID) =>
        (from row in ctx.Vendor
         where row.Company == CompanyID &&
         row.VendorID == vendorID
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, bool, IEnumerable<APInvHed>>> APInvHedExpression64 =
      (ctx, CompanyID, VendorNum, _Posted) =>
        (from row in ctx.APInvHed
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.Posted == _Posted
         select row);


        private class CurrencyPartialRow : Epicor.Data.TempRowBase
        {
            public bool BaseCurr { get; set; }
            public string CurrSymbol { get; set; }
            public string CurrencyID { get; set; }
            public string CurrencyCode { get; set; }
        }

        private static Func<ErpContext, string, string, CurrencyPartialRow> selectCurrencyQuery;
        private CurrencyPartialRow SelectCurrency(string Company, string CurrencyCode)
        {
            if (selectCurrencyQuery == null)
            {
                Expression<Func<ErpContext, string, string, CurrencyPartialRow>> expression =
                    (context, Company_ex, CurrencyCode_ex) =>
                    (from row in context.Currency
                     where row.Company == Company_ex &&
                     row.CurrencyCode == CurrencyCode_ex
                     select new CurrencyPartialRow()
                     {
                         BaseCurr = row.BaseCurr,
                         CurrSymbol = row.CurrSymbol,
                         CurrencyID = row.CurrencyID,
                         CurrencyCode = row.CurrencyCode,
                     }).FirstOrDefault();

                selectCurrencyQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectCurrencyQuery(this.Db, Company, CurrencyCode);
        }


        private static Func<ErpContext, string, bool, Currency> findFirstBaseCurrencyQuery;
        private Currency FindFirstBaseCurrency(string Company, bool isBaseCurr)
        {
            if (findFirstBaseCurrencyQuery == null)
            {
                Expression<Func<ErpContext, string, bool, Currency>> expression =
                    (context, Company_ex, isBaseCurr_ex) =>
                    (from row in context.Currency
                     where row.Company == Company_ex &&
                     row.BaseCurr == isBaseCurr_ex
                     select row).FirstOrDefault();

                findFirstBaseCurrencyQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstBaseCurrencyQuery(this.Db, Company, isBaseCurr);
        }

        private static Func<ErpContext, string, bool, string> findFirstBaseCurrencyQuery2;
        private string FindFirstBaseCurrencyCode(string ipCompany, bool ipBaseCurr)
        {
            if (findFirstBaseCurrencyQuery2 == null)
            {
                Expression<Func<ErpContext, string, bool, string>> expression =
                    (context, ipCompany_ex, ipBaseCurr_ex) =>
                    (from row in context.Currency
                     where row.Company == ipCompany_ex &&
                            row.BaseCurr == ipBaseCurr_ex
                     select row.CurrencyCode).FirstOrDefault();
                findFirstBaseCurrencyQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstBaseCurrencyQuery2(this.Db, ipCompany, ipBaseCurr);
        }

        private static Func<ErpContext, string, bool, string> selectBaseCurrencyCodeQuery;
        private string SelectBaseCurrencyCode(string Company, bool isBaseCurr)
        {
            if (selectBaseCurrencyCodeQuery == null)
            {
                Expression<Func<ErpContext, string, bool, string>> expression =
                    (context, Company_ex, isBaseCurr_ex) =>
                    (from row in context.Currency
                     where row.Company == Company_ex &&
                     row.BaseCurr == isBaseCurr_ex
                     select row.CurrencyCode).FirstOrDefault();

                selectBaseCurrencyCodeQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectBaseCurrencyCodeQuery(this.Db, Company, isBaseCurr);
        }

        private static Func<ErpContext, string, bool, string> selectBaseCurrencySymbolQuery;
        private string SelectBaseCurrencySymbol(string Company, bool isBaseCurr)
        {
            if (selectBaseCurrencySymbolQuery == null)
            {
                Expression<Func<ErpContext, string, bool, string>> expression =
                    (context, Company_ex, isBaseCurr_ex) =>
                    (from row in context.Currency
                     where row.Company == Company_ex &&
                     row.BaseCurr == isBaseCurr_ex
                     select row.CurrSymbol).FirstOrDefault();

                selectBaseCurrencySymbolQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectBaseCurrencySymbolQuery(this.Db, Company, isBaseCurr);
        }

        private static Func<ErpContext, string, bool, string> selectBaseCurrencyIDQuery;
        private string SelectBaseCurrencyID(string Company, bool isBaseCurr)
        {
            if (selectBaseCurrencyIDQuery == null)
            {
                Expression<Func<ErpContext, string, bool, string>> expression =
                    (context, Company_ex, isBaseCurr_ex) =>
                    (from row in context.Currency
                     where row.Company == Company_ex &&
                     row.BaseCurr == isBaseCurr_ex
                     select row.CurrencyID).FirstOrDefault();

                selectBaseCurrencyIDQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectBaseCurrencyIDQuery(this.Db, Company, isBaseCurr);
        }


        static Expression<Func<ErpContext, string, string, int, DMRActn>> DMRActnExpression2 =
      (ctx, CompanyID, _ActionType, _DebitMemoLine) =>
        (from row in ctx.DMRActn
         where row.Company == CompanyID &&
         row.ActionType == _ActionType &&
         row.DebitMemoNum == "" &&
         row.DebitMemoLine == _DebitMemoLine
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, int, IEnumerable<DMRActn>>> DMRActnExpression3 =
      (ctx, CompanyID, _ActionType, _DebitMemoLine) =>
        (from row in ctx.DMRActn.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.ActionType == _ActionType &&
         row.DebitMemoNum == "" &&
         row.DebitMemoLine == _DebitMemoLine
         orderby row.Company, row.ActionType, row.DMRNum, row.ActionDate, row.ActionNum
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, int, DMRHead>> BDMRHeadExpression =
      (ctx, CompanyID, DMRNum, _VendorNum) =>
        (from row in ctx.DMRHead
         where row.Company == CompanyID &&
         row.DMRNum == DMRNum &&
         row.VendorNum > _VendorNum
         select row).FirstOrDefault();

        static Expression<Func<ErpContext, string, string, int, IEnumerable<APInvHed>>> APInvHedExpression92 =
     (ctx, companyId, groupId, vendorId) =>
       (from row in ctx.APInvHed
        where row.Company == companyId &&
        row.GroupID == groupId &&
        row.VendorNum == vendorId &&
        row.Posted == false
        select row);



        static Expression<Func<ErpContext, string, string, int, DMRActn>> DMRActnExpression4 =
      (ctx, CompanyID, _ActionType, _DebitMemoLine) =>
        (from row in ctx.DMRActn
         where row.Company == CompanyID &&
         row.ActionType == _ActionType &&
         row.DebitMemoNum == "" &&
         row.DebitMemoLine == _DebitMemoLine
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, DateTime?, DateTime?, FiscalPer>> FiscalPerExpression3 =
      (ctx, CompanyID, CompanyFiscalCalendarID, Now, Now2) =>
        (from row in ctx.FiscalPer
         where row.Company == CompanyID &&
         row.FiscalCalendarID == CompanyFiscalCalendarID &&
         row.StartDate.Value <= Now.Value &&
         row.EndDate.Value >= Now2.Value
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, string, bool, APInvHed>> APInvHedExpression67 =
      (ctx, CompanyID, inVendor, inInvNum, inGroupID, _DebitMemo) =>
        (from row in ctx.APInvHed
         where row.Company == CompanyID &&
         row.VendorNum == inVendor &&
         row.InvoiceNum == inInvNum &&
         row.GroupID == inGroupID
         //row.DebitMemo == _DebitMemo
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, bool, IEnumerable<APInvHed>>> APInvHedExpression68 =
      (ctx, CompanyID, cGroupID, _DebitMemo) =>
        (from row in ctx.APInvHed.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.GroupID == cGroupID &&
         row.DebitMemo == _DebitMemo &&
         row.LegalNumber == ""
         select row);
        //HOLDLOCK


        static Expression<Func<ErpContext, string, string, int, APInvHed>> APInvHedExpression70 =
      (ctx, CompanyID, ipInvoiceNum, ipVendorNum) =>
        (from row in ctx.APInvHed.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.InvoiceNum == ipInvoiceNum &&
         row.VendorNum == ipVendorNum
         select row).FirstOrDefault();
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, int, DMRActn>> DMRActnExpression5 =
      (ctx, CompanyID, DMRNum, DMRActionNum) =>
        (from row in ctx.DMRActn
         where row.Company == CompanyID &&
         row.DMRNum == DMRNum &&
         row.ActionNum == DMRActionNum
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, string, DropShipHead>> DropShipHeadExpression2 =
      (ctx, Company, VendorNum, PurPoint, DropShipPackSlip) =>
        (from row in ctx.DropShipHead
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == DropShipPackSlip
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, string, int, DropShipDtl>> DropShipDtlExpression4 =
      (ctx, Company, VendorNum, PurPoint, DropShipPackSlip, DropShipPackLine) =>
        (from row in ctx.DropShipDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == DropShipPackSlip &&
         row.PackLine == DropShipPackLine
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, int, string, string, RcvHead>> RcvHeadExpression2 =
      (ctx, Company, VendorNum, PurPoint, PackSlip) =>
        (from row in ctx.RcvHead
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == PackSlip
         select row).FirstOrDefault();

        static Expression<Func<ErpContext, string, int, IEnumerable<RcvDtl>>> RcvDtlExpression8 =
      (ctx, Company, PONum) =>
        (from row in ctx.RcvDtl
         where row.Company == Company &&
         row.PONum == PONum
         select row);


        class APInvMscExpression20ColumnResult
        {
            public decimal MiscAmt { get; set; }
            public decimal DocMiscAmt { get; set; }
            public decimal Rpt1MiscAmt { get; set; }
            public decimal Rpt2MiscAmt { get; set; }
            public decimal Rpt3MiscAmt { get; set; }
        }
        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvMscExpression20ColumnResult>>> APInvMscExpression20 =
      (ctx, Company, VendorNum, InvoiceNum, _InvoiceLine) =>
        (from row in ctx.APInvMsc
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == _InvoiceLine
         select new APInvMscExpression20ColumnResult() { MiscAmt = row.MiscAmt, DocMiscAmt = row.DocMiscAmt, Rpt1MiscAmt = row.Rpt1MiscAmt, Rpt2MiscAmt = row.Rpt2MiscAmt, Rpt3MiscAmt = row.Rpt3MiscAmt });



        class APInvExpExpression19ColumnResult
        {
            public decimal ExpAmt { get; set; }
            public decimal DocExpAmt { get; set; }
            public decimal Rpt1ExpAmt { get; set; }
            public decimal Rpt2ExpAmt { get; set; }
            public decimal Rpt3ExpAmt { get; set; }
        }
        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvExpExpression19ColumnResult>>> APInvExpExpression19 =
      (ctx, Company, VendorNum, InvoiceNum, _InvoiceLine) =>
        (from row in ctx.APInvExp
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == _InvoiceLine
         select new APInvExpExpression19ColumnResult() { ExpAmt = row.ExpAmt, DocExpAmt = row.DocExpAmt, Rpt1ExpAmt = row.Rpt1ExpAmt, Rpt2ExpAmt = row.Rpt2ExpAmt, Rpt3ExpAmt = row.Rpt3ExpAmt });



        static Expression<Func<ErpContext, string, string, string, DateTime, decimal, TaxGRate>> TaxGRateExpression2 =
      (ctx, CompanyID, TaxCode, RateCode, ipInvDate, TaxableAmt) =>
        (from row in ctx.TaxGRate
         where row.Company == CompanyID &&
         row.TaxCode == TaxCode &&
         row.RateCode == RateCode &&
         row.EffectiveFrom <= ipInvDate &&
         row.FromAmount >= TaxableAmt
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, bool, bool>> APInvHedExpression75 =
      (ctx, CompanyID, intVendorNum, txtInvoiceNum, _Posted) =>
        (from row in ctx.APInvHed
         where row.Company == CompanyID &&
         row.VendorNum == intVendorNum &&
         row.InvoiceNum == txtInvoiceNum &&
         row.Posted == _Posted
         select row).Any();



        static Expression<Func<ErpContext, string, string, bool>> TaxRgnExpression2 =
      (ctx, CompanyID, sReturnTaxRegionCode) =>
        (from row in ctx.TaxRgn
         where row.Company == CompanyID &&
         row.TaxRegionCode == sReturnTaxRegionCode
         select row).Any();



        static Expression<Func<ErpContext, string, int, IEnumerable<VendBank>>> VendBankExpression2 =
      (ctx, CompanyID, VendorNum) =>
        (from row in ctx.VendBank
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum
         select row);



        static Expression<Func<ErpContext, string, int, string, bool>> VendorExpression17 =
      (ctx, CompanyID, VendorNum, BankID) =>
        (from row in ctx.Vendor
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.PrimaryBankID == BankID
         select row).Count() == 1;

        static Expression<Func<ErpContext, Guid, APInvDtl>> APInvDtlExpression26 =
      (ctx, SysRowID) =>
        (from row in ctx.APInvDtl
         where row.SysRowID == SysRowID
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, bool>> APAlcHedExpression =
      (ctx, Company, AllocationID) =>
        (from row in ctx.APAlcHed
         where row.Company == Company &&
         row.AllocID == AllocationID
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, int, string, int, int, APInvExp>> APInvExpExpression21 =
      (ctx, Company, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq) =>
        (from row in ctx.APInvExp
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.InvExpSeq == InvExpSeq
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, string, string, string, string, string, string, string, TranGLC>> BTranGLCExpression =
      (ctx, Company, RelatedToFile, Key1, Key2, Key3, Key4, Key5, Key6, SysGLControlType) =>
        (from row in ctx.TranGLC
         where row.Company == Company &&
         row.RelatedToFile == RelatedToFile &&
         row.Key1 == Key1 &&
         row.Key2 == Key2 &&
         row.Key3 == Key3 &&
         row.Key4 == Key4 &&
         row.Key5 == Key5 &&
         row.Key6 == Key6 &&
         row.SysGLControlType == SysGLControlType
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, string, int, int, IEnumerable<LogAPInvTax>>> LogAPInvTaxExpression =
      (ctx, Company, InvoiceNum, VendorNum, _ECAcquisitionSeq) =>
        (from row in ctx.LogAPInvTax
         where row.Company == Company &&
         row.InvoiceNum == InvoiceNum &&
         row.VendorNum == VendorNum &&
         row.ECAcquisitionSeq != _ECAcquisitionSeq
         select row);



        static Expression<Func<ErpContext, string, string, string, string, IEnumerable<TranGLC>>> BufTranGLCExpression =
      (ctx, CompanyID, _RelatedToFile, VendorNum, InvoiceNum) =>
        (from row in ctx.TranGLC
         where row.Company == CompanyID &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum
         select row);



        static Expression<Func<ErpContext, string, string, DateTime?, DateTime?, FiscalPer>> FiscalPerExpression4 =
      (ctx, CompanyID, CompanyFiscalCalendarID, NewApplyDate, NewApplyDate2) =>
        (from row in ctx.FiscalPer
         where row.Company == CompanyID &&
         row.FiscalCalendarID == CompanyFiscalCalendarID &&
         row.StartDate.Value <= NewApplyDate.Value &&
         row.EndDate.Value >= NewApplyDate2.Value
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, string, TranDocType>> TranDocTypeExpression7 =
      (ctx, CompanyID, ipTranDocTypeID, SystemTranType) =>
        (from row in ctx.TranDocType
         where row.Company == CompanyID &&
         row.TranDocTypeID == ipTranDocTypeID &&
         row.SystemTranID == SystemTranType
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, int, ContainerHeader>> ContainerHeaderExpression =
      (ctx, CompanyID, ipShipmentID) =>
        (from row in ctx.ContainerHeader
         where row.Company == CompanyID &&
         row.ContainerID == ipShipmentID
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, string, string, string, EntityGLC>> EntityGLCExpression2 =
      (ctx, CompanyID, _RelatedToFile, VendorNum, InvoiceNum) =>
        (from row in ctx.EntityGLC
         where row.Company == CompanyID &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, int, int, APInvMsc>> BufAPInvMscExpression3 =
      (ctx, CompanyID, APInvVendorNum, InvoiceNum, InvoiceLine, MscNum) =>
        (from row in ctx.APInvMsc
         where row.Company == CompanyID &&
         row.VendorNum == APInvVendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.MscNum == MscNum
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, int, int, string, int, string, string, int, IEnumerable<RcvMisc>>> BufRcvMiscExpression4 =
      (ctx, CompanyID, APInvVendorNum, InvoiceNum, InvoiceLine, MscNum, Company, VendorNum, PurPoint, PackSlip, MiscSeq) =>
        (from row in ctx.RcvMisc
         where row.Company == CompanyID &&
         row.APInvVendorNum == APInvVendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.MscNum == MscNum &&
        !(row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == PackSlip &&
         row.MiscSeq == MiscSeq)
         select row);



        static Expression<Func<ErpContext, string, int, string, int, int, string, int, string, string, int, IEnumerable<RcvMisc>>> BufRcvMiscExpression5 =
      (ctx, CompanyID, APInvVendorNum, InvoiceNum, InvoiceLine, MscNum, Company, VendorNum, PurPoint, PackSlip, MiscSeq) =>
        (from row in ctx.RcvMisc.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.APInvVendorNum == APInvVendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == InvoiceLine &&
         row.MscNum == MscNum &&
        !(row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PurPoint == PurPoint &&
         row.PackSlip == PackSlip &&
         row.MiscSeq == MiscSeq)
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, string, int, APInvHed>> APInvHedExpression82 =
      (ctx, CompanyID, transInvoiceNum, transVendorNum) =>
        (from row in ctx.APInvHed.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.InvoiceNum == transInvoiceNum &&
         row.VendorNum == transVendorNum
         select row).FirstOrDefault();
        //HOLDLOCK


        static Expression<Func<ErpContext, string, int, string, IEnumerable<APInvTax>>> APInvTaxExpression12 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvTax.With(LockHint.UpdLock)
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, IEnumerable<APInvMsc>>> APInvMscExpression21 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvMsc.With(LockHint.UpdLock)
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, IEnumerable<APInvExp>>> APInvExpExpression22 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvExp.With(LockHint.UpdLock)
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row);
        //HOLDLOCK


        //RJM

        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvExp>>> APInvExpExpression24 =
      (ctx, ipCompany, ipVendorNum, ipInvoiceNum, ipInvoiceLine) =>
        (from row in ctx.APInvExp.With(LockHint.UpdLock)
         where row.Company == ipCompany &&
         row.VendorNum == ipVendorNum &&
         row.InvoiceNum == ipInvoiceNum &&
         row.InvoiceLine == ipInvoiceLine
         orderby Math.Abs(row.ExpAmt) descending
         select row);

        static Expression<Func<ErpContext, string, int, string, int, decimal>> SelectTotalExpAmt =
      (ctx, ipCompany, ipVendorNum, ipInvoiceNum, ipInvoiceLine) =>
        (from row in ctx.APInvExp
         where row.Company == ipCompany &&
         row.VendorNum == ipVendorNum &&
         row.InvoiceNum == ipInvoiceNum &&
         row.InvoiceLine == ipInvoiceLine
         select row.ExpAmt).Sum();



        static Expression<Func<ErpContext, string, string, string, string, string, string, bool, IEnumerable<TranGLC>>> TranGLCExpression4 =
      (ctx, Company, _RelatedToFile, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq, _UserCanModify) =>
        (from row in ctx.TranGLC
         where row.Company == Company &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum &&
         row.Key3 == InvoiceLine &&
         row.Key4 == InvExpSeq &&
         row.UserCanModify == _UserCanModify
         select row);



        static Expression<Func<ErpContext, string, int, RcvDtlXRef>> RcvDtlXRefExpression =
      (ctx, CompanyID, RcvXRefNum) =>
        (from row in ctx.RcvDtlXRef.With(LockHint.UpdLock)
         where row.Company == CompanyID &&
         row.RcvXRefNum == RcvXRefNum
         select row).FirstOrDefault();
        //HOLDLOCK


        static Expression<Func<ErpContext, string, string, IEnumerable<TaxRgnSalesTax>>> TaxRgnSalesTaxExpression2 =
      (ctx, Company, TaxRegionCode) =>
        (from row in ctx.TaxRgnSalesTax
         where row.Company == Company &&
         row.TaxRegionCode == TaxRegionCode
         select row);



        static Expression<Func<ErpContext, string, string, int, SalesTax>> SalesTaxExpression9 =
      (ctx, CompanyID, TaxCode, _CollectionType) =>
        (from row in ctx.SalesTax
         where row.Company == CompanyID &&
         row.TaxCode == TaxCode &&
         row.CollectionType == _CollectionType
         select row).FirstOrDefault();

        static Expression<Func<ErpContext, string, string, PurTermD>> PurTermDExpression3 =
      (ctx, Company, TermsCode) =>
        (from row in ctx.PurTermD
         where row.Company == Company &&
         row.TermsCode == TermsCode
         orderby row.Company descending, row.TermsCode descending, row.DiscountPercent descending
         select row).FirstOrDefault();



        class APInvDtlExpression28ColumnResult
        {
            public decimal TotalMiscChrg { get; set; }
            public decimal DocTotalMiscChrg { get; set; }
        }
        static Expression<Func<ErpContext, string, int, string, IEnumerable<APInvDtlExpression28ColumnResult>>> APInvDtlExpression28 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select new APInvDtlExpression28ColumnResult() { TotalMiscChrg = row.TotalMiscChrg, DocTotalMiscChrg = row.DocTotalMiscChrg });



        class APInvMscExpression22ColumnResult
        {
            public decimal MiscAmt { get; set; }
            public decimal DocMiscAmt { get; set; }
            public string MiscCode { get; set; }
            public decimal Rpt1MiscAmt { get; set; }
            public decimal Rpt2MiscAmt { get; set; }
            public decimal Rpt3MiscAmt { get; set; }
        }

        static Expression<Func<ErpContext, string, int, string, IEnumerable<APInvMscExpression22ColumnResult>>> APInvMscExpression22 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvMsc
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select new APInvMscExpression22ColumnResult()
         {
             MiscAmt = row.MiscAmt,
             DocMiscAmt = row.DocMiscAmt,
             MiscCode = row.MiscCode,
             Rpt1MiscAmt = row.Rpt1MiscAmt,
             Rpt2MiscAmt = row.Rpt2MiscAmt,
             Rpt3MiscAmt = row.Rpt3MiscAmt,
         });



        static Expression<Func<ErpContext, string, string, int, int, IEnumerable<APInvTax>>> APInvTaxExpression13 =
      (ctx, Company, InvoiceNum, _ECAcquisitionSeq, VendorNum) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.InvoiceNum == InvoiceNum &&
         row.ECAcquisitionSeq == _ECAcquisitionSeq &&
         row.VendorNum == VendorNum
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, string, int, int, bool>> SalesTaxExpression11 =
      (ctx, Company, TaxCode, _DiscountType, _DiscountType2) =>
        (from row in ctx.SalesTax
         where row.Company == Company &&
         row.TaxCode == TaxCode &&
        (row.DiscountType == _DiscountType ||
         row.DiscountType == _DiscountType2)
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, IEnumerable<PurTermD>>> PurTermDExpression4 =
      (ctx, Company, TermsCode) =>
        (from row in ctx.PurTermD
         where row.Company == Company &&
         row.TermsCode == TermsCode
         orderby row.Company descending, row.TermsCode descending, row.DiscountPercent descending
         select row);


        static Expression<Func<ErpContext, string, string, string, string, string, string, IEnumerable<CurrExChain>>> CurrExChainExpression5 =
      (ctx, _TableName, Company, VendorNum, InvoiceNum, changedFromCurr, changedTargetCurr) =>
        (from row in ctx.CurrExChain.With(LockHint.UpdLock)
         where row.TableName == _TableName &&
         row.Company == Company &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum &&
         row.FromCurrCode == changedFromCurr &&
         row.ToCurrCode == changedTargetCurr
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, string, string, string, CurrConvRule>> CurrConvRuleExpression2 =
      (ctx, CompanyID, RateGrpCode, TargetCurrCode, changedFromCurr) =>
        (from row in ctx.CurrConvRule
         where row.Company == CompanyID &&
         row.RateGrpCode == RateGrpCode &&
         row.SourceCurrCode == TargetCurrCode &&
         row.TargetCurrCode == changedFromCurr
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, string, string, CurrConvRule>> CurrConvRuleExpression3 =
      (ctx, CompanyID, TaxRateGrpCode, TargetCurrCode, changedFromCurr) =>
        (from row in ctx.CurrConvRule
         where row.Company == CompanyID &&
         row.RateGrpCode == TaxRateGrpCode &&
         row.SourceCurrCode == TargetCurrCode &&
         row.TargetCurrCode == changedFromCurr
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, string, string, string, string, string, IEnumerable<CurrExChain>>> CurrExChainExpression6 =
      (ctx, _TableName, Company, VendorNum, InvoiceNum, changedFromCurr, changedTargetCurr) =>
        (from row in ctx.CurrExChain.With(LockHint.UpdLock)
         where row.TableName == _TableName &&
         row.Company == Company &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum &&
         row.FromCurrCode == changedFromCurr &&
         row.ToCurrCode == changedTargetCurr
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, IEnumerable<APInvMsc>>> APInvMscExpression23 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvMsc.With(LockHint.UpdLock)
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, IEnumerable<APInvSel>>> APInvSelExpression =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvSel.With(LockHint.UpdLock)
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, int, string, IEnumerable<APInvTax>>> APInvTaxExpression14 =
      (ctx, Company, VendorNum, InvoiceNum) =>
        (from row in ctx.APInvTax.With(LockHint.UpdLock)
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum
         select row);
        //HOLDLOCK



        static Expression<Func<ErpContext, string, string, int, int, IEnumerable<APInvTax>>> APInvTaxExpression15 =
      (ctx, CompanyID, bInvoiceNum, bVendorNum, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == CompanyID &&
         row.InvoiceNum == bInvoiceNum &&
         row.VendorNum == bVendorNum &&
         (row.ECAcquisitionSeq == _ECAcquisitionSeq || row.ECAcquisitionSeq == 2)
         select row);


        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvTax>>> APInvTaxExpression16 =
      (ctx, CompanyID, bVendorNum, bInvoiceNum, _ECAcquisitionSeq) =>
        (from row in ctx.APInvTax
         where row.Company == CompanyID &&
         row.VendorNum == bVendorNum &&
         row.InvoiceNum == bInvoiceNum &&
         (row.ECAcquisitionSeq == _ECAcquisitionSeq || row.ECAcquisitionSeq == 2)
         select row);
        static Expression<Func<ErpContext, string, string, string, string, string, IEnumerable<Memo>>> BMemoExpression =
      (ctx, Company, OldGroupID, DumKey2, DumKey3, _RelatedToFile) =>
        (from row in ctx.Memo
         where row.Company == Company &&
         row.RelatedToSchemaName == "Erp" &&
         row.Key1 == OldGroupID &&
         row.Key2 == DumKey2 &&
         row.Key3 == DumKey3 &&
         row.RelatedToFile == _RelatedToFile
         select row);



        static Expression<Func<ErpContext, Guid, Memo>> BNewMemoExpression =
      (ctx, SysRowID) =>
        (from row in ctx.Memo.With(LockHint.UpdLock)
         where row.SysRowID == SysRowID
         select row).FirstOrDefault();
        //HOLDLOCK

        static Expression<Func<ErpContext, string, string, string, string, IEnumerable<TranGLC>>> BufTranGLCExpression2 =
      (ctx, CompanyID, _RelatedToFile, VendorNum, InvoiceNum) =>
        (from row in ctx.TranGLC
         where row.Company == CompanyID &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum
         select row);



        static Expression<Func<ErpContext, string, string, int, int, IEnumerable<LogAPInvTax>>> LogAPInvTaxExpression2 =
      (ctx, Company, InvoiceNum, VendorNum, _ECAcquisitionSeq) =>
        (from row in ctx.LogAPInvTax
         where row.Company == Company &&
         row.InvoiceNum == InvoiceNum &&
         row.VendorNum == VendorNum &&
         row.ECAcquisitionSeq != _ECAcquisitionSeq
         select row);



        static Expression<Func<ErpContext, string, string, bool, IEnumerable<APInvHed>>> APInvHedExpression86 =
      (ctx, CompanyID, cGroupID, _Posted) =>
        (from row in ctx.APInvHed
         where row.Company == CompanyID &&
         row.GroupID == cGroupID &&
         row.Posted == _Posted
         select row);



        static Expression<Func<ErpContext, Guid, APInvHed>> APInvHedExpression88 =
      (ctx, rAPInvHedRowid) =>
        (from row in ctx.APInvHed
         where row.SysRowID == rAPInvHedRowid
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, string, string, int, APInvTax>> AltAPInvTaxExpression =
      (ctx, Company, VendorNum, InvoiceNum, TaxCode, RateCode, ECAcquisitionSeq) =>
        (from row in ctx.APInvTax.With(LockHint.UpdLock)
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.TaxCode == TaxCode &&
         row.RateCode == RateCode &&
         row.ECAcquisitionSeq == ECAcquisitionSeq
         select row).FirstOrDefault();
        //HOLDLOCK



        static Expression<Func<ErpContext, string, string, Vendor>> VendorExpression21 =
      (ctx, CompanyID, txtVendorID) =>
        (from row in ctx.Vendor
         where row.Company == CompanyID &&
         row.VendorID == txtVendorID
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, LogAPInv>> LogAPInvExpression5 =
      (ctx, CompanyID, _intVendorNum, txtInvoiceNum) =>
        (from row in ctx.LogAPInv
         where row.Company == CompanyID &&
         row.VendorNum == _intVendorNum &&
         row.InvoiceNum == txtInvoiceNum
         select row).FirstOrDefault();

        private class RefAPInvHedPartialRow : Epicor.Data.TempRowBase
        {
            public bool CorrectionInv { get; set; }
            public bool Posted { get; set; }
            public bool DebitMemo { get; set; }
            public decimal VendorNum { get; set; }
            public string CurrencyCode { get; set; }
            public string RateGrpCode { get; set; }
            public string InvoiceNum { get; set; }
        }

        private static Func<ErpContext, string, int, string, RefAPInvHedPartialRow> findFirstRefAPInvHedQuery;

        private RefAPInvHedPartialRow FindFirstRefAPInvHed(string Company, int VendorNum, string InvoiceRef)
        {
            if (findFirstRefAPInvHedQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, RefAPInvHedPartialRow>> expression =
                (ctx, CompanyID, VendorNum_ex, InvoiceRef_ex) =>
                (from row in ctx.APInvHed
                 where row.Company == CompanyID &&
                row.VendorNum == VendorNum_ex &&
                row.InvoiceNum == InvoiceRef_ex
                 select new RefAPInvHedPartialRow
                 {
                     CorrectionInv = row.CorrectionInv,
                     Posted = row.Posted,
                     DebitMemo = row.DebitMemo,
                     VendorNum = row.VendorNum,
                     CurrencyCode = row.CurrencyCode,
                     RateGrpCode = row.RateGrpCode,
                     InvoiceNum = row.InvoiceNum
                 }).FirstOrDefault();
                findFirstRefAPInvHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstRefAPInvHedQuery(this.Db, Company, VendorNum, InvoiceRef);
        }


        static Expression<Func<ErpContext, string, string, bool>> PurMiscExpression12 =
      (ctx, CompanyID, cMiscCode) =>
        (from row in ctx.PurMisc
         where row.Company == CompanyID &&
         row.MiscCode == cMiscCode
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, string, TaxRate>> TaxRateExpression11 =
      (ctx, CompanyID, TaxCode, proposedRateCode) =>
        (from row in ctx.TaxRate
         where row.Company == CompanyID &&
         row.TaxCode == TaxCode &&
         row.RateCode == proposedRateCode
         select row).FirstOrDefault();



        static Expression<Func<ErpContext, string, int, string, string, string, Guid, bool>> APInvTaxExpression17 =
      (ctx, Company, VendorNum, InvoiceNum, TaxCode, proposedRateCode, SysRowID) =>
        (from row in ctx.APInvTax
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.TaxCode == TaxCode &&
         row.RateCode == proposedRateCode &&
         row.SysRowID != SysRowID
         select row).Count() == 1;



        static Expression<Func<ErpContext, string, string, bool, bool>> ExtCompanyExpression4 =
      (ctx, CompanyID, _ExtSystemID, _AllowAPAlloc) =>
        (from row in ctx.ExtCompany
         where row.Company == CompanyID &&
         row.ExtSystemID == _ExtSystemID &&
         row.AllowAPAlloc == _AllowAPAlloc
         select row).Any();



        static Expression<Func<ErpContext, string, string, int, APInvDtl>> APInvDtlExpression31 =
      (ctx, CompanyID, ipInvNum, ipInvLine) =>
        (from row in ctx.APInvDtl
         where row.Company == CompanyID &&
         row.InvoiceNum == ipInvNum &&
         row.InvoiceLine == ipInvLine
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, bool>> ISSystExpression =
      (ctx, CompanyID) =>
        (from row in ctx.ISSyst
         where row.Company == CompanyID
         select row).Count() == 1;


        static Expression<Func<ErpContext, string, string, string, string, string, string, TranGLC>> TranGLCExpression5 =
      (ctx, CompanyID, _RelatedToFile, VendorNum, InvoiceNum, InvoiceLine, InvExpSeq) =>
        (from row in ctx.TranGLC
         where row.Company == CompanyID &&
         row.RelatedToFile == _RelatedToFile &&
         row.Key1 == VendorNum &&
         row.Key2 == InvoiceNum &&
         row.Key3 == InvoiceLine &&
         row.Key4 == InvExpSeq
         select row).FirstOrDefault();

        static Expression<Func<ErpContext, Guid, TranGLC>> TranGLCExpression6 =
      (ctx, rTranGLC) =>
        (from row in ctx.TranGLC.With(LockHint.UpdLock)
         where row.SysRowID == rTranGLC
         select row).FirstOrDefault();

        static Expression<Func<ErpContext, string, string, APInvHed>> APInvHedExpression90 =
      (ctx, Company, InvoiceNum) =>
        (from row in ctx.APInvHed
         where row.Company == Company &&
         row.InvoiceNum == InvoiceNum
         select row).FirstOrDefault();

        static Expression<Func<ErpContext, string, int, string, int, int, APInvMsc>> AltAPInvMscExpression =
      (ctx, CompanyID, VendorNum, InvoiceNum, _InvoiceLine, InvExpSeq) =>
        (from row in ctx.APInvMsc
         where row.Company == CompanyID &&
         row.VendorNum == VendorNum &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine == _InvoiceLine &&
         row.InvExpSeq == InvExpSeq
         select row).FirstOrDefault();


        static Expression<Func<ErpContext, string, string, IEnumerable<APInvHed>>> APInvHedExpression91 =
      (ctx, CompanyID, cGroupID) =>
        (from row in ctx.APInvHed.With(LockHint.NoLock)
         where row.Company == CompanyID &&
         row.GroupID == cGroupID &&
         row.Posted == false &&
         (row.SEBankRef).Trim() == ""
         select row);


        static Expression<Func<ErpContext, string, int, int, int, string, int, IEnumerable<APInvDtl>>> APInvDtlExpression32 =
      (ctx, Company, VendorNum, PONum, POLine, InvoiceNum, InvoiceLine) =>
        (from row in ctx.APInvDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PONum == PONum &&
         row.POLine == POLine &&
         row.InvoiceNum == InvoiceNum &&
         row.InvoiceLine != InvoiceLine
         select row);

        static Expression<Func<ErpContext, string, int, int, string, string, int, APInvDtl>> APInvDtlExpression33 =
      (ctx, Company, VendorNum, PONum, InvoiceNum, LineType, POLine) =>
        (from row in ctx.APInvDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PONum == PONum &&
         row.InvoiceNum != InvoiceNum &&
         row.LineType == LineType &&
         row.POLine == POLine
         select row).FirstOrDefault();

        static Expression<Func<ErpContext, string, int, int, string, decimal>> SelectTotalInvoicedAmt =
      (ctx, Company, VendorNum, PONum, PartNum) =>
        (from row in ctx.APInvDtl
         where row.Company == Company &&
         row.VendorNum == VendorNum &&
         row.PONum == PONum &&
         row.PartNum == PartNum
         select row.ExtCost).Sum();

        static Expression<Func<ErpContext, string, string, string, bool>> ExistsCorrTranDocType =
        (ctx, CompanyID, ipTranDocTypeID, ipSystemTranID) =>
        (from row in ctx.TranDocType
         where row.Company == CompanyID &&
         row.TranDocTypeID == ipTranDocTypeID &&
         row.SystemTranID == ipSystemTranID
         select 1).Any();

        static Func<ErpContext, string, string, bool, IEnumerable<string>> selectAPInvHedVendorIDQuery;
        private IEnumerable<string> SelectAPInvHedVendorIDVatCheck(string company, string groupID, bool posted)
        {
            if (selectAPInvHedVendorIDQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, IEnumerable<string>>> expression =
              (ctx, company_ex, groupID_ex, posted_ex) =>
                (from row in ctx.APInvHed
                 join vendorRow in ctx.Vendor on new { row.Company, row.VendorNum } equals new { vendorRow.Company, vendorRow.VendorNum }
                 where row.Company == company_ex &&
                 row.GroupID == groupID_ex &&
                 row.Posted == posted_ex &&
                 row.EmpID == string.Empty
                 group row by vendorRow.VendorID into grpRow
                 select grpRow.Key);
                selectAPInvHedVendorIDQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvHedVendorIDQuery(Db, company, groupID, posted);
        }

        #region APInvHed
        private static Func<ErpContext, string, int, string, bool> existsAPInvHedWithVendorQuery;
        private bool ExistsAPInvHed(string company, int vendorNum, string invoiceNum)
        {
            if (existsAPInvHedWithVendorQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                    (context, company_ex, vendorNum_ex, invoiceNum_ex) =>
                    (from row in context.APInvHed
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex
                     select row)
                    .Any();
                existsAPInvHedWithVendorQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvHedWithVendorQuery(this.Db, company, vendorNum, invoiceNum);
        }
        #endregion APInvHed

        #region JPAPPerBillDtl Queries

        static Func<ErpContext, string, int, int, bool> existsJPAPPerBillDtlQuery;
        private bool ExistsJPAPPerBillDtl(string company, int vendNum, int summarizationDay)
        {
            if (existsJPAPPerBillDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, bool>> expression =
      (ctx, company_ex, vendNum_ex, summarizationDay_ex) =>
        (from row in ctx.JPAPPerBillDtl
         where row.Company == company_ex &&
         row.VendorNum == vendNum_ex &&
         row.SummarizationDay != summarizationDay_ex
         select row).Any();
                existsJPAPPerBillDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJPAPPerBillDtlQuery(this.Db, company, vendNum, summarizationDay);
        }
        #endregion PerBillDtl Queries

        #region JPAPPerBillHead Queries

        static Func<ErpContext, string, int, bool> existsJPAPPerBillHeadQuery;
        private bool ExistsJPAPPerBillHead(string company, int vendNum)
        {
            if (existsJPAPPerBillHeadQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool>> expression =
      (ctx, company_ex, vendNum_ex) =>
        (from row in ctx.JPAPPerBillHead
         where row.Company == company_ex &&
         row.VendorNum == vendNum_ex
         select row).Any();
                existsJPAPPerBillHeadQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJPAPPerBillHeadQuery(this.Db, company, vendNum);
        }
        #endregion PerBillHead Queries

        #region XbSyst Queries
        private class XbSystPartialRow : Epicor.Data.TempRowBase
        {
            public bool IsDiscountforDebitM { get; set; }
            public bool OCRCalcType { get; set; }
            public string OCRNumDrivenFrom { get; set; }
            public decimal NOThresholdAmt { get; set; }
            public decimal COIFRSInterestRate { get; set; }
        }

        private static Func<ErpContext, string, XbSystPartialRow> selectXbSystQuery;
        private XbSystPartialRow SelectXbSystRow(string company)
        {
            if (selectXbSystQuery == null)
            {
                Expression<Func<ErpContext, string, XbSystPartialRow>> expression =
                    (context, company_ex) =>
                    (from row in context.XbSyst
                     where row.Company == company_ex
                     select new XbSystPartialRow
                     {
                         IsDiscountforDebitM = row.IsDiscountforDebitM,
                         OCRCalcType = row.OCRCalcType,
                         OCRNumDrivenFrom = row.OCRNumDrivenFrom,
                         NOThresholdAmt = row.NOThresholdAmt,
                         COIFRSInterestRate = row.COIFRSInterestRate
                     }).FirstOrDefault();
                selectXbSystQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectXbSystQuery(this.Db, company);
        }

        static Func<ErpContext, string, bool> selectXbSystLACTaxQuery;
        private bool SelectXbSystLACTax(string company)
        {
            if (selectXbSystLACTaxQuery == null)
            {
                Expression<Func<ErpContext, string, bool>> expression =
      (ctx, company_ex) =>
        (from row in ctx.XbSyst
         where row.Company == company_ex
         select row.LACTaxCalcEnabled).FirstOrDefault();
                selectXbSystLACTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectXbSystLACTaxQuery(this.Db, company);
        }

        private class XbSystTWParamRow : Epicor.Data.TempRowBase
        {
            public string TWGUIRegNum { get; set; }
        }

        private static Func<ErpContext, string, XbSystTWParamRow> findFirstXbSystTWParamQuery;
        private XbSystTWParamRow FindFirstXbSystTWParam(string company)
        {
            if (findFirstXbSystTWParamQuery == null)
            {
                Expression<Func<ErpContext, string, XbSystTWParamRow>> expression =
                    (context, company_ex) =>
                    (from row in context.XbSyst
                     where row.Company == company_ex
                     select new XbSystTWParamRow
                     {
                         TWGUIRegNum = row.TWGUIRegNum
                     }).FirstOrDefault();
                findFirstXbSystTWParamQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstXbSystTWParamQuery(this.Db, company);
        }


        private static Func<ErpContext, string, bool> isAPTaxLnLevelQuery;
        private bool IsAPTaxLnLevel(string Company)
        {
            if (isAPTaxLnLevelQuery == null)
            {
                Expression<Func<ErpContext, string, bool>> expression =
                    (context, Company_ex) =>
                    (from row in context.XbSyst
                     where row.Company == Company_ex
                     select row.APTaxLnLevel).FirstOrDefault();
                isAPTaxLnLevelQuery = DBExpressionCompiler.Compile(expression);
            }

            return isAPTaxLnLevelQuery(this.Db, Company);
        }

        private static Func<ErpContext, string, bool> siteIsLegalEntityQuery;
        private bool SiteIsLegalEntity(string company)
        {
            if (siteIsLegalEntityQuery == null)
            {
                Expression<Func<ErpContext, string, bool>> expression =
      (ctx, company_ex) =>
        (from row in ctx.XbSyst
         where row.Company == company_ex
         select row.SiteIsLegalEntity).FirstOrDefault();
                siteIsLegalEntityQuery = DBExpressionCompiler.Compile(expression);
            }

            return siteIsLegalEntityQuery(this.Db, company);
        }

        static Func<ErpContext, string, bool> whToInterimTaxQuery;
        private bool WhToInterimTax(string company)
        {
            if (whToInterimTaxQuery == null)
            {
                Expression<Func<ErpContext, string, bool>> expression =
      (ctx, company_ex) =>
        (from row in ctx.XbSyst
         where row.Company == company_ex
         select row.WithholdAcctToInterim).FirstOrDefault();
                whToInterimTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return whToInterimTaxQuery(this.Db, company);
        }

        #endregion XbSyst Queries

        #region APSyst Queries
        private class APSystPartialRow : Epicor.Data.TempRowBase
        {
            public int ExchangeDateToUse { get; set; }
            public bool CopyExcRateDM { get; set; }
            public bool ApplyAPPrePayAuto { get; set; }
            public string CPayCompany { get; set; }
            public bool RoundInvoice { get; set; }
            public bool DatesSetUp { get; set; }
            public string APLinkApplyDate { get; set; }
            public string APLinkTaxPDate { get; set; }
            public string APLinkTaxRateD { get; set; }
            public bool CopyExcRateCorrInv { get; set; }
            public bool CopyExcRateCancelInv { get; set; }
            public bool InvcReadyToCalcDflt { get; set; }
            public bool APAmdApplyDate { get; set; }
            public bool APAmdTaxPDate { get; set; }
            public bool APAmdTaxRateD { get; set; }
            public bool LNReqForInvc { get; set; }
            public string CPayParent { get; set; }
            public int LNBasedOnDate { get; set; }
            public bool AllowMultInvcReceipts { get; set; }
            public int DaysOutstanding { get; set; }
            public decimal PercentageTolerance { get; set; }
            public int ToleranceAmt { get; set; }
            public int APTaxRoundOption { get; set; }
        }

        static Func<ErpContext, string, APSystPartialRow> findPartialAPSystQuery;
        private APSystPartialRow FindPartialAPSyst(string company)
        {
            if (findPartialAPSystQuery == null)
            {
                Expression<Func<ErpContext, string, APSystPartialRow>> expression =
                    (ctx, company_ex) =>
                        (from row in ctx.APSyst
                         where row.Company == company_ex
                         select new APSystPartialRow
                         {
                             ExchangeDateToUse = row.ExchangeDateToUse,
                             CopyExcRateDM = row.CopyExcRateDM,
                             ApplyAPPrePayAuto = row.ApplyAPPrePayAuto,
                             CPayCompany = row.CPayCompany,
                             RoundInvoice = row.RoundInvoice,
                             DatesSetUp = row.DatesSetUp,
                             APLinkApplyDate = row.APLinkApplyDate,
                             APLinkTaxPDate = row.APLinkTaxPDate,
                             APLinkTaxRateD = row.APLinkTaxRateD,
                             CopyExcRateCorrInv = row.CopyExcRateCorrInv,
                             CopyExcRateCancelInv = row.CopyExcRateCancelInv,
                             InvcReadyToCalcDflt = row.InvcReadyToCalcDflt,
                             APAmdApplyDate = row.APAmdApplyDate,
                             APAmdTaxPDate = row.APAmdTaxPDate,
                             APAmdTaxRateD = row.APAmdTaxRateD,
                             LNReqForInvc = row.LNReqForInvc,
                             CPayParent = row.CPayParent,
                             LNBasedOnDate = row.LNBasedOnDate,
                             AllowMultInvcReceipts = row.AllowMultInvcReceipts,
                             DaysOutstanding = row.DaysOutstanding,
                             PercentageTolerance = row.PcntTolerance,
                             ToleranceAmt = row.AmountTolerance,
                             APTaxRoundOption = row.APTaxRoundOption
                         }).FirstOrDefault();
                findPartialAPSystQuery = DBExpressionCompiler.Compile(expression);
            }

            return findPartialAPSystQuery(this.Db, company);
        }

        private static Func<ErpContext, string, bool> existsAPSyst;
        private bool ExistsAPSyst(string companyID)
        {
            if (existsAPSyst == null)
            {
                Expression<Func<ErpContext, string, bool>> expression =
                    (context, companyID_ex) =>
                    (from row in context.APSyst
                     where row.Company == companyID_ex
                     select row).Any();
                existsAPSyst = DBExpressionCompiler.Compile(expression);
            }

            return existsAPSyst(this.Db, companyID);
        }

        private static Func<ErpContext, string, string> findFirstAPSystQuery;
        private string FindFirstAPSystTWAPLegNumSource(string company)
        {
            if (findFirstAPSystQuery == null)
            {
                Expression<Func<ErpContext, string, string>> expression =
                    (context, company_ex) =>
                    (from row in context.APSyst
                     where row.Company == company_ex
                     select row.TWAPLegNumSource)
                    .FirstOrDefault();
                findFirstAPSystQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPSystQuery(this.Db, company);
        }

        static Func<ErpContext, string, int> apTaxRoundQuery;
        private int APTaxRound(string company)
        {
            if (apTaxRoundQuery == null)
            {
                Expression<Func<ErpContext, string, int>> expression =
      (ctx, company_ex) =>
        (from row in ctx.APSyst
         where row.Company == company_ex
         select row.APTaxRoundOption).FirstOrDefault();
                apTaxRoundQuery = DBExpressionCompiler.Compile(expression);
            }

            return apTaxRoundQuery(this.Db, company);
        }

        static Func<ErpContext, string, bool> haveAPTaxToLineLevelQuery;
        private bool HaveAPTaxToLineLevel(string company)
        {
            if (haveAPTaxToLineLevelQuery == null)
            {
                Expression<Func<ErpContext, string, bool>> expression =
               (ctx, company_ex) =>
                                    (from row in ctx.XbSyst
                                     where row.Company == company_ex
                                     select row.APTaxLnLevel).FirstOrDefault();
                haveAPTaxToLineLevelQuery = DBExpressionCompiler.Compile(expression);
            }

            return haveAPTaxToLineLevelQuery(this.Db, company);
        }

        static Func<ErpContext, string, bool> isPODtlTaxableQuery;
        private bool IsPODtlTaxable(string company)
        {
            if (isPODtlTaxableQuery == null)
            {
                Expression<Func<ErpContext, string, bool>> expression =
               (ctx, company_ex) =>
                                    (from row in ctx.APSyst
                                     where row.Company == company_ex
                                     select row.UsePODtlTaxable).FirstOrDefault();
                isPODtlTaxableQuery = DBExpressionCompiler.Compile(expression);
            }

            return isPODtlTaxableQuery(this.Db, company);
        }
        #endregion

        #region PEAPInvTax queries
        class DetractionsResult
        {
            public string DetTaxCode { get; set; }
            public string DetRateCode { get; set; }
            public decimal TaxAmt { get; set; }
        }
        static Func<ErpContext, string, int, string, IEnumerable<DetractionsResult>> selectDetractionsQuery;
        private IEnumerable<DetractionsResult> SelectDetractions(string ipCompany, int ipVendorNum, string ipInvoiceNum)
        {
            if (selectDetractionsQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<DetractionsResult>>> expression =
                (ctx, ipCompany_ex, ipVendorNum_ex, ipInvoiceNum_ex) =>
                (from PEAPInvTax_row in ctx.PEAPInvTax
                 where PEAPInvTax_row.Company == ipCompany_ex &&
                       PEAPInvTax_row.VendorNum == ipVendorNum_ex &&
                       PEAPInvTax_row.InvoiceNum == ipInvoiceNum_ex
                 select new DetractionsResult()
                 {
                     DetTaxCode = PEAPInvTax_row.TaxCode,
                     DetRateCode = PEAPInvTax_row.RateCode,
                     TaxAmt = PEAPInvTax_row.TaxAmt
                 });
                selectDetractionsQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectDetractionsQuery(this.Db, ipCompany, ipVendorNum, ipInvoiceNum);
        }

        private static Func<ErpContext, string, int, string, PEAPInvTax> findFirstPEAPInvTaxQuery_2;
        private PEAPInvTax FindFirstPEAPInvTaxUpdLock(string ipCompany, int ipVendorNum, string ipInvoiceNum)
        {
            if (findFirstPEAPInvTaxQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, string, PEAPInvTax>> expression =
                (ctx, ipCompany_ex, ipVendorNum_ex, ipInvoiceNum_ex) =>
                (from row in ctx.PEAPInvTax.With(LockHint.UpdLock)
                 where row.Company == ipCompany_ex &&
                        row.VendorNum == ipVendorNum_ex &&
                        row.InvoiceNum == ipInvoiceNum_ex
                 select row).FirstOrDefault();
                findFirstPEAPInvTaxQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPEAPInvTaxQuery_2(this.Db, ipCompany, ipVendorNum, ipInvoiceNum);
        }

        private static Func<ErpContext, string, int, string, decimal> selectPEAPInvTaxQuery;
        private decimal SelectPEAPInvTaxSum(string ipCompany, int ipVendorNum, string ipInvoiceNum)
        {
            if (selectPEAPInvTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, decimal>> expression =
                (ctx, ipCompany_ex, ipVendorNum_ex, ipInvoiceNum_ex) =>
                (from row in ctx.PEAPInvTax
                 where row.Company == ipCompany_ex &&
                        row.VendorNum == ipVendorNum_ex &&
                        row.InvoiceNum == ipInvoiceNum_ex
                 select row.TaxAmt).DefaultIfEmpty().Sum();
                selectPEAPInvTaxQuery = DBExpressionCompiler.Compile(expression);
            }
            return selectPEAPInvTaxQuery(this.Db, ipCompany, ipVendorNum, ipInvoiceNum);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<PEAPInvTax>> selectPEAPInvTaxQuery2;
        private IEnumerable<PEAPInvTax> SelectPEAPInvTax(string ipCompany, int ipVendorNum, string ipInvoiceNum)
        {
            if (selectPEAPInvTaxQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<PEAPInvTax>>> expression =
                (ctx, ipCompany_ex, ipVendorNum_ex, ipInvoiceNum_ex) =>
                (from row in ctx.PEAPInvTax
                 where row.Company == ipCompany_ex &&
                        row.VendorNum == ipVendorNum_ex &&
                        row.InvoiceNum == ipInvoiceNum_ex
                 select row);
                selectPEAPInvTaxQuery2 = DBExpressionCompiler.Compile(expression);
            }
            return selectPEAPInvTaxQuery2(this.Db, ipCompany, ipVendorNum, ipInvoiceNum);
        }

        private static Func<ErpContext, string, int, string, IEnumerable<PEAPInvTax>> selectPEAPInvTaxUpdLockQuery2;
        private IEnumerable<PEAPInvTax> SelectPEAPInvTaxUpdLock(string ipCompany, int ipVendorNum, string ipInvoiceNum)
        {
            if (selectPEAPInvTaxUpdLockQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<PEAPInvTax>>> expression =
                (ctx, ipCompany_ex, ipVendorNum_ex, ipInvoiceNum_ex) =>
                (from row in ctx.PEAPInvTax.With(LockHint.UpdLock)
                 where row.Company == ipCompany_ex &&
                         row.VendorNum == ipVendorNum_ex &&
                         row.InvoiceNum == ipInvoiceNum_ex
                 select row);
                selectPEAPInvTaxUpdLockQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectPEAPInvTaxUpdLockQuery2(this.Db, ipCompany, ipVendorNum, ipInvoiceNum);
        }

        private static Func<ErpContext, string, int, string, string, string, PEAPInvTax> findFirstPEAPInvTaxQuery;
        private PEAPInvTax FindFirstPEAPInvTaxUpdLock(string ipCompany, int ipVendorNum, string ipInvoiceNum, string taxCode, string rateCode)
        {
            if (findFirstPEAPInvTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, string, PEAPInvTax>> expression =
                (ctx, ipCompany_ex, ipVendorNum_ex, ipInvoiceNum_ex, taxCode_ex, rateCode_ex) =>
                (from row in ctx.PEAPInvTax.With(LockHint.UpdLock)
                 where row.Company == ipCompany_ex &&
                         row.VendorNum == ipVendorNum_ex &&
                         row.InvoiceNum == ipInvoiceNum_ex &&
                         row.TaxCode == taxCode_ex &&
                         row.RateCode == rateCode_ex
                 select row).FirstOrDefault();
                findFirstPEAPInvTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPEAPInvTaxQuery(this.Db, ipCompany, ipVendorNum, ipInvoiceNum, taxCode, rateCode);
        }

        #endregion PEAPInvTax queries

        #region PESUNATDtl Queries
        private static Func<ErpContext, string, int, string, int?> findPESUNATDtlMaxLineNumQuery;
        private int? FindPESUNATDtlMaxLineNum(string company, int vendorNum, string invoiceNum)
        {
            if (findPESUNATDtlMaxLineNumQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int?>> expression =
                    (context, company_ex, vendorNum_ex, invoiceNum_ex) =>
                    (from row in context.PESUNATDtl
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex
                     orderby row.LineNum descending
                     select row.LineNum)
                    .FirstOrDefault();
                findPESUNATDtlMaxLineNumQuery = DBExpressionCompiler.Compile(expression);
            }

            return findPESUNATDtlMaxLineNumQuery(this.Db, company, vendorNum, invoiceNum);
        }
        #endregion PESUNATDtl Queries

        #region APInvDtlDEASch Queries

        static Func<ErpContext, string, int, string, bool, bool> existsAPInvDtlDEASchQuery;
        private bool ExistsAPInvDtlDEASch(string company, int vendorNum, string invoiceNum, bool posted)
        {
            if (existsAPInvDtlDEASchQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, bool>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceNum_ex, posted_ex) =>
                    (from row in ctx.APInvDtlDEASch
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex &&
                      row.Posted == posted_ex
                     select row).Any();
                existsAPInvDtlDEASchQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvDtlDEASchQuery(this.Db, company, vendorNum, invoiceNum, posted);
        }

        static Func<ErpContext, string, int, string, int, bool, bool> existsAPInvDtlDEASch;
        private bool ExistsAPInvDtlDEASch(string company, int vendorNum, string invoiceNum, int invoiceLine, bool posted)
        {
            if (existsAPInvDtlDEASch == null)
            {
                Expression<Func<ErpContext, string, int, string, int, bool, bool>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceNum_ex, invoiceLine_ex, posted_ex) =>
                    (from row in ctx.APInvDtlDEASch
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex &&
                     row.InvoiceLine == invoiceLine_ex &&
                     (!posted_ex || row.Posted == posted_ex)
                     select row).Any();
                existsAPInvDtlDEASch = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvDtlDEASch(this.Db, company, vendorNum, invoiceNum, invoiceLine, posted);
        }

        static Func<ErpContext, string, int, string, int, IEnumerable<APInvDtlDEASch>> selectAPInvDtlDEASchQuery;
        private IEnumerable<APInvDtlDEASch> SelectAPInvDtlDEASch(string company, int vendorNum, string invoiceNum, int invoiceLine)
        {
            if (selectAPInvDtlDEASchQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvDtlDEASch>>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceNum_ex, invoiceLine_ex) =>
                    (from row in ctx.APInvDtlDEASch
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex &&
                     row.InvoiceLine == invoiceLine_ex
                     select row);
                selectAPInvDtlDEASchQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlDEASchQuery(this.Db, company, vendorNum, invoiceNum, invoiceLine);
        }

        static Func<ErpContext, string, int, string, int, IEnumerable<APInvDtlDEASch>> selectAPInvDtlDEASchQuery2;
        private IEnumerable<APInvDtlDEASch> SelectAPInvDtlDEASchWithUpdLock(string company, int vendorNum, string invoiceNum, int invoiceLine)
        {
            if (selectAPInvDtlDEASchQuery2 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvDtlDEASch>>> expression =
                  (ctx, company_ex, vendorNum_ex, invoiceNum_ex, invoiceLine_ex) =>
                    (from row in ctx.APInvDtlDEASch.With(LockHint.UpdLock)
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex &&
                     row.InvoiceLine == invoiceLine_ex
                     select row);
                selectAPInvDtlDEASchQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvDtlDEASchQuery2(this.Db, company, vendorNum, invoiceNum, invoiceLine);
        }

        private static Func<ErpContext, string, int, string, int, int, APInvDtlDEASch> findFirstAPInvDtlDEASchQuery;
        private APInvDtlDEASch FindFirstAPInvDtlDEASchWithUpdLock(string Company, int VendorNum, string InvoiceNum, int InvoiceLine, int AmortSeq)
        {
            if (findFirstAPInvDtlDEASchQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, APInvDtlDEASch>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex, AmortSeq_ex) =>
                    (from row in context.APInvDtlDEASch.With(LockHint.UpdLock)
                     where row.Company == Company_ex &&
                           row.VendorNum == VendorNum_ex &&
                           row.InvoiceNum == InvoiceNum_ex &&
                           row.InvoiceLine == InvoiceLine_ex &&
                           row.AmortSeq == AmortSeq_ex
                     select row)
                    .FirstOrDefault();
                findFirstAPInvDtlDEASchQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvDtlDEASchQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine, AmortSeq);
        }

        #endregion
        #region APInvMisc Queries
        class APInvMiscPartial
        {
            public decimal DocMiscAmt { get; set; }
        }

        static Func<ErpContext, string, int, string, int, int, APInvMiscPartial> findFirstAPInvMiscPartialQuery;
        private APInvMiscPartial FindFirstAPInvMiscPartial(string company, int vendorNum, string invoiceNum, int invoiceLine, int miscNum)
        {
            if (findFirstAPInvMiscPartialQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, APInvMiscPartial>> expression =
                  (ctx, company_ex, vendornum_ex, invoicenum_ex, invoiceLine_ex, miscNum_ex) =>
                    (from row in ctx.APInvMsc
                     where row.Company == company_ex &&
                           row.VendorNum == vendornum_ex &&
                           row.InvoiceNum == invoicenum_ex &&
                           row.InvoiceLine == invoiceLine_ex &&
                           row.MscNum == miscNum_ex
                     select new APInvMiscPartial()
                     {
                         DocMiscAmt = row.DocMiscAmt
                     }).FirstOrDefault();
                findFirstAPInvMiscPartialQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstAPInvMiscPartialQuery(this.Db, company, vendorNum, invoiceNum, invoiceLine, miscNum);
        }

        #endregion APInvMiscQueries

        #region APInvLineMscTax Queries

        private static Func<ErpContext, string, int, string, int, IEnumerable<APInvLnMscTax>> selectAPInvLnMiscTaxQuery;
        private IEnumerable<APInvLnMscTax> SelectAPInvLnMiscTax(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (selectAPInvLnMiscTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<APInvLnMscTax>>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    (from row in context.APInvLnMscTax
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == InvoiceNum_ex &&
                     row.InvoiceLine == InvoiceLine_ex &&
                     row.InvoiceLine != 0
                     select row);
                selectAPInvLnMiscTaxQuery = DBExpressionCompiler.Compile(expression);

            }

            return selectAPInvLnMiscTaxQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }

        #endregion

        #region COA Queries
        static Func<ErpContext, string, string, string> getSeparatorChar;
        private string GetSeparatorChar(string company, string coacode)
        {
            if (getSeparatorChar == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
      (ctx, company_ex, coacode_ex) =>
        (from row in ctx.COA
         where row.Company == company_ex &&
         row.COACode == coacode_ex
         select row.SeparatorChar).FirstOrDefault();
                getSeparatorChar = DBExpressionCompiler.Compile(expression);
            }

            return getSeparatorChar(this.Db, company, coacode);
        }
        #endregion

        #region UDCodes Queries

        static Func<ErpContext, string, string, bool, string, bool> existsUDCodesQuery;
        private bool ExistsUDCodes(string company, string codeTypeID, bool isActive, string codeID)
        {
            if (existsUDCodesQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, string, bool>> expression =
                  (ctx, company_ex, codeTypeID_ex, isActive_ex, codeID_ex) =>
                    (from row in ctx.UDCodes
                     where row.Company == company_ex &&
                     row.CodeTypeID == codeTypeID_ex &&
                     row.IsActive == isActive_ex &&
                     row.CodeID == codeID_ex
                     select row).Any();
                existsUDCodesQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsUDCodesQuery(this.Db, company, codeTypeID, isActive, codeID);
        }
        #endregion UDCodes Queries

        #region UOMClass Queries
        static Func<ErpContext, string, string, UOMClass> findFirstUOMClassQuery;
        private UOMClass FindFirstUOMClassByType(string company, string classType)
        {
            if (findFirstUOMClassQuery == null)
            {
                Expression<Func<ErpContext, string, string, UOMClass>> expression =
              (ctx, company_ex, classType_ex) =>
                (from row in ctx.UOMClass
                 where row.Company == company_ex &&
                 row.ClassType == classType_ex
                 select row).FirstOrDefault();
                findFirstUOMClassQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstUOMClassQuery(this.Db, company, classType);
        }
        #endregion UOMClass Queries

        #region PORel Queries
        static Func<ErpContext, string, int, int, int, PORel> findFirstPORelWithUpdLockQuery;
        private PORel FindFirstPORelWithUpdLock(string company, int ponum, int poline, int porelNum)
        {
            if (findFirstPORelWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, PORel>> expression =
      (ctx, company_ex, ponum_ex, poline_ex, porelNum_ex) =>
        (from row in ctx.PORel.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PONum == ponum_ex &&
         row.POLine == poline_ex &&
         row.PORelNum == porelNum_ex
         select row).FirstOrDefault();
                findFirstPORelWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPORelWithUpdLockQuery(this.Db, company, ponum, poline, porelNum);
        }
        #endregion

        #region POHeader

        private static Func<ErpContext, string, int, int, string> findFirstPOHeaderCurrencyCodeQuery;
        private string FindFirstPOHeaderCurrencyCode(string Company, int PoNum, int VendorNum)
        {
            if (findFirstPOHeaderCurrencyCodeQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, string>> expression =
                    (context, Company_ex, PoNum_ex, VendorNum_ex) =>
                    (from row in context.POHeader
                     where row.Company == Company_ex &&
                     row.PONum == PoNum_ex &&
                     row.VendorNum == VendorNum_ex
                     select row.CurrencyCode).FirstOrDefault();
                findFirstPOHeaderCurrencyCodeQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPOHeaderCurrencyCodeQuery(this.Db, Company, PoNum, VendorNum);
        }

        #endregion

        #region JPAPPerBillStmtHead

        static Func<ErpContext, string, int, string, bool> existsJPAPPerBillStmtHeadBillingNo;
        private bool ExistsJPAPPerBillStmtHeadBillingNo(string ipCompany, int ipVendorNum, string ipInvoiceNum)
        {
            if (existsJPAPPerBillStmtHeadBillingNo == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                (ctx, ipCompany_ex, ipVendorNum_ex, ipInvoiceNum_ex) =>
                (from JPAPPerBillStmtHead_row in ctx.JPAPPerBillStmtHead
                 where JPAPPerBillStmtHead_row.Company == ipCompany_ex &&
                       JPAPPerBillStmtHead_row.VendorNum == ipVendorNum_ex &&
                       JPAPPerBillStmtHead_row.AdjInvoiceNum == ipInvoiceNum_ex
                 select true).Any();
                existsJPAPPerBillStmtHeadBillingNo = DBExpressionCompiler.Compile(expression);
            }
            return existsJPAPPerBillStmtHeadBillingNo(this.Db, ipCompany, ipVendorNum, ipInvoiceNum);
        }

        #endregion JPAPPerBillStmtHead

        #region APInvExp Queries

        private static Func<ErpContext, string, int, string, int, int, APInvExp> findFirstAPInvExpQuery3;
        private APInvExp FindFirstAPInvExp(string company, int vendorNum, string invoiceNum, int invoiceLine, int invExpSeq)
        {
            if (findFirstAPInvExpQuery3 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, int, APInvExp>> expression =
                    (ctx, company_ex, vendorNum_ex, invoiceNum_ex, invoiceLine_ex, invExpSeq_ex) =>
                    (from row in ctx.APInvExp
                     where row.Company == company_ex &&
                     row.VendorNum == vendorNum_ex &&
                     row.InvoiceNum == invoiceNum_ex &&
                     row.InvoiceLine == invoiceLine_ex &&
                     row.InvExpSeq == invExpSeq_ex
                     select row).FirstOrDefault();
                findFirstAPInvExpQuery3 = DBExpressionCompiler.Compile(expression);
            }
            return findFirstAPInvExpQuery3(Db, company, vendorNum, invoiceNum, invoiceLine, invExpSeq);
        }

        private class selectedGLAccount
        {
            public string Company { get; set; }
            public string ExtCompanyID { get; set; }
            public DateTime? InvoiceDate { get; set; }
            public string InvoiceNum { get; set; }
            public string InvoiceLine { get; set; }
            public string GLAccountContext { get; set; }
            public string COACode { get; set; }
            public string GLAccount { get; set; }
            public string RelatedToFile { get; set; }
            public string BookID { get; set; }
        }
        private static Func<ErpContext, string, string, IEnumerable<selectedGLAccount>> existedGLAccount;
        private IEnumerable<selectedGLAccount> selectGLAccountFromTranGLC(string Company, string GroupID)
        {
            if (existedGLAccount == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<selectedGLAccount>>> expression =
                (ctx, Company_ex, Group_ex) =>
                (from APInvHedRow in ctx.APInvHed
                 join APInvExpRow in ctx.APInvExp on new { APInvHedRow.Company, APInvHedRow.VendorNum, APInvHedRow.InvoiceNum } equals new
                 { APInvExpRow.Company, APInvExpRow.VendorNum, APInvExpRow.InvoiceNum }

                 join TranGLCRow in ctx.TranGLC on new
                 {
                     APInvExpRow.Company,
                     VendorNum = SqlFunctions.StringConvert((decimal)APInvExpRow.VendorNum).Trim(),
                     APInvExpRow.InvoiceNum
                 }
                 equals new
                 {
                     TranGLCRow.Company,
                     VendorNum = TranGLCRow.Key1,
                     InvoiceNum = TranGLCRow.Key2

                 }
                 where APInvHedRow.Company == Company_ex &&
                       APInvHedRow.GroupID == Group_ex &&
                       APInvHedRow.Posted == false &&
                       TranGLCRow.RecordType == "A" &&
                       (TranGLCRow.RelatedToFile == "APInvDtl" || TranGLCRow.RelatedToFile == "APInvExp")
                 select new selectedGLAccount
                 {
                     Company = TranGLCRow.Company,
                     InvoiceDate = APInvHedRow.ApplyDate,
                     COACode = TranGLCRow.COACode,
                     GLAccount = TranGLCRow.GLAccount,
                     ExtCompanyID = TranGLCRow.ExtCompanyID,
                     InvoiceLine = TranGLCRow.Key3,
                     InvoiceNum = TranGLCRow.Key2,
                     RelatedToFile = TranGLCRow.RelatedToFile,
                     GLAccountContext = TranGLCRow.GLAcctContext,
                     BookID = TranGLCRow.BookID

                 });
                existedGLAccount = DBExpressionCompiler.Compile(expression);
            }

            return existedGLAccount(this.Db, Company, GroupID);
        }

        static Func<ErpContext, string, int, string, IEnumerable<APInvExp>> selectAPInvExpNonDedTax2Query;
        private IEnumerable<APInvExp> SelectAPInvExpNonDedTax2(string company, int vendorNum, string invoiceNum)
        {
            if (selectAPInvExpNonDedTax2Query == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<APInvExp>>> expression =
                (ctx, company_ex, vendorNum_ex, invoiceNum_ex) =>
                                                               (from expense in ctx.APInvExp
                                                                where expense.Company == company_ex &&
                                                                      expense.VendorNum == vendorNum_ex &&
                                                                      expense.InvoiceNum == invoiceNum_ex &&
                                                                      expense.InvoiceLine != 0
                                                                select expense);
                selectAPInvExpNonDedTax2Query = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvExpNonDedTax2Query(this.Db, company, vendorNum, invoiceNum);
        }

        static Func<ErpContext, string, int, string, bool, IEnumerable<APInvExp>> selectAPInvExpNonDedTaxQuery;
        private IEnumerable<APInvExp> SelectAPInvExpNonDedTax(string company, int vendorNum, string invoiceNum, bool lineLevel)
        {
            if (selectAPInvExpNonDedTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, IEnumerable<APInvExp>>> expression =
                (ctx, company_ex, vendorNum_ex, invoiceNum_ex, lineLevel_ex) =>
                                                               (from expense in ctx.APInvExp
                                                                where expense.Company == company_ex &&
                                                                      expense.VendorNum == vendorNum_ex &&
                                                                      expense.InvoiceNum == invoiceNum_ex &&
                                                                      expense.NonDedTax == true &&
                                                                      ((lineLevel_ex && expense.InvoiceLine != 0) ||
                                                                       (!lineLevel_ex && expense.InvoiceLine == 0))
                                                                select expense);
                selectAPInvExpNonDedTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvExpNonDedTaxQuery(this.Db, company, vendorNum, invoiceNum, lineLevel);
        }


        static Func<ErpContext, string, int, string, string, Guid, IEnumerable<APInvExp>> selectAPInvExpTaxQuery;
        private IEnumerable<APInvExp> SelectAPInvExpTax(string company, int vendorNum, string invoiceNum, string taxTable, Guid taxSysRowID)
        {
            if (selectAPInvExpTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, Guid, IEnumerable<APInvExp>>> expression =
                (ctx, company_ex, vendorNum_ex, invoiceNum_ex, taxTable_ex, taxSysRowID_ex) =>
                                                               (from expense in ctx.APInvExp
                                                                where expense.Company == company_ex &&
                                                                      expense.VendorNum == vendorNum_ex &&
                                                                      expense.InvoiceNum == invoiceNum_ex &&
                                                                      expense.NonDedTaxRelatedToSysRowID == taxSysRowID_ex &&
                                                                      expense.NonDedTaxRelatedToTable == taxTable_ex &&
                                                                      expense.NonDedTaxRelatedToSchema == "Erp"
                                                                select expense);
                selectAPInvExpTaxQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvExpTaxQuery(this.Db, company, vendorNum, invoiceNum, taxTable, taxSysRowID);
        }
        static Func<ErpContext, string, int, string, string, Guid, IEnumerable<APInvExp>> selectAPInvExpTaxWithLockQuery;
        private IEnumerable<APInvExp> SelectAPInvExpTaxWithLock(string company, int vendorNum, string invoiceNum, string taxTable, Guid taxSysRowID)
        {
            if (selectAPInvExpTaxWithLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, Guid, IEnumerable<APInvExp>>> expression =
                (ctx, company_ex, vendorNum_ex, invoiceNum_ex, taxTable_ex, taxSysRowID_ex) =>
                                                               (from expense in ctx.APInvExp.With(LockHint.UpdLock)
                                                                where expense.Company == company_ex &&
                                                                      expense.VendorNum == vendorNum_ex &&
                                                                      expense.InvoiceNum == invoiceNum_ex &&
                                                                      expense.NonDedTaxRelatedToSysRowID == taxSysRowID_ex &&
                                                                      expense.NonDedTaxRelatedToTable == taxTable_ex &&
                                                                      expense.NonDedTaxRelatedToSchema == "Erp"
                                                                select expense);
                selectAPInvExpTaxWithLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvExpTaxWithLockQuery(this.Db, company, vendorNum, invoiceNum, taxTable, taxSysRowID);
        }

        static Func<ErpContext, string, int, string, bool, IEnumerable<APInvExp>> selectAPInvExpForInvoiceQuery;
        private IEnumerable<APInvExp> SelectAPInvExpForInvoice(string company, int vendorNum, string invoiceNum, bool lineLevel)
        {
            if (selectAPInvExpForInvoiceQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool, IEnumerable<APInvExp>>> expression =
                (ctx, company_ex, vendorNum_ex, invoiceNum_ex, lineLevel_ex) =>
                                                               (from expense in ctx.APInvExp
                                                                where expense.Company == company_ex &&
                                                                      expense.VendorNum == vendorNum_ex &&
                                                                      expense.InvoiceNum == invoiceNum_ex &&
                                                                      ((lineLevel_ex && expense.InvoiceLine != 0) ||
                                                                       (!lineLevel_ex && expense.InvoiceLine == 0))
                                                                select expense);
                selectAPInvExpForInvoiceQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAPInvExpForInvoiceQuery(this.Db, company, vendorNum, invoiceNum, lineLevel);
        }

        static Func<ErpContext, Guid, bool> existsAPInvExpQuery;
        private bool ExistsAPInvExp(Guid sysRowID)
        {
            if (existsAPInvExpQuery == null)
            {
                Expression<Func<ErpContext, Guid, bool>> expression =
                (ctx, sysRowID_ex) =>
                    (from expense in ctx.APInvExp
                     where expense.SysRowID == sysRowID_ex
                     select expense).Any();
                existsAPInvExpQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsAPInvExpQuery(this.Db, sysRowID);
        }

        #endregion

        #region IStatTrn Queries

        static Func<ErpContext, string, string, int, string, int, bool> existsIStatTrnQuery;
        private bool ExistsIStatTrn(string company, string sourceModule, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (existsIStatTrnQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, int, bool>> expression =
                (ctx, Company_ex, sourceModule_ex, VendorNum_ex, InvoiceNum_ex, InvoiceLine_ex) =>
                    (from row in ctx.IStatTrn
                     where row.Company == Company_ex &&
                    row.SourceModule == sourceModule_ex &&
                    row.VendorNum == VendorNum_ex &&
                    row.InvoiceNum == InvoiceNum_ex &&
                    row.InvoiceLine == InvoiceLine_ex
                     select row.Company).Any();
                existsIStatTrnQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsIStatTrnQuery(this.Db, company, sourceModule, VendorNum, InvoiceNum, InvoiceLine);
        }
        #endregion

        #region TaxCat Queries
        private static Func<ErpContext, string, string, string> findFirstTaxCatDescQuery;
        private string FindFirstTaxCatDesc(string company, string taxCatID)
        {
            if (findFirstTaxCatDescQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
                    (ctx, company_ex, taxCatID_ex) =>
                    (from row in ctx.TaxCat
                     where row.Company == company_ex &&
                     row.TaxCatID == taxCatID_ex
                     select row.Description).FirstOrDefault();
                findFirstTaxCatDescQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstTaxCatDescQuery(Db, company, taxCatID);
        }
        #endregion TaxCat Queries

        #region TaxCatD Queries
        static Func<ErpContext, string, string, string, string> findFirstTaxCatDQuery;
        private string FindFirstTaxCatD(string company, string taxCatID, string taxCode)
        {
            if (findFirstTaxCatDQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string>> expression =
                  (ctx, company_ex, taxCatID_ex, taxCode_ex) =>
                    (from row in ctx.TaxCatD
                     where row.Company == company_ex &&
                     row.TaxCatID == taxCatID_ex &&
                     row.TaxCode == taxCode_ex
                     select row.RateCode).FirstOrDefault();
                findFirstTaxCatDQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTaxCatDQuery(this.Db, company, taxCatID, taxCode);
        }
        #endregion TaxCatD Queries

        #region APLnTax

        static Func<ErpContext, string, int, string, int, IEnumerable<Guid>> getNonDedTaxesForInvLnQuery;
        private IEnumerable<Guid> GetNonDedTaxesForInvLn(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (getNonDedTaxesForInvLnQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<Guid>>> expression =
               (ctx, company_ex, vendorNum_ex, invoiceNum_ex, invoiceLine_ex) =>
                                    (from row in ctx.APLnTax
                                     join taxRow in ctx.SalesTax
                                     on new { row.Company, row.TaxCode } equals
                                     new { taxRow.Company, taxRow.TaxCode }
                                     where row.Company == company_ex &&
                                            row.VendorNum == vendorNum_ex &&
                                            row.InvoiceNum == invoiceNum_ex &&
                                            row.InvoiceLine == invoiceLine_ex &&
                                           (
                                             (row.DocTaxAmt != row.DocDedTaxAmt) ||
                                             (row.TaxAmt != row.DedTaxAmt) ||
                                             (row.Rpt1TaxAmt != row.Rpt1DedTaxAmt) ||
                                             (row.Rpt2TaxAmt != row.Rpt2DedTaxAmt) ||
                                             (row.Rpt3TaxAmt != row.Rpt3DedTaxAmt)
                                           ) &&
                                           (
                                             taxRow.Timing == 0 ||
                                             taxRow.Timing == 1 ||
                                             taxRow.Timing == 2
                                           ) &&
                                           taxRow.CollectionType == 0
                                     select row.SysRowID);
                getNonDedTaxesForInvLnQuery = DBExpressionCompiler.Compile(expression);
            }

            return getNonDedTaxesForInvLnQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }
        #endregion APLnTax

        #region APInvTax

        static Func<ErpContext, string, int, string, bool> existsNonDedTaxForInvoiceQuery;
        private bool ExistsNonDedTaxForInvoice(string Company, int VendorNum, string InvoiceNum)
        {
            if (existsNonDedTaxForInvoiceQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
               (ctx, company_ex, vendorNum_ex, invoiceNum_ex) =>
                                    (from row in ctx.APInvTax
                                     join taxRow in ctx.SalesTax
                                     on new { row.Company, row.TaxCode, Timing = 0, CollectionType = 0 } equals
                                     new { taxRow.Company, taxRow.TaxCode, taxRow.Timing, taxRow.CollectionType }
                                     where row.Company == company_ex &&
                                            row.VendorNum == vendorNum_ex &&
                                            row.InvoiceNum == invoiceNum_ex &&
                                           (
                                             (row.DocTaxAmt != row.DocDedTaxAmt) ||
                                             (row.TaxAmt != row.DedTaxAmt) ||
                                             (row.Rpt1TaxAmt != row.Rpt1DedTaxAmt) ||
                                             (row.Rpt2TaxAmt != row.Rpt2DedTaxAmt) ||
                                             (row.Rpt3TaxAmt != row.Rpt3DedTaxAmt)
                                           )
                                     select 1).Any();
                existsNonDedTaxForInvoiceQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsNonDedTaxForInvoiceQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        static Func<ErpContext, string, int, string, IEnumerable<Guid>> getNonDedTaxForInvoiceQuery;
        private IEnumerable<Guid> GetNonDedTaxForInvoice(string Company, int VendorNum, string InvoiceNum)
        {
            if (getNonDedTaxForInvoiceQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<Guid>>> expression =
               (ctx, company_ex, vendorNum_ex, invoiceNum_ex) =>
                                    (from row in ctx.APInvTax
                                     join taxRow in ctx.SalesTax
                                     on new { row.Company, row.TaxCode } equals
                                     new { taxRow.Company, taxRow.TaxCode }
                                     where row.Company == company_ex &&
                                            row.VendorNum == vendorNum_ex &&
                                            row.InvoiceNum == invoiceNum_ex &&
                                            row.ECAcquisitionSeq != 2 &&
                                           (
                                             (row.DocTaxAmt != row.DocDedTaxAmt) ||
                                             (row.TaxAmt != row.DedTaxAmt) ||
                                             (row.Rpt1TaxAmt != row.Rpt1DedTaxAmt) ||
                                             (row.Rpt2TaxAmt != row.Rpt2DedTaxAmt) ||
                                             (row.Rpt3TaxAmt != row.Rpt3DedTaxAmt)
                                           ) &&
                                           (
                                             taxRow.Timing == 0 ||
                                             taxRow.Timing == 1 ||
                                             taxRow.Timing == 2
                                           ) &&
                                           taxRow.CollectionType == 0
                                     select row.SysRowID);
                getNonDedTaxForInvoiceQuery = DBExpressionCompiler.Compile(expression);
            }

            return getNonDedTaxForInvoiceQuery(this.Db, Company, VendorNum, InvoiceNum);
        }

        #endregion APInvTax

        #region APInvLnMscTax

        static Func<ErpContext, string, int, string, int, IEnumerable<Guid>> getNonDedTaxesForInvLnMscQuery;
        private IEnumerable<Guid> GetNonDedTaxesForInvLnMsc(string Company, int VendorNum, string InvoiceNum, int InvoiceLine)
        {
            if (getNonDedTaxesForInvLnMscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, int, IEnumerable<Guid>>> expression =
               (ctx, company_ex, vendorNum_ex, invoiceNum_ex, invoiceLine_ex) =>
                                    (from row in ctx.APInvLnMscTax
                                     join taxRow in ctx.SalesTax
                                     on new { row.Company, row.TaxCode } equals
                                     new { taxRow.Company, taxRow.TaxCode }
                                     where row.Company == company_ex &&
                                            row.VendorNum == vendorNum_ex &&
                                            row.InvoiceNum == invoiceNum_ex &&
                                            row.InvoiceLine == invoiceLine_ex &&
                                           (
                                             (row.DocTaxAmt != row.DocDedTaxAmt) ||
                                             (row.TaxAmt != row.DedTaxAmt) ||
                                             (row.Rpt1TaxAmt != row.Rpt1DedTaxAmt) ||
                                             (row.Rpt2TaxAmt != row.Rpt2DedTaxAmt) ||
                                             (row.Rpt3TaxAmt != row.Rpt3DedTaxAmt)
                                           ) &&
                                           (
                                             taxRow.Timing == 0 ||
                                             taxRow.Timing == 1 ||
                                             taxRow.Timing == 2
                                           ) &&
                                           taxRow.CollectionType == 0
                                     select row.SysRowID);
                getNonDedTaxesForInvLnMscQuery = DBExpressionCompiler.Compile(expression);
            }

            return getNonDedTaxesForInvLnMscQuery(this.Db, Company, VendorNum, InvoiceNum, InvoiceLine);
        }

        #endregion APInvLnMscTax

        #region JobMtl Queries
        static Func<ErpContext, string, string, int, int, decimal> getActualJobChargeQuery;
        private decimal GetJobMtlChargeAmt(string company, string jobNum, int assemblySeq, int mtlSeq)
        {
            if (getActualJobChargeQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, decimal>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex) =>
        (from row in ctx.JobMtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex
         select row.MaterialMtlCost).FirstOrDefault();
                getActualJobChargeQuery = DBExpressionCompiler.Compile(expression);
            }

            return getActualJobChargeQuery(this.Db, company, jobNum, assemblySeq, mtlSeq);
        }
        #endregion JobMtl Queries

        #region NettingDtl Queries
        private static Func<ErpContext, string, int, string, bool> existsNettingDtlQuery;
        private bool ExistsNettingDtl(string Company, int VendorNum, string InvoiceNum)
        {
            if (existsNettingDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, bool>> expression =
                    (context, Company_ex, VendorNum_ex, InvoiceNum_ex) =>
                    (from row in context.NettingHead
                     join dtlRow in context.NettingDtl on new { row.Company, row.NettingID } equals new { dtlRow.Company, dtlRow.NettingID }
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     dtlRow.APInvoiceNum == InvoiceNum_ex &&
                     row.Posted == false
                     select dtlRow).Any();
                existsNettingDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsNettingDtlQuery(this.Db, Company, VendorNum, InvoiceNum);
        }
        #endregion

        #region EntityTGLC
        static Func<ErpContext, string, string, string, string, string, EntityGLC> findFirstEntityGLCUpdLockQuery;
        private EntityGLC FindFirstEntityGLCUpdLock(string company, string relatedToFile, string key1, string key2, string key3)
        {
            if (findFirstEntityGLCUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, EntityGLC>> expression =
                (ctx, company_ex, relatedToFile_ex, key1_ex, key2_ex, key3_ex) =>
                                                                            (from row in ctx.EntityGLC.With(LockHint.UpdLock)
                                                                             where row.Company == company_ex &&
                                                                                   row.RelatedToFile == relatedToFile_ex &&
                                                                                   row.Key1 == key1_ex &&
                                                                                   row.Key2 == key2_ex &&
                                                                                   row.Key3 == key3_ex
                                                                             select row).FirstOrDefault();
                findFirstEntityGLCUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstEntityGLCUpdLockQuery(this.Db, company, relatedToFile, key1, key2, key3);
        }
        #endregion EntityTGLC

        #region 
        static Func<ErpContext, string, int, string, IEnumerable<Guid>> getNonDedTaxesForInvHedMscQuery;
        private IEnumerable<Guid> GetNonDedTaxesForInvHedMsc(string Company, int VendorNum, string InvoiceNum)
        {
            if (getNonDedTaxesForInvHedMscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, IEnumerable<Guid>>> expression =
               (ctx, company_ex, vendorNum_ex, invoiceNum_ex) =>
                                    (from row in ctx.APInvHedMscTax
                                     join taxRow in ctx.SalesTax
                                     on new { row.Company, row.TaxCode } equals
                                     new { taxRow.Company, taxRow.TaxCode }
                                     where row.Company == company_ex &&
                                            row.VendorNum == vendorNum_ex &&
                                            row.InvoiceNum == invoiceNum_ex &&
                                            (
                                                taxRow.Timing == 0 ||
                                                taxRow.Timing == 1 ||
                                                taxRow.Timing == 2
                                            ) &&
                                            taxRow.CollectionType == 0
                                     select row.APInvMscSysRowID);
                getNonDedTaxesForInvHedMscQuery = DBExpressionCompiler.Compile(expression);
            }

            return getNonDedTaxesForInvHedMscQuery(this.Db, Company, VendorNum, InvoiceNum);
        }
        #endregion

        #region APLateCosts
        static Expression<Func<ErpContext, string, int, string, int, IEnumerable<ContainerHeader>>> ContainerHeaderExpression1 =
            (ctx, Company, VendorNum, PurPoint, ContainerID) =>
            (from row in ctx.ContainerHeader
             where row.Company == Company &&
             row.VendorNum == VendorNum &&
             row.ContainerID == ContainerID &&
             row.PurPoint == PurPoint
             select row);

        private static Func<ErpContext, string, int, IEnumerable<RcvHead>> selectAllLateCostsRcvHeadQuery;
        private IEnumerable<RcvHead> SelectAllLateCostsRcvHead(string company, int vendorNum)
        {
            if (selectAllLateCostsRcvHeadQuery == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<RcvHead>>> expression =
                (ctx, company_ex, vendorNum_ex) =>
                                                    (from row in ctx.RcvHead
                                                     where row.Company == company_ex
                                                             && row.VendorNum == vendorNum_ex
                                                             && row.SaveForInvoicing == true
                                                     select row);
                selectAllLateCostsRcvHeadQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAllLateCostsRcvHeadQuery(this.Db, company, vendorNum);
        }

        private static Func<ErpContext, string, int, string, string, int, bool> existLateCostRcvDtlQuery;
        private bool ExistsLateCostRcvDtl(string Company, int VendorNum, string PackSlip, string PurPoint, int InPONum)
        {
            if (existLateCostRcvDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, bool>> expression =
                    (ctx, Company_ex, VendorNum_ex, PackSlip_ex, PurPoint_ex, PONum_ex) =>
                    (from row in ctx.RcvDtl
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.PackSlip == PackSlip_ex &&
                     row.PurPoint == PurPoint_ex &&
                     row.PONum == PONum_ex
                     select row)
                    .Any();
                existLateCostRcvDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return existLateCostRcvDtlQuery(this.Db, Company, VendorNum, PackSlip, PurPoint, InPONum);
        }

        private static Func<ErpContext, string, int, IEnumerable<ContainerHeader>> selectAllShippedContainerHeaderQuery;
        private IEnumerable<ContainerHeader> SelectAllShippedContainerHeader(string company, int vendorNum)
        {
            if (selectAllShippedContainerHeaderQuery == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<ContainerHeader>>> expression =
                (ctx, company_ex, vendorNum_ex) =>
                                                    (from row in ctx.ContainerHeader
                                                     where row.Company == company_ex &&
                                                             row.VendorNum == vendorNum_ex &&
                                                             row.ShipStatus == "SHIPPED"
                                                     select row);
                selectAllShippedContainerHeaderQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAllShippedContainerHeaderQuery(this.Db, company, vendorNum);
        }

        private static Func<ErpContext, string, int, int, bool> existsContainerMiscQuery;
        private bool ExistsLateCostContainerMisc(string Company, int ContainerID, int VendorNum)
        {
            if (existsContainerMiscQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, bool>> expression =
                    (ctx, Company_ex, ContainerID_ex, VendorNum_ex) =>
                    (from row in ctx.ContainerMisc
                     where row.Company == Company_ex &&
                     row.ContainerID == ContainerID_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == ""
                     select row)
                    .Any();
                existsContainerMiscQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsContainerMiscQuery(this.Db, Company, ContainerID, VendorNum);
        }

        private static Func<ErpContext, string, int, int, IEnumerable<ContainerMisc>> selectAllContainerMiscQuery;
        private IEnumerable<ContainerMisc> SelectAllContainerMisc(string Company, int ContainerID, int VendorNum)
        {
            if (selectAllContainerMiscQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, IEnumerable<ContainerMisc>>> expression =
                    (ctx, Company_ex, ContainerID_ex, VendorNum_ex) =>
                    (from row in ctx.ContainerMisc
                     where row.Company == Company_ex &&
                     row.ContainerID == ContainerID_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.InvoiceNum == ""
                     select row);
                selectAllContainerMiscQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAllContainerMiscQuery(this.Db, Company, ContainerID, VendorNum);
        }

        private static Func<ErpContext, string, int, ContainerHeader> selectContainerHeaderQuery;
        private ContainerHeader selectContainerHeader(string Company, int ContainerID)
        {
            if (selectContainerHeaderQuery == null)
            {
                Expression<Func<ErpContext, string, int, ContainerHeader>> expression =
                    (ctx, Company_ex, ContainerID_ex) =>
                    (from row in ctx.ContainerHeader
                     where row.Company == Company_ex &&
                     row.ContainerID == ContainerID_ex
                     select row)
                    .FirstOrDefault();
                selectContainerHeaderQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectContainerHeaderQuery(this.Db, Company, ContainerID);
        }

        private static Func<ErpContext, string, int, IEnumerable<ContainerHeader>> selectAllLateCostsContainerHeaderWithContainerIDQuery;
        private IEnumerable<ContainerHeader> SelectAllLateCostsContainerHeaderWithContainerID(string Company, int ContainerID)
        {
            if (selectAllLateCostsContainerHeaderWithContainerIDQuery == null)
            {
                Expression<Func<ErpContext, string, int, IEnumerable<ContainerHeader>>> expression =
                                                                                            (ctx, Company_ex, ContainerID_ex) =>
                                                                                            (from row in ctx.ContainerHeader
                                                                                             where row.Company == Company_ex &&
                                                                                             (row.ShipStatus == "SHIPPED" ||
                                                                                             row.ShipStatus == "ARRIVED" ||
                                                                                             row.ShipStatus == "IMPORTED" ||
                                                                                             row.ShipStatus == "RECEIVED") &&
                                                                                             (ContainerID_ex == 0 || (ContainerID_ex > 0 && row.ContainerID == ContainerID_ex))
                                                                                             select row);
                selectAllLateCostsContainerHeaderWithContainerIDQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAllLateCostsContainerHeaderWithContainerIDQuery(this.Db, Company, ContainerID);
        }

        private static Func<ErpContext, string, int, int, bool> existsLateCostContainerDetailQuery;
        private bool ExistsLateCostContainerDetail(string Company, int ContainerID, int PONum)
        {
            if (existsLateCostContainerDetailQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, bool>> expression =
                    (ctx, Company_ex, ContainerID_ex, PONum_ex) =>
                    (from row in ctx.ContainerDetail
                     where row.Company == Company_ex &&
                     row.ContainerID == ContainerID_ex &&
                     row.PONum == PONum_ex
                     select row)
                    .Any();
                existsLateCostContainerDetailQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsLateCostContainerDetailQuery(this.Db, Company, ContainerID, PONum);
        }

        private static Func<ErpContext, string, int, string, string, bool> existsLateCostRcvMiscQuery;
        private bool ExistsLateCostRcvMisc(string Company, int VendorNum, string PurPoint, string PackSlip)
        {
            if (existsLateCostRcvMiscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, bool>> expression =
                    (ctx, Company_ex, VendorNum_ex, PurPoint_ex, PackSlip_ex) =>
                    (from row in ctx.RcvMisc
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.PurPoint == PurPoint_ex &&
                     row.PackSlip == PackSlip_ex &&
                     row.InvoiceNum == ""
                     select row)
                    .Any();
                existsLateCostRcvMiscQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsLateCostRcvMiscQuery(this.Db, Company, VendorNum, PurPoint, PackSlip);
        }

        private static Func<ErpContext, string, int, string, string, IEnumerable<RcvMisc>> selectAllRcvMiscQuery;
        private IEnumerable<RcvMisc> selectAllRcvMisc(string Company, int VendorNum, string PurPoint, string PackSlip)
        {
            if (selectAllRcvMiscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, IEnumerable<RcvMisc>>> expression =
                    (ctx, Company_ex, VendorNum_ex, PurPoint_ex, PackSlip_ex) =>
                    (from row in ctx.RcvMisc
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex &&
                     row.PurPoint == PurPoint_ex &&
                     row.PackSlip == PackSlip_ex &&
                     row.InvoiceNum == ""
                     select row);
                selectAllRcvMiscQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectAllRcvMiscQuery(this.Db, Company, VendorNum, PurPoint, PackSlip);
        }

        private static Func<ErpContext, string, string, PurMisc> selectMiscContainerMiscChargeQuery;
        private PurMisc SelectMiscContainerMiscCharge(string Company, string MiscCode)
        {
            if (selectMiscContainerMiscChargeQuery == null)
            {
                Expression<Func<ErpContext, string, string, PurMisc>> expression =
                    (ctx, Company_ex, MiscCode_ex) =>
                    (from row in ctx.PurMisc
                     where row.Company == Company_ex &&
                     row.MiscCode == MiscCode_ex
                     select row)
                    .FirstOrDefault();
                selectMiscContainerMiscChargeQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectMiscContainerMiscChargeQuery(this.Db, Company, MiscCode);
        }


        private static Func<ErpContext, string, int, string, string, int, decimal> getSumRcvMiscQuery;
        private decimal GetSumRcvMisc(string company, int vendorNum, string purPoint, string packSlip, int packLine)
        {
            if (getSumRcvMiscQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, decimal>> expression =
                    (ctx, company_ex, vendorNum_ex, purPoint_ex, packSlip_ex, packLine_ex) =>
                                    (from row in ctx.RcvMisc
                                     where row.Company == company_ex &&
                                           row.VendorNum == vendorNum_ex &&
                                           row.PurPoint == purPoint_ex &&
                                           row.PackSlip == packSlip_ex &&
                                           row.PackLine == packLine_ex
                                     group row by new { row.Company, row.VendorNum, row.PurPoint, row.PackSlip, row.PackLine } into g
                                     select g.Sum(result => result.DocActualAmt)).FirstOrDefault();
                getSumRcvMiscQuery = DBExpressionCompiler.Compile(expression);
            }

            return getSumRcvMiscQuery(this.Db, company, vendorNum, purPoint, packSlip, packLine);
        }
        #endregion APLateCosts

        #region PlasticPackTax

        private static Func<ErpContext, string, string, bool> existsAPInvMscPlasticPackTaxReportQuery;
        private bool ExistsAPInvMscPlasticPackTaxReport(string company, string plasticPackTaxReportID)
        {
            if (existsAPInvMscPlasticPackTaxReportQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                  (context, company_ex, plasticPackTaxReportID_ex) =>
                    (from row in context.APInvMsc
                     where row.Company == company_ex &&
                     row.PlasticPackTaxReportID == plasticPackTaxReportID_ex
                     select row).Any();
                existsAPInvMscPlasticPackTaxReportQuery = DBExpressionCompiler.Compile(expression);
            }
            return existsAPInvMscPlasticPackTaxReportQuery(this.Db, company, plasticPackTaxReportID);
        }

        private class RcvDtlPlasticPackTaxRow
        {
            public RcvDtl RcvDtl { get; set; }
            public string VendorName { get; set; }
        }

        static Func<ErpContext, string, int, string, string, int, RcvDtlPlasticPackTaxRow> findFirstRcvDtlPlasticPackTaxQuery;
        private RcvDtlPlasticPackTaxRow FindFirstRcvDtlPlasticPackTax(string company, int vendorNum, string purPoint, string packSlip, int packLine)
        {
            if (findFirstRcvDtlPlasticPackTaxQuery == null)
            {
                Expression<Func<ErpContext, string, int, string, string, int, RcvDtlPlasticPackTaxRow>> expression =
                  (ctx, company_ex, vendorNum_ex, purPoint_ex, packSlip_ex, packLine_ex) =>
                    (from RcvDtl_Row in ctx.RcvDtl
                     join Vendor_Row in ctx.Vendor on new { RcvDtl_Row.Company, RcvDtl_Row.VendorNum }
                                               equals new { Vendor_Row.Company, Vendor_Row.VendorNum }
                     where RcvDtl_Row.Company == company_ex &&
                     RcvDtl_Row.VendorNum == vendorNum_ex &&
                     RcvDtl_Row.PurPoint == purPoint_ex &&
                     RcvDtl_Row.PackSlip == packSlip_ex &&
                     RcvDtl_Row.PackLine == packLine_ex
                     select new RcvDtlPlasticPackTaxRow()
                     {
                         RcvDtl = RcvDtl_Row,
                         VendorName = Vendor_Row.Name
                     }).FirstOrDefault();
                findFirstRcvDtlPlasticPackTaxQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstRcvDtlPlasticPackTaxQuery(this.Db, company, vendorNum, purPoint, packSlip, packLine);
        }
        #endregion

        #region Vendor
        private static Func<ErpContext, string, int, bool> existsVendorQuery;
        private bool ExistsVendor(string Company, int VendorNum)
        {
            if (existsVendorQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool>> expression =
                    (context, Company_ex, VendorNum_ex) =>
                    (from row in context.Vendor
                     where row.Company == Company_ex &&
                     row.VendorNum == VendorNum_ex
                     select row)
                    .Any();
                existsVendorQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsVendorQuery(this.Db, Company, VendorNum);
        }
        #endregion Vendor

        #region Legal Number Generate
        static Func<ErpContext, string, string, string> getLegalNumGenerationTypeQuery;
        private string GetLegalNumGenerationType(string company, string tranDocTypeID)
        {
            if (getLegalNumGenerationTypeQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
                    (ctx, company_ex, tranDocTypeID_ex) =>
                    (from row in ctx.LegalNumDocType
                     join legalNumCnfgRow in ctx.LegalNumCnfg on
                        new { row.Company, row.LegalNumberID } equals
                        new { legalNumCnfgRow.Company, legalNumCnfgRow.LegalNumberID }
                     where row.Company == company_ex &&
                     row.TranDocTypeID == tranDocTypeID_ex
                     select legalNumCnfgRow).FirstOrDefault().GenerationType;
                getLegalNumGenerationTypeQuery = DBExpressionCompiler.Compile(expression);
            }
            return getLegalNumGenerationTypeQuery(this.Db, company, tranDocTypeID);
        }
        #endregion LegalNumber Generate

        #region LegalNumHistory
        private static Func<ErpContext, string, string, LegalNumHistory> findFirstLegalNumHistoryWithUpdLockQuery;
        private LegalNumHistory FindFirstLegalNumHistoryWithUpdLock(string company, string legalNumber)
        {
            if (findFirstLegalNumHistoryWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, LegalNumHistory>> expression =
                    (context, company_ex, legalNumber_ex) =>
                    (from row in context.LegalNumHistory.With(LockHint.UpdLock)
                     where row.Company == company_ex &&
                     row.LegalNumber == legalNumber_ex
                     select row)
                    .FirstOrDefault();
                findFirstLegalNumHistoryWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstLegalNumHistoryWithUpdLockQuery(this.Db, company, legalNumber);
        }
        #endregion LegalNumHistory
    }
}
