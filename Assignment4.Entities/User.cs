using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment4.Entities
{
    public class User
    {

        public int id { get; set; }

        [StringLength(105)]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
