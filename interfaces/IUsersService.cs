using ShoesStor.Models;
using System.Collections.Generic;

namespace ShoesStor.Interfaces;

public interface IUserServices
{
    List<User> GetAll();

    User GetById(int id);

    int Add(User newUser);

    bool Update(int id, User newUser);
    bool Delete(int id);
    int ExistUser(string name, string password);

}