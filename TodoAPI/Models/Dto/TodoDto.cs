using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models.Dto
{
    public class TodoDto
    {
        [Required(ErrorMessage = "ExpiryDate is required.")]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = "";
    }
}
