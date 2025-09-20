using BingPulseHack.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.Configure<BingApiSettings>(
    builder.Configuration.GetSection("BingApiSettings"));

var app = builder.Build();

app.UseHttpsRedirection();

// Configure static files and default files
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "Dashboard.html" }
});
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
