namespace MoogleEngine;
using MoogleEngine.Core;

public static class Moogle
{
    private static IList<Document> library;

    public static SearchResult Query(string query) {

        if (library == null)
        {
           LoadLibrary();
        }

        string[] queryWords = query.Split();

        IList<SearchItem> items = new List<SearchItem>();

        string suggestion = "";

        foreach (Document doc in library)
        {
         if (doc.ContainsAnyWordIn(query))
         {
           items.Add( new SearchItem(doc.Title, "Lorem ipsum dolor sit amet", 1));
         }
        }

        if(items.Count == 0)
        {
            suggestion = "algo no encontrado por el momento...";
        }

        return new SearchResult(items.ToArray(), suggestion);
    }


    public static void LoadLibrary()
    {
        library = Library.Load("../Content");
    }
}
