namespace Couple.Shared.Model
{
    /// <summary>
    /// Based on https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/enum
    /// DO NOT use an enum for open sets (such as the operating system version, names of your friends, etc.).
    /// Therefore, static constants are used instead.
    /// </summary>
    public static class Command
    {
        public const string CreateIssue = "CreateIssue";
        public const string UpdateIssue = "UpdateIssue";
        public const string DeleteIssue = "DeleteIssue";
        public const string CompleteTask = "CompleteTask";
        public const string CreateImage = "CreateImage";
        public const string CreateEvent = "CreateEvent";
        public const string UpdateEvent = "UpdateEvent";
        public const string DeleteEvent = "DeleteEvent";
    }
}
