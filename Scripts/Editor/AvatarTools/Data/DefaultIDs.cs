namespace Pumkin.AvatarTools
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

            public const string Debug = "debug";
        }

        public static class Tools
        {
            public const string EditScale = "tools_editScale";
            public const string RemoveMissingScripts = "tools_removeMissingScripts";
            public const string SetRendererAchors = "tools_setRendererAnchors";
            public const string ResetScale = "tools_resetScale";
            public const string ResetPose = "tools_resetPose";
            public const string SetTPose = "tools_setTPose";
            public const string ZeroBlendshapes = "tools_zeroBlendshapes";
            public const string EditViewpoint = "tools_editViewpoint";
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
