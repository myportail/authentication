using System.ComponentModel.DataAnnotations;

namespace Authlib.Models.Db
{
    public class User
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
