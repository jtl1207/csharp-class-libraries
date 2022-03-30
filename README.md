# .net自用库
未来的,现在写了一点点  
  
现在只是一个简单的登录界面......  
[![](https://github.com/jtl1207/csharp-class-libraries/blob/main/%E5%9B%BE%E7%89%87/%E6%89%8B%E6%9C%BA%E4%BB%A4%E7%89%8C.png)](https://github.com/jtl1207/csharp-class-libraries/blob/main/%E5%9B%BE%E7%89%87/%E6%89%8B%E6%9C%BA%E4%BB%A4%E7%89%8C.png)
#####DB类
一个基于消息队列的异步数据库操作封装
使用方法


    Dbworking.conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data.accdb";//设置连接字符串
    DbStart DbStart = new DbStart();//开始
	
	User = 123;
	Password = 456;
	string key = "注册" + User + DateTime.Now.ToString(); //设置唯一key,用于验证,可以不要
	
	DbRange DbRange = new DbRange();
	DbReturn DbReturn = new DbReturn();
	DbReturn.Overtime = 500;//设置超时时间
	
	DbRange.Search_DataTable("Account", "user", User, key);//向消息队列发送请求
	DataTable dt = DbReturn.Return_datatable(key);//接收返回,key用于验证
	内置部分增删改查,和大部分返回类型