using System.Net.Http.Json;
using TaskManager.Blazor.Models;

namespace TaskManager.Blazor.Services;

public class TaskApiService
{
    private readonly HttpClient _http;
    public TaskApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<(IEnumerable<TaskItem> Tasks, int TotalCount)> GetTasksAsync(int page = 1, int pageSize = 10)
    {
        var response = await _http.GetAsync($"api/tasks?page={page}&pageSize={pageSize}");
        response.EnsureSuccessStatusCode();
        var tasks = await response.Content.ReadFromJsonAsync<IEnumerable<TaskItem>>();
        int totalCount = 0;
        if (response.Headers.TryGetValues("X-Total-Count", out var values))
            int.TryParse(values.FirstOrDefault(), out totalCount);
        return (tasks ?? new List<TaskItem>(), totalCount);
    }

    public async Task<TaskItem?> GetTaskAsync(Guid id)
    {
        return await _http.GetFromJsonAsync<TaskItem>($"api/tasks/{id}");
    }

    public async Task<TaskItem?> CreateTaskAsync(TaskItem task)
    {
        var response = await _http.PostAsJsonAsync("api/tasks", task);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TaskItem>();
    }

    public async Task<bool> UpdateTaskAsync(Guid id, TaskItem task)
    {
        var response = await _http.PutAsJsonAsync($"api/tasks/{id}", task);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/tasks/{id}");
        return response.IsSuccessStatusCode;
    }
}
