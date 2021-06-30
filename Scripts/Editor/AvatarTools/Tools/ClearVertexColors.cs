using Pumkin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools2.Tools
{
    [AutoLoad(DefaultIDs.Tools.ClearVertexColors, ParentModuleID = DefaultIDs.Modules.Tools_FixAvatar)]
    class ClearVertexColors : ToolBase //TODO: Check if this actually saves when unity restarts. It probably doesn't
    {
        protected override bool DoAction(GameObject target)
        {
            var skinnedMeshes = target.GetComponentsInChildren<SkinnedMeshRenderer>(true).Select(x => x.sharedMesh);
            var meshes = target.GetComponentsInChildren<MeshFilter>(true).Select(x => x.sharedMesh);

            foreach(var mesh in skinnedMeshes.Concat(meshes))
            {
                var colors = Enumerable.Repeat(Color.white, mesh.vertexCount).ToList();
                mesh.SetColors(colors);
            }
            return true;
        }
    }
}
