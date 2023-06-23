using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.IO;
using System.Xml;

namespace XRRemote
{   
    [AddComponentMenu("XR/XRRemote/Runtime ARTrackedImageManager")]
    public class XRRemoteImageManager : MonoBehaviour
    {  
   
        
        [Tooltip("The native AR Tracked Image Manager component attached to AR Session Origin.")]
        [SerializeField]
        private ARTrackedImageManager manager;
        public ARTrackedImageManager Manager => manager;

        [HideInInspector]
        public XRReferenceImageLibrary imageLibrary {get; private set;}
    
        // [HideInInspector]
        // Hashtable equivalenceTable;

        [HideInInspector]
        private bool readyToSend = true;

        [HideInInspector]
        // [SerializeField]
        public byte[] bundleByteArray;

        public void Start()
        {
            ConvertBundleToByteArray();
            //send to device here

            //this will only exist on device, just here for testing
            ReconstructLibraryFromBundle();
        }

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
                    Debug.LogWarning("XRRemoteImageManager: Problems with reference image library.");
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
                    readyToSend = false;
                    if (DebugFlags.displayXRRemoteImageManagerStats)
                    {
                        Debug.LogWarning("XRRemoteImageManager: No reference library found on ARTrackedImageManager.");
                    }
                }
                return;
            } 
            readyToSend = false;


            if (DebugFlags.displayXRRemoteImageManagerStats)
            {
                Debug.LogWarning("XRRemoteImageManager: ARTrackedImageManager not found.");
            }
        }

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
                            Debug.LogWarning("XRRemoteImageManager: Reference Image with no texture found.");
                        }
                        if (sizeError)
                        {
                            Debug.LogWarning("XRRemoteImageManager: Reference Image with specified size (0, 0) found.");
                        }
                    }
                }
            }
            return readyToSend;
        }

        private void ConvertBundleToByteArray()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "AssetBundles/imagelibrarybundle");

            try 
            {
                //Does loading this first help? May be able to shorten this to just the second line
                // AssetBundle bundleLoadRequest = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, filePath));
                bundleByteArray = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, filePath));
                // bundleLoadRequest.Unload(false);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Failed to load AssetBundle!");
                Debug.LogWarning(e.Message);
            }
        }

        private void ReconstructLibraryFromBundle()
        {
            // This code will ultimately only exist on the device, but for now it's here for testing purposes.
            
            AssetBundle reconstructedBundle = AssetBundle.LoadFromMemory(bundleByteArray);

            if (reconstructedBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
                return;
            }            
            
            //this can be refined after changing bundling method to not include the user given name for the reference library
            XRReferenceImageLibrary loadedLibrary = reconstructedBundle.LoadAsset(reconstructedBundle.GetAllAssetNames()[0]) as XRReferenceImageLibrary;
            if (loadedLibrary != null) manager.referenceLibrary = loadedLibrary;
            else Debug.LogError("Failed to load asset");

            reconstructedBundle.Unload(false);
        }
    }
}
