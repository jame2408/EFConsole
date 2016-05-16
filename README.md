# EFConsole
練習使用entity framework 6

---

練習項目：
01 建立 ContosoUniversity 資料庫
02 練習建立 Entity Framework 實體資料模型並取出 dbo.Course 資料
03 練習用 LINQ 查詢資料 ( 查出 Course 資料表中所有 Git 資料 ) 與新增資料到資料庫 ( 查詢新增成功後 CourseID 的值 )
04 練習用 Entity Framework 批次更新與刪除資料
05 練習套用實體模型顏色與多圖表管理
06 練習「從資料庫更新模型」的各種注意事項
07 練習手動刪除特定實體模型時，如何復原原本的欄位定義 (砍掉重練+版控復原)
08 練習「導覽屬性」的應用與 POCO 代理物件的特性
09 練習用 Native SQL 外加自定義 Model 來接收查詢結果
10 解決 SQL Server 檢視表 (Views) 無法匯入 EDMX 的問題
11 設定實體物件的預設值的開發技巧
12 了解「預設值」的應用技巧（資料庫設定預設值、EDMX 設定 StoredGeneratePattern="Computed" )
13 深入了解 DbEntityEntry<T> 類別，檢查物件狀態與調整物件狀態
14 深入了解 DbPropertyValues 類別，練習取得與設定 OriginalValues 或 CurrentValues 的值
15 自訂/覆寫 ContosoUniversityEntities 的 SaveChanges() 方法
16 透過程式碼理解 Entity Framework 的 連線模式 v.s. 離線模式
17 練習使用並行模式 (Concurrency Mode) 控制資料可否更新 (DbUpdateConcurrencyException)
18 練習匯入 Stored Procedure (預存程序) 查詢資料
19 練習實體 (Entity) 與 Stored Procedure (預存程序) 對應，設定 Create, Update, Delete 的實體對應到函式
20 使用列舉型別替代原本 Int16, Int32, Int64, Byte, SBye 類型的屬性
21 練習 預先載入 (Eager Loading) 與 延遲載入 (Lazy Loading) 的各種開發技巧
22 追蹤 EF 對 DB 下達的 SQL 指令
23 使用 EF Reverse POCO Generator 動態產生 Code First 所需的 EDM 
