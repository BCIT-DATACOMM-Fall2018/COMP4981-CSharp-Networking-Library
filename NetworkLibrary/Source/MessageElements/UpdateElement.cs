using System;

namespace NetworkLibrary.MessageElements
{
	/// ----------------------------------------------
	/// Abstract Class: MessageElement - A MessageElement that can update the game state.
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	public UpdateElement ()
	/// 				public UpdateElement (BitStream bitstream)
	/// 
	/// FUNCTIONS:	public abstract void UpdateState (IStateMessageBridge bridge);
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
	public abstract class UpdateElement : MessageElement
	{
		/// ----------------------------------------------
		/// CONSTRUCTOR: UpdateElement
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public UpdateElement ()
		/// 
		/// NOTES:		
		/// ----------------------------------------------
		public UpdateElement ()
		{
		}

		/// ----------------------------------------------
		/// CONSTRUCTOR: UpdateElement
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public UpdateElement (BitStream bitstream)
		/// 
		/// NOTES:	Calls the parent constructor to create
		/// 		the UpdateElement from a BitStream
		/// ----------------------------------------------
		public UpdateElement (BitStream bitstream) : base (bitstream)
		{
		}

		/// ----------------------------------------------
		/// FUNCTION:	UpdateState
		/// 
		/// DATE:		January 28th, 2018
		/// 
		/// REVISIONS:	
		/// 
		/// DESIGNER:	Cameron Roberts
		/// 
		/// PROGRAMMER:	Cameron Roberts
		/// 
		/// INTERFACE: 	public abstract void UpdateState (IStateMessageBridge bridge)
		/// 
		/// RETURNS: 	void.
		/// 
		/// NOTES:		A function to be implemented by concrete subclasses.
		/// 			Contains logic needed to update the game state through
		/// 			the use of a IStateMessageBridge.
		/// ----------------------------------------------
		public abstract void UpdateState (IStateMessageBridge bridge);

	}
}

