using System.Text.RegularExpressions;
using Annytab.Stemmer;

namespace MoogleEngine.Core;

public class Document
{
    public Document(string title, string literalContent, Stemmer stemmer)
    {
        this.Title = title;
        this.Content = literalContent;
        this.Relevance = 0;
        this.StemmedVocabulary = new List<string>();
        this.StemmedContent = new List<string>();
        PreProcessContent(stemmer);
    }

    public string Title { get; private set; }
    public string Content { get; private set; }
    public double Relevance { get; private set;}
    public IList<string> StemmedVocabulary { get; private set; }
    public IList<string> StemmedContent { get; private set; }
    public void AddRelevance( double value)
    {
        Relevance += value;
    }

    public string GetSnippet(string[] words)
    {   
        string snippet = "fragmento del texto en proceso";
        return snippet;
    }

    private void PreProcessContent(Stemmer stemmer)
    {            
        //poner texto en minúscula y sustituir tildes por vocales correspondientes
        string text = Content.ToLower().FlatString();
        //tokenizar el texto, como direcciones de correo, números, etc y fragmentar
        string[] docParts = text.Tokenize().Split(" @$/#.-:&*+=[]¿?¡!(){},''\">_<;%\\".ToCharArray());
        foreach (string part in docParts)
        {
            // tomar solo caracteres y números
            string strippedText = Regex.Replace(part, "[^a-zA-Z0-9]", "");

            // ignorar palabras comunes
            if (!StopWords.SpanishStopWordsList.Contains(strippedText))
            {
                try
                {                    
                    string stem = stemmer.GetSteamWord(strippedText);
                    if (stem.Length > 0)
                    {
                        if (!StemmedVocabulary.Contains(stem))
                        {
                            StemmedVocabulary.Add(stem);
                        }
                        StemmedContent.Add(stem);
                    }
                }
                catch
                {                        
                }
            }                    
        }
    }             
}