using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// time:2019/1/19
/// author:King.Sun
/// description:AB包加载
/// </summary>
public class AbLoad : MonoBehaviour
{
    /// <summary>
    /// AB包路径
    /// </summary>
    private string _abPath;
    /// <summary>
    /// AB包
    /// </summary>
    private AssetBundle ab;
    // Use this for initialization
    void Start()
    {

        //不同平台下路径不同
#if UNITY_ANDROID
		_abPath = "jar:file://" + Application.dataPath + "!/assets/AssetBundles/";
#elif UNITY_IPHONE
		_abPath = Application.dataPath + "/Raw/AssetBundles/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        _abPath = Application.streamingAssetsPath+"/";
#else
		_abPath = string.Empty;
#endif


        //第一种 LoadFromMemoryAsync 异步加载
        StartCoroutine(LoadABPackage1(_abPath));

        /*
		//第二种 LoadFromMemory 同步加载
		LoadABPackage2();
		*/

        /*
		//第三种 LoadFromFileAsync 只能加载本地包
		StartCoroutine(LoadABPackage3(_abPath));
		*/

        /*
		//第四种 LoadFromFile 只能加载本地包
		LoadABPackage4(_abPath);
		*/

        /*
		//第五种 WWW加载方法 本地服务器均可
		StartCoroutine(LoadABPackage5(_abPath));
		*/

        /*
		//第六种 UnityWebRequest 加载方式
		StartCoroutine(LoadABPackage6(_abPath));
		*/


        /*
		//加载依赖文件可以通过--->记载描述文件中的信息
		
		//***Manifest 依赖描述文件
		AssetBundleManifest manifest = manifestAB.LoadAsset<AssetBundleManifest>("***Manifest");
		//***.unity3d 需要加载的物体 获取依赖项名字数组
		string[] strs =  manifest.GetAllDependencies("***.unity3d");
		//根据名字依次加载依赖
		foreach (string name in strs)
		{
			AssetBundle.LoadFromFile("AssetBundles/" + name);
		}
		*/

    }

    #region 第一种加载方式  通过LoadFromMemoryAsync加载从服务器或本地获取的字节流来加载

    IEnumerator LoadABPackage1(string path)
    {
        Vector3 vector3 = new Vector3(0, 0, 1);
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path+ "object"));
        yield return request;
        ab = request.assetBundle;
        Object cube = ab.LoadAsset("Cube.prefab");
        Instantiate(cube);
    }

    #endregion

    #region 第二种方法  LoadFromMemory 同步加载

    void LoadABPackage2()
    {
        ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(_abPath + "cube.unity3d.assetbundle"));
        Object cube = ab.LoadAsset("Cube.prefab");
        Instantiate(cube);
    }

    #endregion

    #region 第三种加载方式 LoadFromFileAsync 只能加载本地包
    IEnumerator LoadABPackage3(string path)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path + "cube.unity3d.assetbundle");

        yield return request;

        ab = request.assetBundle;
        Object cube = ab.LoadAsset("Cube.prefab");
        Instantiate(cube);
    }
    #endregion

    #region 第四种加载方式 LoadFromFile 只能加载本地

    void LoadABPackage4(string path)
    {
        ab = AssetBundle.LoadFromFile(path + "cube.unity3d.assetbundle");
        Object cube = ab.LoadAsset("Cube.prefab");
        Instantiate(cube);
    }

    #endregion

    #region 第五种加载方式 WWW加载 网络本地均可

    IEnumerator LoadABPackage5(string path)
    {
        WWW www = WWW.LoadFromCacheOrDownload(path + "cube.unity3d.assetbundle", 1);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield break;
        }

        ab = www.assetBundle;
        Object cube = ab.LoadAsset("Cube.prefab");
        Instantiate(cube);
    }

    #endregion

    #region 第六种加载方式 UnityWebRequest 

    IEnumerator LoadABPackage6(string path)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path + "cube.unity3d.assetbundle");
        yield return request.Send();
        ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
        Object cube = ab.LoadAsset("Cube.prefab");
        Instantiate(cube);
    }

    #endregion

}
