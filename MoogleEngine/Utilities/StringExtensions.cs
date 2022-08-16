using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static int[] GetAllIndexOf(this string @string, string str, StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
    {
        IList<int> allIndexOf = new List<int>();
        int index = @string.IndexOf(str, comparisonType);
        while(index != -1)
        {
            allIndexOf.Add(index);
            index = @string.IndexOf(str, index + 1, comparisonType);
        }
        return allIndexOf.ToArray();
    }

    public static string FlatString(this string @string)
    {
        return @string.Replace("á","a").Replace("é","e").Replace("í","i").Replace("ó","o").Replace("ú","u").ToLowerInvariant();
    }

    public static string Tokenize(this string @string)
    {
        // Strip all HTML.
      @string = Regex.Replace(@string, "<[^<>]+>", "");
      // Strip numbers.
      @string = Regex.Replace(@string, "[0-9]+", "number");
      // Strip urls.
      @string = Regex.Replace(@string, @"(http|https)://[^\s]*", "httpaddr");
      // Strip email addresses.
      @string = Regex.Replace(@string, @"[^\s]+@[^\s]+", "emailaddr");
      // Strip dollar sign.
      @string = Regex.Replace(@string, "[$]+", "dollar");
      // Strip usernames.
      @string = Regex.Replace(@string, @"@[^\s]+", "username");  

      return @string;
    }
}

