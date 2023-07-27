using Epicor.Data;
using Erp.Tables;
using Erp.Tablesets;
using Ice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Strings = Erp.BO.APInvoice.Resources.Strings;
namespace Erp.Services.BO
{
    /// <summary>
    /// AP Invoice Service
    /// </summary>
    public partial class APInvoiceSvc
    {
        /// <summary>
        /// Find Group Id for invoice. For Locate INv. group form.
        /// </summary>
        /// <param name="ipLocGrpInvoiceNum"></param>
        /// <param name="ipLocGrpVendorID"></param>
        /// <param name="outLocGrpVendorID"></param>
        /// <param name="GroupID"></param>
        public void InvokeLocateGroupID(string ipLocGrpInvoiceNum, string ipLocGrpVendorID, out string outLocGrpVendorID, out string GroupID)
        {
            outLocGrpVendorID = ipLocGrpVendorID;
            GroupID = "";
            int vnum = 0;

            if (string.IsNullOrEmpty(ipLocGrpInvoiceNum))
            {
                outLocGrpVendorID = "";
                return;
            }

            if (!string.IsNullOrEmpty(ipLocGrpVendorID))
            {
                var VendorQuery14 = DBExpressionCompiler.Compile(VendorExpression14);
                var vend = VendorQuery14(Db, Session.CompanyID, ipLocGrpVendorID);
                if (vend != null)
                    vnum = vend.VendorNum;
                else
                {
                    throw new BLException(Strings.AValidSupplierIdIsRequired);
                }
            }

            IEnumerable<APInvHed> foundAPInvoices;
            Expression<Func<ErpContext, string, string, int, IEnumerable<APInvHed>>> APInvHedExpression101 =
                        (ctx, company_ex, invoiceNum_ex, vendorNum_ex) => (from ApInvHed in Db.APInvHed
                                                                           where ApInvHed.Company == company_ex &&
                                                                                 ApInvHed.InvoiceNum == invoiceNum_ex &&
                                                                                 (ApInvHed.VendorNum == vendorNum_ex || vendorNum_ex == 0)
                                                                           select ApInvHed);
            var APInvHedQuery101 = DBExpressionCompiler.Compile(APInvHedExpression101);
            foundAPInvoices = APInvHedQuery101(Db, Session.CompanyID, ipLocGrpInvoiceNum, vnum);

            if (foundAPInvoices != null && foundAPInvoices.Any())
            {
                if (foundAPInvoices.Count() > 1 && ipLocGrpVendorID == "")
                {
                    outLocGrpVendorID = "needed";
                    throw new BLException(Strings.EnterVendorID);
                }
                else
                {
                    if (string.IsNullOrEmpty(outLocGrpVendorID))
                        outLocGrpVendorID = this.FindFirstVendor7(Session.CompanyID, foundAPInvoices.First().VendorNum).VendorID;

                    GroupID = foundAPInvoices.First().GroupID;
                }
            }
            else
            {
                throw new BLException(Strings.NoInvoiceLocated);
            }
        }


        /// <summary>
        /// Transfer Selected AP Invoices between groups.
        /// </summary>
        /// <param name="TransferGroupID"></param>
        /// <param name="currentGroupID"></param>
        /// <param name="ds"></param>
        /// <param name="grpNotFound"></param>
        public void TransferAPInvoices(string TransferGroupID, string currentGroupID, ref APInvHedListTableset ds, out string grpNotFound)
        {
            grpNotFound = "";

            if (string.IsNullOrEmpty(TransferGroupID))
            {
                throw new BLException(Strings.GroupIDInvalid);
            }

            if (!ValidateGroupID(TransferGroupID))
            {
                grpNotFound = Strings.GroupIDNotFound;
                return;

            }

            var apInvHedTransferList = ds.APInvHedTransferList;

            if (!apInvHedTransferList.Where(s => s.SelectedForAction == true).Any())
            {
                throw new BLException(Strings.NoRecordsSelected);
            }

            DateTime dtApplyDate;
            bool lEnableGenLegalNum = false;
            decimal grpTotalInvAmt = 0;

            foreach (var row in apInvHedTransferList)
            {
                if (row.SelectedForAction)
                {
                    if (DateTime.TryParse(row["ApplyDate"].ToString(), out dtApplyDate))
                        TransferInvoiceToGroup(TransferGroupID, (int)row["VendorNum"], row["InvoiceNum"].ToString(), dtApplyDate, currentGroupID, out grpTotalInvAmt, out lEnableGenLegalNum);
                    else
                        throw new BLException(Strings.InvalidTransferApplyDate);
                }
            }
        }

