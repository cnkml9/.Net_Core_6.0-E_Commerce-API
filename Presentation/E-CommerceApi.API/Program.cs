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


//log mekanýzmasý
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
    //görselleþtirmek için seq yapýsýnada yazdýrmýþ olduk loglarý
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
//log mekanýzmasý




//jwtTok-authonication
builder.Services.AddHttpContextAccessor();//client'tan gele request neticesinde oluþturulan httpcontext nesnesine katmanlardak 
//classlar üzerinden(business logic) üzerinden eriþebilmemizi saðlayan bir servistir.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, //oluþturulacak token deðerini kimlerin/sitelerin kullanacaðýný belirler
            ValidateIssuer = true, //oluþturulacak token deðerini kimin daðýttýðýný ifade edeceðimiz alandýr
            ValidateLifetime = true, //Token süresini kontrol eder doðrulama yeri
            ValidateIssuerSigningKey = true, //üretilecek token deðerinin uygulamamýza ait bir deðer olduðunu ifade eden seciry key verisisinin doðrulamasýdýr.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            //jwt ömrünü belirliyor 15 saniyelik token'ý 15saniye sonra etkisiz yapýyor ve eriþimi engelliyor-jwtRefreshToken yapýlandýrmasý için
            LifetimeValidator = (notBefore,  expires,  securityToken,  validationParameters) => expires != null?expires>DateTime.UtcNow:false,

            //Log mekanýzmasý için
            NameClaimType = ClaimTypes.Name   //JWT üzerinde Name claime karþýlýk gelen deðeri
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
//enum kullanýmý
//builder.Services.AddStorage(StorageType.Local);


//FluentValidaton Tanýmlanacak Yer
builder.Services.AddControllers(options=>options.Filters.Add<ValidationFiter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
//FluentValidaton Tanýmlanacak Yer



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


//loglama için global exception handler middleware extension'ý
app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

//wwwroot kullanabilmek için eklenmeli
app.UseStaticFiles();

//loglanmasýný istemediðiniz middlewarelerin altýna konur sadece alt taraftakiler loglarnýr
app.UseSerilogRequestLogging();

//log mekanizmasý uygulamadki loglarý tutan kýsým
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
