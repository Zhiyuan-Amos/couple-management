using Couple.Shared.Models;

namespace Couple.Client.Shared.Helpers;

public static class CssHelper
{
    public static string GetIcon(For @for) =>
        @for switch
        {
            For.Him => @"content: url(""icons/male.svg"")",
            For.Her => @"content: url(""icons/female.svg"")",
            For.Us => @"content: url(""icons/us.svg"")",
            _ => throw new ArgumentOutOfRangeException(nameof(@for), @for, null)
        };

    public static string GetBackgroundColor(int index) =>
        (index % 3) switch
        {
            0 => "background-color:#FF99C8",
            1 => "background-color:#A6BFAD",
            2 => "background-color:#FCF6BD",
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
        };
}
