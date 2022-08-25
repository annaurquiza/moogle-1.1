using Annytab.Stemmer;

namespace MoogleEngine.Core;

public class Library
{
    public Library(string location)
    {
        LibraryStemmer = new SpanishStemmer();        
        Documents = this.Load(location);
        StemmedVocabulary = new List<string>();
        LiteralVocabulary = new List<string>();
        TF_IDF_Weigth = new Dictionary<String, IList<double>>();
        ProcessDocuments();
    }
    
    public Stemmer LibraryStemmer { get; private set; }
    public IList<Document> Documents { get; private set;}
    private IList<string> StemmedVocabulary;
    private IList<string> LiteralVocabulary;  
    private Dictionary<String, IList<double>> TF_IDF_Weigth;

    public double GetTF_IDF_WEIGHT(Document document, SearchCriteria criteria)
    {
        double relevance = 0;
        if(TF_IDF_Weigth.ContainsKey(document.Title))
        {
            string[] valuedStemms = document.StemmedVocabulary.ToArray().Intersect(LibraryStemmer.GetSteamWords(criteria.Words.ToArray())).ToArray();
            foreach (var stem in valuedStemms)
            {
                int stemIndex = StemmedVocabulary.IndexOf(stem);
                relevance += TF_IDF_Weigth[document.Title].ToArray()[stemIndex];
            }
        }
        return relevance;
    }

    public string[] GetRelevantWords(string[] words)
    {
        return LibraryStemmer.GetSteamWords(words).Where(x => !LibraryStemmer.StopWordsList().Contains(x)).ToArray();
    }

    public string GetMostRelevantWord(Document document, string[] words)
    {
        string relevantWord = "";
        try
        {
            var splittedDoc = document.Content.Split();
            if (splittedDoc.Count() > 0)
            {
                relevantWord = splittedDoc[0];
            }            
        }
        catch (System.Exception)
        {
        }

        Double maxValue = 0;
        if(TF_IDF_Weigth.ContainsKey(document.Title))
        {   
            string[] valuedStemms = StemmedVocabulary.ToArray().Intersect(LibraryStemmer.GetSteamWords(words)).ToArray();
            foreach (var stem in valuedStemms)
            {
                int stemIndex = StemmedVocabulary.IndexOf(stem);
                double stemValue = TF_IDF_Weigth[document.Title].ToArray()[stemIndex];
                if (stemValue > maxValue)
                {
                    relevantWord = stem;
                    maxValue = stemValue;
                }
            }
        }
        return relevantWord;
    }
    
    public string GetBestSuggestion(string[] words)
    {
        string bestSuggestion = ""; // "Relevancia del resultado insuficiente. Mostrar sugerencia...";
        Dictionary<string,double> matchWords = new Dictionary<string, double>();
        foreach (var word in words)
        {
            //tomar las palabras del vocabulario con al menos un porciento de similitud (60)
            string[] similarTerm = LiteralVocabulary.Where(x => x.ToLower() != word.ToLower() &&  word.GetCommonLettersPercent(x) >= 65).ToArray();
            foreach (string term in similarTerm)
            {
                matchWords.Add(term, DamerauLevenshtein.Distance(word, term));
            }
        }
        bestSuggestion = String.Join( ", ", matchWords.Where(x => x.Value <= 4).OrderBy(x => x.Value).Select(x => x.Key).ToArray());
        return bestSuggestion;
    }

    private IList<Document> Load(string direction)
    {
        //colección de respuesta
        IList<Document> documents = new List<Document>();
        //Lista de documentos en directorio con extensión txt
        string[] filesInDirection = Directory.GetFiles(direction,"*.txt");
        foreach (var item in filesInDirection)
        {            
            string fileName = Path.GetFileNameWithoutExtension(item);   
            string content = System.IO.File.ReadAllText(item);        
            documents.Add(new Document(fileName, content, LibraryStemmer));
        }
        return documents;
    }  

    private void ProcessDocuments()
    {
        if (Documents.Count > 0)
        {
            LiteralVocabulary = Documents.SelectMany(d => d.LiteralVocabulary.Select(v => v)).Distinct().OrderBy(x => x).ToList();
            StemmedVocabulary = Documents.SelectMany(d => d.StemmedVocabulary.Select(v => v)).Distinct().OrderBy(x => x).ToList();
            foreach (var doc in Documents)
            {
                List<double> vector = new List<double>();
                foreach (var term in StemmedVocabulary)
                {                    
                    double documentsWithTerm = Documents.Where(d => d.StemmedVocabulary.Contains(term)).Count();
                    double idf = Math.Log((double)Documents.Count / ((double)1 + (double)documentsWithTerm));

                    double tf = doc.StemmedVocabulary.Where(d => d == term).Count();
                    double tfidf = tf * idf;
                    vector.Add(tfidf);                    
                }
                TF_IDF_Weigth.Add(doc.Title, vector);
            }
        }
    }
}