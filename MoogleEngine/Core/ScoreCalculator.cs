using Annytab.Stemmer;

namespace MoogleEngine.Core;

public class ScoreCalculator
{
    public static double GetComputedScore(Document document, SearchCriteria criteria, Stemmer stemmer)
    {
        double score = 0;
        
        //puntos por coincidencia en título
        //si el titulo coincide agregar un punto
        if (document.Title.ToLower().Contains(criteria.LiteralSearch.ToLower()))
        {   
            score += 1;
        }
        else
        //si no coincide agregar una fraccion proporcional a la cantidad de palabras coincidentes
        {
            string[] stemmedTitle = stemmer.GetSteamWords(document.Title.Split());
            string[] stemmedSearch = stemmer.GetSteamWords(criteria.LiteralSearch.Split());
            string[] match = stemmedTitle.ToList().Intersect(stemmedSearch.ToList()).ToArray();
            if (match.Count() > 0)
            {
                double value = (stemmedTitle.Count() + stemmedSearch.Count())/(match.Count()*2);
                score += match.Count() * value;
            }           
        }

        //incremento de score por palabras
        //agregar un cuarto de punto por cada incremento
        foreach (var item in criteria.SearchAndScoreWords)
        {
            string word = item.Key;
            if (document.Content.FlatString().Contains(word.FlatString()))
            {
                score += (double)1/4 * (double)item.Value;
            }
        }

        //incremento por cercania de palabras
        foreach (var relWords in criteria.RelatedWords)
        {
            int[] w1Locations = document.Content.GetAllIndexOf(relWords.Item1);
            int[] w2Locations = document.Content.GetAllIndexOf(relWords.Item2);
            if ( w1Locations.Count() > 0 && w2Locations.Count() > 0 )
            {
                int minimumDistance = ArrayUtilities.FindSmallestDifference(w1Locations,w2Locations);
                score += (double)10 / (double)minimumDistance;
            }
        }   

        return score;
    }

}