using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Entities.System;
using XamlingCore.Portable.Contract.Config;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Net.DownloadConfig;
using XamlingCore.Portable.Net.Service;

#pragma warning disable 1998

namespace Arlo.SDK.Services.System
{
    public class TransferConfigService : HttpTransferConfigServiceBase
    {
        private readonly IConfig _config;

        public TransferConfigService(IConfig config)
        {
            _config = config;
        }

        public override async Task<IHttpTransferConfig> GetConfig(string url, string verb)
        {
            var baseUrl = new Uri(_config["ArloBaseUrl"]);

            var finalUrl = url.StartsWith("http:") ? new Uri(url) : new Uri(baseUrl, url);

            var config = new StandardHttpConfig
            {
                Accept = "application/xml",
                ContentEncoding = "application/xml",
                IsValid = true,
                Url = finalUrl.ToString(),
                BaseUrl = baseUrl.ToString(),
                Verb = verb,
                Headers = new Dictionary<string, string>(),
                Retries = 1,
                RetryOnNonSuccessCode = false,
                Gzip = true,
                Auth = _getPassword(), 
                AuthScheme  = "Basic"
            };

            Debug.WriteLine($"Transferring: {finalUrl.ToString()}");

            return config;
        }
        string _getPassword()
        {
            var user = _config[Constants.ArloUsername];
            var pass = _config[Constants.ArloPassword];

            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{pass}"));
        }
    }
}

