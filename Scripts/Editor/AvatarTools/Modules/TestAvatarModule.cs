using Pumkin.Core;
using Pumkin.Core.Helpers;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;


namespace Pumkin.AvatarTools2.Modules
{
    [AutoLoad(DefaultIDs.Modules.TestAvatar)]
    class TestAvatarModule : UIModuleBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Avatar Testing", 4);
    }
}
