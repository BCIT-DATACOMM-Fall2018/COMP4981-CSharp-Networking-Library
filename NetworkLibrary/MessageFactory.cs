using System;

namespace NetworkLibrary
{
	public class MessageFactory
	{

		public MessageFactory ()
		{
			
		}

		public MessageElement CreateMessage(ElementId id, BitStream bitStream){
			switch (id) {
			case ElementId.PacketHeaderElement:
				return new PacketHeaderElement(bitStream);
			case ElementId.HealthElement:
				return new HealthElement (bitStream);
			default:
				throw new InvalidOperationException ();;
			}
		}
	}
}

