using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

public class ReqData
{
    public byte code;
    public Dictionary<byte, object> data;
    public System.Action<Dictionary<byte, object>> callback;
}
public class PhotonEngine : MonoBehaviour,IPhotonPeerListener
{
    public static PhotonPeer Peer { get { return peer; } }
    private static PhotonEngine Instance;
    private static PhotonPeer peer;
    private static Dictionary<int, ReqData> ResMap = new Dictionary<int, ReqData>();
    public static void SendReq(Common.OperationCode pCode, Dictionary<byte, object> pData,System.Action<Dictionary<byte, object>> pCB)
    {
        int code = (int)pCode;
        if (ResMap.ContainsKey(code) && ResMap[code] != null)
        {
            
        }
        else
        {
            ReqData reqData = new ReqData() { code  = (byte)pCode , data  = pData, callback = pCB };
            if (ResMap.ContainsKey(code))
            {
                ResMap[code] = reqData;
            }
            else
            {
                ResMap.Add(code, reqData);
            }
            Peer.OpCustom((byte)pCode, pData, true);
        }

       
    }
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        int code = operationResponse.OperationCode;
        if (ResMap.ContainsKey(code))
        {
            ReqData reqData = ResMap[code];
            if (reqData.callback != null)
            {
                reqData.callback.Invoke(operationResponse.Parameters);
            }
            ResMap[code] = null;
        }
        //switch (operationResponse.OperationCode)
        //{
        //    case 1:
        //        Debug.Log("OnOperationResponse !!!");
        //        Dictionary<byte, object> data = operationResponse.Parameters;
        //        object value1;
        //        data.TryGetValue(1, out value1);
        //        object value2;
        //        data.TryGetValue(2, out value2);
        //        Debug.Log("OnOperationResponse Parameters is :" + value1.ToString() + value2.ToString());
        //        break;
        //    default:
        //        break;
        //}
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            GameObject.Destroy(this.gameObject);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        peer = new PhotonPeer(this,ConnectionProtocol.Udp);
        peer.Connect("127.0.0.1:5055","MyGame1");
    }
    void Update()
    {
        peer.Service();  
    }
    void OnDestory()
    {
        if (peer != null && peer.PeerState == PeerStateValue.Connected)
        {
            peer.Disconnect();
        }
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnEvent(EventData eventData)
    {
        switch (eventData.Code)
        {
            case 1:
                Debug.Log("OnEvent !!! ");
                Dictionary<byte, object> data = eventData.Parameters;
                object value1;
                data.TryGetValue(1, out value1);
                object value2;
                data.TryGetValue(2, out value2);
                Debug.Log("OnEvent Parameters is :" + value1.ToString() + value2.ToString());
                break;
            default:

                break;
        }
    }

   

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log("OnStatusChanged is : " + statusCode) ;
    }
}
