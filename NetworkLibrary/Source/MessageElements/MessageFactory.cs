using System;
using NetworkLibrary.MessageElements;

namespace NetworkLibrary.MessageElements
{
	public class MessageFactory
	{

		public MessageFactory ()
		{
		}

		public MessageElement CreateMessageElement (ElementId id, BitStream bitStream)
		{
			switch (id) {
			case ElementId.PacketHeaderElement:
				return new PacketHeaderElement (bitStream);
			case ElementId.HealthElement:
				return new HealthElement (bitStream);
			default:
				throw new InvalidOperationException ();
				;
			}
		}

		public UpdateElement CreateUpdateElement (ElementId id, BitStream bitStream)
		{
			switch (id) {
			case ElementId.HealthElement:
				return new HealthElement (bitStream);
			default:
				throw new InvalidOperationException ();
				;
			}
		}
	}
}

