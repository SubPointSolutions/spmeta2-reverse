using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Reverse.CSOM.ReverseHosts;

namespace SPMeta2.Reverse.CSOM.Standard.ReverseHosts
{
    public class TaxonomyTermReverseHost : TaxonomyTermSetReverseHost
    {

        public Term HostTerm { get; set; }
    }
}
