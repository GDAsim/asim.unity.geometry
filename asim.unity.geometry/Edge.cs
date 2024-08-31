using System;
using UnityEngine;

namespace asim.unity.geometry
{
    public struct Edge : IEquatable<Edge>
    {
        public Vector2 v0 { get; set; }
        public Vector2 v1 { get; set; }

        public Edge(Vector2 v0, Vector2 v1)
        {
            this.v0 = v0;
            this.v1 = v1;
        }

        public bool Equals(Edge other)
        {
            return (v0 == other.v0 && v1 == other.v1) || (v0 == other.v1 && v1 == other.v0);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Edge other = (Edge)obj;

            return (v0 == other.v0 && v1 == other.v1) || (v0 == other.v1 && v1 == other.v0);
        }
    }
}
