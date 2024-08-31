using System.Collections.Generic;

using UnityEngine;

namespace asim.unity.geometry
{
    public static class GeometryUtils
    {
        /// <summary>
        /// return the max distance of p of two other points
        /// </summary>
        public static float MaxDistance(Vector2 p, Vector2 p1, Vector2 p2)
        {
            return Mathf.Abs((p.y - p1.y) * (p2.x - p1.x) - (p2.y - p1.y) * (p.x - p1.x));
        }

        //SignedVolume are volumes that can be either positive or negative, depending on the winding
        //depending on the orientation in space of the region whose volume is being measured.
        //The volume is positive if d is to the left of the plane defined by the triangle(a, b, c).
        //IMPORTANT NOTE, this might result in floating point precision issue, which causes wrong values
        public static double SignedVolume(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            //first check if any points are same this will prevent possible floating point errors when doing dot and cross product
            if (a == b || b == c || a == c) return 0;

            return Vector3.Dot(a - d, Vector3.Cross(b - d, c - d)) / 6;
        }

        public static Vector2 RotatePointAroundPivot(Vector2 pivot, Vector2 point, float angle)
        {
            //step 1. take point and minus pivot, this will give coordinates based on origin (0,0)
            Vector2 p = (point - pivot);
            //Step 2. apply rotation algoirthm
            Vector2 rotatedPoint = new Vector2(p.x * Mathf.Cos(angle) - p.y * Mathf.Sin(angle), p.y * Mathf.Cos(angle) + p.x * Mathf.Sin(angle));
            return rotatedPoint;
        }



        





        /// <summary>
        /// compute direction of points going to the left or right of vector v1->v2 using cross product of 2d vector
        /// this is also known as calculating the determinant of two vector
        /// by checking the sign of cross product, will give us the direction, or which side the point is
        /// relative to the vector
        /// returns 1 if point is going to the left.
        /// returns -1 if point is going to the right.
        /// returns 0 if point is going straight/ lines on vector
        /// </summary>
        public static int Orientation(Vector2 p1, Vector2 p2, Vector2 p)
        {
            //First Vector v1 = p2-p1
            //Second Vector v2 = p-p1
            //Cross Product/det of (v1,v2) = v1.x*v2.y - v1
            //by checking the sign of cross product, will give us the direction, or which side the point is

            float val = (p2.x - p1.x) * (p.y - p1.y) -
                        (p2.y - p1.y) * (p.x - p1.x);

            if (val == 0) return 0;  // colinear
            return (int)Mathf.Sign(val); // CCW or CW
        }

        /// <summary>
        /// compute direction of points going to the left or right of vector v1->v2 using dot product
        /// this works by calculating the normal of the vector (simply by flipping the x=y,y=-x and negate one component)
        /// calculate the shortest signed distance of the vector to the point,
        /// check the sign to see if its pointing the same direction as normal vector
        /// this optimizes to computing the sign of dot product of (p-p1,normal) 
        /// </summary>
        public static int Orientation2(Vector2 p1, Vector2 p2, Vector2 p)
        {
            //p1->p = (p.x - p1.x,p.y - p1.y)
            //normal = (p2.y - p1.y,p1.x - p2.x)
            //dot = (p.x - p1.x)(p2.y - p1.y) + (p.y - p1.y)*(p1.x - p2.x)

            float val = (p.x - p1.x) * (p2.y - p1.y) -
                        (p.y - p1.y) * (p2.x - p1.x);

            if (val == 0) return 0;  // colinear
            return (int)Mathf.Sign(-val); // CCW or CW
        }

        /// <summary>
        /// compute direction of points going to the left or right of vector v1->v2 using dot product
        /// Uses the Shoelace algorithm to calculate area, if area is negative, then it is going to the left(CCW)
        /// </summary>
        public static int Orientation3(Vector2 p1, Vector2 p2, Vector2 p)
        {
            //not exactly using the shoelace algorithm,
            float area = p1.x * (p.y - p2.y) + 
                         p2.x * (p1.y - p.y) + 
                         p.x * (p2.y - p1.y);

            if (area == 0) return 0; //colinear
            return (int)Mathf.Sign(-area); // CCW or CW
        }

