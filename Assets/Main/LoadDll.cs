using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadDll : MonoBehaviour
{
    void Start()
    {
        LoadGameDll();
        RunMain();
    }

    public static System.Reflection.Assembly gameAss;

    private void LoadGameDll()
    {
#if UNITY_EDITOR
        string gameDll = Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll";
        // 使用File.ReadAllBytes是为了避免Editor下gameDll文件被占用导致后续编译后无法覆盖
        gameAss = System.Reflection.Assembly.Load(File.ReadAllBytes(gameDll));
#else
        string gameDll = Application.streamingAssetsPath + "/HotFix.dll";
        gameAss = System.Reflection.Assembly.LoadFile(gameDll);
#endif
    }

    public void RunMain()
    {
        if (gameAss == null)
        {
            UnityEngine.Debug.LogError("dll未加载");
            return;
        }
        var appType = gameAss.GetType("App");
        var mainMethod = appType.GetMethod("Main");
        mainMethod.Invoke(null, null);
    }
}
