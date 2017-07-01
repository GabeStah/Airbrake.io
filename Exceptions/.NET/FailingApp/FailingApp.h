// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the FAILINGAPP_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// FAILINGAPP_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef FAILINGAPP_EXPORTS
#define FAILINGAPP_API __declspec(dllexport)
#else
#define FAILINGAPP_API __declspec(dllimport)
#endif

// This class is exported from the FailingApp.dll
class FAILINGAPP_API CFailingApp {
public:
	CFailingApp(void);
	// TODO: add your methods here.
};

extern FAILINGAPP_API int nFailingApp;

FAILINGAPP_API int fnFailingApp(void);
