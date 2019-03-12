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
	/// DATE: 		January 28th, 2019
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

		void UpdateActorPosition (int actorId, float x, float z);

		void UpdateActorHealth (int actorId, int newHealth);

		void UseTargetedAbility (int actorId, AbilityType abilityId, int targetId);

		void UseAreaAbility (int actorId, AbilityType abilityId, float x, float z);

		void ProcessCollision(AbilityType abilityId, int actorHitId, int actorCastId);

		void SpawnActor(ActorType actorType, int ActorId, float x, float z);

		void SetActorMovement(int actorId, float x, float z, float targetX, float targetZ);

		void SetReady(int clientId, bool ready);

		void StartGame(int playerNum);

	}
}

