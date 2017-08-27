using System;

namespace Xero.Refactor.Services.Exceptions
{

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {

        }

    }
}
