
using System.ComponentModel.DataAnnotations;

namespace todo_api.Models
{
    public class TodoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
    }

    public class TodoCreateDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required and cannot be empty.")]
        [StringLength(100, ErrorMessage = "Title cannot be more than 100 characters.")]
        public required string Title { get; set; }
    }

    public class TodoUpdateDTO
    {
        [MinLength(1, ErrorMessage = "Title cannot be empty.")]
        [StringLength(100, ErrorMessage = "Title cannot be more than 100 characters.")]
        public string? Title { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}

