/*** Object: StoredProcedure [Erp].[_ZFW_ConfigurationRuntimePcValueGrp_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_ConfigurationRuntimePcValueGrp_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table PcValueGrp AS PcValueGrp / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[GroupSeq],
[t0].[RelatedToTableName],
[t0].[RelatedToSysRowID],
[t0].[CreateDate],
[t0].[CreateUserID],
[t0].[LastUpdated],
[t0].[LastUpdatedBy],
[t0].[ConfigStatus],
[t0].[SIValues],
[t0].[HeadNum],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
CAST(0 AS bit) AS [DisplaySummary],
CAST(0 AS bit) AS [IncomingSmartString],
CAST(null AS uniqueidentifier) AS [TestID],
'' AS [TestMode],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[PcValueGrp] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcValueGrp' AND b1.RelatedToRowId = t0.SysRowID
	WHERE [t0].SysRowID = @SysRowID
END
