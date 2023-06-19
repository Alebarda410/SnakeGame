namespace SnakeGame.Core
{
    public class Snake
    {
        public MovementDirection MovementDirection { get; private set; } = MovementDirection.Top;
        public int AppleCount { get; private set; }
        public LinkedList<OneCellObject> Segments { get; } = new LinkedList<OneCellObject>();

        public Snake(int startX, int startY, int startLenght)
        {
            for (var i = startY; i < startY + startLenght; i++)
            {
                var seg = new OneCellObject(startX, i, ObjectType.SnakeSegment);
                Segments.AddLast(seg);
            }
        }

        public void ChangeDirection(MovementDirection direction)
        {
            switch (MovementDirection)
            {
                case MovementDirection.Top:
                    if (direction == MovementDirection.Bottom) return;
                    break;
                case MovementDirection.Bottom:
                    if (direction == MovementDirection.Top) return;
                    break;
                case MovementDirection.Left:
                    if (direction == MovementDirection.Right) return;
                    break;
                case MovementDirection.Right:
                    if (direction == MovementDirection.Left) return;
                    break;
            }
            MovementDirection = direction;
        }

        public void Eat(OneCellObject apple)
        {
            Segments.AddFirst(new OneCellObject(apple.X, apple.Y, ObjectType.SnakeSegment));
            AppleCount++;
        }

        public (int delX, int delY) Move()
        {
            var (delX, delY) = (Segments.Last.ValueRef.X, Segments.Last.ValueRef.Y);
            for (var node = Segments.Last; node.Previous != null; node = node.Previous)
            {
                node.Value.X = node.Previous.ValueRef.X;
                node.Value.Y = node.Previous.ValueRef.Y;
            }

            switch (MovementDirection)
            {
                case MovementDirection.Top:
                    Segments.First.Value.Y -= 1;
                    break;
                case MovementDirection.Bottom:
                    Segments.First.Value.Y += 1;
                    break;
                case MovementDirection.Left:
                    Segments.First.Value.X -= 1;
                    break;
                case MovementDirection.Right:
                    Segments.First.Value.X += 1;
                    break;
            }
            return (delX, delY);
        }
    }

    public enum MovementDirection
    {
        Top, Bottom, Left, Right
    }
}