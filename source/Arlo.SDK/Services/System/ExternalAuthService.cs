using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;

namespace Arlo.SDK.Services.System
{
    public class ExternalAuthService
    {
        private readonly IArloConfigService _config;

        public ExternalAuthService(IArloConfigService config)
        {
            _config = config;
        }

        //public string GetAuthCallbackUrl(string resumptionCookie)
        //{
            
        //}
    }
}
