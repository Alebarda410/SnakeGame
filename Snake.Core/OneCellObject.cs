namespace SnakeGame.Core
{
    public class OneCellObject
    {
        public OneCellObject(int x, int y, ObjectType objectType)
        {
            ObjectType = objectType;
            X = x;
            Y = y;
        }
        public ObjectType ObjectType { get; init; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum ObjectType
    {
        Apple, Border, SnakeSegment, Void
    }
}
