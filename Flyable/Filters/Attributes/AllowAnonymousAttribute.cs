namespace Flyable.Filters.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class AllowAnonymousAttribute: Attribute;