using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Epicor.Data;
using Erp.Tables;
namespace Erp.Internal.PE
{
    public partial class BVRule
    {
        #region BVRule Queries

       
        class BVRuleInfo 
        {
            public int BVRuleUID { get; set; }
            public int VRuleUID { get; set; }
            public string Action { get; set; }
            public bool IsDefault { get; set; }
        }
        static Func<ErpContext, string, int, int, string, int, IEnumerable<BVRuleInfo>> selectBVRuleQuery;
        private IEnumerable<BVRuleInfo> SelectBVRuleInfo(string company, int actTypeUID, int actRevisionUID, string bookID, int ruleUID)
        {
            if (selectBVRuleQuery == null)
            {
                Expression<Func<ErpContext, string, int, int, string, int, IEnumerable<BVRuleInfo>>> expression =
                  (ctx, company_ex, actTypeUID_ex, actRevisionUID_ex, bookID_ex, ruleUID_ex) =>
                    (from row in ctx.BVRule.With(LockHint.UpdLock)
                     where row.Company == company_ex &&
                     row.ACTTypeUID == actTypeUID_ex &&
                     row.ACTRevisionUID == actRevisionUID_ex &&
                     row.BookID == bookID_ex &&
                     row.RuleUID == ruleUID_ex
                     select new BVRuleInfo 
                     {
                         BVRuleUID = row.BVRuleUID,
                         VRuleUID = row.VRuleUID,
                         IsDefault = row.IsDefault,
                         Action = row.Action
                     });
                selectBVRuleQuery = DBExpressionCompiler.Compile(expression);
            }

            return selectBVRuleQuery(this.Db, company, actTypeUID, actRevisionUID, bookID, ruleUID);
        }
        #endregion BVRule Queries
        //HOLDLOCK
    }
}
