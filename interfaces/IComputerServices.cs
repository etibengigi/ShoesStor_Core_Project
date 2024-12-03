using ShoesStor.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoesStor.Interfaces
{
    public interface IShoesService
    {
        List<MyShoes> GetAll(int userId);

        MyShoes GetById(int id);

        int Add(MyShoes newShoes,int userId);

        bool Delete(int id);

        bool Update(int id,MyShoes Shoes,int userId);

    }
}