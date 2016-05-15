using System;
using System.Linq;

namespace EFConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var db = new ContosoUniversityEntities())
            {
                #region LINQ to Entities 寫法

                //var one = db.Course.Include("Department").FirstOrDefault(s => s.CourseID == 1);
                //Console.WriteLine(one?.Title + ":" + one?.Department.Name);

                #endregion LINQ to Entities 寫法

                #region 雙重迴圈寫法

                //foreach (var item in db.Department)
                //{
                //    Console.WriteLine(item?.Name + "：");
                //    foreach (var data in item.Course)
                //    {
                //        Console.WriteLine("    " + data?.Title);
                //    }
                //}

                #endregion 雙重迴圈寫法

                #region Native SQL 寫法

                //var sql = @"SELECT
                //                Department.Name as DeptName,
                //                COUNT(*) AS CourseCount
                //            FROM Course
                //            INNER JOIN Department
                //                ON Course.DepartmentID = Department.DepartmentID
                //            GROUP BY Department.Name";
                //var data = db.Database.SqlQuery<DeptCourseCount>(sql);
                //foreach (var item in data)
                //{
                //    Console.WriteLine(item?.DeptName + ":" + item?.CourseCount);
                //}

                #endregion Native SQL 寫法

                #region view 寫法(將Native SQL改放置view中)

                //var data = db.vwDeptCourseCount;
                ////判斷集合是否有值，使用Any()，效能最好
                //if (data.Any())
                //{
                //    foreach (var item in data)
                //    {
                //        Console.WriteLine(item?.DeptName + ":" + item?.CourseCount);
                //    }
                //}

                #endregion view 寫法(將Native SQL改放置view中)

                //var c = new Course()
                //{
                //    Title = "Git Test123",
                //    //Credits = 4
                //};
                //c.Department = db.Department.Find(2);
                //db.Course.Add(c);
                //db.SaveChanges();

                #region AsNoTracking() - 效能較快（唯讀資料使用）

                //var data = db.Course.AsNoTracking();
                //foreach (var item in data)
                //{
                //    Console.WriteLine(item.Title);
                //}

                #endregion AsNoTracking() - 效能較快（唯讀資料使用）

                #region EF狀態

                ////開啟記錄Log
                //db.Database.Log = Console.WriteLine;

                ////利用db.Entry(c).State取得c當下的狀態
                //var c = db.Course.Find(10);
                //Console.WriteLine(c.Title + "\t" + db.Entry(c).State); //狀態：Unchanged

                //c.Credits += 1;
                //Console.WriteLine(c.Title + "\t" + db.Entry(c).State); //狀態：Modified
                //db.SaveChanges();

                //db.Course.Remove(c);
                //Console.WriteLine(c.Title + "\t" + db.Entry(c).State); //狀態：Deleted

                ///* 以下範例為直接修改狀態，EF會依據你給的狀態做變更 */

                ////將c資料刪除
                //db.Entry(c).State = System.Data.Entity.EntityState.Deleted;

                ////使用c的資料直接update c（有做事，但你可能看不出來）
                //db.Entry(c).State = System.Data.Entity.EntityState.Modified;

                ////將c的資料直接Insert回DB（像複製資料的感覺）
                //db.Entry(c).State = System.Data.Entity.EntityState.Added;

                //db.SaveChanges();

                #endregion EF狀態

                #region 自製log

                //db.Database.Log = Console.WriteLine;

                //var c = db.Course.Find(10);
                //c.Title = "MVC6 + C# 6.0 + Asp.net Core";
                //if (db.Entry(c).State == System.Data.Entity.EntityState.Modified)
                //{
                //    var ce = db.Entry(c);
                //    var v1 = ce.CurrentValues.GetValue<string>("Title");
                //    v1 = c.Title;
                //    v1 = ce.Entity.Title;

                //    var v2 = ce.OriginalValues.GetValue<string>("Title");
                //    foreach (var prop in ce.OriginalValues.PropertyNames)
                //    {
                //        //ce.OriginalValues.GetValue<string>("Title");
                //    }
                //    Console.WriteLine("New:" + v1 + "\t\nOrig:" + v2);

                //    /*將ModifiedOn改至SaveChanges的partial中（請參考ContosoUniversityEntities.partial.cs），未來使用到*/
                //    //ce.CurrentValues.SetValues(new
                //    //{
                //    //    ModifiedOn = DateTime.Now
                //    //});
                //    db.SaveChanges();
                //}

                #endregion 自製log

            }

        #region 離線物件變成連線物件
        var c = new Course()
            {
                CourseID = 11,
                Title = "123",
                DepartmentID = 1,
                Credits = 1
            };

            //寫法1：使用Attach
            using (var db = new ContosoUniversityEntities())
            {
                Console.WriteLine(db.Entry(c).State);

                db.Course.Attach(c);
                Console.WriteLine(db.Entry(c).State);

                c.Title = "MVC 6";
                Console.WriteLine(db.Entry(c).State);

                db.SaveChanges();
            }

            //寫法2：直接改狀態（不建議這樣寫，有資安問題）
            //using (var db = new ContosoUniversityEntities())
            //{
            //    c.Title = "MVC Core 1.0";
            //    db.Entry(c).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //}
            #endregion
        }

        private class DeptCourseCount
        {
            public string DeptName { get; set; }
            public int CourseCount { get; set; }
        }

        private static void EF基本練習(string[] args)
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