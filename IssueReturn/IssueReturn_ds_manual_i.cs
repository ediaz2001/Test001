
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Epicor.Data;
using Erp;
using Erp.Tables;
using Ice;
using Epicor.Hosting;
using Erp.Tablesets;
#if USE_EF_CORE
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif
namespace Erp.Services.BO
{
    public partial class IssueReturnSvc
    {
        /// <summary>
        /// 
        /// </summary>
        private class IssueReturnWaveParent
        {
            private string _Company = string.Empty;
            public string Company
            {
                get
                {
                    return this._Company;
                }

                set
                {
                    this._Company = value;
                }
            }

            public DateTime? TranDate { get; set; }

            private string _PartNum = string.Empty;
            public string PartNum
            {
                get
                {
                    return this._PartNum;
                }

                set
                {
                    this._PartNum = value;
                }
            }

            public decimal TranQty { get; set; }
            private string _DimCode = string.Empty;
            public string DimCode
            {
                get
                {
                    return this._DimCode;
                }

                set
                {
                    this._DimCode = value;
                }
            }

            private string _LotNum = string.Empty;
            public string LotNum
            {
                get
                {
                    return this._LotNum;
                }

                set
                {
                    this._LotNum = value;
                }
            }

            private string _ReasonCode = string.Empty;
            public string ReasonCode
            {
                get
                {
                    return this._ReasonCode;
                }

                set
                {
                    this._ReasonCode = value;
                }
            }

            public int OrderNum { get; set; }
            public int OrderLine { get; set; }
            public int OrderRel { get; set; }
            private string _FromJobNum = string.Empty;
            public string FromJobNum
            {
                get
                {
                    return this._FromJobNum;
                }

                set
                {
                    this._FromJobNum = value;
                }
            }

            public int FromAssemblySeq { get; set; }
            public int FromJobSeq { get; set; }
            private string _FromWarehouseCode = string.Empty;
            public string FromWarehouseCode
            {
                get
                {
                    return this._FromWarehouseCode;
                }

                set
                {
                    this._FromWarehouseCode = value;
                }
            }

            private string _FromBinNum = string.Empty;
            public string FromBinNum
            {
                get
                {
                    return this._FromBinNum;
                }

                set
                {
                    this._FromBinNum = value;
                }
            }

            private string _FromPCID = string.Empty;
            public string FromPCID
            {
                get
                {
                    return this._FromPCID;
                }

                set
                {
                    this._FromPCID = value;
                }
            }

            private string _FromJobPartNum = string.Empty;
            public string FromJobPartNum
            {
                get
                {
                    return this._FromJobPartNum;
                }

                set
                {
                    this._FromJobPartNum = value;
                }
            }

            private string _FromAssemblyPartNum = string.Empty;
            public string FromAssemblyPartNum
            {
                get
                {
                    return this._FromAssemblyPartNum;
                }

                set
                {
                    this._FromAssemblyPartNum = value;
                }
            }

            private string _FromJobSeqPartNum = string.Empty;
            public string FromJobSeqPartNum
            {
                get
                {
                    return this._FromJobSeqPartNum;
                }

                set
                {
                    this._FromJobSeqPartNum = value;
                }
            }

            private string _FromJobPartDesc = string.Empty;
            public string FromJobPartDesc
            {
                get
                {
                    return this._FromJobPartDesc;
                }

                set
                {
                    this._FromJobPartDesc = value;
                }
            }

            private string _FromAssemblyPartDesc = string.Empty;
            public string FromAssemblyPartDesc
            {
                get
                {
                    return this._FromAssemblyPartDesc;
                }

                set
                {
                    this._FromAssemblyPartDesc = value;
                }
            }

            private string _FromJobSeqPartDesc = string.Empty;
            public string FromJobSeqPartDesc
            {
                get
                {
                    return this._FromJobSeqPartDesc;
                }

                set
                {
                    this._FromJobSeqPartDesc = value;
                }
            }

            public decimal OnHandQty { get; set; }
            public decimal QtyRequired { get; set; }
            public decimal QtyPreviouslyIssued { get; set; }
            public bool IssuedComplete { get; set; }
            private string _ToJobNum = string.Empty;
            public string ToJobNum
            {
                get
                {
                    return this._ToJobNum;
                }

                set
                {
                    this._ToJobNum = value;
                }
            }

