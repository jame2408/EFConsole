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
                //foreach (var item in db.Course)
                //{
                //    Console.WriteLine(item.Title);
                //}

                //var c = new Course()
                //{
                //    Title = "Git Test",
                //    Credits = 4
                //};
                //c.Department = db.Department.Find(2);
                //db.Course.Add(c);

                //Console.WriteLine(c.CourseID);
                //db.SaveChanges();
                //Console.WriteLine(c.CourseID);
                //Console.ReadLine();

                db.Course.Remove(db.Course.Find(8));
                foreach (var item in db.Course)
                {
                    item.Credits += 1;
                }
                db.SaveChanges();

                var data = from p in db.Course
                           where p.Title != Guid.NewGuid().ToString()
                           select p;
                foreach (var item in data)
                {
                    Console.WriteLine(item.Title);
                }
            }
        }
    }
}
