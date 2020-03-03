#region copyright
// Copyright (c) 2018 Project Agonyl
#endregion

using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Agonyl.A3TcpProxy
{
    class Program
    {
        static void Main(string[] args)
        {
			try
			{
				CliUtil.WriteHeader("A3 TCP Proxy", ConsoleColor.Magenta);
				CliUtil.LoadingTitle();

				// Ready
				CliUtil.RunningTitle();

				// Config
				var config = new Config();

				using (var r = new StreamReader("config.json"))
				{
					var json = r.ReadToEnd();
					config = JsonConvert.DeserializeObject<Config>(json);
				}

				// Loading global config so that it can be accessed by TcpForwarder
				GlobalConfig.SourceHost = config.SourceHost;
				GlobalConfig.SourcePort = config.SourcePort;
				GlobalConfig.DestinationHost = config.DestinationHost;
				GlobalConfig.DestinationPort = config.DestinationPort;
				GlobalConfig.ClientVersionFix = config.ClientVersionFix;

				Log.Status("A3 TCP proxy ready");
				Log.Status("Forwarding data from {0}:{1} to {2}:{3}", config.SourceHost, config.SourcePort, config.DestinationHost, config.DestinationPort);

				if (GlobalConfig.ClientVersionFix > 0)
				{
					Log.Info("Client version will be fixed to " + GlobalConfig.ClientVersionFix);
				}

				// Start forwarding packets
				new TcpForwarder().Start(
					new IPEndPoint(IPAddress.Parse(config.SourceHost), config.SourcePort),
					new IPEndPoint(IPAddress.Parse(config.DestinationHost), config.DestinationPort));

				// Wait for enter key to exit
				Console.ReadLine();
			}
			catch (Exception e)
			{
				Log.Exception(e);
				// Wait for enter key to exit
				Console.ReadLine();
			}
		}
    }
}
