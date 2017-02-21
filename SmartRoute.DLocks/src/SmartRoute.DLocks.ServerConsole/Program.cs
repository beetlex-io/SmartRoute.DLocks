using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRoute.DLocks.ServerConsole
{
	public class Program
	{
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
	}
}