        /// <summary>
        /// Validate Invoice Number - find an invoice by number or inform that the number is incorrect (for the Create Cancellation Invoice form).
        /// </summary>
        /// <param name="ipInvoiceNum"></param>
        /// <param name="opVendorNum"></param>
        /// <param name="opMultipleInvcsFound"></param>
        public void ValidateOriginalInvoice(string ipInvoiceNum, out int opVendorNum, out bool opMultipleInvcsFound)
        {

            opVendorNum = 0;
            opMultipleInvcsFound = false;

            if (ipInvoiceNum == "")
                return;
            //InvoiceNum = '{0}' and Posted = true and DebitMemo = false and RefCancelledby = ''

            IEnumerable<APInvHed> foundAPInvoices;
            Expression<Func<ErpContext, string, string, IEnumerable<APInvHed>>> APInvHedExpression102 =
                        (ctx, company_ex, invoiceNum_ex) => (from ApInvHed in Db.APInvHed
                                                             where ApInvHed.Company == company_ex &&
                                                                   ApInvHed.Posted &&
                                                                   !ApInvHed.DebitMemo &&
                                                                   ApInvHed.RefCancelledby == "" &&
                                                                   ApInvHed.InvoiceNum == invoiceNum_ex
                                                             select ApInvHed);
            var APInvHedQuery102 = DBExpressionCompiler.Compile(APInvHedExpression102);
            foundAPInvoices = APInvHedQuery102(Db, Session.CompanyID, ipInvoiceNum);

            if (foundAPInvoices == null || !foundAPInvoices.Any())
                throw new BLException(Strings.InvalidAPInvoice);

            if (foundAPInvoices.Count() == 1)
            {
                opVendorNum = foundAPInvoices.First().VendorNum;
                return;
            }
            else
            {
                opMultipleInvcsFound = true;
            }
        }

        /// <summary>
        /// Set rowMods before Uninvoiced Rcpt Lines Selection
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="InVendorNum"></param>
        /// <param name="InPurPoint"></param>
        /// <param name="InPONum"></param>
        /// <param name="InPackSlip"></param>
        /// <param name="InDropShip"></param>
        /// <param name="InvoiceNum"></param>
        /// <param name="InGRNIClearing"></param>
        public void SelectUninvoicedRcptLines_AdditionalActions(ref APInvReceiptBillingTableset ds, int InVendorNum, string InPurPoint, int InPONum, string InPackSlip, bool InDropShip, string InvoiceNum, bool InGRNIClearing)
        {
            foreach (var row in ds.APReceiptTotals)
            {
                row.RowMod = "U";
            }

            foreach (var row in ds.APUninvoicedReceipts)
            {
                row.RowMod = "U";
            }

            foreach (var row in ds.APSelectedRcptLines)
            {
                row.RowMod = "U";
            }

            foreach (var row in ds.APUninvoicedRcptLines)
            {
                row.RowMod = "U";
            }

            SelectUninvoicedRcptLines(ref ds, InVendorNum, InPurPoint, InPONum, InPackSlip, InDropShip, InvoiceNum, InGRNIClearing);

            var apUninvoicedRcptLines = ds.APUninvoicedRcptLines;
            var apSelectedRcptLines = ds.APSelectedRcptLines;
            var apPReceiptTotals = ds.APReceiptTotals;

            apUninvoicedRcptLines.RemoveAll(l => l.RowMod == "D");


            foreach (var rowLine in apSelectedRcptLines.Where(s => s.RowMod == "U"))
            {
                rowLine.RowMod = "";
            }

            foreach (var rowLine in apPReceiptTotals.Where(s => s.RowMod == "U"))
            {
                rowLine.RowMod = "";
            }



        }

        /// <summary>
        /// Set RowMods before Deselect Selected Rcpt Lines
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="VendorNum"></param>
        /// <param name="PurPoint"></param>
        /// <param name="PackSlip"></param>
        /// <param name="isDropShip"></param>
        /// <param name="InvoiceNum"></param>
        /// <param name="iPONum"></param>
        public void DeselectSelectedRcptLines_AdditionalActions(ref APInvReceiptBillingTableset ds, int VendorNum, string PurPoint, string PackSlip, bool isDropShip, string InvoiceNum, int iPONum)
        {

            foreach (var row in ds.APReceiptTotals)
            {
                row.RowMod = "U";
            }

            foreach (var row in ds.APUninvoicedReceipts)
            {
                row.RowMod = "U";
            }

            foreach (var row in ds.APSelectedRcptLines)
            {
                row.RowMod = "U";
            }

            foreach (var row in ds.APUninvoicedRcptLines)
            {
                row.RowMod = "U";
            }

            DeselectSelectedRcptLines(ref ds, VendorNum, PurPoint, PackSlip, isDropShip, InvoiceNum, iPONum);

            var apUninvoicedRcptLines = ds.APUninvoicedRcptLines;
            var apSelectedRcptLines = ds.APSelectedRcptLines;
            var apPReceiptTotals = ds.APReceiptTotals;

            apSelectedRcptLines.RemoveAll(l => l.RowMod == "D");

            foreach (var rowLine in apUninvoicedRcptLines.Where(s => s.RowMod == "U"))
            {
                rowLine.RowMod = "";
            }

            foreach (var rowLine in apPReceiptTotals.Where(s => s.RowMod == "U"))
            {
                rowLine.RowMod = "";
            }

            //APReceiptTotals

        }

        /// <summary>
        /// Invoke InvoiceSelectedLines with additional actions.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="opLOCMsg"></param>
        public void InvokeInvoiceSelectedLines(ref APInvReceiptBillingTableset ds, out string opLOCMsg)
        {
            var apPSelectedRcptLines = ds.APSelectedRcptLines;
            foreach (var row in apPSelectedRcptLines)
            {
                row.RowMod = "U";
            }

            opLOCMsg = "";
            InvoiceSelectedLines(ref ds, out opLOCMsg);
        }

    }
}