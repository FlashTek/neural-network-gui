// NNCreator.cpp : Definiert den Einstiegspunkt f�r die Konsolenanwendung.
//

#include "stdafx.h"
#include "../3rd/tinyxml2/tinyxml2.h"
#include "xmltags.h"
#include "NetworkArchitecture.h"
#include <iostream>

using namespace std;

int main()
{
	tinyxml2::XMLDocument doc;
	
	doc.LoadFile("C:\\Users\\Roland\\Documents\\Visual Studio 2017\\Projects\\neural-network-gui\\NNGui\\bin\\Debug\\network.xml");
	if (doc.Error() == tinyxml2::XML_SUCCESS)
	{
		tinyxml2::XMLElement *pRoot = doc.FirstChildElement(XML_TAG_NETWORK_ARCHITECTURE);
		if (pRoot)
		{
			auto networkArchitecture = CNetworkArchitecture::getInstance(pRoot);
			cout << networkArchitecture;
		}
	}
    return 0;
}

