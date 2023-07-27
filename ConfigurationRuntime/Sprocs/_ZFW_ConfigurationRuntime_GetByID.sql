/*** Object: StoredProcedure [Erp].[_ZFW_ConfigurationRuntime_GetByID] ******/

CREATE OR ALTER PROCEDURE [Erp].[_ZFW_ConfigurationRuntime_GetByID]
	-- Add the parameters for the stored procedure here
	@Company nvarchar(8),
	@GroupSeq int,
	@PcValueGrp_WhereClause nvarchar(max) = null,
	@PcValueGrp_OrderBy nvarchar(max) = null,
	@PcValueHead_WhereClause nvarchar(max) = null,
	@PcValueHead_OrderBy nvarchar(max) = null,
	@PcInputsLayerDetail_WhereClause nvarchar(max) = null,
	@PcInputsLayerDetail_OrderBy nvarchar(max) = null,
	@PcInputsLayerHeader_WhereClause nvarchar(max) = null,
	@PcInputsLayerHeader_OrderBy nvarchar(max) = null,
	@PcInputVar_WhereClause nvarchar(max) = null,
	@PcInputVar_OrderBy nvarchar(max) = null,
	@PcValueInputLayerDetail_WhereClause nvarchar(max) = null,
	@PcValueInputLayerDetail_OrderBy nvarchar(max) = null,
	@PcValueInputLayerHeader_WhereClause nvarchar(max) = null,
	@PcValueInputLayerHeader_OrderBy nvarchar(max) = null,
	@QBuildMapping_WhereClause nvarchar(max) = null,
	@QBuildMapping_OrderBy nvarchar(max) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


DECLARE @Erp_PcValueGrp_udTableExists bit = 0
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Erp].[PcValueGrp_UD]') AND type = N'U')
	SET @Erp_PcValueGrp_udTableExists = 1


DECLARE @Erp_PcValueHead_udTableExists bit = 0
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Erp].[PcValueHead_UD]') AND type = N'U')
	SET @Erp_PcValueHead_udTableExists = 1


DECLARE @ERP_PcInputsLayerDetail_udTableExists bit = 0
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Erp].[PcInputsLayerDetail_UD]') AND type = N'U')
	SET @ERP_PcInputsLayerDetail_udTableExists = 1


DECLARE @ERP_PcInputsLayerHeader_udTableExists bit = 0
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Erp].[PcInputsLayerHeader_UD]') AND type = N'U')
	SET @ERP_PcInputsLayerHeader_udTableExists = 1


DECLARE @Erp_PcInputVar_udTableExists bit = 0
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Erp].[PcInputVar_UD]') AND type = N'U')
	SET @Erp_PcInputVar_udTableExists = 1


DECLARE @ERP_PcValueInputLayerDetail_udTableExists bit = 0
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Erp].[PcValueInputLayerDetail_UD]') AND type = N'U')
	SET @ERP_PcValueInputLayerDetail_udTableExists = 1


DECLARE @ERP_PcValueInputLayerHeader_udTableExists bit = 0
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Erp].[PcValueInputLayerHeader_UD]') AND type = N'U')
	SET @ERP_PcValueInputLayerHeader_udTableExists = 1


DECLARE @ERP_QBuildMapping_udTableExists bit = 0
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Erp].[QBuildMapping_UD]') AND type = N'U')
	SET @ERP_QBuildMapping_udTableExists = 1

-- Create a temp table to hold rowids of the selected rows in this table.
DECLARE @Erp_PcValueGrp Ice.JoinTableType;

--Insert the rowids into the temp table to join against later
IF (@PcValueGrp_WhereClause IS null OR @PcValueGrp_WhereClause='')
	--No parameters passed for this table, direct sql can be used
	BEGIN
		INSERT INTO @Erp_PcValueGrp
		SELECT [PcValueGrp].[SysRowID]
		FROM [Erp].[PcValueGrp] AS [PcValueGrp]
		WHERE [PcValueGrp].[Company] = @Company AND [PcValueGrp].[GroupSeq] = @GroupSeq
	END
ELSE
	--Parameters passed for this table, fall back to dynamic sql to obtain the rowids
	BEGIN
		--Declare and build the dynamic sql statement to populate the rowids
		DECLARE @PcValueGrp_SelectFrom nvarchar(512)
		IF (@Erp_PcValueGrp_udTableExists = 1)
		BEGIN
			SET @PcValueGrp_SelectFrom = N'
			[Erp].[PcValueGrp] AS [PcValueGrp]
			LEFT JOIN [Erp].[PcValueGrp_UD] AS [PcValueGrp_UD] ON [PcValueGrp_UD].[ForeignSysRowID] = [PcValueGrp].[SysRowID]';
		END
		ELSE
		   SET @PcValueGrp_SelectFrom = N'[Erp].[PcValueGrp] AS [PcValueGrp]';

		DECLARE @PcValueGrp_SelectString NVARCHAR(max)
		SET @PcValueGrp_SelectString = N'
			SELECT [PcValueGrp].[SysRowID]
			FROM ' + @PcValueGrp_SelectFrom + '
			WHERE [PcValueGrp].[Company] = @Company AND [PcValueGrp].[GroupSeq] = @GroupSeq
			AND (' + @PcValueGrp_WhereClause + ')'

		--Execute the dynamic sql to populate the temp table with rowids
		DECLARE @ParmDefinition nvarchar(500) = N'@Company nvarchar(8), @GroupSeq int';
		INSERT INTO @Erp_PcValueGrp
		EXEC sp_executesql @PcValueGrp_SelectString, @ParmDefinition, @Company, @GroupSeq
	END

