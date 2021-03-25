#include "Quotes.h"

CQuotes::CQuotes(std::string fileName)
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
				quotes.push_back(running);
				running = "";
			}
		}
	}
}

CQuotes::~CQuotes()
{
}

std::string CQuotes::GetRandomQuotes()
{
	int r = rand() % quotes.size();
	return quotes[r];
}
