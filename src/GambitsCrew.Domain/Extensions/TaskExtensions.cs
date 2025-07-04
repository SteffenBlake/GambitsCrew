namespace GambitsCrew.Domain.Extensions;

public static class TaskExtensions
{
    public static async Task WhenAllWithCancellation(
        List<Task> tasks, CancellationTokenSource cts
    )
    {
        var taskList = tasks.Select(t => t.CancelOnThrowAsync(cts)).ToList();
        await Task.WhenAll(taskList);
    }

    public static async Task CancelOnThrowAsync(
        this Task task, CancellationTokenSource cts
    )
    {
        try
        {
            await task;
        }
        catch
        {
            cts.Cancel();
            throw;
        }
    }

    public static async Task<T> CancelOnThrowAsync<T>(
        this Task<T> task, CancellationTokenSource cts
    )
    {
        try
        {
            return await task;
        }
        catch
        {
            cts.Cancel();
            throw;
        }
    }
}
