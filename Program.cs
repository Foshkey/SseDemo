using SseDemo.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<SseService>();

var app = builder.Build();
app.UseFileServer();
app.MapControllers();
app.Run();
