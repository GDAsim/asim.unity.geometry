using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;
using System.Collections.Generic;

namespace asim.unity.utils.geometry
{
    [ExecuteInEditMode]
    public class IsPolygonMonotone_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject Polygon;

        public static List<(Vector2,Vector2)> diagonals = new();

        (Vector2, Vector2, Vector2) FindAnEar(List<Vector2> GSP,int index)//Good sub polygon
        {
            Vector2 prevVertex = GSP[(int)MathExtensions.IndexLoop(index - 1, GSP.Count)];
            Vector2 currentVertex = GSP[index];
            Vector2 nextVertex = GSP[(int)MathExtensions.IndexLoop(index + 1, GSP.Count)];

            //step 1. if index is ear, exit 
            //To determine if its an ear, it needs to be an convex/obtuse, and the triangle form from the neighbouring vertex does not contain any vertex
            int orientation = GeometryUtils.Orientation(prevVertex, currentVertex, nextVertex);
            if (orientation == -1 || orientation == 0)
            {
                bool found = false;
                for (int i = 0; i < GSP.Count; i++)
                {
                    if (GeometryUtils.IsPointInTriangle(prevVertex,currentVertex,nextVertex,GSP[i]) == 1)
                    {
                        found = true;
                    }
                }
                if(!found)
                {
                    return (prevVertex, currentVertex, nextVertex);
                }
            }


            //step 2. find vertex pj that will form a 'diagonal'
            int pj;
            //by creating a ray from index to the inside of the polygon,
            //loop though all edges with the ray to find a yintercept that has the min distance
            //2a. construct the ray by rotating a point by half of the angle of angle between vector, and extending the rotatated point
            float halfangle = (360 - Vector2.Angle(prevVertex - currentVertex, nextVertex - currentVertex))/2;
            var RotatedPoint = GeometryUtils.RotatePointAroundPivot(currentVertex, prevVertex, halfangle * Mathf.Deg2Rad);
            var EndRayPoint = currentVertex + (RotatedPoint - currentVertex)*20;
            
            //2b. search each edge of the polygon to see which edge the ray intercepts(y) with
            float mindist = float.PositiveInfinity;
            Vector2 mininterceptPoint = Vector2.zero;
            int kindex = 0;
            for (int i = 0; i < GSP.Count; i++)
            {
                Vector2 edgep1 = GSP[(int)MathExtensions.IndexLoop(i, GSP.Count)];
                Vector2 edgep2 = GSP[(int)MathExtensions.IndexLoop(i + 1, GSP.Count)];

                if (currentVertex == edgep1 || currentVertex == edgep2) continue;//skip if we have matching vertex

                (bool IsParallel, bool IsLinesIntercept, Vector2 interceptPoint, _) =
                    GeometryUtils.IsLinesIntercept(currentVertex, EndRayPoint, edgep1, edgep2);

                //2c. if an edge is found, make sure its we take the interceptpoint closest (min distance)
                if(!IsParallel && IsLinesIntercept)
                {
                    float dist = Vector2.Distance(currentVertex, interceptPoint);
                    if(dist < mindist)
                    {
                        mindist = dist;
                        mininterceptPoint = interceptPoint;
                        kindex = i;
                    }
                }
            }

            //step 3. Choosing 'z'
            int? zIndex = null;
            Vector2 pi = currentVertex;
            Vector2 pk = GSP[kindex];
            Vector2 pk1 = GSP[(int)MathExtensions.IndexLoop(kindex + 1, GSP.Count)];
            Vector2 y = mininterceptPoint;

            //3a. check for possible points that are within the triangle (pi,pk+1,y) 
            int count = MathExtensions.CountFromToIndexLoop(kindex + 1, index, GSP.Count);//find out if there are any index between the points
            if(count>0)
            {
                //step 3b. loopthough each point and see if they are within the triangle,
                float minangle = float.PositiveInfinity;
                for (int i = 0; i < count; i++)
                {
                    int r = (int)MathExtensions.IndexLoop(kindex + 1 + i, GSP.Count);
                    Vector2 pr = GSP[r];

                    if (GeometryUtils.IsPointInTriangle(pi, pk1, y, pr) == 1)
                    {
                        //Step 3c. pick 'z' as the one what has the min angle y,pi,pr
                        float angle = Vector2.Angle(y - pi, y - pr);
                        if(angle < minangle)
                        {
                            minangle = angle;
                            zIndex = r;
                        }
                    }
                }

                //Step 3d. if there are points in between the triangle and its not the pi-1 then we have found a diagonal
                if (zIndex.HasValue && zIndex.Value != index-1)
                {
                    pj = zIndex.Value;

                    List<Vector2> newGSP = new List<Vector2>();
                    count = MathExtensions.CountFromToIndexLoop(pj, index, GSP.Count);
                    for (int i = 0; i < count; i++)
                    {
                        newGSP.Add(GSP[(int)MathExtensions.IndexLoop(pj + i, GSP.Count)]);
                    }
                    return FindAnEar(newGSP, Mathf.FloorToInt(count / 2));
                }
                //Step 3e. otherwise if there are no points in between the triangle, and pk+1 != pi-1 then we have found a diagonal
                else if (pk1 != prevVertex)
                {
                    pj = kindex + 1;

                    List<Vector2> newGSP = new List<Vector2>();
                    count = MathExtensions.CountFromToIndexLoop(pj, index, GSP.Count);
                    for (int i = 0; i < count; i++)
                    {
                        newGSP.Add(GSP[(int)MathExtensions.IndexLoop(pj + i, GSP.Count)]);
                    }
                    return FindAnEar(newGSP, Mathf.FloorToInt(count / 2));
                }
                else
                {
                    return (GSP[index], pk1, pk);
                }
            }
            //step 4a. if there are no index between, check the other side
            //check for possible points that are within the triangle (pi,y,pk) 
            else
            {
                int count2 = MathExtensions.CountFromToIndexLoop(index, kindex, GSP.Count);
                float minangle2 = float.PositiveInfinity;
                for (int i = 0; i < count2; i++)
                {
                    int s = (int)MathExtensions.IndexLoop(kindex + 1 + i, GSP.Count);
                    Vector2 ps = GSP[s];

                    if (GeometryUtils.IsPointInTriangle(pi, y, pk, pk) == 1)
                    {
                        float angle = Vector2.Angle(y - pi, y - ps);
                        if (angle < minangle2)
                        {
                            minangle2 = angle;
                            zIndex = s;
                        }
                    }
                }
                if (zIndex.HasValue && zIndex.Value != index+1)
                {
                    pj = zIndex.Value;

                    List<Vector2> newGSP = new List<Vector2>();
                    count = MathExtensions.CountFromToIndexLoop(pj, index, GSP.Count);
                    for (int i = 0; i < count; i++)
                    {
                        newGSP.Add(GSP[(int)MathExtensions.IndexLoop(pj + i, GSP.Count)]);
                    }
                    return FindAnEar(newGSP, Mathf.FloorToInt(count / 2));
                }
                else if (pk1 != nextVertex)
                {
                    pj = kindex + 1;

                    List<Vector2> newGSP = new List<Vector2>();
                    count = MathExtensions.CountFromToIndexLoop(pj, index, GSP.Count);
                    for (int i = 0; i < count; i++)
                    {
                        newGSP.Add(GSP[(int)MathExtensions.IndexLoop(pj + i, GSP.Count)]);
                    }
                    return FindAnEar(newGSP, Mathf.FloorToInt(count / 2));
                }
            }






            //visualize ray
            Vector3 p1pos = cam.WorldToScreenPoint(currentVertex);
            Vector3 p2pos = cam.WorldToScreenPoint(EndRayPoint);
            p1pos.y = UnityOnGUIHelper.Height - p1pos.y;
            p2pos.y = UnityOnGUIHelper.Height - p2pos.y;
            UnityOnGUIHelper.DrawLine(p1pos, p2pos, Color.red, 2);

            //visualize y intercept point
            Vector3 ypos = cam.WorldToScreenPoint(y);
            ypos.y = UnityOnGUIHelper.Height - ypos.y;
            UnityOnGUIHelper.DrawDot(ypos,4,0, Color.red, Color.red);

            //visualize k and k+1
            Vector3 kpos = cam.WorldToScreenPoint(GSP[kindex]);
            Vector3 kp1pos = cam.WorldToScreenPoint(GSP[(int)MathExtensions.IndexLoop(kindex + 1, GSP.Count)]);
            kpos.y = UnityOnGUIHelper.Height - kpos.y;
            kp1pos.y = UnityOnGUIHelper.Height - kp1pos.y;
            UnityOnGUIHelper.DrawDot(kpos, 6, 0, Color.green, Color.green);
            UnityOnGUIHelper.DrawDot(kp1pos, 6, 0, Color.green, Color.green);

            return (Vector2.zero, Vector2.zero, Vector2.zero);

        }

