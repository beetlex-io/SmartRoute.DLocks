using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRoute.DLocks.Protocols
{
	[ProtoContract]
	public class LockResult
	{
		[ProtoMember(1)]
		public bool Success { get; set; }

		[ProtoMember(2)]
		public string StatusMessage { get; set; }
	}
}
