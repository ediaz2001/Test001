/*** Object: StoredProcedure [Erp].[_ZFW_APInvoiceAPInvExp_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_APInvoiceAPInvExp_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table APInvExp AS APInvExp / Table Number : 1
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
[t0].[GlbCompany],
[t0].[GlbVendorNum],
[t0].[GlbInvoiceNum],
[t0].[GlbInvoiceLine],
[t0].[GlbInvExpSeq],
[t0].[ExtCompanyID],
[t0].[ExtRefType],
[t0].[ExtRefCode],
[t0].[MultiCompany],
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
[t0].[Plant],
CAST(0 AS bit) AS [AllowUpdate],
'' AS [BookID],
'' AS [COACode],
'' AS [CurrencyCode],
CAST(0 AS bit) AS [CurrencySwitch],
CAST(0 AS bit) AS [DebitMemo],
CAST(0.0 AS decimal(20,9)) AS [DocScrExpAmt],
CAST(0 AS bit) AS [EnableExtRefCode],
CAST(0 AS bit) AS [EnableMultiCompany],
CAST(0 AS bit) AS [EnableRefCode],
'' AS [ExpDispGLAcct],
'' AS [ExpGlbDispGLAcct],
'' AS [ExtCOACode],
'' AS [ExtGLAccount],
'' AS [ExtGLAccountDesc],
'' AS [ExtRefAcctChart],
'' AS [ExtRefAcctDept],
'' AS [ExtRefAcctDiv],
'' AS [ExtRefCodeList],
'' AS [ExtRefCodeStatus],
'' AS [ExtRefDisplayAccount],
'' AS [GLAccount],
'' AS [GLAccountDesc],
'' AS [GroupID],
CAST(0 AS bit) AS [Posted],
'' AS [RefCodeList],
'' AS [RefCodeStatus],
CAST(0.0 AS decimal(20,9)) AS [Rpt1ScrExpAmt],
CAST(0.0 AS decimal(20,9)) AS [Rpt2ScrExpAmt],
CAST(0.0 AS decimal(20,9)) AS [Rpt3ScrExpAmt],
CAST(0.0 AS decimal(20,9)) AS [ScrExpAmt],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag],
ISNULL([L1].[RefCodeDesc], '') AS [ExtRefCodeRefCodeDesc],
ISNULL([L2].[Description], '') AS [InvoiceNumDescription],
ISNULL([L3].[Address3], '') AS [VendorNumAddress3],
ISNULL([L3].[TermsCode], '') AS [VendorNumTermsCode],
ISNULL([L3].[Address1], '') AS [VendorNumAddress1],
ISNULL([L3].[State], '') AS [VendorNumState],
ISNULL([L3].[VendorID], '') AS [VendorNumVendorID],
ISNULL([L3].[Address2], '') AS [VendorNumAddress2],
ISNULL([L3].[CurrencyCode], '') AS [VendorNumCurrencyCode],
ISNULL([L3].[Name], '') AS [VendorNumName],
ISNULL([L3].[Country], '') AS [VendorNumCountry],
ISNULL([L3].[DefaultFOB], '') AS [VendorNumDefaultFOB],
ISNULL([L3].[City], '') AS [VendorNumCity],
ISNULL([L3].[ZIP], '') AS [VendorNumZIP]
FROM [Erp].[APInvExp] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'APInvExp' AND b1.RelatedToRowId = t0.SysRowID
LEFT JOIN [Erp].[GlbGLRefCod] AS [L1] ON ([L1].[Company] = [t0].[Company] AND [L1].[ExtCompanyID] = [t0].[ExtCompanyID] AND [L1].[RefType] = [t0].[ExtRefType] AND [L1].[RefCode] = [t0].[ExtRefCode])
LEFT JOIN [Erp].[APInvHed] AS [L2] ON ([L2].[Company] = [t0].[Company] AND [L2].[VendorNum] = [t0].[VendorNum] AND [L2].[InvoiceNum] = [t0].[InvoiceNum])
LEFT JOIN [Erp].[Vendor] AS [L3] ON ([L3].[Company] = [t0].[Company] AND [L3].[VendorNum] = [t0].[VendorNum])
	WHERE [t0].SysRowID = @SysRowID
END
