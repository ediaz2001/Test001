/*** Object: StoredProcedure [Erp].[_ZFW_LaborLaborDtlComment_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_LaborLaborDtlComment_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table LaborDtlComment AS LaborDtlComment / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[LaborHedSeq],
[t0].[LaborDtlSeq],
[t0].[CommentNum],
[t0].[CommentType],
[t0].[CommentText],
[t0].[CreatedBy],
[t0].[CreateDate],
[t0].[CreateTime],
[t0].[ChangedBy],
[t0].[ChangeDate],
[t0].[ChangeTime],
[t0].[TaskSeqNum],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
'' AS [DspChangeTime],
'' AS [DspCreateTime],
'' AS [TreeNodeImageName],
'' AS [IntExternalKey],
'' AS [TimeEntryCommentTypeList],
'' AS [CommentTypeDesc],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[LaborDtlComment] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'LaborDtlComment' AND b1.RelatedToRowId = t0.SysRowID
	WHERE [t0].SysRowID = @SysRowID
END
