using System;

namespace Arlo.SDK.Contract
{
    public interface ITimeService
    {
        DateTime GetQueenslandTime(DateTime input);
    }
}