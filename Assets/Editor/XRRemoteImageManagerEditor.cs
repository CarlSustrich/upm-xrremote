using UnityEditor;
using UnityEngine;
using System.IO;

namespace XRRemote 
{
    [CustomEditor(typeof(XRRemoteImageManager))]
    public class XRRemoteImageManagerEditor : Editor
    {
        private bool isPlayMode;

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            UpdatePlayModeState();
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            // UpdatePlayModeState();
            isPlayMode = EditorApplication.isPlaying;
            Repaint();
        }

        private void UpdatePlayModeState()
        {
            isPlayMode = EditorApplication.isPlaying;
            Repaint();
            //condense into onplaymodestatechanged?
        }

        public override void OnInspectorGUI()
        {
            XRRemoteImageManager manager = (XRRemoteImageManager)target;

            DrawDefaultInspector();



            if (!isPlayMode)
            {
                if (GUILayout.Button("Send Library To Device"))
                {                    
                    if (manager.OnClickTrySend())
                    {
                        manager.imageLibrary.BuildAssetBundle();
                    }
                }
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.LabelField("Send Library", "Disabled in play mode");
                GUI.enabled = true;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
