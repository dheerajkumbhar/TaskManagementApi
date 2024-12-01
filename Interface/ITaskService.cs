using Models.TaskItem;
using TaskStatus = Models.TaskItem.TaskStatus;

namespace TaskManagementApi.Interface
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync(TaskStatus? status, DateTime? dueDate, int page, int pageSize);
        Task<TaskItem> GetTaskByIdAsync(int id);
        Task<TaskItem> CreateTaskAsync(TaskItem task);
        Task<TaskItem> UpdateTaskAsync(int id, TaskItem task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
