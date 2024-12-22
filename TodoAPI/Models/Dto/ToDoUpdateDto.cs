using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models.Dto
{
    public class ToDoUpdateDto
    {
        public DateTime? ExpiryDate { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

    }
}
