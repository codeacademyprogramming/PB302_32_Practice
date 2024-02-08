
using Homework;
using System.Net;
using System.Text.Json;

Data data = new Data();
var todoList = data.TodoItems;

string opt;
do
{
    Console.WriteLine("\n=======menu======");
    Console.WriteLine("1. Add");
    Console.WriteLine("2. Remove");
    Console.WriteLine("3. ShowAll");
    Console.WriteLine("4. Show all external");

    Console.WriteLine("5. Add an external todoItem");
    Console.WriteLine("0. Exit");
    opt = Console.ReadLine();

    switch (opt)
    {
        case "1":
            Console.WriteLine("Title: ");
            string title  = Console.ReadLine();

            Console.WriteLine("is done:");
            string isDoneStr = Console.ReadLine();
            bool isDone = Convert.ToBoolean(isDoneStr);

            TodoItem newTodo = new TodoItem
            {
                Title = title,
                Completed = isDone,
                Id = todoList.Max(x=>x.Id)+1
            };

            todoList.Add(newTodo);
            data.SaveTodoItems(todoList);

            break;
        case "2":
            break;
        case "3":
            Console.WriteLine("\n\tAll todo items");
            foreach (var item in todoList)
            {
                Console.WriteLine(item.Id+"-"+item.Title+"-"+item.Completed);
            }
            break;
        case "4":
            HttpClient client = new HttpClient();
            var externalDataStr = client.GetStringAsync("https://jsonplaceholder.typicode.com/todos").Result;

            List<TodoItem> externalData = JsonSerializer.Deserialize<List<TodoItem>>(externalDataStr, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            Console.WriteLine("\n\tExternal todo items:");
            foreach (var item in externalData)
            {
                Console.WriteLine(item.Id + "-" + item.Title + "-" + item.Completed);
            }
            break;
        case "5":
            Console.WriteLine("Ecternal todo id:");
            int id = Convert.ToInt32(Console.ReadLine());

            if(todoList.Exists(x => x.Id == id))
            {
                Console.WriteLine("todo item is already added by id: "+id);
                break;
            }

            using (client = new HttpClient())
            {
                var response = client.GetAsync("https://jsonplaceholder.typicode.com/todos/" + id).Result;

                if(response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Todo item not found by id:"+id);
                    break;
                }

                var dataStr = response.Content.ReadAsStringAsync().Result;
                TodoItem externalTodo = JsonSerializer.Deserialize<TodoItem>(dataStr, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });


                todoList.Add(externalTodo);
                data.SaveTodoItems(todoList);
            }

            break;
        case "0":
            break;
        default:
            break;
    }
}
while (opt != "0");






