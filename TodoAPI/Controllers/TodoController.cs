using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Data;
using TodoAPI.Models.Entities;
using TodoAPI.Services;
using TodoAPI.Models.Dto;
using TodoAPI.Models.Enum;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public TodoController(ITodoService todoService) 
        { 
            _todoService = todoService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">The data model used to create the task.</param>
        /// <returns>
        ///  - <see cref="Created"/> If the task has been correctly established.
        ///  - <see cref="BadRequest"/> If there was a data validation error in the model.
        /// </returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Add(TodoDto model)
        {
            if (ModelState.IsValid) 
            {
                await _todoService.Add(model);
                return Created();
            }
            return BadRequest();
        }

        /// <summary>
        /// Downloading all tasks
        /// </summary>
        /// <returns>
        ///  - <see cref="NoContent"/> If there are no tasks to download.
        ///  - <see cref="Ok"/> If the tasks were downloaded correctly.
        /// </returns>
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var todoList = await _todoService.GetAll();
            if (todoList.Count>0)
            {
                return Ok(todoList);
            }
            return NoContent();
        }

        /// <summary>
        /// Downloading a specific task
        /// </summary>
        /// <param name="id">Unique task identifier (todo).</param>
        /// <returns>
        ///  - <see cref="NotFound"/> If a task with the specified ID does not exist.
        ///  - <see cref="Ok"/> If the task with the given id exists and has been returned.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (ModelState.IsValid)
            {
                var todo = await _todoService.GetById(id);
                if(todo!=null)
                {
                    return Ok(todo);
                }
                return NotFound();
            }
            return BadRequest();
        }

        /// <summary>
        /// Downloading incoming tasks
        /// </summary>
        /// <param name="type">Range type times of upcoming events</param>
        /// <returns>
        ///  - <see cref="BadRequest"/> The specified compartment type is not correct.
        ///  - <see cref="NoContent"/> There are no tasks in the given time frame.
        ///  - <see cref="Ok"/> Tasks correctly downloaded and returned.
        /// </returns>
        [HttpGet("Incoming")]
        public async Task<IActionResult> GetIncoming(IncomingTypes type)
        {
            /////////////////
            var startDay = DateTime.UtcNow.Date;
            var endDay = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);
            List<Todo> listTodo;
            switch(type)
            {
                case IncomingTypes.Today:
                    listTodo = await _todoService.GetFromRange(startDay,endDay);
                    break;
                case IncomingTypes.NextDay:
                    listTodo = await _todoService.GetFromRange(startDay.AddDays(1),endDay.AddDays(1));
                    break;
                case IncomingTypes.CurrentWeek:
                    int numberOfDay = (int)startDay.DayOfWeek-1;
                    if (numberOfDay == -1)
                        numberOfDay = 6;
                    listTodo = await _todoService.GetFromRange(startDay.AddDays(-numberOfDay), endDay.AddDays(6-numberOfDay));
                    break;
                default:
                    return BadRequest();
            }
            if (listTodo.Count > 0)
                return Ok(listTodo);
            return NoContent();
        }

        /// <summary>
        /// Update task data
        /// </summary>
        /// <param name="id">Unique task identifier (todo).</param>
        /// <param name="model">Data model used for updates.</param>
        /// <returns> 
        ///  - <see cref="NotFound"/> If a task with the specified ID does not exist.
        ///  - <see cref="NoContent"/> The operation was successful but no changes were made because the data in the model and the task are the same.
        ///  - <see cref="Ok"/> If the update was successful and the task was returned.
        ///  - <see cref="BadRequest"/> If there was a data validation error in the model.
        ///  </returns>
        [HttpPatch("Update")]
        public async Task<IActionResult> Update(Guid id, ToDoUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                var todo = await _todoService.GetById(id);

                if (todo == null)
                    return NotFound();

                var todoHash = todo.GetHashCode();

                await _todoService.Update(todo, model);

                var todoUpdatedHash = todo.GetHashCode();
                if (todoHash == todoUpdatedHash)
                    return NoContent();

                return Ok(todo);
            }
            return BadRequest();
        }

        /// <summary>
        /// Deletion of the indicated task
        /// </summary>
        /// <param name="id">Unique task identifier (todo).</param>
        /// <returns>
        ///  - <see cref="NotFound"/> If a task with the specified ID does not exist.
        ///  - <see cref="NoContent"/> If the task has been correctly deleted.
        /// </returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var todo = await _todoService.GetById(id);

            if (todo == null)
                return NotFound();

            await _todoService.Delete(todo);

            return NoContent();
        }

        /// <summary>
        /// Changing the percentage of task completion.
        /// </summary>
        /// <param name="id">Unique task identifier (todo).</param>
        /// <param name="percent">Progress of task completion in percentage</param>
        /// <returns>
        ///  - <see cref="BadRequest"/> If the percentage of task completion is not between 0 and 100.
        ///  - <see cref="NotFound"/> If a task with the specified ID does not exist.
        ///  - <see cref="Ok"/> If the progrs change was saved and the task was returned.
        /// </returns>
        [HttpPost("SetPercentComplete")]
        public async Task<IActionResult> SetPercentComplete(Guid id, int percent)
        {
            if (percent < 0||percent > 100)
                return BadRequest();
            
            var todo = await _todoService.GetById(id);
            if(todo == null)
                return NotFound();

            await _todoService.SetPercentComplete(todo, percent);
            return Ok(todo);
        }

        /// <summary>
        /// Marking the task as completed.
        /// </summary>
        /// <param name="id">Unique task identifier (todo).</param>
        /// <returns>
        /// Zwraca:
        /// - <see cref="NotFound"/> If a task with the specified ID does not exist.
        /// - <see cref="NoContent"/> If the task was already marked as completed and no changes were made.
        /// - <see cref="Ok"/>If the operation to mark as completed has finished and a modiﬁed task has been returned 
        /// </returns>
        [HttpPost("MarkAsDone")]
        public async Task<IActionResult> MarkAsDone(Guid id)
        {
            var todo = await _todoService.GetById(id);
            
            if (todo == null)
                return NotFound();

            var orginalCompleted = todo.IsCompleted;

            if (todo.IsCompleted)
                return NoContent();

            await _todoService.MarkAsDone(todo);

            if(todo.IsCompleted)
                return Ok(todo);
            return NoContent();
        }
    }
}
