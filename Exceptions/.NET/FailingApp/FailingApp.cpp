// FailingApp.cpp
#include "stdafx.h"
#include "FailingApp.h"
#include <wtypes.h>
#include <unknwn.h>

// Constructor
CFailingApp::CFailingApp()
{
    return;
}

// Creates an invalid reference.
extern "C" __declspec(dllexport) int CreateReference()
{
	IUnknown* pUnk = NULL;
	pUnk->AddRef();
	return 0;
}
