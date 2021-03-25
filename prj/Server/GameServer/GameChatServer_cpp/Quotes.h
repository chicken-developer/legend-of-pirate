#pragma once
#include <string>
#include <fstream>
#include <vector>

class CQuotes
{
public:
	CQuotes(std::string fileName);
	~CQuotes();
	std::string GetRandomQuotes();


private:
	std::vector<std::string> quotes;


};