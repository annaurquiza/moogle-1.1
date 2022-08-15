namespace MoogleEngine;
using MoogleEngine.Core;

public static class Moogle
{
    private const string FILESLOCATION = "../Content";
    private static Library library = new Library(FILESLOCATION);

    public static SearchResult Query(string query) 
    {
        //Por si no se ha cargado la biblioteca
        LoadLibrary();
        //Construir el criterio de búsqueda
        SearchCriteria criteria = SearchCriteriaFactory.BuildCriteriaFromQuery(query);
        return SearchEngine.Search(library, criteria);
    }

    public static void LoadLibrary()
    {
        if (library == null)
        {
           library = new Library(FILESLOCATION);
        }
    }
}
