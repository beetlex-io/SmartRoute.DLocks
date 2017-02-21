using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRoute.DLocks
{
	public class DLockExecption : Exception
	{
		public DLockExecption()
		{

		}
		public DLockExecption(string message) : base(message) { }

		public DLockExecption(string message, params object[] parameters) : base(string.Format(message, parameters)) { }

		public DLockExecption(string message, Exception baseError) : base(message, baseError) { }

		public DLockExecption(Exception baseError, string message, params object[] parameters) : base(string.Format(message, parameters), baseError) { }
	}
}
