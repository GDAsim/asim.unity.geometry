using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    [ExecuteInEditMode]
    public class OrientationPoint_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject LineP1;
        [SerializeField] GameObject LineP2;
        [SerializeField] GameObject PointP;

        public enum OrientationMethod
        {
            CrossProduct,
            DotProduct,
            Area,
            Gradient
        }
        [SerializeField] OrientationMethod oreintationMethod;

        void OnGUI()
        {
            //Convert World Pos to Screen Pos
            Vector3 screen_p1 = UnityOnGUIHelper.WorldToScreenPos(cam, LineP1.transform.position);
            Vector3 screen_p2 = UnityOnGUIHelper.WorldToScreenPos(cam, LineP2.transform.position);

            //Draw Line p1p2
            UnityOnGUIHelper.DrawLine(screen_p1, screen_p2, Color.red, 8);

            //Update Text
            int orientation;
            if (oreintationMethod == OrientationMethod.CrossProduct)
            {
                orientation = GeometryUtils.Orientation(LineP1.transform.position, LineP2.transform.position, PointP.transform.position);
            }
            else if (oreintationMethod == OrientationMethod.DotProduct)
            {
                orientation = GeometryUtils.Orientation2(LineP1.transform.position, LineP2.transform.position, PointP.transform.position);
            }
            else if (oreintationMethod == OrientationMethod.Area)
            {
                orientation = GeometryUtils.Orientation3(LineP1.transform.position, LineP2.transform.position, PointP.transform.position);
            }
            else
            {
                orientation = GeometryUtils.Orientation4(LineP1.transform.position, LineP2.transform.position, PointP.transform.position);
            }

            if (orientation == 1)
            {
                Text.text = "Point Is Left Of Vector";
            }
            else if (orientation == -1)
            {
                Text.text = "Point Is Right Of Vector";
            }
            else
            {
                Text.text = "Point Parallel Of Vector";
            }
        }
    }
}
