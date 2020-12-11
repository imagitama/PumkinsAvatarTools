#if UNITY_EDITOR
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    [AutoLoad("tools_zeroblendshapes", ParentModuleID = "tools_fixAvatar")]
    class ZeroBlendshapes : ToolBase
    {
        public override UIDefinition UIDefs { get; set; } =
            new UIDefinition("Zero Blendshapes", "Resets all Blendshapes on all SkinnedMeshRenderers to 0");

        SkinnedMeshRenderer[] renders;

        protected override bool Prepare(GameObject target)
        {
            if(!base.Prepare(target))
                return false;

            renders = target.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            return renders.Length > 0 ? true : false;
        }

        protected override bool DoAction(GameObject target)
        {
            var serialRenders = new SerializedObject(renders);
            SerializedProperty prop;

            if((prop = serialRenders.FindProperty("m_BlendShapeWeights")) == null)
                return false;

            prop.ClearArray();
            serialRenders.ApplyModifiedProperties();
            return true;
        }
    }
}
#endif