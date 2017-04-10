using System;
using Arlo.SDK.Contract;
using XamlingCore.Portable.Contract.Config;

namespace Arlo.SDK.Services.System
{
    public class ArloConfigService : IArloConfigService, IConfig
    {
        private readonly IConfig _config;

        public ArloConfigService(IConfig config)
        {
            _config = config;
        }

        public string BaseApiUrl => _config["ArloBaseUrl"];
        
        public string UserName => "botapi";

        public string Password => "Qasy67!as";

        public string this[string index] => _config[index];
    }
}
