using System;
using System.Collections.Generic;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: RemainingLivesElement - A UpdateElement to update actor health
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public RemainingLivesElement (int actorId, int health)
	/// 				public RemainingLivesElement (BitStream bitstream)
	/// 
	/// FUNCTIONS:	public override ElementIndicatorElement GetIndicator ()
	///				protected override void Serialize (BitStream bitStream)
	/// 			protected override void Deserialize (BitStream bitstream)
	/// 			public override void UpdateState (IStateMessageBridge bridge)
	/// 			protected override void Validate ()
	/// 
	/// DATE: 		April 5th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		
	/// ----------------------------------------------
	public class RemainingLivesElement : UpdateElement
	{
		public struct LivesInfo
		{
			public int TeamNumber { get; private set;}
			public int Lives { get; private set;}

			public LivesInfo (int teamNumber, int lives)
			{
				this.TeamNumber = teamNumber;
				this.Lives = lives;
			}
		}


		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.RemainingLivesElement);

		private const int TEAMS_MAX = 7;
		private const int TEAMNUM_MAX = 7;
		private const int LIVES_MAX = 50;
		private static readonly int TEAMS_BITS = RequiredBits (TEAMS_MAX);
		private static readonly int TEAMNUM_BITS = RequiredBits (TEAMNUM_MAX);
		private static readonly int LIVES_BITS = RequiredBits (LIVES_MAX);

		public List<LivesInfo> TeamLivesInfo { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: RemainingLivesElement
		/// 
		/// DATE:		April 5th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public RemainingLivesElement (List<LivesInfo> livesInfo)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public RemainingLivesElement (List<LivesInfo> livesInfo)
		{
			TeamLivesInfo = new List<LivesInfo>();
			foreach (var item in livesInfo) {
				TeamLivesInfo.Add(new LivesInfo(item.TeamNumber, item.Lives));
			}
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: RemainingLivesElement
		/// 
		/// DATE:		April 5th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public RemainingLivesElement (BitStream bitstream)
		/// 
		/// NOTES:	Calls the parent class constructor to create a 
		/// 		RemainingLivesElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public RemainingLivesElement (BitStream bitstream) : base (bitstream)
		{
		}

		/// ----------------------------------------------
		/// FUNCTION:	GetIndicator
		/// 
		/// DATE:		April 5th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public override ElementIndicatorElement GetIndicator ()
		/// 
		/// RETURNS: 	An ElementIndicatorElement appropriate for a RemainingLivesElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a RemainingLivesElement when 
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
		/// 			RemainingLivesElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a RemainingLivesElement
		/// ----------------------------------------------
		public override int Bits(){
			int bits = 0;
			bits += TEAMS_BITS;
			bits += TeamLivesInfo.Count * (TEAMNUM_BITS + LIVES_BITS);
			return bits;		}

		/// ----------------------------------------------
		/// FUNCTION:	Serialize
		/// 
		/// DATE:		April 5th, 2019
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
		/// 			RemainingLivesElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write (TeamLivesInfo.Count, 0, TEAMS_BITS);
			foreach (var item in TeamLivesInfo) {
				bitStream.Write (item.TeamNumber, 0, TEAMNUM_BITS);
				bitStream.Write (item.Lives, 0, LIVES_BITS);
			}
		}

		/// ----------------------------------------------
		/// FUNCTION:	Deserialize
		/// 
		/// DATE:		April 5th, 2019
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
		/// 			RemainingLivesElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			int towerCount = bitstream.ReadNext (TEAMS_BITS);
			TeamLivesInfo = new List<LivesInfo>();
			for (int i = 0; i < towerCount; i++) {
				int teamNum = bitstream.ReadNext(TEAMNUM_BITS);
				int lives = bitstream.ReadNext(LIVES_BITS);
				TeamLivesInfo.Add (new LivesInfo (teamNum, lives));
			}
		}

		/// ----------------------------------------------
		/// FUNCTION:	UpdateState
		/// 
		/// DATE:		April 5th, 2019
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
			bridge.UpdateLifeCount (TeamLivesInfo);
		}

		/// ----------------------------------------------
		/// FUNCTION:	Validate
		/// 
		/// DATE:		April 5th, 2019
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
		/// 			RemainingLivesElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
			foreach (var item in TeamLivesInfo) {
				if (item.TeamNumber > TEAMNUM_MAX || item.Lives > LIVES_MAX) {
					throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
				}
			}
		}
	}
}

