using SnakeGame.Core;

var mapHeight = Console.BufferHeight / 2;
var mapWidth = Console.BufferWidth / 4;
var map = new OneCellObject[mapWidth, mapHeight];
var voidZone = new List<(int x, int y)>();
var snake = new Snake(mapWidth / 2, mapHeight / 2, 5);
var rnd = new Random();

void DrawBorder()
{
    for (var x = 0; x < mapWidth; x++)
    {
        for (var y = 0; y < mapHeight; y++)
        {
            if (x == 0 || y == 0 || x == mapWidth - 1 || y == mapHeight - 1)
            {
                map[x, y] = new OneCellObject(x, y, ObjectType.Border);
                Console.SetCursorPosition(x, y);
                Console.Write('█');
            }
            else
            {
                voidZone.Add((x, y));
            }
        }
    }
}

void ReDrawSnake(int delX, int delY)
{
    var (headX, headY) = (snake.Segments.First.ValueRef.X, snake.Segments.First.ValueRef.Y);

    map[headX, headY] = snake.Segments.First.Value;
    Console.SetCursorPosition(headX, headY);
    Console.Write('*');
    voidZone.Remove((headX, headY));

    map[delX, delY] = null;
    Console.SetCursorPosition(delX, delY);
    Console.Write(' ');
    voidZone.Add((delX, delY));
}

void DrawSnake()
{
    foreach (var segment in snake.Segments)
    {
        map[segment.X, segment.Y] = segment;
        Console.SetCursorPosition(segment.X, segment.Y);
        Console.Write('*');
        voidZone.Remove((segment.X, segment.Y));
    }
}

void GameControls()
{
    while (true)
    {
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.UpArrow:
                snake.ChangeDirection(MovementDirection.Top);
                break;
            case ConsoleKey.RightArrow:
                snake.ChangeDirection(MovementDirection.Right);
                break;
            case ConsoleKey.LeftArrow:
                snake.ChangeDirection(MovementDirection.Left);
                break;
            case ConsoleKey.DownArrow:
                snake.ChangeDirection(MovementDirection.Bottom);
                break;
        }
    }
}

void DrawAple()
{
    var num = rnd.Next(voidZone.Count);
    var coord = voidZone[num];
    map[coord.x, coord.y] = new OneCellObject(coord.x, coord.y, ObjectType.Apple);
    Console.SetCursorPosition(coord.x, coord.y);
    Console.Write('@');
    voidZone.Remove(coord);
}

void GameOver()
{
    Console.Title = $"Игра закончена. Текущий счет: {snake.AppleCount}";
    // TODO сделать меню перезапуска
}

void MoveSnake()
{
    var (headX, headY) = (snake.Segments.First.ValueRef.X, snake.Segments.First.ValueRef.Y);
    var (checkX, checkY) = (headX, headY);
    switch (snake.MovementDirection)
    {
        case MovementDirection.Top:
            checkY -= 1;
            break;
        case MovementDirection.Bottom:
            checkY += 1;
            break;
        case MovementDirection.Left:
            checkX -= 1;
            break;
        case MovementDirection.Right:
            checkX += 1;
            break;
    }
    var checkObj = map[checkX, checkY];

    switch (checkObj?.ObjectType)
    {
        case ObjectType.Apple:
            Eat(checkObj);
            return;
        case ObjectType.Border:
        case ObjectType.SnakeSegment:
            GameOver();
            return;
    }
    var (x, y) = snake.Move();
    ReDrawSnake(x, y);
}

void Eat(OneCellObject apple)
{
    snake.Eat(apple);
    map[apple.X, apple.Y] = snake.Segments.First.ValueRef;
    voidZone.Remove((apple.X, apple.Y));
    Console.SetCursorPosition(apple.X, apple.Y);
    Console.Write('*');
    Console.Title = $"Текущий счет: {snake.AppleCount}";
    DrawAple();
}

async Task Loop()
{
    _ = Task.Run(GameControls).ConfigureAwait(false);
    while (true)
    {
        var delay = Task.Delay(250).ConfigureAwait(false);
        MoveSnake();
        await delay;
    }
}

Console.CursorVisible = false;
Console.Title = $"Текущий счет: {snake.AppleCount}";
DrawBorder();
DrawSnake();
DrawAple();
await Loop();