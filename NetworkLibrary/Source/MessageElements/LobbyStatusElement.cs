using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: LobbyStatusElement - A UpdateElement to cause an actor to be spawned
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public LobbyStatusElement (List<PlayerInfo> playerInformation)
	/// 				public LobbyStatusElement (BitStream bitstream)
	/// 
	/// FUNCTIONS:	public override ElementIndicatorElement GetIndicator ()
	///				protected override void Serialize (BitStream bitStream)
	/// 			protected override void Deserialize (BitStream bitstream)
	/// 			public override void UpdateState (IStateMessageBridge bridge)
	/// 			protected override void Validate ()
	/// 
	/// DATE: 		March 17th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		
	/// ----------------------------------------------
	public class LobbyStatusElement : UpdateElement
	{

		public struct PlayerInfo
		{
			public  int Id { get; private set;}
			public string Name { get; private set;}
			public int Team { get; set;}
			public bool ReadyStatus { get; set;}

			public PlayerInfo (int id, string name, int team, bool readyStatus)
			{
				this.Id = id;
				this.Name = name;
				this.Team = team;
				this.ReadyStatus = readyStatus;
			}
		}

		private static readonly ElementIndicatorElement INDICATOR = new ElementIndicatorElement (ElementId.LobbyStatusElement);

		private const int PLAYERS_MAX = 31;
		private const int PLAYERID_MAX = 31;
		private const int PLAYERTEAM_MAX = 7;
		private const int PLAYERREADY_MAX = 1;
		private const int NAMECHARS_MAX = 32;

		private static readonly int PLAYERS_BITS = RequiredBits (PLAYERS_MAX);
		private static readonly int PLAYERID_BITS = RequiredBits (PLAYERID_MAX);
		private static readonly int PLAYERTEAM_BITS = RequiredBits (PLAYERTEAM_MAX);
		private static readonly int PLAYERREADY_BITS = RequiredBits (PLAYERREADY_MAX);
		private static readonly int NAMECHARS_BITS = RequiredBits (NAMECHARS_MAX);

		public List<PlayerInfo> PlayerInformation { get; private set; }

		/// ----------------------------------------------
		/// CONSTRUCTOR: LobbyStatusElement
		/// 
		/// DATE:		March 8th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public LobbyStatusElement (List<PlayerInfo> playerInformation)
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public LobbyStatusElement (List<PlayerInfo> playerInformation)
		{
			PlayerInformation = new List<PlayerInfo>();
			foreach (var item in playerInformation) {
				PlayerInformation.Add(new PlayerInfo(item.Id, item.Name, item.Team, item.ReadyStatus));
			}
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: LobbyStatusElement
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
		/// 		LobbyStatusElement by deserializing it 
		/// 		from a BitStream object.
		/// ----------------------------------------------
		public LobbyStatusElement (BitStream bitstream) : base (bitstream)
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
		/// RETURNS: 	An ElementIndicatorElement appropriate for a LobbyStatusElement
		/// 
		/// NOTES:		Returns an ElementIndicatorElement to be used 
		/// 			to reconstruct a LobbyStatusElement when 
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
		/// 			LobbyStatusElement
		/// 
		/// NOTES:		Returns the number of bits needed to store
		/// 			a LobbyStatusElement
		/// ----------------------------------------------
		public override int Bits(){
			int bits = 0;
			bits += PLAYERS_BITS;
			foreach (var player in PlayerInformation) {
				bits += PLAYERID_BITS + PLAYERTEAM_BITS + PLAYERREADY_BITS + NAMECHARS_BITS;
				byte[] name = Encoding.UTF8.GetBytes (player.Name);
				if (name.Length > NAMECHARS_MAX) {
					throw new System.Runtime.Serialization.SerializationException ("Attempt to create a lobby status packet with a name greater than 32 characters");
				}
				bits += name.Length * BitStream.BYTE_SIZE;
			}
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
		/// 			LobbyStatusElement to a BitStream.
		/// ----------------------------------------------
		protected override void Serialize (BitStream bitStream)
		{
			bitStream.Write (PlayerInformation.Count, 0, PLAYERS_BITS);
			foreach (var item in PlayerInformation) {
				bitStream.Write (item.Id, 0, PLAYERID_BITS);
				bitStream.Write (item.Team, 0, PLAYERTEAM_BITS);
				bitStream.Write (item.ReadyStatus ? 1 : 0, 0, PLAYERREADY_BITS);
				byte[] name = Encoding.UTF8.GetBytes (item.Name);
				if (name.Length > NAMECHARS_MAX) {
					throw new System.Runtime.Serialization.SerializationException ("Attempt to create a lobby status packet with a name greater than 32 characters");
				}
				bitStream.Write (name.Length, 0 , NAMECHARS_BITS);
				foreach (var character in name) {
					bitStream.Write (character, 0, BitStream.BYTE_SIZE);
				}
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
		/// 			LobbyStatusElement from a BitStream.
		/// ----------------------------------------------
		protected override void Deserialize (BitStream bitstream)
		{
			int playerCount = bitstream.ReadNext (PLAYERS_BITS);
			PlayerInformation = new List<PlayerInfo>();
			for (int i = 0; i < playerCount; i++) {
				int id = bitstream.ReadNext(PLAYERID_BITS);
				int team = bitstream.ReadNext(PLAYERTEAM_BITS);
				bool ready = bitstream.ReadNext (PLAYERREADY_BITS) == 1 ? true : false;
				int nameBytes = bitstream.ReadNext(NAMECHARS_BITS);
				byte[] name = new byte[nameBytes];
				for (int j = 0; j < nameBytes; j++) {
					name[j] = bitstream.ReadNextByte (BitStream.BYTE_SIZE);
				}
				PlayerInformation.Add(new PlayerInfo(id, Encoding.UTF8.GetString(name), team, ready));
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
			bridge.SetLobbyStatus (PlayerInformation);
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
		/// 			LobbyStatusElement.
		/// ----------------------------------------------
		protected override void Validate ()
		{
			foreach (var item in PlayerInformation) {
				if (item.Id > PLAYERID_MAX || item.Team > PLAYERTEAM_MAX) {
					throw new System.Runtime.Serialization.SerializationException ("Attempt to deserialize invalid packet data");
				}
			}

		}
	}
}

