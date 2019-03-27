using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: MovementElement - A UpdateElement to update actor position
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public MovementElement (int actorId, int x, int z, float targetX, float targetZ)
	/// 				public MovementElement (BitStream bitstream)
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
	public class MovementElement : UpdateElement
	{
		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.MovementElement);

		private const int ACTORID_MAX = 127;
		private const float X_MAX = 500;
		private const float Z_MAX = 500;
		private const float TARGETX_MAX = 500;
		private const float TARGETZ_MAX = 500;

		private static readonly int ACTORID_BITS = RequiredBits (ACTORID_MAX);
		private static readonly int X_BITS = sizeof (float)*BitStream.BYTE_SIZE;
		private static readonly int Z_BITS = sizeof (float)*BitStream.BYTE_SIZE;
		private static readonly int TARGETX_BITS = sizeof (float)*BitStream.BYTE_SIZE;
		private static readonly int TARGETZ_BITS = sizeof (float)*BitStream.BYTE_SIZE;


		public int ActorId { get; private set; }

		public float X { get; private set; }

		public float Z { get; private set; }

		public float TargetX { get; private set; }

		public float TargetZ { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: MovementElement
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public MovementElement (int actorId, int x, int z, float targetX, float targetZ)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public MovementElement (int actorId, float x, float z, float targetX, float targetZ)
		{
			ActorId = actorId;
			X = x;
			Z = z;
			TargetX = targetX;
			TargetZ = targetZ;
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: MovementElement
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
		/// 		MovementElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public MovementElement (BitStream bitstream) : base (bitstream)
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
		/// RETURNS: 	An ElementIndicatorElement appropriate for a MovementElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a MovementElement when 
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
		/// 			MovementElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a MovementElement
		/// ----------------------------------------------
		public override int Bits(){
			return ACTORID_BITS + X_BITS + Z_BITS + TARGETX_BITS + TARGETZ_BITS;
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
		/// 			MovementElement to a BitStream.
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
			bytes = BitConverter.GetBytes (TargetX);
			foreach (var item in bytes) {
				bitStream.Write (item, 0, BitStream.BYTE_SIZE);
			}
			bytes = BitConverter.GetBytes (TargetZ);
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
		/// 			MovementElement from a BitStream.
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
			bytes = new byte[sizeof(float)];
			for (int i = 0; i < sizeof(float); i++) {
				bytes [i] = bitstream.ReadNextByte (BitStream.BYTE_SIZE);
			}
			TargetX = BitConverter.ToSingle (bytes, 0);
			bytes = new byte[sizeof(float)];
			for (int i = 0; i < sizeof(float); i++) {
				bytes [i] = bitstream.ReadNextByte (BitStream.BYTE_SIZE);
			}
			TargetZ = BitConverter.ToSingle (bytes, 0);

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
			bridge.SetActorMovement (ActorId, X, Z, TargetX, TargetZ);
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
		/// 			MovementElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
			if (ActorId > ACTORID_MAX || X > X_MAX || Z > Z_MAX || TargetX > TARGETX_MAX || TargetZ > TARGETZ_MAX) {
				throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
			}
		}
	}
}

