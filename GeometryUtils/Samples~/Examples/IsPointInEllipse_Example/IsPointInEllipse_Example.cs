using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    [ExecuteInEditMode]
    public class IsPointInEllipse_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject Point;
        [SerializeField] GameObject EllipseCenter;
        [SerializeField] Vector2 EllipseSize;

        public bool DoRotate;

        float rotation = 0;
        void OnGUI()
        {
            //Convert World Pos to Screen Pos
            Vector3 screen_centerpos = UnityOnGUIHelper.WorldToScreenPos(cam, EllipseCenter.transform.position);
            Vector3 screen_pointpos = UnityOnGUIHelper.WorldToScreenPos(cam, Point.transform.position);

            Vector3 radiusX = UnityOnGUIHelper.WorldToScreenPos(cam, EllipseCenter.transform.position + new Vector3(EllipseSize.x, 0, 0));
            Vector3 radiusY = UnityOnGUIHelper.WorldToScreenPos(cam, EllipseCenter.transform.position + new Vector3(0, EllipseSize.y, 0));
            var GUIEllipseSize = new Vector2(Vector3.Distance(screen_centerpos, radiusX), Vector3.Distance(screen_centerpos, radiusY));

            if(DoRotate)
            {
                rotation += 0.003f;
            }

            //Draw Ellipse
            UnityOnGUIHelper.DrawEllipse(screen_centerpos, GUIEllipseSize, rotation, new Color32(0, 255, 0, 255), Color.red, 1);

            //Update Text
            int IsPointInEllipse = GeometryUtils.IsPointInEllipse(screen_centerpos, GUIEllipseSize, rotation, screen_pointpos);
            if (IsPointInEllipse == 1)
            {
                Text.text = "Point Inside Ellipse";
            }
            else if (IsPointInEllipse == -1)
            {
                Text.text = "Point Outside Ellipse";
            }
            else
            {
                Text.text = "Point On Ellipse";
            }
        }
    }
}
