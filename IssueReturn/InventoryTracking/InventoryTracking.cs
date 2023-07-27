using System;
using System.Linq;
using Epicor.Hosting;
using Erp.Internal.Lib;
using Erp.Services.Lib.Resources;
using Erp.Tablesets;
using Ice;
using Ice.Common;

namespace Erp.Services.BO
{
    /// <summary>
    /// Services specific to Inventory Tracking
    /// </summary>
    public partial class InventoryTracking : ContextBoundBase<ErpContext>
    {
        #region Context
        /// <summary>
        /// Context
        /// </summary>
        /// <param name="ctx"></param>
        public InventoryTracking(ErpContext ctx)
            : base(ctx)
        {
            base.Initialize();
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }
        #endregion

        #region DoPartRev
        /// <summary>
        /// Sets the default Revision Number and then updates the Attribute Set.
        /// </summary>
        /// <param name="trackInventoryByRevision"></param>
        /// <param name="ttIssueReturn"></param>
        /// <exception cref="DataValidationException"></exception>
        public void DoPartRev(bool trackInventoryByRevision, ref IssueReturnRow ttIssueReturn)
        {
            if (ttIssueReturn == null)
            {
                throw new DataValidationException(GlobalStrings.ModifiedRowNotFound(nameof(IssueReturnRow)));
            }

            ttIssueReturn.RevisionNum = "";

            if (!trackInventoryByRevision)
            {
                return;
            }

            using (var libGetPartRev = new Internal.Lib.GetPartRev(Db))
            {
                ttIssueReturn.RevisionNum = libGetPartRev.GetLatestApprovedPartRevForInventoryByRevision(ttIssueReturn.PartNum, ttIssueReturn.Plant);
            }

            UpdateAttributeSetIDFromRevisionNum(ref ttIssueReturn);
        }
        #endregion

