using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

public class BundleBuilder : Editor
{
	[MenuItem("Build/Build Asset Bundles (Current Build Target)", false, 0)]
	private static void BuildAllAssetBundles_Current()
	{
		Debug.Log("building bundles for '" + EditorUserBuildSettings.activeBuildTarget + "'");
		MkDir(EditorUserBuildSettings.activeBuildTarget);
		BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/" + EditorUserBuildSettings.activeBuildTarget, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
		AssetDatabase.Refresh();
		Debug.Log("done building '" + EditorUserBuildSettings.activeBuildTarget + "'");
	}

	[MenuItem("Build/Build Asset Bundles (Android)", false, 20)]
	private static void BuildAllAssetBundles_Android()
	{
		Debug.Log("building bundles for '" + BuildTarget.Android + "'");
		MkDir(BuildTarget.Android);
		BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/" + BuildTarget.Android, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
		Debug.Log("done building '" + BuildTarget.Android + "'");
	}

	[MenuItem("Build/Build Asset Bundles (WebGL)", false, 20)]
	private static void BuildAllAssetBundles_WebGL()
	{
		Debug.Log("building bundles for '" + BuildTarget.WebGL + "'");
		MkDir(BuildTarget.WebGL);
		BuildPipeline.BuildAssetBundles(@"Assets/StreamingAssets/" + BuildTarget.WebGL, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
		Debug.Log("done building '" + BuildTarget.WebGL + "'");
	}

	private static void MkDir(BuildTarget target)
	{
		if (!Directory.Exists("Assets/StreamingAssets")) Directory.CreateDirectory("Assets/StreamingAssets");
		if (!Directory.Exists("Assets/StreamingAssets/" + target)) Directory.CreateDirectory("Assets/StreamingAssets/" + target);
	}
}

public class BuildPlayer
{
	[MenuItem("Build/Build Android", false, 40)]
	private static void BuildAndroid()
	{
		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
		{
			scenes = GetSceneNames(), //new[] { "Assets/Scene1.unity", "Assets/Scene2.unity" };
			locationPathName = "AndroidBuild",
			target = BuildTarget.Android,
			options = BuildOptions.None
		};

		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
		{
			Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
		}

		if (summary.result == BuildResult.Failed)
		{
			Debug.Log("Build failed");
		}
	}

	[MenuItem("Build/Build WebGL", false, 40)]
	private static void BuildWebGL()
	{
		BuildPipeline.BuildPlayer(new BuildPlayerOptions
		{
			scenes = GetSceneNames(),
			locationPathName = @"C:\temp\Unity\out\temp\",
			assetBundleManifestPath = @"C:\temp\Unity\out\temp\AssetBundles.manifest",
			target = BuildTarget.WebGL,
			options = BuildOptions.None
		});
	}

	private static string[] GetSceneNames()
	{
		int sceneCount = EditorBuildSettings.scenes.Length;
		string[] sceneNames = new string[sceneCount];
		for (int i = 0; i < sceneCount; ++i)
		{
			sceneNames[i] = EditorBuildSettings.scenes[i].path;
		}

		return sceneNames;
	}
}
