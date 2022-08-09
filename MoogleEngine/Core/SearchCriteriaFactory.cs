namespace MoogleEngine.Core;

public static class SearchCriteriaFactory
{
    private const string EXCLUSION_OPERATOR = "!";
    private const string MUST_INCLUDE_OPERATOR = "^";
    private const string INCREASE_RANK_OPERATOR = "*";
    private const string NEARBY_OPERATOR = "~";

    public static SearchCriteria BuildCriteriaFromQuery(string query)
    {
        string flatQuery = query.FlatString();
        List<string> pWords = flatQuery.Split(" ").ToList();

        SearchCriteria criteria = new SearchCriteria();

        int pIndex = -1;

        foreach (string w in pWords)
        {
            pIndex ++;

            //buscando palabras para excluir 
            if (w.IndexOf(EXCLUSION_OPERATOR) >= 0)
            {
                criteria.AddExcludeWord(ClearWordFromOperators(w));
                continue;
            }

            criteria.AddWord(ClearWordFromOperators(w));

            //buscando palabras que deben aparecer
            if (w.IndexOf(MUST_INCLUDE_OPERATOR) >= 0)
            {
                criteria.AddMustWord(ClearWordFromOperators(w));
            }

            //buscando palabras con operador de importancia
            if (w.IndexOf(INCREASE_RANK_OPERATOR) >= 0)
            {
                int relevance = w.GetAllIndexOf(INCREASE_RANK_OPERATOR, StringComparison.InvariantCultureIgnoreCase).Count();
                criteria.AddScoredWord(ClearWordFromOperators(w), relevance);
                continue;
            }

            //buscando palabras con operador de cercania
            if (w.IndexOf(NEARBY_OPERATOR) >= 0)
            {
                //el operador de cercania no puede estar en la primera posicion
                if (pIndex == 0)
                {
                    continue;
                }
                //el operador puede estar separado de las palabras o puede estar empotrado en medio de las palabras
                //ejem: palabra1 ~ palabra2 || palabra1~palabra2
                try
                {
                    //caso 1: palabra1 ~ palabra2
                    if (w.Trim().Length == 1) 
                    {                  
                        criteria.AddRelatedWords(pWords.ElementAt(pIndex-1), pWords.ElementAt(pIndex+1));
                    }
                    else
                    {
                        string[] nWords = w.Split(NEARBY_OPERATOR);
                        for (int i = 0; i < nWords.Length - 1; i++)
                        {
                            criteria.AddRelatedWords(nWords[i],nWords[i+1]);
                        }
                    }
                }
                catch (System.Exception)
                {                    
                    throw new ArgumentException($"El operador [{NEARBY_OPERATOR}] parece estar en una posición incorrecta.");
                }
            }
        }
        return criteria;
    }

    private static string ClearWordFromOperators(string word)
    {
        return word.Replace(EXCLUSION_OPERATOR,"").Replace(MUST_INCLUDE_OPERATOR,"").Replace(INCREASE_RANK_OPERATOR,"").Replace(NEARBY_OPERATOR,"");
    }

}