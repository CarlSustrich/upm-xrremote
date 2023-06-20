using UnityEditor;
using UnityEngine;

namespace XRRemote
{
    [CustomEditor(typeof(XRRemoteImageManager))]
    public class XRRemoteImageManagerEditor : Editor
    {
        private GUIStyle redButtonStyle;
        private GUIStyle greenButtonStyle;
    
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
            UpdatePlayModeState();
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

            redButtonStyle = new GUIStyle(GUI.skin.button);
            redButtonStyle.normal.textColor = Color.red;

            greenButtonStyle = new GUIStyle(GUI.skin.button);
            greenButtonStyle.normal.textColor = Color.green;

            if (!isPlayMode)
            {
                if (GUILayout.Button("Send Library To Device"))
                {                    
                    if (manager.OnClickTrySend())
                    {
                        manager.imageLibrary.BuildAssetBundle();
                    }
                }
                // if (GUILayout.Button(new GUIContent("Send Library", "Green: Library is up to date on device.\nYellow: Library has changed on the device and needs to be sent.\nRed: Library is not ready to be sent."),
                //     myScript.readyToSend ? greenButtonStyle : (myScript.libraryChanged ? yellowButtonStyle : redButtonStyle)))
                // {
                //     myScript.TestingButton();
                // }
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
