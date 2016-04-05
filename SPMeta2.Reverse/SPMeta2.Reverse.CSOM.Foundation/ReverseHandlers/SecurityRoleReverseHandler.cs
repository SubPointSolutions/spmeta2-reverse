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
    public class SecurityRoleReverseHandler : CSOMReverseHandlerBase
    {
        #region properties
        public override Type ReverseType
        {
            get { return typeof(SecurityRoleDefinition); }
        }

        public override IEnumerable<Type> ReverseParentTypes
        {
            get
            {
                return new[]
                {
                    typeof(SiteDefinition)
                };
            }
        }
        #endregion

        #region methods

        public override IEnumerable<ReverseHostBase> ReverseHosts(ReverseHostBase parentHost, ReverseOptions options)
        {
            var result = new List<SecurityRoleReverseHost>();

            var typedHost = parentHost.WithAssertAndCast<SiteReverseHost>("reverseHost", value => value.RequireNotNull());

            var site = typedHost.HostSite;
            var context = typedHost.HostClientContext;

            var items = site.RootWeb.RoleDefinitions;

            context.Load(items);
            context.ExecuteQuery();

            result.AddRange(items.ToArray().Select(i =>
            {
                return ModelHostBase.Inherit<SecurityRoleReverseHost>(parentHost, h =>
                {
                    h.HostRole = i;
                });
            }));

            return result;
        }

        public override ModelNode ReverseSingleHost(object reverseHost, ReverseOptions options)
        {
            var item = (reverseHost as SecurityRoleReverseHost).HostRole;

            var def = new SecurityRoleDefinition();

            def.Name = item.Name;
            def.Description = item.Description;

            var allPermissionNames = Enum.GetNames(typeof(PermissionKind));

            foreach (var permissionName in allPermissionNames)
            {
                var permissionValue = (PermissionKind)Enum.Parse(typeof(PermissionKind), permissionName, true);

                if (item.BasePermissions.Has(permissionValue))
                {
                    if (permissionValue != PermissionKind.EmptyMask)
                        def.BasePermissions.Add(permissionName);
                }
            }

            return new SecurityRoleModelNode
            {
                Options = { RequireSelfProcessing = true },
                Value = def
            };
        }

        #endregion
    }
}
