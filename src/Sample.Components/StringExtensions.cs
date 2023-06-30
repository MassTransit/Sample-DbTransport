namespace Sample.Components;

using System.Text.RegularExpressions;


public static partial class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var startUnderscores = MyRegex().Match(input);
        return startUnderscores + MyRegex1().Replace(input, "$1_$2").ToLower();
    }

    [GeneratedRegex("^_+")]
    private static partial Regex MyRegex();

    [GeneratedRegex("([a-z0-9])([A-Z])")]
    private static partial Regex MyRegex1();
}