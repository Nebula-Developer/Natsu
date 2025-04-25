using System.Diagnostics.CodeAnalysis;

public static class DynamicProperties {
    public const DynamicallyAccessedMemberTypes Accessible = AccessibleMethods | AccessibleProperties | AccessibleFields;
    public const DynamicallyAccessedMemberTypes AccessibleMethods = DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods;
    public const DynamicallyAccessedMemberTypes AccessibleProperties = DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties;
    public const DynamicallyAccessedMemberTypes AccessibleFields = DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields;
}
