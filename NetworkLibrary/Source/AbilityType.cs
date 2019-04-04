﻿using System;

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
		TestProjectile = 0,
		TestTargeted,
		TestTargetedHoming,
		TestAreaOfEffect,
		AutoAttack,
		Wall,
		Banish,
		BulletAbility,
		PorkChop,
		Dart,
		Purification,
		UwuImScared,
		Fireball,
        WeebOut,
        Whale,
        TowerAttack,
		Blink,
		PewPew,
		Sploosh
	}
}
