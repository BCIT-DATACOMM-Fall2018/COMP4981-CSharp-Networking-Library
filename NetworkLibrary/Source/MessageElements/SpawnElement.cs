using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: SpawnElement - A UpdateElement to cause an actor to be spawned
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public SpawnElement (ActorType actorType, int actorId, int x, int y)
	/// 				public SpawnElement (BitStream bitstream)
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
	public class SpawnElement : UpdateElement
	{
		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.SpawnElement);

		private const int ACTORTYPE_MAX = 31;
		private const int ACTORID_MAX = 255;
		private const int X_MAX = 255;
		private const int Y_MAX = 255;

		private static readonly int ACTORTYPE_BITS = RequiredBits (ACTORTYPE_MAX);
		private static readonly int ACTORID_BITS = RequiredBits (ACTORID_MAX);
		private static readonly int X_BITS = RequiredBits (X_MAX);
		private static readonly int Y_BITS = RequiredBits (Y_MAX);


		public ActorType ActorType { get; private set; }

		public int ActorId { get; private set; }

		public int X { get; private set; }

		public int Y { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: SpawnElement
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public SpawnElement (ActorType actorType, int actorId, int x, int y)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public SpawnElement (ActorType actorType, int actorId, int x, int y)
		{
			ActorType = actorType;
			ActorId = actorId;
			X = x;
			Y = y;
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: SpawnElement
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
		/// 		SpawnElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public SpawnElement (BitStream bitstream) : base (bitstream)
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
		/// RETURNS: 	An ElementIndicatorElement appropriate for a SpawnElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a SpawnElement when 
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
		/// 			SpawnElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a SpawnElement
		/// ----------------------------------------------
		public override int Bits(){
			return ACTORTYPE_BITS + ACTORID_MAX + X_BITS + Y_BITS;
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
		/// 			SpawnElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write ((int)ActorType, 0, ACTORTYPE_BITS);
			bitStream.Write (ActorId, 0, ACTORID_BITS);
			bitStream.Write	(X, 0, X_BITS);
			bitStream.Write	(Y, 0, Y_BITS);
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
		/// 			SpawnElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			ActorType = (ActorType)bitstream.ReadNext (ACTORTYPE_BITS);
			ActorId = bitstream.ReadNext (ACTORID_BITS);
			X = bitstream.ReadNext (X_BITS);
			Y = bitstream.ReadNext (Y_BITS);

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
			bridge.SpawnActor (ActorType, ActorId, X, Y);
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
		/// 			SpawnElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
			if ((int)ActorType > ACTORTYPE_BITS || ActorId > ACTORID_MAX || X > X_MAX || Y > Y_MAX) {
				throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
			}
		}
	}
}

