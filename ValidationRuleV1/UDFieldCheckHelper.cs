using Epicor.Data;
using Epicor.Hosting;
using Epicor.Utilities;
using Erp.Tables;
using Ice.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Strings = Erp.Internal.PE.Resources.Strings;

namespace Erp.Internal.PE
{
    public class UDFieldDiagnosticMessage
    {
        public System.Diagnostics.TraceLevel level { get; set; }
        public string message { get; set; }
    }

    public class UDFieldCheckHelper : Ice.Libraries.ContextLibraryBase<Erp.ErpContext>
    {
        public static string[] SymbolsTypes = { "nvarchar", "varchar" };

        public bool StructureCheckProcessed = false;

        public class UDFieldCacheData
        {
            private string format;

            public string TableName { get; set; }
            public string Key { get; set; }
            public string DataType { get; set; }

            public string Format
            {
                get { return this.format; }
                set
                {
                    this.format = value;
                    // Get available length of data for this UD field.
                    System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(this.Format, @"\d+");
                    if (m.Success)
                    {
                        this.Length = Convert.ToInt16(m.Value);
                    }
                }
            }

            public object InitialValue { get; set; }
            public bool Required { get; set; }
            public bool ErrorExists { get; set; }
            public short Length { get; set; }

            public Column Column { get; set; }

            public UDFieldCacheData()
            {
            }

            public UDFieldCacheData(string key)
            {
                this.Key = key;
            }

            /// <summary>
            /// Compares meta definition of UD field with physical column
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                if (this.Column == null)
                    return false;

                if (this.Column.DataType.Equals(this.DataType, StringComparison.OrdinalIgnoreCase))
                {
                    if (SymbolsTypes.Contains(this.DataType, StringComparer.OrdinalIgnoreCase))
                    {
                        if (this.Column.MaxLength.Equals(this.Length) || this.Column.MaxLength == -1) //nvarchar(max)
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return false;

                return true;
            }
        }

        public System.Collections.Generic.Dictionary<string, UDFieldCacheData> CacheUDFieldsChecked = null;

        public System.Collections.Generic.List<UDFieldDiagnosticMessage> UDFieldsDefinitionMessages = new List<UDFieldDiagnosticMessage>();
        public System.Collections.Generic.List<UDFieldDiagnosticMessage> UDFieldsDataMessages = new List<UDFieldDiagnosticMessage>();

        private string TransactionType = String.Empty;

        public UDFieldCheckHelper(Erp.ErpContext ctx)
            : base(ctx)
        {
            CacheUDFieldsChecked = new Dictionary<string, UDFieldCacheData>(StringComparer.InvariantCultureIgnoreCase);
            this.Initialize();
        }

        public void Init(string transactionType)
        {
            this.TransactionType = transactionType;
            UDFieldsDefinitionMessages.Clear();
            UDFieldsDataMessages.Clear();
        }

        /// <summary>
        /// UDFieldsDefinitionsFailed
        /// </summary>
        public bool UDFieldsDefinitionsFailed
        {
            get
            {
                var d = (from row in this.UDFieldsDefinitionMessages where row.level == System.Diagnostics.TraceLevel.Error select row).Any();
                return d;
            }
        }

        /// <summary>
        /// UDFieldsDataFailed
        /// </summary>
        public bool UDFieldsDataFailed
        {
            get
            {
                var d = (from row in this.UDFieldsDataMessages where row.level == System.Diagnostics.TraceLevel.Error select row).Any();
                return d;
            }
        }

        public void CheckUDFieldTranGLC()
        {
            // Check existance of UD field in TranGLC table
            var tranGLCUDFields = (from row in Db.ZDataField
                                   where row.SystemCode == "Erp" && row.DataTableID == "TranGLC_UD" && row.DBFieldName != "ForeignSysRowID" && row.DBFieldName != "UD_SysRevID"
                                   select new UDFieldCacheData { TableName = "TranGLC", Key = row.FieldName, DataType = row.DataType, Format = row.FieldFormat, InitialValue = row.InitialValue, Required = row.Required });

            var TranGLCType = typeof(Erp.Tables.TranGLC);
            var RvTranGLCType = typeof(Erp.Tables.RvTranGLC);

            foreach (var tranGLCUDfield in tranGLCUDFields)
            {
                UDFieldCacheData udFieldCacheData = null;

                if (!CacheUDFieldsChecked.TryGetValue("TranGLC." + tranGLCUDfield.Key, out udFieldCacheData))
                {
                    udFieldCacheData = tranGLCUDfield;

                    // Check physical existance of field
                    var col = FindFirstColumn(Session.SystemCode, "TranGLC_UD", tranGLCUDfield.Key);
                    if (col == null)
                    {
                        this.UDFieldsDefinitionMessages.Add(new UDFieldDiagnosticMessage { level = System.Diagnostics.TraceLevel.Error, message = Strings.ThereIsNoFieldInTable(tranGLCUDfield.Key, TranGLC.GetTableName()) });
                        udFieldCacheData.ErrorExists = true;
                        return;
                    }

                    udFieldCacheData.Column = col;

                    CompareUDFieldDefinitions(udFieldCacheData, "RvTranGLC");

                    CacheUDFieldsChecked.Add("TranGLC." + tranGLCUDfield.Key, udFieldCacheData);
                }
            }
        }

