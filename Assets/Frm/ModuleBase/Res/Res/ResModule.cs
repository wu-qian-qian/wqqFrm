using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
public class ResModule : IResModule
{
    private string resourcePaeh = Application.streamingAssetsPath + "/Biuldstreaming";

    private Dictionary<string, AssetInfo> assetInfos;
    private Dictionary<string, AssetBundleInfo> bundleInfos;

    public ResModule()
    {
        assetInfos = new Dictionary<string, AssetInfo>();
        bundleInfos = new Dictionary<string, AssetBundleInfo>();
        LoadPublicRes(resourcePaeh);
    }
    /// <summary>
    /// ��ȡ��������Դ�ļ�
    /// </summary>
    /// <param name="path"></param>
    private void LoadPublicRes(string path)
    {
        if (!Directory.Exists(path)) return;
        DirectoryInfo directory = new DirectoryInfo(path);
        FileInfo[] files = directory.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension != ".pk")
                continue;
            string bundleName = Path.GetFileNameWithoutExtension(files[i].Name);
            if (!bundleName.EndsWith("_public"))
                continue;
            AssetBundle bundle = AssetBundle.LoadFromFile(files[i].FullName);
            bundleInfos.Add(bundleName, new AssetBundleInfo(bundle));
        }
    }

    public T LoadAsset<T>(string path) where T : Object
    {
        path = path.Replace("\\", "/").ToLower();
        if (assetInfos.TryGetValue(path, out AssetInfo assetInfo))
            return (T)assetInfo.obj;
        //������Դֱ����raw�ļ����·����İ���root_public;
        string bundleName = path.Contains("/") ? GetBundleName(path) : "root_public";
        if(!bundleInfos.TryGetValue(bundleName,out AssetBundleInfo bundle))
        {
            if (bundleName.EndsWith("_custom"))
            {
                Debug.Log("��Դ��custom�ļ����У���Ҫ�ֶ����ذ���");
                return null;
            }
            string loadPath = resourcePaeh + "/" + bundleName + ".pk";
            if (!File.Exists(loadPath))
            {
                Debug.Log("��Դ��û�иð���");
                return null;
            }
            bundle = new AssetBundleInfo(AssetBundle.LoadFromFile(loadPath));
            bundleInfos.Add(bundleName, bundle);
        }
        foreach (var item in bundle.bundle.GetAllAssetNames())
        {
            if (!item.Contains(path))
                continue;
            assetInfo = new AssetInfo(bundle.bundle.LoadAsset<T>(item), bundleName);
            assetInfos.Add(path, assetInfo);
            bundle.assetLoaded++;
            break;
        }
        return (T)assetInfo?.obj;
    }
    /// <summary>
    /// ��ȡ��Դ��ad����
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private string GetBundleName(string path)
    {
        //��ȡ����Լ��ĸ��ļ���
        string dirName = path.Replace("/" + Path.GetFileName(path), "");
        if (dirName.EndsWith("_public"))
            return dirName.GetHashCode() + "_public";
        if (dirName.EndsWith("_custom"))
            return dirName.GetHashCode() + "_custom";
        return dirName.GetHashCode().ToString();
    }

    public void LoadAssetAsync<T>(string path, Action<T> callBack) where T : Object
    {
        throw new NotImplementedException();
    }

    public void LoadCustom(string path)
    {
        path = path.Replace("\\", "/").ToLower();
        if (!path.EndsWith("_custom")) return;
        string bundleName = path.GetHashCode() + "_custom";
        if (bundleInfos.ContainsKey(bundleName)) return;
        string loadPath = resourcePaeh + "/" + bundleName + ".pk";
        if (!File.Exists(loadPath)) return;
        bundleInfos.Add(bundleName, new AssetBundleInfo(AssetBundle.LoadFromFile(loadPath)));
    }

    public void LoadCustomAsync(string path)
    {
        throw new NotImplementedException();
    }

    public void UnLoadAsset(Object obj)
    {
        string target = string.Empty;
        foreach (KeyValuePair<string,AssetInfo> kv in assetInfos)
        {
            if (kv.Value.obj != obj)
                continue;
            target = kv.Key;
        }
        if (target == string.Empty) return;
        string bundleName = assetInfos[target].bundleName;
        assetInfos.Remove(target);
        if (!(obj is GameObject))
            Resources.UnloadAsset(obj);
        else
            Resources.UnloadUnusedAssets();
        if (!bundleInfos.TryGetValue(bundleName, out AssetBundleInfo assetBundle))
            return;
        if (--assetBundle.assetLoaded > 0)
            return;
        if (bundleName.EndsWith("_public") || bundleName.EndsWith("_custom"))
            return;
        assetBundle.bundle.Unload(true);
        bundleInfos.Remove(bundleName);
    }

    public void UnLoadCustom(string path)
    {
        path = path.Replace("\\", "/").ToLower();
        if (!path.EndsWith("_custom")) return;
        string bundleName = path.GetHashCode() + "_custom";
        if (!bundleInfos.TryGetValue(bundleName, out AssetBundleInfo bundleInfo))
            return;
        foreach (KeyValuePair<string, AssetInfo> kv in assetInfos)
        {
            if (kv.Value.bundleName != bundleName)
                continue;
            UnLoadAsset(kv.Value.obj);
            assetInfos.Remove(kv.Key);
        }
        bundleInfo.bundle.Unload(true);
        bundleInfos.Remove(bundleName);
    }
    public void Clear()
    {
        foreach (var item in assetInfos.Values)
        {
            if(!(item.obj is GameObject))
            {
                Resources.UnloadAsset(item.obj);
            }
        }
        assetInfos.Clear();
    }
}
