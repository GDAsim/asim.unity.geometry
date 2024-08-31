using System.Collections.Generic;

using UnityEngine;

namespace asim.unity.geometry
{
    public static partial class SmallestCircle
    {
        /// <summary>
        /// Welzl algorithm to calculate smallest circle from a list of points
        /// Recursive function
        /// Uses an extra list for boundarypoints
        /// https://en.wikipedia.org/wiki/Smallest-circle_problem
        /// </summary>
        public static Circle Welzl(List<Vector2> RemainingPoints)
        {
            List<Vector2> boundaryPoints = new();

            return WelzlInner(RemainingPoints, boundaryPoints);
        }
        static Circle WelzlInner(List<Vector2> RemainingPoints, List<Vector2> BoundaryPoints)
        {
            //0. Recursive Exist condition
            if (RemainingPoints.Count == 0 || BoundaryPoints.Count == 3)
            {
                return Trivial(BoundaryPoints);
            }

            // 1. Pop a point from existing P (optional to be random, but might exist two solutions)
            Vector2 randomPoint = RemainingPoints[RemainingPoints.Count - 1];
            RemainingPoints.RemoveAt(RemainingPoints.Count - 1);

            //2. Get the smallest circle without this point
            var D = WelzlInner(new(RemainingPoints), new(BoundaryPoints));

            //3. If the p is inside the circle, return the current circle
            if (D.ContainsPoint(randomPoint))
                return D;

            //4. Otherwise, the point must be on the boundary of the minimum circle
            BoundaryPoints.Add(randomPoint);
            return WelzlInner(new(RemainingPoints), new(BoundaryPoints));
        }
        static Circle Trivial(List<Vector2> boundary)
        {
            if (boundary.Count == 0)
            {
                return default;
            }
            if (boundary.Count == 1)
            {
                return new Circle(boundary[0], 0);
            }
            else if (boundary.Count == 2)
            {
                return GeometryUtils.GetCircleFromPoints(boundary[0], boundary[1]);
            }
            else // boundary.Count == 3
            {
                return GeometryUtils.GetCircumcircle(boundary[0], boundary[1], boundary[2]);
            }
        }
    }
}