        public void CheckUDFieldsGLJrnDtl()
        {
            // Check existance of UD field in GLJrnDtl table
            var GLJrnDtlFields = (from row in Db.ZDataField
                                  where row.SystemCode == "Erp" && row.DataTableID == "GLJrnDtl_UD" && row.DBFieldName != "ForeignSysRowID" && row.DBFieldName != "UD_SysRevID"
                                  select new UDFieldCacheData { TableName = "GLJrnDtl", Key = row.FieldName, DataType = row.DataType, Format = row.FieldFormat, InitialValue = row.InitialValue, Required = row.Required });

            foreach (var GLJrnDtlField in GLJrnDtlFields)
            {
                UDFieldCacheData udFieldCacheData = null;

                if (!CacheUDFieldsChecked.TryGetValue("GLJrnDtl." + GLJrnDtlField.Key, out udFieldCacheData))
                {
                    udFieldCacheData = GLJrnDtlField;

                    // Check physical existance of field
                    var col = FindFirstColumn(Session.SystemCode, "GLJrnDtl_UD", GLJrnDtlField.Key);
                    if (col == null)
                    {
                        this.UDFieldsDefinitionMessages.Add(new UDFieldDiagnosticMessage { level = System.Diagnostics.TraceLevel.Error, message = Strings.ThereIsNoFieldInTable(GLJrnDtlField.Key, GLJrnDtl.GetTableName()) });
                        udFieldCacheData.ErrorExists = true;
                        return;
                    }

                    udFieldCacheData.Column = col;

                    CompareUDFieldDefinitions(udFieldCacheData, "RvJrnTrDtl");

                    // Check GLJrnDtlMnl structure
                    if (this.TransactionType.KeyEquals("SingleGLJrn") || this.TransactionType.KeyEquals("MultiGLJrn"))
                    {
                        CompareUDFieldDefinitions(udFieldCacheData, "GLJrnDtlMnl");
                    }

                    CacheUDFieldsChecked.Add("GLJrnDtl." + udFieldCacheData.Key, udFieldCacheData);
                }
            }
        }

        private void CompareUDFieldDefinitions(UDFieldCacheData originalFieldData, string compareToTable)
        {
            string key = originalFieldData.Key;
            string compareToTableStr = compareToTable + "_UD";
            var compareToField = (from row in Db.ZDataField
                                  where row.SystemCode == "Erp" && row.DataTableID == compareToTableStr && row.DBFieldName == key
                                  select new UDFieldCacheData { TableName = compareToTable, Key = row.FieldName, DataType = row.DataType, Format = row.FieldFormat, InitialValue = row.InitialValue, Required = row.Required }).FirstOrDefault();
            if (compareToField == null)
            {
                this.UDFieldsDefinitionMessages.Add(new UDFieldDiagnosticMessage { level = System.Diagnostics.TraceLevel.Error, message = Strings.NoUDFieldInTable(compareToTable, key) });
                originalFieldData.ErrorExists = true;
                return;
            }

            var compareToFieldCol = FindFirstColumn("ERP", compareToTableStr, originalFieldData.Key);
            if (compareToFieldCol == null)
            {
                this.UDFieldsDefinitionMessages.Add(new UDFieldDiagnosticMessage { level = System.Diagnostics.TraceLevel.Error, message = Strings.ThereIsNoFieldInTable(originalFieldData.Key, compareToTable) });
                originalFieldData.ErrorExists = true;
            }
            compareToField.Column = compareToFieldCol;

            if (compareToField != null && compareToFieldCol != null)
            {
                // Check compatibility of data types and formats
                if (!originalFieldData.IsValid() || !compareToField.IsValid() || originalFieldData.DataType.Compare(compareToField.DataType) != 0 || originalFieldData.Format.Compare(compareToField.Format) != 0 ||
                        originalFieldData.Column.DataType.Compare(compareToFieldCol.DataType) != 0 || originalFieldData.Column.MaxLength.CompareTo(compareToFieldCol.MaxLength) != 0)
                {
                    this.UDFieldsDefinitionMessages.Add(new UDFieldDiagnosticMessage { level = System.Diagnostics.TraceLevel.Error, message = Strings.IncorrectDataTypes(key, FormatDataTypeStr(originalFieldData.Column), originalFieldData.Format, FormatDataTypeStr(compareToFieldCol), compareToField.Format) });
                    originalFieldData.ErrorExists = true;
                }

                if (!originalFieldData.Required.Equals(compareToField.Required) || (!originalFieldData.InitialValue.ToString().KeyEquals(compareToField.InitialValue.ToString())))
                {
                    this.UDFieldsDefinitionMessages.Add(new UDFieldDiagnosticMessage { level = System.Diagnostics.TraceLevel.Warning, message = Strings.ThereAreDifferentSettingsRequiredInitialValue(originalFieldData.TableName, key, compareToTable) });
                }
            }
        }

