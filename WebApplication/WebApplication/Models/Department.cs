using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Departname { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Custom> Customs { get; set; }
    }
}