-- Create a temp table to hold rowids of the selected rows in this table.
DECLARE @Erp_PcValueHead Ice.JoinTablePagingType;

--Insert the rowids into the temp table to join against later
IF (@PcValueHead_WhereClause IS null OR @PcValueHead_WhereClause='') AND (@PcValueHead_OrderBy IS null OR @PcValueHead_OrderBy='')
	--No parameters passed for this table, direct sql can be used
	BEGIN
		INSERT INTO @Erp_PcValueHead
		SELECT [PcValueHead].[SysRowID]
		FROM [Erp].[PcValueHead] AS [PcValueHead]
		JOIN [Erp].[PcValueGrp] AS [PcValueGrp] ON ([PcValueHead].[Company] = [PcValueGrp].[Company] AND [PcValueHead].[GroupSeq] = [PcValueGrp].[GroupSeq])
		JOIN @Erp_PcValueGrp AS [tt] ON ([tt].[SysRowID] = [PcValueGrp].[SysRowID])
		ORDER BY [PcValueHead].[Company], [PcValueHead].[GroupSeq], [PcValueHead].[HeadNum]
	END
ELSE
	--Parameters passed for this table, fall back to dynamic sql to obtain the rowids
	BEGIN
		--If no order by clause was passed, infer the order by from the primary index of the table
		IF @PcValueHead_OrderBy IS NULL OR @PcValueHead_OrderBy = ''
			SET @PcValueHead_OrderBy = 'ORDER BY [PcValueHead].[Company], [PcValueHead].[GroupSeq], [PcValueHead].[HeadNum]'

		--If a where clause was passed, add the 'WHERE' keyword to the front of the statement
		IF @PcValueHead_WhereClause IS NOT NULL AND @PcValueHead_WhereClause <> ''
			SET @PcValueHead_WhereClause = 'WHERE ' + @PcValueHead_WhereClause
		ELSE
			SET @PcValueHead_WhereClause = ''
		--Declare and build the dynamic sql statement to populate the rowids
		DECLARE @PcValueHead_SelectFrom nvarchar(512)
		IF (@Erp_PcValueHead_udTableExists = 1)
		BEGIN
			SET @PcValueHead_SelectFrom = N'
			[Erp].[PcValueHead] AS [PcValueHead]
			LEFT JOIN [Erp].[PcValueHead_UD] AS [PcValueHead_UD] ON [PcValueHead_UD].[ForeignSysRowID] = [PcValueHead].[SysRowID]';
		END
		ELSE
		   SET @PcValueHead_SelectFrom = N'[Erp].[PcValueHead] AS [PcValueHead]';

		DECLARE @PcValueHead_SelectString NVARCHAR(max)
		SET @PcValueHead_SelectString = N'
			SELECT [PcValueHead].[SysRowID]
			FROM ' + @PcValueHead_SelectFrom + '
			JOIN [Erp].[PcValueGrp] AS [PcValueGrp] ON ([PcValueHead].[Company] = [PcValueGrp].[Company] AND [PcValueHead].[GroupSeq] = [PcValueGrp].[GroupSeq])
			JOIN @Erp_PcValueGrp AS [tt] ON ([tt].[SysRowID] = [PcValueGrp].[SysRowID])
			' + @PcValueHead_WhereClause + @PcValueHead_OrderBy

		--Execute the dynamic sql to populate the temp table with rowids
		SET @ParmDefinition = N'@Erp_PcValueGrp Ice.JoinTableType readonly';
		INSERT INTO @Erp_PcValueHead
		EXEC sp_executesql @PcValueHead_SelectString, @ParmDefinition, @Erp_PcValueGrp
	END

-- Create a temp table to hold rowids of the selected rows in this table.
DECLARE @ERP_PcInputsLayerDetail Ice.JoinTablePagingType;

--Insert the rowids into the temp table to join against later
IF (@PcInputsLayerDetail_WhereClause IS null OR @PcInputsLayerDetail_WhereClause='') AND (@PcInputsLayerDetail_OrderBy IS null OR @PcInputsLayerDetail_OrderBy='')
	--No parameters passed for this table, direct sql can be used
	BEGIN
		INSERT INTO @ERP_PcInputsLayerDetail
		SELECT [PcInputsLayerDetail].[SysRowID]
		FROM [Erp].[PcInputsLayerDetail] AS [PcInputsLayerDetail]
		WHERE 0 = 1
	END
