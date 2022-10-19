using System.Collections;
using System.Collections.Generic;


namespace ResServer
{
    [System.Serializable]
    public class ResServerData
    {
        /// <summary>
        /// 大版本号请求
        /// </summary>
        [System.Serializable]
        public sealed class ResVerRequest
        {
            public string channelid;
            public string version;
        }
        /// <summary>
        /// 大版本号返回
        /// </summary>
        [System.Serializable]
        public sealed class ResVerResult
        {
            public bool success;
            public string error;
            public int code;
            public ResVerRespond data;

        }
        [System.Serializable]
        public sealed class ResVerRespond
        {
            public string ResVersion;
            public string ClientVersion;
        }

        /// <summary>
        /// 服务器列表请求
        /// </summary>
        [System.Serializable]
        public sealed class SvrlistRequest
        {
            public string channelid;
            public string version;
        }

        /// <summary>
        /// 服务器列表返回
        /// </summary>
        [System.Serializable]
        public sealed class SvrlistResult
        {
            public bool success;
            public string error;
            public int code;
            public SvrlistRespond data;
        }
        [System.Serializable]
        public sealed class SvrlistRespond
        {
            public ChannelPolicyData ChannelData;
            public SvrlistPolicyData ServerlistData;
        }
        [System.Serializable]
        public class ChannelPolicyData
        {
            public string ResVer;
            public string ResourcesUrl;
            public bool ShareFunction = true;
            public bool OpenExchange = true;
            public string ClientVersion;
            public string ClientUrl;
            public string LoginUrl;
        }

        /// <summary>
        /// 服务器列表返回数据
        /// </summary>
        [System.Serializable]
        public class SvrlistPolicyData
        {
            public string DefaultMessageStop;//服务器停服维护信息
            public string DefaultMessageFull;//服务器满员信息
            public DMServerInfo[] ServerInfos;//服务器列表

        }
        [System.Serializable]
        public class DMServerInfo
        {
            public int State;           // 服务器状态 0正常 1停服维护 2 爆满 3 新开
            public string DisplayID;    //显示的服务器ID  
            public string ID;           //正式的服务器ID(没有特殊需求和显示的一样)
            public string ReamID;       //服务器大区ID
            public string Name;         //服务器名称
            public string IpAddress;    //服务器ip
            public string Port;         //服务器端口
            public List<string> WhiteIPList = new List<string>(); //服务器白名单列表(没有配置都能进入 配置了话在列表中的玩家才能进入)
        }

    }
}

