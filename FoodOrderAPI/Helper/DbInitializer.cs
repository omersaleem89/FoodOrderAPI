using FoodOrderAPI.Models;
using System.Linq;


namespace FoodOrderAPI.Helper
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDBContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.User.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User{ Email="admin@xyz.com",Password="admin",Role="Admin"},
                new User{ Email="adnan@xyz.com",Password="123",Role="Customer"},
            };
            foreach (User u in users)
            {
                context.User.Add(u);
            }
            context.SaveChanges();
        }
    }
}