            public int ToAssemblySeq { get; set; }
            public int ToJobSeq { get; set; }
            private string _ToWarehouseCode = string.Empty;
            public string ToWarehouseCode
            {
                get
                {
                    return this._ToWarehouseCode;
                }

                set
                {
                    this._ToWarehouseCode = value;
                }
            }

            private string _ToBinNum = string.Empty;
            public string ToBinNum
            {
                get
                {
                    return this._ToBinNum;
                }

                set
                {
                    this._ToBinNum = value;
                }
            }

            private string _ToPCID = string.Empty;
            public string ToPCID
            {
                get
                {
                    return this._ToPCID;
                }

                set
                {
                    this._ToPCID = value;
                }
            }

            private string _ToJobPartNum = string.Empty;
            public string ToJobPartNum
            {
                get
                {
                    return this._ToJobPartNum;
                }

                set
                {
                    this._ToJobPartNum = value;
                }
            }

            private string _ToAssemblyPartNum = string.Empty;
            public string ToAssemblyPartNum
            {
                get
                {
                    return this._ToAssemblyPartNum;
                }

                set
                {
                    this._ToAssemblyPartNum = value;
                }
            }

            private string _ToJobSeqPartNum = string.Empty;
            public string ToJobSeqPartNum
            {
                get
                {
                    return this._ToJobSeqPartNum;
                }

                set
                {
                    this._ToJobSeqPartNum = value;
                }
            }

            private string _ToJobPartDesc = string.Empty;
            public string ToJobPartDesc
            {
                get
                {
                    return this._ToJobPartDesc;
                }

                set
                {
                    this._ToJobPartDesc = value;
                }
            }

            private string _ToAssemblyPartDesc = string.Empty;
            public string ToAssemblyPartDesc
            {
                get
                {
                    return this._ToAssemblyPartDesc;
                }

                set
                {
                    this._ToAssemblyPartDesc = value;
                }
            }

            private string _ToJobSeqPartDesc = string.Empty;
            public string ToJobSeqPartDesc
            {
                get
                {
                    return this._ToJobSeqPartDesc;
                }

                set
                {
                    this._ToJobSeqPartDesc = value;
                }
            }

            private string _ReasonType = string.Empty;
            public string ReasonType
            {
                get
                {
                    return this._ReasonType;
                }

                set
                {
                    this._ReasonType = value;
                }
            }

            private string _FromJobPartDescription = string.Empty;
            public string FromJobPartDescription
            {
                get
                {
                    return this._FromJobPartDescription;
                }

                set
                {
                    this._FromJobPartDescription = value;
                }
            }

            private string _FromAssemblyPartDescription = string.Empty;
            public string FromAssemblyPartDescription
            {
                get
                {
                    return this._FromAssemblyPartDescription;
                }

                set
                {
                    this._FromAssemblyPartDescription = value;
                }
            }

            private string _FromJobSeqPartDescription = string.Empty;
            public string FromJobSeqPartDescription
            {
                get
                {
                    return this._FromJobSeqPartDescription;
                }

                set
                {
                    this._FromJobSeqPartDescription = value;
                }
            }

            private string _ToJobPartDescription = string.Empty;
            public string ToJobPartDescription
            {
                get
                {
                    return this._ToJobPartDescription;
                }

                set
                {
                    this._ToJobPartDescription = value;
                }
            }

            private string _ToAssemblyPartDescription = string.Empty;
            public string ToAssemblyPartDescription
            {
                get
                {
                    return this._ToAssemblyPartDescription;
                }

                set
                {
                    this._ToAssemblyPartDescription = value;
                }
            }

            private string _ToJobSeqPartDescription = string.Empty;
            public string ToJobSeqPartDescription
            {
                get
                {
                    return this._ToJobSeqPartDescription;
                }

                set
                {
                    this._ToJobSeqPartDescription = value;
                }
            }

            private string _TranReference = string.Empty;
            public string TranReference
            {
                get
                {
                    return this._TranReference;
                }

                set
                {
                    this._TranReference = value;
                }
            }

            public int SerialNoQty { get; set; }
            private string _MtlQueueRowId = string.Empty;
            public string MtlQueueRowId
            {
                get
                {
                    return this._MtlQueueRowId;
                }

                set
                {
                    this._MtlQueueRowId = value;
                }
            }

            private string _TranType = string.Empty;
            public string TranType
            {
                get
                {
                    return this._TranType;
                }

                set
                {
                    this._TranType = value;
                }
            }

