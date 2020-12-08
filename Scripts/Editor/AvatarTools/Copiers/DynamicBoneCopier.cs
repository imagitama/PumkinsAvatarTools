using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Destroyers;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.Modules;
using Pumkin.AvatarTools.Settings;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.DynamicBone, ParentModuleID = DefaultIDs.Modules.Copier)]
    class DynamicBoneCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => "DynamicBone";
        public override ISettingsContainer Settings => settings;

        DynamicBoneCopier_Settings settings;

        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Dynamic Bone", 1);

        protected override bool DoCopyComponents(GameObject objFrom, GameObject objTo)
        {
            if(settings.removeAllBeforeCopying)
            {
                var des = new DynamicBoneDestroyer();
                if(!des.TryDestroyComponents(objTo))
                {
                    PumkinTools.LogError($"Unable to remove {ComponentType.Name} components from {objTo.name}. Aborting copy");
                    return false;
                }
            }
            return base.DoCopyComponents(objFrom, objTo);
        }

        protected override void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<DynamicBoneCopier_Settings>();
        }
    }
}
