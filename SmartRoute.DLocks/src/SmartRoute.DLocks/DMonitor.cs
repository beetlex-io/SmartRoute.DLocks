using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRoute.DLocks
{
	public class DMonitor
	{

		private string mID;

		public DMonitor(INode node = null)
		{
			if (node == null)
				node = SmartRoute.NodeFactory.Default;
			Node = node;
			mID = "SMART_DLOCKER_" + Guid.NewGuid().ToString("N");
			Subscriber = Node.Register<EventSubscriber>(mID);
			TimeOut = 5000;

		}

		public int TimeOut { get; set; }

		public SmartRoute.EventSubscriber Subscriber { get; set; }

		public INode Node { get; private set; }

		public IDisposable Enter(string key)
		{
			string owner = mID + "_" + System.Threading.Thread.CurrentThread.ManagedThreadId;
			Protocols.EnterKey enter = new Protocols.EnterKey();
			enter.Owner = owner;
			enter.Key = key;
			Protocols.LockResult result = Subscriber.Publish<Protocols.LockResult>(DMonitoCenter.SMART_DLOCKCENTER, enter, TimeOut);
			if (!result.Success)
				throw new DLockExecption(result.StatusMessage);
			return new Locker { Key = key, Owner = owner, Moitor = this };


		}

		public void Exit(string key, string owner)
		{
			Protocols.FreeKey free = new Protocols.FreeKey();
			free.Owner = owner;
			free.Key = key;
			Protocols.LockResult result = Subscriber.Publish<Protocols.LockResult>(DMonitoCenter.SMART_DLOCKCENTER, free, TimeOut);
			if (!result.Success)
				throw new DLockExecption(result.StatusMessage);
		}

	}
}
