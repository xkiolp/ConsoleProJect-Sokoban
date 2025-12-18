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
                Console.WriteLine("게임 클리어! 종료");
                break;
            }
            //입력 받기 (W, A, S, D, Q, 그외.)
            
            ConsoleKey inputKey;
            string pressedExtraKey;
            if (!TryGetInput(out inputKey, out pressedExtraKey)) continue;
            
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
                PlayerMove(_playerPosition, nextPos);
                _playerPosition = nextPos;
                _moveCount++;
            }
            
            //폭탄 밀면서 이동
            if (targetTile == BOMB || targetTile == BOMB_ON_GOAL)
            {
                if (pressedExtraKey == "Shift")
                {
                    SwapPlayerWithBomb(nextPos);
                    _playerPosition = nextPos;
                    _moveCount++;
                }
                else if (!TryPushingBomb(_playerPosition, nextPos))
                {
                    continue;
                }
                else
                {
                    _playerPosition = nextPos;
                    _moveCount++;
                }
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
    
    //첫번째 키를 눌렀을때 shift가 눌러져있으면 다시 입력을 받고 inputKey1=shift, inputkey2=wasd반환 
    static bool TryGetInput(out ConsoleKey inputKey, out string pressedTogether)
    {
        
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        inputKey = keyInfo.Key;
        bool isShift = keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift); // Shift 여부
        
        if (isShift)
        {
            pressedTogether = "Shift";
        }
        else pressedTogether = "";
        
        return inputKey == ConsoleKey.W ||
               inputKey == ConsoleKey.A ||
               inputKey == ConsoleKey.S ||
               inputKey == ConsoleKey.D ||
               inputKey == ConsoleKey.Q;
    }
    static bool IsGameClear()
    {
        //전 루프를 돌았을때 BOMB가 존재하지 않을 경우 게임 클리어
        for (int i = 0; i < map.GetLength(0);i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == BOMB)
                    return false;

            }
            
        }
        return true;
    }

    static Position GetNextPosition(ConsoleKey inputKey)
    {
        Position nextPos = new Position();
        nextPos.X = _playerPosition.X;
        nextPos.Y = _playerPosition.Y;

        if (inputKey == ConsoleKey.W)
            nextPos.X--;
        if (inputKey == ConsoleKey.A)
            nextPos.Y--;
        if (inputKey == ConsoleKey.S)
            nextPos.X++;
        if (inputKey == ConsoleKey.D)
            nextPos.Y++;
        
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

    static void PlayerMove(Position playerPos, Position nextPos)
    {//기존 타일 = 현재 타일 - playerPos, next타일 = nextPos의 타일 + playerPos
        //origin = player or playerongoal, destinationtile = empty or goal
        char fromTile = GetTile(playerPos);
        char toTile = GetTile(nextPos);

        char originTile = fromTile;
        char destinationTile = toTile;
        
        //타일 변환.
        //origin tile - player면 empty, player_ON_goal이면 goal.
        if (fromTile == PLAYER)
        {
            originTile = EMPTY;
        }
        else if (fromTile == PLAYER_ON_GOAL)
        {
            originTile = GOAL;
        }
        else
        {
            originTile = fromTile;
        }
        //destination tile
        if (toTile == EMPTY)
        {
            destinationTile = PLAYER;
        }
        else if (toTile == GOAL)
        {
            destinationTile = PLAYER_ON_GOAL;
        }
        else
        {
            destinationTile = toTile;
        }

        SetTile(playerPos, originTile);
        SetTile(nextPos, destinationTile);

    }

    static void SetTile(Position playerPos, char tile)
    {
        map[playerPos.X, playerPos.Y] = tile;
    }

    static bool TryPushingBomb(Position playerPos, Position nextPos)
    {
        char nextTile = GetTile(nextPos);
        
        //폭탄을 밀 위치 구하기. 
        Position pushedBombLocation = new Position()
        {
            X = nextPos.X + (nextPos.X - playerPos.X),
            Y = nextPos.Y + (nextPos.Y - playerPos.Y)
        };

        //폭탄을 밀 위치가 비어있는지 (EMPTY or GOAL)
        char pushedBombLocationTile = GetTile(pushedBombLocation);

        if (!(pushedBombLocationTile == EMPTY || pushedBombLocationTile == GOAL))
        {
            return false;
        }
        //폭탄을 밀 위치에 폭탄 배치 (EMPTY -> BOMB, GOAL -> BOMB_ON_GOAL)
        if (pushedBombLocationTile == EMPTY)
        {
            SetTile(pushedBombLocation, BOMB);
        }
        else if (pushedBombLocationTile == GOAL)
        {
            SetTile(pushedBombLocation, BOMB_ON_GOAL);
        }
        //기존 폭탄의 위치의 폭탄 제거
        if (nextTile == BOMB)
        {
            SetTile(nextPos, EMPTY);
        }
        else if(nextTile == BOMB_ON_GOAL)
        {
            SetTile(nextPos, GOAL);
        }
        //플레이어 이동 (move함수 사용)
        PlayerMove(playerPos, nextPos);
        return true;
    }

    static void SwapPlayerWithBomb(Position nextPos)
    {
        char currentTile = GetTile(_playerPosition);
        char nextTile = GetTile(nextPos);

        if (currentTile == PLAYER)
        {
            if (nextTile == BOMB)
            {
                SetTile(_playerPosition, BOMB);
                SetTile(nextPos, PLAYER);
            }
            else if (nextTile == BOMB_ON_GOAL)
            {
                SetTile(_playerPosition, BOMB);
                SetTile(nextPos, PLAYER_ON_GOAL);
            }
        }
        else if (currentTile == PLAYER_ON_GOAL)
        {
            if (nextTile == BOMB)
            {
                SetTile(_playerPosition, BOMB_ON_GOAL);
                SetTile(nextPos, PLAYER);
            }
            else if (nextTile == BOMB_ON_GOAL)
            {
                SetTile(_playerPosition, BOMB_ON_GOAL);
                SetTile(nextPos, PLAYER_ON_GOAL);
            }
        }
    }
}