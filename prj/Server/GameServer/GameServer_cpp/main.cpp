#include <iostream>
#include <string>

#include "TcpListener.h"
#include "GameData.h"

using namespace std;
CGameData gameData("templateData.txt");

void Listener_MessageReceive(CTcpListener *listener, int client, string msg);

void Listener_MessageReceive(CTcpListener *listener, int client, string msg)
{
	if(msg == "update")
	{
		listener->Send(client, gameData.Update());
	}
}


void main()
{
	CTcpListener server("127.0.0.1", 8003, Listener_MessageReceive);
	if (server.Init())
	{
		server.Run();
	}
}