        /// <summary>
        /// compute direction of points going to the left or right of vector v1->v2 using gradient
        /// This works by comparing slopes, to see which one is steeper
        /// </summary>
        public static int Orientation4(Vector2 p1, Vector2 p2, Vector2 p)
        {
            float val = (p2.x - p1.x) * (p.y - p2.y) -
                        (p2.y - p1.y) * (p.x - p2.x);

            if (val == 0) return 0;  // colinear
            return (int)Mathf.Sign(val); // CCW or CW
        }

        /// <summary>
        /// Check if point is colinear
        /// Uses Triangle area method
        /// </summary>
        public static bool IsPointColinear(Vector2 p1, Vector2 p2, Vector2 p) => TriangleGetDeterminant(p1, p2, p) == 0;

        /// <summary>
        /// Check if point is colinear
        /// Uses Slope/Gradient method -> optimized to use cross product to avoid division by zero
        /// </summary>
        public static bool IsPointColinear2(Vector2 p1, Vector2 p2, Vector2 p)
        {
            return (p2.y - p1.y) * (p.x - p2.x) == (p.y - p2.y) * (p2.x - p1.x);
        }


        /// <summary>
        /// Check if a certain point is between a line (Asuming that all 3 points are colinear)
        /// </summary>
        public static bool IsPointBetweenColinearLine(Vector2 p1, Vector2 p2, Vector2 p)
        {
            Vector2 ab = p2 - p1;
            Vector2 ac = p - p1;
            Vector2 bc = p - p2;

            //Step 1. Compare Length to be smaller
            return ab.sqrMagnitude >= ac.sqrMagnitude && ab.sqrMagnitude >= bc.sqrMagnitude;
        }

        /// <summary>
        /// Check to see if Point is within certain radius of a target center
        /// </summary>
        public static int IsPointInRadius(Vector3 targetCenter,float radius,Vector3 point)
        {
            //use Square magnitud to save a squareroot call
            float distance = (point - targetCenter).sqrMagnitude;
            float val = radius * radius - distance;

            if (val == 0) return 0;  // on circle
            return (int)Mathf.Sign(val); // +1 or -1 , inside or outside
        }

        /// <summary>
        /// Check to see if Point is within certain radius of a target center
        /// </summary>
        public static int IsPointInEllipse(Vector2 targetCenter, Vector2 radius,float rotation, Vector2 point)
        {
            var xdiff = (point.x - targetCenter.x);
            var ydiff = (point.y - targetCenter.y);

            var cos = Mathf.Cos(rotation);
            var sin = Mathf.Sin(rotation);

            float val = ((cos * xdiff + sin * ydiff) * (cos * xdiff + sin * ydiff)) / (radius.x * radius.x) +
                        ((sin * xdiff - cos * ydiff) * (sin * xdiff - cos * ydiff)) / (radius.y * radius.y);

            val = 1 - val;

            if (val == 0) return 0;  // on ellipse
            return (int)Mathf.Sign(val); // +1 or -1 , inside or outside
        }

        /// <summary>
        /// Get area of Triangle using 3 points
        /// determinant method
        /// </summary>
        public static float TriangleGetArea(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return Mathf.Abs(p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) / 2f;
        }

        /// <summary>
        /// Get signed area of Triangle using 3 points
        /// </summary>
        public static float TriangleGetSignedArea(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) / 2f;
        }

