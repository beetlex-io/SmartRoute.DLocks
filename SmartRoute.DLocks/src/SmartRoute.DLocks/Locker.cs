using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRoute.DLocks
{
	class Locker : IDisposable
	{
		public string Owner { get; set; }

		public string Key { get; set; }

		public DMonitor Moitor { get; set; }

		public void Dispose()
		{
			Moitor.Exit(Key, Owner);
		}
	}
}
