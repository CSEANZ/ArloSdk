namespace Arlo.SDK.Contract
{
    public interface IArloConfigService
    {
        string BaseApiUrl { get; }
        string UserName { get; }
        string Password { get; }
    }
}