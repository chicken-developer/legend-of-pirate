#pragma once
#include <string>
#include <WS2tcpip.h>

#define MAX_BUFFER_SIZE (49152)

#pragma comment(lib, "ws2_32.lib")
class CTcpListener;

typedef void(*MessageReceiveHandler)(CTcpListener* listener, int socketId, std::string msg);

class CTcpListener
{
public:
	CTcpListener(std::string ipAddress, int port, MessageReceiveHandler handler);
	~CTcpListener();

	//Initialize win sock
	bool Init();

	//Send a message to special client
	void Send(int clientSocket, std::string msg);

	//The main process lopp
	void Run();

	//Cleanup
	void Cleanup();

	//Receive loop

	//Send back message


private:
	std::string				m_ipAddress;
	int						m_port;
	MessageReceiveHandler	MessageReceived;

	//Create a socket
	SOCKET CreateSocket();

	//Wait for connection
	SOCKET WaitForConnection(SOCKET listening);


};
