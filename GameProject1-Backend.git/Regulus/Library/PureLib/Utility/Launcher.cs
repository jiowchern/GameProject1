﻿using System.Collections.Generic;


using Regulus.Framework;

namespace Regulus.Utility
{
	public class Launcher
	{
		private readonly List<IBootable> _Launchers;

		public int Count
		{
			get { return _Launchers.Count; }
		}

		public Launcher()
		{
			_Launchers = new List<IBootable>();
		}

		public void Push(IBootable laucnher)
		{
			_Launchers.Add(laucnher);
		}

		public void Shutdown()
		{
			foreach(var l in _Launchers)
			{
				l.Shutdown();
			}
		}

		public void Launch()
		{
			foreach(var l in _Launchers)
			{
				l.Launch();
			}
		}
	}
}
