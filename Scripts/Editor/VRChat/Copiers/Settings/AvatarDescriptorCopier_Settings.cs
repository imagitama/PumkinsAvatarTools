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
        protected override bool ShowRemoveAll => true;

        new void Awake()
        {
            onlyAllowOneComponentOfType = true;
        }

        public override PropertyDefinitions Properties => _props;

        PropertyDefinitions _props = new PropertyDefinitions(new Dictionary<Type, PropertyGroup[]>
        {
            { VRChatTypes.VRC_AvatarDescriptor, new PropertyGroup[]
                {
                    new PropertyGroup("Settings",
                        "Name",
                        "ScaleIPD",
                        "portraitCameraPositionOffset",
                        "portraitCameraRotationOffset",
                        "enableEyeLook",
                        "autoFootsteps",
                        "autoLocomotion"),
                    new PropertyGroup("Animations",
                        "Animations",
                        "customizeAnimationLayers",
                        "baseAnimationLayers",
                        "specialAnimationLayers",
                        "AnimationPreset"),
                    new PropertyGroup("Lipsync",
                        "lipSync",
                        "MouthOpenBlendShapeName",
                        "lipSyncJawBone",
                        "VisemeSkinnedMesh",
                        "lipSyncJawClosed",
                        "lipSyncJawOpen",
                        "VisemeBlendShapes"),
                    new PropertyGroup("Viewpoint",
                        "ViewPosition"),
                }
            },
            { VRChatTypes.PipelineManager, new PropertyGroup[]
              {
                new PropertyGroup("Blueprint ID", "blueprintId")
              }
            }
        });
    }
}