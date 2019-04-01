using System;

namespace NetworkLibrary
{
	public struct AbilityInfo
	{
		public static readonly AbilityInfo[] InfoArray = {
			// TestProjectile
			new AbilityInfo (isArea: true, cooldown: 30),
			// TestTargeted
			new AbilityInfo (isTargeted: true, allyTargetAllowed: true, enemyTargetAllowed: true, cooldown: 60, range:15),
			// TestHomingTargeted
			new AbilityInfo (isTargeted: true, enemyTargetAllowed: true, cooldown: 120, range:30),
			// TestAreaOfEffect
			new AbilityInfo (isArea: true, cooldown: 180, range:30),
			// AutoAttack
			new AbilityInfo (isTargeted: true, enemyTargetAllowed: true, cooldown: 30, range: 15),
			// Wall
			new AbilityInfo (isArea: true, cooldown:10, range: 30),
			// Banish
			new AbilityInfo (isTargeted: true, allyTargetAllowed: true, requiresCollision: false, enemyTargetAllowed: true, cooldown: 10, range: 40),
			// Bullet Ability
			new AbilityInfo (isTargeted: true, enemyTargetAllowed: true, cooldown: 50, range: 50),
			// Pork Chop
			new AbilityInfo (isTargeted: true, enemyTargetAllowed: true, cooldown: 105, range: 20),
			// Dart
			new AbilityInfo (isArea: true, enemyTargetAllowed: true, cooldown: 150, range: 40),
			// Purification
			new AbilityInfo (isTargeted: true, allyTargetAllowed: true, isSelf: true, cooldown: 150, range: 40),
			// UwuImScared
			new AbilityInfo (isSelf: true, allyTargetAllowed: true, requiresCollision: false, cooldown: 180),
			// Fireball
			new AbilityInfo (isArea: true, enemyTargetAllowed: true, cooldown: 180, range: 80)
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

		public int Range { get; private set; }

		private AbilityInfo (bool isArea = false, bool isTargeted = false, bool isSelf = false, bool allyTargetAllowed = false, bool enemyTargetAllowed = false, bool requiresCollision = true, int cooldown = 0, int range = 0)
		{
			IsArea = isArea;
			IsTargeted = isTargeted;
			IsSelf = isSelf;
			AllyTargetAllowed = allyTargetAllowed;
			EnemyTargetAllowed = enemyTargetAllowed;
			RequiresCollision = requiresCollision;
			Cooldown = cooldown;
			Range = range;

		}


	}
}