        #region GetDefaultAttributeSet
        /// <summary>
        /// Returns the default attribute set if defined for part
        /// </summary>
        /// <param name="partNum"></param>
        /// <returns></returns>
        public int GetDefaultAttributeSet(string partNum)
        {
            if (Session.ModuleLicensed(Erp.License.ErpLicensableModules.AdvancedUnitOfMeasure))
            {
                using (DynAttributes libDynAttributes = new Internal.Lib.DynAttributes(Db))
                {
                    return libDynAttributes.GetDefaultAttributeSetID(partNum);
                }
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region GetAttributeDescriptions
        /// <summary>
        /// Returns the Attribute Set's descriptions.
        /// </summary>
        /// <param name="ttIssueReturn"></param>
        public void GetAttributeDescriptions(ref IssueReturnRow ttIssueReturn)
        {

            string attributeSetDescription = string.Empty;
            string attributeSetShortDescription = string.Empty;

            if (ttIssueReturn.PartTrackInventoryAttributes)
            {
                using (Erp.Internal.Lib.AdvancedUOM LibAdvancedUOM = new Erp.Internal.Lib.AdvancedUOM(Db))
                {
                    LibAdvancedUOM.GetAttributeSetDescriptions(Session.CompanyID, ttIssueReturn.AttributeSetID, out attributeSetDescription, out attributeSetShortDescription);
                }
            }
            ttIssueReturn.AttributeSetDescription = attributeSetDescription;
            ttIssueReturn.AttributeSetShortDescription = attributeSetShortDescription;
        }
        #endregion

        #region RefreshAttributeDescriptions         
        /// <summary>
        /// Maintains the attribute descriptions based on if they are Attribute tracked
        /// </summary>
        /// <param name="ttIssueReturn"></param>
        public void RefreshAttributeDescriptions(ref IssueReturnRow ttIssueReturn)
        {
            if (ttIssueReturn != null && !ttIssueReturn.PartTrackInventoryAttributes)
            {
                ttIssueReturn.AttributeSetDescription = string.Empty;
                ttIssueReturn.AttributeSetShortDescription = string.Empty;
            }
        }
        #endregion

        #region OnChangingAttributeSet
        /// <summary>
        /// Call this method when the attribute set changes
        /// </summary>
        /// <param name="attributeSetID"></param>
        /// <param name="ds"></param>
        public void OnChangingAttributeSet(int attributeSetID, ref IssueReturnTableset ds)
        {
            var ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                                 where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                                 select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                ttIssueReturn.AttributeSetID = attributeSetID;

                if (ttIssueReturn.PartTrackInventoryAttributes)
                {
                    using (var libAdvancedUOM = new Internal.Lib.AdvancedUOM(Db))
                    {
                        libAdvancedUOM.GetAttributeSetDescriptions(ttIssueReturn.Company, ttIssueReturn.AttributeSetID, out string attributeSetDescription, out string attributeSetShortDescription);
                        ttIssueReturn.AttributeSetDescription = attributeSetDescription;
                        ttIssueReturn.AttributeSetShortDescription = attributeSetShortDescription;
                    }
                }
                else
                {
                    RefreshAttributeDescriptions(ref ttIssueReturn);
                }

                //Recalculate the number of pieces using the new attribute set.
                UpdateNumberOfPieces(ref ttIssueReturn);

                UpdateRevisionNumFromAttributeSetID(ref ttIssueReturn);
            }
        }

        /// <summary>
        /// Call this method when the attribute set changes for adjustment transactions (Issue Misc, Return Misc) to maintain inventory tracking
        /// </summary>
        /// <param name="attributeSetID"></param>
        /// <param name="ds"></param>
        public void OnChangingAttributeSetAdjustments(int attributeSetID, ref IssueReturnTableset ds)
        {
            var ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                                 where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                                 select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                ttIssueReturn.AttributeSetID = attributeSetID;

                if (ttIssueReturn.PartTrackInventoryAttributes)
                {
                    using (var libAdvancedUOM = new Internal.Lib.AdvancedUOM(Db))
                    {
                        libAdvancedUOM.GetAttributeSetDescriptions(ttIssueReturn.Company, ttIssueReturn.AttributeSetID, out string attributeSetDescription, out string attributeSetShortDescription);
                        ttIssueReturn.AttributeSetDescription = attributeSetDescription;
                        ttIssueReturn.AttributeSetShortDescription = attributeSetShortDescription;
                    }
                }
                else
                {
                    RefreshAttributeDescriptions(ref ttIssueReturn);
                }

                //Recalculate the number of pieces using the new attribute set.
                UpdateNumberOfPieces(ref ttIssueReturn);

                // Do not execute UpdateRevisionNumFromAttributeSetID - left in to comment
                // UpdateRevisionNumFromAttributeSetID(ref ttIssueReturn);
            }
        }
        #endregion

        #region OnChangingNumberOfPieces
        /// <summary>
        /// Call this method when the Nbr Of Pieces changes to calculate a new tran qty
        /// </summary>
        /// <param name="numberOfPieces"></param>
        /// <param name="ds"></param>
        public void OnChangingNumberOfPieces(decimal numberOfPieces, ref IssueReturnTableset ds)
        {
            var ttIssueReturn = (from ttIssueReturn_Row in ds.IssueReturn
                                 where !String.IsNullOrEmpty(ttIssueReturn_Row.RowMod)
                                 select ttIssueReturn_Row).FirstOrDefault();
            if (ttIssueReturn != null)
            {
                if (ttIssueReturn.AttributeSetID == 0)
                {
                    throw new DataValidationException(GlobalStrings.NumberOfPiecesRequiresAttributeSet);
                }

                using (var libDynAttributes = new Erp.Internal.Lib.DynAttributes(Db))
                using (Erp.Internal.Lib.ErpCallContext.SetDisposableKey("NumberOfPiecesChanged"))
                using (var issueReturnSvc = Ice.Assemblies.ServiceRenderer.GetService<Erp.Contracts.IssueReturnSvcContract>(Db))
                {
                    //Calculate the tranqty using the number of pieces that was specified
                    var proposedQty = libDynAttributes.GetTranQtyFromNumberOfPieces(ttIssueReturn.PartNum, numberOfPieces, ttIssueReturn.UM, ttIssueReturn.AttributeSetID);
                    issueReturnSvc.OnChangeTranQty(proposedQty, ref ds);
                }
            }
        }
        #endregion

