using System;
using System.Text;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: NameElement - A MessageElement to store a clients Name in a packet
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public NameElement (string name)
	/// 				public NameElement (BitStream bitStream)
	/// 
	/// FUNCTIONS:	public override PacketHeaderElement GetIndicator ()
	///				protected override void Serialize (BitStream bitStream)
	/// 			protected override void Deserialize (BitStream bitstream)
	/// 			protected override void Validate ()
	/// 
	/// DATE: 		February 12th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		This NameElement is used to indentify the 
	/// 			player who sent a packet.
	/// ----------------------------------------------
	public class NameElement : MessageElement
	{
		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.NameElement);

		private const int NAMECHARS_MAX = 32;
		private static readonly int NAMECHARS_BITS = RequiredBits (NAMECHARS_MAX);

		public string Name { get; private set;}

		/// ----------------------------------------------
		/// CONSTRUCTOR: NameElement
		/// 
		/// DATE: 		February 12th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public NameElement (string name)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public NameElement (string name)
		{
			Name = name;
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: NameElement
		/// 
		/// DATE: 		February 12th, 2019
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
		/// 		NameElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public NameElement (BitStream bitStream) : base (bitStream)
		{
		}

		/// ----------------------------------------------
		/// FUNCTION:	GetIndicator
		/// 
		/// DATE: 		February 12th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public override ElementIndicatorElement GetIndicator ()
		/// 
		/// RETURNS: 	An ElementIndicatorElement appropriate for a NameElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a NameElement when 
		/// 			deserializing a Packet.
		/// ----------------------------------------------
		public override ElementIndicatorElement GetIndicator ()
		{
			return INDICATOR;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Bits
		/// 
		/// DATE: 		February 12th, 2019
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
		/// 			NameElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a NameElement
		/// ----------------------------------------------
		public override int Bits(){
			int bits = 0;
			bits += NAMECHARS_BITS;
			byte[] name = Encoding.UTF8.GetBytes (Name);
			if (name.Length > NAMECHARS_MAX) {
				throw new System.Runtime.Serialization.SerializationException ("Attempt to create a lobby status packet with a name greater than 32 characters");
			}
			bits += name.Length * BitStream.BYTE_SIZE;


			return bits;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Serialize
		/// 
		/// DATE: 		February 12th, 2019
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
		/// 			NameElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			byte[] name = Encoding.UTF8.GetBytes (Name);
			if (name.Length > NAMECHARS_MAX) {
				throw new System.Runtime.Serialization.SerializationException ("Attempt to create a lobby status packet with a name greater than 32 characters");
			}
			bitStream.Write (name.Length, 0 , NAMECHARS_BITS);
			foreach (var character in name) {
				bitStream.Write (character, 0, BitStream.BYTE_SIZE);
			}
		}

		/// ----------------------------------------------
		/// FUNCTION:	Deserialize
		/// 
		/// DATE: 		February 12th, 2019
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
		/// 			NameElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			int nameBytes = bitstream.ReadNext(NAMECHARS_BITS);
			byte[] name = new byte[nameBytes];
			for (int j = 0; j < nameBytes; j++) {
				name[j] = bitstream.ReadNextByte (BitStream.BYTE_SIZE);
			}
			Name = Encoding.UTF8.GetString(name);
		}

		/// ----------------------------------------------
		/// FUNCTION:	Validate
		/// 
		/// DATE: 		February 12th, 2019
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
		/// 			NameElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
		}
	}
}

