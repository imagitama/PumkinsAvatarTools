using System.Collections.Generic;
using System.Linq;
using Pumkin.AvatarTools2.Tools;
using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using VRC.SDKBase.Editor.BuildPipeline;

namespace Pumkin.AvatarTools2.VRChat.Tools
{
    [AutoLoad("tools_avatarUploadHider", "VRChat", ParentModuleID = DefaultIDs.Modules.Camera)]
    public class AvatarUploadHider : ToolUpdateToggleBase, IVRCSDKPreprocessAvatarCallback //TODO: Make a base callback tool
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Hide Other Avatars on Upload");

        public int callbackOrder => 0;
        const string uploadTargetPrefName = "UploadTargetAvatarID";

        static GameObject _uploadTarget;
        static IEnumerable<GameObject> _avatarCache;

        static string AvatarID
        {
            get => PrefManager.GetString(uploadTargetPrefName);
            set => PrefManager.SetString(uploadTargetPrefName, value);
        }
        public static GameObject UploadTarget
        {
            get
            {
                if(!_uploadTarget)
                {
                    string id = AvatarID;
                    if(!string.IsNullOrWhiteSpace(id))
                        _uploadTarget = GameObject.FindObjectsOfType<PipelineManager>()
                            .Where(a => a.blueprintId == id)
                            .Select(a => a.gameObject)
                            .FirstOrDefault();
                    AvatarID = null;
                }
                return _uploadTarget;
            }
        }
        static IEnumerable<GameObject> AvatarCache
        {
            get
            {
                if(_avatarCache == null)
                {
                    _avatarCache = Object.FindObjectsOfType<VRC_AvatarDescriptor>()
                        .Select(x => x.gameObject);
                }
                return _avatarCache;
            }
        }


        public AvatarUploadHider()
        {
            EditorApplication.playModeStateChanged -= HandlePlayModeStateChange;
            EditorApplication.playModeStateChanged += HandlePlayModeStateChange;
        }
        
        protected override void OnDisableToggle()
        {
            base.OnDisableToggle();
            if(EditorApplication.isPlaying)
                SetOtherAvatarsActiveState(true);
        }

        protected override void OnEnableToggle()
        {
            base.OnEnableToggle();
            if(EditorApplication.isPlaying)
                SetOtherAvatarsActiveState(false);
        }

        public bool OnPreprocessAvatar(GameObject avatarGameObject)
        {
            AvatarID = avatarGameObject.GetComponent<PipelineManager>()?.blueprintId;
            return true;
        }

        void HandlePlayModeStateChange(PlayModeStateChange mode)
        {
            if(mode != PlayModeStateChange.EnteredPlayMode)
                return;

            if(!_allowUpdate || UploadTarget == null)
                return;

            SetOtherAvatarsActiveState(false);
        }

        static void SetOtherAvatarsActiveState(bool isActive)
        {
            if(AvatarCache is null || UploadTarget is null)
                return;

            foreach(var av in AvatarCache)
            {
                if(av == UploadTarget)
                    continue;
                av.gameObject.SetActive(isActive);
            }
        }
    }
}