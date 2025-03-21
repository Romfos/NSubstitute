using System.Reflection;

namespace NSubstitute.Routing.AutoValues;

internal sealed class AutoTaskProvider(Lazy<IReadOnlyCollection<IAutoValueProvider>> autoValueProviders) : IAutoValueProvider
{
    public bool CanProvideValueFor(Type type) => typeof(Task).IsAssignableFrom(type);

    public object GetValue(Type type)
    {
        if (!CanProvideValueFor(type))
            throw new InvalidOperationException();

        if (type.GetTypeInfo().IsGenericType)
        {
            var taskType = type.GetGenericArguments()[0];
            var valueProvider = autoValueProviders.Value.FirstOrDefault(vp => vp.CanProvideValueFor(taskType));

            var value = valueProvider == null ? GetDefault(type) : valueProvider.GetValue(taskType);
            var taskCompletionSourceType = typeof(TaskCompletionSource<>).MakeGenericType(taskType);
            var taskCompletionSource = Activator.CreateInstance(taskCompletionSourceType);
            taskCompletionSourceType.GetMethod(nameof(TaskCompletionSource<object>.SetResult))!.Invoke(taskCompletionSource, [value]);
            return taskCompletionSourceType.GetProperty(nameof(TaskCompletionSource<object>.Task))!.GetValue(taskCompletionSource, null)!;
        }
        else
        {
            var taskCompletionSource = new TaskCompletionSource<object?>();
            taskCompletionSource.SetResult(null);
            return taskCompletionSource.Task;
        }
    }

    private static object? GetDefault(Type type)
    {
        return type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
    }
}
