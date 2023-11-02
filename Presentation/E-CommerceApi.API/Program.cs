using E_CommerApi.SignalR;
using E_CommerceApi.API.Configurations.ColumnWriters;
using E_CommerceApi.API.Extensions;
using E_CommerceApi.Application;
using E_CommerceApi.Application.Validators.Products;
using E_CommerceApi.Infrastructure;
using E_CommerceApi.Infrastructure.enums;
using E_CommerceApi.Infrastructure.Filters;
using E_CommerceApi.Infrastructure.Services.Storage.Azure;
using E_CommerceApi.Infrastructure.Services.Storage.Local;
using E_CommerceApi.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Cors
builder.Services.AddCors(opt=>opt.AddDefaultPolicy(
    policy=>policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials()
    ));


//log mekan�zmas�
Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/logs.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"),"logs",
    needAutoCreateTable:true,
    columnOptions:new Dictionary<string, ColumnWriterBase>
    {
        {"message",new RenderedMessageColumnWriter() },
        {"message_template",new MessageTemplateColumnWriter() },
        {"level",new LevelColumnWriter() },
        {"time_stamp",new TimestampColumnWriter() },
        {"exception",new ExceptionColumnWriter() },
        {"log_event",new LogEventSerializedColumnWriter() },
        {"user_name",new UserNameColumnWriter() }
    }
    )
    //g�rselle�tirmek i�in seq yap�s�nada yazd�rm�� olduk loglar�
    //.WriteTo.Seq("asdsda")
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch--ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit= 4096;
});
//log mekan�zmas�




//jwtTok-authonication
builder.Services.AddHttpContextAccessor();//client'tan gele request neticesinde olu�turulan httpcontext nesnesine katmanlardak 
//classlar �zerinden(business logic) �zerinden eri�ebilmemizi sa�layan bir servistir.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, //olu�turulacak token de�erini kimlerin/sitelerin kullanaca��n� belirler
            ValidateIssuer = true, //olu�turulacak token de�erini kimin da��tt���n� ifade edece�imiz aland�r
            ValidateLifetime = true, //Token s�resini kontrol eder do�rulama yeri
            ValidateIssuerSigningKey = true, //�retilecek token de�erinin uygulamam�za ait bir de�er oldu�unu ifade eden seciry key verisisinin do�rulamas�d�r.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            //jwt �mr�n� belirliyor 15 saniyelik token'� 15saniye sonra etkisiz yap�yor ve eri�imi engelliyor-jwtRefreshToken yap�land�rmas� i�in
            LifetimeValidator = (notBefore,  expires,  securityToken,  validationParameters) => expires != null?expires>DateTime.UtcNow:false,

            //Log mekan�zmas� i�in
            NameClaimType = ClaimTypes.Name   //JWT �zerinde Name claime kar��l�k gelen de�eri
            //User.Identity.Name propertysinden elde edebiliriz.

        }; 
    });
//jwtTok-authentication

//serviceRegistration
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureService();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();
//storaceServiceRegistration
builder.Services.AddStorage<AzureStorage>();
//enum kullan�m�
//builder.Services.AddStorage(StorageType.Local);


//FluentValidaton Tan�mlanacak Yer
builder.Services.AddControllers(options=>options.Filters.Add<ValidationFiter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
//FluentValidaton Tan�mlanacak Yer



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//loglama i�in global exception handler middleware extension'�
app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

//wwwroot kullanabilmek i�in eklenmeli
app.UseStaticFiles();

//loglanmas�n� istemedi�iniz middlewarelerin alt�na konur sadece alt taraftakiler loglarn�r
app.UseSerilogRequestLogging();

//log mekanizmas� uygulamadki loglar� tutan k�s�m
app.UseHttpLogging();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", username);
    await next();
});

app.MapControllers();

//hub-signalR extension
app.MapHubs();


app.Run();
