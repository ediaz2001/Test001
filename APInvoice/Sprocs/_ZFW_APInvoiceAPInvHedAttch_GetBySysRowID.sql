/*** Object: StoredProcedure [Erp].[_ZFW_APInvoiceAPInvHedAttch_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_APInvoiceAPInvHedAttch_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table XFileAttch AS APInvHedAttch / Table Number : 1
---------------------------------------------------------------------
SELECT [P].[Company],
[P].[VendorNum],
[P].[InvoiceNum],
[t0].[AttachNum] AS [DrawingSeq],
[t0].[XFileRefNum],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
[t0].[ForeignSysRowID],
[L1].[XFileDesc] AS [DrawDesc],
[L1].[XFileName] AS [FileName],
[L1].[PDMDocID],
[L1].[DocTypeID]
FROM [Ice].[XFileAttch] AS [t0]
JOIN [Erp].[APInvHed] AS [P] ON ([P].[SysRowID] = [t0].[ForeignSysRowID])
LEFT JOIN [Ice].[XFileRef] AS [L1] ON ([L1].[Company] = [t0].[Company] AND [L1].[XFileRefNum] = [t0].[XFileRefNum])
	WHERE [t0].SysRowID = @SysRowID
END
