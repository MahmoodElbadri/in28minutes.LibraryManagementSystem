using Library.Data;

namespace in28minutes.Library.Services;

public interface ISeedDataService
{
    void Initialize(ApplicationDbContext context);
}
