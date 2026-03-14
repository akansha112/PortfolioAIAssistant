using PortfolioAI.Data;
using PortfolioAI.Repositories;
using PortfolioAI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ ADD CORS HERE
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddHttpClient<GeminiService>();
builder.Services.AddScoped<IVectorRepository, PineconeRepository>();
builder.Services.AddScoped<IAIService, GeminiService>();
builder.Services.AddSingleton(new ChatHistoryRepository(
    "mongodb+srv://saxenaakansha014_db_user:Ee9fzrZVhUr23IoF@portfolioai.awha7rv.mongodb.net/PortfolioAI?retryWrites=true&w=majority"
));
builder.Services.AddScoped<RagService>();
builder.Services.AddSingleton<ResumeDataSeeder>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ResumeDataSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ ADD CORS MIDDLEWARE HERE
app.UseCors("AllowReact");

app.UseAuthorization();

app.MapControllers();

app.Run();