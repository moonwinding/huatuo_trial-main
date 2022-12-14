using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

struct MyValue
{
    public int x;
    public float y;
    public string s;
}

public class App
{
    public static int Main()
    {
        Debug.Log("hello, huatuo202206061337");

        var go = new GameObject("HotFix2");
        go.AddComponent<CreateByHotFix2>();
        GameAppStartUp.StartUp();
        return 0;
    }

    /// <summary>
    /// 测试 aot泛型，这个代码大家自己主动调吧
    /// </summary>
    public static void TestAOTGeneric()
    {
        var arr = new List<MyValue>();
        arr.Add(new MyValue() { x = 1, y = 10, s = "abc" });
        var e = arr[0];
        Debug.LogFormat("x:{0} y:{1} s:{2}", e.x, e.y, e.s);
    }

    /// <summary>
    /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    /// </summary>
    public static unsafe void LoadMetadataForAOTAssembly()
    {
        // 可以加载任意aot assembly的对应的dll。但要求dll必须与unity build过程中生成的裁剪后的dll一致，而不能直接使用
        // 原始dll。
        // 这些dll可以在目录 Temp\StagingArea\Il2Cpp\Managed 下找到。
        // 对于Win Standalone，也可以在 build目录的 {Project}/Managed目录下找到。
        // 对于Android及其他target, 导出工程中并没有这些dll，因此还是得去 Temp\StagingArea\Il2Cpp\Managed 获取。
        //
        // 这里以最常用的mscorlib.dll举例
        //
        // 加载打包时 unity在build目录下生成的 裁剪过的 mscorlib，注意，不能为原始mscorlib
        //
        string mscorelib = @$"{Application.dataPath}/../Temp/StagingArea/Il2Cpp/Managed/mscorlib.dll";
        byte[] dllBytes = File.ReadAllBytes(mscorelib);

        fixed (byte* ptr = dllBytes)
        {
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            int err = Huatuo.HuatuoApi.LoadMetadataForAOTAssembly((IntPtr)ptr, dllBytes.Length);
            Debug.Log("LoadMetadataForAOTAssembly. ret:" + err);
        }
    }
}
