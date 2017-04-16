using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class ListViewReverseHost : ListReverseHost
    {
        #region constructors

        public ListViewReverseHost()
        {

        }

        public ListViewReverseHost(ClientContext clientContext, List list, View listView)
            : base(clientContext, list)
        {
            HostListView = listView;
        }

        #endregion

        #region properties

        public View HostListView { get; set; }

        #endregion
    }
}
