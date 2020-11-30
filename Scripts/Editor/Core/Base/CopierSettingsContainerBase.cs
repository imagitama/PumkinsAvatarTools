using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pumkin.AvatarTools.Base
{
    [Serializable]
    public class CopierSettingsContainerBase : SettingsContainerBase
    {//TODO: Figure out a way to hide these
        [DrawToggleLeft]
        public bool removeAllBeforeCopying = false;
        [DrawToggleLeft]
        public bool createGameObjects = false;
    }
}
