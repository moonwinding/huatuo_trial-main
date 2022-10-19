using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ResServer
{
    public static class ResServerTools
    {
        private static string _SuffixSvrlist = "svrlist/";
        private static string _SuffixResVer = "resver/";
        private static string _SuffixBulletin = "bulletion/";


        public static string ResServerPath = "http://192.168.97.29:3328/";
        public static string ResVersion = "1.0.0";

        public static string GetChannelId() {
#if UNITY_EDITOR
            return "PC";
#elif UNITY_ANDROID
            return "Android";
#else
            return "iOS";
#endif
        }
        public static string GetResVersion()
        {
            return ResVersion;
        }
        public static void GerServerInfo(System.Action onSuccess,System.Action onFail)
        {
            string url = ResServerPath + _SuffixSvrlist;
            Debug.Log(url);
            ResServerData.SvrlistRequest requst = new ResServerData.SvrlistRequest() {
                channelid = GetChannelId(),
                 version = GetResVersion()
            };
            DownLoadUtility.BasePostFromWWW(url, requst.ToJson(), (www) => {

                if (www == null)
                {
                    onFail?.Invoke();
                }
                else {
                    Debug.Log(www.text);
                    var isSuccess = OnLoadSvrListRespond(www.text);
                    if (isSuccess)
                        onSuccess?.Invoke();
                    else
                        onFail?.Invoke();
                }
            });
        }
        private static bool OnLoadSvrListRespond(string jsonString) {
            ResServerData.SvrlistResult result = new ResServerData.SvrlistResult();
            jsonString.FromJsonOverwrite(result);
            if (result.success)
            {
                ResServerDataRecord.Instacnce.OnResServerListResult(result.data);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void DownLoadOneResByRelatedPath(string relatePath, System.Action<byte[]> callback) {
            var url = ResServerDataRecord.Instacnce.ResServerUrl;
            var fullUrl = (url + relatePath);//.ToFormatPath();
            //fullUrl = "http://192.168.97.29/Ver1/ver.txt";
            Debug.Log("fullUrl is : "+ fullUrl);
           
            DownLoadUtility.DownloadFromWWW(fullUrl, (oResult,oData) => {
                if (oResult == DownLoadUtility.DownLoadResult.Success)
                    callback?.Invoke(oData);
                else
                    callback?.Invoke(null);

            },null);

        }
    }
}

