using Google.Cloud.Vision.V1;
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

        services.AddHsts(options => { options.MaxAge = TimeSpan.FromDays(365); });

        SetupServices(services, settings);

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/";
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(3);
        });

        services.AddIdentity<AccountLogin, string>(options => { options.User.RequireUniqueEmail = true; })
            .AddSignInManager<SignInManager>()
            .AddTokenProvider<AuthenticatorTokenProvider<AccountLogin>>(TokenOptions.DefaultAuthenticatorProvider)
            .AddUserStore<UsersStore>()
            .AddRoleStore<RolesStore>();
    }

    public static void SetupServices(IServiceCollection services, Settings settings)
    {
        services.AddSingleton(new DocumentStoreHolder(settings.Database));

        services.AddHttpClient();

        services.AddSingleton<ImageAnnotatorClient>(serviceProvider =>
        {
            return new ImageAnnotatorClientBuilder
            {
                CredentialsPath = "gcp-cloud-vision-creds.json"
            }.Build();
        });

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGalleryService, GalleryService>();

        services.AddScoped(serviceProvider =>
        {
            return serviceProvider.GetService<DocumentStoreHolder>().OpenAsyncSession();
        });

        services.AddScoped<IAuthUserRepository, AuthUserRepository>();

        services.AddControllers()
            .AddControllersAsServices()
            .AddNewtonsoftJson(options => { options.SerializerSettings.Converters.Add(new StringEnumConverter()); });

        services.AddScoped<ISignInManager>(provider => provider.GetService<SignInManager>());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseHsts();

        app.UseRouting();
        app.UseCors(x => x
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        if (env.IsProduction()) app.UseHttpsRedirection();
    }
}
