using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameInMatchServer : MonoBehaviour
{
    private string messageSend = "update";
    private string finalMessage;
    struct Profile
    {
        private Text userName;
        private Text level;
        private Text currentPoint;
    }

    private Profile player;
    private Profile enemy;

    private TcpClient client;
    byte[] buffer = new byte[49152];

    void Message_Received(IAsyncResult result)
    {
        if (result.IsCompleted && client.Connected)
        {
            var stream = client.GetStream();
            int bytesIn = stream.EndRead(result);
            finalMessage = Encoding.ASCII.GetString(buffer, 0, bytesIn);
        }
    }
    void UpdateRequest()
    {
        if (!client.Connected)
        {
            return;
        }
        finalMessage = "";
        //Setup async read
        var stream = client.GetStream();
        stream.BeginRead(buffer, 0, buffer.Length, Message_Received, null);

        //Send message
        byte[] msg = Encoding.ASCII.GetBytes(messageSend);
        stream.Write(msg, 0, msg.Length);
    }

    void Awake()
    {
        //TODO: Init profile for player and enemy
    }
    void Start()
    {
        //TODO: Get from server name and level, set current point to zero

        client = new TcpClient();
        client.Connect("127.0.0.1", 8003);
    }
    private void OnDestroy()
    {
        if (client.Connected)
        {
            client.Close();
        }
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(finalMessage))
        {
            //TODO: Update score, win, lost
        }
    }
}