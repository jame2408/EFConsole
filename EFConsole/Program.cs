using System;
using System.Diagnostics;
using System.Linq;

namespace EFConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var db = new ContosoUniversityEntities())
            {
                //LINQtoEntities寫法(db);

                //雙重迴圈寫法(db);

                //NativeSQL(db);

                //ExecDBView(db);

                //預設值(db);

                //AsNoTracking(db);

                //EF狀態(db);

                //自製log(db);

                //SP練習_Select(db);

                //SP練習_Insert(db);

                //Enum(db);

                //預先載入與延遲載入(db);

                //輸出Log(db);
            }

            //離線物件變成連線物件();
        }

        private static void LINQtoEntities寫法(ContosoUniversityEntities db)
        {
            #region LINQ to Entities 寫法

            var one = db.Course.Include("Department").FirstOrDefault(s => s.CourseID == 1);
            Console.WriteLine(one?.Title + ":" + one?.Department.Name);

            #endregion LINQ to Entities 寫法
        }

        private static void 雙重迴圈寫法(ContosoUniversityEntities db)
        {
            #region 雙重迴圈寫法

            foreach (var item in db.Department)
            {
                Console.WriteLine(item?.Name + "：");
                foreach (var data in item.Course)
                {
                    Console.WriteLine("    " + data?.Title);
                }
            }

            #endregion 雙重迴圈寫法
        }

        private static void NativeSQL(ContosoUniversityEntities db)
        {
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

            #endregion Native SQL 寫法
        }

        private static void ExecDBView(ContosoUniversityEntities db)
        {
            #region view 寫法(將Native SQL改放置view中)

            var data = db.vwDeptCourseCount;
            //判斷集合是否有值，使用Any()，效能最好
            if (data.Any())
            {
                foreach (var item in data)
                {
                    Console.WriteLine(item?.DeptName + ":" + item?.CourseCount);
                }
            }

            #endregion view 寫法(將Native SQL改放置view中)
        }

        private static void 預設值(ContosoUniversityEntities db)
        {
            #region 設定實體物件的預設值的開發技巧（參考edmx.Init.partial.cs）

            var c = new Course()
            {
                Title = "Git Test123",
                //Credits改用初始值寫法（參考edmx.Init.partial.cs）
                //Credits = 4
            };
            c.Department = db.Department.Find(2);
            db.Course.Add(c);
            db.SaveChanges();

            #endregion 設定實體物件的預設值的開發技巧（參考edmx.Init.partial.cs）
        }

        private static void AsNoTracking(ContosoUniversityEntities db)
        {
            #region AsNoTracking() - 效能較快（唯讀資料使用）

            var data = db.Course.AsNoTracking();
            foreach (var item in data)
            {
                Console.WriteLine(item.Title);
            }

            #endregion AsNoTracking() - 效能較快（唯讀資料使用）
        }

        private static void EF狀態(ContosoUniversityEntities db)
        {
            #region EF狀態

            //開啟記錄Log
            db.Database.Log = Console.WriteLine;

            //利用db.Entry(c).State取得c當下的狀態
            var c = db.Course.Find(10);
            Console.WriteLine(c.Title + "\t" + db.Entry(c).State); //狀態：Unchanged

            c.Credits += 1;
            Console.WriteLine(c.Title + "\t" + db.Entry(c).State); //狀態：Modified
            db.SaveChanges();

            db.Course.Remove(c);
            Console.WriteLine(c.Title + "\t" + db.Entry(c).State); //狀態：Deleted

            /* 以下範例為直接修改狀態，EF會依據你給的狀態做變更 */

            //將c資料刪除
            db.Entry(c).State = System.Data.Entity.EntityState.Deleted;

            //使用c的資料直接update c（有做事，但你可能看不出來）
            db.Entry(c).State = System.Data.Entity.EntityState.Modified;

            //將c的資料直接Insert回DB（像複製資料的感覺）
            db.Entry(c).State = System.Data.Entity.EntityState.Added;

            db.SaveChanges();

            #endregion EF狀態
        }

        private static void 自製log(ContosoUniversityEntities db)
        {
            #region 自製log

            db.Database.Log = Console.WriteLine;

            var c = db.Course.Find(10);
            c.Title = "MVC6 + C# 6.0 + Asp.net Core";
            if (db.Entry(c).State == System.Data.Entity.EntityState.Modified)
            {
                var ce = db.Entry(c);
                var v1 = ce.CurrentValues.GetValue<string>("Title");
                v1 = c.Title;
                v1 = ce.Entity.Title;

                var v2 = ce.OriginalValues.GetValue<string>("Title");
                foreach (var prop in ce.OriginalValues.PropertyNames)
                {
                    //ce.OriginalValues.GetValue<string>("Title");
                }
                Console.WriteLine("New:" + v1 + "\t\nOrig:" + v2);

                /*將ModifiedOn改至SaveChanges的partial中（請參考ContosoUniversityEntities.partial.cs），未來使用到*/
                //ce.CurrentValues.SetValues(new
                //{
                //    ModifiedOn = DateTime.Now
                //});
                db.SaveChanges();
            }

            #endregion 自製log
        }

        private static void SP練習_Select(ContosoUniversityEntities db)
        {
            #region SP練習 - Select

            var sp = db.Get各部門開課數量統計();
            foreach (var item in sp)
            {
                Console.WriteLine(item.DeptName + "\t" + item.CourseCount);
            }

            #endregion SP練習 - Select
        }

        private static void SP練習_Insert(ContosoUniversityEntities db)
        {
            #region SP練習 - Insert

            var data = db.Insert部門資料("資訊部門", 50000.12m);

            #endregion SP練習 - Insert
        }

        private static void Enum(ContosoUniversityEntities db)
        {
            #region Enum

            db.Database.Log = Console.WriteLine;

            var data = db.Course.Where(p => p.CourseType.Value.HasFlag(CourseType.後端));

            foreach (var item in data)
            {
                Console.WriteLine(item.Title);
            }

            //var c = db.Course.Find(5);
            //c.CourseType = CourseType.前端 | CourseType.後端;
            //db.SaveChanges();

            Console.WriteLine(db.Course.Find(5).CourseType);

            #endregion Enum
        }

        private static void 預先載入與延遲載入(ContosoUniversityEntities db)
        {
            #region 預先載入與延遲載入

            db.Database.Log = Console.WriteLine;

            //關閉延遲載入
            db.Configuration.LazyLoadingEnabled = false;

            //使用Include預先載入（EF會將Course join Department，若筆數太多，要注意效能問題）
            var data = db.Course//.Include(s => s.Department)
                .Where(p => p.CourseType.Value.HasFlag(CourseType.後端));

            foreach (var item in data)
            {
                Console.WriteLine("-----------------------");
                Console.WriteLine(item.Title);

                //使用Reference與Load進行手動載入（因為已經關掉延遲載入，假設沒使用Include預先載入，會錯誤！）
                //若有一堆部門資料，每次出現新的部門時，都會再去DB撈取一次
                var RefLink = db.Entry(item).Reference(p => p.Department);
                if (!RefLink.IsLoaded)
                {
                    RefLink.Load();
                }

                Console.WriteLine(item.Department.Name);
                Console.WriteLine("-----------------------");
            }

            //備註：如果『預先載入』資料量太多，效能不好。『延遲載入』在迴圈內又會一直撈資料，效能也不好。可以將語法改至view中，效能會比較好。

            //備註：Include與Reference要使用強型別，請using System.Data.Entity;

            #endregion 預先載入與延遲載入
        }

        private static void 輸出Log(ContosoUniversityEntities db)
        {
            #region 輸出Log

            db.Database.Log = (msg) =>
            {
                //輸出檔案
                //File.AppendAllText(@"C:\Users\User\Documents\visual studio 2015\Projects\EFConsole", msg);

                //輸出至『輸出』
                Debug.WriteLine(msg);
            };

            var data = db.Course.Where(s => s.CourseType == CourseType.後端);
            foreach (var item in data)
            {
                Console.WriteLine(item.Title);
            }

            #endregion 輸出Log
        }

        private static void 離線物件變成連線物件()
        {
            #region 離線物件變成連線物件

            var c = new Course()
            {
                CourseID = 11,
                Title = "123",
                DepartmentID = 1,
                Credits = 1
            };

            //寫法1：使用Attach繫結離線物件，但繫結後是Unchanged狀態，要再修改儲存才有意義
            using (var db = new ContosoUniversityEntities())
            {
                Console.WriteLine(db.Entry(c).State);

                db.Course.Attach(c);
                Console.WriteLine(c.Title);
                Console.WriteLine(db.Entry(c).State);

                //這邊會再去db撈取編號11的資料，但撈回來後發現catch已經有資料，所以Title不會是DB資料，而是catch中的123。
                db.Course.ToList();
                var tt = db.Course.Find(11);
                Console.WriteLine(c.Title);

                //c.Title = "MVC 6";
                //Console.WriteLine(db.Entry(c).State);

                //db.SaveChanges();
            }

            //寫法2：直接改狀態（不建議這樣寫，有資安問題！）
            using (var db = new ContosoUniversityEntities())
            {
                c.Title = "MVC Core 1.0";
                db.Entry(c).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            #endregion 離線物件變成連線物件
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