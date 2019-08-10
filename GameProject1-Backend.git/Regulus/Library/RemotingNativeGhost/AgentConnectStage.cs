﻿using System;
using System.Net;
using System.Net.Sockets;
using Regulus.Network;
using Regulus.Utility;

namespace Regulus.Remote.Ghost
{
	public partial class Agent
	{
		private class ConnectStage : IStage
		{
			public event Action<bool, IPeer> ResultEvent;

			private readonly IPEndPoint _Ip;			

			private readonly IConnectable _Peer;

			private IAsyncResult _AsyncResult;

			private bool? _Result;

			public ConnectStage(IPEndPoint ip, IConnectProviderable agent)
			{
                
			    _Peer = agent.Spawn();


                _Ip = ip;

            }

			void IStage.Enter()
			{
				Singleton<Log>.Instance.WriteInfo("connect stage enter.");
				Singleton<Log>.Instance.WriteInfo("Agent connect start .");

				try
				{
                    // _Peer.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.ReuseAddress, true);
                    // _Peer.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Any, 42255));
                    

                    _Peer.Connect(_Ip, _ConnectResult);
				}
				catch(Exception e)
				{
					Singleton<Log>.Instance.WriteInfo(string.Format("begin connect fail {0}.", e));
					ResultEvent(false, null);
				}
				finally
				{
					Singleton<Log>.Instance.WriteInfo("agent connect started .");
				}
			}

			void IStage.Leave()
			{
				if(_Result.HasValue == false && ResultEvent != null)
				{
					var call = ResultEvent;
					ResultEvent = null;
					call(false, null);
				}

				if(_Result.HasValue && _Result.Value == false)
				{
					_Peer.Close();
				}

				Singleton<Log>.Instance.WriteInfo("Agent connect leave.");
			}

			void IStage.Update()
			{
				_InvokeResultEvent();
			}

			private void _ConnectResult(bool result)
			{
			    _Result = result;
			    Singleton<Log>.Instance.WriteInfo(string.Format("connect result {0}.", _Result));
            }

			private void _InvokeResultEvent()
			{
				if(_Result.HasValue && ResultEvent != null)
				{
					var call = ResultEvent;
					ResultEvent = null;
					call(_Result.Value, _Peer);
				}
			}
		}
	}
}
