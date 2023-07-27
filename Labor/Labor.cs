#region Pre 10 Change Comments
//==========================================================================================
//FixUpSvcConversion: Strings Extraction Completed at: 7/21/2012 7:28 PM
//==========================================================================================
/*------------------------------------------------------------------------
  File        :  Labor Entry Business Object
  Author(s)   : Jennifer Johnson 
  Created     : 06/02/03
  Notes       : scr 10557 
    
Revision History:
-----------------
*** Histoy previous to 2008 is on _history01
01/11/08 AlbertoC - SCR 31257 - Changes were rolled back.
02/26/08 DanielG scr41083 - Modified reference {lib/RoundAmount.i} to {lib/RoundAmountEF.i}
02/28/08 AlexanderP SCR 40650 (opr. 360) - Rounding of variables and fields according to the currency setup.
02/29/08 AlbertoC SCR 40600 - UOM Changes.
03/03/08 HeribertoC SCR 40600 - LaborDtl.LaborUOM, LaborDtl.ScrapUOM, LaborDtl.DiscrepUOM and LaborPart.PartUOM were added and populated.
                (GetRowsWhoIsHere was adjusted on its Params tags just to accomplish with BO Generation)
03/21/08 GlennB   - SCR 49318 - Modified getLaborDtlBurdenRates to get correct BurdenRates using Resource/ResourceGroup.                                
04/11/08 gjh - SCR 40761 - Serial number changes
04/24/08 gjh - SCR 50028 - Changed EnableSN logic
05/02/08 JeanetteP  SCR 49381 - Modified the DefaultOprSeq procedure to assign the correct LaborRate for Quantity Only 
                    labor before calling the chgWcCode procedure to calculate the Burden Rate correctly.
06/04/08 VladimirD scr 42513 - Inventory: GetWIPGL, GetINVGl,GetCOSGL,GLDiv*,GLDep*,GLChart* fields should not be used 
                             Hardcoded logic was moved to booking rules of Inventory transaction.                    
09/16/08 jajohnson - fixed SysRevID mismatch issue during calcLaborHours
09/29/08 AlbertoC - SCR 55891 - Modified proc GetRowsWhoIsHere, changed its firm and parameters passed to GetRows.
10/14/08 DebbieP - SCR-51288 - ValidateSerialBeforeSelect will return total reqiured qty for SN validation
10/15/08 gjh - SCR#40099 - Serial Number changes.
11/06/08 - gjh - SCR#57310 - Changes to SNTran/SerialNo updates.
11/07/08 - AlbertoC - SCR 57389 - Exclude updated and get for table ttSNFormat.
12/10/08 - AlbertoC - SCR 58145 - Modified laborHedAfterUpdate, refresh LaborDtl records if LaborHed.PayrollDate has changed.
01/22/09 YulianaR -57343- Update laborDtlBeforeUpdate to add a validation for mes you can`t start a activity without a ResourceGroup.        
01/23/09 - andreaP scr 59100 - LaborDtlafterGetRows was not populating UOM fields.   
01/28/09 - RicardoS - SCR 57362 - Modified SetComplete procedure for the correct setting of the flag.
02/04/09 - gjh - SCR#58512 - Added additional check to Serial Number validation to see if SN was already reported on the current opr on another LaborHed/Dtl.
02/05/09 IgnacioL SCR 52733 - Added a return in summAllBurdenCost Procedure  so it doesnt loop anymore after it found a ResourceGroupID
                            to prevent double burden costs.
02/23/09 - DebbieP - SCR-57631 - Add PatchFld logic for SerialNo, SNTran.
03/04/09 - RicardoS - SCR 60586 - Modified genLaborPart. Prevent updated by another user error.
03/18/09 dparillo SCR 48868 - modified LaborDtlSetDefaults to handle hours after midnight.
04/10/09 dparillo SCR 12952 - changed input parameters for GetNextOprSeq
05/04/09 Pradeep scr60788 - Who is Here tab shows constraint error when more than 100 employees are clocked in.
05/05/09 Pradeep scr61890 - Modified code to exclude scrapped serial numbers from quantity needed.
06/23/09 AlbertoC SCR  63710 - Modified proc validateSerialAvail, modified code to exclude LaborQty and NonConfQty from other LaborDtl records.
07/07/09 AlbertoC SCR 61014 - Added new logic for Project Billing.
07/15/09 LorenS SCR 61012 - Opr 375 - Implementation of new JobTypes "PRJ" & "MNT".
07/17/09 AlbertoC SCR 61014 - TimeTypCd and PrjRoleCd cannot be changed if LaborDtl.PBInvNum has data.
08/11/09 AlbertoC - SCR 61014 - Rework for Role Code.
08/11/09 AlbertoC - SCR 61014 - Modified code that sets fields used to disable RoleCd and TimeTypCd.
08/14/09 gjh - SCR#63285 - Added support for starting multiple activities at once from WorkQueue
08/13/09 andreaP - scr 65573 - modify rules for concurrent mode vs. sequential process mode.
08/18/09 andreaP - scr 65573 - rework - referenced a tt record when I should not have in chkPartQty
08/20/09 Orv - SCR61012: Maint Mgmt, added logic to set fields when Job is a maintenance job.
09/22/09 CeciliaL - SCR 61012 - laborDtlAfterUpdate: Added code in order to create LaborEquip.
09/23/09 CeciliaL - SCR 61012 - Added ChangeEquipID procedure.
10/09/09 AbrahamG - SCR 61011 - Added LaborDtl.EnableInspection external field. New logic added to validate Inspection options.
10/25/09 AbrahamG - SCR 66785 - ValidateSerialAvail - Added condition when obtaining the total Available, to not consider the previous SN assigned to the Job.
12/03/09 Pradeep - scr68317 - Removed the ProjectID condition when checking for Role Code.
12/08/09 scottr - scr63319 - partially approved transactions can be recalled
12/09/09 djy - scr69467 - laborDtlSetDefaults needs to be called every time we create ttLaborDtl without calling GetNew
12/11/09 YuriR  - scr68212 - validateJobOper: Service Call is not allowed; DefaultOprSeq: set default Role from the Operation;
12/14/09 DJY - scr69485 - unable to modify rejected time, modified edit in laborDtlBeforeUpdate
12/15/09 DJY - scr69508 - laborDtlSetDefaults should not reset LaborType on a copied row
12/06/09 DJY - scr69418 - validations should happen in the same order on TimeWeeklyView as on LaborDtl
12/21/09 YuriR - SCR 68326 - Modified: validateProject - validation of Role, validateJobOper - Service Call is not allowed;
01/06/10 DJY - scr70458 - tweak assign of ttLaborDtl.LaborDtlSeq to work for the many ways Time and Expense Entry 
                          creates Labor as well as how Start Activity creates it
01/07/10 DJY - scr70148 - rearrange order of assignment/validation calls in OnChangeQuickEntryCode
01/13/10 DJY - scr70695 - GetNew logic, handle a GetNew in T&E of LaborDtl with a LaborHed in context
01/14/10 scottr - scr70646 - labor entered through MES not going through approval process
01/18/10 scottr scr 61010 - TimeDisableUpdate and TimeDisableDelete flags not getting set properly when approvals are not required
01/21/10 scottr scr 61010 - rework logic for TimeDIsableUpdate and TimeDisableDelete flags
01/27/10 andreaP scr 70089 - clear project fields when laborType changes from project to something else.
01/28/10 karinam scr 70013 - DefaultProjectID: Clear PhaseIDDescription field when select a new ProjectID.
01/29/10 andreaP scr 70549 - set ResourceGrpID and ResourceID from EmpBasic for Indirect LaborTypes.
02/01/10  djy  scr68324 - laborDtlSeq and laborHedSeq setting in ttLbrScrapSerialNumbers when set in ttLaborDtl
02/02/10 JulioDV  SCR 68062 - Modified validateSerialAvail to count the number of completed,nonconformance and scrapped serial numbers.
02/02/10 DebbieP SCR-68062 - Modified validateSerialAvail - rework of SN count, deleteSerialNumbers - clean up SerialNO/SNTran
02/03/10  djy  scr71343 - changing date on ttLaborDtl was not switching properly to an existing ttLaborHed if it already existed
02/03/10  djy  scr68333 - CopyLaborDetail should not change current TimeStatus
02/08/10 scottr scr69440 - validate job types of prj are not used on a non-project labor record
02/09/10 andreaP scr 68830 - add new laborDtlComment.CommentType flag - INV (Invoice).
02/10/10 dparillo scr 70945 - set LaborTypePseudo = LaborType in StartActivity
02/10/10  djy  scr69453 - ClockInTime and ClockOutTime are now defaulting properly on ttLaborDtl
02/10/10  djy  scr69363 & 70287 - LaborHrs and BurdenHrs are now defaulting properly on ttLaborDtl
02/11/10  djy  scr69457 - Booked Hours was not filtering by employee
02/11/10  djy  scr71130 - validate that at least one day bucket has hours on a new Weekly Time record, otherwise validate.
02/17/10 scottr scr 71228 - remove transactions the current user does not have rights to view
02/19/10 dparillo scr 65036 - modified afterUpdate.
02/19/10 andreaP scr 71138 - if employee set to not allow booking to direct time, set new LaborDtl to LaborType "I" rather than "P".
02/21/10 DebbieP SCR 69599 - disPrjFields, disPrjFieldsTimeWeeklyView added code for RoleCdDisplayAll/ChkEmpPrjRole
02/22/10 scottr scr 71228 - correct issue with setting disableeupdate flag for weekly view
02/23/10 andreaP scr 71138 - change logic for what type of LaborType are valid when 'Allow to book to direct jobs' flag is false.
02/24/10 andreaP scr 72323 - PlantConfCtlr record not available in setUpdateRules method.
03/03/10 andreaP scr 72586 - set status back to 'E'ntered when approver changes the record.
03/04/10 scottr scr 71232 - set fields for enabling and disabling buttons for the weekly view
03/05/10 YuriR  scr 72584 - Time Type is available only for Time&Material projects, error messages are modified. 
03/05/10  djy  scr69390 - consider employee calendar exceptions when calculating work hours
03/09/10 DebbieP SCR-72589 - changes to role code and time type logic throughout to conform with Time and Expense design specifications
03/10/10 DebbieP SCR-72589 - if role code is changed, clear time type code; fixed role code defaulting based on project contract type
03/12/10 DebbieP SCR-72589 - time type is never required, validate role rate even if time type is blank.
03/13/10 scottr scr 72966 - quick entry values not being brought into the time record
03/16/10 andreaP scr 73122 - OnChangeQuickEntryCode throwing error when type is project.
03/17/10 gjh SCR#73136 - Added external MES flag to LaborDtl and LaborHed. The MES flag is used to get around some of the new update 
                         functionality that broke MES (it is checked in beforeUpdate).
03/18/10 scottr scr 73216 - time restrict entry option not working properly                         
03/19/10 SergeyK SCR 73183 - Prevent crearing of RoleCd and TimeTypeCd in the LaborDtlAfterGetRows procedure.
03/19/10 scottr scr 73118 - handle change of status when updated by an approver in the write trigger
03/22/10 SergeyK SCR 73121 - Set default value to RoleCd field from employee's primary role. disPrjFields procedure was modified.
03/24/10 gjh SCR#73302 - SelectForWork labor type was being changed from "S" to "P".
03/31/10 andreaP scr 71088 - fields not clearing properly on timeWeeklyView when LaborType changes.
04/02/10 DebbieP SCR-73840 - beforeUpdate: add code to set ttLaborDtl cost and burden for TimeWeeklyView
04/05/10 scottr scr 73860 - submit on weekly view was not submitting the records
4/5/10 sep 70153 - blank out scrap reason code if scrap qty = 0
04/06/10 jhMartinez SCR 70997 - laborDtlCommentAfterGetRows: assign LaborDtlComment.TreeNodeImageName.
04/07/10 jhMartinez SCR 73045 - refreshTtLaborDtl: exclude UD fields on buffer-copy.
04/09/10 jhMartinez SCR 70094 - laborDtlAfterGetRows: assign LaborDtl.EnteredOnCurPlant to know if the LaborDtl was created on the currrent plant.
04/09/10 jhMartinez SCR 73872 - modified DefaultShift proc to default shift times.
04/12/10 scottr scr 68872 - allow recall of approved transaction if not yet posted to wip
04/16/10 andreaP scr 71138 - rework some code for Allow to book direct jobs.
04/28/10 scottr scr74690 - correct issue with setting enablesubmit and enablerecall fields
05/06/10 JulioDV SCR 74904 - getSerialNumbers method: Added PartNum to SNTran For-Each where clause and used no-lock clause to impove performance.
05/07/10 YuriR  SCR-73121 - added untranslated option U
05/10/07 scottr scr 73118 - correct enabling of fields on a submitted transaction
05/18/10 RicardoS - SCR 60368 - Modified Overrides and OverridesResource. Fixed Labor Rate. 
05/18/10 Todd - SCR 74196- Changes to the time weekly view. 
05/21/10 scottr - scr 74524 - don't return weekly view records if time management license is not active
05/24/10 scottr - scr 75656 - from weekly view recall only the records in the selected row(s)
05/24/10 Todd - SCR 74196- Changes to the time weekly view. 
06/02/10 scottr - scr75697 - implement new options for disallowing time entry and allow update for any employee
06/02/10 Todd - SCR 74196- Changes to the time weekly view. 
06/04/10 Todd - SCR 74196- Changes to delete in the time weekly view. 
06/07/10 Todd - SCR 75909 - Unable add and submit a negative time. 
06/09/10 scottr - scr 76049 - when submitting weekly view, the wrong approver is being assigned
06/08/10 Todd - SCR 76081  - Unable to copy a row if  there are multiple detail records for a day
06/14/10 scottr - scr76095 - weekly view - time type code not defaulting from quick code
06/17/10 VladimirJ - SCR 71211 - update Labor Expense Code when Labor code is changed.
06/17/10 scottr scr 75670 - when copying weekly time don't copy the status
06/21/10 scottr scr 76142 - quick entry defaults were getting overridden
06/22/10 scottr scr 76021 - fix performance issue in procedure validateSerialAvail
06/23/10 scottr scr 75838 - laborhed values were not populate consistently among the different ways the record was created
06/24/10 scottr scr 74690 - enablerecall flag not getting set properly
06/29/10 scottr scr 76445 - header can be updated unless processed by payroll or a labordtl has been cos wip captured
07/08/10 scottr scr 76081 - rework - department was getting overridden with blank, causing the department validation to fail
07/27/10 scottr scr 76428 - quickentrycode getting blank value on existing records
08/03/10 scottr scr 77485 - When called from MES, labordtl records are not being returned to UI.
08/11/10 scottr scr 76081 - Correct issue with changing hours on a copied weekly view record when the date is not changed
09/14/10 scottr scr 78883 - apply supervisor update rights to laborhed records
09/21/10 JessicaR SCR 76313 - Modified DefaultOprSeq and laborDtlAfterGetRows, in order to use JobHead.IUM. 
10/15/10 RicardoS SCR 72418 - Modified afterupdate and chkPartQty. It should use JobHead.IUM. 
10/26/10 scottr scr 78727 - labor rate was getting overridden when changing the resource or resource group
10/26/10 AlbertoC SCR 75228 - Modified laborDtlBeforeUpdate, save ttLaborDtl.RequestMove value in a PatchFld to use it later on the JobOper write trigger.
10/28/10 YeseniaA SCR 79708 - Modified genLaborPart, DefaultOprSeq and DefaultLaborQty to handle when labor qty should be enabled or disabled (if there are job parts).
11/11/10 scottr scr 80240 - new methods added to get and set time and expense retrieval options
11/15/10 Orv SCR80553:  Incorrect burden rate when more that one resource/group are on the job
12/03/10 scottr scr 81083 - allow service call operations when job is linked to a project
01/13/11 Orv SCR80553 Rework.
01/14/11 LorenS SCR 81979 Fix "Find First" error.
02/10/11 nzirbes scr 78761 corrected LaborDtl.LaborHrs and LaborDtl.BurHrs calculation to take lunch into consideration.  Added a
                           parameter to method payHoursDtl and updated all calls to this method.  The parameter indicates if the
                           temp table is used (create detail without a laborhed) or if the labor hed is to be retrieved.  This method
                           contains the logic to handle lunch processing. Modified method getnewLaborDtlwithHdr to not update the
                           laborDtl clock in and out times if they have been set by labordtlsetdefaults.  
03/02/11 CCORTEZ - SCR 83296 - added validation check for earliest apply date.
03/03/11 YuriR   - SCR 79920 - Invoicing method is got from couple: Project ID and WBS Phase ID
03/15/11 AbrahamM - SCR 80675 - Added External Column NewDifDateFlag used in submit time procedure in UI TimeAndExpenseEntry and server/bo/Labor.
03/17/11 LorenS - SCR 83315 - labor rate was getting overridden when changing the resource or resource group
04/05/11 CCORTEZ - SCR 83296 - Added Earliest Apply Date validation at DefaultDate. GetNewLaborHed1 procedure signature changed. Added new procedure OnChanegClockInDate.
04/06/11 CCORTEZ - SCR 83296 - Using EAD validation after validDate. Using bitt Table to ensure validation.
04/06/11 CCORTEZ - SCR 83296 - Adding EAD validation to processTimeWeekly.
04/19/11 CCORTEZ - SCR 83296 - Adding EAD validation to GetNewLaborDtlNoHdr and the copy process.
04/28/11 Pradeep - scr83342 - Complete checkbox not checked when reporting qty in job parts.
5/5/11 sep - scr 85230 - no ttlabordtl record is available error in weekly time entry
05/25/11 RPeterson SCR 85875 - Added field list to queries to improve performance.
06/09/11 DebbieP SCR-85446 - enable super SQL, change find to findtbl, changes to Throw_Private vs Throw_Public
07/27/11 YeseniaA  SCR 86516 - tmpLaborPart added in order to update JobPart.QtyCompleted correctly.. here we subtract the Old LaborPart.PartQty to all JobPart records. the new PartQty is added in LaborDtl write trg
08/19/11 RPeterson SCR 81721 - laborDtlAfterGetRows: add filling of flags that determine if the scrap field can be entered.
08/25/11 AbrahamG SCR 71777 - Changed logic to calculate burden cost. It involves now the parameter on PlantConfCtrl.ApplyBurdenCost.
08/26/11 andreaP scr 88564 - replace can-do with lookup.
09/01/11 Daperez scr 86450 - Procedure laborDtlAfterGetRows thrown log errors related with format of created and changed time labor.
09/20/11 andreaP scr 87946 - removed some preprocessor code that is obsolete and does not convert well to ICE3.
09/28/11 almartinez SCR 88809 - Disable Recall if Job is Closed.
10/03/11 dparillo SCR 88836 - modified validateserial for nonconformance and rejected/scrapped
10/12/11 CaMorales SCR 89211 - Deleted a message left in the program during tracing of another issue.
10/25/11 RPeterson SCR 81721 - laborDtlAfterGetRows: removed enabling of labor qty.  Should not be enabled/disabled here.
11/01/11 DebbieP SCR-88429 - getSerialNumbers: fix build of LbrScrapSerialNumbers from SNTran
11/01/11 CristianG SCR 61891 - Added conditions to default the Serial Number Status to Complete, Scrap or Non-Conformance for the Add Serial Number Button.
11/04/11 Almartinez SCR 85693 - New shared variables used to retrive Whse and Bin from PartWIP in pwipmtlq.p
11/07/11 Almartinez SCR 85693 - Peer Review. Use of exclusive-lock in FIND Blocks when SNTran and lastSNTran buffers are Updated/Deleted.
11/17/11 DanielM SCR 90963 - Modified validateSerialAvail, when checking if the current labor operation has SN Transactions to not check the scrap labor hed and dtl
11/17/11 CristianG SCR 76620 - Changed logic to assign value to ClockInDate on DefaultDate Procedure.
11/18/11 LorenS SCR 51079 - Serial Number Enhancement to auto receive serialized parts.
11/22/11 LorenS SCR 51079 - Serial Number enhancement.
11/22/11 almartinez SCR 88863 - Divide ttLaborDtlComment.CreateTime / 3600 before apply format ">9.99" to avoid app server log warning.
11/28/11 almartinez SCR 81309 - modify logic to re-assign oldLbrQty. Also modify logic to detect when laborQty is updated directly from LaborDtl or from LaborPart
                                This was in order to set the Complete flag automaticaly when JobOperQtyCompleted >= JobOper.RunQty.
11/30/11 LorenS SCR 51079 - Serial Number enhancement.
12/06/11 CristianG SCR 76620 - Changed logic to assign value to ActualClockInDate on DefaultDate Procedure.
12/05/11 jajohnson scr 88424 - change references of Job Part to Co-Part
12/13/11 LorenS - SCR 51079 - Serial Number enhancement.
12/13/11 LorenS - SCR 51079 - Serial Number enhancement.
12/13/11 CristianG SCR 89730 - Added validation of completed Labor Quantity when a time details is copied.
12/14/11 LorenS - SCR 51079 - Serial Number enhancement.
01/10/12 LorenS - SCR 89471 - Not allow Time Entry for unscheduled job.
01/16/12 CristianG SCR 89213 - Added filter to show the UOM of the the Sub Assembly being recorded.
01/19/12 Almartinez SCR 88840 -  Rework. Order AltLaborDtl by CLOCKINTIME descending to set new LaborDtl.CLockInTime = Last and bigger AltLaborDtl.ClockInOutTime
01/24/12 Almartinez SCR 88840 - Rework. Redesign logic to calculate correct Labor Hrs. Enhancement to identify if labor period and lunch time goes over midnight.
01/24/12 Almartinez SCR 88840 - Added missing condition to add a day to lunchIn and lunchOut when both go over midnight
01/30/12 JeanetteP SCR 92027 - Modified afterUpdate procedure to fix the logic for updating the LaborQty when the associated labor is for Concurrent Job.
02/06/12 JeanetteP SCR 88840 - Modified payHoursDtl procedure to correctly calculate the labor hours taking lunch hour into consideration.
02/08/12 YuriR SCR-76179 - laborDtlSetDefaults: default value of TimeAutoSubmit is set from Plant Configuration Control; 
02/09/12 JeanetteP SCR 88840 - Modified payHoursDtl to consider the scenario where the ttLaborDtl is being created/updated without LaborHed.
                               If LaborHed is not available then the LaborHrs should be calculated based on the Employee's Shift Lunch In/Out.
02/09/12 RosalindaV SCR 89598 - Added logic to correctly apply Labor rate in overrides method.
02/10/12 RosalindaV SCR 89598 - Added Labor logic to OverridesResource method. 
02/10/12 RosalidnaV SCR 89598 - Adding standards to methods modified: overrides and OverridesResource.
02/10/12 RosalindaV SCR 89598 - Added missing standard.
02/21/12 LorenS SCR 93868 - Undo previous fix for SCR 89471 which was done on 01/10/12.
03/14/12 dparillo SCR 93466 - added ttJCSyst and populate with JCSyst if exists. If not, assign values. Modified all instances to use ttJCSyst.
03/16/12 JeanetteP SCR 94780 - Modified the logic that calculates the sum of burden rates for the LaborDtl. If Plant is configured to use the
                               sum of burden rates from OpDtls, we added a check to see if the override values of resourcegroup/resource/capability
                               in LaborDtl match one of the OpDtl then the OpDtl's burden rate will not be included in the routine sumAllBurdenCost.
03/22/12 BrockM SCR 75440 - Modified GetRowsWhoIsHere procedure to only retrive the required records instead of getting the entire dataset.
04/02/12 JeanetteP SCR 92148 - Initial check in to fix the issue with LaborParts not generating Auto-Receipt transactions.
                               Modified afterUpdate procedure to correctly update the JobPart.QtyCompleted when entering LaborPart.PartQty.
                               The fix for this SCR undid the changes done for SCRs 53348 and 86516 and implemented the correct solution to 
                               update the completed quantity and allow Auto-Receipt logic calculate the correct costs.
04/19/12 almartinez SCR 96065 - Modify validateJob procedure for validating id ttLaborDtl exist and if not, ttTimeWeeklyView exist. To avoid no buffer 
                                availbale for ttLabroDtl in Log.
04/20/12 almartinez SCR 96065 - Add suggestion from Jeanette Pineda to assign AssemblySeq to avariable and make can-find clearer. Switch conditions
                                to validate first if ttLaborDtl exist and if not, validate ttTimeWeeklyView. Sometimes both buffers exist at the sametime.
04/20/12 CristianG SCR 76407 - Add Attachment funcionality to Time And Expense records.                               
04/20/12 almartinez SCR 96065 - Add Release to ttLaborDtl after refresh it when Racalling Weekly View to ensure that the use of the correct ttLabotDtl.
04/25/12 CristianG SCR 96252 - Added logic to change the labor detail date, modified laborDtlAfterUpdate to delete old labor dtl.
                             - Modified laborDtlBeforeUpdate to avoid validate date as validation is obsolete.
04/25/12 EduardoR  SCR 96252 - Modified beforeUpdate to remove obsolete logic.
                             - Modified OnChangeClockInDate to create a new Labor Header on each change of ClockInDate at LaborDtl.
04/25/12 RosalindaV SCR 96252 - Modified OnChangeClockInDate to add some performance enhancement.
04/25/12 EduardoR  SCR 96252 - Moved logic from OnChangeClockInDate to laborDtlAfterUpdate to create new header and update DtlSeq.
05/03/12 Almartinez SCR 96051 - Reset WBS Phase, Job, Assembly, Operation for Weekly Time when changing Project ID.
05/05/12 EduardoR  SCR 76620 & 96252 - Rollback.
05/11/12 jajohnson SCR 95805 - default Resource ID for Indirect activity from EmpBasic
05/17/12 almartinez SCR 95691 - ByPass LabotDtl and TimeWeeklyView records when trying to re-submit/re-recall them and they have already been Submited or Recalled Status.
                                Set EnableSubmit and EnableRecall fields in ttTimeWeeklyView like in ttLaborDtl in order to apply correctly the rowrules in UI for this buttons.
06/14/12 CaMorales SCR 97812 - Created variable lDefDateFromNewHeader, used to only default date of clock in date and adjusted when creating new header.
06/29/12 CristianG SCR 97924 - Added validation for Expense management or Inventory management Licenses.
07/05/12 Almartinez SCR 68226 - Assign new fields PartDescription and RevisionNum for ttLaborPart temp-table
07/11/12 Almartinez SCR 68226 - Peer Review. Set value for new Field ttLaborDtl.EnablePrintTagsList.
07/25/12 JeanetteP  SCR 99740 - 2012R Conversion
08/14/12 Almartinez SCR 95691 - Peer Review. Remove Unnedded do-end blocks. Fix comsmetic indenting and assign olApprover variable.
08/14/12 jstevermer SCR 94540 - Corrected the assignment of MtlQueue.ToWhse and MtlQueue.ToBinNum to default from PlantConfig - this use to happen that way in labordtl/write.p
08/14/12 Almartinez SCR 95691 - Peer Review. Assign correctly LaborPart Description.
08/15/12 CaMorales SCR 100685 - Added Company to the Who Is Here where clause filter in the GetRowsWhoIsHere Procedure.
08/22/12 Almartinez SCR 95691 - Peer Review changes. laborDtlCommentAfteGetRows methdo is now private and canApprove method firm was change to accept HedSeq and DtlSeq as parameters.
09/05/12 jstevermer SCR 94540 - Changed the assign of MtlQueue.ToWhse and MtlQueue.ToBinNum and moved that logic to labordtl/write.p
10/10/12 RicardoS SCR 103410 - Modified updateSerialNumbers to assign SerialNo.WareHouseCode= NonConf.ToWarehouseCode and SerialNo.Binnum = NonConf.ToBinNum.
11/07/12 NPorras SCR 89457 - Modified DefaultOprSeq, CheckWarnings and chgWcCode procedures and DEWRN100.I to move the validation when the ResourceGrp has changed from the selection of the Operation to execute when saving.
11/16/12 NPorras SCR 94819 - Mofified Labor.p to validate and show the LaborDtl.ClockInTime and LaborDtl.ClockOutTime in the proper format (Hours/Minutes 
								or Hours/Hundredths) according to the Company's configuration in the Time and Expense Entry Detail's List tab.
11/26/12 NPorras SCR 94819 - Forgot to include the :U on empty string.
01/23/13 CristianG SCR 106543 - Modified the number of decimals to calculate PayHours and PayHoursdtl, to avoid rounding issues.
01/24/12 MelissaC SCR 103893 - Modified Override proc to allow resource groups that aren't location for Indirect labor Type.
09/30/13 CristianG SCR 119858 - Added external field TimeWeeklyView.ProjPhaseID for better use of WBS Phase Combo Box.
09/30/13 CristianG SCR 119858 - Added external field LaborDtl.ProjPhaseID for better use of WBS Phase Combo Box.
12/19/14 - DiegoF	- SCR 150595 - Fixed getShiftDescription() method to show de correct format in the cmbShift.Text
----------------------------------------------------------------------*/
#endregion Pre 10 Change Comments
using Epicor.Data;
using Epicor.Hosting;
using Epicor.Utilities;
using Erp.Internal.Lib;
using Erp.Tables;
using Erp.Tablesets;
using Ice;
using Ice.Assemblies;
using Ice.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Transactions;
using Strings = Erp.BO.Labor.Resources.Strings;

namespace Erp.Services.BO
{
    /// <summary>
    /// Labor Entry Business Object
    /// </summary>    
    public partial class LaborSvc
    {
        #region Variables and Classes

        private System.Data.SqlClient.SqlConnection sqlConnection;
        /// <summary>
        /// DAYZERO DateTime(1953,10,30)
        /// </summary>  
        public static DateTime DAYZERO = new DateTime(1953, 10, 30);

        private Erp.Internal.IM.PartTranSNtranLink.SNtran2 ttSNTran;
        private List<Erp.Internal.IM.PartTranSNtranLink.SNtran2> ttSNTranRows;
        private Erp.Internal.Lib.TimeExpense.ttTEKey ttTEKey;
        private List<Erp.Internal.Lib.TimeExpense.ttTEKey> ttTEKeyRows;

        const string PHYSICAL_TABLE_07 = "ttLbrScrapSerialNumbers";
        const string PHYSICAL_TABLE_09 = "ttSelectedSerialNumbers";
        const string PHYSICAL_TABLE_10 = "ttSNFormat";
        const bool EXCLUDE_LOGICAL_TABLE_UPDATE_10 = true;
        const bool EXCLUDE_LOGICAL_TABLE_GET_10 = true;
        const bool EXCLUDE_LOGICAL_TABLE_GET_06 = true;
        string oldJobNum = string.Empty;
        int oldAssSeq = 0;
        int oldOprSeq = 0;
        decimal oldLbrQty = decimal.Zero;
        decimal oldScrapQty = decimal.Zero;
        decimal oldDiscrepQty = decimal.Zero;
        decimal oldJobQty = decimal.Zero;
        string oldWcCode = string.Empty;
        string sWhseCode = string.Empty;
        string sBinNum = string.Empty;
        string sPCID = string.Empty;
        decimal saveActProdHours = decimal.Zero;
        decimal saveActProdRwkHours = decimal.Zero;
        decimal saveActSetupHours = decimal.Zero;
        decimal saveActSetupRwkHours = decimal.Zero;
        decimal saveBurdenHrs = decimal.Zero;
        decimal saveLaborQty = decimal.Zero;
        decimal savePartQty = decimal.Zero;
        bool lRuncreateMtlqPWip = false;
        bool runEndActJobPartUpd = false;
        string modList = string.Empty;
        //public LaborDataSet.LaborDtlRow t1LaborDtl;/* KEEPS TRACK OF LABORPART THAT WAS DELETED, SEE AFTERUPDATE */
        int del_LaborPart_LaborHedSeq = 0;
        int del_LaborPart_LaborDtlSeq = 0;
        string saveResourceID = string.Empty;
        string saveResourceGrpID = string.Empty;
        decimal vBurdenRate = decimal.Zero;
        List<string> deniedColumns = null;
        bool vBurdenRateSec = false;
        int iCounter = 0;
        string sRoleCd = string.Empty;
        bool lFromGetRowsCalendarView = false;
        bool[] tt_brk_array = new bool[1440];
        int SerTraPlantType = -1;
        Hashtable hshJobAfterGetRows;   //Cache data used in ReportPartQtyAllowed() 
        private class JobAsmblRoot
        {
            public bool ExistsLaborEntityBF { get; set; }
            public int FinalOprSeq { get; set; }
            public bool JobPartQtyAllowed { get; set; }
        }


        /// <summary>
        /// Labor Entry Business Object
        /// </summary>   
        [SuppressMessage("Epicor.Server", "IceRowShouldHaveDataMemberAttributeOnColumns:IceRowShouldHaveDataMemberAttributeOnColumns")]
        public class _LaborHedRow : Erp.Tablesets.LaborHedRow
        {
            /// <summary>
            /// ExistingLaborHedSeq
            /// </summary>  
            public int ExistingLaborHedSeq { get; set; }


        }

        /// <summary>
        /// bDelttLaborHed
        /// </summary> 
        public _LaborHedRow bDelttLaborHed;
        /// <summary>
        /// bDelttLaborHedRows
        /// </summary> 
        public List<_LaborHedRow> bDelttLaborHedRows;
        /// <summary>
        /// bDelttLaborDtl
        /// </summary> 
        public LaborDtlRow bDelttLaborDtl;
        /// <summary>
        /// bDelttLaborDtlRows
        /// </summary> 
        public List<LaborDtlRow> bDelttLaborDtlRows;

        /// <summary>
        /// _LaborPartRow
        /// </summary> 
        public class _LaborPartRow : LaborPart
        {
            /// <summary>
            /// OldPartQty
            /// </summary> 
            public decimal OldPartQty { get; set; }

            /// <summary>
            /// OldScrapQty
            /// </summary> 
            public decimal OldScrapQty { get; set; }

            /// <summary>
            /// OldDiscrepQty
            /// </summary> 
            public decimal OldDiscrepQty { get; set; }
        }
        /// <summary>
        /// tmpLaborPart
        /// </summary> 
        public _LaborPartRow tmpLaborPart;
        /// <summary>
        /// tmpLaborPartRows
        /// </summary> 
        public List<_LaborPartRow> tmpLaborPartRows;

        /// <summary>
        /// SerialNoRow
        /// </summary> 
        [SuppressMessage("Epicor.Server", "IceRowShouldHaveDataMemberAttributeOnColumns:IceRowShouldHaveDataMemberAttributeOnColumns")]
        public class SerialNoRow : LbrScrapSerialNumbersRow
        {
            /// <summary>
            /// delSerialNumber
            /// </summary>  
            public string delSerialNumber { get; set; }
        }
        /// <summary>
        /// delSerialNo
        /// </summary>  
        public SerialNoRow delSerialNo;
        /// <summary>
        /// delSerialNoRows
        /// </summary>  
        public List<SerialNoRow> delSerialNoRows;
        /// <summary>
        /// AltbttTimeWeeklyView
        /// </summary>  
        public TimeWeeklyViewRow AltbttTimeWeeklyView = null;
        /// <summary>
        /// delttTimeWeeklyView
        /// </summary>  
        public TimeWeeklyViewRow delttTimeWeeklyView;
        /// <summary>
        /// delttTimeWeeklyViewRows
        /// </summary>  
        public List<TimeWeeklyViewRow> delttTimeWeeklyViewRows;

        /// <summary>
        /// delttLbrScrapSerialNumber
        /// </summary>  
        public LbrScrapSerialNumbersRow delttLbrScrapSerialNumber;
        /// <summary>
        /// delttLbrScrapSerialNumbersRows
        /// </summary>  
        public List<LbrScrapSerialNumbersRow> delttLbrScrapSerialNumbersRows;

        // Some fields of TimeWeeklyView provided its PK
        private class TimeWeekKey
        {
            string Company;
            string EmployeeNum;
            DateTime? WeekBeginDate;
            DateTime? WeekEndDate;
            string LaborTypePseudo;
            string ProjectID;
            string PhaseID;
            string TimeTypCd;
            string JobNum;
            int AssemblySeq;
            int OprSeq;
            string IndirectCode;
            string RoleCd;
            string ResourceGrpID;
            string ResourceID;
            string ExpenseCode;
            int Shift;
            string TimeStatus;
            string QuickEntryCode;
            string LaborEntryMethod;
            public string PRKey;
            public TimeWeekKey(Tables.LaborDtl LaborDtl, DateTime? ipCurrentWeekStartDate, DateTime? ipCurrentWeekEndDate)
            {
                this.Company = LaborDtl.Company;
                this.EmployeeNum = LaborDtl.EmployeeNum;
                this.WeekBeginDate = ipCurrentWeekStartDate.Value.Date;
                this.WeekEndDate = ipCurrentWeekEndDate.Value.Date;
                this.LaborTypePseudo = LaborDtl.LaborTypePseudo;
                this.ProjectID = LaborDtl.ProjectID;
                this.PhaseID = LaborDtl.PhaseID;
                this.TimeTypCd = LaborDtl.TimeTypCd;
                this.JobNum = LaborDtl.JobNum;
                this.AssemblySeq = LaborDtl.AssemblySeq;
                this.OprSeq = LaborDtl.OprSeq;
                this.IndirectCode = LaborDtl.IndirectCode;
                this.RoleCd = LaborDtl.RoleCd;
                this.ResourceGrpID = LaborDtl.ResourceGrpID;
                this.ResourceID = LaborDtl.ResourceID;
                this.ExpenseCode = LaborDtl.ExpenseCode;
                this.Shift = LaborDtl.Shift;
                this.TimeStatus = LaborDtl.TimeStatus;
                this.QuickEntryCode = LaborDtl.QuickEntryCode;
                this.LaborEntryMethod = LaborDtl.LaborEntryMethod;

                string sFormat = "";
                for (int ii = 0; ii < 20; ii++)
                {
                    sFormat = sFormat + "{" + ii.ToString() + "}~";
                }
                this.PRKey = String.Format(sFormat, this.Company, this.EmployeeNum, this.WeekBeginDate, this.WeekEndDate,
                    this.LaborTypePseudo, this.ProjectID, this.PhaseID, this.TimeTypCd, this.JobNum, this.AssemblySeq,
                    this.OprSeq, this.IndirectCode, this.RoleCd, this.ResourceGrpID, this.ResourceID, this.ExpenseCode,
                    this.Shift, this.TimeStatus, this.QuickEntryCode, this.LaborEntryMethod);
            }
        }

        Dictionary<string, TimeWeeklyViewRow> dicTimeWeeklyView;

        string lcEmployeeNum = string.Empty;
        DateTime? ldCalendarStartDate = null;
        DateTime? ldCalendarEndDate = null;
        DateTime? ldFromDate = null;
        DateTime? ldToDate = null;
        DateTime? ldRefreshFromDate = null;
        DateTime? ldRefreshToDate = null;
        string lcRefreshEmployeeNum = string.Empty;
        bool DelFlag = false;
        bool SNVerify = false;
        string DelCompany = string.Empty;
        string DelPartNum = string.Empty;
        string DelEmployeeNum = "zzzzzzzzzz";
        DateTime? DelWeekBeginDate = null;
        DateTime? DelWeekEndDate = null;
        string DelLaborType = string.Empty;
        string DelLaborTypePseudo = string.Empty;
        string DelProjectID = string.Empty;
        string DelPhaseID = string.Empty;
        string DelTimeTypCd = string.Empty;
        string DelJobNum = string.Empty;
        int DelAssemblySeq = 0;
        int DelOprSeq = 0;
        string DelIndirectCode = string.Empty;
        string DelRoleCd = string.Empty;
        string DelResourceGrpID = string.Empty;
        string DelResourceID = string.Empty;
        string DelExpenseCode = string.Empty;
        int DelShift = 0;
        string DelStatus = string.Empty;
        string DelQuickEntryCode = string.Empty;
        int tmpLaborHedSeq = 0;
        string vInvMethod = string.Empty;
        string vRevMethod = string.Empty;
        string eadErrMsg = string.Empty;
        bool vUpdatingJobParts = false;
        bool getBackflushRecords = false;

        /// <summary>
        /// ttJCSyst
        /// </summary>  
        public JCSyst ttJCSyst;
        /// <summary>
        /// ttJCSystRows
        /// </summary>  
        public List<JCSyst> ttJCSystRows;
        /* SCR 94780 - Get the ApplyBurdenCost option of CUR-PALNT */
        bool vApplySumBurdenRates = false;
        bool lDefDateFromNewHeader = false;

        // WARNDEF.i variables
        int VlabWarnNum = 0;
        int VVariancePct = 0;
        decimal VEstQtyPerHr = decimal.Zero;
        decimal VEstProdHrs = decimal.Zero;
        decimal VEarnedHrs = decimal.Zero;
        decimal VActualHrs = decimal.Zero;
        bool DispWarn = false;
        string FromProg = string.Empty;
        string VContinue = string.Empty;
        string VJobNum = string.Empty;
        int VAssemblySeq = 0;
        int VOprSeq = 0;
        string VWCCode = string.Empty;
        string VEmployeeNum = string.Empty;
        //bool VNoApply = false;
        //int i = 0;
        int j = 0;
        string RecipientList = string.Empty;
        bool VDo6070Alert = false;
        bool VDo6070Warn = false;

        /// <summary>
        /// LaborDtlParams
        /// </summary>  
        public struct LaborDtlParams
        {
            /// <summary>
            /// EmployeeNum
            /// </summary>  
            public string EmployeeNum;
            /// <summary>
            /// PayrollDate
            /// </summary>  
            public DateTime? PayrollDate;
            /// <summary>
            /// LaborType
            /// </summary>  
            public string LaborType;
            /// <summary>
            /// ProjectID
            /// </summary>  
            public string ProjectID;
            /// <summary>
            /// PhaseID
            /// </summary>  
            public string PhaseID;
            /// <summary>
            /// TimeTypCd
            /// </summary>  
            public string TimeTypCd;
            /// <summary>
            /// JobNum
            /// </summary>  
            public string JobNum;
            /// <summary>
            /// AssemblySeq
            /// </summary>  
            public int AssemblySeq;
            /// <summary>
            /// OprSeq
            /// </summary>  
            public int OprSeq;
            /// <summary>
            /// IndirectCode
            /// </summary>  
            public string IndirectCode;
            /// <summary>
            /// RoleCd
            /// </summary>  
            public string RoleCd;
            /// <summary>
            /// ResourceGrpID
            /// </summary>  
            public string ResourceGrpID;
            /// <summary>
            /// ResourceID
            /// </summary>  
            public string ResourceID;
            /// <summary>
            /// ExpenseCode
            /// </summary>  
            public string ExpenseCode;
            /// <summary>
            /// Shift
            /// </summary>  
            public int Shift;
            /// <summary>
            /// LaborDtlSeq
            /// </summary>  
            public int LaborDtlSeq;
            /// <summary>
            /// QuickEntryCode
            /// </summary>  
            public string QuickEntryCode;
            /// <summary>
            /// TimeStatus
            /// </summary>  
            public string TimeStatus;
            /// <summary>
            /// LaborTypePseudo
            /// </summary>  
            public string LaborTypePseudo;
            /// <summary>
            /// WeekBeginDate
            /// </summary>  
            public DateTime? WeekBeginDate;
            /// <summary>
            /// WeekEndDate
            /// </summary>  
            public DateTime? WeekEndDate;
            /// <summary>
            /// LaborHrs
            /// </summary>  
            public decimal LaborHrs;
        }

        #endregion Variables and Classes

        #region Implicit buffers
        Erp.Tables.JCSyst JCSyst;
        Erp.Tables.JCShift JCShift;
        Erp.Tables.JobHead JobHead;
        Erp.Tables.JobPart JobPart;
        Erp.Tables.JobOper JobOper;
        Erp.Tables.ResourceGroup ResourceGroup;
        Erp.Tables.Resource Resource;
        Erp.Tables.Capability Capability;
        Erp.Tables.Equip Equip;
        Erp.Tables.Indirect Indirect;
        Erp.Tables.EmpBasic EmpBasic;
        Erp.Tables.JobOpDtl JobOpDtl;
        Erp.Tables.ShopWrn ShopWrn;
        Erp.Tables.FirstArt FirstArt;
        Erp.Tables.JobAsmbl JobAsmbl;
        Erp.Tables.NonConf NonConf;
        Erp.Tables.MtlQueue MtlQueue;
        Erp.Tables.Reason Reason;
        Erp.Tables.OpMaster OpMaster;
        Erp.Tables.ResourceTimeUsed ResourceTimeUsed;
        Erp.Tables.ProjPhase ProjPhase;
        Erp.Tables.Project Project;
        Erp.Tables.CapResLnk CapResLnk;
        Erp.Tables.SNTran SNTran;
        Erp.Tables.EmpRole EmpRole;
        Erp.Tables.UserFile UserFile;
        Erp.Tables.PlantConfCtrl PlantConfCtrl;
        Erp.Tables.LabExpCd LabExpCd;
        Erp.Tables.QuickEntry QuickEntry;
        Erp.Tables.EmpCal EmpCal;
        Erp.Tables.ProdCal ProdCal;
        Erp.Tables.Plant Plant;
        Erp.Tables.Company Company;
        Erp.Tables.ResourceCal ResourceCal;
        Erp.Tables.ProdCalDay ProdCalDay;
        Erp.Tables.TimeTypCd TimeTypCd;
        Erp.Tables.PartTran PartTran;
        Erp.Tables.JCDept JCDept;
        Erp.Tables.FSCallhd FSCallhd;
        Erp.Tables.Task Task;
        Erp.Tables.HCMLaborDtlSync HCMLaborDtlSync;

        #endregion

        #region Lazy Loads
        private Lazy<Ice.Core.Getcodedesclist> libGCDL;
        private Ice.Core.Getcodedesclist LibGCDL { get { return this.libGCDL.Value; } }

        private Lazy<Erp.Internal.Lib.OffSet> libOffSet;
        private Erp.Internal.Lib.OffSet LibOffset { get { return this.libOffSet.Value; } }

        private Lazy<Erp.Internal.DE.ShiftBrkIncl> libShiftBrk;
        private Erp.Internal.DE.ShiftBrkIncl LibShiftBrk { get { return this.libShiftBrk.Value; } }

        private Lazy<Erp.Internal.DE.WarnDef> libWARNDEF;
        private Erp.Internal.DE.WarnDef LibWARNDEF { get { return this.libWARNDEF.Value; } }

        private Lazy<Erp.Internal.Lib.GetNextOprSeq> libGetNextOprSeq;
        private Erp.Internal.Lib.GetNextOprSeq LibGetNextOprSeq { get { return this.libGetNextOprSeq.Value; } }

        private Lazy<Ice.Lib.UsePatchFld> libUsePatchFld;
        private Ice.Lib.UsePatchFld LibUsePatchFld { get { return this.libUsePatchFld.Value; } }

        private Lazy<Erp.Internal.Lib.SerialCommon> libSerialCommon;
        private Erp.Internal.Lib.SerialCommon LibSerialCommon { get { return this.libSerialCommon.Value; } }

        private Lazy<Erp.Internal.PJ.ProjectCommon> libProjectCommon;
        private Erp.Internal.PJ.ProjectCommon LibProjectCommon { get { return this.libProjectCommon.Value; } }

        private Lazy<Erp.Internal.Lib.EADValidation> libEADValidation;
        private Erp.Internal.Lib.EADValidation LibEADValidation { get { return this.libEADValidation.Value; } }

        private Lazy<Erp.Internal.IM.PartTranSNtranLink> libPartTranSNtranLink;
        private Erp.Internal.IM.PartTranSNtranLink LibPartTranSNtranLink { get { return this.libPartTranSNtranLink.Value; } }

        private Lazy<Erp.Internal.Lib.PWIPMtlQ> libPWIPMtlQ;
        private Erp.Internal.Lib.PWIPMtlQ LibPWIPMtlQ { get { return this.libPWIPMtlQ.Value; } }

        private Lazy<Erp.Internal.Lib.GetNewSNtran> libGetNewSNtran;
        private Erp.Internal.Lib.GetNewSNtran LibGetNewSNtran { get { return this.libGetNewSNtran.Value; } }

        private Lazy<Erp.Internal.Lib.RoundAmountEF> libRoundAmountEF;
        private Erp.Internal.Lib.RoundAmountEF LibRoundAmountEF { get { return this.libRoundAmountEF.Value; } }

        private Lazy<Erp.Internal.Lib.AppService> libAppService;
        private Erp.Internal.Lib.AppService LibAppService { get { return this.libAppService.Value; } }

        private Lazy<Erp.Internal.Lib.TEApproverLists> libTEApproverLists;
        private Erp.Internal.Lib.TEApproverLists LibTEApproverLists { get { return this.libTEApproverLists.Value; } }

        private Lazy<Erp.Internal.Lib.TimeExpenseSubmit> libTimeExpenseSubmit;
        private Erp.Internal.Lib.TimeExpenseSubmit LibTimeExpenseSubmit { get { return this.libTimeExpenseSubmit.Value; } }

        private Lazy<Erp.Internal.Lib.XAP05> libXAP05;
        private Erp.Internal.Lib.XAP05 LibXAP05 { get { return this.libXAP05.Value; } }

        private Lazy<Erp.Internal.Lib.CanApproveTE> libCanApproveTE;
        private Erp.Internal.Lib.CanApproveTE LibCanApproveTE { get { return this.libCanApproveTE.Value; } }

        private Lazy<Ice.Lib.NextValue> libNextValue;
        private Ice.Lib.NextValue LibNextValue { get { return this.libNextValue.Value; } }

        private Lazy<Erp.Internal.Lib.LaborRate> libLaborRate;
        private Erp.Internal.Lib.LaborRate LibLaborRate { get { return this.libLaborRate.Value; } }

        private Lazy<Erp.Internal.Lib.BillableServiceRate> libBillableServiceRate;
        private Erp.Internal.Lib.BillableServiceRate LibBillableServiceRate { get { return this.libBillableServiceRate.Value; } }

        private Lazy<Erp.Internal.PE.PELock> peLock;
        private Erp.Internal.PE.PELock PELock
        {
            get { return this.peLock.Value; }
        }
        #endregion

        /// <summary>
        /// Initialize components
        /// </summary>
        protected override void Initialize()
        {
            this.libBillableServiceRate = new Lazy<Internal.Lib.BillableServiceRate>(() => new Internal.Lib.BillableServiceRate(this.Db));
            ErpCallContext.Properties["Labor"] = "";

            this.libGCDL = new Lazy<Ice.Core.Getcodedesclist>(() => new Ice.Core.Getcodedesclist(Db));
            this.libOffSet = new Lazy<Erp.Internal.Lib.OffSet>(() => new Erp.Internal.Lib.OffSet(Db));
            this.libShiftBrk = new Lazy<Erp.Internal.DE.ShiftBrkIncl>(() => new Erp.Internal.DE.ShiftBrkIncl(Db));
            this.libWARNDEF = new Lazy<Erp.Internal.DE.WarnDef>(() => new Erp.Internal.DE.WarnDef(Db));
            this.libGetNextOprSeq = new Lazy<Erp.Internal.Lib.GetNextOprSeq>(() => new Erp.Internal.Lib.GetNextOprSeq(Db));
            this.libUsePatchFld = new Lazy<Ice.Lib.UsePatchFld>(() => new Ice.Lib.UsePatchFld(Db));
            this.libSerialCommon = new Lazy<Erp.Internal.Lib.SerialCommon>(() => new Erp.Internal.Lib.SerialCommon(Db));
            this.libProjectCommon = new Lazy<Erp.Internal.PJ.ProjectCommon>(() => new Erp.Internal.PJ.ProjectCommon(Db));
            this.libEADValidation = new Lazy<EADValidation>(() => new Erp.Internal.Lib.EADValidation(Db));
            this.libPartTranSNtranLink = new Lazy<Erp.Internal.IM.PartTranSNtranLink>(() => new Erp.Internal.IM.PartTranSNtranLink(Db));
            this.libPWIPMtlQ = new Lazy<Erp.Internal.Lib.PWIPMtlQ>(() => new Erp.Internal.Lib.PWIPMtlQ(Db));
            this.libGetNewSNtran = new Lazy<Erp.Internal.Lib.GetNewSNtran>(() => new Erp.Internal.Lib.GetNewSNtran(Db));
            this.libRoundAmountEF = new Lazy<Erp.Internal.Lib.RoundAmountEF>(() => new Erp.Internal.Lib.RoundAmountEF(Db));
            this.libAppService = new Lazy<Erp.Internal.Lib.AppService>(() => new Erp.Internal.Lib.AppService(Db));
            this.libTEApproverLists = new Lazy<Erp.Internal.Lib.TEApproverLists>(() => new Erp.Internal.Lib.TEApproverLists(this.Db));
            this.libTimeExpenseSubmit = new Lazy<Erp.Internal.Lib.TimeExpenseSubmit>(() => new Erp.Internal.Lib.TimeExpenseSubmit(Db));
            this.libXAP05 = new Lazy<Erp.Internal.Lib.XAP05>(() => new Erp.Internal.Lib.XAP05(Db));
            this.libCanApproveTE = new Lazy<Erp.Internal.Lib.CanApproveTE>(() => new Erp.Internal.Lib.CanApproveTE(Db));
            this.libNextValue = new Lazy<Ice.Lib.NextValue>(() => new Ice.Lib.NextValue(Db));
            this.libLaborRate = new Lazy<Erp.Internal.Lib.LaborRate>(() => new Erp.Internal.Lib.LaborRate(Db));
            peLock = new Lazy<Internal.PE.PELock>(() => new Erp.Internal.PE.PELock(Db));
            /* Check security */
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.JobManagement) && !Session.ModuleLicensed(Erp.License.ErpLicensableModules.ExpenseManagement) && !Session.ModuleLicensed(Erp.License.ErpLicensableModules.TimeManagement))
                throw new Exception(Strings.TheModulesRequiToAccessBOLaborAreNotLicen);

            modList = IceRow.ROWSTATE_ADDED + "," + IceRow.ROWSTATE_UPDATED;
            PlantConfCtrl = this.FindFirstPlantConfCtrl(Session.CompanyID, Session.PlantID);
            if (PlantConfCtrl != null)
                vApplySumBurdenRates = PlantConfCtrl.ApplyBurdenCost;

            base.Initialize();

            ttJCSyst = new Tables.JCSyst();
            ttJCSystRows = new List<Tables.JCSyst>();
            ttJCSystRows.Add(ttJCSyst);


            JCSyst = this.FindFirstJCSyst(Session.CompanyID);
            if (JCSyst != null)
            {
                BufferCopy.Copy(JCSyst, ref ttJCSyst);
            }
            else
            {
                ttJCSyst.DetailGrace = false;
                ttJCSyst.GenLaborWarnMsg = false;
                ttJCSyst.PreventFABypass = false;
                ttJCSyst.ChkEmpPrjRole = false;
                ttJCSyst.DfltPrjRoleLoc = "";
                ttJCSyst.MachinePrompt = false;
                ttJCSyst.ScrapReasons = false;
                ttJCSyst.ReworkReasons = false;
                ttJCSyst.FeedPayroll = false;
                ttJCSyst.EarlyClockInAllowance = 0;
                ttJCSyst.LateClockInAllowance = 0;
                ttJCSyst.EarlyClockOutAllowance = 0;
                ttJCSyst.LateClockOutAllowance = 0;
                ttJCSyst.ClockFormat = "M";
            }
        }

        private void adjustForGracePeriod(DateTime? SaveSysDate, int SaveSysTime, ref int CurrTime)
        {
            int BaseClockInMin = 0;
            int BaseNewClockOutMin = 0;
            DateTime? NewClockOutDate = null;
            int V_FromTime = 0;
            int V_ToTime = 0;
            int AdjTime = 0;
            DateTime? AdjDate = null;
            DateTime? EndDate = null;
            DateTime? NewSysDate = null;
            CurrTime = SaveSysTime;
            /* The purpose of the next section of code is to determine the clock out
               time of the actual clock out time when adjusted by the grace period. */
            if (CurrTime == 0)
            {
                CurrTime = 24 * 60 * 60;
                NewSysDate = SaveSysDate.Value.AddDays(-1);
            }
            else
            {
                NewSysDate = SaveSysDate;
            }
            /* If the shift ends at 24.00 hours, the date has to adjusted according to whether if the
               clock out date is equal to or greater than the clock in date.  It is necessary to get the
               clock in date right in order to calculate the grace period limits correctly */
            if (JCShift.EndTime != 24.00m)
            {
                if (LaborHed.ClockInDate != null && NewSysDate != null)
                {
                    if (LaborHed.ClockInDate.Value.Date < NewSysDate.Value.Date)
                        AdjDate = LaborHed.ClockInDate.Value.AddDays(1);
                    else AdjDate = LaborHed.ClockInDate;
                }
                else
                {
                    AdjDate = LaborHed.ClockInDate;
                }
            }
            else
            {
                if (LaborHed.ClockInDate != null && NewSysDate != null)
                {
                    if (LaborHed.ClockInDate.Value.Date == NewSysDate.Value.Date)
                        AdjDate = NewSysDate.Value.AddDays(-1);
                    else AdjDate = NewSysDate;
                }
                else
                {
                    AdjDate = NewSysDate;
                }
            }
            V_FromTime = (((TimeSpan)(AdjDate - DAYZERO)).Days * 24 * 60) - (ttJCSyst.EarlyClockOutAllowance) + Compatibility.Convert.ToInt32(JCShift.EndTime * 60);
            V_ToTime = (((TimeSpan)(AdjDate - DAYZERO)).Days * 24 * 60) + (ttJCSyst.LateClockOutAllowance) + Compatibility.Convert.ToInt32(JCShift.EndTime * 60);
            if (JCShift.EndTime != 24.00m)
            {
                AdjTime = (((TimeSpan)(AdjDate - DAYZERO)).Days * 24 * 60) + CurrTime / 60;
            }
            else
            {
                if (AdjDate.Value.Date < NewSysDate.Value.Date)
                {
                    AdjTime = (((TimeSpan)(AdjDate - DAYZERO)).Days * 24 * 60) + CurrTime / 60;
                }
                else
                {
                    AdjTime = (((TimeSpan)(AdjDate - DAYZERO)).Days * 24 * 60) + Compatibility.Convert.ToInt32(CurrTime / 60m) + Compatibility.Convert.ToInt32(JCShift.EndTime * 60);
                }
            }
            if (AdjTime < V_FromTime || AdjTime > V_ToTime)
            {
                NewClockOutDate = NewSysDate;
                EndDate = NewSysDate;
            }
            else
            {
                if (((decimal)CurrTime / 60M) < JCShift.EndTime * 60)
                {/* if JCSyst.DetailGrace */
                    if (ttJCSyst.DetailGrace)
                    {
                        ttLaborDtl.ClockOutTime = JCShift.EndTime; /* Hours.Hundreths */

                        if (JCShift.EndTime == 24)
                        {
                            EndDate = LaborHed.ClockInDate;
                        }
                        else
                        {
                            EndDate = NewSysDate;
                        }
                    }
                    else
                    {
                        NewClockOutDate = NewSysDate;
                        EndDate = NewSysDate;
                    }
                }
                else
                {
                    ttLaborDtl.ClockOutTime = JCShift.EndTime; /* Hours.Hundreths */
                    if (JCShift.EndTime == 24)
                    {
                        EndDate = LaborHed.ClockInDate;
                    }
                    else
                    {
                        EndDate = NewSysDate;
                    }
                }
            }
            /* Convert in/out to Date/Minute format for comparisons. */
            BaseClockInMin = (((TimeSpan)(ttLaborDtl.ClockInDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(ttLaborDtl.ClockinTime * 60);
            /* SCR #20669 - make sure that if clock out time is exactly midnight then use 24:00 (1440 mins). *
             * Use CurrTime to check for exact clock out time up to the last second. In all cases, including *
             * the scenario where CurrTime is just a few seconds after midnight (less than a minute), use the*
             * LaborDtl.ClockOutTime since EndDate is the true end date unlike if the CurrTime is originally *
             * zero (adjusted to 86400) the EndDate is already adjusted (SaveSysDate - 1).                   */
            if (CurrTime == 86400)
            {
                BaseNewClockOutMin = (((TimeSpan)(EndDate - DAYZERO)).Days * 1440) + 1440;
            }
            else
            {
                BaseNewClockOutMin = (((TimeSpan)(EndDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(ttLaborDtl.ClockOutTime * 60);
            }
            /* if clockout minute is less than clock in minute... this
               could happen if clock in time was rolled forward because of
               grace period and then the employee clocked out right away
               before the shift start time.... then force clock out minute
               to equal clock in minute. */
            if (BaseNewClockOutMin < BaseClockInMin)
            {
                BaseNewClockOutMin = BaseClockInMin;
                ttLaborDtl.ClockOutTime = ttLaborDtl.ClockinTime;
            }
            ttLaborDtl.ClockOutMinute = BaseNewClockOutMin;

            /* format time for display */
            if (ttJCSyst.ClockFormat.Compare("M") == 0)
            {
                ttLaborDtl.DspClockOutTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborDtl.ClockOutTime), "99") + ":" + Compatibility.Convert.ToString(((ttLaborDtl.ClockOutTime - Math.Truncate(ttLaborDtl.ClockOutTime)) * 60), "99");
            }
            else
            {
                ttLaborDtl.DspClockOutTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborDtl.ClockOutTime), "99") + Compatibility.Convert.ToString((ttLaborDtl.ClockOutTime - Math.Truncate(ttLaborDtl.ClockOutTime)), ".99");
            }
        }

        partial void AfterGetRows()
        {
            using (ErpCallContext.SetDisposableKey("LaborAfterGetRows"))
            {
                hshJobAfterGetRows = new Hashtable();
                foreach (var ttLaborDtlRow in (CurrentFullTableset.LaborDtl).ToList())
                {
                    ttLaborDtl = ttLaborDtlRow;
                    if (ttLaborDtl.HasAccessToRow == false)
                    {
                        CurrentFullTableset.LaborDtlComment.RemoveAll(ttLaborDtlComment_Row => ttLaborDtlComment_Row.Company.Compare(ttLaborDtl.Company) == 0
                        && ttLaborDtlComment_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                        && ttLaborDtlComment_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq);

                        CurrentFullTableset.LaborEquip.RemoveAll(ttLaborEquip_Row => ttLaborEquip_Row.Company.Compare(ttLaborDtl.Company) == 0
                        && ttLaborEquip_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                        && ttLaborEquip_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq);

                        CurrentFullTableset.LaborPart.RemoveAll(ttLaborPart_Row => ttLaborPart_Row.Company.Compare(ttLaborDtl.Company) == 0
                        && ttLaborPart_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                        && ttLaborPart_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq);

                        CurrentFullTableset.LbrScrapSerialNumbers.RemoveAll(ttLbrScrapSerialNumbers_Row => ttLbrScrapSerialNumbers_Row.Company.Compare(ttLaborDtl.Company) == 0
                        && ttLbrScrapSerialNumbers_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                        && ttLbrScrapSerialNumbers_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq);
                        CurrentFullTableset.LaborDtl.Remove(ttLaborDtl);
                    }
                    else
                    {
                        this.refreshLaborPart(ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);
                    }
                }
            }
        }

        partial void AfterUpdate()
        {
            decimal origQty = decimal.Zero;
            string vMainPart = string.Empty;

            if (bDelttLaborDtlRows != null)
            {
                foreach (var bDelttLaborDtl in bDelttLaborDtlRows)
                {
                    ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
                    CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
                    BufferCopy.Copy(bDelttLaborDtl, ref ttLaborDtl);
                    ttLaborDtl.RowMod = IceRow.ROWSTATE_UNCHANGED;
                    ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
                    CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
                    BufferCopy.Copy(bDelttLaborDtl, ref ttLaborDtl);
                    ttLaborDtl.RowMod = IceRow.ROWSTATE_DELETED;
                }
            }
            /* If a ttLaborHed was deleted in beforeUpdate, add it back with
               a row state of deleted so the UI no longer has it. */

            if (bDelttLaborHedRows != null)
            {
                foreach (var bDelttLaborHed in bDelttLaborHedRows)
                {
                    if (bDelttLaborHed != null)
                    {
                        ttLaborHed = new Erp.Tablesets.LaborHedRow();
                        CurrentFullTableset.LaborHed.Add(ttLaborHed);
                        BufferCopy.Copy(bDelttLaborHed, ref ttLaborHed);
                        ttLaborHed.RowMod = IceRow.ROWSTATE_UNCHANGED;
                        ttLaborHed = new Erp.Tablesets.LaborHedRow();
                        CurrentFullTableset.LaborHed.Add(ttLaborHed);
                        BufferCopy.Copy(bDelttLaborHed, ref ttLaborHed);
                        ttLaborHed.RowMod = IceRow.ROWSTATE_DELETED;
                    }
                }
            }
            /* refresh TimeWeeklyView and TimeWorkHours */
            lcEmployeeNum = lcRefreshEmployeeNum;
            ldCalendarStartDate = ldRefreshFromDate;
            ldCalendarEndDate = ldRefreshToDate;/* correct doubling of hours error */
            CurrentFullTableset.TimeWeeklyView.Clear();
            this.populateTimeValidateDates();
            this.populateTimeWorkHours();
            this.populateTimeWeeklyView();

            if (delttTimeWeeklyViewRows != null)
            {
                foreach (var delttTimeWeeklyView in delttTimeWeeklyViewRows)
                {

                    ttTimeWeeklyView = (from ttTimeWeeklyView_Row in CurrentFullTableset.TimeWeeklyView
                                        where ttTimeWeeklyView_Row.Company.KeyEquals(Session.CompanyID)
                                        && ttTimeWeeklyView_Row.EmployeeNum.KeyEquals(delttTimeWeeklyView.EmployeeNum)
                                        && ttTimeWeeklyView_Row.WeekBeginDate.Value.Date == delttTimeWeeklyView.WeekBeginDate.Value.Date
                                        && ttTimeWeeklyView_Row.WeekEndDate.Value.Date == delttTimeWeeklyView.WeekEndDate.Value.Date
                                        && ttTimeWeeklyView_Row.LaborTypePseudo.KeyEquals(delttTimeWeeklyView.LaborTypePseudo)
                                        && ttTimeWeeklyView_Row.ProjectID.KeyEquals(delttTimeWeeklyView.ProjectID)
                                        && ttTimeWeeklyView_Row.PhaseID.KeyEquals(delttTimeWeeklyView.PhaseID)
                                        && ttTimeWeeklyView_Row.TimeTypCd.KeyEquals(delttTimeWeeklyView.TimeTypCd)
                                        && ttTimeWeeklyView_Row.JobNum.KeyEquals(delttTimeWeeklyView.JobNum)
                                        && ttTimeWeeklyView_Row.AssemblySeq == delttTimeWeeklyView.AssemblySeq
                                        && ttTimeWeeklyView_Row.OprSeq == delttTimeWeeklyView.OprSeq
                                        && ttTimeWeeklyView_Row.IndirectCode.KeyEquals(delttTimeWeeklyView.IndirectCode)
                                        && ttTimeWeeklyView_Row.RoleCd.KeyEquals(delttTimeWeeklyView.RoleCd)
                                        && ttTimeWeeklyView_Row.ResourceGrpID.KeyEquals(delttTimeWeeklyView.ResourceGrpID)
                                        && ttTimeWeeklyView_Row.ResourceID.KeyEquals(delttTimeWeeklyView.ResourceID)
                                        && ttTimeWeeklyView_Row.ExpenseCode.KeyEquals(delttTimeWeeklyView.ExpenseCode)
                                        && ttTimeWeeklyView_Row.Shift == delttTimeWeeklyView.Shift
                                        && ttTimeWeeklyView_Row.TimeStatus.KeyEquals(delttTimeWeeklyView.TimeStatus)
                                        && ttTimeWeeklyView_Row.QuickEntryCode.KeyEquals(delttTimeWeeklyView.QuickEntryCode)
                                        select ttTimeWeeklyView_Row).FirstOrDefault();
                    if (ttTimeWeeklyView == null)
                    {
                        ttTimeWeeklyView = new Erp.Tablesets.TimeWeeklyViewRow();
                        CurrentFullTableset.TimeWeeklyView.Add(ttTimeWeeklyView);
                        BufferCopy.Copy(delttTimeWeeklyView, ref ttTimeWeeklyView);
                    }
                }
            }
            bool mainJobPart = false;
            /* DID WE UPDATE A LABORDTL OR LABORPART - NEED TO KNOW TO SO THAT WE KEEP THE LaborPart TABLE COMPLETE */
            /* Check for the presence of both ttLaborDtl and ttLaborPart.  If they both exist,
               this is a multi-record update called from End Activity in MES.  */
            /* SCR 92148 - if called from End Activity from MES but with no update in the ttLaborPart *
             * because qtys entered from the ReportQty are not changing then we still need to process *
             * the existing LaborParts in the logic below so do not do a return at this point.        */

            if ((((from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                   select ttLaborDtl_Row).Any()) &&
               !(((from ttLaborPart_Row in CurrentFullTableset.LaborPart
                   select ttLaborPart_Row).Any()))) ||
                runEndActJobPartUpd == true)
            {


                foreach (var ttLaborDtl in CurrentFullTableset.LaborDtl)
                {
                    this.refreshLaborPart(ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);
                }
                if (runEndActJobPartUpd == false)
                {
                    return;
                }
            }

            ttLaborPart = (from ttLaborPart_Row in CurrentFullTableset.LaborPart
                           select ttLaborPart_Row).FirstOrDefault();
            if (ttLaborPart != null)
            {
                decimal sumQty = decimal.Zero;
                decimal sumScrapQty = decimal.Zero;
                decimal sumDiscrepQty = decimal.Zero;
                decimal diffPartQty = decimal.Zero;
                decimal diffScrapQty = decimal.Zero;
                decimal diffDiscrepQty = decimal.Zero;
                decimal vPartQty = decimal.Zero;
                sumQty = 0;
                diffPartQty = 0;
                vPartQty = 0;


                LaborDtl = this.FindFirstLaborDtl(Session.CompanyID, ttLaborPart.LaborHedSeq, ttLaborPart.LaborDtlSeq);
                if (LaborDtl != null)
                {


                    JobHead = this.FindFirstJobHead(LaborDtl.Company, LaborDtl.JobNum);
                    mainJobPart = ((JobHead != null && JobHead.ProcessMode.Compare("C") == 0) ? true : false);
                    vMainPart = (mainJobPart == true) ? JobHead.PartNum : string.Empty;

                    /* Only go through the tmpLaborPart that have been updated to sum up the total PartQty  *
                     * to be added to the LaborDtl.LaborQty. Do not update the JobProd during this loop. We *
                     * need to update the LaborDtl first with the final LaborQty before we update JobPart.  *
                     * We need to delay the update of JobPart because we want that before JobPart performs  *
                     * Auto-Receipt, we already have the final qty so job costs can be calculated correctly.*/

                    if (tmpLaborPartRows != null)
                    {
                        foreach (var tmpLaborPart in (from tmpLaborPart_Row in tmpLaborPartRows
                                                      where tmpLaborPart_Row.Company.KeyEquals(LaborDtl.Company) &&
                                                      tmpLaborPart_Row.LaborHedSeq == LaborDtl.LaborHedSeq &&
                                                      tmpLaborPart_Row.LaborDtlSeq == LaborDtl.LaborDtlSeq &&
                                                      (tmpLaborPart_Row.OldPartQty != tmpLaborPart_Row.PartQty ||
                                                      tmpLaborPart_Row.OldScrapQty != tmpLaborPart_Row.ScrapQty ||
                                                      tmpLaborPart_Row.OldDiscrepQty != tmpLaborPart_Row.DiscrepQty)
                                                      select tmpLaborPart_Row).ToList())
                        {
                            diffPartQty = tmpLaborPart.PartQty - tmpLaborPart.OldPartQty;
                            diffScrapQty = tmpLaborPart.ScrapQty - tmpLaborPart.OldScrapQty;
                            diffDiscrepQty = tmpLaborPart.DiscrepQty - tmpLaborPart.OldDiscrepQty;
                            if (diffPartQty != 0 || diffScrapQty != 0 || diffDiscrepQty != 0)
                            {
                                JobPart = this.FindFirstJobPart(tmpLaborPart.Company, LaborDtl.JobNum, tmpLaborPart.PartNum);
                                if (JobPart != null && JobHead != null)
                                {
                                    LibAppService.UOMConv(JobPart.PartNum, diffPartQty, JobPart.IUM, JobHead.IUM, out diffPartQty, false);
                                }
                                /* if the job is Concurrent then ONLY the PartQty reported by the Main Part is reflected in the LaborDtl */
                                if (mainJobPart == true && JobHead.PartNum.KeyEquals(tmpLaborPart.PartNum))
                                {
                                    if (JobPart != null && JobPart.PartsPerOp != 0)
                                    {
                                        sumQty = diffPartQty / JobPart.PartsPerOp;
                                        sumScrapQty = diffScrapQty / JobPart.PartsPerOp;
                                        sumDiscrepQty = diffDiscrepQty / JobPart.PartsPerOp;
                                    }
                                    else
                                    {
                                        sumQty = diffPartQty;
                                        sumScrapQty = diffScrapQty;
                                        sumDiscrepQty = diffDiscrepQty;
                                    }
                                }
                                else if (mainJobPart == false)
                                {
                                    /* sum up the total changes in the laborpart quantities so we can add it to the LaborDtl all at once */
                                    sumQty = sumQty + diffPartQty;
                                    sumScrapQty = sumScrapQty + diffScrapQty;
                                    sumDiscrepQty = sumDiscrepQty + diffDiscrepQty;
                                }
                            }
                            else
                            {
                                tmpLaborPartRows.Remove(tmpLaborPart);
                            }
                        } /* for each tmpLaborPart */
                    }
                    /* if there is a change in the total LaborQty then update the LaborDtl */
                    /* SCR 137996 - if sumQty = 0, check if concurrent job and previous reported qty <> 0 */
                    /* SCR 185762 - We're now doing net change even for Concurrent MainPart, so only do the logic below if with sumQty <> 0 */
                    if (sumQty != 0 || sumScrapQty != 0 || sumDiscrepQty != 0)
                    {

                        LaborDtl = this.FindFirstLaborDtlWithUpdLock(ttLaborPart.Company, ttLaborPart.LaborHedSeq, ttLaborPart.LaborDtlSeq);
                        if (LaborDtl != null)
                        {
                            /* SCR 185762 - Make sure that the ErpCallContext for "FromEndActivity" is not used for this LaborDtl update */
                            if (ErpCallContext.ContainsKey("FromEndActivity"))
                            {
                                ErpCallContext.RemoveValue("FromEndActivity"); // remove key added in LaborDtlBeforeUpdate
                            }

                            oldLbrQty = LaborDtl.LaborQty;
                            oldScrapQty = LaborDtl.ScrapQty;
                            oldDiscrepQty = LaborDtl.DiscrepQty;
                            LaborDtl.LaborQty = LaborDtl.LaborQty + sumQty;
                            LaborDtl.ScrapQty = LaborDtl.ScrapQty + sumScrapQty;
                            LaborDtl.DiscrepQty = LaborDtl.DiscrepQty + sumDiscrepQty;

                            if ("P,S".Lookup(LaborDtl.LaborType) > -1)
                            {
                                JobOper = this.FindFirstJobOper(Session.CompanyID, LaborDtl.JobNum, LaborDtl.AssemblySeq, LaborDtl.OprSeq);
                                if (JobOper != null)
                                {
                                    this.getBeforeInfo();
                                    if (oldJobNum.Compare(JobOper.JobNum) == 0 &&
                                        oldAssSeq == JobOper.AssemblySeq &&
                                        oldOprSeq == JobOper.OprSeq)
                                    {
                                        origQty = JobOper.QtyCompleted - oldLbrQty;
                                    }
                                    else
                                    {
                                        origQty = JobOper.QtyCompleted;
                                    }
                                    if (origQty < 0)
                                    {
                                        origQty = 0;
                                    }
                                    this.setComplete(LaborDtl.JobNum, LaborDtl.AssemblySeq, LaborDtl.OprSeq, LaborDtl.LaborQty, LaborDtl.ScrapQty);
                                }
                            }
                            Db.Validate(LaborDtl);
                        }
                    } /* sumQty <> 0 */

                    /* We need to delay the update of JobParts because we want that before JobPart performs *
                     * Auto-Receipt, we already have the final qty so job costs can be calculated correctly.*
                     * If any of the LaborPart's PartQty is changed then we have to back out all LaborParts *
                     * OldPartQty to generate the reversing Auto-Receipt transaction if needed. Then create *
                     * new Auto-Receipt transaction using the new PartQty as necessary.  The auto receiving *
                     * logic is in the JobPart write trigger which will happen when we update QtyCompleted. */

                    /* SCR 185762 - Implemented new logic for updating JobPart Quantity for each LaborPart. *
                     * NOTE: MES ReportQty updates the JobOper.QtyCompleted even if the started activity is *
                     * still active. The job quantities reflect the partial reported quantities without the *
                     * labor costs (SCR 124297). So, the original assumption that ReportQty does not update *
                     * job quantities no longer applies. The logic for CoPart JobPart update is now simpler.*
                     * We now only need to account for the net change in LaborPart PartQty. No need to back *
                     * out old PartQty and add the new one.                                                 */
                    if (tmpLaborPartRows != null)
                    {
                        /* SCR 161845 - Make sure that when receiving LaborParts for a Concurrent Job, the MainPart is   *
                         * received LAST. This is to ensure that all received LaborParts will have the correct prorated  *
                         * job costs. Concurrent job costing only reports completed qty from the MainPart, so if it is   *
                         * received first then COSWIP logic will recognized that all completed qty has been received and *
                         * no remaining costs to be relieved.                                                            */
                        /* SCR 161845 - if vMainPart is blank then SelectLaborParts will process all LaborParts.         *
                         * If not blank, SelectLaborParts will only process LaborParts <> vMainPart.                     */
                        foreach (var tmpLaborPart in (from tmpLaborPart_Row in tmpLaborPartRows
                                                      where tmpLaborPart_Row.Company.KeyEquals(LaborDtl.Company) &&
                                                      tmpLaborPart_Row.LaborHedSeq == LaborDtl.LaborHedSeq &&
                                                      tmpLaborPart_Row.LaborDtlSeq == LaborDtl.LaborDtlSeq &&
                                                      tmpLaborPart_Row.OldPartQty != tmpLaborPart_Row.PartQty &&
                                                      !tmpLaborPart_Row.PartNum.KeyEquals(vMainPart)
                                                      select tmpLaborPart_Row).ToList())
                        {
                            LaborPart = this.FindFirstLaborPart(LaborDtl.Company, tmpLaborPart.PartNum, LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq);
                            if (LaborPart == null) continue;

                            vPartQty = tmpLaborPart.PartQty - tmpLaborPart.OldPartQty;
                            if (vPartQty != 0)
                            {
                                JobPart = this.FindFirstJobPartWithUpdLock(LaborDtl.Company, LaborDtl.JobNum, tmpLaborPart.PartNum);
                                if (JobPart != null)
                                {
                                    JobPart.QtyCompleted = JobPart.QtyCompleted + vPartQty;
                                    Db.Validate(JobPart);
                                }
                            }
                        }
                        /* SCR 161845 - if vMainPart is not blank then receive the MainPart LAST */
                        if (!string.IsNullOrEmpty(vMainPart))
                        {
                            LaborPart = this.FindFirstLaborPart(LaborDtl.Company, vMainPart, LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq);
                            if (LaborPart != null)
                            {
                                tmpLaborPart = (from tmpLaborPart_Row in tmpLaborPartRows
                                                where tmpLaborPart_Row.Company.KeyEquals(LaborPart.Company) &&
                                                      tmpLaborPart_Row.LaborHedSeq == LaborPart.LaborHedSeq &&
                                                      tmpLaborPart_Row.LaborDtlSeq == LaborPart.LaborDtlSeq &&
                                                      tmpLaborPart_Row.PartNum.KeyEquals(LaborPart.PartNum)
                                                select tmpLaborPart_Row).FirstOrDefault();
                                if (tmpLaborPart != null)
                                {
                                    vPartQty = tmpLaborPart.PartQty - tmpLaborPart.OldPartQty;
                                    if (vPartQty != 0)
                                    {
                                        JobPart = this.FindFirstJobPartWithUpdLock(LaborPart.Company, LaborDtl.JobNum, LaborPart.PartNum);
                                        if (JobPart != null)
                                        {
                                            JobPart.QtyCompleted = JobPart.QtyCompleted + vPartQty;
                                            Db.Validate(JobPart);
                                        }
                                    }
                                }
                            }
                        }  /* if vMainPart <> blank */
                    }
                } /* avail LaborDtl */
                this.refreshLaborPart(ttLaborPart.LaborHedSeq, ttLaborPart.LaborDtlSeq);
                this.refreshTtLaborDtl(ttLaborPart.LaborHedSeq, ttLaborPart.LaborDtlSeq);

                if (tmpLaborPartRows != null)
                    tmpLaborPartRows.Clear();
                return;
            }
            /* OK MAYBE WE JUST DELETED A JOB PART... NEED TO REBUILD IT */
            if (del_LaborPart_LaborHedSeq > 0)
            {
                this.refreshLaborPart(del_LaborPart_LaborHedSeq, del_LaborPart_LaborDtlSeq);
            }
        }

        partial void BeforeUpdate()
        {
            LaborTableset LaborDS = CurrentFullTableset;
            Erp.Tablesets.LaborDtlRow bttLaborDtl = null;


            foreach (var ttLaborPart_iterator in (from ttLaborPart_Row in CurrentFullTableset.LaborPart
                                                  where ttLaborPart_Row.RowMod.Compare(IceRow.ROWSTATE_UNCHANGED) != 0
                                                  select ttLaborPart_Row).ToList())
            {
                ttLaborPart = ttLaborPart_iterator;
                if (ttLaborPart.PartQty == 0 && ttLaborPart.ScrapQty == 0 && ttLaborPart.DiscrepQty == 0)
                {
                    if (ttLaborPart.SysRowID == Guid.Empty)
                    {
                        CurrentFullTableset.LaborPart.Remove(ttLaborPart);
                    }
                }
                Db.Validate();
            }


            foreach (var bttLaborDtl_iterator in (from bttLaborDtl_Row in CurrentFullTableset.LaborDtl
                                                  where !String.IsNullOrEmpty(bttLaborDtl_Row.RowMod)
                                                  select bttLaborDtl_Row))
            {
                bttLaborDtl = bttLaborDtl_iterator;
                bttLaborDtl.LaborType = (("P,J,V".Lookup(bttLaborDtl.LaborTypePseudo) > -1) ? "P" : ((bttLaborDtl.LaborTypePseudo.Compare("I") == 0) ? "I" : ((bttLaborDtl.LaborTypePseudo.Compare("S") == 0) ? "S" : "")));
            }
            /* If both a labor head and labor detail can be found with an add state,
               they were added via method GetLaborDtlNoHdr.  This signifies a special
               condition for time entry.  The ttLaborHed record will be deleted if an
               existing LaborHed record with a payroll date and employee as the
               ttLaborDtl can be found.  The ttLaborDtl will be linked to that existing
               LaborHed instead of the new one.  The added ttLaborHed is temporarily
               put in a temp-table and removed from ttLaborHed so standard update logic
               doesn't process it.  It is inserted back into the ttLaborHed table with
               a deleted status so it will be removed from the dataset when the
               records are returned. */



            if (((from ttLaborHed_Row in CurrentFullTableset.LaborHed
                  where ttLaborHed_Row.RowMod.Compare(IceRow.ROWSTATE_ADDED) == 0
                  select ttLaborHed_Row).Any()) && ((from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                                                     where ttLaborDtl_Row.RowMod.Compare(IceRow.ROWSTATE_ADDED) == 0
                                                     select ttLaborDtl_Row).Any()))
            {


                ttLaborDtl = (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                              where ttLaborDtl_Row.RowMod.Compare(IceRow.ROWSTATE_ADDED) == 0
                              select ttLaborDtl_Row).FirstOrDefault();

                if (ttLaborDtl.LaborEntryMethod.Compare("X") == 0 && (ttLaborDtl.LaborQty + ttLaborDtl.ScrapQty + ttLaborDtl.DiscrepQty) != 0)
                {
                    throw new BLException(Strings.ReportQtyNotAllowedForTimeAndBackflushOperations, "LaborDtl");
                }
                /* Make PayrollDate be the same as ClockInDate */
                if (ttLaborDtl.ClockInDate == null)
                {
                    ttLaborDtl.PayrollDate = null;
                }
                else
                {
                    ttLaborDtl.PayrollDate = ttLaborDtl.ClockInDate;
                }


                if (ttLaborDtl.LaborHedSeq == 0)
                {
                    LaborHed = this.FindFirstLaborHed(Session.CompanyID, ttLaborDtl.EmployeeNum, ttLaborDtl.PayrollDate);
                    if (LaborHed != null && ExistsLaborHedPosted(Session.CompanyID, LaborHed.LaborHedSeq))
                        LaborHed = null;
                }
                else
                {
                    LaborHed = this.FindFirstLaborHed(Session.CompanyID, ttLaborDtl.LaborHedSeq);
                    if (LaborHed != null && ExistsLaborHedPosted(Session.CompanyID, LaborHed.LaborHedSeq))
                        LaborHed = null;
                }

                if (LaborHed != null)
                {
                    ttLaborDtl.LaborHedSeq = LaborHed.LaborHedSeq;



                    foreach (var ttLbrScrapSerialNumbers_iterator in (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                                                      where ttLbrScrapSerialNumbers_Row.Company.KeyEquals(Session.CompanyID)
                                                                      && ttLbrScrapSerialNumbers_Row.LaborHedSeq == 0
                                                                      && ttLbrScrapSerialNumbers_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq
                                                                      select ttLbrScrapSerialNumbers_Row))
                    {
                        ttLbrScrapSerialNumbers = ttLbrScrapSerialNumbers_iterator;
                        ttLbrScrapSerialNumbers.LaborHedSeq = LaborHed.LaborHedSeq;
                    }



                    ttLaborHed = (from ttLaborHed_Row in CurrentFullTableset.LaborHed
                                  where ttLaborHed_Row.RowMod.Compare(IceRow.ROWSTATE_ADDED) == 0
                                  select ttLaborHed_Row).FirstOrDefault();
                    if (bDelttLaborHedRows == null)
                    {
                        bDelttLaborHedRows = new List<_LaborHedRow>();
                    }
                    bDelttLaborHed = new _LaborHedRow();
                    bDelttLaborHedRows.Add(bDelttLaborHed);
                    BufferCopy.Copy(ttLaborHed, ref bDelttLaborHed);
                    bDelttLaborHed.ExistingLaborHedSeq = LaborHed.LaborHedSeq;
                    CurrentFullTableset.LaborHed.Remove(ttLaborHed);
                    /* Add LaborHed to the dataset */
                    ttLaborHed = new Erp.Tablesets.LaborHedRow();
                    CurrentFullTableset.LaborHed.Add(ttLaborHed);
                    BufferCopy.Copy(LaborHed, ref ttLaborHed);
                    ttLaborHed.SysRowID = Guid.NewGuid();
                    ttLaborHed.SysRowID = LaborHed.SysRowID;
                    this.LaborHedAfterGetRows();
                    LaborHed_Foreign_Link();
                }
                else
                {


                    ttLaborHed = (from ttLaborHed_Row in CurrentFullTableset.LaborHed
                                  where ttLaborHed_Row.RowMod.Compare(IceRow.ROWSTATE_ADDED) == 0
                                  select ttLaborHed_Row).FirstOrDefault();
                    if (ttLaborDtl.ClockInDate == null)
                    {
                        ttLaborHed.ClockInDate = null;
                    }
                    else
                    {
                        ttLaborHed.ClockInDate = ttLaborDtl.ClockInDate;
                    }

                    if (ttLaborDtl.PayrollDate == null)
                    {
                        ttLaborHed.PayrollDate = null;
                    }
                    else
                    {
                        ttLaborHed.PayrollDate = ttLaborDtl.PayrollDate;
                    }

                    ttLaborHed.PayrollDateNav = ttLaborHed.PayrollDate;
                }
            }
            else
            {


                ttLaborDtl = (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                              where ttLaborDtl_Row.RowMod.Compare(IceRow.ROWSTATE_ADDED) == 0
                              select ttLaborDtl_Row).FirstOrDefault();
                if (ttLaborDtl != null && ttLaborDtl.LaborHedSeq != 0)
                {


                    LaborHed = this.FindFirstLaborHed(Session.CompanyID, ttLaborDtl.LaborHedSeq);
                    if ((LaborHed == null) ||
                        (!ttLaborDtl.MES && !ttLaborDtl.HH && LaborHed != null && LaborHed.ClockInDate.Value.Date != ttLaborDtl.ClockInDate.Value.Date))
                    {
                        if (ttLaborDtl.ClockInDate == null)
                        {
                            ttLaborDtl.PayrollDate = null;
                        }
                        else
                        {
                            ttLaborDtl.PayrollDate = ttLaborDtl.ClockInDate;
                        }



                        LaborHed = this.FindFirstLaborHed2(Session.CompanyID, ttLaborDtl.EmployeeNum, ttLaborDtl.PayrollDate);
                        if (LaborHed == null)
                        {

                            GetNewLaborHed(ref LaborDS);
                            CurrentFullTableset = LaborDS;
                            ttLaborHed.EmployeeNum = ttLaborDtl.EmployeeNum;
                            if (ttLaborDtl.ClockInDate != null)
                            {
                                if (ttLaborDtl.ClockInDate == null)
                                {
                                    ttLaborHed.PayrollDate = null;
                                }
                                else
                                {
                                    ttLaborHed.PayrollDate = ttLaborDtl.ClockInDate;
                                }

                                ttLaborHed.PayrollDateNav = ttLaborHed.PayrollDate;

                                if (ttLaborDtl.ClockInDate == null)
                                {
                                    ttLaborHed.ClockInDate = null;
                                }
                                else
                                {
                                    ttLaborHed.ClockInDate = ttLaborDtl.ClockInDate;
                                }

                                ttLaborHed.ClockInTime = ttLaborDtl.ClockinTime;
                                ttLaborHed.ClockOutTime = ttLaborDtl.ClockOutTime;
                            }
                            LaborHedAfterGetNew1(false);
                            if (ttLaborDtl.ClockInDate != null)
                            {
                                if (ttLaborDtl.ClockInDate == null)
                                {
                                    ttLaborHed.PayrollDate = null;
                                }
                                else
                                {
                                    ttLaborHed.PayrollDate = ttLaborDtl.ClockInDate;
                                }

                                ttLaborHed.PayrollDateNav = ttLaborHed.PayrollDate;

                                if (ttLaborDtl.ClockInDate == null)
                                {
                                    ttLaborHed.ClockInDate = null;
                                }
                                else
                                {
                                    ttLaborHed.ClockInDate = ttLaborDtl.ClockInDate;
                                }

                                ttLaborHed.ClockInTime = ttLaborDtl.ClockinTime;
                                ttLaborHed.ClockOutTime = ttLaborDtl.ClockOutTime;
                            }
                            ttLaborDtl.LaborHedSeq = ttLaborHed.LaborHedSeq;


                            foreach (var ttLbrScrapSerialNumbers_iterator in (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                                                              where ttLbrScrapSerialNumbers_Row.Company.KeyEquals(Session.CompanyID)
                                                                              && ttLbrScrapSerialNumbers_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                                                                              && ttLbrScrapSerialNumbers_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq
                                                                              select ttLbrScrapSerialNumbers_Row))
                            {
                                ttLbrScrapSerialNumbers = ttLbrScrapSerialNumbers_iterator;
                                ttLbrScrapSerialNumbers.LaborHedSeq = LaborHed.LaborHedSeq;
                            }
                        }
                        else
                        {
                            ttLaborDtl.LaborHedSeq = LaborHed.LaborHedSeq;


                            foreach (var ttLbrScrapSerialNumbers_iterator in (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                                                              where ttLbrScrapSerialNumbers_Row.Company.KeyEquals(Session.CompanyID)
                                                                              && ttLbrScrapSerialNumbers_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                                                                              && ttLbrScrapSerialNumbers_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq
                                                                              select ttLbrScrapSerialNumbers_Row))
                            {
                                ttLbrScrapSerialNumbers = ttLbrScrapSerialNumbers_iterator;
                                ttLbrScrapSerialNumbers.LaborHedSeq = LaborHed.LaborHedSeq;
                            }
                        }
                    }
                }
            }
            /**************************/
            /*       SCR 61010        */
            /* Time and Expense Entry */
            /**************************/
            this.processTimeWeeklyView();
        }

        /// <summary>
        /// This proc will return the whereclause for the role code combo
        /// Customers
        /// </summary>
        /// <param name="ipJobNum">JobNum</param>
        /// <param name="ipAssemblySeq">AssemblySeq</param>
        /// <param name="ipOprSeq">OprSeq</param>
        /// <param name="ipEmpID">EmpID</param>
        /// <param name="whereClause">Where Clause to use for role code combo</param>
        public string BuildJobOperPrjRoleList(string ipJobNum, int ipAssemblySeq, int ipOprSeq, string ipEmpID, out string whereClause)
        {
            whereClause = string.Empty;
            string projectID = string.Empty;
            string phaseID = string.Empty;
            string roleCodeAUX = string.Empty;
            string roleCodeList = string.Empty;
            int iCounter = 0;


            JobHead = this.FindFirstJobHead2(Session.CompanyID, ipJobNum);
            if (JobHead != null)
            {
                projectID = JobHead.ProjectID;
                phaseID = JobHead.PhaseID;
            }
            roleCodeList = this.buildValidRoleCodeList(ipEmpID, projectID, phaseID, ipJobNum, ipAssemblySeq, ipOprSeq);
            if (roleCodeList.NumEntries(Ice.Constants.LIST_DELIM[0]) > 0)
            {
                for (iCounter = 1; iCounter <= roleCodeList.NumEntries(Ice.Constants.LIST_DELIM[0]); iCounter++)
                {
                    roleCodeAUX = roleCodeList.Entry(iCounter - 1, Ice.Constants.LIST_DELIM);
                    if (!String.IsNullOrEmpty(whereClause))
                    {
                        whereClause = whereClause + " or RoleCode = '" + roleCodeAUX + "'";
                    }
                    else
                    {
                        whereClause = "RoleCode = '" + roleCodeAUX + "'";
                    }
                }
                return whereClause;
            }
            else
            {
                whereClause = "RoleCode = '!@#$%^'";
            }

            return whereClause;
        }

        private void calcBurdenHours()
        {
            decimal[] MinuteArray = new decimal[1440];
            int MinuteIndex = 0;
            int RunMin = 0;
            int StartMin = 0;
            int EndMin = 0;
            decimal V_BurdenHrs = decimal.Zero;
            /* est hours for this ttLaborDtl */
            decimal EstHrs = decimal.Zero;
            /* est hours for each ttLaborDtl involved */
            decimal AltEstHrs = decimal.Zero;
            int tmpmin = 0;
            Erp.Tables.LaborDtl AltLaborDtl = null;

            ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, ttLaborDtl.ResourceGrpID);
            LibShiftBrk.GetTotalBreakMinutes(LaborHed.Shift, ttLaborDtl.ClockInDate, ttLaborDtl.ClockinTime, ttLaborDtl.ClockOutTime, false, false, true, true, LaborHed.ActLunchOutTime, LaborHed.ActLunchInTime, out tmpmin, out tt_brk_array);
            if ((ResourceGroup.SplitBurden && ResourceGroup.UseEstimates == true))
            { /* indirect */
                if (ttLaborDtl.LaborType.Compare("I") != 0)
                {
                    JobOper = this.FindFirstJobOper2(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                    if (JobOper != null)
                    {
                        switch (ttLaborDtl.LaborType.ToUpperInvariant())
                        {
                            case "S":
                                {
                                    EstHrs = JobOper.EstSetHours * 60;
                                }
                                break;
                            case "P":
                                {
                                    EstHrs = JobOper.EstProdHours * 60;
                                }
                                break;
                            default:
                                EstHrs = 1;
                                break;
                        }
                    }
                }
                else
                {
                    EstHrs = 1;
                }
            }
            else
            {
                EstHrs = 1;
            }

            if (ResourceGroup.SplitBurden != true)
            {
                RunMin = (ttLaborDtl.ClockOutMinute - ttLaborDtl.ClockInMInute);
                StartMin = 1;
                EndMin = ((StartMin + RunMin) - 1);
                if (EndMin > 1440)
                {
                    EndMin = 1440;
                }

                for (MinuteIndex = StartMin; MinuteIndex <= EndMin; MinuteIndex = MinuteIndex + 1)
                {
                    if (tt_brk_array[MinuteIndex - 1] == false)
                    {
                        MinuteArray[MinuteIndex - 1] = 1;
                    }
                }
            }
            else
            {
                foreach (var AltLaborDtl_iterator in (this.SelectLaborDtl(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.ResourceID, ttLaborDtl.ClockInMInute, ttLaborDtl.ClockOutMinute)))
                {
                    AltLaborDtl = AltLaborDtl_iterator;
                    if (ResourceGroup.UseEstimates == true)
                    { /* indirect */
                        if (ttLaborDtl.LaborType.Compare("I") != 0)
                        {
                            JobOper = this.FindFirstJobOper3(AltLaborDtl.Company, AltLaborDtl.JobNum, AltLaborDtl.AssemblySeq, AltLaborDtl.OprSeq);
                            if (JobOper != null)
                            {
                                switch (ttLaborDtl.LaborType.ToUpperInvariant())
                                {
                                    case "S":
                                        {
                                            AltEstHrs = JobOper.EstSetHours * 60;
                                        }
                                        break;
                                    case "P":
                                        {
                                            AltEstHrs = JobOper.EstProdHours * 60;
                                        }
                                        break;
                                    default:
                                        AltEstHrs = 1;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            AltEstHrs = 1;
                        }
                    }
                    else
                    {
                        AltEstHrs = 1;
                    }

                    if (AltLaborDtl.ClockInMInute >= ttLaborDtl.ClockInMInute)
                    {
                        RunMin = (AltLaborDtl.ClockOutMinute - AltLaborDtl.ClockInMInute);
                        StartMin = ((AltLaborDtl.ClockInMInute - ttLaborDtl.ClockInMInute) + 1);
                    }
                    else
                    {
                        RunMin = (AltLaborDtl.ClockOutMinute - ttLaborDtl.ClockInMInute);
                        StartMin = 1;
                    }
                    EndMin = ((StartMin + RunMin) - 1);
                    if (EndMin > 1440)
                    {
                        EndMin = 1440;
                    }

                    for (MinuteIndex = StartMin; MinuteIndex <= EndMin; MinuteIndex = MinuteIndex + 1)
                    {
                        if (tt_brk_array[MinuteIndex - 1] == false)
                        {
                            MinuteArray[MinuteIndex - 1] = ((MinuteArray[MinuteIndex - 1]) + AltEstHrs);
                        }
                    }
                }
            }/* if ResourceGroup.split burden NE YES */

            EndMin = (ttLaborDtl.ClockOutMinute - ttLaborDtl.ClockInMInute);
            if (EndMin > 1440)
            {
                EndMin = 1440;
            }

            for (MinuteIndex = 1; MinuteIndex <= EndMin; MinuteIndex++)
            {
                if ((MinuteArray[MinuteIndex - 1]) > 0)
                {
                    V_BurdenHrs = V_BurdenHrs + (EstHrs / (MinuteArray[MinuteIndex - 1]));
                }
            }
            /* ERPS-213225 - Removed the special consideration for midnight clock out time to hardcode 24 hours in the calculation. *
             * It is working correctly now using the same formula which also corrected the incorrect split of labor hours among the *
             * simultaneous jobs overlapping with the current job activity.                                                         */
            /* HANDLED PROPERLY WHEN LUNCH AND / OR BREAK TIME IS TAKEN WITHIN THE SHIFT. */
            ttLaborDtl.BurdenHrs = Math.Round((V_BurdenHrs / 60), 2, MidpointRounding.AwayFromZero);

        }
        /* -----------------------------------------------------------
          Purpose: This routine determines how many other jobs this employee
                   worked on simultaneously for each minute during the period
                   of time for this current transaction.  The logic used will
                   be to build an array of 1440 integer elements.  Each element
                   represents the number of active jobs per minute for the 
                   specific employee.  The array is based on the time when the
                   current transaction was started. In other words element 1
                   represents the start time.  Next the routine reads all the
                   LaborDtl transactions that fall into this time period and are
                   for the same employee.  Each time a record is found the 
                   appropriate array elements are updated by 1.  Once the array
                   is built the routine loops through each element accumulating
                   the labor time to be posted for this transaction.  It is
                   calculated as ( 1 / value of the element).

          Notes:   MinuteArray = An array representing the 1440 minutes in
                                 a working day.
                   MinuteIndex = A pointer to the elements in above array.
                   RunMin      = Number of minutes a particular transaction ran
                                 for. Number of MinuteArray elements incremented.
                   StartMin    = Beginning MinuteArray element to start incrementing
                                 from.
                   EndMin      = End MinuteArray element to end incrementing.

        -------------------------------------------------------------*/
        private void calcLaborHours()
        {
            decimal[] MinuteArray = new decimal[1440];
            int MinuteIndex = 0;
            int RunMin = 0;
            int StartMin = 0;
            int EndMin = 0;
            decimal V_LaborHrs = decimal.Zero;
            Erp.Tables.LaborDtl altLaborDtl = null;/* est hours for this labordtl */
            decimal EstHrs = decimal.Zero;
            /* est hours for each labordtl involved */
            decimal AltEstHrs = decimal.Zero;
            int tmpmin = 0;

            //If it's called from Ad hoc Job Output, don't calculate labor hours. Labor hours will be always one minute per PCID or set of PCIDs generated in one transaction.
            if (ErpCallContext.ContainsKey("FromPkgControlGenPCID")) return;

            LibShiftBrk.GetTotalBreakMinutes(LaborHed.Shift, ttLaborDtl.ClockInDate, ttLaborDtl.ClockinTime, ttLaborDtl.ClockOutTime, false, false, true, true, LaborHed.ActLunchOutTime, LaborHed.ActLunchInTime, out tmpmin, out tt_brk_array);
            if (ResourceGroup.UseEstimates == true)
            { /* indirect */
                if (ttLaborDtl.LaborType.Compare("I") != 0)
                {
                    JobOper = this.FindFirstJobOper4(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                    if (JobOper != null)
                    {
                        switch (ttLaborDtl.LaborType.ToUpperInvariant())
                        {                 /* Multiply the EstSetHours or EstProdHours by 60 to get it into minutes so it is in the same denomination as the MinuteArray */
                            case "S":
                                {
                                    EstHrs = JobOper.EstSetHours * 60;
                                }
                                break;
                            case "P":
                                {
                                    EstHrs = JobOper.EstProdHours * 60;
                                }
                                break;
                            default:
                                EstHrs = 1;
                                break;
                        }
                    }
                }
                else
                {
                    EstHrs = 1;
                }
            }
            else
            {
                EstHrs = 1;
            }

            using (TransactionScope trans = ErpContext.CreateDefaultTransactionScope())
            {
                foreach (var altLaborDtl_iterator in (this.SelectLaborDtlWithUpdLock(Session.CompanyID, LaborHed.EmployeeNum, ttLaborDtl.LaborHedSeq, ttLaborDtl.ClockOutMinute, ttLaborDtl.ClockInMInute)))
                {
                    altLaborDtl = altLaborDtl_iterator;
                    if (altLaborDtl.LaborDtlSeq == ttLaborDtl.LaborDtlSeq)
                    {
                        altLaborDtl.ClockOutMinute = ttLaborDtl.ClockOutMinute;
                        Db.Validate(altLaborDtl);
                        ttLaborDtl.SysRevID = (long)altLaborDtl.SysRevNum;
                    }
                    if (ResourceGroup.UseEstimates == true)
                    { /* indirect */
                        if (altLaborDtl.LaborType.Compare("I") != 0)
                        {
                            JobOper = this.FindFirstJobOper5(altLaborDtl.Company, altLaborDtl.JobNum, altLaborDtl.AssemblySeq, altLaborDtl.OprSeq);
                            if (JobOper != null)
                            {
                                switch (altLaborDtl.LaborType.ToUpperInvariant())
                                {                     /* Multiply the EstSetHours or EstProdHours by 60 to get it into minutes so it is in the same denomination as the MinuteArray */
                                    case "S":
                                        {
                                            AltEstHrs = JobOper.EstSetHours * 60;
                                        }
                                        break;
                                    case "P":
                                        {
                                            AltEstHrs = JobOper.EstProdHours * 60;
                                        }
                                        break;
                                    default:
                                        AltEstHrs = 1;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            AltEstHrs = 1;
                        }
                    }
                    else
                    {
                        AltEstHrs = 1;
                    }

                    if (altLaborDtl.ClockInMInute >= ttLaborDtl.ClockInMInute)
                    {
                        RunMin = (altLaborDtl.ClockOutMinute - altLaborDtl.ClockInMInute);
                        StartMin = ((altLaborDtl.ClockInMInute - ttLaborDtl.ClockInMInute) + 1);
                    }
                    else
                    {
                        RunMin = (altLaborDtl.ClockOutMinute - ttLaborDtl.ClockInMInute);
                        StartMin = 1;
                    }
                    EndMin = ((StartMin + RunMin) - 1);
                    if (EndMin > 1440)
                    {
                        EndMin = 1440;
                    }

                    if (altLaborDtl.Downtime != true)
                    {
                        for (MinuteIndex = StartMin; MinuteIndex <= EndMin; MinuteIndex = MinuteIndex + 1)
                        {
                            if (tt_brk_array[MinuteIndex - 1] == false)
                            {
                                MinuteArray[MinuteIndex - 1] = ((MinuteArray[MinuteIndex - 1]) + AltEstHrs);
                            }
                        }
                    }
                }/* end for each labordtl */
                trans.Complete();
            }

            EndMin = (ttLaborDtl.ClockOutMinute - ttLaborDtl.ClockInMInute);
            if (EndMin > 1440)
            {
                EndMin = 1440;
            }

            for (MinuteIndex = 1; MinuteIndex <= EndMin; MinuteIndex++)
            {
                if ((MinuteArray[MinuteIndex - 1]) > 0)
                {
                    V_LaborHrs = V_LaborHrs + (EstHrs / (MinuteArray[MinuteIndex - 1]));
                }
            }
            /* ERPS-213225 - Removed the special consideration for midnight clock out time to hardcode 24 hours in the calculation. *
             * It is working correctly now using the same formula which also corrected the incorrect split of labor hours among the *
             * simultaneous jobs overlapping with the current job activity.                                                         */
            /* HANDLED PROPERLY WHEN LUNCH AND / OR BREAK TIME IS TAKEN WITHIN THE SHIFT. */
            ttLaborDtl.LaborHrs = Math.Round((V_LaborHrs / 60), 2, MidpointRounding.AwayFromZero);

        }

        private void calcOpDtlBurdenRate(string ipResourceID, string ipResourceGrpID, string ipCapabilityID, decimal ipLaborRate, out decimal opProdBurRate, out decimal opSetupBurRate)
        {
            opProdBurRate = decimal.Zero;
            opSetupBurRate = decimal.Zero;
            opProdBurRate = 0;
            opSetupBurRate = 0;
            Db.Release(ref Resource);
            Db.Release(ref ResourceGroup);
            Db.Release(ref Capability);
            /* use the resource rates if specified else use resource group rates */
            if (!String.IsNullOrEmpty(ipResourceID))
            {


                Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, ipResourceID);
                if (Resource == null)
                {
                    return;      /* find the related resource group in case rates should come from group */
                }



                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Resource.Company, Resource.ResourceGrpID);
            }
            else if (!String.IsNullOrEmpty(ipResourceGrpID))
            {


                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, ipResourceGrpID);
                if (ResourceGroup == null)
                {
                    return;
                }
            }
            else if (!String.IsNullOrEmpty(ipCapabilityID))
            {


                Capability = this.FindFirstCapability(Session.CompanyID, ipCapabilityID);
                if (Capability == null)
                {
                    return;      /* find the primary resource group to get the rates from */
                }



                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Capability.Company, Capability.PrimaryResourceGrpID);
            }
            /* use the rates from resource if available else use the resource group rates */
            if (Resource != null)
            { /* get burden rates from resource */
                if ((Resource.GetDefaultBurdenFromGroup == false && Resource.Location == true))
                {
                    if (Resource.BurdenType.Compare("F") == 0)
                    {
                        opSetupBurRate = Resource.SetupBurRate;
                        opProdBurRate = Resource.ProdBurRate;
                    }
                    else
                    {
                        opSetupBurRate = (ipLaborRate * Resource.SetupBurRate) / 100;
                        opProdBurRate = (ipLaborRate * Resource.ProdBurRate) / 100;
                    }
                    return;
                }
                else
                {


                    ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, Resource.ResourceGrpID);
                }
            } /* available Resource */
            if (ResourceGroup != null && ResourceGroup.Location)
            {
                if (ResourceGroup.BurdenType.Compare("F") == 0)
                {
                    opSetupBurRate = ResourceGroup.SetupBurRate;
                    opProdBurRate = ResourceGroup.ProdBurRate;
                }
                else
                {
                    opSetupBurRate = (ipLaborRate * ResourceGroup.SetupBurRate) / 100;
                    opProdBurRate = (ipLaborRate * ResourceGroup.ProdBurRate) / 100;
                }
            }
        }

        /// <summary>
        /// This method calculates the laborhed total hours for display
        /// </summary>
        private void calcTotHrs(decimal laborHrs)
        {
            decimal oldLaborHrs = decimal.Zero;
            decimal totLabHrs = decimal.Zero;

            /* SCR 120033 - if the ttLaborDtl.LaborHedSeq = 0 then it means that there's no LaborHed *
             * yet and no other LaborDtl exists besides the ttlaborDtl. If this is the case then no  *
             * need to go through AltLaborDtl to improve performance.                                */
            if (ttLaborDtl.LaborHedSeq == 0) return;


            //LABORHRS_LOOP:
            foreach (var altLaborDtl in this.SelectLaborDtl(Session.CompanyID, ttLaborDtl.LaborHedSeq))
            {
                totLabHrs = totLabHrs + altLaborDtl.LaborHrs;
                if (altLaborDtl.SysRowID == ttLaborDtl.SysRowID)
                {
                    oldLaborHrs = altLaborDtl.LaborHrs;
                }
            }
            /* existing records */
            if (ttLaborDtl.RowMod.Compare(IceRow.ROWSTATE_UPDATED) == 0)
            {
                totLabHrs = (totLabHrs - oldLaborHrs) + ttLaborDtl.LaborHrs;
                if ((totLabHrs > 999.99m) || (totLabHrs < -99.99m))
                {
                    ttLaborDtl.DspTotHours = Compatibility.Convert.ToString(totLabHrs, "->>>>9");
                }
                else
                {
                    ttLaborDtl.DspTotHours = Compatibility.Convert.ToString(totLabHrs, "->9.99");
                }
            }
        }

        /// <summary>
        /// This method should call when EquipID is changed
        /// </summary>
        /// <param name= "equipID"> equipID </param>
        /// <param name="ds">Labor Entry Data set</param>
        public void ChangeEquipID(string equipID, ref LaborTableset ds)
        {
            CurrentFullTableset = ds;


            ttLaborEquip = (from ttLaborEquip_Row in ds.LaborEquip
                            where !String.IsNullOrEmpty(ttLaborEquip_Row.RowMod)
                            select ttLaborEquip_Row).FirstOrDefault();
            if (ttLaborEquip == null)
            {
                throw new BLException(Strings.LaborEquipRecordHasNotChanged, "LaborEquip");
            }

            if (this.ExistsLaborEquip(Session.CompanyID, ttLaborEquip.LaborHedSeq, ttLaborEquip.LaborDtlSeq, ttLaborEquip.SysRowID, equipID))
            {
                throw new BLException(Strings.EquipMustBeUnique, "LaborEquip", "EquipID");
            }


            Equip = this.FindFirstEquip(Session.CompanyID, equipID);
            if (Equip == null)
            {
                throw new BLException(Strings.EquipIDDoesNotExist, "Equip");
            }
            if (Equip.InActive)
            {
                throw new BLException(Strings.EquipIDIsInactive, "Equip");
            }


            LaborDtl = this.FindFirstLaborDtl2(Session.CompanyID, ttLaborEquip.LaborHedSeq, ttLaborEquip.LaborDtlSeq);
            if (Equip.LaborMeterOpt.Compare("Hrs") == 0)
            {
                ttLaborEquip.Hours = LaborDtl.BurdenHrs;
            }
            else if (Equip.LaborMeterOpt.Compare("Qty") == 0)
            {
                ttLaborEquip.Qty = LaborDtl.LaborQty;
            }
        }

        /// <summary>
        /// This method clears the JobNumber and Quantity fields when the LaborType changes to Indirect
        /// leaves the values as is if changed between Production and Setup
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        public void ChangeIndirectCode(ref LaborTableset ds)
        {
            CurrentFullTableset = ds;
            if (ttLaborDtl == null)
            {
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttLaborDtl == null && ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborHasNotChanged, "LaborDtl");
            }
            if (ttLaborDtl != null)
            {
                this.clearRecordBuffer();



                Indirect = this.FindFirstIndirect(Session.CompanyID, ttLaborDtl.IndirectCode);
                if (Indirect != null)
                {
                    ttLaborDtl.ExpenseCode = Indirect.ExpenseCode;
                    ttLaborDtl.Downtime = Indirect.DownTime;
                }

                LaborDtl_Foreign_Link();
            }
            if (ttTimeWeeklyView != null)
            {


                Indirect = this.FindFirstIndirect2(Session.CompanyID, ttTimeWeeklyView.IndirectCode);
                if (Indirect != null && String.IsNullOrEmpty(ttTimeWeeklyView.ExpenseCode))
                {
                    ttTimeWeeklyView.ExpenseCode = Indirect.ExpenseCode;
                }
                TimeWeeklyView_Foreign_Link();
            }
        }

        /// <summary>
        /// This method clears the JobNumber and Quantity fields when the LaborType changes to Indirect
        /// leaves the values as is if changed between Production and Setup
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        public void ChangeLaborType(ref LaborTableset ds)
        {
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }
            this.clearRecordBuffer();
            ttLaborDtl.JobType = this.getJobType(ttLaborDtl.JobNum);
            ttLaborDtl.FSComplete = this.getComplete(ttLaborDtl.CallNum);
            ttLaborDtl.ProdDesc = this.getProdDesc(ttLaborDtl.LaborType);
            ttLaborDtl.DisplayJob = this.getDisplayJob(ttLaborDtl.LaborType, ttLaborDtl.IndirectCode, ttLaborDtl.JobNum);
            ttLaborDtl.CompleteFlag = ((ttLaborDtl.CallNum == 0) ? ttLaborDtl.Complete : ttLaborDtl.FSComplete);
            JobOper = this.FindFirstJobOper11(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
            if (JobOper != null)
            {
                var outResourceGrpID = string.Empty;
                var outResourceID = string.Empty;
                var outCapabilityID = string.Empty;
                this.defaultOprSeq2(ttLaborDtl.LaborType, ref outResourceGrpID, ref outResourceID, ref outCapabilityID);
                ttLaborDtl.ResourceGrpID = outResourceGrpID;
                ttLaborDtl.ResourceID = outResourceID;
            }

            if (!String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID))
            {
                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, ttLaborDtl.ResourceGrpID);
                if (ResourceGroup != null)
                {
                    ttLaborDtl.JCDept = ((ResourceGroup != null) ? ResourceGroup.JCDept : "");
                }
            }

            JobHead _JobHead = FindFirstJobHead(ttLaborDtl.Company, ttLaborDtl.JobNum);
            if (_JobHead != null)
            {
                if (!String.IsNullOrEmpty(_JobHead.ExpenseCode))
                {
                    ttLaborDtl.ExpenseCode = _JobHead.ExpenseCode;
                }
                else
                {
                    EmpBasic = this.FindFirstEmpBasic3(Session.CompanyID, ttLaborDtl.EmployeeNum);
                    if (EmpBasic != null)
                    {
                        ttLaborDtl.ExpenseCode = EmpBasic.ExpenseCode;
                    }
                }
            }

            ttLaborDtl.LaborRate = LibLaborRate.LaborRateCalc(ttLaborDtl);
            ttLaborDtl.BillServiceRate = LibBillableServiceRate.BillableServiceRateCalc(ttLaborDtl.EmployeeNum, ttLaborDtl.ExpenseCode, ttLaborDtl.LaborType, ttLaborDtl.CallNum, ttLaborDtl.CallLine, ttLaborDtl.JobNum);

            LaborDtl_Foreign_Link();
        }

        /// <summary>
        /// Main logic from ChangeResourceId to validate the resource id assigned to a Job.
        /// This method does not depend on a tableset or LaborDtl record.
        /// </summary>
        /// <param name="resourceGrpId">Resource Group of the Job</param>
        /// <param name="proposedResId">Proposed Resource ID to assign.</param>
        /// <param name="pcMsg">Warning message to be shown to the user.</param>
        public void CheckResourceId(string resourceGrpId, string proposedResId, out string pcMsg)
        {
            pcMsg = String.Empty;
            string cTmp = string.Empty;
            if (String.IsNullOrEmpty(proposedResId)) return;
            Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, proposedResId);
            if (Resource == null)
            {
                cTmp = Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.ResourceID);
                throw new BLException(cTmp);
            }
            if (Resource.Inactive == true)
            {
                throw new BLException(Strings.TheResouIsInactAndMayNotBeSelec, "LaborDtl", "ResourceID");
            }
            if (Resource.Location == false)
            {
                throw new BLException(Strings.TheResouIsNotALocatAndMayNotBeSelec(Resource.ResourceID), "LaborDtl", "ResourceID");
            }
            if (Resource.ResourceGrpID.Compare(resourceGrpId) != 0)
            {
                pcMsg = Strings.ResouBelongsToADiffeResouResouWillBeChangedDoYou(Resource.ResourceID);
            }
        }

        /// <summary>
        /// For use with MES (ShopFloor) only.
        /// This method checks the Resource Group of the proposed Resource, and if it  
        /// is different than the current Resource Group, provides a warning question 
        /// suitable for presenting to the user.
        /// The UI code should place the user's answer to the question in the 
        /// ttLaborDtl.OkToChangeResourceGrpID.
        /// This method should be called prior to calling the DefaultResourceID method.
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="pcResourceID">The ID of the machine that was used to do the work.</param>
        /// <param name="pcMsg">The message sent as output for the user</param>
        public void ChangeResourceId(ref LaborTableset ds, string pcResourceID, out string pcMsg)
        {
            pcMsg = string.Empty;
            CurrentFullTableset = ds;
            string cTmp = string.Empty;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged);
            }
            if (!String.IsNullOrEmpty(pcResourceID) && ttLaborDtl.ActiveTrans == true)
            {
                CheckResourceId(ttLaborDtl.ResourceGrpID, pcResourceID, out pcMsg);
                ttLaborDtl.ResourceID = pcResourceID;
            }
        }

        /// <summary>
        /// This method checks if the current employee is already working on a Job/Asm/Opr/Resource combination 
        /// If he/she is already working on it, the opMessage will be populated with an error message
        /// </summary>
        /// <param name="ipEmpID">The current Employee ID</param>
        /// <param name="ipLaborHedSeq">LaborHed Sequence</param>
        /// <param name="ipJobNum">Job Number</param>
        /// <param name="ipAsmSeq">Assembly Sequence</param>
        /// <param name="ipOprSeq">Operation Sequence</param>
        /// <param name="ipResourceID">Resource ID</param>
        /// <param name="opMessage">Error Message</param>
        public void CheckEmployeeActivity(string ipEmpID, int ipLaborHedSeq, string ipJobNum, int ipAsmSeq, int ipOprSeq, string ipResourceID, out string opMessage)
        {
            opMessage = string.Empty;



            LaborDtl = this.FindFirstLaborDtl(Session.CompanyID, ipEmpID, ipLaborHedSeq, ipJobNum, ipAsmSeq, ipOprSeq, ipResourceID, true);
            if (LaborDtl != null)
            {
                opMessage = Strings.EmploIsAlreadyActiveOnThisSeque;
            }
        }

        /// <summary>
        /// Check if there are NonConformance records, if they exists it will ask the user for his approval to delete them
        /// </summary>
        /// <param name="jobNum">JobNumber</param>
        /// <param name="laborHedSeq">LaborHedSeq </param>
        /// <param name="laborDtlSeq">LaborDtlSeq </param>
        /// <param name="pcMsg">The message sent as output for the user</param>
        public void CheckNonConformance(string jobNum, int laborHedSeq, int laborDtlSeq, out string pcMsg)
        {
            pcMsg = string.Empty;

            if (this.ExistsNonConf(Session.CompanyID, jobNum, laborHedSeq, laborDtlSeq))
            {
                pcMsg = Strings.ExistingNonConformanceRecordsQuestion;
            }

        }

        /// <summary>
        /// Performs all First Article Validations
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="pcMsg">The message sent as output for the user</param>
        public void CheckFirstArticleWarning(ref LaborTableset ds, out string pcMsg)
        {
            pcMsg = string.Empty;
            CurrentFullTableset = ds;
            string VTxt = string.Empty;
            string DspWrnStr = "1";
            string cTmp = string.Empty;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where ttLaborDtl_Row.RowMod.Compare("U") == 0 || ttLaborDtl_Row.RowMod.Compare("A") == 0
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged);
            }/* if not available ttLaborDtl */
            /* CHECK FOR AND GENERATE LABOR WARNINGS - 7486 */

            /* indirect */
            if (ttJCSyst.GenLaborWarnMsg &&
                ttLaborDtl.LaborType.Compare("I") != 0)
            {
                VJobNum = ttLaborDtl.JobNum;
                VAssemblySeq = ttLaborDtl.AssemblySeq;
                VOprSeq = ttLaborDtl.OprSeq;
                VWCCode = ttLaborDtl.ResourceGrpID;
                VEmployeeNum = ttLaborDtl.EmployeeNum;
                DispWarn = true;
                FromProg = "DataColl";
                DspWrnStr = ((DispWarn) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));
            }/* if JCSyst.GenLaborWarnMsg... */



            JobOper = this.FindFirstJobOper(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.ResourceGrpID.Length > 0);
            /* production */
            if (ttLaborDtl.LaborType.Compare("P") == 0)
            {
                LibWARNDEF.RunDEWRN200(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.ResourceID, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(200, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
            }/* if ttLaborDtl.LaborType = "P"... */
            if (ttLaborDtl.LaborType.Compare("S") == 0 /* setup */ && JobOper != null && JobOper.FAQty > 0)
            {
                LibWARNDEF.RunDEWRN210(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.ResourceID, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(210, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
            }/* if ttLaborDtl.LaborType = "S":U and JobOper.FAQty > 0 */
            /* production */
            if (ttLaborDtl.LaborType.Compare("P") == 0)
            {
                LibWARNDEF.RunDEWRN220(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.ResourceID, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(220, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }/* if VContinue <> "" */
                    if (ttJCSyst.PreventFABypass == true)
                    {
                        cTmp = Strings.UnableToStartProduUntilFirstArticleInspeHasBeen;
                        throw new BLException(cTmp);
                    }
                }
            }/* if ttLaborDtl.LaborType = "P" */
            if (pcMsg.Length > 0)
            {
                pcMsg = pcMsg + " " + Strings.DoYouWantToContinue;
            }
        }

        /// <summary>
        /// This method validates if InspResults has been entered when the Inspection Data is allowed for the current OprSeq.
        /// </summary>
        /// <param name="ipJobNum">Current Job</param>
        /// <param name="ipAssemblySeq"> Current AssembleSeq </param>
        /// <param name="ipOprSeq"> Current OprSeq</param>
        /// <param name="inspectionOK">Returns true if InspResults records are found</param>
        public void CheckInspResults(string ipJobNum, int ipAssemblySeq, int ipOprSeq, out bool inspectionOK)
        {
            inspectionOK = false;


            if ((this.ExistsInspResults(Session.CompanyID, ipJobNum, ipAssemblySeq, ipOprSeq)))
            {
                inspectionOK = true;
            }
            else
            {
                inspectionOK = false;
            }
        }

        private void checkLaborDtlChange()
        {
            if (BIttLaborDtl != null &&
                (ttLaborDtl.LaborType.Compare(BIttLaborDtl.LaborType) != 0 ||
                 ttLaborDtl.ProjectID.Compare(BIttLaborDtl.ProjectID) != 0 ||
                 ttLaborDtl.PhaseID.Compare(BIttLaborDtl.PhaseID) != 0 ||
                 ttLaborDtl.TimeTypCd.Compare(BIttLaborDtl.TimeTypCd) != 0 ||
                 ttLaborDtl.JobNum.Compare(BIttLaborDtl.JobNum) != 0 ||
                 ttLaborDtl.AssemblySeq != BIttLaborDtl.AssemblySeq ||
                 ttLaborDtl.OprSeq != BIttLaborDtl.OprSeq ||
                 ttLaborDtl.IndirectCode.Compare(BIttLaborDtl.IndirectCode) != 0 ||
                 ttLaborDtl.RoleCd.Compare(BIttLaborDtl.RoleCd) != 0 ||
                 ttLaborDtl.ResourceGrpID.Compare(BIttLaborDtl.ResourceGrpID) != 0 ||
                 ttLaborDtl.ResourceID.Compare(BIttLaborDtl.ResourceID) != 0 ||
                 ttLaborDtl.ExpenseCode.Compare(BIttLaborDtl.ExpenseCode) != 0 ||
                 ttLaborDtl.Shift != BIttLaborDtl.Shift))
            {
                DelFlag = true;
                DelEmployeeNum = BIttLaborDtl.EmployeeNum;
                DelWeekBeginDate = BIttLaborDtl.PayrollDate.Value.AddDays(1 - Convert.ToInt32(ttLaborDtl.PayrollDate.Value.DayOfWeek + 1));
                DelWeekEndDate = BIttLaborDtl.PayrollDate.Value.AddDays(7 - Convert.ToInt32(ttLaborDtl.PayrollDate.Value.DayOfWeek + 1));
                DelLaborType = BIttLaborDtl.LaborType;
                DelLaborTypePseudo = BIttLaborDtl.LaborTypePseudo;
                DelProjectID = BIttLaborDtl.ProjectID;
                DelPhaseID = BIttLaborDtl.PhaseID;
                DelTimeTypCd = BIttLaborDtl.TimeTypCd;
                DelJobNum = BIttLaborDtl.JobNum;
                DelAssemblySeq = BIttLaborDtl.AssemblySeq;
                DelOprSeq = BIttLaborDtl.OprSeq;
                DelIndirectCode = BIttLaborDtl.IndirectCode;
                DelRoleCd = BIttLaborDtl.RoleCd;
                DelResourceGrpID = BIttLaborDtl.ResourceGrpID;
                DelResourceID = BIttLaborDtl.ResourceID;
                DelExpenseCode = BIttLaborDtl.ExpenseCode;
                DelShift = BIttLaborDtl.Shift;
                DelStatus = BIttLaborDtl.TimeStatus;
                DelQuickEntryCode = BIttLaborDtl.QuickEntryCode;
            }
        }

        /// <summary>
        /// This method checks to see if the new resource is in the current resource group.
        /// This needs to be run right before the DefaultResourceID.  If the user answers
        /// okay then the group will be changed in the DefaultResourceID method.  
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ProposedResourceID">Proposed Resource ID</param>
        /// <param name="vMessage">List of error warnings for user</param>
        public void CheckResourceGroup(ref LaborTableset ds, string ProposedResourceID, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;
            if (String.IsNullOrEmpty(ProposedResourceID))
            {
                return;
            }



            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where ttLaborDtl_Row.RowMod.Compare("U") == 0 || ttLaborDtl_Row.RowMod.Compare("A") == 0
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                ExceptionManager.AddBLException(Strings.NoLaborDtlRecordAvailable, "LaborDtl");
            }



            Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, ProposedResourceID);
            if (Resource == null)
            {
                throw new BLException(Strings.InvalidResource, "LaborDtl", "ResourceID");
            }
            if (Resource.Inactive == true)
            {
                throw new BLException(Strings.TheResouIsInactAndMayNotBeSelec, "LaborDtl", "ResourceID");
            }
            if (Resource.Location == false)
            {
                throw new BLException(Strings.TheResouIsNotALocatAndMayNotBeSelec(Resource.ResourceID), "LaborDtl", "ResourceID");
            }
            if (Resource.ResourceGrpID.Compare(ttLaborDtl.ResourceGrpID) != 0)
            {
                vMessage = Strings.ResouBelongsToAnotherResouGroupOKToSwitchTheResou(Resource.ResourceID);
            }
            else
            {
                vMessage = "";
            }
        }

        /// <summary>
        /// This method runs the labor warning routine and returns any warnings the user needs
        /// to be aware of.  This needs to be run right before the update method.  If the user answers
        /// okay to all of the questions, then the update method can be run.  Otherwise the labor record
        /// needs to be corrected
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="vMessage">List of error warnings for user</param>
        public void CheckWarnings(ref LaborTableset ds, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where ttLaborDtl_Row.RowMod.Compare("U") == 0 || ttLaborDtl_Row.RowMod.Compare("A") == 0
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                if (ds.LaborDtlAction.Count > 0)
                {
                    return;
                }
                ExceptionManager.AddBLException(Strings.NoLaborDtlRecordAvailable, "LaborDtl");
            }
            /* set midnight to 24 for calculations */
            if (ttLaborDtl.ClockOutTime == 0)
            {
                ttLaborDtl.ClockOutTime = 24.0m;
            }

            if (ttLaborDtl.ActiveTrans)
            {
                if (ttLaborDtl.EndActivity == false)
                {
                    return;
                }
                else if (ttLaborDtl.EndActivity == true && ttLaborDtl.LaborType.Compare("I") != 0)
                {


                    LaborHed = this.FindFirstLaborHed2(Session.CompanyID, ttLaborDtl.LaborHedSeq);


                    JCShift = this.FindFirstJCShift(Session.CompanyID, LaborHed.Shift);
                    this.validateWcCode(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.ResourceGrpID, "LaborDtl", ttLaborDtl.SysRowID);
                    this.clockOutTimeMES();
                    this.calcLaborHours();



                    JobOper = this.FindFirstJobOper6(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                    if ((ttLaborDtl.LaborType.Compare("P") == 0 /* production */ &&
                         ttLaborDtl.Complete) ||
                        (ttLaborDtl.LaborType.Compare("S") == 0 /* setup */ &&
                         ttLaborDtl.Complete &&
                         JobOper.EstProdHours == 0))
                    {
                        ttLaborDtl.OpComplete = true;
                    }

                    this.calcBurdenHours();
                    this.warnHrs(out vMessage);
                }
            }
            else
            {
                this.payHoursDtl(false, false, true, out vMessage);    /*should call from Mes*/
            }

            this.warnLabor(ref vMessage);    /* set midnight back to 0 for UI */
            if (ttLaborDtl.ClockOutTime == 24.0m)
            {
                ttLaborDtl.ClockOutTime = 0;
            }

            /*SCR 89457  Add validation if the ResourceGrp has changed when saving the Labor*/
            if (!String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID))
            {
                this.chgWcCode(ttLaborDtl.ResourceGrpID, false, ref vMessage);
            }                  /* project */

            /* Check for 130 Scrap Shop Warning */
            if (ttLaborDtl.ScrapQty > 0)
            {
                this.warnScrapReasonCode(ref vMessage);
            }
            if (vMessage.Length > 0)
            {
                vMessage = vMessage + " " + Strings.DoYouWantToContinue;
            }
            ds = CurrentFullTableset;
        }

        /// <summary>
        /// This method is run when the WCCode changes.  Defaults dataset fields as needed
        /// </summary>
        /// <param name="wcCode">New WcCode value </param>
        /// <param name="lResetResourceID">Reset Resource ID value </param>
        /// <param name="vMessage">Returns any warning the User needs to be aware of</param>
        private void chgWcCode(string wcCode, bool lResetResourceID, ref string vMessage)
        {
            string vTxt = string.Empty;
            string dspWrnStr = string.Empty;
            string tmpCode = string.Empty;
            /*SCR 89457  This assignations have been moved to DefaultOprSeq, this procedure will only check for changes in the ResourceGrp*/
            //this.validateWcCode(wcCode);
            //ttLaborDtl.ResourceGrpDescription = ((ResourceGroup != null) ? ResourceGroup.Description : "");
            //ttLaborDtl.JCDept = ResourceGroup.JCDept;
            //this.validateJCDept(ttLaborDtl.JCDept);
            /* set machine id if ResourceGroup has changed */
            this.getBeforeInfo();
            if (oldWcCode.Compare(wcCode) != 0 &&
            lResetResourceID == true)
            {
                ttLaborDtl.ResourceID = "";
                if (ttLaborDtl.LaborType.Compare("I") == 0)
                {


                    EmpBasic = this.FindFirstEmpBasic(Session.CompanyID, ttLaborDtl.EmployeeNum);
                    if (EmpBasic != null && EmpBasic.ResourceGrpID.Compare(wcCode) == 0)
                    {
                        ttLaborDtl.ResourceID = EmpBasic.ResourceID;
                    }
                }
                if (ttLaborDtl.LaborType.Compare("I") != 0 || String.IsNullOrEmpty(ttLaborDtl.ResourceID))
                {


                    Resource = this.FindFirstResource(Session.CompanyID, wcCode, false);
                    if (Resource != null)
                    {
                        ttLaborDtl.ResourceID = Resource.ResourceID;
                    }
                }
            }
            var outBurdenRate = ttLaborDtl.BurdenRate;
            this.getLaborDtlBurdenRates(lResetResourceID, out outBurdenRate);
            ttLaborDtl.BurdenRate = outBurdenRate;
            /* indirect */
            if (ttJCSyst != null && ttJCSyst.GenLaborWarnMsg && ttLaborDtl.LaborType.Compare("I") != 0)
            {
                VJobNum = ttLaborDtl.JobNum;
                VAssemblySeq = ttLaborDtl.AssemblySeq;
                VOprSeq = ttLaborDtl.OprSeq;
                VWCCode = wcCode;
                VEmployeeNum = ttLaborDtl.EmployeeNum;
                DispWarn = true;
                FromProg = "LabEnt";
                dspWrnStr = ((DispWarn) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));
                /* include uses tt value so the new value must temporarily be assigned to the tt */
                tmpCode = ttLaborDtl.ResourceGrpID;
                ttLaborDtl.ResourceGrpID = wcCode;        /* workcenter code changed */
                LibWARNDEF.RunDEWRN100(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.ResourceGrpID, out VlabWarnNum, out VVariancePct);
                ttLaborDtl.ResourceGrpID = tmpCode;
                if (VlabWarnNum > 0)
                {
                    vTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + dspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(100, vTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        vMessage = vMessage + (String.IsNullOrEmpty(vMessage) ? "" : " ") + VContinue;
                    }
                }
            }
            LaborDtl_Foreign_Link();
        }

        /// <summary>
        /// This method checks Labor Qty and runs any labor warnings for quantity
        /// </summary>
        /// <param name="laborQty">new laborqty value </param>
        /// <param name="vMessage">Returns any warnings the user needs to be aware of</param>
        private void chkLaborQty(decimal laborQty, out string vMessage)
        {
            vMessage = string.Empty;
            bool negQty = false;
            bool chgJob = false;

            this.getBeforeInfo();
            if (oldJobNum.Compare(ttLaborDtl.JobNum) != 0 ||
                oldAssSeq != ttLaborDtl.AssemblySeq ||
                oldOprSeq != ttLaborDtl.OprSeq)
            {
                chgJob = true;
            }


            JobOper = this.FindFirstJobOper8(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
            switch (ttLaborDtl.LaborType.ToUpperInvariant())
            {
                case "I":
                    {
                        if (laborQty < 0)
                        {
                            negQty = true;
                        }
                    }
                    break;
                case "P":
                    {
                        if (chgJob == true)
                        {
                            if (JobOper != null && (JobOper.QtyCompleted + laborQty) < 0)
                            {
                                negQty = true;
                            }
                        }
                        else
                        {
                            if (JobOper != null && ((oldJobQty - oldLbrQty) + laborQty) < 0)
                            {
                                negQty = true;
                            }
                        }
                    }
                    break;
                case "S":
                    {
                        if (chgJob == true)
                        {
                            if (JobOper != null && (JobOper.QtyCompleted + laborQty) < 0)
                            {
                                negQty = true;
                            }
                        }
                        else
                        {
                            if (JobOper != null && ((oldJobQty - oldLbrQty) + laborQty) < 0)
                            {
                                negQty = true;
                            }
                        }
                    }
                    break;
            }
            if (negQty == true && !ttLaborDtl.ReWork)
            {
                throw new BLException(Strings.TheQuantEnteredWillResultInANegatTotalQuantPosted);
            }
            /* CHECK FOR AND GENERATE LABOR WARNINGS - 7486 */
            this.warnQty(laborQty, out vMessage);

            if (!ttLaborDtl.ReWork)
            {
                if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.EnhancedQualityAssurance))
                {
                    if ((this.ExistsJobOperInsp(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq)) && ttLaborDtl.ReWork == false)
                    {
                        ttLaborDtl.EnableInspection = true;
                    }
                    else
                    {
                        ttLaborDtl.EnableInspection = false;
                    }
                }
            }
            else
            {
                ttLaborDtl.EnableInspection = false;
            }
        }

        /// <summary>
        /// This method checks for negative hours
        /// </summary>
        private void chkNegHrs()
        {
            bool negHrs = false;
            bool negRwkHrs = false;
            bool chgJob = false;
            if (oldJobNum.Compare(ttLaborDtl.JobNum) != 0 ||
                oldAssSeq != ttLaborDtl.AssemblySeq ||
                oldOprSeq != ttLaborDtl.OprSeq)
            {
                chgJob = true;
            }



            JobOper = this.FindFirstJobOper8(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
            switch (ttLaborDtl.LaborType.ToUpperInvariant())
            {
                case "I":
                    {
                        if (ttLaborDtl.BurdenHrs < 0)
                        {
                            negHrs = true;
                        }
                    }
                    break;
                case "P":
                    {
                        if (chgJob == true)
                        {
                            if (JobOper != null && (JobOper.ActProdHours + ttLaborDtl.BurdenHrs) < 0)
                            {
                                negHrs = true;
                            }

                            if (JobOper != null && negHrs == false && ttLaborDtl.ReWork && (JobOper.ActProdHours + ttLaborDtl.BurdenHrs) < 0)
                            {
                                negRwkHrs = true;
                            }
                        }
                        else
                        {
                            if (JobOper != null && (saveActProdHours - saveBurdenHrs) + ttLaborDtl.BurdenHrs < 0)
                            {
                                negHrs = true;
                            }

                            if (JobOper != null && negHrs == false && ttLaborDtl.ReWork == true && (saveActProdRwkHours - saveBurdenHrs) + ttLaborDtl.BurdenHrs < 0)
                            {
                                negRwkHrs = true;
                            }
                        }
                    }
                    break;
                case "S":
                    {
                        if (chgJob == true)
                        {
                            if (JobOper != null && (JobOper.ActSetupHours + ttLaborDtl.BurdenHrs) < 0)
                            {
                                negHrs = true;
                            }

                            if (JobOper != null && negHrs == false && ttLaborDtl.ReWork == true && (JobOper.ActSetupRwkHours + ttLaborDtl.BurdenHrs) < 0)
                            {
                                negRwkHrs = true;
                            }
                        }
                        else
                        {
                            if (JobOper != null && (saveActSetupHours - saveBurdenHrs) + ttLaborDtl.BurdenHrs < 0)
                            {
                                negHrs = true;
                            }

                            if (JobOper != null && negHrs == false && ttLaborDtl.ReWork == true && (saveActSetupHours - saveBurdenHrs) + ttLaborDtl.BurdenHrs < 0)
                            {
                                negRwkHrs = true;
                            }
                        }
                    }
                    break;
            }
            if (negHrs == true)
            {
                ExceptionManager.AddBLException(Strings.TheHoursEnteredWillResultInANegatTotalHoursPosted);
            }

            if (negRwkHrs == true)
            {
                ExceptionManager.AddBLException(Strings.TheHoursEnteredWillResultInANegatTotalReworkHours);
            }

            ExceptionManager.AssertNoBLExceptions();
        }

        /// <summary>
        /// Check Labor Part Quantity with Labor Quantity
        /// </summary>
        private void chkPartQty()
        {
            decimal dTempQty = decimal.Zero;
            decimal dTempScrapQty = decimal.Zero;
            decimal dTempDiscrepQty = decimal.Zero;
            decimal convQty = decimal.Zero;

            JobHead = this.FindFirstJobHead3(ttLaborDtl.Company, ttLaborDtl.JobNum);
            if (JobHead != null && JobHead.ProcessMode.Compare("C") == 0)
            {
                return;
            }

            if ((this.ExistsLaborPart(Session.CompanyID, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq)))
            {
                string invUM = string.Empty;


                foreach (var LaborPart_iterator in (this.SelectLaborPart3(Session.CompanyID, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq)))
                {
                    LaborPart = LaborPart_iterator;


                    invUM = FindFirstJobPartIUM(Session.CompanyID, ttLaborDtl.JobNum, LaborPart.PartNum);

                    if (string.IsNullOrEmpty(invUM))
                        invUM = FindFirstPartIUM(Session.CompanyID, LaborPart.PartNum);

                    if (LaborPart.ScrapQty > 0)
                    {
                        if (!this.ExistsScrapCode(Session.CompanyID, LaborPart.ScrapReasonCode))
                        {
                            throw new BLException(Strings.InvalidScrapReasonCode);
                        }

                        using (var libAdvancedUOMValidations = new Erp.Internal.Lib.AdvancedUOMValidations(Db))
                        {
                            libAdvancedUOMValidations.CheckAttributeSetIsValidForPart(LaborPart.ScrapAttributeSetID, true, LaborPart.PartNum);
                        }
                    }

                    if (LaborPart.DiscrepQty > 0)
                    {
                        if (!this.ExistsScrapCode(Session.CompanyID, LaborPart.DiscrpRsnCode))
                        {
                            throw new BLException(Strings.InvalidDiscrepancyReasonCode);
                        }

                        using (var libAdvancedUOMValidations = new Erp.Internal.Lib.AdvancedUOMValidations(Db))
                        {
                            libAdvancedUOMValidations.CheckAttributeSetIsValidForPart(LaborPart.DiscrpAttributeSetID, true, LaborPart.PartNum);
                        }

                    }

                    if (LaborPart.PartQty > 0)
                    {
                        using (var libAdvancedUOMValidations = new Erp.Internal.Lib.AdvancedUOMValidations(Db))
                        {
                            libAdvancedUOMValidations.CheckAttributeSetIsValidForPart(LaborPart.LaborAttributeSetID, true, LaborPart.PartNum);
                        }

                    }

                    if (LaborPart.ScrapQty < 0)
                    {
                        throw new BLException(Strings.ScrapQuantityCannotBeNegative);
                    }

                    if (LaborPart.DiscrepQty < 0)
                    {
                        throw new BLException(Strings.NonConformQuantCannotBeNegat);
                    }

                    JobOper = this.FindFirstJobOper9(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                    LibAppService.UOMConv(LaborPart.PartNum, LaborPart.PartQty, invUM, JobHead.IUM, out convQty, false);
                    dTempQty = dTempQty + convQty;
                    LibAppService.UOMConv(LaborPart.PartNum, LaborPart.ScrapQty, invUM, JobHead.IUM, out convQty, false);
                    dTempScrapQty = dTempScrapQty + convQty;
                    LibAppService.UOMConv(LaborPart.PartNum, LaborPart.DiscrepQty, invUM, JobHead.IUM, out convQty, false);
                    dTempDiscrepQty = dTempDiscrepQty + convQty;

                }

                if (ttLaborDtl.LaborEntryMethod.Compare("X") == 0 && ttLaborDtl.LaborQty + ttLaborDtl.ScrapQty + ttLaborDtl.DiscrepQty != 0)
                {
                    throw new BLException(Strings.ReportQtyNotAllowedForTimeAndBackflushOperations, "LaborDtl");
                }

                if (ttLaborDtl.LaborQty != dTempQty)
                {
                    throw new BLException(Strings.TheTotalCoPartsProduQuantAndLaborQuantDoNotBalance, "LaborPart");
                }

                if (ttLaborDtl.ScrapQty != dTempScrapQty)
                {
                    throw new BLException(Strings.TheTotalCoPartsScrapAndLaborQuantDoNotBalance, "LaborPart");
                }

                if (ttLaborDtl.DiscrepQty != dTempDiscrepQty)
                {
                    throw new BLException(Strings.TheTotalCoPartsDiscrepQuantAndLaborQuantDoNotBalance, "LaborPart");
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// assumes ttLaborDtl record is available.
        /// </remarks>
        private void chkShopWarn(out string pcMsg)
        {
            pcMsg = string.Empty;
            string VTxt = string.Empty;
            string DspWrnStr = "1";
            string cTmp = string.Empty;
            if (ttJCSyst.GenLaborWarnMsg && ttLaborDtl.LaborType.Compare("I") != 0)
            {
                VJobNum = ttLaborDtl.JobNum;
                VAssemblySeq = ttLaborDtl.AssemblySeq;
                VOprSeq = ttLaborDtl.OprSeq;
                VWCCode = ttLaborDtl.ResourceGrpID;
                VEmployeeNum = ttLaborDtl.EmployeeNum;
                DispWarn = true;
                FromProg = "DataColl";
                DspWrnStr = ((DispWarn) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));
                /* PREVIOUS OPERATION NOT STARTED */
                LibWARNDEF.RunDEWRN010(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(10, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
                /* PREVIOUS OPERATION NOT Completed */
                LibWARNDEF.RunDEWRN020(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(20, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
                /* OPERATION ALREADY COMPLETE */
                LibWARNDEF.RunDEWRN030(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(30, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
                /* OPERATION SETUP NOT COMPLETE */
                LibWARNDEF.RunDEWRN040(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(40, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
                /* OPERATION BEHIND SCHEDULE */
                LibWARNDEF.RunDEWRN050(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(50, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
            }
            /* production */
            if (ttLaborDtl.LaborType.Compare("P") == 0)
            {
                LibWARNDEF.RunDEWRN200(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.ResourceID, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(200, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
            }
            if (ttLaborDtl.LaborType.Compare("S") == 0 /* setup */ && (JobOper != null && JobOper.FAQty > 0))
            {
                LibWARNDEF.RunDEWRN210(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.ResourceID, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(210, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
            }
            /* production */
            if (ttLaborDtl.LaborType.Compare("P") == 0)
            {
                LibWARNDEF.RunDEWRN220(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.ResourceID, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(220, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                    if (ttJCSyst.PreventFABypass == true)
                    {
                        cTmp = Strings.UnableToStartProduUntilFirstArticleInspeHasBeen;
                        throw new BLException(cTmp);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="company">Company ID in ReportQty record</param>
        /// <param name="jobNum">Job Number in ReportQty record</param>
        /// <param name="assemblySeq">Assembly Sequence Number in ReportQty record</param>
        /// <param name="oprSeq">Operation Sequence Number in ReportQty record</param>
        /// <param name="empID">Employee ID in ReportQty record</param>
        /// <param name="activeTrans">Active Transaction FLAG in ReportQty record</param>
        /// <param name="pcMsg">returns WARNING messages</param>
        /// <remarks>
        /// sets a ttLaborDtl record based on parameters.
        /// </remarks>
        public void chkReportQtyShopWarn(string company, string jobNum, int assemblySeq, int oprSeq, string empID, bool activeTrans, out string pcMsg)
        {
            //Initialize ttLaborDtl * JCSyst based on parameters.
            JCSyst ttJCSyst = new JCSyst();
            Erp.Tables.JCSyst JCSyst = this.FindFirstJCSyst(Session.CompanyID);
            if (JCSyst != null)
            {
                BufferCopy.Copy(JCSyst, ref ttJCSyst);
            }

            Erp.Tables.LaborDtl LaborDtl = this.FindFirstLaborDtl(company, jobNum, assemblySeq, oprSeq, empID, activeTrans);
            if (LaborDtl != null)
            {
                BufferCopy.Copy(LaborDtl, ref ttLaborDtl);
            }
            else
            {
                ttLaborDtl = new LaborDtlRow();
                ttLaborDtl.Company = company;
                ttLaborDtl.JobNum = jobNum;
                ttLaborDtl.AssemblySeq = assemblySeq;
                ttLaborDtl.OprSeq = oprSeq;
                ttLaborDtl.EmployeeNum = empID;
                ttLaborDtl.ActiveTrans = false;
                ttLaborDtl.LaborType = "B";
                ttLaborDtl.ResourceGrpID = "";
                ttLaborDtl.LaborHedSeq = 0;
                ttLaborDtl.LaborDtlSeq = 0;
            }

            //THEN CALL the chkStartActShopWarn() method.
            using (ErpCallContext.SetDisposableKey("CheckReportQtyShopWarns"))
            {
                chkStartActShopWarn(out pcMsg);
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// assumes ttLaborDtl record is available.
        /// </remarks>
        private void chkStartActShopWarn(out string pcMsg)
        {
            pcMsg = string.Empty;
            string VTxt = string.Empty;
            string DspWrnStr = "1";
            string cTmp = string.Empty;
            if (ttJCSyst.GenLaborWarnMsg && ttLaborDtl.LaborType.Compare("I") != 0)
            {
                VJobNum = ttLaborDtl.JobNum;
                VAssemblySeq = ttLaborDtl.AssemblySeq;
                VOprSeq = ttLaborDtl.OprSeq;
                VWCCode = ttLaborDtl.ResourceGrpID;
                VEmployeeNum = ttLaborDtl.EmployeeNum;
                DispWarn = true;
                FromProg = "DataColl";
                DspWrnStr = ((DispWarn) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));
                /* PREVIOUS OPERATION NOT STARTED */
                LibWARNDEF.RunDEWRN010(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(10, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
                else /* check if NOT COMPLETED (VlabWarnNum = 020) only if is already STARTED (VlabWarnNum != 010) */
                {
                    /* PREVIOUS OPERATION NOT Completed */
                    LibWARNDEF.RunDEWRN020(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, out VlabWarnNum, out VVariancePct);
                    if (VlabWarnNum > 0)
                    {
                        VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                        LibXAP05._XAP05(20, VTxt, out VContinue);
                        if (!String.IsNullOrEmpty(VContinue))
                        {
                            pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                        }
                    }
                }
                /* OPERATION ALREADY COMPLETE */
                LibWARNDEF.RunDEWRN030(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(30, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
                /* OPERATION SETUP NOT COMPLETE */
                LibWARNDEF.RunDEWRN040(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(40, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
                /* OPERATION BEHIND SCHEDULE */
                LibWARNDEF.RunDEWRN050(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    VTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(50, VTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        pcMsg = pcMsg + ((String.IsNullOrEmpty(pcMsg)) ? "" : Ice.Constants.LIST_DELIM) + VContinue;
                    }
                }
            }
        }

        /// <summary>
        /// This method clears the buffer when LaborType changes
        /// </summary>
        private void clearRecordBuffer()
        { /* production */
            if (ttLaborDtl.LaborType.Compare("P") == 0)
            {
                ttLaborDtl.SetupPctComplete = 0;
                ttLaborDtl.IndirectCode = "";
            }
            /* setup */
            if (ttLaborDtl.LaborType.Compare("S") == 0)
            {
                ttLaborDtl.IndirectCode = "";
            }

            if (ttLaborDtl.LaborType.Compare("I") == 0)
            { /* indirect */
                ttLaborDtl.JobNum = "";
                ttLaborDtl.AssemblySeq = 0;
                ttLaborDtl.OprSeq = 0;
                ttLaborDtl.OpCode = "";
                ttLaborDtl.LaborQty = 0;
                ttLaborDtl.ScrapQty = 0;
                ttLaborDtl.DiscrepQty = 0;
                ttLaborDtl.SetupPctComplete = 0;
                ttLaborDtl.ScrapReasonCode = "";
                ttLaborDtl.ReworkReasonCode = "";
                ttLaborDtl.DiscrpRsnCode = "";
                ttLaborDtl.BurdenHrs = 0;
                ttLaborDtl.BurdenRate = 0;
                ttLaborDtl.ReWork = false;
                ttLaborDtl.Complete = false;
                ttLaborDtl.OpComplete = false;
                ttLaborDtl.CallNum = 0;
                ttLaborDtl.CallLine = 0;
                ttLaborDtl.JobType = "";
            }
            if (ttLaborDtl.JobTypeCode.Compare("MNT") == 0)
            {
                ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
            }

            if (ttLaborDtl.LaborType.Compare("I") != 0 && ttLaborDtl.ScrapQty == 0)
            {
                ttLaborDtl.ScrapReasonCode = "";
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 1) assumes ttLaborDtl is available
        /// 2) it seems odd that ttLaborDtl.ClockOutMinute is set, but that's 
        /// way it was in v6 code... note ClockOutTime is not set.
        /// </remarks>
        private void clockInTimeMES()
        {
            int V_HdrClockInMinute = 0;
            decimal HrsMin = decimal.Zero;
            decimal HrsDec = decimal.Zero;
            string HrMinTime = string.Empty;
            int CurrTime = 0;
            int CurrSec = 0;
            int CurrMinute = 0;
            int CurrHour = 0;


            LaborHed = this.FindFirstLaborHed3(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq);
            if (LibOffset.OffsetToday() == null)
            {
                ttLaborDtl.ClockInDate = null;
            }
            else
            {
                ttLaborDtl.ClockInDate = LibOffset.OffsetToday().Value.Date;
            }

            ttLaborDtl.ClockinTime = LibOffset.OffSetTime();
            CurrTime = LibOffset.OffSetTime();
            /* break out seconds */
            CurrSec = (CurrTime % 60);
            CurrTime = (CurrTime - CurrSec) / 60;
            /* break out minutes */
            CurrMinute = (CurrTime % 60);
            /* break out hour */
            CurrHour = (CurrTime - CurrMinute) / 60;
            /* concatenate the two values */
            HrMinTime = Compatibility.Convert.ToString(CurrHour, "99") + Compatibility.Convert.ToString(CurrMinute, "99");
            /* convert it to a decimal value */
            HrsMin = Compatibility.Convert.ToDecimal(HrMinTime);
            /* convert to 2 decimal places */
            HrsMin = HrsMin / 100;
            /* convert to hours/hundreths */
            HrsDec = Math.Truncate(HrsMin) + ((HrsMin - Math.Truncate(HrsMin)) * 100) / 60m;
            ttLaborDtl.ClockinTime = HrsDec;
            /* convert input date and time to minutes since base date */
            ttLaborDtl.ClockInMInute = (((TimeSpan)(ttLaborDtl.ClockInDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(ttLaborDtl.ClockinTime * 60);
            ttLaborDtl.ClockOutMinute = (ttLaborDtl.ClockInMInute + 1440);
            /* Adjust the labordtl clock in time and date to be same as LaborHed clock in time and date
               if clock in is within grace. */
            JCShiftResult JCShift = this.FindFirstJCShift2(Session.CompanyID, LaborHed.Shift);
            if (JCShift != null)
            {
                V_HdrClockInMinute = (((TimeSpan)(LaborHed.ClockInDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(LaborHed.ClockInTime * 60);
                if ((ttJCSyst.DetailGrace && ((LaborHed.ClockInTime == JCShift.StartTime)
                && (ttLaborDtl.ClockInMInute - V_HdrClockInMinute) <= ttJCSyst.LateClockInAllowance)))
                {
                    ttLaborDtl.ClockinTime = LaborHed.ClockInTime;
                    ttLaborDtl.DspClockInTime = LaborHed.DspClockInTime;
                    ttLaborDtl.ClockInDate = LaborHed.ClockInDate;
                    ttLaborDtl.ClockInMInute = (((TimeSpan)(ttLaborDtl.ClockInDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(ttLaborDtl.ClockinTime * 60);
                    ttLaborDtl.ClockOutMinute = (ttLaborDtl.ClockInMInute + 1440);
                }
            }
        }

        private void clockOutTimeMES()
        {
            int CurrTime = 0;
            int CurrHour = 0;
            int CurrMinute = 0;
            int CurrSec = 0;
            decimal HrsMin = decimal.Zero;
            decimal HrsDec = decimal.Zero;
            string HrMinTime = string.Empty;
            int SaveSysTime = 0;
            DateTime? SaveSysDate = null;
            SaveSysTime = LibOffset.OffSetTime();
            SaveSysDate = LibOffset.OffsetToday();
            CurrTime = SaveSysTime;

            /* break out seconds */
            CurrSec = (CurrTime % 60);
            CurrTime = (CurrTime - CurrSec) / 60;
            /* break out minutes */
            CurrMinute = (CurrTime % 60);
            /* break out hour */
            CurrHour = (CurrTime - CurrMinute) / 60;
            /* concatenate the two values */
            HrMinTime = Compatibility.Convert.ToString(CurrHour, "99") + Compatibility.Convert.ToString(CurrMinute, "99");
            /* convert it to a decimal value */
            HrsMin = Compatibility.Convert.ToDecimal(HrMinTime);
            /* convert to 2 decimal places */
            HrsMin = HrsMin / 100;
            /* convert to hours/hundreths */
            HrsDec = Math.Truncate(HrsMin) + ((HrsMin - Math.Truncate(HrsMin)) * 100) / 60m;
            ttLaborDtl.ClockOutTime = HrsDec;
            /* Convert time to seconds */
            CurrTime = CurrTime * 60;
            this.adjustForGracePeriod(SaveSysDate, SaveSysTime, ref CurrTime);
            /* Convert Time back into minutes */
            CurrTime = CurrTime / 60;
        }

        /// <summary>
        /// Common validation code for PhaseOprSeq/OprSeq
        /// </summary>
        private void commonOprSeq(string ipEmployeeNum, string ipJobNum, int ipAssemblySeq, int ipPhaseOprSeq, out string opRoleCd)
        {
            opRoleCd = string.Empty;
            bool chkEmpPrjRoleAUX = false;
            Erp.Tables.JobHead altJobHead = null;
            string projectID = string.Empty;
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling))
            {


                altJobHead = this.FindFirstJobHead4(Session.CompanyID, ipJobNum);
                if (altJobHead == null)
                {
                    return;
                }

                projectID = altJobHead.ProjectID;
                /* no default if no project or project is not TM, CP, PP, or FF  */
                LibProjectCommon.getWBSPhaseMethods(altJobHead.ProjectID, altJobHead.PhaseID, out vInvMethod, out vRevMethod);
                if ("TM,CP,PP,FF".Lookup(vInvMethod) > -1)
                {
                    opRoleCd = this.getRoleCodeDefault(ipEmployeeNum, ipJobNum, ipAssemblySeq, ipPhaseOprSeq, projectID, altJobHead.PhaseID);
                    if (String.IsNullOrEmpty(opRoleCd))
                    {
                        chkEmpPrjRoleAUX = this.getchkEmpPrjRole(projectID, altJobHead.PhaseID);
                        if (chkEmpPrjRoleAUX == true)
                        {
                            throw new BLException(Strings.ThisEmploIsNotAssigToAnyRoleCodeRequiByThisJob, "LaborHed", "EmployeeNum");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method to copy the vales from one Labor record to a new Labor record. 
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="cMessageText">Message text to present to the user after the process is finished </param>
        public void CopyLaborDetail(ref LaborTableset ds, out string cMessageText)
        {
            cMessageText = string.Empty;
            CurrentFullTableset = ds;
            int i = 0;
            Erp.Tablesets.LaborDtlRow bttLaborDtl = null;


            foreach (var bttLaborDtl_iterator in (from bttLaborDtl_Row in CurrentFullTableset.LaborDtl
                                                  where !String.IsNullOrEmpty(bttLaborDtl_Row.RowMod)
                                                  select bttLaborDtl_Row))
            {
                bttLaborDtl = bttLaborDtl_iterator;
                i = i + 1;
            }
            if (i != 1)
            {
                throw new BLException(Strings.OneRecordCanBeSelectedToCopy, "LaborDtl", "Company");
            }
            else
            {


                bttLaborDtl = (from bttLaborDtl_Row in ds.LaborDtl
                               where !String.IsNullOrEmpty(bttLaborDtl_Row.RowMod)
                               select bttLaborDtl_Row).FirstOrDefault();
                CurrentFullTableset = ds;

                if (bttLaborDtl.ClockInDate != null)
                {
                    eadErrMsg = LibEADValidation.validateEAD(bttLaborDtl.ClockInDate, "IP", "");
                    if (!String.IsNullOrEmpty(eadErrMsg))
                    {
                        throw new BLException(eadErrMsg);
                    }
                }

                if (!ValidateProjectClosedRecallCopy(bttLaborDtl.ProjectID, bttLaborDtl.JobNum, bttLaborDtl.LaborTypePseudo))
                {
                    throw new BLException(Strings.ProjectClosed(bttLaborDtl.ProjectID));
                }

                ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
                CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
                BufferCopy.CopyExceptFor(bttLaborDtl, ttLaborDtl, "SysRowID", "LaborDtlSeq", "TimeDisableDelete", "RowMod", "TimeDisableUpdate", "RowSelected", "SysRevID");
                ttLaborDtl.RowMod = "A";
                ttLaborDtl.TimeStatus = "E";
                bttLaborDtl.RowMod = "";
                this.LaborDtlSetDefaults(ttLaborDtl);

                Erp.Tables.JobPart tmpJobPart = null;
                Erp.Tables.LaborPart tmpLaborPart = null;
                Erp.Tables.Part altPart = null;
                bool SetPartQtyToZero = false;

                SetPartQtyToZero = ExistsJobPartChanges(Session.CompanyID, bttLaborDtl.LaborHedSeq, bttLaborDtl.LaborDtlSeq);

                foreach (var JobPart_iterator in (this.SelectJobPart(Session.CompanyID, Session.PlantID, ttLaborDtl.JobNum)))
                {
                    tmpJobPart = JobPart_iterator;
                    tmpLaborPart = this.FindFirstLaborPart(Session.CompanyID, tmpJobPart.PartNum, bttLaborDtl.LaborHedSeq, bttLaborDtl.LaborDtlSeq);
                    altPart = this.FindFirstPart(Session.CompanyID, tmpJobPart.PartNum);

                    ttLaborPart = new Erp.Tablesets.LaborPartRow();
                    CurrentFullTableset.LaborPart.Add(ttLaborPart);
                    ttLaborPart.Company = tmpJobPart.Company;
                    ttLaborPart.LaborHedSeq = ttLaborDtl.LaborHedSeq;
                    ttLaborPart.LaborDtlSeq = ttLaborDtl.LaborDtlSeq;
                    ttLaborPart.PartNum = tmpJobPart.PartNum;
                    ttLaborPart.LaborAttributeSetID = tmpJobPart.AttributeSetID;
                    ttLaborPart.PartUOM = tmpJobPart.IUM;
                    ttLaborPart.ScrapUOM = tmpJobPart.IUM;
                    ttLaborPart.DiscrepUOM = tmpJobPart.IUM;
                    ttLaborPart.RevisionNum = tmpJobPart.RevisionNum;
                    ttLaborPart.PartDescription = GetPartDescription(tmpJobPart, altPart);
                    ttLaborPart.PartQty = SetPartQtyToZero ? 0 : (tmpLaborPart == null) ? 0 : (tmpJobPart.PartQty == 0) ? 0 : tmpLaborPart.PartQty;
                    ttLaborPart.ScrapQty = SetPartQtyToZero ? 0 : (tmpLaborPart == null) ? 0 : tmpLaborPart.ScrapQty;
                    ttLaborPart.DiscrepQty = SetPartQtyToZero ? 0 : (tmpLaborPart == null) ? 0 : tmpLaborPart.DiscrepQty;
                    ttLaborPart.DiscrpAttributeSetID = ttLaborPart.DiscrepQty == 0 ? 0 : tmpJobPart.AttributeSetID;
                    ttLaborPart.ScrapAttributeSetID = ttLaborPart.ScrapQty == 0 ? 0 : tmpJobPart.AttributeSetID;
                    ttLaborPart.RowMod = IceRow.ROWSTATE_ADDED;
                }

                if (CurrentFullTableset.LaborPart.Count() > 1)
                    ttLaborDtl.LaborQty = 0;

                this.setComplete(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.LaborQty, ttLaborDtl.ScrapQty);
                this.LaborDtlAfterGetRows();
            }
        }

        /// <summary>
        /// Method to get the Part Description for the Labor Part.
        /// </summary>
        /// <param name="jobPart"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        private string GetPartDescription(JobPart jobPart, Part part)
        {
            if (jobPart != null)
            {
                return jobPart.PartDescription;
            }
            else if (part != null)
            {
                return part.PartDescription;
            }

            return String.Empty;
        }

        /// <summary>
        /// Method to copy the vales from one Weekly Time record to a new Weekly Time record. 
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="cMessageText">Message text to present to the user after the process is finished </param>
        public void CopyTimeWeeklyView(ref LaborTableset ds, out string cMessageText)
        {
            cMessageText = string.Empty;
            CurrentFullTableset = ds;
            int i = 0;
            Erp.Tablesets.TimeWeeklyViewRow bttTimeWeeklyView = null;


            foreach (var bttTimeWeeklyView_iterator in (from bttTimeWeeklyView_Row in CurrentFullTableset.TimeWeeklyView
                                                        where !String.IsNullOrEmpty(bttTimeWeeklyView_Row.RowMod)
                                                        select bttTimeWeeklyView_Row))
            {
                bttTimeWeeklyView = bttTimeWeeklyView_iterator;
                i = i + 1;
            }
            if (i != 1)
            {
                throw new BLException(Strings.OneRecordCanBeSelectedToCopy, "LaborDtl", "Company");
            }
            else
            {


                bttTimeWeeklyView = (from bttTimeWeeklyView_Row in ds.TimeWeeklyView
                                     where !String.IsNullOrEmpty(bttTimeWeeklyView_Row.RowMod)
                                     select bttTimeWeeklyView_Row).FirstOrDefault();
                ttTimeWeeklyView = new Erp.Tablesets.TimeWeeklyViewRow();
                CurrentFullTableset.TimeWeeklyView.Add(ttTimeWeeklyView);
                BufferCopy.CopyExceptFor(bttTimeWeeklyView, ttTimeWeeklyView, "MessageText", "TimeStatus", "SysRowID", "RowMod");
                ttTimeWeeklyView.RowMod = "A";
                ttTimeWeeklyView.NewRowType = "C";
                ttTimeWeeklyView.TimeStatus = "E";
                ttTimeWeeklyView.TimeAutoSubmit = this.IsTimeAutoSubmitPlantConfCtrl(Session.CompanyID, Session.PlantID);
                TimeWeeklyView_Foreign_Link();
                bttTimeWeeklyView.RowMod = "";
            }
        }

        private void createDelttTimeWeeklyView()
        {
            ttTimeWeeklyView = (from ttTimeWeeklyView_Row in CurrentFullTableset.TimeWeeklyView
                                where ttTimeWeeklyView_Row.Company.KeyEquals(Session.CompanyID)
                                && ttTimeWeeklyView_Row.EmployeeNum.Compare(DelEmployeeNum) == 0
                                && ttTimeWeeklyView_Row.WeekBeginDate.Value.Date == DelWeekBeginDate.Value.Date
                                && ttTimeWeeklyView_Row.WeekEndDate.Value.Date == DelWeekEndDate.Value.Date
                                && ttTimeWeeklyView_Row.LaborTypePseudo.Compare(DelLaborTypePseudo) == 0
                                && ttTimeWeeklyView_Row.ProjectID.Compare(DelProjectID) == 0
                                && ttTimeWeeklyView_Row.PhaseID.Compare(DelPhaseID) == 0
                                && ttTimeWeeklyView_Row.TimeTypCd.Compare(DelTimeTypCd) == 0
                                && ttTimeWeeklyView_Row.JobNum.Compare(DelJobNum) == 0
                                && ttTimeWeeklyView_Row.AssemblySeq == DelAssemblySeq
                                && ttTimeWeeklyView_Row.OprSeq == DelOprSeq
                                && ttTimeWeeklyView_Row.IndirectCode.Compare(DelIndirectCode) == 0
                                && ttTimeWeeklyView_Row.RoleCd.Compare(DelRoleCd) == 0
                                && ttTimeWeeklyView_Row.ResourceGrpID.Compare(DelResourceGrpID) == 0
                                && ttTimeWeeklyView_Row.ResourceID.Compare(DelResourceID) == 0
                                && ttTimeWeeklyView_Row.ExpenseCode.Compare(DelExpenseCode) == 0
                                && ttTimeWeeklyView_Row.Shift == DelShift
                                && ttTimeWeeklyView_Row.TimeStatus.Compare(DelStatus) == 0
                                && ttTimeWeeklyView_Row.QuickEntryCode.Compare(DelQuickEntryCode) == 0
                                select ttTimeWeeklyView_Row).FirstOrDefault();
            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = new Erp.Tablesets.TimeWeeklyViewRow();
                CurrentFullTableset.TimeWeeklyView.Add(ttTimeWeeklyView);
                ttTimeWeeklyView.Company = Session.CompanyID;
                ttTimeWeeklyView.EmployeeNum = DelEmployeeNum;
                if (DelWeekBeginDate == null)
                {
                    ttTimeWeeklyView.WeekBeginDate = null;
                }
                else
                {
                    ttTimeWeeklyView.WeekBeginDate = DelWeekBeginDate.Value.Date;
                }

                if (DelWeekEndDate == null)
                {
                    ttTimeWeeklyView.WeekEndDate = null;
                }
                else
                {
                    ttTimeWeeklyView.WeekEndDate = DelWeekEndDate.Value.Date;
                }

                ttTimeWeeklyView.LaborType = DelLaborType;
                ttTimeWeeklyView.LaborTypePseudo = DelLaborTypePseudo;
                ttTimeWeeklyView.ProjectID = DelProjectID;
                ttTimeWeeklyView.PhaseID = DelPhaseID;
                ttTimeWeeklyView.TimeTypCd = DelTimeTypCd;
                ttTimeWeeklyView.JobNum = DelJobNum;
                ttTimeWeeklyView.AssemblySeq = DelAssemblySeq;
                ttTimeWeeklyView.OprSeq = DelOprSeq;
                ttTimeWeeklyView.IndirectCode = DelIndirectCode;
                ttTimeWeeklyView.RoleCd = DelRoleCd;
                ttTimeWeeklyView.ResourceGrpID = DelResourceGrpID;
                ttTimeWeeklyView.ResourceID = DelResourceID;
                ttTimeWeeklyView.ExpenseCode = DelExpenseCode;
                ttTimeWeeklyView.Shift = DelShift;
                ttTimeWeeklyView.TimeStatus = DelStatus;
                ttTimeWeeklyView.QuickEntryCode = DelQuickEntryCode;
                ttTimeWeeklyView.RowMod = "D";
                TimeWeeklyView_Foreign_Link();
                delttTimeWeeklyView = new TimeWeeklyViewRow();
                BufferCopy.Copy(ttTimeWeeklyView, ref delttTimeWeeklyView);
            }
        }

        private void createFirstArt(Erp.Tables.JobOper bJobOper)
        {
            Erp.Tables.FirstArt b_FirstArt = null;
            Erp.Tables.JobHead b_JobHead = null;

            b_JobHead = this.FindFirstJobHead5(bJobOper.Company, bJobOper.JobNum);

            if (!(this.ExistsFirstArt(bJobOper.Company, "W", bJobOper.JobNum, bJobOper.AssemblySeq, bJobOper.OprSeq, ttLaborDtl.ResourceID)))
            {
                b_FirstArt = this.FindLastFirstArt(bJobOper.Company, bJobOper.JobNum, bJobOper.AssemblySeq, bJobOper.OprSeq, ttLaborDtl.ResourceID);

                FirstArt = new Erp.Tables.FirstArt();
                Db.FirstArt.Insert(FirstArt);
                FirstArt.Company = bJobOper.Company;
                FirstArt.Plant = b_JobHead.Plant;
                FirstArt.FAStatus = "W";
                FirstArt.JobNum = bJobOper.JobNum;
                FirstArt.AssemblySeq = bJobOper.AssemblySeq;
                FirstArt.OprSeq = bJobOper.OprSeq;
                FirstArt.UOMCode = bJobOper.IUM;
                FirstArt.ResourceID = ttLaborDtl.ResourceID;
                FirstArt.SeqNum = ((b_FirstArt != null) ? b_FirstArt.SeqNum + 1 : 1);
                FirstArt.ExpectedQuantity = bJobOper.FAQty;
                FirstArt.EmployeeNum = ttLaborDtl.EmployeeNum;
                FirstArt.EntryPerson = Session.UserID;
                FirstArt.SysDate = CompanyTime.Today();
                FirstArt.SysTime = CompanyTime.Now().SecondsSinceMidnight();
            }
        }

        private void createMtlqPwip()
        {
            string foreignKey1 = LaborDtl.Company + LaborDtl.LaborHedSeq + LaborDtl.LaborDtlSeq;
            decimal qtyRepToMtlQBefEndAct = LibUsePatchFld.GetPatchFldDecimal(Session.CompanyID, "LaborDtl", "QtyRepToMtlQBefEndAct", foreignKey1);
            decimal qtyToMtlQueue = LaborDtl.LaborQty - qtyRepToMtlQBefEndAct;

            /* Process MtlQueue and PartWip only for production and setup work and there is a qty to be reported. We can have a negative qty. They must have the AMM module - DCD-AM = TRUE */
            if (LaborDtl.LaborType.Compare("I") != 0 /* indirect */
                && ((LaborDtl.LaborQty - saveLaborQty) != 0 || (qtyToMtlQueue != 0) || LaborDtl.ScrapQty != 0 || LaborDtl.DiscrepQty != 0)
                && Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement))
            {
                saveLaborQty = LaborDtl.LaborQty - saveLaborQty;
                if ((oldJobQty + saveLaborQty) < 0 && !ttLaborDtl.ReWork)
                {
                    throw new BLException(Strings.TheQuantEnteredWillResultInANegatTotalQuantPosted);
                }

                if (!string.IsNullOrEmpty(ttLaborDtl.PCID))
                {
                    if (!EnablePCID(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.LaborType, ttLaborDtl.ReWork)
                        || saveLaborQty <= 0)
                    {
                        throw new BLException(Strings.PCIDCannotBeUsedForThisTransaction, "PCID");
                    }

                    if (ttLaborDtl.EnableLot && string.IsNullOrEmpty(ttLaborDtl.LotNum))
                    {
                        throw new BLException(Strings.LotMustBeEnteredWhenUsingPCID, "LotNum");
                    }

                    ttLaborDtl.PCID = ValidatePCID(ttLaborDtl.Company, ttLaborDtl.PCID, ttLaborDtl.PartNum, ttLaborDtl.LotNum, ttLaborDtl.LaborUOM, ttLaborDtl.JobNum, false);
                    sPCID = ttLaborDtl.PCID;

                    using (PackageControl libPackageControl = new PackageControl(Db))
                    {
                        string pkgControlStatus = libPackageControl.GetPCIDStatus(ttLaborDtl.Company, ttLaborDtl.PCID);
                        if (!pkgControlStatus.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                        {
                            libPackageControl.GetPCIDLocation(ttLaborDtl.Company, ttLaborDtl.PCID, out sWhseCode, out sBinNum);
                        }
                    }
                }

                LibPWIPMtlQ.RunPWIPMtlQ(ref sWhseCode, ref sBinNum, ref sPCID, saveLaborQty, qtyToMtlQueue, LaborDtl.SysRowID, ttLaborDtl.NextAssemblySeq, ttLaborDtl.NextOprSeq, ttLaborDtl.RequestMove, string.Empty, ttLaborDtl.LotNum);
            }
        }

        /// <summary>
        /// Create/Update a Material Queue Record for the Nonconformance 
        /// if the "Request Move" box was selected in MES>End Activity.
        /// I had to move the logic from the NonConf trigger to here, because the "LaborDtl.RequestMove"
        /// field is an external field and cannot be used in the triggers. 
        /// </summary>
        /// <remarks>
        /// This procedure assumes that the NonConf write trigger has already being fired 
        /// and that the ttLaborDtl buffer is available.      
        /// </remarks>
        private void createNonConfMtlQ()
        {
            NonConf = this.FindFirstNonConfWithUpdLock(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);
            if (NonConf == null)
            {
                return;
            }

            NonConf.RequestMove = ttLaborDtl.RequestMove;
            NonConf.AttributeSetID = ttLaborDtl.DiscrepAttributeSetID;



            /* This part was copied from the NonConf>Write.p trigger. I just changed the "find" statements to use the findtbl library 
               and switched the statements (if, create, etc) to lower case to comply with standards */
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) && NonConf.TrnTyp.Compare("O") != 0 && NonConf.TrnTyp.Compare("M") != 0 && ttLaborDtl.RequestMove)
            {/*if from and to warehouse or bin is not same*/
                if (((!String.IsNullOrEmpty(NonConf.WarehouseCode) && !String.IsNullOrEmpty(NonConf.BinNum)) && ((NonConf.WarehouseCode.Compare(NonConf.ToWarehouseCode) != 0) || (NonConf.BinNum.Compare(NonConf.ToBinNum) != 0))))
                {


                    MtlQueue = this.FindFirstMtlQueueWithUpdLock(Session.CompanyID, NonConf.TranID);
                    if (MtlQueue == null)
                    {
                        MtlQueue = new Erp.Tables.MtlQueue();
                        Db.MtlQueue.Insert(MtlQueue);
                        MtlQueue.Company = Session.CompanyID;
                        MtlQueue.NCTranID = NonConf.TranID;
                    }/*if not avail MtlQueue*/

                    MtlQueue.JobNum = NonConf.JobNum;
                    MtlQueue.AssemblySeq = NonConf.AssemblySeq;
                    MtlQueue.JobSeq = ((NonConf.TrnTyp.Compare("M") == 0) ? NonConf.MtlSeq : NonConf.OprSeq);
                    MtlQueue.JobSeqType = ((NonConf.TrnTyp.Compare("M") == 0) ? "M" : "O");
                    MtlQueue.FromBinNum = NonConf.BinNum;
                    MtlQueue.FromWhse = NonConf.WarehouseCode;
                    MtlQueue.IUM = NonConf.ScrapUM;
                    MtlQueue.PartNum = NonConf.PartNum;
                    MtlQueue.PartDescription = NonConf.Description;
                    MtlQueue.Plant = Session.PlantID;
                    MtlQueue.Quantity = NonConf.Quantity;
                    MtlQueue.Reference = Compatibility.Convert.ToString(NonConf.TranID);
                    MtlQueue.ReferencePrefix = "NonConf:";
                    MtlQueue.RevisionNum = NonConf.RevisionNum;
                    MtlQueue.SysDate = CompanyTime.Today();
                    MtlQueue.SysTime = CompanyTime.Now().SecondsSinceMidnight();
                    MtlQueue.NeedByDate = CompanyTime.Today();
                    MtlQueue.NeedByTime = CompanyTime.Now().SecondsSinceMidnight();
                    MtlQueue.ToBinNum = NonConf.ToBinNum;
                    MtlQueue.ToWhse = NonConf.ToWarehouseCode;
                    MtlQueue.TranType = ((NonConf.TrnTyp.Compare("D") == 0) ? "ASM-INS" : ((NonConf.TrnTyp.Compare("I") == 0) ? "STK-INS" : ((NonConf.TrnTyp.Compare("S") == 0) ? "SUB-INS" : "")));
                    MtlQueue.LotNum = NonConf.LotNum;
                }
                else
                {


                    MtlQueue = this.FindFirstMtlQueueWithUpdLock(Session.CompanyID, NonConf.TranID, "INS");
                    if (MtlQueue != null)
                    {
                        Db.MtlQueue.Delete(MtlQueue);
                    }
                }
            }
        }

        private void crewCount()
        {
            int crewCount = 0;
            Erp.Tables.LaborDtl bLaborDtl = null;


            foreach (var bLaborDtl_iterator in (this.SelectLaborDtl(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.LaborDtlSeq, true)))
            {
                bLaborDtl = bLaborDtl_iterator;
                crewCount = crewCount + 1;
            }
            if (crewCount > 0)
            {
                ttLaborDtl.MultipleEmployeesText = Strings.EmploSAreAlreadyWorkingOnThisOpera(crewCount);
            }
            else
            {
                ttLaborDtl.MultipleEmployeesText = "";
            }
        }

        /// <summary>
        /// This method sets dataset fields when the AssemblySeq field changes
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="assemblySeq">Proposed AssemblySeq change </param>
        public void DefaultAssemblySeq(ref LaborTableset ds, int assemblySeq)
        {
            CurrentFullTableset = ds;
            string vTest = string.Empty;

            if (ttLaborDtl == null)
            {
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttLaborDtl == null && ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborHasNotChanged, "LaborDtl");
            }
            /************************************/
            /* Set default values on ttLaborDtl */
            /************************************/
            if (ttLaborDtl != null)
            {
                this.getBeforeInfo();
                if (oldAssSeq != assemblySeq)
                {
                    ttLaborDtl.OprSeq = 0;
                    ttLaborDtl.MultipleEmployeesText = "";
                    ttLaborDtl.RoleCd = "";
                    ttLaborDtl.RoleCdRoleDescription = "";
                    ttLaborDtl.TimeTypCd = "";
                    ttLaborDtl.TimeTypCdDescription = "";
                }
                if ("P,S".Lookup(ttLaborDtl.LaborType) > -1)
                {
                    this.setComplete(ttLaborDtl.JobNum, assemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.LaborQty, ttLaborDtl.ScrapQty);
                }

                ttLaborDtl.CompleteFlag = ((ttLaborDtl.CallNum == 0) ? ttLaborDtl.Complete : ttLaborDtl.FSComplete);
                ttLaborDtl.AssemblySeq = assemblySeq;
                ttLaborDtl.EnableSN = enableSN(ttLaborDtl.JobNum, assemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.PCID, ttLaborDtl.PartNum);

                /* SCR 156781 - If the previous job/asm/opr combination is for QuantityOnly/Fixed Hours labor type then   *
                 * we have to reset the ISFixHoursAndQtyOnly flag and default the correct labor/burden hours based on the *
                 * clock in/out times if the new combination is not fixed hours. Otherwise use the estimated prod hours.  */
                /* Only reset the default hours using the clockin/out times if previously QuantityOnly/FixedHours.        */
                if (ttLaborDtl.ISFixHourAndQtyOnly == true)
                {
                    ttLaborDtl.ISFixHourAndQtyOnly = false; /* needs to reset before calling payHoursDtl() */
                    this.payHoursDtl(true, true, true, out vTest);
                }
                ttLaborDtl.ISFixHourAndQtyOnly = this.IsFixHoursAndQtyOnly(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                /* if the new OprSeq has QuantityOnly/FixedHours setting then default the hours based on estimated production hours */
                if (ttLaborDtl.ISFixHourAndQtyOnly == true)
                {
                    ttLaborDtl.LaborHrs = this.CalcProdFixedHours(ttLaborDtl.LaborType, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                    ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
                }

                /* Set DisPrjRoleCd and DisTimeTypCd */
                this.disPrjFields(true);
            }
            /******************************************/
            /* Set default values on ttTimeWeeklyView */
            /******************************************/
            if (ttTimeWeeklyView != null)
            {
                ttTimeWeeklyView.AssemblySeq = assemblySeq;
                ttTimeWeeklyView.RoleCd = "";
                ttTimeWeeklyView.RoleCdDescription = "";
                ttTimeWeeklyView.TimeTypCd = "";
                ttTimeWeeklyView.TimeTypCdDescription = "";
                /* Set DisPrjRoleCd and DisTimeTypCd */
                this.disPrjFieldsTimeWeeklyView();
            }
        }

        /// <summary>
        /// This method updates the dataset after complete flag is set
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="cmplete">Proposed change to the Complete field </param>
        /// <param name="vMessage">Returns a string of warnings user needs to know</param>
        public void DefaultComplete(ref LaborTableset ds, bool cmplete, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            JobOper = this.FindFirstJobOper10(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
            this.setOprComplete(cmplete);
            ttLaborDtl.CompleteFlag = ((ttLaborDtl.CallNum == 0) ? cmplete : ttLaborDtl.FSComplete);
            ttLaborDtl.Complete = cmplete;
            this.warnLabor(ref vMessage);
        }

        /// <summary>
        /// This method updates the clock in and clock out dates for the LaborHed and LaborDtl
        /// tables when the payroll date has changed.  
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="payrollDate">Proposed Payroll Date change</param>
        public void DefaultDate(ref LaborTableset ds, DateTime? payrollDate)
        {
            CurrentFullTableset = ds;


            ttLaborHed = (from ttLaborHed_Row in ds.LaborHed
                          where modList.Lookup(ttLaborHed_Row.RowMod) != -1
                          select ttLaborHed_Row).FirstOrDefault();
            if (ttLaborHed == null)
            {
                throw new BLException(Strings.LaborHeaderHasNotChanged, "LaborHed");
            }
            eadErrMsg = LibEADValidation.validateEAD(payrollDate, "IP", "");
            if (!String.IsNullOrEmpty(eadErrMsg))
            {
                throw new BLException(eadErrMsg);
            }
            if (lDefDateFromNewHeader)
            {
                if (payrollDate == null)
                {
                    ttLaborHed.ActualClockinDate = null;
                }
                else
                {
                    ttLaborHed.ActualClockinDate = payrollDate.Value.Date;
                }

                if (payrollDate == null)
                {
                    ttLaborHed.ClockInDate = null;
                }
                else
                {
                    ttLaborHed.ClockInDate = payrollDate.Value.Date;
                }
            }
            if (ttLaborHed.RowMod.Compare(IceRow.ROWSTATE_ADDED) != 0)
            {

                using (TransactionScope txScopeUpdateDefaultDate = ErpContext.CreateDefaultTransactionScope())
                {
                    foreach (var LaborDtl_iterator in (this.SelectLaborDtlWithUpdLock(ttLaborHed.Company, ttLaborHed.LaborHedSeq)))
                    {
                        LaborDtl = LaborDtl_iterator;
                        LaborDtl.PayrollDate = payrollDate.Value.Date; /* Pass Payroll Date to Detail */
                        Db.Validate(LaborDtl);
                        this.refreshTtLaborDtl(LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq);
                        if (ttLaborDtl.ClockOutTime == 24.0m)
                        {
                            ttLaborDtl.ClockOutTime = 0;
                        }
                    }
                    txScopeUpdateDefaultDate.Complete();
                }
            }
            if (ttLaborHed.ActualClockinDate.Value.Date != ttLaborHed.ClockInDate.Value.Date)
            {
                if (ttLaborHed.ActualClockinDate == null)
                {
                    ttLaborHed.ClockInDate = null;
                }
                else
                {
                    ttLaborHed.ClockInDate = ttLaborHed.ActualClockinDate.Value.Date;
                }
            }

            if (payrollDate == null)
            {
                ttLaborHed.PayrollDate = null;
            }
            else
            {
                ttLaborHed.PayrollDate = payrollDate.Value.Date;
            }

            ttLaborHed.PayrollDateNav = ttLaborHed.PayrollDate;
        }

        /// <summary>
        /// This method defaults fields when the discrepancy reason code field changes.  
        /// Also checks for any warnings the user needs to be aware of
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ProposedDiscrpRsnCode">Proposed discrepancy reason </param>
        public void DefaultDiscrpRsnCode(ref LaborTableset ds, string ProposedDiscrpRsnCode)
        {
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }



            Reason = this.FindFirstReason(Session.CompanyID, "S", ProposedDiscrpRsnCode);
            if (Reason == null)
            {
                throw new BLException(Strings.InvalidDiscrepancyReasonCode, "LaborDtl", "DiscrpRsnCode");
            }
            ttLaborDtl.DiscrpRsnCode = ProposedDiscrpRsnCode;
            LaborDtl_Foreign_Link();
        }

        /// <summary>
        /// This method defaults fields when the discrepancy reason code field changes.  
        /// Also checks for any warnings the user needs to be aware of
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ProposedReworkReasonCode">Proposed discrepancy reason </param>
        public void DefaultReworkReasonCode(ref LaborTableset ds, string ProposedReworkReasonCode)
        {
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            Reason = this.FindFirstReason(Session.CompanyID, "R", ProposedReworkReasonCode);
            if (Reason == null)
            {
                throw new BLException(Strings.InvalidReworkReasonCode, "LaborDtl", "ReworkReasonCode");
            }
            ttLaborDtl.ReworkReasonCode = ProposedReworkReasonCode;
            LaborDtl_Foreign_Link();
        }

        /// <summary>
        /// This method updates the hours when a time field changes
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        public void DefaultDtlTime(ref LaborTableset ds)
        {
            CurrentFullTableset = ds;
            string vTest = string.Empty;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }
            if (ttLaborDtl.ClockOutTime == 0)
            {
                ttLaborDtl.ClockOutTime = 24.0m;
            }

            this.payHoursDtl(true, true, true, out vTest);
            if (this.isHCMEnabledAt(ttLaborDtl.EmployeeNum).Equals("DTL", StringComparison.OrdinalIgnoreCase) && ttLaborDtl.RowMod.Equals("A"))
            {
                ttLaborDtl.HCMPayHours = ((ttLaborDtl.LaborHrs >= 0) ? ttLaborDtl.LaborHrs : 0);
            }

            if (ttLaborDtl.ClockOutTime == 24)
            {
                ttLaborDtl.ClockOutTime = 0;
            }
        }

        /// <summary>
        /// This method defaults the expense code when the indirect code has changed
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="indirectCode">Proposed change to the indirect code </param>
        public void DefaultIndirect(ref LaborTableset ds, string indirectCode)
        {
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }
            this.validateIndirect(indirectCode, false);
            ExceptionManager.AssertNoBLExceptions();
            if (!String.IsNullOrEmpty(Indirect.ExpenseCode))
            {
                if (ttLaborDtl.RowMod.Compare(IceRow.ROWSTATE_ADDED) == 0)
                {
                    ttLaborDtl.ExpenseCode = Indirect.ExpenseCode;
                }

                ttLaborDtl.IndirectDescription = Indirect.Description;
            }
            else
            {


                EmpBasic = this.FindFirstEmpBasic2(Session.CompanyID, ttLaborDtl.EmployeeNum);
                if (EmpBasic != null)
                {
                    ttLaborDtl.ExpenseCode = EmpBasic.ExpenseCode;
                }

                ttLaborDtl.IndirectDescription = "";
            }
            ttLaborDtl.IndirectCode = indirectCode;
            LaborDtl_Foreign_Link();
        }

        /// <summary>
        /// This method will take the selected rows from the work queue and process them in one server call.
        /// GetNewLaborDtlOnSelectForWork is called for each work queue row, after that SelectForWork will be called filling required information in all the added LaborDtl rows
        /// If there is any warning that needs user input the method will finish before calling Update and the prompts will be shown to the user, after the UI will call Update to finish.
        /// </summary>
        /// <param name="laborDS">LaborTableSet - Contains only the header.</param>
        /// <param name="selectedWorkQueueRows">Selected rows from Work Queue / type WorkQueueTable</param>
        /// <param name="empID">Employee ID which is starting the activities.</param>
        /// <param name="resourceGrpID">Resource Group ID for all activities.</param>
        /// <param name="resourceID">Resource used for all activities.</param>
        /// <param name="laborType">Labor Type, can be 'P' for Production or 'S' for Setup</param>
        /// <param name="warningsMsg">Contains warning messages that need to be shown to the user before proceeding.</param>
        public void SelectAllForWork(ref LaborTableset laborDS, Erp.Tablesets.WorkQueueTable selectedWorkQueueRows, string empID, string resourceGrpID, string resourceID, string laborType, out string warningsMsg)
        {
            warningsMsg = String.Empty;
            string laborHedWhereClause = "ActiveTrans = true AND EmployeeNum='" + empID + "'";
            string emptyGuidWhereClause = "SysRowId = '" + Guid.Empty + "'";
            bool morePages = false;
            bool machinePrompt = false;
            laborDS = GetRows(laborHedWhereClause, emptyGuidWhereClause, emptyGuidWhereClause, emptyGuidWhereClause, emptyGuidWhereClause, emptyGuidWhereClause, emptyGuidWhereClause, emptyGuidWhereClause, emptyGuidWhereClause, emptyGuidWhereClause, emptyGuidWhereClause, emptyGuidWhereClause, emptyGuidWhereClause, 0, 0, out morePages);
            laborDS.LaborHed[0].RowMod = "U";
            foreach (var wqRow in selectedWorkQueueRows)
            {
                if (this.ReportPartQtyAllowed(wqRow.JobNum, wqRow.AssemblySeq, wqRow.OprSeq))
                {
                    throw new BLException(Strings.CoPartError(wqRow.JobNum, wqRow.AssemblySeq, wqRow.OprSeq));
                }
                GetNewLaborDtlOnSelectForWork(ref laborDS, laborDS.LaborHed[0].LaborHedSeq, wqRow.JobNum, wqRow.AssemblySeq, wqRow.OprSeq, wqRow.ResourceGrpID, wqRow.SetupOrProd, out machinePrompt);
                if (machinePrompt && String.IsNullOrEmpty(resourceID))
                {
                    throw new BLException(Strings.NeedResource);
                }
                laborDS.LaborDtl[laborDS.LaborDtl.Count - 1].OkToChangeResourceGrpID = true;
                laborDS.LaborDtl[laborDS.LaborDtl.Count - 1].ResourceID = resourceID;
            }
            SelectForWork(ref laborDS, resourceGrpID, resourceID, laborType);
            string checkWarningsMsg = String.Empty;
            string firstArticleWarningMsg = String.Empty;
            CheckWarnings(ref laborDS, out checkWarningsMsg);
            CheckFirstArticleWarning(ref laborDS, out firstArticleWarningMsg);
            if (String.IsNullOrEmpty(checkWarningsMsg) && String.IsNullOrEmpty(firstArticleWarningMsg))
            {
                Update(ref laborDS);
            }
            else
            {
                warningsMsg = checkWarningsMsg + Ice.Constants.LIST_DELIM + firstArticleWarningMsg;
            }
        }

        /// <summary>
        /// This method defaults dataset fields when the JobNum field changes
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="jobNum">Proposed change to the JobNum field </param>
        public void DefaultJobNum(ref LaborTableset ds, string jobNum)
        {
            string vTest = string.Empty;
            CurrentFullTableset = ds;
            if (ttLaborDtl == null)
            {
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttLaborDtl == null && ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborHasNotChanged, "LaborDtl");
            }
            /************************************/
            /* Set default values on ttLaborDtl */
            /************************************/
            if (ttLaborDtl != null)
            {
                using (var jobEntrySvc = ServiceRenderer.GetService<Erp.Contracts.JobEntrySvcContract>(this.Db))
                {
                    jobEntrySvc.ValidateJobNum(jobNum);
                }
                this.validateJob(jobNum, true, true);
                ExceptionManager.AssertNoBLExceptions();
                ttLaborDtl.AssemblySeq = 0;
                ttLaborDtl.OprSeq = 0;
                ttLaborDtl.MultipleEmployeesText = "";

                /* SCR 156781 - If the previous job/asm/opr combination is for QuantityOnly/Fixed Hours labor type then   *
                 * we have to reset the ISFixHoursAndQtyOnly flag and default the correct labor/burden hours based on the *
                 * clock in/out times.                                                                                    */
                if (ttLaborDtl.ISFixHourAndQtyOnly == true)
                {
                    ttLaborDtl.ISFixHourAndQtyOnly = false;
                    this.payHoursDtl(true, true, true, out vTest);
                }

                if (!String.IsNullOrEmpty(JobHead.ExpenseCode))
                {
                    ttLaborDtl.ExpenseCode = JobHead.ExpenseCode;
                }
                else
                {


                    EmpBasic = this.FindFirstEmpBasic3(Session.CompanyID, ttLaborDtl.EmployeeNum);
                    if (EmpBasic != null)
                    {
                        ttLaborDtl.ExpenseCode = EmpBasic.ExpenseCode;
                    }
                }
                ttLaborDtl.JobType = this.getJobType(jobNum);
                if (ttLaborDtl.JobTypeCode.Compare("MNT") == 0)
                {
                    ttLaborDtl.LaborType = "S"; /* setup */
                    ttLaborDtl.DiscrepQty = 0;
                    ttLaborDtl.DiscrpRsnCode = "";
                    ttLaborDtl.LaborQty = 0;
                    ttLaborDtl.ScrapQty = 0;
                    ttLaborDtl.ScrapReasonCode = "";
                }
                if ("P,S".Lookup(ttLaborDtl.LaborType) > -1)
                {
                    this.setComplete(jobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.LaborQty, ttLaborDtl.ScrapQty);
                }

                ttLaborDtl.DisplayJob = this.getDisplayJob(ttLaborDtl.LaborType, ttLaborDtl.IndirectCode, jobNum);
                ttLaborDtl.CompleteFlag = ((ttLaborDtl.CallNum == 0) ? ttLaborDtl.Complete : ttLaborDtl.FSComplete);
                ttLaborDtl.JobNum = jobNum;
                ttLaborDtl.EnableSN = enableSN(jobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.PCID, ttLaborDtl.PartNum);
                ttLaborDtl.RoleCd = "";
                ttLaborDtl.RoleCdRoleDescription = "";
                ttLaborDtl.TimeTypCd = "";
                ttLaborDtl.TimeTypCdDescription = "";
                /* Set DisPrjRoleCd and DisTimeTypCd */
                this.disPrjFields(true);
                LaborDtl_Foreign_Link();
            }
            /******************************************/
            /* Set default values on ttTimeWeeklyView */
            /******************************************/
            if (ttTimeWeeklyView != null)
            {


                JobHead = this.FindFirstJobHead(Session.CompanyID, Session.PlantID, jobNum);       /* indirect */
                if (JobHead != null && ttTimeWeeklyView.LaborType.Compare("I") != 0)
                {
                    if (JobHead.JobClosed)
                    {
                        throw new BLException(Strings.ThisJobHasBeenClosedEntryNotAllowed);
                    }
                    if (!JobHead.JobReleased)
                    {
                        throw new BLException(Strings.ThisJobHasNotBeenReleaEntryNotAllowed);
                    }
                    if (String.IsNullOrEmpty(ttTimeWeeklyView.ProjectID) && JobHead.JobType.Compare("PRJ") == 0)
                    {
                        throw new BLException(Strings.ThisJobIsAJobTypeOfProjectEntryNotAllowedForNon, "ttTimeWeeklyView");
                    }
                }       /* indirect */
                if (JobHead == null && ttTimeWeeklyView.LaborType.Compare("I") != 0)
                {
                    throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.JobNumber));
                }
                if (!String.IsNullOrEmpty(JobHead.ExpenseCode))
                {
                    ttTimeWeeklyView.ExpenseCode = JobHead.ExpenseCode;
                }
                else
                {


                    EmpBasic = this.FindFirstEmpBasic4(Session.CompanyID, ttTimeWeeklyView.EmployeeNum);
                    if (EmpBasic != null)
                    {
                        ttTimeWeeklyView.ExpenseCode = EmpBasic.ExpenseCode;
                    }
                }
                ttTimeWeeklyView.JobNum = jobNum;
                ttTimeWeeklyView.RoleCd = "";
                ttTimeWeeklyView.RoleCdDescription = "";
                ttTimeWeeklyView.TimeTypCd = "";
                ttTimeWeeklyView.TimeTypCdDescription = "";
                /* Set DisPrjRoleCd and DisTimeTypCd */
                this.disPrjFieldsTimeWeeklyView();
                TimeWeeklyView_Foreign_Link();
            }
        }

        /// <summary>
        /// This method updates the tot hours display field when the labor hours clock in/out
        /// time changes
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="laborHrs">Proposed Labor Hrs change </param>
        public void DefaultLaborHrs(ref LaborTableset ds, decimal laborHrs)
        {
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }
            ttLaborDtl.LaborHrs = Math.Round(laborHrs, 2, MidpointRounding.AwayFromZero);
            this.calcTotHrs(laborHrs);
        }

        /// <summary>
        /// This method defaults fields when the labor qty fields changes.  Also checks
        /// for any labor warnings the user needs to be aware of
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="laborQty">Proposed change to LaborQty field </param>
        /// <param name="vMessage">Returns a string of warnings user needs to know</param>
        public void DefaultLaborQty(ref LaborTableset ds, decimal laborQty, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            if (ReportPartQtyAllowed(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq))
            {
                throw new BLException(Strings.LaborQtyShouldNotBeDirecUpdatedThereAreCoParts, "LaborDtl", "LaborQty");
            }

            if (ttLaborDtl.EndActivity && !ttLaborDtl.EnablePCID)
            {
                ttLaborDtl.EnablePCID = EnablePCID(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.LaborType, ttLaborDtl.ReWork)
                    && laborQty > FindFirstLaborDtlLaborQty(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);

                if (!ttLaborDtl.EnablePCID)
                {
                    ttLaborDtl.PCID = string.Empty;
                    ttLaborDtl.LotNum = string.Empty;
                    ttLaborDtl.EnableLot = false;
                    ttLaborDtl.PrintPCIDContents = false;
                }
            }

            chkLaborQty(laborQty, out vMessage);

            if ("P,S".Lookup(ttLaborDtl.LaborType) > -1)
            {
                setComplete(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, laborQty, ttLaborDtl.ScrapQty);
            }

            ttLaborDtl.CompleteFlag = ((ttLaborDtl.CallNum == 0) ? ttLaborDtl.Complete : ttLaborDtl.FSComplete);
            ttLaborDtl.LaborQty = laborQty;
        }

        /// <summary>
        /// This method defaults fields when the labor qty fields changes.  Also checks
        /// for any labor warnings the user needs to be aware of
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="nonConformanceQty">Proposed change to LaborQty field </param>
        public void DefaultNonConformanceQty(ref LaborTableset ds, decimal nonConformanceQty)
        {

            CurrentFullTableset = ds;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            if (ttLaborDtl.EndActivity && !ttLaborDtl.EnablePCID)
            {
                ttLaborDtl.EnablePCID = EnablePCID(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.LaborType, ttLaborDtl.ReWork)
                    && nonConformanceQty > FindFirstLaborDtlDiscrepQtyQty(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);

                if (!ttLaborDtl.EnablePCID)
                {
                    ttLaborDtl.NonConfPCID = string.Empty;
                }
            }

            ttLaborDtl.DiscrepQty = nonConformanceQty;
        }

        /// <summary>
        /// This method defaults fields when the scrap qty field changes.  Also checks
        /// for any labor warnings the user needs to be aware of
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>        
        /// <param name="scrapQty">Proposed change to ScrapQty field </param>
        /// <param name="vMessage">Returns a string of warnings user needs to know</param>
        public void VerifyScrapQty(ref LaborTableset ds, decimal scrapQty, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            if ("P,S".Lookup(ttLaborDtl.LaborType) > -1)
            {
                this.setComplete(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.LaborQty, scrapQty);
            }
            ttLaborDtl.CompleteFlag = ((ttLaborDtl.CallNum == 0) ? ttLaborDtl.Complete : ttLaborDtl.FSComplete);
            ttLaborDtl.ScrapQty = scrapQty;
        }

        /// <summary>
        /// This method sets Complete checkbox when part qty field changes in End Activity. 
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="partQty">Proposed change to PartQty field </param>
        /// <param name="sysRowID">sysRowID of line updated in LaborPart</param>
        /// <param name="vMessage">Returns a string of warnings user needs to know</param>
        public void OnChangePartQty(ref LaborTableset ds, decimal partQty, Guid sysRowID, out string vMessage)
        {
            vMessage = string.Empty;
            decimal acumQty = 0;
            decimal acumScrapQty = 0;
            string vPartNum;
            LaborPartRow tmpLaborPart;
            string vJobMainPart = string.Empty;
            bool vJobSequential = true;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where ttLaborDtl_Row.RowMod.Compare(IceRow.ROWSTATE_UPDATED) == 0
                          select ttLaborDtl_Row).FirstOrDefault();

            var JobPartProcessModeResult = GetJobHeadPartProcessMode(Session.CompanyID, ttLaborDtl.JobNum);
            if (JobPartProcessModeResult != null)
            {
                vJobMainPart = JobPartProcessModeResult.PartNum;
                vJobSequential = !JobPartProcessModeResult.ProcessMode.Equals("C", StringComparison.OrdinalIgnoreCase);
            }

            tmpLaborPart = (from tmpLaborPart_Row in ds.LaborPart
                            where tmpLaborPart_Row.SysRowID == sysRowID &&
                            modList.Lookup(tmpLaborPart_Row.RowMod) != -1
                            select tmpLaborPart_Row).FirstOrDefault();

            if (tmpLaborPart != null)
            {
                vPartNum = tmpLaborPart.PartNum;
                tmpLaborPart.PartQty = partQty;
            }
            else
            {
                //First Change in LaborPart does not bring the LaborPart in dataset
                LaborPart = FindFirstLaborPart(sysRowID);
                vPartNum = LaborPart.PartNum;
                if (vJobSequential == true || vJobMainPart.Equals(vPartNum, StringComparison.OrdinalIgnoreCase))
                {
                    acumQty = partQty;  //Value from record not yet in dataset
                }
            }

            #region Acum PartQty and ScrapQty in current Labor Part
            //Values in LaborPart before Update
            Hashtable LaborPartQty = new Hashtable();
            Hashtable LaborPartQty2 = new Hashtable();
            foreach (LaborPartRow tmpLaborPart2 in (from tmpLaborPart_Row in ds.LaborPart
                                                    where modList.Lookup(tmpLaborPart_Row.RowMod) == -1
                                                    select tmpLaborPart_Row))
            {
                if (vJobSequential == true || vJobMainPart.Equals(tmpLaborPart2.PartNum, StringComparison.OrdinalIgnoreCase))
                {
                    LaborPartQty.Add(tmpLaborPart2.SysRowID, tmpLaborPart2.PartQty);
                    LaborPartQty2.Add(tmpLaborPart2.SysRowID, tmpLaborPart2.ScrapQty);
                }
            }

            //Values in LaborPart with RowMod = "U"
            foreach (LaborPartRow tmpLaborPart3 in (from tmpLaborPart_Row in ds.LaborPart
                                                    where tmpLaborPart_Row.RowMod.Compare(IceRow.ROWSTATE_UPDATED) == 0
                                                    select tmpLaborPart_Row))
            {
                if (vJobSequential == true || vJobMainPart.Equals(tmpLaborPart3.PartNum, StringComparison.OrdinalIgnoreCase))
                {
                    LaborPartQty[tmpLaborPart3.SysRowID] = tmpLaborPart3.PartQty;
                    LaborPartQty2[tmpLaborPart3.SysRowID] = tmpLaborPart3.ScrapQty;
                }
            }

            //Get Total PartQty with current updates
            foreach (decimal values in LaborPartQty.Values)
            {
                acumQty = acumQty + values;
            }

            //Get Total with current updates
            foreach (decimal values in LaborPartQty2.Values)
            {
                acumScrapQty = acumScrapQty + values;
            }
            #endregion

            this.setComplete(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, acumQty, acumScrapQty);
        }

        /// <summary>
        /// This method defaults dataset fields when the LaborType field changes.
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ipLaborType">Proposed LaborType change </param>
        public void DefaultLaborType(ref LaborTableset ds, string ipLaborType)
        {
            CurrentFullTableset = ds;
            string vTest = string.Empty;

            if (ttLaborDtl == null)
            {
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttLaborDtl == null && ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborHasNotChanged, "LaborDtl", "RowMod");
            }
            /************************************/
            /* Set default values on ttLaborDtl */
            /************************************/
            if (ttLaborDtl != null)
            {
                if (ttLaborDtl.LaborTypePseudo.Compare("J") == 0 && ipLaborType.Compare("J") != 0)
                {
                    ttLaborDtl.ProjectID = "";
                    ttLaborDtl.PhaseID = "";
                    ttLaborDtl.PhaseJobNum = "";
                    ttLaborDtl.PhaseOprSeq = 0;
                    ttLaborDtl.DisplayJob = "";
                    ttLaborDtl.JobNum = "";
                    ttLaborDtl.AssemblySeq = 0;
                    ttLaborDtl.OprSeq = 0;
                    ttLaborDtl.QuickEntryCode = "";
                }
                if (!ttLaborDtl.LaborTypePseudo.KeyEquals("J") && ipLaborType.KeyEquals("J"))
                {
                    ttLaborDtl.JobNum = "";
                    ttLaborDtl.AssemblySeq = 0;
                    ttLaborDtl.OprSeq = 0;
                }
                ttLaborDtl.LaborTypePseudo = ipLaborType;
                ttLaborDtl.LaborType = ((ttLaborDtl.LaborTypePseudo.Compare("I") == 0) ? "I" : ((ttLaborDtl.LaborTypePseudo.Compare("S") == 0) ? "S" : "P"));
                this.disPrjFields(true);
                ttLaborDtl.ResourceGrpID = "";
                ttLaborDtl.ResourceID = "";
                ttLaborDtl.JCDept = "";
                if (ipLaborType.Compare("I") == 0)
                {


                    EmpBasic = this.FindFirstEmpBasic5(ttLaborDtl.Company, ttLaborDtl.EmployeeNum);
                    if (EmpBasic != null)
                    {
                        ttLaborDtl.ResourceGrpID = EmpBasic.ResourceGrpID;
                        ttLaborDtl.ResourceID = EmpBasic.ResourceID;
                        ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, EmpBasic.ResourceGrpID);
                        if (ResourceGroup != null)
                        {
                            ttLaborDtl.JCDept = ResourceGroup.JCDept;
                        }
                    }
                }
                if (ipLaborType.Compare("I") != 0)
                {
                    ttLaborDtl.IndirectCode = "";
                    ttLaborDtl.ExpenseCode = "";
                }

                /* SCR 156781 - If the current job/asm/opr combination is for QuantityOnly/Fixed Hours labor type then *
                 * we have to default the correct labor/burden hours based on the Setup/Production Estimated Hours.    */
                ttLaborDtl.ISFixHourAndQtyOnly = this.IsFixHoursAndQtyOnly(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                /* if the new OprSeq has QuantityOnly/FixedHours setting then default the hours based on estimated production hours */
                if (ttLaborDtl.ISFixHourAndQtyOnly == true)
                {
                    ttLaborDtl.LaborHrs = this.CalcProdFixedHours(ttLaborDtl.LaborType, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                    ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
                }
                else
                {
                    this.payHoursDtl(true, true, true, out vTest);
                }

                LaborDtl_Foreign_Link();
            }
            /******************************************/
            /* Set default values on ttTimeWeeklyView */
            /******************************************/
            if (ttTimeWeeklyView != null)
            {
                if (ttTimeWeeklyView.LaborTypePseudo.Compare("J") == 0 && ipLaborType.Compare("J") != 0)
                {
                    ttTimeWeeklyView.ProjectID = "";
                    ttTimeWeeklyView.PhaseID = "";
                    ttTimeWeeklyView.JobNum = "";
                    ttTimeWeeklyView.OprSeq = 0;
                    ttTimeWeeklyView.QuickEntryCode = "";
                }
                if (ttTimeWeeklyView.LaborTypePseudo.Compare("P") == 0 && ipLaborType.Compare("P") != 0)
                {
                    ttTimeWeeklyView.JobNum = "";
                    ttTimeWeeklyView.AssemblySeq = 0;
                    ttTimeWeeklyView.OprSeq = 0;
                    ttTimeWeeklyView.OpCode = "";
                    ttTimeWeeklyView.ExpenseCode = "";
                }
                ttTimeWeeklyView.LaborTypePseudo = ipLaborType;
                ttTimeWeeklyView.LaborType = ((ttTimeWeeklyView.LaborTypePseudo.Compare("I") == 0) ? "I" : ((ttTimeWeeklyView.LaborTypePseudo.Compare("S") == 0) ? "S" : "P"));
                this.disPrjFieldsTimeWeeklyView();
                if (ttTimeWeeklyView.DisPrjRoleCd)
                {
                    ttTimeWeeklyView.RoleCd = "";
                    ttTimeWeeklyView.RoleCdDescription = "";
                }
                if (ttTimeWeeklyView.DisTimeTypCd)
                {
                    ttTimeWeeklyView.TimeTypCd = "";
                    ttTimeWeeklyView.TimeTypCdDescription = "";
                }
                ttTimeWeeklyView.ResourceGrpID = "";
                ttTimeWeeklyView.ResourceID = "";
                ttTimeWeeklyView.JCDept = "";
                if (ipLaborType.Compare("I") == 0)
                {


                    EmpBasic = this.FindFirstEmpBasic6(ttTimeWeeklyView.Company, ttTimeWeeklyView.EmployeeNum);
                    if (EmpBasic != null)
                    {
                        ttTimeWeeklyView.ResourceGrpID = EmpBasic.ResourceGrpID;
                        ttTimeWeeklyView.ResourceID = EmpBasic.ResourceID;
                    }
                }
                if (ipLaborType.Compare("I") != 0)
                {
                    ttTimeWeeklyView.IndirectCode = "";
                    ttTimeWeeklyView.ExpenseCode = "";
                }
                TimeWeeklyView_Foreign_Link();
            }
        }

        /// <summary>
        /// This method defaults the Lunch Time fields when the Lunch Break field changes.  
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        public void DefaultLunchBreak(ref LaborTableset ds)
        {
            CurrentFullTableset = ds;


            ttLaborHed = (from ttLaborHed_Row in ds.LaborHed
                          where modList.Lookup(ttLaborHed_Row.RowMod) != -1
                          select ttLaborHed_Row).FirstOrDefault();
            if (ttLaborHed == null)
            {
                throw new BLException(Strings.LaborHeaderHasNotChanged, "LaborHed");
            }
            if (ttLaborHed.LunchBreak)
            {


                JCShift = this.FindFirstJCShift3(ttLaborHed.Company, ttLaborHed.Shift);
                if (JCShift != null)
                {
                    ttLaborHed.ActLunchOutTime = JCShift.LunchStart;
                    ttLaborHed.ActLunchInTime = JCShift.LunchEnd;
                    ttLaborHed.LunchOutTime = JCShift.LunchStart;
                    ttLaborHed.LunchInTime = JCShift.LunchEnd;
                }
            }
            else
            {
                ttLaborHed.ActLunchInTime = 0;
                ttLaborHed.ActLunchOutTime = 0;
                ttLaborHed.LunchInTime = 0;
                ttLaborHed.LunchOutTime = 0;
            }
            this.payHours();
            if (ttLaborHed.ActLunchInTime == 24.0m)
            {
                ttLaborHed.ActLunchInTime = 0;
            }

            if (ttLaborHed.LunchInTime == 24.0m)
            {
                ttLaborHed.LunchInTime = 0;
            }
        }

        /// <summary>
        /// This method updates the dataset after next operation seq is set
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ProposedNextOprSeq">Proposed next operation sequence </param>
        public void DefaultNextOprSeq(ref LaborTableset ds, int ProposedNextOprSeq)
        {
            CurrentFullTableset = ds;
            bool isFinalOp = false;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }
            if ((ProposedNextOprSeq > 0 && IsLaborEntryMethodTimeAndBackflush(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ProposedNextOprSeq)) ||
                IsLaborEntryMethodTimeAndBackflush(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq))
                throw new BLException(Strings.NotAllowedToChangeNextOperationForATimeBackflushQty);

            ttLaborDtl.NextOprSeq = ProposedNextOprSeq;
            /* Find out if we are on the last operation */
            isFinalOp = this.IsFinalOp(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);

            /* If we are not on the last operation then verify operation
               otherwise if we are on the last operation, the NextOprSeq SHOULD be 0 */
            if (!(isFinalOp && ttLaborDtl.NextOprSeq == 0))
            {
                this.getEnteredOprSeq();
                ExceptionManager.AssertNoBLExceptions();
            }
            else
            {
                ttLaborDtl.NextResourceDesc = "";
                ttLaborDtl.NextOperDesc = "";
            }
        }

        /// <summary>
        /// This method checks for any warnings user needs to know on change of OpCode
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="opCode">Proposed OpCode field change </param>
        /// <param name="vMessage">Returns list of warnings for user's review</param>
        public void DefaultOpCode(ref LaborTableset ds, string opCode, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;
            string vTxt = string.Empty;
            string dspWrnStr = string.Empty;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }
            this.validateOpCode(opCode);
            ExceptionManager.AssertNoBLExceptions();
            ttLaborDtl.OpCodeOpDesc = ((OpMaster != null) ? OpMaster.OpDesc : "");
            this.warnOpCode(opCode, out vMessage);
            ttLaborDtl.OpCode = opCode;
            LaborDtl_Foreign_Link();
        }

        /// <summary>
        /// This method defaults fields when Operation sequence changes.  Also returns any
        /// warnings user needs to know.
        /// </summary>
        /// <param name="oprSeq">Proposed oprSeq change </param>
        /// <param name="vMessage">Returns warnings for user's review</param>
        /// <param name="ds">Labor Entry Data set</param>
        public void DefaultOprSeq(ref LaborTableset ds, int oprSeq, out string vMessage)
        {
            vMessage = string.Empty;
            string vTest = string.Empty;
            CurrentFullTableset = ds;
            string dfltRoleCd = string.Empty;
            string attributeSetDescription = string.Empty;
            string attributeSetShortDescription = string.Empty;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl != null)
            {
                if (ttLaborDtl.RowMod.Compare(IceRow.ROWSTATE_UPDATED) == 0)
                {

                    if (deniedColumns == null) deniedColumns = Ice.Manager.Security.GetWriteDeniedColumns(Session.CompanyID, Session.UserID, "Erp", "LaborDtl");

                    if (deniedColumns.Count > 0)
                    {

                        LaborDtl = this.FindFirstLaborDtl(ttLaborDtl.SysRowID);
                        if (LaborDtl != null)
                        {
                            foreach (string deniedFld in deniedColumns)
                            {
                                ttLaborDtl[deniedFld] = LaborDtl[deniedFld];
                            }
                        }
                    }
                }
            }



            ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                select ttTimeWeeklyView_Row).FirstOrDefault();
            if (ttLaborDtl == null && ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged);
            }
            /************************************/
            /* Set default values on ttLaborDtl */
            /************************************/
            if (ttLaborDtl != null)
            {
                this.validateJobOper(ttLaborDtl.LaborType, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, oprSeq);
                ExceptionManager.AssertNoBLExceptions();
                ttLaborDtl.OprSeq = oprSeq;
                ttLaborDtl.EnableSN = enableSN(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, oprSeq, ttLaborDtl.PCID, ttLaborDtl.PartNum);
                var outResourceGrpID = ttLaborDtl.ResourceGrpID;
                var outResourceID = ttLaborDtl.ResourceID;
                var outCapabilityID = ttLaborDtl.CapabilityID;
                this.defaultOprSeq2(ttLaborDtl.LaborType, ref outResourceGrpID, ref outResourceID, ref outCapabilityID);
                ttLaborDtl.ResourceGrpID = outResourceGrpID;
                ttLaborDtl.ResourceID = outResourceID;
                ttLaborDtl.CapabilityID = outCapabilityID;
                ttLaborDtl.OpCode = JobOper.OpCode;
                ttLaborDtl.OpComplete = JobOper.OpComplete;
                ttLaborDtl.LaborEntryMethod = JobOper.LaborEntryMethod;
                ttLaborDtl.TemplateID = JobOper.TemplateID;

                /*If LaborDtl.ReWork is already set as true is because it comes from MES Start Rework if not is set has the Job's Operation was set*/
                if (ttLaborDtl.ReWork != true)
                {
                    ttLaborDtl.ReWork = JobOper.ReWork;
                }


                JobHead = this.FindFirstJobHead6(JobOper.Company, JobOper.JobNum);
                if (JobHead != null)
                {
                    var partPartial = FindFirstPartPartial(Session.CompanyID, JobHead.PartNum);
                    if (partPartial != null && (partPartial.TrackInventoryByRevision || partPartial.TrackInventoryAttributes))
                    {
                        ttLaborDtl.LaborAttributeSetID = JobHead.AttributeSetID;
                        ttLaborDtl.DiscrepAttributeSetID = JobHead.AttributeSetID;
                        ttLaborDtl.ScrapAttributeSetID = JobHead.AttributeSetID;
                        ttLaborDtl.TrackInventoryByRevision = partPartial.TrackInventoryByRevision;
                        runLibGetDescriptions(JobHead.AttributeSetID, partPartial.TrackInventoryAttributes, out attributeSetDescription, out attributeSetShortDescription);
                        ttLaborDtl.LaborAttributeSetDescription = attributeSetDescription;
                        ttLaborDtl.LaborAttributeSetShortDescription = attributeSetShortDescription;
                        ttLaborDtl.ScrapAttributeSetDescription = attributeSetDescription;
                        ttLaborDtl.ScrapAttributeSetShortDescription = attributeSetShortDescription;
                        ttLaborDtl.DiscrepAttributeSetDescription = attributeSetDescription;
                        ttLaborDtl.DiscrepAttributeSetShortDescription = attributeSetShortDescription;
                    }

                    JobAsmbl = this.FindFirstJobAsmbl2(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
                    if (JobAsmbl != null)
                    {
                        ttLaborDtl.LaborUOM = JobAsmbl.IUM;
                        ttLaborDtl.ScrapUOM = JobAsmbl.IUM;
                        ttLaborDtl.DiscrepUOM = JobAsmbl.IUM;
                        ttLaborDtl.LaborRevision = JobAsmbl.RevisionNum;
                        ttLaborDtl.ScrapRevision = JobAsmbl.RevisionNum;
                        ttLaborDtl.DiscrepRevision = JobAsmbl.RevisionNum;
                    }
                    else
                    {
                        ttLaborDtl.LaborUOM = JobHead.IUM;
                        ttLaborDtl.ScrapUOM = JobHead.IUM;
                        ttLaborDtl.DiscrepUOM = JobHead.IUM;
                        ttLaborDtl.LaborRevision = JobHead.RevisionNum;
                        ttLaborDtl.ScrapRevision = JobHead.RevisionNum;
                        ttLaborDtl.DiscrepRevision = JobHead.RevisionNum;
                    }
                }
                if (String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID))
                {
                    if (!String.IsNullOrEmpty(ttLaborDtl.ResourceID))
                    {


                        Resource = Resource.FindFirstByPrimaryKey(Db, ttLaborDtl.Company, ttLaborDtl.ResourceID);
                        if (Resource != null)
                        {
                            ttLaborDtl.ResourceGrpID = Resource.ResourceGrpID;
                        }
                    }
                }
                /* validate capability */
                if (!String.IsNullOrEmpty(ttLaborDtl.CapabilityID))
                {
                    this.validateCapabilityID(ttLaborDtl.CapabilityID);


                    Capability = this.FindFirstCapability2(Session.CompanyID, ttLaborDtl.CapabilityID);
                    ttLaborDtl.CapabilityDescription = ((Capability != null) ? Capability.Description : "");
                }
                /* validate and send warning for changed opcode */
                this.validateOpCode(ttLaborDtl.OpCode);
                ExceptionManager.AssertNoBLExceptions();
                this.warnOpCode(ttLaborDtl.OpCode, out vMessage);
                /* SCR #49381 - The assignment of LaborRate should happen before the call of chgWcCode *
                 * to make sure that the BurdenRate is calculated correctly using the new labor rate.  */
                if (JobOper.LaborEntryMethod.Compare("Q") == 0)
                {
                    ttLaborDtl.LaborRate = ((ttLaborDtl.LaborType.Compare("P") == 0) ? JobOper.ProdLabRate : ((ttLaborDtl.LaborType.Compare("S") == 0) ? JobOper.SetupLabRate : ttLaborDtl.LaborRate));
                }

                if (!String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID))
                {
                    /*SCR 89457  Removed chgWcCode when setting Defaults to OprSeq and apply this validation when saving the Labor*/
                    /* validate and send warning for changed wccode */
                    //this.chgWcCode(ttLaborDtl.ResourceGrpID, false, out vMessage);
                    /*SCR 89457  Since chgWcCode was removed, we still need to assign the corresponding*/
                    /*ResourceGrpDescription and JCDept to the ttLaborDtl taken from chgWcCode procedure*/
                    this.validateWcCode(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.ResourceGrpID, "LaborDtl", ttLaborDtl.SysRowID);
                    ttLaborDtl.ResourceGrpDescription = ((ResourceGroup != null) ? ResourceGroup.Description : "");
                    ttLaborDtl.JCDept = ((ResourceGroup != null) ? ResourceGroup.JCDept : "");
                    this.validateJCDept(ttLaborDtl.JCDept);
                    /* SCR 151111 - need to assign the Burden Rate at this point for the default Resource/Group */
                    var outBurdenRate = ttLaborDtl.BurdenRate;
                    this.getLaborDtlBurdenRates(false, out outBurdenRate);
                    ttLaborDtl.BurdenRate = outBurdenRate;
                }
                else if (!String.IsNullOrEmpty(ttLaborDtl.CapabilityID))
                {
                    ttLaborDtl.JCDept = this.getJCDeptFromCapability(ttLaborDtl.CapabilityID);
                }
                /* lost JobOper record - refind */



                JobOper = this.FindFirstJobOper11(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, oprSeq);
                /* SCR #20558 - do not allow rework of Quantity Only operation */
                if (ttLaborDtl.ReWork && JobOper.LaborEntryMethod.Compare("Q") == 0)
                {
                    throw new BLException(Strings.ThisIsAQuantOnlyOperaReworkNotAllowed, "LaborDtl");
                }
                if (ttLaborDtl.LaborTypePseudo.Compare("V") == 0 /* service */ && JobOper.CallNum > 0)
                {
                    ttLaborDtl.LaborQty = JobOper.RunQty - JobOper.QtyCompleted;
                    this.calcTotHrs(ttLaborDtl.LaborHrs);
                }
                if ("P,S".Lookup(ttLaborDtl.LaborType) > -1)
                {
                    this.setComplete(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, oprSeq, ttLaborDtl.LaborQty, ttLaborDtl.ScrapQty);
                }

                ttLaborDtl.CompleteFlag = ((ttLaborDtl.CallNum == 0) ? ttLaborDtl.Complete : ttLaborDtl.FSComplete);
                if (ttLaborDtl.ActiveTrans == true)
                {
                    this.crewCount();
                }

                /* SCR 156781 - If the previous job/asm/opr combination is for QuantityOnly/Fixed Hours labor type then   *
                 * we have to reset the ISFixHoursAndQtyOnly flag and default the correct labor/burden hours based on the *
                 * clock in/out times if the new combination is not fixed hours. Otherwise use the estimated prod hours.  */
                /* Only reset the default hours using the clockin/out times if previously QuantityOnly/FixedHours.        */
                if (ttLaborDtl.ISFixHourAndQtyOnly == true)
                {
                    ttLaborDtl.ISFixHourAndQtyOnly = false; /* needs to reset before calling payHoursDtl() */
                    this.payHoursDtl(true, true, true, out vTest);
                }
                ttLaborDtl.ISFixHourAndQtyOnly = (JobOper.LaborEntryMethod.KeyEquals("Q") && JobOper.StdFormat.KeyEquals("HR"));
                /* if the new OprSeq has QuantityOnly/FixedHours setting then default the hours based on estimated production hours */
                if (ttLaborDtl.ISFixHourAndQtyOnly == true)
                {
                    ttLaborDtl.LaborHrs = this.CalcProdFixedHours(ttLaborDtl.LaborType, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                    ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
                }
                /* Operation Role Codes */
                this.commonOprSeq(ttLaborDtl.EmployeeNum, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, oprSeq, out dfltRoleCd);
                ttLaborDtl.RoleCd = dfltRoleCd;
                this.disPrjFields(true);
                LaborDtl_Foreign_Link();
                /* SCR #20234 - check shop warnings at this point if created through Office MES or from Shop Floor */
                if (ttLaborDtl.ActiveTrans && ttLaborDtl.LaborCollection)
                {
                    this.chkStartActShopWarn(out vMessage);
                }
                ttLaborDtl.ReportPartQtyAllowed = ReportPartQtyAllowed(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                ttLaborDtl.EnableLaborQty = ((this.ReportPartQtyAllowed(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq)) ? false : true);
                /*If labor LaborQty got disabled, reset Qty to Zero so Qty not be processed after field changed from enabled modifield then disabled.*/
                if (!ttLaborDtl.EnableLaborQty)
                {
                    ttLaborDtl.LaborQty = 0;
                    ttLaborDtl.ScrapQty = 0;
                    ttLaborDtl.DiscrepQty = 0;
                    ttLaborDtl.EnableScrapQty = false;
                    ttLaborDtl.EnableDiscrepQty = false;
                }
                else
                {
                    EmpBasic = this.FindFirstEmpBasic8(Session.CompanyID, ttLaborDtl.EmployeeNum);
                    if (EmpBasic != null)
                    {

                        ttLaborDtl.EnableScrapQty = (!ttLaborDtl.LaborEntryMethod.KeyEquals("X")) && EmpBasic.CanReportScrapQty && !ttLaborDtl.ReportPartQtyAllowed;
                        ttLaborDtl.EnableDiscrepQty = (!ttLaborDtl.LaborEntryMethod.KeyEquals("X")) && (EmpBasic.CanReportNCQty && Session.ModuleLicensed(Erp.License.ErpLicensableModules.QualityAssurance) == true) && !ttLaborDtl.ReportPartQtyAllowed;
                    }
                    else
                    {
                        ttLaborDtl.EnableScrapQty = false;
                        ttLaborDtl.EnableDiscrepQty = false;
                    }
                }

                ttLaborDtl.RoleCdList = getRoleCdList(ttLaborDtl.EmployeeNum,
                                                      ttLaborDtl.ProjectID,
                                                      ttLaborDtl.PhaseID,
                                                      ttLaborDtl.JobNum,
                                                      ttLaborDtl.AssemblySeq,
                                                      ttLaborDtl.OprSeq);
            }/* AVAILABLE TTLABORDTL */
            /******************************************/
            /* Set default values on ttTimeWeeklyView */
            /******************************************/
            if (ttTimeWeeklyView != null)
            {
                this.validateJobOper(ttTimeWeeklyView.LaborType, ttTimeWeeklyView.JobNum, ttTimeWeeklyView.AssemblySeq, oprSeq);
                ExceptionManager.AssertNoBLExceptions();
                ttTimeWeeklyView.OprSeq = oprSeq;
                string dummyCharVar = string.Empty;
                var outResourceGrpID2 = ttTimeWeeklyView.ResourceGrpID;
                var outResourceID2 = ttTimeWeeklyView.ResourceID;
                this.defaultOprSeq2(ttTimeWeeklyView.LaborType, ref outResourceGrpID2, ref outResourceID2, ref dummyCharVar);
                ttTimeWeeklyView.ResourceGrpID = outResourceGrpID2;
                ttTimeWeeklyView.ResourceID = outResourceID2;
                ttTimeWeeklyView.OpCode = JobOper.OpCode;
                ttTimeWeeklyView.OpComplete = JobOper.OpComplete;
                ttTimeWeeklyView.LaborEntryMethod = JobOper.LaborEntryMethod;
                if (String.IsNullOrEmpty(ttTimeWeeklyView.ResourceGrpID))
                {
                    if (!String.IsNullOrEmpty(ttTimeWeeklyView.ResourceID))
                    {


                        Resource = Resource.FindFirstByPrimaryKey(Db, ttTimeWeeklyView.Company, ttTimeWeeklyView.ResourceID);
                        if (Resource != null)
                        {
                            ttTimeWeeklyView.ResourceGrpID = Resource.ResourceGrpID;
                        }
                    }
                }
                /* validate and send warning for changed opcode */
                this.validateOpCode(ttTimeWeeklyView.OpCode);
                ExceptionManager.AssertNoBLExceptions();
                if (JobOper.LaborEntryMethod.Compare("Q") == 0)
                {
                    ttTimeWeeklyView.LaborRate = ((ttTimeWeeklyView.LaborType.Compare("P") == 0) ? JobOper.ProdLabRate : ((ttTimeWeeklyView.LaborType.Compare("S") == 0) ? JobOper.SetupLabRate : ttTimeWeeklyView.LaborRate));
                }
                /* lost JobOper record - refind */

                if (!String.IsNullOrEmpty(ttTimeWeeklyView.ResourceGrpID))
                {
                    this.validateWcCode(ttTimeWeeklyView.Company, ttTimeWeeklyView.JobNum, ttTimeWeeklyView.ResourceGrpID, "TimeWeeklyView", ttTimeWeeklyView.SysRowID);
                    ttTimeWeeklyView.ResourceGrpIDDescription = ((ResourceGroup != null) ? ResourceGroup.Description : "");
                    ttTimeWeeklyView.JCDept = ((ResourceGroup != null) ? ResourceGroup.JCDept : "");
                    this.validateJCDept(ttTimeWeeklyView.JCDept);
                }


                JobOper = this.FindFirstJobOper12(Session.CompanyID, ttTimeWeeklyView.JobNum, ttTimeWeeklyView.AssemblySeq, oprSeq);
                ttTimeWeeklyView.OprSeq = oprSeq;
                /* Operation Role Codes */
                this.commonOprSeq(ttTimeWeeklyView.EmployeeNum, ttTimeWeeklyView.JobNum, ttTimeWeeklyView.AssemblySeq, ttTimeWeeklyView.OprSeq, out dfltRoleCd);
                ttTimeWeeklyView.RoleCd = dfltRoleCd;
                ttTimeWeeklyView.RoleCdList = getRoleCdList(ttTimeWeeklyView.EmployeeNum,
                                                            ttTimeWeeklyView.ProjectID,
                                                            ttTimeWeeklyView.PhaseID,
                                                            ttTimeWeeklyView.JobNum,
                                                            ttTimeWeeklyView.AssemblySeq,
                                                            ttTimeWeeklyView.OprSeq);
                this.disPrjFieldsTimeWeeklyView();
                TimeWeeklyView_Foreign_Link();
            }
        }

        /// <summary>
        /// To default a ResourceGrpID, Resource and CapablityID when the Operation Sequence changes.
        /// Called from public method DefaultOprSeq.
        /// </summary>
        private void defaultOprSeq2(string ip_LaborType, ref string io_ResourceGrpID, ref string io_ResourceID, ref string io_CapabilityID, int ipOpDtlSeq = 0)
        {
            Guid JobOpDtlRowid = Guid.Empty;
            bool lRecordFound = false;
            /* Find the first JobOpDtl record that has a resource group or resource
               that is a location.  This is where the default ResourceGrpID and
               ResourceID values for LaborDtl should come from. 
            */
            /* 1st TRY USING THE PRIMARY JOBOPDTL - IF IT'S A LOCATION */
            int tmp_OpDtlSeq = 0;
            if (ipOpDtlSeq == 0) // Get Primary OpDtl if nothing was passed in ipOpDtlSeq
            {
                /* setup */
                if (ip_LaborType.Compare("S") == 0)
                {
                    tmp_OpDtlSeq = JobOper.PrimarySetupOpDtl;
                }
                else
                {
                    tmp_OpDtlSeq = JobOper.PrimaryProdOpDtl;
                }
            }
            else
            {
                tmp_OpDtlSeq = ipOpDtlSeq;
            }



            //JobOpDtl_LOOP1:
            JobOpDtl = this.FindFirstJobOpDtl2(JobOper.Company, JobOper.JobNum, JobOper.AssemblySeq, JobOper.OprSeq, tmp_OpDtlSeq);
            if (JobOpDtl != null)
            {
                if (this.isLocation("ResourceGroup", JobOpDtl.ResourceGrpID) == true ||
                this.isLocation("Resource", JobOpDtl.ResourceID) == true ||
                this.getLocationByCapability(JobOpDtl.CapabilityID) == true)
                {
                    lRecordFound = true;
                    JobOpDtlRowid = JobOpDtl.SysRowID;
                }
            }/* JobOpDtl_LOOP1 */
            /* PRIMARY JOBOPDTL DID NOT PROVIDE DEFAULT, NOW TRY RELATED JOBOPDTL DEFINED FOR TYPE OF PRODUCTION WHICH IS A LOCATION */
            if (lRecordFound == false)
            {
                if (ipOpDtlSeq > 0) return;

                //JobOpDtl_LOOP2:
                foreach (var JobOpDtl_iterator in (this.SelectJobOpDtl(JobOper.Company, JobOper.JobNum, JobOper.AssemblySeq, JobOper.OprSeq, "B", ip_LaborType)))
                {
                    JobOpDtl = JobOpDtl_iterator;
                    if (this.isLocation("ResourceGroup", JobOpDtl.ResourceGrpID) == true ||
                    this.isLocation("Resource", JobOpDtl.ResourceID) == true ||
                    this.getLocationByCapability(JobOpDtl.CapabilityID) == true)
                    {
                        lRecordFound = true;
                        JobOpDtlRowid = JobOpDtl.SysRowID;
                        break;
                    }
                }
            }/* if lRecordFound = false */

            if (lRecordFound == true)
            {


                JobOpDtl = this.FindFirstJobOpDtl(JobOpDtlRowid);
                /* find scheduled RTU to get resource id */



                ResourceTimeUsed = this.FindFirstResourceTimeUsed(JobOpDtl.Company, JobOpDtl.JobNum, JobOpDtl.AssemblySeq, JobOpDtl.OprSeq, JobOpDtl.OpDtlSeq, false, 1);
                /*  SCR 46410 not allow selecting default ResourceID as   */
                /*  ResourceTimeUsed.ResourceID or JobOpDtl.ResourceID,   */
                /*  if this Resource doesn't exists or is Inactive or     */
                /*  is not Location at that moment                        */
                if (ResourceTimeUsed != null)
                {


                    Resource = Resource.FindFirstByPrimaryKey(Db, ResourceTimeUsed.Company, ResourceTimeUsed.ResourceID);
                }
                else
                {


                    Resource = Resource.FindFirstByPrimaryKey(Db, JobOpDtl.Company, JobOpDtl.ResourceID);
                }
                io_ResourceGrpID = ((ResourceTimeUsed != null) ? ResourceTimeUsed.ResourceGrpID : JobOpDtl.ResourceGrpID);
                io_ResourceID = ((Resource != null && Resource.Inactive == false && Resource.Location == true) ? Resource.ResourceID : "");
                io_CapabilityID = JobOpDtl.CapabilityID;
            }
        }

        /// <summary>
        /// Call GetNewLaborDtl base method then assign selected values and default values for MES/Work Queue/Select for Work.
        /// ResourceID is defaulted the same way than for MES- Start Production Activity. ResourceID must be required only if Company Configuration MachinePrompt is true, otherwise is optional.
        /// </summary>
        public void GetNewLaborDtlOnSelectForWork(ref LaborTableset ds, int laborHedSeq, string sJobNum, int iAssemblySeq, int iOprSeq, string sResourceGrpID, string setupOrProd, out bool bMachinePrompt)
        {
            this.GetNewLaborDtl(ref ds, laborHedSeq);

            Erp.Tablesets.LaborDtlRow LaborDtlNewRow;
            LaborDtlNewRow = ds.LaborDtl[ds.LaborDtl.Count - 1];
            LaborDtlNewRow.JobNum = sJobNum;
            LaborDtlNewRow.AssemblySeq = iAssemblySeq;
            LaborDtlNewRow.OprSeq = iOprSeq;
            LaborDtlNewRow.ResourceGrpID = sResourceGrpID;
            LaborDtlNewRow.ResourceID = "";

            JCSyst = this.FindFirstJCSyst(Session.CompanyID);
            bMachinePrompt = JCSyst.MachinePrompt;

            JobOpDtl = this.FindFirstJobOpDtl6(LaborDtlNewRow.Company, LaborDtlNewRow.JobNum, LaborDtlNewRow.AssemblySeq, LaborDtlNewRow.OprSeq, sResourceGrpID, "B", setupOrProd);

            if (JobOpDtl != null)
            {
                if (this.isLocation("ResourceGroup", JobOpDtl.ResourceGrpID) == true
                        || this.isLocation("Resource", JobOpDtl.ResourceID) == true
                        || this.getLocationByCapability(JobOpDtl.CapabilityID) == true)
                {
                    ResourceTimeUsed = this.FindFirstResourceTimeUsed(JobOpDtl.Company, JobOpDtl.JobNum, JobOpDtl.AssemblySeq, JobOpDtl.OprSeq, JobOpDtl.OpDtlSeq, false, 1);

                    if (ResourceTimeUsed != null)
                    {
                        Resource = Resource.FindFirstByPrimaryKey(Db, ResourceTimeUsed.Company, ResourceTimeUsed.ResourceID);
                    }
                    else
                    {
                        Resource = Resource.FindFirstByPrimaryKey(Db, JobOpDtl.Company, JobOpDtl.ResourceID);
                    }
                    LaborDtlNewRow.ResourceID = ((Resource != null && Resource.Inactive == false && Resource.Location == true) ? Resource.ResourceID : "");
                }
            }
        }

        /// <summary>
        /// This method defaults dataset fields when the PhaseID field changes.
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ipPhaseID">Proposed PhaseID change </param>
        public void DefaultPhaseID(ref LaborTableset ds, string ipPhaseID)
        {
            CurrentFullTableset = ds;
            if (ttLaborDtl == null)
            {
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttLaborDtl == null && ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl", "RowMod");
            }
            /************************************/
            /* Set default values on ttLaborDtl */
            /************************************/
            if (ttLaborDtl != null)
            {


                ProjPhase = this.FindFirstProjPhase(Session.CompanyID, ttLaborDtl.ProjectID, ipPhaseID);
                if (ProjPhase == null)
                {
                    throw new BLException(Strings.InvalidProjectPhase, "LaborDtl", "PhaseID");
                }
                ttLaborDtl.PhaseID = ipPhaseID;
                ttLaborDtl.ProjPhaseID = ttLaborDtl.ProjectID + Ice.Constants.LIST_DELIM + ipPhaseID;
                ttLaborDtl.PhaseJobNum = ProjPhase.WBSJobNum;
                ttLaborDtl.PhaseOprSeq = 0;
                ttLaborDtl.RoleCd = "";
                ttLaborDtl.RoleCdRoleDescription = "";
                ttLaborDtl.TimeTypCd = "";
                ttLaborDtl.TimeTypCdDescription = "";
                this.DefaultJobNum(ref ds, ttLaborDtl.PhaseJobNum);
                this.DefaultAssemblySeq(ref ds, 0);
                /* Set DisPrjRoleCd and DisTimeTypCd */
                this.disPrjFields(true);
                LaborDtl_Foreign_Link();
            }
            /******************************************/
            /* Set default values on ttTimeWeeklyView */
            /******************************************/
            if (ttTimeWeeklyView != null)
            {
                ttTimeWeeklyView.PhaseID = ipPhaseID;


                ProjPhase = this.FindFirstProjPhase2(Session.CompanyID, ttTimeWeeklyView.ProjectID, ipPhaseID);
                this.DefaultJobNum(ref ds, ProjPhase.WBSJobNum);
                this.DefaultAssemblySeq(ref ds, 0);
                /* Set DisPrjRoleCd and DisTimeTypCd */
                this.disPrjFieldsTimeWeeklyView();
                TimeWeeklyView_Foreign_Link();
            }
        }

        /// <summary>
        /// This method defaults dataset fields when the PhaseOprSeq field changes.
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ipPhaseOprSeq">Proposed PhaseOprSeq change </param>
        /// <param name="vMessage">Returns warnings for user's review</param>
        public void DefaultPhaseOprSeq(ref LaborTableset ds, int ipPhaseOprSeq, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl", "RowMod");
            }



            JobOper = this.FindFirstJobOper13(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ipPhaseOprSeq);
            if (JobOper == null)
            {
                throw new BLException(Strings.InvalidJobOperationSequence, "LaborDtl", "PhaseOprSeq");
            }
            ttLaborDtl.PhaseOprSeq = JobOper.OprSeq;
            this.DefaultOprSeq(ref ds, ttLaborDtl.PhaseOprSeq, out vMessage);
        }

        /// <summary>
        /// This method defaults dataset fields when the ProjectID field changes.
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ipProjectID">Proposed ProjectID change </param>
        public void DefaultProjectID(ref LaborTableset ds, string ipProjectID)
        {
            CurrentFullTableset = ds;
            if (ttLaborDtl == null)
            {
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttLaborDtl == null && ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborHasNotChanged, "LaborDtl", "RowMod");
            }
            /************************************/
            /* Set default values on ttLaborDtl */
            /************************************/
            if (ttLaborDtl != null)
            {


                Project = this.FindFirstProject(Session.CompanyID, ipProjectID);
                if (Project == null)
                {
                    throw new BLException(Strings.InvalidProject, "LaborDtl", "ProjectID");
                }
                if (!Project.ActiveProject)
                {
                    throw new BLException(Strings.ProjectMustBeActive, "LaborDtl", "ProjectID");
                }
                ttLaborDtl.ProjectID = Project.ProjectID;
                ttLaborDtl.PhaseID = "";
                ttLaborDtl.PhaseJobNum = "";
                ttLaborDtl.PhaseOprSeq = 0;
                ttLaborDtl.JobNum = "";
                ttLaborDtl.AssemblySeq = 0;
                ttLaborDtl.OprSeq = 0;
                ttLaborDtl.PhaseIDDescription = "";
            }
            /******************************************/
            /* Set default values on ttTimeWeeklyView */
            /******************************************/
            if (ttTimeWeeklyView != null)
            {


                Project = this.FindFirstProject2(Session.CompanyID, ipProjectID);
                if (Project == null)
                {
                    throw new BLException(Strings.InvalidProject, "TimeWeeklyView", "ProjectID");
                }
                if (!Project.ActiveProject)
                {
                    throw new BLException(Strings.ProjectMustBeActive, "TimeWeeklyView", "ProjectID");
                }
                ttTimeWeeklyView.ProjectID = Project.ProjectID;
                ttTimeWeeklyView.PhaseID = "";
                ttTimeWeeklyView.JobNum = "";
                ttTimeWeeklyView.AssemblySeq = 0;
                ttTimeWeeklyView.OprSeq = 0;
                ttTimeWeeklyView.PhaseIDDescription = "";
                ttTimeWeeklyView.WBSPhaseDesc = "";
            }
        }

        /// <summary>
        /// This method updates dataset fields when the ResourceID field changes.
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="ProposedResourceID">The proposed resource id</param>
        public void DefaultResourceID(ref LaborTableset ds, string ProposedResourceID)
        {
            CurrentFullTableset = ds;
            if (ttLaborDtl == null)
            {
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttLaborDtl == null && ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborHasNotChanged, "LaborDtl");
            }
            if (ttLaborDtl != null)
            {


                Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, ProposedResourceID);
                if (Resource == null)
                {
                    throw new BLException(Strings.InvalidResource, "LaborDtl", "ResourceID");
                }
                if (!String.IsNullOrEmpty(ttLaborDtl.CapabilityID))
                {


                    CapResLnk = this.FindFirstCapResLnk(Session.CompanyID, ttLaborDtl.CapabilityID, ProposedResourceID);
                    if (CapResLnk == null)
                    {
                        throw new BLException(Strings.InvalidResourceForCapability, "LaborDtl", "ResourceID");
                    }
                }
                saveResourceID = ttLaborDtl.ResourceID;
                saveResourceGrpID = ttLaborDtl.ResourceGrpID;
                ttLaborDtl.ResourceID = ProposedResourceID;
                ttLaborDtl.ResourceGrpID = Resource.ResourceGrpID;



                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, ttLaborDtl.ResourceGrpID);
                if (ResourceGroup != null)
                {
                    ttLaborDtl.JCDept = ResourceGroup.JCDept;
                }
                var outBurdenRate2 = ttLaborDtl.BurdenRate;
                this.getLaborDtlBurdenRates(true, out outBurdenRate2);
                ttLaborDtl.BurdenRate = outBurdenRate2;
                LaborDtl_Foreign_Link();
            }
            if (ttTimeWeeklyView != null)
            {


                Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, ProposedResourceID);
                if (Resource == null)
                {
                    throw new BLException(Strings.InvalidResource, "LaborDtl", "ResourceID");
                }
                saveResourceID = ttTimeWeeklyView.ResourceID;
                saveResourceGrpID = ttTimeWeeklyView.ResourceGrpID;
                ttTimeWeeklyView.ResourceID = ProposedResourceID;
                ttTimeWeeklyView.ResourceGrpID = Resource.ResourceGrpID;



                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, ttTimeWeeklyView.ResourceGrpID);
                if (ResourceGroup != null)
                {
                    ttTimeWeeklyView.JCDept = ResourceGroup.JCDept;
                }
            }
        }

        /// <summary>
        /// This method defaults dataset fields when the RoleCd field changes.
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ipRoleCd">Proposed RoleCd change </param>
        public void DefaultRoleCd(ref LaborTableset ds, string ipRoleCd)
        {
            CurrentFullTableset = ds;
            string roleCodeList = string.Empty;
            if (ttLaborDtl == null)
            {
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttLaborDtl == null && ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborHasNotChanged, "LaborDtl", "RowMod");
            }
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) && !String.IsNullOrEmpty(ipRoleCd))
            {
                throw new BLException(Strings.RoleCodeCannotBeEnteredProjectBillingIsNotLicen, "LaborDtl", "RoleCd");
            }
            /************************************/
            /* Set default values on ttLaborDtl */
            /************************************/
            if (ttLaborDtl != null)
            {
                if (String.IsNullOrEmpty(ipRoleCd))
                {
                    ttLaborDtl.RoleCd = "";
                    ttLaborDtl.TimeTypCd = "";
                    ttLaborDtl.TimeTypCdDescription = "";
                    ttLaborDtl.DisTimeTypCd = true;
                    return;
                }
                /* indirect */
                if (ttLaborDtl.LaborType.Compare("I") == 0)
                {
                    throw new BLException(Strings.RoleCodeCannotBeEnteredForIndirLabor, "LaborDtl", "RoleCd");
                }



                JobHead = this.FindFirstJobHead7(ttLaborDtl.Company, ttLaborDtl.JobNum);
                if (JobHead == null)
                {
                    throw new BLException(Strings.JobHeadIsNotAvailable, "LaborDtl", "JobNum");
                }      /* gets the valid role codes based on company flags, project, job, operation, etc */
                roleCodeList = this.buildValidRoleCodeList(ttLaborDtl.EmployeeNum, JobHead.ProjectID, JobHead.PhaseID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                if (roleCodeList.Lookup(ipRoleCd, Ice.Constants.LIST_DELIM[0]) <= -1)
                {
                    throw new BLException(Strings.RoleCodeIsNotValidForThisEmploAndOpera, "LaborDtl", "RoleCd");
                }
                ttLaborDtl.RoleCd = ipRoleCd;
                ttLaborDtl.DisTimeTypCd = false;
                ttLaborDtl.TimeTypCd = "";
                ttLaborDtl.TimeTypCdDescription = "";
                LibProjectCommon.getWBSPhaseMethods(JobHead.ProjectID, JobHead.PhaseID, out vInvMethod, out vRevMethod);
                if (vInvMethod.Compare("TM") != 0)
                {
                    ttLaborDtl.DisTimeTypCd = true;
                }
            }
            /******************************************/
            /* Set default values on ttTimeWeeklyView */
            /******************************************/
            if (ttTimeWeeklyView != null)
            {
                if (String.IsNullOrEmpty(ipRoleCd))
                {
                    ttTimeWeeklyView.RoleCd = "";
                    ttTimeWeeklyView.TimeTypCd = "";
                    ttTimeWeeklyView.TimeTypCdDescription = "";
                    ttTimeWeeklyView.DisTimeTypCd = true;
                    return;
                }
                /* indirect */
                if (ttTimeWeeklyView.LaborType.Compare("I") == 0)
                {
                    throw new BLException(Strings.RoleCodeCannotBeEnteredForIndirLabor, "LaborDtl", "RoleCd");
                }



                JobHead = this.FindFirstJobHead8(ttTimeWeeklyView.Company, ttTimeWeeklyView.JobNum);
                if (JobHead == null)
                {
                    throw new BLException(Strings.JobHeadIsNotAvailable, "LaborDtl", "JobNum");
                }
                roleCodeList = this.buildValidRoleCodeList(ttTimeWeeklyView.EmployeeNum, JobHead.ProjectID, JobHead.PhaseID, ttTimeWeeklyView.JobNum, ttTimeWeeklyView.AssemblySeq, ttTimeWeeklyView.OprSeq);
                if (roleCodeList.Lookup(ipRoleCd, Ice.Constants.LIST_DELIM[0]) <= -1)
                {
                    throw new BLException(Strings.RoleCodeIsNotValidForThisEmploAndOpera, "LaborDtl", "RoleCd");
                }
                ttTimeWeeklyView.RoleCd = ipRoleCd;
                ttTimeWeeklyView.DisTimeTypCd = false;
                ttTimeWeeklyView.TimeTypCd = "";
                ttTimeWeeklyView.TimeTypCdDescription = "";
                LibProjectCommon.getWBSPhaseMethods(JobHead.ProjectID, JobHead.PhaseID, out vInvMethod, out vRevMethod);
                if (vInvMethod.Compare("TM") != 0)
                {
                    ttTimeWeeklyView.DisTimeTypCd = true;
                }
            }
        }

        /// <summary>
        /// This method defaults fields when the scrap reason code fields changes.  Also checks
        /// for any labor warnings the user needs to be aware of
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ProposedScrapReasonCode">Proposed scrap reason </param>
        /// <param name="vMessage">Returns a string of warnings user needs to know</param>
        public void DefaultScrapReasonCode(ref LaborTableset ds, string ProposedScrapReasonCode, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }



            Reason = this.FindFirstReason2(Session.CompanyID, "S", ProposedScrapReasonCode);
            if (Reason == null)
            {
                throw new BLException(Strings.InvalidScrapReasonCode, "LaborDtl", "ScrapReasonCode");
            }
            ttLaborDtl.ScrapReasonCode = ProposedScrapReasonCode;
            vMessage = "";
            this.warnScrapReasonCode(ref vMessage);
            LaborDtl_Foreign_Link();
        }

        /// <summary>
        /// This method validates and reassigns the setup percent complete field.
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="PercentComplete">Proposed percent complete </param>
        public void DefaultSetupPctComplete(ref LaborTableset ds, decimal PercentComplete)
        {
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }
            if (PercentComplete > 100)
            {
                throw new BLException(Strings.ThePerceCannotBeGreaterThan, "LaborDtl", "SetupPctComplete");
            }



            JobOper = this.FindFirstJobOper14(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
            if (JobOper != null &&
            PercentComplete < JobOper.SetupPctComplete)
            {
                ttLaborDtl.SetupPctComplete = JobOper.SetupPctComplete;
            }
            else
            {
                ttLaborDtl.SetupPctComplete = Compatibility.Convert.ToInt32(PercentComplete);
            }
        }

        /// <summary>
        /// This method updates clock in/out and lunch in/out fields after shift field changes
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="shift">Proposed Shift field change </param>
        public void DefaultShift(ref LaborTableset ds, int shift)
        {
            CurrentFullTableset = ds;
            if (ttLaborHed == null)
            {
                ttLaborHed = (from ttLaborHed_Row in ds.LaborHed
                              where modList.Lookup(ttLaborHed_Row.RowMod) != -1
                              select ttLaborHed_Row).FirstOrDefault();
            }

            if (ttLaborDtl == null)
            {
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttLaborDtl == null && ttTimeWeeklyView == null && ttLaborHed == null)
            {
                throw new BLException(Strings.LaborHeaderHasNotChanged, "LaborHed");
            }
            if (ttLaborHed != null)
            {


                JCShift = this.FindFirstJCShift4(Session.CompanyID, shift);
                if (JCShift == null)
                {
                    throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.Shift), "LaborHed");
                }
                /* validate times */


                ttLaborHed.ActualClockInTime = JCShift.StartTime;
                ttLaborHed.ActualClockOutTime = JCShift.EndTime;
                ttLaborHed.ActLunchOutTime = JCShift.LunchStart;
                ttLaborHed.ActLunchInTime = JCShift.LunchEnd;
                ttLaborHed.ClockInTime = JCShift.StartTime;
                ttLaborHed.ClockOutTime = JCShift.EndTime;
                ttLaborHed.LunchOutTime = JCShift.LunchStart;
                ttLaborHed.LunchInTime = JCShift.LunchEnd;
                if (ttJCSyst.ClockFormat.Compare("M") == 0)
                {
                    ttLaborHed.DspClockInTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborHed.ClockInTime), "99") + ":" + Compatibility.Convert.ToString(((ttLaborHed.ClockInTime - Math.Truncate(ttLaborHed.ClockInTime)) * 60), "99");
                    ttLaborHed.DspClockOutTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborHed.ClockOutTime), "99") + ":" + Compatibility.Convert.ToString(((ttLaborHed.ClockOutTime - Math.Truncate(ttLaborHed.ClockOutTime)) * 60), "99");
                }
                else
                {
                    ttLaborHed.DspClockInTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborHed.ClockInTime), "99") + Compatibility.Convert.ToString((ttLaborHed.ClockInTime - Math.Truncate(ttLaborHed.ClockInTime)), ".99");
                    ttLaborHed.DspClockOutTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborHed.ClockOutTime), "99") + Compatibility.Convert.ToString((ttLaborHed.ClockOutTime - Math.Truncate(ttLaborHed.ClockOutTime)), ".99");
                }
                this.payHours();
                ttLaborHed.Shift = shift;
                ttLaborHed.ShiftDescription = JCShift.Description;
                if (ttLaborHed.ActLunchInTime != 0 || ttLaborHed.LunchOutTime != 0)
                {
                    ttLaborHed.LunchBreak = true;
                }
                else
                {
                    ttLaborHed.LunchBreak = false;
                }

                this.setdispValue();
            }
            if (ttLaborDtl != null)
            {


                JCShift = this.FindFirstJCShift4(Session.CompanyID, shift);
                if (JCShift == null)
                {
                    throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.Shift), "LaborDtl");
                }
                ttLaborDtl.Shift = shift;
            }
            if (ttTimeWeeklyView != null)
            {


                JCShift = this.FindFirstJCShift4(Session.CompanyID, shift);
                if (JCShift == null)
                {
                    throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.Shift), "LaborDtl");
                }
                ttTimeWeeklyView.Shift = shift;
                ttTimeWeeklyView.ShiftDescription = JCShift.Description;
            }
        }

        /// <summary>
        /// This method updates time and pay hours when a time field changes
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="cFieldName">name of field being changed</param>
        /// <param name="timeValue">proposed time change </param>
        public void DefaultTime(ref LaborTableset ds, string cFieldName, decimal timeValue)
        {
            CurrentFullTableset = ds;
            decimal tmpTime = decimal.Zero;


            ttLaborHed = (from ttLaborHed_Row in ds.LaborHed
                          where modList.Lookup(ttLaborHed_Row.RowMod) != -1
                          select ttLaborHed_Row).FirstOrDefault();
            if (ttLaborHed == null)
            {
                ExceptionManager.AddBLException(Strings.LaborHeaderHasNotChanged, "LaborHed");
            }
            /* validate times */

            ExceptionManager.AssertNoBLExceptions();
            switch (cFieldName.ToUpperInvariant())
            {
                case "CLOCKINTIME":
                    {
                        tmpTime = ttLaborHed.ClockInTime;
                        ttLaborHed.ClockInTime = timeValue;
                    }
                    break;
                case "CLOCKOUTTIME":
                    {
                        if (timeValue == 0)
                        {
                            timeValue = 24.0m;
                        }

                        tmpTime = ttLaborHed.ClockOutTime;
                        ttLaborHed.ClockOutTime = timeValue;
                    }
                    break;
                case "ACTUALCLOCKINTIME":
                    {
                        if (ttLaborHed.ClockInTime == 0)
                        {
                            ttLaborHed.ClockInTime = timeValue;
                        }

                        tmpTime = ttLaborHed.ActualClockInTime;
                        ttLaborHed.ActualClockInTime = timeValue;
                    }
                    break;
                case "ACTUALCLOCKOUTTIME":
                    {
                        if (timeValue == 0)
                        {
                            timeValue = 24.0m;
                        }

                        if (ttLaborHed.ClockOutTime == 0)
                        {
                            ttLaborHed.ClockOutTime = timeValue;
                        }

                        tmpTime = ttLaborHed.ActualClockOutTime;
                        ttLaborHed.ActualClockOutTime = timeValue;
                    }
                    break;
                case "LUNCHINTIME":
                    {
                        if (timeValue == 0)
                        {
                            timeValue = 24.0m;
                        }

                        tmpTime = ttLaborHed.LunchInTime;
                        ttLaborHed.LunchInTime = timeValue;
                    }
                    break;
                case "LUNCHOUTTIME":
                    {
                        tmpTime = ttLaborHed.LunchOutTime;
                        ttLaborHed.LunchOutTime = timeValue;
                    }
                    break;
                case "ACTLUNCHINTIME":
                    {
                        if (timeValue == 0)
                        {
                            timeValue = 24.0m;
                        }

                        if (ttLaborHed.LunchInTime == 0)
                        {
                            ttLaborHed.LunchInTime = timeValue;
                        }

                        tmpTime = ttLaborHed.ActLunchInTime;
                        ttLaborHed.ActLunchInTime = timeValue;
                    }
                    break;
                case "ACTLUNCHOUTTIME":
                    {
                        if (ttLaborHed.LunchOutTime == 0)
                        {
                            ttLaborHed.LunchOutTime = timeValue;
                        }

                        tmpTime = ttLaborHed.ActLunchOutTime;
                        ttLaborHed.ActLunchOutTime = timeValue;
                    }
                    break;
            }
            this.payHours();
            switch (cFieldName.ToUpperInvariant())
            {
                case "ACTLUNCHINTIME":
                    {
                        if (ttLaborHed.ActLunchInTime == 24.0m)
                        {
                            ttLaborHed.ActLunchInTime = 0;
                            ttLaborHed.LunchInTime = 0;
                        }
                    }
                    break;
                case "LUNCHINTIME":
                    {
                        if (ttLaborHed.LunchInTime == 24.0m)
                        {
                            ttLaborHed.LunchInTime = 0;
                        }
                    }
                    break;
                case "CLOCKOUTTIME":
                    {
                        if (ttLaborHed.ClockOutTime == 24.0m)
                        {
                            ttLaborHed.ClockOutTime = 0;
                        }
                    }
                    break;
                case "ACTUALCLOCKOUTTIME":
                    {
                        if (ttLaborHed.ActualClockOutTime == 24.0m)
                        {
                            ttLaborHed.ActualClockOutTime = 0;
                            ttLaborHed.ClockOutTime = 0;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// This method defaults dataset fields when the TimeTypCd field changes.
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ipTimeTypCd">Proposed TimeTypCd change </param>
        /// <param name="vMessage">Returns warnings the user needs to review </param>
        public void DefaultTimeTypCd(ref LaborTableset ds, string ipTimeTypCd, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;
            if (ttLaborDtl == null)
            {
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttLaborDtl == null && ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborHasNotChanged, "LaborDtl", "RowMod");
            }
            if (ttLaborDtl != null)
            {
                if (String.IsNullOrEmpty(ipTimeTypCd))
                {
                    ttLaborDtl.TimeTypCd = "";
                    return;
                }
                if (ttLaborDtl.LaborType.Compare("I") == 0)
                {
                    throw new BLException(Strings.TimeTypeCodeCannotBeEnteredForIndirLabor, "LaborDtl", "TimeTypCd");
                }
                vMessage = this.validateTimeTypCd(ttLaborDtl.JobNum, ttLaborDtl.RoleCd, ipTimeTypCd, ttLaborDtl.EmployeeNum);
                ttLaborDtl.TimeTypCd = ipTimeTypCd;
            }
            if (ttTimeWeeklyView != null)
            {
                if (String.IsNullOrEmpty(ipTimeTypCd))
                {
                    ttTimeWeeklyView.TimeTypCd = "";
                    return;
                }
                if (ttTimeWeeklyView.LaborType.Compare("I") == 0)
                {
                    throw new BLException(Strings.TimeTypeCodeCannotBeEnteredForIndirLabor, "LaborDtl", "TimeTypCd");
                }
                vMessage = this.validateTimeTypCd(ttTimeWeeklyView.JobNum, ttTimeWeeklyView.RoleCd, ipTimeTypCd, ttTimeWeeklyView.EmployeeNum);
                ttTimeWeeklyView.TimeTypCd = ipTimeTypCd;
            }
        }

        /// <summary>
        /// This method updates dataset fields when the ResourceGroup field changes.  Also checks
        /// for any warning the user needs to know
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="wcCode">Proposed WorkCenter Code change</param>
        /// <param name="vMessage">Returns any warnings the user needs to review</param>
        public void DefaultWCCode(ref LaborTableset ds, string wcCode, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }
            saveResourceID = ttLaborDtl.ResourceID;
            saveResourceGrpID = ttLaborDtl.ResourceGrpID;
            /* moved assign before chgWCCode because BurdenCosting logic uses temp-table not parameter */
            ttLaborDtl.ResourceGrpID = wcCode;
            this.chgWcCode(wcCode, true, ref vMessage);
            LaborDtl_Foreign_Link();
        }

        /// <summary>
        /// This method delete records related to HCM PTO.
        /// </summary>
        /// <param name="LaborHedSeq">LaborHedSeq</param>
        /// <param name="LaborDtlSeq">LaborDtlSeq</param>
        /// <param name="CallFrom">Proposed value to filter logic for HCM</param>
        /// <param name="vMessage">Error message</param>
        public bool DeleteLaborDtl(int LaborHedSeq, int LaborDtlSeq, string CallFrom, out string vMessage)
        {
            bool _success = false;
            vMessage = string.Empty;
            string laborDtlSysRowID = string.Empty;

            try
            {
                if (CallFrom.Equals("HCM", StringComparison.OrdinalIgnoreCase))
                {
                    using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                    {
                        LaborDtl = this.FindFirstLaborDtlWithUpdLock(Session.CompanyID, LaborHedSeq, LaborDtlSeq);
                        if (LaborDtl != null)
                        {
                            HCMLaborDtlSync = this.FindFirstHCMLaborDtlWithUpdLock(Session.CompanyID, LaborDtl.SysRowID);
                            if (HCMLaborDtlSync != null)
                            {
                                if (this.ExistsHCMEnabled(Session.CompanyID) && HCMLaborDtlSync.Status.KeyEquals("R") && LaborDtl.TimeStatus.KeyEquals("E"))
                                {
                                    ErpCallContext.Properties["SkipHCMValidation"] = "SkipHCMValidation";

                                    Db.LaborDtl.Delete(LaborDtl);
                                    Db.HCMLaborDtlSync.Delete(HCMLaborDtlSync);

                                    _success = true;

                                    if (!this.ExistsLaborDtl8(Session.CompanyID, LaborHedSeq))
                                    {
                                        LaborHed = this.FindFirstLaborHedWithUpdLockQuery(Session.CompanyID, LaborHedSeq);

                                        if (LaborHed != null)
                                        {
                                            Db.LaborHed.Delete(LaborHed);
                                        }
                                    }
                                    ErpCallContext.RemoveValue("SkipHCMValidation");
                                }
                                else
                                {
                                    vMessage = Strings.HCMRecordDelete;
                                }
                            }
                            else
                            {
                                vMessage = Strings.HCMRecordNotFound;
                            }
                        }
                        else
                        {
                            vMessage = Strings.LaborDetailRecordNotFound;
                        }
                        txScope.Complete();
                    }
                }
                else
                {
                    vMessage = Strings.CallFromIncorrect;
                }
            }
            catch (Exception ex)
            {
                vMessage = ex.Message;
            }
            return _success;
        }

        /// <summary>
        /// Reverts serial numbers associated with current ttLaborDtl
        /// </summary>
        private void deleteSerialNumbers()
        {
            bool revertStatus = false;
            int lastSNTran = 0;
            Erp.Tables.SNTran cmplSNTran = null;
            Erp.Tables.SNTran revertSNTran = null;

            if (ttLaborDtl.ReWork)
            {
                JobAsmbl = this.FindFirstJobAsmbl2(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
                if (JobAsmbl != null)
                {
                    foreach (var SNTran_iterator in (this.SelectSNTranWithUpdLock(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, "OPR-RWK", ttLaborDtl.OprSeq)))
                    {
                        SNTran = SNTran_iterator;
                        Db.SNTran.Delete(SNTran);
                    }
                }
            }
            else
            {
                if (ttLaborDtl.EnableSN && CurrentFullTableset.LbrScrapSerialNumbers.Count == 0)
                {
                    getSerialNumbersForDelete();
                }

                foreach (var ttLbrScrapSerialNumbers_iterator in (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                                                  where ttLbrScrapSerialNumbers_Row.Company.Compare(ttLaborDtl.Company) == 0
                                                                  && ttLbrScrapSerialNumbers_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                                                                  && ttLbrScrapSerialNumbers_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq
                                                                  && !String.IsNullOrEmpty(ttLbrScrapSerialNumbers_Row.RowMod)
                                                                  select ttLbrScrapSerialNumbers_Row).ToList())
                {

                    ttLbrScrapSerialNumbers = ttLbrScrapSerialNumbers_iterator;/* assemblyseq, jobnum, or oprseq changed */

                    SerialNo serialNo = null;
                    if (ttLbrScrapSerialNumbers.AssemblySeq != ttLaborDtl.AssemblySeq || ttLbrScrapSerialNumbers.JobNum.Compare(ttLaborDtl.JobNum) != 0 || ttLbrScrapSerialNumbers.OprSeq != ttLaborDtl.OprSeq)
                    {

                        serialNo = this.FindFirstSerialNoWithUpdLock(ttLaborDtl.Company, ttLbrScrapSerialNumbers.JobNum, ttLbrScrapSerialNumbers.AssemblySeq, 0, ttLbrScrapSerialNumbers.PartNum, ttLbrScrapSerialNumbers.LaborHedSeq, ttLbrScrapSerialNumbers.LaborDtlSeq, ttLbrScrapSerialNumbers.SerialNumber);
                    }
                    else
                    {
                        serialNo = this.FindFirstSerialNoWithUpdLock(ttLaborDtl.Company, ttLbrScrapSerialNumbers.JobNum, ttLbrScrapSerialNumbers.AssemblySeq, 0, ttLbrScrapSerialNumbers.SerialNumber);
                    }

                    if (serialNo != null)
                    {
                        if (delSerialNoRows == null)
                        {
                            delSerialNoRows = new List<SerialNoRow>();
                        }
                        delSerialNo = new SerialNoRow();
                        delSerialNoRows.Add(delSerialNo);
                        delSerialNo.delSerialNumber = ttLbrScrapSerialNumbers.SerialNumber;

                        serialNo.ScrapLaborHedSeq = 0;
                        serialNo.ScrapLaborDtlSeq = 0;
                        serialNo.NonConfNum = 0;
                        serialNo.Scrapped = false;
                        serialNo.ScrapReasonCode = "";
                        CurrentFullTableset.LbrScrapSerialNumbers.Remove(ttLbrScrapSerialNumbers);


                        SNTran = this.FindLastSNTranWithUpdLock(serialNo.Company, serialNo.PartNum, serialNo.SerialNumber);
                        revertStatus = false;
                        if (SNTran != null)
                        {
                            lastSNTran = SNTran.TranNum;
                        }

                        if (SNTran != null && serialNo.SNStatus.Compare("REJECTED") == 0 && SNTran.TranType.Compare("ASM-REJ") == 0)
                        {
                            revertStatus = true;
                        }
                        else if (SNTran != null && serialNo.SNStatus.Compare("INSPECTION") == 0 && SNTran.TranType.Compare("ASM-INS") == 0)
                        {
                            revertStatus = true;
                        }

                        if (revertStatus)
                        {
                            Db.SNTran.Delete(SNTran);
                        }

                        cmplSNTran = this.FindLastSNTranWithUpdLock(serialNo.Company, serialNo.PartNum, serialNo.SerialNumber, "OPR-CMP", ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);
                        if (cmplSNTran != null)
                        {
                            if (cmplSNTran.TranNum == lastSNTran)
                            {
                                revertStatus = true;
                            }

                            Db.SNTran.Delete(cmplSNTran);
                        }
                        if (revertStatus)
                        {
                            revertSNTran = this.FindLastSNTran(serialNo.Company, serialNo.PartNum, serialNo.SerialNumber);
                            if (revertSNTran != null)
                            {
                                BufferCopy.CopyExceptFor(revertSNTran, serialNo, SNTran.ColumnNames.SysRowID, SNTran.ColumnNames.SysRevID);
                            }
                        }

                        SNTran = this.FindLastSNTran(serialNo.Company, serialNo.PartNum, serialNo.SerialNumber, "OPR-CMP");
                        if (SNTran != null)
                        {
                            serialNo.LastLbrOprSeq = SNTran.LastLbrOprSeq;
                            serialNo.NextLbrAssySeq = SNTran.NextLbrAssySeq;
                            serialNo.NextLbrOprSeq = SNTran.NextLbrOprSeq;
                        }
                        else
                        {
                            serialNo.LastLbrOprSeq = 0;
                            serialNo.NextLbrAssySeq = 0;
                            serialNo.NextLbrOprSeq = 0;
                        }
                    } //SerialNo != null 
                } //foreach ttLbrScrapSerialNumbers 
            } // not rework
        }

        /// <summary>
        /// Common code to set the value for field DisLaborType, which is used for row rules in UI.
        /// </summary>
        private void disLaborTypeProc()
        {
            ttLaborDtl.DisLaborType = false;
        }

        /// <summary>
        /// Common code to set the value for field DisLaborType in TimeWeeklyView, 
        /// which is used for row rules in UI.
        /// </summary>
        private void disLaborTypeProcTimeWeeklyView()
        {
            ttTimeWeeklyView.DisLaborType = false;
        }

        /// <summary>
        /// Common code to set the value for fields DisPrjRoleCd and DisTimeTypCd, which are used for row rules in UI.
        /// </summary>
        private void disPrjFields(bool clearRoleCdAndTimeType)
        {
            string roleCdList = string.Empty;
            string roleCd = string.Empty;
            ttLaborDtl.DisPrjRoleCd = false;
            ttLaborDtl.DisTimeTypCd = false;

            /* always disable for indirect or Project Billing not licensed or job num not entered yet */
            if (ttLaborDtl.LaborType.Compare("I") == 0 ||
                (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling)) ||
                 ttLaborDtl.TimeStatus.Compare("S") == 0 || ttLaborDtl.TimeStatus.Compare("P") == 0 || ttLaborDtl.TimeStatus.Compare("A") == 0 ||
                 ttLaborDtl.OprSeq <= 0)
            {
                ttLaborDtl.DisPrjRoleCd = true;
                ttLaborDtl.DisTimeTypCd = true;
                if (clearRoleCdAndTimeType)
                {
                    ttLaborDtl.RoleCd = "";
                    ttLaborDtl.RoleCdRoleDescription = "";
                    ttLaborDtl.TimeTypCd = "";
                    ttLaborDtl.TimeTypCdDescription = "";
                }
            }
            else
            {
                JobHeadResPrj JobHeadRes = this.FindFirstJobHeadResPrj(ttLaborDtl.Company, ttLaborDtl.JobNum);
                if (JobHeadRes != null && !String.IsNullOrEmpty(JobHeadRes.ProjectID))
                {
                    LibProjectCommon.getWBSPhaseMethods(JobHeadRes.ProjectID, JobHeadRes.PhaseID, out vInvMethod, out vRevMethod);
                }

                if (JobHeadRes == null || String.IsNullOrEmpty(JobHeadRes.ProjectID) || String.IsNullOrEmpty(ttLaborDtl.RoleCd) ||
                    ttLaborDtl.TimeStatus.Compare("S") == 0 || ttLaborDtl.TimeStatus.Compare("P") == 0 || ttLaborDtl.TimeStatus.Compare("A") == 0)
                {
                    ttLaborDtl.DisTimeTypCd = true;
                }
                else
                {
                    if (vInvMethod.Compare("TM") != 0)
                    {
                        ttLaborDtl.DisTimeTypCd = true;
                    }
                }
            }

            /* Set default value for Rolecd */
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling))
            {
                return;
            }

            if ("TM,CP,PP,FF".Lookup(vInvMethod) == -1)
            {
                return;
            }

            if (ttJCSyst.ChkEmpPrjRole == true)
            {
                return;
            }

            if (ttJCSyst.DfltPrjRoleLoc.Compare("OPR") == 0)
            {
                if (!String.IsNullOrEmpty(ttLaborDtl.ProjectID))
                {


                    JobOper = this.FindFirstJobOper(Session.CompanyID, ttLaborDtl.PhaseJobNum, ttLaborDtl.PhaseOprSeq);
                }
                else
                {


                    JobOper = this.FindFirstJobOper15(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                }
                if (JobOper == null)
                {
                    return;
                }

                if (String.IsNullOrEmpty(JobOper.PrjRoleList))
                {


                    OpMaster = this.FindFirstOpMaster(Session.CompanyID, ttLaborDtl.OpCode);
                    if (OpMaster != null)
                    {
                        roleCdList = OpMaster.PrjRoleList;
                    }
                }
                else
                {
                    roleCdList = JobOper.PrjRoleList;
                }

                if (!String.IsNullOrEmpty(roleCdList))
                {
                    roleCd = roleCdList.Entry(0, Ice.Constants.LIST_DELIM);
                    ttLaborDtl.RoleCd = roleCd;
                }
            }
            if (ttJCSyst.DfltPrjRoleLoc.Compare("OPR") != 0 || String.IsNullOrEmpty(roleCd))
            {


                EmpBasic = this.FindFirstEmpBasic7(ttLaborDtl.Company, ttLaborDtl.EmployeeNum);
                if (EmpBasic != null)
                {
                    if (ttLaborDtl.DisPrjRoleCd == false && String.IsNullOrEmpty(ttLaborDtl.RoleCd))
                    {


                        EmpRole = this.FindFirstEmpRole(Session.CompanyID, EmpBasic.EmpID, true);
                        if (EmpRole != null)
                        {
                            ttLaborDtl.RoleCd = EmpRole.RoleCd;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Common code to set the value for fields DisPrjRoleCd and DisTimeTypCd
        /// in TimeWeeklyView, which are used for row rules in UI.
        /// </summary>
        private void disPrjFieldsTimeWeeklyView()
        {
            ttTimeWeeklyView.DisPrjRoleCd = false;
            ttTimeWeeklyView.DisTimeTypCd = false;



            JobHead = this.FindFirstJobHead10(ttTimeWeeklyView.Company, ttTimeWeeklyView.JobNum);
            /* always disable for indirect or Project Billing not licensed or job num not entered yet */
            if (ttTimeWeeklyView.LaborType.Compare("I") == 0 ||
                (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling)) ||
                ttTimeWeeklyView.OprSeq <= 0)
            {
                ttTimeWeeklyView.DisPrjRoleCd = true;
                ttTimeWeeklyView.DisTimeTypCd = true;
                ttTimeWeeklyView.RoleCd = "";
                ttTimeWeeklyView.TimeTypCd = "";
                ttTimeWeeklyView.RoleCdDescription = "";
                ttTimeWeeklyView.TimeTypCdDescription = "";
            }
            else
            {
                if (JobHead == null || String.IsNullOrEmpty(JobHead.ProjectID) || String.IsNullOrEmpty(ttTimeWeeklyView.RoleCd))
                {
                    ttTimeWeeklyView.DisTimeTypCd = true;
                }
                else
                {
                    LibProjectCommon.getWBSPhaseMethods(JobHead.ProjectID, JobHead.PhaseID, out vInvMethod, out vRevMethod);
                    if (vInvMethod.Compare("TM") != 0)
                    {
                        ttTimeWeeklyView.DisTimeTypCd = true;
                    }
                }
            }
        }

        private void EnableLotAndGetDefault(string partNum, string jobNum)
        {
            PartPartial partPartial = FindFirstPartPartial(Session.CompanyID, partNum);
            if (partPartial != null && partPartial.TrackLots)
            {
                ttLaborDtl.EnableLot = true;

                if (ExistsXbSystJobLotDflt(Session.CompanyID))
                {
                    ttLaborDtl.LotNum = jobNum;
                }
                else if (partPartial.LotUseGlobalSeq)
                {
                    using (var libLotCommon = new Erp.Internal.Lib.LotCommon(Db))
                    {
                        string newLotNum = string.Empty;
                        libLotCommon.GenerateNewLotNum(partNum, out newLotNum);
                        ttLaborDtl.LotNum = newLotNum;
                    }
                }
            }
            else
            {
                ttLaborDtl.EnableLot = false;
                ttLaborDtl.LotNum = string.Empty;
            }
        }

        private bool EnablePCID(string jobNum, int assemblySeq, int oprSeq, string laborType, bool rework)
        {
            if (!(from ttReportQtyPart_Row in CurrentFullTableset.LaborPart
                  select ttReportQtyPart_Row).Any())
            {
                using (PackageControlValidations libPackageControlValidations = new PackageControlValidations(Db))
                {
                    return libPackageControlValidations.EnablePCID(jobNum, assemblySeq, oprSeq, laborType, rework);
                }
            }

            return false;
        }

        /// <summary>
        /// Method to call to end an activity in Shop Floor.  This method will mark
        /// the EndActivity flag in LaborDtl so when the Update method is run, special
        /// end activity processing can occur.  It will also default values in other
        /// fields that apply to the end activity.  Before this method is called, the 
        /// LaborDtl.RowMod value needs to be set to 'U'.
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        public void EndActivity(ref LaborTableset ds)
        {
            CurrentFullTableset = ds;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailRecordHasNotChanged, "LaborDtl");
            }

            ttLaborDtl.EndActivity = true;

            if (!String.IsNullOrEmpty(ttLaborDtl.ResourceID))
            {
                ttLaborDtl.ResourceDesc = ttLaborDtl.ResourceID;
            }
            else
            {
                ttLaborDtl.ResourceDesc = ttLaborDtl.ResourceGrpID;
            }

            /* indirect */
            if (ttLaborDtl.LaborType.Equals("I", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) == true)
            {
                string vResourceGrpID = string.Empty;
                /* Get the next OprSeq */
                ttLaborDtl.NextOprSeq = 0;
                ttLaborDtl.NextResourceDesc = "";
                ttLaborDtl.NextOperDesc = "";
                var outNextOprSeq = ttLaborDtl.NextOprSeq;
                var outNextAssemblySeq = ttLaborDtl.NextAssemblySeq;
                this.LibGetNextOprSeq.RunGetNextOprSeq(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, false, out outNextOprSeq, out outNextAssemblySeq, out vResourceGrpID);
                ttLaborDtl.NextOprSeq = outNextOprSeq;
                ttLaborDtl.NextAssemblySeq = outNextAssemblySeq;

                JobOper Next_JobOper = FindFirstJobOper16(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.NextAssemblySeq, ttLaborDtl.NextOprSeq);
                if (Next_JobOper != null)
                {
                    JobOpDtl = FindFirstJobOpDtl3(Next_JobOper.Company, Next_JobOper.JobNum, Next_JobOper.AssemblySeq, Next_JobOper.OprSeq, Next_JobOper.PrimaryProdOpDtl);

                    Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, JobOpDtl.ResourceID);

                    string cNextResourceDesc = string.Empty;

                    ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, JobOpDtl.ResourceGrpID);
                    if (Resource != null && Resource.Location == true)
                    {
                        cNextResourceDesc = Resource.Description;
                    }
                    else if (ResourceGroup != null)
                    {
                        cNextResourceDesc = ResourceGroup.Description;
                    }

                    OpMaster = FindFirstOpMaster2(Session.CompanyID, Next_JobOper.OpCode);
                    ttLaborDtl.NextResourceDesc = cNextResourceDesc;
                    ttLaborDtl.NextOperDesc = OpMaster.OpDesc;

                    if (ttLaborDtl.EnableSN)
                    {
                        getSerialNumbers();
                    }
                }
            }

            ttLaborDtl.EnableResourceGrpID = ttJCSyst.MachinePrompt;
            ttLaborDtl.JCSystScrapReasons = ttJCSyst.ScrapReasons;
            ttLaborDtl.JCSystReworkReasons = ttJCSyst.ReworkReasons;

            JobOper = FindFirstJobOper17(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
            if (JobOper != null)
            {
                ttLaborDtl.JobOperComplete = JobOper.OpComplete;
                /* SCR #25376 - set the Complete flag when ending the activity */
                if (JobOper.QtyCompleted >= JobOper.RunQty)
                {
                    ttLaborDtl.Complete = true;
                }
                else if (JobOper.OpComplete == false)
                {
                    ttLaborDtl.Complete = false;
                }
                ttLaborDtl.CompleteFlag = ((ttLaborDtl.CallNum == 0) ? ttLaborDtl.Complete : ttLaborDtl.FSComplete);
            }

            ttLaborDtl.EnableRequestMove = (genMtlQ() == true && Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) == true);
            ttLaborDtl.RequestMove = ttLaborDtl.EnableRequestMove;
            if (ttLaborDtl.EnableRequestMove == true)
            {
                JobOper = FindFirstJobOper18(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                if (JobOper != null)
                {
                    JobOpDtl = FindFirstJobOpDtl4(JobOper.Company, JobOper.JobNum, JobOper.AssemblySeq, JobOper.OprSeq, JobOper.PrimaryProdOpDtl);

                    var resourceTimeUsedPartialRow = FindFirstResourceTimeUsed(JobOper.Company, JobOper.JobNum, JobOper.AssemblySeq, JobOper.OprSeq, JobOpDtl.OpDtlSeq, false);
                    if (resourceTimeUsedPartialRow != null)
                    {
                        var resourcePartialRow = FindFirstResource20(JobOper.Company, resourceTimeUsedPartialRow.ResourceID);
                        ttLaborDtl.RequestMove = (resourcePartialRow.AutoMove == false);
                    }
                    else
                    {
                        ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, JobOpDtl.ResourceGrpID);
                        if (ResourceGroup != null)
                        {
                            ttLaborDtl.RequestMove = (ResourceGroup.AutoMove == false);
                        }
                    }
                }
            }

            JobAsmblPartResult jobAsmblPart = FindFirstJobAsmblPart(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
            if (jobAsmblPart != null)
            {
                ttLaborDtl.PartNum = jobAsmblPart.PartNum;
                ttLaborDtl.PartDescription = jobAsmblPart.PartDescription;

                PartPartial pp = FindFirstPartPartial(ttLaborDtl.Company, ttLaborDtl.PartNum);
                if (pp != null)
                {
                    ttLaborDtl.AttrClassID = pp.AttrClassID;
                }
            }

            SetOutputWarehouseAndBin(ttLaborDtl.ResourceID, ttLaborDtl.ResourceGrpID);

            /* setup */
            if (ttLaborDtl.LaborType.Compare("S") == 0)
            {
                ttLaborDtl.UnapprovedFirstArt = this.getUnapprovedFirstArt();
            }

            EmpBasic = FindFirstEmpBasic8(Session.CompanyID, ttLaborDtl.EmployeeNum);
            if (EmpBasic != null)
            {
                JobOper NextJobOper = FindFirstJobOperPrimary(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.NextOprSeq);
                if (NextJobOper != null)
                    ttLaborDtl.EnableNextOprSeq = !(NextJobOper.LaborEntryMethod.Compare("X") == 0 || ttLaborDtl.LaborEntryMethod.Compare("X") == 0);

                if (ttLaborDtl.LaborEntryMethod.Compare("X") != 0)
                {
                    ttLaborDtl.EnableLaborQty = EmpBasic.CanReportQty;
                    ttLaborDtl.EnableComplete = EmpBasic.CanReportQty;
                    ttLaborDtl.EnableScrapQty = EmpBasic.CanReportScrapQty && !ttLaborDtl.ReportPartQtyAllowed;
                    ttLaborDtl.EnableDiscrepQty = (EmpBasic.CanReportNCQty && Session.ModuleLicensed(Erp.License.ErpLicensableModules.QualityAssurance) == true) && !ttLaborDtl.ReportPartQtyAllowed;
                }
                else
                {
                    ttLaborDtl.EnableLaborQty = false;
                    ttLaborDtl.EnableScrapQty = false;
                    ttLaborDtl.EnableDiscrepQty = false;
                }
            }

            genLaborPart(ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);

            if (EmpBasic.CanReportQty == true && ttLaborDtl.LaborEntryMethod.Compare("X") != 0)
            {
                if (((from ttLaborPart_Row in CurrentFullTableset.LaborPart
                      where ttLaborPart_Row.Company.Compare(ttLaborDtl.Company) == 0
                      && ttLaborPart_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                      && ttLaborPart_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq
                      select ttLaborPart_Row).Any()))
                {
                    ttLaborDtl.EnableLaborQty = false;
                    ttLaborDtl.EnablePrintTagsList = true;
                }
                else
                {
                    ttLaborDtl.EnableLaborQty = true;
                    ttLaborDtl.EnablePrintTagsList = false;
                }
            }
        }

        /// <summary>
        /// This method checks for any necessary labor warning when the complete flag is checked in MES End Activity
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="cmplete">Proposed change to the Complete field </param>
        /// <param name="vMessage">Returns a string of warnings user needs to know</param>
        public void EndActivityComplete(ref LaborTableset ds, bool cmplete, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }
            ttLaborDtl.Complete = cmplete;
            this.warnLabor(ref vMessage);

            if (!(ttLaborDtl.EnableSN && ttLaborDtl.ReWork))
            {
                setOprComplete(cmplete);
            }
        }
        private void genLaborPart(int ip_LaborHedSeq, int ip_LaborDtlSeq)
        {
            string partUOM = string.Empty;
            int vCont = 0;
            int vCont2 = 0;
            string AttributeSetDescription = string.Empty;
            string AttributeSetShortDescription = string.Empty;
            Erp.Tables.Part altPart;


            LaborDtl = this.FindFirstLaborDtl3(Session.CompanyID, ip_LaborHedSeq, ip_LaborDtlSeq);
            if (LaborDtl == null)
            {
                return;
            }

            EmpBasic = this.FindFirstEmpBasic8(Session.CompanyID, LaborDtl.EmployeeNum);

            foreach (var LaborPart_iterator in (this.SelectLaborPart4(LaborDtl.Company, LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq)))
            {
                LaborPart = LaborPart_iterator;


                ttLaborPart = (from ttLaborPart_Row in CurrentFullTableset.LaborPart
                               where ttLaborPart_Row.Company.Compare(LaborPart.Company) == 0
                                  && ttLaborPart_Row.LaborHedSeq == LaborPart.LaborHedSeq
                                  && ttLaborPart_Row.LaborDtlSeq == LaborPart.LaborDtlSeq
                                  && ttLaborPart_Row.PartNum.Compare(LaborPart.PartNum) == 0
                                  && ttLaborPart_Row.RowMod.Compare(IceRow.ROWSTATE_UNCHANGED) == 0
                               select ttLaborPart_Row).FirstOrDefault();


                JobPart = this.FindFirstJobPart3(Session.CompanyID, LaborDtl.JobNum, LaborPart.PartNum);


                altPart = this.FindFirstPart(Session.CompanyID, LaborPart.PartNum);

                if (ttLaborPart == null)
                {
                    ttLaborPart = new Erp.Tablesets.LaborPartRow();
                    CurrentFullTableset.LaborPart.Add(ttLaborPart);
                    BufferCopy.Copy(LaborPart, ref ttLaborPart);
                    ttLaborPart.PartUOM = ((JobPart != null) ? JobPart.IUM : "");
                    ttLaborPart.RevisionNum = ((JobPart != null) ? JobPart.RevisionNum : "");
                    ttLaborPart.PartDescription = GetPartDescription(JobPart, altPart);
                    ttLaborPart.ScrapUOM = ((JobPart != null) ? JobPart.IUM : "");
                    ttLaborPart.ScrapRevision = ((JobPart != null) ? JobPart.RevisionNum : "");
                    ttLaborPart.DiscrepUOM = ((JobPart != null) ? JobPart.IUM : "");
                    ttLaborPart.DiscrepRevision = ((JobPart != null) ? JobPart.RevisionNum : "");
                    ttLaborPart.RowMod = IceRow.ROWSTATE_UNCHANGED;
                    ttLaborPart.SysRowID = LaborPart.SysRowID;

                    if (altPart != null && altPart.TrackInventoryAttributes && JobPart.AttributeSetID != 0)
                    {
                        // Use the Erp.Internal.Lib.AdvanceUOM method GetAttributeSetDescription
                        using (Erp.Internal.Lib.AdvancedUOM LibAdvancedUOM = new Erp.Internal.Lib.AdvancedUOM(Db))
                        {
                            LibAdvancedUOM.GetAttributeSetDescriptions(JobPart.Company, JobPart.AttributeSetID, out AttributeSetDescription, out AttributeSetShortDescription);
                        }
                        ttLaborPart.LaborAttributeSetDescription = AttributeSetDescription;
                        ttLaborPart.LaborAttributeSetShortDescription = AttributeSetShortDescription;
                        ttLaborPart.ScrapAttributeSetDescription = AttributeSetDescription;
                        ttLaborPart.ScrapAttributeSetShortDescription = AttributeSetShortDescription;
                        ttLaborPart.DiscrepAttributeSetDescription = AttributeSetDescription;
                        ttLaborPart.DiscrepAttributeSetShortDescription = AttributeSetShortDescription;
                    }
                }
                else
                {
                    ttLaborPart.PartUOM = ((JobPart != null) ? JobPart.IUM : "");
                    ttLaborPart.RevisionNum = ((JobPart != null) ? JobPart.RevisionNum : "");
                    ttLaborPart.PartDescription = GetPartDescription(JobPart, altPart);
                    ttLaborPart.ScrapUOM = ((JobPart != null) ? JobPart.IUM : "");
                    ttLaborPart.ScrapRevision = ((JobPart != null) ? JobPart.RevisionNum : "");
                    ttLaborPart.DiscrepUOM = ((JobPart != null) ? JobPart.IUM : "");
                    ttLaborPart.DiscrepRevision = ((JobPart != null) ? JobPart.RevisionNum : "");
                    ttLaborPart.NonConfTranID = LaborPart.NonConfTranID;
                    ttLaborPart.DiscrpRsnCode = LaborPart.DiscrpRsnCode;
                    ttLaborPart.SysRowID = LaborPart.SysRowID;
                    ttLaborPart.SysRevID = (long)LaborPart.SysRevNum;

                    if (altPart != null && altPart.TrackInventoryAttributes && JobPart.AttributeSetID != 0)
                    {
                        // Use the Erp.Internal.Lib.AdvanceUOM method GetAttributeSetDescription
                        using (Erp.Internal.Lib.AdvancedUOM LibAdvancedUOM = new Erp.Internal.Lib.AdvancedUOM(Db))
                        {
                            LibAdvancedUOM.GetAttributeSetDescriptions(JobPart.Company, JobPart.AttributeSetID, out AttributeSetDescription, out AttributeSetShortDescription);
                        }
                        ttLaborPart.LaborAttributeSetDescription = AttributeSetDescription;
                        ttLaborPart.LaborAttributeSetShortDescription = AttributeSetShortDescription;
                        ttLaborPart.ScrapAttributeSetDescription = AttributeSetDescription;
                        ttLaborPart.ScrapAttributeSetShortDescription = AttributeSetShortDescription;
                        ttLaborPart.DiscrepAttributeSetDescription = AttributeSetDescription;
                        ttLaborPart.DiscrepAttributeSetShortDescription = AttributeSetShortDescription;
                    }
                }
                if (EmpBasic != null && ReportPartQtyAllowed(LaborDtl.JobNum, LaborDtl.AssemblySeq, LaborDtl.OprSeq) == true)
                {
                    ttLaborPart.EnableScrapQty = (!LaborDtl.LaborEntryMethod.KeyEquals("X")) && EmpBasic.CanReportScrapQty;
                    ttLaborPart.EnableDiscrpQty = (!LaborDtl.LaborEntryMethod.KeyEquals("X")) && (EmpBasic.CanReportNCQty && Session.ModuleLicensed(Erp.License.ErpLicensableModules.QualityAssurance) == true);
                }

                if (ttLaborDtl != null)
                {
                    ttLaborPart.MESProcessing = ttLaborDtl.EndActivity;
                }
            }
            if (ttLaborDtl != null)
            {
                ttLaborDtl.EnableLaborQty = true;
            }

            if (this.ReportPartQtyAllowed(LaborDtl.JobNum, LaborDtl.AssemblySeq, LaborDtl.OprSeq) == true)
            {


                foreach (var JobPart_iterator in (this.SelectJobPart(LaborDtl.Company, LaborDtl.JobNum)))
                {
                    JobPart = JobPart_iterator;
                    vCont = vCont + 1;


                    if (!((from ttLaborPart_Row in CurrentFullTableset.LaborPart
                           where ttLaborPart_Row.Company.Compare(LaborDtl.Company) == 0
                              && ttLaborPart_Row.LaborHedSeq == LaborDtl.LaborHedSeq
                              && ttLaborPart_Row.LaborDtlSeq == LaborDtl.LaborDtlSeq
                              && ttLaborPart_Row.PartNum.Compare(JobPart.PartNum) == 0
                           select ttLaborPart_Row).Any()))
                    {
                        vCont2 = vCont2 + 1;


                        altPart = this.FindFirstPart(Session.CompanyID, JobPart.PartNum);

                        LaborPart = new Erp.Tables.LaborPart();
                        Db.LaborPart.Insert(LaborPart);
                        LaborPart.Company = JobPart.Company;
                        LaborPart.LaborHedSeq = LaborDtl.LaborHedSeq;
                        LaborPart.LaborDtlSeq = LaborDtl.LaborDtlSeq;
                        LaborPart.PartNum = JobPart.PartNum;
                        LaborPart.LaborAttributeSetID = JobPart.AttributeSetID;
                        LaborPart.ScrapAttributeSetID = JobPart.AttributeSetID;
                        LaborPart.DiscrpAttributeSetID = JobPart.AttributeSetID;
                        Db.Validate(LaborPart);
                        ttLaborPart = new Erp.Tablesets.LaborPartRow();
                        CurrentFullTableset.LaborPart.Add(ttLaborPart);
                        ttLaborPart.Company = JobPart.Company;
                        ttLaborPart.LaborHedSeq = LaborDtl.LaborHedSeq;
                        ttLaborPart.LaborDtlSeq = LaborDtl.LaborDtlSeq;
                        ttLaborPart.PartNum = JobPart.PartNum;
                        ttLaborPart.PartUOM = JobPart.IUM;
                        ttLaborPart.RevisionNum = JobPart.RevisionNum;
                        ttLaborPart.LaborAttributeSetID = JobPart.AttributeSetID;

                        if (altPart != null && altPart.TrackInventoryAttributes && JobPart.AttributeSetID != 0)
                        {
                            // Use the Erp.Internal.Lib.AdvanceUOM method GetAttributeSetDescription
                            using (Erp.Internal.Lib.AdvancedUOM LibAdvancedUOM = new Erp.Internal.Lib.AdvancedUOM(Db))
                            {
                                LibAdvancedUOM.GetAttributeSetDescriptions(JobPart.Company, JobPart.AttributeSetID, out AttributeSetDescription, out AttributeSetShortDescription);
                            }
                            ttLaborPart.LaborAttributeSetDescription = AttributeSetDescription;
                            ttLaborPart.LaborAttributeSetShortDescription = AttributeSetShortDescription;
                        }
                        ttLaborPart.ScrapUOM = JobPart.IUM;
                        ttLaborPart.ScrapRevision = JobPart.RevisionNum;
                        ttLaborPart.ScrapAttributeSetID = JobPart.AttributeSetID;
                        ttLaborPart.DiscrepUOM = JobPart.IUM;
                        ttLaborPart.DiscrepRevision = JobPart.RevisionNum;
                        ttLaborPart.DiscrpAttributeSetID = JobPart.AttributeSetID;
                        ttLaborPart.PartDescription = GetPartDescription(JobPart, altPart);
                        ttLaborPart.RowMod = IceRow.ROWSTATE_UNCHANGED;
                        ttLaborPart.SysRowID = LaborPart.SysRowID;
                        ttLaborPart.SysRevID = (long)LaborPart.SysRevNum;

                        if (EmpBasic != null)
                        {
                            ttLaborPart.EnableScrapQty = (!LaborDtl.LaborEntryMethod.KeyEquals("X")) && EmpBasic.CanReportScrapQty;
                            ttLaborPart.EnableDiscrpQty = (!LaborDtl.LaborEntryMethod.KeyEquals("X")) && (EmpBasic.CanReportNCQty && Session.ModuleLicensed(Erp.License.ErpLicensableModules.QualityAssurance) == true);
                        }

                    }
                }
                if (ttLaborDtl != null && vCont > 0)
                {
                    ttLaborDtl.EnableLaborQty = false;
                    if (vCont == vCont2)
                    {
                        ttLaborDtl.LaborQty = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Method to retrieve the active Labor Details and header records by employee.
        /// </summary>
        /// <param name="employeeNum">The Employee Num</param>
        /// <returns>The Labor data set </returns>
        public LaborTableset GetActiveLaborDtl(string employeeNum)
        {
            LaborHed = FindFirstLaborHed(Session.CompanyID, employeeNum);
            if (LaborHed == null)
            {
                throw new BLException(Strings.LaborRecordNotFound, "LaborHed");
            }
            ttLaborHed = new Erp.Tablesets.LaborHedRow();
            CurrentFullTableset.LaborHed.Add(ttLaborHed);
            BufferCopy.Copy(LaborHed, ref ttLaborHed);

            foreach (var activeLaborDtls in SelectActiveLaborDtl(Session.CompanyID, employeeNum))
            {
                ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
                CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
                BufferCopy.Copy(activeLaborDtls, ref ttLaborDtl);
                LaborDtl_Foreign_Link();
            }
            return CurrentFullTableset;
        }

        private void getAttributeDescriptions(LaborDtlRow ttLaborDtl, string type)
        {
            string attributeSetDescription = string.Empty;
            string attributeSetShortDescription = string.Empty;
            int attributeSetID = 0;
            bool trackInventoryAttributes = false;

            var jobAsmblPart = FindFirstJobAsmblPart(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);

            if (jobAsmblPart != null)
            {
                var partPartial = FindFirstPartPartial(Session.CompanyID, jobAsmblPart.PartNum);
                if (partPartial != null && partPartial.TrackInventoryAttributes)
                {
                    trackInventoryAttributes = partPartial.TrackInventoryAttributes;
                }

            }

            if (type.KeyEquals("D"))
            {
                attributeSetID = ttLaborDtl.DiscrepAttributeSetID;
                runLibGetDescriptions(attributeSetID, trackInventoryAttributes, out attributeSetDescription, out attributeSetShortDescription);

                ttLaborDtl.DiscrepAttributeSetDescription = attributeSetDescription;
                ttLaborDtl.DiscrepAttributeSetShortDescription = attributeSetShortDescription;

            }

            if (type.KeyEquals("L"))
            {
                attributeSetID = ttLaborDtl.LaborAttributeSetID;
                runLibGetDescriptions(attributeSetID, trackInventoryAttributes, out attributeSetDescription, out attributeSetShortDescription);

                ttLaborDtl.LaborAttributeSetDescription = attributeSetDescription;
                ttLaborDtl.LaborAttributeSetShortDescription = attributeSetShortDescription;

            }

            if (type.KeyEquals("S"))
            {
                attributeSetID = ttLaborDtl.ScrapAttributeSetID;
                runLibGetDescriptions(attributeSetID, trackInventoryAttributes, out attributeSetDescription, out attributeSetShortDescription);

                ttLaborDtl.ScrapAttributeSetDescription = attributeSetDescription;
                ttLaborDtl.ScrapAttributeSetShortDescription = attributeSetShortDescription;

            }

        }

        private void runLibGetDescriptions(int attributeSetID, bool trackInventoryAttributes, out string attrSetDesc, out string attrSetShortDesc)
        {
            attrSetDesc = string.Empty;
            attrSetShortDesc = string.Empty;

            if (!trackInventoryAttributes)
            {
                return;
            }

            using (Erp.Internal.Lib.AdvancedUOM LibAdvancedUOM = new Erp.Internal.Lib.AdvancedUOM(Db))
            {
                LibAdvancedUOM.GetAttributeSetDescriptions(Session.CompanyID, attributeSetID, out attrSetDesc, out attrSetShortDesc);
            }

        }
        /// <summary>
        /// This method gets the original values of fields for comparison/update
        /// </summary>
        private void getBeforeInfo()
        {
            Erp.Tables.LaborDtl altLaborDtl = null;
            Erp.Tables.JobOper altJobOper = null;
            oldJobNum = "";
            oldOprSeq = 0;
            oldAssSeq = 0;
            oldJobQty = 0;
            oldWcCode = "";
            saveActProdHours = 0;
            saveActProdRwkHours = 0;
            saveActSetupHours = 0;
            saveActSetupRwkHours = 0;
            saveBurdenHrs = 0;    /*If ttLaborDtl is available, I'm updating LabotDtl directly*/
            /*If LaborDtl is available, I'm updating LaborDtl.LaborQty from LaborPart (Job Parts Tab)*/
            if (ttLaborDtl != null)
            {
                if (ttLaborDtl.SentFromMES && ttLaborPart != null)
                    vUpdatingJobParts = true;
                else
                    vUpdatingJobParts = false;


                altLaborDtl = this.FindFirstLaborDtl2(ttLaborDtl.SysRowID);
            }
            else if (LaborDtl != null)
            {
                vUpdatingJobParts = true;


                altLaborDtl = this.FindFirstLaborDtl3(LaborDtl.SysRowID);
            }
            if (altLaborDtl != null)
            {
                if ((oldLbrQty == 0 && !vUpdatingJobParts))
                {
                    oldLbrQty = altLaborDtl.LaborQty;
                }

                oldJobNum = altLaborDtl.JobNum;
                oldOprSeq = altLaborDtl.OprSeq;
                oldAssSeq = altLaborDtl.AssemblySeq;
                saveBurdenHrs = altLaborDtl.BurdenHrs;
                oldWcCode = altLaborDtl.ResourceGrpID;



                altJobOper = this.FindFirstJobOper19(Session.CompanyID, altLaborDtl.JobNum, altLaborDtl.AssemblySeq, altLaborDtl.OprSeq);
                if (altJobOper != null)
                {
                    oldJobQty = altJobOper.QtyCompleted;             /* setup */
                    if (altLaborDtl.LaborType.Compare("S") != 0)
                    {
                        saveActProdHours = altJobOper.ActProdHours;
                        saveActProdRwkHours = altJobOper.ActProdRwkHours;
                    }
                    else
                    {
                        saveActSetupHours = altJobOper.ActSetupHours;
                        saveActSetupRwkHours = altJobOper.ActSetupRwkHours;
                    }
                }
            }
        }

        /// <summary>
        /// Method to Begin Downtime for Kinetic MES
        /// </summary>
        /// <param name="employeeNum">Employee Number </param>
        /// <param name="indirectCode">Indirect Code</param>
        /// <param name="indirectNote">Indirect Labor Note</param>
        public void InitiateDowntime(string employeeNum, string indirectCode, string indirectNote)
        {

            LaborTableset ds = new LaborTableset();

            this.ValidateIndirectCodeIsDowntime(indirectCode);

            LaborHed = FindFirstLaborHed(Session.CompanyID, employeeNum);
            if (LaborHed == null)
            {
                throw new BLException(Strings.LaborRecordNotFound, "LaborHed");
            }

            if (ExistsLaborDtlDowntime(Session.CompanyID, LaborHed.EmployeeNum, LaborHed.LaborHedSeq))
            {
                throw new BLException(Strings.CannotStartDowntimeWhenDowntimeAlreadyExists);
            }

            ttLaborHed = new Erp.Tablesets.LaborHedRow();
            CurrentFullTableset.LaborHed.Add(ttLaborHed);
            BufferCopy.Copy(LaborHed, ref ttLaborHed);
            ttLaborHed.SysRowID = Guid.NewGuid();
            ttLaborHed.SysRowID = LaborHed.SysRowID;

            LaborHedAfterGetRows();
            LaborHed_Foreign_Link();

            foreach (var activeLaborDtls in SelectActiveLaborDtlProdSetup(Session.CompanyID, LaborHed.EmployeeNum, LaborHed.LaborHedSeq))
            {
                LaborDtl = activeLaborDtls;
                ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
                CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
                BufferCopy.Copy(LaborDtl, ref ttLaborDtl);
                ttLaborDtl.SysRowID = LaborDtl.SysRowID;
                ttLaborDtl.RowMod = "U";
                ttLaborDtl.Downtime = true;
                ttLaborDtl.EndActivity = true;
                LaborDtlAfterGetRows();

            }

            ds = CurrentFullTableset;

            this.ExternalMESDowntime(ds, indirectCode, indirectNote);

            this.Update(ref ds);
        }

        /// <summary>
        /// Method to End Downtime for Kinetic MES
        /// </summary>
        /// <param name="employeeNum">Employee Number </param>
        public void EndDowntime(string employeeNum)
        {

            LaborTableset ds = new LaborTableset();

            LaborHed = FindFirstLaborHed(Session.CompanyID, employeeNum);
            if (LaborHed == null)
            {
                throw new BLException(Strings.LaborRecordNotFound, "LaborHed");
            }

            if (!ExistsLaborDtlDowntime(Session.CompanyID, LaborHed.EmployeeNum, LaborHed.LaborHedSeq))
            {
                throw new BLException(Strings.CannotEndDowntimeWithoutDowntime);
            }

            ttLaborHed = new Erp.Tablesets.LaborHedRow();
            CurrentFullTableset.LaborHed.Add(ttLaborHed);
            BufferCopy.Copy(LaborHed, ref ttLaborHed);
            ttLaborHed.SysRowID = Guid.NewGuid();
            ttLaborHed.SysRowID = LaborHed.SysRowID;

            LaborHedAfterGetRows();
            LaborHed_Foreign_Link();

            foreach (var activeLaborDtls in SelectActiveLaborDtlIndirect(Session.CompanyID, LaborHed.EmployeeNum, LaborHed.LaborHedSeq))
            {
                LaborDtl = activeLaborDtls;
                ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
                CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
                BufferCopy.Copy(LaborDtl, ref ttLaborDtl);
                ttLaborDtl.SysRowID = LaborDtl.SysRowID;
                ttLaborDtl.RowMod = "U";
                LaborDtlAfterGetRows();

            }

            ds = CurrentFullTableset;

            this.ExternalMESEndDowntime(ref ds);

            this.Update(ref ds);

        }

        /// <summary>
        /// Method to call to retrieve the Labor dataset with just the header 
        /// and a specific detail record.
        /// </summary>
        /// <param name="iLaborHedSeq">The LaborHedSeq of the LaborHed record to retrieve </param>
        /// <param name="iLaborDtlSeq">The LaborDtlSeq of the LaborDtl record to retrieve </param>
        /// <returns>The Labor data set </returns>
        public LaborTableset GetDetail(int iLaborHedSeq, int iLaborDtlSeq)
        {
            LaborHed = FindFirstLaborHed4(Session.CompanyID, iLaborHedSeq);
            if (LaborHed == null)
            {
                throw new BLException(Strings.LaborRecordNotFound, "LaborHed");
            }

            LaborDtl = FindFirstLaborDtl4(Session.CompanyID, iLaborHedSeq, iLaborDtlSeq);
            if (LaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailRecordNotFound, "LaborDtl");
            }

            ttLaborHed = new Erp.Tablesets.LaborHedRow();
            CurrentFullTableset.LaborHed.Add(ttLaborHed);
            BufferCopy.Copy(LaborHed, ref ttLaborHed);
            ttLaborHed.SysRowID = Guid.NewGuid();
            ttLaborHed.SysRowID = LaborHed.SysRowID;

            LaborHedAfterGetRows();
            LaborHed_Foreign_Link();

            ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
            CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
            BufferCopy.Copy(LaborDtl, ref ttLaborDtl);
            ttLaborDtl.SysRowID = LaborDtl.SysRowID;
            LaborDtlAfterGetRows();

            foreach (var LaborEquip_iterator in SelectLaborEquip(Session.CompanyID, iLaborHedSeq, iLaborDtlSeq))
            {
                LaborEquip = LaborEquip_iterator;
                ttLaborEquip = new Erp.Tablesets.LaborEquipRow();
                CurrentFullTableset.LaborEquip.Add(ttLaborEquip);
                BufferCopy.Copy(LaborEquip, ref ttLaborEquip);
                ttLaborEquip.SysRowID = LaborEquip.SysRowID;
                LaborEquip_Foreign_Link();
            }
            this.getLaborDtlActionDS();
            return CurrentFullTableset;
        }

        /// <summary>
        /// This method gets the labor hours totals
        /// </summary>
        /// <param name="laborHedSeq">Labor Head sequence Number</param>
        /// <param name="TotalLabHrs">Total Labor hours</param>
        /// <param name="TotalBurHrs">Total Burden Hours</param>
        /// <param name="TotalHCMPayHrs"> Total HCM Burden Hours</param>
        /// <param name="source"> Source of the total pay hours</param>
        private void getDetailHours(int laborHedSeq, out decimal TotalLabHrs, out decimal TotalBurHrs, out decimal TotalHCMPayHrs, string source)
        {
            TotalLabHrs = decimal.Zero;
            TotalBurHrs = decimal.Zero;
            TotalHCMPayHrs = decimal.Zero;

            LaborDtlSummary laborDtlSummary = this.SumLaborHrsByHed(Session.CompanyID, laborHedSeq, false).FirstOrDefault();
            TotalLabHrs = laborDtlSummary.TotalLabHrs;
            TotalBurHrs = laborDtlSummary.TotalBurHrs;

            if (source.Equals("DTL", StringComparison.OrdinalIgnoreCase))
            {
                TotalHCMPayHrs = laborDtlSummary.TotalHCMPayHrs;
            }
            else if (source.Equals("HDR", StringComparison.OrdinalIgnoreCase))
            {
                LaborHed tmpLaborHed = this.FindFirstLaborHed10(Session.CompanyID, laborHedSeq);
                TotalHCMPayHrs = tmpLaborHed.PayHours;
            }
        }

        /// <summary>
        /// This method gets the elapsed time from a start date-startTime until now
        /// </summary>
        /// <param name="startDate">Initial Date</param>
        /// <param name="startTime">Initial Time</param>
        public decimal getElapsedTime(DateTime startDate, Decimal startTime)
        {
            if (startDate.Date > CompanyTime.Now().Date)  //to avoid calculations with startDate in the future
                return 0m;
            else if ((startDate.Date == CompanyTime.Now().Date) && startTime > (CompanyTime.Now().Hour + (CompanyTime.Now().Minute / 60m))) //to avoid calculations with StartTime in the future on the same startDate
                return 0m;

            TimeSpan timespanDays = (CompanyTime.Now().Date - startDate);

            decimal dtHours = CompanyTime.Now().Hour;
            decimal dtMinutes = CompanyTime.Now().Minute;

            decimal elapsedMinutes = dtMinutes == 0 ? 0 : dtMinutes / 60;
            decimal elapsedHours = dtHours + elapsedMinutes;
            decimal elapsedTime = decimal.Zero;

            if (elapsedHours >= startTime)
            {
                elapsedTime = elapsedHours - startTime;
                elapsedTime = (timespanDays.Days * 24m) + elapsedTime;
            }
            else
            {
                elapsedTime = (24 - startTime) + elapsedHours;
                elapsedTime = ((timespanDays.Days - 1) * 24m) + elapsedTime;
            }

            return Math.Ceiling(elapsedTime); //<= Rounded to 2 DECIMALs because Equip.CurrentMeter is set as INTEGER.
        }

        private void getEnteredOprSeq()
        {
            string cResourceGrpID = string.Empty;
            string cResourceID = string.Empty;
            ttLaborDtl.NextResourceDesc = "";
            ttLaborDtl.NextOperDesc = "";



            JobOper = this.FindFirstJobOper20(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.NextOprSeq);
            if (JobOper == null)
            {
                ExceptionManager.AddBLException(Strings.InvalidOperationSeqence, "LaborDtl", "NextOprSeq");
                return;
            }/* not avial joboper */

            if (JobOper != null)
            {


                //JobOpDtl_LOOP:
                foreach (var JobOpDtl_iterator in (this.SelectJobOpDtl(Session.CompanyID, JobOper.JobNum, JobOper.AssemblySeq, JobOper.OprSeq)))
                {
                    JobOpDtl = JobOpDtl_iterator;
                    if (this.isLocation("ResourceGroup", JobOpDtl.ResourceGrpID) == true ||
                    this.isLocation("Resource", JobOpDtl.ResourceID) == true)
                    {
                        cResourceGrpID = JobOpDtl.ResourceGrpID;
                        cResourceID = JobOpDtl.ResourceID;
                        break;
                    }
                }
                if (!String.IsNullOrEmpty(cResourceGrpID))
                {


                    ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, cResourceGrpID);
                }
                if (!String.IsNullOrEmpty(cResourceID))
                {


                    Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, cResourceID);
                }


                OpMaster = this.FindFirstOpMaster3(Session.CompanyID, JobOper.OpCode);
                ttLaborDtl.NextOprSeq = JobOper.OprSeq;
                ttLaborDtl.NextResourceDesc = ((Resource != null && Resource.Location == true) ? Resource.Description : ((ResourceGroup != null) ? ResourceGroup.Description : ""));
                ttLaborDtl.NextOperDesc = OpMaster.OpDesc;
            }
        }

        /// <summary>
        /// To return the Burden Rate to be used for the Labor transaction    
        /// </summary>
        private void getLaborDtlBurdenRates(bool ipReplaceResRate, out decimal opTotalBurdenRate)
        {
            opTotalBurdenRate = decimal.Zero;
            decimal dSetupBurRate = decimal.Zero;
            decimal dProdBurRate = decimal.Zero;
            JobOpDtl altJobOpDtl = null;
            /*1.  Go total up all burden rates related to the operation excluding the primary JobOpDtl.
                  why exclude primary? Normally it would be what is currently being reported, so that is
                  how it would be included, if they chose some other resource then the primary should still be excluded
                  as in this case we consider they are overriding the primary */
            /* SCR 71777 - Now the PlantConfCtrl.ApplyBurdenCost is evaluated to determine if all burdens are added *
             * when having multiples Resource Groups for one operation OR just use selected ResourceGrp/Resource.   */
            opTotalBurdenRate = 0;
            if (vApplySumBurdenRates == true)
            {
                this.sumAllBurdenCost(ttLaborDtl.ResourceID, ttLaborDtl.ResourceGrpID, ttLaborDtl.CapabilityID, out opTotalBurdenRate);
            }    /*2. Get the burden rate for the current reporting Resource/ResourceGroup/Capability */

            altJobOpDtl = FindFirstJobOpDtl2(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.ResourceID, ttLaborDtl.ResourceGrpID, ttLaborDtl.CapabilityID);

            //If found a match and Override rates..
            if (altJobOpDtl != null)
            {
                dProdBurRate = altJobOpDtl.ProdBurRate;
                dSetupBurRate = altJobOpDtl.SetupBurRate;
            }
            else
            {
                this.calcOpDtlBurdenRate(ttLaborDtl.ResourceID, ttLaborDtl.ResourceGrpID, ttLaborDtl.CapabilityID, ttLaborDtl.LaborRate, out dProdBurRate, out dSetupBurRate);    /*3. Add the current reporting rate  (SETUP OR PRODUCTION) to the total */
            }

            /* setup */
            if (ttLaborDtl.LaborType.Compare("S") == 0)
            {
                opTotalBurdenRate = opTotalBurdenRate + dSetupBurRate;
            }
            else
            {
                opTotalBurdenRate = opTotalBurdenRate + dProdBurRate;
            }
        }

        partial void GetListRowLoaded()
        {
            BufferCopy.Copy(ttLaborHedList, ref ttLaborHed);/* Copy the list buffer to the row buffer */
            this.LaborHedAfterGetRows();                /* run the procedure which update the row buffer */
            LaborHed_Foreign_Link();
            BufferCopy.Copy(ttLaborHed, ref ttLaborHedList);
        }

        /// <summary>
        /// This method is called to add a new labor detail without having a
        /// labor header record available
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="EmployeeNum">The employee id for this labor record</param>
        /// <param name="ShopFloor">Indicates whether this is being called from the shop floor
        /// labor entry program</param>
        /// <param name="ClockInDate">The clock in date</param>
        /// <param name="ClockInTime">The clock in time</param>
        /// <param name="ClockOutDate">The clock out date</param>
        /// <param name="ClockOutTime">The clock out time</param>
        public void GetNewLaborDtlNoHdr(ref LaborTableset ds, string EmployeeNum, bool ShopFloor, DateTime? ClockInDate, decimal ClockInTime, DateTime? ClockOutDate, decimal ClockOutTime)
        {
            CurrentFullTableset = ds;
            string vmessage = string.Empty;
            GetNewLaborHed(ref ds);
            CurrentFullTableset = ds;
            ttLaborHed.EmployeeNum = EmployeeNum;
            ttLaborHed.GetNewNoHdr = true;
            if (ClockInDate != null)
            {
                if (ClockInDate == null)
                {
                    ttLaborHed.PayrollDate = null;
                }
                else
                {
                    ttLaborHed.PayrollDate = ClockInDate.Value.Date;
                }

                ttLaborHed.PayrollDateNav = ttLaborHed.PayrollDate;

                if (ClockInDate == null)
                {
                    ttLaborHed.ClockInDate = null;
                }
                else
                {
                    ttLaborHed.ClockInDate = ClockInDate.Value.Date;
                }

                ttLaborHed.ClockInTime = ClockInTime;
                ttLaborHed.ClockOutTime = ClockOutTime;
            }
            LaborHedAfterGetNew1(ShopFloor);
            if (ClockInDate != null)
            {
                if (ClockInDate == null)
                {
                    ttLaborHed.PayrollDate = null;
                }
                else
                {
                    ttLaborHed.PayrollDate = ClockInDate.Value.Date;
                }

                ttLaborHed.PayrollDateNav = ttLaborHed.PayrollDate;

                if (ClockInDate == null)
                {
                    ttLaborHed.ClockInDate = null;
                }
                else
                {
                    ttLaborHed.ClockInDate = ClockInDate.Value.Date;
                }

                if (ClockInDate == null)
                {
                    ttLaborHed.ActualClockinDate = null;
                }
                else
                {
                    ttLaborHed.ActualClockinDate = ClockInDate.Value.Date;
                }
            }
            if ((ClockInTime != 0 || ClockOutTime != 0))
            {
                ttLaborHed.ClockInTime = ClockInTime;
                ttLaborHed.ClockOutTime = ClockOutTime;
            }
            eadErrMsg = LibEADValidation.validateEAD(ttLaborHed.PayrollDate, "IP", "");
            if (!String.IsNullOrEmpty(eadErrMsg))
            {
                ExceptionManager.AddBLException(eadErrMsg, "ttLaborHed");
            }
            ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
            CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
            ttLaborDtl.Company = Session.CompanyID;
            ttLaborDtl.EmployeeNum = EmployeeNum;
            ttLaborDtl.LaborHedSeq = ttLaborHed.LaborHedSeq;
            if (ttLaborHed.ClockInDate == null)
            {
                ttLaborDtl.ClockInDate = null;
            }
            else
            {
                ttLaborDtl.ClockInDate = ttLaborHed.ClockInDate.Value.Date;
            }

            ttLaborDtl.ClockinTime = ttLaborHed.ClockInTime;
            ttLaborDtl.ClockOutTime = ttLaborHed.ClockOutTime;
            if (ttLaborHed.PayrollDate == null)
            {
                ttLaborDtl.PayrollDate = null;
            }
            else
            {
                ttLaborDtl.PayrollDate = ttLaborHed.PayrollDate.Value.Date;
            }

            ttLaborHed.PayrollDateNav = ttLaborHed.PayrollDate;
            ttLaborDtl.LaborHrs = ((ttLaborDtl.ClockinTime > ttLaborDtl.ClockOutTime) ? ttLaborDtl.ClockOutTime + 24 : ttLaborDtl.ClockOutTime) - ttLaborDtl.ClockinTime;
            ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
            ttLaborDtl.LaborTypePseudo = "P"; /* production */
            ttLaborDtl.LaborType = "P"; /* production */
            ttLaborDtl.ActiveTrans = false;
            ttLaborDtl.RowMod = IceRow.ROWSTATE_ADDED;
            this.LaborDtlSetDefaults(ttLaborDtl);



            EmpBasic = this.FindFirstEmpBasic9(ttLaborDtl.Company, ttLaborDtl.EmployeeNum);
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) && EmpBasic != null &&
            !EmpBasic.AllowDirLbr)
            {
                ttLaborDtl.LaborTypePseudo = "I";
                ttLaborDtl.LaborType = "I";
                ttLaborDtl.AllowDirLbr = false;
                ttLaborDtl.ResourceGrpID = EmpBasic.ResourceGrpID;
                ttLaborDtl.ResourceID = EmpBasic.ResourceID;
                if (!String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID))
                {
                    string dept = FindFirstResourceGroupJCDept(Session.CompanyID, ttLaborDtl.ResourceGrpID);
                    ttLaborDtl.JCDept = (dept != null) ? dept : "";
                }
            }

            if (EmpBasic != null) ttLaborDtl.Shift = EmpBasic.Shift;

            this.payHoursDtl(true, true, false, out vmessage);/* calc payroll pay hours */
            if (this.isHCMEnabledAt(ttLaborDtl.EmployeeNum).Equals("DTL", StringComparison.OrdinalIgnoreCase) && ttLaborDtl.RowMod.Equals("A"))
            {
                ttLaborDtl.HCMPayHours = ((ttLaborDtl.LaborHrs >= 0) ? ttLaborDtl.LaborHrs : 0);
            }
            this.disPrjFields(true);
            this.LaborDtlAfterGetRows();
            ds = CurrentFullTableset;
        }

        /// <summary>
        /// This method is called to add a new labor detail without having a
        /// labor header record available
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="ipClockInDate">The clock in date</param>
        /// <param name="ipClockInTime">The clock in time</param>
        /// <param name="ipClockOutDate">The clock out date</param>
        /// <param name="ipClockOutTime">The clock out time</param>
        /// <param name="ipLaborHedSeq">Unique identifier of the LaborHed</param>
        public void GetNewLaborDtlWithHdr(ref LaborTableset ds, DateTime? ipClockInDate, decimal ipClockInTime, DateTime? ipClockOutDate, decimal ipClockOutTime, int ipLaborHedSeq)
        {
            CurrentFullTableset = ds;

            string vValidateError = string.Empty;
            string vmessage = string.Empty;
            if (ipLaborHedSeq == 0)
            {
                throw new BLException(Strings.LaborHeaderDoesNotExist);
            }



            LaborHed = this.FindFirstLaborHed5(Session.CompanyID, ipLaborHedSeq);
            if (LaborHed == null)
            {
                throw new BLException(Strings.LaborHeaderDoesNotExist);
            }

            if (IsHCMEnabledAtCompany(Session.CompanyID, true))
            {
                string payrollValuesForHCM = LaborHed.HCMPayHoursCalcType;
                payrollValuesForHCM = (string.IsNullOrEmpty(payrollValuesForHCM)) ? "DTL" : payrollValuesForHCM;
                string empHCMlevel = isHCMEnabledAt(LaborHed.EmployeeNum);
                if (!empHCMlevel.Equals("NON", StringComparison.OrdinalIgnoreCase) && !empHCMlevel.Equals(payrollValuesForHCM, StringComparison.OrdinalIgnoreCase))
                    throw new BLException(Strings.HCMLaborMustHaveSameType);
            }

            eadErrMsg = LibEADValidation.validateEAD(LaborHed.PayrollDate, "IP", "");
            if (!String.IsNullOrEmpty(eadErrMsg))
            {
                throw new BLException(eadErrMsg, "ttLaborHed");
            }

            ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
            CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
            ttLaborDtl.Company = Session.CompanyID;
            ttLaborDtl.LaborHedSeq = LaborHed.LaborHedSeq;
            ttLaborDtl.RowMod = IceRow.ROWSTATE_ADDED;



            foreach (var ttLbrScrapSerialNumbers_iterator in (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                                              where ttLbrScrapSerialNumbers_Row.Company.KeyEquals(Session.CompanyID)
                                                              && ttLbrScrapSerialNumbers_Row.LaborHedSeq == 0
                                                              select ttLbrScrapSerialNumbers_Row))
            {
                ttLbrScrapSerialNumbers = ttLbrScrapSerialNumbers_iterator;
                ttLbrScrapSerialNumbers.LaborHedSeq = LaborHed.LaborHedSeq;
            }
            this.LaborDtlSetDefaults(ttLaborDtl);
            if (ipClockInDate != null)
            {
                if (ipClockInDate == null)
                {
                    ttLaborDtl.ClockInDate = null;
                }
                else
                {
                    ttLaborDtl.ClockInDate = ipClockInDate.Value.Date;
                }
            }

            /* the defaults values for these fields are populated in LaborDtlsSetDefaults so do not override them if they have
               already been set.  LaborDtlsSetDefaults has the logic to set the default values based upon previous labordtls. For
               example: if a labor dtl exists for times 07:00 to 08:00 and the laborhed is set for 07:00 AM to 03:30 PM the
               defaults for this labor detail are 08:00 to 03:30.  This is how 9.04 behaves.  */
            if ((ipClockInTime != 0 || ipClockOutTime != 0))
            {
                ttLaborDtl.ClockinTime = ipClockInTime;
                ttLaborDtl.ClockOutTime = ((ipClockOutDate > ipClockInDate) ? 24 : ipClockOutTime);
            }
            else if (ttLaborDtl.ClockInDate == null)
            {
                ttLaborDtl.ClockInDate = LaborHed.ClockInDate;
                ttLaborDtl.ClockinTime = LaborHed.ClockInTime;
                ttLaborDtl.ClockOutTime = LaborHed.ClockOutTime;
            }
            ttLaborDtl.EmployeeNum = LaborHed.EmployeeNum;
            ttLaborDtl.PayrollDate = LaborHed.PayrollDate;
            ttLaborDtl.LaborHrs = ((ttLaborDtl.ClockinTime > ttLaborDtl.ClockOutTime) ? ttLaborDtl.ClockOutTime + 24 : ttLaborDtl.ClockOutTime) - ttLaborDtl.ClockinTime;
            ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
            ttLaborDtl.LaborTypePseudo = "P"; /* production */
            ttLaborDtl.LaborType = "P"; /* production */
            ttLaborDtl.ActiveTrans = false;
            ttLaborDtl.RowMod = IceRow.ROWSTATE_ADDED;

            /* callt he method that removes lunch time from the burden and labor hours */
            this.payHoursDtl(true, true, true, out vmessage);/* calc payroll pay hours */
            if (this.isHCMEnabledAt(ttLaborDtl.EmployeeNum).Equals("DTL", StringComparison.OrdinalIgnoreCase) && ttLaborDtl.RowMod.Equals("A"))
            {
                ttLaborDtl.HCMPayHours = ((ttLaborDtl.LaborHrs >= 0) ? ttLaborDtl.LaborHrs : 0);
            }
            this.validDate();



            EmpBasic = this.FindFirstEmpBasic9(ttLaborDtl.Company, ttLaborDtl.EmployeeNum);
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) && EmpBasic != null &&
            !EmpBasic.AllowDirLbr)
            {
                ttLaborDtl.LaborTypePseudo = "I";
                ttLaborDtl.LaborType = "I";
                ttLaborDtl.AllowDirLbr = false;
                ttLaborDtl.ResourceGrpID = EmpBasic.ResourceGrpID;
                ttLaborDtl.ResourceID = EmpBasic.ResourceID;
                if (!String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID))
                {
                    string dept = FindFirstResourceGroupJCDept(Session.CompanyID, ttLaborDtl.ResourceGrpID);
                    ttLaborDtl.JCDept = (dept != null) ? dept : "";
                }
            }
            this.disPrjFields(true);
            this.LaborDtlAfterGetRows();
            ds = CurrentFullTableset;
        }

        /// <summary>
        /// This method to be used in place of GetNewLaborHed.  This method asks for an
        /// employee number to default fields based on the employee.  
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="EmployeeNum">The employee id for this labor record</param>
        /// <param name="ShopFloor">Indicates whether this is being called from the shop floor
        /// labor entry program</param>
        /// <param name="payrollDate">Payroll Date for this labor record</param>
        public void GetNewLaborHed1(ref LaborTableset ds, string EmployeeNum, bool ShopFloor, DateTime? payrollDate)
        {
            CurrentFullTableset = ds;
            GetNewLaborHed(ref ds);
            CurrentFullTableset = ds;
            ttLaborHed.EmployeeNum = EmployeeNum;
            LaborHedAfterGetNew1(ShopFloor);
            lDefDateFromNewHeader = true;
            this.DefaultDate(ref ds, payrollDate);
            lDefDateFromNewHeader = false;
            ds = CurrentFullTableset;
        }

        /// <summary>
        /// Gets a new LbrScrapSerialNumbers record for current LaborDtl
        /// </summary>
        /// <param name="ds">Labor data set</param>
        public void GetNewLbrScrapSerialNumbers(ref LaborTableset ds)
        {
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where !String.IsNullOrEmpty(ttLaborDtl_Row.RowMod)
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.TtLaborDtlRecordNotFound, "ttLaborDtl", "RowMod");
            }


            JobAsmbl = this.FindFirstJobAsmbl3(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
            if (JobAsmbl == null)
            {
                throw new BLException(Strings.JobAsmblRecordNotFound, "JobAsmbl", "AssemblySeq");
            }
            ttLbrScrapSerialNumbers = new Erp.Tablesets.LbrScrapSerialNumbersRow();
            CurrentFullTableset.LbrScrapSerialNumbers.Add(ttLbrScrapSerialNumbers);
            ttLbrScrapSerialNumbers.Company = ttLaborDtl.Company;
            ttLbrScrapSerialNumbers.JobNum = ttLaborDtl.JobNum;
            ttLbrScrapSerialNumbers.AssemblySeq = ttLaborDtl.AssemblySeq;
            ttLbrScrapSerialNumbers.OprSeq = ttLaborDtl.OprSeq;
            ttLbrScrapSerialNumbers.LaborHedSeq = ttLaborDtl.LaborHedSeq;
            ttLbrScrapSerialNumbers.LaborDtlSeq = ttLaborDtl.LaborDtlSeq;
            ttLbrScrapSerialNumbers.PartNum = JobAsmbl.PartNum;
            ttLbrScrapSerialNumbers.EnableStatus = true;
            ttLbrScrapSerialNumbers.SNStatus = ((ttLaborDtl.LaborQty == 0 && ttLaborDtl.ScrapQty > 0) ? "REJECTED" : ((ttLaborDtl.DiscrepQty > 0 && ttLaborDtl.LaborQty == 0) ? "INSPECTION" : "COMPLETE"));
            ttLbrScrapSerialNumbers.SNStatusDesc = ((ttLaborDtl.LaborQty == 0 && ttLaborDtl.ScrapQty > 0) ? Strings.Scrap : ((ttLaborDtl.DiscrepQty > 0 && ttLaborDtl.LaborQty == 0) ? Strings.Nonconformance : Strings.Completed));
            ttLbrScrapSerialNumbers.RowMod = IceRow.ROWSTATE_ADDED;
            ttLbrScrapSerialNumbers.SysRowID = Guid.NewGuid();
            ds = CurrentFullTableset;
        }

        /// <summary>
        /// Gets a new TimeWeeklyView record for the current week
        /// </summary>
        /// <param name="ds">Labor data set</param>
        /// <param name="ipEmployeeNum">The employee id for this labor record</param>
        /// <param name="ipDateInWeek">Date within the week for which a new TimeWeeklyView record is to be created</param>
        public void GetNewTimeWeeklyView(ref LaborTableset ds, string ipEmployeeNum, DateTime? ipDateInWeek)
        {
            CurrentFullTableset = ds;
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.TimeManagement) == false)
            {
                throw new BLException(Strings.TimeManagLicenseNotActiveAddNotAllowed, "TimeWeeklyView");
            }
            if (String.IsNullOrEmpty(ipEmployeeNum))
            {
                throw new BLException(Strings.EmployeeNotFound, "TimeWeeklyView", "EmployeeNum");
            }

            UserFile = UserFile.FindFirstByPrimaryKey(Db, Session.UserID);
            if (UserFile != null && UserFile.TERetrieveByDay == true)
            {
                throw new BLException(Strings.YouHaveChosenToRetriRecordsByDayWeeklyTimeView, "TimeWeeklyView");
            }
            if (ipDateInWeek == null)
            {
                if (Convert.ToInt32(CompanyTime.Now().DayOfWeek + 1) != 1)
                {
                    ldCalendarStartDate = (CompanyTime.Today().AddDays(1 - Convert.ToInt32(CompanyTime.Now().DayOfWeek + 1)));
                }
                if (Convert.ToInt32(CompanyTime.Now().DayOfWeek + 1) != 7)
                {
                    ldCalendarEndDate = (CompanyTime.Today().AddDays(7 - Convert.ToInt32(CompanyTime.Now().DayOfWeek + 1)));
                }
            }
            else
            {
                ldCalendarStartDate = ipDateInWeek;
                ldCalendarEndDate = ipDateInWeek;
                this.populateTimeValidateDates();
            }


            EmpBasic = this.FindFirstEmpBasic10(Session.CompanyID, ipEmployeeNum);
            ttTimeWeeklyView = new Erp.Tablesets.TimeWeeklyViewRow();
            CurrentFullTableset.TimeWeeklyView.Add(ttTimeWeeklyView);
            ttTimeWeeklyView.Company = Session.CompanyID;
            ttTimeWeeklyView.EmployeeNum = ipEmployeeNum;
            ttTimeWeeklyView.Shift = ((EmpBasic != null) ? EmpBasic.Shift : 0);
            if (ldCalendarStartDate == null)
            {
                ttTimeWeeklyView.WeekBeginDate = null;
            }
            else
            {
                ttTimeWeeklyView.WeekBeginDate = ldCalendarStartDate.Value.Date;
            }

            if (ldCalendarEndDate == null)
            {
                ttTimeWeeklyView.WeekEndDate = null;
            }
            else
            {
                ttTimeWeeklyView.WeekEndDate = ldCalendarEndDate.Value.Date;
            }

            ttTimeWeeklyView.WeekDisplayText = ldCalendarStartDate.ToShortDateString() + " - " + ldCalendarEndDate.ToShortDateString();
            ttTimeWeeklyView.LaborTypePseudo = "P"; /* production */
            ttTimeWeeklyView.LaborType = "P"; /* production */
            ttTimeWeeklyView.NewRowType = "A"; /* add */
            ttTimeWeeklyView.RowMod = IceRow.ROWSTATE_ADDED;
            ttTimeWeeklyView.TimeStatus = "E";
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) && EmpBasic != null &&
            !EmpBasic.AllowDirLbr)
            {
                ttTimeWeeklyView.LaborTypePseudo = "I";
                ttTimeWeeklyView.LaborType = "I";
                ttTimeWeeklyView.ResourceGrpID = EmpBasic.ResourceGrpID;
                ttTimeWeeklyView.ResourceID = EmpBasic.ResourceID;
                ttTimeWeeklyView.AllowDirLbr = false;
                string dept = FindFirstResourceGroupJCDept(Session.CompanyID, ttTimeWeeklyView.ResourceGrpID);
                ttTimeWeeklyView.JCDept = (dept != null) ? dept : "";
            }
            this.disPrjFieldsTimeWeeklyView();
            if (ttTimeWeeklyView.DisPrjRoleCd)
            {
                ttTimeWeeklyView.RoleCd = "";
                ttTimeWeeklyView.RoleCdDescription = "";
            }
            if (ttTimeWeeklyView.DisTimeTypCd)
            {
                ttTimeWeeklyView.TimeTypCd = "";
                ttTimeWeeklyView.TimeTypCdDescription = "";
            }

            ttTimeWeeklyView.TimeAutoSubmit = this.IsTimeAutoSubmitPlantConfCtrl(Session.CompanyID, Session.PlantID);
            TimeWeeklyView_Foreign_Link();
            ds = CurrentFullTableset;
        }

        ///<summary>
        ///</summary>
        public LaborTableset GetRowsCalendarView(string whereClauseLaborHed, string whereClauseLaborDtl, string whereClauseLaborDtlAttach, string whereClauseLaborDtlAction, string whereClauseLaborDtlCom, string whereClauseLaborEquip, string whereClauseLaborPart, string whereClauseLbrScrapSerialNumbers, string whereClauseTimeWorkHours, string whereClauseTimeWeeklyView, string whereClauseLaborDtlGroup, string whereClauseSelectedSerialNumbers, string whereClauseSNFormat, int pageSize, int absolutePage, string ipEmployeeNum, DateTime? ipCalendarStartDate, DateTime? ipCalendarEndDate, out bool morePages)
        {
            morePages = false;
            int rowCount = 0;
            bool retrieveByDay = false;
            lFromGetRowsCalendarView = true;

            getBackflushRecords = (whereClauseLaborDtl.IndexOf("LaborEntryMethod = 'B'", StringComparison.OrdinalIgnoreCase) > 0);
            whereClauseLaborDtl = whereClauseLaborDtl.Replace("LaborEntryMethod = 'B'", "").Trim();

            using (ErpCallContext.SetDisposableKey("LaborGetRows"))
            {
                /*** Get the rows */
                CurrentFullTableset = GetRows(whereClauseLaborHed, whereClauseLaborDtl, whereClauseLaborDtlAttach, whereClauseLaborDtlAction, whereClauseLaborDtlCom, whereClauseLaborEquip, whereClauseLaborPart, whereClauseLbrScrapSerialNumbers, whereClauseLaborDtlGroup, whereClauseSelectedSerialNumbers, whereClauseSNFormat, whereClauseTimeWeeklyView, whereClauseTimeWorkHours, 0, absolutePage, out morePages);
                /* page sizing */
            }

            if (getBackflushRecords)
            {
                //build backflush labordtl transactions
                foreach (var ttLaborHed in CurrentFullTableset.LaborHed.ToList())
                {
                    foreach (var laborDtl in this.SelectLaborDtlEntryMeth(Session.CompanyID, ttLaborHed.EmployeeNum, ttLaborHed.PayrollDate, "B"))
                    {
                        ttLaborDtl = new LaborDtlRow();
                        BufferCopy.Copy(laborDtl, ref ttLaborDtl);
                        ttLaborDtl.LaborHedSeq = ttLaborHed.LaborHedSeq;
                        CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
                        this.LaborDtlAfterGetRows(); //foreign link method is being called in LaborDtlAfterGetRows
                    }
                }
            }
            else
            {
                //clean backflush labordtl transactions
                foreach (var ttLaborDtl in CurrentFullTableset.LaborDtl.ToList())
                {
                    if (!ttLaborDtl.LaborEntryMethod.KeyEquals("B")) continue;
                    CurrentFullTableset.LaborPart.RemoveAll(ttLaborPart_Row => ttLaborPart_Row.Company.Compare(ttLaborDtl.Company) == 0
                                        && ttLaborPart_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                                        && ttLaborPart_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq);
                    CurrentFullTableset.LaborDtl.Remove(ttLaborDtl);
                }
            }


            foreach (var ttLaborHed in CurrentFullTableset.LaborHed.ToList())
            {
                if (pageSize > 0)
                {
                    rowCount = rowCount + 1;
                    if (rowCount > pageSize)
                    {
                        morePages = true;


                        CurrentFullTableset.LaborDtl.RemoveAll(ttLaborDtl_Row => ttLaborDtl_Row.LaborHedSeq == ttLaborHed.LaborHedSeq);
                        CurrentFullTableset.LaborHed.Remove(ttLaborHed);
                    }
                }
            }

            using (ErpCallContext.SetDisposableKey("GetRowsCalendarView"))
            {
                lcEmployeeNum = ipEmployeeNum;
                ldCalendarStartDate = ipCalendarStartDate;
                ldCalendarEndDate = ipCalendarEndDate;
                if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.TimeManagement))
                {
                    UserFile = UserFile.FindFirstByPrimaryKey(Db, Session.UserID);
                    retrieveByDay = UserFile != null && UserFile.TERetrieveByDay;
                    if (retrieveByDay == false)
                    {
                        this.populateTimeValidateDates();
                        this.populateTimeWorkHours();
                        this.populateTimeWeeklyView();
                    }
                }
            }
            return CurrentFullTableset;
        }

        ///<summary>
        ///</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:query is checked for injections")]
        public LaborTableset GetRowsWhoIsHere(string whereClauseLaborHed, string whereClauseLaborDtl, string whereClauseLaborDtlCom, string whereClauseLaborEquip, string whereClauseLaborPart, string whereClauseLbrScrapSerialNumbers, string whereClauseTimeWorkHours, string whereClauseTimeWeeklyView, string whereClauseLaborDtlGroup, string whereClauseSelectedSerialNumbers, string whereClauseSNFormat, int pageSize, int absolutePage, string ipSupervisorID, out bool morePages)
        {
            this.sqlConnection = Db.SqlConnection;
            morePages = false;
            string LaborSortBy = string.Empty;
            int startpos = 1;
            int stringpos = 1;
            int endstringpos = 1;
            string compSupID = string.Empty;
            string whereclause = string.Empty;
            string selectClause = string.Empty;
            string selectClauseForCount = string.Empty;
            string whereClauseLaborHedForCount = string.Empty;
            string joinEmpBasic = string.Empty;

            /* if sorting by supervisor, parse it out of the where clause
               since it's not an actual db field */
            if ((whereClauseLaborHed.IndexOf("EmpBasicSupervisorID", StringComparison.OrdinalIgnoreCase) + 1) > 0)
            {
                whereclause = "";
                startpos = (whereClauseLaborHed.IndexOf("EmpBasicSupervisorID", startpos - 1, StringComparison.OrdinalIgnoreCase) + 1);
                if (startpos > 0)
                {
                    stringpos = (whereClauseLaborHed.IndexOf("'", startpos - 1) + 1);
                }

                if (startpos > 0 && stringpos > 0)
                {
                    endstringpos = (whereClauseLaborHed.IndexOf("'", stringpos + 1 - 1) + 1) - 1;
                }

                if (stringpos > 0 && endstringpos > 0)
                {
                    compSupID = whereClauseLaborHed.SubString(stringpos + 1 - 1, endstringpos - stringpos);
                }

                if (startpos > 0 && endstringpos > 0)
                {
                    whereclause = whereClauseLaborHed.SubString(0, startpos - 6);
                    whereclause = whereclause + whereClauseLaborHed.SubString(endstringpos + 2 - 1, whereClauseLaborHed.Length - 1);
                }
            }
            if (!String.IsNullOrEmpty(whereclause))
            {
                whereClauseLaborHed = whereclause;
            }

            LaborSortBy = Ice.Manager.Data.ParseSort(ref whereClauseLaborHed);

            if (!String.IsNullOrEmpty(whereClauseLaborHed))
            {
                whereClauseLaborHed = $" where LaborHed.Company = '{Session.CompanyID}' and " + whereClauseLaborHed;
            }
            else
            {
                whereClauseLaborHed = $" where LaborHed.Company = '{Session.CompanyID}'";
            }

            if ((whereClauseLaborHed.IndexOf("EmployeeNumName", StringComparison.OrdinalIgnoreCase) + 1) > 0 || (LaborSortBy.IndexOf("EmployeeNumName", StringComparison.OrdinalIgnoreCase) + 1) > 0)
            {
                joinEmpBasic = " inner join Erp.EmpBasic with(nolock) on EmpBasic.Company = LaborHed.Company and EmpBasic.EmpID = LaborHed.EmployeeNum ";
                whereClauseLaborHed = joinEmpBasic + whereClauseLaborHed.Replace("EmployeeNumName", "EmpBasic.Name");
            }

            if (LaborSortBy != string.Empty)
            {
                LaborSortBy = " order " + LaborSortBy;
                LaborSortBy = LaborSortBy.Replace("EmployeeNumName", "EmpBasic.Name");
            }
            else
            {
                LaborSortBy = " order by LaborHedSeq";
            }

            selectClause = " from Erp.LaborHed with (nolock) " + whereClauseLaborHed;

            if (!String.IsNullOrEmpty(ipSupervisorID) || !String.IsNullOrEmpty(compSupID))
            {
                string vSuppervisID = !String.IsNullOrEmpty(ipSupervisorID) ? ipSupervisorID : compSupID;
                selectClause = selectClause + " and  EmployeeNum in " +
                $" (select EmpID from erp.EmpBasic where EmpBasic.Company = '{Session.CompanyID}' " +
                $" and SupervisorID = '{vSuppervisID}')";
            }


            selectClauseForCount = "select Count(*) " + selectClause;

            if (pageSize != 0)
            {
                if (absolutePage <= 1)
                {
                    selectClause = $"select top {pageSize} * " + selectClause + LaborSortBy;
                }
                else
                {
                    int startRow = pageSize * (absolutePage - 1) + 1;
                    int endRow = pageSize * absolutePage;

                    selectClause = $"select * from ( select ROW_NUMBER() over ( {LaborSortBy} ) as RowNumber, * " + selectClause +
                                   $") as rr where RowNumber between {startRow} and {endRow}";
                }
            }
            else
            {
                selectClause = "select * " + selectClause + LaborSortBy;
            }


            System.Data.SqlClient.SqlDataReader reader;
            Epicor.Data.SafeSql.CheckSQLInjection.ValidateSyntax(selectClause);
            using (System.Data.SqlClient.SqlCommand queryHandle = new System.Data.SqlClient.SqlCommand(selectClause, sqlConnection))
            {
                reader = queryHandle.ExecuteReader();
            }

            List<LaborHedRow> LaborHedRows = new List<LaborHedRow>();
            Epicor.ServiceModel.Tableset.DBReaderAdapter.LoadFromReader(reader, LaborHedRows);

            var laborHedQuery = (from row in LaborHedRows select row).ToList();

            foreach (var laborHedQuery_iterator in (laborHedQuery).ToList())
            {
                ttLaborHed = laborHedQuery_iterator;
                CurrentFullTableset.LaborHed.Add(ttLaborHed);
                this.LaborHedAfterGetRows();
                LaborHed_Foreign_Link();

            }

            // Handle paging
            if (pageSize > 0)
            {
                int rowsCnt;
                Epicor.Data.SafeSql.CheckSQLInjection.ValidateSyntax(selectClauseForCount);
                using (System.Data.SqlClient.SqlCommand queryHandle = new System.Data.SqlClient.SqlCommand(selectClauseForCount, sqlConnection))
                {
                    rowsCnt = (Int32)queryHandle.ExecuteScalar();
                }

                if (absolutePage == 0)
                {
                    morePages = (rowsCnt > pageSize);
                }
                else
                {
                    morePages = (rowsCnt > (pageSize * absolutePage));
                }
            }
            return CurrentFullTableset;
        }

        /// <summary>
        /// Populates LbrScrapSerialNumbers from SerialNo
        /// </summary>
        private void getSerialNumbers()
        {
            JobAsmbl = FindFirstJobAsmbl3(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
            if (JobAsmbl == null)
            {
                throw new BLException(Strings.JobAsNotFoundCannotRetriSerialNumbers, "JobAsmbl");
            }

            if (ttLaborDtl.ReWork)
            {
                foreach (var SNTran_iterator in SelectSNTran(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, "OPR-RWK", ttLaborDtl.OprSeq))
                {
                    var SNTran = SNTran_iterator;
                    ttLbrScrapSerialNumbers = new Erp.Tablesets.LbrScrapSerialNumbersRow();
                    CurrentFullTableset.LbrScrapSerialNumbers.Add(ttLbrScrapSerialNumbers);
                    ttLbrScrapSerialNumbers.Company = ttLaborDtl.Company;
                    ttLbrScrapSerialNumbers.LaborHedSeq = ttLaborDtl.LaborHedSeq;
                    ttLbrScrapSerialNumbers.LaborDtlSeq = ttLaborDtl.LaborDtlSeq;
                    ttLbrScrapSerialNumbers.SerialNumber = SNTran.SerialNumber;
                    ttLbrScrapSerialNumbers.PartNum = SNTran.PartNum;
                    ttLbrScrapSerialNumbers.JobNum = SNTran.JobNum;
                    ttLbrScrapSerialNumbers.AssemblySeq = SNTran.AssemblySeq;
                    ttLbrScrapSerialNumbers.OprSeq = ttLaborDtl.OprSeq;
                    ttLbrScrapSerialNumbers.SNStatus = SNTran.SNStatus;
                    ttLbrScrapSerialNumbers.EnableStatus = false;
                    ttLbrScrapSerialNumbers.SysRowID = Guid.NewGuid();
                    if (ttLaborDtl.EndActivity && ttLbrScrapSerialNumbers.SNStatus.KeyEquals("WIP"))
                    {
                        ttLbrScrapSerialNumbers.SNStatus = "COMPLETE";
                    }

                    assignSNStatusDesc();
                }
            }
            else
            {
                int attributeSetID = ttLaborDtl.LaborAttributeSetID > 0 ? ttLaborDtl.LaborAttributeSetID : ttLaborDtl.ScrapAttributeSetID > 0 ? ttLaborDtl.ScrapAttributeSetID : ttLaborDtl.DiscrepAttributeSetID > 0 ? ttLaborDtl.DiscrepAttributeSetID : 0;
                foreach (var SerialNo_iterator in SelectSerialNo(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, 0, JobAsmbl.PartNum, attributeSetID, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq))
                {
                    var JoinFieldsResult = SerialNo_iterator;
                    bool delSerialNo = false;

                    if (delttLbrScrapSerialNumbersRows != null)
                    {
                        foreach (var delSerialNo_iterator in delttLbrScrapSerialNumbersRows)
                        {
                            if (JoinFieldsResult.AssemblySeq == delSerialNo_iterator.AssemblySeq &&
                                JoinFieldsResult.Company.KeyEquals(delSerialNo_iterator.Company) &&
                                JoinFieldsResult.JobNum.KeyEquals(delSerialNo_iterator.JobNum) &&
                                JoinFieldsResult.ScrapLaborDtlSeq == delSerialNo_iterator.LaborDtlSeq &&
                                JoinFieldsResult.ScrapLaborHedSeq == delSerialNo_iterator.LaborHedSeq &&
                                JoinFieldsResult.SerialNumber.KeyEquals(delSerialNo_iterator.SerialNumber))
                            {
                                delttLbrScrapSerialNumbersRows.Remove(delSerialNo_iterator);
                                delSerialNo = true;
                                break;
                            }
                        }
                    }
                    if (delSerialNo)
                        continue;

                    ttLbrScrapSerialNumbers = (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                               where ttLbrScrapSerialNumbers_Row.SerialNumber.KeyEquals(JoinFieldsResult.SerialNumber)
                                               select ttLbrScrapSerialNumbers_Row).FirstOrDefault();
                    if (ttLbrScrapSerialNumbers == null)
                    {
                        ttLbrScrapSerialNumbers = new Erp.Tablesets.LbrScrapSerialNumbersRow();
                        CurrentFullTableset.LbrScrapSerialNumbers.Add(ttLbrScrapSerialNumbers);
                        ttLbrScrapSerialNumbers.SysRowID = Guid.NewGuid();
                    }
                    ttLbrScrapSerialNumbers.Company = JoinFieldsResult.Company;
                    ttLbrScrapSerialNumbers.LaborHedSeq = JoinFieldsResult.ScrapLaborHedSeq;
                    ttLbrScrapSerialNumbers.LaborDtlSeq = JoinFieldsResult.ScrapLaborDtlSeq;
                    ttLbrScrapSerialNumbers.SerialNumber = JoinFieldsResult.SerialNumber;
                    ttLbrScrapSerialNumbers.PartNum = JobAsmbl.PartNum;
                    ttLbrScrapSerialNumbers.JobNum = JoinFieldsResult.JobNum;
                    ttLbrScrapSerialNumbers.AssemblySeq = JoinFieldsResult.AssemblySeq;
                    ttLbrScrapSerialNumbers.OprSeq = ttLaborDtl.OprSeq;
                    ttLbrScrapSerialNumbers.SNStatus = JoinFieldsResult.SNStatus;
                    ttLbrScrapSerialNumbers.EnableStatus = false;
                    if (ttLbrScrapSerialNumbers.SNStatus.Compare("WIP") == 0 || ttLbrScrapSerialNumbers.SNStatus.Compare("CONSUMED") == 0)
                    {
                        ttLbrScrapSerialNumbers.SNStatus = "COMPLETE";
                    }

                    if (ttLbrScrapSerialNumbers.SNStatus.Compare("COMPLETE") == 0)
                    {
                        ttLbrScrapSerialNumbers.EnableStatus = true;
                    }
                    else if (ttLaborDtl.ScrapQty > 0 && ttLbrScrapSerialNumbers.SNStatus.Compare("REJECTED") == 0)
                    {
                        ttLbrScrapSerialNumbers.EnableStatus = true;
                    }
                    else if (ttLaborDtl.DiscrepQty > 0 && ttLbrScrapSerialNumbers.SNStatus.Compare("INSPECTION") == 0)
                    {
                        ttLbrScrapSerialNumbers.EnableStatus = true;
                    }

                    assignSNStatusDesc();
                }

                foreach (var SNTran_iterator in SelectSNTran(ttLaborDtl.Company, "OPR-CMP", JobAsmbl.PartNum, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.OprSeq))
                {
                    var JoinFieldsResult = SNTran_iterator;

                    if (!((from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                           where ttLbrScrapSerialNumbers_Row.Company.Compare(JoinFieldsResult.Company) == 0
                           && ttLbrScrapSerialNumbers_Row.SerialNumber.Compare(JoinFieldsResult.SerialNumber) == 0
                           && ttLbrScrapSerialNumbers_Row.PartNum.Compare(JobAsmbl.PartNum) == 0
                           && ttLbrScrapSerialNumbers_Row.LaborHedSeq == JoinFieldsResult.ScrapLaborHedSeq
                           && ttLbrScrapSerialNumbers_Row.LaborDtlSeq == JoinFieldsResult.ScrapLaborDtlSeq
                           select ttLbrScrapSerialNumbers_Row).Any()))
                    {
                        ttLbrScrapSerialNumbers = new Erp.Tablesets.LbrScrapSerialNumbersRow();
                        CurrentFullTableset.LbrScrapSerialNumbers.Add(ttLbrScrapSerialNumbers);
                        ttLbrScrapSerialNumbers.Company = JoinFieldsResult.Company;
                        ttLbrScrapSerialNumbers.LaborHedSeq = JoinFieldsResult.ScrapLaborHedSeq;
                        ttLbrScrapSerialNumbers.LaborDtlSeq = JoinFieldsResult.ScrapLaborDtlSeq;
                        ttLbrScrapSerialNumbers.SerialNumber = JoinFieldsResult.SerialNumber;
                        ttLbrScrapSerialNumbers.PartNum = JobAsmbl.PartNum;
                        ttLbrScrapSerialNumbers.JobNum = ttLaborDtl.JobNum;
                        ttLbrScrapSerialNumbers.AssemblySeq = ttLaborDtl.AssemblySeq;
                        ttLbrScrapSerialNumbers.OprSeq = ttLaborDtl.OprSeq;
                        ttLbrScrapSerialNumbers.EnableStatus = true;
                        ttLbrScrapSerialNumbers.SNStatus = "COMPLETE";
                        ttLbrScrapSerialNumbers.SysRowID = Guid.NewGuid();

                        assignSNStatusDesc();
                    }
                }
            }
        }

        private void assignSNStatusDesc()
        {
            if (ttLbrScrapSerialNumbers.SNStatus.Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
            {
                ttLbrScrapSerialNumbers.SNStatusDesc = Strings.Completed;
            }
            else if (ttLbrScrapSerialNumbers.SNStatus.Equals("INSPECTION", StringComparison.OrdinalIgnoreCase))
            {
                ttLbrScrapSerialNumbers.SNStatusDesc = Strings.Nonconformance;
            }
            else if (ttLbrScrapSerialNumbers.SNStatus.Equals("REJECTED", StringComparison.OrdinalIgnoreCase))
            {
                ttLbrScrapSerialNumbers.SNStatusDesc = Strings.Scrap;
            }
        }

        /// <summary>
        /// Method to get the value UserFile.TERetrieveApproved 
        /// </summary>
        /// <param name="opTERetrieveApproved">Should Approved records be retrieved on search? Yes/No</param>
        public void GetTERetrieveApproved(out bool opTERetrieveApproved)
        {
            opTERetrieveApproved = false;


            UserFile = this.FindFirstUserFile(Session.UserID);
            if (UserFile != null)
            {
                opTERetrieveApproved = UserFile.TERetrieveApproved;
            }
        }

        /// <summary>
        /// Method to get retrieve by options 
        /// </summary>
        /// <param name="opTERetrieveByDay">Should records be retrieve one day at a time? Yes/No</param>
        /// <param name="opTERetrieveByWeek">Should records be retrieve a week at a time? Yes/No</param>
        /// <param name="opTERetrieveByMonth">Should records be retrieve a month at a time? Yes/No</param>
        public void GetTERetrieveByOption(out bool opTERetrieveByDay, out bool opTERetrieveByWeek, out bool opTERetrieveByMonth)
        {
            opTERetrieveByDay = false;


            opTERetrieveByWeek = false;
            opTERetrieveByMonth = false;

            UserFile = UserFile.FindFirstByPrimaryKey(Db, Session.UserID);
            if (UserFile == null) return;

            opTERetrieveByDay = UserFile.TERetrieveByDay;
            opTERetrieveByWeek = UserFile.TERetrieveByWeek;
            opTERetrieveByMonth = UserFile.TERetrieveByMonth;
            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
            {
                if (opTERetrieveByDay == false &&
                    opTERetrieveByWeek == false &&
                    opTERetrieveByMonth == false)
                {
                    opTERetrieveByMonth = true;

                    UserFile = FindFirstUserFileUpd(Session.UserID);
                    if (UserFile != null)
                    {
                        UserFile.TERetrieveByMonth = opTERetrieveByMonth;
                        Db.Validate(UserFile);
                    }
                }
                txScope.Complete();
            }
        }

        /// <summary>
        /// Method to get the value UserFile.TERetrieveEntered 
        /// </summary>
        /// <param name="opTERetrieveEntered">Should Entered records be retrieved on search? Yes/No</param>
        public void GetTERetrieveEntered(out bool opTERetrieveEntered)
        {
            opTERetrieveEntered = false;


            UserFile = this.FindFirstUserFile(Session.UserID);
            if (UserFile != null)
            {
                opTERetrieveEntered = UserFile.TERetrieveEntered;
            }
        }

        /// <summary>
        /// Method to get the value UserFile.TERetrievePartiallyApproved 
        /// </summary>
        /// <param name="opTERetrievePartiallyApproved">Should Partially Approved records be retrieved on search? Yes/No</param>
        public void GetTERetrievePartiallyApproved(out bool opTERetrievePartiallyApproved)
        {
            opTERetrievePartiallyApproved = false;


            UserFile = this.FindFirstUserFile(Session.UserID);
            if (UserFile != null)
            {
                opTERetrievePartiallyApproved = UserFile.TERetrievePartiallyApproved;
            }
        }

        /// <summary>
        /// Method to get the value UserFile.TERetrieveRejected 
        /// </summary>
        /// <param name="opTERetrieveRejected">Should Rejected records be retrieved on search? Yes/No</param>
        public void GetTERetrieveRejected(out bool opTERetrieveRejected)
        {
            opTERetrieveRejected = false;


            UserFile = this.FindFirstUserFile(Session.UserID);
            if (UserFile != null)
            {
                opTERetrieveRejected = UserFile.TERetrieveRejected;
            }
        }

        /// <summary>
        /// Method to get the value UserFile.TERetrieveSubmitted 
        /// </summary>
        /// <param name="opTERetrieveSubmitted">Should Submitted records be retrieved on search? Yes/No</param>
        public void GetTERetrieveSubmitted(out bool opTERetrieveSubmitted)
        {
            opTERetrieveSubmitted = false;


            UserFile = this.FindFirstUserFile(Session.UserID);
            if (UserFile != null)
            {
                opTERetrieveSubmitted = UserFile.TERetrieveSubmitted;
            }
        }

        /// <summary>
        /// Validate if an assembly is valid for a job. if not returns false, 
        /// otherwise returns true.
        /// </summary>
        /// <param name="pcJobNum">Job number to which this labor transaction applies.</param>
        /// <param name="piAssemblySeq">The Assembly Sequence of the Job that the labor transaction applies to.</param>
        /// <param name="plFound">Found YES/NO</param>
        public void IsValidAssembly(string pcJobNum, int piAssemblySeq, out bool plFound)
        {
            plFound = false;
            plFound = false;
            if ((this.ExistsJobHead(Session.CompanyID, pcJobNum, false, true)))
            {


                if ((this.ExistsJobAsmbl(Session.CompanyID, pcJobNum, piAssemblySeq)))
                {
                    plFound = true;
                }
                else
                {
                    throw new BLException(Strings.AValidAssemblyIsRequired, "JobAsmbl", "AssemblySeq");
                }
            }
            else
            {
                throw new BLException(Strings.ThisJobHasNotBeenReleaEntryNotAllowed, "JobHead", "JobNum");
            }
        }

        partial void LaborDtlAfterDelete()
        {
            if (DelFlag)
            {
                if (SNVerify)
                {
                    string tranType = string.Empty;
                    string vKey = string.Empty;
                    string vChar = string.Empty;
                    DateTime? PTSysDate = null;
                    int PTSysTime = 0;
                    int PTTranNum = 0;
                    Guid SNTranRowId = Guid.Empty;
                    bool lgPlantIsFullSerialTracking = false;

                    vKey = DelCompany + Ice.Constants.LIST_DELIM + DelJobNum + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(DelAssemblySeq) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(ttLaborDtl.OprSeq) + Ice.Constants.LIST_DELIM + Session.SessionID;
                    vChar = this.LibUsePatchFld.GetPatchFldChar(Session.CompanyID, "jobOper", "autoRecPartTran", vKey);
                    if (!String.IsNullOrEmpty(vChar))
                    {
                        PTSysDate = Compatibility.Convert.ToDateTime(vChar.Entry(1, Ice.Constants.LIST_DELIM));
                        PTSysTime = Compatibility.Convert.ToInt32(vChar.Entry(2, Ice.Constants.LIST_DELIM));
                        PTTranNum = Compatibility.Convert.ToInt32(vChar.Entry(3, Ice.Constants.LIST_DELIM));

                        PartTran = this.FindFirstPartTran(ttLaborDtl.Company, PTSysDate.Value, PTSysTime, PTTranNum);
                    }

                    if (!String.IsNullOrEmpty(vChar) && PartTran != null && delSerialNoRows != null)
                    {

                        foreach (var delSerNo in delSerialNoRows)
                        {
                            SerialNo serialNo = FindFirstSerialNoWithUpdLock(DelCompany, DelJobNum, DelAssemblySeq, 0, delSerNo.delSerialNumber);
                            if (serialNo != null && serialNo.SNStatus.KeyEquals(Strings.InventoryStatus))
                            {
                                serialNo.WareHouseCode = string.Empty;
                                serialNo.BinNum = string.Empty;
                                serialNo.SNStatus = serialNo.PrevSNStatus;
                                serialNo.PrevSNStatus = Strings.InventoryStatus;

                                SNTran = this.FindLastSNTranWithUpdLock(Session.CompanyID, PartTran.PartNum, serialNo.SerialNumber, PartTran.TranType);
                                if (SNTran != null)
                                {
                                    SNTran.ScrapLaborDtlSeq = 0;
                                    SNTran.ScrapLaborHedSeq = 0;
                                    SNTran.LastLbrOprSeq = 0;

                                    PlantConfCtrl = this.FindFirstPlantConfCtrl(Session.CompanyID, PartTran.Plant);
                                    lgPlantIsFullSerialTracking = (PlantConfCtrl != null && PlantConfCtrl.SerialTracking == 2) ? true : false;

                                    if (ExistsUniqueSNTran(serialNo.Company, PartTran.PartNum, serialNo.SerialNumber, PartTran.TranType) && !lgPlantIsFullSerialTracking)
                                    {
                                        Db.SNTran.Delete(SNTran);
                                    }
                                    tranType = (PartTran.TranQty < 0) ? PartTran.TranType + "-" : PartTran.TranType;

                                    LibGetNewSNtran._GetNewSNtran(serialNo, tranType, PartTran.TranDate, out SNTranRowId);
                                    ttSNTranRows = ttSNTranRows ?? new List<Erp.Internal.IM.PartTranSNtranLink.SNtran2>();
                                    this.LibPartTranSNtranLink.createttSNtranRecord(SNTranRowId, ref ttSNTranRows, ref ttSNTran);
                                } // SNTran != null
                            } // SerialNo != null && SerialNo.SNStatus == Strings.InventoryStatus
                        } // foreach delSerNo

                        this.LibUsePatchFld.DeletePatchFld("jobOper", "autoRecPartTran", vKey);

                        this.LibPartTranSNtranLink.createSNTranPartTranLink(PartTran, ref ttSNTranRows);
                    } //!String.IsNullOrEmpty(vChar) && PartTran != null

                    if (delSerialNoRows != null)
                    {
                        delSerialNoRows.Clear();
                    }

                } // SNVerify

                this.createDelttTimeWeeklyView();
                this.deleteFSAExtFlds(ttLaborDtl.SysRowID);

                //ERP-335 - this is intentional to use PatchFld - very short lived record
                //DO NOT remove this PatchFLd
                string patchKey = Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                LibUsePatchFld.DeletePatchFld("LaborDtl", "IntExternalKey", patchKey);

                //Attributes
                LibUsePatchFld.DeletePatchFld("LaborDtl", "SQAttributeSetID", patchKey);
                LibUsePatchFld.DeletePatchFld("LaborDtl", "NCAttributeSetID", patchKey);

            }
        }

        partial void LaborDtlAfterGetRows()
        {
            bool olApprover = false;
            string ocSalesRepCode = string.Empty;
            bool supervisorHasRights = false;
            bool useEmployeeRules = false;
            bool useApproverRules = false;
            bool disallowTimeEntry = false;
            int setType = 0;
            bool userCanUpdate = false;
            string vMessage = string.Empty;
            ttLaborDtl.HasAccessToRow = true;
            ttLaborDtl.HasComments = false;
            ttLaborDtl.JobType = this.getJobType(ttLaborDtl.JobNum);
            ttLaborDtl.FSComplete = this.getComplete(ttLaborDtl.CallNum);
            ttLaborDtl.ProdDesc = this.getProdDesc(ttLaborDtl.LaborType);
            ttLaborDtl.DisplayJob = this.getDisplayJob(ttLaborDtl.LaborType, ttLaborDtl.IndirectCode, ttLaborDtl.JobNum);
            ttLaborDtl.CompleteFlag = ((ttLaborDtl.CallNum == 0) ? ttLaborDtl.Complete : ttLaborDtl.FSComplete);
            ttLaborDtl.LaborCost = LibRoundAmountEF.RoundDecimalsTT(ttLaborDtl.LaborHrs * ttLaborDtl.LaborRate, ttLaborDtl, "LaborCost");
            ttLaborDtl.BurdenCost = LibRoundAmountEF.RoundDecimalsTT(ttLaborDtl.BurdenHrs * ttLaborDtl.BurdenRate, ttLaborDtl, "BurdenCost");
            ttLaborDtl.JCSystReworkReasons = ttJCSyst.ReworkReasons;
            ttLaborDtl.EnableSN = enableSN(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.PCID, ttLaborDtl.PartNum);
            ttLaborDtl.ProjPhaseID = (ttLaborDtl.ProjectID + Ice.Constants.LIST_DELIM + ttLaborDtl.PhaseID).KeyEquals(Ice.Constants.LIST_DELIM) ? string.Empty : ttLaborDtl.ProjectID + Ice.Constants.LIST_DELIM + ttLaborDtl.PhaseID;
            /* If JCSyst.ClockFormat = "M" then display the time as Hours:Minutes else output it as Hours.Hundreths. */
            ttLaborDtl.DspCreateTime = ((ttJCSyst.ClockFormat.Compare("M") == 0) ? Compatibility.Convert.TimeToString(ttLaborDtl.CreateTime, "HH:MM") : Compatibility.Convert.ToString(((decimal)ttLaborDtl.CreateTime / 3600M), ">9.99"));
            ttLaborDtl.DspChangeTime = ((ttJCSyst.ClockFormat.Compare("M") == 0) ? Compatibility.Convert.TimeToString(ttLaborDtl.ChangeTime, "HH:MM") : Compatibility.Convert.ToString(((decimal)ttLaborDtl.ChangeTime / 3600M), ">9.99"));
            ttLaborDtl.WeekDisplayText = ttLaborDtl.PayrollDate.Value.AddDays(1 - Convert.ToInt32(ttLaborDtl.PayrollDate.Value.DayOfWeek + 1)).ToShortDateString() + " - " +
                                         ttLaborDtl.PayrollDate.Value.AddDays(7 - Convert.ToInt32(ttLaborDtl.PayrollDate.Value.DayOfWeek + 1)).ToShortDateString();

            ttLaborDtl.EfficiencyPercentage = (ttLaborDtl.BurdenHrs == 0) ? 0 : (ttLaborDtl.EarnedHrs / ttLaborDtl.BurdenHrs * 100);

            readFSAExtFlds(ref ttLaborDtl);

            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.TimeManagement) == true)
            {
                var outApprovedBy = ttLaborDtl.ApprovedBy;
                var outPendingApprovalBy = ttLaborDtl.PendingApprovalBy;
                LibTEApproverLists._TEApproverLists("LaborDtl", Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq), Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq), out outApprovedBy, out outPendingApprovalBy);
                ttLaborDtl.ApprovedBy = outApprovedBy;
                ttLaborDtl.PendingApprovalBy = outPendingApprovalBy;
            }
            this.calcTotHrs(ttLaborDtl.LaborHrs);


            /* SCR 94819 format time for display - do this prior to the resetting of ClockOutTime to 0 if midnight so dspClockOutTime would still show 24 */
            if (ttJCSyst.ClockFormat.Compare("M") == 0)      /* Hrs:Minutes */
            {
                ttLaborDtl.DspClockInTime = this.convMin(Compatibility.Convert.ToString(ttLaborDtl.ClockinTime));
                ttLaborDtl.DspClockOutTime = this.convMin(Compatibility.Convert.ToString(ttLaborDtl.ClockOutTime));
            }
            else        /* Hrs.Hundreths */
            {
                ttLaborDtl.DspClockInTime = this.convDec(Compatibility.Convert.ToString(ttLaborDtl.ClockinTime));
                ttLaborDtl.DspClockOutTime = this.convDec(Compatibility.Convert.ToString(ttLaborDtl.ClockOutTime));
            }

            /* temporarily set clockouttime to 0 if midnight*/
            if (ttLaborDtl.ClockOutTime == 24.0m)
            {
                ttLaborDtl.ClockOutTime = 0;
            }

            if (ttLaborDtl.EnableSN)
            {
                this.getSerialNumbers();
            }

            var JobAsmblPartial = this.FindFirstJobAsmblPartial(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
            if (JobAsmblPartial != null)
            {
                ttLaborDtl.LaborUOM = JobAsmblPartial.IUM;
                ttLaborDtl.ScrapUOM = JobAsmblPartial.IUM;
                ttLaborDtl.DiscrepUOM = JobAsmblPartial.IUM;
                ttLaborDtl.LaborRevision = JobAsmblPartial.RevisionNum;
                ttLaborDtl.ScrapRevision = JobAsmblPartial.RevisionNum;
                ttLaborDtl.DiscrepRevision = JobAsmblPartial.RevisionNum;

            }
            else
            {
                var JobHeadPartial = this.FindFirstJobHeadPartial(ttLaborDtl.Company, ttLaborDtl.JobNum);
                if (JobHeadPartial != null)
                {
                    ttLaborDtl.LaborUOM = JobHeadPartial.IUM;
                    ttLaborDtl.ScrapUOM = JobHeadPartial.IUM;
                    ttLaborDtl.DiscrepUOM = JobHeadPartial.IUM;
                    ttLaborDtl.LaborRevision = JobHeadPartial.RevisionNum;
                    ttLaborDtl.ScrapRevision = JobHeadPartial.RevisionNum;
                    ttLaborDtl.DiscrepRevision = JobHeadPartial.RevisionNum;
                }
            }

            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.EnhancedQualityAssurance))
            {


                if ((this.ExistsJobOperInsp(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq)) && ttLaborDtl.ReWork == false)
                {
                    ttLaborDtl.EnableInspection = true;
                }
                else
                {
                    ttLaborDtl.EnableInspection = false;
                }

                // Attributes
                // Labor
                if (ttLaborDtl.LaborQty > 0 && ttLaborDtl.LaborAttributeSetID != 0)
                {
                    this.getAttributeDescriptions(ttLaborDtl, "L");
                }

                // Discrep
                if (ttLaborDtl.DiscrepQty > 0 && ttLaborDtl.DiscrepAttributeSetID != 0)
                {
                    this.getAttributeDescriptions(ttLaborDtl, "D");
                }

                // Scrap
                if (ttLaborDtl.ScrapQty > 0 && ttLaborDtl.ScrapAttributeSetID != 0)
                {
                    this.getAttributeDescriptions(ttLaborDtl, "S");
                }

                JobAsmblPartResult jobAsmblPart = FindFirstJobAsmblPart(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
                if (jobAsmblPart != null)
                {
                    ttLaborDtl.PartNum = jobAsmblPart.PartNum;

                    PartPartial pp = FindFirstPartPartial(ttLaborDtl.Company, ttLaborDtl.PartNum);
                    if (pp != null)
                    {
                        ttLaborDtl.AttrClassID = pp.AttrClassID;
                        ttLaborDtl.TrackInventoryByRevision = pp.TrackInventoryByRevision;
                    }
                }
            }

            /* project */
            if (ttLaborDtl.LaborTypePseudo.Compare("J") == 0)
            {
                ttLaborDtl.PhaseJobNum = ttLaborDtl.JobNum;
                ttLaborDtl.PhaseOprSeq = ttLaborDtl.OprSeq;
            }
            /* Set DisPrjRoleCd and DisTimeTypCd */
            this.disPrjFields(false);    /* Set DisLaborType */
            this.disLaborTypeProc();
            ttLaborDtl.TreeNodeImageName = ((ttLaborDtl.TimeStatus.Compare("E") == 0) ? "Entered" : ((ttLaborDtl.TimeStatus.Compare("S") == 0) ? "Submitted" : ((ttLaborDtl.TimeStatus.Compare("P") == 0) ? "PartiallyApproved" : ((ttLaborDtl.TimeStatus.Compare("A") == 0) ? "Approved" : ((ttLaborDtl.TimeStatus.Compare("R") == 0) ? "Rejected" : "Entered")))));
            ttLaborDtl.NotSubmitted = (String.IsNullOrEmpty(ttLaborDtl.TimeStatus) ||
                                       ttLaborDtl.TimeStatus.Compare("E") == 0 ||
                                       ttLaborDtl.TimeStatus.Compare("R") == 0);
            olApprover = this.canApprove(ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);
            this.checkIfNonConfProcessed(ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);

            ttLaborDtl.ReportPartQtyAllowed = ReportPartQtyAllowed(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);

            if (EmpBasic == null || (EmpBasic != null && !EmpBasic.EmpID.KeyEquals(ttLaborDtl.EmployeeNum)))
                EmpBasic = this.FindFirstEmpBasic11(Session.CompanyID, ttLaborDtl.EmployeeNum);
            if (EmpBasic != null)
            {
                ttLaborDtl.EnableComplete = EmpBasic.CanReportQty;

                ttLaborDtl.EnableScrapQty = !ttLaborDtl.LaborEntryMethod.Equals("X", StringComparison.OrdinalIgnoreCase) && EmpBasic.CanReportScrapQty && !ttLaborDtl.ReportPartQtyAllowed;
                ttLaborDtl.EnableDiscrepQty = !ttLaborDtl.LaborEntryMethod.Equals("X", StringComparison.OrdinalIgnoreCase) && (EmpBasic.CanReportNCQty && Session.ModuleLicensed(Erp.License.ErpLicensableModules.QualityAssurance) == true) && !ttLaborDtl.ReportPartQtyAllowed;

            }

            if (EmpBasic != null && EmpBasic.DcdUserID.KeyEquals(Session.UserID))
            {
                if (EmpBasic.DisallowTimeEntry == false)
                {
                    useEmployeeRules = true;
                    useApproverRules = olApprover;
                }
                else
                {
                    useEmployeeRules = false;
                    useApproverRules = false;
                    disallowTimeEntry = true;
                }
            }
            else if (EmpBasic != null && !EmpBasic.DcdUserID.KeyEquals(Session.UserID))
            {

                if (PlantConfCtrl == null || (PlantConfCtrl != null && !PlantConfCtrl.Plant.KeyEquals(Session.PlantID)))
                    PlantConfCtrl = this.FindFirstPlantConfCtrl(Session.CompanyID, Session.PlantID);
                if (PlantConfCtrl != null)
                {
                    if (PlantConfCtrl.TimeRestrictEntry == false)
                    {
                        if (olApprover == false)
                        {
                            useEmployeeRules = true;
                        }
                        else
                        {
                            useApproverRules = true;
                        }
                    }
                    else
                    {
                        supervisorHasRights = this.getSupervisorRights(ttLaborDtl.EmployeeNum);
                        if (supervisorHasRights || this.CanUserUpdateTime(Session.CompanyID, Session.UserID, true))
                        {
                            userCanUpdate = true;
                        }
                        else
                        {
                            userCanUpdate = false;
                        }

                        if (userCanUpdate == false && olApprover == false)
                        {
                            ttLaborDtl.HasAccessToRow = false;
                        }
                        else if (userCanUpdate == true && olApprover == false)
                        {
                            useEmployeeRules = true;
                        }
                        else if (userCanUpdate == false && olApprover == true)
                        {
                            useApproverRules = true;
                        }
                        else
                        {
                            useApproverRules = true;
                            useEmployeeRules = true;
                        }
                    }
                }
            }/* else if available EmpBasic and EmpBasic.DCDUserID <> DCD-UserID */

            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) && EmpBasic != null)
            {
                ttLaborDtl.AllowDirLbr = EmpBasic.AllowDirLbr;
            }
            setType = SetRuleTypeForLabor(useEmployeeRules, useApproverRules, disallowTimeEntry);
            if (lFromGetRowsCalendarView == false)
            {
                ttLaborDtl.HasAccessToRow = true;
                useEmployeeRules = true;
            }
            var outEnableCopy = ttLaborDtl.EnableCopy;
            var outEnableSubmit = ttLaborDtl.EnableSubmit;
            var outEnableRecall = ttLaborDtl.EnableRecall;
            var outTimeDisableUpdate = ttLaborDtl.TimeDisableUpdate;
            var outTimeDisableDelete = ttLaborDtl.TimeDisableDelete;
            this.setUpdateRules(setType, ttLaborDtl.JobNum, ttLaborDtl.NotSubmitted, ttLaborDtl.TimeStatus, ttLaborDtl.ApprovalRequired, ttLaborDtl.WipPosted, out outEnableCopy, out outEnableSubmit, out outEnableRecall, out outTimeDisableUpdate, out outTimeDisableDelete);
            ttLaborDtl.EnableCopy = outEnableCopy;
            ttLaborDtl.EnableSubmit = outEnableSubmit;
            ttLaborDtl.EnableRecall = outEnableRecall;
            ttLaborDtl.TimeDisableUpdate = outTimeDisableUpdate;
            ttLaborDtl.TimeDisableDelete = outTimeDisableDelete;

            ttLaborDtl.ISFixHourAndQtyOnly = false;
            if (!string.IsNullOrEmpty(ttLaborDtl.JobNum))
            {
                ttLaborDtl.ISFixHourAndQtyOnly = this.IsFixHoursAndQtyOnly(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
            }

            ResourceGroup = null;
            if (!String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID))
            {
                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, ttLaborDtl.ResourceGrpID);
            }

            if (ResourceGroup != null)
            {
                if (ResourceGroup.Plant.KeyEquals(Session.PlantID))
                {
                    ttLaborDtl.EnteredOnCurPlant = true;
                }
            }
            else
            {
                ttLaborDtl.EnteredOnCurPlant = true;
            }

            if (this.ExistsLaborDtlComment(Session.CompanyID, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq))
            {
                ttLaborDtl.HasComments = true;
            }

            ttLaborDtl.HasAttachments = ExistsXFileAttch(Session.CompanyID, "LaborDtl", Session.SystemCode, ttLaborDtl.LaborHedSeq.ToString(), ttLaborDtl.LaborDtlSeq.ToString());

            // Foreign_Link fields are already populated in GetRows from sql
            if (!CallContext.Properties.ContainsKey("LaborGetRows"))
            {
                LaborDtl_Foreign_Link();
            }

            ttLaborDtl.DisableRecallAndDelete = false;

            var laborHedRow = FindFirstLaborHed(Session.CompanyID, ttLaborDtl.LaborHedSeq);

            if (laborHedRow != null)
            {

                string payrollValuesForHCM = string.Empty;
                if (this.IsHCMEnabledAtCompany(Session.CompanyID, true))
                {
                    string HCMPayHoursCalcType = FindFirstLaborHedHCMPayHoursCalcType(laborHedRow.SysRowID);
                    payrollValuesForHCM = (string.IsNullOrEmpty(HCMPayHoursCalcType)) ? isHCMEnabledAt(laborHedRow.EmployeeNum) : HCMPayHoursCalcType;
                }
                else
                    payrollValuesForHCM = "NON";

                string hcmStatus = String.Empty;

                if (!payrollValuesForHCM.Equals("NON", StringComparison.OrdinalIgnoreCase))
                {
                    var hcmLaborDtlSyncRow = FindFirstHCMLaborDtlSync(laborHedRow.Company, laborHedRow.SysRowID);
                    if (hcmLaborDtlSyncRow != null)
                    {
                        hcmStatus = hcmLaborDtlSyncRow.Status;
                    }
                }

                if (ttLaborDtl.TimeStatus.Equals("A", StringComparison.OrdinalIgnoreCase) &&
                   ((payrollValuesForHCM.Equals("NON", StringComparison.OrdinalIgnoreCase) && laborHedRow.TransferredToPayroll == true)) ||
                    (!payrollValuesForHCM.Equals("NON", StringComparison.OrdinalIgnoreCase) && hcmStatus.Equals("S", StringComparison.OrdinalIgnoreCase)))
                {
                    ttLaborDtl.DisableRecallAndDelete = true;
                }
            }

            ttLaborDtl.RoleCdList = getRoleCdList(ttLaborDtl.EmployeeNum,
                                                  ttLaborDtl.ProjectID,
                                                  ttLaborDtl.PhaseID,
                                                  ttLaborDtl.JobNum,
                                                  ttLaborDtl.AssemblySeq,
                                                  ttLaborDtl.OprSeq);

            ttLaborDtl.RowSelected = false;

            string templateID = String.Empty;
            if (JobOper != null && JobOper.Company.KeyEquals(Session.CompanyID) && JobOper.JobNum.KeyEquals(ttLaborDtl.JobNum) && JobOper.AssemblySeq == ttLaborDtl.AssemblySeq && JobOper.OprSeq == ttLaborDtl.OprSeq)
                templateID = JobOper.TemplateID;
            else
                templateID = this.FindJobOperTemplateId(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
            if (templateID != null)
            {
                ttLaborDtl.TemplateID = templateID;
            }

            updateFieldsForCalendarView(ref ttLaborDtl);
        }

        /// <summary>
        /// Calls LaborDtlAfterGetRows for the passed in LaborDtl row
        /// </summary>
        /// <param name="laborDtlRow">LaborDtl tableset row</param>
        public void LaborDtlAfterGetRowsWrapper(ref LaborDtlRow laborDtlRow)
        {
            ttLaborDtl = laborDtlRow;
            LaborDtlAfterGetRows();
        }

        private static int SetRuleTypeForLabor(bool useEmployeeRules, bool useApproverRules, bool disallowTimeEntry)
        {
            int setType;
            if (disallowTimeEntry == true)
            {
                setType = 4;
            }
            else
            {
                if (useEmployeeRules == true && useApproverRules == false)
                {
                    setType = 1;
                }
                else if (useEmployeeRules == false && useApproverRules == true)
                {
                    setType = 2;
                }
                else if (useEmployeeRules == true && useApproverRules == true)
                {
                    setType = 3;
                }
                else
                {
                    setType = 2;
                }
            }

            return setType;
        }

        partial void LaborDtlAfterUpdate()
        {
            decimal oldBurdenRate = decimal.Zero;
            decimal NegActBurCost = decimal.Zero;
            decimal NegReworkBurCost = decimal.Zero;
            if (ttLaborDtl.Downtime != true)
            {

                if (lRuncreateMtlqPWip == true && this.ReportPartQtyAllowed(LaborDtl.JobNum, LaborDtl.AssemblySeq, LaborDtl.OprSeq) == false)
                {
                    this.createMtlqPwip();
                    /* ATR (12/05/07) - SCR #47774 */
                    this.createNonConfMtlQ();
                }

                this.updateTotHours(ttLaborDtl.LaborHedSeq);
                if (ttLaborDtl.RowMod.Compare(IceRow.ROWSTATE_UPDATED) == 0)
                {
                    if (BIttLaborDtl != null && !((BIttLaborDtl.JobNum.Compare(ttLaborDtl.JobNum) == 0)
                        && BIttLaborDtl.AssemblySeq == ttLaborDtl.AssemblySeq
                        && BIttLaborDtl.OprSeq == ttLaborDtl.OprSeq))
                    {


                        foreach (var LaborPart_iterator in (this.SelectLaborPartWithUpdLock(BIttLaborDtl.Company, BIttLaborDtl.LaborHedSeq, BIttLaborDtl.LaborDtlSeq)))
                        {
                            LaborPart = LaborPart_iterator;
                            ttLaborPart = new Erp.Tablesets.LaborPartRow();
                            CurrentFullTableset.LaborPart.Add(ttLaborPart);
                            BufferCopy.Copy(LaborPart, ref ttLaborPart);
                            ttLaborPart.RowMod = IceRow.ROWSTATE_DELETED;
                            ttLaborPart.SysRowID = LaborPart.SysRowID;

                            /*Find the UOM for the Part defined in the Operation*/


                            JobOper = this.FindFirstJobOper21(Session.CompanyID, BIttLaborDtl.JobNum, BIttLaborDtl.AssemblySeq, BIttLaborDtl.OprSeq);
                            if (JobOper != null)
                            {
                                ttLaborPart.PartUOM = JobOper.IUM;
                            }
                            /* NOW DELETE THE DATABASE RECORD */
                            Db.LaborPart.Delete(LaborPart);
                        }
                    }
                }
                if (deniedColumns == null) deniedColumns = Ice.Manager.Security.GetWriteDeniedColumns(Session.CompanyID, Session.UserID, "Erp", "LaborDtl");
                vBurdenRateSec = (deniedColumns.Contains("BurdenRate", StringComparer.OrdinalIgnoreCase));
                if (vBurdenRateSec)
                {
                    Db.DisableTriggers(Erp.Tables.LaborDtl.GetTableName(), TriggerType.Write);
                    this.getLaborDtlBurdenRates(true, out vBurdenRate);
                    if (LaborDtl != null)
                    {
                        oldBurdenRate = LaborDtl.BurdenRate;
                        LaborDtl.BurdenRate = vBurdenRate;
                    }
                    Db.Validate(LaborDtl);
                    BufferCopy.Copy(LaborDtl, ref ttLaborDtl);
                    Db.EnableTriggers(Erp.Tables.LaborDtl.GetTableName(), TriggerType.Write);



                    JobOper = this.FindFirstJobOperWithUpdLock(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                    if (JobOper != null)
                    {
                        JobOper.ActBurCost = JobOper.ActBurCost - LibRoundAmountEF.RoundDecimalsApply(LaborDtl.BurdenHrs * oldBurdenRate, "", "JobOper", "ActBurCost");
                        if (JobOper.ActBurCost < 0)
                        {
                            NegActBurCost = JobOper.ActBurCost;
                            JobOper.ActBurCost = 0;
                        }
                        /* Remove the old LaborDtl Rework Burden Cost from the JobOper Actual Rework Burden Costs */
                        if (LaborDtl.ReWork == true)
                        {
                            JobOper.ReworkBurCost = JobOper.ReworkBurCost - LibRoundAmountEF.RoundDecimalsApply(LaborDtl.BurdenHrs * oldBurdenRate, "", "JobOper", "ReworkBurCost");
                            if (JobOper.ReworkBurCost < 0)
                            {
                                NegReworkBurCost = JobOper.ReworkBurCost;
                                JobOper.ReworkBurCost = 0;
                            }
                        }
                        /* Add LaborDtl Burden Cost to the JobOper Actual Burden Costs */
                        JobOper.ActBurCost = JobOper.ActBurCost + LibRoundAmountEF.RoundDecimalsApply(LaborDtl.BurdenHrs * LaborDtl.BurdenRate, "", "JobOper", "ActBurCost") + NegActBurCost;

                        /* Add LaborDtl Rework Burden Cost to the JobOper Actual Rework Burden Costs */
                        if (LaborDtl.ReWork == true)
                        {
                            JobOper.ReworkBurCost = JobOper.ReworkBurCost + LibRoundAmountEF.RoundDecimalsApply(LaborDtl.BurdenHrs * LaborDtl.BurdenRate, "", "JobOper", "ReworkBurCost") + NegReworkBurCost;
                        }
                    }
                }
                if (ttLaborDtl.EnableSN)
                {
                    updateSerialNumbers(ttLaborDtl.PCID, ttLaborDtl.LotNum);
                }
                /*SCR 61012*/
                if (ttLaborDtl.RowMod.Equals(IceRow.ROWSTATE_ADDED, StringComparison.OrdinalIgnoreCase) || (ttLaborDtl.SentFromMES && ttLaborDtl.RowMod.Equals(IceRow.ROWSTATE_UPDATED, StringComparison.OrdinalIgnoreCase)))
                {


                    JobHead = this.FindFirstJobHead12(Session.CompanyID, ttLaborDtl.JobNum);
                    if (JobHead != null && JobHead.JobType.Compare("MNT") != 0)
                    {


                        foreach (var JobOpDtl_iterator in (this.SelectJobOpDtl2(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq)))
                        {
                            JobOpDtl = JobOpDtl_iterator;
                            if (!String.IsNullOrEmpty(JobOpDtl.ResourceID) && JobOpDtl.ResourceID.Compare(ttLaborDtl.ResourceID) != 0)
                            {


                                foreach (var Equip_iterator in (this.SelectEquip(Session.CompanyID, JobOpDtl.ResourceID, "No", false)))
                                {
                                    Equip = Equip_iterator;


                                    LaborEquip = this.FindFirstLaborEquip(Session.CompanyID, LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq, Equip.EquipID);
                                    if (LaborEquip == null)
                                    {
                                        LaborEquip = new Erp.Tables.LaborEquip();
                                        Db.LaborEquip.Insert(LaborEquip);
                                        LaborEquip.Company = Session.CompanyID;
                                        LaborEquip.LaborHedSeq = LaborDtl.LaborHedSeq;
                                        LaborEquip.LaborDtlSeq = LaborDtl.LaborDtlSeq;
                                        LaborEquip.EquipID = Equip.EquipID;
                                        if (Equip.LaborMeterOpt.Compare("Hrs") == 0)
                                        {
                                            LaborEquip.Hours = LaborDtl.BurdenHrs;
                                        }

                                        if (Equip.LaborMeterOpt.Compare("Qty") == 0)
                                        {
                                            LaborEquip.Qty = LaborDtl.LaborQty;
                                        }
                                        Db.Validate(LaborEquip);

                                        ttLaborEquip = new Erp.Tablesets.LaborEquipRow();
                                        CurrentFullTableset.LaborEquip.Add(ttLaborEquip);
                                        ttLaborEquip.SysRowID = LaborEquip.SysRowID;
                                        BufferCopy.Copy(LaborEquip, ref ttLaborEquip);
                                    }/*if not available LaborEquip*/
                                }
                            }/*if JobOpDtl.ResourceID <> ""*/
                        }/*for each JobOptDtl */
                        if (!String.IsNullOrEmpty(ttLaborDtl.ResourceID))
                        {

                            foreach (var Equip_iterator in (this.SelectEquip2(Session.CompanyID, ttLaborDtl.ResourceID, "No", false)))
                            {
                                Equip = Equip_iterator;


                                LaborEquip = this.FindFirstLaborEquip(Session.CompanyID, LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq, Equip.EquipID);
                                //tt-record to be synchronized with db-record above
                                ttLaborEquip = (from LaborEquip_Row in CurrentFullTableset.LaborEquip
                                                where LaborEquip_Row.Company.KeyEquals(Session.CompanyID) &&
                                                      LaborEquip_Row.LaborHedSeq == LaborDtl.LaborHedSeq &&
                                                      LaborEquip_Row.LaborDtlSeq == LaborDtl.LaborDtlSeq &&
                                                      LaborEquip_Row.EquipID.KeyEquals(Equip.EquipID) &&
                                                      LaborEquip_Row.RowMod != ""
                                                select LaborEquip_Row).FirstOrDefault();

                                if (LaborEquip == null)
                                {
                                    LaborEquip = new Erp.Tables.LaborEquip();
                                    Db.LaborEquip.Insert(LaborEquip);
                                    LaborEquip.Company = Session.CompanyID;
                                    LaborEquip.LaborHedSeq = LaborDtl.LaborHedSeq;
                                    LaborEquip.LaborDtlSeq = LaborDtl.LaborDtlSeq;
                                    LaborEquip.EquipID = Equip.EquipID;
                                    if (Equip.LaborMeterOpt.Compare("Hrs") == 0)
                                    {
                                        LaborEquip.Hours = LaborDtl.BurdenHrs;
                                    }

                                    if (Equip.LaborMeterOpt.Compare("Qty") == 0)
                                    {
                                        LaborEquip.Qty = LaborDtl.LaborQty;
                                    }
                                    Db.Validate(LaborEquip);

                                    if (ttLaborEquip == null)
                                    {
                                        ttLaborEquip = new Erp.Tablesets.LaborEquipRow();
                                        CurrentFullTableset.LaborEquip.Add(ttLaborEquip);
                                    }
                                    ttLaborEquip.SysRowID = LaborEquip.SysRowID;
                                    BufferCopy.Copy(LaborEquip, ref ttLaborEquip);
                                }
                                else if (ttLaborDtl.SentFromMES && LaborEquip != null)
                                {
                                    if (Equip.LaborMeterOpt.Equals("Hrs", StringComparison.OrdinalIgnoreCase))
                                    {
                                        LaborEquip.Hours = LaborDtl.BurdenHrs;
                                    }

                                    if (Equip.LaborMeterOpt.Equals("Qty", StringComparison.OrdinalIgnoreCase))
                                    {
                                        LaborEquip.Qty = LaborDtl.LaborQty;
                                    }
                                    Db.Validate(LaborEquip);

                                    if (ttLaborEquip == null)
                                    {
                                        ttLaborEquip = new Erp.Tablesets.LaborEquipRow();
                                        CurrentFullTableset.LaborEquip.Add(ttLaborEquip);
                                    }
                                    ttLaborEquip.SysRowID = LaborEquip.SysRowID;
                                    BufferCopy.Copy(LaborEquip, ref ttLaborEquip);
                                }
                            }
                        }
                    }
                }/* if ttLaborDtl.RowMod = {&RowState_Added} */
                if (!String.IsNullOrEmpty(ttLaborDtl.ClaimRef))
                {


                    LaborDtlGroup = this.FindFirstLaborDtlGroup(Session.CompanyID, LaborDtl.EmployeeNum, LaborDtl.ClaimRef);
                    if (LaborDtlGroup == null)
                    {
                        LaborDtlGroup = new Erp.Tables.LaborDtlGroup();
                        Db.LaborDtlGroup.Insert(LaborDtlGroup);
                        LaborDtlGroup.Company = Session.CompanyID;
                        LaborDtlGroup.EmployeeNum = LaborDtl.EmployeeNum;
                        LaborDtlGroup.ClaimRef = LaborDtl.ClaimRef;
                    }
                }
            } //For not ExternalMES records    

            //If ending an activity and timestatus is blank or e, run the submit process
            if (ttLaborDtl.EndActivity == true &&
                (String.IsNullOrEmpty(ttLaborDtl.TimeStatus) ||
                 ttLaborDtl.TimeStatus.Equals("E", StringComparison.OrdinalIgnoreCase)) &&
                 !(ttLaborDtl.Downtime && !ttLaborDtl.LaborType.Equals("I", StringComparison.OrdinalIgnoreCase)))
            {
                string cmessagetext = string.Empty;
                ttTEKey = new TimeExpense.ttTEKey();
                ttTEKey.Key1 = Compatibility.Convert.ToString(LaborDtl.LaborHedSeq);
                ttTEKey.Key2 = Compatibility.Convert.ToString(LaborDtl.LaborDtlSeq);
                if (ttTEKeyRows == null)
                {
                    ttTEKeyRows = new List<TimeExpense.ttTEKey>();
                }
                ttTEKeyRows.Add(ttTEKey);
                LibTimeExpenseSubmit._TimeExpenseSubmit(ref ttTEKeyRows, "Time", out cmessagetext);
                ttTEKeyRows.Clear();

                string foreignKey1 = LaborDtl.Company + LaborDtl.LaborHedSeq + LaborDtl.LaborDtlSeq;
                decimal qtyRepToMtlQBefEndAct = LibUsePatchFld.GetPatchFldDecimal(Session.CompanyID, "LaborDtl", "QtyRepToMtlQBefEndAct", foreignKey1);
                LibUsePatchFld.DeletePatchFld("LaborDtl", "QtyRepToMtlQBefEndAct", foreignKey1);
                LibUsePatchFld.DeletePatchFld("LaborDtl", "MtlQueueSeq", foreignKey1);

                /*SCR 80675 - Flag is turned on(2) when comes from submit time(UI- TimeExpenseEntry), turns off(0) after SubmitFor Approval*/
                if (ttLaborDtl.NewDifDateFlag == 1)
                {
                    ttLaborDtl.NewDifDateFlag = 2;    /*END SCR 80675*/
                }

                if (DelFlag == true)
                {
                    this.LaborDtlAfterDelete();
                }
            }

            if (Internal.Lib.ErpCallContext.ContainsKey("IMLaborHed") && ttLaborDtl.TimeAutoSubmit && ttLaborDtl.TimeStatus.KeyEquals("E"))
            {
                string cmessagetext = string.Empty;
                ttTEKey = new TimeExpense.ttTEKey();
                ttTEKey.Key1 = Compatibility.Convert.ToString(LaborDtl.LaborHedSeq);
                ttTEKey.Key2 = Compatibility.Convert.ToString(LaborDtl.LaborDtlSeq);
                if (ttTEKeyRows == null)
                {
                    ttTEKeyRows = new List<TimeExpense.ttTEKey>();
                }
                ttTEKeyRows.Add(ttTEKey);
                LibTimeExpenseSubmit._TimeExpenseSubmit(ref ttTEKeyRows, "Time", out cmessagetext);
                ttTEKeyRows.Clear();
            }

            writeFSAExtFlds(ref ttLaborDtl);

            //ERP-335 - this is intentional to use PatchFld - very short lived record
            //DO NOT remove this PatchFLd

            if (!string.IsNullOrEmpty(ttLaborDtl.IntExternalKey))
            {
                string patchKey = Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                LibUsePatchFld.SetPatchFldChar(Session.CompanyID, "LaborDtl", "IntExternalKey", patchKey, ttLaborDtl.IntExternalKey);
            }

            ttLaborDtl.SysRevID = (long)LaborDtl.SysRevNum;

            if (ttLaborDtl.RowMod.Equals(IceRow.ROWSTATE_ADDED))
            {
                foreach (var laborDtlAction in CurrentFullTableset.LaborDtlAction)
                {
                    laborDtlAction.LaborDtlSeq = ttLaborDtl.LaborDtlSeq;
                    laborDtlAction.LaborHedSeq = ttLaborDtl.LaborHedSeq;
                }
            }
        }

        /// <summary>
        /// This method is to swap the before image endtimes back to 24.0 from 0.
        /// </summary>
        partial void LaborDtlBeforeBI()
        {
            Erp.Tables.LaborDtl altLaborDtl = null;

            altLaborDtl = this.FindFirstLaborDtl4(ttLaborDtl.SysRowID);
            /* set the labordtl clockouttime back to 24 from 0 */
            if (BIttLaborDtl != null)
            {
                if (altLaborDtl != null)
                {
                    BIttLaborDtl.ClockOutTime = altLaborDtl.ClockOutTime;
                }
            }
            if (ttLaborDtl.StartActivity == false)
            {
                if (ttLaborDtl.ClockOutTime == 0)
                {
                    ttLaborDtl.ClockOutTime = 24.0m;
                }
            }
        }

        partial void LaborDtlBeforeCreate()
        {
            LaborHed = this.FindFirstLaborHed6(Session.CompanyID, ttLaborDtl.LaborHedSeq);
            if (LaborHed != null)
            {
                ttLaborDtl.Shift = LaborHed.Shift;
            }
            if (ttLaborDtl.LaborDtlSeq == 0)
            {
                ttLaborDtl.LaborDtlSeq = LibNextValue.GetNextSequence("LaborDtlSeq");
            }

            foreach (var ttLbrScrapSerialNumbers_iterator in (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                                              where ttLbrScrapSerialNumbers_Row.Company.KeyEquals(Session.CompanyID)
                                                              && ttLbrScrapSerialNumbers_Row.LaborDtlSeq == 0
                                                              select ttLbrScrapSerialNumbers_Row))
            {
                ttLbrScrapSerialNumbers = ttLbrScrapSerialNumbers_iterator;
                ttLbrScrapSerialNumbers.LaborDtlSeq = ttLaborDtl.LaborDtlSeq;
            }
            ttLaborDtl.CreateDate = CompanyTime.Today();
            ttLaborDtl.CreateTime = CompanyTime.Now().SecondsSinceMidnight();
            ttLaborDtl.CreatedBy = Session.UserID;
            this.LaborDtlBeforeBI();
        }

        partial void LaborDtlBeforeDelete()
        {
            if (PELock.IsDocumentLock(Session.CompanyID, "LaborDtl", Convert.ToString(ttLaborDtl.LaborHedSeq), Convert.ToString(ttLaborDtl.LaborDtlSeq), "", "", "", ""))
            {
                throw new BLException(PELock.LockMessage);
            }

            if (ttLaborDtl.WipPosted == true)
            {
                throw new BLException(Strings.InvalidToDeleteAPostedLabor, "LaborDtl");
            }

            if (ttLaborDtl.ActiveTrans == true)
            {
                throw new BLException(Strings.InvalidToDeleteAnActiveTrans, "LaborDtl");
            }

            if (ttLaborDtl.TimeStatus.Compare("A") == 0)
            {
                throw new BLException(Strings.InvalidToDeleteAnApprovedTrans, "LaborDtl");
            }

            if (ttLaborDtl.LaborEntryMethod.KeyEquals("B"))
            {
                throw new BLException(Strings.InvalidToDeleteBackFlushedTrans);
            }

            this.updateTotHours(ttLaborDtl.LaborHedSeq);
            if (ttLaborDtl.EnableSN)
            {
                if (!ttLaborDtl.ReWork &&
                    (this.ExistsSerialNoRw(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.OprSeq, "OPR-RWK") || this.ExistsSNTranRw(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.OprSeq, "OPR-RWK")))
                {
                    throw new BLException(Strings.InvalidToDeleteRWExists, "LaborDtl");
                }
                SNVerify = true;
                this.deleteSerialNumbers();
            }

            lcRefreshEmployeeNum = ttLaborDtl.EmployeeNum;
            ldFromDate = ((BIttLaborDtl != null) ? BIttLaborDtl.PayrollDate : ttLaborDtl.PayrollDate);
            ldToDate = ttLaborDtl.PayrollDate;
            if (ldFromDate.Value.Date > ldToDate.Value.Date)
            {
                ldRefreshToDate = ((ldRefreshToDate == null || ldFromDate > ldRefreshToDate) ? ldFromDate : ldRefreshToDate);
                ldRefreshFromDate = ((ldRefreshFromDate == null || ldToDate < ldRefreshFromDate) ? ldToDate : ldRefreshFromDate);
            }
            else
            {
                ldRefreshToDate = ((ldRefreshToDate == null || ldToDate > ldRefreshToDate) ? ldToDate : ldRefreshToDate);
                ldRefreshFromDate = ((ldRefreshFromDate == null || ldFromDate < ldRefreshFromDate) ? ldFromDate : ldRefreshFromDate);
            }
            /* when LaborDtl is being deleted, need to force a refresh to the WeeklyView record that pertains to this LaborDtl  */
            /* in case one is being left on the UI as a result of the last hours for any day during the week being removed. DJY */
            DelFlag = true;
            DelCompany = ttLaborDtl.Company;
            DelEmployeeNum = ttLaborDtl.EmployeeNum;
            DelWeekBeginDate = ttLaborDtl.PayrollDate.Value.AddDays(1 - Convert.ToInt32(ttLaborDtl.PayrollDate.Value.DayOfWeek + 1));
            DelWeekEndDate = ttLaborDtl.PayrollDate.Value.AddDays(7 - Convert.ToInt32(ttLaborDtl.PayrollDate.Value.DayOfWeek + 1));
            DelLaborType = ttLaborDtl.LaborType;
            DelLaborTypePseudo = ttLaborDtl.LaborTypePseudo;
            DelProjectID = ttLaborDtl.ProjectID;
            DelPhaseID = ttLaborDtl.PhaseID;
            DelTimeTypCd = ttLaborDtl.TimeTypCd;
            DelJobNum = ttLaborDtl.JobNum;
            DelAssemblySeq = ttLaborDtl.AssemblySeq;
            DelOprSeq = ttLaborDtl.OprSeq;
            DelIndirectCode = ttLaborDtl.IndirectCode;
            DelRoleCd = ttLaborDtl.RoleCd;
            DelResourceGrpID = ttLaborDtl.ResourceGrpID;
            DelResourceID = ttLaborDtl.ResourceID;
            DelExpenseCode = ttLaborDtl.ExpenseCode;
            DelShift = ttLaborDtl.Shift;
            DelStatus = ttLaborDtl.TimeStatus;
            DelQuickEntryCode = ttLaborDtl.QuickEntryCode;
        }

        partial void LaborDtlBeforeUpdate()
        {
            string clockDelim = string.Empty;
            string vMessage = string.Empty;
            decimal tothours = decimal.Zero;
            Erp.Tables.JobOper bJobOper = null;
            Erp.Tables.LaborDtl altLaborDtl = null;
            string foreignKey = string.Empty;

            if (deniedColumns == null) deniedColumns = Ice.Manager.Security.GetWriteDeniedColumns(Session.CompanyID, Session.UserID, "Erp", "LaborDtl");

            if (deniedColumns.Contains(LaborDtl.ColumnNames.LaborRate, StringComparer.OrdinalIgnoreCase) && (BIttLaborDtl == null ||
                !ttLaborDtl.JobNum.Equals(BIttLaborDtl.JobNum, StringComparison.OrdinalIgnoreCase) ||
                ttLaborDtl.AssemblySeq != BIttLaborDtl.AssemblySeq || ttLaborDtl.OprSeq != BIttLaborDtl.OprSeq))
            {
                ttLaborDtl.LaborRate = this.LibLaborRate.LaborRateCalc(ttLaborDtl);
            }

            ttLaborDtl.LaborType = (("P,J,V".Lookup(ttLaborDtl.LaborTypePseudo) > -1) ? "P" : ((ttLaborDtl.LaborTypePseudo.Compare("I") == 0) ? "I" : ((ttLaborDtl.LaborTypePseudo.Compare("S") == 0) ? "S" : "")));
            /* can't update record if transferred to payroll */

            if (PELock.IsDocumentLock(Session.CompanyID, "LaborDtl", Convert.ToString(LaborDtl.LaborHedSeq), Convert.ToString(LaborDtl.LaborDtlSeq), "", "", "", ""))
            {
                throw new BLException(PELock.LockMessage);
            }

            if (ttLaborDtl.EnableLaborQty == true && ttLaborDtl.EnableScrapQty == false && ttLaborDtl.ScrapQty > 0)
            {
                throw new BLException(Strings.EmployeeUnableToUpdateScrapQty);
            }

            if (ttLaborDtl.EnableLaborQty == true && ttLaborDtl.EnableDiscrepQty == false && ttLaborDtl.DiscrepQty > 0)
            {
                throw new BLException(Strings.EmployeeUnableToUpdateDiscrepQty);
            }


            if (ttLaborDtl.Downtime != true)
            {
                LaborHed = this.FindFirstLaborHed6(Session.CompanyID, ttLaborDtl.LaborHedSeq);
                if (LaborHed != null && LaborHed.TransferredToPayroll)
                {
                    throw new BLException(Strings.CanTUpdateRecordAfterInforHasBeenTransToPayroll, "LaborDtl");
                }
                /* INVALID TO MODIFY A RECORD WHICH IS CURRENTLY ACTIVE IN MES
                (employee is clocked in but not yet clocked out on this transaction ) */
                if (ttLaborDtl.RowMod.Compare(IceRow.ROWSTATE_UPDATED) == 0)
                {
                    if (ttLaborDtl.ActiveTrans == true && ttLaborDtl.EndActivity == false)
                    {
                        throw new BLException(Strings.InvalidToModifyAnActiveTrans, "LaborDtl");
                    }
                }


                EmpBasic = this.FindFirstEmpBasic11(Session.CompanyID, ttLaborDtl.EmployeeNum);
                if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) && EmpBasic != null && EmpBasic.AllowDirLbr == false &&
                   ("I,J".Lookup(ttLaborDtl.LaborTypePseudo) == -1))
                {
                    throw new BLException(Strings.InvalidLaborTypeWhenAllowToBookToDirectJobsFlag, "LaborDtl", ttLaborDtl.LaborType);
                } /* if lookup(",E,R":U,ttLaborDtl.TimeStatus) then do:*/
                if (ttLaborDtl.TimeDisableUpdate == true && (",E,R".Lookup(ttLaborDtl.TimeStatus) == -1))
                {
                    throw new BLException(Strings.InvalidToModifyATransThatHasBeenSubmiForAppro, "LaborDtl", ttLaborDtl.TimeStatus);
                }
                if (!String.IsNullOrEmpty(ttLaborDtl.RoleCd) && (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) ||
                    ttLaborDtl.LaborType.Compare("I") == 0))
                {
                    throw new BLException(Strings.RoleCodeCannotBeEntered, "LaborDtl", "RoleCd");
                }
                this.clearRecordBuffer();
                this.validateJob(ttLaborDtl.JobNum, false, false);
                this.validateJobOper(ttLaborDtl.LaborType, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                this.chkPartQty();
                this.checkIfNonConfProcessed(ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);

                if (ttLaborDtl.EnableSN)
                {
                    this.validateSerial();
                    string snErr = string.Empty;
                    decimal totQtyReq = decimal.Zero;
                    decimal totNewQtyReq = decimal.Zero;
                    int chkAttrSetID = 0;
                    var PartPartial = FindFirstPartPartial(Session.CompanyID, ttLaborDtl.PartNum);
                    if (PartPartial != null && PartPartial.TrackInventoryAttributes)
                    {
                        if (ttLaborDtl.LaborQty > 0 && ttLaborDtl.LaborAttributeSetID == 0 ||
                            ttLaborDtl.ScrapQty > 0 && ttLaborDtl.ScrapAttributeSetID == 0 ||
                            ttLaborDtl.DiscrepQty > 0 && ttLaborDtl.DiscrepAttributeSetID == 0)
                        {
                            throw new BLException(Strings.AttributeSetsMustBeEnteredForAllQtys);
                        }
                        if (ttLaborDtl.LaborQty > 0)
                        {
                            chkAttrSetID = ttLaborDtl.LaborAttributeSetID;
                        }
                        if (ttLaborDtl.ScrapQty > 0)
                        {
                            if (chkAttrSetID == 0)
                                chkAttrSetID = ttLaborDtl.ScrapAttributeSetID;
                            else if (chkAttrSetID != ttLaborDtl.ScrapAttributeSetID)
                            {
                                throw new BLException(Strings.AllAttrSetsMustBeTheSame);
                            }
                        }
                        if (ttLaborDtl.DiscrepQty > 0)
                        {
                            if (chkAttrSetID == 0)
                                chkAttrSetID = ttLaborDtl.DiscrepAttributeSetID;
                            else if (chkAttrSetID != ttLaborDtl.DiscrepAttributeSetID)
                            {
                                throw new BLException(Strings.AllAttrSetsMustBeTheSame);
                            }
                        }

                    }
                    if (PartPartial != null && !PartPartial.TrackInventoryAttributes && PartPartial.TrackInventoryByRevision)
                    {
                        chkAttrSetID = ttLaborDtl.LaborAttributeSetID;
                    }
                    this.validateSerialAvail(chkAttrSetID, out snErr, out totQtyReq, out totNewQtyReq);
                    if (!String.IsNullOrEmpty(snErr))
                    {
                        throw new BLException(Strings.NotEnoughSerialNumbersAssigned, "SerialNo");
                    }
                }


                if (ttLaborDtl.ActiveTrans && (this.ExistsLaborDtl1(Session.CompanyID, ttLaborDtl.LaborHedSeq, ttLaborDtl.EmployeeNum, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.ResourceID, ttLaborDtl.LaborDtlSeq, true)))
                {
                    throw new BLException(Strings.EmploIsAlreadyActiveOnThisSeque);
                }

                if (ttLaborDtl.ScrapQty > 0 && !ttLaborDtl.ReportPartQtyAllowed)
                {
                    if (ttJCSyst.ScrapReasons == true)
                    {
                        if (!this.ExistsScrapCode(Session.CompanyID, ttLaborDtl.ScrapReasonCode))
                        {
                            throw new BLException(Strings.InvalidScrapReasonCode);
                        }
                    }
                    using (var libAdvancedUOMValidations = new Erp.Internal.Lib.AdvancedUOMValidations(Db))
                    {
                        libAdvancedUOMValidations.CheckAttributeSetIsValidForPart(ttLaborDtl.ScrapAttributeSetID, true, ttLaborDtl.PartNum);
                    }
                }


                if (ttLaborDtl.DiscrepQty > 0 && !ttLaborDtl.ReportPartQtyAllowed)
                {
                    using (var libAdvancedUOMValidations = new Erp.Internal.Lib.AdvancedUOMValidations(Db))
                    {
                        libAdvancedUOMValidations.CheckAttributeSetIsValidForPart(ttLaborDtl.DiscrepAttributeSetID, true, ttLaborDtl.PartNum);
                    }

                }

                if (ttLaborDtl.LaborQty > 0 && !ttLaborDtl.ReportPartQtyAllowed)
                {
                    using (var libAdvancedUOMValidations = new Erp.Internal.Lib.AdvancedUOMValidations(Db))
                    {
                        libAdvancedUOMValidations.CheckAttributeSetIsValidForPart(ttLaborDtl.LaborAttributeSetID, true, ttLaborDtl.PartNum);
                    }

                }


                if (LaborDtl.WipPosted == true)
                {
                    throw new BLException(Strings.InvalidToModifyAPostedLabor, "LaborDtl");
                }

                if (ttLaborDtl.LaborHrs != 0)
                {


                    LaborDtlParams ipLaborDtlParams = new LaborDtlParams();
                    ipLaborDtlParams.PayrollDate = ttLaborDtl.PayrollDate.Value.Date;
                    ipLaborDtlParams.JobNum = ttLaborDtl.JobNum;
                    ipLaborDtlParams.AssemblySeq = ttLaborDtl.AssemblySeq;
                    ipLaborDtlParams.OprSeq = ttLaborDtl.OprSeq;
                    ipLaborDtlParams.EmployeeNum = ttLaborDtl.EmployeeNum;
                    ipLaborDtlParams.LaborType = ttLaborDtl.LaborType;
                    ipLaborDtlParams.ProjectID = ttLaborDtl.ProjectID;
                    ipLaborDtlParams.PhaseID = ttLaborDtl.PhaseID;
                    ipLaborDtlParams.IndirectCode = ttLaborDtl.IndirectCode;
                    ipLaborDtlParams.RoleCd = ttLaborDtl.RoleCd;
                    ipLaborDtlParams.TimeTypCd = ttLaborDtl.TimeTypCd;
                    ipLaborDtlParams.ResourceGrpID = ttLaborDtl.ResourceGrpID;
                    ipLaborDtlParams.ResourceID = ttLaborDtl.ResourceID;
                    ipLaborDtlParams.ExpenseCode = ttLaborDtl.ExpenseCode;
                    ipLaborDtlParams.Shift = ttLaborDtl.Shift;
                    ipLaborDtlParams.LaborDtlSeq = ttLaborDtl.LaborDtlSeq;

                    if ((this.ExistsLaborDtl2(Session.CompanyID, ipLaborDtlParams)))
                    {
                        foreach (var altLaborDtl_iterator in (this.SelectLaborDtl(Session.CompanyID, ipLaborDtlParams)))
                        {
                            altLaborDtl = altLaborDtl_iterator;
                            tothours = tothours + altLaborDtl.LaborHrs;
                        }
                    }
                    if (tothours + ttLaborDtl.LaborHrs < 0)
                    {
                        throw new BLException(Strings.TotalLaborHoursCannotBeNegat);
                    }
                }
                if (String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID) && ttLaborDtl.StartActivity == true)
                {
                    throw new BLException(Strings.ResourceGroupIsMandatory, "LaborDtl", "ResourceGrpID", ttLaborDtl.SysRowID);
                }
                clockDelim = ((ttJCSyst.ClockFormat.Compare("M") == 0) ? ":" : ".");

                if (ttLaborDtl.ResourceID == string.Empty && ttLaborDtl.ResourceGrpID != string.Empty && ttLaborDtl.JCDept == string.Empty)
                {
                    ttLaborDtl.JCDept = ((ResourceGroup != null) ? ResourceGroup.JCDept : "");
                }
                /* indirect */
                /* labortype <> "I" */
                if (ttLaborDtl.LaborType.Compare("I") != 0)
                {
                    this.validateJobFields();
                    this.validateOpCode(ttLaborDtl.OpCode);
                    this.validateJCDept(ttLaborDtl.JCDept);
                    this.validateProject();
                    /* check for negative hours */
                    this.chkNegHrs();        /* check labor qty */
                    this.chkLaborQty(ttLaborDtl.LaborQty, out vMessage);
                }
                else
                {
                    this.validateIndirect(ttLaborDtl.IndirectCode, true);
                }
                /* setup percent */
                if (ttLaborDtl.SetupPctComplete > 100)
                {
                    throw new BLException(Strings.SetupPercentComplCanTBeGreaterThan);
                }

                /* expense code */
                if (String.IsNullOrEmpty(ttLaborDtl.ExpenseCode))
                {
                    throw new BLException(Strings.ExpenseCodeIsMandatory);
                }



                if (!((this.ExistsLabExpCd(Session.CompanyID, ttLaborDtl.ExpenseCode))))
                {
                    throw new BLException(Strings.InvalidLaborExpenseCode);
                }
                if (!String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID))
                {
                    this.validateWcCode(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.ResourceGrpID, "LaborDtl", ttLaborDtl.SysRowID);
                    if (ResourceGroup != null)
                    {
                        if (ResourceGroup.Inactive == true)
                        {
                            throw new BLException(Strings.TheResouGroupIsInactAndMayNotBeSelec);
                        }
                    }
                }

                if ((ttLaborDtl.MES || ttLaborDtl.HH) && ttLaborDtl.RowMod.KeyEquals(IceRow.ROWSTATE_ADDED))
                {
                    LaborTableset ds = this.CurrentFullTableset;
                    this.SetClockInAndDisplayTimeMES(ref ds);
                    this.CurrentFullTableset = ds;
                }

                /* clock in */
                this.validHrMin(ttLaborDtl.ClockinTime);
                if (ttJCSyst.ClockFormat.Compare("M") == 0)
                {
                    ttLaborDtl.DspClockInTime = this.convMin(Compatibility.Convert.ToString(ttLaborDtl.ClockinTime));
                }
                else
                {
                    ttLaborDtl.DspClockInTime = this.convDec(Compatibility.Convert.ToString(ttLaborDtl.ClockinTime));
                }
                /* clock out */
                this.validHrMin(ttLaborDtl.ClockOutTime);
                if (ttJCSyst.ClockFormat.Compare("M") == 0)
                {
                    ttLaborDtl.DspClockOutTime = this.convMin(Compatibility.Convert.ToString(ttLaborDtl.ClockOutTime));
                }
                else
                {
                    ttLaborDtl.DspClockOutTime = this.convDec(Compatibility.Convert.ToString(ttLaborDtl.ClockOutTime));
                }

                if (ttLaborDtl.ActiveTrans == false)
                {
                    this.payHoursDtl(false, false, true, out vMessage);
                }

                this.validDate();
                ExceptionManager.AssertNoBLExceptions();
                if (BIttLaborDtl != null && ttLaborDtl.PayrollDate.Value.Date != BIttLaborDtl.PayrollDate.Value.Date)
                {
                    eadErrMsg = LibEADValidation.validateEAD(ttLaborDtl.PayrollDate, "IP", "");
                    if (!String.IsNullOrEmpty(eadErrMsg))
                    {
                        throw new BLException(eadErrMsg, "ttLaborDtl");
                    }
                }
                /* setup */
                if (ttLaborDtl.StartActivity && ttLaborDtl.LaborType.Compare("S") == 0)
                {


                    bJobOper = this.FindFirstJobOper22(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                    if (bJobOper != null && bJobOper.FAQty > 0)
                    {
                        this.createFirstArt(bJobOper);
                    }
                }
                if (ttLaborDtl.EndActivity == true)
                {


                    JCShift = this.FindFirstJCShift5(Session.CompanyID, LaborHed.Shift);
                    this.validateWcCode(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.ResourceGrpID, "LaborDtl", ttLaborDtl.SysRowID);
                    this.clockOutTimeMES();
                    this.calcLaborHours();
                    if (this.isHCMEnabledAt(ttLaborDtl.EmployeeNum).Equals("DTL", StringComparison.OrdinalIgnoreCase))
                    {
                        ttLaborDtl.HCMPayHours = ((ttLaborDtl.LaborHrs >= 0 && ttLaborDtl.HCMPayHours == 0) ? ttLaborDtl.LaborHrs : 0);
                    }
                    /* indirect */
                    if (ttLaborDtl.LaborType.Compare("I") != 0)
                    {


                        JobOper = this.FindFirstJobOper23(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                        if ((ttLaborDtl.LaborType.Compare("P") == 0 /* production */ &&
                             ttLaborDtl.Complete) ||
                            (ttLaborDtl.LaborType.Compare("S") == 0 /* setup */ &&
                             ttLaborDtl.Complete &&
                             JobOper.EstProdHours == 0))
                        {
                            ttLaborDtl.OpComplete = true;
                        }

                        this.calcBurdenHours();
                        if (ttLaborDtl.ReWork == false) lRuncreateMtlqPWip = true;
                        this.warnHrs(out vMessage);
                        this.warnQty(ttLaborDtl.LaborQty, out vMessage);
                        this.warnLabor(ref vMessage);
                    }
                    ttLaborDtl.ActiveTrans = false;
                    ErpCallContext.Properties["FromEndActivity"] = "";
                    saveLaborQty = ((LaborDtl != null) ? LaborDtl.LaborQty : 0);
                    /* SCR 92148 - if no changes in the LaborPart then make sure that the existing LaborPart *
                     * quantities get added to the JobPart when ending the activity from MES.               */



                    runEndActJobPartUpd = (((tmpLaborPartRows == null) || (!(from tmpLaborPart_Row in tmpLaborPartRows
                                                                             select tmpLaborPart_Row).Any())) ? true : false);
                } /* ttLaborDtl.EndActivity = true */
                ttLaborDtl.TimeStatus = ((String.IsNullOrEmpty(ttLaborDtl.TimeStatus)) ? "E" : ttLaborDtl.TimeStatus);
                lcRefreshEmployeeNum = ttLaborDtl.EmployeeNum;
                ldFromDate = ((BIttLaborDtl != null) ? BIttLaborDtl.PayrollDate : ttLaborDtl.PayrollDate);
                ldToDate = ttLaborDtl.PayrollDate;
                if (ldFromDate.Value.Date > ldToDate.Value.Date)
                {
                    ldRefreshToDate = ((ldRefreshToDate == null || ldFromDate.Value.Date > ldRefreshToDate.Value.Date) ? ldFromDate : ldRefreshToDate);
                    ldRefreshFromDate = ((ldRefreshFromDate == null || ldToDate.Value.Date < ldRefreshFromDate.Value.Date) ? ldToDate : ldRefreshFromDate);
                }
                else
                {
                    ldRefreshToDate = ((ldRefreshToDate == null || ldToDate.Value.Date > ldRefreshToDate.Value.Date) ? ldToDate : ldRefreshToDate);
                    ldRefreshFromDate = ((ldRefreshFromDate == null || ldFromDate.Value.Date < ldRefreshFromDate.Value.Date) ? ldFromDate : ldRefreshFromDate);
                }
                /* SCR 70293 - for Project time, BurdenHrs should always equal LaborHrs */
                if (ttLaborDtl.LaborTypePseudo.Compare("J") == 0)
                {
                    ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
                }
                if (ttLaborDtl.ActiveTrans == false)
                {
                    this.checkLaborDtlChange();
                }
                if (JobOper == null)
                {


                    JobOper = this.FindFirstJobOper(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                }
                if (JobOper != null && JobOper.RequestMove != ttLaborDtl.RequestMove)
                {
                    using (Db.DisableTriggerScope(Erp.Tables.JobOper.GetTableName(), TriggerType.Write))
                    {
                        using (TransactionScope updJobOptx = ErpContext.CreateDefaultTransactionScope())
                        {
                            Db.ReadCurrent(ref JobOper, LockHint.UpdLock);
                            JobOper.RequestMove = ttLaborDtl.RequestMove;
                            Db.Validate(JobOper);
                            updJobOptx.Complete();
                        }
                    }
                }
            } //if not an ExternalMES

            validateFSAExtFlds(ttLaborDtl);

            // Validate all "Required" actions are completed prior to closing operation. Checks on db records and currentFullTableset records.
            if (ttLaborDtl.OpComplete)
            {
                bool incomingRequiredActionNotCompleted = (from row in CurrentFullTableset.LaborDtlAction
                                                           where row.Required == true &&
                                                                 row.Completed == false &&
                                                                 (row.RowMod == IceRow.ROWSTATE_UPDATED || row.RowMod == IceRow.ROWSTATE_ADDED)
                                                           select row).Any();
                if (incomingRequiredActionNotCompleted)
                {
                    throw new BLException(Strings.CannotCompleteOpActionReq);
                }
                // If theres no incoming changes or new rows, check DB records
                else if (CurrentFullTableset.LaborDtlAction.Count == 0)
                {
                    bool requiredActionNotCompleted = this.ExistRequiredOpenJobOperAction(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                    if (requiredActionNotCompleted)
                    {
                        throw new BLException(Strings.CannotCompleteOpActionReq);
                    }
                }


            }
        }

        partial void LaborDtlCommentAfterGetNew()
        {


            LaborDtl = this.FindFirstLaborDtl5(Session.CompanyID, ttLaborDtlComment.LaborHedSeq, ttLaborDtlComment.LaborDtlSeq);
            if (LaborDtl != null)
            {
                if (String.IsNullOrEmpty(LaborDtl.TimeStatus) ||
                    LaborDtl.TimeStatus.Compare("E") == 0 ||
                    LaborDtl.TimeStatus.Compare("S") == 0 ||
                    LaborDtl.TimeStatus.Compare("R") == 0)
                {
                    if ((this.ExistsLaborDtlComment(Session.CompanyID, ttLaborDtlComment.LaborHedSeq, ttLaborDtlComment.LaborDtlSeq, "INV")))
                    {
                        ttLaborDtlComment.CommentType = "SUB";
                    }
                }
                else if (LaborDtl.TimeStatus.Equals("A", StringComparison.OrdinalIgnoreCase))
                {
                    throw new BLException(Strings.CannotEnterCommentForApproved);
                }
                else
                {
                    ttLaborDtlComment.CommentType = "APP";
                }

                assignTimeEntryCommentTypeList(ref ttLaborDtlComment);
                assignCommentTypeDesc(ref ttLaborDtlComment);
            }
        }

        partial void LaborDtlCommentAfterGetRows()
        {
            ttLaborDtlComment.DspCreateTime = ((ttJCSyst.ClockFormat.Compare("M") == 0) ? Compatibility.Convert.TimeToString(ttLaborDtlComment.CreateTime, "HH:MM") : Compatibility.Convert.ToString(((decimal)ttLaborDtlComment.CreateTime / 3600M), ">9.99"));
            ttLaborDtlComment.DspChangeTime = ((ttJCSyst.ClockFormat.Compare("M") == 0) ? Compatibility.Convert.TimeToString(ttLaborDtlComment.ChangeTime, "HH:MM") : Compatibility.Convert.ToString(((decimal)ttLaborDtlComment.ChangeTime / 3600M), ">9.99"));
            ttLaborDtlComment.TreeNodeImageName = ((ttLaborDtlComment.CommentType.Compare("SUB") == 0) ? "Submitted" : ((ttLaborDtlComment.CommentType.Compare("APP") == 0) ? "Approved" : ((ttLaborDtlComment.CommentType.Compare("REJ") == 0) ? "Rejected" : "")));
            assignTimeEntryCommentTypeList(ref ttLaborDtlComment);
            assignCommentTypeDesc(ref ttLaborDtlComment);
        }
        partial void LaborDtlCommentAfterCreate()
        {
            LaborDtl = this.FindFirstLaborDtl(Session.CompanyID, ttLaborDtlComment.LaborHedSeq, ttLaborDtlComment.LaborDtlSeq);
            if (LaborDtl != null)
            {
                ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
                CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
                BufferCopy.Copy(LaborDtl, ref ttLaborDtl);
                ttLaborDtl.SysRowID = LaborDtl.SysRowID;
                this.LaborDtlAfterGetRows();
            }
        }

        partial void LaborDtlCommentAfterUpdate()
        {
            //ERP-335 - this is intentional to use PatchFld - very short lived record
            //DO NOT remove this PatchFLd
            if (!string.IsNullOrEmpty(ttLaborDtlComment.IntExternalKey))
            {
                string patchKey = Compatibility.Convert.ToString(ttLaborDtlComment.LaborHedSeq) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(ttLaborDtlComment.LaborDtlSeq) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(ttLaborDtlComment.CommentNum);
                LibUsePatchFld.SetPatchFldChar(Session.CompanyID, "LaborDtlComment", "IntExternalKey", patchKey, ttLaborDtlComment.IntExternalKey);
            }
        }

        partial void LaborDtlCommentBeforeCreate()
        {
            ttLaborDtlComment.CreateDate = CompanyTime.Today();
            ttLaborDtlComment.CreateTime = CompanyTime.Now().SecondsSinceMidnight();
            ttLaborDtlComment.CreatedBy = Session.UserID;
        }

        partial void LaborDtlCommentBeforeDelete()
        {
            throw new BLException(Strings.LaborCommentsCannotBeDeleted, "LaborDtl");
        }

        partial void LaborDtlCommentBeforeUpdate()
        {
            if ((!String.IsNullOrEmpty(ttLaborDtlComment.RowMod) && ttLaborDtlComment.RowMod.Compare("A") != 0 &&
                ttLaborDtlComment.CommentType.Compare(BIttLaborDtlComment.CommentType) != 0))
            {
                throw new BLException(Strings.CommentTypeCannotBeChanged, "LaborDtlComment", ttLaborDtlComment.CommentType);
            }
            if (ttLaborDtlComment.CommentType.Compare("INV") == 0)
            {


                if ((this.ExistsLaborDtlComment(Session.CompanyID, ttLaborDtlComment.LaborHedSeq, ttLaborDtlComment.LaborDtlSeq, "INV", ttLaborDtlComment.SysRowID)))
                {
                    throw new BLException(Strings.AnInvoiceCommentTypeAlreadyExistsForThisTimeRecord, "LaborDtlComment", ttLaborDtlComment.CommentType);
                }
            }
        }

        /// <summary>
        /// </summary>
        public void LaborRateCalc(ref LaborTableset ds)
        {
            if (ttLaborDtl == null)
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();

            if (ttLaborDtl != null)
            {
                if (!ttLaborDtl.LaborEntryMethod.Equals("Q", StringComparison.OrdinalIgnoreCase))
                {
                    ttLaborDtl.LaborRate = this.LibLaborRate.LaborRateCalc(ttLaborDtl);
                }
                ttLaborDtl.BillServiceRate = this.LibBillableServiceRate.BillableServiceRateCalc(ttLaborDtl.EmployeeNum, ttLaborDtl.ExpenseCode, ttLaborDtl.LaborType, ttLaborDtl.CallNum, ttLaborDtl.CallLine, ttLaborDtl.JobNum, ref ds);
            }
        }

        partial void LaborDtlSetDefaults(LaborDtlRow ttLaborDtl)
        {
            Erp.Tables.LaborHed altLaborHed = null;
            string vMessage = string.Empty;
            if (ttLaborDtl.LaborHedSeq == 0)
            {

                ttLaborDtl.LaborRate = this.LibLaborRate.LaborRateCalc(ttLaborDtl);
                ttLaborDtl.BillServiceRate = this.LibBillableServiceRate.BillableServiceRateCalc(ttLaborDtl.EmployeeNum, ttLaborDtl.ExpenseCode, ttLaborDtl.LaborType, ttLaborDtl.CallNum, ttLaborDtl.CallLine, ttLaborDtl.JobNum);
                ttLaborDtl.Complete = false;
                ttLaborDtl.OpComplete = false;
                ttLaborDtl.JCSystReworkReasons = ttJCSyst.ReworkReasons;
                /* Set DisPrjRoleCd and DisTimeTypCd */
                this.disPrjFields(true);
                /* Set DisLaborType */
                this.disLaborTypeProc();
            }
            else
            {


                altLaborHed = this.FindFirstLaborHed7(Session.CompanyID, ttLaborDtl.LaborHedSeq);
                if (altLaborHed == null)
                {
                    throw new BLException(Strings.LaborTimeNotFound);
                }
                /* validate employee id */



                EmpBasic = this.FindFirstEmpBasic13(Session.CompanyID, altLaborHed.EmployeeNum);
                if (EmpBasic == null)
                {
                    throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.EmployeeID));
                }
                if (EmpBasic.EmpStatus.Compare("A") != 0)
                {
                    ExceptionManager.AddBLException(Strings.EmployeeNotActive);
                }

                ExceptionManager.AssertNoBLExceptions();
                if (String.IsNullOrEmpty(ttLaborDtl.LaborTypePseudo))
                {
                    ttLaborDtl.LaborTypePseudo = "P"; /* production */
                    ttLaborDtl.LaborType = "P";
                }/* production */
                ttLaborDtl.EmployeeNum = altLaborHed.EmployeeNum;
                ttLaborDtl.ClockInDate = altLaborHed.ClockInDate;
                ttLaborDtl.PayrollDate = altLaborHed.PayrollDate;
                ttLaborDtl.ClockinTime = altLaborHed.ClockInTime;
                ttLaborDtl.ClockOutTime = altLaborHed.ClockOutTime;
                ttLaborDtl.LaborRate = this.LibLaborRate.LaborRateCalc(ttLaborDtl);
                ttLaborDtl.BillServiceRate = this.LibBillableServiceRate.BillableServiceRateCalc(ttLaborDtl.EmployeeNum, ttLaborDtl.ExpenseCode, ttLaborDtl.LaborType, ttLaborDtl.CallNum, ttLaborDtl.CallLine, ttLaborDtl.JobNum);
                ttLaborDtl.Complete = false;
                ttLaborDtl.OpComplete = false;
                ttLaborDtl.ActiveTrans = altLaborHed.ActiveTrans;
                ttLaborDtl.LaborCollection = false;
                ttLaborDtl.JCSystReworkReasons = ttJCSyst.ReworkReasons;
                if (ttLaborDtl.LaborDtlSeq == 0)
                {
                    //ttLaborDtl.LaborDtlSeq = Compatibility.Convert.ToInt32(Services.Transition.Sequences.LaborDtlSeq) + 1;
                    ttLaborDtl.LaborDtlSeq = LibNextValue.GetNextSequence("LaborDtlSeq");
                }
                /*** if coming from ShopFloor, use offsetToday ***/
                /* if altLaborHed.ActiveTrans */
                if (altLaborHed.ActiveTrans)
                {
                    this.clockInTimeMES();
                }
                else
                {
                    if (altLaborHed.ClockInTime > altLaborHed.ClockOutTime)
                    {


                        //Last_Loop:
                        foreach (var altLaborDtl_iterator in (this.SelectLaborDtl2(Session.CompanyID, altLaborHed.LaborHedSeq, altLaborHed.ClockInTime).OrderByDescending(calc => calc.calc_ClockInDate).ThenByDescending(clockOut => clockOut.ClockOutTime)))
                        {
                            LaborDtlCustom altLaborDtlCust = altLaborDtl_iterator;
                            ttLaborDtl.ClockinTime = altLaborDtlCust.ClockOutTime;            /* spans midnight - bump date if after midnight */
                            if (ttLaborDtl.ClockinTime < altLaborHed.ClockInTime)
                            {
                                ttLaborDtl.ClockInDate = (ttLaborDtl.ClockInDate.Value.AddDays(1)).Date;
                            }

                            break;
                        }

                    }
                    else
                    {


                        //Last_Loop1:
                        foreach (var altLaborDtl_iterator in (this.SelectLaborDtl2(Session.CompanyID, altLaborHed.LaborHedSeq, altLaborHed.ClockInTime).OrderByDescending(calc => calc.calc_ClockInDate).ThenByDescending(clockOut => clockOut.ClockOutTime)))
                        {
                            LaborDtlCustom altLaborDtlCust = altLaborDtl_iterator;
                            ttLaborDtl.ClockinTime = altLaborDtlCust.ClockOutTime;
                            break;
                        }

                    }
                    /* if clockin time = 24 set to 0 since start a midnight is always 0 */
                    if (ttLaborDtl.ClockinTime == 24.0m)
                    {
                        ttLaborDtl.ClockinTime = 0;
                    }

                    this.payHoursDtl(false, true, true, out vMessage);
                }/*** else altLaborHed.ActiveTrans = no ***/
                /* set the display-formatted times */
                this.setDisplayTime();
                if (ttLaborDtl.ClockOutTime == 24.0m)
                {
                    ttLaborDtl.ClockOutTime = 0;
                }

                if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) && ttLaborDtl.LaborTypePseudo.Compare("V") != 0 /* service */ && !EmpBasic.AllowDirLbr)
                {
                    ttLaborDtl.LaborType = "I";
                }
                /* Set DisPrjRoleCd and DisTimeTypCd */
                this.disPrjFields(true);
                /* Set DisLaborType */
                this.disLaborTypeProc();
            }/* else  ttLaborDtl.LaborHedSeq = 0 */

            ttLaborDtl.TimeAutoSubmit = this.IsTimeAutoSubmitPlantConfCtrl(Session.CompanyID, Session.PlantID);

            /* SCR 94819 format time for display */
            if (ttJCSyst.ClockFormat.Compare("M") == 0)      /* Hrs:Minutes */
            {
                ttLaborDtl.DspClockInTime = this.convMin(Compatibility.Convert.ToString(ttLaborDtl.ClockinTime));
                ttLaborDtl.DspClockOutTime = this.convMin(Compatibility.Convert.ToString(ttLaborDtl.ClockOutTime));
            }
            else        /* Hrs.Hundreths */
            {
                ttLaborDtl.DspClockInTime = this.convDec(Compatibility.Convert.ToString(ttLaborDtl.ClockinTime));
                ttLaborDtl.DspClockOutTime = this.convDec(Compatibility.Convert.ToString(ttLaborDtl.ClockOutTime));
            }
        }

        partial void LaborEquipBeforeUpdate()
        {
            Equip = this.FindFirstEquip(Session.CompanyID, ttLaborEquip.EquipID);
            if (Equip == null)
            {
                throw new BLException(Strings.EquipIDDoesNotExist, "Equip");
            }
            if (Equip.InActive)
            {
                throw new BLException(Strings.EquipIDIsInactive, "Equip");
            }
            if (this.ExistsLaborEquip(Session.CompanyID, ttLaborEquip.LaborHedSeq, ttLaborEquip.LaborDtlSeq, ttLaborEquip.SysRowID, ttLaborEquip.EquipID))
            {
                throw new BLException(Strings.EquipMustBeUnique, "LaborEquip", "EquipID");
            }
        }

        partial void LaborHedAfterDelete()
        {
            lcEmployeeNum = lcRefreshEmployeeNum;
            ldCalendarStartDate = ldRefreshFromDate;
            ldCalendarEndDate = ldRefreshToDate;
            this.populateTimeValidateDates();
            this.populateTimeWorkHours();
            this.populateTimeWeeklyView();
        }

        private void LaborHedAfterGetNew1(bool ShopFloor)
        {


            EmpBasic = this.FindFirstEmpBasic14(Session.CompanyID, ttLaborHed.EmployeeNum);
            if (EmpBasic == null)
            {
                throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.EmployeeID));
            }

            if (EmpBasic.EmpStatus.Compare("A") != 0)
            {
                ExceptionManager.AddBLException(Strings.EmployeeNotActive);
            }



            JCShift = this.FindFirstJCShift6(Session.CompanyID, EmpBasic.Shift);
            ExceptionManager.AssertNoBLExceptions();
            /* Have LaborHed.FeedPayRoll default from EmpBasic.PayRoll initially AXL (3807)*/

            if (EmpBasic != null)
            {
                ttLaborHed.FeedPayroll = EmpBasic.Payroll && ttJCSyst.FeedPayroll;
            }

            ttLaborHed.Shift = EmpBasic.Shift;
            ttLaborHed.ShiftDescription = this.FindFirstJCShiftDescription(Session.CompanyID, ttLaborHed.Shift);
            ttLaborHed.EmpBasicShift = EmpBasic.Shift;
            ttLaborHed.EmpBasicSupervisorID = EmpBasic.SupervisorID;
            ttLaborHed.ImagePath = this.getImagePath(EmpBasic.PhotoFile);

            ttLaborHed.WipPosted = false;
            if (((ShopFloor) ? this.LibOffset.OffsetToday() : (CompanyTime.Now().AddDays(-1))) == null)
            {
                ttLaborHed.ActualClockinDate = null;
            }
            else
            {
                ttLaborHed.ActualClockinDate = ((ShopFloor) ? this.LibOffset.OffsetToday().Value.Date : (CompanyTime.Now().AddDays(-1)).Date);
            }

            if (((ShopFloor) ? this.LibOffset.OffsetToday() : (CompanyTime.Now().AddDays(-1))) == null)
            {
                ttLaborHed.ClockInDate = null;
            }
            else
            {
                ttLaborHed.ClockInDate = ((ShopFloor) ? this.LibOffset.OffsetToday().Value.Date : (CompanyTime.Now().AddDays(-1)).Date);
            }

            ttLaborHed.ActiveTrans = ShopFloor;
            ttLaborHed.LaborCollection = ShopFloor;
            ttLaborHed.LunchStatus = "N";
            if (JCShift != null)
            {
                ttLaborHed.ActualClockInTime = JCShift.StartTime;
                ttLaborHed.ActualClockOutTime = JCShift.EndTime;
                ttLaborHed.ActLunchOutTime = JCShift.LunchStart;
                ttLaborHed.ActLunchInTime = JCShift.LunchEnd;
                ttLaborHed.ClockInTime = JCShift.StartTime;
                ttLaborHed.ClockOutTime = JCShift.EndTime;
                ttLaborHed.LunchOutTime = JCShift.LunchStart;
                ttLaborHed.LunchInTime = JCShift.LunchEnd;
            }
            /* To default Payroll date (3908) */
            if (ttLaborHed.PayrollDate == null)
            {
                ttLaborHed.PayrollDate = ((CompanyTime.Now().AddDays(-1)).Date);
            }

            ttLaborHed.PayrollDateNav = ttLaborHed.PayrollDate;

            /* To default Actual and Adjusted ClockInDates (4676) */
            if (ttLaborHed.ActualClockinDate == null)
            {
                ttLaborHed.ActualClockinDate = ((CompanyTime.Now().AddDays(-1)).Date);

            }

            if (ttLaborHed.ClockInDate == null)
            {
                ttLaborHed.ClockInDate = ((CompanyTime.Now().AddDays(-1)).Date);
            }

            if (JCShift != null)
            {
                if (ttJCSyst.ClockFormat.Compare("M") == 0)
                {
                    ttLaborHed.DspClockInTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborHed.ClockInTime), "99") + ":" + Compatibility.Convert.ToString(((ttLaborHed.ClockInTime - Math.Truncate(ttLaborHed.ClockInTime)) * 60), "99");
                    ttLaborHed.DspClockOutTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborHed.ClockOutTime), "99") + ":" + Compatibility.Convert.ToString(((ttLaborHed.ClockOutTime - Math.Truncate(ttLaborHed.ClockOutTime)) * 60), "99");
                }
                else
                {
                    ttLaborHed.DspClockInTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborHed.ClockInTime), "99") + Compatibility.Convert.ToString((ttLaborHed.ClockInTime - Math.Truncate(ttLaborHed.ClockInTime)), ".99");
                    ttLaborHed.DspClockOutTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborHed.ClockOutTime), "99") + Compatibility.Convert.ToString((ttLaborHed.ClockOutTime - Math.Truncate(ttLaborHed.ClockOutTime)), ".99");
                }
                this.payHours();
            }
            if (ttLaborHed.LunchInTime != 0 || ttLaborHed.LunchOutTime != 0)
            {
                ttLaborHed.LunchBreak = true;
            }

            this.setdispValue();
            string HCMPayHoursCalcType = FindFirstLaborHedHCMPayHoursCalcType(ttLaborHed.SysRowID);
            if (this.IsHCMEnabledAtCompany(Session.CompanyID, true))
                ttLaborHed.PayrollValuesForHCM = (string.IsNullOrEmpty(HCMPayHoursCalcType)) ? isHCMEnabledAt(ttLaborHed.EmployeeNum) : HCMPayHoursCalcType;
            else
                ttLaborHed.PayrollValuesForHCM = "NON";
        }

        partial void LaborHedAfterGetRows()
        {
            bool supervisorHasRights = false;
            decimal outTotLbrHrs = decimal.Zero;
            decimal outTotBurHrs = decimal.Zero;
            decimal outTotHCMPayHrs = decimal.Zero;
            string hcmSource = string.Empty;

            EmpBasic = this.FindFirstEmpBasic14(Session.CompanyID, ttLaborHed.EmployeeNum);

            ttLaborHed.WipPosted = this.getWipPosted(ttLaborHed.LaborHedSeq);
            ttLaborHed.ImagePath = this.getImagePath(EmpBasic.PhotoFile);

            if (ttLaborHed.LunchInTime != 0 || ttLaborHed.LunchOutTime != 0)
            {
                ttLaborHed.LunchBreak = true;
            }
            /* set hours to display value */
            this.setdispValue();

            if (this.IsHCMEnabledAtCompany(Session.CompanyID, true))
            {
                string HCMPayHoursCalcType = FindFirstLaborHedHCMPayHoursCalcType(ttLaborHed.SysRowID);
                ttLaborHed.PayrollValuesForHCM = (string.IsNullOrEmpty(HCMPayHoursCalcType)) ? "DTL" : HCMPayHoursCalcType;
            }
            else
                ttLaborHed.PayrollValuesForHCM = "NON";



            if (EmpBasic != null)
            {
                ttLaborHed.EmpBasicShift = EmpBasic.Shift;
                ttLaborHed.EmpBasicSupervisorID = EmpBasic.SupervisorID;
            }

            /* ACC - 09/13/05* - SCR 24315 */
            ttLaborHed.DspPayHours = ttLaborHed.PayHours;
            if (lFromGetRowsCalendarView == true)
            {


                if (EmpBasic == null || !EmpBasic.EmpID.Equals(ttLaborHed.EmployeeNum, StringComparison.OrdinalIgnoreCase))
                {
                    EmpBasic = EmpBasic.FindFirstByPrimaryKey(Db, Session.CompanyID, ttLaborHed.EmployeeNum);
                }

                if (EmpBasic != null)
                {/* if EmpBasic.DCDUserID <> DCD-USERID */
                    if (!EmpBasic.DcdUserID.KeyEquals(Session.UserID))
                    {


                        PlantConfCtrl = this.FindFirstPlantConfCtrl2(Session.CompanyID, Session.PlantID);
                        if (PlantConfCtrl != null && PlantConfCtrl.TimeRestrictEntry)
                        {
                            supervisorHasRights = this.getSupervisorRights(ttLaborHed.EmployeeNum);
                            if (supervisorHasRights || this.CanUserUpdateTime(Session.CompanyID, Session.UserID, true))
                            {
                                ttLaborHed.TimeDisableUpdate = false;
                                ttLaborHed.TimeDisableDelete = false;
                            }
                            else
                            {
                                ttLaborHed.TimeDisableUpdate = true;
                                ttLaborHed.TimeDisableDelete = true;
                            }
                        }
                    }
                    else
                    {
                        if (EmpBasic.DisallowTimeEntry == true)
                        {
                            ttLaborHed.TimeDisableUpdate = true;
                            ttLaborHed.TimeDisableDelete = true;
                        }
                    }
                }
            }
            /*hcm integration*/
            outTotLbrHrs = ttLaborHed.TotLbrHrs;
            outTotBurHrs = ttLaborHed.TotBurHrs;
            outTotHCMPayHrs = ttLaborHed.HCMTotPayHours;
            this.getDetailHours(ttLaborHed.LaborHedSeq, out outTotLbrHrs, out outTotBurHrs, out outTotHCMPayHrs, ttLaborHed.PayrollValuesForHCM);
            ttLaborHed.TotLbrHrs = outTotLbrHrs;
            ttLaborHed.TotBurHrs = outTotBurHrs;
            ttLaborHed.HCMTotPayHours = outTotHCMPayHrs;

            int totalLaborDtl = 0;
            int totalPostedLaborDtl = 0;
            int totalLaborDtlNonBlankHCMStatus = 0;

            ttLaborHed.HCMExistsWithStatus = false;

            if (ttLaborHed.PayrollValuesForHCM.Equals("HDR", StringComparison.OrdinalIgnoreCase) ||
                ttLaborHed.PayrollValuesForHCM.Equals("DTL", StringComparison.OrdinalIgnoreCase))
            {
                if (ExistsHCMLaborDtlSync(ttLaborHed.Company, ttLaborHed.SysRowID))
                {
                    ttLaborHed.HCMExistsWithStatus = true;
                }
            }

            foreach (var laborDtlRow in (this.SelectLaborDtl(ttLaborHed.Company, ttLaborHed.LaborHedSeq)))
            {
                totalLaborDtl += 1;
                if (laborDtlRow.WipPosted == true)
                {
                    totalPostedLaborDtl += 1;
                }

                if (ttLaborHed.PayrollValuesForHCM.Equals("HDR", StringComparison.OrdinalIgnoreCase) ||
                    ttLaborHed.PayrollValuesForHCM.Equals("DTL", StringComparison.OrdinalIgnoreCase))
                {
                    if (ExistsHCMLaborDtlSync(laborDtlRow.Company, laborDtlRow.SysRowID))
                    {
                        totalLaborDtlNonBlankHCMStatus += 1;
                    }
                }
            }

            if (totalLaborDtlNonBlankHCMStatus > 0)
            {
                ttLaborHed.HCMExistsWithStatus = true;
            }

            if (totalPostedLaborDtl == 0)
            {
                ttLaborHed.FullyPosted = false;
                ttLaborHed.PartiallyPosted = false;
            }
            else
            {
                if (totalPostedLaborDtl == totalLaborDtl)
                {
                    ttLaborHed.FullyPosted = true;
                    ttLaborHed.PartiallyPosted = false;
                }
                else
                {
                    ttLaborHed.FullyPosted = false;
                    ttLaborHed.PartiallyPosted = true;
                }
            }

            ttLaborHed.PayrollDateNav = ttLaborHed.PayrollDate;
        }

        partial void LaborHedAfterUpdate()
        {
            ttLaborHed.GetNewNoHdr = false;
            if (BIttLaborHed != null)
            {
                if (BIttLaborHed.PayrollDate.Value.Date != ttLaborHed.PayrollDate.Value.Date ||
                    BIttLaborHed.Shift != ttLaborHed.Shift)
                {
                    lcRefreshEmployeeNum = ttLaborHed.EmployeeNum;
                    ldFromDate = BIttLaborHed.PayrollDate;
                    ldToDate = ttLaborHed.PayrollDate;
                    if (ldFromDate.Value.Date > ldToDate.Value.Date)
                    {
                        ldRefreshToDate = ((ldRefreshToDate == null || ldFromDate.Value.Date > ldRefreshToDate.Value.Date) ? ldFromDate : ldRefreshToDate);
                        ldRefreshFromDate = ((ldRefreshFromDate == null || ldToDate.Value.Date < ldRefreshFromDate.Value.Date) ? ldToDate : ldRefreshFromDate);
                    }
                    else
                    {
                        ldRefreshToDate = ((ldRefreshToDate == null || ldToDate.Value.Date > ldRefreshToDate.Value.Date) ? ldToDate : ldRefreshToDate);
                        ldRefreshFromDate = ((ldRefreshFromDate == null || ldFromDate.Value.Date < ldRefreshFromDate.Value.Date) ? ldFromDate : ldRefreshFromDate);
                    }



                    foreach (var LaborDtl_iterator in (this.SelectLaborDtl4(ttLaborHed.Company, ttLaborHed.LaborHedSeq)))
                    {
                        LaborDtl = LaborDtl_iterator;
                        this.refreshTtLaborDtl(LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq);



                        ttLaborDtl = (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                                      where ttLaborDtl_Row.Company.KeyEquals(Session.CompanyID) &&
                                      ttLaborDtl_Row.LaborHedSeq == LaborDtl.LaborHedSeq &&
                                      ttLaborDtl_Row.LaborDtlSeq == LaborDtl.LaborDtlSeq
                                      select ttLaborDtl_Row).FirstOrDefault();
                        if (ttLaborDtl != null)
                        {
                            DelFlag = true;
                            DelEmployeeNum = ttLaborDtl.EmployeeNum;
                            DelWeekBeginDate = ttLaborDtl.PayrollDate.Value.AddDays(1 - Convert.ToInt32(ttLaborDtl.PayrollDate.Value.DayOfWeek + 1));
                            DelWeekEndDate = ttLaborDtl.PayrollDate.Value.AddDays(7 - Convert.ToInt32(ttLaborDtl.PayrollDate.Value.DayOfWeek + 1));
                            DelLaborType = ttLaborDtl.LaborType;
                            DelLaborTypePseudo = ttLaborDtl.LaborTypePseudo;
                            DelProjectID = ttLaborDtl.ProjectID;
                            DelPhaseID = ttLaborDtl.PhaseID;
                            DelTimeTypCd = ttLaborDtl.TimeTypCd;
                            DelJobNum = ttLaborDtl.JobNum;
                            DelAssemblySeq = ttLaborDtl.AssemblySeq;
                            DelOprSeq = ttLaborDtl.OprSeq;
                            DelIndirectCode = ttLaborDtl.IndirectCode;
                            DelRoleCd = ttLaborDtl.RoleCd;
                            DelResourceGrpID = ttLaborDtl.ResourceGrpID;
                            DelResourceID = ttLaborDtl.ResourceID;
                            DelExpenseCode = ttLaborDtl.ExpenseCode;
                            DelShift = BIttLaborHed.Shift;
                            DelStatus = ttLaborDtl.TimeStatus;
                            DelQuickEntryCode = ttLaborDtl.QuickEntryCode;
                            this.createDelttTimeWeeklyView();
                        }/* if available ttLaborDtl */
                    }
                }
            }
        }

        /// <summary>
        /// This method is to swap the before image endtimes back to 24.0 from 0.
        /// </summary>
        partial void LaborHedBeforeBI()
        {
            Erp.Tables.LaborHed altHed = null;

            altHed = this.FindFirstLaborHed(ttLaborHed.SysRowID);
            /* set the LaborHed clockouttime back to 24 from 0 */
            if (BIttLaborHed != null)
            {
                if (altHed != null)
                {
                    BIttLaborHed.ClockOutTime = altHed.ClockOutTime;
                    BIttLaborHed.ActualClockOutTime = altHed.ActualClockOutTime;
                    BIttLaborHed.LunchInTime = altHed.LunchInTime;
                    BIttLaborHed.ActLunchInTime = altHed.ActLunchInTime;
                }
            }
            /* DefaultTime takes care of defaulting clockouttime from actualtime */
            if (ttLaborHed.ActualClockOutTime == 0)
            {
                ttLaborHed.ActualClockOutTime = 24.0m;
            }

            if (ttLaborHed.ClockOutTime == 0)
            {
                ttLaborHed.ClockOutTime = 24;
            }
            /* only flip time from 0 to 24 if user indicates a lunchbreak */
            if (ttLaborHed.LunchBreak)
            {
                if (ttLaborHed.ActLunchInTime == 0)
                {
                    ttLaborHed.ActLunchInTime = 24.0m;
                }

                if (ttLaborHed.LunchInTime == 0)
                {
                    ttLaborHed.LunchInTime = 24.0m;
                }
            }
        }

        partial void LaborHedBeforeCreate()
        {
            //ttLaborHed.LaborHedSeq = Compatibility.Convert.ToInt32(Services.Transition.Sequences.LaborHedSeq) + 1;
            ttLaborHed.LaborHedSeq = LibNextValue.GetNextSequence("LaborHedSeq");
            /* If there are ttLaborDtl records in add mode for the header,
               update the LaborHedSeq value.  This condition should exist
               only if the GetNewLaborDtlNoHdr method was used to add
               records to the dataset. */



            foreach (var ttLaborDtl_iterator in (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                                                 where ttLaborDtl_Row.Company.Compare(ttLaborHed.Company) == 0
                                                 && ttLaborDtl_Row.RowMod.Compare(IceRow.ROWSTATE_ADDED) == 0
                                                 select ttLaborDtl_Row))
            {
                ttLaborDtl = ttLaborDtl_iterator;
                ttLaborDtl.LaborHedSeq = ttLaborHed.LaborHedSeq;



                foreach (var ttLbrScrapSerialNumbers_iterator in (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                                                  where ttLbrScrapSerialNumbers_Row.Company.KeyEquals(Session.CompanyID)
                                                                  && ttLbrScrapSerialNumbers_Row.LaborHedSeq == 0
                                                                  && ttLbrScrapSerialNumbers_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq
                                                                  select ttLbrScrapSerialNumbers_Row))
                {
                    ttLbrScrapSerialNumbers = ttLbrScrapSerialNumbers_iterator;
                    ttLbrScrapSerialNumbers.LaborHedSeq = ttLaborDtl.LaborHedSeq;
                }
            }

            this.LaborHedBeforeBI();
        }

        partial void LaborHedBeforeDelete()
        {
            if (LaborHed.ActiveTrans == true)
            {
                throw new BLException(Strings.InvalidToModifyAnActiveTrans, "LaborHed");
            }
            lcRefreshEmployeeNum = LaborHed.EmployeeNum;
            ldFromDate = ((BIttLaborHed != null) ? BIttLaborHed.PayrollDate : LaborHed.PayrollDate);
            ldToDate = LaborHed.PayrollDate;
            if (ldFromDate.Value.Date > ldToDate.Value.Date)
            {
                ldRefreshToDate = ((ldRefreshToDate == null || ldFromDate.Value.Date > ldRefreshToDate.Value.Date) ? ldFromDate : ldRefreshToDate);
                ldRefreshFromDate = ((ldRefreshFromDate == null || ldToDate.Value.Date < ldRefreshFromDate.Value.Date) ? ldToDate : ldRefreshFromDate);
            }
            else
            {
                ldRefreshToDate = ((ldRefreshToDate == null || ldToDate.Value.Date > ldRefreshToDate.Value.Date) ? ldToDate : ldRefreshToDate);
                ldRefreshFromDate = ((ldRefreshFromDate == null || ldFromDate.Value.Date < ldRefreshFromDate.Value.Date) ? ldFromDate : ldRefreshFromDate);
            }



            LaborDtl = this.FindFirstLaborDtl(LaborHed.Company, LaborHed.Company, LaborHed.LaborHedSeq);
            if (LaborDtl != null)
            {


                ttLaborDtl = (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                              where LaborDtl.Company.Compare(LaborHed.Company) == 0 &&
                              ttLaborDtl_Row.Company.Compare(LaborHed.Company) == 0 &&
                              ttLaborDtl_Row.LaborHedSeq == LaborHed.LaborHedSeq
                              select ttLaborDtl_Row).FirstOrDefault();
                if (ttLaborDtl == null)
                {
                    BufferCopy.Copy(LaborDtl, ref ttLaborDtl);
                }
                /* for weekly view zzz */
                this.LaborDtlBeforeDelete();
            }
        }

        partial void LaborHedBeforeUpdate()
        {
            string clockDelim = string.Empty;
            if (ttLaborHed.TransferredToPayroll)
            {
                ExceptionManager.AddBLException(Strings.CanTUpdateRecordAfterInforHasBeenTransToPayroll, "LaborHed", "TransferredToPayroll");
            }
            /* validate Shift */



            if (!((this.ExistsJCShift(Session.CompanyID, ttLaborHed.Shift))))
            {
                ExceptionManager.AddBLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.Shift), "LaborHed");
            }
            /* validate times */

            ExceptionManager.AssertNoBLExceptions();
            clockDelim = ((ttJCSyst.ClockFormat.Compare("M") == 0) ? ":" : ".");
            /* scr 19568 - defaultTime will default adjust from actual only *
             * assume time listed here is the correct time */
            /* clock in */
            this.validHrMin(ttLaborHed.ClockInTime);
            if (ttJCSyst.ClockFormat.Compare("M") == 0)
            {
                ttLaborHed.DspClockInTime = this.convMin(Compatibility.Convert.ToString(ttLaborHed.ClockInTime));
            }
            else
            {
                ttLaborHed.DspClockInTime = this.convDec(Compatibility.Convert.ToString(ttLaborHed.ClockInTime));
            }
            /* actual clock in */
            this.validHrMin(ttLaborHed.ActualClockInTime);
            /* clock out */
            this.validHrMin(ttLaborHed.ClockOutTime);
            if (ttJCSyst.ClockFormat.Compare("M") == 0)
            {
                ttLaborHed.DspClockOutTime = this.convMin(Compatibility.Convert.ToString(ttLaborHed.ClockOutTime));
            }
            else
            {
                ttLaborHed.DspClockOutTime = this.convDec(Compatibility.Convert.ToString(ttLaborHed.ClockOutTime));
            }
            /* actual clock out */
            this.validHrMin(ttLaborHed.ActualClockOutTime);
            /* lunch in */
            this.validHrMin(ttLaborHed.LunchInTime);
            /* actual lunch in */
            this.validHrMin(ttLaborHed.ActLunchInTime);
            /* lunch out */
            this.validHrMin(ttLaborHed.LunchOutTime);
            /* actual lunch out */
            this.validHrMin(ttLaborHed.ActLunchOutTime);
            /* no lunch, don't validate */
            if (ttLaborHed.ActLunchOutTime != 0 || ttLaborHed.ActLunchInTime != 0)
            {
                this.validLunchTimes(ttLaborHed.ActualClockInTime, ttLaborHed.ActualClockOutTime, ttLaborHed.ActLunchOutTime, ttLaborHed.ActLunchInTime);
            }
            /* NO LUNCH TAKEN, DON'T VALIDATE */
            if (ttLaborHed.LunchOutTime != 0 || ttLaborHed.LunchInTime != 0)
            {
                this.validLunchTimes(ttLaborHed.ClockInTime, ttLaborHed.ClockOutTime, ttLaborHed.LunchOutTime, ttLaborHed.LunchInTime);
            }

            ExceptionManager.AssertNoBLExceptions();
        }

        partial void LaborPartAfterUpdate()
        {

            tmpLaborPart = null;
            if (tmpLaborPartRows != null)
            {
                tmpLaborPart = (from tmpLaborPart_Row in tmpLaborPartRows
                                where tmpLaborPart_Row.Company.KeyEquals(LaborPart.Company) &&
                                tmpLaborPart_Row.LaborHedSeq == LaborPart.LaborHedSeq &&
                                tmpLaborPart_Row.LaborDtlSeq == LaborPart.LaborDtlSeq &&
                                tmpLaborPart_Row.PartNum.KeyEquals(LaborPart.PartNum)
                                select tmpLaborPart_Row).FirstOrDefault();
            }
            else tmpLaborPartRows = new List<_LaborPartRow>();
            if (tmpLaborPart == null)
            {
                tmpLaborPart = new _LaborPartRow();
                tmpLaborPartRows.Add(tmpLaborPart);
                tmpLaborPart.Company = LaborPart.Company;
                tmpLaborPart.LaborHedSeq = LaborPart.LaborHedSeq;
                tmpLaborPart.LaborDtlSeq = LaborPart.LaborDtlSeq;
                tmpLaborPart.PartNum = LaborPart.PartNum;
                tmpLaborPart.OldPartQty = 0;
                tmpLaborPart.OldScrapQty = 0;
                tmpLaborPart.OldDiscrepQty = 0;
            }
            tmpLaborPart.PartQty = LaborPart.PartQty;
            tmpLaborPart.ScrapQty = LaborPart.ScrapQty;
            tmpLaborPart.DiscrepQty = LaborPart.DiscrepQty;
            tmpLaborPart.DiscrpAttributeSetID = tmpLaborPart.DiscrepQty == 0 ? 0 : tmpLaborPart.DiscrpAttributeSetID;
            tmpLaborPart.ScrapAttributeSetID = tmpLaborPart.ScrapQty == 0 ? 0 : tmpLaborPart.ScrapAttributeSetID;
            runEndActJobPartUpd = false;
            if (ttLaborPart.MESProcessing == false)
            {
                return;
            }

            savePartQty = LaborPart.PartQty - savePartQty;



            LaborDtl = this.FindFirstLaborDtl6(ttLaborPart.Company, ttLaborPart.LaborHedSeq, ttLaborPart.LaborDtlSeq);



            ttLaborDtl = (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                          where ttLaborDtl_Row.Company.Compare(ttLaborPart.Company) == 0
                          && ttLaborDtl_Row.LaborHedSeq == ttLaborPart.LaborHedSeq
                          && ttLaborDtl_Row.LaborDtlSeq == ttLaborPart.LaborDtlSeq
                          && ttLaborDtl_Row.RowMod.Compare(IceRow.ROWSTATE_UPDATED) != 0
                          select ttLaborDtl_Row).LastOrDefault();
            LibPWIPMtlQ.RunPWIPMtlQ(ref sWhseCode, ref sBinNum, ref sPCID, savePartQty, LaborDtl.SysRowID, 0, 0, ((ttLaborDtl != null) ? ttLaborDtl.RequestMove : false), ttLaborPart.PartNum, string.Empty);
        }

        partial void LaborPartBeforeDelete()
        {
            del_LaborPart_LaborHedSeq = ttLaborPart.LaborHedSeq;
            del_LaborPart_LaborDtlSeq = ttLaborPart.LaborDtlSeq;
        }

        partial void LaborPartBeforeGetNew()
        {
            throw new BLException(Strings.LaborMethodNotToBeUsedLaborRecordsWillBeCreated);
        }

        partial void LaborPartBeforeUpdate()
        {

            // Validations
            validateLaborPart();

            if (LaborPart != null)
            {
                savePartQty = LaborPart.PartQty;
            }
            else
            {
                savePartQty = 0;
            }
            /* 92148 - create the tmpLaborPart here if not available yet. Assign the new  *
             * PartQty at this point so we can compare the before and after values in the *
             * afterUpdate procedure to fire off JobPart write trigger logic.             */

            tmpLaborPart = null;
            if (tmpLaborPartRows != null)
            {
                tmpLaborPart = (from tmpLaborPart_Row in tmpLaborPartRows
                                where tmpLaborPart_Row.Company.KeyEquals(ttLaborPart.Company) &&
                                tmpLaborPart_Row.LaborHedSeq == ttLaborPart.LaborHedSeq &&
                                tmpLaborPart_Row.LaborDtlSeq == ttLaborPart.LaborDtlSeq &&
                                tmpLaborPart_Row.PartNum.KeyEquals(ttLaborPart.PartNum)
                                select tmpLaborPart_Row).FirstOrDefault();
            }
            else tmpLaborPartRows = new List<_LaborPartRow>();
            if (tmpLaborPart == null)
            {
                tmpLaborPart = new _LaborPartRow();
                tmpLaborPartRows.Add(tmpLaborPart);
                tmpLaborPart.Company = ttLaborPart.Company;
                tmpLaborPart.LaborHedSeq = ttLaborPart.LaborHedSeq;
                tmpLaborPart.LaborDtlSeq = ttLaborPart.LaborDtlSeq;
                tmpLaborPart.PartNum = ttLaborPart.PartNum;
                tmpLaborPart.LaborAttributeSetID = ttLaborPart.LaborAttributeSetID;
                tmpLaborPart.DiscrpAttributeSetID = ttLaborPart.LaborAttributeSetID;
                tmpLaborPart.ScrapAttributeSetID = ttLaborPart.LaborAttributeSetID;
            }

            if (BIttLaborPart != null)
            {
                tmpLaborPart.OldPartQty = BIttLaborPart.PartQty;  /* old quantity */
                tmpLaborPart.OldScrapQty = BIttLaborPart.ScrapQty;
                tmpLaborPart.OldDiscrepQty = BIttLaborPart.DiscrepQty;
            }

            tmpLaborPart.PartQty = ttLaborPart.PartQty;
        }

        private void getLaborDtlActionDS()
        {
            CurrentFullTableset.LaborDtlAction.Clear();
            /* If there are LaborDtlAction rows already created, get those, else create new ones based on JobOperAction records */
            if (ExistsLaborDtlAction(Session.CompanyID, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq))
            {
                foreach (var laborDtlAction in this.SelectLaborDtlAction(Session.CompanyID, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq))
                {
                    var laborDtlActionRow = new LaborDtlActionRow();
                    BufferCopy.Copy(laborDtlAction, ref laborDtlActionRow);
                    laborDtlActionRow.RowMod = IceRow.ROWSTATE_UNCHANGED;
                    CurrentFullTableset.LaborDtlAction.Add(laborDtlActionRow);
                }
            }
            else
            {
                foreach (var jobOperAction in this.SelectJobOperAction(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq))
                {
                    var laborDtlAction = new LaborDtlActionRow();
                    laborDtlAction.Company = Session.CompanyID;
                    laborDtlAction.LaborHedSeq = ttLaborDtl.LaborHedSeq;
                    laborDtlAction.LaborDtlSeq = ttLaborDtl.LaborDtlSeq;
                    laborDtlAction.ActionSeq = jobOperAction.ActionSeq;
                    laborDtlAction.ActionDesc = jobOperAction.ActionDesc;
                    laborDtlAction.Required = jobOperAction.Required;
                    laborDtlAction.Completed = jobOperAction.Completed;
                    laborDtlAction.CompletedBy = jobOperAction.CompletedBy;
                    laborDtlAction.CompletedOn = jobOperAction.CompletedOn;
                    laborDtlAction.SysRowID = jobOperAction.SysRowID;
                    laborDtlAction.RowMod = IceRow.ROWSTATE_ADDED;
                    CurrentFullTableset.LaborDtlAction.Add(laborDtlAction);
                }
            }
        }

        partial void LaborDtlActionBeforeUpdate()
        {
            var laborDtl = ttLaborDtl;

            if (laborDtl == null)
            {
                laborDtl = (from row in CurrentFullTableset.LaborDtl
                            where row.Company.KeyEquals(Session.CompanyID) &&
                                  row.LaborHedSeq == ttLaborDtlAction.LaborHedSeq &&
                                  row.LaborDtlSeq == ttLaborDtlAction.LaborDtlSeq
                            select row).FirstOrDefault();
                if (laborDtl == null)
                {
                    throw new BLException(Strings.NoLaborDtlRecordAvailable, "LaborDtl");
                }
            }

            var jobOperAction = this.FindFirstJobOperAction(Session.CompanyID, laborDtl.JobNum, laborDtl.AssemblySeq, laborDtl.OprSeq, ttLaborDtlAction.ActionSeq);
            if (jobOperAction == null)
            {
                throw new BLException(Strings.JobOperActionNotFound);
            }
            jobOperAction.Completed = ttLaborDtlAction.Completed;
            if (ttLaborDtlAction.Completed)
            {
                ttLaborDtlAction.CompletedBy = laborDtl.EmployeeNum;
                ttLaborDtlAction.CompletedOn = CompanyTime.Now();
                jobOperAction.CompletedBy = laborDtl.EmployeeNum;
                jobOperAction.CompletedOn = CompanyTime.Now();
            }
            else
            {
                ttLaborDtlAction.CompletedBy = "";
                ttLaborDtlAction.CompletedOn = null;
                jobOperAction.CompletedBy = "";
                jobOperAction.CompletedOn = null;
            }


        }


        /// <summary>
        /// Call this procedure when LaborDtl.ClockInDate changes 
        /// </summary>
        /// <param name="ds">Epicor.Mfg.BO.LaborDataSet</param>    
        /// <param name="ipClockInDate">Proposed Clock In Date</param>
        public void OnChangeClockInDate(ref LaborTableset ds, DateTime? ipClockInDate)
        {
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }
            eadErrMsg = LibEADValidation.validateEAD(ipClockInDate, "IP", "");
            if (!String.IsNullOrEmpty(eadErrMsg))
            {
                throw new BLException(eadErrMsg);
            }
            if (ipClockInDate == null)
            {
                ttLaborDtl.ClockInDate = null;
            }
            else
            {
                ttLaborDtl.ClockInDate = ipClockInDate.Value.Date;
            }
        }

        /// <summary>
        /// This method validates the PCID
        /// </summary>
        /// <param name="pcid">PCID to validate</param>
        /// <param name="isNonConformance">Is this the NonConformance PCID</param>
        /// <param name="ds">Report Qty Dataset</param>
        public void OnChangePCID(string pcid, bool isNonConformance, ref LaborTableset ds)
        {
            CurrentFullTableset = ds;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            if (!string.IsNullOrEmpty(pcid))
            {
                using (ControlIDExtract libControlIDExtract = new ControlIDExtract(Db))
                {
                    pcid = libControlIDExtract.ExtractValueFromString_PCID(Session.CompanyID, pcid);

                    pcid = ValidatePCID(ttLaborDtl.Company, pcid, ttLaborDtl.PartNum, ttLaborDtl.LotNum, ttLaborDtl.LaborUOM, ttLaborDtl.JobNum, isNonConformance);
                }
            }

            if (!isNonConformance)
            {
                ttLaborDtl.PCID = pcid;
            }
            else
            {
                ttLaborDtl.NonConfPCID = pcid;
            }

            if (!string.IsNullOrEmpty(ttLaborDtl.PCID) || !string.IsNullOrEmpty(ttLaborDtl.NonConfPCID))
            {
                EnableLotAndGetDefault(ttLaborDtl.PartNum, ttLaborDtl.JobNum);
            }
            else
            {
                ttLaborDtl.LotNum = string.Empty;
                ttLaborDtl.EnableLot = false;
                ttLaborDtl.PrintPCIDContents = false;
            }


            if (!ttLaborDtl.EnableSN)
            {
                ttLaborDtl.EnableSN = enableSN(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.PCID, ttLaborDtl.PartNum);
                if (!ttLaborDtl.EnableSN)
                {
                    ttLaborDtl.EnableSN = enableSN(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.NonConfPCID, ttLaborDtl.PartNum);
                }
            }
        }

        /// <summary>
        /// This method validates field QuickEntryCode, and if it is valid, uses the 
        /// values from the QuickEntry record to populate the LaborDtl values.
        /// </summary>
        /// <param name="ipEmpID">Proposed EmpID value</param>
        /// <param name="ipQuickEntryCode">Proposed QuickEntryCode value</param>
        /// <param name="ds">The Labor data set </param>
        public void OnChangeQuickEntryCode(string ipEmpID, string ipQuickEntryCode, ref LaborTableset ds)
        {
            CurrentFullTableset = ds;
            string vMessage = string.Empty;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where !String.IsNullOrEmpty(ttLaborDtl_Row.RowMod)
                          select ttLaborDtl_Row).FirstOrDefault();

            ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                where !String.IsNullOrEmpty(ttTimeWeeklyView_Row.RowMod)
                                select ttTimeWeeklyView_Row).FirstOrDefault();
            if (ttLaborDtl == null && ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborNotFound, "LaborDtl", "RowMod");
            }
            if (ttLaborDtl != null)
            {

                QuickEntry = this.FindFirstQuickEntry(Session.CompanyID, ipEmpID, "TIME", ipQuickEntryCode);
                if (QuickEntry == null)
                {
                    throw new BLException(Strings.QuickEntryCodeDoesNotExist, "LaborDtl", "QuickEntryCode");
                }
                if (ttLaborDtl.AllowDirLbr == false && QuickEntry.LaborType.Compare("I") != 0 && QuickEntry.LaborType.Compare("J") != 0)
                {
                    throw new BLException(Strings.QuickEntryCodeMustHaveALaborTypeOfIndirOrService, "LaborDtl", "QuickEntryCode");
                }
                ttLaborDtl.QuickEntryCode = ipQuickEntryCode;
                ttLaborDtl.LaborType = (("P,J,V".Lookup(QuickEntry.LaborType) > -1) ? "P" : ((QuickEntry.LaborType.Compare("I") == 0) ? "I" : ((QuickEntry.LaborType.Compare("S") == 0) ? "S" : "")));
                ttLaborDtl.LaborTypePseudo = QuickEntry.LaborType;
                if (!String.IsNullOrEmpty(QuickEntry.IndirectCode))
                {
                    ttLaborDtl.IndirectCode = QuickEntry.IndirectCode;
                }

                if (!String.IsNullOrEmpty(ttLaborDtl.IndirectCode))
                {
                    this.ChangeIndirectCode(ref ds);
                }

                if (!String.IsNullOrEmpty(QuickEntry.ProjectID))
                {
                    ttLaborDtl.ProjectID = QuickEntry.ProjectID;
                }

                if (!String.IsNullOrEmpty(ttLaborDtl.ProjectID))
                {
                    this.DefaultProjectID(ref ds, ttLaborDtl.ProjectID);
                }

                if (!String.IsNullOrEmpty(QuickEntry.PhaseID))
                {
                    ttLaborDtl.PhaseID = QuickEntry.PhaseID;
                }

                if (!String.IsNullOrEmpty(ttLaborDtl.PhaseID))
                {
                    this.DefaultPhaseID(ref ds, ttLaborDtl.PhaseID);
                }

                if (!String.IsNullOrEmpty(QuickEntry.JobNum))
                {
                    ttLaborDtl.JobNum = QuickEntry.JobNum;
                }

                if (!String.IsNullOrEmpty(ttLaborDtl.JobNum))
                {
                    this.DefaultJobNum(ref ds, ttLaborDtl.JobNum);
                }

                if (QuickEntry.AssemblySeq != 0)
                {
                    ttLaborDtl.AssemblySeq = QuickEntry.AssemblySeq;
                }

                if (ttLaborDtl.AssemblySeq != 0)
                {
                    this.DefaultAssemblySeq(ref ds, ttLaborDtl.AssemblySeq);
                }

                if (QuickEntry.OprSeq != 0)
                {
                    ttLaborDtl.OprSeq = QuickEntry.OprSeq;
                }

                if (ttLaborDtl.OprSeq != 0)
                {
                    this.DefaultOprSeq(ref ds, ttLaborDtl.OprSeq, out vMessage);
                }

                if (!String.IsNullOrEmpty(QuickEntry.RoleCode))
                {
                    ttLaborDtl.RoleCd = QuickEntry.RoleCode;
                }

                if (!String.IsNullOrEmpty(ttLaborDtl.RoleCd) && !String.IsNullOrEmpty(ttLaborDtl.JobNum))
                {
                    this.DefaultRoleCd(ref ds, ttLaborDtl.RoleCd);
                }

                if (!String.IsNullOrEmpty(QuickEntry.TimeTypCd))
                {
                    ttLaborDtl.TimeTypCd = QuickEntry.TimeTypCd;
                }

                if (!String.IsNullOrEmpty(ttLaborDtl.TimeTypCd) && !String.IsNullOrEmpty(ttLaborDtl.RoleCd) && !String.IsNullOrEmpty(ttLaborDtl.JobNum))
                {
                    this.DefaultTimeTypCd(ref ds, ttLaborDtl.TimeTypCd, out vMessage);
                }

                if (!String.IsNullOrEmpty(QuickEntry.ResourceGrpID))
                {
                    this.Overrides(ref ds, ttLaborDtl.OpCode, QuickEntry.ResourceGrpID);
                }

                if (!String.IsNullOrEmpty(QuickEntry.ResourceID))
                {
                    this.DefaultResourceID(ref ds, QuickEntry.ResourceID);
                }

                if (!String.IsNullOrEmpty(QuickEntry.ExpenseCode))
                {
                    ttLaborDtl.ExpenseCode = QuickEntry.ExpenseCode;
                }

                if (QuickEntry.LaborHrs != 0)
                {
                    ttLaborDtl.LaborHrs = QuickEntry.LaborHrs;
                }

                LaborDtl_Foreign_Link();
            }
            if (ttTimeWeeklyView != null)
            {


                QuickEntry = this.FindFirstQuickEntry(Session.CompanyID, ipEmpID, "TIME", ipQuickEntryCode);
                if (QuickEntry == null)
                {
                    throw new BLException(Strings.QuickEntryCodeDoesNotExist, "LaborDtl", "QuickEntryCode");
                }
                if (ttTimeWeeklyView.AllowDirLbr == false && QuickEntry.LaborType.Compare("I") != 0 && QuickEntry.LaborType.Compare("J") != 0)
                {
                    throw new BLException(Strings.QuickEntryCodeMustHaveALaborTypeOfIndirOrService, "LaborDtl", "QuickEntryCode");
                }
                ttTimeWeeklyView.QuickEntryCode = ipQuickEntryCode;
                ttTimeWeeklyView.LaborType = (("P,J,V".Lookup(QuickEntry.LaborType) > -1) ? "P" : ((QuickEntry.LaborType.Compare("I") == 0) ? "I" : ((QuickEntry.LaborType.Compare("S") == 0) ? "S" : "")));
                ttTimeWeeklyView.LaborTypePseudo = QuickEntry.LaborType;
                ttTimeWeeklyView.ExpenseCode = QuickEntry.ExpenseCode;
                ttTimeWeeklyView.MessageText = "";
                if (!String.IsNullOrEmpty(QuickEntry.IndirectCode))
                {
                    ttTimeWeeklyView.IndirectCode = QuickEntry.IndirectCode;
                }

                if (!String.IsNullOrEmpty(ttTimeWeeklyView.IndirectCode))
                {
                    this.ChangeIndirectCode(ref ds);
                }

                if (!String.IsNullOrEmpty(QuickEntry.ProjectID))
                {
                    ttTimeWeeklyView.ProjectID = QuickEntry.ProjectID;
                }

                if (!String.IsNullOrEmpty(ttTimeWeeklyView.ProjectID))
                {
                    this.DefaultProjectID(ref ds, ttTimeWeeklyView.ProjectID);
                }

                if (!String.IsNullOrEmpty(QuickEntry.PhaseID))
                {
                    ttTimeWeeklyView.PhaseID = QuickEntry.PhaseID;
                }

                if (!String.IsNullOrEmpty(ttTimeWeeklyView.PhaseID))
                {
                    this.DefaultPhaseID(ref ds, ttTimeWeeklyView.PhaseID);
                }

                if (!String.IsNullOrEmpty(QuickEntry.JobNum))
                {
                    ttTimeWeeklyView.JobNum = QuickEntry.JobNum;
                }

                if (!String.IsNullOrEmpty(ttTimeWeeklyView.JobNum))
                {
                    this.DefaultJobNum(ref ds, ttTimeWeeklyView.JobNum);
                }

                if (QuickEntry.AssemblySeq != 0)
                {
                    ttTimeWeeklyView.AssemblySeq = QuickEntry.AssemblySeq;
                }

                if (ttTimeWeeklyView.AssemblySeq != 0)
                {
                    this.DefaultAssemblySeq(ref ds, ttTimeWeeklyView.AssemblySeq);
                }

                if (QuickEntry.OprSeq != 0)
                {
                    ttTimeWeeklyView.OprSeq = QuickEntry.OprSeq;
                }

                if (ttTimeWeeklyView.OprSeq != 0)
                {
                    this.DefaultOprSeq(ref ds, ttTimeWeeklyView.OprSeq, out vMessage);
                }

                if (!String.IsNullOrEmpty(QuickEntry.RoleCode))
                {
                    ttTimeWeeklyView.RoleCd = QuickEntry.RoleCode;
                }

                if (!String.IsNullOrEmpty(ttTimeWeeklyView.RoleCd))
                {
                    this.DefaultRoleCd(ref ds, ttTimeWeeklyView.RoleCd);
                }

                if (!String.IsNullOrEmpty(QuickEntry.TimeTypCd))
                {
                    ttTimeWeeklyView.TimeTypCd = QuickEntry.TimeTypCd;
                }

                if (!String.IsNullOrEmpty(ttTimeWeeklyView.TimeTypCd))
                {
                    this.DefaultTimeTypCd(ref ds, ttTimeWeeklyView.TimeTypCd, out vMessage);
                }

                if (!String.IsNullOrEmpty(QuickEntry.ResourceGrpID))
                {
                    ttTimeWeeklyView.ResourceGrpID = QuickEntry.ResourceGrpID;
                }

                if (!String.IsNullOrEmpty(ttTimeWeeklyView.ResourceGrpID))
                {
                    this.OnChangeResourceGrpID(ref ds, ttTimeWeeklyView.ResourceGrpID);
                }

                if (!String.IsNullOrEmpty(QuickEntry.ResourceID))
                {
                    ttTimeWeeklyView.ResourceID = QuickEntry.ResourceID;
                }

                if (!String.IsNullOrEmpty(ttTimeWeeklyView.ResourceID))
                {
                    this.DefaultResourceID(ref ds, ttTimeWeeklyView.ResourceID);
                }
                TimeWeeklyView_Foreign_Link();
            }
        }

        /// <summary>
        /// Call this procedure when TimeWeeklyView.ResourceGrpID changes 
        /// </summary>
        /// <param name="ds">Epicor.Mfg.BO.LaborDataSet</param>    
        /// <param name="ipResourceGrpID">Proposed Resource Group</param>
        public void OnChangeResourceGrpID(ref LaborTableset ds, string ipResourceGrpID)
        {
            CurrentFullTableset = ds;
            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborHasNotChanged, "LaborDtl");
            }


            ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, ipResourceGrpID);
            if (ResourceGroup == null)
            {
                throw new BLException(Strings.InvalidResourceGroup, "LaborDtl", "ResourceGrpID");
            }
            if (ResourceGroup.Location == false)
            {
                throw new BLException(Strings.TheResouGroupIsNotALocatAndMayNotBeSelec, "LaborDtl", "ResourceGrpID");
            }
            if (!String.IsNullOrEmpty(ipResourceGrpID) && ipResourceGrpID != null)
            {
                ttTimeWeeklyView.JCDept = ResourceGroup.JCDept;
                /* Default the first Resource when ResourceGrpID is changed  */
                if (!String.IsNullOrEmpty(ipResourceGrpID))
                {
                    ttTimeWeeklyView.ResourceID = "";
                    if (ttTimeWeeklyView.LaborType.Compare("I") == 0)
                    {


                        EmpBasic = this.FindFirstEmpBasic16(Session.CompanyID, ttTimeWeeklyView.EmployeeNum);
                        if (EmpBasic != null && EmpBasic.ResourceGrpID.Compare(ipResourceGrpID) == 0)
                        {
                            ttTimeWeeklyView.ResourceID = EmpBasic.ResourceID;
                        }

                        if (!String.IsNullOrEmpty(ttTimeWeeklyView.ResourceID))
                        {


                            Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, ttTimeWeeklyView.ResourceID);
                            if (Resource != null)
                            {
                                ttTimeWeeklyView.ResourceIDDescription = Resource.Description;
                            }
                        }
                    }
                    if (ttTimeWeeklyView.LaborType.Compare("I") != 0 || String.IsNullOrEmpty(ttTimeWeeklyView.ResourceID))
                    {


                        Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, ipResourceGrpID);
                        ttTimeWeeklyView.ResourceID = ((Resource != null) ? Resource.ResourceID : "");
                        ttTimeWeeklyView.ResourceIDDescription = ((Resource != null) ? Resource.Description : "");
                    }
                }/* if inResGrpID <> "" */
                ttTimeWeeklyView.ResourceGrpID = ipResourceGrpID;
            }
        }

        /// <summary>
        /// Call this method when loading end activity on Kinetic-MES.
        /// </summary>
        /// <param name="iLaborHedSeq">The LaborHedSeq of the LaborHed record to retrieve </param>
        /// <param name="iLaborDtlSeq">The LaborDtlSeq of the LaborDtl record to retrieve </param>
        public LaborTableset OnLoadEndActivity(int iLaborHedSeq, int iLaborDtlSeq)
        {
            Erp.Tablesets.LaborTableset ds = new LaborTableset();
            ds = GetDetail(iLaborHedSeq, iLaborDtlSeq);
            setRowMod(ds);
            EndActivity(ref ds);
            return CurrentFullTableset;
        }

        /// <summary>
        /// Call this procedure to override the Resource Group and Operation Code in a 
        /// job.
        /// </summary>
        /// <param name="ds">Epicor.Mfg.BO.LaborDataSet</param>    
        /// <param name="inOpCode"> OpCode to override </param>
        /// <param name="inResGrpID"> Resource Group OD to override </param>
        public void Overrides(ref LaborTableset ds, string inOpCode, string inResGrpID)
        {
            CurrentFullTableset = ds;
            decimal dSetupBurRate = decimal.Zero;
            decimal dProdBurRate = decimal.Zero;
            decimal dSetupLabRate = decimal.Zero;
            decimal dProdLabRate = decimal.Zero;
            string cBurdenType = string.Empty;
            decimal tempBurdenRate = decimal.Zero;
            string tmpResourceID = string.Empty;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            if (!String.IsNullOrEmpty(inResGrpID))
            {

                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, inResGrpID);
                if (ResourceGroup == null)
                {
                    throw new BLException(Strings.InvalidResourceGroup, "LaborDtl", "ResourceGrpID");
                }
                if (ResourceGroup.Location == false && ttLaborDtl.LaborType.Compare("I") != 0)
                {
                    throw new BLException(Strings.TheResouGroupIsNotALocatAndMayNotBeSelec, "LaborDtl", "ResourceGrpID");
                }

                ttLaborDtl.JCDept = ResourceGroup.JCDept;
                /* Default the first Resource when ResourceGrpID is changed  */
                if (!String.IsNullOrEmpty(inResGrpID))
                {
                    tmpResourceID = "";
                    if (ttLaborDtl.LaborType.Compare("I") == 0)
                    {


                        EmpBasic = this.FindFirstEmpBasic(Session.CompanyID, ttLaborDtl.EmployeeNum);
                        if (EmpBasic != null && EmpBasic.ResourceGrpID.Compare(inResGrpID) == 0)
                        {
                            tmpResourceID = EmpBasic.ResourceID;
                        }
                    }
                    Resource = null;
                    Resource = Resource.FindFirstByPrimaryKey(Db, ttLaborDtl.Company, (!String.IsNullOrEmpty(tmpResourceID)) ? tmpResourceID : inResGrpID);

                    /* if avail Resource */
                    if (Resource != null)
                    {


                        if (!String.IsNullOrEmpty(ttLaborDtl.CapabilityID) &&
                        !(this.ExistsCapResLnk(Session.CompanyID, ttLaborDtl.CapabilityID, Resource.ResourceID)))
                        {
                            throw new BLException(Strings.InvalidResourceForCapability, "LaborDtl", "ResourceID");
                        }/* if ttLaborDtl.CapabilityID <> '' and not available CapResLnk */
                        ttLaborDtl.ResourceID = Resource.ResourceID;
                        if (Resource.GetDefaultBurdenFromGroup == false)
                        {
                            dSetupBurRate = Resource.SetupBurRate;
                            dProdBurRate = Resource.ProdBurRate;
                            dSetupLabRate = Resource.SetupLabRate;
                            dProdLabRate = Resource.ProdLabRate;
                        }
                        else
                        {
                            dSetupBurRate = ResourceGroup.SetupBurRate;
                            dProdBurRate = ResourceGroup.ProdBurRate;
                            dSetupLabRate = ResourceGroup.SetupLabRate;
                            dProdLabRate = ResourceGroup.ProdLabRate;
                        }
                        ttLaborDtl.BurdenRate = 0;
                        /* setup */
                        if (ttLaborDtl.LaborType.Compare("S") == 0)
                        {
                            if (ttLaborDtl.LaborEntryMethod.Compare("Q") == 0 || ttLaborDtl.LaborEntryMethod.Compare("B") == 0)
                            {
                                ttLaborDtl.LaborRate = dSetupLabRate;
                            }

                            if (ResourceGroup.BurdenType.Compare("F") == 0)
                            {
                                ttLaborDtl.BurdenRate = dSetupBurRate;
                            }
                            else
                            {
                                ttLaborDtl.BurdenRate = ((dSetupBurRate / 100) * ttLaborDtl.LaborRate);
                            }
                        }
                        else
                        {
                            if (ttLaborDtl.LaborEntryMethod.Compare("Q") == 0 || ttLaborDtl.LaborEntryMethod.Compare("B") == 0)
                            {
                                ttLaborDtl.LaborRate = dProdLabRate;
                            }

                            if (ResourceGroup.BurdenType.Compare("F") == 0)
                            {
                                ttLaborDtl.BurdenRate = dProdBurRate;
                            }
                            else
                            {
                                ttLaborDtl.BurdenRate = ((dProdBurRate / 100) * ttLaborDtl.LaborRate);
                            }
                        }
                        /* SCR 94780 - Get the burden rate sum of all other OpDtl besides the Primary OpDtl *
                         * and add this to the burden rate derived from the Resource/ResourceGrp override.  *
                         * If the current combination of ResGrpID/ResourceID/CapID in LaborDtl has an exact *
                         * match in the JobOpDtl then it will be skipped in the sumAllBurdenCost logic.     *
                         * We only do this logic if the ApplyBurdenCost (PatchFld) flag is true.            */
                        if (vApplySumBurdenRates == true)
                        {
                            this.sumAllBurdenCost(ttLaborDtl.ResourceID, inResGrpID, ttLaborDtl.CapabilityID, out tempBurdenRate);
                            ttLaborDtl.BurdenRate = ttLaborDtl.BurdenRate + tempBurdenRate;
                        }
                        if (ttLaborDtl.BurdenRate < 0)
                        {
                            ttLaborDtl.BurdenRate = 0;
                        }
                    }
                    else
                    {
                        ttLaborDtl.ResourceID = "";
                    }
                }/* if inResGrpID <> "" */
                ttLaborDtl.ResourceGrpID = inResGrpID;
            }/* if inResGrp <> "" or inResGrp <> ? then */

            if (!String.IsNullOrEmpty(inOpCode))
            {
                ttLaborDtl.OpCode = inOpCode;
            }

            LaborDtl_Foreign_Link();
        }

        /// <summary>
        /// Call this procedure to override the Resource in a LaborDtl record
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="ProposedResourceID">The proposed resource id</param>
        public void OverridesResource(ref LaborTableset ds, string ProposedResourceID)
        {
            CurrentFullTableset = ds;
            decimal dSetupBurRate = decimal.Zero;
            decimal dProdBurRate = decimal.Zero;
            decimal dSetupLabRate = decimal.Zero;
            decimal dProdLabRate = decimal.Zero;
            string cBurdenType = string.Empty;
            decimal tempBurdenRate = decimal.Zero;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }
            /* end if ProposedResourceID <> "" and ProposedResourceID <> ? */
            if (!String.IsNullOrEmpty(ProposedResourceID))
            {


                Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, ProposedResourceID);
                if (Resource == null)
                {
                    throw new BLException(Strings.InvalidResource, "LaborDtl", "ResourceID");
                }



                if (!String.IsNullOrEmpty(ttLaborDtl.CapabilityID) &&
                !(this.ExistsCapResLnk2(Session.CompanyID, ttLaborDtl.CapabilityID, ProposedResourceID)))
                {
                    throw new BLException(Strings.InvalidResourceForCapability, "LaborDtl", "ResourceID");
                }
                ttLaborDtl.ResourceID = ProposedResourceID;
                ttLaborDtl.ResourceGrpID = Resource.ResourceGrpID;



                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, ttLaborDtl.ResourceGrpID);
                if (ResourceGroup != null)
                {
                    ttLaborDtl.JCDept = ResourceGroup.JCDept;
                    if (Resource != null && Resource.GetDefaultBurdenFromGroup == false)
                    {
                        dSetupBurRate = Resource.SetupBurRate;
                        dProdBurRate = Resource.ProdBurRate;
                        dSetupLabRate = Resource.SetupLabRate;
                        dProdLabRate = Resource.ProdLabRate;
                    }
                    else
                    {
                        dSetupBurRate = ResourceGroup.SetupBurRate;
                        dProdBurRate = ResourceGroup.ProdBurRate;
                        dSetupLabRate = ResourceGroup.SetupLabRate;
                        dProdLabRate = ResourceGroup.ProdLabRate;
                    }
                    ttLaborDtl.BurdenRate = 0;
                    /* setup */
                    if (ttLaborDtl.LaborType.Compare("S") == 0)
                    {
                        if (ttLaborDtl.LaborEntryMethod.Compare("Q") == 0 || ttLaborDtl.LaborEntryMethod.Compare("B") == 0)
                        {
                            ttLaborDtl.LaborRate = dSetupLabRate;
                        }

                        if (ResourceGroup.BurdenType.Compare("F") == 0)
                        {
                            ttLaborDtl.BurdenRate = dSetupBurRate;
                        }
                        else
                        {
                            ttLaborDtl.BurdenRate = ((dSetupBurRate / 100) * ttLaborDtl.LaborRate);
                        }
                    }
                    else
                    {
                        if (ttLaborDtl.LaborEntryMethod.Compare("Q") == 0 || ttLaborDtl.LaborEntryMethod.Compare("B") == 0)
                        {
                            ttLaborDtl.LaborRate = dProdLabRate;
                        }

                        if (ResourceGroup.BurdenType.Compare("F") == 0)
                        {
                            ttLaborDtl.BurdenRate = dProdBurRate;
                        }
                        else
                        {
                            ttLaborDtl.BurdenRate = ((dProdBurRate / 100) * ttLaborDtl.LaborRate);
                        }
                    }
                    if (ttLaborDtl.BurdenRate < 0)
                    {
                        ttLaborDtl.BurdenRate = 0;
                    }
                }
            }
            else
            {
                if (ProposedResourceID != null)
                {


                    ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, ttLaborDtl.ResourceGrpID);
                    if (ResourceGroup != null)
                    {
                        dSetupBurRate = ResourceGroup.SetupBurRate;
                        dProdBurRate = ResourceGroup.ProdBurRate;
                        dSetupLabRate = ResourceGroup.SetupLabRate;
                        dProdLabRate = ResourceGroup.ProdLabRate;
                        ttLaborDtl.BurdenRate = 0;
                        /* setup */
                        if (ttLaborDtl.LaborType.Compare("S") == 0)
                        {
                            if (ttLaborDtl.LaborEntryMethod.Compare("Q") == 0 || ttLaborDtl.LaborEntryMethod.Compare("B") == 0)
                            {
                                ttLaborDtl.LaborRate = dSetupLabRate;
                            }

                            if (ResourceGroup.BurdenType.Compare("F") == 0)
                            {
                                ttLaborDtl.BurdenRate = dSetupBurRate;
                            }
                            else
                            {
                                ttLaborDtl.BurdenRate = ((dSetupBurRate / 100) * ttLaborDtl.LaborRate);
                            }
                        }
                        else
                        {
                            if (ttLaborDtl.LaborEntryMethod.Compare("Q") == 0 || ttLaborDtl.LaborEntryMethod.Compare("B") == 0)
                            {
                                ttLaborDtl.LaborRate = dProdLabRate;
                            }

                            if (ResourceGroup.BurdenType.Compare("F") == 0)
                            {
                                ttLaborDtl.BurdenRate = dProdBurRate;
                            }
                            else
                            {
                                ttLaborDtl.BurdenRate = ((dProdBurRate / 100) * ttLaborDtl.LaborRate);
                            }
                        }
                    }
                    ttLaborDtl.ResourceID = ProposedResourceID;
                }
            }
            /* SCR 94780 - Get the burden rate sum of all other OpDtl besides the Primary OpDtl *
             * and add this to the burden rate derived from the Resource/ResourceGrp override.  *
             * If the current combination of ResGrpID/ResourceID/CapID in LaborDtl has an exact *
             * match in the JobOpDtl then it will be skipped in the sumAllBurdenCost logic.     *
             * We only do this logic if the ApplyBurdenCost (PatchFld) flag is true.            */
            if (vApplySumBurdenRates == true)
            {
                this.sumAllBurdenCost(ttLaborDtl.ResourceID, ttLaborDtl.ResourceGrpID, ttLaborDtl.CapabilityID, out tempBurdenRate);
                ttLaborDtl.BurdenRate = ttLaborDtl.BurdenRate + tempBurdenRate;
            }
            LaborDtl_Foreign_Link();
        }

        /// <summary>
        /// This method updates the LaborHed payroll hours
        /// </summary>
        private void payHours()
        {
            decimal wrkPayHours = decimal.Zero;
            decimal wrkLunchHours = decimal.Zero;
            decimal twrkPayHours = decimal.Zero;
            decimal TempClockInTime = decimal.Zero;
            decimal TempClockOutTime = decimal.Zero;
            TempClockInTime = Math.Truncate(ttLaborHed.ClockInTime) + Compatibility.Convert.ToInt32((ttLaborHed.ClockInTime - Math.Truncate(ttLaborHed.ClockInTime)) * 60) / 60m;
            TempClockOutTime = Math.Truncate(ttLaborHed.ClockOutTime) + Compatibility.Convert.ToInt32((ttLaborHed.ClockOutTime - Math.Truncate(ttLaborHed.ClockOutTime)) * 60) / 60m;

            if (ttLaborHed.ClockInTime > ttLaborHed.ClockOutTime)
            {
                wrkPayHours = (ttLaborHed.ClockOutTime + 24.00m) - ttLaborHed.ClockInTime;
                twrkPayHours = (TempClockOutTime + 24.00m) - TempClockInTime;
            }
            else
            {
                wrkPayHours = ttLaborHed.ClockOutTime - ttLaborHed.ClockInTime;
                twrkPayHours = TempClockOutTime - TempClockInTime;
            }

            if (ttLaborHed.LunchOutTime > ttLaborHed.LunchInTime)
            {
                if (ttLaborHed.LunchOutTime < ttLaborHed.ClockInTime)
                {
                    wrkLunchHours = (ttLaborHed.LunchInTime + 24.00m) - TempClockInTime;
                }
                else
                {
                    wrkLunchHours = (ttLaborHed.LunchInTime + 24.00m) - ttLaborHed.LunchOutTime;
                }
            }
            else
            {
                if (ttLaborHed.ClockInTime == ttLaborHed.ClockOutTime) // 24 hours 
                {
                    wrkLunchHours = ttLaborHed.LunchInTime - ttLaborHed.LunchOutTime;
                }
                else
                {
                    if (ttLaborHed.ClockOutTime <= ttLaborHed.LunchOutTime && ttLaborHed.ClockOutTime >= ttLaborHed.ClockInTime || ttLaborHed.LunchInTime == ttLaborHed.LunchOutTime || (ttLaborHed.LunchOutTime > ttLaborHed.ClockOutTime && ttLaborHed.ClockInTime > ttLaborHed.LunchInTime) || (ttLaborHed.LunchOutTime < ttLaborHed.ClockInTime && ttLaborHed.LunchInTime < ttLaborHed.ClockInTime && ttLaborHed.ClockOutTime > ttLaborHed.ClockInTime))

                    {
                        wrkLunchHours = 0;
                    }
                    else
                    {
                        if (ttLaborHed.ClockOutTime > ttLaborHed.LunchOutTime && ttLaborHed.ClockOutTime < ttLaborHed.LunchInTime)
                            wrkLunchHours = ttLaborHed.ClockOutTime - ttLaborHed.LunchOutTime;
                        else
                            wrkLunchHours = ttLaborHed.LunchInTime - ttLaborHed.LunchOutTime;
                    }
                }
                if (ttLaborHed.LunchOutTime != 0 && ttLaborHed.LunchOutTime < ttLaborHed.ClockInTime && ttLaborHed.ClockInTime < ttLaborHed.LunchInTime)
                {
                    wrkLunchHours = ttLaborHed.LunchInTime - ttLaborHed.ClockInTime;
                }
            }
            //24 hours shift
            if (twrkPayHours == 0)
                twrkPayHours = 24.00m;
            ttLaborHed.PayHours = Math.Max((twrkPayHours - wrkLunchHours), 0);
            ttLaborHed.DspPayHours = ttLaborHed.PayHours;
        }

        /// <summary>
        /// This method updates the labordtl payhours.
        /// </summary>
        /// <param name="vsetHrsOnly"> indicates to only set the labor/burden hrs </param>
        /// <param name="vUpdateLabandBurHrs"></param>
        /// <param name="vUseAltLaborHed">indicates if this method is to find the altlaborHed or if temp-table is being used</param>
        /// <param name="vMessage">Returns any messages the User needs to review</param>
        private void payHoursDtl(bool vsetHrsOnly, bool vUpdateLabandBurHrs, bool vUseAltLaborHed, out string vMessage)
        {
            vMessage = string.Empty;
            DateTime? lunchStartDate = null;
            DateTime? lunchEndDate = null;
            int tot_break_mins = 0;
            bool[] tt_brk_array;
            int baseLunchInMin = 0;
            int baseLunchOutMin = 0;
            int basePayMinutes = 0;
            int baseLunchMinutes = 0;
            decimal tmpClockIn = decimal.Zero;
            decimal tmpClockOut = decimal.Zero;
            decimal tmpLunchClockIn = decimal.Zero;
            decimal tmpLunchClockOut = decimal.Zero;
            DateTime? dtlClockOutDate = null;
            Erp.Tables.LaborHed altLaborHed = null;
            decimal vLunchOutTime = decimal.Zero;
            decimal vLunchInTime = decimal.Zero;
            decimal vActLunchOutTime = decimal.Zero;
            decimal vActLunchInTime = decimal.Zero;
            decimal vShiftOutTime = decimal.Zero;
            decimal vShiftInTime = decimal.Zero;
            int vShift = 0;
            bool vLunchFromHdr = false;

            vUpdateLabandBurHrs = vUpdateLabandBurHrs && !ttLaborDtl.Downtime;

            if (vUseAltLaborHed == true)
            {
                if (ttLaborDtl.LaborHedSeq != 0)
                {


                    altLaborHed = this.FindFirstLaborHed8(Session.CompanyID, ttLaborDtl.LaborHedSeq);
                    if (altLaborHed != null)
                    {
                        vLunchFromHdr = true;
                        vLunchOutTime = altLaborHed.LunchOutTime;
                        vLunchInTime = altLaborHed.LunchInTime;
                        vActLunchOutTime = altLaborHed.ActLunchOutTime;
                        vActLunchInTime = altLaborHed.ActLunchInTime;
                        vShift = altLaborHed.Shift;
                        vShiftInTime = altLaborHed.ClockInTime;
                        vShiftOutTime = altLaborHed.ClockOutTime;
                    }
                }
            }
            else
            {
                if (ttLaborHed != null)
                {
                    vLunchFromHdr = true;
                    vLunchOutTime = ttLaborHed.LunchOutTime;
                    vLunchInTime = ttLaborHed.LunchInTime;
                    vActLunchOutTime = ttLaborHed.ActLunchOutTime;
                    vActLunchInTime = ttLaborHed.ActLunchInTime;
                    vShift = ttLaborHed.Shift;
                    vShiftInTime = ttLaborHed.ClockInTime;
                    vShiftOutTime = ttLaborHed.ClockOutTime;
                }
            } /* vUseAltLaborHed = no */

            /* SCR 88840 - If in case the ttLaborDtl is being created without the LaborHed, we have to *
             * get the LunchIn/Out information directly from the Shift associated with the employee.   */
            if (vLunchFromHdr == false)
            {
                EmpBasic = this.FindFirstEmpBasic17(Session.CompanyID, ttLaborDtl.EmployeeNum);
                if (EmpBasic == null)
                {
                    return;
                }
                JCShift = this.FindFirstJCShift7(Session.CompanyID, EmpBasic.Shift);
                if (JCShift == null)
                {
                    return;
                }
                vLunchOutTime = JCShift.LunchStart;
                vLunchInTime = JCShift.LunchEnd;
                vActLunchOutTime = JCShift.LunchStart;
                vActLunchInTime = JCShift.LunchEnd;
                vShift = JCShift.Shift;
                vShiftInTime = JCShift.StartTime;
                vShiftOutTime = JCShift.EndTime;
            } /* vLunchFromHdr = no */
            /* SCR 88840 - if clockin time = 24 set to 0 since a start of midnight is always 0 */
            if (ttLaborDtl.ClockinTime == 24.0m)
            {
                ttLaborDtl.ClockinTime = 0;
            }

            dtlClockOutDate = ttLaborDtl.ClockInDate;
            tmpClockIn = Math.Truncate(ttLaborDtl.ClockinTime) + Compatibility.Convert.ToInt32((ttLaborDtl.ClockinTime - Math.Truncate(ttLaborDtl.ClockinTime)) * 60) / 60m;
            if (ttLaborDtl.ClockinTime > ttLaborDtl.ClockOutTime)
            {
                dtlClockOutDate = (DateTime)dtlClockOutDate.Value.AddDays(1); /* over midnight, advance day */
                tmpClockOut = (Math.Truncate(ttLaborDtl.ClockOutTime) + Compatibility.Convert.ToInt32((ttLaborDtl.ClockOutTime - Math.Truncate(ttLaborDtl.ClockOutTime)) * 60) / 60m) + 24m;
            }
            else
            {
                tmpClockOut = (Math.Truncate(ttLaborDtl.ClockOutTime) + Compatibility.Convert.ToInt32((ttLaborDtl.ClockOutTime - Math.Truncate(ttLaborDtl.ClockOutTime)) * 60) / 60m);
            }
            /* Calculate Base Time in minutes.. */
            ttLaborDtl.ClockInMInute = (((TimeSpan)(ttLaborDtl.ClockInDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(ttLaborDtl.ClockinTime * 60);
            ttLaborDtl.ClockOutMinute = (((TimeSpan)(dtlClockOutDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(ttLaborDtl.ClockOutTime * 60);
            lunchStartDate = ttLaborDtl.ClockInDate; /* default clockin date */
            lunchEndDate = ttLaborDtl.ClockInDate;
            tmpLunchClockOut = vLunchOutTime;
            tmpLunchClockIn = vLunchInTime;
            /* SCR 88840 - The only time LunchEndDate should be different from LunchStartDate is when lunch goes over midnight */
            if (vLunchOutTime > vLunchInTime)
            {
                lunchEndDate = (DateTime)lunchEndDate.Value.AddDays(1); /* over midnight, advance day */
                tmpLunchClockIn = vLunchInTime + 24;
            }
            /* SCR 88840 - We should change lunchStart/EndDate only if lunch is not over midnight.  *
             * When changing the date of lunchStartDate, we have to change the lunchEndDate as well *
             * to make sure these dates still agree.                                                */
            if (ttLaborDtl.ClockinTime > ttLaborDtl.ClockOutTime)
            {
                if ((vLunchOutTime < ttLaborDtl.ClockinTime) &&
                    (vLunchInTime <= ttLaborDtl.ClockinTime) &&
                    (vLunchOutTime <= vLunchInTime))
                {
                    lunchStartDate = ((DateTime)lunchStartDate.Value.AddDays(1));
                    tmpLunchClockOut = vLunchOutTime + 24;
                    lunchEndDate = ((DateTime)lunchEndDate.Value.AddDays(1));
                    tmpLunchClockIn = vLunchInTime + 24;
                }
            }
            baseLunchOutMin = (((TimeSpan)(lunchStartDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(vLunchOutTime * 60);
            baseLunchInMin = (((TimeSpan)(lunchEndDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(vLunchInTime * 60);
            if (ttLaborDtl.ClockOutMinute != ttLaborDtl.ClockInMInute)
            {
                /*clock in after lunch */
                if (ttLaborDtl.ClockInMInute > baseLunchInMin)
                { /* clock in before lunch */
                    baseLunchOutMin = 0;
                    baseLunchInMin = 0;
                    tmpLunchClockOut = 0;
                    tmpLunchClockIn = 0;
                }

                /* clock out before lunch */
                if (ttLaborDtl.ClockOutMinute < baseLunchOutMin)
                { /* clock in before lunch */
                    baseLunchOutMin = 0;
                    baseLunchInMin = 0;
                    tmpLunchClockOut = 0;
                    tmpLunchClockIn = 0;
                }
            }
            if (ttLaborDtl.ClockInMInute != ttLaborDtl.ClockOutMinute)
            {
                /* clock in in the middle of lunch */
                if ((ttLaborDtl.ClockInMInute >= baseLunchOutMin) &&
                    (ttLaborDtl.ClockInMInute <= baseLunchInMin))
                {
                    baseLunchOutMin = ttLaborDtl.ClockInMInute;
                    tmpLunchClockOut = tmpClockIn;
                }
                /* clock out in the middle of lunch */
                if ((ttLaborDtl.ClockOutMinute >= baseLunchOutMin) &&
                    (ttLaborDtl.ClockOutMinute <= baseLunchInMin))
                {
                    baseLunchInMin = ttLaborDtl.ClockOutMinute;
                    tmpLunchClockIn = tmpClockOut;
                }
            }
            basePayMinutes = (ttLaborDtl.ClockOutMinute - ttLaborDtl.ClockInMInute);
            baseLunchMinutes = (baseLunchInMin - baseLunchOutMin);           /*LaborDtl.LaborHrs = ((basePayMinutes / 60) - (baseLunchMinutes / 60))*/

            LibShiftBrk.GetTotalBreakMinutes(
                vShift,
                ttLaborDtl.ClockInDate,
                ttLaborDtl.ClockinTime,
                ttLaborDtl.ClockOutTime,
                true, false, true, true,
                vActLunchOutTime,
                vActLunchInTime,
                out tot_break_mins, out tt_brk_array);
            if (vUpdateLabandBurHrs == true)
            {

                /* SCR 156781 - If the Labor Method is Quantity Only and Production Standard is Fixed Hours then set the hours based on the *
                 * operation's estimated production hours. Otherwise, calculate hours based on the clock in/out times.                      */
                if (ttLaborDtl.ISFixHourAndQtyOnly == true && ttLaborDtl.OprSeq != 0)
                {
                    ttLaborDtl.LaborHrs = this.CalcProdFixedHours(ttLaborDtl.LaborType, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                    ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
                }
                else
                {
                    /* ERPS-213225 - check if valid to calculate 24 hours if clock in/out times are the same or from midnight to midnight */
                    if (tmpClockIn == tmpClockOut || tmpClockOut - tmpClockIn == 24.0m)
                    {
                        // if clock in = clock out that could mean either the shift is 24 hours
                        // or possibly we had previous hours booked. If previous hours and they equal
                        // we've used up the entire shift and therefore we set hours to zero
                        /* ERPS-213225 - Do not default 24 hours if the shift is not really 24 hours */
                        if (vShiftInTime == 24.0m)
                        {
                            vShiftInTime = 0;
                        }
                        if ((ttLaborDtl.LaborHedSeq != 0 && ExistsOtherLaborDtl(Session.CompanyID, ttLaborDtl.LaborHedSeq, ttLaborDtl.SysRowID)) ||
                            (vShiftOutTime - vShiftInTime != 0 && vShiftOutTime - vShiftInTime != 24.0m))
                        {
                            ttLaborDtl.LaborHrs = 0;
                            ttLaborDtl.BurdenHrs = 0;
                        }
                        else
                        {
                            /* ERPS-213225 - add the 24 hour adjustment only if clock in/out times are the same */
                            ttLaborDtl.LaborHrs = Math.Round(((tmpClockIn == tmpClockOut ? (tmpClockOut + 24.0m) : tmpClockOut) - tmpClockIn) - ((tmpLunchClockIn - tmpLunchClockOut) + Math.Round((decimal)tot_break_mins / 60, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero);
                            ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
                        }
                    }
                    else
                    {
                        ttLaborDtl.LaborHrs = Math.Round((tmpClockOut - tmpClockIn) - ((tmpLunchClockIn - tmpLunchClockOut) + Math.Round((decimal)tot_break_mins / 60, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero);
                        ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
                    }
                }
            }

            if (!vsetHrsOnly)
            {
                this.calcTotHrs(ttLaborDtl.LaborHrs);



                if (this.ExistJobOperForJobNumAssemblyOprSeq(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq))
                {
                    this.warnHrs(out vMessage);
                }
            }
        }

        private decimal CalcProdFixedHours(string ipLaborType, int ipLaborHedSeq, int ipLaborDtlSeq, string ipJobNum, int ipAsmSeq, int ipOprSeq)
        {
            decimal opFixedHours = decimal.Zero;
            decimal oldBurdenHrs = decimal.Zero;
            Erp.Tables.LaborDtl altLaborDtl;

            altLaborDtl = this.FindFirstLaborDtl(Session.CompanyID, ipLaborHedSeq, ipLaborDtlSeq);
            if (altLaborDtl != null)
            {
                oldBurdenHrs = ((!altLaborDtl.JobNum.KeyEquals(ipJobNum)) || altLaborDtl.AssemblySeq != ipAsmSeq || altLaborDtl.OprSeq != ipOprSeq) ? 0 : altLaborDtl.BurdenHrs;
            }
            if (ipLaborType.KeyEquals("P"))
            {
                opFixedHours = Math.Max((this.GetProdFixedHours(Session.CompanyID, ipJobNum, ipAsmSeq, ipOprSeq) + oldBurdenHrs), 0);
            }
            else if (ipLaborType.KeyEquals("S"))
            {
                opFixedHours = Math.Max((this.GetSetupFixedHours(Session.CompanyID, ipJobNum, ipAsmSeq, ipOprSeq) + oldBurdenHrs), 0);
            }
            return opFixedHours;
        }

        private void populateTimeValidateDates()
        {
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.TimeManagement) == false)
            {
                return;
            }

            if (ldCalendarStartDate == null || ldCalendarEndDate == null)
            {
                return;
            }
            /* if the start date that is passed to us is not a Sunday, back up to the Sunday of that week */
            /* In 9.05, weekday(Sunday) will give you 1 but DayOfWeek will give you 0, so we need to add 1 to result */
            if (Convert.ToInt32(((DateTime)ldCalendarStartDate).DayOfWeek + 1) != 1)
            {
                ldCalendarStartDate = ldCalendarStartDate.Value.AddDays(1 - Convert.ToInt32(((DateTime)ldCalendarStartDate).DayOfWeek + 1));
            }/* if the end date that is passed to us is not a Saturday, go forward to the Saturday of that week */
            if (Convert.ToInt32(((DateTime)ldCalendarEndDate).DayOfWeek + 1) != 7)
            {
                ldCalendarEndDate = ldCalendarEndDate.Value.AddDays(7 - Convert.ToInt32(((DateTime)ldCalendarEndDate).DayOfWeek + 1));
            }
        }

        private void populateTimeWeeklyAll()
        {
            DateTime? vCurrentWeekStartDate = null;
            DateTime? vCurrentWeekEndDate = null;
            string strPKey;


            dicTimeWeeklyView = new Dictionary<string, TimeWeeklyViewRow>(StringComparer.OrdinalIgnoreCase);

            //Process all Labors into period (not ActiveTrans, nonzero Labor Hrs), create Dictionary dicTimeWeeklyView 
            foreach (var LaborDtl_iterator in this.SelectLaborDtlPeriod(Session.CompanyID, lcEmployeeNum, ldCalendarStartDate, ldCalendarEndDate, false, 0))
            {
                LaborDtl = LaborDtl_iterator;

                if (LaborDtl.LaborEntryMethod.Equals("B", StringComparison.OrdinalIgnoreCase) && !getBackflushRecords) return;

                //ERPS-163044, Avoid creation Time Week if user has not access to labor 
                if (ErpCallContext.ContainsKey("GetRowsCalendarView"))
                {
                    ttLaborDtl = (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                                  where ttLaborDtl_Row.SysRowID == LaborDtl.SysRowID
                                  select ttLaborDtl_Row).FirstOrDefault();

                    if (ttLaborDtl == null || !ttLaborDtl.HasAccessToRow)
                    {
                        continue;
                    }
                }

                setWeekStartEndDates(LaborDtl.PayrollDate, out vCurrentWeekStartDate, out vCurrentWeekEndDate);

                strPKey = (new TimeWeekKey(LaborDtl, vCurrentWeekStartDate, vCurrentWeekEndDate)).PRKey;

                if (!dicTimeWeeklyView.TryGetValue(strPKey, out ttTimeWeeklyView))
                {
                    ttTimeWeeklyView = new Erp.Tablesets.TimeWeeklyViewRow();
                    dicTimeWeeklyView.Add(strPKey, ttTimeWeeklyView);
                    ttTimeWeekSetKeyFields(vCurrentWeekStartDate, vCurrentWeekEndDate);
                }
                ttTimeWeekSetOtherFields();
            }

            //Copy dicTimeWeeklyView -> TimeWeeklyView
            foreach (var ttTimeWeekly_iterator in dicTimeWeeklyView.Values)
            {
                ttTimeWeeklyView = ttTimeWeekly_iterator;
                CurrentFullTableset.TimeWeeklyView.Add(ttTimeWeeklyView);
            }
        }

        private void setWeekStartEndDates(DateTime? vCurrentDate, out DateTime? vCurrentWeekStartDate, out DateTime? vCurrentWeekEndDate)
        {
            vCurrentWeekStartDate = ((Convert.ToInt32(vCurrentDate.Value.DayOfWeek + 1) != 1) ? (vCurrentDate.Value.AddDays(1 - Convert.ToInt32(vCurrentDate.Value.DayOfWeek + 1))) : vCurrentDate);
            vCurrentWeekEndDate = ((Convert.ToInt32(vCurrentDate.Value.DayOfWeek + 1) != 7) ? (vCurrentDate.Value.AddDays(7 - Convert.ToInt32(vCurrentDate.Value.DayOfWeek + 1))) : vCurrentDate);
        }

        /// <summary>
        /// Populate the ttTimeWeeklyView table during a call to GetRows   
        /// </summary>
        private void populateTimeWeeklyView()
        {
            int i = 0;
            int j = 0;
            DateTime? vCurrentDate = null;
            DateTime? vCurrentWeekStartDate = null;
            DateTime? vCurrentWeekEndDate = null;
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.TimeManagement) == false)
            {
                return;
            }
            if (ldCalendarEndDate == null || ldCalendarStartDate == null)
            { return; }

            //If Empty then populate against the whole period
            if (!CurrentFullTableset.TimeWeeklyView.Any())
            {
                populateTimeWeeklyAll();
                return;
            }

            for (i = 1; i <= (Compatibility.Convert.ToInt32(((ldCalendarEndDate.Value - ldCalendarStartDate.Value).Days + 1) / 7m)); i++)
            {
                for (j = 1; j <= 7; j++)
                {
                    vCurrentDate = ldCalendarStartDate.Value.AddDays(((i - 1) * 7) + j - 1);
                    setWeekStartEndDates(vCurrentDate, out vCurrentWeekStartDate, out vCurrentWeekEndDate);


                    //Backflushed records are processed in the separate code block
                    foreach (var LaborDtl_iterator in (this.SelectLaborDtlExceptEntryMeth(Session.CompanyID, lcEmployeeNum, vCurrentDate, "B", 0)))
                    {
                        LaborDtl = LaborDtl_iterator;

                        ttTimeWeeklyView = (from ttTimeWeeklyView_Row in CurrentFullTableset.TimeWeeklyView
                                            where ttTimeWeeklyView_Row.Company.KeyEquals(Session.CompanyID) &&
                                                    ttTimeWeeklyView_Row.EmployeeNum.Compare(lcEmployeeNum) == 0 &&
                                                    ttTimeWeeklyView_Row.WeekBeginDate.Value.Date == vCurrentWeekStartDate.Value.Date &&
                                                    ttTimeWeeklyView_Row.WeekEndDate.Value.Date == vCurrentWeekEndDate.Value.Date &&
                                                    ttTimeWeeklyView_Row.LaborTypePseudo.Compare(LaborDtl.LaborTypePseudo) == 0 &&
                                                    ttTimeWeeklyView_Row.ProjectID.Compare(LaborDtl.ProjectID) == 0 &&
                                                    ttTimeWeeklyView_Row.PhaseID.Compare(LaborDtl.PhaseID) == 0 &&
                                                    ttTimeWeeklyView_Row.TimeTypCd.Compare(LaborDtl.TimeTypCd) == 0 &&
                                                    ttTimeWeeklyView_Row.JobNum.Compare(LaborDtl.JobNum) == 0 &&
                                                    ttTimeWeeklyView_Row.AssemblySeq == LaborDtl.AssemblySeq &&
                                                    ttTimeWeeklyView_Row.OprSeq == LaborDtl.OprSeq &&
                                                    ttTimeWeeklyView_Row.IndirectCode.Compare(LaborDtl.IndirectCode) == 0 &&
                                                    ttTimeWeeklyView_Row.RoleCd.Compare(LaborDtl.RoleCd) == 0 &&
                                                    ttTimeWeeklyView_Row.ResourceGrpID.Compare(LaborDtl.ResourceGrpID) == 0 &&
                                                    ttTimeWeeklyView_Row.ResourceID.Compare(LaborDtl.ResourceID) == 0 &&
                                                    ttTimeWeeklyView_Row.ExpenseCode.Compare(LaborDtl.ExpenseCode) == 0 &&
                                                    ttTimeWeeklyView_Row.Shift == LaborDtl.Shift &&
                                                    ttTimeWeeklyView_Row.TimeStatus.Compare(LaborDtl.TimeStatus) == 0 &&
                                                    ttTimeWeeklyView_Row.QuickEntryCode.Compare(LaborDtl.QuickEntryCode) == 0
                                            select ttTimeWeeklyView_Row).FirstOrDefault();
                        if (ttTimeWeeklyView == null)
                        {
                            ttTimeWeeklyView = new Erp.Tablesets.TimeWeeklyViewRow();
                            CurrentFullTableset.TimeWeeklyView.Add(ttTimeWeeklyView);
                            ttTimeWeekSetKeyFields(vCurrentWeekStartDate, vCurrentWeekEndDate);
                        }
                        ttTimeWeekSetOtherFields();
                    }

                    /*******************************PROCESS BACKFLUSHED RECORDS******************/
                    if (getBackflushRecords)
                    {
                        foreach (var LaborDtl_iterator in this.SelectLaborDtlEntryMeth(Session.CompanyID, lcEmployeeNum, vCurrentDate, "B", 0))
                        {
                            LaborDtl = LaborDtl_iterator;

                            ttTimeWeeklyView = (from ttTimeWeeklyView_Row in CurrentFullTableset.TimeWeeklyView
                                                where ttTimeWeeklyView_Row.Company.KeyEquals(Session.CompanyID) &&
                                                        ttTimeWeeklyView_Row.EmployeeNum.Compare(lcEmployeeNum) == 0 &&
                                                        ttTimeWeeklyView_Row.WeekBeginDate.Value.Date == vCurrentWeekStartDate.Value.Date &&
                                                        ttTimeWeeklyView_Row.WeekEndDate.Value.Date == vCurrentWeekEndDate.Value.Date &&
                                                        ttTimeWeeklyView_Row.LaborTypePseudo.Compare(LaborDtl.LaborTypePseudo) == 0 &&
                                                        ttTimeWeeklyView_Row.ProjectID.Compare(LaborDtl.ProjectID) == 0 &&
                                                        ttTimeWeeklyView_Row.PhaseID.Compare(LaborDtl.PhaseID) == 0 &&
                                                        ttTimeWeeklyView_Row.TimeTypCd.Compare(LaborDtl.TimeTypCd) == 0 &&
                                                        ttTimeWeeklyView_Row.JobNum.Compare(LaborDtl.JobNum) == 0 &&
                                                        ttTimeWeeklyView_Row.AssemblySeq == LaborDtl.AssemblySeq &&
                                                        ttTimeWeeklyView_Row.OprSeq == LaborDtl.OprSeq &&
                                                        ttTimeWeeklyView_Row.IndirectCode.Compare(LaborDtl.IndirectCode) == 0 &&
                                                        ttTimeWeeklyView_Row.RoleCd.Compare(LaborDtl.RoleCd) == 0 &&
                                                        ttTimeWeeklyView_Row.ResourceGrpID.Compare(LaborDtl.ResourceGrpID) == 0 &&
                                                        ttTimeWeeklyView_Row.ResourceID.Compare(LaborDtl.ResourceID) == 0 &&
                                                        ttTimeWeeklyView_Row.ExpenseCode.Compare(LaborDtl.ExpenseCode) == 0 &&
                                                        ttTimeWeeklyView_Row.Shift == LaborDtl.Shift &&
                                                        ttTimeWeeklyView_Row.TimeStatus.Compare(LaborDtl.TimeStatus) == 0 &&
                                                        ttTimeWeeklyView_Row.QuickEntryCode.Compare(LaborDtl.QuickEntryCode) == 0 &&
                                                        ttTimeWeeklyView_Row.LaborEntryMethod.Compare(LaborDtl.LaborEntryMethod) == 0
                                                select ttTimeWeeklyView_Row).FirstOrDefault();
                            if (ttTimeWeeklyView == null)
                            {
                                ttTimeWeeklyView = new Erp.Tablesets.TimeWeeklyViewRow();
                                CurrentFullTableset.TimeWeeklyView.Add(ttTimeWeeklyView);
                                ttTimeWeekSetKeyFields(vCurrentWeekStartDate, vCurrentWeekEndDate);
                            }
                            ttTimeWeekSetOtherFields();
                        }
                        /********************************************************************************/
                    }
                }
            }
        }

        private void ttTimeWeekSetKeyFields(DateTime? vCurrentWeekStartDate, DateTime? vCurrentWeekEndDate)
        {
            ttTimeWeeklyView.Company = Session.CompanyID;
            ttTimeWeeklyView.EmployeeNum = lcEmployeeNum;

            if (EmpBasic == null || !EmpBasic.EmpID.Equals(LaborDtl.EmployeeNum, StringComparison.OrdinalIgnoreCase))
            {
                EmpBasic = EmpBasic.FindFirstByPrimaryKey(Db, Session.CompanyID, LaborDtl.EmployeeNum);
            }

            ttTimeWeeklyView.Shift = ((EmpBasic != null) ? EmpBasic.Shift : 0);
            if (vCurrentWeekStartDate == null)
            {
                ttTimeWeeklyView.WeekBeginDate = null;
            }
            else
            {
                ttTimeWeeklyView.WeekBeginDate = vCurrentWeekStartDate.Value.Date;
            }

            if (vCurrentWeekEndDate == null)
            {
                ttTimeWeeklyView.WeekEndDate = null;
            }
            else
            {
                ttTimeWeeklyView.WeekEndDate = vCurrentWeekEndDate.Value.Date;
            }

            ttTimeWeeklyView.LaborType = LaborDtl.LaborType;
            ttTimeWeeklyView.LaborTypePseudo = LaborDtl.LaborTypePseudo;
            ttTimeWeeklyView.ProjectID = LaborDtl.ProjectID;
            if (!string.IsNullOrEmpty(ttTimeWeeklyView.ProjectID))
                ttTimeWeeklyView.ProjDesc = this.FindFirstProjectDescription(Session.CompanyID, ttTimeWeeklyView.ProjectID) ?? string.Empty;
            ttTimeWeeklyView.PhaseID = LaborDtl.PhaseID;
            ttTimeWeeklyView.TimeTypCd = LaborDtl.TimeTypCd;
            ttTimeWeeklyView.JobNum = LaborDtl.JobNum;
            ttTimeWeeklyView.AssemblySeq = LaborDtl.AssemblySeq;
            ttTimeWeeklyView.OprSeq = LaborDtl.OprSeq;
            ttTimeWeeklyView.IndirectCode = LaborDtl.IndirectCode;
            ttTimeWeeklyView.RoleCd = LaborDtl.RoleCd;
            ttTimeWeeklyView.ExpenseCode = LaborDtl.ExpenseCode;
            ttTimeWeeklyView.Shift = LaborDtl.Shift;
            ttTimeWeeklyView.Complete = LaborDtl.Complete;
            ttTimeWeeklyView.ResourceGrpID = LaborDtl.ResourceGrpID;
            ttTimeWeeklyView.ResourceID = LaborDtl.ResourceID;
            ttTimeWeeklyView.OpCode = LaborDtl.OpCode;
            ttTimeWeeklyView.OpComplete = LaborDtl.OpComplete;
            ttTimeWeeklyView.LaborEntryMethod = LaborDtl.LaborEntryMethod;
            ttTimeWeeklyView.LaborRate = LaborDtl.LaborRate;
            ttTimeWeeklyView.WeekDisplayText = vCurrentWeekStartDate.ToShortDateString() + " - " + vCurrentWeekEndDate.ToShortDateString();
            ttTimeWeeklyView.TimeStatus = LaborDtl.TimeStatus;
            ttTimeWeeklyView.QuickEntryCode = LaborDtl.QuickEntryCode;
            ttTimeWeeklyView.SysRowID = Guid.NewGuid();

            TimeWeeklyView_Foreign_Link();
        }

        private void ttTimeWeekSetOtherFields()
        {

            bool olApprover = false;
            string ocSalesRepCode = string.Empty;
            bool dummyTimeDisableDelete = false;
            bool supervisorHasRights = false;
            bool useEmployeeRules = false;
            bool useApproverRules = false;
            bool disallowTimeEntry = false;
            int setType = 0;
            bool userCanUpdate = false;
            bool bNotSubmitted = false;

            switch (Convert.ToInt32(LaborDtl.PayrollDate.Value.DayOfWeek + 1))
            {
                case 1:
                    {
                        ttTimeWeeklyView.HoursSun = ttTimeWeeklyView.HoursSun + LaborDtl.LaborHrs;
                    }
                    break;
                case 2:
                    {
                        ttTimeWeeklyView.HoursMon = ttTimeWeeklyView.HoursMon + LaborDtl.LaborHrs;
                    }
                    break;
                case 3:
                    {
                        ttTimeWeeklyView.HoursTue = ttTimeWeeklyView.HoursTue + LaborDtl.LaborHrs;
                    }
                    break;
                case 4:
                    {
                        ttTimeWeeklyView.HoursWed = ttTimeWeeklyView.HoursWed + LaborDtl.LaborHrs;
                    }
                    break;
                case 5:
                    {
                        ttTimeWeeklyView.HoursThu = ttTimeWeeklyView.HoursThu + LaborDtl.LaborHrs;
                    }
                    break;
                case 6:
                    {
                        ttTimeWeeklyView.HoursFri = ttTimeWeeklyView.HoursFri + LaborDtl.LaborHrs;
                    }
                    break;
                case 7:
                    {
                        ttTimeWeeklyView.HoursSat = ttTimeWeeklyView.HoursSat + LaborDtl.LaborHrs;
                    }
                    break;
            }
            ttTimeWeeklyView.HoursTotal = ttTimeWeeklyView.HoursTotal + LaborDtl.LaborHrs;
            ttTimeWeeklyView.HCMTotWeeklyPayHours = ttTimeWeeklyView.HCMTotWeeklyPayHours + LaborDtl.HCMPayHours;
            ttTimeWeeklyView.MessageText = "";
            ttTimeWeeklyView.RowSelected = false;

            if (!LaborDtl.LaborEntryMethod.Equals("B", StringComparison.OrdinalIgnoreCase))
            {
                bNotSubmitted = (String.IsNullOrEmpty(LaborDtl.TimeStatus) ||
                                 LaborDtl.TimeStatus.Compare("E") == 0 ||
                                 LaborDtl.TimeStatus.Compare("R") == 0);

                olApprover = this.canApprove(LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq);


                if (EmpBasic == null || !EmpBasic.EmpID.Equals(LaborDtl.EmployeeNum, StringComparison.OrdinalIgnoreCase))
                {
                    EmpBasic = EmpBasic.FindFirstByPrimaryKey(Db, Session.CompanyID, LaborDtl.EmployeeNum);
                }

                /*available EmpBasic and EmpBasic.DCDUserID = DCD-USERID*/
                if (EmpBasic != null && EmpBasic.DcdUserID.KeyEquals(Session.UserID))
                {
                    if (EmpBasic.DisallowTimeEntry == false)
                    {
                        useEmployeeRules = true;
                        useApproverRules = olApprover;
                    }
                    else
                    {
                        useEmployeeRules = false;
                        useApproverRules = false;
                        disallowTimeEntry = true;
                    }
                }
                else if (EmpBasic != null && !EmpBasic.DcdUserID.KeyEquals(Session.UserID))
                {


                    if (PlantConfCtrl == null || !PlantConfCtrl.Plant.Equals(Session.PlantID, StringComparison.OrdinalIgnoreCase))
                    {
                        PlantConfCtrl = PlantConfCtrl.FindFirstByPrimaryKey(Db, Session.CompanyID, Session.PlantID);
                    }
                    if (PlantConfCtrl != null)
                    {
                        if (PlantConfCtrl.TimeRestrictEntry == false)
                        {
                            if (olApprover == false)
                            {
                                useEmployeeRules = true;
                            }
                            else
                            {
                                useApproverRules = true;
                            }
                        }
                        else
                        {
                            supervisorHasRights = this.getSupervisorRights(LaborDtl.EmployeeNum);
                            if (supervisorHasRights || this.CanUserUpdateTime(Session.CompanyID, Session.UserID, true))
                            {
                                userCanUpdate = true;
                            }
                            else
                            {
                                userCanUpdate = false;
                            }

                            if (userCanUpdate == true && olApprover == false)
                            {
                                useEmployeeRules = true;
                            }
                            else if (userCanUpdate == false && olApprover == true)
                            {
                                useApproverRules = true;
                            }
                            else
                            {
                                useApproverRules = true;
                                useEmployeeRules = true;
                            }
                        }
                    }
                }/* else if available EmpBasic and EmpBasic.DCDUserID <> DCD-UserID */

                setType = SetRuleTypeForLabor(useEmployeeRules, useApproverRules, disallowTimeEntry);
                var outEnableCopy2 = ttTimeWeeklyView.EnableCopy;
                var outEnableSubmit2 = ttTimeWeeklyView.EnableSubmit;
                var outEnableRecall2 = ttTimeWeeklyView.EnableRecall;
                var outTimeDisableUpdate2 = ttTimeWeeklyView.TimeDisableUpdate;
                this.setUpdateRules(setType, LaborDtl.JobNum, bNotSubmitted, LaborDtl.TimeStatus, LaborDtl.ApprovalRequired, LaborDtl.WipPosted, out outEnableCopy2, out outEnableSubmit2, out outEnableRecall2, out outTimeDisableUpdate2, out dummyTimeDisableDelete);
                ttTimeWeeklyView.EnableCopy = outEnableCopy2;
                ttTimeWeeklyView.EnableSubmit = outEnableSubmit2;
                ttTimeWeeklyView.EnableRecall = outEnableRecall2;
                ttTimeWeeklyView.TimeDisableUpdate = outTimeDisableUpdate2;
                ttTimeWeeklyView.TimeAutoSubmit = this.IsTimeAutoSubmitPlantConfCtrl(Session.CompanyID, Session.PlantID);

                ttTimeWeeklyView.RoleCdList = getRoleCdList(ttTimeWeeklyView.EmployeeNum,
                                                        ttTimeWeeklyView.ProjectID,
                                                        ttTimeWeeklyView.PhaseID,
                                                        ttTimeWeeklyView.JobNum,
                                                        ttTimeWeeklyView.AssemblySeq,
                                                        ttTimeWeeklyView.OprSeq);

                ttTimeWeeklyView = null;
            }
            else // BackFlush
            {
                ttTimeWeeklyView.EnableCopy = false;
                ttTimeWeeklyView.EnableSubmit = false;
                ttTimeWeeklyView.EnableRecall = false;
                ttTimeWeeklyView.TimeDisableUpdate = true;
                ttTimeWeeklyView.TimeAutoSubmit = false;

                ttTimeWeeklyView.RoleCdList = getRoleCdList(ttTimeWeeklyView.EmployeeNum,
                                                        ttTimeWeeklyView.ProjectID,
                                                        ttTimeWeeklyView.PhaseID,
                                                        ttTimeWeeklyView.JobNum,
                                                        ttTimeWeeklyView.AssemblySeq,
                                                        ttTimeWeeklyView.OprSeq);
            }
        }

        /// <summary>
        /// Populate the ttTimeWorkHours table during a call to GetRows   
        /// </summary>
        private void populateTimeWorkHours()
        {
            int h = 0;
            int i = 0;
            int j = 0;
            string[] lcCalendarID = new string[7];
            decimal[] vBookedHours = new decimal[7];
            decimal[] vWorkHours = new decimal[7];
            DateTime? vCurrentDate = null;
            string resourceCalProdHourField = string.Empty;
            string prodCalDayProdHourField = string.Empty;
            string prodCalHourField = string.Empty;

            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.TimeManagement) == false)
            {
                return;
            }

            if (ldCalendarStartDate == null)
            {
                return;
            }



            EmpBasic = this.FindFirstEmpBasic20(Session.CompanyID, lcEmployeeNum);
            if (EmpBasic == null)
            {
                return;
            }

            for (h = 1; h <= (Compatibility.Convert.ToInt32(((ldCalendarEndDate.Value - ldCalendarStartDate.Value).Days + 1) / 7m)); h++)
            {
                Array.Clear(vWorkHours, 0, 7);
                Array.Clear(vBookedHours, 0, 7);
                for (i = 1; i <= 7; i++)
                {
                    vCurrentDate = (ldCalendarStartDate.Value.AddDays(((h - 1) * 7) + i - 1));      /* set work hours */

                    //CALENDARBLOCK:
                    do
                    {


                        EmpCal = this.FindLastEmpCal(Session.CompanyID, EmpBasic.EmpID, vCurrentDate.Value, null, vCurrentDate);
                        if (EmpCal == null)
                        {


                            EmpCal = this.FindLastEmpCal(Session.CompanyID, EmpBasic.EmpID, vCurrentDate.Value, null);
                        }
                        if (EmpCal != null)
                        {


                            ProdCal = this.FindFirstProdCal(Session.CompanyID, EmpCal.CalendarID);
                            if (ProdCal != null && !String.IsNullOrEmpty(ProdCal.CalendarID))
                            {
                                lcCalendarID[i - 1] = ProdCal.CalendarID;
                                break;
                            }
                        }
                        if (!String.IsNullOrEmpty(EmpBasic.ResourceID))
                        {


                            Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, EmpBasic.ResourceID);
                            if (Resource != null && !String.IsNullOrEmpty(Resource.CalendarID))
                            {
                                lcCalendarID[i - 1] = Resource.CalendarID;
                                break;
                            }
                        }
                        if (!String.IsNullOrEmpty(EmpBasic.ResourceGrpID))
                        {


                            ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, EmpBasic.ResourceGrpID);
                            if (ResourceGroup != null && !String.IsNullOrEmpty(ResourceGroup.CalendarID))
                            {
                                lcCalendarID[i - 1] = ResourceGroup.CalendarID;
                                break;
                            }
                        }


                        Plant = this.FindFirstPlant(Session.CompanyID, Session.PlantID);
                        if (Plant != null && !String.IsNullOrEmpty(Plant.CalendarID))
                        {
                            lcCalendarID[i - 1] = Plant.CalendarID;
                            break;
                        }


                        Company = this.FindFirstCompany(Session.CompanyID);
                        if (Company != null && !String.IsNullOrEmpty(Company.CalendarID))
                        {
                            lcCalendarID[i - 1] = Company.CalendarID;
                            break;
                        }
                    }
                    while (false);



                    ProdCal = this.FindFirstProdCal2(Session.CompanyID, lcCalendarID[i - 1]);
                    if (ProdCal == null)
                    {
                        return;
                    }



                    ResourceCal = this.FindFirstResourceCal(EmpBasic.Company, EmpBasic.EmpID, EmpBasic.EmpID, vCurrentDate.Value);
                    if (ResourceCal != null)
                    {
                        for (j = 1; j <= 24; j++)
                        {
                            resourceCalProdHourField = "ProdHour" + j.ToString().PadLeft(2, '0');
                            if (Convert.ToBoolean(ResourceCal[resourceCalProdHourField].ToString()) == true)
                            {
                                vWorkHours[i - 1] = vWorkHours[i - 1] + 1;
                            }
                        }
                    }
                    else
                    {


                        ProdCalDay = this.FindFirstProdCalDay(Session.CompanyID, lcCalendarID[i - 1], vCurrentDate.Value);
                        if (ProdCalDay != null)
                        {
                            if (ProdCalDay.WorkingDay)
                            {
                                for (j = 1; j <= 24; j++)
                                {
                                    prodCalDayProdHourField = "ProdHour" + j.ToString().PadLeft(2, '0');
                                    if (Convert.ToBoolean(ProdCalDay[prodCalDayProdHourField].ToString()) == true)
                                    {
                                        vWorkHours[i - 1] = vWorkHours[i - 1] + 1;
                                    }
                                }
                            }
                        }
                        else if (ProdCal != null)
                        {
                            for (j = 1; j <= 24; j++)
                            {
                                prodCalHourField = "Hour" + (j + (Convert.ToInt32(vCurrentDate.Value.DayOfWeek) * 24)).ToString().PadLeft(3, '0');
                                if (Convert.ToBoolean(ProdCal[prodCalHourField].ToString()) == true)
                                {
                                    vWorkHours[i - 1] = vWorkHours[i - 1] + 1;
                                }
                            }
                        }
                    }
                    /* set booked hours */



                    foreach (var LaborDtl_iterator in (this.SelectLaborDtl(Session.CompanyID, vCurrentDate, EmpBasic.EmpID)))
                    {
                        LaborDtl = LaborDtl_iterator;
                        if (LaborDtl.LaborHrs != 0)
                        {
                            vBookedHours[i - 1] = vBookedHours[i - 1] + LaborDtl.LaborHrs;
                        }
                    }
                }
                ttTimeWorkHours = new Erp.Tablesets.TimeWorkHoursRow();
                CurrentFullTableset.TimeWorkHours.Add(ttTimeWorkHours);
                ttTimeWorkHours.Company = Session.CompanyID;

                ttTimeWorkHours.WeekBeginDate = (ldCalendarStartDate.Value.AddDays((h - 1) * 7)).Date;
                ttTimeWorkHours.WeekEndDate = ttTimeWorkHours.WeekBeginDate.Value.AddDays(6);
                ttTimeWorkHours.SunDisplayDate = Compatibility.Convert.ToString(ttTimeWorkHours.WeekBeginDate.Value.Date);
                ttTimeWorkHours.MonDisplayDate = Compatibility.Convert.ToString((ttTimeWorkHours.WeekBeginDate.Value.AddDays(1)).Date);
                ttTimeWorkHours.TueDisplayDate = Compatibility.Convert.ToString((ttTimeWorkHours.WeekBeginDate.Value.AddDays(2)).Date);
                ttTimeWorkHours.WedDisplayDate = Compatibility.Convert.ToString((ttTimeWorkHours.WeekBeginDate.Value.AddDays(3)).Date);
                ttTimeWorkHours.ThuDisplayDate = Compatibility.Convert.ToString((ttTimeWorkHours.WeekBeginDate.Value.AddDays(4)).Date);
                ttTimeWorkHours.FriDisplayDate = Compatibility.Convert.ToString((ttTimeWorkHours.WeekBeginDate.Value.AddDays(5)).Date);
                ttTimeWorkHours.SatDisplayDate = Compatibility.Convert.ToString((ttTimeWorkHours.WeekBeginDate.Value.AddDays(6)).Date);
                ttTimeWorkHours.WeekDisplayText = ttTimeWorkHours.WeekBeginDate.ToShortDateString() + " - " + ttTimeWorkHours.WeekEndDate.ToShortDateString();
                ttTimeWorkHours.SunWorkHours = vWorkHours[0];
                ttTimeWorkHours.MonWorkHours = vWorkHours[1];
                ttTimeWorkHours.TueWorkHours = vWorkHours[2];
                ttTimeWorkHours.WedWorkHours = vWorkHours[3];
                ttTimeWorkHours.ThuWorkHours = vWorkHours[4];
                ttTimeWorkHours.FriWorkHours = vWorkHours[5];
                ttTimeWorkHours.SatWorkHours = vWorkHours[6];
                ttTimeWorkHours.SunBookedHours = vBookedHours[0];
                ttTimeWorkHours.MonBookedHours = vBookedHours[1];
                ttTimeWorkHours.TueBookedHours = vBookedHours[2];
                ttTimeWorkHours.WedBookedHours = vBookedHours[3];
                ttTimeWorkHours.ThuBookedHours = vBookedHours[4];
                ttTimeWorkHours.FriBookedHours = vBookedHours[5];
                ttTimeWorkHours.SatBookedHours = vBookedHours[6];
                ttTimeWorkHours.SunDiffHours = ttTimeWorkHours.SunWorkHours - ttTimeWorkHours.SunBookedHours;
                ttTimeWorkHours.MonDiffHours = ttTimeWorkHours.MonWorkHours - ttTimeWorkHours.MonBookedHours;
                ttTimeWorkHours.TueDiffHours = ttTimeWorkHours.TueWorkHours - ttTimeWorkHours.TueBookedHours;
                ttTimeWorkHours.WedDiffHours = ttTimeWorkHours.WedWorkHours - ttTimeWorkHours.WedBookedHours;
                ttTimeWorkHours.ThuDiffHours = ttTimeWorkHours.ThuWorkHours - ttTimeWorkHours.ThuBookedHours;
                ttTimeWorkHours.FriDiffHours = ttTimeWorkHours.FriWorkHours - ttTimeWorkHours.FriBookedHours;
                ttTimeWorkHours.SatDiffHours = ttTimeWorkHours.SatWorkHours - ttTimeWorkHours.SatBookedHours;
                ttTimeWorkHours.EmployeeNum = lcEmployeeNum;
                ttTimeWorkHours.CalendarDescription = ((ProdCal != null) ? ProdCal.Description : "");
                ttTimeWorkHours.TotalWorkHours = ttTimeWorkHours.SunWorkHours + ttTimeWorkHours.MonWorkHours + ttTimeWorkHours.TueWorkHours + ttTimeWorkHours.WedWorkHours + ttTimeWorkHours.ThuWorkHours + ttTimeWorkHours.FriWorkHours + ttTimeWorkHours.SatWorkHours;
                ttTimeWorkHours.TotalBookedHours = ttTimeWorkHours.SunBookedHours + ttTimeWorkHours.MonBookedHours + ttTimeWorkHours.TueBookedHours + ttTimeWorkHours.WedBookedHours + ttTimeWorkHours.ThuBookedHours + ttTimeWorkHours.FriBookedHours + ttTimeWorkHours.SatBookedHours;
                ttTimeWorkHours.TotalDiffHours = ttTimeWorkHours.SunDiffHours + ttTimeWorkHours.MonDiffHours + ttTimeWorkHours.TueDiffHours + ttTimeWorkHours.WedDiffHours + ttTimeWorkHours.ThuDiffHours + ttTimeWorkHours.FriDiffHours + ttTimeWorkHours.SatDiffHours;
                ttTimeWorkHours.SysRowID = Guid.NewGuid();
            }
        }

        private void processTimeWeeklyView()
        {
            LaborTableset LaborDS = CurrentFullTableset;
            bool vAmbiguousLaborDtl = false;
            int k = 0;
            DateTime? vCurrentDate = null;
            DateTime? vCurrentWeekStartDate = null;
            DateTime? vCurrentWeekEndDate = null;
            decimal[] vActualHours = new decimal[7];
            decimal[] vProposedHours = new decimal[7];
            decimal[] vDiffHours = new decimal[7];
            string vMessage = string.Empty;
            Erp.Tablesets.TimeWeeklyViewRow bttTimeWeeklyView = null;


            //bttTimeWeeklyViewloop:
            foreach (var bttTimeWeeklyView_iterator in (from bttTimeWeeklyView_Row in CurrentFullTableset.TimeWeeklyView
                                                        where !String.IsNullOrEmpty(bttTimeWeeklyView_Row.RowMod)
                                                        select bttTimeWeeklyView_Row).ToList())
            {
                bttTimeWeeklyView = bttTimeWeeklyView_iterator;
                if (bttTimeWeeklyView.TimeStatus.Compare("R") == 0)
                {


                    AltbttTimeWeeklyView = (from AltbttTimeWeeklyView_Row in CurrentFullTableset.TimeWeeklyView
                                            where String.IsNullOrEmpty(AltbttTimeWeeklyView_Row.RowMod)
                                            select AltbttTimeWeeklyView_Row).FirstOrDefault();
                    if (AltbttTimeWeeklyView != null)
                    {
                        if ((AltbttTimeWeeklyView.HoursSun == 0 && bttTimeWeeklyView.HoursSun != 0) ||
                           (AltbttTimeWeeklyView.HoursMon == 0 && bttTimeWeeklyView.HoursMon != 0) ||
                           (AltbttTimeWeeklyView.HoursTue == 0 && bttTimeWeeklyView.HoursTue != 0) ||
                           (AltbttTimeWeeklyView.HoursWed == 0 && bttTimeWeeklyView.HoursWed != 0) ||
                           (AltbttTimeWeeklyView.HoursThu == 0 && bttTimeWeeklyView.HoursThu != 0) ||
                           (AltbttTimeWeeklyView.HoursFri == 0 && bttTimeWeeklyView.HoursFri != 0) ||
                           (AltbttTimeWeeklyView.HoursSat == 0 && bttTimeWeeklyView.HoursSat != 0))
                        {
                            BufferCopy.CopyExceptFor(AltbttTimeWeeklyView, bttTimeWeeklyView, "RowMod");
                            throw new BLException(Strings.CanTAddTimeToARowWithARejecStatus, "LaborDtl");
                        }
                    }
                }
                /*****************************************************************************************/
                /* if this row was copied from another row and submitted with no actual hours, we are    */
                /* going to ignore it because if they want to clear out the hours they will need to do   */
                /* so with the original record.  we don't want them to hit the copy button, then the     */
                /* save button and have it wipe out all the labor from the original record inadvertently */
                /*****************************************************************************************/
                if (((!bttTimeWeeklyView.DeleteRow || ("C,A".Lookup(bttTimeWeeklyView.NewRowType) > -1)) && bttTimeWeeklyView.HoursSun == 0 && bttTimeWeeklyView.HoursMon == 0 && bttTimeWeeklyView.HoursTue == 0 &&
                bttTimeWeeklyView.HoursWed == 0 && bttTimeWeeklyView.HoursThu == 0 && bttTimeWeeklyView.HoursFri == 0 && bttTimeWeeklyView.HoursSat == 0))
                {
                    throw new BLException(Strings.HoursForAtLeastOneDayOfTheWeekMustBeEnteredToSave, "LaborDtl");
                }
                if ("E,R".Lookup(bttTimeWeeklyView.TimeStatus) == -1)
                {
                    this.populateTimeWeeklyView();
                    throw new BLException(Strings.CanOnlyModifyTimeThatHasnTBeenSubmi, "LaborDtl");
                }
                bttTimeWeeklyView.LaborType = (("P,J,V".Lookup(bttTimeWeeklyView.LaborTypePseudo) > -1) ? "P" : ((bttTimeWeeklyView.LaborTypePseudo.Compare("I") == 0) ? "I" : ((bttTimeWeeklyView.LaborTypePseudo.Compare("S") == 0) ? "S" : "")));

                //CurrentDateLoop:
                for (j = 1; j <= 7; j++)
                {
                    vCurrentDate = (bttTimeWeeklyView.WeekBeginDate.Value.AddDays(j - 1));
                    vCurrentWeekStartDate = ((Convert.ToInt32(vCurrentDate.Value.DayOfWeek + 1) != 1) ? (vCurrentDate.Value.AddDays(1 - Convert.ToInt32(vCurrentDate.Value.DayOfWeek + 1))) : vCurrentDate);
                    vCurrentWeekEndDate = ((Convert.ToInt32(vCurrentDate.Value.DayOfWeek + 1) != 7) ? (vCurrentDate.Value.AddDays(7 - Convert.ToInt32(vCurrentDate.Value.DayOfWeek + 1))) : vCurrentDate);

                    for (k = 0; k < 7; k++)
                    {
                        vProposedHours[k] = ((j == 1) ? bttTimeWeeklyView.HoursSun : ((j == 2) ? bttTimeWeeklyView.HoursMon : ((j == 3) ? bttTimeWeeklyView.HoursTue : ((j == 4) ? bttTimeWeeklyView.HoursWed : ((j == 5) ? bttTimeWeeklyView.HoursThu : ((j == 6) ? bttTimeWeeklyView.HoursFri : ((j == 7) ? bttTimeWeeklyView.HoursSat : 0)))))));
                    }

                    eadErrMsg = LibEADValidation.validateEAD(vCurrentDate, "IP", "");
                    if (vProposedHours[j - 1] > 0 && !String.IsNullOrEmpty(eadErrMsg))
                    {
                        throw new BLException(Strings.TimeWeeklyFieldWithCorrectYourWeeklyTimeBefore(eadErrMsg), "ttLaborDtl");
                    }
                    /* delete weekly */
                    if ((bttTimeWeeklyView.HoursSun == 0 && bttTimeWeeklyView.HoursMon == 0 &&
                         bttTimeWeeklyView.HoursTue == 0 && bttTimeWeeklyView.HoursWed == 0 &&
                         bttTimeWeeklyView.HoursThu == 0 && bttTimeWeeklyView.HoursFri == 0 &&
                         bttTimeWeeklyView.HoursSat == 0))
                    {
                        bttTimeWeeklyView.RowMod = "D";


                        //foreach (var LaborDtl_iterator in (this.SelectLaborDtl(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.LaborType, bttTimeWeeklyView.LaborTypePseudo, bttTimeWeeklyView.ProjectID, bttTimeWeeklyView.PhaseID, bttTimeWeeklyView.TimeTypCd, bttTimeWeeklyView.JobNum, bttTimeWeeklyView.AssemblySeq, bttTimeWeeklyView.OprSeq, bttTimeWeeklyView.IndirectCode, bttTimeWeeklyView.RoleCd, bttTimeWeeklyView.ResourceGrpID, bttTimeWeeklyView.ResourceID, bttTimeWeeklyView.ExpenseCode, bttTimeWeeklyView.Shift, bttTimeWeeklyView.QuickEntryCode, bttTimeWeeklyView.TimeStatus)))
                        /* For some reasons, the query with more than 16 parameters does not retrieve the records correctly using structured parameter list. Using workaround to pass 14 parameters and filter within the foreach */
                        foreach (var LaborDtl_iterator in (this.SelectLaborDtlWithUpdLock(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.LaborType, bttTimeWeeklyView.LaborTypePseudo, bttTimeWeeklyView.ProjectID, bttTimeWeeklyView.PhaseID, bttTimeWeeklyView.TimeTypCd, bttTimeWeeklyView.JobNum, bttTimeWeeklyView.AssemblySeq, bttTimeWeeklyView.OprSeq, bttTimeWeeklyView.IndirectCode, bttTimeWeeklyView.RoleCd, bttTimeWeeklyView.ResourceGrpID)))
                        {
                            LaborDtl = LaborDtl_iterator;
                            if (!LaborDtl.ResourceID.KeyEquals(bttTimeWeeklyView.ResourceID) ||
                                !LaborDtl.ExpenseCode.KeyEquals(bttTimeWeeklyView.ExpenseCode) ||
                                LaborDtl.Shift != bttTimeWeeklyView.Shift ||
                                !LaborDtl.QuickEntryCode.KeyEquals(bttTimeWeeklyView.QuickEntryCode) ||
                                !LaborDtl.TimeStatus.KeyEquals(bttTimeWeeklyView.TimeStatus))
                            { continue; }
                            bDelttLaborDtl = null;
                            if (bDelttLaborDtlRows != null)
                            {
                                bDelttLaborDtl = (from bDelttLaborDtl_Row in bDelttLaborDtlRows
                                                  where bDelttLaborDtl_Row.Company.KeyEquals(Session.CompanyID) &&
                                                  bDelttLaborDtl_Row.EmployeeNum.KeyEquals(bttTimeWeeklyView.EmployeeNum) &&
                                                  bDelttLaborDtl_Row.PayrollDate.Value.Date == vCurrentDate.Value.Date &&
                                                  bDelttLaborDtl_Row.LaborType.KeyEquals(bttTimeWeeklyView.LaborType) &&
                                                  bDelttLaborDtl_Row.LaborTypePseudo.KeyEquals(bttTimeWeeklyView.LaborTypePseudo) &&
                                                  bDelttLaborDtl_Row.ProjectID.KeyEquals(bttTimeWeeklyView.ProjectID) &&
                                                  bDelttLaborDtl_Row.PhaseID.KeyEquals(bttTimeWeeklyView.PhaseID) &&
                                                  bDelttLaborDtl_Row.TimeTypCd.KeyEquals(bttTimeWeeklyView.TimeTypCd) &&
                                                  bDelttLaborDtl_Row.JobNum.KeyEquals(bttTimeWeeklyView.JobNum) &&
                                                  bDelttLaborDtl_Row.AssemblySeq == bttTimeWeeklyView.AssemblySeq &&
                                                  bDelttLaborDtl_Row.OprSeq == bttTimeWeeklyView.OprSeq &&
                                                  bDelttLaborDtl_Row.IndirectCode.KeyEquals(bttTimeWeeklyView.IndirectCode) &&
                                                  bDelttLaborDtl_Row.RoleCd.KeyEquals(bttTimeWeeklyView.RoleCd) &&
                                                  bDelttLaborDtl_Row.ResourceGrpID.KeyEquals(bttTimeWeeklyView.ResourceGrpID) &&
                                                  bDelttLaborDtl_Row.ResourceID.KeyEquals(bttTimeWeeklyView.ResourceID) &&
                                                  bDelttLaborDtl_Row.ExpenseCode.KeyEquals(bttTimeWeeklyView.ExpenseCode) &&
                                                  bDelttLaborDtl_Row.Shift == bttTimeWeeklyView.Shift &&
                                                  bDelttLaborDtl_Row.QuickEntryCode.KeyEquals(bttTimeWeeklyView.QuickEntryCode) &&
                                                  bDelttLaborDtl_Row.TimeStatus.KeyEquals(bttTimeWeeklyView.TimeStatus)
                                                  select bDelttLaborDtl_Row).FirstOrDefault();
                            }
                            else bDelttLaborDtlRows = new List<LaborDtlRow>();
                            if (bDelttLaborDtl == null)
                            {
                                BufferCopy.Copy(LaborDtl, ref bDelttLaborDtl);
                            }
                            BufferCopy.Copy(LaborDtl, ref ttLaborDtl);
                            tmpLaborHedSeq = LaborDtl.LaborHedSeq;
                            this.LaborDtlBeforeDelete();
                            Db.LaborDtl.Delete(LaborDtl);
                        }
                        /* cleanup and zeros */

                        foreach (var LaborDtl_iterator in (this.SelectLaborDtlWithUpdLock(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, 0, bttTimeWeeklyView.TimeStatus)))
                        {
                            LaborDtl = LaborDtl_iterator;
                            bDelttLaborDtl = null;
                            if (bDelttLaborDtlRows != null)
                            {
                                bDelttLaborDtl = (from bDelttLaborDtl_Row in bDelttLaborDtlRows
                                                  where bDelttLaborDtl_Row.Company.KeyEquals(Session.CompanyID) &&
                                                  bDelttLaborDtl_Row.EmployeeNum.KeyEquals(bttTimeWeeklyView.EmployeeNum) &&
                                                  bDelttLaborDtl_Row.PayrollDate.Value.Date == vCurrentDate.Value.Date &&
                                                  bDelttLaborDtl_Row.LaborType.KeyEquals(bttTimeWeeklyView.LaborType) &&
                                                  bDelttLaborDtl_Row.LaborTypePseudo.KeyEquals(bttTimeWeeklyView.LaborTypePseudo) &&
                                                  bDelttLaborDtl_Row.ProjectID.KeyEquals(bttTimeWeeklyView.ProjectID) &&
                                                  bDelttLaborDtl_Row.PhaseID.KeyEquals(bttTimeWeeklyView.PhaseID) &&
                                                  bDelttLaborDtl_Row.TimeTypCd.KeyEquals(bttTimeWeeklyView.TimeTypCd) &&
                                                  bDelttLaborDtl_Row.JobNum.KeyEquals(bttTimeWeeklyView.JobNum) &&
                                                  bDelttLaborDtl_Row.AssemblySeq == bttTimeWeeklyView.AssemblySeq &&
                                                  bDelttLaborDtl_Row.OprSeq == bttTimeWeeklyView.OprSeq &&
                                                  bDelttLaborDtl_Row.IndirectCode.KeyEquals(bttTimeWeeklyView.IndirectCode) &&
                                                  bDelttLaborDtl_Row.RoleCd.KeyEquals(bttTimeWeeklyView.RoleCd) &&
                                                  bDelttLaborDtl_Row.ResourceGrpID.KeyEquals(bttTimeWeeklyView.ResourceGrpID) &&
                                                  bDelttLaborDtl_Row.ResourceID.KeyEquals(bttTimeWeeklyView.ResourceID) &&
                                                  bDelttLaborDtl_Row.ExpenseCode.KeyEquals(bttTimeWeeklyView.ExpenseCode) &&
                                                  bDelttLaborDtl_Row.Shift == bttTimeWeeklyView.Shift &&
                                                  bDelttLaborDtl_Row.QuickEntryCode.KeyEquals(bttTimeWeeklyView.QuickEntryCode) &&
                                                  bDelttLaborDtl_Row.TimeStatus.KeyEquals(bttTimeWeeklyView.TimeStatus)
                                                  select bDelttLaborDtl_Row).FirstOrDefault();
                            }
                            else bDelttLaborDtlRows = new List<LaborDtlRow>();
                            if (bDelttLaborDtl == null)
                            {
                                BufferCopy.Copy(LaborDtl, ref bDelttLaborDtl);
                            }
                            BufferCopy.Copy(LaborDtl, ref ttLaborDtl);
                            tmpLaborHedSeq = LaborDtl.LaborHedSeq;
                            this.LaborDtlBeforeDelete();
                            Db.LaborDtl.Delete(LaborDtl);
                        }
                        continue;
                    }/* end delete */
                    /* ignore if proposed hours are zero and copy */
                    if (vProposedHours[j - 1] == 0 && bttTimeWeeklyView.NewRowType.Compare("C") == 0)
                    {
                        continue;
                    }

                    if (vProposedHours[j - 1] != 0)
                    {

                        LaborHed = this.FindFirstLaborHed(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.Shift);
                        if (LaborHed != null && ExistsLaborHedPosted(Session.CompanyID, LaborHed.LaborHedSeq))
                        {
                            if (ExistsLaborHedNotPosted(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, LaborHed.LaborHedSeq))
                                LaborHed = this.FindFirstLaborHedNotPosted(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.Shift);
                            else
                                LaborHed = null;
                        }
                        if (LaborHed == null && bttTimeWeeklyView.TimeStatus.Compare("R") != 0)
                        {
                            ttLaborHed = new Erp.Tablesets.LaborHedRow();
                            CurrentFullTableset.LaborHed.Add(ttLaborHed);
                            ttLaborHed.Company = Session.CompanyID;
                            ttLaborHed.LaborHedSeq = LibNextValue.GetNextSequence("LaborHedSeq");
                            ttLaborHed.EmployeeNum = bttTimeWeeklyView.EmployeeNum;
                            ttLaborHed.Shift = bttTimeWeeklyView.Shift;
                            if (vCurrentDate == null)
                            { ttLaborHed.PayrollDate = null; }
                            else
                            { ttLaborHed.PayrollDate = vCurrentDate.Value.Date; }

                            if (vCurrentDate == null)
                            { ttLaborHed.ClockInDate = null; }
                            else
                            { ttLaborHed.ClockInDate = vCurrentDate.Value.Date; }

                            if (vCurrentDate == null)
                            { ttLaborHed.ActualClockinDate = null; }
                            else
                            { ttLaborHed.ActualClockinDate = vCurrentDate.Value.Date; }

                            LaborHedAfterGetNew1(false);
                            if (vCurrentDate == null)
                            { ttLaborHed.PayrollDate = null; }
                            else
                            { ttLaborHed.PayrollDate = vCurrentDate.Value.Date; }

                            ttLaborHed.PayrollDateNav = ttLaborHed.PayrollDate;

                            if (vCurrentDate == null)
                            { ttLaborHed.ClockInDate = null; }
                            else
                            { ttLaborHed.ClockInDate = vCurrentDate.Value.Date; }

                            if (vCurrentDate == null)
                            { ttLaborHed.ActualClockinDate = null; }
                            else
                            { ttLaborHed.ActualClockinDate = vCurrentDate.Value.Date; }

                            LaborHed = new Erp.Tables.LaborHed();
                            Db.LaborHed.Insert(LaborHed);
                            BufferCopy.Copy(ttLaborHed, ref LaborHed);
                            Db.Validate(LaborHed);
                            this.LaborHedAfterGetRows();
                            LaborHed_Foreign_Link();
                            BufferCopy.Copy(LaborHed, ref ttLaborHed);
                            ttLaborHed.SysRowID = Guid.NewGuid();
                            ttLaborHed.SysRowID = LaborHed.SysRowID;
                        }
                    }



                    vActualHours[Convert.ToInt32(vCurrentDate.Value.DayOfWeek)] = decimal.Zero;

                    //foreach (var LaborDtl_iterator in (this.SelectLaborDtl2(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.LaborType, bttTimeWeeklyView.LaborTypePseudo, bttTimeWeeklyView.ProjectID, bttTimeWeeklyView.PhaseID, bttTimeWeeklyView.TimeTypCd, bttTimeWeeklyView.JobNum, bttTimeWeeklyView.AssemblySeq, bttTimeWeeklyView.OprSeq, bttTimeWeeklyView.IndirectCode, bttTimeWeeklyView.RoleCd, bttTimeWeeklyView.ResourceGrpID, bttTimeWeeklyView.ResourceID, bttTimeWeeklyView.ExpenseCode, bttTimeWeeklyView.Shift, bttTimeWeeklyView.TimeStatus, bttTimeWeeklyView.QuickEntryCode)))
                    /* For some reasons, the query with more than 16 parameters does not retrieve the records correctly using structured parameter list. Using workaround to pass 14 parameters and filter within the foreach */
                    foreach (var LaborDtl_iterator in (this.SelectLaborDtl2(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.LaborType, bttTimeWeeklyView.LaborTypePseudo, bttTimeWeeklyView.ProjectID, bttTimeWeeklyView.PhaseID, bttTimeWeeklyView.TimeTypCd, bttTimeWeeklyView.JobNum, bttTimeWeeklyView.AssemblySeq, bttTimeWeeklyView.OprSeq, bttTimeWeeklyView.IndirectCode, bttTimeWeeklyView.RoleCd, bttTimeWeeklyView.ResourceGrpID)))
                    {
                        LaborDtl = LaborDtl_iterator;
                        if (!LaborDtl.ResourceID.KeyEquals(bttTimeWeeklyView.ResourceID) ||
                            !LaborDtl.ExpenseCode.KeyEquals(bttTimeWeeklyView.ExpenseCode) ||
                            LaborDtl.Shift != bttTimeWeeklyView.Shift ||
                            !LaborDtl.QuickEntryCode.KeyEquals(bttTimeWeeklyView.QuickEntryCode) ||
                            !LaborDtl.TimeStatus.KeyEquals(bttTimeWeeklyView.TimeStatus))
                        { continue; }
                        if (LaborDtl.LaborHrs != 0)
                        {
                            /* DayOfWeek(Sunday) = 0 while original ABL equivalent of Weekday(Sunday) = 1; no need to subtract 1 to match array element */
                            vActualHours[Convert.ToInt32(LaborDtl.PayrollDate.Value.DayOfWeek)] = vActualHours[Convert.ToInt32(LaborDtl.PayrollDate.Value.DayOfWeek)] + LaborDtl.LaborHrs;
                        }
                    }               /* Increasing Labor Hours - create a new ttLaborDtl */
                    /* Add Time */
                    /* Setting hours to zero */
                    if (vProposedHours[j - 1] > vActualHours[j - 1] || (("C,A".Lookup(bttTimeWeeklyView.NewRowType) > -1) && vProposedHours[j - 1] != 0))
                    {
                        if (("C,A".Lookup(bttTimeWeeklyView.NewRowType) == -1))
                        {
                            //LaborDtl = this.FindFirstLaborDtlWithUpdLock(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.LaborType, bttTimeWeeklyView.LaborTypePseudo, bttTimeWeeklyView.ProjectID, bttTimeWeeklyView.PhaseID, bttTimeWeeklyView.TimeTypCd, bttTimeWeeklyView.JobNum, bttTimeWeeklyView.AssemblySeq, bttTimeWeeklyView.OprSeq, bttTimeWeeklyView.IndirectCode, bttTimeWeeklyView.RoleCd, bttTimeWeeklyView.ResourceGrpID, bttTimeWeeklyView.ResourceID, bttTimeWeeklyView.ExpenseCode, bttTimeWeeklyView.QuickEntryCode, bttTimeWeeklyView.Shift, bttTimeWeeklyView.TimeStatus);
                            LaborDtl = null;
                            vAmbiguousLaborDtl = false;
                            /* For some reasons, the query with more than 16 parameters does not retrieve the records correctly using structured parameter list. Using workaround to pass 14 parameters and filter within the foreach */
                            foreach (var LaborDtl_iterator in (this.SelectLaborDtlWithUpdLock4(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.LaborType, bttTimeWeeklyView.LaborTypePseudo, bttTimeWeeklyView.ProjectID, bttTimeWeeklyView.PhaseID, bttTimeWeeklyView.TimeTypCd, bttTimeWeeklyView.JobNum, bttTimeWeeklyView.AssemblySeq, bttTimeWeeklyView.OprSeq, bttTimeWeeklyView.IndirectCode, bttTimeWeeklyView.RoleCd, bttTimeWeeklyView.ResourceGrpID)))
                            {
                                if (!LaborDtl_iterator.ResourceID.KeyEquals(bttTimeWeeklyView.ResourceID) ||
                                    !LaborDtl_iterator.ExpenseCode.KeyEquals(bttTimeWeeklyView.ExpenseCode) ||
                                    LaborDtl_iterator.Shift != bttTimeWeeklyView.Shift ||
                                    !LaborDtl_iterator.QuickEntryCode.KeyEquals(bttTimeWeeklyView.QuickEntryCode) ||
                                    !LaborDtl_iterator.TimeStatus.KeyEquals(bttTimeWeeklyView.TimeStatus))
                                { continue; }
                                // workaround for ambiguous check with more than 16 parameters 
                                if (LaborDtl != null)
                                {
                                    vAmbiguousLaborDtl = true;
                                    break;
                                }
                                LaborDtl = LaborDtl_iterator;
                            }               /* Increasing Labor Hours - create a new ttLaborDtl */
                            if (vAmbiguousLaborDtl)
                            {
                                vMessage = Strings.TheLaborHoursYouHaveSelecUpdatedForAreComprOfMulti(vCurrentDate);
                                throw new BLException(vMessage, "LaborDtl");
                            }
                        }
                        /* added */
                        if ((LaborDtl == null && bttTimeWeeklyView.TimeStatus.Compare("R") != 0) ||
                            ("C,A".Lookup(bttTimeWeeklyView.NewRowType) > -1))
                        {
                            ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
                            CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
                            ttLaborDtl.Company = Session.CompanyID;
                            ttLaborDtl.LaborHedSeq = LaborHed.LaborHedSeq;
                            ttLaborDtl.LaborDtlSeq = 0;
                            ttLaborDtl.EmployeeNum = LaborHed.EmployeeNum;
                            BufferCopy.Copy(bttTimeWeeklyView, ref ttLaborDtl);
                            if (ttLaborDtl.JobNum != null) validateJob(ttLaborDtl.JobNum, true, true);
                            if (vCurrentDate == null)
                            {
                                ttLaborDtl.PayrollDate = null;
                            }
                            else
                            {
                                ttLaborDtl.PayrollDate = vCurrentDate.Value.Date;
                            }

                            if (vCurrentDate == null)
                            {
                                ttLaborDtl.ClockInDate = null;
                            }
                            else
                            {
                                ttLaborDtl.ClockInDate = vCurrentDate.Value.Date;
                            }

                            ttLaborDtl.LaborHrs = vProposedHours[j - 1] - vActualHours[j - 1];
                            ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
                            ttLaborDtl.RowMod = "A";
                            if (this.isHCMEnabledAt(ttLaborDtl.EmployeeNum).Equals("DTL", StringComparison.OrdinalIgnoreCase))
                            {
                                ttLaborDtl.HCMPayHours = ((ttLaborDtl.LaborHrs >= 0) ? ttLaborDtl.LaborHrs : 0);
                            }
                            this.LaborDtlSetDefaults(ttLaborDtl);                   /* chgWcCode has to run after laborDtlSetDefaults so we have a rate */
                            if (!String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID))
                            {
                                this.chgWcCode(ttLaborDtl.ResourceGrpID, false, ref vMessage);
                            }
                            BufferCopy.CopyExceptFor(bttTimeWeeklyView, ttLaborDtl, "LaborRate", "BillServiceRate", "JCDept");
                            if (vCurrentDate == null)
                            {
                                ttLaborDtl.PayrollDate = null;
                            }
                            else
                            {
                                ttLaborDtl.PayrollDate = vCurrentDate.Value.Date;
                            }

                            if (vCurrentDate == null)
                            {
                                ttLaborDtl.ClockInDate = null;
                            }
                            else
                            {
                                ttLaborDtl.ClockInDate = vCurrentDate.Value.Date;
                            }

                            ttLaborDtl.LaborHrs = vProposedHours[j - 1];
                            ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
                            ttLaborDtl.RowMod = "A";
                            /* project */
                            if (ttLaborDtl.LaborTypePseudo.Compare("J") == 0)
                            {
                                ttLaborDtl.PhaseJobNum = ttLaborDtl.JobNum;
                                ttLaborDtl.PhaseOprSeq = ttLaborDtl.OprSeq;
                            }
                        }
                        else if (LaborDtl != null)
                        {
                            LaborDtl.LaborHrs = LaborDtl.LaborHrs + (vProposedHours[j - 1] - vActualHours[j - 1]);
                            LaborDtl.BurdenHrs = LaborDtl.LaborHrs;
                            this.refreshTtLaborDtl(LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq);
                            this.LaborDtlBeforeUpdate();
                        }
                        bttTimeWeeklyView.HoursTotal = bttTimeWeeklyView.HoursTotal + (vProposedHours[j - 1] - vActualHours[j - 1]);
                        if (!String.IsNullOrEmpty(bttTimeWeeklyView.ResourceID))
                        {
                            this.DefaultResourceID(ref LaborDS, bttTimeWeeklyView.ResourceID);
                        }
                    }
                    else if (vProposedHours[j - 1] == 0 && vActualHours[j - 1] != 0 &&
                            (("A,C".Lookup(bttTimeWeeklyView.NewRowType) == -1)))
                    {

                        //foreach (var LaborDtl_iterator in (this.SelectLaborDtl3(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.LaborType, bttTimeWeeklyView.LaborTypePseudo, bttTimeWeeklyView.ProjectID, bttTimeWeeklyView.PhaseID, bttTimeWeeklyView.TimeTypCd, bttTimeWeeklyView.JobNum, bttTimeWeeklyView.AssemblySeq, bttTimeWeeklyView.OprSeq, bttTimeWeeklyView.IndirectCode, bttTimeWeeklyView.RoleCd, bttTimeWeeklyView.ResourceGrpID, bttTimeWeeklyView.ResourceID, bttTimeWeeklyView.ExpenseCode, bttTimeWeeklyView.Shift, bttTimeWeeklyView.QuickEntryCode, bttTimeWeeklyView.TimeStatus)))
                        /* For some reasons, the query with more than 16 parameters does not retrieve the records correctly using structured parameter list. Using workaround to pass 14 parameters and filter within the foreach */
                        foreach (var LaborDtl_iterator in (this.SelectLaborDtlWithUpdLock3(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.LaborType, bttTimeWeeklyView.LaborTypePseudo, bttTimeWeeklyView.ProjectID, bttTimeWeeklyView.PhaseID, bttTimeWeeklyView.TimeTypCd, bttTimeWeeklyView.JobNum, bttTimeWeeklyView.AssemblySeq, bttTimeWeeklyView.OprSeq, bttTimeWeeklyView.IndirectCode, bttTimeWeeklyView.RoleCd, bttTimeWeeklyView.ResourceGrpID)))
                        {
                            LaborDtl = LaborDtl_iterator;
                            if (!LaborDtl.ResourceID.KeyEquals(bttTimeWeeklyView.ResourceID) ||
                                !LaborDtl.ExpenseCode.KeyEquals(bttTimeWeeklyView.ExpenseCode) ||
                                LaborDtl.Shift != bttTimeWeeklyView.Shift ||
                                !LaborDtl.QuickEntryCode.KeyEquals(bttTimeWeeklyView.QuickEntryCode) ||
                                !LaborDtl.TimeStatus.KeyEquals(bttTimeWeeklyView.TimeStatus))
                            { continue; }

                            bDelttLaborDtl = null;
                            if (bDelttLaborDtlRows != null)
                            {
                                bDelttLaborDtl = (from bDelttLaborDtl_Row in bDelttLaborDtlRows
                                                  where bDelttLaborDtl_Row.Company.KeyEquals(Session.CompanyID) &&
                                                  bDelttLaborDtl_Row.EmployeeNum.KeyEquals(bttTimeWeeklyView.EmployeeNum) &&
                                                  bDelttLaborDtl_Row.PayrollDate.Value.Date == vCurrentDate.Value.Date &&
                                                  bDelttLaborDtl_Row.LaborType.KeyEquals(bttTimeWeeklyView.LaborType) &&
                                                  bDelttLaborDtl_Row.LaborTypePseudo.KeyEquals(bttTimeWeeklyView.LaborTypePseudo) &&
                                                  bDelttLaborDtl_Row.ProjectID.KeyEquals(bttTimeWeeklyView.ProjectID) &&
                                                  bDelttLaborDtl_Row.PhaseID.KeyEquals(bttTimeWeeklyView.PhaseID) &&
                                                  bDelttLaborDtl_Row.TimeTypCd.KeyEquals(bttTimeWeeklyView.TimeTypCd) &&
                                                  bDelttLaborDtl_Row.JobNum.KeyEquals(bttTimeWeeklyView.JobNum) &&
                                                  bDelttLaborDtl_Row.AssemblySeq == bttTimeWeeklyView.AssemblySeq &&
                                                  bDelttLaborDtl_Row.OprSeq == bttTimeWeeklyView.OprSeq &&
                                                  bDelttLaborDtl_Row.IndirectCode.KeyEquals(bttTimeWeeklyView.IndirectCode) &&
                                                  bDelttLaborDtl_Row.RoleCd.KeyEquals(bttTimeWeeklyView.RoleCd) &&
                                                  bDelttLaborDtl_Row.ResourceGrpID.KeyEquals(bttTimeWeeklyView.ResourceGrpID) &&
                                                  bDelttLaborDtl_Row.ResourceID.KeyEquals(bttTimeWeeklyView.ResourceID) &&
                                                  bDelttLaborDtl_Row.ExpenseCode.KeyEquals(bttTimeWeeklyView.ExpenseCode) &&
                                                  bDelttLaborDtl_Row.Shift == bttTimeWeeklyView.Shift &&
                                                  bDelttLaborDtl_Row.QuickEntryCode.KeyEquals(bttTimeWeeklyView.QuickEntryCode) &&
                                                  bDelttLaborDtl_Row.TimeStatus.KeyEquals(bttTimeWeeklyView.TimeStatus)
                                                  select bDelttLaborDtl_Row).FirstOrDefault();
                            }
                            else bDelttLaborDtlRows = new List<LaborDtlRow>();
                            if (bDelttLaborDtl == null)
                            {
                                BufferCopy.Copy(LaborDtl, ref bDelttLaborDtl);
                            }
                            BufferCopy.Copy(LaborDtl, ref ttLaborDtl);
                            tmpLaborHedSeq = LaborDtl.LaborHedSeq;
                            this.LaborDtlBeforeDelete();
                            Db.LaborDtl.Delete(LaborDtl);
                        }
                    }
                    else if (vProposedHours[j - 1] < vActualHours[j - 1]  /* and vProposedHours[j] >= 0 and
                            (can-do("C,A":U,bttTimeWeeklyView.NewRowType) = false) */
                              &&
                              (("A".Lookup(bttTimeWeeklyView.NewRowType) == -1)))
                    {
                        vDiffHours[j - 1] = vActualHours[j - 1] - vProposedHours[j - 1]; /* always a positive number */
                        //DECREASELABORHOURSBLOCK:
                        do
                        {

                            //LaborDtl = this.FindFirstLaborDtlWithUpdLock(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.LaborType, bttTimeWeeklyView.LaborTypePseudo, bttTimeWeeklyView.ProjectID, bttTimeWeeklyView.PhaseID, bttTimeWeeklyView.TimeTypCd, bttTimeWeeklyView.JobNum, bttTimeWeeklyView.AssemblySeq, bttTimeWeeklyView.OprSeq, bttTimeWeeklyView.IndirectCode, bttTimeWeeklyView.RoleCd, bttTimeWeeklyView.ResourceGrpID, bttTimeWeeklyView.ResourceID, bttTimeWeeklyView.ExpenseCode, bttTimeWeeklyView.Shift, bttTimeWeeklyView.TimeStatus, bttTimeWeeklyView.QuickEntryCode);
                            LaborDtl = null;
                            vAmbiguousLaborDtl = false;
                            /* For some reasons, the query with more than 16 parameters does not retrieve the records correctly using structured parameter list. Using workaround to pass 14 parameters and filter within the foreach */
                            foreach (var LaborDtl_iterator in (this.SelectLaborDtlWithUpdLock4(Session.CompanyID, bttTimeWeeklyView.EmployeeNum, vCurrentDate, bttTimeWeeklyView.LaborType, bttTimeWeeklyView.LaborTypePseudo, bttTimeWeeklyView.ProjectID, bttTimeWeeklyView.PhaseID, bttTimeWeeklyView.TimeTypCd, bttTimeWeeklyView.JobNum, bttTimeWeeklyView.AssemblySeq, bttTimeWeeklyView.OprSeq, bttTimeWeeklyView.IndirectCode, bttTimeWeeklyView.RoleCd, bttTimeWeeklyView.ResourceGrpID)))
                            {
                                if (!LaborDtl_iterator.ResourceID.KeyEquals(bttTimeWeeklyView.ResourceID) ||
                                    !LaborDtl_iterator.ExpenseCode.KeyEquals(bttTimeWeeklyView.ExpenseCode) ||
                                    LaborDtl_iterator.Shift != bttTimeWeeklyView.Shift ||
                                    !LaborDtl_iterator.QuickEntryCode.KeyEquals(bttTimeWeeklyView.QuickEntryCode) ||
                                    !LaborDtl_iterator.TimeStatus.KeyEquals(bttTimeWeeklyView.TimeStatus))
                                { continue; }
                                // workaround for ambiguous check with more than 16 parameters 
                                if (LaborDtl != null)
                                {
                                    vAmbiguousLaborDtl = true;
                                    break;
                                }
                                LaborDtl = LaborDtl_iterator;
                            }               /* Increasing Labor Hours - create a new ttLaborDtl */
                            if (vAmbiguousLaborDtl)
                            {
                                vMessage = Strings.TheLaborHoursYouHaveSelecUpdatedForAreComprOfMulti(vCurrentDate);
                                throw new BLException(vMessage, "LaborDtl");
                            }

                            /* added */
                            if (LaborDtl == null)
                            {
                                ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
                                CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
                                ttLaborDtl.Company = Session.CompanyID;
                                ttLaborDtl.LaborHedSeq = LaborHed.LaborHedSeq;
                                ttLaborDtl.LaborDtlSeq = 0;
                                ttLaborDtl.EmployeeNum = LaborHed.EmployeeNum;
                                this.LaborDtlSetDefaults(ttLaborDtl);
                                BufferCopy.Copy(bttTimeWeeklyView, ref ttLaborDtl);
                                if (ttLaborDtl.JobNum != null) validateJob(ttLaborDtl.JobNum, true, true);
                                if (vCurrentDate == null)
                                {
                                    ttLaborDtl.PayrollDate = null;
                                }
                                else
                                {
                                    ttLaborDtl.PayrollDate = vCurrentDate.Value.Date;
                                }

                                if (vCurrentDate == null)
                                {
                                    ttLaborDtl.ClockInDate = null;
                                }
                                else
                                {
                                    ttLaborDtl.ClockInDate = vCurrentDate.Value.Date;
                                }

                                ttLaborDtl.LaborHrs = vProposedHours[j - 1] - vActualHours[j - 1];
                                ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
                                ttLaborDtl.RowMod = "A";

                                /* chgWcCode has to run after laborDtlSetDefaults so we have a rate */
                                if (!String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID))
                                {
                                    this.chgWcCode(ttLaborDtl.ResourceGrpID, false, ref vMessage);
                                }                  /* project */
                                if (ttLaborDtl.LaborTypePseudo.Compare("J") == 0)
                                {
                                    ttLaborDtl.PhaseJobNum = ttLaborDtl.JobNum;
                                    ttLaborDtl.PhaseOprSeq = ttLaborDtl.OprSeq;
                                }
                            }
                            else if (LaborDtl != null)
                            {  /* and LaborDtl.LaborHrs - vDiffHours[j] >= 0 */
                                if (vDiffHours[j - 1] > 0)
                                {
                                    LaborDtl.LaborHrs = LaborDtl.LaborHrs - vDiffHours[j - 1];
                                    LaborDtl.BurdenHrs = LaborDtl.LaborHrs;
                                    bttTimeWeeklyView.HoursTotal = bttTimeWeeklyView.HoursTotal - vDiffHours[j - 1];
                                    Db.Validate(LaborDtl);
                                    this.refreshTtLaborDtl(LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq);
                                    this.LaborDtlBeforeUpdate();
                                    vDiffHours[j - 1] = 0;
                                    break;
                                }
                                else
                                {
                                    bttTimeWeeklyView.HoursTotal = bttTimeWeeklyView.HoursTotal - LaborDtl.LaborHrs;
                                    vDiffHours[j - 1] = vDiffHours[j - 1] - LaborDtl.LaborHrs;
                                    LaborDtl.LaborHrs = 0;

                                    Db.Validate(LaborDtl);
                                    this.refreshTtLaborDtl(LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq);
                                }
                            }
                            if (LaborDtl != null && ("S,P,A".Lookup(LaborDtl.TimeStatus) > -1))
                            {
                                bttTimeWeeklyView.MessageText = "Time exists that is Submitted, Partially Approved or Approved and was unable to be updated.";
                            }
                            else
                            {
                                if (vDiffHours[j - 1] > 0 && ttLaborDtl.LaborHrs - vDiffHours[j - 1] >= 0)
                                {
                                    ttLaborDtl.LaborHrs = ttLaborDtl.LaborHrs - vDiffHours[j - 1];
                                    ttLaborDtl.BurdenHrs = ttLaborDtl.LaborHrs;
                                    bttTimeWeeklyView.HoursTotal = bttTimeWeeklyView.HoursTotal - vDiffHours[j - 1];
                                    //Db.Validate(ttLaborDtl);
                                    this.refreshTtLaborDtl(ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);
                                    vDiffHours[j - 1] = 0;
                                    break;
                                }
                            }
                        }
                        while (false);
                    }
                    if (ldRefreshToDate == null)
                    {
                        ldRefreshToDate = bttTimeWeeklyView.WeekEndDate;
                    }
                    else if (bttTimeWeeklyView.WeekEndDate != null)
                    {
                        if (bttTimeWeeklyView.WeekEndDate.Value.Date > ldRefreshToDate.Value.Date)
                            ldRefreshToDate = bttTimeWeeklyView.WeekEndDate;
                    }
                    if (ldRefreshFromDate == null)
                    {
                        ldRefreshFromDate = bttTimeWeeklyView.WeekBeginDate;
                    }
                    else if (bttTimeWeeklyView.WeekBeginDate != null)
                    {
                        if (bttTimeWeeklyView.WeekBeginDate.Value.Date < ldRefreshFromDate.Value.Date)
                            ldRefreshFromDate = bttTimeWeeklyView.WeekBeginDate;
                    }
                }
                if ((bttTimeWeeklyView.HoursSun == 0 && bttTimeWeeklyView.HoursMon == 0 &&
                    bttTimeWeeklyView.HoursTue == 0 && bttTimeWeeklyView.HoursWed == 0 &&
                    bttTimeWeeklyView.HoursThu == 0 && bttTimeWeeklyView.HoursFri == 0 &&
                    bttTimeWeeklyView.HoursSat == 0))
                {
                    bttTimeWeeklyView.RowMod = "D";
                }
                else if (bttTimeWeeklyView.RowMod.Compare("A") == 0)
                {
                    CurrentFullTableset.TimeWeeklyView.Remove(bttTimeWeeklyView);
                }
            }
        }

        /// <summary>
        /// Method to recall Labor for Approval. 
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="lWeeklyView">Is this method being called with WeeklyView records?</param>
        /// <param name="cMessageText">Message text to present to the user after the process is finished </param>
        public void RecallFromApproval(ref LaborTableset ds, bool lWeeklyView, out string cMessageText)
        {
            cMessageText = string.Empty;
            CurrentFullTableset = ds;
            bool canRecall = false;

            switch (lWeeklyView)
            {
                case true:
                    {

                        //WeeklyView_Loop:
                        foreach (var ttTimeWeeklyView_iterator in (from ttTimeWeeklyView_Row in CurrentFullTableset.TimeWeeklyView
                                                                   where !String.IsNullOrEmpty(ttTimeWeeklyView_Row.RowMod)
                                                                   select ttTimeWeeklyView_Row))
                        {
                            ttTimeWeeklyView = ttTimeWeeklyView_iterator;
                            if (ttTimeWeeklyView.TimeStatus.Compare("E") == 0)
                            {
                                continue;
                            }

                            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                            {
                                LaborDtlParams ipLaborDtlParams2 = new LaborDtlParams();
                                ipLaborDtlParams2.EmployeeNum = ttTimeWeeklyView.EmployeeNum;
                                ipLaborDtlParams2.WeekBeginDate = ttTimeWeeklyView.WeekBeginDate.Value.Date;
                                ipLaborDtlParams2.WeekEndDate = (ttTimeWeeklyView.WeekBeginDate.Value.AddDays(6)).Date;
                                ipLaborDtlParams2.LaborType = ttTimeWeeklyView.LaborType;
                                ipLaborDtlParams2.TimeStatus = ttTimeWeeklyView.TimeStatus;
                                ipLaborDtlParams2.LaborTypePseudo = ttTimeWeeklyView.LaborTypePseudo;
                                ipLaborDtlParams2.ProjectID = ttTimeWeeklyView.ProjectID;
                                ipLaborDtlParams2.PhaseID = ttTimeWeeklyView.PhaseID;
                                ipLaborDtlParams2.TimeTypCd = ttTimeWeeklyView.TimeTypCd;
                                ipLaborDtlParams2.JobNum = ttTimeWeeklyView.JobNum;
                                ipLaborDtlParams2.AssemblySeq = ttTimeWeeklyView.AssemblySeq;
                                ipLaborDtlParams2.OprSeq = ttTimeWeeklyView.OprSeq;
                                ipLaborDtlParams2.IndirectCode = ttTimeWeeklyView.IndirectCode;
                                ipLaborDtlParams2.RoleCd = ttTimeWeeklyView.RoleCd;
                                ipLaborDtlParams2.ResourceGrpID = ttTimeWeeklyView.ResourceGrpID;
                                ipLaborDtlParams2.ResourceID = ttTimeWeeklyView.ResourceID;
                                ipLaborDtlParams2.ExpenseCode = ttTimeWeeklyView.ExpenseCode;
                                ipLaborDtlParams2.Shift = ttTimeWeeklyView.Shift;
                                ipLaborDtlParams2.QuickEntryCode = ttTimeWeeklyView.QuickEntryCode;
                                ipLaborDtlParams2.LaborHrs = decimal.Zero;

                                foreach (var LaborDtl_iterator in (this.SelectWeeklyLaborDtlWithUpdLock(Session.CompanyID, ipLaborDtlParams2)))
                                {
                                    LaborDtl = LaborDtl_iterator;

                                    if (PELock.IsDocumentLock(Session.CompanyID, "LaborDtl", Convert.ToString(LaborDtl.LaborHedSeq), Convert.ToString(LaborDtl.LaborDtlSeq), "", "", "", ""))
                                    {
                                        throw new BLException(PELock.LockMessage);
                                    }

                                    canRecall = false;
                                    this.validateJob(LaborDtl.JobNum, false, false);
                                    if (LaborDtl.WipPosted == false &&
                                        !LaborDtl.TimeStatus.KeyEquals("E") &&
                                        !LaborDtl.TimeStatus.KeyEquals("R") &&
                                        !String.IsNullOrEmpty(LaborDtl.TimeStatus))
                                    {
                                        canRecall = true;
                                    }

                                    if (canRecall == true)
                                    {
                                        LaborDtl.TimeStatus = "E";
                                        Db.Validate(LaborDtl);
                                        this.refreshTtLaborDtl(LaborDtl.LaborHedSeq, LaborDtl.LaborDtlSeq);
                                        ttLaborDtl = null;
                                    }
                                }

                                txScope.Complete();
                            }

                            ttTimeWeeklyView.RowMod = "";
                            ttTimeWeeklyView.RowSelected = false;
                        }
                    }
                    break;
                default:


                    //LaborDtl_Loop:
                    foreach (var ttLaborDtl_iterator in (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                                                         where !String.IsNullOrEmpty(ttLaborDtl_Row.RowMod)
                                                         select ttLaborDtl_Row))
                    {
                        ttLaborDtl = ttLaborDtl_iterator;
                        if (ttLaborDtl.TimeStatus.Compare("E") == 0)
                        {
                            continue;
                        }

                        if (!ValidateProjectClosedRecallCopy(ttLaborDtl.ProjectID, ttLaborDtl.JobNum, ttLaborDtl.LaborTypePseudo))
                        {
                            throw new BLException(Strings.ProjectClosed(ttLaborDtl.ProjectID));
                        }

                        if (PELock.IsDocumentLock(Session.CompanyID, "LaborDtl", Convert.ToString(ttLaborDtl.LaborHedSeq), Convert.ToString(ttLaborDtl.LaborDtlSeq), "", "", "", ""))
                        {
                            throw new BLException(PELock.LockMessage);
                        }

                        canRecall = false;
                        this.validateJob(ttLaborDtl.JobNum, false, false);

                        using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                        {

                            LaborDtl = this.FindFirstLaborDtlWithUpdLock2(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);
                            if (LaborDtl != null)
                            {
                                if (LaborDtl.WipPosted == false &&
                                    (LaborDtl.TimeStatus.Compare("E") != 0 &&
                                     LaborDtl.TimeStatus.Compare("R") != 0 &&
                                     !String.IsNullOrEmpty(LaborDtl.TimeStatus)))
                                {
                                    canRecall = true;
                                }

                                if (canRecall == true)
                                {
                                    LaborDtl.TimeStatus = "E";
                                    Db.Validate(LaborDtl);
                                    ttLaborDtl.SysRevID = (long)LaborDtl.SysRevNum;
                                    ttLaborDtl.TimeStatus = LaborDtl.TimeStatus;
                                    ttLaborDtl.TimeAutoSubmit = this.IsTimeAutoSubmitPlantConfCtrl(Session.CompanyID, Session.PlantID);

                                }
                            }

                            ttLaborDtl.RowMod = "";
                            this.LaborDtlAfterGetRows();
                            txScope.Complete();
                        }
                    }
                    break;
            }

        }

        /// <summary>
        /// Refreshes the LaborDtl records after they have been "Submitted" for approval   
        /// </summary>
        private void refreshAfterSubmit(out string timeWeeklyStatus)
        {
            timeWeeklyStatus = string.Empty;
            foreach (var ttTEKey in ttTEKeyRows)
            {
                foreach (var LaborDtl_iterator in (this.SelectLaborDtl(Session.CompanyID, Compatibility.Convert.ToInt32(ttTEKey.Key1), Compatibility.Convert.ToInt32(ttTEKey.Key2))))
                {
                    LaborDtl = LaborDtl_iterator;
                    this.refreshTtLaborDtl(LaborDtl.LaborHedSeq, LaborDtl.LaborHedSeq);
                    timeWeeklyStatus = LaborDtl.TimeStatus;
                }
            }
        }
        /// <summary>
        /// Refreshes the LaborPart records in the dataset so that the dataset always contains all end parts
        /// defined in the JobPart table. It considers that the dataset may already contain LaborPart (updating) so it 
        /// only inserts parts that do not already exist.   
        /// </summary>
        /// <remarks>
        /// Called as part updating a LaborDtl or LaborPart record and also as part of GetRows. 
        /// When updating a labordtl and they change the JobNum/Assembly/opr seq then we will delete all
        /// related LaborPart records.
        /// See special logic in BeforeUpdate that was required due to these generated records.
        /// </remarks>
        private void refreshLaborPart(int ip_LaborHedSeq, int ip_LaborDtlSeq)
        {
            string partUOM = string.Empty;
            this.genLaborPart(ip_LaborHedSeq, ip_LaborDtlSeq);
            return;
        }

        private void refreshTtLaborDtl(int ipLaborHedSeq, int ipLaborDtlSeq)
        {
            if (LaborDtl == null)
            {


                LaborDtl = this.FindFirstLaborDtl7(Session.CompanyID, ipLaborHedSeq, ipLaborDtlSeq);
            }

            if (LaborDtl != null)
            {


                ttLaborDtl = (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                              where ttLaborDtl_Row.SysRowID == LaborDtl.SysRowID &&
                                    ttLaborDtl_Row.RowMod.Compare(IceRow.ROWSTATE_UNCHANGED) != 0
                              select ttLaborDtl_Row).LastOrDefault();
                if (ttLaborDtl == null)
                {


                    ttLaborDtl = (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                                  where ttLaborDtl_Row.SysRowID == LaborDtl.SysRowID
                                  select ttLaborDtl_Row).LastOrDefault();
                }
                if (ttLaborDtl == null)
                {
                    ttLaborDtl = new Erp.Tablesets.LaborDtlRow();
                    CurrentFullTableset.LaborDtl.Add(ttLaborDtl);
                    ttLaborDtl.SysRowID = LaborDtl.SysRowID;
                }
                BufferCopy.Copy(LaborDtl, ttLaborDtl);
                this.LaborDtlAfterGetRows();
            }
        }

        /// <summary>
        /// This method is intended to be used when the MES/ShopFloor user selects an 
        /// operation from the WorkQueue to work on.  Use this method in place of the 
        /// Update method in this situation.
        /// 
        /// This method expects a LaborDataSet containing a single LaborHed with a 
        /// RowMod indicating a changed row, and a LaborDtl row with a RowMod indicating 
        /// a changed row.  This can be obtained with a call to Labor.GetRows() with a 
        /// whereClauseLaborHed of 
        ///   ActiveTrans = YES and EmployeeNum = xxxx
        ///      substituting the desired employeeNum for the xxxx.
        /// followed by a call to LaborDtlGetNew.
        /// 
        /// After validating the given Job, Assembly, Operation, ResourceID, ResourceGrpID
        /// and LaborType, and additional validations are applied, the LaborDtl is updated.  
        /// 
        /// An exception is thrown if:
        /// - a changed Laborhed row is not found.
        /// - the given Job, Assembly and Operation is not valid
        /// - the LaborHed.ActiveTrans = no.  This method is for MES (ShopFloor) use only.
        /// - the given LaborType is not "S" or "P"
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="pcResourceGrpId">The Resource Group id for this work.</param>
        /// <param name="pcResourceId">The Resource id for this work.</param>
        /// <param name="pcLaborType">Labor Type: S=Setup, P=Production</param>
        public void SelectForWork(ref LaborTableset ds, string pcResourceGrpId, string pcResourceId, string pcLaborType)
        {
            CurrentFullTableset = ds;
            string cTmp = string.Empty;
            bool lResChanged = false;
            string pcJobNum = string.Empty;
            int piAsmSeq = 0;
            int piOprSeq = 0;


            ttLaborHed = (from ttLaborHed_Row in ds.LaborHed
                          where modList.Lookup(ttLaborHed_Row.RowMod) != -1
                          select ttLaborHed_Row).FirstOrDefault();
            if (ttLaborHed == null)
            {
                throw new BLException(Strings.LaborHeaderHasNotChanged);
            }



            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged);
            }
            if (ttLaborHed.ActiveTrans == false)
            {
                throw new BLException(Strings.SelecMethodIsForMESOnly);
            }



            EmpBasic = this.FindFirstEmpBasic21(ttLaborHed.Company, ttLaborHed.EmployeeNum);
            if (EmpBasic == null)
            {
                cTmp = Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.EmployeeID);
                throw new BLException(cTmp);
            }


            foreach (var ttLaborDtl_iterator in (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                                                 where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                                                 select ttLaborDtl_Row))
            {
                ttLaborDtl = ttLaborDtl_iterator;
                pcJobNum = ttLaborDtl.JobNum;
                piAsmSeq = ttLaborDtl.AssemblySeq;
                piOprSeq = ttLaborDtl.OprSeq;


                if ((this.ExistsLaborDtl3(ttLaborHed.Company, ttLaborHed.EmployeeNum, pcJobNum, piAsmSeq, piOprSeq, true)))
                {
                    cTmp = Erp.Services.Lib.Resources.GlobalStrings.MsgDuplicate(Strings.EmployeeID);
                    throw new BLException(cTmp);
                }
                if (ttLaborDtl.OkToChangeResourceGrpID == false && (ttLaborDtl.ResourceGrpID.Compare(pcResourceGrpId) != 0))
                {
                    throw new BLException(Strings.ResouGroupChangeHasNotBeenAppro);
                }



                JobHead = this.FindFirstJobHead13(ttLaborHed.Company, pcJobNum);
                if (JobHead == null)
                {
                    cTmp = Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.JobNumber);
                    throw new BLException(cTmp);
                }



                JobOper = this.FindFirstJobOper26(ttLaborHed.Company, pcJobNum, piAsmSeq, piOprSeq);
                if (JobOper == null)
                {
                    cTmp = Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.JobOperation);
                    throw new BLException(cTmp);
                }/* if not avail JobOper */

                if ("S,P".Lookup(pcLaborType) == -1)
                {
                    cTmp = Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.LaborType);
                    throw new BLException(cTmp);
                }
                /* if resources changed without changing the joboper then lResChanged = yes */
                lResChanged = (ttLaborDtl.ResourceGrpID.Compare(pcResourceGrpId) != 0 ||
                               ttLaborDtl.ResourceID.Compare(pcResourceId) != 0) &&
                              (ttLaborDtl.OprSeq == piOprSeq &&
                               ttLaborDtl.AssemblySeq == piAsmSeq &&
                               ttLaborDtl.JobNum.Compare(pcJobNum) == 0);
                saveResourceID = ttLaborDtl.ResourceID;
                saveResourceGrpID = ttLaborDtl.ResourceGrpID;
                ttLaborDtl.Company = ttLaborHed.Company;
                ttLaborDtl.EmployeeNum = ttLaborHed.EmployeeNum;
                ttLaborDtl.LaborHedSeq = ttLaborHed.LaborHedSeq;
                ttLaborDtl.Shift = ttLaborHed.Shift;
                ttLaborDtl.JobNum = pcJobNum;
                ttLaborDtl.AssemblySeq = piAsmSeq;
                ttLaborDtl.OprSeq = piOprSeq;
                ttLaborDtl.LaborType = pcLaborType;
                ttLaborDtl.LaborTypePseudo = pcLaborType;
                if (ttLaborHed.PayrollDate == null)
                {
                    ttLaborDtl.PayrollDate = null;
                }
                else
                {
                    ttLaborDtl.PayrollDate = ttLaborHed.PayrollDate;
                }

                ttLaborHed.PayrollDateNav = ttLaborHed.PayrollDate;
                ttLaborDtl.ReWork = false;
                ttLaborDtl.LaborCollection = true;
                ttLaborDtl.ActiveTrans = true;
                ttLaborDtl.Complete = false;
                ttLaborDtl.OpComplete = false;
                ttLaborDtl.OpCode = JobOper.OpCode;
                ttLaborDtl.ResourceGrpID = pcResourceGrpId;
                ttLaborDtl.ResourceID = pcResourceId;
                ttLaborDtl.LaborEntryMethod = JobOper.LaborEntryMethod;
                ttLaborDtl.MES = true;
                ttLaborHed.MES = true;

                ttLaborDtl.LaborRate = EmpBasic.LaborRate;
                ttLaborDtl.ExpenseCode = ((!String.IsNullOrEmpty(JobHead.ExpenseCode)) ? JobHead.ExpenseCode : EmpBasic.ExpenseCode);

                if (JobOper.LaborEntryMethod.KeyEquals("Q"))
                {
                    if (ttLaborDtl.LaborType.KeyEquals("P"))
                    {
                        ttLaborDtl.LaborRate = JobOper.ProdLabRate;
                    }
                    else if (ttLaborDtl.LaborType.KeyEquals("S"))
                    {
                        ttLaborDtl.LaborRate = JobOper.SetupLabRate;
                    }
                }


                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, pcResourceGrpId);
                if (ResourceGroup != null)
                {
                    ttLaborDtl.JCDept = ResourceGroup.JCDept;

                    var ResourceResult = this.FindFirstResource20(Session.CompanyID, ResourceGroup.ResourceGrpID, ttLaborDtl.ResourceID);
                    if (ResourceResult != null)
                    {
                        if (JobOper.LaborEntryMethod.KeyEquals("Q"))
                        {
                            if (ttLaborDtl.LaborType.KeyEquals("P"))
                            {
                                ttLaborDtl.LaborRate = ResourceResult.ProdLabRate;
                            }
                            else if (ttLaborDtl.LaborType.KeyEquals("S"))
                            {
                                ttLaborDtl.LaborRate = ResourceResult.SetupLabRate;
                            }
                        }
                    }

                }/* if avail ResourceGroup */
                var outBurdenRate3 = ttLaborDtl.BurdenRate;
                this.getLaborDtlBurdenRates(lResChanged, out outBurdenRate3);
                ttLaborDtl.BurdenRate = outBurdenRate3;
            }
        }

        /// <summary>
        /// This method runs the shop warning routine and returns any warnings the user needs
        /// to be aware of.  This needs to be run right before the SelectForWork method.  If the user answers
        /// okay to all of the questions, then the SelectForWork method can be run.  Otherwise the labor record
        /// needs to be corrected
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="vMessage">List of error warnings for user</param>
        public void SelectForWorkCheckWarnings(ref LaborTableset ds, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where ttLaborDtl_Row.RowMod.Compare("U") == 0 || ttLaborDtl_Row.RowMod.Compare("A") == 0
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged);
            }
            this.chkShopWarn(out vMessage);
        }

        /// <summary>
        /// Sets the Time Stamp in which the Employee Starts his/her activity and
        /// also populates the field that displays the time correctly.
        /// </summary>
        /// <param name="ds">The Labor data set</param>
        public void SetClockInAndDisplayTimeMES(ref LaborTableset ds)
        {
            CurrentFullTableset = ds;


            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl != null)
            {
                this.clockInTimeMES();
                this.setDisplayTime();
            }
        }

        /// <summary>
        /// This method sets the complete flags
        /// </summary>
        private void setComplete(string jobNum, int assemblySeq, int oprSeq, decimal laborQty, decimal scrapQty)
        {
            decimal origQty = decimal.Zero;
            bool vlComplete = false;


            JobOper = this.FindFirstJobOper27(Session.CompanyID, jobNum, assemblySeq, oprSeq);
            if (JobOper != null)
            {
                this.getBeforeInfo();
                if (oldJobNum.Compare(jobNum) == 0 &&
                oldAssSeq == assemblySeq &&
                oldOprSeq == oprSeq)
                {
                    origQty = JobOper.QtyCompleted - oldLbrQty;
                }
                else
                {
                    origQty = JobOper.QtyCompleted;
                }

                if (ttLaborDtl == null && LaborDtl == null)
                {
                    origQty = 0;
                }

                if (origQty < 0)
                {
                    origQty = 0;
                }

                if (laborQty + scrapQty + origQty >= JobOper.RunQty)
                {
                    if (ttLaborDtl != null)
                    {
                        if (!(ttLaborDtl.EnableSN && ttLaborDtl.ReWork))
                            ttLaborDtl.Complete = true;
                    }
                    else
                    {
                        LaborDtl.Complete = true;
                    }
                }
                else
                {
                    if (ttLaborDtl != null)
                    {
                        ttLaborDtl.Complete = false;
                    }
                    else
                    {
                        LaborDtl.Complete = false;
                    }
                }
                vlComplete = ((ttLaborDtl != null) ? (!(ttLaborDtl.EnableSN && ttLaborDtl.ReWork) ? ttLaborDtl.Complete : false) : LaborDtl.Complete);        /* if SVComplete <> LaborDtl.Complete then */
                this.setOprComplete(vlComplete);
            }
            else
            {
                if (ttLaborDtl != null)
                {
                    ttLaborDtl.Complete = false;
                }
                else
                {
                    LaborDtl.Complete = false;           /*** AXL 7911 - Dale Bostrom ***/
                }

                if (ttLaborDtl != null)
                {
                    this.setOprComplete(ttLaborDtl.Complete);
                }
                else
                {
                    this.setOprComplete(LaborDtl.Complete);
                }
            }
        }

        private void setDisplayTime()
        {
            if (ttJCSyst.ClockFormat.Compare("M") == 0)
            {
                ttLaborDtl.DspClockInTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborDtl.ClockinTime), "99") + ":" + Compatibility.Convert.ToString(((ttLaborDtl.ClockinTime - Math.Truncate(ttLaborDtl.ClockinTime)) * 60), "99");
                ttLaborDtl.DspClockOutTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborDtl.ClockOutTime), "99") + ":" + Compatibility.Convert.ToString(((ttLaborDtl.ClockOutTime - Math.Truncate(ttLaborDtl.ClockOutTime)) * 60), "99");
            }
            else
            {
                ttLaborDtl.DspClockInTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborDtl.ClockinTime), "99") + Compatibility.Convert.ToString((ttLaborDtl.ClockinTime - Math.Truncate(ttLaborDtl.ClockinTime)), ".99");
                ttLaborDtl.DspClockOutTime = Compatibility.Convert.ToString(Math.Truncate(ttLaborDtl.ClockOutTime), "99") + Compatibility.Convert.ToString((ttLaborDtl.ClockOutTime - Math.Truncate(ttLaborDtl.ClockOutTime)), ".99");
            }
        }

        private void setdispValue()
        {
            if (ttLaborHed.ClockOutTime == 24.0m)
            {
                ttLaborHed.ClockOutTime = 0;
            }

            if (ttLaborHed.ActualClockOutTime == 24.0m)
            {
                ttLaborHed.ActualClockOutTime = 0;
            }

            if (ttLaborHed.LunchInTime == 24.0m)
            {
                ttLaborHed.LunchInTime = 0;
            }

            if (ttLaborHed.ActLunchInTime == 24.0m)
            {
                ttLaborHed.ActLunchInTime = 0;
            }
            /* ACC - 09/13/05 - SCR 24315 */
            ttLaborHed.DspPayHours = ttLaborHed.PayHours;
        }

        /// <summary>
        /// This method sets the operation complete flags
        /// </summary>
        private void setOprComplete(bool cmplete)
        {
            /* if labordtl.complete = yes */
            /* LaborDtl.Complete = NO */
            if (cmplete)
            {
                if (ttLaborDtl != null)
                { /* production */
                    if (ttLaborDtl.LaborType.Compare("P") == 0)
                    {
                        ttLaborDtl.OpComplete = true;
                    }
                    /* setup */
                    if (ttLaborDtl.LaborType.Compare("S") == 0)
                    {
                        if (JobOper != null)
                        {
                            if (JobOper.EstProdHours == 0.00m)
                            {
                                ttLaborDtl.OpComplete = true;
                            }
                        }
                    }
                }
                else if (LaborDtl != null)
                { /* production */
                    if (LaborDtl.LaborType.Compare("P") == 0)
                    {
                        LaborDtl.OpComplete = true;
                    }
                    /* setup */
                    if (LaborDtl.LaborType.Compare("S") == 0)
                    {
                        if (JobOper != null)
                        {
                            if (JobOper.EstProdHours == 0.00m)
                            {
                                LaborDtl.OpComplete = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (ttLaborDtl != null)
                {
                    if (JobOper != null && JobOper.OpComplete == false)
                    {
                        ttLaborDtl.OpComplete = false;
                    }
                    else
                    {


                        if (ttLaborDtl.OprSeq > 0 && !String.IsNullOrEmpty(ttLaborDtl.JobNum) &&
                            this.ExistsLaborDtl4(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, true, ttLaborDtl.LaborDtlSeq))
                        {
                            ttLaborDtl.OpComplete = true;
                        }
                        else
                        {
                            ttLaborDtl.OpComplete = false;
                        }
                    }
                }
                else
                {
                    if (JobOper != null && JobOper.OpComplete == false)
                    {
                        LaborDtl.OpComplete = false;
                    }
                    else
                    {


                        if (LaborDtl.OprSeq > 0 && !String.IsNullOrEmpty(LaborDtl.JobNum) &&
                            this.ExistsLaborDtl4(LaborDtl.Company, LaborDtl.JobNum, LaborDtl.AssemblySeq, LaborDtl.OprSeq, true, LaborDtl.LaborDtlSeq))
                        {
                            LaborDtl.OpComplete = true;
                        }
                        else
                        {
                            LaborDtl.OpComplete = false;
                        }
                    }
                }
            }
        }
        private void SetOutputWarehouseAndBin(string resourceID, string resourceGrpID)
        {
            ResourceWhseBin resourceWhseBin = (!string.IsNullOrEmpty(resourceID)) ? FindFirstResourceWhseBin(Session.CompanyID, resourceID) : null;

            if (resourceWhseBin != null && resourceWhseBin.Location == true)
            {
                if (!string.IsNullOrEmpty(resourceWhseBin.OutputWhse))
                {
                    ttLaborDtl.OutputBin = resourceWhseBin.OutputBinNum;
                    ttLaborDtl.OutputWarehouse = resourceWhseBin.OutputWhse;
                }
                else
                {
                    ttLaborDtl.OutputBin = resourceWhseBin.InputBinNum;
                    ttLaborDtl.OutputWarehouse = resourceWhseBin.InputWhse;
                }
            }
            else
            {
                ResourceWhseBin resourceGroupWhseBin = FindFirstResourceGroupWhseBin(Session.CompanyID, resourceGrpID);
                if (resourceGroupWhseBin != null)
                {
                    if (!string.IsNullOrEmpty(resourceGroupWhseBin.OutputWhse))
                    {
                        ttLaborDtl.OutputBin = resourceGroupWhseBin.OutputBinNum;
                        ttLaborDtl.OutputWarehouse = resourceGroupWhseBin.OutputWhse;
                    }
                    else
                    {
                        ttLaborDtl.OutputBin = resourceGroupWhseBin.InputBinNum;
                        ttLaborDtl.OutputWarehouse = resourceGroupWhseBin.InputWhse;
                    }
                }
            }
        }

        private void setRetrieveRange(bool inByDay, bool inByWeek, bool inByMonth)
        {
            if (inByDay == false &&
                inByWeek == false &&
                inByMonth == false)
            {
                throw new BLException(Strings.OneRetriOptionMustBeSelec);
            }
            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
            {
                UserFile = FindFirstUserFileUpd(Session.UserID);
                if (UserFile != null)
                {
                    UserFile.TERetrieveByDay = inByDay;
                    UserFile.TERetrieveByWeek = inByWeek;
                    UserFile.TERetrieveByMonth = inByMonth;
                    Db.Validate(UserFile);
                }
                txScope.Complete();
            }
        }

        private void setRowMod(LaborTableset ds)
        {
            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          select ttLaborDtl_Row).FirstOrDefault();
            ttLaborDtl.RowMod = IceRow.ROWSTATE_UPDATED;
        }

        /// <summary>
        /// Method to set the value UserFile.TERetrieveApproved 
        /// </summary>
        /// <param name="ipTERetrieveApproved">Value to set UserFile.TERetrieveApproved Yes/No</param>
        public void SetTERetrieveApproved(bool ipTERetrieveApproved)
        {
            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope()) //start the transaction
            {

                UserFile = this.FindFirstUserFileWithUpdLock(Session.UserID);
                if (UserFile != null)
                {
                    UserFile.TERetrieveApproved = ipTERetrieveApproved;
                    Db.Validate(UserFile);
                }
                txScope.Complete();
            }
        }

        /// <summary>
        /// Method to set the value for retrieve by day 
        /// </summary>
        /// <param name="ipTERetrieveByDay">Value to set the by day option Yes/No</param>
        public void SetTERetrieveByDay(bool ipTERetrieveByDay)
        {
            this.setRetrieveRange(ipTERetrieveByDay, false, false);
        }

        /// <summary>
        /// Method to set the value for retrieve by month 
        /// </summary>
        /// <param name="ipTERetrieveByMonth">Value to set the by month option Yes/No</param>
        public void SetTERetrieveByMonth(bool ipTERetrieveByMonth)
        {
            this.setRetrieveRange(false, false, ipTERetrieveByMonth);
        }

        /// <summary>
        /// Method to set the value for retrieve by week 
        /// </summary>
        /// <param name="ipTERetrieveByWeek">Value to set the by week option Yes/No</param>
        public void SetTERetrieveByWeek(bool ipTERetrieveByWeek)
        {
            this.setRetrieveRange(false, ipTERetrieveByWeek, false);
        }

        /// <summary>
        /// Method to set the value UserFile.TERetrieveEntered 
        /// </summary>
        /// <param name="ipTERetrieveEntered">Value to set UserFile.TERetrieveEntered Yes/No</param>
        public void SetTERetrieveEntered(bool ipTERetrieveEntered)
        {
            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope()) //start the transaction
            {

                UserFile = this.FindFirstUserFileWithUpdLock(Session.UserID);
                if (UserFile != null)
                {
                    UserFile.TERetrieveEntered = ipTERetrieveEntered;
                    Db.Validate(UserFile);
                }
                txScope.Complete();
            }
        }

        /// <summary>
        /// Method to set the value UserFile.TERetrievePartiallyApproved 
        /// </summary>
        /// <param name="ipTERetrievePartiallyApproved">Value to set UserFile.TERetrievePartiallyApproved Yes/No</param>
        public void SetTERetrievePartiallyApproved(bool ipTERetrievePartiallyApproved)
        {
            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope()) //start the transaction
            {

                UserFile = this.FindFirstUserFileWithUpdLock(Session.UserID);
                if (UserFile != null)
                {
                    UserFile.TERetrievePartiallyApproved = ipTERetrievePartiallyApproved;
                    Db.Validate(UserFile);
                }
                txScope.Complete();
            }
        }

        /// <summary>
        /// Method to set the value UserFile.TERetrieveRejected 
        /// </summary>
        /// <param name="ipTERetrieveRejected">Value to set UserFile.TERetrieveRejected Yes/No</param>
        public void SetTERetrieveRejected(bool ipTERetrieveRejected)
        {
            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope()) //start the transaction
            {

                UserFile = this.FindFirstUserFileWithUpdLock(Session.UserID);
                if (UserFile != null)
                {
                    UserFile.TERetrieveRejected = ipTERetrieveRejected;
                    Db.Validate(UserFile);
                }
                txScope.Complete();
            }
        }

        /// <summary>
        /// Method to set the value UserFile.TERetrieveSubmitted 
        /// </summary>
        /// <param name="ipTERetrieveSubmitted">Value to set UserFile.TERetrieveSubmitted Yes/No</param>
        public void SetTERetrieveSubmitted(bool ipTERetrieveSubmitted)
        {
            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope()) //start the transaction
            {

                UserFile = this.FindFirstUserFileWithUpdLock(Session.UserID);
                if (UserFile != null)
                {
                    UserFile.TERetrieveSubmitted = ipTERetrieveSubmitted;
                    Db.Validate(UserFile);
                }
                txScope.Complete();
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// inSetType:
        ///  1 = employee rules
        ///  2 = approver rules
        ///  3 = both employee and approver rules
        ///  4 = view-only rules
        /// </remarks>
        private void setUpdateRules(int inSetType, string inJobNum, bool inNotSubmitted, string inStatus, bool inApprvalReq, bool inWIPPosted, out bool outEnableCopy, out bool outEnableSubmit, out bool outEnableRecall, out bool outDisableUpdate, out bool outDisableDelete)
        {
            outEnableCopy = false;
            outEnableSubmit = false;
            outEnableRecall = false;
            outDisableUpdate = false;
            outDisableDelete = false;
            if (PlantConfCtrl == null)
            {


                PlantConfCtrl = this.FindFirstPlantConfCtrl3(Session.CompanyID, Session.PlantID);
            }
            switch (inSetType)
            {
                case 1:
                    {
                        outEnableCopy = true;
                        if (inNotSubmitted)
                        {
                            outDisableUpdate = false;
                            outDisableDelete = false;
                        }
                        else
                        {
                            outDisableUpdate = true;
                        }

                        outEnableSubmit = inNotSubmitted;
                        outDisableDelete = (inStatus.Compare("A") == 0);
                        if (inWIPPosted == false &&
                            (inStatus.Compare("E") != 0 &&
                             inStatus.Compare("R") != 0 &&
                             !String.IsNullOrEmpty(inStatus)))
                        {
                            outEnableRecall = true;
                        }        /*almartinez - SCR 88809*/



                        if ((this.ExistsJobHead(Session.CompanyID, inJobNum, true)))
                        {
                            outEnableRecall = false;
                        }
                    }
                    break;
                case 2:
                    {
                        outEnableCopy = !(PlantConfCtrl.TimeRestrictEntry);
                        if (PlantConfCtrl != null &&
                            (PlantConfCtrl.TimeApproverCanDel == true ||
                             PlantConfCtrl.TimeRestrictEntry == false))
                        {
                            outDisableDelete = (inStatus.Compare("A") == 0);
                        }
                        else
                        {
                            outDisableDelete = true;
                        }

                        if (PlantConfCtrl != null &&
                            (PlantConfCtrl.TimeApproverCanUpd == true ||
                             PlantConfCtrl.TimeRestrictEntry == false))
                        {
                            outDisableUpdate = (inStatus.Compare("A") == 0);
                            outEnableSubmit = inNotSubmitted;
                            if (inWIPPosted == false &&
                                (inStatus.Compare("E") != 0 &&
                                 inStatus.Compare("R") != 0 &&
                                 !String.IsNullOrEmpty(inStatus)))
                            {
                                outEnableRecall = true;
                            }
                        }
                        else
                        {
                            outDisableUpdate = true;
                            outEnableSubmit = false;
                            outEnableRecall = false;
                        }        /*almartinez - SCR 88809*/



                        if ((this.ExistsJobHead(Session.CompanyID, inJobNum, true)))
                        {
                            outEnableRecall = false;
                        }
                    }
                    break;
                case 3:
                    {
                        outEnableCopy = true;
                        outDisableDelete = (inStatus.Compare("A") == 0);
                        outEnableSubmit = inNotSubmitted;
                        if (inWIPPosted == false &&
                            (inStatus.Compare("E") != 0 &&
                             inStatus.Compare("R") != 0 &&
                             !String.IsNullOrEmpty(inStatus)))
                        {
                            outEnableRecall = true;
                        }
                        if (PlantConfCtrl != null &&
                            (PlantConfCtrl.TimeApproverCanUpd == true ||
                             PlantConfCtrl.TimeRestrictEntry == false))
                        {
                            outDisableUpdate = (inStatus.Compare("A") == 0);
                        }
                        else
                        {
                            outDisableUpdate = !(inNotSubmitted);
                        }        /*almartinez - SCR 88809*/



                        if ((this.ExistsJobHead(Session.CompanyID, inJobNum, true)))
                        {
                            outEnableRecall = false;
                        }
                    }
                    break;
                case 4:
                    {
                        outEnableCopy = false;
                        outEnableSubmit = false;
                        outEnableRecall = false;
                        outDisableUpdate = true;
                        outDisableDelete = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// Method to call to start an activity in Shop Floor. 
        /// </summary>
        /// <param name="LaborHedSeq">The labor header sequence </param>
        /// <param name="StartType">The type of activity being started.
        /// Values are: P - Production 
        ///             I - Indirect
        ///             S - Setup 
        ///             R - Rework </param>
        /// <param name="ds">The Labor data set </param>
        public void StartActivity(int LaborHedSeq, string StartType, ref LaborTableset ds)
        {
            CurrentFullTableset = ds;


            LaborHed = this.FindFirstLaborHed9(Session.CompanyID, LaborHedSeq);
            if (LaborHed == null)
            {
                throw new BLException(Strings.InvalidLaborHeaderRecord, "LaborHed");
            }
            if (LaborHed.ActiveTrans == false)
            {
                throw new BLException(Strings.TheLaborHeaderRecordIsInactive, "LaborHed");
            }
            if (StartType.Compare("I") == 0)
            {


                /* indirect */
                if ((this.ExistsLaborDtl5(Session.CompanyID, LaborHed.EmployeeNum, true, "I")))
                {
                    throw new BLException(Strings.AnIndirLaborActivAlreadyExists, "LaborDtl");
                }
            }/* if StartType = "I":U */
            this.GetNewLaborDtl(ref ds, LaborHedSeq);
            ttLaborDtl.EmployeeNum = LaborHed.EmployeeNum;
            ttLaborDtl.LaborType = ((StartType.Compare("R") == 0) ? "P" : StartType);
            ttLaborDtl.LaborTypePseudo = ttLaborDtl.LaborType;
            ttLaborDtl.PayrollDate = LaborHed.PayrollDate;
            ttLaborDtl.ClockInDate = LaborHed.ClockInDate;
            ttLaborDtl.ReWork = (StartType.Compare("R") == 0);
            ttLaborDtl.LaborCollection = true;
            ttLaborDtl.ActiveTrans = true;
            ttLaborDtl.Complete = false;
            ttLaborDtl.OpComplete = false;
            ttLaborDtl.EnableResourceGrpID = ttJCSyst.MachinePrompt;
            ttLaborDtl.JCSystScrapReasons = ttJCSyst.ScrapReasons;
            ttLaborDtl.JCSystReworkReasons = ttJCSyst.ReworkReasons;
            ttLaborDtl.StartActivity = true;
            ttLaborDtl.MES = true;
            ttLaborDtl.RowMod = IceRow.ROWSTATE_ADDED;
            this.clockInTimeMES();
            /* set the display-formatted times */
            this.setDisplayTime();


            if (EmpBasic == null || (EmpBasic != null && !EmpBasic.EmpID.KeyEquals(ttLaborDtl.EmployeeNum)))
                EmpBasic = this.FindFirstEmpBasic22(Session.CompanyID, ttLaborDtl.EmployeeNum);

            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) && EmpBasic != null && EmpBasic.AllowDirLbr == false &&
                ("I,J".Lookup(ttLaborDtl.LaborTypePseudo) == -1))
            {
                throw new BLException(Strings.LaborTypeMustBeIndirEmploIsNotAllowedToBookDirect, "LaborDtl", "LaborType");
            }
            if (EmpBasic != null)
            {
                ttLaborDtl.LaborRate = EmpBasic.LaborRate;
                ttLaborDtl.ExpenseCode = EmpBasic.ExpenseCode;
                if (StartType.Compare("I") == 0)
                {
                    ttLaborDtl.ResourceGrpID = EmpBasic.ResourceGrpID;
                    ttLaborDtl.ResourceID = EmpBasic.ResourceID;

                    ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, EmpBasic.ResourceGrpID);
                    if (ResourceGroup != null)
                    {
                        ttLaborDtl.JCDept = ResourceGroup.JCDept;
                    }


                }
            }
        }

        /// <summary>
        /// Method to call to start an activity in Shop Floor by Employee. 
        /// </summary>
        /// <param name="employeeID">Employee ID </param>
        /// <param name="startType">The type of activity being started.
        /// Values are: P - Production 
        ///             I - Indirect
        ///             S - Setup 
        ///             R - Rework </param>
        /// <param name="ds">The Labor data set </param>
        public void StartActivityByEmp(string employeeID, string startType, ref LaborTableset ds)
        {
            if (string.IsNullOrEmpty(employeeID)) return;

            CurrentFullTableset = ds;
            var laborHead = FindFirstActiveLaborHed(Session.CompanyID, employeeID);
            if (laborHead != null)
            {
                ttLaborHed = new Erp.Tablesets.LaborHedRow();
                CurrentFullTableset.LaborHed.Add(ttLaborHed);
                BufferCopy.Copy(laborHead, ref ttLaborHed);

                this.LaborHedAfterGetRows();
                LaborHed_Foreign_Link();

                StartActivity(laborHead.LaborHedSeq, startType, ref ds);

            }
        }

        /// <summary>
        /// Method to submit Labor for Approval. 
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="lWeeklyView">Is this method being called with WeeklyView records?</param>
        /// <param name="cMessageText">Message text to present to the user after the process is finished </param>
        public void SubmitForApproval(ref LaborTableset ds, bool lWeeklyView, out string cMessageText)
        {
            cMessageText = string.Empty;
            CurrentFullTableset = ds;

            switch (lWeeklyView)
            {
                case true:
                    {
                        // The WeeklyView is a temporary dataset; remove the rows that were not modified to avoid duplicate rows on the Classic screen.
                        foreach (var timeWeeklyRow in (from ttTimeWeeklyView_Row in CurrentFullTableset.TimeWeeklyView
                                                       where String.IsNullOrEmpty(ttTimeWeeklyView_Row.RowMod)
                                                       select ttTimeWeeklyView_Row).ToList())
                        {
                            timeWeeklyRow.RowMod = IceRow.ROWSTATE_DELETED;
                        }

                        //WeeklyView_Loop:
                        foreach (var ttTimeWeeklyView_iterator in (from ttTimeWeeklyView_Row in CurrentFullTableset.TimeWeeklyView
                                                                   where !String.IsNullOrEmpty(ttTimeWeeklyView_Row.RowMod) && ttTimeWeeklyView_Row.RowMod != IceRow.ROWSTATE_DELETED
                                                                   select ttTimeWeeklyView_Row))
                        {
                            ttTimeWeeklyView = ttTimeWeeklyView_iterator;
                            if ("A,P,S".Lookup(ttTimeWeeklyView.TimeStatus) != -1)
                            {
                                // The hours are calculated while refreshing the dataset.
                                ttTimeWeeklyView.RowMod = "";
                                ttTimeWeeklyView.RowSelected = false;
                                ttTimeWeeklyView.HoursSun = 0;
                                ttTimeWeeklyView.HoursMon = 0;
                                ttTimeWeeklyView.HoursTue = 0;
                                ttTimeWeeklyView.HoursWed = 0;
                                ttTimeWeeklyView.HoursThu = 0;
                                ttTimeWeeklyView.HoursFri = 0;
                                ttTimeWeeklyView.HoursSat = 0;
                                ttTimeWeeklyView.HoursTotal = 0;
                                ttTimeWeeklyView.HCMTotWeeklyPayHours = 0;
                                continue;
                            }

                            string timeWeeklyStatus = string.Empty;
                            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                            {
                                LaborDtlParams ipLaborDtlParams2 = new LaborDtlParams();
                                ipLaborDtlParams2.EmployeeNum = ttTimeWeeklyView.EmployeeNum;
                                ipLaborDtlParams2.WeekBeginDate = ttTimeWeeklyView.WeekBeginDate.Value.Date;
                                ipLaborDtlParams2.WeekEndDate = (ttTimeWeeklyView.WeekBeginDate.Value.AddDays(6)).Date;
                                ipLaborDtlParams2.LaborType = ttTimeWeeklyView.LaborType;
                                ipLaborDtlParams2.TimeStatus = ttTimeWeeklyView.TimeStatus;
                                ipLaborDtlParams2.LaborTypePseudo = ttTimeWeeklyView.LaborTypePseudo;
                                ipLaborDtlParams2.ProjectID = ttTimeWeeklyView.ProjectID;
                                ipLaborDtlParams2.PhaseID = ttTimeWeeklyView.PhaseID;
                                ipLaborDtlParams2.TimeTypCd = ttTimeWeeklyView.TimeTypCd;
                                ipLaborDtlParams2.JobNum = ttTimeWeeklyView.JobNum;
                                ipLaborDtlParams2.AssemblySeq = ttTimeWeeklyView.AssemblySeq;
                                ipLaborDtlParams2.OprSeq = ttTimeWeeklyView.OprSeq;
                                ipLaborDtlParams2.IndirectCode = ttTimeWeeklyView.IndirectCode;
                                ipLaborDtlParams2.RoleCd = ttTimeWeeklyView.RoleCd;
                                ipLaborDtlParams2.ResourceGrpID = ttTimeWeeklyView.ResourceGrpID;
                                ipLaborDtlParams2.ResourceID = ttTimeWeeklyView.ResourceID;
                                ipLaborDtlParams2.ExpenseCode = ttTimeWeeklyView.ExpenseCode;
                                ipLaborDtlParams2.Shift = ttTimeWeeklyView.Shift;
                                ipLaborDtlParams2.QuickEntryCode = ttTimeWeeklyView.QuickEntryCode;
                                ipLaborDtlParams2.LaborHrs = decimal.Zero;

                                ttTEKeyRows = null;
                                foreach (var LaborDtl_iterator in (this.SelectWeeklyLaborDtlWithUpdLock2(Session.CompanyID, ipLaborDtlParams2)))
                                {
                                    LaborDtl = LaborDtl_iterator;
                                    ttTEKey = new TimeExpense.ttTEKey();
                                    ttTEKey.Key1 = Compatibility.Convert.ToString(LaborDtl.LaborHedSeq);
                                    ttTEKey.Key2 = Compatibility.Convert.ToString(LaborDtl.LaborDtlSeq);
                                    if (ttTEKeyRows == null)
                                    {
                                        ttTEKeyRows = new List<TimeExpense.ttTEKey>();
                                    }
                                    ttTEKeyRows.Add(ttTEKey);
                                }
                                if (ttTEKeyRows == null)                           //SCR 123833
                                    ttTEKeyRows = new List<TimeExpense.ttTEKey>(); //SCR 123833
                                LibTimeExpenseSubmit._TimeExpenseSubmit(ref ttTEKeyRows, "Time", out cMessageText);
                                this.refreshAfterSubmit(out timeWeeklyStatus);
                                txScope.Complete();
                            }

                            // The hours are calculated while refreshing the dataset.
                            ttTimeWeeklyView.RowMod = "";
                            ttTimeWeeklyView.RowSelected = false;
                            if (!string.IsNullOrEmpty(timeWeeklyStatus))
                                ttTimeWeeklyView.TimeStatus = timeWeeklyStatus;
                            ttTimeWeeklyView.HoursSun = 0;
                            ttTimeWeeklyView.HoursMon = 0;
                            ttTimeWeeklyView.HoursTue = 0;
                            ttTimeWeeklyView.HoursWed = 0;
                            ttTimeWeeklyView.HoursThu = 0;
                            ttTimeWeeklyView.HoursFri = 0;
                            ttTimeWeeklyView.HoursSat = 0;
                            ttTimeWeeklyView.HoursTotal = 0;
                            ttTimeWeeklyView.HCMTotWeeklyPayHours = 0;
                        }

                        // Refresh TimeWeeklyView
                        if (ttTimeWeeklyView != null)
                        {
                            lcEmployeeNum = ttTimeWeeklyView.EmployeeNum;
                            ldCalendarStartDate = ttTimeWeeklyView.WeekBeginDate;
                            ldCalendarEndDate = ttTimeWeeklyView.WeekEndDate;
                        }
                        this.populateTimeWeeklyView();
                    }
                    break;
                default:

                    using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                    {

                        //LaborDtl_Loop:
                        foreach (var ttLaborDtl_iterator in (from ttLaborDtl_Row in CurrentFullTableset.LaborDtl
                                                             where !String.IsNullOrEmpty(ttLaborDtl_Row.RowMod)
                                                             select ttLaborDtl_Row))
                        {
                            ttLaborDtl = ttLaborDtl_iterator;
                            //This is to protect from External services to Report Qty behind scenes.
                            if (ttLaborDtl.LaborEntryMethod.Equals("X", StringComparison.OrdinalIgnoreCase) && (ttLaborDtl.LaborQty + ttLaborDtl.ScrapQty + ttLaborDtl.DiscrepQty) != 0)
                            {
                                throw new BLException(Strings.ReportQtyNotAllowedForTimeAndBackflushOperations, "LaborDtl");
                            }

                            if ("A,P,S".Lookup(ttLaborDtl.TimeStatus) != -1)
                            {
                                ttLaborDtl.RowMod = "";
                                continue;
                            }
                            ttTEKey = new TimeExpense.ttTEKey();
                            ttTEKey.Key1 = Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq);
                            ttTEKey.Key2 = Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                            if (ttTEKeyRows == null)
                            {
                                ttTEKeyRows = new List<TimeExpense.ttTEKey>();
                            }
                            ttTEKeyRows.Add(ttTEKey);

                            ttLaborDtl.RowMod = "";
                            /*SCR 80675 - Turn off the Flag*/
                            if (ttLaborDtl.NewDifDateFlag == 2)
                            {
                                ttLaborDtl.NewDifDateFlag = 0;
                            }
                        }

                        if (ttTEKeyRows == null)                           //SCR 123833
                            ttTEKeyRows = new List<TimeExpense.ttTEKey>(); //SCR 123833

                        LibTimeExpenseSubmit._TimeExpenseSubmit(ref ttTEKeyRows, "Time", out cMessageText);
                        this.refreshAfterSubmit(out string timeWeeklyStatus);
                        txScope.Complete();

                    }
                    break;
            }
        }

        /// <summary>
        /// Total up the burden rates for all resources related to the operation. 
        /// </summary>
        private void sumAllBurdenCost(string ipLabResourceID, string ipLabResourceGrpID, string ipLabCapabilityID, out decimal opTotalBurdenRate)
        {
            opTotalBurdenRate = decimal.Zero;
            decimal dSetupBurRate = decimal.Zero;
            decimal dProdBurRate = decimal.Zero;
            bool isCopyPrimaryOpDtl;
            bool isCopyOpDtl = false;
            int primaryOpDtl;
            JobOpDtl JobOpDtl;

            JobOper = this.FindFirstJobOper28(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
            if (JobOper == null) return;
            primaryOpDtl = ttLaborDtl.LaborType.Compare("S") == 0 ? JobOper.PrimarySetupOpDtl :
                          (ttLaborDtl.LaborType.Compare("P") == 0 ? JobOper.PrimaryProdOpDtl : -1);


            isCopyPrimaryOpDtl = isCopyResCombination(primaryOpDtl, ipLabResourceID, ipLabResourceGrpID, ipLabCapabilityID);

            //JobOpDtl_Loop1:
            foreach (var JobOpDtl_iterator in (this.SelectJobOpDtl3(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq)))
            {
                JobOpDtl = JobOpDtl_iterator;
                if (JobOpDtl.OpDtlSeq == primaryOpDtl)
                {
                    continue;
                }
                /* SCR 94780 - we need to compare if the current combination of ResGrp/Res/Cap in LaborDtl  *
                 * exists in the OpDtl. We need to skip this OpDtl as well so we don't duplicate the burden *
                 * rate for the same combination.                                                           */
                if (ipLabResourceGrpID.Compare(JobOpDtl.ResourceGrpID) == 0 && ipLabResourceID.Compare(JobOpDtl.ResourceID) == 0 &&
                    ipLabCapabilityID.Compare(JobOpDtl.CapabilityID) == 0)
                {
                    continue;
                }

                //ERPS-159703, If the current combination of ResGrp/Res exists in the related RTU and was not from the primary OpDtl and  then skip it 
                if (!isCopyPrimaryOpDtl && !isCopyOpDtl &&
                    isCopyResCombination(JobOpDtl.OpDtlSeq, ipLabResourceID, ipLabResourceGrpID, ipLabCapabilityID))
                {
                    isCopyOpDtl = true;
                    continue;
                }


                /* GO DETERMINE THE BURDEN RATE FOR THIS JOBOPDTL */
                /*AlexMtz - Override rates for the rest of JobOpDtl different from Primary*/
                if (JobOpDtl.OverrideRates == true)
                {
                    dProdBurRate = JobOpDtl.ProdBurRate;
                    dSetupBurRate = JobOpDtl.SetupBurRate;
                }
                else
                {
                    this.calcOpDtlBurdenRate(JobOpDtl.ResourceID, JobOpDtl.ResourceGrpID, JobOpDtl.CapabilityID, ttLaborDtl.LaborRate, out dProdBurRate, out dSetupBurRate);
                }

                /* ADD THE APPRORIATE RATE (SETUP OR PRODUCTION) OF THE JOBOPDTL TO THE TOTAL RATE */
                /* setup */
                if (ttLaborDtl.LaborType.Compare("S") == 0)
                {
                    opTotalBurdenRate = opTotalBurdenRate + dSetupBurRate;
                }
                else
                {
                    opTotalBurdenRate = opTotalBurdenRate + dProdBurRate;
                }
            }
        }

        //ERPS-159703, Compare if JobOpDtl record provides the same Labor combination Resource/ResGroup/Capability using defaultOprSeq2() 
        private bool isCopyResCombination(int opDtlSeq, string ipLabResourceID, string ipLabResourceGrpID, string ipLabCapabilityID)
        {
            if (String.IsNullOrEmpty(ipLabResourceID + ipLabResourceGrpID + ipLabCapabilityID)) return false;
            string outResourceGrpID = "";
            string outResourceID = "";
            string outCapabilityID = "";

            this.defaultOprSeq2(ttLaborDtl.LaborType, ref outResourceGrpID, ref outResourceID, ref outCapabilityID, opDtlSeq);

            return ipLabResourceID.KeyEquals(outResourceID) && ipLabResourceGrpID.KeyEquals(outResourceGrpID) && ipLabCapabilityID.KeyEquals(outCapabilityID);
        }

        /// <summary>
        /// Updates SerialNo and SNTran Records
        /// </summary>
        private void TimeWeeklyView_Foreign_Link()
        {
            bool olApprover = false;
            string ocSalesRepCode = string.Empty;
            RoleCdResult RoleCd = null;
            TimeTypCd = null;
            Indirect = null;
            ProjPhase = null;
            ResourceGroup = null;
            Resource = null;
            LabExpCd = null;


            if (!String.IsNullOrEmpty(ttTimeWeeklyView.TimeTypCd))
            {
                TimeTypCd = this.FindFirstTimeTypCd(Session.CompanyID, ttTimeWeeklyView.TimeTypCd);
            }



            if (!String.IsNullOrEmpty(ttTimeWeeklyView.RoleCd))
            {
                RoleCd = this.FindFirstRoleCd(Session.CompanyID, ttTimeWeeklyView.RoleCd);
            }


            if (!String.IsNullOrEmpty(ttTimeWeeklyView.IndirectCode))
            {
                Indirect = this.FindFirstIndirect3(Session.CompanyID, ttTimeWeeklyView.IndirectCode);
            }


            if (!String.IsNullOrEmpty(ttTimeWeeklyView.PhaseID))
            {
                ProjPhase = this.FindFirstProjPhase(Session.CompanyID, ttTimeWeeklyView.ProjectID, ttTimeWeeklyView.PhaseID);
            }



            if (!String.IsNullOrEmpty(ttTimeWeeklyView.ResourceGrpID))
            {
                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, ttTimeWeeklyView.ResourceGrpID);
            }


            if (!String.IsNullOrEmpty(ttTimeWeeklyView.ResourceID))
            {
                Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, ttTimeWeeklyView.ResourceID);
            }


            if (!String.IsNullOrEmpty(ttTimeWeeklyView.ExpenseCode))
            {
                LabExpCd = this.FindFirstLabExpCd(Session.CompanyID, ttTimeWeeklyView.ExpenseCode);
            }


            if (JCShift == null || JCShift.Shift != ttTimeWeeklyView.Shift)
            {
                JCShift = this.FindFirstJCShift8(Session.CompanyID, ttTimeWeeklyView.Shift);
            }

            ttTimeWeeklyView.TimeTypCdDescription = ((TimeTypCd != null) ? TimeTypCd.Description : "");
            ttTimeWeeklyView.RoleCdDescription = ((RoleCd != null) ? RoleCd.RoleDescription : "");
            ttTimeWeeklyView.IndirectCodeDescription = ((Indirect != null) ? Indirect.Description : "");
            ttTimeWeeklyView.WBSPhaseDesc = ((ProjPhase != null) ? ProjPhase.Description : "");
            ttTimeWeeklyView.ResourceGrpIDDescription = ((ResourceGroup != null) ? ResourceGroup.Description : "");
            ttTimeWeeklyView.ResourceIDDescription = ((Resource != null) ? Resource.Description : "");
            ttTimeWeeklyView.ExpenseCodeDescription = ((LabExpCd != null) ? LabExpCd.Description : "");
            ttTimeWeeklyView.ShiftDescription = ((JCShift != null) ? JCShift.Description : "");



            EmpBasic = this.FindFirstEmpBasic23(Session.CompanyID, ttTimeWeeklyView.EmployeeNum);
            if (EmpBasic != null && EmpBasic.DcdUserID.KeyEquals(Session.UserID))
            {
                if (EmpBasic.DisallowTimeEntry == false)
                {
                    ttTimeWeeklyView.TimeDisableUpdate = false;
                    ttTimeWeeklyView.EnableSubmit = true;
                    ttTimeWeeklyView.EnableRecall = true;
                    ttTimeWeeklyView.EnableCopy = true;
                }
                else
                {
                    ttTimeWeeklyView.TimeDisableUpdate = true;
                    ttTimeWeeklyView.EnableSubmit = false;
                    ttTimeWeeklyView.EnableRecall = false;
                    ttTimeWeeklyView.EnableCopy = false;
                }
            }
            else if (EmpBasic != null && !EmpBasic.DcdUserID.KeyEquals(Session.UserID))
            {


                PlantConfCtrl = this.FindFirstPlantConfCtrl(Session.CompanyID, Session.PlantID);
                if (PlantConfCtrl != null)
                {
                    if (PlantConfCtrl.TimeRestrictEntry == false ||
                    this.getSupervisorRights(ttTimeWeeklyView.EmployeeNum) == true ||
                    this.CanUserUpdateTime(Session.CompanyID, Session.UserID, true))
                    {
                        ttTimeWeeklyView.TimeDisableUpdate = false;
                        ttTimeWeeklyView.EnableSubmit = true;
                        ttTimeWeeklyView.EnableRecall = true;
                        ttTimeWeeklyView.EnableCopy = true;
                    }
                    else
                    {
                        LibCanApproveTE._CanApproveTE("Time", ttTimeWeeklyView.EmployeeNum, ttTimeWeeklyView.ProjectID, ttTimeWeeklyView.PhaseID, ttTimeWeeklyView.LaborType, (ttTimeWeeklyView.LaborTypePseudo.Compare("J") == 0), false, out olApprover, out ocSalesRepCode);
                        if (olApprover == false)
                        {
                            ttTimeWeeklyView.TimeDisableUpdate = true;
                            ttTimeWeeklyView.EnableSubmit = false;
                            ttTimeWeeklyView.EnableRecall = false;
                            ttTimeWeeklyView.EnableCopy = false;
                        }
                        else
                        {
                            ttTimeWeeklyView.TimeDisableUpdate = !PlantConfCtrl.TimeApproverCanUpd;
                            ttTimeWeeklyView.EnableSubmit = PlantConfCtrl.TimeApproverCanUpd;
                            ttTimeWeeklyView.EnableRecall = PlantConfCtrl.TimeApproverCanUpd;
                            ttTimeWeeklyView.EnableCopy = false;
                        }
                    }
                }
            }/* else if available EmpBasic and EmpBasic.DCDUserID <> DCD-USERID */
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) && EmpBasic != null)
            {
                ttTimeWeeklyView.AllowDirLbr = EmpBasic.AllowDirLbr;
            }
        }

        /// <summary>
        /// Updates SerialNo and SNTran Records
        /// </summary>
        private void updateSerialNumbers(string pcid, string lotNum)
        {
            Guid tmpRowID = Guid.Empty;
            int nSNCount = 0;
            const int NSNCountMax = 50;

            /* indirect */
            if (ttLaborDtl.LaborType.Equals("I", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            JobAsmbl = FindFirstJobAsmbl2(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
            if (JobAsmbl == null)
            {
                return;
            }

            int lbrDtlNextAssy = 0;
            int lbrDtlNextOpr = 0;
            string dummy = string.Empty;

            if (ttLaborDtl.EndActivity)
            {
                lbrDtlNextAssy = ttLaborDtl.NextAssemblySeq;
                lbrDtlNextOpr = ttLaborDtl.NextOprSeq;
            }
            else
            {
                if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedMaterialManagement) == true)
                {
                    LibGetNextOprSeq.RunGetNextOprSeq(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, true, out lbrDtlNextOpr, out lbrDtlNextAssy, out dummy);
                }
            }

            string vKey = ttLaborDtl.Company + Ice.Constants.LIST_DELIM + ttLaborDtl.JobNum + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(ttLaborDtl.AssemblySeq) + Ice.Constants.LIST_DELIM + Compatibility.Convert.ToString(ttLaborDtl.OprSeq) + Ice.Constants.LIST_DELIM + Session.SessionID;
            string vChar = LibUsePatchFld.GetPatchFldChar(Session.CompanyID, "jobOper", "autoRecPartTran", vKey);
            if (!String.IsNullOrEmpty(vChar))
            {
                DateTime? PTSysDate = Compatibility.Convert.ToDateTime(vChar.Entry(1, Ice.Constants.LIST_DELIM));
                int PTSysTime = Compatibility.Convert.ToInt32(vChar.Entry(2, Ice.Constants.LIST_DELIM));
                int PTTranNum = Compatibility.Convert.ToInt32(vChar.Entry(3, Ice.Constants.LIST_DELIM));

                PartTran = FindFirstPartTran(ttLaborDtl.Company, PTSysDate.Value, PTSysTime, PTTranNum);
            }

            foreach (var ttLbrScrapSerialNumbers_iterator in (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                                              where ttLbrScrapSerialNumbers_Row.Company.KeyEquals(ttLaborDtl.Company)
                                                              && ttLbrScrapSerialNumbers_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                                                              && ttLbrScrapSerialNumbers_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq
                                                              && !String.IsNullOrEmpty(ttLbrScrapSerialNumbers_Row.RowMod)
                                                              select ttLbrScrapSerialNumbers_Row).ToList())
            {
                ttLbrScrapSerialNumbers = ttLbrScrapSerialNumbers_iterator;
                /* assemblyseq, jobnum, or oprseq changed */
                if (ttLbrScrapSerialNumbers.AssemblySeq != ttLaborDtl.AssemblySeq || !ttLbrScrapSerialNumbers.JobNum.KeyEquals(ttLaborDtl.JobNum) || ttLbrScrapSerialNumbers.OprSeq != ttLaborDtl.OprSeq)
                {
                    SerialNo serialNo = FindFirstSerialNoWithUpdLock(ttLaborDtl.Company, ttLbrScrapSerialNumbers.JobNum, ttLbrScrapSerialNumbers.AssemblySeq, 0, ttLbrScrapSerialNumbers.PartNum, ttLbrScrapSerialNumbers.LaborHedSeq, ttLbrScrapSerialNumbers.LaborDtlSeq, ttLbrScrapSerialNumbers.SerialNumber);
                    if (serialNo != null)
                    {
                        if (ttLbrScrapSerialNumbers.SNStatus.Equals("REJECTED", StringComparison.OrdinalIgnoreCase))
                        {
                            if (snTranExists(ttLbrScrapSerialNumbers.PartNum, ttLbrScrapSerialNumbers.SerialNumber, "ASM-REJ", ttLaborDtl.OprSeq))
                            {
                                LibGetNewSNtran._GetNewSNtran(serialNo, "REJ-ASM", CompanyTime.Today(), out tmpRowID);
                            }
                        }
                        else if (ttLbrScrapSerialNumbers.SNStatus.Equals("INSPECTION", StringComparison.OrdinalIgnoreCase))
                        {
                            if (snTranExists(ttLbrScrapSerialNumbers.PartNum, ttLbrScrapSerialNumbers.SerialNumber, "ASM-INS", ttLaborDtl.OprSeq))
                            {
                                LibGetNewSNtran._GetNewSNtran(serialNo, "INS-ASM", CompanyTime.Today(), out tmpRowID);
                            }
                        }
                        else if (ttLbrScrapSerialNumbers.SNStatus.Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
                        {
                            SNTran = FindLastSNTranWithUpdLock(ttLaborDtl.Company, ttLbrScrapSerialNumbers.PartNum, ttLbrScrapSerialNumbers.SerialNumber, "OPR-CMP");
                            if (SNTran != null)
                            {
                                Db.SNTran.Delete(SNTran);
                            }
                        }

                        serialNo.PrevSNStatus = serialNo.SNStatus;
                        serialNo.ScrapReasonCode = "";
                        serialNo.ScrapLaborDtlSeq = 0;
                        serialNo.ScrapLaborHedSeq = 0;
                        serialNo.NonConfNum = 0;
                        serialNo.SNStatus = getWipOrConsumed(serialNo.PartNum, serialNo.SerialNumber, serialNo.JobNum, serialNo.AssemblySeq, serialNo.MtlSeq, ttLaborDtl.OprSeq, false);

                        /* reset LastLbrOprSeq and NextLbrOprSeq info based on the data for the last OPR-CMP SNTran? */
                        SNTran = FindLastSNTranWithUpdLock2(serialNo.Company, serialNo.PartNum, serialNo.SerialNumber, "OPR-CMP");
                        if (SNTran != null)
                        {
                            SNTran.WareHouseCode = sWhseCode;
                            SNTran.BinNum = sBinNum;
                            serialNo.LastLbrOprSeq = SNTran.LastLbrOprSeq;
                            serialNo.NextLbrAssySeq = SNTran.NextLbrAssySeq;
                            serialNo.NextLbrOprSeq = SNTran.NextLbrOprSeq;
                            serialNo.WareHouseCode = SNTran.WareHouseCode;
                            serialNo.BinNum = SNTran.BinNum;
                        }
                        else
                        {
                            serialNo.LastLbrOprSeq = 0;
                            serialNo.NextLbrAssySeq = 0;
                            serialNo.NextLbrOprSeq = 0;
                        }
                        CurrentFullTableset.LbrScrapSerialNumbers.Remove(ttLbrScrapSerialNumbers);
                    }
                }
                else
                {
                    SerialNo serialNo = FindFirstSerialNoWithUpdLock(ttLaborDtl.Company, ttLbrScrapSerialNumbers.JobNum, ttLbrScrapSerialNumbers.AssemblySeq, 0, ttLbrScrapSerialNumbers.SerialNumber);
                    if (serialNo != null)
                    {
                        if (ttLaborDtl.ReWork)
                        {
                            if (ttLbrScrapSerialNumbers.SNStatus.Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
                            {
                                int vLaborHedSeq = 0;
                                int vLaborDtlSeq = 0;
                                vLaborHedSeq = serialNo.ScrapLaborHedSeq;
                                serialNo.ScrapLaborHedSeq = ttLaborDtl.LaborHedSeq;
                                vLaborDtlSeq = serialNo.ScrapLaborDtlSeq;
                                serialNo.ScrapLaborDtlSeq = ttLaborDtl.LaborDtlSeq;

                                if (!ExistsSNTran3(serialNo.Company, serialNo.PartNum, ttLbrScrapSerialNumbers.JobNum, ttLbrScrapSerialNumbers.AssemblySeq, 0, serialNo.SerialNumber, "OPR-RWK", ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq))
                                {
                                    LibGetNewSNtran._GetNewSNtran(serialNo, "OPR-RWK", CompanyTime.Today(), out tmpRowID, ref SNTran);
                                }
                                else
                                {
                                    SNTran = FindLastSNTranWithUpdLock(serialNo.Company, serialNo.PartNum, serialNo.SerialNumber, ttLaborDtl.OprSeq, "OPR-RWK");
                                }

                                if (SNTran != null)
                                {
                                    SNTran.NextLbrAssySeq = lbrDtlNextAssy;
                                    SNTran.NextLbrOprSeq = lbrDtlNextOpr;
                                    SNTran.WareHouseCode = sWhseCode;
                                    SNTran.BinNum = sBinNum;
                                }

                                serialNo.ScrapLaborHedSeq = vLaborHedSeq;
                                serialNo.ScrapLaborDtlSeq = vLaborDtlSeq;
                                serialNo.Scrapped = false;
                                serialNo.ScrapReasonCode = "";
                                serialNo.NonConfNum = 0;
                            }
                            ttLbrScrapSerialNumbers.RowMod = "";
                        }
                        else
                        {
                            string tranType = string.Empty;
                            /* scrap */
                            if (ttLbrScrapSerialNumbers.SNStatus.Equals("REJECTED", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!serialNo.SNStatus.Equals(ttLbrScrapSerialNumbers.SNStatus, StringComparison.OrdinalIgnoreCase))
                                {
                                    tranType = serialNo.SNStatus;
                                    serialNo.PrevSNStatus = serialNo.SNStatus;
                                }
                                serialNo.SNStatus = ttLbrScrapSerialNumbers.SNStatus;
                                serialNo.ScrapLaborHedSeq = ttLbrScrapSerialNumbers.LaborHedSeq;
                                serialNo.ScrapLaborDtlSeq = ttLbrScrapSerialNumbers.LaborDtlSeq;
                                serialNo.LastLbrOprSeq = ttLaborDtl.OprSeq;
                                serialNo.NonConfNum = 0;
                                serialNo.Scrapped = true;
                                serialNo.ScrapReasonCode = ttLaborDtl.ScrapReasonCode;
                                if (!snTranExists(serialNo.PartNum, serialNo.SerialNumber, "ASM-REJ", ttLaborDtl.OprSeq))
                                {
                                    LibGetNewSNtran._GetNewSNtran(serialNo, "ASM-REJ", CompanyTime.Today(), out tmpRowID);
                                }

                                ttLbrScrapSerialNumbers.RowMod = "";
                            }
                            else if (ttLbrScrapSerialNumbers.SNStatus.Equals("INSPECTION", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!serialNo.SNStatus.Equals(ttLbrScrapSerialNumbers.SNStatus, StringComparison.OrdinalIgnoreCase))
                                {
                                    tranType = serialNo.SNStatus;
                                    serialNo.PrevSNStatus = serialNo.SNStatus;
                                }
                                serialNo.SNStatus = ttLbrScrapSerialNumbers.SNStatus;
                                serialNo.ScrapLaborHedSeq = ttLbrScrapSerialNumbers.LaborHedSeq;
                                serialNo.ScrapLaborDtlSeq = ttLbrScrapSerialNumbers.LaborDtlSeq;
                                serialNo.LastLbrOprSeq = ttLaborDtl.OprSeq;
                                serialNo.Scrapped = false;
                                serialNo.ScrapReasonCode = "";

                                ttLbrScrapSerialNumbers.RowMod = "";

                                NonConf = FindFirstNonConf2(ttLbrScrapSerialNumbers.Company, ttLbrScrapSerialNumbers.LaborHedSeq, ttLbrScrapSerialNumbers.LaborDtlSeq);
                                if (NonConf != null)
                                {
                                    serialNo.NonConfNum = NonConf.TranID;
                                    serialNo.WareHouseCode = NonConf.ToWarehouseCode;
                                    serialNo.BinNum = NonConf.ToBinNum;
                                }

                                if (!snTranExists(serialNo.PartNum, serialNo.SerialNumber, "ASM-INS", ttLaborDtl.OprSeq))
                                {
                                    LibGetNewSNtran._GetNewSNtran(serialNo, "ASM-INS", CompanyTime.Today(), out tmpRowID);
                                }
                            }
                            else if (ttLbrScrapSerialNumbers.SNStatus.Equals("COMPLETE", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!serialNo.SNStatus.Equals(ttLbrScrapSerialNumbers.SNStatus, StringComparison.OrdinalIgnoreCase))
                                {
                                    tranType = serialNo.SNStatus;
                                    serialNo.PrevSNStatus = serialNo.SNStatus;
                                }
                                serialNo.ScrapLaborHedSeq = ttLbrScrapSerialNumbers.LaborHedSeq;
                                serialNo.ScrapLaborDtlSeq = ttLbrScrapSerialNumbers.LaborDtlSeq;
                                serialNo.LastLbrOprSeq = ttLaborDtl.OprSeq;
                                serialNo.Scrapped = false;
                                serialNo.ScrapReasonCode = "";
                                serialNo.NonConfNum = 0;
                                serialNo.SNStatus = getWipOrConsumed(serialNo.PartNum, serialNo.SerialNumber, serialNo.JobNum, serialNo.AssemblySeq, serialNo.MtlSeq, ttLaborDtl.OprSeq, true);
                                serialNo.PCID = pcid;
                                serialNo.LotNum = lotNum;

                                SNTran = FindLastSNTranWithUpdLock5(serialNo.Company, serialNo.PartNum, serialNo.SerialNumber, ttLaborDtl.OprSeq, "OPR-CMP", serialNo.JobNum);
                                if (SNTran == null)
                                {
                                    LibGetNewSNtran._GetNewSNtran(serialNo, "OPR-CMP", CompanyTime.Today(), out tmpRowID, ref SNTran);
                                }

                                if (SNTran != null)
                                {
                                    SNTran.NextLbrAssySeq = lbrDtlNextAssy;
                                    SNTran.NextLbrOprSeq = lbrDtlNextOpr;
                                    SNTran.WareHouseCode = !String.IsNullOrEmpty(sWhseCode) ? sWhseCode : serialNo.WareHouseCode;
                                    SNTran.BinNum = !String.IsNullOrEmpty(sBinNum) ? sBinNum : serialNo.BinNum;
                                }
                                serialNo.NextLbrAssySeq = lbrDtlNextAssy;
                                serialNo.NextLbrOprSeq = lbrDtlNextOpr;
                                ttLbrScrapSerialNumbers.RowMod = "";
                                serialNo.WareHouseCode = !String.IsNullOrEmpty(sWhseCode) ? sWhseCode : serialNo.WareHouseCode;
                                serialNo.BinNum = !String.IsNullOrEmpty(sBinNum) ? sBinNum : serialNo.BinNum;

                                if (!String.IsNullOrEmpty(vChar) && PartTran != null)
                                {
                                    serialNo.WareHouseCode = PartTran.WareHouseCode;
                                    serialNo.BinNum = PartTran.BinNum;
                                    serialNo.PrevSNStatus = serialNo.SNStatus;
                                    serialNo.SNStatus = "INVENTORY";

                                    if (PartTran.TranQty < 0)
                                    {
                                        tranType = PartTran.TranType + "-";
                                    }
                                    else
                                    {
                                        tranType = PartTran.TranType;
                                    }

                                    if (SNTran != null)
                                    {
                                        Db.Validate(SNTran);
                                    }

                                    Guid SNTranRowId = Guid.Empty;
                                    LibGetNewSNtran._GetNewSNtran(serialNo, tranType, PartTran.TranDate, out SNTranRowId);

                                    ttSNTranRows = ttSNTranRows ?? new List<Erp.Internal.IM.PartTranSNtranLink.SNtran2>();

                                    LibPartTranSNtranLink.createttSNtranRecord(SNTranRowId, ref ttSNTranRows, ref ttSNTran);
                                }
                            }
                            else if (ttLbrScrapSerialNumbers.SNStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase))
                            {
                                string sntranType = "OPR-CMP";

                                if (!serialNo.SNStatus.Equals(ttLbrScrapSerialNumbers.SNStatus, StringComparison.OrdinalIgnoreCase))
                                {
                                    tranType = serialNo.SNStatus;
                                    serialNo.PrevSNStatus = serialNo.SNStatus;
                                    if (tranType.KeyEquals("REJECTED"))
                                    {
                                        sntranType = "ASM-REJ";
                                    }
                                }
                                serialNo.SNStatus = ttLbrScrapSerialNumbers.SNStatus;
                                serialNo.ScrapLaborHedSeq = 0;
                                serialNo.ScrapLaborDtlSeq = 0;
                                serialNo.NonConfNum = 0;
                                serialNo.Scrapped = false;
                                serialNo.ScrapReasonCode = "";
                                ttLbrScrapSerialNumbers.RowMod = "D";

                                delttLbrScrapSerialNumber = new LbrScrapSerialNumbersRow();
                                delttLbrScrapSerialNumber = ttLbrScrapSerialNumbers;
                                if (delttLbrScrapSerialNumbersRows == null)
                                    delttLbrScrapSerialNumbersRows = new List<LbrScrapSerialNumbersRow>();
                                delttLbrScrapSerialNumbersRows.Add(delttLbrScrapSerialNumber);

                                SNTran = FindFirstSNTranWithUpdLock(Session.CompanyID, serialNo.PartNum, serialNo.SerialNumber, ttLbrScrapSerialNumbers.LaborHedSeq, ttLbrScrapSerialNumbers.LaborDtlSeq, ttLbrScrapSerialNumbers.OprSeq, sntranType);
                                if (SNTran != null)
                                {
                                    Db.SNTran.Delete(SNTran);
                                }

                                /* reset LastLbrOprSeq and NextLbrOprSeq info based on data for the last OPR-CMP SNTran? */
                                SNTran lastSNTran = FindLastSNTranWithUpdLock3(serialNo.Company, serialNo.PartNum, serialNo.SerialNumber);
                                if (lastSNTran != null)
                                {
                                    lastSNTran.WareHouseCode = sWhseCode;
                                    lastSNTran.BinNum = sBinNum;
                                    serialNo.LastLbrOprSeq = lastSNTran.LastLbrOprSeq;
                                    serialNo.NextLbrAssySeq = lastSNTran.NextLbrAssySeq;
                                    serialNo.NextLbrOprSeq = lastSNTran.NextLbrOprSeq;
                                    serialNo.WareHouseCode = lastSNTran.WareHouseCode;
                                    serialNo.ScrapLaborHedSeq = lastSNTran.ScrapLaborHedSeq;
                                    serialNo.ScrapLaborDtlSeq = lastSNTran.ScrapLaborDtlSeq;
                                    serialNo.BinNum = lastSNTran.BinNum;
                                }
                                else
                                {
                                    serialNo.NextLbrAssySeq = 0;
                                    serialNo.NextLbrOprSeq = 0;
                                }

                                if (!serialNo.SNStatus.Equals(tranType, StringComparison.OrdinalIgnoreCase))
                                {/* was scrap, now wip */
                                    if (tranType.Equals("REJECTED", StringComparison.OrdinalIgnoreCase))
                                    {
                                        SNTran = FindLastSNTran2(ttLaborDtl.Company, ttLbrScrapSerialNumbers.PartNum, ttLbrScrapSerialNumbers.SerialNumber, "ASM-REJ");
                                        if (SNTran != null)
                                        {
                                            serialNo.SNStatus = SNTran.PrevSNStatus;
                                            LibGetNewSNtran._GetNewSNtran(serialNo, "REJ-ASM", CompanyTime.Today(), out tmpRowID);
                                        }
                                    }
                                    else if (tranType.Equals("INSPECTION", StringComparison.OrdinalIgnoreCase))
                                    {
                                        SNTran = FindLastSNTran3(ttLaborDtl.Company, ttLbrScrapSerialNumbers.PartNum, ttLbrScrapSerialNumbers.SerialNumber, "ASM-INS");
                                        serialNo.SNStatus = SNTran.PrevSNStatus;
                                        LibGetNewSNtran._GetNewSNtran(serialNo, "INS-ASM", CompanyTime.Today(), out tmpRowID);
                                    }
                                    else
                                    {
                                        serialNo.SNStatus = getWipOrConsumed(serialNo.PartNum, serialNo.SerialNumber, serialNo.JobNum, serialNo.AssemblySeq, serialNo.MtlSeq, ttLaborDtl.OprSeq, false);
                                    }
                                }
                            }
                        }
                    }
                }

                //Performance - validate a whole group of records                 
                if (nSNCount++ >= NSNCountMax)
                {
                    nSNCount = 0;
                    Db.Validate();
                }

            }/* for each ttLbrScrapSerialNumbers */

            Db.Validate();

            /* vChar is created ONLY when a patchFld record has been created for Autoreceive of Serialized parts. */
            if (!String.IsNullOrEmpty(vChar))
            {
                LibUsePatchFld.DeletePatchFld("jobOper", "autoRecPartTran", vKey);
                if (PartTran != null && ttSNTranRows != null)
                {
                    LibPartTranSNtranLink.createSNTranPartTranLink(PartTran, ref ttSNTranRows);
                }

                CurrentFullTableset.SNFormat.Clear();
            }
        }

        /// <summary>
        /// This method updates the total hour fields in labor hed when labordtl changes
        /// </summary>
        /// <param name="laborHedSeq">LaborHed sequence number</param>
        private void updateTotHours(int laborHedSeq)
        {


            if (!((this.ExistsLaborHed(Session.CompanyID, laborHedSeq))))
            {
                return;
            }



            ttLaborHed = (from ttLaborHed_Row in CurrentFullTableset.LaborHed
                          where ttLaborHed_Row.Company.KeyEquals(Session.CompanyID)
                          && ttLaborHed_Row.LaborHedSeq == laborHedSeq
                          select ttLaborHed_Row).FirstOrDefault();
            if (ttLaborHed == null)
            {


                LaborHed = this.FindFirstLaborHed10(Session.CompanyID, laborHedSeq);
                ttLaborHed = new Erp.Tablesets.LaborHedRow();
                CurrentFullTableset.LaborHed.Add(ttLaborHed);
                BufferCopy.Copy(LaborHed, ref ttLaborHed);
                ttLaborHed.SysRowID = Guid.NewGuid();
                ttLaborHed.SysRowID = LaborHed.SysRowID;
            }
            this.LaborHedAfterGetRows();
            LaborHed_Foreign_Link();
            var outTotLbrHrs2 = ttLaborHed.TotLbrHrs;
            var outTotBurHrs2 = ttLaborHed.TotBurHrs;
            var outTotHCMPayHrs2 = ttLaborHed.HCMTotPayHours;
            this.getDetailHours(ttLaborHed.LaborHedSeq, out outTotLbrHrs2, out outTotBurHrs2, out outTotHCMPayHrs2, ttLaborHed.PayrollValuesForHCM);
            ttLaborHed.TotLbrHrs = outTotLbrHrs2;
            ttLaborHed.TotBurHrs = outTotBurHrs2;
            ttLaborHed.HCMTotPayHours = outTotHCMPayHrs2;

            this.setdispValue();
        }

        /// <summary>
        /// This method validates the Capability code
        /// </summary>
        private void validateCapabilityID(string capCode)
        {
            string tmpStr = string.Empty;



            Capability = this.FindFirstCapability3(Session.CompanyID, capCode);
            if (Capability == null)
            {
                tmpStr = Strings.CapabNotFoundInCompanyPlantID(capCode, Session.CompanyID, Session.PlantID);
                throw new BLException(tmpStr);
            }
        }

        /// <summary>
        /// This method validates the IndirectCode
        /// </summary>
        private void validateIndirect(string pindirectCode, bool update)
        {    /* indirect code */
            if (String.IsNullOrEmpty(pindirectCode))
            {
                throw new BLException(Strings.IndirectCodeIsMandatory, pindirectCode);
            }



            Indirect = this.FindFirstIndirect4(Session.CompanyID, pindirectCode);
            if (Indirect == null)
            {
                throw new BLException(Strings.InvalidIndirectCode, pindirectCode);
            }
            if (update && String.IsNullOrEmpty(ttLaborDtl.ResourceGrpID) && !String.IsNullOrEmpty(ttLaborDtl.IndirectCode))
            {
                throw new BLException(Strings.ResourceGroupIsMandatory, pindirectCode);
            }
        }

        /// <summary>
        /// This method validates the IndirectCode is marked as Downtime
        /// </summary>
        public void ValidateIndirectCodeIsDowntime(string indirectCode)
        {
            Indirect = this.FindFirstIndirect4(Session.CompanyID, indirectCode);
            if (Indirect == null || Indirect.DownTime != true)
            {
                throw new BLException(Strings.InvalidIndirectCode, indirectCode);
            }
        }

        /// <summary>
        /// This method validates the JCDept field 
        /// </summary>
        private void validateJCDept(string ipJCDept)
        {


            if (!((this.ExistsJCDept(Session.CompanyID, ipJCDept))))
            {
                throw new BLException(Strings.LaborCannotBeReporToANonLocatResou);
            }
        }

        /// <summary>
        /// This method validates the JobNumber
        /// </summary>
        private void validateJob(string jobNum, bool dflt, bool newRecord)
        {
            int varAsmSeq = 0;    /* validate job */



            JobHead = this.FindFirstJobHead2(Session.CompanyID, Session.PlantID, jobNum);     /* indirect */
            if (JobHead != null && ((ttLaborDtl != null && ttLaborDtl.LaborType.Compare("I") != 0) || (ttTimeWeeklyView != null && ttTimeWeeklyView.LaborType.Compare("I") != 0)))
            {
                if (JobHead.JobClosed)
                {
                    throw new BLException(Strings.ThisJobHasBeenClosedEntryNotAllowed);
                }
                if (!JobHead.JobReleased &&
                    ((ttLaborDtl != null && ttLaborDtl.EndActivity == false) || ttTimeWeeklyView != null) && /*does not matters EndActivity when validating Job from Weekly view*/
                     newRecord)
                {
                    throw new BLException(Strings.ThisJobHasNotBeenReleaEntryNotAllowed);
                }
                if (((ttLaborDtl != null && String.IsNullOrEmpty(ttLaborDtl.ProjectID)) || (ttTimeWeeklyView != null && String.IsNullOrEmpty(ttTimeWeeklyView.ProjectID))) &&
                    JobHead.JobType.Compare("PRJ") == 0)
                {
                    throw new BLException(Strings.ThisJobIsAJobTypeOfProjectEntryNotAllowedForNon, "LaborDtl");
                }
            }
            /* indirect */
            if (JobHead == null && ((ttLaborDtl != null && ttLaborDtl.LaborType.Compare("I") != 0) || (ttTimeWeeklyView != null && ttTimeWeeklyView.LaborType.Compare("I") != 0)))
            {
                throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.JobNumber));
            }
            /* indirect */
            if (((ttLaborDtl != null && ttLaborDtl.LaborType.Compare("I") != 0) || (ttTimeWeeklyView != null && ttTimeWeeklyView.LaborType.Compare("I") != 0)))
            {
                if (((ttLaborDtl != null && ttLaborDtl.LaborTypePseudo.Compare("V") == 0) || (ttTimeWeeklyView != null && ttTimeWeeklyView.LaborTypePseudo.Compare("V") == 0)) &&
                    JobHead.JobType.Compare("SRV") != 0)
                {
                    throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.ServiceCallJobNumber));
                }
                if (JobHead.JobType.Compare("SRV") == 0)
                {
                    if (dflt && ttLaborDtl != null)
                    {
                        ttLaborDtl.FSComplete = false;
                    }


                    FSCallhd = this.FindFirstFSCallhd(Session.CompanyID, JobHead.CallNum);
                    if (FSCallhd == null)
                    {
                        throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.ServiceCallJobNumber));
                    }
                    if (FSCallhd.Invoiced)
                    {
                        throw new BLException(Strings.TheServiceCallHasBeenInvoiEntryNotAllowed);
                    }
                    if (dflt && ttLaborDtl != null)
                    {
                        ttLaborDtl.FSComplete = FSCallhd.LaborComplete;
                    }
                }
                if (dflt && ttLaborDtl != null)
                {
                    ttLaborDtl.CallNum = JobHead.CallNum;
                    ttLaborDtl.CallLine = JobHead.CallLine;
                }
                varAsmSeq = ((ttLaborDtl != null) ? ttLaborDtl.AssemblySeq : ((ttTimeWeeklyView != null) ? ttTimeWeeklyView.AssemblySeq : 0));



                if (!((this.ExistsJobAsmbl(Session.CompanyID, jobNum, varAsmSeq))))
                {
                    throw new BLException(Strings.InvalidAssemblySequenceNumber);
                }
            }
        }

        private void validateLaborPart()
        {
            string tmpDesc = string.Empty;

            /* validate reasons */
            if (ttJCSyst.ScrapReasons == true)
            {
                if (ttLaborPart.ScrapQty > 0)
                {
                    tmpDesc = this.getReasonDesc(ttLaborPart.ScrapReasonCode, "S");
                    if (String.IsNullOrEmpty(tmpDesc))
                    {
                        throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.ReasonCode));
                    }
                }
                else if (ttLaborPart.ScrapQty < 0)
                {
                    throw new BLException(Strings.ScrapQuantityCannotBeNegative);
                }
            }

            if (ttLaborPart.DiscrepQty > 0)
            {
                tmpDesc = this.getReasonDesc(ttLaborPart.DiscrpRsnCode, "S");
                if (String.IsNullOrEmpty(tmpDesc))
                {
                    throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.ReasonCode));
                }
            }
            else if (ttLaborPart.DiscrepQty < 0)
            {
                throw new BLException(Strings.NonConformQuantCannotBeNegat);
            }
        }

        /// <summary>
        /// This method validates job related fields
        /// </summary>
        private void validateJobFields()
        {
            string tmpDesc = string.Empty;


            if (!String.IsNullOrEmpty(ttLaborDtl.ResourceID) &&
            !((this.ExistsResource(Session.CompanyID, ttLaborDtl.ResourceID))))
            {
                throw new BLException(Strings.InvalideMachineID);
            }
            /* validate reasons */
            if (ttJCSyst.ScrapReasons == true)
            {
                if (ttLaborDtl.ScrapQty > 0 && !ttLaborDtl.ReportPartQtyAllowed)
                {
                    tmpDesc = this.getReasonDesc(ttLaborDtl.ScrapReasonCode, "S");
                    if (String.IsNullOrEmpty(tmpDesc))
                    {
                        throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.ReasonCode));
                    }
                }
                else if (ttLaborDtl.ScrapQty < 0)
                {
                    throw new BLException(Strings.ScrapQuantityCannotBeNegative);
                }
            }

            if (ttLaborDtl.DiscrepQty > 0 && !ttLaborDtl.ReportPartQtyAllowed)
            {
                tmpDesc = this.getReasonDesc(ttLaborDtl.DiscrpRsnCode, "S");
                if (String.IsNullOrEmpty(tmpDesc))
                {
                    throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.ReasonCode));
                }
            }
            else if (ttLaborDtl.DiscrepQty < 0)
            {
                throw new BLException(Strings.NonConformQuantCannotBeNegat);
            }

            if (ttJCSyst.ReworkReasons == true && ttLaborDtl.ReWork)
            {
                tmpDesc = this.getReasonDesc(ttLaborDtl.ReworkReasonCode, "R");
                if (String.IsNullOrEmpty(tmpDesc))
                {
                    throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.ReworkReasonCode));
                }
            }
            /* shopwrn types 10 - 60 are not valid if maintained in this program */



            foreach (var ShopWrn_iterator in (this.SelectShopWrnWithUpdLock(Session.CompanyID, "L", ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, 10, 60)))
            {
                ShopWrn = ShopWrn_iterator;
                Db.ShopWrn.Delete(ShopWrn);
            }
            /* scr #1669 - update service call header */
            if (ttLaborDtl.CallNum > 0)
            {


                FSCallhd = this.FindFirstFSCallhdWithUpdLock(Session.CompanyID, ttLaborDtl.CallNum);
                if (FSCallhd != null && FSCallhd.Invoiced == false)
                {
                    FSCallhd.LaborComplete = ttLaborDtl.FSComplete;
                }

                Db.Release(ref FSCallhd);
            }
        }

        /// <summary>
        /// This method validates the job operation field
        /// </summary>
        private void validateJobOper(string ipLaborType, string ipJobNum, int ipAssemblySeq, int ipOprSeq)
        {
            string VResourceGrpID = string.Empty;
            if (ipLaborType.Compare("I") != 0)
            {


                JobOper = this.FindFirstJobOper29(Session.CompanyID, ipJobNum, ipAssemblySeq, ipOprSeq);
                if (JobOper == null)
                {
                    throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.JobOperationSequence));
                }
                if (JobOper.LaborEntryMethod.Compare("B") == 0)
                {
                    ExceptionManager.AddBLException(Strings.ThisIsABackfOperaEntryNotAllowed);
                }

                if (JobOper.SubContract)
                {
                    ExceptionManager.AddBLException(Strings.JobOperaSubcoSequeEntryNotAllowed);        /* Get the Right default op detail based on Labor Type */
                }



                var JobOpDtlQuery10_Param = ((ipLaborType.Compare("S") == 0) ? JobOper.PrimarySetupOpDtl : JobOper.PrimaryProdOpDtl);
                JobOpDtl = this.FindFirstJobOpDtl5(JobOper.Company, JobOper.JobNum, JobOper.AssemblySeq, JobOper.OprSeq, JobOpDtlQuery10_Param);
                if (JobOpDtl == null)
                {
                    throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.DefaultJobOperationDetail));
                }
                if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.EnhancedQualityAssurance) && ttLaborDtl != null)
                {


                    if ((this.ExistsJobOperInsp2(Session.CompanyID, ipJobNum, ipAssemblySeq, ipOprSeq)) && ttLaborDtl.ReWork == false)
                    {
                        ttLaborDtl.EnableInspection = true;
                    }
                    else
                    {
                        ttLaborDtl.EnableInspection = false;
                    }
                }
            }

            if (ttLaborDtl != null)
            {
                JobAsmblPartResult jobAsmblPart = FindFirstJobAsmblPart(Session.CompanyID, ipJobNum, ipAssemblySeq);
                if (jobAsmblPart != null)
                {
                    ttLaborDtl.PartNum = jobAsmblPart.PartNum;
                    PartPartial pp = FindFirstPartPartial(ttLaborDtl.Company, ttLaborDtl.PartNum);
                    if (pp != null)
                    {
                        ttLaborDtl.AttrClassID = pp.AttrClassID;
                    }
                }
            }
            ExceptionManager.AssertNoBLExceptions();
        }

        /// <summary>
        /// This method validates the Non Conformance value and validates if it has already been processed
        /// </summary>
        public void validateNonConfProcessed(ref LaborTableset ds, int laborHedSeq, int laborDtlSeq, decimal vDiscrepQty, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;
            if (ttLaborDtl == null)
            {
                ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                              where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                              select ttLaborDtl_Row).FirstOrDefault();
            }

            if (laborHedSeq != 0 && laborDtlSeq != 0 && this.ExistsNonConfWithProcessedInspection(Session.CompanyID, laborHedSeq, laborDtlSeq))
            {
                ttLaborDtl.NonConfProcessed = true;
                if (vDiscrepQty != ttLaborDtl.DiscrepQty)
                {
                    ttLaborDtl.DiscrepQty = vDiscrepQty;
                    vMessage = Strings.NonConfAlreadyProcessed;
                }
                else
                {
                    LaborDtl = FindFirstLaborDtl(Session.CompanyID, laborHedSeq, laborDtlSeq);
                    if (LaborDtl != null && LaborDtl.DiscrepQty != vDiscrepQty)
                    {
                        ttLaborDtl.DiscrepQty = LaborDtl.DiscrepQty;
                        vMessage = Strings.NonConfAlreadyProcessed;
                    }
                }
            }
            else
                ttLaborDtl.NonConfProcessed = false;
        }

        private void checkIfNonConfProcessed(int laborHedSeq, int laborDtlSeq)
        {
            if (laborHedSeq != 0 && laborDtlSeq != 0 && this.ExistsNonConfWithProcessedInspection(Session.CompanyID, laborHedSeq, laborDtlSeq))
                ttLaborDtl.NonConfProcessed = true;
        }

        /// <summary>
        /// This method validates the Operation Code
        /// </summary>
        private void validateOpCode(string opCode)
        {    /* operation */



            OpMaster = this.FindFirstOpMaster4(Session.CompanyID, opCode);
            if (OpMaster == null)
            {
                throw new BLException(Strings.InvalidOperationCode);
            }
        }
        private string ValidatePCID(string company, string pcid, string partNum, string lotNum, string uom, string jobNum, bool isNonConformance)
        {
            string opPCID = pcid;

            PackageControlValidations.PkgControlHeaderPartialRow pkgControlHeaderPartialRow = null;
            using (PackageControlValidations libPackageControlValidations = new PackageControlValidations(Db))
            {

                var validPkgControlStageStatuses = ttLaborDtl.WIPTransaction ?
                   (isNonConformance ? new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "EMPTY", "NONCONF" } : new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "EMPTY", "WIPFG", "WIP" }) :
                   new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "EMPTY", "WIPFG" };
                // Validate the Inventory / Staged PCID for the correct statuses
                pkgControlHeaderPartialRow = libPackageControlValidations.ValidatePCID(
                    company,
                    pcid,
                    Session.PlantID,
                    new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "EMPTY" },
                    validPkgControlStageStatuses);

                int selectedSerialNumberCount = (from ttSelectedSerialNumbers_Row in CurrentFullTableset.SelectedSerialNumbers
                                                 where ttSelectedSerialNumbers_Row.Deselected == false
                                                 && !String.IsNullOrEmpty(ttSelectedSerialNumbers_Row.RowMod)
                                                 select ttSelectedSerialNumbers_Row).Count();

                // Validates the AllowMixedParts / AllowMixedLots / AllowMixedUOMS /AllowMixedSerialNumberByPerPCID attributes
                libPackageControlValidations.ValidateAllowAttributes(pkgControlHeaderPartialRow, partNum, lotNum, uom, selectedSerialNumberCount, string.Empty, true, true);
                ExceptionManager.AssertNoBLExceptions();

                // Validate that WIP from only one Job can be in the PCID if it is Labor Print Controlled
                // If the PCID is not Label Print Controlled it can contain WIP from multiple jobs, but then
                // the PCID can only be moved by Move WIP PCID or a Move WIP PCID Request
                if (pkgControlHeaderPartialRow.LabelPrintControlled &&
                    ExistsPkgControlStageItemDifferentSupplyJobNum(pkgControlHeaderPartialRow.Company, pkgControlHeaderPartialRow.PCID, jobNum))
                {
                    throw new BLException(Strings.PCIDContainsADifferentJob);
                }

                // return the PCID retrieved from the db using pcid, to correct entries that might have been made in an alternate case
                if ((pkgControlHeaderPartialRow != null) && pkgControlHeaderPartialRow.PCID.KeyEquals(pcid))
                {
                    opPCID = pkgControlHeaderPartialRow.PCID;
                }
            }
            return opPCID;
        }

        /// <summary>
        /// This method validates the JobNumber
        /// </summary>
        private void validateProject()
        {
            string roleCodeList = string.Empty;
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling))
            {
                return;
            }

            if (!String.IsNullOrEmpty(ttLaborDtl.ProjectID))
            {


                Project = this.FindFirstProject3(ttLaborDtl.Company, ttLaborDtl.ProjectID);
                if (Project == null)
                {
                    throw new BLException(Strings.InvalidProject, "LaborDtl", "ProjectID");
                }
                if (!Project.ActiveProject)
                {
                    throw new BLException(Strings.ProjectMustBeActive, "LaborDtl", "ProjectID");
                }
            }
            /* setup */
            if ((ttLaborDtl.LaborTypePseudo.Compare("J") == 0 && ttLaborDtl.LaborType.Compare("P") != 0) || /* project */
                (ttLaborDtl.LaborTypePseudo.Compare("P") == 0 && ttLaborDtl.LaborType.Compare("P") != 0) || /* production */
                (ttLaborDtl.LaborTypePseudo.Compare("V") == 0 && ttLaborDtl.LaborType.Compare("P") != 0) || /* service */
                (ttLaborDtl.LaborTypePseudo.Compare("I") == 0 && ttLaborDtl.LaborType.Compare("I") != 0) || /* indirect */
                (ttLaborDtl.LaborTypePseudo.Compare("S") == 0 && ttLaborDtl.LaborType.Compare("S") != 0))
            {
                throw new BLException(Strings.InvalidLaborType, "LaborDtl", "LaborType");
            }



            EmpBasic = this.FindFirstEmpBasic22(Session.CompanyID, ttLaborDtl.EmployeeNum);
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) && EmpBasic != null && EmpBasic.AllowDirLbr == false &&
                ("I,J".Lookup(ttLaborDtl.LaborTypePseudo) == -1))
            {
                throw new BLException(Strings.LaborTypeMustBeIndirEmploIsNotAllowedToBookDirect, "LaborDtl", "LaborType");
            }



            JobHead = this.FindFirstJobHead14(ttLaborDtl.Company, ttLaborDtl.JobNum);



            JobOper = this.FindFirstJobOper30(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
            if (String.IsNullOrEmpty(ttLaborDtl.RoleCd) && Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) && JobHead != null && !String.IsNullOrEmpty(JobHead.ProjectID))
            {
                LibProjectCommon.getWBSPhaseMethods(JobHead.ProjectID, JobHead.PhaseID, out vInvMethod, out vRevMethod);
                if (vInvMethod.Compare("TM") == 0)
                {
                    throw new BLException(Strings.TheJobIsForATimeAndMaterProjectOrPhaseRoleCode, "LaborDtl", "RoleCd");
                }
            }
            if (!String.IsNullOrEmpty(ttLaborDtl.RoleCd))
            {
                roleCodeList = this.buildValidRoleCodeList(ttLaborDtl.EmployeeNum, JobHead.ProjectID, JobHead.PhaseID, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                if (roleCodeList.Lookup(ttLaborDtl.RoleCd, Ice.Constants.LIST_DELIM[0]) <= -1)
                {
                    throw new BLException(Strings.RoleCodeIsNotValidForThisEmploAndOpera, "LaborDtl", "RoleCd");
                }
            }
            if ((ttLaborDtl.LaborType.Compare("I") == 0) &&
                (!String.IsNullOrEmpty(ttLaborDtl.TimeTypCd)))
            {
                throw new BLException(Strings.TimeTypeCodeCannotBeEnteredForIndirLabor, "LaborDtl", "TimeTypCd");
            }
            this.validateTimeTypCd(ttLaborDtl.JobNum, ttLaborDtl.RoleCd, ttLaborDtl.TimeTypCd, ttLaborDtl.EmployeeNum);
            if (ttLaborDtl.PBInvNum != 0 && BIttLaborDtl != null && BIttLaborDtl.RoleCd.Compare(ttLaborDtl.RoleCd) != 0)
            {
                throw new BLException(Strings.ThisEntryHasAlreadyBeenProceByProjectBillingRole, "LaborDtl", "RoleCd");
            }
            if (ttLaborDtl.PBInvNum != 0 && BIttLaborDtl != null && BIttLaborDtl.TimeTypCd.Compare(ttLaborDtl.TimeTypCd) != 0)
            {
                throw new BLException(Strings.ThisEntryHasAlreadyBeenProceByProjectBillingTime, "LaborDtl", "TimeTypCd");
            }
        }

        /// <summary>
        /// this method validates if the Project linked to the Job in Labor Detail is closed.
        /// </summary>
        public bool ValidateProjectClosed(string projectID, string jobNum, string laborTypePseudo)
        {
            bool _success = true;

            _success = ValidateProjectClosedRecallCopy(projectID, jobNum, laborTypePseudo);

            return _success;
        }


        private bool ValidateProjectClosedRecallCopy(string projectID, string jobNum, string laborTypePseudo)
        {
            bool _success = true;

            if (laborTypePseudo.Equals("P", StringComparison.OrdinalIgnoreCase) ||
                    laborTypePseudo.Equals("V", StringComparison.OrdinalIgnoreCase) ||
                    laborTypePseudo.Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(jobNum))
                {
                    var JobHeadProjectResult = this.FindFirstJobHeadProject(Session.CompanyID, jobNum);
                    if (JobHeadProjectResult != null)
                    {
                        if (!string.IsNullOrEmpty(JobHeadProjectResult.ProjectID) && !this.ExistsActiveProject(Session.CompanyID, JobHeadProjectResult.ProjectID))
                        {
                            _success = false;
                        }
                    }
                }
            }
            else if (laborTypePseudo.Equals("J", StringComparison.OrdinalIgnoreCase) || laborTypePseudo.Equals("Expense", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(projectID) && !this.ExistsActiveProject(Session.CompanyID, projectID))
                {
                    _success = false;
                }
            }
            return _success;
        }

        /// <summary>
        /// Validates before calling SN selection screen - expects ttLaborDtl row
        /// </summary>
        private void validateSerial()
        {
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.TtLaborDtlRecordNotFound, "ttLaborDtl", "RowMod");
            }

            if (!ttLaborDtl.EnableSN)
            {
                return;
            }

            JobAsmbl = FindFirstJobAsmbl4(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
            if (JobAsmbl == null)
            {
                throw new BLException(Strings.JobAsmblRecordNotFound, "JobAsmbl", "JobAsmbl");
            }

            int noncomCount = 0;
            int scrapCount = 0;
            int compCount = 0;
            bool useAsComplete = false;

            foreach (var ttLbrScrapSerialNumbers_iterator in (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                                              where ttLbrScrapSerialNumbers_Row.Company.KeyEquals(ttLaborDtl.Company)
                                                              && ttLbrScrapSerialNumbers_Row.JobNum.KeyEquals(ttLaborDtl.JobNum)
                                                              && ttLbrScrapSerialNumbers_Row.AssemblySeq == ttLaborDtl.AssemblySeq
                                                              && ttLbrScrapSerialNumbers_Row.OprSeq == ttLaborDtl.OprSeq
                                                              && ttLbrScrapSerialNumbers_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                                                              && ttLbrScrapSerialNumbers_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq
                                                              && !String.IsNullOrEmpty(ttLbrScrapSerialNumbers_Row.RowMod)
                                                              select ttLbrScrapSerialNumbers_Row))
            {
                ttLbrScrapSerialNumbers = ttLbrScrapSerialNumbers_iterator;

                if (String.IsNullOrEmpty(ttLbrScrapSerialNumbers.SerialNumber))
                {
                    throw new BLException(Strings.SerialNumberCannotBeBlank, "LbrScrapSerialNumbers");
                }

                /* Check for duplicates */
                int dupeCheck = (from altLbrSSN_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                 where ttLbrScrapSerialNumbers.Company.KeyEquals(ttLaborDtl.Company)
                                 && altLbrSSN_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                                 && altLbrSSN_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq
                                 && altLbrSSN_Row.JobNum.KeyEquals(ttLaborDtl.JobNum)
                                 && altLbrSSN_Row.AssemblySeq == ttLaborDtl.AssemblySeq
                                 && altLbrSSN_Row.OprSeq == ttLaborDtl.OprSeq
                                 && !altLbrSSN_Row.SNStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase)
                                 && !String.IsNullOrEmpty(altLbrSSN_Row.RowMod)
                                 && altLbrSSN_Row.SerialNumber.KeyEquals(ttLbrScrapSerialNumbers.SerialNumber)
                                 select altLbrSSN_Row).Count();

                if (dupeCheck > 1)
                {
                    throw new BLException(Strings.MoreThanOneEntryForIsNotAllowed(ttLbrScrapSerialNumbers.SerialNumber), "LbrScrapSerialNumbers");
                }

                /* Check for valid serial number for job/asm/part and status */
                SerialNo serialNo = FindFirstSerialNo(ttLaborDtl.Company, JobAsmbl.PartNum, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, 0, JobAsmbl.PartNum, ttLbrScrapSerialNumbers.SerialNumber);
                if (serialNo == null)
                {
                    throw new BLException(Strings.SerialNumberIsNotValidForThisJobAndPart(ttLbrScrapSerialNumbers.SerialNumber), "LbrScrapSerialNumbers");
                }
                else if (!serialNo.SNStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase)
                    && !serialNo.SNStatus.Equals("CONSUMED", StringComparison.OrdinalIgnoreCase)
                    && !serialNo.SNStatus.Equals("REJECTED", StringComparison.OrdinalIgnoreCase)
                    && !serialNo.SNStatus.Equals("INSPECTION", StringComparison.OrdinalIgnoreCase)
                    && (!ttLaborDtl.ReWork || (ttLaborDtl.ReWork && !serialNo.SNStatus.Equals("INVENTORY", StringComparison.OrdinalIgnoreCase))))
                {
                    if (ttLaborDtl.EndActivity
                        && !ttLaborDtl.ReWork
                        && serialNo.SNStatus.Equals("INVENTORY", StringComparison.OrdinalIgnoreCase)
                        && ttLbrScrapSerialNumbers.Selected == false
                        && ExistsSNTran2(serialNo.Company, serialNo.JobNum, serialNo.AssemblySeq, 0, serialNo.PartNum, serialNo.SerialNumber, "OPR-CMP", ttLaborDtl.OprSeq, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq))
                    {
                        // this logic is meant to allow serial numbers that were entered for this laborDtl in ReportQty, and then before being entered in EndActivity they were received to Inventory
                        // (see SCRs 143112 and 137551), in this case there would already be an OPR-CMP SNTran for this same LaborDtl record. 
                        // If the SN is assigned to the job but does not have any OPR-CMP and has status INVENTORY, it should not be allowed to be selected in End Activity, 
                        // same as it would be prevented from being selected in the Time and Expense form (see ERPS-147121).
                        useAsComplete = true;
                    }
                    else
                    {
                        throw new BLException(Strings.SerialNumberAlreadyHasAStatusOtherThanWIPScrap(ttLbrScrapSerialNumbers.SerialNumber), "LbrScrapSerialNumbers");
                    }
                }
                else if (!ttLaborDtl.ReWork && (serialNo.LastLbrOprSeq == ttLaborDtl.OprSeq && (serialNo.ScrapLaborHedSeq != ttLaborDtl.LaborHedSeq || serialNo.ScrapLaborDtlSeq != ttLaborDtl.LaborDtlSeq)))
                {
                    throw new BLException(Strings.SerialNumberHasAlreadyBeenAssigToThisOpera(ttLbrScrapSerialNumbers.SerialNumber), "LbrScrapSerialNumbers");
                }
                else if ((ttLaborDtl.ReWork && !ttLaborDtl.EndActivity) && (serialNo.LastLbrOprSeq != ttLaborDtl.OprSeq || (serialNo.ScrapLaborHedSeq == ttLaborDtl.LaborHedSeq && serialNo.ScrapLaborDtlSeq == ttLaborDtl.LaborDtlSeq)))
                {
                    throw new BLException(Strings.SerialNumberHasNotBeenAssigToThisOp(ttLbrScrapSerialNumbers.SerialNumber), "LbrScrapSerialNumbers");
                }
                /* validate existing REJECTED status */
                if (serialNo.SNStatus.Equals("REJECTED", StringComparison.OrdinalIgnoreCase))
                {        /* updating or correcting an existing record  - if it is the same operation then we will allow it to be changed here*/
                    if (ttLbrScrapSerialNumbers.RowMod.Compare("A") == 0)
                    {
                        if (serialNo.DMRNum != 0)
                        {
                            throw new BLException(Strings.SerialNumberAlreadyRejecByDMR(ttLbrScrapSerialNumbers.SerialNumber, serialNo.DMRNum), "LbrScrapSerialNumbers");
                        }
                        if (serialNo.Scrapped == true)
                        {
                            throw new BLException(Strings.SerialNumberWasScrapOnOpera(ttLbrScrapSerialNumbers.SerialNumber, serialNo.LastLbrOprSeq), "LbrScrapSerialNumbers");
                        }
                    }
                    else if (ttLbrScrapSerialNumbers.RowMod.Compare("U") == 0)
                    {
                        if (ttLbrScrapSerialNumbers.OprSeq != serialNo.LastLbrOprSeq && serialNo.LastLbrOprSeq != 0)
                        {
                            throw new BLException(Strings.SerialNumberWasScrapOnOpera(ttLbrScrapSerialNumbers.SerialNumber, serialNo.LastLbrOprSeq), "LbrScrapSerialNumbers");
                        }
                    }
                }
                /* inspection has to be 'undone' via inspection or DMR */
                if (serialNo.SNStatus.Equals("INSPECTION", StringComparison.OrdinalIgnoreCase) && !ttLbrScrapSerialNumbers.SNStatus.KeyEquals("INSPECTION"))
                {
                    throw new BLException(Strings.SerialNumberIsInInspeUnderNonCoNumber(ttLbrScrapSerialNumbers.SerialNumber, serialNo.NonConfNum), "LbrScrapSerialNumbers");
                }

                if (ttLbrScrapSerialNumbers.SNStatus.Equals("WIP", StringComparison.OrdinalIgnoreCase))
                {
                    if (ExistsSNTran(serialNo.Company, serialNo.JobNum, serialNo.AssemblySeq, 0, serialNo.PartNum, serialNo.SerialNumber, "OPR-RWK", ttLaborDtl.OprSeq))
                    {
                        throw new BLException(Strings.SerialNumberHasAlreadyBeenAssigToThisOperaRework(ttLbrScrapSerialNumbers.SerialNumber), "LbrScrapSerialNumbers");
                    }
                }

                if (!ttLaborDtl.ReWork && !String.IsNullOrEmpty(serialNo.PCID))
                {
                    throw new BLException(Strings.CannotSelectSerialNoWithPCID(ttLbrScrapSerialNumbers.SerialNumber), "LbrScrapSerialNumbers");
                }

                if (ttLbrScrapSerialNumbers.SNStatus.Equals("INSPECTION", StringComparison.OrdinalIgnoreCase))
                {
                    noncomCount += 1;
                }

                if (ttLbrScrapSerialNumbers.SNStatus.Equals("REJECTED", StringComparison.OrdinalIgnoreCase))
                {
                    scrapCount += 1;
                }

                if (ttLbrScrapSerialNumbers.SNStatus.Equals("COMPLETE", StringComparison.OrdinalIgnoreCase) || useAsComplete)
                {
                    compCount += 1;
                    useAsComplete = false;
                }
            } /* for each ttLbrScrapSerialNumbers */

            /* count unmodified serial numbers already out there for this LaborDtl */
            int attributeSetID = ttLaborDtl.LaborAttributeSetID > 0 ? ttLaborDtl.LaborAttributeSetID : ttLaborDtl.ScrapAttributeSetID > 0 ? ttLaborDtl.ScrapAttributeSetID : ttLaborDtl.DiscrepAttributeSetID > 0 ? ttLaborDtl.DiscrepAttributeSetID : 0;
            foreach (SerialNo serialNo in SelectSerialNo(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, 0, JobAsmbl.PartNum, attributeSetID))
            {
                ttLbrScrapSerialNumbers = (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                           where ttLbrScrapSerialNumbers_Row.PartNum.KeyEquals(serialNo.PartNum)
                                           && ttLbrScrapSerialNumbers_Row.SerialNumber.KeyEquals(serialNo.SerialNumber)
                                           && !String.IsNullOrEmpty(ttLbrScrapSerialNumbers_Row.RowMod)
                                           select ttLbrScrapSerialNumbers_Row).FirstOrDefault();
                if (ttLbrScrapSerialNumbers == null)
                {
                    if (serialNo.SNStatus.Equals("INSPECTION", StringComparison.OrdinalIgnoreCase))
                    {
                        noncomCount += 1;
                    }

                    if (serialNo.SNStatus.Equals("REJECTED", StringComparison.OrdinalIgnoreCase))
                    {
                        scrapCount += 1;
                    }
                }
            }

            foreach (var SNTran_iterator in SelectSNTran(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, (ttLaborDtl.ReWork ? "OPR-RWK" : "OPR-CMP"), ttLaborDtl.OprSeq))
            {
                SNTran = SNTran_iterator;

                ttLbrScrapSerialNumbers = (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                           where ttLbrScrapSerialNumbers_Row.PartNum.Compare(SNTran.PartNum) == 0
                                           && ttLbrScrapSerialNumbers_Row.SerialNumber.Compare(SNTran.SerialNumber) == 0
                                           && !String.IsNullOrEmpty(ttLbrScrapSerialNumbers_Row.RowMod)
                                           select ttLbrScrapSerialNumbers_Row).FirstOrDefault();
                if (ttLbrScrapSerialNumbers == null)
                {
                    compCount += 1;
                }
            }
            /* Check counts versus qtys */
            if (ttLaborDtl.ScrapQty > scrapCount)
            {
                throw new BLException(Strings.NotEnoughScrapSerialNumbersSpeci, "LbrScrapSerialNumbers");
            }
            if (ttLaborDtl.ScrapQty < scrapCount)
            {
                throw new BLException(Strings.TooManyScrapSerialNumbersSpeci, "LbrScrapSerialNumbers");
            }
            if (ttLaborDtl.DiscrepQty > noncomCount)
            {
                throw new BLException(Strings.NotEnoughNoncoSerialNumbersSpeci, "LbrScrapSerialNumbers");
            }
            if (ttLaborDtl.DiscrepQty < noncomCount)
            {
                throw new BLException(Strings.TooManyNoncoSerialNumbersSpeci, "LbrScrapSerialNumbers");
            }
            if (ttLaborDtl.LaborQty > compCount)
            {
                throw new BLException(Strings.NotEnoughComplSerialNumbersSpeci, "LbrScrapSerialNumbers");
            }
            if (ttLaborDtl.LaborQty < compCount)
            {
                throw new BLException(Strings.TooManyComplSerialNumbersSpeci, "LbrScrapSerialNumbers");
            }
        }

        /// <summary>
        /// Validates after calling SN selection screen
        /// </summary>
        /// <param name="ds">Labor data set</param>
        public void ValidateSerialAfterSelect(ref LaborTableset ds)
        {
            CurrentFullTableset = ds;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where !String.IsNullOrEmpty(ttLaborDtl_Row.RowMod)
                          select ttLaborDtl_Row).FirstOrDefault();

            validateSerial();
        }

        /// <summary>
        /// Validates if serial number is valid after selecting SN on scan interface kinetic MES
        /// </summary>
        /// <param name="jobNum">JobNum of the SN</param>
        /// <param name="assemblySeq">AssemblySeq of the SN</param>
        /// <param name="partNum">PartNum of the SN</param>
        /// <param name="proposedSN">Proposed SN</param>
        /// <param name="oprSeq">Operation sequence of the labor detail</param>
        /// <param name="laborHedSeq">Labor Head sequence of the labor detail</param>
        /// <param name="laborDtlSeq">Labor Detail sequence of the labor detail</param>
        /// <param name="rework">Indicates if labor record is rework</param>
        public void ValidateSerialScanInterface(string jobNum, int assemblySeq, string partNum, string proposedSN, int oprSeq, int laborHedSeq, int laborDtlSeq, bool rework)
        {
            JobAsmbl = FindFirstJobAsmbl4(Session.CompanyID, jobNum, assemblySeq);
            if (JobAsmbl == null)
            {
                throw new BLException(Strings.JobAsmblRecordNotFound, "JobAsmbl", "JobAsmbl");
            }
            if (String.IsNullOrEmpty(proposedSN))
            {
                throw new BLException(Strings.SerialNumberCannotBeBlank, "LbrScrapSerialNumbers");
            }
            SerialNo serialNo = FindFirstSerialNo(Session.CompanyID, partNum, jobNum, assemblySeq, 0, partNum, proposedSN);
            if (serialNo == null)
            {
                throw new BLException(Strings.SerialNumberIsNotValidForThisJobAndPart(proposedSN), "LbrScrapSerialNumbers");
            }
            if (!rework && (serialNo.LastLbrOprSeq == oprSeq && (serialNo.ScrapLaborHedSeq != laborHedSeq || serialNo.ScrapLaborDtlSeq != laborDtlSeq)))
            {
                throw new BLException(Strings.SerialNumberHasAlreadyBeenAssigToThisOpera(proposedSN), "LbrScrapSerialNumbers");
            }
        }

        /// <summary>
        /// Validates the number of available serial numbers - expects current ttLaborDtl
        /// </summary>
        private void validateSerialAvail(int attributeSetID, out string snErrorMsg, out decimal tot_NeedSN, out decimal tot_NeedSNNew)
        {
            snErrorMsg = string.Empty;
            tot_NeedSN = decimal.Zero;
            tot_NeedSNNew = decimal.Zero;

            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.TtLaborDtlRecordNotFound, "ttLaborDtl", "RowMod");
            }

            if (!ttLaborDtl.EnableSN)
            {
                return;
            }

            JobAsmbl = FindFirstJobAsmbl3(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
            if (JobAsmbl == null)
            {
                throw new BLException(Strings.JobAsmblRecordNotFound, "JobAsmbl", "AssemblySeq");
            }

            decimal tot_priorNeedSN = decimal.Zero;
            decimal tot_Scrap = ttLaborDtl.ScrapQty;
            decimal tot_Noncom = ttLaborDtl.DiscrepQty;
            decimal tot_Labor = ttLaborDtl.LaborQty;
            decimal tot_AvailSN = decimal.Zero;
            decimal tot_PrevLabor = decimal.Zero;
            tot_NeedSN = tot_Scrap + tot_Noncom + tot_Labor;

            LaborDtl altLaborDtl = LaborDtl.FindFirstByPrimaryKey(Db, Session.CompanyID, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq);
            if (ttLaborDtl.EndActivity && altLaborDtl != null)
            {
                tot_PrevLabor = altLaborDtl.LaborQty;
            }

            if (altLaborDtl != null)
            {
                tot_priorNeedSN = altLaborDtl.ScrapQty + altLaborDtl.DiscrepQty + altLaborDtl.LaborQty;
                if (tot_priorNeedSN >= tot_NeedSN)
                {
                    return;
                }
            }

            if (!ttLaborDtl.ReWork && ttLaborDtl.LaborDtlSeq > 0)
            {
                tot_AvailSN = CountSerialNoNew(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, 0, JobAsmbl.PartNum, attributeSetID, 0, false, 0, 0, "");
                if (tot_AvailSN >= tot_NeedSN)
                {
                    return;
                }
            }
            tot_AvailSN = 0;

            bool thisOprHasSNTran = false;
            int tot_CompletedSN = 0;
            int tot_Scrapped = 0;
            int tot_NonConformance = 0;
            int subtractFromTotNeed = 0;

            foreach (SerialNo serialNo in SelectSerialNo(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, 0, JobAsmbl.PartNum, attributeSetID))
            {
                thisOprHasSNTran = false;    /*Non-scrapped serial numbers have been entered for this operation.*/

                /*Non-scrapped but inspected serial numbers have been entered for this operation*/
                if (ExistsSNTran(serialNo.Company, "OPR-CMP", serialNo.PartNum, serialNo.SerialNumber, serialNo.JobNum, serialNo.AssemblySeq, 0, ttLaborDtl.OprSeq, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq))
                {
                    tot_CompletedSN += 1;
                }
                else
                {
                    if (ExistsSNTran(serialNo.Company, serialNo.JobNum, serialNo.AssemblySeq, 0, serialNo.PartNum, serialNo.SerialNumber, "ASM-INS", ttLaborDtl.OprSeq, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq))
                    {
                        tot_NonConformance += 1;
                    }
                    else
                    {
                        if (serialNo.Scrapped)
                        {
                            if (ExistsSNTran2(serialNo.Company, serialNo.JobNum, serialNo.AssemblySeq, 0, serialNo.PartNum, serialNo.SerialNumber, "ASM-REJ", ttLaborDtl.OprSeq, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq))
                            {
                                tot_AvailSN += 1;
                            }
                            else
                            {
                                tot_Scrapped += 1;
                            }
                        }
                        else
                        {
                            tot_AvailSN += 1;
                        }
                    }
                }

                if (ExistsSNTran(serialNo.Company, "OPR-CMP", serialNo.PartNum, serialNo.SerialNumber, serialNo.JobNum, serialNo.AssemblySeq, 0, ttLaborDtl.OprSeq))
                {
                    thisOprHasSNTran = true;
                }
                else if (ExistsSNTran(serialNo.Company, serialNo.JobNum, serialNo.AssemblySeq, 0, serialNo.PartNum, serialNo.SerialNumber, "ASM-INS", ttLaborDtl.OprSeq))
                {
                    thisOprHasSNTran = true;
                }

                if (thisOprHasSNTran == false)
                {
                    if (serialNo.Scrapped == false && serialNo.Voided == false && ExistsSNTran(serialNo.Company, serialNo.PartNum, serialNo.JobNum, serialNo.AssemblySeq, 0, serialNo.SerialNumber, "OPR-CMP", ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq))
                    {
                        subtractFromTotNeed += 1;
                    }
                    else if (ExistsSNTran2(serialNo.Company, serialNo.PartNum, serialNo.JobNum, serialNo.AssemblySeq, 0, serialNo.SerialNumber, "ASM-INS", ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq))
                    {
                        subtractFromTotNeed += 1;
                    }
                }
            }

            /* Should not prompt the user to create more Serial Numbers if already created for current labor */
            if ((!ttLaborDtl.ReWork && tot_AvailSN < tot_NeedSN) || (ttLaborDtl.ReWork && tot_CompletedSN < tot_NeedSN))
            {
                tot_NeedSNNew = tot_NeedSN - tot_AvailSN;
                snErrorMsg = ttLaborDtl.ReWork ? Strings.NotEnoughSerialNumbersToBeSelectAsRW(tot_CompletedSN) : Strings.NotEnoughSerialNumbersAssigToJobNeedMoreSerial((int)(tot_NeedSNNew));

                /* set the tot-NeedSN = the total extra we need since this is the value sent to SerialNoAssign.p which adds back in all of the 
                   currently assigned SN which includes priors for this laborDtl record and any others */
                if (!ttLaborDtl.ReWork)
                {
                    tot_NeedSN = tot_NeedSN - tot_priorNeedSN - subtractFromTotNeed;
                }
            }
        }

        /// <summary>
        /// Call before allowing the select of serial numbers
        /// </summary>
        /// <param name="ds">Labor data set</param>
        /// <param name="notEnoughSerials">Not enough serials for labor entry</param>
        /// <param name="totSNReq">Total number of serials required for labor entry</param>
        /// <param name="totNewSNReq">Number of new serial numbers needed</param>
        public void ValidateSerialBeforeSelect(ref LaborTableset ds, out string notEnoughSerials, out decimal totSNReq, out decimal totNewSNReq)
        {
            notEnoughSerials = string.Empty;
            totSNReq = decimal.Zero;
            totNewSNReq = decimal.Zero;
            int chkAttrSetID = 0;

            CurrentFullTableset = ds;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where !String.IsNullOrEmpty(ttLaborDtl_Row.RowMod)
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.TtLaborDtlRecordNotFound, "ttLaborDtl", "RowMod");
            }

            JobHead jobHead = FindFirstJobHead15(ttLaborDtl.Company, ttLaborDtl.JobNum);
            if (jobHead == null)
            {
                throw new BLException(Strings.JobNotFound, "JobHead", "JobNum");
            }

            if (LibSerialCommon.isSerTraPlantType(jobHead.Plant) == SerialCommon.SerialTracking.None)
            {
                throw new BLException(Strings.SerialTrackIsNotEnabledInThisPlant, "PlantConfCtrl", "SerialTracking");
            }

            JobAsmbl jobAsmbl = FindFirstJobAsmbl5(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
            if (jobAsmbl != null)
            {
                if (!ExistsPartTrackSerialNum(Session.CompanyID, jobAsmbl.PartNum, true))
                {
                    throw new BLException(Strings.PartNumberIsNotSerialTracked, "Part", "TrackSerialNum");
                }
            }

            decimal tot_Qty = ttLaborDtl.LaborQty + ttLaborDtl.ScrapQty + ttLaborDtl.DiscrepQty;
            if (tot_Qty == 0)
            {
                throw new BLException(Strings.YouMustSpecifyAQuantity, "ttLaborDtl", "LaborQty");
            }

            if (Math.Truncate(tot_Qty) != tot_Qty)
            {
                throw new BLException(Strings.TheTotalCombiQuantMustBeAWholeNumber(tot_Qty), "ttLaborDtl");
            }

            var PartPartial = FindFirstPartPartial(Session.CompanyID, ttLaborDtl.PartNum);
            if (PartPartial != null && PartPartial.TrackInventoryAttributes)
            {
                if (ttLaborDtl.LaborQty > 0 && ttLaborDtl.LaborAttributeSetID == 0 ||
                    ttLaborDtl.ScrapQty > 0 && ttLaborDtl.ScrapAttributeSetID == 0 ||
                    ttLaborDtl.DiscrepQty > 0 && ttLaborDtl.DiscrepAttributeSetID == 0)
                {
                    throw new BLException(Strings.AttributeSetsMustBeEnteredForAllQtys);
                }

                if (ttLaborDtl.LaborQty > 0)
                {
                    chkAttrSetID = ttLaborDtl.LaborAttributeSetID;
                }
                if (ttLaborDtl.ScrapQty > 0)
                {
                    if (chkAttrSetID == 0)
                        chkAttrSetID = ttLaborDtl.ScrapAttributeSetID;
                    else
                        if (chkAttrSetID != ttLaborDtl.ScrapAttributeSetID)
                    {
                        throw new BLException(Strings.AllAttrSetsMustBeTheSame);
                    }
                }
                if (ttLaborDtl.DiscrepQty > 0)
                {
                    if (chkAttrSetID == 0)
                        chkAttrSetID = ttLaborDtl.DiscrepAttributeSetID;
                    else if (chkAttrSetID != ttLaborDtl.DiscrepAttributeSetID)
                    {
                        throw new BLException(Strings.AllAttrSetsMustBeTheSame);
                    }
                }

            }
            if (PartPartial != null && !PartPartial.TrackInventoryAttributes && PartPartial.TrackInventoryByRevision)
            {
                chkAttrSetID = ttLaborDtl.LaborAttributeSetID;
            }
            validateSerialAvail(chkAttrSetID, out notEnoughSerials, out totSNReq, out totNewSNReq);
            if (ttLaborDtl.ReWork && !string.IsNullOrEmpty(notEnoughSerials))
            {
                throw new BLException(notEnoughSerials, "ttLaborDtl");
            }
        }
        private string validateTimeTypCd(string ipJobNum, string ipRoleCd, string ipTimeTypCd, string ipEmployeeNum)
        {
            Erp.Tables.JobHead timJobHead = null;
            Erp.Tables.Project timProject = null;
            bool gotRate = false;
            string rateErrMsg = string.Empty;


            timJobHead = this.FindFirstJobHead16(Session.CompanyID, ipJobNum);
            if (timJobHead == null)
            {
                throw new BLException(Strings.JobHeadIsNotAvailable, "LaborDtl");
            }



            timProject = this.FindFirstProject4(Session.CompanyID, timJobHead.ProjectID);
            if (!string.IsNullOrEmpty(ipTimeTypCd))
            {
                if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling))
                {
                    throw new BLException(Strings.TimeTypeCodeCannotBeEnteredIfProjectBillingIsNot, "LaborDtl", "TimeTypCd");
                }
                if (String.IsNullOrEmpty(ipRoleCd))
                {
                    throw new BLException(Strings.ARoleCodeMustBeEnteredBeforeTheTimeTypeCode, "LaborDtl", "RoleCd");
                }
                if (timProject == null)
                {
                    throw new BLException(Strings.InvalidProjectTimeTypeCodeCannotBeEntered, "LaborDtl", "ProjectID");
                }
                LibProjectCommon.getWBSPhaseMethods(timJobHead.ProjectID, timJobHead.PhaseID, out vInvMethod, out vRevMethod);
                if (vInvMethod.Compare("TM") != 0)
                {
                    throw new BLException(Strings.InvalidProjectOrWBSPhaseInvoiMethodTimeTypeCode, "LaborDtl", "TimeTypCd");
                }
            }
            if (timProject != null && vInvMethod.Compare("TM") == 0)
            {

                if (timProject.PBPrjRtSrc.Compare("HIER") == 0 || timProject.PBPrjRtSrc.Compare("PROJ") == 0)
                {


                    if ((this.ExistsPBRoleRt(Session.CompanyID, timJobHead.ProjectID, ipRoleCd, ipTimeTypCd)))
                    {
                        gotRate = true;
                    }
                }
                if ((timProject.PBPrjRtSrc.Compare("HIER") == 0 && !gotRate) || timProject.PBPrjRtSrc.Compare("EMPL") == 0)
                {


                    if ((this.ExistsEmpRoleRt(Session.CompanyID, ipEmployeeNum, ipRoleCd, ipTimeTypCd)))
                    {
                        gotRate = true;
                    }
                }
                if ((timProject.PBPrjRtSrc.Compare("HIER") == 0 && !gotRate) || timProject.PBPrjRtSrc.Compare("ROLE") == 0)
                {


                    if ((this.ExistsPrjRoleRt(Session.CompanyID, ipRoleCd, ipTimeTypCd)))
                    {
                        gotRate = true;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ipTimeTypCd))
                        {
                            throw new BLException(Strings.TimeTypeCodeCannotBeBlank, "LaborDtl", "TimeTypCd");
                        }
                    }
                }
                if (!gotRate)
                {
                    if (timProject.PBPrjRtSrc.Compare("HIER") == 0)
                    {
                        rateErrMsg = Strings.ThereIsNoChargeRateDefinedForTheSelecTimeTypePlease;
                    }
                    else
                    {
                        if (timProject.PBPrjRtSrc.Compare("PROJ") == 0)
                        {
                            rateErrMsg = Strings.ThereIsNoChargeRateDefinedOnTheProjectForTheSelec;
                        }
                        else
                        {
                            if (timProject.PBPrjRtSrc.Compare("EMPL") == 0)
                            {
                                rateErrMsg = Strings.ThereIsNoChargeRateDefinedOnTheEmploForTheSelec;
                            }
                            else
                            {
                                rateErrMsg = Strings.ThereIsNoChargeRateDefinedOnTheRoleCodeForTheSelec;
                            }
                        }
                    }
                }
            }
            return rateErrMsg;
        }

        /// <summary>
        /// This method validates the ResourceGroup code
        /// </summary>        
        private void validateWcCode(string CompanyID, string JobNum, string wcCode, string TableName, Guid TableSysRowID)
        {
            string tmpStr = string.Empty;
            string v_ValidPlants = string.Empty;
            if (String.IsNullOrEmpty(wcCode))
            {
                return;
            }
            /* SET THE LIST OF VALID RESOURCE PLANTS
             1. Current Plant (JobHead.Plant) is valid for all job types.
             A MAINTENANCE JOB ALSO ALLOWS RESOURCES IN;
             2. Equipment Plant
             3. Current Plants Maintenance Plant (Plant.MaintPlant) */
            v_ValidPlants = Session.PlantID;


            JobHead = this.FindFirstJobHead3(CompanyID, JobNum, "MNT");
            if (JobHead != null)
            {


                Equip = this.FindFirstEquip2(JobHead.Company, JobHead.EquipID);
                if (Equip != null && !Equip.Plant.KeyEquals(Session.PlantID))
                {
                    v_ValidPlants = v_ValidPlants + Ice.Constants.LIST_DELIM + Equip.Plant;
                }



                Plant = this.FindFirstPlant2(Session.CompanyID, Session.PlantID);
                if (!String.IsNullOrEmpty(Plant.MaintPlant) && !Plant.MaintPlant.KeyEquals(Session.PlantID))
                {
                    v_ValidPlants = v_ValidPlants + Ice.Constants.LIST_DELIM + Plant.MaintPlant;
                }
            }/* avail JobHead */



            ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, wcCode);
            if (ResourceGroup == null)
            {
                throw new BLException(Strings.ResourceGroupNotValid, TableName, "ResourceGrpID", TableSysRowID);
            }
            if (v_ValidPlants.Lookup(ResourceGroup.Plant, Ice.Constants.LIST_DELIM[0]) == -1)
            {
                throw new BLException(Strings.ResouGroupNotValidForThePlant, TableName, "ResourceGrpID", TableSysRowID);
            }
        }

        /// <summary>
        /// This method validates the date field 
        /// </summary>
        private void validDate()
        {


            LaborHed = this.FindFirstLaborHed11(Session.CompanyID, ttLaborDtl.LaborHedSeq);
            if (LaborHed != null && !LaborHed.ActiveTrans)
            {
                if (LaborHed.ClockInTime > LaborHed.ClockOutTime)
                {
                    if ((ttLaborDtl.ClockInDate.Value.Date > LaborHed.ClockInDate.Value.AddDays(1).Date) ||
                     (ttLaborDtl.ClockInDate.Value.Date < LaborHed.ClockInDate.Value.Date))
                    {
                        throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.Date));
                    }
                }
                else
                {
                    if (ttLaborDtl.ClockInDate.Value.Date != LaborHed.ClockInDate.Value.Date)
                    {
                        throw new BLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.Date));
                    }
                }
            }
        }

        /// <summary>
        /// This method validates hour entry
        /// </summary>
        /// <param name="dspTime">This is the time being tested</param>
        private void validHrMin(decimal dspTime)
        {
            if (Math.Truncate(dspTime) > 24)
            {
                ExceptionManager.AddBLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.Hour));
            }
            else if (dspTime > 24)
            {
                ExceptionManager.AddBLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.Minute));
            }

            ExceptionManager.AssertNoBLExceptions();
        }

        /// <summary>
        /// This method validates that the lunch times are within the clock in/out times
        /// </summary>
        /// <param name="i_ClockInTime">Clock In time</param>
        /// <param name="i_ClockOutTime">Clock Out time</param>
        /// <param name="i_LunchOutTime">Lunch Out Time</param>
        /// <param name="i_LunchInTime">Lunch In time</param>
        private void validLunchTimes(decimal i_ClockInTime, decimal i_ClockOutTime, decimal i_LunchOutTime, decimal i_LunchInTime)
        {
            DateTime? v_ClockOutDate = null;
            DateTime? v_LunchStartDate = null;
            DateTime? v_LunchEndDate = null;
            int v_ClockInMin = 0;
            int v_ClockOutMin = 0;
            int v_LunchInMin = 0;
            int v_LunchOutMin = 0;

            if (i_ClockInTime > i_ClockOutTime)
            {
                v_ClockOutDate = (ttLaborHed.ClockInDate.Value.AddDays(1));
            }
            else
            {
                v_ClockOutDate = ttLaborHed.ClockInDate;
            }

            v_ClockInMin = (((TimeSpan)(ttLaborHed.ClockInDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(i_ClockInTime * 60);
            v_ClockOutMin = (((TimeSpan)(v_ClockOutDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(i_ClockOutTime * 60);
            /* LUNCH TIMES */
            v_LunchStartDate = ttLaborHed.ClockInDate; /* default clockin date */
            v_LunchEndDate = ttLaborHed.ClockInDate;
            if (i_ClockInTime > i_ClockOutTime)
            {
                if (((i_LunchOutTime < i_ClockInTime) && (i_LunchOutTime < 8)))
                {
                    v_LunchStartDate = (ttLaborHed.ClockInDate.Value.AddDays(1));
                }

                if (((i_LunchInTime < i_ClockInTime) && (i_LunchInTime < 8)))
                {
                    v_LunchEndDate = (ttLaborHed.ClockInDate.Value.AddDays(1));
                }
            }
            v_LunchOutMin = (((TimeSpan)(v_LunchStartDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(i_LunchOutTime * 60);
            v_LunchInMin = (((TimeSpan)(v_LunchEndDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(i_LunchInTime * 60);
        }

        /// <summary>
        /// This method validates time format
        /// </summary>
        /// <param name="dspTime">This is the time being tested</param>
        /// <param name="dspToken">This is the token(":",".") being tested</param>
        private void validTimeFormat(string dspTime, string dspToken)
        {
            if ((dspTime.IndexOf(dspToken, StringComparison.OrdinalIgnoreCase) + 1) == 0)
            {
                ExceptionManager.AddBLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.CurrTimeFormat));
            }

            ExceptionManager.AssertNoBLExceptions();
        }

        /// <summary>
        /// Verifies if the user should enter child serial numbers for the serial numbers
        /// being received depending on the setting of the Serial Number Matching before save.
        /// </summary>
        /// <param name="ds">Epicor.Mfg.BO.LaborDataSet</param>
        /// <param name="pcMsg">Set by the BL. It is used to send warning or error messages to the UI.</param> 
        /// <param name="piMsgType">The type of the message being returned. when 1 - it is a warning msg, when 2 - it is an error msg, when 0 - no msg should be displayed.</param>         
        public void VerifySerialMatch(ref LaborTableset ds, out string pcMsg, out int piMsgType)
        {
            pcMsg = string.Empty;
            piMsgType = 0;

            CurrentFullTableset = ds;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where !String.IsNullOrEmpty(ttLaborDtl_Row.RowMod)
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.TtLaborDtlRecordNotFound, "ttLaborDtl", "RowMod");
            }

            JobAsmbl jobAsmbl = FindFirstJobAsmbl6(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);

            /* ONLY verify Serial Matching for Auto Receive of Serialized Part. */
            if (jobAsmbl.AutoRecOpr == ttLaborDtl.OprSeq && ExistsJobOperSNRequiredOpr(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq))
            {
                JobHead = FindFirstJobHead17(Session.CompanyID, ttLaborDtl.JobNum);
                if (JobHead == null)
                {
                    throw new BLException(Strings.InvalidJobNumber, "JobHead", "JobNum");
                }

                PlantConfCtrl = FindFirstPlantConfCtrl4(Session.CompanyID, JobHead.Plant);
                if (PlantConfCtrl == null)
                {
                    throw new BLException(Strings.PlantConfiControlRecordNotFound, "PlantConfCtrl", "Plant");
                }

                /*Only if the plant of the Job is full serial tracking*/
                if (PlantConfCtrl.SerialTracking == SerialCommon.SerialTracking.Full && PlantConfCtrl.SerialMatchWarn > 1)
                {
                    bool logTrackRequired = false;
                    bool checkSerialNo = false;
                    SerialNo serialNo = null;
                    JobMtl jobMtl = null;
                    Part part = null;
                    LibSerialCommon.checkComponents(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, checkSerialNo, out logTrackRequired, ref jobMtl, ref part, ref serialNo);

                    /* Only if any part or subassembly is serial tracked*/
                    if (logTrackRequired)
                    {
                        foreach (var LbrScrapSerialNumbers_iterator in (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                                                        where ttLbrScrapSerialNumbers_Row.Company.KeyEquals(ttLaborDtl.Company) &&
                                                                              ttLbrScrapSerialNumbers_Row.JobNum.KeyEquals(ttLaborDtl.JobNum) &&
                                                                              ttLbrScrapSerialNumbers_Row.AssemblySeq == ttLaborDtl.AssemblySeq &&
                                                                              ttLbrScrapSerialNumbers_Row.PartNum.KeyEquals(jobAsmbl.PartNum) &&
                                                                              ttLbrScrapSerialNumbers_Row.SNStatus.KeyEquals("COMPLETE")
                                                                        select ttLbrScrapSerialNumbers_Row))
                        {
                            ttLbrScrapSerialNumbers = LbrScrapSerialNumbers_iterator;

                            serialNo = FindFirstSerialNo(ttLbrScrapSerialNumbers.Company, ttLbrScrapSerialNumbers.PartNum, ttLbrScrapSerialNumbers.SerialNumber);
                            if (serialNo == null)
                            {
                                throw new BLException(Strings.InvalidSerialNumberNumber, "SerialNo", "SerialNumber");
                            }

                            /*If the related assembly/material is set to reassign sn to assembly then it's okay if not fully matched.*/
                            bool isReassignFrom = false;
                            if (serialNo.MtlSeq != 0)
                            {
                                isReassignFrom = ExistsJobMtlReassignSNAsm(Session.CompanyID, serialNo.JobNum, serialNo.AssemblySeq, serialNo.MtlSeq);
                            }
                            else
                            {
                                isReassignFrom = jobAsmbl.ReassignSNAsm;
                            }

                            /*Process only those Serial Numbers that are not fully matched*/
                            if (!serialNo.FullyMatched && !isReassignFrom)
                            {
                                if (PlantConfCtrl.SerialMatchWarn == 2)
                                {
                                    pcMsg = Strings.ThereIsAtLeastOneSerialNumberWhichHasNotBeenMatched;
                                    piMsgType = 1;
                                }
                                else if (PlantConfCtrl.SerialMatchWarn == 3)
                                {
                                    pcMsg = Strings.LowerLevelSerialMatchIsRequiForTheReceiptSerial;
                                    piMsgType = 2;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method checks for and generates labor warnings - 7486 
        /// </summary>
        /// <param name="vMessage">Warnings to return for user review</param>
        private void warnHrs(out string vMessage)
        {
            vMessage = string.Empty;
            string vTxt = string.Empty;
            string dspWrnStr = string.Empty;
            if (ttJCSyst.GenLaborWarnMsg && ttLaborDtl.LaborType.Compare("I") != 0)
            {
                VJobNum = ttLaborDtl.JobNum;
                VAssemblySeq = ttLaborDtl.AssemblySeq;
                VOprSeq = ttLaborDtl.OprSeq;
                VWCCode = ttLaborDtl.ResourceGrpID;
                VEmployeeNum = ttLaborDtl.EmployeeNum;
                DispWarn = true;
                FromProg = ((ttLaborDtl.ActiveTrans) ? "DataColl" : "LabEnt");
                dspWrnStr = ((DispWarn) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));
                /* OPERATION COMPLETED UNDER ESTIMATE */
                LibWARNDEF.RunDEWRN080(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.Complete, ttLaborDtl.LaborHrs, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    vTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + dspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(80, vTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        vMessage = vMessage + ((String.IsNullOrEmpty(vMessage)) ? "" : " ") + VContinue;
                    }
                }
                /* OPERATION OVER ESTIMATE */
                LibWARNDEF.RunDEWRN090(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.LaborHrs, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    vTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + dspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(90, vTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        vMessage = vMessage + ((String.IsNullOrEmpty(vMessage)) ? "" : " ") + VContinue;
                    }
                }
                if (ttLaborDtl.LaborQty > 0)
                {
                    LibWARNDEF.RunDEWRN140(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.Complete, ttLaborDtl.LaborQty, ttLaborDtl.ScrapQty, ttLaborDtl.DiscrepQty, ttLaborDtl.LaborHrs, out VlabWarnNum, out VVariancePct, out VEstQtyPerHr, out VEstProdHrs, out VEarnedHrs);
                    if (VlabWarnNum > 0)
                    {
                        vTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + dspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                        LibXAP05._XAP05(140, vTxt, out VContinue);
                        if (!String.IsNullOrEmpty(VContinue))
                        {
                            vMessage = vMessage + ((String.IsNullOrEmpty(vMessage)) ? "" : " ") + VContinue;
                        }
                    }

                    /* TRANSACTION EFF OVER % LIMIT */
                    LibWARNDEF.RunDEWRN150(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.Complete, ttLaborDtl.LaborQty, ttLaborDtl.ScrapQty, ttLaborDtl.DiscrepQty, ttLaborDtl.LaborHrs, out VlabWarnNum, out VVariancePct, out VEstQtyPerHr, out VEstProdHrs, out VEarnedHrs);
                    if (VlabWarnNum > 0)
                    {
                        vTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + dspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                        LibXAP05._XAP05(150, vTxt, out VContinue);
                        if (!String.IsNullOrEmpty(VContinue))
                        {
                            vMessage = vMessage + ((String.IsNullOrEmpty(vMessage)) ? "" : " ") + VContinue;
                        }
                    }
                    /* OPERATION EFF UNDER % LIMIT */
                    LibWARNDEF.RunDEWRN160(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.Complete, ttLaborDtl.ReWork, ttLaborDtl.LaborQty, ttLaborDtl.LaborHrs, out VlabWarnNum, out VVariancePct, out VEstQtyPerHr, out VEstProdHrs, out VEarnedHrs, out VActualHrs);
                    if (VlabWarnNum > 0)
                    {
                        vTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + dspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                        LibXAP05._XAP05(160, vTxt, out VContinue);
                        if (!String.IsNullOrEmpty(VContinue))
                        {
                            vMessage = vMessage + ((String.IsNullOrEmpty(vMessage)) ? "" : " ") + VContinue;
                        }
                    }
                    /* OPERATION EFF OVER % LIMIT */
                    LibWARNDEF.RunDEWRN170(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.Complete, ttLaborDtl.ReWork, ttLaborDtl.LaborQty, ttLaborDtl.LaborHrs, out VlabWarnNum, out VVariancePct, out VEstQtyPerHr, out VEstProdHrs, out VEarnedHrs, out VActualHrs);
                    if (VlabWarnNum > 0)
                    {
                        vTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + dspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                        LibXAP05._XAP05(170, vTxt, out VContinue);
                        if (!String.IsNullOrEmpty(VContinue))
                        {
                            vMessage = vMessage + ((String.IsNullOrEmpty(vMessage)) ? "" : " ") + VContinue;
                        }
                    }
                }
            }
        }

        private void warnLabor(ref string vMessage)
        {
            string vTxt = string.Empty;
            string DspWrnStr = string.Empty;
            string Do6070AlertStr = string.Empty;
            string Do6070WarnStr = string.Empty;

            if (ttJCSyst.GenLaborWarnMsg &&
                ttLaborDtl.LaborType.Compare("I") != 0)
            {
                VJobNum = ttLaborDtl.JobNum;
                VAssemblySeq = ttLaborDtl.AssemblySeq;
                VOprSeq = ttLaborDtl.OprSeq;
                VWCCode = ttLaborDtl.ResourceGrpID;
                VEmployeeNum = ttLaborDtl.EmployeeNum;
                DispWarn = true;
                FromProg = ((ttLaborDtl.ActiveTrans) ? "DataColl" : "LabEnt");
                DspWrnStr = ((DispWarn) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));

                /* OPERATION COMPLETED UNDER QUANTITY */
                LibWARNDEF.RunDEWRN060(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.Complete, ttLaborDtl.ReWork, ttLaborDtl.LaborQty, out VlabWarnNum, out VVariancePct, out VDo6070Alert, out VDo6070Warn);
                Do6070AlertStr = ((VDo6070Alert) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));
                Do6070WarnStr = ((VDo6070Warn) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));      /*     VLabWarnNum = 60. */

                if (VlabWarnNum > 0)
                {
                    vTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + DspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq) + "|" + Do6070AlertStr + "|" + Do6070WarnStr;
                    LibXAP05._XAP05(60, vTxt, out VContinue);
                    if (!String.IsNullOrEmpty(VContinue))
                    {
                        vMessage = vMessage + ((String.IsNullOrEmpty(vMessage)) ? "" : " ") + VContinue;
                    }
                }
            }
        }

        /// <summary>
        /// This method generates labor warning on the opCode field
        /// </summary>
        /// <param name="opCode">Operation Code</param>
        /// <param name="vMessage">Returns message user needs to review</param>
        private void warnOpCode(string opCode, out string vMessage)
        {
            vMessage = string.Empty;
            string vTxt = string.Empty;
            string dspWrnStr = string.Empty;
            string origOpcode = string.Empty;
            if (ttJCSyst != null && ttJCSyst.GenLaborWarnMsg && ttLaborDtl.LaborType.Compare("I") != 0)
            {
                VJobNum = ttLaborDtl.JobNum;
                VAssemblySeq = ttLaborDtl.AssemblySeq;
                VOprSeq = ttLaborDtl.OprSeq;
                VWCCode = ttLaborDtl.ResourceGrpID;
                VEmployeeNum = ttLaborDtl.EmployeeNum;
                DispWarn = true;
                FromProg = "LabEnt";
                dspWrnStr = ((DispWarn) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));        /* include file looks at temp-table so the OpCode must temporarily be assigned to the temp-table */
                origOpcode = ttLaborDtl.OpCode;
                ttLaborDtl.OpCode = opCode;        /* operation code changed */
                LibWARNDEF.RunDEWRN110(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.OpCode, out VlabWarnNum, out VVariancePct);
                ttLaborDtl.OpCode = origOpcode;
                if (VlabWarnNum > 0)
                {
                    vTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + dspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(110, vTxt, out VContinue);
                    vMessage = VContinue;
                }
            }
        }

        /// <summary>
        /// This method created warnings based on qty
        /// </summary>
        /// <param name="laborQty">New LaborQty value</param>
        /// <param name="vMessage">Returns warnings for user review</param>
        private void warnQty(decimal laborQty, out string vMessage)
        {
            vMessage = string.Empty;
            string dspWrnStr = string.Empty;
            string vTxt = string.Empty;
            decimal tmpQty = decimal.Zero;
            string Do6070AlertStr = string.Empty;
            string Do6070WarnStr = string.Empty;
            decimal oldLaborQty = decimal.Zero;
            decimal newLaborQty = decimal.Zero;

            if (ttJCSyst.GenLaborWarnMsg && ttLaborDtl.LaborType.Compare("I") != 0)
            {
                /* ERPS-94466 - When checking for over quantity warning, we need to pass the difference between the  *
                 * new LaborQty and the old value. The old value is already included in JobOper.QtyCompleted. But we *
                 * need to make sure that the old value is for the same job operation.                               */
                oldLaborQty = GetOldLaborDtlQty(Session.CompanyID, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);
                newLaborQty = laborQty - oldLaborQty;

                VJobNum = ttLaborDtl.JobNum;
                VAssemblySeq = ttLaborDtl.AssemblySeq;
                VOprSeq = ttLaborDtl.OprSeq;
                VWCCode = ttLaborDtl.ResourceGrpID;
                VEmployeeNum = ttLaborDtl.EmployeeNum;
                DispWarn = true;
                FromProg = ((ttLaborDtl.ActiveTrans) ? "DataColl" : "LabEnt");
                dspWrnStr = ((DispWarn) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));
                /* warning uses tt values so new value must temporarily be assigned to the tt */
                tmpQty = ttLaborDtl.LaborQty;
                ttLaborDtl.LaborQty = newLaborQty;
                /* OPERATION OVER QUANTITY */
                LibWARNDEF.RunDEWRN070(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.LaborType, ttLaborDtl.ReWork, ttLaborDtl.LaborQty, out VlabWarnNum, out VVariancePct, out VDo6070Alert, out VDo6070Warn);
                ttLaborDtl.LaborQty = tmpQty;
                Do6070AlertStr = ((VDo6070Alert) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));
                Do6070WarnStr = ((VDo6070Warn) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));
                if (VlabWarnNum > 0)
                {
                    vTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + dspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq) + "|" + Do6070AlertStr + "|" + Do6070WarnStr;
                    LibXAP05._XAP05(70, vTxt, out VContinue);
                    vMessage = VContinue;
                }
            }
        }

        private void warnScrapReasonCode(ref string vMessage)
        {
            string vTxt = string.Empty;
            string dspWrnStr = string.Empty;
            if (ttJCSyst.GenLaborWarnMsg && ttLaborDtl.LaborType.Compare("I") != 0)
            {
                VJobNum = ttLaborDtl.JobNum;
                VAssemblySeq = ttLaborDtl.AssemblySeq;
                VOprSeq = ttLaborDtl.OprSeq;
                VWCCode = ttLaborDtl.ResourceGrpID;
                VEmployeeNum = ttLaborDtl.EmployeeNum;
                DispWarn = true;
                FromProg = "DataColl";
                dspWrnStr = ((DispWarn) ? Compatibility.Convert.ToString(1) : Compatibility.Convert.ToString(0));
                /* EXCESS SCRAP REPORTED */
                LibWARNDEF.RunDEWRN130(VJobNum, VAssemblySeq, VOprSeq, DispWarn, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.ScrapQty, out VlabWarnNum, out VVariancePct);
                if (VlabWarnNum > 0)
                {
                    vTxt = Compatibility.Convert.ToString(VlabWarnNum) + "|" + VJobNum + "|" + Compatibility.Convert.ToString(VAssemblySeq) + "|" + Compatibility.Convert.ToString(VOprSeq) + "|" + VWCCode + "|" + VEmployeeNum + "|" + dspWrnStr + "|" + FromProg + "|" + Compatibility.Convert.ToString(VVariancePct) + "|" + ttLaborDtl.SysRowID + "|" + ttLaborDtl.ReworkReasonCode + "|" + ttLaborDtl.ScrapReasonCode + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborHedSeq) + "|" + Compatibility.Convert.ToString(ttLaborDtl.LaborDtlSeq);
                    LibXAP05._XAP05(130, vTxt, out VContinue);
                    if (vMessage.Length == 0)
                    {
                        vMessage = VContinue;
                    }
                    else
                    {
                        vMessage = vMessage + " " + VContinue;
                    }
                }
            }
        }
        private void _history01()
        {
            //01/27/04 - jaj   assigned the proposed value to the dataset in the validation methods    
            //04/21/04 - Pradeep scr13149 - Change all BL code that references Wrkcenter to ResourceGroup. 
            //04/30/04 - Terry   scr13149 - Change all BL code that references Wrkcenter to ResourceGroup. 
            //06/03/04 - Bar     SCR13811 - default JobOpdtl records were not being found. ResourceGroupId was missing.
            //06/04/04 - jaj     scr13818 - fixed Job validation for Indirect Type
            //                   scr13774 - don't validate shift in LaborHedAFterGetNew1.
            //                   scr13813 - added ChangeLaborType to clear fields when type changes
            //06/24/04 - jaj              - fixed offsettoday for labordtl from shopfloor.
            //07/06/04 - sev     scr14045 - added public methods SelectForWork, ChangeResourceId.
            //07/12/04 - sev     scr14045 - add duplicate LaborDtl validation in SelectForWork()
            //07/12/04 - jaj              - modified calls to xap05.p to work
            //07/13/04 - sev              - added method SelectForWorkCheckWarnings
            //07/28/04 - Pradeep          - Implemented ProDataSets.
            //07/28/04 - smr              - Added GetDetail method to return a LaborHed record and just one LaborDtl record.
            //07/26/04 - bpm              - If the JobOpDtl.ResourceGrpID is null then assign the LaborDtl.ResourceGrpID
            //                              using the Resource Group from the JobOpDtl ResourceID.
            //09/03/04 - sev     scr14876 - Add LaborCost and BurdenCost calculated fields
            //09/30/04   Bar     SCR17136 - The valid date procedure should ignore active transactions (laborhed.activetrans)
            //09/30/04   Bar     SCR17136 - The valid date procedure should ignore active transactions (laborhed.activetrans)
            //09/30/04   jaj     scr17146 - The active transaction labor/burden hours were getting calculated incorrectly
            //10/06/04   jaj     scr17268 - Labor warnings were not showing up for MES menu
            //10/13/04 Pradeep   scr17239 - The Clock In and Clock Out times are not converted to adjusted time.
            //10/21/04 - Raj     scr17686 - Labor Hours inconsistent for indirect activity.
            //10/21/04 - ldb     scr17532 - Moved default of FeedPayroll to GetNew of LaborHed.
            //11/08/04 - jaj     scr18185 - Apply Grace not working for Clockin Times
            //11/15/04 - Todd    scr17990 - Labor Entry not working correctly with APS
            //12/07/04  Orv      18945 - Added "Do you want to continue?" to msg string generated by XAP05.p.
            //                           This string was removed from xap05 to eliminate duplication.
            //01/10/05  jaj      19628 - labordtl date validation was wrong for shifts crossing dates
            //01/12/05  jaj      19645 - Added public method for UI to call to calculate LaborDtl hours
            //          jaj      19703 - time object does not handle 24 as a valid time for midnight.
            //01/19/05  smr      19878 - Allow End Activity on a job that is not released.
            //                   19847 - Don't override labor and burden hours manually entered by the user.
            //01/20/05  smr      19654 - Added field in LaborDtl datatable to store info text when multiple employees are working on the same operation.
            //01/21/05 Pradeep   scr19852 - Error returned on leave of Labor Type.
            //02/11/05  JSP      SCR #20234 - Modified DefaultOprSeq to check for shop warnings if
            //                   the LaborDtl is created through Data Collection or from Shop Floor.
            //02/18/05  JSP      SCR #20558 - Modified DefaultOprSeq to throw exception if the
            //                   activity is a rework of a Quantity Only operation.
            //02/22/05  smr      scr20421 - First Article record was not being created when an operation requires it.
            //03/02/05  smr      scr20998 - Burden can only be reported for location resource groups/resources. 
            //03/09/05 Pradeep   scr21088 -  Labor Entry needs to default in the Primary Production Resource Group.
            //04/05/05 ATR       SCR #20424 - The Employee should not be abled to start activity for a job/asm/seq /resource id he/she 
            //                                is currently working on more than once. So I added the CheckEmployeeActivity procedure to validate it.
            //04/12/05 ATR       SCR #20166 - The Time stamp was being set when the StartActivity procedure was being called and that procedure was being called
            //                                by UI when the window Opened. So I needed to do a new public procedure to set the Time stamp when the user hit the OK button.
            //                                The new procedure is called SetClockInAndDisplayTimeMES.
            //04/25/05 jaj       SCR 22205 - Burden Rate should always default from JobOper.BurdenRate.  If the 
            //                                resource/resourcegrp changes then subtract the old resources rate out 
            //                                and add the new resource's burden rate back in (getLaborDtlBurdeRates)
            //05/11/05 smr       SCR 22550 - Don't recalculate the Payroll hours when saving a LaborHed record.  The user may have overridden the value.
            //05/13/05  JSP      SCR #20669 - (Vantage 6.1 SCR #8457) Modified the adjustForGracePeriod procedure to
            //                   adjust the BaseNewClockOutMin calculation to use 1440 (24:00 in minutes) instead of 
            //                   the ttLaborDtl.ClockOutTime if the clock out time (CurrTime) is exactly at midnight.
            //06/06/05  MFG      SCR #21891  Added the Overrides Procedure, to modify the ResourceGroupID and Opeation in a given LaborDtl row.              
            //06/06/05  MFG      SCR #21891  Modified the Overrides Procedure to avoid changing the values if the incoming parameters are blank.
            //06/28/05  DJP      SCR #22417 Added code to laborHedAfterGetNew1 and laborHedAfterGetRows to set EmpBasicShift and EmpBasicSupervisorID
            //07/04/05  laq      SCR #23389 Added public procedure (CheckFirstArticleWarnings) to validate First Article warnings.
            //07/12/05  DJP      SCR #22417 Added GetRowsWhoIsHere procedure for Shop Tracker
            //07/15/05  jaj      scr 22784  Translated the JobType and ProdDesc descriptions
            //07/22/05  jaj      scr 24043  BurdenRate was using the EstLaborRate from JobOper not the actual LaborRate
            //07/29/05  dmp      scr 24130  Added code chgWcCode to exclude resources that are flagged as inactive.
            //08/26/05  jaj      scr 24695  Fixed the "JobOper Not available" error in CheckFirstArticleWarnings
            //09/12/05  dcr      scr 24884 laborDtlBeforeUpdate and ValidateIndirect check if the expense code and direct code are empty, in this case it throws
            //                              an exception to warn the user that these fields are mandatory.
            //09/29/05  plp      scr 25117  Added test for Capability to Overrides procedure.                              
            //10/24/05  JSP      SCR 25736 - Modified the EndActivity method to set the LaborDtl.Complete flag.
            //01/16/06  acc      SCR 26699 - Added public procedure OverridesResource.
            //01/25/06  DVE   scr27413 - XML Comments in BO Public methods are cleanup.
            //02/28/06  grb   scr13671 - Removed (Subscribe "SkipBFReset") logic from the code and deleted procedure skipBFReset.
            //                           It was causing Progress "Cycle" error 2868 in BFLaborD.p.
            //03/02/06  IGP      SCR 27701 - Added code to laborDtlBeforeUpdate to check inactive Resourse Group
            //03/03/06  amv      SCR 19696 - Added chkPartQty () procedure . Changed beforeUpdate() procedure.
            //                               Compare total Job Parts Quantity with Labor Quantity.
            //03/09/06  DJY      SCR 28516 - Burden Rates assign modifications considering JobOpDtl.OverrideRates and LaborDtl.ResourceID
            //03/22/06  acc      SCR 26699 - Modified logic in Overrides and OverridesResource.
            //03/23/06  Orv      SCR 28069 - Allow maintenance of LaborDtl which employee has clocked out of regardless of the employee being clocked out.
            //                               Added validation to reject laborhed updates/delete if LaborHed.ActiveTrans = Yes
            //                               Added validation to reject labordtl updates/delete if LaborDtl.ActiveTrans = yes
            //04/18/06 - DJP     Job 3028 - warnLabor, warnQty new parameters required for warnings 60 and 70                                
            //04/18/06 AlbertoC SCR 26699 - Added some locig in Overrides, OverridesResource.
            //                                Modified parametes to call calcOpDtlBurdenRate in getLaborDtlBurdenRates
            //                                Modified function IsLocation. If cKeyValue empty then IsLocation = false.
            //04/24/06 tatyana.v SCR 19692 - Modified afterUpdate. It allow to update LaborDtl table by LaborPart data.
            //05/04/06 scottr    scr 29340 - Get JCDept from the capability if the resource/resource group is blank.
            //05/22/06 LorenS    SCR 27733 - Clock In/Out times should NOT change when changing Actual Clock In/Out times.
            //05/24/06 Pradeep   scr29691  - 29691  Labor Entry - Setting Complete flag if completed quantity is changed, even if it is not for the total qty.
            //06/07/06 debbieP SCR-29730 - add new EndActivityComplete procedure to Labor.p, and other changes so shop warning 60 will display warning when Complete is checked.
            //06/12/06 Pradeep   scr30244  - burden pulling from resource group not the resource.
            //06/23/06 JuliaK    scr30159  - warnLabor should call in CheckWarning from MES!
            //07/19/06 scottr    scr29834  - Replace PatchFld location with Resource.Location and ResourceGroup.Location.
            //07/20/06 AlbertoC  SCR31094 - Modified procedure setComplete in order to set correctly ttLaborDtl.Complete flag (based on 6.1 code).
            //07/28/06 debbieP   SCR30978 - setComplete - correct setting of complete flags.
            //08/04/06 EduardoR  SCR31429 - A piece of logic was missing from 6.1 to 8.0 on calcLaborHours.
            //08/15/06 Diogenes  scr28980 - Added explicit lock to For Each Statements missing no-lock/exclusive-lock parameter
            //08/16/06 GlennB    scr28980 - "No-Lock" was in the wrong place in a couple of "for each" statements.  Code wouldn't compile.
            //08/17/06 GlennB    scr29949 - Indirect labor expense code was not defaulting in on the labor entry screen.
            //                              Added a method to check if code record exists in the labor expense code table.
            //                              If so, updated labor detail record expense code with indirect code only if expense code is blank. 
            //09/06/06 GlennB    scr31968 - Removed validation in laborHedBeforeUpdate.  Now have the ability to start activity from a Work Queue during labor activity on a job.                             
            //09/06/06 GlennB    scr31771 - Removed code that validates lunch times that fall withing the start/stop times.  
            //                              Now have ability to start activity from a Work Queue during lunch break.
            //09/14/06 lgarduno SCR 32281 - Modified the validation message shown in validateJCDept procedure.
            //09/18/06 GlennB   SCR29949  - Changed logic. Was trying to capture expense code off of table LabExpCd instead of Indirect.
            //09/21/06 AlbertoC SCR32373 - Fixed a not found buffer issue in laborDtlBeforeUpdate and CheckWarnings.
            //10/12/06 debbieP   SCR30978 - setOprComplete - correct setting of OprComplete flag if LaborDtl.Complete = false.
            //10/13/06 JuliaK    SCR32522 - the resource group should be required for Indirect Activity.
            //11/06/06 scottr    scr40509 - Corrected issue with zero labor hours when Resource Group is marked to use estimates.
            //11/28/06 SergeyM   scr27688 - laborDtlBeforeUpdate: A warning is shown, preventing the user to start the same activity more than once in MES.
            //12/04/06 DJY      SCR41156 - create of LaborDtl must happen in laborDtlSetDefaults rather than laborDtlAfterGetNew so the field security framework
            //                             can handle security on the LaborDtl.LaborRate field.  Framework also changed (core/ServerManagerBase.i} to support this.
            //02/19/07 AlbertoC scr42171 - Calculate BurdenCost even if BurdenRate has security for the current user.
            //02/21/07 AlbertoC scr42247 - Look for all burden rates related to the operation when calculation Burden Cost.
            //02/28/07 AlbertoC scr42491 - JobOper.ActLabCost and JobOper.ActBurCost were not calculated correctly when BurdenRate has security for the current user.
            //03/06/07 scottr scr42529 - Save LaborDtl.BurdenHrs and LaborDtl.LaborHrs with 2 decimal precision.
            //03/13/07 ArmantdoT SCR #42853 - The fix done under PVCS SCR #27688 on laborDtlBeforeUpdate missed the Employee in the validation.
            //05/14/07 IrinaY scr 42182 - modified getEnteredOprSeq() and EndActivity() - default value whse/bin for Job operation should be 
            //                            from Job operation's Resource, if it is set and it is Location. 
            //05/24/07 IrinaY scr 42182 - rework. modified EndActivity() - Resource in EndActivity shoud be like it was in StartActivity.
            //06/09/07 AlbertoC SCR45012- Modified procedure chkPartQty, call it in laborDtlBeforeUpdate.
            //07/05/07 LudmilaS SCR 45533 - modified SelectForWork procedure: assigned of LaborEntryMethod was added.
            //07/10/07 scottr scr45629 - Burden rates should always come from the resource/resource group, not the JobOper.
            //07/12/07 scottr scr45635 - genLaborPart was generating an appserver error when updating ttLaborPart.
            //07/12/07 scottr scr45678 - When summing burden rates, and the resource uses group values, the burden type should come from the group.
            //07/16/07 scottr scr45635 - Correct issues with batch jobs.
            //07/24/07 PPhillips SCR 45806 - Enable editing of records for jobs that are not released.
            //07/27/07 EduardoR  SCR 45846 - Modified getLaborDtlBurdenRates to get correct BurdenRates.
            //                               For qty only it should come from the job and for time & qty entry it should not come from the job.
            //07/30/07 EduardoR  SCR 45846 - Modified getLaborDtlBurdenRates to determine whether the burden rate is a flat rate or a percentage of the labor rate.
            //04/09/07 JoseRA    SCR 40529 - Added call to AfterGetRow and Update Foreign Links for LaborHed table in updateTotHours procedure in order to refresh UI data.
            //09/07/07 IrinaY SCR 46410 - modified DefaultOprSeq, ChangeResourceID and CheckResourceGroup not to allow selecting Inactive Resource.
            //09/25/07 gjh SCR#46648 - Added excess scrap check to CheckWarnings,
            //10/19/07 JoseRA - SCR 31257 - Added code to laborPartBeforeUpdate procedure to validate LaborPart vs JobPart Quantities.
            //12/06/07 ArmandoT - SCR 47774 - Added the createNonConfMtlQ procedure to create material queue records for Nonconformances 
            //                                only when the LaborDtl.RequestMove (MES>End Activity>Request Move box) box is checked. 
            //12/11/07 JoseRA - SCR 31257 - Rework
            //12/19/07 AlexanderP SCR 40458 (opr.364) - Replace hard-coded rounding with rounding according to the currency setup and properties of the Amount. 
        }

        /// <summary>
        /// Builds a delimited list of role codes valid for the laborDtl
        /// </summary>
        private string buildValidRoleCodeList(string iEmpID, string iProjID, string iPhaseID, string iJobNum, int iAssSeq, int iOprSeq)
        {
            string validRoleList = string.Empty;
            string oprRoleList = string.Empty;
            bool empHasRoles = false;
            bool chkEmpPrjRoleAUX = false;
            string roleCodeAUX = string.Empty;
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling))
            {
                return string.Empty;
            }

            /* Know if the Employee has Roles assigned */



            empHasRoles = (this.ExistsEmpRole(Session.CompanyID, iEmpID));
            if (!empHasRoles)
            {
                return string.Empty;
            }
            /* Set the chkEmpPrjRoleAUX value */
            if (!string.IsNullOrEmpty(iProjID))
                chkEmpPrjRoleAUX = this.getchkEmpPrjRole(iProjID, iPhaseID);
            /* Get the operation Role List */



            JobOper = this.FindFirstJobOperPrimary(Session.CompanyID, iJobNum, iAssSeq, iOprSeq);
            if (JobOper != null)
            {
                oprRoleList = JobOper.PrjRoleList;
            }
            /* Employee and Operation Role Codes */
            if (chkEmpPrjRoleAUX == false)
            {
                validRoleList = oprRoleList;



                foreach (var EmpRole_iterator in (this.SelectEmpRole(Session.CompanyID, iEmpID)))
                {
                    var JoinFieldsResult = EmpRole_iterator;
                    if (!String.IsNullOrEmpty(validRoleList))
                    {
                        validRoleList = validRoleList + Ice.Constants.LIST_DELIM + JoinFieldsResult.RoleCd;
                    }
                    else
                    {
                        validRoleList = JoinFieldsResult.RoleCd;
                    }
                }
                return validRoleList;
            }
            /* Role Codes existing on both Employee and Operation */
            if (chkEmpPrjRoleAUX)
            {
                for (iCounter = 1; iCounter <= oprRoleList.NumEntries(Ice.Constants.LIST_DELIM[0]); iCounter++)
                {
                    roleCodeAUX = oprRoleList.Entry(iCounter - 1, Ice.Constants.LIST_DELIM);


                    if ((this.ExistsEmpRole(Session.CompanyID, iEmpID, roleCodeAUX)))
                    {
                        if (!String.IsNullOrEmpty(validRoleList))
                        {
                            validRoleList = validRoleList + Ice.Constants.LIST_DELIM + roleCodeAUX;
                        }
                        else
                        {
                            validRoleList = roleCodeAUX;
                        }
                    }
                }
                return validRoleList;
            }
            return string.Empty;
        }

        private bool canApprove(int ipLaborHedSeq, int ipLaborDtlSeq)
        {
            bool outCanApprove = false;



            //Task_LOOP:
            foreach (var Task in (this.SelectTask(Session.CompanyID, "LaborDtl", Compatibility.Convert.ToString(ipLaborHedSeq), Compatibility.Convert.ToString(ipLaborDtlSeq), false, "Aprv")))
            {
                if ((this.ExistsSaleAuth(Session.CompanyID, Task.SalesRepCode, Session.UserID)))
                {
                    outCanApprove = true;
                    break;
                }
            }
            return outCanApprove;  /* Function return value. */
        }

        private string convDec(string clckTimeC)
        {
            decimal clckTime = decimal.Zero;
            clckTime = Compatibility.Convert.ToDecimal(clckTimeC);
            return Compatibility.Convert.ToString(Math.Truncate(clckTime), "99") + Compatibility.Convert.ToString((clckTime - Math.Truncate(clckTime)), ".99");
        }

        private decimal convDec2(string clckTimeC)
        {
            decimal clckTime = decimal.Zero;
            clckTime = Compatibility.Convert.ToDecimal(clckTimeC);
            return Compatibility.Convert.ToDecimal(Compatibility.Convert.ToString(Math.Truncate(clckTime), "99") + Compatibility.Convert.ToString((clckTime - Math.Truncate(clckTime)), ".99"));
        }

        private string convMin(string clckTimeC)
        {
            decimal clckTime = decimal.Zero;
            clckTime = Compatibility.Convert.ToDecimal(clckTimeC);
            return Compatibility.Convert.ToString(Math.Truncate(clckTime), "99") + ":" + Compatibility.Convert.ToString(((clckTime - Math.Truncate(clckTime)) * 60), "99");
        }

        private decimal convMin2(string clckTimeC)
        {
            string clckTimeH = string.Empty;
            int clckTimeCM = 0;
            decimal clckTimeM = decimal.Zero;
            decimal clckTime = decimal.Zero;
            decimal clckTimeCH = decimal.Zero;

            clckTimeH = clckTimeC.Substring(0, clckTimeC.IndexOf(":"));
            clckTimeCH = Compatibility.Convert.ToDecimal(clckTimeH);
            clckTimeCM = Compatibility.Convert.ToInt32(clckTimeC.Substring(clckTimeC.IndexOf(":") + 1));
            clckTimeM = Math.Round(clckTimeCM / 60M, 2, MidpointRounding.AwayFromZero);

            clckTime = clckTimeCH + clckTimeM;

            if (clckTimeCH > 24)
            {
                ExceptionManager.AddBLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.Hour));
            }
            if (clckTimeCM > 59 || (clckTimeCH == 24 && clckTime > 24))
            {
                ExceptionManager.AddBLException(Erp.Services.Lib.Resources.GlobalStrings.MsgRequired(Strings.Minute));
            }

            return clckTime;
        }

        private bool enableSN(string jobNum, int assemblySeq, int oprSeq, string pcid, string partNum)
        {
            if (SerTraPlantType == -1)
            {
                SerTraPlantType = LibSerialCommon.isSerTraPlantType(Session.PlantID);
            }

            if (SerTraPlantType == SerialCommon.SerialTracking.None)
            {
                return false;
            }
            else if (SerTraPlantType == SerialCommon.SerialTracking.Full)
            {
                if (!string.IsNullOrEmpty(pcid) && !string.IsNullOrEmpty(partNum) && ExistsPartTrackSerialNum(Session.CompanyID, partNum, true))
                {
                    return true;
                }

                if (!string.IsNullOrEmpty(jobNum) && oprSeq > 0)
                {
                    return ExistsJobOperSNRequiredOpr(Session.CompanyID, jobNum, assemblySeq, oprSeq);
                }
            }
            else if (SerTraPlantType == SerialCommon.SerialTracking.Outbound)
            {
                if (!string.IsNullOrEmpty(pcid) && !string.IsNullOrEmpty(partNum) && ExistsPartTrackSerialNum(Session.CompanyID, partNum, true))
                {
                    return ExistsPkgControlHeaderOutboundContainer(Session.CompanyID, pcid, true)
                        || ExistsPkgControlStageHeaderOutboundContainer(Session.CompanyID, pcid, true);
                }
            }

            return false;
        }

        private bool genMtlQ()
        {
            JobOper bJobOper = FindFirstJobOper33(Session.CompanyID, ttLaborDtl.JobNum, ttLaborDtl.NextAssemblySeq, ttLaborDtl.NextOprSeq);
            if (bJobOper == null)
            {
                return true;
            }

            string cResourceGrpID = string.Empty;

            foreach (var JobOpDtl_iterator in SelectJobOpDtl4(bJobOper.Company, bJobOper.JobNum, bJobOper.AssemblySeq, bJobOper.OprSeq))
            {
                JobOpDtl = JobOpDtl_iterator;
                if (isLocation("ResourceGroup", JobOpDtl.ResourceGrpID) == true
                    || isLocation("Resource", JobOpDtl.ResourceID) == true)
                {
                    cResourceGrpID = JobOpDtl.ResourceGrpID;
                    break;
                }
            }

            ResourceGroup bInResourceGroup = FindFirstResourceGroup(Session.CompanyID, Session.PlantID, cResourceGrpID);
            if (bInResourceGroup == null)
            {
                return true;
            }

            /**** Find workcenter for current operation for outgoing whse/bin ****/
            ResourceGroup bOutResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, ttLaborDtl.Company, ttLaborDtl.ResourceGrpID);
            if (bOutResourceGroup == null)
            {
                return true;
            }

            bool lMoveIt = !(bInResourceGroup.InputWhse.KeyEquals(bOutResourceGroup.OutputWhse) && bInResourceGroup.InputBinNum.KeyEquals(bOutResourceGroup.OutputBinNum));

            return lMoveIt;  /* Function return value. */
        }

        /*------------------------------------------------------------------------------
          Purpose:  Get the chkEmpPrjRole value from JCSyst or Project
        ------------------------------------------------------------------------------*/
        private bool getchkEmpPrjRole(string iProjID, string iPhaseID)
        {
            if (String.IsNullOrEmpty(iProjID))
            {
                return false;
            }

            LibProjectCommon.getWBSPhaseMethods(iProjID, iPhaseID, out vInvMethod, out vRevMethod);
            /* this flag only pertains to TM, CP, PP, FF projects (phases) */
            if ("TM,CP,PP,FF".Lookup(vInvMethod) == -1)
            {
                return false;
            }
            else
            {


                Project = this.FindFirstProject5(Session.CompanyID, iProjID);
                return Project != null && Project.ChkEmpPrjRole;
            }
        }

        /// <summary>
        /// This method is called to update the values of the Display columns
        /// DspClockInTime and DspClockOutTime
        /// </summary>
        /// <param name="dspClckTm">The clock time to be parsed as Decimal</param>
        /// <param name="clckTm">The clock time formatted sent as output for the user</param>
        public void GetClockTime(string dspClckTm, out decimal clckTm)
        {
            /* SCR 94819 format time for display */
            if (ttJCSyst.ClockFormat.Compare("M") == 0)    /* Hrs:Minutes */
            {
                this.validTimeFormat(dspClckTm, ":");
                clckTm = this.convMin2(dspClckTm);
            }
            else   /* Hrs.Hundredths */
            {
                this.validTimeFormat(dspClckTm, ".");
                clckTm = this.convDec2(dspClckTm);
                this.validHrMin(clckTm);
            }
            ExceptionManager.AssertNoBLExceptions();
        }

        /// <summary>
        /// </summary>
        public string GetCodeDescList(string tableName, string fieldName)
        {
            return LibGCDL.GetCodeDescList(Session.SystemCode, tableName, fieldName);
        }

        private bool getComplete(int callNum)
        {
            if (callNum > 0 && this.ExistsFSCallhd(Session.CompanyID, callNum))
            {
                FSCallhd = this.FindFirstFSCallhd2(Session.CompanyID, callNum);
                return (FSCallhd == null) ? false : FSCallhd.LaborComplete;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This method is called to update the values of the Display columns
        /// DspClockInTime and DspClockOutTime
        /// </summary>
        /// <param name="clckTm">The clock time to be formatted</param>
        /// <param name="dspClckTm">The clock time formatted sent as output for the user</param>
        public void GetDspClockTime(decimal clckTm, out string dspClckTm)
        {
            this.validHrMin(clckTm);

            if (ttJCSyst.ClockFormat.Compare("M") == 0)    /* Hrs:Minutes */
            {
                dspClckTm = this.convMin(Compatibility.Convert.ToString(clckTm));
            }
            else   /* Hrs.Hundredths */
            {
                dspClckTm = this.convDec(Compatibility.Convert.ToString(clckTm));
            }
            ExceptionManager.AssertNoBLExceptions();
        }

        private string getDisplayJob(string laborType, string indirectCode, string jobNum)
        {
            string dispJob = string.Empty;
            if (laborType.Compare("I") == 0)
            {
                dispJob = this.getIndirectDesc(indirectCode);
                return dispJob;
            }
            else
            {
                return jobNum;
            }
        }

        private string getExpenseDesc(string expenseCode)
        {
            LabExpCd = this.FindFirstLabExpCd2(Session.CompanyID, expenseCode);
            return (LabExpCd == null) ? "" : LabExpCd.Description;  /* Function return value. */
        }

        private string getImagePath(string photoFile)
        {
            return Compatibility.Convert.ToString("empphoto/" + Session.CompanyID.Trim() + "/" + photoFile + ".bmp");  /* Function return value. */
        }

        private string getIndirectDesc(string indirectCode)
        {
            Indirect = this.FindFirstIndirect5(Session.CompanyID, indirectCode);
            return (Indirect == null) ? "" : Indirect.Description;  /* Function return value. */
        }

        private string getJCDeptDesc(string _jcDept)
        {
            JCDept = this.FindFirstJCDept(Session.CompanyID, _jcDept);
            return (JCDept == null) ? "" : JCDept.Description;  /* Function return value. */
        }

        private string getJCDeptFromCapability(string ipCapabilityID)
        {
            string outJCDept = string.Empty;



            Capability = this.FindFirstCapability4(Session.CompanyID, ipCapabilityID);
            if (Capability != null)
            {


                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, Capability.PrimaryResourceGrpID);
                if (ResourceGroup != null)
                {
                    outJCDept = ResourceGroup.JCDept;
                }
            }
            return outJCDept;  /* Function return value. */
        }

        private string getJobType(string jobNum)
        {
            if (String.IsNullOrEmpty(jobNum)) return string.Empty;

            string jobType = this.FindFirstJobHeadJobType(Session.CompanyID, jobNum);
            if (String.IsNullOrEmpty(jobType)) return string.Empty;

            ttLaborDtl.JobTypeCode = jobType;
            switch (jobType.ToUpperInvariant())
            {
                case "MFG":
                    return Strings.Manufacture;
                case "SRV":
                    return Strings.Service;
                case "PRJ":
                    return Strings.Project;
                case "MNT":
                    return Strings.Maintenance;
                default:
                    return string.Empty;  /* Function return value. */
            }
        }

        /*------------------------------------------------------------------------------
          Purpose: Determines if the Capability PrimaryResourceGroup set as location.
            Notes: Use in addNew...OpDtl and ...OpDtlAfterUpdate. 
        ------------------------------------------------------------------------------*/
        private bool getLocationByCapability(string ipCapabilityID)
        {
            bool lLocation = false;
            if (!String.IsNullOrEmpty(ipCapabilityID))
            {


                Capability = this.FindFirstCapability5(Session.CompanyID, ipCapabilityID);
                if (Capability != null)
                {


                    ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, Capability.PrimaryResourceGrpID);
                    if (ResourceGroup != null)
                    {
                        lLocation = ResourceGroup.Location;
                    }
                }
            }
            return lLocation;  /* Function return value. */
        }

        private string getMachineDesc(string machineId)
        {
            Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, machineId);
            return (Resource == null) ? "" : Resource.Description;  /* Function return value. */
        }

        private string getOPDesc(string opCode)
        {
            OpMaster = this.FindFirstOpMaster6(Session.CompanyID, opCode);
            return (OpMaster == null) ? "" : OpMaster.OpDesc;  /* Function return value. */
        }

        private string getProdDesc(string laborType)
        {
            switch (laborType.ToUpperInvariant())
            {
                case "P":
                    return Strings.Production;
                case "S":
                    return Strings.Setup;
                default:
                    return Strings.Indirect;
            }
        }

        private string getReasonDesc(string reasonCode, string reasonType)
        {
            Reason = this.FindFirstReason3(Session.CompanyID, reasonCode, reasonType);
            return (Reason == null) ? "" : Reason.Description;  /* Function return value. */
        }

        /*------------------------------------------------------------------------------
          Purpose: return the default value for Role Code according to some rules.
        ------------------------------------------------------------------------------*/
        private string getRoleCodeDefault(string iEmpID, string iJobNum, int iAssSeq, int iOprSeq, string iProjectID, string iPhaseID)
        {
            bool empHasRoles = false;
            bool chkEmpPrjRoleAUX = false;
            string roleCodeAUX = string.Empty;
            string oprRoleList = string.Empty;
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling))
            {
                return "";
            }

            if (String.IsNullOrEmpty(iProjectID))
            {
                return "";
            }


            /* Know if the Employee has Roles assigned */



            empHasRoles = (this.ExistsEmpRole(Session.CompanyID, iEmpID));
            /* Set the chkEmpPrjRoleAUX value */
            chkEmpPrjRoleAUX = this.getchkEmpPrjRole(iProjectID, iPhaseID);
            /* Get the Project Role List */
            if (!String.IsNullOrEmpty(iJobNum) && iOprSeq != 0)
            {


                JobOper = this.FindFirstJobOperPrimary(Session.CompanyID, iJobNum, iAssSeq, iOprSeq);
                if (JobOper != null)
                {
                    oprRoleList = JobOper.PrjRoleList;
                }
            }
            if (ttJCSyst.DfltPrjRoleLoc.Compare("OPR") == 0)
            {
                if (chkEmpPrjRoleAUX == false)
                {
                    for (iCounter = 1; iCounter <= oprRoleList.NumEntries(Ice.Constants.LIST_DELIM[0]);)
                    {
                        roleCodeAUX = oprRoleList.Entry(iCounter - 1, Ice.Constants.LIST_DELIM);
                        break;
                    }
                    return roleCodeAUX;
                }
                else
                {
                    if (empHasRoles)
                    {
                        for (iCounter = 1; iCounter <= oprRoleList.NumEntries(Ice.Constants.LIST_DELIM[0]); iCounter++)
                        {
                            roleCodeAUX = oprRoleList.Entry(iCounter - 1, Ice.Constants.LIST_DELIM);


                            if ((this.ExistsEmpRole2(Session.CompanyID, iEmpID, roleCodeAUX)))
                            {
                                return roleCodeAUX;
                            }
                        }
                        return "";
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            else if (ttJCSyst.DfltPrjRoleLoc.Compare("EMP") == 0)
            {
                if (chkEmpPrjRoleAUX == false)
                {


                    EmpRole = this.FindFirstEmpRole2(Session.CompanyID, iEmpID, true);
                    if (EmpRole != null)
                    {
                        return EmpRole.RoleCd;
                    }
                    else
                    {
                        for (iCounter = 1; iCounter <= oprRoleList.NumEntries(Ice.Constants.LIST_DELIM[0]);)
                        {
                            roleCodeAUX = oprRoleList.Entry(iCounter - 1, Ice.Constants.LIST_DELIM);
                            break;
                        }
                        return roleCodeAUX;
                    }
                }
                else if (chkEmpPrjRoleAUX == true && empHasRoles)
                {


                    EmpRole = this.FindFirstEmpRole2(Session.CompanyID, iEmpID, true);
                    if (EmpRole != null && oprRoleList.Lookup(EmpRole.RoleCd, Ice.Constants.LIST_DELIM[0]) > -1)
                    {
                        return EmpRole.RoleCd;
                    }
                    else
                    {


                        foreach (var EmpRole_iterator in (this.SelectEmpRole(Session.CompanyID, iEmpID, false)))
                        {
                            var JoinFieldsResult = EmpRole_iterator;
                            if (oprRoleList.Lookup(JoinFieldsResult.RoleCd, Ice.Constants.LIST_DELIM[0]) > -1)
                            {
                                return JoinFieldsResult.RoleCd;
                            }
                        }
                    }
                    return "";
                }
                else
                {
                    return "";
                }
            }
            return string.Empty;
        }

        private bool getSupervisorRights(string inEmployeeNum)
        {
            bool outHasRights = false;
            bool isAuthorizedUser = false;
            Erp.Tables.EmpBasic bCurrentEmpBasic = null;
            Erp.Tables.EmpBasic bSuperEmpBasic = null;



            bCurrentEmpBasic = this.FindFirstEmpBasic25(Session.CompanyID, inEmployeeNum);

            if (bCurrentEmpBasic != null &&
            !String.IsNullOrEmpty(bCurrentEmpBasic.SupervisorID))
            {
                bSuperEmpBasic = this.FindFirstEmpBasic26(Session.CompanyID, bCurrentEmpBasic.SupervisorID);
            }
            if (bSuperEmpBasic != null)
            {
                isAuthorizedUser = this.ExistsSaleAuth(Session.CompanyID, bCurrentEmpBasic.SupervisorID, Session.UserID);

                if (isAuthorizedUser)
                {
                    isAuthorizedUser = this.IsactiveWorkForce(Session.CompanyID, bCurrentEmpBasic.SupervisorID);
                }
                if (PlantConfCtrl == null)
                {


                    PlantConfCtrl = this.FindFirstPlantConfCtrl(Session.CompanyID, Session.PlantID);
                }
                if ((bSuperEmpBasic.DcdUserID.KeyEquals(Session.UserID) || isAuthorizedUser) &&
                    (PlantConfCtrl != null &&
                     PlantConfCtrl.TimeSuperCanMaintain == true))
                {
                    outHasRights = true;
                }
            }
            return outHasRights;  /* Function return value. */
        }

        private bool getUnapprovedFirstArt()
        {
            bool lUnapprovedFirstArt = false;



            JobOper = this.FindFirstJobOper34(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq);



            if (JobOper.FAQty > 0 || (this.ExistsFirstArt(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.ResourceID)))
            {



                FirstArt = this.FindLastFirstArt2(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, ttLaborDtl.ResourceID);

                if (FirstArt == null ||
                    FirstArt.FAStatus.Compare("R") == 0)
                {
                    lUnapprovedFirstArt = true;
                }
                else if (FirstArt.FAStatus.Compare("W") == 0)
                {
                    if (FirstArt == null ||
                        FirstArt.FAStatus.Compare("P") != 0)
                    {
                        lUnapprovedFirstArt = true;
                    }
                }
            } /*if JobOper.FAQty > 0 OR CAN-FIND... */

            return lUnapprovedFirstArt;  /* Function return value. */
        }

        private string getWCDesc(string wcCode)
        {
            ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, wcCode);
            return (ResourceGroup == null) ? "" : ResourceGroup.Description;  /* Function return value. */
        }

        /// <summary>
        /// This function is used to determine whether to set the Serial SNSTatus to WIP or CONSUMED when the serial number is still on the Job(not in inspection, completed, etc).
        /// 
        ///- Top level assembly: 
        ///  always uses WIP until moved off the job for shipment, inventory, another job, etc
        ///- Components(MtlSeq > 0 or AssemblySeq > 0)): 
        ///  If there is no lower level tracking performed set components to CONSUMED.
        ///  If already matched to a parent serial number set to CONSUMED.
        ///  If the top assembly on the job is not serial tracked set to CONSUMED if this serial is being set to complete and it's the last operation for this assembly.
        ///  If none of the above set to WIP because serial matching will be required for this assembly
        /// </summary>
        private string getWipOrConsumed(string partNum, string serialNumber, string jobNum, int snAssemblySeq, int snMtlSeq, int lbrOprSeq, bool isComplete)
        {
            if (snAssemblySeq == 0 && snMtlSeq == 0)
            {
                return "WIP";
            }

            bool llTracked = LibSerialCommon.plantLowLvlSerTrkType(Session.PlantID, ref PlantConfCtrl) > 1;
            if (!llTracked)
            {
                return "CONSUMED";
            }

            if (ExistsSerialMatch(Session.CompanyID, partNum, serialNumber))
            {
                return "CONSUMED";
            }

            bool isFinalOp = IsFinalOp(jobNum, snAssemblySeq, lbrOprSeq);
            if (isComplete && isFinalOp)
            {
                JobHead jobHead = FindFirstJobHead19(Session.CompanyID, jobNum);
                if (jobHead != null)
                {
                    Part part = FindFirstPart(Session.CompanyID, jobHead.PartNum);
                    if (part == null || part.TrackSerialNum == false)
                    {
                        return "CONSUMED";
                    }
                }
            }
            return "WIP";
        }

        private bool getWipPosted(int laborHedSeq)
        {
            return (this.ExistsLaborDtl6(Session.CompanyID, laborHedSeq, true));
        }

        /// <summary>
        /// To determine if the given operation is the "final" operation of the final assembly 
        /// </summary>
        /// <param name="ip_JobNum">job number</param>
        /// <param name="ip_AssemblySeq">assembly seq number</param>
        /// <param name="ip_OprSeq">operation seq number</param>
        /// <returns>bool true if is final operation, false if not</returns>
        private bool IsFinalOp(string ip_JobNum, int ip_AssemblySeq, int ip_OprSeq)
        {
            if (ip_AssemblySeq != 0)
            {
                return false;
            }

            if (ExistsJobAsmbl(Session.CompanyID, ip_JobNum, ip_AssemblySeq, ip_OprSeq))
            {
                return true;
            }

            if (!IsThereAnyFurtherOperations(Session.CompanyID, ip_JobNum, ip_AssemblySeq, ip_OprSeq))
            {
                return true;
            }

            return false;
        }

        private bool isLocation(string cTableName, string cKeyValue)
        {
            bool lIsLocation = false;
            if (cTableName.Compare("Resource") == 0)
            {


                Resource = Resource.FindFirstByPrimaryKey(Db, Session.CompanyID, cKeyValue);
                if (Resource != null)
                {
                    lIsLocation = Resource.Location;
                }
            }
            else if (cTableName.Compare("ResourceGroup") == 0)
            {


                ResourceGroup = ResourceGroup.FindFirstByPrimaryKey(Db, Session.CompanyID, cKeyValue);
                if (ResourceGroup != null)
                {
                    lIsLocation = ResourceGroup.Location;
                }
            }
            return lIsLocation;  /* Function return value. */
        }

        /* -----------------------------------------------------------
          Purpose: Convert Time in Hours/Min to Hours/Hundreths.      
        -------------------------------------------------------------*/
        private decimal minToDec(string dspTime)
        {
            decimal hrsMin = decimal.Zero;
            dspTime = dspTime.SubString(0, 2) + "." + dspTime.SubString(3, 2);
            hrsMin = Compatibility.Convert.ToDecimal(dspTime);
            return Math.Truncate(hrsMin) + ((hrsMin - Math.Truncate(hrsMin)) * 100) / 60m;
        }

        /* NOTE: THIS IS A PUBLIC FUNCTION. HOWEVER, I DID NOT USE THE STANDARD TRY_PUBLIC/CATCH_PUBLIC.
        THIS IS BECAUSE I WANT IT TO BE  CALLABLE BY THE CLIENT AND THE SERVER. IF A PUBLIC CALLS A PUBLIC
        THE EXCEPTION MESSAGES WOULD GET CLEARED TOO EARLY. 
        */
        /// <summary>
        /// Returns TRUE if Part Quantity Reporting is allowed for a given operation.
        /// </summary>
        /// <param name="ip_JobNum">Job number</param>
        /// <param name="ip_AssemblySeq">Assembly Sequence number</param>
        /// <param name="ip_OprSeq">Operation Sequence number</param>
        /// <remarks>
        /// In order to be allowed the following conditions must be meet.
        ///  1. Must be final assembly (AssemblySeq = 0)
        ///  2. Must be the final operation.
        ///  3. Job must have more that one end part defined or has one or more Parts with PartPerOp > 1 
        /// </remarks>
        public bool ReportPartQtyAllowed(string ip_JobNum, int ip_AssemblySeq, int ip_OprSeq)
        {
            if (ip_AssemblySeq != 0)
            {
                return false;
            }

            if (String.IsNullOrEmpty(ip_JobNum))
            {
                return false;
            }

            JobAsmblRoot vJobCache = null;

            if (CallContext.Properties.ContainsKey("LaborAfterGetRows"))
            {
                vJobCache = setJobCache(ip_JobNum);
            }

            if ((vJobCache != null && !vJobCache.ExistsLaborEntityBF) ? false :
                 IsLaborEntryMethodTimeAndBackflush(Session.CompanyID, ip_JobNum, ip_AssemblySeq, ip_OprSeq))
            {
                return true;
            }

            if (vJobCache != null)
            {
                //Cache data hshJobAfterGetRows has already calculated values with the original logic (from [else]-block)   
                if (vJobCache.FinalOprSeq != ip_OprSeq) return false;
                return vJobCache.JobPartQtyAllowed;
            }
            else
            {
                if (this.IsFinalOp(ip_JobNum, ip_AssemblySeq, ip_OprSeq) == false)
                {
                    return false;
                }
                else
                {
                    /*If there are coparts, verify if not reporting duplicate final operations */
                    var vAsmFinalOprSeq = GetJobAsmblFinalOpr(Session.CompanyID, ip_JobNum, ip_AssemblySeq);
                    if ((vAsmFinalOprSeq > 0) && vAsmFinalOprSeq != ip_OprSeq)
                    {
                        return false;
                    }
                }

                if ((this.ExistsJobPart(Session.CompanyID, ip_JobNum, 1)))
                {
                    return true;
                }

                if ((this.ExistsUniqueJobPart(Session.CompanyID, ip_JobNum)))
                {
                    return false;
                }

                if (!(this.ExistsFirstJobPart(Session.CompanyID, ip_JobNum)))
                {
                    return false;
                }
            }
            return true;
        }

        // Set cache data from the root assembly of the job, the logic duplicates ReportPartQtyAllowed() - without cache
        private JobAsmblRoot setJobCache(string ipJobNum)
        {
            if (hshJobAfterGetRows == null) hshJobAfterGetRows = new Hashtable();
            if (hshJobAfterGetRows.ContainsKey(ipJobNum))
            {
                return (JobAsmblRoot)hshJobAfterGetRows[ipJobNum];
            }
            JobAsmblRoot vJobAsmRoot = new JobAsmblRoot();
            vJobAsmRoot.ExistsLaborEntityBF = ExistsLaborEntryMethodBF(Session.CompanyID, ipJobNum, 0);
            //Final Operation of the root assembly is got from JobAsmbl or if it's zero then the last operation in the sequence
            int vFinalOpr = GetJobAsmblFinalOpr(Session.CompanyID, ipJobNum, 0);
            if (vFinalOpr <= 0)
            {
                vFinalOpr = MaxFurtherOperation(Session.CompanyID, ipJobNum, 0);
            }
            vJobAsmRoot.FinalOprSeq = vFinalOpr;

            if (this.ExistsJobPart(Session.CompanyID, ipJobNum, 1))
            {
                vJobAsmRoot.JobPartQtyAllowed = true;
            }
            else if (this.ExistsUniqueJobPart(Session.CompanyID, ipJobNum))
            {
                vJobAsmRoot.JobPartQtyAllowed = false;
            }
            else if (!this.ExistsFirstJobPart(Session.CompanyID, ipJobNum))
            {
                vJobAsmRoot.JobPartQtyAllowed = false;
            }
            else
                vJobAsmRoot.JobPartQtyAllowed = true;

            hshJobAfterGetRows.Add(ipJobNum, vJobAsmRoot);
            return vJobAsmRoot;
        }


        private bool snTranExists(string partNum, string serialNumber, string tranType, int oprSeq)
        {
            SNTran AltSNTran = FindFirstSNTran(Session.CompanyID, partNum, serialNumber, tranType, oprSeq);
            if (AltSNTran != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string formatTime(decimal pdNewIdleHrs)
        {
            string vClockFormat = string.Empty;
            string cFormatTime = string.Empty;

            JCSyst = this.FindFirstJCSyst(Session.CompanyID);

            if (JCSyst == null)
            {
                vClockFormat = "M";
            }
            else
            {
                vClockFormat = JCSyst.ClockFormat;
            }


            if (vClockFormat.Compare("M") == 0)
            {
                cFormatTime = Compatibility.Convert.ToString(Math.Truncate((Double)pdNewIdleHrs), "99") + ":" + Compatibility.Convert.ToString(((pdNewIdleHrs - Convert.ToDecimal(Math.Truncate((Double)pdNewIdleHrs))) * 60), "99");
            }
            else
            {
                cFormatTime = Compatibility.Convert.ToString(Math.Truncate((Double)pdNewIdleHrs), "99") + Compatibility.Convert.ToString((pdNewIdleHrs - Convert.ToDecimal(Math.Truncate((Double)pdNewIdleHrs))), ".99");
            }

            if (cFormatTime == null)
            {
                cFormatTime = "";
            }

            return cFormatTime;
        }

        /// <summary>
        /// Methods updates Downtime codes 
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="indirectCode">IndirectCode </param>
        /// <param name="downtimeNote">Downtime Note </param>
        public void ExternalMESDowntime(LaborTableset ds, string indirectCode, string downtimeNote)
        {
            int V_HdrClockInMinute = 0;
            decimal HrsMin = decimal.Zero;
            decimal HrsDec = decimal.Zero;
            string HrMinTime = string.Empty;
            int CurrTime = 0;
            int CurrSec = 0;
            int CurrMinute = 0;
            int CurrHour = 0;

            if (indirectCode == "")
            {
                throw new BLException(Strings.ExternalMESIndirectCodeNotBlank);
            }

            foreach (var ttLaborDtl_iterator in (from ttLaborDtl_Row in ds.LaborDtl
                                                 where ttLaborDtl_Row.ActiveTrans == true &&
                                                       (ttLaborDtl_Row.LaborType.KeyEquals("P") ||
                                                       ttLaborDtl_Row.LaborType.KeyEquals("S")) &&
                                                       ttLaborDtl_Row.RowMod.Compare("U") == 0
                                                 select ttLaborDtl_Row).ToList())
            {
                ttLaborDtl = ttLaborDtl_iterator;

                using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                {
                    LaborHed = this.FindFirstLaborHed3(ttLaborDtl.Company, ttLaborDtl.LaborHedSeq);

                    LaborDtl ttIndirectLaborDtl = new LaborDtl();
                    Db.LaborDtl.Insert(ttIndirectLaborDtl);

                    ttIndirectLaborDtl.Company = ttLaborDtl.Company;
                    ttIndirectLaborDtl.EmployeeNum = ttLaborDtl.EmployeeNum;
                    ttIndirectLaborDtl.LaborHedSeq = ttLaborDtl.LaborHedSeq;
                    ttIndirectLaborDtl.LaborDtlSeq = LibNextValue.GetNextSequence("LaborDtlSeq");
                    ttIndirectLaborDtl.LaborType = "I";
                    ttIndirectLaborDtl.LaborTypePseudo = "I";
                    ttIndirectLaborDtl.JobNum = ttLaborDtl.JobNum;
                    ttIndirectLaborDtl.AssemblySeq = ttLaborDtl.AssemblySeq;
                    ttIndirectLaborDtl.OprSeq = ttLaborDtl.OprSeq;
                    ttIndirectLaborDtl.JCDept = ttLaborDtl.JCDept;
                    ttIndirectLaborDtl.ResourceGrpID = ttLaborDtl.ResourceGrpID;
                    ttIndirectLaborDtl.OpCode = ttLaborDtl.OpCode;
                    ttIndirectLaborDtl.LaborNote = downtimeNote;
                    ttIndirectLaborDtl.IndirectCode = indirectCode;

                    var expenseCode = FindFirstIndirect(ttLaborDtl.Company, indirectCode);
                    if (expenseCode != null)
                    {
                        ttIndirectLaborDtl.ExpenseCode = expenseCode.ExpenseCode;
                    }

                    ttIndirectLaborDtl.ClockInMInute = CompanyTime.Now().Minute;

                    ttIndirectLaborDtl.ClockinTime = LibOffset.OffSetTime();
                    CurrTime = LibOffset.OffSetTime();
                    /* break out seconds */
                    CurrSec = (CurrTime % 60);
                    CurrTime = (CurrTime - CurrSec) / 60;
                    /* break out minutes */
                    CurrMinute = (CurrTime % 60);
                    /* break out hour */
                    CurrHour = (CurrTime - CurrMinute) / 60;
                    /* concatenate the two values */
                    HrMinTime = Compatibility.Convert.ToString(CurrHour, "99") + Compatibility.Convert.ToString(CurrMinute, "99");
                    /* convert it to a decimal value */
                    HrsMin = Compatibility.Convert.ToDecimal(HrMinTime);
                    /* convert to 2 decimal places */
                    HrsMin = HrsMin / 100;
                    /* convert to hours/hundreths */
                    HrsDec = Math.Truncate(HrsMin) + ((HrsMin - Math.Truncate(HrsMin)) * 100) / 60m;
                    ttIndirectLaborDtl.ClockinTime = HrsDec;
                    /* convert input date and time to minutes since base date */
                    ttIndirectLaborDtl.ClockInMInute = (((TimeSpan)(ttLaborDtl.ClockInDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(HrsMin * 60);

                    if (LibOffset.OffsetToday() == null)
                    {
                        ttIndirectLaborDtl.ClockInDate = null;
                        ttIndirectLaborDtl.DspClockInTime = Compatibility.Convert.ToString(CurrHour, "99") + ":" + Compatibility.Convert.ToString(CurrMinute, "99");
                    }
                    else
                    {
                        ttIndirectLaborDtl.ClockInDate = LibOffset.OffsetToday().Value.Date;
                        ttIndirectLaborDtl.DspClockInTime = Compatibility.Convert.ToString(CurrHour, "99") + ":" + Compatibility.Convert.ToString(CurrMinute, "99");
                    }

                    var vHoursToDsply = formatTime(ttIndirectLaborDtl.ClockInMInute);
                    JCShiftResult JCShift = this.FindFirstJCShift2(Session.CompanyID, LaborHed.Shift);
                    if (JCShift != null)
                    {
                        V_HdrClockInMinute = (((TimeSpan)(LaborHed.ClockInDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(LaborHed.ClockInTime * 60);
                        if ((ttJCSyst.DetailGrace && ((LaborHed.ClockInTime == JCShift.StartTime)
                        && (ttLaborDtl.ClockInMInute - V_HdrClockInMinute) <= ttJCSyst.LateClockInAllowance)))
                        {
                            ttIndirectLaborDtl.ClockinTime = LaborHed.ClockInTime;
                            ttIndirectLaborDtl.DspClockInTime = LaborHed.DspClockInTime;
                            ttIndirectLaborDtl.ClockInDate = LaborHed.ClockInDate;
                            ttIndirectLaborDtl.ClockInMInute = (((TimeSpan)(ttLaborDtl.ClockInDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(HrsMin * 60);
                        }
                    }

                    ttIndirectLaborDtl.GLTrans = ttLaborDtl.GLTrans;
                    ttIndirectLaborDtl.LaborEntryMethod = "T";
                    ttIndirectLaborDtl.GLTrans = ttLaborDtl.GLTrans;
                    ttIndirectLaborDtl.PayrollDate = ttLaborDtl.PayrollDate;
                    ttIndirectLaborDtl.TimeStatus = ttLaborDtl.TimeStatus;
                    ttIndirectLaborDtl.CreatedBy = ttLaborDtl.CreatedBy;
                    ttIndirectLaborDtl.CreateDate = ttLaborDtl.CreateDate;
                    ttIndirectLaborDtl.CreateTime = ttLaborDtl.CreateTime;
                    ttIndirectLaborDtl.ChangedBy = ttLaborDtl.ChangedBy;
                    ttIndirectLaborDtl.ChangeDate = ttLaborDtl.ChangeDate;
                    ttIndirectLaborDtl.ChangeTime = ttLaborDtl.ChangeTime;
                    ttIndirectLaborDtl.Shift = ttLaborDtl.Shift;
                    ttIndirectLaborDtl.ApprovedDate = ttLaborDtl.ApprovedDate;
                    ttIndirectLaborDtl.AsOfDate = ttLaborDtl.AsOfDate;
                    ttIndirectLaborDtl.Downtime = ttLaborDtl.Downtime;
                    ttIndirectLaborDtl.ActiveTrans = false;
                    ttIndirectLaborDtl.RefAssemblySeq = ttLaborDtl.AssemblySeq;
                    ttIndirectLaborDtl.RefJobNum = ttLaborDtl.JobNum;
                    ttIndirectLaborDtl.RefOprSeq = ttLaborDtl.OprSeq;

                    Db.Validate(ttIndirectLaborDtl);
                    txScope.Complete();
                }

            }
        }

        /// <summary>
        /// Methods updates Downtime codes 
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        public void ExternalMESEndDowntime(ref LaborTableset ds)
        {
            decimal HrsMin = decimal.Zero;
            decimal HrsDec = decimal.Zero;
            string HrMinTime = string.Empty;
            int CurrTime = 0;
            int CurrSec = 0;
            int CurrMinute = 0;
            int CurrHour = 0;

            foreach (var ttLaborDtl_iterator in (from ttLaborDtl_Row in ds.LaborDtl
                                                 where ttLaborDtl_Row.LaborType.KeyEquals("I") &&
                                                       ttLaborDtl_Row.RowMod.Compare("U") == 0
                                                 select ttLaborDtl_Row).ToList())
            {
                using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                {
                    ttLaborDtl_iterator.ClockOutMinute = CompanyTime.Now().Minute;
                    ttLaborDtl_iterator.ClockOutTime = LibOffset.OffSetTime();
                    CurrTime = LibOffset.OffSetTime();
                    /* break out seconds */
                    CurrSec = (CurrTime % 60);
                    CurrTime = (CurrTime - CurrSec) / 60;
                    /* break out minutes */
                    CurrMinute = (CurrTime % 60);
                    /* break out hour */
                    CurrHour = (CurrTime - CurrMinute) / 60;
                    /* concatenate the two values */
                    HrMinTime = Compatibility.Convert.ToString(CurrHour, "99") + Compatibility.Convert.ToString(CurrMinute, "99");
                    /* convert it to a decimal value */
                    HrsMin = Compatibility.Convert.ToDecimal(HrMinTime);
                    /* convert to 2 decimal places */
                    HrsMin = HrsMin / 100;
                    /* convert to hours/hundreths */
                    HrsDec = Math.Truncate(HrsMin) + ((HrsMin - Math.Truncate(HrsMin)) * 100) / 60m;
                    ttLaborDtl_iterator.ClockOutTime = HrsDec;
                    /* convert input date and time to minutes since base date */
                    ttLaborDtl_iterator.ClockOutMinute = (((TimeSpan)(ttLaborDtl_iterator.ClockInDate - DAYZERO)).Days * 1440) + Compatibility.Convert.ToInt32(HrsMin * 60);
                    ttLaborDtl_iterator.DspClockOutTime = Compatibility.Convert.ToString(CurrHour, "99") + ":" + Compatibility.Convert.ToString(CurrMinute, "99");
                    var ParentLaborDtl = FindFirstParentLaborDtlWithUpdLock(ttLaborDtl_iterator.Company, ttLaborDtl_iterator.LaborHedSeq, ttLaborDtl_iterator.JobNum);
                    if (ParentLaborDtl != null)
                    {
                        ParentLaborDtl.Downtime = false;
                        ParentLaborDtl.IndirectCode = "";
                    }
                    Db.Validate(ParentLaborDtl);
                    txScope.Complete();
                }
            }
        }

        /// <summary>
        /// Description: Public method which retrieves the labor information HCM third party requires.
        /// </summary>
        /// <param name="employeeNum">String value with the list of employees</param>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="includeStatus">String value with status value</param>
        public HCMLaborDtlTableset HCMGetLaborRecords(string employeeNum, DateTime? startDate, DateTime? endDate, string includeStatus)
        {
            HCMLaborDtlTableset ttHCMLaborDtlTableSetDS = new HCMLaborDtlTableset();
            List<string> employeeList = new List<string>();
            List<LaborHCMRecord> LaborRecords = new List<LaborHCMRecord>();
            string sourceLabor = string.Empty;
            LaborHed tempLaborHed;
            if (endDate.Value < startDate.Value)
            {
                throw new BLException(Strings.InvalidDateRangeSelected, "LaborDtl");
            }

            employeeList = employeeNum.Split(Char.Parse(Ice.Constants.LIST_DELIM)).ToList<string>();

            foreach (string employee_Iterator in employeeList)
            {
                LaborRecords.Clear();
                if (string.IsNullOrEmpty(includeStatus))
                {
                    LaborRecords.AddRange(this.SelectLaborDtlJoin(Session.CompanyID, employee_Iterator, Convert.ToDateTime(startDate.ToShortDateString()), Convert.ToDateTime(endDate.ToShortDateString()), "A").ToList<LaborHCMRecord>());
                    LaborRecords.AddRange(this.SelectLaborHedJoin(Session.CompanyID, employee_Iterator, Convert.ToDateTime(startDate.ToShortDateString()), Convert.ToDateTime(endDate.ToShortDateString()), true).ToList<LaborHCMRecord>());
                }
                else
                {
                    LaborRecords.AddRange(this.SelectLaborDtlJoin2(Session.CompanyID, employee_Iterator, Convert.ToDateTime(startDate.ToShortDateString()), Convert.ToDateTime(endDate.ToShortDateString()), "A", includeStatus).ToList<LaborHCMRecord>());
                    LaborRecords.AddRange(this.SelectLaborHedJoin2(Session.CompanyID, employee_Iterator, Convert.ToDateTime(startDate.ToShortDateString()), Convert.ToDateTime(endDate.ToShortDateString()), true, includeStatus).ToList<LaborHCMRecord>());
                }
                foreach (LaborHCMRecord LaborDtlResult_iterator in LaborRecords)
                {
                    string type = "";
                    if (LaborDtlResult_iterator.LaborDtlSeq == 0) //Type = HDR
                    {
                        type = LaborDtlResult_iterator.HCMPayHoursCalcType;
                        LaborDtlResult_iterator.HCMEnabledAt = "HDR";
                    }
                    else
                    {
                        tempLaborHed = Tables.LaborHed.FindFirstByPrimaryKey(Db, Session.CompanyID, LaborDtlResult_iterator.LaborHedSeq);
                        if (!tempLaborHed.FeedPayroll) continue;
                        type = tempLaborHed.HCMPayHoursCalcType;
                        LaborDtlResult_iterator.HCMEnabledAt = "DTL";
                    }

                    type = (string.IsNullOrEmpty(type)) ? "DTL" : type;
                    if (!LaborDtlResult_iterator.HCMEnabledAt.Equals(type, StringComparison.OrdinalIgnoreCase)) continue;
                    if (LaborDtlResult_iterator.HCMLaborDtlSync == null)
                    {
                        using (TransactionScope txHCMLaborDtlSyncScope = ErpContext.CreateDefaultTransactionScope())
                        {

                            LaborDtlResult_iterator.HCMLaborDtlSync = new Erp.Tables.HCMLaborDtlSync();
                            Db.HCMLaborDtlSync.Insert(LaborDtlResult_iterator.HCMLaborDtlSync);
                            LaborDtlResult_iterator.HCMLaborDtlSync.Company = LaborDtlResult_iterator.Company;
                            LaborDtlResult_iterator.HCMLaborDtlSync.LaborDtlSysRowID = LaborDtlResult_iterator.SysRowID;
                            LaborDtlResult_iterator.HCMLaborDtlSync.Status = "IP";
                            Db.Validate(LaborDtlResult_iterator.HCMLaborDtlSync);
                            txHCMLaborDtlSyncScope.Complete();
                        }
                    }

                    if ((string.IsNullOrEmpty(includeStatus) && (LaborDtlResult_iterator.HCMLaborDtlSync.Status.Equals("IP", StringComparison.OrdinalIgnoreCase) || LaborDtlResult_iterator.HCMLaborDtlSync.Status.Equals("E", StringComparison.OrdinalIgnoreCase))) || (!string.IsNullOrEmpty(includeStatus) && (includeStatus.IndexOf(LaborDtlResult_iterator.HCMLaborDtlSync.Status.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)))
                    {
                        ttHCMLaborDtl = new Erp.Tablesets.HCMLaborDtlRow();
                        ttHCMLaborDtlTableSetDS.HCMLaborDtl.Add(ttHCMLaborDtl);
                        ttHCMLaborDtl.Company = LaborDtlResult_iterator.Company;
                        ttHCMLaborDtl.EmployeeNum = LaborDtlResult_iterator.EmployeeNum;
                        ttHCMLaborDtl.LaborHedSeq = LaborDtlResult_iterator.LaborHedSeq;
                        ttHCMLaborDtl.LaborDtlSeq = LaborDtlResult_iterator.LaborDtlSeq;
                        ttHCMLaborDtl.LaborTypePseudo = LaborDtlResult_iterator.LaborTypePseudo;
                        ttHCMLaborDtl.ClockInDate = LaborDtlResult_iterator.ClockInDate;
                        ttHCMLaborDtl.DspClockInTime = LaborDtlResult_iterator.DspClockInTime;
                        ttHCMLaborDtl.DspClockOutTime = LaborDtlResult_iterator.DspClockOutTime;
                        ttHCMLaborDtl.PayHours = LaborDtlResult_iterator.PayHours;
                        ttHCMLaborDtl.LaborNote = LaborDtlResult_iterator.LaborNote;
                        ttHCMLaborDtl.JCDept = LaborDtlResult_iterator.JCDept;
                        ttHCMLaborDtl.Shift = LaborDtlResult_iterator.Shift;
                        ttHCMLaborDtl.ProjectID = LaborDtlResult_iterator.ProjectID;
                        ttHCMLaborDtl.HCMEnabledAt = LaborDtlResult_iterator.HCMEnabledAt;
                        ttHCMLaborDtl.Status = LaborDtlResult_iterator.HCMLaborDtlSync.Status;
                        ttHCMLaborDtl.SysRowID = LaborDtlResult_iterator.SysRowID;
                    }
                }
            }

            return ttHCMLaborDtlTableSetDS;
        }

        /// <summary>
        /// </summary>        
        public void HCMSetLaborStatus(HCMLaborDtlTableset hcmDs)
        {
            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
            {
                foreach (HCMLaborDtlSyncRow ttHCMLaborDtlSync_iterator in hcmDs.HCMLaborDtlSync.ToList())
                {
                    try
                    {
                        //validate if the main fields have values
                        if (string.IsNullOrEmpty(ttHCMLaborDtlSync_iterator.Company) &&
                            string.IsNullOrEmpty(ttHCMLaborDtlSync_iterator.LaborSource) &&
                            !ttHCMLaborDtlSync_iterator.LaborDtlSysRowID.Equals(Guid.Empty) &&
                            string.IsNullOrEmpty(ttHCMLaborDtlSync_iterator.Status))
                        {
                            continue;
                        }

                        //validate if the company at least exists
                        if (!this.ExistsCompany(ttHCMLaborDtlSync_iterator.Company))
                            continue;

                        //validate the only two possible Labor source options (DTL/HDR)
                        if (!ttHCMLaborDtlSync_iterator.LaborSource.Equals("DTL", StringComparison.OrdinalIgnoreCase) && !ttHCMLaborDtlSync_iterator.LaborSource.Equals("HDR", StringComparison.OrdinalIgnoreCase))
                            continue;

                        //validate the Status belongs to any of the HCM integration
                        if (!ttHCMLaborDtlSync_iterator.Status.Equals("IP", StringComparison.OrdinalIgnoreCase) &&
                             !ttHCMLaborDtlSync_iterator.Status.Equals("E", StringComparison.OrdinalIgnoreCase) &&
                             !ttHCMLaborDtlSync_iterator.Status.Equals("S", StringComparison.OrdinalIgnoreCase) &&
                             !ttHCMLaborDtlSync_iterator.Status.Equals("X", StringComparison.OrdinalIgnoreCase) &&
                             !ttHCMLaborDtlSync_iterator.Status.Equals("R", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        HCMLaborDtlSync = this.FindFirstHCMLaborDtlWithUpdLock(ttHCMLaborDtlSync_iterator.Company, ttHCMLaborDtlSync_iterator.LaborDtlSysRowID);
                        if (HCMLaborDtlSync == null)
                        {
                            HCMLaborDtlSync = new Erp.Tables.HCMLaborDtlSync();
                            Db.HCMLaborDtlSync.Insert(HCMLaborDtlSync);
                            HCMLaborDtlSync.Company = ttHCMLaborDtlSync_iterator.Company;
                            HCMLaborDtlSync.LaborDtlSysRowID = ttHCMLaborDtlSync_iterator.LaborDtlSysRowID;
                            HCMLaborDtlSync.Status = ttHCMLaborDtlSync_iterator.Status;
                        }
                        else
                        {
                            HCMLaborDtlSync.Status = ttHCMLaborDtlSync_iterator.Status;
                        }
                        Db.Release(ref HCMLaborDtlSync);
                        if (ttHCMLaborDtlSync_iterator.LaborSource.Equals("HDR", StringComparison.OrdinalIgnoreCase) && ttHCMLaborDtlSync_iterator.Status.Equals("S", StringComparison.OrdinalIgnoreCase))
                        {
                            Erp.Tables.LaborHed tmLaborHead = LaborHed.FindFirstBySysRowIDWithUpdLock(Db, ttHCMLaborDtlSync_iterator.LaborDtlSysRowID);
                            tmLaborHead.TransferredToPayroll = true;
                            Db.Release(ref tmLaborHead);

                        }
                    }
                    catch
                    {
                        //if any exception is raised during the updating then nothing will happens, TBD
                        Db.DetachAll();
                    }



                }
                txScope.Complete();
            }
        }

        /// <summary>
        /// Validates if there is no valid Charge Rate according to selected Time Type.
        /// This validation can also be found on BO/LaborApproval.
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="vMessage">Warning message</param>
        public void ValidateChargeRateForTimeType(ref LaborTableset ds, out string vMessage)
        {
            vMessage = string.Empty;
            CurrentFullTableset = ds;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where ttLaborDtl_Row.RowMod.Equals("U", StringComparison.OrdinalIgnoreCase) || ttLaborDtl_Row.RowMod.Equals("A", StringComparison.OrdinalIgnoreCase)
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl != null)
            {
                Erp.Tables.JobHead tmJobHead = new Tables.JobHead();
                Erp.Tables.Project tmProject = new Tables.Project();

                tmJobHead = this.FindFirstJobHead16(Session.CompanyID, ttLaborDtl.JobNum);
                if (tmJobHead != null)
                {
                    tmProject = this.FindFirstProject4(Session.CompanyID, tmJobHead.ProjectID);
                    if (tmProject != null && tmProject.ConInvMeth.Equals("TM", StringComparison.OrdinalIgnoreCase))
                    {
                        bool gotRate = false;
                        if (tmProject.PBPrjRtSrc.Equals("HIER", StringComparison.OrdinalIgnoreCase) || tmProject.PBPrjRtSrc.Equals("PROJ", StringComparison.OrdinalIgnoreCase))
                        {
                            if (this.ExistsPBRoleRt(Session.CompanyID, tmJobHead.ProjectID, ttLaborDtl.RoleCd, ttLaborDtl.TimeTypCd))
                            {
                                gotRate = true;
                            }
                        }
                        if ((tmProject.PBPrjRtSrc.Equals("HIER", StringComparison.OrdinalIgnoreCase) && !gotRate) || tmProject.PBPrjRtSrc.Equals("EMPL", StringComparison.OrdinalIgnoreCase))
                        {
                            if (this.ExistsEmpRoleRt(Session.CompanyID, ttLaborDtl.EmployeeNum, ttLaborDtl.RoleCd, ttLaborDtl.TimeTypCd))
                            {
                                gotRate = true;
                            }
                        }
                        if ((tmProject.PBPrjRtSrc.Equals("HIER", StringComparison.OrdinalIgnoreCase) && !gotRate) || tmProject.PBPrjRtSrc.Equals("ROLE", StringComparison.OrdinalIgnoreCase))
                        {
                            if (this.ExistsPrjRoleRt(Session.CompanyID, ttLaborDtl.RoleCd, ttLaborDtl.TimeTypCd))
                            {
                                gotRate = true;
                            }
                        }
                        if (!gotRate)
                        {
                            vMessage = Strings.TimeTypeChangeRateWarning;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Review if the document is Lock when user tries to recall the record from UI    
        /// </summary>
        /// <param name="laborHedSeq">Labor Hed Sequence</param>
        /// <param name="laborDtlSeq">Labor Detail Sequence</param>
        /// <param name="vMessage">Error message</param>
        public void ReviewIsDocumentLock(string laborHedSeq, string laborDtlSeq, out string vMessage)
        {
            vMessage = string.Empty;
            if (PELock.IsDocumentLock(Session.CompanyID, "LaborDtl", laborHedSeq, laborDtlSeq, "", "", "", ""))
            {
                vMessage = PELock.LockMessage;
            }
        }

        /// <summary>
        /// Returns true if HCM is enable at company level.
        /// </summary>
        /// <returns></returns>
        public bool IsHCMEnabledAtCompany()
        {
            return this.IsHCMEnabledAtCompany(Session.CompanyID, true);
        }

        private string isHCMEnabledAt(string employeeID)
        {
            string result = string.Empty;

            if (this.IsHCMEnabledAtCompany(Session.CompanyID, true))
            {
                if (!string.IsNullOrEmpty(employeeID) && this.IsHCMEnabledByEmployee(Session.CompanyID, Session.PlantID, true))
                {
                    result = this.GetHCMValueAtEmpl(Session.CompanyID, employeeID);
                    if (result.Equals("NON", StringComparison.OrdinalIgnoreCase))
                    {
                        result = this.GetHCMValueAtSite(Session.CompanyID, Session.PlantID);
                    }
                }
                else
                {
                    result = this.GetHCMValueAtSite(Session.CompanyID, Session.PlantID);
                }
            }
            else
            {
                result = this.GetHCMValueAtSite(Session.CompanyID, Session.PlantID);
            }

            return result;
        }

        private void getSerialNumbersForDelete()
        {
            getSerialNumbers();

            foreach (var ttLbrScrapSerialNumbersRow in (from ttLbrScrapSerialNumbers_Row in CurrentFullTableset.LbrScrapSerialNumbers
                                                        where ttLbrScrapSerialNumbers_Row.Company.Compare(ttLaborDtl.Company) == 0
                                                        && ttLbrScrapSerialNumbers_Row.LaborHedSeq == ttLaborDtl.LaborHedSeq
                                                        && ttLbrScrapSerialNumbers_Row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq
                                                        select ttLbrScrapSerialNumbers_Row))
            {
                ttLbrScrapSerialNumbersRow.RowMod = IceRow.ROWSTATE_DELETED;
            }
        }

        #region EmpExpenseAttach Before/After methods
        partial void LaborDtlAttchAfterCreate()
        {
            using (MobileNotification libMobileNotification = new MobileNotification(Db))
            {
                var laborDtlPartialRow = FindFirstLaborDtlPartialRow(ttLaborDtlAttch.Company, ttLaborDtlAttch.LaborHedSeq, ttLaborDtlAttch.LaborDtlSeq);

                if (laborDtlPartialRow != null)
                {
                    libMobileNotification.NewAttachmentNotification(
                        ttLaborDtlAttch.Company,
                        laborDtlPartialRow.CreatedBy,
                        laborDtlPartialRow.SysRowID,
                        Session.SystemCode,
                        "LaborDtl");
                }
            }

        }
        #endregion


        #region FSA specific logic for FSAExtData
        private void validateFSAExtFlds(LaborDtlRow laborDtl)
        {
            // Can only set FSAExtData fields if EpicorFSA flag is set
            ValidateValuesAreNotSetIfNotEpicorFSATransaction(laborDtl);

            if (!laborDtl.EpicorFSA)
                return;

            // Validate license and values set for fields that require db validation
            using (var libFSA = new Internal.SI.FSA.FSAExtDataUtil(Db))
            {
                string errMsg;
                if (!libFSA.CheckFSALicense(out errMsg))
                    throw new BLException(errMsg);
                if (!libFSA.ValidateCallCode(Session.CompanyID, laborDtl.CallCode, out errMsg))
                    throw new BLException(errMsg);
                if (!libFSA.ValidateContractCode(Session.CompanyID, laborDtl.ContractCode, out errMsg))
                    throw new BLException(errMsg);
                if (!libFSA.ValidateContractNum(Session.CompanyID, laborDtl.ContractNum, out errMsg))
                    throw new BLException(errMsg);
                if (!libFSA.ValidateWarrantyCode(Session.CompanyID, laborDtl.WarrantyCode, out errMsg))
                    throw new BLException(errMsg);
                if (!libFSA.ValidateEquipmentPartNum(Session.CompanyID, laborDtl.FSAEquipmentPartNum, out errMsg))
                    throw new BLException(errMsg);
            }
        }

        internal static void ValidateValuesAreNotSetIfNotEpicorFSATransaction(LaborDtlRow laborDtl)
        {
            if (laborDtl.EpicorFSA)
                return;

            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(laborDtl.CallCode))
                sb.AppendLine(Strings.FSACallCodeOnlyForFSA);
            if (!string.IsNullOrEmpty(laborDtl.ContractCode))
                sb.AppendLine(Strings.FSAContractCodeOnlyForFSA);
            if (!string.IsNullOrEmpty(laborDtl.WarrantyCode))
                sb.AppendLine(Strings.FSAWarrantyCodeOnlyForFSA);
            if (laborDtl.FSAServiceOrderNum > 0)
                sb.AppendLine(Strings.FSAServiceOrderNumberOnlyForFSA);
            if (laborDtl.FSAServiceOrderResourceNum > 0)
                sb.AppendLine(Strings.FSAServiceOrderResourceNumberOnlyForFSA);
            if (laborDtl.ContractNum > 0)
                sb.AppendLine(Strings.FSAContractNumOnlyForFSA);
            if (!string.IsNullOrEmpty(laborDtl.FSAEquipmentPartNum))
                sb.AppendLine(Strings.FSAEquipmentPartNumOnlyForFSA);
            if (laborDtl.FSAEquipmentInstallID > 0)
                sb.AppendLine(Strings.FSAEquipmentInstallIDOnlyForFSA);
            if (!string.IsNullOrEmpty(laborDtl.FSAAction))
                sb.AppendLine(Strings.FSAActionOnlyForFSA);

            if (sb.Length > 0)
                throw new BLException(sb.ToString());
        }

        private void writeFSAExtFlds(ref LaborDtlRow laborDtl)
        {
            var laborDtlPartial = FindFirstLaborDtlSysRowID(Session.CompanyID, laborDtl.LaborHedSeq, laborDtl.LaborDtlSeq);
            if (laborDtlPartial == null)
                return;

            if (!laborDtlPartial.EpicorFSA)
            {
                deleteFSAExtFlds(laborDtlPartial.SysRowID);
                return;
            }

            using (var libFSA = new Internal.SI.FSA.FSAExtDataUtil(Db))
            {
                bool alreadyExists;
                var fsaExtData = libFSA.GetFSAExtData(Session.CompanyID, "LaborDtl", laborDtlPartial.SysRowID, out alreadyExists);
                fsaExtData.CallCode = laborDtl.CallCode;
                fsaExtData.ContractCode = laborDtl.ContractCode;
                fsaExtData.WarrantyCode = laborDtl.WarrantyCode;
                fsaExtData.FSAServiceOrderNum = laborDtl.FSAServiceOrderNum;
                fsaExtData.FSAServiceOrderResourceNum = laborDtl.FSAServiceOrderResourceNum;
                fsaExtData.ContractNum = laborDtl.ContractNum;
                fsaExtData.FSAEquipmentPartNum = laborDtl.FSAEquipmentPartNum;
                fsaExtData.FSAEquipmentInstallID = laborDtl.FSAEquipmentInstallID;
                fsaExtData.FSAAction = laborDtl.FSAAction;
                if (!alreadyExists)
                    Db.FSAExtData.Insert(fsaExtData);
                else
                    Db.Validate(fsaExtData);
            }
        }

        private void readFSAExtFlds(ref LaborDtlRow laborDtl)
        {
            if (!laborDtl.EpicorFSA)
                return;

            using (var libFSA = new Internal.SI.FSA.FSAExtDataUtil(Db))
            {
                var fsaExtData = libFSA.ReadFSAExtData(Session.CompanyID, "LaborDtl", laborDtl.SysRowID);
                if (fsaExtData != null)
                {
                    laborDtl.CallCode = fsaExtData.CallCode;
                    laborDtl.ContractCode = fsaExtData.ContractCode;
                    laborDtl.WarrantyCode = fsaExtData.WarrantyCode;
                    laborDtl.FSAServiceOrderNum = fsaExtData.FSAServiceOrderNum;
                    laborDtl.FSAServiceOrderResourceNum = fsaExtData.FSAServiceOrderResourceNum;
                    laborDtl.ContractNum = fsaExtData.ContractNum;
                    laborDtl.FSAEquipmentPartNum = fsaExtData.FSAEquipmentPartNum;
                    laborDtl.FSAEquipmentInstallID = fsaExtData.FSAEquipmentInstallID;
                    laborDtl.FSAAction = fsaExtData.FSAAction;
                }
            }
        }

        private void deleteFSAExtFlds(Guid sysRowID)
        {
            using (var libFSA = new Internal.SI.FSA.FSAExtDataUtil(Db))
            {
                libFSA.DeleteFSAExtData(Session.CompanyID, "LaborDtl", sysRowID);
            }
        }
        #endregion

        private void updateFieldsForCalendarView(ref LaborDtlRow ttLaborDtlRow)
        {
            updateLaborDtlAppointmentDates(ref ttLaborDtl);
            updateLaborDtlAppointmentTitle(ref ttLaborDtl);
        }

        private void updateLaborDtlAppointmentDates(ref LaborDtlRow ttLaborDtl)
        {
            TimeSpan startTimeSpan = TimeSpan.FromHours(decimal.ToDouble(ttLaborDtl.ClockinTime));
            DateTime startDateTime = ttLaborDtl.PayrollDate.Value;
            startDateTime = startDateTime.Add(startTimeSpan);

            TimeSpan endTimeSpan = TimeSpan.FromHours(decimal.ToDouble(ttLaborDtl.ClockOutTime));
            DateTime endDateTime = ttLaborDtl.PayrollDate.Value;
            endDateTime = endDateTime.Add(endTimeSpan);

            ttLaborDtl.AppointmentStart = startDateTime;
            ttLaborDtl.AppointmentEnd = endDateTime;
        }

        private void updateLaborDtlAppointmentTitle(ref LaborDtlRow ttLaborDtl)
        {
            string appointmentTitle = Strings.LaborHours + ": " + ttLaborDtl.LaborHrs.ToString() + " | ";
            string timeStatus = String.Empty;

            switch (ttLaborDtl.TimeStatus.ToUpperInvariant())
            {
                case "S":
                    timeStatus = Strings.Submitted;
                    break;
                case "R":
                    timeStatus = Strings.Rejected;
                    break;
                case "P":
                    timeStatus = Strings.PartiallyApproved;
                    break;
                case "A":
                    timeStatus = Strings.Approved;
                    break;
                case "E":
                case "":
                    timeStatus = Strings.Entered;
                    break;
            }

            switch (ttLaborDtl.LaborTypePseudo.ToUpperInvariant())
            {
                case "I":   // Indirect
                    appointmentTitle +=
                        Strings.Indirect + ": " + ttLaborDtl.IndirectDescription +
                        " | " + Strings.Status + ": " + timeStatus;
                    break;
                case "P":   // Production
                    appointmentTitle += Strings.Job + ": " + ttLaborDtl.JobNum +
                                        " | " + Strings.Opr + ": " + ttLaborDtl.OprSeq.ToString() +
                                        " | " + Strings.Status + ": " + timeStatus;
                    break;
                case "J":   // Project
                case "V":   // Service
                    appointmentTitle += Strings.Project + ": " + ttLaborDtl.ProjectID +
                                        " | " + Strings.PhaseID + ": " + ttLaborDtl.PhaseIDDescription;

                    if (!String.IsNullOrEmpty(ttLaborDtl.TimeTypCdDescription))
                        appointmentTitle += " | " + Strings.TimeType + ": " + ttLaborDtl.TimeTypCdDescription;

                    appointmentTitle += " | " + Strings.Job + ": " + ttLaborDtl.PhaseJobNum +
                                        " | " + Strings.Opr + ": " + ttLaborDtl.PhaseOprSeq.ToString() +
                                        " | " + Strings.Role + ": " + ttLaborDtl.RoleCdRoleDescription +
                                        " | " + Strings.Status + ": " + timeStatus;
                    break;
                case "S":   // Setup
                    appointmentTitle += Strings.Job + ": " + ttLaborDtl.JobNum +
                                        " | " + Strings.Opr + ": " + ttLaborDtl.OprSeq.ToString() +
                                        " | " + Strings.Status + ": " + timeStatus;

                    break;
            }

            appointmentTitle +=
                " | " + Strings.ClockInDate + ": " + ttLaborDtl.ClockInDate.Value.ToShortDateString();

            ttLaborDtl.AppointmentTitle = appointmentTitle;

        }

        #region Methods for MetaUI
        /// <summary>
        /// Called when Time Weekly View Week Begin Date is changing
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="weekBeginDate">Proposed week begin date</param>
        public void ChangeTimeWeeklyViewWeekBeginDate(ref LaborTableset ds, DateTime weekBeginDate)
        {
            if (ttTimeWeeklyView == null)
            {
                ttTimeWeeklyView = (from ttTimeWeeklyView_Row in ds.TimeWeeklyView
                                    where modList.Lookup(ttTimeWeeklyView_Row.RowMod) != -1
                                    select ttTimeWeeklyView_Row).FirstOrDefault();
            }

            if (ttTimeWeeklyView == null)
            {
                throw new BLException(Strings.LaborHasNotChanged, "TimeWeeklyView");
            }

            if (weekBeginDate.DayOfWeek != System.DayOfWeek.Sunday)
            {
                throw new BLException(Strings.WeekBeginDateSunday);
            }
            else
            {
                var weekEndDate = weekBeginDate = weekBeginDate.AddDays(6);
                ttTimeWeeklyView.WeekEndDate = weekEndDate;
            }
        }

        /// <summary>
        /// This method defaults LaborDtl fields when Operation sequence changes.  Also returns any
        /// warnings user needs to know.
        /// </summary>
        /// <param name="oprSeq">Proposed oprSeq change </param>
        /// <param name="message">Returns warnings for user's review</param>
        /// <param name="ds">Labor Entry Data set</param>
        public void ChangeLaborDtlOprSeq(ref LaborTableset ds, int oprSeq, out string message)
        {
            message = String.Empty;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();

            if (ttLaborDtl != null)
            {
                ttLaborDtl.ReWork = false;
                using (var laborService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.LaborSvcContract>(Db))
                {
                    laborService.DefaultOprSeq(ref ds, oprSeq, out message);
                }
            }
            this.CurrentFullTableset = ds;
            this.getLaborDtlActionDS();

        }

        /// <summary>
        /// This method defaults fields when the scrap qty field changes.  
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>        
        /// <param name="scrapQty">Proposed change to ScrapQty field </param>
        public void ChangeLaborDtlScrapQty(ref LaborTableset ds, decimal scrapQty)
        {

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            string message = String.Empty;

            using (var laborService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.LaborSvcContract>(Db))
            {
                laborService.VerifyScrapQty(ref ds, scrapQty, out message);
            }

            if (ttLaborDtl.ScrapQty == 0)
            {
                ttLaborDtl.ScrapReasonCode = String.Empty;
            }
        }


        /// <summary>
        /// This method updates the revision field when the attribute ID field changes.  
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>        
        /// <param name="attributeSetID">Proposed change to ScrapQty field </param>
        /// <param name="type">Discrep, Scrap or Labor</param>
        public void ChangeLaborDtlAttributeSetID(ref LaborTableset ds, int attributeSetID, string type)
        {
            string revisionNum = string.Empty;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            var jobAsmblPart = FindFirstJobAsmblPart(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
            if (jobAsmblPart != null)
            {
                PartPartial part = FindFirstPartPartial(Session.CompanyID, jobAsmblPart.PartNum);
                if (part == null || !part.TrackInventoryByRevision)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            using (var libInventoryByRevision = new Internal.Lib.InventoryByRevision(Db))
            {
                revisionNum = libInventoryByRevision.GetRevisionNumFromDynAttrValueSet(attributeSetID);
            }

            if (type.KeyEquals("L"))
            {
                ttLaborDtl.LaborRevision = revisionNum;
            }

            if (type.KeyEquals("S"))
            {
                ttLaborDtl.ScrapRevision = revisionNum;
            }

            if (type.KeyEquals("D"))
            {
                ttLaborDtl.DiscrepRevision = revisionNum;
            }
        }

        /// <summary>
        /// Called after LaborDtl.DiscrepQty has been changed.
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="message">Return message</param>
        public void AfterChangeLaborDtlDiscrepQty(ref LaborTableset ds, out string message)
        {
            message = String.Empty;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            if (ttLaborDtl.DiscrepQty == 0)
            {
                ttLaborDtl.DiscrpRsnCode = String.Empty;
            }
            else
            {
                using (var laborService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.LaborSvcContract>(Db))
                {
                    laborService.validateNonConfProcessed(ref ds, ttLaborDtl.LaborHedSeq, ttLaborDtl.LaborDtlSeq, ttLaborDtl.DiscrepQty, out message);
                }
            }
        }

        /// <summary>
        /// Called when labor clock in or clock out time is changing
        /// </summary>
        /// <param name="fieldName">The name of the field that is changing</param>
        /// <param name="timeValue">The new time value</param>
        /// <param name="ds">Labor Entry Data set</param>
        public void ChangeLaborDtlTimeField(string fieldName, decimal timeValue, ref LaborTableset ds)
        {
            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            using (var laborService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.LaborSvcContract>(Db))
            {
                string displayTime = String.Empty;
                laborService.GetDspClockTime(timeValue, out displayTime);

                if (fieldName.Equals("ClockinTime", StringComparison.OrdinalIgnoreCase))
                {
                    ttLaborDtl.DspClockInTime = displayTime;
                    ttLaborDtl.ClockinTime = timeValue;
                }
                else
                {
                    ttLaborDtl.DspClockOutTime = displayTime;
                    ttLaborDtl.ClockOutTime = timeValue;
                }

                decimal workTime = timeValue;
                workTime = Math.Floor(workTime) + (Math.Round((workTime - Math.Floor(workTime)) * 0.6M, 2) / 0.6M);

                calculateHours(ref ttLaborDtl, fieldName, workTime);

                laborService.DefaultDtlTime(ref ds);
            }
        }

        /// <summary>
        /// Called when labor display clock in or clock out time is changing
        /// </summary>
        /// <param name="fieldName">The name of the field that is changing</param>
        /// <param name="timeValue">The new time value</param>
        /// <param name="ds">Labor Entry Data set</param>
        public void ChangeLaborDtlDspTimeField(string fieldName, string timeValue, ref LaborTableset ds)
        {
            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            using (var laborService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.LaborSvcContract>(Db))
            {
                decimal workClockTime = 0;
                string clockFormat = "^(\\d{1,2}(\\.|\\:)\\d{2})$";

                if (System.Text.RegularExpressions.Regex.IsMatch(timeValue, clockFormat, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    laborService.GetClockTime(timeValue, out workClockTime);

                    if (fieldName.Equals("DspClockInTime", StringComparison.OrdinalIgnoreCase))
                    {
                        ttLaborDtl.ClockinTime = workClockTime;
                        calculateHours(ref ttLaborDtl, "ClockinTime", workClockTime);
                    }
                    else
                    {
                        ttLaborDtl.ClockOutTime = workClockTime;
                        calculateHours(ref ttLaborDtl, "ClockOutTime", workClockTime);
                    }

                    laborService.DefaultDtlTime(ref ds);
                }
            }
        }

        /// <summary>
        /// Returns company job production settings for Advance Labor Rate, Clock Format
        /// </summary>
        /// <param name="advanceLaborRate">Advance Labor Rate value</param>
        /// <param name="clockFormat">Clock format value</param>
        public void GetJobProductionCompanySettings(out bool advanceLaborRate, out string clockFormat)
        {
            advanceLaborRate = false;
            clockFormat = String.Empty;

            var jcSystRow = GetJCSystJobProductionValues(Session.CompanyID);

            if (jcSystRow != null)
            {
                advanceLaborRate = jcSystRow.AdvancedLaborRate;
                clockFormat = jcSystRow.ClockFormat;
            }
        }

        private void calculateHours(ref LaborDtlRow ttLaborDtlRow, string fieldName, decimal timeValue)
        {
            decimal inTime = 0;
            decimal outTime = 0;
            decimal vAuxTime = 0;
            if (ttLaborDtlRow.Downtime == true) return;

            if (fieldName.Equals("ClockinTime", StringComparison.OrdinalIgnoreCase))
            {
                inTime = timeValue;
                vAuxTime = ttLaborDtlRow.ClockOutTime;
                outTime = Math.Floor(vAuxTime) + (Math.Round((vAuxTime - Math.Floor(vAuxTime)) * 0.6M, 2) / 0.6M);
            }
            else
            {
                vAuxTime = ttLaborDtlRow.ClockinTime;
                inTime = Math.Floor(vAuxTime) + (Math.Round((vAuxTime - Math.Floor(vAuxTime)) * 0.6M, 2) / 0.6M);
                outTime = timeValue;
            }

            var laborHedRow = FindFirstLaborHed(Session.CompanyID, ttLaborDtlRow.LaborHedSeq);

            if (laborHedRow == null) return;

            decimal lunchIn = laborHedRow.LunchInTime;
            decimal lunchOut = laborHedRow.LunchOutTime;
            decimal lunch = lunchIn - lunchOut;
            decimal day = 24;
            decimal hours = outTime - inTime;
            decimal hold = 0;
            if ((inTime <= lunchOut && outTime >= lunchIn) || (hours < 0))
                hours = outTime - inTime - lunch;
            else
                hours = outTime - inTime;
            if (hours < 0)
            {
                hold = hours;
                hours = day + hold;
            }
            hours = Math.Round(hours, 2);

            ttLaborDtlRow.LaborHrs = hours;
            ttLaborDtlRow.BurdenHrs = hours;
        }

        private string getRoleCdList(string employeeNum, string projectID, string phaseID, string jobNum, int assemblySeq, int oprSeq)
        {
            string roleCdList = string.Empty;

            var workList = buildValidRoleCodeList(employeeNum, projectID, phaseID, jobNum, assemblySeq, oprSeq);

            if (workList.NumEntries(Ice.Constants.LIST_DELIM[0]) > 0)
            {
                for (iCounter = 1; iCounter <= workList.NumEntries(Ice.Constants.LIST_DELIM[0]); iCounter++)
                {
                    var roleCodeAUX = workList.Entry(iCounter - 1, Ice.Constants.LIST_DELIM);
                    var roleCdRow = FindFirstRoleCd(Session.CompanyID, roleCodeAUX);

                    if (roleCdRow != null)
                    {
                        if (!String.IsNullOrEmpty(roleCdList))
                        {
                            roleCdList += Ice.Constants.LIST_DELIM;
                        }

                        roleCdList += roleCdRow.RoleCode + Ice.Constants.SUBLIST_DELIM + roleCdRow.RoleDescription;

                    }
                }
            }

            return roleCdList;
        }

        /// <summary>
        /// Method to submit Labor for Approval using RowSelected flag. 
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="weeklyView">Is this method being called with WeeklyView records?</param>
        /// <param name="messageText">Message text to present to the user after the process is finished </param>
        public void SubmitForApprovalBySelected(ref LaborTableset ds, bool weeklyView, out string messageText)
        {
            messageText = string.Empty;

            if (weeklyView == true)
            {
                foreach (var timeWeeklyRow in (from timeWeeklyView_Row in ds.TimeWeeklyView
                                               where String.IsNullOrEmpty(timeWeeklyView_Row.RowMod) ||
                                               timeWeeklyView_Row.RowSelected == false
                                               select timeWeeklyView_Row).ToList())
                {
                    ds.TimeWeeklyView.Remove(timeWeeklyRow);
                }
            }
            else
            {
                foreach (var laborDtlRow in (from laborDtl_Row in ds.LaborDtl
                                             where String.IsNullOrEmpty(laborDtl_Row.RowMod) ||
                                             laborDtl_Row.RowSelected == false
                                             select laborDtl_Row).ToList())
                {
                    ds.LaborDtl.Remove(laborDtlRow);
                }
            }

            using (var laborService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.LaborSvcContract>(Db))
            {
                laborService.SubmitForApproval(ref ds, weeklyView, out messageText);
            }
        }

        /// <summary>
        /// Method to recall Labor for Approval using RowSelected flag.
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="weeklyView">Is this method being called with WeeklyView records?</param>
        /// <param name="messageText">Message text to present to the user after the process is finished </param>
        public void RecallFromApprovalBySelected(ref LaborTableset ds, bool weeklyView, out string messageText)
        {
            messageText = String.Empty;

            if (weeklyView == true)
            {
                foreach (var timeWeeklyRow in (from timeWeeklyView_Row in ds.TimeWeeklyView
                                               where String.IsNullOrEmpty(timeWeeklyView_Row.RowMod) ||
                                               timeWeeklyView_Row.RowSelected == false
                                               select timeWeeklyView_Row).ToList())
                {
                    ds.TimeWeeklyView.Remove(timeWeeklyRow);
                }
            }
            else
            {
                foreach (var laborDtlRow in (from laborDtl_Row in ds.LaborDtl
                                             where String.IsNullOrEmpty(laborDtl_Row.RowMod) ||
                                             laborDtl_Row.RowSelected == false
                                             select laborDtl_Row).ToList())
                {
                    ds.LaborDtl.Remove(laborDtlRow);
                }
            }

            using (var laborService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.LaborSvcContract>(Db))
            {
                laborService.RecallFromApproval(ref ds, weeklyView, out messageText);
            }
        }

        /// <summary>
        /// Method to copy Labor detail record(s) using RowSelected flag.
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="messageText">Message text to present to the user after the process is finished </param>
        public void CopyLaborDtlBySelected(ref LaborTableset ds, out string messageText)
        {
            messageText = String.Empty;


            foreach (var laborDtlRow in (from laborDtl_Row in ds.LaborDtl
                                         select laborDtl_Row).ToList())
            {
                if (laborDtlRow.RowSelected == false)
                {
                    ds.LaborDtl.Remove(laborDtlRow);
                }
                else
                {
                    laborDtlRow.RowSelected = false;
                    laborDtlRow.RowMod = IceRow.ROWSTATE_UPDATED;
                }
            }

            using (var laborService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.LaborSvcContract>(Db))
            {
                laborService.CopyLaborDetail(ref ds, out messageText);
            }

            // Remove ttLaborPart records created in CopyLaborDetail.  They will be recreated on save of LaborDtl when necessary
            ds.LaborPart.Clear();
        }

        /// <summary>
        /// Method to copy TimeWeeklyView record(s) using RowSelected flag.
        /// </summary>
        /// <param name="ds">The Labor data set </param>
        /// <param name="messageText">Message text to present to the user after the process is finished </param>
        public void CopyTimeWeeklyViewBySelected(ref LaborTableset ds, out string messageText)
        {
            messageText = String.Empty;


            foreach (var timeWeeklyViewRow in (from timeWeeklyView_row in ds.TimeWeeklyView
                                               select timeWeeklyView_row).ToList())
            {
                if (timeWeeklyViewRow.RowSelected == false)
                {
                    ds.TimeWeeklyView.Remove(timeWeeklyViewRow);
                }
                else
                {
                    timeWeeklyViewRow.RowSelected = false;
                    timeWeeklyViewRow.RowMod = IceRow.ROWSTATE_UPDATED;
                }
            }

            using (var laborService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.LaborSvcContract>(Db))
            {
                laborService.CopyTimeWeeklyView(ref ds, out messageText);
            }
        }

        /// <summary>
        /// Get rows for Time Entry.  This method will consider user time retrieval options for retrieving approved, entered, partially approved, rejected, and submitted records.
        /// </summary>
        /// <param name="whereClauseLaborHed">LaborHed where clause</param>
        /// <param name="whereClauseLaborDtl">LaborDtl where clause</param>
        /// <param name="whereClauseLaborDtlAttach">LaborDtlAttach where clause</param>
        /// <param name="whereClauseLaborDtlAction">LaborDtlAction where clause</param>
        /// <param name="whereClauseLaborDtlCom">LaborDtlCom where clause</param>
        /// <param name="whereClauseLaborEquip">LaborEquip where clause</param>
        /// <param name="whereClauseLaborPart">LaborPart where clause</param>
        /// <param name="whereClauseLbrScrapSerialNumbers">LbrScrapSerialNumbers where clause</param>
        /// <param name="whereClauseTimeWorkHours">LaborTimeWorkHours where clause</param>
        /// <param name="whereClauseTimeWeeklyView">LaborTimeWeeklyView where clause</param>
        /// <param name="whereClauseLaborDtlGroup">LaborDtlGroup where clause</param>
        /// <param name="whereClauseSelectedSerialNumbers">SelectedSerialNumbers where clause</param>
        /// <param name="whereClauseSNFormat">SNFormat where clause</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="absolutePage">Absolute page</param>
        /// <param name="employeeNum">Employee number</param>
        /// <param name="calendarStartDate">Calendar start date</param>
        /// <param name="calendarEndDate">Calendar end date</param>
        /// <param name="morePages">More pages</param>
        /// <returns></returns>
        public LaborTableset GetRowsTimeEntry(
            string whereClauseLaborHed,
            string whereClauseLaborDtl,
            string whereClauseLaborDtlAttach,
            string whereClauseLaborDtlAction,
            string whereClauseLaborDtlCom,
            string whereClauseLaborEquip,
            string whereClauseLaborPart,
            string whereClauseLbrScrapSerialNumbers,
            string whereClauseTimeWorkHours,
            string whereClauseTimeWeeklyView,
            string whereClauseLaborDtlGroup,
            string whereClauseSelectedSerialNumbers,
            string whereClauseSNFormat,
            int pageSize,
            int absolutePage,
            string employeeNum,
            DateTime? calendarStartDate,
            DateTime? calendarEndDate,
            out bool morePages)
        {
            LaborTableset ds = new LaborTableset();
            morePages = false;

            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.TimeManagement) == false &&
                Session.ModuleLicensed(Erp.License.ErpLicensableModules.JobManagement) == false)
            {
                return ds;
            }

            string laborDtlWhereClauseAppend = getLaborDtlWhereClauseRetrieveOptions();

            if (!String.IsNullOrEmpty(laborDtlWhereClauseAppend))
            {
                if (!String.IsNullOrEmpty(whereClauseLaborDtl))
                {
                    whereClauseLaborDtl += " AND ";
                }

                whereClauseLaborDtl += laborDtlWhereClauseAppend;
            }

            using (var laborService = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.LaborSvcContract>(Db))
            {
                ds = laborService.GetRowsCalendarView(
                        whereClauseLaborHed,
                        whereClauseLaborDtl,
                        whereClauseLaborDtlAttach,
                        whereClauseLaborDtlAction,
                        whereClauseLaborDtlCom,
                        whereClauseLaborEquip,
                        whereClauseLaborPart,
                        whereClauseLbrScrapSerialNumbers,
                        whereClauseTimeWorkHours,
                        whereClauseTimeWeeklyView,
                        whereClauseLaborDtlGroup,
                        whereClauseSelectedSerialNumbers,
                        whereClauseSNFormat,
                        pageSize,
                        absolutePage,
                        employeeNum,
                        calendarStartDate,
                        calendarEndDate,
                        out morePages);
            }

            return ds;

        }

        private string getLaborDtlWhereClauseRetrieveOptions()
        {
            string laborDtlWhereClauesRetrieveOptions = String.Empty;

            var userFile = FindFirstUserFile(Session.UserID);

            if (userFile != null)
            {
                if (userFile.TERetrieveApproved == false)
                {
                    laborDtlWhereClauesRetrieveOptions = "TimeStatus <> 'A'";
                }

                if (userFile.TERetrieveEntered == false)
                {
                    if (!String.IsNullOrEmpty(laborDtlWhereClauesRetrieveOptions))
                    {
                        laborDtlWhereClauesRetrieveOptions += " AND ";
                    }

                    laborDtlWhereClauesRetrieveOptions += "TimeStatus <> 'E' AND TimeStatus <> ''";
                }

                if (userFile.TERetrievePartiallyApproved == false)
                {
                    if (!String.IsNullOrEmpty(laborDtlWhereClauesRetrieveOptions))
                    {
                        laborDtlWhereClauesRetrieveOptions += " AND ";
                    }

                    laborDtlWhereClauesRetrieveOptions += "TimeStatus <> 'P'";
                }

                if (userFile.TERetrieveRejected == false)
                {
                    if (!String.IsNullOrEmpty(laborDtlWhereClauesRetrieveOptions))
                    {
                        laborDtlWhereClauesRetrieveOptions += " AND ";
                    }

                    laborDtlWhereClauesRetrieveOptions += "TimeStatus <> 'R'";
                }

                if (userFile.TERetrieveSubmitted == false)
                {
                    if (!String.IsNullOrEmpty(laborDtlWhereClauesRetrieveOptions))
                    {
                        laborDtlWhereClauesRetrieveOptions += " AND ";
                    }

                    laborDtlWhereClauesRetrieveOptions += "TimeStatus <> 'S'";
                }
            }

            return laborDtlWhereClauesRetrieveOptions;
        }

        private void assignTimeEntryCommentTypeList(ref LaborDtlCommentRow ttLaborDtlComment)
        {
            if (ttLaborDtlComment.CommentType.Equals("SUB", StringComparison.OrdinalIgnoreCase))
            {
                ttLaborDtlComment.TimeEntryCommentTypeList = "SUB" + Constants.SUBLIST_DELIM + Strings.Submit;
            }
            else if (ttLaborDtlComment.CommentType.Equals("APP", StringComparison.OrdinalIgnoreCase))
            {
                ttLaborDtlComment.TimeEntryCommentTypeList = "APP" + Constants.SUBLIST_DELIM + Strings.Approve;
            }
            else if (ttLaborDtlComment.CommentType.Equals("REJ", StringComparison.OrdinalIgnoreCase))
            {
                ttLaborDtlComment.TimeEntryCommentTypeList = "REJ" + Constants.SUBLIST_DELIM + Strings.Reject;
            }
            else
            {
                ttLaborDtlComment.TimeEntryCommentTypeList = "SUB" + Constants.SUBLIST_DELIM + Strings.Submit +
                                                             Constants.LIST_DELIM +
                                                             "INV" + Constants.SUBLIST_DELIM + Strings.Invoice;
            }
        }

        private void assignCommentTypeDesc(ref LaborDtlCommentRow ttLaborDtlComment)
        {
            switch (ttLaborDtlComment.CommentType.ToUpperInvariant())
            {
                case "SUB":
                    ttLaborDtlComment.CommentTypeDesc = Strings.Submit;
                    break;
                case "INV":
                    ttLaborDtlComment.CommentTypeDesc = Strings.Invoice;
                    break;
                case "APP":
                    ttLaborDtlComment.CommentTypeDesc = Strings.Approve;
                    break;
                case "REJ":
                    ttLaborDtlComment.CommentTypeDesc = Strings.Reject;
                    break;

            }
        }

        /// <summary>
        /// Returns valid labor types based on the employee
        /// </summary>
        /// <param name="employeeNum">Employee Number</param>
        /// <param name="laborTypeList">Labor Type List</param>
        public void GetLaborTypeList(string employeeNum, out string laborTypeList)
        {
            laborTypeList = String.Empty;

            EmpBasic = this.FindFirstEmpBasic9(Session.CompanyID, employeeNum);

            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.ProjectBilling) &&
                EmpBasic != null &&
                EmpBasic.AllowDirLbr == false)
            {
                laborTypeList = "I" + Constants.SUBLIST_DELIM + Strings.Indirect +
                                Constants.LIST_DELIM +
                                "J" + Constants.SUBLIST_DELIM + Strings.Project;
            }
            else
            {
                using (var getCodeDescList = new Ice.Core.Getcodedesclist(Db))
                {
                    laborTypeList = getCodeDescList.GetCodeDescList("LaborDtl", "LaborTypePseudo");
                }
            }
        }

        /// <summary>
        /// Create LbrScrapSerialNumbers dataset records from a list of selected serial numbers
        /// </summary>
        /// <param name="serialNumberList">Serial Number List</param>
        /// <param name="partNumList">Part Number List</param>
        /// <param name="ds">Labor dataset</param>
        public void CreateLbrScrapSerialNumbersFromList(string serialNumberList, string partNumList, ref LaborTableset ds)
        {
            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where modList.Lookup(ttLaborDtl_Row.RowMod) != -1
                          select ttLaborDtl_Row).FirstOrDefault();
            if (ttLaborDtl == null)
            {
                throw new BLException(Strings.LaborDetailHasNotChanged, "LaborDtl");
            }

            if (String.IsNullOrEmpty(serialNumberList))
            {
                return;
            }

            for (iCounter = 1; iCounter <= serialNumberList.NumEntries(Ice.Constants.LIST_DELIM[0]); iCounter++)
            {
                string serialNumber = serialNumberList.Entry(iCounter - 1, Ice.Constants.LIST_DELIM);
                string partNum = partNumList.Entry(iCounter - 1, Ice.Constants.LIST_DELIM);

                if (!String.IsNullOrEmpty(serialNumber))
                {
                    // Verify this serial number doesn't already exist in ttLbrScrapSerialNumbers
                    ttLbrScrapSerialNumbers = (from ttLbrScrapSerialNumbers_row in ds.LbrScrapSerialNumbers
                                               where ttLbrScrapSerialNumbers_row.Company.KeyEquals(ttLaborDtl.Company) &&
                                                     ttLbrScrapSerialNumbers_row.LaborHedSeq == ttLaborDtl.LaborHedSeq &&
                                                     ttLbrScrapSerialNumbers_row.LaborDtlSeq == ttLaborDtl.LaborDtlSeq &&
                                                     ttLbrScrapSerialNumbers_row.JobNum.KeyEquals(ttLaborDtl.JobNum) &&
                                                     ttLbrScrapSerialNumbers_row.AssemblySeq == ttLaborDtl.AssemblySeq &&
                                                     ttLbrScrapSerialNumbers_row.OprSeq == ttLaborDtl.OprSeq &&
                                                     ttLbrScrapSerialNumbers_row.SerialNumber.KeyEquals(serialNumber)
                                               select ttLbrScrapSerialNumbers_row).FirstOrDefault();

                    if (ttLbrScrapSerialNumbers == null)
                    {
                        string defaultStatus = "COMPLETE";
                        if (ttLaborDtl.LaborQty == 0 && ttLaborDtl.DiscrepQty == 0 && ttLaborDtl.ScrapQty > 0)
                        {
                            defaultStatus = "REJECTED";
                        }
                        else if (ttLaborDtl.LaborQty == 0 && ttLaborDtl.ScrapQty == 0 && ttLaborDtl.DiscrepQty > 0)
                        {
                            defaultStatus = "INSPECTION";
                        }

                        ttLbrScrapSerialNumbers = new LbrScrapSerialNumbersRow();
                        ttLbrScrapSerialNumbers.Company = ttLaborDtl.Company;
                        ttLbrScrapSerialNumbers.LaborHedSeq = ttLaborDtl.LaborHedSeq;
                        ttLbrScrapSerialNumbers.LaborDtlSeq = ttLaborDtl.LaborDtlSeq;
                        ttLbrScrapSerialNumbers.JobNum = ttLaborDtl.JobNum;
                        ttLbrScrapSerialNumbers.AssemblySeq = ttLaborDtl.AssemblySeq;
                        ttLbrScrapSerialNumbers.OprSeq = ttLaborDtl.OprSeq;
                        ttLbrScrapSerialNumbers.SerialNumber = serialNumber;
                        ttLbrScrapSerialNumbers.PartNum = partNum;
                        ttLbrScrapSerialNumbers.SNStatus = defaultStatus;
                        ttLbrScrapSerialNumbers.Selected = true;
                        ttLbrScrapSerialNumbers.EnableStatus = (ttLaborDtl.ReWork == false);
                        ttLbrScrapSerialNumbers.SysRowID = Guid.NewGuid();
                        ttLbrScrapSerialNumbers.RowMod = IceRow.ROWSTATE_ADDED;
                        assignSNStatusDesc();

                        ds.LbrScrapSerialNumbers.Add(ttLbrScrapSerialNumbers);
                    }

                }
            }
        }

        /// <summary>
        /// Called when LaborHed Payroll Date is changing
        /// </summary>
        /// <param name="payrollDate">Payroll Date</param>
        /// <param name="ds">Labor Dataset</param>
        public void LaborHedPayrollDateChanging(DateTime? payrollDate, ref LaborTableset ds)
        {
            ttLaborHed = (from ttLaborHed_Row in ds.LaborHed
                          where modList.Lookup(ttLaborHed_Row.RowMod) != -1
                          select ttLaborHed_Row).FirstOrDefault();
            if (ttLaborHed == null)
            {
                throw new BLException(Strings.LaborHeaderHasNotChanged, "LaborHed");
            }

            if (ttLaborHed.RowMod.KeyEquals(IceRow.ROWSTATE_ADDED))
            {
                lDefDateFromNewHeader = true;
            }

            DefaultDate(ref ds, payrollDate);
        }

        /// <summary>
        /// Return values for a new labor record that is added via a calendar.  Parses the date and time from the calendar data parameters and
        /// returns the LaborHedSeq value of the first LaborHed record that exists for the calendarStartDateTime if one exists.  Returns zero 
        /// if a LaborHed record does not exist.
        /// </summary>
        /// <param name="empID">Employee ID</param>
        /// <param name="calendarStartDateTime">Start date and time selected from calendar</param>
        /// <param name="calendarEndDateTime">End</param>
        /// <param name="laborHedSeq">LaborHedSeq for existing LaborHed record on the calendarStartDateTime.  Returns 0 if a LaborHed does not exist.</param>
        /// <param name="startDate">Return start date</param>
        /// <param name="startTime">Return start time</param>
        /// <param name="endDate">Return end date</param>
        /// <param name="endTime">Return end time</param>
        public void GetDefaultsAddLaborDtlFromCalendar(
            string empID,
            DateTime? calendarStartDateTime,
            DateTime? calendarEndDateTime,
            out int laborHedSeq,
            out DateTime? startDate,
            out decimal startTime,
            out DateTime? endDate,
            out decimal endTime)
        {
            laborHedSeq = 0;
            startDate = calendarStartDateTime.Value.Date;
            endDate = calendarEndDateTime.Value.Date;
            startTime = timeToDecimal(calendarStartDateTime);
            endTime = timeToDecimal(calendarEndDateTime);

            foreach (var laborHedRow in (SelectLaborHed(Session.CompanyID, empID, startDate)))
            {
                if (getWipPosted(laborHedRow.LaborHedSeq))
                {
                    continue;
                }
                else
                {
                    laborHedSeq = laborHedRow.LaborHedSeq;
                    break;
                }
            }
        }

        private decimal timeToDecimal(DateTime? dateTime)
        {
            int workSecounds = 0;
            decimal returnHours = 0;

            workSecounds = dateTime.Value.Hour * 3600;
            workSecounds += dateTime.Value.Minute * 60;
            workSecounds += dateTime.Value.Second;

            try
            {
                returnHours = workSecounds / System.Convert.ToDecimal(3600.0);
                returnHours = Decimal.Round(returnHours, 2);
            }
            catch
            {
                returnHours = 0;
            }
            return returnHours;
        }
        #endregion Methods for MetaUI

        #region Changing Methods For LaborPart

        /// <summary>
        /// This method sets Complete checkbox when scrap qty field changes in End Activity. 
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="scrapQty">Proposed change to PartQty field </param>
        /// <param name="sysRowID">sysRowID of line updated in LaborPart</param>
        /// <param name="vMessage">Returns a string of warnings user needs to know</param>
        public void OnChangeLaborPartScrapQty(ref LaborTableset ds, decimal scrapQty, Guid sysRowID, out string vMessage)
        {
            vMessage = string.Empty;
            decimal acumPartQty = 0;
            decimal acumScrapQty = 0;
            string vPartNum;
            LaborPartRow tmpLaborPart;
            string vJobMainPart = string.Empty;
            bool vJobSequential = true;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where ttLaborDtl_Row.RowMod.Compare(IceRow.ROWSTATE_UPDATED) == 0
                          select ttLaborDtl_Row).FirstOrDefault();

            if (ttLaborDtl == null) return;

            var JobPartProcessModeResult = GetJobHeadPartProcessMode(Session.CompanyID, ttLaborDtl.JobNum);
            if (JobPartProcessModeResult != null)
            {
                vJobMainPart = JobPartProcessModeResult.PartNum;
                vJobSequential = !JobPartProcessModeResult.ProcessMode.Equals("C", StringComparison.OrdinalIgnoreCase);
            }

            tmpLaborPart = (from tmpLaborPart_Row in ds.LaborPart
                            where tmpLaborPart_Row.SysRowID == sysRowID &&
                            modList.Lookup(tmpLaborPart_Row.RowMod) != -1
                            select tmpLaborPart_Row).FirstOrDefault();

            if (tmpLaborPart != null)
            {
                vPartNum = tmpLaborPart.PartNum;
                tmpLaborPart.ScrapQty = scrapQty;
            }
            else
            {
                //First Change in LaborPart does not bring the LaborPart in dataset
                LaborPart = FindFirstLaborPart(sysRowID);
                vPartNum = LaborPart.PartNum;
                if (vJobSequential == true || vJobMainPart.Equals(vPartNum, StringComparison.OrdinalIgnoreCase))
                {
                    acumScrapQty = scrapQty;  //Value from record not yet in dataset
                }
            }

            #region Acum PartQty and ScrapQty in current Labor Part
            //Values in LaborPart before Update
            Hashtable LaborPartQty = new Hashtable();
            Hashtable LaborPartQty2 = new Hashtable();
            foreach (LaborPartRow tmpLaborPart2 in (from tmpLaborPart_Row in ds.LaborPart
                                                    where modList.Lookup(tmpLaborPart_Row.RowMod) == -1
                                                    select tmpLaborPart_Row))
            {
                if (vJobSequential == true || vJobMainPart.Equals(tmpLaborPart2.PartNum, StringComparison.OrdinalIgnoreCase))
                {
                    LaborPartQty.Add(tmpLaborPart2.SysRowID, tmpLaborPart2.PartQty);
                    LaborPartQty2.Add(tmpLaborPart2.SysRowID, tmpLaborPart2.ScrapQty);
                }
            }

            //Values in LaborPart with RowMod = "U"
            foreach (LaborPartRow tmpLaborPart3 in (from tmpLaborPart_Row in ds.LaborPart
                                                    where tmpLaborPart_Row.RowMod.Compare(IceRow.ROWSTATE_UPDATED) == 0
                                                    select tmpLaborPart_Row))
            {
                if (vJobSequential == true || vJobMainPart.Equals(tmpLaborPart3.PartNum, StringComparison.OrdinalIgnoreCase))
                {
                    LaborPartQty[tmpLaborPart3.SysRowID] = tmpLaborPart3.PartQty;
                    LaborPartQty2[tmpLaborPart3.SysRowID] = tmpLaborPart3.ScrapQty;
                }
            }

            //Get Total PartQty with current updates
            foreach (decimal values in LaborPartQty.Values)
            {
                acumPartQty = acumPartQty + values;
            }

            //Get Total ScrapQty with current updates
            foreach (decimal values in LaborPartQty2.Values)
            {
                acumScrapQty = acumScrapQty + values;
            }
            #endregion

            this.setComplete(ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq, ttLaborDtl.OprSeq, acumPartQty, acumScrapQty);

            if (tmpLaborPart.ScrapQty == 0)
            {
                tmpLaborPart.ScrapReasonCode = String.Empty;
            }
        }

        /// <summary>
        /// This method updates the revision field when the attribute ID field changes.  
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>        
        /// <param name="attributeSetID">Proposed change to ScrapQty field </param>
        /// <param name="type">Discrep, Scrap or Labor</param>
        public void ChangeLaborPartAttributeSetID(ref LaborTableset ds, int attributeSetID, string type)
        {
            string revisionNum = string.Empty;

            ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                          where ttLaborDtl_Row.RowMod.Compare(IceRow.ROWSTATE_UPDATED) == 0
                          select ttLaborDtl_Row).FirstOrDefault();

            if (ttLaborDtl == null) return;

            ttLaborPart = (from ttLaborPart_Row in ds.LaborPart
                           where modList.Lookup(ttLaborPart_Row.RowMod) != -1
                           select ttLaborPart_Row).FirstOrDefault();
            if (ttLaborPart == null)
            {
                throw new BLException(Strings.LaborPartHasNotChanged, "LaborPart");
            }

            var jobAsmblPart = FindFirstJobAsmblPart(ttLaborDtl.Company, ttLaborDtl.JobNum, ttLaborDtl.AssemblySeq);
            if (jobAsmblPart != null)
            {
                PartPartial part = FindFirstPartPartial(Session.CompanyID, jobAsmblPart.PartNum);
                if (part == null || !part.TrackInventoryByRevision)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            using (var libInventoryByRevision = new Internal.Lib.InventoryByRevision(Db))
            {
                revisionNum = libInventoryByRevision.GetRevisionNumFromDynAttrValueSet(attributeSetID);
            }

            if (type.KeyEquals("L"))
            {
                ttLaborPart.RevisionNum = revisionNum;
            }

            if (type.KeyEquals("S"))
            {
                ttLaborPart.ScrapRevision = revisionNum;
            }

            if (type.KeyEquals("D"))
            {
                ttLaborPart.DiscrepRevision = revisionNum;
            }
        }

        /// <summary>
        /// Called after LaborDtl.DiscrepQty has been changed.
        /// </summary>
        /// <param name="ds">Labor Entry Data set</param>
        /// <param name="message">Return message</param>
        public void AfterChangeLaborPartDiscrepQty(ref LaborTableset ds, out string message)
        {
            message = String.Empty; ttLaborDtl = (from ttLaborDtl_Row in ds.LaborDtl
                                                  where ttLaborDtl_Row.RowMod.Compare(IceRow.ROWSTATE_UPDATED) == 0
                                                  select ttLaborDtl_Row).FirstOrDefault();

            if (ttLaborDtl == null) return;

            ttLaborPart = (from ttLaborPart_Row in ds.LaborPart
                           where modList.Lookup(ttLaborPart_Row.RowMod) != -1
                           select ttLaborPart_Row).FirstOrDefault();
            if (ttLaborPart == null)
            {
                throw new BLException(Strings.LaborPartHasNotChanged, "LaborPart");
            }

            if (ttLaborPart.DiscrepQty == 0)
            {
                ttLaborPart.DiscrpRsnCode = String.Empty;
            }
        }
        #endregion
    }
}
