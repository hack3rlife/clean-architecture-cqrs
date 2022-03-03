using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.EndToEndTests.Infrastructure
{
    public interface IEndToEndClient
    {
        Task<ApiResponse<T>> GetApiResponseAsync<T>(string url);
    }
}
