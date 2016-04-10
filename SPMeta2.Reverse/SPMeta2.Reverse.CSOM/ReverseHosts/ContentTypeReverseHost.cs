using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class ContentTypeReverseHost : CSOMReverseHostBase
    {
        public ContentType HostContentType { get; set; }
    }
}
