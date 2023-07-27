/*** Object: StoredProcedure [Erp].[_ZFW_APInvoiceAPInvDtlDEASch_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_APInvoiceAPInvDtlDEASch_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table APInvDtlDEASch AS APInvDtlDEASch / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[VendorNum],
[t0].[InvoiceNum],
[t0].[InvoiceLine],
[t0].[AmortSeq],
[t0].[FiscalCalendarID],
[t0].[FiscalYear],
[t0].[FiscalYearSuffix],
[t0].[FiscalPeriod],
[t0].[AmortDate],
[t0].[AmortPercent],
[t0].[AmortAmt],
[t0].[Rpt1AmortAmt],
[t0].[Rpt2AmortAmt],
[t0].[Rpt3AmortAmt],
[t0].[DocAmortAmount],
[t0].[Hold],
[t0].[HoldReasonCode],
[t0].[HoldText],
[t0].[Posted],
[t0].[PostedDate],
[t0].[TranDocTypeID],
[t0].[LegalNumber],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
'' AS [GroupID],
CAST(0.0 AS decimal(20,9)) AS [DocAmortAmt],
'' AS [CurrencyCode],
CAST(0.0 AS decimal(20,9)) AS [DspAmortAmt],
CAST(0.0 AS decimal(20,9)) AS [DocDspAmortAmt],
CAST(0.0 AS decimal(20,9)) AS [Rpt1DspAmortAmt],
CAST(0.0 AS decimal(20,9)) AS [Rpt2DspAmortAmt],
CAST(0.0 AS decimal(20,9)) AS [Rpt3DspAmortAmt],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[APInvDtlDEASch] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'APInvDtlDEASch' AND b1.RelatedToRowId = t0.SysRowID
	WHERE [t0].SysRowID = @SysRowID
END
