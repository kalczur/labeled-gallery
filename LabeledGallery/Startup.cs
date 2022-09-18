using LabeledGallery.Models.User;
using LabeledGallery.Services;
using LabeledGallery.Utils;
using LabeledGallery.Utils.Auth;
using Microsoft.AspNetCore.Identity;
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

        services.AddIdentity<AccountLogin, string>(options => { options.User.RequireUniqueEmail = true; })
            .AddSignInManager<SignInManager>()
            .AddTokenProvider<AuthenticatorTokenProvider<AccountLogin>>(TokenOptions.DefaultAuthenticatorProvider)
            .AddUserStore<UsersStore>()
            .AddRoleStore<RolesStore>();

        services.AddScoped(serviceProvider =>
        {
            return serviceProvider.GetService<DocumentStoreHolder>().OpenAsyncSession();
        });

        services.AddScoped<IAuthUserRepository, AuthUserRepository>();

        services.AddScoped<ISignInManager>(provider => provider.GetService<SignInManager>());

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
