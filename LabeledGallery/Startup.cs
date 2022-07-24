using LabeledGallery.Services;
using LabeledGallery.Utils;
using Newtonsoft.Json.Converters;

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

        services.AddCors(c =>
        {
            c.AddPolicy("AllowOrigin", options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            });
        });

        SetupServices(services, settings);
    }

    public static void SetupServices(IServiceCollection services, Settings settings)
    {
        services.AddSingleton(new DocumentStoreHolder(settings.Database));

        services.AddScoped<IUserService, UserService>();

        services.AddControllers()
            .AddControllersAsServices()
            .AddNewtonsoftJson(options => { options.SerializerSettings.Converters.Add(new StringEnumConverter()); });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.UseRouting();
        app.UseCors(options =>
        {
            options.AllowAnyOrigin();
            options.AllowAnyHeader();
            options.AllowAnyMethod();
        });

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        if (env.IsProduction()) app.UseHttpsRedirection();
    }
}
