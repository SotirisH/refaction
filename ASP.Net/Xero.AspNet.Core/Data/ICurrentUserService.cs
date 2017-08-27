namespace Xero.AspNet.Core.Data
{
    /// <summary>
    /// Intrface for providing the name of the user who had perfomed the changes changes on the entity objects
    /// </summary>
    public interface ICurrentUserService
    {
        string GetCurrentUser();
    }
}