using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Reverse.CSOM.ReverseHosts
{
    public class FieldReverseHost : CSOMReverseHostBase
    {
        public Field Field { get; set; }
    }
}
