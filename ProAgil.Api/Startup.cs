using System;
using System.IO;
using System.Reflection;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using ProAgil.Dominio.Identity;
using ProAgil.Repositorio;
using ProAgil.Repositorio.Interfaces;

namespace ProAgil.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		private string GetStringConnection()
		{
			var pgUserId = Environment.GetEnvironmentVariable("POSTGRES_USER_ID") ?? Configuration.GetConnectionString("pgUserId");
			var pgPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? Configuration.GetConnectionString("pgPassword");
			var pgHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? Configuration.GetConnectionString("pgHost");
			var pgPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? Configuration.GetConnectionString("pgPort");
			var pgDatabase = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? Configuration.GetConnectionString("pgDatabase");
			var pgExtConfig = Environment.GetEnvironmentVariable("POSTGRES_CONFIG_EXTRA") ?? Configuration.GetConnectionString("pgExtConfig");
			var connectionString = $"Server={pgHost};Port={pgPort};User Id={pgUserId};Password={pgPassword};Database={pgDatabase};{pgExtConfig}";
			return connectionString;
		}
		public void ConfigureServices(IServiceCollection services)
		{
			string nameAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

			var connection = GetStringConnection();
			services.AddEntityFrameworkNpgsql().AddDbContext<ProAgilContext>(options =>
			{
				options.UseNpgsql(connection, m => m.MigrationsAssembly(nameAssembly));
			});

			IdentityBuilder builder = services.AddIdentityCore<User>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 4;
			});
			builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
			builder.AddEntityFrameworkStores<ProAgilContext>();
			builder.AddRoleValidator<RoleValidator<Role>>();
			builder.AddRoleManager<RoleManager<Role>>();
			builder.AddSignInManager<SignInManager<User>>();

			#region Token JWT
		
			var tokenConfigurationsSection = Configuration.GetSection("TokenConfigurations");
            services.Configure<TokenConfigurations>(tokenConfigurationsSection);

            var tokenConfigurations = tokenConfigurationsSection.Get<TokenConfigurations>();
            var key = Encoding.ASCII.GetBytes(tokenConfigurations.Secret);
            var tokenValidationParameter =  new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = false,
					ValidIssuer = tokenConfigurations.Emissor,

                    ValidateAudience = false,
                    ValidAudience = tokenConfigurations.ValidoEm
                    
                };
            services.AddSingleton(tokenValidationParameter);
            services.AddAuthentication(it =>
                {
                    it.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    it.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer( it => {
                it.RequireHttpsMetadata = true;
                it.SaveToken = true;
                it.TokenValidationParameters = tokenValidationParameter;
            });
			#endregion
			
			services.AddMvc(
				options =>
				{
					var policy = new AuthorizationPolicyBuilder()
						.RequireAuthenticatedUser()
						.Build();
					options.Filters.Add(new AuthorizeFilter(policy));
				}
			)
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
			.AddJsonOptions(options =>  options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

			services.AddScoped<IRepositorio, Repositorio.Repositorio>();
			services.AddAutoMapper();
			services.AddCors();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				try
				{
					scope.ServiceProvider.GetRequiredService<ProAgilContext>().Database.Migrate();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Erro AutoMigrate: {ex.Message} {ex.InnerException} {ex.StackTrace}");
				}
			}
			using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				scope.ServiceProvider.GetRequiredService<ProAgilContext>().Database.EnsureCreated();

				var optionsBuilder = new DbContextOptionsBuilder<ProAgilContext>();
				optionsBuilder.UseNpgsql(GetStringConnection());
				using (var context = new ProAgilContext(optionsBuilder.Options))
				{
					EnsureSeedData(context);
				}
			}

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseAuthentication();
			app.UseCors(
				x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
			);
			
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseStaticFiles(
				new StaticFileOptions()
				{
					FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
					RequestPath = new PathString("/Resources")
				}
			);
			app.UseMvc();
		}

		private void EnsureSeedData(ProAgilContext context)
		{

		}
	}


}