        #region OnChangingRevisionNum
        /// <summary>
        /// Call this method when the Revision changes to maintain inventory tracking
        /// </summary>
        /// <param name="revisionNum"></param>
        /// <param name="ds"></param>
        public void OnChangingRevisionNum(string revisionNum, ref IssueReturnTableset ds)
        {
            var ttIssueReturn = ds.IssueReturn.FirstOrDefault(r => r.RowMod.Equals(IceRow.ROWSTATE_ADDED, StringComparison.OrdinalIgnoreCase) || r.RowMod.Equals(IceRow.ROWSTATE_UPDATED, StringComparison.OrdinalIgnoreCase));

            if (ttIssueReturn == null)
            {
                throw new DataValidationException(GlobalStrings.ModifiedRowNotFound(nameof(IssueReturnRow)));
            }

            var part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (part == null || !part.TrackInventoryByRevision)
            {
                return;
            }

            using (var libGetPartRev = new Internal.Lib.GetPartRev(Db))
            {
                libGetPartRev.IsValidPartRevForPart(ttIssueReturn.PartNum, revisionNum);
            }

            ttIssueReturn.RevisionNum = revisionNum;
            UpdateAttributeSetIDFromRevisionNum(ref ttIssueReturn);
        }

        /// <summary>
        /// Call this method when the Revision changes for adjustment transactions (Issue Misc, Return Misc) to maintain inventory tracking
        /// </summary>
        /// <param name="revisionNum"></param>
        /// <param name="ds"></param>
        public void OnChangingRevisionNumAdjustments(string revisionNum, ref IssueReturnTableset ds)
        {
            var ttIssueReturn = ds.IssueReturn.FirstOrDefault(r => r.RowMod.Equals(IceRow.ROWSTATE_ADDED, StringComparison.OrdinalIgnoreCase) || r.RowMod.Equals(IceRow.ROWSTATE_UPDATED, StringComparison.OrdinalIgnoreCase));

            if (ttIssueReturn == null)
            {
                throw new DataValidationException(GlobalStrings.ModifiedRowNotFound(nameof(IssueReturnRow)));
            }

            var part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (part == null || !part.TrackInventoryByRevision)
            {
                return;
            }

            using (var libGetPartRev = new Internal.Lib.GetPartRev(Db))
            {
                libGetPartRev.IsValidPartRevForPart(ttIssueReturn.PartNum, revisionNum);
            }

            ttIssueReturn.RevisionNum = revisionNum;

            // Only execute UpdateAttributeSetIDFromRevisionNum if not TrackInventoryAttributes
            if (!part.TrackInventoryAttributes)
            {
                UpdateAttributeSetIDFromRevisionNum(ref ttIssueReturn);
            }
        }
        #endregion

