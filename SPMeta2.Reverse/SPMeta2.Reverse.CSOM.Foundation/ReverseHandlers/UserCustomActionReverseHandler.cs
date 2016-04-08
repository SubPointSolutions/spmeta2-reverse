using SPMeta2.Definitions;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Models;
using SPMeta2.Reverse.CSOM.Foundation.ReverseHosts;
using SPMeta2.Reverse.ReverseHosts;
using SPMeta2.Reverse.Services;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.ModelHosts;

namespace SPMeta2.Reverse.CSOM.Foundation.ReverseHandlers
{
    public class UserCustomActionReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(UserCustomActionDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(SiteDefinition),
                    typeof(WebDefinition)
                };
            }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<UserCustomActionReverseHost>();

            var siteHost = parentHost as SiteReverseHost;
            var webHost = parentHost as WebReverseHost;

            var site = siteHost.HostSite;
            var web = siteHost.HostWeb;

            UserCustomActionCollection items = null;

            if (webHost != null)
            {
                web = webHost.HostWeb;
                items = web.UserCustomActions;
            }
            else if(siteHost != null)
            {
                items = site.UserCustomActions;
            }

            var context = siteHost.HostClientContext;

            context.Load(items);
            context.ExecuteQuery();

            result.AddRange(ApplyReverseFilters(items, options).ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<UserCustomActionReverseHost>(parentHost, h =>
                {
                    h.HostUserCustomAction = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as UserCustomActionReverseHost).HostUserCustomAction;

            var def = new UserCustomActionDefinition();

            def.Title = item.Title;
            def.Name = item.Name;
            def.Description = item.Description;

            def.Group = item.Group;

            def.ScriptSrc = item.ScriptSrc;
            def.ScriptBlock = item.ScriptBlock;

            def.Location = item.Location;
            def.Sequence = item.Sequence;

            def.Url = item.Url;

            def.RegistrationId = item.RegistrationId;
            def.RegistrationType = item.RegistrationType.ToString();

            return new UserCustomActionModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
