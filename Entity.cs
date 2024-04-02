using System;

public abstract class Entity
{
	public int health;
	public int damage;

	public Entity(int health, int damage)
	{
		this.health = health;
		this.damage = damage;
	}
	public void TakeDamage(int damage)
	{
		health = damage > health ? 0 : health - damage;
	}

	public int GetDamage() => damage;

	public int GetHealth() => health;

	public bool IsAlive() => health > 0;

	public virtual void OnAttack() { }
}