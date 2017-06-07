using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStatics
{
    public static class C_FunctionLibrary
    {
        public static bool IsValidIndex<T>(this List<T> inList, int index)
        {
            if(inList != null)
            {
                return index <= inList.Count - 1 && index >= 0;
            }

            return false;
        }
    }
}