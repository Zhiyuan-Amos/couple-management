namespace Couple.Shared.Model;

/// <summary>
///     Based on https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/enum
///     DO NOT use an enum for open sets (such as the operating system version, names of your friends, etc.).
///     Therefore, static constants are used instead.
/// </summary>
public static class Command
{
    public const string Create = "Create";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string Complete = "Complete";
}
