namespace Helper
{
    public interface IResources
    {
        ResurceResponse Save(string saveDestination, string saveSubject);
        ResurceResponse Load(string loadDestination);
    }
}