using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class TestPeerSend : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendRequest();
        }
    }
    void SendRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add(1,100);
        data.Add(2, "ereaddtex2122");
        PhotonEngine.Peer.OpCustom(1,data,true);
    }
}
