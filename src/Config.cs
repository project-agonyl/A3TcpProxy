#region copyright
// Copyright (c) 2018 Project Agonyl
#endregion

namespace Agonyl.A3TcpProxy
{
	public class Config
	{
		public string SourceHost { get;set; }
		public int SourcePort { get; set; }
		public string DestinationHost { get; set; }
		public int DestinationPort { get; set; }
		public int ClientVersionFix { get; set; }

		public Config()
		{
			this.SourceHost = "127.0.0.1";
			this.SourcePort = 3550;
			this.DestinationHost = "8.8.8.8";
			this.DestinationPort = 3550;
			this.ClientVersionFix = 0;
		}
	}
}
