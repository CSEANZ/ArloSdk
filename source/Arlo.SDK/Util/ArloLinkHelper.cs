using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Entities.Base;

namespace Arlo.SDK.Util
{
    public static class ArloLinkHelper
    {
        public static string FindLink(this ListOfLinks links, string linkRel)
        {
            return links.Link.FirstOrDefault(_ => _.Rel == linkRel)?.Href;
        }
    }
}
