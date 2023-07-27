using System;
using System.Linq;
using System.Linq.Expressions;
using Epicor.Data;

namespace Erp.Services.BO
{
    public partial class InventoryTracking
    {
        #region Part
        class PartPartial
        {
            public string AttrClassID { get; set; }
            public bool TrackInventoryAttributes { get; set; }
            public bool TrackInventoryByRevision { get; set; }
        }

        static Func<ErpContext, string, string, PartPartial> findFirstPartQuery;
        private PartPartial FindFirstPart(string company, string partNum)
        {
            if (findFirstPartQuery == null)
            {
                Expression<Func<ErpContext, string, string, PartPartial>> expression =
                    (ctx, company_ex, partNum_ex) =>
                    (from row in ctx.Part
                     where row.Company == company_ex &&
                     row.PartNum == partNum_ex
                     select new PartPartial()
                     {
                         AttrClassID = row.AttrClassID,
                         TrackInventoryAttributes = row.TrackInventoryAttributes,
                         TrackInventoryByRevision = row.TrackInventoryByRevision
                     }).FirstOrDefault();
                findFirstPartQuery = DBExpressionCompiler.Compile(expression);
            }

            return findFirstPartQuery(this.Db, company, partNum);
        }
        #endregion
    }
}
