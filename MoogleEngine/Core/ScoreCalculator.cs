using Annytab.Stemmer;

namespace MoogleEngine.Core;

public class ScoreCalculator
{
    public static double GetComputedScore(Document document, SearchCriteria criteria)
    {
        double score = 0;
        Stemmer stemmer = new SpanishStemmer(); 
        
        //puntos por coincidencia en título
        if (document.Title.ToLower().Contains(criteria.LiteralSearch.ToLower()))
        {   
            score += 1;
        }
        else
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

        return score;
    }

}