        #region UpdateAttributeSetIDFromRevisionNum
        /// <summary>
        /// Updates the Attribute Set from Revision Number, Revision Number is always the master.
        /// </summary>        
        /// <param name="ttIssueReturn"></param>
        /// <exception cref="DataValidationException"></exception>
        public void UpdateAttributeSetIDFromRevisionNum(ref IssueReturnRow ttIssueReturn)
        {
            if (ttIssueReturn == null)
            {
                throw new DataValidationException(GlobalStrings.ModifiedRowNotFound(nameof(IssueReturnRow)));
            }
            PartPartial part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (part == null || !part.TrackInventoryByRevision)
            {
                return;
            }

            using (var libInventoryByRevision = new Internal.Lib.InventoryByRevision(Db))
            {
                InventoryByRevision.InventoryByRevisonInfo inventoryByRevisonInfo = new()
                {
                    PartNum = ttIssueReturn.PartNum,
                    RevisionNum = ttIssueReturn.RevisionNum,
                    AttrClassID = part.AttrClassID,
                    TrackInventoryAttributes = part.TrackInventoryAttributes,
                    TrackInventoryByRevision = part.TrackInventoryByRevision,
                    CurrentAttributeSetID = ttIssueReturn.AttributeSetID,
                };

                ttIssueReturn.AttributeSetID = libInventoryByRevision.GetAttributeSetIDFromRevisionNum(inventoryByRevisonInfo);
            }

            if (!part.TrackInventoryAttributes)
            {
                RefreshAttributeDescriptions(ref ttIssueReturn);
            }
            else
            {
                using (var libDynAttributes = new Internal.Lib.DynAttributes(Db))
                {
                    libDynAttributes.GetDynAttrValueSetDescriptions(ttIssueReturn.AttributeSetID, out string shortDescription, out string description);
                    ttIssueReturn.AttributeSetDescription = description;
                    ttIssueReturn.AttributeSetShortDescription = shortDescription;
                }
            }
        }
        #endregion

        #region UpdateNumberOfPieces
        /// <summary>
        /// Updates the Number of Pieces on change of transaction quantity
        /// </summary>
        /// <param name="ttIssueReturn"></param>
        public void UpdateNumberOfPieces(ref IssueReturnRow ttIssueReturn)
        {
            if (!Internal.Lib.ErpCallContext.ContainsKey("NumberOfPiecesChanged")) // if this call originated with a change to number of pieces, don't update it again
            {
                using (var libDynAttributes = new Erp.Internal.Lib.DynAttributes(Db))
                {
                    ttIssueReturn.DispNumberOfPieces = libDynAttributes.GetNumberOfPiecesFromTranQty(ttIssueReturn.PartNum, ttIssueReturn.TranQty, ttIssueReturn.UM, ttIssueReturn.AttributeSetID);
                }
            }
        }
        #endregion

        #region UpdateRevisionNumFromAttributeSetID
        private void UpdateRevisionNumFromAttributeSetID(ref IssueReturnRow ttIssueReturn)
        {
            if (ttIssueReturn == null)
            {
                throw new DataValidationException(GlobalStrings.ModifiedRowNotFound(nameof(IssueReturnRow)));
            }

            PartPartial part = FindFirstPart(Session.CompanyID, ttIssueReturn.PartNum);
            if (part == null || !part.TrackInventoryByRevision)
            {
                return;
            }

            using (var libGetPartRev = new Internal.Lib.GetPartRev(Db))
            using (var libInventoryByRevision = new Internal.Lib.InventoryByRevision(Db))
            {
                string revisionNum = libInventoryByRevision.GetRevisionNumFromDynAttrValueSet(ttIssueReturn.AttributeSetID);
                libGetPartRev.IsValidPartRevForPart(ttIssueReturn.PartNum, revisionNum);
                ttIssueReturn.RevisionNum = revisionNum;
            }
        }
        #endregion

        #region ValidateInventoryByRevision
        /// <summary>
        /// Validates Revision Number
        /// </summary>
        /// <param name="trackInventoryByRevision"></param>
        /// <param name="ttIssueReturn"></param>
        public void ValidateInventoryByRevision(bool trackInventoryByRevision, IssueReturnRow ttIssueReturn)
        {
            if (!trackInventoryByRevision)
            {
                return;
            }

            using (var libInventoryByRevision = new Internal.Lib.InventoryByRevision(Db))
            {
                libInventoryByRevision.ValidateInventoryByRevision(ttIssueReturn.PartNum, ttIssueReturn.RevisionNum, trackInventoryByRevision, ttIssueReturn.AttributeSetID);
            }
        }
        #endregion

    }
}
