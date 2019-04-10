using System;

namespace NetworkLibrary
{
	/// ----------------------------------------------
	/// Enum: 	ElementId - An enum to store MessageElement IDs
	///
	/// PROGRAM: NetworkLibrary
	///
	/// DATE: 		January 28th, 2019
	///
	/// REVISIONS:
	///
	/// DESIGNER: 	Cameron Roberts
	///
	/// PROGRAMMER: Cameron Roberts, Simon Wu
	///
	/// NOTES:
	/// ----------------------------------------------
	public enum AbilityType : int
	{
		// Debug abilities
		TestProjectile = 0,
		TestTargeted,
		TestTargetedHoming,
		TestAreaOfEffect,
		// Auto attack abilities
		AutoAttack,
		TowerAttack,
		// Basic abilities
		PewPew,
		Sploosh,
		Dart,
		WeebOut,
		Slash,
		// Normal abilities
		Purification,
		Blink,
		UwuImScared,
		Wall,
		BulletAbility,
		Banish,
		PorkChop,
		// Ultimate abilities
		Fireball,
		Gungnir,
		Whale

	}
}
