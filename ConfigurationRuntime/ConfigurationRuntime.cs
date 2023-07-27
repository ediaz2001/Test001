using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using Epicor.Data;
using Epicor.Hosting;
using Epicor.Security.Cryptography;
using Epicor.Utilities;
using Erp.Internal.Lib;
using Erp.Internal.PC;
using Erp.Internal.PC.Configuration;
using Erp.Internal.PC.Configuration.RuleEngine;
using Erp.Internal.PC.TableCache;
using Erp.Internal.PC.TestRules;
using Erp.Services.Lib.Resources;
using Erp.Shared.Lib.Configurator;
using Erp.Shared.Lib.ERPExceptions;
using Erp.Tables;
using Erp.Tablesets;
using Ice;
using Ice.Assemblies;
using Ice.Common;
using Ice.Lib;
using Ice.Proxy.Lib;
using Ice.Security.Token;
using Ice.TableCache;
using Ice.Tablesets;
using Ice.Utility;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Strings = Erp.BO.ConfigurationRuntime.Resources.Strings;

namespace Erp.Services.BO
{



    /*
     * IMPORTANT - Public methods on this class must be of a type that is supported by SOAP for EWA.
     * Please see http://support.microsoft.com/default.aspx?scid=kb;en-us;326791 for details.
     * */
    /// <summary>
    ///
    /// </summary>
    public partial class ConfigurationRuntimeSvc
    {
        private Lazy<Erp.Internal.SI.EC.ExportCreatedPart> siECExportCreatedPart;
        private Erp.Internal.SI.EC.ExportCreatedPart SIECExportCreatedPart { get { return siECExportCreatedPart.Value; } }

        private Lazy<Erp.Internal.SI.EC.ExportMfgSIValueConfig> _SIExportMfgSIValueConfig;
        private Erp.Internal.SI.EC.ExportMfgSIValueConfig SIECExportMfgSIValueConfig { get { return this._SIExportMfgSIValueConfig.Value; } }

        private Lazy<Erp.Internal.PC.ConfigurationResolver> _pcConfiguratorResolver;
        private ConfigurationResolver PCConfiguratorResolver
        {
            get
            {
                return _pcConfiguratorResolver.Value;
            }
        }

        private Lazy<Erp.Internal.PC.RepriceConfig> _pcRepriceConfig;
        private RepriceConfig PCRepriceConfig
        {
            get
            {
                return _pcRepriceConfig.Value;
            }
        }

        private Lazy<Erp.Internal.PC.GenerateMethods> pcGenerateMethods;
        private Erp.Internal.PC.GenerateMethods PCGenerateMethods
        {
            get { return pcGenerateMethods.Value; }
        }

        private Lazy<Erp.Internal.Lib.RoundAmountEF> libRoundAmountEF;
        private Erp.Internal.Lib.RoundAmountEF LibRoundAbountEF
        {
            get { return libRoundAmountEF.Value; }
        }

        private Lazy<Erp.Internal.Lib.GetCurrencyRatesEF> libGetCurrencyRatesEF;
        private Erp.Internal.Lib.GetCurrencyRatesEF LibGetCurrencyRatesEF
        {
            get { return libGetCurrencyRatesEF.Value; }
        }

        private Lazy<Erp.Internal.PC.ConvertPCInValue> libConvertPCInValues;
        private Erp.Internal.PC.ConvertPCInValue LibConvertPCInValues { get { return libConvertPCInValues.Value; } }

        private Lazy<Erp.Internal.PC.PartCreation> libPartCreation;
        private Erp.Internal.PC.PartCreation LibPartCreation
        {
            get { return libPartCreation.Value; }
        }


        private LinqRow relatedEntity;

        private static System.Collections.Generic.List<Erp.Shared.Lib.Configurator.PCKeyValuePair<string, byte[]>> RuntimeImages = new System.Collections.Generic.List<Erp.Shared.Lib.Configurator.PCKeyValuePair<string, byte[]>>();

        /// <summary>
        /// Initialize
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            _pcConfiguratorResolver = new Lazy<Erp.Internal.PC.ConfigurationResolver>(() => new Erp.Internal.PC.ConfigurationResolver(Db));
            _pcRepriceConfig = new Lazy<Erp.Internal.PC.RepriceConfig>(() => new Erp.Internal.PC.RepriceConfig(Db));
            libConvertPCInValues = new Lazy<ConvertPCInValue>(() => new Erp.Internal.PC.ConvertPCInValue(Db));
            siECExportCreatedPart = new Lazy<Erp.Internal.SI.EC.ExportCreatedPart>(() => new Erp.Internal.SI.EC.ExportCreatedPart(Db));
            pcGenerateMethods = new Lazy<GenerateMethods>(() => new Erp.Internal.PC.GenerateMethods(Db));
            libRoundAmountEF = new Lazy<RoundAmountEF>(() => new Erp.Internal.Lib.RoundAmountEF(Db));
            libGetCurrencyRatesEF = new Lazy<GetCurrencyRatesEF>(() => new Erp.Internal.Lib.GetCurrencyRatesEF(Db));
            _SIExportMfgSIValueConfig = new Lazy<Internal.SI.EC.ExportMfgSIValueConfig>(() => new Internal.SI.EC.ExportMfgSIValueConfig(Db));
            libPartCreation = new Lazy<PartCreation>(() => new Internal.PC.PartCreation(Db));

        }

        /// <summary>
        /// Returns the generated source code to compile the client code of a configuration
        /// </summary>
        /// <param name="configID">Target configuration to generate, note that company is taken from session.</param>
        /// <param name="TestID">When executing under a test (Test Inputs/Rules) this parameter should contain a valid <see cref="System.Guid"/>, otherwise it can be <see cref="Guid.Empty"/></param>
        /// <param name="IsTestPlan">True when configuring inspection plan configuration</param>
        /// <param name="SpecID">Specification ID of inspection plan configuration</param>
        /// <param name="SpecRevNum">Specification Revision of inspection plan configuration</param>
        /// <param name="clientCheckSyntaxArgs"></param>
        /// <returns>An array of compressed source code strings.</returns>
        public byte[] GetGeneratedClient(string configID, Guid TestID, bool IsTestPlan, string SpecID, string SpecRevNum, Erp.Shared.Lib.Configurator.ClientCheckSyntaxArgs clientCheckSyntaxArgs = null)
        {
            Dictionary<string, string> clientSource = PCConfiguratorResolver.GetClientSources(Session.CompanyID, configID, TestID, IsTestPlan, SpecID, SpecRevNum, clientCheckSyntaxArgs);
            byte[] buffer = null;

            using (MemoryStream stm = new MemoryStream())
            {
                Serializer.Serialize(clientSource.Count, stm);

                foreach (string key in clientSource.Keys)
                {
                    Serializer.Serialize(key, stm);
                    Serializer.Serialize(ConfiguratorUtil.CompressString(clientSource[key]), stm);
                }

                stm.Flush();
                buffer = stm.GetBuffer();
            }

            return buffer;
        }


        /// <summary>
        /// Perform any logic that needs to be executed before starting a configuration
        /// </summary>
        /// <param name="configurationRuntimeDS">If PcStatus.DispConfSummary is enabled then the existing configuration values will be returned, otherwise only the PcValueGrp and top PcValueHead records are returned.</param>
        ///  <param name="configurationSummaryTS">summary data set</param>
        /// <returns>ConfigurationSequence calculated for the given configuration id, it contains all sequences of subconfigurations as well.</returns>
        ///
        public ConfigurationSequenceTableset PreStartConfiguration(ref ConfigurationRuntimeTableset configurationRuntimeDS, ref ConfigurationSummaryTableset configurationSummaryTS)
        {
            ttPcConfigurationParams = (from row in configurationRuntimeDS.PcConfigurationParams
                                       where !String.IsNullOrEmpty(row.RowMod)
                                       select row).FirstOrDefault();

            if (ttPcConfigurationParams == null)
                throw new BLException(Strings.PcConfigurationNoParams);

            ConfigurationSequenceTableset configSequenceTS = ValidateAndBuildConfigurationData(configurationRuntimeDS, ref configurationSummaryTS, true);
            // Check for EWC configurator, no processing is required for this type at this time.

            if (ttPcConfigurationParams.ConfigType.KeyEquals("EWC")) return null;
            // End check for EWC.

            var pcValueGrpRow = (from row in configurationRuntimeDS.PcValueGrp select row).FirstOrDefault();
            if (pcValueGrpRow == null)
                throw new BLException(Strings.PcValueGrpNotFound);

            if (pcValueGrpRow != null && (pcValueGrpRow.DisplaySummary || ttPcConfigurationParams.TestMode.Equals("Tracker", StringComparison.OrdinalIgnoreCase)) && pcValueGrpRow.GroupSeq > 0)
            {
                configurationSummaryTS = new ConfigurationSummaryTableset();
                // scr 141830 states the summary option is only available for Order line, quote line, demand line and PO line.
                switch (ttPcConfigurationParams.RelatedToTable.ToUpperInvariant())
                {
                    case "ORDERDTL":
                        getOrderSummary(ttPcConfigurationParams.RelatedToSysRowID, configurationSummaryTS);
                        break;
                    case "QUOTEDTL":
                        getQuoteSummary(ttPcConfigurationParams.RelatedToSysRowID, configurationSummaryTS);
                        break;
                    case "PODETAIL":
                        getPOSummary(ttPcConfigurationParams.RelatedToSysRowID, configurationSummaryTS);
                        break;
                    case "DEMANDDETAIL":
                        getDemandSummary(ttPcConfigurationParams.RelatedToSysRowID, configurationSummaryTS);
                        break;
                }
            }
            return configSequenceTS;
        }



        private ConfigurationSequenceTableset ValidateAndBuildConfigurationData(ConfigurationRuntimeTableset configurationRuntimeDS, ref ConfigurationSummaryTableset configurationSummaryTS, bool calledFromWinClient = true)
        {
            string validSummaryTargets = "ORDERDTL,QUOTEDTL,PODETAIL,DEMANDDETAIL";


            ttPcConfigurationParams = (from row in configurationRuntimeDS.PcConfigurationParams
                                       where !String.IsNullOrEmpty(row.RowMod)
                                       select row).FirstOrDefault();

            if (ttPcConfigurationParams == null)
                throw new BLException(Strings.PcConfigurationNoParams);

            if (ttPcConfigurationParams.ConfigType.KeyEquals("EWC") && ttPcConfigurationParams.TestID != Guid.Empty && ttPcConfigurationParams.TestMode.KeyEquals("RULES"))
            {
                ttPcConfigurationParams.SourceTable = "PartRev";
            }

            LinqRow foreignRow = null;
            LinqRow relatedToRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcConfigurationParams.RelatedToTable, ttPcConfigurationParams.RelatedToSysRowID, LockHint.UpdLock);
            if (relatedToRow == null)
                throw new BLException(Strings.RelatedToRowNotFound(ttPcConfigurationParams.RelatedToTable, ttPcConfigurationParams.RelatedToSysRowID));

            // retrieve the sourceRow when not configuring an inspection plan.  For inspection plans, the sourceRow and relatedToRow are the same
            LinqRow sourceRow = null;
            if (ttPcConfigurationParams.RelatedToTable.Equals("InspResults", StringComparison.OrdinalIgnoreCase))
            {
                sourceRow = relatedToRow;
            }
            else
            {
                if (ttPcConfigurationParams.SourceTable.KeyEquals("PartRev"))
                {

                    sourceRow = this.FindFirstPartRevAlt(ttPcConfigurationParams.Company, ttPcConfigurationParams.PartNum, ttPcConfigurationParams.RevisionNum, ttPcConfigurationParams.AltMethod);
                }
                else if (String.Compare(ttPcConfigurationParams.SourceTable, "PcStatus", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    sourceRow = this.FindFirstPcStatus(ttPcConfigurationParams.Company, ttPcConfigurationParams.ConfigID);
                }
                else if (String.IsNullOrEmpty(ttPcConfigurationParams.SourceTable) && IsEWC(Session.CompanyID, ttPcConfigurationParams.ConfigID))
                {
                    if (ttPcConfigurationParams.TestID == Guid.Empty)
                    {
                        if (string.IsNullOrEmpty(ttPcConfigurationParams.PartNum))
                            ttPcConfigurationParams.PartNum = relatedToRow["PartNum"].ToString();
                        if (string.IsNullOrEmpty(ttPcConfigurationParams.RevisionNum))
                            ttPcConfigurationParams.RevisionNum = relatedToRow["RevisionNum"].ToString();
                        sourceRow = this.FindFirstPartRevAlt(ttPcConfigurationParams.Company, ttPcConfigurationParams.PartNum, ttPcConfigurationParams.RevisionNum, ttPcConfigurationParams.AltMethod);
                    }
                    else
                        sourceRow = this.FindFirstPcStatus(ttPcConfigurationParams.Company, ttPcConfigurationParams.ConfigID);
                }
            }
            if (sourceRow == null)
                throw new BLException(Strings.SourceRowIsNull);

            if (!String.IsNullOrWhiteSpace(ttPcConfigurationParams.ForeignTableName) && !ttPcConfigurationParams.ForeignSysRowID.Equals(Guid.Empty))
                foreignRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcConfigurationParams.ForeignTableName, ttPcConfigurationParams.ForeignSysRowID, LockHint.UpdLock);
            int groupSeq = 0;
            if (foreignRow != null)
            {
                groupSeq = ConfiguratorUtil.ResolveGroupSeq(this.Db, foreignRow);
            }
            else groupSeq = ConfiguratorUtil.ResolveGroupSeq(this.Db, relatedToRow);
            string configID = "";
            string curPartNum = "";
            string curRevisionNum = "";
            string curRelatedToTable = ttPcConfigurationParams.RelatedToTable;
            PcStatusResult PcStatus = null;

