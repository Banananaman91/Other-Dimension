namespace Terrain
{
    public class MinMax
    {
        public float Min { get; private set; }
        public float MaX { get; private set; }

        public MinMax()
        {
            Min = float.MaxValue;
            MaX = float.MinValue;
        }

        public void AddValue(float v)
        {
            if (v > MaX)
            {
                MaX = v;
            }

            if (v < Min)
            {
                Min = v;
            }
        }
    }
}
