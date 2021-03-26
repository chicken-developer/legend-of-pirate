using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ScoreNetwork : MonoBehaviour
{
    private Text text;
    private Button button;

    private string _defIP = "127.0.0.1";
    private int _defPort = 54010;
    private string finalMessage;
    private string messageSend = "QUOTE";



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
    void btnGetQuotesClicked()
    {
        if (!client.Connected)
        {
            return;
        }
        button.interactable = false;
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
        text = GameObject.FindGameObjectWithTag("tag_Text").GetComponent<Text>();
        button = GameObject.FindGameObjectWithTag("tag_btn_GetQuotes").GetComponent<Button>();
    }
    void Start()
    {
        text.text = "No Quotes";
        button.onClick.AddListener(btnGetQuotesClicked);
        client = new TcpClient();
        client.Connect(_defIP, _defPort);
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
            text.text = finalMessage;
            finalMessage = "";
            button.interactable = true;
        }
    }
}