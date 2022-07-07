namespace MoogleEngine.Core;

public class Library
{
    public static IList<Document> Load(string direction)
    {
        //colección de respuesta
        IList<Document> documents = new List<Document>();
        //Lista de documentos en directorio con extensión txt
        string[] filesInDirection = Directory.GetFiles(direction,"*.txt");
        foreach (var item in filesInDirection)
        {            
            string fileName = Path.GetFileNameWithoutExtension(item);   
            string content = System.IO.File.ReadAllText(item);        
            documents.Add(new Document(fileName, content));
        }
        return documents;
    }  
}