/*** Object: StoredProcedure [Erp].[_ZFW_ConfigurationRuntimePcInputsLayerDetail_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_ConfigurationRuntimePcInputsLayerDetail_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table PcInputsLayerDetail AS PcInputsLayerDetail / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[ConfigID],
[t0].[InputName],
[t0].[ImageLayerID],
[t0].[LayerSeq],
[t0].[LayerName],
[t0].[LayerDesc],
[t0].[ZIndex],
[t0].[ImageID],
[t0].[FileType],
[t0].[Category],
[t0].[Width],
[t0].[Height],
[t0].[xPos],
[t0].[yPos],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[PcInputsLayerDetail] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcInputsLayerDetail' AND b1.RelatedToRowId = t0.SysRowID
	WHERE [t0].SysRowID = @SysRowID
END
