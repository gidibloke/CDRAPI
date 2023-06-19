using Domain.LookupModels;

namespace CDRAPI.Data
{
    public class Seed
    {
        public async static Task SeedData(ApplicationDbContext context)
        {
            if(!context.Currencies.Any())
            {
                var data = new List<Currency>
                {
                    new Currency
                    {
                        Id= 1,
                        Description = "GBP"
                    }
                };
                context.AddRange(data);
            }
            await context.SaveChangesAsync();
        }
    }
}
