using UnityEngine;

namespace NP
{
    public static class TransformTool
    {
        public static void DeleteAllChildren(this Transform t)
        {
            for (int i = t.childCount - 1; i >= 0; i--)
            {
                if (Application.isEditor)
                {
                    Object.DestroyImmediate(t.GetChild(i).gameObject);
                }
                else
                {
                    Object.Destroy(t.GetChild(i).gameObject);
                }
            }
        }
    }
}