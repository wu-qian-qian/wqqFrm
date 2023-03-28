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
    //�����Դ���ļ���
    [MenuItem("AssetBunndle/biuldPack",false)]
    public static void BuildPack()
    {
        string path = Application.dataPath +"/"+DevConfig.RawResources;
        if (!Directory.Exists(path))
        {
            Debug.LogError("ȱ����Դ����ļ��У����ʧ��");
            return;
        }
        //string compilationTemp = Environment.CurrentDirectory.Replace("\\", "/") + "/Temp/Cirilla";
        //ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
        //scriptCompilationSettings.group = BuildPipeline.GetBuildTargetGroup(selectedBuildTarget);
        //scriptCompilationSettings.target = selectedBuildTarget;
        //scriptCompilationSettings.options = ScriptCompilationOptions.DevelopmentBuild;
        ////�� ������/Temp/Cirilla  Ŀ¼���������� dll �Լ� ��Ӧ�� .pdb
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
        //�����ȶ��ļ�
        CreatMathPath(biuld);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// ���ļ����еݹ�
    /// </summary>
    /// <param name="path"></param>
    /// <param name="rawResource"></param>
    private static void Collect(string path, Dictionary<string, List<string>> rawResource)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError("buildʧ�ܣ���Ҫ����Դ�ŵ�RawResources�ļ�����");
        }
        PickResources(path, rawResource);
        string[] paths = Directory.GetDirectories(path);
        for (int i = 0; i < paths.Length; i++)
        {
            PickResources(paths[i], rawResource);
        }
    }
    /// <summary>
    /// �洢��Ҫ������ļ�
    /// </summary>
    /// <param name="path"></param>
    /// <param name="rawResource"></param>
    private static void PickResources(string path, Dictionary<string, List<string>> rawResource)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] files = directoryInfo.GetFiles();
        if (files.Length <= 0)
            return;
        //����
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
    /// ������Ҫ������ļ�
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
    /// ����������ļ�����Ȼ�������Ϊ��ϣֵ
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
    /// �����ȶ��ļ�
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
            //ɾ���ļ���
            Directory.Delete(path,true);
            if (!Directory.Exists(path))
            {
                Debug.Log("ɾ���ɹ�");
            }
        }
        string pathAsst = Application.dataPath;
        DirectoryInfo directoryInfo = new DirectoryInfo(pathAsst);
        FileInfo[] files = directoryInfo.GetFiles();
        //��ȡ��asset/֮����ַ�
        string pathName = path.Substring(pathAsst.Length+1)+ ".meta";
        Debug.Log(pathName);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name == pathName)
            {
                //ɾ���ļ�
                files[i].Delete();
                return;
            }
        }
        AssetDatabase.Refresh();
    }
}
