using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Reverse.CSOM.ReverseHosts;
using SPMeta2.Reverse.Services;

namespace SPMeta2.Reverse.CSOM.Services
{
    public static class CSOMReverseServiceExtensions
    {
        public static ReverseResult ReverseSiteModel(this CSOMReverseService service, ClientContext context, ReverseOptions options)
        {
            return service.Reverse(SiteReverseHost.FromClientContext(context), options);
        }

        public static ReverseResult ReverseWebModel(this CSOMReverseService service, ClientContext context, ReverseOptions options)
        {
            return service.Reverse(WebReverseHost.FromClientContext(context), options);
        }
    }
}
