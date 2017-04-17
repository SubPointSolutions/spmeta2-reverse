using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Reverse.CSOM.ReverseHosts;

namespace SPMeta2.Reverse.CSOM.Standard.ReverseHosts
{
    public class TaxonomyTermGroupReverseHost : TaxonomyTermStoreReverseHost
    {

        public Microsoft.SharePoint.Client.Taxonomy.TermGroup HostTermGroup { get; set; }
    }
}
