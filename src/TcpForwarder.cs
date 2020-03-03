#region copyright
// Copyright (c) 2018 Project Agonyl
#endregion

using System;
using System.Net;
using System.Net.Sockets;

namespace Agonyl.A3TcpProxy
{
	public class TcpForwarder
	{
		private readonly Socket _mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		public void Start(IPEndPoint local, IPEndPoint remote)
		{
			_mainSocket.Bind(local);
			_mainSocket.Listen(10);

			while (true)
			{
				var source = _mainSocket.Accept();
				var destination = new TcpForwarder();
				var state = new State(source, destination._mainSocket);
				destination.Connect(remote, source);
				source.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, OnDataReceive, state);
			}
		}

		private void Connect(EndPoint remoteEndpoint, Socket destination)
		{
			var state = new State(_mainSocket, destination);
			_mainSocket.Connect(remoteEndpoint);
			_mainSocket.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, OnDataReceive, state);
		}

		private static void OnDataReceive(IAsyncResult result)
		{
			var state = (State)result.AsyncState;
			try
			{
				var bytesRead = state.SourceSocket.EndReceive(result);
				if (bytesRead > 0)
				{
					if (GlobalConfig.ClientVersionFix > 0 && bytesRead == 56 && GlobalConfig.SourcePort == 3550)
					{
						var version = Util.IntToReverseHexBytes(GlobalConfig.ClientVersionFix);
						state.Buffer[52] = version[0];
						state.Buffer[53] = version[1];
					}
					state.DestinationSocket.Send(state.Buffer, bytesRead, SocketFlags.None);
					state.SourceSocket.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, OnDataReceive, state);
				}
			}
			catch
			{
				state.DestinationSocket.Close();
				state.SourceSocket.Close();
			}
		}

		private class State
		{
			public Socket SourceSocket { get; private set; }
			public Socket DestinationSocket { get; private set; }
			public byte[] Buffer { get; private set; }

			public State(Socket source, Socket destination)
			{
				this.SourceSocket = source;
				this.DestinationSocket = destination;
				this.Buffer = new byte[8192];
			}
		}
	}
}
