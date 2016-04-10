using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class FeatureReverseHost : WebReverseHost
    {
        public Feature Feature { get; set; }

        public bool IsSiteLevel { get; set; }
        public bool IsWebLevel { get; set; }

        public bool IsActivated { get; set; }

        public List<FeatureDefinition> AllKnownFeatures { get; set; }

        public Guid FeatureId { get; set; }
    }
}
