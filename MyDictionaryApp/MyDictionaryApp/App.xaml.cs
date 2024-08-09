using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MyDictionaryApp.Services;
using Uno.Resizetizer;

namespace MyDictionaryApp;
public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    protected Window? MainWindow { get; private set; }
    protected IHost? _Host { get; private set; }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args);


        // Add navigation support for toolkit controls such as TabBar and NavigationView
        builder.UseToolkitNavigation()
            .Configure(host => host
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) =>
                {
                    // Configure log levels for different categories of logging
                    logBuilder
                        .SetMinimumLevel(
                            context.HostingEnvironment.IsDevelopment() ?
                                LogLevel.Information :
                                LogLevel.Warning)

                        // Default filters for core Uno Platform namespaces
                        .CoreLogLevel(LogLevel.Warning);

                    // Uno Platform namespace filter groups
                    // Uncomment individual methods to see more detailed logging
                    //// Generic Xaml events
                    //logBuilder.XamlLogLevel(LogLevel.Debug);
                    //// Layout specific messages
                    //logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
                    //// Storage messages
                    //logBuilder.StorageLogLevel(LogLevel.Debug);
                    //// Binding related messages
                    //logBuilder.XamlBindingLogLevel(LogLevel.Debug);
                    //// Binder memory references tracking
                    //logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
                    //// DevServer and HotReload related
                    //logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
                    //// Debug JS interop
                    //logBuilder.WebAssemblyLogLevel(LogLevel.Debug);

                }, enableUnoLogging: true)
                .UseConfiguration(configure: configBuilder =>
                    configBuilder
                        .EmbeddedSource<App>()
                        .Section<AppConfig>()
                        //.Section<OnlineDictionarySettings>()
                )
                // Enable localization (see appsettings.json for supported languages)
                .UseLocalization()
                .UseHttp((context, services) => services
                            // Register HttpClient
        #if DEBUG
                            // DelegatingHandler will be automatically injected into Refit Client
                            .AddTransient<DelegatingHandler, DebugHttpHandler>()
        #endif
                            .AddSingleton<IDictionaryService, DictionaryService>()
                            .AddRefitClientWithEndpoint<IApiClient, OnlineDictionarySettings>(
                                context,
                                configure: (clientBuilder, options) => clientBuilder
                                    .ConfigureHttpClient(httpClient =>
                                    {
                                        httpClient.BaseAddress = new Uri(options!.ApiBaseUrl!);
                                        httpClient.DefaultRequestHeaders.Add("app_id", options!.AppId);
                                        httpClient.DefaultRequestHeaders.Add("app_key", options!.AppKey);
                                    }))
                            //.AddRefitClient<IApiClient>(context, configure: (builder, options) => builder
                            //.ConfigureHttpClient(HttpClient =>
                            //{
                            //    //builder.Services.Configure<OnlineDictionarySettings>(context.Configuration.GetSection(nameof(OnlineDictionarySettings)));

                            //    //var apiUrl = section.GetValue<string>("ApiUrl");

                            //    //var configSection = context.Configuration.GetSection(nameof(OnlineDictionarySettings));

                            //    //var onlineDictionarySettings = ConfigurationBinder.Get<OnlineDictionarySettings>(configSection);

                            //    //if (onlineDictionarySettings is null || onlineDictionarySettings is default(OnlineDictionarySettings))
                            //    //{
                            //    //    throw new Exception("Invalid dictionary settings");
                            //    //}
                            //    //builder.ConfigureHttpClient(client => {

                            //    //    client.BaseAddress = new Uri(onlineDictionarySettings.ApiBaseUrl);
                            //    //    client.DefaultRequestHeaders.Add("app_id", onlineDictionarySettings.AppId);
                            //    //    client.DefaultRequestHeaders.Add("app_key", onlineDictionarySettings.AppKey);
                            //    //    return;
                            //    //});

                            //    //var config = context.Configuration.GetSection(nameof(OnlineDictionarySettings));

                            //    //string apiUrl = config.GetValue(("ApiUrl");

                            //    //return builder.ConfigureHttpClient(x => x.BaseAddress = new Uri("https://od-api-sandbox.oxforddictionaries.com"));
                            //})
                        )
                    
                .ConfigureServices((context, services) =>
                {
                    // TODO: Register your services
                    //services.AddSingleton<IMyService, MyService>();
                })
                .UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes)
            );
        MainWindow = builder.Window;

#if DEBUG
        MainWindow.EnableHotReload();
#endif
        MainWindow.SetWindowIcon();

        _Host = await builder.NavigateAsync<Shell>();
    }

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap(ViewModel: typeof(ShellModel)),
            new ViewMap<MainPage, MainModel>()
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellModel>(),
                Nested:
                [
                    new ("Main", View: views.FindByViewModel<MainModel>(), IsDefault:true)
                ]
            )
        );
    }
}
