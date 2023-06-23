using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.IO;
using UnityEngine.XR.ARSubsystems;

namespace XRRemote
{
    public class BundleLoader : MonoBehaviour
    {
        private XRReferenceImageLibrary imageLibrary;

        void Awake()
        {

        }

        IEnumerator Start() 
        {

            string assetBundleFilePath = "AssetBundles/imagelibrarybundle";
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, assetBundleFilePath));

            yield return bundleLoadRequest;

            if (bundleLoadRequest.assetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
            }
            else
            {
                var assetNames = bundleLoadRequest.assetBundle.GetAllAssetNames();
                var assetLoadRequest = bundleLoadRequest.assetBundle.LoadAssetAsync<XRReferenceImageLibrary>("assets/testlibrary.asset");

                yield return assetLoadRequest;

                if(assetLoadRequest.asset==null)
                {
                    Debug.LogError("Failed to load asset");
                }
                else
                {
                    imageLibrary = assetLoadRequest.asset as XRReferenceImageLibrary;
                    GetComponent<ARTrackedImageManager>().referenceLibrary = imageLibrary;
                }
                bundleLoadRequest.assetBundle.Unload(false);
            }
        }   
    }
}
