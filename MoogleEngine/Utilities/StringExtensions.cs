public static class StringExtensions
{
    public static int[] GetAllIndexOf(this string @string, string str, StringComparison comparisonType)
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
}

