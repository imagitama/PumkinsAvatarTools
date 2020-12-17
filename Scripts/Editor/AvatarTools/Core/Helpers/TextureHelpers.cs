using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.Core.Helpers
{
    public static class TextureHelpers
    {
        public static Texture2D GetReadableTextureCopy(Texture2D tex)
        {
            byte[] pix = tex.GetRawTextureData();
            Texture2D finalTex = new Texture2D(tex.width, tex.height, tex.format, false);
            finalTex.LoadRawTextureData(pix);
            finalTex.Apply();
            return finalTex;
        }
    }
}
