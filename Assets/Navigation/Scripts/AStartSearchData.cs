namespace Navigation
{
    public struct AStarSearchNodeData
    {
        public int costSoFar;
        public int cameFrom;
        public bool walkable;
    }
}