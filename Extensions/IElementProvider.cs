namespace System.Configuration
{
    public interface IElementProvider
    {
        string GetElementName();
        object GetElementKey();
    }
}