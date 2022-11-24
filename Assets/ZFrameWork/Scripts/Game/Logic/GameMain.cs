using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace ZFrameWork
{
    public class GameMain : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("GameMain >>11");
            GameLogic.Instance.LoadGameDll();
            GameLogic.Instance.RunMain();
            //LoadGameDllGame();
            //RunMainGame();
        }
    }
}

