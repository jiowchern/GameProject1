﻿using System;
using System.Collections.Generic;
using Regulus.Utility.WindowConsoleAppliction;

namespace Regulus.Application.Server
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var command = new List<string>();

			foreach(var a in args)
			{
				command.AddRange(a.Split(new[]{' '},StringSplitOptions.RemoveEmptyEntries));
			}
			var app = new Regulus.Remote.Soul.Console.Application(command.ToArray());

			app.Run();
		}
	}
}
