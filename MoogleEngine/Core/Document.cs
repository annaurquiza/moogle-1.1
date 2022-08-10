using System.Text.RegularExpressions;
using Annytab.Stemmer;

namespace MoogleEngine.Core;

public class Document
{
    public Document(string title, string literalContent, Stemmer stemmer)
    {
        this.Title = title;
        this.Content = literalContent;
        this.StemmedVocabulary = new List<string>();
        this.StemmedContent = new List<string>();
        PreProcessContent(stemmer);
    }

    public string Title { get; private set; }
    public string Content { get; private set; }
    public IList<string> StemmedVocabulary { get; private set; }
    public IList<string> StemmedContent { get; private set; }

     private void PreProcessContent(Stemmer stemmer)
    {            
        //poner texto en minúscula y sustituir tildes por vocales correspondientes
        string text = Content.ToLower().FlatString();
        //tokenizar partes del texto, como direcciones de correo, números, etc y fragmentar
        string[] docParts = Tokenize(text);
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

    private static string[] Tokenize(string text)
    {
      // Strip all HTML.
      text = Regex.Replace(text, "<[^<>]+>", "");
      // Strip numbers.
      text = Regex.Replace(text, "[0-9]+", "number");
      // Strip urls.
      text = Regex.Replace(text, @"(http|https)://[^\s]*", "httpaddr");
      // Strip email addresses.
      text = Regex.Replace(text, @"[^\s]+@[^\s]+", "emailaddr");
      // Strip dollar sign.
      text = Regex.Replace(text, "[$]+", "dollar");
      // Strip usernames.
      text = Regex.Replace(text, @"@[^\s]+", "username");      
      // Tokenize and also get rid of any punctuation
      return text.Split(" @$/#.-:&*+=[]?!(){},''\">_<;%\\".ToCharArray());
    }
   
}