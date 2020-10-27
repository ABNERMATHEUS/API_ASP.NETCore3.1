using API.NETCore3._1.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Text;

namespace API.NETCore3._1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)//Quais servi�os a nossa aplica��o vai utilizar
        {
            services.AddResponseCompression(option => { //adicionando compress�o da para ficar otimizado os dados que ser�o enviado para o client
                option.Providers.Add<GzipCompressionProvider>();
                option.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" }); //tipo da minha compress�o
            });

            services.AddCors();//CORS
            //services.AddResponseCaching(); cachear em tudo
            services.AddControllers();

            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x =>
              {
                  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              }).AddJwtBearer(x =>
              {
                  x.RequireHttpsMetadata = false;
                  x.SaveToken = true;
                  x.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(key),
                      ValidateIssuer = false,
                      ValidateAudience = false

                  };
              });


            //services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database")); //utilizado para banco de dados em mem�ria
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("connectionString")));
            
            //!!NESTA VERS�O N�O PRECISA MAIS COLOCAR, PORQUE O ADDDBCONTEXT J� FAZ ISSO!! services.AddScoped<DataContext, DataContext>();//dependencia somente 1 instancia 
            //services.AddScoped<DataContext, DataContext>();//dependencia N's instancias 
            //services.AddSingleton<DataContext, DataContext>();//dependencia somente 1 instancias por sess�o do usu�rio, e se fechar ela n�o abre outra

            services.AddSwaggerGen(c => //adicionando o Swagger
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop api", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)//quais e como op��o de servi�os vamos utilizar
        {
            if (env.IsDevelopment())//se a aplica��o tiver em ambiente de desenvolvimento
            {
                app.UseDeveloperExceptionPage(); //mostrar mais detalhes do erro 
            }
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection(); //HTTPS
            app.UseSwagger(); //Utilizando o Swagger
            app.UseSwaggerUI(c => //utilizando o Swagger UI(Ferramenta Visual) 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop API V1");
            });
            app.UseRouting(); //ROTEAMENTO

            app.UseCors(x =>
            {
                x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });  

            app.UseAuthentication();//Autentica��o de cada perfil
            app.UseAuthorization();//Autoriza��o de cada perfil

            app.UseEndpoints(endpoints => //MAPEAMENTO DOS ENDPOINTS
            {
                endpoints.MapControllers();
            });
        }
    }
}
