using LabeledGallery.Services;
using LabeledGallery.Utils;

namespace LabeledGallery;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var settings = new Settings();
        Configuration.Bind(settings);
        
        SetupServices(services, settings);
    }

    public static void SetupServices(IServiceCollection services, Settings settings)
    {
        services.AddSingleton(new DocumentStoreHolder(settings.Database));
        
        services.AddScoped<IUserService, UserService>();
        
        services.AddControllers()
            .AddControllersAsServices()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.UseRouting();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        if (env.IsProduction())
        {
            app.UseHttpsRedirection();
        }
    }
}