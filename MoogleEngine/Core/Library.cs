using Annytab.Stemmer;

namespace MoogleEngine.Core;

public class Library
{
    public Library(string location)
    {
        LibraryStemmer = new SpanishStemmer();        
        Documents = this.Load(location);
        Vocabulary = new List<string>();
        TF_IDF_Weigth = new Dictionary<String, IList<double>>();
        ComputeVocabularyWeight();
    }

    public Stemmer LibraryStemmer { get; private set; }

    public IList<Document> Documents { get; private set;}

    public double GetTF_IDF_WEIGHT(Document document, SearchCriteria criteria)
    {
        double relevance = 0;
        if(TF_IDF_Weigth.ContainsKey(document.Title))
        {
            string[] valuedStemms = document.StemmedVocabulary.ToArray().Intersect(LibraryStemmer.GetSteamWords(criteria.Words.ToArray())).ToArray();
            foreach (var stem in valuedStemms)
            {
                int stemIndex = Vocabulary.IndexOf(stem);
                relevance += TF_IDF_Weigth[document.Title].ToArray()[stemIndex];
            }
        }
        return relevance;
    }

    public string[] GetRelevantWords(string[] words)
    {
        return LibraryStemmer.GetSteamWords(words).Where(x => !StopWords.SpanishStopWordsList.Contains(x)).ToArray();
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
            string[] valuedStemms = Vocabulary.ToArray().Intersect(LibraryStemmer.GetSteamWords(words)).ToArray();
            foreach (var stem in valuedStemms)
            {
                int stemIndex = Vocabulary.IndexOf(stem);
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

    private IList<string> Vocabulary;
    
    private Dictionary<String, IList<double>> TF_IDF_Weigth;

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

    private void ComputeVocabularyWeight()
    {
        if (Documents.Count > 0)
        {
            Vocabulary = Documents.SelectMany(d => d.StemmedVocabulary.Select(v => v)).Distinct().ToList().OrderBy(e => e).ToList();
            foreach (var doc in Documents)
            {
                List<double> vector = new List<double>();
                foreach (var term in Vocabulary)
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