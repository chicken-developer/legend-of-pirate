using System;
using WebSocketSharp;
using UnityEngine;

public class CustomNetworkManager {
    
        public WebSocket ws;
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
        public void Connect(string playerNameStr)
        {
            ws = new WebSocket("wss://localhost:8001");
            ws.Connect();
            ws.OnMessage += (sender, e) => {
                Debug.Log("Message received from " + ((WebSocket)sender).Url + " is: " + e.Data);
            };
            
        }

}