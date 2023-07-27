/*** Object: StoredProcedure [Erp].[_ZFW_ConfigurationRuntimePcInputVar_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_ConfigurationRuntimePcInputVar_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table PcInputVar AS PcInputVar / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[VarName],
[t0].[DataType],
[t0].[InitValue],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
'' AS [InitString],
CAST(0.0 AS decimal(20,9)) AS [InitDecimal],
CAST(0 AS bit) AS [InitLogical],
null AS [InitDate],
CAST(0 AS bit) AS [InUse],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[PcInputVar] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcInputVar' AND b1.RelatedToRowId = t0.SysRowID
	WHERE [t0].SysRowID = @SysRowID
END
