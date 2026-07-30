using System.ComponentModel.DataAnnotations;

namespace todo_api.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is needed")]
        [StringLength(100, ErrorMessage = "Title can not be more than 100 characters..")]
        public required string Title { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime? CompletedAt { get; set; }
    }

}
