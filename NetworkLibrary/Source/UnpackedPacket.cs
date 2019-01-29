using System;
using System.Collections.Generic;
using NetworkLibrary.MessageElements;

namespace NetworkLibrary
{
	public class UnpackedPacket
	{
		public List<UpdateElement> UnreliableElements { get;}
		public List<UpdateElement> ReliableElements { get;}

		public UnpackedPacket (List<UpdateElement> unreliableElements, List<UpdateElement> reliableElements )
		{
			this.UnreliableElements = unreliableElements;
			this.ReliableElements = reliableElements;
		}
	}
}

