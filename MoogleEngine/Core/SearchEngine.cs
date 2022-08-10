namespace MoogleEngine.Core;

public static class SearchEngine
{
    public static SearchResult Search(Library library, SearchCriteria critera)
    {
        IList<SearchItem> items = new List<SearchItem>();
        string suggestion = "";

         //Sacando documentos con palabras excluyentes
         string[] excludeStems = library.LibraryStemmer.GetSteamWords(critera.ExcludeWords.ToArray());
         IList<Document> docsToSearch = library.Documents.Where(x => x.StemmedVocabulary.ToArray().Intersect(excludeStems).Count() == 0).ToList();

        //Buscando coincidencias basicas
         string[] includeStems = library.LibraryStemmer.GetSteamWords(critera.Words.ToArray());
         if (includeStems.Count() > 0)
         {
            docsToSearch = docsToSearch.Where(x => x.StemmedVocabulary.Intersect(includeStems).Count() > 0).ToList();
         }

         //Calculando la valoracion de cada documento
        foreach (var doc in docsToSearch)
         {
            library.ComputeDocumentRelevance(doc, critera);
            SearchItem docFound = new SearchItem(doc.Title, doc.GetSnippet(includeStems), (float)doc.Relevance);
            items.Add(docFound);
         }

        //Comprobar si no hubo coincidencias
         if (docsToSearch.Count() == 0)
         {
            //buscar similitud mayor entre vocabulario y elementos del criterio

         }



        return new SearchResult(items.ToArray(), suggestion);
    }
}