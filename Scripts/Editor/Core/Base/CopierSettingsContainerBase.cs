using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Base
{
    [Serializable]
    public class CopierSettingsContainerBase : SettingsContainerBase
    {
        protected bool ShowRemoveAll = false;
        protected bool ShowCreateGameObjects = false;

        //TODO: Figure out what to do with these
        //[ConditionalHide("ShowRemoveAll", HideInInspector = true)]
        [DrawToggleLeft] public bool removeAllBeforeCopying = false;

        //[ConditionalHide(nameof(ShowCreateGameObjects), HideInInspector = true)]
        [DrawToggleLeft] public bool createGameObjects = false;

        public virtual string[] PropertyNames => null;
    }

    //TODO: Figure this out
    //[Serializable]
    //public class MultiCopierSettingsContainerBase : SettingsContainerBase
    //{
    //    protected bool ShowRemoveAll = false;
    //    protected bool ShowCreateGameObjects = false;

    //    [DrawToggleLeft] public bool removeAllBeforeCopying = false;
    //    [DrawToggleLeft] public bool createGameObjects = false;

    //    public virtual string[] PropertyNames => null;
    //}
}
