using System.Text.RegularExpressions;
using Annytab.Stemmer;

namespace MoogleEngine.Core;

public class Document
{
    public Document(string title, string literalContent, Stemmer stemmer)
    {
        this.Title = title;
        this.Content = literalContent;
        this.LiteralVocabulary = new List<string>();
        this.StemmedVocabulary = new List<string>();
        this.StemmedContent = new List<string>();
        PreProcessContent(stemmer);
    }
    public string Title { get; private set; }
    public string Content { get; private set; }

    public string FullContentIncludingTitle {
        get 
        {
            return $"{this.Title} {this.Content}";
        }
    }
    public IList<string> StemmedVocabulary { get; private set; }
    public IList<string> LiteralVocabulary { get; private set; }
    public IList<string> StemmedContent { get; private set; }    
    public string GetSnippet(string guideWord, int length = 100)
    {   
        int start = 0;
        int guideWordPosition = this.Content.FlatString().IndexOf(guideWord.FlatString());
        int padding = 30;
        if (guideWordPosition - padding > 0)
        {
            start = guideWordPosition - padding;
        }        
        if (start + length > this.Content.Length)
        {
            length = this.Content.Length - start;
        }
        string snippet = this.Content.Substring(start, length);
        return snippet;
    }
    private void PreProcessContent(Stemmer stemmer)
    {   
        LiteralVocabulary = FullContentIncludingTitle.ToLower().Split(" @$/#.-:&*+=[]¿?¡!(){},''\">_<;%\\".ToCharArray()).Distinct().Where(x => x.Length > 0 && !stemmer.StopWordsList().Contains(x)).OrderBy(x => x).ToList();
        //poner texto en minúscula y sustituir tildes por vocales correspondientes
        string text = FullContentIncludingTitle.FlatString();
        //tokenizar el texto, como direcciones de correo, números, etc y fragmentar
        string[] docParts = text.Tokenize().Split(" @$/#.-:&*+=[]¿?¡!(){},''\">_<;%\\".ToCharArray());
        foreach (string part in docParts)
        {
            // tomar solo caracteres y números
            string strippedText = Regex.Replace(part, "[^a-zA-Z0-9]", "");
            if (string.IsNullOrEmpty(strippedText))
            {
                continue;
            }

            // ignorar palabras comunes
            if (!stemmer.StopWordsList().Contains(strippedText))
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