using Refit;

namespace MyDictionaryApp.Services.Endpoints;
[Headers("Content-Type: application/json")]
public interface IApiClient
{

    [Get("/api/v2/entries/en/{term}")]
    Task<ApiResponse<RootDictionaryEntry>> GetDictionaryEntry(string term, CancellationToken cancellationToken = default);
}
