using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Implementation.Destroyers;
using Pumkin.AvatarTools.Implementation.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Copiers
{
    [AutoLoad("copier_dbones", ParentModuleID = DefaultModuleIDs.COPIER)]
    [UIDefinition("Dynamic Bones", OrderInUI = 1)]
    class DynamicBoneCopier : ComponentCopierBase
    {
        public override string ComponentTypeNameFull { get => "DynamicBone"; }
        public override SettingsContainerBase Settings => settings;

        DynamicBoneCopier_Settings settings;

        protected override bool DoCopy(GameObject objFrom, GameObject objTo)
        {
            if(settings.removeOldBones)
            {
                var des = new DynamicBoneDestroyer();
                if(!des.TryDestroyComponents(objTo))
                {
                    PumkinTools.LogError($"Unable to remove {ComponentType.Name} components from {objTo.name}. Aborting copy");
                    return false;
                }
            }
            return base.DoCopy(objFrom, objTo);
        }

        protected override void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<DynamicBoneCopier_Settings>();
        }
    }
}
