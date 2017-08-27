using System;

namespace Xero.AspNet.Core.Data
{

    public abstract class DbServiceBase<DB> where DB : AuditableDbContext
    {
        protected DB DbContext { get; private set; }


        protected DbServiceBase(DB auditableDbContext)
        {
            DbContext = auditableDbContext ?? throw new ArgumentNullException("auditableDbContext");
        }


    }
}
