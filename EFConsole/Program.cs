using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ContosoUniversityEntities())
            {
                foreach (var item in db.Course)
                {
                    Console.WriteLine(item.Title);
                }
            }
        }
    }
}