ELSE
	--Parameters passed for this table, fall back to dynamic sql to obtain the rowids
	BEGIN
		--If no order by clause was passed, infer the order by from the primary index of the table
		IF @PcInputsLayerDetail_OrderBy IS NULL OR @PcInputsLayerDetail_OrderBy = ''
			SET @PcInputsLayerDetail_OrderBy = 'ORDER BY [PcInputsLayerDetail].[Company], [PcInputsLayerDetail].[ConfigID], [PcInputsLayerDetail].[InputName], [PcInputsLayerDetail].[ImageLayerID], [PcInputsLayerDetail].[LayerSeq]'

		--If a where clause was passed, add the 'WHERE' keyword to the front of the statement
		IF @PcInputsLayerDetail_WhereClause IS NOT NULL AND @PcInputsLayerDetail_WhereClause <> ''
			SET @PcInputsLayerDetail_WhereClause = 'WHERE ' + @PcInputsLayerDetail_WhereClause
		ELSE
			SET @PcInputsLayerDetail_WhereClause = ''
		--Declare and build the dynamic sql statement to populate the rowids
		DECLARE @PcInputsLayerDetail_SelectFrom nvarchar(512)
		IF (@ERP_PcInputsLayerDetail_udTableExists = 1)
		BEGIN
			SET @PcInputsLayerDetail_SelectFrom = N'
			[Erp].[PcInputsLayerDetail] AS [PcInputsLayerDetail]
			LEFT JOIN [Erp].[PcInputsLayerDetail_UD] AS [PcInputsLayerDetail_UD] ON [PcInputsLayerDetail_UD].[ForeignSysRowID] = [PcInputsLayerDetail].[SysRowID]';
		END
		ELSE
		   SET @PcInputsLayerDetail_SelectFrom = N'[Erp].[PcInputsLayerDetail] AS [PcInputsLayerDetail]';

		DECLARE @PcInputsLayerDetail_SelectString NVARCHAR(max)
		SET @PcInputsLayerDetail_SelectString = N'
			SELECT [PcInputsLayerDetail].[SysRowID]
			FROM ' + @PcInputsLayerDetail_SelectFrom + '
			WHERE 0 = 1'

		--Execute the dynamic sql to populate the temp table with rowids
		INSERT INTO @ERP_PcInputsLayerDetail
		EXEC (@PcInputsLayerDetail_SelectString)
	END

-- Create a temp table to hold rowids of the selected rows in this table.
DECLARE @ERP_PcInputsLayerHeader Ice.JoinTablePagingType;

--Insert the rowids into the temp table to join against later
IF (@PcInputsLayerHeader_WhereClause IS null OR @PcInputsLayerHeader_WhereClause='') AND (@PcInputsLayerHeader_OrderBy IS null OR @PcInputsLayerHeader_OrderBy='')
	--No parameters passed for this table, direct sql can be used
	BEGIN
		INSERT INTO @ERP_PcInputsLayerHeader
		SELECT [PcInputsLayerHeader].[SysRowID]
		FROM [Erp].[PcInputsLayerHeader] AS [PcInputsLayerHeader]
		WHERE 0 = 1
	END
ELSE
	--Parameters passed for this table, fall back to dynamic sql to obtain the rowids
	BEGIN
		--If no order by clause was passed, infer the order by from the primary index of the table
		IF @PcInputsLayerHeader_OrderBy IS NULL OR @PcInputsLayerHeader_OrderBy = ''
			SET @PcInputsLayerHeader_OrderBy = 'ORDER BY [PcInputsLayerHeader].[Company], [PcInputsLayerHeader].[ConfigID], [PcInputsLayerHeader].[InputName], [PcInputsLayerHeader].[ImageLayerID]'

		--If a where clause was passed, add the 'WHERE' keyword to the front of the statement
		IF @PcInputsLayerHeader_WhereClause IS NOT NULL AND @PcInputsLayerHeader_WhereClause <> ''
			SET @PcInputsLayerHeader_WhereClause = 'WHERE ' + @PcInputsLayerHeader_WhereClause
		ELSE
			SET @PcInputsLayerHeader_WhereClause = ''
		--Declare and build the dynamic sql statement to populate the rowids
		DECLARE @PcInputsLayerHeader_SelectFrom nvarchar(512)
		IF (@ERP_PcInputsLayerHeader_udTableExists = 1)
		BEGIN
			SET @PcInputsLayerHeader_SelectFrom = N'
			[Erp].[PcInputsLayerHeader] AS [PcInputsLayerHeader]
			LEFT JOIN [Erp].[PcInputsLayerHeader_UD] AS [PcInputsLayerHeader_UD] ON [PcInputsLayerHeader_UD].[ForeignSysRowID] = [PcInputsLayerHeader].[SysRowID]';
		END
		ELSE
		   SET @PcInputsLayerHeader_SelectFrom = N'[Erp].[PcInputsLayerHeader] AS [PcInputsLayerHeader]';

		DECLARE @PcInputsLayerHeader_SelectString NVARCHAR(max)
		SET @PcInputsLayerHeader_SelectString = N'
			SELECT [PcInputsLayerHeader].[SysRowID]
			FROM ' + @PcInputsLayerHeader_SelectFrom + '
			WHERE 0 = 1'

		--Execute the dynamic sql to populate the temp table with rowids
		INSERT INTO @ERP_PcInputsLayerHeader
		EXEC (@PcInputsLayerHeader_SelectString)
	END

