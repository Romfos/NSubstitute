namespace NSubstitute.Routing.AutoValues;

internal sealed class AutoStringProvider : IAutoValueProvider
{
    public bool CanProvideValueFor(Type type) => type == typeof(string);

    public object GetValue(Type type)
    {
        return string.Empty;
    }
}