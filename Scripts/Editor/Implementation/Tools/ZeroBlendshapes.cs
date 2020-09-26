#if UNITY_EDITOR
using Pumkin.AvatarTools.Attributes;
using Pumkin.AvatarTools.Implementation.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Implementation.Tools
{
    [AutoLoad("tools_zeroblendshapes", ParentModuleID = "tools_fixAvatar")]
    [UIDefinition("Zero Blendshapes", Description = "Resets all Blendshapes on all SkinnedMeshRenderers to 0")]
    class ZeroBlendshapes : SubToolBase
    {
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
            SerializedObject serialRenders = new SerializedObject(renders);
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