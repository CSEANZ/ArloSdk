using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using XamlingCore.Portable.Contract.Config;

namespace Arlo.SDK.Services.System
{
    public class SystemService : ISystemService
    {
        private readonly IArloConfigService _config;

        public SystemService(IArloConfigService config)
        {
            _config = config;
        }
        public string GetRealUrl(string url)
        {
            var uriBase = new Uri(_config.BaseApiUrl, UriKind.Absolute);
            var uriFinal = new Uri(uriBase, new Uri(url, UriKind.Relative));

            return uriFinal.ToString();
        }
    }
}
