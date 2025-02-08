using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeongame_Butaeva
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            string[] dungeonMap = new string[10];
            Random rand = new Random();   // Карта подземелья        
            for (int i = 0; i < dungeonMap.Length - 1; i++) // Заполнение карты подземелья случайными событиями
            {
                int eventType = rand.Next(0, 5); // 0-4: монстр, ловушка, сундук, торговец, пустая комната
                switch (eventType)
                {
                    case 0:
                        dungeonMap[i] = "Monster";
                        break;
                    case 1:
                        dungeonMap[i] = "Trap";
                        break;
                    case 2:
                        dungeonMap[i] = "Chest";
                        break;
                    case 3:
                        dungeonMap[i] = "Merchant";
                        break;
                    default:
                        dungeonMap[i] = "Empty Room";
                        break;
                }
            }
            dungeonMap[9] = "Boss"; // Босс в последней комнате
            for (int i = 0; i < dungeonMap.Length; i++)
            {
                Console.WriteLine($"Вы входите в комнату {i + 1}: {dungeonMap[i]}");

                switch (dungeonMap[i])
                {
                    case "Monster":
                        FightWithMonster(player);
                        break;
                    case "Trap":
                        EncounterTrap(player);
                        break;
                    case "Chest":
                        OpenChest(player);
                        break;
                    case "Merchant":
                        VisitMerchant(player);
                        break;
                    case "Empty Room":
                        Console.WriteLine("В этой комнате ничего нет.");
                        break;
                    case "Boss":
                        FightWithBoss(player);
                        break;
                }
                if (player.Health <= 0)
                {
                    Console.WriteLine("Вы погибли. Игра окончена.");
                    return;
                }
            }
            Console.WriteLine("Поздравляем! Вы победили в игре!"); // Игровой цикл
        }
        static void FightWithMonster(Player player)
        {
            Random rand = new Random();
            int monsterHealth = rand.Next(20, 51); // Здоровье монстра от 20 до 50
            Console.WriteLine($"Вы встретили монстра с {monsterHealth} HP!");

            while (monsterHealth > 0 && player.Health > 0)
            {
                Console.WriteLine("Выберите оружие: 1 - Меч, 2 - Лук");
                int choice = int.Parse(Console.ReadLine());

                int damage = 0;
                if (choice == 1) // Меч
                {
                    damage = rand.Next(10, 21);
                }
                else if (choice == 2 && player.Arrows > 0) // Лук
                {
                    damage = rand.Next(5, 16);
                    player.Arrows--;
                }
                else if (choice == 2 && player.Arrows <= 0)
                {
                    Console.WriteLine("У вас нет стрел!");
                    continue; 
                }

                monsterHealth -= damage;
                Console.WriteLine($"Вы нанесли {damage} урона монстру. У него осталось {monsterHealth} HP.");

                if (monsterHealth > 0)
                {
                    int monsterDamage = rand.Next(5, 16);
                    player.Health -= monsterDamage;
                    Console.WriteLine($"Монстр атакует вас и наносит {monsterDamage} урона. У вас осталось {player.Health} HP.");
                }
            }

            if (monsterHealth <= 0)
            {
                Console.WriteLine("Вы победили монстра!");
            }
        }

        static void EncounterTrap(Player player)
        {
            Random rand = new Random();
            int damage = rand.Next(10, 21);
            player.Health -= damage;
            Console.WriteLine($"Вы попали в ловушку и потеряли {damage} HP. У вас осталось {player.Health} HP.");
        }

        static void OpenChest(Player player)
        {
            Random rand = new Random();
            int answer, correctAnswer;

            // Генерация загадки
            int a = rand.Next(1, 10);
            int b = rand.Next(1, 10);
            correctAnswer = a + b;
            Console.WriteLine($"Для того,чтобы открыть сундук, решите: {a} + {b} = ?");
            while (true)
            {
                answer = int.Parse(Console.ReadLine());
                if (answer == correctAnswer)
                {
                    int lootType = rand.Next(0, 3); // 0 - зелье, 1 - золото, 2 - стрелы
                    switch (lootType)
                    {
                        case 0:
                            if (player.InventoryCount < 5)
                            {
                                player.Inventory[player.InventoryCount++] = "Potion";
                                Console.WriteLine("УРААА,Вы нашли зелье!");
                            }
                            else
                            {
                                Console.WriteLine("Ваш инвентарь полон!");
                            }
                            break;
                        case 1:
                            player.Gold += 10;
                            Console.WriteLine("ПОЗДРАВЛЯЮ,Вы нашли золото!");
                            break;
                        case 2:
                            player.Arrows += 3;
                            Console.WriteLine("ПРЕКРАСНО,Вы нашли стрелы!");
                            break;
                    }
                    break; // Выход из цикла после успешного ответа
                }
                else
                {
                    Console.WriteLine("Ответ неверный. Попробуйте снова.");
                }
            }
        }
        static void VisitMerchant(Player player)
        {
            Console.WriteLine("Торговец предлагает купить зелье за 30 золота.");
            if (player.Gold >= 30)
            {
                player.Gold -= 30;
                if (player.InventoryCount < 5)
                {
                    player.Inventory[player.InventoryCount++] = "Potion";
                    Console.WriteLine("Вы купили зелье.");
                }
                else
                {
                    Console.WriteLine("Ваш инвентарь полон!");
                }
            }
            else
            {
                Console.WriteLine("У вас недостаточно золота.");
            }
        }
        static void FightWithBoss(Player player)
        {
            Random rand = new Random();
            int bossHealth = 100; // Здоровье босса
            Console.WriteLine("Вы встретили босса!");

            while (bossHealth > 0 && player.Health > 0)
            {
                Console.WriteLine("Выберите оружие: 1 - Меч, 2 - Лук");
                int choice = int.Parse(Console.ReadLine());

                int damage = 0;
                if (choice == 1) // Меч
                {
                    damage = rand.Next(10, 21);
                }
                else if (choice == 2 && player.Arrows > 0) // Лук
                {
                    damage = rand.Next(5, 16);
                    player.Arrows--;
                }
                else if (choice == 2 && player.Arrows <= 0)
                {
                    Console.WriteLine("У вас нет стрел!");
                    continue;
                }
                bossHealth -= damage;
                Console.WriteLine($"Вы нанесли {damage} урона боссу. У него осталось {bossHealth} HP.");
                if (bossHealth > 0)
                {
                    int bossDamage = rand.Next(10, 21);
                    player.Health -= bossDamage;
                    Console.WriteLine($"Босс атакует вас и наносит {bossDamage} урона. У вас осталось {player.Health} HP.");
                }
            }
            if (bossHealth <= 0)
            {
                Console.WriteLine("Вы победили босса! Поздравляем! УРААА") ;
            }
        }
    }
    class Player
    {
        public int Health { get; set; } = 100; // Здоровье игрока
        public int Gold { get; set; } = 0; // Золото игрока
        public int Arrows { get; set; } = 5; // Количество стрел
        public string[] Inventory { get; set; } = new string[5]; // Инвентарь игрока
        public int InventoryCount { get; set; } = 0; // Количество предметов в инвентаре
        public Player() { } // Хранит характеристики игрока: здоровье,золото,кол-во стрел, инвентарь и кол-во предметов в инвентаре
    }
}

