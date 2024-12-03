using ShoesStor.Models;
using ShoesStor.Interfaces;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using ShoesStor.Services;


namespace ShoesStor.Services
{

    public class UsersServices : IUserServices
    {

        private List<User> user;
        private string fileName = "Task.json";

        public UsersServices()
        {
            this.fileName = Path.Combine("data", "user.json");

            using (var jsonFile = File.OpenText(fileName))
            {
                user = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                 new JsonSerializerOptions
                 {
                     PropertyNameCaseInsensitive = true
                 });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(user));
        }

        public List<User> GetAll() => user;

        public User GetById(int id)
        {
            return user.FirstOrDefault(p => p.Id == id);
        }

        public int Add(User newUser)
        {
            if (user.Count == 0)
                newUser.Id = 1;
            else
                newUser.Id = user.Max(p => p.Id) + 1;

            user.Add(newUser);
            saveToFile();
            return newUser.Id;
        }

        public bool Delete(int id)
        {
            var existingUser = GetById(id);
            if (existingUser is null)
                return false;
            var index = user.IndexOf(existingUser);
            if (index == -1)
                return false;

            user.RemoveAt(index);
            saveToFile();
            return true;
        }

        public bool Update(int id, User newUser)
        {
            if (id != newUser.Id)
                return false;

            var existingUser = GetById(id);
            if (existingUser == null)
                return false;

            var index = user.IndexOf(existingUser);
            if (index == -1)
                return false;

            user[index] = newUser;
            saveToFile();
            return true;
        }
        public int ExistUser(string name, string password)
        {
            User existUser = user.FirstOrDefault(u => u.Username.Equals(name) && u.Password.Equals(password));
            if (existUser != null)
                return existUser.Id;
            return -1;
        }

    }
}

public static class UserUtils
{
    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUserServices,UsersServices>();

    }
}
