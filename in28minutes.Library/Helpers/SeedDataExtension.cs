using in28minutes.Library.Services;
using Library.Data;
using Microsoft.EntityFrameworkCore;

namespace in28minutes.Library.Helpers;

public static class SeedDataExtension
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
            seedDataService.Initialize(db);
        }
    }
}
