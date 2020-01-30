// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandaloneFactory.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the StandaloneFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

﻿using Regulus.Framework;

using Regulus.Remote;
using Regulus.Remote.Standalone;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Storage.User
{
	public class StandaloneFactory : IUserFactoty<IUser>
	{
		private readonly IBinderProvider _Core;

        private readonly IProtocol _Protocol;

        public StandaloneFactory(IBinderProvider core, IProtocol provider)
	    {
	        this._Core = core;
            _Protocol = provider;
	    }

	    IUser IUserFactoty<IUser>.SpawnUser()
		{


			var agent = new Agent(_Protocol);
			agent.ConnectedEvent += () => { this._Core.AssignBinder(agent); };
			return new User(agent);
		}

		ICommandParsable<IUser> IUserFactoty<IUser>.SpawnParser(Command command, Console.IViewer view, IUser user)
		{
			return new CommandParser(command, view, user);
		}
	}
}
