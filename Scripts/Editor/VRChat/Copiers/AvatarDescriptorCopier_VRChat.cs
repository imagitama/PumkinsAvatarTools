using Pumkin.AvatarTools2.Interfaces;
using Pumkin.AvatarTools2.Settings;
using Pumkin.AvatarTools2.Types;
using Pumkin.AvatarTools2.UI;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEngine;

namespace Pumkin.AvatarTools2.Copiers
{
    [AutoLoad(DefaultIDs.Copiers.AvatarDescriptor, "vrchat", ParentModuleID = DefaultIDs.Modules.Copier)]
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
