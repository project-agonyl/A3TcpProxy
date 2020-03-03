#region copyright
// Copyright (c) 2018 Project Agonyl
#endregion

using System;

namespace Agonyl.A3TcpProxy
{
	public static class Util
	{
		/// <summary>
		/// Returns packet with byte equivalent of int
		/// </summary>
		public static byte[] IntToReverseHexBytes(int num)
		{
			if (num == 0)
				return new byte[] { 0x00, 0x00 };
			var hexPort = string.Format("{0:x}", num);
			while (hexPort.Length < 4)
				hexPort = "0" + hexPort;
			var temp = hexPort[2] + hexPort[3].ToString();
			var temp1 = hexPort[0] + hexPort[1].ToString();
			var tempByte = new[] { Convert.ToByte(temp, 16), Convert.ToByte(temp1, 16) };
			return tempByte;
		}
	}
}
