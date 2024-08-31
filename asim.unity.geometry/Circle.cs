using UnityEngine;

namespace asim.unity.geometry
{
    public struct Circle
    {
        public Vector2 Center { get; }
        public float Radius { get; }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool ContainsPoint(Vector2 point)
        {
            return Vector2.Distance(Center, point) <= Radius;
        }

        public override readonly string ToString()
        {
            return $"Circle(Center: {Center}, Radius: {Radius})";
        }

        public void Deconstruct(out Vector2 center, out float radius)
        {
            center = Center;
            radius = Radius;
        }
    }
}