            public decimal DimConvFactor { get; set; }
            private string _InvAdjReason = string.Empty;
            public string InvAdjReason
            {
                get
                {
                    return this._InvAdjReason;
                }

                set
                {
                    this._InvAdjReason = value;
                }
            }

            private string _DUM = string.Empty;
            public string DUM
            {
                get
                {
                    return this._DUM;
                }

                set
                {
                    this._DUM = value;
                }
            }

            private string _UM = string.Empty;
            public string UM
            {
                get
                {
                    return this._UM;
                }

                set
                {
                    this._UM = value;
                }
            }

            private string _FromJobPlant = string.Empty;
            public string FromJobPlant
            {
                get
                {
                    return this._FromJobPlant;
                }

                set
                {
                    this._FromJobPlant = value;
                }
            }

            private string _ToJobPlant = string.Empty;
            public string ToJobPlant
            {
                get
                {
                    return this._ToJobPlant;
                }

                set
                {
                    this._ToJobPlant = value;
                }
            }

            private string _DummyKeyField = string.Empty;
            public string DummyKeyField
            {
                get
                {
                    return this._DummyKeyField;
                }

                set
                {
                    this._DummyKeyField = value;
                }
            }

            private string _TreeDisplay = string.Empty;
            public string TreeDisplay
            {
                get
                {
                    return this._TreeDisplay;
                }

                set
                {
                    this._TreeDisplay = value;
                }
            }

            public bool EnableToFields { get; set; }
            public bool EnableFromFields { get; set; }
            private string _RequirementUOM = string.Empty;
            public string RequirementUOM
            {
                get
                {
                    return this._RequirementUOM;
                }

                set
                {
                    this._RequirementUOM = value;
                }
            }

            public decimal RequirementQty { get; set; }
            public bool EnableSN { get; set; }
            private string _SerialControlPlant = string.Empty;
            public string SerialControlPlant
            {
                get
                {
                    return this._SerialControlPlant;
                }

                set
                {
                    this._SerialControlPlant = value;
                }
            }

            public bool SerialControlPlantIsFromPlt { get; set; }
            private string _ProcessID = string.Empty;
            public string ProcessID
            {
                get
                {
                    return this._ProcessID;
                }

                set
                {
                    this._ProcessID = value;
                }
            }

            private string _SerialControlPlant2 = string.Empty;
            public string SerialControlPlant2
            {
                get
                {
                    return this._SerialControlPlant2;
                }

                set
                {
                    this._SerialControlPlant2 = value;
                }
            }

            public bool TrackDimension { get; set; }
            private string _OnHandUM = string.Empty;
            public string OnHandUM
            {
                get
                {
                    return this._OnHandUM;
                }

                set
                {
                    this._OnHandUM = value;
                }
            }

            private string _TranDocTypeID = string.Empty;
            public string TranDocTypeID
            {
                get
                {
                    return this._TranDocTypeID;
                }

                set
                {
                    this._TranDocTypeID = value;
                }
            }

            private string _TFOrdNum = string.Empty;
            public string TFOrdNum
            {
                get
                {
                    return this._TFOrdNum;
                }

                set
                {
                    this._TFOrdNum = value;
                }
            }

            public int TFOrdLine { get; set; }
            public bool ReassignSNAsm { get; set; }
            public bool ReplenishDecreased { get; set; }
            private string _DimCodeDimCodeDescription = string.Empty;
            public string DimCodeDimCodeDescription
            {
                get
                {
                    return this._DimCodeDimCodeDescription;
                }

                set
                {
                    this._DimCodeDimCodeDescription = value;
                }
            }

            private string _FromBinNumDescription = string.Empty;
            public string FromBinNumDescription
            {
                get
                {
                    return this._FromBinNumDescription;
                }

                set
                {
                    this._FromBinNumDescription = value;
                }
            }

            private string _FromWarehouseCodeDescription = string.Empty;
            public string FromWarehouseCodeDescription
            {
                get
                {
                    return this._FromWarehouseCodeDescription;
                }

                set
                {
                    this._FromWarehouseCodeDescription = value;
                }
            }

            private string _LotNumPartLotDescription = string.Empty;
            public string LotNumPartLotDescription
            {
                get
                {
                    return this._LotNumPartLotDescription;
                }

                set
                {
                    this._LotNumPartLotDescription = value;
                }
            }

