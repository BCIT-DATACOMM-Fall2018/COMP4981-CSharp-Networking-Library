using System;

namespace NetworkLibrary
{
	/// ----------------------------------------------
	/// Interface: 	IStateMessageBridge - An interface to allow message elements
	/// 								  to update the game state.
	/// 
	/// PROGRAM: NetworkLibrary
	///
	/// CONSTRUCTORS:	None
	/// 
	/// FUNCTIONS:	TBD
	/// 
	/// DATE: 		January 28th, 2018
	///
	/// REVISIONS: 
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts
	///
	/// NOTES:		The purpose of this interface is to allow messages to be able
	/// 			to update the game state without knowledge of how the state
	/// 			is implemented and stored so they can be used on both the client
	/// 			and server.
	/// 
	/// 			The functions in the interface are placeholders and will change
	/// 			in the future.
	/// ----------------------------------------------
	public interface IStateMessageBridge
	{

		void UpdateActorPosition (int actorId, double x, double y);

		void UpdateActorHealth (int actorId, int newHealth);

		void UseActorAbility (int actorId, int abilityId, int targetId, int x, int y);

		void CreateHealthMessage ();

	}
}

