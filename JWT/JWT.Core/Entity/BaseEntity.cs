using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Core.Entity
{
    public class BaseEntity 
    {
        public BaseEntity()
        {
            CreatedAt = UpdatedAt = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public bool IsActive { get; set; }


        public DateTime CreatedAt { get; set; }

        public int? CreatedById { get; set; }


        public DateTime UpdatedAt { get; set; }

        public int? UpdatedById { get; set; }


        public DateTime? DeletedAt { get; set; }

        public int? DeletedById { get; set; }

        public bool IsDeleted { get; set; }
    }
}