            private string _PartSalesUM = string.Empty;
            public string PartSalesUM
            {
                get
                {
                    return this._PartSalesUM;
                }

                set
                {
                    this._PartSalesUM = value;
                }
            }

            public bool PartTrackLots { get; set; }
            public decimal PartSellingFactor { get; set; }
            private string _PartPartDescription = string.Empty;
            public string PartPartDescription
            {
                get
                {
                    return this._PartPartDescription;
                }

                set
                {
                    this._PartPartDescription = value;
                }
            }

            public bool PartTrackSerialNum { get; set; }
            private string _PartPricePerCode = string.Empty;
            public string PartPricePerCode
            {
                get
                {
                    return this._PartPricePerCode;
                }

                set
                {
                    this._PartPricePerCode = value;
                }
            }

            public bool PartTrackDimension { get; set; }
            private string _PartIUM = string.Empty;
            public string PartIUM
            {
                get
                {
                    return this._PartIUM;
                }

                set
                {
                    this._PartIUM = value;
                }
            }

            private string _ReasonCodeDescription = string.Empty;
            public string ReasonCodeDescription
            {
                get
                {
                    return this._ReasonCodeDescription;
                }

                set
                {
                    this._ReasonCodeDescription = value;
                }
            }

            private string _ToBinNumDescription = string.Empty;
            public string ToBinNumDescription
            {
                get
                {
                    return this._ToBinNumDescription;
                }

                set
                {
                    this._ToBinNumDescription = value;
                }
            }

            private string _ToWarehouseCodeDescription = string.Empty;
            public string ToWarehouseCodeDescription
            {
                get
                {
                    return this._ToWarehouseCodeDescription;
                }

                set
                {
                    this._ToWarehouseCodeDescription = value;
                }
            }

            private string _RowIdent = string.Empty;
            public string RowIdent
            {
                get
                {
                    return this._RowIdent;
                }

                set
                {
                    this._RowIdent = value;
                }
            }

            private string _RowMod = string.Empty;
            public string RowMod
            {
                get
                {
                    return this._RowMod;
                }

                set
                {
                    this._RowMod = value;
                }
            }

