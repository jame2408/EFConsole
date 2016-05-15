﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("APP 1");
            using (var db = new ContosoUniversityEntities())
            {
                db.Database.Log = Console.WriteLine;

                var c = db.Department.Find(1);
                c.Name += "!!";

                Console.ReadLine();

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw;
                }
            }
        }
    }
}
