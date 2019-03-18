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
	/// DATE: 		January 28th, 2019
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
		private const int MAXIMUM_RELIABLE_BITS = 1024 * 8;
		private const int BUFFER_SIZE = 1024;
		private const int RESEND_TIMER = 3;
		private const int BYTE_SIZE = 8;
		private readonly ClientIDElement ClientID;

		private MessageElement[] MessageBuffer { get; set; }

		public int MessageIndex{ get; private set; }

		public int LastKnownWanted { get; private set; }

		public int CurrentAck { get; private set; }

		private int[] MessageTimer { get; set; }

		private MessageFactory messageFactory;

		public ReliableUDPConnection (int clientID)
		{
			ClientID = new ClientIDElement (clientID);
			MessageBuffer = new MessageElement[BUFFER_SIZE];
			MessageTimer = new int[BUFFER_SIZE];
			messageFactory = new MessageFactory ();
		}

		/// ----------------------------------------------
		/// FUNCTION:	CreatePacket
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	February 9th, 2019
		/// 				- Changed the reliableElements parameter to be optional and
		/// 				  have a default value of null.
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
		public Packet CreatePacket (List<UpdateElement> unreliableElements, List<UpdateElement> reliableElements = null, PacketType packetType = PacketType.GameplayPacket)
		{
			if (reliableElements != null) {
				for (int i = 1; i <= reliableElements.Count; i++) {
					if ((MessageIndex + i) % BUFFER_SIZE == LastKnownWanted % BUFFER_SIZE) {
						Console.WriteLine ("no space :(");
						throw new InsufficientMemoryException ("Not enough space to add reliable elements to buffer");
					}
				}
				foreach (var element in reliableElements) {
					Console.WriteLine ("Addedreliable element to queue");
					MessageBuffer [MessageIndex % BUFFER_SIZE] = element;
					MessageTimer [MessageIndex % BUFFER_SIZE] = 0;
					MessageIndex++;
				}
			}

			int currentPacket = MessageIndex;


			int neededBits = 0;
			// Calculate bits needed for unreliable elements
			foreach (var element in unreliableElements) {
				neededBits += element.Bits ();
			}
				
			int reliableBits = 0;
			var packetReliableElements = new List<MessageElement> ();
			bool resending = false;
			int indexOfLastMessage = LastKnownWanted-1;
			for (int i = LastKnownWanted; i % BUFFER_SIZE != MessageIndex % BUFFER_SIZE; i++) {
				if (resending || MessageTimer [i % BUFFER_SIZE] == 0) {
					if ((reliableBits + MessageBuffer [i % BUFFER_SIZE].Bits () + MessageBuffer [i % BUFFER_SIZE].GetIndicator ().Bits ()) > MAXIMUM_RELIABLE_BITS) {
						break;
					}
					reliableBits += MessageBuffer [i % BUFFER_SIZE].Bits ();
					reliableBits += MessageBuffer [i % BUFFER_SIZE].GetIndicator ().Bits ();
					packetReliableElements.Add (MessageBuffer [i % BUFFER_SIZE]);
					indexOfLastMessage = i;
					MessageTimer [i % BUFFER_SIZE] = RESEND_TIMER;
					resending = true;
				} else {
					MessageTimer [i % BUFFER_SIZE]--;
				}
			}

			neededBits += reliableBits;


			// get header data
			int ack = CurrentAck % BUFFER_SIZE;
			int reliableCount = packetReliableElements.Count;
			int seqNumber = indexOfLastMessage % BUFFER_SIZE;

			// put packet header information into packet
			PacketHeaderElement header = new PacketHeaderElement (packetType, seqNumber, ack, reliableCount);
			neededBits += header.Bits ();
			neededBits += ClientID.Bits ();

			//TODO Create packet of the needed size
			Packet packet = new Packet ((neededBits - 1) / BYTE_SIZE + 1);
			BitStream bitStream = new BitStream (packet.Data);
			header.WriteTo (bitStream);
			ClientID.WriteTo (bitStream);

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
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	February 9th, 2019
		/// 				- Change the check that the unreliable element is new to fix an error where packets
		/// 				  with no reliable elements would always be discarded.
		/// 					Before: if(seqNumber >= CurrentAck)
		/// 					After: if(seqNumber >= CurrentAck -1)
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
			if (header.Type != PacketType.GameplayPacket && header.Type != PacketType.HeartbeatPacket) {
				throw new ArgumentException ("Attempted to process non gameplay packet as a gameplay packet.");
			}

			ClientIDElement packetClientID = new ClientIDElement (bitStream);

			// read packet header info
			int seqNumber = header.SeqNumber;
			int ackNumber = header.AckNumber;
			int	reliableCount = header.ReliableElements;

			LastKnownWanted = ackNumber;

			List<UpdateElement> unreliableElements = new List<UpdateElement> ();
			// extract unreliable elements from packet
			foreach (var id in expectedUnreliableIds) {
				unreliableElements.Add (messageFactory.CreateUpdateElement (id, bitStream));
			}

			List<UpdateElement> reliableElements = new List<UpdateElement> ();
			try{
				// reliable elements were not missed
				if ((CurrentAck  + reliableCount - 1)% BUFFER_SIZE == seqNumber) {
					// extract reliable elements from packet
					for (int i = 0; i < reliableCount; i++) {
						// get element indicator
						ElementIndicatorElement indicatorElement = new ElementIndicatorElement (bitStream);
						// use indicator to create message and call its update state function;
						reliableElements.Add (messageFactory.CreateUpdateElement (indicatorElement.ElementIndicator, bitStream));
					}
					CurrentAck += reliableCount;
				}
			} catch (InvalidOperationException e){ 
				string packetData = "";
				for (int i = 0; i < packet.Data.Length; i++) {
					packetData += packet.Data[i].ToString("x4") + " ";
				}
				string expectedIds = "[";
				for (int i = 0; i < expectedUnreliableIds.Length; i++) {
					expectedIds += expectedUnreliableIds [i] + ", ";
				}
				throw new InvalidOperationException (e.Message + "Packet Data: " + packetData + ", Expected unreliableElements: " + expectedIds);
			}

			return new UnpackedPacket (unreliableElements, reliableElements);
		}

		/// ----------------------------------------------
		/// FUNCTION:	GetPlayerID
		/// 
		/// DATE:		January 12th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static int GetPlayerID (Packet packet)
		/// 				Packet packet: The packet to retrieve a player ID from
		/// 
		/// RETURNS: 	An integer representing a player ID
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public static int GetPlayerID (Packet packet)
		{
			BitStream bitStream = new BitStream (packet.Data);
			PacketHeaderElement header = new PacketHeaderElement (bitStream);
			ClientIDElement packetClientID = new ClientIDElement (bitStream);
			return packetClientID.ClientID;
		}

		/// ----------------------------------------------
		/// FUNCTION:	GetPacketType
		/// 
		/// DATE:		January 12th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static PacketType GetPacketType (Packet packet)
		/// 				Packet packet: The packet to retrieve type information from
		/// 
		/// RETURNS: 	A PacketType
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public static PacketType GetPacketType (Packet packet)
		{
			BitStream bitStream = new BitStream (packet.Data);
			PacketHeaderElement header = new PacketHeaderElement (bitStream);
			return header.Type;
		}

		/// ----------------------------------------------
		/// FUNCTION:	CreateRequestPacket
		/// 
		/// DATE:		January 12th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static Packet CreateRequestPacket ()
		/// 
		/// RETURNS: 	A request Packet
		/// 
		/// NOTES:		Creates a Packet to send to a server to initiate a connection
		/// ----------------------------------------------
		public static Packet CreateRequestPacket ()
		{
			int neededBits = 0;
			PacketHeaderElement header = new PacketHeaderElement (PacketType.RequestPacket, 0, 0, 0);
			neededBits += header.Bits ();
			Packet packet = new Packet ((neededBits - 1) / BYTE_SIZE + 1);

			BitStream bitStream = new BitStream (packet.Data);
			header.WriteTo (bitStream);

			return packet;
		}

		/// ----------------------------------------------
		/// FUNCTION:	CreateConfirmationPacket
		/// 
		/// DATE:		January 12th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public static Packet CreateConfirmationPacket(int clientID)
		/// 				int clientID: The client ID to be placed in the packet
		/// 
		/// RETURNS: 	A response Packet
		/// 
		/// NOTES:		Creates a Packet used to respond to a client request.
		/// ----------------------------------------------
		public static Packet CreateConfirmationPacket(int clientID){
			int neededBits = 0;
			PacketHeaderElement header = new PacketHeaderElement (PacketType.ConfirmationPacket, 0, 0, 0);
			ClientIDElement clientIdElement = new ClientIDElement(clientID);
			neededBits += header.Bits ();
			neededBits += clientIdElement.Bits ();
			Packet packet = new Packet ((neededBits - 1) / BYTE_SIZE + 1);

			BitStream bitStream = new BitStream (packet.Data);
			header.WriteTo (bitStream);
			clientIdElement.WriteTo (bitStream);
			return packet;
		}

		public static int GetClientIdFromConfirmationPacket(Packet packet){
			BitStream bitStream = new BitStream (packet.Data);
			PacketHeaderElement header = new PacketHeaderElement (bitStream);
			ClientIDElement idElement = new ClientIDElement (bitStream);
			return idElement.ClientID;

		}
	}
}

