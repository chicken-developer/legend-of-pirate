#include "TcpListener.h"

#include <iostream>
using namespace std;
//Initialize win sock
CTcpListener::CTcpListener(std::string ipAddress, int port, MessageReceiveHandler handler)
	: m_ipAddress(ipAddress), m_port(port), MessageReceived(handler)
{
	
}

CTcpListener::~CTcpListener()
{
	Cleanup();
}


bool CTcpListener::Init()
{
	WSAData data;
	WORD ver = MAKEWORD(2, 2); // Need learn about this
	
	int wsInit = WSAStartup(ver, &data);
	//TODO: Inform caller the error 
	return wsInit == 0;
}

//Send a message to special client
void CTcpListener::Send(int clientSocket, std::string msg)
{
	send(clientSocket, msg.c_str(), msg.size() + 1, 0);
	cout << "Server have sent: " << msg << "\n";
}

//The main process lopp
void CTcpListener::Run()
{
	char buffer[MAX_BUFFER_SIZE];
	while(true) 
	{
		//Creating a listening socket
		SOCKET listening = CreateSocket();
		if(listening == INVALID_SOCKET)
		{
			break;
		}

		//Wait for connection
		SOCKET client = WaitForConnection(listening);
		cout << "Waiting for connection \n";
		//Loop
		if(client != INVALID_SOCKET)
		{
			closesocket(listening);
			int bytesReceive = 0;
			do
			{
				ZeroMemory(buffer, MAX_BUFFER_SIZE);
				bytesReceive = recv(client, buffer, MAX_BUFFER_SIZE, 0);
				if(bytesReceive > 0)
				{
					if(MessageReceived != NULL)
					{
						MessageReceived(this, client, string(buffer, 0, bytesReceive));
					}
				}
			}
			while (bytesReceive > 0);
			closesocket(client);
		}
		
	}
}

//Cleanup
void CTcpListener::Cleanup()
{
	WSACleanup();
}

//Create socket
SOCKET CTcpListener::CreateSocket()
{
	SOCKET listener = socket(AF_INET, SOCK_STREAM, 0);
	if(listener != INVALID_SOCKET)
	{
		sockaddr_in hint;
		hint.sin_family = AF_INET;
		hint.sin_port = htons(m_port);
		inet_pton(AF_INET, m_ipAddress.c_str(), &hint.sin_addr);

		int bindOK = bind(listener, (sockaddr*)&hint, sizeof(hint));
		if(bindOK != SOCKET_ERROR)
		{
			int listenOK = listen(listener, SOMAXCONN);
			if(listenOK == SOCKET_ERROR)
			{
				return -1;
			}
		}
		else
		{
			return -1;
		}
	}
	cout << m_ipAddress << " connected on port " << m_port << endl;

	return listener;
}

//Wait for connection
SOCKET CTcpListener::WaitForConnection(SOCKET listening )
{
	SOCKET client = accept(listening, NULL, NULL);
	return client;
}

