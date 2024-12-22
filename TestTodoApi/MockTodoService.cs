using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.Models.Dto;
using TodoAPI.Models.Entities;
using TodoAPI.Services;

namespace TestTodoApi
{
    internal class MockTodoService : ITodoService
    {
        public bool HasData = false;
        public bool Completed = false;

        public Task<Todo> Add(TodoDto model)
        {
            return Task.FromResult(new Todo(model));
        }

        public Task Delete(Todo todo)
        {
            return Task.FromResult(new Todo());
        }

        public Task<List<Todo>> GetAll()
        {
            if(HasData)
                return Task.FromResult(new List<Todo> { new Todo(),new Todo() });
            else
                return Task.FromResult(new List<Todo> {});

        }

        public Task<Todo?> GetById(Guid id)
        {
            if(id==new Guid("329726b9-e337-4205-a86b-8f52afa0b341"))
                return Task.FromResult<Todo?>(new Todo { IsCompleted = Completed});
            return Task.FromResult<Todo?>(null);
        }

        public Task<List<Todo>> GetFromRange(DateTime lower, DateTime upper)
        {
            if(HasData)
                return Task.FromResult(new List<Todo> { new Todo(), new Todo() });
            else
                return Task.FromResult(new List<Todo> {});
        }

        public Task<Todo> MarkAsDone(Todo todo)
        {
            if (todo.IsCompleted)
                return Task.FromResult(todo);

            todo.IsCompleted = true;

            return Task.FromResult(todo);
        }

        public Task<Todo> SetPercentComplete(Todo todo, int percent)
        {
            todo.PercentComplete = percent;
            return Task.FromResult(todo);
        }

        public Task<Todo> Update(Todo todo, ToDoUpdateDto model)
        {
            if (model.ExpiryDate != null)
                todo.ExpiryDate = model.ExpiryDate.Value;
            if (model.Title != null)
                todo.Title = model.Title;
            if (model.Description != null)
                todo.Description = model.Description;

           
            return Task.FromResult(todo);
        }
    }
}
