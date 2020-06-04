using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Repo;
using BookStore.Services.Handlers;
using BookStore.Services.Models;
using BookStore.Services.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
using Newtonsoft.Json;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BookStore.Services
{
    public class Startup
    {


        ////private IWebHostEnvironment_environment;
        //private IHostingEnvironment _env;

        //public Startup(IHostingEnvironment env, IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //    var _env = env;

        //    Configuration = new ConfigurationBuilder()
        //    .SetBasePath(env.ContentRootPath)
        //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
        //    //.AddJsonFile("appsettings.json")
        //    .Build();
        //}

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddControllers();



            services.AddMvc(option => option.EnableEndpointRouting = false)
             .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
             .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);


            //Basic Authentication
            //services.AddAuthentication("BasicAuthentication")
            //.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddDbContext<BookStoreContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BookStoresDB")));

            var jwtSection = Configuration.GetSection("JWTSettings");
            services.Configure<JWTSettings>(jwtSection);


            //to validate the token which has been sent by clients
            var appSettings = jwtSection.Get<JWTSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            services.AddTransient<IAuthRepo, AuthRepo>();
            services.AddTransient<IBookRepo, BookRepo>(); //same for all request in a particular session
            //Scoped -- each request changes the object
            //Singleton - same object throught session for each req
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
