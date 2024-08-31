using UnityEngine;
using UnityEngine.UI;
using asim.unity.helpers;

namespace asim.unity.utils.geometry
{
    [ExecuteInEditMode]
    public class IsPointInRect_Example : MonoBehaviour
    {
        [SerializeField] Text Text;

        [SerializeField] Camera cam;

        [SerializeField] GameObject TargetCenter;
        [SerializeField] Vector2 Size = Vector2.one;

        [SerializeField] GameObject Point;

        float rotation = 0;
        void OnGUI()
        {
            //Convert World Pos to GUI Pos
            Vector3 screen_centerpos = UnityOnGUIHelper.WorldToScreenPos(cam, TargetCenter.transform.position);
            Vector3 screen_pointpos = UnityOnGUIHelper.WorldToScreenPos(cam, Point.transform.position);

            Vector3 sizeX = UnityOnGUIHelper.WorldToScreenPos(cam, TargetCenter.transform.position + new Vector3(Size.x, 0, 0));
            Vector3 sizeY = UnityOnGUIHelper.WorldToScreenPos(cam, TargetCenter.transform.position + new Vector3(0, Size.y, 0));
            var GUISize = new Vector2(Vector3.Distance(screen_centerpos, sizeX), Vector3.Distance(screen_centerpos, sizeY));

            rotation += 0.003f;

            //Draw Rect
            UnityOnGUIHelper.DrawRect(screen_centerpos, GUISize, rotation, new Color32(0, 0, 0, 0), Color.black, true, 1);

            //Update Text
            int IsPointInRect = GeometryUtils.IsPointInRectOrientation(screen_centerpos, GUISize, rotation, screen_pointpos);
            if (IsPointInRect == 1) 
            {
                Text.text = "Point Inside Rect";
            } 
            else if (IsPointInRect == -1) 
            {
                Text.text = "Point Outside Rect";
            } 
            else
            {
                Text.text = "Point On Rect";
            } 
        }
    }
}
