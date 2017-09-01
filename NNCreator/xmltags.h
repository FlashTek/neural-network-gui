#pragma once

#define XML_TAG_NETWORK_ARCHITECTURE "NetworkArchitecture"
#define XML_TAG_Chains "Chains"
#define XML_TAG_Chain "Chain"
#define XML_TAG_ChainLinks "ChainLinks"
#define XML_TAG_LinkBase "LinkBase"
#define XML_TAG_Parameters "Parameters"
#define XML_TAG_Value "Value"
#define XML_ATTRIBUTE_Name "Name"
#define XML_ATTRIBUTE_Id "ID"
#define XML_TAG_Id "ID"
#define XML_TAG_ParameterBase "ParameterBase"

#define XML_TAG_Tuple_1 "x1"
#define XML_TAG_Tuple_2 "x2"
#define XML_TAG_Tuple_3 "x3"
#define XML_TAG_Tuple_4 "x4"

#define XML_TAG_LinkConnection "LinkConnection"

#define XML_ATTRIBUTE_TargetId "TargetID"

#define XML_ATTRIBUTE_ActivationFunctionParameter "ActivationFunctionParameter"
#define XML_ATTRIBUTE_DoubleParameter "DoubleParameter"
#define XML_ATTRIBUTE_InputDataParameter "InputDataParameter"
#define XML_ATTRIBUTE_IntParameter "IntParameter"
#define XML_ATTRIBUTE_IntTuple1DParameter "IntTuple1DParameter"
#define XML_ATTRIBUTE_IntTuple2DParameter "IntTuple2DParameter"
#define XML_ATTRIBUTE_IntTuple3DParameter "IntTuple3DParameter"
#define XML_ATTRIBUTE_IntTuple4DParameter "IntTuple4DParameter"
#define XML_ATTRIBUTE_LinkConnectionListParameter "LinkConnectionListParameter"
#define XML_ATTRIBUTE_Type "xsi:type"
//#define XML_ATTRIBUTE_ ""
//#define XML_TAG_ ""

#include <vector>

template <typename T>
void loadChildren(tinyxml2::XMLElement* pParentNode, const char* xmlTag, std::vector<T*> &outVec)
{
	tinyxml2::XMLElement *pNode = pParentNode->FirstChildElement(xmlTag);
	while (pNode)
	{
		T* item = T::getInstance(pNode);
		if (item)
			outVec.push_back(item);
		pNode = pNode->NextSiblingElement(xmlTag);
	}
}