-- Create a temp table to hold rowids of the selected rows in this table.
DECLARE @Erp_PcInputVar Ice.JoinTablePagingType;

--Insert the rowids into the temp table to join against later
IF (@PcInputVar_WhereClause IS null OR @PcInputVar_WhereClause='') AND (@PcInputVar_OrderBy IS null OR @PcInputVar_OrderBy='')
	--No parameters passed for this table, direct sql can be used
	BEGIN
		INSERT INTO @Erp_PcInputVar
		SELECT [PcInputVar].[SysRowID]
		FROM [Erp].[PcInputVar] AS [PcInputVar]
		WHERE 0 = 1
	END
ELSE
	--Parameters passed for this table, fall back to dynamic sql to obtain the rowids
	BEGIN
		--If no order by clause was passed, infer the order by from the primary index of the table
		IF @PcInputVar_OrderBy IS NULL OR @PcInputVar_OrderBy = ''
			SET @PcInputVar_OrderBy = 'ORDER BY [PcInputVar].[Company], [PcInputVar].[VarName]'

		--If a where clause was passed, add the 'WHERE' keyword to the front of the statement
		IF @PcInputVar_WhereClause IS NOT NULL AND @PcInputVar_WhereClause <> ''
			SET @PcInputVar_WhereClause = 'WHERE ' + @PcInputVar_WhereClause
		ELSE
			SET @PcInputVar_WhereClause = ''
		--Declare and build the dynamic sql statement to populate the rowids
		DECLARE @PcInputVar_SelectFrom nvarchar(512)
		IF (@Erp_PcInputVar_udTableExists = 1)
		BEGIN
			SET @PcInputVar_SelectFrom = N'
			[Erp].[PcInputVar] AS [PcInputVar]
			LEFT JOIN [Erp].[PcInputVar_UD] AS [PcInputVar_UD] ON [PcInputVar_UD].[ForeignSysRowID] = [PcInputVar].[SysRowID]';
		END
		ELSE
		   SET @PcInputVar_SelectFrom = N'[Erp].[PcInputVar] AS [PcInputVar]';

		DECLARE @PcInputVar_SelectString NVARCHAR(max)
		SET @PcInputVar_SelectString = N'
			SELECT [PcInputVar].[SysRowID]
			FROM ' + @PcInputVar_SelectFrom + '
			WHERE 0 = 1'

		--Execute the dynamic sql to populate the temp table with rowids
		INSERT INTO @Erp_PcInputVar
		EXEC (@PcInputVar_SelectString)
	END

-- Create a temp table to hold rowids of the selected rows in this table.
DECLARE @ERP_PcValueInputLayerDetail Ice.JoinTablePagingType;

--Insert the rowids into the temp table to join against later
IF (@PcValueInputLayerDetail_WhereClause IS null OR @PcValueInputLayerDetail_WhereClause='') AND (@PcValueInputLayerDetail_OrderBy IS null OR @PcValueInputLayerDetail_OrderBy='')
	--No parameters passed for this table, direct sql can be used
	BEGIN
		INSERT INTO @ERP_PcValueInputLayerDetail
		SELECT [PcValueInputLayerDetail].[SysRowID]
		FROM [Erp].[PcValueInputLayerDetail] AS [PcValueInputLayerDetail]
		WHERE 0 = 1
	END
