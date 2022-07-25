using System.Text.Json.Serialization;

namespace Couple.Shared.Models;

/// <summary>
///     Based on https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/enum
///     DO NOT use an enum for open sets (such as the operating system version, names of your friends, etc.).
///     The other alternatives are Static Constants or Value Objects.
///     Value Objects are preferred over Static Constants because it provides compile time safety.
/// </summary>
public class Command
{
    [JsonInclude]
    public string Name { get; private init; }
    
    // Required for deserialization
#pragma warning disable CS8618
    public Command() { }
#pragma warning restore CS8618

    private Command(string name) => Name = name;

    public static readonly Command CreateIssue = new("CreateIssue");
    public static readonly Command UpdateIssue = new("UpdateIssue");
    public static readonly Command DeleteIssue = new("DeleteIssue");
    public static readonly Command CompleteTask = new("CompleteTask");
    public static readonly Command CreateImage = new("CreateImage");
    public static readonly Command UpdateImage = new("UpdateImage");
    public static readonly Command DeleteImage = new("DeleteImage");

    private bool Equals(Command other) => Name == other.Name;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((Command)obj);
    }

    public override int GetHashCode() => Name.GetHashCode();
}
