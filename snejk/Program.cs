using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snejk
{
    //TODO:
    // menu
    // ranking
    //muzyczka
    //bomby czy inne gowno
    //poziom trudnosci
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
                    score++;
                    UpdateScore();

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

            Console.SetCursorPosition(0, 20);
            Console.WriteLine("game over");
            while(true)
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
        static void Menu()
        {
            Console.WriteLine("||========================================================||");
            Console.WriteLine("||--------------------------------------------------------||");
            Console.WriteLine("||---------------- Welcome to Snake Game -----------------||");
            Console.WriteLine("||--------------------------------------------------------||");
            Console.WriteLine("||========================================================||");

            while(!gameStart)
            {
                ConsoleKeyInfo s;
                s = Console.ReadKey();
                if (s.Key == ConsoleKey.DownArrow)
                {
                    if(selection<4)
                    selection++;
                }
                if (s.Key == ConsoleKey.UpArrow)
                {
                    if (selection >0)
                        selection--;
                }
                if (s.Key==ConsoleKey.Enter)
                {
                    switch (selection)
                    {
                        case 0:
                            gameStart = true;
                            break;
                        case 1:
                            Console.Clear();
                            Console.WriteLine("ja najlepszy");
                            Console.ReadKey();
                            Console.Clear();


                            break;
                        case 2:
                            time /= 2;
                            break;
                        case 3:
                            Environment.Exit(0);
                            break;

                    }

                    
                }

            }
            //0 graj se 
            //1 ranking
            //2 poziom trudnosci
            //3 wyjdz
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
            Console.SetCursorPosition(51, 0);
            Console.Write("Score : 0");
        }

        static void UpdateScore()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(59, 0);
            Console.Write(score);
        }
    }
}
