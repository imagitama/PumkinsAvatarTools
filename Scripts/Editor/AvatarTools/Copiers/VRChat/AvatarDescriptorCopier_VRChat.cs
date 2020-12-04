using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pumkin.AvatarTools.Base;
using Pumkin.AvatarTools.Interfaces;
using Pumkin.AvatarTools.Modules;
using Pumkin.AvatarTools.Settings;
using Pumkin.AvatarTools.Types;
using Pumkin.AvatarTools.UI;
using Pumkin.Core;
using Pumkin.Core.Extensions;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;

namespace Pumkin.AvatarTools.Copiers
{
    [AutoLoad("copier_avatarDescriptor", "vrchat", ParentModuleID = DefaultModuleIDs.COPIER)]
    class AvatarDescriptorCopier_VRChat : ComponentCopierBase
    {
        public override UIDefinition UIDefs { get; set; }
            = new UIDefinition("Avatar Descriptor");

        public override string ComponentTypeFullName
            => VRChatTypes.VRC_AvatarDescriptor?.FullName;

        protected override GUIContent CreateGUIContent() =>
            new GUIContent(UIDefs.Name, Icons.AvatarDescriptor.image);

        public override ISettingsContainer Settings => settings;

        AvatarDescriptorCopier_VRChat_Settings settings;

        protected override void SetupSettings()
        {
            settings = ScriptableObject.CreateInstance<AvatarDescriptorCopier_VRChat_Settings>();
        }

        //TODO: Make a multi component copier or provide support for multiple right out the box
        //protected override void AfterCopyComponent(GameObject from, GameObject to)
        //{
        //    if(!settings.pipelineID)
        //        return;

        //    var fromPipe = from.GetComponent(VRChatTypes.PipelineManager);
        //    var toPipe = to.GetOrAddComponent(VRChatTypes.PipelineManager);

        //    if(!fromPipe || !toPipe)
        //        return;

        //    var serialFromPipe = new SerializedObject(fromPipe);
        //    var serialToPipe = new SerializedObject(toPipe);

        //    string[] propNames =
        //    {
        //        "pipelineId", "launchedFromSDKPipeline", "completedSDKPipeline", "contentType"
        //    };

        //    foreach(var pName in propNames)
        //    {
        //        var p = serialFromPipe.FindProperty(pName);
        //        if(p == null)
        //            continue;

        //        serialToPipe.CopyFromSerializedProperty(p);
        //    }
        //}
    }
}
