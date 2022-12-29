using Website_Ecommerce.API.Data.Entities;

namespace Website_Ecommerce.API.Data.Seed
{
    public static class Seed
    {
        public static void SeedCategory(DataContext dataContext)
        {
            // if (_dataContext.Users.Any()) return;

            // var usersText = System.IO.File.ReadAllText("Data/Seed/users.json");

            // var users = JsonSerializer.Deserialize<List<Category>>(usersText);
            // // var users = JsonConvert.DeserializeObject<List<User>>(usersText);

            // // if(users == null) return;

            // foreach (var user in users)
            // {
            //     using var hmac = new HMACSHA512();
            //     user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123456"));
            //     user.PasswordSalt = hmac.Key;
            //     user.CreatedAt = DateTime.Now;
            //     _dataContext.Users.Add(user);
            // }
            // _dataContext.SaveChanges();
            dataContext.Categories.Add(new Category { Id = 9, Name = "Ban", CreateBy = 1, DateCreate = DateTime.Now });
            dataContext.SaveChanges();
        }
    }
}