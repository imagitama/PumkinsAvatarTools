#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Helpers
{
    public static class StringHelpers
    {
        public static string ToSpacedOutWords(string str)
        {
            if(string.IsNullOrWhiteSpace(str))
                return str;

            List<char> chars = str.ToCharArray().ToList();
            for(int i = chars.Count - 2; i >= 0; i--)            
            {
                if(char.IsUpper(str[i]))
                {
                    chars.Insert(i + 1, ' ');
                    for(; i >= 0; i--)
                    {
                        if(!char.IsUpper(str[i]))
                            break;
                    }
                }
            }

            return new string(chars.ToArray());
        }
    }
}
#endif