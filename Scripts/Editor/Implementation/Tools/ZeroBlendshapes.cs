using Pumkin.UnityTools.Attributes;
using Pumkin.UnityTools.Implementation.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.UnityTools.Implementation.Tools
{
    [AutoLoad("tools_zeroblendshapes", "tools")]
    class ZeroBlendshapes : SubToolBase
    {
        SkinnedMeshRenderer[] renders;

        public override SettingsContainer Settings => null;

        public ZeroBlendshapes()
        {
            Name = "Zero Blendshapes";
            Description = "Resets all Blendshapes on all SkinnedMeshRenderers to 0";
        }

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
