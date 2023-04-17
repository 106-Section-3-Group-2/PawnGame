namespace PawnGame
{
    internal class Level
    {
        private Room[,] _rooms;

        public Room this[int i, int j]
        {
            get
            {
                return _rooms[i, j];
            }
        }

        public Level()
        {

        }
    }
}
