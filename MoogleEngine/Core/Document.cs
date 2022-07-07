namespace MoogleEngine.Core;

public class Document
{
        public Document(string title, string literalContent)
    {
        this.Title = title;
        this.Content = literalContent;
        this.Vocabulary = new Dictionary<string, int[]>();
        ComputeVocabulary();
    }

    public string Title { get; private set; }
    public string Content { get; private set; }
    private Dictionary<string,int[]> Vocabulary;

    private void ComputeVocabulary()
    {
        this.Vocabulary.Clear();
        //palabras distintas de longitud mayor que 1.
        string[] words = this.Content.ToLower().Split().Distinct().Where(x => x.Length > 1).ToArray();
        foreach (string word in words)
        {
            //ubicaciones de cada palabra en el documento
            int[] locations = this.Content.AllIndexOf(word, StringComparison.OrdinalIgnoreCase).ToArray();
            this.Vocabulary.Add(word, locations);
        } 
        //acciones para calcular importancia de cada palabra
    }

    public bool ContainsAnyWordIn(string phrase)
    {
        string[] words = phrase.Split();
        foreach (string word in words)
        {
            if(this.Content.FlatString().IndexOf(word.FlatString(), StringComparison.OrdinalIgnoreCase) > 0)
            {
                return true;
            }
        }
        return false;
    }

}