using System.Collections.Generic;

using UnityEngine;

namespace asim.unity.geometry
{
    public static class GeometryHelpers
    {
        public static void GizmoDrawHalfEdges(List<DoublyConnectedEdgeList.HalfEdge> HalfEdges,
            Color edgeColor,
            Color borderEdgeColor)
        {
            for (int i = 0; i < HalfEdges.Count; i++)
            {
                var he = HalfEdges[i];

                if (he.IncidentFace != null)
                {
                    Gizmos.color = edgeColor;
                }
                else
                {
                    Gizmos.color = borderEdgeColor;
                }

                var from = he.Origin.Pos;
                var to = he.Next.Origin.Pos;
                var dir = (from - to).normalized;
                var offset = Vector3.Cross(dir, Vector3.forward) * 0.2f;
                Gizmos.DrawLine(from - offset - dir * 0.8f, to - offset + dir * 0.8f);
            }
        }

        public static void GizmoDrawFaceHalfEdges(DoublyConnectedEdgeList.Face face,
            Color edgeColor,
            Color twinColor,
            float offsetAmt)
        {
            foreach (var he in DoublyConnectedEdgeList.WalkFace(face))
            {
                Gizmos.color = edgeColor;

                var from = he.Origin.Pos;
                var to = he.Next.Origin.Pos;
                var dir = (from - to).normalized;
                var offset = Vector3.Cross(dir, Vector3.forward) * offsetAmt;
                Gizmos.DrawLine(from - offset - dir * 0.8f, to - offset + dir * 0.8f);

                Gizmos.color = twinColor;

                var from2 = he.Twin.Origin.Pos;
                var to2 = he.Twin.Next.Origin.Pos;
                var dir2 = (from2 - to2).normalized;
                var offset2 = Vector3.Cross(dir2, Vector3.forward) * offsetAmt;
                Gizmos.DrawLine(from2 - offset2 - dir2 * 0.8f, to2 - offset2 + dir2 * 0.8f);
            }
        }
    }
}