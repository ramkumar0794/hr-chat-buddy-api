using Process;
using Process.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IPromptManager, PromptManager>();
builder.Services.AddScoped<ClientManager.PineCone.IPineConeClient, ClientManager.PineCone.PineConeClient>();
builder.Services.AddScoped<ClientManager.OpenAI.IOpenAIManager, ClientManager.OpenAI.OpenAIManager>();
builder.Services.AddTransient<Process.Interface.IProcessPDF, Process.ProcessPDF>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // Makes Swagger the root page
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
