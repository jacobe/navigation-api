namespace NavigationApi.Api.Domain
{
    public interface IPathAlgorithm
    {
        Path Find(Map map, string startId, string endId);
    }
}