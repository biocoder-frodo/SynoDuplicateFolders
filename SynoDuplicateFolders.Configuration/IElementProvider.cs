namespace SynoDuplicateFolders.Configuration
{
    public interface IElementProvider
    {
        string GetElementName();
        object GetElementKey();
    }
}
