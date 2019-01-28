using System;
using System.Collections.Generic;

namespace NetworkLibrary
{
	public class ReliableUDPConnection
	{

		private static int BUFFER_SIZE = 1024;

		public MessageElement[] messageBuffer;

		public int messageIndex{ get; private set; }

		public int lastKnownWanted { get; private set; }

		public int currentAck { get; private set; }

		public int[] messageTimer { get; private set; }

		private MessageFactory messageFactory;

		public ReliableUDPConnection ()
		{
			messageBuffer = new MessageElement[BUFFER_SIZE];
			messageTimer = new int[BUFFER_SIZE];
			currentAck = 1;
			messageFactory = new MessageFactory ();
		}

		public Packet CreatePacket (UnpackedPacket unpackedPacket)
		{
			foreach (var element in unpackedPacket.ReliableElements) {
				messageBuffer [messageIndex % BUFFER_SIZE] = element;
				messageTimer [messageIndex % BUFFER_SIZE] = 0;
				messageIndex++;
			}

			int currentPacket = messageIndex;

			var packetReliableElements = new List<MessageElement> ();
			for (int i = lastKnownWanted; i != messageIndex; i++) {
				if (messageTimer [i % BUFFER_SIZE] == 0) {
					packetReliableElements.Add (messageBuffer [i % BUFFER_SIZE]);
					messageTimer [i % BUFFER_SIZE]--;
				} else {
					messageTimer [i % BUFFER_SIZE]--;
				}
			}

			Packet packet = new Packet ();
			BitStream bitStream = new BitStream (packet.Data);

			// get header data
			int seqNumber = messageIndex;
			int ack = currentAck;
			int reliableCount = packetReliableElements.Count;

			// put packet header information into packet
			PacketHeaderElement header = new PacketHeaderElement (seqNumber, ack, reliableCount);
			header.WriteTo (bitStream);



			// pack unreliable elements into packet
			foreach (var element in unpackedPacket.UnreliableElements) {
				element.WriteTo (bitStream);
			}


			// pack reliable packets into header
			foreach (var element in packetReliableElements) {
				element.GetIndicator ().WriteTo (bitStream);
				element.WriteTo (bitStream);
			}

			// return packet
			return packet;
		}

		public UnpackedPacket ProcessPacket (Packet packet)
		{
			BitStream bitStream = new BitStream (packet.Data);
			PacketHeaderElement header = new PacketHeaderElement (bitStream);


			// read packet header info
			int seqNumber = header.SeqNumber;
			int ackNumber = header.AckNumber;
			int	reliableCount = header.ReliableElements;

			
			// extract unreliable elements from packet
			List<MessageElement> unreliableElements = new List<MessageElement> ();
			unreliableElements.Add (new HealthElement (bitStream));

			List<MessageElement> reliableElements = new List<MessageElement> ();
			// reliable elements were not missed
			if (currentAck + reliableCount -1 == seqNumber) {
				// extract reliable elements from packet
				for (int i = 0; i < reliableCount; i++) {
					// get element indicator
					ElementIndicatorElement indicatorElement = new ElementIndicatorElement (bitStream);
					// use indicator to create message and call its update state function;
					reliableElements.Add (messageFactory.CreateMessage (indicatorElement.ElementIndicator, bitStream));
				}
				currentAck += reliableCount;
			}
			return new UnpackedPacket (unreliableElements, reliableElements);
		}
	}
}

