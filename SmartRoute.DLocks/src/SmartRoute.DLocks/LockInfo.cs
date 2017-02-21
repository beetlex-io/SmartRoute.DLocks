using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRoute.DLocks
{
	class LockInfo
	{
		public string Owner { get; set; }

		public string Key { get; set; }

		public long Time { get; set; }

		public Message RequestMessage { get; set; }

		public ProcessType Type { get; set; }

	}

	public enum ProcessType
	{
		Enter,
		Free,
		Detection
	}

}
