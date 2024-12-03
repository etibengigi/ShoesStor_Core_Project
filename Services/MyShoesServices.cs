
using ShoesStor.Models;
using ShoesStor.Interfaces;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

// using Tasks.Interfaces;
// using Tasks.Models;
// using System.Text.Json;

namespace ShoesStor.Services
{

    public class MyShoesService : IShoesService
    {

        private List<MyShoes> Shoes;
        private string fileName = "Shoes.json";

        public MyShoesService()
        {
            this.fileName = Path.Combine("data", "Shoes.json");

            using (var jsonFile = File.OpenText(fileName))
            {
                Shoes = JsonSerializer.Deserialize<List<MyShoes>>(jsonFile.ReadToEnd(),
                 new JsonSerializerOptions
                 {
                     PropertyNameCaseInsensitive = true
                 });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(Shoes));
        }

        public List<MyShoes> GetAll(int userId)

        {
            return Shoes.FindAll(t => t.UserId==userId);
        }

        public MyShoes GetById(int id)
        {
            return Shoes.First(p => p.Id == id);
        }

        public int Add(MyShoes newShoes, int userId)
        {
            newShoes.UserId = userId;
            if (Shoes.Count == 0)
                newShoes.Id = 1;
            else
                newShoes.Id = Shoes.Max(p => p.Id) + 1;

            Shoes.Add(newShoes);
            saveToFile();
            return newShoes.Id;
        }

        public bool Delete(int id)
        {
            var existingShoes = GetById(id);
            if (existingShoes is null)
                return false;
            var index = Shoes.IndexOf(existingShoes);
            if (index == -1)
                return false;

            Shoes.RemoveAt(index);
            saveToFile();
            return true;
        }

        public bool Update(int id, MyShoes newShoes, int userId)
        {
            if (id != newShoes.Id)
                return false;

            var existingShoes = GetById(id);
            if (existingShoes == null)
                return false;

            newShoes.UserId = userId;

            var index = Shoes.IndexOf(existingShoes);
            if (index == -1)
                return false;

            Shoes[index] = newShoes;
            saveToFile();

            return true;
        }

    }
}
