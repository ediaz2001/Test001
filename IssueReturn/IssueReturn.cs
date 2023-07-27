#region Pre 10 Change Comments
/*------------------------------------------------------------------------
    File        : bo/IssueReturn/IssueReturn.p
    Purpose     : Use this function to enter inventory issues or returns.
                    => Issue transactions reduce inventory quantities, and post material costs to jobs in the Job Management module. 
                    => On the other hand, return transactions increase inventory quantities.
                  Use this object for miscellaneous issues or returns. It just reduces or increases the inventory quantity on hand, 
                  and has no job reference.
    Author(s)   : Rajesh Tapde
    Created     : 11-03-2003
Revision History:
-----------------
*** 10/28/08  JeanetteP - All comments prior to 01/19/07 can be found in procedure _History. ***
*** 10/13/09  dparillo - comments dating 07 thru 08 can be found in procedure _History07-08 ***
01/07/09 ROlivares SCR 50779 - Modified PerformMaterialMovement - Added new logic for the inactive bins 
01/15/09 YuriR    SCR 55509 - Modified getToJobSeq - converted Requered, Previously Issued Qty, removed redundant code; 
                              Modified getFromJobSeq - to fix the same issue in [Return Material].
01/21/09 DJY      SCR58588 - rework STK-SHP processing            
02/04/09 andreaP  scr57989 - added code for CMP-STK with managed bins.                   
03/23/09 AlbertoC SCR 60837 - Modified proc PerformMaterialMovement, pass Job/AssemblySeq/JobSeq to proc CheckAllocations.
03/24/09 AbrahamG SCR 59211 - Modified process-STK-MTL, added code to update the PartAlloc.AllocatedQty.
03/25/09 AlbertoC SCR 60924 - Modified proc processMtlQueue, assing fields IssueReturn.TFOrdNum and IssueReturn.TFOrdLine from MtlQueue.
04/01/09 AlbertoC SCR 60921 - Modified proc process-STK-SHP, set PartTran.OrderNum/Line/Rel since these values are used in lib/Appservice.p-AllocatePickedQty, release PartTran when necessary.
                                Modified proc updatePartAlloc, dont convert ttIssueReturn.TranQty for multi tracked uom part, release PartAlloc when necessary.
04/15/09 GlennB   scr 47324 - AMM - Reworked Resource Group and Resource logic from SCR 42182 added lib/getWarehouseInfo.i.
04/15/09 GlennB   SCR 47324 - AMM - Modified lookup statment.  Was causing invalid error 22 and not allowing deletion of Material Queue records after processing.                               
04/15/09 GlennB   SCR 58823 - AMM - Modified code to eliminate negative values and is now working where parts display in thier correct location.
04/21/09 AlbertoC SCR SCR 61039, 60816, 61487 - Modified procs getFromJobSeq, getToJobSeq, onChangeToAssemblySeqCore, onChangeToJobNumCore, 
                                    added call to RoundToUOMDec (lib/Appservice.p) to round material required qty to correct UOM decimals.
04/27/09 AlbertoC SCR 60922 - Added proc updatePartAllocTF, call it on process-STK-PLT to update/create PartAlloc table (based on updatePartAlloc).
04/29/09 SHustings SCR 56409 - Added logic to check serial number allocations when issuing serial tracked material to a job.
04/30/09 JeanetteP SCR 59049 - Modified the ASM-STK/STK-ASM/STK-MTL/MTL-STK processes to correctly apply the rounding difference to the
                               highest component cost instead of always the material component cost.
04/30/09 DebbieP SCR-62221 - fix error logic using IssueReturn table/ JobNum field.                               
05/11/09 JoseRA SCR 62521 - Added code to method GetNewIssueReturn to calculate Requirement Qty using UOM appservices.
05/19/09  DJY  SCR 56342 - create subsequent pick after processing a Wave pick transaction
05/28/09 TatyanaK SCR 62801 - The new field TranDocTypeID was added.
05/28/09 GlennB   SCR 58925 - Modified code to get correct to and from warehouse.
05/28/09 GlennB   SCR 59713 - Modified code to display the correct FROM location when processing the selected queue record from the Material Request Queue.
05/28/09 dparillo SCR 54265 - moved paging block in getListCore.
06/04/09 ReneO    SCR 63347 - Modified methods getFromJobSeq getToJobSeq OnChangeTranQty .- Modified logic to respect the number of decimals added in UOMs, and added
                              functions with the purpose to apply a correct rounding to decimals.
06/19/09 TatyanaK SCR 63580 - It's allowed to create several legal numbers for "StockStock" legal number type.
06/19/09 VictorI  SCR 62817 - PerformMaterialMovement method returns the primary keys of the PartTran records it has created.  
06/19/09 GlennB   SCR 64126 - Needed to condtion Process-WIP-WIP to run the old way since just moving material not advancing to the next operation.
06/25/09 JulioDV  SCR 58406 - Replaced "Find" statements with findtbl.i library.
07/08/09 SHustings SCR 63767 - Added HardAllocation = no/yes to various PartAlloc where clauses
07/14/09 LorenS   SCR 61012 - Opr 375 - Implementation of new JobTypes "PRJ" & "MNT".
07/14/09 dparillo SCR 63885 - modified PerFormMaterialMovement to do FIFO logic if TranType = "PUR-STK".
08/03/09 JulioDV - SCR 65017 - Modified GetSelectSerialNumbersParams, enableSNButton and validateSerialNumber methods to enable serial numbers functionality when doing RMA Disposition of a serial number tracked part.
08/07/09 JulioDV - SCR 65184 - Modified updatePartAlloc method. Added LotNum when creating partAlloc. Needed for Lot-Tracked parts.
                               checkAllocations method in lib/Allocations.i finds partAlloc including LotNum in the where clause.
08/11/09 FelipeM SCR 64898 - Added cOCRNumber paramter to lib/LegalNumberGenerate.p     
10/13/09 dparillo - SCR 67134 - modified getToJobSeq to not return if JobOper not found.
10/15/09 dparillo - SCR 67134 - modified validateJobNum and validateJobSeq for TranType INS-SUB
10/23/09 AlbertoC - SCR 61070 - Modified proc process-STK-MTL, fixed logic to set PartAlloc values.
10/27/09 KrisM  -  SCR 67453 - modified updatePartAlloc and updatePartAllocTF to check for Bin change when checking for whse change when deleting PartAlloc.
10/29/09 dparillo - SCR 63296 - added OnChangingFromBin, modified checkStatusTracking, getSelectSerialNumbersParams, OnChangeFromBinNum, validateSerialNumber
11/13/09 dparillo - SCR 63296 - added var glOverrideBinChange, and modified OnChangeFromBinNum, OnChangingFromBinNum,PerformMaterialMovement, validateSerialNumber
11/17/09 dparillo - SCR 63296 - set glOverrideBinChange in ValidateSN
11/18/09 YeseniaA - SCR 68137 - PerformMaterialMovement procedure was modified to update FSCallHd.MaterialComplete flag in accord with if issue is complete or not. 
01/06/09 AlbertoC - SCR 67491 - Created proc process-INS-SHP, updated logic where necessary to process INS-SHP transaction.
01/13/10 Pradeep  - scr58681 - Minor change.
02/02/10 ABuslenko - SCR 61164 -validateWareHouseCodeBinNum: error message forming corrected.
02/18/10 YulianaR - SCR 52091 - OnChangingJobSeq - Remove a condition for PartWip validation
03/09/10 TatyanaK - SCR 70374 - Transaction Document Type is defaulted then a user chooses a part.
03/24/10 YulianaR - SCR 52091 - OnChangingJobSeq - Add more code to assign the Bin and whse.
04/08/10 YulianaR - SCR 52091 - updatePartWIP - Delete the condition to always create partWip
04/15/10 amiletin - scr 69998 - Added check for TranDocTypeID using lib\ValidateTranDocType.i.
04/20/10 amiletin - scr 69998 - Added check for TranDocTypeID using lib\ValidateTranDocType.i.
06/24/10 amiletin - scr 69998 - Remove validation TranDocTypeID from PerformMaterialMovement.
06/28/10 JulioDV  - SCR 75728 - updatePickedOrders: Added condition for OTS.
07/20/10 GlennB   - SCR 76740 - OnChangingJobSeq - Modified code by replacing PartNum with TrackType = M for WIP-WIP and ADJ-WIP validation.
                                ValidateJobNum- Removed INS-SUB validation which resolves the issue of subcontracting the whole job(including material). 
                                Only subcontract operation is used to inspect parts prior to receiving them into stock.
08/06/10 - Orv R. SCR57730 - Enhancement to allow movement between operations on same assembly.
08/10/10 JeanetteP  SCR 76743 - Added new LotNum parameter when calling im/GenSMIReceipt.p.                                
08/11/10 JeanetteP  SCR 76743/76753 - Fixed error handling when process results into negative FIFO cost/qty.
08/26/10 gjh SCR#59308 - Changes for unpick
09/03/10 gjh SCR#76744 - Changes to support mismatched part and assembly seq.
09/08/10 MZelensky SCR 76394 - getAvailTranDocTypes lib included.
09/08/10 Orv Scr78757 - Move Material Reporting 'There has been no material issued' when material has been issued
09/09/10 CCisneros -  SCR 48271 filter by Bin when PlantConfCtrl.BinToBinReqSN is set to true in GetSelectSerialNumbersParams procedure 
09/10/10 Orv R. SCR57730 - Rework, unable to select Serial#'s after move.
09/17/10 KrisM SCR 78289 - Only assign issueReturn.ToWarehouseCode and ToBinNum if it is blank for STK transaction in getToWhse.
09/22/10 JulioDV - SCR 78930 - process-STK-MTL procedure: Added LotNum to where clause to correct wrong allocations.
09/23/10 MarcoA SCR 78242 - Returning Hold value ToJobNum to buffer in order to can find the jobAsmbl row
09/30/10 gjh SCR#59308 - Serial numbers changed to no longer use ComXRef
10/14/10 djy SCR79519 - Partial material queue movements were not updating PartAlloc correctly
12/03/10 gjh SCR#72047 - Corrected costing calculation for last costed parts that have been through inspection processing
12/08/10 JeanetteP SCR 80528 - Modified process-PUR-STK and process-STK-SHP procedures to consume/add FIFO costs only if doing STK-STK
                               movement of FIFO costed parts from one CostID to a different CostID.
12/9/10 sep SCR 80391 - to whse/bin  defaulted in return assembly not the same as the  from  whse/bin defaulted in  issue assembly for the same  assembly part
01/16/11 irinay scr 69998 - corrected parameters for lib/getAvailTranDocTypes.i
02/03/11 ScottH - SCR 81952 - removed code that was incorrectly attempting to consume PartAlloc records.  PartAlloc consumption is handled by AppService.p as called from PartTran trigger.
02/11/11 AbrahamM - SCR 81561 - xlate(substitute()). Should be substitute(xlate).
02/14/11 dparillo - SCR 79596 - modified validations to allow TranType STK-SHP to use shared warehouses
03/02/11 ccisneros - SCR 82485 Translation project: added missing :U
03/02/11 CCORTEZ - SCR 83295 - added validation check for earliest apply date.
03/18/11 gjh SCR#80487 - updatePartAlloc was not taking into account LotNum which was resulting in two or more partallocs for different lots being combined into one record with the wrong
                         lot number.
03/22/11 AbrahamM - SCR 82485 - Fixed some strings to be translatable.   
04/05/11 KrisM  SCR 84410 - pass date parameter to im/GenSMIReceipt.p   
04/18/11 dparillo SCR 76198 - validateTranQty - added call to run RoundToUOMDec       
04/26/11 amiletin SCR 79413 - Added code to set the default transaction document type.
04/26/11 amiletin SCR 79413 - Added code to set the default transaction document type for every create IssueReturn.
04/28/11 amiletin - SCR 79605 - Added GetTranDocTypeIdDefault to get default Transaction Document Type.
05/16/11  DJY  SCR83581 - enhance processing of Material Queue during Customer Shipment
05/24/11  DJY  SCR57641 - consolidated Material Queue record for Wave Pick transactions
06/03/11 gjh SCR#81234 - Added support for RAU-STK, RMN-STK, RMG-STK
06/21/11 CeciliaL - SCR 67922 - updateSerialNo: Added validation ttIssueReturn.TranType <> "PUR-SUB" and <> "INS-SUB" and <> "DMR-SUB" in order to assign SerialNo.MtlSeq.
07/12/11 Pradeep scr82353 - Added code to enable\disable part number based on user setting.
07/05/11  DJY  SCR 87207 - Job wave pick inadvertently setting PartAlloc.SupplyJobNum
07/27/11 Pradeep scr82353 - End user wanted a message instead of just disabling the field.
08/10/11 JesusC SCR 88038 - Modified processMtlQueue added condition to assign TargetJobNum to ToJobNum when TranType is STK-MTL.
08/17/11 JoseRA SCR 81576 - Added code to process enableSN to allow to skip serial number validation.
08/30/11 dparillo SCR 82618 - modified whereclause for case MaterialQueue, trantype = MFG-OPR in GetSelectSerialNumbersParams
08/30/11 Pradeep scr88568 - getFromType - Changed RMA to RMG.
08/31/11 dparillo SCR 82662 - modified validateSerialNumber to set checkSNalloc = true when coming from Material Queue and type is STK-SHP
09/09/11 CaMorales SCR 88919 - Added a rule to the warehouse bin validation of the types of transaction to void it when the supplier or the customer is the same in both bins, depending in the type of bin.
09/15/11 jhmartinez SCR 88954 - Added OnChangingToJobSeq procedure.
09/20/11 RPerez SCR88907 - Added PreGetNewIssueReturn method.
09/20/11 CaMorales SCR 89261 - Added a rule to the warehouse bin validation of the types of transaction to void it when the movement is from a Supplier Managed bin to a Standard bin.
10/10/11 RPerez SCR88907 - Added support for excluding reserved, allocated, picking and picked quantities in available amount.
10/13/11 RPerez SCR89261 - Added support for creating PUR-STK transaction in process-pur-stk procedure for Replenishment types (RAU-STK, RMN-STK, RMG-STK).
10/13/11 RPerez SCR88907 - Removed support for excluding reserved quantities in available amount. It won't be needed according to PM.
10/17/11 DanielM SCR82945 - Reassign Serial Number to Assembly Enhancements
10/26/11 dparillo SCR 88516 - modified process-STK-INS to create parttran via qa/NCPartTran.i and added updInvQty process.
10/27/11 RPerez SCR88907 - Added code to PerformMaterialMovement to handle replenishment transactions that have been decreased because it exceeds the available amount.
11/04/11 CCORTEZ SCR 89366 - Adding support for Replenishment.
11/07/11 CCORTEZ   SCR 89366 - Removing all code for Replenishment. Everything will be handled by PartBin triggers.
11/15/11 dparillo SCR 89067 - process nonconf material queue records. modified or added: process-MTL-INS, updatePartWIP, updJobMtlCosts, getLEgalNumberType, isValidTranType.
11/30/11 dparillo SCR 89067 - modifications to process-MTL-INS
11/30/11 gjh SCR#88712 - Wrong transUOM and description was being set for subassemblies in getFromJobSeq.
12/05/11 dparillo SCR 89067 - removed updinvqty, modified process-MTL-INS and process-STK-INS
01/06/12 scotth SCR 87317 - Corrected allocation processing in updatePartAllocTF for lot tracked parts
01/13/12 dparillo SCR 91701 - modified process-STK-MTL to set PlantTran.RecIssuedComplete = IssuedComplete.
01/13/12 dparillo SCR 92182 - modified process-STK-MTL in PartTran.TranType = "STK-PLT condition to not updatePartWIP
01/18/12 Pradeep scr59685 - Added code to assign the lot number to serial number records.
01/30/12 JoseRA SCR 60125 - Added code to have HHAutoSelectTransaction included when called from AutoSelect Hand Held screen.
01/25/12 jstevermer SCR 89601 - changed the parameters for calling procedure checkAllocations to properly validate against Employee
02/07/12 YuriR  SCR-92538 - process-MTL-INS: modified condition for PartTran.GLTrans the same as in qa/NCPartTran.i
02/08/12 dparillo SCR 91511 - modified getToJobSeq - clear FromWarehouseCode to by pass condition in getFromWhse 
02/13/12 dparillo SCR 82618 - modified enableSNButton to use fromjob oper when enabling serials for materialqueue process
02/15/12 dparillo SCR 82618 - removed previous change to enableSNButton
02/15/12 JoseRA SCR 81576 - Removed unnecesary code that will never be reached. Changed code when calculate whseOwnerPlant as requested by Debbie Pittman.
02/20/12 RicardoS SCR93376 - Modified validateSerialNumber. The Serial Number window should validate that the entered serial number exists in the From bin 
                             when using Full Serial Tracking and 'Record Serial Numbers on Inventory Moves' = True.
02/22/12 DJY SCR83244 - CheckAllocations needs to be called on a PUR-STK material movement                             
02/24/12 DebbieP SCR-93816 - Multiple changes for mtlqueue Serial processing
02/27/12 DJY SCR83290 - handle when Part Number or UOM is modified on a Shipment line
02/28/12 DebbieP SCR-93819 - checkStatus: set correct status for INS-STK
03/05/12 CaMorales SCR 76022 - Modified the GetNewPartMultiple procedure by doing the same as in GetNewPartNum in each part.
03/05/12 gjh SCR#93800 - Check for shared warehouse when doing STK-STK moves. 
03/05/12 CaMorales SCR 76022 - Added :u to the strings IssueMiscellaneousMaterial and ReturnMiscellaneousMaterial of GetNewPartNum and GetNewPartMultiple procedures.
03/09/12 RicardoS - SCR 91513 - Added new shared temp-table ttPackSlipSNCount to honor the selected serial numbers and create the receipt against the corresponding PO.
03/12/12 DebbieP SCR-94533 - do not reassign SerialNo.AssemblySeq for DMR of subassembly to parent assembly.
03/12/12 gjh SCR#93800 - Create PlantTran record for STK-PLT moves.
03/14/12 ScottH SCR 93193 - Add logic to prevent the user from changing the PartNum when processing MtlQueue records.
03/16/12 DebbieP SCR-94523 - EnableSN = false for PUR-SUB mtlqueue when subcontract oper does not require serial numbers
03/12/12  DJY  SCR93703 - Transfer Order allocation material movement
03/16/12 DebbieP SCR-94523 - checkStatus: INS-SUB is WIP type; GetSelectedSerialNumbersParams: INS-SUB should filter by job/assembly; updateSerialNo - do not change SerialNo.MtlSeq for PUR-INS
03/16/12 SandraN SCR-93820 - OnChangeFromWarehouse: moved the code that wipes the value of the bin and description from the UI to here to not trigger the onChangingFromBinNum which throws an error when the FromBinNum is empty.
03/20/12 jstevermer SCR 94891 - moved the clearing of FromWarehouseCode getToJobSeq to OnChangeToJobSeq
03/16/12 SandraN SCR-93820 - OnChangeFromWarehouse: changed the "for fist" for a find to meet standards.
03/21/12 dparillo SCR 88516 - modified process-MTL-INS (which is also used by STK-INS) to only update NonConf to warehouse and bin
03/21/12 dparillo SCR 88516 - modified validateSerialNumber and GetSelectSerialNumbersParams for STK-INS
04/18/12 CaMorales SCR 87097 - modified process-UKN-STK procedure to have the transaction type back after doing the transaction.
04/23/12 CristianG SCR 67602 - modified procedure onChangeFromWarehouse in order to default Primary Bin quen Warehouse is changed.
04/24/12 CristianG SCR 67602 - Missing closing "".
04/24/12 CaMorales SCR 82861 - Modified getFromWhse procedure from STK transaction, to get the warehouse from the job material when available.
05/10/12 dparillo SCR 60951 - moved lot validation to PrePerformMaterialMovement 
 * 
05/21/12 CristianG SCR 48760 - Added Validation and filters for Reason Codes.
05/21/12 DebbieP SCR-95113 - enableSN: fix enable of serial button for pass inspection to job for subcontract receipt
05/21/12 CristianG SCR 48760 Tunned Reason Codes conditions to avoid conflicts beyond Issue Miscelaneous Materials Entry.
05/23/12 AbrahamM SCR 94543 - Added additional clause to find the correct assembly.
05/30/12 NancyA SCR 79916 - Added functionality to create PartTranSNTran records and in getFromAssemblySeq procedure added buffer to JobAsmbl, also changed find to findtbl according to the standards.
06/15/12 jstevermer SCR 94583 - enableSN fix for tran type ASM-INS
07/23/12 CCORTEZ - SCR 98630 - 2012R Conversion
08/15/12 JesusC  - SCR 98369 - Modified PerformanceMaterialMovement. Changed find first JobMtl to follow a index and added condition for TranType STK-UKN.
09/05/12 jstevermer - SCR 94540 - Modified process-ASM-INS for NonConf warehouse and bin assignments
09/06/12 dparillo - SCR 90188 - Modified GetNewIssueReturn to call getFromJobSeq for MTL-STK
09/26/12 YuriR    - SCR 101370 - Use proc. getPartCostFIFOLayers with two additional parameters (PartNum, Plant). Non-quantity bearing parts should not be allowed to be FIFO costed.
10/19/12 Orv R. SCR101475 - Part OTF UOM Enhancement
10/26/12 YuriR - SCR 97759 - Modified CreatePartTran - Use GetCurrentUserEmpID library to get the EmpID and use it in the PartTran.
10/30/12 YuriR - SCR 97759 - Modified CreatePartTran - use CUR-EMPID to set PartTran.EmpID.
11/08/12 YuriR - SCR 97759 - Simplify changes, because CUR-EMPID uses logic of GetCurrentUserEmpID.
11/19/12 YuriR - SCR 95504 - Modified getToJobSeq - added condition PartWip.TrackType = 'M' in find, set default FromWH, FromBin, Lot
11/19/12 YuriR - SCR 95504 - Standards - added un-translated options.
12/03/12 - Orv - 2012R Conversion
12/06/12 IrisE - SCR 101665 - Modified getToWhse procedure when the cToType is STK, to get the warehouse from the job material when available.
03/05/13 IrisE - SCR 101665 (Rework) - In procedure onChangePartNumCore: Clear TO wareshouse/bin and getToWhse proc will get new default. 
03/18/13 ScottH - SCR 117753 - Rolled back changes made for SCR 91513.
04/04/13 YuriR  - SCR 118326 - Modified onChangePartNumCore: if it's from STK then reset From WH/Bin, so getFromWhse proc gets default. 
06/13/13 YeseniaA SCR 118326 - Modified getFromWhse method: get the warehouse from the job material only when the partNum is the same that the one entered in the field.
07/24/13 ScottH - SCR 120245 - Removed EmpID input paramter from checkAllocations procedure, modified checkAllocations to determine the current employee.
09/26/13 YuriR  - SCR 113763 - Modified getToJobSeq: find at first WIP Part location according to the current IssueReturn record to set correct defaults of From WH/Bin.
10/02/13 ScottH - SCR 125918 - New parameter for shipmentUpdatePartAlloc procedure.
12/05/13 Pradeep scr129647 - Fixed logic bug related to warehouses/shared warehouses.
05/12/21 tguseynov - ERP-32650 - Erp.UI.ReturnMiscEntry.dll.
06/04/21 tguseynov - ERP-32650 - Erp.UI.ReturnMiscEntry.dll

----------------------------------------------------------------------*/
/*
{core/BOBase.i
    &OBJECT_NAME = IssueReturn
}
*/
#endregion Pre 10 Change Comments

extern alias inventoryQtyAdjAlias;
extern alias InvTransferAlias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using Epicor.Data;
using Epicor.Data.SafeSql;
using Epicor.Hosting;
using Epicor.Utilities;
using Erp.Internal.CSF;
using Erp.Internal.SI.FSA;
using Erp.Services.Lib.Resources;
using Erp.Tables;
using Erp.Tablesets;
using Ice;
using Ice.Lib;
using Strings = Erp.BO.IssueReturn.Resources.Strings;

namespace Erp.Services.BO
{
    /// <summary>
    /// Use this function to enter inventory issues or returns.
    /// 1) [Hold] Special public methods for DDSLs FromWhse and ToWhse as they have special conditions.
    /// 2) [ FYI ]This procedure gets called from lots of other program too - 
    /// im/ime20.w flmenu.w am/ame10.w am/ame20-am.p am/ame20-aw.p am/ame20-mm.p am/ame20-mw.p 
    /// Only am/ame20-dg.w uses MtlQueue functionality
    /// </summary>
    public partial class IssueReturnSvc
    {
        #region Class and Variable Declarations

        IssueReturnTableset CurrentFullTableset = new IssueReturnTableset();

        string SkipList = "INS-DMR,SVG-STK,PUR-INS,PLT-MTL,PLT-ASM,DMR-REJ"; /* add Multi-Site PLT-MTL/PLT-MTL and DMR-REJ types to avoid double counting */
        string SubtractList = "MTL-DMR,MTL-INS";
        string TempLegalNumber = string.Empty;
        decimal ConvQty = decimal.Zero;
        int x = 0; /* scr #28357*/
        bool vEnableFIFOLayers = false;

        List<Internal.IM.PartTranSNtranLink.SNtran2> ttSNtranRows = new List<Internal.IM.PartTranSNtranLink.SNtran2>();

        private class JoinTableJobHeadAsmbl
        {
            //JobHead Columns
            private string _JobType = string.Empty;
            public string JobType
            {
                get
                {
                    return this._JobType;
                }

                set
                {
                    this._JobType = value;
                }
            }

            public int CallNum { get; set; }
            private string _JobNum = string.Empty;
            public string JobNum
            {
                get
                {
                    return this._JobNum;
                }

                set
                {
                    this._JobNum = value;
                }
            }

            private string _PartNum = string.Empty;
            public string PartNum
            {
                get
                {
                    return this._PartNum;
                }

                set
                {
                    this._PartNum = value;
                }
            }

            private string _PartDescription = string.Empty;
            public string PartDescription
            {
                get
                {
                    return this._PartDescription;
                }

                set
                {
                    this._PartDescription = value;
                }
            }

            private string _EquipID = string.Empty;
            public string EquipID
            {
                get
                {
                    return this._EquipID;
                }

                set
                {
                    this._EquipID = value;
                }
            }

            //JobAsmbl Columns
            private string _Company = string.Empty;
            public string Company
            {
                get
                {
                    return this._Company;
                }

                set
                {
                    this._Company = value;
                }
            }

            public int AssemblySeq { get; set; }
            private string _Description = string.Empty;
            public string Description
            {
                get
                {
                    return this._Description;
                }

                set
                {
                    this._Description = value;
                }
            }

            public Guid SysRowID { get; set; }
        }
        private JoinTableJobHeadAsmbl ttJoinTableJobHeadAsmbl;
        private List<JoinTableJobHeadAsmbl> ttJoinTableJobHeadAsmblRows = new List<JoinTableJobHeadAsmbl>();

        private class FieldAttribute
        {
            private string _FieldName = string.Empty;
            public string FieldName
            {
                get
                {
                    return this._FieldName;
                }

                set
                {
                    this._FieldName = value;
                }
            }

            public Guid SysRowID { get; set; }
            public bool isSensitive { get; set; }
        }
        private FieldAttribute ttFieldAttribute;
        private List<FieldAttribute> ttFieldAttributeRows = new List<FieldAttribute>();
        private class PackSlipSNCount
        {
            private string _PackSlip = string.Empty;
            public string PackSlip
            {
                get
                {
                    return this._PackSlip;
                }

                set
                {
                    this._PackSlip = value;
                }
            }

            private string _PurPoint = string.Empty;
            public string PurPoint
            {
                get
                {
                    return this._PurPoint;
                }

                set
                {
                    this._PurPoint = value;
                }
            }

            public int SNCount { get; set; }
        }

        List<Erp.Internal.Lib.ProcessFIFO.ReturnMaterialCosts> ttReturnMaterialCostsRows = new List<Erp.Internal.Lib.ProcessFIFO.ReturnMaterialCosts>();

        string partTranPK = string.Empty;
        bool okWhse = false;
        string PW_SaveFromOpcode = string.Empty;
        string PW_SaveFromResourceGrpID = string.Empty;
        int PW_SaveFromAssemblySeq = 0;
        int PW_SaveFromOprSeq = 0;
        int vSaveToAssemblySeq = 0;
        int vSaveToJobSeq = 0;

        internal static class PkgControlStatus
        {
            public const string Stock = "STOCK";
            public const string Empty = "EMPTY";
            public const string Arrived = "ARRIVED";
            public const string TFOPick = "TFOPICK";
            public const string Child = "CHILD";
            public const string Wip = "WIP";
        }

        bool isMalaysiaLocalization = false;

        #endregion Class and Variable Declarations

        #region Implicit buffers
        Erp.Tables.JCSyst JCSyst;
        Erp.Tables.JobAsmbl JobAsmbl;
        Erp.Tables.SerialNo SerialNo;
        Erp.Tables.PlantShared PlantShared;
        Erp.Tables.Part Part;
        Erp.Tables.MtlQueue MtlQueue;
        Erp.Tables.PartTran PartTran;
        Erp.Tables.PlantConfCtrl PlantConfCtrl;
        Erp.Tables.JobMtl JobMtl;
        Erp.Tables.JobOper JobOper;
        Erp.Tables.OpMaster OpMaster;
        Erp.Tables.PartWip PartWip;
        Erp.Tables.WhseBin WhseBin;
        Erp.Tables.PartWhse PartWhse;
        Erp.Tables.PartPlant PartPlant;
        Erp.Tables.PlantWhse PlantWhse;
        Erp.Tables.PartBin PartBin;
        Erp.Tables.JobOpDtl JobOpDtl;
        Erp.Tables.JobHead JobHead;
        Erp.Tables.FSCallhd FSCallhd;
        Erp.Tables.PartFIFOCost PartFIFOCost;
        Erp.Tables.Wave Wave;
        Erp.Tables.WaveOrder WaveOrder;
        Erp.Tables.NonConf NonConf;
        Erp.Tables.PartRev PartRev;
        Erp.Tables.PickedOrders PickedOrders;
        Erp.Tables.OrderDtl OrderDtl;
        Erp.Tables.OrderRel OrderRel;
        Erp.Tables.PlantTran PlantTran;
        Erp.Tables.RcvDtl RcvDtl;
        Erp.Tables.RMARcpt RMARcpt;
        Erp.Tables.PartCost PartCost;
        Erp.Tables.PartAlloc PartAlloc;
        Erp.Tables.SNTran SNTran;
        Erp.Tables.SerialMatch SerialMatch;
        Erp.Tables.Plant Plant;
        Erp.Tables.XaSyst XaSyst;
        Erp.Tables.DMRHead DMRHead;
        Erp.Tables.PkgControlHeader PkgControlHeader;
        Erp.Tables.PkgControlItem PkgControlItem;
        Erp.Tables.PkgControlLabelValue PkgControlLabelValue;
        #endregion

        #region Lazy loads
        private Lazy<Erp.Internal.IM.PartTranSNtranLink> _IMPartTranSNtranLink;
        private Erp.Internal.IM.PartTranSNtranLink IMPartTranSNtranLink { get { return _IMPartTranSNtranLink.Value; } }



        private Lazy<Erp.Internal.IM.IMPlant> iMIMPlant;
        private Erp.Internal.IM.IMPlant IMIMPlant { get { return this.iMIMPlant.Value; } }

        private Lazy<Erp.Internal.Lib.Findpart> libFindpart;
        private Erp.Internal.Lib.Findpart LibFindpart { get { return this.libFindpart.Value; } }

        private Lazy<Erp.Internal.Lib.NegInvTest> libNegInvTest;
        private Erp.Internal.Lib.NegInvTest LibNegInvTest { get { return this.libNegInvTest.Value; } }

        private Lazy<Erp.Internal.Lib.InvCosts> libInvCosts;
        private Erp.Internal.Lib.InvCosts LibInvCosts { get { return this.libInvCosts.Value; } }

        private Lazy<Erp.Internal.Lib.GetResourceGrpID> libGetResourceGrpID;
        private Erp.Internal.Lib.GetResourceGrpID LibGetResourceGrpID { get { return this.libGetResourceGrpID.Value; } }

        private Lazy<Erp.Internal.Lib.OffSet> libOffSet;
        private Erp.Internal.Lib.OffSet LibOffSet { get { return this.libOffSet.Value; } }

        private Lazy<Erp.Internal.Lib.SNFormat> libSNFormat;
        private Erp.Internal.Lib.SNFormat LibSNFormat { get { return this.libSNFormat.Value; } }

        private Lazy<Erp.Internal.Lib.ProcessFIFO> libProcessFIFO;
        private Erp.Internal.Lib.ProcessFIFO LibProcessFIFO { get { return this.libProcessFIFO.Value; } }

        private Lazy<Erp.Internal.Lib.SerialCommon> libSerialCommon;
        private Erp.Internal.Lib.SerialCommon LibSerialCommon { get { return this.libSerialCommon.Value; } }

        private Lazy<Erp.Internal.Lib.Allocations> libAllocations;
        private Erp.Internal.Lib.Allocations LibAllocations { get { return this.libAllocations.Value; } }

        private Lazy<Erp.Internal.Lib.GetAvailTranDocTypes> libGetAvailTranDocTypes;
        private Erp.Internal.Lib.GetAvailTranDocTypes LibGetAvailTranDocTypes { get { return this.libGetAvailTranDocTypes.Value; } }

        private Lazy<Erp.Internal.Lib.EADValidation> libEADValidation;
        private Erp.Internal.Lib.EADValidation LibEADValidation { get { return this.libEADValidation.Value; } }

        private Lazy<Erp.Internal.Lib.AppService> libAppService;
        private Erp.Internal.Lib.AppService LibAppService { get { return this.libAppService.Value; } }

        private Lazy<Erp.Internal.Lib.CreatePartTran> libCreatePartTran;
        private Erp.Internal.Lib.CreatePartTran LibCreatePartTran { get { return this.libCreatePartTran.Value; } }

        private Lazy<Erp.Internal.Lib.DefPartTran> libDefPartTran;
        private Erp.Internal.Lib.DefPartTran LibDefPartTran { get { return this.libDefPartTran.Value; } }

        private Lazy<Erp.Internal.Lib.GetNewSNtran> libGetNewSNtran;
        private Erp.Internal.Lib.GetNewSNtran LibGetNewSNtran { get { return this.libGetNewSNtran.Value; } }

        private Lazy<Erp.Internal.Lib.AsmCostUpdate> libAsmCostUpdate;
        private Erp.Internal.Lib.AsmCostUpdate LibAsmCostUpdate { get { return this.libAsmCostUpdate.Value; } }

        private Lazy<Erp.Internal.Lib.GetWarehouseInfo> libGetWarehouseInfo;
        private Erp.Internal.Lib.GetWarehouseInfo LibGetWarehouseInfo { get { return this.libGetWarehouseInfo.Value; } }

        private Lazy<Erp.Internal.Lib.GetDecimalsNumber> libGetDecimalsNumber;
        private Erp.Internal.Lib.GetDecimalsNumber LibGetDecimalsNumber { get { return this.libGetDecimalsNumber.Value; } }

        private Lazy<Erp.Internal.Lib.GetPlantCostID> libGetPlantCostID;
        private Erp.Internal.Lib.GetPlantCostID LibGetPlantCostID { get { return this.libGetPlantCostID.Value; } }

        private Lazy<Erp.Internal.Lib.RoundAmountEF> libRoundAmountEF;
        private Erp.Internal.Lib.RoundAmountEF LibRoundAmountEF { get { return this.libRoundAmountEF.Value; } }

        private Lazy<Erp.Internal.Lib.GetCostFIFOLayers> libGetCostFIFOLayers;
        private Erp.Internal.Lib.GetCostFIFOLayers LibGetCostFIFOLayers { get { return this.libGetCostFIFOLayers.Value; } }

        private Lazy<Erp.Internal.Lib.LegalNumberGenerate> libLegalNumberGenerate;
        private Erp.Internal.Lib.LegalNumberGenerate LibLegalNumberGenerate { get { return this.libLegalNumberGenerate.Value; } }

        private Lazy<Erp.Internal.Lib.LegalNumberGetDflts> libLegalNumberGetDflts;
        private Erp.Internal.Lib.LegalNumberGetDflts LibLegalNumberGetDflts { get { return this.libLegalNumberGetDflts.Value; } }

        private Lazy<Ice.Lib.NextValue> libNextValue;
        private Ice.Lib.NextValue LibNextValue { get { return this.libNextValue.Value; } }

        private Lazy<Erp.Internal.Lib.ValidateTranDocType> libValidateTranDocType;
        private Erp.Internal.Lib.ValidateTranDocType LibValidateTranDocType { get { return this.libValidateTranDocType.Value; } }

        private Lazy<Erp.Internal.Lib.NonQtyBearingBin> libNonQtyBearingBin;
        private Erp.Internal.Lib.NonQtyBearingBin LibNonQtyBearingBin { get { return this.libNonQtyBearingBin.Value; } }

        private Lazy<Erp.Internal.Lib.MtlRcpt> libMtlRcpt;
        private Erp.Internal.Lib.MtlRcpt LibMtlRcpt { get { return this.libMtlRcpt.Value; } }

        private Lazy<Ice.Lib.ExecuteQuery> libExecuteQuery;
        private Ice.Lib.ExecuteQuery LibExecuteQuery { get { return this.libExecuteQuery.Value; } }

        private Lazy<Erp.Internal.IM.GenSMIReceipt> libGenSMIReceipt;
        private Erp.Internal.IM.GenSMIReceipt LibGenSMIReceipt { get { return this.libGenSMIReceipt.Value; } }

        private Lazy<Erp.Internal.Lib.DeferredUpdate> libDeferredUpdate;
        private Erp.Internal.Lib.DeferredUpdate LibDeferredUpdate { get { return this.libDeferredUpdate.Value; } }

        private Lazy<Erp.Internal.Lib.GetNextOprSeq> _GetNextOprSeq;
        private Erp.Internal.Lib.GetNextOprSeq GetNextOprSeq { get { return this._GetNextOprSeq.Value; } }

        private Lazy<Erp.Internal.Lib.PackageControl> libPackageControl;
        private Erp.Internal.Lib.PackageControl LibPackageControl { get { return this.libPackageControl.Value; } }

        private Lazy<Erp.Internal.Lib.PackageControlValidations> libPackageControlValidations;
        private Erp.Internal.Lib.PackageControlValidations LibPackageControlValidations { get { return this.libPackageControlValidations.Value; } }


        private Lazy<Erp.Internal.Lib.PkgControlAdjustReturnContainer> _libAdjustReturnContainer;
        private Erp.Internal.Lib.PkgControlAdjustReturnContainer LibAdjustReturnContainer { get { return this._libAdjustReturnContainer.Value; } }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected override void Initialize()
        {
            this._IMPartTranSNtranLink = new Lazy<Erp.Internal.IM.PartTranSNtranLink>(() => new Erp.Internal.IM.PartTranSNtranLink(this.Db));
            this.iMIMPlant = new Lazy<Erp.Internal.IM.IMPlant>(() => new Erp.Internal.IM.IMPlant(Db));
            this.libNegInvTest = new Lazy<Erp.Internal.Lib.NegInvTest>(() => new Erp.Internal.Lib.NegInvTest(Db));
            this.libInvCosts = new Lazy<Erp.Internal.Lib.InvCosts>(() => new Erp.Internal.Lib.InvCosts(Db));
            this.libGetResourceGrpID = new Lazy<Erp.Internal.Lib.GetResourceGrpID>(() => new Erp.Internal.Lib.GetResourceGrpID(Db));
            this.libOffSet = new Lazy<Erp.Internal.Lib.OffSet>(() => new Erp.Internal.Lib.OffSet(Db));
            this.libSNFormat = new Lazy<Erp.Internal.Lib.SNFormat>(() => new Erp.Internal.Lib.SNFormat(Db));
            this.libProcessFIFO = new Lazy<Erp.Internal.Lib.ProcessFIFO>(() => new Erp.Internal.Lib.ProcessFIFO(Db));
            this.libFindpart = new Lazy<Erp.Internal.Lib.Findpart>(() => new Erp.Internal.Lib.Findpart(Db));
            this.libSerialCommon = new Lazy<Erp.Internal.Lib.SerialCommon>(() => new Erp.Internal.Lib.SerialCommon(Db));
            this.libAllocations = new Lazy<Erp.Internal.Lib.Allocations>(() => new Erp.Internal.Lib.Allocations(Db));
            this.libGetAvailTranDocTypes = new Lazy<Erp.Internal.Lib.GetAvailTranDocTypes>(() => new Erp.Internal.Lib.GetAvailTranDocTypes(Db));
            this.libEADValidation = new Lazy<Erp.Internal.Lib.EADValidation>(() => new Erp.Internal.Lib.EADValidation(Db));
            this.libAppService = new Lazy<Erp.Internal.Lib.AppService>(() => new Erp.Internal.Lib.AppService(Db));
            this.libCreatePartTran = new Lazy<Erp.Internal.Lib.CreatePartTran>(() => new Erp.Internal.Lib.CreatePartTran(Db));
            this.libDefPartTran = new Lazy<Erp.Internal.Lib.DefPartTran>(() => new Erp.Internal.Lib.DefPartTran(Db));
            this.libGetNewSNtran = new Lazy<Erp.Internal.Lib.GetNewSNtran>(() => new Erp.Internal.Lib.GetNewSNtran(Db));
            this.libAsmCostUpdate = new Lazy<Erp.Internal.Lib.AsmCostUpdate>(() => new Erp.Internal.Lib.AsmCostUpdate(Db));
            this.libGetWarehouseInfo = new Lazy<Erp.Internal.Lib.GetWarehouseInfo>(() => new Erp.Internal.Lib.GetWarehouseInfo(Db));
            this.libGetDecimalsNumber = new Lazy<Erp.Internal.Lib.GetDecimalsNumber>(() => new Erp.Internal.Lib.GetDecimalsNumber(Db));
            this.libGetPlantCostID = new Lazy<Erp.Internal.Lib.GetPlantCostID>(() => new Erp.Internal.Lib.GetPlantCostID(Db));
            this.libRoundAmountEF = new Lazy<Erp.Internal.Lib.RoundAmountEF>(() => new Erp.Internal.Lib.RoundAmountEF(Db));
            this.libGetCostFIFOLayers = new Lazy<Erp.Internal.Lib.GetCostFIFOLayers>(() => new Erp.Internal.Lib.GetCostFIFOLayers(Db));
            this.libLegalNumberGenerate = new Lazy<Erp.Internal.Lib.LegalNumberGenerate>(() => new Erp.Internal.Lib.LegalNumberGenerate(Db));
            this.libLegalNumberGetDflts = new Lazy<Erp.Internal.Lib.LegalNumberGetDflts>(() => new Erp.Internal.Lib.LegalNumberGetDflts(Db));
            this.libValidateTranDocType = new Lazy<Erp.Internal.Lib.ValidateTranDocType>(() => new Erp.Internal.Lib.ValidateTranDocType(Db));
            this.libNonQtyBearingBin = new Lazy<Erp.Internal.Lib.NonQtyBearingBin>(() => new Erp.Internal.Lib.NonQtyBearingBin(Db));
            this.libMtlRcpt = new Lazy<Erp.Internal.Lib.MtlRcpt>(() => new Erp.Internal.Lib.MtlRcpt(Db));
            this.libExecuteQuery = new Lazy<Ice.Lib.ExecuteQuery>(() => new Ice.Lib.ExecuteQuery(Db));
            this.libNextValue = new Lazy<Ice.Lib.NextValue>(() => new Ice.Lib.NextValue(Db));
            this.libGenSMIReceipt = new Lazy<Internal.IM.GenSMIReceipt>(() => new Erp.Internal.IM.GenSMIReceipt(Db));
            this.libDeferredUpdate = new Lazy<Internal.Lib.DeferredUpdate>(() => new Erp.Internal.Lib.DeferredUpdate(Db));
            this._GetNextOprSeq = new Lazy<Erp.Internal.Lib.GetNextOprSeq>(() => new Erp.Internal.Lib.GetNextOprSeq(Db));
            this.libPackageControl = new Lazy<Erp.Internal.Lib.PackageControl>(() => new Erp.Internal.Lib.PackageControl(Db));
            this.libPackageControlValidations = new Lazy<Erp.Internal.Lib.PackageControlValidations>(() => new Erp.Internal.Lib.PackageControlValidations(Db));
            this._libAdjustReturnContainer = new Lazy<Erp.Internal.Lib.PkgControlAdjustReturnContainer>(() => new Internal.Lib.PkgControlAdjustReturnContainer(Db));

            switch (Session.CountryCode.ToUpperInvariant())
            {
                case "MY":
                    {
                        isMalaysiaLocalization = true;
                    }
                    break;
            }

            //libgetavailtrandoctypes = new Lib.GetAvailTranDocTypes(Db, "StockWIP,StockStock,WIPStock");
            base.Initialize();
        }
        private void _History()
        {
            //<summary>
            //  Purpose:
            //  Parameters:  none
            //  Notes:
            //</summary>
            /***
            04/21/04 Pradeep scr13149 - Changed WrkCenter to ResourceGroup.
            05/04/04 Terry   scr13149 - Changed WrkCenter to ResourceGroup.
            05/05/04 Terry   Implement standard functions for getting ResourceGrpID.
            08/17/04 Raj     scr13578  - Implement 6.1 SCR's into bo/IssueReturn/IssueReturn.p.
            09/15/04 Pradeep scr16286 - Missing Warehouse and Bin description.
            09/15/04 Raj     scr 16480, 16488, 16486, 16487, 16465 - SCRs related to error messages and field labels.
            09/22/04 Raj     scr 16527  On Hand field displays Zero.
            09/22/04 Raj     scr 16553  Issue Material - Issued Complete is not displayed checked as default.
            10/04/04 Raj     scr 17236  Cannot select already existent serial numbers to issue.
            10/08/04 smr     scr16457 - Rework legal numbers.
            10/26/04 smr     scr16457 - Rework legal numbers.
            11/02/04 smr     scr17605 - Serial Number SNStatus was not being set properly.
            11/02/04 smr     scr17605 - Store Order info for a serial number in ComXref.
            11/08/04 Pradeep scr17744 - MfgSys global Variable is Obsolete with new Licensing configuration.
            11/09/04 sev/lws scr 8295 - Fix OnHand Qty calculation.
            11/18/04 Pradeep scr17821 Issues/returns BL needs to check to AMM license.
            12/06/04 Raj     scr18867 - Material Issue defaults "Issue Complete" for partial issued quantity
            12/15/04 JSP     SCR #19110 - Modified the getPartTranTotalCost method to correct how
                             we calculate the total transaction quantity.  Made sure that the new
                             PartTran is not included in the for each.  There was a timing issue
                             as to when the new PartTran is committed in the database.
            12/29/04 bpm     scr 19252 - Modified so that it doesn't default the quantity value.
            01/10/04 sev     scr19561, scr19562: wrong Inventory Adjustment GL acct in stk-ukn transactions
            02/23/05 Pradeep scr19796 Issue Material with no part/plant gives no message.
            03/02/05 smr     scr20765 Issue Material from MtlQueue was not default values properly.
            03/03/05 ldb     scr21060 Added temp tabler ttIssueReturn to call to PUR-STK from STK-STK.
            03/15/05 DJP     scr20988 Method processMtlQueue set IssueReturn.UM from MtlQueue.IUM
            03/16/05 ldb     scr 21462 Add sortby to get rows.
            03/17/05 gdd     scr 21271 validateLotNum procedure was modified to allow saving the record without Lot number entered.
            05/02/05 Pradeep scr22264  Issue Material - lot number should be required.
            05/06/05 laq     scr21069 Added validation for the AMM license when setting the 'To Warehouse' and 'To Bin' fields in the getToWhse procedure.
            05/10/05 Raj     scr22230 - Added logic to handle PLT-STK transaction type via internal proc process-PLT-STK.
                                        This new procedure is required for moving Transfer order materials. Handled like PUR-STK.
            06/09/05 laq     scr22719 - Added disableFromToFields() method which determines when the "To" and "From" whse/bin fields should be enabled.
            06/23/05 smr     scr23309 - PartWip records should not be created for JobOper records that are complete.
            08/10/05 bpm     scr24362 - Added MFG-SHP to IsValidTranType list.  Wasn't allowing shipment from job.
            09/21/05 JSP     SCR 25115 - Modified the determineSensitivity to correctly set LotNum sensitivity.
            10/10/05 gjh     n/a - Added GetUnissuedQty() and updated obsolete parseSortBy.i calls in getListCore() to use ParseSort().
            10/28/05 Pradeep scr25820  Serial number issued twice to the same job.
            11/01/05  JSP    SCR #25757 - Modified the updatePartWip procedure to remove the
                             corresponding PatchFld (FromOpCode) record when deleting PartWip.
            11/16/05 - alh - SCR 26259 - Added  (Non standard method), just to know, if is a valid assembly
            12/09/05 - mfg - SCR#26667 - Modified the updateSerialNo method to validate if the SerialNo table is available before trying to access it.
            12/22/05 - jkm - SCR#26799 - Modified the validateJobNum method to validate if the AssemblySeg=0 .
            12/27/05 - jkm - SCR#26801 - Modified the getListCore method, "JobReleased" filter should be in Whereclause.
            12/22/05 - jkm - SCR#26914 - Modified the validateJobNum method to validate if the AssemblySeg=0 for TranType  = "ASM-STK".
            01/05/06 - plp - SCR 26594 - Modified  method to give better error messages when a job is entered that is
                                         closed or not released.
            01/24/06  DVE   scr27413 - XML Comments in BO Public methods are cleanup.
            02/06/06  smr   scr28025 - Implement lib/createPartTran.i to insure unique values for the primary index.
            02/13/06  LWS   scr27867 - Default and Validate warehouse and bin for Non-Multi-Warehouse companies.
            02/13/06  plp  SCR 28063 - In setIssuedComplete, for issues, don't set IssueReturn.IssuedComplete to true when TranQty, QtyRequired and
                                       QtyPreviouslyIssued are all 0.
            03/06/06  alf   scr#28357 - Dummy key field calculation modified in GetNewPartMultiple and getNewPartNumCore
            05/10/06 Pradeep Job:803 - Added code to not update bin quantity if not quantity bearing.
            05/31/06 YuriR SCR-29249 - Modified determineSensitivity set Sesitivity of WarehouseFrom, BinFrom to "no" for ADJ-MTL and ADJ-WIP.
            08/14/06 Orv R. SCR31477 - Created logic to handle Material Queue records for picking of Kit Parent and Kit Components.
                                       Introduced new MtlQueue.TranType of Kit-Shp (Kit Parent pick for shiping) and
                                       CMP-SHP (Kit component pick for shiping),
            08/17/06 JuliaK scr31630 - JobExists public procedure was added.
            09/19/06 AlbertoC SCR 32373 - Changes to UpdateSerialNo procedure to use 'Find first'.
                                            Changes in checkStatus and serialNoStatus to add MFG-OPR.
            09/22/06 AlbertoC SCR 32373 - Changes in updateSerialNo to assign correct status when an Order is linked to a Job and not picking.
            10/10/06 SergeyM  SCR 29157 - Default and Validate To warehouse and bin for Non-Multi-Warehouse companies.
            10/11/06 EduardoR SCR32045 - Added logic to handle INS-DMR transaction. For now, for this transaction no actions need to be done so
                                         I just created the procedure for consistency so the error message dont get displayed.
            10/23/06 CNash  SCR 27877 - Renamed  to IsValidAssembly to be consistent with our naming conventions.
            12/18/06 GlennB SCR 30682 - DummyKeyField and RowIdent were not unique so I set fields to UserID + a counter instead to make them unique.
            12/22/06 YuriB  SCR 41230 - PerformMaterialMovement procedure : NegativeInventoryTest call use invalid qty parmeter, with negative sign.
            ***/
        }
        /// <summary>
        /// 
        /// </summary>
        public void _History07_08()
        {
            //<summary>
            //  Purpose:     
            //  Parameters:  none
            //  Notes:       
            //</summary>
            /***
            01/19/07 - EduardoR SCR40899 - Added Object_Name argument to core/NonBOBase.i include file to enforced security.
            01/19/07 jajohnson scr 41940 - modify PartBin update to use temp-tables
            01/26/07 - DanielM SCR 41850 - Modified updateSerialNo, it should only check for ttSelectedSerialNumbers rows with RowMod = A or U, not "".
            02/07/07 PPhillips SCR 42158 - Update ttIssueReturn.OnHandQty after material is issued.
            03/02/07 jajohnson scr 42610 - some transactions were subtracting instead of adding due to SCR 41940
            03/28/07 Orv R. SCR43256 - Consider DMR-SUB trantype as DMR status for serial number selection
            05/11/07 IrinaY scr42182 - modified getFromWhse() and getToWhse() - default value whse/bin for Job operation should be
                                        from Job operation's Resource, if it is set and it is Location
            05/17/07 Orv R. SCR44049 - INS-SUB not updating PartWip correctly.
                                       Also fixed "unexpected error.."  app LOG would have "getToWhse bo/IssueReturn/IssueReturn.p' Line:18234) ** No Resource record is available. (91)"
                                       Also fixed issue of incorrect "TO" warehouse/bin setting due to scr42182.
                                       Added logic to handle "SUB-INS" instead of rejecting trans as invalid type.
            05/24/07 dparillo SCR44737 - added MTL-MTL (Move Material) to checkStatus
            05/29/07 dparillo SCR44047 - added procedure process-DMR-ASM. Added DMR-ASM tp function isValidTranType.
            05/30/07 ludmilaS SCR43166 - assigning of cFromType for pcTranType="INS-MTL" is removed from function getFromType to prevent assigning of FromWarehouseCode from resources.
            05/31/07 IrinaY SCR 42603 - modified getFromJobSeq, in case WIP-WIP transaction to get default From Whse/Bin for Part and selected Job Operation, and recalculate OnHand.
            06/22/07 dparillo SCR44047RW - added processes DMR-MTL, DMR-STK and DMR-SUB and added to isValidTranType.
            07/11/07 andreaP scr 45654 - call to generatePlantReceiptTran was not passing all the necessary parameters.
            07/19/07 ludmilaS SCR 43166 - Modified getToWhse procedure: when creating transaction to material, if ToWarehouseCode is not filled, it comes from Resource or ResourceGroup,
                                          otherwise it comes from ttIssueReturn.
            07/23/07 HeribertoC SCR 44994 - Warehouse set by default was not the correct one, getToWhse was modified.
            07/27/07 DanielM SCR 45861 - Modified process-CMP-SHP, when trying to get the first component of the sales kit it was trying to find it by comparing
                                        the DisplaySeq and OrderLine, i modified it to get the first component available that is defined as a child of the parent line.
            08/03/07 VladimirD SCR 41434 - Checked the JobMtl record and validated if the IssuedComplete field was already set to True,
                                            if so i set the IssueReturn.IssuedComplete field to true as well since this means that the user had checked that box before.
            08/23/07 IrinaY SCR 43203 - modified OnChangeDimCode to able at first select necessary Dimention and after it set its Qty.
            08/27/07 AbrahamG SCR 46310 - A line code validation was added to validatePartNum procedure in IssueReturn.p uisin trim function to avoid blanks to be saved in PartNum field.
            10/01/07 JoseRA SCR 46783 - Modified procedure zerottIssueReturn to keep value of field PartTrackLots from previous data retrived
            10/30/07 Pradeep scr47341 - Some move type transactions prompting for serial numbers when they shouldn't be.
            11/29/07 HeribertoC SCR 40761 - lib\GetNewSNTran.p was implemented for creation of new records in SNTran table.
            01/08/08 JeanetteP  SCR 40698 - Added new logic for the FIFO costing project.
            01/18/08 JulioDV SCR 48058 - zerottIssueReturn method - Assigned hold-ttIssueReturn.PartTrackSerialNum field value to ttIssueReturn.PartTrackSerialNum field.
            01/29/08 PAULOS SCR 40761 - Added some code to indicate whether the serial number button will be enable when
                                        transaction data is changed by the user in the UI, this would be fields
                                        such as the Part Number, from job or warehouse; or to job or warehouse,
                                        Assembly number, job Mtl number.
                        - Added the following methods:
                                            checkStatusTracking
                                            enableSNButton
                                            getJobHeadPart
                                            getOwnerPlant
                                            partSerialTracking
                                            plantSerialTracking
                                            onChangeToWarehouse
                                            onChangeFromWarehouse
                                            snRequired

                                      - Modified the following methods adding the parameter 'call process':
                                            GetNewJobAsmblMultiple
                                            OnChangePartNum
                                            OnChangeToAssemblySeq
                                            OnChangeToJobSeq
                                            OnChangeFromJobNum
                                            OnChangeFromAssemblySeq
                                            OnChangeFromJobSeq
            01/30/08 VictorI SCR 40458 (opr.379) - Review - Missed Rounding in server code.
            02/20/08 DebbieP SCR-40761 - Serial tracking changes for 9.0
            02/22/08 DebbieP SCR-40761 - Returns processing for 9.0 serial tracking
            02/28/08 AlexanderP SCR-40650 (opr. 360) - Rounding of variables.
            02/28/08 DebbieP SCR-40761 - additional change for MaterialQueue
            02/29/08 AlexanderP SCR 40650 (opr. 360) - Rounding of variables and fields according to the currency setup.
            03/03/08 EduardoR SCR 40600 - Modified PerformMaterialMovement to send the correct UOM to NegativeInventoryTest.
            03/06/08 Orv SCR40600 - PartWip Quantity will be in the Requirements UOM.
                                    Removed code to set ttIssueReturn.OnHandQty, this will no longer be used in UI's.
            03/16/08 JeanetteP SCR 40698 - Added logic for the job cost rollup enhancement.
            03/25/08 gjh SCR#40761 - Made serial changes to handle UOM to GetSelectSerialNumberParams and PerformMaterialMovement
            03/26/08 andreaP scr40600 - Inventory UOM modifications.
            03/28/08 HeribertoC - SCR 40600 - TrackDimension was added and populated for IssueReturn temp-table.
            04/03/08 HeribertoC - SCR 40600 - UOM: OnHandUM code field was added and populated.
            04/07/08 HeribertoC - SCR 40600 - UOM: ttIssueReturn.UM and ttIssueReturn.OnHandUM were not correctly populated.
            04/08/08 DanielM SCR48984 - Modified getFromJobSeq if Operation 0 was received then we need to be able to move the materials from the final operation.
            04/08/08 dparillo scr 47962 - modified validations and process STK-MTL.
            04/23/08 JeanetteP SCR 40698 - Get the SplitMfgCostElements from JobHead instead of JCSyst.
                                           Added changes for the FIFO costing project.
            04/24/08 dparillo scr 50285 - updatePartWIP - moved check for warehouseCode.
            04/29/08 scottr scr40631 - legal number changes
            04/30/08 andreaP scr 40711 - populate new field PartTran.BinType.
            05/02/08 andreaP scr 40711 - check for managed bins on some tranTypes in validateWareHouseCodeBinNum
            05/12/08 andreaP scr 40711 - add code to call consumption routine if moving out of a SMI bin (GenSMIReceipt.p)
            05/14/08 dparillo scr 47962rework - modified validations, STK-MTL and OnChangeFromBin
            06/02/08 VladimirD scr 42513 - Inventory: GetWIPGL, GetINVGl,GetCOSGL,GLDiv*,GLDep*,GLChart* fields should not be used
                                         Hardcoded logic was moved to booking rules of Inventory transaction.
            06/06/08 andreaP scr 40711 - modifications for managed bins project.
            06/11/08 VladimirD scr 51859 - Obsolete calls to GetWIPGL, GetINVGL procedures has been removed. This logic will be implemented in the
                                            booking rules of Inventory transaction
            06/13/08 - andreaP scr 52012 - changed 'vendor' to 'supplier'
            06/24/08 - andreaP scr 52231 - fixed issue with running consuption for STK-MTL.
            07/02/08 - andreaP scr 52384 - fixed cost issue in PUR-STK processing.
            07/15/08 - Orv SCR40853 - Fullfillment/allocation project, replaced existing logic with calls to new  updatealloc in appservice
            07/28/08 - debbieP SCR-50363 - updateSerialNo: ignore any serial nubmers that are deselected.
            07/29/08 - debbieP SCR-51690 - OnChangingJobSeq - added parameter for calling process type and added code to properly set SN data.
            07/31/08 - andreaP scr 53023 - redesign of CMI bins..now use CustNum rather than VendorNum
            08/01/08 - AlbertoC SCR 51070 - Modified proc getToJobSeq, assign correct value to ttIssueReturn.UM from JobMtl.IUM.
            08/05/08 - gjh - SCR#49536 - Corrected wording on error message.
            08/13/08 JeanetteP  SCR 51417 - Modified FIFO cost calculations for the UKN-STK transaction.
            08/27/08 - andreaP scr 54341 - decision was made to bring back PUR-SMI and PUR-CMI.
            09/02/08 - gjh - SCR#54812 - Populate new UOM field in PickedOrders from STK-SHP/MFG-SHP MtlQueue record.
            09/04/08 - HeribertoC - SCR 51365 - Pass ShipToCustNum field value from OrderRel to PickedOrders table.
            09/07/08  JeanetteP  SCR 51749 - Implemented the FIFO SubSequence and the FIFO Layers for non-FIFO costed parts.
            09/22/08 DebbieP SCR-40761 - additional change for MaterialQueue MFG-OPR type
            09/22/09 - DebbieP - Fix Unix comiple errors (case sensitive)
            09/23/08 - DebbieP SCR-55685 added code to properly process serial transactions for MFG-OPR type from Material Queue.
            10/01/07  JeanetteP  SCR 51749 - More changes for the FIFO Layer logic.
            10/10/08 - RicardoS - SCR 49989 - Modified getToJob to set ttIssueReturn.DimCode value from JobMtl.BaseUOM.
            10/22/08 - IgnacioL - SCR 56277 - Added filter to JobAsmbl WhereClause in getListCore.
            10/24/08 - DebbieP - SCR-56389 - changes for processing of MtlQueue WIP-WIP transactions
            10/28/08  JeanetteP  SCR 51621 - Modified the logic for determining the correct unit costs when returning
                                 LOT FIFO costed part back to stock.
            11/12/08 dparillo scr 48706 - added offsetToday() to Validation: Trandate  in validations.
            11 / 12 / 08 rmurphy scr 50268 - Issue Material Handheld defaulting to Issued Complete.
            11 / 13 / 08 AlbertoC SCR 57294 - Modified proc PerformMaterialMovement, fixed parameters passed to CheckAllocations.
            11 / 17 / 08 JeanetteP SCR 50678 - Modified getLotFIFOCosts, getPartUnitCosts, getPartTranAltTotalCosts and getPartTranTotalCost
                                   procedures to output the correct FIFODate / FIFOSeq.
            11 / 18 / 08 EduardoR SCR 57212 - Modified process - STK - SHP to set OrderNum, Line and Release to the PartTran record before update allocation.
            11 / 19 / 08 EduardoR SCR 57489 - Created process - STK - PLT to perform movement for an allocated Transfer Order(STK - PLT).
                                                  Modified isValidTranType to add STK - PLT as a valid transaction type.
                   11 / 20 / 08 AbrahamG  SCR 57419 - Modified process - STK - MTL procedure to update the PartAlloc.Reserved value.This will execute the trigger on PartAlloc and will complete the update to fhe columns to PartAlloc and PartQty tables.
                   11 / 20 / 08 AlbertoC SCR 57176 - Created process - PUR - SHP to process material queue tran type PUR - SHP.
                   11 / 21 / 08 ErickG   SCR 57427 - Added logic in method updateMtlQueue to delete MtlQueue Records created without reason to be created after a asynchronous process
                   11 / 25 / 08 ErickG   SCR 57427 - Removed logic in method updateMtlQueue to delete MtlQueue Records created without reason to be created after a asynchronous process
                   11 / 25 / 08 ErickG   SCR 58021 - Added logic in method updateMtlQueue to delete MtlQueue Records when is a Return Miscellaneous transaction an the transaction must be negative but the minus
                                                 sign intead of deleting the record it incremented
                   12 / 02 / 08 andreaP  scr 55639 - Return Mtl qty UOM not defaulting from JobMtl when Job.
                   12 / 09 / 08 YuriR    scr 57778 - Modified onChangePartNumCore - set DimCode.
                   12 / 08 / 08 EduardoR SCR 55165 - Modified getToJobSeq so if the {
                                UOM will }

                        be different, we should convert the quantity too.
            12 / 12 / 08 YuriR SCR 55509 - Modified getToJobSeq if the {
                            Part is not }

                        Track Dimention then set RequirentUOM as default
            ***/
        }
        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void buildAssembly(string pcJobNum, ref IssueReturnJobAsmblTableset ds)
        {
            ttIssueReturnJobAsmblTablesetDS = ds;


            //BuildAssemblies_Loop:
            foreach (var JobAsmbl_iterator in (this.SelectJobAsmbl(Session.CompanyID, pcJobNum, false)))
            {
                JobAsmbl = JobAsmbl_iterator;
                ttIssueReturnJobAsmbl = new Erp.Tablesets.IssueReturnJobAsmblRow();
                ttIssueReturnJobAsmblTablesetDS.IssueReturnJobAsmbl.Add(ttIssueReturnJobAsmbl);
                BufferCopy.Copy(JobAsmbl, ref ttIssueReturnJobAsmbl);
                ttIssueReturnJobAsmbl.SysRowID = Guid.NewGuid();
            }
        }

        /// <summary>
        /// Run LibAllocations.checkAllocations logic for the passed issueReturnRow
        /// </summary>
        /// <param name="issueReturnRow"></param>
        /// <param name="fromType"></param>
        /// <param name="wave"></param>
        /// <param name="waveMtlQueueSeq"></param>
        private void CheckAllocations(IssueReturnRow issueReturnRow, string fromType, bool wave, int waveMtlQueueSeq)
        {
            if (fromType.Equals("STK", StringComparison.OrdinalIgnoreCase))
            {
                if (wave)
                {
                    decimal vWaveLeftToProcess = issueReturnRow.TranQty;
                    decimal vThisTranQty = decimal.Zero;

                    foreach (MtlQueue waveMtlQueue in SelectMtlQueue(Session.CompanyID, Session.PlantID, issueReturnRow.PartNum, "Wave:", Compatibility.Convert.ToString(waveMtlQueueSeq)))
                    {
                        vThisTranQty = ((waveMtlQueue.Quantity <= vWaveLeftToProcess) ? waveMtlQueue.Quantity : vWaveLeftToProcess);
                        vWaveLeftToProcess = vWaveLeftToProcess - vThisTranQty;
                        string allocationAction = string.Empty;
                        string allocationMessage = string.Empty;

                        LibAllocations.checkAllocations(issueReturnRow.PartNum, issueReturnRow.FromWarehouseCode, issueReturnRow.FromBinNum, issueReturnRow.UM, vThisTranQty, issueReturnRow.LotNum, issueReturnRow.AttributeSetID, issueReturnRow.FromPCID, 0, 0, "", waveMtlQueue.OrderNum, waveMtlQueue.OrderLine, waveMtlQueue.OrderRelNum, waveMtlQueue.TargetJobNum, waveMtlQueue.TargetAssemblySeq, waveMtlQueue.TargetMtlSeq, waveMtlQueue.TargetTFOrdNum, waveMtlQueue.TargetTFOrdLine, out allocationAction, out allocationMessage);
                        if (allocationAction.Equals("Stop", StringComparison.OrdinalIgnoreCase))
                            throw new BLException(allocationMessage, "IssueReturn", "TranQty");
                    }
                }
                else
                {
                    string allocationAction = string.Empty;
                    string allocationMessage = string.Empty;
                    LibAllocations.checkAllocations(issueReturnRow.PartNum, issueReturnRow.FromWarehouseCode, issueReturnRow.FromBinNum, issueReturnRow.UM, issueReturnRow.TranQty, issueReturnRow.LotNum, issueReturnRow.AttributeSetID, issueReturnRow.FromPCID, 0, 0, "", issueReturnRow.OrderNum, issueReturnRow.OrderLine, issueReturnRow.OrderRel, issueReturnRow.ToJobNum, issueReturnRow.ToAssemblySeq, issueReturnRow.ToJobSeq, issueReturnRow.TFOrdNum, issueReturnRow.TFOrdLine, out allocationAction, out allocationMessage);
                    if (allocationAction.Equals("Stop", StringComparison.OrdinalIgnoreCase))
                        throw new BLException(allocationMessage, "IssueReturn", "TranQty");
                }
            }

            if (issueReturnRow.TranType.Equals("PUR-STK", StringComparison.OrdinalIgnoreCase) || issueReturnRow.TranType.Equals("PLT-STK", StringComparison.OrdinalIgnoreCase))
            {
                string allocationAction = string.Empty;
                string allocationMessage = string.Empty;
                LibAllocations.checkAllocations(issueReturnRow.PartNum, issueReturnRow.FromWarehouseCode, issueReturnRow.FromBinNum, issueReturnRow.UM, issueReturnRow.TranQty, issueReturnRow.LotNum, issueReturnRow.AttributeSetID, issueReturnRow.FromPCID, 0, 0, "", 0, 0, 0, "", 0, 0, "", 0, out allocationAction, out allocationMessage);
                if (!String.IsNullOrEmpty(allocationAction))
                    throw new BLException(allocationMessage, "IssueReturn", "TranQty");
            }
        }

        /// <summary>
        /// Run LibProcessFIFO.NegativeFIFOTest logic for the passed issueReturnRow 
        /// </summary>
        /// <param name="issueReturnRow"></param>
        /// <param name="plant"></param>
        /// <param name="plantCostID"></param>
        private void CheckNegativeFIFO(IssueReturnRow issueReturnRow, string plant, string plantCostID)
        {
            LibGetCostFIFOLayers.getPartCostFIFOLayers(plantCostID, issueReturnRow.PartNum, plant, out vEnableFIFOLayers);

            /* SCR #40698 - validate that enough FIFO qty is available if issuing qty from stock */
            /* SCR 206151 - Get the correct Cost method for the PartNum/Plant */
            string vCostMethod = getPartCostMethod(plant, issueReturnRow.PartNum);
            if ((StringExtensions.Lookup("F,O", vCostMethod) > -1) || (vEnableFIFOLayers == true))
            {
                string vLotNum = ((StringExtensions.Compare(vCostMethod, "O") == 0) ? issueReturnRow.LotNum : "");
                /* Check if the adjustment of quantity will result in negative FIFO. *
                 * The NegativeFIFOTest procedure is in lib/ProcessFIFO.i.           */
                bool vError = false;
                LibProcessFIFO.NegativeFIFOTest(issueReturnRow.PartNum, vLotNum, plantCostID, issueReturnRow.TranQty, issueReturnRow.UM, out vError);
                if (vError == true)
                    throw new BLException(Strings.ThisTransWillResultIntoANegatFIFOOnhandQuantTrans, "ttIssueReturn", "TranQty");
            }
        }

        ///<summary>
        ///  Purpose: Based on the TranType and TranQty, the correct value for the
        ///           SerialNo.SNStatus field is determined.
        ///  Parameters:  Input: TranType and TranQty.
        ///               Output: snselectstatus.
        ///  Notes:       (copied from SerialNumberSearch.p\getSelectStatus)
        ///</summary>
        private void checkSNStatus(string TranType, decimal TranQty, out string snselectstatus)
        {
            /* Get the Serial number status for the select. */
            /*Based on Transaction type determine the status of the serialno records 
            //which will qualify for use by the transaction.
            //Example: STK-MTL transaction would only look at serial no with a status of "Inventory"
            //This takes a little thought about how/when the serialno status is updated and
            //where this specific program is called from.
            //For example you might think a STK-INS transaction would look for status of 
            //"Inventory".  You would be wrong. The NonConformance program is the 
            //one that changes the status to "Inspection". This program is called from the
            //plant floor material que which is just a move type of transaction. Therefore 
            //it would be looking for status of "Inspection".  This does twist the mind a 
            //little, it's just that you have to really understand the data flow.

            //Starting in 9.0 this validation is only used for the Material Move Queue transactions, the other trans
            //actions use thier own validations based on the specific transaction type */
            snselectstatus = string.Empty;
            string V_WIP = string.Empty;
            string V_Inventory = string.Empty;
            string V_Inspection = string.Empty;
            string V_Misc = string.Empty;
            string V_DMR = string.Empty;
            V_WIP = "ADJ-MTL,ADJ-WIP,ASM-STK,MTL-STK,PUR-MTL,MFG-CUS,MFG-OPR,MFG-STK,MFG-SHP,MFG-WIP,PUR-SUB,WIP-WIP,SPLIT,MTL-MTL,INS-ASM,INS-MTL,INS-SUB,DMR-ASM,DMR-SUB,DMR-MTL";
            V_Inventory = "INS-STK,INS-SHP,PUR-STK,PUR-SMI,PUR-CMI,STK-ASM,STK-MTL,STK-SHP,STK-STK,STK-UKN,RAU-STK,RMA-STK,RMN-STK,DMR-STK";
            V_Inspection = "ASM-INS,SUB-INS,MTL-INS,PUR-INS,STK-INS,RMA-INS";
            V_Misc = "UKN-STK";
            V_DMR = "INS-DMR,DMR-REJ";
            /*  FOR THE FOLLOWING TRANACTION TYPES THE SAME TRAN TYPE IS USED FOR NEGATIVE TRANSACTIONS,
                THEREFORE WE NEED SPECIAL CODE TO HANDLE THEM */
            if (TranQty < 0)
            {
                if (StringExtensions.Lookup("MFG-CUS,MFG-SHP,MFG-STK,SVG-STK", TranType) > -1)
                {
                    snselectstatus = "INVENTORY";
                }
            }
            if (String.IsNullOrEmpty(snselectstatus))
            {
                if (StringExtensions.Lookup(V_WIP, TranType) > -1)
                {
                    snselectstatus = "WIP";
                }

                if (StringExtensions.Lookup(V_Inventory, TranType) > -1)
                {
                    snselectstatus = "INVENTORY";
                }

                if (StringExtensions.Lookup(V_Inspection, TranType) > -1)
                {
                    snselectstatus = "INSPECTION";
                }

                if (StringExtensions.Lookup(V_Misc, TranType) > -1)
                {
                    snselectstatus = "MISC-ISSUE";
                }

                if (StringExtensions.Lookup(V_DMR, TranType) > -1)
                {
                    snselectstatus = "DMR";
                }

                if (String.IsNullOrEmpty(snselectstatus))
                {
                    snselectstatus = "INVENTORY";
                }
            }
        }

        ///<summary>
        ///  Purpose:  Removes selected serial numbers if IssueReturn data changes and the
        ///  numbers are no longer valid for selection based on the new IssueReturn data.
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        /* Delete any selected serial numbers if EnableSN has been set to false */
        /* if EnableSN = yes and serials have been selected, validate and remove any
        that are not valid based on the new data */
        private void checkStatusTracking()
        {
            string snError = string.Empty;
            if (ttIssueReturn.EnableSN == false)
            {
                foreach (var ttSelectedSerialNumbers_iterator in (from ttSelectedSerialNumbers_Row in CurrentFullTableset.SelectedSerialNumbers
                                                                  where ttSelectedSerialNumbers_Row.Company.KeyEquals(ttIssueReturn.Company)
                                                                  && !String.IsNullOrEmpty(ttSelectedSerialNumbers_Row.RowMod)
                                                                  select ttSelectedSerialNumbers_Row).ToList())
                {
                    ttSelectedSerialNumbers = ttSelectedSerialNumbers_iterator;
                    ttSelectedSerialNumbers.RowMod = IceRow.ROWSTATE_DELETED;
                }

                foreach (var ttSNFormat_iterator in (from ttSNFormat_Row in CurrentFullTableset.SNFormat
                                                     where ttSNFormat_Row.PartNum.KeyEquals(ttIssueReturn.PartNum)
                                                     && !String.IsNullOrEmpty(ttSNFormat_Row.RowMod)
                                                     select ttSNFormat_Row))
                {
                    ttSNFormat = ttSNFormat_iterator;
                    ttSNFormat.RowMod = IceRow.ROWSTATE_DELETED;
                }
            }
            else
            {
                foreach (var ttSelectedSerialNumbers_iterator in (from ttSelectedSerialNumbers_Row in CurrentFullTableset.SelectedSerialNumbers
                                                                  where ttSelectedSerialNumbers_Row.Company.KeyEquals(ttIssueReturn.Company)
                                                                  && !ttSelectedSerialNumbers_Row.Deselected
                                                                  && !String.IsNullOrEmpty(ttSelectedSerialNumbers_Row.RowMod)
                                                                  select ttSelectedSerialNumbers_Row))
                {
                    ttSelectedSerialNumbers = ttSelectedSerialNumbers_iterator;

                    SerialNo SerialNo = this.FindFirstSerialNo(Session.CompanyID, ttIssueReturn.PartNum, ttSelectedSerialNumbers.SerialNumber);
                    snError = "";
                    this.validateSerialNumber(ttSelectedSerialNumbers.SerialNumber, SerialNo, out snError);

                    /* deselect any previous that do not meet the validation requirements */
                    if (!String.IsNullOrEmpty(snError))
                    {
                        ttSelectedSerialNumbers.SourceRowID = Guid.Empty;
                        ttSelectedSerialNumbers.Deselected = true;
                        ttSelectedSerialNumbers.RowMod = IceRow.ROWSTATE_UPDATED;
                    }
                }
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void checkWarehouseBin(string ip_WarehouseCode, string ip_plant, out bool okWarehse)
        {
            okWarehse = false;

            PlantShared = this.FindFirstPlantShared(Session.CompanyID, ip_WarehouseCode, ip_plant);
            okWarehse = PlantShared != null;
        }

        ///<summary>
        ///  Purpose: Create a PartTran record.
        ///  Parameters:  none
        ///  Notes: The PartTran in this procedure are incomplete. The transaction specific
        ///         details (TranType, GL Accounts, Quantity, etc) are updated in other procedures specific
        ///         for the transaction being processed.
        ///</summary>
        private void createPartTran(IssueReturnRow ttIssueReturn)
        {
            DateTime? CurDate = null;
            int CurTime = 0;
            bool PartTranError = false;
            bool vWave = false;
            CurDate = CompanyTime.Today();
            CurTime = Erp.Internal.Lib.DateExtensions.SecondsSinceMidnight(CompanyTime.Now());

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part != null)
            {
                this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, Part.IUM, out ConvQty, false);
                this.LibAppService.RoundToUOMDec(Part.IUM, ref ConvQty);
                //CREATE_PARTTRAN:

                if (ConvQty == 0)
                {
                    throw new BLException(Strings.InvalidIssueQty);
                }
            }

            this.LibCreatePartTran.Create_PartTran(ref PartTran, Convert.ToDateTime(CurDate), CurTime, out PartTranError);
            if (PartTranError == true)
            {
                throw new BLException(Strings.ErrorCreatingPartTran, "PartTran");
            }

            MtlQueue = this.FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
            if (MtlQueue != null)
            {
                vWave = MtlQueue.WaveNum != 0;
                // Create the FSA Data for the PartTran "FROM" Movement
                using (var libFSA = new FSAExtDataUtil(Db))
                {
                    string errormsg;
                    if (libFSA.CheckFSALicense(out errormsg) && MtlQueue.EpicorFSA)
                    {
                        writeFSAExtFields(MtlQueue.SysRowID, MtlQueue.GetTableName());
                    }
                }
            }

            PartTran.EntryPerson = Session.UserID;
            if (ttIssueReturn.TranDate == null)
            {
                PartTran.TranDate = null;
            }
            else
            {
                PartTran.TranDate = ttIssueReturn.TranDate;
            }

            PartTran.PartDescription = ttIssueReturn.PartPartDescription;
            PartTran.PartNum = ttIssueReturn.PartNum;
            using (var inventoryTracking = new InventoryTracking(Db))
            {
                PartTran.AttributeSetID = ttIssueReturn.AttributeSetID;
                PartTran.AttributeSetDescription = ttIssueReturn.AttributeSetDescription;
                PartTran.AttributeSetShortDescription = ttIssueReturn.AttributeSetShortDescription;
                if (Part != null && Part.TrackInventoryByRevision)
                {
                    inventoryTracking.ValidateInventoryByRevision(Part.TrackInventoryByRevision, ttIssueReturn);
                    ExceptionManager.AssertNoBLExceptions();

                    PartTran.RevisionNum = ttIssueReturn.RevisionNum;
                }
                if (ttIssueReturn.AttributeSetID != 0 && (ttIssueReturn.DispNumberOfPieces == 0 && ttIssueReturn.TranQty != 0))
                {
                    inventoryTracking.UpdateNumberOfPieces(ref ttIssueReturn);
                }
            }

            PartTran.NumberOfPieces = (int)Math.Round(ttIssueReturn.DispNumberOfPieces, MidpointRounding.AwayFromZero);
            PartTran.TranQty = ConvQty;
            PartTran.UM = Part.IUM;
            PartTran.ActTranQty = ttIssueReturn.TranQty;
            PartTran.ActTransUOM = ttIssueReturn.UM;
            PartTran.DUM = ttIssueReturn.UM;
            PartTran.LotNum = ttIssueReturn.LotNum;
            PartTran.TranReference = ((vWave) ? "WAVE" : ttIssueReturn.TranReference);
            PartTran.InvAdjReason = ttIssueReturn.ReasonCode;
            PartTran.InvAdjSrc = "O"; /* other */
            PartTran.LegalNumber = TempLegalNumber;
            PartTran.TranDocTypeID = ttIssueReturn.TranDocTypeID;
            PartTran.OrderNum = ttIssueReturn.OrderNum;
            PartTran.OrderLine = ttIssueReturn.OrderLine;
            PartTran.OrderRelNum = ttIssueReturn.OrderRel;
            PartTran.JobNum = ttIssueReturn.ToJobNum;
            PartTran.AssemblySeq = ttIssueReturn.ToAssemblySeq;
            PartTran.JobSeq = ttIssueReturn.ToJobSeq;
            PartTran.EmpID = Session.EmployeeID;
            PartTran.TranReference = PartTran.TranReference + ((String.IsNullOrEmpty(ttIssueReturn.TFOrdNum)) ? "" : "|TFO|" + ttIssueReturn.TFOrdNum + "|" + Compatibility.Convert.ToString(ttIssueReturn.TFOrdLine));
            if (Part.TrackDimension == true)
            {
                PartTran.InvtyUOM = PartTran.ActTransUOM;
            }
            else
            {
                PartTran.InvtyUOM = Part.IUM;
            }

            this.LibDefPartTran._DefPartTran(ref PartTran, ref Part, ref PartPlant, Session.CompanyID, ttIssueReturn.PartNum, Session.PlantID, PartTranError);

            /*Creating PartTranSNTran records*/
            IMPartTranSNtranLink.createSNTranPartTranLink(PartTran, ref ttSNtranRows);
        }

        private void writeFSAExtFields(Guid sysrowid, string tableName)
        {
            using (var libFSA = new FSAExtDataUtil(Db))
            {
                PartTran.EpicorFSA = true;
                var FSAExtData = libFSA.ReadFSAExtData(Session.CompanyID, tableName, sysrowid);
                if (FSAExtData != null)
                {
                    var FSAExtData_PartTran = new FSAExtData();
                    BufferCopy.CopyExceptFor(FSAExtData, FSAExtData_PartTran, FSAExtData.ColumnNames.SysRowID, FSAExtData.ColumnNames.SysRevID);
                    FSAExtData_PartTran.ForeignSysRowID = PartTran.SysRowID;
                    FSAExtData_PartTran.TableName = PartTran.GetTableName();
                    Db.FSAExtData.Insert(FSAExtData_PartTran);
                }
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void createSNTran(string pcTranType, DateTime? pdtTranDate, SerialNo tmpSerialNo)
        {
            Guid SNTranRowId = Guid.Empty;
            LibGetNewSNtran._GetNewSNtran(tmpSerialNo, pcTranType, pdtTranDate, out SNTranRowId);

            Internal.IM.PartTranSNtranLink.SNtran2 ttSNtran = null;
            IMPartTranSNtranLink.createttSNtranRecord(SNTranRowId, ref ttSNtranRows, ref ttSNtran);
        }

        ///<summary>
        ///  Purpose:     This procedure determines the sensitivity of various fields
        ///               in ttIssueReturn table.
        ///  Parameters:  none
        ///  notes:
        ///</summary>
        private void determineSensitivity(IssueReturnRow ttIssueReturn)
        {
            string cFromType = string.Empty;
            string cToType = string.Empty;
            bool lSensitive = false;
            bool lFromJobSensitive = false;
            bool lToJobSensitive = false;
            string cFromJobSeqTranTypes = "MTL-MTL,WIP-WIP,MTL-STK";
            string cToJobSeqTranTypes = "ADJ-MTL,MOV-WIP,ADJ-WIP,STK-MTL";
            cFromType = this.getFromType(ttIssueReturn.TranType);
            cToType = this.getToType(ttIssueReturn.TranType);

            /* Trandate sensitivity */
            /* ReasonCode sensitivity */
            if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-UKN") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "UKN-STK") == 0)
            {
                this.setSensitive("ttIssueReturn.ReasonCode", ttIssueReturn.SysRowID, true);
            }
            else
            {
                this.setSensitive("ttIssueReturn.ReasonCode", ttIssueReturn.SysRowID, false);
            }
            /* PartNum-Sensitivity.*/
            lSensitive = true;
            if (StringExtensions.Compare(cFromType, "STK") != 0 && StringExtensions.Compare(cToType, "STK") != 0)
            {
                lSensitive = false;
            }

            if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
            {
                if (ttIssueReturn.FromJobSeq == 0)
                {
                    lSensitive = false;
                }
                else
                {
                    lSensitive = true;
                }
            }
            if (StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-WIP") == 0)
            {
                if (ttIssueReturn.ToJobSeq == 0)
                {
                    lSensitive = false;
                }
                else
                {
                    lSensitive = true;
                }
            }
            if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-ASM") == 0)
            {
                if (ttIssueReturn.ToAssemblySeq == 0)
                {
                    lSensitive = false;
                }
            }
            /* ISSUE TO JOB MATERIAL */
            if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-MTL") == 0)
            {
                if (ttIssueReturn.ToJobSeq == 0)
                {
                    lSensitive = false;
                }
            }
            if (StringExtensions.Compare(ttIssueReturn.TranType, "ASM-STK") == 0)
            {
                if (ttIssueReturn.FromAssemblySeq == 0)
                {
                    lSensitive = false;
                }
            }
            /* RETURN JOB MATERIAL TO STOCK */
            if (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") == 0)
            {
                if (ttIssueReturn.FromJobSeq == 0)
                {
                    lSensitive = false;
                }
            }
            this.setSensitive("ttIssueReturn.PartNum", ttIssueReturn.SysRowID, lSensitive);
            /* LotNum Sensitivity */
            /* DISABLE LOT NUMBER FOR THE FOLLOWING TRANSACTION TYPES */
            /* ADJUST WIP */
            if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0 /* MOVE WIP */
            || StringExtensions.Compare(ttIssueReturn.TranType, "MFG-OPR") == 0  /* MOVE WIP TO NEXT OPERATION */
            || StringExtensions.Compare(ttIssueReturn.TranType, "ASM-INS") == 0 /* MOVE FROM WIP TO INSPECTION */
            || StringExtensions.Compare(ttIssueReturn.TranType, "INS-ASM") == 0 /* MOVE FROM INSPECTION TO WIP */
            || StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-WIP") == 0)
            {
                this.setSensitive("ttIssueReturn.LotNum", ttIssueReturn.SysRowID, false);
            }
            else
            {
                Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
                if (Part != null)
                {
                    this.setSensitive("ttIssueReturn.LotNum", ttIssueReturn.SysRowID, Part.TrackLots);
                }
                else
                {
                    this.setSensitive("ttIssueReturn.LotNum", ttIssueReturn.SysRowID, false);
                }
            }
            /*FromJobNum sensitivity */
            lFromJobSensitive = false;
            if (this.getJobDirection(ttIssueReturn.TranType).KeyEquals("From"))
            {
                lFromJobSensitive = true;
            }
            /* if THE ASM # WAS SUPPLIED BY A MTLQUEUE REQUEST then IT CAN'T BE CHANGED and WILL BE DISABLED */
            if (ttIssueReturn.MtlQueueRowId != Guid.Empty && !String.IsNullOrEmpty(ttIssueReturn.FromJobNum))
            {
                lFromJobSensitive = false;
            }

            this.setSensitive("ttIssueReturn.FromJobNum", ttIssueReturn.SysRowID, lFromJobSensitive);
            /* FromAssemblySeq Sensitivity */
            lSensitive = false;
            lSensitive = ((lFromJobSensitive == false) ? false : true);
            if (lSensitive == true)
            {
                if (StringExtensions.Compare(ttIssueReturn.TranType, "ASM-STK") != 0)
                {


                    lSensitive = (this.ExistsJobAsmbl(Session.CompanyID, ttIssueReturn.FromJobNum, 0));
                }
            }
            /* if THE ASM # WAS SUPPLIED BY A MTLQUEUE REQUEST then IT CAN'T BE CHANGED and WILL BE DISABLED */
            if (ttIssueReturn.MtlQueueRowId != Guid.Empty && !String.IsNullOrEmpty(ttIssueReturn.FromJobNum))
            {
                lSensitive = false;
            }

            this.setSensitive("ttIssueReturn.FromAssemblySeq", ttIssueReturn.SysRowID, lSensitive);
            /* FromJobSeq Sensitivity */
            /* DOES IT PERTAIN TO THIS TRANSACTION TYPE ? */
            lSensitive = false;
            if (StringExtensions.Lookup(cFromJobSeqTranTypes, ttIssueReturn.TranType) > -1)
            {
                lSensitive = true;
            }
            /* if THE SEQ # WAS SUPPLIED BY A MTLQUEUE REQUEST then IT CAN'T BE CHANGED and WILL BE DISABLED */
            if (ttIssueReturn.MtlQueueRowId != Guid.Empty && ttIssueReturn.FromJobSeq > 0)
            {
                lSensitive = false;
            }

            this.setSensitive("ttIssueReturn.FromJobSeq", ttIssueReturn.SysRowID, lSensitive);
            /* FromWarehouseCode Sensitivity */
            /* if MFGSYS = "VS":U */
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.MultipleWarehouse))
            {
                this.setSensitive("ttIssueReturn.FromWarehouseCode", ttIssueReturn.SysRowID, true);
            }
            else
            {
                switch (cFromType.ToUpperInvariant())
                {
                    case "MTL":
                        {
                            if (!ttIssueReturn.FromJobPlant.KeyEquals(Session.PlantID) || (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") == 0 && Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) == false))
                            {
                                this.setSensitive("ttIssueReturn.FromWarehouseCode", ttIssueReturn.SysRowID, false);
                                this.setSensitive("ttIssueReturn.FromBinNum", ttIssueReturn.SysRowID, false);
                            }
                            if (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-INS") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "INS-MTL") == 0)
                            {
                                this.setSensitive("ttIssueReturn.FromWarehouseCode", ttIssueReturn.SysRowID, false);
                                this.setSensitive("ttIssueReturn.FromBinNum", ttIssueReturn.SysRowID, false);
                            }
                        }
                        break;/* "FROM" MTL */
                    case "ASM":
                        {
                            if (!ttIssueReturn.FromJobPlant.KeyEquals(Session.PlantID) || (StringExtensions.Compare(ttIssueReturn.TranType, "ASM-STK") == 0 && Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) == false))
                            {
                                this.setSensitive("ttIssueReturn.FromWarehouseCode", ttIssueReturn.SysRowID, false);
                                this.setSensitive("ttIssueReturn.FromBinNum", ttIssueReturn.SysRowID, false);
                            }
                        }
                        break;/* "FROM" ASM */
                    case "UKN":
                        {
                            this.setSensitive("ttIssueReturn.FromWarehouseCode", ttIssueReturn.SysRowID, false);
                            this.setSensitive("ttIssueReturn.FromBinNum", ttIssueReturn.SysRowID, false);
                        }
                        break;
                    case "ADJ":
                        {
                            this.setSensitive("ttIssueReturn.FromWarehouseCode", ttIssueReturn.SysRowID, false);
                            this.setSensitive("ttIssueReturn.FromBinNum", ttIssueReturn.SysRowID, false);
                        }
                        break;
                    default:
                        this.setSensitive("ttIssueReturn.FromWarehouseCode", ttIssueReturn.SysRowID, true);
                        this.setSensitive("ttIssueReturn.FromBinNum", ttIssueReturn.SysRowID, false);
                        break;
                }
            }/* else if MFGSYS = "VS":U */
            /* FromBinNum sensitivity */
            lSensitive = false;
            if ((StringExtensions.Compare(cFromType, "STK") == 0
            || StringExtensions.Compare(cFromType, "OPR") == 0
            || StringExtensions.Compare(cFromType, "MTL") == 0
            || StringExtensions.Compare(cFromType, "ASM") == 0))
            {
                lSensitive = true;
            }

            if (StringExtensions.Compare(cFromType, "MTL") == 0 && !ttIssueReturn.FromJobPlant.KeyEquals(Session.PlantID) || StringExtensions.Compare(ttIssueReturn.TranType, "MTL-INS") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "INS-MTL") == 0)
            {
                lSensitive = false;
            }

            this.setSensitive("ttIssueReturn.FromBinNum", ttIssueReturn.SysRowID, lSensitive);
            /* ToJobNum-Sensitivity */
            lToJobSensitive = false;
            if (this.getJobDirection(ttIssueReturn.TranType).KeyEquals("To"))
            {
                lToJobSensitive = true;
            }
            /* if THE JOB # WAS SUPPLIED BY A MTLQUEUE REQUEST then IT CAN'T BE CHANGED and WILL BE DISABLED */
            if (ttIssueReturn.MtlQueueRowId != Guid.Empty && !String.IsNullOrEmpty(ttIssueReturn.ToJobNum))
            {
                lToJobSensitive = false;
            }

            this.setSensitive("ttIssueReturn.ToJobNum", ttIssueReturn.SysRowID, lToJobSensitive);
            /* ToAssemblySeq-Sensitivity */
            lSensitive = ((lToJobSensitive == false) ? false : true);
            if (lSensitive == true)
            {
                if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-ASM") != 0)
                {
                    lSensitive = (this.ExistsJobAsmbl2(Session.CompanyID, ttIssueReturn.ToJobNum, 0));
                }
            }
            /* if THE ASM # WAS SUPPLIED BY A MTLQUEUE REQUEST then IT CAN'T BE CHANGED and WILL BE DISABLED */
            if (ttIssueReturn.MtlQueueRowId != Guid.Empty && !String.IsNullOrEmpty(ttIssueReturn.ToJobNum))
            {
                lSensitive = false;
            }

            this.setSensitive("ttIssueReturn.ToAssemblySeq", ttIssueReturn.SysRowID, lSensitive);

            /* ToJobSeq-Sensitivity */
            /* DOES IT PERTAIN TO THIS TRANSACTION TYPE ? */
            lSensitive = false;
            if (StringExtensions.Lookup(cToJobSeqTranTypes, ttIssueReturn.TranType) > -1)
            {
                lSensitive = true;
            }
            /* if THE SEQ # WAS SUPPLIED BY A MTLQUEUE REQUEST then IT CAN'T BE CHANGED and WILL BE DISABLED */
            if (ttIssueReturn.MtlQueueRowId != Guid.Empty && ttIssueReturn.ToJobSeq > 0)
            {
                lSensitive = false;
            }

            this.setSensitive("ttIssueReturn.ToJobSeq", ttIssueReturn.SysRowID, lSensitive);
            /* ToWarehoseCode-Sensitivity */
            /* if MFGSYS = "VS":U */
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.MultipleWarehouse))
            {
                this.setSensitive("ttIssueReturn.ToWarehouseCode", ttIssueReturn.SysRowID, true);
            }
            else
            {
                if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && StringExtensions.Compare(cToType, "STK") != 0)
                {
                    this.setSensitive("ttIssueReturn.ToWarehouseCode", ttIssueReturn.SysRowID, Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement));
                }
                else
                {
                    switch (cToType.ToUpperInvariant())
                    {
                        case "MTL":
                            {
                                if (!ttIssueReturn.ToJobPlant.KeyEquals(Session.PlantID))
                                {
                                    this.setSensitive("ttIssueReturn.ToWarehouseCode", ttIssueReturn.SysRowID, false);
                                    this.setSensitive("ttIssueReturn.ToBinNum", ttIssueReturn.SysRowID, false);
                                }
                                else
                                {
                                    this.setSensitive("ttIssueReturn.ToWarehouseCode", ttIssueReturn.SysRowID, true);
                                    this.setSensitive("ttIssueReturn.ToBinNum", ttIssueReturn.SysRowID, true);
                                }
                            }
                            break;/* "TO" MTL */
                        case "ASM":
                            {
                                if (!ttIssueReturn.ToJobPlant.KeyEquals(Session.PlantID))
                                {
                                    this.setSensitive("ttIssueReturn.ToWarehouseCode", ttIssueReturn.SysRowID, false);
                                    this.setSensitive("ttIssueReturn.ToBinNum", ttIssueReturn.SysRowID, false);
                                }
                                else
                                {
                                    this.setSensitive("ttIssueReturn.ToWarehouseCode", ttIssueReturn.SysRowID, true);
                                    this.setSensitive("ttIssueReturn.ToBinNum", ttIssueReturn.SysRowID, true);
                                }
                            }
                            break;/* "TO" ASM */
                        case "UKN":
                            {
                                this.setSensitive("ttIssueReturn.ToWarehouseCode", ttIssueReturn.SysRowID, false);
                                this.setSensitive("ttIssueReturn.ToBinNum", ttIssueReturn.SysRowID, false);
                            }
                            break;
                        default:
                            this.setSensitive("ttIssueReturn.ToWarehouseCode", ttIssueReturn.SysRowID, true);
                            this.setSensitive("ttIssueReturn.ToBinNum", ttIssueReturn.SysRowID, true);
                            break;
                    }
                }
            }/* else if MFGSYS = "VS":U */
            /* ToBin-Sensitivity */
            lSensitive = false;        /* if the cToType is MTL then the Amm Module doesn't need to be installed */
            if (StringExtensions.Compare(cToType, "MTL") == 0 && !ttIssueReturn.ToJobPlant.KeyEquals(Session.PlantID))
            {
                lSensitive = false;
            }
            else if (StringExtensions.Compare(cToType, "STK") != 0)
            {
                lSensitive = (lSensitive && Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement));
            }

            this.setSensitive("ttIssueReturn.ToBinNum", ttIssueReturn.SysRowID, lSensitive);
        }

        ///<summary>
        ///  Purpose: Sets the EnableToFields and EnableFromFields accordingly depending on
        ///           the licensing conditions and transaction type
        ///  Parameters:  pcTranType - Transaction Type
        ///               plToField - ttIssueReturn.EnableToFields
        ///               plFromField - ttIssueReturn.EnableFromFields
        ///  Notes:
        ///</summary>
        private void disableFromToFields(string pcTranType, out bool plToField, out bool plFromField)
        {
            plToField = false;
            plFromField = false;
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && (!this.getToType(pcTranType).KeyEquals("STK")))
            {
                plToField = false;
            }
            else
            {
                plToField = true;
            }
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && (!this.getFromType(pcTranType).KeyEquals("STK")))
            {
                plFromField = false;
            }
            else
            {
                plFromField = true;
            }
        }

        ///<summary>
        ///  Purpose: Indicate whether the serial number button will be enable when transaction data
        ///  is changed by the user in the UI, this would be fields such as the
        ///  Part Number, from job or warehouse; or to job or warehouse, Assembly number, job Mtl number.
        ///  And sets some ttIssueReturn fields required for serial number processing.
        ///  Parameters:  Input: processID.
        ///  Notes:
        ///</summary>
        private void enableSNButton(IssueReturnRow issueReturnRow, string processID)
        {
            issueReturnRow.SerialControlPlant = "";
            issueReturnRow.SerialControlPlant2 = "";
            issueReturnRow.SerialControlPlantIsFromPlt = false;
            issueReturnRow.ProcessID = processID;
            issueReturnRow.EnableSN = true; /* Defaulted to yes */
            /* verify if the part is serial tracked*/
            /* the setting of the EnableSN field will This external field would be used to
            indicate for the BL whether serial numbers should be validated/updated and
            indicated for the various UIs whether the serial number button will be enabled.
            Therefore it is critical that the setting of this field to yes or no include all of
            the rules that are applicable as to whether this transaction does nor does not
            require serial number entries. If EnableSN = yes then the SerialControlPlant field will
            indicate the plant whose PlantConfCtrl serial tracking setting are used, and the
            SeiralControlPlantIsFromPlt will indicate whether the SerialControlPlant is the from
            plant or the to plant for the IssueReturn transaction. This is necessary to know
            how to set the paramters for the Select Serial Numbers form */
            if (string.IsNullOrEmpty(issueReturnRow.PartNum) || !partSerialTracking(issueReturnRow.PartNum))
            {
                issueReturnRow.EnableSN = false;
                return;
            }

            string jobHeadPlant = string.Empty;
            string whseOwnerPlant = string.Empty;
            int fromPlantTracking = 0;
            int toPlantTracking = 0;

            switch (processID.ToUpperInvariant())
            {
                case "ISSUEMATERIAL":
                case "HHISSUEMATERIAL":
                    {
                        jobHeadPlant = this.getJobHeadPlant(issueReturnRow.ToJobNum);
                        /* get the owner plant of the warehouse*/
                        whseOwnerPlant = this.getOwnerPlant(issueReturnRow.FromWarehouseCode);
                        fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(whseOwnerPlant);
                        toPlantTracking = this.LibSerialCommon.isSerTraPlantType(jobHeadPlant);
                        /* at least one plant must be full serial tracked or the transaction involves an outbound container PCID in an OB tracking plant */
                        if ((fromPlantTracking == 2) ||
                           (fromPlantTracking == 3 && (String.IsNullOrEmpty(issueReturnRow.FromPCID) ? false : GetPkgControlHeaderOutboundContainer(Session.CompanyID, issueReturnRow.FromPCID))))

                        {
                            issueReturnRow.EnableSN = true;
                            issueReturnRow.SerialControlPlant = whseOwnerPlant;
                            issueReturnRow.SerialControlPlant2 = jobHeadPlant;
                            issueReturnRow.SerialControlPlantIsFromPlt = true;
                        }
                        else
                        {
                            if (toPlantTracking == 2)
                            {
                                issueReturnRow.EnableSN = true;
                                issueReturnRow.SerialControlPlant = jobHeadPlant;
                            }
                            else
                            {
                                issueReturnRow.EnableSN = false;
                            }
                        }
                    }
                    break;
                case "UNPICK":
                case "UNPICKPCID":
                    {
                        whseOwnerPlant = this.getOwnerPlant(issueReturnRow.FromWarehouseCode);
                        fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(whseOwnerPlant);
                        /* the plant must be full serial tracked*/
                        if (fromPlantTracking == 2)
                        {
                            issueReturnRow.EnableSN = true;
                            issueReturnRow.SerialControlPlant = whseOwnerPlant;
                            issueReturnRow.SerialControlPlantIsFromPlt = true;
                        }
                        else
                        {
                            issueReturnRow.EnableSN = false;
                        }
                    }
                    break;
                case "ISSUEASSEMBLY":
                case "HHISSUEASSEMBLY":
                    {
                        jobHeadPlant = this.getJobHeadPlant(issueReturnRow.ToJobNum);
                        /* get the owner plant of the warehouse*/
                        whseOwnerPlant = this.getOwnerPlant(issueReturnRow.FromWarehouseCode);
                        fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(whseOwnerPlant);
                        toPlantTracking = this.LibSerialCommon.isSerTraPlantType(jobHeadPlant);
                        /* at least one plant must be full serial tracked*/
                        if ((fromPlantTracking == 2) ||
                            (fromPlantTracking == 3 && (String.IsNullOrEmpty(issueReturnRow.FromPCID) ? false : GetPkgControlHeaderOutboundContainer(Session.CompanyID, issueReturnRow.FromPCID))))

                        {
                            issueReturnRow.EnableSN = true;
                            issueReturnRow.SerialControlPlant = whseOwnerPlant;
                            issueReturnRow.SerialControlPlant2 = jobHeadPlant;
                            issueReturnRow.SerialControlPlantIsFromPlt = true;
                        }
                        else
                        {
                            if (toPlantTracking == 2)
                            {
                                issueReturnRow.EnableSN = true;
                                issueReturnRow.SerialControlPlant = jobHeadPlant;
                            }
                            else
                            {
                                issueReturnRow.EnableSN = false;
                            }
                        }
                    }
                    break;
                case "ISSUEMISCELLANEOUSMATERIAL":
                    {
                        whseOwnerPlant = this.getOwnerPlant(issueReturnRow.FromWarehouseCode);
                        fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(whseOwnerPlant);
                        /* the plant must be full serial tracked*/
                        if (fromPlantTracking == 2)
                        {
                            issueReturnRow.EnableSN = true;
                            issueReturnRow.SerialControlPlant = whseOwnerPlant;
                            issueReturnRow.SerialControlPlantIsFromPlt = true;
                        }
                        else
                        {
                            issueReturnRow.EnableSN = false;
                        }
                    }
                    break;
                case "RETURNMATERIAL":
                case "HHRETURNMATERIAL":
                    {
                        jobHeadPlant = this.getJobHeadPlant(issueReturnRow.FromJobNum);
                        /* get the owner plant of the warehouse*/
                        whseOwnerPlant = this.getOwnerPlant(issueReturnRow.ToWarehouseCode);
                        toPlantTracking = this.LibSerialCommon.isSerTraPlantType(whseOwnerPlant);
                        fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(jobHeadPlant);
                        /* at least one plant must be full serial tracked*/
                        if (fromPlantTracking == 2)
                        {
                            issueReturnRow.EnableSN = true;
                            issueReturnRow.SerialControlPlant = jobHeadPlant;
                            issueReturnRow.SerialControlPlant2 = whseOwnerPlant;
                            issueReturnRow.SerialControlPlantIsFromPlt = true;
                        }
                        else
                        {
                            if (toPlantTracking == 2)
                            {
                                issueReturnRow.EnableSN = true;
                                issueReturnRow.SerialControlPlant = whseOwnerPlant;
                            }
                            else
                            {
                                issueReturnRow.EnableSN = false;
                            }
                        }
                    }
                    break;
                case "RETURNASSEMBLY":
                case "HHRETURNASSEMBLY":
                    {
                        jobHeadPlant = this.getJobHeadPlant(issueReturnRow.FromJobNum);
                        /* get the owner plant of the warehouse*/
                        whseOwnerPlant = this.getOwnerPlant(issueReturnRow.ToWarehouseCode);
                        toPlantTracking = this.LibSerialCommon.isSerTraPlantType(whseOwnerPlant);
                        fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(jobHeadPlant);
                        /* at least one plant must be full serial tracked*/
                        if (fromPlantTracking == 2)
                        {
                            issueReturnRow.EnableSN = true;
                            issueReturnRow.SerialControlPlant = jobHeadPlant;
                            issueReturnRow.SerialControlPlant2 = whseOwnerPlant;
                            issueReturnRow.SerialControlPlantIsFromPlt = true;
                        }
                        else
                        {
                            if (toPlantTracking == 2)
                            {
                                issueReturnRow.EnableSN = true;
                                issueReturnRow.SerialControlPlant = whseOwnerPlant;
                            }
                            else
                            {
                                issueReturnRow.EnableSN = false;
                            }
                        }
                    }
                    break;
                case "RETURNMISCELLANEOUSMATERIAL":
                    {
                        whseOwnerPlant = this.getOwnerPlant(issueReturnRow.ToWarehouseCode);
                        toPlantTracking = this.LibSerialCommon.isSerTraPlantType(whseOwnerPlant);
                        /* the plant must be full serial tracked*/
                        if (toPlantTracking == 2)
                        {
                            issueReturnRow.EnableSN = true;
                            issueReturnRow.SerialControlPlant = whseOwnerPlant;
                        }
                        else
                        {
                            issueReturnRow.EnableSN = false;
                        }
                    }
                    break;
                case "ADJUSTWIP":
                    {
                        jobHeadPlant = this.getJobHeadPlant(issueReturnRow.ToJobNum);
                        fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(jobHeadPlant);
                        /* the plant must be full serial tracked*/
                        if (fromPlantTracking == 2 && this.snRequired(issueReturnRow.ToJobNum, issueReturnRow.ToAssemblySeq, issueReturnRow.ToJobSeq, false))
                        {
                            issueReturnRow.EnableSN = true;
                            issueReturnRow.SerialControlPlant = jobHeadPlant;
                        }
                        else
                        {
                            issueReturnRow.EnableSN = false;
                        }
                    }
                    break;
                case "MOVEWIP":
                    {
                        jobHeadPlant = this.getJobHeadPlant(issueReturnRow.FromJobNum);
                        fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(jobHeadPlant);
                        /* the plant must be full serial tracked*/
                        if (fromPlantTracking == 2 && this.snRequired(issueReturnRow.FromJobNum, issueReturnRow.FromAssemblySeq, issueReturnRow.FromJobSeq, false))
                        {
                            issueReturnRow.EnableSN = true;
                            issueReturnRow.SerialControlPlant = jobHeadPlant;
                        }
                        else
                        {
                            issueReturnRow.EnableSN = false;
                        }
                    }
                    break;
                case "ADJUSTMATERIAL":
                    {
                        jobHeadPlant = this.getJobHeadPlant(issueReturnRow.ToJobNum);
                        fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(jobHeadPlant);
                        /* the plant must be full serial tracked*/
                        if (fromPlantTracking == 2)
                        {
                            issueReturnRow.EnableSN = true;
                            issueReturnRow.SerialControlPlant = jobHeadPlant;
                        }
                        else
                        {
                            issueReturnRow.EnableSN = false;
                        }
                    }
                    break;
                case "MOVEMATERIAL":
                    {
                        jobHeadPlant = this.getJobHeadPlant(issueReturnRow.ToJobNum);
                        fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(jobHeadPlant);
                        /* the plant must be full serial tracked*/
                        if (fromPlantTracking == 2)
                        {
                            issueReturnRow.EnableSN = true;
                            issueReturnRow.SerialControlPlant = jobHeadPlant;
                        }
                        else
                        {
                            issueReturnRow.EnableSN = false;
                        }
                        /* do not require serial numbers if the plant is set to not require serial entry on bin to bin movement within the same whs */
                        if (issueReturnRow.EnableSN == true)
                        {
                            PlantConfCtrl = this.FindFirstPlantConfCtrl(Session.CompanyID, jobHeadPlant);
                            if (PlantConfCtrl != null)
                            {
                                if (PlantConfCtrl.BinToBinReqSN == false && (StringExtensions.Compare(issueReturnRow.FromWarehouseCode, issueReturnRow.ToWarehouseCode) == 0))
                                {
                                    issueReturnRow.EnableSN = false;
                                    issueReturnRow.SerialControlPlant = "";
                                }
                            }
                        }
                    }
                    break;
                case "MATERIALQUEUE":
                case "HHMATERIALQUEUE":
                case "HHAUTOSELECTTRANSACTIONS":
                    {
                        if (
                            (issueReturnRow.TranType.Compare("WIP-WIP") == 0) ||
                            (issueReturnRow.TranType.Compare("PUR-SUB") == 0) ||
                            (issueReturnRow.TranType.Compare("INS-SUB") == 0)
                           )
                        {
                            jobHeadPlant = this.getJobHeadPlant(issueReturnRow.FromJobNum);
                            fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(jobHeadPlant);
                            issueReturnRow.EnableSN = false;
                            /* the plant must be full serial tracked*/
                            if ((fromPlantTracking == 2 && this.snRequired(issueReturnRow.FromJobNum, issueReturnRow.FromAssemblySeq, issueReturnRow.FromJobSeq, (issueReturnRow.TranType.Compare("PUR-SUB") == 0 || issueReturnRow.TranType.Compare("INS-SUB") == 0))))
                            {
                                issueReturnRow.EnableSN = true;
                                issueReturnRow.SerialControlPlant = jobHeadPlant;
                            }
                        }
                        else if (!string.IsNullOrEmpty(issueReturnRow.FromPCID))
                        {
                            string pcidPlant = string.Empty;
                            bool pcidOutboundContainer = false;

                            PkgControlHeader = this.FindFirstPkgControlHeader(Session.CompanyID, issueReturnRow.FromPCID);
                            if (PkgControlHeader == null)
                            {
                                var PkgControlStageHeader = this.FindFirstPkgControlStageHeader(Session.CompanyID, issueReturnRow.FromPCID);
                                if (PkgControlStageHeader == null)
                                {
                                    throw new BLException(Strings.PCIDNotFound(issueReturnRow.FromPCID));
                                }
                                else
                                {
                                    pcidPlant = PkgControlStageHeader.Plant;
                                    pcidOutboundContainer = PkgControlStageHeader.OutboundContainer;
                                }
                            }
                            else
                            {
                                pcidPlant = PkgControlHeader.Plant;
                                pcidOutboundContainer = PkgControlHeader.OutboundContainer;
                            }

                            fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(pcidPlant);

                            /* the plant must be full serial tracked*/
                            if (fromPlantTracking == 2)
                            {
                                issueReturnRow.EnableSN = true;
                                issueReturnRow.SerialControlPlant = pcidPlant;
                            }
                            else
                            {
                                issueReturnRow.EnableSN = false;
                            }

                            /* do not require serial numbers if the plant is set to not require serial entry on bin to bin movement within the same whs */
                            if (issueReturnRow.EnableSN == true)
                            {
                                PlantConfCtrl = this.FindFirstPlantConfCtrl(Session.CompanyID, pcidPlant);
                                if (PlantConfCtrl != null)
                                {
                                    if (PlantConfCtrl.BinToBinReqSN == false && (StringExtensions.Compare(issueReturnRow.FromWarehouseCode, issueReturnRow.ToWarehouseCode) == 0))
                                    {
                                        issueReturnRow.EnableSN = false;
                                        issueReturnRow.SerialControlPlant = "";
                                    }
                                }
                            }

                            // After checking the site configuration values, if the enable S/N is false, if the PCID is an outbound container, we need to prompt for serial numbers
                            if (issueReturnRow.EnableSN == false && pcidOutboundContainer)
                            {
                                issueReturnRow.EnableSN = true;
                                issueReturnRow.SerialControlPlant = pcidPlant;

                                //TODO: populate selected serial numbers 
                            }
                        }
                        else
                        {
                            whseOwnerPlant = ((issueReturnRow.TranType.Compare("MFG-OPR") == 0 || issueReturnRow.TranType.Compare("ASM-INS") == 0 || issueReturnRow.TranType.Compare("SUB-INS") == 0) ? this.getJobHeadPlant(issueReturnRow.FromJobNum) : this.getOwnerPlant(issueReturnRow.FromWarehouseCode));
                            fromPlantTracking = this.LibSerialCommon.isSerTraPlantType(whseOwnerPlant);
                            /* at least one plant must be serial tracked*/

                            bool subCon = false;

                            MtlQueue = this.FindFirstMtlQueue(issueReturnRow.MtlQueueRowId);
                            if (MtlQueue != null && (MtlQueue.NCTranID != 0 && (MtlQueue.TranType.KeyEquals("INS-ASM"))))
                            {
                                NonConf = this.FindFirstNonConf(MtlQueue.Company, MtlQueue.NCTranID);
                                subCon = (((NonConf != null && (NonConf.TrnTyp).KeyEquals("S"))) ? true : false);
                            }
                            bool isRMADisp = (((MtlQueue != null && (MtlQueue.RMADisp) > 0)) ? true : false);
                            issueReturnRow.EnableSN = false;
                            if ((fromPlantTracking == 2 || (fromPlantTracking == 3 && isRMADisp)))
                            {
                                if (
                                    // if the tran type are not any of the following
                                    (issueReturnRow.TranType.Compare("MFG-OPR") != 0 && issueReturnRow.TranType.Compare("ASM-INS") != 0 && issueReturnRow.TranType.Compare("SUB-INS") != 0 && issueReturnRow.TranType.Compare("INS-ASM") != 0
                                    ||
                                    // or the tran type is MFG-OPR or ASM-INS check the operation SN required flag for non-subcon oper
                                    ((issueReturnRow.TranType.Compare("MFG-OPR") == 0 || issueReturnRow.TranType.Compare("ASM-INS") == 0) && this.snRequired(issueReturnRow.FromJobNum, issueReturnRow.FromAssemblySeq, issueReturnRow.FromJobSeq, false))
                                    ||
                                    // or the tran type is SUB-INS check the operation SN required flag for subcon oper
                                    ((issueReturnRow.TranType.Compare("SUB-INS") == 0) && this.snRequired(issueReturnRow.FromJobNum, issueReturnRow.FromAssemblySeq, issueReturnRow.FromJobSeq, true))
                                    ||
                                    // or the tran type is INS-ASM check the operation SN required flag for operation or 
                                    ((issueReturnRow.TranType.Compare("INS-ASM") == 0) && this.snRequired(issueReturnRow.FromJobNum, issueReturnRow.FromAssemblySeq, issueReturnRow.FromJobSeq, subCon))
                                   ))
                                {
                                    if (!CanSkipSNForToFromWhsMatch(issueReturnRow.Company, issueReturnRow.TranType, whseOwnerPlant, issueReturnRow.ToWarehouseCode, issueReturnRow.FromWarehouseCode, issueReturnRow.PartNum))
                                    {
                                        issueReturnRow.EnableSN = true;
                                        issueReturnRow.SerialControlPlant = whseOwnerPlant;
                                        issueReturnRow.SerialControlPlant2 = this.getOwnerPlant(issueReturnRow.ToWarehouseCode);
                                        issueReturnRow.SerialControlPlantIsFromPlt = true;
                                    }
                                }
                            }
                            else
                            {
                                whseOwnerPlant = this.getOwnerPlant(issueReturnRow.ToWarehouseCode);
                                toPlantTracking = this.LibSerialCommon.isSerTraPlantType(whseOwnerPlant);
                                if ((toPlantTracking == 2 || (fromPlantTracking == 3 && isRMADisp) ||
                                    (toPlantTracking == 3 && (String.IsNullOrEmpty(issueReturnRow.ToPCID) ? false : GetPkgControlHeaderOutboundContainer(Session.CompanyID, issueReturnRow.ToPCID)))))
                                {
                                    issueReturnRow.EnableSN = true;
                                    issueReturnRow.SerialControlPlant = whseOwnerPlant;
                                }
                            }
                        }
                        /*
                         This will allow to skip serial number validation if no serial numbers
                         were added in the receipt process SCR # 81576
                        */
                        if (StringExtensions.Compare(issueReturnRow.TranType, "PUR-INS") == 0)
                        {
                            string validStatus = string.Empty;
                            checkSNStatus(issueReturnRow.TranType, issueReturnRow.TranQty, out validStatus);

                            SerialNo = FindFirstSerialNo(Session.CompanyID, issueReturnRow.PartNum, issueReturnRow.AttributeSetID, issueReturnRow.FromWarehouseCode, false, validStatus);
                            issueReturnRow.EnableSN = SerialNo != null;
                        }
                    }
                    break;
            }
        }

        ///<summary>
        ///  Purpose: Returns true if serial number selection should NOT be required based on 
        ///  whether a site has PlantConfCtrl.BinToBinReqSN = false
        ///  and other data in the ttIssueReturn row   
        ///  Notes:
        ///</summary>
        private bool CanSkipSNForToFromWhsMatch(string company, string tranType, string plant, string toWarehouseCode, string fromWarehouseCode, string partNum)
        {
            // for stock movement MTLQUEUE transactions if the site does not require SN on bin to bin in the same whs
            // and there are no serial number allocations for the part (to be consistent with InvTransfer), skip Serial selection requirements 
            if ((StringExtensions.Lookup("STK-STK,RAU-STK,RMG-STK,RMN-STK", tranType) > -1)
                && (!ExistsBinToBinReqSNPlantConfCtrl(company, plant, true))
                && fromWarehouseCode.KeyEquals(toWarehouseCode)
                && (!AreSNumsAllocated(company, partNum)))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Extract the PCID from a given string. Users can scan a barcode where only part of the string is the PCID.
        /// </summary>
        /// <param name="companyID">Current Company</param>
        /// <param name="fromPCID">Scanned string</param>
        /// <param name="libControlIDExtract">Internal.Lib.ControlIDExtract</param>
        /// <returns></returns>
        internal static string extractPCIDFromString(string companyID, string fromPCID, Internal.Lib.ControlIDExtract libControlIDExtract)
        {
            return libControlIDExtract.ExtractValueFromString_PCID(companyID, fromPCID);
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void getFromAssemblySeq(IssueReturnRow ttIssueReturn)
        {
            var JobAsmblResult = this.FindFirstJobAsmblFL(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq);
            if (JobAsmblResult != null)
            {
                if (StringExtensions.Compare(ttIssueReturn.TranType, "ASM-STK") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                {
                    ttIssueReturn.PartNum = JobAsmblResult.PartNum;
                    this.onChangePartNumCore(ttIssueReturn);
                    ttIssueReturn.UM = string.IsNullOrEmpty(ttIssueReturn.UM) ? JobAsmblResult.IUM : ttIssueReturn.UM; //If onChangePartNumCore does not set UM then it is a "Part on the Fly", set the JobAsm IUM.
                }
                if (StringExtensions.Compare(ttIssueReturn.TranType, "ASM-STK") == 0)
                {
                    ttIssueReturn.TranQty = 0;
                    ttIssueReturn.QtyPreviouslyIssued = JobAsmblResult.IssuedQty;
                    ttIssueReturn.QtyRequired = JobAsmblResult.PullQty;
                    ttIssueReturn.IssuedComplete = JobAsmblResult.IssuedComplete;
                    var outRequirementUOM = ttIssueReturn.RequirementUOM;
                    this.LibAppService.DefaultTransUOM(ttIssueReturn.PartNum, ttIssueReturn.UM, out outRequirementUOM);
                    ttIssueReturn.RequirementUOM = outRequirementUOM;
                }
                ttIssueReturn.FromAssemblyPartNum = JobAsmblResult.PartNum;
                ttIssueReturn.FromAssemblyPartDesc = JobAsmblResult.Description;

                //If the Attribute Set does not already exist and one does exist on JobAsmbl, pull it in and set the descriptions accordingly
                if (ttIssueReturn.AttributeSetID == 0 && JobAsmblResult.AttributeSetID != 0)
                {
                    ttIssueReturn.AttributeSetID = JobAsmblResult.AttributeSetID;

                }
                this.GetAttributeDescriptions(ref ttIssueReturn);
                ttIssueReturn.FromAssemblyRevisionNum = JobAsmblResult.RevisionNum;
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void getFromJobSeq(IssueReturnRow ttIssueReturn, bool runOnChangePartNumCore, bool clearQtyAndLot)
        {
            string cFromType = string.Empty;
            string tmpResourceGrpID = string.Empty;
            string defaultUOM = string.Empty;
            string transUOM = string.Empty;

            cFromType = this.getFromType(ttIssueReturn.TranType);
            /* Begin Get-FromJobSeq */
            if (StringExtensions.Compare(cFromType, "MTL") == 0)
            {
                JobMtl = this.FindFirstJobMtl(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq);
                if (JobMtl != null)
                {
                    transUOM = JobMtl.IUM;            /* Begin Disp-FromJobMtl */
                    if (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "MTL-MTL") == 0)
                    {
                        if (!ttIssueReturn.PartNum.Equals(string.Empty))
                        {
                            if (clearQtyAndLot)
                            {
                                ttIssueReturn.TranQty = 0;
                                ttIssueReturn.LotNum = string.Empty;
                            }
                        }
                        ttIssueReturn.PartNum = JobMtl.PartNum;
                    }
                    /* Set defaults from part */
                    this.getFromWhse(ttIssueReturn);
                    /* Uses validation rountine to set defaults from part */
                    if (runOnChangePartNumCore)
                        this.onChangePartNumCore(ttIssueReturn);
                    ttIssueReturn.FromJobSeqPartNum = JobMtl.PartNum;
                    ttIssueReturn.FromJobSeqPartDesc = JobMtl.Description;
                    ttIssueReturn.UM = JobMtl.IUM;
                    ttIssueReturn.RequirementUOM = JobMtl.IUM;
                    ttIssueReturn.OnHandUM = JobMtl.IUM;

                    //If the Attribute Set does exist on JobMtl, pull it in and set the descriptions accordingly
                    if (JobMtl.AttributeSetID != 0)
                    {
                        ttIssueReturn.AttributeSetID = JobMtl.AttributeSetID;
                        this.GetAttributeDescriptions(ref ttIssueReturn);
                    }

                    ttIssueReturn.RevisionNum = JobMtl.RevisionNum;

                    if (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") == 0)
                    {
                        ttIssueReturn.QtyRequired = JobMtl.RequiredQty;
                        ttIssueReturn.IssuedComplete = JobMtl.IssuedComplete;
                        ttIssueReturn.QtyPreviouslyIssued = JobMtl.IssuedQty;
                        ttIssueReturn.ToJobNum = "";
                        ttIssueReturn.ToAssemblySeq = 0;
                        ttIssueReturn.ToJobSeq = 0;
                    }

                    /* IF NO PART DESCRIPTION THEN USE THE JOB MATERIAL PART DESCRIPTION, UM */
                    if (String.IsNullOrEmpty(ttIssueReturn.PartPartDescription))
                    {
                        ttIssueReturn.PartPartDescription = JobMtl.Description;
                    }
                }
            }/* if cFromType = "MTL":U then */

            if (StringExtensions.Compare(cFromType, "OPR") == 0)
            {
                JobOper = this.FindFirstJobOper(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq);
                /* if available JobOper */
                if (JobOper != null)
                {
                    transUOM = JobOper.IUM;
                    if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                    {
                        this.getFromWhse(ttIssueReturn);
                        this.SetQuantity();
                    }

                    /* Disp-FromJobOper */
                    OpMaster = this.FindFirstOpMaster(JobOper.Company, JobOper.OpCode);
                    this.LibGetResourceGrpID.getJobOperResourceGrpID(JobOper, string.Empty);
                    ttIssueReturn.FromJobSeqPartDesc = ((OpMaster != null) ? OpMaster.OpDesc : "");
                    ttIssueReturn.FromJobSeqPartNum = tmpResourceGrpID + "/" + JobOper.OpCode;

                    /* IF NO PART DESCRIPTION THEN USE THE ASSEMLBY PART DESCRIPTION, UM */
                    if (String.IsNullOrEmpty(ttIssueReturn.PartPartDescription))
                    {
                        JobAsmbl = this.FindFirstJobAsmbl2(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq);
                        if (JobAsmbl != null)
                        {
                            ttIssueReturn.PartPartDescription = JobAsmbl.Description;
                            ttIssueReturn.UM = JobAsmbl.IUM;
                            ttIssueReturn.OnHandUM = JobAsmbl.IUM;
                        }
                    }
                }
                else if (ttIssueReturn.FromJobSeq == 0)
                {
                    ttIssueReturn.FromJobSeqPartDesc = "";
                    ttIssueReturn.FromJobSeqPartNum = "";

                    PartWip = FindFirstPartWip(ttIssueReturn.Company, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq, ttIssueReturn.FromPCID);
                    if (PartWip != null)
                    {
                        WhseBin = this.FindFirstWhseBin(PartWip.Company, PartWip.WareHouseCode, PartWip.BinNum);
                        ttIssueReturn.FromWarehouseCode = PartWip.WareHouseCode;
                        ttIssueReturn.FromWarehouseCodeDescription = FindFirstWarehseDescription(PartWip.Company, PartWip.WareHouseCode);
                        ttIssueReturn.FromBinNum = PartWip.BinNum;
                        ttIssueReturn.FromBinNumDescription = ((WhseBin != null) ? WhseBin.Description : "");
                        ttIssueReturn.FromPCID = "";
                    }
                    this.SetQuantity();
                }
            }/* if cFromType = "OPR":U then */

            /* Note (Orv) SCR101475 added the condition cFromType <> "MTL":U
               I did this just to safe and only affect  to MTL transactions.
               However, I believe this could be simplifed and apply to everything but this would require much more testing */

            if (cFromType.Compare("MTL") != 0)
            {
                if (String.IsNullOrEmpty(transUOM))
                {
                    transUOM = ttIssueReturn.UM;
                }

                this.LibAppService.DefaultTransUOM(ttIssueReturn.PartNum, transUOM, out defaultUOM);
                if (ttIssueReturn.TrackDimension || StringExtensions.Compare(cFromType, "MTL") != 0)
                {
                    ttIssueReturn.RequirementUOM = transUOM;
                }
                else
                {
                    if (StringExtensions.Compare(ttIssueReturn.RequirementUOM, defaultUOM) != 0 &&
                    StringExtensions.Compare(transUOM, defaultUOM) != 0)
                    {
                        var outQtyPreviouslyIssued = ttIssueReturn.QtyPreviouslyIssued;
                        this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.QtyPreviouslyIssued, transUOM, defaultUOM, out outQtyPreviouslyIssued, false);
                        ttIssueReturn.QtyPreviouslyIssued = outQtyPreviouslyIssued;
                        var outQtyRequired = ttIssueReturn.QtyRequired;
                        this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.QtyRequired, transUOM, defaultUOM, out outQtyRequired, false);
                        ttIssueReturn.QtyRequired = outQtyRequired;
                    }
                    ttIssueReturn.RequirementUOM = defaultUOM;
                }
            }
            var outQtyRequired2 = ttIssueReturn.QtyRequired;
            this.LibAppService.RoundToUOMDec(ttIssueReturn.RequirementUOM, ref outQtyRequired2);
            ttIssueReturn.QtyRequired = outQtyRequired2;
            if (ttIssueReturn.QtyPreviouslyIssued != 0)
            {
                var outQtyPreviouslyIssued2 = ttIssueReturn.QtyPreviouslyIssued;
                this.LibAppService.RoundToUOMDec(ttIssueReturn.RequirementUOM, ref outQtyPreviouslyIssued2);
                ttIssueReturn.QtyPreviouslyIssued = outQtyPreviouslyIssued2;
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void getFromWhse(IssueReturnRow ttIssueReturn)
        {
            string cFromType = string.Empty;
            string tmpWhse = string.Empty;
            string tmpWhseBin = string.Empty;
            string tmpPCID = string.Empty;
            bool lMultipleFromWhse = false;
            /*params for GetWarehosueInfo */
            string op_InputWhse = string.Empty;
            string op_InputBinNum = string.Empty;
            string op_OutputWhse = string.Empty;
            string op_OutputBinNum = string.Empty;
            string v_ResourceGrpID = string.Empty;
            string v_ResourceID = string.Empty;
            string contractID = string.Empty;
            string cToType = string.Empty;
            decimal outOnHandQty = decimal.Zero;
            ttIssueReturn.FromPCID = "";

            cFromType = this.getFromType(ttIssueReturn.TranType);
            cToType = this.getToType(ttIssueReturn.TranType);
            switch (cFromType.ToUpperInvariant())
            {
                case "STK":
                    {
                        if (cToType.Equals("MTL", StringComparison.OrdinalIgnoreCase))
                        {
                            contractID = this.ExistsJobMtlContractID(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq);
                        }
                        else if (cToType.Equals("ASM", StringComparison.OrdinalIgnoreCase) && ttIssueReturn.ToAssemblySeq != 0)
                        {
                            contractID = this.ExistsJobAsmblContractID(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq);
                        }

                        if (!string.IsNullOrEmpty(contractID))
                        {
                            var bPlanContractHdr = FindFirstPlanContractHdr(Session.CompanyID, contractID);
                            if (bPlanContractHdr != null)
                            {
                                var bPlanContractDtl = FindFirstPlanContractDtl(bPlanContractHdr.Company, bPlanContractHdr.ContractID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID);
                                if (bPlanContractDtl != null && !String.IsNullOrEmpty(bPlanContractDtl.WarehouseCode))
                                {
                                    ttIssueReturn.FromWarehouseCode = bPlanContractDtl.WarehouseCode;
                                    ttIssueReturn.FromWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, bPlanContractDtl.WarehouseCode);

                                    ttIssueReturn.FromBinNum = bPlanContractDtl.BinNum;
                                    ttIssueReturn.FromBinNumDescription = this.FindFirstWhseBinDescription(Session.CompanyID, bPlanContractDtl.WarehouseCode, bPlanContractDtl.BinNum);
                                    break;
                                }
                                else
                                {
                                    var bPlanContractWhseBinDefaultInv = FindFirstPlanContractWhseBinDefaultInv(bPlanContractHdr.Company, bPlanContractHdr.ContractID);
                                    if (bPlanContractWhseBinDefaultInv != null)
                                    {
                                        ttIssueReturn.FromWarehouseCode = bPlanContractWhseBinDefaultInv.WarehouseCode;
                                        ttIssueReturn.FromWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, bPlanContractWhseBinDefaultInv.WarehouseCode);

                                        ttIssueReturn.FromBinNum = bPlanContractWhseBinDefaultInv.BinNum;
                                        ttIssueReturn.FromBinNumDescription = this.FindFirstWhseBinDescription(Session.CompanyID, bPlanContractWhseBinDefaultInv.WarehouseCode, bPlanContractWhseBinDefaultInv.BinNum);
                                        break;
                                    }
                                }
                            }
                        }

                        lMultipleFromWhse = this.isWarehouseMultiple(cFromType, ttIssueReturn.PartNum);

                        var bJobMtlColumnsResult = this.FindFirstJobMtl2(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq, ttIssueReturn.PartNum);

                        /* FROM STOCK - DEFAULT TO PART PRIMARY WAREHOUSE */
                        /* if not lMultipleFromWhse */
                        if (!lMultipleFromWhse)
                        {
                            var PartWhseColumnsResult = this.FindFirstPartWhse(Session.CompanyID, ttIssueReturn.PartNum);

                            ttIssueReturn.FromWarehouseCode = ((!string.IsNullOrEmpty(bJobMtlColumnsResult)) ? bJobMtlColumnsResult : ((!string.IsNullOrEmpty(PartWhseColumnsResult)) ? PartWhseColumnsResult : ""));
                            ttIssueReturn.FromWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, ((!string.IsNullOrEmpty(bJobMtlColumnsResult)) ? bJobMtlColumnsResult : ((!string.IsNullOrEmpty(PartWhseColumnsResult)) ? PartWhseColumnsResult : "")));
                        }
                        else
                        {
                            var PartPlantColumnsResult = this.FindFirstPartPlantPrimWhse(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum);

                            ttIssueReturn.FromWarehouseCode = ((!string.IsNullOrEmpty(bJobMtlColumnsResult)) ? bJobMtlColumnsResult : ((!string.IsNullOrEmpty(PartPlantColumnsResult)) ? PartPlantColumnsResult : ""));
                            ttIssueReturn.FromWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, ((!string.IsNullOrEmpty(bJobMtlColumnsResult)) ? bJobMtlColumnsResult : ((!string.IsNullOrEmpty(PartPlantColumnsResult)) ? PartPlantColumnsResult : "")));
                        }/* else - if not lMultipleFromWhse */
                        /* Get From BinNumber */

                        var PrimBin = this.FindFirstPlantWhse(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode);
                        if (!string.IsNullOrEmpty(PrimBin))
                        {
                            var Description = this.FindFirstWhseBinDescription(Session.CompanyID, ttIssueReturn.FromWarehouseCode, PrimBin);
                            ttIssueReturn.FromBinNum = PrimBin;

                            if (!string.IsNullOrEmpty(Description))
                            {
                                ttIssueReturn.FromBinNumDescription = Description;
                            }
                        }
                        else
                        {
                            ttIssueReturn.FromBinNumDescription = "";
                        }    /* if available PlantWhse */

                        /* NO PRIMARY BIN - ATTEMPT TO FIND ANY BIN THAT CONTAINS THIS PART */
                        if (String.IsNullOrEmpty(ttIssueReturn.FromBinNum))
                        {
                            var BinNum = this.FindFirstPartBinBinNum(Session.CompanyID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, ttIssueReturn.FromWarehouseCode);
                            if (!string.IsNullOrEmpty(BinNum))
                            {
                                var Description = this.FindFirstWhseBinDescription(Session.CompanyID, ttIssueReturn.FromWarehouseCode, BinNum);
                                ttIssueReturn.FromBinNum = BinNum;

                                if (!string.IsNullOrEmpty(Description))
                                {
                                    ttIssueReturn.FromBinNumDescription = Description;
                                }
                            }
                            else
                            {
                                ttIssueReturn.FromBinNumDescription = "";
                            }
                        }
                        this.LibAppService.GetOnHandQty(ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode, "", ttIssueReturn.FromBinNum, ttIssueReturn.UM, "", ttIssueReturn.AttributeSetID, true, true, out outOnHandQty);
                        ttIssueReturn.OnHandQty = outOnHandQty;
                    }
                    break;/* when "STK":u */

                case "MTL":
                    {
                        if (!ttIssueReturn.FromJobPlant.KeyEquals(Session.PlantID) || (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") == 0 && Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) == false))
                        {
                            ttIssueReturn.FromWarehouseCode = "";
                            ttIssueReturn.FromWarehouseCodeDescription = "";
                            ttIssueReturn.FromBinNum = "";
                            ttIssueReturn.FromBinNumDescription = "";
                            if ((StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") == 0 && Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) == false))
                            {
                                return;
                            }
                        }
                        if (!ttIssueReturn.FromJobPlant.KeyEquals(Session.PlantID))
                        {
                            return;
                        }

                        var altJobMtlColumnsResult = this.FindFirstJobMtl3(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq);
                        if (altJobMtlColumnsResult == null)
                        {
                            return;
                        }

                        JobOperResult JobOperColumnsResult2 = new JobOperResult();
                        /* DEFAULT "FROM" WAREHOUSE TO THE "INPUT" WAREHOUSE OF THE WORKCENTER OF THE RELATED OPERATION */
                        /* DETERIMINE THE MATERIALS RELATED OPERATION */
                        if (altJobMtlColumnsResult.RelatedOperation > 0)
                        {
                            JobOperColumnsResult2 = this.FindFirstJobOper2(altJobMtlColumnsResult.Company, altJobMtlColumnsResult.JobNum, altJobMtlColumnsResult.AssemblySeq, altJobMtlColumnsResult.RelatedOperation);
                        }
                        else
                        {
                            var JobOperColumnsResult3 = this.FindFirstJobOper3(altJobMtlColumnsResult.Company, altJobMtlColumnsResult.JobNum, altJobMtlColumnsResult.AssemblySeq, 0);

                            if (JobOperColumnsResult3 != null)
                            {
                                JobOperColumnsResult2.JobNum = JobOperColumnsResult3.JobNum;
                                JobOperColumnsResult2.AssemblySeq = JobOperColumnsResult3.AssemblySeq;
                                JobOperColumnsResult2.OprSeq = JobOperColumnsResult3.OprSeq;
                                JobOperColumnsResult2.PrimaryProdOpDtl = JobOperColumnsResult3.PrimaryProdOpDtl;
                            }
                        }

                        /* GET THE DEFAULT WAREHOUSE/BIN FOR THE OPERATION FROM IT'S WORKCENETER */
                        if (JobOperColumnsResult2 != null)
                        {
                            JobOpDtl = this.FindFirstJobOpDtl(Session.CompanyID, JobOperColumnsResult2.JobNum, JobOperColumnsResult2.AssemblySeq, JobOperColumnsResult2.OprSeq, JobOperColumnsResult2.PrimaryProdOpDtl);
                            if (JobOpDtl != null)
                            {
                                this.LibGetWarehouseInfo._GetWarehouseInfo(out op_InputWhse, out op_InputBinNum, out op_OutputWhse, out op_OutputBinNum, ref v_ResourceGrpID, ref v_ResourceID, JobOperColumnsResult2.PrimaryProdOpDtl, ref JobOpDtl);
                            }/* if available JobOpDtl */

                            /* Check if warehouse and bin was selected from Material Request Queue */
                            tmpWhse = ((!String.IsNullOrEmpty(ttIssueReturn.FromWarehouseCode)) ? ttIssueReturn.FromWarehouseCode : op_InputWhse);
                            tmpWhseBin = ((!String.IsNullOrEmpty(ttIssueReturn.FromBinNum)) ? ttIssueReturn.FromBinNum : op_InputBinNum);
                            tmpPCID = ((!String.IsNullOrEmpty(ttIssueReturn.FromPCID)) ? ttIssueReturn.FromPCID : string.Empty);

                            var WhseBinDescription = this.FindFirstWhseBinDescription(Session.CompanyID, tmpWhse, tmpWhseBin);

                            ttIssueReturn.FromWarehouseCode = tmpWhse;
                            ttIssueReturn.FromWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, tmpWhse);
                            ttIssueReturn.FromBinNum = tmpWhseBin;
                            ttIssueReturn.FromBinNumDescription = ((!string.IsNullOrEmpty(WhseBinDescription)) ? WhseBinDescription : "");
                            ttIssueReturn.FromPCID = tmpPCID;
                        }
                    }
                    break;/* "FROM" MTL */
                case "ASM":
                    {
                        if (!ttIssueReturn.FromJobPlant.KeyEquals(Session.PlantID) || (StringExtensions.Compare(ttIssueReturn.TranType, "ASM-STK") == 0 && Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) == false))
                        {
                            ttIssueReturn.FromWarehouseCode = "";
                            ttIssueReturn.FromWarehouseCodeDescription = "";
                            ttIssueReturn.FromBinNum = "";
                            ttIssueReturn.FromBinNumDescription = "";
                            ttIssueReturn.FromPCID = "";
                            if ((StringExtensions.Compare(ttIssueReturn.TranType, "ASM-STK") == 0 && Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) == false))
                            {
                                return;
                            }
                        }
                        if (!ttIssueReturn.FromJobPlant.KeyEquals(Session.PlantID))
                        {
                            return;
                        }

                        var JobAsmblColumnsResult3 = this.FindFirstJobAsmbl3(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq);
                        if (JobAsmblColumnsResult3 == null)
                        {
                            return;
                        }

                        JobOperResult3 JobOperColumnsResult4 = new JobOperResult3();
                        /* DEFAULT "FROM" WAREHOUSE TO THE "INPUT" WAREHOUSE OF THE WORKCENTER OF THE RELATED OPERATION */
                        /* DETERIMINE THE ASSEMLBLIES RELATED OPERATION */
                        if (JobAsmblColumnsResult3.RelatedOperation > 0)
                        {
                            JobOperColumnsResult4 = this.FindFirstJobOper4(JobAsmblColumnsResult3.Company, JobAsmblColumnsResult3.JobNum, JobAsmblColumnsResult3.Parent, JobAsmblColumnsResult3.RelatedOperation);
                        }
                        else
                        {
                            var JobOperColumnsResult5 = this.FindFirstJobOper5(JobAsmblColumnsResult3.Company, JobAsmblColumnsResult3.JobNum, JobAsmblColumnsResult3.Parent, 0);
                            if (JobOperColumnsResult5 != null)
                            {
                                JobOperColumnsResult4.JobNum = JobOperColumnsResult5.JobNum;
                                JobOperColumnsResult4.AssemblySeq = JobOperColumnsResult5.AssemblySeq;
                                JobOperColumnsResult4.OprSeq = JobOperColumnsResult5.OprSeq;
                                JobOperColumnsResult4.PrimaryProdOpDtl = JobOperColumnsResult5.PrimaryProdOpDtl;
                            }
                        }

                        /* GET THE DEFAULT WAREHOUSE/BIN FOR THE OPERATION FROM IT'S WORKCENETER */
                        if (JobOperColumnsResult4 != null)
                        {
                            JobOpDtl = this.FindFirstJobOpDtl(Session.CompanyID, JobOperColumnsResult4.JobNum, JobOperColumnsResult4.AssemblySeq, JobOperColumnsResult4.OprSeq, JobOperColumnsResult4.PrimaryProdOpDtl);
                            if (JobOpDtl != null)
                            {
                                this.LibGetWarehouseInfo._GetWarehouseInfo(out op_InputWhse, out op_InputBinNum, out op_OutputWhse, out op_OutputBinNum, ref v_ResourceGrpID, ref v_ResourceID, JobOperColumnsResult4.PrimaryProdOpDtl, ref JobOpDtl);
                            }

                            tmpWhse = op_InputWhse;
                            tmpWhseBin = op_InputBinNum;
                            tmpPCID = string.Empty;

                            var WhseBinDescription = this.FindFirstWhseBinDescription(Session.CompanyID, tmpWhse, tmpWhseBin);

                            ttIssueReturn.FromWarehouseCode = tmpWhse;
                            ttIssueReturn.FromWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, tmpWhse);
                            ttIssueReturn.FromBinNum = tmpWhseBin;
                            ttIssueReturn.FromBinNumDescription = ((!string.IsNullOrEmpty(WhseBinDescription)) ? WhseBinDescription : "");
                            ttIssueReturn.FromPCID = tmpPCID;
                        }
                    }
                    break;/* "FROM" ASM */
                case "OPR":
                    //case "ASM":
                    {
                        tmpWhse = "";
                        if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                        {
                            var PartWipColumnsResult2 = FindFirstPartWip(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, "M", ttIssueReturn.FromJobSeq, ttIssueReturn.FromPCID, ttIssueReturn.AttributeSetID);
                            if (PartWipColumnsResult2 != null)
                            {
                                tmpWhse = PartWipColumnsResult2.WareHouseCode;
                                tmpWhseBin = PartWipColumnsResult2.BinNum;
                                ttIssueReturn.LotNum = PartWipColumnsResult2.LotNum;
                            }
                        }
                        if (String.IsNullOrEmpty(tmpWhse))
                        {
                            var JobOperColumnsResult6 = this.FindFirstJobOper6(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq);
                            if (JobOperColumnsResult6 != null)
                            {
                                JobOpDtl = this.FindFirstJobOpDtl(Session.CompanyID, JobOperColumnsResult6.JobNum, JobOperColumnsResult6.AssemblySeq, JobOperColumnsResult6.OprSeq, JobOperColumnsResult6.PrimaryProdOpDtl);
                                if (JobOpDtl != null)
                                {
                                    this.LibGetWarehouseInfo._GetWarehouseInfo(out op_InputWhse, out op_InputBinNum, out op_OutputWhse, out op_OutputBinNum, ref v_ResourceGrpID, ref v_ResourceID, JobOperColumnsResult6.PrimaryProdOpDtl, ref JobOpDtl);
                                }

                                tmpWhse = op_OutputWhse;
                                tmpWhseBin = op_OutputBinNum;
                            }
                        }

                        var WhseBinDescription = this.FindFirstWhseBinDescription(Session.CompanyID, tmpWhse, tmpWhseBin);

                        ttIssueReturn.FromWarehouseCode = tmpWhse;
                        ttIssueReturn.FromWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, tmpWhse);
                        ttIssueReturn.FromBinNum = tmpWhseBin;
                        ttIssueReturn.FromBinNumDescription = ((!string.IsNullOrEmpty(WhseBinDescription)) ? WhseBinDescription : "");
                    }
                    break;/* FROM WIP */
                case "UKN":
                    {
                    }
                    break;
                default:
                    {
                    }
                    break;
            }
        }

        private string getPlantFromWarehouse(string warehouse)
        {
            string plant = FindFirstWarehsePlant(Session.CompanyID, warehouse);
            return (!string.IsNullOrEmpty(plant)) ? plant : Session.PlantID;
        }

        private void GetAttributeDescriptions(ref IssueReturnRow ttIssueReturn)
        {
            using (var inventoryTracking = new InventoryTracking(Db))
            {
                inventoryTracking.GetAttributeDescriptions(ref ttIssueReturn);
            }
        }

        /// <summary>
        /// List of jobs that can be selected for Mass Issue.
        /// </summary>
        /// <param name="whereClauseJobHead">Where condition without the where word</param>
        /// <param name="whereClauseJobAsmbl">Where condition without the where word</param>
        /// <returns>Returns Epicor.Mfg.BO.IssueReturnJobAsmblDataSet</returns>
        /// <param name="pageSize"># of records returned.  0 means all</param>
        /// <param name="absolutePage"></param>
        /// <param name="morePages">Are there more pages ? Yes/No</param>
        [Ice.Hosting.Http.HttpGet]
        public IssueReturnJobAsmblTableset GetList(string whereClauseJobHead, string whereClauseJobAsmbl, int pageSize, int absolutePage, out bool morePages)
        {
            morePages = false;
            IssueReturnJobAsmblTableset ttIssueReturnJobAsmblTablesetDS = new IssueReturnJobAsmblTableset();
            ttIssueReturnJobAsmblTablesetDS = this.getListCore(whereClauseJobHead, whereClauseJobAsmbl, pageSize, absolutePage, out morePages);
            return ttIssueReturnJobAsmblTablesetDS;
        }

        /// <summary>
        /// MiscShipTo is not a real database table.  It stores results from
        /// </summary>
        /// <param name="whereClauseJobHead">Where clause to filter the query results.</param>
        /// <param name="whereClauseJobAsmbl">Where clause to filter the query results.</param>
        /// <returns>Returns MiscShipToList data set.</returns>
        /// <param name="pageSize">page size.</param>
        /// <param name="absolutePage">absolute page</param>
        /// <param name="morePages">more pages.</param>
        private IssueReturnJobAsmblTableset getListCore(string whereClauseJobHead, string whereClauseJobAsmbl, int pageSize, int absolutePage, out bool morePages)
        {
            morePages = false;
            IssueReturnJobAsmblTableset ttIssueReturnJobAsmblTablesetDS = new IssueReturnJobAsmblTableset();
            //bool successful = false;
            string sJobHeadWhere = string.Empty;
            string sJobHeadWhere1 = string.Empty;
            string sJobHeadWhere2 = string.Empty;
            string sJobAsmblWhere = string.Empty;
            string sJobAsmblWhere1 = string.Empty;
            string sJobAsmblWhere2 = string.Empty;
            string cQueryPrepare = string.Empty;
            //Erp.Tables.JobAsmbl altJobAsmbl = null;
            string cJobHeadSortBy = string.Empty;
            string cJobAsmblSortBy = string.Empty;
            int rowCount = 0;

            //SqlCommand qryJobHead = new SqlCommand();
            //object hJobHead_GetRows = null;
            //hJobHead_GetRows = qryJobHead;
            string[] pcTableName = new string[Ice.Constants.MAX_TABLES];/* Real database table name.  No temp-table names */
            string[] pcEFL = new string[Ice.Constants.MAX_TABLES];/* each, first or last for every pcTableName entry*/
            string[] pcFieldName = new string[Ice.Constants.MAX_TABLES];/* field name for every table. blank for all, without the tablename, pass RepRate[10] for array field */
            string[] pcJoinType = new string[Ice.Constants.MAX_TABLES];/* valid value is blank or outer-join. Mapping = outer-join = "left join" , blank = " join " of SQL */
            string[] pcWhereClause = new string[Ice.Constants.MAX_TABLES];/* where clause for every table including company if applicable and without the "where" itself */
            string pcOrderBy = string.Empty;/* Order By for the whole query comma separated tablename.fieldname without "by"  */
            string pcErrorMsg = string.Empty;/* This method returns Blank if everything ok */
            string cSortBy = string.Empty;
            ttJoinTableJobHeadAsmblRows.Clear();

            /* Prepare JobHead Where clause */
            sJobHeadWhere = whereClauseJobHead;
            if ((whereClauseJobAsmbl.IndexOf("JobHead.PartNum", StringComparison.InvariantCultureIgnoreCase) >= 0) || (whereClauseJobAsmbl.IndexOf("JobHead.EquipID", StringComparison.InvariantCultureIgnoreCase) >= 0))
            {
                sJobHeadWhere = whereClauseJobAsmbl;
            }

            sJobHeadWhere1 = " and JobHead.Plant ='" + Session.PlantID + "' and JobHead.JobClosed = 0";
            if (String.IsNullOrEmpty(sJobHeadWhere))
            {
                sJobHeadWhere = "JobHead.Company = '" + Session.CompanyID + "'" + sJobHeadWhere1;
            }
            else
            {
                sJobHeadWhere2 = sJobHeadWhere;
                cJobHeadSortBy = Ice.Manager.Data.ParseSort(ref sJobHeadWhere2);
                sJobHeadWhere = "JobHead.Company = '" + Session.CompanyID + "'" + sJobHeadWhere1;
                if (!String.IsNullOrEmpty(sJobHeadWhere2))
                {
                    sJobHeadWhere = sJobHeadWhere + " and " + sJobHeadWhere2;
                }
                //sJobHeadWhere = sJobHeadWhere + cJobHeadSortBy;
            }

            if (String.IsNullOrEmpty(cJobHeadSortBy))
            {
                cJobHeadSortBy = "Company,";

                if ((sJobHeadWhere.IndexOf("PartNum", StringComparison.InvariantCultureIgnoreCase) >= 0))
                    cJobHeadSortBy = cJobHeadSortBy + "PartNum,";

                if ((sJobHeadWhere.IndexOf("EquipID", StringComparison.InvariantCultureIgnoreCase) >= 0))
                    cJobHeadSortBy = cJobHeadSortBy + "EquipID,";
            }

            //if (!String.IsNullOrEmpty(sJobHeadWhere))
            //{
            //    sJobHeadWhere = " where " + sJobHeadWhere;
            //}

            /* Prepare JobAsmbl Where clause */
            sJobAsmblWhere = whereClauseJobAsmbl;
            if ((sJobAsmblWhere.IndexOf("JobAsmbl.Description", StringComparison.InvariantCultureIgnoreCase) >= 0))
            {
                cJobAsmblSortBy = "JobAsmbl.Description";
            }
            else
            {
                cJobAsmblSortBy = " JobAsmbl.Company, JobAsmbl.JobNum, JobAsmbl.AssemblySeq"; //"By" command removed.
            }
            sJobAsmblWhere1 = " and JobAsmbl.JobNum = JobHead.JobNum and JobAsmbl.Plant ='" + Session.PlantID + "'";

            if (sJobAsmblWhere == string.Empty)
            {
                sJobAsmblWhere = "JobAsmbl.Company = '" + Session.CompanyID + "'" + sJobAsmblWhere1;
            }
            else
            {
                sJobAsmblWhere2 = sJobAsmblWhere;
                cJobAsmblSortBy = Ice.Manager.Data.ParseSort(ref sJobAsmblWhere2) + cJobAsmblSortBy;
                sJobAsmblWhere = "JobAsmbl.Company = '" + Session.CompanyID + "'" + sJobAsmblWhere1;
                if (!String.IsNullOrEmpty(sJobAsmblWhere2))
                {
                    sJobAsmblWhere = sJobAsmblWhere + " and " + sJobAsmblWhere2;
                }
                //sJobAsmblWhere = sJobAsmblWhere + cJobAsmblSortBy;
            }

            //if (!String.IsNullOrEmpty(sJobAsmblWhere))
            //{
            //    sJobAsmblWhere = " where " + sJobAsmblWhere;
            //}

            //JOBHEAD
            pcTableName[0] = "Erp.JobHead";
            pcEFL[0] = " each ";
            pcFieldName[0] = "JobType,CallNum,JobNum,PartNum,PartDescription,EquipID";
            pcJoinType[0] = "";
            pcWhereClause[0] = sJobHeadWhere;
            //JOBASMBL
            pcTableName[1] = "Erp.JobAsmbl";
            pcEFL[1] = " each ";
            pcFieldName[1] = "Company,AssemblySeq,Description,SysRowID";
            pcJoinType[1] = "";
            pcWhereClause[1] = sJobAsmblWhere;
            pcOrderBy = cJobHeadSortBy + " " + cJobAsmblSortBy; //WARNING   

            this.LibExecuteQuery.Run<JoinTableJobHeadAsmbl>(pcTableName, pcEFL, pcFieldName, pcJoinType, pcWhereClause, pcOrderBy, 1, ttJoinTableJobHeadAsmblRows, out pcErrorMsg);

            /*** Copy the records to the temp-table */
            //Job_Loop:
            foreach (var item in ttJoinTableJobHeadAsmblRows)
            {
                ttJoinTableJobHeadAsmbl = item;

                if (StringExtensions.Compare(ttJoinTableJobHeadAsmbl.JobType, "SRV") == 0)
                {
                    FSCallhd = this.FindFirstFSCallhd(Session.CompanyID, ttJoinTableJobHeadAsmbl.CallNum);
                    if (FSCallhd == null)
                    {
                        continue;
                    }

                    if (FSCallhd.Invoiced == true)
                    {
                        continue;
                    }
                }/* if JobHead.JobType = "SRV" */

                /* CHECK FOR MATERIAL RECORDS */
                if (!(this.ExistsJobMtl(Session.CompanyID, ttJoinTableJobHeadAsmbl.JobNum)))
                {

                    if (!(this.ExistsJobAsmbl(Session.CompanyID, ttJoinTableJobHeadAsmbl.JobNum, 0)))
                    {
                        continue;
                    }
                }/* if not can-find (first JobMtl ... */

                ttIssueReturnJobAsmbl = new Erp.Tablesets.IssueReturnJobAsmblRow();
                ttIssueReturnJobAsmblTablesetDS.IssueReturnJobAsmbl.Add(ttIssueReturnJobAsmbl);
                ttIssueReturnJobAsmbl.Company = ttJoinTableJobHeadAsmbl.Company;
                ttIssueReturnJobAsmbl.JobNum = ttJoinTableJobHeadAsmbl.JobNum;
                ttIssueReturnJobAsmbl.AssemblySeq = ttJoinTableJobHeadAsmbl.AssemblySeq;
                ttIssueReturnJobAsmbl.PartNum = ttJoinTableJobHeadAsmbl.PartNum;
                ttIssueReturnJobAsmbl.Description = ttJoinTableJobHeadAsmbl.Description;
                ttIssueReturnJobAsmbl.JobHeadPartNum = ttJoinTableJobHeadAsmbl.PartNum;
                ttIssueReturnJobAsmbl.JobHeadPartDescription = ttJoinTableJobHeadAsmbl.PartDescription;
                ttIssueReturnJobAsmbl.EquipID = ttJoinTableJobHeadAsmbl.EquipID;
                ttIssueReturnJobAsmbl.SysRowID = ttJoinTableJobHeadAsmbl.SysRowID;

                /*** Paging */
                if (pageSize > 0)
                {
                    rowCount = rowCount + 1;
                    if (rowCount == pageSize)
                    {
                        morePages = true;
                        break;
                    }
                }
            }

            return ttIssueReturnJobAsmblTablesetDS;
        }

        /// <summary>
        /// List of jobs that can be selected for Mass Issue.
        /// </summary>
        /// <param name="whereClauseJobHead">Where condition without the where word</param>
        /// <returns>Returns Epicor.Mfg.BO.IssueReturnJobListDataSet</returns>
        /// <param name="pageSize"># of records returned.  0 means all</param>
        /// <param name="absolutePage"></param>
        /// <param name="morePages">Are there more pages ? Yes/No</param>
        public IssueReturnJobListTableset GetListJobs(string whereClauseJobHead, int pageSize, int absolutePage, out bool morePages)
        {
            morePages = false;
            IssueReturnJobListTableset ttIssueReturnJobListTablesetDS = new IssueReturnJobListTableset();
            //bool successful = false;
            string sWhere = string.Empty;
            string sWhere1 = string.Empty;
            //SqlCommand qryJobHead = new SqlCommand();
            //object hJobHead_GetRows = null;
            //hJobHead_GetRows = qryJobHead;
            JobHeadTable ttJobHeadRows = new JobHeadTable();

            string[] pcTableName = new string[Ice.Constants.MAX_TABLES];/* Real database table name.  No temp-table names */
            string[] pcEFL = new string[Ice.Constants.MAX_TABLES];/* each, first or last for every pcTableName entry*/
            string[] pcFieldName = new string[Ice.Constants.MAX_TABLES];/* field name for every table. blank for all, without the tablename, pass RepRate[10] for array field */
            string[] pcJoinType = new string[Ice.Constants.MAX_TABLES];/* valid value is blank or outer-join. Mapping = outer-join = "left join" , blank = " join " of SQL */
            string[] pcWhereClause = new string[Ice.Constants.MAX_TABLES];/* where clause for every table including company if applicable and without the "where" itself */
            string pcOrderBy = string.Empty;/* Order By for the whole query comma separated tablename.fieldname without "by"  */
            string pcErrorMsg = string.Empty;/* This method returns Blank if everything ok */
            string cSortBy = string.Empty;

            /*** Retrieve list of columns the user is not granted access to */
            IList<string> deniedColumns01 = Ice.Manager.Security.GetReadDeniedColumns(Session.CompanyID, Session.UserID, "Erp", "JobHead");

            sWhere = whereClauseJobHead;
            sWhere1 = " and JobHead.Plant ='" + Session.PlantID + "' and JobHead.JobReleased and NOT JobHead.JobClosed ";

            if (sWhere == string.Empty)
            {
                sWhere = "Company = '" + Session.CompanyID + "'" + sWhere1;
            }
            else
            {
                sWhere = "Company = '" + Session.CompanyID + "'" + sWhere1 + " and " + sWhere;
            }

            //if (!String.IsNullOrEmpty(sWhere))
            //{
            //    sWhere = " where " + sWhere;
            //}

            pcTableName[0] = "Erp.JobHead";
            pcEFL[0] = " each ";
            pcFieldName[0] = "";
            pcJoinType[0] = "";
            pcWhereClause[0] = sWhere; //Warning
            pcOrderBy = "";
            this.LibExecuteQuery.Run(pcTableName, pcEFL, pcFieldName, pcJoinType, pcWhereClause, pcOrderBy, 1, ttJobHeadRows, out pcErrorMsg);

            foreach (var JobHeadRow_iterator in ttJobHeadRows)
            {
                if (JobHeadRow_iterator != null)
                {
                    if (JobHeadRow_iterator.JobReleased == false)
                    {
                        continue;
                    }

                    if (JobHeadRow_iterator.JobClosed)
                    {
                        continue;
                    }

                    if (!JobHeadRow_iterator.Plant.KeyEquals(Session.PlantID))
                    {
                        continue;
                    }

                    if (StringExtensions.Compare(JobHeadRow_iterator.JobType, "SRV") == 0)
                    {
                        FSCallhd = this.FindFirstFSCallhd(Session.CompanyID, JobHeadRow_iterator.CallNum);
                        if (FSCallhd == null)
                        {
                            continue;
                        }

                        if (FSCallhd.Invoiced == true)
                        {
                            continue;
                        }
                    }/* if JobHead.JobType = "SRV" */

                    /* CHECK FOR MATERIAL RECORDS */
                    if (!(this.ExistsJobMtl(Session.CompanyID, JobHeadRow_iterator.JobNum)))
                    {
                        if (!(this.ExistsJobAsmbl2(Session.CompanyID, JobHeadRow_iterator.JobNum, 0)))
                        {
                            continue;
                        }
                    }/* if not can-find (first JobMtl ... */
                    ttJobHead = new Erp.Tablesets.JobHeadRow();
                    ttIssueReturnJobListTablesetDS.JobHead.Add(ttJobHead);
                    //BufferCopy.Copy(ttJobHead, JobHead, deniedColumns01);
                    BufferCopy.CopyExceptFor(JobHeadRow_iterator, ttJobHead, deniedColumns01);
                    ttJobHead.SysRowID = JobHeadRow_iterator.SysRowID;
                }
            }
            return ttIssueReturnJobListTablesetDS;
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void getLotFIFOCosts(IssueReturnRow ttIssueReturn, string ipCostID, out decimal opMtlUnitCost, out decimal opLbrUnitCost, out decimal opBurUnitCost, out decimal opSubUnitCost, out decimal opMtlBurUnitCost, out DateTime? opFIFODate, out int opFIFOSeq, out bool opFIFOFound)
        {
            opMtlUnitCost = decimal.Zero;
            opLbrUnitCost = decimal.Zero;
            opBurUnitCost = decimal.Zero;
            opSubUnitCost = decimal.Zero;
            opMtlBurUnitCost = decimal.Zero;
            opFIFODate = null;
            opFIFOSeq = 0;
            opFIFOFound = false;

            PartFIFOCost = this.FindFirstPartFIFOCost(ttIssueReturn.Company, false, ttIssueReturn.PartNum, ttIssueReturn.LotNum, ipCostID);
            if (PartFIFOCost != null)
            {
                opFIFOFound = true;
                opMtlUnitCost = PartFIFOCost.FIFOMaterialCost;
                opLbrUnitCost = PartFIFOCost.FIFOLaborCost;
                opBurUnitCost = PartFIFOCost.FIFOBurdenCost;
                opSubUnitCost = PartFIFOCost.FIFOSubContCost;
                opMtlBurUnitCost = PartFIFOCost.FIFOMtlBurCost;
                opFIFODate = PartFIFOCost.FIFODate;
                opFIFOSeq = PartFIFOCost.FIFOSeq;
            }
        }

        ///<summary>
        ///  Purpose:     Similar logic found in getMiscReturnUnitCosts procedure.
        ///               This logic is to get the correct miscellaneous return costs for
        ///               non-FIFO costed parts.
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void getMiscReturnAltUnitCosts(IssueReturnRow ttIssueReturn, Guid pPartTranRowid, string ipCostId, out decimal pMtlUnitCost, out decimal pLbrunitCost, out decimal pBurunitCost, out decimal pSubUnitCost, out decimal pMtlBurunitCost)
        {
            pMtlUnitCost = decimal.Zero;
            pLbrunitCost = decimal.Zero;
            pBurunitCost = decimal.Zero;
            pSubUnitCost = decimal.Zero;
            pMtlBurunitCost = decimal.Zero;
            decimal TotalMtlCost = decimal.Zero;
            decimal TotalLbrCost = decimal.Zero;
            decimal TotalBurCost = decimal.Zero;
            decimal TotalSubCost = decimal.Zero;
            decimal TotalMtlBurCost = decimal.Zero;
            decimal TranMtlCost = decimal.Zero;
            decimal TranLbrCost = decimal.Zero;
            decimal TranBurCost = decimal.Zero;
            decimal TranSubCost = decimal.Zero;
            decimal TranMtlBurCost = decimal.Zero;
            decimal TotalTranQty = decimal.Zero;
            string curCostID = string.Empty;
            int ndec = 0;
            Erp.Tables.PartTran bPartTran = null;
            ndec = this.LibGetDecimalsNumber.getDecimalsNumberByName("PartTran", "MtlUnitCost", "");
            /* SCR #19110 - when trying to calculate outstanding transaction qty, make sure *
             * that the current PartTran is not included in the for each.  In Vantage 6.1,  *
             * the original logic does not include the line that qualifies the Rowid since  *
             * in 6.1 version, when this logic is called the new PartTran is not committed  *
             * yet in the database.  In Sonoma, when this logic is called the new PartTran  *
             * is already committed in the DB and thus causing the tranqty total to be off. */

            //bPartTran_Loop:
            foreach (var bPartTran_iterator in (this.SelectPartTran(Session.CompanyID, "STK-UKN", ttIssueReturn.PartNum, pPartTranRowid)))
            {
                bPartTran = bPartTran_iterator;
                curCostID = bPartTran.CostID;
                if (String.IsNullOrEmpty(bPartTran.CostID))
                {
                    this.LibGetPlantCostID._getPlantCostID(bPartTran.Plant, out curCostID, ref Plant, ref XaSyst);
                }
                if (StringExtensions.Compare(curCostID, ipCostId) != 0)
                {
                    continue;
                }

                TranMtlCost = bPartTran.AltMtlUnitCost;
                TranLbrCost = bPartTran.AltLbrUnitCost;
                TranBurCost = bPartTran.AltBurUnitCost;
                TranSubCost = bPartTran.AltSubUnitCost;
                TranMtlBurCost = bPartTran.AltMtlBurUnitCost;        /* In case the alternate costs are not populated then use the regular costs */
                if (TranMtlCost == 0 && TranLbrCost == 0 && TranBurCost == 0 && TranSubCost == 0 && TranMtlBurCost == 0)
                {
                    TranMtlCost = bPartTran.MtlUnitCost;
                    TranLbrCost = bPartTran.LbrUnitCost;
                    TranBurCost = bPartTran.BurUnitCost;
                    TranSubCost = bPartTran.SubUnitCost;
                    TranMtlBurCost = bPartTran.MtlBurUnitCost;
                }
                TotalTranQty = TotalTranQty + bPartTran.TranQty;
                TotalMtlCost = Math.Round(TotalMtlCost + (TranMtlCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                TotalLbrCost = Math.Round(TotalLbrCost + (TranLbrCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                TotalBurCost = Math.Round(TotalBurCost + (TranBurCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                TotalSubCost = Math.Round(TotalSubCost + (TranSubCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                TotalMtlBurCost = Math.Round(TotalMtlBurCost + (TranMtlBurCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
            }/* FOR EACH PartTran */
            if (TotalTranQty > 0)
            {
                pMtlUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(TotalMtlCost / TotalTranQty, "", "PartTran", "MtlUnitCost");
                pLbrunitCost = this.LibRoundAmountEF.RoundDecimalsApply(TotalLbrCost / TotalTranQty, "", "PartTran", "LbrunitCost");
                pBurunitCost = this.LibRoundAmountEF.RoundDecimalsApply(TotalBurCost / TotalTranQty, "", "PartTran", "BurunitCost");
                pSubUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(TotalSubCost / TotalTranQty, "", "PartTran", "SubUnitCost");
                pMtlBurunitCost = this.LibRoundAmountEF.RoundDecimalsApply(TotalMtlBurCost / TotalTranQty, "", "PartTran", "MtlBurunitCost");
            }
        }
        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void getMiscReturnUnitCosts(IssueReturnRow ttIssueReturn, Guid pPartTranRowid, string ipCostId, out decimal pMtlUnitCost, out decimal pLbrunitCost, out decimal pBurunitCost, out decimal pSubUnitCost, out decimal pMtlBurunitCost)
        {
            pMtlUnitCost = decimal.Zero;
            pLbrunitCost = decimal.Zero;
            pBurunitCost = decimal.Zero;
            pSubUnitCost = decimal.Zero;
            pMtlBurunitCost = decimal.Zero;
            decimal TotalMtlCost = decimal.Zero;
            decimal TotalLbrCost = decimal.Zero;
            decimal TotalBurCost = decimal.Zero;
            decimal TotalSubCost = decimal.Zero;
            decimal TotalMtlBurCost = decimal.Zero;
            decimal TotalTranQty = decimal.Zero;
            string curCostID = string.Empty;
            int ndec = 0;
            Erp.Tables.PartTran bPartTran = null;
            ndec = this.LibGetDecimalsNumber.getDecimalsNumberByName("PartTran", "MtlUnitCost", "");
            /* SCR #19110 - when trying to calculate outstanding transaction qty, make sure *
             * that the current PartTran is not included in the for each.  In Vantage 6.1,  *
             * the original logic does not include the line that qualifies the Rowid since  *
             * in 6.1 version, when this logic is called the new PartTran is not committed  *
             * yet in the database.  In Sonoma, when this logic is called the new PartTran  *
             * is already committed in the DB and thus causing the tranqty total to be off. */

            //bPartTran_Loop:
            foreach (var bPartTran_iterator in (this.SelectPartTran(Session.CompanyID, "STK-UKN", ttIssueReturn.PartNum, pPartTranRowid)))
            {
                bPartTran = bPartTran_iterator;
                curCostID = bPartTran.CostID;
                if (String.IsNullOrEmpty(bPartTran.CostID))
                {
                    this.LibGetPlantCostID._getPlantCostID(bPartTran.Plant, out curCostID, ref Plant, ref XaSyst);
                }
                if (StringExtensions.Compare(curCostID, ipCostId) != 0)
                {
                    continue;
                }

                TotalTranQty = TotalTranQty + bPartTran.TranQty;
                TotalMtlCost = Math.Round(TotalMtlCost + (bPartTran.MtlUnitCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                TotalLbrCost = Math.Round(TotalLbrCost + (bPartTran.LbrUnitCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                TotalBurCost = Math.Round(TotalBurCost + (bPartTran.BurUnitCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                TotalSubCost = Math.Round(TotalSubCost + (bPartTran.SubUnitCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                TotalMtlBurCost = Math.Round(TotalMtlBurCost + (bPartTran.MtlBurUnitCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
            }/* FOR EACH PartTran */
            if (TotalTranQty > 0)
            {
                pMtlUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(TotalMtlCost / TotalTranQty, "", "PartTran", "MtlUnitCost");
                pLbrunitCost = this.LibRoundAmountEF.RoundDecimalsApply(TotalLbrCost / TotalTranQty, "", "PartTran", "LbrunitCost");
                pBurunitCost = this.LibRoundAmountEF.RoundDecimalsApply(TotalBurCost / TotalTranQty, "", "PartTran", "BurunitCost");
                pSubUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(TotalSubCost / TotalTranQty, "", "PartTran", "SubUnitCost");
                pMtlBurunitCost = this.LibRoundAmountEF.RoundDecimalsApply(TotalMtlBurCost / TotalTranQty, "", "PartTran", "MtlBurunitCost");
            }
        }

        /// <summary>
        /// Call this method to create a new Epicor.Mfg.BO.IssueReturnDataSet with
        /// default values.
        /// </summary>
        /// <param name="pcTranType">Material movement type XXX-XXX e.g STK-MTL.  It can be blank when a valid MtlQueue RowIdent is passed.</param>
        /// <param name="pcMtlQueueRowID">Progress database RowId of MtlQueue record</param>
        /// <param name="pCallProcess">Calling Process</param>
        /// <param name="ds">IssueReturnDataSet</param>
        public void GetNewIssueReturn(string pcTranType, Guid pcMtlQueueRowID, string pCallProcess, ref IssueReturnTableset ds)
        {
            CurrentFullTableset = ds;

            ttIssueReturn = new Erp.Tablesets.IssueReturnRow();
            CurrentFullTableset.IssueReturn.Add(ttIssueReturn);
            ttIssueReturn.DummyKeyField = Session.UserID + Erp.Internal.Lib.DateExtensions.SecondsSinceMidnight(CompanyTime.Now()) + Compatibility.Convert.ToString((new Random()).Next(0, Erp.Internal.Lib.DateExtensions.SecondsSinceMidnight(CompanyTime.Now()))); /* Just trying to make a unique key */
            ttIssueReturn.Company = Session.CompanyID;
            ttIssueReturn.TranDate = LibOffSet.OffsetToday();
            ttIssueReturn.IssuedComplete = false;
            ttIssueReturn.Plant = Session.PlantID;
            ttIssueReturn.FromJobPlant = Session.PlantID;
            ttIssueReturn.ToJobPlant = Session.PlantID;
            ttIssueReturn.SysRowID = Guid.NewGuid(); //Guid.Parse(ttIssueReturn.DummyKeyField);
            ttIssueReturn.DimConvFactor = 1;
            ttIssueReturn.RowMod = IceRow.ROWSTATE_ADDED;

            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && !pcMtlQueueRowID.Equals(Guid.Empty))
            {
                MtlQueue = FindFirstMtlQueue(pcMtlQueueRowID);
                if (MtlQueue == null)
                    throw new BLException(Strings.AValidMtlQuRowIdIsRequi, "MtlQueue");

                if (MtlQueue.Visible == false)
                    throw new BLException(Strings.ProgramErrorVisibleFlagNoShouldNotHaveAllowedSelec);

                processMtlQueue(pcMtlQueueRowID, ttIssueReturn);

                /* ERPS-106804 - Special consideration for WIP-WIP coming from MtlQueue. Assign the correct ToAssemblySeq/ToJobSeq  *
                 * instead of using the values assigned from processMtlQueue temporarily.                                           */
                if (ttIssueReturn.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase) && ttIssueReturn.MtlQueueRowId != Guid.Empty)
                {
                    vSaveToAssemblySeq = ttIssueReturn.ToAssemblySeq;
                    vSaveToJobSeq = ttIssueReturn.ToJobSeq;
                    ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
                    ttIssueReturn.ToJobSeq = ttIssueReturn.FromJobSeq;
                }

                string pcMessage = string.Empty;
                getToJobSeq(ttIssueReturn, false, false, out pcMessage);

                // since this is coming from materialQueue, warehouses should already be populated
                // but we'll check and populate empty ones
                if (string.IsNullOrEmpty(ttIssueReturn.FromWarehouseCode))
                    getFromWhse(ttIssueReturn);

                if (string.IsNullOrEmpty(ttIssueReturn.ToWarehouseCode))
                    getToWhse(ttIssueReturn);

                if (MtlQueue.TranType.Equals("MTL-STK", StringComparison.OrdinalIgnoreCase))
                    getFromJobSeq(ttIssueReturn, false, false);

                ConvQty = ttIssueReturn.TranQty;
                if (!string.IsNullOrEmpty(ttIssueReturn.PartNum))
                    LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, ttIssueReturn.RequirementUOM, out ConvQty, false);
                ttIssueReturn.RequirementQty = ConvQty;

                if (!MtlQueue.TranType.Equals("MTL-STK", StringComparison.OrdinalIgnoreCase))
                    setIssuedComplete(ttIssueReturn);

                /* determineSensitivity builds the list of fields which are applicable for the TranType */
                determineSensitivity(ttIssueReturn);

                /* PCID */
                bool enablePackageControl = ExistsPlantConfCtrlEnablePackageControl(Session.CompanyID, Session.PlantID, true);

                if (!String.IsNullOrEmpty(MtlQueue.PCID) && MtlQueue.PCID.KeyCompare("USE-FROMPCID-TOPCID") != 0)
                {
                    ValidatePCIDForMovement(enablePackageControl, MtlQueue.PCID, MtlQueue.TranType);
                    ttIssueReturn.FromPCID = MtlQueue.PCID;
                }
                // If FromPCID and ToPCID are non-blank and they are different, execute the validation, if it passes, set the FromPCID
                else if (!String.IsNullOrEmpty(MtlQueue.FromPCID) && !String.IsNullOrEmpty(MtlQueue.ToPCID) && !(StringExtensions.Compare(ttIssueReturn.FromPCID, MtlQueue.PCID) != 0) && MtlQueue.PCID.KeyCompare("USE-FROMPCID-TOPCID") == 0)
                {
                    ValidatePCIDForMovement(enablePackageControl, MtlQueue.FromPCID, MtlQueue.TranType);
                    ttIssueReturn.FromPCID = MtlQueue.FromPCID;
                }
                if (string.IsNullOrEmpty(MtlQueue.ToPCID))
                {
                    if (!string.IsNullOrEmpty(MtlQueue.PCID) && MtlQueue.PCID.KeyCompare("USE-FROMPCID-TOPCID") == 1)
                    {
                        ttIssueReturn.ToPCID = MtlQueue.PCID;
                    }
                    else if ((string.IsNullOrEmpty(MtlQueue.PCID) || MtlQueue.PCID.KeyCompare("USE-FROMPCID-TOPCID") == 0) &&
                             (!string.IsNullOrEmpty(MtlQueue.LastUsedPCID)))
                    {
                        ttIssueReturn.ToPCID = MtlQueue.LastUsedPCID;
                        // if PCID is not empty, default the current PCID whs/bin
                        PkgControlHeaderWhsAndStatus WhsAndStatusPkgControlHeader = FindFirstPkgControlHeaderPartialRow(Session.CompanyID, ttIssueReturn.ToPCID);
                        if (WhsAndStatusPkgControlHeader != null && !WhsAndStatusPkgControlHeader.PkgControlStatus.KeyEquals("EMPTY"))
                        {
                            ttIssueReturn.ToWarehouseCode = WhsAndStatusPkgControlHeader.WarehouseCode;
                            ttIssueReturn.ToBinNum = WhsAndStatusPkgControlHeader.BinNum;
                            SetToWhsBinDescriptions(ref ttIssueReturn);
                        }
                    }
                }

                using (Erp.Internal.Lib.PackageControl libPackageControl = new Internal.Lib.PackageControl(Db))
                {
                    if (!enablePackageControl)
                    {
                        ttIssueReturn.EnableToPCID = false;
                    }
                    else if (libPackageControl.IsWinClientClientType() &&
                             !MtlQueue.TranType.Equals("CMP-SHP", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("STK-SHP", StringComparison.OrdinalIgnoreCase))
                    {
                        ttIssueReturn.EnableToPCID = false;
                    }
                    else if (!libPackageControl.IsWinClientClientType() &&
                             !MtlQueue.TranType.Equals("ASM-INS", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("CMP-SHP", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("DMR-ASM", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("DMR-MTL", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("DMR-SUB", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("INS-ASM", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("INS-MTL", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("INS-SUB", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("MTL-INS", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("MFG-CUS", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("MFG-OPR", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("MTL-INS", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("MTL-MTL", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("PLT-MTL", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("STK-ASM", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("STK-MTL", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("STK-SHP", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("SUB-INS", StringComparison.OrdinalIgnoreCase) &&
                             !MtlQueue.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase))
                    {
                        ttIssueReturn.EnableToPCID = false;
                    }
                    else
                    {
                        ttIssueReturn.EnableToPCID = true;
                        ttIssueReturn.EnablePCIDGen = true;
                        ttIssueReturn.EnablePCIDPrint = true;
                    }
                }


                /* ERPS-106804 - Special consideration for WIP-WIP coming from MtlQueue. */
                if (ttIssueReturn.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase) && ttIssueReturn.MtlQueueRowId != Guid.Empty)
                {
                    ttIssueReturn.ToAssemblySeq = vSaveToAssemblySeq;
                    ttIssueReturn.ToJobSeq = vSaveToJobSeq;
                }
            }
            else
            {
                if (!isValidTranType(pcTranType))
                    throw new BLException(Strings.AValidTrantypeIsRequired, "IssueReturn", "TranType");

                ttIssueReturn.TranType = pcTranType;
                ttIssueReturn.MtlQueueRowId = Guid.Empty;
            }

            string sysTranID = getLegalNumberType(ttIssueReturn.TranType);
            ttIssueReturn.TranDocTypeID = LibGetAvailTranDocTypes.GetTranDocTypeIdDefault(sysTranID);

            var outEnableToFields = ttIssueReturn.EnableToFields;
            var outEnableFromFields = ttIssueReturn.EnableFromFields;
            disableFromToFields(pcTranType, out outEnableToFields, out outEnableFromFields);
            ttIssueReturn.EnableToFields = outEnableToFields;
            ttIssueReturn.EnableFromFields = outEnableFromFields;

            checkStatusTracking();
            enableSNButton(ttIssueReturn, pCallProcess);
            FillForeignFields(ttIssueReturn);
        }

        /// <summary>
        /// Call this method to create a new Epicor.Mfg.BO.IssueReturnDataSet with
        /// default values.
        /// </summary>
        /// <param name="pcFromJobNum">From Job number.</param>
        /// <param name="piFromAssemblySeq">From Assembly Seq.</param>
        /// <param name="pcTranType">Material movement type XXX-XXX e.g STK-MTL</param>
        /// <param name="pcMtlQueueRowID">Progress database rowid for MtlQueue record</param>
        /// <param name="ds">IssueReturnDataSet</param>
        public void GetNewIssueReturnFromJob(string pcFromJobNum, int piFromAssemblySeq, string pcTranType, Guid pcMtlQueueRowID, ref IssueReturnTableset ds)
        {
            CurrentFullTableset = ds;
            Guid rMtlQueueRowID = Guid.Empty;
            string sysTranID = string.Empty;
            if (!this.isValidTranType(pcTranType))
            {
                throw new BLException(Strings.AValidTrantypeIsRequired, "IssueReturn", "TranType");
            }
            ttIssueReturn = new Erp.Tablesets.IssueReturnRow();
            CurrentFullTableset.IssueReturn.Add(ttIssueReturn);
            ttIssueReturn.DummyKeyField = Session.UserID + Compatibility.Convert.TimeToString(CompanyTime.Now()) + Compatibility.Convert.ToString((new Random()).Next(0, Erp.Internal.Lib.DateExtensions.SecondsSinceMidnight(CompanyTime.Now()))); /* Just trying to make a unique key */
            ttIssueReturn.Company = Session.CompanyID;
            ttIssueReturn.TranDate = this.LibOffSet.OffsetToday();
            ttIssueReturn.IssuedComplete = false;
            ttIssueReturn.MtlQueueRowId = Guid.Empty;
            ttIssueReturn.TranType = pcTranType;
            ttIssueReturn.Plant = Session.PlantID;
            ttIssueReturn.FromJobPlant = Session.PlantID;
            ttIssueReturn.ToJobPlant = Session.PlantID;
            ttIssueReturn.SysRowID = Guid.NewGuid(); // Guid.Parse(ttIssueReturn.DummyKeyField);

            rMtlQueueRowID = pcMtlQueueRowID;
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && !rMtlQueueRowID.Equals(Guid.Empty))
            {
                this.processMtlQueue(rMtlQueueRowID, ttIssueReturn);
            }
            else
            {
                ttIssueReturn.FromJobNum = pcFromJobNum;
                this.onChangeFromJobNumCore(ttIssueReturn);
                ttIssueReturn.FromAssemblySeq = piFromAssemblySeq;
                this.onChangeFromAssemblySeqCore(ttIssueReturn);
            }
            sysTranID = this.getLegalNumberType(ttIssueReturn.TranType);
            ttIssueReturn.TranDocTypeID = this.LibGetAvailTranDocTypes.GetTranDocTypeIdDefault(sysTranID);
            var outEnableToFields2 = ttIssueReturn.EnableToFields;
            var outEnableFromFields2 = ttIssueReturn.EnableFromFields;
            this.disableFromToFields(pcTranType, out outEnableToFields2, out outEnableFromFields2);
            ttIssueReturn.EnableToFields = outEnableToFields2;
            ttIssueReturn.EnableFromFields = outEnableFromFields2;
            this.FillForeignFields(ttIssueReturn);
        }

        /// <summary>
        /// Call this method to create a new Epicor.Mfg.BO.IssueReturnDataSet with
        /// default values.
        /// </summary>
        /// <param name="pcToJobNum">To Job number.</param>
        /// <param name="piToAssemblySeq">To Assembly Seq.</param>
        /// <param name="pcTranType">Material movement type XXX-XXX e.g STK-MTL</param>
        /// <param name="pcMtlQueueRowID">Progress database rowid for MtlQueue record</param>
        /// <param name="pcMessage">Non-Error, informational message</param>
        /// <param name="ds">IssueReturnDataSet</param>
        public void GetNewIssueReturnToJob(string pcToJobNum, int piToAssemblySeq, string pcTranType, Guid pcMtlQueueRowID, out string pcMessage, ref IssueReturnTableset ds)
        {
            pcMessage = string.Empty;
            CurrentFullTableset = ds;
            Guid rMtlQueueRowID = Guid.Empty;
            string cMessage = string.Empty;
            string sysTranID = string.Empty;
            if (!this.isValidTranType(pcTranType))
            {
                throw new BLException(Strings.AValidTrantypeIsRequired, "IssueReturn", "TranType");
            }
            ttIssueReturn = new Erp.Tablesets.IssueReturnRow();
            CurrentFullTableset.IssueReturn.Add(ttIssueReturn);
            ttIssueReturn.DummyKeyField = Session.UserID + Compatibility.Convert.TimeToString(CompanyTime.Now()) + Compatibility.Convert.ToString((new Random()).Next(0, Erp.Internal.Lib.DateExtensions.SecondsSinceMidnight(CompanyTime.Now()))); /* Just trying to make a unique key */
            ttIssueReturn.Company = Session.CompanyID;
            ttIssueReturn.TranDate = this.LibOffSet.OffsetToday();
            ttIssueReturn.IssuedComplete = false;
            ttIssueReturn.MtlQueueRowId = Guid.Empty;
            ttIssueReturn.TranType = pcTranType;
            ttIssueReturn.Plant = Session.PlantID;
            ttIssueReturn.FromJobPlant = Session.PlantID;
            ttIssueReturn.ToJobPlant = Session.PlantID;
            ttIssueReturn.SysRowID = Guid.NewGuid(); // Guid.Parse(ttIssueReturn.DummyKeyField);

            rMtlQueueRowID = pcMtlQueueRowID;
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && !rMtlQueueRowID.Equals(Guid.Empty))
            {
                this.processMtlQueue(rMtlQueueRowID, ttIssueReturn);
            }
            else
            {
                ttIssueReturn.ToJobNum = pcToJobNum;
                this.onChangeToJobNumCore(ttIssueReturn, out pcMessage);
                ttIssueReturn.ToAssemblySeq = piToAssemblySeq;
                this.onChangeToAssemblySeqCore(ttIssueReturn);
            }
            sysTranID = this.getLegalNumberType(ttIssueReturn.TranType);
            ttIssueReturn.TranDocTypeID = this.LibGetAvailTranDocTypes.GetTranDocTypeIdDefault(sysTranID);
            var outEnableToFields3 = ttIssueReturn.EnableToFields;
            var outEnableFromFields3 = ttIssueReturn.EnableFromFields;
            this.disableFromToFields(pcTranType, out outEnableToFields3, out outEnableFromFields3);
            ttIssueReturn.EnableToFields = outEnableToFields3;
            ttIssueReturn.EnableFromFields = outEnableFromFields3;
            this.FillForeignFields(ttIssueReturn);
        }

        /// <summary>
        /// This method creates multiple IssueReturnJobs rows using IssueReturnJobSearch dataset.
        /// </summary>
        /// <param name="pcTranType">Material movement type XXX-XXX e.g STK-MTL</param>
        /// <param name="pcMtlQueueRowID">Progress database rowid for MtlQueue record</param>
        /// <param name="pCallProcess">Calling Process</param>
        /// <param name="ds">SelectedJobAsmblDataSet</param>
        /// <param name="pcMessage">Non-Error, informational message</param>
        /// <returns>IssueReturnDataSet</returns>
        public IssueReturnTableset GetNewJobAsmblMultiple(string pcTranType, Guid pcMtlQueueRowID, string pCallProcess, ref SelectedJobAsmblTableset ds, out string pcMessage)
        {
            pcMessage = string.Empty;
            ttSelectedJobAsmblTablesetDS = ds;

            string cDirection = string.Empty;
            string cMessage = string.Empty;
            Guid rMtlQueueRowID = Guid.Empty;
            int iAssemblySeq = 0;
            int icntr = 0;
            string sysTranID = string.Empty;
            cDirection = this.getJobDirection(pcTranType);
            if (StringExtensions.Compare(cDirection, "From") != 0 && StringExtensions.Compare(cDirection, "To") != 0)
            {
                throw new BLException(Strings.AValidTranTIsRequiCannotDeterTheDirecOfTheJobFrom, "IssueReturn", "TranType");
            }

            foreach (var ttSelectedJobAsmbl_iterator in (from ttSelectedJobAsmbl_Row in ds.SelectedJobAsmbl
                                                         where !String.IsNullOrEmpty(ttSelectedJobAsmbl_Row.RowMod)
                                                         select ttSelectedJobAsmbl_Row))
            {
                ttSelectedJobAsmbl = ttSelectedJobAsmbl_iterator;
                icntr = icntr + 1;/* SCR 30682 */
                ttIssueReturn = new Erp.Tablesets.IssueReturnRow();
                CurrentFullTableset.IssueReturn.Add(ttIssueReturn);
                ttIssueReturn.DummyKeyField = Session.UserID + Compatibility.Convert.ToString(icntr); /* Just trying to make a unique key */
                ttIssueReturn.Company = Session.CompanyID;
                ttIssueReturn.TranDate = this.LibOffSet.OffsetToday();
                ttIssueReturn.IssuedComplete = false;
                ttIssueReturn.MtlQueueRowId = Guid.Empty;
                ttIssueReturn.TranType = pcTranType;
                ttIssueReturn.Plant = Session.PlantID;
                ttIssueReturn.FromJobPlant = Session.PlantID;
                ttIssueReturn.ToJobPlant = Session.PlantID;
                ttIssueReturn.SysRowID = Guid.NewGuid(); //Guid.Parse(ttIssueReturn.DummyKeyField);
                sysTranID = this.getLegalNumberType(pcTranType);
                ttIssueReturn.TranDocTypeID = this.LibGetAvailTranDocTypes.GetTranDocTypeIdDefault(sysTranID);
                rMtlQueueRowID = pcMtlQueueRowID;

                /* if rMtlQueueRowID ne ? */
                if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && !rMtlQueueRowID.Equals(Guid.Empty))
                {
                    this.processMtlQueue(rMtlQueueRowID, ttIssueReturn);
                }
                else
                {/* if cDirection = "From" */
                    if (StringExtensions.Compare(cDirection, "From") == 0)
                    {
                        ttIssueReturn.FromJobNum = ttSelectedJobAsmbl.JobNum;
                        iAssemblySeq = ttSelectedJobAsmbl.AssemblySeq;
                        if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                        {
                            ttIssueReturn.ToJobNum = ttIssueReturn.FromJobNum;
                            ttIssueReturn.ToAssemblySeq = 0;
                        }
                        /* We are re-using onChangeFromJobNumCore logic.
                           OnChange logic defaults the AssemblySeq for the Job#.  So we will store the
                           user assigned AssemblySeq in iAssemblySeq and re-assign it after the OnChange call
                        */
                        this.onChangeFromJobNumCore(ttIssueReturn);
                        ttIssueReturn.FromAssemblySeq = iAssemblySeq;
                        this.onChangeFromAssemblySeqCore(ttIssueReturn);
                    }
                    else
                    {
                        ttIssueReturn.ToJobNum = ttSelectedJobAsmbl.JobNum;
                        iAssemblySeq = ttSelectedJobAsmbl.AssemblySeq;                /* We are re-using onChangeToJobNumCore logic.
                   OnChange logic defaults the AssemblySeq for the Job#.  So we will store the 
                   user assigned AssemblySeq in iAssemblySeq and re-assign it after the OnChange call
                */
                        this.onChangeToJobNumCore(ttIssueReturn, out pcMessage);
                        ttIssueReturn.ToAssemblySeq = iAssemblySeq;
                        this.onChangeToAssemblySeqCore(ttIssueReturn);
                        if (!String.IsNullOrEmpty(cMessage))
                        {
                            pcMessage = pcMessage + cMessage;
                        }
                    }
                }/* else - if rMtlQueueRowID ne ? */

                /* ERPS-106804 - Special consideration for WIP-WIP coming from MtlQueue. Assign the correct ToAssemblySeq/ToJobSeq  *
                 * instead of using the values assigned from processMtlQueue temporarily.                                           */
                if (ttIssueReturn.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase) && ttIssueReturn.MtlQueueRowId != Guid.Empty)
                {
                    vSaveToAssemblySeq = ttIssueReturn.ToAssemblySeq;
                    vSaveToJobSeq = ttIssueReturn.ToJobSeq;
                    ttIssueReturn.ToJobNum = ttIssueReturn.FromJobNum;
                    ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
                    ttIssueReturn.ToJobSeq = ttIssueReturn.FromJobSeq;
                }
                else if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                {
                    ttIssueReturn.ToJobNum = ttIssueReturn.FromJobNum;
                    ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
                }
                var outEnableToFields4 = ttIssueReturn.EnableToFields;
                var outEnableFromFields4 = ttIssueReturn.EnableFromFields;
                this.disableFromToFields(pcTranType, out outEnableToFields4, out outEnableFromFields4);
                ttIssueReturn.EnableToFields = outEnableToFields4;
                ttIssueReturn.EnableFromFields = outEnableFromFields4;
                FillForeignFields(ttIssueReturn);
                this.checkStatusTracking();
                this.enableSNButton(ttIssueReturn, pCallProcess);
                if (ttIssueReturn.EnableSN)
                {
                    switch (pCallProcess.ToUpperInvariant())
                    {
                        case "HHISSUEMATERIAL":
                        case "ISSUEMATERIAL":
                            {
                                JobMtl = FindFirstJobMtl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq);
                                ttIssueReturn.ReassignSNAsm = (JobMtl != null) ? JobMtl.ReassignSNAsm : false;
                            }
                            break;
                        case "HHISSUEASSEMBLY":
                        case "ISSUEASSEMBLY":
                            {
                                JobAsmbl = FindFirstJobAsmbl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq);
                                ttIssueReturn.ReassignSNAsm = (JobAsmbl != null) ? JobAsmbl.ReassignSNAsm : false;
                            }
                            break;
                    }
                }

                /* ERPS-106804 - Special consideration for WIP-WIP coming from MtlQueue. */
                if (ttIssueReturn.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase) && ttIssueReturn.MtlQueueRowId != Guid.Empty)
                {
                    ttIssueReturn.ToAssemblySeq = vSaveToAssemblySeq;
                    ttIssueReturn.ToJobSeq = vSaveToJobSeq;
                }
            }

            return CurrentFullTableset;
        }

        /// <summary>
        /// This method creates a new ttSelectedJobAsmbl row entry.
        /// </summary>
        /// <param name="ds">SelectedJobAsmblDataSet</param>
        public void GetNewJobAsmblSearch(ref SelectedJobAsmblTableset ds)
        {
            ttSelectedJobAsmblTablesetDS = ds;
            ttSelectedJobAsmbl = new Erp.Tablesets.SelectedJobAsmblRow();
            ttSelectedJobAsmblTablesetDS.SelectedJobAsmbl.Add(ttSelectedJobAsmbl);
            ttSelectedJobAsmbl.Company = Session.CompanyID;
            ttSelectedJobAsmbl.RowMod = IceRow.ROWSTATE_ADDED;
            ttSelectedJobAsmbl.SysRowID = Guid.NewGuid();
        }

        /// <summary>
        /// This method creates multiple IssueReturnJobs rows using IssueReturnJobSearch dataset.
        /// </summary>
        /// <param name="pcTranType">Material movement type XXX-XXX e.g STK-MTL</param>
        /// <param name="pcMtlQueueRowID">Progress database rowid for MtlQueue record</param>
        /// <param name="ds">SelectedPartDataSet</param>
        /// <param name="pcMessage">Non-Error, informational message</param>
        /// <returns>IssueReturnDataSet</returns>
        public IssueReturnTableset GetNewPartMultiple(string pcTranType, Guid pcMtlQueueRowID, ref SelectedPartTableset ds, out string pcMessage)
        {
            pcMessage = string.Empty;
            //ttSelectedPartsTablesetDS = ds;
            string cCallProcess = string.Empty;
            if (StringExtensions.Compare(pcTranType, "STK-UKN") == 0)
            {
                cCallProcess = "IssueMiscellaneousMaterial";
            }

            if (StringExtensions.Compare(pcTranType, "UKN-STK") == 0)
            {
                cCallProcess = "ReturnMiscellaneousMaterial";
            }

            x = 1;

            foreach (var ttSelectedParts_iterator in (from ttSelectedParts_Row in ds.SelectedParts
                                                      where !String.IsNullOrEmpty(ttSelectedParts_Row.RowMod)
                                                      select ttSelectedParts_Row))
            {
                ttIssueReturn = new IssueReturnRow();
                ttSelectedParts = ttSelectedParts_iterator;
                this.getNewPartNumCore(ttSelectedParts.PartNum, pcTranType, pcMtlQueueRowID, ref ttIssueReturn);
                ttIssueReturn.RowMod = IceRow.ROWSTATE_ADDED;

                this.enableSNButton(ttIssueReturn, cCallProcess);
            }
            return CurrentFullTableset;
        }

        /// <summary>
        /// Call this method to create a new Epicor.Mfg.BO.IssueReturnDataSet with
        /// Part#.
        /// </summary>
        /// <param name="pcPartNum">Part number</param>
        /// <param name="pcTranType">Material movement type XXX-XXX e.g STK-UKN</param>
        /// <param name="pcMtlQueueRowID">Progress database rowid for MtlQueue record</param>
        /// <param name="ds">IssueReturnDataSet</param>
        public void GetNewPartNum(string pcPartNum, string pcTranType, Guid pcMtlQueueRowID, ref IssueReturnTableset ds)
        {
            string cCallProcess = string.Empty;
            CurrentFullTableset = ds;
            ttIssueReturn = new IssueReturnRow();
            this.getNewPartNumCore(pcPartNum, pcTranType, pcMtlQueueRowID, ref ttIssueReturn);
            ttIssueReturn.RowMod = IceRow.ROWSTATE_ADDED;
            if (StringExtensions.Compare(pcTranType, "STK-UKN") == 0)
            {
                cCallProcess = "IssueMiscellaneousMaterial";
            }

            if (StringExtensions.Compare(pcTranType, "UKN-STK") == 0)
            {
                cCallProcess = "ReturnMiscellaneousMaterial";
            }

            this.enableSNButton(ttIssueReturn, cCallProcess);
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void getNewPartNumCore(string pcPartNum, string pcTranType, Guid pcMtlQueueRowID, ref IssueReturnRow ttIssueReturn)
        {
            string sysTranID = string.Empty;
            x = x + 1;
            sysTranID = this.getLegalNumberType(pcTranType);
            ttIssueReturn = new Erp.Tablesets.IssueReturnRow();
            CurrentFullTableset.IssueReturn.Add(ttIssueReturn);
            ttIssueReturn.DummyKeyField = Compatibility.Convert.TimeToString(CompanyTime.Now()) + "-" + Compatibility.Convert.ToString(x);  /* scr #28357*/
            ttIssueReturn.Company = Session.CompanyID;
            ttIssueReturn.TranDate = this.LibOffSet.OffsetToday();
            ttIssueReturn.PartNum = pcPartNum;
            ttIssueReturn.TreeDisplay = ttIssueReturn.PartNum;
            ttIssueReturn.IssuedComplete = false;
            ttIssueReturn.MtlQueueRowId = pcMtlQueueRowID;
            ttIssueReturn.TranType = pcTranType;
            ttIssueReturn.Plant = Session.PlantID;
            ttIssueReturn.FromJobPlant = Session.PlantID;
            ttIssueReturn.ToJobPlant = Session.PlantID;
            ttIssueReturn.SysRowID = Guid.NewGuid(); //Guid.Parse(ttIssueReturn.DummyKeyField);
            ttIssueReturn.TranDocTypeID = this.LibGetAvailTranDocTypes.GetTranDocTypeIdDefault(sysTranID);
            this.LibValidateTranDocType.RunValidateTranDocType(ttIssueReturn.TranDocTypeID, sysTranID);

            /* Validation: PartNum */
            this.validatePartNum(ttIssueReturn);
            ExceptionManager.AssertNoBLExceptions();

            this.onChangePartNumCore(ttIssueReturn);
            this.FillForeignFields(ttIssueReturn);
        }

        /// <summary>
        /// This method creates a new ttSelectedParts row entry.
        /// </summary>
        /// <param name="ds">SelectedPartDataSet</param>
        public void GetNewPartSearch(ref SelectedPartTableset ds)
        {
            //ttSelectedPartsTablesetDS = ds;
            ttSelectedParts = new Erp.Tablesets.SelectedPartsRow();
            ttSelectedPartTablesetDS.SelectedParts.Add(ttSelectedParts);
            ttSelectedParts.Company = Session.CompanyID;
            ttSelectedParts.RowMod = IceRow.ROWSTATE_ADDED;
            ttSelectedParts.SysRowID = Guid.NewGuid();
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void getPartIssuedQty(IssueReturnRow ttIssueReturn, out decimal pPartIssuedQty)
        {
            pPartIssuedQty = decimal.Zero;
            Erp.Tables.PartTran bPartTran = null;

            foreach (var bPartTran_iterator in (this.SelectPartTran(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, "M", ttIssueReturn.FromJobSeq, ttIssueReturn.PartNum)))
            {
                bPartTran = bPartTran_iterator;
                if (StringExtensions.Lookup(SkipList, bPartTran.TranType) > -1)
                {
                    continue;
                }

                if (StringExtensions.Lookup(SubtractList, bPartTran.TranType) == -1)
                {
                    pPartIssuedQty = pPartIssuedQty + bPartTran.TranQty;
                }
                else
                {
                    pPartIssuedQty = pPartIssuedQty - bPartTran.TranQty;
                }
            }
        }

        ///<summary>
        ///  Purpose:  Calculate the appropriate costs for the returned material. If the material is FIFO/LOTFIFO costed
        ///            then the costs will come from the actual FIFO layers issued for this job material. If non-FIFO
        ///            then costs will be the weighted average costs of all issued/returned transactions.
        ///  parameters:  none
        ///  Notes:
        ///</summary>
        private void getReturnMaterialCosts(IssueReturnRow ttIssueReturn, ref PartTran PartTran, ref List<Erp.Internal.Lib.ProcessFIFO.ReturnMaterialCosts> ttReturnMaterialCostsRows, bool ipEnableFIFOLayers)
        {
            /* SCR 170491 - If returning non-FIFO parts, the costs of the return will be based on the weighted average costs *
             * of all previous issues/returns made against the job material/assembly. This is the same logic for calculating *
             * alternate return costs (secondary cost method is FIFO if Enable FIFO Layer setting is true).                  *
             * If returning FIFO/LOTFIFO parts, the costs will come from the actual FIFO layers issued to the job material,  *
             * starting from the very last FIFO layers consumed. New FIFO Layer will be added back for the same FIFODate and *
             * FIFOSeq of the original layer but with FIFOSubSeq incremented.                                                */

            /* FIFO/LOTFIFO cost methods - this will also calculate Average Costs if needed */
            if (StringExtensions.Lookup("F,O", PartTran.CostMethod) > -1)
            {
                LibProcessFIFO.GetReturnedFIFOLinked(ref PartTran, ref ttReturnMaterialCostsRows);
            }
            else  /* not FIFO/LOTFIFO */
            {
                /* non-FIFO cost methods - will use weighted average costs. */
                /* SCR 170491 - Combined the logic that calculates the main average costs and the alternate costs. *
                 * The calculated costs will be written to the PartTran directly instead of passing them back.     */
                this.getPartTranTotalCosts(ttIssueReturn, ref PartTran, ref ttReturnMaterialCostsRows, vEnableFIFOLayers);

                /* SCR 201263 - If doing return assembly, override the weighted average costs from the getPartTranTotalCosts with PartCost */
                if ((PartTran.TranType.Equals("STK-ASM", StringComparison.OrdinalIgnoreCase)) ||
                    (PartTran.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase) && PartTran.JobSeq == 0))
                {
                    /* Get cost from Part - procedure found in lib/InvCosts.i */
                    var outMtlUnitCost = PartTran.MtlUnitCost;
                    var outLbrUnitCost = PartTran.LbrUnitCost;
                    var outBurUnitCost = PartTran.BurUnitCost;
                    var outSubUnitCost = PartTran.SubUnitCost;
                    var outMtlBurUnitCost = PartTran.MtlBurUnitCost;
                    this.LibInvCosts.getInvCost(ttIssueReturn.LotNum, ((!String.IsNullOrEmpty(PartTran.Plant)) ? PartTran.Plant : Session.PlantID), PartTran.PartNum, PartTran.CostMethod, out outMtlUnitCost, out outLbrUnitCost, out outBurUnitCost, out outSubUnitCost, out outMtlBurUnitCost, ref Part);
                    PartTran.MtlUnitCost = outMtlUnitCost;
                    PartTran.LbrUnitCost = outLbrUnitCost;
                    PartTran.BurUnitCost = outBurUnitCost;
                    PartTran.SubUnitCost = outSubUnitCost;
                    PartTran.MtlBurUnitCost = outMtlBurUnitCost;
                    PartTran.MtlMtlUnitCost = PartTran.MtlUnitCost;
                }
            }  /* not FIFO/LOTFIFO */
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void getPartTranTotalCosts(IssueReturnRow ttIssueReturn, ref PartTran mainPartTran, ref List<Erp.Internal.Lib.ProcessFIFO.ReturnMaterialCosts> ttReturnMaterialCostsRows, bool ipEnableFIFOLayers)
        {
            decimal TotalMtlCost = decimal.Zero;
            decimal TotalLbrCost = decimal.Zero;
            decimal TotalBurCost = decimal.Zero;
            decimal TotalSubCost = decimal.Zero;
            decimal TotalMtlBurCost = decimal.Zero;
            decimal TotalTranQty = decimal.Zero;
            decimal AltTotalMtlCost = decimal.Zero;
            decimal AltTotalLbrCost = decimal.Zero;
            decimal AltTotalBurCost = decimal.Zero;
            decimal AltTotalSubCost = decimal.Zero;
            decimal AltTotalMtlBurCost = decimal.Zero;
            decimal AltTranMtlCost = decimal.Zero;
            decimal AltTranLbrCost = decimal.Zero;
            decimal AltTranBurCost = decimal.Zero;
            decimal AltTranSubCost = decimal.Zero;
            decimal AltTranMtlBurCost = decimal.Zero;
            decimal vTranQty;
            DateTime? pFIFODate = null;
            int pFIFOSeq = 0;
            int ndec = 0;
            Erp.Tables.PartTran bPartTran = null;

            ndec = this.LibGetDecimalsNumber.getDecimalsNumberByName("PartTran", "MtlUnitCost", "");

            /* SCR #19110 - when trying to calculate outstanding transaction qty, make sure *
             * that the current PartTran is not included in the for each.  In Vantage 6.1,  *
             * the original logic does not include the line that qualifies the Rowid since  *
             * in 6.1 version, when this logic is called the new PartTran is not committed  *
             * yet in the database.  In Sonoma, when this logic is called the new PartTran  *
             * is already committed in the DB and thus causing the tranqty total to be off. */

            //bPartTran_Loop:
            foreach (var bPartTran_iterator in (this.SelectPartTran2(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, "M", ttIssueReturn.FromJobSeq, ttIssueReturn.PartNum, mainPartTran.SysRowID)))
            {
                bPartTran = bPartTran_iterator;
                if (StringExtensions.Lookup(SkipList, bPartTran.TranType) > -1)
                {
                    continue;
                }

                /* alternate costs if secondary FIFO cost is needed */
                if (ipEnableFIFOLayers == true)
                {
                    AltTranMtlCost = bPartTran.AltMtlUnitCost;
                    AltTranLbrCost = bPartTran.AltLbrUnitCost;
                    AltTranBurCost = bPartTran.AltBurUnitCost;
                    AltTranSubCost = bPartTran.AltSubUnitCost;
                    AltTranMtlBurCost = bPartTran.AltMtlBurUnitCost;
                    /* In case the alternate costs are not populated then use the regular costs */
                    if (AltTranMtlCost == 0 && AltTranLbrCost == 0 && AltTranBurCost == 0 && AltTranSubCost == 0 && AltTranMtlBurCost == 0)
                    {
                        AltTranMtlCost = bPartTran.MtlUnitCost;
                        AltTranLbrCost = bPartTran.LbrUnitCost;
                        AltTranBurCost = bPartTran.BurUnitCost;
                        AltTranSubCost = bPartTran.SubUnitCost;
                        AltTranMtlBurCost = bPartTran.MtlBurUnitCost;
                    }
                }

                /* IF LOOKUP(bPartTran.TranType,SubtractList) = 0 */
                if (StringExtensions.Lookup(SubtractList, bPartTran.TranType) == -1) /* NOT in the SubtractList */
                {
                    if (pFIFODate == null)
                    {
                        pFIFODate = bPartTran.FIFODate;
                        pFIFOSeq = bPartTran.FIFOSeq;
                    }
                    TotalTranQty = TotalTranQty + bPartTran.TranQty;
                    vTranQty = bPartTran.TranType.Equals("ADJ-PUR", StringComparison.OrdinalIgnoreCase) && bPartTran.TranQty == 0 ? 1 : bPartTran.TranQty;
                    TotalMtlCost = Math.Round(TotalMtlCost + (bPartTran.MtlUnitCost * vTranQty), ndec, MidpointRounding.AwayFromZero);
                    TotalLbrCost = Math.Round(TotalLbrCost + (bPartTran.LbrUnitCost * vTranQty), ndec, MidpointRounding.AwayFromZero);
                    TotalBurCost = Math.Round(TotalBurCost + (bPartTran.BurUnitCost * vTranQty), ndec, MidpointRounding.AwayFromZero);
                    TotalSubCost = Math.Round(TotalSubCost + (bPartTran.SubUnitCost * vTranQty), ndec, MidpointRounding.AwayFromZero);
                    TotalMtlBurCost = Math.Round(TotalMtlBurCost + (bPartTran.MtlBurUnitCost * vTranQty), ndec, MidpointRounding.AwayFromZero);
                    /* alternate costs for secondary FIFO */
                    if (ipEnableFIFOLayers == true)
                    {
                        AltTotalMtlCost = Math.Round(AltTotalMtlCost + (AltTranMtlCost * vTranQty), ndec, MidpointRounding.AwayFromZero);
                        AltTotalLbrCost = Math.Round(AltTotalLbrCost + (AltTranLbrCost * vTranQty), ndec, MidpointRounding.AwayFromZero);
                        AltTotalBurCost = Math.Round(AltTotalBurCost + (AltTranBurCost * vTranQty), ndec, MidpointRounding.AwayFromZero);
                        AltTotalSubCost = Math.Round(AltTotalSubCost + (AltTranSubCost * vTranQty), ndec, MidpointRounding.AwayFromZero);
                        AltTotalMtlBurCost = Math.Round(AltTotalMtlBurCost + (AltTranMtlBurCost * vTranQty), ndec, MidpointRounding.AwayFromZero);
                    }
                }
                else
                {
                    TotalTranQty = TotalTranQty - bPartTran.TranQty;
                    TotalMtlCost = Math.Round(TotalMtlCost - (bPartTran.MtlUnitCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                    TotalLbrCost = Math.Round(TotalLbrCost - (bPartTran.LbrUnitCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                    TotalBurCost = Math.Round(TotalBurCost - (bPartTran.BurUnitCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                    TotalSubCost = Math.Round(TotalSubCost - (bPartTran.SubUnitCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                    TotalMtlBurCost = Math.Round(TotalMtlBurCost - (bPartTran.MtlBurUnitCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                    /* alternate costs for secondary FIFO */
                    if (ipEnableFIFOLayers == true)
                    {
                        AltTotalMtlCost = Math.Round(AltTotalMtlCost - (AltTranMtlCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                        AltTotalLbrCost = Math.Round(AltTotalLbrCost - (AltTranLbrCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                        AltTotalBurCost = Math.Round(AltTotalBurCost - (AltTranBurCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                        AltTotalSubCost = Math.Round(AltTotalSubCost - (AltTranSubCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                        AltTotalMtlBurCost = Math.Round(AltTotalMtlBurCost - (AltTranMtlBurCost * bPartTran.TranQty), ndec, MidpointRounding.AwayFromZero);
                    }
                }
            }  /* foreach bPartTran SelectPartTran2 */

            /* write the average costs directly to the PartTran */
            if (mainPartTran != null)
            {
                /* non-FIFO average costs */
                mainPartTran.MtlUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(Internal.Lib.MathExtensions.SafeDivision(TotalMtlCost, TotalTranQty), "", "PartTran", "MtlUnitCost");
                mainPartTran.LbrUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(Internal.Lib.MathExtensions.SafeDivision(TotalLbrCost, TotalTranQty), "", "PartTran", "LbrunitCost");
                mainPartTran.BurUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(Internal.Lib.MathExtensions.SafeDivision(TotalBurCost, TotalTranQty), "", "PartTran", "BurunitCost");
                mainPartTran.SubUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(Internal.Lib.MathExtensions.SafeDivision(TotalSubCost, TotalTranQty), "", "PartTran", "SubUnitCost");
                mainPartTran.MtlBurUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(Internal.Lib.MathExtensions.SafeDivision(TotalMtlBurCost, TotalTranQty), "", "PartTran", "MtlBurunitCost");
                mainPartTran.MtlMtlUnitCost = mainPartTran.MtlUnitCost;
                /* alternate costs for secondary FIFO */
                if (ipEnableFIFOLayers == true)
                {
                    mainPartTran.FIFODate = pFIFODate;
                    mainPartTran.FIFOSeq = pFIFOSeq;
                    mainPartTran.AltMtlUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(AltTotalMtlCost / TotalTranQty, "", "PartTran", "MtlUnitCost");
                    mainPartTran.AltLbrUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(AltTotalLbrCost / TotalTranQty, "", "PartTran", "LbrunitCost");
                    mainPartTran.AltBurUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(AltTotalBurCost / TotalTranQty, "", "PartTran", "BurunitCost");
                    mainPartTran.AltSubUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(AltTotalSubCost / TotalTranQty, "", "PartTran", "SubUnitCost");
                    mainPartTran.AltMtlBurUnitCost = this.LibRoundAmountEF.RoundDecimalsApply(AltTotalMtlBurCost / TotalTranQty, "", "PartTran", "MtlBurunitCost");
                    mainPartTran.AltMtlMtlUnitCost = mainPartTran.AltMtlUnitCost;
                }
            }
        }

        ///<summary>
        ///  Purpose: Get the JobOper.OprSeq of the JobOper related to a JobMtl or JobAsmbl.
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void getRelatedOperation(string pcTranType, string pcJobNum, ref int piAssemblySeq, ref int piJobSeq)
        {
            int _piJobSeq = piJobSeq;
            piJobSeq = 0;
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement))
            {
                return;
            }

            if (StringExtensions.Compare(pcTranType, "STK-ASM") != 0 && StringExtensions.Compare(pcTranType, "ASM-STK") != 0)
            {
                JobMtl = this.FindFirstJobMtl(Session.CompanyID, pcJobNum, piAssemblySeq, _piJobSeq);
                if (JobMtl != null)
                {
                    if (JobOper != null)
                    {
                        Db.Release(ref JobOper);/* Just to make sure the buffer is clean */
                    }

                    if (JobMtl.RelatedOperation > 0)
                    {
                        JobOper = this.FindFirstJobOper7(Session.CompanyID, JobMtl.JobNum, JobMtl.AssemblySeq, JobMtl.RelatedOperation);
                    }
                    if (JobOper == null)
                    {
                        JobOper = this.FindFirstJobOper8(JobMtl.Company, JobMtl.JobNum, JobMtl.AssemblySeq, 0);
                    }
                    if (JobOper != null)
                    {
                        piJobSeq = JobOper.OprSeq;
                    }
                }
            }
            if (StringExtensions.Compare(pcTranType, "STK-ASM") == 0 || StringExtensions.Compare(pcTranType, "ASM-STK") == 0)
            {
                JobAsmbl = this.FindFirstJobAsmbl(Session.CompanyID, pcJobNum, piAssemblySeq);
                if (JobAsmbl != null)
                {
                    if (JobOper != null)
                    {
                        Db.Release(ref JobOper);/* Just to make sure the buffer is clean */
                    }

                    if (JobAsmbl.RelatedOperation > 0)
                    {
                        JobOper = this.FindFirstJobOper9(Session.CompanyID, JobAsmbl.JobNum, JobAsmbl.Parent, JobAsmbl.RelatedOperation);
                    }
                    if (JobOper == null)
                    {
                        JobOper = this.FindFirstJobOper10(Session.CompanyID, JobAsmbl.JobNum, JobAsmbl.Parent, 0);
                    }
                    if (JobOper != null)
                    {
                        piAssemblySeq = JobOper.AssemblySeq;
                        piJobSeq = JobOper.OprSeq;
                    }
                }
            }
        }

        /// <summary>
        /// List of jobs that can be selected for Mass Issue.
        /// </summary>
        /// <param name="whereClauseJobHead">Where condition without the where word</param>
        /// <param name="whereClauseJobAsmbl">Where condition without the where word</param>
        /// <returns>Returns Epicor.Mfg.BO.IssueReturnJobAsmblDataSet</returns>
        /// <param name="pageSize"># of records returned.  0 means all</param>
        /// <param name="absolutePage"></param>
        /// <param name="morePages">Are there more pages ? Yes/No</param>
        [Ice.Hosting.Http.HttpGet]
        public IssueReturnJobAsmblTableset GetRows(string whereClauseJobHead, string whereClauseJobAsmbl, int pageSize, int absolutePage, out bool morePages)
        {
            morePages = false;
            IssueReturnJobAsmblTableset ttIssueReturnJobAsmblTablesetDS = new IssueReturnJobAsmblTableset();
            ttIssueReturnJobAsmblTablesetDS = this.getListCore(whereClauseJobHead, whereClauseJobAsmbl, pageSize, absolutePage, out morePages);
            return ttIssueReturnJobAsmblTablesetDS;
        }

        /// <summary>
        /// List of jobs/assemblies that can be selected for Mass Issue.
        /// </summary>
        /// <param name="whereClause">Where condition without the where word</param>
        /// <returns>Returns Epicor.Mfg.BO.IssueReturnJobAsmblDataSet</returns>
        /// <param name="pageSize"># of records returned.  0 means all</param>
        /// <param name="absolutePage"></param>
        /// <param name="morePages">Are there more pages ? Yes/No</param>
        [Ice.Hosting.Http.HttpGet]
        public IssueReturnJobAsmblTableset GetRowsJobAssemblies(string whereClause, int pageSize, int absolutePage, out bool morePages)
        {
            morePages = true;
            CheckSQLInjection.ValidateWhereClause(whereClause);

            string workOrderBy = "JobHead.Company,";

            if ((whereClause.IndexOf("PartNum", StringComparison.InvariantCultureIgnoreCase) >= 0))
                workOrderBy += workOrderBy + "JobHead.PartNum,";

            if ((whereClause.IndexOf("EquipID", StringComparison.InvariantCultureIgnoreCase) >= 0))
                workOrderBy += workOrderBy + "JobHead.EquipID,";

            if ((whereClause.IndexOf("JobAsmbl.Description", StringComparison.InvariantCultureIgnoreCase) >= 0))
            {
                workOrderBy += "JobAsmbl.Description";
            }
            else if ((whereClause.IndexOf("JobAsmbl.AssemblySeq=JobAsmbl.AssemblySeq", StringComparison.InvariantCultureIgnoreCase) >= 0))
            {
                workOrderBy += " JobAsmbl.Company, JobAsmbl.AssemblySeq, JobAsmbl.JobNum";
            }
            else
            {
                workOrderBy += " JobAsmbl.Company, JobAsmbl.JobNum, JobAsmbl.AssemblySeq";
            }

            return ExecuteQuery(whereClause, workOrderBy, pageSize, absolutePage, out morePages);
        }

        //Creates and executes sql query for GetRowsJobAssemblies and GetRowsWIP
        private IssueReturnJobAsmblTableset ExecuteQuery(string whereClause, string workOrderBy, int pageSize, int absolutePage, out bool morePages)
        {
            IssueReturnJobAsmblTableset ttIssueReturnJobAsmblTablesetDS = new IssueReturnJobAsmblTableset();
            morePages = true;

            int startRow = 0;
            int endRow = 0;

            startRow = pageSize * (absolutePage - 1) + 1;

            if (pageSize == 0)
            {
                endRow = 2147483647;
            }
            else
            {
                endRow = startRow + pageSize - 1;
            }

            string selectFields = @"JobHead.JobType,JobHead.CallNum,JobHead.JobNum,JobHead.PartNum,JobHead.PartDescription,JobAsmbl.Company,JobAsmbl.AssemblySeq,JobAsmbl.Description,JobAsmbl.SysRowID ";
            string fromString = @"from Erp.JobAsmbl join Erp.JobHead on JobHead.Company = JobAsmbl.Company and JobHead.JobNum = JobAsmbl.JobNum";

            string sqlWhereClause =
                @"where JobAsmbl.Company = '" + Session.CompanyID + @"' and JobAsmbl.Plant = '" + Session.PlantID +
                @"' and JobHead.Plant = '" + Session.PlantID + "' and JobHead.JobClosed = 0 and " +
                @"(exists(select * from Erp.JobMtl where JobMtl.Company = JobAsmbl.Company and JobMtl.JobNum = JobAsmbl.JobNum) or" +
                @" exists(select * from Erp.JobAsmbl as subAsmbl where subAsmbl.Company = JobAsmbl.Company and subAsmbl.JobNum = JobAsmbl.JobNum and subAsmbl.AssemblySeq > 0))";

            if (!String.IsNullOrEmpty(whereClause))
                sqlWhereClause = sqlWhereClause + " and " + whereClause;

            if ((whereClause.IndexOf("'SRV'", StringComparison.InvariantCultureIgnoreCase) >= 0))
            {
                sqlWhereClause += @" and exists(select * from Erp.FSCallhd where FSCallhd.Company = JobHead.Company and FSCallhd.CallNum = JobHead.CallNum and FSCallhd.Invoiced = 0)";
            }

            string orderBy = @"order by " + workOrderBy;

            string sqlQuery =
                @"select * from ( select ROW_NUMBER() over ( " + orderBy + @" ) as RowNumber, " + selectFields +
                " " + fromString + " " + sqlWhereClause +
                @") as result where result.RowNumber between " + startRow.ToString() + @" and " + endRow.ToString();

            using (SafeCommand sqlCommand = new SafeCommand(Db.SqlConnection))
            {
                sqlCommand.SafeCommandText = sqlQuery;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                Epicor.ServiceModel.Tableset.DBReaderAdapter.LoadFromReader(reader, (IIceTable)ttIssueReturnJobAsmblTablesetDS.IssueReturnJobAsmbl);
            }

            return ttIssueReturnJobAsmblTablesetDS;
        }


        /// <summary>
        /// List of jobs/assemblies that can be selected for Move WIP and Move Material.
        /// </summary>
        /// <param name="whereClause">Where condition without the where word</param>
        /// <returns>Returns Epicor.Mfg.BO.IssueReturnJobAsmblDataSet</returns>
        /// <param name="pageSize"># of records returned.</param>
        /// <param name="absolutePage"></param>
        /// <param name="morePages">Are there more pages ? Yes/No</param>
        [Ice.Hosting.Http.HttpGet]
        public IssueReturnJobAsmblTableset GetRowsWIP(string whereClause, int pageSize, int absolutePage, out bool morePages)
        {
            // Separate the where clause in tokens.
            List<string> tokens;
            tokens = SqlTokenizer.Parse(whereClause);

            bool firstTime = true;
            var sb = new StringBuilder();

            //Append the tokens back together and add the N prefix to the 'string' tokens.
            foreach (var token in tokens)
            {
                string item = Ice.Manager.Data.CheckUnicodeLiteral(token);

                if (firstTime)
                {
                    firstTime = false;
                    sb.Append(item);
                }
                else
                {
                    sb.Append(" ");
                    sb.Append(item);
                }
            }

            whereClause = sb.ToString();

            morePages = true;
            CheckSQLInjection.ValidateWhereClause(whereClause);

            string sortby = Ice.Manager.Data.ParseSort(ref whereClause);
            string workOrderBy = "";

            if (!String.IsNullOrEmpty(whereClause))
            {
                if ((whereClause.IndexOf("JobNum", StringComparison.InvariantCultureIgnoreCase) >= 0))
                    whereClause = whereClause.Replace("JobNum", "JobHead.JobNum");

                if ((whereClause.IndexOf("PartNum", StringComparison.InvariantCultureIgnoreCase) >= 0))
                    whereClause = whereClause.Replace("PartNum", "JobHead.PartNum");

                if ((whereClause.IndexOf("EquipID", StringComparison.InvariantCultureIgnoreCase) >= 0))
                    whereClause = whereClause.Replace("EquipID", "JobHead.EquipID");

                if ((whereClause.IndexOf("Description", StringComparison.InvariantCultureIgnoreCase) >= 0))
                    whereClause = whereClause.Replace("Description", "JobAsmbl.Description");

                if ((whereClause.IndexOf("AssemblySeq", StringComparison.InvariantCultureIgnoreCase) >= 0))
                    whereClause = whereClause.Replace("AssemblySeq", "JobAsmbl.AssemblySeq");
            }

            if (String.IsNullOrEmpty(sortby))
            {
                workOrderBy = "JobHead.Company, JobAsmbl.Company, JobAsmbl.JobNum, JobAsmbl.AssemblySeq";
            }
            else
            {
                sortby = sortby.SubString(3, sortby.Length);
                if ((sortby.IndexOf("JobNum", StringComparison.InvariantCultureIgnoreCase) >= 0) ||
                    (sortby.IndexOf("PartNum", StringComparison.InvariantCultureIgnoreCase) >= 0) ||
                    (sortby.IndexOf("EquipID", StringComparison.InvariantCultureIgnoreCase) >= 0))

                {
                    workOrderBy = "JobHead." + sortby;
                }
                else if ((sortby.IndexOf("Description", StringComparison.InvariantCultureIgnoreCase) >= 0) ||
                         (sortby.IndexOf("AssemblySeq", StringComparison.InvariantCultureIgnoreCase) >= 0))
                {
                    workOrderBy = "JobAsmbl." + sortby;
                }
            }

            return ExecuteQuery(whereClause, workOrderBy, pageSize, absolutePage, out morePages);
        }

        /// <summary>
        /// Gets parameters required for launching Select Serial Numbers
        /// </summary>
        /// <param name="ds">Issue Return data set</param>
        /// <returns>The SelectSerialNumbersParams dataset</returns>
        public SelectSerialNumbersParamsTableset GetSelectSerialNumbersParams(ref IssueReturnTableset ds)
        {
            SelectSerialNumbersParamsTableset ttSelectSerialNumbersParamsTablesetDS = new SelectSerialNumbersParamsTableset();
            CurrentFullTableset = ds;
            string snWhere = string.Empty;
            string validStatus = string.Empty;
            string statusClause = string.Empty;
            bool binToBin = false;
            string revisionNum = string.Empty;
            string attributeSetShortDescription = string.Empty;


            ttIssueReturn = (from ttIssueReturn_Row in CurrentFullTableset.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn == null)
            {
                throw new BLException(Strings.IssueReturnRecordNotFound);
            }
            if (ttIssueReturn.EnableSN == false)
            {
                throw new BLException(Strings.SerialNumbersAreNotRequiForThisTrans);
            }
            using (var InventoryTrackingSerialNumbers = new Internal.Lib.InventoryTrackingSerialNumbers(Db))
            {
                InventoryTrackingSerialNumbers.GetSerialNumbersInventoryTrackingData(ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, out revisionNum, out attributeSetShortDescription);
            }

            Part = this.FindFirstPart(ttIssueReturn.Company, ttIssueReturn.PartNum);
            if (Part == null)
            {
                throw new BLException(Strings.PartRecordNotFound);
            }
            decimal convQty = decimal.Zero;
            string errMsg = string.Empty;
            this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, Part.IUM, out convQty, false);
            errMsg = Strings.TheTotalQuantMustBeAWholeNumber(convQty, Part.IUM);
            if (Math.Truncate(convQty) != convQty)
            {
                throw new BLException(errMsg);
            }

            PlantConfCtrl = this.FindFirstPlantConfCtrl2(ttIssueReturn.Company, ttIssueReturn.FromJobPlant);
            if (PlantConfCtrl != null)
            {
                binToBin = PlantConfCtrl.BinToBinReqSN;
            }
            /* selected serial number params will vary depending on the UI form being used
and if it is the from plant or the to plant that is controlling the serial tracking
parameters */
            ttSelectSerialNumbersParams = new Erp.Tablesets.SelectSerialNumbersParamsRow();
            ttSelectSerialNumbersParamsTablesetDS.SelectSerialNumbersParams.Add(ttSelectSerialNumbersParams);
            ttSelectSerialNumbersParams.sourceRowID = ttIssueReturn.SysRowID;
            ttSelectSerialNumbersParams.plant = ttIssueReturn.SerialControlPlant;
            ttSelectSerialNumbersParams.quantity = Convert.ToInt32(Math.Abs(convQty));
            ttSelectSerialNumbersParams.allowVoided = false;
            ttSelectSerialNumbersParams.enableCreate = false;
            ttSelectSerialNumbersParams.enableRetrieve = true;
            ttSelectSerialNumbersParams.enableSelect = true;
            ttSelectSerialNumbersParams.partNum = ttIssueReturn.PartNum;
            ttSelectSerialNumbersParams.attributeSetID = ttIssueReturn.AttributeSetID;
            ttSelectSerialNumbersParams.attributeSetShortDescription = attributeSetShortDescription;
            ttSelectSerialNumbersParams.revisionNum = revisionNum;

            switch (ttIssueReturn.ProcessID.ToUpperInvariant())
            {
                case "ISSUEMATERIAL":
                case "HHISSUEMATERIAL":
                    {
                        ttSelectSerialNumbersParams.transType = "STK-MTL";
                        if (ttIssueReturn.SerialControlPlantIsFromPlt == true)
                        {
                            snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and WarehouseCode = '" + ttIssueReturn.FromWarehouseCode + "' and SNStatus = 'INVENTORY' and Voided = 0 and Scrapped = 0";
                            if (binToBin == true)
                            {
                                snWhere = snWhere + " and BinNum = '" + ttIssueReturn.FromBinNum + "'";
                            }
                            snWhere = snWhere + " and PCID = '" + ttIssueReturn.FromPCID + "'";
                            // if there is a PCID then we need the lotNum to ensure we get the correct SerialNo to select from since there 
                            // can be multiple PkgControlItem for the same part but different lots for a serial tracked part
                            if (!String.IsNullOrEmpty(ttIssueReturn.FromPCID))
                            {
                                snWhere = snWhere + " and LotNum = '" + ttIssueReturn.LotNum + "'";
                            }
                        }
                        else
                        {
                            ttSelectSerialNumbersParams.enableCreate = true;
                            ttSelectSerialNumbersParams.enableRetrieve = false;
                            ttSelectSerialNumbersParams.allowVoided = true;
                        }
                    }
                    break;/* end IssueMaterial and HHIssueMaterial */
                case "UNPICK":
                case "UNPICKPCID":
                    {
                        bool unpickFromWIP = ttIssueReturn.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase);
                        ttSelectSerialNumbersParams.transType = unpickFromWIP ? "WIP-WIP" : "STK-STK";
                        if (ttIssueReturn.SerialControlPlantIsFromPlt == true)
                        {
                            snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and WarehouseCode = '" + ttIssueReturn.FromWarehouseCode + "' and Binnum = '" + ttIssueReturn.FromBinNum + "' and Voided = 0 and Scrapped = 0 and OrderNum = " + Compatibility.Convert.ToString(ttIssueReturn.OrderNum) + " and OrderLine = " + Compatibility.Convert.ToString(ttIssueReturn.OrderLine) + " and OrderRelNum = " + Compatibility.Convert.ToString(ttIssueReturn.OrderRel);
                            snWhere = snWhere + " and PCID = '" + ttIssueReturn.FromPCID + "'";
                            if (unpickFromWIP)
                            {
                                snWhere = snWhere + " and JobNum = '" + ttIssueReturn.FromJobNum + "'";
                            }
                            if (String.IsNullOrEmpty(ttIssueReturn.FromPCID))
                            {
                                snWhere = snWhere + " and SNStatus = 'PICKED'";
                            }
                            else
                            {
                                snWhere = snWhere + " and (SNStatus = 'PICKED' or SNStatus = 'INVENTORY')";
                            }
                        }
                        else
                        {
                            ttSelectSerialNumbersParams.enableCreate = true;
                            ttSelectSerialNumbersParams.enableRetrieve = false;
                            ttSelectSerialNumbersParams.allowVoided = true;
                        }
                    }
                    break;/* end Unpick */
                case "ISSUEASSEMBLY":
                case "HHISSUEASSEMBLY":
                    {
                        ttSelectSerialNumbersParams.transType = "STK-ASM";
                        if (ttIssueReturn.SerialControlPlantIsFromPlt == true)
                        {
                            snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and WarehouseCode = '" + ttIssueReturn.FromWarehouseCode + "' and SNStatus = 'INVENTORY' and Voided = 0 and Scrapped = 0";
                            if (binToBin == true)
                            {
                                snWhere = snWhere + " and BinNum = '" + ttIssueReturn.FromBinNum + "'";
                            }
                            snWhere = snWhere + " and PCID = '" + ttIssueReturn.FromPCID + "'";
                            // if there is a PCID then we need the lotNum to ensure we get the correct SerialNo to select from since there 
                            // can be multiple PkgControlItem for the same part but different lots for a serial tracked part
                            if (!String.IsNullOrEmpty(ttIssueReturn.FromPCID))
                            {
                                snWhere = snWhere + " and LotNum = '" + ttIssueReturn.LotNum + "'";
                            }
                        }
                        else
                        {
                            ttSelectSerialNumbersParams.enableCreate = true;
                            ttSelectSerialNumbersParams.enableRetrieve = false;
                            ttSelectSerialNumbersParams.allowVoided = true;
                        }
                    }
                    break;/* end IssueAssembly and HHIssueAssembly */
                case "ISSUEMISCELLANEOUSMATERIAL":
                    {
                        ttSelectSerialNumbersParams.transType = "STK-UKN";
                        snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and WarehouseCode = '" + ttIssueReturn.FromWarehouseCode + "' and SNStatus = 'INVENTORY' and Voided = 0 and Scrapped = 0";
                        if (binToBin == true)
                        {
                            snWhere = snWhere + " and BinNum = '" + ttIssueReturn.FromBinNum + "'";
                        }
                        snWhere = snWhere + " and PCID = ''";
                    }
                    break;/* end IssueMiscellaneousMaterial  */

                case "RETURNMATERIAL":
                case "HHRETURNMATERIAL":
                    {
                        ttSelectSerialNumbersParams.transType = "MTL-STK";
                        if (ttIssueReturn.SerialControlPlantIsFromPlt == true)
                        {
                            snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and JobNum = '" + ttIssueReturn.FromJobNum + "' and AssemblySeq = '" + Compatibility.Convert.ToString(ttIssueReturn.FromAssemblySeq) + "' and Voided = 0 and Scrapped = 0 and (SNStatus = 'WIP' or SNStatus = 'CONSUMED')";
                            snWhere = !String.IsNullOrEmpty(ttIssueReturn.FromPCID) ?
                              snWhere + " and PCID = '" + ttIssueReturn.FromPCID + "'"
                            : snWhere + " and PCID = ''";
                        }
                        else
                        {
                            ttSelectSerialNumbersParams.enableCreate = true;
                            ttSelectSerialNumbersParams.enableRetrieve = false;
                            ttSelectSerialNumbersParams.allowVoided = true;
                        }
                    }
                    break;/* end ReturnMaerial or HHReturnMaterial */
                case "RETURNASSEMBLY":
                case "HHRETURNASSEMBLY":
                    {
                        ttSelectSerialNumbersParams.transType = "ASM-STK";
                        if (ttIssueReturn.SerialControlPlantIsFromPlt == true)
                        {
                            snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and JobNum = '" + ttIssueReturn.FromJobNum + "' and AssemblySeq = '" + Compatibility.Convert.ToString(ttIssueReturn.FromAssemblySeq) + "' and Voided = 0 and Scrapped = 0 and (SNStatus = 'WIP' or SNStatus = 'CONSUMED')";
                            snWhere = !String.IsNullOrEmpty(ttIssueReturn.FromPCID) ?
                               snWhere + " and PCID = '" + ttIssueReturn.FromPCID + "'"
                             : snWhere + " and PCID = ''";
                        }
                        else
                        {
                            ttSelectSerialNumbersParams.enableCreate = true;
                            ttSelectSerialNumbersParams.enableRetrieve = false;
                            ttSelectSerialNumbersParams.allowVoided = true;
                        }
                    }
                    break;/* end ReturnAssembly or HHReturnAssembly */
                case "RETURNMISCELLANEOUSMATERIAL":
                    {
                        ttSelectSerialNumbersParams.transType = "UKN-STK";
                        snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and SNStatus = 'MISC-ISSUE' and Voided = 0 and Scrapped = 0";
                        snWhere = snWhere + " and PCID = ''";
                    }
                    break;/* end ReturnMiscellaneousMaterial */
                case "ADJUSTWIP":
                    {
                        ttSelectSerialNumbersParams.transType = "ADJ-WIP";
                        snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and JobNum = '" + ttIssueReturn.ToJobNum + "' and AssemblySeq = '" + Compatibility.Convert.ToString(ttIssueReturn.ToAssemblySeq) + "' and Voided = 0 and (SNStatus = 'WIP' or SNStatus = 'CONSUMED')";
                        snWhere = !String.IsNullOrEmpty(ttIssueReturn.ToPCID) ?
                           snWhere + " and PCID = '" + ttIssueReturn.ToPCID + "'"
                         : snWhere + " and PCID = ''";
                    }
                    break;/* end AdjustWIP */
                case "MOVEWIP":
                    {
                        ttSelectSerialNumbersParams.transType = "WIP-WIP";
                        snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and WarehouseCode = '" + ttIssueReturn.FromWarehouseCode + "' and JobNum = '" + ttIssueReturn.FromJobNum + "' and AssemblySeq = '" + Compatibility.Convert.ToString(ttIssueReturn.FromAssemblySeq) + "' and NextLbrOprSeq = " + Compatibility.Convert.ToString(ttIssueReturn.FromJobSeq) + " and Voided = 0 and (SNStatus = 'WIP' or SNStatus = 'CONSUMED')";
                    }
                    if (binToBin == true)
                    {
                        snWhere = snWhere + " and BinNum = '" + ttIssueReturn.FromBinNum + "'";
                    }
                    if (!String.IsNullOrEmpty(snWhere))
                    {
                        snWhere = !String.IsNullOrEmpty(ttIssueReturn.FromPCID) ?
                           snWhere + " and PCID = '" + ttIssueReturn.FromPCID + "'"
                         : snWhere + " and PCID = ''";
                    }
                    else
                    { snWhere = "PCID = ''"; }

                    break;/* end MoveWIP */
                case "ADJUSTMATERIAL":
                    {
                        ttSelectSerialNumbersParams.transType = "ADJ-MTL";
                        snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and JobNum = '" + ttIssueReturn.ToJobNum + "' and AssemblySeq = '" + Compatibility.Convert.ToString(ttIssueReturn.ToAssemblySeq) + "' and MtlSeq = '" + Compatibility.Convert.ToString(ttIssueReturn.ToJobSeq) + "' and Voided = 0 and (SNStatus = 'WIP' or SNStatus = 'CONSUMED')";
                        snWhere = !String.IsNullOrEmpty(ttIssueReturn.ToPCID) ?
                           snWhere + " and PCID = '" + ttIssueReturn.ToPCID + "'"
                         : snWhere + " and PCID = ''";
                    }
                    break;/* end AdjustMaterial */
                case "MOVEMATERIAL":
                    {
                        ttSelectSerialNumbersParams.transType = "MTL-MTL";
                        snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and JobNum = '" + ttIssueReturn.FromJobNum + "' and AssemblySeq = '" + Compatibility.Convert.ToString(ttIssueReturn.FromAssemblySeq) + "' and MtlSeq = '" + Compatibility.Convert.ToString(ttIssueReturn.FromJobSeq) + "' and Voided = 0 and (SNStatus = 'WIP' or SNStatus = 'CONSUMED')";
                        snWhere = snWhere + " and PCID = ''";
                    }
                    break;/* end MoveMaterial */
                case "MATERIALQUEUE":
                case "HHMATERIALQUEUE":
                case "HHAUTOSELECTTRANSACTIONS":
                    {
                        ttSelectSerialNumbersParams.transType = ttIssueReturn.TranType;
                        ttSelectSerialNumbersParams.allowVoided = true;
                        this.checkSNStatus(ttIssueReturn.TranType, ttIssueReturn.TranQty, out validStatus);
                        // if the to plant is controlling serial processing and to plant is outbound tracking 
                        // and the parts are going into an outbound PCID we have to allow SN to be created or select voided.
                        bool voidedOrNewOnly = (!ttIssueReturn.SerialControlPlantIsFromPlt) && (toPCIDIsOutboundPCIDInOutboundPlant(ttIssueReturn));
                        if (voidedOrNewOnly)
                        {
                            int toPlantTracking = this.LibSerialCommon.isSerTraPlantType(ttIssueReturn.SerialControlPlant);
                            GetPkgControlHeaderOutboundContainer(Session.CompanyID, ttIssueReturn.ToPCID);
                            ttSelectSerialNumbersParams.enableCreate = true;
                            ttSelectSerialNumbersParams.allowVoided = true;
                            ttSelectSerialNumbersParams.enableRetrieve = false;
                            ttSelectSerialNumbersParams.enableSelect = true;
                            statusClause = string.Empty;
                            snWhere = string.Empty; // no retrieve allowed
                        }
                        else
                        {
                            if (StringExtensions.Compare(validStatus, "WIP") == 0)
                            {
                                statusClause = " and (SNStatus = 'WIP' or SNStatus = 'CONSUMED')";
                                if (StringExtensions.Compare(ttIssueReturn.TranType, "MFG-OPR") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                                {
                                    statusClause = statusClause + " and JobNum = '" + ttIssueReturn.FromJobNum + "' and AssemblySeq = '" + Compatibility.Convert.ToString(ttIssueReturn.FromAssemblySeq) + "'";
                                    if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                                    {
                                        statusClause = statusClause + " and NextLbrOprSeq = " + Compatibility.Convert.ToString(ttIssueReturn.FromJobSeq);
                                    }

                                    if (StringExtensions.Compare(ttIssueReturn.TranType, "MFG-OPR") == 0)
                                    {
                                        statusClause = statusClause + " and NextLbrOprSeq = " + Compatibility.Convert.ToString(ttIssueReturn.ToJobSeq);
                                    }
                                    /* Mfg-OPR cannot insist that the serial be in the from warehouse because WIP serial assignment, completions and
    movements do not always assign a warehouse */
                                    snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and Voided = 0" + statusClause;
                                }
                                else
                                {
                                    if (StringExtensions.Compare(ttIssueReturn.TranType, "INS-SUB") == 0)
                                    {
                                        statusClause = statusClause + " and JobNum = '" + ttIssueReturn.FromJobNum + "' and AssemblySeq = '" + Compatibility.Convert.ToString(ttIssueReturn.FromAssemblySeq) + "'";
                                    }

                                    snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and WarehouseCode = '" + ttIssueReturn.FromWarehouseCode + "' and Voided = 0" + statusClause;
                                }
                                if (!String.IsNullOrEmpty(snWhere))
                                {
                                    snWhere = snWhere + " and PCID = '" + ttIssueReturn.FromPCID + "'";
                                }
                                else
                                {
                                    snWhere = "PCID = '" + ttIssueReturn.FromPCID + "'";
                                }
                            }
                            else
                            {
                                statusClause = " and SNStatus = '" + validStatus + "'";
                                // ASM-INS and SUB-INS cannot insist that the serial be in the from warehouse because WIP serial assignment, completions and
                                // movements do not always assign a warehouse so do not add whs or bin clause 
                                if (StringExtensions.Compare(ttIssueReturn.TranType, "ASM-INS") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "STK-INS") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "SUB-INS") == 0)
                                {
                                    snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and Voided = 0" + statusClause;

                                    MtlQueue = this.FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
                                    if (MtlQueue != null && MtlQueue.NCTranID > 0)
                                    {
                                        snWhere = snWhere + " and NonConfNum = " + Compatibility.Convert.ToString(MtlQueue.NCTranID);
                                    }
                                }
                                else if (StringExtensions.Compare(ttIssueReturn.TranType, "DMR-ASM") == 0)
                                {
                                    snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and WarehouseCode = '" + ttIssueReturn.ToWarehouseCode + "' and BinNum = '" + ttIssueReturn.ToBinNum + "' and Voided = 0" + statusClause;
                                }
                                else
                                {
                                    snWhere = "Company = '" + Session.CompanyID + "' and PartNum = '" + ttIssueReturn.PartNum + "' and AttributeSetID = " + ttIssueReturn.AttributeSetID + " and WarehouseCode = '" + ttIssueReturn.FromWarehouseCode + "' and Voided = 0" + statusClause;
                                }

                                if (StringExtensions.Compare(ttIssueReturn.TranType, "INS-STK") == 0)
                                {
                                    MtlQueue = this.FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
                                    if (MtlQueue != null)
                                    {
                                        if (MtlQueue.RMADisp > 0)
                                        {
                                            snWhere = snWhere + " and RMANum = " + Compatibility.Convert.ToString(MtlQueue.RMANum) + " and RMALine = " + Compatibility.Convert.ToString(MtlQueue.RMALine) + " and RMADisp = " + Compatibility.Convert.ToString(MtlQueue.RMADisp);
                                        }
                                    }
                                }
                                /*SCR 48271 filter by Bin when PlantConfCtrl.BinToBinReqSN is set to true, also see SCR-88516 for STK-INS */
                                // SCR-138322 - only filter by bin if the from plant is controlling the SN processing
                                // and the plant that owns the from whs requires SN on binToBin.
                                if ((ttIssueReturn.SerialControlPlantIsFromPlt == true) && LibSerialCommon.binMovesRequireSN(ttIssueReturn.SerialControlPlant, ref PlantConfCtrl)
                                     && StringExtensions.Compare(ttIssueReturn.TranType, "ASM-INS") != 0 && StringExtensions.Compare(ttIssueReturn.TranType, "SUB-INS") != 0 && StringExtensions.Compare(ttIssueReturn.TranType, "STK-INS") != 0 && StringExtensions.Compare(ttIssueReturn.TranType, "DMR-ASM") != 0)
                                {
                                    snWhere = snWhere + " and BinNum = '" + ttIssueReturn.FromBinNum + "'";
                                }
                                // PCID is OK for RMA receipt to Inspection
                                if (!ExistsMtlQueue(ttIssueReturn.MtlQueueRowId, "RMA-INS"))
                                {
                                    if (!String.IsNullOrEmpty(snWhere))
                                    {
                                        snWhere = snWhere + " and PCID = '" + ttIssueReturn.FromPCID + "'";
                                    }
                                    else
                                    { snWhere = "PCID = '" + ttIssueReturn.FromPCID + "'"; }
                                }
                            } // end of else for if (StringExtensions.Compare(validStatus, "WIP") == 0)
                        } // else for voidedOrNewOnly
                    }
                    break;
            }
            ttSelectSerialNumbersParams.whereClause = snWhere;
            return ttSelectSerialNumbersParamsTablesetDS;
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void getToJobSeq(IssueReturnRow ttIssueReturn, bool runOnChangePartNumCore, bool runGetToWhse, out string pcMessage)
        {
            pcMessage = string.Empty;
            string cToType = string.Empty;
            string tmpResourceGrpID = string.Empty;
            string defaultUOM = string.Empty;
            string transUOM = string.Empty;
            cToType = this.getToType(ttIssueReturn.TranType);
            /* CHECK FOR JobMtl EXISTANCE */
            if (StringExtensions.Compare(cToType, "MTL") == 0)
            {
                JobMtl = this.FindFirstJobMtl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq);
                if (JobMtl == null)
                {
                    return;
                }
                /* Begin Disp-JobMtl */
                if (JobMtl != null)
                {
                    ttIssueReturn.ToJobSeqPartNum = JobMtl.PartNum;
                    ttIssueReturn.ToJobSeqPartDesc = JobMtl.Description;
                    ttIssueReturn.PartNum = JobMtl.PartNum;
                    ttIssueReturn.DimCode = JobMtl.BaseUOM;
                    ttIssueReturn.ReassignSNAsm = JobMtl.ReassignSNAsm;
                    ttIssueReturn.AttributeSetID = JobMtl.AttributeSetID;
                    ttIssueReturn.RevisionNum = JobMtl.RevisionNum;
                    /*If the UOM will be different, we should convert the quantity too*/
                    if (StringExtensions.Compare(ttIssueReturn.UM, JobMtl.IUM) != 0)
                    {
                        var outTranQty = ttIssueReturn.TranQty;
                        this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, JobMtl.IUM, out outTranQty, false);
                        ttIssueReturn.TranQty = outTranQty;
                    }
                    /* Uses the onChangePartNumCore rountine to set defaults from part */
                    // but not if called from GetNewIssueReturn for a mtlqueue record as these defaults have
                    // already been set.
                    if (runOnChangePartNumCore)
                        this.onChangePartNumCore(ttIssueReturn);

                    //If the Attribute Set does not already exist and one does exist on JobMtl, pull it in and set the descriptions accordingly
                    if (ttIssueReturn.AttributeSetID == 0 && JobMtl.AttributeSetID != 0)
                    {
                        ttIssueReturn.AttributeSetID = JobMtl.AttributeSetID;
                        this.GetAttributeDescriptions(ref ttIssueReturn);
                    }

                    ttIssueReturn.UM = JobMtl.IUM;
                    ttIssueReturn.RequirementUOM = JobMtl.IUM;
                    if (String.IsNullOrEmpty(ttIssueReturn.PartPartDescription))
                    {
                        ttIssueReturn.PartPartDescription = JobMtl.Description;
                    }
                    /* Begin Disp-RequiredQty */
                    ttIssueReturn.QtyPreviouslyIssued = JobMtl.IssuedQty;
                    ttIssueReturn.QtyRequired = JobMtl.RequiredQty;
                    transUOM = JobMtl.IUM;            /* End Disp-RequiredQty */

                    /* Set-IssuedComplete */
                    this.setIssuedComplete(ttIssueReturn);
                }/* if available JobAsmbl  End Disp-JobAsm */
                /* End Disp-JobMtl */
                /* Warning message */
                if (JobMtl.BackFlush)
                {
                    pcMessage = Strings.ThePartYouAreIssuingIsABackfPartDupliMaterCould;
                }
            }/* IF cToType = "MTL":U then  */
            if (StringExtensions.Compare(cToType, "ASM") == 0)
            {
                JobAsmbl = this.FindFirstJobAsmbl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq);
                if (JobAsmbl != null)
                {
                    ttIssueReturn.QtyPreviouslyIssued = JobAsmbl.IssuedQty;
                    ttIssueReturn.QtyRequired = JobAsmbl.PullQty;
                }
            }/* if cToType = "ASM":U */
            if (StringExtensions.Compare(cToType, "OPR") == 0)
            {
                JobOper = this.FindFirstJobOper11(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq);
                if (JobOper != null)
                {
                    this.LibGetResourceGrpID.getJobOperResourceGrpID(JobOper, string.Empty);

                    OpMaster = this.FindFirstOpMaster(JobOper.Company, JobOper.OpCode);
                    ttIssueReturn.ToJobSeqPartDesc = OpMaster.OpDesc;
                    ttIssueReturn.ToJobSeqPartNum = tmpResourceGrpID + "/" + JobOper.OpCode;
                    transUOM = JobOper.IUM;            /* IF NO PART DESCRIPTION THEN USE THE ASSEMLBY PART DESCRIPTION, UM */
                    if (String.IsNullOrEmpty(ttIssueReturn.PartPartDescription))
                    {
                        JobAsmbl = this.FindFirstJobAsmbl(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq);
                        if (JobAsmbl != null)
                        {
                            ttIssueReturn.PartPartDescription = JobAsmbl.Description;
                            transUOM = JobAsmbl.IUM;
                        }
                    }/* if ttIssueReturn.PartPartDescription = "" then */
                    /* End Disp-JobOper */

                    /* Find at first Part location wuith the same WH/Bin */
                    PartWip = FindFirstPartWip4(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq, ttIssueReturn.FromAssemblySeq, ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, ttIssueReturn.FromPCID);
                    if (PartWip == null)
                    {
                        PartWip = FindFirstPartWip6(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq, ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, ttIssueReturn.FromPCID);

                        if (PartWip == null)
                            PartWip = FindFirstPartWip2(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq, ttIssueReturn.FromAssemblySeq);
                    }

                    if (PartWip != null && StringExtensions.Compare(PartWip.PartNum, ttIssueReturn.PartNum) != 0)
                    {
                        ttIssueReturn.PartNum = PartWip.PartNum;
                        ttIssueReturn.PartPartDescription = "";

                        Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
                        if (Part != null)
                        {
                            ttIssueReturn.PartPartDescription = Part.PartDescription;
                        }
                    }

                    if (PartWip != null)
                    {
                        //ERPS-99557 Store From-parameters as they are not available in Adjust WIP  
                        if (ttIssueReturn.TranType.Equals("ADJ-WIP", StringComparison.OrdinalIgnoreCase))
                        {
                            ttIssueReturn.FromJobNum = PartWip.JobNum;
                            ttIssueReturn.FromAssemblySeq = PartWip.FromAssemblySeq;
                            ttIssueReturn.FromJobSeq = PartWip.FromOprSeq;
                        }
                        transUOM = PartWip.UM;
                        ttIssueReturn.FromWarehouseCode = PartWip.WareHouseCode;
                        ttIssueReturn.FromBinNum = PartWip.BinNum;
                        ttIssueReturn.LotNum = PartWip.LotNum;
                        ttIssueReturn.FromWarehouseCodeDescription = FindFirstWarehseDescription(PartWip.Company, PartWip.WareHouseCode);
                        ttIssueReturn.FromBinNumDescription = FindFirstWhseBinDescription(Session.CompanyID, PartWip.WareHouseCode, PartWip.BinNum);
                        ttIssueReturn.AttributeSetID = PartWip.AttributeSetID;
                        GetAttributeDescriptions(ref ttIssueReturn);
                        ttIssueReturn.RevisionNum = PartWip.RevisionNum;
                    }
                }/* end if avail JobOper */
                if (ttIssueReturn.ToAssemblySeq == 0 && ttIssueReturn.ToJobSeq == 0)
                {
                    ttIssueReturn.ToJobSeqPartNum = "";
                    ttIssueReturn.ToJobSeqPartDesc = "";
                }
            }/* WIP-BLOCK */

            /* Note (Orv) SCR101475 added the condition cFromType <> "MTL":U
               I did this just to safe and only affect  to MTL transactions.
               However, I believe this could be simplifed and apply to everything but this would require much more testing */

            if (cToType.Compare("MTL") != 0)
            {
                if (String.IsNullOrEmpty(transUOM))
                {
                    transUOM = ttIssueReturn.UM;
                }

                this.LibAppService.DefaultTransUOM(ttIssueReturn.PartNum, transUOM, out defaultUOM);
                if (ttIssueReturn.TrackDimension || StringExtensions.Compare(cToType, "MTL") != 0)
                {
                    ttIssueReturn.RequirementUOM = transUOM;
                }
                else
                {
                    if (StringExtensions.Compare(transUOM, defaultUOM) != 0)
                    {
                        var outQtyPreviouslyIssued3 = ttIssueReturn.QtyPreviouslyIssued;
                        this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.QtyPreviouslyIssued, transUOM, defaultUOM, out outQtyPreviouslyIssued3, false);
                        ttIssueReturn.QtyPreviouslyIssued = outQtyPreviouslyIssued3;
                        var outQtyRequired3 = ttIssueReturn.QtyRequired;
                        this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.QtyRequired, transUOM, defaultUOM, out outQtyRequired3, false);
                        ttIssueReturn.QtyRequired = outQtyRequired3;
                    }
                    ttIssueReturn.RequirementUOM = defaultUOM;
                }
            }
            if (ttIssueReturn.QtyPreviouslyIssued != 0)
            {
                var outQtyPreviouslyIssued4 = ttIssueReturn.QtyPreviouslyIssued;
                this.LibAppService.RoundToUOMDec(ttIssueReturn.RequirementUOM, ref outQtyPreviouslyIssued4);
                ttIssueReturn.QtyPreviouslyIssued = outQtyPreviouslyIssued4;
            }
            var outQtyRequired4 = ttIssueReturn.QtyRequired;
            this.LibAppService.RoundToUOMDec(ttIssueReturn.RequirementUOM, ref outQtyRequired4);
            ttIssueReturn.QtyRequired = outQtyRequired4;
            if (runGetToWhse)
                this.getToWhse(ttIssueReturn);
        }

        private void getToWhse(IssueReturnRow ttIssueReturn)
        {
            string cToType = string.Empty;
            string cFromType = string.Empty;
            string tmpWhse = string.Empty;
            string tmpWhseBin = string.Empty;
            Erp.Tables.JobMtl altJobMtl = null;
            bool lMultipleToWhse = false;
            bool lMultipleFromWhse = false;
            var WarehseQuery6_Param = ((PartWhse != null) ? PartWhse.WarehouseCode : "");
            var WarehseQuery7_Param = ((PartPlant != null) ? PartPlant.PrimWhse : "");
            string warehouseCode = string.Empty;
            cToType = this.getToType(ttIssueReturn.TranType);
            /*params for GetWarehosueInfo */
            string op_InputWhse = string.Empty;
            string op_InputBinNum = string.Empty;
            string op_OutputWhse = string.Empty;
            string op_OutputBinNum = string.Empty;
            string v_ResourceGrpID = string.Empty;
            string v_ResourceID = string.Empty;
            string contractID = string.Empty;

            switch (cToType.ToUpperInvariant())
            {
                case "STK":
                    {
                        cFromType = this.getFromType(ttIssueReturn.TranType);

                        if (cFromType.Equals("MTL", StringComparison.OrdinalIgnoreCase))
                        {
                            contractID = this.ExistsJobMtlContractID(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq);
                        }
                        else if (cFromType.Equals("ASM", StringComparison.OrdinalIgnoreCase) && ttIssueReturn.FromAssemblySeq != 0)
                        {
                            contractID = this.ExistsJobAsmblContractID(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq);
                        }

                        if (!string.IsNullOrEmpty(contractID))
                        {
                            var bPlanContractHdr = FindFirstPlanContractHdr(Session.CompanyID, contractID);
                            if (bPlanContractHdr != null)
                            {
                                var bPlanContractDtl = FindFirstPlanContractDtl(bPlanContractHdr.Company, bPlanContractHdr.ContractID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID);
                                if (bPlanContractDtl != null && !String.IsNullOrEmpty(bPlanContractDtl.WarehouseCode))
                                {
                                    ttIssueReturn.ToWarehouseCode = bPlanContractDtl.WarehouseCode;
                                    ttIssueReturn.ToWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, bPlanContractDtl.WarehouseCode);
                                    ttIssueReturn.ToBinNum = bPlanContractDtl.BinNum;
                                    ttIssueReturn.ToBinNumDescription = this.FindFirstWhseBinDescription(Session.CompanyID, bPlanContractDtl.WarehouseCode, bPlanContractDtl.BinNum);
                                    ttIssueReturn.ToPCID = "";
                                    break;
                                }
                                else
                                {
                                    var bPlanContractWhseBinDefaultInv = FindFirstPlanContractWhseBinDefaultInv(bPlanContractHdr.Company, bPlanContractHdr.ContractID);
                                    if (bPlanContractWhseBinDefaultInv != null)
                                    {
                                        ttIssueReturn.ToWarehouseCode = bPlanContractWhseBinDefaultInv.WarehouseCode;
                                        ttIssueReturn.ToWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, bPlanContractWhseBinDefaultInv.WarehouseCode);
                                        ttIssueReturn.ToBinNum = bPlanContractWhseBinDefaultInv.BinNum;
                                        ttIssueReturn.ToBinNumDescription = this.FindFirstWhseBinDescription(Session.CompanyID, bPlanContractWhseBinDefaultInv.WarehouseCode, bPlanContractWhseBinDefaultInv.BinNum);
                                        ttIssueReturn.ToPCID = "";
                                        break;
                                    }
                                }
                            }
                        }

                        lMultipleFromWhse = this.isWarehouseMultiple(cFromType, ttIssueReturn.PartNum);
                        lMultipleToWhse = this.isWarehouseMultiple(cToType, ttIssueReturn.PartNum);            /* "TO" INVENTORY */
                        if (String.IsNullOrEmpty(ttIssueReturn.ToWarehouseCode))
                        {
                            var JobMtlResult = this.FindFirstJobMtl10(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq);
                            if (!lMultipleToWhse)
                            {
                                PartWhse = this.FindFirstPartWhse2(Session.CompanyID, ttIssueReturn.PartNum);

                                if (JobMtlResult != null && !String.IsNullOrEmpty(JobMtlResult.WarehouseCode))
                                {
                                    warehouseCode = JobMtlResult.WarehouseCode;
                                }
                                else if (PartWhse != null)
                                {
                                    warehouseCode = PartWhse.WarehouseCode;
                                }

                                ttIssueReturn.ToWarehouseCode = warehouseCode;
                                ttIssueReturn.ToWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, warehouseCode);
                            }
                            else
                            {
                                PartPlant = this.FindFirstPartPlant(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum);

                                if (JobMtlResult != null && !String.IsNullOrEmpty(JobMtlResult.WarehouseCode))
                                {
                                    warehouseCode = JobMtlResult.WarehouseCode;
                                }
                                else if (PartPlant != null)
                                {
                                    warehouseCode = PartPlant.PrimWhse;
                                }

                                ttIssueReturn.ToWarehouseCode = warehouseCode;
                                ttIssueReturn.ToWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, warehouseCode);
                            }
                        }
                        /* Get To BinNumber */
                        if (String.IsNullOrEmpty(ttIssueReturn.ToBinNum))
                        {
                            PlantWhse = this.FindFirstPlantWhse2(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum, ttIssueReturn.ToWarehouseCode);
                            if (PlantWhse != null)
                            {
                                WhseBin = this.FindFirstWhseBin(Session.CompanyID, ttIssueReturn.ToWarehouseCode, PlantWhse.PrimBin);
                                ttIssueReturn.ToBinNum = PlantWhse.PrimBin;
                                ttIssueReturn.ToBinNumDescription = ((WhseBin != null) ? WhseBin.Description : "");
                            }
                        }
                        /* NO PRIMARY BIN - ATTEMPT TO find ANY BIN THAT CONTAINS THIS PART */
                        if (String.IsNullOrEmpty(ttIssueReturn.ToBinNum))
                        {
                            PartBin = this.FindFirstPartBin(Session.CompanyID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, ttIssueReturn.ToWarehouseCode);
                            if (PartBin != null)
                            {
                                WhseBin = this.FindFirstWhseBin(Session.CompanyID, ttIssueReturn.ToWarehouseCode, PartBin.BinNum);
                                ttIssueReturn.ToBinNum = PartBin.BinNum;
                                ttIssueReturn.ToBinNumDescription = ((WhseBin != null) ? WhseBin.Description : "");
                            }
                        }
                    }
                    break;/* when "STK:U */
                case "MTL":
                    {
                        if (!ttIssueReturn.ToJobPlant.KeyEquals(Session.PlantID))
                        {
                            ttIssueReturn.ToWarehouseCode = "";
                            ttIssueReturn.ToWarehouseCodeDescription = "";
                            ttIssueReturn.ToBinNum = "";
                            ttIssueReturn.ToBinNumDescription = "";
                            ttIssueReturn.ToPCID = "";
                        }

                        altJobMtl = this.FindFirstJobMtl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq);
                        if (altJobMtl == null)
                        {
                            return;
                        }

                        if (!ttIssueReturn.ToJobPlant.KeyEquals(Session.PlantID))
                        {
                            return;
                        }
                        /* DEFAULT "TO" WareHouse TO THE "INPUT" WareHouse OF THE WORKCENTER OF THE RELATED OPERATION */
                        /* DETERIMINE THE MATERIALS RELATED OPERATION */
                        if (altJobMtl.RelatedOperation > 0)
                        {
                            JobOper = this.FindFirstJobOper12(altJobMtl.Company, altJobMtl.JobNum, altJobMtl.AssemblySeq, altJobMtl.RelatedOperation);
                        }
                        else
                        {
                            JobOper = this.FindFirstJobOper13(altJobMtl.Company, altJobMtl.JobNum, altJobMtl.AssemblySeq, 0);
                        }
                        if (String.IsNullOrEmpty(ttIssueReturn.ToWarehouseCode) || ttIssueReturn.TranType.Equals("STK-MTL", StringComparison.OrdinalIgnoreCase))
                        {
                            if (JobOper != null)
                            {
                                JobOpDtl = this.FindFirstJobOpDtl(Session.CompanyID, JobOper.JobNum, JobOper.AssemblySeq, JobOper.OprSeq, JobOper.PrimaryProdOpDtl);
                                if (JobOpDtl != null)
                                {
                                    this.LibGetWarehouseInfo._GetWarehouseInfo(out op_InputWhse, out op_InputBinNum, out op_OutputWhse, out op_OutputBinNum, ref v_ResourceGrpID, ref v_ResourceID, JobOper.PrimaryProdOpDtl, ref JobOpDtl);
                                }/* if available   JobOpDtl */
                                tmpWhse = op_InputWhse;
                                tmpWhseBin = op_InputBinNum;

                                WhseBin = this.FindFirstWhseBin(Session.CompanyID, tmpWhse, tmpWhseBin);
                                ttIssueReturn.ToWarehouseCode = tmpWhse;
                                ttIssueReturn.ToWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, tmpWhse);
                                ttIssueReturn.ToBinNum = tmpWhseBin;
                                ttIssueReturn.ToBinNumDescription = ((WhseBin != null) ? WhseBin.Description : "");
                                ttIssueReturn.ToPCID = "";
                            }
                        }
                    }
                    break;/* "TO" MTL */
                case "ASM":
                    {
                        if (!ttIssueReturn.ToJobPlant.KeyEquals(Session.PlantID))
                        {
                            ttIssueReturn.ToWarehouseCode = "";
                            ttIssueReturn.ToWarehouseCodeDescription = "";
                            ttIssueReturn.ToBinNum = "";
                            ttIssueReturn.ToBinNumDescription = "";
                            ttIssueReturn.ToPCID = "";
                        }

                        JobAsmbl = this.FindFirstJobAsmbl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq);
                        if (JobAsmbl == null)
                        {
                            return;
                        }

                        if (!ttIssueReturn.ToJobPlant.KeyEquals(Session.PlantID))
                        {
                            return;
                        }
                        /* DEFAULT "TO" WareHouse TO THE "INPUT" WareHouse OF THE WORKCENTER OF THE RELATED OPERATION */
                        /* DETERIMINE THE ASSEMBLIES RELATED OPERATION */
                        if (JobAsmbl.RelatedOperation > 0)
                        {
                            JobOper = this.FindFirstJobOper(JobAsmbl.Company, JobAsmbl.JobNum, JobAsmbl.Parent, JobAsmbl.RelatedOperation, Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement));
                        }
                        else
                        {
                            JobOper = this.FindFirstJobOper2(JobAsmbl.Company, JobAsmbl.JobNum, JobAsmbl.Parent, 0, Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement));
                        }
                        /* GET THE DEFAULT WareHouse FOR THE OPERATION FROM IT'S WORKCENETER */
                        if (JobOper != null)
                        {
                            int OutAsmSeq = 0;
                            int OutOprSeq = 0;
                            string OutWCCode = string.Empty;
                            string next_op_InputWhse = string.Empty;
                            string next_op_InputBin = string.Empty;
                            this.GetNextOprSeq.RunGetNextOprSeq(JobOper.JobNum, JobOper.AssemblySeq, JobOper.OprSeq, false, out OutOprSeq, out OutAsmSeq, out OutWCCode);
                            /* Captures the next operations input warehouse and bin */
                            if (OutOprSeq > 0)
                            {
                                JobOpDtl = this.FindFirstJobOpDtl(Session.CompanyID, JobOper.JobNum, OutAsmSeq, OutOprSeq);
                                this.LibGetWarehouseInfo._GetWarehouseInfo(out next_op_InputWhse, out next_op_InputBin, out op_OutputWhse, out op_OutputBinNum, ref v_ResourceGrpID, ref v_ResourceID, JobOper.PrimaryProdOpDtl, ref JobOpDtl);
                            }

                            /* Sets op variables to current operations warehouse and bin */
                            JobOpDtl = this.FindFirstJobOpDtl(JobOper.Company, JobOper.JobNum, JobOper.AssemblySeq, JobOper.OprSeq, JobOper.PrimaryProdOpDtl);
                            this.LibGetWarehouseInfo._GetWarehouseInfo(out op_InputWhse, out op_InputBinNum, out op_OutputWhse, out op_OutputBinNum, ref v_ResourceGrpID, ref v_ResourceID, JobOper.PrimaryProdOpDtl, ref JobOpDtl);
                            /* Updates input warehouse and bin if next operation exists */
                            // dparillo SCR 148797 - commented out below code from SCR 103345 - I am not convinced this is
                            // needed. I will review further and remove later.
                            //if (OutOprSeq > 0)
                            //{
                            //    op_InputWhse = next_op_InputWhse;
                            //    op_InputBinNum = next_op_InputBin;
                            //}

                            /* Checks for final operation if found uses output warehouse and bin */
                            // dparillo SCR 149899 - commented this all out as well for the same reason as above.
                            //var JobOperCheck = this.FindFirstJobAsmbl(JobOper.Company, JobOper.JobNum, JobOper.AssemblySeq);
                            //if (JobOperCheck != null && JobOperCheck.FinalOpr != OutOprSeq)
                            //{
                            tmpWhse = op_InputWhse;
                            tmpWhseBin = op_InputBinNum;
                            //}
                            //else
                            //{
                            //    tmpWhse = op_OutputWhse;
                            //    tmpWhseBin = op_OutputBinNum;
                            //}  

                            WhseBin = this.FindFirstWhseBin(Session.CompanyID, tmpWhse, tmpWhseBin);
                            ttIssueReturn.ToWarehouseCode = tmpWhse;
                            ttIssueReturn.ToWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, tmpWhse);
                            ttIssueReturn.ToBinNum = tmpWhseBin;
                            ttIssueReturn.ToBinNumDescription = ((WhseBin != null) ? WhseBin.Description : "");
                            ttIssueReturn.ToPCID = "";
                        }
                    }
                    break;/* "TO" ASM */
                case "UKN":
                    {
                    }
                    break;
                case "OPR":
                    {
                        if (!ttIssueReturn.ToJobPlant.KeyEquals(Session.PlantID))
                        {
                            return;
                        }

                        if (String.IsNullOrEmpty(ttIssueReturn.ToWarehouseCode))
                        {
                            int v_JobSeq = 0;
                            v_JobSeq = ttIssueReturn.ToJobSeq;
                            if (v_JobSeq == 0)
                            {
                                JobAsmbl = this.FindFirstJobAsmbl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq);
                                if (JobAsmbl != null)
                                {
                                    v_JobSeq = JobAsmbl.RelatedOperation;
                                }
                            }

                            JobOper = this.FindFirstJobOper3(ttIssueReturn.Company, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, v_JobSeq, Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement));
                            if (JobOper != null)
                            {
                                JobOpDtl = this.FindFirstJobOpDtl(Session.CompanyID, JobOper.JobNum, JobOper.AssemblySeq, JobOper.OprSeq, JobOper.PrimaryProdOpDtl);
                                if (JobOpDtl != null)
                                {
                                    this.LibGetWarehouseInfo._GetWarehouseInfo(out op_InputWhse, out op_InputBinNum, out op_OutputWhse, out op_OutputBinNum, ref v_ResourceGrpID, ref v_ResourceID, JobOper.PrimaryProdOpDtl, ref JobOpDtl);
                                }/* if available   JobOpDtl */
                                tmpWhse = op_InputWhse;
                                tmpWhseBin = op_InputBinNum;

                                WhseBin = this.FindFirstWhseBin(Session.CompanyID, tmpWhse, tmpWhseBin);
                                ttIssueReturn.ToWarehouseCode = tmpWhse;
                                ttIssueReturn.ToWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, tmpWhse);
                                ttIssueReturn.ToBinNum = tmpWhseBin;
                                ttIssueReturn.ToBinNumDescription = ((WhseBin != null) ? WhseBin.Description : "");
                                ttIssueReturn.ToPCID = "";
                            }
                        }
                    }
                    break;
                default:
                    {
                    }
                    break;
            }
        }

        /// <summary>
        /// Call this method to set the Qty to the remaining unissued amount.
        /// </summary>
        /// <param name="ds">IssueReturnDataSet</param>
        public void GetUnissuedQty(ref IssueReturnTableset ds)
        {
            CurrentFullTableset = ds;
            decimal UnissuedQty = decimal.Zero;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                UnissuedQty = ttIssueReturn.QtyRequired - ttIssueReturn.QtyPreviouslyIssued;
                this.LibAppService.UOMConv(ttIssueReturn.PartNum, UnissuedQty, ttIssueReturn.RequirementUOM, ttIssueReturn.UM, out ConvQty, false);
                if (UnissuedQty >= 0)
                {
                    ttIssueReturn.TranQty = ConvQty;
                }
            }
        }

        /// <summary>
        /// Sets ttIssueReturn fields for Unpick
        /// </summary>
        /// <param name="ds">Issue Return data set</param>
        public void GetUnpickSettings(ref IssueReturnTableset ds)
        {
            CurrentFullTableset = ds;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn == null)
            {
                throw new BLException(Strings.IssueReturnRecordNotFound, "ttIssueReturn", "RowMod");
            }
            this.enableSNButton(ttIssueReturn, "Unpick");
        }

        /// <summary>
        /// Validate if an assembly is valid for a job. if not returns false,
        /// otherwise returns true.
        /// </summary>
        /// <param name="pcJobNum">Job Number</param>
        /// <param name="piAssemblySeq">A sequence number that uniquely
        /// identifies the JobAsmbl record within the JobNum.</param>
        /// <param name="plFound">Found or not</param>
        public void IsValidAssembly(string pcJobNum, int piAssemblySeq, out bool plFound)
        {
            plFound = false;
            plFound = false;
            if ((this.ExistsJobHead(Session.CompanyID, pcJobNum, false, true)))
            {
                if ((this.ExistsJobAsmbl3(Session.CompanyID, pcJobNum, piAssemblySeq)))
                {
                    plFound = true;
                }
                else
                {
                    throw new BLException(Strings.AValidAssemblyIsRequired, "JobAsmbl", "AssemblySeq");
                }
            }

            JobHead = this.FindFirstJobHead(Session.CompanyID, pcJobNum);
            if (JobHead != null)
            {
                if (JobHead.JobReleased == false)
                {
                    throw new BLException(Strings.ThisJobHasNotBeenReleaEntryNotAllowed, "JobHead", "JobNum");
                }
                if (JobHead.JobClosed == true)
                {
                    throw new BLException(Strings.ThisJobHasBeenClosedEntryNotAllowed, "JobHead", "JobNum");
                }
            }
            else
            {
                throw new BLException(Strings.JobNotFound, "JobHead", "JobNum");
            }
        }

        private void InvokeTransactionSpecificProcessMethod(IssueReturnRow issueReturnRow, bool vWave, int vWaveNum, int vWaveMtlQueueSeq, out string pcMessage)
        {
            pcMessage = string.Empty;

            if (this.GetType().GetMethod("process_" + issueReturnRow.TranType.Replace('-', '_'), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) != null)
            {
                if (vWave)
                {
                    #region Wave

                    decimal vWaveLeftToProcess = issueReturnRow.TranQty;
                    decimal vThisTranQty = decimal.Zero;
                    string vWavePartNum = issueReturnRow.PartNum;
                    if (vWaveLeftToProcess > 0)
                    {
                        Wave = FindFirstWaveWithUpdLock(Session.CompanyID, vWaveNum, false);
                        if (Wave != null)
                        {
                            Wave.PickStarted = true;
                        }
                    }
                    ttIssueReturnWaveParentRows.Clear();
                    ttIssueReturnWaveParent = new IssueReturnWaveParent();
                    ttIssueReturnWaveParentRows.Add(ttIssueReturnWaveParent);
                    BufferCopy.Copy(issueReturnRow, ref ttIssueReturnWaveParent);
                    ttIssueReturnWaveChildRows.Clear();
                    CurrentFullTableset.IssueReturn.Clear();

                    foreach (var waveMtlQueue_iterator in SelectMtlQueueWithUpdLock(Session.CompanyID, Session.PlantID, vWavePartNum, "Wave-1:", Compatibility.Convert.ToString(vWaveMtlQueueSeq)))
                    {
                        MtlQueue waveMtlQueue = waveMtlQueue_iterator;
                        vThisTranQty = ((waveMtlQueue.Quantity <= vWaveLeftToProcess) ? waveMtlQueue.Quantity : vWaveLeftToProcess);
                        vWaveLeftToProcess = vWaveLeftToProcess - vThisTranQty;
                        CurrentFullTableset.IssueReturn.Clear();
                        issueReturnRow = new Erp.Tablesets.IssueReturnRow();
                        CurrentFullTableset.IssueReturn.Add(issueReturnRow);
                        BufferCopy.Copy(ttIssueReturnWaveParent, ref issueReturnRow);
                        issueReturnRow.TranQty = vThisTranQty;
                        issueReturnRow.OrderNum = waveMtlQueue.OrderNum;
                        issueReturnRow.OrderLine = waveMtlQueue.OrderLine;
                        issueReturnRow.OrderRel = waveMtlQueue.OrderRelNum;
                        issueReturnRow.FromJobNum = waveMtlQueue.TargetJobNum;
                        issueReturnRow.FromAssemblySeq = waveMtlQueue.TargetAssemblySeq;
                        issueReturnRow.FromJobSeq = waveMtlQueue.TargetMtlSeq;
                        issueReturnRow.ToJobNum = waveMtlQueue.TargetJobNum;
                        issueReturnRow.ToAssemblySeq = waveMtlQueue.TargetAssemblySeq;
                        issueReturnRow.ToJobSeq = waveMtlQueue.TargetMtlSeq;
                        issueReturnRow.TFOrdNum = waveMtlQueue.TargetTFOrdNum;
                        issueReturnRow.TFOrdLine = waveMtlQueue.TargetTFOrdLine;

                        this.GetType().InvokeMember("process_" + ttIssueReturnWaveParent.TranType.Replace('-', '_'), System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, this, new object[] { issueReturnRow });

                        if (vThisTranQty == waveMtlQueue.Quantity)
                        {
                            WaveOrder = FindFirstWaveOrderWithUpdLock(Session.CompanyID, vWaveNum, waveMtlQueue.OrderNum, waveMtlQueue.OrderLine, waveMtlQueue.OrderRelNum, waveMtlQueue.TargetJobNum, waveMtlQueue.TargetAssemblySeq, waveMtlQueue.TargetMtlSeq, waveMtlQueue.TargetTFOrdNum, waveMtlQueue.TargetTFOrdLine);
                            if (WaveOrder != null)
                            {
                                WaveOrder.BulkPickComplete = true;
                            }

                            Db.Release(ref WaveOrder);
                            Db.MtlQueue.Delete(waveMtlQueue);
                        }
                        else
                        {
                            waveMtlQueue.Quantity = waveMtlQueue.Quantity - vThisTranQty;
                        }

                        ttIssueReturnWaveChild = new IssueReturnWaveChild();
                        ttIssueReturnWaveChildRows.Add(ttIssueReturnWaveChild);
                        BufferCopy.Copy(issueReturnRow, ref ttIssueReturnWaveChild);

                        Wave = FindFirstWaveWithUpdLock(Session.CompanyID, vWaveNum);
                        if (Wave != null && !ExistsWaveOrder(Wave.Company, Wave.WaveNum, false))
                        {
                            Wave.PickComplete = true;
                        }

                        if (vWaveLeftToProcess <= 0)
                        {
                            break;
                        }
                    }
                    CurrentFullTableset.IssueReturn.Clear();

                    foreach (var ttIssueReturnWaveParentRow in (from ttIssueReturnWaveParent_Row in ttIssueReturnWaveParentRows
                                                                select ttIssueReturnWaveParent_Row))
                    {
                        ttIssueReturnWaveParent = ttIssueReturnWaveParentRow;
                        issueReturnRow = new Erp.Tablesets.IssueReturnRow();
                        CurrentFullTableset.IssueReturn.Add(issueReturnRow);
                        BufferCopy.Copy(ttIssueReturnWaveParent, ref issueReturnRow);
                    }

                    #endregion Wave
                }
                else
                {
                    try
                    {
                        string outMessage = string.Empty;
                        object[] args = new object[2] { issueReturnRow, outMessage };
                        this.GetType()
                            .InvokeMember("process_" + issueReturnRow.TranType.Replace('-', '_'),
                                          System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Instance |
                                          System.Reflection.BindingFlags.NonPublic, null, this, args);
                        outMessage = (string)args[1];
                        if (!string.IsNullOrEmpty(outMessage))
                        {
                            pcMessage += !string.IsNullOrEmpty(pcMessage)
                                ? "\n" + outMessage
                                : outMessage;
                        }
                    }
                    catch (Exception ex)
                    {
                        Exception innerEx = ex;
                        if (innerEx is not BLException)
                        {
                            innerEx = innerEx.InnerException;
                        }
                        if (innerEx == null)
                        {
                            innerEx = ex;
                        }
                        throw innerEx;

                    }
                }
            }
            else
            {
                throw new BLException(Strings.TransTypeIsNotHandledByIssueBusinProcess(issueReturnRow.TranType));
            }
        }

        /// <summary>
        /// Validate if FromPCID change is valid
        /// otherwise returns true.
        /// </summary>
        /// <param name="fromPCID">From PCID</param>
        /// <param name="allowDiffPartAndUM">If true allows to default parts that have a different part and/or  unit of measure</param>
        /// <param name="questionCheck">If true throws questions  for the user before defaulting PCID and Part values,
        ///     depending on whether it found a match for the Assembly part on the selected PCID</param>
        /// <param name="pCallProcess"> Calling process </param>
        /// <param name="questionMsg">Return the message for the question for the user</param>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pcMessage">Return pcMessage</param>
        public void OnChangeFromPCID(string fromPCID, bool allowDiffPartAndUM, bool questionCheck, string pCallProcess, out string questionMsg, ref IssueReturnTableset ds, out string pcMessage)
        {
            Erp.Tables.PkgControlStageHeader PkgControlStageHeader = null;

            questionMsg = string.Empty;
            pcMessage = string.Empty;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn == null)
            {
                throw new BLException(Strings.IssueReturnRecordNotFound, "ttIssueReturn", "RowMod");
            }

            ttIssueReturn.DisablePCIDRelatedFields = false;

            if (string.IsNullOrEmpty(fromPCID))
            {
                ttIssueReturn.FromPCID = fromPCID;

                switch (pCallProcess.ToUpperInvariant())
                {
                    case "HHISSUEASSEMBLY":
                    case "ISSUEASSEMBLY":
                        OnChangeToJobNum(ref ds, pCallProcess, out pcMessage);
                        break;
                    case "HHISSUEMATERIAL":
                    case "ISSUEMATERIAL":
                        OnChangeToJobSeq(ref ds, pCallProcess, out pcMessage);
                        break;
                }
                return;
            }

            // Extract PCID 
            string opPCID = string.Empty;
            using (Erp.Internal.Lib.ControlIDExtract libControlIDExtract = new Internal.Lib.ControlIDExtract(Db))
            {
                opPCID = extractPCIDFromString(Session.CompanyID, fromPCID, libControlIDExtract);
            }

            if (!string.IsNullOrEmpty(opPCID))
            {
                using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
                {
                    switch (pCallProcess.ToUpperInvariant())
                    {
                        case "ADJUSTWIP":
                        case "HHISSUEASSEMBLY":
                        case "HHISSUEMATERIAL":
                        case "HHMOVEMATERIAL":
                        case "HHRETURNASSEMBLY":
                        case "HHRETURNMATERIAL":
                        case "ISSUEMATERIAL":
                        case "ISSUEASSEMBLY":
                        case "MOVEMATERIAL":
                        case "MOVEWIP":
                        case "MOVEWIPPCID":
                        case "RETURNMATERIAL":
                        case "RETURNASSEMBLY":
                            {
                                PkgControlHeader = FindFirstPkgControlHeader(Session.CompanyID, opPCID);
                                if (PkgControlHeader == null)
                                {
                                    PkgControlStageHeader = FindFirstPkgControlStageHeader(Session.CompanyID, opPCID);
                                    if (PkgControlStageHeader == null)
                                        throw new BLException(Strings.PCIDNotFound(opPCID));
                                    else
                                        opPCID = PkgControlStageHeader.PCID;
                                }
                                else
                                    opPCID = PkgControlHeader.PCID;

                                libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, opPCID, true, string.Empty, false, pCallProcess);

                                using (Internal.Lib.PackageControl libPackageControl = new Internal.Lib.PackageControl(Db))
                                {
                                    string pcidWarehouseCode = string.Empty;
                                    string pcidBinNum = string.Empty;
                                    string pkgControlStatus = libPackageControl.GetPCIDStatus(Session.CompanyID, opPCID);
                                    if (!pkgControlStatus.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                                    {
                                        libPackageControl.GetPCIDLocation(Session.CompanyID, opPCID, out pcidWarehouseCode, out pcidBinNum);
                                        ttIssueReturn.FromWarehouseCode = pcidWarehouseCode;
                                        ttIssueReturn.FromBinNum = pcidBinNum;
                                    }
                                }

                                switch (pCallProcess.ToUpperInvariant())
                                {
                                    case "ADJUSTWIP":
                                    case "HHMOVEMATERIAL":
                                    case "HHRETURNASSEMBLY":
                                    case "HHRETURNMATERIAL":
                                    case "MOVEMATERIAL":
                                    case "MOVEWIP":
                                    case "RETURNASSEMBLY":
                                    case "RETURNMATERIAL":
                                        {
                                            using (Internal.Lib.PackageControlValidations libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
                                            {
                                                if (!libPackageControlValidations.ValidatePartExistsIsInPCID(Session.CompanyID, opPCID, ttIssueReturn.PartNum))
                                                    throw new BLException(Strings.PartNumIsNotItemPCID);
                                            }
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }

                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }

            Erp.Internal.Lib.PackageControlValidations.PkgControlHeaderPartialRow pkgControlHeaderRow = null;
            //Validations
            using (Internal.Lib.PackageControlValidations libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
            {
                bool isPCIDPick = ttIssueReturn.TranReference.Equals("PCIDPICK", StringComparison.OrdinalIgnoreCase);
                pcidUpdateValidations(isPCIDPick, ttIssueReturn.TranType, opPCID, Session.CompanyID, Session.PlantID, libPackageControlValidations, out pkgControlHeaderRow);
                ExceptionManager.AssertNoBLExceptions();
            }

            //Actions
            #region Actions - Try to auto-populate part information based.
            #region Considerations / scenarios
            // ERP-3198
            // Here we try to find a possible matching part num inside of the PCID
            // to auto-populate Part related information.
            // There are 3 scenarios:
            // 1) PCID contains only one part item, and that part item matches the requested Part Num and UM.
            //      - We default the Part fields.
            // 2) PCID contains only one part item, and that part item does not matches the requested Part Num and UM.
            //      A)  If we DO NOT allow use of a diff Part/Um
            //          - Then we throw an error.
            //
            //      B) If we allow use a different Part/Um 
            //          - Then we warn/ask the user that we have a different part/um than the requested.             
            //          - if the user agree then we default the Part fields.
            // 3) PCID contains more than one part item.
            //      A)  If we allow use of a different Part/Um
            //          - We warn/ask the user that the pcid have multiple items
            //          - if the user agree then we clean the Part fields for the user to select
            //
            //      B) If do not allow use a different Part/Um 
            //          - Then we make sure that there are multiple items for the Part/Num
            //            - And warn/the user that the pcid have multiple items and clean Lot Number   
            //          - If there is not match, then we throw an error
            //
            // A 4th scenario was added
            // 4) PCID contains more than one part and there are matches with the requested part
            //        -if only one, default part
            //        -if multiple parts with different lot numbers, send warning and clear part info
            #endregion
            // When doing a material movement, do not try to modify the Part information based on the From PCID selected.
            if (!pCallProcess.KeyEquals("MaterialQueue") && !pCallProcess.KeyEquals("HHMaterialQueue") && !pCallProcess.KeyEquals("HHAutoSelectTransactions"))
            {
                string partnum = string.Empty;
                if (ttIssueReturn.TranType.Equals("STK-ASM", StringComparison.OrdinalIgnoreCase))
                {
                    partnum = ttIssueReturn.ToAssemblyPartNum;
                }

                if (ttIssueReturn.TranType.Equals("STK-MTL", StringComparison.OrdinalIgnoreCase))
                {
                    partnum = ttIssueReturn.ToJobSeqPartNum;
                }

                if (ttIssueReturn.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))
                {
                    partnum = ttIssueReturn.ToAssemblyPartNum;
                }

                if (ExistsUniquePkgControlItemWithAnyPart(Session.CompanyID, Session.PlantID, opPCID))
                {
                    PkgControlItemPartialRow pkgControlItemPartialRow = FindFirstPkgControlItemWithPartPartialRow(Session.CompanyID, Session.PlantID, opPCID);

                    if (!(pkgControlItemPartialRow.ItemPartNum.KeyEquals(partnum)
                        && pkgControlItemPartialRow.ItemIUM.KeyEquals(ttIssueReturn.RequirementUOM)))
                    {
                        string message = Strings.DifferentPartPCIDQuestion(pkgControlItemPartialRow.ItemPartNum, pkgControlItemPartialRow.ItemIUM);
                        if (!allowDiffPartAndUM)
                        {
                            throw new BLException(message);
                        }

                        if (questionCheck)
                        {
                            questionMsg = string.Format("{0} {1}", message, Strings.Continue);
                            return;
                        }
                    }

                    PopulatePCIDItemRelatedFields(opPCID, allowDiffPartAndUM, false, pkgControlItemPartialRow);
                }
                else
                {
                    if (!allowDiffPartAndUM)
                    {
                        IEnumerable<PkgControlItemPartialRow> pkgControlItemPartialRows = SelectPkgControlItemMatchingPartNumAndUM(Session.CompanyID, Session.PlantID, opPCID, partnum, ttIssueReturn.RequirementUOM);
                        if (pkgControlItemPartialRows != null && pkgControlItemPartialRows.Count() == 1)
                        {
                            PkgControlItemPartialRow singlePkgControlItemPartialRow = (from partialRow in pkgControlItemPartialRows
                                                                                       select partialRow).FirstOrDefault();
                            PopulatePCIDItemRelatedFields(opPCID, allowDiffPartAndUM, false, singlePkgControlItemPartialRow);
                        }
                        else
                        {
                            if (pkgControlItemPartialRows.Count() >= 2)
                            {
                                if (questionCheck)
                                {
                                    questionMsg = Strings.MultipleItemsPCIDQuestionHH;
                                    return;
                                }
                                PopulatePCIDItemRelatedFields(opPCID, allowDiffPartAndUM, true, null);
                            }
                            else
                            {
                                throw new BLException(Strings.NoMatchFoundOnPCIDForPartAndUM(partnum, ttIssueReturn.RequirementUOM));
                            }
                        }
                    }
                    else
                    {
                        if (ExistsUniquePkgControlItemWithPart(Session.CompanyID, Session.PlantID, opPCID, partnum))
                        {
                            PkgControlItemPartialRow pkgControlItemPartialRow = FindFirstPkgCtrlItemWithPartPartialRow(Session.CompanyID, Session.PlantID, opPCID, partnum);
                            PopulatePCIDItemRelatedFields(opPCID, allowDiffPartAndUM, false, pkgControlItemPartialRow);
                        }
                        else if (ExistsMultiplePkgControlItem(Session.CompanyID, Session.PlantID, opPCID))
                        {
                            if (questionCheck)
                            {
                                questionMsg = Strings.MultipleItemsPCIDQuestion;
                                return;
                            }
                            PopulatePCIDItemRelatedFields(opPCID, allowDiffPartAndUM, true, null);
                        }
                    }
                }
            }
            #endregion

            // If we reach this point no errors or message where found, 
            //so we fill Whse and Bin information
            ttIssueReturn.DisablePCIDRelatedFields = true;
            ttIssueReturn.FromWarehouseCode = pkgControlHeaderRow.WarehouseCode;
            ttIssueReturn.FromBinNum = pkgControlHeaderRow.BinNum;
            ttIssueReturn.FromBinNumDescription = FindFirstWhseBinDescription(Session.CompanyID, pkgControlHeaderRow.WarehouseCode, pkgControlHeaderRow.BinNum);
            if (!ttIssueReturn.FromPCID.KeyEquals(opPCID))
            {
                ttIssueReturn.FromPCID = opPCID;
                this.enableSNButton(ttIssueReturn, pCallProcess);
                this.checkStatusTracking();
            }
        }

        private void PopulatePCIDItemRelatedFields(string pcid, bool allowDiffPartAndUM, bool multipleParts, PkgControlItemPartialRow pkgControlItemPartialRow)
        {
            if (multipleParts)
            {
                ttIssueReturn.LotNum = string.Empty;

                if (allowDiffPartAndUM)
                {
                    ttIssueReturn.PartNum = string.Empty;
                    ttIssueReturn.PartPartDescription = string.Empty;
                    ttIssueReturn.UM = string.Empty;
                }
            }
            else
            {
                ttIssueReturn.PartNum = pkgControlItemPartialRow.ItemPartNum;
                ttIssueReturn.PartPartDescription = pkgControlItemPartialRow.ItemPartDesc;
                ttIssueReturn.UM = pkgControlItemPartialRow.ItemIUM;
                ttIssueReturn.LotNum = pkgControlItemPartialRow.ItemLotNum;
            }
        }

        /// <summary>
        /// Check JobNum and return JobRelease and JobClosed
        /// </summary>
        /// <param name="ipJobNum">JobNum which should check</param>
        /// <param name="opJobReleased">Job Released</param>
        /// <param name="opJobClosed">Job Closed</param>
        /// <param name="opJobExists">Job exists</param>
        public void JobExists(string ipJobNum, out bool opJobReleased, out bool opJobClosed, out bool opJobExists)
        {
            opJobClosed = false;
            opJobExists = false;
            opJobReleased = false;

            JobHead = this.FindFirstJobHead(Session.CompanyID, ipJobNum);
            if (JobHead != null)
            {
                opJobExists = true;
                opJobClosed = JobHead.JobClosed;
                opJobReleased = JobHead.JobReleased;
            }
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.ToPCID changes (this is for ToPCID, for FromPCID use OnChangeFromPCID method).
        /// </summary>
        /// <param name="ipPCID">Proposed To PCID value</param>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pCallProcess">Calling Process value</param>
        public void OnChangeToPCID(string ipPCID, ref IssueReturnTableset ds, string pCallProcess)
        {
            Erp.Tables.PkgControlStageHeader PkgControlStageHeader = null;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();

            if (ttIssueReturn == null)
            {
                throw new BLException(Strings.TtIssueReturnRecordNotFound);
            }

            if (String.IsNullOrEmpty(ipPCID))
            {
                if (!String.IsNullOrEmpty(ttIssueReturn.ToPCID))
                {
                    ttIssueReturn.ToPCID = string.Empty;
                    SetToWhsBinFromMtlQueue(ref ttIssueReturn);
                    SetToWhsBinDescriptions(ref ttIssueReturn);
                    this.enableSNButton(ttIssueReturn, pCallProcess);
                    if (!ttIssueReturn.EnableSN)
                    {
                        this.checkStatusTracking();
                    }
                }
            }
            else
            {
                // Extract PCID 
                string opPCID = string.Empty;
                using (Erp.Internal.Lib.ControlIDExtract libControlIDExtract = new Internal.Lib.ControlIDExtract(Db))
                {
                    opPCID = extractPCIDFromString(Session.CompanyID, ipPCID, libControlIDExtract);
                }

                using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
                {
                    switch (pCallProcess.ToUpperInvariant())
                    {
                        case "ADJUSTMATERIAL":
                        case "ADJUSTWIP":
                        case "HHISSUEASSEMBLY":
                        case "HHISSUEMATERIAL":
                        case "HHMOVEMATERIAL":
                        case "HHRETURNASSEMBLY":
                        case "HHRETURNMATERIAL":
                        case "ISSUEASSEMBLY":
                        case "ISSUEMATERIAL":
                        case "MOVEMATERIAL":
                        case "MOVEWIP":
                        case "RETURNASSEMBLY":
                        case "RETURNMATERIAL":
                            {
                                PkgControlHeader = FindFirstPkgControlHeader(Session.CompanyID, opPCID);
                                if (PkgControlHeader == null)
                                {
                                    PkgControlStageHeader = FindFirstPkgControlStageHeader(Session.CompanyID, opPCID);
                                    if (PkgControlStageHeader == null)
                                    {
                                        throw new BLException(Strings.PCIDNotFound(opPCID));
                                    }
                                }

                                libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, string.Empty, false, opPCID, true, pCallProcess);

                                //If this is a Staged PCID in WIP Status, set the To Warehouse/Bin to where it currently resides
                                if (PkgControlStageHeader != null && PkgControlStageHeader.PkgControlStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase))
                                {
                                    // Set to location based on PkgControlHeader location
                                    SetToWhseBinFromPCID(ref ttIssueReturn, Session.CompanyID, PkgControlStageHeader.PkgControlStatus, PkgControlStageHeader.WarehouseCode, PkgControlStageHeader.BinNum);
                                }
                                else if (PkgControlHeader != null && PkgControlHeader.PkgControlStatus.Equals("STOCK", StringComparison.OrdinalIgnoreCase))
                                {
                                    // Set to location based on PkgControlHeader location
                                    SetToWhseBinFromPCID(ref ttIssueReturn, Session.CompanyID, PkgControlHeader.PkgControlStatus, PkgControlHeader.WarehouseCode, PkgControlHeader.BinNum);
                                }

                                break;
                            }
                        default:
                            {
                                // Locate PkgControlHeader
                                PkgControlHeader = FindFirstPkgControlHeader(Session.CompanyID, opPCID);
                                if (PkgControlHeader == null)
                                {
                                    throw new BLException(Strings.PCIDNotFound(opPCID));
                                }

                                // Check status and custnum/shipto
                                if (PkgControlHeader.PkgControlType.Equals("STATIC", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (PkgControlHeader.PkgControlStatus.Equals("BUSY", StringComparison.OrdinalIgnoreCase))
                                    {
                                        pcidCheckCustShipTo(PkgControlHeader, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel, opPCID, opPCID);
                                    }
                                    else if (!PkgControlHeader.PkgControlStatus.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                                    {
                                        throw new BLException(Strings.PCIDStatusInvalid(opPCID));
                                    }
                                }
                                else
                                {
                                    if (PkgControlHeader.PkgControlStatus.Equals("SOPICK", StringComparison.OrdinalIgnoreCase))
                                    {
                                        pcidCheckCustShipTo(PkgControlHeader, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel, opPCID, opPCID);
                                    }
                                    else if (!PkgControlHeader.PkgControlStatus.Equals("EMPTY", StringComparison.OrdinalIgnoreCase) &&
                                             !PkgControlHeader.PkgControlStatus.Equals("TFOPICK", StringComparison.OrdinalIgnoreCase))
                                    {
                                        throw new BLException(Strings.PCIDStatusInvalid(opPCID));
                                    }
                                }

                                // Set to location based on PkgControlHeader location
                                SetToWhseBinFromPCID(ref ttIssueReturn, Session.CompanyID, PkgControlHeader.PkgControlStatus, PkgControlHeader.WarehouseCode, PkgControlHeader.BinNum);

                                break;
                            }
                    }
                }

                bool isPCIDChanged = (!ttIssueReturn.ToPCID.KeyEquals(opPCID));
                if (PkgControlHeader != null)
                {
                    ttIssueReturn.ToPCID = PkgControlHeader.PCID;
                }
                else if (PkgControlStageHeader != null)
                {
                    ttIssueReturn.ToPCID = PkgControlStageHeader.PCID;
                }

                if (isPCIDChanged)
                {
                    this.enableSNButton(ttIssueReturn, pCallProcess);
                    if (!ttIssueReturn.EnableSN) { this.checkStatusTracking(); }
                }
            }
        }

        private void SetToWhseBinFromPCID(ref IssueReturnRow ttIssueReturn, string company, string pkgControlStatus, string pkgControlWarehouseCode, string pkgControlBinNum)
        {
            if (!ttIssueReturn.TranType.Equals("STK-SHP", StringComparison.OrdinalIgnoreCase)
                && !ttIssueReturn.TranType.Equals("CMP-SHP", StringComparison.OrdinalIgnoreCase)
                && !ttIssueReturn.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(pkgControlWarehouseCode))
                {
                    Warehse warehse = FindFirstWarehse(company, pkgControlWarehouseCode);
                    if (warehse != null)
                    {
                        ttIssueReturn.ToWarehouseCode = warehse.WarehouseCode;
                        ttIssueReturn.ToWarehouseCodeDescription = warehse.Description;
                    }
                }
                if (!String.IsNullOrEmpty(pkgControlBinNum))
                {
                    WhseBin whseBin = FindFirstWhseBin(company, pkgControlWarehouseCode, pkgControlBinNum);
                    if (whseBin != null)
                    {
                        ttIssueReturn.ToBinNum = whseBin.BinNum;
                        ttIssueReturn.ToBinNumDescription = whseBin.Description;
                    }
                }
                else
                {
                    ttIssueReturn.ToBinNum = "";
                    ttIssueReturn.ToBinNumDescription = "";
                }
            }

            if (ttIssueReturn.TranType.Equals("STK-SHP", StringComparison.OrdinalIgnoreCase)
                || ttIssueReturn.TranType.Equals("CMP-SHP", StringComparison.OrdinalIgnoreCase)
                || ttIssueReturn.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))
            {
                // A PCID with contents should use the current PCID location, an empty PCID should use the whs/bin specified on the related MtlQueue record. 
                if (!pkgControlStatus.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                {
                    ttIssueReturn.ToWarehouseCode = string.Empty;
                    ttIssueReturn.ToWarehouseCodeDescription = string.Empty;
                    ttIssueReturn.ToBinNum = string.Empty;
                    ttIssueReturn.ToBinNumDescription = string.Empty;
                    if (!string.IsNullOrEmpty(pkgControlWarehouseCode))
                    {
                        Warehse warehse = FindFirstWarehse(company, pkgControlWarehouseCode);
                        if (warehse != null)
                        {
                            ttIssueReturn.ToWarehouseCode = warehse.WarehouseCode;
                            ttIssueReturn.ToBinNum = pkgControlBinNum;
                        }
                    }
                }
                else
                {
                    SetToWhsBinFromMtlQueue(ref ttIssueReturn);
                }
            }
            SetToWhsBinDescriptions(ref ttIssueReturn);
        }

        private void SetToWhsBinFromMtlQueue(ref IssueReturnRow ttIssueReturn)
        {
            if (ttIssueReturn.MtlQueueRowId != Guid.Empty)
            {
                var PartialRowMtlQueue = this.FindFirstMtlQueueToWhseBin(ttIssueReturn.MtlQueueRowId);
                if (PartialRowMtlQueue != null)
                {
                    ttIssueReturn.ToWarehouseCode = PartialRowMtlQueue.ToWhse;
                    ttIssueReturn.ToBinNum = PartialRowMtlQueue.ToBinNum;
                }
            }
        }

        private void SetToWhsBinDescriptions(ref IssueReturnRow ttIssueReturn)
        {
            ttIssueReturn.ToWarehouseCodeDescription = !String.IsNullOrEmpty(ttIssueReturn.ToWarehouseCode) ?
                                                        FindFirstWarehseDescription(ttIssueReturn.Company, ttIssueReturn.ToWarehouseCode)
                                                        : string.Empty;
            ttIssueReturn.ToBinNumDescription = ((!String.IsNullOrEmpty(ttIssueReturn.ToWarehouseCode)) && !String.IsNullOrEmpty(ttIssueReturn.ToBinNum))
                                                ? FindFirstWhseBinDescription(ttIssueReturn.Company, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum)
                                                : string.Empty;
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.FromAssemblySeq changes.
        /// </summary>
        /// <param name="piFromAssemblySeq">From Assembly Seq</param>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pCallProcess">Calling Process value</param>
        public void OnChangeFromAssemblySeq(int piFromAssemblySeq, ref IssueReturnTableset ds, string pCallProcess)
        {
            CurrentFullTableset = ds;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                this.determineSensitivity(ttIssueReturn);
                ttIssueReturn.FromAssemblySeq = piFromAssemblySeq;
                ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
                this.onChangeFromAssemblySeqCore(ttIssueReturn);
                ttIssueReturn.RowMod = IceRow.ROWSTATE_UNCHANGED;
                this.FillForeignFields(ttIssueReturn);
                this.enableSNButton(ttIssueReturn, pCallProcess);
                this.checkStatusTracking();
                this.SetQuantity();
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void onChangeFromAssemblySeqCore(IssueReturnRow ttIssueReturn)
        {
            this.validateAssemblySeq(ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.TranType, ttIssueReturn.SysRowID, "FromAssemblySeq", "From");
            ExceptionManager.AssertNoBLExceptions();

            ttIssueReturn.TreeDisplay = ttIssueReturn.FromJobNum + " : " + Compatibility.Convert.ToString(ttIssueReturn.FromAssemblySeq);
            ttIssueReturn.FromJobSeq = 0;
            ttIssueReturn.FromJobSeqPartNum = "";
            ttIssueReturn.FromJobSeqPartDescription = "";
            ttIssueReturn.FromWarehouseCode = "";
            ttIssueReturn.FromWarehouseCodeDescription = "";
            ttIssueReturn.FromBinNum = "";
            ttIssueReturn.FromBinNumDescription = "";
            ttIssueReturn.FromPCID = "";

            this.getFromJobSeq(ttIssueReturn, true, true);
            this.getFromAssemblySeq(ttIssueReturn);
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.FromBinNum changes.
        /// </summary>
        /// <param name="ds">IssueReturnDataSet</param>
        public void OnChangeFromBinNum(ref IssueReturnTableset ds)
        {
            CurrentFullTableset = ds;
            bool lConsiderPartNum = false;
            string InvtyUOM = string.Empty;
            decimal outOnHandQty = decimal.Zero;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                /* determineSensitivity builds the list of fields which are applicable for the TranType */
                this.determineSensitivity(ttIssueReturn);
                lConsiderPartNum = (this.getFromType(ttIssueReturn.TranType).KeyEquals("STK"));
                /* scr 47962 set lconsiderPartNum to no as this could be a shared warehouse */
                if (ttIssueReturn.TranType.Equals("STK-MTL", StringComparison.OrdinalIgnoreCase))
                {
                    lConsiderPartNum = false;
                }

                this.validateWareHouseCodeBinNum(ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.PartNum, lConsiderPartNum, ttIssueReturn.SysRowID, "From");
                ExceptionManager.AssertNoBLExceptions();

                /* Begin Get-Bin */
                ttIssueReturn.FromBinNumDescription = "";

                WhseBin = this.FindFirstWhseBin(Session.CompanyID, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum);
                if (WhseBin != null)
                {
                    ttIssueReturn.FromBinNumDescription = WhseBin.Description;
                }
                /* End Get-Bin */
                /* if IssueMtl, or IssueAsm or IssueMisc */
                if (ttIssueReturn.TranType.Equals("STK-MTL", StringComparison.OrdinalIgnoreCase)
                    || ttIssueReturn.TranType.Equals("STK-ASM", StringComparison.OrdinalIgnoreCase)
                    || ttIssueReturn.TranType.Equals("STK-UKN", StringComparison.OrdinalIgnoreCase))
                {
                    if (ttIssueReturn.EnableSN
                        && ExistsBinToBinReqSNPlantConfCtrl(ttIssueReturn.Company, ttIssueReturn.FromJobPlant, true))
                    {
                        // the user has been warned on bin change that the selected SN will be deselected due to the bin change.
                        if ((from ttSelectedSerialNumbers_Row in ds.SelectedSerialNumbers
                             where ttSelectedSerialNumbers_Row.Company.KeyEquals(ttIssueReturn.Company)
                             && !String.IsNullOrEmpty(ttSelectedSerialNumbers_Row.RowMod)
                             select ttSelectedSerialNumbers_Row).Any())
                        {
                            this.checkStatusTracking();
                        }

                        foreach (var ttSelectedSerialNumbers in (from ttSelectedSerialNumbers_Row in CurrentFullTableset.SelectedSerialNumbers
                                                                 where ttSelectedSerialNumbers_Row.Company.KeyEquals(ttIssueReturn.Company)
                                                                 && !String.IsNullOrEmpty(ttSelectedSerialNumbers_Row.RowMod)
                                                                 select ttSelectedSerialNumbers_Row))
                        {
                            if (ExistsSerialNo(Session.CompanyID, ttIssueReturn.PartNum, ttSelectedSerialNumbers.SerialNumber, ttIssueReturn.FromBinNum)
                                && ttSelectedSerialNumbers.SourceRowID == Guid.Empty)
                            {
                                // need to re-associate the rows auto-deselected by checkStatusTracking method for a previous bin so that they will show up 
                                // as available to be re-selected if the user switches back to the prior bin. 
                                ttSelectedSerialNumbers.SourceRowID = ttIssueReturn.SysRowID;
                            }
                        }
                    } // EnableSN and BinToBin requires SN
                }/* if IssueMtl, or IssueAsm or IssueMisc */
                this.LibAppService.GetOnHandQty(ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode, "", ttIssueReturn.FromBinNum, ttIssueReturn.UM, "", ttIssueReturn.AttributeSetID, true, true, out outOnHandQty);
                ttIssueReturn.OnHandQty = outOnHandQty;
                this.FillForeignFields(ttIssueReturn);
                this.SetQuantity();
            }
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.FromJobNum changes.
        /// </summary>
        /// <param name="pcFromJobNum">From Job Number</param>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pCallProcess">Calling Process value</param>
        public void OnChangeFromJobNum(string pcFromJobNum, ref IssueReturnTableset ds, string pCallProcess)
        {
            CurrentFullTableset = ds;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                this.determineSensitivity(ttIssueReturn);
                ttIssueReturn.FromJobNum = pcFromJobNum;
                this.onChangeFromJobNumCore(ttIssueReturn);
                if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                {
                    ttIssueReturn.ToJobNum = ttIssueReturn.FromJobNum;
                    ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
                    ttIssueReturn.ToJobSeq = ttIssueReturn.FromJobSeq;
                }
                ttIssueReturn.RowMod = IceRow.ROWSTATE_UNCHANGED;
                this.FillForeignFields(ttIssueReturn);
                this.enableSNButton(ttIssueReturn, pCallProcess);
                this.checkStatusTracking();
                this.SetQuantity();
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void onChangeFromJobNumCore(IssueReturnRow ttIssueReturn)
        {
            this.validateJobNum(ttIssueReturn.FromJobNum, ttIssueReturn.SysRowID, "FromJobNum", "From", ttIssueReturn.ProcessID);
            ExceptionManager.AssertNoBLExceptions();

            ttIssueReturn.FromAssemblySeq = 0;
            ttIssueReturn.FromJobSeq = 0;
            ttIssueReturn.FromAssemblyPartNum = "";
            ttIssueReturn.FromAssemblyPartDescription = "";
            ttIssueReturn.FromAssemblyRevisionNum = "";
            ttIssueReturn.FromJobSeqPartNum = "";
            ttIssueReturn.FromJobSeqPartDescription = "";
            ttIssueReturn.FromWarehouseCode = "";
            ttIssueReturn.FromWarehouseCodeDescription = "";
            ttIssueReturn.FromBinNum = "";
            ttIssueReturn.FromBinNumDescription = "";

            JobHead = this.FindFirstJobHead(Session.CompanyID, ttIssueReturn.FromJobNum);
            if (JobHead == null)
            {
                throw new BLException(Strings.AValidFromJobNumberIsRequired, "IssueReturn", "FromJobNum");
            }
            if (JobHead != null)
            {
                ttIssueReturn.FromJobPartNum = JobHead.PartNum;
                ttIssueReturn.FromJobPartDescription = JobHead.PartDescription;
                ttIssueReturn.FromJobPlant = JobHead.Plant;
                ttIssueReturn.FromJobRevisionNum = JobHead.RevisionNum;
            }
            this.getFromAssemblySeq(ttIssueReturn);
            this.getFromJobSeq(ttIssueReturn, true, true);
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.FromJobNum changes.
        /// </summary>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pCallProcess">Calling Process value</param>
        /// <param name="pcMessage">Non-error, informational message</param>
        public void OnChangeFromJobSeq(ref IssueReturnTableset ds, string pCallProcess, out string pcMessage)
        {
            pcMessage = string.Empty;
            CurrentFullTableset = ds;
            string cFromType = string.Empty;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                this.determineSensitivity(ttIssueReturn);
                /* Validation: FromJobSeq */
                cFromType = this.getFromType(ttIssueReturn.TranType);
                this.validateJobSeq(ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq, cFromType, ttIssueReturn.SysRowID, "FromJobSeq", "From");
                ExceptionManager.AssertNoBLExceptions();

                this.getFromJobSeq(ttIssueReturn, true, true);
                if (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-MTL") == 0)
                {
                    ttIssueReturn.ToJobNum = ttIssueReturn.FromJobNum;
                    this.onChangeToJobNumCore(ttIssueReturn, out pcMessage);
                    ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
                    this.onChangeToAssemblySeqCore(ttIssueReturn);
                    ttIssueReturn.ToJobSeq = ttIssueReturn.FromJobSeq;
                    this.onChangeToJobSeqCore(ttIssueReturn, out pcMessage);
                    /* Clear the TO Warehouse/Bin fields */
                    ttIssueReturn.ToWarehouseCodeDescription = "";
                    ttIssueReturn.ToWarehouseCode = "";
                    ttIssueReturn.ToBinNum = "";
                    ttIssueReturn.ToBinNumDescription = "";
                    ttIssueReturn.ToPCID = "";
                }/* if ttIssueReturn.TranType */


                this.FillForeignFields(ttIssueReturn);
                this.enableSNButton(ttIssueReturn, pCallProcess);
                this.checkStatusTracking();
                this.SetQuantity();
            }
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.PartNum changes.
        /// </summary>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pCallProcess">Calling Process value</param>
        public void OnChangeFromWarehouse(ref IssueReturnTableset ds, string pCallProcess)
        {
            CurrentFullTableset = ds;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                PlantWhse = this.FindFirstPlantWhse3(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode);
                if (PlantWhse != null)
                {
                    WhseBin = this.FindFirstWhseBin(Session.CompanyID, ttIssueReturn.FromWarehouseCode, PlantWhse.PrimBin);
                }
                ttIssueReturn.RowMod = IceRow.ROWSTATE_UNCHANGED;
                ttIssueReturn.FromPCID = "";
                ttIssueReturn.FromBinNum = ((PlantWhse != null) ? PlantWhse.PrimBin : "");
                ttIssueReturn.FromBinNumDescription = ((WhseBin != null) ? WhseBin.Description : "");
                this.FillForeignFields(ttIssueReturn);
                this.enableSNButton(ttIssueReturn, pCallProcess);
                this.checkStatusTracking();
                this.SetQuantity();
            }
            else
            {
                throw new BLException(Strings.RecordNotFound, "ttIssueReturn", "RowMod");
            }
        }

        /// <summary>
        /// Call when from warehouse changes.  This method will also reset the bin to the default value for the warehouse
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="pCallProcess"></param>
        public void ChangeFromWarehouseDefaultBin(ref IssueReturnTableset ds, string pCallProcess)
        {
            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();

            if (ttIssueReturn != null)
            {
                ttIssueReturn.FromBinNum = String.Empty;
                ttIssueReturn.FromBinNumDescription = String.Empty;

                using (var issueReturnService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.IssueReturnSvcContract>(Db))
                {
                    issueReturnService.OnChangeFromWarehouse(ref ds, pCallProcess);
                }
            }
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.LotNum changes.
        /// </summary>
        /// <param name="pcLotNum">Proposed LotNum value</param>
        /// <param name="ds">IssueReturnDataSet</param>
        public void OnChangeLotNum(string pcLotNum, ref IssueReturnTableset ds)
        {
            CurrentFullTableset = ds;
            //bool lConsiderPartNum = false;
            string InvtyUOM = string.Empty;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                bool lotIsChanged = (ttIssueReturn.LotNum.KeyEquals(pcLotNum));
                ttIssueReturn.LotNum = pcLotNum;
                /* determineSensitivity builds the list of fields which are applicable for the TranType */
                this.determineSensitivity(ttIssueReturn);
                /* Validate Lot Number */
                this.validateLotNum(ttIssueReturn.PartTrackLots, ttIssueReturn.PartNum, ttIssueReturn.LotNum, ttIssueReturn.SysRowID);
                ExceptionManager.AssertNoBLExceptions();
                if (lotIsChanged && ttIssueReturn.EnableSN && !string.IsNullOrEmpty(ttIssueReturn.FromPCID))
                {
                    this.checkStatusTracking();
                }

                this.FillForeignFields(ttIssueReturn);
                this.SetQuantity();
            }
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.PartNum changes.
        /// </summary>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pCallProcess">Calling Process value</param>
        public void OnChangePartNum(ref IssueReturnTableset ds, string pCallProcess)
        {
            CurrentFullTableset = ds;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                this.determineSensitivity(ttIssueReturn);
                /* Validation: PartNum */
                this.validatePartNum(ttIssueReturn);
                ExceptionManager.AssertNoBLExceptions();

                if (!String.IsNullOrEmpty(ttIssueReturn.FromPCID))
                {
                    using (Internal.Lib.PackageControlValidations libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
                    {
                        if (!libPackageControlValidations.ValidatePartExistsIsInPCID(Session.CompanyID, ttIssueReturn.FromPCID, ttIssueReturn.PartNum))
                        {
                            throw new BLException(Strings.PartNumIsNotItemPCID);
                        }

                    }
                }
                else
                {
                    var PrimBin = this.FindFirstPlantWhse(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode);
                    if (!string.IsNullOrEmpty(PrimBin))
                    {
                        var Description = this.FindFirstWhseBinDescription(Session.CompanyID, ttIssueReturn.FromWarehouseCode, PrimBin);
                        ttIssueReturn.FromBinNum = PrimBin;

                        if (!string.IsNullOrEmpty(Description))
                        {
                            ttIssueReturn.FromBinNumDescription = Description;
                        }
                    }
                    else
                    {
                        ttIssueReturn.FromBinNumDescription = String.Empty;
                    }    /* if available PlantWhse */
                }

                // Reset certain fields
                ttIssueReturn.LotNum = String.Empty;
                ttIssueReturn.AttributeSetID = 0;
                ttIssueReturn.RevisionNum = String.Empty;

                this.onChangePartNumCore(ttIssueReturn);
                ttIssueReturn.RowMod = IceRow.ROWSTATE_UNCHANGED;
                this.FillForeignFields(ttIssueReturn);
                this.enableSNButton(ttIssueReturn, pCallProcess);
                this.checkStatusTracking();
                this.SetQuantity();
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void onChangePartNumCore(IssueReturnRow ttIssueReturn)
        {
            string cFromType = this.getFromType(ttIssueReturn.TranType);
            string cToType = this.getToType(ttIssueReturn.TranType);
            string pUOM = string.Empty;
            /* Begin Get-Part */
            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part == null)
            {
                return;
            }
            ttIssueReturn.PartNum = Part.PartNum;

            PartPlant = this.FindFirstPartPlant(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum);

            ttIssueReturn.PartPartDescription = Part.PartDescription;
            ttIssueReturn.UM = Part.IUM;
            ttIssueReturn.OnHandUM = Part.IUM;
            ttIssueReturn.TrackDimension = Part.TrackDimension;
            ttIssueReturn.PartTrackInventoryAttributes = Part.TrackInventoryAttributes;
            ttIssueReturn.PartTrackInventoryByRevision = Part.TrackInventoryByRevision;
            ttIssueReturn.PartAttrClassID = Part.AttrClassID;
            ttIssueReturn.EnableAttributeSetSearch = (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedUnitOfMeasure) && Part.TrackInventoryAttributes);
            ttIssueReturn.AttributeSetID = ttIssueReturn.EnableAttributeSetSearch ? Part.DefaultAttributeSetID : 0;

            if (ttIssueReturn.AttributeSetID == 0)
            {
                using (var inventoryTracking = new InventoryTracking(Db))
                {
                    inventoryTracking.DoPartRev(Part.TrackInventoryByRevision, ref ttIssueReturn);
                    inventoryTracking.UpdateNumberOfPieces(ref ttIssueReturn);
                }
            }
            GetAttributeDescriptions(ref ttIssueReturn);

            if (StringExtensions.Lookup("ASM-STK,MTL-STK,UKN-STK", ttIssueReturn.TranType) > -1)
            {
                /* clear TO wareshouse/bin getToWhse will get new default */
                ttIssueReturn.ToWarehouseCodeDescription = "";
                ttIssueReturn.ToWarehouseCode = "";
                ttIssueReturn.ToBinNum = "";
                ttIssueReturn.ToBinNumDescription = "";
                ttIssueReturn.ToPCID = "";
            }
            if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-UKN") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "UKN-STK") == 0)
            {
                ttIssueReturn.RequirementUOM = Part.IUM;
                ttIssueReturn.DimCode = Part.IUM;
            }
            pUOM = Part.IUM;
            var altJobMtl = this.FindFirstJobMtl3(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq);
            if (altJobMtl != null)
            {
                if (!altJobMtl.PartNum.KeyEquals(ttIssueReturn.PartNum))
                {
                    pUOM = altJobMtl.IUM;
                }
            }
            this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, pUOM, out ConvQty, false);
            ttIssueReturn.RequirementQty = ConvQty;

            /* Set-IssuedComplete Logic */
            this.setIssuedComplete(ttIssueReturn);
            if (!String.IsNullOrEmpty(ttIssueReturn.FromPCID))
            {
                using (Internal.Lib.PackageControlValidations libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
                {
                    if (!libPackageControlValidations.ValidatePartExistsIsInPCID(Session.CompanyID, ttIssueReturn.FromPCID, ttIssueReturn.PartNum))
                    {
                        throw new BLException(Strings.PartNumIsNotItemPCID);
                    }

                }
            }
            else
            {
                this.getFromWhse(ttIssueReturn);
            }
            this.getToWhse(ttIssueReturn);
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.ToAssemblySeq changes.
        /// </summary>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pCallProcess">Calling Process value</param>
        public void OnChangeToAssemblySeq(ref IssueReturnTableset ds, string pCallProcess)
        {
            CurrentFullTableset = ds;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                this.determineSensitivity(ttIssueReturn);
                this.onChangeToAssemblySeqCore(ttIssueReturn);
                ttIssueReturn.RowMod = IceRow.ROWSTATE_UNCHANGED;
                this.FillForeignFields(ttIssueReturn);
                this.enableSNButton(ttIssueReturn, pCallProcess);
                this.checkStatusTracking();
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void onChangeToAssemblySeqCore(IssueReturnRow ttIssueReturn)
        {
            string cToType = string.Empty;
            this.validateAssemblySeq(ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.TranType, ttIssueReturn.SysRowID, "ToAssemblySeq", "To");
            ExceptionManager.AssertNoBLExceptions();

            cToType = this.getToType(ttIssueReturn.TranType);
            ttIssueReturn.TreeDisplay = ttIssueReturn.ToJobNum + " : " + Compatibility.Convert.ToString(ttIssueReturn.ToAssemblySeq);
            ttIssueReturn.ToJobSeq = 0;
            ttIssueReturn.ToJobSeqPartDescription = "";
            ttIssueReturn.ToJobSeqPartNum = "";

            /* Begin Get-JobAsm */
            /* CHECK FOR JobAsmbl EXISTANCE */
            JobAsmbl = this.FindFirstJobAsmbl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq);
            if (JobAsmbl != null)
            {
                if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-ASM") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-WIP") == 0)
                {
                    ttIssueReturn.PartNum = JobAsmbl.PartNum;            /* Uses the onChangePartNumCore rountine to set defaults from part */
                    this.onChangePartNumCore(ttIssueReturn);
                }
                ttIssueReturn.ToAssemblyPartNum = JobAsmbl.PartNum;
                ttIssueReturn.ToAssemblyPartDesc = JobAsmbl.Description;
                /* Begin Disp-RequiredQty */
                if (StringExtensions.Compare(cToType, "MTL") == 0)
                {
                    ttIssueReturn.QtyPreviouslyIssued = 0;
                    ttIssueReturn.QtyRequired = 0;            /* CHECK FOR JobMtl EXISTANCE */

                    JobMtl = this.FindFirstJobMtl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq);
                    if (JobMtl != null)
                    {
                        ttIssueReturn.QtyPreviouslyIssued = JobMtl.IssuedQty;
                        ttIssueReturn.QtyRequired = JobMtl.RequiredQty;
                        var outQtyRequired5 = ttIssueReturn.QtyRequired;
                        this.LibAppService.RoundToUOMDec(ttIssueReturn.RequirementUOM, ref outQtyRequired5);
                        ttIssueReturn.QtyRequired = outQtyRequired5;
                        ttIssueReturn.ReassignSNAsm = JobMtl.ReassignSNAsm;
                        //If the Attribute Set does not already exist and one does exist on JobMtl, pull it in and set the descriptions accordingly
                        if (ttIssueReturn.AttributeSetID == 0 && JobMtl.AttributeSetID != 0)
                        {
                            ttIssueReturn.AttributeSetID = JobMtl.AttributeSetID;
                            this.GetAttributeDescriptions(ref ttIssueReturn);
                        }
                        ttIssueReturn.RevisionNum = JobMtl.RevisionNum;
                    }
                }/* if cToType = "MTL":U*/
                if (StringExtensions.Compare(cToType, "ASM") == 0)
                {
                    ttIssueReturn.QtyPreviouslyIssued = JobAsmbl.IssuedQty;
                    ttIssueReturn.QtyRequired = JobAsmbl.PullQty;
                    var outRequirementUOM2 = ttIssueReturn.RequirementUOM;
                    this.LibAppService.DefaultTransUOM(ttIssueReturn.PartNum, ttIssueReturn.UM, out outRequirementUOM2);
                    ttIssueReturn.RequirementUOM = outRequirementUOM2;
                    ttIssueReturn.ReassignSNAsm = JobAsmbl.ReassignSNAsm;
                    //If the Attribute Set does not already exist and one does exist on JobAsmbl, pull it in and set the descriptions accordingly
                    if (ttIssueReturn.AttributeSetID == 0 && JobAsmbl.AttributeSetID != 0)
                    {
                        ttIssueReturn.AttributeSetID = JobAsmbl.AttributeSetID;
                        this.GetAttributeDescriptions(ref ttIssueReturn);
                    }
                    ttIssueReturn.RevisionNum = JobAsmbl.RevisionNum;
                }/* if cToType = "ASM":U */

                ttIssueReturn.QtyPreviouslyIssued = ttIssueReturn.QtyPreviouslyIssued;
                /* Set-IssuedComplete */
                this.setIssuedComplete(ttIssueReturn);
            }
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.ToJobNum changes.
        /// </summary>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pCallProcess">Calling Process value</param>
        /// <param name="pcMessage">Non-Error, informational message</param>
        public void OnChangeToJobNum(ref IssueReturnTableset ds, string pCallProcess, out string pcMessage)
        {
            pcMessage = string.Empty;
            CurrentFullTableset = ds;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                this.determineSensitivity(ttIssueReturn);
                /* Validation: ToJobNum */
                this.validateJobNum(ttIssueReturn.ToJobNum, ttIssueReturn.SysRowID, "ToJobNum", "To", ttIssueReturn.ProcessID);
                ExceptionManager.AssertNoBLExceptions();

                this.onChangeToJobNumCore(ttIssueReturn, out pcMessage);
                ttIssueReturn.RowMod = IceRow.ROWSTATE_UNCHANGED;
                this.FillForeignFields(ttIssueReturn);
                this.enableSNButton(ttIssueReturn, pCallProcess);
                this.checkStatusTracking();
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void onChangeToJobNumCore(IssueReturnRow ttIssueReturn, out string pcMessage)
        {
            pcMessage = string.Empty;
            string cToType = string.Empty;
            cToType = this.getToType(ttIssueReturn.TranType);

            JobHead = this.FindFirstJobHead(Session.CompanyID, ttIssueReturn.ToJobNum);
            if (JobHead == null)
            {
                throw new BLException(Strings.AValidToJobNumberIsRequired, "IssueReturn", "ToJobNum");
            }
            ttIssueReturn.ToAssemblySeq = 0;
            ttIssueReturn.ToJobSeq = 0;
            ttIssueReturn.ToAssemblyPartDescription = "";
            ttIssueReturn.ToJobSeqPartDescription = "";
            ttIssueReturn.ToAssemblyPartNum = "";
            ttIssueReturn.ToJobSeqPartNum = "";
            ttIssueReturn.ToJobPartNum = JobHead.PartNum;
            ttIssueReturn.ToJobPartDescription = JobHead.PartDescription;
            ttIssueReturn.ToJobPlant = JobHead.Plant;

            /* Begin Get-JobAsm */
            /* CHECK FOR JobAsmbl EXISTANCE */
            JobAsmbl = this.FindFirstJobAsmbl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq);
            if (JobAsmbl != null)
            {
                if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-ASM") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-WIP") == 0)
                {
                    ttIssueReturn.PartNum = JobAsmbl.PartNum;            /* Uses the onChangePartNumCore rountine to set defaults from part */
                    this.onChangePartNumCore(ttIssueReturn);
                }
                ttIssueReturn.ToAssemblyPartNum = JobAsmbl.PartNum;
                ttIssueReturn.ToAssemblyPartDesc = JobAsmbl.Description;
                if (StringExtensions.Compare(cToType, "MTL") == 0)
                {
                    ttIssueReturn.QtyPreviouslyIssued = 0;
                    ttIssueReturn.QtyRequired = 0;            /* CHECK FOR JobMtl EXISTANCE */

                    JobMtl = this.FindFirstJobMtl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq);
                    if (JobMtl != null)
                    {
                        ttIssueReturn.QtyPreviouslyIssued = JobMtl.IssuedQty;
                        ttIssueReturn.QtyRequired = JobMtl.RequiredQty;
                        var outQtyRequired6 = ttIssueReturn.QtyRequired;
                        this.LibAppService.RoundToUOMDec(ttIssueReturn.RequirementUOM, ref outQtyRequired6);
                        ttIssueReturn.QtyRequired = outQtyRequired6;
                    }
                }/* if cToType = "MTL":U*/

                if (StringExtensions.Compare(cToType, "ASM") == 0)
                {
                    ttIssueReturn.QtyPreviouslyIssued = 0;
                    ttIssueReturn.QtyRequired = 0;

                    JobAsmbl = this.FindFirstJobAsmbl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq);
                    if (JobAsmbl != null)
                    {
                        ttIssueReturn.QtyPreviouslyIssued = JobAsmbl.IssuedQty;
                        ttIssueReturn.QtyRequired = JobAsmbl.PullQty;
                    }
                }/* if cToType = "ASM":U */

                /* End Disp-RequiredQty */

                /* Set-IssuedComplete */
                this.setIssuedComplete(ttIssueReturn);
            }/* if available JobAsmbl  End Disp-JobAsm */
            /* Disp-JobAsm */
            /* End Get-JobAsm */

            this.getToJobSeq(ttIssueReturn, true, true, out pcMessage);
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.ToJobSeq changes.
        /// </summary>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pCallProcess">Calling Process value</param>
        /// <param name="pcMessage">Non-Error, informational message</param>
        public void OnChangeToJobSeq(ref IssueReturnTableset ds, string pCallProcess, out string pcMessage)
        {
            pcMessage = string.Empty;
            CurrentFullTableset = ds;
            string cToType = string.Empty;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                this.determineSensitivity(ttIssueReturn);
                cToType = this.getToType(ttIssueReturn.TranType);
                /* Validation: ToJobSeq */
                this.validateJobSeq(ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq, cToType, ttIssueReturn.SysRowID, "ToJobSeq", "To");
                ExceptionManager.AssertNoBLExceptions();

                /* need to clear warehouse field to by pass condition in getFromWhse */
                ttIssueReturn.FromWarehouseCode = "";
                ttIssueReturn.FromBinNum = "";
                this.onChangeToJobSeqCore(ttIssueReturn, out pcMessage);
                this.FillForeignFields(ttIssueReturn);
                this.enableSNButton(ttIssueReturn, pCallProcess);
                this.checkStatusTracking();
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void onChangeToJobSeqCore(IssueReturnRow ttIssueReturn, out string pcMessage)
        {
            pcMessage = string.Empty;
            string cToType = string.Empty;
            cToType = this.getToType(ttIssueReturn.TranType);
            this.getToJobSeq(ttIssueReturn, true, true, out pcMessage);
            this.setIssuedComplete(ttIssueReturn);
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.PartNum changes.
        /// </summary>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pCallProcess">Calling Process value</param>
        public void OnChangeToWarehouse(ref IssueReturnTableset ds, string pCallProcess)
        {
            CurrentFullTableset = ds;

            //here we try to remove all rows with empty RowMod but only those
            //rows, which have at least one corresponding row (by SysRowID) with non-empty
            //RowMod. it is done so to avoid the situation when server misresponse
            //is treated as a command to delete a row (when only one incoming row with
            //specific SysRowID and empty RowMod).
            int nRows = ds.IssueReturn.Count;
            Dictionary<int, int> indices = new Dictionary<int, int>(nRows); //indices to remove
            for (int iRow1 = 0; iRow1 < nRows; iRow1++)
            {
                IssueReturnRow r1 = ds.IssueReturn[iRow1];
                if (!string.IsNullOrEmpty(r1.RowMod)) continue;

                for (int iRow2 = 0; iRow2 < nRows; iRow2++)
                {
                    if (iRow1 == iRow2) continue;

                    IssueReturnRow r2 = ds.IssueReturn[iRow2];
                    if (r1.SysRowID != r2.SysRowID) continue;
                    if (string.IsNullOrEmpty(r2.RowMod)) continue;

                    if (indices.ContainsKey(iRow1)) continue;
                    indices.Add(iRow1, iRow1);
                }
            }
            //indices computed

            //and clean the table
            foreach (KeyValuePair<int, int> kvp in indices)
                ds.IssueReturn.RemoveAt(kvp.Value);

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                string sRowMod = ttIssueReturn.RowMod; //store RowMod
                ttIssueReturn.ToPCID = "";
                ttIssueReturn.ToBinNum = "";
                ttIssueReturn.RowMod = IceRow.ROWSTATE_UNCHANGED;

                this.FillForeignFields(ttIssueReturn);
                this.enableSNButton(ttIssueReturn, pCallProcess);
                this.checkStatusTracking();
                this.checkWhseBin(ttIssueReturn);

                ttIssueReturn.RowMod = sRowMod; //restore RowMod before sending to client
            }
        }

        /// <summary>
        /// Call when to warehouse changes.  This method will also reset the bin to the default value for the warehouse
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="pCallProcess"></param>
        public void ChangeToWarehouseDefaultBin(ref IssueReturnTableset ds, string pCallProcess)
        {
            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();

            if (ttIssueReturn != null)
            {
                ttIssueReturn.ToBinNum = String.Empty;
                ttIssueReturn.ToBinNumDescription = String.Empty;

                using (var issueReturnService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.IssueReturnSvcContract>(Db))
                {
                    issueReturnService.OnChangeToWarehouse(ref ds, pCallProcess);
                }
            }
        }

        /// <summary>
        /// Call this method when the Nbr Of Pieces changes to calculate a new tran qty
        /// </summary>
        /// <param name="numberOfPieces"></param>
        /// <param name="ds"></param>
        public void OnChangingNumberOfPieces(decimal numberOfPieces, ref IssueReturnTableset ds)
        {
            using (var inventoryTracking = new InventoryTracking(Db))
            {
                inventoryTracking.OnChangingNumberOfPieces(numberOfPieces, ref ds);
            }
        }

        /// <summary>
        /// Call this method when the attribute set changes
        /// </summary>
        /// <param name="attributeSetID"></param>
        /// <param name="ds"></param>
        public void OnChangingAttributeSet(int attributeSetID, ref IssueReturnTableset ds)
        {
            using (var inventoryTracking = new InventoryTracking(Db))
            {
                inventoryTracking.OnChangingAttributeSet(attributeSetID, ref ds);
            }
        }

        /// <summary>
        /// Call this method when the attribute set changes for adjustment transactions (Issue Misc, Return Misc) to maintain inventory tracking
        /// </summary>
        /// <param name="attributeSetID"></param>
        /// <param name="ds"></param>
        public void OnChangingAttributeSetAdjustments(int attributeSetID, ref IssueReturnTableset ds)
        {
            using (var inventoryTracking = new InventoryTracking(Db))
            {
                inventoryTracking.OnChangingAttributeSetAdjustments(attributeSetID, ref ds);
            }
        }

        /// <summary>
        /// Call this method when the Revision changes to maintain inventory tracking
        /// </summary>
        /// <param name="revisionNum"></param>
        /// <param name="ds"></param>
        public void OnChangingRevisionNum(string revisionNum, ref IssueReturnTableset ds)
        {
            using (var inventoryTracking = new InventoryTracking(Db))
            {
                inventoryTracking.OnChangingRevisionNum(revisionNum, ref ds);
            }
        }

        /// <summary>
        /// Call this method when the Revision changes for adjustment transactions (Issue Misc, Return Misc) to maintain inventory tracking
        /// </summary>
        /// <param name="revisionNum"></param>
        /// <param name="ds"></param>
        public void OnChangingRevisionNumAdjustments(string revisionNum, ref IssueReturnTableset ds)
        {
            using (var inventoryTracking = new InventoryTracking(Db))
            {
                inventoryTracking.OnChangingRevisionNumAdjustments(revisionNum, ref ds);
            }
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.TranQty changes.
        /// This method performs validation on TranQty and sets the Issued Complete flag.
        /// </summary>
        /// <param name="pdTranQty">Transaction Qty</param>
        /// <param name="ds">IssueReturnDataSet</param>
        public void OnChangeTranQty(decimal pdTranQty, ref IssueReturnTableset ds)
        {
            CurrentFullTableset = ds;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                ttIssueReturn.TranQty = pdTranQty;
                /* Validation: TranQty */
                this.validateTranQty(ttIssueReturn);
                ExceptionManager.AssertNoBLExceptions();
                using (var inventoryTracking = new InventoryTracking(Db))
                {
                    inventoryTracking.UpdateNumberOfPieces(ref ttIssueReturn);
                }

                /* scr50268 */
                this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, ttIssueReturn.RequirementUOM, out ConvQty, false);
                ttIssueReturn.RequirementQty = ConvQty;
                var outRequirementQty = ttIssueReturn.RequirementQty;
                this.LibAppService.RoundToUOMDec(ttIssueReturn.RequirementUOM, ref outRequirementQty);
                ttIssueReturn.RequirementQty = outRequirementQty;
                if (ttIssueReturn.TranType.Equals("ADJ-MTL", StringComparison.OrdinalIgnoreCase))
                    return;
                /* Set-IssuedComplete */
                this.setIssuedComplete(ttIssueReturn);
            }
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.UM changes.
        /// </summary>
        /// <param name="pUM">Transaction UOM</param>
        /// <param name="ds">IssueReturnDataSet</param>
        public void OnChangeUM(string pUM, ref IssueReturnTableset ds)
        {
            CurrentFullTableset = ds;
            string InvtyUOM = string.Empty;
            decimal outOnHandQty = decimal.Zero;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                ttIssueReturn.UM = pUM;
                ttIssueReturn.OnHandUM = pUM;
                this.LibAppService.GetOnHandQty(ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode, "", ttIssueReturn.FromBinNum, pUM, "", true, true, out outOnHandQty);
                ttIssueReturn.OnHandQty = outOnHandQty;
                this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, ttIssueReturn.RequirementUOM, out ConvQty, false);
                ttIssueReturn.RequirementQty = ConvQty;
                //Recalculate the number of pieces using the new UM.
                using (var inventoryTracking = new InventoryTracking(Db))
                {
                    inventoryTracking.UpdateNumberOfPieces(ref ttIssueReturn);
                }
                /* Set-IssuedComplete Logic */
                this.setIssuedComplete(ttIssueReturn);
            }
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.FromBinNum is changing.
        /// </summary>
        /// <param name="pUM">from unit of measure</param>        
        /// <param name="ds">IssueReturnDataSet</param> 
        public void OnChangeASTUom(string pUM, ref IssueReturnTableset ds)
        {
            CurrentFullTableset = ds;
            decimal convQty = decimal.Zero;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();

            if (ttIssueReturn == null)
            {
                throw new BLException(Strings.RecordNotFound, "IssueReturn");
            }

            this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, pUM, ttIssueReturn.UM, out convQty, false);
            ttIssueReturn.TranQty = convQty;

        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.FromBinNum is changing.
        /// </summary>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pcMessage"> Warning if serial numbers have already been selected for another bin.</param>
        public void OnChangingFromBinNum(ref IssueReturnTableset ds, out string pcMessage)
        {
            pcMessage = string.Empty;
            CurrentFullTableset = ds;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                if (ttIssueReturn.TranType.Equals("STK-MTL", StringComparison.OrdinalIgnoreCase)
                 || ttIssueReturn.TranType.Equals("STK-ASM", StringComparison.OrdinalIgnoreCase)
                 || ttIssueReturn.TranType.Equals("STK-UKN", StringComparison.OrdinalIgnoreCase))
                {
                    if ((ttIssueReturn.EnableSN)
                        && this.ExistsBinToBinReqSNPlantConfCtrl(ttIssueReturn.Company, ttIssueReturn.FromJobPlant, true))
                    {
                        foreach (var ttSelectedSerialNumbers in (from ttSelectedSerialNumbers_Row in CurrentFullTableset.SelectedSerialNumbers
                                                                 where ttSelectedSerialNumbers_Row.Company.KeyEquals(ttIssueReturn.Company) &&
                                                                 ttSelectedSerialNumbers_Row.Deselected == false &&
                                                                 !String.IsNullOrEmpty(ttSelectedSerialNumbers_Row.RowMod)
                                                                 select ttSelectedSerialNumbers_Row))
                        {
                            if (ExistsSerialNo(Session.CompanyID, ttIssueReturn.PartNum, ttSelectedSerialNumbers.SerialNumber, ttIssueReturn.FromBinNum))
                            {
                                pcMessage = Strings.SerialNumberSHaveAlreadyBeenSelecForBinDoYouWish(ttIssueReturn.FromBinNum);
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSetjobseq (either to or from is changing)
        /// </summary>
        /// <param name="piJobSeq">JobSeq</param>
        /// <param name="pcDirection">Direction</param>
        /// <param name="pCallProcess">Calling Process value</param>
        /// <param name="ds">IssueReturnDataSet</param>
        public void OnChangingJobSeq(int piJobSeq, string pcDirection, string pCallProcess, ref IssueReturnTableset ds)
        {
            CurrentFullTableset = ds;
            string pcJobNum = string.Empty;
            int piAssemblySeq = 0;
            string pcMessage = string.Empty;
            string cTOTYPE = string.Empty;
            string cFromType = string.Empty;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {

                if (!isAllowedMaterialTransaction() && ttIssueReturn.ToJobSeq > 0)
                {
                    JobOper JobOper = FindFirstJobOper(ttIssueReturn.Company, piJobSeq, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobNum);
                    if (JobOper != null && JobOper.LaborEntryMethod.Equals("X", StringComparison.InvariantCultureIgnoreCase))
                        throw new BLException(Strings.NotAbleToMoveMtlFromToTimeBackflushQtyOperations);
                }
                if (StringExtensions.Compare(pcDirection, "From") == 0)
                {
                    pcJobNum = ttIssueReturn.FromJobNum;
                    piAssemblySeq = ttIssueReturn.FromAssemblySeq;
                    ttIssueReturn.FromPCID = "";
                }
                else
                {
                    pcJobNum = ttIssueReturn.ToJobNum;
                    piAssemblySeq = ttIssueReturn.ToAssemblySeq;
                    ttIssueReturn.ToPCID = "";
                }
                if (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-MTL") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-MTL") == 0)
                {
                    PartWip = FindFirstPartWip3(Session.CompanyID, pcJobNum, piAssemblySeq, piJobSeq);
                    if (PartWip == null)
                    {
                        throw new BLException(Strings.ThereIsNoOutstWIPBalanceForThisMater, "PartWip", "MtlSeq");
                    }
                }
                if ((StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0 && StringExtensions.Compare(pcDirection, "From") == 0) || StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-WIP") == 0)
                {
                    PartWip = FindFirstPartWip(Session.CompanyID, pcJobNum, piAssemblySeq, "M", piJobSeq);
                    if (PartWip == null)
                    {
                        throw new BLException(Strings.ThereIsNoOutstWIPBalanceForThisOpera, "PartWip", "OprSeq");
                    }
                }
                if (StringExtensions.Compare(pcDirection, "From") == 0)
                {
                    ttIssueReturn.FromJobSeq = piJobSeq;       /* determineSensitivity builds the list of fields which are applicable for the TranType */
                    this.determineSensitivity(ttIssueReturn);
                    /* Validation: FromJobSeq */
                    cFromType = this.getFromType(ttIssueReturn.TranType);
                    this.validateJobSeq(ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq, cFromType, ttIssueReturn.SysRowID, "FromJobSeq", "From");
                    ExceptionManager.AssertNoBLExceptions();

                    this.getFromJobSeq(ttIssueReturn, true, true);
                    if (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-MTL") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                    {
                        ttIssueReturn.ToJobNum = ttIssueReturn.FromJobNum;
                        this.onChangeToJobNumCore(ttIssueReturn, out pcMessage);
                        ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
                        this.onChangeToAssemblySeqCore(ttIssueReturn);
                        ttIssueReturn.ToJobSeq = ttIssueReturn.FromJobSeq;
                        this.onChangeToJobSeqCore(ttIssueReturn, out pcMessage);
                        /* Clear the TO Warehouse/Bin fields */
                        ttIssueReturn.ToWarehouseCodeDescription = "";
                        ttIssueReturn.ToWarehouseCode = "";
                        ttIssueReturn.ToBinNum = "";
                        ttIssueReturn.ToBinNumDescription = "";
                    }/* if ttIssueReturn.TranType */
                    this.FillForeignFields(ttIssueReturn);
                }
                else
                {
                    ttIssueReturn.ToJobSeq = piJobSeq;
                    /* clear TO wareshouse/bin getToWhse will get new default */
                    ttIssueReturn.ToWarehouseCode = "";
                    ttIssueReturn.ToWarehouseCodeDescription = "";
                    ttIssueReturn.ToBinNum = "";
                    ttIssueReturn.ToBinNumDescription = "";
                    /* determineSensitivity builds the list of fields which are applicable for the TranType */
                    this.determineSensitivity(ttIssueReturn);
                    cTOTYPE = this.getToType(ttIssueReturn.TranType);
                    this.getToWhse(ttIssueReturn);
                    /* Validation: ToJobSeq */
                    this.validateJobSeq(ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq, cTOTYPE, ttIssueReturn.SysRowID, "ToJobSeq", "To");
                    ExceptionManager.AssertNoBLExceptions();

                    this.onChangeToJobSeqCore(ttIssueReturn, out pcMessage);
                    this.FillForeignFields(ttIssueReturn);
                }
                this.enableSNButton(ttIssueReturn, pCallProcess);
                this.checkStatusTracking();
            }
        }

        /// <summary>
        /// Call this method when the value of Epicor.Mfg.BO.IssueReturnDataSet.ToJobSeq is changing.
        /// </summary>
        /// <param name="piToJobSeq"> Propose ToJobSeq value.</param>
        /// <param name="ds">IssueReturnDataSet</param>
        public void OnChangingToJobSeq(int piToJobSeq, ref IssueReturnTableset ds)
        {
            CurrentFullTableset = ds;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                if (this.getToType(ttIssueReturn.TranType).KeyEquals("MTL"))
                {
                    if (!(this.ExistsJobMtl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, piToJobSeq)))
                    {
                        throw new BLException(Strings.AValidJobMaterialIsRequired, "IssueReturn");
                    }
                }

                if ((this.ExistsJobMtl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, piToJobSeq, true)))
                {
                    throw new BLException(Strings.MaterIsAJobMisceChargeCannotIssue, "IssueReturn");
                }
            }
        }

        /// <summary>
        /// Perform Material Movement.
        /// </summary>
        /// <param name="plNegQtyAction">when TranQty changes, perform NegativeInventoryTest. Provide the answer of that test here.</param>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="legalNumberMessage">The legal number message.  Can be blank.</param>
        /// <param name="partTranPKs">The PartTran primary keys.</param>
        public void PerformMaterialMovement(bool plNegQtyAction, ref IssueReturnTableset ds, out string legalNumberMessage, out string partTranPKs)
        {
            PerformMaterialMovementEx(plNegQtyAction, ref ds, out legalNumberMessage, out _, out partTranPKs);
        }

        /// <summary>
        /// Perform Material Movement.
        /// </summary>
        /// <param name="plNegQtyAction">when TranQty changes, perform NegativeInventoryTest. Provide the answer of that test here.</param>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="legalNumberMessage">The legal number message.  Can be blank.</param>
        /// <param name="message">Message.  Can be blank.</param>
        /// <param name="partTranPKs">The PartTran primary keys.</param>
        public void PerformMaterialMovement2(bool plNegQtyAction, ref IssueReturnTableset ds, out string legalNumberMessage, out string message, out string partTranPKs)
        {
            PerformMaterialMovementEx(plNegQtyAction, ref ds, out legalNumberMessage, out message, out partTranPKs);
        }

        private void PerformMaterialMovementEx(bool plNegQtyAction, ref IssueReturnTableset ds, out string legalNumberMessage, out string message, out string partTranPKs)
        {
            /*plNegQtyAction is obsolete. MasterInventoryBinTests should be called prior to this method. */
            legalNumberMessage = string.Empty;
            message = string.Empty;
            partTranPKs = string.Empty;

            CurrentFullTableset = ds;

            partTranPK = "";

            using (var mainTxScope = ErpContext.CreateDefaultTransactionScope())
            {
                ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                                 where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                                 select ttIssueReturn_Row).FirstOrDefault();
                if (ttIssueReturn != null)
                {
                    var isTransfer = ttIssueReturn.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase);
                    bool isPCIDPick = ttIssueReturn.TranReference.Equals("PCIDPICK", StringComparison.OrdinalIgnoreCase);

                    if ((ttIssueReturn.TranType.Equals("STK-ASM", StringComparison.OrdinalIgnoreCase)
                        || ttIssueReturn.TranType.Equals("STK-MTL", StringComparison.OrdinalIgnoreCase)
                        || ttIssueReturn.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))
                        && !string.IsNullOrEmpty(ttIssueReturn.FromPCID) && ttIssueReturn.TranQty > 0)
                    {
                        Erp.Internal.Lib.PackageControlValidations.PkgControlHeaderPartialRow pkgControlHeaderRow = null;
                        using (Internal.Lib.PackageControlValidations libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
                        {
                            pcidUpdateValidations(isPCIDPick, ttIssueReturn.TranType, ttIssueReturn.FromPCID, Session.CompanyID, Session.PlantID, libPackageControlValidations, out pkgControlHeaderRow);
                            ExceptionManager.AssertNoBLExceptions();
                        }

                        if (!isPCIDPick && (!ExistsPkgControlItem(Session.CompanyID, ttIssueReturn.FromPCID, ttIssueReturn.PartNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, ttIssueReturn.UM)))
                        {
                            throw new BLException(Strings.ThisPartLotUomCombinationDoesNotExistInPCID(ttIssueReturn.FromPCID));
                        }
                    }

                    if (isTransfer)
                    {
                        CheckPlantSerialTrack(ttIssueReturn);
                        CheckSamePlantOnTFOrders(ttIssueReturn);
                        CheckNotReturnablePCID(ttIssueReturn.Company, ttIssueReturn.ToPCID);
                        if ((!String.IsNullOrEmpty(ttIssueReturn.ToPCID)) && !isPCIDPick)
                        {
                            CheckPCIDLocation(ttIssueReturn);
                        }
                    }
                    else
                    {
                        if ((ttIssueReturn.TranType.Equals("STK-SHP", StringComparison.OrdinalIgnoreCase) || ttIssueReturn.TranType.Equals("CMP-SHP", StringComparison.OrdinalIgnoreCase)) && (!String.IsNullOrEmpty(ttIssueReturn.ToPCID)) && !isPCIDPick)
                        {
                            CheckPCIDLocation(ttIssueReturn);
                        }
                    }

                    string eadErrMsg = LibEADValidation.validateEAD(ttIssueReturn.TranDate, "IP", "");
                    if (!String.IsNullOrEmpty(eadErrMsg))
                        throw new BLException(eadErrMsg, "ttIssueReturn");

                    if (!isAllowedMaterialTransaction() && ttIssueReturn.ToJobSeq > 0)
                    {
                        JobOper JobOper = FindFirstJobOper(ttIssueReturn.Company, ttIssueReturn.ToJobSeq, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobNum);
                        if (JobOper != null && JobOper.LaborEntryMethod.Equals("X", StringComparison.InvariantCultureIgnoreCase))
                            throw new BLException(Strings.NotAbleToMoveMtlFromToTimeBackflushQtyOperations);
                    }

                    determineSensitivity(ttIssueReturn); /* determineSensitivity builds the list of fields which are applicable for the TranType */
                    CheckIssueReturnRow(ttIssueReturn); /* Validations */

                    List<IssueReturnRow> pcidIssueReturnRows = null;
                    List<string> childPCIDs = null;

                    using (Internal.Lib.PackageControlValidations libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
                    {
                        bool isPCIDMovement = libPackageControlValidations.IsPCIDMovement(ttIssueReturn.FromPCID, ttIssueReturn.ToPCID, ttIssueReturn.PartNum);

                        if (isPCIDMovement)
                        {
                            bool enablePackageControl = ExistsPlantConfCtrlEnablePackageControl(Session.CompanyID, Session.PlantID, true);
                            ValidatePCIDForMovement(enablePackageControl, ttIssueReturn.FromPCID, ttIssueReturn.TranType);

                            ExceptionManager.AssertNoBLExceptions();

                            pcidIssueReturnRows = new List<IssueReturnRow>();
                            childPCIDs = new List<string>();
                            if (isPCIDPick)
                            {
                                PopulateIssueReturnRowsForPCIDPick(ttIssueReturn, ttIssueReturn.MtlQueueRowId, ref ds, ref pcidIssueReturnRows);
                                PopulateChildPCIDsRowsForEachPkgControlItem(ttIssueReturn, ttIssueReturn.FromPCID, ref childPCIDs);
                            }
                            else
                            {
                                if (ttIssueReturn.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase))
                                {
                                    using (Erp.Internal.Lib.PackageControl libPackageControl = new Internal.Lib.PackageControl(Db))
                                    {
                                        if (libPackageControl.IsWinClientClientType())
                                        {
                                            throw new BLException(Strings.WIPExistsInAPCIDCannotUseClassicUI);
                                        }
                                    }
                                    PopulateIssueReturnRowsForEachPkgControlStageItem(ttIssueReturn, ttIssueReturn.FromPCID, 1, ref pcidIssueReturnRows, ref childPCIDs);
                                }
                                else
                                {
                                    PopulateIssueReturnRowsForEachPkgControlItem(ttIssueReturn, ttIssueReturn.FromPCID, 1, ref pcidIssueReturnRows, ref childPCIDs);
                                }
                            }
                        }
                        else
                        {
                            //Validation for Non-Master Parts, take UOM from ttIssueReturn.UM. SCR 204179
                            string inventoryUOM = (ExistsPart(Session.CompanyID, ttIssueReturn.PartNum)) ? getInventoryUOM() : ttIssueReturn.UM;

                            if (!inventoryUOM.Equals(ttIssueReturn.UM, StringComparison.OrdinalIgnoreCase))
                            {
                                decimal convQty = decimal.Zero;
                                LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, inventoryUOM, out convQty, false);
                                ttIssueReturn.TranQty = convQty;
                                ttIssueReturn.UM = inventoryUOM;
                            }

                            // Execute this logic when executing an Unpick but not when executing an UnpickPCID
                            if (ttIssueReturn.ProcessID.Equals("Unpick", StringComparison.OrdinalIgnoreCase) && ttIssueReturn.TranType.Equals("STK-STK", StringComparison.OrdinalIgnoreCase))
                            {
                                ttIssueReturn.ToPCID = String.Empty;

                                bool hasOrder = ttIssueReturn.OrderNum > 0 && ttIssueReturn.OrderLine > 0 && ttIssueReturn.OrderRel > 0;
                                bool hasPCID = !String.IsNullOrEmpty(ttIssueReturn.FromPCID);
                                if (hasOrder && !hasPCID)
                                {
                                    string pcid = String.Empty;
                                    bool multipleMatch = false;
                                    bool blankMatch = false;

                                    LibPackageControl.ValidateOrderOnPCID(Session.CompanyID, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel, ttIssueReturn.LotNum, ttIssueReturn.UM, out pcid, out multipleMatch, out blankMatch);
                                    if (!blankMatch)
                                    {
                                        if (multipleMatch)
                                            throw new BLException(Strings.PCIDMultipleMatchingPCIDs);
                                        ttIssueReturn.FromPCID = pcid;
                                    }
                                }
                            }
                            if (ttIssueReturn.ProcessID.Equals("Unpick", StringComparison.OrdinalIgnoreCase) && ttIssueReturn.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase))
                            {
                                ttIssueReturn.RequirementUOM = ttIssueReturn.UM;
                            }
                        }

                        bool epicorFSA = false;
                        bool vWave = false;
                        int vWaveNum = 0;
                        int vWaveMtlQueueSeq = 0;

                        MtlQueue waveMtlQueue = FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
                        if (waveMtlQueue != null)
                        {
                            epicorFSA = waveMtlQueue.EpicorFSA;
                            vWave = waveMtlQueue.WaveNum != 0;
                            vWaveNum = waveMtlQueue.WaveNum;
                            vWaveMtlQueueSeq = waveMtlQueue.MtlQueueSeq;
                        }

                        // Additional validation for EpicorFSA related MtlQueue transactions.
                        if (epicorFSA)
                        {
                            if (!waveMtlQueue.ToWhse.KeyEquals(ttIssueReturn.ToWarehouseCode) || !waveMtlQueue.ToBinNum.KeyEquals(ttIssueReturn.ToBinNum))
                                throw new BLException(Strings.FSAToLocationCannotBeChanged);
                        }

                        string cFromType = getFromType(ttIssueReturn.TranType);

                        if (StringExtensions.Compare(cFromType, "STK") == 0)
                        {
                            /* SCR 50779 - Checking for inactive FromBins */
                            if (ExistsInactiveWhseBin(Session.CompanyID, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum))
                                throw new BLException(Strings.BinIsInactive(ttIssueReturn.FromBinNum));

                            /* SCR 50779 - Checking for inactive ToBins */
                            if (ExistsInactiveWhseBin(Session.CompanyID, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum))
                                throw new BLException(Strings.BinIsInactive(ttIssueReturn.ToBinNum));
                        }

                        if (cFromType.Equals("STK", StringComparison.OrdinalIgnoreCase) || ttIssueReturn.TranType.Equals("PUR-STK", StringComparison.OrdinalIgnoreCase) || ttIssueReturn.TranType.Equals("PLT-STK", StringComparison.OrdinalIgnoreCase))
                        {
                            /* Now proceed with FIFO logic for both  cFromType = "STK" and PUR-STK */
                            /* SCR #51749 - Check FIFO negative onhandqty for non-FIFO cost methods if enabled */
                            /* get the plant cost id */
                            /* SCR #76743 - Do not validate negative FIFO qty for CMI and SMI bins. *
                             * The CMI bin does not have any costs with it and no FIFO gets created *
                             * when qty received to the CMI bin. So, no need to check for FIFO qty. *
                             * The SMI bin does not issue qty directly to job but instead receives  *
                             * the qty first into standard bin (through im/GenSMIReceipt.p) so cost *
                             * can be calculated/applied. At this point, qty is still in SMI bin so *
                             * no FIFO qty to check yet. We'll stop negative FIFO when we save.     */
                            /* check for BinType to determine if SMI or SMI bin */

                            string plant = string.Empty;
                            string vPlantCostID = string.Empty;
                            bool nonSuppCustBinType = false;

                            WhseBin whseBin = FindFirstWhseBin(ttIssueReturn.Company, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum);
                            if (whseBin != null && StringExtensions.Lookup("Supp,Cust", whseBin.BinType) == -1)
                            {
                                nonSuppCustBinType = true;
                                plant = getPlantFromWarehouse(ttIssueReturn.FromWarehouseCode);
                                LibGetPlantCostID._getPlantCostID(plant, out vPlantCostID, ref Plant, ref XaSyst);
                            }

                            if (isPCIDMovement)
                            {
                                foreach (IssueReturnRow pcidIssueReturnRow in pcidIssueReturnRows)
                                {
                                    if (nonSuppCustBinType)
                                        CheckNegativeFIFO(pcidIssueReturnRow, plant, vPlantCostID);
                                }
                            }
                            else
                            {
                                CheckAllocations(ttIssueReturn, cFromType, vWave, vWaveMtlQueueSeq);

                                if (nonSuppCustBinType)
                                    CheckNegativeFIFO(ttIssueReturn, plant, vPlantCostID);
                            }
                        }

                        bool L_FinalOp = true;

                        if (!isPCIDMovement)
                        {
                            if (StringExtensions.Compare(ttIssueReturn.TranType, "MFG-OPR") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-WIP") == 0)
                            {
                                L_FinalOp = (ttIssueReturn.FromAssemblySeq != 0) ? false : ((ExistsJobAsmbl(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, 0, ttIssueReturn.FromJobSeq)) ? true : ((!ExistsJobOper(ttIssueReturn.Company, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq)) ? true : false));
                            }

                            if (ttIssueReturn.EnableSN && (L_FinalOp || StringExtensions.Compare(ttIssueReturn.TranType, "MFG-OPR") == 0))
                            {
                                updatePartSNFormat(ttIssueReturn.PartNum, ttIssueReturn.SerialControlPlant, CurrentFullTableset.SNFormat);
                                ExceptionManager.AssertNoBLExceptions();
                            }
                        }

                        bool isPCIDPICKForPLTSTK = false;

                        using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())//start the transaction
                        {
                            #region Serial Number for Non PCID

                            if (!isPCIDMovement)
                            {
                                if (ttIssueReturn.EnableSN && (L_FinalOp || StringExtensions.Compare(ttIssueReturn.TranType, "MFG-OPR") == 0))
                                {
                                    string snErrors = string.Empty;
                                    int snErrorsCount = 0;
                                    int snCount = 0;
                                    // Calculate the # of Rows and validate each selected serial number.  The Serial# records should match the TranQty 
                                    // Validate the selected serial numbers and lock the SerialNo records */
                                    foreach (var ttSelectedSerialNumbers_iterator in (from ttSelectedSerialNumbers_Row in CurrentFullTableset.SelectedSerialNumbers
                                                                                      where ttSelectedSerialNumbers_Row.Deselected == false
                                                                                      && ttSelectedSerialNumbers_Row.PartNum.KeyEquals(ttIssueReturn.PartNum)
                                                                                      && ttSelectedSerialNumbers_Row.AttributeSetID == ttIssueReturn.AttributeSetID
                                                                                      && !String.IsNullOrEmpty(ttSelectedSerialNumbers_Row.RowMod)
                                                                                      select ttSelectedSerialNumbers_Row))
                                    {
                                        ttSelectedSerialNumbers = ttSelectedSerialNumbers_iterator;
                                        snCount = snCount + 1;

                                        SerialNo = FindFirstSerialNoWithUpdLock(Session.CompanyID, ttSelectedSerialNumbers.PartNum, ttSelectedSerialNumbers.SerialNumber);
                                        string snError = string.Empty;
                                        validateSerialNumber(ttSelectedSerialNumbers.SerialNumber, SerialNo, out snError);
                                        if (!String.IsNullOrEmpty(snError))
                                        {
                                            snErrorsCount = snErrorsCount + 1;
                                            if (snErrorsCount <= LibSerialCommon.sercomErrorLimit)
                                            {
                                                snErrors = snErrors + snError;
                                            }
                                        }
                                    }/* for each ttSelectedSerialNumbers */

                                    Part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
                                    if (Part == null)
                                        throw new BLException(Strings.PartNotFound);

                                    decimal convQty = decimal.Zero;
                                    LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, Part.IUM, out convQty, false);
                                    /* validate that there are the proper number of serial numbers selected */
                                    if (snCount != Math.Abs(convQty))
                                    {
                                        snErrors = GlobalStrings.SNQuantityTxt(snCount, String.Format("{0}", (int)convQty));
                                        throw new BLException(snErrors);
                                    }
                                    if (!String.IsNullOrEmpty(snErrors))
                                    {
                                        if (snErrorsCount > LibSerialCommon.sercomErrorLimit)
                                        {
                                            snErrors = snErrors + GlobalStrings.SNErrorLimitTxt(LibSerialCommon.sercomErrorLimit, snErrorsCount);
                                        }

                                        throw new BLException(snErrors);
                                    }
                                    /* if we get this far, it is OK to update the serial number data */
                                    if (Math.Abs(convQty) > 0)
                                    {
                                        this.updateSerialNo(CurrentFullTableset.SNFormat, CurrentFullTableset.SelectedSerialNumbers, ttIssueReturn);
                                        ExceptionManager.AssertNoBLExceptions();
                                    }
                                }/* if ttIssueReturn.EnableSN */
                            }

                            #endregion Serial Number for Non PCID

                            #region Generate Legal Number if necessary

                            ttLegalNumGenOpts = (from ttLegalNumGenOpts_Row in ds.LegalNumGenOpts
                                                 where StringExtensions.Compare(ttLegalNumGenOpts_Row.RowMod, IceRow.ROWSTATE_UNCHANGED) != 0
                                                 select ttLegalNumGenOpts_Row).FirstOrDefault();
                            /* If a record is available, generate the number.  Otherwise, a number isn't needed. */
                            if (ttLegalNumGenOpts != null)
                            {
                                var IssueReturnRows = (from IssueReturnrows in ds.IssueReturn select IssueReturnrows).FirstOrDefault();
                                bool allowLegalNumber = false;

                                if (!String.IsNullOrEmpty(IssueReturnRows.TranDocTypeID))
                                {
                                    allowLegalNumber = true;
                                }

                                if (allowLegalNumber)
                                {
                                    string cOCRNumber = string.Empty;
                                    this.LibLegalNumberGenerate.GenerateLegalNumber(CurrentFullTableset.LegalNumGenOpts, "", "", out TempLegalNumber, out cOCRNumber, out legalNumberMessage);
                                    ttLegalNumGenOpts.RowMod = IceRow.ROWSTATE_DELETED;
                                }
                            }

                            #endregion Generate Legal Number if necessary

                            /* STEP 3 -  CALL THE SPECIFIC TRANSACTION TYPE HANDLING PROCEDURES */
                            if (isPCIDMovement)
                            {
                                if (ttIssueReturn.TranType.KeyEquals("PLT-STK") && isPCIDPick)
                                {
                                    TransferPCIDToNewBin(ttIssueReturn.Company, ttIssueReturn.Plant, ttIssueReturn.FromPCID, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, string.Empty, Db);

                                    MtlQueue pltstkMtlQueue = FindFirstMtlQueueWithUpdLock(ttIssueReturn.MtlQueueRowId);
                                    if (pltstkMtlQueue != null)
                                    {
                                        Db.MtlQueue.Delete(pltstkMtlQueue);
                                    }
                                    isPCIDPICKForPLTSTK = true;
                                }
                                else
                                {
                                    bool serialTrackedPartExists = false;
                                    string fromPlant = FindFirstWarehsePlant(ttIssueReturn.Company, ttIssueReturn.FromWarehouseCode);
                                    string toPlant = FindFirstWarehsePlant(ttIssueReturn.Company, ttIssueReturn.ToWarehouseCode);

                                    /* Run logic to set EnableSN for each IssuReturnRow and set serialTrackedPartExists if any serial items exist on PCID */
                                    foreach (IssueReturnRow pcidIssueReturnRow in pcidIssueReturnRows)
                                    {
                                        enableSNButton(pcidIssueReturnRow, pcidIssueReturnRow.ProcessID);
                                        if (pcidIssueReturnRow.EnableSN)
                                            serialTrackedPartExists = true;
                                    }

                                    bool moveSerials = false;
                                    bool voidSerials = false;
                                    bool updatePCIDOutboundContainer = false;
                                    bool pcidOutboundContainerValue = false;

                                    /* If serial items exist on PCID then validate that the movement can be done using from/to plant SerialTracking flag */
                                    if (serialTrackedPartExists)
                                    {
                                        int fromPlantSerialTracking = LibSerialCommon.isSerTraPlantType(fromPlant);
                                        int toPlantSerialTracking = LibSerialCommon.isSerTraPlantType(toPlant);

                                        libPackageControlValidations.ValidateSerialProcessingForPCIDMovement(ttIssueReturn.Company, fromPlantSerialTracking, toPlantSerialTracking, ttIssueReturn.FromPCID, out moveSerials, out voidSerials, out updatePCIDOutboundContainer, out pcidOutboundContainerValue);

                                        foreach (string childPCID in childPCIDs)
                                        {
                                            libPackageControlValidations.ValidateSerialProcessingForPCIDMovement(ttIssueReturn.Company, fromPlantSerialTracking, toPlantSerialTracking, childPCID, out moveSerials, out voidSerials, out updatePCIDOutboundContainer, out pcidOutboundContainerValue);
                                        }
                                    }

                                    /* Move/Void Serials for all Serials on PCID and then invoke transaction specific process method to do move */
                                    foreach (IssueReturnRow pcidIssueReturnRow in pcidIssueReturnRows)
                                    {
                                        if (pcidIssueReturnRow.EnableSN && (moveSerials || voidSerials))
                                            UpdateSerialNoForPCID(pcidIssueReturnRow, voidSerials);

                                        InvokeTransactionSpecificProcessMethod(pcidIssueReturnRow, false, 0, 0, out message);
                                        ttSNtranRows.Clear();
                                    }

                                    /* Move parent PCID and all child PCIDs */
                                    using (var libPCIDMove = new Erp.Internal.Lib.PCIDMove(Db))
                                    {
                                        if (ttIssueReturn.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase))
                                        {
                                            if (childPCIDs.Count > 0)
                                            {
                                                throw new BLException(Strings.ChildPCIDsNotSupportedWithWIP(ttIssueReturn.FromPCID));
                                            }

                                            libPCIDMove.MoveWIPPCID(ttIssueReturn.Company, ttIssueReturn.FromPCID, fromPlant, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, toPlant, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum);
                                        }
                                        else
                                        {
                                            libPCIDMove.MovePCID(ttIssueReturn.Company, ttIssueReturn.FromPCID, fromPlant, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, toPlant, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, updatePCIDOutboundContainer, pcidOutboundContainerValue, !isPCIDPick);

                                            foreach (string childPCID in childPCIDs)
                                            {
                                                libPCIDMove.MovePCID(ttIssueReturn.Company, childPCID, fromPlant, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, toPlant, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, updatePCIDOutboundContainer, pcidOutboundContainerValue, !isPCIDPick);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                InvokeTransactionSpecificProcessMethod(ttIssueReturn, vWave, vWaveNum, vWaveMtlQueueSeq, out message);
                            }

                            if (ttIssueReturn.TranType.Compare("STK-UKN") != 0)
                            {
                                JobMtl jobMtl = null;

                                if (!string.IsNullOrEmpty(ttIssueReturn.ToJobNum))
                                    jobMtl = FindFirstJobMtlCN(ttIssueReturn.Company, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq);

                                if (jobMtl == null && !string.IsNullOrEmpty(ttIssueReturn.FromJobNum))
                                    jobMtl = FindFirstJobMtlCN(ttIssueReturn.Company, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq);

                                if (jobMtl != null)
                                {
                                    FSCallhd = FindFirstFSCallhdWithUpdLock(Session.CompanyID, jobMtl.CallNum);
                                    if (FSCallhd != null)
                                        FSCallhd.MaterialComplete = jobMtl.IssuedComplete;
                                }
                            }

                            /* STEP 4 - GO UPDATE THE MTLQUEUE */

                            if (isPCIDMovement)
                            {
                                if (!isPCIDPICKForPLTSTK)
                                {
                                    Erp.Tables.MtlQueue mtlQueue;

                                    mtlQueue = FindFirstMtlQueueWithUpdLock(ttIssueReturn.MtlQueueRowId);
                                    if (mtlQueue != null)
                                    {
                                        if (isPCIDPick)
                                        {
                                            foreach (var pcidPickContentsMtlQueue_iterator in this.SelectMtlQueuePCIDPickContentsWithUpdLock(mtlQueue.Company, mtlQueue.Plant, mtlQueue.FromWhse, mtlQueue.FromBinNum, "PCIDPICK", mtlQueue.MtlQueueSeq.ToString()))
                                            {
                                                Db.MtlQueue.Delete(pcidPickContentsMtlQueue_iterator);
                                            }
                                        }
                                        Db.MtlQueue.Delete(mtlQueue);
                                    }

                                    bool updateParentCustInfo = isPCIDPick ? true : false;
                                    foreach (var pcidIssueReturnRow_iterator in pcidIssueReturnRows)
                                    {
                                        IssueReturnRow pcidIssueReturnRow = pcidIssueReturnRow_iterator;

                                        if ((pcidIssueReturnRow.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase) || pcidIssueReturnRow.TranType.Equals("STK-SHP", StringComparison.OrdinalIgnoreCase) || pcidIssueReturnRow.TranType.Equals("CMP-SHP", StringComparison.OrdinalIgnoreCase)) && !String.IsNullOrEmpty(pcidIssueReturnRow.ToPCID))
                                        {
                                            pcidUpdate(ref pcidIssueReturnRow, pcidIssueReturnRow.ProcessID, ttIssueReturn.ToPCID);
                                            // update the cust/shipto info on the parent PCID if necessary
                                            if (updateParentCustInfo)
                                            {
                                                //Adjust container for toplevel parent PCID
                                                this.LibAdjustReturnContainer.AdjustContainer(ttIssueReturn.ToPCID, "Picking", -1);
                                                UpdateParentPCIDCustInfoFromChildPCID(ttIssueReturn.Company, ttIssueReturn.FromPCID, pcidIssueReturnRow.ToPCID);
                                                updateParentCustInfo = false;
                                            }
                                        }
                                    }

                                    //Adjust containers for child PCIDs
                                    foreach (string childPCID in childPCIDs)
                                    {
                                        this.LibAdjustReturnContainer.AdjustContainer(childPCID, "Picking", -1);
                                    }
                                }
                            }
                            else
                            {
                                if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && (!(ttIssueReturn.MtlQueueRowId == Guid.Empty) && !String.IsNullOrEmpty(ttIssueReturn.MtlQueueRowId.ToString())))
                                {
                                    if (vWave)
                                    {
                                        #region Wave
                                        foreach (var ttIssueReturnWaveChildRow in (from ttIssueReturnWaveChild_Row in ttIssueReturnWaveChildRows
                                                                                   select ttIssueReturnWaveChild_Row))
                                        {
                                            ttIssueReturnWaveChild = ttIssueReturnWaveChildRow;
                                            this.LibAllocations.updateMtlQueue(ttIssueReturnWaveChild.UM, ttIssueReturnWaveChild.FromWarehouseCode, ttIssueReturnWaveChild.FromBinNum, ttIssueReturnWaveChild.LotNum, ttIssueReturnWaveChild.OrderNum, ttIssueReturnWaveChild.OrderLine, ttIssueReturnWaveChild.OrderRel, ttIssueReturnWaveChild.PartNum, ttIssueReturnWaveChild.FromJobNum, ttIssueReturnWaveChild.ToWarehouseCode, ttIssueReturnWaveChild.ToBinNum, ttIssueReturnWaveChild.TranQty, Guid.Parse(ttIssueReturnWaveChild.MtlQueueRowId));
                                        }
                                        decimal vWaveLeftToProcess = ttIssueReturn.TranQty;
                                        decimal vThisTranQty = decimal.Zero;
                                        string vWavePartNum = ttIssueReturn.PartNum;

                                        foreach (var waveMtlQueue_iterator in SelectMtlQueueWithUpdLock(Session.CompanyID, Session.PlantID, vWavePartNum, "Wave-2:", Compatibility.Convert.ToString(vWaveMtlQueueSeq)))
                                        {
                                            waveMtlQueue = waveMtlQueue_iterator;
                                            vThisTranQty = ((waveMtlQueue.Quantity <= vWaveLeftToProcess) ? waveMtlQueue.Quantity : vWaveLeftToProcess);
                                            vWaveLeftToProcess = vWaveLeftToProcess - vThisTranQty;
                                            if (vThisTranQty != waveMtlQueue.Quantity)
                                            {
                                                waveMtlQueue.Quantity = waveMtlQueue.Quantity - vThisTranQty;
                                                MtlQueue waveMtlQueuePartial = new Erp.Tables.MtlQueue();
                                                Db.MtlQueue.Insert(waveMtlQueuePartial);
                                                BufferCopy.CopyExceptFor(waveMtlQueue, waveMtlQueuePartial, MtlQueue.ColumnNames.MtlQueueSeq);
                                                //waveMtlQueuePartial.MtlQueueSeq = Compatibility.Convert.ToInt32(Services.Transition.Sequences.MtlQueueSeq) + 1;
                                                waveMtlQueuePartial.Lock = false;
                                                waveMtlQueuePartial.AssignedToEmpID = ((StringExtensions.Compare(waveMtlQueuePartial.AssignedToEmpID, "PICKPACK") != 0) ? "" : waveMtlQueuePartial.AssignedToEmpID);
                                                waveMtlQueuePartial.SelectedByEmpID = ((StringExtensions.Compare(waveMtlQueuePartial.SelectedByEmpID, "PICKPACK") != 0) ? "" : waveMtlQueuePartial.SelectedByEmpID);
                                                waveMtlQueuePartial.TranStatus = "RELEASED";
                                                waveMtlQueuePartial.ReferencePrefix = ((waveMtlQueue.OrderNum != 0) ? "SO:" : ((!String.IsNullOrEmpty(waveMtlQueue.TargetJobNum)) ? "Job:" : ((!String.IsNullOrEmpty(waveMtlQueue.TargetTFOrdNum)) ? "TFO:" : "")));
                                                waveMtlQueuePartial.Reference = ((waveMtlQueue.OrderNum != 0) ? Compatibility.Convert.ToString(waveMtlQueue.OrderNum) + "/" + Compatibility.Convert.ToString(waveMtlQueue.OrderLine) + "/" + Compatibility.Convert.ToString(waveMtlQueue.OrderRelNum) : ((!String.IsNullOrEmpty(waveMtlQueue.TargetJobNum)) ? Compatibility.Convert.ToString(waveMtlQueue.TargetJobNum) + "/" + Compatibility.Convert.ToString(waveMtlQueue.TargetAssemblySeq) + "/" + Compatibility.Convert.ToString(waveMtlQueue.TargetMtlSeq) : ((!String.IsNullOrEmpty(waveMtlQueue.TargetTFOrdNum)) ? Compatibility.Convert.ToString(waveMtlQueue.TargetTFOrdNum) + "/" + Compatibility.Convert.ToString(waveMtlQueue.TargetTFOrdLine) : "")));
                                                waveMtlQueuePartial.WaveNum = 0;
                                                waveMtlQueuePartial.Quantity = vThisTranQty;
                                                //The lot number & "from" location of the STK-SHP transaction should match the lot number & "To" location of the STK-STK wave transaction.
                                                waveMtlQueuePartial.FromWhse = ttIssueReturn.ToWarehouseCode;
                                                waveMtlQueuePartial.FromBinNum = ttIssueReturn.ToBinNum;
                                                waveMtlQueuePartial.FromPCID = ttIssueReturn.ToPCID;
                                                waveMtlQueuePartial.LotNum = ttIssueReturn.LotNum;
                                            }
                                            else
                                            {
                                                waveMtlQueue.Lock = false;
                                                waveMtlQueue.AssignedToEmpID = ((StringExtensions.Compare(waveMtlQueue.AssignedToEmpID, "PICKPACK") != 0) ? "" : waveMtlQueue.AssignedToEmpID);
                                                waveMtlQueue.SelectedByEmpID = ((StringExtensions.Compare(waveMtlQueue.SelectedByEmpID, "PICKPACK") != 0) ? "" : waveMtlQueue.SelectedByEmpID);
                                                waveMtlQueue.TranStatus = "RELEASED";
                                                waveMtlQueue.ReferencePrefix = ((waveMtlQueue.OrderNum != 0) ? "SO:" : ((!String.IsNullOrEmpty(waveMtlQueue.TargetJobNum)) ? "Job:" : ((!String.IsNullOrEmpty(waveMtlQueue.TargetTFOrdNum)) ? "TFO:" : "")));
                                                waveMtlQueue.Reference = ((waveMtlQueue.OrderNum != 0) ? Compatibility.Convert.ToString(waveMtlQueue.OrderNum) + "/" + Compatibility.Convert.ToString(waveMtlQueue.OrderLine) + "/" + Compatibility.Convert.ToString(waveMtlQueue.OrderRelNum) : ((!String.IsNullOrEmpty(waveMtlQueue.TargetJobNum)) ? Compatibility.Convert.ToString(waveMtlQueue.TargetJobNum) + "/" + Compatibility.Convert.ToString(waveMtlQueue.TargetAssemblySeq) + "/" + Compatibility.Convert.ToString(waveMtlQueue.TargetMtlSeq) : ((!String.IsNullOrEmpty(waveMtlQueue.TargetTFOrdNum)) ? Compatibility.Convert.ToString(waveMtlQueue.TargetTFOrdNum) + "/" + Compatibility.Convert.ToString(waveMtlQueue.TargetTFOrdLine) : "")));
                                                waveMtlQueue.WaveNum = 0;
                                                //The lot number & "from" location of the STK-SHP transaction should match the lot number & "To" location of the STK-STK wave transaction.
                                                waveMtlQueue.FromWhse = ttIssueReturn.ToWarehouseCode;
                                                waveMtlQueue.FromBinNum = ttIssueReturn.ToBinNum;
                                                waveMtlQueue.FromPCID = ttIssueReturn.ToPCID;
                                                waveMtlQueue.LotNum = ttIssueReturn.LotNum;
                                            }
                                            if (vWaveLeftToProcess <= 0)
                                            {
                                                break;
                                            }
                                        }
                                        #endregion Wave
                                    }
                                    else
                                    {
                                        decimal tranQty = ttIssueReturn.TranQty;
                                        if (ttIssueReturn.ReplenishDecreased && (StringExtensions.Compare(ttIssueReturn.TranType, "RAU-STK") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "RMN-STK") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "RMG-STK") == 0))
                                        {
                                            var MtlQueueColumnsResult6 = FindFirstMtlQueue4(ttIssueReturn.MtlQueueRowId);
                                            if (MtlQueueColumnsResult6 != null)
                                            {
                                                decimal qtyAvailable = decimal.Zero;
                                                /* Check the available amount for the bin.*/

                                                var altPartBinColumnsResult = FindFirstPartBin(Session.CompanyID, MtlQueueColumnsResult6.PartNum, MtlQueueColumnsResult6.AttributeSetID, MtlQueueColumnsResult6.FromWhse, MtlQueueColumnsResult6.FromBinNum, MtlQueueColumnsResult6.LotNum, MtlQueueColumnsResult6.IUM, MtlQueueColumnsResult6.FromPCID);
                                                if (altPartBinColumnsResult != null)
                                                {
                                                    qtyAvailable = altPartBinColumnsResult.OnHandQty - altPartBinColumnsResult.SalesAllocatedQty - altPartBinColumnsResult.JobAllocatedQty - altPartBinColumnsResult.TFOrdAllocatedQty
                                                                   - altPartBinColumnsResult.SalesPickingQty - altPartBinColumnsResult.JobPickingQty - altPartBinColumnsResult.TFOrdPickingQty
                                                                   - altPartBinColumnsResult.SalesPickedQty - altPartBinColumnsResult.JobPickedQty - altPartBinColumnsResult.TFOrdPickedQty;
                                                }

                                                tranQty = (MtlQueueColumnsResult6.Quantity - qtyAvailable) + ttIssueReturn.TranQty;
                                            }
                                        }

                                        if (isTransfer)
                                        {
                                            LibAllocations.updateMtlQueueTransfer(ttIssueReturn.UM, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.LotNum, ttIssueReturn.TFOrdNum, ttIssueReturn.TFOrdLine, ttIssueReturn.PartNum, ttIssueReturn.FromJobNum, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, tranQty, ttIssueReturn.MtlQueueRowId);
                                        }
                                        else
                                        {
                                            LibAllocations.updateMtlQueue(ttIssueReturn.UM, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.LotNum, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel, ttIssueReturn.PartNum, ttIssueReturn.FromJobNum, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, tranQty, ttIssueReturn.MtlQueueRowId);
                                        }
                                    }
                                }

                                // PCID related updates for picking sales orders
                                if ((ttIssueReturn.TranType.Equals("STK-SHP", StringComparison.OrdinalIgnoreCase) || ttIssueReturn.TranType.Equals("CMP-SHP", StringComparison.OrdinalIgnoreCase) || (ttIssueReturn.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))) && !String.IsNullOrEmpty(ttIssueReturn.ToPCID))
                                    pcidUpdate(ref ttIssueReturn, ttIssueReturn.ProcessID, ttIssueReturn.ToPCID);

                                FillForeignFields(ttIssueReturn);

                                // ERP-2914 - When performing a material movement it is possible for PCIDs with the 'Allow Delete' flag to be deleted from the DB.
                                //            This can cause issues with the logic from where PerformMaterialMovement was called so this external field was added to deal with them when needed.
                                if (!string.IsNullOrEmpty(ttIssueReturn.FromPCID))
                                {
                                    ttIssueReturn.PkgControlHeaderDeleted = !ExistsPkgControlHeader(Session.CompanyID, ttIssueReturn.FromPCID);
                                }

                                /* UI requirement.  Initialize the record to a state where the user can perform material movement again. */
                                zerottIssueReturn(ref ttIssueReturn);
                                ttIssueReturn.OnHandQty = ttIssueReturn.OnHandQty - ConvQty;
                            }

                            CurrentFullTableset.SelectedSerialNumbers.Clear();
                            CurrentFullTableset.SNFormat.Clear();
                            partTranPKs = partTranPK;

                            Db.Validate();
                            txScope.Complete();//commit the transaction
                        }
                    }
                }
                Db.Validate();
                mainTxScope.Complete();
            }
        }

        private void fromPCIDStatusUpdate(string company, string updPCID)
        {
            // update similar to the status update of the source PCID in BuildSplitMerge 
            if (String.IsNullOrEmpty(updPCID))
                return;

            PkgControlHeaderPartial PartialRowpkgControlHeader = FindFirstPkgControlHeaderPartial(company, updPCID);
            if (PartialRowpkgControlHeader == null)
                throw new BLException(Strings.PCIDNotFound(updPCID));

            if (PartialRowpkgControlHeader.PkgControlStatus.Equals(PkgControlStatus.Child, StringComparison.OrdinalIgnoreCase))
            {
                LibPackageControl.SetPCIDStatus(company, updPCID, PkgControlStatus.Child);
            }
            else
            {
                // Change the status of the Source PCID to STOCK or EMPTY 
                if (this.ExistsPkgControlItem(company, updPCID))
                    LibPackageControl.SetPCIDStatus(company, updPCID, PkgControlStatus.Stock);
                else
                    LibPackageControl.SetPCIDStatus(company, updPCID, PkgControlStatus.Empty);
            }
        }

        private void UpdateParentPCIDCustInfoFromChildPCID(string companyID, string parentPCID, string childPCID)
        {
            PkgControlHeader parentPkgControlHeader = this.FindFirstPkgControlHeaderByCustWithUpdLock(companyID, parentPCID, 0);
            PkgControlHeader childPkgControlHeader = this.FindFirstPkgControlHeader(companyID, childPCID);

            if (parentPkgControlHeader != null && childPkgControlHeader != null && childPkgControlHeader.CustNum > 0)
            {
                // set the Customer and ShipTo info on the Child PkgControlheader, so that validations can ensure that all PCID under the top parent
                // are assigned to the same cust/shipto.
                parentPkgControlHeader.CustNum = childPkgControlHeader.CustNum;
                parentPkgControlHeader.CustID = childPkgControlHeader.CustID;
                parentPkgControlHeader.CustName = childPkgControlHeader.CustName;
                parentPkgControlHeader.ShipToNum = childPkgControlHeader.ShipToNum;
                parentPkgControlHeader.ShipToName = childPkgControlHeader.ShipToName;
                parentPkgControlHeader.ShipToAddress1 = childPkgControlHeader.ShipToAddress1;
                parentPkgControlHeader.ShipToAddress2 = childPkgControlHeader.ShipToAddress2;
                parentPkgControlHeader.ShipToAddress3 = childPkgControlHeader.ShipToAddress3;
                parentPkgControlHeader.ShipToCity = childPkgControlHeader.ShipToCity;
                parentPkgControlHeader.ShipToState = childPkgControlHeader.ShipToState;
                parentPkgControlHeader.ShipToZip = childPkgControlHeader.ShipToZip;
                parentPkgControlHeader.ShipToCountryNum = childPkgControlHeader.ShipToCountryNum;
                parentPkgControlHeader.ShipToCountryDesc = childPkgControlHeader.ShipToCountryDesc;
                Db.Validate(parentPkgControlHeader);
            }
        }

        private void CheckPlantSerialTrack(IssueReturnRow issueReturnRow)
        {
            bool isSerialTrack = false;
            bool isDynamicPCID = false;

            var TFOrdHedQry = this.FindFirstTFOrdHed(ttIssueReturn.Company, issueReturnRow.TFOrdNum);
            if (ttIssueReturn.TranReference.Equals("PCIDPICK", StringComparison.OrdinalIgnoreCase))
            {
                var issueReturnDS = ttIssueReturn.Table.Tableset as IssueReturnTableset;
                List<IssueReturnRow> pcidIssueReturnRows = new List<IssueReturnRow>();
                PopulateIssueReturnRowsForPCIDPick(issueReturnRow, issueReturnRow.MtlQueueRowId, ref issueReturnDS, ref pcidIssueReturnRows);
                foreach (IssueReturnRow issueReturn in pcidIssueReturnRows)
                {
                    isSerialTrack = partSerialTracking(issueReturn.PartNum);
                    if (isSerialTrack) break;
                }
            }
            else
            {
                if (TFOrdHedQry != null)
                {
                    var TFOrdDtlQry = this.FindFirstTFOrdDtl(Session.CompanyID, ttIssueReturn.TFOrdNum, ttIssueReturn.TFOrdLine);
                    if (TFOrdDtlQry != null)
                    {
                        isSerialTrack = partSerialTracking(TFOrdDtlQry.PartNum);
                    }
                }
            }

            var PkgControlHeaderQry = this.FindFirstPkgControlHeader(Session.CompanyID, issueReturnRow.ToPCID);
            if (PkgControlHeaderQry != null)
            {
                var PkgControlQry = FindFirstPkgControl(Session.CompanyID, PkgControlHeaderQry.Plant, PkgControlHeaderQry.PkgControlIDCode);
                if (PkgControlQry != null)
                {
                    isDynamicPCID = PkgControlQry.PkgControlType.Equals("DYNAMIC", StringComparison.OrdinalIgnoreCase);
                }
            }

            if (isSerialTrack && isDynamicPCID)
            {
                using (Internal.Lib.PackageControlValidations libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
                {
                    libPackageControlValidations.SerialNumbersCanBeProcessedForPCIDValidation(Session.CompanyID, TFOrdHedQry.Plant, TFOrdHedQry.ToPlant, issueReturnRow.ToPCID);
                }
            }

        }

        /// <summary>
        /// Validates plant to be the same from current tforder/line against the first pcid package item tforder
        /// </summary>
        /// <param name="ttIssueReturn"></param>
        private void CheckSamePlantOnTFOrders(IssueReturnRow ttIssueReturn)
        {
            var tforder = this.FindFirstTFOrdHed(ttIssueReturn.Company, ttIssueReturn.TFOrdNum);
            if (!string.IsNullOrEmpty(ttIssueReturn.ToPCID) && tforder != null)
            {
                var pcid = this.FindFirstPkgControlHeaderPartial(ttIssueReturn.Company, ttIssueReturn.ToPCID);
                if (pcid != null)
                {
                    if (!pcid.PkgControlStatus.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                    {
                        var pcidItem = this.FindFirstPkgControlItem(ttIssueReturn.Company, ttIssueReturn.ToPCID);
                        if (pcidItem != null)
                        {
                            var tforderPkg = this.FindFirstTFOrdHed(ttIssueReturn.Company, pcidItem.TFOrdNum);
                            if (tforderPkg != null)
                            {
                                if (!tforderPkg.ToPlant.KeyEquals(tforder.ToPlant))
                                {
                                    throw new Exception(Strings.PCIDToSiteDoesNotMatchTFOrderToSite);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validates the current pcid throwing error if containerreturnable is true.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="pcid"></param>
        private void CheckNotReturnablePCID(string company, string pcid)
        {
            if (string.IsNullOrEmpty(pcid)) return;

            var isReturnable = this.FindFirstPkgControlHeaderContainerReturnable(company, pcid);
            if (isReturnable)
            {
                throw new Exception(Strings.ReturnablePCIDNotAllowed);
            }
        }

        private void CheckPCIDLocation(IssueReturnRow ttIssueReturn)
        {
            if (string.IsNullOrEmpty(ttIssueReturn.ToPCID)) return;

            var pkgControlHeader = this.FindFirstPkgControlHeader(ttIssueReturn.Company, ttIssueReturn.ToPCID);
            if (pkgControlHeader != null)
            {
                // Check PCID status
                if ((pkgControlHeader.PkgControlType.Equals("STATIC", StringComparison.OrdinalIgnoreCase) &&
                    pkgControlHeader.PkgControlStatus.Equals("BUSY", StringComparison.OrdinalIgnoreCase))
                    || (pkgControlHeader.PkgControlType.Equals("DYNAMIC", StringComparison.OrdinalIgnoreCase) &&
                        !pkgControlHeader.PkgControlStatus.Equals("EMPTY", StringComparison.OrdinalIgnoreCase)))
                {
                    pcidCheckLocation(pkgControlHeader, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum);
                }
            }
            else
            {
                throw new BLException(Strings.PCIDNotFound(ttIssueReturn.ToPCID));
            }
        }

        /// <summary>
        /// Perform Material Movement.
        /// </summary>
        /// <param name="plNegQtyAction">when TranQty changes, perform NegativeInventoryTest. Provide the answer of that test here.</param>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="legalNumberMessage">The legal number message.  Can be blank.</param>
        /// <param name="partTranPKs">The PartTran primary keys.</param>
        /// <param name="legalNum">The Legal Number to be used in PartTran.</param>
        public void PerformMaterialMovementWithLegalNum(bool plNegQtyAction, ref IssueReturnTableset ds, out string legalNumberMessage, out string partTranPKs, string legalNum)
        {
            this.TempLegalNumber = legalNum;
            PerformMaterialMovementEx(plNegQtyAction, ref ds, out legalNumberMessage, out _, out partTranPKs);
        }

        /// <summary>
        /// Recursive function to go through all PkgControlItem records for a PCID and build up a list of IssueReturnRows and a list of child PCIDs 
        /// </summary>
        /// <param name="originalIssueReturnRow">The original ttIssueReturn row</param>
        /// <param name="pcid">The PCID for which to retrieve items</param>
        /// <param name="parentChildLevel">How many levels down we are</param>
        /// <param name="pcidIssueReturnRows">List of IssueReturnRow</param>
        /// <param name="childPCIDs">List of child PCIDs</param>
        private void PopulateIssueReturnRowsForEachPkgControlItem(IssueReturnRow originalIssueReturnRow, string pcid, int parentChildLevel, ref List<IssueReturnRow> pcidIssueReturnRows, ref List<string> childPCIDs)
        {
            Internal.Lib.PackageControlValidations.PCIDParentChildLevelDoesNotExceedMaxValidation(parentChildLevel, 5);

            foreach (PkgControlItemPartialRow pkgControlItemPartial in SelectPkgControlItemPartial(originalIssueReturnRow.Company, pcid))
            {
                if (!string.IsNullOrEmpty(pkgControlItemPartial.ItemPartNum))
                {
                    IssueReturnRow newIssueReturn = new Erp.Tablesets.IssueReturnRow();
                    pcidIssueReturnRows.Add(newIssueReturn);
                    BufferCopy.Copy(originalIssueReturnRow, ref newIssueReturn);

                    newIssueReturn.PartNum = pkgControlItemPartial.ItemPartNum;
                    newIssueReturn.AttributeSetID = pkgControlItemPartial.ItemAttributeSetID;
                    newIssueReturn.LotNum = pkgControlItemPartial.ItemLotNum;
                    newIssueReturn.UM = pkgControlItemPartial.ItemIUM;
                    newIssueReturn.TranQty = pkgControlItemPartial.ItemQuantity;
                    newIssueReturn.FromPCID = pcid;
                    newIssueReturn.ToPCID = pcid;
                }
                else if (!string.IsNullOrEmpty(pkgControlItemPartial.ItemPCID))
                {
                    childPCIDs.Add(pkgControlItemPartial.ItemPCID);
                    PopulateIssueReturnRowsForEachPkgControlItem(originalIssueReturnRow, pkgControlItemPartial.ItemPCID, parentChildLevel + 1, ref pcidIssueReturnRows, ref childPCIDs);
                }
            }
        }

        /// <summary>
        /// Recursive function to go through all PkgControlStageItem records for a PCID and build up a list of IssueReturnRows and a list of child PCIDs 
        /// </summary>
        /// <param name="originalIssueReturnRow">The original ttIssueReturn row</param>
        /// <param name="pcid">The PCID for which to retrieve items</param>
        /// <param name="parentChildLevel">How many levels down we are</param>
        /// <param name="pcidIssueReturnRows">List of IssueReturnRow</param>
        /// <param name="childPCIDs">List of child PCIDs</param>
        private void PopulateIssueReturnRowsForEachPkgControlStageItem(IssueReturnRow originalIssueReturnRow, string pcid, int parentChildLevel, ref List<IssueReturnRow> pcidIssueReturnRows, ref List<string> childPCIDs)
        {
            Internal.Lib.PackageControlValidations.PCIDParentChildLevelDoesNotExceedMaxValidation(parentChildLevel, 5);

            //DJY - Intentionally re-using the PkgControlItemPartialRow collection as it is the same collection
            //of columns required by SelectPkgControlItemPartial and SelectPkgControlStageItemPartial
            foreach (PkgControlItemPartialRow pkgControlStageItemPartial in SelectPkgControlStageItemPartial(originalIssueReturnRow.Company, pcid))
            {
                if (!string.IsNullOrEmpty(pkgControlStageItemPartial.ItemPartNum))
                {
                    IssueReturnRow newIssueReturn = new Erp.Tablesets.IssueReturnRow();
                    pcidIssueReturnRows.Add(newIssueReturn);
                    BufferCopy.Copy(originalIssueReturnRow, ref newIssueReturn);

                    newIssueReturn.PartNum = pkgControlStageItemPartial.ItemPartNum;
                    newIssueReturn.AttributeSetID = pkgControlStageItemPartial.ItemAttributeSetID;
                    newIssueReturn.LotNum = pkgControlStageItemPartial.ItemLotNum;
                    newIssueReturn.UM = pkgControlStageItemPartial.ItemIUM;
                    newIssueReturn.TranQty = pkgControlStageItemPartial.ItemQuantity;
                    newIssueReturn.FromPCID = pcid;
                    newIssueReturn.ToPCID = pcid;
                }
                else if (!string.IsNullOrEmpty(pkgControlStageItemPartial.ItemPCID))
                {
                    childPCIDs.Add(pkgControlStageItemPartial.ItemPCID);
                    PopulateIssueReturnRowsForEachPkgControlStageItem(originalIssueReturnRow, pkgControlStageItemPartial.ItemPCID, parentChildLevel + 1, ref pcidIssueReturnRows, ref childPCIDs);
                }
            }
        }

        /// <summary>
        /// Find all MtlQueue rows for PCIDPICK and turn them into ttIssueReturn rows and a list of child PCIDs 
        /// </summary>
        /// <param name="originalIssueReturnRow">The original ttIssueReturn row</param>
        /// <param name="mtlQueueRowID">RowId of MtlQueue record</param>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pcidIssueReturnRows">List of IssueReturnRow</param>
        private void PopulateIssueReturnRowsForPCIDPick(IssueReturnRow originalIssueReturnRow, Guid mtlQueueRowID, ref IssueReturnTableset ds, ref List<IssueReturnRow> pcidIssueReturnRows)
        {
            Erp.Tables.MtlQueue pcidPickMtlQueue = null;
            Erp.Tables.MtlQueue pcidPickContentsMtlQueue = null;

            pcidPickMtlQueue = this.FindFirstMtlQueue(mtlQueueRowID);
            if (pcidPickMtlQueue != null)
            {
                //If the to location was changed by the user, update the child PCIDPICK transactions
                if ((StringExtensions.Compare(pcidPickMtlQueue.ToWhse, originalIssueReturnRow.ToWarehouseCode) != 0) ||
                    (StringExtensions.Compare(pcidPickMtlQueue.ToBinNum, originalIssueReturnRow.ToBinNum) != 0))
                {
                    pcidPickMtlQueue.ToWhse = originalIssueReturnRow.ToWarehouseCode;
                    pcidPickMtlQueue.ToBinNum = originalIssueReturnRow.ToBinNum;

                    Db.Validate(pcidPickContentsMtlQueue);
                }

                foreach (var MtlQueue_iterator in SelectMtlQueuePCIDPickContentsWithUpdLock(pcidPickMtlQueue.Company, pcidPickMtlQueue.Plant, pcidPickMtlQueue.FromWhse, pcidPickMtlQueue.FromBinNum, "PCIDPICK", pcidPickMtlQueue.MtlQueueSeq.ToString()))
                {
                    pcidPickContentsMtlQueue = MtlQueue_iterator;

                    //If the to location was changed by the user, update the child PCIDPICK transactions
                    if ((StringExtensions.Compare(pcidPickContentsMtlQueue.ToWhse, originalIssueReturnRow.ToWarehouseCode) != 0) ||
                        (StringExtensions.Compare(pcidPickContentsMtlQueue.ToBinNum, originalIssueReturnRow.ToBinNum) != 0))
                    {
                        pcidPickContentsMtlQueue.ToWhse = originalIssueReturnRow.ToWarehouseCode;
                        pcidPickContentsMtlQueue.ToBinNum = originalIssueReturnRow.ToBinNum;

                        Db.Validate(pcidPickContentsMtlQueue);
                    }

                    //Turn MtlQueue rows for PCIDPICK and turn them into ttIssueReturn rows
                    this.GetNewIssueReturn(pcidPickContentsMtlQueue.TranType, pcidPickContentsMtlQueue.SysRowID, originalIssueReturnRow.ProcessID, ref ds);

                    IssueReturnRow newIssueReturn = new Erp.Tablesets.IssueReturnRow();
                    pcidIssueReturnRows.Add(newIssueReturn);
                    BufferCopy.Copy(ttIssueReturn, ref newIssueReturn);

                    ttIssueReturn = originalIssueReturnRow;
                }
            }
        }

        /// <summary>
        /// Recursive function to go through all PkgControlItem records for a PCID and build up a list of child PCIDs 
        /// </summary>
        /// <param name="originalIssueReturnRow">The original ttIssueReturn row</param>
        /// <param name="pcid">The PCID for which to retrieve items</param>
        /// <param name="childPCIDs">List of child PCIDs</param>
        private void PopulateChildPCIDsRowsForEachPkgControlItem(IssueReturnRow originalIssueReturnRow, string pcid, ref List<string> childPCIDs)
        {
            foreach (PkgControlItemPartialRow pkgControlItemPartial in SelectPkgControlItemPartial(originalIssueReturnRow.Company, pcid))
            {
                if (!string.IsNullOrEmpty(pkgControlItemPartial.ItemPCID))
                {
                    childPCIDs.Add(pkgControlItemPartial.ItemPCID);
                    PopulateChildPCIDsRowsForEachPkgControlItem(originalIssueReturnRow, pkgControlItemPartial.ItemPCID, ref childPCIDs);
                }
            }
        }

        #region PCID
        /// <summary>
        /// Performs a mass unpick by PCID - this handles the actual inventory movement - everything else is handled by UnpickTransaction BO
        /// </summary>
        /// <param name="pcid">PCID</param>
        /// <param name="whse">Destination Warehouse Code</param>
        /// <param name="bin">Destination Bin Num</param>
        /// <param name="partNum">Part number for Mass Unpick By PCID (optional)</param>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="legalNumMsg">The legal number message.  Can be blank.</param>
        public void MassUnpickByPCID(string pcid, string whse, string bin, string partNum, ref IssueReturnTableset ds, out string legalNumMsg)
        {
            Erp.Tables.PkgControlHeader massUnpickPkgControlHeader;
            Erp.Tables.PkgControlItem massUnpickPkgControlItem;

            legalNumMsg = String.Empty;
            string tranDocTypeID = string.Empty;
            string partTranPKs = String.Empty;
            bool isChild = false;
            bool isStatic = false;
            bool isSoPick = false;
            bool isTfoPick = false;
            bool isBusy = false;

            Warehse warehse = FindFirstWarehse(Session.CompanyID, whse);
            if (warehse == null)
            {
                throw new BLException(Strings.AValidToWarehouseIsRequired);
            }

            WhseBin = FindFirstWhseBin(Session.CompanyID, warehse.WarehouseCode, bin);
            if (WhseBin == null)
            {
                throw new BLException(Strings.AValidToBinNumberIsRequired);
            }

            if (String.IsNullOrEmpty(pcid))
            {
                throw new BLException(Strings.PCIDRequired);
            }

            massUnpickPkgControlHeader = FindFirstPkgControlHeader(Session.CompanyID, pcid);
            if (massUnpickPkgControlHeader == null)
            {
                throw new BLException(Strings.PCIDNotFound(pcid));
            }

            isChild = this.CheckPkgControlItemChild(Session.CompanyID, pcid);
            isStatic = massUnpickPkgControlHeader.PkgControlType.Equals("STATIC", StringComparison.OrdinalIgnoreCase);
            isSoPick = isChild ? massUnpickPkgControlHeader.PkgControlStatus.Equals("CHILD", StringComparison.OrdinalIgnoreCase) : massUnpickPkgControlHeader.PkgControlStatus.Equals("SOPICK", StringComparison.OrdinalIgnoreCase);
            isTfoPick = isChild ? massUnpickPkgControlHeader.PkgControlStatus.Equals("CHILD", StringComparison.OrdinalIgnoreCase) : massUnpickPkgControlHeader.PkgControlStatus.Equals("TFOPICK", StringComparison.OrdinalIgnoreCase);
            isBusy = massUnpickPkgControlHeader.PkgControlStatus.Equals("BUSY", StringComparison.OrdinalIgnoreCase);

            if (ds.IssueReturn.Any())
            {
                tranDocTypeID = ds.IssueReturn[0].TranDocTypeID;
            }

            //If the record doesn't match any of the following status, it is invalid.
            //-The PCID is STATIC and its status is BUSY
            //-The PCID is DYNAMIC and its status is TFOPICK
            //-The PCID is DYNAMIC and its status is SOPICK
            if ((isStatic && !isBusy) || (!isStatic && (!isSoPick && !isTfoPick)))
            {
                throw new BLException(Strings.PCIDInvalidStatus);
            }

            //During the AppService method ConsumeTrfOrderAlloc, we must differentiate if it's an unpick transaction but the relation is broken on PartTran,
            //So we set a Call Context Key for validation

            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
            {
                IEnumerable<PkgControlItem> PkgControlItemRows = null;
                if (String.IsNullOrEmpty(partNum))
                {
                    PkgControlItemRows = SelectPkgControlItem(Session.CompanyID, pcid);
                }
                else
                {
                    PkgControlItemRows = SelectPkgControlItem(Session.CompanyID, pcid, partNum);
                }

                foreach (var pkgControlItem_iterator in PkgControlItemRows)
                {
                    massUnpickPkgControlItem = pkgControlItem_iterator;

                    // If the user has passed us a specific PartNum to remove from the PCID, we will only 
                    // be removing anything from that level, not removing anything from any nested children
                    if (string.IsNullOrEmpty(partNum) && !string.IsNullOrEmpty(massUnpickPkgControlItem.ItemPCID))
                    {
                        this.MassUnpickByPCID(massUnpickPkgControlItem.ItemPCID, whse, bin, string.Empty, ref ds, out legalNumMsg);
                    }
                    else
                    {
                        foreach (var partAlloc_iterator in (this.SelectPartAlloc(Session.CompanyID, massUnpickPkgControlItem.ItemPartNum, massUnpickPkgControlItem.ItemAttributeSetID, massUnpickPkgControlHeader.WarehouseCode, massUnpickPkgControlHeader.BinNum, massUnpickPkgControlItem.ItemLotNum, massUnpickPkgControlItem.ItemIUM, massUnpickPkgControlItem.PCID)))
                        {
                            PartAlloc = partAlloc_iterator;

                            GetNewIssueReturn("STK-STK", new Guid(), (string.IsNullOrEmpty(partNum) ? "UnpickPCID" : "Unpick"), ref ds);
                            ttIssueReturn = (from ttIssueReturn_Row in CurrentFullTableset.IssueReturn
                                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                                             select ttIssueReturn_Row).FirstOrDefault();
                            if (ttIssueReturn == null)
                            {
                                throw new BLException(Strings.IssueReturnRecordNotFound);
                            }

                            ttIssueReturn.ProcessID = (string.IsNullOrEmpty(partNum) ? "UnpickPCID" : "Unpick");
                            ttIssueReturn.PartNum = PartAlloc.PartNum;
                            ttIssueReturn.LotNum = PartAlloc.LotNum;
                            ttIssueReturn.FromWarehouseCode = PartAlloc.WarehouseCode;
                            ttIssueReturn.FromBinNum = PartAlloc.BinNum;
                            ttIssueReturn.FromPCID = PartAlloc.PCID;
                            ttIssueReturn.TranQty = PartAlloc.PickedQty;
                            ttIssueReturn.UM = PartAlloc.DimCode;
                            ttIssueReturn.ToWarehouseCode = whse;
                            ttIssueReturn.ToBinNum = bin;
                            ttIssueReturn.ToPCID = (string.IsNullOrEmpty(partNum) ? PartAlloc.PCID : string.Empty);

                            if (PartAlloc.OrderNum > 0)
                            {
                                ttIssueReturn.OrderNum = PartAlloc.OrderNum;
                                ttIssueReturn.OrderLine = PartAlloc.OrderLine;
                                ttIssueReturn.OrderRel = PartAlloc.OrderRelNum;
                                ttIssueReturn.TranReference = "SO:" + PartAlloc.OrderNum + "/" + PartAlloc.OrderLine + "/" + PartAlloc.OrderRelNum;
                            }
                            else if (!string.IsNullOrEmpty(PartAlloc.TFOrdNum))
                            {
                                ttIssueReturn.TFOrdNum = PartAlloc.TFOrdNum;
                                ttIssueReturn.TFOrdLine = PartAlloc.TFOrdLine;
                                ttIssueReturn.TranReference = "TFO:" + PartAlloc.TFOrdNum + "/" + PartAlloc.TFOrdLine;
                            }


                            if (!String.IsNullOrEmpty(tranDocTypeID))
                            {
                                ttIssueReturn.TranDocTypeID = tranDocTypeID;
                            }

                            Part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
                            if (Part != null && Part.TrackSerialNum)
                            {
                                ttIssueReturn.EnableSN = true;

                                IEnumerable<SerialNo> SerialNoRows = SelectSerialNoForMassUnpick(Session.CompanyID, ttIssueReturn.FromPCID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel);
                                foreach (SerialNo serialNo_iterator in SerialNoRows)
                                {
                                    SerialNo = serialNo_iterator;

                                    ttSelectedSerialNumbers = new SelectedSerialNumbersRow();
                                    ttSelectedSerialNumbers.Company = SerialNo.Company;
                                    ttSelectedSerialNumbers.Deselected = false;
                                    ttSelectedSerialNumbers.PartNum = SerialNo.PartNum;
                                    ttSelectedSerialNumbers.AttributeSetID = SerialNo.AttributeSetID;
                                    ttSelectedSerialNumbers.RawSerialNum = SerialNo.RawSerialNum;
                                    ttSelectedSerialNumbers.RowMod = "U";
                                    ttSelectedSerialNumbers.SerialNumber = SerialNo.SerialNumber;
                                    ttSelectedSerialNumbers.SNBaseNumber = SerialNo.SNBaseNumber;
                                    ttSelectedSerialNumbers.SNMask = SerialNo.SNMask;
                                    ttSelectedSerialNumbers.SNPrefix = SerialNo.SNPrefix;
                                    ttSelectedSerialNumbers.SourceRowID = SerialNo.SysRowID;
                                    ttSelectedSerialNumbers.SysRowID = new Guid();
                                    ttSelectedSerialNumbers.XRefPartNum = SerialNo.XRefPartNum;
                                    ttSelectedSerialNumbers.XRefPartType = SerialNo.XRefPartType;

                                    CurrentFullTableset.SelectedSerialNumbers.Add(ttSelectedSerialNumbers);
                                }
                            }

                            try
                            {
                                if (isTfoPick || (isStatic && isBusy))
                                {
                                    CallContext.Current.Properties.TryAdd("TFOMassUnpickByPCID", "TFOMassUnpickByPCID");
                                }

                                //Adjust container when partnum is used only if there are no more PkgControlItems for that level
                                if (!String.IsNullOrEmpty(partNum))
                                {
                                    bool skippedRows = false;
                                    foreach (PkgControlItem pkgControlItem in SelectPkgControlItem(Session.CompanyID, pcid))
                                    {
                                        if (!pkgControlItem.ItemPartNum.Equals(partNum, StringComparison.OrdinalIgnoreCase))
                                        {
                                            skippedRows = true;
                                            continue;
                                        }
                                    }

                                    if (!skippedRows)
                                    {
                                        this.LibAdjustReturnContainer.AdjustContainer(pcid, "Picking", 1);
                                    }
                                }

                                PerformMaterialMovementEx(true, ref ds, out legalNumMsg, out _, out partTranPKs);
                                // There should always be a one-to-one link between PartAlloc and a related PickedOrder record, even if the PCID was 
                                // not originally picked but got sent back to picked after being packed, and even if a single PartAlloc mtlqueue
                                // got packed into multiple PCIDs (this should split the original PartAlloc into multiple PartAlloc)
                                PickedOrders = this.FindFirstPickedOrdersForUnpickWithUpdLock(Session.CompanyID, PartAlloc.OrderNum, PartAlloc.OrderLine, PartAlloc.OrderRelNum, PartAlloc.PartNum, massUnpickPkgControlHeader.WarehouseCode, massUnpickPkgControlHeader.BinNum, massUnpickPkgControlItem.ItemLotNum, massUnpickPkgControlItem.ItemAttributeSetID, massUnpickPkgControlItem.PCID);
                                if (PickedOrders != null)
                                {
                                    Db.PickedOrders.Delete(PickedOrders);
                                }
                            }
                            finally
                            {
                                CallContext.Current.Properties.TryRemove("TFOMassUnpickByPCID", out object removedValue);
                            }

                            ds.IssueReturn.Clear();
                            ds.SelectedSerialNumbers.Clear();

                            // Unpicking to stock, so clear the PkgControlItem references to the Sales Order
                            massUnpickPkgControlItem = FindFirstPkgControlItemWithUpdLock(massUnpickPkgControlItem.Company, massUnpickPkgControlItem.PCID, massUnpickPkgControlItem.ItemPCID, massUnpickPkgControlItem.ItemPartNum, massUnpickPkgControlItem.ItemLotNum, massUnpickPkgControlItem.ItemAttributeSetID, massUnpickPkgControlItem.ItemIUM);
                            if (PkgControlItem != null)
                            {
                                massUnpickPkgControlItem.DemandType = string.Empty;
                                massUnpickPkgControlItem.CustPONum = string.Empty;
                                massUnpickPkgControlItem.OrderNum = 0;
                                massUnpickPkgControlItem.OrderLine = 0;
                                massUnpickPkgControlItem.OrderRelNum = 0;
                                massUnpickPkgControlItem.TFOrdNum = string.Empty;
                                massUnpickPkgControlItem.TFOrdLine = 0;
                                Db.Validate(massUnpickPkgControlItem);
                            }
                        }
                    }
                }

                // !!!!!!!!!! Developer warning - do not delete this commented code!!!!!!!!
                // As of 10.2.600 release, the user is not able to change the location of the PkgControlHeader during mass unpick, 
                // so the code within this if statement should never be getting executed and has been commented out but SHOULD NOT be deleted.
                // The move to a new location during mass unpick is being restricted by validations in the UnpickTransaction BO, 
                // but that does not prevent custom code from directly calling MassUnpickByPCID method.
                // The intention of the logic is to move the top level PCID to a new location at the same time that it is getting mass unpicked.
                // This logic is not completed or tested and is known to corrupt data if somehow data comes through where to whs/bin is not the same as the
                // top level PkgControlHeader current whs/bin, but should be left in place per Dave York's instructions. 
                // If the restrictions for changing the to whs/bin during unpick is lifted, this code will need updates at the time it is uncommented. See ERPS-134227
                // !!!!!!!!!! Developer warning - do not delete this commented code !!!!!!!!
                //if (string.IsNullOrEmpty(partNum) &&
                //    (massUnpickPkgControlHeader.PkgControlStatus.Equals("SOPICK", StringComparison.OrdinalIgnoreCase)
                //    || massUnpickPkgControlHeader.PkgControlStatus.Equals("TFOPICK", StringComparison.OrdinalIgnoreCase))
                //    && (!massUnpickPkgControlHeader.WarehouseCode.Equals(whse, StringComparison.OrdinalIgnoreCase)
                //    || !massUnpickPkgControlHeader.BinNum.Equals(bin, StringComparison.OrdinalIgnoreCase)))
                //{
                //    GetNewIssueReturn("STK-STK", new Guid(), (string.IsNullOrEmpty(partNum) ? "UnpickPCID" : "Unpick"), ref ds);
                //    ttIssueReturn = (from ttIssueReturn_Row in CurrentFullTableset.IssueReturn
                //                     where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                //                     select ttIssueReturn_Row).FirstOrDefault();
                //    if (ttIssueReturn == null)
                //    {
                //        throw new BLException(Strings.IssueReturnRecordNotFound);
                //    }

                //    //djyXXX
                //    ttIssueReturn.ProcessID = "UnpickPCID";
                //    ttIssueReturn.TFOrdNum = string.Empty;
                //    ttIssueReturn.TFOrdLine = 0;
                //    ttIssueReturn.OrderNum = 0;
                //    ttIssueReturn.OrderLine = 0;
                //    ttIssueReturn.OrderRel = 0;
                //    ttIssueReturn.PartNum = string.Empty;
                //    ttIssueReturn.LotNum = string.Empty;
                //    ttIssueReturn.FromWarehouseCode = massUnpickPkgControlHeader.WarehouseCode;
                //    ttIssueReturn.FromBinNum = massUnpickPkgControlHeader.BinNum;
                //    ttIssueReturn.FromPCID = massUnpickPkgControlHeader.PCID;
                //    ttIssueReturn.TranQty = 1;
                //    ttIssueReturn.UM = string.Empty;
                //    ttIssueReturn.ToWarehouseCode = whse;
                //    ttIssueReturn.ToBinNum = bin;
                //    ttIssueReturn.ToPCID = massUnpickPkgControlHeader.PCID;
                //    ttIssueReturn.TranReference = "PCIDUNPICK";

                //    if (!String.IsNullOrEmpty(tranDocTypeID))
                //    {
                //        ttIssueReturn.TranDocTypeID = tranDocTypeID;
                //    }

                //    PerformMaterialMovement(true, ref ds, out legalNumMsg, out partTranPKs);

                //    ds.IssueReturn.Clear();
                //    ds.SelectedSerialNumbers.Clear();
                //}
                // !!!!!!!!!! Developer warning - do not delete the above commented code !!!!!!!!

                //Adjust container for parent and child PCIDs when partnum is not used
                if (String.IsNullOrEmpty(partNum))
                {
                    this.LibAdjustReturnContainer.AdjustContainer(pcid, "Picking", 1);     //HAVE TO DO IT ONLY ONCE PER PCID
                }

                Db.Validate();
                txScope.Complete();
            }

            if ((isStatic && isBusy)
                || ((!isStatic) && (isSoPick || isTfoPick) && !isChild))
                PCIDUpdateAfterMassUnpickByPCID(pcid, partNum);
        }

        // For regular parts, returns Part.IUM. For multi-tracked parts, returns ttIssueReturn.UM
        // Requires ttIssueReturn
        private string getInventoryUOM()
        {
            if (Part == null)
            {
                Part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
                if (Part == null)
                    throw new BLException(Strings.PartNotFound);
            }

            if (Part.TrackDimension)
                return ttIssueReturn.UM;

            return Part.IUM;
        }

        private void pcidUpdate(ref IssueReturnRow ttIssueReturn, string process, string topLevelPCID)
        {
            // !!! Developer Warning....the ttIssueReturn buffer here could be an independent/specific single IssueReturn being processed or it could be
            // a pcidIssueReturnRow (a child ttIssueReturn for a top level PCID ttIssueReturn that is being processed). In the case where it is for
            // a pcidIssueReturnRow the data in the global ttIssueReturn will be the top level parent PCID ttIssueReturn row. Caution must be taken
            // in this method and any method that it calls to distinguish between the "global" ttIssueReturn buffer and the ttIssueReturn buffer
            // being processed here.!!!

            string PackingPkgCode = string.Empty;

            if (process.Equals("MaterialQueue", StringComparison.OrdinalIgnoreCase) || process.Equals("HHMaterialQueue", StringComparison.OrdinalIgnoreCase) || process.Equals("HHAutoSelectTransactions", StringComparison.OrdinalIgnoreCase))
            {
                if (ttIssueReturn == null)
                    throw new BLException(Strings.IssueReturnRecordNotFound);

                if (String.IsNullOrEmpty(ttIssueReturn.ToPCID))
                    return;

                var isTransfer = ttIssueReturn.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase);
                if (isTransfer)
                {
                    if (string.IsNullOrEmpty(ttIssueReturn.TFOrdNum)) return;
                }
                else
                {
                    // Only sales order picks are supported at the moment - other transaction types will require different statuses, validations, etc
                    if ((!ttIssueReturn.TranType.Equals("STK-SHP", StringComparison.OrdinalIgnoreCase) && !ttIssueReturn.TranType.Equals("CMP-SHP", StringComparison.OrdinalIgnoreCase)) || ttIssueReturn.OrderNum == 0)
                        return;
                }

                // Update PkgControlHeader
                PkgControlHeader = FindFirstPkgControlHeaderWithUpdLock(ttIssueReturn.Company, ttIssueReturn.ToPCID);
                if (PkgControlHeader == null)
                    throw new BLException(Strings.PCIDNotFound(ttIssueReturn.ToPCID));

                //Set default pkgCode from the PkgControl 
                PkgControl PkgControl = FindFirstPkgControl(ttIssueReturn.Company, ttIssueReturn.Plant, ttIssueReturn.PkgControlID);
                if (PkgControl != null && string.IsNullOrEmpty(PkgControlHeader.PkgCode))
                {
                    PkgControlHeader.PkgCode = PkgControl.PkgCode;

                    //this code was added to set the package code to be the same case as the entry in Packing table.
                    PackingPkgCode = this.GetPackingPkgCode(Session.CompanyID, PkgControl.PkgCode);
                    PkgControlHeader.PkgCode = (PackingPkgCode != null ? PackingPkgCode : PkgControlHeader.PkgCode);
                }

                // Check status and custnum/shipto
                if (PkgControlHeader.PkgControlType.Equals("STATIC", StringComparison.OrdinalIgnoreCase))
                {
                    if (PkgControlHeader.PkgControlStatus.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                    {
                        PkgControlHeader.PkgControlPriorStatus = PkgControlHeader.PkgControlStatus;
                        PkgControlHeader.PkgControlStatus = "BUSY";
                        PkgControlHeader.WarehouseCode = ttIssueReturn.ToWarehouseCode;
                        PkgControlHeader.BinNum = ttIssueReturn.ToBinNum;

                        this.LibAdjustReturnContainer.AdjustContainer(PkgControlHeader.PCID, "Picking", -1);     //HAVE TO DO IT ONLY ONCE PER PCID

                    }
                    else if (PkgControlHeader.PkgControlStatus.Equals("BUSY", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!isTransfer)
                        {
                            pcidCheckCustShipTo(PkgControlHeader, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel, ttIssueReturn.ToPCID, topLevelPCID);
                        }

                        pcidCheckLocation(PkgControlHeader, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum);
                    }
                    else
                        throw new BLException(Strings.PCIDStatusInvalid(ttIssueReturn.ToPCID));
                }
                else
                {
                    if (PkgControlHeader.PkgControlStatus.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                    {
                        PkgControlHeader.PkgControlPriorStatus = PkgControlHeader.PkgControlStatus;

                        if (!isTransfer)
                        {
                            PkgControlHeader.PkgControlStatus = "SOPICK";
                        }
                        else
                        {
                            PkgControlHeader.PkgControlStatus = "TFOPICK";
                        }

                        if (PkgControlHeader.LabelPrintControlled)
                        {
                            PkgControlHeader.LabelPrintControlPriorStatus = PkgControlHeader.LabelPrintControlStatus;
                            PkgControlHeader.LabelPrintControlStatus = "SOPICK";

                        }
                        PkgControlHeader.WarehouseCode = ttIssueReturn.ToWarehouseCode;
                        PkgControlHeader.BinNum = ttIssueReturn.ToBinNum;

                        this.LibAdjustReturnContainer.AdjustContainer(PkgControlHeader.PCID, "Picking", -1);     //HAVE TO DO IT ONLY ONCE PER PCID

                    }
                    else if (PkgControlHeader.PkgControlStatus.Equals("SOPICK", StringComparison.OrdinalIgnoreCase) ||
                             PkgControlHeader.PkgControlStatus.Equals("TFOPICK", StringComparison.OrdinalIgnoreCase) ||
                             PkgControlHeader.PkgControlStatus.Equals("CHILD", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!isTransfer)
                        {
                            pcidCheckCustShipTo(PkgControlHeader, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel, ttIssueReturn.ToPCID, topLevelPCID);
                        }

                        pcidCheckLocation(PkgControlHeader, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum);
                    }
                    else
                        throw new BLException(Strings.PCIDStatusInvalid(ttIssueReturn.ToPCID));
                }

                pcidCheckRestrictions(PkgControlHeader, ttIssueReturn);
                pcidUpdatePkgControlHeader(ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel, ttIssueReturn.ToPCID, ttIssueReturn.TranType, topLevelPCID);
                Db.Validate(PkgControlHeader);

                if (!string.IsNullOrEmpty(ttIssueReturn.ToPCID))
                {
                    // Update MtlQueue with PCID (in case partial qty was handled - PCID will default in for subsequent transactions)
                    MtlQueue mtlQueue = FindFirstMtlQueueWithUpdLock(ttIssueReturn.MtlQueueRowId);
                    if (mtlQueue != null)
                    {
                        mtlQueue.LastUsedPCID = ttIssueReturn.ToPCID;
                        Db.Validate(mtlQueue);
                    }
                }

                //Create or update PkgControlItem record
                //TODO: we want the PartTran trigger to do this in the future, but it's not ready yet so this will still do it for now
                updatePkgControlItem(ref ttIssueReturn);

                pcidSetLabelValues();

                if ((!String.IsNullOrEmpty(ttIssueReturn.FromPCID))
                    && (!String.IsNullOrEmpty(ttIssueReturn.ToPCID))
                    && (!ttIssueReturn.FromPCID.KeyEquals(ttIssueReturn.ToPCID)))
                {
                    fromPCIDStatusUpdate(ttIssueReturn.Company, ttIssueReturn.FromPCID);
                }
            }
        }

        /// <summary>
        /// PCIDUpdateAfterMassUnpickByPCID - makes any remaining necessary PCID updates if the PCID has not been removed during mass unpick 
        /// If the optional partNum is passed IssueReturn will only Unpick that part
        /// This logic was moved from the UnpickTransaction BO for performance and to ensure that it gets executed regardless of where
        /// MassUnpickByPCID is invoked from (i.e. REST, etc) it only needs to execute against the top level PCID that is being mass-unpicked.
        /// </summary>
        private void PCIDUpdateAfterMassUnpickByPCID(string pcid, string partNum)
        {
            Erp.Tables.PkgControlHeader pkgControlHeader;

            bool skippedRows = false;

            var PartialRowPkgControlHeader = FindFirstPkgControlHeaderPartial2(Session.CompanyID, pcid);
            // It's possible that the PkgControlHeader in was completely emptied or moved to stock or history 
            // during the IssueReturn mass unpick PCID processing. If it does not currently exist there is nothing further to do.
            if (PartialRowPkgControlHeader == null)
            {
                return;
            }

            bool isStatic = PartialRowPkgControlHeader.PkgControlType.Equals("STATIC", StringComparison.OrdinalIgnoreCase);
            bool archivePCID = PartialRowPkgControlHeader.ArchivePCIDHistory;

            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
            {
                if (!string.IsNullOrEmpty(partNum))
                {
                    skippedRows = this.MassUnpickPCIDContents(pcid, partNum, isStatic, archivePCID, skippedRows);
                    if (!skippedRows)
                    {
                        pkgControlHeader = FindFirstPkgControlHeaderWithUpdLock(Session.CompanyID, pcid);
                        if ((pkgControlHeader != null) && !pkgControlHeader.PkgControlStatus.KeyEquals("EMPTY"))
                        {
                            pkgControlHeader.PkgControlPriorStatus = pkgControlHeader.PkgControlStatus;
                            pkgControlHeader.PkgControlStatus = "EMPTY";
                            pkgControlHeader.WarehouseCode = String.Empty;
                            pkgControlHeader.BinNum = String.Empty;

                            LibPackageControl.ClearPCIDShipToInfo(Session.CompanyID, pcid);
                            Db.Validate(pkgControlHeader);

                            this.LibAdjustReturnContainer.AdjustContainer(pcid, "Picking", 1);     //HAVE TO DO IT ONLY ONCE PER PCID
                        }
                    }
                }
                else
                {
                    skippedRows = this.MassUnpickPCIDContents(pcid, partNum, isStatic, archivePCID, skippedRows);

                    pkgControlHeader = FindFirstPkgControlHeaderWithUpdLock(Session.CompanyID, pcid);
                    if ((pkgControlHeader != null)
                        && (!pkgControlHeader.PkgControlStatus.KeyEquals("STOCK"))
                        && (!pkgControlHeader.PkgControlStatus.KeyEquals("CHILD")))
                    {
                        pkgControlHeader.PkgControlPriorStatus = pkgControlHeader.PkgControlStatus;
                        pkgControlHeader.PkgControlStatus = "STOCK";

                        ClearSalesOrderInfoFromPCIDContents(pkgControlHeader.Company, pkgControlHeader.PCID);

                        LibPackageControl.ClearPCIDShipToInfo(Session.CompanyID, pcid);
                        Db.Validate(PkgControlHeader);
                    }
                }
                Db.Validate();
                txScope.Complete();
            }
        }

        private bool MassUnpickPCIDContents(string ipPCID, string ipPartNum, bool ipIsStatic, bool ipArchivePCID, bool ipSkippedRows)
        {
            IEnumerable<PkgControlItem> PkgControlItemRows = SelectPkgControlItemWithUpdLock(Session.CompanyID, ipPCID);
            foreach (PkgControlItem pkgControlItem_iterator in PkgControlItemRows)
            {
                PkgControlItem = pkgControlItem_iterator;
                // If the user has passed us a specific PartNum to remove from the PCID, we will only 
                // be removing anything from that level, not removing anything from any nested children
                if (String.IsNullOrEmpty(ipPartNum) && !string.IsNullOrEmpty(PkgControlItem.ItemPCID))
                {
                    ipSkippedRows = this.MassUnpickPCIDContents(PkgControlItem.ItemPCID, ipPartNum, ipIsStatic, ipArchivePCID, ipSkippedRows);
                }
                else
                {
                    if (!String.IsNullOrEmpty(ipPartNum))
                    {
                        if (!PkgControlItem.ItemPartNum.Equals(ipPartNum, StringComparison.OrdinalIgnoreCase))
                        {
                            ipSkippedRows = true;
                            continue;
                        }
                        else
                        {
                            if (!ipIsStatic && ipArchivePCID)
                            {
                                LibPackageControl.CreateHXPkgControlItem(PkgControlItem.Company, PkgControlItem.PCID, PkgControlItem.PCIDItemSeq);
                            }

                            Db.PkgControlItem.Delete(PkgControlItem);
                        }
                    }
                }
            }
            return ipSkippedRows;
        }

        private void ClearSalesOrderInfoFromPCIDContents(string ipCompany, string ipPCID)
        {
            Erp.Tables.PkgControlHeader pkgControlHeader;
            Erp.Tables.PkgControlItem pkgControlItem;

            pkgControlHeader = FindFirstPkgControlHeaderWithUpdLock(ipCompany, ipPCID);
            if (pkgControlHeader != null)
            {
                foreach (var pkgControlItem_iterator in SelectPkgControlItemWithUpdLock(ipCompany, ipPCID))
                {
                    pkgControlItem = pkgControlItem_iterator;

                    if (!string.IsNullOrEmpty(pkgControlItem.ItemPCID))
                    {
                        ClearSalesOrderInfoFromPCIDContents(ipCompany, pkgControlItem.ItemPCID);
                    }

                    pkgControlItem.DemandType = string.Empty;
                    pkgControlItem.OrderNum = 0;
                    pkgControlItem.OrderLine = 0;
                    pkgControlItem.OrderRelNum = 0;
                    pkgControlItem.TFOrdNum = string.Empty;
                    pkgControlItem.TFOrdLine = 0;
                    Db.Validate(pkgControlItem);
                }

                LibPackageControl.ClearPCIDShipToInfo(Session.CompanyID, ipPCID);
            }
        }

        /// <summary>
        /// Run set of validations when updating a PCID.
        /// </summary>
        /// <param name="isPCIDPick">is PCIDPick</param>
        /// <param name="tranType">Transaction Type</param>
        /// <param name="pcid">(From) PCID being updated</param>
        /// <param name="companyID">Current Company</param>
        /// <param name="plantID">Current Site</param>
        /// <param name="libPackageControlValidations">Internal.Lib.PackageControlValidations</param>
        /// <param name="pkgControlHeaderRow">pkgControlHeaderRow</param>
        internal static void pcidUpdateValidations(bool isPCIDPick, string tranType, string pcid, string companyID, string plantID, Internal.Lib.PackageControlValidations libPackageControlValidations, out Erp.Internal.Lib.PackageControlValidations.PkgControlHeaderPartialRow pkgControlHeaderRow)
        {
            HashSet<string> controlStatus = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if ((tranType.Compare("MTL-STK") == 0) ||
                (tranType.Compare("MTL-ASM") == 0) ||
                (tranType.Compare("MTL-MTL") == 0))
            {
                controlStatus.Add(PkgControlStatus.Wip);
            }
            else if (tranType.Compare("STK-PLT") == 0)
            {
                controlStatus.Add(PkgControlStatus.Stock);
                controlStatus.Add(PkgControlStatus.TFOPick);
                controlStatus.Add(PkgControlStatus.Child);
            }
            else
            {
                controlStatus.Add(PkgControlStatus.Stock);
            }

            pkgControlHeaderRow = null;
            pkgControlHeaderRow = libPackageControlValidations.ValidatePCID(companyID, pcid, plantID, controlStatus);

            if (tranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))
            {
                if (!isPCIDPick)
                {
                    libPackageControlValidations.PCIDDoesNotContainAnyItemPartNum(companyID, pcid, true);
                }
            }
            else if (!tranType.Equals("STK-MTL", StringComparison.OrdinalIgnoreCase))
            {
                libPackageControlValidations.PCIDIsNotAChildValidation(companyID, pcid, true);
                libPackageControlValidations.PCIDDoesNotContainAnyItemPartNum(companyID, pcid, true);
            }

            libPackageControlValidations.PkgControlHeaderIsNotLabelPrintControlledValidation(pkgControlHeaderRow.LabelPrintControlled, true);
            libPackageControlValidations.PkgControlIDDoesNotExistInSiteOrWhse(companyID, pcid, plantID, pkgControlHeaderRow.Plant, pkgControlHeaderRow.WarehouseCode, true);
        }

        /// <summary>
        /// Set PkgControlHeader label values
        /// </summary>
        private void pcidSetLabelValues()
        {
            if (PkgControlHeader == null) return;

            if (PkgControlHeader.LabelPrintControlled)
            {
                // find PkgControlLabelValue and assign LabelValue01 through LabelValue30
                PkgControlLabelValue = this.FindFirstPkgControlLabelValue(PkgControlHeader.Company, PkgControlHeader.Plant, PkgControlHeader.CustNum, PkgControlHeader.ShipToNum, PkgControlItem.ItemPartNum);
                if (PkgControlHeader != null && PkgControlLabelValue != null)
                {
                    PkgControlHeader.LabelValue01 = PkgControlLabelValue.LabelValue01;
                    PkgControlHeader.LabelValue02 = PkgControlLabelValue.LabelValue02;
                    PkgControlHeader.LabelValue03 = PkgControlLabelValue.LabelValue03;
                    PkgControlHeader.LabelValue04 = PkgControlLabelValue.LabelValue04;
                    PkgControlHeader.LabelValue05 = PkgControlLabelValue.LabelValue05;
                    PkgControlHeader.LabelValue06 = PkgControlLabelValue.LabelValue06;
                    PkgControlHeader.LabelValue07 = PkgControlLabelValue.LabelValue07;
                    PkgControlHeader.LabelValue08 = PkgControlLabelValue.LabelValue08;
                    PkgControlHeader.LabelValue09 = PkgControlLabelValue.LabelValue09;
                    PkgControlHeader.LabelValue10 = PkgControlLabelValue.LabelValue10;
                    PkgControlHeader.LabelValue11 = PkgControlLabelValue.LabelValue11;
                    PkgControlHeader.LabelValue12 = PkgControlLabelValue.LabelValue12;
                    PkgControlHeader.LabelValue13 = PkgControlLabelValue.LabelValue13;
                    PkgControlHeader.LabelValue14 = PkgControlLabelValue.LabelValue14;
                    PkgControlHeader.LabelValue15 = PkgControlLabelValue.LabelValue15;
                    PkgControlHeader.LabelValue16 = PkgControlLabelValue.LabelValue16;
                    PkgControlHeader.LabelValue17 = PkgControlLabelValue.LabelValue17;
                    PkgControlHeader.LabelValue18 = PkgControlLabelValue.LabelValue18;
                    PkgControlHeader.LabelValue19 = PkgControlLabelValue.LabelValue19;
                    PkgControlHeader.LabelValue20 = PkgControlLabelValue.LabelValue20;
                    PkgControlHeader.LabelValue21 = PkgControlLabelValue.LabelValue21;
                    PkgControlHeader.LabelValue22 = PkgControlLabelValue.LabelValue22;
                    PkgControlHeader.LabelValue23 = PkgControlLabelValue.LabelValue23;
                    PkgControlHeader.LabelValue24 = PkgControlLabelValue.LabelValue24;
                    PkgControlHeader.LabelValue25 = PkgControlLabelValue.LabelValue25;
                    PkgControlHeader.LabelValue26 = PkgControlLabelValue.LabelValue26;
                    PkgControlHeader.LabelValue27 = PkgControlLabelValue.LabelValue27;
                    PkgControlHeader.LabelValue28 = PkgControlLabelValue.LabelValue28;
                    PkgControlHeader.LabelValue29 = PkgControlLabelValue.LabelValue29;
                    PkgControlHeader.LabelValue30 = PkgControlLabelValue.LabelValue30;

                    if (PkgControlLabelValue.LabelValue02.Length > 50) PkgControlHeader.ShipToDock = PkgControlLabelValue.LabelValue02.Substring(0, 50);
                    else PkgControlHeader.ShipToDock = PkgControlLabelValue.LabelValue02;
                }
                Db.Validate(PkgControlHeader);    //Added this code to make sure no errors are thrown in the future
                                                  //without the validate, create of PkgControlItem was throwing errors.
            }
        }

        /// pcidCheckRestrictions
        /// <summary>
        /// Checks the restriction flags on PkgControlIDConfiguration
        /// Requires a current PkgControlHeader and ttIssueReturn
        /// </summary>
        private void pcidCheckRestrictions(PkgControlHeader PkgControlHeader, IssueReturnRow ttIssueReturn)
        {
            if (PkgControlHeader == null || ttIssueReturn == null)
                return;

            if (!PkgControlHeader.AllowMixedParts)
            {
                if (FindAnyPkgControlItemByPart(Session.CompanyID, PkgControlHeader.PCID, ttIssueReturn.PartNum))
                    throw new BLException(Strings.PCIDMixedPartsNotAllowed);
            }

            if (!PkgControlHeader.AllowMixedUOMs)
            {
                // Any other UOM tied to the PCID for any Part will cause this validation to fail
                string uom = String.Empty;
                Part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
                if (Part == null)
                    throw new BLException(Strings.PartNotFound);
                uom = Part.TrackDimension ? ttIssueReturn.UM : Part.IUM;

                if (FindAnyPkgControlItemByUOM(Session.CompanyID, PkgControlHeader.PCID, uom))
                    throw new BLException(Strings.PCIDMixedUOMNotAllowed);
            }

            if (!PkgControlHeader.AllowMixedLots && !String.IsNullOrEmpty(ttIssueReturn.LotNum))
            {
                // Any Lot Num tied to the PCID for any Part that doesn't match the current Lot Num and is not blank will cause this validation to fail
                if (FindAnyPkgControlItemByLot(Session.CompanyID, PkgControlHeader.PCID, ttIssueReturn.PartNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID))
                    throw new BLException(Strings.PCIDMixedLotNumbersNotAllowed);
            }

            if (!PkgControlHeader.AllowMultipleSerialNumPerPCID && ttIssueReturn.PartTrackSerialNum)
            {
                int ssnCount = 0;

                IEnumerable<SelectedSerialNumbersRow> ssnRows = (from ttSelectedSerialNumbers_Row in CurrentFullTableset.SelectedSerialNumbers
                                                                 where !ttSelectedSerialNumbers_Row.Deselected
                                                                 select ttSelectedSerialNumbers_Row);
                ssnCount = ssnRows.Count();

                if (ssnCount > 1)
                    throw new BLException(Strings.PCIDMultipleSerialNumbersNotAllowed);

                if (ssnCount == 1)
                {
                    // Any Serial Number tied to the PCID for any Part will cause this validation to fail
                    if (FindAnySerialNoByPCID(Session.CompanyID, PkgControlHeader.PCID, ssnRows.FirstOrDefault().SerialNumber))
                        throw new BLException(Strings.PCIDMultipleSerialNumbersNotAllowed);
                }
            }
        }

        private void pcidUpdatePkgControlHeader(int orderNum, int orderLine, int orderRel, string toPCID, string tranType, string topLevelPCID)
        {
            if (!tranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))
            {
                if (PkgControlHeader == null)
                    throw new BLException(Strings.PCIDNotFound(toPCID));

                // Validate and retrieve orderdtl/orderrel
                pcidCheckCustShipTo(PkgControlHeader, orderNum, orderLine, orderRel, toPCID, topLevelPCID);

                // Set customer and shipto information
                PkgControlHeader.CustNum = OrderRel == null ? 0 : OrderRel.ShipToCustNum;
                string shipToNum = OrderRel == null ? String.Empty : OrderRel.ShipToNum;
                if (PkgControlHeader.CustNum != 0)
                    LibPackageControl.SetPCIDShipToInfo(Session.CompanyID, toPCID, PkgControlHeader.CustNum, shipToNum);
            }
        }

        /// <summary>
        /// Generate a dynamic PCID
        /// </summary>
        /// <param name="pkgControlID">Package control ID code</param>
        /// <returns>PCID</returns>
        public string GenerateDynamicPCID(string pkgControlID)
        {
            string pcid = String.Empty;
            pcid = LibPackageControl.InitializeDynamicPCID(Session.CompanyID, Session.PlantID, pkgControlID);
            return pcid;
        }

        /// <summary>
        /// Returns if the employee has the 'Suppress Print Messages' flag on
        /// </summary>
        /// <param name="empID">Employee ID</param>
        /// <returns>Suppress Print Messages</returns>
        public bool SuppressPrintMessages(string empID)
        {
            return LibPackageControl.SuppressPrintMessages(empID);
        }

        /// <summary>
        /// To verify if autoprint is setup
        /// </summary>
        /// <param name="ipWriteToStaged">write to staged table</param>
        /// <returns></returns>
        public bool IsAutoPrintSetup(bool ipWriteToStaged)
        {
            return this.LibPackageControl.IsAutoPrintSetup(ipWriteToStaged);
        }

        /// <summary>
        /// Validates a pkg control ID code
        /// </summary>
        /// <param name="pkgControlIDCode">Proposed package control ID code</param>
        public void ValidatePkgControlID(string pkgControlIDCode)
        {
            PkgControl PkgControl = FindFirstPkgControl(Session.CompanyID, Session.PlantID, pkgControlIDCode);
            if (PkgControl == null)
                throw new BLException(Strings.PCIDPkgControlIDCodeInvalid(pkgControlIDCode));
            if (!PkgControl.PkgControlType.Equals("DYNAMIC", StringComparison.OrdinalIgnoreCase))
                throw new BLException(Strings.PCIDPkgControlIDConfigNotDynamic(pkgControlIDCode));
        }

        private void updatePkgControlItem(ref IssueReturnRow ttIssueReturn)
        {
            OrderHed OrderHed;
            // UOM - handle multitracked parts
            string uom = String.Empty;
            var isTransfer = ttIssueReturn.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase);

            if (!string.IsNullOrEmpty(ttIssueReturn.PartNum))
            {
                Part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
                if (Part == null)
                    throw new BLException(Strings.PartNotFound);
                uom = Part.TrackDimension ? ttIssueReturn.UM : Part.IUM;

                // Check to see if PkgControlItem already exists - add to existing record if it does
                PkgControlItem = FindFirstPkgControlItemWithUpdLock(Session.CompanyID, ttIssueReturn.ToPCID, string.Empty, ttIssueReturn.PartNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, uom);
                if (PkgControlItem == null)
                {
                    PkgControlItem = new PkgControlItem();
                    PkgControlItem.Company = Session.CompanyID;
                    PkgControlItem.PCID = ttIssueReturn.ToPCID;
                    PkgControlItem.ItemPartNum = ttIssueReturn.PartNum;
                    PkgControlItem.ItemAttributeSetID = ttIssueReturn.AttributeSetID;
                    PkgControlItem.ItemLotNum = ttIssueReturn.LotNum;
                    PkgControlItem.ItemIUM = uom;
                    PkgControlItem.Plant = Session.PlantID;
                    PkgControlItem.DemandType = "Order";
                    PkgControlItem.PkgControlBoolean01 = true; // indicates the order info comes from picking/picked, used to determine whether to retain the demand data if the PCID is packed then unpacked
                    PkgControlItem.SafetyIndicator = Part.IsSafetyItem;

                    decimal convQty = ttIssueReturn.TranQty;
                    if (!ttIssueReturn.UM.Equals(uom, StringComparison.OrdinalIgnoreCase))
                        LibAppService.UOMConv(ttIssueReturn.PartNum, convQty, ttIssueReturn.UM, uom, out convQty, true);
                    PkgControlItem.ItemQuantity = convQty;

                    PkgControlItem altPCI = FindLastPkgControlItem(Session.CompanyID, ttIssueReturn.ToPCID);
                    PkgControlItem.PCIDItemSeq = altPCI != null ? altPCI.PCIDItemSeq + 1 : 1;

                    Part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
                    PkgControlItem.ItemPartDesc = Part != null ? Part.PartDescription : String.Empty;

                    Db.PkgControlItem.Insert(PkgControlItem);
                }
                else if (string.IsNullOrEmpty(ttIssueReturn.FromPCID))
                {
                    if (!isTransfer)
                    {
                        decimal convQty = ttIssueReturn.TranQty;
                        if (!ttIssueReturn.UM.Equals(uom, StringComparison.OrdinalIgnoreCase))
                            LibAppService.UOMConv(ttIssueReturn.PartNum, convQty, ttIssueReturn.UM, uom, out convQty, true);
                        PkgControlItem.ItemQuantity += convQty;
                    }
                }

                if (isTransfer)
                {
                    PkgControlItem.TFOrdNum = ttIssueReturn.TFOrdNum;
                    PkgControlItem.TFOrdLine = ttIssueReturn.TFOrdLine;
                }
                else
                {
                    PkgControlItem.OrderNum = ttIssueReturn.OrderNum;
                    PkgControlItem.OrderLine = ttIssueReturn.OrderLine;
                    PkgControlItem.OrderRelNum = ttIssueReturn.OrderRel;
                }

                OrderRel = FindFirstOrderRel(Session.CompanyID, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel);
                PkgControlItem.ItemRevisionNum = OrderRel != null ? OrderRel.RevisionNum : String.Empty;

                OrderHed = FindFirstOrderHed(Session.CompanyID, ttIssueReturn.OrderNum);
                PkgControlItem.CustPONum = OrderHed != null ? OrderHed.PONum : String.Empty;

                Db.Validate(PkgControlItem);
            }
        }

        // Check to see if ShipTo and CustNum and ERSOrder flags match on an in-use PCID
        // Requires PkgControlHeader for the ttIssueReturn that being validated/updated
        // in the case of processing pcidIssueReturnRows we need to compare address against the top level PCID that is being processed
        // to ensure that all pcidIssueReturnRows are for the same shipto location.
        private void pcidCheckCustShipTo(PkgControlHeader PkgControlHeader, int ordNum, int ordLine, int ordRel, string toPCID, string topLevelPCID)
        {
            if (PkgControlHeader == null)
                throw new BLException(Strings.PCIDNotFound(toPCID));

            PkgControlHeaderPartialRow2 topLevelPkgControlHeader = null;
            if ((!toPCID.KeyEquals(topLevelPCID)) && !String.IsNullOrEmpty(topLevelPCID))
            {
                topLevelPkgControlHeader = FindFirstPkgControlHeaderCustInfo(PkgControlHeader.Company, topLevelPCID);
                if (topLevelPkgControlHeader == null)
                    throw new BLException(Strings.PCIDNotFound(topLevelPCID));
            }

            int custNumPkg = topLevelPkgControlHeader != null ? topLevelPkgControlHeader.CustNum : PkgControlHeader.CustNum;
            string shipToPkg = topLevelPkgControlHeader != null ? topLevelPkgControlHeader.ShipToNum : PkgControlHeader.ShipToNum;
            // !!! do not change OrderRel to a local var here, at least one caller depends on it getting populated. !!!
            OrderRel = FindFirstOrderRel(Session.CompanyID, ordNum, ordLine, ordRel);
            string orderLineRel = Compatibility.Convert.ToString(ordNum) + "/" + Compatibility.Convert.ToString(ordLine) + "/" + Compatibility.Convert.ToString(ordRel);
            int custNumIR = OrderRel != null ? OrderRel.ShipToCustNum : custNumPkg;
            if (custNumPkg > 0 && custNumPkg != custNumIR)
                throw new BLException(Strings.PCIDCustNoMatch(orderLineRel, topLevelPkgControlHeader != null ? topLevelPCID : PkgControlHeader.PCID));

            string shipToIR = OrderRel != null ? OrderRel.ShipToNum : shipToPkg;
            if (custNumPkg > 0 && !shipToPkg.Equals(shipToIR, StringComparison.OrdinalIgnoreCase))
                throw new BLException(Strings.PCIDCustNoMatch(orderLineRel, topLevelPkgControlHeader != null ? topLevelPCID : PkgControlHeader.PCID));

            // if we get here then customer number and shipTo match. If ship to is blank we have to check for the
            // possibility of OTS address mismatch, because blank in OrderRel.ShipToNum and/or PkgControlHeader.ShiptoNum  
            // happens if the release is for the customer's main address or if the release is using an OTS.
            if (custNumPkg > 0 && OrderRel != null && String.IsNullOrEmpty(OrderRel.ShipToNum))
            {
                PartAllocResult partialRowPartAlloc = FindFirstPCIDOrderRelPartAlloc(Session.CompanyID, PkgControlHeader.PCID, "ORDER", ordNum, ordLine, ordRel);
                if (partialRowPartAlloc != null)
                {
                    string orderRelOTSName = string.Empty;
                    string orderRelOTSZip = string.Empty;
                    string pcidOTSName = string.Empty;
                    string pcidOTSZip = string.Empty;
                    getOrderRelOTSInfo(Session.CompanyID, ordNum, ordLine, ordRel, out orderRelOTSName, out orderRelOTSZip);
                    getOrderRelOTSInfo(Session.CompanyID, partialRowPartAlloc.OrderNum, partialRowPartAlloc.OrderLine, partialRowPartAlloc.OrderRelNum, out pcidOTSName, out pcidOTSZip);
                    if ((!pcidOTSName.Equals(orderRelOTSName, StringComparison.OrdinalIgnoreCase))
                     || (!pcidOTSZip.Equals(orderRelOTSZip, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new BLException(Strings.PCIDCustNoMatch(orderLineRel, topLevelPkgControlHeader != null ? topLevelPCID : PkgControlHeader.PCID));
                    }
                }
            }

            // verify that the ToPCID does not already contain PkgControlItem for a sales order with a different ERSOrder value than the order/line/rel that is being added
            ValidatePCIDERSOrder(PkgControlHeader.Company, toPCID, ordNum);
        }

        private void ValidatePCIDERSOrder(string ipCompany, string toPCID, int orderNum)
        {
            // validate that the ERSOrder flag on the OrderHed being added to the PCID
            // does not conflict with the ERSOrder flag for orders already on the PCID
            OrderHedPartialRow orderHeadPartialRow = FindFirstOrderHedERSOrder(ipCompany, orderNum);
            if (orderHeadPartialRow != null)
            {
                RecursiveCheckPCIDERSOrderMatch(ipCompany, toPCID, orderNum, orderHeadPartialRow.ERSOrder, toPCID);
            }
        }

        private void RecursiveCheckPCIDERSOrderMatch(string ipCompany, string validatingPCID, int checkOrderNum, bool checkERSOrderValue, string toPCID)
        {
            PkgControlItemPartialRow2 pkgControlItemAlt = null;
            var pcidChildren = SelectNestedPCIDsItems(ipCompany, validatingPCID, checkOrderNum);
            foreach (PkgControlItemPartialRow2 pcidNestedItem in pcidChildren)
            {
                pkgControlItemAlt = pcidNestedItem;
                if (!String.IsNullOrEmpty(pkgControlItemAlt.ItemPCID))
                {
                    RecursiveCheckPCIDERSOrderMatch(ipCompany, pkgControlItemAlt.ItemPCID, checkOrderNum, checkERSOrderValue, toPCID);
                }

                if ((pkgControlItemAlt.OrderNum != 0) && pkgControlItemAlt.OrderNum != checkOrderNum)
                {
                    OrderHedPartialRow orderHeadPartialRow = FindFirstOrderHedERSOrder(ipCompany, pkgControlItemAlt.OrderNum);
                    // Stop checking as soon as we hit any PkgControlItem that references an OrderNum with a different ERSOrder
                    // than the one being added, since all of them already associated to the same PCID must have the same ERSOrder value 
                    if (orderHeadPartialRow != null)
                    {
                        if (checkERSOrderValue != orderHeadPartialRow.ERSOrder)
                        {
                            throw new BLException(Strings.PCIDOrderNumHasInvalidERSOrder(toPCID));
                        }
                    }
                }
            }
        }

        private void getOrderRelOTSInfo(string company, int orderNum, int orderLine, int orderRelNum, out string orderRelOTSName, out string orderRelOTSZip)
        {
            orderRelOTSName = string.Empty;
            orderRelOTSZip = string.Empty;
            if (orderNum == 0 || orderLine == 0 || orderRelNum == 0) { return; }
            OrderRelOTSInfoResult partialRowOrderRel = FindFirstOrderRelOTS(company, orderNum, orderLine, orderRelNum);
            if (partialRowOrderRel == null) { return; }
            if (!partialRowOrderRel.UseOTS)
            {
                OrderHedOTSInfoResult partialRowOrderHed = FindFirstOrderHedOTS(company, partialRowOrderRel.OrderNum);
                if (partialRowOrderHed != null && partialRowOrderHed.UseOTS && string.IsNullOrEmpty(partialRowOrderRel.ShipToNum))
                {
                    orderRelOTSName = partialRowOrderHed.OTSName;
                    orderRelOTSZip = partialRowOrderHed.OTSZIP;
                }
            }
            else
            {
                orderRelOTSName = partialRowOrderRel.OTSName;
                orderRelOTSZip = partialRowOrderRel.OTSZIP;
            }
        }

        // Check to see if to location matches what is already on the PkgControlHeader
        private void pcidCheckLocation(PkgControlHeader pkgControlHeader, string whse, string bin)
        {
            if (pkgControlHeader == null)
                throw new BLException(Strings.PCIDNotFound(ttIssueReturn.ToPCID));

            if (!pkgControlHeader.WarehouseCode.Equals(whse, StringComparison.OrdinalIgnoreCase) || !pkgControlHeader.BinNum.Equals(bin, StringComparison.OrdinalIgnoreCase))
                throw new BLException(Strings.PCIDLocationDoesNotMatch);
        }
        #endregion

        /// <summary>
        /// This method will check, depending on the Tran Type, if the available quantity
        /// has been reduced before the creation of the issue return.
        /// </summary>
        /// <param name="pcMtlQueueRowID">Progress database RowId of MtlQueue record</param>
        /// <param name="pcAction">The action to be taken.</param>
        /// <param name="pcMessage">Error message passed back from the business logic.</param>
        /// <param name="pdQtyAvailable">The on hand qty for the part bin specified in the material queue.</param>
        public void PreGetNewIssueReturn(Guid pcMtlQueueRowID, out string pcAction, out string pcMessage, out decimal pdQtyAvailable)
        {
            pcAction = string.Empty;
            pcMessage = string.Empty;
            pdQtyAvailable = decimal.Zero;
            if (String.IsNullOrEmpty(pcMtlQueueRowID.ToString()))
            {
                return;
            }

            /* ... and tran type is for replenishment */
            MtlQueue = this.FindFirstMtlQueue(pcMtlQueueRowID);
            if (MtlQueue != null)
            {
                if (MtlQueue.TranType.Equals("RAU-STK", StringComparison.OrdinalIgnoreCase) || MtlQueue.TranType.Equals("RMN-STK", StringComparison.OrdinalIgnoreCase) || MtlQueue.TranType.Equals("RMG-STK", StringComparison.OrdinalIgnoreCase) || MtlQueue.TranType.Equals("INS-STK", StringComparison.OrdinalIgnoreCase))
                {
                    string negQtyAction = string.Empty;
                    string negQtyMessage = string.Empty;
                    decimal qtyAvailable = decimal.Zero;
                    /* Do the negative inventory test and get the quantities. */
                    this.LibNegInvTest.NegativeInventoryTestGetQty(MtlQueue.PartNum, MtlQueue.FromWhse, MtlQueue.FromBinNum, MtlQueue.LotNum, MtlQueue.AttributeSetID, MtlQueue.FromPCID, MtlQueue.IUM, 1, MtlQueue.Quantity, true, true, true, string.Empty, out negQtyAction, out negQtyMessage, out qtyAvailable);
                    if (StringExtensions.Compare(negQtyAction, "STOP") == 0)
                    {
                        if (qtyAvailable == 0)
                        {
                            pcAction = "DELETED";
                            pcMessage = negQtyMessage + "\r\n" + Strings.QuantAvailToMoveHasBeenReducedToRequestHasBeen;
                            pdQtyAvailable = qtyAvailable;

                            //Transaction block
                            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                            {
                                MtlQueue = this.FindFirstMtlQueueWithUpdLock(pcMtlQueueRowID);
                                if (MtlQueue != null)
                                {
                                    Db.MtlQueue.Delete(MtlQueue);
                                }
                                Db.Validate();
                                txScope.Complete();
                            } //Transaction block end
                        }
                        else
                        {
                            pcAction = "DECREASED";
                            pcMessage = Strings.QuantAvailToMoveHasBeenReducedTo(qtyAvailable);
                            pdQtyAvailable = qtyAvailable;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method performs multiple functions:
        /// It will return a record in the LegalNumGenOpts datatable if
        /// a legal number is required for this transaction.  The RequiresUserInput
        /// flag will indicate if this legal number requires input from the user.  If
        /// it does, the LegalNumberPrompt business objects needs to be called to
        /// gather that information.  This method should be called when the user
        /// saves the record but before the Update method is called.
        /// 
        /// It will also auto populate ttSelectedSerialNumbers table under some conditions for eligible types of IR transactions
        /// </summary>
        /// <param name="ds">Issue Return data set</param>
        /// <param name="requiresUserInput">Indicates if the legal number requires user input</param>
        public void PrePerformMaterialMovement(ref IssueReturnTableset ds, out bool requiresUserInput)
        {
            requiresUserInput = false;
            CurrentFullTableset = ds;

            string cLegalNumberType = string.Empty;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where StringExtensions.Compare(ttIssueReturn_Row.RowMod, IceRow.ROWSTATE_UNCHANGED) != 0
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn == null)
            {
                return;
            }

            this.FillForeignFields(ttIssueReturn);
            cLegalNumberType = this.getLegalNumberType(ttIssueReturn.TranType);


            if (string.IsNullOrEmpty(ttIssueReturn.TranDocTypeID))
            {
                var tranDocTypeID = this.ExistsTranDocTypeID(ttIssueReturn.Company, cLegalNumberType);
                if (tranDocTypeID != null)
                    ttIssueReturn.TranDocTypeID = tranDocTypeID;
            }
            this.LibValidateTranDocType.RunValidateTranDocType(ttIssueReturn.TranDocTypeID, cLegalNumberType);

            if (!cLegalNumberType.KeyEquals("NonConf"))
            {
                this.LibLegalNumberGetDflts.GetDefaults(cLegalNumberType, ttIssueReturn.TranDocTypeID, ttIssueReturn.TranDate, CurrentFullTableset.LegalNumGenOpts);
            }

            ttLegalNumGenOpts = (from ttLegalNumGenOpts_Row in CurrentFullTableset.LegalNumGenOpts
                                 where StringExtensions.Compare(ttLegalNumGenOpts_Row.RowMod, IceRow.ROWSTATE_ADDED) == 0
                                 select ttLegalNumGenOpts_Row).FirstOrDefault();
            if (ttLegalNumGenOpts != null)
            {
                if (StringExtensions.Compare(ttLegalNumGenOpts.GenerationType, "manual") == 0)
                {
                    requiresUserInput = true;
                }
            }
            autoGenSelectedSerialNumbersForPCIDIssue(ttIssueReturn);
        }

        /// <summary>
        /// Adjust the WIP Material tracking information Warehouse/quantity (PartWip)
        /// </summary>
        private void process_ADJ_MTL(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
            {
                libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, false, ttIssueReturn.ToPCID, true, "ADJUSTMATERIAL");
            }

            this.updatePartWIP("TO", ttIssueReturn);
        }

        /// <summary>
        /// Adjust the WIP part tracking information Warehouse/quantity (PartWip)
        /// </summary>
        private void process_ADJ_WIP(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
            {
                libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, false, ttIssueReturn.ToPCID, true, "ADJUSTWIP");
            }

            this.updatePartWIP("TO", ttIssueReturn);
        }

        /// <summary>
        /// Perform the movement of assembly non-conformance from the reporting area
        /// to the inspection warehouse (ASM-INS).
        /// This function simply updates the Warehouse/Bin location on the NonConf.
        /// </summary>
        private void process_ASM_INS(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            using (Db.DisableTriggerScope(nameof(Erp.Tables.NonConf), TriggerType.Write))
            {
                if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement))
                {
                    MtlQueue = this.FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
                    if (MtlQueue != null)
                    {
                        NonConf = this.FindFirstNonConfWithUpdLock(MtlQueue.Company, MtlQueue.NCTranID);
                        if (NonConf != null)
                        {
                            NonConf.ToWarehouseCode = ttIssueReturn.ToWarehouseCode;
                            NonConf.ToBinNum = ttIssueReturn.ToBinNum;
                        }
                    }
                }
                if (NonConf != null)
                    Db.Validate(NonConf);
            }
        }

        ///<summary>
        ///  Purpose: Perform the update to the database for a ASM-STK (return OF ISSUED STOCK FROM A JOB ASSEMBLY
        ///  ) TRANSACTION
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_ASM_STK(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            decimal Trans_Cost = decimal.Zero;
            decimal Trans_MtlCost = decimal.Zero;
            decimal Trans_LbrCost = decimal.Zero;
            decimal Trans_BurCost = decimal.Zero;
            decimal Trans_SubCost = decimal.Zero;
            decimal Trans_MtlBurCost = decimal.Zero;
            decimal absMaxCost = decimal.Zero;
            decimal adjRoundingError = decimal.Zero;
            decimal Conv_Qty = decimal.Zero;
            /* Variables required for lib/AsmCostUpdate.i*/
            int parent_assemblyseq = 0;
            int Escape_Counter = 0;
            decimal Orig_PullQty = decimal.Zero;
            bool vSplitMfgCosts = false;
            bool vUpdMfgComponents = false;

            outMessage = string.Empty;

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part == null)
            {
                throw new BLException(Strings.AValidPartNumberIsRequired, "IssueReturn", "PartNum");
            }
            if (!this.ValidUOM(ttIssueReturn.PartNum, ttIssueReturn.UM))
            {
                throw new BLException(Strings.InvalidUOMForPartNumber, "IssueReturn", "UM");
            }
            okWhse = false;

            JobHead = this.FindFirstJobHead(Session.CompanyID, ttIssueReturn.FromJobNum);
            if (!JobHead.Plant.Equals(Session.PlantID, StringComparison.OrdinalIgnoreCase))
            {
                this.checkWarehouseBin(ttIssueReturn.ToWarehouseCode, JobHead.Plant, out okWhse);
                if (!okWhse)
                {
                    throw new BLException(Strings.InvalidToWarehouse, "IssueReturn", "ToWarehouseCode");
                }
            }

            /* SCR 170491 - Clear the ttReturnMaterialCosts to get ready for FIFO/LOTFIFO return costing logic. */
            ttReturnMaterialCostsRows.Clear();

            this.createPartTran(ttIssueReturn);
            PartTran.TranClass = "I";
            PartTran.TranType = "STK-ASM";  /* Flip from "ASM-STK" to issue tran type */
            PartTran.TranQty = -(PartTran.TranQty); /* Flip sign - makes it a negative issue */
            PartTran.ActTranQty = -(PartTran.ActTranQty); /* Flip sign - makes it a negative issue */
            PartTran.InventoryTrans = true;
            PartTran.JobNum = ttIssueReturn.FromJobNum;
            PartTran.AssemblySeq = ttIssueReturn.FromAssemblySeq;
            PartTran.JobSeq = ttIssueReturn.FromJobSeq;
            PartTran.PartNum = ttIssueReturn.PartNum;
            PartTran.PartDescription = ttIssueReturn.PartPartDescription;
            PartTran.WareHouseCode = ttIssueReturn.ToWarehouseCode;
            PartTran.BinNum = ttIssueReturn.ToBinNum;
            PartTran.PCID = ttIssueReturn.ToPCID;
            partTranPK = partTranPK + ((partTranPK.Length > 0) ? "|" : "");
            partTranPK = partTranPK + Compatibility.Convert.ToString(PartTran.SysDate) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.SysTime) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.TranNum);

            WhseBin = this.FindFirstWhseBin(Session.CompanyID, PartTran.WareHouseCode, PartTran.BinNum);
            PartTran.BinType = ((WhseBin != null) ? WhseBin.BinType : "");

            if (!Part.TrackInventoryByRevision)
            {
                /*run getPartRev.*/
                PartRev = this.FindLastPartRev(Session.CompanyID, ttIssueReturn.PartNum, CompanyTime.Today());
                if (PartRev != null)
                {
                    PartTran.RevisionNum = PartRev.RevisionNum;
                }
                else
                {
                    PartTran.RevisionNum = "";
                }
            }

            PartTran.Plant = getPlantFromWarehouse(PartTran.WareHouseCode);

            /* INITIALLY THE PLANT2 FIELD SHOULD BE SET TO BE THE SAME. */
            PartTran.Plant2 = PartTran.Plant;

            /* Get Cost method for the PartNum */
            PartTran.CostMethod = this.getPartCostMethod(PartTran.Plant, PartTran.PartNum);

            this.updatePartBin(PartTran);

            JobHead = this.FindFirstJobHead(Session.CompanyID, ttIssueReturn.FromJobNum);
            if (JobHead == null)
            {
                throw new BLException(Strings.AValidJobNumberIsRequired, "IssueReturn", "FromJobNum");
            }
            /* SCR #1669 - assign the service call number */
            PartTran.CallNum = JobHead.CallNum;
            PartTran.CallLine = JobHead.CallLine;
            PartTran.Plant2 = JobHead.Plant;

            /* SCR #40698 - Get the system setting for updating the Mfg Cost Elements and *
             * set flag to indicate if updating the Mfg component cost buckets.           */
            vSplitMfgCosts = JobHead.SplitMfgCostElements;
            vUpdMfgComponents = ((Part != null && StringExtensions.Compare(Part.TypeCode, "M") == 0) ? true : false);
            /* ISSUE TO ANOTHER PLANT ?  if YES then CHANGE THE TRAN TYPE TO STK-PLT */
            if (StringExtensions.Compare(PartTran.Plant, PartTran.Plant2) != 0)
            {
                PartTran.TranType = "STK-PLT";
            }
            /* SCR #40698 - Return stock quantity to FIFO OnHand and use the weighted *
             * FIFO average costs of previously issued quantities as new FIFO costs.  */
            var outCostID = PartTran.CostID;
            this.LibGetPlantCostID._getPlantCostID(PartTran.Plant, out outCostID, ref Plant, ref XaSyst);
            PartTran.CostID = outCostID;

            /* SCR #51749 - Create FIFO layers for non-FIFO cost methods if enabled */
            this.LibGetCostFIFOLayers.getPartCostFIFOLayers(PartTran.CostID, PartTran.PartNum, PartTran.Plant, out vEnableFIFOLayers);

            /* SCR 170491 - Implementing new logic when returning assembly with FIFO/LOTFIFO cost method.  *
             * Regular cost methods will continue to use the weighted average costs of all issued/returned *
             * transactions as costs of the qty returned to stock. FIFO/LOTFIFO will not use average costs *
             * but will return qty/costs from the original FIFO layers issued for this assembly. If return *
             * quantity will use more than one FIFO layer then the PartTran record will hold weighted FIFO *
             * Average Costs of the FIFO Layers to be returned.  Multiple PartFIFOTrans will be created to *
             * show the details of the new FIFO Layers returned.                                           *
             * NOTE: PartTran.xxxUnitCosts are now directly assigned in the getReturnMaterialCosts().      */
            this.getReturnMaterialCosts(ttIssueReturn, ref PartTran, ref ttReturnMaterialCostsRows, vEnableFIFOLayers);

            /* STK-PLT TRANSACTION - DO THE PLANT TO PLANT LOGIC */
            if (StringExtensions.Compare(PartTran.TranType, "STK-PLT") == 0)
            {
                this.IMIMPlant.CreatePlantTran(PartTran.TranType, JobHead.ProdCode, PartTran.TranQty, JobHead.CallNum, JobHead.CallLine, ref PartTran, ref PlantTran);
                /* FYI:  NO NEED TO assign G/L ACCOUNTS HERE for STK-PLT TRANSACTIONS.
                THEY ARE assignED IN THE lib/SetInterPlant.i LOGIC FOUND IN im/implant.i */
                /* SCR 170491 - If returning FIFO/LOTFIFO and ttReturnMaterialCosts has records then do not return. *
                 * We still need to process FIFO layers for return.                                                 */
                if (StringExtensions.Lookup("F,O", PartTran.CostMethod) == -1 && vEnableFIFOLayers == false)
                {
                    return;
                }
            }
            else /* NOT STK-PLT */
            {
                JobAsmbl = this.FindFirstJobAsmblWithUpdLock(Session.CompanyID, PartTran.JobNum, PartTran.AssemblySeq);
                if (JobAsmbl == null)
                {
                    throw new BLException(Strings.AValidJobAssemblyIsRequired, "IssueReturn", "AssemblySeq");
                }
                PartTran.ContractID = (JobAsmbl.LinkToContract) ? JobAsmbl.ContractID : "";

                // SCR 127565 - PartTran.ExtCost only gets updated/calculated when you save the PartTran. So before using
                // this field we just have to validate the PartTran record.
                Db.Validate(PartTran);
                UpdateLegalNumHistory(PartTran.LegalNumber, partTranPK);

                /* NOT A STK-PLT - CONTINUE PROCESSING THE ASM-STK TRANSACTION  getInvGl defined in lib/getglinv.i */
                this.LibAppService.UOMConv(PartTran.PartNum, PartTran.ActTranQty, PartTran.ActTransUOM, JobAsmbl.IUM, out Conv_Qty, false);
                JobAsmbl.IssuedQty = JobAsmbl.IssuedQty + Conv_Qty;
                JobAsmbl.IssuedComplete = ttIssueReturn.IssuedComplete;
                int nDec = 0;
                nDec = this.LibGetDecimalsNumber.getDecimalsNumberByName("JobAsmbl", "TotalCost", "");
                /* SCR 176758 - If the Extended Subcomponent Costs are available then use them instead of deriving costs from TranQty and UnitCosts */
                Trans_MtlCost = (PartTran.ExtMtlCost != 0) ? PartTran.ExtMtlCost : Math.Round(PartTran.TranQty * PartTran.MtlUnitCost, nDec, MidpointRounding.AwayFromZero);
                Trans_LbrCost = (PartTran.ExtLbrCost != 0) ? PartTran.ExtLbrCost : Math.Round(PartTran.TranQty * PartTran.LbrUnitCost, nDec, MidpointRounding.AwayFromZero);
                Trans_BurCost = (PartTran.ExtBurCost != 0) ? PartTran.ExtBurCost : Math.Round(PartTran.TranQty * PartTran.BurUnitCost, nDec, MidpointRounding.AwayFromZero);
                Trans_SubCost = (PartTran.ExtSubCost != 0) ? PartTran.ExtSubCost : Math.Round(PartTran.TranQty * PartTran.SubUnitCost, nDec, MidpointRounding.AwayFromZero);
                Trans_MtlBurCost = (PartTran.ExtMtlBurCost != 0) ? PartTran.ExtMtlBurCost : Math.Round(PartTran.TranQty * PartTran.MtlBurUnitCost, nDec, MidpointRounding.AwayFromZero);
                Trans_Cost = Trans_MtlCost + Trans_LbrCost + Trans_BurCost + Trans_SubCost + Trans_MtlBurCost;

                /* if ISSUED QTY IS NOW ZERO then BE SURE THAT THE COST IS ALSO */
                if (JobAsmbl.IssuedQty == 0 && StringExtensions.Lookup("F,O", PartTran.CostMethod) == -1)
                {
                    int nDecimals = 0;
                    nDecimals = this.LibGetDecimalsNumber.getDecimalsNumberByName("PartTran", "MtlUnitCost", "");
                    Trans_MtlCost = -(JobAsmbl.TotalMtlMtlCost);
                    Trans_LbrCost = -(JobAsmbl.TotalMtlLabCost);
                    Trans_BurCost = -(JobAsmbl.TotalMtlBurCost);
                    Trans_SubCost = -(JobAsmbl.TotalMtlSubCost);
                    Trans_MtlBurCost = -(JobAsmbl.MtlBurCost);
                    Trans_Cost = -(JobAsmbl.TotalCost);

                    /* Override the unit costs if NOT FIFO */
                    PartTran.MtlUnitCost = Math.Round(Trans_MtlCost / PartTran.TranQty, nDecimals, MidpointRounding.AwayFromZero);
                    PartTran.LbrUnitCost = Math.Round(Trans_LbrCost / PartTran.TranQty, nDecimals, MidpointRounding.AwayFromZero);
                    PartTran.BurUnitCost = Math.Round(Trans_BurCost / PartTran.TranQty, nDecimals, MidpointRounding.AwayFromZero);
                    PartTran.SubUnitCost = Math.Round(Trans_SubCost / PartTran.TranQty, nDecimals, MidpointRounding.AwayFromZero);
                    PartTran.MtlBurUnitCost = Math.Round(Trans_MtlBurCost / PartTran.TranQty, nDecimals, MidpointRounding.AwayFromZero);
                    /* ERPS-130913 - Using the ExtxxxCost fields to store the actual remaining issued costs of JobAsmbl to avoid rounding variances */
                    PartTran.ExtMtlCost = Trans_MtlCost;
                    PartTran.ExtLbrCost = Trans_LbrCost;
                    PartTran.ExtBurCost = Trans_BurCost;
                    PartTran.ExtSubCost = Trans_SubCost;
                    PartTran.ExtMtlBurCost = Trans_MtlBurCost;
                    PartTran.ExtMtlMtlCost = Trans_MtlCost;
                    PartTran.ExtMtlLabCost = 0;
                    PartTran.ExtMtlBurdenCost = 0;
                    PartTran.ExtMtlSubCost = 0;
                    // SCR 127565 - PartTran.ExtCost only gets updated/calculated when you save the PartTran. So before using
                    // this field we just have to validate the PartTran record.
                    Db.Validate(PartTran);
                }
                else
                {
                    /* Compute the rounding error */
                    adjRoundingError = PartTran.ExtCost - /* Total - Sum of the parts */
                                      (Trans_MtlCost + Trans_LbrCost + Trans_BurCost + Trans_SubCost + Trans_MtlBurCost);
                    /* SCR #59049 - Adjust the highest component cost with the rounding difference instead of just   *
                     * adding it to the material cost. This is to avoid negative cost in case material cost is zero. *
                     * It's important to keep this order (Mtl/Lbr/Bur/Sub) to check where to add the rounding error. */
                    if (adjRoundingError != 0)
                    {
                        absMaxCost = Math.Max(Math.Max(Math.Max(Math.Abs(Trans_MtlCost), Math.Abs(Trans_LbrCost)), Math.Abs(Trans_BurCost)), Math.Abs(Trans_SubCost));
                        if (Math.Abs(Trans_MtlCost) == absMaxCost)
                        {
                            Trans_MtlCost = Trans_MtlCost + adjRoundingError;
                        }
                        else if (Math.Abs(Trans_LbrCost) == absMaxCost)
                        {
                            Trans_LbrCost = Trans_LbrCost + adjRoundingError;
                        }
                        else if (Math.Abs(Trans_BurCost) == absMaxCost)
                        {
                            Trans_BurCost = Trans_BurCost + adjRoundingError;
                        }
                        else
                        {
                            Trans_SubCost = Trans_SubCost + adjRoundingError;
                        }
                    }
                    /* Make sure the total is right */
                    Trans_Cost = Trans_MtlCost + Trans_LbrCost + Trans_BurCost + Trans_SubCost;
                }

                this.LibAsmCostUpdate.runAsmCostUpdate(Escape_Counter, true, Trans_Cost, Trans_MtlCost, Trans_LbrCost, Trans_BurCost, Trans_SubCost, Trans_MtlBurCost, Orig_PullQty, parent_assemblyseq, vUpdMfgComponents, vSplitMfgCosts, PartTran, JobAsmbl);
            }  /* NOT STK-PLT */

            /* SCR 170491 - If returning FIFO/LOTFIFO then process the ttReturnMaterialCost records *
             * to create the FIFO layers to be returned to the FIFO queue.                          *
             * If returning NON-FIFO and vEnableFIFOLayers is true then create FIFO layer for the   *
             * alternate costs.                                                                     */
            /* NOTE: Both STK-ASM and STK-PLT will do the FIFO processing logic. */
            if (StringExtensions.Lookup("F,O", PartTran.CostMethod) > -1)
            {
                /* If the ttReturnMaterialCostsRows is empty then we only have 1 FIFO layer to return *
                 * and the FIFO info is already stored in the PartTran. If this is the case, we will  *
                 * process the FIFO layer from PartTran. Otherwise, process/return FIFO layers from   *
                 * the ttReturnMaterialCostsRows.                                                     */
                if (ttReturnMaterialCostsRows.Count == 0)
                {
                    if (PartTran.FIFODate == null)
                    {
                        PartTran.FIFODate = PartTran.TranDate;
                        PartTran.FIFOSeq = 0;
                    }
                    PartTran.FIFOAction = "A"; /* Add */
                    /* CreatePartFIFOCost procedure can be found in lib/ProcessFIFO.i */
                    var outFIFOSeq4 = PartTran.FIFOSeq;
                    var outFIFOSubSeq = PartTran.FIFOSubSeq;
                    this.LibProcessFIFO.CreatePartFIFOCost(true, true, PartTran.FIFODate, PartTran.FIFOSeq, 0, out outFIFOSeq4, out outFIFOSubSeq, ref PartTran);
                    PartTran.FIFOSeq = outFIFOSeq4;
                    PartTran.FIFOSubSeq = outFIFOSubSeq;
                }
                else
                {
                    LibProcessFIFO.ProcessReturnedFIFO(ref ttReturnMaterialCostsRows, ref PartTran, "STK-ASM");
                }
            }
            else if (vEnableFIFOLayers == true)
            {
                if (PartTran.FIFODate == null)
                {
                    PartTran.FIFODate = PartTran.TranDate;
                    PartTran.FIFOSeq = 0;
                }
                PartTran.FIFOAction = "NFA"; /* Non-FIFO Add */
                /* SCR #51749 - If FIFO Layers enabled then create FIFO cost for the non-FIFO costed part. */
                /* CreateNonFIFOCost procedure can be found in lib/ProcessFIFO.i */
                var outFIFOSeq5 = PartTran.FIFOSeq;
                var outFIFOSubSeq2 = PartTran.FIFOSubSeq;
                this.LibProcessFIFO.CreateNonFIFOCost(true, PartTran.FIFODate, PartTran.FIFOSeq, 0, PartTran.AltMtlUnitCost, PartTran.AltLbrUnitCost, PartTran.AltBurUnitCost, PartTran.AltSubUnitCost, PartTran.AltMtlBurUnitCost, PartTran.AltMtlUnitCost, 0, 0, 0, out outFIFOSeq5, out outFIFOSubSeq2, ref PartTran);
                PartTran.FIFOSeq = outFIFOSeq5;
                PartTran.FIFOSubSeq = outFIFOSubSeq2;
            } /* lookup(PartTran.CostMethod,"F,O") */

            /* Only STK-ASM transaction will call the updatePartWIP since STK-PLT will handle the Job update and PartWIP *
             * when the PLT-ASM is processed in IMPlant.CreatePlantTran.                                                 */
            if (PartTran.TranType.KeyEquals("STK-ASM"))
            {
                using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
                {
                    libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, true, ttIssueReturn.ToPCID, true, "RETURNASSEMBLY");
                }

                this.updatePartWIP("From", ttIssueReturn);
            }
        }

        ///<summary>
        ///  Purpose: Perform the movement of Pick Stock of KIT COMPONENT for Shipment  (CMP-SHP).
        ///           For the KIT COMPONENT
        ///               1. process as a STK-SHP.
        ///               2  Create ttIssueReturn record (KitParent) it's Kit Parent.
        ///           For the Kit Parent...
        ///               1. Update the PartAlloc
        ///               2. Create PickOrder
        ///
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_CMP_SHP(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            Erp.Tablesets.IssueReturnRow KitParent = null;
            decimal qtyRemovedFromPickedOrders = decimal.Zero;

            outMessage = string.Empty;

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part == null)
            {
                throw new BLException(Strings.AValidPartNumberIsRequired, "IssueReturn", "PartNum");
            }
            /* STEP 1 - PROCESS THE COMPONENT AS A STK-SHP */
            this.process_STK_SHP(ttIssueReturn, out _);
            /* STEP 2. CREATE THE KtlParent (ttIssueReturn) RECORD FOR THE KIT COMPONENTS */
            /* Note: The MtlQueue.QueuePickSeq contains Kit Parent Order line */
            //STEP2:
            do
            {
                MtlQueue = this.FindFirstMtlQueueWithUpdLock(ttIssueReturn.MtlQueueRowId);
                if (MtlQueue == null)
                {
                    break;
                }
                int ParentLineNo = 0;
                ParentLineNo = MtlQueue.QueuePickSeq;

                MtlQueue = this.FindFirstMtlQueue(ttIssueReturn.Company, 0, "M", 0, "KIT-SHP", 0, 0, ttIssueReturn.OrderNum, ParentLineNo, ttIssueReturn.OrderRel);
                if (MtlQueue == null)
                {
                    break;
                }
                KitParent = new Erp.Tablesets.IssueReturnRow();
                CurrentFullTableset.IssueReturn.Add(KitParent);
                BufferCopy.Copy(ttIssueReturn, ref KitParent);
                this.processMtlQueue(MtlQueue.SysRowID, KitParent);/* CREATES A TTIssueReturn record in special KitParent buffer */
                /* assign the "To Warehouse/Bin" that was used on the Kit Component transaction */
                KitParent.ToWarehouseCode = ttIssueReturn.ToWarehouseCode;
                KitParent.ToBinNum = ttIssueReturn.ToBinNum;

                /* UPDATE ANY EXISTING PICKEDORDER FOR THE KIT PARENT TO THE LATEST WAREHOUSE/BIN ENTERED */
                PickedOrders = this.FindFirstPickedOrdersWithUpdLock(KitParent.Company, Session.PlantID, KitParent.OrderNum, KitParent.OrderLine, KitParent.OrderRel);
                if (PickedOrders != null)
                {
                    if ((StringExtensions.Compare(PickedOrders.WarehouseCode, KitParent.ToWarehouseCode) != 0 || StringExtensions.Compare(PickedOrders.BinNum, KitParent.ToBinNum) != 0))
                    {
                        PickedOrders.WarehouseCode = KitParent.ToWarehouseCode;
                        PickedOrders.BinNum = KitParent.ToBinNum;
                        Db.Release(ref PickedOrders);
                    }
                }
                /* CALCUALATE THE COMPONENT QUANTITY  - THIS IS ONLY DONE WHEN TRANSACTION IS FOR THE 1ST COMPONENT */
                KitParent.TranQty = 0;            /* DETERMINE THE FIRST COMPONENT OF THE ORDER LINE */

                OrderDtl = this.FindFirstOrderDtl(KitParent.Company, KitParent.OrderNum, KitParent.OrderLine, "C");
                if (OrderDtl != null)
                {
                    if (ttIssueReturn.OrderLine == OrderDtl.OrderLine && OrderDtl.KitQtyPer > 0)
                    {
                        KitParent.TranQty = ttIssueReturn.TranQty / OrderDtl.KitQtyPer;
                    }
                }
                this.LibAllocations.updatePickedOrders(KitParent.DimConvFactor, KitParent.FromJobNum, KitParent.LotNum, KitParent.AttributeSetID, KitParent.ToPCID, KitParent.OrderNum, KitParent.OrderLine, KitParent.OrderRel, KitParent.PartNum, KitParent.ToWarehouseCode, KitParent.ToBinNum, KitParent.TranQty, KitParent.UM, out qtyRemovedFromPickedOrders);            /* STEP 3a - GO UPDATE THE MTLQUEUE */
                this.LibAllocations.updateMtlQueue(KitParent.UM, KitParent.FromWarehouseCode, KitParent.FromBinNum, KitParent.LotNum, KitParent.OrderNum, KitParent.OrderLine, KitParent.OrderRel, KitParent.PartNum, KitParent.FromJobNum, KitParent.ToWarehouseCode, KitParent.ToBinNum, KitParent.TranQty, KitParent.MtlQueueRowId);           /* DELETE THE KitParent(ttIssueReturn) DON'T WANT IT SENT BACK TO CLIENT....
              IT WAS CREATED AS A TEMPORARY RECORD TO BE USED TO PROCESS THE KIT PARENT AUTOMATICALLY  */
                CurrentFullTableset.IssueReturn.Remove(KitParent);
            }
            while (false);
        }

        ///<summary>
        ///  Purpose:  DMR-ASM - move from DMR location to job location.
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_DMR_ASM(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_WIP_WIP(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose:  DMR-MTL - move from DMR location to job location/jobmtl.
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_DMR_MTL(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_WIP_WIP(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement from DMR to stock warehouse (DMR-STK).
        ///           This is bascially a transfer transaction.
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: NOTE: This rountine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_DMR_STK(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_PUR_STK(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement of job subcontract receipts from DMR to
        ///           the next operations input warehouse/bin (DMR-SUB).
        ///           1. Update the PartWip records to reflect the new location of the part.
        ///
        ///</summary>
        private void process_DMR_SUB(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_WIP_WIP(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the physical movement from inspection to WIP location.
        ///  Parameters:  none
        ///  Notes:
        ///  This same routine also handles INS-MTL transactions.
        ///  The job was already updated, and a PartWip created when the item was passed in the
        ///  inspection process. All we have to do here is update the PartWip current Warehouse/Bin information.
        ///
        ///           1. Set the "FROM" job equal to "TO" since this is only a movement of whse/bin location and
        ///              the PartWip created from the Inspection Passed process created the PartWip at the "Next"
        ///              operation
        ///           2. Update the PartWip records to reflect the new location of the part.
        ///
        ///</summary>
        private void process_INS_ASM(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_WIP_WIP(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose:     Movement from inspecton to DMR.
        ///  Parameters:  none
        ///  Notes:       For now, for this transaction no actions need to be done. Created the procedure for consistency.
        ///</summary>
        private void process_INS_DMR(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            if (MtlQueue != null)
            {
                NonConf = this.FindFirstNonConf(MtlQueue.Company, MtlQueue.NCTranID);
                if (NonConf != null)
                {
                    DMRHead = this.FindFirstDMRHeadWithUpdLock(NonConf.Company, NonConf.DMRNum);
                    if (DMRHead != null)
                    {
                        DMRHead.WarehouseCode = ttIssueReturn.ToWarehouseCode;
                        DMRHead.BinNum = ttIssueReturn.ToBinNum;
                    }
                }
            }
        }

        ///<summary>
        ///  Purpose: To process the movement of material in inspection to WIP.
        ///  Parameters:  none
        ///  Notes: This process uses the Process-INS-ASM procedure to perform the task.
        ///</summary>
        private void process_INS_MTL(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_INS_ASM(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Movement of cross docked purchased stock for an order from the receiving area to warehouse
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: This rountine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_INS_SHP(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_STK_SHP(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement from inspection to stock warehouse (INS-STK).
        ///           This is bascially a transfer transaction.
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: NOTE: This rountine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_INS_STK(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_PUR_STK(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement of job subcontract receipts from the inspection area to
        ///           the next operations input warehouse/bin (INS-SUB).
        ///           1. Update the PartWip records to reflect the new location of the part.
        ///
        ///</summary>
        private void process_INS_SUB(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_WIP_WIP(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement of Pick Stock of KIT PARENT for Shipment  (KIT-SHP).
        ///           For the KIT parent
        ///               1. Update the PartAlloc
        ///               2.  Create PickOrder
        ///           For the Kit Components...
        ///               1. Creates ttIssueReturn records from the related MtlQueue records.
        ///               2. Process these records using the STK-STK process.
        ///
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_KIT_SHP(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            Erp.Tablesets.IssueReturnRow KitDtl = null;
            decimal qtyRemovedFromPickedOrders = decimal.Zero;

            outMessage = string.Empty;

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part == null)
            {
                throw new BLException(Strings.AValidPartNumberIsRequired, "IssueReturn", "PartNum");
            }

            /* STEP 2. UPDATE PICKEDORDER TABLE */
            this.LibAllocations.updatePickedOrders(ttIssueReturn.DimConvFactor, ttIssueReturn.FromJobNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, ttIssueReturn.ToPCID, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel, ttIssueReturn.PartNum, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, ttIssueReturn.TranQty, ttIssueReturn.UM, out qtyRemovedFromPickedOrders);

            /* STEP 3. CREATE THE KtlDtl (ttIssueReturn) RECORDS FOR THE KIT COMPONENTS */
            /* Note: The MtlQueue.QueuePickSeq contains Kit Parent Order line */
            foreach (var MtlQueue_iterator in (this.SelectMtlQueue(ttIssueReturn.Company, "KIT", ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel)))
            {
                MtlQueue = MtlQueue_iterator;
                KitDtl = new Erp.Tablesets.IssueReturnRow();
                CurrentFullTableset.IssueReturn.Add(KitDtl);
                BufferCopy.Copy(ttIssueReturn, ref KitDtl);
                this.processMtlQueue(MtlQueue.SysRowID, KitDtl);/* CREATES A TTIssueReturn record in special KitDtl buffer */
                /* assign the "To Warehouse/Bin" that was used on the Kit Parent transaction */
                KitDtl.ToWarehouseCode = ttIssueReturn.ToWarehouseCode;
                KitDtl.ToBinNum = ttIssueReturn.ToBinNum;

                /* CALCULATE THE COMPONENT QUANTITY BASED ON KIT QUANTIY AND QTY PER */
                OrderDtl = this.FindFirstOrderDtl(KitDtl.Company, KitDtl.OrderNum, KitDtl.OrderLine);
                if (OrderDtl != null)
                {
                    KitDtl.TranQty = ttIssueReturn.TranQty * OrderDtl.KitQtyPer;
                }
                else
                {
                    KitDtl.TranQty = ttIssueReturn.TranQty;
                }

                this.process_STK_SHP(KitDtl, out _);/* PROCESS THE KitDtl AS A STK-SHP TRANSACTION */
                /* STEP 3a - GO UPDATE THE MTLQUEUE */
                this.LibAllocations.updateMtlQueue(KitDtl.UM, KitDtl.FromWarehouseCode, KitDtl.FromBinNum, KitDtl.LotNum, KitDtl.OrderNum, KitDtl.OrderLine, KitDtl.OrderRel, KitDtl.PartNum, KitDtl.FromJobNum, KitDtl.ToWarehouseCode, KitDtl.ToBinNum, KitDtl.TranQty, KitDtl.MtlQueueRowId);
                /* DELETE THE KITDTL (ttIssueReturn) DON'T WANT IT SENT BACK TO CLIENT....
                   IT WAS CREATED AS A TEMPORARY RECORD TO BE USED TO PROCESS THE KIT COMPONENTS AUTOMATICALLY  */
                CurrentFullTableset.IssueReturn.Remove(KitDtl);
            }
        }

        ///<summary>
        ///  Purpose: Perform the movement of completed items on the final operation to the shipping area (MFG-CUS).
        ///           1. Update the PartWip records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes:  Screen shows the from Asm/opr seq# of the last operation that created the qty for shipment.
        ///   This makes logical sense, visually. However, the quantity in PartWip is actually allocated to the next asm/opr.
        ///   Which for a MFG-CUS would be 0/0.  Therefore they are set to zero here prior to updating PartWip.
        ///</summary>
        private void process_MFG_CUS(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
            {
                libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, true, ttIssueReturn.ToPCID, true, "RCPTTOINVENTORY");
            }

            ttIssueReturn.FromAssemblySeq = 0;
            ttIssueReturn.FromJobSeq = 0;
            this.updatePartWIP("From", ttIssueReturn);
            /* UPDATE PARTWIP TO REFLECT THE "TO" MOVEMENT */
            ttIssueReturn.ToAssemblySeq = 0;
            ttIssueReturn.ToJobSeq = 0;
            this.updatePartWIP("TO", ttIssueReturn);
        }

        ///<summary>
        ///  Purpose: Perform the movement of completed quantity from one operation to the next.
        ///           1. Update the PartWip records to reflect the new location of the part.
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_MFG_OPR(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_WIP_WIP(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Parameters:  none
        ///  Purpose: Perform the movement of Pick from Manufacturing for Shipment  (MFG-SHP).
        ///           This is bascially a transfer transaction with a bit more.
        ///           1. Update the PartWip records to reflect the new location of the part.
        ///           2. Update the PartAlloc to reflect the change in PickingQty, PickedQty
        ///           3. Update the PickedOrders table.
        ///</summary>
        private void process_MFG_SHP(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
            {
                libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, true, ttIssueReturn.ToPCID, true, "RCPTTOINVENTORY");
            }

            // Update PartWip to reflect the "From" movement
            ttIssueReturn.FromAssemblySeq = 0;
            ttIssueReturn.FromJobSeq = 0;
            this.updatePartWIP("From", ttIssueReturn);

            // Update PartWip to reflect the "To" movement
            ttIssueReturn.ToAssemblySeq = 0;
            ttIssueReturn.ToJobSeq = 0;
            this.updatePartWIP("TO", ttIssueReturn);

            // Update Partalloc to reflect the "From" movement
            UpdatePartAllocWIP("MFG-SHP", ttIssueReturn);

            // Update PartAlloc to reflect the "To" movement
            LibAllocations.UpdatePartAllocPickedQty(ttIssueReturn.PartNum, "", "", ttIssueReturn.FromJobNum, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel, "", ttIssueReturn.UM, "", 0, false, ttIssueReturn.TranQty);

            // Update PickedOrders table
            LibAllocations.updatePickedOrders(ttIssueReturn.DimConvFactor, ttIssueReturn.FromJobNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, ttIssueReturn.ToPCID, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel, ttIssueReturn.PartNum, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, ttIssueReturn.TranQty, ttIssueReturn.UM, out _);
        }

        /// <summary>
        /// Purpose: Call UpdatePartAlloc for WIP transactions
        ///          UpdatePartAlloc works off of a PartTran record and is normally called by the PartTran/Write trigger.
        ///          For MFG-SHP and WIP-WIP "Unpick" transactions, a PartTran record does not get created.
        ///          Instead, we create a fake PartTran here and call UpdatePartAlloc directly passing it the tempPartTran.
        /// </summary>
        private void UpdatePartAllocWIP(string tranType, IssueReturnRow ttIssueReturn)
        {
            PartTran tempPartTran = new()
            {
                Company = Session.CompanyID,
                OrderNum = ttIssueReturn.OrderNum,
                OrderLine = ttIssueReturn.OrderLine,
                OrderRelNum = ttIssueReturn.OrderRel,
                PartNum = ttIssueReturn.PartNum,
                TranQty = -ttIssueReturn.TranQty,
                ActTranQty = -ttIssueReturn.TranQty,
                UM = ttIssueReturn.UM,
                InvtyUOM = ttIssueReturn.UM,
                TranType = tranType,
                TranReference = ttIssueReturn.ProcessID,
                JobNum = ttIssueReturn.FromJobNum,
                WareHouseCode = "", // Supply Job PartAlloc Reservations do not have a WarehouseCode
                BinNum = "", // Supply Job PartAlloc Reservations do not have a BinNum. 
                LotNum = ttIssueReturn.LotNum,
                //TODO: DJY - not sure if FromPCID should be populated in this case
                PCID = ttIssueReturn.FromPCID,
                AttributeSetID = ttIssueReturn.AttributeSetID
            };

            LibAppService.UpdatePartAlloc(tempPartTran);
        }

        ///<summary>
        ///  Purpose: Perform the movement of NON-Conformance to inspection
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_MTL_INS(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            using (Db.DisableTriggerScope(nameof(Erp.Tables.NonConf), TriggerType.Write))
            {
                MtlQueue = this.FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
                if (MtlQueue == null)
                {
                    return;
                }

                NonConf = this.FindFirstNonConfWithUpdLock(MtlQueue.Company, MtlQueue.NCTranID);
                if (NonConf != null)
                {
                    NonConf.ToWarehouseCode = ttIssueReturn.ToWarehouseCode;
                    NonConf.ToBinNum = ttIssueReturn.ToBinNum;
                    Db.Validate(NonConf);
                }
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_MTL_MTL(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_WIP_WIP(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the update to the database for a MTL-STK (return JOB MATERIAL TO STOCK) TRANSACTION
        ///  Parameters:  none
        ///  Notes: This ends up as a "negative issue" parttran (STK-MTL).
        ///</summary>
        private void process_MTL_STK(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            /* scr3974 - Bab -
             * These variables are used to hold the individual component amounts to be
             * applied to the JobMtl record.  each of these may have a rounding error associated with
             * it which makes the sum of the parts NOT equal to the PartTran extended cost (PartTran.ExtCost).
             * To correctly post these to the JobMtl record an adjustment (adjRoundingError) will be computed
             * which is the difference between the sum of the parts and PartTran.ExtCost.
             * The adjRoundingError will be applied to the MaterialMtlCost.
             * SCR #59049 - Modified the rounding logic to apply the rounding difference to the highest
             * component costs instead of always adding it to MaterialMtlCost. This is to avoid negative
             * cost in material cost in case the rounding amount is negative and no current material cost.
            */
            decimal amtMaterialMtlCost = decimal.Zero;
            decimal amtMaterialLabCost = decimal.Zero;
            decimal amtMaterialBurCost = decimal.Zero;
            decimal amtMaterialSubCost = decimal.Zero;
            decimal amtMtlBurCost = decimal.Zero;
            decimal adjRoundingError = decimal.Zero;
            decimal absMaxCost = decimal.Zero;
            decimal Conv_Qty = decimal.Zero;
            string cLegalNumber = string.Empty;

            outMessage = string.Empty;

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part == null)
            {
                throw new BLException(Strings.AValidPartNumberIsRequired, "IssueReturn", "PartNum", ttIssueReturn.SysRowID);
            }
            okWhse = false;

            JobHead = this.FindFirstJobHead(Session.CompanyID, ttIssueReturn.FromJobNum);
            if (!JobHead.Plant.Equals(Session.PlantID, StringComparison.OrdinalIgnoreCase))
            {
                this.checkWarehouseBin(ttIssueReturn.ToWarehouseCode, JobHead.Plant, out okWhse);
                if (!okWhse)
                {
                    throw new BLException(Strings.InvalidToWarehouse, "IssueReturn", "ToWarehouseCode");
                }
            }
            #region China CSF
            if (this.IsChinaLocalization())
            {
                using (var csfChinaLib = new Internal.CSF.China(this.Db))
                {
                    csfChinaLib.CheckPartTranBonded(ttIssueReturn.PartNum, ttIssueReturn.ToWarehouseCode, China.BondedType.ReturnMaterial, out outMessage);
                }
                ExceptionManager.AssertNoExceptions();
            }
            #endregion

            /* SCR 170491 - Clear the ttReturnMaterialCosts to get ready for FIFO/LOTFIFO return costing logic. */
            ttReturnMaterialCostsRows.Clear();

            this.createPartTran(ttIssueReturn);
            PartTran.TranClass = "I";
            PartTran.InventoryTrans = true;
            PartTran.TranType = "STK-MTL";  /* Flip from "MTL-STK" to issue tran type */
            PartTran.TranQty = -(PartTran.TranQty); /* Flip sign - makes it a negative issue */
            PartTran.ActTranQty = -(PartTran.ActTranQty); /* Flip sign - makes it a negative issue */
            PartTran.JobNum = ttIssueReturn.FromJobNum;
            PartTran.AssemblySeq = ttIssueReturn.FromAssemblySeq;
            PartTran.JobSeq = ttIssueReturn.FromJobSeq;
            PartTran.PartNum = ttIssueReturn.PartNum;
            PartTran.PartDescription = ttIssueReturn.PartPartDescription;
            PartTran.WareHouse2 = ttIssueReturn.FromWarehouseCode;
            PartTran.BinNum2 = ttIssueReturn.FromBinNum;
            //DJY - PartTran.PCID2 should not be populated - removing it from WIP will be handled by PartWip triggers
            PartTran.PCID2 = string.Empty;
            PartTran.WIPPCID2 = ttIssueReturn.FromPCID;
            PartTran.WareHouseCode = ttIssueReturn.ToWarehouseCode;
            PartTran.BinNum = ttIssueReturn.ToBinNum;
            PartTran.PCID = ttIssueReturn.ToPCID;
            partTranPK = partTranPK + ((partTranPK.Length > 0) ? "|" : "");
            partTranPK = partTranPK + Compatibility.Convert.ToString(PartTran.SysDate) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.SysTime) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.TranNum);

            WhseBin = this.FindFirstWhseBin(Session.CompanyID, PartTran.WareHouseCode, PartTran.BinNum);
            PartTran.BinType = ((WhseBin != null) ? WhseBin.BinType : "");

            if (!Part.TrackInventoryByRevision)
            {
                /*run getPartRev.*/
                PartRev = this.FindLastPartRev(Session.CompanyID, ttIssueReturn.PartNum, CompanyTime.Today());
                if (PartRev != null)
                {
                    PartTran.RevisionNum = PartRev.RevisionNum;
                }
                else
                {
                    PartTran.RevisionNum = "";
                }
            }

            PartTran.Plant = getPlantFromWarehouse(PartTran.WareHouseCode);

            /* Get Cost method for the PartNum */
            PartTran.CostMethod = this.getPartCostMethod(PartTran.Plant, PartTran.PartNum);

            /* CALCULATE COST AS THE JOB MATERIAL AVERAGE COST - NEGATIVE ISSUE */
            JobMtl = this.FindFirstJobMtlWithUpdLock(Session.CompanyID, PartTran.JobNum, PartTran.AssemblySeq, PartTran.JobSeq);
            if (JobMtl == null)
            {
                throw new BLException(Strings.JobsMaterInforIsNotOnFileCannotIssue, "IssueReturn", "AssemblySeq");
            }
            PartTran.ContractID = (JobMtl.LinkToContract) ? JobMtl.ContractID : "";
            /* SCR #51749 - Create FIFO layers for non-FIFO cost methods if enabled */
            /* get the plant cost id */
            var outCostID2 = PartTran.CostID;
            this.LibGetPlantCostID._getPlantCostID(PartTran.Plant, out outCostID2, ref Plant, ref XaSyst);
            PartTran.CostID = outCostID2;
            this.LibGetCostFIFOLayers.getPartCostFIFOLayers(PartTran.CostID, PartTran.PartNum, PartTran.Plant, out vEnableFIFOLayers);

            /* FYI: On negative issue transaction the MtlMtlUnitCost (breakdown) must be the same as the MtlUnitCost.
                This is because it's treated like a receipt, being processed by Mtlrcpt.p  */
            if (JobMtl.IssuedQty > 0)
            {
                /* SCR 170491 - Implementing new logic when returning material with FIFO/LOTFIFO cost method.  *
                 * Regular cost methods will continue to use the weighted average costs of all issued/returned *
                 * transactions as costs of the qty returned to stock. FIFO/LOTFIFO will not use average costs *
                 * but will return qty/costs from the original FIFO layers issued for this material. If return *
                 * quantity will use more than one FIFO layer then the PartTran record will hold weighted FIFO *
                 * Average Costs of the FIFO Layers to be returned.  Multiple PartFIFOTrans will be created to *
                 * show the details of the new FIFO Layers returned.                                           *
                 * NOTE: PartTran.xxxUnitCosts are now directly assigned in the getReturnMaterialCosts().      */
                this.getReturnMaterialCosts(ttIssueReturn, ref PartTran, ref ttReturnMaterialCostsRows, vEnableFIFOLayers);
            } /* JobMtl.IssuedQty > 0 */

            /* SCR 170491 - We will only process return of FIFO/LOTFIFO in MtlRcpt if we only have *
             * 1 FIFO layer to return to queue. If returning multiple FIFO layers then we have to  *
             * process outside of MtlRcpt logic. The Alternate Costs for secondary FIFO costs will *
             * be processed here if the vEnableFIFOLayer is true.                                  */
            if (StringExtensions.Lookup("F,O", PartTran.CostMethod) > -1)
            {
                if (ttReturnMaterialCostsRows.Count == 0)
                {
                    if (PartTran.FIFODate == null)
                    {
                        PartTran.FIFODate = PartTran.TranDate;
                        PartTran.FIFOSeq = 0;
                    }
                    PartTran.FIFOAction = "A";
                }
                else
                {
                    PartTran.FIFOAction = "";  /* MtlRcpt logic will skip the FIFO processing for this PartTran */
                }
            }
            else if (vEnableFIFOLayers == true)
            {
                if (PartTran.FIFODate == null)
                {
                    PartTran.FIFODate = PartTran.TranDate;
                    PartTran.FIFOSeq = 0;
                }
                PartTran.FIFOAction = "NFA";
            } /* lookup(PartTran.CostMethod,"F,O") */

            JobHead = this.FindFirstJobHead(Session.CompanyID, ttIssueReturn.FromJobNum);
            if (JobHead == null)
            {
                throw new BLException(Strings.FromJobNumberInforIsNotOnFileCannotIssue, "IssueReturn", "FromJobNum");
            }
            /* SCR #1669 - assign the service call number */
            PartTran.CallNum = JobHead.CallNum;
            PartTran.CallLine = JobHead.CallLine;
            PartTran.Plant2 = JobHead.Plant;

            /* ISSUE TO ANOTHER PLANT ?  if YES then CHANGE THE TRAN TYPE TO STK-PLT */
            if (StringExtensions.Compare(PartTran.Plant, PartTran.Plant2) != 0)
            {
                PartTran.TranType = "STK-PLT";
            }
            /* STK-PLT TRANSACTION - DO THE PLANT TO PLANT LOGIC */
            if (StringExtensions.Compare(PartTran.TranType, "STK-PLT") == 0)
            {
                this.IMIMPlant.CreatePlantTran(PartTran.TranType, JobHead.ProdCode, PartTran.TranQty, JobHead.CallNum, JobHead.CallLine, ref PartTran, ref PlantTran);
                /* FYI:  NO NEED TO assign G/L ACCOUNTS HERE for STK-PLT TRANSACTIONS.
                   THEY ARE assigned in the lib/SetInterPlant.i LOGIC FOUND IN im/implant.i */

                /* SCR #7145 - Automate the Receive From Plant procedure to make the return *
                 * to shared warehouse from job appear like it involves local warehouse.    */
                if (PlantTran != null)
                {
                    if (ttIssueReturn.TranDate == null)
                    {
                        PlantTran.RecTranDate = null;
                    }
                    else
                    {
                        PlantTran.RecTranDate = ttIssueReturn.TranDate;
                    }
                    /* Defined in im/implant.i */
                    this.IMIMPlant.GeneratePlantReceiptTran(PlantTran.SysRowID, "", ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, PlantTran.JobNum, PlantTran.AssemblySeq, PlantTran.JobMtl, cLegalNumber, ttIssueReturn.TranDocTypeID, PlantTran.PackNum, string.Empty, PartTran);
                } /* SCR #7145 - available PlantTran */
                /* SCR 170491 - If returning FIFO/LOTFIFO and ttReturnMaterialCosts has records then do not return. *
                 * We still need to process FIFO layers for return.                                                 */
                if ((StringExtensions.Lookup("F,O", PartTran.CostMethod) == -1) || ttReturnMaterialCostsRows.Count == 0)
                {
                    return;
                }
            }/* if PartTran.TranType = "STK-PLT":U */
            else  /* NOT A STK-PLT - CONTINUE PROCESSING THE MTL-STK TRANSACTION */
            {
                /* UPDATE JobMtl QUANTITY */
                this.LibAppService.UOMConv(PartTran.PartNum, PartTran.ActTranQty, PartTran.ActTransUOM, JobMtl.IUM, out Conv_Qty, false);
                JobMtl.IssuedQty = JobMtl.IssuedQty + Conv_Qty;
                JobMtl.IssuedComplete = ttIssueReturn.IssuedComplete;
                /* UPDATE JobMtl COSTS */
                Db.Validate(PartTran);
                UpdateLegalNumHistory(PartTran.LegalNumber, partTranPK);
                Db.ReadCurrent(ref PartTran);
                int ndec = 0;
                ndec = this.LibGetDecimalsNumber.getDecimalsNumberByName("JobMtl", "MaterialMtlCost", "");
                /* SCR 176758 - If the Extended Subcomponent Costs are available then use them instead of deriving costs from TranQty and UnitCosts */
                amtMaterialMtlCost = (PartTran.ExtMtlCost != 0) ? PartTran.ExtMtlCost : Math.Round((PartTran.TranQty * PartTran.MtlUnitCost), ndec, MidpointRounding.AwayFromZero);
                amtMaterialLabCost = (PartTran.ExtLbrCost != 0) ? PartTran.ExtLbrCost : Math.Round((PartTran.TranQty * PartTran.LbrUnitCost), ndec, MidpointRounding.AwayFromZero);
                amtMaterialBurCost = (PartTran.ExtBurCost != 0) ? PartTran.ExtBurCost : Math.Round((PartTran.TranQty * PartTran.BurUnitCost), ndec, MidpointRounding.AwayFromZero);
                amtMaterialSubCost = (PartTran.ExtSubCost != 0) ? PartTran.ExtSubCost : Math.Round((PartTran.TranQty * PartTran.SubUnitCost), ndec, MidpointRounding.AwayFromZero);
                amtMtlBurCost = (PartTran.ExtMtlBurCost != 0) ? PartTran.ExtMtlBurCost : Math.Round((PartTran.TranQty * PartTran.MtlBurUnitCost), ndec, MidpointRounding.AwayFromZero);

                /* ERPS-130913 - If the Material IssuedQty is now 0 then make sure that all costs associated with it are also 0s */
                if (JobMtl.IssuedQty == 0 && StringExtensions.Lookup("F,O", PartTran.CostMethod) == -1)
                {
                    /* Override the PartTran's unit costs with unit costs derived from the remaining costs of JobMtl */
                    int ndec2 = this.LibGetDecimalsNumber.getDecimalsNumberByName("PartTran", "MtlUnitCost", "");
                    PartTran.MtlUnitCost = Math.Round(-(JobMtl.MaterialMtlCost) / PartTran.TranQty, ndec2, MidpointRounding.AwayFromZero);
                    PartTran.LbrUnitCost = Math.Round(-(JobMtl.MaterialLabCost) / PartTran.TranQty, ndec2, MidpointRounding.AwayFromZero);
                    PartTran.BurUnitCost = Math.Round(-(JobMtl.MaterialBurCost) / PartTran.TranQty, ndec2, MidpointRounding.AwayFromZero);
                    PartTran.SubUnitCost = Math.Round(-(JobMtl.MaterialSubCost) / PartTran.TranQty, ndec2, MidpointRounding.AwayFromZero);
                    PartTran.MtlBurUnitCost = Math.Round(-(JobMtl.MtlBurCost) / PartTran.TranQty, ndec2, MidpointRounding.AwayFromZero);
                    /* Using the ExtxxxCost fields to store the actual remaining costs of JobMtl to avoid rounding variances */
                    PartTran.ExtMtlCost = -(JobMtl.MaterialMtlCost);
                    PartTran.ExtLbrCost = -(JobMtl.MaterialLabCost);
                    PartTran.ExtBurCost = -(JobMtl.MaterialBurCost);
                    PartTran.ExtSubCost = -(JobMtl.MaterialSubCost);
                    PartTran.ExtMtlBurCost = -(JobMtl.MtlBurCost);
                    PartTran.ExtMtlMtlCost = -(JobMtl.MaterialMtlCost);
                    PartTran.ExtMtlLabCost = 0;
                    PartTran.ExtMtlBurdenCost = 0;
                    PartTran.ExtMtlSubCost = 0;
                    Db.Validate(PartTran);

                    JobMtl.MaterialMtlCost = 0;
                    JobMtl.MaterialLabCost = 0;
                    JobMtl.MaterialBurCost = 0;
                    JobMtl.MaterialSubCost = 0;
                    JobMtl.MtlBurCost = 0;
                    JobMtl.TotalCost = 0;
                }
                else
                {
                    /* Compute the rounding error */
                    /* Total cost minus Sum of the component costs */
                    adjRoundingError = PartTran.ExtCost - (amtMaterialMtlCost + amtMaterialLabCost + amtMaterialBurCost + amtMaterialSubCost + amtMtlBurCost);
                    /* SCR #59049 - Adjust the highest component cost with the rounding difference instead of just   *
                        * adding it to the material cost. This is to avoid negative cost in case material cost is zero. *
                        * It's important to keep this order (Mtl/Lbr/Bur/Sub) to check where to add the rounding error. */
                    if (adjRoundingError != 0)
                    {
                        absMaxCost = Math.Max(Math.Max(Math.Max(Math.Abs(amtMaterialMtlCost), Math.Abs(amtMaterialLabCost)), Math.Abs(amtMaterialBurCost)), Math.Abs(amtMaterialSubCost));
                        if (Math.Abs(amtMaterialMtlCost) == absMaxCost)
                        {
                            amtMaterialMtlCost = amtMaterialMtlCost + adjRoundingError;
                        }
                        else if (Math.Abs(amtMaterialLabCost) == absMaxCost)
                        {
                            amtMaterialLabCost = amtMaterialLabCost + adjRoundingError;
                        }
                        else if (Math.Abs(amtMaterialBurCost) == absMaxCost)
                        {
                            amtMaterialBurCost = amtMaterialBurCost + adjRoundingError;
                        }
                        else
                        {
                            amtMaterialSubCost = amtMaterialSubCost + adjRoundingError;
                        }
                    }
                    JobMtl.MaterialMtlCost = Math.Max(0, (JobMtl.MaterialMtlCost + amtMaterialMtlCost));
                    JobMtl.MaterialLabCost = Math.Max(0, (JobMtl.MaterialLabCost + amtMaterialLabCost));
                    JobMtl.MaterialBurCost = Math.Max(0, (JobMtl.MaterialBurCost + amtMaterialBurCost));
                    JobMtl.MaterialSubCost = Math.Max(0, (JobMtl.MaterialSubCost + amtMaterialSubCost));
                    JobMtl.MtlBurCost = Math.Max(0, (JobMtl.MtlBurCost + amtMtlBurCost));
                    JobMtl.TotalCost = JobMtl.MaterialMtlCost + JobMtl.MaterialLabCost + JobMtl.MaterialBurCost + JobMtl.MaterialSubCost;
                }
            }  /* NOT A STK-PLT - CONTINUE PROCESSING THE MTL-STK TRANSACTION */

            /* ERPS-130913 - Moved this call of MtlRcpt here instead of calling it before the possible reset *
             * of JobMtl and adjustment of PartTran UnitCosts. This is to make sure that we use the correct  *
             * costs when we call the receipt logic. We ONLY need to call MtlRcpt logic if doing STK-MTL.    */
            /* UPDATE PART COSTS and PARTBIN USING MTLRCPT.P */
            if (PartTran.TranType.KeyEquals("STK-MTL"))
            {
                LibMtlRcpt._MtlRcpt(PartTran.LotNum, PartTran.SysRowID);
                ExceptionManager.AssertNoBLExceptions();
            }
            /* SCR 170491 - Process FIFO layers that are skipped by the MtlRcpt logic. */
            /* NOTE: Both STK-MTL and STK-PLT will do the FIFO processing logic. */
            if ((StringExtensions.Lookup("F,O", PartTran.CostMethod) > -1) && (ttReturnMaterialCostsRows.Count > 0))
            {
                LibProcessFIFO.ProcessReturnedFIFO(ref ttReturnMaterialCostsRows, ref PartTran, "STK-MTL");
            }
            /* Only STK-MTL transaction will call the updatePartWIP since STK-PLT will handle the Job update and PartWIP *
             * when the PLT-MTL is processed in IMPlant.GeneratePlantReceiptTran.                                        */
            if (PartTran.TranType.KeyEquals("STK-MTL"))
            {
                using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
                {
                    libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, true, ttIssueReturn.ToPCID, true, "RETURNMATERIAL");
                }

                this.updatePartWIP("From", ttIssueReturn, false);
            }
        }

        ///<summary>
        ///  Purpose: Perform the movement of job material plant receipts (PLT-MTL) from the receiving area to warehouse.
        ///           1. Update the PartWip records to reflect the new location of the wip part.
        ///
        ///  Parameters:  none
        ///  Notes: Only the "TO" Job fields have data. The "From" Job fields are blank.
        ///    We need to flip flop them between "FROM/TO" in order to use the common updatePartWIP procedure.
        ///</summary>
        private void process_PLT_MTL(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
            {
                libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, true, ttIssueReturn.ToPCID, true, "RCPTTOJOBENTRY");
            }

            ttIssueReturn.FromJobNum = ttIssueReturn.ToJobNum;
            ttIssueReturn.FromAssemblySeq = ttIssueReturn.ToAssemblySeq;
            ttIssueReturn.FromJobSeq = ttIssueReturn.ToJobSeq;
            ttIssueReturn.ToJobNum = "";
            ttIssueReturn.ToAssemblySeq = 0;
            ttIssueReturn.ToJobSeq = 0;

            this.updatePartWIP("From", ttIssueReturn);
            /* UPDATE PARTWIP TO REFLECT THE "TO" MOVEMENT */
            /* Flip/Flop From/to */
            ttIssueReturn.ToJobNum = ttIssueReturn.FromJobNum;
            ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
            ttIssueReturn.ToJobSeq = ttIssueReturn.FromJobSeq;
            ttIssueReturn.FromJobNum = "";
            ttIssueReturn.FromAssemblySeq = 0;
            ttIssueReturn.FromJobSeq = 0;

            this.updatePartWIP("TO", ttIssueReturn);
        }

        ///<summary>
        ///  Purpose: Perform the movement of transfer orders from receiving area to stock warehouse (PLT-STK).
        ///           This is bascially a transfer transaction.
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: NOTE: This rountine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_PLT_STK(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_PUR_STK(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement of a receipt from CMI receiving area to CMI bin (PUR-CMI).
        ///           This is bascially a transfer transaction.
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: NOTE: This rountine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_PUR_CMI(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_PUR_STK(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement of purchased receipts pending inspection from the receiving area
        ///   to the inspection warehouse (PUR-INS).
        ///           1. This function simply updates the Warehouse/Bin location on the RcvDtl.
        ///
        ///  Parameters:  none
        ///  Notes: A receipt pending inspection can only exist in one warehouse/bin. Currently this is a design
        ///  limitation.
        ///</summary>
        private void process_PUR_INS(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            using (Db.DisableTriggerScope(nameof(Erp.Tables.RcvDtl), TriggerType.Write))
            {
                MtlQueue = this.FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
                if (MtlQueue == null)
                {
                    return;
                }

                RcvDtl = this.FindFirstRcvDtlWithUpdLock(MtlQueue.Company, MtlQueue.VendorNum, MtlQueue.PurPoint, MtlQueue.PackSlip, MtlQueue.PackLine);
                if (RcvDtl != null)
                {
                    RcvDtl.WareHouseCode = ttIssueReturn.ToWarehouseCode;
                    RcvDtl.BinNum = ttIssueReturn.ToBinNum;
                    Db.Validate(RcvDtl);
                }
            }
        }

        ///<summary>
        ///  Purpose: Perform the movement of direct job material purchases from the receiving area to warehouse (PUR-MTL).
        ///           1. Update the PartWip records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: Only the "TO" Job fields have data. The "From" Job fields are blank.
        ///    We need to flip flop them between "FROM/TO" in order to use the common updatePartWIP procedure.
        ///</summary>
        private void process_PUR_MTL(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
            {
                libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, true, ttIssueReturn.ToPCID, true, "RCPTTOJOBENTRY");
            }

            ttIssueReturn.FromJobNum = ttIssueReturn.ToJobNum;
            ttIssueReturn.FromAssemblySeq = ttIssueReturn.ToAssemblySeq;
            ttIssueReturn.FromJobSeq = ttIssueReturn.ToJobSeq;
            ttIssueReturn.ToJobNum = "";
            ttIssueReturn.ToAssemblySeq = 0;
            ttIssueReturn.ToJobSeq = 0;
            this.updatePartWIP("From", ttIssueReturn);
            /* UPDATE PARTWIP TO REFLECT THE "TO" MOVEMENT */
            /* Flip/Flop From/to */
            ttIssueReturn.ToJobNum = ttIssueReturn.FromJobNum;
            ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
            ttIssueReturn.ToJobSeq = ttIssueReturn.FromJobSeq;
            ttIssueReturn.FromJobNum = "";
            ttIssueReturn.FromAssemblySeq = 0;
            ttIssueReturn.FromJobSeq = 0;

            this.updatePartWIP("TO", ttIssueReturn);
        }

        ///<summary>
        ///  Purpose: Movement of cross docked purchased stock for an order from the receiving area to warehouse
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: This rountine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_PUR_SHP(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_STK_SHP(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement of a receipt from SMI receiving area to SMI bin (PUR-SMI).
        ///           This is bascially a transfer transaction.
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: NOTE: This rountine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_PUR_SMI(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_PUR_STK(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement of purchased stock from the receiving area to warehouse (PUR-STK).
        ///           This is bascially a transfer transaction.
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes:  THIS SAME ROUNTINE IS USED TO HandLE "INS-STK" and "PLT-STK" TRANSACTION.
        ///</summary>
        private void process_PUR_STK(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            Part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part == null)
                throw new BLException(Strings.AValidPartNumberIsRequired, "IssueReturn", "PartNum");

            /* Check for shared warehouses */
            string vFromPlant = getPlantFromWarehouse(ttIssueReturn.FromWarehouseCode);
            string vToPlant = getPlantFromWarehouse(ttIssueReturn.ToWarehouseCode);

            // SCR 127621 - Even if the move is being initiated from another plant, it is still a STK-STK if both warehouses are in the same plant, not STK-PLT.
            string vTranType = (vFromPlant.KeyEquals(vToPlant) ? "STK-STK" : "STK-PLT");

            WhseBin fromWhseBin = FindFirstWhseBin(ttIssueReturn.Company, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum);
            WhseBin toWhseBin = FindFirstWhseBin(ttIssueReturn.Company, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum);

            if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-STK") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "RAU-STK") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "RMN-STK") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "RMG-STK") == 0)
            {
                if (fromWhseBin != null && toWhseBin != null)
                {
                    if (fromWhseBin.BinType.Equals("Supp", StringComparison.OrdinalIgnoreCase) && (toWhseBin.BinType.Equals("Std", StringComparison.OrdinalIgnoreCase) || toWhseBin.BinType.Equals("Cont", StringComparison.OrdinalIgnoreCase)) && ttIssueReturn.TranQty > 0)
                    {
                        LibGenSMIReceipt.GenerateSMIReceipt(ttIssueReturn.TranQty, ttIssueReturn.UM, fromWhseBin.VendorNum, ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, "STK-STK", ttIssueReturn.Company, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, null);
                    }
                }
            }

            var hasPartAllocOtherWhse = this.ExistPartAllocPickingQty(ttIssueReturn.Company, ttIssueReturn.TFOrdNum, ttIssueReturn.TFOrdLine, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.LotNum, ttIssueReturn.FromPCID, 0);

            /* CREATE PARTTRAN TO REFLECT THE "FROM" MOVEMENT */
            this.createPartTran(ttIssueReturn);
            PartTran.TranClass = "A";
            PartTran.InventoryTrans = true;
            PartTran.TranType = vTranType;
            PartTran.TranQty = -(PartTran.TranQty); /* Flip sign - makes it a negative */
            PartTran.ActTranQty = -(PartTran.ActTranQty); /* Flip sign - makes it a negative issue */
            PartTran.WareHouseCode = ttIssueReturn.FromWarehouseCode;
            PartTran.BinNum = ttIssueReturn.FromBinNum;
            PartTran.PCID = ttIssueReturn.FromPCID;
            PartTran.WareHouse2 = ttIssueReturn.ToWarehouseCode;
            PartTran.BinNum2 = ttIssueReturn.ToBinNum;
            PartTran.PCID2 = ttIssueReturn.ToPCID;

            if (vTranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))
            {
                PartTran.TranQty = -(PartTran.TranQty);     /* flip signs back for STK-PLT */
                PartTran.ActTranQty = -(PartTran.ActTranQty);
            }
            partTranPK = partTranPK + ((partTranPK.Length > 0) ? "|" : "");
            partTranPK = partTranPK + Compatibility.Convert.ToString(PartTran.SysDate) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.SysTime) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.TranNum);
            PartTran.BinType = ((fromWhseBin != null) ? fromWhseBin.BinType : "");

            if (!Part.TrackInventoryByRevision)
            {
                /*run getPartRev.*/
                PartRev = FindLastPartRev(Session.CompanyID, ttIssueReturn.PartNum, CompanyTime.Today());
                if (PartRev != null)
                    PartTran.RevisionNum = PartRev.RevisionNum;
                else
                    PartTran.RevisionNum = "";
            }

            PartTran.Plant = vFromPlant;

            if (vTranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))
                PartTran.Plant2 = vToPlant;

            /* Get Cost method for the PartNum */
            PartTran.CostMethod = getPartCostMethod(PartTran.Plant, PartTran.PartNum);

            /* SCR #51749 - Create FIFO layers for non-FIFO cost methods if enabled */
            var outCostID3 = PartTran.CostID;
            LibGetPlantCostID._getPlantCostID(PartTran.Plant, out outCostID3, ref Plant, ref XaSyst);
            PartTran.CostID = outCostID3;
            LibGetCostFIFOLayers.getPartCostFIFOLayers(PartTran.CostID, PartTran.PartNum, PartTran.Plant, out vEnableFIFOLayers);

            /* SCR #80528 - Although the transaction involved is STK-STK, we still have to check *
             * if the "To" Warehouse/Plant belongs to a different CostID. If From and To CostIDs *
             * are not the same then we have to consume the FIFO cost/qty from the "From" CostID *
             * and recreate the new FIFO cost/qty in the "To" CostID.                            */

            string vToCostID = string.Empty;
            LibGetPlantCostID._getPlantCostID(PartTran.Plant, out vToCostID, ref Plant, ref XaSyst);

            bool vCreateFIFO = false;    /* if different GL divisions then will have to make a GL transaction */
            /* PartTran.GLDiv <> PartTran.GL2Div */
            if (StringExtensions.Compare(ttIssueReturn.TranType, "PUR-SMI") != 0 && StringExtensions.Compare(ttIssueReturn.TranType, "PUR-CMI") != 0
            && (
                (fromWhseBin.BinType.Equals("Supp", StringComparison.OrdinalIgnoreCase) && toWhseBin.BinType.Equals("Std", StringComparison.OrdinalIgnoreCase)) ||
                (fromWhseBin.BinType.Equals("Std", StringComparison.OrdinalIgnoreCase) && toWhseBin.BinType.Equals("Std", StringComparison.OrdinalIgnoreCase)) ||
                (fromWhseBin.BinType.Equals("Supp", StringComparison.OrdinalIgnoreCase) && toWhseBin.BinType.Equals("Cont", StringComparison.OrdinalIgnoreCase)) ||
                (fromWhseBin.BinType.Equals("Std", StringComparison.OrdinalIgnoreCase) && toWhseBin.BinType.Equals("Cont", StringComparison.OrdinalIgnoreCase))))
            {

                var outMtlUnitCost6 = PartTran.MtlUnitCost;
                var outLbrUnitCost6 = PartTran.LbrUnitCost;
                var outBurUnitCost6 = PartTran.BurUnitCost;
                var outSubUnitCost6 = PartTran.SubUnitCost;
                var outMtlBurUnitCost6 = PartTran.MtlBurUnitCost;
                this.LibInvCosts.getInvCost(ttIssueReturn.LotNum, ((!String.IsNullOrEmpty(PartTran.Plant)) ? PartTran.Plant : Session.PlantID), PartTran.PartNum, PartTran.CostMethod, out outMtlUnitCost6, out outLbrUnitCost6, out outBurUnitCost6, out outSubUnitCost6, out outMtlBurUnitCost6, ref Part);
                PartTran.MtlUnitCost = outMtlUnitCost6;
                PartTran.LbrUnitCost = outLbrUnitCost6;
                PartTran.BurUnitCost = outBurUnitCost6;
                PartTran.SubUnitCost = outSubUnitCost6;
                PartTran.MtlBurUnitCost = outMtlBurUnitCost6;
                PartTran.MtlMtlUnitCost = PartTran.MtlUnitCost;
                PartTran.GLTrans = true;
                /* SCR #80528 - When transferring Stocks between two different CostIDs, *
                 * consume the FIFO qty from the first plant costid.                    */
                /* lookup(PartTran.CostMethod,"F,O") */
                /* SCR #51749 - Apply same FIFO logic for non-FIFO parts if FIFO Layers enabled. */
                if (StringExtensions.Lookup("F,O", PartTran.CostMethod) > -1)
                {
                    if (StringExtensions.Compare(PartTran.CostID, vToCostID) != 0)
                    {
                        PartTran.FIFOAction = "D"; /* Decrease */
                        var outFIFODate7 = PartTran.FIFODate;
                        var outFIFOSeq9 = PartTran.FIFOSeq;
                        var outFIFOSubSeq3 = PartTran.FIFOSubSeq;
                        var outMtlUnitCost7 = PartTran.MtlUnitCost;
                        var outLbrUnitCost7 = PartTran.LbrUnitCost;
                        var outBurUnitCost7 = PartTran.BurUnitCost;
                        var outSubUnitCost7 = PartTran.SubUnitCost;
                        var outMtlBurUnitCost7 = PartTran.MtlBurUnitCost;
                        /* SCR 176758 - Call ConsumePartFIFOCost with vUpdPartTran option to update the PartTran's Extended Subcomponent Costs */
                        LibProcessFIFO.ConsumePartFIFOCost(true, out outFIFODate7, out outFIFOSeq9, out outFIFOSubSeq3, out outMtlUnitCost7, out outLbrUnitCost7, out outBurUnitCost7, out outSubUnitCost7, out outMtlBurUnitCost7, ref PartTran, true);
                        PartTran.FIFODate = (DateTime)outFIFODate7;
                        PartTran.FIFOSeq = outFIFOSeq9;
                        PartTran.FIFOSubSeq = outFIFOSubSeq3;
                        PartTran.MtlUnitCost = outMtlUnitCost7;
                        PartTran.LbrUnitCost = outLbrUnitCost7;
                        PartTran.BurUnitCost = outBurUnitCost7;
                        PartTran.SubUnitCost = outSubUnitCost7;
                        PartTran.MtlBurUnitCost = outMtlBurUnitCost7;
                        ExceptionManager.AssertNoBLExceptions();
                        PartTran.MtlMtlUnitCost = PartTran.MtlUnitCost;
                        PartTran.MtlLabUnitCost = 0;
                        PartTran.MtlSubUnitCost = 0;
                        PartTran.MtlBurdenUnitCost = 0;
                        /* PartTran.ExtMtlXXXCosts are not updated in ConsumePartFIFOCost, update extended material subcomponent costs here */
                        PartTran.ExtMtlMtlCost = PartTran.ExtMtlCost;
                        PartTran.ExtMtlLabCost = 0;
                        PartTran.ExtMtlSubCost = 0;
                        PartTran.ExtMtlBurdenCost = 0;
                        vCreateFIFO = true;
                    }
                }
                else if (vEnableFIFOLayers == true)
                {
                    if (StringExtensions.Compare(PartTran.CostID, vToCostID) != 0)
                    {
                        PartTran.FIFOAction = "NFD"; /* Non-FIFO Deactivate */
                        var outFIFODate8 = PartTran.FIFODate;
                        var outFIFOSeq10 = PartTran.FIFOSeq;
                        var outFIFOSubSeq4 = PartTran.FIFOSubSeq;
                        var outAltMtlUnitCost3 = PartTran.AltMtlUnitCost;
                        var outAltLbrUnitCost3 = PartTran.AltLbrUnitCost;
                        var outAltBurUnitCost3 = PartTran.AltBurUnitCost;
                        var outAltSubUnitCost3 = PartTran.AltSubUnitCost;
                        var outAltMtlBurUnitCost3 = PartTran.AltMtlBurUnitCost;
                        this.LibProcessFIFO.ConsumeNonFIFOCost(true, out outFIFODate8, out outFIFOSeq10, out outFIFOSubSeq4, out outAltMtlUnitCost3, out outAltLbrUnitCost3, out outAltBurUnitCost3, out outAltSubUnitCost3, out outAltMtlBurUnitCost3, ref PartTran);
                        PartTran.FIFODate = (DateTime)outFIFODate8;
                        PartTran.FIFOSeq = outFIFOSeq10;
                        PartTran.FIFOSubSeq = outFIFOSubSeq4;
                        PartTran.AltMtlUnitCost = outAltMtlUnitCost3;
                        PartTran.AltLbrUnitCost = outAltLbrUnitCost3;
                        PartTran.AltBurUnitCost = outAltBurUnitCost3;
                        PartTran.AltSubUnitCost = outAltSubUnitCost3;
                        PartTran.AltMtlBurUnitCost = outAltMtlBurUnitCost3;
                        ExceptionManager.AssertNoBLExceptions();
                        PartTran.AltMtlMtlUnitCost = PartTran.AltMtlUnitCost;
                        PartTran.AltMtlLabUnitCost = 0;
                        PartTran.AltMtlSubUnitCost = 0;
                        PartTran.AltMtlBurdenUnitCost = 0;
                        vCreateFIFO = true;
                    }
                }
            }
            else
            {
                PartTran.GLTrans = false;
            }

            ExceptionManager.AssertNoBLExceptions();

            /* NOTE: IN THIS CASE THERE IS NO COST ON THIS TRANSACTION
               UPDATE PARTBIN TO REFLECT THE "FROM" MOVEMENT
            */
            decimal Inventory_Qty = decimal.Zero;
            LibAppService.UOMConv(PartTran.PartNum, PartTran.ActTranQty, PartTran.ActTransUOM, PartTran.InvtyUOM, out Inventory_Qty, false);

            /* Create PlantTran for STK-PLT transactions */
            if (vTranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))
            {
                IMIMPlant.CreatePlantTran("I", Part.ProdCode, PartTran.TranQty, 0, 0, ref PartTran, ref PlantTran); /* CallNum/CallLine */
                if (PlantTran != null)
                {
                    PlantTran.RecTranDate = ttIssueReturn.TranDate;
                    IMIMPlant.GeneratePlantReceiptTran(PlantTran.SysRowID, "", ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, PlantTran.JobNum, PlantTran.AssemblySeq, PlantTran.JobMtl, PartTran.LegalNumber, PartTran.TranDocTypeID, PlantTran.PackNum, PartTran.PCID2, PartTran);
                } /* SCR #7145 - available PlantTran */

                Inventory_Qty = -(Inventory_Qty);  /* flip sign for STK-PLT */
            }

            PartPlant = LibNonQtyBearingBin._NonQtyBearingBin(PartTran.WareHouseCode, PartTran.PartNum);
            if (PartPlant != null && PartPlant.QtyBearing)
            {
                LibDeferredUpdate.PostPBOnHand(PartTran.Company, PartTran.PartNum, PartTran.WareHouseCode, PartTran.BinNum, PartTran.LotNum, PartTran.AttributeSetID, PartTran.InvtyUOM, (Inventory_Qty) * -1, string.Empty, PartTran.PCID);
            }

            /* CREATE PARTTRAN TO REFLECT THE "TO" MOVEMENT */
            if (!vTranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))
            {
                if (hasPartAllocOtherWhse && !Internal.Lib.ErpCallContext.ContainsKey("IsMaterialQueueTFPicking-StaticPCID"))
                {
                    // This flag will let run the ConsumeTrfOrderAlloc method from AppService Lib, which will clear any previous allocation 
                    // from other warehouses different from the current selected FromWarehouse
                    CallContext.Current.Properties.TryAdd("TFOHasPartAllocOtherWhse", "TFOHasPartAllocOtherWhse");
                    Db.Validate(PartTran);
                    CallContext.Current.Properties.TryRemove("TFOHasPartAllocOtherWhse", out object tfoHasPartAllocOtherWhse);
                }
                else
                {
                    Db.Validate(PartTran);
                }

                UpdateLegalNumHistory(PartTran.LegalNumber, partTranPK);

                PartTran From_PartTran = FindFirstPartTran(PartTran.SysRowID);
                Db.Release(ref PartTran);
                DateTime? curDate = From_PartTran.SysDate;
                int curTime = From_PartTran.SysTime;
                bool PartTranError = false;
                LibCreatePartTran.Create_PartTran(ref PartTran, Convert.ToDateTime(curDate), curTime, out PartTranError);
                if (PartTranError == true)
                {
                    throw new BLException(Strings.ErrorCreatingPartTran, "PartTran");
                }

                BufferCopy.CopyExceptFor(From_PartTran, PartTran, PartTran.ColumnNames.SysDate, PartTran.ColumnNames.SysTime, PartTran.ColumnNames.TranNum);
                PartTran.TranQty = -(From_PartTran.TranQty); /* Flip sign - puts it back to positive */
                PartTran.ActTranQty = -(From_PartTran.ActTranQty); /* Flip sign - puts it back to positive */
                PartTran.WareHouse2 = ttIssueReturn.FromWarehouseCode;
                PartTran.BinNum2 = ttIssueReturn.FromBinNum;
                PartTran.PCID2 = ttIssueReturn.FromPCID;
                PartTran.WareHouseCode = ttIssueReturn.ToWarehouseCode;
                PartTran.BinNum = ttIssueReturn.ToBinNum;
                PartTran.PCID = ttIssueReturn.ToPCID;
                PartTran.GLTrans = false;
                Inventory_Qty = -(Inventory_Qty);

                /* SCR 176758 - Flip the signs of the Extended Subcomponent Costs - puts it back to positive */
                PartTran.ExtMtlCost = -(PartTran.ExtMtlCost);
                PartTran.ExtLbrCost = -(PartTran.ExtLbrCost);
                PartTran.ExtBurCost = -(PartTran.ExtBurCost);
                PartTran.ExtSubCost = -(PartTran.ExtSubCost);
                PartTran.ExtMtlBurCost = -(PartTran.ExtMtlBurCost);
                PartTran.ExtMtlMtlCost = -(PartTran.ExtMtlMtlCost);
                PartTran.ExtMtlLabCost = -(PartTran.ExtMtlLabCost);
                PartTran.ExtMtlBurdenCost = -(PartTran.ExtMtlBurdenCost);
                PartTran.ExtMtlSubCost = -(PartTran.ExtMtlSubCost);

                partTranPK = partTranPK + ((partTranPK.Length > 0) ? "|" : "");
                partTranPK = partTranPK + Compatibility.Convert.ToString(PartTran.SysDate) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.SysTime) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.TranNum);
                /* SCR #40698 - Find the Plant/Cost for the "To" Warehouse */

                WhseBin = FindFirstWhseBin(PartTran.Company, PartTran.WareHouseCode, PartTran.BinNum);
                PartTran.BinType = ((WhseBin != null) ? WhseBin.BinType : "");

                PartTran.Plant = vToPlant;

                LibDefPartTran._DefPartTran(ref PartTran, ref Part, ref PartPlant, Session.CompanyID, PartTran.PartNum, PartTran.Plant, PartTranError);
                var outCostID4 = PartTran.CostID;
                LibGetPlantCostID._getPlantCostID(PartTran.Plant, out outCostID4, ref Plant, ref XaSyst);
                PartTran.CostID = outCostID4;

                /* SCR #51749 - Create FIFO layers for non-FIFO cost methods if enabled */
                LibGetCostFIFOLayers.getPartCostFIFOLayers(PartTran.CostID, PartTran.PartNum, PartTran.Plant, out vEnableFIFOLayers);

                /* SCR #40698 - When transferring Stocks between two different CostIDs, *
                * create new FIFO qty for the destination plant costid.                */
                if (vCreateFIFO == true)
                { /* lookup(PartTran.CostMethod,"F,O") */
                    if (StringExtensions.Lookup("F,O", PartTran.CostMethod) > -1)
                    {
                        PartTran.FIFODate = PartTran.TranDate;
                        PartTran.FIFOAction = "A"; /* Add */
                        /* Run the FIFO logic that adds FIFO qty for the new costid.  Create the supporting *
                         * PartFIFOTran records as details of the PartTran record if FIFO costed.           *
                         * CreatePartFIFOCost procedure can be found in lib/ProcessFIFO.i                   */
                        var outFIFOSeq11 = PartTran.FIFOSeq;
                        var outFIFOSubSeq5 = PartTran.FIFOSubSeq;
                        this.LibProcessFIFO.CreatePartFIFOCost(true, true, PartTran.FIFODate, 0, 0, out outFIFOSeq11, out outFIFOSubSeq5, ref PartTran);
                        PartTran.FIFOSeq = outFIFOSeq11;
                        PartTran.FIFOSubSeq = outFIFOSubSeq5;
                    }
                    else if (vEnableFIFOLayers == true)
                    {
                        PartTran.FIFODate = PartTran.TranDate;
                        PartTran.FIFOAction = "NFA"; /* Non-FIFO Add */
                        /* Run the FIFO logic that adds FIFO qty for the new costid.  Create the supporting *
                         * PartFIFOTran records as details of the PartTran record if FIFO costed.           *
                         * CreateNonFIFOCost procedure can be found in lib/ProcessFIFO.i                    */
                        var outFIFOSeq12 = PartTran.FIFOSeq;
                        var outFIFOSubSeq6 = PartTran.FIFOSubSeq;
                        this.LibProcessFIFO.CreateNonFIFOCost(true, PartTran.FIFODate, 0, 0, PartTran.AltMtlUnitCost, PartTran.AltLbrUnitCost, PartTran.AltBurUnitCost, PartTran.AltSubUnitCost, PartTran.AltMtlBurUnitCost, PartTran.AltMtlUnitCost, 0, 0, 0, out outFIFOSeq12, out outFIFOSubSeq6, ref PartTran);
                        PartTran.FIFOSeq = outFIFOSeq12;
                        PartTran.FIFOSubSeq = outFIFOSubSeq6;
                    }
                } /* vCreateFIFO = yes */

                // Create the FSA Ext Data for the PartTran "TO" Movement
                using (var libFSA = new FSAExtDataUtil(Db))
                {
                    string errormsg;
                    if (libFSA.CheckFSALicense(out errormsg) && PartTran.EpicorFSA)
                    {
                        writeFSAExtFields(From_PartTran.SysRowID, PartTran.GetTableName());
                    }
                }

                if ((PartTran.TranReference.IndexOf("TFO:", StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    // This flag specifies is transfer order picking in order to populate PkgControlItem.PkgControlBoolean01 = true (picked)
                    using (Erp.Internal.Lib.ErpCallContext.SetDisposableKey("MaterialQueue-STK-STK-TFO"))
                    {
                        SubmitPartTranFromMovement(ref this.PartTran, Inventory_Qty);
                    }
                }
                else
                {
                    SubmitPartTranFromMovement(ref this.PartTran, Inventory_Qty);
                }
            }
        }

        private void SubmitPartTranFromMovement(ref PartTran partTran, decimal inventoryQty)
        {
            /* UPDATE PARTBIN TO REFLECT THE "FROM" MOVEMENT */
            var partPlant = LibNonQtyBearingBin._NonQtyBearingBin(partTran.WareHouseCode, partTran.PartNum);
            if (partPlant != null && partPlant.QtyBearing)
            {
                LibDeferredUpdate.PostPBOnHand(partTran.Company, partTran.PartNum, partTran.WareHouseCode, partTran.BinNum, partTran.LotNum, partTran.AttributeSetID, partTran.InvtyUOM, inventoryQty * -1, string.Empty, partTran.PCID);
            }
            Db.Validate(partTran);
            UpdateLegalNumHistory(partTran.LegalNumber, partTranPK);
        }

        ///<summary>
        ///  Purpose: Perform the movement of job subcontract receipts from the receiving area to
        ///           the next operations input warehouse/bin (PUR-SUB).
        ///           1. Update the PartWip records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_PUR_SUB(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_WIP_WIP(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement of RMA receipts pending inspection from the receiving area
        ///   to the inspection warehouse (RMA-INS).
        ///           1. This function simply updates the Warehouse/Bin location on the RMARCPT.
        ///
        ///  Parameters:  none
        ///  Notes: A receipt pending inspection can only exist in one warehouse/bin. Currently this is a design
        ///  limitation.
        ///</summary>
        private void process_RMA_INS(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            MtlQueue = this.FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
            if (MtlQueue == null)
            {
                return;
            }

            using (Erp.Internal.Lib.ErpCallContext.SetDisposableKey("Override-RMARcpt-QueWrite"))
            {
                RMARcpt = this.FindFirstRMARcptWithUpdLock(MtlQueue.Company, MtlQueue.RMANum, MtlQueue.RMALine, MtlQueue.RMAReceipt);
                RMARcpt OldRMARcpt = RMARcpt;
                if (RMARcpt != null)
                {
                    RMARcpt.WareHouseCode = ttIssueReturn.ToWarehouseCode;
                    RMARcpt.BinNum = ttIssueReturn.ToBinNum;
                    Db.Validate(RMARcpt);
                }
            }  // end of using ErpCallContext
        }

        ///<summary>
        ///  Purpose: Perform the update to the database for a STK-ASM
        ///           (ISSUE STOCK TO JOB ASSEMBLY) TRANSACTION
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_STK_ASM(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            decimal Trans_Cost = decimal.Zero;
            decimal Trans_MtlCost = decimal.Zero;
            decimal Trans_LbrCost = decimal.Zero;
            decimal Trans_BurCost = decimal.Zero;
            decimal Trans_SubCost = decimal.Zero;
            decimal Trans_MtlBurCost = decimal.Zero;
            int Parent_AssemblySeq = 0;
            int Escape_Counter = 0;
            //int SNQty = 0;
            //bool findAssmbl = false;
            decimal Orig_PullQty = decimal.Zero;
            decimal adjRoundingError = decimal.Zero;
            decimal absMaxCost = decimal.Zero;
            decimal Conv_Qty = decimal.Zero;
            /* SCR #40698 - Variables used by lib/AsmCostUpdate.i */
            bool vSplitMfgCosts = false;
            bool vUpdMfgComponents = false;
            string cLegalNumber = string.Empty;

            outMessage = string.Empty;

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part == null)
            {
                throw new BLException(Strings.AValidPartNumberIsRequired, "IssueReturn", "PartNum");
            }
            if (!this.ValidUOM(ttIssueReturn.PartNum, ttIssueReturn.UM))
            {
                throw new BLException(Strings.InvalidUOMForPartNumber, "IssueReturn", "UM");
            }
            okWhse = false;

            JobHead = this.FindFirstJobHead(Session.CompanyID, ttIssueReturn.ToJobNum);
            if (!JobHead.Plant.Equals(Session.PlantID, StringComparison.OrdinalIgnoreCase))
            {
                this.checkWarehouseBin(ttIssueReturn.FromWarehouseCode, JobHead.Plant, out okWhse);
                if (!okWhse)
                {
                    throw new BLException(Strings.InvalidFromWarehouse, "IssueReturn", "FromWarehouseCode");
                }
            }
            this.createPartTran(ttIssueReturn);
            PartTran.TranClass = "I";
            PartTran.TranType = ttIssueReturn.TranType;
            PartTran.InventoryTrans = true;
            PartTran.JobNum = ttIssueReturn.ToJobNum;
            PartTran.AssemblySeq = ttIssueReturn.ToAssemblySeq;
            PartTran.JobSeq = ttIssueReturn.ToJobSeq;
            PartTran.PartNum = ttIssueReturn.PartNum;
            PartTran.PartDescription = ttIssueReturn.PartPartDescription;
            PartTran.WareHouseCode = ttIssueReturn.FromWarehouseCode;
            PartTran.BinNum = ttIssueReturn.FromBinNum;
            PartTran.PCID = ttIssueReturn.FromPCID;
            //DJY - PartTran.PCID2 should not be populated - adding it to WIP will be handled by PartWip triggers
            PartTran.PCID2 = string.Empty;
            PartTran.WIPPCID2 = ttIssueReturn.ToPCID;
            partTranPK = partTranPK + ((partTranPK.Length > 0) ? "|" : "");
            partTranPK = partTranPK + Compatibility.Convert.ToString(PartTran.SysDate) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.SysTime) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.TranNum);

            WhseBin = this.FindFirstWhseBin(Session.CompanyID, PartTran.WareHouseCode, PartTran.BinNum);
            PartTran.BinType = ((WhseBin != null) ? WhseBin.BinType : "");

            if (!Part.TrackInventoryByRevision)
            {
                /*run getPartRev.*/
                PartRev = this.FindLastPartRev(Session.CompanyID, ttIssueReturn.PartNum, CompanyTime.Today());
                if (PartRev != null)
                {
                    PartTran.RevisionNum = PartRev.RevisionNum;
                }
                else
                {
                    PartTran.RevisionNum = "";
                }
            }

            PartTran.Plant = getPlantFromWarehouse(PartTran.WareHouseCode);

            /* Get Cost method for the PartNum */
            PartTran.CostMethod = this.getPartCostMethod(PartTran.Plant, PartTran.PartNum);

            if (PartTran.PCID.KeyEquals(PartTran.WIPPCID2))
            {
                Erp.Internal.Lib.ErpCallContext.SetDisposableKey("DisableVoidPCIDWhenEmpty");
            }

            this.updatePartBin(PartTran);

            JobHead = this.FindFirstJobHead(Session.CompanyID, ttIssueReturn.ToJobNum);
            if (JobHead == null)
            {
                throw new BLException(Strings.AValidJobNumberIsRequired, "IssueReturn", "ToJobNum");
            }

            PartTran.CallNum = JobHead.CallNum;
            PartTran.CallLine = JobHead.CallLine;
            PartTran.Plant2 = JobHead.Plant;
            /* SCR #40698 - Get the system setting for updating the Mfg Cost Elements and *
             * set flag to indicate if updating the Mfg component cost buckets.           */
            vSplitMfgCosts = JobHead.SplitMfgCostElements;
            vUpdMfgComponents = ((Part != null && StringExtensions.Compare(Part.TypeCode, "M") == 0) ? true : false);
            /* Get cost from Part - defined in lib/InvCosts.i*/
            var outMtlUnitCost8 = PartTran.MtlUnitCost;
            var outLbrUnitCost8 = PartTran.LbrUnitCost;
            var outBurUnitCost8 = PartTran.BurUnitCost;
            var outSubUnitCost8 = PartTran.SubUnitCost;
            var outMtlBurUnitCost8 = PartTran.MtlBurUnitCost;
            this.LibInvCosts.getInvCost(ttIssueReturn.LotNum, ((!String.IsNullOrEmpty(PartTran.Plant)) ? PartTran.Plant : Session.PlantID), PartTran.PartNum, PartTran.CostMethod, out outMtlUnitCost8, out outLbrUnitCost8, out outBurUnitCost8, out outSubUnitCost8, out outMtlBurUnitCost8, ref Part);
            PartTran.MtlUnitCost = outMtlUnitCost8;
            PartTran.LbrUnitCost = outLbrUnitCost8;
            PartTran.BurUnitCost = outBurUnitCost8;
            PartTran.SubUnitCost = outSubUnitCost8;
            PartTran.MtlBurUnitCost = outMtlBurUnitCost8;
            /* SCR #51749 - Create FIFO layers for non-FIFO cost methods if enabled */
            var outCostID5 = PartTran.CostID;
            this.LibGetPlantCostID._getPlantCostID(PartTran.Plant, out outCostID5, ref Plant, ref XaSyst);
            PartTran.CostID = outCostID5;

            this.LibGetCostFIFOLayers.getPartCostFIFOLayers(PartTran.CostID, PartTran.PartNum, PartTran.Plant, out vEnableFIFOLayers);

            /* SCR #40698 - If the part is FIFO Costed then consume the FIFO qty/cost. */
            /* PartTran.CostMethod = "F" (FIFO) or "O" (LOT FIFO) */
            /* SCR #51749 - If non-FIFO Costed but FIFO Layers enabled then consume FIFO qty/cost. */
            if (StringExtensions.Lookup("F,O", PartTran.CostMethod) > -1)
            {
                PartTran.FIFOAction = "D"; /* Decrease */
                var outFIFODate11 = PartTran.FIFODate;
                var outFIFOSeq15 = PartTran.FIFOSeq;
                var outFIFOSubSeq9 = PartTran.FIFOSubSeq;
                var outMtlUnitCost10 = PartTran.MtlUnitCost;
                var outLbrUnitCost10 = PartTran.LbrUnitCost;
                var outBurUnitCost10 = PartTran.BurUnitCost;
                var outSubUnitCost10 = PartTran.SubUnitCost;
                var outMtlBurUnitCost10 = PartTran.MtlBurUnitCost;
                /* SCR 176758 - Call ConsumePartFIFOCost with vUpdPartTran option to update the PartTran's Extended Subcomponent Costs */
                LibProcessFIFO.ConsumePartFIFOCost(true, out outFIFODate11, out outFIFOSeq15, out outFIFOSubSeq9, out outMtlUnitCost10, out outLbrUnitCost10, out outBurUnitCost10, out outSubUnitCost10, out outMtlBurUnitCost10, ref PartTran, true);
                PartTran.FIFODate = (DateTime)outFIFODate11;
                PartTran.FIFOSeq = outFIFOSeq15;
                PartTran.FIFOSubSeq = outFIFOSubSeq9;
                PartTran.MtlUnitCost = outMtlUnitCost10;
                PartTran.LbrUnitCost = outLbrUnitCost10;
                PartTran.BurUnitCost = outBurUnitCost10;
                PartTran.SubUnitCost = outSubUnitCost10;
                PartTran.MtlBurUnitCost = outMtlBurUnitCost10;
                ExceptionManager.AssertNoBLExceptions();
                PartTran.MtlMtlUnitCost = PartTran.MtlUnitCost;
                PartTran.MtlLabUnitCost = 0;
                PartTran.MtlSubUnitCost = 0;
                PartTran.MtlBurdenUnitCost = 0;
                /* PartTran.ExtMtlXXXCosts are not updated in ConsumePartFIFOCost, update extended material subcomponent costs here */
                PartTran.ExtMtlMtlCost = PartTran.ExtMtlCost;
                PartTran.ExtMtlLabCost = 0;
                PartTran.ExtMtlSubCost = 0;
                PartTran.ExtMtlBurdenCost = 0;
            }
            else if (vEnableFIFOLayers == true)
            {
                PartTran.FIFOAction = "NFD"; /* Non-FIFO Decrease */
                var outFIFODate12 = PartTran.FIFODate;
                var outFIFOSeq16 = PartTran.FIFOSeq;
                var outFIFOSubSeq10 = PartTran.FIFOSubSeq;
                var outAltMtlUnitCost5 = PartTran.AltMtlUnitCost;
                var outAltLbrUnitCost5 = PartTran.AltLbrUnitCost;
                var outAltBurUnitCost5 = PartTran.AltBurUnitCost;
                var outAltSubUnitCost5 = PartTran.AltSubUnitCost;
                var outAltMtlBurUnitCost5 = PartTran.AltMtlBurUnitCost;
                this.LibProcessFIFO.ConsumeNonFIFOCost(true, out outFIFODate12, out outFIFOSeq16, out outFIFOSubSeq10, out outAltMtlUnitCost5, out outAltLbrUnitCost5, out outAltBurUnitCost5, out outAltSubUnitCost5, out outAltMtlBurUnitCost5, ref PartTran);
                PartTran.FIFODate = (DateTime)outFIFODate12;
                PartTran.FIFOSeq = outFIFOSeq16;
                PartTran.FIFOSubSeq = outFIFOSubSeq10;
                PartTran.AltMtlUnitCost = outAltMtlUnitCost5;
                PartTran.AltLbrUnitCost = outAltLbrUnitCost5;
                PartTran.AltBurUnitCost = outAltBurUnitCost5;
                PartTran.AltSubUnitCost = outAltSubUnitCost5;
                PartTran.AltMtlBurUnitCost = outAltMtlBurUnitCost5;
                ExceptionManager.AssertNoBLExceptions();
                PartTran.AltMtlMtlUnitCost = PartTran.AltMtlUnitCost;
                PartTran.AltMtlLabUnitCost = 0;
                PartTran.AltMtlSubUnitCost = 0;
                PartTran.AltMtlBurdenUnitCost = 0;
            }

            JobAsmbl = this.FindFirstJobAsmblWithUpdLock(Session.CompanyID, PartTran.JobNum, PartTran.AssemblySeq);
            if (JobAsmbl == null)
            {
                throw new BLException(Strings.AValidJobAssemblyIsRequired, "IssueReturn", "AssemblySeq");
            }
            PartTran.ContractID = (JobAsmbl.LinkToContract) ? JobAsmbl.ContractID : "";

            using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
            {
                libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, true, ttIssueReturn.ToPCID, true, "ISSUEASSEMBLY");
            }

            Db.Validate(PartTran);

            if (PartTran.PCID.KeyEquals(PartTran.WIPPCID2))
            {
                Erp.Internal.Lib.ErpCallContext.RemoveValue("DisableVoidPCIDWhenEmpty");
            }

            UpdateLegalNumHistory(PartTran.LegalNumber, partTranPK);

            ExceptionManager.AssertNoBLExceptions(); /* PartTran.CostMethod = "F" (FIFO) or "O" (LOT FIFO) */
            /* ISSUE TO ANOTHER PLANT ?  if YES then CHANGE THE TRAN TYPE TO STK-PLT */
            if (StringExtensions.Compare(PartTran.Plant, PartTran.Plant2) != 0)
            {
                PartTran.TranType = "STK-PLT";
            }
            /* STK-PLT TRANSACTION - DO THE PLANT TO PLANT LOGIC */
            if (StringExtensions.Compare(PartTran.TranType, "STK-PLT") == 0)
            {
                this.IMIMPlant.CreatePlantTran(PartTran.TranType, JobHead.ProdCode, PartTran.TranQty, JobHead.CallNum, JobHead.CallLine, ref PartTran, ref PlantTran);
                /* FYI:  NO NEED TO assign G/L ACCOUNTS HERE for STK-PLT TRANSACTIONS.
                   THEY ARE assignED IN THE lib/SetInterPlant.i LOGIC FOUND IN im/implant.i */

                /* SCR-148038 Automate the Receive From Plant procedure to make the issue from shared warehouse to job */
                if (PlantTran != null)
                {
                    PlantTran.RecTranDate = ttIssueReturn.TranDate;
                    PlantTran.RecIssuedComplete = ttIssueReturn.IssuedComplete;
                    this.IMIMPlant.GeneratePlantReceiptTran(PlantTran.SysRowID, "", ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, PlantTran.JobNum, PlantTran.AssemblySeq, PlantTran.JobMtl, cLegalNumber, ttIssueReturn.TranDocTypeID, PlantTran.PackNum, string.Empty, PartTran);
                }

                return;
            }
            /* NOT A STK-PLT - CONTINUE PROCESSING THE STK-ASM TRANSACTION */

            this.LibAppService.UOMConv(PartTran.PartNum, PartTran.ActTranQty, PartTran.ActTransUOM, JobAsmbl.IUM, out Conv_Qty, false);
            JobAsmbl.IssuedQty = JobAsmbl.IssuedQty + Conv_Qty;
            JobAsmbl.IssuedComplete = ttIssueReturn.IssuedComplete;
            int nDec = 0;
            nDec = this.LibGetDecimalsNumber.getDecimalsNumberByName("JobAsmbl", "TotalCost", "");
            /* SCR 176758 - If the Extended Subcomponent Costs are available then use them instead of deriving costs from TranQty and UnitCosts */
            Trans_MtlCost = (PartTran.ExtMtlCost != 0) ? PartTran.ExtMtlCost : Math.Round(PartTran.TranQty * PartTran.MtlUnitCost, nDec, MidpointRounding.AwayFromZero);
            Trans_LbrCost = (PartTran.ExtLbrCost != 0) ? PartTran.ExtLbrCost : Math.Round(PartTran.TranQty * PartTran.LbrUnitCost, nDec, MidpointRounding.AwayFromZero);
            Trans_BurCost = (PartTran.ExtBurCost != 0) ? PartTran.ExtBurCost : Math.Round(PartTran.TranQty * PartTran.BurUnitCost, nDec, MidpointRounding.AwayFromZero);
            Trans_SubCost = (PartTran.ExtSubCost != 0) ? PartTran.ExtSubCost : Math.Round(PartTran.TranQty * PartTran.SubUnitCost, nDec, MidpointRounding.AwayFromZero);
            Trans_MtlBurCost = (PartTran.ExtMtlBurCost != 0) ? PartTran.ExtMtlBurCost : Math.Round(PartTran.TranQty * PartTran.MtlBurUnitCost, nDec, MidpointRounding.AwayFromZero);
            Trans_Cost = Trans_MtlCost + Trans_LbrCost + Trans_BurCost + Trans_SubCost;

            /* if ISSUED QTY IS NOW ZERO then BE SURE THAT THE COST IS ALSO */
            if (JobAsmbl.IssuedQty == 0 && (JobAsmbl.TotalCost + Trans_Cost) != 0 && (Epicor.Utilities.StringExtensions.Lookup("F,O", PartTran.CostMethod) == -1))
            {
                int nDecimals = 0;
                nDecimals = this.LibGetDecimalsNumber.getDecimalsNumberByName("PartTran", "MtlUnitCost", "");
                Trans_MtlCost = -(JobAsmbl.TotalMtlMtlCost);
                Trans_LbrCost = -(JobAsmbl.TotalMtlLabCost);
                Trans_BurCost = -(JobAsmbl.TotalMtlBurCost);
                Trans_SubCost = -(JobAsmbl.TotalMtlSubCost);
                Trans_MtlBurCost = -(JobAsmbl.MtlBurCost);
                Trans_Cost = -(JobAsmbl.TotalCost);
                PartTran.MtlUnitCost = Math.Round(Trans_MtlCost / PartTran.TranQty, nDecimals, MidpointRounding.AwayFromZero);
                PartTran.LbrUnitCost = Math.Round(Trans_LbrCost / PartTran.TranQty, nDecimals, MidpointRounding.AwayFromZero);
                PartTran.BurUnitCost = Math.Round(Trans_BurCost / PartTran.TranQty, nDecimals, MidpointRounding.AwayFromZero);
                PartTran.SubUnitCost = Math.Round(Trans_SubCost / PartTran.TranQty, nDecimals, MidpointRounding.AwayFromZero);
                PartTran.MtlBurUnitCost = Math.Round(Trans_MtlBurCost / PartTran.TranQty, nDecimals, MidpointRounding.AwayFromZero);
            }
            else
            {
                Db.ReadCurrent(ref PartTran, LockHint.UpdLock);
                /* Compute the rounding error */
                adjRoundingError = PartTran.ExtCost - /* Total - Sum of the parts */
                                  (Trans_MtlCost + Trans_LbrCost + Trans_BurCost + Trans_SubCost + Trans_MtlBurCost);
                /* SCR #59049 - Adjust the highest component cost with the rounding difference instead of just   *
                 * adding it to the material cost. This is to avoid negative cost in case material cost is zero. *
                 * It's important to keep this order (Mtl/Lbr/Bur/Sub) to check where to add the rounding error. */
                if (adjRoundingError != 0)
                {
                    absMaxCost = Math.Max(Math.Max(Math.Max(Math.Abs(Trans_MtlCost), Math.Abs(Trans_LbrCost)), Math.Abs(Trans_BurCost)), Math.Abs(Trans_SubCost));
                    if (Math.Abs(Trans_MtlCost) == absMaxCost)
                    {
                        Trans_MtlCost = Trans_MtlCost + adjRoundingError;
                    }
                    else if (Math.Abs(Trans_LbrCost) == absMaxCost)
                    {
                        Trans_LbrCost = Trans_LbrCost + adjRoundingError;
                    }
                    else if (Math.Abs(Trans_BurCost) == absMaxCost)
                    {
                        Trans_BurCost = Trans_BurCost + adjRoundingError;
                    }
                    else
                    {
                        Trans_SubCost = Trans_SubCost + adjRoundingError;
                    }
                }
                /* Apply the rounding error and make sure the total is right */
                Trans_Cost = Trans_MtlCost + Trans_LbrCost + Trans_BurCost + Trans_SubCost;
            }

            this.LibAsmCostUpdate.runAsmCostUpdate(Escape_Counter, true, Trans_Cost, Trans_MtlCost, Trans_LbrCost, Trans_BurCost, Trans_SubCost, Trans_MtlBurCost, Orig_PullQty, Parent_AssemblySeq, vUpdMfgComponents, vSplitMfgCosts, PartTran, JobAsmbl);

            this.updatePartWIP("TO", ttIssueReturn);
        }

        ///<summary>
        ///  Purpose: Perform the movement of non-conforming stock (STK-INS) from stocking area to the
        ///           inspection warehouse.
        ///           1. This function simply updates the Warehouse/Bin location on the NonConf.
        ///Note
        ///Since we update the nonconf record directly we really can't just change the WarehouseCode/BinNum since they
        ///represent where the nonconformance was found at. The NonConf maint program actually reduces the PartBin quantity at
        ///this warehouse/bin so we don't want to change it because they may perform maintainance. Therefore we consider the
        ///ToWarehouse/ToBinNum as the current location of the NonConformance.
        ///</summary>
        private void process_STK_INS(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_MTL_INS(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the update to the database for a STK-MTL (ISSUE STOCK TO JOB MATERIAL) TRANSACTION
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_STK_MTL(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            /* scr3974 - Bab -
                These variables are used to hold the individual component amounts to be
                applied to the JobMtl record.  each of these may have a rounding error associated with
                it which makes the sum of the parts NOT equal to the PartTran extended cost (PartTran.ExtCost).
                To correctly post these to the JobMtl record an adjustment (adjRoundingError) will be computed
                which is the difference between the sum of the parts and PartTran.ExtCost.
                The adjRoundingError will be applied to the MaterialMtlCost.
            * SCR #59049 - Modified the rounding logic to apply the rounding difference to the highest
            * component costs instead of always adding it to MaterialMtlCost. This is to avoid negative
            * cost in material cost in case the rounding amount is negative and no current material cost.
            */
            decimal amtMaterialMtlCost = decimal.Zero;
            decimal amtMaterialLabCost = decimal.Zero;
            decimal amtMaterialBurCost = decimal.Zero;
            decimal amtMaterialSubCost = decimal.Zero;
            decimal amtMtlBurCost = decimal.Zero;
            decimal adjRoundingError = decimal.Zero;
            decimal absMaxCost = decimal.Zero;
            string cLegalNumber = string.Empty;
            //int iLegalNumberYear = 0;
            string cLegalNumberType = string.Empty;
            string cLegalNumberPrefix = string.Empty;
            string cLegalNumberNumber = string.Empty;
            string cLegalNumberPrefixList = string.Empty;
            string cLegalNumberReadOnlyFields = string.Empty;
            //bool lValidLegalNumber = false;
            string cMessage = string.Empty;
            decimal Conv_Qty = decimal.Zero;

            outMessage = string.Empty;

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part == null)
            {
                throw new BLException(Strings.AValidPartNumberIsRequired, "IssueReturn", "PartNum");
            }
            if (!this.ValidUOM(ttIssueReturn.PartNum, ttIssueReturn.UM))
            {
                throw new BLException(Strings.InvalidUOMForPartNumber, "IssueReturn", "UM");
            }
            okWhse = false;

            JobHead = this.FindFirstJobHead(Session.CompanyID, ttIssueReturn.ToJobNum);
            if (!JobHead.Plant.Equals(Session.PlantID, StringComparison.OrdinalIgnoreCase))
            {
                this.checkWarehouseBin(ttIssueReturn.FromWarehouseCode, JobHead.Plant, out okWhse);
                if (!okWhse)
                {
                    throw new BLException(Strings.InvalidFromWarehouse, "IssueReturn", "FromWarehouseCode");
                }
            }

            ttIssueReturn.ToJobNum = JobHead.JobNum; //to remove possible trailing spaces
            /* check if moving from a supplier managed bin  */

            WhseBin = this.FindFirstWhseBin(ttIssueReturn.Company, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum);
            if (WhseBin != null && StringExtensions.Compare(WhseBin.BinType, "Std") != 0)
            {
                throw new BLException(Strings.CannotPerformSTKMTLTransToAManagedBin, "IssueReturn", "ToBinNum");
            }

            WhseBin = this.FindFirstWhseBin(ttIssueReturn.Company, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum);
            /* if Supplier managed bin, then auto create a PackSlip to update costs - no change to OnHandQty  */
            if (WhseBin != null && StringExtensions.Compare(WhseBin.BinType, "Supp") == 0 && ttIssueReturn.TranQty > 0)
            {
                LibGenSMIReceipt.GenerateSMIReceipt(ttIssueReturn.TranQty, ttIssueReturn.UM, WhseBin.VendorNum, ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, "STK-STK", ttIssueReturn.Company, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, null);
            }

            /*
             * Moved validation of PkgControl Statuses before creating PartTran record.
             * PkgControl status validation was failing because when everything is taken from a STOCK PCID, status change to EMPTY.
             * Validation expected it to be STOCK.
             */
            using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
            {
                libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, true, ttIssueReturn.ToPCID, true, "ISSUEMATERIAL");
            }

            this.createPartTran(ttIssueReturn);
            PartTran.TranClass = "I";
            PartTran.TranType = ttIssueReturn.TranType;
            PartTran.InventoryTrans = true;
            PartTran.JobNum = ttIssueReturn.ToJobNum;
            PartTran.AssemblySeq = ttIssueReturn.ToAssemblySeq;
            PartTran.JobSeq = ttIssueReturn.ToJobSeq;
            PartTran.PartNum = ttIssueReturn.PartNum;
            PartTran.PartDescription = ttIssueReturn.PartPartDescription;
            PartTran.WareHouseCode = ttIssueReturn.FromWarehouseCode;
            PartTran.BinNum = ttIssueReturn.FromBinNum;
            PartTran.PCID = ttIssueReturn.FromPCID;
            PartTran.WareHouse2 = ttIssueReturn.ToWarehouseCode;
            PartTran.BinNum2 = ttIssueReturn.ToBinNum;
            //DJY - PartTran.PCID2 should not be populated - adding it to WIP will be handled by PartWip triggers
            PartTran.PCID2 = string.Empty;
            PartTran.WIPPCID2 = ttIssueReturn.ToPCID;
            PartTran.BinType = ((WhseBin != null) ? WhseBin.BinType : "");
            partTranPK = partTranPK + ((partTranPK.Length > 0) ? "|" : "");
            partTranPK = partTranPK + Compatibility.Convert.ToString(PartTran.SysDate) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.SysTime) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.TranNum);

            if (!Part.TrackInventoryByRevision)
            {
                /*run getPartRev.*/
                PartRev = this.FindLastPartRev(Session.CompanyID, ttIssueReturn.PartNum, CompanyTime.Today());
                if (PartRev != null)
                {
                    PartTran.RevisionNum = PartRev.RevisionNum;
                }
                else
                {
                    PartTran.RevisionNum = "";
                }
            }

            PartTran.Plant = getPlantFromWarehouse(PartTran.WareHouseCode);

            /* Get Cost method for the PartNum */
            PartTran.CostMethod = this.getPartCostMethod(PartTran.Plant, PartTran.PartNum);

            if (PartTran.PCID.KeyEquals(PartTran.WIPPCID2))
            {
                Erp.Internal.Lib.ErpCallContext.SetDisposableKey("DisableVoidPCIDWhenEmpty");
            }

            this.updatePartBin(PartTran);

            /* BinType <> 'Cust' */
            if (StringExtensions.Compare(WhseBin.BinType, "Cust") != 0)
            {
                var outMtlUnitCost14 = PartTran.MtlUnitCost;
                var outLbrUnitCost14 = PartTran.LbrUnitCost;
                var outBurUnitCost14 = PartTran.BurUnitCost;
                var outSubUnitCost14 = PartTran.SubUnitCost;
                var outMtlBurUnitCost14 = PartTran.MtlBurUnitCost;
                this.LibInvCosts.getInvCost(ttIssueReturn.LotNum, ((!String.IsNullOrEmpty(PartTran.Plant)) ? PartTran.Plant : Session.PlantID), PartTran.PartNum, PartTran.CostMethod, out outMtlUnitCost14, out outLbrUnitCost14, out outBurUnitCost14, out outSubUnitCost14, out outMtlBurUnitCost14, ref Part);
                PartTran.MtlUnitCost = outMtlUnitCost14;
                PartTran.LbrUnitCost = outLbrUnitCost14;
                PartTran.BurUnitCost = outBurUnitCost14;
                PartTran.SubUnitCost = outSubUnitCost14;
                PartTran.MtlBurUnitCost = outMtlBurUnitCost14;
                var outCostID7 = PartTran.CostID;
                this.LibGetPlantCostID._getPlantCostID(PartTran.Plant, out outCostID7, ref Plant, ref XaSyst);
                PartTran.CostID = outCostID7;

                /* SCR #51749 - Create FIFO layers for non-FIFO cost methods if enabled */
                this.LibGetCostFIFOLayers.getPartCostFIFOLayers(PartTran.CostID, PartTran.PartNum, PartTran.Plant, out vEnableFIFOLayers);

                /* SCR #40698 - If the part is FIFO Costed then consume the FIFO qty/cost. */
                /* PartTran.CostMethod = "F" (FIFO) or "O" (LOT FIFO) */
                /* SCR #51749 - If non-FIFO Costed but FIFO Layers enabled then consume the FIFO qty/cost. */
                if (StringExtensions.Lookup("F,O", PartTran.CostMethod) > -1)
                {
                    PartTran.FIFOAction = "D"; /* Decrease */
                    var outFIFODate19 = PartTran.FIFODate;
                    var outFIFOSeq23 = PartTran.FIFOSeq;
                    var outFIFOSubSeq17 = PartTran.FIFOSubSeq;
                    var outMtlUnitCost16 = PartTran.MtlUnitCost;
                    var outLbrUnitCost16 = PartTran.LbrUnitCost;
                    var outBurUnitCost16 = PartTran.BurUnitCost;
                    var outSubUnitCost16 = PartTran.SubUnitCost;
                    var outMtlBurUnitCost16 = PartTran.MtlBurUnitCost;
                    /* SCR 176758 - Call ConsumePartFIFOCost with vUpdPartTran option to update the PartTran's Extended Subcomponent Costs */
                    LibProcessFIFO.ConsumePartFIFOCost(true, out outFIFODate19, out outFIFOSeq23, out outFIFOSubSeq17, out outMtlUnitCost16, out outLbrUnitCost16, out outBurUnitCost16, out outSubUnitCost16, out outMtlBurUnitCost16, ref PartTran, true);
                    PartTran.FIFODate = (DateTime)outFIFODate19;
                    PartTran.FIFOSeq = outFIFOSeq23;
                    PartTran.FIFOSubSeq = outFIFOSubSeq17;
                    PartTran.MtlUnitCost = outMtlUnitCost16;
                    PartTran.LbrUnitCost = outLbrUnitCost16;
                    PartTran.BurUnitCost = outBurUnitCost16;
                    PartTran.SubUnitCost = outSubUnitCost16;
                    PartTran.MtlBurUnitCost = outMtlBurUnitCost16;
                    ExceptionManager.AssertNoBLExceptions();
                    PartTran.MtlMtlUnitCost = PartTran.MtlUnitCost;
                    PartTran.MtlLabUnitCost = 0;
                    PartTran.MtlSubUnitCost = 0;
                    PartTran.MtlBurdenUnitCost = 0;
                    /* PartTran.ExtMtlXXXCosts are not updated in ConsumePartFIFOCost, update extended material subcomponent costs here */
                    PartTran.ExtMtlMtlCost = PartTran.ExtMtlCost;
                    PartTran.ExtMtlLabCost = 0;
                    PartTran.ExtMtlSubCost = 0;
                    PartTran.ExtMtlBurdenCost = 0;
                }
                else if (vEnableFIFOLayers == true)
                {
                    PartTran.FIFOAction = "NFD"; /* Non-FIFO Decrease */
                    var outFIFODate20 = PartTran.FIFODate;
                    var outFIFOSeq24 = PartTran.FIFOSeq;
                    var outFIFOSubSeq18 = PartTran.FIFOSubSeq;
                    var outAltMtlUnitCost9 = PartTran.AltMtlUnitCost;
                    var outAltLbrUnitCost9 = PartTran.AltLbrUnitCost;
                    var outAltBurUnitCost9 = PartTran.AltBurUnitCost;
                    var outAltSubUnitCost9 = PartTran.AltSubUnitCost;
                    var outAltMtlBurUnitCost9 = PartTran.AltMtlBurUnitCost;
                    this.LibProcessFIFO.ConsumeNonFIFOCost(true, out outFIFODate20, out outFIFOSeq24, out outFIFOSubSeq18, out outAltMtlUnitCost9, out outAltLbrUnitCost9, out outAltBurUnitCost9, out outAltSubUnitCost9, out outAltMtlBurUnitCost9, ref PartTran);
                    PartTran.FIFODate = (DateTime)outFIFODate20;
                    PartTran.FIFOSeq = outFIFOSeq24;
                    PartTran.FIFOSubSeq = outFIFOSubSeq18;
                    PartTran.AltMtlUnitCost = outAltMtlUnitCost9;
                    PartTran.AltLbrUnitCost = outAltLbrUnitCost9;
                    PartTran.AltBurUnitCost = outAltBurUnitCost9;
                    PartTran.AltSubUnitCost = outAltSubUnitCost9;
                    PartTran.AltMtlBurUnitCost = outAltMtlBurUnitCost9;
                    ExceptionManager.AssertNoBLExceptions();
                    PartTran.AltMtlMtlUnitCost = PartTran.AltMtlUnitCost;
                    PartTran.AltMtlLabUnitCost = 0;
                    PartTran.AltMtlSubUnitCost = 0;
                    PartTran.AltMtlBurdenUnitCost = 0;
                } /* vEnableFIFOLayers = true */
                PartTran.MtlMtlUnitCost = PartTran.MtlUnitCost;
            }
            else
            {
                PartTran.GLTrans = false;
            }

            ExceptionManager.AssertNoBLExceptions();

            JobHead = this.FindFirstJobHead(Session.CompanyID, ttIssueReturn.ToJobNum);
            if (JobHead == null)
            {
                throw new BLException(Strings.AValidJobNumberIsRequired, "IssueReturn", "FromJobNum");
            }

            /* SCR #1669 - assign the service call number */
            PartTran.CallNum = JobHead.CallNum;
            PartTran.CallLine = JobHead.CallLine;
            PartTran.Plant2 = JobHead.Plant;

            /* ISSUE TO ANOTHER PLANT ?  if YES then CHANGE THE TRAN TYPE TO STK-PLT */
            if (StringExtensions.Compare(PartTran.Plant, PartTran.Plant2) != 0)
            {
                PartTran.TranType = "STK-PLT";
            }
            /* STK-PLT TRANSACTION - DO THE PLANT TO PLANT LOGIC */
            if (StringExtensions.Compare(PartTran.TranType, "STK-PLT") == 0)
            {
                this.IMIMPlant.CreatePlantTran(PartTran.TranType, JobHead.ProdCode, PartTran.TranQty, JobHead.CallNum, JobHead.CallLine, ref PartTran, ref PlantTran);
                /* FYI:  NO NEED TO assign G/L ACCOUNTS HERE for STK-PLT TRANSACTIONS.
                THEY ARE assignED IN THE lib/SetInterPlant.i LOGIC FOUND IN im/implant.i */

                /* SCR #7145 - Automate the Receive From Plant procedure to make the issue *
                 * from shared warehouse to job appear like it involves local warehouse.   */
                if (PlantTran != null)
                {
                    if (ttIssueReturn.TranDate == null)
                    {
                        PlantTran.RecTranDate = null;
                    }
                    else
                    {
                        PlantTran.RecTranDate = ttIssueReturn.TranDate;
                    }

                    PlantTran.RecIssuedComplete = ttIssueReturn.IssuedComplete;
                    this.IMIMPlant.GeneratePlantReceiptTran(PlantTran.SysRowID, "", ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, PlantTran.JobNum, PlantTran.AssemblySeq, PlantTran.JobMtl, cLegalNumber, ttIssueReturn.TranDocTypeID, PlantTran.PackNum, string.Empty, PartTran);
                } /* SCR #7145 - available PlantTran */
                return;
            }/*if PartTran.TranType = "STK-PLT":U */

            JobMtl = this.FindFirstJobMtlWithUpdLock(Session.CompanyID, PartTran.JobNum, PartTran.AssemblySeq, PartTran.JobSeq);
            if (JobMtl == null)
            {
                throw new BLException(Strings.JobsMaterInforIsNotOnFileCannotIssue, "IssueReturn", "AssemblySeq");
            }
            PartTran.ContractID = (JobMtl.LinkToContract) ? JobMtl.ContractID : "";

            // PartTran must be validated so PartTran.ExtPrice is calculated 
            Db.Validate(PartTran);

            if (PartTran.PCID.KeyEquals(PartTran.WIPPCID2))
            {
                Erp.Internal.Lib.ErpCallContext.RemoveValue("DisableVoidPCIDWhenEmpty");
            }

            UpdateLegalNumHistory(PartTran.LegalNumber, partTranPK);

            /* NOT A STK-PLT - CONTINUE PROCESSING THE STK-MTL TRANSACTION */

            this.LibAppService.UOMConv(PartTran.PartNum, PartTran.ActTranQty, PartTran.ActTransUOM, JobMtl.IUM, out Conv_Qty, false);

            /* UPDATE JobMtl */
            JobMtl.IssuedQty = JobMtl.IssuedQty + Conv_Qty;
            JobMtl.IssuedComplete = ttIssueReturn.IssuedComplete;
            /* COSTS */
            if (JobMtl.IssuedQty == 0)
            {
                JobMtl.TotalCost = 0;
                JobMtl.MtlBurCost = 0;
                JobMtl.MaterialMtlCost = 0;
                JobMtl.MaterialLabCost = 0;
                JobMtl.MaterialBurCost = 0;
                JobMtl.MaterialSubCost = 0;
            }
            else
            {
                int ndec = 0;
                ndec = this.LibGetDecimalsNumber.getDecimalsNumberByName("JobMtl", "MaterialMtlCost", "");
                /* SCR 176758 - If the Extended Subcomponent Costs are available then use them instead of deriving costs from TranQty and UnitCosts */
                amtMaterialMtlCost = (PartTran.ExtMtlCost != 0) ? PartTran.ExtMtlCost : Math.Round((PartTran.TranQty * PartTran.MtlUnitCost), ndec, MidpointRounding.AwayFromZero);
                amtMaterialLabCost = (PartTran.ExtLbrCost != 0) ? PartTran.ExtLbrCost : Math.Round((PartTran.TranQty * PartTran.LbrUnitCost), ndec, MidpointRounding.AwayFromZero);
                amtMaterialBurCost = (PartTran.ExtBurCost != 0) ? PartTran.ExtBurCost : Math.Round((PartTran.TranQty * PartTran.BurUnitCost), ndec, MidpointRounding.AwayFromZero);
                amtMaterialSubCost = (PartTran.ExtSubCost != 0) ? PartTran.ExtSubCost : Math.Round((PartTran.TranQty * PartTran.SubUnitCost), ndec, MidpointRounding.AwayFromZero);
                amtMtlBurCost = (PartTran.ExtMtlBurCost != 0) ? PartTran.ExtMtlBurCost : Math.Round((PartTran.TranQty * PartTran.MtlBurUnitCost), ndec, MidpointRounding.AwayFromZero);
                /* Compute the rounding error */
                adjRoundingError = PartTran.ExtCost - /* Total - Sum of the parts */
                                  (amtMaterialMtlCost + amtMaterialLabCost + amtMaterialBurCost + amtMaterialSubCost + amtMtlBurCost);
                /* SCR #59049 - Adjust the highest component cost with the rounding difference instead of just   *
                 * adding it to the material cost. This is to avoid negative cost in case material cost is zero. *
                 * It's important to keep this order (Mtl/Lbr/Bur/Sub) to check where to add the rounding error. */
                if (adjRoundingError != 0)
                {
                    absMaxCost = Math.Max(Math.Max(Math.Max(Math.Abs(amtMaterialMtlCost), Math.Abs(amtMaterialLabCost)), Math.Abs(amtMaterialBurCost)), Math.Abs(amtMaterialSubCost));
                    if (Math.Abs(amtMaterialMtlCost) == absMaxCost)
                    {
                        amtMaterialMtlCost = amtMaterialMtlCost + adjRoundingError;
                    }
                    else if (Math.Abs(amtMaterialLabCost) == absMaxCost)
                    {
                        amtMaterialLabCost = amtMaterialLabCost + adjRoundingError;
                    }
                    else if (Math.Abs(amtMaterialBurCost) == absMaxCost)
                    {
                        amtMaterialBurCost = amtMaterialBurCost + adjRoundingError;
                    }
                    else
                    {
                        amtMaterialSubCost = amtMaterialSubCost + adjRoundingError;
                    }
                }
                JobMtl.MaterialMtlCost = Math.Max(0, (JobMtl.MaterialMtlCost + amtMaterialMtlCost));
                JobMtl.MaterialLabCost = Math.Max(0, (JobMtl.MaterialLabCost + amtMaterialLabCost));
                JobMtl.MaterialBurCost = Math.Max(0, (JobMtl.MaterialBurCost + amtMaterialBurCost));
                JobMtl.MaterialSubCost = Math.Max(0, (JobMtl.MaterialSubCost + amtMaterialSubCost));
                JobMtl.MtlBurCost = Math.Max(0, (JobMtl.MtlBurCost + amtMtlBurCost));
                JobMtl.TotalCost = JobMtl.MaterialMtlCost + JobMtl.MaterialLabCost + JobMtl.MaterialBurCost + JobMtl.MaterialSubCost;
            }

            this.updatePartWIP("TO", ttIssueReturn);
        }

        private void UpdateLegalNumHistory(string legalNumber, string foreignKey)
        {
            using (TransactionScope tranScope = ErpContext.CreateDefaultTransactionScope())
            {
                var LegalNumberHistory = FindFirstLegalNumHistoryWithUpdLock(Session.CompanyID, legalNumber);
                if (LegalNumberHistory != null)
                {
                    LegalNumberHistory.RelatedToFile = "PartTran";
                    LegalNumberHistory.ForeignKey = foreignKey;

                    Db.Validate(LegalNumberHistory);
                }

                tranScope.Complete();
            }
        }

        ///<summary>
        ///  Purpose: Movement for an allocated Transfer Order (STK-PLT)
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: This routine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_STK_PLT(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            // If this is a transfer order material queue pick transaction from a static PCID, 
            // set ErpCallContext to control PartAlloc updates
            if ((ttIssueReturn.ProcessID.Equals("MaterialQueue", StringComparison.OrdinalIgnoreCase) || ttIssueReturn.ProcessID.Equals("HHMaterialQueue", StringComparison.OrdinalIgnoreCase))
                && (!String.IsNullOrEmpty(ttIssueReturn.TFOrdNum)) && ttIssueReturn.TFOrdLine > 0
                && ((!String.IsNullOrEmpty(ttIssueReturn.FromPCID)) && ExistsPkgControlHeaderByPkgControlType(ttIssueReturn.Company, ttIssueReturn.FromPCID, "STATIC")))
            {
                Erp.Internal.Lib.ErpCallContext.Add("IsMaterialQueueTFPicking-StaticPCID");
            }
            try
            {
                this.process_PUR_STK(ttIssueReturn, out _);
                this.updatePartAllocTF(ttIssueReturn);
            }
            finally
            {
                if (Erp.Internal.Lib.ErpCallContext.ContainsKey("IsMaterialQueueTFPicking-StaticPCID"))
                {
                    Erp.Internal.Lib.ErpCallContext.RemoveValue("IsMaterialQueueTFPicking-StaticPCID");
                }
            }

        }

        ///<summary>
        ///  Purpose: Perform the movement of Pick Stock for Shipment  (STK-SHP).
        ///           This is basically a transfer transaction with a bit more.
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///           3. Update the PartAlloc to reflect the change in PickingQty, PickedQty
        ///           4. Update the PickedOrders table.
        ///
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_STK_SHP(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            DateTime? curDate = null;
            int curTime = 0;
            bool PartTranError = false;
            bool vCreateFIFO = false;
            string vToPlant = string.Empty;
            string vToCostID = string.Empty;
            decimal qtyRemovedFromPickedOrders = decimal.Zero;
            decimal vNotAllocatedQty = decimal.Zero;
            Erp.Tables.PartTran From_PartTran = null;
            Erp.Tables.WhseBin fWhseBin = null;
            Erp.Tables.WhseBin tWhseBin = null;

            outMessage = string.Empty;

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part == null)
            {
                throw new BLException(Strings.AValidPartNumberIsRequired, "IssueReturn", "PartNum");
            }

            fWhseBin = this.FindFirstWhseBin(ttIssueReturn.Company, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum);

            tWhseBin = this.FindFirstWhseBin(ttIssueReturn.Company, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum);

            if (fWhseBin != null && tWhseBin != null)
            {
                if (StringExtensions.Compare(fWhseBin.BinType, "Supp") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Std") == 0 && ttIssueReturn.TranQty > 0)
                {
                    LibGenSMIReceipt.GenerateSMIReceipt(ttIssueReturn.TranQty, ttIssueReturn.UM, fWhseBin.VendorNum, ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, "STK-STK", ttIssueReturn.Company, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, null);
                }
            }

            /* CREATE PARTTRAN TO REFLECT THE "FROM" MOVEMENT */
            this.createPartTran(ttIssueReturn);

            PartTran.Plant = getPlantFromWarehouse(ttIssueReturn.FromWarehouseCode);

            vToPlant = getPlantFromWarehouse(ttIssueReturn.ToWarehouseCode);

            PartTran.TranClass = "A";
            PartTran.InventoryTrans = true;
            PartTran.TranType = "STK-STK";
            PartTran.TranQty = -(PartTran.TranQty);         /* Flip sign - makes it a negative */
            PartTran.ActTranQty = -(PartTran.ActTranQty);   /* Flip sign - makes it a negative */
            PartTran.WareHouseCode = ttIssueReturn.FromWarehouseCode;
            PartTran.BinNum = ttIssueReturn.FromBinNum;
            PartTran.PCID = ttIssueReturn.FromPCID;
            PartTran.WareHouse2 = ttIssueReturn.ToWarehouseCode;
            PartTran.BinNum2 = ttIssueReturn.ToBinNum;
            PartTran.PCID2 = ttIssueReturn.ToPCID;
            PartTran.OrderNum = ttIssueReturn.OrderNum;
            PartTran.OrderLine = ttIssueReturn.OrderLine;
            PartTran.OrderRelNum = ttIssueReturn.OrderRel;
            PartTran.BinType = (fWhseBin != null) ? fWhseBin.BinType : "";

            partTranPK = partTranPK + ((partTranPK.Length > 0) ? "|" : "");
            partTranPK = partTranPK + Compatibility.Convert.ToString(PartTran.SysDate) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.SysTime) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.TranNum);

            if (!Part.TrackInventoryByRevision)
            {
                /*run getPartRev.*/
                PartRev = this.FindLastPartRev(Session.CompanyID, ttIssueReturn.PartNum, CompanyTime.Today());
                PartTran.RevisionNum = (PartRev != null) ? PartRev.RevisionNum : "";
            }
            /* Get Cost method for the PartNum */
            PartTran.CostMethod = this.getPartCostMethod(PartTran.Plant, PartTran.PartNum);
            vCreateFIFO = false;

            /* if different GL divisions then will have to make a GL transaction */
            /* PartTran.GLDiv <> PartTran.GL2Div */
            if ((StringExtensions.Compare(fWhseBin.BinType, "Supp") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Std") == 0) ||
                (StringExtensions.Compare(fWhseBin.BinType, "Std") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Std") == 0))
            {
                var outMtlUnitCost17 = PartTran.MtlUnitCost;
                var outLbrUnitCost17 = PartTran.LbrUnitCost;
                var outBurUnitCost17 = PartTran.BurUnitCost;
                var outSubUnitCost17 = PartTran.SubUnitCost;
                var outMtlBurUnitCost17 = PartTran.MtlBurUnitCost;
                this.LibInvCosts.getInvCost(ttIssueReturn.LotNum, ((!String.IsNullOrEmpty(PartTran.Plant)) ? PartTran.Plant : Session.PlantID), PartTran.PartNum, PartTran.CostMethod, out outMtlUnitCost17, out outLbrUnitCost17, out outBurUnitCost17, out outSubUnitCost17, out outMtlBurUnitCost17, ref Part);
                PartTran.MtlUnitCost = outMtlUnitCost17;
                PartTran.LbrUnitCost = outLbrUnitCost17;
                PartTran.BurUnitCost = outBurUnitCost17;
                PartTran.SubUnitCost = outSubUnitCost17;
                PartTran.MtlBurUnitCost = outMtlBurUnitCost17;
                PartTran.GLTrans = true;
                /* SCR #40698 - When transferring Stocks between two different divisions, *
                 * consume the FIFO qty from the first plant costid.                      */
                var outCostID8 = PartTran.CostID;
                this.LibGetPlantCostID._getPlantCostID(PartTran.Plant, out outCostID8, ref Plant, ref XaSyst);
                PartTran.CostID = outCostID8;        /* SCR #51749 - Create FIFO layers for non-FIFO cost methods if enabled */
                this.LibGetCostFIFOLayers.getPartCostFIFOLayers(PartTran.CostID, PartTran.PartNum, PartTran.Plant, out vEnableFIFOLayers);

                /* SCR #80528 - Although the transaction involved is STK-STK, we still have to check *
                 * if the "To" Warehouse/Plant belongs to a different CostID. If From and To CostIDs *
                 * are not the same then we have to consume the FIFO cost/qty from the "From" CostID *
                 * and recreate the new FIFO cost/qty in the "To" CostID.                            */

                this.LibGetPlantCostID._getPlantCostID(vToPlant, out vToCostID, ref Plant, ref XaSyst);

                if (StringExtensions.Compare(PartTran.CostID, vToCostID) != 0)
                {
                    /* SCR #80528 - When transferring Stocks between two different CostIDs, *
                     * consume the FIFO qty from the first plant costid.                    */
                    /* lookup(PartTran.CostMethod,"F,O") */
                    /* SCR #51749 - Apply same FIFO logic for non-FIFO parts if FIFO Layers enabled. */
                    if (StringExtensions.Lookup("F,O", PartTran.CostMethod) > -1)
                    {
                        PartTran.FIFOAction = "D"; /* Decrease */
                        var outFIFODate23 = PartTran.FIFODate;
                        var outFIFOSeq27 = PartTran.FIFOSeq;
                        var outFIFOSubSeq21 = PartTran.FIFOSubSeq;
                        var outMtlUnitCost19 = PartTran.MtlUnitCost;
                        var outLbrUnitCost19 = PartTran.LbrUnitCost;
                        var outBurUnitCost19 = PartTran.BurUnitCost;
                        var outSubUnitCost19 = PartTran.SubUnitCost;
                        var outMtlBurUnitCost19 = PartTran.MtlBurUnitCost;
                        /* SCR 176758 - Call ConsumePartFIFOCost with vUpdPartTran option to update the PartTran's Extended Subcomponent Costs */
                        LibProcessFIFO.ConsumePartFIFOCost(true, out outFIFODate23, out outFIFOSeq27, out outFIFOSubSeq21, out outMtlUnitCost19, out outLbrUnitCost19, out outBurUnitCost19, out outSubUnitCost19, out outMtlBurUnitCost19, ref PartTran, true);
                        PartTran.FIFODate = (DateTime)outFIFODate23;
                        PartTran.FIFOSeq = outFIFOSeq27;
                        PartTran.FIFOSubSeq = outFIFOSubSeq21;
                        PartTran.MtlUnitCost = outMtlUnitCost19;
                        PartTran.LbrUnitCost = outLbrUnitCost19;
                        PartTran.BurUnitCost = outBurUnitCost19;
                        PartTran.SubUnitCost = outSubUnitCost19;
                        PartTran.MtlBurUnitCost = outMtlBurUnitCost19;
                        ExceptionManager.AssertNoBLExceptions();
                        PartTran.MtlMtlUnitCost = PartTran.MtlUnitCost;
                        PartTran.MtlLabUnitCost = 0;
                        PartTran.MtlSubUnitCost = 0;
                        PartTran.MtlBurdenUnitCost = 0;
                        /* PartTran.ExtMtlXXXCosts are not updated in ConsumePartFIFOCost, update extended material subcomponent costs here */
                        PartTran.ExtMtlMtlCost = PartTran.ExtMtlCost;
                        PartTran.ExtMtlLabCost = 0;
                        PartTran.ExtMtlSubCost = 0;
                        PartTran.ExtMtlBurdenCost = 0;
                        vCreateFIFO = true;
                    }
                    else if (vEnableFIFOLayers == true)
                    {
                        PartTran.FIFOAction = "NFD"; /* Non-FIFO Deactivate */
                        var outFIFODate24 = PartTran.FIFODate;
                        var outFIFOSeq28 = PartTran.FIFOSeq;
                        var outFIFOSubSeq22 = PartTran.FIFOSubSeq;
                        var outAltMtlUnitCost11 = PartTran.AltMtlUnitCost;
                        var outAltLbrUnitCost11 = PartTran.AltLbrUnitCost;
                        var outAltBurUnitCost11 = PartTran.AltBurUnitCost;
                        var outAltSubUnitCost11 = PartTran.AltSubUnitCost;
                        var outAltMtlBurUnitCost11 = PartTran.AltMtlBurUnitCost;
                        this.LibProcessFIFO.ConsumeNonFIFOCost(true, out outFIFODate24, out outFIFOSeq28, out outFIFOSubSeq22, out outAltMtlUnitCost11, out outAltLbrUnitCost11, out outAltBurUnitCost11, out outAltSubUnitCost11, out outAltMtlBurUnitCost11, ref PartTran);
                        PartTran.FIFODate = (DateTime)outFIFODate24;
                        PartTran.FIFOSeq = outFIFOSeq28;
                        PartTran.FIFOSubSeq = outFIFOSubSeq22;
                        PartTran.AltMtlUnitCost = outAltMtlUnitCost11;
                        PartTran.AltLbrUnitCost = outAltLbrUnitCost11;
                        PartTran.AltBurUnitCost = outAltBurUnitCost11;
                        PartTran.AltSubUnitCost = outAltSubUnitCost11;
                        PartTran.AltMtlBurUnitCost = outAltMtlBurUnitCost11;
                        ExceptionManager.AssertNoBLExceptions();
                        PartTran.AltMtlMtlUnitCost = PartTran.AltMtlUnitCost;
                        PartTran.AltMtlLabUnitCost = 0;
                        PartTran.AltMtlSubUnitCost = 0;
                        PartTran.AltMtlBurdenUnitCost = 0;
                        vCreateFIFO = true;
                    }
                }
            }
            else
            {
                PartTran.GLTrans = false;
            }

            ExceptionManager.AssertNoBLExceptions();

            /* NOTE: IN THIS CASE THERE IS NO COST ON THIS TRANSACTION */
            /* ESTABLISH THE QUANTITY IN THE INVENTORY TRACKING UOM */
            decimal Inventory_Qty = decimal.Zero;
            this.LibAppService.UOMConv(PartTran.PartNum, PartTran.ActTranQty, PartTran.ActTransUOM, PartTran.InvtyUOM, out Inventory_Qty, false);

            /* To From and To Plant are different then TranType is STK-PLT */
            if (!PartTran.Plant.KeyEquals(vToPlant))
            {
                /* UpdatePartAlloc needs to be run before setting TranType to STK-PLT */
                LibAppService.UpdatePartAlloc(PartTran);
                PartTran.TranType = "STK-PLT";
                PartTran.Plant2 = vToPlant;

                /* Qty should be positive for STK-PLT so flip sign*/
                PartTran.ActTranQty = -PartTran.ActTranQty;
                PartTran.TranQty = -PartTran.TranQty;

                /* SCR 176758 - Flip the signs of the Extended Subcomponent Costs */
                PartTran.ExtMtlCost = -(PartTran.ExtMtlCost);
                PartTran.ExtLbrCost = -(PartTran.ExtLbrCost);
                PartTran.ExtBurCost = -(PartTran.ExtBurCost);
                PartTran.ExtSubCost = -(PartTran.ExtSubCost);
                PartTran.ExtMtlBurCost = -(PartTran.ExtMtlBurCost);
                PartTran.ExtMtlMtlCost = -(PartTran.ExtMtlMtlCost);
                PartTran.ExtMtlLabCost = -(PartTran.ExtMtlLabCost);
                PartTran.ExtMtlBurdenCost = -(PartTran.ExtMtlBurdenCost);
                PartTran.ExtMtlSubCost = -(PartTran.ExtMtlSubCost);

                IMIMPlant.CreatePlantTran("I", Part.ProdCode, PartTran.TranQty, 0, 0, ref PartTran, ref PlantTran);

                if (PlantTran != null)
                {
                    PlantTran.RecTranDate = ttIssueReturn.TranDate;
                    IMIMPlant.GeneratePlantReceiptTran(PlantTran.SysRowID, "", ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, PlantTran.JobNum, PlantTran.AssemblySeq, PlantTran.JobMtl, PartTran.LegalNumber, PartTran.TranDocTypeID, PlantTran.PackNum, string.Empty, PartTran);
                }
            }

            /* UPDATE PARTBIN TO REFLECT THE "FROM" MOVEMENT */
            PartPlant = this.LibNonQtyBearingBin._NonQtyBearingBin(PartTran.WareHouseCode, PartTran.PartNum);
            if (PartPlant != null && PartPlant.QtyBearing)
            {
                /* Removing stock from From Location so always pass positive quantity */
                LibDeferredUpdate.PostPBOnHand(PartTran.Company, PartTran.PartNum, PartTran.WareHouseCode, PartTran.BinNum, PartTran.LotNum, PartTran.AttributeSetID, PartTran.InvtyUOM, Math.Abs(Inventory_Qty), string.Empty, PartTran.PCID);
            }

            /* CREATE PARTTRAN TO REFLECT THE "TO" MOVEMENT */
            if (!PartTran.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))   /* "To" movement PartTran is created by GeneratePlantReceiptTran() for STK-PLT so skip this */
            {
                Db.Validate(PartTran);
                UpdateLegalNumHistory(PartTran.LegalNumber, partTranPK);

                From_PartTran = this.FindFirstPartTran(PartTran.SysRowID);
                Db.Release(ref PartTran);
                curDate = From_PartTran.SysDate;
                curTime = From_PartTran.SysTime;
                this.LibCreatePartTran.Create_PartTran(ref PartTran, Convert.ToDateTime(curDate), curTime, out PartTranError);
                if (PartTranError == true)
                {
                    throw new BLException(Strings.ErrorCreatingPartTran, "PartTran");
                }
                BufferCopy.CopyExceptFor(From_PartTran, PartTran, PartTran.ColumnNames.SysDate, PartTran.ColumnNames.SysTime, PartTran.ColumnNames.TranNum);
                PartTran.TranQty = -(From_PartTran.TranQty); /* Flip sign - puts it back to positive */
                PartTran.ActTranQty = -(From_PartTran.ActTranQty); /* Flip sign - puts it back to positive */
                PartTran.WareHouse2 = ttIssueReturn.FromWarehouseCode;
                PartTran.BinNum2 = ttIssueReturn.FromBinNum;
                PartTran.PCID2 = ttIssueReturn.FromPCID;
                PartTran.WareHouseCode = ttIssueReturn.ToWarehouseCode;
                PartTran.BinNum = ttIssueReturn.ToBinNum;
                PartTran.PCID = ttIssueReturn.ToPCID;
                PartTran.GLTrans = false;

                /* SCR 176758 - Flip the signs of the Extended Subcomponent Costs - puts it back to positive */
                PartTran.ExtMtlCost = -(PartTran.ExtMtlCost);
                PartTran.ExtLbrCost = -(PartTran.ExtLbrCost);
                PartTran.ExtBurCost = -(PartTran.ExtBurCost);
                PartTran.ExtSubCost = -(PartTran.ExtSubCost);
                PartTran.ExtMtlBurCost = -(PartTran.ExtMtlBurCost);
                PartTran.ExtMtlMtlCost = -(PartTran.ExtMtlMtlCost);
                PartTran.ExtMtlLabCost = -(PartTran.ExtMtlLabCost);
                PartTran.ExtMtlBurdenCost = -(PartTran.ExtMtlBurdenCost);
                PartTran.ExtMtlSubCost = -(PartTran.ExtMtlSubCost);

                partTranPK = partTranPK + ((partTranPK.Length > 0) ? "|" : "");
                partTranPK = partTranPK + Compatibility.Convert.ToString(PartTran.SysDate) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.SysTime) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.TranNum);

                /* SCR #40698 - Find the Plant/Cost for the "To" Warehouse */
                PartTran.Plant = vToPlant;

                this.LibDefPartTran._DefPartTran(ref PartTran, ref Part, ref PartPlant, Session.CompanyID, PartTran.PartNum, PartTran.Plant, PartTranError);
                var outCostID9 = PartTran.CostID;
                this.LibGetPlantCostID._getPlantCostID(PartTran.Plant, out outCostID9, ref Plant, ref XaSyst);
                PartTran.CostID = outCostID9;    /* SCR #51749 - Create FIFO layers for non-FIFO cost methods if enabled */
                this.LibGetCostFIFOLayers.getPartCostFIFOLayers(PartTran.CostID, PartTran.PartNum, PartTran.Plant, out vEnableFIFOLayers);

                /* SCR #40698 - When transferring Stocks between two different divisions, *
                 * create new FIFO qty for the destination plant costid.                  */
                if (vCreateFIFO == true)
                { /* lookup(PartTran.CostMethod,"F,O") */
                    if (StringExtensions.Lookup("F,O", PartTran.CostMethod) > -1)
                    {
                        PartTran.FIFODate = PartTran.TranDate;
                        PartTran.FIFOAction = "A"; /* Add */
                        /* Run the FIFO logic that adds FIFO qty for the new costid.  Create the supporting *
                         * PartFIFOTran records as details of the PartTran record if FIFO costed.           *
                         * CreatePartFIFOCost procedure can be found in lib/ProcessFIFO.i                   */
                        var outFIFOSeq29 = PartTran.FIFOSeq;
                        var outFIFOSubSeq23 = PartTran.FIFOSubSeq;
                        this.LibProcessFIFO.CreatePartFIFOCost(true, true, PartTran.FIFODate, 0, 0, out outFIFOSeq29, out outFIFOSubSeq23, ref PartTran);
                        PartTran.FIFOSeq = outFIFOSeq29;
                        PartTran.FIFOSubSeq = outFIFOSubSeq23;
                    }
                    else if (vEnableFIFOLayers == true)
                    {
                        PartTran.FIFODate = PartTran.TranDate;
                        PartTran.FIFOAction = "NFA"; /* Non-FIFO Add */
                        /* Run the FIFO logic that adds FIFO qty for the new costid.  Create the supporting *
                         * PartFIFOTran records as details of the PartTran record if FIFO costed.           *
                         * CreateNonFIFOCost procedure can be found in lib/ProcessFIFO.i                    */
                        var outFIFOSeq30 = PartTran.FIFOSeq;
                        var outFIFOSubSeq24 = PartTran.FIFOSubSeq;
                        this.LibProcessFIFO.CreateNonFIFOCost(true, PartTran.FIFODate, 0, 0, PartTran.AltMtlUnitCost, PartTran.AltLbrUnitCost, PartTran.AltBurUnitCost, PartTran.AltSubUnitCost, PartTran.AltMtlBurUnitCost, PartTran.AltMtlUnitCost, 0, 0, 0, out outFIFOSeq30, out outFIFOSubSeq24, ref PartTran);
                        PartTran.FIFOSeq = outFIFOSeq30;
                        PartTran.FIFOSubSeq = outFIFOSubSeq24;
                    }
                } /* vCreateFIFO = yes */

                /* UPDATE PARTBIN TO REFLECT THE "TO" MOVEMENT */
                PartPlant = this.LibNonQtyBearingBin._NonQtyBearingBin(PartTran.WareHouseCode, PartTran.PartNum);
                if (PartPlant != null && PartPlant.QtyBearing)
                {
                    /* Adding stock to To Location so always pass negative quantity */
                    LibDeferredUpdate.PostPBOnHand(PartTran.Company, PartTran.PartNum, PartTran.WareHouseCode, PartTran.BinNum, PartTran.LotNum, PartTran.AttributeSetID, PartTran.InvtyUOM, -(Math.Abs(Inventory_Qty)), string.Empty, PartTran.PCID);
                }
            }

            // fromPCID can get deleted during PartTran validate before allocations updates
            // save the PCID type to set ErpCallContext for allocations updates.
            bool fromPCIDIsDynamic = (!string.IsNullOrEmpty(ttIssueReturn.FromPCID)) && ExistsPkgControlHeaderByPkgControlType(ttIssueReturn.Company, ttIssueReturn.FromPCID, "DYNAMIC");

            if (PartTran != null)
            {
                Db.Validate();
                UpdateLegalNumHistory(PartTran.LegalNumber, partTranPK);
                Db.Release(ref PartTran);
            }

            decimal autoPickedQty = decimal.Zero;

            /* STEP 3 - UPDATE PICKEDORDER TABLE */
            this.LibAllocations.updatePickedOrders(ttIssueReturn.DimConvFactor,
                                                   ttIssueReturn.FromJobNum,
                                                   ttIssueReturn.LotNum,
                                                   ttIssueReturn.AttributeSetID,
                                                   ttIssueReturn.ToPCID,
                                                   ttIssueReturn.OrderNum,
                                                   ttIssueReturn.OrderLine,
                                                   ttIssueReturn.OrderRel,
                                                   ttIssueReturn.PartNum,
                                                   ttIssueReturn.ToWarehouseCode,
                                                   ttIssueReturn.ToBinNum,
                                                   ttIssueReturn.TranQty,
                                                   ttIssueReturn.UM,
                                                   out qtyRemovedFromPickedOrders);

            // If this is a sales order material queue pick transaction from a static PCID, 
            // set ErpCallContext to control PartAlloc updates
            if ((ttIssueReturn.ProcessID.Equals("MaterialQueue", StringComparison.OrdinalIgnoreCase) || ttIssueReturn.ProcessID.Equals("HHMaterialQueue", StringComparison.OrdinalIgnoreCase))
                && (ttIssueReturn.TranType.KeyEquals("STK-SHP") || ttIssueReturn.TranType.KeyEquals("CMP-SHP"))
                && ttIssueReturn.OrderNum > 0 && ttIssueReturn.OrderLine > 0 && ttIssueReturn.OrderRel > 0
                && (!String.IsNullOrEmpty(ttIssueReturn.FromPCID)) && ExistsPkgControlHeaderByPkgControlType(ttIssueReturn.Company, ttIssueReturn.FromPCID, "STATIC"))
            {
                Erp.Internal.Lib.ErpCallContext.Add("IsMaterialQueueSOPicking-StaticPCID");
            }

            if ((ttIssueReturn.ProcessID.Equals("MaterialQueue", StringComparison.OrdinalIgnoreCase) || ttIssueReturn.ProcessID.Equals("HHMaterialQueue", StringComparison.OrdinalIgnoreCase))
                && (ttIssueReturn.TranType.KeyEquals("STK-SHP") || ttIssueReturn.TranType.KeyEquals("CMP-SHP"))
                && ttIssueReturn.OrderNum > 0 && ttIssueReturn.OrderLine > 0 && ttIssueReturn.OrderRel > 0
                && fromPCIDIsDynamic)
            {
                Erp.Internal.Lib.ErpCallContext.Add("IsMaterialQueueSOPicking-DynamicPCID");
            }

            try
            {
                /* STEP 4 - UPDATE PARTALLOC TABLE */
                this.LibAllocations.shipmentUpdatePartAlloc(ttIssueReturn.UM,
                                                    ttIssueReturn.FromWarehouseCode,
                                                    ttIssueReturn.FromBinNum,
                                                    ttIssueReturn.FromPCID,
                                                    ttIssueReturn.LotNum,
                                                    ttIssueReturn.AttributeSetID,
                                                    ttIssueReturn.OrderNum,
                                                    ttIssueReturn.OrderLine,
                                                    ttIssueReturn.OrderRel,
                                                    ttIssueReturn.PartNum,
                                                    ttIssueReturn.ToJobNum,
                                                    ttIssueReturn.ToWarehouseCode,
                                                    ttIssueReturn.ToBinNum,
                                                    ttIssueReturn.ToPCID,
                                                    ttIssueReturn.TranQty,
                                                    0,
                                                    0,
                                                    false,
                                                    false,
                                                     ((StringExtensions.Compare(ttIssueReturn.FromWarehouseCode, ttIssueReturn.ToWarehouseCode) == 0 && StringExtensions.Compare(ttIssueReturn.FromBinNum, ttIssueReturn.ToBinNum) == 0) ? true : false),
                                                    true,
                                                    false,
                                                    ref vNotAllocatedQty,
                                                    out autoPickedQty);
            }
            finally
            {
                if (Erp.Internal.Lib.ErpCallContext.ContainsKey("IsMaterialQueueSOPicking-StaticPCID"))
                {
                    Erp.Internal.Lib.ErpCallContext.RemoveValue("IsMaterialQueueSOPicking-StaticPCID");
                }
                if (Erp.Internal.Lib.ErpCallContext.ContainsKey("IsMaterialQueueSOPicking-DynamicPCID"))
                {
                    Erp.Internal.Lib.ErpCallContext.RemoveValue("IsMaterialQueueSOPicking-DynamicPCID");
                }
            }
        }

        ///<summary>
        ///  Purpose: Perform the movement from one stock location to another (STK-STK).
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: NOTE: This rountine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_STK_STK(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_PUR_STK(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement from one stock location to another (STK-STK).
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: NOTE: This rountine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_RAU_STK(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_PUR_STK(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement from one stock location to another (STK-STK).
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: NOTE: This rountine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_RMN_STK(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_PUR_STK(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the movement from one stock location to another (STK-STK).
        ///           1. Create two PartTran (STK-STK) records to reflect movement between locations.
        ///           2. Update the PartBin records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: NOTE: This rountine uses the Process-PUR-STK procedure.
        ///</summary>
        private void process_RMG_STK(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            this.process_PUR_STK(ttIssueReturn, out _);
        }

        ///<summary>
        ///  Purpose: Perform the update to the database for a STOCK MISCELLANEOUS ISSUE ( STK-UKN STOCK TO UNKNOWN)
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void process_STK_UKN(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            decimal curMtlUnitCost = decimal.Zero;
            decimal curLbrUnitCost = decimal.Zero;
            decimal curBurUnitCost = decimal.Zero;
            decimal curSubUnitCost = decimal.Zero;
            decimal curMtlBurUnitCost = decimal.Zero;
            bool vLotFIFOFound = false;
            DateTime? vFIFODate = null;
            int vFIFOSeq = 0;

            outMessage = string.Empty;

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part == null)
            {
                throw new BLException(Strings.AValidPartNumberIsRequired, "IssueReturn", "PartNum");
            }
            if (!this.ValidUOM(ttIssueReturn.PartNum, ttIssueReturn.UM))
            {
                throw new BLException(Strings.InvalidUOMForPartNumber, "IssueReturn", "UM");
            }

            /* check if moving from a supplier managed bin  */
            WhseBin = this.FindFirstWhseBin(ttIssueReturn.Company, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum);
            if (WhseBin != null && StringExtensions.Compare(WhseBin.BinType, "Std") != 0)
            {
                throw new BLException(Strings.CannotPerformSTKUKNTransToAManagedBin, "IssueReturn", "ToBinNum");
            }

            WhseBin = this.FindFirstWhseBin(ttIssueReturn.Company, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum);
            /* if Supplier managed bin, then auto create a PackSlip to update costs - no change to OnHandQty  */
            if (WhseBin != null && StringExtensions.Compare(WhseBin.BinType, "Supp") == 0 && ttIssueReturn.TranQty > 0)
            {
                LibGenSMIReceipt.GenerateSMIReceipt(ttIssueReturn.TranQty, ttIssueReturn.UM, WhseBin.VendorNum, ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, "STK-MTL", ttIssueReturn.Company, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, null);
            }
            this.createPartTran(ttIssueReturn);
            PartTran.TranClass = "I";
            PartTran.TranType = ttIssueReturn.TranType;
            PartTran.InventoryTrans = true;
            PartTran.WareHouseCode = ttIssueReturn.FromWarehouseCode;
            PartTran.BinNum = ttIssueReturn.FromBinNum;
            PartTran.PCID = ttIssueReturn.FromPCID;
            PartTran.BinType = ((WhseBin != null) ? WhseBin.BinType : "");
            partTranPK = partTranPK + ((partTranPK.Length > 0) ? "|" : "");
            partTranPK = partTranPK + Compatibility.Convert.ToString(PartTran.SysDate) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.SysTime) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(PartTran.TranNum);

            if (!Part.TrackInventoryByRevision)
            {
                /*run getPartRev.*/
                PartRev = this.FindLastPartRev(Session.CompanyID, ttIssueReturn.PartNum, CompanyTime.Today());
                if (PartRev != null)
                {
                    PartTran.RevisionNum = PartRev.RevisionNum;
                }
                else
                {
                    PartTran.RevisionNum = "";
                }
            }

            PartTran.Plant = getPlantFromWarehouse(PartTran.WareHouseCode);

            /* Get Cost method for the PartNum */
            PartTran.CostMethod = this.getPartCostMethod(PartTran.Plant, PartTran.PartNum);

            this.updatePartBin(PartTran);

            /* BinType <> 'Cust' */
            if (StringExtensions.Compare(WhseBin.BinType, "Cust") != 0)
            {
                var outMtlUnitCost23 = PartTran.MtlUnitCost;
                var outLbrUnitCost23 = PartTran.LbrUnitCost;
                var outBurUnitCost23 = PartTran.BurUnitCost;
                var outSubUnitCost23 = PartTran.SubUnitCost;
                var outMtlBurUnitCost23 = PartTran.MtlBurUnitCost;
                this.LibInvCosts.getInvCost(ttIssueReturn.LotNum, ((!String.IsNullOrEmpty(PartTran.Plant)) ? PartTran.Plant : Session.PlantID), PartTran.PartNum, PartTran.CostMethod, out outMtlUnitCost23, out outLbrUnitCost23, out outBurUnitCost23, out outSubUnitCost23, out outMtlBurUnitCost23, ref Part);
                PartTran.MtlUnitCost = outMtlUnitCost23;
                PartTran.LbrUnitCost = outLbrUnitCost23;
                PartTran.BurUnitCost = outBurUnitCost23;
                PartTran.SubUnitCost = outSubUnitCost23;
                PartTran.MtlBurUnitCost = outMtlBurUnitCost23;
                var outCostID11 = PartTran.CostID;
                this.LibGetPlantCostID._getPlantCostID(PartTran.Plant, out outCostID11, ref Plant, ref XaSyst);
                PartTran.CostID = outCostID11;

                /* SCR #51749 - Create FIFO layers for non-FIFO cost methods if enabled */
                this.LibGetCostFIFOLayers.getPartCostFIFOLayers(PartTran.CostID, PartTran.PartNum, PartTran.Plant, out vEnableFIFOLayers);

                /* SCR #40698 - If the part is FIFO Costed then consume the FIFO qty/cost. */
                /* PartTran.CostMethod = "F" (FIFO) or "O" (LOT FIFO) */
                /* SCR #51749 - If non-FIFO Costed but FIFO Layers enabled then consume the FIFO qty/cost. */
                if (StringExtensions.Lookup("F,O", PartTran.CostMethod) > -1)
                { /* PartTran.TranQty > 0 */
                    if (PartTran.TranQty > 0)
                    {
                        PartTran.FIFOAction = "D";
                        var outFIFODate31 = PartTran.FIFODate;
                        var outFIFOSeq43 = PartTran.FIFOSeq;
                        var outFIFOSubSeq37 = PartTran.FIFOSubSeq;
                        var outMtlUnitCost25 = PartTran.MtlUnitCost;
                        var outLbrUnitCost25 = PartTran.LbrUnitCost;
                        var outBurUnitCost25 = PartTran.BurUnitCost;
                        var outSubUnitCost25 = PartTran.SubUnitCost;
                        var outMtlBurUnitCost25 = PartTran.MtlBurUnitCost;
                        /* SCR 176758 - Call ConsumePartFIFOCost with vUpdPartTran option to update the PartTran's Extended Subcomponent Costs */
                        LibProcessFIFO.ConsumePartFIFOCost(true, out outFIFODate31, out outFIFOSeq43, out outFIFOSubSeq37, out outMtlUnitCost25, out outLbrUnitCost25, out outBurUnitCost25, out outSubUnitCost25, out outMtlBurUnitCost25, ref PartTran, true);
                        PartTran.FIFODate = (DateTime)outFIFODate31;
                        PartTran.FIFOSeq = outFIFOSeq43;
                        PartTran.FIFOSubSeq = outFIFOSubSeq37;
                        PartTran.MtlUnitCost = outMtlUnitCost25;
                        PartTran.LbrUnitCost = outLbrUnitCost25;
                        PartTran.BurUnitCost = outBurUnitCost25;
                        PartTran.SubUnitCost = outSubUnitCost25;
                        PartTran.MtlBurUnitCost = outMtlBurUnitCost25;
                        ExceptionManager.AssertNoBLExceptions();
                        PartTran.MtlMtlUnitCost = PartTran.MtlUnitCost;
                        PartTran.MtlLabUnitCost = 0;
                        PartTran.MtlSubUnitCost = 0;
                        PartTran.MtlBurdenUnitCost = 0;
                        /* PartTran.ExtMtlXXXCosts are not updated in ConsumePartFIFOCost, update extended material subcomponent costs here */
                        PartTran.ExtMtlMtlCost = PartTran.ExtMtlCost;
                        PartTran.ExtMtlLabCost = 0;
                        PartTran.ExtMtlSubCost = 0;
                        PartTran.ExtMtlBurdenCost = 0;
                    }
                    else
                    {
                        vLotFIFOFound = false;
                        /* SCR #51621 - We use a slightly different logic if returning a LOTFIFO costed *
                       * part back to stock.  Check if there is active FIFO Cost for the given LotNum *
                       * and use its costs when you return the FIFO part back to the same lot.  If no *
                       * active FIFO cost available then use the regular logic of finding the average *
                       * costs of all transactions related to the part.                               */
                        if (StringExtensions.Compare(PartTran.CostMethod, "O") == 0)
                        {
                            this.getLotFIFOCosts(ttIssueReturn, PartTran.CostID, out curMtlUnitCost, out curLbrUnitCost, out curBurUnitCost, out curSubUnitCost, out curMtlBurUnitCost, out vFIFODate, out vFIFOSeq, out vLotFIFOFound);
                        } /* PartTran.CostMethod = "O" */
                        if (vLotFIFOFound == false)
                        {
                            this.getMiscReturnUnitCosts(ttIssueReturn, PartTran.SysRowID, PartTran.CostID, out curMtlUnitCost, out curLbrUnitCost, out curBurUnitCost, out curSubUnitCost, out curMtlBurUnitCost);
                        }
                        if (curMtlUnitCost != 0 || curLbrUnitCost != 0 ||
                            curBurUnitCost != 0 || curSubUnitCost != 0 ||
                            curMtlBurUnitCost != 0)
                        {
                            PartTran.MtlUnitCost = curMtlUnitCost;
                            PartTran.LbrUnitCost = curLbrUnitCost;
                            PartTran.BurUnitCost = curBurUnitCost;
                            PartTran.SubUnitCost = curSubUnitCost;
                            PartTran.MtlBurUnitCost = curMtlBurUnitCost;
                            PartTran.MtlMtlUnitCost = PartTran.MtlUnitCost;
                            PartTran.MtlLabUnitCost = 0;
                            PartTran.MtlSubUnitCost = 0;
                            PartTran.MtlBurdenUnitCost = 0;
                        } /* getMiscReturnUnitCosts <> 0 */

                        /* The GetInvCosts procedure above defaulted the average or lot average costs even *
                         * for FIFO parts.  If the average costs are zeroes then use last costs of part.   */
                        if (PartTran.MtlUnitCost == 0 && PartTran.LbrUnitCost == 0 &&
                            PartTran.BurUnitCost == 0 && PartTran.SubUnitCost == 0 &&
                            PartTran.MtlBurUnitCost == 0)
                        {
                            PartCost = this.FindFirstPartCost(PartTran.Company, PartTran.PartNum, PartTran.CostID);
                            if (PartCost != null)
                            {
                                PartTran.MtlUnitCost = PartCost.LastMaterialCost;
                                PartTran.LbrUnitCost = PartCost.LastLaborCost;
                                PartTran.BurUnitCost = PartCost.LastBurdenCost;
                                PartTran.SubUnitCost = PartCost.LastSubContCost;
                                PartTran.MtlBurUnitCost = PartCost.LastMtlBurCost;
                            }
                        } /* Unit Costs = 0 */
                        PartTran.FIFOAction = "A";
                        PartTran.FIFODate = PartTran.TranDate;              /* CreatePartFIFOCost procedure can be found in lib/ProcessFIFO.i */
                        var outFIFOSeq44 = PartTran.FIFOSeq;
                        var outFIFOSubSeq38 = PartTran.FIFOSubSeq;
                        this.LibProcessFIFO.CreatePartFIFOCost(true, true, PartTran.FIFODate, 0, 0, out outFIFOSeq44, out outFIFOSubSeq38, ref PartTran);
                        PartTran.FIFOSeq = outFIFOSeq44;
                        PartTran.FIFOSubSeq = outFIFOSubSeq38;
                    }
                }
                else if (vEnableFIFOLayers == true)
                { /* PartTran.TranQty > 0 */
                    if (PartTran.TranQty > 0)
                    {
                        PartTran.FIFOAction = "NFD"; /* Non-FIFO Decrease */
                        var outFIFODate32 = PartTran.FIFODate;
                        var outFIFOSeq45 = PartTran.FIFOSeq;
                        var outFIFOSubSeq39 = PartTran.FIFOSubSeq;
                        var outAltMtlUnitCost15 = PartTran.AltMtlUnitCost;
                        var outAltLbrUnitCost15 = PartTran.AltLbrUnitCost;
                        var outAltBurUnitCost15 = PartTran.AltBurUnitCost;
                        var outAltSubUnitCost15 = PartTran.AltSubUnitCost;
                        var outAltMtlBurUnitCost15 = PartTran.AltMtlBurUnitCost;
                        this.LibProcessFIFO.ConsumeNonFIFOCost(true, out outFIFODate32, out outFIFOSeq45, out outFIFOSubSeq39, out outAltMtlUnitCost15, out outAltLbrUnitCost15, out outAltBurUnitCost15, out outAltSubUnitCost15, out outAltMtlBurUnitCost15, ref PartTran);
                        PartTran.FIFODate = (DateTime)outFIFODate32;
                        PartTran.FIFOSeq = outFIFOSeq45;
                        PartTran.FIFOSubSeq = outFIFOSubSeq39;
                        PartTran.AltMtlUnitCost = outAltMtlUnitCost15;
                        PartTran.AltLbrUnitCost = outAltLbrUnitCost15;
                        PartTran.AltBurUnitCost = outAltBurUnitCost15;
                        PartTran.AltSubUnitCost = outAltSubUnitCost15;
                        PartTran.AltMtlBurUnitCost = outAltMtlBurUnitCost15;
                        ExceptionManager.AssertNoBLExceptions();
                        PartTran.AltMtlMtlUnitCost = PartTran.AltMtlUnitCost;
                        PartTran.AltMtlLabUnitCost = 0;
                        PartTran.AltMtlSubUnitCost = 0;
                        PartTran.AltMtlBurdenUnitCost = 0;
                    }
                    else
                    {
                        this.getMiscReturnAltUnitCosts(ttIssueReturn, PartTran.SysRowID, PartTran.CostID, out curMtlUnitCost, out curLbrUnitCost, out curBurUnitCost, out curSubUnitCost, out curMtlBurUnitCost);
                        /* If the costs from getMiscReturnAltUnitCosts are zeroes then use *
                        * average costs from GetInvCosts.                                 */
                        if (curMtlUnitCost != 0 || curLbrUnitCost != 0 ||
                            curBurUnitCost != 0 || curSubUnitCost != 0 ||
                            curMtlBurUnitCost != 0)
                        {
                            PartTran.AltMtlUnitCost = curMtlUnitCost;
                            PartTran.AltLbrUnitCost = curLbrUnitCost;
                            PartTran.AltBurUnitCost = curBurUnitCost;
                            PartTran.AltSubUnitCost = curSubUnitCost;
                            PartTran.AltMtlBurUnitCost = curMtlBurUnitCost;
                            PartTran.AltMtlMtlUnitCost = PartTran.AltMtlUnitCost;
                            PartTran.AltMtlLabUnitCost = 0;
                            PartTran.AltMtlSubUnitCost = 0;
                            PartTran.AltMtlBurdenUnitCost = 0;
                        } /* getMiscReturnUnitCosts <> 0 */
                        /* The GetInvCosts procedure above defaulted the average or lot average costs even *
                         * for FIFO parts.  If the average costs are zeroes then use last costs of part.   */
                        if (PartTran.AltMtlUnitCost == 0 && PartTran.AltLbrUnitCost == 0 &&
                            PartTran.AltBurUnitCost == 0 && PartTran.AltSubUnitCost == 0 &&
                            PartTran.AltMtlBurUnitCost == 0)
                        {
                            PartCost = this.FindFirstPartCost(PartTran.Company, PartTran.PartNum, PartTran.CostID);
                            if (PartCost != null)
                            {
                                PartTran.AltMtlUnitCost = PartCost.LastMaterialCost;
                                PartTran.AltLbrUnitCost = PartCost.LastLaborCost;
                                PartTran.AltBurUnitCost = PartCost.LastBurdenCost;
                                PartTran.AltSubUnitCost = PartCost.LastSubContCost;
                                PartTran.AltMtlBurUnitCost = PartCost.LastMtlBurCost;
                            }
                        } /* Unit Costs = 0 */
                        PartTran.FIFOAction = "NFA";  /* Non-FIFO Add */
                        PartTran.FIFODate = PartTran.TranDate;              /* CreateNonFIFOCost procedure can be found in lib/ProcessFIFO.i */
                        var outFIFOSeq46 = PartTran.FIFOSeq;
                        var outFIFOSubSeq40 = PartTran.FIFOSubSeq;
                        this.LibProcessFIFO.CreateNonFIFOCost(true, PartTran.FIFODate, 0, 0, PartTran.AltMtlUnitCost, PartTran.AltLbrUnitCost, PartTran.AltBurUnitCost, PartTran.AltSubUnitCost, PartTran.AltMtlBurUnitCost, PartTran.AltMtlUnitCost, 0, 0, 0, out outFIFOSeq46, out outFIFOSubSeq40, ref PartTran);
                        PartTran.FIFOSeq = outFIFOSeq46;
                        PartTran.FIFOSubSeq = outFIFOSubSeq40;
                    }
                }
            }
            else
            {
                PartTran.GLTrans = false;
            }

            UpdateLegalNumHistory(PartTran.LegalNumber, partTranPK);

            ExceptionManager.AssertNoBLExceptions();
        }

        ///<summary>
        ///  Purpose: Perform the movement of subcontract non-conformance from production area to inspection area (SUB-INS).
        ///           1. This function simply updates the Warehouse/Bin location on the NonConf.
        ///
        ///  Parameters:  none
        ///  Notes: A NonConf pending inspection can only exist in one warehouse/bin.
        ///
        ///
        ///Since we update the nonconf record directly we really can't just change the WarehouseCode/BinNum since they
        ///represent where the nonconformance was found at.
        ///Therefore we consider the ToWarehouse/ToBinNum as the current location of the NonConformance.
        ///</summary>
        private void process_SUB_INS(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            using (Db.DisableTriggerScope(nameof(Erp.Tables.NonConf), TriggerType.Write))
            {
                MtlQueue = this.FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
                if (MtlQueue == null)
                {
                    return;
                }

                NonConf = this.FindFirstNonConfWithUpdLock(MtlQueue.Company, MtlQueue.NCTranID);
                if (NonConf != null)
                {
                    NonConf.ToWarehouseCode = ttIssueReturn.ToWarehouseCode;
                    NonConf.ToBinNum = ttIssueReturn.ToBinNum;
                    Db.Validate(NonConf);
                }
            }
        }

        ///<summary>
        ///  Purpose: Perform the update to the database for a STOCK MISCELLANEOUS return ( UKN-STK UNKNOWN TO STOCK)
        ///  Parameters:  none
        ///  Notes: This actually results in a negative STK-UKN transaction.
        ///         Most of the processing is done using the Process-Stk-Ukn procedure.
        ///</summary>
        private void process_UKN_STK(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part == null)
            {
                throw new BLException(Strings.AValidPartNumberIsRequired, "IssueReturn", "PartNum");
            }
            ttIssueReturn.TranQty = -(ttIssueReturn.TranQty); /* make the quantity negative */
            ttIssueReturn.TranType = "STK-UKN";            /* Change the trans type */
            ttIssueReturn.FromWarehouseCode = ttIssueReturn.ToWarehouseCode; /* Copy TO Whse/Bin to From Whse/Bin. */
            ttIssueReturn.FromWarehouseCodeDescription = ttIssueReturn.ToWarehouseCodeDescription;
            ttIssueReturn.FromBinNum = ttIssueReturn.ToBinNum;       /* then we can use the Process-Stk-Ukn procedure */
            ttIssueReturn.FromBinNumDescription = ttIssueReturn.ToBinNumDescription;
            this.process_STK_UKN(ttIssueReturn, out _);
            ttIssueReturn.TranType = "UKN-STK";
        }

        ///<summary>
        ///  Purpose: Perform the movement of wip quantity from one whse/bin to another whse/bin.
        ///           1. Set the "TO" job equal to "From" since a WIP-WIP is only a movement of
        ///              wip between Whse/Bins, not asm/operations.
        ///              Note: as of 9.05.601v5 we can now move from on operation seq to another on the same job/assembly
        ///           2. Update the PartWip records to reflect the new location of the part.
        ///
        ///  Parameters:  none
        ///  Notes: This same routine also handles MTL-MTL transactions.
        ///</summary>
        private void process_WIP_WIP(IssueReturnRow ttIssueReturn, out string outMessage)
        {
            outMessage = string.Empty;

            if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
            {
                if (!string.IsNullOrEmpty(ttIssueReturn.FromPCID) && ttIssueReturn.FromPCID.KeyEquals(ttIssueReturn.ToPCID))
                {
                    //If here, this is Move WIP PCID, logic elsewhere is looping through PkgControlStageItem and finding an associated
                    //PartWip, NonConf or DMRHead and making the change to the Plant, Warehouse and Bin there.
                    return;
                }

                /* ERPS-106804 - Special consideration for WIP-WIP to save the original source of the PartWIP (FromAssemblySeq/FromOprSeq). *
                 * The MtlQueue for WIP-WIP stores the source PartWIP.FromAssemblySeq/FromOprSeq in NextAssemblySeq/NextJobSeq and saves in *
                 * ttIssueReturn.ToAssemblySeq/ToJobSeq temporarily. Since WIP-WIP is just a change in location, make sure to use the same  *
                 * ttIssueReturn.FromAssemblySeq/FromJobSeq as your ttIssueReturn.ToAssemblySeq/ToJobSeq.                                   */
                PW_SaveFromAssemblySeq = 0;
                PW_SaveFromOprSeq = 0;
                if (ttIssueReturn.ToJobSeq != 0 && ttIssueReturn.MtlQueueRowId != Guid.Empty)
                {
                    PW_SaveFromAssemblySeq = ttIssueReturn.ToAssemblySeq;
                    PW_SaveFromOprSeq = ttIssueReturn.ToJobSeq;
                    ttIssueReturn.ToJobSeq = ttIssueReturn.FromJobSeq;
                }
                ttIssueReturn.ToJobNum = ttIssueReturn.FromJobNum;
                ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
                if (ttIssueReturn.ToJobSeq == 0)
                {
                    ttIssueReturn.ToJobSeq = ttIssueReturn.FromJobSeq;
                }

                /* Step 2 - run the Update Partwip rountines */
                using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
                {
                    libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, true, ttIssueReturn.ToPCID, true, "ADJUSTWIP");
                }

                this.updatePartWIP("From", ttIssueReturn);
                this.updatePartWIP("TO", ttIssueReturn);

                if (ttIssueReturn.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase) &&
                    ttIssueReturn.ProcessID.Equals("UNPICK", StringComparison.OrdinalIgnoreCase) &&
                    LibAppService.IsMakeDirectSupplyJobTransaction(ttIssueReturn.PartNum, ttIssueReturn.FromJobNum, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel))
                {
                    // Update PartAlloc to reflect the "From" movement
                    UpdatePartAllocWIP("WIP-WIP", ttIssueReturn);
                }
            }
            else
            {
                using (Erp.Internal.Lib.PackageControlBuildSplitMerge libPackageControlBuildSplitMerge = new Internal.Lib.PackageControlBuildSplitMerge(Db))
                {
                    libPackageControlBuildSplitMerge.ValidatePkgControlStatuses(Session.CompanyID, ttIssueReturn.FromPCID, true, ttIssueReturn.ToPCID, true, "MOVEMATERIAL");
                }

                /* ERPS-106804 - Special consideration for MFG-OPR/PUR-SUB/INS-SUB to save the original source of the PartWIP (FromAssemblySeq/FromOprSeq) */
                PW_SaveFromAssemblySeq = 0;
                PW_SaveFromOprSeq = 0;
                if (StringExtensions.Lookup("MFG-OPR,PUR-SUB,INS-SUB,DMR-ASM,DMR-SUB", ttIssueReturn.TranType) > -1 && ttIssueReturn.MtlQueueRowId != Guid.Empty)
                {
                    PW_SaveFromAssemblySeq = ttIssueReturn.FromAssemblySeq;
                    PW_SaveFromOprSeq = ttIssueReturn.FromJobSeq;
                }
                ttIssueReturn.FromJobNum = ttIssueReturn.ToJobNum;
                ttIssueReturn.FromAssemblySeq = ttIssueReturn.ToAssemblySeq;
                ttIssueReturn.FromJobSeq = ttIssueReturn.ToJobSeq;
                /* Step 2 - run the Update Partwip rountines */
                this.updatePartWIP("From", ttIssueReturn);
                /* Step 3 - Set the To = From */
                ttIssueReturn.ToJobNum = ttIssueReturn.FromJobNum;
                ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
                ttIssueReturn.ToJobSeq = ttIssueReturn.FromJobSeq;
                /* Step 4 - run the Update Partwip rountines */
                this.updatePartWIP("TO", ttIssueReturn);
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void processMtlQueue(Guid prMtlQueueRowID, IssueReturnRow ttIssueReturn)
        {
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement))
                return;

            MtlQueue = FindFirstMtlQueue(prMtlQueueRowID);
            if (MtlQueue == null)
                return;

            bool isPick = MtlQueue.TranType.Equals("STK-SHP", StringComparison.OrdinalIgnoreCase) || MtlQueue.TranType.Equals("CMP-SHP", StringComparison.OrdinalIgnoreCase);

            /* MOVE THE INFORMATION ON THE MtlQueue RECORD TO THE SCREEN FIELDS */
            ttIssueReturn.MtlQueueRowId = MtlQueue.SysRowID;
            ttIssueReturn.TranType = MtlQueue.TranType;
            ttIssueReturn.PartNum = MtlQueue.PartNum;
            ttIssueReturn.TranQty = MtlQueue.Quantity;
            ttIssueReturn.DispNumberOfPieces = MtlQueue.NumberOfPieces;
            ttIssueReturn.UM = MtlQueue.IUM;
            ttIssueReturn.AttributeSetID = MtlQueue.AttributeSetID;

            if (ttIssueReturn.AttributeSetID != 0)
            {
                string attributeSetDescription = string.Empty;
                string attributeSetShortDescription = string.Empty;
                using (Erp.Internal.Lib.AdvancedUOM LibAdvancedUOM = new Erp.Internal.Lib.AdvancedUOM(Db))
                {
                    LibAdvancedUOM.GetAttributeSetDescriptions(Session.CompanyID, ttIssueReturn.AttributeSetID, out attributeSetDescription, out attributeSetShortDescription);
                }
                ttIssueReturn.AttributeSetDescription = attributeSetDescription;
                ttIssueReturn.AttributeSetShortDescription = attributeSetShortDescription;
            }
            ttIssueReturn.RevisionNum = MtlQueue.RevisionNum;
            ttIssueReturn.ToJobNum = MtlQueue.JobNum;
            ttIssueReturn.ToAssemblySeq = MtlQueue.AssemblySeq;
            ttIssueReturn.ToJobSeq = MtlQueue.JobSeq;
            ttIssueReturn.ToWarehouseCode = MtlQueue.ToWhse;
            ttIssueReturn.ToWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, MtlQueue.ToWhse);
            ttIssueReturn.ToBinNum = MtlQueue.ToBinNum;
            ttIssueReturn.ToBinNumDescription = FindFirstWhseBinDescription(Session.CompanyID, MtlQueue.ToWhse, MtlQueue.ToBinNum);
            ttIssueReturn.ToPCID = MtlQueue.ToPCID;
            ttIssueReturn.FromWarehouseCode = MtlQueue.FromWhse;
            ttIssueReturn.FromWarehouseCodeDescription = FindFirstWarehseDescription(Session.CompanyID, MtlQueue.FromWhse);
            ttIssueReturn.FromBinNumDescription = FindFirstWhseBinDescription(Session.CompanyID, MtlQueue.FromWhse, MtlQueue.FromBinNum);
            ttIssueReturn.FromBinNum = MtlQueue.FromBinNum;
            ttIssueReturn.FromPCID = MtlQueue.FromPCID;
            ttIssueReturn.OrderNum = MtlQueue.OrderNum;
            ttIssueReturn.OrderLine = MtlQueue.OrderLine;
            ttIssueReturn.OrderRel = MtlQueue.OrderRelNum;
            ttIssueReturn.TFOrdNum = MtlQueue.TargetTFOrdNum;
            ttIssueReturn.TFOrdLine = MtlQueue.TargetTFOrdLine;
            ttIssueReturn.LotNum = MtlQueue.LotNum;
            ttIssueReturn.TranReference = TranslateString(MtlQueue.ReferencePrefix) + MtlQueue.Reference;
            ttIssueReturn.OnHandUM = MtlQueue.IUM;
            ttIssueReturn.EpicorFSA = MtlQueue.EpicorFSA;

            if (!string.IsNullOrEmpty(MtlQueue.PartNum))
            {
                Part = FindFirstPart(Session.CompanyID, MtlQueue.PartNum);
                if (Part != null)
                {
                    ttIssueReturn.TrackDimension = Part.TrackDimension;
                    ttIssueReturn.PartTrackInventoryAttributes = Part.TrackInventoryAttributes;
                    ttIssueReturn.PartTrackInventoryByRevision = Part.TrackInventoryByRevision;
                    ttIssueReturn.PartAttrClassID = Part.AttrClassID;
                    ttIssueReturn.EnableAttributeSetSearch = (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedUnitOfMeasure) && Part.TrackInventoryAttributes);
                }
            }

            /* Special consideration for MTL-MTL */
            if (StringExtensions.Lookup("MTL-MTL,MTL-STK,MTL-INS,INS-MTL,ASM-STK", MtlQueue.TranType) > -1)
            {
                ttIssueReturn.FromJobNum = MtlQueue.JobNum;
                ttIssueReturn.FromAssemblySeq = MtlQueue.AssemblySeq;
                ttIssueReturn.FromJobSeq = MtlQueue.JobSeq;
            }
            /* Special considerations for MFG-OPR  */
            if (StringExtensions.Lookup("ASM-INS,INS-ASM,INS-SUB,MFG-OPR,MFG-CUS,MFG-SHP,WIP-WIP,PUR-SUB,SUB-INS,DMR-ASM,DMR-SUB", MtlQueue.TranType) > -1)
            {
                ttIssueReturn.FromJobNum = MtlQueue.JobNum;
                ttIssueReturn.FromAssemblySeq = MtlQueue.AssemblySeq;
                ttIssueReturn.FromJobSeq = MtlQueue.JobSeq;
                ttIssueReturn.ToAssemblySeq = ((StringExtensions.Compare(MtlQueue.TranType, "WIP-WIP") == 0) ? MtlQueue.AssemblySeq : MtlQueue.NextAssemblySeq);
                ttIssueReturn.ToJobSeq = ((StringExtensions.Compare(MtlQueue.TranType, "WIP-WIP") == 0) ? MtlQueue.JobSeq : MtlQueue.NextJobSeq);
            }
            if (StringExtensions.Lookup("STK-MTL", MtlQueue.TranType) > -1 && !String.IsNullOrEmpty(MtlQueue.TargetJobNum))
            {
                ttIssueReturn.ToJobNum = MtlQueue.TargetJobNum;
                ttIssueReturn.ToAssemblySeq = MtlQueue.TargetAssemblySeq;
                ttIssueReturn.ToJobSeq = MtlQueue.TargetMtlSeq;
            }
            /* Special considerations for MFG-WIP - IMPORTANT THIS TRANSACTION IS NOT YET SUPPORTED BY THIS PROGRAM.
               CURRENTLY IT IS STILL BEING HANDLED VIA IME10-DG.W
            */
            if (StringExtensions.Compare(MtlQueue.TranType, "MFG-MTL") == 0)
            {
                ttIssueReturn.FromJobNum = MtlQueue.JobNum;
                ttIssueReturn.FromAssemblySeq = MtlQueue.AssemblySeq;
                ttIssueReturn.FromJobSeq = MtlQueue.JobSeq;
                ttIssueReturn.ToJobNum = MtlQueue.TargetJobNum;
                ttIssueReturn.ToAssemblySeq = MtlQueue.TargetAssemblySeq;
                ttIssueReturn.ToJobSeq = MtlQueue.TargetMtlSeq;
            }

            // If we are moving an entire PCID (with its contains) from one place to another
            // then the FromPCID and ToPCID should be the same PCID ( MtlQueue.PCID)
            if (MtlQueue.TranType.Equals("STK-STK", StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(MtlQueue.PCID)
                && !(MtlQueue.PCID.KeyCompare("USE-FROMPCID-TOPCID") != 0)
                && string.IsNullOrEmpty(MtlQueue.PartNum)
                && !(StringExtensions.Compare(ttIssueReturn.FromPCID, MtlQueue.PCID) != 0))
            {
                ttIssueReturn.FromPCID = MtlQueue.PCID;
            }

            if (!string.IsNullOrEmpty(ttIssueReturn.ToJobNum))
            {
                JobHead = FindFirstJobHead(Session.CompanyID, ttIssueReturn.ToJobNum);
                if (JobHead != null)
                {
                    ttIssueReturn.ToJobPartDescription = JobHead.PartDescription;
                    ttIssueReturn.ToJobPlant = JobHead.Plant;
                }

                JobAsmbl = FindFirstJobAsmbl(Session.CompanyID, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq);
                if (JobAsmbl != null)
                {
                    ttIssueReturn.ToAssemblyPartNum = JobAsmbl.PartNum;
                    ttIssueReturn.ToAssemblyPartDesc = JobAsmbl.Description;
                }
            }

            if (!String.IsNullOrEmpty(ttIssueReturn.FromJobNum))
            {
                JobHead = FindFirstJobHead(Session.CompanyID, ttIssueReturn.FromJobNum);
                if (JobHead != null)
                {
                    ttIssueReturn.FromJobPartDescription = JobHead.PartDescription;
                    ttIssueReturn.FromJobPlant = JobHead.Plant;
                    ttIssueReturn.FromJobPartNum = JobHead.PartNum;
                    ttIssueReturn.FromJobRevisionNum = JobHead.RevisionNum;
                }

                JobAsmbl = FindFirstJobAsmbl(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq);
                if (JobAsmbl != null)
                {
                    ttIssueReturn.FromAssemblyPartDesc = JobAsmbl.Description;
                    ttIssueReturn.FromAssemblyPartNum = JobAsmbl.PartNum;
                    ttIssueReturn.FromAssemblyRevisionNum = JobAsmbl.RevisionNum;
                }
            }

            /* TreeDisplay */
            string cJobDirection = getJobDirection(ttIssueReturn.TranType);

            if (StringExtensions.Compare(cJobDirection, "From") == 0)
                ttIssueReturn.TreeDisplay = ttIssueReturn.FromJobNum + " : " + Compatibility.Convert.ToString(ttIssueReturn.FromAssemblySeq);
            else if (StringExtensions.Compare(cJobDirection, "To") == 0)
                ttIssueReturn.TreeDisplay = ttIssueReturn.ToJobNum + " : " + Compatibility.Convert.ToString(ttIssueReturn.ToAssemblySeq);
            else
                ttIssueReturn.TreeDisplay = ttIssueReturn.PartNum;

            /* ERPS-106804 - It is intentional for WIP-WIP to assign the NextAssemblySeq/NextJobSeq to the ToAssemblySeq/ToJobSeq  *
             * instead of making sure that the From/To values are the same since we're just changing location. The Next values for *
             * MtlQueue WIP-WIP now store the source FromAssemblySeq/FromOprSeq of the PartWip we're moving. This is to preserve   *
             * the source of PartWip even when it is moved to a different location.                                                */
            if (MtlQueue.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase))
            {
                ttIssueReturn.ToAssemblySeq = MtlQueue.NextAssemblySeq;
                ttIssueReturn.ToJobSeq = MtlQueue.NextJobSeq;
            }

            RefreshAttributeDescriptions();
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void setIssuedComplete(IssueReturnRow ttIssueReturn)
        {
            string cToType = string.Empty;
            bool jobMtlIssued = false;
            decimal qtyPer = decimal.Zero;
            Erp.Tables.JobMtl bJobMtl = null;
            cToType = this.getToType(ttIssueReturn.TranType);
            if (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") != 0 && StringExtensions.Compare(cToType, "STK") == 0)
            {
                return;
            }

            bJobMtl = this.FindFirstJobMtl(ttIssueReturn.Company, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq);
            if (bJobMtl != null)
            {
                qtyPer = bJobMtl.QtyPer;
                if (bJobMtl.IssuedComplete == true)
                {
                    jobMtlIssued = true;
                }
            }
            else
            {
                qtyPer = this.FindJobMtlQtyPer(ttIssueReturn.Company, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq);
            }

            if (jobMtlIssued == true)
            {
                ttIssueReturn.IssuedComplete = true;
            }
            else
            {/* if ttIssueReturn.TranType = "MTL-STK":U */
                if (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") == 0)
                {
                    if (qtyPer > 0)
                    {
                        if (ttIssueReturn.QtyRequired > (ttIssueReturn.QtyPreviouslyIssued - ttIssueReturn.RequirementQty))
                        {
                            ttIssueReturn.IssuedComplete = false;
                        }
                        else
                        {
                            ttIssueReturn.IssuedComplete = true;
                        }
                    }
                    else
                    {
                        if (ttIssueReturn.QtyRequired >= (ttIssueReturn.QtyPreviouslyIssued - ttIssueReturn.RequirementQty))
                        {
                            ttIssueReturn.IssuedComplete = false;
                        }
                        else
                        {
                            ttIssueReturn.IssuedComplete = true;
                        }

                    }
                }
                else
                {
                    if ((ttIssueReturn.RequirementQty >= (ttIssueReturn.QtyRequired - ttIssueReturn.QtyPreviouslyIssued))
                    && (ttIssueReturn.RequirementQty + ttIssueReturn.QtyRequired + ttIssueReturn.QtyPreviouslyIssued != 0))
                    {
                        ttIssueReturn.IssuedComplete = true;
                    }
                    else
                    {
                        ttIssueReturn.IssuedComplete = false;
                    }
                }
            }
        }

        ///<summary>
        ///  Purpose:  Gets the PartWip
        ///            sets the "From quantity" (ttIssueReturn.QtyPrevioulsyIssued)
        ///            and "TO Quantity" (ttIssueReturn.TranQty) to the PartWip.
        ///Notes:This is called when the from fields are changed. (FromJobNum, FromAssemblySeq, FromJobSeq, FromWarehouseCode, FromBinNum)
        ///      Currently this is only for a WIP-WIP transaction
        ///      Created as part of scr57730
        ///</summary>
        public void SetQuantity()
        {
            if (!(StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0))
            {
                return;
            }

            ttIssueReturn.QtyPreviouslyIssued = 0;
            ttIssueReturn.TranQty = 0;
            ttIssueReturn.RequirementQty = 0;

            PartWip = FindFirstPartWip(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, "M", ttIssueReturn.FromJobSeq, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, 0, ttIssueReturn.FromPCID);
            if (PartWip != null)
            {
                ttIssueReturn.QtyPreviouslyIssued = PartWip.Quantity;
                ttIssueReturn.TranQty = PartWip.Quantity;
                ttIssueReturn.RequirementQty = PartWip.Quantity;
            }
        }

        ///<summary>
        ///  Purpose:   Sets the attribute of a field in a dataset
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void setSensitive(string pcFieldName, Guid pcSysRowID, bool isSensitive)
        {/*for example ttIssueReturn.FromJobNum */
            /* Enter RowIdent if keeping field status for multiple records */
            ttFieldAttribute = (from ttFieldAttribute_Row in ttFieldAttributeRows
                                where StringExtensions.Compare(ttFieldAttribute_Row.FieldName, pcFieldName) == 0
                                && ttFieldAttribute_Row.SysRowID == pcSysRowID
                                select ttFieldAttribute_Row).FirstOrDefault();
            if (ttFieldAttribute == null)
            {
                ttFieldAttribute = new FieldAttribute();
                ttFieldAttributeRows.Add(ttFieldAttribute);
                ttFieldAttribute.FieldName = pcFieldName;
                ttFieldAttribute.SysRowID = pcSysRowID;
            }
            ttFieldAttribute.isSensitive = isSensitive;
        }

        ///<summary>
        ///  Purpose: To Update/Create PartAlloc table (based on updatePartAlloc).
        ///  As Transfer Orders are Picked from Stock (STK-PLT) PartAlloc table gets updated to reflect the
        ///  movement from "Picking" to "Picked".
        ///  This routine is used for Pick from Stock.
        ///  Notes: We first try to reduce the Picking Qty with the Picked Qty.  if PickingQty is less than Picked Qty
        ///         then we reduce the reserve quantity by the quantity over picked.
        ///</summary>
        private void updatePartAllocTF(IssueReturnRow ttIssueReturn)
        {
            decimal V_PickedQty = decimal.Zero;
            decimal V_OverPickedQty = decimal.Zero;
            string V_FromWarehouseCode = string.Empty;
            string V_FromBinNum = string.Empty;
            string V_FromPCID = string.Empty;
            string V_ToWarehouseCode = string.Empty;
            string V_ToBinNum = string.Empty;
            string V_ToPCID = string.Empty;
            string V_DimCode = string.Empty;
            string V_LotNum = string.Empty;
            decimal V_TempPickingQty = decimal.Zero;
            decimal V_TempReservedQty = decimal.Zero;
            decimal V_TempAllocatedQty = decimal.Zero;
            Erp.Tables.PartAlloc bPartAlloc = null;
            V_FromWarehouseCode = ttIssueReturn.FromWarehouseCode;
            V_FromBinNum = ttIssueReturn.FromBinNum;
            V_FromPCID = ttIssueReturn.FromPCID;
            V_ToWarehouseCode = ttIssueReturn.ToWarehouseCode;
            V_ToBinNum = ttIssueReturn.ToBinNum;
            V_ToPCID = ttIssueReturn.ToPCID;
            V_LotNum = ttIssueReturn.LotNum;
            V_DimCode = ttIssueReturn.UM;

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (!Part.TrackDimension)
            {
                this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, Part.IUM, out V_PickedQty, false);
            }
            else
            {
                V_PickedQty = ttIssueReturn.TranQty;
            }

            /* REDUCE PICKING QUANTITY */
            /* FIRST ATTEMPT TO REDUCE ALLOCATION IN THE WAREHOUSE/BIN FROM WHICH IT WAS PICKED */
            PartAlloc = this.FindFirstPartAllocWithUpdLock(Session.CompanyID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, V_FromWarehouseCode, V_FromBinNum, V_LotNum, V_FromPCID, V_DimCode, ttIssueReturn.TFOrdNum, ttIssueReturn.TFOrdLine, true);
            /* NEXT REDUCE ALLOCATION IN THE WAREHOUSE/BIN FROM OTHER BINS */
            if (PartAlloc == null)
            {
                PartAlloc = this.FindFirstPartAllocWithUpdLock(Session.CompanyID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, V_FromWarehouseCode, V_LotNum, V_DimCode, ttIssueReturn.TFOrdNum, ttIssueReturn.TFOrdLine, true);
            }
            if (PartAlloc != null)
            {
                V_TempPickingQty = PartAlloc.PickingQty;
                V_TempAllocatedQty = PartAlloc.AllocatedQty;
                V_TempPickingQty = V_TempPickingQty - V_PickedQty;
                if (V_TempPickingQty < 0)
                {
                    V_OverPickedQty = Math.Abs(V_TempPickingQty);
                    V_TempPickingQty = 0;
                }
                PartAlloc.PickingQty = V_TempPickingQty;
                PartAlloc.AllocatedQty = V_TempAllocatedQty;
                Db.Validate(PartAlloc);
                if (PartAlloc.ReservedQty == 0 && PartAlloc.AllocatedQty == 0 && PartAlloc.PickingQty == 0 && PartAlloc.PickedQty == 0)
                {
                    Db.PartAlloc.Delete(PartAlloc);
                }
                if (PartAlloc != null)
                {
                    Db.Release(ref PartAlloc);
                }

                /* INCREASE PICKED QUANTITY IN THE TO WAREHOUSE OR JOB */
                bPartAlloc = this.FindFirstPartAllocWithUpdLock(Session.CompanyID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, V_ToWarehouseCode, V_ToBinNum, V_LotNum, V_ToPCID, V_DimCode, ttIssueReturn.TFOrdNum, ttIssueReturn.TFOrdLine, "TRANSFER", true);
                if (bPartAlloc == null)
                {
                    bPartAlloc = new Erp.Tables.PartAlloc();
                    Db.PartAlloc.Insert(bPartAlloc);
                    bPartAlloc.Company = Session.CompanyID;
                    bPartAlloc.PartNum = ttIssueReturn.PartNum;
                    using (var inventoryTracking = new InventoryTracking(Db))
                    {
                        inventoryTracking.ValidateInventoryByRevision(Part?.TrackInventoryByRevision ?? false, ttIssueReturn);
                        ExceptionManager.AssertNoBLExceptions();

                        bPartAlloc.RevisionNum = ttIssueReturn.RevisionNum;
                    }
                    bPartAlloc.AttributeSetID = ttIssueReturn.AttributeSetID;
                    bPartAlloc.WarehouseCode = V_ToWarehouseCode;
                    bPartAlloc.BinNum = V_ToBinNum;
                    bPartAlloc.PCID = V_ToPCID;
                    bPartAlloc.LotNum = V_LotNum;
                    bPartAlloc.DimCode = V_DimCode;
                    bPartAlloc.TFOrdNum = ttIssueReturn.TFOrdNum;
                    bPartAlloc.TFOrdLine = ttIssueReturn.TFOrdLine;
                    bPartAlloc.HardAllocation = true;
                    bPartAlloc.DemandType = "Transfer";

                    bPartAlloc.RelatedToSchemaName = "ERP";
                    bPartAlloc.RelatedToTableName = "TFOrdDtl";
                    var tfOrdDtl = this.FindFirstTFOrdDtl(Session.CompanyID, ttIssueReturn.TFOrdNum, ttIssueReturn.TFOrdLine);
                    if (tfOrdDtl != null)
                    {
                        bPartAlloc.RelatedToSysRowID = tfOrdDtl.SysRowID;
                    }
                }
                bPartAlloc.PickedQty = bPartAlloc.PickedQty + V_PickedQty;
                V_PickedQty = 0;
            }
            if (bPartAlloc != null)
            {
                Db.Release(ref bPartAlloc);
            }

            /* RELIEVE PCID EXISTING ALLOCATIONS */

            /* REDUCE RESERVED QUANTITY */
            if (V_PickedQty > 0)
            {
                V_TempPickingQty = 0;
                V_TempReservedQty = 0;
                V_TempAllocatedQty = 0;

                /* REDUCE PICKING QUANTITY */
                /* FIRST ATTEMPT TO REDUCE IT IN THE WAREHOUSE OR JOB THAT IT WAS PICKED FROM */
                PartAlloc = this.FindFirstPartAllocWithUpdLock(Session.CompanyID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, V_FromWarehouseCode, V_DimCode, ttIssueReturn.TFOrdNum, ttIssueReturn.TFOrdLine);
                /* FAILED TO find ALLOCATION AT PICKING AREA - REDUCE IT IN ANY ALLOCATION RECORD FOR THE TRANSFER ORDER */
                if (PartAlloc == null)
                {
                    PartAlloc = this.FindFirstPartAllocWithUpdLock2(Session.CompanyID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, V_FromWarehouseCode, V_DimCode, ttIssueReturn.TFOrdNum, ttIssueReturn.TFOrdLine);
                }
                if (PartAlloc != null)
                {
                    V_TempPickingQty = PartAlloc.PickingQty;
                    V_TempReservedQty = PartAlloc.ReservedQty;
                    V_TempPickingQty = V_TempPickingQty - V_PickedQty;
                    if (V_TempPickingQty < 0)
                    {
                        V_OverPickedQty = Math.Abs(V_TempPickingQty);
                        V_TempPickingQty = 0;
                    }
                    /* REDUCE RESERVED QUANTITY if WE OVER PICKED */
                    if (V_OverPickedQty > 0)
                    {
                        V_TempReservedQty = V_TempReservedQty - V_OverPickedQty;
                        if (V_TempReservedQty < 0)
                        {
                            V_TempReservedQty = 0;
                        }
                    }
                    PartAlloc.PickingQty = V_TempPickingQty;
                    PartAlloc.ReservedQty = V_TempReservedQty;
                    Db.Validate(PartAlloc);
                    if (PartAlloc.ReservedQty == 0 && PartAlloc.AllocatedQty == 0 && PartAlloc.PickingQty == 0 && PartAlloc.PickedQty == 0)
                    {
                        Db.PartAlloc.Delete(PartAlloc);
                    }
                }
                if (PartAlloc != null)
                {
                    Db.Release(ref PartAlloc);
                }

                var tfOrdDtl = this.FindFirstTFOrdDtl(Session.CompanyID, ttIssueReturn.TFOrdNum, ttIssueReturn.TFOrdLine);
                Guid tfOrdDtlSysRowID = tfOrdDtl != null ? tfOrdDtl.SysRowID : Guid.Empty;

                /* INCREASE PICKED QUANTITY IN THE TO WAREHOUSE OR JOB */
                bPartAlloc = this.FindFirstPartAllocWithUpdLock3(Session.CompanyID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, V_DimCode, V_ToWarehouseCode, V_ToBinNum, V_LotNum, V_ToPCID, tfOrdDtlSysRowID);
                if (bPartAlloc == null)
                {
                    bPartAlloc = new Erp.Tables.PartAlloc();
                    Db.PartAlloc.Insert(bPartAlloc);
                    bPartAlloc.Company = Session.CompanyID;
                    bPartAlloc.PartNum = ttIssueReturn.PartNum;
                    using (var inventoryTracking = new InventoryTracking(Db))
                    {
                        inventoryTracking.ValidateInventoryByRevision(Part?.TrackInventoryByRevision ?? false, ttIssueReturn);
                        ExceptionManager.AssertNoBLExceptions();

                        bPartAlloc.RevisionNum = ttIssueReturn.RevisionNum;
                    }
                    bPartAlloc.AttributeSetID = ttIssueReturn.AttributeSetID;
                    bPartAlloc.DimCode = V_DimCode;
                    bPartAlloc.WarehouseCode = V_ToWarehouseCode;
                    bPartAlloc.BinNum = V_ToBinNum;
                    bPartAlloc.PCID = V_ToPCID;
                    bPartAlloc.TFOrdNum = ttIssueReturn.TFOrdNum;
                    bPartAlloc.TFOrdLine = ttIssueReturn.TFOrdLine;
                    bPartAlloc.LotNum = V_LotNum;
                    bPartAlloc.HardAllocation = false;
                    bPartAlloc.DemandType = "Transfer";

                    bPartAlloc.RelatedToSchemaName = "ERP";
                    bPartAlloc.RelatedToTableName = "TFOrdDtl";
                    bPartAlloc.RelatedToSysRowID = tfOrdDtl.SysRowID;
                }
                bPartAlloc.PickedQty = bPartAlloc.PickedQty + V_PickedQty;
                Db.Validate(bPartAlloc);
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void updatePartBin(Erp.Tables.PartTran PartTran)
        {
            decimal vTranQty = decimal.Zero;
            string vUM = string.Empty;
            if (Part != null && Part.TrackDimension)
            {
                vTranQty = PartTran.ActTranQty;
                vUM = PartTran.ActTransUOM;
            }
            else
            {
                vTranQty = PartTran.TranQty;
                vUM = PartTran.UM;
            }

            PartPlant = this.LibNonQtyBearingBin._NonQtyBearingBin(PartTran.WareHouseCode, PartTran.PartNum);
            if (PartPlant != null && PartPlant.QtyBearing)
            {
                LibDeferredUpdate.PostPBOnHand(PartTran.Company, PartTran.PartNum, PartTran.WareHouseCode, PartTran.BinNum, PartTran.LotNum, PartTran.AttributeSetID, vUM, vTranQty, string.Empty, PartTran.PCID);
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void updatePartSNFormat(string pcPartNum, string pcPlant, Erp.Tablesets.SNFormatTable ttSNFormatRows)
        {
            string cErrorMessage = string.Empty;

            ttSNFormat = (from ttSNFormat_Row in ttSNFormatRows
                          where StringExtensions.Compare(ttSNFormat_Row.PartNum, pcPartNum) == 0
                          && StringExtensions.Compare(ttSNFormat_Row.Plant, pcPlant) == 0
                          select ttSNFormat_Row).FirstOrDefault();
            if (ttSNFormat == null)
            {
                ttSNFormat = new Erp.Tablesets.SNFormatRow();
                ttSNFormatRows.Add(ttSNFormat);
                ttSNFormat.PartNum = pcPartNum;
                ttSNFormat.Company = Session.CompanyID;
                ttSNFormat.Plant = pcPlant;
                var outSNPrefix = ttSNFormat.SNPrefix;
                var outSNBaseDataType = ttSNFormat.SNBaseDataType;
                var outSNFormat = ttSNFormat.SNFormat;
                var outNumberOfDigits = ttSNFormat.NumberOfDigits;
                var outLeadingZeroes = ttSNFormat.LeadingZeroes;
                var outSNMask = ttSNFormat.SNMask;
                var outSNMaskPrefix = ttSNFormat.SNMaskPrefix;
                var outSNMaskSuffix = ttSNFormat.SNMaskSuffix;
                this.LibSNFormat.getSerialNoFormat(ttSNFormat.PartNum, ttSNFormat.Plant, out outSNPrefix, out outSNBaseDataType, out outSNFormat, out outNumberOfDigits, out outLeadingZeroes, out cErrorMessage, out outSNMask, out outSNMaskPrefix, out outSNMaskSuffix);
                ttSNFormat.SNPrefix = outSNPrefix;
                ttSNFormat.SNBaseDataType = outSNBaseDataType;
                ttSNFormat.SNFormat = outSNFormat;
                ttSNFormat.NumberOfDigits = outNumberOfDigits;
                ttSNFormat.LeadingZeroes = outLeadingZeroes;
                ttSNFormat.SNMask = outSNMask;
                ttSNFormat.SNMaskPrefix = outSNMaskPrefix;
                ttSNFormat.SNMaskSuffix = outSNMaskSuffix;
                if (!String.IsNullOrEmpty(cErrorMessage))
                {
                    throw new BLException(Strings.ErrorEncouWhenRetriSNFormat(cErrorMessage), "SNFormat", "SNFormat");
                }
            }
        }

        /// <summary>
        /// To update the PartWip with the transaction
        /// </summary>
        private void updatePartWIP(string Wip_Direction, IssueReturnRow ttIssueReturn, bool enforcePartWipExistence = true)
        {/* "To" - Use "To Fields", "From" - Use "From Fields" */
            string PW_WarehouseCode = string.Empty;
            string PW_BinNum = string.Empty;
            string PW_PCID = string.Empty;
            string PW_JobNum = string.Empty;
            int PW_AssemblySeq = 0;
            int PW_JobSeq = 0;
            decimal PW_Quantity = decimal.Zero;
            int PW_MtlSeq = 0;
            int PW_FromAssemblySeq = 0;
            int PW_FromJobSeq = 0;
            string LV_TrackType = string.Empty;
            string tmpResourceGrpID = string.Empty;
            bool checkFromValues = false;
            bool newPartWip = false;
            bool opChangeWipWip = false;
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) == false)
            {
                return;
            }
            /* STEP 1. ESTABLISH THE CORRECT VALUES IN THE COMMON "PW" VARIABLES */
            /* Sent TO WIP ex: issue stock to JobMtl */
            if (StringExtensions.Compare(Wip_Direction, "TO") == 0)
            {
                //DJY - if ToPCID is populated, ToWarehouseCode and ToBinNum must match
                //the current location of the PCID if it is not currently empty
                if (!string.IsNullOrEmpty(ttIssueReturn.ToPCID))
                {
                    var pkgControlStageHeader = this.FindFirstPkgControlStageHeader(ttIssueReturn.Company, ttIssueReturn.ToPCID);
                    if (pkgControlStageHeader != null && pkgControlStageHeader.PkgControlStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!pkgControlStageHeader.WarehouseCode.KeyEquals(ttIssueReturn.ToWarehouseCode) ||
                            !pkgControlStageHeader.BinNum.KeyEquals(ttIssueReturn.ToBinNum))
                        {
                            throw new BLException(Strings.CannotMovePCIDAlreadyContainingWIP(ttIssueReturn.ToPCID));
                        }
                    }
                }

                PW_WarehouseCode = ttIssueReturn.ToWarehouseCode;
                PW_BinNum = ttIssueReturn.ToBinNum;
                PW_PCID = ttIssueReturn.ToPCID;
                PW_JobNum = ttIssueReturn.ToJobNum;
                PW_AssemblySeq = ttIssueReturn.ToAssemblySeq;
                PW_JobSeq = ttIssueReturn.ToJobSeq;
                PW_FromAssemblySeq = ttIssueReturn.FromAssemblySeq;
                PW_FromJobSeq = ttIssueReturn.FromJobSeq;
            }
            else
            {
                PW_WarehouseCode = ttIssueReturn.FromWarehouseCode;
                PW_BinNum = ttIssueReturn.FromBinNum;
                PW_PCID = ttIssueReturn.FromPCID;
                PW_JobNum = ttIssueReturn.FromJobNum;
                PW_AssemblySeq = ttIssueReturn.FromAssemblySeq;
                PW_JobSeq = ttIssueReturn.FromJobSeq;
                PW_FromAssemblySeq = ttIssueReturn.ToAssemblySeq;
                PW_FromJobSeq = ttIssueReturn.ToJobSeq;
                /* SPECIAL CONSIDERATIONS for MFG-OPR, PUR-SUB, INS-SUB, DMR-SUB
                    Need to use the "TO" job fields, since wip is tracked at the next operation sequence
                */
                if (StringExtensions.Compare(ttIssueReturn.TranType, "MFG-OPR") == 0
                || StringExtensions.Compare(ttIssueReturn.TranType, "INS-SUB") == 0
                || StringExtensions.Compare(ttIssueReturn.TranType, "PUR-SUB") == 0
                || StringExtensions.Compare(ttIssueReturn.TranType, "DMR-SUB") == 0)
                {
                    PW_JobNum = ttIssueReturn.ToJobNum;
                    PW_AssemblySeq = ttIssueReturn.ToAssemblySeq;
                    PW_JobSeq = ttIssueReturn.ToJobSeq;
                    PW_FromAssemblySeq = ttIssueReturn.FromAssemblySeq;
                    PW_FromJobSeq = ttIssueReturn.FromJobSeq;
                }

                //DJY - if any existing PartWip exists in a PCID for this JobNum/AssemblySeq
                //do not let the user proceed if they come from the Classic UI which does not have From PCID
                using (Erp.Internal.Lib.PackageControl libPackageControl = new Internal.Lib.PackageControl(Db))
                {
                    if (libPackageControl.IsWinClientClientType() && this.ExistsPartWip(ttIssueReturn.Company, PW_JobNum, PW_AssemblySeq))
                    {
                        throw new BLException(Strings.WIPExistsInAPCIDCannotUseClassicUI);
                    }
                }
            }

            /* decided not to create PartWip's for blank warehouses */
            if (String.IsNullOrEmpty(PW_WarehouseCode))
            {
                return;
            }
            /* STEP 2. DETERMINE THE TRACKTYPE SETTING */
            /* R = RAW MATERIAL */
            if (ttIssueReturn.TranType.StartsWith("STK", StringComparison.OrdinalIgnoreCase)
            || ttIssueReturn.TranType.StartsWith("PUR", StringComparison.OrdinalIgnoreCase)
            || StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") == 0
            || StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-MTL") == 0
            || StringExtensions.Compare(ttIssueReturn.TranType, "INS-MTL") == 0
            || StringExtensions.Compare(ttIssueReturn.TranType, "MTL-MTL") == 0
            || StringExtensions.Compare(ttIssueReturn.TranType, "PLT-MTL") == 0
            || StringExtensions.Compare(ttIssueReturn.TranType, "MTL-INS") == 0)
            {
                LV_TrackType = "R";
            }
            else
            {
                LV_TrackType = "M";/* M = MANUFACTURED */
            }

            /*  Some transaction types didn't work with the above logic. Consider them here. */
            if (StringExtensions.Compare(ttIssueReturn.TranType, "PUR-SUB") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "STK-ASM") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "ASM-STK") == 0
            || StringExtensions.Compare(ttIssueReturn.TranType, "DMR-SUB") == 0)
            {
                LV_TrackType = "M";
            }
            /* STEP 3. if RAW MATERIAL OR ISSUE/return OF SUBASSEMBLY find THE SEQ OF THE RELATED OPERATION
                if RAW MATERIAL STORE THE MTLSEQ (at this point PW-JobSeq contains the material seq ) */

            if (StringExtensions.Compare(LV_TrackType, "R") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "STK-ASM") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "ASM-STK") == 0)
            {
                if (StringExtensions.Compare(LV_TrackType, "R") == 0)
                {
                    PW_MtlSeq = PW_JobSeq;
                }

                this.getRelatedOperation(ttIssueReturn.TranType, PW_JobNum, ref PW_AssemblySeq, ref PW_JobSeq);

            }
            /* ERPS-106804 - When processing WIP-WIP/MFG-OPR, we need to make sure that we consume/create the correct PartWip considering the *
                * source FromAssemblySeq/FromOprSeq. This is to avoid merging PartWips from different subassemblies into a single PartWip.       *
                * NOTE: DMR-SUB is a special case, the FROM direction will not use the checkFromValues parameter since the PW_SaveFrom will have *
                * no values. But the TO direction should be able to use the PW_SaveFrom values to preserve the source of the PartWip.            */
            opChangeWipWip = (ttIssueReturn.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase) && ttIssueReturn.MtlQueueRowId == Guid.Empty && ttIssueReturn.FromJobSeq != ttIssueReturn.ToJobSeq);
            checkFromValues = (StringExtensions.Lookup("WIP-WIP,MFG-OPR,PUR-SUB,INS-SUB,DMR-SUB,DMR-ASM", ttIssueReturn.TranType) > -1) && opChangeWipWip == false && (PW_SaveFromAssemblySeq != 0 || PW_SaveFromOprSeq != 0);
            /* STEP 4. - CREATE OR UPDATE THE PARTWIP RECORD */
            PartWipQueryParams partWipQueryParams;
            partWipQueryParams = new PartWipQueryParams();
            partWipQueryParams.company = Session.CompanyID;
            partWipQueryParams.plant = Session.PlantID;
            partWipQueryParams.partNum = ttIssueReturn.PartNum;
            partWipQueryParams.jobNum = PW_JobNum;
            partWipQueryParams.assemblySeq = PW_AssemblySeq;
            partWipQueryParams.oprSeq = PW_JobSeq;
            partWipQueryParams.mtlSeq = PW_MtlSeq;
            partWipQueryParams.wareHouseCode = PW_WarehouseCode;
            partWipQueryParams.lotNum = ttIssueReturn.LotNum;
            partWipQueryParams.attributeSetID = ttIssueReturn.AttributeSetID;
            partWipQueryParams.binNum = PW_BinNum;
            partWipQueryParams.trackType = LV_TrackType;
            partWipQueryParams.pcid = PW_PCID;
            partWipQueryParams.checkFromValues = checkFromValues;
            partWipQueryParams.fromAsmSeq = PW_SaveFromAssemblySeq;
            partWipQueryParams.fromOprSeq = PW_SaveFromOprSeq;

            PartWip = FindFirstPartWipWithUpdLock(partWipQueryParams);
            if (PartWip == null)
            {
                partWipQueryParams.lotNum = string.Empty;
                PartWip = FindFirstPartWipWithUpdLock(partWipQueryParams);
            }
            if (PartWip == null)
            {
                PartWip = new Erp.Tables.PartWip();
                Db.PartWip.Insert(PartWip);
                PartWip.Company = Session.CompanyID;
                PartWip.Plant = Session.PlantID;
                PartWip.PartNum = ttIssueReturn.PartNum;
                using (var inventoryTracking = new InventoryTracking(Db))
                {
                    inventoryTracking.ValidateInventoryByRevision(ttIssueReturn.PartTrackInventoryByRevision, ttIssueReturn);
                    ExceptionManager.AssertNoBLExceptions();

                    PartWip.RevisionNum = ttIssueReturn.RevisionNum;
                }
                PartWip.AttributeSetID = ttIssueReturn.AttributeSetID;
                PartWip.JobNum = PW_JobNum;
                PartWip.AssemblySeq = PW_AssemblySeq;
                PartWip.OprSeq = PW_JobSeq;
                PartWip.MtlSeq = PW_MtlSeq;
                PartWip.WareHouseCode = PW_WarehouseCode;
                PartWip.LotNum = ttIssueReturn.LotNum;
                PartWip.BinNum = PW_BinNum;
                PartWip.PCID = PW_PCID;
                PartWip.TrackType = LV_TrackType;
                PartWip.UM = ttIssueReturn.RequirementUOM;
                newPartWip = true;
            }
            /* CONVERT QUANTITY TO THE PARTWIP UNIT OF MEASURE */
            this.LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, PartWip.UM, out PW_Quantity, true);
            if (StringExtensions.Compare(Wip_Direction, "TO") != 0)
            {
                PW_Quantity = -(PW_Quantity);/* REDUCE PART WIP */
            }

            //DJY - for Move Material and Issue Material, do not allow the quantity in the PCID
            //to become negative, they can only move up to the amount that has been issued.
            if (StringExtensions.Compare(Wip_Direction, "FROM") == 0)
            {
                if (PartWip.Quantity + PW_Quantity < 0)
                {
                    if (!string.IsNullOrEmpty(PartWip.PCID))
                    {
                        throw new BLException(Strings.CannotMoveMoreThanWasIssuedToThisPCID(PartWip.PCID));
                    }
                    else if (enforcePartWipExistence)
                    {
                        throw new BLException(Strings.CannotMoveMoreThanCurrentlyExistsInThisLocation);
                    }
                }
            }

            PartWip.Quantity = PartWip.Quantity + PW_Quantity;
            PartWip.LastActivityDate = CompanyTime.Today();
            PartWip.LastActivityTime = Erp.Internal.Lib.DateExtensions.SecondsSinceMidnight(CompanyTime.Now());
            PartWip.UpdateStageQty = ttIssueReturn.TranType;

            /* Do not clear the FromAssemblySeq/FromOprSeq of the source (FROM) PartWip if Trantype is WIP-WIP/MFG-OPR/PUR-SUB/INS-SUB/DMR-SUB/DMR-ASM */
            if (StringExtensions.Lookup("WIP-WIP,MFG-OPR,PUR-SUB,INS-SUB,DMR-SUB,DMR-ASM", ttIssueReturn.TranType) == -1)
            {
                PartWip.FromAssemblySeq = PW_FromAssemblySeq;
                PartWip.FromOprSeq = PW_FromJobSeq;
            }

            /* If FROM WIP-WIP save the fromopcode, fromResourceGrpID, to be copied to the TO WIP-WIP  */
            /* If TO WIP-WIP assign the FromOpCode, FromResourceGrID using the saved values.           */
            /* ERPS-106804 - Preserve the source FromAssemblySeq/FromOprSeq when moving WIP location.  */
            if (StringExtensions.Lookup("WIP-WIP,MFG-OPR,PUR-SUB,INS-SUB,DMR-SUB,DMR-ASM", ttIssueReturn.TranType) > -1)
            {
                if (StringExtensions.Compare(Wip_Direction, "TO") == 0)
                {
                    /* Overwrite the From Values with the saved source values only if not opChangeWipWip or it's the special WIP-WIP but new PartWip record */
                    if (opChangeWipWip == false || newPartWip == true)
                    {
                        PartWip.FromAssemblySeq = PW_SaveFromAssemblySeq;
                        PartWip.FromOprSeq = PW_SaveFromOprSeq;
                        PartWip.FromOpCode = PW_SaveFromOpcode;
                        PartWip.FromResourceGrpID = PW_SaveFromResourceGrpID;
                    }
                }
                else
                {
                    if (opChangeWipWip == true)
                    {
                        PW_SaveFromAssemblySeq = PartWip.AssemblySeq;
                        PW_SaveFromOprSeq = PartWip.OprSeq;
                        PW_SaveFromOpcode = PartWip.OpCode;
                        PW_SaveFromResourceGrpID = PartWip.ResourceGrpID;
                    }
                    else
                    {
                        PW_SaveFromAssemblySeq = PartWip.FromAssemblySeq;
                        PW_SaveFromOprSeq = PartWip.FromOprSeq;
                        PW_SaveFromOpcode = PartWip.FromOpCode;
                        PW_SaveFromResourceGrpID = PartWip.FromResourceGrpID;
                    }
                }
            }

            if (PartWip.OprSeq != 0)
            {
                JobOper = this.FindFirstJobOper14(PartWip.Company, PartWip.JobNum, PartWip.AssemblySeq, PartWip.OprSeq);
                if (JobOper != null)
                {
                    tmpResourceGrpID = this.LibGetResourceGrpID.getJobOperResourceGrpID(JobOper, string.Empty);
                    PartWip.ResourceGrpID = tmpResourceGrpID;
                    PartWip.OpCode = JobOper.OpCode;
                }
            }
            /*  MANUFACTURED ITEMS, THAT ARE ALLOCATED TO ASM 0,OPER 0 MEANS THAT IT'S ON THE FINAL OP. SO SET THE FLAG */
            if (StringExtensions.Compare(PartWip.TrackType, "M") == 0 && PartWip.AssemblySeq == 0 && PartWip.OprSeq == 0)
            {
                PartWip.FinalOp = true;
            }

            Db.Validate(PartWip);

            if (PartWip.Quantity <= 0)
            {
                Db.PartWip.Delete(PartWip);
            }

            Db.Release(ref PartWip);
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private bool isAllowedMaterialTransaction()
        {
            return (ttIssueReturn.TranType.StartsWith("STK", StringComparison.OrdinalIgnoreCase)
                    || ttIssueReturn.TranType.StartsWith("PUR", StringComparison.OrdinalIgnoreCase)
                    //MTLs
                    || StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") == 0
                    || StringExtensions.Compare(ttIssueReturn.TranType, "MTL-INS") == 0
                    || StringExtensions.Compare(ttIssueReturn.TranType, "MTL-MTL") == 0

                    //inspections
                    || StringExtensions.Compare(ttIssueReturn.TranType, "INS-MTL") == 0
                    || StringExtensions.Compare(ttIssueReturn.TranType, "INS-ASM") == 0
                    || StringExtensions.Compare(ttIssueReturn.TranType, "INS-SUB") == 0

                    //DMRs
                    || StringExtensions.Compare(ttIssueReturn.TranType, "DMR-ASM") == 0
                    || StringExtensions.Compare(ttIssueReturn.TranType, "DMR-MTL") == 0
                    || StringExtensions.Compare(ttIssueReturn.TranType, "DMR-SUB") == 0

                    //Stocks
                    || StringExtensions.Compare(ttIssueReturn.TranType, "STK-MTL") == 0
                    || StringExtensions.Compare(ttIssueReturn.TranType, "STK-ASM") == 0

                    //Various
                    || StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-MTL") == 0
                    || StringExtensions.Compare(ttIssueReturn.TranType, "PLT-MTL") == 0
                    || StringExtensions.Compare(ttIssueReturn.TranType, "PUR-MTL") == 0
                    || StringExtensions.Compare(ttIssueReturn.TranType, "MFG-OPR") == 0
                    || StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0
                    ) ? true : false;
        }

        private void updateSerialNo(SNFormatTable ttSNFormatRows, SelectedSerialNumbersTable ttSelectedSerialNumbersRows, IssueReturnRow ttIssueReturn)
        {
            bool voidSerial = false;
            if (ttIssueReturn.ProcessID.Equals("PkgControlIDBuildSplitMerge", StringComparison.OrdinalIgnoreCase))
            {
                // When issuing from one pcid to another, if the plant does outbound serial tracking, and moving from an outbound PCID to a non-outbound PCID, void the serial numbers
                if (!String.IsNullOrEmpty(ttIssueReturn.ToPCID) && !String.IsNullOrEmpty(ttIssueReturn.FromPCID))
                {
                    if (LibSerialCommon.isSerTraPlantType(ttIssueReturn.SerialControlPlant) == 3 &&
                        GetPkgControlHeaderOutboundContainer(Session.CompanyID, ttIssueReturn.FromPCID) &&
                        !GetPkgControlHeaderOutboundContainer(Session.CompanyID, ttIssueReturn.ToPCID))
                    {
                        voidSerial = true;
                    }
                }
                // When removing from a pcid back to inventory, if the plant does outbound serial tracking, and moving from an outbound PCID, void the serial numbers
                if (String.IsNullOrEmpty(ttIssueReturn.ToPCID) && !String.IsNullOrEmpty(ttIssueReturn.FromPCID))
                {
                    if (LibSerialCommon.isSerTraPlantType(ttIssueReturn.SerialControlPlant) == 3 &&
                        GetPkgControlHeaderOutboundContainer(Session.CompanyID, ttIssueReturn.FromPCID))
                    {
                        voidSerial = true;
                    }
                }
            }
            else
            {
                /* Set SerialNo.Voided if going to a non-full tracking plant, void indicates the SerialNo has been moved to a non-tracking plant */
                // Exception: if the to plant is outbound tracking and the SerialNo are being moved into an outbound container (PCID) do not void the SerialNo.
                string toPlant = ((!string.IsNullOrEmpty(ttIssueReturn.SerialControlPlant2) ? ttIssueReturn.SerialControlPlant2 : ttIssueReturn.SerialControlPlant));
                int toPlantTracking = LibSerialCommon.isSerTraPlantType(toPlant);
                if (toPlantTracking == 1)
                {
                    voidSerial = true;
                }
                else if (toPlantTracking == 2)
                {
                    voidSerial = false;
                }
                else if (toPlantTracking == 3)
                {
                    if ((!string.IsNullOrEmpty(ttIssueReturn.ToPCID)) && (GetPkgControlHeaderOutboundContainer(ttIssueReturn.Company, ttIssueReturn.ToPCID)))
                    {
                        voidSerial = false;
                    }
                    else voidSerial = true;
                }
            }

            foreach (var ttSelectedSerialNumbers_iterator in (from ttSelectedSerialNumbers_Row in ttSelectedSerialNumbersRows
                                                              where ttSelectedSerialNumbers_Row.PartNum.KeyEquals(ttIssueReturn.PartNum)
                                                              && ttSelectedSerialNumbers_Row.AttributeSetID == ttIssueReturn.AttributeSetID
                                                              select ttSelectedSerialNumbers_Row).ToList())
            {
                ttSelectedSerialNumbers = ttSelectedSerialNumbers_iterator;
                if (String.IsNullOrEmpty(ttSelectedSerialNumbers.RowMod) || ttSelectedSerialNumbers.Deselected == true)
                {
                    ttSelectedSerialNumbersRows.Remove(ttSelectedSerialNumbers);
                    continue;
                }

                /* there should always be a ttSNFormat record to find since we make sure the record is there before staring the update logic */
                ttSNFormat = (from ttSNFormat_Row in ttSNFormatRows
                              where ttSNFormat_Row.PartNum.KeyEquals(ttSelectedSerialNumbers.PartNum) &&
                                  ttSNFormat_Row.Plant.KeyEquals(ttIssueReturn.SerialControlPlant)
                              select ttSNFormat_Row).FirstOrDefault();

                SerialNo serialNo = FindFirstSerialNoWithUpdLock2(Session.CompanyID, ttSelectedSerialNumbers.PartNum, ttSelectedSerialNumbers.SerialNumber);
                /* find or create the SerialNo record. for issue transactions if the from plant controls the serial processing the serial number must
                exist; if the to plant is controlling serial processing if the serial number exists it must be Voided = yes. Whether a new serial number
                can be created or not has already been validated in validateSerialNumber so here we can go ahead and call getNewSNRecord and let
                it find or create the new serial number as needed.  */
                if (serialNo == null)
                {
                    serialNo = LibSNFormat.getNewSerialNo(ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, ttSNFormat.SNBaseDataType, ttSNFormat.SNPrefix, ttSNFormat.SNFormat, ttSelectedSerialNumbers.SerialNumber, ttSelectedSerialNumbers.SNBaseNumber, ttSelectedSerialNumbers.RawSerialNum, ttSNFormat.SNMask, ttSNFormat.SNMaskPrefix, ttSNFormat.SNMaskSuffix, ttSNFormat.Plant);

                    if (serialNo == null)
                        throw new BLException(Strings.CouldNotCreateSerialNumber(ttSelectedSerialNumbers.SerialNumber), "SerialNo", "SerialNumber");
                }

                UpdateSerialNoRow(serialNo, ttIssueReturn, voidSerial);
            }
        }

        private void UpdateSerialNoForPCID(IssueReturnRow issueReturnRow, bool voidSerial)
        {
            string validSNStatus = string.Empty;
            checkSNStatus(issueReturnRow.TranType, 1, out validSNStatus);

            foreach (SerialNo serialNo in SelectSerialNoByPCIDAndPartNumWithUpdLock(Session.CompanyID, issueReturnRow.FromPCID, issueReturnRow.PartNum, issueReturnRow.AttributeSetID, validSNStatus, false))
            {
                UpdateSerialNoRow(serialNo, issueReturnRow, voidSerial);
            }
        }

        private void UpdateSerialNoRow(SerialNo serialNo, IssueReturnRow issueReturnRow, bool voidSerial)
        {
            serialNo.TempTranID = 0;
            serialNo.TranAdd = false;
            serialNo.PrevSNStatus = serialNo.SNStatus;
            serialNo.Selected = false;
            serialNo.WareHouseCode = issueReturnRow.ToWarehouseCode;
            serialNo.BinNum = issueReturnRow.ToBinNum;
            serialNo.LotNum = issueReturnRow.LotNum;
            serialNo.PCID = voidSerial == false ? issueReturnRow.ToPCID : string.Empty;
            serialNo.Voided = voidSerial;

            if (issueReturnRow.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase))
                serialNo.NextLbrOprSeq = issueReturnRow.ToJobSeq;

            bool deleteSerialInfo = false;

            /* ISSUES-TRAN-BLOCK - if lIssueTransaction = yes */
            bool issueTransaction = (StringExtensions.Lookup("MTL-STK,ASM-STK,UKN-STK", issueReturnRow.TranType) == -1) ? true : false;
            if (issueTransaction == true)
            {
                serialNo.PrevSNStatus = serialNo.SNStatus;

                /* Don't wipe out job info if issue is not to another job */
                serialNo.AssemblySeq = (((StringExtensions.Compare(issueReturnRow.TranType, "MFG-OPR") != 0 && StringExtensions.Compare(issueReturnRow.TranType, "INS-ASM") != 0)) ? issueReturnRow.ToAssemblySeq : serialNo.AssemblySeq);

                /* generally those trantype that are handled as "OPR" types should not reset SerialNo.MtlSeq  otherwise it ends up getting set to the OprSeq, not a valid MtlSeq*/
                if (!issueReturnRow.TranType.Equals("MFG-OPR", StringComparison.OrdinalIgnoreCase) &&
                    !issueReturnRow.TranType.Equals("INS-ASM", StringComparison.OrdinalIgnoreCase) &&
                    !issueReturnRow.TranType.Equals("ASM-INS", StringComparison.OrdinalIgnoreCase) &&
                    !issueReturnRow.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase) &&
                    !issueReturnRow.TranType.Equals("PUR-SUB", StringComparison.OrdinalIgnoreCase) &&
                    !issueReturnRow.TranType.Equals("PUR-INS", StringComparison.OrdinalIgnoreCase) &&
                    !issueReturnRow.TranType.Equals("INS-SUB", StringComparison.OrdinalIgnoreCase) &&
                    !issueReturnRow.TranType.Equals("DMR-SUB", StringComparison.OrdinalIgnoreCase))
                {
                    serialNo.MtlSeq = issueReturnRow.ToJobSeq;
                }

                if (!String.IsNullOrEmpty(issueReturnRow.ToJobNum) && issueReturnRow.ReassignSNAsm && !String.IsNullOrEmpty(serialNo.JobNum))
                {
                    serialNo.PriorJobNum = serialNo.JobNum;
                    serialNo.PriorAssemblySeq = serialNo.AssemblySeq;
                    serialNo.PriorMtlSeq = serialNo.MtlSeq;
                    serialNo.PriorPartNum = serialNo.PartNum;
                }

                serialNo.JobNum = (!String.IsNullOrEmpty(issueReturnRow.ToJobNum)) ? issueReturnRow.ToJobNum : serialNo.JobNum;

                /* Verify if is not a picking transaction. Not all records where ttIssueReturn.OrderNum <> 0 are picking records. We can have an Order linked to a Job */
                if (StringExtensions.Lookup("MFG-SHP,STK-SHP,PUR-SHP,KIT-SHP,CMP-SHP", issueReturnRow.TranType) == -1 || issueReturnRow.OrderNum == 0)
                {
                    serialNo.SNStatus = serialNoStatus(issueReturnRow.TranType, serialNo.SNStatus);

                    if (issueReturnRow.TranType.Equals("STK-STK", StringComparison.OrdinalIgnoreCase) &&
                        String.IsNullOrEmpty(serialNo.SNStatus) && !String.IsNullOrEmpty(issueReturnRow.ToPCID))
                    {
                        serialNo.SNStatus = "INVENTORY";
                    }

                    if (issueReturnRow.ProcessID.Equals("Unpick", StringComparison.OrdinalIgnoreCase) || issueReturnRow.ProcessID.Equals("UnpickPCID", StringComparison.OrdinalIgnoreCase))
                    {
                        serialNo.SNStatus = issueReturnRow.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase) ? "WIP" : "INVENTORY";
                        serialNo.OrderNum = 0;
                        serialNo.OrderLine = 0;
                        serialNo.OrderRelNum = 0;
                        serialNo.TFOrdNum = string.Empty;
                        serialNo.TFOrdLine = 0;
                    }

                    if (issueReturnRow.TranType.Equals("STK-PLT", StringComparison.OrdinalIgnoreCase))
                    {
                        serialNo.SNStatus = "PICKED";
                        serialNo.TFOrdNum = issueReturnRow.TFOrdNum;
                        serialNo.TFOrdLine = issueReturnRow.TFOrdLine;
                    }

                    if (serialNo.SNStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase))
                    {
                        if (issueReturnRow.ReassignSNAsm == false && serialNo.MtlSeq > 0)
                        {
                            JobHead altJobHead = FindFirstJobHead(Session.CompanyID, issueReturnRow.ToJobNum);
                            if (altJobHead != null)
                            {
                                if ((LibSerialCommon.plantLowLvlSerTrkType(altJobHead.Plant, ref PlantConfCtrl) < 2) || (partSerialTracking(altJobHead.PartNum) == false))
                                {
                                    serialNo.SNStatus = "CONSUMED";
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (issueReturnRow.ProcessID.Equals("Unpick", StringComparison.OrdinalIgnoreCase) || issueReturnRow.ProcessID.Equals("UnpickPCID", StringComparison.OrdinalIgnoreCase))
                        serialNo.SNStatus = issueReturnRow.TranType.Equals("WIP-WIP", StringComparison.OrdinalIgnoreCase) ? "WIP" : "INVENTORY";
                    else
                        serialNo.SNStatus = "PICKED";

                    /* only update order fields if the "to plant" is set for full serial tracking, see 9.00 serial tracking design */
                    if (LibSerialCommon.isSerTraPlantType(getOwnerPlant(issueReturnRow.ToWarehouseCode)) == 2)
                    {
                        serialNo.OrderNum = issueReturnRow.OrderNum;
                        serialNo.OrderLine = issueReturnRow.OrderLine;
                        serialNo.OrderRelNum = issueReturnRow.OrderRel;
                    }
                }
            }
            else
            {
                string partNum = serialNo.PartNum;
                string serialNumber = serialNo.SerialNumber;

                if (LibSerialCommon.isSerTraPlantType(getOwnerPlant(issueReturnRow.ToWarehouseCode)) != 2)
                {
                    SNTran = FindFirstSNTran(Session.CompanyID, partNum, serialNumber, 1);
                    if (SNTran != null && (StringExtensions.Lookup("STK-MTL,STK-ASM,STK-UKN", SNTran.TranType) > -1))
                    {
                        SNTran = FindFirstSNTran2(Session.CompanyID, partNum, serialNumber, 1);
                    }
                    if (SNTran == null)
                    {
                        Db.SerialNo.Delete(serialNo);
                        deleteSerialInfo = true;
                    }
                }

                /* if a child serial number is moving and the parent is not, we need to break the parent/child relationship; if it was  added by matching but is now being sent to stock, it becomes a "real" serial number part and we remove the AddedByMatching flag*/
                if (issueReturnRow.TranType.Equals("ASM-STK", StringComparison.OrdinalIgnoreCase) || issueReturnRow.TranType.Equals("MTL-STK", StringComparison.OrdinalIgnoreCase))
                {
                    SerialMatch = FindFirstSerialMatchWithUpdLock(Session.CompanyID, partNum, serialNumber);
                    if (SerialMatch != null)
                    {
                        if (deleteSerialInfo == false)
                            serialNo.AddedByMatching = false;

                        string parentPartNum = SerialMatch.ParentPartNum;
                        string parentSerialNum = SerialMatch.ParentSerialNo;
                        Db.SerialMatch.Delete(SerialMatch);

                        SerialNo tmpSerialNo = FindFirstSerialNoWithUpdLock3(Session.CompanyID, parentPartNum, parentSerialNum);
                        if (tmpSerialNo != null)
                            tmpSerialNo.FullyMatched = false;
                    }
                }

                /* if the serial info is not deleted then make the rest of the SerialNo updates */
                if (deleteSerialInfo == false)
                {
                    serialNo.PrevSNStatus = serialNo.SNStatus;
                    serialNo.SNStatus = serialNoStatus(issueReturnRow.TranType, serialNo.SNStatus);
                    serialNo.JobNum = "";
                    serialNo.AssemblySeq = 0;
                    serialNo.MtlSeq = 0;

                    if (issueReturnRow.TranType.Equals("STK-STK", StringComparison.OrdinalIgnoreCase) &&
                        String.IsNullOrEmpty(serialNo.SNStatus) && !String.IsNullOrEmpty(issueReturnRow.ToPCID))
                    {
                        serialNo.SNStatus = "INVENTORY";
                    }

                    /* reset the prior job info on the serial number if it's a return, try to reset to what it was before it was issued */
                    if (issueReturnRow.TranType.Equals("UKN-STK", StringComparison.OrdinalIgnoreCase))
                    {
                        SNTran = FindLastSNTran(Session.CompanyID, partNum, serialNumber, "STK-UKN");
                        if (SNTran != null)
                        {
                            serialNo.JobNum = SNTran.JobNum;
                            serialNo.AssemblySeq = SNTran.AssemblySeq;
                            serialNo.MtlSeq = SNTran.MtlSeq;
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(issueReturnRow.FromJobNum))
                        {
                            SNTran = FindFirstSNTran(Session.CompanyID, partNum, serialNumber, issueReturnRow.FromJobNum);
                            if (SNTran != null && SNTran.TranNum > 1)
                            {
                                int prevJobTranNum = SNTran.TranNum - 1;

                                SNTran prevSNTran = FindFirstSNTran3(Session.CompanyID, partNum, serialNumber, prevJobTranNum);
                                if (prevSNTran != null)
                                {
                                    serialNo.JobNum = prevSNTran.JobNum;
                                    serialNo.AssemblySeq = prevSNTran.AssemblySeq;
                                    serialNo.MtlSeq = prevSNTran.MtlSeq;
                                }
                            }
                        }
                    }
                }
            }

            if (deleteSerialInfo == false)
                createSNTran(issueReturnRow.TranType, issueReturnRow.TranDate, serialNo);
        }

        ///<summary>
        ///  Purpose: To apply the NonConformance material move entry (MTL-INS) to Inspection.
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void updJobMtlCosts()
        {
            string OldJobNum = string.Empty;
            int OldAsmSeq = 0;
            int OldMtlSeq = 0;
            decimal PrevMtlBurUnitCost = decimal.Zero;
            decimal PrevMaterialMtlUnitCost = decimal.Zero;
            decimal PrevMaterialSubUnitCost = decimal.Zero;
            decimal PrevMaterialLabUnitCost = decimal.Zero;
            decimal PrevMaterialBurUnitCost = decimal.Zero;
            decimal PrevQty = decimal.Zero;
            int ndec = 0;
            decimal partTranConvQty = decimal.Zero;
            decimal prevConvQty = decimal.Zero;

            NonConf = this.FindFirstNonConf(PartTran.Company, PartTran.NonConfID);
            if (NonConf != null)
            {
                OldJobNum = NonConf.JobNum;
                OldAsmSeq = NonConf.AssemblySeq;
                OldMtlSeq = NonConf.MtlSeq;
                PrevMtlBurUnitCost = NonConf.ScrapMaterialBurCost;
                PrevMaterialMtlUnitCost = NonConf.MaterialMtlCost;
                PrevMaterialLabUnitCost = NonConf.MaterialLabCost;
                PrevMaterialSubUnitCost = NonConf.MaterialSubCost;
                PrevMaterialBurUnitCost = NonConf.MaterialBurCost;
                PrevQty = NonConf.Quantity;
            }

            JobMtl = this.FindFirstJobMtlWithUpdLock(Session.CompanyID, OldJobNum, OldAsmSeq, OldMtlSeq);
            if (JobMtl != null)
            {
                if (PrevQty != 0 && (StringExtensions.Compare(JobMtl.IUM, NonConf.ScrapUM) != 0))
                {
                    this.LibAppService.UOMConv(JobMtl.PartNum, PrevQty, NonConf.ScrapUM, JobMtl.IUM, out prevConvQty, false);
                }
                else
                {
                    prevConvQty = PrevQty;
                }

                JobMtl.IssuedQty = JobMtl.IssuedQty + prevConvQty;
                JobMtl.IssuedComplete = (JobMtl.IssuedQty >= JobMtl.RequiredQty);
                if (JobMtl.IssuedQty == 0)
                {
                    JobMtl.TotalCost = 0;
                    JobMtl.MtlBurCost = 0;
                    JobMtl.MaterialMtlCost = 0;
                    JobMtl.MaterialLabCost = 0;
                    JobMtl.MaterialSubCost = 0;
                    JobMtl.MaterialBurCost = 0;
                }
                else
                {
                    if (PrevQty != 0 && (StringExtensions.Compare(JobMtl.BaseUOM, NonConf.ScrapUM) != 0))
                    {
                        this.LibAppService.UOMConv(JobMtl.PartNum, PrevQty, NonConf.ScrapUM, JobMtl.BaseUOM, out prevConvQty, false);
                    }
                    else
                    {
                        prevConvQty = PrevQty;
                    }

                    ndec = this.LibGetDecimalsNumber.getDecimalsNumberByName("JobMtl", "TotalCost", "");
                    JobMtl.MaterialMtlCost = Math.Round(JobMtl.MaterialMtlCost + (prevConvQty * PrevMaterialMtlUnitCost), ndec, MidpointRounding.AwayFromZero);
                    JobMtl.MaterialLabCost = Math.Round(JobMtl.MaterialLabCost + (prevConvQty * PrevMaterialLabUnitCost), ndec, MidpointRounding.AwayFromZero);
                    JobMtl.MaterialSubCost = Math.Round(JobMtl.MaterialSubCost + (prevConvQty * PrevMaterialSubUnitCost), ndec, MidpointRounding.AwayFromZero);
                    JobMtl.MaterialBurCost = Math.Round(JobMtl.MaterialBurCost + (prevConvQty * PrevMaterialBurUnitCost), ndec, MidpointRounding.AwayFromZero);
                    JobMtl.TotalCost = JobMtl.MaterialMtlCost + JobMtl.MaterialLabCost + JobMtl.MaterialSubCost + JobMtl.MaterialBurCost;
                    JobMtl.MtlBurCost = Math.Round(JobMtl.MtlBurCost + (prevConvQty * PrevMtlBurUnitCost), ndec, MidpointRounding.AwayFromZero);
                }
                Db.Release(ref JobMtl);
            }

            /* ADJUST COST FOR JOBMTL AND PARTTRAN WITH THE JOBS AVERAGE COST - NEGATIVE ISSUE */
            JobMtl = this.FindFirstJobMtlWithUpdLock(Session.CompanyID, PartTran.JobNum, PartTran.AssemblySeq, PartTran.JobSeq);
            if (JobMtl == null)
            {
                throw new BLException(Strings.JobsMaterInforIsNotOnFileCannotIssue);
            }
            /* UPDATE JOBMTL */
            /* PartTran.TranQty is in PartTran.UM which might not be the same as the JobMtl.IUM so we may need to convert
            the quantity. JobMtl costs should still be based on PartTran.TranQty because JobMtl costs are per Unit in Base UOM */
            if (StringExtensions.Compare(PartTran.UM, JobMtl.IUM) != 0)
            {
                if (StringExtensions.Compare(JobMtl.IUM, PartTran.ActTransUOM) == 0)
                {
                    partTranConvQty = PartTran.ActTranQty;
                }
                else
                {
                    this.LibAppService.UOMConv(JobMtl.PartNum, PartTran.TranQty, PartTran.UM, JobMtl.IUM, out partTranConvQty, false);
                }
            }
            else
            {
                partTranConvQty = PartTran.TranQty;
            }

            JobMtl.IssuedQty = JobMtl.IssuedQty - partTranConvQty;
            JobMtl.IssuedComplete = (JobMtl.IssuedQty >= JobMtl.RequiredQty);
            ndec = this.LibGetDecimalsNumber.getDecimalsNumberByName("JobMtl", "MaterialMtlCost", "");
            if (JobMtl.IssuedQty == 0)
            {
                JobMtl.TotalCost = 0;
                JobMtl.MtlBurCost = 0;
                JobMtl.MaterialMtlCost = 0;
                JobMtl.MaterialLabCost = 0;
                JobMtl.MaterialSubCost = 0;
                JobMtl.MaterialBurCost = 0;
            }
            else
            {
                JobMtl.MaterialMtlCost = Math.Round(JobMtl.MaterialMtlCost - (PartTran.TranQty * PartTran.MtlUnitCost), ndec, MidpointRounding.AwayFromZero);
                JobMtl.MaterialLabCost = Math.Round(JobMtl.MaterialLabCost - (PartTran.TranQty * PartTran.LbrUnitCost), ndec, MidpointRounding.AwayFromZero);
                JobMtl.MaterialSubCost = Math.Round(JobMtl.MaterialSubCost - (PartTran.TranQty * PartTran.SubUnitCost), ndec, MidpointRounding.AwayFromZero);
                JobMtl.MaterialBurCost = Math.Round(JobMtl.MaterialBurCost - (PartTran.TranQty * PartTran.BurUnitCost), ndec, MidpointRounding.AwayFromZero);
                JobMtl.TotalCost = JobMtl.MaterialMtlCost + JobMtl.MaterialLabCost + JobMtl.MaterialSubCost + JobMtl.MaterialBurCost;
                JobMtl.MtlBurCost = Math.Round(JobMtl.MtlBurCost - (PartTran.TranQty * PartTran.MtlBurUnitCost), ndec, MidpointRounding.AwayFromZero);
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void validateAssemblySeq(string pcJobNum, int piAssemblySeq, string pcTranType, Guid pcSysRowID, string pcFieldName, string pcFromToTag)
        {
            string cTranType = string.Empty;
            bool lAssemblySeqSensitive = false;
            cTranType = ((StringExtensions.Compare(pcFromToTag, "From") == 0) ? "ASM-STK" : "STK-ASM");

            /* Check to see if AssemblySeq is applicable for this record.  If not then return.*/
            if (piAssemblySeq == 0)
            {
                if (StringExtensions.Compare(pcFromToTag, "From") == 0)
                {
                    lAssemblySeqSensitive = this.isSensitive("ttIssueReturn.FromAssemblySeq", pcSysRowID);
                }
                else
                {
                    lAssemblySeqSensitive = this.isSensitive("ttIssueReturn.ToAssemblySeq", pcSysRowID);
                }

                if (!lAssemblySeqSensitive)
                {
                    return;
                }
            }/* if piAssemblySeq = 0 */
            if (StringExtensions.Compare(pcTranType, cTranType) == 0 && piAssemblySeq == 0)
            {
                ExceptionManager.AddBLException(Strings.YouCannotIssuePartsToTheTopLevelAssem, "IssueReturn", "pcFieldName", pcSysRowID);
            }

            if (!(this.ExistsJobAsmbl3(Session.CompanyID, pcJobNum, piAssemblySeq)))
            {
                int assmSeq = 0;
                assmSeq = this.FindFirstJobAsmblSeq(Session.CompanyID, pcJobNum, piAssemblySeq);
                if (assmSeq == 0)
                    ExceptionManager.AddBLException(Strings.AValidAssemblyIsRequired_0(pcFromToTag), "IssueReturn", "pcFieldName", pcSysRowID);
                else
                {
                    ttIssueReturn.FromAssemblySeq = assmSeq;
                }
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void validateJobNum(string pcJobNum, Guid pcSysRowID, string pcFieldName, string pcFromToTag, string processID)
        {
            bool lJobSensitive = false;
            int iAssSeq = 0;
            /* Check to see if JobNum is applicable for this record.  If not then return.*/
            if (String.IsNullOrEmpty(pcJobNum))
            {
                if (StringExtensions.Compare(pcFromToTag, "From") == 0)
                {
                    lJobSensitive = this.isSensitive("ttIssueReturn.FromJobNum", pcSysRowID);
                }
                else
                {
                    lJobSensitive = this.isSensitive("ttIssueReturn.ToJobNum", pcSysRowID);
                }

                if (!lJobSensitive)
                {
                    return;
                }
            }/* if pcJobNum = "" */

            JobHead = this.FindFirstJobHead(Session.CompanyID, pcJobNum);
            if (JobHead == null)
            {
                throw new BLException(Strings.AValidJobNumberIsRequired_0(pcFromToTag), "IssueReturn", "pcFieldName", pcSysRowID);
            }
            /* Validation: Job must be released */
            if (!JobHead.JobReleased)
            {
                ExceptionManager.AddBLException(Strings.JobHasNotBeenReleaCannotIssue(pcFromToTag), "IssueReturn", "pcFieldName", pcSysRowID);
            }
            /* Validation: Job must not be closed */
            if (JobHead.JobClosed)
            {
                //Except Overlay/Void PCID
                if (!processID.Equals("PkgControlVoidPCID", StringComparison.OrdinalIgnoreCase))
                {
                    ExceptionManager.AddBLException(Strings.JobHasBeenClosedCannotIssue(pcFromToTag), "IssueReturn", "pcFieldName", pcSysRowID);
                }
            }
            /* SCR #2997 - check if the related service call is valid */
            if (StringExtensions.Compare(JobHead.JobType, "SRV") == 0)
            {
                FSCallhd = this.FindFirstFSCallhd(Session.CompanyID, JobHead.CallNum);
                if (FSCallhd == null)
                {
                    ExceptionManager.AddBLException(Strings.AValidServiceCallJobNumberIsRequi, "IssueReturn", "pcFieldName", pcSysRowID);
                }
                if (FSCallhd.Invoiced == true)
                {
                    ExceptionManager.AddBLException(Strings.TheServiceCallHasBeenInvoiCannotIssue, "IssueReturn", "pcFieldName", pcSysRowID);
                }
            } /* if JobHead.JobType = "SRV":U */
            if (StringExtensions.Compare(ttIssueReturn.TranType, "ASM-STK") == 0)
            {
                iAssSeq = -1;

                JobAsmbl = this.FindFirstJobAsmblDiffAsm(Session.CompanyID, pcJobNum, 0);
                if (JobAsmbl != null)
                {
                    iAssSeq = JobAsmbl.AssemblySeq;
                }
                if (iAssSeq == -1 && StringExtensions.Compare(pcFromToTag, "From") == 0)
                {
                    ExceptionManager.AddBLException(Strings.TheJobDoNotHaveAnyAssembly, "IssueReturn", "pcFieldName", pcSysRowID);
                }
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void validateJobSeq(string pcJobNum, int piAssemblySeq, int piJobSeq, string pcType, Guid pcSysRowID, string pcFieldName, string pcFromToTag)
        {
            bool lJobSeqSensitive = false;
            if (StringExtensions.Compare(ttIssueReturn.TranType, "INS-SUB") == 0)
            {
                return;    /* Check to see if JobSeq is applicable for this record.  If not then return.*/
            }

            if (piJobSeq == 0)
            {
                if (StringExtensions.Compare(pcFromToTag, "From") == 0)
                {
                    lJobSeqSensitive = this.isSensitive("ttIssueReturn.FromJobSeq", pcSysRowID);
                }
                else
                {
                    lJobSeqSensitive = this.isSensitive("ttIssueReturn.ToJobSeq", pcSysRowID);
                }

                if (!lJobSeqSensitive)
                {
                    return;
                }
            }/* if pcJobSeq = 0 */
            if (StringExtensions.Compare(pcType, "MTL") == 0)
            {
                if (!(this.ExistsJobMtl(Session.CompanyID, pcJobNum, piAssemblySeq, piJobSeq)))
                {
                    ExceptionManager.AddBLException(Strings.AValidJobMaterialIsRequired, "IssueReturn", "pcFieldName", pcSysRowID);
                }

                /* SCR #2210 - do not issue from Miscellaneous Charge type of JobMtl */
                if ((this.ExistsJobMtl(Session.CompanyID, pcJobNum, piAssemblySeq, piJobSeq, true)))
                {
                    ExceptionManager.AddBLException(Strings.MaterIsAJobMisceChargeCannotIssue, "IssueReturn", "pcFieldName", pcSysRowID);
                }
            } /* Mtl-Block */
            if (StringExtensions.Compare(pcType, "OPR") == 0)
            {
                if (piJobSeq != 0 &&
                !(this.ExistsJobOper2(Session.CompanyID, pcJobNum, piAssemblySeq, piJobSeq)))
                {
                    ExceptionManager.AddBLException(Strings.AValidJobOperationIsRequired, "IssueReturn", "pcFieldName", pcSysRowID);
                }
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void validateLotNum(bool plPartTrackLots, string pcPartNum, string pcLotNum, Guid pcRowIdent)
        {
            /* if isSensitive */
            if (this.isSensitive("ttIssueReturn.LotNum", pcRowIdent))
            {
                if (plPartTrackLots)
                {/* if pcLotNum = "":U */
                    if (String.IsNullOrEmpty(pcLotNum))
                    {
                        if (ttIssueReturn.TranType.Equals("DMR-ASM", StringComparison.OrdinalIgnoreCase) && !JobHead.JobComplete)
                        {
                            return;
                        }
                        else
                        {
                            ExceptionManager.AddBLException(Strings.ALotNumberIsRequired, "IssueReturn", "LotNum", pcRowIdent);
                        }
                    }
                    else
                    {
                        if (!ExistsPartLot(Session.CompanyID, pcPartNum, pcLotNum))
                            ExceptionManager.AddBLException(Strings.AValidLotNumberIsRequired, "IssueReturn", "LotNum", pcRowIdent);
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(pcLotNum))
                {
                    //ERPS-99557 WIP adjustment with not empty LotNum from subcontract operation;
                    if (StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-WIP") == 0)
                    {
                        var JobOperColumnsRes = FindFirstJobOper2(ttIssueReturn.Company, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq);
                        if (JobOperColumnsRes != null && JobOperColumnsRes.SubContract)
                            return;
                    }
                    ExceptionManager.AddBLException(Strings.ThePartDoesNotRequireALotNumber, "IssueReturn", "LotNum", pcRowIdent);
                }
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void validatePartNum(IssueReturnRow ttIssueReturn)
        {
            string cFromType = string.Empty;
            string cToType = string.Empty;
            bool lPartNumSensitive = false;
            cFromType = getFromType(ttIssueReturn.TranType);
            cToType = getToType(ttIssueReturn.TranType);

            /* Check to see if PartNum is applicable for this record.  If not then return.*/
            ttIssueReturn.PartNum = ttIssueReturn.PartNum.Trim();
            if (String.IsNullOrEmpty(ttIssueReturn.PartNum))
            {
                lPartNumSensitive = isSensitive("ttIssueReturn.PartNum", ttIssueReturn.SysRowID);
                if (!lPartNumSensitive)
                    return;
            }

            if (String.IsNullOrEmpty(ttIssueReturn.PartNum))
            {
                ExceptionManager.AddBLException(Strings.AValidPartNumberIsRequired_0, "IssueReturn", "PartNum", ttIssueReturn.SysRowID);
                return;
            }

            /* Validation: If its a stock transaction, then it must be defined in the Part table */
            if (StringExtensions.Compare(cFromType, "STK") == 0 || StringExtensions.Compare(cToType, "STK") == 0)
            {
                if (!ExistsPart(Session.CompanyID, ttIssueReturn.PartNum))
                {
                    ExceptionManager.AddBLException(Strings.AValidPartNumberIsRequired_0, "IssueReturn", "PartNum", ttIssueReturn.SysRowID);
                    return;
                }

                if (ExistsInactivePart(Session.CompanyID, ttIssueReturn.PartNum, true))
                    ExceptionManager.AddBLException(Strings.PartIsInactive(ttIssueReturn.PartNum), "IssueReturn", "PartNum", ttIssueReturn.SysRowID);

                if (ttIssueReturn.TranType.Equals("STK-MTL", StringComparison.OrdinalIgnoreCase))
                {
                    if (!ttIssueReturn.PartNum.KeyEquals(ttIssueReturn.ToJobSeqPartNum) && Part != null)
                    {
                        var UOMClass = ExistsUOMClassForPart(ttIssueReturn.Company, ttIssueReturn.ToJobSeqPartNum);

                        // We cannot issue a different part if it does not have the same UOM Class or the same UM of the Job Requirement
                        if (!UOMClass.KeyEquals(Part.UOMClassID) && !ExistsUOMConv(Session.CompanyID, Part.UOMClassID, ttIssueReturn.RequirementUOM))
                            ExceptionManager.AddBLException(Strings.CannotChangeUOMClass(Part.UOMClassID, UOMClass));
                    }
                }

                /* if this is a return, validate the part was issued to the job */
                if (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") == 0)
                {
                    if (!ExistsPartTran(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, "M", ttIssueReturn.FromJobSeq, ttIssueReturn.PartNum))
                        ExceptionManager.AddBLException(Strings.ThePartHasNotBeenIssuedToTheJob, "IssueReturn", "PartNum", ttIssueReturn.SysRowID);
                }
            }

            MtlQueue = FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
            if (MtlQueue != null)
            {
                if (StringExtensions.Compare(ttIssueReturn.PartNum, MtlQueue.PartNum) != 0)
                {
                    ExceptionManager.AddBLException(Strings.ThePartNumberCannotBeChanged, "IssueReturn", "PartNum", ttIssueReturn.SysRowID);
                }

                if (StringExtensions.Compare(ttIssueReturn.TranType, "PUR-SHP") == 0 &&
                    ttIssueReturn.AttributeSetID != MtlQueue.AttributeSetID)
                {
                    ExceptionManager.AddBLException(Strings.BTOAttribute, "IssueReturn", "PartNum", ttIssueReturn.SysRowID);

                }
            }
        }

        ///<summary>
        ///  Purpose: Checks that the from fields reference a PartWip record.
        ///  Parameters:  none
        ///  Notes: initially created for WIP-WIP transaction.
        ///</summary>
        private void validatePartWip()
        {
            using (var libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
            {
                //Validate PartWip row exists related to WIP/Material movement
                switch (ttIssueReturn.TranType.ToUpperInvariant())
                {
                    case "STK-MTL":
                    case "STK-ASM":
                        {
                            libPackageControlValidations.ValidateStockToWIPPCID(ttIssueReturn.PartNum, ttIssueReturn.PartIUM, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, ttIssueReturn.FromPCID, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.ToPCID, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, ttIssueReturn.TranQty, ttIssueReturn.UM);
                            break;
                        }
                    case "MTL-MTL":
                        {
                            PartWip = FindFirstPartWipMaterial(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, ttIssueReturn.FromPCID);
                            if (PartWip == null)
                            {
                                ExceptionManager.AddBLException(Strings.APartTrackRecordDoesNotExistForTheGivenFromValues);
                            }
                            else
                            {
                                //isStatusChange is false for Move Material (WIP->WIP)
                                libPackageControlValidations.ValidateWIPPCID(PartWip, ttIssueReturn.FromPCID, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.ToPCID, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, ttIssueReturn.TranQty, ttIssueReturn.UM, false);
                            }

                            libPackageControlValidations.ValidatePCIDMovementLocationInfo(ttIssueReturn.PartNum, ttIssueReturn.FromPCID, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.ToPCID, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum);

                            break;
                        }
                    case "WIP-WIP":
                        {
                            //DJY - If this is a PCID movement, do not need to validate the individual PartWip to be
                            //moved because we will be moving all of the PartWip items associated with the PCID
                            //We also do not validate the PartWip if this is an unpick transaction for a make direct sales
                            //order release. The user must be allowed to unpick even if in scenarios where there isn't a matching PartWip.
                            if (!libPackageControlValidations.IsPCIDMovement(ttIssueReturn.FromPCID, ttIssueReturn.ToPCID, ttIssueReturn.PartNum) &&
                                !ttIssueReturn.ProcessID.Equals("UNPICK", StringComparison.OrdinalIgnoreCase))
                            {
                                PartWip = FindFirstPartWipFinishedGood(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, ttIssueReturn.FromPCID);
                                if (PartWip == null)
                                {
                                    ExceptionManager.AddBLException(Strings.APartTrackRecordDoesNotExistForTheGivenFromValues);
                                }
                            }
                            if (PartWip != null)
                            {
                                //isStatusChange is false for Move WIP (WIP->WIP)
                                libPackageControlValidations.ValidateWIPPCID(PartWip, ttIssueReturn.FromPCID, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.ToPCID, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, ttIssueReturn.TranQty, ttIssueReturn.UM, false);
                            }

                            libPackageControlValidations.ValidatePCIDMovementLocationInfo(ttIssueReturn.PartNum, ttIssueReturn.FromPCID, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.ToPCID, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum);

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private void ValidatePCIDForMovement(bool enablePackageControl, string pcid, string tranType)
        {
            using (var libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
            {
                Erp.Internal.Lib.PackageControlValidations.PkgControlHeaderPartialRow pkgControlHeaderPartial = libPackageControlValidations.ValidatePCID(
                    ttIssueReturn.Company,
                    pcid,
                    ttIssueReturn.Plant,
                    null,
                    null);

                if (tranType.Equals("PUR-STK", StringComparison.OrdinalIgnoreCase) && !pkgControlHeaderPartial.PkgControlStatus.Equals("STOCK", StringComparison.OrdinalIgnoreCase))
                    throw new BLException(Strings.PCIDStatusMustBeStockForPUR_STK);

                libPackageControlValidations.PCIDIsNotAChildValidation(Session.CompanyID, pcid);
                ExceptionManager.AssertNoBLExceptions();
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:  this procedure presumes that ttIssueRturn row is available already
        ///</summary>
        private void validateSerialNumber(string SerialNum, Erp.Tables.SerialNo SerialNo, out string ErrMessage)
        {
            ErrMessage = string.Empty;
            string formatErrMessage = string.Empty;
            string validstatus = string.Empty;
            bool statusError = false;
            bool checkSNalloc = false;
            string opMessage = string.Empty;
            bool binToBin = false;
            /* validations will depend on the process being run */
            switch (ttIssueReturn.ProcessID.ToUpperInvariant())
            {
                case "ISSUEMATERIAL":
                case "HHISSUEMATERIAL":
                    {
                        if (ttIssueReturn.SerialControlPlantIsFromPlt == true)
                        {
                            if (SerialNo != null)
                            {
                                if (SerialNo.Voided == true)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                                }

                                if (SerialNo.Scrapped == true)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsScrapped + Ice.Constants.LIST_DELIM;
                                }

                                if (!SerialNo.SNStatus.Equals("INVENTORY", StringComparison.OrdinalIgnoreCase))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotHaveTheStatusOfInven + Ice.Constants.LIST_DELIM;
                                }

                                if (!SerialNo.WareHouseCode.KeyEquals(ttIssueReturn.FromWarehouseCode))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistInInvenAtThisWareh + Ice.Constants.LIST_DELIM;
                                }

                                binToBin = this.ExistsBinToBinReqSNPlantConfCtrl(ttIssueReturn.Company, ttIssueReturn.FromJobPlant, true);
                                if (binToBin)
                                {
                                    if (!SerialNo.BinNum.KeyEquals(ttIssueReturn.FromBinNum))
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistInInvenInThisBin + Ice.Constants.LIST_DELIM;
                                    }
                                }
                                checkSNalloc = true;
                                if (!SerialNo.PCID.KeyEquals(ttIssueReturn.FromPCID))
                                {
                                    ErrMessage = ErrMessage + (String.IsNullOrEmpty(ttIssueReturn.FromPCID) ? Strings.ThisSerialNumberForPCID(SerialNo.PCID) : Strings.ThisSerialNumberNotOnPCID) + Ice.Constants.LIST_DELIM;
                                }
                                // if issue is from a PCID the SerialNo.LotNum has to match the PCID lot number from which the issue is made due to design limitations for SerialNo in regards to LotNum
                                if ((!String.IsNullOrEmpty(ttIssueReturn.LotNum)) && (!String.IsNullOrEmpty(ttIssueReturn.FromPCID)) && (!SerialNo.LotNum.KeyEquals(ttIssueReturn.LotNum)))
                                {
                                    ErrMessage = ErrMessage + Strings.SerialLotNumMustMatchIssueFromLotForPCID + Ice.Constants.LIST_DELIM;
                                }
                            }
                            else
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                            }
                        }
                        else
                        {
                            if (SerialNo != null)
                            {
                                if (SerialNo.Voided == false)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberAlreadyExistsInInven + Ice.Constants.LIST_DELIM;
                                    ErrMessage = ErrMessage + Strings.ExistSerialNumberMustBeFlaggedAsVoidedToBeValid + Ice.Constants.LIST_DELIM;
                                }
                            }
                            else if (ttIssueReturn.ReassignSNAsm)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                            }
                            if ((SerialNo != null) && (!String.IsNullOrEmpty(SerialNo.PCID)))
                            {
                                ErrMessage = ErrMessage + Strings.CannotSelectSerialNoWithPCID + Ice.Constants.ListSeparator;
                            }
                        }
                    }
                    break;/* end IssueMaterial and HHIssueMaterial */
                case "ISSUEASSEMBLY":
                case "HHISSUEASSEMBLY":
                    {
                        if (ttIssueReturn.SerialControlPlantIsFromPlt == true)
                        {
                            if (SerialNo != null)
                            {
                                if (SerialNo.Voided == true)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                                }

                                if (SerialNo.Scrapped == true)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsScrapped + Ice.Constants.LIST_DELIM;
                                }

                                if (!SerialNo.SNStatus.Equals("INVENTORY", StringComparison.OrdinalIgnoreCase))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotHaveTheStatusOfInven + Ice.Constants.LIST_DELIM;
                                }

                                if (!SerialNo.WareHouseCode.KeyEquals(ttIssueReturn.FromWarehouseCode))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistInInvenAtThisWareh + Ice.Constants.LIST_DELIM;
                                }

                                binToBin = this.ExistsBinToBinReqSNPlantConfCtrl(ttIssueReturn.Company, ttIssueReturn.FromJobPlant, true);
                                if (binToBin)
                                {
                                    if (!SerialNo.BinNum.KeyEquals(ttIssueReturn.FromBinNum))
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistInInvenInThisBin + Ice.Constants.LIST_DELIM;
                                    }
                                }
                                if (!SerialNo.PCID.KeyEquals(ttIssueReturn.FromPCID))
                                {
                                    ErrMessage = ErrMessage + (String.IsNullOrEmpty(ttIssueReturn.FromPCID) ? Strings.ThisSerialNumberForPCID(SerialNo.PCID) : Strings.ThisSerialNumberNotOnPCID) + Ice.Constants.LIST_DELIM;
                                }
                                // if issue is from a PCID the SerialNo.LotNum has to match the PCID lot number from which the issue is made due to design limitations for SerialNo in regards to LotNum
                                if ((!String.IsNullOrEmpty(ttIssueReturn.LotNum)) && (!String.IsNullOrEmpty(ttIssueReturn.FromPCID)) && (!SerialNo.LotNum.KeyEquals(ttIssueReturn.LotNum)))
                                {
                                    ErrMessage = ErrMessage + Strings.SerialLotNumMustMatchIssueFromLotForPCID + Ice.Constants.LIST_DELIM;
                                }
                            }
                            else
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                            }
                        }
                        else
                        {
                            if (SerialNo != null)
                            {
                                if (SerialNo.Voided == false)
                                {
                                    ErrMessage = ErrMessage + Strings.ExistSerialNumberMustBeFlaggedAsVoidedToBeValid + Ice.Constants.LIST_DELIM;
                                }
                            }
                            else if (ttIssueReturn.ReassignSNAsm)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                            }
                            if ((SerialNo != null) && (!String.IsNullOrEmpty(SerialNo.PCID)))
                            {
                                ErrMessage = ErrMessage + Strings.CannotSelectSerialNoWithPCID + Ice.Constants.ListSeparator;
                            }
                        }
                    }
                    break;/* END IssueAssembly AND HHIssueAssembly */
                case "ISSUEMISCELLANEOUSMATERIAL":
                    {
                        if (SerialNo != null)
                        {
                            if (SerialNo.Voided == true)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                            }

                            if (SerialNo.Scrapped == true)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsScrapped + Ice.Constants.LIST_DELIM;
                            }

                            if (!SerialNo.SNStatus.Equals("INVENTORY", StringComparison.OrdinalIgnoreCase))
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotHaveTheStatusOfInven + Ice.Constants.LIST_DELIM;
                            }

                            if (!SerialNo.WareHouseCode.KeyEquals(ttIssueReturn.FromWarehouseCode))
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistInInvenAtThisWareh + Ice.Constants.LIST_DELIM;
                            }

                            binToBin = this.ExistsBinToBinReqSNPlantConfCtrl(ttIssueReturn.Company, ttIssueReturn.FromJobPlant, true);
                            if (binToBin)
                            {
                                if (!SerialNo.BinNum.KeyEquals(ttIssueReturn.FromBinNum))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistInInvenInThisBin + Ice.Constants.LIST_DELIM;
                                }
                            }
                            if (!String.IsNullOrEmpty(SerialNo.PCID))
                            {
                                ErrMessage = ErrMessage + Strings.CannotSelectSerialNoWithPCID + Ice.Constants.ListSeparator;
                            }
                        }
                        else
                        {
                            ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                        }
                    }
                    break;
                case "RETURNMATERIAL":
                case "HHRETURNMATERIAL":
                    {
                        if (ttIssueReturn.SerialControlPlantIsFromPlt == true)
                        {
                            if (SerialNo != null)
                            {
                                if (SerialNo.Voided == true)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                                }

                                if (SerialNo.Scrapped == true)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsScrapped + Ice.Constants.LIST_DELIM;
                                }

                                if (StringExtensions.Compare(SerialNo.SNStatus, "WIP") != 0 && StringExtensions.Compare(SerialNo.SNStatus, "CONSUMED") != 0)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotHaveTheStatusOfWIPOrCONSU + Ice.Constants.LIST_DELIM;
                                }

                                if ((StringExtensions.Compare(SerialNo.JobNum, ttIssueReturn.FromJobNum) != 0 || SerialNo.AssemblySeq != ttIssueReturn.FromAssemblySeq))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsNotIssuedToThisJobAssem + Ice.Constants.LIST_DELIM;
                                }
                            }
                            else
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                            }
                        }
                        else
                        {
                            if (SerialNo != null)
                            {
                                if (SerialNo.Voided == false)
                                {
                                    ErrMessage = ErrMessage + Strings.ExistSerialNumberMustBeFlaggedAsVoidedToBeValid + Ice.Constants.LIST_DELIM;
                                }
                            }
                        }

                    }
                    break;/* end Return Material or HHReturnMaterial */
                case "RETURNASSEMBLY":
                case "HHRETURNASSEMBLY":
                    {
                        if (ttIssueReturn.SerialControlPlantIsFromPlt == true)
                        {
                            if (SerialNo != null)
                            {
                                if (SerialNo.Voided == true)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                                }

                                if (SerialNo.Scrapped == true)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsScrapped + Ice.Constants.LIST_DELIM;
                                }

                                if (StringExtensions.Compare(SerialNo.SNStatus, "WIP") != 0 && StringExtensions.Compare(SerialNo.SNStatus, "CONSUMED") != 0)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotHaveTheStatusOfWIPOrCONSU + Ice.Constants.LIST_DELIM;
                                }

                                if ((StringExtensions.Compare(SerialNo.JobNum, ttIssueReturn.FromJobNum) != 0 || SerialNo.AssemblySeq != ttIssueReturn.FromAssemblySeq))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsNotIssuedToThisJobAssem + Ice.Constants.LIST_DELIM;
                                }
                            }
                            else
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                            }
                        }
                        else
                        {
                            if (SerialNo != null)
                            {
                                if (SerialNo.Voided == false)
                                {
                                    ErrMessage = ErrMessage + Strings.ExistSerialNumberMustBeFlaggedAsVoidedToBeValid + Ice.Constants.LIST_DELIM;
                                }
                            }
                        }

                        using (Internal.Lib.PackageControl libPackageControl = new Internal.Lib.PackageControl(Db))
                        {
                            string pkgControlStatus = libPackageControl.GetPCIDStatus(Session.CompanyID, SerialNo.PCID);
                            if (!pkgControlStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase))
                            {
                                if ((SerialNo != null) && (!String.IsNullOrEmpty(SerialNo.PCID)))
                                {
                                    ErrMessage = ErrMessage + Strings.CannotSelectSerialNoWithPCID + Ice.Constants.ListSeparator;
                                }
                            }
                        }
                    }
                    break;/* end ReturnAssembly or HHReturnAssembly */
                case "RETURNMISCELLANEOUSMATERIAL":
                    {
                        if (SerialNo != null)
                        {
                            if (SerialNo.Voided == true)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                            }

                            if (SerialNo.Scrapped == true)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsScrapped + Ice.Constants.LIST_DELIM;
                            }

                            if (StringExtensions.Compare(SerialNo.SNStatus, "MISC-ISSUE") != 0)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotHaveTheStatusOfMISCISSUE + Ice.Constants.LIST_DELIM;
                            }
                            if (!String.IsNullOrEmpty(SerialNo.PCID))
                            {
                                ErrMessage = ErrMessage + Strings.CannotSelectSerialNoWithPCID + Ice.Constants.ListSeparator;
                            }
                        }
                        else
                        {
                            ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                        }
                    }
                    break;/* end ReturnMiscellaneousMaterial */
                case "ADJUSTWIP":
                    {
                        if (SerialNo != null)
                        {
                            if (SerialNo.Voided == true)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                            }

                            if (StringExtensions.Compare(SerialNo.SNStatus, "WIP") != 0 && StringExtensions.Compare(SerialNo.SNStatus, "CONSUMED") != 0)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotHaveTheStatusOfWIPOrCONSU + Ice.Constants.LIST_DELIM;
                            }

                            if ((StringExtensions.Compare(SerialNo.JobNum, ttIssueReturn.ToJobNum) != 0 || SerialNo.AssemblySeq != ttIssueReturn.ToAssemblySeq))
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsNotIssuedToThisJobAssem + Ice.Constants.LIST_DELIM;
                            }

                            using (Internal.Lib.PackageControl libPackageControl = new Internal.Lib.PackageControl(Db))
                            {
                                string pkgControlStatus = libPackageControl.GetPCIDStatus(Session.CompanyID, SerialNo.PCID);
                                if (!pkgControlStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase) && !pkgControlStatus.Equals("WIPFG", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!String.IsNullOrEmpty(SerialNo.PCID))
                                    {
                                        ErrMessage = ErrMessage + Strings.CannotSelectSerialNoWithPCID + Ice.Constants.ListSeparator;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                        }
                    }
                    break;/* end AdjustWIP */
                case "MOVEWIP":
                    {
                        if (SerialNo != null)
                        {
                            if (SerialNo.Voided == true)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                            }

                            if ((!SerialNo.SNStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase))
                                && (!SerialNo.SNStatus.Equals("CONSUMED", StringComparison.OrdinalIgnoreCase)))
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotHaveTheStatusOfWIPOrCONSU + Ice.Constants.LIST_DELIM;
                            }

                            if ((!SerialNo.JobNum.KeyEquals(ttIssueReturn.FromJobNum)) || SerialNo.AssemblySeq != ttIssueReturn.FromAssemblySeq)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsNotIssuedToThisJobAssem + Ice.Constants.LIST_DELIM;
                            }

                            using (Internal.Lib.PackageControl libPackageControl = new Internal.Lib.PackageControl(Db))
                            {
                                string pkgControlStatus = libPackageControl.GetPCIDStatus(Session.CompanyID, SerialNo.PCID);
                                if (!pkgControlStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase) && !pkgControlStatus.Equals("WIPFG", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!String.IsNullOrEmpty(SerialNo.PCID))
                                    {
                                        ErrMessage = ErrMessage + Strings.CannotSelectSerialNoWithPCID + Ice.Constants.ListSeparator;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                        }
                    }
                    break;/* end MoveWIP */
                case "ADJUSTMATERIAL":
                    {
                        if (SerialNo != null)
                        {
                            if (SerialNo.Voided == true)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                            }

                            if (StringExtensions.Compare(SerialNo.SNStatus, "WIP") != 0 && StringExtensions.Compare(SerialNo.SNStatus, "CONSUMED") != 0)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotHaveTheStatusOfWIPOrCONSU + Ice.Constants.LIST_DELIM;
                            }

                            if ((StringExtensions.Compare(SerialNo.JobNum, ttIssueReturn.ToJobNum) != 0 || SerialNo.AssemblySeq != ttIssueReturn.ToAssemblySeq) ||
                            SerialNo.MtlSeq != ttIssueReturn.ToJobSeq)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsNotIssuedToThisJobAssemMtl + Ice.Constants.LIST_DELIM;
                            }

                            using (Internal.Lib.PackageControl libPackageControl = new Internal.Lib.PackageControl(Db))
                            {
                                string pkgControlStatus = libPackageControl.GetPCIDStatus(Session.CompanyID, SerialNo.PCID);
                                if (!pkgControlStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!String.IsNullOrEmpty(SerialNo.PCID))
                                    {
                                        ErrMessage = ErrMessage + Strings.CannotSelectSerialNoWithPCID + Ice.Constants.ListSeparator;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                        }
                    }
                    break;/* end AdjustMaterial */
                case "MOVEMATERIAL":
                    {
                        if (SerialNo != null)
                        {
                            if (SerialNo.Voided == true)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                            }

                            if (StringExtensions.Compare(SerialNo.SNStatus, "WIP") != 0 && StringExtensions.Compare(SerialNo.SNStatus, "CONSUMED") != 0)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotHaveTheStatusOfWIPOrCONSU + Ice.Constants.LIST_DELIM;
                            }

                            if ((StringExtensions.Compare(SerialNo.JobNum, ttIssueReturn.FromJobNum) != 0 || SerialNo.AssemblySeq != ttIssueReturn.FromAssemblySeq) ||
                            SerialNo.MtlSeq != ttIssueReturn.FromJobSeq)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsNotIssuedToThisJobAssemMtl + Ice.Constants.LIST_DELIM;
                            }
                            if (!String.IsNullOrEmpty(SerialNo.PCID))
                            {
                                ErrMessage = ErrMessage + Strings.CannotSelectSerialNoWithPCID + Ice.Constants.ListSeparator;
                            }
                        }
                        else
                        {
                            ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                        }
                    }
                    break;/* end MoveMaterial */
                case "MATERIALQUEUE":
                case "HHMATERIALQUEUE":
                case "HHAUTOSELECTTRANSACTIONS":
                    {
                        if (ttIssueReturn.SerialControlPlantIsFromPlt == true)
                        {
                            if (SerialNo != null)
                            {
                                if (((StringExtensions.Compare(ttIssueReturn.TranType, "DMR-ASM") == 0 && StringExtensions.Compare(SerialNo.WareHouseCode, ttIssueReturn.FromWarehouseCode) != 0)) ||
                                    (StringExtensions.Compare(SerialNo.WareHouseCode, ttIssueReturn.FromWarehouseCode) != 0 &&
                                        StringExtensions.Compare(ttIssueReturn.TranType, "MFG-OPR") != 0 && StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") != 0 &&
                                        StringExtensions.Compare(ttIssueReturn.TranType, "ASM-INS") != 0 && StringExtensions.Compare(ttIssueReturn.TranType, "SUB-INS") != 0 &&
                                        StringExtensions.Compare(ttIssueReturn.TranType, "STK-INS") != 0 && StringExtensions.Compare(ttIssueReturn.TranType, "DMR-ASM") != 0))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistAtThisWareh + Ice.Constants.LIST_DELIM;
                                }

                                this.checkSNStatus(ttIssueReturn.TranType, ttIssueReturn.TranQty, out validstatus);
                                if (StringExtensions.Compare(validstatus, "WIP") == 0)
                                {
                                    if (StringExtensions.Compare(SerialNo.SNStatus, "WIP") != 0 && StringExtensions.Compare(SerialNo.SNStatus, "CONSUMED") != 0)
                                    {
                                        statusError = true;
                                    }
                                }
                                else
                                {
                                    PlantConfCtrl tempPlantConfCtrl = null;
                                    if (ttIssueReturn.SerialControlPlantIsFromPlt && LibSerialCommon.binMovesRequireSN(ttIssueReturn.SerialControlPlant, ref tempPlantConfCtrl) && StringExtensions.Compare(ttIssueReturn.TranType, "ASM-INS") != 0 &&
                                        StringExtensions.Compare(ttIssueReturn.TranType, "SUB-INS") != 0 &&
                                        StringExtensions.Compare(ttIssueReturn.TranType, "STK-INS") != 0)
                                    {
                                        if ((StringExtensions.Compare(ttIssueReturn.TranType, "DMR-ASM") == 0 && StringExtensions.Compare(SerialNo.BinNum, ttIssueReturn.ToBinNum) != 0) ||
                                            (StringExtensions.Compare(ttIssueReturn.TranType, "DMR-ASM") != 0 && StringExtensions.Compare(SerialNo.BinNum, ttIssueReturn.FromBinNum) != 0))
                                        {

                                            ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistInInvenInThisBin + Ice.Constants.LIST_DELIM;
                                        }
                                    }
                                    if (StringExtensions.Compare(SerialNo.SNStatus, validstatus) != 0)
                                    {
                                        statusError = true;
                                    }
                                }
                                if (statusError == true)
                                {
                                    ErrMessage = ErrMessage + Strings.CannotMoveSerialNumberHasInvalidStatusThisTrans(SerialNo.SerialNumber, SerialNo.SNStatus, validstatus) + Ice.Constants.LIST_DELIM;
                                }
                                if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                                {
                                    if (SerialNo.Voided == true)
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                                    }

                                    if ((StringExtensions.Compare(SerialNo.JobNum, ttIssueReturn.FromJobNum) != 0 || SerialNo.AssemblySeq != ttIssueReturn.FromAssemblySeq))
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberIsNotIssuedToThisJobAssem + Ice.Constants.LIST_DELIM;
                                    }
                                }
                                if (StringExtensions.Compare(ttIssueReturn.TranType, "INS-STK") == 0)
                                {
                                    MtlQueue = this.FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
                                    if (MtlQueue != null)
                                    {
                                        if (MtlQueue.RMADisp > 0)
                                        {
                                            if (!(SerialNo.RMANum == MtlQueue.RMANum &&
                                            SerialNo.RMALine == MtlQueue.RMALine &&
                                            SerialNo.RMADisp == MtlQueue.RMADisp))
                                            {
                                                ErrMessage = ErrMessage + Strings.ThisSerialNumberWasNotSelecForTheDispoForThisMater + Ice.Constants.LIST_DELIM;
                                            }
                                        }
                                    }
                                }
                                // cannot currently be assigned to a PCID, and the "to pcid" if there is one will get added to the SerialNo during update.

                                // PCID is OK for RMA receipt to Inspection
                                if ((!ExistsMtlQueue(ttIssueReturn.MtlQueueRowId, "RMA-INS")) && (!String.IsNullOrEmpty(SerialNo.PCID)))
                                {
                                    ErrMessage = ErrMessage + Strings.CannotSelectSerialNoWithPCID + Ice.Constants.ListSeparator;
                                }
                                // if the SerialNo exists with a blank PCID and the transaction ToPCID is not blank, the Serial Number cannot be added 
                                // to a PCID if there is an open cycle count tag for the part/SerialNumber. SCR-171025
                                if (String.IsNullOrEmpty(SerialNo.PCID) && (!String.IsNullOrEmpty(ttIssueReturn.ToPCID)) &&
                                    (ExistsCCTag(ttIssueReturn.Company, SerialNo.PartNum, SerialNo.SerialNumber, 0)))
                                {
                                    ErrMessage = ErrMessage + Strings.CannotSelectSerialOnOpenCycleCount + Ice.Constants.ListSeparator;
                                }
                            }
                            else
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                            }
                            if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-SHP") == 0)
                            {
                                checkSNalloc = true;
                            }
                        } // if (ttIssueReturn.SerialControlPlantIsFromPlt == true)
                        else
                        {
                            bool voidedOrNewOnly = (!ttIssueReturn.SerialControlPlantIsFromPlt) && (toPCIDIsOutboundPCIDInOutboundPlant(ttIssueReturn));
                            if (voidedOrNewOnly)
                            {
                                if (SerialNo != null)
                                {
                                    if (SerialNo.Voided == false)
                                    {
                                        ErrMessage = ErrMessage + Strings.OnlyVoidedOrNewSerialNumbersAllowed + Ice.Constants.LIST_DELIM;
                                    }
                                }
                            }
                            else
                            {
                                if (SerialNo == null)
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                            }
                        }
                    }
                    break;
                case "UNPICK":
                case "UNPICKPCID":
                    {
                        if (ttIssueReturn.SerialControlPlantIsFromPlt)
                        {
                            if (SerialNo != null)
                            {
                                if (SerialNo.OrderNum != ttIssueReturn.OrderNum ||
                                    SerialNo.OrderLine != ttIssueReturn.OrderLine ||
                                    SerialNo.OrderRelNum != ttIssueReturn.OrderRel)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberNotOnThisOrderRelease + Ice.Constants.LIST_DELIM;
                                }

                                if (!SerialNo.JobNum.KeyEquals(ttIssueReturn.FromJobNum))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberForJob(SerialNo.JobNum) + Ice.Constants.LIST_DELIM;
                                }

                                if (String.IsNullOrEmpty(ttIssueReturn.FromPCID))
                                {
                                    if (!SerialNo.SNStatus.Equals("PICKED", StringComparison.OrdinalIgnoreCase))
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberStatusNotPicked + Ice.Constants.LIST_DELIM;
                                    }

                                    if (!SerialNo.PCID.KeyEquals(ttIssueReturn.FromPCID))
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberForPCID(SerialNo.PCID) + Ice.Constants.LIST_DELIM;
                                    }
                                }
                                else
                                {
                                    //You can unpick a PCID that is in PICKED or PICKING (i.e. INVENTORY)
                                    if (!SerialNo.SNStatus.Equals("PICKED", StringComparison.OrdinalIgnoreCase) &&
                                        !SerialNo.SNStatus.Equals("INVENTORY", StringComparison.OrdinalIgnoreCase))
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberStatusNotPickedOrInventory + Ice.Constants.LIST_DELIM;
                                    }

                                    if (!SerialNo.PCID.KeyEquals(ttIssueReturn.FromPCID))
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberNotOnPCID + Ice.Constants.LIST_DELIM;
                                    }
                                }

                                //Not sure that WareHouseCode should matter, kept condition to retain existing functionality
                                if (!SerialNo.WareHouseCode.KeyEquals(ttIssueReturn.FromWarehouseCode))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistAtThisWareh + Ice.Constants.LIST_DELIM;
                                }

                                //Not sure that BinNum should matter, kept condition to retain existing functionality
                                if (!SerialNo.BinNum.KeyEquals(ttIssueReturn.FromBinNum))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistInThisBin + Ice.Constants.LIST_DELIM;
                                }

                            }
                            else
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                            }
                        }
                    }
                    break;

                case "PKGCONTROLIDBUILDSPLITMERGE":
                    int plantSerialTracking = LibSerialCommon.isSerTraPlantType(ttIssueReturn.SerialControlPlant);

                    // Common validations for all cases
                    if (SerialNo != null)
                    {
                        if (StringExtensions.Compare(SerialNo.SNStatus, "INVENTORY") != 0)
                        {
                            ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotHaveTheStatusOfInven + Ice.Constants.LIST_DELIM;
                        }

                        if (!SerialNo.WareHouseCode.KeyEquals(ttIssueReturn.FromWarehouseCode))
                        {
                            ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistAtThisWareh + Ice.Constants.LIST_DELIM;
                        }

                        bool binToBinReqSN = GetPlantBinToBinReqSN(ttIssueReturn.Company, ttIssueReturn.SerialControlPlant);
                        if (binToBinReqSN)
                        {
                            if (!SerialNo.BinNum.KeyEquals(ttIssueReturn.FromBinNum))
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExistInThisBin + Ice.Constants.LIST_DELIM;
                            }
                        }

                        if (!SerialNo.LotNum.KeyEquals(ttIssueReturn.LotNum))
                        {
                            ErrMessage = ErrMessage + Strings.TheSerialNumberDoesNotExistsInThisLot + Ice.Constants.LIST_DELIM;
                        }
                    }

                    // Blank FromPCID means a part is being added to a PCID from inventory
                    if (String.IsNullOrEmpty(ttIssueReturn.FromPCID))
                    {
                        if (SerialNo != null)
                        {
                            if (plantSerialTracking == 3 && GetPkgControlHeaderOutboundContainer(Session.CompanyID, ttIssueReturn.ToPCID) == true)
                            {
                                // Only voided serial numbers can be selected
                                if (SerialNo.Voided == false)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsNotVoided + Ice.Constants.LIST_DELIM;
                                }
                            }
                            else
                            {
                                if (SerialNo.Voided == true)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                                }
                            }

                            if (!String.IsNullOrEmpty(SerialNo.PCID))
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberForPCID(SerialNo.PCID) + Ice.Constants.LIST_DELIM;
                            }
                        }
                        else
                        {
                            // When plantSerialTracking = 3 (outbound serial tracking), the user can create serial numbers
                            if (plantSerialTracking == 2)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                            }
                        }
                    }
                    // Blank ToPCID means a part is being removed from a PCID and put back into inventory
                    else if (String.IsNullOrEmpty(ttIssueReturn.ToPCID))
                    {
                        if (SerialNo != null)
                        {
                            if (!SerialNo.PCID.KeyEquals(ttIssueReturn.FromPCID))
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberNotOnPCID + Ice.Constants.LIST_DELIM;
                            }

                            if (SerialNo.Voided == true)
                            {
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                            }
                        }
                        else
                        {
                            ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                        }
                    }
                    // The part is moving from one PCID to another
                    else
                    {
                        if (SerialNo != null)
                        {
                            if (plantSerialTracking == 2)
                            {
                                if (!SerialNo.PCID.KeyEquals(ttIssueReturn.FromPCID))
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberNotOnPCID + Ice.Constants.LIST_DELIM;
                                }

                                if (SerialNo.Voided == true)
                                {
                                    ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                                }
                            }
                            else if (plantSerialTracking == 3)
                            {
                                if (GetPkgControlHeaderOutboundContainer(Session.CompanyID, ttIssueReturn.FromPCID) == false &&
                                    GetPkgControlHeaderOutboundContainer(Session.CompanyID, ttIssueReturn.ToPCID) == true)
                                {
                                    // Only voided serial numbers can be selected
                                    if (SerialNo.Voided == false)
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberIsNotVoided + Ice.Constants.LIST_DELIM;
                                    }

                                    if (!String.IsNullOrEmpty(SerialNo.PCID))
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberForPCID(SerialNo.PCID) + Ice.Constants.LIST_DELIM;
                                    }
                                }
                                else
                                {
                                    if (!SerialNo.PCID.KeyEquals(ttIssueReturn.FromPCID))
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberNotOnPCID + Ice.Constants.LIST_DELIM;
                                    }

                                    if (SerialNo.Voided == true)
                                    {
                                        ErrMessage = ErrMessage + Strings.ThisSerialNumberIsVoided + Ice.Constants.LIST_DELIM;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // When plantSerialTracking = 3 (outbound serial tracking, and the FromPCID is not an outbound container, and the ToPCID is an outbound container, 
                            // the user can create serial numbers.  Therefore throw an error that the serial number does not exist when the plant does full serial tracking or
                            // if the plant does outbound serial tracking, throw the error if it is not the case where the FromPCID is not outbound and the ToPCID is outbound
                            if (plantSerialTracking == 2 ||
                                (plantSerialTracking == 3 &&
                                !(GetPkgControlHeaderOutboundContainer(Session.CompanyID, ttIssueReturn.FromPCID) == false &&
                                    GetPkgControlHeaderOutboundContainer(Session.CompanyID, ttIssueReturn.ToPCID) == true)))
                                ErrMessage = ErrMessage + Strings.ThisSerialNumberDoesNotExist + Ice.Constants.LIST_DELIM;
                        }
                    }

                    break;

            }
            using (var libInventoryTrackingSerialNumbers = new Internal.Lib.InventoryTrackingSerialNumbers(Db))
            {
                libInventoryTrackingSerialNumbers.ValidateSerialNumberAttributeSetAgainstEntity(SerialNo.PartNum, SerialNo.AttributeSetID, ttIssueReturn.AttributeSetID, out string serialNumberAttributeError);
                if (!string.IsNullOrEmpty(serialNumberAttributeError))
                {
                    ErrMessage = ErrMessage + serialNumberAttributeError + Ice.Constants.LIST_DELIM;
                }
            }

            if (checkSNalloc)
            {
                this.LibAllocations.checkSNAllocations(SerialNo.SerialNumber, SerialNo.PartNum, ttIssueReturn.OrderNum, ttIssueReturn.OrderLine, ttIssueReturn.OrderRel, ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq, ttIssueReturn.TFOrdNum, ttIssueReturn.TFOrdLine, out opMessage, ref SerialNo);
                if (!String.IsNullOrEmpty(opMessage))
                {
                    ErrMessage = ErrMessage + opMessage + Ice.Constants.LIST_DELIM;
                }
            }
            this.LibSerialCommon.formatSNErrors(SerialNum, ref ErrMessage);
        }

        /// <summary>
        /// Validates that a single serial number is valid for this transaction
        /// </summary>
        /// <param name="ds">Issue Return data set</param>
        /// <param name="serialNumber">Serial number to validate.</param>
        /// <param name="isVoided">Serial Number Voided flag</param>
        public void ValidateSN(ref IssueReturnTableset ds, string serialNumber, out bool isVoided)
        {
            isVoided = false;
            CurrentFullTableset = ds;
            string ErrMessage = string.Empty;
            string snValidationErrors = string.Empty;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn == null)
            {
                throw new BLException(Strings.TtIssueReturnRecordNotFound, "ttIssueReturn", "RowMod");
            }

            SerialNo = this.FindFirstSerialNo3(Session.CompanyID, ttIssueReturn.PartNum, serialNumber);
            if (SerialNo == null)
            {
                ErrMessage = GlobalStrings.SNSelectButtonTxt(serialNumber);
                throw new BLException(ErrMessage);
            }
            // ValidateSN is called only when the user enters a sn manually in the select form, in this case it has to be an existing SN
            this.validateSerialNumber(serialNumber, SerialNo, out snValidationErrors);
            if (!String.IsNullOrEmpty(snValidationErrors))
            {
                throw new BLException(snValidationErrors);
            }
            isVoided = SerialNo.Voided;
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void validateTranQty(IssueReturnRow ttIssueReturn)
        {
            if (ttIssueReturn.TranQty == 0)
                ExceptionManager.AddBLException(Strings.AValidQuantityIsRequired, "IssueReturn", "TranQty", ttIssueReturn.SysRowID);

            if (StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-WIP") != 0 && StringExtensions.Compare(ttIssueReturn.TranType, "ADJ-MTL") != 0)
            {
                if (ttIssueReturn.TranQty < 0)
                    ExceptionManager.AddBLException(Strings.QuantityCannotBeNegative, "IssueReturn", "PartNum", ttIssueReturn.SysRowID);
            }
            ConvQty = ttIssueReturn.TranQty;

            Part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (Part != null)
                LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, Part.IUM, out ConvQty, false);

            // validate if decimals are allowed
            if (StopOnUOMNoRound(Session.CompanyID) && NoRounding(Session.CompanyID, ttIssueReturn.UM))
            {
                if (ConvQty - Math.Truncate(ConvQty) != 0)
                    ExceptionManager.AddBLException(Strings.NoDecAllowed(ConvQty, ttIssueReturn.UM));
            }

            if (StringExtensions.Compare(ttIssueReturn.TranType, "MTL-STK") == 0)
            {
                decimal PartIssuedQty = decimal.Zero;
                getPartIssuedQty(ttIssueReturn, out PartIssuedQty);
                LibAppService.RoundToUOMDec(ttIssueReturn.UM, ref PartIssuedQty);
                if ((ConvQty) > (PartIssuedQty))
                    ExceptionManager.AddBLException(Strings.CannotReturnMoreThanWasIssued, "IssueReturn", "TranQty", ttIssueReturn.SysRowID);
            }
            if (StringExtensions.Compare(ttIssueReturn.TranType, "ASM-STK") == 0)
            {
                JobAsmbl = FindFirstJobAsmbl(Session.CompanyID, ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq);
                if (JobAsmbl != null)
                    LibAppService.UOMConv(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, JobAsmbl.IUM, out ConvQty, false);

                if (JobAsmbl != null && (ConvQty) > (JobAsmbl.IssuedQty))
                    ExceptionManager.AddBLException(Strings.CannotReturnMoreThanWasIssued, "IssueReturn", "TranQty", ttIssueReturn.SysRowID);
            }

            if (ttIssueReturn.TranType.Compare("STK-INS") == 0)
            {
                MtlQueue = FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
                if (MtlQueue != null)
                {
                    if (MtlQueue.Quantity < ttIssueReturn.TranQty)
                        ExceptionManager.AddBLException(Strings.QuantityCannotBeGreaterThanNonConformanceQuantity, "IssueReturn", "TranQty", ttIssueReturn.SysRowID);
                }
            }
            ExceptionManager.AssertNoBLExceptions();
        }


        /// <summary>
        /// Validates To Location, Bin Number exists
        /// </summary>
        /// <param name="ds">IssueReturnTableset DataSet</param>
        /// <param name="binNum">binNum</param>
        public void validateToBinNum(ref IssueReturnTableset ds, string binNum)
        {
            CurrentFullTableset = ds;
            bool lConsiderPartNum = false;
            string InvtyUOM = string.Empty;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                /* determineSensitivity builds the list of fields which are applicable for the TranType */
                this.determineSensitivity(ttIssueReturn);
                lConsiderPartNum = (this.getFromType(ttIssueReturn.TranType).KeyEquals("STK"));
                /* scr 47962 set lconsiderPartNum to no as this could be a shared warehouse */
                if (ttIssueReturn.TranType.Equals("STK-MTL", StringComparison.OrdinalIgnoreCase))
                {
                    lConsiderPartNum = false;
                }

                this.validateWareHouseCodeBinNum(ttIssueReturn.ToWarehouseCode, binNum, ttIssueReturn.PartNum, lConsiderPartNum, ttIssueReturn.SysRowID, "To");
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void validateWareHouseCodeBinNum(string pcWareHouseCode, string pcBinNum, string pcPartNum, bool plConsiderPartNum, Guid pcSysRowID, string pcFromToTag)
        {
            bool lWarehouseCodeSensitive = false;
            string cWarehouseReqMsg = string.Empty;
            string cBinNumReqMsg = string.Empty;
            string cBinCannotBeManagedMsg = string.Empty;
            int iCustNumFromBinNum = 0;
            int iCustNumToBinNum = 0;
            int iVendorNumFromBinNum = 0;
            int iVendorNumToBinNum = 0;
            string cWarehouseBinType = string.Empty;
            string cWarehouseToBinType = string.Empty;

            if (StringExtensions.Compare(pcFromToTag, "To") == 0 && !Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && (!this.getToType(ttIssueReturn.TranType).KeyEquals("STK")))
            {
                if (!String.IsNullOrEmpty(pcWareHouseCode))
                    ExceptionManager.AddBLException(Strings.ToWarehCannotBeSetWithoutAdvanMaterManagLicense, "IssueReturn", "ToWarehouseCode");

                if (!String.IsNullOrEmpty(pcBinNum))
                    ExceptionManager.AddBLException(Strings.ToBinCannotBeSetWithoutAdvanMaterManagLicense, "IssueReturn", "ToBinNum", pcSysRowID);

                ExceptionManager.AssertNoBLExceptions();
                return;
            }

            if (StringExtensions.Compare(pcFromToTag, "From") == 0 && !Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && (!this.getFromType(ttIssueReturn.TranType).KeyEquals("STK")))
            {
                if (!String.IsNullOrEmpty(pcWareHouseCode))
                    ExceptionManager.AddBLException(Strings.FromWarehCannotBeSetWithoutAdvanMaterManagLicense, "IssueReturn", "FromWarehouseCode");

                if (!String.IsNullOrEmpty(pcBinNum))
                    ExceptionManager.AddBLException(Strings.FromBinCannotBeSetWithoutAdvanMaterManagLicense, "IssueReturn", "FromBinNum", pcSysRowID);

                ExceptionManager.AssertNoBLExceptions();
                return;
            }

            /* Check to see if WarehouseCode is applicable for this record.  If not then return.*/
            if (String.IsNullOrEmpty(pcWareHouseCode))
            {
                if (StringExtensions.Compare(pcFromToTag, "From") == 0)
                    lWarehouseCodeSensitive = this.isSensitive("ttIssueReturn.FromWarehouseCode", pcSysRowID);
                else
                    lWarehouseCodeSensitive = this.isSensitive("ttIssueReturn.ToWarehouseCode", pcSysRowID);

                if (!lWarehouseCodeSensitive)
                    return;
            }

            if (StringExtensions.Compare(pcFromToTag, "From") == 0)
            {
                cWarehouseReqMsg = Strings.AValidFromWarehouseIsRequired;
                cBinNumReqMsg = Strings.AValidFromBinNumberIsRequired;
                cBinCannotBeManagedMsg = Strings.TheFromBinCannotBeAManagedBin;
            }
            else
            {
                cWarehouseReqMsg = Strings.AValidToWarehouseIsRequired;
                cBinNumReqMsg = Strings.AValidToBinNumberIsRequired;
                cBinCannotBeManagedMsg = Strings.TheToBinCannotBeAManagedBin;
            }

            if (String.IsNullOrEmpty(pcWareHouseCode))
                throw new BLException(cWarehouseReqMsg, "IssueReturn", "ToWarehouseCode", pcSysRowID);

            string whseType = "";
            Warehse warehse = FindFirstWarehse(Session.CompanyID, pcWareHouseCode);
            if (warehse == null)
                throw new BLException(cWarehouseReqMsg, "IssueReturn", "FromWarehouseCode", pcSysRowID);

            using (var libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
            {
                if (!libPackageControlValidations.IsPCIDMovement(ttIssueReturn.FromPCID, ttIssueReturn.ToPCID, ttIssueReturn.PartNum))
                {
                    if (plConsiderPartNum == false)
                    {
                        if (warehse.Plant.KeyEquals(Session.PlantID))
                        {
                            if ((StringExtensions.Compare(ttIssueReturn.ProcessID, "IssueMaterial") == 0) && ((StringExtensions.Compare(pcFromToTag, "To") == 0) || ExistsUniquePartWhse(Session.CompanyID, pcPartNum, pcWareHouseCode)) ||
                                (StringExtensions.Compare(ttIssueReturn.ProcessID, "IssueMaterial") != 0))
                            {
                                whseType = "STK";
                            }
                        }
                        else if (ExistsPlantShared(Session.CompanyID, Session.PlantID, warehse.WarehouseCode))
                        {
                            whseType = "SHR";
                        }
                    }
                    else
                    {
                        if (ExistPlantWhse(Session.CompanyID, Session.PlantID, pcPartNum, warehse.WarehouseCode))
                            whseType = "STK";
                        else if (ExistsPlantShared2(Session.CompanyID, Session.PlantID, warehse.WarehouseCode))
                            whseType = "SHR";
                    }

                    if (String.IsNullOrEmpty(whseType))
                        throw new BLException(cWarehouseReqMsg, "IssueReturn", "FromWarehouseCode", pcSysRowID);
                }
            }

            if (String.IsNullOrEmpty(pcBinNum))
                throw new BLException(cBinNumReqMsg, "IssueReturn", "FromBinNum", pcSysRowID);

            WhseBin = FindFirstWhseBin(Session.CompanyID, pcWareHouseCode, pcBinNum);
            if (WhseBin == null)
                throw new BLException(cBinNumReqMsg, "IssueReturn", "FromWarehouseCode", pcSysRowID);

            cWarehouseBinType = WhseBin.BinType;

            WhseBin = FindFirstWhseBin(Session.CompanyID, ttIssueReturn.FromBinNum);
            if (WhseBin != null)
            {
                iCustNumFromBinNum = WhseBin.CustNum;
                iVendorNumFromBinNum = WhseBin.VendorNum;
            }

            WhseBin = FindFirstWhseBin(Session.CompanyID, ttIssueReturn.ToBinNum);
            if (WhseBin != null)
            {
                iCustNumToBinNum = WhseBin.CustNum;
                iVendorNumToBinNum = WhseBin.VendorNum;
                cWarehouseToBinType = WhseBin.BinType;
            }

            if (StringExtensions.Compare(cWarehouseBinType, "Supp") == 0 && (iVendorNumFromBinNum != iVendorNumToBinNum) && (StringExtensions.Compare(cWarehouseToBinType, "Std") != 0))
            {
                if (StringExtensions.Compare(pcFromToTag, "To") == 0)
                {
                    if (StringExtensions.Lookup("STK-STK,STK-SHP,PUR-STK,PUR-SMI", ttIssueReturn.TranType) == -1)
                        throw new BLException(cBinCannotBeManagedMsg, "IssueReturn", "pcRowIdent");
                }

                if (StringExtensions.Compare(pcFromToTag, "From") == 0)
                {
                    if (StringExtensions.Lookup("STK-STK,STK-MTL,STK-SHP,STK-UKN,PUR-SMI,CMP-SHP", ttIssueReturn.TranType) == -1)
                        throw new BLException(cBinCannotBeManagedMsg, "IssueReturn", "pcRowIdent");
                }
            }

            if (StringExtensions.Compare(cWarehouseBinType, "Cust") == 0 && (iCustNumFromBinNum != iCustNumToBinNum))
            {
                if (StringExtensions.Compare(pcFromToTag, "To") == 0)
                {
                    if (StringExtensions.Lookup("STK-STK,STK-SHP,PUR-STK,PUR-CMI", ttIssueReturn.TranType) == -1)
                        throw new BLException(cBinCannotBeManagedMsg, "IssueReturn", "pcRowIdent");
                }
                if (StringExtensions.Compare(pcFromToTag, "From") == 0)
                {
                    if (StringExtensions.Lookup("STK-ASM,STK-STK,STK-MTL,STK-SHP,STK-UKN,PUR-CMI,CMP-SHP", ttIssueReturn.TranType) == -1)
                        throw new BLException(cBinCannotBeManagedMsg, "IssueReturn", "pcRowIdent");
                }
            }

            if (!cWarehouseBinType.Equals("Std", StringComparison.OrdinalIgnoreCase) && !cWarehouseBinType.Equals("Cont", StringComparison.OrdinalIgnoreCase))
            {
                if (StringExtensions.Lookup("ADJ-MTL,ADJ-WIP,ASM-STK,MTL-STK,UKN-STK,WIP-WIP", ttIssueReturn.TranType) != -1)
                    throw new BLException(cBinCannotBeManagedMsg, "IssueReturn", "pcRowIdent");
            }
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void CheckIssueReturnRow(IssueReturnRow ttIssueReturn)
        {
            if (isMalaysiaLocalization)
            {
                var tranDocType = TranDocType.FindFirstByPrimaryKey(Db, Session.CompanyID, ttIssueReturn.TranDocTypeID);
                if (tranDocType != null && tranDocType.MYOwnUse)
                {
                    var part = Part.FindFirstByPrimaryKey(Db, Session.CompanyID, ttIssueReturn.PartNum);
                    if (part != null && string.IsNullOrEmpty(part.CommodityCode))
                    {
                        throw new BLException(Strings.TariffCodeShouldBeSetOnPart, "IssueReturn");
                    }
                }
            }

            if (!isValidTranType(ttIssueReturn.TranType))
                throw new BLException(Strings.AValidTrantypeIsRequired, "IssueReturn", "TranType");

            if (!string.IsNullOrEmpty(ttIssueReturn.FromJobNum))
            {
                validateJobNum(ttIssueReturn.FromJobNum, ttIssueReturn.SysRowID, "FromJobNum", "From", ttIssueReturn.ProcessID);
                ExceptionManager.AssertNoBLExceptions();

                validateAssemblySeq(ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.TranType, ttIssueReturn.SysRowID, "FromAssemblySeq", "From");
                ExceptionManager.AssertNoBLExceptions();

                string cType = getFromType(ttIssueReturn.TranType);

                validateJobSeq(ttIssueReturn.FromJobNum, ttIssueReturn.FromAssemblySeq, ttIssueReturn.FromJobSeq, cType, ttIssueReturn.SysRowID, "FromJobSeq", "From");
                ExceptionManager.AssertNoBLExceptions();
            }

            if (!string.IsNullOrEmpty(ttIssueReturn.ToJobNum))
            {
                validateJobNum(ttIssueReturn.ToJobNum, ttIssueReturn.SysRowID, "ToJobNum", "To", ttIssueReturn.ProcessID);
                ExceptionManager.AssertNoBLExceptions();

                validateAssemblySeq(ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.TranType, ttIssueReturn.SysRowID, "ToAssemblySeq", "To");
                ExceptionManager.AssertNoBLExceptions();

                string cType = getToType(ttIssueReturn.TranType);

                validateJobSeq(ttIssueReturn.ToJobNum, ttIssueReturn.ToAssemblySeq, ttIssueReturn.ToJobSeq, cType, ttIssueReturn.SysRowID, "ToJobSeq", "To");
                ExceptionManager.AssertNoBLExceptions();
            }

            using (var libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
            {
                if (!libPackageControlValidations.IsPCIDMovement(ttIssueReturn.FromPCID, ttIssueReturn.ToPCID, ttIssueReturn.PartNum))
                {
                    validateTranQty(ttIssueReturn);
                    ExceptionManager.AssertNoBLExceptions();

                    validatePartNum(ttIssueReturn);
                    ExceptionManager.AssertNoBLExceptions();
                }
            }

            if (ttIssueReturn.TranDate == null || ttIssueReturn.TranDate > LibOffSet.OffsetToday())
                ExceptionManager.AddBLException(Strings.AValidDateIsRequired, "IssueReturn", "Trandate", ttIssueReturn.SysRowID);

            bool lConsiderPartNum = getFromType(ttIssueReturn.TranType).KeyEquals("STK");
            if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-MTL") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "STK-SHP") == 0)
                lConsiderPartNum = false;/* scr 47962 allow shared warehouse in Issue Material */

            validateWareHouseCodeBinNum(ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.PartNum, lConsiderPartNum, ttIssueReturn.SysRowID, "From");
            ExceptionManager.AssertNoBLExceptions();

            lConsiderPartNum = getToType(ttIssueReturn.TranType).KeyEquals("STK");

            validateWareHouseCodeBinNum(ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum, ttIssueReturn.PartNum, lConsiderPartNum, ttIssueReturn.SysRowID, "To");
            ExceptionManager.AssertNoBLExceptions();

            // Validations: Blank Reason Type 
            if (JCSyst == null)
                JCSyst = FindFirstJCSyst(Session.CompanyID);

            if (JCSyst.InvAdjReasons && (ttIssueReturn.TranType.Compare("STK-UKN") == 0 || ttIssueReturn.TranType.Compare("UKN-STK") == 0))
            {
                if (string.IsNullOrEmpty(ttIssueReturn.ReasonCode))
                {
                    ExceptionManager.AddBLException(Strings.ReasonCodeIsRequired);
                }
                else
                {
                    if (!ExistsReason(Session.CompanyID, "M", ttIssueReturn.ReasonCode))
                    {
                        ExceptionManager.AddBLException(Strings.ReasonCodeReferInvalidValue);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ttIssueReturn.ReasonCode) && !ExistsReason(Session.CompanyID, ttIssueReturn.ReasonCode))
                {
                    ExceptionManager.AddBLException(Strings.ReasonCodeReferInvalidValue);
                }
            }

            /* Now have to check combination of From and To Bins to make sure they are a valid combination
               depending on the transaction type */

            WhseBin fWhseBin = FindFirstWhseBin(Session.CompanyID, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum);
            WhseBin tWhseBin = FindFirstWhseBin(Session.CompanyID, ttIssueReturn.ToWarehouseCode, ttIssueReturn.ToBinNum);

            if (fWhseBin != null && tWhseBin != null)
            {
                if ((StringExtensions.Compare(fWhseBin.BinType, "Supp") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Cust") == 0) ||
                    (StringExtensions.Compare(fWhseBin.BinType, "Cust") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Supp") == 0))
                {
                    throw new BLException(Strings.CannotMoveBetweenSupplAndCustoManagedBins);
                }
            }

            if (StringExtensions.Compare(ttIssueReturn.TranType, "PUR-SMI") == 0)
            {
                if ((StringExtensions.Compare(fWhseBin.BinType, "Supp") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Supp") != 0) ||
                    (StringExtensions.Compare(tWhseBin.BinType, "Supp") == 0 && StringExtensions.Compare(fWhseBin.BinType, "Supp") != 0))
                {
                    throw new BLException(Strings.BothFromAndToBinsMustBeSupplManagedBinsWhenPerfo);
                }

                if (StringExtensions.Compare(fWhseBin.BinType, "Supp") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Supp") == 0)
                {
                    if (fWhseBin.VendorNum != tWhseBin.VendorNum)
                    {
                        throw new BLException(Strings.TransFromOneSupplManagedBinToAnotherMustBeForThe);
                    }
                }
            }

            if (StringExtensions.Compare(ttIssueReturn.TranType, "PUR-CMI") == 0)
            {
                if ((StringExtensions.Compare(fWhseBin.BinType, "Cust") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Cust") != 0) ||
                    (StringExtensions.Compare(tWhseBin.BinType, "Cust") == 0 && StringExtensions.Compare(fWhseBin.BinType, "Cust") != 0))
                {
                    throw new BLException(Strings.BothFromAndToBinsMustBeCustoManagedBinsWhenPerfo);
                }

                if (StringExtensions.Compare(fWhseBin.BinType, "Cust") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Cust") == 0)
                {
                    if (fWhseBin.CustNum != tWhseBin.CustNum)
                    {
                        throw new BLException(Strings.TransFromOneCustoManagedBinToAnotherMustBeForThe);
                    }
                }
            }

            if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-STK") == 0)
            {
                if ((StringExtensions.Compare(fWhseBin.BinType, "Cust") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Cust") != 0) ||
                    (StringExtensions.Compare(tWhseBin.BinType, "Cust") == 0 && StringExtensions.Compare(fWhseBin.BinType, "Cust") != 0))
                {
                    throw new BLException(Strings.BothFromAndToBinsMustBeCustoManagedBinsWhenTrans);
                }

                if (StringExtensions.Compare(fWhseBin.BinType, "Cust") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Cust") == 0)
                {
                    if (fWhseBin.CustNum != tWhseBin.CustNum)
                    {
                        throw new BLException(Strings.TransFromOneCustoManagedBinToAnotherMustBeForThe);
                    }
                }

                if (StringExtensions.Compare(fWhseBin.BinType, "Std") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Supp") == 0)
                {
                    throw new BLException(Strings.CannotTransFromAStandBinToASupplManagedBin);
                }

                if (StringExtensions.Compare(fWhseBin.BinType, "Supp") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Supp") == 0)
                {
                    if (fWhseBin.VendorNum != tWhseBin.VendorNum)
                    {
                        throw new BLException(Strings.TransFromOneSupplManagedBinToAnotherMustBeForThe);
                    }
                }
            }

            if (StringExtensions.Compare(ttIssueReturn.TranType, "STK-SHP") == 0)
            {
                if (StringExtensions.Compare(fWhseBin.BinType, "Cust") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Cust") == 0)
                {
                    if (fWhseBin.CustNum != tWhseBin.CustNum)
                    {
                        throw new BLException(Strings.TransFromOneCustoManagedBinToAnotherMustBeForThe);
                    }
                }

                if (StringExtensions.Compare(fWhseBin.BinType, "Supp") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Supp") == 0)
                {
                    if (fWhseBin.VendorNum != tWhseBin.VendorNum)
                    {
                        throw new BLException(Strings.TransFromOneSupplManagedBinToAnotherMustBeForThe);
                    }
                }

                if ((StringExtensions.Compare(fWhseBin.BinType, "Std") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Supp") == 0) ||
                    (StringExtensions.Compare(fWhseBin.BinType, "Std") == 0 && StringExtensions.Compare(tWhseBin.BinType, "Cust") == 0))
                {
                    throw new BLException(Strings.CannotTransFromAStandBinToAManagedBin);
                }
            }

            if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") != 0)
            {
                validateLotNum(ttIssueReturn.PartTrackLots, ttIssueReturn.PartNum, ttIssueReturn.LotNum, ttIssueReturn.SysRowID);
                ExceptionManager.AssertNoBLExceptions();
            }

            validatePartWip();

            Part = this.FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            using (var inventoryTracking = new InventoryTracking(Db))
            using (var libAdvancedUOMValidations = new Erp.Internal.Lib.AdvancedUOMValidations(Db))
            {
                inventoryTracking.ValidateInventoryByRevision(Part?.TrackInventoryByRevision ?? false, ttIssueReturn);
                libAdvancedUOMValidations.CheckAttributeSetIsValidForPart(ttIssueReturn.AttributeSetID, true, ttIssueReturn.PartNum);
            }

            ExceptionManager.AssertNoBLExceptions();
        }

        ///<summary>
        ///  Purpose:
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void zerottIssueReturn(ref IssueReturnRow ttIssueReturn)
        {
            Erp.Tablesets.IssueReturnRow hold_ttIssueReturn = null;
            string cDirection = string.Empty;
            string cMessage = string.Empty;
            Guid rMtlQueueRowId = Guid.Empty;
            hold_ttIssueReturn = new Erp.Tablesets.IssueReturnRow();
            CurrentFullTableset.IssueReturn.Add(hold_ttIssueReturn);
            BufferCopy.Copy(ttIssueReturn, ref hold_ttIssueReturn);

            CurrentFullTableset.IssueReturn.Remove(ttIssueReturn);

            /* Create a new ttIssueReturn */
            ttIssueReturn = new Erp.Tablesets.IssueReturnRow();
            CurrentFullTableset.IssueReturn.Add(ttIssueReturn);
            /* Re-assign hold values */
            ttIssueReturn.DummyKeyField = hold_ttIssueReturn.DummyKeyField;
            ttIssueReturn.Company = hold_ttIssueReturn.Company;
            if (hold_ttIssueReturn.TranDate == null)
            {
                ttIssueReturn.TranDate = null;
            }
            else
            {
                ttIssueReturn.TranDate = hold_ttIssueReturn.TranDate;
            }

            ttIssueReturn.TranType = hold_ttIssueReturn.TranType;
            ttIssueReturn.IssuedComplete = false;
            ttIssueReturn.MtlQueueRowId = hold_ttIssueReturn.MtlQueueRowId;
            ttIssueReturn.FromJobPlant = hold_ttIssueReturn.FromJobPlant;
            ttIssueReturn.ToJobPlant = hold_ttIssueReturn.ToJobPlant;
            ttIssueReturn.TreeDisplay = hold_ttIssueReturn.TreeDisplay;
            ttIssueReturn.PartTrackSerialNum = hold_ttIssueReturn.PartTrackSerialNum; /* SCR 48058 */
            ttIssueReturn.PartTrackLots = hold_ttIssueReturn.PartTrackLots; /* SCR 46783 */
            ttIssueReturn.SysRowID = hold_ttIssueReturn.SysRowID;
            ttIssueReturn.ProcessID = hold_ttIssueReturn.ProcessID;
            ttIssueReturn.ToJobNum = hold_ttIssueReturn.ToJobNum;
            ttIssueReturn.TranDocTypeID = hold_ttIssueReturn.TranDocTypeID;
            ttIssueReturn.PlantConfCtrlEnablePackageControl = hold_ttIssueReturn.PlantConfCtrlEnablePackageControl;
            ttIssueReturn.PkgControlHeaderDeleted = hold_ttIssueReturn.PkgControlHeaderDeleted;

            cDirection = this.getJobDirection(ttIssueReturn.TranType);
            /* determineSensitivity builds the list of fields which are applicable for the TranType */
            this.determineSensitivity(ttIssueReturn);
            if (StringExtensions.Compare(cDirection, "From") == 0)
            {
                ttIssueReturn.FromJobNum = hold_ttIssueReturn.FromJobNum;
                if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                {
                    ttIssueReturn.ToJobNum = ttIssueReturn.FromJobNum;
                }

                this.onChangeFromJobNumCore(ttIssueReturn);
                ttIssueReturn.FromAssemblySeq = hold_ttIssueReturn.FromAssemblySeq;
                if (StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
                {
                    ttIssueReturn.ToAssemblySeq = ttIssueReturn.FromAssemblySeq;
                }

                this.onChangeFromAssemblySeqCore(ttIssueReturn);
            }/* if cDirection = "From" */
            if (StringExtensions.Compare(cDirection, "To") == 0 || StringExtensions.Compare(ttIssueReturn.TranType, "WIP-WIP") == 0)
            {
                ttIssueReturn.ToJobNum = hold_ttIssueReturn.ToJobNum;
                this.onChangeToJobNumCore(ttIssueReturn, out cMessage);
                ttIssueReturn.ToAssemblySeq = hold_ttIssueReturn.ToAssemblySeq;
                this.onChangeToAssemblySeqCore(ttIssueReturn);
            }/* if cDirection = "To" */
            /* cDirection will be blank in case of Misc. Issue & Return */
            if (String.IsNullOrEmpty(cDirection))
            {
                if (StringExtensions.Lookup("STK-UKN,UKN-STK", ttIssueReturn.TranType) > -1)
                {
                    ttIssueReturn.PartNum = hold_ttIssueReturn.PartNum;
                    ttIssueReturn.IssuedComplete = false;
                    this.onChangePartNumCore(ttIssueReturn);
                }
            }/* if cDirection = "" */

            rMtlQueueRowId = hold_ttIssueReturn.MtlQueueRowId;
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && rMtlQueueRowId != Guid.Empty)
            {
                this.processMtlQueue(rMtlQueueRowId, ttIssueReturn);
            }
            CurrentFullTableset.IssueReturn.Remove(hold_ttIssueReturn);
            this.enableSNButton(ttIssueReturn, ttIssueReturn.ProcessID);
        }

        /*------------------------------------------------------------------------------
          Purpose:
            Notes:
        ------------------------------------------------------------------------------*/
        private string getFromType(string pcTranType)
        {
            string cFromType = string.Empty;
            /* SET THE FROM/TO CONTROL BY EXTRACTION FROM THE TRAN TYPE */
            cFromType = StringExtensions.SubString(pcTranType, 0, 3);
            /* SOME OF THE TRAN-TYPES DO NOT BREAKDOWN INTO A STANDARD "FROM/TO" VARIABLE
               THEREFORE WE NEED TO CONSIDER THEM AND SET THE FROM/TO ACCORDINGLY
            */
            if (StringExtensions.Compare(pcTranType, "INS-ASM") == 0 ||
                StringExtensions.Compare(pcTranType, "ASM-INS") == 0)
            {
                cFromType = "OPR";
            }
            if (StringExtensions.Compare(cFromType, "SUB") == 0 ||
                StringExtensions.Compare(cFromType, "MFG") == 0 ||
                StringExtensions.Compare(cFromType, "WIP") == 0 ||
                StringExtensions.Compare(pcTranType, "PUR-SUB") == 0 ||
                StringExtensions.Compare(pcTranType, "DMR-SUB") == 0)
            {
                cFromType = "OPR";
            }

            if (StringExtensions.Compare(cFromType, "RAU") == 0 || StringExtensions.Compare(cFromType, "RMG") == 0 || StringExtensions.Compare(cFromType, "RMN") == 0)
            {
                cFromType = "STK";
            }

            return cFromType;
        }

        /// <summary>
        /// Gets the default values for the browse section of the adjustment screen
        /// </summary>
        /// <param name="partNum">Part number for the adjustment.</param>
        /// <param name="attributeSetID">Attribute Set ID used to get bin information. Bins are not filtered by Attribute Set ID if a zero is sent</param>        
        /// <param name="wareHouseCode">Warehouse code used to get bin information.</param>  
        /// <param name="primaryBin">Part Warehouse's Primary Bin.</param>  
        /// <returns>Browse for inventory qty adj data set</returns>    
        public InventoryQtyAdjBrwTableset GetInventoryQtyAdjBrw(string partNum, int attributeSetID, string wareHouseCode, out string primaryBin)
        {
            return GetInventoryQtyAdjBrwInternal(partNum, "", attributeSetID, wareHouseCode, out primaryBin);
        }

        /// <summary>
        /// Gets the default values for the browse section of the adjustment screen
        /// </summary>
        /// <param name="partNum">Part number for the adjustment.</param>
        /// <param name="revisionNum">Revision Number used to get bin information. Bins are not filtered by Revision Number if no value is sent.</param>
        /// <param name="attributeSetID">Attribute Set ID used to get bin information. Bins are not filtered by Attribute Set ID if a zero is sent</param>
        /// <param name="wareHouseCode">Warehouse code used to get bin information.</param>  
        /// <param name="primaryBin">Part Warehouse's Primary Bin.</param>  
        /// <returns>Browse for inventory qty adj data set</returns>    
        public InventoryQtyAdjBrwTableset GetInventoryQtyAdjBrwInventoryTracking(string partNum, string revisionNum, int attributeSetID, string wareHouseCode, out string primaryBin)
        {
            return GetInventoryQtyAdjBrwInternal(partNum, revisionNum, attributeSetID, wareHouseCode, out primaryBin);
        }

        /// <summary>
        /// Gets the default values for the browse section of the adjustment screen
        /// Copy of the same method of BO InventoryQtyAdj
        /// Specific for web (client) use.
        /// </summary>
        /// <param name="partNum">Part number for the adjustment.</param>
        /// <param name="attributeSetID">Attribute Set ID for the adjustment</param>
        /// <param name="wareHouseCode">Warehouse code used to get bin information.</param>  
        /// <param name="primaryBin">Part Warehouse's Primary Bin.</param>  
        /// <returns>Browse for inventory qty adj data set</returns>    
        [Obsolete("This method is has been obsoleted, please use GetInventoryQtyAdjBrwInventoryTracking")]
        public InventoryQtyAdjBrwTableset GetInventoryQtyAdjBrwForWeb(string partNum, int attributeSetID, string wareHouseCode, out string primaryBin)
        {
            InventoryQtyAdjBrwTableset ret = this.GetInventoryQtyAdjBrw(partNum, attributeSetID, wareHouseCode, out primaryBin);
            if (ret == null) return ret; //if no response, just return it

            InventoryQtyAdjBrwTable tab = ret.InventoryQtyAdjBrw;
            if (tab == null) return ret; //if no table, just return the response

            //get just arbitrary UUIDs so the MetaFx erp-rest can process the response
            //correctly because otherwise, with null UUIDs, it cannot.
            foreach (InventoryQtyAdjBrwRow r in tab) r.SysRowID = Guid.NewGuid();

            return ret;
        }

        private InventoryQtyAdjBrwTableset GetInventoryQtyAdjBrwInternal(string partNum, string revisionNum, int attributeSetID, string wareHouseCode, out string primaryBin)
        {
            primaryBin = string.Empty;
            InventoryQtyAdjBrwTableset ttInventoryQtyAdjBrwTablesetDS = new InventoryQtyAdjBrwTableset();

            inventoryQtyAdjAlias.Erp.Tablesets.InventoryQtyAdjBrwTableset aliasDS = new();
            InventoryQtyAdjBrwTableset returnDS = new();

            using (var InventoryQtyAdjSvc = Ice.Assemblies.ServiceRenderer.GetService<inventoryQtyAdjAlias::Erp.Contracts.InventoryQtyAdjSvcContract>(Db))
            {
                aliasDS = InventoryQtyAdjSvc.GetInventoryQtyAdjBrwInventoryTracking(partNum, revisionNum, attributeSetID, wareHouseCode, out primaryBin);
            }

            foreach (var ttInventoryQtyAdjBrwRow in aliasDS.InventoryQtyAdjBrw)
            {
                InventoryQtyAdjBrwRow ttInventoryQtyAdjBrw = new InventoryQtyAdjBrwRow();
                BufferCopy.Copy(ttInventoryQtyAdjBrwRow, ttInventoryQtyAdjBrw);
                ttInventoryQtyAdjBrw.SysRowID = Guid.NewGuid();
                returnDS.InventoryQtyAdjBrw.Add(ttInventoryQtyAdjBrw);
            }

            return returnDS;
        }

        /*------------------------------------------------------------------------------
          Purpose:  Based on the transaction type it returns the type of Job
                    i.e. "From" or "To"
            Notes:
        ------------------------------------------------------------------------------*/
        private string getJobDirection(string pcTranType)
        {
            string cFromJobTranTypes = "MTL-MTL,WIP-WIP,MTL-STK,ASM-STK";
            string cToJobTranTypes = "ADJ-MTL,ADJ-WIP,STK-MTL,STK-ASM";
            string cDirection = string.Empty;
            cDirection = "";
            if (StringExtensions.Lookup(cFromJobTranTypes, pcTranType) > -1)
            {
                cDirection = "From";
            }

            if (StringExtensions.Lookup(cToJobTranTypes, pcTranType) > -1)
            {
                cDirection = "To";
            }

            return cDirection;
        }

        /*------------------------------------------------------------------------------
          Purpose:
            Notes:
        ------------------------------------------------------------------------------*/
        private string getJobHeadPlant(string iJobNum)
        {
            string ReturnValue = string.Empty;

            JobHead = this.FindFirstJobHead(Session.CompanyID, iJobNum);
            if (JobHead != null)
            {
                ReturnValue = JobHead.Plant;
            }

            return ReturnValue;   /*Function return value. */
        }

        /*------------------------------------------------------------------------------
          Purpose:
            Notes:
        ------------------------------------------------------------------------------*/
        private string getLegalNumberType(string pcTranType)
        {
            string TempLegalNumberType = string.Empty;
            switch (pcTranType.ToUpperInvariant())
            {
                case "ADJ-MTL":
                case "ADJ-WIP":
                    {
                        TempLegalNumberType = "StQtyAdj";
                    }
                    break;
                case "CMP-SHP":
                case "INS-STK":
                case "PLT-STK":
                case "STK-PLT":
                case "PUR-CMI":
                case "PUR-SHP":
                case "PUR-SMI":
                case "PUR-STK":
                case "RAU-STK":
                case "RMG-STK":
                case "RMN-STK":
                case "STK-SHP":
                case "STK-STK":
                case "STK-UKN":
                case "UKN-STK":
                    {
                        TempLegalNumberType = "StockStock";
                    }
                    break;
                case "ASM-STK":
                case "MTL-STK":
                    {
                        TempLegalNumberType = "WIPStock";
                    }
                    break;
                case "STK-ASM":
                case "STK-MTL":
                    {
                        TempLegalNumberType = "StockWIP";
                    }
                    break;
            }/* pcTranType */
            return TempLegalNumberType;  /* Function return value. */
        }

        /*------------------------------------------------------------------------------
          Purpose:
            Notes:
        ------------------------------------------------------------------------------*/
        private string getOwnerPlant(string iWarehouseCode)
        {
            return FindFirstWarehsePlant(Session.CompanyID, iWarehouseCode);
        }

        /*------------------------------------------------------------------------------
          Purpose:
            Notes:
        ------------------------------------------------------------------------------*/
        private string getPartCostMethod(string pcPlant, string pcPartNum)
        {
            Erp.Tables.Part altPart = null;
            string cCostMethod = string.Empty;

            PartPlant = this.FindFirstPartPlant(Session.CompanyID, pcPlant, pcPartNum);
            if (PartPlant != null)
            {
                cCostMethod = PartPlant.CostMethod;
            }
            else
            {
                altPart = this.FindFirstPart(Session.CompanyID, pcPartNum);
                if (altPart != null)
                {
                    cCostMethod = Part.CostMethod;
                }
            }/* else - if available PartPlant */
            return cCostMethod;
        }

        /// <summary>
        /// This method defaults PartAdvisor fields when the PartNum field changes
        /// </summary>
        /// <param name="partNum"> Proposed PartNumber change</param>
        /// <param name="sysRowID"> RowID of the selected record. Skips find part logic if this has a value. </param>
        /// <param name="rowType"> RowType of the selected record. Only used with sysRowID. </param>
        /// <param name="uomCode"> UOM Code (only used for Product Codes)</param>
        /// <param name="qty"> Qty (converted if UOM is different)</param>
        /// <param name="serialWarning">Warning message if the InvTransfer line contains serial numbers</param>
        /// <param name="questionString">If the part is being changed to something different than what was on the order, ask if the user wants to continue </param>
        /// <param name="multipleMatch"> Multiple matches were found by FindPart </param>
        public void GetPartXRefInfo(ref string partNum, string sysRowID, string rowType, ref string uomCode, ref decimal qty, out string serialWarning, out string questionString, out bool multipleMatch)
        {
            serialWarning = string.Empty;
            questionString = string.Empty;
            multipleMatch = false;
            string opPartNum = string.Empty;
            string opUOM = string.Empty;
            string opMatchType = string.Empty;
            string xrUomCode = string.Empty;
            /* no sysRowID */
            if (String.IsNullOrEmpty(sysRowID))
            {
                this.LibFindpart.FindPart(partNum, out opPartNum, out opUOM, out opMatchType);
                if (IsDimTracked(Session.CompanyID, opPartNum))
                {
                    partNum = opPartNum;
                    if (opMatchType.KeyEquals("MULTIPLE")) multipleMatch = true;
                    return;
                }
                switch (opMatchType.ToUpperInvariant())
                {
                    case "NONE":
                        {
                        }
                        break;
                    case "MULTIPLE":
                        multipleMatch = true;
                        return;
                    case "PARTPC":
                        {
                            partNum = opPartNum;
                            if (!uomCode.KeyEquals(opUOM) && qty != 0)
                            {
                                LibAppService.UOMConv(partNum, qty, uomCode, opUOM, out qty, false);
                                //It was decided by management to always round down.
                                LibAppService.RoundToUOMDec(opUOM, ref qty, "DWN");
                            }
                            uomCode = opUOM;
                        }
                        break;
                    default:
                        partNum = opPartNum;
                        break;
                }
            }
            else
            {
                this.LibFindpart.GetPartFromRowID(rowType, Guid.Parse(sysRowID), out partNum, out xrUomCode);
                uomCode = xrUomCode;
            }
        }

        /*------------------------------------------------------------------------------
          Purpose:
            Notes:
        ------------------------------------------------------------------------------*/
        private string getToType(string pcTranType)
        {
            string cToType = string.Empty;
            /* SET THE FROM/TO CONTROL BY EXTRACTION FROM THE TRAN TYPE */
            cToType = StringExtensions.SubString(pcTranType, 4, 3);
            /* SOME OF THE TRAN-TYPES DO NOT BREAKDOWN INTO A STANDARD "FROM/TO" VARIABLE
               THEREFORE WE NEED TO CONSIDER THEM AND SET THE FROM/TO ACCORDINGLY
            */
            if (StringExtensions.Compare(pcTranType, "INS-ASM") == 0 || StringExtensions.Compare(pcTranType, "ASM-INS") == 0)
            {
                cToType = "OPR";
            }

            if (StringExtensions.Compare(pcTranType, "MTL-INS") == 0)
            {
                cToType = "MTL";
            }

            if (StringExtensions.Compare(cToType, "SUB") == 0 || StringExtensions.Compare(cToType, "WIP") == 0)
            {
                cToType = "OPR";
            }

            return cToType;  /* Function return value. */
        }

        /*------------------------------------------------------------------------------
          Purpose:  Determines whether a field in a dataset is sensitive or not.
            Notes:
        ------------------------------------------------------------------------------*/
        private bool isSensitive(string pcFieldName, Guid pcSysRowID)
        {
            ttFieldAttribute = (from ttFieldAttribute_Row in ttFieldAttributeRows
                                where StringExtensions.Compare(ttFieldAttribute_Row.FieldName, pcFieldName) == 0
                                && ttFieldAttribute_Row.SysRowID == pcSysRowID
                                select ttFieldAttribute_Row).FirstOrDefault();
            if (ttFieldAttribute == null)
            {
                return true;
            }
            else
            {
                return ttFieldAttribute.isSensitive;
            }
        }

        /*------------------------------------------------------------------------------
          Purpose:  Validate if the TranType is supported or Valid.

          pcTranType  ----------------------------- DESCRIPTION ------------------------------
          ADJ-MTL      Adjust the WIP "raw material" tracking information Warehouse/quantity (PartWip)
          ADJ-WIP      Adjust the WIP part tracking information Warehouse/quantity (PartWip)
          ASM-INS      Movement of assembly non-conformance from the reporting area to the inspection warehouse (ASM-INS).
          ASM-STK      Return of issued stock on a job assemlby to stock
          INS-ASM      Movement of assembly from inspection to WIP location
          INS-DMR      Movement from inspecton to DMR.
          INS-MTL      Movement of raw material from inspection to WIP location
          INS-STK      Movement from inspection to stock warehouse
          INS-SUB      Movement of subcontract part from inspection to WIP location
          INS-SHP      Movement from inspection (related to cross docked purchase order) to shipping area.
          MFG-CUS      Movement of completed items on the final operation to the shipping area
          MFG-OPR      Request from labor processing to move quantity from operation to the next.
          MTL-INS      Movement of non-conforming material from production to inspection.
          MTL-MTL      Move WIP raw material to another WIP location
          MTL-STK      Return of issued stock on a job material to stock
          PLT-MTL      Movement of job material plant receipts from the receiving area to wip location
          PUR-INS      Movement of purchased receipts pending inspection from the receiving area to inspection
          PUR-MTL      Movement of direct job material purchases from the receiving area to WIP warehouse location
          PUR-STK      Movement of purchased stock from the receiving area to warehouse
          PUR-SMI      Movement of purchased stock from a SMI receiving area to SMI warehouse bin
          PUR-CMI      Movement of purchased stock from a CMI receiving area to CMI warehouse bin
          PUR-SUB      Movement of job subcontract receipts from receiving area to the next operations input warehouse/bin
          PUR-SHP      Movement of cross docked purchased stock for an order from the receiving area to warehouse
          RMA-INS      Movement of RMA receipts pending inspection from the receiving area to the inspection warehouse
          STK-ASM      Issue Stock to Job Assembly
          STK-INS      Movement of non-conforming stock from stocking area to the inspection warehouse.
          STK-MTL      Issue Stock to Job Material
          STK-PLT      Movement for an allocated Transfer Order
          STK-SHP      Pick Stock for Shipment (Move to shipping area).
          STK-STK      Stock to Stock transfer
          STK-UKN      Issue stock to unknown (miscllaneous)
          SUB-INS      Movement of subcontract nonconformance from production area to inspection
          UKN-STK      Return from unknown to stock
          WIP-WIP      Manual move of the Mfg Part to another location (Not moving to another op).
          PLT-STK      Movement of transfer orders from Receiving area to warehouse. Per scr22230.
          MFG-SHP      Movement of Pick from Manufacturing for Shipment.
          KIT-SHP      Movement of Picked Kit Parent to Shiping area.
          CMP-SHP      Movement of Picked Kit Component to Shiping
          DMR-ASM      Movement of DMR to WIP location/JobOper
          DMR-MTL      Movement of DMR to WIP location/JobMtl
          DMR-REJ      Movement of DMR - Reject
          DMR-STK      Movement of DMR to Stock
          DMR-SUB      Movement of DMR to Subcontract
          RAU-STK      Auto replenishment to stock
          RMN-STK      Manual replenishment to stock
          RMG-STK      Managed replenishment to stock
        ------------------------------------------------------------------------------*/
        private bool isValidTranType(string pcTranType)
        {
            if (StringExtensions.Compare(pcTranType, "ADJ-MTL") == 0
            || StringExtensions.Compare(pcTranType, "ADJ-WIP") == 0
            || StringExtensions.Compare(pcTranType, "ASM-INS") == 0
            || StringExtensions.Compare(pcTranType, "ASM-STK") == 0
            || StringExtensions.Compare(pcTranType, "INS-ASM") == 0
            || StringExtensions.Compare(pcTranType, "INS-DMR") == 0
            || StringExtensions.Compare(pcTranType, "INS-MTL") == 0
            || StringExtensions.Compare(pcTranType, "INS-STK") == 0
            || StringExtensions.Compare(pcTranType, "INS-SUB") == 0
            || StringExtensions.Compare(pcTranType, "INS-SHP") == 0
            || StringExtensions.Compare(pcTranType, "MFG-CUS") == 0
            || StringExtensions.Compare(pcTranType, "MFG-OPR") == 0
            || StringExtensions.Compare(pcTranType, "MTL-INS") == 0
            || StringExtensions.Compare(pcTranType, "MTL-MTL") == 0
            || StringExtensions.Compare(pcTranType, "MTL-STK") == 0
            || StringExtensions.Compare(pcTranType, "PLT-MTL") == 0
            || StringExtensions.Compare(pcTranType, "PUR-INS") == 0
            || StringExtensions.Compare(pcTranType, "PUR-MTL") == 0
            || StringExtensions.Compare(pcTranType, "PUR-STK") == 0
            || StringExtensions.Compare(pcTranType, "PUR-SMI") == 0
            || StringExtensions.Compare(pcTranType, "PUR-CMI") == 0
            || StringExtensions.Compare(pcTranType, "PUR-SUB") == 0
            || StringExtensions.Compare(pcTranType, "PUR-SHP") == 0
            || StringExtensions.Compare(pcTranType, "RMA-INS") == 0
            || StringExtensions.Compare(pcTranType, "STK-ASM") == 0
            || StringExtensions.Compare(pcTranType, "STK-INS") == 0
            || StringExtensions.Compare(pcTranType, "STK-MTL") == 0
            || StringExtensions.Compare(pcTranType, "STK-PLT") == 0
            || StringExtensions.Compare(pcTranType, "STK-SHP") == 0
            || StringExtensions.Compare(pcTranType, "STK-STK") == 0
            || StringExtensions.Compare(pcTranType, "STK-UKN") == 0
            || StringExtensions.Compare(pcTranType, "SUB-INS") == 0
            || StringExtensions.Compare(pcTranType, "UKN-STK") == 0
            || StringExtensions.Compare(pcTranType, "WIP-WIP") == 0
            || StringExtensions.Compare(pcTranType, "PLT-STK") == 0
            || StringExtensions.Compare(pcTranType, "MFG-SHP") == 0
            || StringExtensions.Compare(pcTranType, "KIT-SHP") == 0
            || StringExtensions.Compare(pcTranType, "CMP-SHP") == 0
            || StringExtensions.Compare(pcTranType, "DMR-ASM") == 0
            || StringExtensions.Compare(pcTranType, "DMR-MTL") == 0
            || StringExtensions.Compare(pcTranType, "DMR-REJ") == 0
            || StringExtensions.Compare(pcTranType, "DMR-STK") == 0
            || StringExtensions.Compare(pcTranType, "DMR-SUB") == 0
            || StringExtensions.Compare(pcTranType, "RAU-STK") == 0
            || StringExtensions.Compare(pcTranType, "RMN-STK") == 0
            || StringExtensions.Compare(pcTranType, "RMG-STK") == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*------------------------------------------------------------------------------
          Purpose:
            Notes:
        ------------------------------------------------------------------------------*/
        private bool isWarehouseMultiple(string pcFromType, string pcPartNum)
        {
            bool lMultipleWarehouse = false;
            Guid rFirstPartWhseRowID = Guid.Empty;
            int iRecordCount = 0;
            if (StringExtensions.Compare(pcFromType, "STK") == 0)
            {
                if (pcPartNum.Length == 0)
                {
                    return lMultipleWarehouse;
                }

                /* TEST IF THIS PART IS DEFINED IN MULTIPLE WAREHOUSES */
                PartWhse = this.FindFirstPartWhse3(Session.CompanyID, pcPartNum);
                if (PartWhse != null)
                {
                    rFirstPartWhseRowID = PartWhse.SysRowID;


                    lMultipleWarehouse = (this.ExistsPartWhse(Session.CompanyID, pcPartNum, rFirstPartWhseRowID));
                }
            }
            else
            {
                //COUNT_WHSE:
                foreach (Warehse warehse in SelectWarehse(Session.CompanyID, Session.PlantID))
                {
                    iRecordCount = iRecordCount + 1;
                    if (iRecordCount > 1)
                    {
                        break;
                    }
                }/* COUNT-WHSE */
                if (iRecordCount > 1)
                {
                    lMultipleWarehouse = true;
                }
            }
            return lMultipleWarehouse;
        }

        /*------------------------------------------------------------------------------
          Purpose:
            Notes:
        ------------------------------------------------------------------------------*/
        private bool partSerialTracking(string iPart)
        {
            return ExistsPartTrackSerialNum(Session.CompanyID, iPart);
        }

        /*------------------------------------------------------------------------------
          Purpose: To return a new Serial No Status base on Transaction type.
            Notes: Not all transactions result in a change of Serial No Status.
                   If Tran-Type is not found in the lists, then this function
                   will return the current serial no status.
        ------------------------------------------------------------------------------*/
        private string serialNoStatus(string pcTranType, string pcCurrentSNStatus)
        {
            string cWIP = "ADJ-MTL,ADJ-WIP,INS-ASM,INS-MTL,INS-SUB,MFG-SHP,MFG-CUS,MFG-OPR,PUR-MTL,PUR-SUB,STK-ASM,STK-MTL";
            string cInventory = "ASM-STK,INS-STK,INS-SHP,MTL-STK,MFG-STK,PUR-STK,PUR-SMI,PUR-CMI,UKN-STK";
            string cInspection = "ASM-INS,PUR-INS,RMA-INS,STK-INS";
            string cMisc = "STK-UKN";
            if (StringExtensions.Lookup(cWIP, pcTranType) > -1)
            {
                return "WIP";
            }

            if (StringExtensions.Lookup(cInventory, pcTranType) > -1)
            {
                return "INVENTORY";
            }

            if (StringExtensions.Lookup(cInspection, pcTranType) > -1)
            {
                return "INSPECTION";
            }

            if (StringExtensions.Lookup(cMisc, pcTranType) > -1)
            {
                return "MISC-ISSUE";
            }

            return pcCurrentSNStatus;
        }

        /*------------------------------------------------------------------------------
          Purpose:
            Notes:
        ------------------------------------------------------------------------------*/
        private bool snRequired(string iJobNum, int iAssemblySeq, int iOprSeq, bool iSubCon)
        {
            bool ReturnValue = false;

            JobOper = this.FindFirstJobOper(Session.CompanyID, iOprSeq, iAssemblySeq, iJobNum);
            if (JobOper != null)
            {
                ReturnValue = ((iSubCon) ? JobOper.SNRequiredSubConShip : JobOper.SNRequiredOpr);
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;   /*Function return value. */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPartNum"></param>
        /// <param name="iUOM"></param>
        /// <returns></returns>
        public bool ValidUOM(string iPartNum, string iUOM)
        {
            Part = this.FindFirstPart(Session.CompanyID, iPartNum);
            if (Part != null)
            {
                if (Part.TrackDimension)
                {
                    if (StringExtensions.Compare(Part.IUM, iUOM) == 0)
                    {
                        return true;
                    }

                    if ((this.ExistsPartUOM(Session.CompanyID, Part.PartNum, iUOM, true)))
                    {
                        return true;
                    }
                }
                else
                {
                    if ((this.ExistsUOMConv(Session.CompanyID, Part.UOMClassID, iUOM)))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if ((this.ExistsUOMConv(Session.CompanyID, true, iUOM)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method to call to get available tran doc types. 
        /// </summary>
        /// <param name="AvailTypes">The available tran doc types</param>
        public void GetAvailTranDocTypes(out string AvailTypes)
        {
            AvailTypes = string.Empty;
            LibGetAvailTranDocTypes.RunGetAvailTranDocTypes(out AvailTypes, "StockWIP,StockStock,WIPStock");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pcPartNum"></param>
        /// <param name="pcWhseCode"></param>
        /// <param name="pcBinNum"></param>
        /// <param name="pcLotNum"></param>
        /// <param name="pcAttributeSetID"></param>
        /// <param name="pcPCID"></param>
        /// <param name="pcDimCode"></param>
        /// <param name="pdDimConvFactor"></param>
        /// <param name="pgTranRowId"></param>
        /// <param name="ipSellingQuantity"></param>
        /// <param name="pcNeqQtyAction"></param>
        /// <param name="pcMessage"></param>
        public void NegativeInventoryTestTran(string pcPartNum, string pcWhseCode, string pcBinNum, string pcLotNum, int pcAttributeSetID, string pcPCID, string pcDimCode, decimal pdDimConvFactor, Guid pgTranRowId, decimal ipSellingQuantity, out string pcNeqQtyAction, out string pcMessage)
        {
            string status = string.Empty;
            var Result = this.FindFirstMtlQueueNonConfQuery(pgTranRowId);
            if (Result != null && Result.MtlQueue != null && Result.NonConf != null)
            {
                checkSNStatus(Result.MtlQueue.TranType, ipSellingQuantity, out status);
                //pcNeqQtyAction = string.Empty;
                //pcMessage = string.Empty;
                if (status.KeyEquals("INSPECTION"))
                {
                    pcNeqQtyAction = string.Empty;
                    pcMessage = string.Empty;
                    return;
                }
            }
            LibNegInvTest.NegativeInventoryTest(pcPartNum, pcWhseCode, pcBinNum, pcLotNum, pcAttributeSetID, pcPCID, pcDimCode, pdDimConvFactor, ipSellingQuantity, out pcNeqQtyAction, out pcMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pcPartNum"></param>
        /// <param name="pcWhseCode"></param>
        /// <param name="pcBinNum"></param>
        /// <param name="pcLotNum"></param>
        /// <param name="pcAttributeSetID"></param>
        /// <param name="pcPCID"></param>
        /// <param name="pcDimCode"></param>
        /// <param name="pdDimConvFactor"></param>
        /// <param name="ipSellingQuantity"></param>
        /// <param name="pcNeqQtyAction"></param>
        /// <param name="pcMessage"></param>
        public void NegativeInventoryTest(string pcPartNum, string pcWhseCode, string pcBinNum, string pcLotNum, int pcAttributeSetID, string pcPCID, string pcDimCode, decimal pdDimConvFactor, decimal ipSellingQuantity, out string pcNeqQtyAction, out string pcMessage)
        {
            LibNegInvTest.NegativeInventoryTest(pcPartNum, pcWhseCode, pcBinNum, pcLotNum, pcAttributeSetID, pcPCID, pcDimCode, pdDimConvFactor, ipSellingQuantity, out pcNeqQtyAction, out pcMessage);
        }

        /// <summary>
        /// Methods to check Negative Inventory and Contract bin.
        /// Planning Contract Bin Actions.
        /// </summary>
        /// <param name="ds">IssueReturnDataSet</param>
        /// <param name="pcNeqQtyAction"></param>
        /// <param name="pcNeqQtyMessage"></param>
        /// <param name="pcPCBinAction"></param>
        /// <param name="pcPCBinMessage"></param>
        /// <param name="pcOutBinAction"></param>
        /// <param name="pcOutBinMessage"></param>
        public void MasterInventoryBinTests(ref IssueReturnTableset ds, out string pcNeqQtyAction, out string pcNeqQtyMessage, out string pcPCBinAction, out string pcPCBinMessage, out string pcOutBinAction, out string pcOutBinMessage)
        {
            pcNeqQtyAction = string.Empty;
            pcNeqQtyMessage = string.Empty;
            pcPCBinAction = string.Empty;
            pcPCBinMessage = string.Empty;
            pcOutBinAction = string.Empty;
            pcOutBinMessage = string.Empty;

            CurrentFullTableset = ds;

            string contractID = string.Empty;
            string jobNum = string.Empty;
            int asmSeq;
            int mtlSeq;
            string binNum = string.Empty; ;
            string whseCode = string.Empty; ;

            ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                             where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                             select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                bool fromTypeIsStock;
                if (tranTypeRequiresNegInvTest(ttIssueReturn.TranType, ttIssueReturn.ProcessID, out fromTypeIsStock))
                    LibNegInvTest.NegativeInventoryTest(ttIssueReturn.PartNum, ttIssueReturn.FromWarehouseCode, ttIssueReturn.FromBinNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, ttIssueReturn.FromPCID, ttIssueReturn.UM, ttIssueReturn.DimConvFactor, ttIssueReturn.TranQty, out pcNeqQtyAction, out pcNeqQtyMessage);

                // If moving the PCID as a whole STK-STK, ensure that the PCID exists in the MtlQueue from location. NegativeInventoryTest does not verify entire PCID contents 
                if (fromTypeIsStock
                    && (ttIssueReturn.ProcessID.Equals("MaterialQueue", StringComparison.OrdinalIgnoreCase)
                        || ttIssueReturn.ProcessID.Equals("HHMaterialQueue", StringComparison.OrdinalIgnoreCase)
                        || ttIssueReturn.ProcessID.Equals("HHAutoSelectTransactions", StringComparison.OrdinalIgnoreCase))
                    && ttIssueReturn.TranType.Equals("STK-STK", StringComparison.OrdinalIgnoreCase)
                    && String.IsNullOrEmpty(ttIssueReturn.PartNum)
                    && (!String.IsNullOrEmpty(ttIssueReturn.FromPCID))
                    && ttIssueReturn.FromPCID.KeyEquals(ttIssueReturn.ToPCID)
                    && ttIssueReturn.TranQty == 1)
                {
                    using (var libPackageControlValidations = new Internal.Lib.PackageControlValidations(Db))
                    {
                        PkgControlHeaderWhsAndStatus WhsAndStatusPkgControlHeader = FindFirstPkgControlHeaderPartialRow(Session.CompanyID, ttIssueReturn.FromPCID);
                        if (WhsAndStatusPkgControlHeader != null)
                        {
                            libPackageControlValidations.PCIDExistsInLocationValidation(
                                WhsAndStatusPkgControlHeader.Plant,
                                WhsAndStatusPkgControlHeader.WarehouseCode,
                                WhsAndStatusPkgControlHeader.BinNum,
                                Session.PlantID,
                                ttIssueReturn.FromWarehouseCode,
                                ttIssueReturn.FromBinNum);
                        }
                    }
                }

                if (fromTypeIsStock)
                {
                    jobNum = ttIssueReturn.ToJobNum;
                    asmSeq = ttIssueReturn.ToAssemblySeq;
                    mtlSeq = ttIssueReturn.ToJobSeq;
                    binNum = ttIssueReturn.FromBinNum;
                    whseCode = ttIssueReturn.FromWarehouseCode;
                }
                else
                {
                    jobNum = ttIssueReturn.FromJobNum;
                    asmSeq = ttIssueReturn.FromAssemblySeq;
                    mtlSeq = ttIssueReturn.FromJobSeq;
                    binNum = ttIssueReturn.ToBinNum;
                    whseCode = ttIssueReturn.ToWarehouseCode;
                }

                if (mtlSeq != 0)
                {
                    contractID = this.ExistsJobMtlContractID(Session.CompanyID, jobNum, asmSeq, mtlSeq);
                }
                else
                {
                    contractID = this.ExistsJobAsmblContractID(Session.CompanyID, jobNum, asmSeq);
                }

                if (ttIssueReturn.TranType.Equals("PUR-STK", StringComparison.OrdinalIgnoreCase) || ttIssueReturn.ProcessID.Equals("MaterialQueue", StringComparison.OrdinalIgnoreCase) || ttIssueReturn.ProcessID.Equals("HHMaterialQueue", StringComparison.OrdinalIgnoreCase) || ttIssueReturn.ProcessID.Equals("HHAutoSelectTransactions", StringComparison.OrdinalIgnoreCase))
                {
                    MtlQueue = this.FindFirstMtlQueue(ttIssueReturn.MtlQueueRowId);
                    if (MtlQueue != null)
                    {
                        switch (MtlQueue.TranType.ToUpperInvariant())
                        {
                            case "PUR-STK":
                                {
                                    contractID = this.ExistsPORelContractID(Session.CompanyID, MtlQueue.PONum, MtlQueue.POLine, MtlQueue.PORelNum);
                                }
                                break;
                            case "STK-STK":
                                {
                                    binNum = ttIssueReturn.ToBinNum;

                                    var bPlanContractHdrWhseBin = this.FindFirstPlanContractHdrWhseBin(Session.CompanyID, MtlQueue.ToWhse, MtlQueue.ToBinNum);
                                    if (bPlanContractHdrWhseBin != null)
                                    {
                                        contractID = bPlanContractHdrWhseBin.ContractID;
                                    }
                                }
                                break;
                        }
                    }
                }

                //Logic for "Non-Planning Contract Bin Action"
                if (!string.IsNullOrEmpty(contractID))
                {
                    var binCheck = this.FindFirstPCWarehouseBin(Session.CompanyID, contractID);
                    //Validate PC Inventory Location & PC Receiving Location.                
                    if (!ExistsPlanContractWhseBin(Session.CompanyID, contractID, binNum))
                    {
                        if (!binCheck.NonPCBinAction.Equals("None", StringComparison.OrdinalIgnoreCase))
                        {
                            pcPCBinAction = binCheck.NonPCBinAction;
                            if (binCheck.NonPCBinAction.Equals("Stop", StringComparison.OrdinalIgnoreCase))
                            {
                                pcPCBinMessage = Strings.PlanContractStop;
                            }
                            else
                            {
                                pcPCBinMessage = Strings.PlanContractWarn;
                            }
                        }

                    }
                }
                //Logic for "When Used Outsite the Contract Action".
                {
                    //Validate Planning Contract Inventory Location             
                    var bPlanContractHdrWhseBin = this.FindFirstPlanContractHdrWhseBin(Session.CompanyID, whseCode, binNum);
                    if (bPlanContractHdrWhseBin != null && !bPlanContractHdrWhseBin.ContractID.KeyEquals(contractID) && !bPlanContractHdrWhseBin.NonPCOutsideAction.Equals("None", StringComparison.OrdinalIgnoreCase))
                    {
                        pcOutBinAction = bPlanContractHdrWhseBin.NonPCOutsideAction;
                        if (pcOutBinAction.Equals("Stop", StringComparison.OrdinalIgnoreCase))
                        {
                            pcOutBinMessage = string.IsNullOrEmpty(contractID) ? Strings.PlanContractOutsideStop : Strings.PlanContractOutsideAnotherPCStop;
                        }
                        else
                        {
                            pcOutBinMessage = string.IsNullOrEmpty(contractID) ? Strings.PlanContractOutsideWarn : Strings.PlanContractOutsideAnotherPCWarn;
                        }
                    }
                }
            }
        }

        private bool tranTypeRequiresNegInvTest(string tranType, string processID, out bool fromTypeIsStock)
        {
            string fromType = getFromType(tranType);
            fromTypeIsStock = false;

            // STK-INS - With Stock to inspection transactions, the inventory is removed from the bin when the material queue record is created.
            // Therefore, the program that creates the STK-INS material queue record is responsible for performing the negative inventory check.
            if (tranType.Equals("STK-INS", StringComparison.OrdinalIgnoreCase) &&
                (processID.Equals("MaterialQueue", StringComparison.OrdinalIgnoreCase) || processID.Equals("HHMaterialQueue", StringComparison.OrdinalIgnoreCase) || processID.Equals("HHAutoSelectTransactions", StringComparison.OrdinalIgnoreCase)))
                return false;

            // STK-* - This also includes RAU-* / RMG-* / RMN-* which are reported as STK by getFromType
            if (fromType.Equals("STK", StringComparison.OrdinalIgnoreCase))
            {
                fromTypeIsStock = true;
                return true;
            }

            // PUR-STK - this is actually a STK-STK move - inventory has already been received
            if (tranType.Equals("PUR-STK", StringComparison.OrdinalIgnoreCase))
                return true;

            // CMP-SHP - kit components
            if (tranType.Equals("CMP-SHP", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        /// <summary>
        /// Method to check values for whsebin. 
        /// </summary>
        /// <param name="ttIssueReturn"></param>
        public void checkWhseBin(IssueReturnRow ttIssueReturn)
        {
            if (String.IsNullOrEmpty(ttIssueReturn.ToBinNum) && !string.IsNullOrEmpty(ttIssueReturn.PartNum) && !string.IsNullOrEmpty(ttIssueReturn.ToWarehouseCode))
            {
                PlantWhse = FindFirstPlantWhse2(Session.CompanyID, Session.PlantID, ttIssueReturn.PartNum, ttIssueReturn.ToWarehouseCode);
                if (PlantWhse != null)
                {
                    WhseBin = this.FindFirstWhseBin(Session.CompanyID, ttIssueReturn.ToWarehouseCode, PlantWhse.PrimBin);
                    ttIssueReturn.ToBinNum = PlantWhse.PrimBin;
                    ttIssueReturn.ToBinNumDescription = ((WhseBin != null) ? WhseBin.Description : "");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ttIssueReturn"></param>
        public void FillForeignFields(IssueReturnRow ttIssueReturn)
        {
            Part part = null;
            if (!string.IsNullOrEmpty(ttIssueReturn.PartNum))
                part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);

            if (part != null)
            {
                ttIssueReturn.PartPartDescription = part.PartDescription;
                ttIssueReturn.PartPricePerCode = part.PricePerCode;
                ttIssueReturn.PartSalesUM = part.SalesUM;
                ttIssueReturn.PartSellingFactor = part.SellingFactor;
                ttIssueReturn.PartTrackDimension = part.TrackDimension;
                ttIssueReturn.PartTrackLots = part.TrackLots;
                ttIssueReturn.PartTrackSerialNum = part.TrackSerialNum;
                ttIssueReturn.PartIUM = part.IUM;
            }
            else
            {
                ttIssueReturn.PartPartDescription = string.Empty;
                ttIssueReturn.PartPricePerCode = string.Empty;
                ttIssueReturn.PartSalesUM = string.Empty;
                ttIssueReturn.PartSellingFactor = decimal.Zero;
                ttIssueReturn.PartTrackDimension = false;
                ttIssueReturn.PartTrackLots = false;
                ttIssueReturn.PartTrackSerialNum = false;
                ttIssueReturn.PartIUM = string.Empty;
            }

            if (!string.IsNullOrEmpty(ttIssueReturn.PartNum) && !string.IsNullOrEmpty(ttIssueReturn.DimCode))
                ttIssueReturn.DimCodeDimCodeDescription = FindFirstPartDimDimCodeDescription(Session.CompanyID, ttIssueReturn.PartNum, ttIssueReturn.DimCode);
            else
                ttIssueReturn.DimCodeDimCodeDescription = string.Empty;


            if (!string.IsNullOrEmpty(ttIssueReturn.ReasonType) && !string.IsNullOrEmpty(ttIssueReturn.ReasonCode))
                ttIssueReturn.ReasonCodeDescription = FindFirstReasonDescription(Session.CompanyID, ttIssueReturn.ReasonType, ttIssueReturn.ReasonCode);
            else
                ttIssueReturn.ReasonCodeDescription = string.Empty;

            if (!string.IsNullOrEmpty(ttIssueReturn.PartNum) && !string.IsNullOrEmpty(ttIssueReturn.LotNum))
                ttIssueReturn.LotNumPartLotDescription = FindFirstPartLotDescription(Session.CompanyID, ttIssueReturn.PartNum, ttIssueReturn.LotNum);
            else
                ttIssueReturn.LotNumPartLotDescription = string.Empty;

            if (ExistsPlantConfCtrlEnablePackageControl(Session.CompanyID, Session.PlantID, true))
                ttIssueReturn.PlantConfCtrlEnablePackageControl = true;
            else
                ttIssueReturn.PlantConfCtrlEnablePackageControl = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipcalledFrom"></param>
        /// <param name="ipPCID"></param>
        /// <param name="ipQty"></param>
        /// <param name="opWarning"></param>
        /// <param name="opAction"></param>
        /// <param name="opAllocWarning"></param>
        /// <param name="opAllocAction"></param>
        public void CheckPackageCodeAllocNegQty(string ipcalledFrom, string ipPCID, decimal ipQty, out string opWarning, out string opAction, out string opAllocWarning, out string opAllocAction)
        {
            string vPackageCode = string.Empty;
            opWarning = string.Empty;
            opAction = string.Empty;
            opAllocAction = string.Empty;
            opAllocWarning = string.Empty;

            if (string.IsNullOrWhiteSpace(ipPCID)) return;

            PkgControlHeader = this.FindFirstPkgControlHeader(Session.CompanyID, ipPCID);
            if (PkgControlHeader == null)
            {
                var PkgControlStageHeader = this.FindFirstPkgControlStageHeader(Session.CompanyID, ipPCID);
                if (PkgControlStageHeader == null)
                {
                    throw new BLException(Strings.RecordNotFound);
                }
                else
                {
                    if (PkgControlStageHeader.PkgControlStatus.KeyCompare("EMPTY") != 0) return; //have to check only once per pcid
                    if (this.libPackageControl == null) this.Initialize();  //for unit testing
                    IList<string> PCIDList = new List<string>();
                    PCIDList.Add(PkgControlStageHeader.PCID);
                    this.LibAdjustReturnContainer.CheckPackageCodeAllocNegQty(ipcalledFrom, PkgControlStageHeader.PkgCode, ipQty, out opWarning, out opAction, out opAllocWarning, out opAllocAction, PCIDList);
                }
            }
            else
            {
                if (PkgControlHeader.PkgControlStatus.KeyCompare("EMPTY") != 0) return; //have to check only once per pcid
                if (this.libPackageControl == null) this.Initialize();  //for unit testing
                IList<string> PCIDList = new List<string>();
                PCIDList.Add(PkgControlHeader.PCID);
                this.LibAdjustReturnContainer.CheckPackageCodeAllocNegQty(ipcalledFrom, PkgControlHeader.PkgCode, ipQty, out opWarning, out opAction, out opAllocWarning, out opAllocAction, PCIDList);
            }
        }

        // auto select serial numbers if the issue requires SN, is from a PCID and all PCID ItemQty is being issued, and no serial numbers have been selected yet.
        private void autoGenSelectedSerialNumbersForPCIDIssue(IssueReturnRow ttIssueReturn)
        {
            if (string.IsNullOrEmpty(ttIssueReturn.FromPCID) || string.IsNullOrEmpty(ttIssueReturn.PartNum) || !ttIssueReturn.EnableSN)
            {
                return;
            }

            string tranTypesAllowedToAutoSelectSN = "STK-MTL,STK-ASM";
            if (StringExtensions.Lookup(tranTypesAllowedToAutoSelectSN, ttIssueReturn.TranType) == -1)
            {
                return;
            }

            if (((from ttSelectedSerialNumbers_Row in CurrentFullTableset.SelectedSerialNumbers
                  where !ttSelectedSerialNumbers_Row.Deselected &&
                  ttSelectedSerialNumbers_Row.SourceRowID == ttIssueReturn.SysRowID
                  select ttSelectedSerialNumbers_Row).Any()))
            {
                return;
            }

            decimal pcidItemQty = FindFirstPkgControlItemQty(ttIssueReturn.Company, ttIssueReturn.FromPCID, ttIssueReturn.PartNum, ttIssueReturn.LotNum, ttIssueReturn.AttributeSetID, ttIssueReturn.UM);

            if (ttIssueReturn.TranQty > 0 && (pcidItemQty != ttIssueReturn.TranQty)) { return; }

            IEnumerable<SerialNo> SerialNoRows = SelectSerialNoForPCIDIssue(ttIssueReturn.Company, ttIssueReturn.FromPCID, ttIssueReturn.PartNum, ttIssueReturn.AttributeSetID, ttIssueReturn.LotNum, "INVENTORY", false);
            foreach (SerialNo serialNo_iterator in SerialNoRows)
            {
                SerialNo = serialNo_iterator;

                ttSelectedSerialNumbers = new SelectedSerialNumbersRow();
                BufferCopy.Copy(SerialNo, ref ttSelectedSerialNumbers);

                ttSelectedSerialNumbers.Deselected = false;
                ttSelectedSerialNumbers.PreventDeselect = false;
                ttSelectedSerialNumbers.PreDeselected = false;
                ttSelectedSerialNumbers.TransType = ttIssueReturn.TranType;
                ttSelectedSerialNumbers.RowMod = "A";
                ttSelectedSerialNumbers.SourceRowID = ttIssueReturn.SysRowID;
                ttSelectedSerialNumbers.SysRowID = new Guid();
                ttSelectedSerialNumbers.NotSavedToDB = true;
                ttSelectedSerialNumbers.SysRowID = SerialNo.SysRowID;
                CurrentFullTableset.SelectedSerialNumbers.Add(ttSelectedSerialNumbers);
            }
        }

        private bool toPCIDIsOutboundPCIDInOutboundPlant(IssueReturnRow ttIssueReturn)
        {
            // if the to plant is controlling serial processing and to plant is outbound tracking 
            // and the parts are going into an outbound PCID serial numbers will be needed.
            bool toPCIDIsOBContainerInOBPlant = false;
            if (!string.IsNullOrEmpty(ttIssueReturn.ToPCID))
            {
                string toPlant = ((ttIssueReturn.SerialControlPlantIsFromPlt) ? ttIssueReturn.SerialControlPlant2 : ttIssueReturn.SerialControlPlant);
                int toPlantTracking = this.LibSerialCommon.isSerTraPlantType(toPlant);
                if ((toPlantTracking == 3) && (GetPkgControlHeaderOutboundContainer(ttIssueReturn.Company, ttIssueReturn.ToPCID)))
                    toPCIDIsOBContainerInOBPlant = true;
            }
            return toPCIDIsOBContainerInOBPlant;
        }

        /// <summary>
        /// Returns true if there are any PartAllocSerial records for the company/part, consistent with InvTransfer.
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="partNum"></param>
        /// <returns></returns>
        public bool AreSNumsAllocated(string companyID, string partNum)
        {
            return ExistsPartAllocSerial(companyID, partNum);
        }

        private void TransferPCIDToNewBin(string company, string plant, string pcid, string fromWarehouseCode, string fromBinNum, string moveToWarehouseCode, string moveToBinNum, string tranReference, ErpContext moveContext)
        {
            string moveErrorMsg = string.Empty;
            string fromWhsePlant = FindFirstWarehsePlant(company, fromWarehouseCode);
            string toWhsePlant = FindFirstWarehsePlant(company, moveToWarehouseCode);

            try
            {
                using (var serviceInvTransfer = Ice.Assemblies.ServiceRenderer.GetService<InvTransferAlias::Erp.Contracts.InvTransferSvcContract>(moveContext))
                {
                    InvTransferAlias::Erp.Tablesets.InvTransferTableset InvTransferTS = new InvTransferAlias::Erp.Tablesets.InvTransferTableset();
                    InvTransferAlias::Erp.Tablesets.InvTransRow ttInvTrans = new InvTransferAlias::Erp.Tablesets.InvTransRow();

                    InvTransferTS.InvTrans.Add(ttInvTrans);
                    ttInvTrans.Company = company;
                    ttInvTrans.Plant = plant;
                    ttInvTrans.Plant2 = plant;
                    ttInvTrans.TranDate = CompanyTime.Now().Date;
                    ttInvTrans.FromPlant = !String.IsNullOrEmpty(fromWhsePlant) ? fromWhsePlant : plant;
                    ttInvTrans.FromWarehouseCode = fromWarehouseCode;
                    ttInvTrans.FromBinNum = fromBinNum;
                    ttInvTrans.ToPlant = !String.IsNullOrEmpty(toWhsePlant) ? toWhsePlant : plant;
                    ttInvTrans.ToWarehouseCode = moveToWarehouseCode;
                    ttInvTrans.ToBinNum = moveToBinNum;
                    ttInvTrans.TransferQtyUOM = String.Empty;
                    ttInvTrans.TransferQty = 1;
                    ttInvTrans.TranReference = Strings.TransferOrderReceiptChangePICDLocation + ": " + tranReference;
                    ttInvTrans.PCID = pcid;
                    ttInvTrans.RowMod = IceRow.ROWSTATE_UPDATED;

                    string legalNumberMessage;
                    string partTrankPKs;

                    serviceInvTransfer.CommitTransfer(ref InvTransferTS, out legalNumberMessage, out partTrankPKs);

                    InvTransferTS.InvTrans.Clear();
                }
            }
            catch (Exception ex)
            {
                moveErrorMsg = Strings.PCIDInventoryTransferFailed(pcid) + ": " + ex.Message;
                throw new BLException(moveErrorMsg, "IssueReturn");
            }
        }

        /// <summary>
        /// Returns a whereclause for To Bin Num searches
        /// </summary>
        /// <param name="tranType">Tran Type from IssueReturn</param>
        /// <param name="toWarehouseCode">To Warehouse Code from IssueReturn</param>
        /// <param name="toBinNum">To Bin Num from IssueReturn (can be blank)</param>
        /// <param name="toBinNumWhereClause"></param>
        public void GetToBinNumWhereClause(string tranType, string toWarehouseCode, string toBinNum, out string toBinNumWhereClause)
        {
            toBinNumWhereClause = String.Empty;

            if (!String.IsNullOrEmpty(toBinNum))
            {
                toBinNumWhereClause = "BinNum = '" + toBinNum + "'";
            }

            if (!String.IsNullOrEmpty(toBinNumWhereClause))
            {
                toBinNumWhereClause += " AND ";
            }

            switch (tranType.ToUpperInvariant())
            {
                case "RAU-STK":
                case "RMN-STK":
                case "RMG-STK":
                case "STK-STK":
                case "STK-SHP":
                    toBinNumWhereClause += "WareHouseCode = '" + toWarehouseCode + "' AND Inactive = false";
                    break;
                case "PUR-SMI":
                    toBinNumWhereClause += "WareHouseCode = '" + toWarehouseCode + "' AND BinType = 'Supp' AND Inactive = false";
                    break;
                case "PUR-CMI":
                    toBinNumWhereClause += "WareHouseCode = '" + toWarehouseCode + "' AND BinType = 'Cust' AND Inactive = false";
                    break;
                default:
                    toBinNumWhereClause += "WareHouseCode = '" + toWarehouseCode + "' AND (BinType = 'Std' OR BinType = 'Cont') AND Inactive = false";
                    break;
            }
        }

        private void RefreshAttributeDescriptions()
        {
            if (ttIssueReturn == null)
            {
                return;
            }

            using (var inventoryTracking = new InventoryTracking(Db))
            {
                inventoryTracking.RefreshAttributeDescriptions(ref ttIssueReturn);
            }
        }

    }
}