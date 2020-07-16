
using UnityEngine;

namespace SP.Core.Data
{
    [System.Serializable]
    public class WorldPosition
    {
        private const int POSITION_SCALE = 1000;
        
        public int x;
        public int y;

        public void SetPosition(float x, float y)
        {
            this.x = Mathf.FloorToInt(x * POSITION_SCALE);
            this.y = Mathf.FloorToInt(y * POSITION_SCALE);
        }

        public Vector2 GetPosition()
        {
            return new Vector2(x / POSITION_SCALE, y / POSITION_SCALE);
        }

        public static Vector2 FromServerPosition(float x, float y)
        {
            return new Vector2(x / POSITION_SCALE, y / POSITION_SCALE);
        }
    }
}