namespace MyDictionaryApp.Services;
public class DictionaryService(IApiClient apiClient, ILogger<DictionaryService> logger) : IDictionaryService
{

    public async Task<DefinitionsWrapper> Lookup(string term, CancellationToken token)
    {
        if (string.IsNullOrEmpty(term))
        {
            return DefinitionsWrapper.Empty();
        }
        logger.LogInformation($"Term is: {term}");

        var response = await apiClient.GetDictionaryEntry(term, token);

        //logger.LogInformation($"GetDictionaryEntry Response is {JsonSerializer.Serialize(response)}");
        if (response.IsSuccessStatusCode && response.Content is not null)
        {
            var definitions = new List<Definition>();  
            foreach (var result in response.Content.results)
            {
                var tempDefinitions = result.lexicalEntries.ConvertAll(x =>
                {
                    var senses = x.entries.Where(o => o.senses is not null).SelectMany(o => o.senses);
                    var definitionsInSenses = senses.Where(o => o.definitions is not null).SelectMany(o => o.definitions).ToList();
                    var examplesInSenses = senses.Where(o => o.examples is not null).SelectMany(o => o.examples).ToList();
                    var synonymsInSenses = senses.Where(o => o.synonyms is not null).SelectMany(o => o.synonyms).ToList();
                    var definition = new Definition
                    {
                        Word = result.word, 
                        Category = x.lexicalCategory.text,
                        Definitions = string.Join("\n\n", definitionsInSenses),
                        Examples = examplesInSenses.Select(o => o.text).ToImmutableList(),
                        Synonyms = string.Join(", ", synonymsInSenses.Select(o => o.text))
                    };

                    return definition;
                });
                definitions.AddRange(tempDefinitions);
            } 
            

            return new(definitions.ToImmutableList());
        }
        else if (response.Error is not null)
        {
            logger.LogError(response.Error, "An error occurred while doing a lookup in the dictionary.");
            throw response.Error;
        }
        else
        {
            return DefinitionsWrapper.Empty();
        }

    }
}

public record Definition
{
    public string Word { get; set; }
    public string Category { get; set; }
    public string Definitions { get; set; }
    public IImmutableList<string> Examples { get; set; }
    public string Synonyms { get; set; }
}

public record DefinitionsWrapper(IImmutableList<Definition> Definitions)
{
    public static DefinitionsWrapper Empty() => new([]);
}

public interface IDictionaryService
{
    Task<DefinitionsWrapper> Lookup(string term, CancellationToken token = default);
}
