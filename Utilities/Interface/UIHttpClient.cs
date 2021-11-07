using System.Threading.Tasks;

namespace Utilities.Interface
{
    public interface UIHttpClient
    {
        Task<string> Post(string parameter, string url, string requestId, string header = null, string token = null);

        Task<string> Get(string url, string requestId);
    }
}