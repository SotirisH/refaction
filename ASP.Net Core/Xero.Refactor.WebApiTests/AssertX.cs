using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Xero.Refactor.WebApiTests
{
    /// <summary>
    /// Extension over Assert
    /// </summary>
    public class AssertX : Assert
    {
        /// <summary>
        /// Checkes if an object IsInstanceOfType X and returns a meaningfull message
        /// </summary>
        /// <param name="value"></param>
        /// <param name="expectedType"></param>
        public new static void IsInstanceOfType(object value, Type expectedType)
        {
            var newMessage = $"A type of {expectedType.ToString()} is expected but a type of {  (value ==null?"Null":value.GetType().FullName) } was returned!";
            IsInstanceOfType(value, expectedType, newMessage);
        }
    }
}
