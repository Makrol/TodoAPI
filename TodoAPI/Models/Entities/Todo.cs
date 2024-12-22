using System.ComponentModel.DataAnnotations;
using TodoAPI.Models.Dto;
namespace TodoAPI.Models.Entities
{
    public class Todo
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal PercentComplete { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;

        public Todo() { }

        public Todo(TodoDto model)
        {
            ExpiryDate = model.ExpiryDate;
            Title = model.Title;
            Description = model.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Description,ExpiryDate,PercentComplete,IsCompleted);
        }

    }
}
