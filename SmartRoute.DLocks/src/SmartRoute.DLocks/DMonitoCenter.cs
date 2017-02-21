using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProtoBuf;
namespace SmartRoute.DLocks
{
	public class DMonitoCenter
	{

		internal const string SMART_DLOCKCENTER = "SMART_DLOCKCENTER";

		private EventSubscriber mLockEventSubscriber;

		private Dictionary<string, LockerQueue> mLockers = new Dictionary<string, LockerQueue>();

		private System.Collections.Concurrent.ConcurrentQueue<LockInfo> mLockInfoQueue = new System.Collections.Concurrent.ConcurrentQueue<LockInfo>();

		private System.Threading.Timer mDetectionTimer;

		public int TimeOut { get; set; }

		public INode Node { get; private set; }

		public DMonitoCenter(int timeout = 10000)
		{
			Node = SmartRoute.NodeFactory.Default;
			TimeOut = timeout;
			mLockEventSubscriber = Node.Register<EventSubscriber>(SMART_DLOCKCENTER);
			mLockEventSubscriber.Register<Protocols.EnterKey>(OnEnter);
			mLockEventSubscriber.Register<Protocols.FreeKey>(OnFree);
		}

		public DMonitoCenter(INode node, int timeout = 10000)
		{
			TimeOut = timeout;
			mLockEventSubscriber = node.Register<EventSubscriber>(SMART_DLOCKCENTER);
			mLockEventSubscriber.Register<Protocols.EnterKey>(OnEnter);
			mLockEventSubscriber.Register<Protocols.FreeKey>(OnFree);
			Node = node;
		}

		private void OnEnter(Message msg, Protocols.EnterKey enter)
		{
			LockInfo info = new LockInfo();
			info.RequestMessage = msg;
			info.Key = enter.Key;
			info.Owner = enter.Owner;
			info.Type = ProcessType.Enter;
			mLockInfoQueue.Enqueue(info);
		}

		private void OnFree(Message msg, Protocols.FreeKey free)
		{
			LockInfo info = new LockInfo();
			info.RequestMessage = msg;
			info.Key = free.Key;
			info.Owner = free.Owner;
			info.Type = ProcessType.Free;
			mLockInfoQueue.Enqueue(info);
		}

		private void OnBeEmpty(LockerQueue queue)
		{
			mLockers.Remove(queue.Key);
		}

		private LockerQueue GetQueue(string key)
		{
			LockerQueue result = null;
			if (!mLockers.TryGetValue(key, out result))
			{
				result = new LockerQueue(TimeOut, Node);
				result.Key = key;
				mLockers[key] = result;
			}
			return result;
		}

		private void OnDetection()
		{
			foreach (LockerQueue item in mLockers.Values)
			{
				item.Detection();
			}
		}

		private void OnStart(object state)
		{
			while (true)
			{
				try
				{
					LockInfo result = null;
					if (mLockInfoQueue.TryDequeue(out result))
					{
						if (result.Type == ProcessType.Detection)
						{
							OnDetection();
						}
						else
						{
							LockerQueue lockerQueue = GetQueue(result.Key);
							lockerQueue.Push(result);
						}
					}
					else
					{
						System.Threading.Thread.Sleep(10);
					}
				}
				catch (Exception e_)
				{
					Node.Loger.Process(LogType.DEBUG, "DMonitoCenter start error {0}", e_.Message);
				}
			}
		}

		public void Start()
		{
			System.Threading.ThreadPool.QueueUserWorkItem(OnStart);
			mDetectionTimer = new System.Threading.Timer(o =>
			{
				LockInfo info = new LockInfo();
				info.Type = ProcessType.Detection;
				mLockInfoQueue.Enqueue(info);
			}, null, 2000, 2000);
		}


	}
}
