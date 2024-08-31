using UnityEngine;

namespace asim.unity.geometry
{
    public struct Triangle
    {
        public Vector2 v0 { get; set; }
        public Vector2 v1 { get; set; }
        public Vector2 v2 { get; set; }

        public Triangle(Vector2 v0, Vector2 v1, Vector2 v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
        }
    }
}