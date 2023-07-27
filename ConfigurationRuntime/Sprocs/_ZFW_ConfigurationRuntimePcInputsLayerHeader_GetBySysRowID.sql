/*** Object: StoredProcedure [Erp].[_ZFW_ConfigurationRuntimePcInputsLayerHeader_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_ConfigurationRuntimePcInputsLayerHeader_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table PcInputsLayerHeader AS PcInputsLayerHeader / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[ConfigID],
[t0].[InputName],
[t0].[ImageLayerID],
[t0].[ImageID],
[t0].[Description],
[t0].[ImageURL],
[t0].[FileType],
[t0].[Width],
[t0].[Height],
[t0].[Version],
[t0].[xPos],
[t0].[yPos],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[PcInputsLayerHeader] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcInputsLayerHeader' AND b1.RelatedToRowId = t0.SysRowID
	WHERE [t0].SysRowID = @SysRowID
END
