using System;
using System.ComponentModel.DataAnnotations;

namespace Xero.AspNet.Core.Modelling
{
    /// <summary>
    /// Base class for all EF Class with audit tracking fields & Timestamp
    /// </summary>
    /// <remarks>
    /// </remarks>
    public abstract class EntityBase
    {
        /// <summary>
        /// User name either from the AD or the application
        /// </summary>
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
