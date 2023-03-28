using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Build.Player;
using System;

public class BiuldAssetBunndle : MonoBehaviour
{
    private  static BuildTarget selectedBuildTarget = BuildTarget.StandaloneWindows64;
    //存放资源的文件夹
    [MenuItem("AssetBunndle/biuldPack",false)]
    public static void BuildPack()
    {
        string path = Application.dataPath +"/"+DevConfig.RawResources;
        if (!Directory.Exists(path))
        {
            Debug.LogError("缺少资源存放文件夹，打包失败");
            return;
        }
        //string compilationTemp = Environment.CurrentDirectory.Replace("\\", "/") + "/Temp/Cirilla";
        //ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
        //scriptCompilationSettings.group = BuildPipeline.GetBuildTargetGroup(selectedBuildTarget);
        //scriptCompilationSettings.target = selectedBuildTarget;
        //scriptCompilationSettings.options = ScriptCompilationOptions.DevelopmentBuild;
        ////在 代码中/Temp/Cirilla  目录下生成所有 dll 以及 对应的 .pdb
        //PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, compilationTemp);
        Dictionary<string, List<string>> rawResource = new Dictionary<string, List<string>>();
        Collect(path, rawResource);
        string biuld = Application.streamingAssetsPath +"/"+DevConfig.Biuldstreaming ;
        if (Directory.Exists(biuld))
        {
            Directory.Delete(biuld, true);
        }
        Directory.CreateDirectory(biuld);
        AssetBundleBuild[] assetBundles = new AssetBundleBuild[rawResource.Count];
        int i = 0;
        foreach ( KeyValuePair<string,List<string>> kv in rawResource)
        {
            assetBundles[i] = new AssetBundleBuild();
            assetBundles[i].assetBundleName = kv.Key;
            assetBundles[i].assetNames = kv.Value.ToArray();
            i++;
        }
        BuildPipeline.BuildAssetBundles(biuld, assetBundles, BuildAssetBundleOptions.ChunkBasedCompression, selectedBuildTarget);
        foreach (var item in assetBundles)
        {
            File.Delete(biuld+"/" + item.assetBundleName + ".manifest");
            //string str = biuld + "/" + item.assetBundleName + ".mainfest";
            File.Delete(biuld +"/"+ item.assetBundleName + ".manifest.meta");
          //  str = biuld + "/" + item.assetBundleName + ".mainfest.meta";
        }
        File.Delete(biuld + "/"+DevConfig.Biuldstreaming);
        File.Delete(biuld + "/" + $"{DevConfig.Biuldstreaming}.{DevConfig.Meta}");
        File.Delete(biuld + "/" + $"{DevConfig.Biuldstreaming}.{DevConfig.Manifest}");
        File.Delete(biuld + "/" + $"{DevConfig.Biuldstreaming}.{DevConfig.Manifest}.{DevConfig.Meta}");
        //创建比对文件
        CreatMathPath(biuld);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 对文件进行递归
    /// </summary>
    /// <param name="path"></param>
    /// <param name="rawResource"></param>
    private static void Collect(string path, Dictionary<string, List<string>> rawResource)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError("build失败，需要把资源放到RawResources文件夹中");
        }
        PickResources(path, rawResource);
        string[] paths = Directory.GetDirectories(path);
        for (int i = 0; i < paths.Length; i++)
        {
            PickResources(paths[i], rawResource);
        }
    }
    /// <summary>
    /// 存储需要打包的文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="rawResource"></param>
    private static void PickResources(string path, Dictionary<string, List<string>> rawResource)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] files = directoryInfo.GetFiles();
        if (files.Length <= 0)
            return;
        //包名
        string sealedPath = path.EndsWith(DevConfig.RawResources) ? "root_public"+DevConfig.PK : GetBunndle(path);
        List<string> pathStrs = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            string fileName = files[i].FullName;
            if (IgnoreFile(fileName))
                continue;
            string asset = "Assets" + fileName.Substring(Application.dataPath.Length).Replace("\\", "/");
            if (!pathStrs.Contains(asset)) pathStrs.Add(asset);
        }
        if (pathStrs.Count > 0)
            rawResource.Add(sealedPath, pathStrs);
    }

    /// <summary>
    /// 区分需要打包的文件
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private static bool IgnoreFile(string file)
    {
        if (file.EndsWith(".meta") || file.EndsWith(".cs") || file.EndsWith(".dll"))
            return true;

        return false;
    }
    /// <summary>
    /// 创建打包的文件名，然后把他变为哈希值
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string GetBunndle(string path)
    {
        path = path.ToLower().Replace("\\","/");
        string biuldName = path.Split(new[] { DevConfig.RawResources.ToLower() + "/" }, StringSplitOptions.None)[1].GetHashCode().ToString();
        if (path.EndsWith("_custom"))
        {
            return biuldName + "_custom." + DevConfig.PK;
        }
        if (path.EndsWith("_public"))
        {
            return biuldName + "_public."+DevConfig.PK;
        }
        return biuldName+DevConfig.PK;
    }
    /// <summary>
    /// 创建比对文件
    /// </summary>
    /// <param name="Path"></param>
    private static void  CreatMathPath(string path)
    {
        string mathName = path + "/" + DevConfig.MathFile;
        string info = DevConfig.Version.ToString() + "\n";
        DirectoryInfo directoryInfos = new DirectoryInfo(path);
        FileInfo[] files = directoryInfos.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (IgnoreFile(files[i].Name))
                continue;
            FileStream stream = files[i].Open(FileMode.Open);
            info += $"{files[i].Name}|{GetMD5(stream)}";
            if (i != files.Length - 1)
                info += "\n";
            stream.Close();
        }
        Write(mathName, info);
    }
    private static string GetMD5(FileStream fileStream)
    {
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(fileStream);

        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
            stringBuilder.Append(retVal[i].ToString("x2"));

        return stringBuilder.ToString();
    }
    private static void Write(string path,string md5)
    {
        if (File.Exists(path))
            File.Delete(path);
        File.Create(path).Dispose();
        StreamWriter stream = new StreamWriter(path);
        stream.Write(md5);
        stream.Close();
    }
    [MenuItem("AssetBunndle/clearPack", false)]
    public static void ClearPack()
    {
        string path = Application.streamingAssetsPath;
        if (Directory.Exists(path))
        {
            //删除文件夹
            Directory.Delete(path,true);
            if (!Directory.Exists(path))
            {
                Debug.Log("删除成功");
            }
        }
        string pathAsst = Application.dataPath;
        DirectoryInfo directoryInfo = new DirectoryInfo(pathAsst);
        FileInfo[] files = directoryInfo.GetFiles();
        //截取到asset/之后的字符
        string pathName = path.Substring(pathAsst.Length+1)+ ".meta";
        Debug.Log(pathName);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name == pathName)
            {
                //删除文件
                files[i].Delete();
                return;
            }
        }
        AssetDatabase.Refresh();
    }
}
