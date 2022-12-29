using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PBL6_ECOMMERCE.Website_Ecommerce.API.services;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using Website_Ecommerce.API.Data;
using Website_Ecommerce.API.Queries;
using Website_Ecommerce.API.Repositories;
using Website_Ecommerce.API.services;

namespace Website_Ecommerce.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
            services.AddSignalR();
            services.Configure<Audience>(Configuration.GetSection("Audience"));
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PBL6.WebAPI", Version = "v1" });
                //add frame to get token bearer
                c.AddSecurityDefinition("Bearer",
                   new OpenApiSecurityScheme
                   {
                       In = ParameterLocation.Header,
                       Description = "Please enter into field the word 'Bearer' following by space and JWT",
                       Name = "Authorization",
                       Type = SecuritySchemeType.ApiKey,
                       Scheme = "MyAuthKey"
                   });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 30));
            services.AddDbContext<DataContext>(
                dbContextOptions =>
                {
                    dbContextOptions
                    .UseMySql(Configuration["ConnectionString"], serverVersion)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
                }, ServiceLifetime.Scoped);

            // services.AddDbContext<DataContext>(options =>
            // {
            //     options.UseSqlServer(Configuration["ConnectionString"]);
            // }, ServiceLifetime.Scoped);
            //set up dependency entity
            //set up DI Repository
            services.AddScoped<IIdentityServices, IdentityServices>(); //d
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICategroyRepository, CategroyRepository>();
            services.AddScoped<IUserRepository, UserRepository>(); //d
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<IVoucherOrderRepository, VoucherOrderRepository>();
            services.AddTransient<IShopRepository, ShopRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddTransient<IStatisticService, StatisticService>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IVoucherProductRepository, VoucherProductRepository>();
            services.AddTransient<IServices, Service>();

            services.AddTransient<IAppQueries>(x => new AppQueries(Configuration["ConnectionString"]));

            //using Auto Mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // //set up mediator
            // services.AddMediatR(Assembly.GetExecutingAssembly());
            // // Assembly để .net vào bin và get file dll của những command DI với nhau không cần khai báo tất cả
            // services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(AddCategoryCommand).Assembly);
            // //chỉ lấy những file dll like addcategoryCommand

            //setup validator for fluent validator
            // AssemblyScanner
            //     .FindValidatorsInAssembly(typeof(AddCategoryCommandValidator).Assembly)
            //     .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

            // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));


            var audience = Configuration.GetSection("Audience").Get<Audience>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audience.Secret));
            var tokenValidationParameters = new TokenValidationParameters //verify token
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audience.Issuer,
                ValidateAudience = true,
                ValidAudience = audience.Name,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            services.AddAuthentication()
                .AddJwtBearer("MyAuthKey", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = tokenValidationParameters;
                });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // using var scope = app.ApplicationServices.CreateAsyncScope();
            // var serviceProvider = scope.ServiceProvider;
            // //khong lam anh huong den app. Update dc thi tot, ko thi thoi
            // try
            // {
            //     var context = serviceProvider.GetRequiredService<DataContext>();
            //     context.Database.Migrate();
            //     Seed.SeedCategory(context);
            //     var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            //     logger.LogError("Migration success!");
            // }
            // catch (Exception ex)
            // {
            //     var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            //     logger.LogError(ex, "Migration failed!");
            // }

            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PBL6.WebAPI v1"));
                /*app.UseSwaggerUI(c=> {
                    c.DisplayRequestDuration();
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PBL4.WebAPI v1");
                });*/
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy"); //applie doamin allow config above cross origin request

            app.UseAuthentication();

            app.UseAuthorization();




            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //.RequireCors(MyAllowSpecificOrigins);
                // endpoints.MapHub<PBL4Hub>("/signalr");
            });
        }
    }
}
