using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xero.Refactor.Data
{
    /// <summary>
    /// An inplementation of the IDesignTimeDbContextFactory
    /// is needed to run Add-Migration
    /// </summary>
    /// <remarks>https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext</remarks>
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<RefactorDb>
    {
        public RefactorDb CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RefactorDb>();
            return new RefactorDb();
        }
    }
}
