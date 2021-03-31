#pragma once
#include <string>
#include <fstream>
#include <vector>

class CGameData
{
public:
	CGameData(std::string fileName);
	~CGameData();
	std::string Update();


private:
	std::vector<std::string> data;

};