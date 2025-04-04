﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//---------------------------------------------------------------------------
//
//
// 
//
// Description: Set of helpers used by all Commands. 
//
//  
//
//
//---------------------------------------------------------------------------

using System.Windows.Input;



namespace MS.Internal
{
    internal static class CommandLibraryHelper
    {
        internal static RoutedUICommand CreateUICommand(string name, Type ownerType, byte commandId)
        {
            RoutedUICommand routedUICommand = new RoutedUICommand(name, ownerType, commandId)
            {
                AreInputGesturesDelayLoaded = true
            };
            return routedUICommand;
        }                        
     }
}     
