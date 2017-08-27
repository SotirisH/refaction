using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xero.AspNet.Core
{
    /// <summary>
    /// Intrface for providing the name of the user who had perfomed the changes changes on the entity objects
    /// </summary>
    public interface ICurrentUserService
    {
        string GetCurrentUser();
    }
}
