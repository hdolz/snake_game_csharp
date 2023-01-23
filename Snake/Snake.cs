namespace SnakeGame
{
    public class Snake
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;
        int sX = 0;
        int sY = 0;
        int gX = 0, gY = 0;

        List<SnakePiece> snake;

        bool keepPlaying = true;
        int score = 0;
        int movimentFlag = 1;

        enum MoveDirection
        {
            UP = 2,
            DOWN = 4,
            LEFT = 3,
            RIGHT = 1
        }

        class SnakePiece
        {
            public int X { get; set; }
            public int Y { get; set; }
            public SnakePiece(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        void UndrawSnake()
        {
            Console.SetCursorPosition(snake[snake.Count - 1].X, snake[snake.Count - 1].Y);
            Console.Write(" ");
        }

        void DrawGameLimits()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            //linhas horizontais
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(i, 0); Console.Write("-"); //superior
                Console.SetCursorPosition(i, height - 1); Console.Write("-");//inferior
            }
            //linhas verticais
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(0, i); Console.Write("¦"); //esquerda
                Console.SetCursorPosition(width - 1, i); Console.Write("¦"); //direita
            }
        }

        void FramePause()
        {
            Thread.Sleep(1000 / 30);
        }

        void DrawSnake()
        {
            for (int i = 0; i < snake.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (i == 0)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(snake[i].X, snake[i].Y);
                Console.Write("O");
            }
        }

        void DrawGoal(int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(x, y);
            Console.Write("8");
        }

        bool CheckObjectivePosition(int sX, int sY, int gX, int gY)
        {
            return (sX == gX && sY == gY);
        }

        void DrawScoreboard(int point)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(3, 3);
            Console.Write($"POINTS: {point}");
        }

        void GenerateGoalPosition()
        {
            int randomX = 0;
            int randomY = 0;
            bool randomOk = true;
            while (randomOk)
            {
                randomX = new Random().Next(1, width - 2);
                randomY = new Random().Next(2, height - 2);

                for (int i = 0; i < snake.Count; i++)
                    if (snake[i].X == randomX && snake[i].Y == randomY)
                        randomOk = false;
                if (randomOk)
                    break;
                randomOk = true;
            }
            gX = randomX;
            gY = randomY;
        }

        public void Run()
        {
            snake = new List<SnakePiece>();
            int iniPosX = width / 2;
            int iniPosY = height / 2;
            snake.Add(new SnakePiece(iniPosX, iniPosY));
            GenerateGoalPosition();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;
            DrawGameLimits();
            DrawScoreboard(score);
            DrawGoal(gX, gY);
            sX = width / 2;
            sY = height / 2;
            while (keepPlaying)
            {
                if (Console.KeyAvailable)
                {
                    Input(Console.ReadKey(true).Key);
                }
                Move(movimentFlag);
                DrawScoreboard(score);
                DrawSnake();
                FramePause();
            }
        }

        void Input(ConsoleKey key)
        {
            if (key == ConsoleKey.W) movimentFlag = (int)MoveDirection.UP;
            if (key == ConsoleKey.S) movimentFlag = (int)MoveDirection.DOWN;
            if (key == ConsoleKey.D) movimentFlag = (int)MoveDirection.RIGHT;
            if (key == ConsoleKey.A) movimentFlag = (int)MoveDirection.LEFT;
        }

        void UpdateSnakePiecePositions(int x, int y)
        {
            for (int i = snake.Count - 1; i >= 1; i--)
            {
                snake[i].X = snake[i - 1].X;
                snake[i].Y = snake[i - 1].Y;
            }
            snake[0].X = x;
            snake[0].Y = y;
        }

        private void Move(int movimentFlag)
        {
            switch (movimentFlag)
            {
                case (int)MoveDirection.RIGHT:
                    if (sX < width - 2)
                    {
                        if (CheckObjectivePosition(sX + 1, sY, gX, gY))
                        {
                            //marcou ponto
                            score++;
                            //DrawScoreboard(score);
                            snake.Insert(0, new SnakePiece(sX + 1, sY));
                            GenerateGoalPosition();
                            DrawGoal(gX, gY);
                        }
                        sX += 1;

                        UndrawSnake();
                        UpdateSnakePiecePositions(sX, sY);
                    }
                    break;
                case (int)MoveDirection.DOWN:
                    if (sY < height - 2)
                    {
                        if (CheckObjectivePosition(sX, sY + 1, gX, gY))
                        {
                            //marcou ponto
                            score++;
                            //DrawScoreboard(score);
                            snake.Insert(0, new SnakePiece(sX, sY + 1));
                            GenerateGoalPosition();
                            DrawGoal(gX, gY);
                        }
                        sY += 1;
                        UndrawSnake();
                        UpdateSnakePiecePositions(sX, sY);
                    }
                    break;
                case (int)MoveDirection.LEFT:
                    if (sX > 1)
                    {
                        if (CheckObjectivePosition(sX - 1, sY, gX, gY))
                        {
                            //marcou ponto
                            score++;
                            //DrawScoreboard(score);
                            snake.Insert(0, new SnakePiece(sX - 1, sY));
                            GenerateGoalPosition();
                            DrawGoal(gX, gY);
                        }
                        sX -= 1;
                        UndrawSnake();
                        UpdateSnakePiecePositions(sX, sY);
                    }
                    break;
                case (int)MoveDirection.UP:
                    if (sY > 1)
                    {
                        if (CheckObjectivePosition(sX, sY - 1, gX, gY))
                        {
                            //marcou ponto
                            score++;
                            //DrawScoreboard(score);
                            snake.Insert(0, new SnakePiece(sX, sY - 1));
                            GenerateGoalPosition();
                            DrawGoal(gX, gY);
                        }
                        sY -= 1;
                        UndrawSnake();
                        UpdateSnakePiecePositions(sX, sY);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
