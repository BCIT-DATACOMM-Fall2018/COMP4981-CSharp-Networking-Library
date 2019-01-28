using System;
using System.Collections.Generic;

namespace NetworkLibrary
{
	public class UnpackedPacket
	{
		public List<MessageElement> UnreliableElements { get;}
		public List<MessageElement> ReliableElements { get;}

		public UnpackedPacket (List<MessageElement> unreliableElements, List<MessageElement> reliableElements )
		{
			this.UnreliableElements = unreliableElements;
			this.ReliableElements = reliableElements;
		}
	}
}

