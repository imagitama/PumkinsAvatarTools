using System.Collections;
using System.Collections.Generic;
using Pumkin.Core;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class _DependencyDefiner
{
    const string PUMKIN_VRC_SDK = "PUMKIN_VRC_SDK";
    const string VRC_SDK2 = "VRC_SDK_VRCSDK2";
    const string VRC_SDK3 = "VRC_SDK_VRCSDK3";

    static _DependencyDefiner()
    {
        // Add custom VRChat SDK define for assembly definition
        if(ScriptDefineManager.IsDefinedAny(VRC_SDK2, VRC_SDK3))
            ScriptDefineManager.AddDefinesIfMissing(PUMKIN_VRC_SDK);
        else
            ScriptDefineManager.RemoveDefines(PUMKIN_VRC_SDK);
    }
}
