using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class Custom
    {
        [Key]
        public int Id { get; set; }
        public string Cname { get; set; }
        public int DepartID { get; set; }
        public int Age { get; set; }
        public string Ename { get; set; }
        public string Password { get; set; }

        [ForeignKey("DepartID")]
        public virtual Department Department { get; set; }
    }
}
