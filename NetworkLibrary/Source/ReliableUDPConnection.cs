using System;
using System.Collections.Generic;
using NetworkLibrary.MessageElements;

namespace NetworkLibrary
{
	public class ReliableUDPConnection
	{

		private const int BUFFER_SIZE = 1024;

		private MessageElement[] MessageBuffer { get; set; }

		public int MessageIndex{ get; private set; }

		public int LastKnownWanted { get; private set; }

		public int CurrentAck { get; private set; }

		private int[] MessageTimer { get; set; }

		private MessageFactory messageFactory;

		public ReliableUDPConnection ()
		{
			MessageBuffer = new MessageElement[BUFFER_SIZE];
			MessageTimer = new int[BUFFER_SIZE];
			CurrentAck = 1;
			messageFactory = new MessageFactory ();
		}

		public Packet CreatePacket (List<UpdateElement> unreliableElements, List<UpdateElement> reliableElements)
		{
			foreach (var element in reliableElements) {
				MessageBuffer [MessageIndex % BUFFER_SIZE] = element;
				MessageTimer [MessageIndex % BUFFER_SIZE] = 0;
				MessageIndex++;
			}

			int currentPacket = MessageIndex;

			var packetReliableElements = new List<MessageElement> ();
			bool resending = false;
			for (int i = LastKnownWanted; i != MessageIndex; i++) {
				if (resending || MessageTimer [i % BUFFER_SIZE] == 0) {
					packetReliableElements.Add (MessageBuffer [i % BUFFER_SIZE]);
					MessageTimer [i % BUFFER_SIZE] = 3;
					resending = true;
				} else {
					MessageTimer [i % BUFFER_SIZE]--;
				}
			}

			//TODO Create packet of the needed size
			Packet packet = new Packet ();
			BitStream bitStream = new BitStream (packet.Data);

			// get header data
			int seqNumber = MessageIndex;
			int ack = CurrentAck;
			int reliableCount = packetReliableElements.Count;

			// put packet header information into packet
			PacketHeaderElement header = new PacketHeaderElement (seqNumber, ack, reliableCount);
			header.WriteTo (bitStream);



			// pack unreliable elements into packet
			foreach (var element in unreliableElements) {
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

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			// check that this unreliable information is new
			if (seqNumber >= CurrentAck) {
				// extract unreliable elements from packet
				//TODO Add actual elements that need to be read from unreliable section
				unreliableElements.Add (new HealthElement (bitStream));
			}
			

			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			// reliable elements were not missed
			if (CurrentAck + reliableCount - 1 == seqNumber) {
				// extract reliable elements from packet
				for (int i = 0; i < reliableCount; i++) {
					// get element indicator
					ElementIndicatorElement indicatorElement = new ElementIndicatorElement (bitStream);
					// use indicator to create message and call its update state function;
					reliableElements.Add (messageFactory.CreateUpdateElement (indicatorElement.ElementIndicator, bitStream));
				}
				CurrentAck += reliableCount;
			}
			return new UnpackedPacket (unreliableElements, reliableElements);
		}
	}
}

