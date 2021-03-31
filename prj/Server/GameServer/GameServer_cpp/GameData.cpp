#include "GameData.h"

CGameData::CGameData(std::string fileName)
{
	std::ifstream file;
	file.open(fileName);
	if(file.is_open())
	{
		std::string line;
		std::string running = "";
		while (getline(file, line))
		{
			if(line != "%")
			{
				running = running + line + "\n";
			}
			else 
			{
				data.push_back(running);
				running = "";
			}
		}
	}
}

CGameData::~CGameData()
{
}

std::string CGameData::Update()
{
	
}
