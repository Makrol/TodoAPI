using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Models.Dto;
using TodoAPI.Models.Entities;

namespace TodoAPI.Services
{
    public interface ITodoService
    {
        public Task<Todo> Add(TodoDto model);
        public Task<List<Todo>> GetAll();
        public Task<Todo?> GetById(Guid id);
        public Task<List<Todo>> GetFromRange(DateTime lower, DateTime upper);
        public Task<Todo> Update(Todo todo, ToDoUpdateDto model);
        public Task Delete(Todo todo);
        public Task<Todo> SetPercentComplete(Todo todo, int percent);
        public Task<Todo> MarkAsDone(Todo todo);
    }
}
