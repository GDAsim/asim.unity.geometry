using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    [ExecuteInEditMode]
    public class IsPointBetweenColinearLine_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject Point;
        [SerializeField] GameObject Line_P1;
        [SerializeField] GameObject Line_P2;

        void OnGUI()
        {
            //Convert World Pos to Screen Pos
            Vector3 screen_Line_P1_pos = UnityOnGUIHelper.WorldToScreenPos(cam, Line_P1.transform.position);
            Vector3 screen_Line_P2_pos = UnityOnGUIHelper.WorldToScreenPos(cam, Line_P2.transform.position);

            //Draw Line P1P2
            UnityOnGUIHelper.DrawLine(screen_Line_P1_pos, screen_Line_P2_pos, Color.red, 2);

            //Update Text
            bool isPointcolinear = GeometryUtils.IsPointColinear2(Line_P1.transform.position, Line_P2.transform.position, Point.transform.position);
            if(!isPointcolinear)
            {
                Text.text = "Point Is Not Colinear";
                return;
            }

            bool IsPointBetweenLine = GeometryUtils.IsPointBetweenColinearLine(Line_P1.transform.position, Line_P2.transform.position, Point.transform.position);
            if (IsPointBetweenLine)
            {
                Text.text = "Point Is Between Colinear Line";
            }
            else 
            {
                Text.text = "Point Is Not Between Colinear Line";
            }
            
        }
    }
}