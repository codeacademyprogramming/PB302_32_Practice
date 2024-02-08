using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Homework
{
    internal class Data
    {
        public Data()
        {
            Seed();
        }
        private const string dataPath = "C:\\Users\\elchin\\Desktop\\CodeLessons\\32. 08-02-2024\\08022024\\Homework\\DataFiles\\";
        public List<TodoItem> TodoItems => GetTodoItems();

        private List<TodoItem> GetTodoItems()
        {
            using (FileStream fs = new FileStream(dataPath + "todoitems.json", FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                string jsonStr = sr.ReadToEnd();
                sr.Close();

                var data = JsonSerializer.Deserialize<List<TodoItem>>(jsonStr,new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                return data;
            }
        }

        private void Seed()
        {
            if (!File.Exists(dataPath + "todoitems.json"))
            {
                List<TodoItem> items = new List<TodoItem>()
            {
                new TodoItem{Id=1,Title="Title 1",Completed=false},
                new TodoItem{Id=2,Title="Title 2",Completed=false},
                new TodoItem{Id=3,Title="Title 3",Completed=true},
                new TodoItem{Id=4,Title="Title 4",Completed=false},
            };
                SaveTodoItems(items);
            }

        }

        public void SaveTodoItems(List<TodoItem> items)
        {
            var jsonStr = JsonSerializer.Serialize(items,new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            using (FileStream fs = new FileStream(dataPath + "todoitems.json", FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(jsonStr);
                sw.Close();
            }
        }
    }
}
