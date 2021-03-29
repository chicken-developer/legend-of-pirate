#include <iostream>
#include <string>

#include "TcpListener.h"
#include "Quotes.h"

using namespace std;
CQuotes quotes("quotes.txt");

void Listener_MessageReceive(CTcpListener *listener, int client, string msg);

void Listener_MessageReceive(CTcpListener *listener, int client, string msg)
{
	if(msg == "QUOTE")
	{
		listener->Send(client, quotes.GetRandomQuotes());
	}
}


void main()
{
	CTcpListener server("127.0.0.1", 54010, Listener_MessageReceive);
	if (server.Init())
	{
		server.Run();
	}
}

