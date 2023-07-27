/*** Object: StoredProcedure [Erp].[_ZFW_ConfigurationRuntimePcValueInputLayer_GetBySysRowID] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Erp].[_ZFW_ConfigurationRuntimePcValueInputLayer_GetBySysRowID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Erp].[_ZFW_ConfigurationRuntimePcValueInputLayer_GetBySysRowID]
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Erp].[_ZFW_ConfigurationRuntimePcValueInputLayer_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table PcValueInputLayer AS PcValueInputLayer / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[GroupSeq],
[t0].[HeadNum],
[t0].[ConfigID],
[t0].[InputName],
[t0].[ImageLayerID],
[t0].[LayerSeq],
[t0].[ImageURL],
[t0].[ZIndex],
[t0].[ImageID],
[t0].[FileType],
[t0].[Width],
[t0].[Height],
[t0].[Category],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[PcValueInputLayer] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcValueInputLayer' AND b1.RelatedToRowId = t0.SysRowID
	WHERE [t0].SysRowID = @SysRowID
END
GO
