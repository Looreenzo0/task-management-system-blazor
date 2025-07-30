using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskDbContext _context;
        public TasksController(TaskDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks - Get all tasks 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Invalid pagination parameters.");
            var totalCount = await _context.Tasks.CountAsync();
            var tasks = await _context.Tasks
                .OrderByDescending(t => t.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            return Ok(tasks);
        }

        // GET: api/tasks/{id} - Get task by ID 
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // POST: api/tasks - Create new task
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItem task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(task.Title))
                return BadRequest("Title is required.");

            if (task.DueDate.HasValue && task.DueDate < DateTime.UtcNow)
                return BadRequest("Due date cannot be in the past.");

            task.Id = Guid.NewGuid();
            task.CreatedDate = DateTime.UtcNow;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/tasks/{id} - Update existing task 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskItem task)
        {
            if (id != task.Id)
                return BadRequest("ID in URL does not match ID in body.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(task.Title))
                return BadRequest("Title is required.");

            if (task.DueDate.HasValue && task.DueDate < DateTime.UtcNow)
                return BadRequest("Due date cannot be in the past.");

            var existing = await _context.Tasks.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.Status = task.Status;
            existing.Priority = task.Priority;
            existing.DueDate = task.DueDate;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/tasks/{id} -  Delete task 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
