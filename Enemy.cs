using System;

public abstract class Enemy: Entity
{
	public Enemy(int health, int damage) : base(health, damage) { }

	public virtual bool CanAttack() => true;

	public virtual string GetEnemyType() => "";
}
