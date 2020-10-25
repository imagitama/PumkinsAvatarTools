#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.Core.Helpers
{
    public static class Vector3Helpers
    {
        public static Vector3 RoundVectorValues(Vector3 v, int decimals)
        {
            return new Vector3((float)Math.Round(v.x, decimals), (float)Math.Round(v.y, decimals), (float)Math.Round(v.z, decimals));
        }

        public static Vector3 ScaleVector(Vector3 vec, float deltaScale)
        {
            return new Vector3(vec.x + deltaScale, vec.y + deltaScale, vec.z + deltaScale);
        }
    }
}
#endif