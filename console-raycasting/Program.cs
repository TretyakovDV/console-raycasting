using System;

namespace console_raycasting
{
    class Program
    {
        private static string map = "";

        private static double cameraX = 2;
        private static double cameraY = 2;
        private static double cameraAngle = 0;

        private const double fov = Math.PI / 3;

        private const int screenW = 150;
        private const int screenH = 40;

        private const int mapW = 24;
        private const int mapH = 12;

        private const double cameraSpeed = 15d;
        private const double raySpeed = 0.1d;

        private static char[] buffer = new char[screenW * screenH];

        private static double deltaTime = 1;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(screenW, screenH);
            Console.SetBufferSize(screenW, screenH);

            map += "########################";
            map += "#......................#";
            map += "#......................#";
            map += "#......................#";
            map += "#......................#";
            map += "#......................#";
            map += "#......................#";
            map += "#......########........#";
            map += "#.............#........#";
            map += "#.............#........#";
            map += "#.............#........#";
            map += "########################";

            DateTime currentDate = DateTime.Now;

            while (true)
            {
                DateTime newDate = DateTime.Now;
                deltaTime = (currentDate - newDate).TotalSeconds;
                currentDate = DateTime.Now;

                Input();

                double[] distances = new double[screenW];

                for(int x = 0; x < screenW; x++)
                {
                    double rayDirection = cameraAngle + fov / 2 - x * fov / screenW;
                    double rayX = Math.Sin(rayDirection);
                    double rayY = Math.Cos(rayDirection);

                    double distance = 0;
                    bool hit = false;
                    double depth = 30d;

                    while(!hit && distance < depth)
                    {
                        distance += raySpeed;

                        int targetX = (int)(cameraX + rayX * distance);
                        int targetY = (int)(cameraY + rayY * distance);

                        if(targetX < 0 || targetX >= depth + cameraX || targetY < 0 || targetY >= depth + cameraY)
                        {
                            hit = true;
                            distance = depth;
                        }
                        else
                        {
                            if(map[targetX * mapW + targetY] == '#')
                            {
                                hit = true;
                            } 
                        }
                    }

                    distances[x] = distance;

                    int wall = (int)(screenH / 2d - screenH * fov / distance);
                    int floor = screenH - wall;

                    for(int y= 0; y < screenH; y++)
                    {
                        if(y <= wall)
                        {
                            buffer[y * screenW + x] = ' ';
                        }
                        else if (y > wall && y <= floor)
                        {
                            char wallColor = ' ';

                            if (distance <= depth / 4d)
                                wallColor = '▓';
                            else if (distance <= depth / 3d)
                                wallColor = '▒';
                            else if (distance <= depth / 2d)
                                wallColor = '░';

                            buffer[y * screenW + x] = wallColor;
                        }
                        else
                        {
                            buffer[y * screenW + x] = '.';
                        }
                    }
                }

                Console.SetCursorPosition(0, 0);
                Console.Write(buffer);
            }
        }

        private static void Input()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.A:
                        cameraAngle -= cameraSpeed * deltaTime;
                        break;
                    case ConsoleKey.D:
                        cameraAngle += cameraSpeed * deltaTime;
                        break;
                    case ConsoleKey.W:
                        cameraX -= Math.Sin(cameraAngle) * cameraSpeed * deltaTime;
                        cameraY -= Math.Cos(cameraAngle) * cameraSpeed * deltaTime;
                        if (map[(int)cameraX * mapW + (int)cameraY] == '#')
                        {
                            cameraX += Math.Sin(cameraAngle) * cameraSpeed * deltaTime;
                            cameraY += Math.Cos(cameraAngle) * cameraSpeed * deltaTime;
                        }
                        break;
                    case ConsoleKey.S:
                        cameraX += Math.Sin(cameraAngle) * cameraSpeed * deltaTime;
                        cameraY += Math.Cos(cameraAngle) * cameraSpeed * deltaTime;
                        if (map[(int)cameraX * mapW + (int)cameraY] == '#')
                        {
                            cameraX -= Math.Sin(cameraAngle) * cameraSpeed * deltaTime;
                            cameraY -= Math.Cos(cameraAngle) * cameraSpeed * deltaTime;
                        }
                        break;
                }
            }
        }
    }
}
