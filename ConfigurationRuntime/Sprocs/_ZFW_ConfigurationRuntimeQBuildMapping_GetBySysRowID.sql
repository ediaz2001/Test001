/*** Object: StoredProcedure [Erp].[_ZFW_ConfigurationRuntimeQBuildMapping_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_ConfigurationRuntimeQBuildMapping_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table QBuildMapping AS QBuildMapping / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[ConfigID],
[t0].[InputName],
[t0].[ObjName],
[t0].[ObjParameter],
[t0].[MappedInputName],
[t0].[ObjParameterDataType],
[t0].[ObjParameterInitValue],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
'' AS [DataType],
CAST(0 AS int) AS [PageSeq],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag],
ISNULL([L1].[ObjType], '') AS [QBuildObjObjType]
FROM [Erp].[QBuildMapping] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'QBuildMapping' AND b1.RelatedToRowId = t0.SysRowID
LEFT JOIN [Erp].[QBuildObj] AS [L1] ON ([L1].[Company] = [t0].[Company] AND [L1].[ConfigID] = [t0].[ConfigID] AND [L1].[InputName] = [t0].[InputName] AND [L1].[ObjName] = [t0].[ObjName])
	WHERE [t0].SysRowID = @SysRowID
END
