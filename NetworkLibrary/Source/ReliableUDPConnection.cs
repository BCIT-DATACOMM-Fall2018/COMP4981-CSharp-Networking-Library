using System;
using System.Collections.Generic;
using NetworkLibrary.MessageElements;

namespace NetworkLibrary
{
	/// ----------------------------------------------
	/// Class: 	ReliableUDPConnection - A class to keep track of state in a
	/// 								reliable udp connection.
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public ReliableUDPConnection ()
	/// 
	/// FUNCTIONS:	public Packet CreatePacket (List<UpdateElement> unreliableElements, List<UpdateElement> reliableElements)
	///				public UnpackedPacket ProcessPacket (Packet packet)
	/// 
	/// DATE: 		January 28th, 2018
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		This class does not send or recieve data. It is purely
	/// 			for the purpose of keeping track of keeping track of
	/// 			the connection state.
	/// ----------------------------------------------
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

		/// ----------------------------------------------
		/// FUNCTION:	CreatePacket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public Packet CreatePacket (List<UpdateElement> unreliableElements, List<UpdateElement> reliableElements = null)
		/// 				List<UpdateElement> unreliableElements: A list of unreliable elements to add to the packet.
		/// 				List<UpdateElement> reliableElements: A list of reliable elements to add to the message buffer. Optional parameter.
		/// 
		/// RETURNS: 	A Packet object to be consumed to another ReliableUDPConnection
		/// 
		/// NOTES:		The reliable elements given to this function will be added to a message buffer
		/// 			that will resend them after a certain amount of packet creations if they have 
		/// 			not been acknowledged by an consumed packet.
		/// ----------------------------------------------
		public Packet CreatePacket (List<UpdateElement> unreliableElements, List<UpdateElement> reliableElements = null)
		{
			if (reliableElements != null) {
				foreach (var element in reliableElements) {
					MessageBuffer [MessageIndex % BUFFER_SIZE] = element;
					MessageTimer [MessageIndex % BUFFER_SIZE] = 0;
					MessageIndex++;
				}
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

		/// ----------------------------------------------
		/// FUNCTION:	ProcessPacket
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public UnpackedPacket ProcessPacket (Packet packet, ElementId[] expectedUnreliableIds)
		/// 				Packet packet: The packet to consume
		/// 				ElementId[] expectedUnreliableIds: An array of IDs to use to create the unreliable UpdateElements
		/// 
		/// RETURNS: 	An unpacked packet containing unreliable and reliable UpdateElements
		/// 
		/// NOTES:		Consuming a packet will change the state of the connection.
		/// ----------------------------------------------
		public UnpackedPacket ProcessPacket (Packet packet, ElementId[] expectedUnreliableIds)
		{
			BitStream bitStream = new BitStream (packet.Data);
			PacketHeaderElement header = new PacketHeaderElement (bitStream);

			// read packet header info
			int seqNumber = header.SeqNumber;
			int ackNumber = header.AckNumber;
			int	reliableCount = header.ReliableElements;

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			// check that this unreliable information is new
			if (seqNumber >= CurrentAck -1) {
				// extract unreliable elements from packet
				foreach (var id in expectedUnreliableIds) {
					unreliableElements.Add (messageFactory.CreateUpdateElement(id, bitStream));
				}
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

