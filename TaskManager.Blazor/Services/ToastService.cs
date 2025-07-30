using System.Timers;

namespace TaskManager.Blazor.Services;

public class ToastService
{
    public event Action<string, string, string>? OnShow;
    public void ShowSuccess(string message) => OnShow?.Invoke(message, "success", "Success");
    public void ShowError(string message) => OnShow?.Invoke(message, "danger", "Error");
    public void ShowInfo(string message) => OnShow?.Invoke(message, "info", "Info");
}