            public Guid DBRowIdent { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        private IssueReturnWaveParent ttIssueReturnWaveParent;

        /// <summary>
        ///
        /// </summary>
        private static List<IssueReturnWaveParent> ttIssueReturnWaveParentRows = new List<IssueReturnWaveParent>();

        private class IssueReturnWaveChild
        {
            private string _Company = string.Empty;
            public string Company
            {
                get
                {
                    return this._Company;
                }

                set
                {
                    this._Company = value;
                }
            }

            public DateTime? TranDate { get; set; }

            private string _PartNum = string.Empty;
            public string PartNum
            {
                get
                {
                    return this._PartNum;
                }

                set
                {
                    this._PartNum = value;
                }
            }

            public decimal TranQty { get; set; }
            private string _DimCode = string.Empty;
            public string DimCode
            {
                get
                {
                    return this._DimCode;
                }

                set
                {
                    this._DimCode = value;
                }
            }

            private string _LotNum = string.Empty;
            public string LotNum
            {
                get
                {
                    return this._LotNum;
                }

                set
                {
                    this._LotNum = value;
                }
            }

            private string _ReasonCode = string.Empty;
            public string ReasonCode
            {
                get
                {
                    return this._ReasonCode;
                }

                set
                {
                    this._ReasonCode = value;
                }
            }

            public int OrderNum { get; set; }
            public int OrderLine { get; set; }
            public int OrderRel { get; set; }
            private string _FromJobNum = string.Empty;
            public string FromJobNum
            {
                get
                {
                    return this._FromJobNum;
                }

                set
                {
                    this._FromJobNum = value;
                }
            }

            public int FromAssemblySeq { get; set; }
            public int FromJobSeq { get; set; }
            private string _FromWarehouseCode = string.Empty;
            public string FromWarehouseCode
            {
                get
                {
                    return this._FromWarehouseCode;
                }

                set
                {
                    this._FromWarehouseCode = value;
                }
            }

            private string _FromBinNum = string.Empty;
            public string FromBinNum
            {
                get
                {
                    return this._FromBinNum;
                }

                set
                {
                    this._FromBinNum = value;
                }
            }

            private string _FromPCID = string.Empty;
            public string FromPCID
            {
                get
                {
                    return this._FromPCID;
                }

                set
                {
                    this._FromPCID = value;
                }
            }

            private string _FromJobPartNum = string.Empty;
            public string FromJobPartNum
            {
                get
                {
                    return this._FromJobPartNum;
                }

                set
                {
                    this._FromJobPartNum = value;
                }
            }

            private string _FromAssemblyPartNum = string.Empty;
            public string FromAssemblyPartNum
            {
                get
                {
                    return this._FromAssemblyPartNum;
                }

                set
                {
                    this._FromAssemblyPartNum = value;
                }
            }

            private string _FromJobSeqPartNum = string.Empty;
            public string FromJobSeqPartNum
            {
                get
                {
                    return this._FromJobSeqPartNum;
                }

                set
                {
                    this._FromJobSeqPartNum = value;
                }
            }

            private string _FromJobPartDesc = string.Empty;
            public string FromJobPartDesc
            {
                get
                {
                    return this._FromJobPartDesc;
                }

                set
                {
                    this._FromJobPartDesc = value;
                }
            }

            private string _FromAssemblyPartDesc = string.Empty;
            public string FromAssemblyPartDesc
            {
                get
                {
                    return this._FromAssemblyPartDesc;
                }

                set
                {
                    this._FromAssemblyPartDesc = value;
                }
            }

            private string _FromJobSeqPartDesc = string.Empty;
            public string FromJobSeqPartDesc
            {
                get
                {
                    return this._FromJobSeqPartDesc;
                }

                set
                {
                    this._FromJobSeqPartDesc = value;
                }
            }

            public decimal OnHandQty { get; set; }
            public decimal QtyRequired { get; set; }
            public decimal QtyPreviouslyIssued { get; set; }
            public bool IssuedComplete { get; set; }
            private string _ToJobNum = string.Empty;
            public string ToJobNum
            {
                get
                {
                    return this._ToJobNum;
                }

                set
                {
                    this._ToJobNum = value;
                }
            }

            public int ToAssemblySeq { get; set; }
            public int ToJobSeq { get; set; }
            private string _ToWarehouseCode = string.Empty;
            public string ToWarehouseCode
            {
                get
                {
                    return this._ToWarehouseCode;
                }

                set
                {
                    this._ToWarehouseCode = value;
                }
            }

            private string _ToBinNum = string.Empty;
            public string ToBinNum
            {
                get
                {
                    return this._ToBinNum;
                }

                set
                {
                    this._ToBinNum = value;
                }
            }

            private string _ToPCID = string.Empty;
            public string ToPCID
            {
                get
                {
                    return this._ToPCID;
                }

                set
                {
                    this._ToPCID = value;
                }
            }

            private string _ToJobPartNum = string.Empty;
            public string ToJobPartNum
            {
                get
                {
                    return this._ToJobPartNum;
                }

                set
                {
                    this._ToJobPartNum = value;
                }
            }

            private string _ToAssemblyPartNum = string.Empty;
            public string ToAssemblyPartNum
            {
                get
                {
                    return this._ToAssemblyPartNum;
                }

                set
                {
                    this._ToAssemblyPartNum = value;
                }
            }

            private string _ToJobSeqPartNum = string.Empty;
            public string ToJobSeqPartNum
            {
                get
                {
                    return this._ToJobSeqPartNum;
                }

                set
                {
                    this._ToJobSeqPartNum = value;
                }
            }

            private string _ToJobPartDesc = string.Empty;
            public string ToJobPartDesc
            {
                get
                {
                    return this._ToJobPartDesc;
                }

                set
                {
                    this._ToJobPartDesc = value;
                }
            }

            private string _ToAssemblyPartDesc = string.Empty;
            public string ToAssemblyPartDesc
            {
                get
                {
                    return this._ToAssemblyPartDesc;
                }

                set
                {
                    this._ToAssemblyPartDesc = value;
                }
            }

            private string _ToJobSeqPartDesc = string.Empty;
            public string ToJobSeqPartDesc
            {
                get
                {
                    return this._ToJobSeqPartDesc;
                }

                set
                {
                    this._ToJobSeqPartDesc = value;
                }
            }

            private string _ReasonType = string.Empty;
            public string ReasonType
            {
                get
                {
                    return this._ReasonType;
                }

                set
                {
                    this._ReasonType = value;
                }
            }

            private string _FromJobPartDescription = string.Empty;
            public string FromJobPartDescription
            {
                get
                {
                    return this._FromJobPartDescription;
                }

                set
                {
                    this._FromJobPartDescription = value;
                }
            }

            private string _FromAssemblyPartDescription = string.Empty;
            public string FromAssemblyPartDescription
            {
                get
                {
                    return this._FromAssemblyPartDescription;
                }

                set
                {
                    this._FromAssemblyPartDescription = value;
                }
            }

            private string _FromJobSeqPartDescription = string.Empty;
            public string FromJobSeqPartDescription
            {
                get
                {
                    return this._FromJobSeqPartDescription;
                }

                set
                {
                    this._FromJobSeqPartDescription = value;
                }
            }

            private string _ToJobPartDescription = string.Empty;
            public string ToJobPartDescription
            {
                get
                {
                    return this._ToJobPartDescription;
                }

                set
                {
                    this._ToJobPartDescription = value;
                }
            }

            private string _ToAssemblyPartDescription = string.Empty;
            public string ToAssemblyPartDescription
            {
                get
                {
                    return this._ToAssemblyPartDescription;
                }

                set
                {
                    this._ToAssemblyPartDescription = value;
                }
            }

            private string _ToJobSeqPartDescription = string.Empty;
            public string ToJobSeqPartDescription
            {
                get
                {
                    return this._ToJobSeqPartDescription;
                }

                set
                {
                    this._ToJobSeqPartDescription = value;
                }
            }

            private string _TranReference = string.Empty;
            public string TranReference
            {
                get
                {
                    return this._TranReference;
                }

                set
                {
                    this._TranReference = value;
                }
            }

            public int SerialNoQty { get; set; }
            private string _MtlQueueRowId = string.Empty;
            public string MtlQueueRowId
            {
                get
                {
                    return this._MtlQueueRowId;
                }

                set
                {
                    this._MtlQueueRowId = value;
                }
            }

            private string _TranType = string.Empty;
            public string TranType
            {
                get
                {
                    return this._TranType;
                }

                set
                {
                    this._TranType = value;
                }
            }

            public decimal DimConvFactor { get; set; }
            private string _InvAdjReason = string.Empty;
            public string InvAdjReason
            {
                get
                {
                    return this._InvAdjReason;
                }

                set
                {
                    this._InvAdjReason = value;
                }
            }

            private string _DUM = string.Empty;
            public string DUM
            {
                get
                {
                    return this._DUM;
                }

                set
                {
                    this._DUM = value;
                }
            }

            private string _UM = string.Empty;
            public string UM
            {
                get
                {
                    return this._UM;
                }

                set
                {
                    this._UM = value;
                }
            }

            private string _FromJobPlant = string.Empty;
            public string FromJobPlant
            {
                get
                {
                    return this._FromJobPlant;
                }

                set
                {
                    this._FromJobPlant = value;
                }
            }

            private string _ToJobPlant = string.Empty;
            public string ToJobPlant
            {
                get
                {
                    return this._ToJobPlant;
                }

                set
                {
                    this._ToJobPlant = value;
                }
            }

            private string _DummyKeyField = string.Empty;
            public string DummyKeyField
            {
                get
                {
                    return this._DummyKeyField;
                }

                set
                {
                    this._DummyKeyField = value;
                }
            }

            private string _TreeDisplay = string.Empty;
            public string TreeDisplay
            {
                get
                {
                    return this._TreeDisplay;
                }

                set
                {
                    this._TreeDisplay = value;
                }
            }

            public bool EnableToFields { get; set; }
            public bool EnableFromFields { get; set; }
            private string _RequirementUOM = string.Empty;
            public string RequirementUOM
            {
                get
                {
                    return this._RequirementUOM;
                }

                set
                {
                    this._RequirementUOM = value;
                }
            }

            public decimal RequirementQty { get; set; }
            public bool EnableSN { get; set; }
            private string _SerialControlPlant = string.Empty;
            public string SerialControlPlant
            {
                get
                {
                    return this._SerialControlPlant;
                }

                set
                {
                    this._SerialControlPlant = value;
                }
            }

            public bool SerialControlPlantIsFromPlt { get; set; }
            private string _ProcessID = string.Empty;
            public string ProcessID
            {
                get
                {
                    return this._ProcessID;
                }

                set
                {
                    this._ProcessID = value;
                }
            }

            private string _SerialControlPlant2 = string.Empty;
            public string SerialControlPlant2
            {
                get
                {
                    return this._SerialControlPlant2;
                }

                set
                {
                    this._SerialControlPlant2 = value;
                }
            }

            public bool TrackDimension { get; set; }
            private string _OnHandUM = string.Empty;
            public string OnHandUM
            {
                get
                {
                    return this._OnHandUM;
                }

                set
                {
                    this._OnHandUM = value;
                }
            }

            private string _TranDocTypeID = string.Empty;
            public string TranDocTypeID
            {
                get
                {
                    return this._TranDocTypeID;
                }

                set
                {
                    this._TranDocTypeID = value;
                }
            }

            private string _TFOrdNum = string.Empty;
            public string TFOrdNum
            {
                get
                {
                    return this._TFOrdNum;
                }

                set
                {
                    this._TFOrdNum = value;
                }
            }

            public int TFOrdLine { get; set; }
            public bool ReassignSNAsm { get; set; }
            public bool ReplenishDecreased { get; set; }
            private string _DimCodeDimCodeDescription = string.Empty;
            public string DimCodeDimCodeDescription
            {
                get
                {
                    return this._DimCodeDimCodeDescription;
                }

                set
                {
                    this._DimCodeDimCodeDescription = value;
                }
            }

            private string _FromBinNumDescription = string.Empty;
            public string FromBinNumDescription
            {
                get
                {
                    return this._FromBinNumDescription;
                }

                set
                {
                    this._FromBinNumDescription = value;
                }
            }

            private string _FromWarehouseCodeDescription = string.Empty;
            public string FromWarehouseCodeDescription
            {
                get
                {
                    return this._FromWarehouseCodeDescription;
                }

                set
                {
                    this._FromWarehouseCodeDescription = value;
                }
            }

            private string _LotNumPartLotDescription = string.Empty;
            public string LotNumPartLotDescription
            {
                get
                {
                    return this._LotNumPartLotDescription;
                }

                set
                {
                    this._LotNumPartLotDescription = value;
                }
            }

            private string _PartSalesUM = string.Empty;
            public string PartSalesUM
            {
                get
                {
                    return this._PartSalesUM;
                }

                set
                {
                    this._PartSalesUM = value;
                }
            }

            public bool PartTrackLots { get; set; }
            public decimal PartSellingFactor { get; set; }
            private string _PartPartDescription = string.Empty;
            public string PartPartDescription
            {
                get
                {
                    return this._PartPartDescription;
                }

                set
                {
                    this._PartPartDescription = value;
                }
            }

            public bool PartTrackSerialNum { get; set; }
            private string _PartPricePerCode = string.Empty;
            public string PartPricePerCode
            {
                get
                {
                    return this._PartPricePerCode;
                }

                set
                {
                    this._PartPricePerCode = value;
                }
            }

            public bool PartTrackDimension { get; set; }
            private string _PartIUM = string.Empty;
            public string PartIUM
            {
                get
                {
                    return this._PartIUM;
                }

                set
                {
                    this._PartIUM = value;
                }
            }

            private string _ReasonCodeDescription = string.Empty;
            public string ReasonCodeDescription
            {
                get
                {
                    return this._ReasonCodeDescription;
                }

                set
                {
                    this._ReasonCodeDescription = value;
                }
            }

            private string _ToBinNumDescription = string.Empty;
            public string ToBinNumDescription
            {
                get
                {
                    return this._ToBinNumDescription;
                }

                set
                {
                    this._ToBinNumDescription = value;
                }
            }

            private string _ToWarehouseCodeDescription = string.Empty;
            public string ToWarehouseCodeDescription
            {
                get
                {
                    return this._ToWarehouseCodeDescription;
                }

                set
                {
                    this._ToWarehouseCodeDescription = value;
                }
            }

            private string _RowIdent = string.Empty;
            public string RowIdent
            {
                get
                {
                    return this._RowIdent;
                }

                set
                {
                    this._RowIdent = value;
                }
            }

            private string _RowMod = string.Empty;
            public string RowMod
            {
                get
                {
                    return this._RowMod;
                }

                set
                {
                    this._RowMod = value;
                }
            }

            public Guid DBRowIdent { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        private IssueReturnWaveChild ttIssueReturnWaveChild;

        /// <summary>
        /// 
        /// </summary>
        private static List<IssueReturnWaveChild> ttIssueReturnWaveChildRows = new List<IssueReturnWaveChild>();
    }
}