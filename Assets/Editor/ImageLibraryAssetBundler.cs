using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using System.IO;


namespace XRRemote
{
    #if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
    #endif

    public static class ImageLibraryAssetBundler
    { 

        public static void BuildAssetBundle(this XRReferenceImageLibrary imageLibrary)
        {
            // Create the array of bundle build details.
            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

            buildMap[0].assetBundleName = "imagelibrarybundle";

            string[] library = new string[1];
            library[0] = AssetDatabase.GetAssetPath(imageLibrary);

            buildMap[0].assetNames = library;

            string outputDirectory = "Assets/StreamingAssets/AssetBundles/";

            BuildPipeline.BuildAssetBundles(outputDirectory, buildMap, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
        
        
        // public static void BuildAssetBundle(this XRReferenceImageLibrary imageLibrary)
        // {
        //     string assetBundleName = "referenceimagelibrary";
        //     string assetName = imageLibrary.name;
        //     string outputDirectory = "Assets/StreamingAssets/AssetBundles/";
        //     string outputPath = Path.Combine(outputDirectory, assetName.ToLower() + ".assetbundle");
            
        //     Debug.Log("Building asset bundle...");

        //     if (!Directory.Exists(outputDirectory))
        //     {
        //         Directory.CreateDirectory(outputDirectory);
        //     }

        //     //Delete previous bundle, if present
        //     System.IO.DirectoryInfo di = new DirectoryInfo(outputDirectory);
        //     foreach(FileInfo file in di.GetFiles())
        //     {   
        //         file.Delete();
        //     }

        //     // Set the build options for the asset bundle
        //     BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.None;

        //     // Create a temporary asset bundle manifest
        //     AssetBundleBuild[] assetBundleBuilds = new AssetBundleBuild[1];
        //     AssetBundleBuild bundleBuild = new AssetBundleBuild();
        //     bundleBuild.assetBundleName = assetBundleName;
        //     bundleBuild.assetNames = new string[] { AssetDatabase.GetAssetPath(imageLibrary) };
        //     bundleBuild.assetBundleAssets = new string[] { AssetDatabase.GetAssetPath(imageLibrary) };
        //     assetBundleBuilds[0] = bundleBuild;

        //     // Build the asset bundle
        //     AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(outputDirectory, assetBundleBuilds, buildOptions, EditorUserBuildSettings.activeBuildTarget);

        //     if (manifest != null)
        //     {
        //         Debug.Log("Asset bundle created successfully at: " + outputPath);
        //     }
        //     else
        //     {
        //         Debug.LogError("Failed to create asset bundle.");
        //     }
        // }
    

        // public static void BuildAssetBundle(this XRReferenceImageLibrary imageLibrary)
        // {
        //     string assetBundleName = "ReferenceImageLibrary";
        //     string assetName = imageLibrary.name;
        //     string outputDirectory = "Assets/StreamingAssetBundles/";
        //     string outputPath = outputDirectory + assetName + ".asset"; 

        //     Debug.Log("Building asset bundle...");

        //     if (!Directory.Exists(outputDirectory))
        //     {
        //         Directory.CreateDirectory(outputDirectory);
        //     }

        //     if(File.Exists(outputDirectory))
        //     {
        //         File.Delete(outputDirectory);
        //     }

        //     // Create an instance of AssetBundleBuild
        //     AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
        //     assetBundleBuild.assetBundleName = assetBundleName;
        //     assetBundleBuild.assetNames = new string[] { assetName };


        //     // Build the asset bundle
        //     BuildPipeline.BuildAssetBundles(outputDirectory, new AssetBundleBuild[] { assetBundleBuild }, BuildAssetBundleOptions.None, BuildTarget.Android);

        //     Debug.Log("Asset bundle created successfully at: " + outputDirectory);
            
        // }

        // public static void BuildAssetBundle(this XRReferenceImageLibrary imageLibrary)
        // {
        //     string assetBundleName = "referenceimagelibrary";
        //     string assetName = imageLibrary.name;
        //     string outputDirectory = "Assets/StreamingAssets/AssetBundles";
        //     string outputPath = Path.Combine(outputDirectory, assetName.ToLower() + ".assetbundle");

        //     Debug.Log("Building asset bundle...");

        //     if (!Directory.Exists(outputDirectory))
        //     {
        //         Directory.CreateDirectory(outputDirectory);
        //     }

        //     // Set the build options for the asset bundle
        //     BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.None;

        //     // Create a temporary asset bundle manifest
        //     AssetBundleBuild[] assetBundleBuilds = new AssetBundleBuild[1];
        //     AssetBundleBuild bundleBuild = new AssetBundleBuild();
        //     bundleBuild.assetBundleName = assetBundleName;
        //     bundleBuild.assetNames = new string[] { AssetDatabase.GetAssetPath(imageLibrary) };
        //     assetBundleBuilds[0] = bundleBuild;

        //     // Build the asset bundle
        //     BuildReport buildReport = BuildPipeline.BuildAssetBundles(outputDirectory, assetBundleBuilds, buildOptions, EditorUserBuildSettings.activeBuildTarget);

        //     if (buildReport != null && buildReport.summary.result == BuildResult.Succeeded)
        //     {
        //         Debug.Log("Asset bundle created successfully at: " + outputPath);
        //     }
        //     else
        //     {
        //         Debug.LogError("Failed to create asset bundle.");
        //     }
        // }
    } 
   
}

// Needs setting added to specify target build platform android / iOS  -- addressed via dynmically targetting build platform
/*
Must review compression methods:

BuildAssetBundleOptions.None: This bundle option uses LZMA Format compression, which is a single compressed LZMA stream of serialized data files. LZMA compression requires that the entire bundle is decompressed before it’s used. This results in the smallest possible file size but a slightly longer load time due to the decompression. It is worth noting that when using this BuildAssetBundleOptions, in order to use any assets from the bundle the entire bundle must be uncompressed initially.
Once the bundle has been decompressed, it will be recompressed on disk using LZ4 compression which doesn’t require the entire bundle be decompressed before using assets from the bundle. This is best used when a bundle contains assets such that to use one asset from the bundle would mean all assets are going to be loaded. Packaging all assets for a character or scene are some examples of bundles that might use this.
Using LZMA compression is only recommended for the initial download of an AssetBundle from an off-site host due to the smaller file size. LZMA compressed asset bundles loaded through UnityWebRequestAssetBundle are automatically recompressed to LZ4 compression and cached on the local file system. If you download and store the bundle through other means, you can recompress it with the AssetBundle.RecompressAssetBundleAsync API.

BuildAssetBundleOptions.UncompressedAssetBundle: This bundle option builds the bundles in such a way that the data is completely uncompressed. The downside to being uncompressed is the larger file download size. However, the load times once downloaded will be much faster. Uncompressed AssetBundles are 16-byte aligned.

BuildAssetBundleOptions.ChunkBasedCompression: This bundle option uses a compression method known as LZ4, which results in larger compressed file sizes than LZMA but does not require that entire bundle is decompressed, unlike LZMA, before it can be used. LZ4 uses a chunk based algorithm which allows the AssetBundle be loaded in pieces or “chunks.” Decompressing a single chunk allows the contained assets to be used even if the other chunks of the AssetBundle are not decompressed.
*/
