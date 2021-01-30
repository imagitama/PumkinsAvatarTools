#if UNITY_EDITOR
using Pumkin.AvatarTools2.Settings;
using Pumkin.AvatarTools2.VRChat.Copiers;
using Pumkin.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Settings
{
    [CustomSettingsContainer(typeof(AvatarDescriptorCopier))]
    class AvatarDescriptorCopier_Settings : CopierSettingsContainerBase
    {
        protected override bool ShowCreateGameObjects => false;
        protected override bool ShowRemoveAll => false;

        [Space]
        [DrawToggleLeft] public bool settings = false;
        [DrawToggleLeft] public bool animations = false;
        [DrawToggleLeft] public bool lipsync = false;
        [DrawToggleLeft] public bool viewpoint = false;
        [Space]
        [DrawToggleLeft] public bool pipelineID = false;

        public override PropertyDefinitions Properties => new PropertyDefinitions(
            new PropertyGroup("settings", "Name", "ScaleIPD", "portraitCameraPositionOffset", "portraitCameraRotationOffset",
                        "enableEyeLook", "autoFootsteps", "autoLocomotion"),
            new PropertyGroup("animations", "Animations", "customizeAnimationLayers", "baseAnimationLayers",
                "specialAnimationLayers", "AnimationPreset"),
            new PropertyGroup("lipsync", "lipSync", "MouthOpenBlendShapeName", "lipSyncJawBone",
                "VisemeSkinnedMesh", "lipSyncJawClosed", "lipSyncJawOpen", "VisemeBlendShapes"),
            new PropertyGroup("viewpoint", "ViewPosition")
        );

        //public override string[] PropertyNames
        //{
        //    get
        //    {
        //        var names = new List<string>();
        //        if(settings)
        //            names.AddRange(new string[]
        //            {
        //                "Name", "ScaleIPD", "portraitCameraPositionOffset", "portraitCameraRotationOffset",
        //                "enableEyeLook", "autoFootsteps", "autoLocomotion"
        //            });

        //        if(animations)
        //            names.AddRange(new string[]
        //            {
        //                "Animations", "customizeAnimationLayers", "baseAnimationLayers",
        //                "specialAnimationLayers", "AnimationPreset"
        //            });

        //        if(lipsync)
        //            names.AddRange(new string[]
        //            {
        //                "lipSync", "MouthOpenBlendShapeName", "lipSyncJawBone", "VisemeSkinnedMesh", "lipSyncJawClosed", "lipSyncJawOpen", "VisemeBlendShapes"
        //            });

        //        if(viewpoint)
        //            names.Add("ViewPosition");

        //        return names.ToArray();
        //    }
        //}
    }
}
#endif