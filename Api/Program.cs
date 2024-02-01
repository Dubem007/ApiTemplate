using API.Extensions;
using API.Middlewares;
using Application.Helpers;
using Application.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureCors();
builder.Services.ConfigureIisIntegration();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureApiVersioning(builder.Configuration);
builder.Services.ConfigureMvc();
builder.Services.ConfigureHangFire(builder.Configuration);
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRedis(builder.Configuration);
builder.Services.ConfigureAWSServices(builder.Configuration);


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
        
    });
}


app.UseDeveloperExceptionPage();
app.UseHangfireDashboard();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseErrorHandler();
app.MapControllers();

WebHelper.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

HangFireService.RunJobs();
app.Run();
