using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace asim.unity.geometry
{
    /// <summary>
    /// Simple Polygon define as OuterVertices + HoleVerticies
    /// Supports Multiple Holes and Inner Polygon
    /// OuterVertices must be in Clockwise winding direction
    /// Hole Verticies must be in Anti-Clockwise winding direction
    /// </summary>
    public class SimplePolygon
    {
        public List<Vector2> OuterVertices;
        public List<List<Vector2>> HolesVertices;
        public List<SimplePolygon> InnerPolygon;

        /// <param name="outerVertices">Outer Vertices given in Clockwise</param>
        /// <param name="holesVertices">Hole Vertices given in Anti-Clockwise</param>
        public SimplePolygon(List<Vector2> outerVertices, List<List<Vector2>> holesVertices = null, List<SimplePolygon> innerPolygon = null)
        {
            //Ensure No Duplicate Vertices
            OuterVertices = outerVertices.Distinct().ToList();
            HolesVertices = holesVertices.Distinct().ToList();
            InnerPolygon = innerPolygon;
        }
        public List<Vector3> GetVertices()
        {
            List<Vector3> vertices = new List<Vector3>();
            for (int i = 0; i < OuterVertices.Count; i++)
            {
                vertices.Add(OuterVertices[i]);
            }
            if (HolesVertices != null)
            {
                foreach (var holes in HolesVertices)
                {
                    for (int i = 0; i < holes.Count; i++)
                    {
                        vertices.Add(holes[i]);
                    }
                }
            }

            return vertices;
        }
    }
}