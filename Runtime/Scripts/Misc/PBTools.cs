using System;
using System.Collections.Generic;
using pbc = global::Google.Protobuf.Collections;

namespace NP
{
    public static class PBTools
    {
        public static void CopyToList(this pbc::RepeatedField<uint> src, List<int> dst)
        {
            dst.Clear();
            foreach (var item in src)
            {
                dst.Add((int)item);
            }
        }

        public static int[] ToArray(this pbc::RepeatedField<uint> src)
        {
            var dst = new int[src.Count];
            for (int i = 0; i < src.Count; i++)
            {
                dst[i] = (int)src[i];
            }

            return dst;
        }

        public static void ToList(this pbc::RepeatedField<uint> src, List<int> dst)
        {
            for (int i = 0; i < src.Count; i++)
            {
                dst.Add((int)src[i]);
            }
        }
    }
}