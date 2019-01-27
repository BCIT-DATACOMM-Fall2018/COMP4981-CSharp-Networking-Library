using System;

namespace NetworkLibrary
{
	public interface IStateMessageBridge
	{

		void UpdateActorPosition (int actorId, double x, double y);

		void UpdateActorHealth (int actorId, int newHealth);

		void UseActorAbility (int actorId, int abilityId, int targetId, int x, int y);

		void CreateHealthMessage();

	}
}