ELSE
	--Parameters passed for this table, fall back to dynamic sql to obtain the rowids
	BEGIN
		--If no order by clause was passed, infer the order by from the primary index of the table
		IF @PcValueInputLayerDetail_OrderBy IS NULL OR @PcValueInputLayerDetail_OrderBy = ''
			SET @PcValueInputLayerDetail_OrderBy = 'ORDER BY [PcValueInputLayerDetail].[Company], [PcValueInputLayerDetail].[GroupSeq], [PcValueInputLayerDetail].[HeadNum], [PcValueInputLayerDetail].[ConfigID], [PcValueInputLayerDetail].[InputName], [PcValueInputLayerDetail].[ImageLayerID], [PcValueInputLayerDetail].[LayerSeq]'

		--If a where clause was passed, add the 'WHERE' keyword to the front of the statement
		IF @PcValueInputLayerDetail_WhereClause IS NOT NULL AND @PcValueInputLayerDetail_WhereClause <> ''
			SET @PcValueInputLayerDetail_WhereClause = 'WHERE ' + @PcValueInputLayerDetail_WhereClause
		ELSE
			SET @PcValueInputLayerDetail_WhereClause = ''
		--Declare and build the dynamic sql statement to populate the rowids
		DECLARE @PcValueInputLayerDetail_SelectFrom nvarchar(512)
		IF (@ERP_PcValueInputLayerDetail_udTableExists = 1)
		BEGIN
			SET @PcValueInputLayerDetail_SelectFrom = N'
			[Erp].[PcValueInputLayerDetail] AS [PcValueInputLayerDetail]
			LEFT JOIN [Erp].[PcValueInputLayerDetail_UD] AS [PcValueInputLayerDetail_UD] ON [PcValueInputLayerDetail_UD].[ForeignSysRowID] = [PcValueInputLayerDetail].[SysRowID]';
		END
		ELSE
		   SET @PcValueInputLayerDetail_SelectFrom = N'[Erp].[PcValueInputLayerDetail] AS [PcValueInputLayerDetail]';

		DECLARE @PcValueInputLayerDetail_SelectString NVARCHAR(max)
		SET @PcValueInputLayerDetail_SelectString = N'
			SELECT [PcValueInputLayerDetail].[SysRowID]
			FROM ' + @PcValueInputLayerDetail_SelectFrom + '
			WHERE 0 = 1'

		--Execute the dynamic sql to populate the temp table with rowids
		INSERT INTO @ERP_PcValueInputLayerDetail
		EXEC (@PcValueInputLayerDetail_SelectString)
	END

-- Create a temp table to hold rowids of the selected rows in this table.
DECLARE @ERP_PcValueInputLayerHeader Ice.JoinTablePagingType;

--Insert the rowids into the temp table to join against later
IF (@PcValueInputLayerHeader_WhereClause IS null OR @PcValueInputLayerHeader_WhereClause='') AND (@PcValueInputLayerHeader_OrderBy IS null OR @PcValueInputLayerHeader_OrderBy='')
	--No parameters passed for this table, direct sql can be used
	BEGIN
		INSERT INTO @ERP_PcValueInputLayerHeader
		SELECT [PcValueInputLayerHeader].[SysRowID]
		FROM [Erp].[PcValueInputLayerHeader] AS [PcValueInputLayerHeader]
		WHERE 0 = 1
	END
ELSE
	--Parameters passed for this table, fall back to dynamic sql to obtain the rowids
	BEGIN
		--If no order by clause was passed, infer the order by from the primary index of the table
		IF @PcValueInputLayerHeader_OrderBy IS NULL OR @PcValueInputLayerHeader_OrderBy = ''
			SET @PcValueInputLayerHeader_OrderBy = 'ORDER BY [PcValueInputLayerHeader].[Company], [PcValueInputLayerHeader].[GroupSeq], [PcValueInputLayerHeader].[HeadNum], [PcValueInputLayerHeader].[ConfigID], [PcValueInputLayerHeader].[InputName], [PcValueInputLayerHeader].[ImageLayerID]'

		--If a where clause was passed, add the 'WHERE' keyword to the front of the statement
		IF @PcValueInputLayerHeader_WhereClause IS NOT NULL AND @PcValueInputLayerHeader_WhereClause <> ''
			SET @PcValueInputLayerHeader_WhereClause = 'WHERE ' + @PcValueInputLayerHeader_WhereClause
		ELSE
			SET @PcValueInputLayerHeader_WhereClause = ''
		--Declare and build the dynamic sql statement to populate the rowids
		DECLARE @PcValueInputLayerHeader_SelectFrom nvarchar(512)
		IF (@ERP_PcValueInputLayerHeader_udTableExists = 1)
		BEGIN
			SET @PcValueInputLayerHeader_SelectFrom = N'
			[Erp].[PcValueInputLayerHeader] AS [PcValueInputLayerHeader]
			LEFT JOIN [Erp].[PcValueInputLayerHeader_UD] AS [PcValueInputLayerHeader_UD] ON [PcValueInputLayerHeader_UD].[ForeignSysRowID] = [PcValueInputLayerHeader].[SysRowID]';
		END
		ELSE
		   SET @PcValueInputLayerHeader_SelectFrom = N'[Erp].[PcValueInputLayerHeader] AS [PcValueInputLayerHeader]';

		DECLARE @PcValueInputLayerHeader_SelectString NVARCHAR(max)
		SET @PcValueInputLayerHeader_SelectString = N'
			SELECT [PcValueInputLayerHeader].[SysRowID]
			FROM ' + @PcValueInputLayerHeader_SelectFrom + '
			WHERE 0 = 1'

		--Execute the dynamic sql to populate the temp table with rowids
		INSERT INTO @ERP_PcValueInputLayerHeader
		EXEC (@PcValueInputLayerHeader_SelectString)
	END

