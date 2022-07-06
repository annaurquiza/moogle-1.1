namespace MoogleEngine;

public class Document
{
        public Document(string title, string literalContent)
    {
        this.Title = title;
        this.Content = literalContent;
    }

    public string Title { get; private set; }
    public string Content { get; private set; }
}