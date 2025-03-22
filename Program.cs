namespace _0318_solo
{
    internal class Program
    {
        // 데카르트 좌표계
        struct Position
        {
            public int x;
            public int y;
        }
        // 초기 조건 시작, 먹이 위치, 맵 배치
        static void Start(out Position snakePos, out Position foodPos, out char[,] map, ref List<Position> snakeList, int snakeLength)
        {
            snakePos.x = 5;
            snakePos.y = 5;

            foodPos.x = 5;
            foodPos.y = 6;

            snakeList.Add(snakePos);



            map = new char[20, 20];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (i == 0 || j == 0 || i == 19 || j == 19)
                    {
                        map[i, j] = ' ';
                    }
                    else
                    {
                        map[i, j] = '□';
                    }
                }
            }
        }
        // 메인 메서드
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            bool gameOver = false;
            Position snakePos;
            Position foodPos;
            int snakeLength = 1;
            List<Position> snakeList = new List<Position>();
            char[,] map;

            Start(out snakePos, out foodPos, out map, ref snakeList, snakeLength);

            while (gameOver == false)
            {

                Render(snakeList, foodPos, map, snakeLength);
                ConsoleKey key = Input();
                Update(key, ref snakePos, ref gameOver, ref foodPos, map, ref snakeLength, ref snakeList);

            }

            End(snakeLength);
        }
        // 맵, 오브젝트 구현 함수
        static void Render(List<Position> snakeList, Position foodPos, char[,] map, int snakeLength)
        {
            Console.SetCursorPosition(0, 0);

            PrintMap(map);
            PrintSnake(snakeList, snakeLength);
            PrintFood(foodPos);
        }
        // 맵 표시
        static void PrintMap(char[,] map)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (map[i, j] != '■')
                    {
                        Console.Write(map[i, j]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(map[i, j]);
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
        }
        // 뱀의 위치 표시 -> List<Position>의 모든 요소 출력 -> 초록색 네모
        static void PrintSnake(List<Position> snakeList, int snakeLength)
        {
            for (int i = 0; i < snakeLength; i++)
            {
                Console.SetCursorPosition(snakeList[i].x, snakeList[i].y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine('■');
                Console.ResetColor();
            }
        }
        // 음식 위치 표시 -> 노랑색 동그라미 -> 네모로 할 수 있도록 하는 방법이 있을까?
        static void PrintFood(Position foodPos)
        {
            Console.SetCursorPosition(foodPos.x, foodPos.y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine('●');
            Console.ResetColor();
        }
        // 콘솔키를 활용하여 사용자 입력 값 받도록
        static ConsoleKey Input()
        {
            return Console.ReadKey(true).Key;
        }
        // 게임 구현 기본 로직 함수
        static void Update(ConsoleKey key, ref Position snakePos, ref bool gameOver, ref Position foodPos, char[,] map, ref int snakeLength, ref List<Position> snakeList)
        {
            Move(key, ref snakePos, map, ref snakeList, ref gameOver);
            SnakeAdd(map, snakePos, foodPos, snakeLength, ref snakeList);
            Food(snakePos, ref foodPos, map, ref snakeLength);

            gameOver = IsDead(gameOver, snakeList, snakeLength);
        }
        // 뱀의 무빙 로직 구현
        static void Move(ConsoleKey key, ref Position snakePos, char[,] map, ref List<Position> snakeList, ref bool gameOver)
        {
            int timer = Environment.TickCount;
            // 음식을 먹으면 뱀의 끝부분에 몸이 생성되어 길어지도록 구현
            // 뱀은 머리가 가는 경로를 따라감 -> 이걸 어떻게 구현하지? -> List<Position>에 넣고 요소를 출력하는 것으로 해결
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    if (map[snakePos.y - 1, snakePos.x] == '□')
                    {
                            snakePos.y--;
                    }
                    // 뱀의 머리가 벽에 닿을 경우 게임 종료되도록 설계
                    else if (map[snakePos.y - 1, snakePos.x] == ' ')
                    {
                        gameOver = true;
                    }
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    if (map[snakePos.y, snakePos.x - 1] == '□')
                    {
                            snakePos.x--;                       
                    }
                    else if (map[snakePos.y, snakePos.x - 1] == ' ')
                    {
                        gameOver = true;
                    }
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    if (map[snakePos.y + 1, snakePos.x] == '□')
                    {
                            snakePos.y++;
                    }                
                    else if (map[snakePos.y + 1, snakePos.x] == ' ')
                    {
                        gameOver = true;
                    }
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    if (map[snakePos.y, snakePos.x + 1] == '□')
                    {
                            snakePos.x++;                       
                    }
                    else if (map[snakePos.y, snakePos.x + 1] == ' ')
                    {
                        gameOver = true;
                    }
                    break;
            }
            // 앞으로 이동한 자리가 새로운 뱀의 머리가 되고, 뱀의 꼬리는 버리는 형태로 움직임 구현
            snakeList.RemoveAt(0);
            snakeList.Add(snakePos);
        }
        // 몸통 생성 로직 구현
        static void SnakeAdd(char[,] map, Position snakePos, Position foodPos, int snakeLength, ref List<Position> snakeList)
        {   // 뱀이 음식을 먹을 경우 음식이 List<Position>에 포함되어 몸통으로 출력되도록 설계
            if (snakePos.x == foodPos.x && snakePos.y == foodPos.y)
                snakeList.Add(foodPos);
        }
        // 음식 생성 로직 구현
        static void Food(Position playerPos, ref Position foodPos, char[,] map, ref int snakeLength)
        {   // 뱀이 음식을 먹을 경우, 맵 안에 랜덤 스폰되도록 구현.
            // 음식이 뱀이 자리하고 있는 부분에 안나오도록 구현할 수 있을까?
            Random foodNewPos = new Random();
            if (playerPos.x == foodPos.x && playerPos.y == foodPos.y)
            {
                foodPos.x = foodNewPos.Next(1, map.GetLength(1) - 1);
                foodPos.y = foodNewPos.Next(1, map.GetLength(0) - 1);
                snakeLength++;
            }
            //맵에 랜덤으로 생성되도록 맵 크기 한정한 난수 생성
        }
        // 종료 조건 설정
        static bool IsDead(bool gameOver, List<Position> snakeList, int snakeLength)
        {
            {   // 뱀의 머리(맨 앞)이 뱀의 몸통과 닿을 경우 게임 종료 
                for (int i = 0; i < snakeLength - 2; i++)
                {
                    if (snakeList[snakeLength - 1].x == snakeList[i].x && snakeList[snakeLength - 1].y == snakeList[i].y)
                    {
                        gameOver = true;
                    }
                }
            }
            return gameOver;
        }
        // 종료 페이즈
        static void End(int snakeLength)
        {
            // 최대 길이 출력, 수고 메세지
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game Over");
            Console.ResetColor();
            Console.WriteLine($"Score : {snakeLength}");
        }
    }
}
