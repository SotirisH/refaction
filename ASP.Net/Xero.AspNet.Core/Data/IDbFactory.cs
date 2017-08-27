using System;
using System.Data.Entity;

namespace BH.MedicalRecords.Audit.Services.Infrastructure
{
    public interface IDbFactory<T> : IDisposable where T : DbContext
    {
        /// <summary>
        /// Returns a DbContext Object. The object lifetime is per request
        /// </summary>
        /// <returns></returns>
        T Init();
    }
}