using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    // Start is called before the first frame update
    //localhost:8001 : Rails server
    //localhost:8002: Akka server
    //localhost:8003: Cpp server
    private String gameMasterServerAddress = "localhost:8001";
    private String gameChatServerAddress = "localhost:8002";
    private String gameInMatchServerAddress = "localhost:8003";

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
