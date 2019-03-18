using System;
using NetworkLibrary.MessageElements;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Class: 	MessageFactory - A class to create MessageElements from a given ElementId.
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public MessageFactory ()
	/// 
	/// FUNCTIONS:	public MessageElement CreateMessageElement (ElementId id, BitStream bitStream)
	///				public UpdateElement CreateUpdateElement (ElementId id, BitStream bitStream)
	/// 
	/// DATE: 		January 28th, 2019
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		
	/// ----------------------------------------------
	public class MessageFactory
	{

		/// ----------------------------------------------
		/// CONSTRUCTOR: Bitstream
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public MessageFactory ()
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public MessageFactory ()
		{
		}

		/// ----------------------------------------------
		/// FUNCTION:	CreateMessageElement
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public MessageElement CreateMessageElement (ElementId id, BitStream bitStream)
		/// 				ElementId id: The ElementId corresponding to the kind of element to create.
		/// 			 	BitStream bitStream: The BitStream to create the element from.
		/// 
		/// RETURNS: 	A new MessageElement created from the BitStream of the type specified
		/// 			by the given ElementId.
		/// 
		/// NOTES:		When creating a MessageElement that will be treated as an
		/// 			UpdateElement call CreateUpdateElement rather that casting 
		/// 			the result of this method.
		/// ----------------------------------------------
		public MessageElement CreateMessageElement (ElementId id, BitStream bitStream)
		{
			switch (id) {
			case ElementId.PacketHeaderElement:
				return new PacketHeaderElement (bitStream);
			case ElementId.HealthElement:
				return new HealthElement (bitStream);
			case ElementId.ClientIDElement:
				return new ClientIDElement (bitStream);
			case ElementId.PositionElement:
				return new PositionElement (bitStream);
			case ElementId.CollisionElement:
				return new CollisionElement (bitStream);
			case ElementId.TargetedAbilityElement:
				return new TargetedAbilityElement (bitStream);
			case ElementId.AreaAbilityElement:
				return new AreaAbilityElement (bitStream);
			case ElementId.SpawnElement:
				return new SpawnElement (bitStream);
			case ElementId.MovementElement:
				return new PositionElement (bitStream);
			case ElementId.ReadyElement:
				return new ReadyElement (bitStream);
			case ElementId.GameStartElement:
				return new GameStartElement (bitStream);
			case ElementId.LobbyStatusElement:
				return new LobbyStatusElement (bitStream);
			default:
				throw new InvalidOperationException ("Attempted to create a MessageElement with an invalid ID");
			}
		}

		/// ----------------------------------------------
		/// FUNCTION:	CreateMessageElement
		/// 
		/// DATE:		January 28th, 2019
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public UpdateElement CreateUpdateElement (ElementId id, BitStream bitStream)
		/// 				ElementId id: The ElementId corresponding to the kind of element to create.
		/// 			 	BitStream bitStream: The BitStream to create the element from.
		/// 
		/// RETURNS: 	A new UpdateElement created from the BitStream of the type specified
		/// 			by the given ElementId.
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public UpdateElement CreateUpdateElement (ElementId id, BitStream bitStream)
		{
			switch (id) {
			case ElementId.HealthElement:
				return new HealthElement (bitStream);
			case ElementId.PositionElement:
				return new PositionElement (bitStream);
			case ElementId.CollisionElement:
				return new CollisionElement (bitStream);
			case ElementId.TargetedAbilityElement:
				return new TargetedAbilityElement (bitStream);
			case ElementId.AreaAbilityElement:
				return new AreaAbilityElement (bitStream);
			case ElementId.SpawnElement:
				return new SpawnElement (bitStream);
			case ElementId.MovementElement:
				return new MovementElement (bitStream);
			case ElementId.ReadyElement:
				return new ReadyElement (bitStream);
			case ElementId.GameStartElement:
				return new GameStartElement (bitStream);
			case ElementId.LobbyStatusElement:
				return new LobbyStatusElement (bitStream);
			default:
				throw new InvalidOperationException ("Attempted to create a UpdateElement with an invalid ID: " + id);
			}
		}
	}
}

