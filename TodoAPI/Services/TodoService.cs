using Microsoft.EntityFrameworkCore;
using System;
using TodoAPI.Data;
using TodoAPI.Models.Dto;
using TodoAPI.Models.Entities;

namespace TodoAPI.Services
{
    public class TodoService : ITodoService
    {
        private readonly AppDbContext _context;

        public TodoService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Todo> Add(TodoDto model)
        {
            var todo = new Todo(model);
            _context.todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<List<Todo>> GetAll()
        {
            return await _context.todos.ToListAsync();
        }

        public async Task<Todo?> GetById(Guid id)
        {
            return await _context.todos.FirstOrDefaultAsync((o => o.Id == id));
        }

        public async Task<Todo> Update(Todo todo,ToDoUpdateDto model)
        {
            if(model.ExpiryDate!=null)
                todo.ExpiryDate = model.ExpiryDate.Value;
            if (model.Title != null)
                todo.Title = model.Title;
            if(model.Description!=null)
                todo.Description = model.Description;

            _context.todos.Update(todo);
            await _context.SaveChangesAsync();
            return todo;
        }
        public async Task Delete(Todo todo)
        {
            _context.todos.Remove(todo);
            await _context.SaveChangesAsync();
        }

        public async Task<Todo> SetPercentComplete(Todo todo, int percent)
        {
            todo.PercentComplete = percent;
            _context.todos.Update(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<Todo> MarkAsDone(Todo todo)
        {
            if (todo.IsCompleted)
                return todo;
            
            todo.IsCompleted = true;
            _context.todos.Update(todo);
            await _context.SaveChangesAsync();

            return todo;
        }

        public async Task<List<Todo>> GetFromRange(DateTime lower, DateTime upper)
        {
            return await _context.todos
            .Where(todo => todo.ExpiryDate >= lower && todo.ExpiryDate <= upper)
            .ToListAsync();
        }
    }
}
