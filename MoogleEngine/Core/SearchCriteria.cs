namespace MoogleEngine.Core;

public class SearchCriteria
{
    public SearchCriteria(string literalSearch) 
    {
        LiteralSearch =literalSearch;
        Words = new List<string>();
        ExcludeWords = new List<string>();
        MustWords =  new List<string>();
        SearchAndScoreWords = new Dictionary<string, int>();
        RelatedWords = new List<(string,string)>();
    }
    public string LiteralSearch {get; private set; }
    public IList<string> Words {get; private set; }
    public IList<string> ExcludeWords { get; private set; }
    public IList<string> MustWords { get; private set; }
    public Dictionary<string, int> SearchAndScoreWords { get; private set; }
    public IList<(string,string)> RelatedWords { get; private set; }

    public void AddWord(string word)
    {
        if (!String.IsNullOrEmpty(word))
        {
            if(!Words.Contains(word))
            {
                Words.Add(word);
            }
        }
    }

    public void AddExcludeWord(string word)
    {
        if (!String.IsNullOrEmpty(word))
        {
            if(!ExcludeWords.Contains(word))
            {
                ExcludeWords.Add(word);
            }
        }
    }

    public void AddMustWord(string word)
    {
        if (!String.IsNullOrEmpty(word))
        {
            if(!MustWords.Contains(word))
            {
                MustWords.Add(word);
            }
            AddWord(word);
        }        
    }

    public void AddScoredWord(string word, int score = 1)
    {
        if(!string.IsNullOrEmpty(word) && score > 0)
        {
            if(!SearchAndScoreWords.ContainsKey(word))
            {
                SearchAndScoreWords.Add(word,0);
            }
            SearchAndScoreWords[word] += score; 
        }
        AddWord(word);               
    }

    public void AddRelatedWords(string word1, string word2)
    {
        if (!string.IsNullOrEmpty(word1) && !string.IsNullOrEmpty(word2))
        {
           var relWords = (word1, word2);

             if (!RelatedWords.Contains(relWords))
             {
                RelatedWords.Add(relWords);
             }
        }
        AddWord(word1);
        AddWord(word2);
    }
}