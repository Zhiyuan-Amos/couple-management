using System;
using Couple.Shared.Model;

namespace Couple.Client.Utility
{
    public static class CssHelper
    {
        public static string GetIcon(For @for)
        {
            return @for switch
            {
                For.Him => @"content: url(""icons/male.svg"")",
                For.Her => @"content: url(""icons/female.svg"")",
                For.Us => @"content: url(""icons/us.svg"")",
                _ => throw new ArgumentOutOfRangeException(nameof(@for), @for, null)
            };
        }
    }
}
