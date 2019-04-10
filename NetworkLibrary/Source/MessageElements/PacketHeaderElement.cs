using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: ElementIndicatorElement - A MessageElement to store connection information in a packet
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public PacketHeaderElement (int seqNumber, int ackNumber, int reliableElements)
	/// 				public PacketHeaderElement (BitStream bitStream)
	/// 
	/// FUNCTIONS:	public override PacketHeaderElement GetIndicator ()
	///				protected override void Serialize (BitStream bitStream)
	/// 			protected override void Deserialize (BitStream bitstream)
	/// 			protected override void Validate ()
	/// 
	/// DATE: 		January 28th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		This ElementIndicatorElement is used to indentify reliable 
	/// 			MessageElements placed after it in a Packet.
	/// ----------------------------------------------
	public class PacketHeaderElement : MessageElement
	{
		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.PacketHeaderElement);

		private const int TYPE_BITS = 4;
		private const int SEQ_BITS = 10;
		private const int ACK_BITS = 10;
		private const int RELIABLE_BITS = 10;

		public PacketType Type { get; private set; }

		public int SeqNumber { get; private set; }

		public int AckNumber { get; private set; }

		public int ReliableElements { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: PacketHeaderElement
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public PacketHeaderElement (int seqNumber, int ackNumber, int reliableElements)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public PacketHeaderElement (PacketType type, int seqNumber, int ackNumber, int reliableElements)
		{
			Type = type;
			SeqNumber = seqNumber;
			AckNumber = ackNumber;
			ReliableElements = reliableElements;
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: PacketHeaderElement
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public PacketHeaderElement (BitStream bitstream)
		/// 
		/// NOTES:	Calls the parent class constructor to create a 
		/// 		PacketHeaderElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public PacketHeaderElement (BitStream bitStream) : base (bitStream)
		{
		}

		/// ----------------------------------------------
		/// FUNCTION:	GetIndicator
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public override ElementIndicatorElement GetIndicator ()
		/// 
		/// RETURNS: 	An ElementIndicatorElement appropriate for a PacketHeaderElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a PacketHeaderElement when 
		/// 			deserializing a Packet.
		/// ----------------------------------------------
		public override ElementIndicatorElement GetIndicator ()
		{
			return INDICATOR;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Bits
		/// 
		/// DATE:		February 10th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public int Bits ()
		/// 
		/// RETURNS: 	The number of bits needed to store a
		/// 			PacketHeaderElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a PacketHeaderElement
		/// ----------------------------------------------
		public override int Bits ()
		{
			return SEQ_BITS + ACK_BITS + RELIABLE_BITS + TYPE_BITS;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Serialize
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	protected override void Serialize (BitStream bitStream)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Contains logic needed to serialize a 
		/// 			PacketHeaderElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write ((int)Type, 0, TYPE_BITS);
			bitStream.Write (SeqNumber, 0, SEQ_BITS);
			bitStream.Write	(AckNumber, 0, ACK_BITS);
			bitStream.Write	(ReliableElements, 0, RELIABLE_BITS);
		}

		/// ----------------------------------------------
		/// FUNCTION:	Deserialize
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	protected override void Deserialize (BitStream bitstream)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Contains logic needed to deserialze a 
		/// 			PacketHeaderElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			Type = (PacketType)bitstream.ReadNext (TYPE_BITS);
			SeqNumber = bitstream.ReadNext (SEQ_BITS);
			AckNumber = bitstream.ReadNext (ACK_BITS);
			ReliableElements = bitstream.ReadNext (RELIABLE_BITS);
		}

		/// ----------------------------------------------
		/// FUNCTION:	Validate
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	protected override void Validate ()
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Contains logic needed to validate a 
		/// 			PacketHeaderElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
		}
	}
}

