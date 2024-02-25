using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static char[,] gameMap;
    static int playerX;
    static int playerY;
    static int mapWidth;
    static int mapHeight;
    static Random random = new Random();
    static int playerHealth = 100;
    static List<Tuple<int, int>>
        enemies = new List<Tuple<int, int>>();
    static bool map_viev = false;

    static void Main(string[] args)
    {
        LoadMap("C:\\Users\\alyak\\\\Desktop\\COLLEGE\\ПРАКТИКА\\Задача 7\\ZADANIE_7\\map.txt");
        InitializePlayer();
        InitializeEnemies();
        DrawMap();

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.Clear();

            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                    MovePlayer(playerX - 1, playerY);
                    break;
                case ConsoleKey.S:
                    MovePlayer(playerX + 1, playerY);
                    break;
                case ConsoleKey.A:
                    MovePlayer(playerX, playerY - 1);
                    break;
                case ConsoleKey.D:
                    MovePlayer(playerX, playerY + 1);
                    break;
                case ConsoleKey.E:
                    map_viev = !map_viev;
                    break;
            }

            Console.WriteLine("Нажмите на E для помощи");
            MoveEnemies();
            DrawMap();
        }
    }

    static void LoadMap(string fileName)
    {
        string[] lines = File.ReadAllLines(fileName);
        mapHeight = lines.Length;
        mapWidth = lines[0].Length;
        gameMap = new char[mapHeight, mapWidth];

        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                gameMap[i, j] = lines[i][j];
                if (lines[i][j] == 'P')
                {
                    playerX = i;
                    playerY = j;
                }
                else if (lines[i][j] == 'E')
                {
                    enemies.Add(new Tuple<int, int>(i, j));
                }
            }
        }
    }

    static void InitializePlayer()
    {
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                if (gameMap[i, j] == 'P')
                {
                    playerX = i;
                    playerY = j;
                    gameMap[i, j] = ' ';
                    return;
                }
            }
        }
    }

    static void MovePlayer(int newX, int newY)
    {
        if (newX >= 0 && newX < mapHeight && newY >= 0 &&
            newY < mapWidth && gameMap[newX, newY] != '#')
        {
            playerX = newX;
            playerY = newY;
        }
        if (gameMap[newX, newY] == 'W')
        {
            GameOver(true);
        }
        if (gameMap[newX, newY] == 'E')
        {
            playerHealth -= 20;
            if (playerHealth <= 0)
                GameOver(false);
        }
    }
    static void InitializeEnemies()
    {
        foreach (var enemy in enemies)
        {
            gameMap[enemy.Item1, enemy.Item2] = 'E';
        }
    }

    static void MoveEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            int newX = enemy.Item1 + random.Next(-1, 2);
            int newY = enemy.Item2 + random.Next(-1, 2);

            if (newX >= 0 && newX < mapHeight && newY >= 0 && 
                newY < mapWidth && gameMap[newX, newY] != '#')
            {
                gameMap[enemy.Item1, enemy.Item2] = ' ';
                gameMap[newX, newY] = 'E';
                enemies[i] = new Tuple<int, int>(newX, newY);
            }
        }
    }

    static void DrawMap()
    {
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                if (i == playerX && j == playerY)
                {
                    Console.Write('P');
                }
                else if ((gameMap[i, j] == '|') || 
                    (gameMap[i, j] == '-'))
                {
                    if (map_viev == true)
                        Console.Write(gameMap[i, j]);
                    else
                        Console.Write(' ');
                }
                else
                {
                    Console.Write(gameMap[i, j]);
                }
            }
            Console.WriteLine();
        }
        DrawHealthBar();
    }

    static void DrawHealthBar()
    {
        int healthBarLength = 10;
        int filledLength = (int)Math.Ceiling((double)playerHealth / 100 * healthBarLength);
        Console.WriteLine();
        Console.Write("[");
        for (int i = 0; i < filledLength; i++)
        {
            Console.Write("#");
        }
        for (int i = 0; i < healthBarLength - filledLength; i++)
        {
            Console.Write("_");
        }
        Console.WriteLine("]");
    }

    static void GameOver(bool over)
    {
        if (over == true)
        {
            Console.WriteLine("Вы сбежали из лабиринта!");
            Console.ReadLine();
            Environment.Exit(0);
        }
        else 
        { 
            Console.WriteLine("Вы проиграли...");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
