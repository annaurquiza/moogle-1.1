namespace MoogleEngine;
using MoogleEngine.Core;

public static class Moogle
{
    private const string FILESLOCATION = "../Content";
    private static Library library = new Library(FILESLOCATION);

    public static SearchResult Query(string query) 
    {
        LoadLibrary();

        SearchCriteria criteria = SearchCriteriaFactory.BuildCriteriaFromQuery(query);

        IList<SearchItem> items = new List<SearchItem>();

        string suggestion = "";


        if(items.Count == 0)
        {
            suggestion = "algo no encontrado por el momento...";
        }

        return new SearchResult(items.ToArray(), suggestion);
    }


    public static void LoadLibrary()
    {
        if (library == null)
        {
           library = new Library(FILESLOCATION);
        }
    }
}
