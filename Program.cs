using Microsoft.EntityFrameworkCore;
using spotifyApi.Repositories;
using SpotifyApi.DbContext;
using SpotifyApi.Repository;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 5001;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "spotify_api", Version = "v1" });
});
string connectionString = builder.Configuration.GetConnectionString("Spotify") ?? "Data Source=spotify.db";
builder.Services.AddDbContext<SpotifyContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<ISpotifyTokenRepository, SpotifyTokenRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFavoriteAlbumRepository, FavoriteAlbumsRepository>();


builder.Services.AddHttpClient();
builder.Services.AddCors();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
