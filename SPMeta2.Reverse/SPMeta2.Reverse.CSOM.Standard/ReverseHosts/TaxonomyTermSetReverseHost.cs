using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Reverse.CSOM.ReverseHosts;

namespace SPMeta2.Reverse.CSOM.Standard.ReverseHosts
{
    public class TaxonomyTermSetReverseHost : TaxonomyTermGroupReverseHost
    {

        public TermSet HostTermSet { get; set; }
    }
}
