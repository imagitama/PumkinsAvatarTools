#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.Core
{
    public static class Delegates
    {
        public delegate void SelectedChangeHandler(GameObject newSelection);
        public delegate void StringChangeHandler(string newString);
    }
}
#endif