        private string FormatDataTypeStr(Column col)
        {
            if (SymbolsTypes.Contains(col.DataType, StringComparer.OrdinalIgnoreCase))
                return col.DataType + "(" + col.MaxLength + ")";
            else
                return col.DataType;
        }

        public bool AssignUDFieldToLine(string key, object value, procRvJrnTrDtl line)
        {
            UDFieldCacheData udFieldCacheData = null;

            if (!CacheUDFieldsChecked.TryGetValue("GLJrnDtl." + key, out udFieldCacheData))
            {
                this.UDFieldsDefinitionMessages.Add(new UDFieldDiagnosticMessage { level = System.Diagnostics.TraceLevel.Error, message = Strings.ThereIsNoFieldInTable(key, Erp.Tables.GLJrnDtl.GetTableName()) });
                return false;
            }

            if (udFieldCacheData != null)
            {
                try
                {
                    if (value != null)
                    {
                        switch (udFieldCacheData.DataType.ToUpperInvariant())
                        {
                            case "NVARCHAR":
                                string val = value.ToString();

                                // Check length of string value and cut it if necessary.
                                if (val.Length > udFieldCacheData.Length)
                                {
                                    val = val.Substring(0, udFieldCacheData.Length);
                                    var warning = String.Format(Strings.DataWasTrancatedForStringUDField(line.JournalLine, value, udFieldCacheData.Key, udFieldCacheData.DataType, udFieldCacheData.Format));
                                    this.UDFieldsDataMessages.Add(new UDFieldDiagnosticMessage { level = System.Diagnostics.TraceLevel.Warning, message = warning });
                                }

                                line[udFieldCacheData.Key] = val;

                                break;

                            case "INT":
                                line[udFieldCacheData.Key] = Compatibility.Convert.ToInt32(value.ToString());
                                break;
                            case "BIGINT":
                                line[udFieldCacheData.Key] = Compatibility.Convert.ToInt64(value.ToString());
                                break;
                            default:
                                line[udFieldCacheData.Key] = Convert.ChangeType(value, value.GetType());
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.UDFieldsDataMessages.Add(new UDFieldDiagnosticMessage { level = System.Diagnostics.TraceLevel.Error, message = String.Format(Strings.IncorrectDataForUDField(line.JournalLine, value, udFieldCacheData.Key, udFieldCacheData.DataType, ex.Message)) });
                    return false;
                }

                return true;
            }

            return false;
        }
        public void CheckUDForRestrictedTables()
        {

            // Check existance of UD field in TranGLC table
            var udTables = (from row in Db.ZDataTable
                            where row.SystemCode == "Erp" &&
                            (row.DataTableID == "AbtWork_UD" || row.DataTableID == "RvLock_UD")
                            select new UDFieldCacheData { TableName = row.DataTableID });

            foreach (var udTable in udTables)
            {
                this.UDFieldsDefinitionMessages.Add(new UDFieldDiagnosticMessage { level = System.Diagnostics.TraceLevel.Error, message = Strings.RestrictedTable(udTable.TableName) });
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.CacheUDFieldsChecked != null)
                {
                    this.CacheUDFieldsChecked.Clear();
                    this.CacheUDFieldsChecked = null;
                }

                if (UDFieldsDataMessages != null)
                {
                    this.UDFieldsDataMessages.Clear();
                    this.UDFieldsDataMessages = null;
                }

                if (UDFieldsDefinitionMessages != null)
                {
                    this.UDFieldsDefinitionMessages.Clear();
                    this.UDFieldsDefinitionMessages = null;
                }
            }
            base.Dispose(disposing);
        }

        #region Column Queries

        private static Func<ErpContext, string, string, string, Column> findFirstColumnQuery;

        private Column FindFirstColumn(string schemaName, string tableName, string columnName)
        {
            if (findFirstColumnQuery == null)
            {
                Expression<Func<ErpContext, string, string, string, Column>> expression =
                    (ctx, schemaName_ex, tableName_ex, columnName_ex) =>
                    (from columnRow in ctx.Column
                     join tableRow in ctx.Table on new { columnRow.TableID } equals new { tableRow.TableID }
                     where tableRow.SchemaName == schemaName_ex &&
                     tableRow.Name == tableName_ex &&
                     columnRow.Name == columnName_ex
                     select columnRow).FirstOrDefault();
                findFirstColumnQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstColumnQuery(this.Db, schemaName, tableName, columnName);
        }

        #endregion Column Queries
    }
}