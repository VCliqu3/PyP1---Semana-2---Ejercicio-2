using System;

public class RangedEnemy: Enemy
{
	public int maxAmmo;
	private int ammo;

	public RangedEnemy(int health, int damage, int maxAmmo) : base(health, damage) 
	{
		this.maxAmmo = maxAmmo;
		ammo = maxAmmo;
	}
	public override string GetEnemyType() => "Rango";
	public override bool CanAttack() => ammo > 0;

	public int GetAmmo() => ammo;
	public void DecreaseAmmo() => ammo--;
	public override void OnAttack()
    {
		DecreaseAmmo();
    }
}
