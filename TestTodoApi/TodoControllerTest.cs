using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models.Entities;
using TodoAPI.Controllers;
using TodoAPI.Services;
using TodoAPI.Models.Dto;
using TodoAPI.Models.Enum;
using Microsoft.AspNetCore.Http;

namespace TestTodoApi
{
    public class TodoControllerTest
    {

        private readonly ITodoService mockService;
        private readonly TodoController controller;

        public TodoControllerTest()
        {
            mockService = new MockTodoService();
            controller = new TodoController(mockService);
        }
        [Fact]
        public async Task GetAllDataExist()
        {
            ((MockTodoService)mockService).HasData = true;
            var result = await controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var todoList = Assert.IsAssignableFrom<List<Todo>>(okResult.Value);
            Assert.Equal(2, todoList.Count);
        }
        [Fact]
        public async Task GetAllDataNotExist()
        {
            ((MockTodoService)mockService).HasData = false;
            var result = await controller.GetAll();

            var okResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Add()
        {
            var result = await controller.Add(new TodoDto());
            var okResult = Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task GetById()
        {
            var result = await controller.GetById(new Guid("329726b9-e337-4205-a86b-8f52afa0b341"));

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<Todo>(okResult.Value);
        }
        [Fact]
        public async Task GetByIdNotExist()
        {
            var result = await controller.GetById(Guid.Empty);

            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetIncomingToday()
        {
            ((MockTodoService)mockService).HasData = true;
            var result = await controller.GetIncoming(IncomingTypes.Today);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var todoList = Assert.IsAssignableFrom<List<Todo>>(okResult.Value);
            Assert.Equal(2, todoList.Count);
        }
        [Fact]
        public async Task GetIncomingNextDay()
        {
            ((MockTodoService)mockService).HasData = true;
            var result = await controller.GetIncoming(IncomingTypes.NextDay);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var todoList = Assert.IsAssignableFrom<List<Todo>>(okResult.Value);
            Assert.Equal(2, todoList.Count);
        }
        [Fact]
        public async Task GetIncomingCurrentWeek()
        {
            ((MockTodoService)mockService).HasData = true;
            var result = await controller.GetIncoming(IncomingTypes.CurrentWeek);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var todoList = Assert.IsAssignableFrom<List<Todo>>(okResult.Value);
            Assert.Equal(2, todoList.Count);
        }
        [Fact]
        public async Task GetIncomingInvalidType()
        {
            ((MockTodoService)mockService).HasData = true;
            var result = await controller.GetIncoming((IncomingTypes)10);
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task GetIncomingNotExist()
        {
            ((MockTodoService)mockService).HasData = false;
            var result = await controller.GetIncoming(IncomingTypes.CurrentWeek);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update()
        {
            var result = await controller.Update(new Guid("329726b9-e337-4205-a86b-8f52afa0b341"), new ToDoUpdateDto{ Description = "Test" });
            var resultValue = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<Todo>(resultValue.Value);
        }

        [Fact]
        public async Task UpdateNotExist()
        {
            var result = await controller.Update(Guid.Empty, new ToDoUpdateDto());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateNotChange()
        {
            var result = await controller.Update(new Guid("329726b9-e337-4205-a86b-8f52afa0b341"), new ToDoUpdateDto());
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete()
        {
            var result = await controller.Delete(new Guid("329726b9-e337-4205-a86b-8f52afa0b341"));
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteNotExist()
        {
            var result = await controller.Delete(Guid.Empty);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SetPercentComplete()
        {
            var result = await controller.SetPercentComplete(new Guid("329726b9-e337-4205-a86b-8f52afa0b341"), 10);
            var resultValue = Assert.IsType<OkObjectResult>(result);
            var todo = Assert.IsAssignableFrom<Todo>(resultValue.Value);

            Assert.Equal(10, todo.PercentComplete);
        }

        [Fact]
        public async Task SetPercentCompleteNotExist()
        {
            var result = await controller.SetPercentComplete(Guid.Empty, 10);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SetPercentCompleteInvalidPercent()
        {
            var result = await controller.SetPercentComplete(Guid.Empty, -10);
            Assert.IsType<BadRequestResult>(result);

            result = await controller.SetPercentComplete(Guid.Empty, 110);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task MarkAsDone()
        {
            ((MockTodoService)mockService).Completed = false;
            var result = await controller.MarkAsDone(new Guid("329726b9-e337-4205-a86b-8f52afa0b341"));
            var resultValue = Assert.IsType<OkObjectResult>(result);

            var todo = Assert.IsAssignableFrom<Todo>(resultValue.Value);

            Assert.True(todo.IsCompleted);

        }

        [Fact]
        public async Task MarkAsDoneNotExist()
        {
            ((MockTodoService)mockService).Completed = false;
            var result = await controller.MarkAsDone(Guid.Empty);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task MarkAsDoneNotChange()
        {
            ((MockTodoService)mockService).Completed = true;
            var result = await controller.MarkAsDone(new Guid("329726b9-e337-4205-a86b-8f52afa0b341"));
            var resultValue = Assert.IsType<NoContentResult>(result);
        }
    }
}