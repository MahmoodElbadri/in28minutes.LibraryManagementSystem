
using in28minutes.Library.Helpers;
using in28minutes.Library.Repository;
using in28minutes.Library.Services;
using Library.Data;
using Library.Mappings;
using Microsoft.EntityFrameworkCore;

namespace in28minutes.Library;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        builder.Services.AddAutoMapper(typeof(Mapping));
        builder.Services.AddSingleton<ISeedDataService, SeedDataService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Register the UnitOfWork service

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ApplicationDbContext>();
                var seedService = services.GetRequiredService<ISeedDataService>();

                context.Database.Migrate(); // Ensure the database is up to date
                seedService.Initialize(context); // Seed the data
            }
        }


        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
