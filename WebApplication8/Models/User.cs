using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6)]
        public string Pwd { get; set; } = string.Empty;

        public string Salt { get; set; } = string.Empty;
    }
}
