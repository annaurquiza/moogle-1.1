using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static double GetCommonLettersPercent(this string @string, string stringToCompare)
    {
        double percent = 0;
        int totalCommonDistinctWords = @string.FlatString().Intersect(stringToCompare.FlatString()).Distinct().Count(); 
        string joinedWords = @string.FlatString() + stringToCompare.FlatString();
        int totalDistinctWords = joinedWords.Distinct().Count();
        if (totalDistinctWords > 0)
        {
            percent = ((double)totalCommonDistinctWords * (double)100) / (double)totalDistinctWords;
        }
        return percent;
    }
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

