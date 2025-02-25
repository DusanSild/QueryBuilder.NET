namespace QueryBuilderDotNet.Utils;

public static class StringExtensions
{
    /// <summary>
    /// Converts the string into camelCase
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToCamelCase(this string str)
    {
        return string.IsNullOrWhiteSpace(str) ? str : string.Concat(str[..1].ToLower(), str.AsSpan(1));
    }
}