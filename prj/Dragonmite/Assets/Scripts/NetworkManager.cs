using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    private string _defIP = "127.0.0.1";
    private int _defPort = 54010;
    // private string _defUsername ="";
    // private string _defPassword = "";

    private Button btn_Connect;
    private InputField if_IPAddress;
    private InputField if_Port;


    private TcpClient client;
    byte[] buffer = new byte[49152];
    private string receiveMessage;

    //Get, set
    string getIP()
    {
        if (if_IPAddress.text == null)
        {
            Debug.Log("IP Adress not valid, using default IP: 127.0.0.1");
            if_IPAddress.text = "127.0.0.1";
        }

        return if_IPAddress.text;
    }

    int getPort()
    {
        if (if_Port.text == null)
        {
            Debug.Log("Port not valid, using default Port: 8000");
            if_IPAddress.text = "8000";
        }

        return Convert.ToInt32(if_Port.text);
    }


    //Custom event handle:
    void OnConnectButtonClicked()
    {
        Debug.Log("Connecting...");
        if (!client.Connected)
        {
            Debug.Log("Disconnected...");
            return;
        }
        //Setup async read
        var stream = client.GetStream();
        stream.BeginRead(buffer, 0, buffer.Length, Message_Receive, null);

        //Send message
        receiveMessage = "";
        byte[] msg = Encoding.ASCII.GetBytes("QUOTE");
        stream.Write(msg, 0, msg.Length);

    }

    void Message_Receive(IAsyncResult result)
    {
        if (result.IsCompleted && client.Connected)
        {
            var stream = client.GetStream();
            int bytesIn = stream.EndRead(result);

            receiveMessage = Encoding.ASCII.GetString(buffer, 0, bytesIn);
        }
    }

    void Awake()
    {
        btn_Connect = GameObject.FindGameObjectWithTag("tag_btn_Connect").GetComponent<Button>();
        if_IPAddress = GameObject.FindGameObjectWithTag("tag_if_IPAddress").GetComponent<InputField>();
        if_Port = GameObject.FindGameObjectWithTag("tag_if_Port").GetComponent<InputField>();
    }
    void Start()
    {
        btn_Connect.onClick.AddListener(OnConnectButtonClicked);
        if_IPAddress.text = _defIP;
        if_Port.text = _defPort.ToString();


        client = new TcpClient();
        client.Connect(getIP(), getPort());
        if (client.Connected)
        {
            var stream = client.GetStream();
        }

    }
    void OnDestroy()
    {
        if (client.Connected)
        {
            client.Close();
        }
    }

    void Update()
    {
        //TODO: Update connection here
        if (!string.IsNullOrEmpty(receiveMessage))
        {
            Debug.Log("Message: " + receiveMessage);
            receiveMessage = "";
        }
    }


}