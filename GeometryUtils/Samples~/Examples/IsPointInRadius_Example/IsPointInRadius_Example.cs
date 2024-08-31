using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    [ExecuteInEditMode]
    public class IsPointInRadius_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject TargetCenter;
        [SerializeField] float CircleRadius;

        [SerializeField] GameObject Point;

        void Update()
        {
            var newpos = new Vector3(Mathf.PingPong(Time.unscaledTime * 8, CircleRadius * 2.4f) - CircleRadius * 1.2f, 0, 0);
            Point.transform.position = newpos;
        }
        void OnGUI()
        {
            //Convert World Pos to Screen Pos
            Vector3 screen_centerpos = UnityOnGUIHelper.WorldToScreenPos(cam, TargetCenter.transform.position);
            Vector3 screen_pointpos = UnityOnGUIHelper.WorldToScreenPos(cam, Point.transform.position);
            Vector3 radius = UnityOnGUIHelper.WorldToScreenPos(cam, TargetCenter.transform.position + new Vector3(CircleRadius, 0, 0));

            var GUICircleRadius = Vector3.Distance(screen_centerpos, radius);

            //Draw Ellipse
            UnityOnGUIHelper.DrawEllipse(screen_centerpos, new Vector2(GUICircleRadius, GUICircleRadius), 0, new Color32(0, 255, 0, 255), Color.red, 1);

            //Update Text
            int IsPointInRadius = GeometryUtils.IsPointInRadius(screen_centerpos, GUICircleRadius, screen_pointpos);
            if (IsPointInRadius == 1)
            {
                Text.text = "Point Inside Center Radius";
            }
            else if (IsPointInRadius == -1)
            {
                Text.text = "Point Outside Center Radius";
            }
            else
            {
                Text.text = "Point On Center Radius";
            }
        }
    }
}
