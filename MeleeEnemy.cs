using System;

public class MeleeEnemy : Enemy
{
	public MeleeEnemy(int health, int damage) : base(health, damage) { }
	public override string GetEnemyType() => "Melee";

}
