using System.Threading.Tasks;

namespace NavigationApi.Api.Domain
{
    public interface IMapRepository
    {
        Task<Map> GetById(string id);
        Task Create(Map map);
    }
}