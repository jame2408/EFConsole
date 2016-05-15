using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCOGen
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ContosoUniversityEntities())
            {
                var data = db.GetDept();
                foreach (var item in data)
                {
                    Console.WriteLine(item.DeptName);
                }
            }
        }
    }
}
