﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Remoting.Ghost
{
	public class LinkState
	{
		public Action	LinkSuccess;
		public Action<string>	LinkFail;
	}
}
