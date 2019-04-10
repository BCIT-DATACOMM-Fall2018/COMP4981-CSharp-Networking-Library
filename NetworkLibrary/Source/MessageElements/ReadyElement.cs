using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: ReadyElement - A UpdateElement to indicate ready status
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public ReadyElement (int actorId, int health, int team)
	/// 				public ReadyElement (BitStream bitstream)
	/// 
	/// FUNCTIONS:	public override ElementIndicatorElement GetIndicator ()
	///				protected override void Serialize (BitStream bitStream)
	/// 			protected override void Deserialize (BitStream bitstream)
	/// 			public override void UpdateState (IStateMessageBridge bridge)
	/// 			protected override void Validate ()
	/// 
	/// DATE: 		March 8th, 2019
	/// 
	///
	/// REVISIONS: 	March 17th, 2019
	/// 				Modified class to support teams
	/// 
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		
	/// ----------------------------------------------
	public class ReadyElement : UpdateElement
	{
		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.ReadyElement);

		private const int READY_MAX = 1;
		private const int CLIENTID_MAX = 127;
		private const int TEAM_MAX = 7;
		private static readonly int READY_BITS = RequiredBits (READY_MAX);
		private static readonly int CLIENTID_BITS = RequiredBits (CLIENTID_MAX);
		private static readonly int TEAM_BITS = RequiredBits (TEAM_MAX);

		public bool Ready { get; private set; }

		public int ClientId { get; private set; }

		public int Team { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: ReadyElement
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public ReadyElement (int actorId, int health, int team)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public ReadyElement (bool ready, int clientId, int team)
		{
			Ready = ready;
			ClientId = clientId;
			Team = team;
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: ReadyElement
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public ReadyElement (BitStream bitstream)
		/// 
		/// NOTES:	Calls the parent class constructor to create a 
		/// 		ReadyElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public ReadyElement (BitStream bitstream) : base (bitstream)
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
		/// RETURNS: 	An ElementIndicatorElement appropriate for a ReadyElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a ReadyElement when 
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
		/// 			ReadyElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a ReadyElement
		/// ----------------------------------------------
		public override int Bits ()
		{
			return READY_BITS + CLIENTID_BITS + TEAM_BITS;
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
		/// 			ReadyElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write (Ready ? 1 : 0, 0, READY_BITS);
			bitStream.Write (ClientId, 0, CLIENTID_BITS);
			bitStream.Write (Team, 0, TEAM_BITS);
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
		/// 			ReadyElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			Ready = bitstream.ReadNext (READY_BITS) == 1 ? true : false;
			ClientId = bitstream.ReadNext (CLIENTID_BITS);
			Team = bitstream.ReadNext (TEAM_BITS);
		}

		/// ----------------------------------------------
		/// FUNCTION:	UpdateState
		/// 
		/// DATE:		January 28th, 2019
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
			bridge.SetReady (ClientId, Ready, Team);
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
		/// 			ReadyElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
			if (ClientId > CLIENTID_MAX || Team > TEAM_MAX) {
				throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
			}
		}
	}
}

