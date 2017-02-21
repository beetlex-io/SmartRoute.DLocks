using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRoute.DLocks.TestConsole
{
	public class Program
	{

		private static DMonitor mMonitor;
		public static void Main(string[] args)
		{
			INode node = NodeFactory.Default;
			node.Loger.Type = LogType.ALL;
			node.AddLogHandler(new ConsoleLogHandler(LogType.ALL));
			node.Open();
			mMonitor = new DMonitor();
			System.Threading.Thread.Sleep(10000);
			System.Threading.ThreadPool.QueueUserWorkItem(OnTest);
			System.Threading.ThreadPool.QueueUserWorkItem(OnTest1);
			System.Threading.Thread.Sleep(-1);

		}

		private static void OnTest(Object state)
		{
			using (var locker = mMonitor.Enter("henryfan"))
			{
				System.Threading.Thread.Sleep(2000);
				Console.WriteLine("lock henryfan {0}", DateTime.Now);
			}
		}

		private static void OnTest1(Object state)
		{
			using (var locker = mMonitor.Enter("henryfan"))
			{
				System.Threading.Thread.Sleep(2000);
				Console.WriteLine("lock henryfan {0}", DateTime.Now);
			}
		}
	}
}
