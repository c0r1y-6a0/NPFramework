using UnityEngine;

namespace NP
{
    public class RTUtility
    {
        public enum ElementKeepAxisStatic
        {
            None,
            X,
            Y
        }

        //set rt to the left bottom position of parentRT
        public static void SetLeftBottomPosition(RectTransform rt, RectTransform parentRT, Vector2 leftBottomPosition,
            ElementKeepAxisStatic keepAxisStaticMode = ElementKeepAxisStatic.None)
        {
            bool stretching = rt.anchorMin != rt.anchorMax;
            var parentSize = parentRT.rect.size;
            if (!stretching)
            {
                var anchorPoint = parentSize * rt.anchorMin;
                var size = rt.rect.size;
                var pivotPoint = size * rt.pivot;
                Vector2 newAnchoredPosition = Vector2.zero;

                switch (keepAxisStaticMode)
                {
                    case ElementKeepAxisStatic.None:
                        newAnchoredPosition = leftBottomPosition + pivotPoint - anchorPoint;
                        break;
                    case ElementKeepAxisStatic.X:
                        newAnchoredPosition = new Vector2(rt.anchoredPosition.x, leftBottomPosition.y + pivotPoint.y - anchorPoint.y);
                        break;
                    case ElementKeepAxisStatic.Y:
                        newAnchoredPosition = new Vector2(leftBottomPosition.x + pivotPoint.x - anchorPoint.x, rt.anchoredPosition.y);
                        break;
                }

                rt.anchoredPosition = newAnchoredPosition;
            }
            else
            {
                var size = rt.rect.size;
                var anchorMinPoint = parentSize * rt.anchorMin;
                var anchorMaxPoint = parentSize * rt.anchorMax;

                switch (keepAxisStaticMode)
                {
                    case ElementKeepAxisStatic.None:
                        rt.offsetMin = leftBottomPosition - anchorMinPoint;
                        rt.offsetMax = leftBottomPosition + size - anchorMaxPoint;
                        break;
                    case ElementKeepAxisStatic.X:
                        rt.offsetMin = new Vector2(rt.offsetMin.x, leftBottomPosition.y - anchorMinPoint.y);
                        rt.offsetMax = new Vector2(rt.offsetMax.x, leftBottomPosition.y + size.y - anchorMaxPoint.y);
                        break;
                    case ElementKeepAxisStatic.Y:
                        rt.offsetMin = new Vector2(leftBottomPosition.x - anchorMinPoint.x, rt.offsetMin.y);
                        rt.offsetMax = new Vector2(leftBottomPosition.x + size.x - anchorMaxPoint.x, rt.offsetMax.y);
                        break;
                }
            }
        }

        public static Vector2 GetLeftBottomPosition(RectTransform rt, RectTransform parentRT)
        {
            bool stretching = rt.anchorMin != rt.anchorMax;
            var parentSize = parentRT.rect.size;
            if (!stretching)
            {
                var anchorPoint = parentSize * rt.anchorMin;
                var size = rt.rect.size;
                var pivotPoint = size * rt.pivot;
                return rt.anchoredPosition + anchorPoint - pivotPoint;
            }

            var anchorMinPoint = parentSize * rt.anchorMin;
            return rt.offsetMin + anchorMinPoint;
        }
        
        public static Vector3 GetRelativePositionBetweenRectTransfroms(RectTransform rectTransform1, RectTransform rectTransform2)
        {
            Vector3 worldPosition = rectTransform1.TransformPoint(rectTransform1.rect.center);
            Vector3 relativePosition = rectTransform2.InverseTransformPoint(worldPosition);
            return relativePosition;
        }

        public static Vector3 GetWorldPosition(RectTransform parent, Vector2 childAnchorPoint, Vector2 childAnchorPosition)
        {
            Vector3[] corners = new Vector3[4];
            // corners[0] 是左下角的世界坐标
            // corners[1] 是左上角的世界坐标
            // corners[2] 是右上角的世界坐标
            // corners[3] 是右下角的世界坐标
            parent.GetWorldCorners(corners);

            Vector3 anchorPointWorldPosition = corners[0] + Vector3.Scale(corners[2] - corners[0] , new Vector3(childAnchorPoint.x, childAnchorPoint.y, 1));
            Vector2 anchorPointInParent = parent.InverseTransformPoint(anchorPointWorldPosition);
            Vector2 childPositionInParent = anchorPointInParent + childAnchorPosition;
            return parent.TransformPoint(childPositionInParent);
        }
    }
}