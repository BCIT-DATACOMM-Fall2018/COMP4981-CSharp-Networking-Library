using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: ExperienceElement - A UpdateElement to update actor experience
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public ExperienceElement (int actorId, int health)
	/// 				public ExperienceElement (BitStream bitstream)
	/// 
	/// FUNCTIONS:	public override ElementIndicatorElement GetIndicator ()
	///				protected override void Serialize (BitStream bitStream)
	/// 			protected override void Deserialize (BitStream bitstream)
	/// 			public override void UpdateState (IStateMessageBridge bridge)
	/// 			protected override void Validate ()
	/// 
	/// DATE: 		March 28th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		
	/// ----------------------------------------------
	public class ExperienceElement : UpdateElement
	{
		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.ExperienceElement);

		private const int EXP_MAX = 4000;
		private const int ACTORID_MAX = 127;
		private static readonly int EXP_BITS = RequiredBits (EXP_MAX);
		private static readonly int ACTORID_BITS = RequiredBits (ACTORID_MAX);

		public int ActorId { get; private set; }

		public int Experience { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: ExperienceElement
		/// 
		/// DATE:		March 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public ExperienceElement (int actorId, int experience)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public ExperienceElement (int actorId, int experience)
		{
			ActorId = actorId;
			Experience = experience;
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: ExperienceElement
		/// 
		/// DATE:		March 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public ExperienceElement (BitStream bitstream)
		/// 
		/// NOTES:	Calls the parent class constructor to create a 
		/// 		ExperienceElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public ExperienceElement (BitStream bitstream) : base (bitstream)
		{
		}

		/// ----------------------------------------------
		/// FUNCTION:	GetIndicator
		/// 
		/// DATE:		March 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public override ElementIndicatorElement GetIndicator ()
		/// 
		/// RETURNS: 	An ElementIndicatorElement appropriate for a ExperienceElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a ExperienceElement when 
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
		/// 			ExperienceElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a ExperienceElement
		/// ----------------------------------------------
		public override int Bits(){
			return EXP_BITS + ACTORID_BITS;
		}

		/// ----------------------------------------------
		/// FUNCTION:	Serialize
		/// 
		/// DATE:		March 28th, 2019
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
		/// 			ExperienceElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write (ActorId, 0, ACTORID_BITS);
			bitStream.Write	(Experience, 0, EXP_BITS);
		}

		/// ----------------------------------------------
		/// FUNCTION:	Deserialize
		/// 
		/// DATE:		March 28th, 2019
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
		/// 			ExperienceElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			ActorId = bitstream.ReadNext (ACTORID_BITS);
			Experience = bitstream.ReadNext (EXP_BITS);

		}

		/// ----------------------------------------------
		/// FUNCTION:	UpdateState
		/// 
		/// DATE:		March 28th, 2019
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
			bridge.UpdateActorExperience (ActorId, Experience);
		}

		/// ----------------------------------------------
		/// FUNCTION:	Validate
		/// 
		/// DATE:		March 28th, 2019
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
		/// 			ExperienceElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
			if (ActorId > ACTORID_MAX || Experience > EXP_MAX) {
				throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
			}
		}
	}
}

