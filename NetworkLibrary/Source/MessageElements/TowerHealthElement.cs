using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: TowerHealthElement - A UpdateElement used to update tower health
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public TowerHealthElement (List<PlayerInfo> playerInformation)
	/// 				public TowerHealthElement (BitStream bitstream)
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
	public class TowerHealthElement : UpdateElement
	{

		public struct TowerInfo
		{
			public int ActorId { get; private set;}
			public int Health { get; private set;}

			public TowerInfo (int actorId, int health)
			{
				this.ActorId = actorId;
				this.Health = health;
			}
		}

		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.TowerHealthElement);

		private const int TOWERS_MAX = 31;
		private const int HEALTH_MAX = 1000;
		private const int ACTORID_MAX = 127;
		private static readonly int TOWERS_BITS = RequiredBits (TOWERS_MAX);
		private static readonly int HEALTH_BITS = RequiredBits (HEALTH_MAX);
		private static readonly int ACTORID_BITS = RequiredBits (ACTORID_MAX);

		public List<TowerInfo> TowerInformation { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: TowerHealthElement
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public TowerHealthElement (List<TowerInfo> towerInformation)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public TowerHealthElement (List<TowerInfo> towerInformation)
		{
			TowerInformation = new List<TowerInfo>();
			foreach (var item in towerInformation) {
				TowerInformation.Add(new TowerInfo(item.ActorId, item.Health));
			}
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: TowerHealthElement
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
		/// 		TowerHealthElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public TowerHealthElement (BitStream bitstream) : base (bitstream)
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
		/// RETURNS: 	An ElementIndicatorElement appropriate for a TowerHealthElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a TowerHealthElement when 
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
		/// 			TowerHealthElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a TowerHealthElement
		/// ----------------------------------------------
		public override int Bits(){
			int bits = 0;
			bits += TOWERS_BITS;
			bits += TowerInformation.Count * (HEALTH_BITS + ACTORID_BITS);
			return bits;
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
		/// 			TowerHealthElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write (TowerInformation.Count, 0, TOWERS_BITS);
			foreach (var item in TowerInformation) {
				bitStream.Write (item.ActorId, 0, ACTORID_BITS);
				bitStream.Write (item.Health, 0, HEALTH_BITS);
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
		/// 			TowerHealthElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			int towerCount = bitstream.ReadNext (TOWERS_BITS);
			TowerInformation = new List<TowerInfo>();
			for (int i = 0; i < towerCount; i++) {
				int actorId = bitstream.ReadNext(ACTORID_BITS);
				int health = bitstream.ReadNext(HEALTH_BITS);
				TowerInformation.Add (new TowerInfo (actorId, health));
			}
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
			foreach(var tower in TowerInformation){
				bridge.UpdateActorHealth (tower.ActorId, tower.Health);
			}
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
		/// 			TowerHealthElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
			foreach (var item in TowerInformation) {
				if (item.ActorId > ACTORID_MAX || item.Health > HEALTH_MAX) {
					throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
				}
			}
		}
	}
}

