using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: PositionElement - A UpdateElement to update actor position
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public PositionElement (int actorId, int x, int z)
	/// 				public PositionElement (BitStream bitstream)
	/// 
	/// FUNCTIONS:	public override ElementIndicatorElement GetIndicator ()
	///				protected override void Serialize (BitStream bitStream)
	/// 			protected override void Deserialize (BitStream bitstream)
	/// 			public override void UpdateState (IStateMessageBridge bridge)
	/// 			protected override void Validate ()
	/// 
	/// DATE: 		March 8th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		
	/// ----------------------------------------------
	public class PositionElement : UpdateElement
	{
		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.PositionElement);

		private const int ACTORID_MAX = 127;
		private const float X_MAX = 255;
		private const float Z_MAX = 255;

		private static readonly int ACTORID_BITS = RequiredBits (ACTORID_MAX);
		private static readonly int X_BITS = sizeof (float)*BitStream.BYTE_SIZE;
		private static readonly int Z_BITS = sizeof (float)*BitStream.BYTE_SIZE;


		public int ActorId { get; private set; }

		public float X { get; private set; }

		public float Z { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: PositionElement
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public PositionElement (int actorId, int x, int z)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public PositionElement (int actorId, float x, float z)
		{
			ActorId = actorId;
			X = x;
			Z = z;
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: PositionElement
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public HealthElement (BitStream bitstream)
		/// 
		/// NOTES:	Calls the parent class constructor to create a 
		/// 		PositionElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public PositionElement (BitStream bitstream) : base (bitstream)
		{
		}

		/// ----------------------------------------------
		/// FUNCTION:	GetIndicator
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public override ElementIndicatorElement GetIndicator ()
		/// 
		/// RETURNS: 	An ElementIndicatorElement appropriate for a PositionElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a PositionElement when 
		/// 			deserializing a Packet.
		/// ----------------------------------------------
		public override ElementIndicatorElement GetIndicator ()
		{
			return INDICATOR;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Bits
		/// 
		/// DATE:		March 8th, 2019
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
		/// 			PositionElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a PositionElement
		/// ----------------------------------------------
		public override int Bits(){
			return ACTORID_BITS + X_BITS + Z_BITS;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Serialize
		/// 
		/// DATE:		March 8th, 2019
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
		/// 			PositionElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write (ActorId, 0, ACTORID_BITS);
			byte[] bytes = BitConverter.GetBytes (X);
			foreach (var item in bytes) {
				bitStream.Write (item, 0, BitStream.BYTE_SIZE);
			}
			bytes = BitConverter.GetBytes (Z);
			foreach (var item in bytes) {
				bitStream.Write (item, 0, BitStream.BYTE_SIZE);
			}
		}

		/// ----------------------------------------------
		/// FUNCTION:	Deserialize
		/// 
		/// DATE:		March 8th, 2019
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
		/// 			PositionElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			ActorId = bitstream.ReadNext (ACTORID_BITS);
			byte[] bytes = new byte[sizeof(float)];
			for (int i = 0; i < sizeof(float); i++) {
				bytes [i] = bitstream.ReadNextByte (BitStream.BYTE_SIZE);
			}
			X = BitConverter.ToSingle (bytes, 0);
			bytes = new byte[sizeof(float)];
			for (int i = 0; i < sizeof(float); i++) {
				bytes [i] = bitstream.ReadNextByte (BitStream.BYTE_SIZE);
			}
			Z = BitConverter.ToSingle (bytes, 0);

		}

		/// ----------------------------------------------
		/// FUNCTION:	UpdateState
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public override void UpdateState (IStateMessageBridge bridge)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		Contains logic needed to update the game state through
		/// 			the use of a IStateMessageBridge.
		/// ----------------------------------------------
		public override void UpdateState (IStateMessageBridge bridge)
		{
			bridge.UpdateActorPosition (ActorId, X, Z);
		}

		/// ----------------------------------------------
		/// FUNCTION:	Validate
		/// 
		/// DATE:		March 8th, 2019
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
		/// 			PositionElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
			if (ActorId > ACTORID_MAX || X > X_MAX || Z > Z_MAX) {
				throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
			}
		}
	}
}

