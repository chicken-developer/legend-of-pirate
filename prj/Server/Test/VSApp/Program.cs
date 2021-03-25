using System;
using WebSocketSharp;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class Setup
    {
        public WebSocket ws;
        public void SendData(string message)
        {
            ws.Send("data");
            ws.OnMessage += (sender, e) => {
                Console.WriteLine("Message received from " + ((WebSocket)sender).Url + " is: " + e.Data);
            };
        }
        public void Connect()
        {
            ws = new WebSocket("wss://localhost:8000");
            ws.OnMessage += (sender, e) => {
                Console.WriteLine("Message received from " + ((WebSocket)sender).Url + " is: " + e.Data);
            };
            ws.Connect();
        }

    }
    class Program
    {
        private static Setup wsClient_root;
        private static List<Setup> wsClient_multi;
        static void Main(string[] args)
        {

            var numberOfClient = 19;
            wsClient_multi = new List<Setup>();
            Console.WriteLine("Client started");

            wsClient_root = new Setup();
            wsClient_multi.Add(wsClient_root);
            wsClient_root.Connect();
            while(wsClient_root.ws.IsConnected){
                string data ="run";
                if(data != ""){
                    wsClient_root.SendData(data);
                    Console.WriteLine("Client send data: " + data);
                }    

            }
            // for (int i = 0; i < numberOfClient; i++)
            // {
            //     Setup client = new Setup();
            //     wsClient_multi.Add(client);
            // }
            // for (int i = 0; i < numberOfClient; i++)
            // {
            //     wsClient_multi[i].Connect();
            //     Console.WriteLine("Client : " + i + " connected to server");
            // }
            // Console.WriteLine("All client connected");

            // while (wsClient_root.ws.IsConnected)
            // {
            //     string data = Console.ReadLine();
            //     if (data != "")
            //     {
            //         for (int i = 0; i < numberOfClient; i++)
            //         {
            //             wsClient_multi[i].SendData(data);
            //             Console.WriteLine("Client : " + i + " send data");

            //         }
            //         Console.WriteLine("All client send data: " + data);
            //     }

            // }
            Console.WriteLine("Client exit!");
        }
    }

}
