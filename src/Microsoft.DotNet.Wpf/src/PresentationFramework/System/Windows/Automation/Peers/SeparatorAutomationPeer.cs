// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
    /// 
    public class SeparatorAutomationPeer : FrameworkElementAutomationPeer
    {
        ///
        public SeparatorAutomationPeer(Separator owner): base(owner)
        {}

        ///
        protected override string GetClassNameCore()
        {
            return "Separator";
        }
    
        ///
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Separator;
        }

        // AutomationControlType.Separator must return IsContentElement false.
        // See http://msdn.microsoft.com/en-us/library/ms750550.aspx
        protected override bool IsContentElementCore()
        {
            return false;
        }
    }
}



