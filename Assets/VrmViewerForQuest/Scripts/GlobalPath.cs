using System;
using UnityEngine;

public class GlobalPath
{
    public static string VrmHomePath => Application.persistentDataPath + "/VRM";
#if UNITY_EDITOR
    //Editor起動の際は、プロジェクトフォルダをdownloadフォルダとして扱う。
    public static string DownloadPath => Environment.CurrentDirectory;
#else
    public static string DownloadPath => Application.persistentDataPath + "/../../../../Download";
#endif
}