            if (!ttPcConfigurationParams.IsTestPlan)
            {
                configID = sourceRow["ConfigID"].ToString();
                // if groupSeq > 0 then we know we are in a reconfigure scenarios.  If the configID is null then we are reconfiguring a part that was created from a configuration that has 'create non configured part' = true;
                // We need to go back to to the sourceRow and determine if we have a valid basePart/Rev and use that to determine the configID assinged to that PartRev row.
                if (String.IsNullOrEmpty(configID) && Convert.ToInt32(relatedToRow["GroupSeq"].ToString()) > 0)
                {

                    PartRev altsourceRow = this.FindFirstPartRevAlt(ttPcConfigurationParams.Company, relatedToRow["BasePartNum"].ToString(), relatedToRow["BaseRevisionNum"].ToString(), "");
                    if (altsourceRow != null && !String.IsNullOrEmpty(altsourceRow.ConfigID))
                        configID = altsourceRow.ConfigID;

                }
                if (!String.IsNullOrEmpty(configID))
                {
                    PcStatus = FindFirstPcStatusResult(Session.CompanyID, configID);
                    if (PcStatus == null)
                        throw new BLException(Strings.PcStatusNotFound(configID));
                    if (!PcStatus.Approved && ttPcConfigurationParams.TestID.Equals(Guid.Empty))
                        throw new BLException(Strings.PcStatusNotApproved(configID));
                }
                // Configuration runtime has to call this method to identify if we are working with an Epicor Mobile configuator.  If we are there is no need to
                // proceed with this logic since EWC will call a different method to build everything they need in order to simulate the Win Client run time engine.
                // Intentionally coded this way to avoid an additional round trip out of ConfigurationRuntime UI and to avoid adding ConfigType to every Target Entity.  Although that
                // could be done, it is better to consolidate the logic in one place.
                if (PcStatus != null)
                {
                    ttPcConfigurationParams.ConfigType = PcStatus.ConfigType;
                    if (calledFromWinClient && PcStatus.ConfigType.KeyEquals("EWC")) return null;

                    if (ttPcConfigurationParams.ConfigType.KeyEquals("KB"))
                    {
                        throw new DataValidationException(Strings.KBMaxConfiguratorTypeCannotBeConfigured);
                    }
                }

                bool useBase = false;
                if (foreignRow == null)
                {
                    // if the E9 Partrev was not properly updated with basepartnum and configured then we could be attempting to run a configuration on demand so try using the values
                    // on the related to row.
                    if (groupSeq == 0 && String.IsNullOrEmpty(configID) && sourceRow.EntitySetName.Equals("PartRev", StringComparison.OrdinalIgnoreCase) && !Convert.ToBoolean(sourceRow["Configured"].ToString())
                        && (!String.IsNullOrEmpty(relatedToRow["BasePartNum"].ToString()) && !relatedToRow["PartNum"].ToString().KeyEquals(relatedToRow["BasePartNum"].ToString())))
                    {
                        curPartNum = relatedToRow["BasePartNum"].ToString();
                        curRevisionNum = relatedToRow["BaseRevisionNum"].ToString();
                        configID = relatedToRow["BasePartNum"].ToString() + "_" + relatedToRow["BaseRevisionNum"].ToString();
                    }
                    else
                    {
                        useBase = sourceRow.EntitySetName.Equals("PartRev", StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(sourceRow["BasePartNum"].ToString()) && ExistsEqualBaseConfigID(Session.CompanyID, sourceRow["BasePartNum"].ToString(), sourceRow["BaseRevisionNum"].ToString(), configID)
                                  && ((!PcStatus.GenerateMethod && !PcStatus.CreateRev) || groupSeq != 0);
                        if (useBase)
                        {
                            curPartNum = sourceRow["BasePartNum"].ToString();
                            curRevisionNum = sourceRow["BaseRevisionNum"].ToString();
                        }
                        else
                        {
                            curPartNum = sourceRow["PartNum"].ToString();
                            curRevisionNum = sourceRow["RevisionNum"].ToString();
                        }
                    }
                }
                else
                {
                    useBase = sourceRow.EntitySetName.Equals("PartRev", StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(sourceRow["BasePartNum"].ToString()) && ExistsEqualBaseConfigID(Session.CompanyID, sourceRow["BasePartNum"].ToString(), sourceRow["BaseRevisionNum"].ToString(), configID)
                                   && ((!PcStatus.GenerateMethod && !PcStatus.CreateRev) || groupSeq != 0);
                    if (useBase)
                    {
                        curPartNum = sourceRow["BasePartNum"].ToString();
                        curRevisionNum = sourceRow["BaseRevisionNum"].ToString();
                    }
                    else
                    {

                        curPartNum = sourceRow["PartNum"].ToString();
                        curRevisionNum = sourceRow["RevisionNum"].ToString();
                    }
                }
            }
            else
            {
                if (ttPcConfigurationParams.RelatedToTable.Equals("InspResults", StringComparison.OrdinalIgnoreCase))
                {
                    InspResultsCols InspResults = this.FindFirstInspResults(ttPcConfigurationParams.RelatedToSysRowID);
                    if (InspResults != null)
                    {
                        InspPlanRevCols InspPlanRev = FindInspPlanRev(Session.CompanyID, InspResults.InspPlanPartNum, InspResults.InspPlanRevNum);
                        if (InspPlanRev != null)
                        {
                            configID = InspPlanRev.ConfigID;
                        }
                    }

                    curPartNum = sourceRow["InspPlanPartNum"].ToString();
                    curRevisionNum = sourceRow["InspPlanRevNum"].ToString();
                    curRelatedToTable = ttPcConfigurationParams.InspType;

                }
                else if (ttPcConfigurationParams.RelatedToTable.Equals("PcStatus", StringComparison.OrdinalIgnoreCase))
                {
                    configID = sourceRow["ConfigID"].ToString();
                }


            }
            if (PcStatus != null)
            {
                if (ttPcConfigurationParams.IsTestPlan || !PcStatus.ConfigID.KeyEquals(configID))
                    PcStatus = FindFirstPcStatusResult(Session.CompanyID, configID);
            }
            else if (!ttPcConfigurationParams.IsTestPlan)
            {
                if (foreignRow == null)
                {
                    if (!String.IsNullOrEmpty(relatedToRow["BasePartNum"].ToString()))
                    {
                        curPartNum = relatedToRow["BasePartNum"].ToString();
                        curRevisionNum = relatedToRow["BaseRevisionNum"].ToString();
                    }
                    else
                    {

                        curPartNum = relatedToRow["PartNum"].ToString();
                        curRevisionNum = relatedToRow["RevisionNum"].ToString();
                    }

                }
                else
                {
                    if (!String.IsNullOrEmpty(foreignRow["BasePartNum"].ToString()))
                    {
                        curPartNum = foreignRow["BasePartNum"].ToString();
                        curRevisionNum = foreignRow["BaseRevisionNum"].ToString();
                    }
                    else
                    {

                        curPartNum = foreignRow["PartNum"].ToString();
                        curRevisionNum = foreignRow["RevisionNum"].ToString();
                    }

                }

                PartRev altsourceRow = this.FindFirstPartRevAlt(ttPcConfigurationParams.Company, curPartNum, curRevisionNum, "");
                if (altsourceRow != null && !String.IsNullOrEmpty(altsourceRow.ConfigID))
                    configID = altsourceRow.ConfigID;

                PcStatus = FindFirstPcStatusResult(Session.CompanyID, configID);
            }

            // 114351 since we only have PcStatus rows for config-to-config structures it is not an error if we do not have one.  For example when a non config part is sold and it has a config componenent, there
            // will not be a pc status in this scenario.
            //if (PcStatus == null)
            //    ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotFound(configID)));
            if (PcStatus != null)
            {
                if (!PcStatus.ConfigType.KeyEquals("NIC"))
                {
                    if (!ExistsPcInputs(Session.CompanyID, configID))
                        ExceptionManager.AddBLException(new BLException(Strings.ConfiguratorHasNoInputs));
                }
                if (ttPcConfigurationParams.TestMode.KeyEquals("EWADEPLOY"))
                {
                    if (PcStatus.ConfigType.KeyEquals("EWC"))
                        ExceptionManager.AddBLException(new BLException(Strings.CannotDeployEWC));
                    if (ExistsPcInputLayeredImage(PcStatus.Company, configID, "EpiPCLayeredImage"))
                        ExceptionManager.AddBLException(new BLException(Strings.CannotDeployLayeredImage));
                }
                if (!PcStatus.Approved && ttPcConfigurationParams.TestID.Equals(Guid.Empty))
                {
                    if (ttPcConfigurationParams.TestMode.KeyEquals("Tracker"))
                        ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotApprovedTracker(configID)));
                    else
                        ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotApproved(configID)));
                }
            }
            else
                ttPcConfigurationParams.RowMod = IceRow.ROWSTATE_UNCHANGED;

            PcTargetEntityCols PcTargetEntity = null;
            PcValueGrp PcValueGrp = null;

            // scr 114351 if the configID is null we are processing a parent that is not configurable but it has a configurable componenent.
            if (ttPcConfigurationParams.TestID.Equals(Guid.Empty) && !String.IsNullOrEmpty(configID) && !ttPcConfigurationParams.TestMode.Equals("EWADEPLOY", StringComparison.OrdinalIgnoreCase))
            {
                PcTargetEntity = this.FindPcTargetEntity(Session.CompanyID, configID, curRelatedToTable);
                if (PcTargetEntity == null)
                    ExceptionManager.AddBLException(new BLException(Strings.PcTargetEntityNotFound(curRelatedToTable, configID)));
            }// 114351 nzirbes moved the pcvaluegrp outside of this if because when our top assembly does not have a configID we still need to retrieve saved values if they exist.

            //Get existing configuration data (not values just top level records) those contain certain parameters that the client needs
            if (groupSeq > 0)
                PcValueGrp = this.FindFirstPcValueGrp(Session.CompanyID, groupSeq);
            ExceptionManager.AssertNoBLExceptions();
            //configurationRuntimeDS = new ConfigurationRuntimeTableset();

            //Calculate Configuration Sequence
            ConfigurationSequenceTableset configurationSequenceDS = new ConfigurationSequenceTableset();
            if (ttPcConfigurationParams.TestMode.Equals("RULES", StringComparison.OrdinalIgnoreCase))
            {
                string testRulesMessage = string.Empty;
                ConfiguratorUtil.GetConfigurationSequence(Db, Session.CompanyID, configID, curPartNum, curRevisionNum, configurationSequenceDS.PcStruct, ttPcConfigurationParams.RelatedToTable, ttPcConfigurationParams.IsTestPlan, ttPcConfigurationParams.SpecID, ttPcConfigurationParams.InspType, true, ref testRulesMessage);
            }
            else if (ttPcConfigurationParams.TestMode.Equals("TRACKER", StringComparison.OrdinalIgnoreCase))
            {
                ConfiguratorUtil.GetConfigurationSequenceTracker(Db, Session.CompanyID, groupSeq, configurationSequenceDS.PcStruct, ttPcConfigurationParams.RelatedToTable);
            }
            else
            {
                if (!String.IsNullOrEmpty(ttPcConfigurationParams.InitialStructTag) && groupSeq > 0)
                {
                    ConfiguratorUtil.GetConfigurationCurrentSequence(Db, Session.CompanyID, ttPcConfigurationParams.RelatedToTable, ttPcConfigurationParams.InitialStructTag, ttPcConfigurationParams.InitialRuleTag, groupSeq, configurationSequenceDS.PcStruct);
                }
                else
                {
                    ConfiguratorUtil.GetConfigurationSequence(Db, Session.CompanyID, configID, curPartNum, curRevisionNum, configurationSequenceDS.PcStruct, ttPcConfigurationParams.RelatedToTable, ttPcConfigurationParams.IsTestPlan, ttPcConfigurationParams.SpecID, ttPcConfigurationParams.InspType);
                }
            }
            // On Demand conversion of PcInValue logic
            if (ttPcConfigurationParams.TestID.Equals(Guid.Empty) && groupSeq == 0 && configurationSequenceDS.PcStruct.Any())
            {
                var pcStructRow = configurationSequenceDS.PcStruct[0];  // always start with the first row - prestart is only called one time
                if (ttPcConfigurationParams.SourceTable.Equals("PartRev", StringComparison.OrdinalIgnoreCase) && ExecuteOnDemandConversion(relatedToRow))
                {
                    LibConvertPCInValues.CreatePcValueSet(ref groupSeq, relatedToRow, foreignRow,
                         pcStructRow.ConfigID, ref configurationSequenceDS);
                    if (groupSeq != 0)
                    {
                        //// executing the check on value set to ensure the values were created
                        if (!this.ExistsPcValueSet(Session.CompanyID, groupSeq))
                        {
                            string relatedRowAndKeys = ConfiguratorUtil.FormatRowKey(relatedToRow);
                            string sourceRowAndKeys = ConfiguratorUtil.FormatRowKey(sourceRow);
                            throw new BLException(Strings.NoSavedPcValueSetFound(pcStructRow.ConfigID, relatedRowAndKeys, sourceRowAndKeys));
                        }
                        PcValueGrp = this.FindFirstPcValueGrp(Session.CompanyID, groupSeq);  // must retrieve this data so the run time dataset is created properly
                    }
                }
            }


            if (ttPcConfigurationParams.TestID.Equals(Guid.Empty) && PcValueGrp != null)
            {
                //If display summary is enabled we get the whole configuration dataset. Note ttPcInputValue is generated in AfterGetRows
                // scr 114351 if (PcStatus.DispConfSummary)
                //configurationRuntimeDS = this.GetByID(PcValueGrp.GroupSeq);

                //There are values for this entity and configuration already
                ttPcValueGrp = new PcValueGrpRow();
                BufferCopy.Copy(PcValueGrp, ttPcValueGrp);
                if (PcTargetEntity != null)
                    ttPcValueGrp.IncomingSmartString = PcTargetEntity.IncomingSmartString;
                if (PcStatus != null)
                    ttPcValueGrp.DisplaySummary = (PcStatus.DispConfSummary && (validSummaryTargets.IndexOf(ttPcConfigurationParams.RelatedToTable, StringComparison.OrdinalIgnoreCase) >= 0));
                ttPcValueGrp.RowMod = IceRow.ROWSTATE_UPDATED;
                configurationRuntimeDS.PcValueGrp.Add(ttPcValueGrp);

                //Just get the first (top) configuration for reference
                PcValueHead PcValueHead = this.FindPcValueHeadByStructTag(Session.CompanyID,
                    ttPcValueGrp.GroupSeq,
                    "/0",
                    configID);
                if (PcValueHead != null)
                {
                    ttPcValueHead = new PcValueHeadRow();
                    BufferCopy.Copy(PcValueHead, ttPcValueHead);
                    configurationRuntimeDS.PcValueHead.Add(ttPcValueHead);
                }
            }
            else
            {
                //It is a brand new configuration or a Test
                ttPcValueGrp = new PcValueGrpRow();
                ttPcValueGrp.Company = Session.CompanyID;
                ttPcValueGrp.GroupSeq = 0;
                ttPcValueGrp.RelatedToTableName = relatedToRow.EntitySetName;
                ttPcValueGrp.RelatedToSysRowID = relatedToRow.SysRowID;
                ttPcValueGrp.ConfigStatus = (ttPcConfigurationParams.TestID.Equals(Guid.Empty) ? "Added" : "Test");
                ttPcValueGrp.TestID = ttPcConfigurationParams.TestID;

                ttPcValueGrp.TestMode = ttPcConfigurationParams.TestMode;
                ttPcValueGrp.SIValues = (PcStatus != null ? PcStatus.SaveInputValues : false);
                if (PcTargetEntity != null)
                    ttPcValueGrp.IncomingSmartString = PcTargetEntity.IncomingSmartString;
                ttPcValueGrp.RowMod = IceRow.ROWSTATE_ADDED;
                configurationRuntimeDS.PcValueGrp.Add(ttPcValueGrp);
            }


            //Any PcStruct row that does not have a source table name means it comes from a Generic configuration, these will have the same
            //source information as the parent.
            foreach (var pcStructRow in configurationSequenceDS.PcStruct)
            {
                if (string.IsNullOrEmpty(pcStructRow.SourceTableName))
                {
                    pcStructRow.SourceTableName = sourceRow.EntitySetName;
                    pcStructRow.SourceSysRowID = sourceRow.SysRowID;
                }
            }
            if (PcStatus != null)
            {
                PopulatePcConfigSmartString(configurationSequenceDS, PcStatus);
                if (PcStatus.StringStyle.Equals("ConStr", StringComparison.OrdinalIgnoreCase))
                {
                    PcStrCompRow smartStringRow;
                    foreach (var PcStrCompRow in (this.FindSmartStringValues(Session.CompanyID, PcStatus.ConfigID)))
                    {
                        smartStringRow = new Tablesets.PcStrCompRow();
                        BufferCopy.Copy(PcStrCompRow, smartStringRow);
                        if (!PcStatus.Separator.Equals("N", StringComparison.OrdinalIgnoreCase))
                            smartStringRow.SmartStringSeparator = PcStatus.Separator;
                        configurationSequenceDS.PcStrComp.Add(smartStringRow);
                    }
                }
            }

            ExceptionManager.AssertNoBLExceptions();

            return configurationSequenceDS;
        }

        private void PopulatePcConfigSmartString(ConfigurationSequenceTableset configurationSequenceDS, PcStatusResult PcStatus)
        {
            PcConfigSmartStringRow PcConfigSmartString = new PcConfigSmartStringRow();
            PcConfigSmartString.Company = PcStatus.Company;
            PcConfigSmartString.ConfigID = PcStatus.ConfigID;
            PcConfigSmartString.StringStyle = PcStatus.StringStyle;
            PcConfigSmartString.PrefacePart = PcStatus.PrefacePart;
            PcConfigSmartString.CrtCustPart = PcStatus.CrtCustPart;
            PcConfigSmartString.Separator = PcStatus.Separator;
            PcConfigSmartString.NumberFormat = PcStatus.NumberFormat;
            PcConfigSmartString.StartNumber = PcStatus.StartNumber;
            configurationSequenceDS.PcConfigSmartString.Add(PcConfigSmartString);
        }
        private bool ExecuteOnDemandConversion(LinqRow RelatedToRow)
        {
            DateTime? ConversionRunDate = this.FindCnvProgs("ERP", "PCCONV905", "Complete");
            if (ConversionRunDate == null) return false;

            switch (RelatedToRow.EntitySetName.ToUpperInvariant())
            {
                case "ORDERDTL":
                    return IsTargetEntityDateEarlierThanConversionRunDate(((OrderDtl)RelatedToRow).LastConfigDate, ConversionRunDate);
                case "QUOTEDTL":
                    return IsTargetEntityDateEarlierThanConversionRunDate(((QuoteDtl)RelatedToRow).LastConfigDate, ConversionRunDate);
                case "JOBHEAD":
                    return IsTargetEntityDateEarlierThanConversionRunDate(((JobHead)RelatedToRow).CreateDate, ConversionRunDate);
                case "DEMANDDETAIL":
                    return true; // per product owner, there is no good date on Demand to use so we will always return true
                case "PODETAIL":
                    return IsTargetEntityDateEarlierThanConversionRunDate(this.FindPOOrderDate(Session.CompanyID, (((PODetail)RelatedToRow).PONUM)), ConversionRunDate);
            }
            return false;
        }

        internal static bool IsTargetEntityDateEarlierThanConversionRunDate(DateTime? TargetEntityDate, DateTime? ConversionRunDate)
        {
            if (TargetEntityDate == null || ConversionRunDate == null) return false;
            return (TargetEntityDate <= ConversionRunDate);
        }

        #region Configuration Summary logic
        private void buildSummaryValues(int groupSeq, int headNum, ConfigurationSummaryTableset configurationSummaryTS)
        {
            foreach (PcValueSet PcValueSet in this.GetPcValueSetRecords(Session.CompanyID, groupSeq, headNum))
            {
                PageSummaryInfo PageSummaryInfo = this.GetPageTitle(Session.CompanyID, PcValueSet.ConfigID, PcValueSet.PageSeq);
                using (StringReader summaryValues = new StringReader(PcValueSet.FieldValues))
                {
                    XElement xPcValues = XElement.Load(summaryValues);
                    if (xPcValues != null)
                    {
                        foreach (XElement xchild in xPcValues.Descendants())
                        {
                            if (String.IsNullOrEmpty(xchild.Name.LocalName))
                                continue;
                            PcInputNodeValues pcInputNodeValues = this.IsNodePcInputs(Session.CompanyID, PcValueSet.ConfigID, xchild.Name.LocalName, false);
                            if (pcInputNodeValues == null) continue;
                            ttPcInputValue = new PcInputValueRow();
                            BufferCopy.CopyUsing(PcValueSet, ttPcInputValue, "Company", "GroupSeq", "HeadNum", "PageSeq", "SubTableName", "ConfigID", "SysRowID");
                            ttPcInputValue.InputName = xchild.Name.LocalName;
                            if (xchild.FirstAttribute.Name.LocalName.Equals("Type", StringComparison.OrdinalIgnoreCase) && xchild.FirstAttribute.Value.Equals("System.Nullable`1[System.DateTime]", StringComparison.OrdinalIgnoreCase))
                            {
                                DateTime? cultureDate = null;
                                cultureDate = string.IsNullOrEmpty(xchild.Value.ToString()) ? cultureDate : DateTime.Parse(xchild.Value.ToString(), CultureInfo.InvariantCulture);
                                ttPcInputValue.InputValue = cultureDate == null ? string.Empty : cultureDate.Value.ToString("d", Session.FormatCulture);
                            }
                            else if (xchild.FirstAttribute.Name.LocalName.Equals("Type", StringComparison.OrdinalIgnoreCase) && xchild.FirstAttribute.Value.Equals("System.Decimal", StringComparison.OrdinalIgnoreCase))
                            {
                                var cultureDecimal = Decimal.Parse(xchild.Value.ToString(), CultureInfo.InvariantCulture);
                                ttPcInputValue.InputValue = cultureDecimal.ToString("N", Session.FormatCulture);
                            }
                            else
                            {
                                ttPcInputValue.InputValue = xchild.Value.ToString();
                            }
                            ttPcInputValue.SideLabel = pcInputNodeValues.SideLabel;
                            ttPcInputValue.SummaryLabel = pcInputNodeValues.SummaryLabel;
                            ttPcInputValue.NoDispSummary = pcInputNodeValues.NoDispSummary;
                            ttPcInputValue.TabOrder = pcInputNodeValues.TabOrder;
                            if (PageSummaryInfo != null)
                            {
                                ttPcInputValue.PageTitle = PageSummaryInfo.PageTitle;
                                ttPcInputValue.DspPageSeq = PageSummaryInfo.DisplaySeq;
                            }
                            configurationSummaryTS.PcInputValue.Add(ttPcInputValue);
                        }
                    }
                }
            }
        }


        private void getPOSummary(Guid poDetailSysRowID, ConfigurationSummaryTableset configurationSummaryTS)
        {
            PODetailCols PODetail = this.FindPODetailBySysRowID(Session.CompanyID, poDetailSysRowID);
            if (PODetail == null)
                throw new BLException(Strings.PODetailNotFound);
            string currencyCode = this.FindPOCurrency(Session.CompanyID, PODetail.PONum);
            if (String.IsNullOrEmpty(currencyCode))
                throw new BLException(Strings.POHeadNotFound);

            foreach (PcValueHead pcValueHead in this.SelectPcValueHeadForSummary(Session.CompanyID, PODetail.GroupSeq))
            {
                ConfigurationSummaryRow configurationSummaryRow = new ConfigurationSummaryRow();
                configurationSummaryTS.ConfigurationSummary.Add(configurationSummaryRow);
                configurationSummaryRow.Company = Session.CompanyID;
                configurationSummaryRow.ConfigID = pcValueHead.ConfigID;
                configurationSummaryRow.SmartString = pcValueHead.NewSmartString;
                configurationSummaryRow.RowMod = "A";
                configurationSummaryRow.GroupSeq = pcValueHead.GroupSeq;
                configurationSummaryRow.HeadNum = pcValueHead.HeadNum;

                // only want this information for the Top level configurator
                if (pcValueHead.StructTag.Equals("/0", StringComparison.OrdinalIgnoreCase))
                {
                    configurationSummaryRow.BasePartNum = PODetail.BasePartNum;
                    configurationSummaryRow.BaseRevisionNum = PODetail.BaseRevisionNum;


                    configurationSummaryRow.ConfigUnitPrice = PODetail.ConfigUnitPrice;
                    configurationSummaryRow.DocConfigUnitPrice = PODetail.ConfigUnitPrice;
                    configurationSummaryRow.ConfigBaseUnitPrice = PODetail.ConfigBaseUnitPrice;
                    configurationSummaryRow.DocConfigBaseUnitPrice = PODetail.ConfigBaseUnitPrice;
                    string pCurrList = string.Empty;
                    string pRateList = string.Empty;
                    List<CurrExChain> ttChainRows = new List<CurrExChain>();
                    LibGetCurrencyRatesEF.FindCurrencyRates("OrderHed", PODetail.PONum.ToString(), "", "", "", "", "", "", "", out ttChainRows, out pCurrList, out pRateList);

                    configurationSummaryRow.DocConfigUnitPrice = LibRoundAbountEF.ConvertAmtRoundDecimals(configurationSummaryRow.ConfigUnitPrice, currencyCode,
                        pCurrList.Entry(0, Ice.Constants.LIST_DELIM), ttChainRows, false, "ttConfigurationSummary", "DocConfigUnitPrice");

                    configurationSummaryRow.DocConfigBaseUnitPrice = LibRoundAbountEF.ConvertAmtRoundDecimals(configurationSummaryRow.ConfigBaseUnitPrice, currencyCode,
                   pCurrList.Entry(0, Ice.Constants.LIST_DELIM), ttChainRows, false, "ttConfigurationSummary", "DocConfigUnitPrice");

                    GetCurrencyInfo(configurationSummaryRow, currencyCode);
                }
                buildSummaryValues(pcValueHead.GroupSeq, pcValueHead.HeadNum, configurationSummaryTS);
            }
        }


        private void getDemandSummary(Guid demandDetailSysRowID, ConfigurationSummaryTableset configurationSummaryTS)
        {
            DemandDetailCols DemandDetail = this.FindFirstDemandDetailBySysRowID(Session.CompanyID, demandDetailSysRowID);
            if (DemandDetail == null)
                throw new BLException(Strings.InvalidDemandLine);
            string CurrencyCode = this.FindDemandHeadCurrencyCode(Session.CompanyID, DemandDetail.DemandHeadSeq);
            if (String.IsNullOrEmpty(CurrencyCode))
                throw new BLException(Strings.DemandHeadNotFound);

            foreach (PcValueHead pcValueHead in this.SelectPcValueHeadForSummary(Session.CompanyID, DemandDetail.GroupSeq))
            {
                ConfigurationSummaryRow configurationSummaryRow = new ConfigurationSummaryRow();
                configurationSummaryTS.ConfigurationSummary.Add(configurationSummaryRow);
                configurationSummaryRow.Company = Session.CompanyID;
                configurationSummaryRow.DemandContractNum = DemandDetail.DemandContractNum;
                configurationSummaryRow.DemandHeadSeq = DemandDetail.DemandHeadSeq;
                configurationSummaryRow.DemandDtlSeq = DemandDetail.DemandDtlSeq;
                configurationSummaryRow.ConfigID = pcValueHead.ConfigID;
                configurationSummaryRow.SmartString = pcValueHead.NewSmartString;
                configurationSummaryRow.RowMod = "A";
                configurationSummaryRow.GroupSeq = pcValueHead.GroupSeq;
                configurationSummaryRow.HeadNum = pcValueHead.HeadNum;

                // only want this information for the Top level configurator
                if (pcValueHead.StructTag.Equals("/0", StringComparison.OrdinalIgnoreCase))
                {
                    configurationSummaryRow.BasePartNum = DemandDetail.BasePartNum;
                    configurationSummaryRow.BaseRevisionNum = DemandDetail.BaseRevisionNum;


                    configurationSummaryRow.ConfigUnitPrice = DemandDetail.ConfigUnitPrice;
                    configurationSummaryRow.DocConfigUnitPrice = DemandDetail.ConfigUnitPrice;
                    configurationSummaryRow.ConfigBaseUnitPrice = DemandDetail.ConfigBaseUnitPrice;
                    configurationSummaryRow.DocConfigBaseUnitPrice = DemandDetail.ConfigBaseUnitPrice;
                    configurationSummaryRow.LastConfigDate = DemandDetail.LastConfigDate;
                    configurationSummaryRow.LastConfigTime = DemandDetail.LastConfigTime;
                    configurationSummaryRow.LastConfigUserID = DemandDetail.LastConfigUserID;
                    string pCurrList = string.Empty;
                    string pRateList = string.Empty;
                    List<CurrExChain> ttChainRows = new List<CurrExChain>();
                    LibGetCurrencyRatesEF.FindCurrencyRates("OrderHed", DemandDetail.DemandContractNum.ToString(), "", "", "", "", "", "", "", out ttChainRows, out pCurrList, out pRateList);

                    configurationSummaryRow.DocConfigUnitPrice = LibRoundAbountEF.ConvertAmtRoundDecimals(configurationSummaryRow.ConfigUnitPrice, CurrencyCode,
                        pCurrList.Entry(0, Ice.Constants.LIST_DELIM), ttChainRows, false, "ttConfigurationSummary", "DocConfigUnitPrice");

                    configurationSummaryRow.DocConfigBaseUnitPrice = LibRoundAbountEF.ConvertAmtRoundDecimals(configurationSummaryRow.ConfigBaseUnitPrice, CurrencyCode,
                   pCurrList.Entry(0, Ice.Constants.LIST_DELIM), ttChainRows, false, "ttConfigurationSummary", "DocConfigUnitPrice");

                    GetCurrencyInfo(configurationSummaryRow, CurrencyCode);
                }
                buildSummaryValues(pcValueHead.GroupSeq, pcValueHead.HeadNum, configurationSummaryTS);
            }
        }

        private void getQuoteSummary(Guid quoteDtlSysRowID, ConfigurationSummaryTableset configurationSummaryTS)
        {
            QuoteDetailCols QuoteDtl = this.FindQuoteDtlBySysRowID(Session.CompanyID, quoteDtlSysRowID);
            if (QuoteDtl == null)
            {
                throw new BLException(Strings.QuoteLineDoesNotExist);
            }
            string CurrencyCode = this.FindQuoteCurrency(Session.CompanyID, QuoteDtl.QuoteNum);
            if (String.IsNullOrEmpty(CurrencyCode))
                throw new BLException(Strings.QuoteDoesNotExist);

            foreach (PcValueHead pcValueHead in this.SelectPcValueHeadForSummary(Session.CompanyID, QuoteDtl.GroupSeq))
            {
                ConfigurationSummaryRow configurationSummaryRow = new ConfigurationSummaryRow();
                configurationSummaryTS.ConfigurationSummary.Add(configurationSummaryRow);
                configurationSummaryRow.Company = Session.CompanyID;
                configurationSummaryRow.QuoteNum = QuoteDtl.QuoteNum;
                configurationSummaryRow.QuoteLine = QuoteDtl.QuoteLine;
                configurationSummaryRow.ConfigID = pcValueHead.ConfigID;
                configurationSummaryRow.SmartString = pcValueHead.NewSmartString;
                configurationSummaryRow.RowMod = "A";
                configurationSummaryRow.GroupSeq = pcValueHead.GroupSeq;
                configurationSummaryRow.HeadNum = pcValueHead.HeadNum;
                // only want this information for the Top level configurator
                if (pcValueHead.StructTag.Equals("/0", StringComparison.OrdinalIgnoreCase))
                {
                    configurationSummaryRow.ConfigUnitPrice = QuoteDtl.ConfigUnitPrice;
                    configurationSummaryRow.DocConfigUnitPrice = QuoteDtl.ConfigUnitPrice;
                    configurationSummaryRow.ConfigBaseUnitPrice = QuoteDtl.ConfigBaseUnitPrice;
                    configurationSummaryRow.DocConfigBaseUnitPrice = QuoteDtl.ConfigBaseUnitPrice;
                    configurationSummaryRow.LastConfigDate = QuoteDtl.LastConfigDate;
                    configurationSummaryRow.LastConfigTime = QuoteDtl.LastConfigTime;
                    configurationSummaryRow.LastConfigUserID = QuoteDtl.LastConfigUserID;
                    configurationSummaryRow.BasePartNum = QuoteDtl.BasePartNum;
                    configurationSummaryRow.BaseRevisionNum = QuoteDtl.BaseRevisionNum;
                    string pCurrList = string.Empty;
                    string pRateList = string.Empty;
                    List<CurrExChain> ttChainRows = new List<CurrExChain>();
                    LibGetCurrencyRatesEF.FindCurrencyRates("QuoteHed", QuoteDtl.QuoteNum.ToString(), "", "", "", "", "", "", "", out ttChainRows, out pCurrList, out pRateList);
                    configurationSummaryRow.DocConfigUnitPrice = LibRoundAbountEF.ConvertAmtRoundDecimals(configurationSummaryRow.ConfigUnitPrice, CurrencyCode,
                     pCurrList.Entry(0, Ice.Constants.LIST_DELIM), ttChainRows, false, "ttConfigurationSummary", "DocConfigUnitPrice");
                    configurationSummaryRow.DocConfigBaseUnitPrice = LibRoundAbountEF.ConvertAmtRoundDecimals(configurationSummaryRow.ConfigBaseUnitPrice, CurrencyCode,
                   pCurrList.Entry(0, Ice.Constants.LIST_DELIM), ttChainRows, false, "ttConfigurationSummary", "DocConfigUnitPrice");
                }
                buildSummaryValues(pcValueHead.GroupSeq, pcValueHead.HeadNum, configurationSummaryTS);
            }
        }


        private void getOrderSummary(Guid orderDtlSysRowID, ConfigurationSummaryTableset configurationSummaryTS)
        {
            OrderDetailCols OrderDtl = this.FindOrderDtlBySysRowID(Session.CompanyID, orderDtlSysRowID);
            if (OrderDtl == null)
            {
                throw new BLException(Strings.OrderLineDoesNotExist);
            }
            string CurrencyCode = this.FindOrderCurrency(Session.CompanyID, OrderDtl.OrderNum);
            if (String.IsNullOrEmpty(CurrencyCode))
            {
                throw new BLException(Strings.OrderDoesNotExist);
            }

            foreach (PcValueHead pcValueHead in this.SelectPcValueHeadForSummary(Session.CompanyID, OrderDtl.GroupSeq))
            {
                ConfigurationSummaryRow configurationSummaryRow = new ConfigurationSummaryRow();
                configurationSummaryTS.ConfigurationSummary.Add(configurationSummaryRow);
                configurationSummaryRow.Company = Session.CompanyID;
                configurationSummaryRow.OrderNum = OrderDtl.OrderNum;
                configurationSummaryRow.OrderLine = OrderDtl.OrderLine;
                configurationSummaryRow.ConfigID = pcValueHead.ConfigID;
                configurationSummaryRow.SmartString = pcValueHead.NewSmartString;
                configurationSummaryRow.RowMod = "A";
                configurationSummaryRow.GroupSeq = pcValueHead.GroupSeq;
                configurationSummaryRow.HeadNum = pcValueHead.HeadNum;
                // information only valid for the Top level configurator
                if (pcValueHead.StructTag.Equals("/0", StringComparison.OrdinalIgnoreCase))
                {
                    configurationSummaryRow.BasePartNum = OrderDtl.BasePartNum;
                    configurationSummaryRow.BaseRevisionNum = OrderDtl.BaseRevisionNum;
                    configurationSummaryRow.ConfigUnitPrice = OrderDtl.ConfigUnitPrice;
                    configurationSummaryRow.DocConfigUnitPrice = OrderDtl.ConfigUnitPrice;
                    configurationSummaryRow.ConfigBaseUnitPrice = OrderDtl.ConfigBaseUnitPrice;
                    configurationSummaryRow.DocConfigBaseUnitPrice = OrderDtl.ConfigBaseUnitPrice;
                    configurationSummaryRow.LastConfigDate = OrderDtl.LastConfigDate;
                    configurationSummaryRow.LastConfigTime = OrderDtl.LastConfigTime;
                    configurationSummaryRow.LastConfigUserID = OrderDtl.LastConfigUserID;
                    string pCurrList = string.Empty;
                    string pRateList = string.Empty;
                    List<CurrExChain> ttChainRows = new List<CurrExChain>();
                    LibGetCurrencyRatesEF.FindCurrencyRates("OrderHed", OrderDtl.OrderNum.ToString(), "", "", "", "", "", "", "", out ttChainRows, out pCurrList, out pRateList);

                    configurationSummaryRow.DocConfigUnitPrice = LibRoundAbountEF.ConvertAmtRoundDecimals(configurationSummaryRow.ConfigUnitPrice, CurrencyCode,
                        pCurrList.Entry(0, Ice.Constants.LIST_DELIM), ttChainRows, false, "ttConfigurationSummary", "DocConfigUnitPrice");

                    configurationSummaryRow.DocConfigBaseUnitPrice = LibRoundAbountEF.ConvertAmtRoundDecimals(configurationSummaryRow.ConfigBaseUnitPrice, CurrencyCode,
                   pCurrList.Entry(0, Ice.Constants.LIST_DELIM), ttChainRows, false, "ttConfigurationSummary", "DocConfigUnitPrice");

                    GetCurrencyInfo(configurationSummaryRow, CurrencyCode);
                }
                buildSummaryValues(pcValueHead.GroupSeq, pcValueHead.HeadNum, configurationSummaryTS);
            }
        }


        private void GetCurrencyInfo(ConfigurationSummaryRow configurationSummaryRow, string docCurrencyCode)
        {
            string tmpCurrencyCode = string.Empty;
            CurrencyData Currency = this.FindBaseCurrency(Session.CompanyID);
            if (Currency != null)
            {
                configurationSummaryRow.BaseCurrSymbol = Currency.CurrSymbol;
                configurationSummaryRow.BaseCurrencyID = Currency.CurrencyID;
                tmpCurrencyCode = Currency.CurrencyCode;
            }

            Currency = this.FindFirstCurrency(Session.CompanyID, docCurrencyCode);
            if (Currency != null)
            {
                configurationSummaryRow.CurrSymbol = Currency.CurrSymbol;
                configurationSummaryRow.CurrencyID = Currency.CurrencyID;
            }
            configurationSummaryRow.CurrencySwitch = (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.MultiCurrencyManagement) ||
                                                      (Session.ModuleLicensed(Erp.License.ErpLicensableModules.MultiCurrencyManagement) &&
                                                      docCurrencyCode.Equals(tmpCurrencyCode, StringComparison.OrdinalIgnoreCase)));
        }


        #endregion Configuration Summary logic
        /// <summary>
        /// Starts a configuration given a configuration sequence (PcStruct) record and the source and target information.
        /// For Keep When process:
        ///  * If PcValueHead does not exists State will be Added
        /// </summary>
        /// <param name="ds">ConfigurationRuntimeDataSet containing at least one PcValueGrp record to which the configuration is/will be related to.</param>
        /// <param name="ds2">Configuration Sequence DataSet which contains at least the configuration that is being started.</param>
        /// <returns>Serialized input values.</returns>
        public PcValueTableset StartPcValueConfiguration(ref ConfigurationRuntimeTableset ds, ConfigurationSequenceTableset ds2)
        {
            RuntimeImages.Clear();
            ttPcConfigurationParams = (from row in ds.PcConfigurationParams
                                       where !String.IsNullOrEmpty(row.RowMod)
                                       select row).FirstOrDefault();

            if (ttPcConfigurationParams == null)
                throw new BLException(Strings.PcConfigurationNoParams);

            ttPcValueGrp = (from row in ds.PcValueGrp
                            where !String.IsNullOrEmpty(row.RowMod)
                            select row).FirstOrDefault();
            if (ttPcValueGrp == null)
                throw new BLException(Strings.PcValueGrpNotFound);
            // scr 114351 - added the configID check sot he correct pcstruct is found
            //ttPcStruct = (from row in ds2.PcStruct
            //              where row.StructTag == tgtStructTag
            //              select row).FirstOrDefault();
            ttPcStruct = (from row in ds2.PcStruct
                          where row.StructTag.KeyEquals(ttPcConfigurationParams.TgtStructTag)
                          && row.StructID == ttPcConfigurationParams.StructID
                          && !String.IsNullOrEmpty(row.ConfigID)
                          select row).FirstOrDefault();
            if (ttPcStruct == null)
                throw new BLException(Strings.PcStructNofFoundInTS);


            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, ttPcStruct.SubConfigID);
            if (PcStatus == null)
                ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotFound(ttPcStruct.SubConfigID)));

            if (ttPcValueGrp.TestID == Guid.Empty) //clear cache from previous configurator version
                PcStatusCache.Cache.InvalidateCache(new PcStatusCacheKey(PcStatus.Company, PcStatus.ConfigID, PcStatus.ConfigVersion - 1));

            // delete any lingering test assemblies - this can happen if the system crashes or the user kills Epicor.exe via the Task Manager
            if (ttPcValueGrp.ConfigStatus.Equals("Test", StringComparison.OrdinalIgnoreCase))
            {
                if (ttPcStruct == null || !ttPcStruct.SourceTableName.Equals("PartRev", StringComparison.OrdinalIgnoreCase))
                {
                    PCConfiguratorResolver.DeleteConfiguration(PcStatus, ttPcValueGrp.TestID);
                    PCConfiguratorResolver.DeleteDocumentRuleEngine(PcStatus.ConfigID, PcStatus.ConfigVersion, PcStatus.SysRowID, ttPcValueGrp.TestID);
                }
                else
                {
                    //Delete both assemblies because Test Rules generates the Configurator assembly and the assembly for the Method Rule Engine.
                    PCConfiguratorResolver.DeleteConfigurationAndMethodRuleEngine(PcStatus, ttPcValueGrp.TestID,
                        String.IsNullOrEmpty(ttPcStruct.SubPartNum) ? ttPcStruct.PartNum : ttPcStruct.SubPartNum,
                        String.IsNullOrEmpty(ttPcStruct.SubPartNum) ? ttPcStruct.RevisionNum : ttPcStruct.SubRevisionNum);
                }
            }

            //Resolve Target Configuration Type
            RunningState state = RunningState.Update;
            // scr 152877 - check for Test before added
            if (ttPcValueGrp.ConfigStatus.Equals("Test", StringComparison.OrdinalIgnoreCase))
                state = RunningState.Test;
            else if ((!String.IsNullOrEmpty(ttPcConfigurationParams.RunningStateOverride) && ttPcConfigurationParams.RunningStateOverride.Equals("Added", StringComparison.OrdinalIgnoreCase)) ||
              (ttPcValueGrp.ConfigStatus.Equals("Added", StringComparison.OrdinalIgnoreCase)) || !ExistPcValueHead(Session.CompanyID, ttPcValueGrp.GroupSeq, ttPcStruct.StructTag, ttPcStruct.SubConfigID, ttPcStruct.RevolvingSeq))
                state = RunningState.Added;

            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, ttPcValueGrp.TestID, ttPcConfigurationParams.IsTestPlan, ttPcConfigurationParams.SpecID, ttPcConfigurationParams.SpecRevNum);
            if (configuration == null)
                throw new BLException(Strings.UnableToResolveConfiguration(ttPcStruct.SubConfigID));

            LinqRow relatedToRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcValueGrp.RelatedToTableName, ttPcValueGrp.RelatedToSysRowID, LockHint.UpdLock);
            LinqRow sourceRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcStruct.SourceTableName, ttPcStruct.SourceSysRowID, LockHint.UpdLock);
            LinqRow foreignRow = null;

            if (!String.IsNullOrWhiteSpace(ttPcConfigurationParams.ForeignTableName) && !ttPcConfigurationParams.ForeignSysRowID.Equals(Guid.Empty))
                foreignRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcConfigurationParams.ForeignTableName, ttPcConfigurationParams.ForeignSysRowID, LockHint.UpdLock);

            if (!PcStatus.ConfigType.Equals("NIC", StringComparison.OrdinalIgnoreCase))
                configuration.LoadConfiguration(ttPcValueGrp, relatedToRow, sourceRow, foreignRow, ttPcStruct, ttPcConfigurationParams);

            ttPcConfigurationParams.RowMod = IceRow.ROWSTATE_UNCHANGED;
            return configuration.GetTransformedTableset();
        }

        /// <summary>
        /// Starts a configuration given a configuration sequence (PcStruct) record and the source and target information.
        /// For Keep When process:
        ///  * If PcValueHead does not exists State will be Added
        /// </summary>
        /// <param name="ds">ConfigurationRuntimeDataSet containing at least one PcValueGrp record to which the configuration is/will be related to.</param>
        /// <param name="ds2">Configuration Sequence DataSet which contains at least the configuration that is being started.</param>
        /// <returns>Serialized input values.</returns>
        public byte[] StartConfiguration(ref ConfigurationRuntimeTableset ds, ConfigurationSequenceTableset ds2)
        {
            RuntimeImages.Clear();
            ttPcConfigurationParams = (from row in ds.PcConfigurationParams
                                       where !String.IsNullOrEmpty(row.RowMod)
                                       select row).FirstOrDefault();

            if (ttPcConfigurationParams == null)
                throw new BLException(Strings.PcConfigurationNoParams);

            if (ttPcConfigurationParams.ConfigType.KeyEquals("KB"))
            {
                throw new DataValidationException(Strings.KBMaxConfiguratorTypeCannotBeConfigured);
            }

            ttPcValueGrp = (from row in ds.PcValueGrp
                            where !String.IsNullOrEmpty(row.RowMod)
                            select row).FirstOrDefault();
            if (ttPcValueGrp == null)
                throw new BLException(Strings.PcValueGrpNotFound);
            // scr 114351 - added the configID check sot he correct pcstruct is found
            //ttPcStruct = (from row in ds2.PcStruct
            //              where row.StructTag == tgtStructTag
            //              select row).FirstOrDefault();
            ttPcStruct = (from row in ds2.PcStruct
                          where row.StructTag.KeyEquals(ttPcConfigurationParams.TgtStructTag)
                          && row.StructID == ttPcConfigurationParams.StructID
                          && !String.IsNullOrEmpty(row.ConfigID)
                          select row).FirstOrDefault();
            if (ttPcStruct == null)
                throw new BLException(Strings.PcStructNofFoundInTS);

            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, ttPcStruct.SubConfigID);
            if (PcStatus == null)
                ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotFound(ttPcStruct.SubConfigID)));

            // delete any lingering test assemblies - this can happen if the system crashes or the user kills Epicor.exe via the Task Manager
            if (ttPcValueGrp.ConfigStatus.Equals("Test", StringComparison.OrdinalIgnoreCase))
            {
                if (ttPcStruct == null || !ttPcStruct.SourceTableName.Equals("PartRev", StringComparison.OrdinalIgnoreCase))
                {
                    PCConfiguratorResolver.DeleteConfiguration(PcStatus, ttPcValueGrp.TestID);
                    PCConfiguratorResolver.DeleteDocumentRuleEngine(PcStatus.ConfigID, PcStatus.ConfigVersion, PcStatus.SysRowID, ttPcValueGrp.TestID);
                }
                else
                {
                    //Delete both assemblies because Test Rules generates the Configurator assembly and the assembly for the Method Rule Engine.
                    PCConfiguratorResolver.DeleteConfigurationAndMethodRuleEngine(PcStatus, ttPcValueGrp.TestID,
                        String.IsNullOrEmpty(ttPcStruct.SubPartNum) ? ttPcStruct.PartNum : ttPcStruct.SubPartNum,
                        String.IsNullOrEmpty(ttPcStruct.SubPartNum) ? ttPcStruct.RevisionNum : ttPcStruct.SubRevisionNum);
                }
            }

            //Resolve Target Configuration Type
            RunningState state = RunningState.Update;
            if ((!String.IsNullOrEmpty(ttPcConfigurationParams.RunningStateOverride) && ttPcConfigurationParams.RunningStateOverride.Equals("Added", StringComparison.OrdinalIgnoreCase)) || (ttPcValueGrp.ConfigStatus.Equals("Added", StringComparison.OrdinalIgnoreCase)) || !ExistPcValueHead(Session.CompanyID, ttPcValueGrp.GroupSeq, ttPcStruct.StructTag, ttPcStruct.SubConfigID, ttPcStruct.RevolvingSeq))
                state = RunningState.Added;
            else if (ttPcValueGrp.ConfigStatus.Equals("Test", StringComparison.OrdinalIgnoreCase))
                state = RunningState.Test;

            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, ttPcValueGrp.TestID, ttPcConfigurationParams.IsTestPlan, ttPcConfigurationParams.SpecID, ttPcConfigurationParams.SpecRevNum);
            if (configuration == null)
                throw new BLException(Strings.UnableToResolveConfiguration(ttPcStruct.SubConfigID));

            LinqRow relatedToRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcValueGrp.RelatedToTableName, ttPcValueGrp.RelatedToSysRowID, LockHint.UpdLock);
            LinqRow sourceRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcStruct.SourceTableName, ttPcStruct.SourceSysRowID, LockHint.UpdLock);
            LinqRow foreignRow = null;

            if (!String.IsNullOrWhiteSpace(ttPcConfigurationParams.ForeignTableName) && !ttPcConfigurationParams.ForeignSysRowID.Equals(Guid.Empty))
                foreignRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcConfigurationParams.ForeignTableName, ttPcConfigurationParams.ForeignSysRowID, LockHint.UpdLock);

            //ttPcConfigurationParams.RowMod = IceRow.ROWSTATE_UNCHANGED;
            if (!PcStatus.ConfigType.Equals("NIC", StringComparison.OrdinalIgnoreCase))
                configuration.LoadConfiguration(ttPcValueGrp, relatedToRow, sourceRow, foreignRow, ttPcStruct, ttPcConfigurationParams);

            ttPcConfigurationParams.RowMod = IceRow.ROWSTATE_UNCHANGED;
            return configuration.GetSerializedTableset();
        }

        /// <summary>
        /// Saves a single level configuration.
        /// </summary>
        /// <param name="configurationSequenceDS"></param>
        /// <param name="configurationRuntimeDS"></param>
        /// <param name="pcValueDS"></param>
        /// <param name="testPassed">returns true if inspection plan test passed</param>
        /// <param name="failText">fail message if inspection plan test did not pass</param>
        /// <param name="testResultsDS">Output parameter that is populated when this method is executed under a rule test context.</param>
        /// <returns></returns>
        public bool SavePcValueConfiguration(ConfigurationSequenceTableset configurationSequenceDS, ref ConfigurationRuntimeTableset configurationRuntimeDS, PcValueTableset pcValueDS, out bool testPassed,
            out string failText, ref PcTestResultsTableset testResultsDS)
        {
            testPassed = true;
            failText = "";



            ttPcConfigurationParams = (from row in configurationRuntimeDS.PcConfigurationParams
                                       where !String.IsNullOrEmpty(row.RowMod)
                                       select row).FirstOrDefault();

            if (ttPcConfigurationParams == null)
                throw new BLException(Strings.PcConfigurationNoParams);



            ttPcStruct = (from row in configurationSequenceDS.PcStruct
                          where !String.IsNullOrEmpty(row.RowMod) &&
                          row.StructTag.KeyEquals(ttPcConfigurationParams.TgtStructTag) &&
                          row.StructID == ttPcConfigurationParams.StructID
                          select row).FirstOrDefault();

            if (ttPcStruct == null)
                throw new BLException(Strings.PcStructNofFoundInTS);

            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, ttPcStruct.SubConfigID);
            if (PcStatus == null)
                ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotFound(ttPcStruct.SubConfigID)));

            ttPcValueGrp = (from row in configurationRuntimeDS.PcValueGrp
                            where !String.IsNullOrEmpty(row.RowMod)
                            select row).FirstOrDefault();
            if (ttPcValueGrp == null)
                ExceptionManager.AddBLException(new BLException(Strings.PcValueGrpRecordNotFound));

            RunningState state = RunningState.Update;
            if (ttPcValueGrp.ConfigStatus.Equals("Added", StringComparison.OrdinalIgnoreCase))
            {
                state = RunningState.Added;
            }
            else if (ttPcValueGrp.ConfigStatus.Equals("Test", StringComparison.OrdinalIgnoreCase))
            {
                state = RunningState.Test;
            }

            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, ttPcValueGrp.TestID, ttPcConfigurationParams.IsTestPlan, ttPcConfigurationParams.SpecID, ttPcConfigurationParams.SpecRevNum);
            if (configuration != null)
            {

                using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                {
                    configuration.SetTransformedTableset(pcValueDS, ttPcStruct);
                    List<PcValueTableset> list = new List<PcValueTableset>();
                    list.Add(configuration.CurrentTableset);
                    configuration.SaveConfiguration(ttPcValueGrp, ttPcStruct, ttPcConfigurationParams, out testPassed, out failText);

                    if (!ttPcConfigurationParams.IsTestPlan && configurationRuntimeDS.PcInputsPublishToDocParams.Count > 0)
                    {
                        this.relatedEntity = ConfiguratorUtil.FindBySysRowID(Db, ttPcValueGrp.RelatedToTableName, ttPcValueGrp.RelatedToSysRowID, LockHint.NoLock);
                        SavePublishToDoc(configurationRuntimeDS.PcInputsPublishToDocParams, ttPcValueGrp.RelatedToTableName, PcStatus.AttrClassID, PcStatus.ConfigID);
                    }
                    //Runs the method rule test and generates the test data in the PcTestResultsTableset parameter
                    if (ttPcValueGrp.TestMode.Equals("RULES", StringComparison.OrdinalIgnoreCase))
                    {
                        this.testMethodRules(configuration, configurationSequenceDS, ttPcValueGrp.TestID, ttPcStruct.StructTag, ttPcStruct.StructID, ref testResultsDS);
                        if (PcStatus.ConfigType.KeyEquals("EWC"))
                        {
                            string FileStoreKey = GetFileStoreKeyForTestRules(ttPcConfigurationParams.PartNum, ttPcConfigurationParams.RevisionNum, PcStatus.ConfigID);
                            // delete any existing file store - clean up done here in case something fails downstream before the request is made to retrieve the data
                            this.deleteFileStoreEntry(FileStoreKey);
                            // create the new one
                            byte[] bytes = null;
                            using (var memory = new MemoryStream())
                            {
                                for (int x = 0; x < testResultsDS.Tables.Count; x++)
                                {
                                    var idc = testResultsDS.Tables[x] as IDCSerializer;
                                    idc?.Serialize(memory);
                                }
                                bytes = memory.ToArray();
                                if (bytes.Length > 0)
                                {
                                    Guid fsSysRowID;
                                    var fileStoreRow = SelectFileStoreQuery(Session.TenantID, Session.CompanyID, FileStoreKey);
                                    if (fileStoreRow == null)
                                    {
                                        using (var fileStoreLib = ServiceRenderer.GetService<Ice.Contracts.FileStoreSvcContract>(Db))
                                        {
                                            fsSysRowID = fileStoreLib.Create(bytes, new Guid(), "", "", FileStoreKey, Session.CompanyID, Session.TenantID, "");
                                            // ice FW does not have a method exposed where we can update the category -> adding the logic here so we can easily identify the file store entries created for
                                            // test rules in case a downstream process fails and we are unable to delete them.  Users will be able to identify the entries created for test rules.
                                            // Intentionally calling the FW methods to create and update the byte[] so we benefit from all of the validations and security checking
                                            var fileStore = FindFirstFileStoreByFileNameAndUpdateLock(Session.TenantID, Session.CompanyID, FileStoreKey);
                                            if (fileStore != null)
                                            {
                                                fileStore.Category = ttPcValueGrp.TestMode;
                                                Db.Validate(fileStore);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // this should never happen.
                                        throw new BLException(Strings.EWCTestRulesFileStoreFoundWhenItShouldNotExist);
                                    }
                                }
                            }
                        }
                    }

                    Db.Validate();
                    txScope.Complete();
                }

                //Part Creation Logic
                using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                {

                    if (ttPcValueGrp.TestID.Equals(Guid.Empty))
                    {
                        bool partRefresh = LibPartCreation.CreatePart(ttPcStruct, ttPcConfigurationParams.RelatedToSysRowID, ttPcConfigurationParams.RelatedToTable, ttPcConfigurationParams.ForeignSysRowID, ttPcConfigurationParams.ForeignTableName, configurationSequenceDS);

                        //Execute DocRules for PartCreation and low levels only. If the new part number does not exist in the database is not needed execute the rules.
                        if (!ttPcStruct.StructTag.KeyEquals("/0") && !String.IsNullOrEmpty(ttPcStruct.NewPartNum) && this.ExistsPart(Session.CompanyID, ttPcStruct.NewPartNum))
                        {
                            //This call cannot be inside the LibPartCreation because Configuration has a reference to that library and to avoid duplicate the code in ProcessDocumentRules.
                            this.ProcessDocumentRules(ref configurationSequenceDS, ref configurationRuntimeDS, pcValueDS);
                        }

                        exportPartCreationInfo(ttPcStruct);
                        if (ttPcStruct.CreatePart && this.ExistsPart(Session.CompanyID, ttPcStruct.NewPartNum) && partRefresh)
                        {
                            configuration.ChangePartNum(ttPcConfigurationParams.RelatedToTable);
                        }


                        if (ttPcStruct == (from row in configurationSequenceDS.PcStruct
                                           where row.KeepIt
                                           orderby row.StructTag
                                           select row).LastOrDefault())
                        {
                            foreach (PcValueHead pcValueHead in SelectPcValueHeadWithUpLock(ttPcStruct.Company, ttPcValueGrp.GroupSeq, ttPcConfigurationParams.InitialStructTag, ttPcConfigurationParams.InitialRuleTag))
                            {
                                if (!configurationSequenceDS.PcStruct.Any(row => row.Company.KeyEquals(ttPcStruct.Company) &&
                                                                   row.StructTag.KeyEquals(pcValueHead.StructTag) &&
                                                                   row.SubConfigID.KeyEquals(pcValueHead.ConfigID) &&
                                                                   row.RevolvingSeq == pcValueHead.RevolvingSeq &&
                                                                   row.KeepIt))
                                {
                                    Db.PcValueHead.Delete(pcValueHead);
                                }
                            }
                        }
                    }
                    //Delete the generated binaries if this was just a test (takes care of method rule engines too)
                    if (ttPcValueGrp.TestID != Guid.Empty)
                    {
                        if (ttPcStruct == null || !ttPcStruct.SourceTableName.Equals("PartRev", StringComparison.OrdinalIgnoreCase))
                        {
                            PCConfiguratorResolver.DeleteConfiguration(PcStatus, ttPcValueGrp.TestID);
                            PCConfiguratorResolver.DeleteDocumentRuleEngine(PcStatus.ConfigID, PcStatus.ConfigVersion, PcStatus.SysRowID, ttPcValueGrp.TestID);
                        }
                        else
                        {
                            //Delete both assemblies because Test Rules generates the Configurator assembly and the assembly for the Method Rule Engine.
                            PCConfiguratorResolver.DeleteConfigurationAndMethodRuleEngine(PcStatus, ttPcValueGrp.TestID,
                                String.IsNullOrEmpty(ttPcStruct.SubPartNum) ? ttPcStruct.PartNum : ttPcStruct.SubPartNum,
                                String.IsNullOrEmpty(ttPcStruct.SubPartNum) ? ttPcStruct.RevisionNum : ttPcStruct.SubRevisionNum);
                        }
                    }

                    if (!ttPcValueGrp.TestMode.Equals("RULES", StringComparison.OrdinalIgnoreCase))
                    {
                        Db.Validate();
                    }

                    txScope.Complete();
                }

                foreach (PcValueGrpRow prow in configurationRuntimeDS.PcValueGrp)
                {
                    if (String.IsNullOrEmpty(prow.RowMod))
                        prow.RowMod = "D";
                }
                foreach (PcConfigurationParamsRow prow in configurationRuntimeDS.PcConfigurationParams)
                {
                    prow.RowMod = IceRow.ROWSTATE_UNCHANGED;
                }
            }
            return true;
        }

        /// <summary>
        /// Saves a multi level configuration from kinetic.
        /// </summary>
        /// <param name="configSequenceDS"></param>
        /// <param name="configRuntimeDS"></param>
        /// <param name="pcValueDsArray"></param>
        /// <param name="testPassed">returns true if inspection plan test passed</param>
        /// <param name="failText">fail message if inspection plan test did not pass</param>
        /// <param name="testResultsDS">Output parameter that is populated when this method is executed under a rule test context.</param>
        /// <returns></returns>
        public bool SavePcValueConfigurationMulti(ConfigurationSequenceTableset configSequenceDS, ref ConfigurationRuntimeTableset configRuntimeDS, Dictionary<string, PcValueTableset[]> pcValueDsArray, out bool testPassed,
             out string failText, ref PcTestResultsTableset testResultsDS)
        {
            testPassed = true;
            failText = "";


            configRuntimeDS.PcConfigurationParams.ForEach(p => p.RowMod = "");

            foreach (var pcValueDsItem in pcValueDsArray)
            {
                foreach (var item in pcValueDsItem.Value)
                {
                    var pcconfigParam = configRuntimeDS.PcConfigurationParams.FirstOrDefault(i => i.ConfigID.KeyEquals(item.PcContextProperties[0].ConfigurationID));
                    if (pcconfigParam != null)
                    {
                        pcconfigParam.RowMod = "U";
                    }
                    SavePcValueConfiguration(configSequenceDS, ref configRuntimeDS, item, out testPassed, out failText, ref testResultsDS);
                }
            }

            return true;
        }


        private string GetFileStoreKeyForTestRules(string partNum, string revisionNum, string configID)
        {
            Guid partRevSysRowID = FindPartRevSysRowID(Session.CompanyID, partNum, revisionNum, configID);
            string FileStoreKey = Session.CompanyID + "~" + Session.TenantID + "~" + Session.UserID
                  + "~" + partRevSysRowID;
            return FileStoreKey;
        }

        /// <summary>
        /// Saves a single level configuration.
        /// </summary>
        /// <param name="configurationSequenceDS"></param>
        /// <param name="configurationRuntimeDS"></param>
        /// <param name="pcValueDS"></param>
        /// <param name="testPassed">returns true if inspection plan test passed</param>
        /// <param name="failText">fail message if inspection plan test did not pass</param>
        /// <param name="testResultsDS">Output parameter that is populated when this method is executed under a rule test context.</param>
        /// <returns></returns>
        public bool SaveConfiguration(ConfigurationSequenceTableset configurationSequenceDS, ref ConfigurationRuntimeTableset configurationRuntimeDS, byte[] pcValueDS, out bool testPassed, out string failText, ref PcTestResultsTableset testResultsDS)
        {
            testPassed = true;
            failText = "";

            ttPcConfigurationParams = (from row in configurationRuntimeDS.PcConfigurationParams
                                       where !String.IsNullOrEmpty(row.RowMod)
                                       select row).FirstOrDefault();

            if (ttPcConfigurationParams == null)
                throw new BLException(Strings.PcConfigurationNoParams);


            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
            {
                ttPcStruct = (from row in configurationSequenceDS.PcStruct
                              where !String.IsNullOrEmpty(row.RowMod) &&
                              row.StructTag.KeyEquals(ttPcConfigurationParams.TgtStructTag) &&
                              row.StructID == ttPcConfigurationParams.StructID
                              select row).FirstOrDefault();

                if (ttPcStruct == null)
                    throw new BLException(Strings.PcStructNofFoundInTS);

                PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, ttPcStruct.SubConfigID);
                if (PcStatus == null)
                    ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotFound(ttPcStruct.SubConfigID)));

                ttPcValueGrp = (from row in configurationRuntimeDS.PcValueGrp
                                where !String.IsNullOrEmpty(row.RowMod)
                                select row).FirstOrDefault();
                if (ttPcValueGrp == null)
                    ExceptionManager.AddBLException(new BLException(Strings.PcValueGrpRecordNotFound));

                RunningState state = RunningState.Update;
                if (ttPcValueGrp.ConfigStatus.Equals("Added", StringComparison.OrdinalIgnoreCase))
                {
                    state = RunningState.Added;
                }
                else if (ttPcValueGrp.ConfigStatus.Equals("Test", StringComparison.OrdinalIgnoreCase))
                {
                    state = RunningState.Test;
                }

                IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, ttPcValueGrp.TestID, ttPcConfigurationParams.IsTestPlan, ttPcConfigurationParams.SpecID, ttPcConfigurationParams.SpecRevNum);
                if (configuration != null)
                {
                    configuration.SetSerializedTableset(pcValueDS, ttPcStruct);
                    List<PcValueTableset> list = new List<PcValueTableset>();
                    list.Add(configuration.CurrentTableset);
                    configuration.SaveConfiguration(ttPcValueGrp, ttPcStruct, ttPcConfigurationParams, out testPassed, out failText);

                    if (!ttPcConfigurationParams.IsTestPlan)
                    {
                        this.relatedEntity = ConfiguratorUtil.FindBySysRowID(Db, ttPcValueGrp.RelatedToTableName, ttPcValueGrp.RelatedToSysRowID, LockHint.NoLock);
                        SavePublishToDoc(configurationRuntimeDS.PcInputsPublishToDocParams, ttPcValueGrp.RelatedToTableName);
                    }

                    //Runs the method rule test and generates the test data in the PcTestResultsTableset parameter
                    if (ttPcValueGrp.TestMode.Equals("RULES", StringComparison.OrdinalIgnoreCase))
                        this.testMethodRules(configuration, configurationSequenceDS, ttPcValueGrp.TestID, ttPcStruct.StructTag, ttPcStruct.StructID, ref testResultsDS);

                    if (ttPcValueGrp.TestID.Equals(Guid.Empty))
                    {
                        LibPartCreation.CreatePart(ttPcStruct, ttPcConfigurationParams.RelatedToSysRowID, ttPcConfigurationParams.RelatedToTable, ttPcConfigurationParams.ForeignSysRowID, ttPcConfigurationParams.ForeignTableName, configurationSequenceDS);
                        exportPartCreationInfo(ttPcStruct);
                        if (ttPcStruct == configurationSequenceDS.PcStruct.LastOrDefault())
                        {
                            foreach (PcValueHead pcValueHead in SelectPcValueHeadWithUpLock(ttPcStruct.Company, ttPcValueGrp.GroupSeq, ttPcConfigurationParams.InitialStructTag, ttPcConfigurationParams.InitialRuleTag))
                            {
                                if (!configurationSequenceDS.PcStruct.Any(row => row.Company.KeyEquals(ttPcStruct.Company) &&
                                                                            row.StructTag.KeyEquals(pcValueHead.StructTag) &&
                                                                            row.SubConfigID.KeyEquals(pcValueHead.ConfigID) &&
                                                                            row.RevolvingSeq == pcValueHead.RevolvingSeq &&
                                                                            row.KeepIt))
                                {
                                    Db.PcValueHead.Delete(pcValueHead);
                                }
                            }
                        }
                    }
                    //Delete the generated binaries if this was just a test (takes care of method rule engines too)
                    if (ttPcValueGrp.TestID != Guid.Empty)
                    {
                        if (ttPcStruct == null || !ttPcStruct.SourceTableName.Equals("PartRev", StringComparison.OrdinalIgnoreCase))
                        {
                            PCConfiguratorResolver.DeleteConfiguration(PcStatus, ttPcValueGrp.TestID);
                            PCConfiguratorResolver.DeleteDocumentRuleEngine(PcStatus.ConfigID, PcStatus.ConfigVersion, PcStatus.SysRowID, ttPcValueGrp.TestID);
                        }
                        else
                        {
                            //Delete both assemblies because Test Rules generates the Configurator assembly and the assembly for the Method Rule Engine.
                            PCConfiguratorResolver.DeleteConfigurationAndMethodRuleEngine(PcStatus, ttPcValueGrp.TestID,
                                String.IsNullOrEmpty(ttPcStruct.SubPartNum) ? ttPcStruct.PartNum : ttPcStruct.SubPartNum,
                                String.IsNullOrEmpty(ttPcStruct.SubPartNum) ? ttPcStruct.RevisionNum : ttPcStruct.SubRevisionNum);
                        }
                    }
                }
                if (!ttPcValueGrp.TestMode.Equals("RULES", StringComparison.OrdinalIgnoreCase))
                {
                    Db.Validate();
                }
                foreach (PcValueGrpRow prow in configurationRuntimeDS.PcValueGrp)
                {
                    if (String.IsNullOrEmpty(prow.RowMod))
                        prow.RowMod = "D";
                }
                txScope.Complete();
            }
            return true;
        }
        /// <summary>
        /// Saves a multi-level configuration.
        /// For Keep When process:
        ///  * For all configurators that KeepIt are false will not be saved
        ///  * Identify all configurators KeepIt are false to delete PcValueHead and PcValueSet
        /// </summary>
        /// <param name="configSequenceDS">Sequence used to launch the configuration, this dataset should have all the configurations to be saved in PcStruct.</param>
        /// <param name="configRuntimeDS">Configuration dataset from where the initial data will be set. It should have at least the new/existing PcValueGrp record.</param>
        /// <param name="serializedData">All configuration values stored in sets of <see cref="string"/> and byte[] in Bas64 format serialized.</param>
        /// <param name="testResultsDS">Output parameter that is populated when this method is executed under a rule test context.</param>
        /// <param name="testPassed">returns true if inspection plan test passed</param>
        /// <param name="failText">fail message if inspection plan test did not pass</param>
        /// <returns></returns>
        public bool SaveMultiConfiguration(ref ConfigurationSequenceTableset configSequenceDS, ref ConfigurationRuntimeTableset configRuntimeDS, byte[] serializedData, ref PcTestResultsTableset testResultsDS, out bool testPassed, out string failText)
        {
            testPassed = false;
            failText = "";

            ttPcConfigurationParams = (from row in configRuntimeDS.PcConfigurationParams
                                       where !String.IsNullOrEmpty(row.RowMod)
                                       select row).FirstOrDefault();

            if (ttPcConfigurationParams == null)
                throw new BLException(Strings.PcConfigurationNoParams);

            Dictionary<string, IConfiguration> loadedConfigurations = new Dictionary<string, IConfiguration>(StringComparer.InvariantCultureIgnoreCase);
            testResultsDS = new PcTestResultsTableset();
            ttPcValueGrp = (from row in configRuntimeDS.PcValueGrp
                            where !String.IsNullOrEmpty(row.RowMod)
                            select row).FirstOrDefault();
            if (ttPcValueGrp == null)
                ExceptionManager.AddBLException(new BLException("No PcValueGrp record was found."));


            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
            {
                using (MemoryStream stm = new MemoryStream(serializedData))
                {
                    int numOfItems = Serializer.DeserializeInt32(stm);

                    for (int i = 0; i < numOfItems; i++)
                    {
                        string key = Serializer.DeserializeString(stm);
                        ttPcStruct = (from row in configSequenceDS.PcStruct
                                      where row.StructTag.KeyEquals(key) &&
                                      !String.IsNullOrEmpty(row.RowMod)
                                      select row).FirstOrDefault();
                        // scr 114351 - do not execute this logic if the configid = null/empty because we don't want to drop through here for a non configured parent with configured components
                        if (ttPcStruct != null && !String.IsNullOrEmpty(ttPcStruct.ConfigID))
                        {
                            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, ttPcStruct.SubConfigID);
                            if (PcStatus == null)
                                ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotFound(ttPcStruct.SubConfigID)));
                            else
                            {
                                byte[] serializedTS = Convert.FromBase64String(Serializer.DeserializeString(stm));
                                if (serializedTS != null)
                                {
                                    IConfiguration configuration = null;

                                    if (loadedConfigurations.ContainsKey(PcStatus.ConfigID))
                                        configuration = loadedConfigurations[PcStatus.ConfigID];
                                    else
                                    {
                                        RunningState state = RunningState.Update;
                                        if (ttPcValueGrp.ConfigStatus.Equals("Added", StringComparison.OrdinalIgnoreCase))
                                            state = RunningState.Added;
                                        else if (ttPcValueGrp.ConfigStatus.Equals("Test", StringComparison.OrdinalIgnoreCase))
                                            state = RunningState.Test;

                                        configuration = PCConfiguratorResolver.Resolve(PcStatus, state, ttPcValueGrp.TestID, ttPcConfigurationParams.IsTestPlan, ttPcConfigurationParams.SpecID, ttPcConfigurationParams.SpecRevNum);
                                    }

                                    if (configuration != null)
                                    {
                                        configuration.SetSerializedTableset(serializedTS);
                                        List<PcValueTableset> list = new List<PcValueTableset>();
                                        list.Add(configuration.CurrentTableset);
                                        if (ttPcStruct.KeepIt)
                                        {
                                            configuration.SaveConfiguration(ttPcValueGrp, ttPcStruct, ttPcConfigurationParams, out testPassed, out failText);

                                            if (!ttPcConfigurationParams.IsTestPlan)
                                            {
                                                this.relatedEntity = ConfiguratorUtil.FindBySysRowID(Db, ttPcValueGrp.RelatedToTableName, ttPcValueGrp.RelatedToSysRowID, LockHint.NoLock);
                                                SavePublishToDoc(configRuntimeDS.PcInputsPublishToDocParams, ttPcValueGrp.RelatedToTableName);
                                            }
                                        }
                                        else
                                        {
                                            this.DeleteSubConfiguration(ttPcValueGrp, ttPcStruct);
                                            continue;
                                        }

                                        if (ttPcValueGrp.TestID != Guid.Empty)
                                        {
                                            //Runs the method rule test and generates the test data in the PcTestResultsTableset parameter
                                            if (ttPcStruct.StructTag.KeyCompare("/0") == 0 && ttPcValueGrp.TestMode.Equals("RULES", StringComparison.OrdinalIgnoreCase))
                                                this.testMethodRules(configuration, configSequenceDS, ttPcValueGrp.TestID, ttPcStruct.StructTag, ttPcStruct.StructID, ref testResultsDS);
                                        }
                                        else
                                        {
                                            LibPartCreation.CreatePart(ttPcStruct, ttPcConfigurationParams.RelatedToSysRowID, ttPcConfigurationParams.RelatedToTable, ttPcConfigurationParams.ForeignSysRowID, ttPcConfigurationParams.ForeignTableName, configSequenceDS);
                                            exportPartCreationInfo(ttPcStruct);
                                            if (ttPcStruct == configSequenceDS.PcStruct.LastOrDefault())
                                            {
                                                foreach (PcValueHead pcValueHead in SelectPcValueHeadWithUpLock(ttPcStruct.Company, ttPcValueGrp.GroupSeq, ttPcConfigurationParams.InitialStructTag, ttPcConfigurationParams.InitialRuleTag))
                                                {
                                                    if (!configSequenceDS.PcStruct.Any(row => row.Company.KeyEquals(ttPcStruct.Company) &&
                                                                                                row.StructTag.KeyEquals(pcValueHead.StructTag) &&
                                                                                                row.SubConfigID.KeyEquals(pcValueHead.ConfigID) &&
                                                                                                row.RevolvingSeq == pcValueHead.RevolvingSeq &&
                                                                                                row.KeepIt))
                                                    {
                                                        Db.PcValueHead.Delete(pcValueHead);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //Delete the generated binaries if this was just a test (takes care of method rule engines too)
                                if (ttPcValueGrp.TestID != Guid.Empty)
                                {
                                    if (ttPcStruct == null || !ttPcStruct.SourceTableName.Equals("PartRev", StringComparison.OrdinalIgnoreCase))
                                    {
                                        PCConfiguratorResolver.DeleteConfiguration(PcStatus, ttPcValueGrp.TestID);
                                        PCConfiguratorResolver.DeleteDocumentRuleEngine(PcStatus.ConfigID, PcStatus.ConfigVersion, PcStatus.SysRowID, ttPcValueGrp.TestID);
                                    }
                                    else
                                    {
                                        //Delete both assemblies because Test Rules generates the Configurator assembly and the assembly for the Method Rule Engine.
                                        PCConfiguratorResolver.DeleteConfigurationAndMethodRuleEngine(PcStatus, ttPcValueGrp.TestID,
                                            String.IsNullOrEmpty(ttPcStruct.SubPartNum) ? ttPcStruct.PartNum : ttPcStruct.SubPartNum,
                                            String.IsNullOrEmpty(ttPcStruct.SubPartNum) ? ttPcStruct.RevisionNum : ttPcStruct.SubRevisionNum);
                                    }
                                }
                            }
                        }
                    }

                    if (!ttPcValueGrp.ConfigStatus.Equals("Test", StringComparison.OrdinalIgnoreCase) && !ttPcValueGrp.ConfigStatus.Equals("Added", StringComparison.OrdinalIgnoreCase))
                        foreach (var ttPcStruct in (from row in configSequenceDS.PcStruct
                                                    where !row.KeepIt &&
                                                    !String.IsNullOrEmpty(row.RowMod)
                                                    select row))
                        {
                            if (ttPcStruct != null && !String.IsNullOrEmpty(ttPcStruct.ConfigID))
                            {
                                if (!ExistsPcStatus(Session.CompanyID, ttPcStruct.SubConfigID))
                                    ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotFound(ttPcStruct.SubConfigID)));
                                else
                                    this.DeleteSubConfiguration(ttPcValueGrp, ttPcStruct);
                            }

                        }

                    loadedConfigurations.Clear();
                }
                if (!ttPcValueGrp.TestMode.Equals("RULES", StringComparison.OrdinalIgnoreCase))
                {
                    Db.Validate();
                }
                txScope.Complete();
                return true;
            }
        }

        private void SavePublishToDoc(PcInputsPublishToDocParamsTable inputsPublishToDoc, string relatedToTableName, string attrClassID = "", string configID = "")
        {

            if (inputsPublishToDoc.Count < 1L) return;

            int OrderNum = 0;
            int OrderLine = 0;
            string pricingMode = string.Empty;

            foreach (var input in (from input_row in inputsPublishToDoc
                                   select input_row))
            {

                IEnumerable linqTargetEntities = null;
                switch (relatedToTableName.ToUpperInvariant())
                {
                    //Get all line details
                    case "ORDERDTL":
                        linqTargetEntities = SelectOrderDtl(Session.CompanyID, Compatibility.Convert.ToInt32(relatedEntity["OrderNum"].ToString()), Compatibility.Convert.ToInt32(relatedEntity["OrderLine"].ToString()));
                        OrderNum = Compatibility.Convert.ToInt32(relatedEntity["OrderNum"].ToString());
                        OrderLine = Compatibility.Convert.ToInt32(relatedEntity["OrderLine"].ToString());
                        pricingMode = "ORDER";
                        break;
                    case "DEMANDDETAIL":
                        linqTargetEntities = SelectDemandDetail(Session.CompanyID, Compatibility.Convert.ToInt32(relatedEntity["DemandContractNum"].ToString()), Compatibility.Convert.ToInt32(relatedEntity["DemandHeadSeq"].ToString()), Compatibility.Convert.ToInt32(relatedEntity["DemandDtlSeq"].ToString()));
                        break;
                    case "QUOTEDTL":
                        linqTargetEntities = SelectQuoteDtl(Session.CompanyID, Compatibility.Convert.ToInt32(relatedEntity["QuoteNum"].ToString()), Compatibility.Convert.ToInt32(relatedEntity["QuoteLine"].ToString()));
                        OrderNum = Compatibility.Convert.ToInt32(relatedEntity["QuoteNum"].ToString());
                        OrderLine = Compatibility.Convert.ToInt32(relatedEntity["QuoteLine"].ToString());
                        pricingMode = "QUOTE";
                        break;
                    case "QUOTEASM":
                        linqTargetEntities = SelectQuoteAsm(Session.CompanyID, Compatibility.Convert.ToInt32(relatedEntity["QuoteNum"].ToString()), Compatibility.Convert.ToInt32(relatedEntity["QuoteLine"].ToString()), Compatibility.Convert.ToInt32(relatedEntity["AssemblySeq"].ToString()));
                        break;
                    case "QUOTEMTL":
                        linqTargetEntities = SelectQuoteMtl(Session.CompanyID, Compatibility.Convert.ToInt32(relatedEntity["QuoteNum"].ToString()), Compatibility.Convert.ToInt32(relatedEntity["QuoteLine"].ToString()), Compatibility.Convert.ToInt32(relatedEntity["AssemblySeq"].ToString()), Compatibility.Convert.ToInt32(relatedEntity["MtlSeq"].ToString()));
                        break;
                    case "PODETAIL":
                        linqTargetEntities = SelectPODetail(Session.CompanyID, Compatibility.Convert.ToInt32(relatedEntity["PONUM"].ToString()));
                        break;
                    case "PCECCORDERDTL":
                        linqTargetEntities = SelectPcECCOrderDtl(Session.CompanyID, relatedEntity["ECCQuoteNum"].ToString());
                        break;
                    default:
                        continue;
                }

                XElement xPcValues = null;
                DynAttrValueSetTableset dynAttrValueSetDS = new DynAttrValueSetTableset();
                //Find PcValueSet from the corresponding line detail
                foreach (SysRowIDResult sysRowIDResult in linqTargetEntities)
                {
                    using (DynAttributeLib dynAttributeLib = new DynAttributeLib(Db))
                    {
                        if (sysRowIDResult == null) continue;
                        foreach (PcValueSet pcValueSetRow in GetPcValueSetsWithLock(Session.CompanyID, input.Key, sysRowIDResult.SysRowID))
                        {
                            using (StringReader summaryValues = new StringReader(pcValueSetRow.FieldValues))
                            {
                                xPcValues = XElement.Load(summaryValues);

                                if (xPcValues.Descendants().Elements(input.Key).Any())
                                {
                                    xPcValues.Descendants().Elements(input.Key).FirstOrDefault().Value = input.Value;
                                    if (!String.IsNullOrEmpty(attrClassID))
                                    {
                                        dynAttributeLib.AddToAttributeSetForPublishToDoc("Erp", relatedToTableName, sysRowIDResult.SysRowID, attrClassID, configID, input.Key, input.Value, ref dynAttrValueSetDS);
                                    }
                                }

                                pcValueSetRow.FieldValues = xPcValues.ToString();
                                if (String.Compare(relatedToTableName, "OrderDtl", StringComparison.OrdinalIgnoreCase) == 0 ||
                                       String.Compare(relatedToTableName, "QuoteDtl", StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    PCRepriceConfig.RunRepriceConfig(sysRowIDResult.SysRowID, pricingMode, relatedToTableName, OrderNum, OrderLine, true, out decimal TotalPrice, out decimal BasePrice);
                                }
                                Db.Validate();
                            }
                        }
                        dynAttributeLib.UpdateDynAttributesForPublishToDoc(attrClassID, relatedToTableName, sysRowIDResult.SysRowID, ref dynAttrValueSetDS);
                    }
                }
            }
        }


        /// <summary>
        /// Method is executed when the customer X-es out of the configurator instead of pressing save and they are in test mode
        /// </summary>
        /// <param name="configSequenceDS">Configuration Sequence</param>
        /// <param name="testID">Test Guid</param>
        public void DeleteAssembliesInTestMode(ref ConfigurationSequenceTableset configSequenceDS, Guid testID)
        {
            if (testID == Guid.Empty) return; // not in test mode, should not have been called

            PcStatus PcStatus = null;
            foreach (var pcStruct in (from row in configSequenceDS.PcStruct
                                      where !String.IsNullOrEmpty(row.RowMod)
                                      select row))
            {
                if (pcStruct.StructTag.KeyCompare("/0") == 0)
                    PcStatus = FindFirstPcStatus(Session.CompanyID, pcStruct.ConfigID);
                else
                    PcStatus = FindFirstPcStatus(Session.CompanyID, pcStruct.SubConfigID);

                if (PcStatus == null)
                    ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotFound(pcStruct.SubConfigID)));


                if (!pcStruct.SourceTableName.Equals("PartRev", StringComparison.OrdinalIgnoreCase))
                {
                    PCConfiguratorResolver.DeleteConfiguration(PcStatus, testID);
                    PCConfiguratorResolver.DeleteDocumentRuleEngine(PcStatus.ConfigID, PcStatus.ConfigVersion, PcStatus.SysRowID, testID);
                }
                else
                {
                    //Delete both assemblies because Test Rules generates the Configurator assembly and the assembly for the Method Rule Engine.
                    PCConfiguratorResolver.DeleteConfigurationAndMethodRuleEngine(PcStatus, testID,
                        String.IsNullOrEmpty(pcStruct.SubPartNum) ? pcStruct.PartNum : pcStruct.SubPartNum,
                        String.IsNullOrEmpty(pcStruct.SubPartNum) ? pcStruct.RevisionNum : pcStruct.SubRevisionNum);
                }

            }

        }

        #region ProcessDocumentRules

        /// <summary>
        /// Process the no inputs configurator for Kinetic screens.
        /// The method uses the parameters to build the Tablesets needed to invoke the ProcessNICDocumentRules.
        /// </summary>
        /// <param name="relatedToTable">Target entity this call is related to</param>
        /// <param name="relatedToSysRowID">SysRowID of the target entity</param>
        /// <param name="partNum">Part Number to run NIC</param>
        /// <param name="revisionNum">Revision Number to run NIC</param>
        /// <param name="altMethod">Alternate Method to run NIC</param>
        /// <param name="configID">Configuration ID</param>
        /// <param name="foreignTableName">Foreign entity this call is related to</param>
        /// <param name="foreignSysRowID">SysRowID of the foreign entity</param>
        /// <exception cref="BLException"></exception>
        public void ProcessNoInputsConfigurator(string relatedToTable, Guid relatedToSysRowID, string partNum, string revisionNum, string altMethod, string configID, string foreignTableName, Guid foreignSysRowID)
        {
            ConfigurationRuntimeTableset configurationRuntimeDS = new ConfigurationRuntimeTableset();
            ConfigurationSequenceTableset configurationSequenceDS = new ConfigurationSequenceTableset();
            ConfigurationSummaryTableset configurationSummaryTS = new ConfigurationSummaryTableset();

            PcConfigurationParamsRow newRow = new PcConfigurationParamsRow();
            newRow.Company = Session.CompanyID;
            newRow.PartNum = partNum;
            newRow.RevisionNum = revisionNum;
            newRow.AltMethod = altMethod;
            newRow.ConfigID = configID;
            newRow.UniqueID = "0/0";

            newRow.RelatedToTable = relatedToTable;
            newRow.RelatedToSysRowID = relatedToSysRowID;
            newRow.SourceTable = "PartRev";
            newRow.TestID = Guid.Empty;
            newRow.TestMode = "PRODUCTION";
            newRow.IsTestPlan = false;
            newRow.SpecID = string.Empty;
            newRow.SpecRevNum = string.Empty;
            newRow.InspType = string.Empty;
            newRow.ForeignTableName = foreignTableName;
            newRow.ForeignSysRowID = foreignSysRowID;
            newRow.RowMod = IceRow.ROWSTATE_ADDED;

            configurationRuntimeDS.PcConfigurationParams.Add(newRow);

            configurationSequenceDS = this.PreStartConfiguration(ref configurationRuntimeDS, ref configurationSummaryTS);

            ttPcStruct = (from row in configurationSequenceDS.PcStruct
                          where row.Company.KeyEquals(Session.CompanyID) &&
                          row.ConfigID.KeyEquals(configID) &&
                          String.IsNullOrEmpty(row.RuleTag) &&
                          String.IsNullOrEmpty(row.RowMod)
                          select row).FirstOrDefault();
            if (ttPcStruct == null)
                throw new BLException(Strings.PcStructNofFoundInTS);
            else
                ttPcStruct.RowMod = IceRow.ROWSTATE_UPDATED;

            ProcessNICDocumentRules(ref configurationSequenceDS, ref configurationRuntimeDS);
        }

        /// <summary>
        /// Process NIC  document rules
        /// </summary>
        /// <param name="configSequenceDS">Sequence used to launch the configuration, this dataset should have all the configurations to be saved in PcStruct.</param>
        /// <param name="configRuntimeDS">Configuration run time data set</param>
        public void ProcessNICDocumentRules(ref ConfigurationSequenceTableset configSequenceDS, ref ConfigurationRuntimeTableset configRuntimeDS)
        {
            ttPcConfigurationParams = (from row in configRuntimeDS.PcConfigurationParams
                                       where !String.IsNullOrEmpty(row.RowMod)
                                       select row).FirstOrDefault();

            if (ttPcConfigurationParams == null)
                throw new BLException(Strings.PcConfigurationNoParams);

            // when a test doc rules is implemented the call to test pricing needs to go somewhere in here

            if (leaveDocumentRules(true)) return;

            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
            {

                ttPcStruct = (from row in configSequenceDS.PcStruct
                              where String.IsNullOrEmpty(row.RuleTag) &&
                              !String.IsNullOrEmpty(row.RowMod)
                              select row).FirstOrDefault();
                if (ttPcStruct != null && !String.IsNullOrEmpty(ttPcStruct.ConfigID))
                {
                    LinqRow relatedToRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcConfigurationParams.RelatedToTable, ttPcConfigurationParams.RelatedToSysRowID, LockHint.UpdLock);
                    PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, ttPcStruct.SubConfigID);
                    if (PcStatus != null)
                    {
                        LinqRow foreignRow = null;
                        LinqRow sourceRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcStruct.SourceTableName, ttPcStruct.SourceSysRowID, LockHint.UpdLock);

                        if (ttPcConfigurationParams.ForeignSysRowID != Guid.Empty)
                            foreignRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcConfigurationParams.ForeignTableName, ttPcConfigurationParams.ForeignSysRowID, LockHint.UpdLock);

                        IConfiguration docconfig = null;
                        docconfig = PCConfiguratorResolver.Resolve(PcStatus, RunningState.Update, ttPcConfigurationParams.TestID, ttPcConfigurationParams.IsTestPlan, ttPcConfigurationParams.SpecID, ttPcConfigurationParams.SpecRevNum);
                        docconfig.LoadNICConfiguration(relatedToRow, sourceRow, foreignRow, ttPcStruct, ttPcConfigurationParams);

                        if (ttPcConfigurationParams.ForeignSysRowID == Guid.Empty)
                        {
                            IRuleEngine<RuleArgs> docRuleEngine = null;
                            docRuleEngine = docconfig.ResolveDocumentRuleEngine(PcStatus, configSequenceDS);
                            if (docRuleEngine != null)
                            {
                                Part newPart = (String.IsNullOrEmpty(ttPcStruct.NewPartNum)) ? null : this.FindFirstPart(Session.CompanyID, ttPcStruct.NewPartNum);

                                RuleArgs ruleArgs = new RuleArgs();
                                ruleArgs.RuleKey = string.Empty;
                                ruleArgs.commitValues = true;
                                ruleArgs.NewPart = newPart;
                                docRuleEngine.Invoke(ruleArgs.RuleKey, ruleArgs);
                            }
                        }
                    }
                }
                Db.Validate();
                txScope.Complete();
            }
            ttPcConfigurationParams.RowMod = IceRow.ROWSTATE_UNCHANGED;
        }

        private bool leaveDocumentRules(bool isNIC)
        {
            return ttPcConfigurationParams.IsTestPlan || ttPcConfigurationParams.RelatedToTable.Equals("ECORev", StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Process document rules
        /// </summary>
        /// <param name="configSequenceDS">Sequence used to launch the configuration, this dataset should have all the configurations to be saved in PcStruct.</param>
        /// <param name="configRuntimeDS">Configuration dataset from where the initial data will be set. It should have at least the new/existing PcValueGrp record.</param>
        /// <param name="pcValueDS">PcValueDataSet </param>
        /// <returns></returns>
        public void ProcessDocumentRules(ref ConfigurationSequenceTableset configSequenceDS, ref ConfigurationRuntimeTableset configRuntimeDS, PcValueTableset pcValueDS)
        {

            ttPcConfigurationParams = (from row in configRuntimeDS.PcConfigurationParams
                                       where !String.IsNullOrEmpty(row.RowMod)
                                       select row).FirstOrDefault();

            if (ttPcConfigurationParams == null)
                throw new BLException(Strings.PcConfigurationNoParams);

            if (ttPcConfigurationParams.TestMode.Equals("INPUTS", StringComparison.OrdinalIgnoreCase))
            {
                ProcessInputPricingTest(ref configSequenceDS, ref configRuntimeDS, pcValueDS);
                return;
            }

            if (leaveDocumentRules(false)) return;

            using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
            {

                ttPcValueGrp = (from row in configRuntimeDS.PcValueGrp
                                where !String.IsNullOrEmpty(row.RowMod)
                                select row).FirstOrDefault();
                if (ttPcValueGrp == null) return;

                if (ttPcValueGrp.TestID != Guid.Empty) return;

                ttPcStruct = (from row in configSequenceDS.PcStruct
                              where !String.IsNullOrEmpty(row.RowMod) &&
                              row.StructTag.KeyEquals(ttPcConfigurationParams.TgtStructTag) &&
                              row.StructID == ttPcConfigurationParams.StructID
                              select row).FirstOrDefault();
                if (ttPcStruct != null && !String.IsNullOrEmpty(ttPcStruct.ConfigID))
                {
                    LinqRow relatedToRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcValueGrp.RelatedToTableName, ttPcValueGrp.RelatedToSysRowID, LockHint.UpdLock);

                    PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, ttPcStruct.SubConfigID);
                    if (PcStatus != null)
                    {
                        LinqRow foreignRow = ttPcConfigurationParams.ForeignSysRowID != Guid.Empty ? ConfiguratorUtil.FindBySysRowID(Db, ttPcConfigurationParams.ForeignTableName, ttPcConfigurationParams.ForeignSysRowID, LockHint.UpdLock) : null;
                        LinqRow sourceRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcStruct.SourceTableName, ttPcStruct.SourceSysRowID, LockHint.UpdLock);

                        IConfiguration docconfig = null;
                        docconfig = PCConfiguratorResolver.Resolve(PcStatus, RunningState.Update, ttPcValueGrp.TestID, ttPcConfigurationParams.IsTestPlan, ttPcConfigurationParams.SpecID, ttPcConfigurationParams.SpecRevNum);
                        using (ErpCallContext.SetDisposableKey("Document Rules"))
                        {
                            docconfig.LoadConfiguration(relatedToRow, sourceRow, foreignRow, ttPcStruct, ttPcConfigurationParams);
                        }

                        InvokeDocRuleEngine(PcStatus, ref configSequenceDS, ref docconfig);
                    }
                }
                Db.Validate();
                txScope.Complete();
            }
            ttPcConfigurationParams.RowMod = IceRow.ROWSTATE_UNCHANGED;
        }

        private void InvokeDocRuleEngine(PcStatus pcStatus, ref ConfigurationSequenceTableset configSequenceDS, ref IConfiguration configuration)
        {
            IRuleEngine<RuleArgs> docRuleEngine = null;
            docRuleEngine = configuration.ResolveDocumentRuleEngine(pcStatus, configSequenceDS);
            if (docRuleEngine != null)
            {
                Part newPart = (String.IsNullOrEmpty(ttPcStruct.NewPartNum)) ? null : this.FindFirstPart(Session.CompanyID, ttPcStruct.NewPartNum);

                RuleArgs ruleArgs = new RuleArgs();
                ruleArgs.RuleKey = string.Empty;
                //Document rules for part creation should be executed in any level.
                ruleArgs.OnlyPartCreationRules = ttPcConfigurationParams.ForeignSysRowID != Guid.Empty || !ttPcConfigurationParams.TgtStructTag.KeyEquals("/0");
                ruleArgs.commitValues = true;
                ruleArgs.NewPart = newPart;
                docRuleEngine.Invoke(ruleArgs.RuleKey, ruleArgs);
            }
        }

        /// <summary>
        /// Processes the input pricing logic in Test Inputs to display the price totals.
        /// </summary>
        private void ProcessInputPricingTest(ref ConfigurationSequenceTableset configSequenceDS, ref ConfigurationRuntimeTableset configRuntimeDS, PcValueTableset pcValueDS)
        {
            ttPcValueGrp = (from row in configRuntimeDS.PcValueGrp
                            where !String.IsNullOrEmpty(row.RowMod)
                            select row).FirstOrDefault();
            if (ttPcValueGrp == null) return;

            ttPcStruct = (from row in configSequenceDS.PcStruct
                          where String.IsNullOrEmpty(row.RuleTag) &&
                          !String.IsNullOrEmpty(row.RowMod)
                          select row).FirstOrDefault();

            ttPcConfigurationParams = (from row in configRuntimeDS.PcConfigurationParams
                                       where !String.IsNullOrEmpty(row.RowMod)
                                       select row).FirstOrDefault();

            if (ttPcConfigurationParams == null)
                throw new BLException(Strings.PcConfigurationNoParams);

            if (ttPcValueGrp.TestID != Guid.Empty)
            {
                PcStatusCols pcStatusCols = FindFirstPcStatus(ttPcValueGrp.RelatedToSysRowID);
                if (pcStatusCols != null)
                {
                    if (ExistsPricingExpression(Session.CompanyID, pcStatusCols.ConfigID))
                    {
                        PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, pcStatusCols.ConfigID);
                        if (PcStatus != null)
                        {
                            IConfiguration docconfig = null;
                            docconfig = PCConfiguratorResolver.Resolve(PcStatus, RunningState.Test, ttPcValueGrp.TestID, ttPcConfigurationParams.IsTestPlan, ttPcConfigurationParams.SpecID, ttPcConfigurationParams.SpecRevNum);
                            if (docconfig != null)
                            {
                                docconfig.SetTransformedTableset(pcValueDS, ttPcStruct);
                            }

                            IRuleEngine<RuleArgs> docRuleEngine = null;
                            // set testMode to true scr 152877
                            docRuleEngine = docconfig.ResolveDocumentRuleEngine(PcStatus, configSequenceDS, true, true);
                            if (docRuleEngine != null)
                            {
                                Part newPart = (String.IsNullOrEmpty(ttPcStruct.NewPartNum)) ? null : this.FindFirstPart(Session.CompanyID, ttPcStruct.NewPartNum);

                                RuleArgs ruleArgs = new RuleArgs();
                                ruleArgs.RuleKey = string.Empty;
                                ruleArgs.commitValues = false;
                                ruleArgs.NewPart = newPart;
                                docRuleEngine.Invoke(ruleArgs.RuleKey, ruleArgs);


                                ttPcConfigurationParams.InputPricingSet = true;

                                ttPcConfigurationParams.OrderPrice = ruleArgs.InputPricing.OrderPrice;
                                ttPcConfigurationParams.QuotePrice = ruleArgs.InputPricing.QuotePrice;
                                ttPcConfigurationParams.DemandPrice = ruleArgs.InputPricing.DemandPrice;
                                ttPcConfigurationParams.PurchasePrice = ruleArgs.InputPricing.PurchasePrice;
                                ttPcConfigurationParams.WebOrderBasketPrice = ruleArgs.InputPricing.WebOrderBasketPrice;
                            }
                        }
                    }
                }
            }
        }

        #endregion ProcessDocumentRules

        /// <summary>
        /// Objective: Run methods rules to determine if assemblies should be keep it or not to hide or show the corresponding configuration.
        /// This process will be executed when:
        ///     * When is clicked next page and changes to next configurator
        ///     * Is multiconfigurator
        ///     * If current part revision has rules set(OPR or ASM)
        /// </summary>
        /// <param name="configurationSequenceDS"></param>
        /// <param name="configRuntimeDS"></param>
        /// <param name="pcValueDS">Values from  current configurator</param>
        /// <param name="parAltMethod">Parent AltMethod</param>
        /// <param name="checkNextCfg">Process Keep When Run before the configuration is display to identify if configuration sequence will be changed by changing partnum rule</param>
        /// <param name="enableNextPage"></param>
        public void ProcessKeepWhen(ref ConfigurationSequenceTableset configurationSequenceDS, ref ConfigurationRuntimeTableset configRuntimeDS, PcValueTableset pcValueDS, string parAltMethod, bool checkNextCfg, ref bool enableNextPage)
        {
            ttPcConfigurationParams = (from row in configRuntimeDS.PcConfigurationParams
                                       where !String.IsNullOrEmpty(row.RowMod)
                                       select row).FirstOrDefault();

            if (ttPcConfigurationParams == null)
                throw new BLException(Strings.PcConfigurationNoParams);


            ttPcStruct = (from row in configurationSequenceDS.PcStruct
                          where !String.IsNullOrEmpty(row.RowMod) &&
                          row.StructTag.KeyEquals(ttPcConfigurationParams.TgtStructTag) &&
                          row.StructID == ttPcConfigurationParams.StructID
                          select row).FirstOrDefault();

            if (ttPcStruct == null)
                throw new BLException(Strings.PcStructNofFoundInTS);

            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, ttPcStruct.SubConfigID);
            if (PcStatus == null)
                ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotFound(ttPcStruct.SubConfigID)));

            LinqRow relatedRow = null;
            LinqRow foreignRow = null;
            RunningState state = RunningState.Update;
            if (ttPcConfigurationParams.TestID != Guid.Empty)
            {
                state = RunningState.Test;
            }
            else
            {
                relatedRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcConfigurationParams.RelatedToTable, ttPcConfigurationParams.RelatedToSysRowID, LockHint.NoLock);

                if (ttPcConfigurationParams.ForeignSysRowID != Guid.Empty)
                    foreignRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcConfigurationParams.ForeignTableName, ttPcConfigurationParams.ForeignSysRowID, LockHint.NoLock);

                if (!((foreignRow != null && Convert.ToInt32(foreignRow["GroupSeq"]) != 0) || Convert.ToInt32(relatedRow["GroupSeq"]) != 0))
                    state = RunningState.Added;
            }

            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, ttPcConfigurationParams.TestID, ttPcConfigurationParams.IsTestPlan, ttPcConfigurationParams.SpecID, ttPcConfigurationParams.SpecRevNum);
            if (configuration != null)
            {
                configuration.SetTransformedTableset(pcValueDS, ttPcStruct);
            }
            PCGenerateMethods.RunProcessKeepWhen(ref configurationSequenceDS, RunningState.KeepWhen, ttPcConfigurationParams.TgtStructTag, ttPcConfigurationParams.StructID, parAltMethod, relatedRow, foreignRow, configuration, checkNextCfg, ref enableNextPage);

            ttPcConfigurationParams.RowMod = IceRow.ROWSTATE_UNCHANGED;
        }
        /// <summary>
        /// DeleteSubConfiguration
        /// </summary>
        public void DeleteSubConfiguration(PcValueGrpRow ttPcValueGrpRow, PcStructRow ttPcStructRow)
        {
            if (ttPcValueGrpRow != null)
            {
                using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                {
                    PcValueHead PcValueHead = this.FindFirstPcValueHeadWithUpLock(
                        Session.CompanyID,
                        ttPcValueGrpRow.GroupSeq,
                        ttPcStructRow.StructTag,
                        ttPcStructRow.SubConfigID,
                        ttPcStructRow.RevolvingSeq);

                    if (PcValueHead != null)
                    {
                        foreach (var pcValueSet in UpdatePCValueSet(Session.CompanyID, PcValueHead.GroupSeq, PcValueHead.HeadNum))
                        {
                            Db.PcValueSet.Delete(pcValueSet);
                        }

                        Db.PcValueHead.Delete(PcValueHead);
                        Db.Validate();
                    }
                    txScope.Complete();
                }
            }
        }
        /// <summary>
        ///  Provides ability to check server side syntax
        /// </summary>
        /// <param name="configID"></param>
        /// <param name="docRuleCheckSyntaxArgs"></param>
        /// <param name="serverEventCheckSyntaxArgs"></param>
        /// <param name="methodRuleCheckSyntaxArgs"></param>
        /// <param name="syntaxErrors"></param>
        public void CheckServerSyntax(string configID, Erp.Shared.Lib.Configurator.DocRuleCheckSyntaxArgs docRuleCheckSyntaxArgs, Erp.Shared.Lib.Configurator.ServerEventCheckSyntaxArgs serverEventCheckSyntaxArgs,
            Erp.Shared.Lib.Configurator.MethodRuleCheckSyntaxArgs methodRuleCheckSyntaxArgs, out string syntaxErrors)
        {
            syntaxErrors = string.Empty;
            PartRev partRevRow = null;

            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, configID);
            if (PcStatus == null)
                throw new BLException(Strings.PcStatusNotFound(configID));
            RunningState state = RunningState.Test;

            if (serverEventCheckSyntaxArgs != null)
            {
                PCConfiguratorResolver.ServerEventCheckSyntaxAndCompile(PcStatus, configID, false, state, serverEventCheckSyntaxArgs, out syntaxErrors);
            }

            if (docRuleCheckSyntaxArgs != null)
            {
                PCConfiguratorResolver.DocumentRuleCheckSyntaxAndCompile(PcStatus, configID, false, state, docRuleCheckSyntaxArgs, out syntaxErrors);
            }

            if (methodRuleCheckSyntaxArgs != null)
            {
                partRevRow = this.FindFirstPartRevAlt(Session.CompanyID, methodRuleCheckSyntaxArgs.PartNum, methodRuleCheckSyntaxArgs.RevisionNum, methodRuleCheckSyntaxArgs.AltMethod);
                if (partRevRow == null)
                    throw new BLException(Strings.PartRevNotFound(methodRuleCheckSyntaxArgs.PartNum, methodRuleCheckSyntaxArgs.RevisionNum));
                PCConfiguratorResolver.MethodRuleCheckSyntaxAndCompile(partRevRow, configID, false, state, methodRuleCheckSyntaxArgs, out syntaxErrors);

                if (syntaxErrors.StartsWith("Error CS0272:", StringComparison.OrdinalIgnoreCase) && (syntaxErrors.IndexOf("ECOMtl.PartNum", StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    syntaxErrors = Strings.UnableToUpdECOMtlPartNum;
                }
            }

        }


        /// <summar>
        /// If configuring in the manufacturing company and it's an enterprise configurator SIValues part
        /// then export the generated part and saved input values to the external sales companies
        /// </summar>
        /// <param name="ttPcStructRow">Configurator Sequence dataset - ttPcStruct</param>
        private void exportPartCreationInfo(PcStructRow ttPcStructRow)
        {
            if (!ttPcStructRow.CreatePart) return;
            if (!ttPcStructRow.ResponseAutoCreatePart) return;

            PcStatusEntResult PcStatus = this.FindFirstPcStatusEntResult(Session.CompanyID, ttPcStructRow.SubConfigID);
            if (PcStatus == null || PcStatus.EntprsConf == false) return;

            if (PcStatus.ExtConfig == false)
            {
                if (!this.existsGlobalPart(Session.CompanyID, ttPcStructRow.NewPartNum))
                {
                    using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                    {
                        Erp.Tables.Part bPart = this.FindPartGlobalUpdate(Session.CompanyID, ttPcStructRow.NewPartNum);
                        if (bPart == null)
                        {
                            txScope.Complete();
                            return;
                        }
                        else
                        {
                            bPart.GlobalPart = true;
                            Db.Validate(bPart);
                            txScope.Complete();
                        }
                    }
                }
                SIECExportMfgSIValueConfig._ExportMfgSIValueConfig(ttPcStructRow.NewPartNum, ttPcStructRow.RevisionNum);
            }
            else
            {
                SIECExportCreatedPart._ExportCreatedPart(ttPcStructRow.ConfigID, ttPcStructRow.NewPartNum, ttPcStructRow.RevisionNum, ttPcStructRow.PartNum, ttPcStructRow.RevisionNum);
            }

        }

        /// <summary>
        /// Call this method when you want to test the rules of a No Inputs Configurator
        /// </summary>
        /// <param name="configID">Configuration ID to run test rules</param>
        /// <param name="partNum">Part Number to run test rules</param>
        /// <param name="revisionNum">Revision Number to run test rules</param>
        /// <param name="ts">Result tableset returned to UI</param>
        public void TestNICRules(string configID, string partNum, string revisionNum, ref PcTestResultsTableset ts)
        {
            if (String.IsNullOrEmpty(configID))
                throw new BLException(Strings.ConfigIDRequired);

            PcStatus PcStatus = this.FindFirstPcStatus(Session.CompanyID, configID);
            if (PcStatus == null)
                throw new BLException(Strings.PcStatusNotFound(configID));

            PartRev bPartRev = this.FindFirstPartRev(Session.CompanyID, partNum, revisionNum, configID);
            if (bPartRev == null)
                throw new BLException(Strings.NoPartRevTestRules);

            Guid testID = Guid.NewGuid();
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, RunningState.Test, testID, false, "", "");
            if (configuration == null)
                throw new BLException(Strings.ResolverError);

            bool useBase = !String.IsNullOrEmpty(bPartRev.BasePartNum) && ExistsEqualBaseConfigID(Session.CompanyID, bPartRev.BasePartNum, bPartRev.BaseRevisionNum, bPartRev.ConfigID);
            ConfigurationSequenceTableset configurationSequenceDS = new ConfigurationSequenceTableset();
            int nStructID = 0;
            ConfiguratorUtil.GetConfigurationSequence(Db, Session.CompanyID, configID, useBase ? bPartRev.BasePartNum : bPartRev.PartNum, useBase ? bPartRev.BaseRevisionNum : bPartRev.RevisionNum, configurationSequenceDS.PcStruct, "PcStatus", false, "", "", ref nStructID);
            PcStructRow PcStructRow = configurationSequenceDS.PcStruct[0];
            ConfigurationRuntimeTableset configRunTimeDS = new ConfigurationRuntimeTableset();
            GetNewPcConfigParams(ref configRunTimeDS, configID, "0/0");
            configRunTimeDS.PcConfigurationParams[0]["ConfigVersion"] = PcStatus.ConfigVersion;
            configRunTimeDS.PcConfigurationParams[0]["RelatedToSysRowID"] = PcStatus.SysRowID; ;
            configRunTimeDS.PcConfigurationParams[0]["SourceTable"] = "PartRev";
            configRunTimeDS.PcConfigurationParams[0]["RelatedToTable"] = "PcStatus";
            configRunTimeDS.PcConfigurationParams[0]["TestID"] = testID;
            configuration.LoadNICConfiguration(PcStatus, bPartRev, null, PcStructRow, configRunTimeDS.PcConfigurationParams[0]);
            try
            {
                using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                {
                    this.testMethodRules(configuration, configurationSequenceDS, testID, "/0", 0, ref ts);
                    txScope.Complete(); // intentional no Db.Validate because there are no rows to write in test mode
                }
            }
            finally
            {
                //Delete both assemblies because Test Rules generates the Configurator assembly and the assembly for the Method Rule Engine.
                PCConfiguratorResolver.DeleteConfigurationAndMethodRuleEngine(PcStatus, testID, bPartRev.PartNum, bPartRev.RevisionNum);
            }
        }

        /// <summary>
        /// Perform a Method Rule test depending on the type of part.
        /// </summary>
        /// <param name="configuration">Generated configuration object.</param>
        /// <param name="configurationSequenceDS">Configuration sequence</param>
        /// <param name="TestID">Test ID</param>
        /// <param name="structTag">Struct Tag</param>
        /// <param name="structID">Struct ID</param>
        /// <param name="ts">Test results tableset.</param>
        private void testMethodRules(IConfiguration configuration, ConfigurationSequenceTableset configurationSequenceDS, Guid TestID, string structTag, int structID, ref PcTestResultsTableset ts)
        {
            PartRev bPartRev = configuration.SourceEntity as PartRev;

            if (bPartRev != null)
            {
                if (this.PartIsSalesKitForParentAndPhantomPart(Session.CompanyID, configurationSequenceDS.PcStruct.FirstOrDefault().PartNum, Session.PlantID))
                {
                    using (TestSalesKits tstSalesKits = new TestSalesKits(Db))
                        tstSalesKits.PerformTest(configuration, configurationSequenceDS, TestID, configuration.SourceEntity as PartRev, structTag, structID, ref ts);
                }
                else
                {
                    using (TestQuoteBOM tstQuote = new TestQuoteBOM(Db))
                        tstQuote.PerformTest(configuration, configurationSequenceDS, TestID, configuration.SourceEntity as PartRev, structTag, structID, ref ts);
                    using (TestJobBOM tstJob = new TestJobBOM(Db))
                        tstJob.PerformTest(configuration, configurationSequenceDS, TestID, configuration.SourceEntity as PartRev, structTag, structID, ref ts);
                }
            }
        }


        /// <summary>
        /// Call this method to retrieve the Test Rules results dataset that is temporarily stored in the FileStore for EWC configurators.
        /// </summary>
        /// <param name="partNum">part number Test Rules was executed for</param>
        /// <param name="revisionNum">revision number Test Rules was executed for</param>
        /// <param name="configID">Configuration ID Test Rules was executed for</param>
        /// <param name="testRulesResultsDS">Data set containing the results of the Test Rules execution</param>
        public void EWCTestRules(string partNum, string revisionNum, string configID, ref PcTestResultsTableset testRulesResultsDS)
        {
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedConfigurator))
                throw new BLException(GlobalStrings.NotLicensedForAdvancedCfg);

            Guid partRevSysRowID = FindPartRevSysRowID(Session.CompanyID, partNum, revisionNum, configID);
            string FileStoreKey = GetFileStoreKeyForTestRules(partNum, revisionNum, configID);

            byte[] serializedTestRulesResultsData = null;
            using (FileStoreUtilities fsUtilities = new FileStoreUtilities(Db))
            {
                serializedTestRulesResultsData = fsUtilities.GetFileStoreEntry(FileStoreKey);
                if (serializedTestRulesResultsData != null && serializedTestRulesResultsData.Length > 0)
                {
                    using (var stm = new MemoryStream(serializedTestRulesResultsData))
                    {

                        for (int x = 0; x < testRulesResultsDS.Tables.Count; x++)
                        {
                            var idc = testRulesResultsDS.Tables[x] as IDCSerializer;
                            idc?.Deserialize(stm);
                        }
                    }
                }
                else
                {
                    throw new BLException(Strings.EWCTestRulesResultsNotFound(configID, partNum, configID));
                }
            }
            // set row mod so the data is available on the client
            for (int x = 0; x < testRulesResultsDS.Tables.Count; x++)
            {
                IIceTable currentTable = testRulesResultsDS.Tables[x];
                foreach (IceRow currentRow in currentTable)
                {
                    currentRow["RowMod"] = IceRow.ROWSTATE_ADDED;
                }
            }
            // delete any existing file store
            this.deleteFileStoreEntry(FileStoreKey);
        }

        /// <summary>
        /// Receives configuration values on a smart string and processes it completely.
        /// </summary>
        /// <param name="parentMfgCompID">Parent Manufacturing Company ID</param>
        /// <param name="iDemandContractNum">Demand Contract Number</param>
        /// <param name="iDemandHeadSeq">Demand Head Sequence that contains the configurable line</param>
        /// <param name="iDemandDtlSeq">Demand Detail Sequence that contains the configurable part</param>
        /// <param name="iSmartString">Smart String value</param>
        public void EDIDemandConfiguration(string parentMfgCompID, int iDemandContractNum, int iDemandHeadSeq, int iDemandDtlSeq, string iSmartString)
        {
            bool vOTSmartString = false;
            Erp.Tables.DemandDetail DemandDetail = null;

            /*Locate the target demand line that will be configured.*/
            DemandDetail = this.FindFirstDemandDetailUpdLock(parentMfgCompID, iDemandContractNum, iDemandHeadSeq, iDemandDtlSeq);
            if (DemandDetail == null)
            {
                throw new BLException(Strings.InvalidDemandLine);
            }

            DemandHeadCols DemandHead = this.FindFirstDemandHead(DemandDetail.Company, DemandDetail.DemandHeadSeq);
            if (DemandHead == null)
            {
                throw new BLException(Strings.DemandHeadNotFound);
            }

            /*Look for One Time Smart String parameter - if ShipToNum <> 0 then get it from Ship To
             * else get it from the Customer.*/
            ShipToCols ShipTo = this.FindFirstShipTo(DemandHead.Company, DemandHead.CustNum, DemandHead.ShipToNum);
            if (ShipTo != null && ShipTo.DemandUseCustomerValues == false)
            {
                vOTSmartString = ShipTo.OTSmartString;
            }
            else
            {
                CustomerCols Customer = this.FindFirstCustomer(DemandHead.Company, DemandHead.CustNum);
                if (Customer != null) vOTSmartString = Customer.OTSmartString;
            }

            /*If related customer is set to One Time Smart String and Demand Detail already has a smart string then leave.*/
            if (vOTSmartString && DemandDetail.SmartStringProcessed)
            {
                using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())//start the transaction
                {
                    Erp.Tables.DemandLog DemandLog = new DemandLog();
                    Db.DemandLog.Insert(DemandLog);
                    DemandLog.Company = Session.CompanyID;
                    DemandLog.DemandContractNum = iDemandContractNum;
                    DemandLog.DemandHeadSeq = iDemandHeadSeq;
                    DemandLog.DemandDtlSeq = iDemandDtlSeq;
                    DemandLog.LogText = Strings.DemandOneTimeOnly;
                    DemandLog.LogCode = Strings.CanConfigureOnce;
                    DemandLog.Action = Strings.Warning;
                    DemandLog.SysTime = Erp.Internal.Lib.DateExtensions.SecondsSinceMidnight(CompanyTime.Now());
                    DemandLog.SysDate = CompanyTime.Now().Date;
                    Db.Validate(DemandLog);
                    txScope.Complete();
                }
                return;
            }

            PartRevCols PartRev = this.FindFirstPartRev(DemandDetail.Company, DemandDetail.PartNum, DemandDetail.RevisionNum);
            if (PartRev == null)
            {
                throw new BLException(Strings.PartRevNotFound(DemandDetail.PartNum, DemandDetail.RevisionNum));
            }

            Erp.Tables.PcStatus PcStatus = this.FindFirstPcStatus(PartRev.Company, PartRev.ConfigID);
            if (PcStatus == null)
            {
                throw new BLException(Strings.PcStatusNotApproved(PartRev.ConfigID));
            }
            if (PcStatus.ConfigType.KeyEquals("EWC"))
            {
                throw new BLException(Strings.EDIDemandConfigNotValidForEWC(PcStatus.ConfigID));
            }
            /*No Smart String allowed? can't configure through EDI!
             * we don't send an error just to avoid stopping the workflow.*/
            if (!this.ExistsDemandAllowIncoming(PcStatus.Company, PcStatus.ConfigID)) return;

            /*Validate the incoming smart string, if there is an error then this procedure will throw it.*/
            string ssLogText = string.Empty;

            if (!EDIValidateSmartString(PartRev.PartNum, PartRev.RevisionNum, iSmartString, out ssLogText))
            {
                if (!String.IsNullOrEmpty(ssLogText))
                {
                    using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())//start the transaction
                    {
                        Erp.Tables.DemandLog DemandLog = new DemandLog();
                        Db.DemandLog.Insert(DemandLog);
                        DemandLog.Company = Session.CompanyID;
                        DemandLog.DemandContractNum = iDemandContractNum;
                        DemandLog.DemandHeadSeq = iDemandHeadSeq;
                        DemandLog.DemandDtlSeq = iDemandDtlSeq;
                        DemandLog.LogText = ssLogText;
                        DemandLog.LogCode = Strings.SmartStringValidation;
                        DemandLog.Action = Strings.Warning;
                        DemandLog.SysTime = Erp.Internal.Lib.DateExtensions.SecondsSinceMidnight(CompanyTime.Now());
                        DemandLog.SysDate = CompanyTime.Now().Date;
                        Db.Validate(DemandLog);
                        txScope.Complete();
                    }
                }
                return;
            }

            ConfigurationRuntimeTableset configurationRuntimeDS = new ConfigurationRuntimeTableset();
            ConfigurationSequenceTableset configurationSequenceDS = new ConfigurationSequenceTableset();
            ConfigurationSummaryTableset configurationSummaryTS = new ConfigurationSummaryTableset();

            PcConfigurationParamsRow newRow = new PcConfigurationParamsRow();
            newRow.Company = Session.CompanyID;
            newRow.PartNum = DemandDetail.PartNum;
            newRow.RevisionNum = DemandDetail.RevisionNum;
            newRow.AltMethod = string.Empty;
            newRow.ConfigID = "testing";
            newRow.UniqueID = "0/0";

            newRow.RelatedToTable = "DemandDetail";
            newRow.RelatedToSysRowID = DemandDetail.SysRowID; ;
            newRow.SourceTable = "PartRev";
            newRow.TestID = Guid.Empty;
            newRow.TestMode = "Production";
            newRow.IsTestPlan = false;
            newRow.SpecID = string.Empty;
            newRow.SpecRevNum = string.Empty;
            newRow.InspType = string.Empty;
            newRow.ForeignTableName = string.Empty;
            newRow.ForeignSysRowID = Guid.Empty;
            newRow.InSmartString = iSmartString;
            newRow.RowMod = IceRow.ROWSTATE_ADDED;

            configurationRuntimeDS.PcConfigurationParams.Add(newRow);

            configurationSequenceDS = this.PreStartConfiguration(ref configurationRuntimeDS, ref configurationSummaryTS);

            ttPcValueGrp = (from row in configurationRuntimeDS.PcValueGrp
                            select row).FirstOrDefault();
            if (ttPcValueGrp == null)
                throw new BLException(Strings.PcValueGrpNotFound);

            ttPcStruct = (from row in configurationSequenceDS.PcStruct
                          where row.StructTag == "/0"
                          && !String.IsNullOrEmpty(row.ConfigID)
                          select row).FirstOrDefault();
            if (ttPcStruct == null)
                throw new BLException(Strings.PcStructNofFoundInTS);

            PcStatus cPcStatus = FindFirstPcStatus(Session.CompanyID, ttPcStruct.SubConfigID);
            if (cPcStatus == null)
                ExceptionManager.AddBLException(new BLException(Strings.PcStatusNotFound(ttPcStruct.SubConfigID)));

            //Resolve Target Configuration Type
            RunningState state = RunningState.Update;
            if ((!String.IsNullOrEmpty(newRow.RunningStateOverride) && newRow.RunningStateOverride.Equals("Added", StringComparison.OrdinalIgnoreCase)) || (ttPcValueGrp.ConfigStatus.Equals("Added", StringComparison.OrdinalIgnoreCase)))
                state = RunningState.Added;
            else if (ttPcValueGrp.ConfigStatus.Equals("Test", StringComparison.OrdinalIgnoreCase))
                state = RunningState.Test;


            if (ttPcValueGrp.TestMode.Equals("RULES", StringComparison.OrdinalIgnoreCase) && !this.FindPcRulesSet(Session.CompanyID, ttPcStruct.ConfigID, ttPcStruct.PartNum, ttPcStruct.RevisionNum))
                throw new BLException(Strings.NoRulesToTest);

            IConfiguration configuration = PCConfiguratorResolver.Resolve(cPcStatus, state, ttPcValueGrp.TestID, newRow.IsTestPlan, newRow.SpecID, newRow.SpecRevNum);
            if (configuration == null)
                throw new BLException(Strings.UnableToResolveConfiguration(ttPcStruct.SubConfigID));

            LinqRow relatedToRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcValueGrp.RelatedToTableName, ttPcValueGrp.RelatedToSysRowID, LockHint.UpdLock);
            LinqRow sourceRow = ConfiguratorUtil.FindBySysRowID(Db, ttPcStruct.SourceTableName, ttPcStruct.SourceSysRowID, LockHint.UpdLock);
            LinqRow foreignRow = null;

            if (!String.IsNullOrWhiteSpace(newRow.ForeignTableName) && !newRow.ForeignSysRowID.Equals(Guid.Empty))
                foreignRow = ConfiguratorUtil.FindBySysRowID(Db, newRow.ForeignTableName, newRow.ForeignSysRowID, LockHint.UpdLock);

            configuration.LoadConfiguration(ttPcValueGrp, relatedToRow, sourceRow, foreignRow, ttPcStruct, configurationRuntimeDS.PcConfigurationParams[0]);
            Byte[] result = null;
            result = configuration.GetSerializedTableset();

            // Smart String:
            List<Erp.Shared.Lib.Configurator.PCKeyValuePair<string, string>> inputValues = new List<Erp.Shared.Lib.Configurator.PCKeyValuePair<string, string>>();
            foreach (var ctrl in configuration.Inputs)
            {
                PCKeyValuePair<string, string> pc = new PCKeyValuePair<string, string>();
                pc.Key = ctrl.Key;
                pc.Value = EDIGetInputValue(configuration, ctrl.Key, FindFirstPcInputs(Session.CompanyID, PcStatus.ConfigID, ctrl.Key).DataType);
                inputValues.Add(pc);
            }

            string outSmartString = string.Empty;
            internalSuggestSmartString(PcStatus.ConfigID, false, ttPcValueGrp.RelatedToTableName, ttPcValueGrp.RelatedToSysRowID, inputValues, null, ref outSmartString, ttPcStruct.SubPartNum, ttPcStruct.BasePartNum, ttPcStruct.PartNum, ttPcStruct.SubBasePartNum);
            ttPcStruct.SmartString = outSmartString;
            if (ttPcStruct.CreatePart && !ttPcStruct.PromptForPart && !ttPcStruct.PromptForAutoCreate)
                ttPcStruct.NewPartNum = outSmartString;

            // Save Configuration:
            configuration.SetSerializedTableset(result, ttPcStruct);
            List<PcValueTableset> list = new List<PcValueTableset>();
            list.Add(configuration.CurrentTableset);
            configuration.SaveConfiguration(ttPcValueGrp, ttPcStruct, configurationRuntimeDS.PcConfigurationParams[0], out bool testPassed, out string failText);

            newRow.TgtStructTag = configuration.StructTag;

            // Part Creation:
            if (ttPcStruct.CreatePart && !ttPcStruct.PromptForPart && !ttPcStruct.PromptForAutoCreate)
            {
                bool partRefresh = LibPartCreation.CreatePart(ttPcStruct, configurationRuntimeDS.PcConfigurationParams[0].RelatedToSysRowID, configurationRuntimeDS.PcConfigurationParams[0].RelatedToTable, configurationRuntimeDS.PcConfigurationParams[0].ForeignSysRowID, configurationRuntimeDS.PcConfigurationParams[0].ForeignTableName, configurationSequenceDS);

                if (partRefresh && this.ExistsPart(Session.CompanyID, ttPcStruct.NewPartNum))
                {
                    configuration.ChangePartNum(ttPcConfigurationParams.RelatedToTable);
                }
            }

            // Document Rules:
            if (configuration.StructTag.Equals("/0", StringComparison.OrdinalIgnoreCase))
            {
                using (TransactionScope txEDIDocRules = ErpContext.CreateDefaultTransactionScope())
                {
                    InvokeDocRuleEngine(PcStatus, ref configurationSequenceDS, ref configuration);
                    Db.Validate();
                    txEDIDocRules.Complete();
                }
            }
        }

        private string EDIGetInputValue(IConfiguration configuration, string inputName, string dataType)
        {
            if (configuration.Inputs[inputName] == null) return "";

            switch (dataType.ToUpperInvariant())
            {
                case "STRING":
                    return ((InputValueBound<string>)configuration.Inputs[inputName].Value).Value;
                case "DATETIME":
                    return ((InputValueBound<DateTime?>)configuration.Inputs[inputName].Value).Value.ToString();
                case "DECIMAL":
                    return ((InputValueBound<Decimal>)configuration.Inputs[inputName].Value).Value.ToString();
                case "BOOLEAN":
                    return ((InputValueBound<Boolean>)configuration.Inputs[inputName].Value).Value.ToString();
            }
            return "";
        }

        /// <summary>
        /// Validate the smart string against inputs
        /// </summary>
        /// <param name="partNum">The part num being used.</param>
        /// <param name="revisionNum">The revision num being used.</param>
        /// <param name="ipSmartString">Smart String value</param>
        /// <param name="ssLogText">Error Text String</param>
        public bool EDIValidateSmartString(string partNum, string revisionNum, string ipSmartString, out string ssLogText)
        {
            ssLogText = string.Empty;
            string tmpInputValue = string.Empty;
            int iVal = 0;
            DateTime? tmpDate = null;

            PcStatusCols cPcStatus = this.FindFirstPcStatus(Session.CompanyID, partNum, revisionNum);
            if (cPcStatus == null)
            {
                ssLogText = Strings.PcStatusNotApproved("");
                throw new BLException(ssLogText);
            }

            if (String.IsNullOrEmpty(ipSmartString))
            {
                ssLogText = Strings.EDIValidSmartStringRequired;
                throw new BLException(ssLogText);
            }

            foreach (var PcInputs_iterator in (this.SelectPcInputs(Session.CompanyID, cPcStatus.ConfigID, "")))
            {
                Erp.Tables.PcInputs PcInputs = PcInputs_iterator;

                if (PcInputs.SmartStringStart > 0)
                {
                    if (PcInputs.SmartStringEnd > ipSmartString.Length)
                    {
                        ssLogText = Strings.EDIValidSmartStringRequired;
                        throw new BLException(ssLogText);
                    }

                    iVal = (PcInputs.SmartStringEnd - PcInputs.SmartStringStart) + 1;
                    tmpInputValue = ipSmartString.Substring((PcInputs.SmartStringStart - 1), iVal);

                    switch (PcInputs.DataType.ToUpperInvariant())
                    {
                        case "BOOLEAN":
                            try
                            {
                                Convert.ToBoolean(tmpInputValue);
                            }
                            catch
                            {
                                ssLogText = Strings.EDIInvalidDateValue(PcInputs.SmartStringStart, PcInputs.SmartStringEnd, PcInputs.InputName);
                                throw new BLException(ssLogText);
                            }
                            break;
                        case "DATETIME":
                            try
                            {
                                tmpDate = Compatibility.Convert.ToDateTime(tmpInputValue.ToString());
                                if (tmpDate == null)
                                {
                                    ssLogText = Strings.EDIInvalidDateValue(PcInputs.SmartStringStart, PcInputs.SmartStringEnd, PcInputs.InputName);
                                    throw new BLException(ssLogText);
                                }
                            }
                            catch
                            {
                                ssLogText = Strings.EDIInvalidDateValue(PcInputs.SmartStringStart, PcInputs.SmartStringEnd, PcInputs.InputName);
                                throw new BLException(ssLogText);
                            }

                            if (PcInputs.StartDate != null || PcInputs.EndDate != null)
                            {
                                /*Properties take precedence over the initial setup.*/
                                if ((Convert.ToDateTime(tmpDate) >= Convert.ToDateTime(PcInputs.StartDate))
                                    && (Convert.ToDateTime(tmpDate) <= Convert.ToDateTime(PcInputs.EndDate)))
                                {
                                    ssLogText = Strings.EDIInvalidDateRange(PcInputs.SmartStringStart, PcInputs.SmartStringEnd, PcInputs.StartDate, PcInputs.EndDate);
                                    throw new BLException(ssLogText);
                                }
                            }
                            break;
                        case "DECIMAL":
                            decimal tmpDecimal;
                            decimal remainder = decimal.Zero;
                            try
                            {
                                tmpDecimal = Convert.ToDecimal(tmpInputValue);
                            }
                            catch
                            {
                                ssLogText = Strings.EDIInvalidDecimalValue(PcInputs.SmartStringStart, PcInputs.SmartStringEnd, PcInputs.InputName);
                                throw new BLException(ssLogText);
                            }

                            if (Convert.ToDecimal(PcInputs.IncrPrec) != decimal.Zero)
                            {
                                remainder = Convert.ToDecimal(tmpDecimal % Convert.ToDecimal(PcInputs.IncrPrec));
                            }
                            else remainder = decimal.Zero;

                            if (Convert.ToDecimal(PcInputs.StartDec) != decimal.Zero || Convert.ToDecimal(PcInputs.EndDec) != decimal.Zero)
                            {

                                if (!(tmpDecimal >= Convert.ToDecimal(PcInputs.StartDec)
                                    && tmpDecimal <= Convert.ToDecimal(PcInputs.EndDec)
                                    && remainder == decimal.Zero))
                                {
                                    if ((Convert.ToDecimal(PcInputs.StartDec) != decimal.Zero ||
                                        Convert.ToDecimal(PcInputs.EndDec) != decimal.Zero)
                                        && Convert.ToDecimal(PcInputs.IncrPrec) != decimal.Zero)
                                    {
                                        ssLogText = Strings.EDIDecMinMaxIncr(PcInputs.SmartStringStart, PcInputs.SmartStringEnd, PcInputs.StartDec, PcInputs.EndDec, PcInputs.IncrPrec);
                                    }
                                    else if (Convert.ToDecimal(PcInputs.IncrPrec) != decimal.Zero)
                                    {
                                        ssLogText = Strings.EDIDecIncr(PcInputs.SmartStringStart, PcInputs.SmartStringEnd, PcInputs.IncrPrec);
                                    }
                                    else ssLogText = Strings.EDIDecEnd(PcInputs.SmartStringStart, PcInputs.SmartStringEnd);

                                    throw new BLException(ssLogText);
                                }
                            }
                            else if (Convert.ToDecimal(PcInputs.IncrPrec) != decimal.Zero && remainder != decimal.Zero)
                            {
                                ssLogText = Strings.EDIDecIncr(PcInputs.SmartStringStart, PcInputs.SmartStringEnd, PcInputs.IncrPrec);
                                throw new BLException(ssLogText);
                            }
                            break;
                        case "STRING":
                            string valList = string.Empty;
                            string errMsg = string.Empty;
                            bool foundVal = false;

                            if (PcInputs.ControlType.KeyEquals("Ice.Lib.Framework.EpiUltraCombo, Ice.Lib.EpiClientLib"))
                            {
                                if (!ExistsPcDynLst(Session.CompanyID, PcInputs.ConfigID, PcInputs.InputName))
                                {
                                    valList = PcInputs.ListItems;
                                    errMsg = Strings.EDIStringListItems(PcInputs.SmartStringStart, PcInputs.SmartStringEnd, PcInputs.InputName);
                                }
                            }
                            else
                            {
                                valList = PcInputs.ValList;
                                errMsg = Strings.EDIStringValList(PcInputs.SmartStringStart, PcInputs.SmartStringEnd, PcInputs.InputName);
                            }

                            if (!String.IsNullOrEmpty(valList))
                            {
                                for (int x = 0; x < valList.NumEntries(Ice.Constants.ListSeparator); x++)
                                {
                                    if (tmpInputValue.Trim().KeyEquals(valList.Entry(x, Ice.Constants.ListSeparator).Entry(0, Ice.Constants.SubListSeparator)))
                                    {
                                        foundVal = true;
                                        break;
                                    }
                                }

                                if (foundVal == false)
                                {
                                    ssLogText = errMsg;
                                    throw new BLException(ssLogText);
                                }
                            }

                            break;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Part exists.
        /// </summary>
        /// <param name="configID"> Configuration ID</param>
        /// <param name="targetEntity">Target Entity - table name being processed by the configurator</param>
        /// <param name="groupSeq">Configuaration PcValueGrp.GroupSeq</param>
        /// <param name="basePartNum">Base Part Number</param>
        /// <param name="baseRevisionNum">Base Revision Number</param>
        /// <param name="newPartNum">The part being configured</param>
        /// <param name="mtlSeq">Material Sequence of the revision being configured</param>
        /// <param name="ruleTag">Generated Rule Tag of the part being configured</param>
        /// <param name="partExists">Part Exists</param>
        /// <param name="notUnique">instructs the UI whether or not to warn of duplicate parts</param>
        /// <param name="sIValues">Saving input values for this configuration</param>
        public void PartExists(string configID, string targetEntity, int groupSeq, string basePartNum, string baseRevisionNum, string newPartNum, int mtlSeq, string ruleTag, out bool partExists, out bool notUnique, out bool sIValues)
        {
            partExists = false;
            notUnique = false;
            sIValues = false;

            Erp.Tables.PcStatus PcStatus = this.FindFirstPcStatus(Session.CompanyID, configID);
            if (PcStatus == null)
            {
                throw new BLException(Strings.PcStatusNotFound(configID));
            }
            if (!PcStatus.CreatePart)
            {
                throw new BLException(Strings.PartCreateNotValid(configID));
            }
            PcTargetEntityCols PcTargetEntity = this.FindPcTargetEntity(Session.CompanyID, configID, targetEntity);
            if (PcTargetEntity == null)
                throw new BLException(Strings.TargetEntityNotFound(targetEntity));
            if (!PcTargetEntity.AllowRecordCreation)
                throw new BLException(Strings.PartCreateNotValidForTarget(targetEntity, configID));
            // overkill not needed just want to know if the part exists and if it does it might not have a revision
            //Erp.Tables.PartRev PartRev = this.FindFirstPartRev(Session.CompanyID, basePartNum, baseRevisionNum, configID);
            //if (PartRev == null)
            //    throw new BLException(Strings.PartRevNotValidForConfig(basePartNum, baseRevisionNum, configID));

            if (!string.IsNullOrEmpty(newPartNum))
            {
                partExists = this.ExistsPart(Session.CompanyID, newPartNum);
            }
            // scr 121377 - there was logic to set promptForSIValues but per Ronald if the PcStatus is setup to SaveSIValues then that is all we need.  The user is NOT to be prompted.
            // if it is determined at a later date that the user needs to be prompted this is where the logic goes

            // scr 123840 - using promptforsivalues for now until chris unlocks the service
            notUnique = PcStatus.NotUnique;
        }
        /// <summary>
        /// Determines if the part rev exists
        /// </summary>
        /// <param name="ipPartNum"></param>
        /// <param name="ipRevisionNum"></param>
        /// <returns></returns>
        public bool PartRevExists(string ipPartNum, string ipRevisionNum)
        {
            return this.ExistsPartRev(Session.CompanyID, ipPartNum, ipRevisionNum);
        }

        //[Serializable]
        //public struct PCKeyValue<K, V>
        //{
        //    public K Key { get; set; }
        //    public V Value { get; set; }
        //}
        //void SuggestSmartString(string configID, int groupSeq, int headNum, bool testMode, ref ConfigurationSequenceTableset ts, string ipRelatedToTable, Guid ipRelatedToSysRowID,
        //             PCKeyValue<string, string> smartStringValues, out string outSmartString)
        //public void SuggestSmartString(string configID, int groupSeq, int headNum, bool testMode, ref ConfigurationSequenceTableset ts, string ipRelatedToTable, Guid ipRelatedToSysRowID,
        //        System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> smartStringValues, out string outSmartString)
        // PCKeyValuePair<K, V>

        /// <summary>
        /// Get AllowRecordCreation and IncomingSmartString columns to process before the configuration is saved for the first time.
        /// These values were obtained but only after the configuration was saved for the first time.
        /// </summary>
        /// <param name="configID"></param>
        /// <param name="relatedToTableName"></param>
        /// <param name="allowRecordCreation"></param>
        /// <param name="useInSmartString"></param>
        public void GetTargetEntityValues(string configID, string relatedToTableName, out bool allowRecordCreation, out bool useInSmartString)
        {
            allowRecordCreation = false;
            useInSmartString = false;

            PcTargetEntityCols pc = FindPcTargetEntity(Session.CompanyID, configID, relatedToTableName);
            if (pc != null)
            {
                allowRecordCreation = pc.AllowRecordCreation;
                useInSmartString = pc.IncomingSmartString;
            }
        }

        /// <summary>
        /// Original logic to suggest a smart string.  This is called from Win client and EWA.  It cannot be called from EWC.
        /// </summary>
        /// <param name="configID">Configuration ID</param>
        /// <param name="testMode">Test Inputs true/false</param>
        /// <param name="ts">ConfigurationSequenceTableset</param>
        /// <param name="ipRelatedToTable">Target entity this call is related to</param>
        /// <param name="ipRelatedToSysRowID">SysRowID of the target entity</param>
        /// <param name="smartStringValues">Collection of current input answers from the configuration session. </param>
        /// <param name="outSmartString">result suggested smart string</param>
        /// <param name="structTag">Identifies the row in the ConfigurationSequenceTableset being processed</param>
        /// <param name="structID">Identifies the row in the ConfigurationSequenceTableset being processed</param>
        /// <returns></returns>
        public void SuggestSmartString(string configID, bool testMode, ref ConfigurationSequenceTableset ts, string ipRelatedToTable, Guid ipRelatedToSysRowID,
                    System.Collections.Generic.List<Erp.Shared.Lib.Configurator.PCKeyValuePair<string, string>> smartStringValues, out string outSmartString, string structTag, int structID)
        {
            outSmartString = string.Empty;
            PcStructRow ttPcStructRow = (from row in ts.PcStruct
                                         where !String.IsNullOrEmpty(row.RowMod) &&
                                         row.StructTag.KeyEquals(structTag) &&
                                         row.StructID == structID
                                         select row).FirstOrDefault();
            if (ttPcStructRow == null)
                throw new BLException(Strings.PcStructNofFoundInTS);
            internalSuggestSmartString(configID, testMode, ipRelatedToTable, ipRelatedToSysRowID, smartStringValues, null, ref outSmartString, ttPcStructRow.SubPartNum, ttPcStructRow.BasePartNum, ttPcStructRow.PartNum, ttPcStructRow.SubBasePartNum);
        }
        /// <summary>
        /// Call this method when you need to suggest a smart string value and you are not calling from EWA or your ConfigType is EWC.
        /// </summary>
        /// <param name="configID">Configuration ID</param>
        /// <param name="testMode">Test Inputs true/false</param>
        /// <param name="ipRelatedToTable">Target entity this call is related to</param>
        /// <param name="ipRelatedToSysRowID">The SysRowID of the target Entity</param>
        /// <param name="smartStringValues">Collection of current input values gathered so far during the configuration session.</param>
        /// <param name="outSmartString">The suggested smart string result.</param>
        /// <param name="subPartNum"></param>
        /// <param name="basePartNum"></param>
        /// <param name="partNum"></param>
        /// <param name="subBasePartNum"></param>

        public void EWCSuggestSmartString(string configID, bool testMode, string ipRelatedToTable, Guid ipRelatedToSysRowID,
                List<KeyValuePair<string, string>> smartStringValues, out string outSmartString, string subPartNum, string basePartNum, string partNum, string subBasePartNum)
        {
            outSmartString = string.Empty;
            internalSuggestSmartString(configID, testMode, ipRelatedToTable, ipRelatedToSysRowID, null, smartStringValues, ref outSmartString, subPartNum, basePartNum, partNum, subBasePartNum);
        }
        private void internalSuggestSmartString(string configID, bool testMode, string ipRelatedToTable, Guid ipRelatedToSysRowID,
               System.Collections.Generic.List<Erp.Shared.Lib.Configurator.PCKeyValuePair<string, string>> smartStringValues, List<KeyValuePair<string, string>> ewcSmartStringValues, ref string outSmartString, string subPartNum, string basePartNum, string partNum, string subBasePartNum)
        {
            string custPartNum = string.Empty;
            Erp.Tables.PcStatus PcStatus = null;
            outSmartString = string.Empty;
            PcStatus = this.FindFirstPcStatus(Session.CompanyID, configID);
            if (PcStatus == null)
                throw new BLException(Strings.PcStatusNotFound(configID));
            string suggestPart = InitializeSuggestedPart(subPartNum, basePartNum, partNum, subBasePartNum);

            if (testMode && String.IsNullOrEmpty(suggestPart))
                suggestPart = PcStatus.ConfigID;

            switch (PcStatus.StringStyle.ToUpperInvariant())
            {
                case "CONSTR":
                    outSmartString = SmartStringFromInputs(testMode, PcStatus, suggestPart, custPartNum, smartStringValues, ewcSmartStringValues);
                    break;
                case "ORDLINE":
                    outSmartString = SmartStringFromOrderLine(testMode, ipRelatedToTable, ipRelatedToSysRowID, PcStatus, suggestPart);
                    break;
                case "SEQ":
                    outSmartString = SmartStringUsingSequentialNumbering(testMode, PcStatus, suggestPart);
                    break;
                default:
                    break;
            }
        }
        internal static string InitializeSuggestedPart(string subPartNum, string basePartNum, string partNum, string subBasePartNum)
        {
            string suggestPart = string.Empty;
            if (String.IsNullOrEmpty(subPartNum))
            {
                suggestPart = !String.IsNullOrEmpty(basePartNum) ? basePartNum : partNum;
            }
            else
            {
                suggestPart = !String.IsNullOrEmpty(subBasePartNum) ? subBasePartNum : subPartNum;
            }
            return suggestPart;
        }



        /// <summary>
        /// Two different collection types are coded because of how EWA and REST pass data.  EWA uses SOAP so the PCKeyValuePair works.  However, REST cannot pass this type as the result on the server is always null.  REST can handle
        /// a generate list KVP.  Since this change is being made in a field release both types are being maintained.  Removing the PCKeyValuePair requires changes to EWA.  This is something we should consider at a later
        /// release level when PM authorizes the change.
        /// </summary>
        /// <param name="testMode">test inputs true/false</param>
        /// <param name="PcStatus">current PCStatus row obtained in calling method.  If it is null string.empty is returned forthe smart string.</param>
        /// <param name="suggestPart">start of the suggested part number</param>
        /// <param name="custPartNum">Customer Part Number</param>
        /// <param name="smartStringValues">collection of input values when not called from EWC.  Send null if EWC is making the call to this method.</param>
        /// <param name="ewcSmartStringValues">Collection of input values when not called from Win/EWA.  Send null if called from Win/EWC.</param>
        /// <returns></returns>
        private string SmartStringFromInputs(bool testMode, PcStatus PcStatus, string suggestPart, string custPartNum,
       System.Collections.Generic.List<Erp.Shared.Lib.Configurator.PCKeyValuePair<string, string>> smartStringValues, List<KeyValuePair<string, string>> ewcSmartStringValues)
        {
            string outSmartString = string.Empty;
            if (PcStatus == null) return string.Empty;
            if (PcStatus.PrefacePart)
                outSmartString = suggestPart;
            if (PcStatus.ConfigType.KeyEquals("EWC") && (ewcSmartStringValues == null || ewcSmartStringValues.Count <= 0))
            {
                return string.Empty;
            }
            else if (!PcStatus.ConfigType.KeyEquals("EWC") && (smartStringValues == null || smartStringValues.Count <= 0))
            {
                return string.Empty;
            }
            if (PcStatus.CrtCustPart && !String.IsNullOrEmpty(custPartNum))
            {
                if (String.Compare(PcStatus.Separator, "N", StringComparison.OrdinalIgnoreCase) != 0)
                    if (!String.IsNullOrEmpty(outSmartString))
                        outSmartString = outSmartString + (!String.IsNullOrEmpty(PcStatus.Separator) ? PcStatus.Separator : " ") + custPartNum;
                    else
                        outSmartString = custPartNum;
                else
                    outSmartString = outSmartString + custPartNum;
            }
            string fillStr = string.Empty;
            foreach (var PcStrCompRow in (this.FindSmartStringValues(Session.CompanyID, PcStatus.ConfigID)))
            {
                string t_formatstring = string.Empty;
                string smartStringInput;
                if (!PcStatus.ConfigType.KeyEquals("EWC"))
                {
                    smartStringInput = smartStringValues.Where(x => x.Key.KeyEquals(PcStrCompRow.CompName)).FirstOrDefault().Value;
                    if (smartStringInput == null) continue;
                }
                else
                {
                    smartStringInput = ewcSmartStringValues.Where(x => x.Key.KeyEquals(PcStrCompRow.CompName)).FirstOrDefault().Value;
                    if (smartStringInput == null) continue;
                }

                PcInputsCols pcInputsCols = this.FindFirstPcInputs(PcStrCompRow.Company, PcStatus.ConfigID, PcStrCompRow.CompName);
                if (pcInputsCols == null) continue;
                if (!String.IsNullOrEmpty(pcInputsCols.FormatString))
                {
                    if (pcInputsCols.DataType.KeyEquals("String"))
                    {
                        t_formatstring = pcInputsCols.FormatString.Substring(0, 1);
                    }
                    else
                        t_formatstring = pcInputsCols.FormatString;
                }
                switch (t_formatstring.ToUpperInvariant())
                {
                    case "A":
                        {
                            this.checkInputFormat(smartStringInput.ToString(), true, false);
                        }
                        break;
                    case "N":
                        {
                            this.checkInputFormat(smartStringInput.ToString(), true, true);
                        }
                        break;
                    case "BOOLEAN":
                        {
                            this.checkInputFormat(smartStringInput.ToString(), true, false);
                        }
                        break;
                    case "9":
                        {
                            this.checkInputFormat(smartStringInput.ToString(), false, true);
                        }
                        break;
                }
                smartStringInput = formatStartString(pcInputsCols.DataType, PcStrCompRow.DisplayFormat,
                    smartStringInput.ToString(), PcStrCompRow.SmartStringStart, PcStrCompRow.SmartStringEnd);
                if (!PcStatus.Separator.KeyEquals("N"))
                {
                    if (outSmartString.Trim().Length > 0)
                    {
                        if ((outSmartString.Length + PcStatus.Separator.Length + ((String.IsNullOrEmpty(PcStatus.Separator)) ? 1 : 0) + fillStr.Trim().Length) > 28000)
                        {
                            return outSmartString;
                        }
                        outSmartString = outSmartString + ((String.IsNullOrEmpty(PcStatus.Separator)) ? " " : PcStatus.Separator) + smartStringInput.ToString().Trim();
                    }
                    else
                    {
                        if ((outSmartString.Length + fillStr.Trim().Length) > 28000)
                        {
                            return outSmartString;
                        }
                        outSmartString = smartStringInput.ToString().Trim();
                    }
                }
                else
                {
                    if ((outSmartString.Length + fillStr.Trim().Length) > 28000)
                    {
                        return outSmartString;
                    }
                    outSmartString = outSmartString + smartStringInput.ToString().Trim();
                }

            }
            return outSmartString;
        }
        private string SmartStringUsingSequentialNumbering(bool testMode, PcStatus PcStatus, string suggestPart)
        {
            string zeroFilledNumber = string.Empty;
            string zeroFilledLine = string.Empty;
            string outSmartString = string.Empty;
            if (PcStatus == null) return string.Empty;
            if (string.Compare(PcStatus.NumberFormat, "ALL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                zeroFilledNumber = PcStatus.StartNumber.ToString("D" + 9);// 9 is based upon the 9.05 code
            }
            if (String.Compare(PcStatus.Separator, "N", StringComparison.OrdinalIgnoreCase) != 0)
            {
                if (string.Compare(PcStatus.NumberFormat, "ALL", StringComparison.OrdinalIgnoreCase) == 0)
                    outSmartString = suggestPart.Trim() + (!String.IsNullOrEmpty(PcStatus.Separator) ? PcStatus.Separator : " ") + zeroFilledNumber;
                else
                    outSmartString = suggestPart.Trim() + (!String.IsNullOrEmpty(PcStatus.Separator) ? PcStatus.Separator : " ") + PcStatus.StartNumber.ToString();
            }
            else
            {
                if (string.Compare(PcStatus.NumberFormat, "ALL", StringComparison.OrdinalIgnoreCase) == 0)
                    outSmartString = suggestPart.Trim() + zeroFilledNumber;
                else
                    outSmartString = suggestPart.Trim() + PcStatus.StartNumber.ToString();
            }
            if (!testMode)
            {
                using (TransactionScope txScope = ErpContext.CreateDefaultTransactionScope())
                {
                    Db.ReadCurrent(ref PcStatus, LockHint.UpdLock);
                    PcStatus.StartNumber++;
                    Db.Validate(PcStatus);
                    txScope.Complete();
                }
            }

            return outSmartString;
        }
        private string SmartStringFromOrderLine(bool testMode, string ipRelatedToTable, Guid ipRelatedToSysRowID, PcStatus PcStatus, string suggestPart)
        {
            string outSmartString = string.Empty;
            string zeroFilledNumber = string.Empty;
            string zeroFilledLine = string.Empty;
            int smartStringNumber = 0;
            int smartStringLine = 0;

            if (testMode)
            {
                string separator = (!String.IsNullOrEmpty(PcStatus.Separator) ? PcStatus.Separator : " ");
                zeroFilledNumber = PcStatus.StartNumber.ToString("D" + 9);
                zeroFilledLine = PcStatus.StartNumber.ToString("D" + 6);
                if (String.Compare(PcStatus.Separator, "N", StringComparison.OrdinalIgnoreCase) != 0)
                {
                    outSmartString = suggestPart.Trim() + separator + zeroFilledNumber + separator + zeroFilledLine;
                }
                else
                {
                    outSmartString = suggestPart.Trim() + zeroFilledNumber + zeroFilledLine;
                }
            }
            else
            {
                if (String.Compare(ipRelatedToTable, "ORDERDTL", StringComparison.OrdinalIgnoreCase) != 0 && String.Compare(ipRelatedToTable, "QUOTEDTL", StringComparison.OrdinalIgnoreCase) != 0)
                    outSmartString = suggestPart.Trim();
                else
                {
                    switch (ipRelatedToTable.ToUpperInvariant())
                    {
                        case "ORDERDTL":
                            OrderDetailCols orderDtlCols = this.FindOrderDtlBySysRowID(Session.CompanyID, ipRelatedToSysRowID);
                            if (orderDtlCols == null)
                                throw new BLException(Strings.OrderLineDoesNotExist);
                            smartStringNumber = orderDtlCols.OrderNum;
                            smartStringLine = orderDtlCols.OrderLine;
                            break;
                        case "QUOTEDTL":
                            QuoteDetailCols quoteDtlCols = this.FindQuoteDtlBySysRowID(Session.CompanyID, ipRelatedToSysRowID);
                            if (quoteDtlCols == null)
                                throw new BLException(Strings.QuoteDoesNotExist);
                            smartStringNumber = quoteDtlCols.QuoteNum;
                            smartStringLine = quoteDtlCols.QuoteLine;
                            break;
                        default:
                            zeroFilledLine = "";
                            zeroFilledNumber = "";
                            break;

                    }
                    if (string.Compare(PcStatus.NumberFormat, "ALL", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        zeroFilledNumber = smartStringNumber.ToString("D" + 9);// 9 is based upon the 9.05 code
                        zeroFilledLine = smartStringLine.ToString("D" + 6); // 6 is based upon the 9.05 code
                    }

                    if (String.Compare(PcStatus.Separator, "N", StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        outSmartString = suggestPart.Trim() + (!String.IsNullOrEmpty(PcStatus.Separator) ? PcStatus.Separator : " ");
                        if (string.Compare(PcStatus.NumberFormat, "ALL", StringComparison.OrdinalIgnoreCase) == 0)
                            outSmartString = outSmartString + zeroFilledNumber + (!String.IsNullOrEmpty(PcStatus.Separator) ? PcStatus.Separator : " ") + zeroFilledLine;
                        else
                            outSmartString = outSmartString + smartStringNumber + (!String.IsNullOrEmpty(PcStatus.Separator) ? PcStatus.Separator : " ") + smartStringLine;
                    }
                    else
                    {
                        outSmartString = suggestPart.Trim();
                        if (string.Compare(PcStatus.NumberFormat, "ALL", StringComparison.OrdinalIgnoreCase) == 0)
                            outSmartString = outSmartString + zeroFilledNumber + zeroFilledLine;
                        else
                            outSmartString = outSmartString + smartStringNumber + smartStringLine;
                    }
                }
            }
            return outSmartString;
        }


        private string formatStartString(string dataType, string displayFormat, string smartStringInput, int start, int end)
        {
            string displayValue = string.Empty;
            switch (dataType.ToUpperInvariant())
            {
                case "LOGICAL":
                case "BOOLEAN":
                    if (!String.IsNullOrEmpty(displayFormat))
                    {
                        string[] displayFormatArray = displayFormat.Split('/');
                        if (displayFormatArray.Length >= 2)
                        {
                            if (Convert.ToBoolean(smartStringInput))
                                displayValue = displayFormatArray[0];
                            else
                                displayValue = displayFormatArray[1];
                        }
                        else
                            displayValue = smartStringInput;
                    }
                    else
                        displayValue = smartStringInput;
                    break;
                case "DATE":
                case "DATETIME":
                    displayValue = smartStringInput;
                    if (!string.IsNullOrEmpty(displayValue) && !String.IsNullOrEmpty(displayFormat))
                    {
                        string[] formatArray = displayFormat.Split(',');
                        if (formatArray.Length == 4)
                        {
                            string formatString = string.Empty;
                            string yearFormat = string.Empty;
                            string monthFormat = string.Empty;
                            if (formatArray[2].Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                                yearFormat = "yyyy";
                            else
                                yearFormat = "yy";
                            if (formatArray[3].Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                                monthFormat = "MMM";
                            else
                                monthFormat = "MM";
                            formatString = monthFormat + formatArray[1] + "dd" + formatArray[1] + yearFormat;
                            switch (formatArray[0].ToUpperInvariant())
                            {

                                case "YM":
                                    formatString = yearFormat + formatArray[1] + monthFormat;
                                    break;
                                case "MY":
                                    formatString = monthFormat + formatArray[1] + yearFormat;
                                    break;
                                case "DM":
                                    formatString = "dd" + formatArray[1] + monthFormat;
                                    break;
                                case "YO":
                                    formatString = yearFormat;
                                    break;
                                case "MO":
                                    formatString = monthFormat;
                                    break;
                                case "DO":
                                    formatString = "dd";
                                    break;
                                default:
                                    formatString = monthFormat + formatArray[1] + "dd" + formatArray[1] + yearFormat;
                                    break;
                            }

                            displayValue = Convert.ToDateTime(displayValue).ToString(formatString);
                        }
                    }
                    break;
                case "CHARACTER":
                case "STRING":
                case "DECIMAL":
                case "INTEGER":
                    displayValue = smartStringInput;
                    break;
            }

            if (start + end >= 0)
            {
                int calculatedLength = end - start + 1;
                if (calculatedLength >= displayValue.Length)
                {
                    calculatedLength = displayValue.Length - start;
                }
                if (calculatedLength < 0) calculatedLength = 0;
                if (displayValue.Length > start)
                    displayValue = displayValue.Substring(start, calculatedLength);
                else
                    displayValue = string.Empty;  // starting position is greater than the amount of data entered by the user.  Therefore there is nothing to display.
            }
            return displayValue;
        }

        ///<summary>
        ///  Purpose: Check the input format against what is in PcInputs.FormatString
        ///  Parameters:  none
        ///  Notes:
        ///</summary>
        private void checkInputFormat(string smartstringValueNode, bool CheckLetters, bool CheckNumbers)
        {

            string CheckList = string.Empty;
            int iCnt = 1;
            string cLetter = string.Empty;
            string errMessage = string.Empty;
            string mString = string.Empty;
            if (CheckLetters)
            {
                CheckList = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
                if (!(CheckNumbers))
                {
                    mString = Strings.MustBeALetter;
                }
                else
                {
                    mString = Strings.MustBeALetterOrDigit;
                }
            }
            if (CheckNumbers)
            {
                if (!String.IsNullOrEmpty(CheckList))
                {
                    CheckList = CheckList + ",";
                }

                CheckList = CheckList + "0,1,2,3,4,5,6,7,8,9";
                if (!(CheckLetters))
                {
                    mString = Strings.MustBeADigit;
                }
            }
            /* see if any commas in list */
            if ((smartstringValueNode.IndexOf(",", 0, StringComparison.OrdinalIgnoreCase) + 1) != 0)
            {
                iCnt = (smartstringValueNode.IndexOf(",", StringComparison.OrdinalIgnoreCase) + 1);
                errMessage = Strings.InputPositionCharacter(mString, smartstringValueNode, iCnt, ",");
                ExceptionManager.AddBLException(errMessage);
            }
            /* check each individual character in inputvalue */
            while (iCnt <= smartstringValueNode.Length)
            {
                smartstringValueNode = Compatibility.Convert.ToString(smartstringValueNode);
                cLetter = smartstringValueNode.SubString(iCnt - 1, 1);
                if ((CheckList.IndexOf(cLetter, StringComparison.OrdinalIgnoreCase) + 1) == 0)
                {
                    errMessage = Strings.InputPositionCharacter(mString, smartstringValueNode, iCnt, cLetter);
                    ExceptionManager.AddBLException(errMessage);
                }
                iCnt = iCnt + 1;
            }
            /* Check if any exceptions were thrown */
            ExceptionManager.AssertNoBLExceptions();
        }


        ///<summary>
        ///  Purpose: Adds a row and populates the string value in the User Defined Table used to pass parameters
        ///  from the client to the server for Server Side UDMethods.
        /// <param name="methodName">The name of the server side UDmethod the parameter is being added for.</param>
        /// <param name="parameterName">The name of the Parameter being added.</param>
        /// <param name="configID">The ID of the configurator the UDmethod is tied to.</param>
        /// <param name="paramSeq">The position the parameter is in the signature of the UDMethod.</param>
        /// <param name="newValue">This holds the string value being passed for string parameters.</param>
        /// <param name="pcValueDS">The name of the server side UDmethod parameter is being added for.</param>
        ///</summary>
        public void AddUserDenfinedParameterString(string methodName, string parameterName, string configID,
                                                   int paramSeq, string newValue, ref PcValueTableset pcValueDS)
        {
            PcUserDefinedMethodParametersRow newRow = new PcUserDefinedMethodParametersRow();

            newRow.Company = Session.CompanyID;
            newRow.ConfigID = configID;
            newRow.FunctionName = methodName;
            newRow.ParameterName = parameterName;
            newRow.ParamType = "string";
            newRow.ParamSeq = paramSeq;
            newRow.StringValue = newValue;
            newRow.RowMod = IceRow.ROWSTATE_ADDED;

            pcValueDS.PcUserDefinedMethodParameters.Add(newRow);
        }

        ///<summary>
        ///  Purpose: Adds a row and populates the int value in the User Defined Table used to pass parameters
        ///  from the client to the server for Server Side UDMethods.
        /// <param name="methodName">The name of the server side UDmethod the parameter is being added for.</param>
        /// <param name="parameterName">The name of the Parameter being added.</param>
        /// <param name="configID">The ID of the configurator the UDmethod is tied to.</param>
        /// <param name="paramSeq">The position the parameter is in the signature of the UDMethod.</param>
        /// <param name="newValue">This holds the int value being passed for string parameters.</param>
        /// <param name="pcValueDS">The name of the server side UDmethod parameter is being added for.</param>
        ///</summary>
        public void AddUserDenfinedParameterInt(string methodName, string parameterName, string configID,
                                                int paramSeq, int newValue, ref PcValueTableset pcValueDS)
        {
            PcUserDefinedMethodParametersRow newRow = new PcUserDefinedMethodParametersRow();

            newRow.Company = Session.CompanyID;
            newRow.ConfigID = configID;
            newRow.FunctionName = methodName;
            newRow.ParameterName = parameterName;
            newRow.ParamType = "int";
            newRow.ParamSeq = paramSeq;
            newRow.IntValue = newValue;
            newRow.RowMod = IceRow.ROWSTATE_ADDED;

            pcValueDS.PcUserDefinedMethodParameters.Add(newRow);
        }

        ///<summary>
        ///  Purpose: Adds a row and populates the decimal value in the User Defined Table used to pass parameters
        ///  from the client to the server for Server Side UDMethods.
        /// <param name="methodName">The name of the server side UDmethod the parameter is being added for.</param>
        /// <param name="parameterName">The name of the Parameter being added.</param>
        /// <param name="configID">The ID of the configurator the UDmethod is tied to.</param>
        /// <param name="paramSeq">The position the parameter is in the signature of the UDMethod.</param>
        /// <param name="newValue">This holds the decimal value being passed for string parameters.</param>
        /// <param name="pcValueDS">The name of the server side UDmethod parameter is being added for.</param>
        ///</summary>
        public void AddUserDenfinedParameterDecimal(string methodName, string parameterName, string configID,
                                                int paramSeq, decimal newValue, ref PcValueTableset pcValueDS)
        {
            PcUserDefinedMethodParametersRow newRow = new PcUserDefinedMethodParametersRow();

            newRow.Company = Session.CompanyID;
            newRow.ConfigID = configID;
            newRow.FunctionName = methodName;
            newRow.ParameterName = parameterName;
            newRow.ParamType = "decimal";
            newRow.ParamSeq = paramSeq;
            newRow.DecimalValue = newValue;
            newRow.RowMod = IceRow.ROWSTATE_ADDED;

            pcValueDS.PcUserDefinedMethodParameters.Add(newRow);
        }

        ///<summary>
        ///  Purpose: Adds a row and populates the DateTime value in the User Defined Table used to pass parameters
        ///  from the client to the server for Server Side UDMethods.
        /// <param name="methodName">The name of the server side UDmethod the parameter is being added for.</param>
        /// <param name="parameterName">The name of the Parameter being added.</param>
        /// <param name="configID">The ID of the configurator the UDmethod is tied to.</param>
        /// <param name="paramSeq">The position the parameter is in the signature of the UDMethod.</param>
        /// <param name="newValue">This holds the DateTime value being passed for string parameters.</param>
        /// <param name="pcValueDS">The name of the server side UDmethod parameter is being added for.</param>
        ///</summary>
        public void AddUserDenfinedParameterDateTime(string methodName, string parameterName, string configID,
                                                int paramSeq, DateTime? newValue, ref PcValueTableset pcValueDS)
        {
            PcUserDefinedMethodParametersRow newRow = new PcUserDefinedMethodParametersRow();

            newRow.Company = Session.CompanyID;
            newRow.ConfigID = configID;
            newRow.FunctionName = methodName;
            newRow.ParameterName = parameterName;
            newRow.ParamType = "DateTime";
            newRow.ParamSeq = paramSeq;
            newRow.DateTimeValue = newValue;
            newRow.RowMod = IceRow.ROWSTATE_ADDED;

            pcValueDS.PcUserDefinedMethodParameters.Add(newRow);
        }

        ///<summary>
        ///  Purpose: Adds a row and populates the bool value in the User Defined Table used to pass parameters
        ///  from the client to the server for Server Side UDMethods.
        /// <param name="methodName">The name of the server side UDmethod the parameter is being added for.</param>
        /// <param name="parameterName">The name of the Parameter being added.</param>
        /// <param name="configID">The ID of the configurator the UDmethod is tied to.</param>
        /// <param name="paramSeq">The position the parameter is in the signature of the UDMethod.</param>
        /// <param name="newValue">This holds the bool value being passed for string parameters.</param>
        /// <param name="pcValueDS">The name of the server side UDmethod parameter is being added for.</param>
        ///</summary>
        public void AddUserDenfinedParameterBool(string methodName, string parameterName, string configID,
                                                     int paramSeq, bool newValue, ref PcValueTableset pcValueDS)
        {
            PcUserDefinedMethodParametersRow newRow = new PcUserDefinedMethodParametersRow();

            newRow.Company = Session.CompanyID;
            newRow.ConfigID = configID;
            newRow.FunctionName = methodName;
            newRow.ParameterName = parameterName;
            newRow.ParamType = "bool";
            newRow.ParamSeq = paramSeq;
            newRow.BoolValue = newValue;
            newRow.RowMod = IceRow.ROWSTATE_ADDED;

            pcValueDS.PcUserDefinedMethodParameters.Add(newRow);
        }

        ///<summary>
        ///  Purpose: Clears the Rows in PcUserDefinedMethodParameters.  Rows should be cleared before and after calling
        ///  a server side UDMethod.
        ///</summary>
        public void ClearUDMethodParams(ref PcValueTableset pcValueDS)
        {
            pcValueDS.PcUserDefinedMethodParameters.Clear();
        }

        /// <summary>
        /// Used to generate Image Layer script code for the given Image Layer ID
        /// </summary>
        /// <param name="imageLayerID">The Image Layer ID to be used for script generation</param>
        /// <returns>Returns the generated script code to be used by an EpiEOWebBrowser control.</returns>
		public string ExecuteGenerateImageLayerScriptCode(string imageLayerID)
        {
            string scriptCode = string.Empty;
            using (ImageLayerEngine imageLayerEngine = new ImageLayerEngine(Db))
            {
                scriptCode = imageLayerEngine.GenerateImageLayerScriptCode(imageLayerID);
            }
            return scriptCode;
        }

        /// <summary>
        /// Used to generate Image Layer script code for the given Image Layer ID
        /// </summary>
        /// <param name="imageLayerID">The Image Layer ID to be used for script generation</param>
        /// <param name="zIndex">The Zindex value to be used for script generation. ZIndex determines the order in which the layers are displayed.</param>
        /// <param name="imageValue">The image to be used on script generation.</param>
        /// <param name="layerSeq">The layer index that belongs to the modified layer.</param>
        /// <returns>Returns the generated script code to be used by an EpiEOWebBrowser control.</returns>
        public string ExecuteGenerateSingleImageLayerScriptCode(string imageLayerID, int zIndex, string imageValue, int layerSeq)
        {
            string scriptCode = string.Empty;
            using (ImageLayerEngine imageLayerEngine = new ImageLayerEngine(Db))
            {
                scriptCode = imageLayerEngine.SingleLayerScriptCode(imageLayerID, zIndex, imageValue, layerSeq);
            }
            return scriptCode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="imageLayersInfo"></param>
        /// <returns></returns>
        //public string ExecuteGenerateFullImageLayerScriptCode(Dictionary<int, Tuple<string, int, string>> imageLayersInfo)
        public string ExecuteGenerateFullImageLayerScriptCode(List<KeyValuePair<string, string>> imageLayersInfo)
        {
            string scriptCode = string.Empty;
            using (ImageLayerEngine imageLayerEngine = new ImageLayerEngine(Db))
            {
                scriptCode = imageLayerEngine.GenerateFullImageLayerScriptCode(imageLayersInfo);
            }
            return scriptCode;
        }
        /// <summary>
        /// Need to execute this method to execute server-side UDmethods that will return an array of objects.
        /// </summary>
        /// <param name="methodName">The name of the server-side UDmethod to execute.</param>
        /// <param name="configID">The ID of the configurator the UDmethod is tied to.</param>
        /// <param name="testID">When executing under a test (Test Inputs/Rules) this parameter should contain a valid <see cref="System.Guid"/>, otherwise it can be <see cref="Guid.Empty"/></param>
        /// <param name="pcValueDS">Values from  current configurator.</param>
        /// <returns></returns>
        public object[] ExecuteUserDefinedWithArrayReturn(string methodName, string configID, Guid testID, ref PcValueTableset pcValueDS)
        {
            return ExecuteUserDefined(methodName, configID, testID, ref pcValueDS) as object[];
        }
        ///<summary>
        /// Need to execute this method to execute server side UDmethods from the client out other API.
        /// <param name="methodName">The name of the server-side UDmethod to execute.</param>
        /// <param name="configID">The ID of the configurator the UDmethod is tied to.</param>
        /// <param name="testID">When executing under a test (Test Inputs/Rules) this parameter should contain a valid <see cref="System.Guid"/>, otherwise it can be <see cref="Guid.Empty"/></param>
        /// <param name="pcValueDS">Values from  current configurator.</param>
        ///</summary>
        public object ExecuteUserDefined(string methodName, string configID, Guid testID, ref PcValueTableset pcValueDS)
        {
            PcStatus PcStatus = null;
            if (testID == Guid.Empty && pcValueDS != null && pcValueDS.PcValueHead != null && pcValueDS.PcValueHead.Count > 0)
                PcStatus = PcStatusCache.Cache.GetCache(new PcStatusCacheKey(Session.CompanyID, configID, pcValueDS.PcValueHead[0].ConfigVersion));
            else
                PcStatus = FindFirstPcStatus(Session.CompanyID, configID);


            var state = testID == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testID, false, string.Empty, string.Empty);

            configuration.SetTransformedTableset(pcValueDS);

            object[] outParams = new object[pcValueDS.PcUserDefinedMethodParameters.Count()];

            object UDMehtodObject = configuration.OnExecuteUserDefined(methodName, ref outParams, configuration.CreateUDParamObject(methodName));

            configuration.UpdateUDParams(outParams, methodName);

            pcValueDS = configuration.GetTransformedTableset();

            return UDMehtodObject;
        }


        ///<summary>
        ///  Purpose: Executes DataLookup functiosn from the client side.
        ///</summary>
        [Obsolete]
        public string ExecuteDataLookup(string methodName, string configId, Guid testId, params string[] inParams)
        {
            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, configId);
            var state = testId == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testId, false, string.Empty, string.Empty);
            return configuration.OnExecuteDataLookup(methodName, inParams);
        }
        ///<summary>
        ///  Purpose: Call from the client to execute Page OnLoad events.
        /// <param name="pageLoadEvent">The name of the page load event wanting to execute.</param>
        /// <param name="configID">The ID of the configurator the UDmethod is tied to.</param>
        /// <param name="testID"></param>
        /// <param name="pcValueDS"></param>
        ///</summary>
        public void ExecutePageOnLoadEvents(string pageLoadEvent, string configID, Guid testID, ref PcValueTableset pcValueDS)
        {
            PcStatus PcStatus = null;
            if (testID == Guid.Empty && pcValueDS != null && pcValueDS.PcValueHead != null && pcValueDS.PcValueHead.Count > 0)
                PcStatus = PcStatusCache.Cache.GetCache(new PcStatusCacheKey(Session.CompanyID, configID, pcValueDS.PcValueHead[0].ConfigVersion));
            else
                PcStatus = FindFirstPcStatus(Session.CompanyID, configID);


            var state = testID == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testID, false, string.Empty, string.Empty);

            configuration.SetTransformedTableset(pcValueDS);
            configuration.OnExecutePageOnLoadEvents(pageLoadEvent);
            pcValueDS = configuration.GetTransformedTableset();

        }
        /// <summary>
        /// GetPictureBoxImage
        /// </summary>
        public byte[] GetPictureBoxImage(string fileName, string inputName)
        {
            byte[] image = FindFirstImage(Session.CompanyID, fileName);
            RuntimeImages.RemoveAll(x => x.Key.KeyEquals(inputName));
            PCKeyValuePair<string, byte[]> item = new PCKeyValuePair<string, byte[]>();
            item.Key = inputName;
            item.Value = image;
            RuntimeImages.Add(item);

            return image;
        }

        /// <summary>
        /// Retrieve all picture box images and 2D Viewer drawings in one trip to the server and send the data back in a tableset
        /// </summary>
        /// <param name="inputImageList">List of images used in the configurator</param>
        /// <param name="ConfigID">Configurator ID to retrieve the drawings and images</param>
        /// <param name="GroupSeq">Group Sequence for the reconfigure scenarios</param>
        /// <param name="HeadNum">Head Number that represents the configuration in the structure of a multi level configurator</param>
        /// <returns>Tableset containing the rows of images</returns>
        public PcImagesTableset GetAllImages(List<KeyValuePair<string, string>> inputImageList, string ConfigID, int GroupSeq, int HeadNum)
        {
            PcImagesTableset ds = new PcImagesTableset();
            internalGetAllImages(inputImageList, (inputImageValue) => this.FindFirstImage(Session.CompanyID, inputImageValue), ds, Session.CompanyID, ConfigID);
            return ds;
        }

        internal static void internalGetAllImages(
           List<KeyValuePair<string, string>> inputImageList,
           Func<string, byte[]> findFirstImage, PcImagesTableset ds, string CompanyID, string ConfigID)
        {
            foreach (var inputImage in inputImageList)
            {
                PcImagesRow pcImages = new PcImagesRow();
                ds.PcImages.Add(pcImages);
                pcImages.Company = CompanyID;
                pcImages.ConfigID = ConfigID;
                pcImages.ImageID = inputImage.Key;
                pcImages.Content = findFirstImage(inputImage.Value);
                pcImages.RowMod = "A";
                pcImages.SysRowID = Guid.NewGuid();
            }
        }


        /// <summary>
        /// Retrieves the images for a page in a configurator
        /// </summary>
        /// <param name="inputImageList">list of images for the current panel in the configuration.  The key is the input name.  The value is the image name.</param>
        /// <returns></returns>
        public List<KeyValuePair<string, byte[]>> GetAllPictureBoxImages(List<KeyValuePair<string, string>> inputImageList)
        {

            List<KeyValuePair<string, byte[]>> imageResultsList = new List<KeyValuePair<string, byte[]>>();
            imageResultsList = internalGetAllPictureBoxImages(
             inputImageList,
             (inputImageValue) => this.FindFirstImage(Session.CompanyID, inputImageValue)
             );

            return imageResultsList;
        }


        internal static List<KeyValuePair<string, byte[]>> internalGetAllPictureBoxImages(
            List<KeyValuePair<string, string>> inputImageList,
            Func<string, byte[]> findFirstImage)
        {
            List<KeyValuePair<string, byte[]>> imageResultsList = new List<KeyValuePair<string, byte[]>>();
            foreach (var inputImage in inputImageList)
            {
                imageResultsList.Add(new KeyValuePair<string, byte[]>(inputImage.Key, findFirstImage(inputImage.Value)));
            }
            return imageResultsList;
        }



        /// <summary>
        /// GetNewPcConfigParams
        /// </summary>
        public void GetNewPcConfigParams(ref ConfigurationRuntimeTableset RuntimeDS, string configID, string uniqueID)
        {
            PcConfigurationParamsRow newRow = new PcConfigurationParamsRow();
            newRow.RowMod = IceRow.ROWSTATE_ADDED;
            newRow.Company = Session.CompanyID;
            newRow.UniqueID = uniqueID;
            newRow.EWCConfiguratorURL = this.GetEWConfiguratorURL(Session.CompanyID);
            RuntimeDS.PcConfigurationParams.Add(newRow);
        }
        /// <summary>
        /// GetNewPcInputsPublishToDoc
        /// </summary>
        public void GetNewPcInputsPublishToDoc(ref ConfigurationRuntimeTableset RuntimeDS, string key)
        {
            PcInputsPublishToDocParamsRow newRow = new PcInputsPublishToDocParamsRow();
            newRow.RowMod = IceRow.ROWSTATE_ADDED;
            newRow.Company = Session.CompanyID;
            newRow.Key = key;
            RuntimeDS.PcInputsPublishToDocParams.Add(newRow);
        }

        #region Datalookup Logic
        /// <summary>
        /// DataColumnLookupList
        /// </summary>
        /// <param name="configId">The configuration ID</param>
        /// <param name="testId">The test ID</param>
        /// <param name="lookupTblID">The lookup table name</param>
        /// <param name="colName">The column name</param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public string DataColumnLookupList(string configId, Guid testId, string lookupTblID, string colName, string searchValue)
        {
            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, configId);
            var state = testId == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testId, false, string.Empty, string.Empty);
            return configuration.OnDataColumnLookupList(lookupTblID, colName, searchValue);
        }
        /// <summary>
        /// DataColumnList
        /// </summary>
        /// <param name="configId">The configuration ID</param>
        /// <param name="testId">The test ID</param>
        /// <param name="lookupTableID">The lookup table name</param>
        /// <param name="colName">The column name</param>
        /// <returns></returns>
        public string DataColumnList(string configId, Guid testId, string lookupTableID, string colName)
        {
            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, configId);
            var state = testId == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testId, false, string.Empty, string.Empty);
            return configuration.OnDataColumnList(lookupTableID, colName);
        }
        /// <summary>
        /// DataColumnListNum
        /// </summary>
        /// <param name="configId">The configuration ID</param>
        /// <param name="testId">The test ID</param>
        /// <param name="lookupTableID">The lookup table name</param>
        /// <param name="colName">The column name</param>
        /// <returns></returns>
        public string DataColumnListNum(string configId, Guid testId, string lookupTableID, string colName)
        {
            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, configId);
            var state = testId == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testId, false, string.Empty, string.Empty);
            return configuration.OnDataColumnListNum(lookupTableID, colName);
        }
        /// <summary>
        /// DataColumnRange
        /// </summary>
        /// <param name="configId">The configuration ID</param>
        /// <param name="testId">The test ID</param>
        /// <param name="lookupTableID">The lookup table name</param>
        /// <param name="colName">The column name</param>
        /// <param name="startRow">The Start row</param>
        /// <param name="endRow">The end row</param>
        /// <returns></returns>
        public string DataColumnRange(string configId, Guid testId, string lookupTableID, string colName, string startRow, string endRow)
        {
            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, configId);
            var state = testId == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testId, false, string.Empty, string.Empty);
            return configuration.OnDataColumnRange(lookupTableID, colName, startRow, endRow);
        }
        /// <summary>
        /// DataRowList
        /// </summary>
        /// <param name="configId">The configuration ID</param>
        /// <param name="testId">The test ID</param>
        /// <param name="lookupTableID">The lookup table name</param>
        /// <param name="rowName">The row name</param>
        /// <returns></returns>
        public string DataRowList(string configId, Guid testId, string lookupTableID, string rowName)
        {
            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, configId);
            var state = testId == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testId, false, string.Empty, string.Empty);
            return configuration.OnDataRowList(lookupTableID, rowName);
        }
        /// <summary>
        /// DataRowListNum
        /// </summary>
        /// <param name="configId">The configuration ID</param>
        /// <param name="testId">The test ID</param>
        /// <param name="lookupTableID">The lookup table name</param>
        /// <param name="rowName">The row name</param>
        /// <returns></returns>
        public string DataRowListNum(string configId, Guid testId, string lookupTableID, string rowName)
        {
            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, configId);
            var state = testId == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testId, false, string.Empty, string.Empty);
            return configuration.OnDataRowListNum(lookupTableID, rowName);
        }
        /// <summary>
        /// DataRowRange
        /// </summary>
        /// <param name="configId">The configuration ID</param>
        /// <param name="testId">The test ID</param>
        /// <param name="lookupTableID">The lookup table name</param>
        /// <param name="rowName">The row name</param>
        /// <param name="startCol">The start column</param>
        /// <param name="endCol">The end column.</param>
        /// <returns></returns>
        public string DataRowRange(string configId, Guid testId, string lookupTableID, string rowName, string startCol, string endCol)
        {
            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, configId);
            var state = testId == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testId, false, string.Empty, string.Empty);
            return configuration.OnDataRowRange(lookupTableID, rowName, startCol, endCol);
        }
        /// <summary>
        /// DataRowLookup
        /// </summary>
        /// <param name="configId">The configuration ID</param>
        /// <param name="testId">The test ID</param>
        /// <param name="lookupTblID">The lookup table name</param>
        /// <param name="rowName">The row name</param>
        /// <param name="searchValue">The value to search</param>
        /// <returns></returns>
        public string DataRowLookup(string configId, Guid testId, string lookupTblID, string rowName, string searchValue)
        {
            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, configId);
            var state = testId == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testId, false, string.Empty, string.Empty);
            return configuration.OnDataRowLookup(lookupTblID, rowName, searchValue);
        }
        /// <summary>
        /// DataLookup
        /// </summary>
        /// <param name="configId">The configuration ID</param>
        /// <param name="testId">The test ID</param>
        /// <param name="lookupTblID">The lookup table name</param>
        /// <param name="rowName">The row name</param>
        /// <param name="colName">The column name</param>
        /// <returns></returns>
        public string DataLookup(string configId, Guid testId, string lookupTblID, string rowName, string colName)
        {
            PcStatus PcStatus = FindFirstPcStatus(Session.CompanyID, configId);
            var state = testId == Guid.Empty ? RunningState.Update : RunningState.Test;
            IConfiguration configuration = PCConfiguratorResolver.Resolve(PcStatus, state, testId, false, string.Empty, string.Empty);
            return configuration.OnDataLookup(lookupTblID, rowName, colName);
        }
        #endregion Datalookup Logic

        #region EWC logic
        /// <summary>
        /// Method to initialize the EWC runtime site files for the specific config ID.
        /// </summary>
        /// <param name="ConfigId">The ConfigID to run</param>
        /// <param name="RelatedToTable">The related to table name</param>
        /// <param name="RelatedToRowID">The related to SysRowID</param>
        /// <param name="PartNum">The part number to configure</param>
        /// <param name="PartRev">The revision to configure</param>
        /// <param name="ECCUser">The encrypted ECC user</param>
        /// <param name="ECCPwd">The encrypted ECC password</param>
        /// <param name="ReturnURL">The return URL</param>
        /// <param name="TestInputMode">True of running test inputs, else false for runtime</param>
        /// <param name="AccessToken">Token for the current user session.</param>
        /// <param name="ExpiresIn">Seconds until token expires</param>
        /// <returns>Path to index.html</returns>
        public string EWCInitializeRuntime(string ConfigId,
            string RelatedToTable,
            Guid RelatedToRowID,
            string PartNum,
            string PartRev,
            string ECCUser,
            string ECCPwd,
            string ReturnURL,
            bool TestInputMode,
            out string AccessToken,
            out int ExpiresIn
            )
        {
            AccessToken = "";
            ExpiresIn = 0;

            var tokenService = ServiceRenderer.GetService<TokenServiceImpl>(Db);
            var tokenDS = tokenService.GetAccessToken(Guid.Empty, "", "");
            if (tokenDS != null && tokenDS.TokenService.Rows.Count > 0)
            {
                AccessToken = tokenDS.TokenService[0].AccessToken;
                ExpiresIn = tokenDS.TokenService[0].ExpiresIn;
            }

            string decryptedUser = string.IsNullOrWhiteSpace(ECCUser) ? "" : decryptECCString(ECCUser);
            string decryptedPwd = string.IsNullOrWhiteSpace(ECCPwd) ? "" : decryptECCString(ECCPwd);

            ServicePointManager.ServerCertificateValidationCallback += (_sender, cert, chain, sslPolicyErrors) => true;

            string ScriptFolderName = GetUniqueConfigString(Session.TenantID, Session.CompanyID, ConfigId);

            string ScriptFolderPath = Path.Combine(Epicor.Utility.SpecialPath.DeploymentServerDirectory, @"Apps\EWC\Scripts", ScriptFolderName);

            Directory.CreateDirectory(ScriptFolderPath);  // if exists then it will not be created
            DateTime zipCreationTime = DateTime.UtcNow.AddYears(-100);

            string zipFileName = ScriptFolderName + ".zip";
            string zipFilePath = Path.Combine(ScriptFolderPath, zipFileName);

            if (File.Exists(zipFilePath))
            {
                zipCreationTime = File.GetLastWriteTimeUtc(zipFilePath);
            }
            byte[] fileContent;
            using (var runtimeSvc = ServiceRenderer.GetService<Erp.Services.BO.ConfigurationRuntimeSvc>(Db))
            {
                fileContent = runtimeSvc.EWCReadAllBytesIfNewer(zipFileName, zipCreationTime);
            }
            if (fileContent != null && fileContent.Length > 0)
            {
                File.WriteAllBytes(zipFilePath, fileContent);
                FastZip zip = new FastZip();
                zip.ExtractZip(zipFilePath, ScriptFolderPath, ICSharpCode.SharpZipLib.Zip.FastZip.Overwrite.Always, null, null, null, true);
            }
            string tenantIDwUnderscore = !string.IsNullOrEmpty(Session.TenantID) ? Session.TenantID + "_" : "";

            string path = Session.AppServerURL + $"/Apps/EWC/{ScriptFolderName}/index.html";

            return path;
        }

        internal static string GetUniqueConfigString(string tenantId, string companyId, string configId, string delimeter = "_")
        {
            string uniqueConfigId = configId;
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                uniqueConfigId = companyId + delimeter + uniqueConfigId;
            }
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                uniqueConfigId = tenantId + delimeter + uniqueConfigId;
            }

            return uniqueConfigId.ToLowerInvariant();
        }

        internal string getConfigIdFromECC(string configName)
        {
            if (string.IsNullOrWhiteSpace(configName))
            {
                return "";
            }
            if (!configName.Contains("."))
            {
                return configName;
            }
            var comps = configName.Split('.');
            return comps[comps.Length - 1];
        }

        internal String decryptECCString(String input)
        {
            Byte[] Key = new Byte[] { 88, 241, 5, 53, 220, 134, 129, 28, 59, 121, 138, 209, 220, 222, 197, 106, 33, 143, 139, 84, 209, 60, 99, 44, 171, 228, 182, 251, 173, 24, 109, 119 };
            Byte[] IV = new Byte[] { 120, 28, 75, 133, 229, 128, 38, 221, 164, 43, 246, 230, 75, 41, 210, 84 };
            byte[] cypher = Convert.FromBase64String(input);
            var sRet = "";
            using (var sAF = SymmetricAlgorithmFactory.Create(SymmetricCryptoAlgorithm.Aes))
            {
                //     var sAF = SymmetricAlgorithmFactory.Create(SymmetricCryptoAlgorithm.Aes);
                sAF.Mode = System.Security.Cryptography.CipherMode.CBC;
                sAF.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                using (MemoryStream msDecrypt = new MemoryStream(cypher))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, sAF.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            sRet = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return sRet;
        }

        /// <summary>
        /// Return the EWC Runtime files from the FileStore ConfigID.zip for the current Tenant and Company.
        /// </summary>
        /// <param name="FileName">The EWC runtime file name</param>
        /// <param name="LastModifiedUTCDateTime">Only return the file if the UTC DateTime is greater than this parameter</param>
        /// <returns>The file as a byte array</returns>
        public byte[] EWCReadAllBytesIfNewer(string FileName, DateTime LastModifiedUTCDateTime)
        {
            byte[] fileBytes = null;
            var fileStoreColumns = SelectFileStoreSysRowIDIfNewer(Session.TenantID, Session.CompanyID, FileName, LastModifiedUTCDateTime);

            if (fileStoreColumns != null)
            {
                using (var fileStore = ServiceRenderer.GetService<Ice.Contracts.FileStoreSvcContract>(Db))
                {
                    fileBytes = fileStore.ReadAllBytes(fileStoreColumns.SysRowID, out FileName);
                }
            }

            return fileBytes;

        }

        /// <summary>
        /// Return the Tenant ID from the current Company.
        /// </summary>
        /// <param name="company">company</param>
        /// <returns>Tenant ID</returns>
        public string GetTenantID(string company)
        {
            //SysCompanyCache this store in cache the details from Company, as the TenantID is not commonly changed it is used the info from cache
            return SysCompanyCache.GetCompanyByCompanyID(company).TenantID;
        }

        /// <summary>
        /// Write or create the byte array to the FileStore for the current company, tenant.
        /// </summary>
        /// <param name="FileName">The name of the file.</param>
        /// <param name="Bytes">The file contents</param>
        public void EWCWriteOrCreateAllBytes(string FileName, byte[] Bytes)
        {
            var fileStoreColResult = SelectFileStoreQuery(Session.TenantID, Session.CompanyID, FileName);
            if (fileStoreColResult == null)
            {
                using (var fileStore = ServiceRenderer.GetService<Ice.Contracts.FileStoreSvcContract>(Db))
                {
                    fileStore.Create(Bytes, new Guid(), "", "", FileName, Session.CompanyID, Session.TenantID, "");
                }
            }
            else
            {
                using (var fileStore = ServiceRenderer.GetService<Ice.Contracts.FileStoreSvcContract>(Db))
                {
                    fileStore.WriteAllBytes(fileStoreColResult.SysRowID, Bytes);
                }
            }

        }

        /// <summary>
        /// Returns the configuration specific data in the transport table for use in Question and Answer sessions not controlled by the
        /// E10 client side run time engine.  This must be called once for every configurator involved in a configuration session.
        /// </summary>
        /// <param name="configurationRuntime">Contains the values needed to retrieve the transport tableset.  For a quote or order line the
        /// minimum values needed are configID, relatedToTableName, relatedToSysRowID, sourceTablename, sourceSysRowID</param>
        /// <param name="configurationSequenceTableSet"></param>
        /// <returns></returns>
        public PcValueTableset GetPCTransportTableset(ref ConfigurationRuntimeTableset configurationRuntime, ref ConfigurationSequenceTableset configurationSequenceTableSet)
        {
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedConfigurator))
                throw new BLException(GlobalStrings.NotLicensedForAdvancedCfg);




            PcConfigurationParamsRow configParamsRow = (from row in configurationRuntime.PcConfigurationParams
                                                        where !String.IsNullOrEmpty(row.RowMod)
                                                        select row).FirstOrDefault();

            if (configParamsRow == null)
                throw new BLException(Strings.ConfigurationParamsRequired);


            var PcStatus = this.FindFirstPcStatusEWCResult(Session.CompanyID, configParamsRow.ConfigID);
            if (PcStatus == null)
            {
                throw new BLException(Strings.PcStatusNotFound(configParamsRow.ConfigID));
            }


            if (!PcStatus.ConfigType.KeyEquals("EWC"))
            {
                throw new InvalidConfigurationTypeForEpicorWebConfigurator(Strings.ConfigurationTypeMustBeEWC, "PcStatus");
            }
            if (configParamsRow.TestID.Equals(Guid.Empty))
            {

                if (!PcStatus.Approved)
                {
                    throw new BLException(Strings.PcStatusNotApproved(configParamsRow.ConfigID));
                }

                if (PcStatus.EWCClientSyncRequired && !PcStatus.ExtConfig)
                {
                    throw new EpicorWebRequiresUpdatesForConfigurator(GlobalStrings.EWCRequiresUpdating(PcStatus.ConfigID), Erp.Tables.PcStatus.GetTableName());
                }
            }



            Erp.Tablesets.PcValueTableset pcValueTableSet = new PcValueTableset();
            if (configurationSequenceTableSet == null || configurationSequenceTableSet.PcStruct.Count <= 0)
            {
                ConfigurationSummaryTableset summaryTS = null;
                configurationSequenceTableSet = ValidateAndBuildConfigurationData(configurationRuntime, ref summaryTS, false);
            }
            pcValueTableSet = StartPcValueConfiguration(ref configurationRuntime, configurationSequenceTableSet);



            return pcValueTableSet;
        }


        /// <summary>
        /// Method to save a configuration session to be used by Epicor Web
        /// </summary>
        /// <param name="configurationRuntimeDS">Configuration Runtime dataset that was populated by the start of the configuration session.
        /// Contains the ConfigurationParams table that informs the server engines which configurator is being processed/saved</param>
        /// <param name="configurationSequenceDS">Configuration Sequence tableset containts the pcStruct rows that identify the top level and all
        /// sub configurators</param>
        /// <param name="pcValueDS">Transport Table containing the results of the Question and Answer table.  This information is
        /// used to construct the PcValueSet data</param>
        /// <param name="testResultsDS">dataset returned for Test Rules</param>
        public void SavePCTransportConfiguration(ConfigurationSequenceTableset configurationSequenceDS, ref ConfigurationRuntimeTableset configurationRuntimeDS, PcValueTableset pcValueDS, ref PcTestResultsTableset testResultsDS)
        {
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedConfigurator))
                throw new BLException(GlobalStrings.NotLicensedForAdvancedCfg);
            SavePcValueConfiguration(configurationSequenceDS, ref configurationRuntimeDS, pcValueDS, out bool testPassed, out string failText, ref testResultsDS);
        }

        /// <summary>
        /// Remove the temporary file storeS entry for Test Inputs
        /// </summary>
        /// <param name="testInputsFileName">file to delete</param>
        public void DeleteTestInputsFileStoreEntry(string testInputsFileName)
        {
            deleteFileStoreEntry(testInputsFileName);
        }

        private void deleteFileStoreEntry(string testInputsFileName)
        {
            if (String.IsNullOrEmpty(testInputsFileName)) return;
            using (TransactionScope txScope = new TransactionScope())
            {
                Ice.Tables.FileStore FileStore = this.FindFirstFileStoreByFileNameAndUpdateLock(Session.TenantID, Session.CompanyID, testInputsFileName);
                if (FileStore != null)
                {
                    Db.FileStore.Delete(FileStore);
                    Db.Validate();
                }
                txScope.Complete();
            }
        }


        /// <summary>
        /// This method retrieves the token and for enterprise configurators when in the sales company verifies the configurator has been deployed to EWC in that company and if not
        /// executes the deploy logic so the end user is able to configure.
        /// </summary>
        /// <param name="configID">Configuration ID</param>
        /// <param name="accessToken">returned token for EWC</param>
        /// <param name="expiresIn">Expiration time</param>
        public void PrepareEWCRequirements(string configID, out string accessToken, out int expiresIn)
        {
            accessToken = string.Empty;
            expiresIn = 0;
            if (!Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedConfigurator))
                throw new BLException(GlobalStrings.NotLicensedForAdvancedCfg);

            if (this.ExistsEnterprisePcStatus(Session.CompanyID, configID))
            {
                string ewcFileName = Session.CompanyID + "_" + configID + ".zip";
                if (Session.IsMultiTenant)
                {
                    ewcFileName = Session.TenantID + "_" + ewcFileName;
                }
                var fileStoreColResult = SelectFileStoreQuery(Session.TenantID, Session.CompanyID, ewcFileName);
                if (fileStoreColResult == null)
                {
                    using (SynchronizationUtilities synchronizationUtilities = new SynchronizationUtilities(Db))
                    {
                        synchronizationUtilities.ExecuteDeployLogic(configID, out ewcFileName, false);
                    }
                }
            }
            RequestAccessToken(ref accessToken, ref expiresIn);
        }

        private void RequestAccessToken(ref string accessToken, ref int expiresIn)
        {
            using (var TokenSvc = ServiceRenderer.GetService<Ice.Contracts.TokenServiceSvcContract>(Db))
            {
                TokenServiceTableset token = null;
                token = TokenSvc.GetAccessToken(Guid.Empty, "secret", "default");
                if (token != null && token.TokenService.Count > 0)
                {
                    accessToken = token.TokenService[0].AccessToken;
                    expiresIn = token.TokenService[0].ExpiresIn;
                }
            }
        }
        #endregion EWC logic


        #region ECC MetaUI Configurator

        /// <summary>
        /// /// ERPS-179747
        /// EccConfigData: Configurator parameters to launch MetaUI Configuration from ECC.
        /// </summary>
        public class EccConfigData
        {
            /// <summary>
            /// //Received from ecc
            /// ConfigName: Configurator name. This parameter includes Company, version and configurator ID
            /// Ex EPIC06_VER1.CFG-1
            /// </summary>
            public string ConfigName { get; set; }

            /// <summary>
            /// //Received from ecc
            /// RelatedToTable: Table that storages related row. As it comes from ECC, value should be PcECCOrderDtl.
            /// </summary>
            public string RelatedToTable { get; set; }

            /// <summary>
            /// //Received from ecc
            /// RelatedToRowID: Related Row unique ID.
            /// </summary>
            public string RelatedToRowID { get; set; }

            /// <summary>
            /// //Received from ecc
            /// PartNum: Part Number which will be configured.
            /// </summary>
            public string PartNum { get; set; }

            /// <summary>
            /// //Received from ecc
            /// PartRev: Specifies the revision reference for a particular part.
            /// </summary>
            public string PartRev { get; set; }

            /// <summary>
            /// //Received from ecc
            /// CSS: Specifies Custom CSS.
            /// </summary>
            /// 
            public string CSS { get; set; }

            /// <summary>
            /// //Received from ecc
            /// Lang: Specifies mapped language.
            /// </summary>
            public string LanguageID { get; set; }

            /// <summary>
            /// //Received from ecc
            /// ReturnURL: After configure,this url adds product to cart.
            /// </summary>
            public string ReturnURL { get; set; }

            /// <summary>
            /// //Received from ecc
            /// ECCUser: Kinetic ecrypted ERP user.
            /// </summary>
            public string ECCUser { get; set; }

            /// <summary>
            /// //Received from ecc
            /// ECCPwd: Kinetic encrypted ERP Password.
            /// </summary>
            public string ECCPwd { get; set; }

            /// <summary>
            /// //Received from ecc
            /// ECCCompanyID: Default Company set in ECC settings.
            /// </summary>
            public string ECCCompanyID { get; set; }

            /// <summary>
            /// //Received from ecc
            /// AppServerURL: Application server and name defined in URL from server.
            /// </summary>
            public string AppServerURL { get; set; }

            /// <summary>
            /// TenantId: Tenant name or domain.
            /// </summary>
            public string TenantId { get; set; }

            /// <summary>
            /// Token: Token Id value to initialize session.
            /// </summary>
            public string Token { get; set; }

            /// <summary>
            /// LanguageCultureName: Specifies mapped Language Culture-Name.
            /// </summary>
            public string LanguageCultureName { get; set; }

            /// <summary>
            /// FormatCultureName: Specifies mapped Format Culture Name.
            /// </summary>
            public string FormatCultureName { get; set; }

            /// <summary>
            /// IsTrackerMode: Defines if configurator is launched in tracker mode.
            /// </summary>
            public string IsTrackerMode { get; set; }

            /// <summary>
            /// Received from ECC
            /// CSSStyles: URL that retrieves CSS defined in ECC configurator settings.           
            /// </summary>
            public string CSSStyles { get; set; }

            /// <summary>
            /// ConfigURL: URL to launch MetaUI configurator component to show Product Configuration.
            /// </summary>
            public string ConfigURL { get; set; }

        }

        private static readonly ServiceProvider iServiceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
        private static readonly IHttpClientFactory httpClientFactory = iServiceProvider.GetService<IHttpClientFactory>();
        EccConfigData configuratorSetup = new EccConfigData();
        string configuratorAppURL = string.Empty;

        /// <summary>
        /// Get ECC Part configurator initial data and process to return configuration setup to display Kinetic Configurator
        /// </summary>
        /// <param name="eccConfigSetup">eccConfigSetup: json object returned from ECC</param>
        /// <returns>configuratorContext string</returns>
        public string GetECCConfigurator(object eccConfigSetup)
        {
            var eccCfgContext = JsonConvert.SerializeObject(eccConfigSetup);

            //Convert JSON object to eccConfigData class
            configuratorSetup = JsonConvert.DeserializeObject<EccConfigData>(eccCfgContext);

            ProcessEccContextAndBuildSetup();

            //Get and process parameters that needs httpRequest
            LoadURLParameters().Wait();

            string configuratorInitialContext = JsonConvert.SerializeObject(configuratorSetup).Replace("\n", "").Replace("\r", "");

            return configuratorInitialContext;
        }


        /// <summary>
        /// Get css from returned ECC URL
        /// </summary>
        /// <param name="httpClient">httpclient</param>
        /// <param name="url">url</param>
        /// <returns>css string</returns>
        private async Task<string> GetCSS(HttpClient httpClient, string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await httpClient.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                    return await response.Content.ReadAsStringAsync();
                else
                    return string.Empty;
            }
            catch
            {
                throw new BLException(Strings.ECCCSSURLResolverError);
            }
        }

        /// <summary>
        /// GetConfiguratorPage
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="httpClient">url</param>
        /// <returns>css string</returns>
        private async Task<bool> IsValidConfiguratorURL(HttpClient httpClient, string url)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        /// <summary>
        /// ProcessEccContextAndBuildSetup
        /// </summary>
        private void ProcessEccContextAndBuildSetup()
        {
            try
            {
                //AppServer URL retrieved from ECC configurator settings
                string appServerURL = configuratorSetup.AppServerURL;

                string channelId = Guid.NewGuid().ToString();
                string queryString = "?channelid=" + channelId + "&ecc=true";
                string configId = GetConfiguratorID(configuratorSetup.ConfigName);
                string eccCompanyID = configuratorSetup.ECCCompanyID;
                string appURL = string.Empty;
                string viewURL = string.Empty;


                configuratorSetup.FormatCultureName = CurrentCultureName();
                configuratorSetup.IsTrackerMode = "false";
                configuratorSetup.LanguageID = CurrentLanguageID();
                configuratorSetup.LanguageCultureName = CurrentLanguageID();
                configuratorSetup.TenantId = GetTenantID(eccCompanyID);
                configuratorSetup.Token = GetToken();



                appURL = appServerURL + "/apps/erp/home/#/";
                viewURL = "view/" + GenerateRandomNumber() + "/Erp.UICfg." + configId + "_" + eccCompanyID + queryString;

                //MetaUI configurator component URL 
                configuratorSetup.ConfigURL = appURL + "configurator";
                //MetaUI product configurator URL
                configuratorAppURL = appURL + viewURL;
            }
            catch
            {
                throw new BLException(Strings.ECCBuildParamsError);
            }
        }

        private async System.Threading.Tasks.Task LoadURLParameters()
        {
            string cssUrl = configuratorSetup.CSS;
            string cfgUrl = configuratorAppURL;
            bool isValid = false;

            //Check if Configurator exists and url can be reached
            if (!string.IsNullOrEmpty(cfgUrl))
            {
                using (var httpClient = httpClientFactory.CreateClient())
                {
                    isValid = (await IsValidConfiguratorURL(httpClient, cfgUrl));

                    if (!isValid)
                    {
                        throw new BLException(Strings.ECCCfgURLResolverError);
                    }

                }
            }

            //Get Css customized style from ecc url returned and asign it to CSSStyles property
            if (!string.IsNullOrEmpty(cssUrl))
            {
                using (var httpClient = httpClientFactory.CreateClient())
                {
                    configuratorSetup.CSSStyles = (await GetCSS(httpClient, cssUrl)).Replace("\n", "").Replace("\r", "");
                }
            }

        }

        private string GetToken()
        {
            try
            {
                return WebTokenIssuer.GetToken(out _);
            }
            catch
            {
                throw new BLException(Strings.ECCCfgTokenError);
            }
        }

        /// <summary>
        /// getConfiguratorID: get Configurator ID removing companyid and version.
        /// </summary>
        /// <param name="eccConfigName">eccConfigSetup: json object returned from ECC</param>
        /// <returns>configurationID string</returns>
        private string GetConfiguratorID(string eccConfigName)
        {
            string configurationID = string.Empty;

            configurationID = eccConfigName.Substring(eccConfigName.IndexOf('.') + 1);

            return configurationID;
        }

        /// <summary>
        /// currentCultureName: get the Culture Name based on current session.
        /// </summary>
        /// <returns>cultureName string</returns>
        private string CurrentCultureName()
        {
            string cultureName = string.Empty;
            if (Session != null)
            {
                cultureName = Session.FormatCultureName;
            }
            return cultureName;
        }

        /// <summary>
        /// currentLanguageID: get the Language ID based on current session.
        /// </summary>
        /// <returns>languageID string</returns>
        private string CurrentLanguageID()
        {
            string languageID = string.Empty;
            if (Session != null)
            {
                languageID = Session.LanguageID;
            }
            return languageID;
        }

        /// <summary>
        /// GenerateRandomNumber: generates a random 6 number to simulate menu ID to disaplay configurator.
        /// </summary>
        /// <returns>randomNumber int</returns>
        private int GenerateRandomNumber()
        {
            int minValue = 10000;
            int maxValue = 99999;
            Random rndm = new Random();
            int randomNumber = rndm.Next(minValue, maxValue);
            return randomNumber;
        }

        #endregion

    }
}
