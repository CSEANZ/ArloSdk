using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Arlo.SDK.Entities.Product;
using Xamling.Azure.Portable.Contract;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Contract.Serialise;
using XamlingCore.Portable.Data.Repos;
using XamlingCore.Portable.Data.Repos.Base;
using XamlingCore.Portable.Model.Response;

namespace Arlo.SDK.Repo.Base
{
    public class ArloRepoBase<T> : XWebRepo2<T>, IArloRepoBase<T>
        where T : class, new()
    {
        private readonly ILogService _logService;



        public ArloRepoBase(IHttpTransferrer downloader, IEntitySerialiser entitySerialiser, ILogService logService, string service) : base(downloader, entitySerialiser, service)
        {
            _logService = logService;
        }

        public override bool OnResultRetrieved(IHttpTransferResult result)
        {
            var resultDic = new Dictionary<string, string>
                {
                    {"Url", Service }
                };

            if (result == null)
            {

                _logService.TrackEvent("ArloResultNull", resultDic, null);
            }
            else
            {
                if (!result.IsSuccessCode)
                {
                    resultDic.Add("StatusCode", result.HttpStatusCode.ToString());
                    resultDic.Add("ResultText", result.Result);
                    _logService.TrackEvent("ArloResultFailed", resultDic, null);
                }
            }


            return base.OnResultRetrieved(result);
        }


        //public override bool OnEntityRetreived(XResult<T> entity)
        //{
        //    if (!entity)
        //    {
        //        _logService.TrackOperation(entity);
        //    }
        //    return base.OnEntityRetreived(entity);
        //}


        
    }
}
