using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    [ExecuteInEditMode]
    public class IsLinesIntercept_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject Line1_P1;
        [SerializeField] GameObject Line1_P2;
        [SerializeField] GameObject Line2_P1;
        [SerializeField] GameObject Line2_P2;

        void OnGUI()
        {
            //Convert World Pos to Screen Pos
            Vector3 screen_Line1_P1_pos = UnityOnGUIHelper.WorldToScreenPos(cam, Line1_P1.transform.position);
            Vector3 screen_Line1_P2_pos = UnityOnGUIHelper.WorldToScreenPos(cam, Line1_P2.transform.position);
            Vector3 screen_Line2_P1_pos = UnityOnGUIHelper.WorldToScreenPos(cam, Line2_P1.transform.position);
            Vector3 screen_Line2_P2_pos = UnityOnGUIHelper.WorldToScreenPos(cam, Line2_P2.transform.position);

            //Draw Lines
            UnityOnGUIHelper.DrawLine(screen_Line1_P1_pos, screen_Line1_P2_pos, Color.red, 2);
            UnityOnGUIHelper.DrawLine(screen_Line2_P1_pos, screen_Line2_P2_pos, Color.red, 2);

            (bool IsParallel,bool IsLinesIntercept, Vector2 interceptPoint,Vector2 p2) = GeometryUtils.IsLinesIntercept(
                Line1_P1.transform.position, Line1_P2.transform.position,
                Line2_P1.transform.position, Line2_P2.transform.position);

            //Update Text
            if (!IsParallel)
            {
                if (IsLinesIntercept)
                {
                    Vector3 screen_interceptpos = UnityOnGUIHelper.WorldToScreenPos(cam, interceptPoint);

                    UnityOnGUIHelper.DrawDot(screen_interceptpos, 5, 0, Color.green, Color.green);

                    Text.text = "Lines Do Intercept";
                }
                else
                {
                    Text.text = "Lines Do Not Intercept";
                }
            }
            else
            {
                if (IsLinesIntercept)
                {
                    Vector3 screen_InterceptSegmentP1 = UnityOnGUIHelper.WorldToScreenPos(cam, interceptPoint);
                    Vector3 screen_InterceptSegmentP2 = UnityOnGUIHelper.WorldToScreenPos(cam, p2);

                    UnityOnGUIHelper.DrawLine(screen_InterceptSegmentP1, screen_InterceptSegmentP2, Color.green, 5);

                    Text.text = "Lines Segment Intercept";
                }
                else
                {
                    Text.text = "Lines Do Not Intercept";
                }
            }
        }
    }
}