/*** Object: StoredProcedure [Erp].[_ZFW_APInvoiceAPIHAPInvExp_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_APInvoiceAPIHAPInvExp_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table APInvExp AS APIHAPInvExp / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[VendorNum],
[t0].[InvoiceNum],
[t0].[InvoiceLine],
[t0].[InvExpSeq],
[t0].[ExpAmt],
[t0].[RefType],
[t0].[RefCode],
[t0].[RefCodeDesc],
[t0].[Rpt1ExpAmt],
[t0].[Rpt2ExpAmt],
[t0].[Rpt3ExpAmt],
[t0].[DocExpAmt],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
[t0].[NonDedTax],
[t0].[NonDedTaxRelatedToSchema],
[t0].[NonDedTaxRelatedToTable],
[t0].[NonDedTaxRelatedToSysRowID],
'' AS [RefDisplayAccount],
CAST(0.0 AS decimal(20,9)) AS [ScrExpAmt],
CAST(0 AS bit) AS [DebitMemo],
CAST(0 AS bit) AS [EnableRefCode],
'' AS [RefCodeList],
CAST(0 AS bit) AS [Posted],
'' AS [GroupID],
'' AS [RefCodeStatus],
'' AS [DispGLAcct],
'' AS [CurrencyCode],
CAST(0.0 AS decimal(20,9)) AS [DocScrExpAmt],
CAST(0.0 AS decimal(20,9)) AS [Rpt1ScrExpAmt],
CAST(0.0 AS decimal(20,9)) AS [Rpt2ScrExpAmt],
CAST(0.0 AS decimal(20,9)) AS [Rpt3ScrExpAmt],
CAST(0 AS bit) AS [CurrencySwitch],
'' AS [GLAccount],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[APInvExp] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'APInvExp' AND b1.RelatedToRowId = t0.SysRowID
	WHERE [t0].SysRowID = @SysRowID
END
