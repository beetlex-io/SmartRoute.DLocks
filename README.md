# SmartRoute.DLocks
SmartRoute分布式锁
##锁服务
``` c#
		public static void Main(string[] args)
		{

			INode node = NodeFactory.Default;
			node.Loger.Type = LogType.ALL;
			node.AddLogHandler(new ConsoleLogHandler(LogType.ALL));
			node.Open();
			DLocks.DMonitoCenter center = new DMonitoCenter();
			center.Start();
			Console.Read();
		}
```

##调用锁
``` c#
			INode node = NodeFactory.Default;
			node.Loger.Type = LogType.ALL;
			node.AddLogHandler(new ConsoleLogHandler(LogType.ALL));
			node.Open();
			mMonitor = new DMonitor();
      
      using (var locker = mMonitor.Enter("henryfan"))
			{
				//....
			}
```
