/*** Object: StoredProcedure [Erp].[_ZFW_ConfigurationRuntime_GetList] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_ConfigurationRuntime_GetList]
	-- Add the parameters for the stored procedure here
	@Company nvarchar(8),
	@WhereClause nvarchar(max) = null,
	@OrderBy nvarchar(max) = null,
	@PageSize int = 100,
	@AbsolutePage int = 0,
	@MoreRows bit OUTPUT,
	@RetrieveExtendedData bit = 0
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Defend against bad parameters
IF(@AbsolutePage < 1)
	SET @AbsolutePage = 1

IF(@PageSize < 1)
	SET @PageSize = 0

-- Declare and set parameters for use in paging
DECLARE @StartRow int
DECLARE @EndRow int
SET @StartRow = @PageSize * (@AbsolutePage - 1) + 1
IF(@PageSize = 0)
	SET @EndRow = 2147483647 --Max int
ELSE
	SET @EndRow = @StartRow + @PageSize -- grab one more than needed for the page to check if MorePages should be set


DECLARE @Erp_PcValueGrpList_udTableExists bit = 0
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Erp].[PcValueGrp_UD]') AND type = N'U')
	SET @Erp_PcValueGrpList_udTableExists = 1

-- Create a temp table to hold rowids of the selected rows in this table.
DECLARE @Erp_PcValueGrpList Ice.JoinTablePagingType;

IF (@WhereClause IS null OR @WhereClause='') AND (@OrderBy IS null OR @OrderBy='')
	BEGIN
		INSERT INTO @Erp_PcValueGrpList
		SELECT [SysRowID]
		FROM (
			SELECT ROW_NUMBER()
			OVER ( ORDER BY [PcValueGrpList].[Company], [PcValueGrpList].[GroupSeq]) AS [ROW_NUMBER], [PcValueGrpList].[SysRowID]
			FROM [Erp].[PcValueGrp] AS [PcValueGrpList]
			WHERE [Company] = @Company
		) AS t0
		WHERE [t0].[ROW_NUMBER] BETWEEN @StartRow AND @EndRow
	END
ELSE
	BEGIN
		IF @OrderBy IS NULL OR @OrderBy = ''
		SET @OrderBy = 'ORDER BY [PcValueGrpList].[Company], [PcValueGrpList].[GroupSeq]'

		IF @WhereClause IS NOT NULL AND @WhereClause <> ''
		SET @WhereClause = 'WHERE ' + @WhereClause
	ELSE
		SET @WhereClause = 'WHERE ([Company] = N''' + @Company + ''')'

	DECLARE @SelectFrom nvarchar(512)
	IF (@Erp_PcValueGrpList_udTableExists = 1)
	BEGIN
		SET @SelectFrom = N'
		[Erp].[PcValueGrp] AS [PcValueGrpList]
		LEFT JOIN [Erp].[PcValueGrp_UD] AS [PcValueGrpList_UD] ON [PcValueGrpList_UD].[ForeignSysRowID] = [PcValueGrpList].[SysRowID]';
	END
	ELSE
	   SET @SelectFrom = N'[Erp].[PcValueGrp] AS [PcValueGrpList]';

	DECLARE @SelectString NVARCHAR(max)
	SET @SelectString = N'
	SELECT [SysRowID]
	FROM (
		SELECT ROW_NUMBER()
		OVER ( ' + @OrderBy + ' ) AS [ROW_NUMBER], [PcValueGrpList].[SysRowID]
		FROM ' + @SelectFrom + '
		' + @WhereClause + '
		) AS t0
		WHERE [t0].[ROW_NUMBER] BETWEEN ' + Cast(@StartRow AS NVARCHAR(10)) + ' AND ' + Cast(@EndRow AS NVARCHAR(10))

	INSERT INTO @Erp_PcValueGrpList
	EXEC (@SelectString)
  END
--Set the MoreRows flag if needed and if MoreRows were indicated by retrieving one more row than asked for
IF( @PageSize = 0 )
	SET @MoreRows = 0
ELSE
	IF ((SELECT MAX([ID]) FROM @Erp_PcValueGrpList) > @PageSize)
		BEGIN
			SET @MoreRows = 1

			DECLARE @MorePagesIndicatorRowId int
			SELECT @MorePagesIndicatorRowId = MAX([ID]) FROM @Erp_PcValueGrpList
			DELETE @Erp_PcValueGrpList WHERE [ID] = @MorePagesIndicatorRowId
		END
	ELSE
		SET @MoreRows = 0
---------------------------------------------------------------------
-- SELECT for table PcValueGrp AS PcValueGrpList / Table Number : 1
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
[t0].[SysRowID]
FROM [Erp].[PcValueGrp] AS [t0]
JOIN @Erp_PcValueGrpList AS [tt] ON ([tt].[SysRowID] = [t0].[SysRowID])
ORDER BY [tt].ID
IF @RetrieveExtendedData = 0 RETURN;

IF @Erp_PcValueGrpList_udTableExists = 0
   SELECT N'[Erp].[PcValueGrp_UD]'
ELSE
   SELECT [UD].* FROM [Erp].[PcValueGrp_UD] AS [UD], @Erp_PcValueGrpList AS [tt0] WHERE [UD].ForeignSysRowID = [tt0].SysRowID ORDER BY ID


END
