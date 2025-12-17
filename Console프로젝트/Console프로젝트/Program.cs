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
            
            //입력 받기 (W, A, S, D, Q, 그외.)
            
            //다음 이동할 곳 위치 계산
            
            //범위 밖일때.
            
            //벽이 있을경우.
            
            //종료 버튼이 눌렸을 경우.
            
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
}