        /// <summary>
        /// Get only Determinant 
        /// </summary>
        public static float TriangleGetDeterminant(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y));
        }

        #region Point in Triangle http://totologic.blogspot.com/2014/01/accurate-point-in-triangle-test.html
        /// <summary>
        /// Check if Point is in triangle , on triangle, or outside of triangle
        /// Based on "book Real Time Collision Detection" 
        /// uses barycentric coordinates method
        /// </summary>
        public static int IsPointInTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 point)
        {
            //calculate barycentric based on p1,p2
            var denominator = ((p2.y - p3.y) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.y - p3.y));
            var a = ((p2.y - p3.y) * (point.x - p3.x) + (p3.x - p2.x) * (point.y - p3.y)) / denominator;
            var b = ((p3.y - p1.y) * (point.x - p3.x) + (p1.x - p3.x) * (point.y - p3.y)) / denominator;
            //var c = 1 - a - b;

            if (a < 0 || b < 0) return -1; // outside
            if (a + b > 1) return -1;// outside
            if (a * b == 0) return 0;//on Triangle;
            return 1;//intriangle
        }

        /// <summary>
        /// Check if Point is in triangle , on triangle, or outside of triangle
        /// Uses the Same-Side or orientation method
        /// </summary>
        public static int IsPointInTriangleOrientation(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 point)
        {
            //Check Orientation of point on p1->p2 and check if its same side as p3 on p1->p2
            var o1 = Orientation(p1, p2, point) * Orientation(p1, p2, p3); // if same side, result should be +1
            //check if result is 0, means a point lines parallel to an edge on triangle,
            if (o1 == 0) return (point - p1).magnitude <= (p2 - p1).magnitude ? 0 : -1;

            //repeat
            var o2 = Orientation(p2, p3, point) * Orientation(p2, p3, p1);
            if (o2 == 0) return (point - p2).magnitude <= (p3 - p2).magnitude ? 0 : -1;

            //repeat
            var o3 = Orientation(p3, p1, point) * Orientation(p3, p1, p2);
            if (o3 == 0) return (point - p3).magnitude <= (p1 - p3).magnitude ? 0 : -1;

            if (o1 == 1 && o2 == 1 && o3 == 1) return 1;//in triangle
            return -1;// outside triangle
        }

        /// <summary>
        /// Check if Point is in triangle , on triangle, or outside of triangle
        /// This uses the easy understandable Area methord
        /// </summary>
        public static int IsPointInTriangleArea(Vector2 p1,Vector2 p2,Vector2 p3, Vector2 p)
        {
            //0. using point p, split the triangle into 3 small triangles with point p as the shared vertex

            //1. Calculate Area of main triangle
            float mainarea = TriangleGetArea(p1, p2, p3);

            //2. Calculate Area of 3 small triangle
            float t1area = TriangleGetArea(p, p2, p3);
            float t2area = TriangleGetArea(p1, p, p3);
            float t3area = TriangleGetArea(p1, p2, p);
            
            //3. Check if the main triangle area is the same as the 3 small triangle combined
            return Mathf.Approximately(mainarea, t1area + t2area + t3area) ? 1 : -1; // +1 or -2 , inside or outside
        }
        #endregion

        public static int IsPointInRect(Vector2 targetCenter, Vector2 size, float rotation, Vector2 point)
        {
            //todo

            float val = 0;

            if (val == 0) return 0;  // on rect
            return (int)Mathf.Sign(val); // +1 or -1 , inside or outside
        }

        /// <summary>
        /// Check to see if Point is within Rect on Rect or outside Rect - orentation method
        /// Uses IsLeft/Orientation checking 4 times
        /// if all is going left, then point is within rect, 
        /// checking for left or right depending on order of points supplied to isleft/orientation function
        /// </summary>
        public static int IsPointInRectOrientation(Vector2 targetCenter, Vector2 size, float rotation, Vector2 point)
        {
            //anticlockwise
            var p1 = targetCenter + new Vector2(-size.x, size.y) / 2;//tl
            var p2 = targetCenter + new Vector2(-size.x, -size.y) / 2;//bl
            var p3 = targetCenter + new Vector2(size.x, -size.y) / 2;//br
            var p4 = targetCenter + new Vector2(size.x, size.y) / 2;//tr

            //rotate points
            var cos = Mathf.Cos(rotation);
            var sin = Mathf.Sin(rotation);
            p1 -= targetCenter;
            p2 -= targetCenter;
            p3 -= targetCenter;
            p4 -= targetCenter;
            p1 = new Vector2(p1.x * cos - p1.y * sin, p1.x * sin + p1.y * cos);
            p2 = new Vector2(p2.x * cos - p2.y * sin, p2.x * sin + p2.y * cos);
            p3 = new Vector2(p3.x * cos - p3.y * sin, p3.x * sin + p3.y * cos);
            p4 = new Vector2(p4.x * cos - p4.y * sin, p4.x * sin + p4.y * cos);
            p1 += targetCenter;
            p2 += targetCenter;
            p3 += targetCenter;
            p4 += targetCenter;

            float v1 = Orientation(p1, p2, point);
            float v2 = Orientation(p2, p3, point);
            float v3 = Orientation(p3, p4, point);
            float v4 = Orientation(p4, p1, point);

            if (v1 == -1 || v2 == -1 || v3 == -1 || v4 == -1) return -1;// outside rect
            return (int)(v1 * v2 * v3 * v4);// 0 or 1 , on rect or inside rect
        }


        /// <summary>
        /// Check to see if two lines intercept each other
        /// Works for Parallel or non Parallel lines
        /// Done by formulating parametric equation of 
        /// Line segment 'L1 = l1p1+u(l1p2-l1p1)'
        /// Line segment 'L2 = l2p1+v(l2p2-l2p1)'
        /// Interceting point is where L1 == L2
        /// http://thirdpartyninjas.com/blog/2008/10/07/line-segment-intersection/
        /// Faster Version,
        /// if lines are not parrelel, returns the interception point
        /// if lines are parrerl, returns interception line segment if any
        /// </summary>
        public static (bool IsParallel,bool IsIntercept,Vector2,Vector2) IsLinesIntercept(Vector2 line1p1, Vector2 line1p2, Vector2 line2p1, Vector2 line2p2)
        {
            Vector2 interceptionpoint = Vector2.zero;

            float denominator = (line2p2.y - line2p1.y) * (line1p2.x - line1p1.x) - (line2p2.x - line2p1.x) * (line1p2.y - line1p1.y);

            //Step 1. Make sure the denominator is not 0, otherwise, the lines are parallel
            if (denominator != 0)
            {
                //Step 2. calculate the u and v values to make sure they are within range of (0-1)
                float u = ((line2p2.x - line2p1.x) * (line1p1.y - line2p1.y) - (line2p2.y - line2p1.y) * (line1p1.x - line2p1.x)) / denominator;
                float v = ((line1p2.x - line1p1.x) * (line1p1.y - line2p1.y) - (line1p2.y - line1p1.y) * (line1p1.x - line2p1.x)) / denominator;
                bool withinrange = u >= 0f && u <= 1f && v >= 0f && v <= 1f;

                //Step 3. Calculate interceptionpoint if within range
                if (withinrange) interceptionpoint = line1p1 + u * (line1p2 - line1p1);

                return (false,withinrange, interceptionpoint,Vector2.zero);
            }
            //Step Ex1. If lines are parallel,
            else
            {
                //Step Ex 2. Check if lines are colinear
                bool isColinear = Orientation(line1p1, line1p2, line2p1) * Orientation(line1p1, line1p2, line2p2) == 0;
                if (!isColinear) return (true, false, Vector2.zero, Vector2.zero);

                //Step Ex 3. There is either zero or two points that are within the lines,
                //Check which two points is within if any
                int i = 0;
                Vector2[] pointWithin = new Vector2[2];
                if (IsPointBetweenColinearLine(line2p1, line2p2, line1p1)) //line1p1
                {
                    pointWithin[i++] = line1p1;
                }
                if (IsPointBetweenColinearLine(line2p1, line2p2, line1p2)) //line1p2
                {
                    pointWithin[i++] = line1p2;
                    if (i == 2) return (true, true, pointWithin[0], pointWithin[1]); //Check for early exit
                }
                if (IsPointBetweenColinearLine(line1p1, line1p2, line2p1)) //line2p1
                {
                    pointWithin[i++] = line2p1;
                    if (i == 2) return (true, true, pointWithin[0], pointWithin[1]); //Check for early exit
                }
                if (IsPointBetweenColinearLine(line1p1, line1p2, line2p2)) //line2p2
                {
                    pointWithin[i++] = line2p2;
                    return (true, true, pointWithin[0], pointWithin[1]);
                }

                return (true, false, Vector2.zero, Vector2.zero);
            }
            
        }

        /// <summary>
        /// Check to see if two lines intercept each other
        /// Done by Checking Orientation of the lines to each other points
        /// https://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/
        /// Slower Version, does not return intercept point
        /// </summary>
        public static bool IsLinesIntercept2(Vector2 line1p1, Vector2 line1p2, Vector2 line2p1, Vector2 line2p2)
        {
            //step 1. check using one the two points of one line, check the orintation to each point on the other line
            int o1 = Orientation(line1p1, line1p2, line2p1);
            int o2 = Orientation(line1p1, line1p2, line2p2);

            //step 2. check using the other two points of the other line, check the orintation to each point on the other line
            int o3 = Orientation(line2p1, line2p2, line1p1);
            int o4 = Orientation(line2p1, line2p2, line1p2);

            //Step 3. Lines intercept if line orintation checks is different (have different orientations)
            if (o1 != o2 && o3 != o4) return true;

            //Step 4. Check Special Cases when points are collinear
            if (o1 == 0 && IsPointBetweenColinearLine(line1p1, line1p2, line2p1)) return true;
            if (o2 == 0 && IsPointBetweenColinearLine(line1p1, line1p2, line2p2)) return true;

            if (o3 == 0 && IsPointBetweenColinearLine(line2p1, line2p2, line1p1)) return true;
            if (o4 == 0 && IsPointBetweenColinearLine(line2p1, line2p2, line1p2)) return true;

            return false; 
        }


        /// <summary>
        /// Odd-Even Rule
        /// LineIntersection Count Rule
        /// Uses an additional known "pointOutside"
        /// Special case is handeled for parrarel line intersection
        /// </summary>
        public static bool IsPointInPolygon(List<Vector2> polygon, Vector2 point, Vector2 pointOutside) 
        {
            int intersectionCount = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                (Vector2 p1, Vector2 p2) line = (polygon[i], polygon[(i + 1) % polygon.Count]);

                (Vector2 p1, Vector2 p2) intersectingLine = (point, pointOutside);

                (bool isParallel, bool isIntersect,Vector2 _, Vector2 _) = IsLinesIntercept(line.p1, line.p2, intersectingLine.p1, intersectingLine.p2);

                if (isParallel)
                {
                    return IsPointBetweenColinearLine(line.p1, line.p2, point);
                }

                if (isIntersect) intersectionCount++;
            }
            return intersectionCount % 2 != 0;
        }

        /// <summary>
        /// Non-Zero Winding Rule
        /// LineIntersection Gradient/Normal Rule
        /// Similar to Odd-Even Rule but checks direction(winding) of polygon
        /// use dotproduct to get signed angle of vectors to determine direction using sign
        /// </summary>
        public static bool IsPointInPolygon2(List<Vector2> polygon, Vector2 point, Vector2 pointOutside)
        {
            int windingCount = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                (Vector2 p1, Vector2 p2) line = (polygon[i], polygon[(i + 1) % polygon.Count]);

                (Vector2 p1, Vector2 p2) intersectingLine = (point, pointOutside);

                (bool isParallel, bool isIntersect, Vector2 _, Vector2 _) = IsLinesIntercept(line.p1, line.p2, intersectingLine.p1, intersectingLine.p2);

                if (isParallel)
                {
                    return IsPointBetweenColinearLine(line.p1, line.p2, point);
                }

                if (isIntersect)
                {
                    var direction = Orientation(intersectingLine.p1, intersectingLine.p2, line.p2);
                    if (direction > 0) windingCount++;
                    else if (direction < 0) windingCount--;
                }
            }
            return windingCount != 0;
        }

        /// <summary>
        /// Check if poylgon is convex.
        /// Check to see if total Angle adds to 360
        /// </summary>
        public static bool IsPolygonConvex(List<Vector2> polygon)
        {
            float totalAngle = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                int index = i - 1;
                if (index < 0) index += polygon.Count;
                var prevIndex = index;

                var currentIndex = i;
                var nextIndex = (i + 1) % polygon.Count;
                totalAngle += Vector2.SignedAngle(polygon[prevIndex] - polygon[currentIndex], polygon[currentIndex] - polygon[nextIndex]);
            }
            
            return Mathf.Approximately(totalAngle, 360);
        }

        /// <summary>
        /// Returns if p is inside circumcircle formed by 3 points
        /// https://stackoverflow.com/questions/39984709/how-can-i-check-wether-a-point-is-inside-the-circumcircle-of-3-points
        /// p1, p2, p3 are in to be in clockwise order
        /// </summary>
        public static bool InsideCircumcircle(Vector2 p, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var circle = GetCircumcircle(p1, p2, p3);
            var dx = circle.Center.x - p.x;
            var dy = circle.Center.y - p.y;

            return Mathf.Sqrt(dx * dx + dy * dy) <= circle.Radius;
        }


        /// <summary>
        /// Returns if p is inside circumcircle formed by 3 points
        /// https://stackoverflow.com/questions/39984709/how-can-i-check-wether-a-point-is-inside-the-circumcircle-of-3-points
        /// p1, p2, p3 are in to be in clockwise order
        /// </summary>
        public static bool InsideCircumcircle2(Vector2 p, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var ax = p1.x - p.x;
            var ay = p1.y - p.y;
            var bx = p2.x - p.x;
            var by = p2.y - p.y;
            var cx = p3.x - p.x;
            var cy = p3.y - p.y;

            var det = (ax * ax + ay * ay) * (bx * cy - cx * by) -
                        (bx * bx + by * by) * (ax * cy - cx * ay) +
                        (cx * cx + cy * cy) * (ax * by - bx * ay);

            // if p1, p2, p3 are in counter clockwise
            // if det == 0 , point is on circumcircle
            // if det > 0 , point is inside circumcircle
            // if det < 0 , point is outside circumcircle

            return det < 0f;
        }

        /// <summary>
        /// Returns the circumcenter of 3 points
        /// Using distance of center to each point formula to get two linear equations
        /// Using Cramer's Rule to reduce the equation
        /// </summary>
        public static Vector2 GetCircumcenter(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float D = 2 * (p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y));
            float A = p1.x * p1.x + p1.y * p1.y;
            float B = p2.x * p2.x + p2.y * p2.y;
            float C = p3.x * p3.x + p3.y * p3.y;

            float x = ((A * (p2.y - p3.y) + B * (p3.y - p1.y) + C * (p1.y - p2.y)) / D);
            float y = ((A * (p3.x - p2.x) + B * (p1.x - p3.x) + C * (p2.x - p1.x)) / D);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns the circumcenter of 3 points
        /// Using distance of center to each point formula to get two linear equations
        /// Using Cramer's Rule to reduce the equation
        /// Different Variation
        /// </summary>
        public static Vector2 GetCircumcenter2(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float determinant = 2 * ((p1.x - p2.x) * (p1.y - p3.y) - (p1.x - p3.x) * (p1.y - p2.y));
            float A = p1.x * p1.x + p1.y * p1.y;
            float B = p2.x * p2.x + p2.y * p2.y;
            float C = p3.x * p3.x + p3.y * p3.y;

            float x = ((A - B) * (p1.y - p3.y) - (A - C) * (p1.y - p2.y)) / determinant;
            float y = ((A - C) * (p1.x - p2.x) - (A - B) * (p1.x - p3.x)) / determinant;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns Circle from 3 points (circumcircle)
        /// </summary>
        public static Circle GetCircumcircle(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var center = GetCircumcenter(p1, p2, p3);
            var radius = Vector3.Distance(center, p1);
            return new Circle(center, radius);
        }
        /// <summary>
        /// Returns Circle from 2 points (circumcircle)
        /// </summary>
        public static Circle GetCircleFromPoints(Vector2 p1, Vector2 p2)
        {
            Vector2 center = (p1 + p2) / 2;
            float radius = Vector2.Distance(p1, p2) / 2;
            return new Circle(center, radius);
        }
    }
}