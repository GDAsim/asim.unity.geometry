using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    [ExecuteInEditMode]
    public class IsPointInTriangle_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject P1;
        [SerializeField] GameObject P2;
        [SerializeField] GameObject P3;

        [SerializeField] GameObject PointP;

        public enum PointTriangleMethod
        {
            Barycentric,
            Orientation,
            Area
        }
        [SerializeField] PointTriangleMethod oreintationMethod;

        void OnGUI()
        {
            //Convert World Pos to Screen Pos
            Vector3 screen_p1 = UnityOnGUIHelper.WorldToScreenPos(cam, P1.transform.position);
            Vector3 screen_p2 = UnityOnGUIHelper.WorldToScreenPos(cam, P2.transform.position);
            Vector3 screen_p3 = UnityOnGUIHelper.WorldToScreenPos(cam, P3.transform.position);

            //Draw Triangle
            UnityOnGUIHelper.DrawLine(screen_p1, screen_p2, Color.red, 8);
            UnityOnGUIHelper.DrawLine(screen_p2, screen_p3, Color.red, 8);
            UnityOnGUIHelper.DrawLine(screen_p3, screen_p1, Color.red, 8);

            //Update Text
            int IsPointInTriangle;
            if (oreintationMethod == PointTriangleMethod.Barycentric)
            {
                IsPointInTriangle = GeometryUtils.IsPointInTriangle(P1.transform.position, P2.transform.position, P3.transform.position, PointP.transform.position);
            }
            else if (oreintationMethod == PointTriangleMethod.Orientation)
            {
                IsPointInTriangle = GeometryUtils.IsPointInTriangleOrientation(P1.transform.position, P2.transform.position, P3.transform.position, PointP.transform.position);
            }
            else if (oreintationMethod == PointTriangleMethod.Area)
            {
                IsPointInTriangle = GeometryUtils.IsPointInTriangleArea(P1.transform.position, P2.transform.position, P3.transform.position, PointP.transform.position);
            }
            else
            {
                IsPointInTriangle = -1;
            }

            if (IsPointInTriangle == 1)
            {
                Text.text = "Point Is Inside Of Triangle";
            }
            else if (IsPointInTriangle == -1)
            {
                Text.text = "Point Is Outside Of Triangle";
            }
            else
            {
                Text.text = "Point Is On Triangle";
            }
        }
    }
}
