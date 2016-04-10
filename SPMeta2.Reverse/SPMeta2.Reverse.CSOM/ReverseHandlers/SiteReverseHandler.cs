using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.CSOM.ReverseHandlers
{
    public class SiteReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(SiteDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get { return Enumerable.Empty<Type>(); }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            return new[] { parentHost };
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var modelNode = new SiteModelNode
            {
                Options = { RequireSelfProcessing = false },
                Value = new SiteDefinition()
            };

            return modelNode;
        }

        #endregion
    }
}
