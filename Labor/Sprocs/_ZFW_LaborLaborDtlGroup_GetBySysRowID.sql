/*** Object: StoredProcedure [Erp].[_ZFW_LaborLaborDtlGroup_GetBySysRowID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_LaborLaborDtlGroup_GetBySysRowID]
	@SysRowID uniqueidentifier = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
---------------------------------------------------------------------
-- SELECT for table LaborDtlGroup AS LaborDtlGroup / Table Number : 1
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[EmployeeNum],
[t0].[ClaimRef],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[LaborDtlGroup] AS [t0]
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'LaborDtlGroup' AND b1.RelatedToRowId = t0.SysRowID
	WHERE [t0].SysRowID = @SysRowID
END
