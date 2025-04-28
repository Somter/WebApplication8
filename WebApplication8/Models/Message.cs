using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public int Id_User { get; set; }

        [Required, StringLength(500)]
        public string MessageText { get; set; } = string.Empty;

        public DateTime MessageDate { get; set; } = DateTime.Now;
    }
}
