using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class ContentTypeLinkReverseHost : ListReverseHost
    {
        public ContentType HostContentType { get; set; }
    }
}
