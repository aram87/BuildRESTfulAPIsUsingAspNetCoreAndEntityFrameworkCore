namespace MyDictionaryApp.Models;
public class OnlineDictionarySettings : EndpointOptions
{
    public string ApiBaseUrl { get; set; }
    public string AppId { get; set; }
    public string AppKey { get; set; }
}
