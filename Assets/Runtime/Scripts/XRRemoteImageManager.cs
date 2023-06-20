using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEditor;

namespace XRRemote
{   
    [AddComponentMenu("XR/XRRemote/Runtime ARTrackedImageManager")]
    public class XRRemoteImageManager : MonoBehaviour
    {  
   
        
        [Tooltip("The native AR Tracked Image Manager component attached to AR Session Origin.")]
        public ARTrackedImageManager manager;

        [HideInInspector]
        public XRReferenceImageLibrary imageLibrary;
    
        [HideInInspector]
        Hashtable equivalenceTable;

        [HideInInspector]
        public bool readyToSend = true;

    #if UNITY_EDITOR
        public bool OnClickTrySend()
        {
            UpdateLibrary();
            
            if(readyToSend)
            {
                return true;
            }
            else
            {
                if (DebugFlags.displayXRRemoteImageManagerStats)
                {
                    Debug.Log("XRRemoteImageManager: Problems with reference image library.");
                }
            }
            return false;
        }
    #endif

        private void UpdateLibrary()
        {
            if (manager != null)
            {
                
                if (manager.referenceLibrary != null)
                {
                    XRReferenceImageLibrary newLibrary = manager.referenceLibrary as XRReferenceImageLibrary;
                    if (CheckLibraryValidity(newLibrary))
                    {
                        imageLibrary = manager.referenceLibrary as XRReferenceImageLibrary;
                    }
                }
                else
                {
                    if (DebugFlags.displayXRRemoteImageManagerStats)
                    {
                        Debug.Log("XRRemoteImageManager: No reference library found on ARTrackedImageManager.");
                    }
                }

                return;
            } 


            if (DebugFlags.displayXRRemoteImageManagerStats)
            {
                Debug.Log("XRRemoteImageManager: ARTrackedImageManager not found.");
            }
        }
        
        // private void BuildAssetBundle()
        // {   
            // ImageLibraryAssetBundler.BuildAssetBundle();
            // Debug.Log("BuildAssetBundleRan");
        //     referenceLibrary.BuildAssetBundle();
        // }
        

        private bool CheckLibraryValidity(XRReferenceImageLibrary newLibrary)
        {
            readyToSend = true;

            for (int i = 0; i < newLibrary.count; i++)
            {
                //check for issues with library entry that would cause errors on server
                bool emptyTextureError = newLibrary[i].textureGuid.Equals(Guid.Empty);
                bool sizeError = newLibrary[i].specifySize && newLibrary[i].size.ToString() == "(0.00, 0.00)";
                if (emptyTextureError || sizeError)
                {
                    readyToSend = false;
                    if (DebugFlags.displayXRRemoteImageManagerStats)
                    {
                        if (emptyTextureError)
                        {
                            Debug.Log("XRRemoteImageManager: Reference Image with no texture found.");
                        }
                        if (sizeError)
                        {
                            Debug.Log("XRRemoteImageManager: Reference Image with specified size (0, 0) found.");
                        }
                    }
                }
            }
            return readyToSend;
        }
    }
}