-- Create a temp table to hold rowids of the selected rows in this table.
DECLARE @ERP_QBuildMapping Ice.JoinTablePagingType;

--Insert the rowids into the temp table to join against later
IF (@QBuildMapping_WhereClause IS null OR @QBuildMapping_WhereClause='') AND (@QBuildMapping_OrderBy IS null OR @QBuildMapping_OrderBy='')
	--No parameters passed for this table, direct sql can be used
	BEGIN
		INSERT INTO @ERP_QBuildMapping
		SELECT [QBuildMapping].[SysRowID]
		FROM [Erp].[QBuildMapping] AS [QBuildMapping]
		WHERE 0 = 1
	END
ELSE
	--Parameters passed for this table, fall back to dynamic sql to obtain the rowids
	BEGIN
		--If no order by clause was passed, infer the order by from the primary index of the table
		IF @QBuildMapping_OrderBy IS NULL OR @QBuildMapping_OrderBy = ''
			SET @QBuildMapping_OrderBy = 'ORDER BY [QBuildMapping].[Company], [QBuildMapping].[ConfigID], [QBuildMapping].[InputName], [QBuildMapping].[ObjName], [QBuildMapping].[ObjParameter]'

		--If a where clause was passed, add the 'WHERE' keyword to the front of the statement
		IF @QBuildMapping_WhereClause IS NOT NULL AND @QBuildMapping_WhereClause <> ''
			SET @QBuildMapping_WhereClause = 'WHERE ' + @QBuildMapping_WhereClause
		ELSE
			SET @QBuildMapping_WhereClause = ''
		--Declare and build the dynamic sql statement to populate the rowids
		DECLARE @QBuildMapping_SelectFrom nvarchar(512)
		IF (@ERP_QBuildMapping_udTableExists = 1)
		BEGIN
			SET @QBuildMapping_SelectFrom = N'
			[Erp].[QBuildMapping] AS [QBuildMapping]
			LEFT JOIN [Erp].[QBuildMapping_UD] AS [QBuildMapping_UD] ON [QBuildMapping_UD].[ForeignSysRowID] = [QBuildMapping].[SysRowID]';
		END
		ELSE
		   SET @QBuildMapping_SelectFrom = N'[Erp].[QBuildMapping] AS [QBuildMapping]';

		DECLARE @QBuildMapping_SelectString NVARCHAR(max)
		SET @QBuildMapping_SelectString = N'
			SELECT [QBuildMapping].[SysRowID]
			FROM ' + @QBuildMapping_SelectFrom + '
			WHERE 0 = 1'

		--Execute the dynamic sql to populate the temp table with rowids
		INSERT INTO @ERP_QBuildMapping
		EXEC (@QBuildMapping_SelectString)
	END

