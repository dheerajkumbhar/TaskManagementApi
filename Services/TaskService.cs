using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Interface;
using Microsoft.EntityFrameworkCore.InMemory;
using Models.TaskItem;
using TaskStatus = Models.TaskItem.TaskStatus;

namespace TaskManagementApi.Services
{

    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TaskService> _logger;
        public TaskService(AppDbContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            if (task == null)
            {
                _logger.LogError("Task is null.");
                throw new ArgumentNullException(nameof(task), "Task cannot be null.");
            }
            try
            {
                // Add the task to the database
                _context.Tasks.Add(task);

                // Save changes to the database and await the result
                await _context.SaveChangesAsync();

                // Log task creation information
                _logger.LogInformation($"Task created: {task.Title}");

                // Return the created task
                return task;
            }
            catch (ArgumentNullException ex)
            {
                // Handle argument null exception specifically
                _logger.LogError($"ArgumentNullException: {ex.Message}");
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-related errors
                _logger.LogError($"Database error while creating task: {dbEx.Message}");
                throw new InvalidOperationException("Error saving task to the database.", dbEx);
            }
        }


        public async Task<bool> DeleteTaskAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Invalid ID: {id}. ID must be a positive number.");
                throw new ArgumentException("Invalid task ID. ID must be a positive number.");
            }
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                _logger.LogWarning($"Task with ID {id} not found.");
                return false;  // Task not found, return false
            }
            _context.Tasks.Remove(task);
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Task deleted: {task.Title}");
                return true;  // Return true if task is deleted successfully
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the task.");
                throw;  // Rethrow the exception to be handled by the controller
            }
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Task ID must be greater than 0.", nameof(id));
            }

            return await _context.Tasks.FindAsync(id);
        }


        public async Task<IEnumerable<TaskItem>> GetTasksAsync(TaskStatus? status, DateTime? dueDate, int page, int pageSize)
        {
            var tasksQuery = _context.Tasks.AsQueryable();

            if (status.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.Status == status.Value);
            }

            if (dueDate.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.DueDate <= dueDate.Value);
            }

            return await tasksQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<TaskItem> UpdateTaskAsync(int id, TaskItem task)
        {
            if (task == null && id <= 0)
            {
                _logger.LogError("Task or id object is null.");
                throw new ArgumentNullException(nameof(task), "Task cannot be null.");
            }
            if (string.IsNullOrEmpty(task.Title) || task.DueDate == default)
            {
                _logger.LogWarning("Task title or due date is missing.");
                throw new ArgumentException("Task title and due date are required.");
            }
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                _logger.LogWarning($"Task with ID {id} not found.");
                return null;  // Task not found, return null or handle it further in the controller
            }
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.DueDate = task.DueDate;
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Task updated: {existingTask.Title}");
                return existingTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the task.");
                throw;  // Rethrow the exception to be handled by the controller
            }
        }

    }
}
