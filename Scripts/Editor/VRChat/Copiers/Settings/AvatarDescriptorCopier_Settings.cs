using Pumkin.AvatarTools2.Settings;
using Pumkin.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Pumkin.AvatarTools2.VRChat.Settings
{
    class AvatarDescriptorCopier_Settings : CopierSettingsContainerBase
    {
        [Space]
        [DrawToggleLeft] public bool settings = false;
        [DrawToggleLeft] public bool lipsync = false;
        [DrawToggleLeft] public bool viewpoint = false;
        [DrawToggleLeft] public bool animations = false;
        [Space]
        [DrawToggleLeft] public bool pipelineID = false;

        public override string[] PropertyNames
        {
            get
            {
                var names = new List<string>();
                if(settings)
                    names.AddRange(new string[]
                    {
                        "Name", "ScaleIPD", "portraitCameraPositionOffset", "portraitCameraRotationOffset",
                        "enableEyeLook", "autoFootsteps", "autoLocomotion"
                    });

                if(animations)
                    names.AddRange(new string[]
                    {
                        "Animations", "customizeAnimationLayers", "baseAnimationLayers",
                        "specialAnimationLayers", "AnimationPreset"
                    });

                if(lipsync)
                    names.AddRange(new string[]
                    {
                        "lipSync", "MouthOpenBlendShapeName", "lipSyncJawBone", "VisemeSkinnedMesh", "lipSyncJawClosed", "lipSyncJawOpen", "VisemeBlendShapes"
                    });

                if(viewpoint)
                    names.Add("ViewPosition");


                return names.ToArray();
            }
        }
    }
}
