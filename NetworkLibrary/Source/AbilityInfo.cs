using System;

namespace NetworkLibrary
{
	public struct AbilityInfo
	{
		public static readonly AbilityInfo[] InfoArray = {
			// TestProjectile
			new AbilityInfo (isArea: true),
			// TestTargeted
			new AbilityInfo (isTargeted: true, allyTargetAllowed: true, enemyTargetAllowed: true),
			// TestHomingTargeted
			new AbilityInfo (isTargeted: true, enemyTargetAllowed: true),
			// TestAreaOfEffect
			new AbilityInfo (isArea: true)
		};

		public bool IsArea { get; private set; }

		public bool IsTargeted{ get; private set; }

		public bool IsSelf{ get; private set; }

		public bool AllyTargetAllowed{ get; private set; }

		public bool EnemyTargetAllowed{ get; private set; }

		private AbilityInfo (bool isArea = false, bool isTargeted = false, bool isSelf = false, bool allyTargetAllowed = false, bool enemyTargetAllowed = false)
		{
			IsArea = isArea;
			IsTargeted = isTargeted;
			IsSelf = isSelf;
			AllyTargetAllowed = allyTargetAllowed;
			EnemyTargetAllowed = enemyTargetAllowed;
		}


	}
}

