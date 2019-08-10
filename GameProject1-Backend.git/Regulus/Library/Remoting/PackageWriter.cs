﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Regulus.Network;
using Regulus.Serialization;
using Regulus.Utility;

namespace Regulus.Remote
{
	public class PackageWriter<TPackage>
	{
	    private readonly ISerializer _Serializer;
		

		public event OnErrorCallback ErrorEvent;

		



		private byte[] _Buffer;



	    

		private IPeer _Peer;

		private volatile bool _Stop;
	    private bool _Idle;

	    /// <summary>
	    ///     Initializes a new instance of the <see cref="PackageWriter" /> class.
	    /// </summary>
	    /// <param name="serializer">序列化物件</param>
	    
		public PackageWriter( ISerializer serializer)
		{
            
            _Serializer = serializer;
        
	        _Idle = true;
		}

		public void Start(IPeer peer)
		{
			_Stop = false;
			_Peer = peer;
		
		}

	    public void Push(TPackage[] packages)
	    {
	        
	        _Write(packages);


	    }
		private void _Write(TPackage[] packages)
		{
			try
			{

                _Buffer = _CreateBuffer(packages);
                                

			    var task = _Peer.Send(_Buffer, 0, _Buffer.Length);
			    task.DoneEvent += _WriteCompletion;



			}
			catch(SystemException e)
			{
			    var info = string.Format("PackageWriter Error Write {0}.", _Peer.Connected);
                Singleton<Log>.Instance.WriteInfo(info);
				if(ErrorEvent != null)
				{
					ErrorEvent();
				}
			}
		}

		private void _WriteCompletion(int send_count )
		{
			try
			{
				if(_Stop == false)
				{
					var sendSize = send_count;
                    NetworkMonitor.Instance.Write.Set(sendSize);
                }
			}
			catch(SystemException e)
			{
				Singleton<Log>.Instance.WriteInfo(string.Format("PackageWriter Error WriteCompletion {0}.", e));
				if(ErrorEvent != null)
				{
					ErrorEvent();
				}
			}
		}

		private byte[] _CreateBuffer(TPackage[] packages)
		{
			var buffers = from p in packages select _Serializer.Serialize(p);

			// Regulus.Utility.Log.Instance.WriteDebug(string.Format("Serialize to Buffer size {0}", buffers.Sum( b => b.Length )));
			using(var stream = new MemoryStream())
			{
				foreach(var buffer in buffers)
				{
				    var len = buffer.Length;
				    var lenCount = Regulus.Serialization.Varint.GetByteCount(len);
				    var lenBuffer = new byte[lenCount];
				    Regulus.Serialization.Varint.NumberToBuffer(lenBuffer, 0, len);
					stream.Write(lenBuffer, 0, lenBuffer.Length);
					stream.Write(buffer, 0, buffer.Length);
				}

				return stream.ToArray();
			}
		}

		public void Stop()
		{
			_Stop = true;

			
			
		}
		
	}
}
