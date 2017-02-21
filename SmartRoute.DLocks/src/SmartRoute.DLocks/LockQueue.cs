using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartRoute.DLocks
{
	class LockerQueue
	{


		public LockerQueue(int timeout, INode node)
		{
			TimeOut = timeout;
			Node = node;
		}

		private Queue<LockInfo> mLokers = new Queue<LockInfo>();

		public Action<LockerQueue> BeEmpty { get; set; }

		public int TimeOut { get; set; }

		public INode Node { get; set; }

		public string Key { get; set; }

		public void Push(LockInfo info)
		{
			if (info.Type == ProcessType.Enter)
			{
				Enter(info);
			}
			else
			{
				Free(info);
			}
		}

		private void Enter(LockInfo info)
		{
			info.Time = Node.GetRuntime();
			mLokers.Enqueue(info);
			if (mLokers.Count == 1)
			{
				Protocols.LockResult result = new Protocols.LockResult();
				result.Success = true;
				info.RequestMessage.Reply(result);
				Node.Loger.Process(LogType.DEBUG, "{0}/{1} enter!", info.Owner, info.Key);
			}
			else
			{
				Node.Loger.Process(LogType.DEBUG, "{0}/{1} wait!", info.Owner, info.Key);
			}
		}

		private void Remove()
		{
			mLokers.Dequeue();
			while (mLokers.Count > 0)
			{
				LockInfo locked = mLokers.Peek();
				if (Node.GetRuntime() - locked.Time > TimeOut)
				{
					Node.Loger.Process(LogType.DEBUG, "{0}/{1} lock timeout!", locked.Owner, locked.Key);
					mLokers.Dequeue();
				}
				else
				{
					Protocols.LockResult result = new Protocols.LockResult();
					result.Success = true;
					locked.RequestMessage.Reply(result);
					Node.Loger.Process(LogType.DEBUG, "{0}/{1} enter!", locked.Owner, locked.Key);
					return;
				}
			}
			if (mLokers.Count == 0)
			{
				if (BeEmpty != null)
					BeEmpty(this);
			}
		}

		private void Free(LockInfo info)
		{
			LockInfo locked = mLokers.Peek();
			Protocols.LockResult result = new Protocols.LockResult();
			if (info.Key == info.Key && info.Owner == info.Owner)
			{
				Node.Loger.Process(LogType.DEBUG, "{0}/{1} exit!", info.Owner, info.Key);
				Remove();
				result.Success = true;
			}
			else
			{
				result.StatusMessage = "invalid operation!";
				result.Success = false;

				Node.Loger.Process(LogType.DEBUG, "{0}/{1} invalid operation {2}/{3}!", info.Owner, info.Key, locked.Owner,
					locked.Key);
			}
			info.RequestMessage.Reply(result);
		}

		public void Detection()
		{
			if (mLokers.Count > 0)
			{
				LockInfo locked = mLokers.Peek();
				if (Node.GetRuntime() - locked.Time > TimeOut)
				{
					Node.Loger.Process(LogType.DEBUG, "{0}/{1} lock timeout!", locked.Owner, locked.Key);
					Remove();
				}
			}

		}
	}


}
