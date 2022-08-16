namespace MoogleEngine.Core;

public static class SearchEngine
{
    public static SearchResult Search(Library library, SearchCriteria critera)
    {
        IList<SearchItem> items = new List<SearchItem>();
        IList<Document> docsToSearch  = new List<Document>();
        string suggestion = "";
        double generalWeightSearch = 0;

         //Si hay palabras excluyentes, sacar documentos que las contengan
         if (critera.ExcludeWords.Count() > 0)
         {
            foreach (var doc in library.Documents)
            {
               if(!critera.ExcludeWords.Any(x => doc.FullContent.FlatString().Contains(x.FlatString())))
               {
                  docsToSearch.Add(doc);
               }
            }  
         }
         else
         {
            docsToSearch = library.Documents;
         }
         

         //si hay palabras obligatorias sacar documentos que no las contengan
         if (critera.MustWords.Count() > 0)
         {
            var titlesToRemove = new List<String>();
            foreach (var doc in docsToSearch)
            {
               if (!critera.MustWords.Any(x => doc.FullContent.FlatString().Contains(x.FlatString())))
               {
                  titlesToRemove.Add(doc.Title);
               }
            }

            docsToSearch = docsToSearch.Where(x => !titlesToRemove.Contains(x.Title)).ToList();
         }

        //Buscando coincidencias basicas
        //TODO: stopwords en criterio?
         string[] includeStems = library.GetRelevantWords(critera.Words.ToArray());
         if (includeStems.Count() > 0)
         {
            docsToSearch = docsToSearch.Where(x => (x.StemmedVocabulary.Intersect(includeStems).Count() > 0) || ( library.LibraryStemmer.GetSteamWords(x.Title.FlatString().Split()).Intersect(includeStems).Count() > 0)).ToList();
         }

         //Calculando la valoracion de cada documento
        foreach (var doc in docsToSearch)
         {
            //Relevancia del documento de acuerdo a la biblioteca
            double relevance = library.GetTF_IDF_WEIGHT(doc, critera);
            //guardar el peso básico del resultado para determinar si hay que buscar alguna sugerencia
            generalWeightSearch += relevance;
            //Relevancia del documento según la búsqueda
            relevance += ScoreCalculator.GetComputedScore(doc, critera, library.LibraryStemmer);
            //buscando la palabra más relevante para construir snippet
            string mostRelevantWord = library.GetMostRelevantWord(doc, critera.Words.ToArray());
            SearchItem docFound = new SearchItem(doc.Title, doc.GetSnippet(mostRelevantWord, 300), (float)relevance);
            items.Add(docFound);            
         }

        //Comprobar si no hubo coincidencias
         if (generalWeightSearch < 1 || docsToSearch.Count() == 0)
         {
            //buscar similitud mayor entre vocabulario y elementos del criterio
            suggestion = "Relevancia del resultado insuficiente. Mostrar sugerencia...";
         }

         //Ordenando elementos del resultado
         SearchItem[] orderedItems = items.OrderByDescending(d => d.Score).ToArray();
         return new SearchResult(orderedItems, suggestion);
    }
}