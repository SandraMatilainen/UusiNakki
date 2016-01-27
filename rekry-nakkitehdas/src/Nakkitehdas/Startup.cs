using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Nakkitehdas.DataProviders;

namespace Nakkitehdas
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddSingleton(
                _ => CloudStorageAccount.Parse(Configuration["Data:StorageConnection:ConnectionString"]));

            services.AddMvc();

            services.AddTransient<IAzureData, AzureData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

         //   Seed(Configuration["Data:StorageConnection:ConnectionString"], "juuri");
        }

        //It is seeded not needed anymore
        //private static void Seed(string blobConnectionString, string rootContainer)
        //{
        //    var account = CloudStorageAccount.Parse(blobConnectionString);
        //    var client = account.CreateCloudBlobClient();
        //    var juuri = client.GetContainerReference(rootContainer);

        //    juuri.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

        //    var nakkiresepti = juuri.GetBlockBlobReference("Hakemisto/Alihakemisto/Nakkiresepti.txt");
        //    var missio = juuri.GetBlockBlobReference("Hakemisto/Alihakemisto/missio.txt");
        //    var lihamyllynKäyttöohje = juuri.GetBlockBlobReference("Hakemisto/Lihamyllyn käyttöohje.txt");
        //    var tiedosto = juuri.GetBlockBlobReference("Tiedosto.txt");

        //    nakkiresepti.UploadText("Salainen nakkiresepti. Nakkitehdas Oy:n omaisuutta!");
        //    missio.UploadText("Yrityksen missio: Teemme parhaat nakit juhlaan ja arkeen");
        //    lihamyllynKäyttöohje.UploadText("Näin käytetään meidän lihamyllyä: \r\n - Avaa kansi \r\n - Kaada ylimääräiset naudan jänteet ja suolet astiaan \r\n - Sulje kansi \r\n - Pyöritä 10 minuuttia täydellä teholla");
        //    tiedosto.UploadText("Tämä on testi");
        //}

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