        void OnGUI()
        {
            List<Vector2> polygon = new List<Vector2>();
            for (int i = 0; i < Polygon.transform.childCount; i++)
            {
                var vertex = Polygon.transform.GetChild(i).transform.position;
                polygon.Add(vertex);
            }

            List<Vector2> GSP = new List<Vector2>(polygon);
            diagonals.Clear();
            while (GSP.Count > 3)
            {
                (Vector2 p1, Vector2 p2, Vector2 p3) = FindAnEar(GSP, 0);
                diagonals.Add((p1, p3));

                if (p2 == Vector2.zero) break;
                GSP.Remove(p2);
            }

            //drawing
            for (int i = 0; i < Polygon.transform.childCount; i++)
            {
                Transform p1 = Polygon.transform.GetChild(i);
                Transform p2 = Polygon.transform.GetChild((i+1) % Polygon.transform.childCount);

                //Convert World Pos to GUI Pos
                Vector3 p1pos = cam.WorldToScreenPoint(p1.position);
                Vector3 p2pos = cam.WorldToScreenPoint(p2.position);
                p1pos.y = UnityOnGUIHelper.Height - p1pos.y;
                p2pos.y = UnityOnGUIHelper.Height - p2pos.y;

                UnityOnGUIHelper.DrawLine(p1pos, p2pos, Color.red, 2);
            }

            foreach (var d in diagonals)
            {
                Vector3 p1 = cam.WorldToScreenPoint(d.Item1);
                Vector3 p2 = cam.WorldToScreenPoint(d.Item2);
                p1.y = UnityOnGUIHelper.Height - p1.y;
                p2.y = UnityOnGUIHelper.Height - p2.y;
                UnityOnGUIHelper.DrawLine(p1, p2, Color.blue, 3);
            }


            //if (IsPointInTriangle == 1) Text.text = "Point Is Inside Of Triangle";
            //else if (IsPointInTriangle == -1) Text.text = "Point Is Outside Of Triangle";
            //else Text.text = "Point Is On Triangle";
        }
    }
}
