using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Dz_13_16_ServerSave.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RandomPeopleController : Controller
    {
        private static List<string> SavedNames = new List<string>(); 
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Person(string name, int age)
            {
                Name = name;
                Age = age;
            }
        }

        private static readonly string[] Names = new[]
        {
            "Anton", "Andriy", "Victor", "Oleg", "Ira", "Kate", "Vira", "Kolia"
        };

        public RandomPeopleController()
        {
            
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return SavedNames;
        }
        [HttpGet("add")]
        public IEnumerable<string> AddPerson(string name, int age)
        {
            if (name != null)
            {
                    string localName = name.Trim();
                    if (!(age < 14 || age > 79))
                    { 
                    SavedNames.Add(localName); 
                }
            }
            return SavedNames;
        }

        [HttpGet("random")]
        public IEnumerable<string> AddRandomPerson()
        {
            List<Person> People = new List<Person>();
            string path = @$"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent}\data.txt";

            using (StreamWriter sw = new StreamWriter(path))
            {
                Random r = new Random();
                for (int i = 0; i < 100; i++)
                {
                    sw.WriteLine(Names[r.Next(Names.Length)] + r.Next(10) + " " + r.Next(100));
                }
            }
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] splited;
                    splited = line.Split(' ');
                    if (splited != null && splited.Length == 2)
                    {
                        string localName = splited[0];
                        int localAge = Int32.Parse(splited[1]);
                        People.Add(new Person(localName, localAge));
                    }
                }
            }
            var notBetwenNames = People.Where(x => !(x.Age < 14 || x.Age > 79)).GroupBy(x => x.Name).Select(x => x.Key).ToList();

            foreach (var item in notBetwenNames)
            {
                SavedNames = SavedNames.Union(notBetwenNames).ToList();
            }
            return SavedNames;
        }

    }
}
