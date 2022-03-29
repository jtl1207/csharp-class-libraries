using System;
using System.Collections;
using System.Data.OleDb;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace 手机令牌
{
    public class DbStart//初始化DB
    {
        public DbStart()
        {
            Debug.WriteLine(111);
            Dbresult.start = true;
            Dbworking.start = true;
            Dbupdate.start = true;
        }
    }
    struct Info
    {
        //传入消息队列的参数
        public int mod;//模式 1.返回行数 2.返回Reader 3.返回第一行第一列
        public string table;//表名称,是否需要更新UI
        public string constr;//sql字符串
        public string key;//唯一key,用于验证返回信息
    }
    struct OutInfo
    {
        //输出的参数
        public ulong id;//唯一操作ID,用于记录
        public string table;//表名称,是否需要更新UI
        public string key;//唯一key,用于验证返回信息

        public string state;//返回执行是否成功
        public OleDbDataReader note_reader;//返回值
        public int note_int;
        public object note_object;

        public string note_error;
    }
    public class DbRange//增删改查入口
    {
        
        public void Add()
        {
            //string strlists = string.Join(",", lists);
            //string strvalues = string.Join(",", values);
            //string constr = "INSERT INTO " + tablename + "(" + strlists + ") VALUES(" + strvalues + ");";
            //Info info = new Info();
            //info.mod = 12;
            //info.constr = constr;
            //info.table = tablename;
            //Dbworking.Database_list.Enqueue(info);
        }
        public void Delete(string tablename)//删除表
        {
            string constr = "DELETE FROM " + tablename;
            Info info = new Info();
            info.mod = 1;
            info.constr = constr;
            info.table = tablename;
            Dbworking.Database_list.Enqueue(info);
        }
        public void Delete(string tablename, string condition_where)//精确删除,自定义where
        {
            string constr = "DELETE FROM " + tablename + " WHERE "+ condition_where;
            Info info = new Info();
            info.mod = 1;
            info.constr = constr;
            info.table = tablename;
            Dbworking.Database_list.Enqueue(info);
        }
        public void Delete_Int(string tablename, string key)//删除表,返回int
        {
            string constr = "DELETE FROM " + tablename;
            Info info = new Info();
            info.mod = 1;
            info.constr = constr;
            info.table = tablename;
            info.key = key;
            Dbworking.Database_list.Enqueue(info);
        }
        public void Replace(string tablename, string list, string value, string[] new_lists, string[] new_values)
        {        }
        public void Replace()
        {
        }
        public void Search_Int(string tablename, string list, string value, string key)//查询目标,返回Int
        {
            string constr = "SELECT * FROM " + tablename + " WHERE " + list + "='" + value + "'";
            Info info = new Info();
            info.mod = 1;
            info.constr = constr;
            info.key = key;
            Dbworking.Database_list.Enqueue(info);
        }
        public void Search_Reader(string tablename, string key)//查询表,返回Reader
        {
            string constr = "SELECT * FROM " + tablename ;
            Info info = new Info();
            info.mod = 2;
            info.constr = constr;
            info.key = key;
            Dbworking.Database_list.Enqueue(info);
        }
        public void Search_Reader(string tablename, string list, string value, string key)//查询目标,返回Reader
        {
            string constr = "SELECT * FROM " + tablename + " WHERE " + list + "='" + value + "'";
            Info info = new Info();
            info.mod = 2;
            info.constr = constr;
            info.key = key;
            Dbworking.Database_list.Enqueue(info);
        }
        public void Search_Reader(string tablename, string condition_where, string key)//精确查询,自定义where,返回Reader
        {
            string constr = "SELECT * FROM " + tablename + " WHERE " + condition_where;
            Info info = new Info();
            info.mod = 2;
            info.constr = constr;
            info.key = key;
            Dbworking.Database_list.Enqueue(info);
        }
        public void Search_Reader(string tablename, string from ,string list, string value, string key)//查询目标,返回部分Reader
        {
            string constr = "SELECT "+ from + " FROM " + tablename + " WHERE " + list + "='" + value + "'";
            Info info = new Info();
            info.mod = 2;
            info.constr = constr;
            info.key = key;
            Dbworking.Database_list.Enqueue(info);
        }
        public void Search_Object(string tablename, string from, string list, string value, string key)//查询目标,返回部分Object
        {
            string constr = "SELECT " + from + " FROM " + tablename + " WHERE " + list + "='" + value + "'";
            Info info = new Info();
            info.mod = 3;
            info.constr = constr;
            info.key = key;
            Dbworking.Database_list.Enqueue(info);
        }
        public void SearchDistinct_Reader(string tablename, string list, string key)//查询不同目标,返回Reader
        {
            string constr = "SELECT DISTINCT "+list+" FROM " + tablename;
            Info info = new Info();
            info.mod = 2;
            info.constr = constr;
            info.key = key;
            Dbworking.Database_list.Enqueue(info);
        }

        public void Any()
        {

        }
    }
    public class DbReturn//等待返回数据,超时抛出异常,请注意回收
    {
        static int Overtime = 1000;
        public bool Return_bool(string key)
        {
            DateTime beforDT = System.DateTime.Now;
            while (true)
            {
                foreach (OutInfo list in Dbworking.Feedback_list)
                {
                    if (list.key == key)
                    {
                        Dbworking.Feedback_list.Remove(list);
                        return list.note_reader.HasRows;
                    }
                }
                DateTime afterDT = System.DateTime.Now;
                TimeSpan ts = afterDT.Subtract(beforDT);
                if (ts.TotalMilliseconds > Overtime)
                {
                    throw new Exception("消息超时");
                }
                Thread.Sleep(1);
            }
            
        }
    }
    static partial class Dbworking
    {
        // 数据库处理
        static public bool start;
        static ulong Conn_num = new ulong();//操作唯一ID
        static public Queue Database_list = new Queue();//消息队列
        static public Queue Feedback_queue = new Queue();//返回队列,用于自动判断
        static public ArrayList Feedback_list = new ArrayList();//返回数组,用于手动接收数据
        static string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data.accdb";
        static OleDbConnection Conn = new OleDbConnection(conStr);
        static Dbworking()
        {
            Threading_Load();
        }
        static void Threading_Load()
        {
            try
            {
                Conn.Open();
                Thread t_queue = new Thread(new ThreadStart(Handling));
                t_queue.IsBackground = true;
                t_queue.Start();
            }
            catch
            {
                MessageBox.Show("数据库连接错误", "警告");
            }
        }
        static void Handling()
        {
            while (true)
            {
                if (Database_list.Count > 0)
                {
                    Info info = (Info)Database_list.Dequeue();
                    Conn_num += 1;
                    string constr = info.constr;
                    int i = info.mod;

                    OutInfo OutInfo = new OutInfo();
                    OutInfo.id = Conn_num;
                    OutInfo.table = info.table;
                    OutInfo.key = info.key;
                    switch (i)
                    {
                        case 1:
                            try
                            {
                                OleDbCommand cmd = Conn.CreateCommand();
                                cmd.CommandText = constr;
                                OutInfo.note_int = cmd.ExecuteNonQuery();
                                OutInfo.state = "done";
                            }
                            catch (Exception e)
                            {
                                OutInfo.state = "error";
                                OutInfo.note_error = Convert.ToString(e);
                            }
                            break;
                        case 2:
                            try
                            {
                                OleDbCommand cmd = Conn.CreateCommand();
                                cmd.CommandText = constr;
                                OutInfo.note_reader = cmd.ExecuteReader();
                                OutInfo.state = "done";
                            }
                            catch (Exception e)
                            {
                                OutInfo.state = "error";
                                OutInfo.note_error = Convert.ToString(e);
                            }
                            break;
                        case 3:
                            try
                            {
                                OleDbCommand cmd = Conn.CreateCommand();
                                cmd.CommandText = constr;
                                OutInfo.note_object = cmd.ExecuteScalar();
                                OutInfo.state = "done";
                            }
                            catch (Exception e)
                            {
                                OutInfo.state = "error";
                                OutInfo.note_error = Convert.ToString(e);
                            }
                            break;
                        }
                    Feedback_queue.Enqueue(OutInfo);
                    if (!(OutInfo.key is null))
                    {
                        Feedback_list.Add(OutInfo);
                    }
                }
            Thread.Sleep(1);
            }
        }
    }
    partial class Dbupdate
    {
        /// <summary>
        /// 反馈
        /// </summary>
        static public bool start;
        static public bool Account = true;
        static public int Account_done = 0;
        static Dbupdate()
        {
            Thread t_queue = new Thread(new ThreadStart(Update_done));
            t_queue.IsBackground = true;
            t_queue.Start();
        }
        static void Update_done()
        {
            while (true)
            {
                if (Account_done == 1)
                {
                    Account = false;
                    Account_done = 0;
                }
                Thread.Sleep(1);
            }
        }
    }
    static partial class Dbresult
    {
        /// <summary>
        /// 输出处理
        /// </summary>
        static public bool start;
        static Dbresult()
        {
            Threading_Load();
        }
        static void Threading_Load()
        {
            Thread t_Result = new Thread(new ThreadStart(Result));
            t_Result.IsBackground = true;
            t_Result.Start();
        }
        static void Result()
        {
            while (true)
            {
                if (Dbworking.Feedback_queue.Count > 0)
                {
                    OutInfo OutInfo = (OutInfo)Dbworking.Feedback_queue.Dequeue();
                    string tabel = OutInfo.table;
                    if (tabel == "Account")
                    {
                        Dbupdate.Account = true;
                    }
                }
                Thread.Sleep(1);
            }
        }
    }


    
}

