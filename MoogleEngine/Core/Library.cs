using System.Text.RegularExpressions;
using Annytab.Stemmer;

namespace MoogleEngine.Core;

public class Library
{
    public Library(string location)
    {
        stemmer = new SpanishStemmer();        
        Documents = this.Load(location);
        Vocabulary = new List<string>();
        TF_IDF_Weigth = new List<IList<Double>>();
        ComputeVocabularyWeight();
    }

    private Stemmer stemmer;

    public IList<Document> Documents { get; private set;}

    private IList<string> Vocabulary;
    
    private IList<IList<double>> TF_IDF_Weigth;

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
            documents.Add(new Document(fileName, content, stemmer));
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
                    double DocumentsWithTerm = Documents.Where(x => x.StemmedVocabulary.Contains(term)).Count();
                    double IDF = Math.Log((double)Documents.Count / ((double)1 + (double)DocumentsWithTerm));

                    double TF = doc.StemmedVocabulary.Where(d => d == term).Count();
                    double TFIDF = TF * IDF;
                    vector.Add(TFIDF);
                }
                TF_IDF_Weigth.Add(vector);
            }

        }

    }

}