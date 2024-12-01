using Microsoft.AspNetCore.Mvc;
using Models.TaskItem;
using TaskManagementApi.Interface;
using TaskStatus = Models.TaskItem.TaskStatus;
using Swashbuckle.AspNetCore.Annotations;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskService taskService, ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    // GET: api/Tasks
    [HttpGet]
    [SwaggerResponse(200, "Successfully retrieved tasks", typeof(IEnumerable<TaskItem>))]
    [SwaggerResponse(404, "No tasks found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks([FromQuery] TaskStatus? status, [FromQuery] DateTime? dueDate, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var tasks = await _taskService.GetTasksAsync(status, dueDate, page, pageSize);
            if (tasks == null || !tasks.Any())
            {
                _logger.LogInformation("No tasks found.");
                return NotFound("No tasks found.");
            }
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching tasks.");
            return StatusCode(500, "Internal server error");
        }
    }

    // GET: api/Tasks/{id}
    [HttpGet("{id}")]
    [SwaggerResponse(200, "Successfully retrieved task", typeof(TaskItem))]
    [SwaggerResponse(400, "Invalid Task ID")]
    [SwaggerResponse(404, "Task not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        try
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                _logger.LogWarning($"Task with id {id} not found.");
                return NotFound($"Task with id {id} not found.");
            }
            return Ok(task);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid Task ID.");
            return BadRequest();  // Return 400 with the validation error message
        }
    }

    // POST: api/Tasks
    [HttpPost]
    [SwaggerResponse(201, "Task created successfully", typeof(TaskItem))]
    [SwaggerResponse(400, "Invalid task data")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<TaskItem>> PostTask(TaskItem task)
    {
        if (string.IsNullOrEmpty(task.Title) || task.DueDate == default)
        {
            _logger.LogWarning("Task title or due date is missing.");
            return BadRequest("Title and Due Date are required.");
        }

        try
        {
            var createdTask = await _taskService.CreateTaskAsync(task);
            _logger.LogInformation($"Task created with ID: {createdTask.Id}");
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException occurred while creating the task.");
            return BadRequest("Invalid task data. Please check the input.");
        }
    }

    // PUT: api/Tasks/{id}
    [HttpPut("{id}")]
    [SwaggerResponse(200, "Task updated successfully", typeof(TaskItem))]
    [SwaggerResponse(400, "Task ID mismatch")]
    [SwaggerResponse(404, "Task not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> PutTask(int id, TaskItem task)
    {
        if (id != task.Id)
        {
            _logger.LogWarning($"Task ID mismatch. Provided: {id}, Expected: {task.Id}");
            return BadRequest("Task ID mismatch.");
        }

        try
        {
            var updatedTask = await _taskService.UpdateTaskAsync(id, task);
            if (updatedTask == null)
            {
                _logger.LogWarning($"Task with id {id} not found for update.");
                return NotFound($"Task with id {id} not found.");
            }
            _logger.LogInformation($"Task updated: {updatedTask.Title}");
            return Ok(updatedTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating the task.");
            return StatusCode(500, "Internal server error");
        }
    }

    // DELETE: api/Tasks/{id}
    [HttpDelete("{id}")]
    [SwaggerResponse(204, "Task deleted successfully")]
    [SwaggerResponse(400, "Invalid task ID")]
    [SwaggerResponse(404, "Task not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        if (id <= 0)
        {
            _logger.LogError($"Invalid ID: {id}. ID must be a positive number.");
            return BadRequest("Invalid task ID. ID must be a positive number.");
        }
        try
        {
            var success = await _taskService.DeleteTaskAsync(id);

            if (!success)
            {
                _logger.LogWarning($"Task with id {id} not found for deletion.");
                return NotFound($"Task with id {id} not found.");
            }
            _logger.LogInformation($"Task with id {id} deleted successfully.");
            return NoContent();  // 204 No Content, indicating successful deletion with no content to return
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting the task.");
            return StatusCode(500, "Internal server error");
        }
    }
}
