using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Autofac;
using XamlingCore.Portable.Data.Glue;

namespace Arlo.SDK.Entities.System
{
    public static class SessionExtensions
    {
        public static bool IsABitLater(this ArloSession session)
        {
            if (!session.DateTime_Start.HasValue)
            {
                return false;
            }

            var sessionService = ContainerHost.Container.Resolve<ISessionService>();

            var timeAway = session.DateTime_Start.Value.Subtract(sessionService.GetCurrentTime());

            return timeAway.TotalMinutes > 75;
        }
    }
}
