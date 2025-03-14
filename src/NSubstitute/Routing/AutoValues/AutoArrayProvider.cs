namespace NSubstitute.Routing.AutoValues;

internal sealed class AutoArrayProvider : IAutoValueProvider
{
    public bool CanProvideValueFor(Type type) =>
        type.IsArray;

    public object GetValue(Type type)
    {
        var rank = type.GetArrayRank();
        var dimensionLengths = new int[rank];
        return Array.CreateInstance(type.GetElementType()!, dimensionLengths);
    }
}