---------------------------------------------------------------------
-- SELECT for table PcValueGrp AS PcValueGrp / Table Number : 1
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
[t0].[HeadNum],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
CAST(0 AS bit) AS [DisplaySummary],
CAST(0 AS bit) AS [IncomingSmartString],
CAST(null AS uniqueidentifier) AS [TestID],
'' AS [TestMode],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[PcValueGrp] AS [t0]
JOIN @Erp_PcValueGrp AS [tt] ON ([tt].[SysRowID] = [t0].[SysRowID])
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcValueGrp' AND b1.RelatedToRowId = t0.SysRowID
---------------------------------------------------------------------
-- SELECT for table PcValueHead AS PcValueHead / Table Number : 2
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
JOIN @Erp_PcValueHead AS [tt] ON ([tt].[SysRowID] = [t0].[SysRowID])
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcValueHead' AND b1.RelatedToRowId = t0.SysRowID
ORDER BY ID
---------------------------------------------------------------------
-- SELECT for table  AS PcConfigurationParams / Table Number : 3
---------------------------------------------------------------------
DECLARE @PcConfigurationParams TABLE
(
    [ForeignTableName] nvarchar(50),
    [ForeignSysRowID] uniqueidentifier,
    [TgtStructTag] nvarchar(50),
    [StructID] int,
    [InSmartString] nvarchar(50),
    [IsTestPlan] bit,
    [SpecID] nvarchar(50),
    [SpecRevNum] nvarchar(50),
    [InspType] nvarchar(50),
    [RunningStateOverride] nvarchar(50),
    [EqmPassed] nvarchar(50),
    [EqmFailDesc] nvarchar(50),
    [EqmOverride] bit,
    [RelatedToTable] nvarchar(50),
    [RelatedToSysRowID] uniqueidentifier,
    [SourceTable] nvarchar(50),
    [TestID] uniqueidentifier,
    [TestMode] nvarchar(50),
    [AppServerID] nvarchar(50),
    [PcStatusSysRowID] uniqueidentifier,
    [ConfigVersion] int,
    [UniqueID] nvarchar(50),
    [ConfigID] nvarchar(50),
    [Company] nvarchar(50),
    [InputPricingSet] bit,
    [OrderPrice] decimal(20,9),
    [QuotePrice] decimal(20,9),
    [DemandPrice] decimal(20,9),
    [PurchasePrice] decimal(20,9),
    [WebOrderBasketPrice] decimal(20,9),
    [PartNum] nvarchar(50),
    [RevisionNum] nvarchar(50),
    [AltMethod] nvarchar(50),
    [InitialStructTag] nvarchar(50),
    [InitialRuleTag] nvarchar(50),
    [TrackerMode] bit,
    [ConfigType] nvarchar(50),
    [EWCConfiguratorURL] nvarchar(50),
    [SysRowID] uniqueidentifier
);
SELECT * FROM @PcConfigurationParams
---------------------------------------------------------------------
-- SELECT for table  AS PcConfiguredDrawings / Table Number : 4
---------------------------------------------------------------------
DECLARE @PcConfiguredDrawings TABLE
(
    [Company] nvarchar(50),
    [ConfigID] nvarchar(50),
    [GroupSeq] int,
    [HeadNum] int,
    [InputName] nvarchar(50),
    [ImageID] nvarchar(50),
    [Content] varbinary,
    [PageSeq] int,
    [QBuildObjExist] bit,
    [SysRowID] uniqueidentifier
);
SELECT * FROM @PcConfiguredDrawings
---------------------------------------------------------------------
-- SELECT for table  AS PcContextProperties / Table Number : 5
---------------------------------------------------------------------
DECLARE @PcContextProperties TABLE
(
    [AttributeID] nvarchar(50),
    [CompanyID] nvarchar(50),
    [ConfigurationID] nvarchar(50),
    [Currency] nvarchar(50),
    [CustomerID] nvarchar(50),
    [DemandHeadNumber] int,
    [DemandLineNumber] int,
    [Entity] nvarchar(50),
    [JobNumber] nvarchar(50),
    [OrderDetailNumber] int,
    [OrderNumber] int,
    [PackSlip] nvarchar(50),
    [PartNumber] nvarchar(50),
    [PartRevisionNumber] nvarchar(50),
    [PODetailNumber] int,
    [PONumber] int,
    [QuoteLineNumber] int,
    [QuoteNumber] int,
    [SpecificationID] nvarchar(50),
    [SpecificationRevision] nvarchar(50),
    [SupplierID] nvarchar(50),
    [UserID] nvarchar(50),
    [NonConfID] nvarchar(50),
    [AssemblySeq] int,
    [OprSeq] int,
    [RMALine] int,
    [PackLine] int,
    [SiteID] nvarchar(50),
    [ECCQuoteNum] nvarchar(50),
    [CustomerNumber] int,
    [SupplierNumber] int,
    [SysRowID] uniqueidentifier
);
SELECT * FROM @PcContextProperties
---------------------------------------------------------------------
-- SELECT for table PcInputsLayerDetail AS PcInputsLayerDetail / Table Number : 6
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
JOIN @ERP_PcInputsLayerDetail AS [tt] ON ([tt].[SysRowID] = [t0].[SysRowID])
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcInputsLayerDetail' AND b1.RelatedToRowId = t0.SysRowID
---------------------------------------------------------------------
-- SELECT for table PcInputsLayerHeader AS PcInputsLayerHeader / Table Number : 7
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
JOIN @ERP_PcInputsLayerHeader AS [tt] ON ([tt].[SysRowID] = [t0].[SysRowID])
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcInputsLayerHeader' AND b1.RelatedToRowId = t0.SysRowID
---------------------------------------------------------------------
-- SELECT for table  AS PcInputsPublishToDocParams / Table Number : 8
---------------------------------------------------------------------
DECLARE @PcInputsPublishToDocParams TABLE
(
    [Key] nvarchar(50),
    [Value] nvarchar(50),
    [Company] nvarchar(50),
    [SysRowID] uniqueidentifier
);
SELECT * FROM @PcInputsPublishToDocParams
---------------------------------------------------------------------
-- SELECT for table PcInputVar AS PcInputVar / Table Number : 9
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
JOIN @Erp_PcInputVar AS [tt] ON ([tt].[SysRowID] = [t0].[SysRowID])
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcInputVar' AND b1.RelatedToRowId = t0.SysRowID
---------------------------------------------------------------------
-- SELECT for table PcValueInputLayerDetail AS PcValueInputLayerDetail / Table Number : 10
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[GroupSeq],
[t0].[HeadNum],
[t0].[ConfigID],
[t0].[InputName],
[t0].[ImageLayerID],
[t0].[LayerSeq],
[t0].[LayerName],
[t0].[LayerDesc],
[t0].[ZIndex],
[t0].[ImageID],
[t0].[FileType],
[t0].[Width],
[t0].[Height],
[t0].[Category],
[t0].[xPos],
[t0].[yPos],
CAST([t0].[SysRevID] AS BigInt) AS SysRevID,
[t0].[SysRowID],
ISNULL(b1.BitValues, CAST(0 AS int)) AS [BitFlag]
FROM [Erp].[PcValueInputLayerDetail] AS [t0]
JOIN @ERP_PcValueInputLayerDetail AS [tt] ON ([tt].[SysRowID] = [t0].[SysRowID])
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcValueInputLayerDetail' AND b1.RelatedToRowId = t0.SysRowID
---------------------------------------------------------------------
-- SELECT for table PcValueInputLayerHeader AS PcValueInputLayerHeader / Table Number : 11
---------------------------------------------------------------------
SELECT [t0].[Company],
[t0].[GroupSeq],
[t0].[HeadNum],
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
FROM [Erp].[PcValueInputLayerHeader] AS [t0]
JOIN @ERP_PcValueInputLayerHeader AS [tt] ON ([tt].[SysRowID] = [t0].[SysRowID])
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'PcValueInputLayerHeader' AND b1.RelatedToRowId = t0.SysRowID
---------------------------------------------------------------------
-- SELECT for table QBuildMapping AS QBuildMapping / Table Number : 12
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
JOIN @ERP_QBuildMapping AS [tt] ON ([tt].[SysRowID] = [t0].[SysRowID])
LEFT JOIN [Ice].[BitFlag] AS b1 ON b1.RelatedToSchemaName=N'Erp' AND b1.RelatedToTable=N'QBuildMapping' AND b1.RelatedToRowId = t0.SysRowID
LEFT JOIN [Erp].[QBuildObj] AS [L1] ON ([L1].[Company] = [t0].[Company] AND [L1].[ConfigID] = [t0].[ConfigID] AND [L1].[InputName] = [t0].[InputName] AND [L1].[ObjName] = [t0].[ObjName])

