using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class AssetLoader : MonoBehaviour
{
	AssetBundle myLoadedAssetBundle;
	UnityWebRequest uwr;

	public static string filename;
	public static float progress;
	public static bool inprogress;
	public static bool allfilesfinished;

	void Start()
	{
		//can't use EditorUserBuildSettings.activeBuildTarget or Application.platform
#if UNITY_ANDROID
		string taregetplatform = "Android";
#elif UNITY_WEBGL
		string taregetplatform = "WebGL";
#else
	no target platform defined for this target
#endif
		string url = Application.streamingAssetsPath + "/" + taregetplatform + "/scenes";
		Debug.Log("Application.streamingAssetsPath = '" + url + "'");
		uwr = UnityWebRequestAssetBundle.GetAssetBundle(url);
		uwr.SendWebRequest();

		allfilesfinished = false;
		progress = 0;
		inprogress = true;
		filename = url;
	}

	private void Update()
	{
		if (uwr == null) return;
		progress = uwr.downloadProgress;
		if (uwr.isNetworkError || uwr.isHttpError)
		{
			inprogress = false;
			Debug.Log("uwr.error: " + uwr.error);
			Debug.Log("uwr.url: " + uwr.url);

			//inprogress = false;
			//yield break;
			uwr = null;
		}
		else if (uwr.isDone)
		{
			inprogress = false;
			Debug.Log("asset bundle is loaded");
			myLoadedAssetBundle = DownloadHandlerAssetBundle.GetContent(uwr);
			uwr = null;
			//FileStream fileStream = new FileStream(Application.streamingAssetsPath + "/Android/scenes", FileMode.Open, FileAccess.Read);
			//AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromStream(fileStream);
			if (myLoadedAssetBundle == null)
			{
				Debug.Log("Failed to load AssetBundle!");
				return;
			}
			foreach (string s in myLoadedAssetBundle.GetAllScenePaths())
			{
				Debug.Log("map bundle contains '" + s + "'");
				string scene = Path.GetFileNameWithoutExtension(s);
			}
			allfilesfinished = true;
		}
		else
		{
			//Debug.Log("uwr progress: " + uwr.downloadProgress);
		}
	}
}
