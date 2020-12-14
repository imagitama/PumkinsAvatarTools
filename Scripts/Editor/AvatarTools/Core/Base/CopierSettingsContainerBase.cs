using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Settings
{
    [Serializable]
    public class CopierSettingsContainerBase : SettingsContainerBase
    {
        protected virtual bool ShowRemoveAll { get; } = true;
        protected virtual bool ShowCreateGameObjects { get; } = true;

        [ConditionalHide(nameof(ShowRemoveAll))]
        public bool removeAllBeforeCopying = false;

        [ConditionalHide(nameof(ShowCreateGameObjects))]
        public bool createGameObjects = false;

        public virtual string[] PropertyNames => null;
    }
}
