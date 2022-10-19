using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class DownLoadUtility
{
    //下载的结果
    public enum DownLoadResult
    {
        Success =0,
        Error = 1,
    }
    public static int  MaxTryCount() {return 3;}
    public static float MaxWaitTime(){return 10;}
    public static void BasePostFromWWW(string url, string postData, System.Action<WWW> onCallBack) {
        GamerOwer.Instance.StartCoroutine(BasePostFromWWW(1, url, postData, onCallBack));
    }
    private static IEnumerator BasePostFromWWW(int tryCount, string url, string postData, System.Action<WWW> onCallBack)
    {
        if (tryCount < MaxTryCount())
        {
            byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(postData);
            WWW getData = new WWW(url, bytes);
            bool timeOut = false;
            float waitingTime = 0f;
            float progress = 0f;
            while (!getData.isDone && getData.error == null && !timeOut)
            {
                if (getData.progress == progress)
                    waitingTime += Time.deltaTime;
                else
                {
                    progress = getData.progress;
                    waitingTime = 0f;
                }

                if (waitingTime >= MaxWaitTime())
                {
                    timeOut = true;
                    break;
                }
                yield return 0;
            }
            if (getData.error != null || timeOut)
            {
                Debug.LogError(getData.error);
                GamerOwer.Instance.StartCoroutine(BasePostFromWWW(tryCount + 1, url, postData, onCallBack));
            }
            else
            {
                onCallBack?.Invoke(getData);
                getData.Dispose();
            }
        }
        else
        {
            Debug.LogError("try too much times but Download faied!! the url is :"+ url);
            onCallBack?.Invoke(null);
        }
    }

    public static void DownloadFromHttp(string url, System.Action<DownLoadResult, byte[]> onCallBack,System.Action<int> onTimeOut
        , System.Action<float> onProgress = null,int tryCount = 1) {

        GamerOwer.Instance.StartCoroutine(DownLoadFromHttpAsync(tryCount, url, onCallBack, onTimeOut, onProgress));
    }
    public static void DownloadFromWWW(string url, System.Action<DownLoadResult, byte[]> onCallBack, System.Action<int> onTimeOut
    , System.Action<float> onProgress = null, int tryCount = 1)
    {

        GamerOwer.Instance.StartCoroutine(DownLoadFromWWW(tryCount, url, onCallBack, onTimeOut, onProgress));
    }
    public static IEnumerator DownLoadFromWWW(int tryCount, string url, System.Action<DownLoadResult, byte[]> onCallBack, System.Action<int> onTimeOut, System.Action<float> onProgress)
    {
        if (tryCount > MaxTryCount())
        {
            onCallBack?.Invoke(DownLoadResult.Error, null);
        }
        else
        {
            var getData = new WWW(url);
            bool timeOut = false;
            float waitingTime = 0f;
            float progress = 0f;
            while (!getData.isDone && getData.error == null && !timeOut)
            {
                if (getData.progress == progress)
                    waitingTime += Time.deltaTime;
                else
                {
                    progress = getData.progress;
                    waitingTime = 0f;
                }

                if (waitingTime >= MaxWaitTime())
                {
                    timeOut = true;
                    break;
                }
                yield return 0;
            }
            if (getData.error != null || timeOut)
            {
                Debug.LogError(getData.error);
                GamerOwer.Instance.StartCoroutine(DownLoadFromWWW(tryCount + 1, url, onCallBack, onTimeOut,onProgress));
            }
            else
            {
                onCallBack?.Invoke( DownLoadResult.Success, getData.bytes);
                getData.Dispose();
            }
        }
    }
    public static IEnumerator DownLoadFromHttpAsync(int tryCount ,string url, System.Action<DownLoadResult, byte[]> onCallBack, System.Action<int> onTimeOut, System.Action<float> onProgress) {
        if (tryCount > MaxTryCount())
        {
            onCallBack?.Invoke(DownLoadResult.Error, null);
        }
        else
        {
            var request =  UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
                GamerOwer.Instance.StartCoroutine(DownLoadFromHttpAsync(tryCount + 1, url, onCallBack, onTimeOut, onProgress));
            }
            else
            {
                onCallBack?.Invoke(DownLoadResult.Success, request.downloadHandler.data);
            }
        }
    }
}
