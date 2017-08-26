using System;
using System.ComponentModel.DataAnnotations;

namespace Xero.AspNet.Core.Data
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
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// The timestamp is used for optimistic concurency
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}