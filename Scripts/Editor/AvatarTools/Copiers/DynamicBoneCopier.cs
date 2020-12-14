using Pumkin.AvatarTools2.Destroyers;
using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Settings;
using Pumkin.Core;
using UnityEngine;

namespace Pumkin.AvatarTools2.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.DynamicBone, ParentModuleID = DefaultIDs.Modules.Copier)]
    class DynamicBoneCopier : ComponentCopierBase
    {
        public override string ComponentTypeFullName => GenericTypes.DynamicBone?.FullName;
        public override ISettingsContainer Settings => settings;

        DynamicBoneCopier_Settings settings;

        //TODO: Remove this once the checkbox for removing works
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
