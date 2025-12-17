using System;
using System.Text;

class Program
{
    private const char PLAYER = 'P';
    private const char PLAYER_ON_GOAL = '@';
    private const char GOAL = 'G';
    
    private const char BOMB = 'B';
    private const char BOMB_ON_GOAL = '!';
    
    private const char WALL = '#';
    private const char EMPTY = ' ';
    
    //맵 구성
    static char[,] map = new char[10, 10]
    {
        { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' },
        { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', 'B', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', ' ', 'G', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', 'P', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', ' ', 'G', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', 'B', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
        { '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' }
    };
    
    //기본 위치
    static Position _playerPosition =
        new Position()
        {
            X = 4,
            Y = 4
        };
    
    //이동거리
    private static int _moveCount = 0;
    
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        PrintGuideText();
        
        while (true)
        {
            PrintMoveCount();
            PrintMap();

            if (IsGameClear())
            {
                Console.WriteLine("종료");
            }
            //입력 받기 (W, A, S, D, Q, 그외.)
            
            ConsoleKey inputKey;
            if (!TryGetInput(out inputKey)) continue;
            
            //종료 버튼이 눌렸을 경우.
            if (inputKey == ConsoleKey.Q)
            {
                Console.WriteLine("종료 버튼이 눌렸습니다 종료합니다");
                break;
                
            }
            
            //다음 이동할 곳 위치 계산
            Position nextPos = GetNextPosition(inputKey);
            
            //범위 밖일때.
            if(OutOfArray(nextPos)) continue;
            //벽이 있을경우.
            char targetTile = GetTile(nextPos);
            if (targetTile == WALL)
            {
                continue;
            }

            //이동 구현
            
            //플레이어 단순 이동
            if (targetTile == EMPTY || targetTile == GOAL)
            {
                Move(_playerPosition, nextPos);
            }
            
            //폭탄 밀면서 이동
            if (targetTile == BOMB || targetTile == BOMB_ON_GOAL)
            {
                
            }

        }
        
        
    }
    struct Position
    {
        public int X;
        public int Y;
    }

    static void PrintGuideText()
    {
        Console.Clear();
        Console.WriteLine("이동  W: 위, A: 왼쪽, S: 아래, D: 오른쪽");
        Console.WriteLine("플레이어를 이동시켜 폭탄을 목표지점에 밀어넣으세요");
    }

    static void PrintMoveCount()
    {
        Console.WriteLine($"총 이동거리: {_moveCount}");
    }

    static void PrintMap()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Console.Write(map[i, j]);
            }
            Console.WriteLine();
        }
    }

    static bool TryGetInput(out ConsoleKey inputKey)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        inputKey = keyInfo.Key;
        
        return inputKey == ConsoleKey.W ||
               inputKey == ConsoleKey.A ||
               inputKey == ConsoleKey.S ||
               inputKey == ConsoleKey.D ||
               inputKey == ConsoleKey.Q;
    }
    static bool IsGameClear()
    {

        return false;
    }

    static Position GetNextPosition(ConsoleKey inputKey)
    {
        Position nextPos = new Position();
        nextPos.X = _playerPosition.X;
        nextPos.Y = _playerPosition.Y;

        if (inputKey == ConsoleKey.W)
            nextPos.Y--;
        if (inputKey == ConsoleKey.A)
            nextPos.X--;
        if (inputKey == ConsoleKey.S)
            nextPos.Y++;
        if (inputKey == ConsoleKey.D)
            nextPos.X++;
        
        return nextPos;
    }

    static bool OutOfArray(Position nextPos)
    {
        if (nextPos.X < 0 || nextPos.X >= map.GetLength(1))
            return true;
        if (nextPos.Y < 0 || nextPos.Y >= map.GetLength(0))
            return true;
        else return false;
    }

    static char GetTile(Position nextPos)
    {
        return map[nextPos.X, nextPos.Y];
    }

    static void Move(Position playerPos, Position nextPos)
    {
        //origin = player or playerongoal, destinationtile = empty or goal
        char originTile = GetTile(playerPos);
        char destinationTile = GetTile(nextPos);
        

    }
}