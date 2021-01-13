namespace Pumkin.AvatarTools2
{
    public static class DefaultIDs
    {
        public static class Modules
        {
            public const string Tools = "tools";
            public const string Tools_SetupAvatar = "tools_setupAvatar";
            public const string Tools_FixAvatar = "tools_fixAvatar";

            public const string Destroyer = "destroyers";

            public const string Copier = "copiers";

            public const string TestAvatar = "testAvatar";

            public const string Debug = "debug";
        }

        public static class Tools
        {
            public const string EditScale = "tool_editScale";
            public const string RemoveMissingScripts = "tool_removeMissingScripts";
            public const string SetRendererAchors = "tool_setRendererAnchors";
            public const string ResetScale = "tool_resetScale";
            public const string ResetPose = "tool_resetPose";
            public const string SetTPose = "tool_setTPose";
            public const string ZeroBlendshapes = "tool_zeroBlendshapes";
            public const string EditViewpoint = "tool_editViewpoint";
            public const string SetupLipsync = "tool_setupLipsync";
            public const string LookAtMouseToggle = "tool_lookAtMouseToggle";
            public const string TestAnimations = "tool_testAnimations";
            public const string ClearVertexColors = "tool_clearVertexColors";

        }

        public static class Copiers
        {
            public const string DynamicBone = "copier_dynamicBone";
            public const string DynamicBoneCollider = "copier_dynamicBoneCollider";
            public const string Light = "copier_light";
            public const string SkinnedMeshRenderer = "copier_skinnedMeshRenderer";
            public const string Collider = "copier_collider";
            public const string AvatarDescriptor = "copier_avatarDescriptor";
            public const string ParentConstraint = "copier_parentConstraint";
            public const string ScaleConstraint = "copier_scaleConstraint";
            public const string RotationConstraint = "copier_rotationConstraint";
            public const string PositionConstraint = "copier_positionConstraint";
            public const string AimConstraint = "copier_aimConstraint";
            public const string LookAtConstraint = "copier_lookAtConstraint";
            public const string AudioSource = "copier_audioSource";
        }

        public static class Destroyers
        {
            public const string DynamicBone = "destroyer_dynamicBone";
            public const string DynamicBoneCollider = "destroyer_dynamicBoneCollider";
            public const string Light = "destroyer_light";
            public const string ParticleSystem = "destroyer_particleSystem";
            public const string Collider = "destroyer_collider";
            public const string Constraint = "destroyer_constraint";
        }
    }
}
