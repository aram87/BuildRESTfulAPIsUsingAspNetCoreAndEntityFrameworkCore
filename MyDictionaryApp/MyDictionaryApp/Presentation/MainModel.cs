using MyDictionaryApp.Services;

namespace MyDictionaryApp.Presentation;

public partial record MainModel
{
    private readonly IDictionaryService _dictionaryService;
    private readonly ILogger _logger;

    public MainModel(IDictionaryService dictionaryService, ILogger<MainModel> logger)
    {
        _dictionaryService = dictionaryService;
        _logger = logger;
        Title = "This is my Dictionary App";
    }
    public string? Title { get; }

    public IState<string> SearchTerm => State<string>.Value(this, () => "Arbitrary");

    public IListFeed<Definition> SearchDefinitions => SearchTerm
       .Where(searchTerm =>
       {
           if (searchTerm is { Length: > 1 })
           {
               _logger.LogInformation($"Search term > 1");
               return true;
           }
           return false;
       })
       .SelectAsync(
           async (searchTerm, ct) =>
           {
               var dictionaryResult = await _dictionaryService.Lookup(searchTerm ?? "", ct);

               _logger.LogInformation($"After lookup API call, result is {dictionaryResult.Definitions.Select(o => o.Definitions)}");

               return dictionaryResult.Definitions;
           }).AsListFeed();
}
