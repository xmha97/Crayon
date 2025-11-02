// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

#pragma once
#include "ShellHelpers.h"
#include "RegisterExtension.h"
#include <strsafe.h>
#include <new>  // std::nothrow

void DllAddRef();
void DllRelease();

// use UUDIGEN.EXE to generate unique CLSID values for your objects

class __declspec(uuid("09E0D2D4-B928-4DCA-8FDA-69671153BB20")) CExplorerCommandVerb;
class __declspec(uuid("9C094B6E-1420-4752-B7F7-02DB39901204")) CExplorerCommandStateHandler;

HRESULT CExplorerCommandVerb_CreateInstance(REFIID riid, void **ppv);
HRESULT CExplorerCommandStateHandler_CreateInstance(REFIID riid, void **ppv);
HRESULT CExplorerCommandVerb_RegisterUnRegister(bool fRegister);
HRESULT CExplorerCommandStateHandler_RegisterUnRegister(bool fRegister);