IF @Erp_PcValueGrp_udTableExists = 0
   SELECT N'[Erp].[PcValueGrp_UD]'
ELSE
   SELECT [UD].* FROM [Erp].[PcValueGrp_UD] AS [UD], @Erp_PcValueGrp AS [tt0] WHERE [UD].ForeignSysRowID = [tt0].SysRowID


IF @Erp_PcValueHead_udTableExists = 0
   SELECT N'[Erp].[PcValueHead_UD]'
ELSE
   SELECT [UD].* FROM [Erp].[PcValueHead_UD] AS [UD], @Erp_PcValueHead AS [tt0] WHERE [UD].ForeignSysRowID = [tt0].SysRowID


IF @ERP_PcInputsLayerDetail_udTableExists = 0
   SELECT N'[Erp].[PcInputsLayerDetail_UD]'
ELSE
   SELECT [UD].* FROM [Erp].[PcInputsLayerDetail_UD] AS [UD], @ERP_PcInputsLayerDetail AS [tt0] WHERE [UD].ForeignSysRowID = [tt0].SysRowID


IF @ERP_PcInputsLayerHeader_udTableExists = 0
   SELECT N'[Erp].[PcInputsLayerHeader_UD]'
ELSE
   SELECT [UD].* FROM [Erp].[PcInputsLayerHeader_UD] AS [UD], @ERP_PcInputsLayerHeader AS [tt0] WHERE [UD].ForeignSysRowID = [tt0].SysRowID


IF @Erp_PcInputVar_udTableExists = 0
   SELECT N'[Erp].[PcInputVar_UD]'
ELSE
   SELECT [UD].* FROM [Erp].[PcInputVar_UD] AS [UD], @Erp_PcInputVar AS [tt0] WHERE [UD].ForeignSysRowID = [tt0].SysRowID


IF @ERP_PcValueInputLayerDetail_udTableExists = 0
   SELECT N'[Erp].[PcValueInputLayerDetail_UD]'
ELSE
   SELECT [UD].* FROM [Erp].[PcValueInputLayerDetail_UD] AS [UD], @ERP_PcValueInputLayerDetail AS [tt0] WHERE [UD].ForeignSysRowID = [tt0].SysRowID


IF @ERP_PcValueInputLayerHeader_udTableExists = 0
   SELECT N'[Erp].[PcValueInputLayerHeader_UD]'
ELSE
   SELECT [UD].* FROM [Erp].[PcValueInputLayerHeader_UD] AS [UD], @ERP_PcValueInputLayerHeader AS [tt0] WHERE [UD].ForeignSysRowID = [tt0].SysRowID


IF @ERP_QBuildMapping_udTableExists = 0
   SELECT N'[Erp].[QBuildMapping_UD]'
ELSE
   SELECT [UD].* FROM [Erp].[QBuildMapping_UD] AS [UD], @ERP_QBuildMapping AS [tt0] WHERE [UD].ForeignSysRowID = [tt0].SysRowID

END
