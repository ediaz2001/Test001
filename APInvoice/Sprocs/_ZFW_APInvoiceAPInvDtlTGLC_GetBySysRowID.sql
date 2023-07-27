/*** Object: StoredProcedure [Erp].[_ZFW_APInvoiceAPInvDtlTGLC_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_APInvoiceAPInvDtlTGLC_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table TranGLC AS APInvDtlTGLC / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[RelatedToFile],
[t0].[Key1],
[t0].[Key2],
[t0].[Key3],
[t0].[Key4],
[t0].[Key5],
[t0].[Key6],
[t0].[TGLCTranNum],
[t0].[GLAcctContext],
[t0].[BookID],
[t0].[COACode],
[t0].[GLAccount],
[t0].[UserCanModify],
[t0].[SegValue1],
[t0].[SegValue2],
[t0].[SegValue3],
[t0].[SegValue4],
[t0].[SegValue5],
[t0].[SegValue6],
[t0].[SegValue7],
[t0].[SegValue8],
[t0].[SegValue9],
[t0].[SegValue10],
[t0].[SegValue11],
[t0].[SegValue12],
[t0].[SegValue13],
[t0].[SegValue14],
[t0].[SegValue15],
[t0].[SegValue16],
[t0].[SegValue17],
[t0].[SegValue18],
[t0].[SegValue19],
[t0].[SegValue20],
[t0].[SysGLControlType],
[t0].[SysGLControlCode],
[t0].[ExtCompanyID],
[t0].[IsExternalCompany],
[t0].[FiscalYear],
[t0].[JournalCode],
[t0].[JournalNum],
[t0].[JournalLine],
[t0].[TranDate],
[t0].[TranSource],
[t0].[LaborHedSeq],
[t0].[LaborDtlSeq],
[t0].[SysDate],
[t0].[SysTime],
[t0].[TranNum],
[t0].[ARInvoiceNum],
[t0].[TransAmt],
[t0].[InvoiceLine],
[t0].[SeqNum],
[t0].[VendorNum],
[t0].[APInvoiceNum],
[t0].[CreateDate],
[t0].[FiscalYearSuffix],
[t0].[FiscalCalendarID],
[t0].[CreditAmount],
[t0].[DebitAmount],
[t0].[BookCreditAmount],
[t0].[BookDebitAmount],
[t0].[CurrencyCode],
[t0].[RecordType],
[t0].[CorrAccUID],
[t0].[ABTUID],
[t0].[RuleUID],
[t0].[Statistical],
[t0].[StatUOMCode],
[t0].[DebitStatAmt],
[t0].[CreditStatAmt],
[t0].[IsModifiedByUser],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
[t0].[MovementNum],
[t0].[MovementType],
[t0].[Plant],
'' AS [InvoiceNum],
'' AS [GroupID],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag],
ISNULL([L1].[Description], '') AS [COADescription],
ISNULL([L2].[AccountDesc], '') AS [GLAccountAccountDesc],
ISNULL([L3].[CurrencyCode], '') AS [GLBookCurrencyCode],
ISNULL([L3].[Description], '') AS [GLBookDescription]
FROM [Erp].[TranGLC] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'TranGLC' AND b1.RelatedToRowId = t0.SysRowID
LEFT JOIN [Erp].[COA] AS [L1] ON ([L1].[Company] = [t0].[Company] AND [L1].[COACode] = [t0].[COACode])
LEFT JOIN [Erp].[GLAcctDisp] AS [L2] ON ([L2].[Company] = [t0].[Company] AND [L2].[COACode] = [t0].[COACode] AND [L2].[GLAccount] = [t0].[GLAccount])
LEFT JOIN [Erp].[GLBook] AS [L3] ON ([L3].[Company] = [t0].[Company] AND [L3].[BookID] = [t0].[BookID])
	WHERE [t0].SysRowID = @SysRowID
END
