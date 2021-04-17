using System;
using WebSocketSharp;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MultiManager : MonoBehaviour
{
    private CustomNetworkManager wsClient;
    public WebSocket ws;

    [SerializeField] Text roomID;

    [SerializeField] Text playerName;
    [SerializeField] Text enemyName;
    [SerializeField] Text playerLevel;
    [SerializeField] Text enemyLevel;
    [SerializeField] Text playerScore;
    [SerializeField] Text enemyScore;
    public void HandleNetwork()
    {   
        // if(wsClient.ws.IsConnected)
        // {
            string data = "update";
            if (data != "")
            {
                wsClient.SendData("newData");
                Debug.Log("Client send data: " + data);
                var stringResult = wsClient.Ask(data);
                Debug.Log("Client ask data: " + stringResult);
            }

        // }
        // else {
        //     Debug.Log("Disconnected... !");
        // }
    }

    void UpdateData(string newData)
    {
        string[] data = newData.Split(';');  

        playerName.text = data[0];
        playerLevel.text = data[1];
        playerScore.text = data[2];

        enemyName.text = data[3];
        enemyLevel.text = data[4];
        enemyScore.text = data[5];

        roomID.text = data[6];
    }

    string GetCurrentData(){
        string currentData = "";
        currentData = playerName.text + ";" + playerLevel.text + ";" + playerScore.text + ";" + enemyName.text + ";" + enemyLevel.text + ";" + enemyScore.text + ";" + roomID.text;
        return currentData;            
    }
    void InitData()
    {
        playerName.text = "Unknow1";
        playerLevel.text = "0";
        playerScore.text = "0";

        enemyName.text = "Unknow2";
        enemyLevel.text = "0";
        playerScore.text = "0";
        roomID.text = "Unknow3";
    }
    void Start()
    {
        using (var wss = new WebSocket ("ws://localhost:8001")) {
        wss.OnMessage += (sender, e) =>
            Debug.Log ("Laputa says: " + e.Data);

        wss.Connect();
        wss.Send("BALUS");
      }
        Debug.Log("Current Data: "  + GetCurrentData());
        Debug.Log("Client started");
        InitData();
        // wsClient = new CustomNetworkManager();
        // wsClient.Connect("nguyenmanhquynh");
        //HandleNetwork();

        InitNetwork();
        Debug.Log("Network data: " + Ask("data"));

        
    }
    public void SendData(string message)
        {
            ws.Send("data");
            ws.OnMessage += (sender, e) => {
                Debug.Log("Message received from " + ((WebSocket)sender).Url + " is: " + e.Data);
            };
    }

    public string Ask(string message)
        {
            string data = "";
            ws.Send("data");
            ws.OnMessage += (sender, e) => {
                Debug.Log("Message received from " + ((WebSocket)sender).Url + " is: " + e.Data);
                data =  e.Data;
            };
            return data;
    }
    void InitNetwork()
    {
         ws = new WebSocket("wss://localhost:8001");
            ws.Connect();
            ws.OnMessage += (sender, e) => {
                Debug.Log("Message received from " + ((WebSocket)sender).Url + " is: " + e.Data);
            };
    }

    // Update is called once per frame
    void Update()
    {
        //HandleNetwork();
    }
}
