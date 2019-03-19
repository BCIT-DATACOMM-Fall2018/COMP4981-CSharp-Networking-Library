﻿using System;

namespace NetworkLibrary
{
	public struct AbilityInfo
	{
		public static readonly AbilityInfo[] InfoArray = {
			// TestProjectile
			new AbilityInfo (isArea: true, requiresCollision: true, cooldown: 30),
			// TestTargeted
			new AbilityInfo (isTargeted: true, allyTargetAllowed: true, enemyTargetAllowed: true, cooldown: 60),
			// TestHomingTargeted
			new AbilityInfo (isTargeted: true, enemyTargetAllowed: true, requiresCollision: true, cooldown: 120),
			// TestAreaOfEffect
			new AbilityInfo (isArea: true, requiresCollision: true, cooldown: 180)
		};

		// The ability targets a location on the map
		public bool IsArea { get; private set; }

		// The ability targets an actor
		public bool IsTargeted{ get; private set; }

		// The ability targets only the player using it
		public bool IsSelf{ get; private set; }

		// The ability is allowed to target allies (includes player)
		public bool AllyTargetAllowed{ get; private set; }

		// The ability is allowed to target enemies
		public bool EnemyTargetAllowed{ get; private set; }

		// The ability requires client calculation of collision. If this is false the server will
		// apply the abilities effects immediatly upon receiving the packet.
		public bool RequiresCollision{ get; private set; }

		public int Cooldown { get; private set; }

		private AbilityInfo (bool isArea = false, bool isTargeted = false, bool isSelf = false, bool allyTargetAllowed = false, bool enemyTargetAllowed = false, bool requiresCollision = false, int cooldown = 0)
		{
			IsArea = isArea;
			IsTargeted = isTargeted;
			IsSelf = isSelf;
			AllyTargetAllowed = allyTargetAllowed;
			EnemyTargetAllowed = enemyTargetAllowed;
			RequiresCollision = requiresCollision;
			Cooldown = cooldown;
		}


	}
}
