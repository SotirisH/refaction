namespace Xero.AspNet.Core.Data
{
    /// <summary>
    /// All DBContext instances should implement this interface in order
    /// the Entity object to be auditable
    /// </summary>
    public interface IAuditableDBContext
    {
        int SaveChanges(string userName);
    }
}