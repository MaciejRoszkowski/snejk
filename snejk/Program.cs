using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace snejk
{


    class Pos
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Pos(int x,int y)
        {
            X = x;
            Y = y;
        }
    }

    class Program
    {
        static int selection = 0;
        static char[,] grid = new char[20,50];
        static int width = 50;
        static int height = 20;
        static bool gameover = false;
        static int snake_x = 25;
        static int snake_y = 9;
        static List<Pos> snake = new List<Pos>();
        static int time = 200;
        static int multi = 1;
        static SoundPlayer player = new SoundPlayer();

        enum direction
        {
            UP=1,
            DOWN=2,
            LEFT =3,
            RIGHT=4
        };
        static direction current = direction.RIGHT;
        static int snakeLength = 5;
        static int lengthTime = 8;
        static int lengthTimer = 0;
        static int target_x;
        static int target_y;
        static int score = 0;
        static bool gameStart = false;

        public static void Main(string[] args)
        {
            //gameover = false;
            //snake = new List<Pos>();
            //snake_y = 9;
            //snake_x = 25;
            Console.WindowHeight = height+5;
            Console.WindowWidth = width+2;
            Menu();
            InitFrame();
            DrawFrame();
            InitSnake();
            DrawSnake();
            SetTarget();
            InitScore();





            while(!gameover)
            {
                DrawSnakeHead();
                if(TargetTaken())
                {
                    SetTarget();
                    snakeLength++;

                    score +=  multi;
                    UpdateScore();

                    Console.Beep();

                }
                Pause();
                ReadKeys();
                DrawSnakeBodyOnHead();
                MoveSnakeHead();

                if (IsGameover())
                    gameover = true;
                IncreaseSnakeLength();
                DeleteSnakeTail();
            }
            DrawSnakeHead();

            Console.SetCursorPosition(40, 20);
            Console.WriteLine("game over");
            //player.SoundLocation = @"c:\sound\box.wav";
            player.SoundLocation = @"c:\sound\crash.wav";

            player.Play();


            while (true)
            {
                ConsoleKeyInfo s;
                s =Console.ReadKey();
                if(s.Key == ConsoleKey.Escape)
                {
                    break;
                }

            }
            //Main(args);
        
        }
        static void DrawMenu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;

            Console.WriteLine("╔══════════════════════════════════════════════════╗");
            Console.WriteLine("║--------------------------------------------------║");
            Console.WriteLine("║--------------------- Snake ----------------------║");
            Console.WriteLine("║--------------------------------------------------║");
            Console.WriteLine("╚══════════════════════════════════════════════════╝");
            Console.WriteLine("odpal gierke");
            Console.WriteLine("ranking");
            Console.WriteLine("hard mode");
            Console.WriteLine("wyjdz");


        }
        static void UpdateMenu(int prev)
        {
            if(selection==0)
            {
                Console.SetCursorPosition(0, 5);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("odpal gierke");
            }
            else if (selection == 1)
            {
                Console.SetCursorPosition(0, 6);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("ranking");
            }
            else if (selection == 2)
            {
                Console.SetCursorPosition(0, 7);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("hard mode");
            }
            else if (selection == 3)
            {
                Console.SetCursorPosition(0, 8);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("wyjdz");
            }
            if(prev==0)
            {
                Console.SetCursorPosition(0, 5);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("odpal gierke");
            }
            else if (prev == 1)
            {
                Console.SetCursorPosition(0, 6);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("ranking");
            }
            else if (prev == 2)
            {
                Console.SetCursorPosition(0, 7);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("hard mode");
            }
            else if (prev == 3)
            {
                Console.SetCursorPosition(0, 8);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("wyjdz");

            }
        }

        static void Menu()
        {
            DrawMenu();
            UpdateMenu(-1);
            int prev = -1;
            while(!gameStart)
            {
                ConsoleKeyInfo s;
                s = Console.ReadKey();
                if (s.Key == ConsoleKey.DownArrow)
                {
                    Console.SetCursorPosition(10, 8);

                    if (selection<3)
                    {
                        prev = selection;
                        selection++;
                        UpdateMenu(prev);
                        Console.SetCursorPosition(10, 8);

                    }

                }
                if (s.Key == ConsoleKey.UpArrow)
                {
                    Console.SetCursorPosition(10, 8);

                    if (selection >0)
                    {
                        prev = selection;
                        selection--;
                        UpdateMenu(prev);
                        Console.SetCursorPosition(10, 8);

                    }
                }
                if (s.Key==ConsoleKey.Enter)
                {
                    switch (selection)
                    {
                        case 0:
                            gameStart = true;
                            break;
                        case 1:
                            Ranking(prev);

                            break;
                        case 2:
                            time =50;
                            multi = 100;
                            gameStart = true;

                            break;
                        case 3:
                            Environment.Exit(0);
                            break;

                    }

                }

            }

        }
        static void Ranking(int prev)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════╗");
            Console.WriteLine("║--------------------------------------------------║");
            Console.WriteLine("║-------------------- Ranking  --------------------║");
            Console.WriteLine("║--------------------------------------------------║");
            Console.WriteLine("╚══════════════════════════════════════════════════╝");

            Console.WriteLine("   1. ja         10000");
            Console.WriteLine("   2. nie ja     5000 ");
            Console.WriteLine("   3. karol      -1 ");
            Console.ReadKey();
            Console.Clear();
            DrawMenu();
            UpdateMenu(prev);
        }
        static void InitFrame()
        {
            Console.Clear();
            Console.CursorVisible = false;
            grid[0, 0] = '╔';
            grid[0, width - 1] = '╗';
            grid[height - 1, 0] = '╚';
            grid[height - 1, width - 1] = '╝';
            for (int i = 1; i < width-1; i++)
            {
                grid[0, i] = '═';
                grid[height - 1, i] = '═';

            }
            for (int i = 1; i < height-1; i++)
            {
                grid[i, 0] = '║';
                grid[i, width - 1] = '║';
            }
            for (int i = 1; i < height -1; i++)
            {
                for (int j = 1; j < width-1; j++)
                {
                    grid[i, j] = ' ';

                }
            }
        }
        static void DrawFrame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(grid[i, j].ToString());
                }
            }
        }
        static void InitSnake()
        {
            snake.Add(new Pos(21, 9));
            snake.Add(new Pos(22, 9));
            snake.Add(new Pos(23, 9));
            snake.Add(new Pos(24, 9));
            snake.Add(new Pos(25, 9));
            foreach(Pos pos in snake)
            {
                grid[pos.Y, pos.X] = 'o';
            }

        }
        static void DrawSnake()
        {
            int count = 0;

            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Pos snakePart in snake)
            {
                Console.SetCursorPosition(snakePart.X, snakePart.Y);
                count++;
                if (count < 5)
                    Console.Write('o');
                else
                {
                    Console.Write('ö');
                }

            }
        }
        static void DrawSnakeHead()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(snake_x, snake_y);
            Console.Write('ö');
        }
        static void DrawSnakeBodyOnHead()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(snake_x, snake_y);
            Console.Write('o');
        }
        static void MoveSnakeHead()
        {
            grid[snake_y, snake_x] = 'o';
            switch (current)
            {
                case direction.UP:
                    snake_y--;
                    break;
                case direction.DOWN:
                    snake_y++;
                    break;
                case direction.LEFT:
                    snake_x--;
                    break;
                case direction.RIGHT:
                    snake_x++;
                    break;
                default:
                    break;
            }
            snake.Add(new Pos(snake_x, snake_y));
        }
        static void Pause()
        {
            System.Threading.Thread.Sleep(time);
        }
        static bool IsGameover()
        {
            if (grid[snake_y, snake_x] != ' ')
                return true;
            return false;
        }
        static void DeleteSnakeTail()
        {
            Console.SetCursorPosition(snake[0].X, snake[0].Y);
            Console.Write(' ');
            if(snake.Count != snakeLength)
            {
                grid[snake[0].Y, snake[0].X] = ' ';
                snake.RemoveAt(0);
            }
        }
        static void ReadKeys()
        {
            ConsoleKeyInfo s;

            if(Console.KeyAvailable)
            {
                player.SoundLocation = @"c:\sound\move.wav";
                //player.Play();
                //player.SoundLocation = @"c:\sound\a.wav";
                player.Play();

                s = Console.ReadKey();
                switch(s.Key)
                {
                    case ConsoleKey.UpArrow:
                        if(current != direction.DOWN)
                        {
                            current = direction.UP;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (current != direction.UP)
                        {
                            current = direction.DOWN;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (current != direction.RIGHT)
                        {
                            current = direction.LEFT;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (current != direction.LEFT)
                        {
                            current = direction.RIGHT;
                        }
                        break;
                }

            }
        }
        static void IncreaseSnakeLength()
        {
            lengthTimer++;
            if (lengthTime==lengthTimer)
            {
                lengthTimer = 0;
                snakeLength++;
            }
        }
        static bool TargetTaken()
        {
            return snake_x == target_x && snake_y == target_y;
        }
        static void SetTarget()
        {
            Random rnd = new Random();

            int x = 0;
            int y = 0;

            while (grid[y,x]!= ' ')
            {
                x = rnd.Next(1, width - 1);
                y = rnd.Next(1, height - 1);

            }
            target_x = x;
            target_y = y;
            DrawTarget(x,y);

        }
        static void DrawTarget(int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(x, y);
            Console.Write('X');

        }
        static void InitScore()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 20);
            Console.Write("Score : 0");
        }

        static void UpdateScore()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(8, 20);
            Console.Write(score);
        }
    }
}
