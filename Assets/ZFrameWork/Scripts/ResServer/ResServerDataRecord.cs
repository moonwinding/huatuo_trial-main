using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResServer
{
    public class ResServerDataRecord
    {
        private static ResServerDataRecord instance;
        public static ResServerDataRecord Instacnce { get { 
                if(instance == null)
                    instance = new ResServerDataRecord();
                return instance;
            }}

        public ResServerData.ChannelPolicyData ChannelData { get; private set; }
        public ResServerData.SvrlistPolicyData SvrlistData { get; private set; }
        public string ResServerUrl;
        public void OnResServerListResult(ResServerData.SvrlistRespond respond)
        {
            ResServerUrl = respond.ChannelData.ResourcesUrl;
            ChannelData = respond.ChannelData;
            SvrlistData = respond.ServerlistData;
        }
    }
}
 
