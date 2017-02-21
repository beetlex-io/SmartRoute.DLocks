using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRoute.DLocks.Protocols
{
	[ProtoContract]
	public class FreeKey
	{
		[ProtoMember(1)]
		public string Owner { get; set; }

		[ProtoMember(2)]
		public string Key { get; set; }
	}
}
