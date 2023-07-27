/*** Object: StoredProcedure [Erp].[_ZFW_APInvoiceAPInvJob_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_APInvoiceAPInvJob_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table APInvJob AS APInvJob / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[VendorNum],
[t0].[InvoiceNum],
[t0].[InvoiceLine],
[t0].[JobNum],
[t0].[AssemblySeq],
[t0].[MtlSeq],
[t0].[ExtCost],
[t0].[Rpt1ExtCost],
[t0].[Rpt2ExtCost],
[t0].[Rpt3ExtCost],
[t0].[DocExtCost],
[t0].[ProjectID],
[t0].[PhaseID],
[t0].[MiscCode],
[t0].[ProjProcessed],
[t0].[AsOfDate],
[t0].[AsOfSeq],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
[t0].[EmpID],
[t0].[EmpExpenseNum],
[t0].[PartTranSysRowID],
CAST(0 AS int) AS [AddedJobMtlSeq],
CAST(0 AS int) AS [CallLine],
CAST(0 AS int) AS [CallNum],
'' AS [CurrencyCode],
CAST(0 AS bit) AS [CurrencySwitch],
'' AS [CurrSymbol],
CAST(0 AS bit) AS [DebitMemo],
CAST(0.0 AS decimal(20,9)) AS [DocScrExtCost],
CAST(0 AS bit) AS [EnableMaterialComplete],
'' AS [GroupID],
'' AS [JobMtlDescription],
'' AS [JobMtlMiscCodeDesc],
'' AS [JobMtlPartNum],
CAST(0.0 AS decimal(20,9)) AS [JobMtlTotalCost],
CAST(0.0 AS decimal(20,9)) AS [JobMtlUnitCost],
'' AS [JobType],
CAST(0 AS bit) AS [MaterialComplete],
'' AS [MtlQuestion],
'' AS [PhaseDescription],
CAST(0 AS bit) AS [Posted],
CAST(0.0 AS decimal(20,9)) AS [Rpt1ScrExtCost],
CAST(0.0 AS decimal(20,9)) AS [Rpt2ScrExtCost],
CAST(0.0 AS decimal(20,9)) AS [Rpt3ScrExtCost],
CAST(0.0 AS decimal(20,9)) AS [ScrExtCost],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag],
ISNULL([L1].[Description], '') AS [InvoiceNumDescription],
ISNULL([L2].[PartDescription], '') AS [JobNumPartDescription],
ISNULL([L3].[Name], '') AS [VendorNumName],
ISNULL([L3].[Address1], '') AS [VendorNumAddress1],
ISNULL([L3].[VendorID], '') AS [VendorNumVendorID],
ISNULL([L3].[Address2], '') AS [VendorNumAddress2],
ISNULL([L3].[ZIP], '') AS [VendorNumZIP],
ISNULL([L3].[TermsCode], '') AS [VendorNumTermsCode],
ISNULL([L3].[CurrencyCode], '') AS [VendorNumCurrencyCode],
ISNULL([L3].[City], '') AS [VendorNumCity],
ISNULL([L3].[Address3], '') AS [VendorNumAddress3],
ISNULL([L3].[DefaultFOB], '') AS [VendorNumDefaultFOB],
ISNULL([L3].[Country], '') AS [VendorNumCountry],
ISNULL([L3].[State], '') AS [VendorNumState]
FROM [Erp].[APInvJob] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'APInvJob' AND b1.RelatedToRowId = t0.SysRowID
LEFT JOIN [Erp].[APInvHed] AS [L1] ON ([L1].[Company] = [t0].[Company] AND [L1].[VendorNum] = [t0].[VendorNum] AND [L1].[InvoiceNum] = [t0].[InvoiceNum])
LEFT JOIN [Erp].[JobHead] AS [L2] ON ([L2].[Company] = [t0].[Company] AND [L2].[JobNum] = [t0].[JobNum])
LEFT JOIN [Erp].[Vendor] AS [L3] ON ([L3].[Company] = [t0].[Company] AND [L3].[VendorNum] = [t0].[VendorNum])
	WHERE [t0].SysRowID = @SysRowID
END
