/*** Object: StoredProcedure [Erp].[_ZFW_LaborLaborDtlAction_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_LaborLaborDtlAction_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table LaborDtlAction AS LaborDtlAction / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[LaborHedSeq],
[t0].[LaborDtlSeq],
[t0].[ActionSeq],
[t0].[ActionDesc],
[t0].[Required],
[t0].[Completed],
[t0].[CompletedBy],
[t0].[CompletedOn],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag],
ISNULL([L1].[Name], '') AS [EmpBasicName]
FROM [Erp].[LaborDtlAction] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'LaborDtlAction' AND b1.RelatedToRowId = t0.SysRowID
LEFT JOIN [Erp].[EmpBasic] AS [L1] ON ([L1].[Company] = [t0].[Company] AND [L1].[EmpID] = [t0].[CompletedBy])
	WHERE [t0].SysRowID = @SysRowID
END
