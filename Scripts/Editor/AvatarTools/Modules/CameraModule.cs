using Pumkin.Core;
using Pumkin.Core.UI;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

namespace Pumkin.AvatarTools2.Modules
{
    [AutoLoad(DefaultIDs.Modules.Camera)]
    public class CameraModule : UIModuleBase
    {
        public override UIDefinition UIDefs { get; set; } = new UIDefinition("Camera", 3);

        public static event Delegates.SelectedCameraChangeHandler OnCameraSelectionChanged;
        
        internal static Camera SelectedCamera
        {
            get => _camera;
            set
            {
                if(_camera != value)
                    OnCameraSelectionChanged?.Invoke(value);
                _camera = value;
            }
        }

        public override void Start()
        {
            SelectedCamera = Camera.main;
        }

        public override void DrawContent()
        {
            SelectedCamera = EditorGUILayout.ObjectField("Camera", SelectedCamera, typeof(Camera), true) as Camera;
            
            EditorGUILayout.Space();
            base.DrawContent();
        }

        internal static Camera _camera;
    }
}