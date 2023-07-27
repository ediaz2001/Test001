using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Epicor.Data;
using Erp.Tables;

namespace Erp.Services.BO
{
    public partial class IssueReturnSvc
    {
        #region CCTag queries
        static Func<ErpContext, string, string, string, int, bool> existsCCTagQuery;
        private bool ExistsCCTag(string company, string partNum, string serialNumber, int tagStatus)
        {
            if (existsCCTagQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, bool>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tagStatus_ex) =>
        (from row in ctx.CCTag
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TagStatus == tagStatus_ex
         select row).Any();
                existsCCTagQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsCCTagQuery(this.Db, company, partNum, serialNumber, tagStatus);
        }
        #endregion CCTag queries

        #region DMRHead Queries
        static Func<ErpContext, string, int, DMRHead> findFirstDMRHeadWithUpdLockQuery;
        private DMRHead FindFirstDMRHeadWithUpdLock(string company, int dmrNum)
        {
            if (findFirstDMRHeadWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, DMRHead>> expression =
      (ctx, company_ex, dmrNum_ex) =>
        (from row in ctx.DMRHead.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.DMRNum == dmrNum_ex
         select row).FirstOrDefault();
                findFirstDMRHeadWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstDMRHeadWithUpdLockQuery(this.Db, company, dmrNum);
        }
        #endregion DMRHead Queries

        #region FSCallhd Queries

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


        static Func<ErpContext, string, int, FSCallhd> findFirstFSCallhdWithUpdLockQuery;
        private FSCallhd FindFirstFSCallhdWithUpdLock(string company, int callNum)
        {
            if (findFirstFSCallhdWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, FSCallhd>> expression =
      (ctx, company_ex, callNum_ex) =>
        (from row in ctx.FSCallhd.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.CallNum == callNum_ex &&
         !row.Invoiced == true
         select row).FirstOrDefault();
                findFirstFSCallhdWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstFSCallhdWithUpdLockQuery(this.Db, company, callNum);
        }
        #endregion FSCallhd Queries

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
        #endregion

        #region JobAsmbl Queries
        static Func<ErpContext, string, string, int, string> existsJobAsmblContractIDQuery;
        private string ExistsJobAsmblContractID(string company, string jobNum, int assmSeq)
        {
            if (existsJobAsmblContractIDQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string>> expression =
      (ctx, company_ex, jobNum_ex, assmSeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assmSeq_ex
         select row.ContractID).FirstOrDefault();
                existsJobAsmblContractIDQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobAsmblContractIDQuery(this.Db, company, jobNum, assmSeq);
        }


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
         row.AssemblySeq != assemblySeq_ex
         select row).Any();
                existsJobAsmblQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobAsmblQuery(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, int, int, int, bool> existsJobAsmblQuery_3;
        private bool ExistsJobAsmbl(string company, string jobNum, int assemblySeq, int finalOpr, int finalOpr2)
        {
            if (existsJobAsmblQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, finalOpr_ex, finalOpr2_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.FinalOpr != finalOpr_ex &&
         row.FinalOpr == finalOpr2_ex
         select row).Any();
                existsJobAsmblQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return existsJobAsmblQuery_3(this.Db, company, jobNum, assemblySeq, finalOpr, finalOpr2);
        }


        static Func<ErpContext, string, string, int, bool> existsJobAsmbl2Query;
        private bool ExistsJobAsmbl2(string company, string jobNum, int assemblySeq)
        {
            if (existsJobAsmbl2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq != assemblySeq_ex
         select row).Any();
                existsJobAsmbl2Query = DBExpressionCompiler.Compile(expression);
            }

            return existsJobAsmbl2Query(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, int, bool> existsJobAsmbl3Query;
        private bool ExistsJobAsmbl3(string company, string jobNum, int assemblySeq)
        {
            if (existsJobAsmbl3Query == null)
            {
                Expression<Func<ErpContext, string, string, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select row).Any();
                existsJobAsmbl3Query = DBExpressionCompiler.Compile(expression);
            }

            return existsJobAsmbl3Query(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, int, JobAsmbl> findFirstJobAsmblQuery;
        private JobAsmbl FindFirstJobAsmbl(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmblQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmbl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select row).FirstOrDefault();
                findFirstJobAsmblQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmblQuery(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, int, JobAsmbl> findFirstJobAsmblQuery_2;
        private JobAsmbl FindFirstJobAsmblDiffAsm(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmblQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmbl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq != assemblySeq_ex
         orderby row.AssemblySeq
         select row).FirstOrDefault();
                findFirstJobAsmblQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmblQuery_2(this.Db, company, jobNum, assemblySeq);
        }

        static Func<ErpContext, string, string, int, int> findFirstJobAsmblSeqQuery;
        private int FindFirstJobAsmblSeq(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmblSeqQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq > assemblySeq_ex
         select row.AssemblySeq).FirstOrDefault();
                findFirstJobAsmblSeqQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmblSeqQuery(this.Db, company, jobNum, assemblySeq);
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


        private class JobAsmblResult
        {
            public string Company { get; set; }
            public string JobNum { get; set; }
            public int Parent { get; set; }
            public int RelatedOperation { get; set; }
        }
        static Func<ErpContext, string, string, int, JobAsmblResult> findFirstJobAsmbl3Query;
        private JobAsmblResult FindFirstJobAsmbl3(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmbl3Query == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmblResult>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select new JobAsmblResult()
         {
             Company = row.Company,
             JobNum = row.JobNum,
             Parent = row.Parent,
             RelatedOperation = row.RelatedOperation
         }
   ).FirstOrDefault();
                findFirstJobAsmbl3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmbl3Query(this.Db, company, jobNum, assemblySeq);
        }

        private class JobAsmblResult2
        {
            public string PartNum { get; set; }
            public string Description { get; set; }
            public decimal IssuedQty { get; set; }
            public decimal PullQty { get; set; }
            public bool IssuedComplete { get; set; }
            public string IUM { get; set; }
            public int AttributeSetID { get; set; }
            public string RevisionNum { get; set; }
        }
        static Func<ErpContext, string, string, int, JobAsmblResult2> findFirstJobAsmblFLQuery;
        private JobAsmblResult2 FindFirstJobAsmblFL(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmblFLQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmblResult2>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select new JobAsmblResult2()
         {
             PartNum = row.PartNum,
             Description = row.Description,
             IssuedQty = row.IssuedQty,
             PullQty = row.PullQty,
             IssuedComplete = row.IssuedComplete,
             IUM = row.IUM,
             AttributeSetID = row.AttributeSetID,
             RevisionNum = row.RevisionNum

         }
   ).FirstOrDefault();
                findFirstJobAsmblFLQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmblFLQuery(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, int, JobAsmbl> findFirstJobAsmblWithUpdLockQuery;
        private JobAsmbl FindFirstJobAsmblWithUpdLock(string company, string jobNum, int assemblySeq)
        {
            if (findFirstJobAsmblWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, JobAsmbl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.JobAsmbl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex
         select row).FirstOrDefault();
                findFirstJobAsmblWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobAsmblWithUpdLockQuery(this.Db, company, jobNum, assemblySeq);
        }


        static Func<ErpContext, string, string, bool, IEnumerable<JobAsmbl>> selectJobAsmblQuery;
        private IEnumerable<JobAsmbl> SelectJobAsmbl(string company, string jobNum, bool jobComplete)
        {
            if (selectJobAsmblQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, IEnumerable<JobAsmbl>>> expression =
      (ctx, company_ex, jobNum_ex, jobComplete_ex) =>
        (from row in ctx.JobAsmbl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.JobComplete == jobComplete_ex
         orderby row.Company, row.JobNum, row.BomSequence
         select row);
                selectJobAsmblQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectJobAsmblQuery(this.Db, company, jobNum, jobComplete);
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
        #endregion JobHead Queries

        #region JobMtl Queries
        static Func<ErpContext, string, string, int, int, decimal> findJobMtlQtyPerQuery;
        private decimal FindJobMtlQtyPer(string company, string jobNum, int assm, int seq)
        {
            if (findJobMtlQtyPerQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, decimal>> expression =
      (ctx, company_ex, jobNum_ex, assm_ex, seq_ex) =>
        (from row in ctx.JobMtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assm_ex &&
         row.MtlSeq == seq_ex
         select row.QtyPer).FirstOrDefault();
                findJobMtlQtyPerQuery = DBExpressionCompiler.Compile(expression);
            }

            return findJobMtlQtyPerQuery(this.Db, company, jobNum, assm, seq);
        }

        static Func<ErpContext, string, string, bool> existsJobMtlQuery;
        private bool ExistsJobMtl(string company, string jobNum)
        {
            if (existsJobMtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, jobNum_ex) =>
        (from row in ctx.JobMtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex
         select row).Any();
                existsJobMtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobMtlQuery(this.Db, company, jobNum);
        }


        static Func<ErpContext, string, string, int, int, bool> existsJobMtlQuery3;
        private bool ExistsJobMtl(string company, string jobNum, int assemblySeq, int mtlSeq)
        {
            if (existsJobMtlQuery3 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex) =>
        (from row in ctx.JobMtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex
         select row).Any();
                existsJobMtlQuery3 = DBExpressionCompiler.Compile(expression);
            }

            return existsJobMtlQuery3(this.Db, company, jobNum, assemblySeq, mtlSeq);
        }


        static Func<ErpContext, string, string, int, int, bool, bool> existsJobMtlQuery2;
        private bool ExistsJobMtl(string company, string jobNum, int assemblySeq, int mtlSeq, bool miscCharge)
        {
            if (existsJobMtlQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, miscCharge_ex) =>
        (from row in ctx.JobMtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.MiscCharge == miscCharge_ex
         select row).Any();
                existsJobMtlQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return existsJobMtlQuery2(this.Db, company, jobNum, assemblySeq, mtlSeq, miscCharge);
        }


        static Func<ErpContext, string, string, int, int, JobMtl> findFirstJobMtlCNQuery;
        private JobMtl FindFirstJobMtlCN(string company, string jobNum, int assemblySeq, int mtlSeq)
        {
            if (findFirstJobMtlCNQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobMtl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex) =>
        (from row in ctx.JobMtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.CallNum != 0

         select row).FirstOrDefault();
                findFirstJobMtlCNQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobMtlCNQuery(this.Db, company, jobNum, assemblySeq, mtlSeq);
        }


        static Func<ErpContext, string, string, int, int, JobMtl> findFirstJobMtlQuery;
        private JobMtl FindFirstJobMtl(string company, string jobNum, int assemblySeq, int mtlSeq)
        {
            if (findFirstJobMtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobMtl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex) =>
        (from row in ctx.JobMtl
         where
         row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex
         select row).FirstOrDefault();
                findFirstJobMtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobMtlQuery(this.Db, company, jobNum, assemblySeq, mtlSeq);
        }


        static Func<ErpContext, string, string, int, int, string, string> findFirstJobMtl2Query;
        private string FindFirstJobMtl2(string company, string jobNum, int assemblySeq, int mtlSeq, string partNum)
        {
            if (findFirstJobMtl2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, string>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, partNum_ex) =>
        (from row in ctx.JobMtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.PartNum == partNum_ex
         select row.WarehouseCode).FirstOrDefault();
                findFirstJobMtl2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobMtl2Query(this.Db, company, jobNum, assemblySeq, mtlSeq, partNum);
        }

        static Func<ErpContext, string, string, int, int, string> existsJobMtlContractIDQuery;
        private string ExistsJobMtlContractID(string company, string jobNum, int assmSeq, int mtlSeq)
        {
            if (existsJobMtlContractIDQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string>> expression =
      (ctx, company_ex, jobNum_ex, assmSeq_ex, mtlSeq_ex) =>
        (from row in ctx.JobMtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assmSeq_ex &&
         row.MtlSeq == mtlSeq_ex
         select row.ContractID).FirstOrDefault();
                existsJobMtlContractIDQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsJobMtlContractIDQuery(this.Db, company, jobNum, assmSeq, mtlSeq);
        }


        private class altJobMtlResult
        {
            public string Company { get; set; }
            public string JobNum { get; set; }
            public int AssemblySeq { get; set; }
            public int RelatedOperation { get; set; }
            public string PartNum { get; set; }
            public string IUM { get; set; }

        }
        static Func<ErpContext, string, string, int, int, altJobMtlResult> findFirstJobMtl3Query;
        private altJobMtlResult FindFirstJobMtl3(string company, string jobNum, int assemblySeq, int mtlSeq)
        {
            if (findFirstJobMtl3Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, altJobMtlResult>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex) =>
        (from row in ctx.JobMtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex
         select new altJobMtlResult()
         {
             Company = row.Company,
             JobNum = row.JobNum,
             AssemblySeq = row.AssemblySeq,
             RelatedOperation = row.RelatedOperation,
             PartNum = row.PartNum,
             IUM = row.IUM
         }
   ).FirstOrDefault();
                findFirstJobMtl3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobMtl3Query(this.Db, company, jobNum, assemblySeq, mtlSeq);
        }

        private class bJobMtlResult
        {
            public string WarehouseCode { get; set; }
        }
        static Func<ErpContext, string, string, int, int, bJobMtlResult> findFirstJobMtl10Query;
        private bJobMtlResult FindFirstJobMtl10(string company, string jobNum, int assemblySeq, int mtlSeq)
        {
            if (findFirstJobMtl10Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bJobMtlResult>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex) =>
        (from row in ctx.JobMtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex
         select new bJobMtlResult()
         {
             WarehouseCode = row.WarehouseCode
         }
   ).FirstOrDefault();
                findFirstJobMtl10Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobMtl10Query(this.Db, company, jobNum, assemblySeq, mtlSeq);
        }


        static Func<ErpContext, string, string, int, int, JobMtl> findFirstJobMtlWithUpdLockQuery;
        private JobMtl FindFirstJobMtlWithUpdLock(string company, string jobNum, int assemblySeq, int mtlSeq)
        {
            if (findFirstJobMtlWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobMtl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex) =>
        (from row in ctx.JobMtl.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex
         select row).FirstOrDefault();
                findFirstJobMtlWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobMtlWithUpdLockQuery(this.Db, company, jobNum, assemblySeq, mtlSeq);
        }

        #endregion JobMtl Queries

        #region JobOpDtl Queries

        static Func<ErpContext, string, string, int, int, int, JobOpDtl> findFirstJobOpDtlQuery;
        private JobOpDtl FindFirstJobOpDtl(string company, string jobNum, int assemblySeq, int oprSeq, int opDtlSeq)
        {
            if (findFirstJobOpDtlQuery == null)
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
                findFirstJobOpDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOpDtlQuery(this.Db, company, jobNum, assemblySeq, oprSeq, opDtlSeq);
        }

        static Func<ErpContext, string, string, int, int, JobOpDtl> findFirstJobOpDtlQuery_2;
        private JobOpDtl FindFirstJobOpDtl(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOpDtlQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOpDtl>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOpDtl
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOpDtlQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOpDtlQuery_2(this.Db, company, jobNum, assemblySeq, oprSeq);
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

        static Func<ErpContext, string, string, int, int, bool> existsJobOperQuery2;
        private bool ExistsJobOper2(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (existsJobOperQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).Any();
                existsJobOperQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return existsJobOperQuery2(this.Db, company, jobNum, assemblySeq, oprSeq);
        }



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
        Param_ex
         select row).FirstOrDefault();
                findFirstJobOperQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOperQuery_2(this.Db, company, jobNum, assemblySeq, oprSeq, Param);
        }


        static Func<ErpContext, string, int, int, string, JobOper> findFirstJobOperQuery_3;
        private JobOper FindFirstJobOper(string company, int oprSeq, int assemblySeq, string jobNum)
        {
            if (findFirstJobOperQuery_3 == null)
            {
                Expression<Func<ErpContext, string, int, int, string, JobOper>> expression =
      (ctx, company_ex, oprSeq_ex, assemblySeq_ex, jobNum_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.OprSeq == oprSeq_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstJobOperQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOperQuery_3(this.Db, company, oprSeq, assemblySeq, jobNum);
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
         row.OprSeq > oprSeq_ex
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
         row.OprSeq > oprSeq_ex
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


        private class JobOperResult
        {
            public string JobNum { get; set; }
            public int AssemblySeq { get; set; }
            public int OprSeq { get; set; }
            public int PrimaryProdOpDtl { get; set; }
            public bool SubContract { get; set; }
        }
        static Func<ErpContext, string, string, int, int, JobOperResult> findFirstJobOper2Query;
        private JobOperResult FindFirstJobOper2(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOperResult>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select new JobOperResult()
         {
             JobNum = row.JobNum,
             AssemblySeq = row.AssemblySeq,
             OprSeq = row.OprSeq,
             PrimaryProdOpDtl = row.PrimaryProdOpDtl,
             SubContract = row.SubContract
         }
   ).FirstOrDefault();
                findFirstJobOper2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper2Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, bool, JobOper> findFirstJobOper2Query_2;
        private JobOper FindFirstJobOper2(string company, string jobNum, int assemblySeq, int oprSeq, bool Param)
        {
            if (findFirstJobOper2Query_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, Param_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq > oprSeq_ex &&
        Param_ex
         select row).FirstOrDefault();
                findFirstJobOper2Query_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper2Query_2(this.Db, company, jobNum, assemblySeq, oprSeq, Param);
        }


        private class JobOperResult2
        {
            public string JobNum { get; set; }
            public int AssemblySeq { get; set; }
            public int OprSeq { get; set; }
            public int PrimaryProdOpDtl { get; set; }
        }
        static Func<ErpContext, string, string, int, int, JobOperResult2> findFirstJobOper3Query;
        private JobOperResult2 FindFirstJobOper3(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper3Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOperResult2>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq > oprSeq_ex
         select new JobOperResult2()
         {
             JobNum = row.JobNum,
             AssemblySeq = row.AssemblySeq,
             OprSeq = row.OprSeq,
             PrimaryProdOpDtl = row.PrimaryProdOpDtl
         }
   ).FirstOrDefault();
                findFirstJobOper3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper3Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, bool, JobOper> findFirstJobOper3Query_2;
        private JobOper FindFirstJobOper3(string company, string jobNum, int assemblySeq, int oprSeq, bool Param)
        {
            if (findFirstJobOper3Query_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, bool, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, Param_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq >= oprSeq_ex &&
        Param_ex
         select row).FirstOrDefault();
                findFirstJobOper3Query_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper3Query_2(this.Db, company, jobNum, assemblySeq, oprSeq, Param);
        }


        private class JobOperResult3
        {
            public string JobNum { get; set; }
            public int AssemblySeq { get; set; }
            public int OprSeq { get; set; }
            public int PrimaryProdOpDtl { get; set; }
        }
        static Func<ErpContext, string, string, int, int, JobOperResult3> findFirstJobOper4Query;
        private JobOperResult3 FindFirstJobOper4(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper4Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOperResult3>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select new JobOperResult3()
         {
             JobNum = row.JobNum,
             AssemblySeq = row.AssemblySeq,
             OprSeq = row.OprSeq,
             PrimaryProdOpDtl = row.PrimaryProdOpDtl
         }
   ).FirstOrDefault();
                findFirstJobOper4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper4Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        private class JobOperResult4
        {
            public string JobNum { get; set; }
            public int AssemblySeq { get; set; }
            public int OprSeq { get; set; }
            public int PrimaryProdOpDtl { get; set; }
        }
        static Func<ErpContext, string, string, int, int, JobOperResult4> findFirstJobOper5Query;
        private JobOperResult4 FindFirstJobOper5(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper5Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOperResult4>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq > oprSeq_ex
         select new JobOperResult4()
         {
             JobNum = row.JobNum,
             AssemblySeq = row.AssemblySeq,
             OprSeq = row.OprSeq,
             PrimaryProdOpDtl = row.PrimaryProdOpDtl
         }
   ).FirstOrDefault();
                findFirstJobOper5Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper5Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        private class JobOperResult5
        {
            public string JobNum { get; set; }
            public int AssemblySeq { get; set; }
            public int OprSeq { get; set; }
            public int PrimaryProdOpDtl { get; set; }
        }
        static Func<ErpContext, string, string, int, int, JobOperResult5> findFirstJobOper6Query;
        private JobOperResult5 FindFirstJobOper6(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper6Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOperResult5>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select new JobOperResult5()
         {
             JobNum = row.JobNum,
             AssemblySeq = row.AssemblySeq,
             OprSeq = row.OprSeq,
             PrimaryProdOpDtl = row.PrimaryProdOpDtl
         }
   ).FirstOrDefault();
                findFirstJobOper6Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper6Query(this.Db, company, jobNum, assemblySeq, oprSeq);
        }


        static Func<ErpContext, string, string, int, int, JobOper> findFirstJobOper7Query;
        private JobOper FindFirstJobOper7(string company, string jobNum, int assemblySeq, int oprSeq)
        {
            if (findFirstJobOper7Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, JobOper>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex) =>
        (from row in ctx.JobOper
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstJobOper7Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstJobOper7Query(this.Db, company, jobNum, assemblySeq, oprSeq);
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
         row.OprSeq > oprSeq_ex
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
        #endregion JobOper Queries

        #region MtlQueue Queries
        static Func<ErpContext, Guid, string, bool> existsMtlQueueQuery;
        private bool ExistsMtlQueue(Guid sysRowID, string tranType)
        {
            if (existsMtlQueueQuery == null)
            {
                Expression<Func<ErpContext, Guid, string, bool>> expression =
      (ctx, sysRowID_ex, tranType_ex) =>
        (from row in ctx.MtlQueue
         where row.SysRowID == sysRowID_ex &&
         row.TranType == tranType_ex
         select row).Any();
                existsMtlQueueQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsMtlQueueQuery(this.Db, sysRowID, tranType);
        }

        static Func<ErpContext, Guid, MtlQueue> findFirstMtlQueueQuery;
        private MtlQueue FindFirstMtlQueue(Guid sysRowID)
        {
            if (findFirstMtlQueueQuery == null)
            {
                Expression<Func<ErpContext, Guid, MtlQueue>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.MtlQueue
         where row.SysRowID == sysRowID_ex
         select row).FirstOrDefault();
                findFirstMtlQueueQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstMtlQueueQuery(this.Db, sysRowID);
        }

        static Func<ErpContext, string, int, string, int, string, int, int, int, int, int, MtlQueue> findFirstMtlQueueQuery_2;
        private MtlQueue FindFirstMtlQueue(string company, int assemblySeq, string jobSeqType, int jobSeq, string tranType, int nextAssemblySeq, int nextJobSeq, int orderNum, int orderLine, int orderRelNum)
        {
            if (findFirstMtlQueueQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, string, int, string, int, int, int, int, int, MtlQueue>> expression =
      (ctx, company_ex, assemblySeq_ex, jobSeqType_ex, jobSeq_ex, tranType_ex, nextAssemblySeq_ex, nextJobSeq_ex, orderNum_ex, orderLine_ex, orderRelNum_ex) =>
        (from row in ctx.MtlQueue
         where row.Company == company_ex &&
         row.JobNum == "" &&
         row.AssemblySeq == assemblySeq_ex &&
         row.JobSeqType == jobSeqType_ex &&
         row.JobSeq == jobSeq_ex &&
         row.TranType == tranType_ex &&
         row.NextAssemblySeq == nextAssemblySeq_ex &&
         row.NextJobSeq == nextJobSeq_ex &&
         row.OrderNum == orderNum_ex &&
         row.OrderLine == orderLine_ex &&
         row.OrderRelNum == orderRelNum_ex
         select row).FirstOrDefault();
                findFirstMtlQueueQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstMtlQueueQuery_2(this.Db, company, assemblySeq, jobSeqType, jobSeq, tranType, nextAssemblySeq, nextJobSeq, orderNum, orderLine, orderRelNum);
        }

        private class MtlQueueResult
        {
            public string PartNum { get; set; }
            public int AttributeSetID { get; set; }
            public string FromWhse { get; set; }
            public string FromBinNum { get; set; }
            public string LotNum { get; set; }
            public string IUM { get; set; }
            public string FromPCID { get; set; }
            public decimal Quantity { get; set; }
        }
        static Func<ErpContext, Guid, MtlQueueResult> findFirstMtlQueue4Query;
        private MtlQueueResult FindFirstMtlQueue4(Guid sysRowID)
        {
            if (findFirstMtlQueue4Query == null)
            {
                Expression<Func<ErpContext, Guid, MtlQueueResult>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.MtlQueue
         where row.SysRowID == sysRowID_ex
         select new MtlQueueResult()
         {
             PartNum = row.PartNum,
             AttributeSetID = row.AttributeSetID,
             FromWhse = row.FromWhse,
             FromBinNum = row.FromBinNum,
             LotNum = row.LotNum,
             IUM = row.IUM,
             Quantity = row.Quantity
         }
   ).FirstOrDefault();
                findFirstMtlQueue4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstMtlQueue4Query(this.Db, sysRowID);
        }

        static Func<ErpContext, Guid, MtlQueue> findFirstMtlQueueWithUpdLockQuery;
        private MtlQueue FindFirstMtlQueueWithUpdLock(Guid sysRowID)
        {
            if (findFirstMtlQueueWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, Guid, MtlQueue>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.MtlQueue.With(LockHint.UpdLock)
         where row.SysRowID == sysRowID_ex
         select row).FirstOrDefault();
                findFirstMtlQueueWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstMtlQueueWithUpdLockQuery(this.Db, sysRowID);
        }

        static Func<ErpContext, string, string, int, int, int, MtlQueue> findFirstMtlQueueWithUpdLock2Query;
        private MtlQueue FindFirstMtlQueueWithUpdLock(string company, string partNum, int orderNum, int orderLine, int orderRel)
        {
            if (findFirstMtlQueueWithUpdLock2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, MtlQueue>> expression =
      (ctx, company_ex, partNum_ex, orderNum_ex, orderLine_ex, orderRel_ex) =>
        (from row in ctx.MtlQueue.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.OrderNum == orderNum_ex &&
         row.OrderLine == orderLine_ex &&
         row.OrderRelNum == orderRel_ex
         select row).FirstOrDefault();
                findFirstMtlQueueWithUpdLock2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstMtlQueueWithUpdLock2Query(this.Db, company, partNum, orderNum, orderLine, orderRel);
        }

        static Func<ErpContext, string, string, string, string, string, IEnumerable<MtlQueue>> selectMtlQueueQuery;
        private IEnumerable<MtlQueue> SelectMtlQueue(string company, string plant, string partNum, string referencePrefix, string reference)
        {
            if (selectMtlQueueQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, IEnumerable<MtlQueue>>> expression =
      (ctx, company_ex, plant_ex, partNum_ex, referencePrefix_ex, reference_ex) =>
        (from row in ctx.MtlQueue
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex &&
         row.ReferencePrefix == referencePrefix_ex &&
         row.Reference == reference_ex
         orderby row.MtlQueueSeq
         select row);
                selectMtlQueueQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectMtlQueueQuery(this.Db, company, plant, partNum, referencePrefix, reference);
        }

        static Func<ErpContext, string, string, string, string, string, string, IEnumerable<MtlQueue>> selectMtlQueuePCIDPickContentsWithUpdLockQuery;
        private IEnumerable<MtlQueue> SelectMtlQueuePCIDPickContentsWithUpdLock(string company, string plant, string fromWhse, string fromBinNum, string referencePrefix, string reference)
        {
            if (selectMtlQueuePCIDPickContentsWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, string, IEnumerable<MtlQueue>>> expression =
      (ctx, company_ex, plant_ex, fromWhse_ex, fromBinNum_ex, referencePrefix_ex, reference_ex) =>
        (from row in ctx.MtlQueue.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.FromWhse == fromWhse_ex &&
         row.FromBinNum == fromBinNum_ex &&
         row.ReferencePrefix == referencePrefix_ex &&
         row.Reference == reference_ex
         select row);
                selectMtlQueuePCIDPickContentsWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectMtlQueuePCIDPickContentsWithUpdLockQuery(this.Db, company, plant, fromWhse, fromBinNum, referencePrefix, reference);
        }

        static Func<ErpContext, string, string, int, int, int, IEnumerable<MtlQueue>> selectMtlQueueQuery_2;
        private IEnumerable<MtlQueue> SelectMtlQueue(string company, string queueID, int orderNum, int queuePickSeq, int orderRelNum)
        {
            if (selectMtlQueueQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, IEnumerable<MtlQueue>>> expression =
      (ctx, company_ex, queueID_ex, orderNum_ex, queuePickSeq_ex, orderRelNum_ex) =>
        (from row in ctx.MtlQueue
         where row.Company == company_ex &&
         row.QueueID == queueID_ex &&
         row.OrderNum == orderNum_ex &&
         row.QueuePickSeq == queuePickSeq_ex &&
         row.OrderRelNum == orderRelNum_ex
         select row);
                selectMtlQueueQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return selectMtlQueueQuery_2(this.Db, company, queueID, orderNum, queuePickSeq, orderRelNum);
        }

        static Func<ErpContext, string, string, string, string, string, IEnumerable<MtlQueue>> selectMtlQueueWithUpdLockQuery;
        private IEnumerable<MtlQueue> SelectMtlQueueWithUpdLock(string company, string plant, string partNum, string referencePrefix, string reference)
        {
            if (selectMtlQueueWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, IEnumerable<MtlQueue>>> expression =
      (ctx, company_ex, plant_ex, partNum_ex, referencePrefix_ex, reference_ex) =>
        (from row in ctx.MtlQueue.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex &&
         row.ReferencePrefix == referencePrefix_ex &&
         row.Reference == reference_ex
         orderby row.MtlQueueSeq
         select row);
                selectMtlQueueWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectMtlQueueWithUpdLockQuery(this.Db, company, plant, partNum, referencePrefix, reference);
        }

        private class MtlQueueNonConf
        {
            public MtlQueue MtlQueue { get; set; }
            public NonConf NonConf { get; set; }
        }

        static Func<ErpContext, Guid, MtlQueueNonConf> findFirstMtlQueueNonConfQuery;
        private MtlQueueNonConf FindFirstMtlQueueNonConfQuery(Guid sysRowID)
        {
            if (findFirstMtlQueueNonConfQuery == null)
            {
                Expression<Func<ErpContext, Guid, MtlQueueNonConf>> expression =
      (ctx, sysRowID_ex) =>
        (from row_MtlQueue in ctx.MtlQueue
         join row_NonConf in ctx.NonConf on new { row_MtlQueue.Company, TranID = row_MtlQueue.NCTranID } equals new { row_NonConf.Company, TranID = row_NonConf.TranID }
         where row_MtlQueue.SysRowID == sysRowID_ex
         select new MtlQueueNonConf { MtlQueue = row_MtlQueue, NonConf = row_NonConf }).FirstOrDefault();
                findFirstMtlQueueNonConfQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstMtlQueueNonConfQuery(this.Db, sysRowID);
        }

        private class MtlQueueResult2
        {
            public string ToWhse { get; set; }
            public string ToBinNum { get; set; }
        }
        static Func<ErpContext, Guid, MtlQueueResult2> findFirstMtlQueueToWhsBinQuery;
        private MtlQueueResult2 FindFirstMtlQueueToWhseBin(Guid sysRowID)
        {
            if (findFirstMtlQueueToWhsBinQuery == null)
            {
                Expression<Func<ErpContext, Guid, MtlQueueResult2>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.MtlQueue
         where row.SysRowID == sysRowID_ex &&
         row.ToWhse != String.Empty
         select new MtlQueueResult2()
         {
             ToWhse = row.ToWhse,
             ToBinNum = row.ToBinNum,
         }).FirstOrDefault();
                findFirstMtlQueueToWhsBinQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstMtlQueueToWhsBinQuery(this.Db, sysRowID);
        }

        #endregion MtlQueue Queries

        #region NonConf Queries

        static Func<ErpContext, string, int, NonConf> findFirstNonConfQuery;
        private NonConf FindFirstNonConf(string company, int tranID)
        {
            if (findFirstNonConfQuery == null)
            {
                Expression<Func<ErpContext, string, int, NonConf>> expression =
      (ctx, company_ex, tranID_ex) =>
        (from row in ctx.NonConf
         where row.Company == company_ex &&
         row.TranID == tranID_ex
         select row).FirstOrDefault();
                findFirstNonConfQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstNonConfQuery(this.Db, company, tranID);
        }


        static Func<ErpContext, string, int, NonConf> findFirstNonConfWithUpdLockQuery;
        private NonConf FindFirstNonConfWithUpdLock(string company, int tranID)
        {
            if (findFirstNonConfWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, NonConf>> expression =
      (ctx, company_ex, tranID_ex) =>
        (from row in ctx.NonConf.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.TranID == tranID_ex
         select row).FirstOrDefault();
                findFirstNonConfWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstNonConfWithUpdLockQuery(this.Db, company, tranID);
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
        #endregion OpMaster Queries

        #region OrderHed Queries
        static Func<ErpContext, string, int, OrderHed> findFirstOrderHedQuery;
        private OrderHed FindFirstOrderHed(string ipCompany, int orderNum)
        {
            if (findFirstOrderHedQuery == null)
            {
                Expression<Func<ErpContext, string, int, OrderHed>> expression =
                  (ctx, company_ex, orderNum_ex) =>
                    (from row in ctx.OrderHed
                     where row.Company == company_ex &&
                     row.OrderNum == orderNum_ex
                     select row).FirstOrDefault();
                findFirstOrderHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOrderHedQuery(this.Db, ipCompany, orderNum);
        }

        private class OrderHedOTSInfoResult
        {
            public bool UseOTS { get; set; }
            public string OTSZIP { get; set; }
            public string OTSName { get; set; }
        }
        static Func<ErpContext, string, int, OrderHedOTSInfoResult> findFirstOrderHedOTSQuery;
        private OrderHedOTSInfoResult FindFirstOrderHedOTS(string company, int orderNum)
        {
            if (findFirstOrderHedOTSQuery == null)
            {
                Expression<Func<ErpContext, string, int, OrderHedOTSInfoResult>> expression =
      (ctx, company_ex, orderNum_ex) =>
        (from row in ctx.OrderHed
         where row.Company == company_ex &&
         row.OrderNum == orderNum_ex
         select new OrderHedOTSInfoResult()
         {
             UseOTS = row.UseOTS,
             OTSName = row.OTSName,
             OTSZIP = row.OTSZIP
         }
   ).FirstOrDefault();
                findFirstOrderHedOTSQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOrderHedOTSQuery(this.Db, company, orderNum);
        }

        class OrderHedPartialRow
        {
            public bool ERSOrder { get; set; }
            public int OrderNum { get; set; }

        }

        static Func<ErpContext, string, int, OrderHedPartialRow> findFirstOrderHedERSOrderQuery;
        private OrderHedPartialRow FindFirstOrderHedERSOrder(string company, int orderNum)
        {
            if (findFirstOrderHedERSOrderQuery == null)
            {
                Expression<Func<ErpContext, string, int, OrderHedPartialRow>> expression =
      (ctx, company_ex, orderNum_ex) =>
        (from row in ctx.OrderHed
         where row.Company == company_ex &&
         row.OrderNum == orderNum_ex
         select new OrderHedPartialRow() { ERSOrder = row.ERSOrder, OrderNum = row.OrderNum }).FirstOrDefault();
                findFirstOrderHedERSOrderQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOrderHedERSOrderQuery(this.Db, company, orderNum);
        }
        #endregion

        #region OrderDtl Queries


        static Func<ErpContext, string, int, int, string, OrderDtl> findFirstOrderDtlQuery;
        private OrderDtl FindFirstOrderDtl(string company, int orderNum, int kitParentLine, string kitFlag)
        {
            if (findFirstOrderDtlQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, string, OrderDtl>> expression =
      (ctx, company_ex, orderNum_ex, kitParentLine_ex, kitFlag_ex) =>
        (from row in ctx.OrderDtl
         where row.Company == company_ex &&
         row.OrderNum == orderNum_ex &&
         row.KitParentLine == kitParentLine_ex &&
         row.KitFlag == kitFlag_ex
         select row).FirstOrDefault();
                findFirstOrderDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOrderDtlQuery(this.Db, company, orderNum, kitParentLine, kitFlag);
        }


        static Func<ErpContext, string, int, int, OrderDtl> findFirstOrderDtlQuery_2;
        private OrderDtl FindFirstOrderDtl(string company, int orderNum, int orderLine)
        {
            if (findFirstOrderDtlQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, int, OrderDtl>> expression =
      (ctx, company_ex, orderNum_ex, orderLine_ex) =>
        (from row in ctx.OrderDtl
         where row.Company == company_ex &&
         row.OrderNum == orderNum_ex &&
         row.OrderLine == orderLine_ex
         select row).FirstOrDefault();
                findFirstOrderDtlQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOrderDtlQuery_2(this.Db, company, orderNum, orderLine);
        }
        #endregion OrderDtl Queries

        #region OrderRel Queries
        static Func<ErpContext, string, int, int, int, OrderRel> findFirstOrderRelQuery;
        private OrderRel FindFirstOrderRel(string company, int orderNum, int orderLine, int orderRel)
        {
            if (findFirstOrderRelQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, OrderRel>> expression =
      (ctx, company_ex, orderNum_ex, orderLine_ex, orderRel_ex) =>
        (from row in ctx.OrderRel
         where row.Company == company_ex &&
         row.OrderNum == orderNum_ex &&
         row.OrderLine == orderLine_ex &&
         row.OrderRelNum == orderRel_ex
         select row).FirstOrDefault();
                findFirstOrderRelQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOrderRelQuery(this.Db, company, orderNum, orderLine, orderRel);
        }

        private class OrderRelOTSInfoResult
        {
            public bool UseOTS { get; set; }
            public string OTSZIP { get; set; }
            public string OTSName { get; set; }
            public int OrderNum { get; set; }
            public string ShipToNum { get; set; }
        }
        static Func<ErpContext, string, int, int, int, OrderRelOTSInfoResult> findFirstOrderRelOTSQuery;
        private OrderRelOTSInfoResult FindFirstOrderRelOTS(string company, int orderNum, int orderLine, int orderRelNum)
        {
            if (findFirstOrderRelOTSQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, OrderRelOTSInfoResult>> expression =
      (ctx, company_ex, orderNum_ex, orderLine_ex, orderRelNum_ex) =>
        (from row in ctx.OrderRel
         where row.Company == company_ex &&
         row.OrderNum == orderNum_ex &&
         row.OrderLine == orderLine_ex &&
         row.OrderRelNum == orderRelNum_ex
         select new OrderRelOTSInfoResult()
         {
             UseOTS = row.UseOTS,
             OTSName = row.OTSName,
             OTSZIP = row.OTSZIP,
             OrderNum = row.OrderNum,
             ShipToNum = row.ShipToNum
         }
   ).FirstOrDefault();
                findFirstOrderRelOTSQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstOrderRelOTSQuery(this.Db, company, orderNum, orderLine, orderRelNum);
        }

        #endregion

        #region Part Queries
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

        static Func<ErpContext, string, string, bool> existsPartTrackSerialNumQuery;
        private bool ExistsPartTrackSerialNum(string company, string partNum)
        {
            if (existsPartTrackSerialNumQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
              (ctx, company_ex, partNum_ex) =>
                (from row in ctx.Part
                 where row.Company == company_ex &&
                 row.PartNum == partNum_ex
                 select row.TrackSerialNum).FirstOrDefault();
                existsPartTrackSerialNumQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPartTrackSerialNumQuery(this.Db, company, partNum);
        }


        static Func<ErpContext, string, string, bool, bool> existsInactivePartQuery;
        private bool ExistsInactivePart(string company, string partNum, bool inactive)
        {
            if (existsInactivePartQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
      (ctx, company_ex, partNum_ex, inactive_ex) =>
        (from row in ctx.Part
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.InActive == inactive_ex
         select row).
         Any();
                existsInactivePartQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsInactivePartQuery(this.Db, company, partNum, inactive);
        }

        static Func<ErpContext, string, string, string> existsUOMClassForPartQuery;
        private string ExistsUOMClassForPart(string company, string partNum)
        {
            if (existsUOMClassForPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
      (ctx, company_ex, partNum_ex) =>
        (from row in ctx.Part
         where row.Company == company_ex &&
         row.PartNum == partNum_ex
         select row.UOMClassID).FirstOrDefault();
                existsUOMClassForPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsUOMClassForPartQuery(this.Db, company, partNum);
        }

        private class PartPartialRow : Epicor.Data.TempRowBase
        {
            public string AttrClassID { get; set; }
            public bool TrackInventoryByRevision { get; set; }
            public bool TrackInventoryAttributes { get; set; }
        }

        private static Func<ErpContext, string, string, PartPartialRow> findFirstPartPartialQuery;
        private PartPartialRow FindFirstPartPartial(string company, string partNum)
        {
            if (findFirstPartPartialQuery == null)
            {
                Expression<Func<ErpContext, string, string, PartPartialRow>> expression =
                    (context, company_ex, partNum_ex) =>
                    (from row in context.Part
                     where row.Company == company_ex &&
                     row.PartNum == partNum_ex
                     select new PartPartialRow
                     {
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

        static Func<ErpContext, string, string, bool> isPartPartDimTrkdQuery;
        private bool IsDimTracked(string company, string partNum)
        {
            if (isPartPartDimTrkdQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, partNum_ex) =>
        (from row in ctx.Part
         where row.Company == company_ex &&
         row.PartNum == partNum_ex
         select row.TrackDimension).FirstOrDefault();
                isPartPartDimTrkdQuery = DBExpressionCompiler.Compile(expression);
            }

            return isPartPartDimTrkdQuery(this.Db, company, partNum);
        }
        #endregion Part Queries

        #region PartAlloc Queries
        static Func<ErpContext, string, string, int, string, string, string, string, string, string, int, bool, PartAlloc> findFirstPartAllocWithUpdLockQuery;
        private PartAlloc FindFirstPartAllocWithUpdLock(string company, string partNum, int attributeSetID, string warehouseCode, string binNum, string lotNum, string pcid, string dimCode, string tfordNum, int tfordLine, bool hardAllocation)
        {
            if (findFirstPartAllocWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, string, string, string, string, int, bool, PartAlloc>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, warehouseCode_ex, binNum_ex, lotNum_ex, pcid_ex, dimCode_ex, tfordNum_ex, tfordLine_ex, hardAllocation_ex) =>
        (from row in ctx.PartAlloc.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.BinNum == binNum_ex &&
         row.LotNum == lotNum_ex &&
         row.DimCode == dimCode_ex &&
         row.PCID == pcid_ex &&
         row.TFOrdNum == tfordNum_ex &&
         row.TFOrdLine == tfordLine_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.HardAllocation == hardAllocation_ex
         select row).FirstOrDefault();
                findFirstPartAllocWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartAllocWithUpdLockQuery(this.Db, company, partNum, attributeSetID, warehouseCode, binNum, lotNum, pcid, dimCode, tfordNum, tfordLine, hardAllocation);
        }

        static Func<ErpContext, string, string, int, string, string, string, string, int, bool, PartAlloc> findFirstPartAllocWithUpdLockQuery_2;
        private PartAlloc FindFirstPartAllocWithUpdLock(string company, string partNum, int attributeSetID, string warehouseCode, string lotNum, string dimCode, string tfordNum, int tfordLine, bool hardAllocation)
        {
            if (findFirstPartAllocWithUpdLockQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, string, string, int, bool, PartAlloc>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, warehouseCode_ex, lotNum_ex, dimCode_ex, tfordNum_ex, tfordLine_ex, hardAllocation_ex) =>
        (from row in ctx.PartAlloc.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.LotNum == lotNum_ex &&
         row.DimCode == dimCode_ex &&
         row.TFOrdNum == tfordNum_ex &&
         row.TFOrdLine == tfordLine_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.BinNum != "" &&
         row.HardAllocation == hardAllocation_ex
         select row).FirstOrDefault();
                findFirstPartAllocWithUpdLockQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartAllocWithUpdLockQuery_2(this.Db, company, partNum, attributeSetID, warehouseCode, lotNum, dimCode, tfordNum, tfordLine, hardAllocation);
        }

        static Func<ErpContext, string, string, int, string, string, string, string, string, string, int, string, bool, PartAlloc> findFirstPartAllocWithUpdLockQuery_3;
        private PartAlloc FindFirstPartAllocWithUpdLock(string company, string partNum, int attributeSetID, string warehouseCode, string binNum, string lotNum, string pcid, string dimCode, string tfordNum, int tfordLine, string demandType, bool hardAllocation)
        {
            if (findFirstPartAllocWithUpdLockQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, string, string, string, string, int, string, bool, PartAlloc>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, warehouseCode_ex, binNum_ex, lotNum_ex, pcid_ex, dimCode_ex, tfordNum_ex, tfordLine_ex, demandType_ex, hardAllocation_ex) =>
        (from row in ctx.PartAlloc.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.BinNum == binNum_ex &&
         row.LotNum == lotNum_ex &&
         row.DimCode == dimCode_ex &&
         row.PCID == pcid_ex &&
         row.TFOrdNum == tfordNum_ex &&
         row.TFOrdLine == tfordLine_ex &&
         row.DemandType == demandType_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.HardAllocation == hardAllocation_ex
         select row).FirstOrDefault();
                findFirstPartAllocWithUpdLockQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartAllocWithUpdLockQuery_3(this.Db, company, partNum, attributeSetID, warehouseCode, binNum, lotNum, pcid, dimCode, tfordNum, tfordLine, demandType, hardAllocation);
        }

        static Func<ErpContext, string, string, int, string, string, string, int, PartAlloc> findFirstPartAllocWithUpdLockQuery_4;
        private PartAlloc FindFirstPartAllocWithUpdLock(string company, string partNum, int attributeSetID, string warehouseCode, string dimCode, string tfordNum, int tfordLine)
        {
            if (findFirstPartAllocWithUpdLockQuery_4 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, string, int, PartAlloc>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, warehouseCode_ex, dimCode_ex, tfordNum_ex, tfordLine_ex) =>
        (from row in ctx.PartAlloc.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.BinNum == "" &&
         row.DimCode == dimCode_ex &&
         row.TFOrdNum == tfordNum_ex &&
         row.TFOrdLine == tfordLine_ex &&
         row.AttributeSetID == attributeSetID_ex
         select row).FirstOrDefault();
                findFirstPartAllocWithUpdLockQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartAllocWithUpdLockQuery_4(this.Db, company, partNum, attributeSetID, warehouseCode, dimCode, tfordNum, tfordLine);
        }

        static Func<ErpContext, string, string, int, string, string, string, int, PartAlloc> findFirstPartAllocWithUpdLock2Query;
        private PartAlloc FindFirstPartAllocWithUpdLock2(string company, string partNum, int attributeSetID, string warehouseCode, string dimCode, string tfordNum, int tfordLine)
        {
            if (findFirstPartAllocWithUpdLock2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, string, int, PartAlloc>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, warehouseCode_ex, dimCode_ex, tfordNum_ex, tfordLine_ex) =>
        (from row in ctx.PartAlloc.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.BinNum == "" &&
         row.DimCode == dimCode_ex &&
         row.TFOrdNum == tfordNum_ex &&
         row.TFOrdLine == tfordLine_ex &&
         row.AttributeSetID == attributeSetID_ex
         select row).FirstOrDefault();
                findFirstPartAllocWithUpdLock2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartAllocWithUpdLock2Query(this.Db, company, partNum, attributeSetID, warehouseCode, dimCode, tfordNum, tfordLine);
        }

        static Func<ErpContext, string, string, int, string, string, string, string, string, Guid, PartAlloc> findFirstPartAllocWithUpdLock3Query;
        private PartAlloc FindFirstPartAllocWithUpdLock3(string company, string partNum, int attributeSetID, string dimCode, string warehouseCode, string binNum, string lotNum, string pcid, Guid relatedToSysRowID)
        {
            if (findFirstPartAllocWithUpdLock3Query == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, string, string, string, Guid, PartAlloc>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, dimCode_ex, warehouseCode_ex, binNum_ex, lotNum_ex, pcid_ex, relatedToSysRowID_ex) =>
        (from row in ctx.PartAlloc.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.BinNum == binNum_ex &&
         row.RelatedToSchemaName == "Erp" &&
         row.RelatedToTableName == "TFOrdDtl" &&
         row.RelatedToSysRowID == relatedToSysRowID_ex &&
         row.LotNum == lotNum_ex &&
         row.DimCode == dimCode_ex &&
         row.PCID == pcid_ex &&
         row.AttributeSetID == attributeSetID_ex
         select row).FirstOrDefault();
                findFirstPartAllocWithUpdLock3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartAllocWithUpdLock3Query(this.Db, company, partNum, attributeSetID, dimCode, warehouseCode, binNum, lotNum, pcid, relatedToSysRowID);
        }

        static Func<ErpContext, string, string, int, string, string, string, int, string, PartAlloc> findFirstPartAllocByTFOrderWithUpdLockQuery;
        private PartAlloc FindFirstPartAllocByTFOrderWithUpdLock(string company, string partNum, int attributeSetID, string dimCode, string warehouseCode, string tfordNum, int tfordLine, string pcid)
        {
            if (findFirstPartAllocByTFOrderWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, string, int, string, PartAlloc>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, dimCode_ex, warehouseCode_ex, tfordNum_ex, tfordLine_ex, pcid_ex) =>
        (from row in ctx.PartAlloc.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.DimCode == dimCode_ex &&
         row.TFOrdNum == tfordNum_ex &&
         row.TFOrdLine == tfordLine_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.PCID == pcid_ex
         select row).FirstOrDefault();
                findFirstPartAllocByTFOrderWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartAllocByTFOrderWithUpdLockQuery(this.Db, company, partNum, attributeSetID, dimCode, warehouseCode, tfordNum, tfordLine, pcid);
        }


        static Func<ErpContext, string, string, int, string, string, string, string, string, IEnumerable<PartAlloc>> selectPartAllocQuery;
        private IEnumerable<PartAlloc> SelectPartAlloc(string company, string partNum, int attributeSetID, string warehouseCode, string binNum, string lotNum, string dimCode, string pcid)
        {
            if (selectPartAllocQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, string, string, string, IEnumerable<PartAlloc>>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, warehouseCode_ex, binNum_ex, lotNum_ex, dimCode_ex, pcid_ex) =>
        (from row in ctx.PartAlloc
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.BinNum == binNum_ex &&
         row.LotNum == lotNum_ex &&
         row.DimCode == dimCode_ex &&
         row.PCID == pcid_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.HardAllocation == true
         select row);
                selectPartAllocQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectPartAllocQuery(this.Db, company, partNum, attributeSetID, warehouseCode, binNum, lotNum, dimCode, pcid);
        }

        private class PartAllocResult
        {
            public int OrderNum { get; set; }
            public int OrderLine { get; set; }
            public int OrderRelNum { get; set; }
        }
        static Func<ErpContext, string, string, string, int, int, int, PartAllocResult> findFirstPCIDOrderRelPartAllocQuery;
        private PartAllocResult FindFirstPCIDOrderRelPartAlloc(string company, string pcid, string demandType, int validatingOrderNum, int validatingOrderLine, int validatingOrderRelNum)
        {
            if (findFirstPCIDOrderRelPartAllocQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, int, int, PartAllocResult>> expression =
      (ctx, company_ex, pcid_ex, demandType_ex, validatingOrderNum_ex, validatingOrderLine_ex, validatingOrderRelNum_ex) =>
        (from row in ctx.PartAlloc
         where row.Company == company_ex &&
         row.DemandType == demandType_ex &&
         row.PCID == pcid_ex &&
         row.OrderRelNum > 0 &&
         (row.AllocatedQty > 0 || row.PickingQty > 0 || row.PickedQty > 0) &&
         (!(row.OrderNum == validatingOrderNum_ex && row.OrderLine == validatingOrderLine_ex && row.OrderRelNum == validatingOrderRelNum_ex))
         select new PartAllocResult()
         {
             OrderNum = row.OrderNum,
             OrderLine = row.OrderLine,
             OrderRelNum = row.OrderRelNum,
         }
   ).FirstOrDefault();
                findFirstPCIDOrderRelPartAllocQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPCIDOrderRelPartAllocQuery(this.Db, company, pcid, demandType, validatingOrderNum, validatingOrderLine, validatingOrderRelNum);
        }

        static Func<ErpContext, string, string, int, string, string, string, string, decimal, bool> selectPartAllocPickingQtyWithUpdLockQuery;
        private bool ExistPartAllocPickingQty(string company, string tfordNum, int tfordLine, string warehouseCode, string binNum, string lot, string pcid, decimal pickingQty)
        {
            if (selectPartAllocPickingQtyWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, string, string, decimal, bool>> expression =
                  (ctx, company_ex, tfordNum_ex, tfordLine_ex, warehouseCode_ex, binNum_ex, lot_ex, pcid_ex, pickingQty_ex) =>
                    (from row in ctx.PartAlloc
                     where row.Company == company_ex &&
                     row.TFOrdNum == tfordNum_ex &&
                     row.TFOrdLine == tfordLine_ex &&
                     row.PickingQty > pickingQty_ex &&
                     (row.WarehouseCode != warehouseCode_ex ||
                      row.BinNum != binNum_ex ||
                      row.LotNum != lot_ex ||
                      row.PCID != pcid_ex)
                     select row).Any();
                selectPartAllocPickingQtyWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectPartAllocPickingQtyWithUpdLockQuery(this.Db, company, tfordNum, tfordLine, warehouseCode, binNum, lot, pcid, pickingQty);
        }
        #endregion PartAlloc Queries

        #region PartAllocSerial Queries
        static Func<ErpContext, string, string, bool> existsPartAllocSerialQuery;
        private bool ExistsPartAllocSerial(string company, string partNum)
        {
            if (existsPartAllocSerialQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (ctx, company_ex, partNum_ex) =>
                    (from row in ctx.PartAllocSerial
                     where row.Company == company_ex &&
                     row.PartNum == partNum_ex
                     select row).Any();
                existsPartAllocSerialQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPartAllocSerialQuery(this.Db, company, partNum);
        }
        #endregion

        #region PartBin Queries
        static Func<ErpContext, string, string, int, string, string> findFirstPartBinBinNumQuery;
        private string FindFirstPartBinBinNum(string company, string partNum, int attributeSetID, string warehouseCode)
        {
            if (findFirstPartBinBinNumQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, warehouseCode_ex) =>
        (from row in ctx.PartBin
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.AttributeSetID == attributeSetID_ex
         select row.BinNum).FirstOrDefault();
                findFirstPartBinBinNumQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartBinBinNumQuery(this.Db, company, partNum, attributeSetID, warehouseCode);
        }

        private class altPartBinResult
        {
            public decimal OnHandQty { get; set; }
            public decimal SalesAllocatedQty { get; set; }
            public decimal JobAllocatedQty { get; set; }
            public decimal TFOrdAllocatedQty { get; set; }
            public decimal SalesPickingQty { get; set; }
            public decimal JobPickingQty { get; set; }
            public decimal TFOrdPickingQty { get; set; }
            public decimal SalesPickedQty { get; set; }
            public decimal JobPickedQty { get; set; }
            public decimal TFOrdPickedQty { get; set; }
        }
        static Func<ErpContext, string, string, int, string, string, string, string, string, altPartBinResult> findFirstPartBinQuery_2;
        private altPartBinResult FindFirstPartBin(string company, string partNum, int attributeSetID, string warehouseCode, string binNum, string lotNum, string dimCode, string pcid)
        {
            if (findFirstPartBinQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, string, string, string, altPartBinResult>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, warehouseCode_ex, binNum_ex, lotNum_ex, dimCode_ex, pcid_ex) =>
        (from row in ctx.PartBin
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.BinNum == binNum_ex &&
         row.DimCode == dimCode_ex &&
         row.LotNum == lotNum_ex &&
         row.PCID == pcid_ex &&
         row.AttributeSetID == attributeSetID_ex
         select new altPartBinResult()
         {
             OnHandQty = row.OnhandQty,
             SalesAllocatedQty = row.SalesAllocatedQty,
             JobAllocatedQty = row.JobAllocatedQty,
             TFOrdAllocatedQty = row.TFOrdAllocatedQty,
             SalesPickingQty = row.SalesPickingQty,
             JobPickingQty = row.JobPickingQty,
             TFOrdPickingQty = row.TFOrdPickingQty,
             SalesPickedQty = row.SalesPickedQty,
             JobPickedQty = row.JobPickedQty,
             TFOrdPickedQty = row.TFOrdPickedQty
         }
   ).FirstOrDefault();
                findFirstPartBinQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartBinQuery_2(this.Db, company, partNum, attributeSetID, warehouseCode, binNum, lotNum, dimCode, pcid);
        }

        static Func<ErpContext, string, string, int, string, PartBin> findFirstPartBinQuery;
        private PartBin FindFirstPartBin(string company, string partNum, int attributeSetID, string warehouseCode)
        {
            if (findFirstPartBinQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, PartBin>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, warehouseCode_ex) =>
        (from row in ctx.PartBin
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.AttributeSetID == attributeSetID_ex
         select row).FirstOrDefault();
                findFirstPartBinQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartBinQuery(this.Db, company, partNum, attributeSetID, warehouseCode);
        }
        #endregion PartBin Queries

        #region PartCost Queries

        static Func<ErpContext, string, string, string, PartCost> findFirstPartCostQuery;
        private PartCost FindFirstPartCost(string company, string partNum, string costID)
        {
            if (findFirstPartCostQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, PartCost>> expression =
      (ctx, company_ex, partNum_ex, costID_ex) =>
        (from row in ctx.PartCost
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.CostID == costID_ex
         select row).FirstOrDefault();
                findFirstPartCostQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartCostQuery(this.Db, company, partNum, costID);
        }
        #endregion PartCost Queries

        #region PartDim Queries
        static Func<ErpContext, string, string, string, string> findFirstPartDimDimCodeDescriptionQuery;
        private string FindFirstPartDimDimCodeDescription(string Company, string PartNum, string DimCode)
        {
            if (findFirstPartDimDimCodeDescriptionQuery == null)
            {

                Expression<Func<ErpContext, string, string, string, string>> expression =
                     (ctx, company_ex, partnum_ex, dimcode_ex) =>
                    (from row in ctx.PartDim
                     where row.Company == company_ex &&
                     row.PartNum == partnum_ex &&
                     row.DimCode == dimcode_ex
                     select row.DimCodeDescription).FirstOrDefault();
                findFirstPartDimDimCodeDescriptionQuery = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPartDimDimCodeDescriptionQuery(this.Db, Company, PartNum, DimCode) ?? string.Empty;
        }
        #endregion

        #region PartFIFOCost Queries

        static Func<ErpContext, string, bool, string, string, string, PartFIFOCost> findFirstPartFIFOCostQuery;
        private PartFIFOCost FindFirstPartFIFOCost(string company, bool inActive, string partNum, string lotNum, string costID)
        {
            if (findFirstPartFIFOCostQuery == null)
            {
                Expression<Func<ErpContext, string, bool, string, string, string, PartFIFOCost>> expression =
      (ctx, company_ex, inActive_ex, partNum_ex, lotNum_ex, costID_ex) =>
        (from row in ctx.PartFIFOCost
         where row.Company == company_ex &&
         row.InActive == inActive_ex &&
         row.PartNum == partNum_ex &&
         row.LotNum == lotNum_ex &&
         row.CostID == costID_ex
         select row).FirstOrDefault();
                findFirstPartFIFOCostQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartFIFOCostQuery(this.Db, company, inActive, partNum, lotNum, costID);
        }
        #endregion PartFIFOCost Queries

        #region PartLot Queries
        static Func<ErpContext, string, string, string, bool> existsPartLotQuery;
        private bool ExistsPartLot(string company, string partNum, string lotNum)
        {
            if (existsPartLotQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
              (ctx, company_ex, partNum_ex, lotNum_ex) =>
                (from row in ctx.PartLot
                 where row.Company == company_ex &&
                 row.PartNum == partNum_ex &&
                 row.LotNum == lotNum_ex
                 select row).Any();
                existsPartLotQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPartLotQuery(this.Db, company, partNum, lotNum);
        }

        static Func<ErpContext, string, string, string, string> findFirstPartLotDescriptionQuery;
        private string FindFirstPartLotDescription(string company, string partNum, string lotNum)
        {
            if (findFirstPartLotDescriptionQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string>> expression =
              (ctx, company_ex, partNum_ex, lotNum_ex) =>
                (from row in ctx.PartLot
                 where row.Company == company_ex &&
                 row.PartNum == partNum_ex &&
                 row.LotNum == lotNum_ex
                 select row.PartLotDescription).FirstOrDefault();
                findFirstPartLotDescriptionQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartLotDescriptionQuery(this.Db, company, partNum, lotNum) ?? string.Empty;
        }
        #endregion PartLot Queries

        #region PartPlant Queries

        static Func<ErpContext, string, string, string, string> findFirstPartPlantPrimWhseQuery;
        private string FindFirstPartPlantPrimWhse(string company, string plant, string partNum)
        {
            if (findFirstPartPlantPrimWhseQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string>> expression =
      (ctx, company_ex, plant_ex, partNum_ex) =>
        (from row in ctx.PartPlant
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex
         select row.PrimWhse).FirstOrDefault();
                findFirstPartPlantPrimWhseQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartPlantPrimWhseQuery(this.Db, company, plant, partNum);
        }


        static Func<ErpContext, string, string, string, PartPlant> findFirstPartPlantQuery;
        private PartPlant FindFirstPartPlant(string company, string plant, string partNum)
        {
            if (findFirstPartPlantQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, PartPlant>> expression =
      (ctx, company_ex, plant_ex, partNum_ex) =>
        (from row in ctx.PartPlant
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex
         select row).FirstOrDefault();
                findFirstPartPlantQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartPlantQuery(this.Db, company, plant, partNum);
        }
        #endregion PartPlant Queries

        #region PartRev Queries

        static Func<ErpContext, string, string, DateTime?, PartRev> findLastPartRevQuery;
        private PartRev FindLastPartRev(string company, string partNum, DateTime? effectiveDate)
        {
            if (findLastPartRevQuery == null)
            {
                Expression<Func<ErpContext, string, string, DateTime?, PartRev>> expression =
      (ctx, company_ex, partNum_ex, effectiveDate_ex) =>
        (from row in ctx.PartRev
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.EffectiveDate.Value <= effectiveDate_ex.Value &&
         row.AltMethod == ""
         select row).LastOrDefault();
                findLastPartRevQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastPartRevQuery(this.Db, company, partNum, effectiveDate);
        }
        #endregion PartRev Queries

        #region LegalNumHistory Queries
        static Func<ErpContext, string, string, LegalNumHistory> findLegalNumHistoryWithUpdLockQuery;
        private LegalNumHistory FindFirstLegalNumHistoryWithUpdLock(string company, string legalNum)
        {
            if (findLegalNumHistoryWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, LegalNumHistory>> expression =
      (ctx, company_ex, legalNum_ex) =>
        (from row in ctx.LegalNumHistory.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.LegalNumber == legalNum_ex
         select row).FirstOrDefault();
                findLegalNumHistoryWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLegalNumHistoryWithUpdLockQuery(this.Db, company, legalNum);
        }
        #endregion

        #region PartTran Queries

        static Func<ErpContext, string, string, int, string, int, string, bool> existsPartTranQuery;
        private bool ExistsPartTran(string company, string jobNum, int assemblySeq, string jobSeqType, int jobSeq, string partNum)
        {
            if (existsPartTranQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, int, string, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, jobSeqType_ex, jobSeq_ex, partNum_ex) =>
        (from row in ctx.PartTran
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.JobSeqType == jobSeqType_ex &&
         row.JobSeq == jobSeq_ex &&
         row.PartNum == partNum_ex
         select row).Any();
                existsPartTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPartTranQuery(this.Db, company, jobNum, assemblySeq, jobSeqType, jobSeq, partNum);
        }


        static Func<ErpContext, Guid, PartTran> findFirstPartTranQuery;
        private PartTran FindFirstPartTran(Guid sysRowID)
        {
            if (findFirstPartTranQuery == null)
            {
                Expression<Func<ErpContext, Guid, PartTran>> expression =
      (ctx, sysRowID_ex) =>
        (from row in ctx.PartTran
         where row.SysRowID == sysRowID_ex
         select row).FirstOrDefault();
                findFirstPartTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartTranQuery(this.Db, sysRowID);
        }


        static Func<ErpContext, string, string, string, Guid, IEnumerable<PartTran>> selectPartTranQuery;
        private IEnumerable<PartTran> SelectPartTran(string company, string tranType, string partNum, Guid sysRowID)
        {
            if (selectPartTranQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, Guid, IEnumerable<PartTran>>> expression =
      (ctx, company_ex, tranType_ex, partNum_ex, sysRowID_ex) =>
        (from row in ctx.PartTran
         where row.Company == company_ex &&
         row.TranType == tranType_ex &&
         row.PartNum == partNum_ex &&
         row.SysRowID != sysRowID_ex
         select row);
                selectPartTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectPartTranQuery(this.Db, company, tranType, partNum, sysRowID);
        }


        static Func<ErpContext, string, string, int, string, int, string, IEnumerable<PartTran>> selectPartTranQuery_2;
        private IEnumerable<PartTran> SelectPartTran(string company, string jobNum, int assemblySeq, string jobSeqType, int jobSeq, string partNum)
        {
            if (selectPartTranQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, int, string, IEnumerable<PartTran>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, jobSeqType_ex, jobSeq_ex, partNum_ex) =>
        (from row in ctx.PartTran
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.JobSeqType == jobSeqType_ex &&
         row.JobSeq == jobSeq_ex &&
         row.PartNum == partNum_ex
         select row);
                selectPartTranQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return selectPartTranQuery_2(this.Db, company, jobNum, assemblySeq, jobSeqType, jobSeq, partNum);
        }






        static Func<ErpContext, string, string, int, string, int, string, Guid, IEnumerable<PartTran>> selectPartTran2Query;
        private IEnumerable<PartTran> SelectPartTran2(string company, string jobNum, int assemblySeq, string jobSeqType, int jobSeq, string partNum, Guid sysRowID)
        {
            if (selectPartTran2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, string, int, string, Guid, IEnumerable<PartTran>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, jobSeqType_ex, jobSeq_ex, partNum_ex, sysRowID_ex) =>
        (from row in ctx.PartTran
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.JobSeqType == jobSeqType_ex &&
         row.JobSeq == jobSeq_ex &&
         row.PartNum == partNum_ex &&
         row.SysRowID != sysRowID_ex
         orderby row.SysDate, row.SysTime, row.TranNum
         select row);
                selectPartTran2Query = DBExpressionCompiler.Compile(expression);
            }

            return selectPartTran2Query(this.Db, company, jobNum, assemblySeq, jobSeqType, jobSeq, partNum, sysRowID);
        }

        static Func<ErpContext, string, string, int, string, int, string, Guid, IEnumerable<PartTran>> selectPartTranSortQuery;
        private IEnumerable<PartTran> SelectPartTranSorted(string company, string jobNum, int assemblySeq, string jobSeqType, int jobSeq, string partNum, Guid sysRowID)
        {
            if (selectPartTranSortQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, int, string, Guid, IEnumerable<PartTran>>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, jobSeqType_ex, jobSeq_ex, partNum_ex, sysRowID_ex) =>
        (from row in ctx.PartTran
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.JobSeqType == jobSeqType_ex &&
         row.JobSeq == jobSeq_ex &&
         row.PartNum == partNum_ex &&
         row.SysRowID != sysRowID_ex
         orderby row.SysDate, row.SysTime, row.TranNum
         select row);
                selectPartTranSortQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectPartTranSortQuery(this.Db, company, jobNum, assemblySeq, jobSeqType, jobSeq, partNum, sysRowID);
        }
        #endregion PartTran Queries

        #region PartUOM Queries

        static Func<ErpContext, string, string, string, bool, bool> existsPartUOMQuery;
        private bool ExistsPartUOM(string company, string partNum, string uomcode, bool trackOnHand)
        {
            if (existsPartUOMQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool, bool>> expression =
      (ctx, company_ex, partNum_ex, uomcode_ex, trackOnHand_ex) =>
        (from row in ctx.PartUOM
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.UOMCode == uomcode_ex &&
         row.TrackOnHand == trackOnHand_ex
         select row).Any();
                existsPartUOMQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPartUOMQuery(this.Db, company, partNum, uomcode, trackOnHand);
        }
        #endregion PartUOM Queries

        #region PartWhse Queries

        static Func<ErpContext, string, string, Guid, bool> existsPartWhseQuery;
        private bool ExistsPartWhse(string company, string partNum, Guid sysRowID)
        {
            if (existsPartWhseQuery == null)
            {
                Expression<Func<ErpContext, string, string, Guid, bool>> expression =
      (ctx, company_ex, partNum_ex, sysRowID_ex) =>
        (from row in ctx.PartWhse
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SysRowID != sysRowID_ex
         select row).Any();
                existsPartWhseQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPartWhseQuery(this.Db, company, partNum, sysRowID);
        }

        static Func<ErpContext, string, string, string, bool> existsPartWhseQuery2;
        private bool ExistsUniquePartWhse(string company, string partNum, string warehouseCode)
        {
            if (existsPartWhseQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, partNum_ex, warehouseCode_ex) =>
        (from row in ctx.PartWhse
         where row.Company == company_ex &&
               row.PartNum == partNum_ex &&
               row.WarehouseCode == warehouseCode_ex
         select row).Take(2).Count() == 1;
                existsPartWhseQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return existsPartWhseQuery2(this.Db, company, partNum, warehouseCode);
        }


        static Func<ErpContext, string, string, string> findFirstPartWhseQuery;
        private string FindFirstPartWhse(string company, string partNum)
        {
            if (findFirstPartWhseQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
      (ctx, company_ex, partNum_ex) =>
        (from row in ctx.PartWhse
         where row.Company == company_ex &&
         row.PartNum == partNum_ex
         select row.WarehouseCode).FirstOrDefault();
                findFirstPartWhseQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWhseQuery(this.Db, company, partNum);
        }


        static Func<ErpContext, string, string, PartWhse> findFirstPartWhse2Query;
        private PartWhse FindFirstPartWhse2(string company, string partNum)
        {
            if (findFirstPartWhse2Query == null)
            {
                Expression<Func<ErpContext, string, string, PartWhse>> expression =
      (ctx, company_ex, partNum_ex) =>
        (from row in ctx.PartWhse
         where row.Company == company_ex &&
         row.PartNum == partNum_ex
         select row).FirstOrDefault();
                findFirstPartWhse2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWhse2Query(this.Db, company, partNum);
        }


        static Func<ErpContext, string, string, PartWhse> findFirstPartWhse3Query;
        private PartWhse FindFirstPartWhse3(string company, string partNum)
        {
            if (findFirstPartWhse3Query == null)
            {
                Expression<Func<ErpContext, string, string, PartWhse>> expression =
      (ctx, company_ex, partNum_ex) =>
        (from row in ctx.PartWhse
         where row.Company == company_ex &&
         row.PartNum == partNum_ex
         select row).FirstOrDefault();
                findFirstPartWhse3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWhse3Query(this.Db, company, partNum);
        }
        #endregion PartWhse Queries

        #region PartWip Queries
        static Func<ErpContext, string, string, int, bool> existsPartWipQuery;
        private bool ExistsPartWip(string company, string jobNum, int assemblySeq)
        {
            if (existsPartWipQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, bool>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex) =>
        (from row in ctx.PartWip
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.PCID != String.Empty
         select row).Any();
                existsPartWipQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPartWipQuery(this.Db, company, jobNum, assemblySeq);
        }

        static Func<ErpContext, string, string, int, int, string, PartWip> findFirstPartWipQuery;
        private PartWip FindFirstPartWip(string company, string jobNum, int assemblySeq, int oprSeq, string pcid)
        {
            if (findFirstPartWipQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, PartWip>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, pcid_ex) =>
        (from row in ctx.PartWip
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.PCID == pcid_ex
         select row).FirstOrDefault();
                findFirstPartWipQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWipQuery(this.Db, company, jobNum, assemblySeq, oprSeq, pcid);
        }

        private class PartWipResult
        {
            public string WareHouseCode { get; set; }
            public string BinNum { get; set; }
            public string LotNum { get; set; }
        }
        static Func<ErpContext, string, string, string, string, int, string, int, string, int, PartWipResult> findFirstPartWipQuery_2;
        private PartWipResult FindFirstPartWip(string company, string plant, string partNum, string jobNum, int assemblySeq, string trackType, int oprSeq, string pcid, int attributeSetID)
        {
            if (findFirstPartWipQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, int, string, int, string, int, PartWipResult>> expression =
      (ctx, company_ex, plant_ex, partNum_ex, jobNum_ex, assemblySeq_ex, trackType_ex, oprSeq_ex, pcid_ex, attributeSetID_ex) =>
        (from row in ctx.PartWip
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.TrackType == trackType_ex &&
         row.PCID == pcid_ex &&
         row.AttributeSetID == attributeSetID_ex
         select new PartWipResult()
         {
             WareHouseCode = row.WareHouseCode,
             BinNum = row.BinNum,
             LotNum = row.LotNum
         }).FirstOrDefault();
                findFirstPartWipQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWipQuery_2(this.Db, company, plant, partNum, jobNum, assemblySeq, trackType, oprSeq, pcid, attributeSetID);
        }

        static Func<ErpContext, string, string, int, string, int, PartWip> findFirstPartWipQuery_3;
        private PartWip FindFirstPartWip(string company, string jobNum, int assemblySeq, string trackType, int oprSeq)
        {
            if (findFirstPartWipQuery_3 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, int, PartWip>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, trackType_ex, oprSeq_ex) =>
        (from row in ctx.PartWip
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.TrackType == trackType_ex &&
         row.OprSeq == oprSeq_ex
         select row).FirstOrDefault();
                findFirstPartWipQuery_3 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWipQuery_3(this.Db, company, jobNum, assemblySeq, trackType, oprSeq);
        }

        static Func<ErpContext, string, string, string, string, int, string, int, string, string, string, int, int, string, PartWip> findFirstPartWipQuery_4;
        private PartWip FindFirstPartWip(string company, string plant, string partNum, string jobNum, int assemblySeq, string trackType, int oprSeq, string wareHouseCode, string binNum, string lotNum, int attributeSetID, int mtlSeq, string pcid)
        {
            if (findFirstPartWipQuery_4 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, int, string, int, string, string, string, int, int, string, PartWip>> expression =
      (ctx, company_ex, plant_ex, partNum_ex, jobNum_ex, assemblySeq_ex, trackType_ex, oprSeq_ex, wareHouseCode_ex, binNum_ex, lotNum_ex, attributeSetID_ex, mtlSeq_ex, pcid_ex) =>
        (from row in ctx.PartWip
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.WareHouseCode == wareHouseCode_ex &&
         row.BinNum == binNum_ex &&
         row.LotNum == lotNum_ex &&
         row.DimCode == "" &&
         row.TrackType == trackType_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.PCID == pcid_ex &&
         row.AttributeSetID == attributeSetID_ex
         select row).FirstOrDefault();
                findFirstPartWipQuery_4 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWipQuery_4(this.Db, company, plant, partNum, jobNum, assemblySeq, trackType, oprSeq, wareHouseCode, binNum, lotNum, attributeSetID, mtlSeq, pcid);
        }

        static Func<ErpContext, string, string, int, int, int, PartWip> findFirstPartWip2Query;
        private PartWip FindFirstPartWip2(string company, string jobNum, int assemblySeq, int oprSeq, int FromAssemblySeq)
        {
            if (findFirstPartWip2Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, PartWip>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, fromAssemblySeq_ex) =>
        (from row in ctx.PartWip
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.FromAssemblySeq == fromAssemblySeq_ex &&
         row.TrackType == "M"
         orderby row.SysRevID descending
         select row).FirstOrDefault();
                findFirstPartWip2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWip2Query(this.Db, company, jobNum, assemblySeq, oprSeq, FromAssemblySeq);
        }

        static Func<ErpContext, string, string, int, int, PartWip> findFirstPartWip3Query;
        private PartWip FindFirstPartWip3(string company, string jobNum, int assemblySeq, int mtlSeq)
        {
            if (findFirstPartWip3Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, PartWip>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex) =>
        (from row in ctx.PartWip
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex
         select row).FirstOrDefault();
                findFirstPartWip3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWip3Query(this.Db, company, jobNum, assemblySeq, mtlSeq);
        }

        static Func<ErpContext, string, string, int, int, int, string, string, string, string, int, string, PartWip> findFirstPartWip4Query;
        private PartWip FindFirstPartWip4(string company, string jobNum, int assemblySeq, int oprSeq, int FromAssemblySeq, string partnum, string warehouseCode, string binnum, string lotnum, int attributeSetID, string pcid)
        {
            if (findFirstPartWip4Query == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, string, string, string, string, int, string, PartWip>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, fromAssemblySeq_ex, partnum_ex, warehouseCode_ex, binnum_ex, lotnum_ex, attributeSetID_ex, pcid_ex) =>
        (from row in ctx.PartWip
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.FromAssemblySeq == fromAssemblySeq_ex &&
         row.TrackType == "M" &&
         row.PartNum == partnum_ex &&
         row.WareHouseCode == warehouseCode_ex &&
         row.BinNum == binnum_ex &&
         row.LotNum == lotnum_ex &&
         row.PCID == pcid_ex &&
         row.AttributeSetID == attributeSetID_ex
         orderby row.SysRevID descending
         select row).FirstOrDefault();
                findFirstPartWip4Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWip4Query(this.Db, company, jobNum, assemblySeq, oprSeq, FromAssemblySeq, partnum, warehouseCode, binnum, lotnum, attributeSetID, pcid);
        }

        class PartWipQueryParams
        {
            public string company;
            public string plant;
            public string partNum;
            public string jobNum;
            public int assemblySeq;
            public int oprSeq;
            public int mtlSeq;
            public string wareHouseCode;
            public string lotNum;
            public int attributeSetID;
            public string binNum;
            public string trackType;
            public string pcid;
            public bool checkFromValues;
            public int fromAsmSeq;
            public int fromOprSeq;
        }

        static Func<ErpContext, PartWipQueryParams, PartWip> findFirstPartWipWithUpdLockQuery;
        private PartWip FindFirstPartWipWithUpdLock(PartWipQueryParams partWipQueryParams)
        {
            if (findFirstPartWipWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, PartWipQueryParams, PartWip>> expression =
      (ctx, partWipQueryParams_ex) =>
        (from row in ctx.PartWip.With(LockHint.UpdLock)
         where row.Company == partWipQueryParams_ex.company &&
         row.Plant == partWipQueryParams_ex.plant &&
         row.PartNum == partWipQueryParams_ex.partNum &&
         row.JobNum == partWipQueryParams_ex.jobNum &&
         row.AssemblySeq == partWipQueryParams_ex.assemblySeq &&
         row.OprSeq == partWipQueryParams_ex.oprSeq &&
         row.WareHouseCode == partWipQueryParams_ex.wareHouseCode &&
         row.BinNum == partWipQueryParams_ex.binNum &&
         row.LotNum == partWipQueryParams_ex.lotNum &&
         row.TrackType == partWipQueryParams_ex.trackType &&
         row.MtlSeq == partWipQueryParams_ex.mtlSeq &&
         row.PCID == partWipQueryParams_ex.pcid &&
         row.AttributeSetID == partWipQueryParams_ex.attributeSetID &&
         (partWipQueryParams_ex.checkFromValues == false || (row.FromAssemblySeq == partWipQueryParams_ex.fromAsmSeq && row.FromOprSeq == partWipQueryParams_ex.fromOprSeq))
         select row).FirstOrDefault();
                findFirstPartWipWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWipWithUpdLockQuery(this.Db, partWipQueryParams);
        }

        static Func<ErpContext, string, string, string, string, int, int, string, string, string, int, string, PartWip> findFirstPartWipQueryMaterial;
        private PartWip FindFirstPartWipMaterial(string company, string plant, string partNum, string jobNum, int assemblySeq, int mtlSeq, string wareHouseCode, string binNum, string lotNum, int attributeSetID, string pcid)
        {
            if (findFirstPartWipQueryMaterial == null)
            {
                Expression<Func<ErpContext, string, string, string, string, int, int, string, string, string, int, string, PartWip>> expression =
      (ctx, company_ex, plant_ex, partNum_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, wareHouseCode_ex, binNum_ex, lotNum_ex, attributeSetID_ex, pcid_ex) =>
        (from row in ctx.PartWip
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.WareHouseCode == wareHouseCode_ex &&
         row.BinNum == binNum_ex &&
         row.LotNum == lotNum_ex &&
         row.DimCode == "" &&
         row.PCID == pcid_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.TrackType == "R"
         select row).FirstOrDefault();
                findFirstPartWipQueryMaterial = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWipQueryMaterial(this.Db, company, plant, partNum, jobNum, assemblySeq, mtlSeq, wareHouseCode, binNum, lotNum, attributeSetID, pcid);
        }

        static Func<ErpContext, string, string, string, string, int, int, string, string, string, int, string, PartWip> findFirstPartWipQueryFinishedGood;
        private PartWip FindFirstPartWipFinishedGood(string company, string plant, string partNum, string jobNum, int assemblySeq, int oprSeq, string wareHouseCode, string binNum, string lotNum, int attributeSetID, string pcid)
        {
            if (findFirstPartWipQueryFinishedGood == null)
            {
                Expression<Func<ErpContext, string, string, string, string, int, int, string, string, string, int, string, PartWip>> expression =
      (ctx, company_ex, plant_ex, partNum_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, wareHouseCode_ex, binNum_ex, lotNum_ex, attributeSetID_ex, pcid_ex) =>
        (from row in ctx.PartWip
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.MtlSeq == 0 &&
         row.WareHouseCode == wareHouseCode_ex &&
         row.BinNum == binNum_ex &&
         row.LotNum == lotNum_ex &&
         row.DimCode == "" &&
         row.PCID == pcid_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.TrackType == "M"
         select row).FirstOrDefault();
                findFirstPartWipQueryFinishedGood = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartWipQueryFinishedGood(this.Db, company, plant, partNum, jobNum, assemblySeq, oprSeq, wareHouseCode, binNum, lotNum, attributeSetID, pcid);
        }

        static Func<ErpContext, string, string, int, int, string, string, string, string, int, string, PartWip> findFirstPartWipQuery_6;
        private PartWip FindFirstPartWip6(string company, string jobNum, int assemblySeq, int oprSeq, string partnum, string warehouseCode, string binnum, string lotnum, int attributeSetID, string pcid)
        {
            if (findFirstPartWipQuery_6 == null)
            {
                Expression<Func<ErpContext, string, string, int, int, string, string, string, string, int, string, PartWip>> expression =
      (ctx, company_ex, jobNum_ex, assemblySeq_ex, oprSeq_ex, partnum_ex, warehouseCode_ex, binnum_ex, lotnum_ex, attributeSetID_ex, pcid_ex) =>
        (from row in ctx.PartWip
         where row.Company == company_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.OprSeq == oprSeq_ex &&
         row.TrackType == "M" &&
         row.PartNum == partnum_ex &
         row.WareHouseCode == warehouseCode_ex &&
         row.BinNum == binnum_ex &&
         row.LotNum == lotnum_ex &&
         row.PCID == pcid_ex &&
         row.AttributeSetID == attributeSetID_ex
         orderby row.SysRevID descending
         select row).FirstOrDefault();
                findFirstPartWipQuery_6 = DBExpressionCompiler.Compile(expression);
            }
            return findFirstPartWipQuery_6(this.Db, company, jobNum, assemblySeq, oprSeq, partnum, warehouseCode, binnum, lotnum, attributeSetID, pcid);
        }
        #endregion PartWip Queries

        #region PickedOrders Queries
        static Func<ErpContext, string, int, int, int, string, string, string, string, int, string, PickedOrders> findFirstPickedOrdersForUnpickWithUpdLockQuery;
        private PickedOrders FindFirstPickedOrdersForUnpickWithUpdLock(string company, int orderNum, int orderLine, int orderRelNum, string partNum, string warehouseCode, string binNum, string lotNum, int attributeSetID, string pcid)
        {
            if (findFirstPickedOrdersForUnpickWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, string, string, string, string, int, string, PickedOrders>> expression =
      (ctx, company_ex, orderNum_ex, orderLine_ex, orderRelNum_ex, partNum_ex, warehouseCode_ex, binNum_ex, lotNum_ex, attributeSetID_ex, pcid_ex) =>
        (from row in ctx.PickedOrders.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.OrderNum == orderNum_ex &&
         row.OrderLine == orderLine_ex &&
         row.OrderRelNum == orderRelNum_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.BinNum == binNum_ex &&
         row.LotNum == lotNum_ex &&
         row.PCID == pcid_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.PartNum == partNum_ex
         select row).FirstOrDefault();
                findFirstPickedOrdersForUnpickWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPickedOrdersForUnpickWithUpdLockQuery(this.Db, company, orderNum, orderLine, orderRelNum, partNum, warehouseCode, binNum, lotNum, attributeSetID, pcid);
        }

        static Func<ErpContext, string, string, int, int, int, PickedOrders> findFirstPickedOrdersWithUpdLockQuery;
        private PickedOrders FindFirstPickedOrdersWithUpdLock(string company, string plant, int orderNum, int orderLine, int orderRelNum)
        {
            if (findFirstPickedOrdersWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, int, int, PickedOrders>> expression =
      (ctx, company_ex, plant_ex, orderNum_ex, orderLine_ex, orderRelNum_ex) =>
        (from row in ctx.PickedOrders.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.OrderNum == orderNum_ex &&
         row.OrderLine == orderLine_ex &&
         row.OrderRelNum == orderRelNum_ex
         select row).FirstOrDefault();
                findFirstPickedOrdersWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPickedOrdersWithUpdLockQuery(this.Db, company, plant, orderNum, orderLine, orderRelNum);
        }
        #endregion PickedOrders Queries

        #region Packing Queries
        static Func<ErpContext, string, string, string> getPackingPkgCodeQuery;
        private string GetPackingPkgCode(string company, string pkgCode)
        {
            if (getPackingPkgCodeQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
      (ctx, company_ex, pkgCode_ex) =>
        (from row in ctx.Packing
         where row.Company == company_ex &&
         row.PkgCode == pkgCode_ex &&
         row.Inactive == false
         select row.PkgCode).FirstOrDefault();
                getPackingPkgCodeQuery = DBExpressionCompiler.Compile(expression);
            }

            return getPackingPkgCodeQuery(this.Db, company, pkgCode);
        }
        #endregion

        #region PkgControl Queries
        static Func<ErpContext, string, string, string, PkgControl> findFirstPkgControlQuery;
        private PkgControl FindFirstPkgControl(string company, string plant, string pkgControlID)
        {
            if (findFirstPkgControlQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, PkgControl>> expression =
      (ctx, company_ex, plant_ex, pkgControlID_ex) =>
        (from row in ctx.PkgControl
         where row.Company == company_ex &&
          row.Plant == plant_ex &&
         row.PkgControlIDCode == pkgControlID_ex
         select row).FirstOrDefault();
                findFirstPkgControlQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlQuery(this.Db, company, plant, pkgControlID);
        }
        #endregion

        #region PkgControlHeader Queries
        private static Func<ErpContext, string, string, bool> existsPkgControlHeaderQuery;
        private bool ExistsPkgControlHeader(string company, string pcid)
        {
            if (existsPkgControlHeaderQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (context, company_ex, pcid_ex) =>
                    (from row in context.PkgControlHeader
                     where row.Company == company_ex
                     && row.PCID == pcid_ex
                     select row)
                    .Any();
                existsPkgControlHeaderQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPkgControlHeaderQuery(this.Db, company, pcid);
        }

        private static Func<ErpContext, string, string, string, bool> existsPkgControlHeaderByPkgControlTypeQuery;
        private bool ExistsPkgControlHeaderByPkgControlType(string company, string pcid, string pkgControlType)
        {
            if (existsPkgControlHeaderByPkgControlTypeQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
                    (context, company_ex, pcid_ex, pkgControlType_ex) =>
                    (from row in context.PkgControlHeader
                     where row.Company == company_ex
                     && row.PCID == pcid_ex
                     && row.PkgControlType == pkgControlType_ex
                     select row)
                    .Any();
                existsPkgControlHeaderByPkgControlTypeQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPkgControlHeaderByPkgControlTypeQuery(this.Db, company, pcid, pkgControlType);
        }

        private class PkgControlHeaderPartialRow2 : Epicor.Data.TempRowBase
        {
            public int CustNum { get; set; }
            public string ShipToNum { get; set; }
        }

        private static Func<ErpContext, string, string, PkgControlHeaderPartialRow2> findFirstPkgControlHeaderCustQuery;
        private PkgControlHeaderPartialRow2 FindFirstPkgControlHeaderCustInfo(string company, string pcid)
        {
            if (findFirstPkgControlHeaderCustQuery == null)
            {
                Expression<Func<ErpContext, string, string, PkgControlHeaderPartialRow2>> expression =
                    (context, company_ex, pcid_ex) =>
                    (from row in context.PkgControlHeader
                     where row.Company == company_ex &&
                     row.PCID == pcid_ex
                     select new PkgControlHeaderPartialRow2 { CustNum = row.CustNum, ShipToNum = row.ShipToNum })
                    .FirstOrDefault();
                findFirstPkgControlHeaderCustQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlHeaderCustQuery(this.Db, company, pcid);
        }

        private static Func<ErpContext, string, string, bool> getPkgControlHeaderOutboundContainerQuery;
        private bool GetPkgControlHeaderOutboundContainer(string company, string pcid)
        {
            if (getPkgControlHeaderOutboundContainerQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (context, company_ex, pcid_ex) =>
                    (from row in context.PkgControlHeader
                     where row.Company == company_ex &&
                     row.PCID == pcid_ex
                     select row.OutboundContainer).FirstOrDefault();
                getPkgControlHeaderOutboundContainerQuery = DBExpressionCompiler.Compile(expression);
            }

            return getPkgControlHeaderOutboundContainerQuery(this.Db, company, pcid);
        }

        private class PkgControlHeaderPartial : TempRowBase
        {
            public string PkgControlStatus { get; set; }
        }

        static Func<ErpContext, string, string, PkgControlHeaderPartial> findFirstPkgControlHeaderPartialQuery;
        private PkgControlHeaderPartial FindFirstPkgControlHeaderPartial(string company, string pcid)
        {
            if (findFirstPkgControlHeaderPartialQuery == null)
            {
                Expression<Func<ErpContext, string, string, PkgControlHeaderPartial>> expression =
              (ctx, company_ex, pcid_ex) =>
                (from row in ctx.PkgControlHeader
                 where row.Company == company_ex &&
                 row.PCID == pcid_ex
                 select new PkgControlHeaderPartial() { PkgControlStatus = row.PkgControlStatus }).FirstOrDefault();
                findFirstPkgControlHeaderPartialQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlHeaderPartialQuery(this.Db, company, pcid);
        }

        private class PkgControlHeaderPartialRow : Epicor.Data.TempRowBase
        {
            public bool ArchivePCIDHistory { get; set; }
            public string PkgControlType { get; set; }
        }

        private static Func<ErpContext, string, string, PkgControlHeaderPartialRow> findFirstPkgControlHeaderPartialQuery2;
        private PkgControlHeaderPartialRow FindFirstPkgControlHeaderPartial2(string company, string pcid)
        {
            if (findFirstPkgControlHeaderPartialQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, PkgControlHeaderPartialRow>> expression =
                    (context, company_ex, pcid_ex) =>
                    (from row in context.PkgControlHeader
                     where row.Company == company_ex &&
                     row.PCID == pcid_ex
                     select new PkgControlHeaderPartialRow { ArchivePCIDHistory = row.ArchivePCIDHistory, PkgControlType = row.PkgControlType })
                    .FirstOrDefault();
                findFirstPkgControlHeaderPartialQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlHeaderPartialQuery2(this.Db, company, pcid);
        }


        static Func<ErpContext, string, string, PkgControlHeader> findFirstPkgControlHeaderWithUpdLockQuery;
        private PkgControlHeader FindFirstPkgControlHeaderWithUpdLock(string company, string pcid)
        {
            if (findFirstPkgControlHeaderWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, PkgControlHeader>> expression =
      (ctx, company_ex, pcid_ex) =>
        (from row in ctx.PkgControlHeader.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PCID == pcid_ex
         select row).FirstOrDefault();
                findFirstPkgControlHeaderWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlHeaderWithUpdLockQuery(this.Db, company, pcid);
        }

        static Func<ErpContext, string, string, int, PkgControlHeader> findFirstPkgControlHeaderByCustWithUpdLockQuery;
        private PkgControlHeader FindFirstPkgControlHeaderByCustWithUpdLock(string company, string pcid, int custNum)
        {
            if (findFirstPkgControlHeaderByCustWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, PkgControlHeader>> expression =
      (ctx, company_ex, pcid_ex, custNum_ex) =>
        (from row in ctx.PkgControlHeader.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PCID == pcid_ex &&
         row.CustNum == custNum_ex
         select row).FirstOrDefault();
                findFirstPkgControlHeaderByCustWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlHeaderByCustWithUpdLockQuery(this.Db, company, pcid, custNum);
        }


        static Func<ErpContext, string, string, PkgControlHeader> findFirstPkgControlHeaderQuery;
        private PkgControlHeader FindFirstPkgControlHeader(string company, string pcid)
        {
            if (findFirstPkgControlHeaderQuery == null)
            {
                Expression<Func<ErpContext, string, string, PkgControlHeader>> expression =
      (ctx, company_ex, pcid_ex) =>
        (from row in ctx.PkgControlHeader
         where row.Company == company_ex &&
         row.PCID == pcid_ex
         select row).FirstOrDefault();
                findFirstPkgControlHeaderQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlHeaderQuery(this.Db, company, pcid);
        }

        static Func<ErpContext, string, string, bool> findFirstPkgControlHeaderContainerReturnableQuery;
        private bool FindFirstPkgControlHeaderContainerReturnable(string company, string pcid)
        {
            if (findFirstPkgControlHeaderContainerReturnableQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, pcid_ex) =>
        (from row in ctx.PkgControlHeader
         where row.Company == company_ex &&
         row.PCID == pcid_ex
         select row.ContainerReturnable).FirstOrDefault();
                findFirstPkgControlHeaderContainerReturnableQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlHeaderContainerReturnableQuery(this.Db, company, pcid);
        }

        private class PkgControlHeaderWhsAndStatus : Epicor.Data.TempRowBase
        {
            public string WarehouseCode { get; set; }
            public string BinNum { get; set; }
            public string Plant { get; set; }
            public string PkgControlStatus { get; set; }
        }

        static Func<ErpContext, string, string, PkgControlHeaderWhsAndStatus> findFirstPkgControlHeaderWhsAndStatusRowQuery;
        private PkgControlHeaderWhsAndStatus FindFirstPkgControlHeaderPartialRow(string company, string pcid)
        {
            if (findFirstPkgControlHeaderWhsAndStatusRowQuery == null)
            {
                Expression<Func<ErpContext, string, string, PkgControlHeaderWhsAndStatus>> expression =
              (ctx, company_ex, pcid_ex) =>
                (from row in ctx.PkgControlHeader
                 where row.Company == company_ex &&
                 row.PCID == pcid_ex
                 select new PkgControlHeaderWhsAndStatus() { PkgControlStatus = row.PkgControlStatus, WarehouseCode = row.WarehouseCode, BinNum = row.BinNum, Plant = row.Plant }).FirstOrDefault();
                findFirstPkgControlHeaderWhsAndStatusRowQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlHeaderWhsAndStatusRowQuery(this.Db, company, pcid);
        }

        #endregion

        #region PkgControlItem Queries
        private class PkgControlItemPartialRow2 : Epicor.Data.TempRowBase
        {
            public string PCID { get; set; }
            public int OrderNum { get; set; }
            public string ItemPCID { get; set; }
        }

        private static Func<ErpContext, string, string, int, IEnumerable<PkgControlItemPartialRow2>> selectNestedPCIDsItemsQuery;
        private IEnumerable<PkgControlItemPartialRow2> SelectNestedPCIDsItems(string companyId, string PCID, int orderNum)
        {
            if (selectNestedPCIDsItemsQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, IEnumerable<PkgControlItemPartialRow2>>> expression =
                    (context, companyId_ex, PCID_ex, orderNum_ex) =>
                    (from row in context.PkgControlItem
                     where row.Company == companyId_ex &&
                     row.PCID == PCID_ex &&
                     row.OrderNum != orderNum_ex
                     orderby row.PCIDItemSeq
                     select new PkgControlItemPartialRow2 { PCID = row.PCID, OrderNum = row.OrderNum, ItemPCID = row.ItemPCID });
                selectNestedPCIDsItemsQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectNestedPCIDsItemsQuery(this.Db, companyId, PCID, orderNum);
        }

        private static Func<ErpContext, string, string, bool> existsPkgControlItem2Query;
        private bool CheckPkgControlItemChild(string company, string itemPCID)
        {
            if (existsPkgControlItem2Query == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (context, company_ex, itemPCID_ex) =>
                    (from row in context.PkgControlItem
                     where row.Company == company_ex
                     && row.ItemPCID == itemPCID_ex
                     select row)
                    .Any();
                existsPkgControlItem2Query = DBExpressionCompiler.Compile(expression);
            }

            return existsPkgControlItem2Query(this.Db, company, itemPCID);
        }

        private static Func<ErpContext, string, string, string, string, int, string, bool> existsPkgControlItemQuery;
        private bool ExistsPkgControlItem(string company, string pcid, string partNum, string lotNum, int attributeSetID, string uom)
        {
            if (existsPkgControlItemQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, int, string, bool>> expression =
                    (context, company_ex, pcid_ex, partNum_ex, lotNum_ex, attributeSetID_ex, uom_ex) =>
                    (from row in context.PkgControlItem
                     where row.Company == company_ex
                     && row.PCID == pcid_ex
                     && row.ItemPCID == string.Empty
                     && row.ItemPartNum == partNum_ex
                     && row.ItemLotNum == lotNum_ex
                     && row.ItemIUM == uom_ex
                     && row.ItemAttributeSetID == attributeSetID_ex
                     select row)
                    .Any();
                existsPkgControlItemQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPkgControlItemQuery(this.Db, company, pcid, partNum, lotNum, attributeSetID, uom);
        }

        private static Func<ErpContext, string, string, bool> existsPkgControlItem3Query;
        private bool ExistsPkgControlItem(string company, string itemPCID)
        {
            if (existsPkgControlItem3Query == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                    (context, company_ex, itemPCID_ex) =>
                    (from row in context.PkgControlItem
                     where row.Company == company_ex
                     && (row.ItemPCID == itemPCID_ex
                     || row.PCID == itemPCID_ex)
                     select row)
                    .Any();
                existsPkgControlItem3Query = DBExpressionCompiler.Compile(expression);
            }

            return existsPkgControlItem3Query(this.Db, company, itemPCID);
        }

        private static Func<ErpContext, string, string, string, string, int, string, decimal> findFirstPkgControlItemQtyQuery;
        private decimal FindFirstPkgControlItemQty(string company, string pcid, string partNum, string lotNum, int attributeSetID, string uom)
        {
            if (findFirstPkgControlItemQtyQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, int, string, decimal>> expression =
                    (context, company_ex, pcid_ex, partNum_ex, lotNum_ex, attributeSetID_ex, uom_ex) =>
                    (from row in context.PkgControlItem
                     where row.Company == company_ex
                     && row.PCID == pcid_ex
                     && row.ItemPCID == string.Empty
                     && row.ItemPartNum == partNum_ex
                     && row.ItemLotNum == lotNum_ex
                     && row.ItemIUM == uom_ex
                     && row.ItemAttributeSetID == attributeSetID_ex
                     select row.ItemQuantity).FirstOrDefault();
                findFirstPkgControlItemQtyQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlItemQtyQuery(this.Db, company, pcid, partNum, lotNum, attributeSetID, uom);
        }

        static Func<ErpContext, string, string, IEnumerable<PkgControlItem>> selectPkgControlItemQuery;
        private IEnumerable<PkgControlItem> SelectPkgControlItem(string company, string pcid)
        {
            if (selectPkgControlItemQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<PkgControlItem>>> expression =
      (ctx, company_ex, pcid_ex) =>
        (from row in ctx.PkgControlItem
         where row.Company == company_ex &&
         row.PCID == pcid_ex
         select row);
                selectPkgControlItemQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectPkgControlItemQuery(this.Db, company, pcid);
        }


        private static Func<ErpContext, string, string, PkgControlItem> findFirstPkgControlItem;
        private PkgControlItem FindFirstPkgControlItem(string company, string pcid)
        {
            if (findFirstPkgControlItem == null)
            {
                Expression<Func<ErpContext, string, string, PkgControlItem>> expression =
                    (context, company_ex, pcid_ex) =>
                    (from row in context.PkgControlItem
                     where row.Company == company_ex
                     && row.PCID == pcid_ex
                     select row).FirstOrDefault();
                findFirstPkgControlItem = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlItem(this.Db, company, pcid);
        }

        private class PkgControlItemPartialRow : TempRowBase
        {
            public string ItemPCID { get; set; }
            public string ItemPartNum { get; set; }
            public string ItemPartDesc { get; set; }
            public string ItemLotNum { get; set; }
            public int ItemAttributeSetID { get; set; }
            public string ItemIUM { get; set; }
            public decimal ItemQuantity { get; set; }
        }

        static Func<ErpContext, string, string, IEnumerable<PkgControlItemPartialRow>> selectPkgControlItemPartialQuery;
        private IEnumerable<PkgControlItemPartialRow> SelectPkgControlItemPartial(string company, string pcid)
        {
            if (selectPkgControlItemPartialQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<PkgControlItemPartialRow>>> expression =
              (ctx, company_ex, pcid_ex) =>
                (from row in ctx.PkgControlItem
                 where row.Company == company_ex &&
                 row.PCID == pcid_ex
                 select new PkgControlItemPartialRow()
                 {
                     ItemPCID = row.ItemPCID,
                     ItemPartNum = row.ItemPartNum,
                     ItemPartDesc = row.ItemPartDesc,
                     ItemLotNum = row.ItemLotNum,
                     ItemAttributeSetID = row.ItemAttributeSetID,
                     ItemIUM = row.ItemIUM,
                     ItemQuantity = row.ItemQuantity
                 });
                selectPkgControlItemPartialQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectPkgControlItemPartialQuery(this.Db, company, pcid);
        }

        static Func<ErpContext, string, string, string, IEnumerable<PkgControlItem>> selectPkgControlItemQuery2;
        private IEnumerable<PkgControlItem> SelectPkgControlItem(string company, string pcid, string partNum)
        {
            if (selectPkgControlItemQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, string, IEnumerable<PkgControlItem>>> expression =
      (ctx, company_ex, pcid_ex, partnum_ex) =>
        (from row in ctx.PkgControlItem
         where row.Company == company_ex &&
         row.PCID == pcid_ex &&
         row.ItemPCID == string.Empty &&
         row.ItemPartNum == partnum_ex
         select row);
                selectPkgControlItemQuery2 = DBExpressionCompiler.Compile(expression);
            }

            return selectPkgControlItemQuery2(this.Db, company, pcid, partNum);
        }

        static Func<ErpContext, string, string, PkgControlItem> findLastPkgControlItemQuery;
        private PkgControlItem FindLastPkgControlItem(string company, string pcid)
        {
            if (findLastPkgControlItemQuery == null)
            {
                Expression<Func<ErpContext, string, string, PkgControlItem>> expression =
      (ctx, company_ex, pcid_ex) =>
        (from row in ctx.PkgControlItem
         where row.Company == company_ex &&
         row.PCID == pcid_ex
         select row).LastOrDefault();
                findLastPkgControlItemQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastPkgControlItemQuery(this.Db, company, pcid);
        }

        static Func<ErpContext, string, string, string, string, string, int, string, PkgControlItem> findFirstPkgControlItemWithUpdLockQuery;
        private PkgControlItem FindFirstPkgControlItemWithUpdLock(string company, string pcid, string itemPCID, string itemPartNum, string itemLotNum, int itemAttributeSetID, string itemIUM)
        {
            if (findFirstPkgControlItemWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, int, string, PkgControlItem>> expression =
      (ctx, company_ex, pcid_ex, itemPCID_ex, itemPartNum_ex, itemLotNum_ex, itemAttributeSetID_ex, itemIUM_ex) =>
        (from row in ctx.PkgControlItem.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PCID == pcid_ex &&
         row.ItemPCID == itemPCID_ex &&
         row.ItemPartNum == itemPartNum_ex &&
         row.ItemLotNum == itemLotNum_ex &&
         row.ItemIUM == itemIUM_ex &&
         row.ItemAttributeSetID == itemAttributeSetID_ex
         select row).FirstOrDefault();
                findFirstPkgControlItemWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlItemWithUpdLockQuery(this.Db, company, pcid, itemPCID, itemPartNum, itemLotNum, itemAttributeSetID, itemIUM);
        }

        static Func<ErpContext, string, string, string, bool> findAnyPkgControlItemByPartQuery;
        private bool FindAnyPkgControlItemByPart(string company, string pcid, string partNum)
        {
            if (findAnyPkgControlItemByPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, pcid_ex, partNum_ex) =>
        (from row in ctx.PkgControlItem
         where row.Company == company_ex &&
         row.PCID == pcid_ex &&
         row.ItemPCID == string.Empty &&
         row.ItemPartNum != partNum_ex
         select row).Any();
                findAnyPkgControlItemByPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return findAnyPkgControlItemByPartQuery(this.Db, company, pcid, partNum);
        }

        static Func<ErpContext, string, string, string, bool> findAnyPkgControlItemDifferentPartQuery;
        private bool FindAnyPkgControlItemDifferentPart(string company, string pcid, string partNum)
        {
            if (findAnyPkgControlItemDifferentPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, pcid_ex, partNum_ex) =>
        (from row in ctx.PkgControlItem
         where row.Company == company_ex &&
         row.PCID == pcid_ex &&
         row.ItemPartNum == partNum_ex
         select row).Any();
                findAnyPkgControlItemDifferentPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return findAnyPkgControlItemDifferentPartQuery(this.Db, company, pcid, partNum);
        }

        static Func<ErpContext, string, string, string, string, int, bool> findAnyPkgControlItemByLotQuery;
        private bool FindAnyPkgControlItemByLot(string company, string pcid, string partNum, string lotNum, int attributeSetID)
        {
            if (findAnyPkgControlItemByLotQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, int, bool>> expression =
      (ctx, company_ex, pcid_ex, partNum_ex, lotNum_ex, attributeSetID_ex) =>
        (from row in ctx.PkgControlItem
         where row.Company == company_ex &&
         row.PCID == pcid_ex &&
         row.ItemLotNum != "" &&
         (row.ItemPartNum != partNum_ex || row.ItemLotNum != lotNum_ex || row.ItemAttributeSetID != attributeSetID_ex)
         select row).Any();
                findAnyPkgControlItemByLotQuery = DBExpressionCompiler.Compile(expression);
            }

            return findAnyPkgControlItemByLotQuery(this.Db, company, pcid, partNum, lotNum, attributeSetID);
        }

        static Func<ErpContext, string, string, string, bool> findAnyPkgControlItemByUOMQuery;
        private bool FindAnyPkgControlItemByUOM(string company, string pcid, string uom)
        {
            if (findAnyPkgControlItemByUOMQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, pcid_ex, uom_ex) =>
        (from row in ctx.PkgControlItem
         where row.Company == company_ex &&
         row.PCID == pcid_ex &&
         row.ItemPartNum != "" &&
         row.ItemIUM != uom_ex
         select row).Any();
                findAnyPkgControlItemByUOMQuery = DBExpressionCompiler.Compile(expression);
            }

            return findAnyPkgControlItemByUOMQuery(this.Db, company, pcid, uom);
        }

        static Func<ErpContext, string, string, string, bool> existsMultiplePkgControlItemQuery;
        private bool ExistsMultiplePkgControlItem(string company, string plant, string pcid)
        {
            if (existsMultiplePkgControlItemQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
                (ctx, company_ex, plant_ex, pcid_ex) =>
                (from row in ctx.PkgControlItem
                 where row.Company == company_ex &&
                 row.Plant == plant_ex &&
                 row.PCID == pcid_ex
                 select row).Count() > 1;
                existsMultiplePkgControlItemQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsMultiplePkgControlItemQuery(this.Db, company, plant, pcid);
        }

        static Func<ErpContext, string, string, string, string, string, IEnumerable<PkgControlItemPartialRow>> selectPkgControlItemMatchingPartNumAndUMQuery;
        private IEnumerable<PkgControlItemPartialRow> SelectPkgControlItemMatchingPartNumAndUM(string company, string plant, string pcid, string partNum, string partUM)
        {
            if (selectPkgControlItemMatchingPartNumAndUMQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string, IEnumerable<PkgControlItemPartialRow>>> expression =
                (ctx, company_ex, plant_ex, pcid_ex, partNum_ex, partUM_ex) =>
                (from row in ctx.PkgControlItem
                 where row.Company == company_ex &&
                 row.Plant == plant_ex &&
                 row.PCID == pcid_ex &&
                 row.ItemPCID == string.Empty &&
                 row.ItemPartNum == partNum_ex &&
                 row.ItemIUM == partUM_ex
                 select new PkgControlItemPartialRow()
                 {
                     ItemPCID = row.ItemPCID,
                     ItemPartNum = row.ItemPartNum,
                     ItemLotNum = row.ItemLotNum,
                     ItemIUM = row.ItemIUM,
                     ItemQuantity = row.ItemQuantity
                 });
                selectPkgControlItemMatchingPartNumAndUMQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectPkgControlItemMatchingPartNumAndUMQuery(this.Db, company, plant, pcid, partNum, partUM);
        }

        static Func<ErpContext, string, string, string, bool> existsUniquePkgControlItemWithAnyPartQuery;
        private bool ExistsUniquePkgControlItemWithAnyPart(string company, string plant, string pcid)
        {
            if (existsUniquePkgControlItemWithAnyPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
                (ctx, company_ex, plant_ex, pcid_ex) =>
                (from row in ctx.PkgControlItem
                 where row.Company == company_ex &&
                 row.Plant == plant_ex &&
                 row.PCID == pcid_ex &&
                 !string.IsNullOrEmpty(row.ItemPartNum)
                 select row).Take(2).Count() == 1;
                existsUniquePkgControlItemWithAnyPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsUniquePkgControlItemWithAnyPartQuery(this.Db, company, plant, pcid);
        }

        private static Func<ErpContext, string, string, string, PkgControlItemPartialRow> findFirstPkgControlItemPartialRowQuery;
        private PkgControlItemPartialRow FindFirstPkgControlItemWithPartPartialRow(string company, string plant, string pcid)
        {
            if (findFirstPkgControlItemPartialRowQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, PkgControlItemPartialRow>> expression =
                    (context, company_ex, plant_ex, pcid_ex) =>
                    (from row in context.PkgControlItem
                     where row.Company == company_ex &&
                     row.Plant == plant_ex &&
                     row.PCID == pcid_ex &&
                     !string.IsNullOrEmpty(row.ItemPartNum)
                     select new PkgControlItemPartialRow { ItemPartNum = row.ItemPartNum, ItemPartDesc = row.ItemPartDesc, ItemLotNum = row.ItemLotNum, ItemIUM = row.ItemIUM })
                    .FirstOrDefault();
                findFirstPkgControlItemPartialRowQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlItemPartialRowQuery(this.Db, company, plant, pcid);
        }

        static Func<ErpContext, string, string, string, string, bool> existsUniquePkgControlItemWithPartQuery;
        private bool ExistsUniquePkgControlItemWithPart(string company, string plant, string pcid, string partNum)
        {
            if (existsUniquePkgControlItemWithPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, bool>> expression =
                (ctx, company_ex, plant_ex, pcid_ex, partNum_ex) =>
                (from row in ctx.PkgControlItem
                 where row.Company == company_ex &&
                 row.Plant == plant_ex &&
                 row.PCID == pcid_ex &&
                 row.ItemPCID == string.Empty &&
                 row.ItemPartNum == partNum_ex
                 select row).Take(2).Count() == 1;
                existsUniquePkgControlItemWithPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsUniquePkgControlItemWithPartQuery(this.Db, company, plant, pcid, partNum);
        }

        private static Func<ErpContext, string, string, string, string, PkgControlItemPartialRow> findFirstPkgCtrlItemPartialRowQuery;
        private PkgControlItemPartialRow FindFirstPkgCtrlItemWithPartPartialRow(string company, string plant, string pcid, string partNum)
        {
            if (findFirstPkgCtrlItemPartialRowQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, PkgControlItemPartialRow>> expression =
                    (context, company_ex, plant_ex, pcid_ex, partNum_ex) =>
                    (from row in context.PkgControlItem
                     where row.Company == company_ex &&
                     row.Plant == plant_ex &&
                     row.PCID == pcid_ex &&
                     row.ItemPCID == string.Empty &&
                     row.ItemPartNum == partNum_ex
                     select new PkgControlItemPartialRow { ItemPartNum = row.ItemPartNum, ItemPartDesc = row.ItemPartDesc, ItemLotNum = row.ItemLotNum, ItemIUM = row.ItemIUM })
                    .FirstOrDefault();
                findFirstPkgCtrlItemPartialRowQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgCtrlItemPartialRowQuery(this.Db, company, plant, pcid, partNum);
        }

        static Func<ErpContext, string, string, IEnumerable<PkgControlItem>> selectPkgControlItemWithUpdLockQuery;
        private IEnumerable<PkgControlItem> SelectPkgControlItemWithUpdLock(string company, string pcid)
        {
            if (selectPkgControlItemWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<PkgControlItem>>> expression =
      (ctx, company_ex, pcid_ex) =>
        (from row in ctx.PkgControlItem.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PCID == pcid_ex
         select row);
                selectPkgControlItemWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectPkgControlItemWithUpdLockQuery(this.Db, company, pcid);
        }

        #endregion

        #region PkgControlLabelValue Queries
        static Func<ErpContext, string, string, int, string, string, PkgControlLabelValue> findFirstPkgControlLabelValueQuery;
        private PkgControlLabelValue FindFirstPkgControlLabelValue(string company, string plant, int shipToCustNum, string shipToNum, string partNum)
        {
            if (findFirstPkgControlLabelValueQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, string, string, PkgControlLabelValue>> expression =
      (ctx, company_ex, plant_ex, shipToCustNum_ex, shipToNum_ex, partNum_ex) =>
        (from row in ctx.PkgControlLabelValue
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.ShipToCustNum == shipToCustNum_ex &&
         row.ShipToNum == shipToNum_ex &&
         row.PartNum == partNum_ex
         select row).FirstOrDefault();
                findFirstPkgControlLabelValueQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlLabelValueQuery(this.Db, company, plant, shipToCustNum, shipToNum, partNum);
        }
        #endregion

        #region PkgControlStageHeader Queries
        static Func<ErpContext, string, string, PkgControlStageHeader> findFirstPkgControlStageHeaderQuery;
        private PkgControlStageHeader FindFirstPkgControlStageHeader(string company, string pcid)
        {
            if (findFirstPkgControlStageHeaderQuery == null)
            {
                Expression<Func<ErpContext, string, string, PkgControlStageHeader>> expression =
      (ctx, company_ex, pcid_ex) =>
        (from row in ctx.PkgControlStageHeader
         where row.Company == company_ex &&
         row.PCID == pcid_ex
         select row).FirstOrDefault();
                findFirstPkgControlStageHeaderQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPkgControlStageHeaderQuery(this.Db, company, pcid);
        }
        #endregion PkgControlStageHeader Queries

        #region PkgControlStageItem Queries
        //DJY - Intentionally re-using the PkgControlItemPartialRow collection as it is the same collection
        //of columns required by SelectPkgControlItemPartial and SelectPkgControlStageItemPartial
        static Func<ErpContext, string, string, IEnumerable<PkgControlItemPartialRow>> selectPkgControlStageItemPartialQuery;
        private IEnumerable<PkgControlItemPartialRow> SelectPkgControlStageItemPartial(string company, string pcid)
        {
            if (selectPkgControlStageItemPartialQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<PkgControlItemPartialRow>>> expression =
              (ctx, company_ex, pcid_ex) =>
                (from row in ctx.PkgControlStageItem
                 where row.Company == company_ex &&
                 row.PCID == pcid_ex
                 select new PkgControlItemPartialRow()
                 {
                     ItemPCID = row.ItemPCID,
                     ItemPartNum = row.ItemPartNum,
                     ItemPartDesc = row.ItemPartDesc,
                     ItemLotNum = row.ItemLotNum,
                     ItemAttributeSetID = row.ItemAttributeSetID,
                     ItemIUM = row.ItemIUM,
                     ItemQuantity = row.ItemQuantity
                 });
                selectPkgControlStageItemPartialQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectPkgControlStageItemPartialQuery(this.Db, company, pcid);
        }
        #endregion PkgControlStageItem Queries

        #region PlanContractWhseBin Queries
        static Func<ErpContext, string, string, string, bool> existsPlanContractWhseBinQuery;
        private bool ExistsPlanContractWhseBin(string company, string contractID, string binNum)
        {
            if (existsPlanContractWhseBinQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, contractID_ex, binNum_ex) =>
        (from row in ctx.PlanContractWhseBin
         where row.Company == company_ex &&
         row.ContractID == contractID_ex &&
         row.BinNum == binNum_ex
         select row).Any();
                existsPlanContractWhseBinQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPlanContractWhseBinQuery(this.Db, company, contractID, binNum);
        }

        static Func<ErpContext, string, string, PlanContractWhseBin> findFirstPlanContractWhseBinDefaultInvQuery;
        private PlanContractWhseBin FindFirstPlanContractWhseBinDefaultInv(string company, string contractID)
        {
            if (findFirstPlanContractWhseBinDefaultInvQuery == null)
            {
                Expression<Func<ErpContext, string, string, PlanContractWhseBin>> expression =
      (ctx, company_ex, contractID_ex) =>
        (from row in ctx.PlanContractWhseBin
         where row.Company == company_ex &&
         row.ContractID == contractID_ex &&
         row.DefaultInvWhseBin == true
         select row).FirstOrDefault();
                findFirstPlanContractWhseBinDefaultInvQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlanContractWhseBinDefaultInvQuery(this.Db, company, contractID);
        }

        #endregion PlanContractWhseBin Queries     

        #region PlanContractDtl Queries
        static Func<ErpContext, string, string, string, int, PlanContractDtl> findFirstPlanContractDtlQuery;
        private PlanContractDtl FindFirstPlanContractDtl(string company, string contractID, string partNum, int attributeSetID)
        {
            if (findFirstPlanContractDtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, PlanContractDtl>> expression =
      (ctx, company_ex, contractID_ex, partNum_ex, attributeSetID_ex) =>
        (from row in ctx.PlanContractDtl
         where row.Company == company_ex &&
         row.ContractID == contractID_ex &&
         row.PartNum == partNum_ex &&
         row.AttributeSetID == attributeSetID_ex
         select row).FirstOrDefault();
                findFirstPlanContractDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlanContractDtlQuery(this.Db, company, contractID, partNum, attributeSetID);
        }
        #endregion PlanContractDtl Queries       

        #region PlanContractHdr Queries

        private class WarehouseBinResult
        {
            public string ContractID { get; set; }
            public string NonPCBinAction { get; set; }
            public string NonPCOutsideAction { get; set; }
        }

        static Func<ErpContext, string, string, WarehouseBinResult> findFirstPCWarehouseBinQuery;
        private WarehouseBinResult FindFirstPCWarehouseBin(string company, string contractID)
        {
            if (findFirstPCWarehouseBinQuery == null)
            {
                Expression<Func<ErpContext, string, string, WarehouseBinResult>> expression =
      (ctx, company_ex, contractID_ex) =>
        (from row in ctx.PlanContractHdr
         where row.Company == company_ex &&
         row.ContractID == contractID_ex &&
         row.Active == true
         select new WarehouseBinResult()
         {
             ContractID = row.ContractID,
             NonPCBinAction = row.NonPCBinAction,
             NonPCOutsideAction = row.NonPCOutsideAction
         }
   ).FirstOrDefault();
                findFirstPCWarehouseBinQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPCWarehouseBinQuery(this.Db, company, contractID);
        }

        static Func<ErpContext, string, string, PlanContractHdr> findFirstPlanContractHdrQuery;
        private PlanContractHdr FindFirstPlanContractHdr(string company, string contractID)
        {
            if (findFirstPlanContractHdrQuery == null)
            {
                Expression<Func<ErpContext, string, string, PlanContractHdr>> expression =
      (ctx, company_ex, contractID_ex) =>
        (from row in ctx.PlanContractHdr
         where row.Company == company_ex &&
         row.ContractID == contractID_ex &&
         row.Active == true
         select row).FirstOrDefault();
                findFirstPlanContractHdrQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlanContractHdrQuery(this.Db, company, contractID);
        }

        private class PlanContractHdrWhseBinResult
        {
            public string ContractID { get; set; }
            public string WarehouseCode { get; set; }
            public string BinNum { get; set; }
            public string NonPCOutsideAction { get; set; }
        }

        static Func<ErpContext, string, string, string, PlanContractHdrWhseBinResult> findFirstPlanContractHdrWhseBinQuery;
        private PlanContractHdrWhseBinResult FindFirstPlanContractHdrWhseBin(string company, string warehouseCode, string binNum)
        {
            if (findFirstPlanContractHdrWhseBinQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, PlanContractHdrWhseBinResult>> expression =
      (ctx, company_ex, warehouseCode_ex, binNum_ex) =>
        (from PlanContractWhseBin_row in ctx.PlanContractWhseBin
         join PlanContractHdr_row in ctx.PlanContractHdr
              on new { PlanContractWhseBin_row.Company, PlanContractWhseBin_row.ContractID } equals new { PlanContractHdr_row.Company, PlanContractHdr_row.ContractID }
         where PlanContractHdr_row.Company == company_ex &&
         PlanContractHdr_row.Active == true &&
         PlanContractWhseBin_row.WarehouseCode == warehouseCode_ex &&
         PlanContractWhseBin_row.BinNum == binNum_ex
         select new PlanContractHdrWhseBinResult()
         {
             ContractID = PlanContractHdr_row.ContractID,
             WarehouseCode = PlanContractWhseBin_row.WarehouseCode,
             BinNum = PlanContractWhseBin_row.BinNum,
             NonPCOutsideAction = PlanContractHdr_row.NonPCOutsideAction
         }
    ).FirstOrDefault();
                findFirstPlanContractHdrWhseBinQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlanContractHdrWhseBinQuery(this.Db, company, warehouseCode, binNum);
        }
        #endregion PlanContractHdr Queries

        #region PlantConfCtrl Queries
        static Func<ErpContext, string, string, bool> getPlantBinToBinReqSNQuery;
        private bool GetPlantBinToBinReqSN(string company, string plant)
        {
            if (getPlantBinToBinReqSNQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
                (ctx, company_ex, plant_ex) =>
                    (from row in ctx.PlantConfCtrl
                     where row.Company == company_ex &&
                     row.Plant == plant_ex
                     select row.BinToBinReqSN).FirstOrDefault();
                getPlantBinToBinReqSNQuery = DBExpressionCompiler.Compile(expression);
            }

            return getPlantBinToBinReqSNQuery(this.Db, company, plant);
        }

        static Func<ErpContext, string, string, bool, bool> existsPlantConfCtrlEnablePackageControlQuery;
        private bool ExistsPlantConfCtrlEnablePackageControl(string company, string plant, bool enablePackageControl)
        {
            if (existsPlantConfCtrlEnablePackageControlQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
              (ctx, company_ex, plant_ex, enablePackageControl_ex) =>
                (from row in ctx.PlantConfCtrl
                 where row.Company == company_ex &&
                 row.Plant == plant_ex &&
                 row.EnablePackageControl == enablePackageControl_ex
                 select row).Any();
                existsPlantConfCtrlEnablePackageControlQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPlantConfCtrlEnablePackageControlQuery(this.Db, company, plant, enablePackageControl);
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


        static Func<ErpContext, string, string, bool, bool> existsBinReqSNPlantConfCtrlQuery;
        private bool ExistsBinToBinReqSNPlantConfCtrl(string company, string plant, bool binToBinReqSN)
        {
            if (existsBinReqSNPlantConfCtrlQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool, bool>> expression =
      (ctx, company_ex, plant_ex, binToBinReqSN_ex) =>
        (from row in ctx.PlantConfCtrl
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.BinToBinReqSN == binToBinReqSN_ex
         select row).Any();
                existsBinReqSNPlantConfCtrlQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsBinReqSNPlantConfCtrlQuery(this.Db, company, plant, binToBinReqSN);
        }
        #endregion PlantConfCtrl Queries

        #region PlantShared Queries

        static Func<ErpContext, string, string, string, bool> existsPlantSharedQuery;
        private bool ExistsPlantShared(string company, string plant, string warehouseCode)
        {
            if (existsPlantSharedQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, plant_ex, warehouseCode_ex) =>
        (from row in ctx.PlantShared
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.WarehouseCode == warehouseCode_ex
         select row).Any();
                existsPlantSharedQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPlantSharedQuery(this.Db, company, plant, warehouseCode);
        }


        static Func<ErpContext, string, string, string, bool> existsPlantShared2Query;
        private bool ExistsPlantShared2(string company, string plant, string warehouseCode)
        {
            if (existsPlantShared2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, plant_ex, warehouseCode_ex) =>
        (from row in ctx.PlantShared
         where row.Company == company_ex &&
         (row.Plant == plant_ex ||
          row.RemotePlant == plant_ex) &&
         row.WarehouseCode == warehouseCode_ex
         select row).Any();
                existsPlantShared2Query = DBExpressionCompiler.Compile(expression);
            }

            return existsPlantShared2Query(this.Db, company, plant, warehouseCode);
        }


        static Func<ErpContext, string, string, string, PlantShared> findFirstPlantSharedQuery;
        private PlantShared FindFirstPlantShared(string company, string warehouseCode, string plant)
        {
            if (findFirstPlantSharedQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, PlantShared>> expression =
      (ctx, company_ex, warehouseCode_ex, plant_ex) =>
        (from row in ctx.PlantShared
         where row.Company == company_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.Plant == plant_ex
         select row).FirstOrDefault();
                findFirstPlantSharedQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlantSharedQuery(this.Db, company, warehouseCode, plant);
        }
        #endregion PlantShared Queries

        #region PlantWhse Queries

        static Func<ErpContext, string, string, string, string, string> findFirstPlantWhseQuery;
        private string FindFirstPlantWhse(string company, string plant, string partNum, string warehouseCode)
        {
            if (findFirstPlantWhseQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, string>> expression =
      (ctx, company_ex, plant_ex, partNum_ex, warehouseCode_ex) =>
        (from row in ctx.PlantWhse
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex
         select row.PrimBin).FirstOrDefault();
                findFirstPlantWhseQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlantWhseQuery(this.Db, company, plant, partNum, warehouseCode);
        }

        static Func<ErpContext, string, string, string, string, bool> existsPlantWhseQuery;
        private bool ExistPlantWhse(string company, string plant, string partNum, string warehouseCode)
        {
            if (existsPlantWhseQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, bool>> expression =
      (ctx, company_ex, plant_ex, partNum_ex, warehouseCode_ex) =>
        (from row in ctx.PlantWhse
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex
         select row).Any();
                existsPlantWhseQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPlantWhseQuery(this.Db, company, plant, partNum, warehouseCode);
        }


        static Func<ErpContext, string, string, string, string, PlantWhse> findFirstPlantWhse2Query;
        private PlantWhse FindFirstPlantWhse2(string company, string plant, string partNum, string warehouseCode)
        {
            if (findFirstPlantWhse2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, string, PlantWhse>> expression =
      (ctx, company_ex, plant_ex, partNum_ex, warehouseCode_ex) =>
        (from row in ctx.PlantWhse
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex
         select row).FirstOrDefault();
                findFirstPlantWhse2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlantWhse2Query(this.Db, company, plant, partNum, warehouseCode);
        }


        static Func<ErpContext, string, string, string, string, PlantWhse> findFirstPlantWhse3Query;
        private PlantWhse FindFirstPlantWhse3(string company, string plant, string partNum, string warehouseCode)
        {
            if (findFirstPlantWhse3Query == null)
            {
                Expression<Func<ErpContext, string, string, string, string, PlantWhse>> expression =
      (ctx, company_ex, plant_ex, partNum_ex, warehouseCode_ex) =>
        (from row in ctx.PlantWhse
         where row.Company == company_ex &&
         row.Plant == plant_ex &&
         row.PartNum == partNum_ex &&
         row.WarehouseCode == warehouseCode_ex
         select row).FirstOrDefault();
                findFirstPlantWhse3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPlantWhse3Query(this.Db, company, plant, partNum, warehouseCode);
        }
        #endregion PlantWhse Queries

        #region PORel Queries
        static Func<ErpContext, string, int, int, int, string> existsPORelContractIDQuery;
        private string ExistsPORelContractID(string company, int poNum, int poLine, int poRel)
        {
            if (existsPORelContractIDQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, string>> expression =
      (ctx, company_ex, poNum_ex, poLine_ex, poRel_ex) =>
        (from row in ctx.PORel
         where row.Company == company_ex &&
         row.PONum == poNum_ex &&
         row.POLine == poLine_ex &&
         row.PORelNum == poRel_ex
         select row.ContractID).FirstOrDefault();
                existsPORelContractIDQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsPORelContractIDQuery(this.Db, company, poNum, poLine, poRel);
        }

        #endregion PORel Queries

        #region RcvDtl Queries
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
        #endregion RcvDtl Queries

        #region Reason Queries

        static Func<ErpContext, string, string, string, string> findFirstReasonDescriptionQuery;
        private string FindFirstReasonDescription(string company, string reasontype, string reasoncode)
        {
            if (findFirstReasonDescriptionQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string>> ReasonExpression =
                (ctx, Company, ReasonType, ReasonCode) =>
                    (from row in ctx.Reason
                     where row.Company == Company &&
                     row.ReasonType == ReasonType &&
                     row.ReasonCode == ReasonCode
                     select row.Description).FirstOrDefault();
                findFirstReasonDescriptionQuery = DBExpressionCompiler.Compile(ReasonExpression);
            }
            return findFirstReasonDescriptionQuery(this.Db, company, reasontype, reasoncode) ?? string.Empty;
        }


        static Func<ErpContext, string, string, string, bool> existsReasonQuery;
        private bool ExistsReason(string company, string reasontype, string reasoncode)
        {
            if (existsReasonQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> ReasonExpression =
                (ctx, Company, ReasonType, ReasonCode) =>
                    (from row in ctx.Reason
                     where row.Company == Company &&
                     row.ReasonType == ReasonType &&
                     row.ReasonCode == ReasonCode
                     select row).Any();
                existsReasonQuery = DBExpressionCompiler.Compile(ReasonExpression);
            }
            return existsReasonQuery(this.Db, company, reasontype, reasoncode);
        }

        static Func<ErpContext, string, string, bool> existsReasonQuery2;
        private bool ExistsReason(string company, string reasoncode)
        {
            if (existsReasonQuery2 == null)
            {
                Expression<Func<ErpContext, string, string, bool>> ReasonExpression =
                (ctx, Company, ReasonCode) =>
                    (from row in ctx.Reason
                     where row.Company == Company &&
                     row.ReasonCode == ReasonCode
                     select row).Any();
                existsReasonQuery2 = DBExpressionCompiler.Compile(ReasonExpression);
            }
            return existsReasonQuery2(this.Db, company, reasoncode);
        }


        #endregion

        #region RMARcpt Queries


        static Func<ErpContext, string, int, int, int, RMARcpt> findFirstRMARcptWithUpdLockQuery;
        private RMARcpt FindFirstRMARcptWithUpdLock(string company, int rmanum, int rmaline, int rmareceipt)
        {
            if (findFirstRMARcptWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, RMARcpt>> expression =
      (ctx, company_ex, rmanum_ex, rmaline_ex, rmareceipt_ex) =>
        (from row in ctx.RMARcpt.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.RMANum == rmanum_ex &&
         row.RMALine == rmaline_ex &&
         row.RMAReceipt == rmareceipt_ex
         select row).FirstOrDefault();
                findFirstRMARcptWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstRMARcptWithUpdLockQuery(this.Db, company, rmanum, rmaline, rmareceipt);
        }
        #endregion RMARcpt Queries

        #region SerialMatch Queries

        static Func<ErpContext, string, string, string, SerialMatch> findFirstSerialMatchWithUpdLockQuery;
        private SerialMatch FindFirstSerialMatchWithUpdLock(string company, string childPartNum, string childSerialNo)
        {
            if (findFirstSerialMatchWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, SerialMatch>> expression =
      (ctx, company_ex, childPartNum_ex, childSerialNo_ex) =>
        (from row in ctx.SerialMatch.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.ChildPartNum == childPartNum_ex &&
         row.ChildSerialNo == childSerialNo_ex
         select row).FirstOrDefault();
                findFirstSerialMatchWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSerialMatchWithUpdLockQuery(this.Db, company, childPartNum, childSerialNo);
        }
        #endregion SerialMatch Queries

        #region SerialNo Queries

        static Func<ErpContext, string, string, string, string, bool> existsSerialNoQuery;
        private bool ExistsSerialNo(string company, string partNum, string serialNumber, string binNum)
        {
            if (existsSerialNoQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, bool>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, binNum_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.BinNum == binNum_ex
         select row).Any();
                existsSerialNoQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsSerialNoQuery(this.Db, company, partNum, serialNumber, binNum);
        }


        static Func<ErpContext, string, string, string, SerialNo> findFirstSerialNoQuery;
        private SerialNo FindFirstSerialNo(string company, string partNum, string serialNumber)
        {
            if (findFirstSerialNoQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, SerialNo>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex
         select row).FirstOrDefault();
                findFirstSerialNoQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSerialNoQuery(this.Db, company, partNum, serialNumber);
        }


        static Func<ErpContext, string, string, int, string, bool, string, SerialNo> findFirstSerialNoQuery_2;
        private SerialNo FindFirstSerialNo(string company, string partNum, int attributeSetID, string wareHouseCode, bool voided, string snstatus)
        {
            if (findFirstSerialNoQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, int, string, bool, string, SerialNo>> expression =
      (ctx, company_ex, partNum_ex, attributeSetID_ex, wareHouseCode_ex, voided_ex, snstatus_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.WareHouseCode == wareHouseCode_ex &&
         row.Voided == voided_ex &&
         row.SNStatus == snstatus_ex
         select row).FirstOrDefault();
                findFirstSerialNoQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSerialNoQuery_2(this.Db, company, partNum, attributeSetID, wareHouseCode, voided, snstatus);
        }


        static Func<ErpContext, string, string, string, SerialNo> findFirstSerialNo3Query;
        private SerialNo FindFirstSerialNo3(string company, string partNum, string serialNumber)
        {
            if (findFirstSerialNo3Query == null)
            {
                Expression<Func<ErpContext, string, string, string, SerialNo>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex
         select row).FirstOrDefault();
                findFirstSerialNo3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSerialNo3Query(this.Db, company, partNum, serialNumber);
        }


        static Func<ErpContext, string, string, string, SerialNo> findFirstSerialNoWithUpdLockQuery;
        private SerialNo FindFirstSerialNoWithUpdLock(string company, string partNum, string serialNumber)
        {
            if (findFirstSerialNoWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, SerialNo>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex) =>
        (from row in ctx.SerialNo.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex
         select row).FirstOrDefault();
                findFirstSerialNoWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSerialNoWithUpdLockQuery(this.Db, company, partNum, serialNumber);
        }


        static Func<ErpContext, string, string, string, SerialNo> findFirstSerialNoWithUpdLock2Query;
        private SerialNo FindFirstSerialNoWithUpdLock2(string company, string partNum, string serialNumber)
        {
            if (findFirstSerialNoWithUpdLock2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, SerialNo>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex) =>
        (from row in ctx.SerialNo.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex
         select row).FirstOrDefault();
                findFirstSerialNoWithUpdLock2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSerialNoWithUpdLock2Query(this.Db, company, partNum, serialNumber);
        }


        static Func<ErpContext, string, string, string, SerialNo> findFirstSerialNoWithUpdLock3Query;
        private SerialNo FindFirstSerialNoWithUpdLock3(string company, string partNum, string serialNumber)
        {
            if (findFirstSerialNoWithUpdLock3Query == null)
            {
                Expression<Func<ErpContext, string, string, string, SerialNo>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex) =>
        (from row in ctx.SerialNo.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex
         select row).FirstOrDefault();
                findFirstSerialNoWithUpdLock3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSerialNoWithUpdLock3Query(this.Db, company, partNum, serialNumber);
        }

        static Func<ErpContext, string, string, string, int, int, int, int, IEnumerable<SerialNo>> selectSerialNoForMassUnpickQuery;
        private IEnumerable<SerialNo> SelectSerialNoForMassUnpick(string company, string pcid, string partNum, int attributeSetID, int orderNum, int orderLine, int orderRel)
        {
            if (selectSerialNoForMassUnpickQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, int, int, int, IEnumerable<SerialNo>>> expression =
      (ctx, company_ex, pcid_ex, partNum_ex, attributeSetID_ex, ordernum_ex, orderline_ex, orderrel_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.OrderNum == ordernum_ex &&
         row.OrderLine == orderline_ex &&
         row.OrderRelNum == orderrel_ex &&
         row.PCID == pcid_ex
         select row);
                selectSerialNoForMassUnpickQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectSerialNoForMassUnpickQuery(this.Db, company, pcid, partNum, attributeSetID, orderNum, orderLine, orderRel);
        }

        static Func<ErpContext, string, string, string, int, string, bool, IEnumerable<SerialNo>> selectSerialNoByPCIDAndPartNumWithUpdLockQuery;
        private IEnumerable<SerialNo> SelectSerialNoByPCIDAndPartNumWithUpdLock(string company, string pcid, string partNum, int attributeSetID, string snStatus, bool voided)
        {
            if (selectSerialNoByPCIDAndPartNumWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, string, bool, IEnumerable<SerialNo>>> expression =
              (ctx, company_ex, pcid_ex, partNum_ex, attributeSetID_ex, snStatus_ex, voided_ex) =>
                (from row in ctx.SerialNo.With(LockHint.UpdLock)
                 where row.Company == company_ex &&
                 row.PCID == pcid_ex &&
                 row.PartNum == partNum_ex &&
                 row.AttributeSetID == attributeSetID_ex &&
                 row.SNStatus == snStatus_ex &&
                 row.Voided == voided_ex
                 select row);
                selectSerialNoByPCIDAndPartNumWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectSerialNoByPCIDAndPartNumWithUpdLockQuery(this.Db, company, pcid, partNum, attributeSetID, snStatus, voided);
        }

        static Func<ErpContext, string, string, string, bool> findAnySerialNoByPCIDQuery;
        private bool FindAnySerialNoByPCID(string company, string pcid, string serialNo)
        {
            if (findAnySerialNoByPCIDQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, pcid_ex, serialNo_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.SerialNumber != serialNo_ex &&
         row.PCID == pcid_ex
         select row).Any();
                findAnySerialNoByPCIDQuery = DBExpressionCompiler.Compile(expression);
            }

            return findAnySerialNoByPCIDQuery(this.Db, company, pcid, serialNo);
        }

        static Func<ErpContext, string, string, string, int, string, string, bool, IEnumerable<SerialNo>> selectSerialNoForPCIDIssueQuery;
        private IEnumerable<SerialNo> SelectSerialNoForPCIDIssue(string company, string pcid, string partNum, int attributeSetID, string lotNum, string snStatus, bool voided)
        {
            if (selectSerialNoForPCIDIssueQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, string, string, bool, IEnumerable<SerialNo>>> expression =
      (ctx, company_ex, pcid_ex, partNum_ex, attributeSetID_ex, lotNum_ex, snStatus_ex, voided_ex) =>
        (from row in ctx.SerialNo
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.AttributeSetID == attributeSetID_ex &&
         row.SNStatus == snStatus_ex &&
         row.Voided == voided_ex &&
         row.LotNum == lotNum_ex &&
         row.PCID == pcid_ex
         select row);
                selectSerialNoForPCIDIssueQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectSerialNoForPCIDIssueQuery(this.Db, company, pcid, partNum, attributeSetID, lotNum, snStatus, voided);
        }

        #endregion SerialNo Queries

        #region SNTran Queries

        static Func<ErpContext, string, string, string, int, SNTran> findFirstSNTranQuery;
        private SNTran FindFirstSNTran(string company, string partNum, string serialNumber, int tranNum)
        {
            if (findFirstSNTranQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, int, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranNum_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranNum == tranNum_ex
         select row).FirstOrDefault();
                findFirstSNTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSNTranQuery(this.Db, company, partNum, serialNumber, tranNum);
        }


        static Func<ErpContext, string, string, string, string, SNTran> findFirstSNTranQuery_2;
        private SNTran FindFirstSNTran(string company, string partNum, string serialNumber, string jobNum)
        {
            if (findFirstSNTranQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, string, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, jobNum_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.JobNum == jobNum_ex
         select row).FirstOrDefault();
                findFirstSNTranQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSNTranQuery_2(this.Db, company, partNum, serialNumber, jobNum);
        }


        static Func<ErpContext, string, string, string, int, SNTran> findFirstSNTran2Query;
        private SNTran FindFirstSNTran2(string company, string partNum, string serialNumber, int tranNum)
        {
            if (findFirstSNTran2Query == null)
            {
                Expression<Func<ErpContext, string, string, string, int, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranNum_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranNum > tranNum_ex
         select row).FirstOrDefault();
                findFirstSNTran2Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSNTran2Query(this.Db, company, partNum, serialNumber, tranNum);
        }


        static Func<ErpContext, string, string, string, int, SNTran> findFirstSNTran3Query;
        private SNTran FindFirstSNTran3(string company, string partNum, string serialNumber, int tranNum)
        {
            if (findFirstSNTran3Query == null)
            {
                Expression<Func<ErpContext, string, string, string, int, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranNum_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranNum == tranNum_ex
         select row).FirstOrDefault();
                findFirstSNTran3Query = DBExpressionCompiler.Compile(expression);
            }

            return findFirstSNTran3Query(this.Db, company, partNum, serialNumber, tranNum);
        }


        static Func<ErpContext, string, string, string, string, SNTran> findLastSNTranQuery;
        private SNTran FindLastSNTran(string company, string partNum, string serialNumber, string tranType)
        {
            if (findLastSNTranQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string, SNTran>> expression =
      (ctx, company_ex, partNum_ex, serialNumber_ex, tranType_ex) =>
        (from row in ctx.SNTran
         where row.Company == company_ex &&
         row.PartNum == partNum_ex &&
         row.SerialNumber == serialNumber_ex &&
         row.TranType == tranType_ex
         select row).LastOrDefault();
                findLastSNTranQuery = DBExpressionCompiler.Compile(expression);
            }

            return findLastSNTranQuery(this.Db, company, partNum, serialNumber, tranType);
        }
        #endregion SNTran Queries

        #region TFOrdHed Queries
        static Func<ErpContext, string, string, TFOrdHed> findFirstTFOrdHedQuery;
        private TFOrdHed FindFirstTFOrdHed(string ipCompany, string tfordNum)
        {
            if (findFirstTFOrdHedQuery == null)
            {
                Expression<Func<ErpContext, string, string, TFOrdHed>> expression =
                  (ctx, company_ex, tfordNum_ex) =>
                    (from row in ctx.TFOrdHed
                     where row.Company == company_ex &&
                     row.TFOrdNum == tfordNum_ex
                     select row).FirstOrDefault();
                findFirstTFOrdHedQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTFOrdHedQuery(this.Db, ipCompany, tfordNum);
        }
        #endregion

        #region TFOrdDtl Queries
        static Func<ErpContext, string, string, int, TFOrdDtl> findFirstTFOrdDtlQuery;
        private TFOrdDtl FindFirstTFOrdDtl(string ipCompany, string tfordNum, int tfordLine)
        {
            if (findFirstTFOrdDtlQuery == null)
            {
                Expression<Func<ErpContext, string, string, int, TFOrdDtl>> expression =
                  (ctx, company_ex, tfordNum_ex, tfordLine_ex) =>
                    (from row in ctx.TFOrdDtl
                     where row.Company == company_ex &&
                     row.TFOrdNum == tfordNum_ex &&
                     row.TFOrdLine == tfordLine_ex
                     select row).FirstOrDefault();
                findFirstTFOrdDtlQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstTFOrdDtlQuery(this.Db, ipCompany, tfordNum, tfordLine);
        }
        #endregion

        #region TranDocType Queries

        static Func<ErpContext, string, string, string> existsTranDocTypeIDQuery;
        private string ExistsTranDocTypeID(string company, string systemTranID)
        {
            if (existsTranDocTypeIDQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
      (ctx, company_ex, systemTranID_ex) =>
        (from row in ctx.TranDocType
         where row.Company == company_ex &&
         row.SystemTranID == systemTranID_ex &&
         row.Inactive == false &&
         row.SystemTranDefault == true
         select row.TranDocTypeID).FirstOrDefault();
                existsTranDocTypeIDQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsTranDocTypeIDQuery(this.Db, company, systemTranID);
        }

        #endregion TranDocType Queries

        #region UOM Queries

        static Func<ErpContext, string, string, bool> noRoundingQuery;
        private bool NoRounding(string company, string UOM)
        {
            if (noRoundingQuery == null)
            {
                Expression<Func<ErpContext, string, string, bool>> expression =
      (ctx, company_ex, uom_ex) =>
        (from row in ctx.UOM
         where row.Company == company_ex &&
         row.UOMCode == uom_ex &&
         row.Active == true &&
         row.Rounding == "NOT"
         select row).Any();
                noRoundingQuery = DBExpressionCompiler.Compile(expression);
            }

            return noRoundingQuery(this.Db, company, UOM);
        }


        #endregion UOM Queries

        #region UOMConv Queries

        static Func<ErpContext, string, string, string, bool> existsUOMConvQuery;
        private bool ExistsUOMConv(string company, string uomclassID, string uomcode)
        {
            if (existsUOMConvQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, uomclassID_ex, uomcode_ex) =>
        (from row in ctx.UOMConv
         where row.Company == company_ex &&
         row.UOMClassID == uomclassID_ex &&
         row.UOMCode == uomcode_ex
         select row).Any();
                existsUOMConvQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsUOMConvQuery(this.Db, company, uomclassID, uomcode);
        }


        static Func<ErpContext, string, bool, string, bool> existsUOMConvQuery_2;
        private bool ExistsUOMConv(string company, bool stdUOM, string uomcode)
        {
            if (existsUOMConvQuery_2 == null)
            {
                Expression<Func<ErpContext, string, bool, string, bool>> expression =
      (ctx, company_ex, stdUOM_ex, uomcode_ex) =>
        (from row in ctx.UOMConv
         where row.Company == company_ex &&
         row.StdUOM == stdUOM_ex &&
         row.UOMCode == uomcode_ex
         select row).Any();
                existsUOMConvQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return existsUOMConvQuery_2(this.Db, company, stdUOM, uomcode);
        }


        static Func<ErpContext, string, string, string, UOMConv> findFirstUOMConvQuery;
        private UOMConv FindFirstUOMConv(string company, string uomclassID, string uomcode)
        {
            if (findFirstUOMConvQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, UOMConv>> expression =
      (ctx, company_ex, uomclassID_ex, uomcode_ex) =>
        (from row in ctx.UOMConv
         where row.Company == company_ex &&
         row.UOMClassID == uomclassID_ex &&
         row.UOMCode == uomcode_ex
         select row).FirstOrDefault();
                findFirstUOMConvQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstUOMConvQuery(this.Db, company, uomclassID, uomcode);
        }

        #endregion UOMConv Queries

        #region Warehse Queries

        static Func<ErpContext, string, string, Warehse> findFirstWarehseQuery;
        private Warehse FindFirstWarehse(string company, string warehouseCode)
        {
            if (findFirstWarehseQuery == null)
            {
                Expression<Func<ErpContext, string, string, Warehse>> expression =
      (ctx, company_ex, warehouseCode_ex) =>
        (from row in ctx.Warehse
         where row.Company == company_ex &&
         row.WarehouseCode == warehouseCode_ex
         select row).FirstOrDefault();
                findFirstWarehseQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstWarehseQuery(this.Db, company, warehouseCode);
        }

        static Func<ErpContext, string, string, string> findFirstWarehseDescriptionQuery;
        private string FindFirstWarehseDescription(string company, string warehouseCode)
        {
            if (findFirstWarehseDescriptionQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
              (ctx, company_ex, warehouseCode_ex) =>
                (from row in ctx.Warehse
                 where row.Company == company_ex &&
                 row.WarehouseCode == warehouseCode_ex
                 select row.Description).FirstOrDefault();
                findFirstWarehseDescriptionQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstWarehseDescriptionQuery(this.Db, company, warehouseCode) ?? string.Empty;
        }

        static Func<ErpContext, string, string, string> findFirstWarehsePlantQuery;
        private string FindFirstWarehsePlant(string company, string warehouseCode)
        {
            if (findFirstWarehsePlantQuery == null)
            {
                Expression<Func<ErpContext, string, string, string>> expression =
              (ctx, company_ex, warehouseCode_ex) =>
                (from row in ctx.Warehse
                 where row.Company == company_ex &&
                 row.WarehouseCode == warehouseCode_ex
                 select row.Plant).FirstOrDefault();
                findFirstWarehsePlantQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstWarehsePlantQuery(this.Db, company, warehouseCode) ?? string.Empty;
        }

        static Func<ErpContext, string, string, IEnumerable<Warehse>> selectWarehseQuery;
        private IEnumerable<Warehse> SelectWarehse(string company, string plant)
        {
            if (selectWarehseQuery == null)
            {
                Expression<Func<ErpContext, string, string, IEnumerable<Warehse>>> expression =
      (ctx, company_ex, plant_ex) =>
        (from row in ctx.Warehse
         where row.Company == company_ex &&
         row.Plant == plant_ex
         select row);
                selectWarehseQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectWarehseQuery(this.Db, company, plant);
        }

        #endregion Warehse Queries

        #region Wave Queries


        static Func<ErpContext, string, int, bool, Wave> findFirstWaveWithUpdLockQuery;
        private Wave FindFirstWaveWithUpdLock(string company, int waveNum, bool pickStarted)
        {
            if (findFirstWaveWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool, Wave>> expression =
      (ctx, company_ex, waveNum_ex, pickStarted_ex) =>
        (from row in ctx.Wave.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.WaveNum == waveNum_ex &&
         row.PickStarted == pickStarted_ex
         select row).FirstOrDefault();
                findFirstWaveWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstWaveWithUpdLockQuery(this.Db, company, waveNum, pickStarted);
        }


        static Func<ErpContext, string, int, Wave> findFirstWaveWithUpdLockQuery_2;
        private Wave FindFirstWaveWithUpdLock(string company, int waveNum)
        {
            if (findFirstWaveWithUpdLockQuery_2 == null)
            {
                Expression<Func<ErpContext, string, int, Wave>> expression =
      (ctx, company_ex, waveNum_ex) =>
        (from row in ctx.Wave.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.WaveNum == waveNum_ex
         select row).FirstOrDefault();
                findFirstWaveWithUpdLockQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstWaveWithUpdLockQuery_2(this.Db, company, waveNum);
        }
        #endregion Wave Queries

        #region WaveOrder Queries


        static Func<ErpContext, string, int, bool, bool> existsWaveOrderQuery;
        private bool ExistsWaveOrder(string company, int waveNum, bool bulkPickComplete)
        {
            if (existsWaveOrderQuery == null)
            {
                Expression<Func<ErpContext, string, int, bool, bool>> expression =
      (ctx, company_ex, waveNum_ex, bulkPickComplete_ex) =>
        (from row in ctx.WaveOrder
         where row.Company == company_ex &&
         row.WaveNum == waveNum_ex &&
         row.BulkPickComplete == bulkPickComplete_ex
         select row).Any();
                existsWaveOrderQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsWaveOrderQuery(this.Db, company, waveNum, bulkPickComplete);
        }


        static Func<ErpContext, string, int, int, int, int, string, int, int, string, int, WaveOrder> findFirstWaveOrderWithUpdLockQuery;
        private WaveOrder FindFirstWaveOrderWithUpdLock(string company, int waveNum, int orderNum, int orderLine, int orderRelNum, string jobNum, int assemblySeq, int mtlSeq, string tfordNum, int tfordLine)
        {
            if (findFirstWaveOrderWithUpdLockQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, int, int, string, int, int, string, int, WaveOrder>> expression =
      (ctx, company_ex, waveNum_ex, orderNum_ex, orderLine_ex, orderRelNum_ex, jobNum_ex, assemblySeq_ex, mtlSeq_ex, tfordNum_ex, tfordLine_ex) =>
        (from row in ctx.WaveOrder.With(LockHint.UpdLock)
         where row.Company == company_ex &&
         row.WaveNum == waveNum_ex &&
         row.OrderNum == orderNum_ex &&
         row.OrderLine == orderLine_ex &&
         row.OrderRelNum == orderRelNum_ex &&
         row.JobNum == jobNum_ex &&
         row.AssemblySeq == assemblySeq_ex &&
         row.MtlSeq == mtlSeq_ex &&
         row.TFOrdNum == tfordNum_ex &&
         row.TFOrdLine == tfordLine_ex
         select row).FirstOrDefault();
                findFirstWaveOrderWithUpdLockQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstWaveOrderWithUpdLockQuery(this.Db, company, waveNum, orderNum, orderLine, orderRelNum, jobNum, assemblySeq, mtlSeq, tfordNum, tfordLine);
        }
        #endregion WaveOrder Queries

        #region WhseBin Queries

        static Func<ErpContext, string, string, string, bool> existsInactiveWhseBinQuery;
        private bool ExistsInactiveWhseBin(string company, string warehouseCode, string binNum)
        {
            if (existsInactiveWhseBinQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, bool>> expression =
      (ctx, company_ex, warehouseCode_ex, binNum_ex) =>
        (from row in ctx.WhseBin
         where row.Company == company_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.BinNum == binNum_ex &&
         row.InActive == true
         select row).Any();
                existsInactiveWhseBinQuery = DBExpressionCompiler.Compile(expression);
            }

            return existsInactiveWhseBinQuery(this.Db, company, warehouseCode, binNum);
        }


        static Func<ErpContext, string, string, string, WhseBin> findFirstWhseBinQuery;
        private WhseBin FindFirstWhseBin(string company, string warehouseCode, string binNum)
        {
            if (findFirstWhseBinQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, WhseBin>> expression =
      (ctx, company_ex, warehouseCode_ex, binNum_ex) =>
        (from row in ctx.WhseBin
         where row.Company == company_ex &&
         row.WarehouseCode == warehouseCode_ex &&
         row.BinNum == binNum_ex
         select row).FirstOrDefault();
                findFirstWhseBinQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstWhseBinQuery(this.Db, company, warehouseCode, binNum);
        }


        static Func<ErpContext, string, string, WhseBin> findFirstWhseBinQuery_2;
        private WhseBin FindFirstWhseBin(string company, string binNum)
        {
            if (findFirstWhseBinQuery_2 == null)
            {
                Expression<Func<ErpContext, string, string, WhseBin>> expression =
      (ctx, company_ex, binNum_ex) =>
        (from row in ctx.WhseBin
         where row.Company == company_ex &&
         row.BinNum == binNum_ex
         select row).FirstOrDefault();
                findFirstWhseBinQuery_2 = DBExpressionCompiler.Compile(expression);
            }

            return findFirstWhseBinQuery_2(this.Db, company, binNum);
        }

        static Func<ErpContext, string, string, string, string> findFirstWhseBinDescriptionQuery;
        private string FindFirstWhseBinDescription(string company, string warehouseCode, string binNum)
        {
            if (findFirstWhseBinDescriptionQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, string>> expression =
              (ctx, company_ex, warehouseCode_ex, binNum_ex) =>
                (from row in ctx.WhseBin
                 where row.Company == company_ex &&
                 row.WarehouseCode == warehouseCode_ex &&
                 row.BinNum == binNum_ex
                 select row.Description).FirstOrDefault();
                findFirstWhseBinDescriptionQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstWhseBinDescriptionQuery(this.Db, company, warehouseCode, binNum) ?? string.Empty;
        }
        #endregion WhseBin Queries

        #region XBSyst Queries

        static Func<ErpContext, string, bool> stopOnUOMNoRoundQuery;
        private bool StopOnUOMNoRound(string company)
        {
            if (stopOnUOMNoRoundQuery == null)
            {
                Expression<Func<ErpContext, string, bool>> expression =
      (ctx, company_ex) =>
        (from row in ctx.XbSyst
         where row.Company == company_ex &&
         row.StopOnUOMNoRound == true
         select row).Any();
                stopOnUOMNoRoundQuery = DBExpressionCompiler.Compile(expression);
            }

            return stopOnUOMNoRoundQuery(this.Db, company);
        }


        #endregion XBSyst Queries
    }
}
