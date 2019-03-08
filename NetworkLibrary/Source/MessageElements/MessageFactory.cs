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
	/// DATE: 		January 28th, 2018
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
		/// DATE:		January 28th, 2018
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
		/// DATE:		January 28th, 2018
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
			default:
				throw new InvalidOperationException ();
			}
		}

		/// ----------------------------------------------
		/// FUNCTION:	CreateMessageElement
		/// 
		/// DATE:		January 28th, 2018
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
			default:
				throw new InvalidOperationException ();
			}
		}
	}
}

