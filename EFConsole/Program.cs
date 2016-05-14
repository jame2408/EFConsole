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
                #region LINQ to Entities 寫法
                //var one = db.Course.Include("Department").FirstOrDefault(s => s.CourseID == 1);                
                //Console.WriteLine(one?.Title + ":" + one?.Department.Name);
                #endregion

                #region 雙重迴圈寫法
                //foreach (var item in db.Department)
                //{
                //    Console.WriteLine(item?.Name + "：");
                //    foreach (var data in item.Course)
                //    {
                //        Console.WriteLine("    " + data?.Title);
                //    }
                //}
                #endregion

                #region Native SQL 寫法
                var sql = @"SELECT 
                                Department.Name as DeptName, 
                                COUNT(*) AS CourseCount
                            FROM Course 
                            INNER JOIN Department 
                                ON Course.DepartmentID = Department.DepartmentID
                            GROUP BY Department.Name";
                var data = db.Database.SqlQuery<DeptCourseCount>(sql);
                foreach (var item in data)
                {
                    Console.WriteLine(item?.DeptName + ":" + item?.CourseCount);
                }
                #endregion
            }
        }

        class DeptCourseCount
        {
            public string DeptName { get; set; }
            public int CourseCount { get; set; }
        }

        static void EF基本練習(string[] args)
        {
            using (var db = new ContosoUniversityEntities())
            {
                //foreach (var item in db.Course)
                //{
                //    Console.WriteLine(item.Title);
                //}

                //新增
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

                //刪除
                db.Course.Remove(db.Course.Find(8));
                foreach (var item in db.Course)
                {
                    item.Credits += 1;
                }
                db.SaveChanges();

                //查詢
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
