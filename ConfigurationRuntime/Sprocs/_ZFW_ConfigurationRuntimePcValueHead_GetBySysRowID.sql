/*** Object: StoredProcedure [Erp].[_ZFW_ConfigurationRuntimePcValueHead_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_ConfigurationRuntimePcValueHead_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table PcValueHead AS PcValueHead / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[GroupSeq],
[t0].[HeadNum],
[t0].[StructTag],
[t0].[ConfigID],
[t0].[RevolvingSeq],
[t0].[ForeignTableName],
[t0].[ForeignSysRowID],
[t0].[SourceTableName],
[t0].[SourceSysRowID],
[t0].[ConfigType],
[t0].[ConfigVersion],
[t0].[DisplayTag],
[t0].[RuleTag],
[t0].[ExtConfig],
[t0].[ExtCompany],
[t0].[AllowRecordCreation],
[t0].[AllowPricing],
[t0].[PromptForConfig],
[t0].[InSmartString],
[t0].[DisplaySummary],
[t0].[AllowReconfig],
[t0].[IsMainForeign],
[t0].[NewPartNum],
[t0].[NewSmartString],
[t0].[SIValues],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
CAST(0 AS int) AS [SIValuesGroupSeq],
CAST(null AS uniqueidentifier) AS [TestID],
CAST(null AS uniqueidentifier) AS [RelatedToSysRowID],
'' AS [RelatedToTableName],
CAST(0 AS int) AS [SIValuesHeadNum],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[PcValueHead] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcValueHead' AND b1.RelatedToRowId = t0.SysRowID
	WHERE [t0].SysRowID = @SysRowID
END
