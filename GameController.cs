using System;
using System.Collections;
using System.Collections.Generic;

namespace Ejercicio2
{
    public class GameController
    {
        private int maxPlayerHealth = 100;
        private int maxPlayerDamage = 100;

        private int maxNumberOfEnemies = 5;
        private int minNumberOfEnemies = 3;

        private int rangedEnemyMaxAmmo = 5;
        private int rangedEnemyMinAmmo = 3;

        private int maxEnemyDamage = 4;
        private int minEnemyDamage = 1;

        private int maxEnemyHealth = 20;
        private int minEnemyHealth = 10;

        public void Execute()
        {
            Console.WriteLine("Worst Game Ever\n");

            #region PlayerCreation
            Console.WriteLine("Crea tu personaje:");

            bool validPlayerLife = false;
            int playerHealth = 0;

            while (!validPlayerLife)
            {
                Console.WriteLine("Inserte la vida de su personaje:");
                playerHealth = int.Parse(Console.ReadLine());

                if (playerHealth > maxPlayerHealth)
                {
                    validPlayerLife = false;
                    Console.WriteLine($"La vida del jugador no puede superar los {maxPlayerHealth} puntos");
                }
                else
                {
                    validPlayerLife = true;
                }
            }

            bool validPlayerDamage = false;
            int playerDamage = 0;

            while (!validPlayerDamage)
            {
                Console.WriteLine("Inserte el daño de su personaje:");
                playerDamage = int.Parse(Console.ReadLine());

                if (playerDamage > maxPlayerDamage)
                {
                    validPlayerDamage = false;
                    Console.WriteLine($"El daño del jugador no puede superar los {maxPlayerDamage} puntos");
                }
                else
                {
                    validPlayerDamage = true;
                }
            }

            Player player = new Player(playerHealth, playerDamage);

            #endregion

            #region EnemiesCreation

            Console.WriteLine("\nTe enfrentaras a:");

            List<Enemy> enemies = new List<Enemy>();

            int numberOfEnemies = GetRandomEnemyNumber();

            for (int i=0; i< numberOfEnemies; i++)
            {
                Enemy enemy = CreateEnemy();
                enemies.Add(enemy);
            }

            DisplayEnemies(enemies);

            #endregion

            bool gameEnded = false;

            while (!gameEnded)
            {
                #region PlayerTurn

                bool validEnemyIndex = false;
                int selectedEnemyIndex = 0;

                while (!validEnemyIndex)
                {
                    Console.WriteLine("\nEs tu turno, selecciona un enemigo a atacar:");
                    DisplayEnemies(enemies);

                    selectedEnemyIndex = int.Parse(Console.ReadLine());

                    if (selectedEnemyIndex > 0 && selectedEnemyIndex <= enemies.Count) validEnemyIndex = true;
                    else
                    {
                        Console.WriteLine("Enemigo seleccionado no valido, selecciona otro enemigo:");
                        DisplayEnemies(enemies);
                    }
                }

                AttackEnemyAtIndex(enemies, selectedEnemyIndex, playerDamage);
                Console.WriteLine("\nEl nuevo estado de los enemigos es:");
                DisplayEnemies(enemies);


                if (CheckEnemiesDead(enemies))
                {
                    Console.WriteLine("\nHas Ganado");
                    gameEnded = true;
                }

                #endregion

                if (!gameEnded)
                {
                    Console.WriteLine("\nPresiona cualquier tecla para que el enemigo juegue su turno");
                    Console.ReadKey();

                    #region EnemyTurn

                    int attackerIndex = SelectRandomEnemyIndex(enemies);
                    Enemy attacker = enemies[attackerIndex];

                    Console.WriteLine($"\nEl enemigo {attackerIndex + 1} atacará");

                    if (attacker.CanAttack())
                    {
                        int damage = attacker.GetDamage();
                        player.TakeDamage(damage);
                        attacker.OnAttack();
                        Console.WriteLine($"\nHas recibido {damage} puntos de daño del enemigo {attackerIndex + 1}");
                    }
                    else
                    {
                        Console.WriteLine($"No has recibido daño del enemigo {attackerIndex + 1}, no puede atacar");
                    }

                    Console.WriteLine($"Tienes {player.GetHealth()} puntos de vida");

                    if (!player.IsAlive())
                    {
                        Console.WriteLine("\nHas Perdido");
                        gameEnded = true;
                    }

                    #endregion
                }
            }

        }

        private Enemy CreateEnemy()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 2);

            if (randomNumber > 0) return CreateRangedEnemy();
            else return CreateMeleeEnemy();
        }

        private MeleeEnemy CreateMeleeEnemy() => new MeleeEnemy(GetRandomHealth(), GetRandomDamage());

        private RangedEnemy CreateRangedEnemy() => new RangedEnemy(GetRandomHealth(), GetRandomDamage(), GetRandomAmmo());

        private int GetRandomDamage() => GetRandomNumber(minEnemyDamage, maxEnemyDamage);

        private int GetRandomHealth() => GetRandomNumber(minEnemyHealth, maxEnemyHealth);

        private int GetRandomAmmo() => GetRandomNumber(rangedEnemyMinAmmo, rangedEnemyMaxAmmo);

        private int GetRandomEnemyNumber() => GetRandomNumber(minNumberOfEnemies, maxNumberOfEnemies);

        private int GetRandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max + 1);
        }

        private void DisplayEnemies(List<Enemy> enemies)
        {
            int enemyIndex = 0;

            foreach (Enemy enemy in enemies)
            {
                enemyIndex++;

                if (enemy.IsAlive())
                {
                    if (enemy is RangedEnemy)
                    {
                        Console.WriteLine($"{enemyIndex}.- Enemigo {enemy.GetEnemyType()} - Vida: {enemy.GetHealth()} - Daño: {enemy.GetDamage()} - Munición: {((RangedEnemy)enemy).GetAmmo()}");
                    }
                    else
                    {
                        Console.WriteLine($"{enemyIndex}.- Enemigo {enemy.GetEnemyType()} - Vida: {enemy.GetHealth()} - Daño: {enemy.GetDamage()}");
                    }
                }
                else Console.WriteLine($"{enemyIndex}.- x_x");

            }
        }

        private bool AttackEnemyAtIndex(List<Enemy> enemies, int indexToAttack, int playerDamage)
        {
            int enemyIndex = 0;

            foreach (Enemy enemy in enemies)
            {
                enemyIndex++;
                
                if(enemyIndex == indexToAttack)
                {
                    if (!enemy.IsAlive())
                    {
                        Console.WriteLine("El enemigo esta muerto, selecciona otro enemigo");
                        return false;
                    }
                    else
                    {
                        enemy.TakeDamage(playerDamage);
                        Console.WriteLine($"\nHas atacado al enemigo {indexToAttack} con {playerDamage} puntos de daño");
                        return true;
                    }
                }

            }

            Console.WriteLine("Enemigo no valido, selecciona otro enemigo");
            return false;
        }

        private bool CheckEnemiesDead(List<Enemy> enemies)
        {
            foreach(Enemy enemy in enemies)
            {
                if (enemy.IsAlive()) return false;
            }

            return true;
        }

        private int SelectRandomEnemyIndex(List<Enemy> enemies)
        {
            if (CheckEnemiesDead(enemies)) return 0;

            int selectedIndex = 0;
            bool validEnemy = false;

            while (!validEnemy)
            {
                selectedIndex = GetRandomNumber(0, enemies.Count -1);
                Enemy selectedEnemy = enemies[selectedIndex];

                if (selectedEnemy.IsAlive()) validEnemy = true;
            }

            return selectedIndex;     
        }
    }
}