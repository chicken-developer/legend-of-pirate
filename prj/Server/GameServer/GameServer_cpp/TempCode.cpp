#include <iostream>
#include <WS2tcpip.h>

#pragma comment(lib, "ws2_32.lib")

using namespace std;

void temp_main()
{
	//: Initialize win sock
	WSAData wsData;
	WORD ver = MAKEWORD(2, 2);
	int wsOk = WSAStartup(ver, &wsData);
	if(wsOk != 0)
	{
		cerr << "Can't initialize win sock, quitting..." << endl;
		return;
	}

	//: Create a socket
	SOCKET listening = socket(AF_INET, SOCK_STREAM, 0);
	if(listening == INVALID_SOCKET)
	{
		cerr << "Can't create socket, quitting..." << endl;
		return;
	}

	//:Bind an ip address and port to a socket
	sockaddr_in hint;
	hint.sin_family = AF_INET;
	hint.sin_port = htons(54000);
	hint.sin_addr.S_un.S_addr = INADDR_ANY; //Also can use inet_pton...
	bind(listening,(sockaddr*)&hint, sizeof(hint));
		
	//: Tell win sock the socket for listening
	listen(listening, SOMAXCONN);
	cout <<"Listening ..."<< endl;

	//: Wait for connection
	sockaddr_in client;
	int clientSize = sizeof(client);
	SOCKET clientSocket = accept(listening, (sockaddr*)&client, &clientSize);
	char host[NI_MAXHOST]; // Client's remote name 
	char service[NI_MAXHOST]; // Service(ex: port) the client is connect on.
	
	ZeroMemory(host, NI_MAXHOST); // Same as: memset(host, 0, NI_MAXHOST);
	ZeroMemory(service, NI_MAXHOST);
	if(getnameinfo((sockaddr*)&client, sizeof(client), host, NI_MAXHOST, service, NI_MAXSERV, 0)== 0)
	{
		cout << host << " connected on port " << service << endl;
	} 
	else 
	{
		inet_ntop(AF_INET, &client.sin_addr, host, NI_MAXHOST);
		cout << host << " connected on port " << 
				ntohs(client.sin_port) << endl;
	}

	//: Closed listen socket
	closesocket(listening);

	//: Loop accept and echo message back to client
	char buffer[4096];
	while(true) 
	{
		ZeroMemory(buffer, 4096);
		//Wait for client to send data
		int bytesReceive = recv(clientSocket, buffer, 4096, 0);
		if(bytesReceive == SOCKET_ERROR)
		{
			cerr << "Error in recv(). Quitting..." << endl;
			break;
		}

		if(bytesReceive == 0)
		{
			cout << "Client disconnect" << endl;
		}

		//Echo message back to client
		send(clientSocket, buffer, bytesReceive + 1, 0);


	}

	//: Close the socket
	closesocket(clientSocket);

	//: Shutdown win sock
	WSACleanup();
}