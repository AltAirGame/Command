namespace GameSystems.Core
{
    public interface IResourcesService
    {
        ResourceResponse Save(string saveDestination, string saveSubject);
        ResourceResponse Load(string loadDestination);
    }
}