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
            for(int i = 0 ; i < src.Count ; i++)
            {
                dst[i] = (int)src[i];
            }

            return dst;
        }
        
        public static List<int> ToList(this pbc::RepeatedField<uint> src)
        {
            var dst = new List<int>(src.Count);
            for(int i = 0 ; i < src.Count ; i++)
            {
                dst.Add((int)src[i]);
            }

            return dst;
        }
        
        public static List<T> ToList<T>(this pbc::RepeatedField<T> src)
        {
            var dst = new List<T>(src.Count);
            for(int i = 0 ; i < src.Count ; i++)
            {
                dst.Add(src[i]);
            }

            return dst;
        }
    }
}