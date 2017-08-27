using AutoMapper;
using FluentValidation.AspNetCore;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Xero.AspNet.Core;
using Xero.Refactor.Data;
using Xero.Refactor.Services;
using Xero.Refactor.WebApi.ErrorHandling;

namespace Xero.Refactor.WebApi.Core
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetCurrentUser()
        {
            return "XeroAPIUser";
        }
    }

    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);


            Configuration = builder.Build();

            environment = env;
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                        c => { c.Filters.Add(typeof(CustomExceptionFilter)); }
                    )
                 .AddFluentValidation(fv =>
                 {
                     fv.ValidatorFactoryType = typeof(AttributedValidatorFactory);
                 });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperDtoProfile());
                cfg.AddProfile(new AutoMapperApiModelProfile());

            });
            services.AddSingleton<IMapper>(sp => config.CreateMapper());

            services.AddOptions();


            //I:\GitRepo\Xero\Refaction\ASP.Net Core\Xero.Refactor.WebApi.Core\Data\Database.mdf
            //Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="I:\GitRepo\Xero\Refaction\ASP.Net Core\Xero.Refactor.WebApi.Core\Data\Database.mdf";Integrated Security=True;Connect Timeout=30
            var cnstr = $@"Data Source = (localdb)\mssqllocaldb; AttachDbFilename =""{ environment.ContentRootPath }\Data\Database.mdf""; Trusted_Connection = True";
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddDbContext<RefactorDb>(options => options.UseSqlServer(cnstr));
            services.AddTransient<IProductServices, ProductServices>();
            services.AddTransient<IProductOptionServices, ProductOptionServices>();


            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Xero API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Xero API V1");
            });

            app.UseMvc(routes => routes.MapRoute("DefaultApi", "api/{controller}/{action}/{id?}"));
        }
    }
}
