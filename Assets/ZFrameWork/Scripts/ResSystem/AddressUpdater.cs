using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class AddressUpdater : MonoBehaviour
{
    public Text outputText;
    public ComponentSetter componentSetter;
    private bool useNewIp = false;
    private string resServerOriIp = "192.168.1.126";//资源服务器的初始测试地址
    private string resServerNewIp = "192.168.1.27";//不同分包设置的不同资源服务器地址
    private string str;
    private List<object> _updateKeys = new List<object>();
    // Start is called before the first frame update
    void Start()
    {
        componentSetter.gameObject.SetActive(false);
        Addressables.InternalIdTransformFunc = InternalIdTransformFunc;
        UpdateCatalog();
    }
    private void SetInfoDialog(string pInfo,System.Action pOnSure,System.Action pOnCancle) {
        componentSetter.gameObject.SetActive(true);
        componentSetter.SetOneComponentData(new TextData("info",new ComponentInfo() { type = "Text",strValue = pInfo } ));
        componentSetter.SetOneComponentData(new ButtonData("SureBtn", new ComponentInfo()
        {
            acValue = () =>
            {
                pOnSure?.Invoke();
            }
        }));
        componentSetter.SetOneComponentData(new ButtonData("CancleBtn", new ComponentInfo() {
            acValue = () =>
            {
                pOnCancle?.Invoke();
            }  
        }));
    }

    private string InternalIdTransformFunc(UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation location)
    {
        //判定是否是一个AB包的请求
        if (location.Data is AssetBundleRequestOptions)
        {
            //PrimaryKey是AB包的名字
            //path就是StreamingAssets/Bundles/AB包名.bundle,其中Bundles是自定义文件夹名字,发布应用程序时,复制的目录
            string InternalId_ = location.InternalId;
            if (useNewIp && InternalId_.Contains(resServerOriIp))
            {
                //Debug.LogError("InternalId_" + InternalId_);
                InternalId_ = InternalId_.Replace(resServerOriIp, resServerNewIp);
            }
            //Debug.LogError("InternalId_"+ InternalId_);
            return InternalId_;
        }
        else
        {
            string InternalId_ = location.InternalId;
            if (useNewIp && InternalId_.Contains(resServerOriIp))
            {
                //Debug.LogError("InternalId_" + InternalId_);
                InternalId_ = InternalId_.Replace(resServerOriIp,resServerNewIp);
            }
            
            return InternalId_;
        }
    }
    //private void SetRemoteLoadPath()
    //{
    //    string remoteLoadPath = "http://localhost/TapTap";
    //    AddressableAssetSettings m_Settings = AddressableAssetSettingsDefaultObject.Settings;
    //    string profileId = m_Settings.profileSettings.GetProfileId("Dynamic");
    //    m_Settings.profileSettings.SetValue(profileId, AddressableAssetSettings.kRemoteLoadPath, remoteLoadPath);
    //    Debug.Log(string.Format("设置Addressables Groups Profile完成\n{0}:{1}"
    //        , AddressableAssetSettings.kRemoteLoadPath, remoteLoadPath));
    //}

    /// <summary>
    /// 对比更新Catalog
    /// </summary>
    public async void UpdateCatalog()
    {
        str = "";
        var handlew = Addressables.InitializeAsync();
        await handlew.Task;
        //开始连接服务器检查更新
        var handle = Addressables.CheckForCatalogUpdates(false);
        await handle.Task;
        Debug.Log("check catalog status " + handle.Status);
        ShowLog("check catalog status " + handle.Status);
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            List<string> catalogs = handle.Result;
            if (catalogs != null && catalogs.Count > 0)
            {
                //foreach (var catalog in catalogs)
                //{
                //    //Debug.Log("catalog  " + catalog);
                //    //ShowLog("catalog  " + catalog);
                //}
                Debug.Log("download catalog start ");
                //str += "download catalog start \n";
                //outputText.text = str;
                var updateHandle = Addressables.UpdateCatalogs(catalogs, false);
                await updateHandle.Task;
                foreach (var item in updateHandle.Result)
                {
                    //Debug.Log("catalog result " + item.LocatorId);
                    //ShowLog("catalog result " + item.LocatorId);
                    //foreach (var key in item.Keys)
                    //{
                    //    //Debug.Log("catalog key " + key);
                    //    //ShowLog("catalog key " + key);
                    //}
                    _updateKeys.AddRange(item.Keys);
                }
                Debug.Log("download catalog finish " + updateHandle.Status);
                ShowLog("download catalog finish " + updateHandle.Status);
            }
            else
            {
                Debug.Log("dont need update catalogs");
                ShowLog("dont need update catalogs");
            }
        }
        Addressables.Release(handle);
        DownLoad();
    }
    /// <summary>
    /// 主界面显示Log
    /// </summary>
    /// <param name="textStr"></param>
    private void ShowLog(string textStr)
    {
        str += textStr + "\n";
        outputText.text = str;
    }

    public IEnumerator DownAssetImpl()
    {
        var downloadsize = Addressables.GetDownloadSizeAsync(_updateKeys);
        yield return downloadsize;
        Debug.Log("start download size :" + downloadsize.Result);
        ShowLog("start download size :" + downloadsize.Result);
        long size = downloadsize.Result;
        Addressables.Release(downloadsize);
        if (size > 0)
        {
            SetInfoDialog("需要更新资源包：大小为" + size,()=> {
                StartCoroutine(DownAssetImplByKeys(()=> {
                    OnLoadXLuaClient();
                }));
            },()=> {
                Application.Quit();
            });
        }
        else {
            OnLoadXLuaClient();
        }
    }
    public IEnumerator DownAssetImplByKeys(System.Action pFinish) {

        var download = Addressables.DownloadDependenciesAsync(_updateKeys,false);//, Addressables.MergeMode.Union
        yield return download;
        //await download.Task;
        if (download.Result != null)
        {
            //Debug.Log("download result type " + download.Result.GetType());
            //ShowLog("download result type " + download.Result.GetType());
            foreach (var item in download.Result as List<UnityEngine.ResourceManagement.ResourceProviders.IAssetBundleResource>)
            {
                var ab = item.GetAssetBundle();
               // Debug.Log("ab name " + ab.name);
                //ShowLog("ab name " + ab.name);
                foreach (var name in ab.GetAllAssetNames())
                {
                    //Debug.Log("asset name " + name);
                    //ShowLog("asset name " + name);
                }
            }
            Addressables.Release(download);
            pFinish?.Invoke();
        }
        else {
            Addressables.Release(download);
            pFinish?.Invoke();
        }
       
    }

    /// <summary>
    /// 下载资源
    /// </summary>
    public void DownLoad()
    {
        str = "";
        StartCoroutine(DownAssetImpl());
        //CtrlLoadSlider();
    }
 
    public void OnLoadXLuaClient()
    {
        //AddressLoadManager.LoadLuaTxtRes(()=> {
        //    if (XLuaClient.Instance == null)
        //    {
        //        var go = new GameObject("XLuaClient");
        //        go.AddComponent<XLuaClient>();
        //    }
        //    XLuaClient.Instance.StartMain();
        //    OnDownLoadFinish();
        //});
        //if (XLuaClient.Instance == null)
        //{
        //    var go = new GameObject("XLuaClient");
        //    go.AddComponent<XLuaClient>();
        //}
        //else
        //{
        //    XLuaClient.Instance.OnClear();
        //}
        OnDownLoadFinish();
    }
    public void OnDownLoadFinish()
    {
        ShowLog("下载资源完成！！");
        UnityEngine.SceneManagement.SceneManager.LoadScene("2_GameMain");
    }

}
